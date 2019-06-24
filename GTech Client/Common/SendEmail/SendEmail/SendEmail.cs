using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Windows.Forms;
using System.IO;
using System.Net;
using ADODB;
using Intergraph.GTechnology.API;
using gtInterfaceLogger;
using System.Diagnostics;

namespace SendEmail
{
    public class SendEmail
    {
        public eMailRequest EmailRequest = new eMailRequest();
        public string EFUrl = string.Empty;
        public IGTDataContext GTDataContext = null;
        public bool GTInteractive = true; // Is the code being run in an interactive session in GTech?
        private string correlationId = string.Empty;

        /// <summary>
        /// Queries the GTech database Application Config table 
        ///    for the correct url for EdgeFrontier's sendEmail system.
        /// If the EFURL property is populated it will use the provided URL.
        /// </summary>
        /// <returns>Returns 'true' if successfull and 'false' if it fails.</returns>
        private bool queryForURL()
        {
            bool tmpReturn = true;
            string tmpUrl = string.Empty;
            Recordset tmpRs = null;
            string tmpQry = "select PARAM_VALUE from SYS_GENERALPARAMETER where SUBSYSTEM_NAME = 'SEND_EMAIL'" +
                            " AND PARAM_NAME = 'EF_URL'";
            try
            {
                // Query the ONCR_APP_PARAMETER table to get the url of the EdgeFrontier SendEmail system.
                if (EFUrl == string.Empty)
                {
                    tmpRs = GTDataContext.OpenRecordset(tmpQry, CursorTypeEnum.adOpenStatic, 
                                                        LockTypeEnum.adLockReadOnly, 
                                                        (int)CommandTypeEnum.adCmdText);
                    tmpRs.MoveFirst();
                    tmpUrl = tmpRs.Fields[0].Value.ToString() ;
                    EFUrl = tmpUrl;
                }
            }
            catch(Exception e)
            {
                if (GTInteractive == true)
                {
                    MessageBox.Show("SendEmail:" + e.Message, "SendEmail.queryForURL Error", MessageBoxButtons.OK);
                    tmpReturn = false;

                }
                else
                {
                    logToDB("SendEmail.queryForURL Error:" + e.Message);
                }
            }
            return tmpReturn;
        }

        /// <summary>
        /// Sends an request to the EdgeFrontier Email system
        /// </summary>
        /// <returns>Returns 'true' if successfull and 'false' if it fails.</returns>
        public bool sendEmail()
        {
            XmlSerializer sendEmailSerializer = new XmlSerializer(typeof(eMailRequest));
            string tmpXML = string.Empty;
            StringWriter StrWrtr = new StringWriter();
            XmlWriter XmlWrtr = XmlWriter.Create(StrWrtr);
            HttpWebRequest request;
            IGTApplication tmpApp = null;
            HttpWebResponse webresponse;
            StreamReader tmpReader;
            StringBuilder strBldr = new StringBuilder();
            XmlDocument tmpXDoc = new XmlDocument();
            System.IO.Stream requestStream;
            byte[] XMLBytes;
            string tmpException = string.Empty;
            SendReceiveMessageLogger msgLogger;
            bool logger = true;

            try
            {
                if (GTDataContext == null)
                {
                    tmpApp = GTClassFactory.Create<IGTApplication>();
                    GTDataContext = tmpApp.DataContext;
                }
                if (EFUrl == string.Empty)
                {
                    queryForURL();
                }
                if (EmailRequest.Attachments == string.Empty)
                {
                    EmailRequest.Attachments = " ";
                }
                correlationId = getCorrelationId();

                // serialize the Email request
                sendEmailSerializer.Serialize(XmlWrtr, EmailRequest);
                tmpXML = StrWrtr.ToString();
                tmpXML = tmpXML.Replace("</eMailRequest>", "<Correlation_Id>" + correlationId + "</Correlation_Id></eMailRequest>"); // code for Testing

                // create and send the web request.
                XMLBytes = System.Text.Encoding.ASCII.GetBytes(tmpXML);

                // log the message to be sent.
                msgLogger = new SendReceiveMessageLogger();
                msgLogger.Interactive = GTInteractive;
                msgLogger.Interface_Name = "Send Email";
                msgLogger.Component_Name = "GTech Client";
                msgLogger.Correlation_Id = correlationId;
                msgLogger.xmlMessage = tmpXML;
                logger =  msgLogger.logEntry(); // add a log entry

                // send the request
                request = HttpWebRequest.Create(EFUrl) as HttpWebRequest;
                request.Method = "POST";
                request.ContentLength = XMLBytes.Length;
                request.ContentType = "text/xml; encoding='utf-8'";
                requestStream = request.GetRequestStream();
                requestStream.Write(XMLBytes, 0, XMLBytes.Length);
                requestStream.Close();
                webresponse = request.GetResponse() as HttpWebResponse;

                //Get the Response.
                
                tmpReader = new StreamReader(webresponse.GetResponseStream());
                strBldr.Append(tmpReader.ReadToEnd());

                // Log the responce
                // get the status from the 
                tmpXDoc.LoadXml(strBldr.ToString());
                XmlNode root = tmpXDoc.FirstChild;
                XmlNodeList tmpNodeLst = root.SelectNodes("//Status");
                tmpNodeLst[0].ToString();
                // If the status is failure.
                if (tmpNodeLst[0].InnerText == "FAILURE")
                {
                    tmpNodeLst = root.SelectNodes("//ErrorMsg");
                    string errStr = tmpNodeLst[0].InnerText;
                    errStr = errStr.Replace("'", "''");
                    tmpException = "Email Failure " + errStr;
                    msgLogger.Log_Detail = "SendEmail.sendEmail Error: " + tmpException;
                    msgLogger.xmlMessage = tmpXDoc.OuterXml;
                    //logToDB("SendEmail.sendEmail Error:" + tmpException);
                    logger = msgLogger.logEntry();
                    if(GTInteractive == true)
                    {
                        MessageBox.Show("SendEmail: " + tmpException, "sendEmail Error", MessageBoxButtons.OK);
                    }
                    logger = false;
                }
                else
                {
                    msgLogger.xmlMessage = tmpXDoc.OuterXml;
                    msgLogger.Log_Detail = "Success";
                    logger = msgLogger.logEntry();
                }
                
            }
            catch(Exception e)
            {
                if (GTInteractive == true)
                {
                    MessageBox.Show("SendEmail: " + e.Message, "sendEmail Error", MessageBoxButtons.OK);
                    logger = false;
                }
                else
                {
                    logToDB("SendEmail.sendEmail Error: " + e.Message);
                }
            }
            return logger;
        }

