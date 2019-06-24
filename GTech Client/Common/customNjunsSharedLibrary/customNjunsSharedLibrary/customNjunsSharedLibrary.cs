//----------------------------------------------------------------------------+
//        Class: customNjunsSharedLibrary
//  Description: This custom dll exposes methods to the following operations WRT to GIS and NJUNS Ticket-
//                                              Create Ticket
//                                              Submit Ticket
//                                              Check Ticket Status
//                                              Submit Ticket by WR and Mode
//                                              Check Ticket Status by WR and mode
//
//----------------------------------------------------------------------------+
//     $Author:: hkonda                                   $
//       $Date:: 22/02/18                                 $
//   $Revision:: 1                                        $
//----------------------------------------------------------------------------+
//    $History:: customNjunsSharedLibrary.cs                     $
// 
// *****************  Version 1  *****************
// User: hkonda     Date: 22/02/18   Time: 18:00  Desc : Created
// User: hkonda     Date: 30/09/18   Time: 18:00  Desc : Code changes made to support latest schema changes
//----------------------------------------------------------------------------+

using ADODB;
using Intergraph.GTechnology.API;
using System;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Net;
using gtInterfaceLogger;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OncorTicketCreation;
using OncorTicketStatus;
using System.Xml;

namespace GTechnology.Oncor.CustomAPI
{
    public class customNjunsSharedLibrary
    {
        #region Private fields

        const int m_DistanceFromPoleInFeet = 300;
        const string m_InterfacePoint_ST = "Submit Ticket ";
        const string m_InterfacePoint_ST_WR = "Submit Ticket By WR ";
        const string m_InterfacePoint_TS = "Ticket Status ";
        const string m_InterfacePoint_TS_WR = "Ticket Status By WR ";

        string m_EfUrl;
        string m_RequestXMl = string.Empty;
        string m_interfacePoint = string.Empty;
        int stepId = 0;
        //List<int> m_UnSubmittedTickets = null;
        //List<string> ticketNumbers = null;
        Dictionary<int, string> ticketStatusCollection = null;
        SendReceiveMessageLogger m_messageLogger = null;
        SendReceiveXMLMessage m_sendReceiveXMLMessage = null;
        TicketCreationType m_ticketCreation = null;
        DataLayer dataLayer = new DataLayer();

        #endregion

        #region Public properties
        public int GIS_NJUNS_TICKET_ID { get; set; }
        public string NJUNS_TICKET_ID { get; set; }
        public int TICKET_NUMBER { get; set; }
        public string TICKET_TYPE { get; set; }
        public string TICKET_STATUS { get; set; }
        public int POLE_FID { get; set; }
        public string POLE_NUMBER { get; set; }
        public string MISCELLANEOUS_ID { get; set; }
        public string NJUNS_MEMBER_CODE { get; set; }
        public string NJUNS_CODE_GUID { get; set; }
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
        public IGTDataContext m_DataContext { get; set; }

        public char Mode { get; set; }

        //public TicketCreationType TicketCreationType { get; set; } 
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public customNjunsSharedLibrary()
        {
            m_DataContext = GTClassFactory.Create<IGTApplication>().DataContext;
            dataLayer.DataContext = m_DataContext;
        }

