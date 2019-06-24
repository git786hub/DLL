//----------------------------------------------------------------------------+
//        Class: rgiVoltageAgreement
//  Description: Copies voltage attributes when establishing connectivity.Validates voltage agreement between directly connected features, based on the voltage attributes corresponding to the connected nodes.
//----------------------------------------------------------------------------+
//     $Author:: pnlella                                   $
//       $Date:: 03/05/19                                 $
//   $Revision:: 1                                        $
//----------------------------------------------------------------------------+
//    $History:: rgiVoltageAgreement.cs                     $
// 
// *****************  Version 1  *****************
// User: pnlella     Date: 03/05/19    Time: 11:00  Desc : Created
//----------------------------------------------------------------------------+
using System;
using System.Collections.Generic;
using ADODB;
using Intergraph.GTechnology.API;
using gtCommandLogger;

namespace GTechnology.Oncor.CustomAPI
{
    public class rgiVoltageAgreement 
    {
        #region Private Members

        private List<string> m_lstErrorMessage = new List<string>();
        private List<string> m_lstErrorPriority = new List<string>();

        private string m_sActualAffectedVoltage = null;
        private string m_sActualRelatedVoltage = null;
        private string m_sOtherVoltageAttribute = null;

        private Recordset m_activeConnectivityRS = null;
        private Recordset m_relatedConnectivityRS = null;

        private string m_sProposedAffectedVoltage = null;
        private string m_sProposedRelatedVoltage = null;
        private string m_sOtherProposedVoltageAttribute = null;

        string m_attrActiveVoltage = null;
        string m_attrActiveProposedVoltage = null;

        private string m_errorPriority = null;

        string m_attrRelatedVoltage = null;
        string m_attrRelatedProposedVoltage = null;

        List<short> m_transmissionDevicesFNOs = new List<short> { 59, 60, 98, 99 };

        RGIBaseClass m_rgiBase = null;
        Common m_rgiCommon = null;

        #endregion

        #region Constructor
        public rgiVoltageAgreement(RGIBaseClass p_rgiBaseClass,Common p_rgiCommon)
        {
            m_rgiBase = p_rgiBaseClass;
            m_rgiCommon = p_rgiCommon;
        }

        #endregion

