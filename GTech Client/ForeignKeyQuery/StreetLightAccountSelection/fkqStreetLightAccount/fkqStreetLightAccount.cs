//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  06/02/2018          Pramod                     Implemented to select ESI_Location from Steet Light Account tables    
//  06/03/2018          Pramod                      Added condition to check role granted to user  
// ======================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ADODB;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;

namespace GTechnology.Oncor.CustomAPI
{
  public class fkqStreetLightAccount : IGTForeignKeyQuery
  {
    #region Private Members

    IGTKeyObject _gtFeature;
    GTArguments _gtArguments;
    IGTDataContext _gtDataContext;
    IGTFieldValue _gtOutputValue;
    string _gtFieldName, _gtTableName;
    bool _gtReadOnly;
    string formCaption = "Street Light ESI Location Selection";
    string stltValidationMsg = "An ESI Location cannot be selected until the Street Light graphic symbol exists and the CU is assigned to the Street Light.";
    string stltAcctValidationMsg = "No Street Light Accounts match the Wattage, Lamp Type, and Luminaire Style that are associated with the selected CU.\n\n Please notify a Street Light Administrator.";
    string _lampType, _wattage, _lmStyle;
    string StreetLightRole = "PRIV_MGMT_STLT";


    #endregion

    public Intergraph.GTechnology.API.GTArguments Arguments
    {
      get { return _gtArguments; }
      set { _gtArguments = value; }
    }

    public Intergraph.GTechnology.API.IGTDataContext DataContext
    {
      get { return _gtDataContext; }
      set { _gtDataContext = value; }
    }

