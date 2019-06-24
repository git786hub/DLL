//----------------------------------------------------------------------------+
//        Class: ccCopyAncillaries
//  Description: This command will allow the user to identify one or more features that will inherit the Ancillary CUs from the source feature.
//----------------------------------------------------------------------------+
//     $Author:: hkonda                                   $
//       $Date:: 19/12/17                                 $
//   $Revision:: 1                                        $
//----------------------------------------------------------------------------+
//    $History:: ccCopyAncillaries.cs                     $
// 
// *****************  Version 1  *****************
// User: hkonda     Date: 19/12/17   Time: 18:00  Desc : Created
// User: hkonda     Date: 30/07/18   Time: 18:00  Desc : Fixed JIRA ONCORDEV-1988
// User: pnlella    Date: 30/07/18   Time: 18:00  Desc : Fixed JIRA ONCORDEV-2272 
// User: hkonda     Date: 22/01/19   Time: 18:00  Desc : Fixed JIRA ONCORDEV-2459 
//----------------------------------------------------------------------------+
using Intergraph.GTechnology.Interfaces;
using System;
using System.Collections.Generic;
using Intergraph.GTechnology.API;
using ADODB;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
using System.Linq;

namespace GTechnology.Oncor.CustomAPI
{
    public class ccCopyAncillaries : IGTCustomCommandModeless
    {
        IGTTransactionManager m_oGTTransactionManager = null;
        IGTApplication m_iGtApplication = null;
        IGTDataContext m_dataContext = null;
        IGTCustomCommandHelper m_oGTCustomCommandHelper;
        IGTDDCKeyObjects m_ooddcKeyObjects = null;
        IGTDDCKeyObject m_originalObject;
        List<IGTDDCKeyObject> m_selectedObjects = new List<IGTDDCKeyObject>();
        string m_jobType;
        frmCopyAncillaryCUs m_ofrmCopyAncillaryCUs;
        bool m_invalidFeatureMessage = false;