        #region Public Methods
        /// <summary>
        /// Method to Create Ticket
        /// </summary>
        /// <param name="wrNumber">Work Request number</param>
        /// <param name="poleFid">Pole Feature Identifier</param>
        /// <returns>True, if Creation is successful. </returns>
        public bool CreateTicket(string wrNumber, int poleFid)
        {
            try
            {
                POLE_FID = poleFid;
                MISCELLANEOUS_ID = wrNumber;
                if (CreateNewTicket())
                {
                    return CreateStepForTicket();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Custom Create Ticket Error", MessageBoxButtons.OK);
                return false;
            }
        }

        /// <summary>
        /// Method to submit ticket to EF system
        /// </summary>
        /// <param name="gisNjunsTicketId">GIS Njuns Ticket Id - Ticket Id generated in GIS DB while creating the ticket</param>
        /// <param name="mode">'I' for Interactive, 'B' for Batch</param>
        public bool SubmitTicket(int gisNjunsTicketId, char mode)
        {
            XDocument xDocument = null;
            string errorMessage = string.Empty;
            string resultCode = string.Empty;
            try
            {
                Mode = mode;
                m_interfacePoint = m_InterfacePoint_ST;
                if (!GetTicketAttributes(gisNjunsTicketId))
                {
                    return false;
                }
                GIS_NJUNS_TICKET_ID = gisNjunsTicketId;
                m_messageLogger = new SendReceiveMessageLogger();
                m_EfUrl = dataLayer.GetEFUrl("EF_URL", "NJUNS_SubmitTicket");

                var xmlString = new StringWriter();
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(SerializableTicketCreation));

                //string Utf8;

                SerializableTicketCreation serializableTicket = new SerializableTicketCreation
                {
                    RequestHeaderType = new OncorTicketCreation.RequestHeaderType()
                };
                serializableTicket.RequestHeaderType.SourceSystem = "GIS";
                serializableTicket.RequestHeaderType.Timestamp = DateTime.Now;
                serializableTicket.RequestHeaderType.TransactionType = "RequestReply";
                serializableTicket.RequestHeaderType.TransactionId = gisNjunsTicketId.ToString();
                serializableTicket.RequestHeaderType.Requestor = GetRacfID();
                serializableTicket.TicketCreationType = m_ticketCreation;

                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");

                xmlSerializer.Serialize(xmlString, serializableTicket, ns);

                m_RequestXMl = xmlString.ToString();
                m_RequestXMl = AddIDToAttribute(m_RequestXMl);

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(m_RequestXMl);
                XmlDeclaration declaration = xml.CreateXmlDeclaration("1.0", "UTF-8", null);
                XmlElement root = xml.DocumentElement;
                xml.InsertBefore(declaration, root);

                m_RequestXMl = xml.InnerXml;

                m_sendReceiveXMLMessage = new SendReceiveXMLMessage
                {
                    URL = m_EfUrl,
                    Method = "POST",
                    RequestXMLBody = m_RequestXMl
                };
                m_messageLogger.xmlMessage = m_RequestXMl;

                dataLayer.LogErrorMessages(m_messageLogger, m_interfacePoint);
                m_sendReceiveXMLMessage.SendMsgToEF();
                string ticketCreationResponse = m_sendReceiveXMLMessage.ResponseXML;

                if (!string.IsNullOrEmpty(ticketCreationResponse))
                {
                    using (TextReader reader = new StringReader(ticketCreationResponse))
                    {
                        xDocument = XDocument.Load(reader);
                        NJUNS_TICKET_ID = xDocument.Root.Elements("TicketCreation").Elements("NjunsTicketId").First().Value;// Convert.ToString(xDocument.Element("TicketCreation").Element("NjunsTicketId").Value);
                        TICKET_NUMBER = Convert.ToInt32(xDocument.Root.Elements("TicketCreation").Elements("TicketNumber").First().Value); //Convert.ToInt32(xDocument.Element("TicketCreation").Element("TicketNumber").Value);
                    }
                }
                dataLayer.CopyPropertiesFrom(this);
                m_messageLogger = new SendReceiveMessageLogger
                {
                    xmlMessage = ticketCreationResponse,
                    Log_Detail = "Success"
                };
                dataLayer.LogErrorMessages(m_messageLogger, m_interfacePoint);
                dataLayer.UpdateTicketIdAndStatus();
                return true;
            }

            catch (WebException ex) when (ex.Status == WebExceptionStatus.Timeout)
            {
                errorMessage = "Timeout has occurred waiting for GIS- " + m_interfacePoint + " to process.";
                if (ex.Response != null)
                    resultCode = Convert.ToString(((HttpWebResponse)ex.Response).StatusCode);

                m_messageLogger.Result_Status = ex.Status.ToString();
                m_messageLogger.Result_Code = resultCode;
                m_messageLogger.Log_Detail = ex.Message;
                dataLayer.CopyPropertiesFrom(this);
                dataLayer.LogErrorMessages(m_messageLogger, m_interfacePoint);

                if (Equals(Mode, 'I'))
                {
                    MessageBox.Show(m_interfacePoint + " : " + errorMessage, "Submit Ticket Error", MessageBoxButtons.OK);
                }
                return false;
            }

            catch (WebException ex) when (ex.Status == WebExceptionStatus.ReceiveFailure)
            {
                errorMessage = "GIS-" + m_interfacePoint + "  Failed: Could not response.";
                if (ex.Response != null)
                    resultCode = Convert.ToString(((HttpWebResponse)ex.Response).StatusCode);

                m_messageLogger.Result_Status = ex.Status.ToString();
                m_messageLogger.Result_Code = resultCode;
                m_messageLogger.Log_Detail = ex.Message;

                dataLayer.LogErrorMessages(m_messageLogger, m_interfacePoint);

                if (Equals(Mode, 'I'))
                {
                    MessageBox.Show(m_interfacePoint + " : " + errorMessage, "Submit Ticket Error", MessageBoxButtons.OK);
                }
                return false;
            }

            catch (WebException ex) when (ex.Status == WebExceptionStatus.ConnectFailure)
            {
                errorMessage = "GIS-" + m_interfacePoint + "  Failed: Connection not found.";
                if (ex.Response != null)
                    resultCode = Convert.ToString(((HttpWebResponse)ex.Response).StatusCode);

                m_messageLogger.Result_Status = ex.Status.ToString();
                m_messageLogger.Result_Code = resultCode;
                m_messageLogger.Log_Detail = ex.Message;

                dataLayer.LogErrorMessages(m_messageLogger, m_interfacePoint);

                if (Equals(Mode, 'I'))
                {
                    MessageBox.Show(m_interfacePoint + " : " + errorMessage, "Submit Ticket Error", MessageBoxButtons.OK);
                }
                return false;
            }
            catch (Exception ex)
            {
                m_messageLogger.Log_Detail = ex.Message;
                dataLayer.LogErrorMessages(m_messageLogger, m_interfacePoint);

                if (Equals(Mode, 'I'))
                {
                    MessageBox.Show(m_interfacePoint + " : " + errorMessage, "Submit Ticket Error", MessageBoxButtons.OK);
                }
                return false;
            }
        }

        private string AddIDToAttribute(string xml)
        {
            XDocument doc = XDocument.Parse(xml);
            IEnumerable<XElement> elements = doc.Elements().Elements();
            //string guid = Guid.NewGuid().ToString();
            foreach (XElement item in elements)
            {
                foreach (XElement elem in item.Elements())
                {
                    if (elem.Name.LocalName == "NjunsMemberCode" && elem.Parent.Name.LocalName == "TicketCreation")
                    {
                        elem.Add(new XAttribute("Id", NJUNS_CODE_GUID));
                    }
                    if (elem.Name.LocalName == "PoleOwner" && elem.Parent.Name.LocalName == "TicketCreation")
                    {
                        elem.Add(new XAttribute("Id", NJUNS_CODE_GUID));
                    }
                    if (elem.Name.LocalName == "TicketSteps")
                    {
                        foreach (XElement subElement in elem.Elements())
                        {
                            foreach (XElement element in subElement.Elements())
                            {
                                if (element.Name.LocalName == "NjunsMemberCode")
                                {
                                    element.Add(new XAttribute("Id", NJUNS_CODE_GUID));
                                    break;
                                }
                            }

                        }

                    }
                }

            }
            return doc.ToString();
        }

