// ===================================================
//  Copyright 2017 Intergraph Corp.
//  File Name: ccTreeTrimRequestEstimate.cs
//
//  Description:   Custom placement for the Tree Trimming Voucher feature class,includes automated form and plot generation for submission of a request for estimate to Vegetation Management.
//  Remarks: 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  15/03/2018          Prathyusha                  Created 
// ======================================================
using System;
using ADODB;
using System.Windows.Forms;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;

namespace GTechnology.Oncor.CustomAPI
{
    public class ccTreeTrimRequestEstimate : IGTCustomCommandModeless
    {
        #region IGTCustomCommandModeless Variables

        private IGTTransactionManager m_oGTTransactionManager = null;
        private IGTApplication m_oGTApp = null;
        private IGTCustomCommandHelper m_oGTCustomCommandHelper;
        private IGTFeatureExplorerService m_oGTExplorerService = null;
        private IGTFeaturePlacementService m_oGTPlacementService = null;
        private string m_oStatusBarMessage = null;
        private IGTKeyObject m_oGTTreeTrimmingfeature = null;
        private bool m_oEventsSubscribed = false;

        #endregion

        #region IGTCustomCommandModeless Members
        public void Activate(IGTCustomCommandHelper CustomCommandHelper)
        {
            try
            {
                m_oGTApp = GTClassFactory.Create<IGTApplication>();
                m_oGTCustomCommandHelper = CustomCommandHelper;

                if (Validate())
                {
                    if (m_oGTExplorerService == null)
                        m_oGTExplorerService = GTClassFactory.Create<IGTFeatureExplorerService>(m_oGTCustomCommandHelper);

                    m_oGTExplorerService.Slide(true);

                    if (m_oGTPlacementService == null)
                        m_oGTPlacementService = GTClassFactory.Create<IGTFeaturePlacementService>(m_oGTCustomCommandHelper);

                    if (!m_oGTTransactionManager.TransactionInProgress)
                        m_oGTTransactionManager.Begin("Tree Trimming Request Estimate");

                    m_oStatusBarMessage = "Draw a polygon encompassing all affected Work Points";

                    SubscribeEvents();

                    m_oGTTreeTrimmingfeature = m_oGTApp.DataContext.NewFeature(190);
                    m_oGTPlacementService.StartFeature(m_oGTTreeTrimmingfeature);
                }
                else
                {
                    ExitCommand();
                }
            }
            catch (Exception ex)
            {
                m_oGTExplorerService_CancelClick(null, EventArgs.Empty);
                if (m_oGTTransactionManager.TransactionInProgress)
                {
                    m_oGTTransactionManager.Rollback();
                }
                MessageBox.Show("Error in TreeTrim Request Estimate command: " + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ExitCommand();
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
                if (m_oGTTransactionManager != null)
                {
                    m_oGTTransactionManager = null;
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public IGTTransactionManager TransactionManager
        {
            get { return m_oGTTransactionManager; }
            set { m_oGTTransactionManager = value; }
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Method to Validate the required conditions to Execute this custom command
        /// </summary>
        private bool Validate()
        {
            string jobType = null;
            Recordset jobRS = null;
            DataAccess dataAccess = null;
            try
            {
                dataAccess = new DataAccess(m_oGTApp);

                jobRS = dataAccess.GetRecordset(string.Format("SELECT * FROM G3E_JOB WHERE G3E_IDENTIFIER  = '{0}'", m_oGTApp.DataContext.ActiveJob));

                if (jobRS != null && jobRS.RecordCount > 0)
                {
                    jobRS.MoveFirst();
                    jobType = Convert.ToString(jobRS.Fields["G3E_JOBTYPE"].Value);
                }
                //Checks for WR Jobs only
                if (jobType.ToUpper() == "NON-WR")
                {
                    MessageBox.Show("This command applies only to WR jobs.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (jobRS != null) jobRS = null;
                dataAccess = null;
            }
        }
        /// <summary>
        /// Register the required events for this Custom command
        /// </summary>
        private void SubscribeEvents()
        {
            m_oGTCustomCommandHelper.KeyUp += new EventHandler<GTKeyEventArgs>(m_oGTCustomCommandHelper_KeyUp);
            m_oGTPlacementService.Finished += m_gtPlacementService_Finished;
            m_oGTExplorerService.SaveClick += new EventHandler(m_oGTExplorerService_SaveClick);
            m_oGTExplorerService.CancelClick += new EventHandler(m_oGTExplorerService_CancelClick);
            m_oGTCustomCommandHelper.MouseMove += new EventHandler<GTMouseEventArgs>(m_oGTCustomCommandHelper_MouseMove);
            m_oEventsSubscribed = true;
        }

        /// <summary>
        /// Unregisters the events registered for this Custom command
        /// </summary>
        private void UnsubscribeEvents()
        {
            m_oGTCustomCommandHelper.KeyUp -= m_oGTCustomCommandHelper_KeyUp;
            m_oGTPlacementService.Finished -= m_gtPlacementService_Finished;
            m_oGTExplorerService.SaveClick -= m_oGTExplorerService_SaveClick;
            m_oGTExplorerService.CancelClick -= m_oGTExplorerService_CancelClick;
            m_oGTCustomCommandHelper.MouseMove -= m_oGTCustomCommandHelper_MouseMove;
        }
        /// <summary>
        /// Method to exceute the TreeTrimRequestEstimate command.
        /// </summary>
        private void TreeTrimRequestEstimate()
        {
            Recordset wpRecordset = null;
            ccTreeTrimRequestForm ccTreeTrimRequestForm;
            VoucherEstimatesUtility estimatesUtility = null;
            try
            {
                estimatesUtility = new VoucherEstimatesUtility(m_oGTApp);

                wpRecordset = estimatesUtility.GetWorkpointRsContainedByTreeTrimming(m_oGTTreeTrimmingfeature);

                if (wpRecordset != null && wpRecordset.RecordCount > 0)
                {
                    ccTreeTrimRequestForm = new ccTreeTrimRequestForm(estimatesUtility.GetWorkPtAccountRecordset(wpRecordset), m_oGTApp, m_oGTTreeTrimmingfeature);
                    ccTreeTrimRequestForm.ShowDialog(m_oGTApp.ApplicationWindow);
                    if (ccTreeTrimRequestForm.ProcessDone && ccTreeTrimRequestForm.CommandExit)
                    {
                        ProcessVoucherEstimates oProcessEstimate = new ProcessVoucherEstimates();
                        oProcessEstimate.UpdateVoucherAttributes(m_oGTTreeTrimmingfeature, ccTreeTrimRequestForm.VoucherAccountVal);

                        m_oGTTransactionManager.Commit();
                        m_oGTTransactionManager.RefreshDatabaseChanges();

                        oProcessEstimate.ProcessEmail(ccTreeTrimRequestForm.RecipientName, ccTreeTrimRequestForm.RecipientEmail, ccTreeTrimRequestForm.VegMngSheetTemplate, ccTreeTrimRequestForm.PlotPDFName, m_oGTApp.DataContext.ActiveJob);

                        ExitCommand();
                    }
                    else
                    {
                        m_oGTExplorerService_CancelClick(null, EventArgs.Empty);
                        ExitCommand();
                    }
                }
                else
                {
                    m_oStatusBarMessage = "No Work Points were found within the polygon; draw a new polygon";
                    m_oGTExplorerService_CancelClick(null, EventArgs.Empty);
                    m_oGTTransactionManager.Begin("TreeTrimRequestEstimate");
                    m_oGTTreeTrimmingfeature = m_oGTApp.DataContext.NewFeature(190);
                    m_oGTPlacementService.StartFeature(m_oGTTreeTrimmingfeature);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (wpRecordset != null) wpRecordset = null;
            }
        }
        /// <summary>
        /// Method to Exit the command.
        /// </summary>
        private void ExitCommand()
        {
            try
            {
                m_oGTApp.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");
                if (m_oEventsSubscribed)
                {
                    UnsubscribeEvents();
                }
                if (m_oGTCustomCommandHelper != null)
                {
                    m_oGTCustomCommandHelper.Complete();
                    m_oGTCustomCommandHelper = null;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (m_oGTCustomCommandHelper != null) m_oGTCustomCommandHelper = null;
                if (m_oGTTransactionManager != null) m_oGTTransactionManager = null;
                if (m_oGTExplorerService != null)
                {
                    m_oGTExplorerService.Dispose();
                    m_oGTExplorerService = null;
                }
                if (m_oGTPlacementService != null)
                {
                    m_oGTPlacementService.Dispose();
                    m_oGTPlacementService = null;
                }
            }
        }
        #endregion

        #region Event Handlers
        public void m_oGTCustomCommandHelper_KeyUp(object sender, GTKeyEventArgs e)
        {
            if (e.KeyCode == 27)
            {
                ExitCommand();
            }
        }

        public void m_oGTExplorerService_CancelClick(object sender, EventArgs e)
        {
            if (m_oGTTransactionManager.TransactionInProgress)
                m_oGTTransactionManager.Rollback();
            m_oGTExplorerService.Clear();
            if (sender != null)
            {
                ExitCommand();
            }
        }

        public void m_gtPlacementService_Finished(object sender, GTFinishedEventArgs e)
        {
            m_oGTExplorerService.ExploreFeature(m_oGTTreeTrimmingfeature, "Placement");
        }

        public void m_oGTExplorerService_SaveClick(object sender, EventArgs e)
        {
            try
            {
                TreeTrimRequestEstimate();
            }
            catch (Exception ex)
            {
                m_oGTExplorerService_CancelClick(null, EventArgs.Empty);
                if (m_oGTTransactionManager.TransactionInProgress)
                {
                    m_oGTTransactionManager.Rollback();
                }
                MessageBox.Show("Error in TreeTrim Request Estimate command: " + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ExitCommand();
            }
        }
        public void m_oGTCustomCommandHelper_MouseMove(object sender, GTMouseEventArgs e)
        {
            m_oGTApp.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, m_oStatusBarMessage);
        }
        #endregion
    }
}
