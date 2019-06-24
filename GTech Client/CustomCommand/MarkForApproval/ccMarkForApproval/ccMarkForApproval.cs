using System;
using System.Collections.Generic;
using System.Linq;
using Intergraph.GTechnology.Interfaces;
using Intergraph.GTechnology.API;
using System.Windows.Forms;
using ADODB;
using CustomWriteBackLibrary;

namespace GTechnology.Oncor.CustomAPI
{
  /// <summary>
  /// Class to implement the modal custom command interface for the Mark for Approval command.
  /// </summary>
  public class ccMarkForApproval : IGTCustomCommandModal
  {

    /// <summary>
    /// The confirmation dialog that is displayed once the writeback is started.
    /// </summary>

    /// <summary>
    /// Added to ensure same string is used throughout this class
    /// </summary>
    private const string approvalPending = "ApprovalPending";

    /// <summary>
    /// Event handler for the Shared Write Back Library SetApprovalPendingJobStatus
    /// needs this value to set the WR status since the job is deactivated at that point.
    /// </summary>
    private int ActiveWRNbr
    { set; get; }

    private string ActiveJobID
    { set; get; }

    /// <summary>
    /// Transaction Manager - not used for this command but necessary part of the interface's implementation.
    /// </summary>
    public IGTTransactionManager TransactionManager { get; set; }

    /// <summary>
    /// Entry method for the Custom Command Modal interface.
    /// </summary>
    public void Activate()
    {
      // Perform Job Property Validations.
      if(!JobPropertiesAreValid())
      {
        return;
      }

      // Check/Resolve any alternate jobs with pending Approval status.
      if(AlternatesPendingApprovalExist())
      {
        return;
      }

      // Validate pending job edits      
      if(!ResolveJobValidationErrors())
      {
        return;
      }

      // Check for existing conflicts
      if(ConflictsExist())
      {
        return;
      }

      // Check for concurrent edits involving jobs that are pending approval and exit if any found.
      if(ConcurrentEditsExistWithApprovalPending())
      {
        return;
      }

      // Check for out of synch WPs and exit if they are not synchronized
      SharedWriteBackLibrary swbl = new SharedWriteBackLibrary();
      if (!swbl.ValidateWorkPoints())
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

      // Cache these here for use with the SetJobStatus interface (in case something deactivates the job before then).
      JobManager jm = new JobManager();
      this.ActiveWRNbr = jm.WRNumber;
      this.ActiveJobID = jm.ActiveJob;

      if(jm.NullNumericFieldValue)
      {
        throw new Exception("The WR value for this job is NULL");
      }

      // Check for writeback flag and perform writeback if set
      if(!DoWriteBack(swbl))
      {
        return;
      }

      // Request job status change to ApprovalPending
      // and set job status to ApprovalPending if request is successful.
      WMISStatus wMISStatus = new WMISStatus();
      wMISStatus.SetJobStatus(this.ActiveJobID, this.ActiveWRNbr.ToString(), approvalPending);
    }

    /// <summary>
    /// Returns true if job properties are valid; otherwise, false.
    /// </summary>
    public bool JobPropertiesAreValid()
    {
      try
      {
        JobManager jobManager = new JobManager();

        // Comparing this way to prevent making multiple calls to the database to get the job type.
        string[] invalidJobtypes = new string[] { "NON-WR", "WR-ESTIMATE" };

        if(invalidJobtypes.Contains(jobManager.JobType.Trim().ToUpper()))
        {
          MessageBox.Show("This command applies only to WR jobs (excluding graphic estimates).", "G/Technology", MessageBoxButtons.OK);
          return false;
        }

        if(jobManager.JobStatus.ToUpper() != "DESIGN")
        {
          MessageBox.Show("Job Status must be Design in order to mark for approval.", "G/Technology", MessageBoxButtons.OK);
          return false;
        }

        string wmisStatus = jobManager.WMISStatus.ToUpper().Trim();

        if(wmisStatus == "ERROR")
        {
          MessageBox.Show("Interface error must be resolved before proceeding.", "G/Technology", MessageBoxButtons.OK);
          return false;
        }

        if(!string.IsNullOrEmpty(wmisStatus) && wmisStatus != "SUCCESS" && wmisStatus != "FAILURE")
        {
          MessageBox.Show("Interface is in use for this WR.", "G/Technology", MessageBoxButtons.OK);
          return false;
        }

        IGTApplication app = GTClassFactory.Create<IGTApplication>();

        string sql = "select count(1) from designarea_p where job_id=?";
        Recordset rs = app.DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText, jobManager.ActiveJob);
        // This SQL will always return a record.
        if(0 == Convert.ToInt16(rs.Fields[0].Value))
        {
          MessageBox.Show("This command requires a Design Area to be placed for the active job.", "G/Technology", MessageBoxButtons.OK);
          rs.Close();
          rs = null;
          return false;
        }


        sql = "select count(1) from plotbnd_n where job_id=? and product_type='Construction Print'";
        rs = app.DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText, jobManager.ActiveJob);
        // This SQL will always return one record with one non-null field.
        short boundaryCount = Convert.ToInt16(rs.Fields[0].Value);
        rs.Close();
        rs = null;

        if(0 == boundaryCount)
        {
          //MessageBox.Show("At least one Plotting Boundary is required prior to marking for approval.", "G/Technology", MessageBoxButtons.OK);
          //return false;

          string msg = string.Format("{0}{1}{1}{2}{1}{1}{3}",
                                      "No Construction Print Plotting Boundaries exist for the active WR job.",
                                      System.Environment.NewLine,
                                      "Without at least one Construction Print Plotting Boundary, Archival Prints will not be generated/uploaded.",
                                      "Proceed anyway?");
          return DialogResult.Yes == MessageBox.Show(msg, "G/Technology", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        return true;
      }
      catch(Exception ex)
      {
        string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
        throw new Exception(exMsg);
      }
    }

