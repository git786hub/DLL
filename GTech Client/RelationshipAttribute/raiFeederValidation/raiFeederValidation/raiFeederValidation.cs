// ===================================================
//  Copyright 2018 Intergraph Corp.
//  File Name: raiFeederValidation.cs
// 
//  Description:This interface sets FeederType,Feeder ID/Tie Feeder ID, Feeder Number/Tie Feeder Number, 
//              and Substation Code/Tie Substation Code value of connectivity component.
//
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  26/04/2018          Sithara                  
//  28/05/2018          Prathyusha                  Modified the code as per the new Requirements mentioned in JIRA-1610.
// ======================================================
using Intergraph.GTechnology.API;
using System;
using System.Windows.Forms;
using gtCommandLogger;

namespace GTechnology.Oncor.CustomAPI
{
    public class raiFeederValidation : RIBase
    {
        #region Private Members
        private string m_sTieAttribute = null;
        private string m_sNormalStatus = null;
        #endregion

        #region IGTFunctional Members
        public override void AfterEstablish()
        {
            DataAccessLayer dataAccessLayer = new DataAccessLayer(DataContext);
            try
            {
                SetVariables(dataAccessLayer);

                string strFeederAttributeVal = Convert.ToString(RelatedFieldValue.FieldValue);

                if (!string.IsNullOrEmpty(strFeederAttributeVal))
                {
                    dataAccessLayer.SetFeederAttribute(ActiveComponents[ActiveComponentName], strFeederAttributeVal, ActiveFieldName, m_sTieAttribute,m_sNormalStatus);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Feeder Validation Relationship Attribute Interface \n" + ex.Message, "G/Technology");
            }
        }

        public override void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            ErrorPriorityArray = new string[1];
            ErrorMessageArray = new string[1];


            IGTKeyObject oRelatedFeature = null;
            IGTKeyObjects oRelatedFeatureOwnedBy = null;
            string sPriCondNodeType = string.Empty;

            IGTKeyObject oActiveFeature = null;
            IGTKeyObjects oActiveFeatureOwnedBy = null;
            int activePhaseCount = 0;

            GTValidationLogger gTValidationLogger = null;
            
            int activeFID = 0;
            int relatedFID = 0;

            string activeFieldValue = string.Empty;
            string relatedFieldValue = string.Empty;

            IGTComponent activeComponent = ActiveComponents[ActiveComponentName];
            if (activeComponent != null && activeComponent.Recordset !=null && activeComponent.Recordset.RecordCount > 0)
            {
                activeFID = int.Parse(activeComponent.Recordset.Fields["G3E_FID"].Value.ToString());
                activeFieldValue = Convert.ToString(activeComponent.Recordset.Fields[ActiveFieldName].Value);
            }

            IGTComponent relatedComponent = RelatedComponents[RelatedComponentName];
            if (relatedComponent != null && relatedComponent.Recordset != null && relatedComponent.Recordset.RecordCount > 0)
            {
                relatedFID = int.Parse(relatedComponent.Recordset.Fields["G3E_FID"].Value.ToString());
                relatedFieldValue = Convert.ToString(relatedComponent.Recordset.Fields[RelatedFieldName].Value);
            }

            if (new gtLogHelper().CheckIfLoggingIsEnabled())
            {
                LogEntries logEntries = new LogEntries
                {
                    ActiveComponentName = ActiveComponentName,
                    ActiveFID = activeFID,
                    ActiveFieldName = ActiveFieldName,
                    ActiveFieldValue = activeFieldValue,
                    JobID = DataContext.ActiveJob,
                    RelatedComponentName = RelatedComponentName,
                    RelatedFID = relatedFID,
                    RelatedFieldName = RelatedFieldName,
                    RelatedFieldValue = relatedFieldValue,
                    ValidationInterfaceName = "Feeder Validation",
                    ValidationInterfaceType = "RAI",
                };
                gTValidationLogger = new GTValidationLogger(logEntries);

                gTValidationLogger.LogEntry("TIMING", "START", "Feeder Validation Entry", "N/A", "");
            }


            DataAccessLayer dataAccessLayer = new DataAccessLayer(DataContext);
            try
            {
                GTClassFactory.Create<IGTApplication>().SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Feeder Validation Started");

                SetVariables(dataAccessLayer);

                string strRelatedFeederAttribute = Convert.ToString(RelatedFieldValue.FieldValue);
                string strActiveFeederAttribute = Convert.ToString(ActiveFieldValue.FieldValue);

                if (m_sTieAttribute != null)
                {
                    string strActiveTieFeederAttribute = dataAccessLayer.GetTieFeederAttribute(ActiveComponents[ActiveComponentName], m_sTieAttribute);

                    if (!string.Equals(strRelatedFeederAttribute, strActiveFeederAttribute) && !string.Equals(strRelatedFeederAttribute, strActiveTieFeederAttribute))
                    {
                        ShowValidation(out ErrorPriorityArray[0], out ErrorMessageArray[0]);
                    }
                }
                else
                {
                    if (!string.Equals(strRelatedFeederAttribute, strActiveFeederAttribute) && string.Equals(m_sNormalStatus.ToUpper(), "CLOSED"))
                    {
                        ShowValidation(out ErrorPriorityArray[0], out ErrorMessageArray[0]);
                    }
                }

                GTClassFactory.Create<IGTApplication>().SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Network Management Validation Completed");

                if(gTValidationLogger != null)
                    gTValidationLogger.LogEntry("TIMING", "END", "Feeder Validation Exit", "N/A", "");

            }
            catch (Exception ex)
            {
                throw new Exception("Error in Feeder Validation Relationship Attribute Interface \n" + ex.Message);
            }
            finally
            {
                dataAccessLayer = null;
            }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Method to get the Validation Message
        /// </summary>
        /// <param name="ErrorPriority"></param>
        /// <param name="ErrorMessage"></param>
        private void ShowValidation(out string ErrorPriority, out string ErrorMessage)
        {
            ValidationRuleManager validateMsg = new ValidationRuleManager();
            object[] messArguments = new object[3];
            try
            {
                validateMsg.Rule_Id = "FEED01";
                messArguments[0] = ActiveFieldName;
                messArguments[1] = ActiveFieldValue.FieldValue;
                messArguments[2] = RelatedFieldValue.FieldValue;
                validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), messArguments);

                ErrorPriority = Priority;
                ErrorMessage = validateMsg.Rule_MSG;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method to set the TieAttribute and Normal Status attributes.
        /// </summary>
        /// <param name="dataAccessLayer"></param>
        private void SetVariables(DataAccessLayer dataAccessLayer)
        {
            try
            {
                long l_tieNumber = Convert.ToInt64(Arguments.GetArgument(0));

                if (l_tieNumber != 0)
                {
                    m_sTieAttribute = dataAccessLayer.GetTieFeederAttributeFieldName(l_tieNumber, ActiveComponents[ActiveComponentName].CNO);
                }

                m_sNormalStatus = dataAccessLayer.GetNormalStatus(ActiveComponents[ActiveComponentName]);
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
