using System;
using System.Windows.Forms;
using Intergraph.GTechnology.API;
using ADODB;
using CustomWriteBackLibrary;

namespace GTechnology.Oncor.CustomAPI
{
  /// <summary>
  /// Incorporates the call to update the WMIS status and the local job status for the WR and status parameters
  /// </summary> 
  public class WMISStatus
  {
    /// <summary>
    /// Calls the shared writeback code to set the indicated job status for the indicated WR
    /// </summary>
    /// <param name="WR">WR to update</param>
    /// <param name="status">Status value to set</param>
    public void SetJobStatus(string WR, string status)
    {
      try
      {
        SharedWriteBackLibrary swbl = new SharedWriteBackLibrary();
        swbl.UpdateJobStatusProcessCompleted += swbl_UpdateJobStatusProcessCompleted;

        Guid taskID = Guid.NewGuid();
        swbl.UpdateJobStatus(WR, status, taskID);

        //Subscribed to the UpdateJobStatusProcessCompleted event so no longer need pointer to the object.
        swbl = null;

        // As long as the request to update the Job Status is successful (regardless of whether the process completes successfully),
        // then update G3E_JOB.G3E_JOBSTATUS for the active WR.
        // Can't use the JobManager here since job may not active at this point.
        string sql = "update g3e_job set g3e_jobstatus=? where wr_nbr=?";
        IGTApplication app = GTClassFactory.Create<IGTApplication>();
        app.DataContext.Execute(sql, out int recs, (int)CommandTypeEnum.adCmdText, status, WR);

        app.DataContext.Execute("commit", out recs, (int)CommandTypeEnum.adCmdText);
      }
      catch(Exception ex)
      {
        string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
        throw new Exception(exMsg);
      }
    }

    /// <summary>
    /// Calls the shared writeback code to set the indicated job status for the indicated WR
    /// This overload is needed for the Mark for Approval because it needs to set only the active
    /// job's Job Status.  If there are alternates, then merely using the WR sets the JobStatus for
    /// all the alternates for the WR and only the identified job ID's status should be set.
    /// </summary>
    /// <param name="JobID">Job Identifier for which to set JobStatus</param>
    /// <param name="WR">WR to update</param>
    /// <param name="status">Status value to set</param>
    public void SetJobStatus(string JobID, string WR, string status)
    {
      try
      {
        SharedWriteBackLibrary swbl = new SharedWriteBackLibrary();
        swbl.UpdateJobStatusProcessCompleted += swbl_UpdateJobStatusProcessCompleted;

        Guid taskID = Guid.NewGuid();
        swbl.UpdateJobStatus(WR, status, taskID);

        //Subscribed to the UpdateJobStatusProcessCompleted event so no longer need pointer to the object.
        swbl = null;

        // As long as the request to update the Job Status is successful (regardless of whether the process completes successfully),
        // then update G3E_JOB.G3E_JOBSTATUS for the indicated Job ID.
        // Can't use the JobManager here since job may not active at this point.
        string sql = "update g3e_job set g3e_jobstatus=? where g3e_identifier=?";
        IGTApplication app = GTClassFactory.Create<IGTApplication>();
        app.DataContext.Execute(sql, out int recs, (int)CommandTypeEnum.adCmdText, status, JobID);

        app.DataContext.Execute("commit", out recs, (int)CommandTypeEnum.adCmdText);
      }
      catch(Exception ex)
      {
        string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
        throw new Exception(exMsg);
      }
    }

    /// <summary>
    /// Event handler for the Request to Update Job Status completed event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e">UpdateStatusCompleteEventArgs</param>
    private void swbl_UpdateJobStatusProcessCompleted(object sender, UpdateStatusCompleteEventArgs e)
    {
      try
      {
        // Shared component should return either SUCCESS or FAILURE in the event argument (Status).
        // If SUCCESS, update the Job Status silently.  Notify the user only if FAILURE or other errors.

        GUIMode guiMode = new GUIMode();

        switch(e.Status.ToUpper())
        {
          case "SUCCESS":
            // Nothing to do here.
            break;

          case "FAILURE":
            if(guiMode.InteractiveMode)
            {
              if(null != e.Error && !string.IsNullOrEmpty(e.Error.Message))
              {
                MessageBox.Show(string.Format("Request to update Job Status failed: {0}.", e.Error.Message), "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
              }
              else
              {
                MessageBox.Show("Request to update Job Status failed.  No additional information is available.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
              }
            }

            break;

          default:
            if(guiMode.InteractiveMode)
            {
              // Assume e.Error.Message is not reliably set in this case so not displaying it.
              string msg = string.Format("{0}{1}{2}{3}", "Request to update Job Status returned an unexpected status value: ", e.Status, Environment.NewLine, "Status of writeback is unknown.");
              MessageBox.Show(msg, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            break;
        }
      }
      catch(Exception ex)
      {
        string exMsg = string.Format("Error occurred in {0} of {1}.{2}{3}", System.Reflection.MethodBase.GetCurrentMethod().Name, this.ToString(), Environment.NewLine, ex.Message);
        throw new Exception(exMsg);
      }
    }

    /// <summary>
    /// Determines whether system is in interactive or unattended mode
    /// based on the absence/presence of the UnattendedMode G/Technology property
    /// </summary>
    private bool InteractiveMode
    {
      get
      {
        bool interactiveMode = true;
        IGTApplication app = GTClassFactory.Create<IGTApplication>();

        foreach(string key in app.Properties.Keys)
        {
          if(0 == string.Compare(key, "unattendedmode", true))
          {
            interactiveMode = false;
            break;
          }
        }

        return interactiveMode;
      }
    }

  }
}
