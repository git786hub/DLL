//----------------------------------------------------------------------------+
//        Class: ccRemoveFeature
//  Description: This command provides the ability for a user to transition features from a specific set of feature states to one of the available remove states.
//----------------------------------------------------------------------------+
//     $Author:: pnlella                                   $
//       $Date:: 30/10/17                                 $
//   $Revision:: 1                                        $
//----------------------------------------------------------------------------+
//    $History:: ccRemoveFeature.cs                     $
// 
// *****************  Version 1  *****************
// User: pnlella     Date: 30/10/17   Time: 18:00  Desc : Created
// User: pnlella     Date: 11/09/18   Time: 18:00  Desc : Updated code as per requirement ONCORDEV-2040.
// User: hkonda      Date: 08/10/18   Time: Code adjusted as per ONCORDEV-2085
// User: hkonda      Date: 29/01/19   Time: Code adjusted as per ALM-1613-JIRA-2483
// User: hkonda     Date: 31/01/19   Time: 18:00 Code adjusted as per ALM-1550-JIRA-2462
//----------------------------------------------------------------------------+
using System;
using Intergraph.GTechnology.API;
using ADODB;
using Intergraph.GTechnology.Interfaces;
using System.Windows.Forms;
using System.Collections.Generic;

namespace GTechnology.Oncor.CustomAPI
{
    /// <summary>
    /// Main Driver of the command
    /// </summary>
    class ccRemoveFeature : IGTCustomCommandModeless
    {
        #region IGTCustomCommandModeless Variables
        IGTTransactionManager m_oGTTransactionManager = null;
        IGTApplication m_oGTApp = null;
        IGTCustomCommandHelper m_oGTCustomCommandHelper;
        IGTDDCKeyObjects m_ooddcKeyObjects = null;
        IGTKeyObject m_ofeature;
        IGTComponent m_ocommonComponent = null;
        IGTComponent m_oCUComponent = null;
        IGTKeyObjects m_oOwnedFeatures = null;
        List<Recordset> m_oRSList = null;
        bool m_ostatusExit = false;
        string m_ojobId = string.Empty;
        string m_ojobStatus = string.Empty;
        string m_ojobType = string.Empty;
        int m_oCorrectionsModeIndex = -1;
        bool m_oCorrectionsMode = false;
        bool m_assetHistoryCheckPassed = true;
        #endregion

