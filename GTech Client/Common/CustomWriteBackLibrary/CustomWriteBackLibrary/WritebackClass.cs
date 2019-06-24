//----------------------------------------------------------------------------+
//        Class: WritebackClass
//  Description: This class holds necessary events and methods to process Writeback asynchronously.
//                                                                  
//----------------------------------------------------------------------------+
//          $Author:: Shubham Agarwal                                       $
//          $Date:: 25/03/18                                                $
//          $Revision:: 1                                                   $
//----------------------------------------------------------------------------+
//    $History:: WritebackClass.cs                     $
// 
// *****************  Version 1  *****************
// User: sagarwal     Date: 25/03/18   Time: 18:00  Desc : Created
//----------------------------------------------------------------------------+

using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;
using Intergraph.GTechnology.API;
using ADODB;
using GTechnology.Oncor.CustomAPI;
using System.Data.OleDb;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CustomWriteBackLibrary
{
  public delegate void OnWriteBackCompletion(object sender, WriteBackCompletedEventArgs e);

  class WritebackClass
  {

    #region Update Writeback Status

    public event OnWriteBackCompletion WriteBackProcessCompleted;
    IGTApplication m_oApp = GTClassFactory.Create<IGTApplication>();
        ConfirmComplete cc;

        public WritebackClass(OnWriteBackCompletion p_WriteBackProcessCompleted)
        {
            WriteBackProcessCompleted = p_WriteBackProcessCompleted;           
        }

        private void Cc_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (null != this.cc)
            {
                this.cc.Dispose();
                this.cc = null;
            }
        }

        private void WritebackClass_WriteBackProcessCompleted(object sender, WriteBackCompletedEventArgs e)
        {
            IGTApplication app = GTClassFactory.Create<IGTApplication>();

            // Shared component should return either SUCCESS or FAILURE in the event argument (Status).
            // Notify the user via the status bar and configure the dialog box (if no errors).
            // If errors, dismiss the dialog box and display a message box containing the errors.

            switch (e.Status.ToUpper())
            {
                case "SUCCESS":
                    app.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Writeback complete.");

                    // If the ConfirmComplete dialog has not been unloaded, then set its properties.
                    if (null != cc)
                    {
                        string sMessage = "Writeback complete.";
                        cc.Size = TextRenderer.MeasureText(sMessage, new System.Drawing.Font("Arial", 10));
                        cc.statusText = sMessage;                       
                        cc.enableOK = true;
                    }

                    break;

                case "TIMEOUT":

                    app.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Timeout waiting on Writeback Status.");
                    DataAccess oDataAccess = new DataAccess(m_oApp);
                    int iPollingInterval = Convert.ToInt32(oDataAccess.GetEFUrl("WMIS_WritebackPollingInterval", "WMIS"));

                    if (null != cc)
                    {
                        // Event Handler for FormClosed already disposes so no need to do that here.
                        string sMessage = "Write back timed out; No response received after " + iPollingInterval + " seconds.";
                        cc.Size = TextRenderer.MeasureText(sMessage, new System.Drawing.Font("Arial", 10));
                        cc.statusText = sMessage;
                        cc.enableOK = true;
                    }

                    // Timeouts will always have an event argument and message.

                    break;

                case "FAILURE":
                    app.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Writeback failed.");

                    if (null != e.Error && !string.IsNullOrEmpty(e.Error.Message))
                    {                        
                        string sMessage = string.Format("Writeback failed: {0}.", e.Error.Message);
                        if (e.Error.Message.ToString().StartsWith("Writeback did not return a SUCCESS status for this job"))
                        {
                            sMessage = string.Format("Failed to request write back: {0}.", e.Error.Message);
                        }
                        cc.Size = TextRenderer.MeasureText(sMessage, new System.Drawing.Font("Arial", 10));
                        cc.statusText = sMessage;
                        cc.enableOK = true;

                       // MessageBox.Show(string.Format("Writeback failed: {0}.", e.Error.Message), "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        string sMessage = "Writeback failed.  No additional information is available.";
                        cc.Size = TextRenderer.MeasureText(sMessage, new System.Drawing.Font("Arial", 10));
                        cc.statusText = sMessage;
                        cc.enableOK = true;

                       // MessageBox.Show("Writeback failed.  No additional information is available.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    break;

                default:
                    // This should NEVER occur but covering it just in case...
                    app.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Error in writeback procedure.");

                    //if (null != cc)
                    //{
                    //    // Event Handler for FormClosed already disposes so no need to do that here.
                    //    cc.Close();
                    //}

                    // Assume e.Error.Message is not reliably set in this case so not displaying it.
                    string msg = string.Format("{0}{1}{2}{3}", "Writeback procedure returned an unexpected status value: ", e.Status, Environment.NewLine, "Status of writeback is unknown.");
                    cc.Size = TextRenderer.MeasureText(msg, new System.Drawing.Font("Arial", 10));
                    cc.statusText = msg;
                    cc.enableOK = true;
                   // MessageBox.Show(msg, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }

        private SendOrPostCallback onWriteBackCompleted;
    private delegate void WorkerEventHandler(string wrNumber, AsyncOperation asyncOp, string userName, string passWord);

    private HybridDictionary userStateToLifetime = new HybridDictionary();

    protected virtual void InitializeWriteBackDelegates()
    {
      onWriteBackCompleted = new SendOrPostCallback(WriteBackCompleted);
    }

        /// <summary>
        /// Method to udpate Writeback status for a given WR. This method will be called by client asyncronously. Client needs to register the event WriteBackProcessCompleted to get the notification of method completion
        /// </summary>
        /// <param name="p_wrNumber">WR number</param>
        /// <param name="taskId"></param>

        public void UpdateWriteBack(string p_wrNumber, object taskId)
        {

            string sJobIdentifier = string.Empty;           
            this.cc = new ConfirmComplete();
            this.WriteBackProcessCompleted += WritebackClass_WriteBackProcessCompleted;
            this.cc.enableOK = false;
            this.cc.statusText = "Writing data to WMIS...";
            this.cc.SetDesktopLocation(0, 0);
            this.cc.Show(m_oApp.ApplicationWindow);
            this.cc.TopMost = true;
            this.cc.Text = "WR# " + p_wrNumber;
            

            // When the cc form closes, dispose of it properly.
            this.cc.FormClosed += Cc_FormClosed;

            InitializeWriteBackDelegates();
            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(taskId);

            string sUserName = m_oApp.DataContext.DatabaseUserName;
            string sPasswordString = m_oApp.DataContext.ViewerConnectionString.Substring(m_oApp.DataContext.ViewerConnectionString.IndexOf("Password="), (m_oApp.DataContext.ViewerConnectionString.Length - m_oApp.DataContext.ViewerConnectionString.IndexOf("Password=")));

            lock (userStateToLifetime.SyncRoot)
            {
                if (userStateToLifetime.Contains(taskId))
                {
                    throw new ArgumentException(
                        "Task ID parameter must be unique",
                        "taskId");
                }

                userStateToLifetime[taskId] = asyncOp;
            }
            WorkerEventHandler workerDelegate = new WorkerEventHandler(UpdateWritebackStatusWorker);
            workerDelegate.BeginInvoke(
               p_wrNumber,
               asyncOp, sUserName, sPasswordString,
                null, null
               );
        }

    private bool IsAlternateJob(string p_WRNumber, out string p_jobIdentifier)
    {
      bool bReturn = false;
      p_jobIdentifier = string.Empty;
 
      Recordset rs = m_oApp.DataContext.OpenRecordset("select count(*) from g3e_job where wr_nbr = {0} and g3e_identifier like '%-%' and g3e_jobstatus = 'ApprovalPending' ", CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText, p_WRNumber);
      if(rs != null)
      {
        if(rs.RecordCount > 0)
        {
          p_jobIdentifier = Convert.ToString(rs.Fields[0].Value);
        }
        rs.Close();
        rs = null;
      }

      bReturn = !string.IsNullOrEmpty(p_jobIdentifier);

      return bReturn;
    }

    private void UpdateWritebackStatusWorker(string p_jobIdentifier, AsyncOperation asyncOp, string p_userName, string p_passWord)
    {
      string status = "SUCCESS";
      Exception ex = null;
      bool bAlternateJobExists = false;

      try
      {
        // Unsure if this test needs to be here since the clients that call this class perform this test; however,
        // leaving this here just in case this is called from a client that does not perform this check.
        string SQL = string.Format("select count(1) from g3e_job where g3e_identifier like '{0}-%' and upper(g3e_jobstatus)='APPROVALPENDING'", p_jobIdentifier.Split('-')[0]);
        Recordset rs = m_oApp.DataContext.OpenRecordset(SQL, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText);

        if(0 < Convert.ToInt32(rs.Fields[0].Value))
        {
          bAlternateJobExists = true;
          ex = new Exception(string.Format("Alternate design {0} has been marked for approval", p_jobIdentifier));
          status = "FAILURE";

          //Raise the completion event here itself
          this.CompletionMethod(
           status,
            ex,
            TaskCanceled(asyncOp.UserSuppliedState),
            asyncOp);
        }

        rs.Close();
        rs = null;

        //Continue to call the process Write Back

        ProcessWriteBack oProcess = new ProcessWriteBack(m_oApp);
        WebServiceResponse sStatus = WebServiceResponse.Success;
        string bErrorInfo = string.Empty;

        oProcess.ProcessWriteBackStatusUpdate(p_jobIdentifier, p_passWord, bAlternateJobExists, out sStatus, out bErrorInfo);
        switch(sStatus)
        {
          case WebServiceResponse.Error:
            ex = new Exception(bErrorInfo);
            status = "FAILURE";
            break;
          case WebServiceResponse.Timeout:
            ex = new Exception(bErrorInfo);
            status = "TIMEOUT";
            break;
          case WebServiceResponse.Success:
            ex = null;
            status = "SUCCESS";
            break;
          default:
            break;
        }
      }
      catch(Exception e)
      {
        ex = e;
        status = "FAILURE";
      }

      this.CompletionMethod(
       status,
        ex,
        TaskCanceled(asyncOp.UserSuppliedState),
        asyncOp);
    }
    private bool TaskCanceled(object taskId)
    {
      return (userStateToLifetime[taskId] == null);
    }
    private void WriteBackCompleted(object state)
    {
      WriteBackCompletedEventArgs e =
         state as WriteBackCompletedEventArgs;

      OnWriteBackCompleted(e);
    }
    protected void OnWriteBackCompleted(
        WriteBackCompletedEventArgs e)
    {
      WriteBackProcessCompleted?.Invoke(this, e);
    }

    private void CompletionMethod(
        string status,
        Exception exception,
        bool canceled,
        AsyncOperation asyncOp)
    {
      // If the task was not previously canceled,
      // remove the task from the lifetime collection.
      if(!canceled)
      {
        lock(userStateToLifetime.SyncRoot)
        {
          userStateToLifetime.Remove(asyncOp.UserSuppliedState);
        }
      }

      // Output SendFileCompletedEventArgs
      WriteBackCompletedEventArgs e =
          new WriteBackCompletedEventArgs(status,
          exception,
          canceled,
          asyncOp.UserSuppliedState);

      // End the task. The asyncOp object is responsible 
      // for marshaling the call.
      asyncOp.PostOperationCompleted(onWriteBackCompleted, e);

    }
    #endregion
  }
}