        public ccCopyAncillaries()
        {
            if (m_iGtApplication == null)
            {
                m_iGtApplication = GTClassFactory.Create<IGTApplication>();
            }
            m_dataContext = m_iGtApplication.DataContext;
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
        public void Activate(IGTCustomCommandHelper CustomCommandHelper)
        {
            try
            {
                #region Perform validations
                m_oGTCustomCommandHelper = CustomCommandHelper;
                if (CheckIfNonWrJob())
                {
                    MessageBox.Show("This command applies only to WR jobs.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                IGTDDCKeyObjects selectedObjects = GTClassFactory.Create<IGTDDCKeyObjects>();
                List<int> fidList = new List<int>();
                m_ooddcKeyObjects = m_iGtApplication.SelectedObjects.GetObjects();
                foreach (IGTDDCKeyObject ddcKeyObject in m_ooddcKeyObjects)
                {
                    if (!fidList.Contains(ddcKeyObject.FID))
                    {
                        fidList.Add(ddcKeyObject.FID);
                        selectedObjects.Add(ddcKeyObject);
                    }
                }
                m_originalObject = selectedObjects[0];

                if (!CheckIfACUAttributesExists())
                {
                    MessageBox.Show("This command applies only to features with Ancillary CUs.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ExitCommand();
                    return;
                }
                #endregion
               
                m_oGTTransactionManager.Begin("begin Copy Ancillaries...");
                SubscribeEvents();
                DataTable acuDataTable = GetData();
                if (acuDataTable.Rows.Count != 0)
                {
                    m_ofrmCopyAncillaryCUs = new frmCopyAncillaryCUs(m_originalObject, acuDataTable);
                    if (m_ofrmCopyAncillaryCUs.ShowDialog(m_iGtApplication.ApplicationWindow) == DialogResult.Cancel)
                    {
                        ExitCommand();
                        return;
                    }
                }
                else
                {
                    if (MessageBox.Show("Selected feature has no Ancillary CUs. Continue and only delete Ancillary CUs from targeted features?",
                        "G/Technology", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                    {
                        ExitCommand();
                    }
                }
            }
            catch (Exception ex)
            {
                if (m_oGTTransactionManager.TransactionInProgress)
                {
                    m_oGTTransactionManager.Rollback();
                }
                ExitCommand();
                MessageBox.Show("Error during execution of Copy Ancillaries custom command." + Environment.NewLine + ex.Message, "G/Techonology", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Event Handlers

        /// <summary>
        /// Mouse click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_oGTCustomCommandHelper_Click(object sender, GTMouseEventArgs e)
        {
            IGTLocateService selectedFeaturesService;
            IGTDDCKeyObjects selectedFeatures;
            try
            {
                if (e.Button == 1)
                {
                    selectedFeaturesService = m_iGtApplication.ActiveMapWindow.LocateService;
                    selectedFeatures = selectedFeaturesService.Locate(e.WorldPoint, -1, 0, GTSelectionTypeConstants.gtmwstSelectAll);
                    IGTDDCKeyObjects selectedObjects = GTClassFactory.Create<IGTDDCKeyObjects>();
                    List<int> fidList = new List<int>();
                    foreach (IGTDDCKeyObject ddcKeyObject in selectedFeatures)
                    {
                        if (!fidList.Contains(ddcKeyObject.FID))
                        {
                            fidList.Add(ddcKeyObject.FID);
                            selectedObjects.Add(ddcKeyObject);
                        }
                    }
                    if (selectedObjects == null || selectedObjects.Count == 0)
                    {
                        return;
                    }
                    if ((selectedObjects[0].FNO == m_originalObject.FNO) && (selectedObjects[0].FID != m_originalObject.FID))  // Restrict selection to features of the same class as the original feature and make sure same feature instance is not selected
                    {
                        m_invalidFeatureMessage = false;
                        if (!m_selectedObjects.Any(o => o.FID == selectedObjects[0].FID))
                        {
                            m_selectedObjects.Add(selectedObjects[0]);
                            m_iGtApplication.ActiveMapWindow.HighlightedObjects.AddSingle(selectedObjects[0]);
                        }
                    }
                    else
                    {
                        m_invalidFeatureMessage = true;
                        m_iGtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Invalid feature selected.Selected features are not of same class as source feature or selected feature itself is source feature.");
                    }
                }
            }
            catch (Exception ex)
            {
                if (m_oGTTransactionManager.TransactionInProgress)
                {
                    m_oGTTransactionManager.Rollback();
                }
                ExitCommand();
                MessageBox.Show("Error during execution of Copy Ancillaries custom command." + Environment.NewLine + ex.Message, "G/Techonology", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        /// <summary>
        /// Mouse double click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_oGTCustomCommandHelper_DblClick(object sender, GTMouseEventArgs e)
        {
            try
            {
                m_iGtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");
                if (m_selectedObjects.Count == 0)
                {
                    MessageBox.Show("No target features were selected.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ExitCommand();
                    return;
                }
                if (m_ofrmCopyAncillaryCUs != null) // Initial feature has ACUs
                {
                    m_iGtApplication.BeginWaitCursor();
                    if (m_ofrmCopyAncillaryCUs.CanDeleteAllExistigAcus)
                    {
                        DeleteExistingACUs();
                    }
                    if (m_ofrmCopyAncillaryCUs.CuInformationList.Count > 0)
                    {

                        CopySourceACUsToTargetFeature();
                        if (CheckForCorrectionModeProperty())
                        {
                            RemoveCorrectionModeProperty();
                        }
                    }
                }
                else // Initial feature no ACUs and user selected to go ahead and delete ACUs from target
                {
                    DeleteExistingACUs();
                }
                m_oGTTransactionManager.Commit();
                m_oGTTransactionManager.RefreshDatabaseChanges();

                SynchronizeWP();
            }
            catch (Exception ex)
            {
                if (m_oGTTransactionManager.TransactionInProgress)
                {
                    m_oGTTransactionManager.Rollback();
                }
                MessageBox.Show("Error during execution of Copy Ancillaries custom command." + Environment.NewLine + ex.Message, "G/Techonology", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
            finally
            {
                m_iGtApplication.EndWaitCursor();
                ExitCommand();
            }
        }

        /// <summary>
        /// Mosue move event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_oGTCustomCommandHelper_MouseMove(object sender, GTMouseEventArgs e)
        {
            if (!m_invalidFeatureMessage)
                m_iGtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Select features to receive copied ancillaries; double-click to accept selection and continue.");
        }

        /// <summary>
        /// Key up event handler 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_oGTCustomCommandHelper_KeyUp(object sender, GTKeyEventArgs e)
        {
            if ((Keys)e.KeyCode == Keys.Escape)
            {
                m_oGTTransactionManager.Rollback();
                ExitCommand();
            }
        }

        #endregion


        #region Private methods

        /// <summary>
        /// Method to check whether the Job is of type NON-WR.
        /// </summary>
        /// <returns>true, if job is of type NON-WR</returns>
        private bool CheckIfNonWrJob()
        {
            Recordset jobInfoRs = null;
            try
            {
                jobInfoRs = GetRecordSet(string.Format("select G3E_JOBTYPE, G3E_JOBSTATUS from g3e_job where g3e_identifier  = '{0}'", m_dataContext.ActiveJob));
                if (jobInfoRs != null && jobInfoRs.RecordCount > 0)
                {
                    jobInfoRs.MoveFirst();
                    m_jobType = Convert.ToString(jobInfoRs.Fields["G3E_JOBTYPE"].Value);

                }
                return !string.IsNullOrEmpty(m_jobType) && m_jobType.ToUpper() == "NON-WR" ? true : false;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (jobInfoRs != null)
                {
                    jobInfoRs.Close();
                }
                jobInfoRs = null;
            }
        }

        /// <summary>
        /// Method to check whether the Ancillary CU attributes component exists for active feature
        /// </summary>
        /// <returns>true, if Anciallry CU attributes component exists</returns>
        private bool CheckIfACUAttributesExists()
        {
            try
            {
                IGTKeyObject feature = m_iGtApplication.DataContext.OpenFeature(m_originalObject.FNO, m_originalObject.FID);
                IGTComponent cuComponent = feature.Components.GetComponent(22);
                if (cuComponent != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get ACU data for initial selected feature
        /// </summary>
        /// <returns> ACU data table for the initial selcted feature</returns>
        private DataTable GetData()
        {
            string selectCommand = null;
            Recordset rs = null;
            DataTable dt = new DataTable();
            try
            {
                selectCommand = "SELECT CU_C ,CU_DESC, G3E_CID FROM COMP_UNIT_N WHERE G3E_CNO = 22 AND UNIT_CNO IS NULL AND UNIT_CID IS NULL AND G3E_FID = " + m_originalObject.FID;
                rs = m_iGtApplication.DataContext.OpenRecordset(selectCommand, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText);
                using (OleDbDataAdapter daComponents = new OleDbDataAdapter())
                {
                    daComponents.Fill(dt, rs);
                    dt.Columns.Add("Copy", typeof(bool)).SetOrdinal(0);
                    dt.Columns[1].ColumnName = "CU";
                    dt.Columns[2].ColumnName = "Description";
                    dt.Columns[3].ColumnName = "Cid";
                }
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (rs != null)
                {
                    rs.Close();
                    rs = null;
                }
            }
        }

        /// <summary>
        /// Method to delete existing CUs
        /// </summary>
        private void DeleteExistingACUs()
        {
            try
            {
                foreach (IGTDDCKeyObject ddcObject in m_selectedObjects)
                {
                    Recordset acuRs = m_dataContext.OpenFeature(ddcObject.FNO, ddcObject.FID).Components.GetComponent(22).Recordset;
                    if (acuRs != null && acuRs.RecordCount > 0)
                    {
                        acuRs.MoveFirst();
                        while (!acuRs.EOF)
                        {
                            if (Convert.ToString(acuRs.Fields["WR_ID"].Value) == m_dataContext.ActiveJob)
                            {
                                acuRs.Delete();
                            }
                            acuRs.MoveNext();
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to copy Ancillary CUS from Source to Targer feature
        /// </summary>
        private void CopySourceACUsToTargetFeature()
        {
            try
            {
                Recordset originalFeatureACUrs = m_dataContext.OpenFeature(m_originalObject.FNO, m_originalObject.FID).Components.GetComponent(22).Recordset;
                foreach (IGTDDCKeyObject ddcObject in m_selectedObjects)
                {
                    originalFeatureACUrs.Filter = FilterGroupEnum.adFilterNone;
                    originalFeatureACUrs.MoveFirst();

                    Recordset targetFeatureRs = m_dataContext.OpenFeature(ddcObject.FNO, ddcObject.FID).Components.GetComponent(22).Recordset;
                    if (targetFeatureRs != null)
                    {
                        foreach (CuInformation acu in m_ofrmCopyAncillaryCUs.CuInformationList)
                        {
                            originalFeatureACUrs.Filter = "G3E_CID = " + acu.G3eCid;
                            originalFeatureACUrs.MoveFirst();
                            targetFeatureRs.AddNew("G3E_FID", ddcObject.FID);
                            targetFeatureRs.Fields["G3E_FNO"].Value = ddcObject.FNO;
                            targetFeatureRs.Fields["G3E_CNO"].Value = 22;
                            targetFeatureRs.Fields["ACTIVITY_C"].Value = GetActivity();
                            targetFeatureRs.Fields["CU_C"].Value = originalFeatureACUrs.Fields["CU_C"].Value;
                            targetFeatureRs.Fields["MACRO_CU_C"].Value = originalFeatureACUrs.Fields["MACRO_CU_C"].Value;
                            targetFeatureRs.Fields["LENGTH_FLAG"].Value = originalFeatureACUrs.Fields["LENGTH_FLAG"].Value;
                            targetFeatureRs.Fields["QTY_LENGTH_Q"].Value = originalFeatureACUrs.Fields["QTY_LENGTH_Q"].Value;
                            targetFeatureRs.Fields["PRIME_ACCT_ID"].Value = originalFeatureACUrs.Fields["PRIME_ACCT_ID"].Value;
                            targetFeatureRs.Fields["PROP_UNIT_ID"].Value = originalFeatureACUrs.Fields["PROP_UNIT_ID"].Value;
                            targetFeatureRs.Fields["CU_DESC"].Value = originalFeatureACUrs.Fields["CU_DESC"].Value;
                            targetFeatureRs.Fields["CIAC_C"].Value = originalFeatureACUrs.Fields["CIAC_C"].Value;
                            targetFeatureRs.Fields["RETIREMENT_C"].Value = originalFeatureACUrs.Fields["RETIREMENT_C"].Value;
                            targetFeatureRs.Fields["WR_ID"].Value = m_dataContext.ActiveJob;
                            originalFeatureACUrs.Filter = FilterGroupEnum.adFilterNone;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SynchronizeWP()
        {
            try
            {
                foreach (IGTDDCKeyObject ddcObject in m_selectedObjects)
                {
                    m_oGTTransactionManager.Begin("Process Synchronize WorkPoints for Copy Ancillary =" + ddcObject.FNO.ToString() + " FID = " + ddcObject.FID.ToString());

                    IGTKeyObject feature = m_dataContext.OpenFeature(ddcObject.FNO, ddcObject.FID);

                    IGTComponents CUComponentsFeature = GTClassFactory.Create<IGTComponents>();

                    foreach (IGTComponent item in feature.Components)
                    {
                        if (item.CNO == 22)
                        {
                            CUComponentsFeature.Add(item);
                        }
                    }
                    WorkPointOperations obj = new WorkPointOperations(CUComponentsFeature, feature, m_dataContext);
                    obj.DoWorkpointOperations();

                    m_oGTTransactionManager.Commit();
                    m_oGTTransactionManager.RefreshDatabaseChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        private string GetActivity()
        {
            try
            {
                if (CheckForCorrectionModeProperty() || m_jobType == "WR-MAPCOR")
                {
                    return "IC";
                }
                return "I";
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
        private Recordset GetRecordSet(string sqlString)
        {
            try
            {
                int outRecords = 0;
                Command command = new ADODB.Command();
                command.CommandText = sqlString;
                Recordset results = m_dataContext.ExecuteCommand(command, out outRecords);
                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void ExitCommand()
        {
            try
            {
                m_ooddcKeyObjects.Clear();
                m_iGtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");
                UnsubscribeEvents();
                if (m_oGTCustomCommandHelper != null)
                {
                    m_oGTCustomCommandHelper.Complete();
                    m_oGTCustomCommandHelper = null;
                }
                m_iGtApplication.EndWaitCursor();
                m_iGtApplication.RefreshWindows();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to check for corrections mode property
        /// </summary>
        /// <returns></returns>
        private bool CheckForCorrectionModeProperty()
        {
            try
            {
                for (int i = 0; i < m_iGtApplication.Properties.Keys.Count; i++)
                {
                    if (string.Equals("CorrectionsMode", Convert.ToString(m_iGtApplication.Properties.Keys.Get(i))))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to remove corrections mode property
        /// </summary>
        private void RemoveCorrectionModeProperty()
        {
            try
            {
                m_iGtApplication.Properties.Remove("CorrectionsMode");
                string mapCaption = m_iGtApplication.ActiveMapWindow.Caption.Replace("CORRECTIONS MODE - ", string.Empty);
                m_iGtApplication.ActiveMapWindow.Caption = mapCaption;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Register the required events for this Custom command
        /// </summary>
        private void SubscribeEvents()
        {
            m_oGTCustomCommandHelper.MouseMove += new EventHandler<GTMouseEventArgs>(m_oGTCustomCommandHelper_MouseMove);
            m_oGTCustomCommandHelper.Click += new EventHandler<GTMouseEventArgs>(m_oGTCustomCommandHelper_Click);
            m_oGTCustomCommandHelper.KeyUp += new EventHandler<GTKeyEventArgs>(m_oGTCustomCommandHelper_KeyUp);
            m_oGTCustomCommandHelper.DblClick += new EventHandler<GTMouseEventArgs>(m_oGTCustomCommandHelper_DblClick);
        }

        /// <summary>
        /// Unregisters the events registered for this Custom command
        /// </summary>
        private void UnsubscribeEvents()
        {
            if (m_oGTCustomCommandHelper != null)
            {
                m_oGTCustomCommandHelper.Click -= m_oGTCustomCommandHelper_Click;
                m_oGTCustomCommandHelper.MouseMove -= m_oGTCustomCommandHelper_MouseMove;
                m_oGTCustomCommandHelper.KeyUp -= m_oGTCustomCommandHelper_KeyUp;
                m_oGTCustomCommandHelper.DblClick -= m_oGTCustomCommandHelper_DblClick;
            }
        }
        #endregion
    }


}
