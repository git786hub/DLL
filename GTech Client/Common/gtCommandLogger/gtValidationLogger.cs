using ADODB;
using Intergraph.GTechnology.API;
using System;
using System.Diagnostics;

namespace gtCommandLogger
{
    public class GTValidationLogger
    {
        private LogEntries m_LogEntry;
        private bool isInteractive = true;

        public GTValidationLogger(LogEntries p_LogEnrty )
        {
            m_LogEntry = p_LogEnrty;
            GTechnology.Oncor.CustomAPI.GUIMode guiMode = new GTechnology.Oncor.CustomAPI.GUIMode();
            this.isInteractive = guiMode.InteractiveMode;
        }

        /// <summary>
        /// Method to create validation log entry in table VALIDATION_LOG
        /// </summary>
        /// <param name="p_LogType"></param>
        /// <param name="p_LogCode"></param>
        /// <param name="p_LogMsg"></param>
        /// <param name="p_LogContext"></param>
        /// <param name="p_ErrorPriority"></param>
        /// <returns></returns>
        public bool LogEntry(string p_LogType,string p_LogCode, string p_LogMsg , string p_LogContext, string p_ErrorPriority)
        {
            IGTApplication tmpApp = GTClassFactory.Create<IGTApplication>();
            IGTDataContext tmpDatCont = tmpApp.DataContext;          

            // Build the data insert script.
            string tmpInsertStr =
                @"begin insert into VALIDATION_LOG columns(VALIDATION_IFACE_TYPE,VALIDATION_NAME,JOB_ID,ACTIVE_FID,
                  ACTIVE_COMP_NAME,ACTIVE_FIELD_NAME, ACTIVE_FIELD_VALUE,RELATED_FID, RELATED_COMP_NAME, RELATED_FIELD_NAME,
                    RELATED_FIELD_VALUE,LOG_CODE,LOG_MSG,LOG_CONTEXT,ERROR_PRIORITY,LOG_TYPE) values";

            bool tmpReturn = true;
            // get the values list.
            string tmpValues = BuildValuesList(p_LogType, p_LogCode, p_LogMsg, p_LogContext,p_ErrorPriority, tmpDatCont);

            if (string.IsNullOrEmpty(tmpValues))
            {
                //The buildValuesList function generated an error and a empty string was returned. 
                tmpReturn = false;
            }
            else
            {
                try
                {
                    // The buildValuesList function succeeded.
                    // Complete the insert script.
                    tmpInsertStr = string.Format("{0} {1};commit;end;", tmpInsertStr, tmpValues);
                    // Execute the insert script
                    tmpDatCont.Execute(tmpInsertStr, out int tmpRecUpdated, (int)CommandTypeEnum.adCmdText);
                }
                catch (Exception ex)
                {
                    tmpReturn = false;

                    // If the error is due to privileges (resulting in a "table or view does not exist"), then only write
                    // the exception message to the event log.  Otherwise, write the error to the command log.
                    if (ex.Message.Contains("ORA-00942"))
                    {
                        // Log an error to the Applcation Error Event Log.
                        if (EventLog.SourceExists("Application Error"))
                        {
                            EventLog.WriteEntry("Application Error", "Error in G/Technology Custom Command Logger - gtCommandLogger.logEntry: " + ex.Message);
                        }
                    }
                    else
                    {
                        // This may risk infinite recursion, but since the error is not due to table access,
                        // then merely logging the error message should not produce an exception and when this call returns,
                        // then this "catch" should fall through and exit.
                        this.LogEntry(null,null, ex.Message,null,null);
                    }
                }
            }

            return tmpReturn;
        }

