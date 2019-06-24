// ===================================================
//  Copyright 2018 Intergraph Corp.
//  File Name: AssetHistoryModel.cs
// 
//  Description: AssetHistoryModel is used to as a base model for the asset history command.
//
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  09/01/2018          Sithara                     
// ======================================================

using System;
using Intergraph.GTechnology.API;
using System.Data;
using ADODB;
using System.Linq;

namespace GTechnology.Oncor.CustomAPI
{
  public class AssetHistoryModel
  {

    public IGTApplication m_Application { get; set; }
    public IGTDataContext m_DataContext { get; set; }
    public IGTSelectedObjects m_SelectedObjects { get; set; }
    public IGTKeyObject m_KeyObject { get; set; }

    public int m_ReplaceAno { get; set; }
    public string m_WRID { get; set; }

    public string m_StructureID { get; set; }
    public bool m_isStructure { get; set; }
    public string m_NamedView { get; set; }

    public int m_FID { get; set; }

    private DataTable m_dataTable;
    public DataTable m_GridDataTable
    {
      get
      {
        return m_dataTable;
      }
      set
      {
        m_dataTable = value;
      }
    }

    #region Datagridview Columns Filters
    /// <summary>
    /// Below variables are used to show the filters on datagridview columns header.
    /// </summary>
    public string[] m_UserData
    {
      get;
      set;
    }

    public string[] m_OperationData
    {
      get;
      set;
    }

    public string[] m_FeatureClassData
    {
      get;
      set;
    }

    public string[] m_ComponentData
    {
      get;
      set;
    }

    public string[] m_AttributeData
    {
      get;
      set;
    }

    public string[] m_OldValueData
    {
      get;
      set;
    }

    public string[] m_NewValueData
    {
      get;
      set;
    }

    public string[] m_DesignerData
    {
      get;
      set;
    }

    #endregion


    /// <summary>
    /// Datagridview column Structure
    /// </summary>
    public void BaseTableStructure()
    {
      m_dataTable = new DataTable();

      m_dataTable.Columns.Add("Date Posted", typeof(DateTime));
      m_dataTable.Columns.Add("User", typeof(string));
      m_dataTable.Columns.Add("Operation", typeof(string));
      m_dataTable.Columns.Add("Feature Class", typeof(string));
      m_dataTable.Columns.Add("FID", typeof(int));
      m_dataTable.Columns.Add("Structure ID", typeof(string));
      m_dataTable.Columns.Add("Component", typeof(string));
      m_dataTable.Columns.Add("Attribute", typeof(string));
      m_dataTable.Columns.Add("Old Value", typeof(string));
      m_dataTable.Columns.Add("New Value", typeof(string));
      m_dataTable.Columns.Add("WR/Job", typeof(string));
      m_dataTable.Columns.Add("Description", typeof(string));
      m_dataTable.Columns.Add("Designer", typeof(string));

    }


    /// <summary>
    /// LoadGridData : Loads asset history data.
    /// </summary>
    /// <returns></returns>
    public DataTable LoadGridData()
    {
      try
      {
        BaseTableStructure();
        if(m_GridDataTable != null)
        {
          GetDataInTable();
          LoadFiltersData();
        }
      }
      catch
      {
        throw;
      }

      return m_GridDataTable;
    }

    /// <summary>
    /// LoadAssetHistoryNamedViews is used to load combo box of the history form.
    /// </summary>
    /// <returns></returns>
    internal Recordset LoadAssetHistoryNamedViews()
    {
      Recordset rs;
      try
      {
        string sql = "select * from asset_history_view order by view_id";
        rs = m_DataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText,
                null);

        if(rs != null && rs.RecordCount > 0)
        {
          return rs;
        }
      }
      catch
      {
        throw;
      }
      finally
      {
        rs = null;
      }
      return null;
    }


