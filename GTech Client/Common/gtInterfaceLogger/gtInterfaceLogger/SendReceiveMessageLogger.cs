using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using ADODB;
using Intergraph.GTechnology.API;

namespace gtInterfaceLogger
{
    public class SendReceiveMessageLogger
    {
        public IGTDataContext dataContext = null;
        public string xmlMessage = string.Empty;
        public bool Interactive = true;

        public string Interface_Name = string.Empty; // required
        public string Sub_Interface_name = string.Empty;
        public string Component_Name = string.Empty; // required
        public string Result_Code = string.Empty;
        public string Result_Status = string.Empty;
        public string Log_Detail = string.Empty;
        public string Correlation_Id = string.Empty; // required
        public string Process_Run_Id = string.Empty;
        public string Data_Table = string.Empty;
        public long Data_Row_Id = -1;

        /// <summary>
        /// Log an entry into the GIS_STG.Interface_log table
        /// </summary>
        /// <returns>Returns 'true' if successfull and 'false' if it fails.</returns>
        public bool logEntry()
        {
            bool tmpReturn = true;
            //Recordset tmpRS = null;
            string tmpQry = string.Empty;
            int tmpRecUpdated = 0;
            long tmpLogId = 0;
            string tmpColumns = string.Empty;
            string tmpColValues = string.Empty;
            //int tmpRecChanged = 0;

            try
            {
                // Set datacontext if it is not set.
                if (dataContext == null)
                {
                    GetDataContext();
                }

                // Get the log id 
                tmpLogId = GetNextIdVal();


                tmpColumns = GetLogEntryColumns();
                if (tmpColumns == string.Empty )
                {
                    return false;
                }

                tmpColValues = GetLogEntryValues(tmpLogId);
                if (tmpColValues == string.Empty)
                {
                    return false;
                }

                tmpQry = "begin INSERT INTO gis_stg.interface_log " + tmpColumns
                                  + " VALUES " + tmpColValues + "; commit; end;";
                               
                dataContext.Execute(tmpQry,out tmpRecUpdated, (int)CommandTypeEnum.adCmdText);
             
                if (xmlMessage != string.Empty)
                {
                    AddXMLDataLog(tmpLogId);
                }
            }
            catch(Exception e)
            {
                tmpReturn = false;
                if (Interactive)
                {
                    MessageBox.Show("SendReceiveMessageLogger.logEntry: + e.Message", "Error: SendReceiveMessageLogger.logEntry", MessageBoxButtons.OK);
                }
                else
                {
                    if (EventLog.SourceExists("Application Error"))
                    {
                        EventLog.WriteEntry("Application Error","Error in G/Technology Custom Logger - SendReceiveMessageLogger.logEntry: " + e.Message);
                    }
                }
            }
            return tmpReturn;
        }

        /// <summary>
        /// Retieves the nextval from the GIS_STG.INTERFACE_LOG_SEQ sequence
        ///   to be used as the entryId
        /// </summary>
        /// <returns>Returns 0 if the method fails or
        /// a number to be uses as the entryId</returns>
        private long GetNextIdVal()
        {
            long tmpReturn = 0;
            string tmpQry = string.Empty;
            Recordset tmpRs = null;
            try
            {
                tmpQry = "select GIS_STG.INTERFACE_LOG_SEQ.nextval " + (char)34 + "NEXTVAL" + (char)34 + " from dual";
                tmpRs = dataContext.OpenRecordset(tmpQry, CursorTypeEnum.adOpenStatic,
                                                        LockTypeEnum.adLockReadOnly,
                                                        (int)CommandTypeEnum.adCmdText);
                tmpRs.MoveFirst();
                tmpReturn = Convert.ToInt64(tmpRs.Fields[0].Value);
            }
            catch (Exception e)
            { 
                tmpReturn = 0;
                if (Interactive)
                {
                    MessageBox.Show("Error: " + e.Message, "Error: SendReceiveMessageLogger.GetNextIdVal", MessageBoxButtons.OK);
                }
                else
                {
                    if (EventLog.SourceExists("Application Error"))
                    {
                        EventLog.WriteEntry("Application Error", "Error in G/Technology Custom Logger - SendReceiveMessageLogger.GetNextIdVal: " + e.Message);
                    }
                }
            }
            return tmpReturn;
        }


