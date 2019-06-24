//----------------------------------------------------------------------------+
//        Class: fiGenerateNjunsTicket
//  Description: This functional interface checks precondtions and creates ticket when condtions are passed. This also validates the required attributes and ticket number throws P1 error if the validations is failed.
//
//----------------------------------------------------------------------------+
//     $Author:: hkonda                                   $
//       $Date:: 30/03/18                                 $
//   $Revision:: 1                                        $
//----------------------------------------------------------------------------+
//    $History:: fiGenerateNjunsTicket.cs                     $
// 
// *****************  Version 1  *****************
// User: hkonda     Date: 30/03/18   Time: 18:00  Desc : Created
//----------------------------------------------------------------------------+
using ADODB;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System;
using System.Collections.Generic;
using gtCommandLogger;

namespace GTechnology.Oncor.CustomAPI
{
    public class fiGenerateNjunsTicket : IGTFunctional
    {
        private GTArguments m_GTArguments = null;
        private string m_ComponentName = string.Empty;
        private IGTComponents m_Components = null;
        private IGTDataContext m_DataContext = null;
        private string m_FieldName = string.Empty;
        private IGTFieldValue m_fieldValueBeforeChange = null;
        private GTFunctionalTypeConstants m_Type;
        private int m_PoleFid;
        private short m_Fno;
        private string m_CurrentFeatureState;
        private string m_OriginalFeatureState;
        private string m_StructureId;
        customNjunsSharedLibrary m_OnjunsLibrary = new customNjunsSharedLibrary();
        private List<string> ValidFeatureStates = new List<string> { "PPR", "ABR", "OSR" };

        public GTArguments Arguments { get => m_GTArguments; set => m_GTArguments = value; }
        public string ComponentName { get => m_ComponentName; set => m_ComponentName = value; }
        public IGTComponents Components { get => m_Components; set => m_Components = value; }
        public IGTDataContext DataContext { get => m_DataContext; set => m_DataContext = value; }
        public string FieldName { get => m_FieldName; set => m_FieldName = value; }
        public IGTFieldValue FieldValueBeforeChange { get => m_fieldValueBeforeChange; set => m_fieldValueBeforeChange = value; }
        public GTFunctionalTypeConstants Type { get => m_Type; set => m_Type = value; }

        public void Execute()
        {
            try
            {
                if (Components[ComponentName].CNO == 34 || Components[ComponentName].CNO == 35)
                    return;
                if (CheckPreConditionsForTicketCreation())
                {
                    m_OnjunsLibrary.CreateTicket(m_DataContext.ActiveJob, m_PoleFid);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            ErrorPriorityArray = new string[2];
            ErrorMessageArray = new string[2];
            Dictionary<string, string> requireAttributesList;
            ValidationRuleManager validateMsg = new ValidationRuleManager();
            List<string> errorMsg = new List<string>();
            List<string> errorPriority = new List<string>();
            bool ticketsExists = false;

            GTValidationLogger gTValidationLogger = null;
            IGTComponent comp = Components[ComponentName];
            int FID = 0;

            string fieldValue = string.Empty;

            if (comp != null && comp.Recordset != null && comp.Recordset.RecordCount > 0)
            {
                FID = int.Parse(comp.Recordset.Fields["G3E_FID"].Value.ToString());
                fieldValue = Convert.ToString(comp.Recordset.Fields[FieldName].Value);
            }

            if (new gtLogHelper().CheckIfLoggingIsEnabled())
            {
                LogEntries logEntries = new LogEntries
                {
                    ActiveComponentName = ComponentName,
                    ActiveFID = FID,
                    ActiveFieldName = FieldName,
                    ActiveFieldValue = fieldValue,
                    JobID = DataContext.ActiveJob,
                    RelatedComponentName = "N/A",
                    RelatedFID = 0,
                    RelatedFieldName = "N/A",
                    RelatedFieldValue = "N/A",
                    ValidationInterfaceName = "Generate NJUNS Ticket",
                    ValidationInterfaceType = "FI",
                };
                gTValidationLogger = new GTValidationLogger(logEntries);

                gTValidationLogger.LogEntry("TIMING", "START", "Generate NJUNS Ticket Entry", "N/A", "");
            }

            try
            {

                GetPoleDetails();
                // Foreach ticket record on the pole, verify that all information required for submission to NJUNS has been provided.

                Recordset tempRs = GetRecordSet(string.Format("SELECT GIS_NJUNS_TICKET_ID , TICKET_NUMBER FROM NJUNS_TICKET WHERE POLE_NUMBER = '{0}' AND POLE_FID = {1} ", m_StructureId, m_PoleFid));
                if (tempRs.RecordCount == 0)
                {
                    if (gTValidationLogger != null)
                    gTValidationLogger.LogEntry("TIMING", "END", "Generate NJUNS Ticket Exit", "N/A", "");
                    return; // No tickets found for this pole.
                }

                if (tempRs.RecordCount > 0)
                    ticketsExists = true;
                tempRs.MoveFirst();

                while (!tempRs.EOF)
                {
                    m_OnjunsLibrary.GetTicketAttributes(Convert.ToInt32(tempRs.Fields["GIS_NJUNS_TICKET_ID"].Value));

                    if (Convert.ToInt32(tempRs.Fields["TICKET_NUMBER"].Value) < 1)
                    {
                        validateMsg.Rule_Id = "NJUNS02";
                        validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), null);
                        errorMsg.Add(validateMsg.Rule_MSG + " [ " + Convert.ToInt32(tempRs.Fields["GIS_NJUNS_TICKET_ID"].Value) + " ]");
                        errorPriority.Add("P1");
                    }

                    requireAttributesList = new Dictionary<string, string>();
                    requireAttributesList.Add("NJUNS_MEMBER", m_OnjunsLibrary.NJUNS_MEMBER_CODE);
                    requireAttributesList.Add("STATE", m_OnjunsLibrary.STATE);
                    requireAttributesList.Add("COUNTY", m_OnjunsLibrary.COUNTY);
                    requireAttributesList.Add("PRIORITY_CODE", m_OnjunsLibrary.PRIORITY_CODE);
                    requireAttributesList.Add("WORK_REQUESTED_DATE", m_OnjunsLibrary.WORK_REQUESTED_DATE.ToString("dd-MMM-yy"));

                    validateMsg.Rule_Id = "NJUNS01";
                    validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), null);

                    foreach (KeyValuePair<string, string> attribute in requireAttributesList)
                    {
                        if (string.IsNullOrEmpty(attribute.Value))
                        {
                            errorMsg.Add(validateMsg.Rule_MSG + ". The " + attribute.Key + " of NJUNS_TICKET attributes is required");
                            errorPriority.Add("P1");
                        }
                    }
                    tempRs.MoveNext();
                }

