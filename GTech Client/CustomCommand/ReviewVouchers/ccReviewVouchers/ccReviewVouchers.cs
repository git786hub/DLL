//----------------------------------------------------------------------------+
//        Class: ccReviewVouchers
//  Description: This command Provides the user a way to review all vouchers for all Work Points in the active WR.
//----------------------------------------------------------------------------+
//     $Author:: pnlella                                   $
//       $Date:: 12/11/17                                 $
//   $Revision:: 1                                        $
//----------------------------------------------------------------------------+
//    $History:: ccReviewVouchers.cs                     $
// 
// *****************  Version 1  *****************
// User: pnlella     Date: 12/11/17   Time: 18:00  Desc : Created
//----------------------------------------------------------------------------+
using System;
using Intergraph.GTechnology.API;
using ADODB;
using Intergraph.GTechnology.Interfaces;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public class ccReviewVouchers : IGTCustomCommandModeless
    {
        #region IGTCustomCommandModeless Variables
        string m_ojobId = string.Empty;
        string m_ojobType = string.Empty;

        #endregion

        #region IGTCustomCommandModeless Members
        public void Activate(IGTCustomCommandHelper customCommandHelper)
        {
            try
            {
                gGlobals.m_oGTApp = GTClassFactory.Create<IGTApplication>();
                gGlobals.m_oGTCustomCommandHelper = customCommandHelper;
                gGlobals.m_oGTApp.Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Initializing ReviewVouchers...");
                gGlobals.m_oGTApp.Application.BeginWaitCursor();
                SetJobAttributes();
                if (Validate())
                {
                    ReviewVouchers();
                }
                else
                {
                    gGlobals.ExitCommand();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Review Vouchers command: " + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
                gGlobals.ExitCommand();
            }
        }
        public bool CanTerminate
        {
            get
            {
                return true;
            }
        }

        public void Pause()
        {
        }

        public void Resume()
        {
        }

        public void Terminate()
        {
            try
            {
                if (gGlobals.m_oGTTransactionManager != null)
                {
                    gGlobals.m_oGTTransactionManager = null;
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public IGTTransactionManager TransactionManager
        {
            get { return gGlobals.m_oGTTransactionManager; }
            set { gGlobals.m_oGTTransactionManager = value; }
        }

        #endregion

        #region Private methods
        /// <summary>
        /// Method to set Job Attributes based on the JOB ID
        /// </summary>
        private void SetJobAttributes()
        {
            try
            {
                Recordset jobRS = null;
                m_ojobId = gGlobals.m_oGTApp.Application.DataContext.ActiveJob;
                jobRS = ExecuteCommand(string.Format("SELECT G3E_JOBTYPE FROM G3E_JOB WHERE G3E_IDENTIFIER  = '{0}'", m_ojobId));
                if (jobRS != null && jobRS.RecordCount > 0)
                {
                    jobRS.MoveFirst();
                    m_ojobType = Convert.ToString(jobRS.Fields["G3E_JOBTYPE"].Value);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Method to Validate the required conditions to Execute this custom command
        /// </summary>
        private bool Validate()
        {
            try
            {
                //Checks for WR Jobs only
                if (m_ojobType.ToUpper() == "NON-WR")
                {
                    MessageBox.Show("This command applies only to WR jobs.", "G/Technology",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to execute sql query and return the result record set
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        private Recordset ExecuteCommand(string sqlString)
        {
            try
            {
                int outRecords = 0;
                Command command = new Command();
                command.CommandText = sqlString;
                Recordset results = gGlobals.m_oGTApp.Application.DataContext.ExecuteCommand(command, out outRecords);
                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to perform operations needs to be done by ReviewVouchers command.
        /// </summary>
        private void ReviewVouchers()
        {
            ccReviewVouchersForm rvFrm = null;
            try
            {
                rvFrm = new ccReviewVouchersForm();
                gGlobals.m_oGTApp.Application.EndWaitCursor();
                gGlobals.m_oGTApp.Application.RefreshWindows();
                rvFrm.StartPosition = FormStartPosition.CenterParent;
                rvFrm.ShowDialog(gGlobals.m_oGTApp.ApplicationWindow);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}