    public bool Execute(object InputValue)
    {
      bool selection = false;
      ESIAccountSelection esiAcctSelection = null;

      try
      {
        //Check selected Street Light has valid geomertry and attribtues values
        if(ValidateStreetLight())
        {
          _gtOutputValue = GTClassFactory.Create<IGTFieldValue>();
          esiAcctSelection = new ESIAccountSelection();

          // Get Geograpchic Street Light Account for selected Street Light Feature Instance
          esiAcctSelection.GeoStreetLightAccounts = GetGeographicStreetLightAct(_gtFeature.FID);
          //Get All Street Light Accoutns matching with selected Street Light Feature Instance
          esiAcctSelection.StreetLightAccounts = GetStreetLightAccts();
          //Check Street Light Account exists otherwise throw validation message
          if(esiAcctSelection.GeoStreetLightAccounts.Count > 0 || esiAcctSelection.StreetLightAccounts.Count > 0)
          {
            esiAcctSelection.LoadData();
            if(esiAcctSelection.ShowDialog() == DialogResult.OK)
            {
              _gtOutputValue.FieldValue = esiAcctSelection.SelectedSTLTAccount.ESI_Location;
              selection = true;
            }
          }
          else
          {
            MessageBox.Show(stltAcctValidationMsg, formCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
          }
        }
        else
        {
          MessageBox.Show(stltValidationMsg, formCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
      }
      catch(Exception ex)
      {
        MessageBox.Show("Error in Street Light ESI Account Selection\n" + ex.Message, formCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      finally
      {
        if(esiAcctSelection != null)
        {
          esiAcctSelection.Dispose();
          esiAcctSelection.StreetLightAccounts = null;
          esiAcctSelection.GeoStreetLightAccounts = null;
          esiAcctSelection = null;
        }
      }
      return selection;
    }

    public Intergraph.GTechnology.API.IGTKeyObject Feature
    {
      get { return _gtFeature; }
      set { _gtFeature = value; }
    }

    public string FieldName
    {
      get { return _gtFieldName; }
      set { _gtFieldName = value; }
    }

    public Intergraph.GTechnology.API.IGTFieldValue OutputValue
    {
      get { return _gtOutputValue; }
    }

    public bool ReadOnly
    {
      get { return _gtReadOnly; }
      set { _gtReadOnly = value; }
    }

    public string TableName
    {
      get { return _gtTableName; }
      set { _gtTableName = value; }
    }

    #region Private Methods

    /// <summary>
    ///  Validate Selected Street Light 
    /// </summary>
    /// <returns></returns>
    private bool ValidateStreetLight()
    {
      bool flag = true;
      Recordset rs = _gtFeature.Components["STREETLIGHT_N"].Recordset;
      if(rs != null)
      {
        rs.MoveFirst();
        _lampType = Convert.ToString(rs.Fields["LAMP_TYPE_C"].Value);
        _wattage = Convert.ToString(rs.Fields["WATT_Q"].Value);
        _lmStyle = Convert.ToString(rs.Fields["LUMIN_STYL_C"].Value);
      }

      if(_gtFeature.Components["STREETLIGHT_S"].Geometry == null || string.IsNullOrEmpty(_lampType) || string.IsNullOrEmpty(_wattage) || string.IsNullOrEmpty(_lmStyle))
      {
        flag = false;
      }
      return flag;
    }



    /// <summary>
    /// Execute SQL statement
    /// </summary>
    /// <param name="sqlStmt"></param>
    /// <returns></returns>
    private Recordset Execute(string sqlStmt)
    {
      int recordsAffected;
      ADODB.Recordset rs = null;
      rs = _gtDataContext.Execute(sqlStmt, out recordsAffected, (int)ADODB.CommandTypeEnum.adCmdText);
      return rs;
    }

    /// <summary>
    /// Get Geographic Street Light Accounts info touching boudaries
    /// </summary>
    /// <param name="g3eFid"></param>
    /// <returns></returns>
    private IList<StreetLightAccount> GetGeographicStreetLightAct(int g3eFid)
    {
      IList<StreetLightAccount> streetLightAccts = new List<StreetLightAccount>();
      IList<StreetLightBoundary> stltBoundarys = new List<StreetLightBoundary>();
      Recordset rs = null;

      string sqlStmt = "SELECT A.ESI_LOCATION,	D.DESCRIPTION,	A.RATE_SCHEDULE,A.LAMP_TYPE,A.WATTAGE,A.LUMINARE_STYLE FROM " +
          " STLT_ACCOUNT A, STLT_BOUNDARY B,{0} C,STLT_DESC_VL D  WHERE A.DESCRIPTION_ID=D.DESCRIPTION_ID AND A.BOUNDARY_CLASS = B.BND_CLASS " +
          "  AND B.BND_FNO={1} AND A.BOUNDARY_ID =TO_CHAR(c.{2}) AND c.G3E_FID={3} " +
          " AND A.LAMP_TYPE = '{4}' AND A.WATTAGE ='{5}'" +
          " AND A.LUMINARE_STYLE ='{6}' ";
      //check role granted user otherwise add condinition restricted ='n' to query
      if(!_gtDataContext.IsRoleGranted(StreetLightRole))
      {
        sqlStmt += "AND A.RESTRICTED = 'N'"; //(included only for non-SLAs)
      }
      //Get boundary info touching Street Light
      stltBoundarys = GetStreetLightBoundarys();
      if(stltBoundarys != null && stltBoundarys.Count > 0)
      {
        short[] boundaryFnos = stltBoundarys.Select(a => a.BoundaryFno).ToArray();

        var boundarys = GetBoundaryTouchStreetLight(boundaryFnos);
        if(boundarys.Count > 0)
        {

          var bndFids = (from a in stltBoundarys
                         join b in boundarys on a.BoundaryFno equals b.Value
                         select new
                         {
                           g3eFno = b.Value,
                           g3eFid = b.Key,
                           g3eTable = a.G3eTable,
                           g3eField = a.G3eField
                         }).ToList();
          foreach(var boundary in bndFids)
          {

            //Get Street Light Account 
            rs = Execute(string.Format(sqlStmt, boundary.g3eTable, boundary.g3eFno, boundary.g3eField, boundary.g3eFid, _lampType, _wattage, _lmStyle));
            if(rs != null && rs.RecordCount > 0)
            {
              ConvertRsToEntity(rs).ToList().ForEach(a => streetLightAccts.Add(a));
            }
          }
        }
      }
      return streetLightAccts;
    }


    /// <summary>
    /// Get Street Light Account info matching selected Street Light 
    /// </summary>
    /// <returns></returns>
    private IList<StreetLightAccount> GetStreetLightAccts()
    {
      IList<StreetLightAccount> streetLightAccts = new List<StreetLightAccount>();
      string sqlStmt = "SELECT	A.ESI_LOCATION,	B.DESCRIPTION,	A.RATE_SCHEDULE,A.LAMP_TYPE,A.WATTAGE,A.LUMINARE_STYLE FROM " +
          " STLT_ACCOUNT A,STLT_DESC_VL B  WHERE A.DESCRIPTION_ID=B.DESCRIPTION_ID AND A.LAMP_TYPE = '{0}' AND A.WATTAGE ='{1}'" +
          " AND A.LUMINARE_STYLE ='{2}'";
      sqlStmt = string.Format(sqlStmt, _lampType, _wattage, _lmStyle);
      Recordset rs = Execute(sqlStmt);
      if(rs != null && rs.RecordCount > 0)
      {
        ConvertRsToEntity(rs).ToList().ForEach(a => streetLightAccts.Add(a));
      }
      return streetLightAccts;
    }

    /// <summary>
    /// Get boundaries touching Street Light
    /// </summary>
    /// <param name="fnoArray"></param>
    /// <returns></returns>
    private IDictionary<int, short> GetBoundaryTouchStreetLight(short[] fnoArray)
    {

      IDictionary<int, short> boundaryFids = new Dictionary<int, short>();
      IGTSpatialService gtspatialService = GTClassFactory.Create<IGTSpatialService>();
      gtspatialService.DataContext = _gtDataContext;

      //gtspatialService.FilterGeometry = _gtDataContext.GetDDCKeyObjects(_gtFeature.FNO, _gtFeature.FID, GTComponentGeometryConstants.gtddcgPrimaryGeographic)[0].Geometry;
      if(null != Feature.Components["STREETLIGHT_S"].Geometry)
      {
        gtspatialService.FilterGeometry = Feature.Components["STREETLIGHT_S"].Geometry;
      }
      else
      {
        throw new Exception("A Street Light's symbol geometry must exist before an ESI Location can be assigned to it.");
      }

      gtspatialService.Operator = GTSpatialOperatorConstants.gtsoTouches;
      Recordset rs = gtspatialService.GetResultsByFNO(fnoArray);
      if(rs != null && rs.RecordCount > 0)
      {
        rs.MoveFirst();
        while(!rs.EOF)
        {
          boundaryFids.Add(Convert.ToInt32(rs.Fields["G3E_FID"].Value), Convert.ToInt16(rs.Fields["G3E_FNO"].Value));
          rs.MoveNext();
        }
      }
      return boundaryFids;
    }

    /// <summary>
    /// Get boundary Attribute info to filter Street Light Account
    /// </summary>
    /// <returns></returns>
    private List<StreetLightBoundary> GetStreetLightBoundarys()
    {
      List<StreetLightBoundary> stltBoundarys = new List<StreetLightBoundary>();
      string sqlStmt = "SELECT BND_CLASS,BND_FNO,A.G3E_FIELD,A.G3E_COMPONENTTABLE FROM STLT_BOUNDARY st,G3E_ATTRIBUTEINFO_OPTABLE a where st.BND_ID_ANO=a.g3e_ano";
      Recordset rs = Execute(sqlStmt);
      if(rs != null && rs.RecordCount > 0)
      {
        rs.MoveFirst();
        while(!rs.EOF)
        {

          stltBoundarys.Add(new StreetLightBoundary
          {
            BoundaryClass = Convert.ToInt32(rs.Fields["BND_CLASS"].Value),
            BoundaryFno = Convert.ToInt16(rs.Fields["BND_FNO"].Value),
            G3eField = Convert.ToString(rs.Fields["G3E_FIELD"].Value),
            G3eTable = Convert.ToString(rs.Fields["G3E_COMPONENTTABLE"].Value)

          }
          );
          rs.MoveNext();
        }
      }
      return stltBoundarys;
    }

    /// <summary>
    /// convert Recordset to Entity
    /// </summary>
    /// <param name="rs"></param>
    /// <returns></returns>
    private IList<StreetLightAccount> ConvertRsToEntity(Recordset rs)
    {
      List<StreetLightAccount> streetAccts = new List<StreetLightAccount>();
      while(!rs.EOF)
      {
        streetAccts.Add(new StreetLightAccount
        {
          ESI_Location = Convert.ToString(rs.Fields["ESI_LOCATION"].Value),
          Description = Convert.ToString(rs.Fields["DESCRIPTION"].Value),
          Lamp_Type = Convert.ToString(rs.Fields["LAMP_TYPE"].Value),
          Wattage = Convert.ToString(rs.Fields["WATTAGE"].Value),
          Rate_Schedule = Convert.ToString(rs.Fields["RATE_SCHEDULE"].Value),
          Luminare_Style = Convert.ToString(rs.Fields["LUMINARE_STYLE"].Value)
        }
        );
        rs.MoveNext();
      }
      return streetAccts.ToList();

    }
    #endregion

  }
}