        #region IGTRelationshipGeometry Members
        public void ProcessValidationEstablish()
        {
            try
            {
                ProcessEstablish();
            }
            catch
            {
                throw;
            }
        }
        public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            try
            {
                m_errorPriority = Convert.ToString(m_rgiBase.Arguments.GetArgument(0));
                ProcessValidate(out ErrorPriorityArray,out ErrorMessageArray);
            }
            catch 
            {
                throw;
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Sets voltage attributes when establishing connectivity,based on the voltage attributes corresponding to the connected nodes.
        /// <summary>
        /// <param name="p_errorPriority">Error Priority</param>
        private void ProcessEstablish(string p_errorPriority = null)
        {
            Common.FeatureInfo featureActive = null;
            Common.FeatureInfo featureRelated = null;

            short activeNode;
            short relatedNode;

            try
            {
                m_activeConnectivityRS = m_rgiBase.ActiveFeature.Components.GetComponent(11).Recordset;
                featureActive = new Common.FeatureInfo(m_activeConnectivityRS);

                foreach (IGTKeyObject relatedFeature in m_rgiCommon.GetRelatedFeatures())
                {
                    activeNode = 0;
                    relatedNode = 0;

                    m_relatedConnectivityRS = relatedFeature.Components.GetComponent(11).Recordset;
                    featureRelated = new Common.FeatureInfo(m_relatedConnectivityRS);

                    m_rgiCommon.GetActiveRelatedNodes (m_activeConnectivityRS, m_relatedConnectivityRS, ref activeNode, ref relatedNode);

                    if (activeNode!=0 && relatedNode!=0)
                    {
                        if (m_transmissionDevicesFNOs.Contains(m_rgiCommon.ActiveFNO) && !m_rgiCommon.Validation && activeNode==2)
                        {
                            break;
                        }
                        else
                        {
                            SetAffectedRelatedNodeVoltages(activeNode, relatedNode);
                        }
                    }

                    if (m_rgiCommon.Validation)
                    {
                        ValidateVoltages(featureActive, featureRelated);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to validate Affected RelatedAttribute Values.
        /// </summary>
        private void ProcessValidate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            try
            {
                ErrorPriorityArray = null;
                ErrorMessageArray = null;

                GTValidationLogger gTValidationLogger = null;

                if (new gtLogHelper().CheckIfLoggingIsEnabled())
                {
                    LogEntries logEntries = new LogEntries
                    {
                        ActiveComponentName = Convert.ToString(m_rgiBase.ActiveFeature.CNO),
                        ActiveFID = m_rgiBase.ActiveFeature.FID,
                        ActiveFieldName = "N/A",
                        ActiveFieldValue = "N/A",
                        JobID = m_rgiBase.DataContext.ActiveJob,
                        RelatedComponentName = Convert.ToString(m_rgiBase.RelatedCNO),
                        RelatedFID = 0,
                        RelatedFieldName = "N/A",
                        RelatedFieldValue = "N/A",
                        ValidationInterfaceName = "Voltage Agreement",
                        ValidationInterfaceType = "RGI"
                    };
                    gTValidationLogger = new GTValidationLogger(logEntries);

                    gTValidationLogger.LogEntry("TIMING", "START", "Voltage Agreement Entry", "N/A", "");
                }

                IGTApplication gtApplication = GTClassFactory.Create<IGTApplication>();

                gtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Voltage Agreement Validation Started");

                ProcessEstablish();

                if (m_lstErrorMessage.Count > 0)
                {
                    ErrorMessageArray = m_lstErrorMessage.ToArray();
                    ErrorPriorityArray = m_lstErrorPriority.ToArray();
                }

                gtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Voltage Agreement Validation Completed");

                if (gTValidationLogger != null)
                    gTValidationLogger.LogEntry("TIMING", "END", "Voltage Agreement Exit", "N/A", "");
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// ValidateVoltages:Validates voltage values of connected features and raises a P1 error for mismatch of the connected features.
        /// <summary>
        /// <param name="p_errorPriority">Error Priority</param>
        private void ValidateVoltages()
        {
            string affectedVoltage = null;
            string relatedVoltage = null;
            try
            {
                if(!String.IsNullOrEmpty(m_sProposedAffectedVoltage))
                {
                    affectedVoltage = m_sProposedAffectedVoltage;
                }
                else
                {
                    affectedVoltage = m_sActualAffectedVoltage;
                }

                if (!String.IsNullOrEmpty(m_sProposedRelatedVoltage))
                {
                    relatedVoltage = m_sProposedRelatedVoltage;
                }
                else
                {
                    relatedVoltage = m_sActualRelatedVoltage;
                }

                if((affectedVoltage != null || relatedVoltage != null)  && !affectedVoltage.Equals(relatedVoltage))
                {
                    ValidationRuleManager validateMsg = new ValidationRuleManager();

                    validateMsg.Rule_Id = "VOLT01";
                    validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), null);

                    m_lstErrorPriority.Add(m_errorPriority);
                    m_lstErrorMessage.Add(validateMsg.Rule_MSG);
                }
            }
            catch
            {
                throw;
            }
        }

        private void ValidateVoltages(Common.FeatureInfo p_affected, Common.FeatureInfo p_related)
        {
            try
            {
                List<string> statesProposedUpdate = new List<string> { "PPI", "ABI", "INI", "CLS" };
                bool isAffectedProposedUpdate = statesProposedUpdate.Contains(p_affected.FeatureState);
                bool isRelatedProposedUpdate = statesProposedUpdate.Contains(p_related.FeatureState);

                string affectedVoltage = null;
                string relatedVoltage = null;

                if (isAffectedProposedUpdate && isRelatedProposedUpdate && !String.IsNullOrEmpty(m_sProposedAffectedVoltage))
                {
                    affectedVoltage = m_sProposedAffectedVoltage;
                }
                else
                {
                    affectedVoltage = m_sActualAffectedVoltage;
                }

                if (isAffectedProposedUpdate && isRelatedProposedUpdate && !String.IsNullOrEmpty(m_sProposedRelatedVoltage))
                {
                    relatedVoltage = m_sProposedRelatedVoltage;
                }
                else
                {
                    relatedVoltage = m_sActualRelatedVoltage;
                }
                
                if ((affectedVoltage != null || relatedVoltage != null) && !affectedVoltage.Equals(relatedVoltage))
                {
                    ValidationRuleManager validateMsg = new ValidationRuleManager();

                    validateMsg.Rule_Id = "VOLT01";
                    validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), null);

                    m_lstErrorPriority.Add(m_errorPriority);
                    m_lstErrorMessage.Add(validateMsg.Rule_MSG);
                }
            }
            catch
            {
                throw;
            }
        }

        private void SetAffectedRelatedNodeVoltages(short activeNode,short relatedNode)
         {
            try
            {
                    GetAffectedRelatedVolatageNodes(activeNode, relatedNode);

                    SetAffectedRelatedVoltageValues();

                    if (!m_rgiCommon.Validation)
                    {
                        SetVoltages();
                    }
                
            }
            catch
            {
                throw;
            }
        }

        private void SetVoltages()
        {
            string activeFeatureState = null;
            string relatedFeatureState = null;

            try
            {
               m_rgiCommon.GetFeatureState(null,ref activeFeatureState,true);

                //Updates Proposed Voltages
                if (activeFeatureState == "PPI" || activeFeatureState == "ABI")
                {
                    if (!String.IsNullOrEmpty(m_sProposedRelatedVoltage))
                    {
                        m_activeConnectivityRS.Fields[m_attrActiveProposedVoltage].Value = m_sProposedRelatedVoltage;

                        if (!(m_transmissionDevicesFNOs.Contains(m_rgiBase.ActiveFeature.FNO) || m_rgiBase.ActiveFeature.FNO == 34))
                        {
                            m_activeConnectivityRS.Fields[m_sOtherProposedVoltageAttribute].Value = m_sProposedRelatedVoltage;
                        }
                    }
                    else
                    {
                        m_activeConnectivityRS.Fields[m_attrActiveProposedVoltage].Value = m_sActualRelatedVoltage;

                        if (!(m_transmissionDevicesFNOs.Contains(m_rgiBase.ActiveFeature.FNO) || m_rgiBase.ActiveFeature.FNO == 34))
                        {
                            m_activeConnectivityRS.Fields[m_sOtherProposedVoltageAttribute].Value = m_sActualRelatedVoltage;
                        }
                    }
                }
                else if (String.IsNullOrEmpty(m_sActualAffectedVoltage)) //Updates Actual Voltages
                {
                    m_rgiCommon.GetFeatureState(m_relatedConnectivityRS,ref relatedFeatureState, false);

                    if (!(relatedFeatureState == "PPI" || relatedFeatureState == "ABI"))
                    {
                        m_activeConnectivityRS.Fields[m_attrActiveVoltage].Value = m_sActualRelatedVoltage;

                        if (!(m_transmissionDevicesFNOs.Contains(m_rgiBase.ActiveFeature.FNO) || m_rgiBase.ActiveFeature.FNO == 34))
                        {
                            m_activeConnectivityRS.Fields[m_sOtherVoltageAttribute].Value = m_sActualRelatedVoltage;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void GetAffectedRelatedVolatageNodes(short activeNode, short relatedNode)
        {
            try
            {
                if (activeNode == 1)
                {
                    m_attrActiveVoltage = "VOLT_1_Q";
                    m_attrActiveProposedVoltage = "PP_VOLT_1_Q";

                    m_sOtherVoltageAttribute = "VOLT_2_Q";
                    m_sOtherProposedVoltageAttribute = "PP_VOLT_2_Q";
                }
                else if (activeNode == 2)
                {
                    m_attrActiveVoltage = "VOLT_2_Q";
                    m_attrActiveProposedVoltage = "PP_VOLT_2_Q";

                    m_sOtherVoltageAttribute = "VOLT_1_Q";
                    m_sOtherProposedVoltageAttribute = "PP_VOLT_1_Q";
                }
                if (relatedNode == 1)
                {
                    m_attrRelatedVoltage = "VOLT_1_Q";
                    m_attrRelatedProposedVoltage = "PP_VOLT_1_Q";
                }
                else if (relatedNode == 2)
                {
                    m_attrRelatedVoltage = "VOLT_2_Q";
                    m_attrRelatedProposedVoltage = "PP_VOLT_2_Q";
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Sets corresponding voltage attributes.
        /// </summary>
        /// <param name="actualAffectedVolt">Actual affected feature voltage value</param>
        /// <param name="actualRelatedVolt">Actual related feature voltage value</param>
        /// <param name="proposedAffectedVolt">Proposed affected feature voltage value</param>
        /// <param name="proposedRelatedVolt">Proposed related feature voltage value</param>
        private void SetAffectedRelatedVoltageValues()
        {
            try
            {
                m_sActualAffectedVoltage = Convert.ToString(m_activeConnectivityRS.Fields[m_attrActiveVoltage].Value);
                m_sActualRelatedVoltage = Convert.ToString(m_relatedConnectivityRS.Fields[m_attrRelatedVoltage].Value);

                m_sProposedAffectedVoltage = Convert.ToString(m_activeConnectivityRS.Fields[m_attrActiveProposedVoltage].Value);
                m_sProposedRelatedVoltage = Convert.ToString(m_relatedConnectivityRS.Fields[m_attrRelatedProposedVoltage].Value);
            }
            catch
            {
                throw;
            }
        }
      
        #endregion
    }
}
