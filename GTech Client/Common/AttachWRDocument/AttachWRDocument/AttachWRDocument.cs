using System;
using ADODB;
using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI
{
  public class AttachWRDocument
  {

    // Subsystem values for SYS_GENERALPARAMETER relative to Document Management
    const string SUBSYSTEMNAME = "Doc_Management";
    const string SUBSYSTEMCOMPONENT = "GT_SharePoint";

    /// <summary>
    /// Attaches a file to the WR in SharePoint and creates the Hyperlink on the Design Area feature for the WR.
    /// </summary>
    /// <param name="WR">WR to attach file</param>
    /// <param name="fileName">Full pathname of file to attach.</param>
    /// <param name="attachmentType">Classification of file to attach (e.g. "As-Built Redlines", "Construction Redlines", etc.).</param>
    /// <returns></returns>
    public bool AttachDocument(string WR, string fileName, string attachmentType)
    {
      try
      {
        bool retVal = false;

        IGTApplication app = GTClassFactory.Create<IGTApplication>();
        app.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Uploading archived prints to the document repository.");

        // Add the document to Sharepoint via the ONC Document Manager interface
        OncDocManage.OncDocManage oncDocManager = new OncDocManage.OncDocManage();

        // These three are set in a given interface because the OncDocManage class is designed
        // such that it can be used by systems other than G/Tech.
        oncDocManager.SPSiteURL = SysGeneralParameter(SUBSYSTEMNAME, SUBSYSTEMCOMPONENT, "SP_URL");
        oncDocManager.SPRelPath = SysGeneralParameter(SUBSYSTEMNAME, SUBSYSTEMCOMPONENT, "JOBWO_REL_PATH");
        oncDocManager.SPRootPath = SysGeneralParameter(SUBSYSTEMNAME, SUBSYSTEMCOMPONENT, "ROOT_PATH");

        oncDocManager.SrcFilePath = fileName;
        oncDocManager.SPFileName = fileName.Substring(fileName.LastIndexOf("\\") + 1);
        oncDocManager.SPFileType = attachmentType;
        oncDocManager.SPFileDescription = string.Format("Archival Print - {0}", attachmentType);
        oncDocManager.WrkOrd_Job = WR;

        // If the document is successfully uploaded, then create the hyperlink record on the Design Area
        if(oncDocManager.AddSPFile(true))
        {
          retVal = AddDesignAreaHyperlink(oncDocManager.RetFileURL, attachmentType, oncDocManager.RetFileName, oncDocManager.SPFileDescription);
        }
        else
        {
          string msg = string.Format("Error attempting to add a file to the document repository:  {0}", oncDocManager.RetErrMessage);
          throw new Exception(msg);
        }

        return retVal;
      }
      catch(Exception ex)
      {
        string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
        throw new Exception(exMsg);
      }
    }

    private bool AddDesignAreaHyperlink(string hyperLinkURL, string attachmentType, string fileName, string description)
    {
      try
      {
        bool retVal = false;

        // Even though the Design Area is a STT feature, using OpenFeature forces the caller
        // to commit this transaction using the Transaction Manager.  If that is ever an issue,
        // this method could be changed to perform a simple insert into the component recordset
        // but by using the API, it allows the system to do some of that work for us
        // (e.g. assigning values for the CNO and the CID).

        IGTKeyObject designArea = DesignArea;

        if(null != designArea)
        {
          IGTComponent comp = designArea.Components["JOB_HYPERLINK_N"];

          if(null != comp && null != comp.Recordset)
          {
            comp.Recordset.AddNew("G3E_FID", designArea.FID);
            comp.Recordset.Fields["G3E_FNO"].Value = designArea.FNO;
            comp.Recordset.Fields["HYPERLINK_T"].Value = hyperLinkURL;
            comp.Recordset.Fields["TYPE_C"].Value = attachmentType;
            comp.Recordset.Fields["FILENAME_T"].Value = fileName;
            comp.Recordset.Fields["DESCRIPTION_T"].Value = description;

            retVal = true;
          }

        }

        return retVal;
      }
      catch(Exception ex)
      {
        string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
        throw new Exception(exMsg);
      }
    }

    /// <summary>
    /// Key Object for the Design Area for the active job
    /// </summary>
    private IGTKeyObject DesignArea
    {
      get
      {
        try
        {
          IGTKeyObject designArea = null;

          IGTApplication app = GTClassFactory.Create<IGTApplication>();
          string sql = "select g3e_fno,g3e_fid from designarea_p where job_id=?";
          Recordset rs = app.DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, app.DataContext.ActiveJob);

          if(null != rs && 0 < rs.RecordCount)
          {
            rs.MoveFirst();
            short fno = Convert.ToInt16(rs.Fields["g3e_fno"].Value);
            int fid = Convert.ToInt32(rs.Fields["g3e_fid"].Value);
            designArea = app.DataContext.OpenFeature(fno, fid);
            rs.Close();
            rs = null;
          }

          return designArea;
        }
        catch(Exception ex)
        {
          string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
          throw new Exception(exMsg);
        }

      }
    }
    /// <summary>
    /// Returns a parameter value from SYS_GENERALPARAMETER
    /// </summary>
    /// <param name="subsystem">SUBSYSTEM_NAME</param>
    /// <param name="component">COMPONENT_NAME</param>
    /// <param name="parameter">PARAMETER_NAME</param>
    /// <returns></returns>
    private string SysGeneralParameter(string subsystem, string component, string parameter)
    {
      try
      {
        string retVal = string.Empty;

        IGTApplication app = GTClassFactory.Create<IGTApplication>();
        string sql = "select param_value from sys_generalparameter where subsystem_name=? and subsystem_component=? and param_name=?";
        Recordset rs = app.DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, subsystem, component, parameter);

        if(null != rs && 0 < rs.RecordCount)
        {
          rs.MoveFirst();
          if(System.DBNull.Value != rs.Fields[0].Value)
          {
            retVal = rs.Fields[0].Value.ToString().Trim();
          }
          else
          {
            string msg = string.Format("Parameter value in SYS_GENERALPARAMETER is NULL where SUBSYSTEM_NAME={0} and SUBSYSTEM_COMPONENT={1} and PARAM_NAME={2}", subsystem, component, parameter);
            throw new Exception(msg);
          }
        }
        else
        {
          string msg = string.Format("No record found in SYS_GENERALPARAMETER where SUBSYSTEM_NAME={0} and SUBSYSTEM_COMPONENT={1} and PARAM_NAME={2}", subsystem, component, parameter);
          throw new Exception(msg);
        }

        return retVal;
      }
      catch(Exception ex)
      {
        string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
        throw new Exception(exMsg);
      }
    }
  }
}
