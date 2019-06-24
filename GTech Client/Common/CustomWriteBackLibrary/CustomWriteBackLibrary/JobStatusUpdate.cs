using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CustomWriteBackLibrary
{
    class JobStatusUpdate
    {
        #region Update Job Status related methods

        public delegate void OnUpdateJobStatusCompletion(object sender, UpdateStatusCompleteEventArgs e);
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
        /// <param name="wrNumber">WR Number</param>
        /// <param name="jobStatus">Job Status</param>
        /// <param name="taskId">A new GUID needs to be passed for this parameter</param>
        public void UpdateJobStatus(string wrNumber, string jobStatus, object taskId)
        {
            InitializeUpdateJobStatusDelegates();
            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(taskId);
            lock (userStateToLifetimeUpdateStatus.SyncRoot)
            {
                if (userStateToLifetimeUpdateStatus.Contains(taskId))
                {
                    throw new ArgumentException(
                        "Task ID parameter must be unique",
                        "taskId");
                }

                userStateToLifetimeUpdateStatus[taskId] = asyncOp;
            }
            UpdateJobStatusWorkerEventHandler workerDelegate = new UpdateJobStatusWorkerEventHandler(UpdateUpdateJobStatusWorker);
            workerDelegate.BeginInvoke(
               wrNumber, jobStatus,
               asyncOp,
               null, null
               );
        }
        private void UpdateUpdateJobStatusWorker(string wrNumber, string jobStatus, AsyncOperation asyncOp)
        {
            string status = "SUCCESS";
            Exception ex = null;

            try
            {
                //just a test to confirm that client continue to work in their thread without getting stuck due to processing in this method
                for (long i = 0; i < 1500000000; i++)
                {

                }
                // int it = Convert.ToInt32("q"); //Uncomment this part to test error thrown by the method
            }
            catch (WebException e) when (e.Status == WebExceptionStatus.Timeout)
            {
                ex = e;
                status = "FAILURE";
            }

            catch (WebException e) when (e.Status == WebExceptionStatus.ReceiveFailure)
            {
                ex = e;
                status = "FAILURE";
            }

            catch (WebException e) when (e.Status == WebExceptionStatus.ConnectFailure)
            {
                ex = e;
                status = "FAILURE";

            }
            catch (Exception e)
            {
                ex = e;
                status = "FAILURE";
            }

            this.UpdateJobStatusCompletionMethod(
             status,
              ex,
              TaskCanceledUpdateJobStatus(asyncOp.UserSuppliedState),
              asyncOp);
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
            if (!canceled)
            {
                lock (userStateToLifetimeUpdateStatus.SyncRoot)
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
