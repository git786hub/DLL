using System;
using System.Windows.Forms;
using System.Collections.Generic;
using CustomWriteBackLibrary;
using Intergraph.GTechnology.Interfaces;
using Intergraph.GTechnology.API;
using ADODB;

namespace GTechnology.Oncor.CustomAPI
{
  public class PostJob : IGTCustomCommandModal
  {
    public IGTTransactionManager TransactionManager { set; get; }

    /// <summary>
    /// The confirmation dialog that is displayed once the writeback is started.
    /// </summary>

    /// <summary>
    /// Added to ensure same values are used throughout this class
    /// </summary>
    private const string asBuilt = "AsBuilt";
    private const string constructionComplete = "ConstructionComplete";
    private const string nonWR = "NON-WR";
    private const string network = "NET";
    private const string networkDrawingType = "CONREDLINE";// "Network Drawing"; // ToDo: Confirm type name with the hyperlink type picklist table (VL_HYPERLINK_TYPE)
    private const short commonCNO = 1;
    private const short manholeFNO = 106;
    private const short vaultFNO = 117;


    /// <summary>
    /// This may need to be changed or, possibly, paramaterized.
    /// Document Management, when complete, should drive this value
    /// </summary>
    private const string drawingType = "Network";

    /// <summary>
    /// Entry point for the Custom Command Modal interface.
    /// </summary>
    public void Activate()
    {
      try
      {
                // Check for out of synch WPs and exit if they are not synchronized
                SharedWriteBackLibrary swbl = new SharedWriteBackLibrary();
                if (!swbl.ValidateWorkPoints())
                {
                    return;
                }

                TransactionManager.Begin("Post Job");

        // PendingEditsExist builds this recordset and we can use it again in
        // ValidateNetworkDrawings to keep from querying for it again there.
        Recordset pendingEdits = null;

        // If there are no pending edits, then nothing else to do here.
        // PendingEditsExist will close/null the recordset object if there are no pending edits.
        if(!PendingEditsExist(ref pendingEdits))
        {
          MessageBox.Show("There are no pending edits in the active job.  Exiting command.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Information);
          return;
        }

        // Validate pending job edits
        if(!ResolveJobValidationErrors())
        {
          return;
        }

        // Perform Job Validations.  Return if any fail.
        if(!JobAttributesAreValid)
        {
          return;
        }

        // Check for existing conflicts
        if(ConflictsExist())
        {
          return;
        }

        

        // Allow user to validate the pertinent network drawings attached to the active job.
        // ValidateNetworkDrawings will close/null the recordset object when finished with it.
        if (!ValidateNetworkDrawings(ref pendingEdits))
        {
          return;
        }

        TransactionManager.Commit(true);

        // Post edits for active job
        if(!PostJobEdits())
        {
          return;
        }

                // Since nothing else is updated in this command from here to the end,
                // a transaction here should not be necessary; however, leaving it just in case.
                TransactionManager.Begin("Post Job");

        // If indicated, then invoke a Writeback
        // ALM 1566 - Requests removal of the check for the write back flag.
        //            Future design change will check for this in a different way
        //            but leaving the clause in the conditional statement for clarity
        //            until that change is implemented.
        JobManager jobManager = new JobManager();

        if(nonWR != jobManager.JobType /*&& jobManager.WriteBackNeeded*/)
        {
          DoWriteBack(swbl);
        }

        if(TransactionManager.TransactionInProgress)
        {
          TransactionManager.Commit(true);
        }

      }
      catch(Exception ex)
      {
        if(TransactionManager.TransactionInProgress)
        {
          TransactionManager.Rollback();
        }

        string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
        throw new Exception(exMsg);
      }
    }

    /// <summary>
    /// Finds any pending edits and returns them in the recordset parameter
    /// </summary>
    /// <param name="pendingEdits">Recordset of pending edits</param>
    /// <returns>true if pending edits exist; else, false</returns>
    private bool PendingEditsExist(ref Recordset pendingEdits)
    {
      try
      {
        IGTApplication app = GTClassFactory.Create<IGTApplication>();
        IGTJobManagementService jobService = GTClassFactory.Create<IGTJobManagementService>();
        jobService.DataContext = app.DataContext;

        pendingEdits = jobService.FindPendingEdits();

        // If there are no pending edits, then close and null the Recordset object.
        if(null != pendingEdits && 0 == pendingEdits.RecordCount)
        {
          if(pendingEdits.State != Convert.ToInt32(ObjectStateEnum.adStateClosed))
          {
            pendingEdits.Close();
          }
          pendingEdits = null;
        }

        return null != pendingEdits && 0 < pendingEdits.RecordCount;
      }
      catch(Exception ex)
      {
        string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
        throw new Exception(exMsg);
      }
    }

    /// <summary>
    /// Returns true if job attributes are valid; otherwise, false.
    /// </summary>
    public bool JobAttributesAreValid
    {
      get
      {
        try
        {
          JobManager jobManager = new JobManager();

          // Use this list to compare the JobStatus rather than force the JobManager to retrieve the JobStatus value twice
          List<string> validJobStatus = new List<string> { asBuilt, constructionComplete };

          if(jobManager.JobType.ToUpper() != nonWR && !validJobStatus.Contains(jobManager.JobStatus))
          {
            MessageBox.Show(string.Format("Posting may only occur in {0} or {1} status.", asBuilt, constructionComplete), "G/Technology", MessageBoxButtons.OK);
            return false;
          }

          return true;
        }
        catch(Exception ex)
        {
          string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
          throw new Exception(exMsg);
        }
      }
    }


    /// <summary>
    /// True if conflicts exist; else, false
    /// </summary>
    public bool ConflictsExist()
    {
      try
      {
        bool retVal = false;

        IGTApplication app = GTClassFactory.Create<IGTApplication>();
        IGTJobManagementService jobService = GTClassFactory.Create<IGTJobManagementService>();
        jobService.DataContext = app.DataContext;

        Recordset rs = jobService.FindConflicts();
        if(null != rs && 0 < rs.RecordCount)
        {
          MessageBox.Show("Some edits conflict with other posted jobs; please run Conflict Detection to resolve before posting.", "G /Technology", MessageBoxButtons.OK);
          retVal = true;
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
    /// Gets active job's pending edits.
    /// If P1, return false.
    /// If P2, present P2 error resolution form.  If user resolves all P2s return true, else return false.
    /// If P3 or P4, display message box asking if user wants to continue.  If yes, return true, else return false.
    /// </summary>
    /// <returns>True if no P1 and P2, P3, and P4 are presented/resolved; else, false</returns>
    private bool ResolveJobValidationErrors()
    {
      try
      {
        bool retVal = false;
        IGTApplication app = GTClassFactory.Create<IGTApplication>();

        IGTJobManagementService jobService = GTClassFactory.Create<IGTJobManagementService>();
        jobService.DataContext = app.DataContext;

        Recordset rs = jobService.ValidatePendingEdits();

        if(null != rs && 0 < rs.RecordCount)
        {
          rs.Sort = "ErrorPriority ASC";
          rs.MoveFirst();
          string highestErrorPriority = rs.Fields[0].Value.ToString();
 

          switch(highestErrorPriority)
          {
            case "P1":
              MessageBox.Show("P1 errors must be resolved before posting job edits.", "G/Technology", MessageBoxButtons.OK);
              break;

            case "P2":
              // display validation overrides form
              ValidationOverrides vo = new ValidationOverrides();

              // Clear the sort criteria (just in case it interferes with the ordering on the form)
              rs.Sort = string.Empty;

              // Only show P2s for Validation Overrides
              rs.Filter = "ErrorPriority='P2'";

              Recordset orrs = vo.ShowValidationComments(rs);

              // If the user Cancels the form, the returned recordset will be null; otherwise,
              // the returned recordset will be a copy of the rs parameter with a new OVERRIDE_COMMENTS
              // column appended to it that contains the override comment strings.
              // If the user cancels the form and returned recordset is null, then
              // the return value will remain unchaged at false (indicating a stop condition).
              if(null != orrs)
              {
                ManageValidationOverrideData mvo = new CustomAPI.ManageValidationOverrideData();

                // If the user has resolved all P2s and the system can update the records successfully,
                // then this will return true; otherwise, false.
                retVal = mvo.UpdateValidationOverrides(orrs);
              }
              break;

            default:
              DialogResult dialogResult = MessageBox.Show("Validation warnings encountered; proceed with post?", "G/Technology", MessageBoxButtons.OKCancel);
              retVal = dialogResult == DialogResult.OK;
              break;
          }

          rs.Close();
          rs = null;
        }
        else
        {
          retVal = true;
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
    /// Checks pending edits for AsBuilt/NET jobs containing hyperlinked documents.  If any exist, invoke a validation form allowing the user to OK each attachment.
    /// </summary>
    /// <param name="pendingEdits">Recordset containing pending edits for active job</param>
    /// <returns>true if no documents or if user reconciles all documents; else, false</returns>
    private bool ValidateNetworkDrawings(ref Recordset PendingEdits)
    {
      try
      {
        // Return value will be false only if the user Cancels the dialog
        bool retVal = true;

        JobManager jobManager = new JobManager();

        // If the properties aren't valid, then nothing to do here.
        if(!(asBuilt == jobManager.JobStatus && network == jobManager.JobType))
        {
          return retVal;
        }

        // This should never fail here but checking anyway.
        if(null != PendingEdits && 0 < PendingEdits.RecordCount)
        {
          // Build the list of NetworkDrawing objects
          List<NetworkDrawing> drawingList = BuildNetworkDrawingList(PendingEdits);

          // Instantiate the form
          PostJobNetworkDrawings networkDrawingForm = new PostJobNetworkDrawings();

          // Set the forms drawing list
          networkDrawingForm.DrawingList = drawingList;

          // Show the form.  If OK, then process updates and removals.
          if(DialogResult.OK == networkDrawingForm.ShowDialog())
          {

            //********************************************************************************************************************************************
            //************************ Complete these two actions after Document Management interface is complete. ***************************************
            //********************************************************************************************************************************************

            // Perform updates
            foreach(NetworkDrawing nd in drawingList.FindAll(NetworkDrawing => NetworkDrawing.Action.ToLower() == "update"))
            {
              OpenFileDialog ofd = new OpenFileDialog();
              ofd.Title = string.Format(@"Select new drawing file for: {0}", string.IsNullOrEmpty(nd.Description) ? "<No description given>" : nd.Description);
              ofd.ShowDialog();

              string updateMsg = string.Format(@"Replace file: {0}{1}With file: {2}{1}For: {3}", nd.Link, Environment.NewLine, ofd.FileName, string.IsNullOrEmpty(nd.Description) ? "<No description given>" : nd.Description);
              updateMsg += string.Format(@"{0}{0}This functionality is waiting on completion of the Document Management interface.", Environment.NewLine);
              MessageBox.Show(updateMsg, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Perform removals
            foreach(NetworkDrawing nd in drawingList.FindAll(NetworkDrawing => NetworkDrawing.Action.ToLower() == "remove"))
            {
              string updateMsg = string.Format(@"Remove file: {0}{1}For: {2}", nd.Link, Environment.NewLine, string.IsNullOrEmpty(nd.Description) ? "<No description given>" : nd.Description);
              updateMsg += string.Format(@"{0}{0}This functionality is waiting on completion of the Document Management interface.", Environment.NewLine);
              MessageBox.Show(updateMsg, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
          }

          networkDrawingForm.Dispose();
          drawingList.Clear();
          drawingList = null;
        }
        else
        {
          // No pending edits so nothing to check for in this case.
          retVal = true;
        }

        // Finished with the PendingEdits recordset
        if(null != PendingEdits)
        {

          if(PendingEdits.State != Convert.ToInt32(ObjectStateEnum.adStateClosed))
          {
            PendingEdits.Close();
          }

          PendingEdits = null;
        }

        return retVal;
      }
      catch(Exception ex)
      {
        string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
        throw new Exception(exMsg);
      }
    }

    private List<NetworkDrawing> BuildNetworkDrawingList(Recordset edits)
    {
      try
      {
        List<NetworkDrawing> ndl = new List<NetworkDrawing>();

        if(null != edits && 0 < edits.RecordCount)
        {
          // Filter the pending edits recordset to only Manholes and Vaults
          edits.Filter = string.Format("(g3e_cno={0} and g3e_fno={1}) or (g3e_cno={0} and g3e_fno={2})", commonCNO, manholeFNO, vaultFNO);

          // If any records meet the criteria, then iterate the recordset and begin building the list of drawing objects.
          if(0 < edits.RecordCount)
          {
            edits.MoveFirst();

            do
            {
              int FID = Convert.ToInt32(edits.Fields["g3e_fid"].Value);
              short FNO = Convert.ToInt16(edits.Fields["g3e_fno"].Value);
              string structureID = StructureIDbyFID(FID); // Get once and supply both methods with it.

              // If the Manhole or Vault has an associated Work Point,
              // then add the network drawings to the list.
              if(FeatureHasWorkPoint(structureID))
              {
                NewNetworkDrawings(FID, structureID, ndl);
              }

              edits.MoveNext();
            } while(!edits.EOF);

          }
        }

        return ndl;
      }
      catch(Exception ex)
      {
        string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
        throw new Exception(exMsg);
      }
    }

    /// <summary>
    /// Determines whether a feature has a workpoint associated to it (via common Structure ID)
    /// </summary>
    /// <param name="StructureID">Structure ID to use to find associated WorkPoint</param>
    /// <returns>true if feature has an associated workpoint; else, false</returns>
    private bool FeatureHasWorkPoint(string StructureID)
    {
      try
      {
        bool retVal = false;

        IGTApplication app = GTClassFactory.Create<IGTApplication>();
        string sql = "select count(1) from workpoint_n where structure_id=?"; //Probably don't really need the FNO here but just in case...
        Recordset rs = app.DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, StructureID);

        // If there's at least one Work Point, then return true
        if(null != rs && 0 < rs.RecordCount && 0 < Convert.ToInt32(rs.Fields[0].Value))
        {
          retVal = true;
          rs.Close();
          rs = null;
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
    /// Adds new NetworkDrawing objects from the Hyperlink component based on the FID
    /// </summary>
    /// <param name="FID">G3E_FID value</param>
    /// <param name="StructureID">Structure ID of feature</param>
    /// <param name="networkDrawings">List of network drawings</param>
    private void NewNetworkDrawings(int FID, string StructureID, List<NetworkDrawing> networkDrawings)
    {
      try
      {
        IGTApplication app = GTClassFactory.Create<IGTApplication>();
        string sql = "select h.g3e_fno,h.hyperlink_t,t.vl_value,h.description_t from hyperlink_n h join vl_hyperlink_type t on h.type_c=t.vl_key where h.g3e_fid=? and h.type_c=?";
        Recordset rs = app.DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, FID, networkDrawingType);
        if(null != rs && 0 < rs.RecordCount)
        {
          rs.MoveFirst();
          do
          {
            short FNO = Convert.ToInt16(rs.Fields["g3e_fno"].Value);
            string link = string.Empty;
            string linkType = string.Empty;
            string description = string.Empty;

            if(System.DBNull.Value != rs.Fields["hyperlink_t"].Value)
            {
              link = rs.Fields["hyperlink_t"].Value.ToString();
            }

            if(DBNull.Value != rs.Fields["vl_value"].Value)
            {
              linkType = rs.Fields["vl_value"].Value.ToString();
            }

            if(DBNull.Value != rs.Fields["description_t"].Value)
            {
              description = rs.Fields["description_t"].Value.ToString();
            }

            // Create the Network Drawing object
            NetworkDrawing nd = new NetworkDrawing()
            {
              FeatureClass = FeatureNameByFNO(FNO),
              StructureID = StructureID,
              Action = string.Empty,
              DrawingType = linkType,
              Link = link,
              Description = description
            };

            // Add the Network Drawing to the list
            networkDrawings.Add(nd);

            rs.MoveNext();
          } while(!rs.EOF);

          rs.Close();
          rs = null;
        }
      }
      catch(Exception ex)
      {
        string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
        throw new Exception(exMsg);
      }
    }

    /// <summary>
    /// Returns the component table name from the CNO parameter value.
    /// </summary>
    /// <param name="CNO">G3E_CNO</param>
    /// <returns>G3E_COMPONENT.G3E_TABLE</returns>
    private string TableNameByCNO(short CNO)
    {
      try
      {
        string tName = string.Empty;

        string sql = "select g3e_table from g3e_componentinfo_optlang where g3e_cno=?";
        IGTApplication app = GTClassFactory.Create<IGTApplication>();
        Recordset rs = app.DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, CNO);

        if(null != rs && 1 == rs.RecordCount)
        {
          if(System.DBNull.Value != rs.Fields["g3e_table"].Value)
          {
            tName = rs.Fields["g3e_table"].Value.ToString();
          }
          rs.Close();
          rs = null;
        }

        return tName;
      }
      catch(Exception ex)
      {
        string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
        throw new Exception(exMsg);
      }
    }

    /// <summary>
    /// Returns the feature name by the FNO value
    /// </summary>
    /// <param name="FNO">G3E_FNO</param>
    /// <returns>G3E_FEATURE.G3E_USERNAME</returns>
    private string FeatureNameByFNO(short FNO)
    {
      try
      {
        string featureName = string.Empty;

        string sql = "select g3e_username from g3e_features_optlang where g3e_fno=?";
        IGTApplication app = GTClassFactory.Create<IGTApplication>();
        Recordset rs = app.DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, FNO);
        if(null != rs && 1 == rs.RecordCount)
        {
          if(System.DBNull.Value != rs.Fields["g3e_username"].Value)
          {
            featureName = rs.Fields["g3e_username"].Value.ToString();
          }
          rs.Close();
          rs = null;
        }

        return featureName;
      }
      catch(Exception ex)
      {
        string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
        throw new Exception(exMsg);
      }
    }

    /// <summary>
    /// Finds a Structure ID by Feature ID
    /// </summary>
    /// <param name="FID">G3E_FID value</param>
    /// <returns>Structure ID</returns>
    private string StructureIDbyFID(int FID)
    {
      try
      {
        string retVal = string.Empty;

        IGTApplication app = GTClassFactory.Create<IGTApplication>();
        string sql = "select structure_id from common_n where g3e_fid=?";
        Recordset rs = app.DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, FID);

        if(null != rs)
        {

          if(1 == rs.RecordCount)
          {
            retVal = rs.Fields[0].Value.ToString();
          }

          rs.Close();
          rs = null;
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
        /// Based on FID,CNO,CID GetActivityOftheFeature will return the activity of the instance. 
        /// </summary>
        /// <param name="pFID"></param>
        /// <param name="pCNO"></param>
        /// <param name="pCID"></param>
        /// <param name="gTApplication"></param>
        /// <returns></returns>
        private string GetActivityOftheFeature(int pFID, int pCNO, int pCID, IGTApplication gTApplication)
        {
            Recordset rs = null;

            try
            {
                string sql = string.Format("select ACTIVITY_C from COMP_UNIT_N where g3e_fid={0} and g3e_cno={1} and g3e_cid={2}", pFID, pCNO, pCID);

                rs = gTApplication.DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText);
                if (rs != null && rs.RecordCount > 0)
                {
                    rs.MoveFirst();
                    return Convert.ToString(rs.Fields[0].Value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                rs = null;
            }
            return null;
        }

        /// <summary>
        /// Handleblanketunitizationactivity will handle the records which have pending activity related to blanket unitization (i.e.- where Activity = UR or UX).
        /// </summary>
        /// <returns></returns>
        public bool Handleblanketunitizationactivity()
        {
            Recordset pendingEdits = null;

            string strRFC = null;
            string strActivity = null;
            String strServiceCode = null;

            try
            {
                bool retVal = true;

                IGTApplication app = GTClassFactory.Create<IGTApplication>();

                IGTJobManagementService jobService = GTClassFactory.Create<IGTJobManagementService>();
                jobService.DataContext = app.DataContext;

                pendingEdits = jobService.FindPendingEdits();

                if (null == pendingEdits || 0 >= pendingEdits.RecordCount)
                {
                    retVal = false;
                    return retVal;
                }
                pendingEdits.MoveFirst();
                pendingEdits.Filter = string.Format("g3e_cno={0} or g3e_cno={1}", 21, 22);

                if (pendingEdits != null && pendingEdits.RecordCount > 0)
                {
                    JobManager jobManager = new JobManager();
                    strRFC = jobManager.DesignerRACFID;
                    pendingEdits.MoveFirst();

                    while (!pendingEdits.EOF)
                    {
                        strActivity = GetActivityOftheFeature(Convert.ToInt32(pendingEdits.Fields["G3E_FID"].Value),
                            Convert.ToInt32(pendingEdits.Fields["G3E_CNO"].Value),
                            Convert.ToInt32(pendingEdits.Fields["G3E_CID"].Value), app);
                        strServiceCode = null;

                        if (strActivity == "UX")
                        {
                            strServiceCode = "RP";
                        }
                        else if (strActivity == "UR")
                        {
                            strServiceCode = "RM";
                        }
                        if (!String.IsNullOrEmpty(strServiceCode))
                        {

                            Command cmd = new Command();
                            cmd.CommandType = CommandTypeEnum.adCmdStoredProc;
                            Parameter param = cmd.CreateParameter("p_FID", DataTypeEnum.adDecimal, ParameterDirectionEnum.adParamInput, 20, Convert.ToInt32(pendingEdits.Fields["G3E_FID"].Value));
                            cmd.Parameters.Append(param);
                            param = cmd.CreateParameter("p_serviceInfoCode", DataTypeEnum.adLongVarChar, ParameterDirectionEnum.adParamInput, 20, strServiceCode);
                            cmd.Parameters.Append(param);
                            param = cmd.CreateParameter("p_userID", DataTypeEnum.adLongVarChar, ParameterDirectionEnum.adParamInput, 20, strRFC);
                            cmd.Parameters.Append(param);
                            param = cmd.CreateParameter("P_CNO", DataTypeEnum.adDecimal, ParameterDirectionEnum.adParamInput, 20, Convert.ToInt32(pendingEdits.Fields["G3E_CNO"].Value));
                            cmd.Parameters.Append(param);
                            param = cmd.CreateParameter("p_CID", DataTypeEnum.adDecimal, ParameterDirectionEnum.adParamInput, 20, Convert.ToInt32(pendingEdits.Fields["G3E_CID"].Value));
                            cmd.Parameters.Append(param);


                            cmd.CommandText = "GISPKG_SERVICEACTIVITY.InsertServiceActivity";
                            app.Application.DataContext.ExecuteCommand(cmd, out int recordsAffected);

                            IGTKeyObject gTKeyObject = null;

                            gTKeyObject = app.Application.DataContext.OpenFeature(Convert.ToInt16(pendingEdits.Fields["G3E_FNO"].Value),
                          Convert.ToInt32(pendingEdits.Fields["G3E_FID"].Value));

                            

                            if (gTKeyObject.Components["COMP_UNIT_N"] != null)
                            {
                                if (gTKeyObject.Components["COMP_UNIT_N"].Recordset != null && gTKeyObject.Components["COMP_UNIT_N"].Recordset.RecordCount > 0)
                                {
                                    gTKeyObject.Components["COMP_UNIT_N"].Recordset.MoveFirst();
                                    while (!gTKeyObject.Components["COMP_UNIT_N"].Recordset.EOF)
                                    {
                                        if (Convert.ToInt32(pendingEdits.Fields["G3E_CNO"].Value) ==
                                            Convert.ToInt32(gTKeyObject.Components["COMP_UNIT_N"].Recordset.Fields["G3E_CNO"].Value)
                                            &&
                                            Convert.ToInt32(gTKeyObject.Components["COMP_UNIT_N"].Recordset.Fields["g3e_cid"].Value) ==
                                            Convert.ToInt32(pendingEdits.Fields["G3E_CID"].Value))
                                        {
                                            gTKeyObject.Components["COMP_UNIT_N"].Recordset.Fields["ACTIVITY_C"].Value = "";
                                        }
                                        gTKeyObject.Components["COMP_UNIT_N"].Recordset.Update();
                                        gTKeyObject.Components["COMP_UNIT_N"].Recordset.MoveNext();
                                    }

                                }
                            }

                            if (gTKeyObject.Components["COMMON_N"] != null)
                            {
                                if (gTKeyObject.Components["COMMON_N"].Recordset != null && gTKeyObject.Components["COMMON_N"].Recordset.RecordCount > 0)
                                {
                                    gTKeyObject.Components["COMMON_N"].Recordset.MoveFirst();
                                    gTKeyObject.Components["COMMON_N"].Recordset.Fields["FEATURE_STATE_C"].Value = "CLS";
                                    gTKeyObject.Components["COMMON_N"].Recordset.Update();
                                }
                            }

                        }

                        pendingEdits.MoveNext();
                    }

                }

                return true;
            }
            catch (Exception ex)
            {
                if (TransactionManager.TransactionInProgress)
                {
                    TransactionManager.Rollback();
                }

                string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
                throw new Exception(exMsg);
            }
        }
        /// <summary>
        /// Post the active job edits using LTT_POST.POST
        /// </summary>
        /// <returns></returns>
        private bool PostJobEdits()
        {
            IGTApplication app = GTClassFactory.Create<IGTApplication>();
            try
            {
                // Since validations have already been performed, using the LTT_POST.POST to avoid rechecking validations, etc.
                TransactionManager.Begin("Post Job");

                bool returnValue = Handleblanketunitizationactivity();                
                app.DataContext.Execute("begin ltt_post.post;end;", out int recs, (int)CommandTypeEnum.adCmdText);

                if (TransactionManager.TransactionInProgress)
                {
                    TransactionManager.Commit(true);
                }

                return true;
            }
            catch (Exception ex)
            {
                if (TransactionManager.TransactionInProgress)
                {
                    TransactionManager.Rollback();
                }

                app.DataContext.Execute("rollback", out int rec, (int)CommandTypeEnum.adCmdText);
                string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
                throw new Exception(exMsg);
            }
        }

    /// <summary>
    /// Starts an asynchronous writeback to WMIS process
    /// </summary>
    private void DoWriteBack(SharedWriteBackLibrary p_swbl)
    {
      try
      {

        // The shared library does the actual writeback.
        // Once complete, it will call the WriteBackProcessCompleted event.
        //SharedWriteBackLibrary swbl = new SharedWriteBackLibrary();
        // p_swbl.WriteBackProcessCompleted += swbl_WriteBackProcessCompleted;

        // Asynchronous call to perform the writeback.
        Guid taskID = Guid.NewGuid();
        JobManager jobManager = new JobManager();
                p_swbl.UpdateWriteBack(jobManager.ActiveJob, taskID);

                // Subscribed to writeback completed event so no longer need a pointer to the library object.
                p_swbl = null;

        // Deactivate the active job.
        // The argument determines whether to keep the job associated with the workspace in read-only mode.
        // Design doesn't state one way or the other but going with true.
        jobManager.DeactivateJob(true);

      }
      catch(Exception ex)
      {
        string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
        throw new Exception(exMsg);
      }
    }
  }
}