    /// <summary>
    /// GetdataIntotheTable : 
    /// This is recursive function if the results show a value in the Common Attributes / Replaced FID attribute, 
    /// then history for that Feature ID will also be queried and appended to the original query results.  
    /// This behavior applies to all subsequent queries as well, so that the full history of the feature is constructed regardless of how many replacements occurred.
    /// </summary>
    private void GetDataInTable()
    {
      Recordset rs = null;
      try
      {
        string strFeatureClass = "";
        string strComponentClass = "";
        string strAttributeClass = "";
        string strJobDescription = "";
        string strDesignerUid = "";
        string WR_NBR = "";
        DateTime? dateTime = new DateTime();
        string[] strJobDetails = new string[3];
        rs = GetAssetHistoryRecordSet();
        if(rs != null && rs.RecordCount > 0)
        {
          rs.MoveFirst();
          while(!rs.EOF)
          {
            strFeatureClass = GetFeatureName(Convert.ToInt16(rs.Fields["G3E_FNO"].Value));
            strComponentClass = GetComponentName(Convert.ToInt16(rs.Fields["G3E_CNO"].Value));
            strAttributeClass = DBNull.Value != rs.Fields["G3E_ANO"].Value ? GetAttributeName(Convert.ToInt32(rs.Fields["G3E_ANO"].Value)) : string.Empty;
            strJobDetails = GetJobDescription(Convert.ToString(rs.Fields["G3E_IDENTIFIER"].Value));
            strJobDescription = strJobDetails[0];
            strDesignerUid = strJobDetails[1];
            WR_NBR = Convert.ToString(rs.Fields["G3E_IDENTIFIER"].Value);
            dateTime = DBNull.Value != rs.Fields["CHANGE_DATE"].Value ? Convert.ToDateTime(rs.Fields["CHANGE_DATE"].Value) : Convert.ToDateTime(null);
            m_GridDataTable.Rows.Add(dateTime,
            Convert.ToString(rs.Fields["CHANGE_UID"].Value),
                                     Convert.ToString(rs.Fields["CHANGE_OPERATION"].Value),
                                     strFeatureClass,
                                     Convert.ToInt32(rs.Fields["G3E_FID"].Value),
                                     Convert.ToString(rs.Fields["STRUCTURE_ID_1"].Value),
                                     strComponentClass,
                                     strAttributeClass,
                                     Convert.ToString(rs.Fields["VALUE_OLD"].Value),
                                     Convert.ToString(rs.Fields["VALUE_NEW"].Value),
                                     WR_NBR,
                                     strJobDescription,
                                     strDesignerUid);

            rs.MoveNext();
          }


          if(m_FID != 0 && !m_isStructure)
          {
            rs.MoveFirst();
            while(!rs.EOF)
            {
              if(rs.Fields["G3E_ANO"].Value.GetType() != typeof(DBNull)) //Handled for the cases of Delete and Create
              {
                if(Convert.ToInt32(rs.Fields["G3E_ANO"].Value) == m_ReplaceAno)
                {
                  m_FID = Convert.ToInt32(rs.Fields["VALUE_NEW"].Value);
                  GetDataInTable();
                }
              }
              rs.MoveNext();
            }
          }
        }
      }
      catch
      {
        throw;
      }
      finally
      {
        if(rs != null)
        {
          rs.Close();
          rs = null;
        }
      }
    }


