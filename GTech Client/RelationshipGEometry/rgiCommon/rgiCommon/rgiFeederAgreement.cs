//----------------------------------------------------------------------------+
//        Class: rgiFeederAgreement
//  Description: This interface sets FeederType,Feeder ID/Tie Feeder ID, Feeder Number/Tie Feeder Number, 
//              and Substation Code/Tie Substation Code value of connectivity component.
//----------------------------------------------------------------------------+
//     $Author:: pnlella                                   $
//       $Date:: 03/05/19                                 $
//   $Revision:: 1                                        $
//----------------------------------------------------------------------------+
//    $History:: rgiFeederAgreement.cs                     $
// 
// *****************  Version 1  *****************
// User: pnlella     Date: 03/05/19    Time: 11:00  Desc : Created
// User: pnlella     Date: 17/05/19    Time: 11:00  Desc : Fixed ALM-2405.
//----------------------------------------------------------------------------+
using System;
using System.Collections.Generic;
using ADODB;
using Intergraph.GTechnology.API;
using gtCommandLogger;

namespace GTechnology.Oncor.CustomAPI
{
    public class rgiFeederAgreement
    {
        #region Private Members

        private List<string> m_lstErrorMessage = new List<string>();
        private List<string> m_lstErrorPriority = new List<string>();

        private Recordset m_activeConnectivityRS = null;
        private Recordset m_relatedConnectivityRS = null;

        private string m_activeFeatureState = null;
        private string m_relatedFeatureState = null;
        private string m_activeAsOperatedStatus = null;
        private string m_activeNormalStatus = null;

        private string m_attrActiveFeederID = null;
        private string m_attrActiveFeederNbr = null;
        private string m_attrActiveSubstationCode = null;

        private string m_attrRelatedFeederID = null;
        private string m_attrRelatedFeederNbr = null;
        private string m_attrRelatedSubstationCode = null;

        private string m_aFeederType = null;
        private string m_rFeederType = null;

        private string m_aActualFeederID = null;
        private string m_rActualFeederID = null;
        private string m_aProposedFeederID = null;
        private string m_rProposedFeederID = null;

        private string m_aActualFeederNbr = null;
        private string m_rActualFeederNbr = null;
        private string m_aProposedFeederNbr = null;
        private string m_rProposedFeederNbr = null;

        private string m_aActualSubstationCode = null;
        private string m_rActualSubstationCode = null;
        private string m_aProposedSubstationCode = null;
        private string m_rProposedSubstationCode = null;

        private string m_errorPriority = null;

        private bool m_secondaryNetworkFeature = false;
        private bool m_updateTie = false;

        ValidationRuleManager m_validateMsg;
        object[] m_messArguments;

        RGIBaseClass m_rgiBaseClass = null;
        Common m_rgiCommon = null;
        #endregion

