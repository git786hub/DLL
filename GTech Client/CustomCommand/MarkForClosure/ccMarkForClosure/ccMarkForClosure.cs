using System;
using System.Windows.Forms;
using System.Collections.Generic;
using CustomWriteBackLibrary;
using Intergraph.GTechnology.Interfaces;
using Intergraph.GTechnology.API;
using ADODB;


namespace GTechnology.Oncor.CustomAPI
{
  public class ccMarkForClosure : IGTCustomCommandModal
  {
    public IGTTransactionManager TransactionManager { get; set; }

    /// <summary>
    /// Added to ensure same string is used throughout this class
    /// </summary>
    private const string closurePending = "ClosurePending";

    /// <summary>
    /// Event handler for the Shared Write Back Library SetClosurePendingJobStatus
    /// needs this value to set the WR status since the job is deactivated at that point.
    /// </summary>
    private int ActiveWRNbr
    { set; get; }

    /// <summary>
    /// Entry point for the Custom Command Modal interface.
    /// </summary>
    public void Activate()
    {
      // Perform Job Validations.  Return if any fail.
      if(!JobAttributesAreValid)
      {
        return;
      }

      if(CompanyNumbersAreEmpty)
      {
        return;
      }

      // Validate pending job edits
      if(!ResolveJobValidationErrors())
      {
        return;
      }

     SharedWriteBackLibrary swbl = new SharedWriteBackLibrary();

     if (!swbl.ValidateWorkPoints())
      {
         return;
      }
            // Check for existing conflicts
      if (ConflictsExist())
      {
        return;
      }

      // Post edits for active job
      if(!PostJobEdits())
      {
        return;
      }

      // If plotting boundaries exist, then attempt plot sheet generation; however,
      // if they don't exist, then bypass generating and storing the archival prints completely.

            IGTApplication app = GTClassFactory.Create<IGTApplication>();
      string sql = "select count(1) from plotbnd_n where job_id=? and product_type='Construction Print'";
      Recordset rs = app.DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText, app.DataContext.ActiveJob);
      // This SQL will always return a record.
      short boundaryCount = Convert.ToInt16(rs.Fields[0].Value);
      rs.Close();
      rs = null;