        /// <summary>
        /// Create a string for the columns list for the insert statement
        /// for inserting into the gis_stg.interface_log.
        /// </summary>
        /// <returns>Returns the generated string if successful,
        ///  or a string.Empty if it fails.</returns>
        private string GetLogEntryColumns()
        {
            string returnColumns = string.Empty;
            string tmpStr = string.Empty;
            string tmpErr = string.Empty;

            try
            {
                tmpStr = "(INTERFACE_LOG_ID,";
                if (Interface_Name == string.Empty)
                {
                    tmpErr = "Interface Name was not set. "; //raise error
                }
                else
                {
                    tmpStr = tmpStr + "INTERFACE_NAME,";

                }
                if (Component_Name == string.Empty)
                {
                     tmpErr =  tmpErr + "Component Name was not set. ";//raise error
                }
                else
                {
                    tmpStr = tmpStr + "COMPONENT_NAME,";

                }
                if (Correlation_Id == string.Empty)
                {
                    tmpErr = tmpErr + "Correlation Id was not set.";
                }
                else
                {
                    tmpStr = tmpStr + "CORRELATION_ID,";
                }
                if (Sub_Interface_name != string.Empty)tmpStr = tmpStr + "SUB_INTERFACE_NAME,";
                if (Result_Code != string.Empty) tmpStr = tmpStr + "RESULT_CODE,";
                if (Result_Status != string.Empty) tmpStr = tmpStr + "RESULT_STATUS,";
                if (Log_Detail != string.Empty) tmpStr = tmpStr + "LOG_DETAIL,";
                if (Process_Run_Id != string.Empty) tmpStr = tmpStr + "PROCESS_RUN_ID,";
                if (Data_Table != string.Empty) tmpStr = tmpStr + "DATA_TABLE,";
                if (Data_Row_Id != -1) tmpStr = tmpStr + "DATA_ROW_ID,";
                tmpStr = tmpStr + "AUD_CREATE_USR_ID)";
               // tmpStr = tmpStr + "AUD_CREATE_TS)";

                returnColumns = tmpStr;

                if (tmpErr != string.Empty)
                {
                    tmpErr = tmpErr + "\n" + "Columns with values. " + returnColumns;
                  throw new Exception(tmpErr);
                }
            }
            catch (Exception e)
            {
                returnColumns = string.Empty;
                if (Interactive)
                {
                    MessageBox.Show("Error: " + e.Message, "Error: SendReceiveMessageLogger.GetLogEntryColumns", MessageBoxButtons.OK);
                }
                {
                    if (EventLog.SourceExists("Application Error"))
                    {
                        EventLog.WriteEntry("Application Error", "Error in G/Technology Custom Logger - SendReceiveMessageLogger.GetLogEntryColumns: " + e.Message);
                    }
                }
            }
            return returnColumns;
        }

        /// <summary>
        /// Add a record to the GIS_STG.INTERFACE_XML_DATA table. - stores XML Messages.
        /// </summary>
        /// <param name="logId">The Reference value to the row in the gis_stg.interface_log row.</param>
        /// <returns>Returns 'true' if successfull and 'false' if it fails.</returns>
        private bool AddXMLDataLog(long logId)
        {
            bool tmpReturn = true;
            string tmpQry = string.Empty;
            int tmpRecChanged = 0;
            try
            {
                tmpQry = "begin insert into GIS_STG.INTERFACE_XML_DATA (INTERFACE_LOG_ID, XML_DATA) VALUES (?,?); commit; end;";
                dataContext.Execute(tmpQry, out tmpRecChanged, (int)CommandTypeEnum.adCmdText, logId,xmlMessage);
                //dataContext.Execute("commit", out tmpRecChanged, (int)CommandTypeEnum.adCmdText);
            }
            catch (Exception e)
            {
                if (Interactive)
                {
                    MessageBox.Show("Error: " + e.Message, "Error: SendReceiveMessageLogger.AddXMLDataLog", MessageBoxButtons.OK);
                }
                {
                    if (EventLog.SourceExists("Application Error"))
                    {
                        EventLog.WriteEntry("Application Error", "Error in G/Technology Custom Logger - SendReceiveMessageLogger.AddXMLDataLog: " + e.Message);
                    }
                }
            }
            return tmpReturn;
        }

