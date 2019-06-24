//----------------------------------------------------------------------------+
//        Class: ccAbandonFeature
//  Description: This command provides the ability for a user to transition features from a specific set of feature states to one of the available abandon states.
//----------------------------------------------------------------------------+
//     $Author:: hkonda                                   $
//       $Date:: 10/11/17                                 $
//   $Revision:: 1                                        $
//----------------------------------------------------------------------------+
//    $History:: ccAbandonFeature.cs                     $
// 
// *****************  Version 1  *****************
// User: hkonda     Date: 10/11/17   Time: 18:00  Desc : Created
// User: hkonda     Date: 11/09/18   Time: 18:00  Desc : Updated code to use shared library to create / update Work point 
// User: hkonda     Date: 08/10/18   Time: Code adjusted as per ONCORDEV-2085
// User: hkonda     Date: 31/01/19   Time: 18:00 Code adjusted as per ALM-1550-JIRA-2462
//----------------------------------------------------------------------------+
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Windows.Forms;
using System.Collections.Generic;
using ADODB;
using System;

namespace GTechnology.Oncor.CustomAPI
{
    public class ccAbandonFeature : IGTCustomCommandModal
    {
        private IGTTransactionManager m_TransactionManager;
        IGTApplication m_iGtApplication = GTClassFactory.Create<IGTApplication>();
        List<string> m_featureStatesList = new List<string> { "INI", "CLS" };
        private IGTDataContext m_DataContext = null;
        bool m_AssetHistoryCheckpassed = true;
        string m_JobType = string.Empty;
        public ccAbandonFeature()
        {
            if (m_iGtApplication == null)
            {
                m_iGtApplication = GTClassFactory.Create<IGTApplication>();
            }
            m_DataContext = m_iGtApplication.DataContext;
        }
        public IGTTransactionManager TransactionManager
        {
            set
            {
                m_TransactionManager = value;
            }
        }
        public void Activate()
        {
            string featureState = string.Empty;
            string jobStatus = string.Empty;
            try
            {
                
                IGTDDCKeyObjects selectedObjects = GTClassFactory.Create<IGTDDCKeyObjects>();
                List<int> fidList = new List<int>();
                IGTDDCKeyObjects ddcKeyObjects = m_iGtApplication.SelectedObjects.GetObjects();
                foreach (IGTDDCKeyObject ddcKeyObject in ddcKeyObjects)
                {
                    if (!fidList.Contains(ddcKeyObject.FID))
                    {
                        fidList.Add(ddcKeyObject.FID);
                        selectedObjects.Add(ddcKeyObject);
                    }
                }
                foreach (IGTDDCKeyObject selectedObject in selectedObjects)
                {
                    if (!ValidateFeatureState(selectedObject))
                    {
                        if (!m_AssetHistoryCheckpassed)
                        {
                            MessageBox.Show("This command cannot be used on unposted features.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                            return;
                        }

                        MessageBox.Show("One or more features are in an invalid feature state for this operation.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                   
                    if (!CheckIfCuAttributesExists(selectedObject))
                    {
                        MessageBox.Show("This command applies only to features with CUs.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!CheckIfInstallAndActiveWrAreDifferent(selectedObject))
                    {
                        MessageBox.Show("The same feature may not be installed and abandoned in the same WR.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                if (CheckIfNonWrJob(out jobStatus))
                {
                    MessageBox.Show("This command applies only to WR jobs.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }



                // Validations have been completed. Update feature state.
                if (!m_TransactionManager.TransactionInProgress)
                {
                    m_TransactionManager.Begin("updating feature state...");
                }

                int current = 1;
                int totalCount = selectedObjects.Count;
                foreach (IGTDDCKeyObject selectedObject in selectedObjects)
                {
                    m_iGtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Transtioning " + current + " out of " + totalCount + " features.");
                    SetFeatureStateBasedOnJobStatus(selectedObject, jobStatus);
                    SetActivity(selectedObject);
                    current++;
                }
                m_TransactionManager.Commit();

                // Sync work point in a new transaction 

                m_TransactionManager.Begin("WP Synchronization...");

                foreach (IGTDDCKeyObject selectedObject in selectedObjects)
                {
                    ProcessWPSync(selectedObject);
                }
                m_TransactionManager.Commit();
                m_TransactionManager.RefreshDatabaseChanges();

                m_iGtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Selected features were transitioned successfully.");
            }
            catch (Exception ex)
            {
                m_TransactionManager.Rollback();
                MessageBox.Show("Error during execution of Abandon Feature custom command." + Environment.NewLine + "Transition failed for selected features." + Environment.NewLine + ex.Message, "G/Techonology", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                m_TransactionManager = null;
            }
        }


        private void ProcessWPSync(IGTDDCKeyObject selectedObject)
        {
            try
            {
                WorkPointOperations oWorkPointOperations = null;
                IGTComponents gTComponents = GTClassFactory.Create<IGTComponents>();
                IGTKeyObject feature = m_iGtApplication.DataContext.OpenFeature(selectedObject.FNO, selectedObject.FID);
                Recordset cuComponentRs = feature.Components.GetComponent(21).Recordset;
                if (cuComponentRs != null && cuComponentRs.RecordCount > 0)
                {
                    gTComponents.Add(feature.Components.GetComponent(21));
                }
                // Set Activity in Ancillary CU attributes
                if (feature.Components.GetComponent(22) != null)
                {
                    Recordset acuComponentRs = feature.Components.GetComponent(22).Recordset;
                    if (acuComponentRs != null && acuComponentRs.RecordCount > 0)
                    {
                        gTComponents.Add(feature.Components.GetComponent(22));
                    }
                }

                // Synchronize the workpoint
                oWorkPointOperations = new WorkPointOperations(gTComponents, feature, m_DataContext);
                oWorkPointOperations.DoWorkpointOperations();
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Method to check whether the Job is of type NON-WR.
        /// </summary>
        /// <returns>true, if job is of type NON-WR</returns>
        private bool CheckIfNonWrJob(out string jobStatus)
        {
            jobStatus = string.Empty;
            try
            {
                string jobId = m_DataContext.ActiveJob;
                Recordset jobInfoRs = GetRecordSet(string.Format("select G3E_JOBTYPE, G3E_JOBSTATUS from g3e_job where g3e_identifier  = '{0}'", jobId));
                if (jobInfoRs != null && jobInfoRs.RecordCount > 0)
                {
                    jobInfoRs.MoveFirst();
                    m_JobType = Convert.ToString(jobInfoRs.Fields["G3E_JOBTYPE"].Value);
                    jobStatus = Convert.ToString(jobInfoRs.Fields["G3E_JOBSTATUS"].Value);
                }
                return !string.IsNullOrEmpty(m_JobType) && m_JobType.ToUpper() == "NON-WR" ? true : false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to check whether the CU attributes component exists for active feature
        /// </summary>
        /// <param name="selectedObject">Selected feature on map window</param>
        /// <returns>true, if CU attributes component exists</returns>
        private bool CheckIfCuAttributesExists(IGTDDCKeyObject selectedObject)
        {
            try
            {
                IGTKeyObject feature = m_iGtApplication.DataContext.OpenFeature(selectedObject.FNO, selectedObject.FID);
                IGTComponent cuComponent = feature.Components.GetComponent(21);
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
        /// Method to validate the feature state
        /// </summary>
        /// <param name="selectedObject">Selected feature on map window</param>
        /// <returns>false if feature state is not in one of legel features states</returns>
        private bool ValidateFeatureState(IGTDDCKeyObject selectedObject)
        {
            string featureState = string.Empty;
            Recordset commonComponentRs = null;
            try
            {
                IGTKeyObject feature = m_iGtApplication.DataContext.OpenFeature(selectedObject.FNO, selectedObject.FID);
                IGTComponent commonComponent = feature.Components.GetComponent(1);
                if (commonComponent != null)
                {
                    commonComponentRs = commonComponent.Recordset;

                    if (commonComponentRs != null && commonComponentRs.RecordCount > 0)
                    {
                        commonComponentRs.MoveFirst();
                        featureState = Convert.ToString(commonComponentRs.Fields["FEATURE_STATE_C"].Value);
                    }
                }
                if (string.IsNullOrEmpty(featureState) || !m_featureStatesList.Contains(featureState))
                {
                    if (featureState == "PPR")
                    {
                        Recordset tempRs = GetRecordSet(string.Format("select G3E_IDENTIFIER from asset_history where g3e_fid = {0}  and rownum = 1 order by change_date desc", selectedObject.FID));
                        if (tempRs != null && tempRs.RecordCount > 0)
                        {
                            tempRs.MoveFirst();
                            if (Convert.ToString(tempRs.Fields["G3E_IDENTIFIER"].Value) == m_DataContext.ActiveJob)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            m_AssetHistoryCheckpassed = false;
                        }

                    }
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool CheckIfInstallAndActiveWrAreDifferent(IGTDDCKeyObject selectedObject)
        {
            try
            {
                IGTKeyObject feature = m_iGtApplication.DataContext.OpenFeature(selectedObject.FNO, selectedObject.FID);
                IGTComponent cuComponent = feature.Components.GetComponent(21);
                Recordset cuComponentRs = cuComponent.Recordset;
                cuComponentRs.MoveFirst();
                while (!cuComponentRs.EOF)
                {
                    string installWr = Convert.ToString(cuComponentRs.Fields["WR_ID"].Value);
                    if (!installWr.Equals(m_DataContext.ActiveJob))
                    {
                        return true;
                    }
                    cuComponentRs.MoveNext();
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This method will update the feature state for the selected feature as per Job status
        /// </summary>
        /// <param name="selectedObject">Selected feature on map window</param>
        private void SetFeatureStateBasedOnJobStatus(IGTDDCKeyObject selectedObject, string JobStatus)
        {
            Recordset commonComponentRs = null;
            try
            {
                IGTKeyObject feature = m_iGtApplication.DataContext.OpenFeature(selectedObject.FNO, selectedObject.FID);
                IGTComponent commonComponent = feature.Components.GetComponent(1);
                if (commonComponent != null)
                {
                    commonComponentRs = commonComponent.Recordset;
                    if (commonComponentRs != null && commonComponentRs.RecordCount > 0)
                    {
                        commonComponentRs.MoveFirst();
                        if (!string.IsNullOrEmpty(m_JobType) && (JobStatus.ToUpper() == "WR-MAPCOR"))
                        {
                            commonComponentRs.Fields["FEATURE_STATE_C"].Value = "OSA";
                        }
                        else if (!string.IsNullOrEmpty(JobStatus) && (JobStatus.ToUpper() == "DESIGN"))
                        {
                            commonComponentRs.Fields["FEATURE_STATE_C"].Value = "PPA";
                        }
                        else if (!string.IsNullOrEmpty(JobStatus) && JobStatus.ToUpper() == "ASBUILT")
                        {
                            commonComponentRs.Fields["FEATURE_STATE_C"].Value = "ABA";
                        }
                        else if (!string.IsNullOrEmpty(JobStatus) && JobStatus.ToUpper() == "CONSTRUCTIONCOMPLETE")
                        {
                            commonComponentRs.Fields["FEATURE_STATE_C"].Value = "OSA";
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
        /// Method to set the activity attribute of the CU and ACU components
        /// </summary>
        /// <param name="selectedObject">Selected feature on map window</param>
        private void SetActivity(IGTDDCKeyObject selectedObject)
        {
            Recordset cuComponentRs = null;
            Recordset acuComponentRs = null;
            WorkPointOperations oWorkPointOperations = null;
            IGTComponents gTComponents = GTClassFactory.Create<IGTComponents>();
            try
            {
                IGTKeyObject feature = m_iGtApplication.DataContext.OpenFeature(selectedObject.FNO, selectedObject.FID);
                // Set Activity in CU attributes
                cuComponentRs = feature.Components.GetComponent(21).Recordset;
                if (cuComponentRs != null && cuComponentRs.RecordCount > 0)
                {
                    gTComponents.Add(feature.Components.GetComponent(21));
                    cuComponentRs.MoveFirst();
                    while (!cuComponentRs.EOF)
                    {
                        cuComponentRs.Fields["ACTIVITY_C"].Value = "A";
                        cuComponentRs.MoveNext();
                    }
                }
                // Set Activity in Ancillary CU attributes
                if (feature.Components.GetComponent(22) != null)
                {
                    acuComponentRs = feature.Components.GetComponent(22).Recordset;
                    if (acuComponentRs != null && acuComponentRs.RecordCount > 0)
                    {
                        gTComponents.Add(feature.Components.GetComponent(22));
                        acuComponentRs.MoveFirst();
                        while (!acuComponentRs.EOF)
                        {
                            string retiremenType = Convert.ToString(acuComponentRs.Fields["RETIREMENT_C"].Value);
                            if (retiremenType == "1" || retiremenType == "2")
                            {
                                acuComponentRs.Fields["ACTIVITY_C"].Value = "R";
                            }
                            acuComponentRs.MoveNext();
                        }
                    }
                }

                //// Synchronize the workpoint
                //oWorkPointOperations = new WorkPointOperations(gTComponents, feature, m_DataContext);
                //oWorkPointOperations.DoWorkpointOperations();
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
                Recordset results = m_DataContext.ExecuteCommand(command, out outRecords);
                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}