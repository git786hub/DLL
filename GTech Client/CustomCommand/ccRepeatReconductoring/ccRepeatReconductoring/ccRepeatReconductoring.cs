//----------------------------------------------------------------------------+
//        Class: ccRepeatReconductoring
//  Description: This command guides the user through applying the same changes to subsequent spans after changing out wires for a single conductor span.
//----------------------------------------------------------------------------+
//     $Author:: hkonda                                   $
//       $Date:: 05/12/17                                 $
//   $Revision:: 1                                        $
//----------------------------------------------------------------------------+
//    $History:: ccRepeatReconductoring.cs                     $
// 
// *****************  Version 1  *****************
// User: hkonda     Date: 05/12/17   Time: 18:00  Desc : Created
// User: hkonda     Date: 12/12/18   Time: 18:00  Desc : Created
// User: hkonda     Date: 1/2/19     Time: 18:00  Desc : Fix for ALM-1837-JIRA-2515 , ALM-1838-JIRA-2514
// User: hkonda     Date: 1/Apr/19   Time: 18:00  Desc : Fix for ALM-1836-JIRA-2516
// User: asgiribo   Date: 13/May/19  Time: 16:00  Desc : Fix for ALM-2392-JIRA-2816
//----------------------------------------------------------------------------+
using Intergraph.GTechnology.Interfaces;
using System;
using System.Collections.Generic;
using Intergraph.GTechnology.API;
using ADODB;
using System.Windows.Forms;
using System.Linq;

namespace GTechnology.Oncor.CustomAPI
{
    public class ccRepeatReconductoring : IGTCustomCommandModeless
    {
        IGTTransactionManager m_oGTTransactionManager = null;
        IGTApplication m_iGtApplication = null;
        IGTDataContext m_dataContext = null;
        IGTCustomCommandHelper m_oGTCustomCommandHelper;
        IGTDDCKeyObjects m_ooddcKeyObjects = null;
        private IGTDDCKeyObject m_originalObject;
        private IGTDDCKeyObject m_selectedObject;
        private bool m_isPrimaryConductor;
        private short m_wireAttributesCno;
        private List<CuInformation> m_SourceCuInfoList = null;
        CuInformation oCuInformation = null;
        //private bool m_overrideCu;
        private int m_changeOutCount;
        private bool m_invalidFeatureMessage = false;

