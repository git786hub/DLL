using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ADODB;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using CustomWriteBackLibrary;

namespace GTechnology.Oncor.CustomAPI
{
  /// <summary>
  /// Implementation of the Write to WMIS custom command
  /// </summary>
  public class WriteToWMIS : IGTCustomCommandModal
  {
    /// <summary>
    /// The confirmation dialog that is displayed once the writeback is started.
    /// </summary>
    /// <summary>
    /// The Writeback is running if G3E_JOB.WMIS_STATUS_C is not SUCCESS or FAILURE for the active job.
    /// </summary>
    private bool WriteBackIsRunning
    {
      get
      {
        JobManager jm = new JobManager();
        string wmisStatus = jm.WMISStatus;
        jm = null;

        return wmisStatus != string.Empty && wmisStatus != "SUCCESS" && wmisStatus != "FAILURE";
      }
    }

    /// <summary>
    /// G/Tech's transaction manager.  Not used in this command.
    /// </summary>
    public IGTTransactionManager TransactionManager { set { /* This command doesn't require a  Transaction Manager. */ } }

    /// <summary>
    /// G/Tech calls this method when the custom command is invoked.
    /// </summary>
    public void Activate()
    {
      // The Writeback is running for this session if the job's WMIS Status is not empty, SUCCESS, or FAILURE
      // If it is running, then exit.

      if(WriteBackIsRunning)
      {
        string msg = string.Format("{0}{1}{2}",
          "Writeback is currently running in this session.",
          Environment.NewLine,
          "Please wait until the current writeback is complete before starting another one.");
        MessageBox.Show(msg, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        return;
      }

      // Test all the conditions for a writeback.
      // Exit if any conditions fail.
      if(!validateJob())
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

      // Check for out of synch WPs and exit if they are not synchronized
       SharedWriteBackLibrary swbl = new SharedWriteBackLibrary();
       if (!swbl.ValidateWorkPoints())
       {
         return;
       }
            //// Evaluate the WriteBack flag and prompt user whether to process accordingly
            //JobManager jobManager = new JobManager();

            //if(jobManager.WriteBackNeeded)
            //{
            //  if(DialogResult.Yes == MessageBox.Show("Write job data to WMIS?", "G/Technology", MessageBoxButtons.YesNo))
            //  {
            //    startWriteback();
            //  }
            //}
            //else
            //{
            //  if(DialogResult.Yes == MessageBox.Show("Accounting data has not changed since the previous write to WMIS.  Proceed anyway?", "G/Technology", MessageBoxButtons.YesNo))
            //  {
            //    startWriteback();
            //  }
            //}

            // ALM 1566 eliminates use of the WriteBack flag here; however, there are other methods which, when designed and implemented
            // will be used in lieu of that flag.  Leaving the prompts, etc., commented above as they may still be used with the new methods.
            // For now, just call the WriteBack with no other checks.
            startWriteback(swbl);
    }

    /// <summary>
    /// Check all the validation criteria.
    /// </summary>
    /// <returns>true if all validations pass; else false</returns>
    private bool validateJob()
    {
      bool isValid = true;

      JobManager jm = new JobManager();

      if(jm.JobType.ToUpper() == "NON-WR")
      {
        MessageBox.Show("This command applies only to WR jobs.", "G/Technology", MessageBoxButtons.OK);
        isValid = false;
      }

      // Get a list of alternates pending approval.
      // There should never be more than one but getting a list "just in case..."
      List<string> alts = jm.AlternatesPendingApproval;

      // If any alternates are Pending Approval, then prompt to unmark.
      if(isValid && 0 < alts.Count)
      {
        switch(MessageBox.Show("An alternate design for this WR is marked for approval.  Unmark it so that this alternate may be written to WMIS?", "G/Technology", MessageBoxButtons.YesNo))
        {
          case DialogResult.Yes:
            // Update any alternates
            foreach(string alt in alts)
            {
              // In case there are multiples (should NEVER happen but...), delay commit until after all updates.
              jm.UpdateJobField(alt, "g3e_jobstatus", "Design");
            }
            commit();
            break;
          case DialogResult.No:
            // If alternate exists with approval pending and user elects not to unmark, then deactivate the job,
            // consider this as a validation failure, and command will exit when returning from this method.
            MessageBox.Show("Job is being deactivated and the alternate design will remain marked for approval.", "G/Technology", MessageBoxButtons.OK);
            jm.DeactivateJob(true);
            isValid = false;
            break;
        }
      }

      return isValid;
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
    /// Runs job validation and analyzes returned error recordset.
    /// </summary>
    /// <returns>true if no P1/P2 errors exist or if user elects to continue with P1/P2 errors; else, else false</returns>
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

          ValidationErrorDisplay ved = new ValidationErrorDisplay();
          ved.Errors = rs;
          ved.StartPosition = FormStartPosition.CenterParent;
          ved.TopLevel = true;
          ved.Show();

          rs.Filter = "ErrorPriority='P1' or ErrorPriority='P2'";

          if(0 < rs.RecordCount)
          {
            // If Yes, then returns true
            retVal = DialogResult.Yes == MessageBox.Show("Validation errors have been detected.  Proceed anyway?", "G/Technology", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
          }

          rs.Close();
          rs = null;
        }
        else
        {
          // No validation errors so return true
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
    /// Calls the shared writeback code to perform the actual writeback.
    /// Displays the confirmation dialog box which the user can dismiss if desired (writeback will continue).
    /// Subscribes to the WriteBackProcessCompleted event so user can be notified when process completes.
    /// </summary>
    private void startWriteback(SharedWriteBackLibrary p_swbl)
    {
      JobManager jm = new JobManager();

      // The shared library does the actual writeback.
      IGTApplication app = GTClassFactory.Create<IGTApplication>();
      // Asynchronous call to perform the writeback.
      Guid taskID = Guid.NewGuid();
      p_swbl.UpdateWriteBack(jm.ActiveJob, taskID);
      p_swbl = null;

      // Deactivate the active job.
      // The argument determines whether to keep the job associated with the workspace in read-only mode.
      // Design doesn't state one way or the other but going with true.
      jm.DeactivateJob(true);
      jm = null;
    }

    /// <summary>
    /// Commits database changes.
    /// </summary>
    private void commit()
    {
      IGTApplication app = GTClassFactory.Create<IGTApplication>();
      app.DataContext.Execute("commit", out int recs, (int)CommandTypeEnum.adCmdText);
    }

  }

}