    /// <summary>
    /// LoadFiltersData : This is used to load filters data on the datagridview columns header.
    /// </summary>
    private void LoadFiltersData()
    {
      int count = 0;
      try
      {
        if(m_GridDataTable.Rows.Count >= 1)
        {
          m_UserData = new string[m_GridDataTable.Rows.Count];
          m_OperationData = new string[m_GridDataTable.Rows.Count];
          m_FeatureClassData = new string[m_GridDataTable.Rows.Count];
          m_ComponentData = new string[m_GridDataTable.Rows.Count];
          m_AttributeData = new string[m_GridDataTable.Rows.Count];
          m_OldValueData = new string[m_GridDataTable.Rows.Count];
          m_NewValueData = new string[m_GridDataTable.Rows.Count];
          m_DesignerData = new string[m_GridDataTable.Rows.Count];

          foreach(DataRow dr in m_GridDataTable.Rows)
          {
            if(!Array.Exists(m_UserData, element => element == Convert.ToString(dr["User"])))
            {
              m_UserData[count] = Convert.ToString(dr["User"]);
            }
            if(!Array.Exists(m_OperationData, element => element == Convert.ToString(dr["Operation"])))
            {
              m_OperationData[count] = Convert.ToString(dr["Operation"]);
            }
            if(!Array.Exists(m_FeatureClassData, element => element == Convert.ToString(dr["Feature Class"])))
            {
              m_FeatureClassData[count] = Convert.ToString(dr["Feature Class"]);
            }
            if(!Array.Exists(m_ComponentData, element => element == Convert.ToString(dr["Component"])))
            {
              m_ComponentData[count] = Convert.ToString(dr["Component"]);
            }
            if(!Array.Exists(m_AttributeData, element => element == Convert.ToString(dr["Attribute"])))
            {
              m_AttributeData[count] = Convert.ToString(dr["Attribute"]);
            }
            if(!Array.Exists(m_OldValueData, element => element == Convert.ToString(dr["Old Value"])))
            {
              m_OldValueData[count] = Convert.ToString(dr["Old Value"]);
            }
            if(!Array.Exists(m_NewValueData, element => element == Convert.ToString(dr["New Value"])))
            {
              m_NewValueData[count] = Convert.ToString(dr["New Value"]);
            }

            if(!Array.Exists(m_DesignerData, element => element == Convert.ToString(dr["Designer"])))
            {
              m_DesignerData[count] = Convert.ToString(dr["Designer"]);
            }

            count++;
          }

          m_UserData = m_UserData.Where(x => !string.IsNullOrEmpty(x)).ToArray();
          m_OperationData = m_OperationData.Where(x => !string.IsNullOrEmpty(x)).ToArray();
          m_FeatureClassData = m_FeatureClassData.Where(x => !string.IsNullOrEmpty(x)).ToArray();
          m_ComponentData = m_ComponentData.Where(x => !string.IsNullOrEmpty(x)).ToArray();
          m_AttributeData = m_AttributeData.Where(x => !string.IsNullOrEmpty(x)).ToArray();
          m_OldValueData = m_OldValueData.Where(x => !string.IsNullOrEmpty(x)).ToArray();
          m_NewValueData = m_NewValueData.Where(x => !string.IsNullOrEmpty(x)).ToArray();
          m_DesignerData = m_DesignerData.Where(x => !string.IsNullOrEmpty(x)).ToArray();
        }
      }
      catch
      {
        throw;
      }
    }


