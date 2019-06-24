//----------------------------------------------------------------------------+
//        Class: ccReplaceFeature
//  Description: This command provides the ability for a user to replace a feature; the existing feature is processed for removal, and a new copy of the feature is processed as an installation.
//----------------------------------------------------------------------------+
//     $Author:: hkonda                                   $
//       $Date:: 05/12/17                                 $
//   $Revision:: 1                                        $
//----------------------------------------------------------------------------+
//    $History:: ccReplaceFeature.cs                     $
// 
// *****************  Version 1  *****************
// User: hkonda     Date: 05/12/17   Time: 18:00  Desc : Created
// User: hkonda     Date: 31/07/18   Time: 18:00  Desc : Fixed bugs per ONCORDEV-1951.
//                                                       1. RNO Changes  2. Work point number creation 
// User: hkonda     Date: 31/01/19   Time: 18:00 Code adjusted as per ALM-1550-JIRA-2462
// User: pnlella    Date: 12/02/19   Time: 15:15 Code adjusted as per ALM-1865-JIRA-2512
// User: hkonda     Date: 25/02/19   Time: 15:15 Code adjusted to fix ALMs - 1993, 2006, 1990.  JIRAs - 2572, 2573, 2582
// User: pnlella    Date: 08/03/19   Time: 15:15 Adjusted code for handling Isolation scenario.
//----------------------------------------------------------------------------+
using Intergraph.GTechnology.Interfaces;
using System;
using System.Collections.Generic;
using Intergraph.GTechnology.API;
using System.Windows.Forms;
using ADODB;

namespace GTechnology.Oncor.CustomAPI
{
    public class ccReplaceFeature : IGTCustomCommandModal
    {
        private IGTTransactionManager m_TransactionManager;
        IGTApplication m_iGtApplication = null;
        List<string> m_featureStatesList = new List<string> { "INI", "CLS" };
        private IGTDataContext m_dataContext = null;
        private string m_replaceAction;
        private string m_jobStatus;
        private string m_jobType;
        private string m_newFeatureState;
        private string m_modifiedActivityOldFeature;
        private int m_offSetX;
        private int m_offSetY;
        private List<ChildFeatureInfo> m_childFeatureInfoList;
        private IGTDDCKeyObject m_selectedObject;
        Helper oHelper;
        NewFeatureCreator oFeatureCreator;
        IsolationScenario oIsolationScenario;
        bool m_oIsolationScenario;


        public ccReplaceFeature()
        {
            if (m_iGtApplication == null)
            {
                m_iGtApplication = GTClassFactory.Create<IGTApplication>();
            }
            m_dataContext = m_iGtApplication.DataContext;
        }

        #region Public properties
        public IGTTransactionManager TransactionManager
        {
            set
            {
                m_TransactionManager = value;
            }
        }
        public string ReplaceAction
        {
            get { return m_replaceAction; }
            set { m_replaceAction = value; }
        }
        public int OffSetX
        {
            get { return m_offSetX; }
            set { m_offSetX = value; }
        }
        public int OffSetY
        {
            get { return m_offSetY; }
            set { m_offSetY = value; }
        }
        public string JobStatus
        {
            get { return m_jobStatus; }
            set { m_jobStatus = value; }
        }
        public string JobType
        {
            get { return m_jobType; }
            set { m_jobType = value; }
        }
        public string ModifiedActivityOldFeature
        {
            get { return m_modifiedActivityOldFeature; }
            set { m_modifiedActivityOldFeature = value; }
        }
        public string NewFeatureState
        {
            get { return m_newFeatureState; }
            set { m_newFeatureState = value; }
        }
        public IGTDDCKeyObject SelectedObject
        {
            get { return m_selectedObject; }
            set { m_selectedObject = value; }
        }

