//----------------------------------------------------------------------------+
//        Class: rgiVoltageAgreement
//  Description: Copies voltage attributes when establishing connectivity.Validates voltage agreement between directly connected features, based on the voltage attributes corresponding to the connected nodes.
//----------------------------------------------------------------------------+
//     $Author:: pnlella                                   $
//       $Date:: 02/05/18                                 $
//   $Revision:: 1                                        $
//----------------------------------------------------------------------------+
//    $History:: rgiVoltageAgreement.cs                     $
// 
// *****************  Version 1  *****************
// User: pnlella     Date: 02/05/18   Time: 11:00  Desc : Created
// USer :pnlella     Date: 21/03/19  Time : 11.00 Desc : Modified Code to exclude the updation of the null related voltage values to the affected voltage.
//----------------------------------------------------------------------------+
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ADODB;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using gtCommandLogger;

namespace GTechnology.Oncor.CustomAPI
{
    public class rgiVoltageAgreement : RIBase
    {
        #region Private Members
        private string m_sActualAffectedVoltage = null;
        private string m_sActualRelatedVoltage = null;
        private string m_sOtherVoltageAttribute = null;
        private string m_sProposedAffectedVoltage = null;
        private string m_sProposedRelatedVoltage = null;
        private string m_sOtherProposedVoltageAttribute = null;
        private List<string> m_lstErrorMessage = new List<string>();
        private List<string> m_lstErrorPriority = new List<string>();

        #endregion

