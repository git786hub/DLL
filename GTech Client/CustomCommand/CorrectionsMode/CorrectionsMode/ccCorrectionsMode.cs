//----------------------------------------------------------------------------+
//        Class: ccCorrectionsMode
//  Description: This command toggles the state of a session property to indicate whether the session is design mode or corrections mode.
//----------------------------------------------------------------------------+
//     $Author:: pnlella                                   $
//       $Date:: 6/11/17                                 $
//   $Revision:: 1                                        $
//----------------------------------------------------------------------------+
//    $History:: ccCorrectionsMode.cs                     $
// 
// *****************  Version 1  *****************
// User: pnlella     Date: 6/11/17   Time: 18:00  Desc : Created
//----------------------------------------------------------------------------+
using System;
using Intergraph.GTechnology.API;
using ADODB;
using Intergraph.GTechnology.Interfaces;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public class ccCorrectionsMode : IGTCustomCommandModal
    {
        #region IGTCustomCommandModal Variables
        IGTTransactionManager m_oGTTransactionManager = null;
        IGTApplication m_oGTApp = null;
        string m_ojobId = string.Empty;
        string m_ojobType = string.Empty;
        #endregion

        #region IGTCustomCommandModal Members
        public void Activate()
        {
            try
            {
                m_oGTApp = GTClassFactory.Create<IGTApplication>();
                m_oGTApp.Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Initializing CorrectionsMode...");
                m_oGTApp.Application.BeginWaitCursor();
                SetJobAttributes();
                if (Validate())
                {
                    CorrectionsMode();
                }
                m_oGTApp.Application.EndWaitCursor();
                m_oGTApp.Application.RefreshWindows();
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Corrections Mode command: " + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {

            }
        }

        public IGTTransactionManager TransactionManager
        {
            get { return m_oGTTransactionManager; }
            set { m_oGTTransactionManager = value; }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Method to perform operations needs to be done by CorrectionsMode command.
        /// </summary>
        private void CorrectionsMode()
        {
            try
            {
                if (CheckCorrectionsMode())
                {
                    m_oGTApp.Application.Properties.Remove("CorrectionsMode");
                    string mapCaption = m_oGTApp.Application.ActiveMapWindow.Caption.Replace("CORRECTIONS MODE - ", "");
                    m_oGTApp.Application.ActiveMapWindow.Caption = mapCaption;
                }
                else
                {
                    m_oGTApp.Application.Properties.Add("CorrectionsMode", "CorrectionsMode");
                    m_oGTApp.Application.ActiveMapWindow.Caption = "CORRECTIONS MODE - " + m_oGTApp.Application.ActiveMapWindow.Caption;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to set Job Attributes based on the JOB ID
        /// </summary>
        private void SetJobAttributes()
        {
            try
            {
                Recordset jobRS = null;
                m_ojobId = m_oGTApp.Application.DataContext.ActiveJob;
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
                if (m_ojobType.ToUpper() == "NON-WR" || m_ojobType.ToUpper() == "WR-MAPCOR")
                {
                    MessageBox.Show("This command applies only to WR jobs (excluding map corrections).", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                Recordset results = m_oGTApp.Application.DataContext.ExecuteCommand(command, out outRecords);
                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Checks whether operation is a correction 
        /// </summary>
        private bool CheckCorrectionsMode()
        {
            for (short i = 0; i < m_oGTApp.Application.Properties.Keys.Count; i++)
            {
                if (Convert.ToString(m_oGTApp.Application.Properties.Keys.Get(i)) == "CorrectionsMode")
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