    /// <summary>
    /// Getting recordset of the asset history.
    /// </summary>
    /// <returns></returns>
    private Recordset GetAssetHistoryRecordSet()
    {
      Recordset rsAssetHistory = null;
      try
      {
        string sql = BuildQuery();
        if(!string.IsNullOrEmpty(sql))
        {
          if(!string.IsNullOrEmpty(m_StructureID) && m_isStructure)
          {
            rsAssetHistory = m_DataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText,
            new object[] { m_StructureID });
          }
          else if(m_FID != 0 && !m_isStructure)
          {
            rsAssetHistory = m_DataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText,
            new object[] { m_FID });
          }
          else if(!string.IsNullOrEmpty(m_WRID) && !m_isStructure)
          {
            rsAssetHistory = m_DataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText,
            new object[] { m_WRID });
          }
        }
      }
      catch
      {
        throw;
      }

      return rsAssetHistory;
    }

    /// <summary>
    /// Getting query of asset history.
    /// </summary>
    /// <returns></returns>
    private string BuildQuery()
    {
      string sql = "";
      try
      {
        if(!string.IsNullOrEmpty(m_StructureID) && m_isStructure)
        {
          sql = "SELECT CHANGE_DATE,CHANGE_UID,CHANGE_OPERATION,G3E_FID,G3E_FNO,STRUCTURE_ID_1,STRUCTURE_ID_2,VALUE_OLD,VALUE_NEW,G3E_IDENTIFIER,G3E_CNO,G3E_ANO FROM asset_history WHERE structure_id_1 = :1 OR structure_id_2 = :1";
        }
        else if(m_FID != 0 && !m_isStructure)
        {
          sql = "SELECT CHANGE_DATE,CHANGE_UID,CHANGE_OPERATION,G3E_FID,G3E_FNO,STRUCTURE_ID_1,STRUCTURE_ID_2,VALUE_OLD,VALUE_NEW,G3E_IDENTIFIER,G3E_CNO,G3E_ANO FROM asset_history WHERE G3E_FID=:1";
        }
        else if(!string.IsNullOrEmpty(m_WRID) && !m_isStructure)
        {
          sql = "SELECT CHANGE_DATE,CHANGE_UID,CHANGE_OPERATION,G3E_FID,G3E_FNO,STRUCTURE_ID_1,STRUCTURE_ID_2,VALUE_OLD,VALUE_NEW,G3E_IDENTIFIER,G3E_CNO,G3E_ANO FROM asset_history WHERE G3E_IDENTIFIER=:1";
        }
      }
      catch
      {
        throw;
      }
      return sql;
    }

    /// <summary>
    /// Get Feature name based on FNO.
    /// </summary>
    /// <param name="Fno">Feature FNO</param>
    /// <returns></returns>
    private string GetFeatureName(short Fno)
    {
      Recordset rs = null;
      try
      {
        rs = m_DataContext.MetadataRecordset("G3E_FEATURES_OPTABLE", "G3E_FNO = " + Fno);

        if(rs != null && rs.RecordCount > 0)
        {
          rs.MoveFirst();

          return Convert.ToString(rs.Fields["G3E_USERNAME"].Value);
        }
      }
      catch
      {
        throw;
      }
      finally
      {
        if(rs != null)
        {
          rs.Close();
          rs = null;
        }
      }
      return null;
    }


    /// <summary>
    /// Get Component name of the CNO.
    /// </summary>
    /// <param name="Cno"> Component CNO </param>
    /// <returns></returns>
    private string GetComponentName(short Cno)
    {
      Recordset rs = null;
      try
      {
        rs = m_DataContext.MetadataRecordset("G3E_COMPONENTINFO_OPTABLE", "g3e_cno = " + Cno);

        if(rs != null && rs.RecordCount > 0)
        {
          rs.MoveFirst();

          return Convert.ToString(rs.Fields["G3E_USERNAME"].Value);
        }
      }
      catch
      {
        throw;
      }
      finally
      {
        if(rs != null)
        {
          rs.Close();
          rs = null;
        }
      }
      return null;
    }

    /// <summary>
    /// Get Attribute name of the ANO.
    /// </summary>
    /// <param name="Ano"> Attribute ANO </param>
    /// <returns></returns>
    private string GetAttributeName(int Ano)
    {
      Recordset rs = null;
      try
      {
        rs = m_DataContext.MetadataRecordset("G3E_ATTRIBUTEINFO_OPTABLE", "g3e_ano = " + Ano);
        if(rs != null && rs.RecordCount > 0)
        {
          rs.MoveFirst();

          return Convert.ToString(rs.Fields["G3E_USERNAME"].Value);
        }
      }
      catch
      {
        throw;
      }
      finally
      {
        if(rs != null)
        {
          rs.Close();
          rs = null;
        }
      }
      return null;
    }

    /// <summary>
    /// Getting Job Description,WR NBR value and Designer UID from G3e_Job
    /// </summary>
    /// <param name="strJobId"> Job ID </param>
    /// <returns></returns>
    private string[] GetJobDescription(string strJobId)
    {
      string[] strJobDescription = new string[3];
      Recordset rs = null;
      string sql = null;
      try
      {
        sql = "select G3E_DESCRIPTION,DESIGNER_UID,WR_NBR from g3e_job where g3e_identifier=:1";
        rs = m_DataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText,
                new object[] { strJobId });

        if(rs != null && rs.RecordCount > 0)
        {
          rs.MoveFirst();
          strJobDescription[0] = Convert.ToString(rs.Fields[0].Value);
          strJobDescription[1] = Convert.ToString(rs.Fields[1].Value);
          if(!string.IsNullOrEmpty(Convert.ToString(rs.Fields[2].Value)))
          {
            strJobDescription[2] = Convert.ToString(rs.Fields[2].Value);
          }
          else
          {
            strJobDescription[2] = "0";
          }
        }
      }
      catch
      {
        throw;
      }
      finally
      {
        if(rs != null)
        {
          if(rs != null)
          {
            rs.Close();
            rs = null;
          }
        }
      }
      return strJobDescription;
    }

    /// <summary>
    /// To Exit the command.
    /// </summary>
    /// <param name="m_oGTTransactionManager"></param>
    /// <param name="m_GTCustomCommandHelper"></param>
    internal void ExitCommand(IGTTransactionManager m_oGTTransactionManager, IGTCustomCommandHelper m_GTCustomCommandHelper)
    {
      try
      {
        if(m_GTCustomCommandHelper != null)
        {
          m_GTCustomCommandHelper.Complete();
          m_GTCustomCommandHelper = null;
        }

        m_Application.Application.RefreshWindows();
      }
      catch
      {
        if(m_GTCustomCommandHelper != null)
        {
          m_GTCustomCommandHelper.Complete();
          m_GTCustomCommandHelper = null;
        }

        m_GTCustomCommandHelper = null;
        m_Application.Application.RefreshWindows();
      }
    }
  }
}