        #region IGTRelationshipGeometry Members
        public override void AfterEstablish()
        {
            try
            {
                SetAffectedRelatedNodeVoltages();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Voltage Agreement Relationship Geometry Interface- AfterEstablish  \n" + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        public override void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            ErrorPriorityArray = null;
            ErrorMessageArray = null;

            string errorPriority = Convert.ToString(Arguments.GetArgument(0));

            GTValidationLogger gTValidationLogger = null;

            if (new gtLogHelper().CheckIfLoggingIsEnabled())
            {
                LogEntries logEntries = new LogEntries
                {
                    ActiveComponentName = Convert.ToString(ActiveFeature.CNO),
                    ActiveFID = ActiveFeature.FID,
                    ActiveFieldName = "N/A",
                    ActiveFieldValue = "N/A",
                    JobID = DataContext.ActiveJob,
                    RelatedComponentName = Convert.ToString(RelatedCNO),
                    RelatedFID = 0,
                    RelatedFieldName = "N/A",
                    RelatedFieldValue = "N/A",
                    ValidationInterfaceName = "Voltage Agreement",
                    ValidationInterfaceType = "RGI"
                };
                gTValidationLogger = new GTValidationLogger(logEntries);

                gTValidationLogger.LogEntry("TIMING", "START", "Voltage Agreement Entry", "N/A", "");
            }

            try
            {
                IGTApplication gtApplication = GTClassFactory.Create<IGTApplication>();

                gtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Voltage Agreement Validation Started");

                SetAffectedRelatedNodeVoltages(errorPriority);

                if (m_lstErrorMessage.Count > 0)
                {
                    ErrorMessageArray = m_lstErrorMessage.ToArray();
                    ErrorPriorityArray = m_lstErrorPriority.ToArray();
                }

                gtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Voltage Agreement Validation Completed");

                if(gTValidationLogger != null)
                   gTValidationLogger.LogEntry("TIMING", "END", "Voltage Agreement Exit", "N/A", "");

            }
            catch (Exception ex)
            {
                throw new Exception("Error in Voltage Agreement Relationship Geometry Interface-Validate: " + ex.Message);
            }
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Returns the related feature for a corresponding relationship number
        /// <returns></returns>
		/// <summary>
        private IGTKeyObjects GetRelatedFeatures()
        {
            try
            {
                IGTKeyObjects configuredKeyObjects = GTClassFactory.Create<IGTKeyObjects>();

                using (IGTRelationshipService relService = GTClassFactory.Create<IGTRelationshipService>())
                {
                    relService.DataContext = DataContext;
                    relService.ActiveFeature = ActiveFeature;
                    IGTKeyObjects relatedFeatures = relService.GetRelatedFeatures(RNO);

                    if (ProcessingMode.Equals(GTRelationshipGeometryProcessingModeConstants.gtrgiValidation))
                    {
                        foreach (IGTKeyObject item in relatedFeatures)
                        {
                            if (IsFeatureConfiguredInRelationshipGeomtery(item.FNO))
                            {
                                if ((ActiveFeature.FNO == 16 && (item.FNO == 17 || item.FNO == 18)) || ((ActiveFeature.FNO == 17 || ActiveFeature.FNO == 18) && item.FNO == 16))
                                {
                                    continue;
                                }
                                else
                                {
                                    configuredKeyObjects.Add(item);
                                }
                            }
                        }
                    }
                    else
                    {
                        configuredKeyObjects = relatedFeatures;
                    }
                    return configuredKeyObjects;
                }
            }
            catch
            {
                throw;
            }
        }

        private bool IsFeatureConfiguredInRelationshipGeomtery(short p_rFNO)
        {
            bool bReturn = false;
            try
            {
                Recordset rs = DataContext.MetadataRecordset("G3E_NODEEDGECONN_ELEC_OPTABLE");
                rs.Filter= "G3E_RINO =" + 72430462 + " AND G3E_SOURCEFNO =" + ActiveFeature.FNO + " AND G3E_CONNECTINGFNO = " + p_rFNO;

                if(rs!=null && rs.RecordCount>0)
                {
                    bReturn = true;
                }
            }
            catch
            {
                throw;
            }
            return bReturn;
        }

        /// <summary>
        /// Sets voltage attributes when establishing connectivity,based on the voltage attributes corresponding to the connected nodes.
		/// <summary>
        /// <param name="p_errorPriority">Error Priority</param>
        private void SetAffectedRelatedNodeVoltages(string p_errorPriority = null)
        {
            Recordset activeConnectivityRS = null;
            Recordset relatedConnectivityRS = null;
            short connectivityCNO = 11;
            try
            {
                activeConnectivityRS = ActiveFeature.Components.GetComponent(connectivityCNO).Recordset;

                foreach (IGTKeyObject relatedFeature in GetRelatedFeatures())
                {
                    relatedConnectivityRS = relatedFeature.Components.GetComponent(connectivityCNO).Recordset;

                    if (activeConnectivityRS != null && relatedConnectivityRS != null)
                    {
                        if (!(activeConnectivityRS.EOF && activeConnectivityRS.BOF) && !(relatedConnectivityRS.EOF && relatedConnectivityRS.BOF))
                        {
                            activeConnectivityRS.MoveFirst();
                            relatedConnectivityRS.MoveFirst();

                            if (activeConnectivityRS.Fields["NODE_1_ID"].Value != DBNull.Value && (activeConnectivityRS.Fields["NODE_1_ID"].Value.Equals(relatedConnectivityRS.Fields["NODE_1_ID"].Value)))
                            {
                                SetAffectedRelatedValues(Convert.ToString(activeConnectivityRS.Fields["VOLT_1_Q"].Value), Convert.ToString(relatedConnectivityRS.Fields["VOLT_1_Q"].Value), Convert.ToString(activeConnectivityRS.Fields["PP_VOLT_1_Q"].Value), Convert.ToString(relatedConnectivityRS.Fields["PP_VOLT_1_Q"].Value), "VOLT_2_Q", "PP_VOLT_2_Q");

                                if (!ProcessingMode.Equals(GTRelationshipGeometryProcessingModeConstants.gtrgiValidation))
                                {
                                    if (!String.IsNullOrEmpty(m_sActualRelatedVoltage))
                                    {
                                        activeConnectivityRS.Fields["VOLT_1_Q"].Value = m_sActualRelatedVoltage;
                                    }
                                    if (!String.IsNullOrEmpty(m_sProposedRelatedVoltage))
                                    {
                                        activeConnectivityRS.Fields["PP_VOLT_1_Q"].Value = m_sProposedRelatedVoltage;
                                    }
                                }
                            }
                            else if (activeConnectivityRS.Fields["NODE_2_ID"].Value != DBNull.Value && (activeConnectivityRS.Fields["NODE_2_ID"].Value.Equals(relatedConnectivityRS.Fields["NODE_2_ID"].Value)))
                            {
                                SetAffectedRelatedValues(Convert.ToString(activeConnectivityRS.Fields["VOLT_2_Q"].Value), Convert.ToString(relatedConnectivityRS.Fields["VOLT_2_Q"].Value), Convert.ToString(activeConnectivityRS.Fields["PP_VOLT_2_Q"].Value), Convert.ToString(relatedConnectivityRS.Fields["PP_VOLT_2_Q"].Value), "VOLT_1_Q", "PP_VOLT_1_Q");

                                if (!ProcessingMode.Equals(GTRelationshipGeometryProcessingModeConstants.gtrgiValidation))
                                {
                                    if (!String.IsNullOrEmpty(m_sActualRelatedVoltage))
                                    {
                                        activeConnectivityRS.Fields["VOLT_2_Q"].Value = m_sActualRelatedVoltage;
                                    }
                                    if (!String.IsNullOrEmpty(m_sProposedRelatedVoltage))
                                    {
                                        activeConnectivityRS.Fields["PP_VOLT_2_Q"].Value = m_sProposedRelatedVoltage;
                                    }
                                }
                            }
                            else if (activeConnectivityRS.Fields["NODE_1_ID"].Value != DBNull.Value && (activeConnectivityRS.Fields["NODE_1_ID"].Value.Equals(relatedConnectivityRS.Fields["NODE_2_ID"].Value)))
                            {
                                SetAffectedRelatedValues(Convert.ToString(activeConnectivityRS.Fields["VOLT_1_Q"].Value), Convert.ToString(relatedConnectivityRS.Fields["VOLT_2_Q"].Value), Convert.ToString(activeConnectivityRS.Fields["PP_VOLT_1_Q"].Value), Convert.ToString(relatedConnectivityRS.Fields["PP_VOLT_2_Q"].Value), "VOLT_2_Q", "PP_VOLT_2_Q");

                                if (!ProcessingMode.Equals(GTRelationshipGeometryProcessingModeConstants.gtrgiValidation))
                                {
                                    if (!String.IsNullOrEmpty(m_sActualRelatedVoltage))
                                    {
                                        activeConnectivityRS.Fields["VOLT_1_Q"].Value = m_sActualRelatedVoltage;
                                    }
                                    if (!String.IsNullOrEmpty(m_sProposedRelatedVoltage))
                                    {
                                        activeConnectivityRS.Fields["PP_VOLT_1_Q"].Value = m_sProposedRelatedVoltage;
                                    }
                                }
                            }
                            else if (activeConnectivityRS.Fields["NODE_2_ID"].Value != DBNull.Value && (activeConnectivityRS.Fields["NODE_2_ID"].Value.Equals(relatedConnectivityRS.Fields["NODE_1_ID"].Value)))
                            {
                                SetAffectedRelatedValues(Convert.ToString(activeConnectivityRS.Fields["VOLT_2_Q"].Value), Convert.ToString(relatedConnectivityRS.Fields["VOLT_1_Q"].Value), Convert.ToString(activeConnectivityRS.Fields["PP_VOLT_2_Q"].Value), Convert.ToString(relatedConnectivityRS.Fields["PP_VOLT_1_Q"].Value), "VOLT_1_Q", "PP_VOLT_1_Q");

                                if (!ProcessingMode.Equals(GTRelationshipGeometryProcessingModeConstants.gtrgiValidation))
                                {
                                    if (!String.IsNullOrEmpty(m_sActualRelatedVoltage))
                                    {
                                        activeConnectivityRS.Fields["VOLT_2_Q"].Value = m_sActualRelatedVoltage;
                                    }
                                    if (!String.IsNullOrEmpty(m_sProposedRelatedVoltage))
                                    {
                                        activeConnectivityRS.Fields["PP_VOLT_2_Q"].Value = m_sProposedRelatedVoltage;
                                    }
                                }
                            }
                        }
                    }
                    if (ProcessingMode.Equals(GTRelationshipGeometryProcessingModeConstants.gtrgiValidation))
                    {
                        ValidateVoltages(p_errorPriority);
                    }
                    else
                    {
                        CopyAffectedRelatedNodeVoltages();
                    }
                }
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
        private void ValidateVoltages(string p_errorPriority)
        {
            try
            {
                if ((m_sActualAffectedVoltage != null && !m_sActualAffectedVoltage.Equals(m_sActualRelatedVoltage)) || (m_sProposedAffectedVoltage != null && !m_sProposedAffectedVoltage.Equals(m_sProposedRelatedVoltage)))
                {
                    ValidationRuleManager validateMsg = new ValidationRuleManager();

                    validateMsg.Rule_Id = "VOLT01";
                    validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), null);

                    m_lstErrorPriority.Add(p_errorPriority);
                    m_lstErrorMessage.Add(validateMsg.Rule_MSG);
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
        /// <param name="actualOtherVoltAttr">Actual affected feature other voltage attribute</param>
        /// <param name="proposedOtherVoltAttr">Proposed affected feature other voltage attribute</param>
        private void SetAffectedRelatedValues(string actualAffectedVolt, string actualRelatedVolt, string proposedAffectedVolt, string proposedRelatedVolt, string actualOtherVoltAttr, string proposedOtherVoltAttr)
        {
            try
            {
                m_sActualAffectedVoltage = actualAffectedVolt;
                m_sActualRelatedVoltage = actualRelatedVolt;
                m_sProposedAffectedVoltage = proposedAffectedVolt;
                m_sProposedRelatedVoltage = proposedRelatedVolt;
                m_sOtherVoltageAttribute = actualOtherVoltAttr;
                m_sOtherProposedVoltageAttribute = proposedOtherVoltAttr;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Copies voltage attributes when establishing connectivity,based on the voltage attributes corresponding to the connected nodes.
        /// <summary>
        private void CopyAffectedRelatedNodeVoltages()
        {
            Recordset activeConnectivityRS = null;
            List<short> transmissionDevicesFNOs = new List<short> { 34, 59, 60, 98, 99 };
            short connectivityCNO = 11;
            try
            {
                activeConnectivityRS = ActiveFeature.Components.GetComponent(connectivityCNO).Recordset;
                activeConnectivityRS.MoveFirst();

                if (!transmissionDevicesFNOs.Contains(ActiveFeature.FNO))
                {
                    if (!String.IsNullOrEmpty(m_sActualRelatedVoltage))
                    {
                        activeConnectivityRS.Fields[m_sOtherVoltageAttribute].Value = m_sActualRelatedVoltage;
                    }
                    if (!String.IsNullOrEmpty(m_sProposedRelatedVoltage))
                    {
                        activeConnectivityRS.Fields[m_sOtherProposedVoltageAttribute].Value = m_sProposedRelatedVoltage;
                    }
                    
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
