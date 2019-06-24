//----------------------------------------------------------------------------+
//        Class: fiCheckDeviceRating
//  Description: This interface Raises validation error when the Summer Amps or Winter Amps attributes for any phase exceed that feature's Device Rating.
//----------------------------------------------------------------------------+
//     $Author:: hkonda                                   $
//       $Date:: 03/010/17                                $
//   $Revision:: 1                                        $
//----------------------------------------------------------------------------+
//    $History:: fiCheckDeviceRating.cs                   $
// 
// *****************  Version 1  *****************
// User: hkonda     Date: 03/10/17    Time: 18:00  Desc : Created
//----------------------------------------------------------------------------+
using System;
using ADODB;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Linq;
using System.Collections.Generic;
using gtCommandLogger;

namespace GTechnology.Oncor.CustomAPI
{
    public class fiCheckDeviceRating : IGTFunctional
    {
        private GTArguments m_Arguments = null;
        private IGTDataContext m_DataContext = null;
        private IGTComponents m_components;
        private string m_ComponentName;
        private string m_FieldName;

        public GTArguments Arguments
        {
            get { return m_Arguments; }
            set { m_Arguments = value; }
        }

        public string ComponentName
        {
            get { return m_ComponentName; }
            set { m_ComponentName = value; }
        }

        public IGTComponents Components
        {
            get { return m_components; }
            set { m_components = value; }
        }

        public IGTDataContext DataContext
        {
            get { return m_DataContext; }
            set { m_DataContext = value; }
        }

        public void Delete()
        {
        }

        public void Execute()
        {
        }

        public string FieldName
        {
            get { return m_FieldName; }
            set { m_FieldName = value; }
        }

        public IGTFieldValue FieldValueBeforeChange
        {
            get;
            set;
        }

        public GTFunctionalTypeConstants Type
        {
            get;
            set;
        }