        private string GetRacfID()
        {
            return m_DataContext.DatabaseUserName;
        }


        /// <summary>
        /// Method to check the status of the ticket
        /// </summary>
        /// <param name="nJunsTicketId">NJUNS Ticket id for which status is to be checked</param>
        /// <param name="mode">'I' for Interactive, 'B' for Batch</param>
        /// <returns>Ticket Status</returns>
        public string CheckTicketStatus(string nJunsTicketId, char mode)
        {
            XDocument xDocument = null;
            string errorMessage = string.Empty;
            string resultCode = string.Empty;
            try
            {

                Mode = mode;
                m_interfacePoint = m_InterfacePoint_TS;
                m_messageLogger = new SendReceiveMessageLogger();
                if (string.IsNullOrEmpty(nJunsTicketId))
                {
                    throw new Exception("Failed to check the status of ticket. Required fields are empty.");
                }
                m_EfUrl = dataLayer.GetEFUrl("EF_URL", "NJUNS_CheckTicketStatus");
                TicketStatusType ticketStatusType = new TicketStatusType
                {
                    NjunsTicketId = nJunsTicketId
                };

                var xmlString = new StringWriter();
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(SerializableTicketStatus));

                SerializableTicketStatus serializableTicketStatus = new SerializableTicketStatus
                {
                    RequestHeaderType = new OncorTicketStatus.RequestHeaderType()
                };
                serializableTicketStatus.RequestHeaderType.SourceSystem = "GIS";
                serializableTicketStatus.RequestHeaderType.Timestamp = DateTime.Now;
                serializableTicketStatus.RequestHeaderType.TransactionType = "RequestReply";
                serializableTicketStatus.RequestHeaderType.TransactionId = nJunsTicketId;
                serializableTicketStatus.RequestHeaderType.Requestor = GetRacfID();
                serializableTicketStatus.TicketStatusype = ticketStatusType;

                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                xmlSerializer.Serialize(xmlString, serializableTicketStatus, ns);
                m_RequestXMl = xmlString.ToString();

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(m_RequestXMl);
                foreach (XmlNode node in xml)
                {
                    if (node.NodeType == XmlNodeType.XmlDeclaration)
                        xml.RemoveChild(node);
                }

                XmlDeclaration declaration = xml.CreateXmlDeclaration("1.0", "UTF-8", null);
                XmlElement root = xml.DocumentElement;
                xml.InsertBefore(declaration, root);

                m_RequestXMl = xml.InnerXml;

                m_sendReceiveXMLMessage = new SendReceiveXMLMessage
                {
                    URL = m_EfUrl,
                    Method = "POST",
                    RequestXMLBody = m_RequestXMl
                };
                m_messageLogger.xmlMessage = m_RequestXMl;
                dataLayer.LogErrorMessages(m_messageLogger, m_interfacePoint);

                m_sendReceiveXMLMessage.SendMsgToEF();

                string ticketStatusResponse = m_sendReceiveXMLMessage.ResponseXML;
                if (!string.IsNullOrEmpty(ticketStatusResponse))
                {
                    using (TextReader reader = new StringReader(ticketStatusResponse))
                    {
                        xDocument = XDocument.Load(reader);
                        //TICKET_STATUS = xDocument.Root.Elements("TicketStatus").Elements("TicketStatus").First().Value;
                        TICKET_STATUS = xDocument.Root.Elements().Last().Elements().Last().Value;
                    }
                }
                dataLayer.CopyPropertiesFrom(this);
                if (string.Equals(TICKET_STATUS, "NoTicketFound"))
                {
                    m_messageLogger = new SendReceiveMessageLogger();
                    m_messageLogger.xmlMessage = ticketStatusResponse;
                    throw new WebException("Failure", WebExceptionStatus.ReceiveFailure);
                }

                m_messageLogger = new SendReceiveMessageLogger
                {
                    xmlMessage = ticketStatusResponse,
                    Log_Detail = "Success"
                };
                dataLayer.LogErrorMessages(m_messageLogger, m_interfacePoint);

                dataLayer.UpdateTicketStatus();
                return TICKET_STATUS;
            }

            catch (WebException ex) when (ex.Status == WebExceptionStatus.Timeout)
            {
                errorMessage = "NJUNS Get Ticket Status – It took too long to get a response to the message that was sent.";
                if (ex.Response != null)
                    resultCode = Convert.ToString(((HttpWebResponse)ex.Response).StatusCode);

                m_messageLogger.Result_Status = ex.Status.ToString();
                m_messageLogger.Result_Code = resultCode;
                m_messageLogger.Log_Detail = ex.Message;

                dataLayer.LogErrorMessages(m_messageLogger, m_interfacePoint);

                if (Equals(Mode, 'I'))
                {
                    MessageBox.Show(m_interfacePoint + " : " + errorMessage, "CheckTicketStatus Error", MessageBoxButtons.OK);
                }
                return TICKET_STATUS;
            }

            catch (WebException ex) when (ex.Status == WebExceptionStatus.ReceiveFailure)
            {
                errorMessage = "NJUNS Get Ticket Status - Could not get a Ticket Status.";
                if (ex.Response != null)
                    resultCode = Convert.ToString(((HttpWebResponse)ex.Response).StatusCode);

                m_messageLogger.Result_Status = ex.Status.ToString();
                m_messageLogger.Result_Code = resultCode;
                m_messageLogger.Log_Detail = ex.Message;

                dataLayer.LogErrorMessages(m_messageLogger, m_interfacePoint);

                if (Equals(Mode, 'I'))
                {
                    MessageBox.Show(m_interfacePoint + " : " + errorMessage, "CheckTicketStatus Error", MessageBoxButtons.OK);
                }
                return TICKET_STATUS;
            }