      if(0 < boundaryCount)
      {
        // Generate the construction prints
        string constructionPrints = string.Empty;

        if(GenerateConstructionPrints(out constructionPrints))
        {
          // There is currently no provision in the design to check whether the construction prints
          // are generated.  Provided there is no processing failure (which will be trapped in an exception
          // and cause the previous call to generate the prints to return a false),
          // then since the validations have already checked for the existence of a plotting boundary,
          // it will be assumed that at least one plot was generated.

          // Attach the construction prints generated above
          if(!AttachConstructionPrints(constructionPrints))
          {
            string msg = string.Format("Failed to upload construction prints for archival.{0}{0}Proceed anyway?", System.Environment.NewLine);

            if(DialogResult.No == MessageBox.Show(msg, "G/Technology", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
              return;
            }
          }
        }
        else
        {
          string msg = string.Format("Failed to generate construction prints for archival.{0}{0}Proceed anyway?", System.Environment.NewLine);

          if(DialogResult.No == MessageBox.Show(msg, "G/Technology", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
          {
            return;
          }
        }
      }


      // Cache that here for use with the SetJobStatus interface (in case something deactivates the job before then).
      JobManager jm = new JobManager();
      this.ActiveWRNbr = jm.WRNumber;

      // Invoke a Writeback if needed.
      if(!DoWriteBack(swbl))
      {
        return;
      }

      // Invoke the ClosurePending status update
      // and, if successful, set the local Job Status to ClosurePending.
      WMISStatus wMISStatus = new WMISStatus();
      wMISStatus.SetJobStatus(this.ActiveWRNbr.ToString(), closurePending);

      // Deactivate the active job.
      // The argument determines whether to keep the job associated with the workspace in read-only mode.
      // Design doesn't state one way or the other but going with true.
      jm.DeactivateJob(true);
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

          if(jobManager.JobType.ToUpper() == "NON-WR" || jobManager.JobType.ToUpper() == "WR-ESTIMATE")
          {
            MessageBox.Show("This command applies only to WR jobs (excluding graphic estimates).", "G/Technology", MessageBoxButtons.OK);
            return false;
          }

          if(jobManager.JobStatus.ToUpper() != "CONSTRUCTIONCOMPLETE")
          {
            MessageBox.Show("Job Status must be ConstructionComplete in order to mark for closure.", "G/Technology", MessageBoxButtons.OK);
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
          MessageBox.Show("Some edits conflict with other posted jobs; please run Conflict Detection to resolve before marking for closure.", "G /Technology", MessageBoxButtons.OK);
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
          rs.Close();
          rs = null;

          switch(highestErrorPriority)
          {
            case "P1":
              MessageBox.Show("P1 errors must be resolved before marking WR for closure.", "G/Technology", MessageBoxButtons.OK);
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
              DialogResult dialogResult = MessageBox.Show("Validation warnings encountered; proceed with marking for closure?", "G/Technology", MessageBoxButtons.OKCancel);
              retVal = dialogResult == DialogResult.OK;
              break;
          }
        }
        else
        {
          // No validation errors will result in a NULL recordset.

          // A non-null recordset with no records should never occur,
          // but just in case, close it if it is open.
          if(null != rs && rs.State == Convert.ToInt32(ObjectStateEnum.adStateOpen))
          {
            rs.Close();
            rs = null;
          }

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
    /// True if any DEIS features for the active job have missing company numbers AND the user does not elect to continue; else, false
    /// Displays message box with a list (max 10) of any features in the active job that have a missing company number.
    /// If any company numbers are missing from the active job, user can elect to ignore this and continue with Mark for Closure.
    /// </summary>
    private bool CompanyNumbersAreEmpty
    {
      get
      {
        try
        {
          // Assume no company numbers are missing in the job.
          // If any are missing, return value will depend on user response.
          bool retVal = false;

          IGTJobManagementService jobService = GTClassFactory.Create<IGTJobManagementService>();
          IGTApplication App = GTClassFactory.Create<IGTApplication>();
          jobService.DataContext = App.DataContext;

          // Validated pending edits are returned in recordset
          Recordset rs = jobService.FindPendingEdits();
          if(null != rs && 0 < rs.RecordCount)
          {
            // Autotransformer Units, OH Transformer Units, UG Transformer Units, Voltage Regulator Units
            rs.Filter = "g3e_cno=3402 or g3e_cno=3602 or g3e_cno=5902 or g3e_cno=6002";

            // If there are any of the above components in the pending edits recordset, then check their company numbers.
            if(0 < rs.RecordCount)
            {
              rs.MoveFirst();

              List<string> features = new List<string>();

              // Iterate the recordset adding FIDs with empty Company Numbers to a list.
              do
              {
                short fno = Convert.ToInt16(rs.Fields["g3e_fno"].Value);
                int fid = Convert.ToInt32(rs.Fields["g3e_fid"].Value);
                short cno = Convert.ToInt16(rs.Fields["g3e_cno"].Value);
                int cid = Convert.ToInt32(rs.Fields["g3e_cid"].Value);

                if(CompanyNumberIsEmpty(cno, fid, cid))
                {
                  string feature = string.Format("{0} ({1})", FeatureNameByFNO(fno), fid.ToString());
                  features.Add(feature);
                }

                rs.MoveNext();
              } while(!rs.EOF);

              // If any features were added to the list, display them in a message box
              if(0 < features.Count)
              {
                // grammar...
                string tmp = 1 == features.Count ? " has" : "s have";
                string msg = string.Format("The following feature{0} not have a Company Number assigned:{1}", tmp, Environment.NewLine);

                short ctr = 0;
                foreach(string feature in features)
                {
                  msg = string.Format("{0}{1}{2}", msg, Environment.NewLine, feature);
                  if(++ctr > 10)
                  {
                    ctr--;
                    break;
                  }
                }

                if(features.Count > ctr)
                {
                  msg = string.Format("{0}{1}{1}The above is only a partial list.{1}There is a total of {2} features that have a missing Company Number.", msg, Environment.NewLine, features.Count.ToString());
                }

                msg = string.Format("{0}{1}{1}Mark for Closure anyway?", msg, Environment.NewLine);

                // If user responds with No, then this function will return true.
                // If user responds with Yes, then even though there are empty company numbers,
                // this function will return false which will allow the commmand to continue just as if there were no empty values.
                retVal = DialogResult.No == MessageBox.Show(msg, "G/Technology", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
              }
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
    }

    /// <summary>
    /// Checks a feature's component instance for a missing company number.
    /// </summary>
    /// <param name="CNO"></param>
    /// <param name="FID"></param>
    /// <param name="CID"></param>
    /// <returns>true if Company Number is missing; else false</returns>
    private bool CompanyNumberIsEmpty(short CNO, int FID, int CID)
    {
      try
      {
        bool isEmpty = false;

        string sql = string.Format("select company_id from {0} where g3e_fid=? and g3e_cid=?", TableNameByCNO(CNO));
        IGTApplication app = GTClassFactory.Create<IGTApplication>();
        Recordset rs = app.DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, FID, CID);

        if(null != rs && 1 == rs.RecordCount)
        {
          isEmpty = System.DBNull.Value == rs.Fields[0].Value;
        }

        return isEmpty;
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
    /// Post the active job edits using LTT_POST.POST
    /// </summary>
    /// <returns></returns>
    private bool PostJobEdits()
    {
      try
      {
        // Since validations have already been performed, using the LTT_POST.POST to avoid rechecking validations, etc.
        IGTApplication app = GTClassFactory.Create<IGTApplication>();
        app.DataContext.Execute("begin ltt_post.post;end;", out int recs, (int)CommandTypeEnum.adCmdText);
        return true;
      }
      catch(Exception ex)
      {
        string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
        throw new Exception(exMsg);
      }
    }


    /// <summary>
    /// Checks for the active job's WriteBackNeeded flag.  If set, then request the writeback.
    /// Successful initiation of a writeback process will display a dialog box that the user may dismiss but leaves the writeback process running.
    /// If Writeback fails to initialize, exit command.
    /// </summary>
    private bool ProcessWriteBack()
    {
      try
      {
        bool retVal = true;

        return retVal;
      }
      catch(Exception ex)
      {
        string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
        throw new Exception(exMsg);
      }
    }

    /// <summary>
    /// Generates the construction prints (plots) for the active job.  Generates a new string List for the prints parameter.
    /// </summary>
    /// <param name="prints">string that will contain the path/file name of the generated plots.</param>
    /// <returns>true if successful; else, false</returns>
    private bool GenerateConstructionPrints(out string prints)
    {
      bool retVal = false;
      prints = string.Empty;

      try
      {
        IGTApplication app = GTClassFactory.Create<IGTApplication>();
        CommonMapPrintingHelper mpHelper = new CommonMapPrintingHelper(app);
        string tmpFileName = string.Format("{0}-Close-WorkPrint", app.DataContext.ActiveJob);
        string tmpFilePath = System.IO.Path.GetTempPath();

        #region Code in this region to be removed entirely once Plotting Template(s) are available.

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Until we get plot templates, then check whether the "Construction Print" named plot exists.
        // If it doesn't, then create one to serve as a temporary solution for a missing plot sheet.
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        bool needNamedPlot = true;

        foreach(IGTNamedPlot namedPlot in app.NamedPlots)
        {
          if(namedPlot.Name == "Construction Print")
          {
            needNamedPlot = false;
          }
        }

        // If the Construction Print named plot doesn't exist, then create one.
        if(needNamedPlot)
        {
          IGTNamedPlot np = app.NewNamedPlot("Construction Print");
          IGTPlotWindow pw = app.NewPlotWindow(np);

          IGTPoint frameTopLeft = GTClassFactory.Create<IGTPoint>();
          frameTopLeft.X = 0.0;
          frameTopLeft.Y = 0.0;

          IGTPoint frameBottomRight = GTClassFactory.Create<IGTPoint>();
          frameBottomRight.X = 20000.0;
          frameBottomRight.Y = 30000.0;

          IGTPaperRange paperRange = GTClassFactory.Create<IGTPaperRange>();
          paperRange.TopLeft = frameTopLeft;
          paperRange.BottomRight = frameBottomRight;

          IGTPlotMap map = pw.InsertMap(paperRange);
        }
        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // For now, defaulting the legend name to Distribution Design Legend for these prints; however,
        // this may be enhanced to determine which legend to use based on the WR type.
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        if(mpHelper.GenerateConstructionPlots("Distribution Design Legend", tmpFileName, tmpFilePath))
        {
          prints = string.Format("{0}\\{1}", tmpFilePath, tmpFileName);
          retVal = true;
        }

      }
      catch(Exception ex)
      {
        string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
        MessageBox.Show(exMsg, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }

      return retVal;
    }

    /// <summary>
    /// Attaches the construction prints, passed in as the prints parameter, to the active job.
    /// </summary>
    /// <param name="prints">File path/name of generated construction prints.</param>
    /// <returns>true if successful; else, false</returns>
    private bool AttachConstructionPrints(string prints)
    {
      bool retVal = false;

      try
      {
        IGTApplication app = GTClassFactory.Create<IGTApplication>();
        AttachWRDocument attachWRDocument = new AttachWRDocument();
        TransactionManager.Begin("Attach Archival Prints");

        if(attachWRDocument.AttachDocument(app.DataContext.ActiveJob, prints, "Work Prints Design"))
        {
          TransactionManager.Commit();
          retVal = true;
        }
        else
        {
          TransactionManager.Rollback();
        }

      }
      catch(Exception ex)
      {
        string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
        MessageBox.Show(exMsg, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }

      return retVal;
    }

    /// <summary>
    /// Checks writeback flag and, if it is set, starts an asynchronous writeback to WMIS process
    /// </summary>
    /// <returns>true if successful; else, false</returns>
    private bool DoWriteBack(SharedWriteBackLibrary p_swbl)
    {
      try
      {
        JobManager jobManager = new JobManager();

        // ALM 1566 - Requests removal of the check for the write back flag.
        //            Future design change will check for this in a different way
        //            but leaving the conditional statement for clarity until
        //            that change is implemented.
        if(true /*jobManager.WriteBackNeeded*/)
        {
                    // The shared library does the actual writeback.

          // Asynchronous call to perform the writeback.
          Guid taskID = Guid.NewGuid();
                    p_swbl.UpdateWriteBack(jobManager.ActiveJob, taskID);


                    // Subscribed to writeback completed event so no longer need a pointer to the library object.
                    p_swbl = null;
        }

        return true;
      }
      catch(Exception ex)
      {
        string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
        throw new Exception(exMsg);
      }
    }

    // Once the replacement (shared code) is confirmed as working properly, these two methods can be removed.  18-DEC-18 - cbs
    /////// <summary>
    /////// Calls the shared writeback code to set the ApprovalPending job status
    /////// </summary>
    ////private void SetClosurePendingJobStatus()
    ////{
    ////  try
    ////  {
    ////    SharedWriteBackLibrary swbl = new SharedWriteBackLibrary();
    ////    swbl.UpdateJobStatusProcessCompleted += swbl_UpdateJobStatusProcessCompleted;

    ////    Guid taskID = Guid.NewGuid();
    ////    swbl.UpdateJobStatus(this.ActiveWRNbr.ToString(), closurePending, taskID);

    ////    //Subscribed to the UpdateJobStatusProcessCompleted event so no longer need pointer to the object.
    ////    swbl = null;
    ////  }
    ////  catch(Exception ex)
    ////  {
    ////    string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
    ////    throw new Exception(exMsg);
    ////  }
    ////}

    /////// <summary>
    /////// Event handler for the Request to Update Job Status completed event
    /////// </summary>
    /////// <param name="sender"></param>
    /////// <param name="e">UpdateStatusCompleteEventArgs</param>
    ////private void swbl_UpdateJobStatusProcessCompleted(object sender, UpdateStatusCompleteEventArgs e)
    ////{
    ////  try
    ////  {
    ////    // Shared component should return either SUCCESS or FAILURE in the event argument (Status).
    ////    // If SUCCESS, update the Job Status silently.  Notify the user only if FAILURE or other errors.

    ////    switch(e.Status.ToUpper())
    ////    {
    ////      case "SUCCESS":
    ////        // Can't use the JobManager here since job is not active at this point.
    ////        string sql = "update g3e_job set g3e_jobstatus=? where wr_nbr=?";
    ////        IGTApplication app = GTClassFactory.Create<IGTApplication>();
    ////        app.DataContext.Execute(sql, out int recs, (int)CommandTypeEnum.adCmdText, closurePending, this.ActiveWRNbr);

    ////        app.DataContext.Execute("commit", out recs, (int)CommandTypeEnum.adCmdText);

    ////        break;

    ////      case "FAILURE":
    ////        if(null != e.Error && !string.IsNullOrEmpty(e.Error.Message))
    ////        {
    ////          MessageBox.Show(string.Format("Request to update Job Status failed: {0}.", e.Error.Message), "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
    ////        }
    ////        else
    ////        {
    ////          MessageBox.Show("Request to update Job Status failed.  No additional information is available.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
    ////        }

    ////        break;

    ////      default:
    ////        // Assume e.Error.Message is not reliably set in this case so not displaying it.
    ////        string msg = string.Format("{0}{1}{2}{3}", "Request to update Job Status returned an unexpected status value: ", e.Status, Environment.NewLine, "Status of writeback is unknown.");
    ////        MessageBox.Show(msg, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
    ////        break;
    ////    }
    ////  }
    ////  catch(Exception ex)
    ////  {
    ////    string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
    ////    throw new Exception(exMsg);
    ////  }
    ////}

  }
}