        private string BuildValuesList(string p_LogType, string p_LogCode, string p_LogMsg, string p_LogContext, string p_ErrorPriority,IGTDataContext gTDataContext)
        {
            string tmpValuesStr = string.Empty;
            string tmpException = string.Empty;
            string tmpStr = string.Empty;

            try
            {
                tmpValuesStr = "(";
                // Build the 'values' portion of the insert string, and check for valid or existing values.
               
                tmpValuesStr = m_LogEntry.ValidationInterfaceType == string.Empty ? tmpValuesStr + "NULL," : tmpValuesStr + "'" + m_LogEntry.ValidationInterfaceType + "',";
               
                tmpValuesStr = m_LogEntry.ValidationInterfaceName == string.Empty ? tmpValuesStr + "NULL," : tmpValuesStr + "'" + m_LogEntry.ValidationInterfaceName + "',";

                tmpValuesStr = m_LogEntry.JobID == string.Empty ? tmpValuesStr + "NULL," : tmpValuesStr + "'" + m_LogEntry.JobID + "',";

                tmpValuesStr = Convert.ToString(m_LogEntry.ActiveFID) == string.Empty ? tmpValuesStr + "NULL," : tmpValuesStr + "'" + m_LogEntry.ActiveFID + "',";

                tmpValuesStr = m_LogEntry.ActiveComponentName == string.Empty ? tmpValuesStr + "NULL," : tmpValuesStr + "'" + m_LogEntry.ActiveComponentName + "',";

                tmpValuesStr = m_LogEntry.ActiveFieldName == string.Empty ? tmpValuesStr + "NULL," : tmpValuesStr + "'" + m_LogEntry.ActiveFieldName + "',";

                tmpValuesStr = m_LogEntry.ActiveFieldValue == string.Empty ? tmpValuesStr + "NULL," : tmpValuesStr + "'" + m_LogEntry.ActiveFieldValue + "',";

                tmpValuesStr = Convert.ToString(m_LogEntry.RelatedFID) == string.Empty ? tmpValuesStr + "NULL," : tmpValuesStr + "'" + m_LogEntry.RelatedFID + "',";

                tmpValuesStr = m_LogEntry.RelatedComponentName == string.Empty ? tmpValuesStr + "NULL," : tmpValuesStr + "'" + m_LogEntry.RelatedComponentName + "',";

                tmpValuesStr = m_LogEntry.RelatedFieldName == string.Empty ? tmpValuesStr + "NULL," : tmpValuesStr + "'" + m_LogEntry.RelatedFieldName + "',";

                tmpValuesStr = m_LogEntry.RelatedFieldValue == string.Empty ? tmpValuesStr + "NULL," : tmpValuesStr + "'" + m_LogEntry.RelatedFieldValue + "',";

                tmpValuesStr = p_LogCode == string.Empty ? tmpValuesStr + "NULL," : tmpValuesStr + "'" + p_LogCode + "',";

                tmpValuesStr = p_LogMsg == string.Empty ? tmpValuesStr + "LogMsg is not set.\n ," : tmpValuesStr + "'" + p_LogMsg + "',";

                tmpValuesStr = p_LogContext == string.Empty ? tmpValuesStr + "NULL," : tmpValuesStr + "'" + p_LogContext + "',";

                tmpValuesStr = p_ErrorPriority == string.Empty ? tmpValuesStr + "''," : tmpValuesStr + "'" + p_ErrorPriority + "',";

                switch (p_LogType)
                {
                    case "ERROR":
                    case "INFO":
                    case "WARNING":
                    case "TIMING":
                        tmpValuesStr = tmpValuesStr + "'" + p_LogType + "'";
                        break;
                    default:
                        tmpStr = tmpStr + "LogType is not set to 'ERROR', 'INFO','TIMING', or 'WARNING.\n";
                        break;
                }

                // Throw an error if any of the values are bad.
                if (tmpStr != string.Empty)
                {
                    tmpException = tmpException + "Values Set: " + tmpStr + "\n A log entry was not added to the VALIDATION_LOG table.";
                    throw new Exception(tmpException);
                }
                else
                {
                    tmpValuesStr = tmpValuesStr + ")";
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            // return the insert values string.
            return tmpValuesStr;
        }

        /// <summary>
        /// buildValuesList builds the 'values' portion of an Oracle insert statement that will 
        ///   insert rows into the Command_log table.
        /// The function also insures that all the values are valid.
        /// </summary>
        /// <returns>
        /// The function returns a string that represents the values portion of the Oracle 
        ///   insert statement in the insert script.</returns>
    }

    public class LogEntries
    {
        public string ValidationInterfaceType { get; set; }
        public string ValidationInterfaceName { get; set; }
        public int ActiveFID { get; set; }
        public string ActiveComponentName { get; set; }
        public string ActiveFieldName { get; set; }
        public string ActiveFieldValue { get; set; }
        public int RelatedFID { get; set; }
        public string RelatedComponentName { get; set; }
        public string RelatedFieldName { get; set; }
        public string RelatedFieldValue { get; set; }
        public string JobID { get; set; }
    }
}