                if (ticketsExists)
                {
                    if (!CheckForJUAttachments(m_PoleFid))
                    {
                        validateMsg.Rule_Id = "NJUNS03";
                        validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), null);
                        errorMsg.Add(validateMsg.Rule_MSG + " [ " + m_OnjunsLibrary.GIS_NJUNS_TICKET_ID + " ]");
                        errorPriority.Add("P1");
                    }
                }

                if (errorMsg.Count > 0)
                {
                    ErrorPriorityArray = errorPriority.ToArray();
                    ErrorMessageArray = errorMsg.ToArray();
                }

                if (gTValidationLogger != null)
                    gTValidationLogger.LogEntry("TIMING", "END", "Generate NJUNS Ticket Exit", "N/A", "");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool CheckPreConditionsForTicketCreation()
        {
            try
            {
                GetPoleDetails();
                if (m_Fno != 110)
                {
                    return false;
                }
                if (!ValidFeatureStates.Contains(m_CurrentFeatureState))
                {
                    return false;
                }

                if (!CheckValidFeatureStateTransition(m_OriginalFeatureState, m_CurrentFeatureState))
                {
                    return false;
                }

                if (!CheckForJUAttachments(m_PoleFid))
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
        /// Method to the details of the selected pole 
        /// </summary>
        private void GetPoleDetails()
        {
            try
            {
                Recordset tempRs = Components[ComponentName].Recordset;
                m_Fno = Convert.ToInt16(tempRs.Fields["G3E_FNO"].Value);
                m_PoleFid = Convert.ToInt32(tempRs.Fields["G3E_FID"].Value);
                m_CurrentFeatureState = Convert.ToString(tempRs.Fields["FEATURE_STATE_C"].Value);
                m_StructureId = Convert.ToString(tempRs.Fields["STRUCTURE_ID"].Value);
                try
                {
                    m_OriginalFeatureState = Convert.ToString(tempRs.Fields["FEATURE_STATE_C"].OriginalValue);
                }
                catch
                { }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool CheckValidFeatureStateTransition(string originalValue, string newValue)
        {
            try
            {
                // The valid feature state to create ticket is when feature state is changed to Either PPR or ABR only.

                if (string.IsNullOrEmpty(originalValue) && (newValue.Equals("PPR") || newValue.Equals("ABR")))
                {
                    return true;
                }
                //if ((originalValue.Equals("PPR") || originalValue.Equals("ABR")) && newValue.Equals("OSR"))
                //{
                //    return false;
                //}
                //return true;
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Check for Joint Use attachments on pole
        /// </summary>
        /// <param name="fid"> pole g3e_fid</param>
        /// <returns>True, if there are attachments on the pole. Else returns false</returns>
        private bool CheckForJUAttachments(int fid)
        {
            IGTKeyObject gTKeyObject = null;
            try
            {
                gTKeyObject = m_DataContext.OpenFeature(110, fid);

                Recordset wireLineAttachmentRs = gTKeyObject.Components.GetComponent(34).Recordset;
                Recordset equipmentAttachmentRs = gTKeyObject.Components.GetComponent(35).Recordset;

                if (wireLineAttachmentRs == null || equipmentAttachmentRs == null || ((wireLineAttachmentRs != null && wireLineAttachmentRs.RecordCount == 0) && (equipmentAttachmentRs != null && equipmentAttachmentRs.RecordCount == 0)))
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
        /// Method to execute sql query and return the result record set
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        private Recordset GetRecordSet(string sqlString)
        {
            try
            {
                int outRecords = 0;
                Command command = new Command();
                command.CommandText = sqlString;
                Recordset results = m_DataContext.ExecuteCommand(command, out outRecords);
                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete()
        {
        }
    }
}
