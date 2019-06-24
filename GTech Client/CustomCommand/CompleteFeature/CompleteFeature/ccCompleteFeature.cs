//----------------------------------------------------------------------------+
//        Class: ccCompleteFeature
//  Description: This command allows features to be translated from proposed or as-built states to the corresponding completed state.
//----------------------------------------------------------------------------+
//     $Author:: hkonda                                   $
//       $Date:: 25/10/17                                 $
//   $Revision:: 1                                        $
//----------------------------------------------------------------------------+
//    $History:: ccCompleteFeature.cs                     $
// 
// *****************  Version 1  *****************
// User: hkonda     Date: 25/10/17   Time: 18:00  Desc : Created
// User: hkonda     Date: 24/11/17   Time: 18:00  Desc : Showing error in message box instead of status bar and added message box error icon.
//----------------------------------------------------------------------------+

using System;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public class ccCompleteFeature : IGTCustomCommandModal
    {
        private IGTTransactionManager m_TransactionManager;
        IGTApplication m_iGtApp = GTClassFactory.Create<IGTApplication>();
        List<string> m_featureStatesList = new List<string> { "PPI", "ABI", "PPR", "ABR" };

        public ccCompleteFeature()
        {
            if (m_iGtApp == null)
            {
                m_iGtApp = GTClassFactory.Create<IGTApplication>();
            }
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
            List<int> fidList = new List<int>();
            IGTDDCKeyObjects selectedObjects = GTClassFactory.Create<IGTDDCKeyObjects>();
            try
            {
                IGTDDCKeyObjects ddcKeyObjects = m_iGtApp.SelectedObjects.GetObjects();
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
                    featureState = GetFeatureState(selectedObject);

                    if (string.IsNullOrEmpty(featureState) || !m_featureStatesList.Contains(featureState))
                    {
                        MessageBox.Show("One or more features are in an invalid feature state for this operation.", "G/Techonology", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                if (!m_TransactionManager.TransactionInProgress)
                {
                    m_TransactionManager.Begin("updating feature state...");
                }
                int current = 1;
                int totalCount = selectedObjects.Count;

                foreach (IGTDDCKeyObject selectedObject in selectedObjects)
                {
                    m_iGtApp.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Transitioning " + current + " out of " + totalCount + " features.");
                    SetFeatureState(selectedObject);
                    current++;
                }

                m_TransactionManager.Commit();
                m_TransactionManager.RefreshDatabaseChanges();

                m_iGtApp.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Selected features were transitioned successfully.");
            }
            catch (Exception ex)
            {
                m_TransactionManager.Rollback();
                MessageBox.Show("Error during execution of Complete Feature custom command." + ex.Message, "G/Techonology", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                m_TransactionManager = null;
            }
        }


        /// <summary>
        /// This method will get the feature state for the selected feature
        /// </summary>
        /// <param name="selectedObject">Selected object from select set</param>
        /// <returns>feature state</returns>
        private string GetFeatureState(IGTDDCKeyObject selectedObject)
        {
            string featureState = string.Empty;
            ADODB.Recordset commonComponentRs = null;

            try
            {
                IGTKeyObject feature = m_iGtApp.DataContext.OpenFeature(selectedObject.FNO, selectedObject.FID);
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
                return featureState;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// This method will update the feature state for the selected feature
        /// </summary>
        /// <param name="selectedObject"></param>
        private void SetFeatureState(IGTDDCKeyObject selectedObject)
        {
            string featureState = string.Empty;
            ADODB.Recordset commonComponentRs = null;

            try
            {
                IGTKeyObject feature = m_iGtApp.DataContext.OpenFeature(selectedObject.FNO, selectedObject.FID);
                IGTComponent commonComponent = feature.Components.GetComponent(1);
                if (commonComponent != null)
                {
                    commonComponentRs = commonComponent.Recordset;
                    if (commonComponentRs != null && commonComponentRs.RecordCount > 0)
                    {
                        commonComponentRs.MoveFirst();
                        featureState = Convert.ToString(commonComponentRs.Fields["FEATURE_STATE_C"].Value);

                        if (featureState == "PPI" || featureState == "ABI")
                        {
                            commonComponentRs.Fields["FEATURE_STATE_C"].Value = "INI";
                        }
                        if (featureState == "PPR" || featureState == "ABR")
                        {
                            commonComponentRs.Fields["FEATURE_STATE_C"].Value = "OSR";
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