        #endregion
        public void Activate()
        {
            string jobStatus = string.Empty;
            bool dummyCuCodeExists = false;
            oHelper = new Helper();
            oHelper.m_dataContext = m_dataContext;
            oIsolationScenario = new IsolationScenario(m_dataContext);

            try
            {
                #region Perform validations
                if (CheckIfNonWrJob())
                {
                    MessageBox.Show("This command applies only to WR jobs.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

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
                m_selectedObject = selectedObjects[0];
                oHelper.m_fid = m_selectedObject.FID;
                oHelper.m_fno = m_selectedObject.FNO;
                if (!CheckIfFeatureIsReplaceable())
                {
                    MessageBox.Show("Replace Feature is not configured to operate on the selected feature class.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!CheckIfValidCuAttributesExists(out dummyCuCodeExists))
                {
                    MessageBox.Show("This command applies only to features with CUs.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (dummyCuCodeExists)
                {
                    MessageBox.Show("This command cannot operate on dummy CUs (i.e.- CU codes beginning with ZZ).", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!ValidateFeatureState())
                {
                    MessageBox.Show("One or more features are in an invalid feature state for this operation.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!GetOffsetValuesFromGeneralParameters())
                {
                    MessageBox.Show("General parameters JobMgmt_ReplaceOffsetX or JobMgmt_ReplaceOffsetY  is not configured correctly.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!CheckIfPointFeature())
                {
                    MessageBox.Show("Command is restricted to only work for point features.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!CheckIfInstallAndActiveWrAreDifferent())
                {
                    MessageBox.Show("The same feature may not be installed and replaced in the same WR.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                #endregion

                frmSelectReplacementStatus oReplacementStatusForm = new frmSelectReplacementStatus();
                if (oReplacementStatusForm.ShowDialog(m_iGtApplication.ApplicationWindow) == DialogResult.Cancel)
                {
                    return;
                }
                else
                {
                    this.m_replaceAction = oReplacementStatusForm.ReplacementStatus;
                    m_iGtApplication.BeginWaitCursor();
                    m_oIsolationScenario =oIsolationScenario.CheckIsoScenarioFeature(m_selectedObject.FNO,m_selectedObject.FID);
                    ProcessReplacement();
                    m_TransactionManager.RefreshDatabaseChanges();
                }
            }
            catch (Exception ex)
            {
                m_iGtApplication.EndWaitCursor();
                m_TransactionManager.Rollback();

                MessageBox.Show("Error during execution of Replace Feature custom command." + Environment.NewLine + "Replace failed for selected feature." + Environment.NewLine + ex.Message, "G/Techonology", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                m_TransactionManager = null;
                m_iGtApplication.EndWaitCursor();
            }
        }

        #region Private methods

        #region Validation methods
        /// <summary>
        /// Method to check whether the Job is of type NON-WR.
        /// </summary>
        /// <returns>true, if job is of type NON-WR</returns>
        private bool CheckIfNonWrJob()
        {
            Recordset jobInfoRs = null;
            try
            {
                m_jobType = string.Empty;
                jobInfoRs = oHelper.GetRecordSet(string.Format("select G3E_JOBTYPE, G3E_JOBSTATUS from g3e_job where g3e_identifier  = '{0}'", m_dataContext.ActiveJob));
                if (jobInfoRs != null && jobInfoRs.RecordCount > 0)
                {
                    jobInfoRs.MoveFirst();
                    m_jobType = Convert.ToString(jobInfoRs.Fields["G3E_JOBTYPE"].Value);
                    m_jobStatus = Convert.ToString(jobInfoRs.Fields["G3E_JOBSTATUS"].Value);
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
        /// Method to check whether the feature is replaceable
        /// </summary>
        /// <returns>true, if job is of type NON-WR</returns>
        private bool CheckIfFeatureIsReplaceable()
        {
            Recordset featureInfoRs = null;
            try
            {
                int replace = 0;
                featureInfoRs = oHelper.GetRecordSet(string.Format("select G3E_REPLACE from g3e_features_optable where g3e_fno  = '{0}'", m_selectedObject.FNO));
                if (featureInfoRs != null && featureInfoRs.RecordCount > 0)
                {
                    featureInfoRs.MoveFirst();
                    replace = Convert.ToInt16(featureInfoRs.Fields["G3E_REPLACE"].Value);
                }
                return replace == 1 ? true : false;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (featureInfoRs != null)
                {
                    featureInfoRs.Close();
                }
                featureInfoRs = null;
            }
        }

        /// <summary>
        /// Method to check whether the CU attributes component exists for active feature
        /// </summary>
        /// <returns>true, if CU attributes component exists</returns>
        private bool CheckIfValidCuAttributesExists(out bool isDummyCUCodeExists)
        {
            isDummyCUCodeExists = false;
            try
            {
                IGTKeyObject feature = m_iGtApplication.DataContext.OpenFeature(m_selectedObject.FNO, m_selectedObject.FID);
                IGTComponent cuComponent = feature.Components.GetComponent(21);
                if (cuComponent != null)
                {
                    Recordset cuComponentRs = cuComponent.Recordset;
                    if (cuComponentRs != null && cuComponentRs.RecordCount > 0)
                    {
                        cuComponentRs.MoveFirst();
                        while (!cuComponentRs.EOF)
                        {
                            string cuCode = Convert.ToString(cuComponentRs.Fields["CU_C"].Value);
                            if (!string.IsNullOrEmpty(cuCode) && cuCode.Length > 2 && cuCode.ToUpper().Substring(0, 2).Contains("ZZ"))
                            {
                                isDummyCUCodeExists = true;
                                break;
                            }
                            cuComponentRs.MoveNext();
                        }
                    }
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
        private bool ValidateFeatureState()
        {
            string featureState = string.Empty;
            Recordset commonComponentRs = null;
            try
            {
                IGTKeyObject feature = m_iGtApplication.DataContext.OpenFeature(m_selectedObject.FNO, m_selectedObject.FID);
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
        /// Get values from SYS_GENERALPARAMETER parameter table
        /// </summary>
        /// <returns></returns>
        private bool GetOffsetValuesFromGeneralParameters()
        {
            try
            {
                string sql = "select param_name, param_value from sys_generalparameter where subsystem_name = ? order by ID ASC";
                Recordset offSetValuesRs = m_dataContext.OpenRecordset(sql, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)CommandTypeEnum.adCmdText, "REPLACEOFFSET");

                //Recordset offSetValuesRs = m_dataContext.MetadataRecordset("SYS_GENERALPARAMETER", "subsystem_name like 'JobMgmt_ReplaceOff%'", "ID ASC");
                if (offSetValuesRs != null && offSetValuesRs.RecordCount > 0)
                {
                    offSetValuesRs.MoveFirst();
                    while (!offSetValuesRs.EOF)
                    {
                        if (m_offSetX == 0)
                        {
                            m_offSetX = Convert.ToInt32(offSetValuesRs.Fields["param_value"].Value);
                        }
                        else
                        {
                            m_offSetY = Convert.ToInt32(offSetValuesRs.Fields["param_value"].Value);
                        }
                        offSetValuesRs.MoveNext();
                    }
                }
                return m_offSetX != 0 && m_offSetY != 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Check whether the selected feature is a point feature
        /// </summary>
        /// <returns></returns>
        private bool CheckIfPointFeature()
        {
            try
            {
                return oHelper.CheckIfPrimaryGraphicIsOrientedPoint();
            }
            catch (Exception)
            {
                throw;
            }
        }


        private bool CheckIfInstallAndActiveWrAreDifferent()
        {
            try
            {
                IGTKeyObject feature = m_iGtApplication.DataContext.OpenFeature(m_selectedObject.FNO, m_selectedObject.FID);
                IGTComponent cuComponent = feature.Components.GetComponent(21);
                Recordset cuComponentRs = cuComponent.Recordset;
                cuComponentRs.MoveFirst();
                while (!cuComponentRs.EOF)
                {
                    string installWr = Convert.ToString(cuComponentRs.Fields["WR_ID"].Value);
                    if (!installWr.Equals(m_dataContext.ActiveJob))
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
        #endregion

        /// <summary>
        /// Method to process the replacement
        /// </summary>
        private void ProcessReplacement()
        {
            try
            {
                DetermineNewFeatureState();
                IdentifyActivity();

                try
                {
                    CreateNewFeature();
                }
                catch (Exception)
                {
                    throw;
                }

                try
                {
                    m_TransactionManager.Begin("Process Old Feature Modification in Replace feature");
                    ModifyExistingFeature();
                    m_TransactionManager.Commit();

                    m_TransactionManager.Begin("WP Synchronization...");
                    IGTKeyObject feature = m_iGtApplication.DataContext.OpenFeature(m_selectedObject.FNO, m_selectedObject.FID);
                    SynchronizeWP(feature);
                    
                    if (m_oIsolationScenario)
                    { 
                         oIsolationScenario.UpdateIsolationPointConnectivity(m_selectedObject.FNO,m_selectedObject.FID,oFeatureCreator.m_newFeature);
                    }

                    m_TransactionManager.Commit();

                }
                catch (Exception)
                {
                    m_TransactionManager.Rollback();
                    throw;
                }

                oFeatureCreator.m_modifiedActivityOldFeature = m_modifiedActivityOldFeature;

                try
                {
                    m_TransactionManager.Begin("Process Reowning in Replace feature");
                    if (CheckIfOwnerFeatureAndGetOwnedFeatures())
                    {
                        ReOwnToNewFeatureAndSetActivity();
                    }
                    if (oHelper.CheckForCorrectionModeProperty())
                    {
                        oHelper.RemoveCorrectionModeProperty();
                    }
                    m_TransactionManager.Commit();
                    m_TransactionManager.RefreshDatabaseChanges();

                    if (m_childFeatureInfoList != null )
                    {
                        foreach (ChildFeatureInfo info in m_childFeatureInfoList)
                        {
                            m_TransactionManager.Begin("Process Synchronize WorkPoints in Replace Feature");
                            SynchronizeWP(m_dataContext.OpenFeature(info.GtKeyObject.FNO, info.GtKeyObject.FID));
                            m_TransactionManager.Commit();
                        }
                    }
                }
                catch (Exception)
                {
                    m_TransactionManager.Rollback();
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #region Replacement helper methods

        /// <summary>
        /// Detmermine a new feature state based on the job status
        /// </summary>
        private void DetermineNewFeatureState()
        {
            try
            {
                switch (m_replaceAction)
                {
                    case "Remove":
                    case "Salvage":
                        if (m_jobStatus.ToUpper() == "ASBUILT")
                        {
                            m_newFeatureState = "ABR";
                        }
                        else if (m_jobStatus.ToUpper() == "CONSTRUCTIONCOMPLETE")
                        {
                            m_newFeatureState = "OSR";
                        }
                        else
                        {
                            m_newFeatureState = "PPR";
                        }

                        break;

                    case "Abandon":
                        if (m_jobStatus.ToUpper() == "ASBUILT")
                        {
                            m_newFeatureState = "ABA";
                        }
                        else if (m_jobStatus.ToUpper() == "CONSTRUCTIONCOMPLETE")
                        {
                            m_newFeatureState = "OSA";
                        }
                        else
                        {
                            m_newFeatureState = "PPA";
                        }
                        break;
                }
                oHelper.m_featureState = m_newFeatureState;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to identify the activity of feature based on user selected replacement status
        /// </summary>
        private void IdentifyActivity()
        {
            try
            {
                m_modifiedActivityOldFeature = "R";
                if (oHelper.CheckForCorrectionModeProperty() || m_jobType == "WR-MAPCOR")
                {
                    m_modifiedActivityOldFeature = "RC";
                }
                else if (m_replaceAction == "Salvage")
                {
                    m_modifiedActivityOldFeature = "S";
                }
                else if (m_replaceAction == "Abandon")
                {
                    m_modifiedActivityOldFeature = "A";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to start new feature creation process
        /// </summary>
        private void CreateNewFeature()
        {
            try
            {
                m_TransactionManager.Begin("Process New Feature Creation in Replace feature");
                oFeatureCreator = new NewFeatureCreator();
                oFeatureCreator.m_dataContext = m_dataContext;
                oFeatureCreator.m_oldFeature = m_selectedObject;
                oFeatureCreator.m_jobType = m_jobType;
                oFeatureCreator.m_jobStatus = m_jobStatus;
                oFeatureCreator.m_modifiedActivityOldFeature = m_modifiedActivityOldFeature;
                oFeatureCreator.ProcessFeatureCreation();
                m_TransactionManager.Commit();

                // Sync work points in a new transaction
                m_TransactionManager.Begin("WP Synchronization");
                short fno = oFeatureCreator.m_newFeature.FNO;
                int fid = oFeatureCreator.m_newFeature.FID;
                oFeatureCreator.m_newFeature = m_dataContext.OpenFeature(fno, fid);
                oFeatureCreator.SynchronizeWP(oFeatureCreator.m_newFeature);
                m_TransactionManager.Commit();
            }
            catch (Exception)
            {
                m_TransactionManager.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Method to process modification of existing feature
        /// </summary>
        private void ModifyExistingFeature()
        {
            Dictionary<int, short> dicAssociatedVirtual = new Dictionary<int, short>();
            try
            {
                oHelper.SetFeatureState();
                ApplyOffSetToGraphicComponents(m_selectedObject.FNO, m_selectedObject.FID);

                if(m_oIsolationScenario)
                {
                    oIsolationScenario.GetAssociatedVirtualPoints(m_selectedObject.FID,ref dicAssociatedVirtual);
                    if(dicAssociatedVirtual.Keys.Count>0)
                    {
                        foreach (KeyValuePair<int, short> keyValue in dicAssociatedVirtual)
                        {
                            ApplyOffSetToGraphicComponents(keyValue.Value,keyValue.Key);
                        }
                    }
                }

                SetActivity(21); // Set activity for CU
                SetActivity(22);// Set activity for ACU
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                dicAssociatedVirtual.Clear();
                dicAssociatedVirtual = null;
            }
        }               
        private void SynchronizeWP(IGTKeyObject p_keyObject)
        {
            IGTComponents CUComponentsOldFeature = GTClassFactory.Create<IGTComponents>();

            foreach (IGTComponent item in p_keyObject.Components)
            {
                if (item != null)
                {
                    if (item.CNO == 21 || item.CNO == 22)
                    {
                        CUComponentsOldFeature.Add(item);
                    }
                }
            }
            WorkPointOperations obj = new WorkPointOperations(CUComponentsOldFeature, p_keyObject, m_dataContext);
            obj.DoWorkpointOperations();
        }
        /// <summary>
        /// Method to check if selected feature is a parent feature 
        /// </summary>
        /// <returns> true- if feature is an owner</returns>
        private bool CheckIfOwnerFeatureAndGetOwnedFeatures()
        {
            ChildFeatureInfo oChildFeatureInfo;
            IGTKeyObjects ownedFeatures = null;
            IGTRelationshipService oService = GTClassFactory.Create<IGTRelationshipService>();
            try
            {
                int fId = m_selectedObject.FID;
                short fNo = m_selectedObject.FNO;
                IGTKeyObject oActiveFeature = m_dataContext.OpenFeature(fNo, fId);
                oService.DataContext = m_dataContext;
                oService.ActiveFeature = oActiveFeature;
                try
                {
                    ownedFeatures = oService.GetRelatedFeatures(2);
                }
                catch
                {
                    return false;
                }
                if (ownedFeatures.Count > 0)
                {
                    m_childFeatureInfoList = new List<ChildFeatureInfo>();
                    foreach (IGTKeyObject child in ownedFeatures)
                    {
                        oChildFeatureInfo = new ChildFeatureInfo();
                        oChildFeatureInfo.GtKeyObject = child;
                        Recordset childRs = child.Components.GetComponent(1).Recordset;
                        childRs.MoveFirst();
                        Recordset parentRs = oActiveFeature.Components.GetComponent(1).Recordset;
                        parentRs.MoveFirst();
                        if (Convert.ToString(childRs.Fields["OWNER1_ID"].Value) == Convert.ToString(parentRs.Fields["G3E_ID"].Value))
                        {
                            oChildFeatureInfo.Owner1IdExists = true;
                        }
                        else if (Convert.ToString(childRs.Fields["OWNER2_ID"].Value) == Convert.ToString(parentRs.Fields["G3E_ID"].Value))
                        {
                            oChildFeatureInfo.Owner2IdExists = true;
                        }
                        m_childFeatureInfoList.Add(oChildFeatureInfo);
                    }
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }  
            finally
            {
                oService.Dispose();
                oService = null;
            }
        }

        /// <summary>
        /// Method to re-own child features to replaced feature
        /// </summary>
        private void ReOwnToNewFeatureAndSetActivity()
        {
            try
            {
                if (m_childFeatureInfoList != null)
                {
                    string structureId = Convert.ToString(oFeatureCreator.m_newFeature.Components.GetComponent(1).Recordset.Fields["STRUCTURE_ID"].Value);
                    foreach (ChildFeatureInfo info in m_childFeatureInfoList)
                    {
                        IGTKeyObject childKeyObject = m_dataContext.OpenFeature(info.GtKeyObject.FNO, info.GtKeyObject.FID);

                        Recordset tempRs = childKeyObject.Components.GetComponent(1).Recordset;
                        tempRs.MoveFirst();
                        if (info.Owner1IdExists)
                        {
                            tempRs.Fields["OWNER1_ID"].Value = oFeatureCreator.m_newG3eId;
                        }
                        else if (info.Owner2IdExists)
                        {
                            tempRs.Fields["OWNER2_ID"].Value = oFeatureCreator.m_newG3eId;
                        }

                        // ALM-1993-ONCORDEV-2572  - Update same structure id to each of the non-linear child features
                        if (!IsActiveFeatureIsLinear(info.GtKeyObject.FNO))
                        {
                            tempRs.Fields["STRUCTURE_ID"].Value = structureId;
                        }
                        tempRs.Update(Type.Missing, Type.Missing);
                    }

                    // Set activity
                    foreach (ChildFeatureInfo info in m_childFeatureInfoList)
                    {
                        if (info.GtKeyObject.Components.GetComponent(21) != null)
                        {
                            Recordset rs = info.GtKeyObject.Components.GetComponent(21).Recordset;
                            if (rs != null && rs.RecordCount > 0)
                            {
                                rs.MoveFirst();
                            }
                            if (oHelper.CheckForCorrectionModeProperty() || m_jobType == "WR-MAPCOR")
                            {
                                while (!rs.EOF)
                                {
                                    rs.Fields["ACTIVITY_C"].Value = "TC";
                                    rs.MoveNext();
                                }
                            }
                            else
                            {
                                while (!rs.EOF)
                                {
                                    rs.Fields["ACTIVITY_C"].Value = "T";
                                    rs.MoveNext();
                                }
                            }
                        }
                        if (info.GtKeyObject.Components.GetComponent(22) != null)
                        {
                            Recordset rs1 = info.GtKeyObject.Components.GetComponent(22).Recordset;

                            if (rs1 != null && rs1.RecordCount > 0)
                            {
                                rs1.MoveFirst();
                            }
                            if (oHelper.CheckForCorrectionModeProperty() || m_jobType == "WR-MAPCOR")
                            {
                                while (!rs1.EOF)
                                {
                                    if (Convert.ToString(rs1.Fields["RETIREMENT_C"].Value).Equals("1") || Convert.ToString(rs1.Fields["RETIREMENT_C"].Value).Equals("2"))
                                    {
                                        rs1.Fields["ACTIVITY_C"].Value = "TC";
                                    }
                                    rs1.MoveNext();
                                }
                            }
                            else
                            {
                                while (!rs1.EOF)
                                {
                                    if (Convert.ToString(rs1.Fields["RETIREMENT_C"].Value).Equals("1") || Convert.ToString(rs1.Fields["RETIREMENT_C"].Value).Equals("2"))
                                    {
                                        rs1.Fields["ACTIVITY_C"].Value = "T";
                                    }
                                    rs1.MoveNext();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                m_TransactionManager.Rollback();
                throw;
            }
        }

        #region Feature modify helper methods
        /// <summary>
        /// Method to set the activity attribute of the CU and ACU components
        /// </summary>
        /// <param name="selectedObject">Selected feature on map window</param>
        private void SetActivity(short cno)
        {
            Recordset ComponentRs = null;
            try
            {
                IGTKeyObject feature = m_iGtApplication.DataContext.OpenFeature(m_selectedObject.FNO, m_selectedObject.FID);
                // Set Activity in CU attributes
                ComponentRs = feature.Components.GetComponent(cno).Recordset;
                if (ComponentRs != null && ComponentRs.RecordCount > 0)
                {
                    ComponentRs.MoveFirst();
                    while (!ComponentRs.EOF)
                    {
                        if (cno == 22)
                        {
                            if (Convert.ToString(ComponentRs.Fields["RETIREMENT_C"].Value).Equals("1") || Convert.ToString(ComponentRs.Fields["RETIREMENT_C"].Value).Equals("2"))
                            {
                                ComponentRs.Fields["ACTIVITY_C"].Value = m_modifiedActivityOldFeature;
                            }
                        }
                        else
                        {
                            ComponentRs.Fields["ACTIVITY_C"].Value = m_modifiedActivityOldFeature;
                        }
                        ComponentRs.MoveNext();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Method to update the Structure Id of associated work points for a structure
        /// </summary>
        /// <param name="fno"></param>
        /// <param name="fid"></param>
        /// <param name="newStructureId"></param>
        private void UpdateAssociatedWorkPointsStructureId(short fno, int fid, string newStructureId)
        {
            List<int> workPointFidList = null;
            try
            {
                int recordsAffected = 0;
                ADODB.Recordset rs = m_dataContext.Execute("select g3e_fid WPFID from workpoint_cu_n where assoc_fid = " + fid + " and assoc_fno =" + fno, out recordsAffected, (int)ADODB.CommandTypeEnum.adCmdText, new int[0]);
                if (rs != null && rs.RecordCount > 0)
                {
                    rs.MoveFirst();
                    workPointFidList = new List<int>();
                    while (!rs.EOF)
                    {
                        workPointFidList.Add(Convert.ToInt32(rs.Fields["WPFID"].Value));
                        rs.MoveNext();
                    }
                }

                if (workPointFidList != null)
                {
                    foreach (int wpFid in workPointFidList)
                    {
                        IGTKeyObject workPointFeature = m_dataContext.OpenFeature(191, wpFid);
                        if (workPointFeature != null)
                        {
                            Recordset workPointFeatureRs = workPointFeature.Components.GetComponent(19101).Recordset;
                            if (workPointFeatureRs != null && workPointFeatureRs.RecordCount > 0)
                            {
                                workPointFeatureRs.MoveFirst();
                                workPointFeatureRs.Fields["STRUCTURE_ID"].Value = newStructureId;
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
        /// Method to apply offset to removal feature / old feature
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        private void ApplyOffSetToGraphicComponents(short featureFNO, int featureFID)
        {
            IGTPoint point = null;
            try
            {
                IGTKeyObject feature = m_iGtApplication.DataContext.OpenFeature(featureFNO, featureFID);
                IGTComponents allComponents = feature.Components;
                feature.Components.GetComponent(1).Recordset.MoveFirst();
                string sStructureID = Convert.ToString(feature.Components.GetComponent(1).Recordset.Fields["STRUCTURE_ID"].Value);

                foreach (IGTComponent component in allComponents)
                {
                    if (!IsGraphicGeoComp(component.CNO))
                    {
                        continue;
                    }

                    if (component.Geometry == null)
                    {
                        continue;
                    }
                    switch (component.Geometry.Type)
                    {
                        case GTGeometryTypeConstants.gtgtOrientedPointGeometry:
                            IGTOrientedPointGeometry orientedPtGeometry = GTClassFactory.Create<IGTOrientedPointGeometry>();
                            point = GTClassFactory.Create<IGTPoint>();
                            point.X = component.Geometry.FirstPoint.X + m_offSetX;
                            point.Y = component.Geometry.FirstPoint.Y + m_offSetY;
                            orientedPtGeometry.Origin = point;
                            orientedPtGeometry.Orientation = ((IGTOrientedPointGeometry)component.Geometry).Orientation;
                            component.Geometry = orientedPtGeometry;
                            break;

                        case GTGeometryTypeConstants.gtgtTextPointGeometry:
                            IGTTextPointGeometry textPtGeometry = GTClassFactory.Create<IGTTextPointGeometry>();
                            point = GTClassFactory.Create<IGTPoint>();
                            point.X = component.Geometry.FirstPoint.X + m_offSetX;
                            point.Y = component.Geometry.FirstPoint.Y + m_offSetY;
                            textPtGeometry.Origin = point;
                            textPtGeometry.Normal = ((IGTOrientedPointGeometry)component.Geometry).Orientation;
                            component.Geometry = textPtGeometry;
                            break;

                        case GTGeometryTypeConstants.gtgtPolylineGeometry:
                            IGTPolylineGeometry newFtPlineGeo = GTClassFactory.Create<IGTPolylineGeometry>();
                            for (int k = 0; k <= component.Geometry.KeypointCount - 1; k++)
                            {
                                point = GTClassFactory.Create<IGTPoint>();
                                point.X = component.Geometry.GetKeypointPosition(k).X + m_offSetX;
                                point.Y = component.Geometry.GetKeypointPosition(k).Y + m_offSetY;
                                newFtPlineGeo.Points.Add(point);
                            }
                            component.Geometry = newFtPlineGeo;
                            break;

                        case GTGeometryTypeConstants.gtgtCompositePolylineGeometry:
                            IGTCompositePolylineGeometry newFtCpline = GTClassFactory.Create<IGTCompositePolylineGeometry>();
                            IGTPolylineGeometry polylineGeometrytemp = GTClassFactory.Create<IGTPolylineGeometry>();
                            foreach (IGTGeometry subgeom in (IGTCompositePolylineGeometry)component.Geometry)
                            {
                                switch (subgeom.Type)
                                {


                                    case GTGeometryTypeConstants.gtgtPointGeometry:
                                    case GTGeometryTypeConstants.gtgtOrientedPointGeometry:
                                        point.X = component.Geometry.FirstPoint.X + m_offSetX;
                                        point.Y = component.Geometry.FirstPoint.Y + m_offSetY;
                                        polylineGeometrytemp.Points.Add(point);
                                        break;

                                    case GTGeometryTypeConstants.gtgtLineGeometry:
                                        point.X = component.Geometry.FirstPoint.X + m_offSetX;
                                        point.Y = component.Geometry.FirstPoint.Y + m_offSetY;
                                        polylineGeometrytemp.Points.Add(point);
                                        point.X = component.Geometry.LastPoint.X + m_offSetX;
                                        point.Y = component.Geometry.LastPoint.Y + m_offSetY;
                                        polylineGeometrytemp.Points.Add(point);
                                        break;

                                    case GTGeometryTypeConstants.gtgtPolylineGeometry:
                                        for (int k = 0; k < subgeom.KeypointCount; k++)
                                        {
                                            point.X = component.Geometry.FirstPoint.X + m_offSetX;
                                            point.Y = component.Geometry.FirstPoint.Y + m_offSetY;
                                            polylineGeometrytemp.Points.Add(point);
                                        }
                                        break;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }

                feature.Components.GetComponent(1).Recordset.Fields["STRUCTURE_ID"].Value = sStructureID; //Reset the Structure ID to original value. The update is needed as Set Structure Locaiton FI changed the original Structure ID
                UpdateAssociatedWorkPointsStructureId(feature.FNO, feature.FID, sStructureID); //Update all the WP associated with the feature
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// isGraphicGeoComp method returns input cno is graphic component or not.
        /// </summary>
        /// <param name="cno"></param>
        /// <returns></returns>
        private bool IsGraphicGeoComp(short cno)
        {
            ADODB.Recordset tmprs = null;
            bool isGraphicGeoComp = false;
            try
            {
                tmprs = m_iGtApplication.DataContext.MetadataRecordset("G3E_COMPONENTINFO_OPTABLE");
                tmprs.Filter = "G3E_CNO = " + cno;

                if ((Convert.ToInt16(tmprs.Fields["g3e_type"].Value) != 1) && Convert.ToInt16(tmprs.Fields["g3e_detail"].Value) == 0)
                {
                    isGraphicGeoComp = true;
                }
                tmprs.Close();
                tmprs = null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isGraphicGeoComp;
        }

        /// <summary>
        /// isGraphicGeoComp method returns input cno is non graphic component or not.
        /// </summary>
        /// <param name="cno"></param>
        /// <returns></returns>
        private bool IsNonGraphicGeoComp(short cno)
        {
            ADODB.Recordset tmprs = null;
            bool isNonGraphicGeoComp = false;
            try
            {
                tmprs = m_iGtApplication.DataContext.MetadataRecordset("G3E_COMPONENTINFO_OPTABLE");
                tmprs.Filter = "G3E_CNO = " + cno;

                if ((Convert.ToInt16(tmprs.Fields["g3e_type"].Value) == 1) && Convert.ToInt16(tmprs.Fields["g3e_detail"].Value) == 0)
                {
                    isNonGraphicGeoComp = true;
                }
                tmprs.Close();
                tmprs = null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isNonGraphicGeoComp;
        }


        /// <summary>
        /// Returns true if activefeature is linear feature.
        /// </summary>
        /// <returns></returns>
        private bool IsActiveFeatureIsLinear(short Fno)
        {
            string sql = "";
            Recordset rsLinear = null;
            try
            {
                sql = "SELECT * FROM G3E_COMPONENTINFO_OPTABLE WHERE G3E_CNO IN(SELECT G3E_PRIMARYGEOGRAPHICCNO FROM G3E_FEATURES_OPTABLE WHERE G3E_FNO=?) AND UPPER(G3E_GEOMETRYTYPE) LIKE '%POINT%'";
                rsLinear = m_dataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly,
            (int)ADODB.CommandTypeEnum.adCmdText, Fno);

                if (rsLinear.RecordCount <= 0)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                throw;
            }

        }
        #endregion

        #endregion helper methods

        #endregion
    }
}
