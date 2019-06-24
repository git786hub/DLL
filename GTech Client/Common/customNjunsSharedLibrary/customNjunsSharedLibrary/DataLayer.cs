//----------------------------------------------------------------------------+
//        Class: DataLayer
//  Description: This class used to perform DB operations for processing ticket and steps
//
//----------------------------------------------------------------------------+
//     $Author:: hkonda                                   $
//       $Date:: 20/03/18                                  $
//   $Revision:: 1                                        $
//----------------------------------------------------------------------------+
//    $History:: DataLayer.cs                     $
// 
// *****************  Version 1  *****************
// User: hkonda     Date: 20/03/18   Time: 18:00  Desc : Created
// User: hkonda     Date: 30/09/18   Time: 18:00  Desc : Code changes made to support latest schema changes
//----------------------------------------------------------------------------+
using ADODB;
using gtInterfaceLogger;
using Intergraph.GTechnology.API;
using OncorTicketCreation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public class DataLayer
    {
        public int GIS_NJUNS_TICKET_ID { get; set; }
        public string NJUNS_TICKET_ID { get; set; }
        public int TICKET_NUMBER { get; set; }
        public string TICKET_TYPE { get; set; }
        public string TICKET_STATUS { get; set; }
        public int POLE_FID { get; set; }
        public string POLE_NUMBER { get; set; }
        public string MISCELLANEOUS_ID { get; set; }
        public string NJUNS_MEMBER_CODE { get; set; }
        public string POLE_OWNER { get; set; }
        public DateTime START_DATE { get; set; }
        public DateTime WORK_REQUESTED_DATE { get; set; }
        public string CONTACT_NAME { get; set; }
        public string CONTACT_PHONE { get; set; }
        public string STATE { get; set; }
        public string COUNTY { get; set; }
        public string PLACE { get; set; }
        public decimal LATITUDE { get; set; }
        public decimal LONGITUDE { get; set; }
        public string HOUSE_NUMBER { get; set; }
        public string STREET_NAME { get; set; }
        public string PRIORITY_CODE { get; set; }
        public string JOB_TYPE { get; set; }
        public string NUMBER_OF_POLES { get; set; }
        public string DAYS_INTERVAL { get; set; }
        public string REMARKS { get; set; }
        public string PLOT { get; set; }
        public string INVOICE { get; set; }
        public int NJUNS_STEP_ID { get; set; }
        public string NJUNS_MEMBER { get; set; }
        public string NUMBER_POLES { get; set; }

        public List<TicketStepType> TicketStepTypeList { get; set; }
        public char Mode { get; set; }
        public IGTDataContext DataContext { get; set; }


        const string m_InterfacePoint_ST = "Submit Ticket ";
        const string m_InterfacePoint_ST_WR = "Submit Ticket By WR ";
        const string m_InterfacePoint_TS = "Ticket Status ";
        const string m_InterfacePoint_TS_WR = "Ticket Status By WR ";

        string m_interfacePoint = string.Empty;

        #region Njuns Ticket DB Operation Methods

        /// <summary>
        ///  Method inserts new record into NJUNS Ticket table
        /// </summary>
        /// <returns>True, if insertion is successful</returns>
        public bool InsertNewTicketRecord()
        {
            int iRecordsAffected = 0;
            try
            {
                GIS_NJUNS_TICKET_ID = GetNextSequenceValue("GIS_ONC.GIS_NJUNS_TICKET_ID_SEQ");
                StringBuilder insertQuery = new StringBuilder();
                insertQuery.AppendFormat(" BEGIN ");
                insertQuery.AppendFormat(@"INSERT INTO GIS_ONC.NJUNS_TICKET(
                                                     NJUNS_TICKET_ID,
                                                     TICKET_NUMBER,
                                                     TICKET_TYPE,
                                                     TICKET_STATUS,
                                                     POLE_FID,
                                                     POLE_NUMBER,
                                                     MISCELLANEOUS_ID,
                                                     NJUNS_MEMBER_CODE,
                                                     POLE_OWNER,
                                                     CONTACT_NAME,
                                                     CONTACT_PHONE,
                                                     STATE,
                                                     COUNTY,
                                                     PLACE,
                                                     LATITUDE,
                                                     LONGITUDE,
                                                     HOUSE_NUMBER,
                                                     STREET_NAME,
                                                     PRIORITY_CODE,
                                                     JOB_TYPE,
                                                     REMARKS,
                                                     NUMBER_OF_POLES,
                                                     DAYS_INTERVAL,
                                                     WORK_REQUESTED_DATE,
                                                     GIS_NJUNS_TICKET_ID
                                                     )
                                                     VALUES('{0}',{1},'{2}','{3}',{4},'{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}',{24});",
                                                     NJUNS_TICKET_ID,
                                                     TICKET_NUMBER,
                                                     TICKET_TYPE,
                                                     TICKET_STATUS,
                                                     POLE_FID,
                                                     POLE_NUMBER,
                                                     MISCELLANEOUS_ID,
                                                     NJUNS_MEMBER_CODE,
                                                     POLE_OWNER,
                                                     CONTACT_NAME,
                                                     CONTACT_PHONE,
                                                     STATE,
                                                     COUNTY,
                                                     PLACE,
                                                     LATITUDE,
                                                     LONGITUDE,
                                                     HOUSE_NUMBER,
                                                     STREET_NAME,
                                                     PRIORITY_CODE,
                                                     JOB_TYPE,
                                                     REMARKS,
                                                     NUMBER_OF_POLES,
                                                     DAYS_INTERVAL,
                                                     WORK_REQUESTED_DATE.ToString("dd-MMM-yy"),
                                                     GIS_NJUNS_TICKET_ID
                                                     );
                insertQuery.AppendFormat("  END ; ");
                string insertSql = insertQuery.ToString();
                DataContext.Execute(insertSql, out iRecordsAffected, (int)CommandTypeEnum.adCmdText);
                return true;
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to insert ticket record", "Custom Create Ticket Error", MessageBoxButtons.OK);
                return false;
            }
        }


        /// <summary>
        ///  Method inserts new record into NJUNS Step table
        /// </summary>
        /// <returns>True, if insertion is successful</returns>/// <returns></returns>
        public bool InsertStepRecords()
        {
            int iRecordsAffected = 0;
            try
            {
                StringBuilder insertQuery = new StringBuilder();
                insertQuery.AppendFormat(" BEGIN ");
                insertQuery.AppendFormat(@"INSERT INTO GIS_ONC.NJUNS_STEP(
                                                     GIS_NJUNS_TICKET_ID,
                                                     NJUNS_TICKET_ID,
                                                     JOB_TYPE,
                                                     NUMBER_POLES,
                                                     DAYS_INTERVAL,
                                                     REMARKS,
                                                     NJUNS_STEP_ID,
                                                     NJUNS_MEMBER
                                                     )
                                                     VALUES({0},'{1}','{2}','{3}','{4}','{5}',{6},'{7}');",
                                                     GIS_NJUNS_TICKET_ID,
                                                     NJUNS_TICKET_ID,
                                                     JOB_TYPE,
                                                     NUMBER_POLES,
                                                     DAYS_INTERVAL,
                                                     REMARKS,
                                                     NJUNS_STEP_ID,
                                                     NJUNS_MEMBER
                                                      );
                insertQuery.AppendFormat("  END ; ");
                string insertSql = insertQuery.ToString();
                DataContext.Execute(insertSql, out iRecordsAffected, (int)CommandTypeEnum.adCmdText);
                return true;
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to insert step record", "Custom Create Ticket Error", MessageBoxButtons.OK);
                return false;
            }
        }

        /// <summary>
        /// Method to save ticket details
        /// </summary>
        /// <param name="ticket">Ticket object</param>
        /// <returns>True- If Saving is successful.Else false</returns>
        public bool SaveTicketAndStepDetails()
        {
            try
            {
                StringBuilder query = new StringBuilder();
                query.AppendFormat(" BEGIN ");
                query.AppendFormat(@"UPDATE GIS_ONC.NJUNS_TICKET SET 
                                                     TICKET_TYPE = '{0}',
                                                     TICKET_STATUS ='{1}' ,
                                                     POLE_FID = {2},
                                                     POLE_NUMBER = '{3}',
                                                     MISCELLANEOUS_ID = '{4}',
                                                     NJUNS_MEMBER_CODE = '{5}',
                                                     POLE_OWNER = '{6}',
                                                     CONTACT_NAME = '{7}',
                                                     CONTACT_PHONE = '{8}',
                                                     STATE = '{9}',
                                                     COUNTY ='{10}',
                                                     PLACE = '{11}',
                                                     LATITUDE = '{12}',
                                                     LONGITUDE = '{13}',
                                                     HOUSE_NUMBER = '{14}',
                                                     STREET_NAME = '{15}',
                                                     PRIORITY_CODE = '{16}',
                                                     JOB_TYPE = '{17}',
                                                     REMARKS = '{18}',
                                                     NUMBER_OF_POLES = '{19}',
                                                     DAYS_INTERVAL = {20},
                                                     START_DATE = {21},
                                                     WORK_REQUESTED_DATE = {22}
                                                     WHERE GIS_NJUNS_TICKET_ID = '{23}' ;",
                                                     TICKET_TYPE,
                                                     TICKET_STATUS,
                                                     POLE_FID,
                                                     POLE_NUMBER,
                                                     MISCELLANEOUS_ID,
                                                     NJUNS_MEMBER_CODE,
                                                     POLE_OWNER,
                                                     CONTACT_NAME,
                                                     CONTACT_PHONE,
                                                     STATE,
                                                     COUNTY,
                                                     PLACE,
                                                     LATITUDE,
                                                     LONGITUDE,
                                                     HOUSE_NUMBER,
                                                     STREET_NAME,
                                                     PRIORITY_CODE,
                                                     JOB_TYPE,
                                                     REMARKS,
                                                     NUMBER_OF_POLES,
                                                     DAYS_INTERVAL,
                                                      "'" + START_DATE.ToString("dd-MMM-yy") + "'",
                                                     "'" + WORK_REQUESTED_DATE.ToString("dd-MMM-yy") + "'",
                                                     GIS_NJUNS_TICKET_ID);

                Recordset tmpRs = GetRecordSet(string.Format("DELETE FROM GIS_ONC.NJUNS_STEP WHERE GIS_NJUNS_TICKET_ID = '{0}'", GIS_NJUNS_TICKET_ID));
                foreach (TicketStepType step in TicketStepTypeList)
                {
                    if (step == null)
                    {
                        continue;
                    }
                    query.AppendFormat(@"INSERT INTO GIS_ONC.NJUNS_STEP(
                                                     GIS_NJUNS_TICKET_ID,
                                                     NJUNS_TICKET_ID,
                                                     JOB_TYPE,
                                                     NUMBER_POLES,
                                                     DAYS_INTERVAL,
                                                     REMARKS,
                                                     NJUNS_MEMBER,  
                                                     NJUNS_STEP_ID
                                                     )
                                                     VALUES({0},{1},'{2}','{3}','{4}','{5}','{6}',{7});",
                                                    GIS_NJUNS_TICKET_ID,
                                                    NJUNS_TICKET_ID,
                                                    step.JobType,
                                                    step.NumberOfPoles,
                                                    step.DaysInterval,
                                                    step.Remarks,
                                                    step.CustomNjunsMemberValue,
                                                    step.ReferenceId
                                                 );
                }
                query.AppendFormat("  COMMIT ; ");
                query.AppendFormat("  END ; ");
                DataContext.Execute(query.ToString(), out int iRecordsAffected, (int)CommandTypeEnum.adCmdText);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void CommitTicketDetails()
        {
            try
            {
                int iRecordsAffected = 0;
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(" BEGIN ");
                sb.AppendFormat(" COMMIT; ");
                sb.AppendFormat("  END ; ");
                string commitQuery = sb.ToString();
                DataContext.Execute(commitQuery, out iRecordsAffected, (int)CommandTypeEnum.adCmdText);
            }
            catch (Exception)
            {

                throw;
            }
        }
        private int GetNextSequenceValue(string sequenceName)
        {
            try
            {
                string sql = "SELECT " + sequenceName + " .NEXTVAL FROM DUAL";
                Recordset tmpRs = GetRecordSet(string.Format("SELECT {0}.NEXTVAL SequenceValue FROM DUAL", sequenceName));
                if (tmpRs != null && tmpRs.RecordCount > 0)
                {
                    tmpRs.MoveFirst();
                    return Convert.ToInt32(tmpRs.Fields["SequenceValue"].Value);
                }
                return -1;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Method to update the NJUNS ticket Id and Ticket number in GIS NJUNS tables for GIS ticket Id
        /// </summary>
        public void UpdateTicketIdAndStatus()
        {
            try
            {
                string errorMessage = string.Empty;
                try
                {
                    int iRecordsAffected = 0;
                    StringBuilder updateQuery = new StringBuilder();
                    updateQuery.AppendFormat(" BEGIN ");
                    updateQuery.AppendFormat("UPDATE GIS_ONC.NJUNS_TICKET SET NJUNS_TICKET_ID  = {0}, TICKET_NUMBER = '{1}' WHERE GIS_NJUNS_TICKET_ID = '{2}' ;", NJUNS_TICKET_ID, TICKET_NUMBER, GIS_NJUNS_TICKET_ID);
                    updateQuery.AppendFormat("UPDATE GIS_ONC.NJUNS_STEP SET NJUNS_TICKET_ID  = {0} WHERE GIS_NJUNS_TICKET_ID = '{1}' ;", NJUNS_TICKET_ID, GIS_NJUNS_TICKET_ID);
                    updateQuery.AppendFormat("  COMMIT ; ");
                    updateQuery.AppendFormat("  END ; ");
                    DataContext.Execute(updateQuery.ToString(), out iRecordsAffected, (int)CommandTypeEnum.adCmdText);
                }
                catch (Exception ex)
                {
                    errorMessage = "GIS -" + m_interfacePoint + "  Failed :" + ex.Message;
                    SendReceiveMessageLogger m_messageLogger = new SendReceiveMessageLogger
                    {
                        Log_Detail = ex.Message
                    };
                    LogErrorMessages(m_messageLogger, m_interfacePoint);
                    if (Equals(Mode, 'I'))
                    {
                        MessageBox.Show(m_interfacePoint + " : " + errorMessage + Environment.NewLine + ex.Message, "Submit Ticket Error", MessageBoxButtons.OK);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Method to log messages to INTERFACE_LOG table and INTERFACE_XML_DATA 
        /// </summary>
        /// <param name="messageLogger">Instance of SendReceiveMessageLogger</param>
        public void LogErrorMessages(SendReceiveMessageLogger messageLogger, string interfacePoint)
        {
            try
            {
                if (messageLogger != null)
                {
                    messageLogger.dataContext = DataContext;
                    messageLogger.Interactive = Equals(Mode, 'I');
                    messageLogger.Correlation_Id = GetCorrelationId();
                    messageLogger.Interface_Name = interfacePoint;
                    messageLogger.Component_Name = "GTech Client";
                    messageLogger.logEntry();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to the EF url for input parameter name and Sub system name
        /// </summary>
        /// <param name="paramName"> param name in the table</param>
        /// <param name="subSystemComponent"> sub system component name for the corresponsing param name</param>
        /// <returns>EF url</returns>
        public string GetEFUrl(string paramName, string subSystemComponent)
        {
            Recordset resultRs = null;
            try
            {
                resultRs = GetRecordSet(string.Format("SELECT PARAM_VALUE FROM SYS_GENERALPARAMETER WHERE PARAM_NAME = '{0}' AND SUBSYSTEM_COMPONENT= '{1}'", paramName, subSystemComponent));
                if (resultRs != null && resultRs.RecordCount > 0)
                {
                    resultRs.MoveFirst();
                    return Convert.ToString(resultRs.Fields["PARAM_VALUE"].Value);
                }
                return string.Empty;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (resultRs != null)
                {
                    resultRs.Close();
                    resultRs = null;
                }
            }
        }

        /// <summary>
        /// Method to update theTicket status for input NJUNS ticket Id and Ticket number in GIS NJUNS tables
        /// </summary>
        public void UpdateTicketStatus()
        {
            string errorMessage = string.Empty;
            string updateTicketStatusSql;
            try
            {
                int iRecordsAffected = 0;

                if (!string.IsNullOrEmpty(NJUNS_TICKET_ID))
                {
                    updateTicketStatusSql = string.Format("UPDATE GIS_ONC.NJUNS_TICKET SET TICKET_STATUS  = '{0}' WHERE NJUNS_TICKET_ID = '{1}' AND TICKET_NUMBER = {2}", TICKET_STATUS, NJUNS_TICKET_ID, TICKET_NUMBER);
                }
                else
                {
                    updateTicketStatusSql = string.Format("UPDATE GIS_ONC.NJUNS_TICKET SET TICKET_STATUS  = '{0}' WHERE TICKET_NUMBER = {1}", TICKET_STATUS, TICKET_NUMBER);
                }
                DataContext.Execute(updateTicketStatusSql, out iRecordsAffected, (int)CommandTypeEnum.adCmdText);
            }
            catch (Exception ex)
            {
                errorMessage = string.Format("Failed to update ticket records for Ticket ID #", NJUNS_TICKET_ID);
                SendReceiveMessageLogger m_messageLogger = new SendReceiveMessageLogger
                {
                    Log_Detail = ex.Message
                };
                LogErrorMessages(m_messageLogger, m_interfacePoint);

                if (Equals(Mode, 'I'))
                {
                    MessageBox.Show(m_interfacePoint + " : " + errorMessage + Environment.NewLine + ex.Message, "UpdateTicketStatus Error", MessageBoxButtons.OK);
                }
            }
        }

        /// <summary>
        /// Generate a Correlation ID.
        /// </summary>
        /// <returns>Returns the Correleation string if it succeed
        ///    and string.Empty if it fails.</returns>
        private string GetCorrelationId()
        {
            string ReturnVal = string.Empty;
            Recordset tmpRs = null;
            string tmpQry = string.Empty;

            try
            {
                tmpQry = "select GIS_STG.CORRELATION_ID_SEQ.nextval " + (char)34 + "NEXTVAL" + (char)34 + " from dual";
                tmpRs = GetRecordSet(tmpQry);     //m_DataContext.OpenRecordset(tmpQry, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText);
                tmpRs.MoveFirst();
                ReturnVal = "GIS" + Convert.ToString(tmpRs.Fields[0].Value);
                return ReturnVal;
            }
            catch (Exception ex)
            {
                SendReceiveMessageLogger m_messageLogger = new SendReceiveMessageLogger
                {
                    Log_Detail = ex.Message
                };
                LogErrorMessages(m_messageLogger, "GetCorrelationId");

                if (Equals(Mode, 'I'))
                {
                    MessageBox.Show(m_interfacePoint + ex.Message, "GetCorrelationId Error", MessageBoxButtons.OK);
                }
                return ReturnVal;
            }
            finally
            {
                if (tmpRs != null)
                {
                    tmpRs.Close();
                    tmpRs = null;
                }
            }
        }


        /// <summary>
        /// Method to execute sql query and return the result record set
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns></returns>

        public Recordset GetRecordSet(string sqlString)
        {
            try
            {
                int outRecords = 0;
                Command command = new Command();
                command.CommandText = sqlString;
                Recordset results = DataContext.ExecuteCommand(command, out outRecords);
                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion
    }
}