            catch (WebException ex) when (ex.Status == WebExceptionStatus.ConnectFailure)
            {
                errorMessage = "NJUNS Get Ticket Status – Could not conntect to the EdgeFrontier NJUNS Submit Web Service.";
                if (ex.Response != null)
                    resultCode = Convert.ToString(((HttpWebResponse)ex.Response).StatusCode);

                m_messageLogger.Result_Status = ex.Status.ToString();
                m_messageLogger.Result_Code = resultCode;
                m_messageLogger.Log_Detail = ex.Message;

                dataLayer.LogErrorMessages(m_messageLogger, m_interfacePoint);

                if (Equals(Mode, 'I'))
                {
                    MessageBox.Show(m_interfacePoint + " : " + errorMessage, "CheckTicketStatus Error", MessageBoxButtons.OK);
                }
                return TICKET_STATUS;
            }
            catch (Exception ex)
            {
                m_messageLogger.Log_Detail = ex.Message;
                dataLayer.LogErrorMessages(m_messageLogger, m_interfacePoint);

                if (Equals(Mode, 'I'))
                {
                    MessageBox.Show(m_interfacePoint + " : " + ex.Message, "CheckTicketStatus Error", MessageBoxButtons.OK);
                }
                return TICKET_STATUS;
            }
        }

        /// <summary>
        /// Method to Submit Tickets
        /// </summary>
        /// <param name="wrNumber">Work request number for which tickets needs to submitted</param>
        /// <param name="mode">'I' for Interactive, 'B' for Batch</param>
        public void SubmitTicketByWr(string wrNumber, char mode)
        {
            try
            {
                MISCELLANEOUS_ID = wrNumber;
                Mode = mode;
                m_interfacePoint = m_InterfacePoint_ST_WR;
                m_messageLogger = new SendReceiveMessageLogger();
                dataLayer.CopyPropertiesFrom(this);
                SubmitTickets(GetUnsubmittedTickets());
            }
            catch (Exception ex)
            {
                m_messageLogger.Log_Detail = ex.Message;
                dataLayer.LogErrorMessages(m_messageLogger, m_interfacePoint);
                if (Equals(Mode, 'I'))
                {
                    MessageBox.Show(m_interfacePoint + " : " + ex.Message, "Submit Ticket By WR Error", MessageBoxButtons.OK);
                }
            }
        }

        /// <summary>
        /// Method to check the ticket statuses in a WR
        /// </summary>
        /// <param name="wrNumber">Work request number for which tickets needs to submitted</param>
        /// <param name="mode">'I' for Interactive, 'B' for Batch</param>
        /// <returns>Key Value pair of Ticket Number and Ticket Status</returns>
        public Dictionary<int, string> CheckTicketStatusByWR(string wrNumber, char mode)
        {
            List<string> njunsTicketIds = new List<string>();
            List<string> ticketNumbers = new List<string>();
            try
            {
                MISCELLANEOUS_ID = wrNumber;
                Mode = mode;
                m_interfacePoint = m_InterfacePoint_TS_WR;
                m_messageLogger = new SendReceiveMessageLogger();
                ticketNumbers = GetTicketNumberForPoles(out njunsTicketIds);
                return GetStatusForTicketNumbers(ticketNumbers,njunsTicketIds);
            }
            catch (Exception ex)
            {
                m_messageLogger.Log_Detail = ex.Message;
                dataLayer.LogErrorMessages(m_messageLogger, m_interfacePoint);
                if (Equals(Mode, 'I'))
                {
                    MessageBox.Show(m_interfacePoint + " : " + ex.Message, "Ticket Status By WR Error", MessageBoxButtons.OK);
                }
                return null;
            }
        }

        #endregion

        #region Ticket Methods