        /// <summary>
        /// Creates the values string for the insert statement 
        /// for inserting into the gis_stg.interface_log table.
        /// </summary>
        /// <param name="EntryId"> The unque value for the row in the gis_stg.interface_log row.</param>
        /// <returns>Returns the generated string if successful,
        ///  or a string.Empty if it fails.</returns>
        private string GetLogEntryValues(long EntryId)
        {
            string returnValues = string.Empty;
            string tmpStr = string.Empty;
            string tmpException = string.Empty;
            try
            {
                tmpStr = "(" + EntryId.ToString() + ",";
                if (Interface_Name == string.Empty)
                {
                    tmpException = "Interface Name not populated.\n";
                }
                else
                {
                    tmpStr = tmpStr + "'" + Interface_Name + "',";
                }

                if (Component_Name == string.Empty)
                {
                    tmpException = tmpException + "Component Name not populated.\n";
                }
                else
                {
                    tmpStr = tmpStr + "'" + Component_Name + "',";

                }

                if (Correlation_Id == string.Empty)
                {
                    tmpException = tmpException + "Correlation_Id not populated. /n";
                }
                else
                {
                    tmpStr = tmpStr + "'" + Correlation_Id + "',";
                }

                if (tmpException != string.Empty)
                {
                    tmpException = tmpException + "Values Set: " + tmpStr;
                    throw new Exception(tmpException);
                }
                if (Sub_Interface_name != string.Empty) tmpStr = tmpStr + "'" + Sub_Interface_name + "',";
                if (Result_Code != string.Empty) tmpStr = tmpStr + "'" + Result_Code + "',";
                if (Result_Status != string.Empty) tmpStr = tmpStr + "'" + Result_Status + "',";
                if (Log_Detail != string.Empty) tmpStr = tmpStr + "'" + Log_Detail + "',";
                if (Process_Run_Id != string.Empty) tmpStr = tmpStr + "'" + Process_Run_Id + "',";
                if (Data_Table != string.Empty) tmpStr = tmpStr + "'" + Data_Table + "',";
                if (Data_Row_Id != -1) tmpStr = tmpStr + Data_Row_Id + ",";
                //tmpStr = tmpStr + "AUD_CREATE_USR_ID)";
                tmpStr = tmpStr + "'"+ System.Environment.UserDomainName + "')";
                //tmpStr = tmpStr + "AUD_CREATE_TS)";
                returnValues = tmpStr;
            }
            catch (Exception e)
            {
                returnValues = string.Empty;
                if (Interactive)
                {
                    MessageBox.Show("Error: " + e.Message, "Error: SendReceiveMessageLogger.GetLogEntryValues", MessageBoxButtons.OK);
                }
           
                {
                    if (EventLog.SourceExists("Application Error"))
                    {
                        EventLog.WriteEntry("Application Error", "Error in G/Technology Custom Logger - SendReceiveMessageLogger.GetLogEntryValues: " + e.Message);
                    }
                }
            }
            return returnValues;
        }

        /// <summary>
        /// Sets the GTechnology DataContext for the class.
        /// </summary>
        private void GetDataContext()
        {
            IGTApplication tmpApp = GTClassFactory.Create<IGTApplication>();
            try
            {
                dataContext = tmpApp.DataContext;
            }
            catch (Exception e)
            {
                if (Interactive)
                {
                    MessageBox.Show("Error: " + e.Message, "Error: SendReceiveMessageLogger.GetDataContext", MessageBoxButtons.OK);
                }
                {
                    if (EventLog.SourceExists("Application Error"))
                    {
                        EventLog.WriteEntry("Application Error", "Error in G/Technology Custom Logger - SendReceiveMessageLogger.GetDataContext: " + e.Message);
                    }
                }
            }
        }

    }
}