        public ccRepeatReconductoring()
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
                m_oGTTransactionManager.Begin(" begin Repeat reconductoring...");
                if (CheckIfNonWrJob())
                {
                    ExitCommand();
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

                if (!CheckIfConductorFeature())
                {
                    ExitCommand();
                    MessageBox.Show("This command only applies to conductors.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!ValidateFeatureState())
                {
                    ExitCommand();
                    MessageBox.Show("Select a PPX conductor span from which to copy reconductoring information.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                #endregion
                SubscribeEvents();
                CheckFeatureAndGetCno();
                ReadCUsWithActivity();
            }
            catch (Exception ex)
            {
                ExitCommand();
                MessageBox.Show("Error during execution of Repeat Reconductoring custom command." + Environment.NewLine + ex.Message, "G/Techonology", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        #region Event Handlers
        void m_oGTCustomCommandHelper_Click(object sender, GTMouseEventArgs e)
        {
            IGTLocateService selectedFeaturesService;
            IGTDDCKeyObjects selectedFeatures;
            try
            {
                m_iGtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");
                m_iGtApplication.BeginWaitCursor();
                m_invalidFeatureMessage = false;
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
                    m_selectedObject = selectedObjects[0];

                    if (m_selectedObject.FNO != m_originalObject.FNO || m_selectedObject.FID == m_originalObject.FID)  // Restrict selection to features of the same class as the original feature
                    {
                        m_invalidFeatureMessage = true;
                        m_iGtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Invalid feature selected.Selected feature(s) are not of same class as source feature or selected feature itself is source feature.");
                        return;
                    }

                    m_iGtApplication.ActiveMapWindow.HighlightedObjects.AddSingle(m_selectedObject);

                    if (m_oGTTransactionManager != null && !m_oGTTransactionManager.TransactionInProgress)
                    {
                        m_oGTTransactionManager.Begin(" begin Repeat reconductoring...");
                    }
                    if (ProcessChangeOutsToTargetFeature())
                    {
                        m_oGTTransactionManager.Commit();
                        m_oGTTransactionManager.RefreshDatabaseChanges();

                        // Synchronize Work points - ALM-1838-JIRA-2514
                        SynchronizeWP(m_dataContext.OpenFeature(m_originalObject.FNO, m_originalObject.FID));
                        SynchronizeWP(m_dataContext.OpenFeature(m_selectedObject.FNO, m_selectedObject.FID));
                       
                        m_iGtApplication.EndWaitCursor();
                        m_iGtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Identify next span to changeout wires; double-click to exit.");
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
                MessageBox.Show("Error during execution of Repeat Reconductoring custom command." + Environment.NewLine + ex.Message, "G/Techonology", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                m_iGtApplication.EndWaitCursor();
            }
        }



        private void m_oGTCustomCommandHelper_MouseMove(object sender, GTMouseEventArgs e)
        {
            if (!m_invalidFeatureMessage)
                m_iGtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Identify next span to changeout wires; double-click to exit.");
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


        #region Private methods

        /// <summary>
        ///  Mehtod to check if featrue is primary or secondary conductor and get the wire attributes cno
        /// </summary>
        private void CheckFeatureAndGetCno()
        {
            IGTKeyObject feature = m_iGtApplication.DataContext.OpenFeature(m_originalObject.FNO, m_originalObject.FID);

            // Primary conductor
            if (m_originalObject.FNO == 8 || m_originalObject.FNO == 84)
            {
                m_isPrimaryConductor = true;
                m_wireAttributesCno = 802;
            }
            else if (m_originalObject.FNO == 9 || m_originalObject.FNO == 85)
            {
                m_isPrimaryConductor = true;
                m_wireAttributesCno = 902;
            }
            // Secondary conductor
            else if (m_originalObject.FNO == 53 || m_originalObject.FNO == 96 || m_originalObject.FNO == 63 || m_originalObject.FNO == 97)
            {
                m_isPrimaryConductor = false;
                m_wireAttributesCno = 5302;
            }
        }
        /// <summary>
        /// Method the to read the CU information of selected feature
        /// </summary>
        private void ReadCUsWithActivity()
        {
            int wireRsCount = 0;
            try
            {
                IGTKeyObject sourceFeature = m_iGtApplication.DataContext.OpenFeature(m_originalObject.FNO, m_originalObject.FID);
                IGTComponent sFeatureWireComponent = sourceFeature.Components.GetComponent(m_wireAttributesCno);
                m_SourceCuInfoList = new List<CuInformation>();
                if (sFeatureWireComponent != null)
                {
                    Recordset wireComponentRs = sFeatureWireComponent.Recordset;
                    wireRsCount = wireComponentRs.RecordCount;
                    if (wireComponentRs != null && wireComponentRs.RecordCount > 0)
                    {
                        wireComponentRs.Sort = "G3E_CID";
                        wireComponentRs.MoveFirst();
                        while (!wireComponentRs.EOF)
                        {

                            oCuInformation = new CuInformation();
                            if (m_isPrimaryConductor)
                            {
                                oCuInformation.Phase = Convert.ToString(wireComponentRs.Fields["PHASE_C"].Value);
                                try
                                {
                                    oCuInformation.PhasePosition = Convert.ToString(wireComponentRs.Fields["PHASE_POS_C"].Value);
                                }
                                catch (Exception)
                                {
                                }
                                
                            }
                            else
                            {
                                if (String.IsNullOrEmpty(Convert.ToString(wireComponentRs.Fields["NEUTRAL_YN"].Value)) || Convert.ToString(wireComponentRs.Fields["NEUTRAL_YN"].Value) == "X")
                                    oCuInformation.isNeutral = false;
                                else
                                    oCuInformation.isNeutral = Convert.ToString(wireComponentRs.Fields["NEUTRAL_YN"].Value) == "Y" ? true : false;
                            }
                            if (Convert.IsDBNull(wireComponentRs.Fields["G3E_CID"].Value))
                            {
                                if (m_oGTTransactionManager.TransactionInProgress)
                                {
                                    m_oGTTransactionManager.Rollback();
                                }
                                ExitCommand();
                                return;
                            }
                            oCuInformation.SourceCid = Convert.ToInt16(wireComponentRs.Fields["G3E_CID"].Value);
                            m_SourceCuInfoList.Add(oCuInformation);
                            wireComponentRs.MoveNext();
                        }
                        m_SourceCuInfoList.OrderBy(a => a.TargetCid);
                    }
                }
                if (oCuInformation == null || m_SourceCuInfoList.Count == 0)
                {
                    if (m_oGTTransactionManager.TransactionInProgress)
                    {
                        m_oGTTransactionManager.Rollback();
                    }
                    ExitCommand();
                    return;
                }
                IGTComponent sCuComponent = sourceFeature.Components.GetComponent(21);
                if (sCuComponent != null)
                {
                    Recordset sCuComponentRs = sCuComponent.Recordset;
                    int cuRsCount = sCuComponentRs.RecordCount;
                    if (cuRsCount != wireRsCount)
                    {
                        if (m_oGTTransactionManager.TransactionInProgress)
                        {
                            m_oGTTransactionManager.Rollback();
                        }
                        ExitCommand();
                        return;
                    }
                    if (sCuComponentRs != null && sCuComponentRs.RecordCount > 0)
                    {
                        sCuComponentRs.Sort = "G3E_CID";
                        sCuComponentRs.MoveFirst();
                        //CU_C
                        int i = 0;
                        while (!sCuComponentRs.EOF)
                        {
                            m_SourceCuInfoList[i].CuCode = Convert.ToString(sCuComponentRs.Fields["CU_C"].Value);
                            m_SourceCuInfoList[i].Activity = Convert.ToString(sCuComponentRs.Fields["ACTIVITY_C"].Value);
                            if (!Convert.IsDBNull(sCuComponentRs.Fields["REPLACED_CID"].Value))
                            {
                                m_SourceCuInfoList[i].ReplacedCID = Convert.ToInt32(sCuComponentRs.Fields["REPLACED_CID"].Value);
                                m_SourceCuInfoList[i].UnitCID = Convert.ToInt32(sCuComponentRs.Fields["UNIT_CID"].Value);
                            }
                            i = i + 1;
                            sCuComponentRs.MoveNext();
                        }
                    }
                }
                m_changeOutCount = m_SourceCuInfoList.Where(a => a.Activity == "R" || a.Activity == "S").ToList().Count;
                if (m_changeOutCount == 0)
                {
                    MessageBox.Show("This command applies only to features that have changed wires (i.e.- those with removal or salvage activity).", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    if (m_oGTTransactionManager.TransactionInProgress)
                    {
                        m_oGTTransactionManager.Rollback();
                    }
                    ExitCommand();
                    return;
                }
                if(m_isPrimaryConductor && (m_SourceCuInfoList.Where(a => a.Activity == "R" || a.Activity == "S").Where(a => string.IsNullOrEmpty(a.Phase)).ToList().Count() == m_changeOutCount))
                {
                    if (m_oGTTransactionManager.TransactionInProgress)
                    {
                        m_oGTTransactionManager.Rollback();
                    }
                    MessageBox.Show("Selected conductor does not have 'Phase' populated.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ExitCommand();
                    return;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to process the change outs to the target feature
        /// </summary>
        /// <returns>True, if Changeout is successful. Else false</returns>
        private bool ProcessChangeOutsToTargetFeature()
        {
            int wireCount = 0;
            Recordset cuAttributesRs = null;
            try
            {
                IGTKeyObject targetFeature = m_dataContext.OpenFeature(m_selectedObject.FNO, m_selectedObject.FID);
                IGTComponent tWireAttributes = targetFeature.Components.GetComponent(m_wireAttributesCno);
                IGTComponent tCuAttributes = targetFeature.Components.GetComponent(21);
                List<CuInformation> TargetCuInfoList = new List<CuInformation>();
                if (tWireAttributes != null)
                {
                    Recordset wireAttributesRs = tWireAttributes.Recordset;
                    wireCount = wireAttributesRs.RecordCount;
                    if (wireAttributesRs != null && wireAttributesRs.RecordCount > 0)
                    {
                        wireAttributesRs.Sort = "G3E_CID";
                        wireAttributesRs.MoveFirst();
                        while (!wireAttributesRs.EOF)
                        {
                            oCuInformation = new CuInformation();
                            if (m_isPrimaryConductor)
                            {
                                oCuInformation.Phase = Convert.ToString(wireAttributesRs.Fields["PHASE_C"].Value);
                            }

                            else
                            {
                                if (String.IsNullOrEmpty(Convert.ToString(wireAttributesRs.Fields["NEUTRAL_YN"].Value)) ||
                                    Convert.ToString(wireAttributesRs.Fields["NEUTRAL_YN"].Value) == "X")
                                    oCuInformation.isNeutral = false;
                                else
                                    oCuInformation.isNeutral = Convert.ToString(wireAttributesRs.Fields["NEUTRAL_YN"].Value) == "Y" ? true : false;
                            }
                            if (Convert.IsDBNull(wireAttributesRs.Fields["G3E_CID"].Value))
                            {
                                if (m_oGTTransactionManager.TransactionInProgress)
                                {
                                    m_oGTTransactionManager.Rollback();
                                }
                                ExitCommand();
                                return false;
                            }
                            oCuInformation.TargetCid = Convert.ToInt16(wireAttributesRs.Fields["G3E_CID"].Value);
                            TargetCuInfoList.Add(oCuInformation);
                            wireAttributesRs.MoveNext();
                        }
                        TargetCuInfoList.OrderBy(a => a.TargetCid);
                    }
                }
                if (oCuInformation == null || TargetCuInfoList.Count == 0)
                {
                    if (m_oGTTransactionManager.TransactionInProgress)
                    {
                        m_oGTTransactionManager.Rollback();
                    }
                    ExitCommand();
                    return false;
                }
                if (tCuAttributes != null)
                {
                    cuAttributesRs = tCuAttributes.Recordset;
                    int cuCount = cuAttributesRs.RecordCount;
                    if (cuCount != wireCount)
                    {
                        if (m_oGTTransactionManager.TransactionInProgress)
                        {
                            m_oGTTransactionManager.Rollback();
                        }
                        ExitCommand();
                        return false;
                    }
                    if (cuAttributesRs != null && cuAttributesRs.RecordCount > 0)
                    {
                        cuAttributesRs.Sort = "G3E_CID";
                        cuAttributesRs.MoveFirst();
                        int i = 0;
                        while (!cuAttributesRs.EOF)
                        {
                            TargetCuInfoList[i].CuCode = Convert.ToString(cuAttributesRs.Fields["CU_C"].Value);
                            TargetCuInfoList[i].Activity = Convert.ToString(cuAttributesRs.Fields["ACTIVITY_C"].Value);
                            //if(!Convert.IsDBNull(cuAttributesRs.Fields["UNIT_CID"].Value))
                            //TargetCuInfoList[i].UnitCID = Convert.ToInt32(cuAttributesRs.Fields["UNIT_CID"].Value);
                            TargetCuInfoList[i].SourceCid = Convert.ToInt16(cuAttributesRs.Fields["G3E_CID"].Value);
                            i = i + 1;
                            cuAttributesRs.MoveNext();
                        }
                    }
                }
                var sourceChangeOutList = m_SourceCuInfoList.Where(a => a.Activity == "R" || a.Activity == "S").ToList();

                List<CuInformation> changeList = new List<CuInformation>();
                List<CuInformation> PhaseAndCUcodeMatchList = new List<CuInformation>();
                List<CuInformation> PhaseMatchList = new List<CuInformation>();
                List<string> completedPhasesList = new List<string>();

                if (m_isPrimaryConductor) // Primary conductor feature
                {

                    bool proceedFurther = false;
                    foreach (CuInformation sourceCU in sourceChangeOutList)
                    {
                        foreach (CuInformation targetCU in TargetCuInfoList)
                        {
                            if (!string.IsNullOrEmpty(sourceCU.Phase) && sourceCU.Phase.Equals(targetCU.Phase))
                            {
                                proceedFurther = true;
                                break;
                            }
                        }
                        if (proceedFurther)
                            break;
                    }

                    if (!proceedFurther)
                    {
                        MessageBox.Show("Target conductor span does not include wires that match changes in the source span.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        m_iGtApplication.ActiveMapWindow.HighlightedObjects.Remove(m_selectedObject);
                        return false;
                    }

                    foreach (CuInformation sourceCU in sourceChangeOutList)
                    {
                        foreach (CuInformation targetCU in TargetCuInfoList)
                        {
                            if (sourceCU.Phase.Equals(targetCU.Phase) && sourceCU.CuCode.Equals(targetCU.CuCode))
                            {
                                CuInformation cu = new CuInformation { SourceCid = sourceCU.SourceCid, TargetCid = targetCU.SourceCid, UnitCID = sourceCU.UnitCID, Activity = sourceCU.Activity, ReplacedCID = sourceCU.ReplacedCID , Phase = sourceCU.Phase , PhasePosition = sourceCU.PhasePosition };
                                PhaseAndCUcodeMatchList.Add(cu);
                                completedPhasesList.Add(sourceCU.Phase);
                                //break;
                            }
                        }
                    }
                    foreach (CuInformation sourceCU in sourceChangeOutList)
                    {
                        foreach (CuInformation targetCU in TargetCuInfoList)
                        {
                            if (sourceCU.Phase.Equals(targetCU.Phase) && !sourceCU.CuCode.Equals(targetCU.CuCode) && !completedPhasesList.Contains(sourceCU.Phase))
                            {
                                CuInformation cu = new CuInformation { SourceCid = sourceCU.SourceCid, TargetCid = targetCU.SourceCid, UnitCID = sourceCU.UnitCID, Activity = sourceCU.Activity, ReplacedCID = sourceCU.ReplacedCID, Phase = sourceCU.Phase, PhasePosition = sourceCU.PhasePosition };
                                PhaseMatchList.Add(cu);
                                completedPhasesList.Add(sourceCU.Phase);
                                //break;
                            }
                        }
                    }

                }

                else // Secondary conductor feature
                {
                    bool proceedFurther = false;

                    foreach (CuInformation sourceCU in sourceChangeOutList)
                    {
                        foreach (CuInformation targetCU in TargetCuInfoList)
                        {
                            if (sourceCU.isNeutral.Equals(targetCU.isNeutral) && !sourceCU.isNeutral.Equals(null))
                            {
                                proceedFurther = true;
                                break;
                            }
                        }
                        if (proceedFurther)
                            break;
                    }

                    if (!proceedFurther)
                    {
                        MessageBox.Show("Target conductor span does not include wires that match changes in the source span.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        m_iGtApplication.ActiveMapWindow.HighlightedObjects.Remove(m_selectedObject);
                        return false;
                    }

                    List<CuInformation> tempList = TargetCuInfoList;
                    List<CuInformation> tempSList = sourceChangeOutList;
                    CuInformation tempCu = null;

                    for (int i = 0; i < tempSList.Count; i++)
                    {
                        for (int j = 0; j < tempList.Count; j++)
                        {
                            if (!tempSList[i].Processed && tempSList[i].isNeutral.Equals(tempList[j].isNeutral) && tempSList[i].CuCode.Equals(tempList[j].CuCode) && !tempSList[i].isNeutral.Equals(null))
                            {
                                CuInformation cu = new CuInformation { SourceCid = tempSList[i].SourceCid, TargetCid = tempList[j].SourceCid, UnitCID = tempSList[i].UnitCID, Activity = tempSList[i].Activity, ReplacedCID = tempSList[i].ReplacedCID , isNeutral = tempSList[i].isNeutral};
                                PhaseAndCUcodeMatchList.Add(cu);
                                completedPhasesList.Add(tempSList[i].isNeutral.ToString());
                                tempCu = cu;
                                tempList.RemoveAt(j);
                                break;
                            }

                            if (!tempSList[i].Processed && tempSList[i].isNeutral.Equals(tempList[j].isNeutral) && !tempSList[i].CuCode.Equals(tempList[j].CuCode) && !tempSList[i].isNeutral.Equals(null))
                            {
                                CuInformation cu = new CuInformation { SourceCid = tempSList[i].SourceCid, TargetCid = tempList[j].SourceCid, UnitCID = tempSList[i].UnitCID, Activity = tempSList[i].Activity, ReplacedCID = tempSList[i].ReplacedCID, isNeutral = tempSList[i].isNeutral };
                                PhaseMatchList.Add(cu);
                                completedPhasesList.Add(tempSList[i].isNeutral.ToString());
                                tempCu = cu;
                                tempList.RemoveAt(j);
                                break;
                            }
                        }
                        tempSList[i].Processed = true;
                    }
                }

                changeList.AddRange(PhaseMatchList);
                changeList.AddRange(PhaseAndCUcodeMatchList);

                int processedRecordCount = changeList.Count;

                List<CuInformation> installedCUs = new List<CuInformation>();
                foreach (CuInformation changedCU in changeList)
                {
                    foreach (CuInformation SourceCu in m_SourceCuInfoList)
                    {
                        if (SourceCu.ReplacedCID == changedCU.SourceCid)
                        {
                            installedCUs.Add(SourceCu);
                        }
                    }
                }
                changeList.AddRange(installedCUs);

                if (PhaseMatchList.Count > 0)
                {
                    if (MessageBox.Show("Target conductor span has different CUs than the original version of the source feature; perform changeout anyway? [Y/N]", "G/Technology", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        //m_overrideCu = true;
                        UpdateTargetFeature(changeList);
                        return true;
                    }
                }

                else if (PhaseAndCUcodeMatchList.Count > 0 && PhaseMatchList.Count == 0)
                {
                    UpdateTargetFeature(changeList);
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
        /// Method to update the target feature. This will either update the target component instance(s) or delete the component instance(s) & install from source
        /// </summary>
        /// <param name="changeList"> Contains the list of CU information that needs to be applied on target feature</param>
        private void UpdateTargetFeature(List<CuInformation> changeList)
        {
            try
            {
                Recordset rs = m_dataContext.OpenFeature(m_selectedObject.FNO, m_selectedObject.FID).Components.GetComponent(21).Recordset;
                rs.MoveFirst();

                Recordset originalfeatureRs = m_dataContext.OpenFeature(m_originalObject.FNO, m_originalObject.FID).Components.GetComponent(21).Recordset;
                foreach (CuInformation item in changeList)
                {
                    if (!rs.EOF && !rs.BOF && item.ReplacedCID == 0)
                    {
                        rs.Filter = "G3E_CID = " + item.TargetCid;
                        if (rs.RecordCount > 0)
                        {
                            originalfeatureRs.Filter = "G3E_CID = " + item.SourceCid;
                            CopyValuesFromSource(originalfeatureRs, item, false);
                            originalfeatureRs.Filter = FilterGroupEnum.adFilterNone;
                            originalfeatureRs.MoveFirst();
                        }
                        rs.Filter = FilterGroupEnum.adFilterNone;
                        rs.MoveFirst();
                    }
                }

                // Finally add the newly Installed CU (Installed as part of Changeout operation) if any,  to target CU
                List<int> cidsToIgnore = new List<int>();
                Recordset originalfeatureRS = m_dataContext.OpenFeature(m_originalObject.FNO, m_originalObject.FID).Components.GetComponent(21).Recordset;
                foreach (CuInformation item in changeList)
                {
                    if (item.ReplacedCID != 0)
                    {
                        originalfeatureRS.Filter = "REPLACED_CID = " + item.ReplacedCID + " AND UNIT_CID = " + item.UnitCID;
                        int replacedCid = GetProperReplaceCid(changeList, cidsToIgnore);
                        cidsToIgnore.Add(replacedCid);
                        CopyValuesFromSource(originalfeatureRS, item, true, replacedCid);
                        originalfeatureRS.Filter = FilterGroupEnum.adFilterNone;
                        originalfeatureRS.MoveFirst();
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This method copied CU values from source feature to target feature
        /// </summary>
        /// <param name="source"> Source recordset</param>
        /// <param name="target"> Target recordset</param>
        /// <param name="cu"> part of CU information about source and target</param>
        //private void CopyValuesFromSource(ref Recordset source, ref Recordset target, CuInformation cu)
        private void CopyValuesFromSource(Recordset source, CuInformation cu, bool newInstall , int replaceCid = -1)
        {
            try
            {
                source.MoveFirst();
                Recordset target = m_dataContext.OpenFeature(m_selectedObject.FNO, m_selectedObject.FID).Components.GetComponent(21).Recordset;

                target.Filter = "REPLACED_CID = " + replaceCid;

                if (!target.BOF && !target.EOF && target.RecordCount > 0) 
                {
                    cu.TargetCid = Convert.ToInt16(target.Fields["g3e_cid"].Value);
                    newInstall = false;
                }

                target.Filter = FilterGroupEnum.adFilterNone;

                if (!newInstall)
                    target.Filter = "G3E_CID = " + cu.TargetCid;
                if (target.RecordCount > 0)
                    target.MoveLast();
                if (newInstall)
                {
                    target.AddNew("G3E_FID", m_selectedObject.FID);
                    target.Fields["G3E_FNO"].Value = m_selectedObject.FNO;
                    target.Fields["G3E_CNO"].Value = 21;
                }
                target.Fields["ACTIVITY_C"].Value = source.Fields["ACTIVITY_C"].Value;
                target.Fields["CIAC_C"].Value = source.Fields["CIAC_C"].Value;
                target.Fields["CU_C"].Value = source.Fields["CU_C"].Value;
                target.Fields["CU_DESC"].Value = source.Fields["CU_DESC"].Value;
                target.Fields["FIELD_DESIGN_XML"].Value = source.Fields["FIELD_DESIGN_XML"].Value;
                target.Fields["LENGTH_FLAG"].Value = source.Fields["LENGTH_FLAG"].Value;
                target.Fields["MACRO_CU_C"].Value = source.Fields["MACRO_CU_C"].Value;
                target.Fields["PRIME_ACCT_ID"].Value = source.Fields["PRIME_ACCT_ID"].Value;
                target.Fields["PROP_UNIT_ID"].Value = source.Fields["PROP_UNIT_ID"].Value;
                target.Fields["QTY_LENGTH_Q"].Value = source.Fields["QTY_LENGTH_Q"].Value;
                target.Fields["RETIREMENT_C"].Value = source.Fields["RETIREMENT_C"].Value;
                target.Fields["WM_SEQ"].Value = source.Fields["WM_SEQ"].Value;
                target.Fields["WR_EDITED"].Value = source.Fields["WR_EDITED"].Value;
                if(replaceCid != -1)
                    target.Fields["REPLACED_CID"].Value = replaceCid;
                target.Update(Type.Missing, Type.Missing);
                target.Filter = FilterGroupEnum.adFilterNone;

                // Copy Phase and Phase position - ALM-1836-ONCORDEV-2516
                target = m_dataContext.OpenFeature(m_selectedObject.FNO, m_selectedObject.FID).Components.GetComponent(m_wireAttributesCno).Recordset;
                if (!newInstall)
                    target.Filter = "G3E_CID = " + cu.TargetCid;
                if (target.RecordCount > 0)
                    target.MoveLast();
                if (m_isPrimaryConductor)
                {
                    target.Fields["PHASE_C"].Value = cu.Phase;
                    try
                    {
                        target.Fields["PHASE_POS_C"].Value = cu.PhasePosition;
                    }
                    catch { }
                }
                else
                {
                    if (cu.isNeutral.Equals(false))
                        target.Fields["NEUTRAL_YN"].Value = "N";
                    else if (cu.isNeutral.Equals(true))
                        target.Fields["NEUTRAL_YN"].Value = "Y";

                }
                target.Update(Type.Missing, Type.Missing);
                target.Filter = FilterGroupEnum.adFilterNone;
            }
            catch (Exception)
            {
                throw;
            }
        }


        private int GetProperReplaceCid(List<CuInformation> changeList, List<int> cidsToIgnore)
        {
            foreach (CuInformation cu in changeList)
            {
                foreach (CuInformation item in changeList)
                {
                    //if (cu.SourceCid == item.ReplacedCID && item.TargetCid == 0 && cu.TargetCid != 0 && !cidsToIgnore.Contains(cu.TargetCid))
                    if (cu.SourceCid == item.ReplacedCID && item.TargetCid == 0 &&  !cidsToIgnore.Contains(cu.TargetCid))
                    {
                        return cu.TargetCid;
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// Method to check whether the Job is of type NON-WR.
        /// </summary>
        /// <returns>true, if job is of type NON-WR</returns>
        private bool CheckIfNonWrJob()
        {
            Recordset jobInfoRs = null;
            try
            {
                string jobType = string.Empty;
                jobInfoRs = GetRecordSet(string.Format("select G3E_JOBTYPE, G3E_JOBSTATUS from g3e_job where g3e_identifier  = '{0}'", m_dataContext.ActiveJob));
                if (jobInfoRs != null && jobInfoRs.RecordCount > 0)
                {
                    jobInfoRs.MoveFirst();
                    jobType = Convert.ToString(jobInfoRs.Fields["G3E_JOBTYPE"].Value);
                    jobType = Convert.ToString(jobInfoRs.Fields["G3E_JOBSTATUS"].Value);
                }
                return !string.IsNullOrEmpty(jobType) && jobType.ToUpper() == "NON-WR" ? true : false;
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
        /// Method to check selected feature is a conductor.
        /// </summary>
        /// <returns>true, if job is of type NON-WR</returns>
        private bool CheckIfConductorFeature()
        {
            try
            {
                return (m_originalObject.FNO == 8 || m_originalObject.FNO == 9 || m_originalObject.FNO == 84 || m_originalObject.FNO == 85 || m_originalObject.FNO == 53 || m_originalObject.FNO == 63 || m_originalObject.FNO == 96 || m_originalObject.FNO == 97);
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
                IGTKeyObject feature = m_iGtApplication.DataContext.OpenFeature(m_originalObject.FNO, m_originalObject.FID);
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
                if (featureState != "PPX")
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
                if (m_oGTTransactionManager != null && m_oGTTransactionManager.TransactionInProgress)
                {
                    m_oGTTransactionManager.Rollback();
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


        private void SynchronizeWP(IGTKeyObject p_KeyObjectFeature)
        {
            try
            {
                m_oGTTransactionManager.Begin("Synchronize WorkPoints for Repeat Reconductoring Feature =" + p_KeyObjectFeature.FNO.ToString() + " FID = " + p_KeyObjectFeature.FID.ToString());

                IGTComponents CUComponentsOldFeature = GTClassFactory.Create<IGTComponents>();

                foreach (IGTComponent item in p_KeyObjectFeature.Components)
                {
                    if (item.CNO == 21 || item.CNO == 22)
                    {
                        CUComponentsOldFeature.Add(item);
                    }
                }
                WorkPointOperations obj = new WorkPointOperations(CUComponentsOldFeature, p_KeyObjectFeature, m_iGtApplication.DataContext);
                obj.DoWorkpointOperations();

                m_oGTTransactionManager.Commit();
                m_oGTTransactionManager.RefreshDatabaseChanges();
            }
            catch (Exception ex)
            {
                if (m_oGTTransactionManager.TransactionInProgress)
                {
                    m_oGTTransactionManager.Rollback();
                }
                throw ex;
            }
        }

        /// <summary>
        /// Register the required events for this Custom command
        /// </summary>
        private void SubscribeEvents()
        {
            if (m_oGTCustomCommandHelper == null)
                return;
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
            if (m_oGTCustomCommandHelper == null)
                return;
            m_oGTCustomCommandHelper.Click -= m_oGTCustomCommandHelper_Click;
            m_oGTCustomCommandHelper.MouseMove -= m_oGTCustomCommandHelper_MouseMove;
            m_oGTCustomCommandHelper.KeyUp -= m_oGTCustomCommandHelper_KeyUp;
            m_oGTCustomCommandHelper.DblClick -= m_oGTCustomCommandHelper_DblClick;
        }
        #endregion
    }
}
