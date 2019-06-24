//----------------------------------------------------------------------------+
//        Class: JobStatusUpdate
//  Description: This class holds necessary events and methods to process Update Job Status asynchronously.
//                                                                  
//----------------------------------------------------------------------------+
//          $Author:: Shubham Agarwal                                       $
//          $Date:: 25/03/18                                                $
//          $Revision:: 1                                                   $
//----------------------------------------------------------------------------+
//    $History:: JobStatusUpdateClass.cs                     $
// 
// *****************  Version 1  *****************
// User: sagarwal     Date: 25/03/18   Time: 18:00  Desc : Created
//----------------------------------------------------------------------------+

using Intergraph.GTechnology.API;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;


namespace CustomWriteBackLibrary
{
  public delegate void OnUpdateJobStatusCompletion(object sender, UpdateStatusCompleteEventArgs e);
  class JobStatusUpdate
  {
    IGTApplication m_App = null;
    public JobStatusUpdate(IGTApplication p_App, OnUpdateJobStatusCompletion p_OnUpdateEvent)
    {
      m_App = p_App;
      UpdateJobStatusProcessCompleted = p_OnUpdateEvent;
    }

    #region Update Job Status related methods


    public event OnUpdateJobStatusCompletion UpdateJobStatusProcessCompleted;

    private SendOrPostCallback onUpdateJobStatusCompleted;
    private delegate void UpdateJobStatusWorkerEventHandler(string wrNumber, string jobStatus, AsyncOperation asyncOp);

    private HybridDictionary userStateToLifetimeUpdateStatus = new HybridDictionary();

    protected virtual void InitializeUpdateJobStatusDelegates()
    {
      onUpdateJobStatusCompleted = new SendOrPostCallback(UpdateJobStatusCompleted);
    }
    /// <summary>
    /// This method will be called by the client to update job status and this will be launched asynchronously. Client needs to register event UpdateJobStatusProcessCompleted to get the notification of this method completion
    /// </summary>
    /// <param name="p_wrNumber">WR Number</param>
    /// <param name="p_jobStatus">Job Status</param>
    /// <param name="taskId">A new GUID needs to be passed for this parameter</param>
    public void UpdateJobStatus(string p_wrNumber, string p_jobStatus, object taskId)
    {
      InitializeUpdateJobStatusDelegates();
      AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(taskId);
      lock(userStateToLifetimeUpdateStatus.SyncRoot)
      {
        if(userStateToLifetimeUpdateStatus.Contains(taskId))
        {
          throw new ArgumentException(
              "Task ID parameter must be unique",
              "taskId");
        }

        userStateToLifetimeUpdateStatus[taskId] = asyncOp;
      }
      UpdateJobStatusWorkerEventHandler workerDelegate = new UpdateJobStatusWorkerEventHandler(UpdateJobStatusWorker);
      workerDelegate.BeginInvoke(
         p_wrNumber, p_jobStatus,
         asyncOp,
         null, null
         );
    }

    /// <summary>
    /// Method to the EF url for input parameter name and Sub system name
    /// </summary>
    /// <param name="paramName"> param name in the table</param>
    /// <param name="subSystemName"> subsystem name for the corresponsing param name</param>
    /// <returns>EF url</returns>

    private string GetCanonicalJobStatus(string p_JobStatus)
    {
      string jobStatus = string.Empty;

      switch(p_JobStatus)
      {
        case "ApprovalPending":
          jobStatus = "ApprovalPending";
          break;
        case "Design":
          jobStatus = "Design";
          break;
        case "ClosurePending":
          jobStatus = "ClosurePending";
          break;
        case "ConstructionComplete":
          jobStatus = "ConstructionComplete";
          break;
        default:
          break;
      }
      return jobStatus;
    }
    private void UpdateJobStatusWorker(string p_wrNumber, string p_jobStatus, AsyncOperation p_asyncOp)
    {
      string status = "SUCCESS";
      Exception ex = null;

      try
      {
        string jobStatus = GetCanonicalJobStatus(p_jobStatus);

        if(String.IsNullOrEmpty(jobStatus))
        {
          this.UpdateJobStatusCompletionMethod(status, ex, TaskCanceledUpdateJobStatus(p_asyncOp.UserSuppliedState), p_asyncOp);
          return;
        }

        ProcessJobStatusUpdate oProcessUpdate = new ProcessJobStatusUpdate(m_App);
        status = oProcessUpdate.ProcessUpdateJobStatus(p_wrNumber, p_jobStatus, out string errorInfo);

        if(status.Equals("FAILURE"))
        {
          ex = new Exception(errorInfo);
          status = "FAILURE";
        }
      }

      catch(Exception e)
      {
        ex = e;
        status = "FAILURE";
      }

      this.UpdateJobStatusCompletionMethod(status, ex, TaskCanceledUpdateJobStatus(p_asyncOp.UserSuppliedState), p_asyncOp);
    }


    private bool TaskCanceledUpdateJobStatus(object taskId)
    {
      return (userStateToLifetimeUpdateStatus[taskId] == null);
    }
    private void UpdateJobStatusCompleted(object state)
    {
      UpdateStatusCompleteEventArgs e =
         state as UpdateStatusCompleteEventArgs;

      OnJobStatusCompleted(e);
    }
    protected void OnJobStatusCompleted(
        UpdateStatusCompleteEventArgs e)
    {
      UpdateJobStatusProcessCompleted?.Invoke(this, e);
    }

    private void UpdateJobStatusCompletionMethod(
        string status,
        Exception exception,
        bool canceled,
        AsyncOperation asyncOp)
    {
      // If the task was not previously canceled,
      // remove the task from the lifetime collection.
      if(!canceled)
      {
        lock(userStateToLifetimeUpdateStatus.SyncRoot)
        {
          userStateToLifetimeUpdateStatus.Remove(asyncOp.UserSuppliedState);
        }
      }

      // Output SendFileCompletedEventArgs
      UpdateStatusCompleteEventArgs e =
          new UpdateStatusCompleteEventArgs(status,
          exception,
          canceled,
          asyncOp.UserSuppliedState);

      // End the task. The asyncOp object is responsible 
      // for marshaling the call.
      asyncOp.PostOperationCompleted(onUpdateJobStatusCompleted, e);

    }
    #endregion

  }
}