        /// <summary>
        /// Generate a Correlation ID.
        /// </summary>
        /// <returns>Returns the Correleation string if it succeed
        ///    and string.Empty if it fails.</returns>
        private string getCorrelationId()
        {
            string ReturnVal = string.Empty;
            Recordset tmpRs = null;
            string tmpQry = string.Empty;

            try
            {
                //tmpQry = "select GIS_STG.CORRELATION_ID_SEQ.nextval " + (char)34 + "NEXTVAL" + (char)34 + " from dual";
                tmpQry = "select GIS_STG.CORRELATION_ID_SEQ.nextval " + '"' + "NEXTVAL" + '"' + "  from dual";
                tmpRs = GTDataContext.OpenRecordset(tmpQry, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText);
                tmpRs.MoveFirst();
                ReturnVal = "GIS" + Convert.ToString(tmpRs.Fields[0].Value);
            }
            catch(Exception e)
            {
                if (GTInteractive == true)
                {
                    MessageBox.Show("SendEmail: " + e.Message, "getCorrelationId Error", MessageBoxButtons.OK);
                     ReturnVal = string.Empty;
                }
                else
                {
                    logToDB("SendEmail.getCorrelationId Error: " + e.Message);
                }
            }
            return ReturnVal;
        }

        /// <summary>
        /// Logs iterface transation (EdgeFrontier SendEmail)
        ///     to the  to the logging table.
        /// </summary>
        /// <param name="message">XML message that was sent.</param>
        /// <returns>Returns 'true' if successfull and 'false' if it fails.</returns>
        private bool logToDB(string message)
        {
            bool ReturnVal = true;
            SendReceiveMessageLogger msgLogger;
            try
            {
                msgLogger = new SendReceiveMessageLogger();
                msgLogger.Interactive = GTInteractive;
                msgLogger.Interface_Name = "Send Email";
                msgLogger.Component_Name = "GTech Client";
                msgLogger.Correlation_Id = correlationId;
                msgLogger.Log_Detail = message;
                //ReturnVal = msgLogger.logEntry();
            }
            catch( Exception e)
            {
                 if (GTInteractive == true)
                {
                    MessageBox.Show("SendEmail:" + e.Message, "getCorrelationId Error", MessageBoxButtons.OK);
                     
                }
                else
                {
                    if (EventLog.SourceExists("Application Error"))
                    {
                        EventLog.WriteEntry("Application Error", "Error in G/Technology Custom SendEmail - logErrorToDB: " + e.Message);
                    }
                }
                ReturnVal = false;
            }
            return ReturnVal;
        }



    }
}