        #region IGTCustomCommandModeless Members
        public void Activate(IGTCustomCommandHelper CustomCommandHelper)
        {
            IGTDDCKeyObjects ddcKeyObjects = GTClassFactory.Create<IGTDDCKeyObjects>();
            List<int> fidList = new List<int>();
            try
            {
                m_oGTApp = GTClassFactory.Create<IGTApplication>();
                m_oGTCustomCommandHelper = CustomCommandHelper;

                m_ooddcKeyObjects = GTClassFactory.Create<IGTDDCKeyObjects>();

                ddcKeyObjects = m_oGTApp.Application.SelectedObjects.GetObjects();

                foreach (IGTDDCKeyObject ddcKeyObject in ddcKeyObjects)
                {
                    if (!fidList.Contains(ddcKeyObject.FID))
                    {
                        fidList.Add(ddcKeyObject.FID);
                        m_ooddcKeyObjects.Add(ddcKeyObject);
                    }
                }

                m_oGTApp.Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Initializing RemoveFeature...");
                m_oGTApp.Application.BeginWaitCursor();

                SetJobAttributes();

                if (Validate())
                {
                    RemoveFeature();

                    if (!m_ostatusExit)
                    {
                        SynchronizeWP();
                    }

                    if (m_oCorrectionsModeIndex != -1)
                    {
                        m_oGTApp.Application.Properties.Remove("CorrectionsMode");
                        string mapCaption = m_oGTApp.Application.ActiveMapWindow.Caption.Replace("CORRECTIONS MODE - ", "");
                        m_oGTApp.Application.ActiveMapWindow.Caption = mapCaption;
                    }
                }

                if (!m_ostatusExit)
                    ExitCommand();
            }
            catch (Exception ex)
            {
                m_oGTTransactionManager.Rollback();
                MessageBox.Show("Error in Remove Feature command: " + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        #region Event Handlers
        void m_oGTCustomCommandHelper_Click(object sender, GTMouseEventArgs e)
        {
            try
            {
                if (e.Button == 1)
                {
                    foreach (Recordset rs in m_oRSList)
                    {
                        UpdateActivity(rs, "R",false);
                    }
                }
                else if (e.Button == 2)
                {
                    foreach (Recordset rs in m_oRSList)
                    {
                        UpdateActivity(rs, "S",false);
                    }
                }

                // Commit the transaction to reflect the WR_EDITED when set by the trigger
                if (m_oGTTransactionManager.TransactionInProgress)
                {
                    m_oGTTransactionManager.Commit();
                    m_oGTTransactionManager.RefreshDatabaseChanges();
                }

                //  Begin a new transaction to process WP synchronization
                m_oGTTransactionManager.Begin(" WP synchronization...");

                SynchronizeWP();

                ExitCommand();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void m_oGTCustomCommandHelper_MouseMove(object sender, GTMouseEventArgs e)
        {
            m_oGTApp.Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Left click to Remove, right click to Salvage.");
        }

        private void m_oGTCustomCommandHelper_KeyUp(object sender, GTKeyEventArgs e)
        {
            if ((Keys)e.KeyCode == Keys.Escape)
            {
                m_oGTTransactionManager.Rollback();
                ExitCommand();
            }
        }
        private void m_oGTCustomCommandHelper_DblClick(object sender, GTMouseEventArgs e)
        {
            m_oGTTransactionManager.Rollback();
            ExitCommand();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Method to set Job Attributes based on the JOB ID
        /// </summary>
        private void SetJobAttributes()
        {
            try
            {
                m_ojobId = m_oGTApp.Application.DataContext.ActiveJob;

                Recordset jobRS = ExecuteCommand(string.Format("SELECT G3E_JOBSTATUS,G3E_JOBTYPE FROM G3E_JOB WHERE G3E_IDENTIFIER  = '{0}'", m_ojobId));

                if (jobRS != null && jobRS.RecordCount > 0)
                {
                    jobRS.MoveFirst();

                    m_ojobType = Convert.ToString(jobRS.Fields["G3E_JOBTYPE"].Value);
                    m_ojobStatus = Convert.ToString(jobRS.Fields["G3E_JOBSTATUS"].Value);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to perform operations needs to be done by Remove feature command.
        /// </summary>
        private void RemoveFeature()
        {
            m_oRSList = new List<Recordset>();
            try
            {
                SubscribeEvents();

                CheckCorrectionsMode();

                foreach (IGTDDCKeyObject ddcobject in m_ooddcKeyObjects)
                {
                    if (!m_oGTTransactionManager.TransactionInProgress)
                    {
                        m_oGTTransactionManager.Begin("Remove Feature");
                    }

                    m_ofeature = m_oGTApp.Application.DataContext.OpenFeature(ddcobject.FNO, ddcobject.FID);

                    m_ocommonComponent = m_ofeature.Components["COMMON_N"];

                    m_oCUComponent = m_ofeature.Components.GetComponent(21);

                    if (m_ofeature.Components.GetComponent(22) != null && m_ofeature.Components.GetComponent(22).Recordset.RecordCount >= 1 && !(m_ofeature.Components.GetComponent(22).Recordset.EOF && m_ofeature.Components.GetComponent(22).Recordset.BOF))
                    {
                        UpdateActivity(m_ofeature.Components.GetComponent(22).Recordset, m_oCorrectionsMode ? "RC" : "R",true);
                    }

                    if (m_ocommonComponent != null)
                    {
                        m_ocommonComponent.Recordset.MoveFirst();

                        if (Convert.ToString(m_ocommonComponent.Recordset.Fields["FEATURE_STATE_C"].Value) == "PPI" || Convert.ToString(m_ocommonComponent.Recordset.Fields["FEATURE_STATE_C"].Value) == "ABI")
                        {
                            DeleteFeature(m_ofeature);
                        }
                        else
                        {
                            SetFeatureStateBasedOnJobStatus();

                            if (m_oCUComponent != null && m_oCUComponent.Recordset.RecordCount >= 1)
                            {
                                if (m_oCorrectionsMode)
                                {
                                    UpdateActivity(m_oCUComponent.Recordset, "RC",false);
                                }
                                else
                                {
                                    m_ostatusExit = true;

                                    m_oRSList.Add(m_oCUComponent.Recordset);

                                    m_oGTApp.Application.EndWaitCursor();
                                    m_oGTApp.Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Left click to Remove, right click to Salvage.");
                                }
                            }
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
        /// Method for Workpoint operations after command execution
        /// </summary>
        private void SynchronizeWP()
        {
            IGTComponents allCUComponents = null;
            try
            {
                foreach (IGTDDCKeyObject ddcobject in m_ooddcKeyObjects)
                {
                    m_ofeature = m_oGTApp.Application.DataContext.OpenFeature(ddcobject.FNO, ddcobject.FID);

                    allCUComponents = GTClassFactory.Create<IGTComponents>();

                    if (m_ofeature != null)
                    {
                        GetAllCUComponents(m_ofeature, ref allCUComponents);

                        if (allCUComponents.Count > 0)
                        {
                            WorkPointOperations oWorkPointOperations = new WorkPointOperations(allCUComponents, m_ofeature, m_oGTApp.Application.DataContext);
                            oWorkPointOperations.DoWorkpointOperations();
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void GetAllCUComponents(IGTKeyObject feature, ref IGTComponents allCUComponents)
        {
            try
            {
                foreach (IGTComponent component in feature.Components)
                {
                    if (component.CNO == 21 || component.CNO == 22)
                    {
                        if (component != null)
                        {
                            if (component.Recordset != null && component.Recordset.RecordCount > 0)
                            {
                                allCUComponents.Add(component);
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to set FeatureState Based On JobStatus
        /// </summary>
        private void SetFeatureStateBasedOnJobStatus()
        {
            try
            {
                if (!string.IsNullOrEmpty(m_ojobType) && m_ojobType.ToUpper() == "WR-MAPCOR")
                {
                    m_ocommonComponent.Recordset.Fields["FEATURE_STATE_C"].Value = "OSR";
                    return;
                }
                if (m_ojobStatus.ToUpper() == "DESIGN")
                {
                    m_ocommonComponent.Recordset.Fields["FEATURE_STATE_C"].Value = "PPR";
                }
                else if (m_ojobStatus.ToUpper() == "ASBUILT")
                {
                    m_ocommonComponent.Recordset.Fields["FEATURE_STATE_C"].Value = "ABR";
                }
                else if (m_ojobStatus.ToUpper() == "CONSTRUCTIONCOMPLETE")
                {
                    m_ocommonComponent.Recordset.Fields["FEATURE_STATE_C"].Value = "OSR";
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Method to Validate the required conditions to Execute this custom command
        /// </summary>
        private bool Validate()
        {
            string featureState = string.Empty;
            try
            {

                foreach (IGTDDCKeyObject keyobject in m_ooddcKeyObjects)
                {
                    m_ofeature = m_oGTApp.Application.DataContext.OpenFeature(keyobject.FNO, keyobject.FID);

                    m_ocommonComponent = m_ofeature.Components["COMMON_N"];

                    m_oCUComponent = m_ofeature.Components.GetComponent(21);

                    if (m_ocommonComponent != null)
                    {
                        m_ocommonComponent.Recordset.MoveFirst();
                        //Checks if feature state is in any of the state INI, CLS, OSA, PPI, ABI, PPA, ABA
                        featureState = Convert.ToString(m_ocommonComponent.Recordset.Fields["FEATURE_STATE_C"].Value);
                        if (!CheckValidFeatureState(featureState))
                        {

                            if (!m_assetHistoryCheckPassed)
                            {
                                MessageBox.Show("This command cannot be used on unposted features.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                                return false;
                            }
                            if ((featureState == "PPI" || featureState == "ABI"))
                            {
                                MessageBox.Show("Proposed install features must be transitioned to INI before they can be removed; contact Support for assistance.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                MessageBox.Show("One or more features are in an invalid feature state for this operation.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            return false;
                        }
                    }

                    //Checks for the childs with PPR or OSR feature states.
                    m_oOwnedFeatures = GetRelatedFeatures(m_ofeature, 2, m_oGTApp.Application.DataContext);

                    if (m_oOwnedFeatures != null)
                    {
                        for (int i = 0; i < m_oOwnedFeatures.Count; i++)
                        {
                            if (m_oOwnedFeatures[i].Components["COMMON_N"].Recordset.RecordCount > 0)
                            {
                                m_oOwnedFeatures[i].Components["COMMON_N"].Recordset.MoveFirst();

                                if (!(Convert.ToString(m_oOwnedFeatures[i].Components["COMMON_N"].Recordset.Fields["FEATURE_STATE_C"].Value) == "PPR" || Convert.ToString(m_oOwnedFeatures[i].Components["COMMON_N"].Recordset.Fields["FEATURE_STATE_C"].Value) == "OSR"))
                                {
                                    MessageBox.Show("Owned features must be removed first.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return false;
                                }
                            }
                        }
                    }
                }

                //Checks for WR Jobs only
                if (m_ojobType.ToUpper() == "NON-WR")
                {
                    MessageBox.Show("This command applies only to WR jobs.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }


                if (featureState.Equals("INI") || featureState.Equals("CLS") || featureState.Equals("OSA"))
                {
                    if (m_oCUComponent == null)
                    {
                        return true;
                    }
                    Recordset cuRs = m_oCUComponent.Recordset;
                    if (cuRs != null && cuRs.RecordCount > 0)
                    {
                        cuRs.MoveFirst();
                        while (!cuRs.EOF)
                        {
                            string installWr = Convert.ToString(cuRs.Fields["WR_ID"].Value);
                            if (!installWr.Equals(m_ojobId))
                            {
                                return true;
                            }
                            cuRs.MoveNext();
                        }

                        MessageBox.Show("The same feature may not be installed and removed in the same WR.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
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
        /// Method to delete the feature with feature state PPI or ABI
        /// </summary>
        /// <param name="feature"></param>
        private void DeleteFeature(IGTKeyObject feature)
        {
            Recordset oRSFeatureComponent = null;
            try
            {
                oRSFeatureComponent = ExecuteCommand(string.Format("SELECT G3E_NAME FROM G3E_FEATURECOMPS_OPTABLE WHERE G3E_FNO  = {0} ORDER BY G3E_DELETEORDINAL", feature.FNO));

                for (int i = 0; i < oRSFeatureComponent.RecordCount; i++)
                {
                    foreach (IGTComponent comp in feature.Components)
                    {
                        if (comp.Recordset != null && comp.Recordset.RecordCount > 0 && comp.Name == Convert.ToString(oRSFeatureComponent.Fields["G3E_NAME"].Value))
                        {
                            comp.Recordset.MoveFirst();

                            while (!comp.Recordset.EOF)
                            {
                                comp.Recordset.Delete();
                                comp.Recordset.MoveNext();
                            }
                            break;
                        }
                    }
                    oRSFeatureComponent.MoveNext();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Returns the valid feature state
        /// </summary>
        /// <param name="featureState"></param>
        /// <returns></returns>
        private bool CheckValidFeatureState(string featureState)
        {
            bool status = false;
            switch (featureState)
            {
                case "INI":
                case "CLS":
                case "OSA":
                    {
                        status = true;
                        break;
                    }
                case "PPI":
                case "ABI":
                case "PPA":
                case "ABA":
                    {
                        if (CheckFeatureActiveWR())
                            status = true;
                        break;
                    }
                default:
                    break;
            }
            return status;
        }

        /// <summary>
        /// Update the activity attribute of CU Attributes
        /// </summary>
        /// <param name="rs"></param>
        /// <param name="activity"></param>
        private void UpdateActivity(Recordset rs, string activity, bool AncillaryCU)
        {
            rs.MoveFirst();
            while (!(rs.EOF || rs.BOF))
            {
                if (AncillaryCU)
                {
                    if (Convert.ToString(rs.Fields["RETIREMENT_C"].Value).Equals("1") || Convert.ToString(rs.Fields["RETIREMENT_C"].Value).Equals("2"))
                    {
                        rs.Fields["ACTIVITY_C"].Value = activity;
                        rs.Update();
                    }
                }
                else
                {
                    rs.Fields["ACTIVITY_C"].Value = activity;
                    rs.Update();
                }
                rs.MoveNext();
            }
        }


        /// <summary>
        /// Checks whether operation is a correction 
        /// </summary>
        private bool CheckCorrectionsMode()
        {
            for (int i = 0; i < m_oGTApp.Application.Properties.Keys.Count; i++)
            {
                if (Convert.ToString(m_oGTApp.Application.Properties.Keys.Get(i)) == "CorrectionsMode")
                {
                    m_oCorrectionsModeIndex = i;
                    m_oCorrectionsMode = true;
                    break;
                }
            }
            if (!m_oCorrectionsMode)
            {
                if (m_ojobType.ToUpper() == "WR-MAPCOR")
                    m_oCorrectionsMode = true;
            }
            return m_oCorrectionsMode;
        }

        /// <summary>
        /// Checks whether operation is a correction 
        /// </summary>
        private bool CheckFeatureActiveWR()
        {
            bool activeWR = false;
            try
            {
                Recordset tempRs = ExecuteCommand(string.Format("select G3E_IDENTIFIER from asset_history where g3e_fid = {0}  and rownum = 1 order by change_date desc", m_ofeature.FID));

                if (tempRs != null && tempRs.RecordCount > 0)
                {
                    tempRs.MoveFirst();
                    if (Convert.ToString(tempRs.Fields["G3E_IDENTIFIER"].Value) == m_ojobId)
                    {
                        activeWR = true;
                    }
                }
                else
                {
                    m_assetHistoryCheckPassed = false;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return activeWR;
        }

        /// <summary>
        /// Returns the related feature for a corresponding relationship number
        /// </summary>
        /// <param name="activeFeature"></param>
        /// <param name="cRNO"></param>
        ///  <param name="dataContext"></param>
        /// <returns></returns>
        private IGTKeyObjects GetRelatedFeatures(IGTKeyObject activeFeature, short cRNO, IGTDataContext dataContext)
        {
            IGTKeyObjects relatedFeatures = null;
            try
            {
                IGTRelationshipService relService = GTClassFactory.Create<IGTRelationshipService>();
                relService.DataContext = dataContext;
                relService.ActiveFeature = activeFeature;
                relatedFeatures = relService.GetRelatedFeatures(cRNO);

            }
            catch (Exception)
            {

            }
            return relatedFeatures;
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
            m_oGTCustomCommandHelper.Click -= m_oGTCustomCommandHelper_Click;
            m_oGTCustomCommandHelper.MouseMove -= m_oGTCustomCommandHelper_MouseMove;
            m_oGTCustomCommandHelper.KeyUp -= m_oGTCustomCommandHelper_KeyUp;
            m_oGTCustomCommandHelper.DblClick -= m_oGTCustomCommandHelper_DblClick;
        }

        private void ExitCommand()
        {
            try
            {
                if (m_oGTTransactionManager.TransactionInProgress)
                {
                    m_oGTTransactionManager.Commit();
                    m_oGTTransactionManager.RefreshDatabaseChanges();
                }

                m_ooddcKeyObjects.Clear();

                m_oGTApp.Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");

                UnsubscribeEvents();

                if (m_oGTCustomCommandHelper != null)
                {
                    m_oGTCustomCommandHelper.Complete();
                    m_oGTCustomCommandHelper = null;
                }

                m_oGTApp.Application.EndWaitCursor();
                m_oGTApp.Application.RefreshWindows();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                m_oGTApp = null;
                m_oGTCustomCommandHelper = null;
                m_ooddcKeyObjects = null;
                m_oGTTransactionManager = null;
                m_ofeature = null;
                m_oOwnedFeatures = null;
                m_ocommonComponent = null;
                m_oCUComponent = null;
            }
        }
        #endregion
    }
}