        public void Validate(out string[] ErrorPriorityArray, out string[] ErrorMessageArray)
        {
            ErrorMessageArray = null;
            ErrorPriorityArray = null;
            short cNo = 0;
            int deviceRating = 0;
            List<int> ampsList = null;

            GTValidationLogger gTValidationLogger = null;
            IGTComponent comp = Components[ComponentName];
            int FID = 0;

            if (comp != null)
            {
                comp.Recordset.MoveFirst();
                FID = int.Parse(comp.Recordset.Fields["G3E_FID"].Value.ToString());
            }

            if (new gtLogHelper().CheckIfLoggingIsEnabled())
            {
                LogEntries logEntries = new LogEntries
                {
                    ActiveComponentName = ComponentName,
                    ActiveFID = FID,
                    ActiveFieldName = FieldName,
                    ActiveFieldValue = Convert.ToString(comp.Recordset.Fields[FieldName].Value),
                    JobID = DataContext.ActiveJob,
                    RelatedComponentName = "N/A",
                    RelatedFID = 0,
                    RelatedFieldName = "N/A",
                    RelatedFieldValue = "N/A",
                    ValidationInterfaceName = "Check Device Rating",
                    ValidationInterfaceType = "FI",
                };
                gTValidationLogger = new GTValidationLogger(logEntries);

                gTValidationLogger.LogEntry("TIMING", "START", "Check Device Rating Entry", "N/A", "");
            }

            try
            {
                List<String> strErrorMsg = new List<String>();
                List<String> strErrorPriority = new List<String>();
                IGTApplication iGtApplication = GTClassFactory.Create<IGTApplication>();
                iGtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Check Device Rating Validation Started");
                string errorPriority = Convert.ToString(m_Arguments.GetArgument(0));
                short fNo = Convert.ToInt16(Components[ComponentName].Recordset.Fields["G3E_FNO"].Value);

                ValidationRuleManager validateMsg = new ValidationRuleManager();

                IGTComponent loadAnalysisAttributes = Components.GetComponent(32); // Load Analysis Attributes Component

                if (loadAnalysisAttributes == null)
                {
                    iGtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Check Device Rating Validation Completed");
                    if (gTValidationLogger != null)
                    gTValidationLogger.LogEntry("TIMING", "END", "Check Device Rating Exit", "N/A", "");
                    return;
                }

                string query = @"SELECT A.G3E_CNO FROM G3E_ATTRIBUTEINFO_OPTABLE A, G3E_FEATURECOMPS_OPTABLE F WHERE 
								 A.G3E_FIELD =  'DEVICE_RTG_Q' AND
								 F.G3E_CNO = A.G3E_CNO AND
								 F.G3E_FNO = {0}";

                Recordset resultRs = GetRecordSet(string.Format(query, fNo));

                // Get the device rating
                if (resultRs != null && resultRs.RecordCount > 0)
                {
                    resultRs.MoveFirst();
                    cNo = Convert.ToInt16(resultRs.Fields["G3E_CNO"].Value); // CNO for the attribute containing Device rating
                    if (cNo > 0)
                    {
                        Recordset tempRs = Components.GetComponent(cNo).Recordset;
                        if (tempRs != null && tempRs.RecordCount > 0)
                        {
                            tempRs.MoveFirst();
                            if (!string.IsNullOrEmpty(Convert.ToString(tempRs.Fields["DEVICE_RTG_Q"].Value)))
                            {
                                deviceRating = Convert.ToInt32(tempRs.Fields["DEVICE_RTG_Q"].Value);
                            }
                        }
                    }
                }
                if (deviceRating <= 0)
                {
                    if (gTValidationLogger != null)
                    gTValidationLogger.LogEntry("TIMING", "END", "Check Device Rating Exit", "N/A", "");
                    return;
                }
                // Get the Summer Amps and Winter Amps
                Recordset loadAnalysisAttributesRs = loadAnalysisAttributes.Recordset;
                if (loadAnalysisAttributesRs != null && loadAnalysisAttributesRs.RecordCount > 0)
                {
                    ampsList = new List<int>();
                    loadAnalysisAttributesRs.MoveFirst();
                    if (!string.IsNullOrEmpty(Convert.ToString(loadAnalysisAttributesRs.Fields["SUMMER_AMPS_A_Q"].Value)))
                    {
                        ampsList.Add(Convert.ToInt32(loadAnalysisAttributesRs.Fields["SUMMER_AMPS_A_Q"].Value));
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(loadAnalysisAttributesRs.Fields["SUMMER_AMPS_B_Q"].Value)))
                    {
                        ampsList.Add(Convert.ToInt32(loadAnalysisAttributesRs.Fields["SUMMER_AMPS_B_Q"].Value));
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(loadAnalysisAttributesRs.Fields["SUMMER_AMPS_C_Q"].Value)))
                    {
                        ampsList.Add(Convert.ToInt32(loadAnalysisAttributesRs.Fields["SUMMER_AMPS_C_Q"].Value));
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(loadAnalysisAttributesRs.Fields["WINTER_AMPS_A_Q"].Value)))
                    {
                        ampsList.Add(Convert.ToInt32(loadAnalysisAttributesRs.Fields["WINTER_AMPS_A_Q"].Value));
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(loadAnalysisAttributesRs.Fields["WINTER_AMPS_B_Q"].Value)))
                    {
                        ampsList.Add(Convert.ToInt32(loadAnalysisAttributesRs.Fields["WINTER_AMPS_B_Q"].Value));
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(loadAnalysisAttributesRs.Fields["WINTER_AMPS_C_Q"].Value)))
                    {
                        ampsList.Add(Convert.ToInt32(loadAnalysisAttributesRs.Fields["WINTER_AMPS_C_Q"].Value));
                    }

                    if (ampsList != null && ampsList.Count > 0 && ampsList.Max() > deviceRating)
                    {
                        validateMsg.Rule_Id = "DRTG01";
                        validateMsg.BuildRuleMessage(GTClassFactory.Create<IGTApplication>(), null);

                        strErrorMsg.Add(validateMsg.Rule_MSG);
                        strErrorPriority.Add(errorPriority);
                    }
                }

                iGtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Check Device Rating Validation Completed");
                ErrorMessageArray = strErrorMsg.ToArray();
                ErrorPriorityArray = strErrorPriority.ToArray();

                if (gTValidationLogger != null)
                    gTValidationLogger.LogEntry("TIMING", "END", "Check Device Rating Exit", "N/A", "");
            }
            catch (Exception ex)
            {
                throw new Exception("Error during Check Device Rating FI validation. " + ex.Message);
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
                ADODB.Command command = new ADODB.Command();
                command.CommandText = sqlString;
                ADODB.Recordset results = DataContext.ExecuteCommand(command, out outRecords);
                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