        /// <summary>
        /// Method to Create new ticket record in NJUNS_TICKET table
        /// </summary>
        /// <returns>True, if ticket creation is successful</returns>
        private bool CreateNewTicket()
        {
            try
            {
                if (GetPoleAttributes())
                {
                    dataLayer.CopyPropertiesFrom(this);
                    return dataLayer.InsertNewTicketRecord();
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get the pole attributes and populates Ticket Attributes class
        /// </summary>
        /// <returns>True, if Ticket Attributes are populated successfully. Else returns false</returns>
        private bool GetPoleAttributes()
        {
            string serviceAreaCode = string.Empty;
            string countyBoundaryId = string.Empty;
            int nearestServicePointFid = 0;
            Dictionary<int, double> servicePointsDictionary = null;

            try
            {
                IGTKeyObject gTKeyObject = m_DataContext.OpenFeature(110, POLE_FID);
                Recordset owningCOmpnayRs = gTKeyObject.Components.GetComponent(11001).Recordset;
                owningCOmpnayRs.MoveFirst();
                string owningCompany = Convert.ToString(owningCOmpnayRs.Fields["OWNING_COMPANY_C"].Value);// 
                //WORK_REQUESTED_DATE = ((DateTime)dataLayer.GetRecordSet(string.Format("SELECT G3E_DATECREATED FROM G3E_JOB WHERE G3E_IDENTIFIER = '{0}'", m_DataContext.ActiveJob)).Fields["G3E_DATECREATED"].Value).ToString("dd-MMM-yy");
                WORK_REQUESTED_DATE = ((DateTime)dataLayer.GetRecordSet(string.Format("SELECT G3E_DATECREATED FROM G3E_JOB WHERE G3E_IDENTIFIER = '{0}'", m_DataContext.ActiveJob)).Fields["G3E_DATECREATED"].Value);

                Recordset rs = gTKeyObject.Components.GetComponent(1).Recordset;
                if (rs != null && rs.RecordCount > 0)
                {
                    rs.MoveFirst();
                    POLE_NUMBER = Convert.ToString(rs.Fields["STRUCTURE_ID"].Value);
                    LATITUDE = Convert.ToDecimal(rs.Fields["LATITUDE"].Value);
                    LONGITUDE = Convert.ToDecimal(rs.Fields["LONGITUDE"].Value);
                }
                IGTGeometry geometry = gTKeyObject.Components.GetComponent(11002).Geometry;
                IGTPoint point = GTClassFactory.Create<IGTPoint>();
                point.X = geometry.FirstPoint.X;
                point.Y = geometry.FirstPoint.Y;
                point.Z = geometry.FirstPoint.Z;
                customBoundaryQuery boundaryQuery = new customBoundaryQuery(point, 235);// County boundary  FNO 235
                Recordset resultRs1 = boundaryQuery.PerformPointInPolygon();
                if (resultRs1 != null && resultRs1.RecordCount > 0)
                {
                    IGTKeyObject countyFeature = m_DataContext.OpenFeature(Convert.ToInt16(resultRs1.Fields["G3E_FNO"].Value), Convert.ToInt32(resultRs1.Fields["G3E_FID"].Value));
                    Recordset boundaryAttributesRs = countyFeature.Components.GetComponent(23501).Recordset; // // County Boundary Attributes
                    if (boundaryAttributesRs != null && boundaryAttributesRs.RecordCount > 0)
                    {
                        boundaryAttributesRs.MoveFirst();
                        COUNTY = Convert.ToString(boundaryAttributesRs.Fields["NAME"].Value);
                        if (!string.IsNullOrEmpty(Convert.ToString(boundaryAttributesRs.Fields["ID"].Value)))
                        {
                            countyBoundaryId = Convert.ToString(boundaryAttributesRs.Fields["ID"].Value).Replace("TX", string.Empty).Trim();

                        }
                    }
                }
                if (!string.IsNullOrEmpty(countyBoundaryId))
                {
                    int nJunsMemberId = 0;

                    Recordset rs2 = dataLayer.GetRecordSet(string.Format("SELECT NJUNS_MEMBER_ID,CONTACT_NAME,CONTACT_PHONE FROM NJUNS_CONTACTS WHERE COUNTY_N_ID = {0}", countyBoundaryId));
                    if (rs2 != null && rs2.RecordCount > 0)
                    {
                        rs2.MoveFirst();
                        CONTACT_NAME = Convert.ToString(rs2.Fields["CONTACT_NAME"].Value);
                        CONTACT_PHONE = Convert.ToString(rs2.Fields["CONTACT_PHONE"].Value);
                        // m_TicketAttributes.PLACE = Convert.ToString(contactRs.Fields["NJUNS_PLACE_NAME"].Value);
                        nJunsMemberId = Convert.ToInt32(rs2.Fields["NJUNS_MEMBER_ID"].Value);
                        rs2.Close();
                        rs2 = null;
                    }
                    rs2 = dataLayer.GetRecordSet(string.Format("SELECT NJUNS_MEMBER FROM NJUNS_MEMBER WHERE NJUNS_MEMBER_ID = {0}", nJunsMemberId));
                    //rs2 = dataLayer.GetRecordSet(string.Format("SELECT NJUNS_CODE FROM NJUNS_MEMBER_COUNTY WHERE NJUNS_MEMBER_ID = {0}", nJunsMemberId));
                    if (rs2 != null && rs2.RecordCount > 0)
                    {
                        rs2.MoveFirst();
                        //NJUNS_MEMBER_CODE = Convert.ToString(rs.Fields["NJUNS_MEMBER"].Value);
                        NJUNS_MEMBER_CODE = Convert.ToString(rs2.Fields["NJUNS_MEMBER"].Value);
                        POLE_OWNER = owningCompany; // Convert.ToString(contactRs.Fields["NJUNS_MEMBER"].Value);
                    }
                }

                customBufferQuery oCustomBufferQuery = new customBufferQuery(point, m_DistanceFromPoleInFeet, 55); //Closest Premise feature within 300 feet of the Pole. Service point FNO 55
                servicePointsDictionary = oCustomBufferQuery.PerformBufferQuery();
                if (servicePointsDictionary != null && servicePointsDictionary.Count > 0)
                {
                    servicePointsDictionary.OrderBy(key => key.Value);
                    nearestServicePointFid = servicePointsDictionary.First().Key;
                    IGTKeyObject servicePointFeature = m_DataContext.OpenFeature(55, nearestServicePointFid);
                    Recordset premiseAttributesRs = servicePointFeature.Components.GetComponent(5504).Recordset; // PREMISE_N
                    if (premiseAttributesRs != null && premiseAttributesRs.RecordCount > 0)
                    {
                        premiseAttributesRs.MoveFirst();
                        HOUSE_NUMBER = Convert.ToString(premiseAttributesRs.Fields["HOUSE_NBR"].Value);
                        STREET_NAME = Convert.ToString(premiseAttributesRs.Fields["DIR_LEADING_C"].Value) + " "
                            + Convert.ToString(premiseAttributesRs.Fields["STREET_NM"].Value) + " "
                            + Convert.ToString(premiseAttributesRs.Fields["STREET_TYPE_C"].Value) + " "
                            + Convert.ToString(premiseAttributesRs.Fields["DIR_TRAILING_C"].Value);
                    }
                }

                // Set default vaues
                TICKET_NUMBER = -1;
                TICKET_TYPE = "Transfer";
                TICKET_STATUS = "OPEN";
                STATE = "TX";
                PRIORITY_CODE = "3";
                NJUNS_TICKET_ID = "-1";
                DAYS_INTERVAL = "30";
                JOB_TYPE = "Transfer";
                NUMBER_OF_POLES = "1";
                REMARKS = string.Empty;
                return true;
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to populate some ticket attributes.", "Custom Create Ticket Error", MessageBoxButtons.OK);
                return false;
            }
        }
        #endregion

        #region Step Methods

        /// <summary>
        /// Create a new record in Step Table
        /// </summary>
        /// <returns>True, if Step Creation is successful. Else returns false</returns>
        private bool CreateStepForTicket()
        {
            Recordset resultRs = null;
            try
            {
                resultRs = dataLayer.GetRecordSet(string.Format("SELECT GIS_NJUNS_TICKET_ID FROM GIS_ONC.NJUNS_TICKET WHERE POLE_FID = {0} ORDER BY GIS_NJUNS_TICKET_ID DESC", POLE_FID));
                if (resultRs != null && resultRs.RecordCount > 0)
                {
                    resultRs.MoveFirst();
                    GIS_NJUNS_TICKET_ID = Convert.ToInt32(resultRs.Fields["GIS_NJUNS_TICKET_ID"].Value);
                    //dataLayer.GIS_NJUNS_TICKET_ID = GIS_NJUNS_TICKET_ID;
                }
                NJUNS_TICKET_ID = "-1";
                NJUNS_MEMBER = NJUNS_MEMBER_CODE;
                JOB_TYPE = "Transfer";
                NUMBER_POLES = "1";
                DAYS_INTERVAL = "30";
                dataLayer.CopyPropertiesFrom(this);
                CreateStepsForEachAttachment();
                dataLayer.CommitTicketDetails();
                return false;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                resultRs.Close();
                resultRs = null;
            }
        }
        private void CreateInitialStep()
        {
            InsertStep();
        }

        private void CreateFinalStep()
        {
            InsertStep();
        }

        /// <summary>
        /// Method to Create Steps for Each Attachement on the pole
        /// </summary>
        private bool CreateStepsForEachAttachment()
        {
            try
            {
                IGTKeyObject gTKeyObject = m_DataContext.OpenFeature(110, POLE_FID);
                Recordset wireLineAttachmentRs = gTKeyObject.Components.GetComponent(34).Recordset;
                if (wireLineAttachmentRs != null && wireLineAttachmentRs.RecordCount > 0)
                {
                    wireLineAttachmentRs.MoveFirst();
                    while (!wireLineAttachmentRs.EOF)
                    {
                        CreateInitialStep();
                        InsertStep();
                        CreateFinalStep();
                        wireLineAttachmentRs.MoveNext();
                    }
                }
                Recordset equipmentAttachmentRs = gTKeyObject.Components.GetComponent(35).Recordset;
                if (equipmentAttachmentRs != null && equipmentAttachmentRs.RecordCount > 0)
                {
                    equipmentAttachmentRs.MoveFirst();
                    while (!equipmentAttachmentRs.EOF)
                    {
                        CreateInitialStep();
                        InsertStep();
                        CreateFinalStep();
                        equipmentAttachmentRs.MoveNext();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to Insert Step record in table
        /// </summary>
        /// <returns>True, if insertion is successful. Else returns false</returns>
        private bool InsertStep()
        {
            try
            {
                stepId = stepId + 1;
                dataLayer.NJUNS_STEP_ID = stepId;
                return dataLayer.InsertStepRecords();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Submit Ticket Methods


        /// <summary>
        /// Method to read the Ticket attributes into TicketCreationType class for the input GIS ticket Id 
        /// </summary>
        /// <param name="m_gisTicketId"></param>
        /// <returns>true, if ticket attributes exists in GIS. Else returns false.</returns>
        public bool GetTicketAttributes(int m_gisTicketId)
        {
            try
            {
                Recordset ticketAttributesRs = dataLayer.GetRecordSet(string.Format("SELECT * FROM GIS_ONC.NJUNS_TICKET WHERE GIS_NJUNS_TICKET_ID = {0}", m_gisTicketId));
                if (ticketAttributesRs != null && ticketAttributesRs.RecordCount == 0)
                {
                    MessageBox.Show(m_interfacePoint + " : " + " Invalid ticket ID.", "Submit Ticket Error", MessageBoxButtons.OK);
                    return false;
                }
                m_ticketCreation = new TicketCreationType();
                TICKET_TYPE = m_ticketCreation.TicketType = Convert.ToString(ticketAttributesRs.Fields["TICKET_TYPE"].Value);
                POLE_NUMBER = m_ticketCreation.PoleNumber = Convert.ToString(ticketAttributesRs.Fields["POLE_NUMBER"].Value);
                MISCELLANEOUS_ID = m_ticketCreation.MiscellaneousId = Convert.ToString(ticketAttributesRs.Fields["MISCELLANEOUS_ID"].Value);
                m_ticketCreation.NjunsMemberCode = new MemberType();
                NJUNS_MEMBER_CODE = m_ticketCreation.NjunsMemberCode.Value = Convert.ToString(ticketAttributesRs.Fields["NJUNS_MEMBER_CODE"].Value);
                NJUNS_CODE_GUID = GetGuidForMemberCode(NJUNS_MEMBER_CODE);
                m_ticketCreation.PoleOwner = new MemberType();
                POLE_OWNER = m_ticketCreation.PoleOwner.Value = Convert.ToString(ticketAttributesRs.Fields["POLE_OWNER"].Value);
                //START_DATE = m_ticketCreation.StartDate = Convert.ToDateTime(ticketAttributesRs.Fields["START_DATE"].Value).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");  // Format date as 2016-11-08T00:00:00
                //WORK_REQUESTED_DATE = m_ticketCreation.WorkRequestedDate = Convert.ToDateTime(ticketAttributesRs.Fields["WORK_REQUESTED_DATE"].Value).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");// Format date as 2016-11-08T00:00:00
                START_DATE =  Convert.ToDateTime(ticketAttributesRs.Fields["START_DATE"].Value);  // Format date as 2016-11-08T00:00:00
                m_ticketCreation.StartDate = Convert.ToDateTime(ticketAttributesRs.Fields["START_DATE"].Value).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
                WORK_REQUESTED_DATE = Convert.ToDateTime(ticketAttributesRs.Fields["WORK_REQUESTED_DATE"].Value);
                m_ticketCreation.WorkRequestedDate = Convert.ToDateTime(ticketAttributesRs.Fields["WORK_REQUESTED_DATE"].Value).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
                CONTACT_NAME = m_ticketCreation.ContactName = Convert.ToString(ticketAttributesRs.Fields["CONTACT_NAME"].Value);
                CONTACT_PHONE = m_ticketCreation.ContactPhone = Convert.ToString(ticketAttributesRs.Fields["CONTACT_PHONE"].Value);
                STATE = m_ticketCreation.State = Convert.ToString(ticketAttributesRs.Fields["STATE"].Value);
                COUNTY = m_ticketCreation.County = Convert.ToString(ticketAttributesRs.Fields["COUNTY"].Value);
                LATITUDE = m_ticketCreation.Latitude = Convert.ToDecimal(ticketAttributesRs.Fields["LATITUDE"].Value);
                LONGITUDE = m_ticketCreation.Longitude = Convert.ToDecimal(ticketAttributesRs.Fields["LONGITUDE"].Value);
                HOUSE_NUMBER = m_ticketCreation.HouseNumber = Convert.ToString(ticketAttributesRs.Fields["HOUSE_NUMBER"].Value);
                STREET_NAME = m_ticketCreation.StreetName = Convert.ToString(ticketAttributesRs.Fields["STREET_NAME"].Value);
                PRIORITY_CODE = m_ticketCreation.PriorityCode = Convert.ToString(ticketAttributesRs.Fields["PRIORITY_CODE"].Value);
                REMARKS = m_ticketCreation.Remarks = Convert.ToString(ticketAttributesRs.Fields["REMARKS"].Value);
                FileAttachmentType[] fileAttachmentType = new FileAttachmentType[2];
                fileAttachmentType[0] = new FileAttachmentType();
                PLOT = fileAttachmentType[0].Name = "Plot";
                fileAttachmentType[0].Content = Encoding.ASCII.GetBytes(Convert.ToString(ticketAttributesRs.Fields["PLOT"].Value));
                fileAttachmentType[1] = new FileAttachmentType();
                INVOICE = fileAttachmentType[1].Name = "Invoice";
                fileAttachmentType[1].Content = Encoding.ASCII.GetBytes(Convert.ToString(ticketAttributesRs.Fields["INVOICE"].Value));
                // JOB_TYPE = m_ticketCreation.JobType = Convert.ToString(ticketAttributesRs.Fields["JOB_TYPE"].Value);
                // NUMBER_OF_POLES = m_ticketCreation.NumberOfPoles = Convert.ToString(ticketAttributesRs.Fields["NUMBER_OF_POLES"].Value);
                //  DAYS_INTERVAL = m_ticketCreation.DaysInterval = Convert.ToString(ticketAttributesRs.Fields["DAYS_INTERVAL"].Value);

                ticketAttributesRs.Close();
                ticketAttributesRs = null;

                ticketAttributesRs = dataLayer.GetRecordSet(string.Format("SELECT * FROM GIS_ONC.NJUNS_STEP WHERE GIS_NJUNS_TICKET_ID = {0}", m_gisTicketId));
                if (ticketAttributesRs != null && ticketAttributesRs.RecordCount == 0)
                {
                    MessageBox.Show(m_interfacePoint + " : " + " Invalid ticket ID.", "Submit Ticket Error", MessageBoxButtons.OK);
                    return false;
                }
                ticketAttributesRs.MoveFirst();
                TicketStepType[] ticketStepType = new TicketStepType[ticketAttributesRs.RecordCount];
                int i = 0;
                while (!ticketAttributesRs.EOF)
                {
                    ticketStepType[i] = new TicketStepType
                    {
                        DaysInterval = Convert.ToString(ticketAttributesRs.Fields["DAYS_INTERVAL"].Value),
                        JobType = Convert.ToString(ticketAttributesRs.Fields["JOB_TYPE"].Value),
                        NjunsMemberCode = new MemberType { Value = Convert.ToString(ticketAttributesRs.Fields["NJUNS_MEMBER"].Value) },
                        NumberOfPoles = Convert.ToString(ticketAttributesRs.Fields["NUMBER_POLES"].Value),
                        Remarks = Convert.ToString(ticketAttributesRs.Fields["REMARKS"].Value),
                        //StepNumber = Convert.ToString(ticketAttributesRs.Fields["NJUNS_STEP_ID"].Value)
                    };
                    i = i + 1;
                    ticketAttributesRs.MoveNext();
                }

                m_ticketCreation.TicketSteps = ticketStepType;
                // m_ticketCreation.FileAttachments = fileAttachmentType;

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Method to Submit Tickets
        /// </summary>
        public void SubmitTicketByWr()
        {
            try
            {
                m_messageLogger = new SendReceiveMessageLogger();
                dataLayer.CopyPropertiesFrom(this);
                SubmitTickets(GetUnsubmittedTickets());
            }
            catch (Exception ex)
            {
                m_messageLogger.Log_Detail = ex.Message;
                dataLayer.LogErrorMessages(m_messageLogger, m_interfacePoint);
                if (Equals(Mode, 'I'))
                {
                    MessageBox.Show(m_interfacePoint + " : " + ex.Message, "Submit Ticket By WR Error", MessageBoxButtons.OK);
                }
            }
        }



        private string GetGuidForMemberCode(string NjunsMemberCode)
        {
            try
            {
                //Recordset guidRs=  dataLayer.GetRecordSet(string.Format("SELECT NJUNS_CODE_GUID  FROM NJUNS_MEMBER_COUNTY MC, NJUNS_MEMBER M WHERE MC.NJUNS_MEMBER_ID = M.NJUNS_MEMBER_ID AND M.NJUNS_MEMBER = {0}", NJUNS_MEMBER_CODE));
                Recordset guidRs = dataLayer.GetRecordSet(string.Format("SELECT NJUNS_CODE_GUID  FROM NJUNS_MEMBER_COUNTY MC WHERE MC.NJUNS_CODE = '{0}'", NJUNS_MEMBER_CODE));
                if (guidRs != null && guidRs.RecordCount > 0)
                {
                    guidRs.MoveFirst();
                    return Convert.ToString(guidRs.Fields["NJUNS_CODE_GUID"].Value);
                }
                return string.Empty;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Gets all Unsubmitted tickets of poles in a WR 
        /// </summary>
        /// <returns>List of UnSubmitted GIS Njuns Ticket Id</returns>
        private List<int> GetUnsubmittedTickets()
        {
            Recordset ticketRs = null;
            List<int> m_UnSubmittedTickets = null;
            try
            {
                //ticketRs = dataLayer.GetRecordSet(string.Format(@"SELECT NT.GIS_NJUNS_TICKET_ID FROM COMP_UNIT_N CU, NJUNS_TICKET_N NT WHERE CU.G3E_FID = NT.G3E_FID AND NT.NJUNS_TICKET_ID IS NULL AND CU.WR_ID = '{0}'", m_DataContext.ActiveJob));
                //ticketRs = dataLayer.GetRecordSet(string.Format(@"SELECT NT.GIS_NJUNS_TICKET_ID FROM  NJUNS_TICKET_N NT WHERE NT.NJUNS_TICKET_ID IS NULL AND NT.MISCELLANEOUS_ID = '{0}'", m_DataContext.ActiveJob));
                ticketRs = dataLayer.GetRecordSet(string.Format(@"SELECT NT.GIS_NJUNS_TICKET_ID FROM  NJUNS_TICKET NT WHERE MISCELLANEOUS_ID = '{0}' AND NT.NJUNS_TICKET_ID = -1 ", m_DataContext.ActiveJob));

                if (ticketRs != null && ticketRs.RecordCount > 0)
                {
                    ticketRs.MoveFirst();
                    m_UnSubmittedTickets = new List<int>();
                    while (!ticketRs.EOF)
                    {
                        m_UnSubmittedTickets.Add(Convert.ToInt32(ticketRs.Fields["GIS_NJUNS_TICKET_ID"].Value));
                        ticketRs.MoveNext();
                    }
                }
                return m_UnSubmittedTickets;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (ticketRs != null)
                {
                    ticketRs.Close();
                    ticketRs = null;
                }
            }

        }

        /// <summary>
        /// Method to call customSubmitTicket's SubmitTicket for each UnSubmitted Ticket
        /// </summary>
        /// <param name="gisTickets"></param>
        private void SubmitTickets(List<int> gisTickets)
        {
            try
            {
                if (gisTickets != null)
                {
                    foreach (int gisTicket in gisTickets)
                    {
                        SubmitTicket(gisTicket, Mode);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }



        /// <summary>
        /// Gets all Ticket numbers of poles in a WR 
        /// </summary>
        /// <returns>List of Ticket Numbers</returns>
        private List<string> GetTicketNumberForPoles(out List<string> njunsTicketsIDs )
        {
            Recordset polesRs = null;
            njunsTicketsIDs = null;
            List<string> ticketNumbers = null;
            try
            {
                polesRs = dataLayer.GetRecordSet(string.Format(@"SELECT TICKET_NUMBER , NJUNS_TICKET_ID FROM GIS_ONC.NJUNS_TICKET WHERE MISCELLANEOUS_ID = '{0}'", m_DataContext.ActiveJob));
                if (polesRs != null && polesRs.RecordCount > 0)
                {
                    polesRs.MoveFirst();
                    ticketNumbers = new List<string>();
                    njunsTicketsIDs = new List<string>();
                    while (!polesRs.EOF)
                    {
                        string ticketNumber = Convert.ToString(polesRs.Fields["TICKET_NUMBER"].Value);
                        string njunsTicketId = Convert.ToString(polesRs.Fields["NJUNS_TICKET_ID"].Value);
                        if (!string.IsNullOrEmpty(ticketNumber) && !string.IsNullOrEmpty(njunsTicketId) && !ticketNumber.Equals("-1") && !njunsTicketId.Equals("-1"))
                        {
                            ticketNumbers.Add(ticketNumber);
                            njunsTicketsIDs.Add(njunsTicketId);
                        }
                        polesRs.MoveNext();
                    }
                }
                return ticketNumbers;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to call customTicketStatus's CheckTicketStatus for each Ticket number
        /// </summary>
        /// <param name="ticketNumbers"></param>
        /// <returns>Key Value pair of Ticket Number and Ticket Status</returns>
        private Dictionary<int, string> GetStatusForTicketNumbers(List<string> ticketNumbers, List<string> njunsTicketsIds)
        {
            try
            {
                if (ticketNumbers != null)
                {
                    ticketStatusCollection = new Dictionary<int, string>();
                    //foreach (string ticketNumber in ticketNumbers)
                    //{
                    //    ticketStatusCollection.Add(Convert.ToInt32(ticketNumber), CheckTicketStatus(NJUNS_TICKET_ID, Mode));
                    //}

                    for (int i = 0; i < ticketNumbers.Count; i++)
                    {
                        ticketStatusCollection.Add(Convert.ToInt32(ticketNumbers[i]), CheckTicketStatus(njunsTicketsIds[i], 'B'));
                    }
                }
                return ticketStatusCollection;
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

    }
}