        #region Constructor
        public rgiFeederAgreement(RGIBaseClass p_rgiBaseClass, Common p_rgiCommon)
        {
            m_rgiBaseClass = p_rgiBaseClass;
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
                m_errorPriority = Convert.ToString(m_rgiBaseClass.Arguments.GetArgument(0));
                m_validateMsg = new ValidationRuleManager();
                m_messArguments = new object[4];
                ProcessValidate(out ErrorPriorityArray, out ErrorMessageArray);
            }
            catch
            {
                throw;
            }
            finally
            {
                m_validateMsg = null;
                m_messArguments = null;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Method to Check for the SecondaryNetworkFeature.
        /// </summary>
        private void IsSecondaryNetworkFeature()
        {
            List<short> lstSecondaryNetworkFNO = new List<short> { 23, 86, 94, 95, 96, 97 };
            try
            {
                if (lstSecondaryNetworkFNO.Contains(m_rgiBaseClass.ActiveFeature.FNO))
                {
                    m_secondaryNetworkFeature = true;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                lstSecondaryNetworkFNO = null;
            }
        }

        /// <summary>
        /// Method to set for the As Operated Status.
        /// </summary>
        private void SetOperatedStatus()
        {
            try
            {
                if (m_activeConnectivityRS != null && m_activeConnectivityRS.RecordCount > 0)
                {
                    m_activeConnectivityRS.MoveFirst();
                    m_activeAsOperatedStatus = Convert.ToString(m_activeConnectivityRS.Fields["STATUS_OPERATED_C"].Value);
                    m_activeNormalStatus = Convert.ToString(m_activeConnectivityRS.Fields["STATUS_NORMAL_C"].Value);

                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to set for ProcessEstablish.
        /// </summary>
        private void ProcessEstablish()
        {
            short activeNode;
            short relatedNode;
            string activeFeatureStatus = null;
            bool normalStatus = false;
            try
            {
                IsSecondaryNetworkFeature();

                m_activeConnectivityRS = m_rgiBaseClass.ActiveFeature.Components.GetComponent(11).Recordset;

                m_rgiCommon.GetFeatureState(null, ref m_activeFeatureState, true);
                SetOperatedStatus();

                if (m_activeAsOperatedStatus.ToUpper() == "OPEN")
                {
                    m_activeConnectivityRS.MoveFirst();
                    m_activeConnectivityRS.Fields["UPSTREAM_NODE"].Value = 1;
                    m_activeConnectivityRS.Fields["PP_UPSTREAM_NODE"].Value = 1;
                }

                foreach (IGTKeyObject relatedFeature in m_rgiCommon.GetRelatedFeatures())
                {
                    m_relatedConnectivityRS = relatedFeature.Components.GetComponent(11).Recordset;

                    m_rgiCommon.GetFeatureState(m_relatedConnectivityRS, ref m_relatedFeatureState, false);
                    //SetOperatedStatus(false);

                    activeNode = 0;
                    relatedNode = 0;

                    m_rgiCommon.GetActiveRelatedNodes(m_activeConnectivityRS, m_relatedConnectivityRS, ref activeNode, ref relatedNode);

                    CheckNormalOrAsOperatedStatus(ref activeFeatureStatus, ref normalStatus);

                    DetermineTieOrNonTie(activeNode, relatedNode);

                    SetAffectedRelatedAttributeValues();

                    if (m_rgiCommon.Validation)
                    {
                        ValidateFeederAttributes();
                    }
                    else
                    {
                        if (activeFeatureStatus.ToUpper() == "CLOSED")
                        {
                            if (normalStatus)
                            {
                                if (m_updateTie)
                                {
                                    UpdateOtherNodeAttributes("PP_FEEDER_2_ID", "PP_FEEDER_1_ID");
                                    UpdateOtherNodeAttributes("PP_TIE_FEEDER_NBR", "PP_FEEDER_NBR");
                                    UpdateOtherNodeAttributes("PP_TIE_SSTA_C", "PP_SSTA_C");
                                }
                                else
                                {
                                    UpdateOtherNodeAttributes("PP_FEEDER_1_ID", "PP_FEEDER_2_ID");
                                    UpdateOtherNodeAttributes("PP_FEEDER_NBR", "PP_TIE_FEEDER_NBR");
                                    UpdateOtherNodeAttributes("PP_SSTA_C", "PP_TIE_SSTA_C");
                                }
                            }
                            else
                            {
                                if (m_updateTie)
                                {
                                    UpdateOtherNodeAttributes("FEEDER_2_ID","FEEDER_1_ID");
                                    UpdateOtherNodeAttributes("TIE_FEEDER_NBR","FEEDER_NBR");
                                    UpdateOtherNodeAttributes("TIE_SSTA_C","SSTA_C");
                                }
                                else
                                {
                                    UpdateOtherNodeAttributes("FEEDER_1_ID", "FEEDER_2_ID");
                                    UpdateOtherNodeAttributes("FEEDER_NBR", "TIE_FEEDER_NBR");
                                    UpdateOtherNodeAttributes("SSTA_C", "TIE_SSTA_C");
                                }
                            }
                        }

                        // Lets keep this commented code for now until we finalize the code passed in all workflow testing
                        /*
                        else if(m_activeAsOperatedStatus.ToUpper().Equals("OPEN"))
                        {
                            if(activeNode==1)
                            {
                                    m_activeConnectivityRS.Fields["PP_FEEDER_2_ID"].Value = null;
                                    m_activeConnectivityRS.Fields["PP_TIE_FEEDER_NBR"].Value = null;
                                    m_activeConnectivityRS.Fields["PP_TIE_SSTA_C"].Value = null;
                                    m_activeConnectivityRS.Fields["FEEDER_2_ID"].Value = null;
                                    m_activeConnectivityRS.Fields["TIE_FEEDER_NBR"].Value = null;
                                    m_activeConnectivityRS.Fields["TIE_SSTA_C"].Value = null;
                            }
                            else if(activeNode==2)
                            {
                                m_activeConnectivityRS.Fields["PP_FEEDER_1_ID"].Value = null;
                                m_activeConnectivityRS.Fields["PP_FEEDER_NBR"].Value = null;
                                m_activeConnectivityRS.Fields["PP_SSTA_C"].Value = null;
                                m_activeConnectivityRS.Fields["FEEDER_1_ID"].Value = null;
                                m_activeConnectivityRS.Fields["FEEDER_NBR"].Value = null;
                                m_activeConnectivityRS.Fields["SSTA_C"].Value = null;
                            }
                        }*/
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void CheckNormalOrAsOperatedStatus(ref string activeFeatureStatus, ref bool normalStatus)
        {
            try
            {
                if (m_activeFeatureState == "PPI" || m_activeFeatureState == "ABI")
                {
                    activeFeatureStatus = m_activeNormalStatus;
                    normalStatus = true;
                }
                else
                {
                    activeFeatureStatus = m_activeAsOperatedStatus;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method for Validate.
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
                        ActiveComponentName = Convert.ToString(m_rgiBaseClass.ActiveFeature.CNO),
                        ActiveFID = m_rgiBaseClass.ActiveFeature.FID,
                        ActiveFieldName = "N/A",
                        ActiveFieldValue = "N/A",
                        JobID = m_rgiBaseClass.DataContext.ActiveJob,
                        RelatedComponentName = Convert.ToString(m_rgiBaseClass.RelatedCNO),
                        RelatedFID = 0,
                        RelatedFieldName = "N/A",
                        RelatedFieldValue = "N/A",
                        ValidationInterfaceName = "Feeder Agreement",
                        ValidationInterfaceType = "RGI"
                    };
                    gTValidationLogger = new GTValidationLogger(logEntries);

                    gTValidationLogger.LogEntry("TIMING", "START", "Feeder Agreement Entry", "N/A", "");
                }

                IGTApplication gtApplication = GTClassFactory.Create<IGTApplication>();

                gtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Feeder Agreement Validation Started");

                ProcessEstablish();


                if (m_lstErrorMessage.Count > 0)
                {
                    ErrorMessageArray = m_lstErrorMessage.ToArray();
                    ErrorPriorityArray = m_lstErrorPriority.ToArray();
                }

                gtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Feeder Agreement Validation Completed");

                if (gTValidationLogger != null)
                    gTValidationLogger.LogEntry("TIMING", "END", "Feeder Agreement Exit", "N/A", "");
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to set Affected RelatedAttribute Values.
        /// </summary>
        private void SetAffectedRelatedAttributeValues()
        {
            string activeProposedName = null;
            string relatedProposedName = null;
            try
            {
                m_activeConnectivityRS.MoveFirst();
                m_relatedConnectivityRS.MoveFirst();

                //Feeder Type
                m_aFeederType = Convert.ToString(m_activeConnectivityRS.Fields["FEEDER_TYPE_C"].Value);
                m_rFeederType = Convert.ToString(m_relatedConnectivityRS.Fields["FEEDER_TYPE_C"].Value);

                if (String.IsNullOrEmpty(m_aFeederType) && !m_rgiCommon.Validation)
                {
                    m_activeConnectivityRS.Fields["FEEDER_TYPE_C"].Value = m_rFeederType;
                }

                //Feeder ID

                if (!m_secondaryNetworkFeature)
                {
                    if (!String.IsNullOrEmpty(m_attrActiveFeederID) && !String.IsNullOrEmpty(m_attrRelatedFeederID))
                    {
                        m_aActualFeederID = Convert.ToString(m_activeConnectivityRS.Fields[m_attrActiveFeederID].Value);
                        m_rActualFeederID = Convert.ToString(m_relatedConnectivityRS.Fields[m_attrRelatedFeederID].Value);

                        activeProposedName = "PP_" + m_attrActiveFeederID;
                        relatedProposedName = "PP_" + m_attrRelatedFeederID;

                        m_aProposedFeederID = Convert.ToString(m_activeConnectivityRS.Fields[activeProposedName].Value);
                        m_rProposedFeederID = Convert.ToString(m_relatedConnectivityRS.Fields[relatedProposedName].Value);

                        if (!m_rgiCommon.Validation)
                        {
                            SetValues(m_attrActiveFeederID, activeProposedName, m_aActualFeederID, m_aProposedFeederID, m_rProposedFeederID, m_rActualFeederID);
                        }
                    }
                }

                //Feeder Number

                if (!m_secondaryNetworkFeature)
                {
                    if (!String.IsNullOrEmpty(m_attrActiveFeederNbr) && !String.IsNullOrEmpty(m_attrRelatedFeederNbr))
                    {
                        m_aActualFeederNbr = Convert.ToString(m_activeConnectivityRS.Fields[m_attrActiveFeederNbr].Value);
                        m_rActualFeederNbr = Convert.ToString(m_relatedConnectivityRS.Fields[m_attrRelatedFeederNbr].Value);

                        activeProposedName = "PP_" + m_attrActiveFeederNbr;
                        relatedProposedName = "PP_" + m_attrRelatedFeederNbr;

                        m_aProposedFeederNbr = Convert.ToString(m_activeConnectivityRS.Fields[activeProposedName].Value);
                        m_rProposedFeederNbr = Convert.ToString(m_relatedConnectivityRS.Fields[relatedProposedName].Value);

                        if (!m_rgiCommon.Validation)
                        {
                            SetValues(m_attrActiveFeederNbr, activeProposedName, m_aActualFeederNbr, m_aProposedFeederNbr, m_rProposedFeederNbr, m_rActualFeederNbr);
                        }
                    }
                }
                //Substation Code
                if (!String.IsNullOrEmpty(m_attrActiveSubstationCode) && !String.IsNullOrEmpty(m_attrRelatedSubstationCode))
                {
                    m_aActualSubstationCode = Convert.ToString(m_activeConnectivityRS.Fields[m_attrActiveSubstationCode].Value);
                    m_rActualSubstationCode = Convert.ToString(m_relatedConnectivityRS.Fields[m_attrRelatedSubstationCode].Value);

                    activeProposedName = "PP_" + m_attrActiveSubstationCode;
                    relatedProposedName = "PP_" + m_attrRelatedSubstationCode;

                    m_aProposedSubstationCode = Convert.ToString(m_activeConnectivityRS.Fields[activeProposedName].Value);
                    m_rProposedSubstationCode = Convert.ToString(m_relatedConnectivityRS.Fields[relatedProposedName].Value);

                    if (!m_rgiCommon.Validation)
                    {
                        SetValues(m_attrActiveSubstationCode, activeProposedName, m_aActualSubstationCode, m_aProposedSubstationCode, m_rProposedSubstationCode, m_rActualSubstationCode);
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Method to send Affected RelatedAttribute Values for validation.
        /// </summary>
        private void ValidateFeederAttributes()
        {
            string activeFieldValue = null;
            string relatedFieldValue = null;
            try
            {
                // Feeder Type
                if (!String.IsNullOrEmpty(m_aFeederType) || !String.IsNullOrEmpty(m_rFeederType))
                {
                    activeFieldValue = m_aFeederType;
                    relatedFieldValue = m_rFeederType;

                    if (m_activeAsOperatedStatus.ToUpper() != "OPEN" && !m_aFeederType.Equals(m_rFeederType))
                    {
                        AddErrorList(activeFieldValue, relatedFieldValue, "FEEDER_TYPE_C");
                    }
                }
                //Feeder ID 
                if (!m_secondaryNetworkFeature)
                {
                    if (ValidateValues(ref activeFieldValue, ref relatedFieldValue, m_aProposedFeederID, m_rProposedFeederID, m_aActualFeederID, m_rActualFeederID))
                    {
                        AddErrorList(activeFieldValue, relatedFieldValue, m_attrActiveFeederID);
                    }
                }
                //Feeder Number

                if (!m_secondaryNetworkFeature)
                {
                    if (ValidateValues(ref activeFieldValue, ref relatedFieldValue, m_aProposedFeederNbr, m_rProposedFeederNbr, m_aActualFeederNbr, m_rActualFeederNbr))
                    {
                        AddErrorList(activeFieldValue, relatedFieldValue, m_attrActiveFeederNbr);
                    }
                }

                //Substation Code
                if (ValidateValues(ref activeFieldValue, ref relatedFieldValue, m_aProposedSubstationCode, m_rProposedSubstationCode, m_aActualSubstationCode, m_rActualSubstationCode))
                {
                    AddErrorList(activeFieldValue, relatedFieldValue, m_attrActiveSubstationCode);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to add to error list.
        /// </summary>
        private void AddErrorList(string p_activeFieldValue, string p_relatedFieldValue, string p_attributeName)
        {
            try
            {
                m_messArguments[0] = p_attributeName;
                m_messArguments[1] = p_activeFieldValue;
                m_messArguments[2] = p_relatedFieldValue;
                m_messArguments[3] = m_relatedConnectivityRS.Fields["G3E_FID"].Value;

                m_validateMsg.Rule_Id = "FEED01";
                m_validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), m_messArguments);

                m_lstErrorPriority.Add(m_errorPriority);
                m_lstErrorMessage.Add(m_validateMsg.Rule_MSG);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to validate Affected RelatedAttribute Values.
        /// </summary>
        private bool ValidateValues(ref string activeFieldValue, ref string relatedFieldValue, string m_aProposedValue, string m_rProposedValue, string m_aActualValue, string m_rActualValue)
        {
            bool validate = false;
            try
            {
                if (!String.IsNullOrEmpty(m_aProposedValue))
                {
                    activeFieldValue = m_aProposedValue;
                }
                else
                {
                    activeFieldValue = m_aActualValue;
                }
                if (!String.IsNullOrEmpty(m_rProposedValue))
                {
                    relatedFieldValue = m_rProposedValue;
                }
                else
                {
                    relatedFieldValue = m_rActualValue;
                }
                if ((activeFieldValue != null || relatedFieldValue != null) && !activeFieldValue.Equals(relatedFieldValue))
                {
                    validate = true;
                }
            }
            catch
            {
                throw;
            }
            return validate;
        }

        /// <summary>
        /// Method to update tie attributes same as non tie attributes.
        /// </summary>
        private void UpdateOtherNodeAttributes(string p_otherNodeAttr, string p_activeNodeAttr)
        {
            try
            {
                m_activeConnectivityRS.MoveFirst();

                if (String.IsNullOrEmpty(Convert.ToString(m_activeConnectivityRS.Fields[p_otherNodeAttr].Value)))
                {
                    m_activeConnectivityRS.Fields[p_otherNodeAttr].Value = m_activeConnectivityRS.Fields[p_activeNodeAttr].Value;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to set active attribute values based on feature state.
        /// </summary>
        private void SetValues(string p_actualAttr, string p_proposedAttr, string p_aActualValue, string p_aProposedValue, string p_rProposedValue, string p_rActualValue)
        {
            try
            {
                m_activeConnectivityRS.MoveFirst();

                if (m_activeFeatureState == "PPI" || m_activeFeatureState == "ABI")
                {
                    if (String.IsNullOrEmpty(p_aProposedValue))
                    {
                        if (!String.IsNullOrEmpty(p_rProposedValue))
                        {
                            m_activeConnectivityRS.Fields[p_proposedAttr].Value = p_rProposedValue;
                        }
                        else
                        {
                            m_activeConnectivityRS.Fields[p_proposedAttr].Value = p_rActualValue;
                        }
                    }
                }
                else if (String.IsNullOrEmpty(p_aActualValue))
                {
                    if (!(m_relatedFeatureState == "PPI" || m_relatedFeatureState == "ABI"))
                    {
                        m_activeConnectivityRS.Fields[p_actualAttr].Value = p_rActualValue;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to tie or non tie attributes to be selected for active and related features.
        /// </summary>
        private void DetermineTieOrNonTie(short p_aNode, short p_rNode)
        {
            try
            {
                if (p_aNode == 1 && p_rNode == 1)
                {
                    m_attrRelatedFeederID = m_attrActiveFeederID = "FEEDER_1_ID";
                    m_attrRelatedFeederNbr = m_attrActiveFeederNbr = "FEEDER_NBR";
                    m_attrRelatedSubstationCode = m_attrActiveSubstationCode = "SSTA_C";
                    m_updateTie = true;
                }
                else if (p_aNode == 1 && p_rNode == 2)
                {
                    m_attrActiveFeederID = "FEEDER_1_ID";
                    m_attrActiveFeederNbr = "FEEDER_NBR";
                    m_attrActiveSubstationCode = "SSTA_C";

                    m_attrRelatedFeederID = "FEEDER_2_ID";
                    m_attrRelatedFeederNbr = "TIE_FEEDER_NBR";
                    m_attrRelatedSubstationCode = "TIE_SSTA_C";

                    m_updateTie = true;
                }
                else if (p_aNode == 2 && p_rNode == 1)
                {
                    m_attrActiveFeederID = "FEEDER_2_ID";
                    m_attrActiveFeederNbr = "TIE_FEEDER_NBR";
                    m_attrActiveSubstationCode = "TIE_SSTA_C";

                    m_attrRelatedFeederID = "FEEDER_1_ID";
                    m_attrRelatedFeederNbr = "FEEDER_NBR";
                    m_attrRelatedSubstationCode = "SSTA_C";

                    m_updateTie = false;
                }
                else if (p_aNode == 2 && p_rNode == 2)
                {
                    m_attrRelatedFeederID = m_attrActiveFeederID = "FEEDER_2_ID";
                    m_attrRelatedFeederNbr = m_attrActiveFeederNbr = "TIE_FEEDER_NBR";
                    m_attrRelatedSubstationCode = m_attrActiveSubstationCode = "TIE_SSTA_C";

                    m_updateTie = false;
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