    /// <summary>
    /// Runs job validation and analyzes/resolves returned error recordset.
    /// </summary>
    /// <returns>true if no P1s exist and any P2s are overridden; else, false</returns>
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
              MessageBox.Show("P1 errors must be resolved before marking WR for approval.", "G/Technology", MessageBoxButtons.OK);
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
              DialogResult dialogResult = MessageBox.Show("Validation warnings encountered; proceed with marking for approval?", "G/Technology", MessageBoxButtons.OKCancel);
              retVal = dialogResult == DialogResult.OK;
              break;
          }

          rs.Close();
          rs = null;
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
    /// Checks for any alternates for active job are pending approval.
    /// </summary>
    /// <returns>true of no alternates are pending approval or if alternates found and user elects to unmark; else, false</returns>
    public bool AlternatesPendingApprovalExist()
    {
      try
      {
        bool retVal = false;

        JobManager jobManager = new JobManager();

        if(jobManager.AlternatesExistPendingApproval)
        {
          DialogResult dialogResult = MessageBox.Show("An alternate design for this WR is marked for approval. Unmark it so that this alternate may be marked instead?", "G/Technology", MessageBoxButtons.YesNo);
          if(dialogResult == DialogResult.Yes)
          {
            List<string> alternatesPending = jobManager.AlternatesPendingApproval;
            foreach(string alternate in alternatesPending)
            {
              jobManager.UpdateJobField(alternate, "G3E_JOBSTATUS", "Design");
            }
          }
          else
          {
            MessageBox.Show("Job is being deactivated and the alternate design will remain marked for approval.", "G/Technology", MessageBoxButtons.OK);
            jobManager.DeactivateJob(true);
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
    /// Runs conflict detection for active job.
    /// </summary>
    /// <returns>true if conflicts exist; else, false</returns>
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
          MessageBox.Show("Some edits conflict with other posted jobs; please run Conflict Detection to resolve before marking for approval.", "G /Technology", MessageBoxButtons.OK);
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
    /// Checks for concurrent edits with jobs that have job status = ApprovalPending.
    /// </summary>
    /// <returns>true if qualifying jobs are found; else false</returns>
    private bool ConcurrentEditsExistWithApprovalPending()
    {
      try
      {
        bool retVal = false;

        IGTApplication app = GTClassFactory.Create<IGTApplication>();
        IGTJobManagementService jobService = GTClassFactory.Create<IGTJobManagementService>();
        jobService.DataContext = app.DataContext;
        Recordset rs = jobService.FindPendingEdits();

        if(null != rs && 0 < rs.RecordCount)
        {
          rs.MoveFirst();
          List<int> fids = new List<int>();
          List<short> fnos = new List<short>();

          do
          {
            // Just need a distinct set of FIDs and their FNOs
            int fid = Convert.ToInt32(rs.Fields["G3E_FID"].Value);

            if(!fids.Contains(fid))
            {
              fids.Add(fid);
              fnos.Add(Convert.ToInt16(rs.Fields["G3E_FNO"].Value));
            }

            rs.MoveNext();

          } while(!rs.EOF);

          rs.Close();
          rs = null;

          // Use the list of FIDs and FNOs to run Concurrent Edit Detection.
          IGTConcurrentEditDetection concEditDetect = GTClassFactory.Create<IGTConcurrentEditDetection>();

          if(0 < concEditDetect.CheckFeatures(fnos.ToArray(), fids.ToArray(), false, true, false))
          {
            // CONCURRENT_EDITS is populated by the CheckFeatures method.
            // Query it for the other jobs that have concurrently-edited features with this job

            string sql = string.Format("select count(1) from concurrent_edits ce join g3e_job j on ce.source_job=j.g3e_identifier where ce.job_name=? and j.g3e_jobstatus='{0}'", approvalPending);
            rs = app.DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText, app.DataContext.ActiveJob);

            // If the above query returns a count greater than zero, then
            // issue the error and set return value to true.
            if(null != rs && 0 < rs.RecordCount)
            {
              rs.MoveFirst();

              if(0 < Convert.ToInt16(rs.Fields[0].Value))
              {
                retVal = true;
                MessageBox.Show("Concurrent edits exist in another job already pending approval.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
              }

              rs.Close();
              rs = null;
            }
          }
        }
        else
        {
          if(null != rs && rs.State == (int)System.Data.ConnectionState.Open)
          {
            rs.Close();
          }
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
        prints = string.Empty;
        IGTApplication app = GTClassFactory.Create<IGTApplication>();
        CommonMapPrintingHelper mpHelper = new CommonMapPrintingHelper(app);
        string tmpFileName = string.Format("{0}-Design-WorkPrints", app.DataContext.ActiveJob);
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
          prints = string.Format("{0}\\{1}.pdf", tmpFilePath, tmpFileName);
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
          // Once complete, it will call the WriteBackProcessCompleted event.
       //   SharedWriteBackLibrary swbl = new SharedWriteBackLibrary();
         // swbl.WriteBackProcessCompleted += swbl_WriteBackProcessCompleted;

          // Asynchronous call to perform the writeback.
          Guid taskID = Guid.NewGuid();
          p_swbl.UpdateWriteBack(jobManager.ActiveJob, taskID);


          // Subscribed to writeback completed event so no longer need a pointer to the library object.
          p_swbl = null;

          // Deactivate the active job.
          // The argument determines whether to keep the job associated with the workspace in read-only mode.
          // Design doesn't state one way or the other but going with true.
          jobManager.DeactivateJob(true);
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
}

