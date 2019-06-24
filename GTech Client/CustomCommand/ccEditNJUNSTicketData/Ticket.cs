//----------------------------------------------------------------------------+
//        Class: Ticket
//  Description: This class is used to fetch all the ticket details based on local ticket Id and used to populate the form. 
//               The class variables are used a binding sources for the controls in the grid.
//
//----------------------------------------------------------------------------+
//     $Author:: hkonda                                   $
//       $Date:: 20/03/18                                 $
//   $Revision:: 1                                        $
//----------------------------------------------------------------------------+
//    $History:: Ticket.cs                     $
// 
// *****************  Version 1  *****************
// User: hkonda     Date: 20/03/18   Time: 18:00  Desc : Created
// User: hkonda     Date: 30/09/18   Time: 18:00  Desc : Code changes made to support latest schema changes
//----------------------------------------------------------------------------+

using ADODB;
using Intergraph.GTechnology.API;
using OncorTicketCreation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public class Ticket
    {
        private int m_PoleFid;
        private string m_NjunsTicketId;
        private int m_GisNjunsTicketId;
        private string m_PoleNumber;
        private string m_StructureId;
        private string m_Wr;
        private string m_Type;
        private string m_status;
        private string m_State;
        private string m_County;
        private string m_Place;
        private string m_HouseNumber;
        private string m_StreetName;
        private int m_TicketNumber;
        private string m_NjunsMemberCode;
        private string m_PoleOwner;
        private string m_PriorityCode;
        private string m_PlotFilePath;
        private string m_InvoiceFilePath;
        private string m_ContactName;
        private string m_ContactPhone;
        private decimal m_Latitude;
        private decimal m_Longitude;
        private string m_Remarks;
        private string m_DaysInterval;
        private string m_NumberOfPoles;
        private string m_JobType;
        private DateTime m_StartDate;
        private DateTime m_WorkRequestedDate;

        private List<string> m_NjunsMemberCodeList;
        private List<string> m_PoleOwnerList;
        private List<string> m_PriorityList;

        private const string m_InterfacePoint = "ccEditNjunsTicket";

        public IGTDataContext DataContext;
        

        List<TicketStepType> m_ticketStepTypeList = null;

        public string POLE_OWNER
        {
            get { return m_PoleOwner; }
            set { m_PoleOwner = value; }
        }
        public string JOB_TYPE
        {
            get { return m_JobType; }
            set { m_JobType = value; }
        }
        public string NJUNS_MEMBER_CODE
        {
            get { return m_NjunsMemberCode; }
            set { m_NjunsMemberCode = value; }
        }
        public string POLE_NUMBER
        {
            get { return m_PoleNumber; }
            set { m_PoleNumber = value; }
        }
        public string MISCELLANEOUS_ID
        {
            get { return m_Wr; }
            set { m_Wr = value; }
        }
        public string TICKET_TYPE
        {
            get { return m_Type; }
            set { m_Type = value; }
        }
        public string TICKET_STATUS
        {
            get { return m_status; }
            set { m_status = value; }
        }
        public System.DateTime START_DATE
        {
            get { return m_StartDate; }
            set { m_StartDate = value; }
        }
        public DateTime WORK_REQUESTED_DATE
        {
            get { return m_WorkRequestedDate; }
            set { m_WorkRequestedDate = value; }
        }
        public string STATE
        {
            get { return m_State; }
            set { m_State = value; }
        }
        public string COUNTY
        {
            get { return m_County; }
            set { m_County = value; }
        }
        public string PLACE
        {
            get { return m_Place; }
            set { m_Place = value; }
        }
        public string HOUSE_NUMBER
        {
            get { return m_HouseNumber; }
            set { m_HouseNumber = value; }
        }
        public string STREET_NAME
        {
            get { return m_StreetName; }
            set { m_StreetName = value; }
        }
        public List<string> PriorityList
        {
            get { return new List<string> { "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT"," NINE", "TEN" }; }
            set { m_PriorityList = value; }
        }
        public string NJUNS_TICKET_ID
        {
            get { return m_NjunsTicketId; }
            set { m_NjunsTicketId = value; }
        }
        public int TICKET_NUMBER
        {
            get { return m_TicketNumber; }
            set { m_TicketNumber = value; }
        }
        public int POLE_FID
        {
            get { return m_PoleFid; }
            set { m_PoleFid = value; }
        }
        public int GIS_NJUNS_TICKET_ID
        {
            get { return m_GisNjunsTicketId; }
            set { m_GisNjunsTicketId = value; }
        }
        public string INVOICE
        {
            get { return m_InvoiceFilePath; }
            set { m_InvoiceFilePath = value; }
        }
        public string PLOT
        {
            get { return m_PlotFilePath; }
            set { m_PlotFilePath = value; }
        }
        public string PRIORITY_CODE
        {
            get { return m_PriorityCode; }
            set { m_PriorityCode = value; }
        }
        public string CONTACT_PHONE
        {
            get { return m_ContactPhone; }
            set { m_ContactPhone = value; }
        }
        public string CONTACT_NAME
        {
            get { return m_ContactName; }
            set { m_ContactName = value; }
        }
        public string NUMBER_OF_POLES
        {
            get { return m_NumberOfPoles; }
            set { m_NumberOfPoles = value; }
        }
        public decimal LATITUDE
        {
            get { return m_Latitude; }
            set { m_Latitude = value; }
        }

        public decimal LONGITUDE
        {
            get { return m_Longitude; }
            set { m_Longitude = value; }
        }
        public string DAYS_INTERVAL
        {
            get { return m_DaysInterval; }
            set { m_DaysInterval = value; }
        }
        public string REMARKS
        {
            get { return m_Remarks; }
            set { m_Remarks = value; }
        }
        public List<string> NJUNSMemberCodeList
        {
            get { return GetMemberCodes(); }
            set { m_NjunsMemberCodeList = value; }
        }
        public List<string> PoleOwnerList
        {
            get { return GetPoleOwnerList(); }
            set { m_PoleOwnerList = value; }
        }
        public List<TicketStepType> TicketStepTypeList
        {
            get { return m_ticketStepTypeList; }
            set { m_ticketStepTypeList = value; }
        }

        /// <summary>
        /// Method to read the Ticket attributes for the input GIS ticket Id 
        /// </summary>
        /// <param name="gisTicketId">Njuns Ticket ID</param>
        /// <param name="ticketNumber">ticketNumber</param>
        /// <param name="ticketId">Njuns Ticket ID</param>
        /// <returns>true, if ticket attributes exists in GIS. Else returns false.</returns>
        internal bool GetTicketAttributes(int gisTicketId, int ticketNumber = 0, string nJunsTicketId = "")
        {
            List<string> memberCodeList;
            List<string> poleOwnerList;
            try
            {
                string sql = string.Empty;
                if (ticketNumber > 0 && !string.IsNullOrEmpty(nJunsTicketId))
                {
                    sql = string.Format("SELECT * FROM GIS_ONC.NJUNS_TICKET WHERE TICKET_NUMBER = {0} AND NJUNS_TICKET_ID = '{1}'", ticketNumber, nJunsTicketId);
                }
                else
                {
                    sql = string.Format("SELECT * FROM GIS_ONC.NJUNS_TICKET WHERE GIS_NJUNS_TICKET_ID = {0}", gisTicketId);
                }
                Recordset ticketAttributesRs = GetRecordSet(sql);
                if (ticketAttributesRs != null && ticketAttributesRs.RecordCount == 0)
                {
                    MessageBox.Show(m_InterfacePoint + " : " + " Invalid ticket ID.", "Submit Ticket Error", MessageBoxButtons.OK);
                    return false;
                }

                m_Type = Convert.ToString(ticketAttributesRs.Fields["TICKET_TYPE"].Value);
                m_StructureId = Convert.ToString(ticketAttributesRs.Fields["POLE_NUMBER"].Value);
                m_Wr = Convert.ToString(ticketAttributesRs.Fields["MISCELLANEOUS_ID"].Value);
                m_NjunsMemberCode = Convert.ToString(ticketAttributesRs.Fields["NJUNS_MEMBER_CODE"].Value);
                m_PoleOwner = Convert.ToString(ticketAttributesRs.Fields["POLE_OWNER"].Value);
                m_PoleNumber = m_StructureId;
                m_status = Convert.ToString(ticketAttributesRs.Fields["TICKET_STATUS"].Value);
                m_StartDate = Convert.ToDateTime(ticketAttributesRs.Fields["START_DATE"].Value);
                m_WorkRequestedDate = Convert.ToDateTime(ticketAttributesRs.Fields["WORK_REQUESTED_DATE"].Value);
                m_State = Convert.ToString(ticketAttributesRs.Fields["STATE"].Value);
                m_County = Convert.ToString(ticketAttributesRs.Fields["COUNTY"].Value);
                m_Place = Convert.ToString(ticketAttributesRs.Fields["PLACE"].Value);
                m_HouseNumber = Convert.ToString(ticketAttributesRs.Fields["HOUSE_NUMBER"].Value);
                m_StreetName = Convert.ToString(ticketAttributesRs.Fields["STREET_NAME"].Value);
                m_PriorityCode = Convert.ToString(ticketAttributesRs.Fields["PRIORITY_CODE"].Value);

                m_PlotFilePath = Convert.ToString(ticketAttributesRs.Fields["PLOT"].Value);
                m_InvoiceFilePath = Convert.ToString(ticketAttributesRs.Fields["INVOICE"].Value);

                m_JobType = Convert.ToString(ticketAttributesRs.Fields["JOB_TYPE"].Value);
                m_NumberOfPoles = Convert.ToString(ticketAttributesRs.Fields["NUMBER_OF_POLES"].Value);
                m_DaysInterval = Convert.ToString(ticketAttributesRs.Fields["DAYS_INTERVAL"].Value);
                m_Remarks = Convert.ToString(ticketAttributesRs.Fields["REMARKS"].Value);
                ticketAttributesRs.Close();
                ticketAttributesRs = null;
                BuildLists(out memberCodeList, out poleOwnerList);
                ticketAttributesRs = GetRecordSet(string.Format("SELECT * FROM GIS_ONC.NJUNS_STEP WHERE GIS_NJUNS_TICKET_ID = {0}", gisTicketId));
                if (ticketAttributesRs != null && ticketAttributesRs.RecordCount == 0)
                {
                    MessageBox.Show(m_InterfacePoint + " : " + " Invalid ticket ID.", "Submit Ticket Error", MessageBoxButtons.OK);
                    return false;
                }
                ticketAttributesRs.MoveFirst();
                TicketStepType[] ticketStepType = new TicketStepType[ticketAttributesRs.RecordCount];

                int i = 0;
                while (!ticketAttributesRs.EOF)
                {
                    ticketStepType[i] = new TicketStepType();
                    ticketStepType[i].DaysInterval = Convert.ToString(ticketAttributesRs.Fields["DAYS_INTERVAL"].Value);
                    ticketStepType[i].JobType = Convert.ToString(ticketAttributesRs.Fields["JOB_TYPE"].Value);
                    MemberType mType = new MemberType();
                    mType.Value = Convert.ToString(ticketAttributesRs.Fields["NJUNS_MEMBER"].Value);
                    Recordset njunsGuidRs = GetRecordSet(string.Format("SELECT NJUNS_CODE_GUID  FROM NJUNS_MEMBER_COUNTY MC, NJUNS_MEMBER M WHERE MC.NJUNS_MEMBER_ID = M.NJUNS_MEMBER_ID AND M.NJUNS_MEMBER = '{0}'", mType.Value));
                    if (njunsGuidRs != null && njunsGuidRs.RecordCount > 0)
                    {
                        njunsGuidRs.MoveFirst();
                        mType.Id = Convert.ToString(njunsGuidRs.Fields["NJUNS_CODE_GUID"].Value);
                    }
                    ticketStepType[i].NjunsMemberCode = mType;
                    ticketStepType[i].CustomNjunsMemberValue = mType.Value;
                    ticketStepType[i].NumberOfPoles = Convert.ToString(ticketAttributesRs.Fields["NUMBER_POLES"].Value);
                    ticketStepType[i].Remarks = Convert.ToString(ticketAttributesRs.Fields["REMARKS"].Value);
                    ticketStepType[i].ReferenceId = Convert.ToString(i + 1);
                    i = i + 1;
                    ticketAttributesRs.MoveNext();

                }
                m_ticketStepTypeList = ticketStepType.ToList();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Method to build member code and pole owner list
        /// </summary>
        /// <param name="memberCodeList"></param>
        /// <param name="poleOwnerList"></param>
        private void BuildLists(out List<string> memberCodeList, out List<string> poleOwnerList)
        {
            memberCodeList = NJUNSMemberCodeList;
            poleOwnerList = PoleOwnerList;
        }

        /// <summary>
        /// Method to get Ticket and Ticket number
        /// </summary>
        /// <param name="poleFid">FID of the pole</param>
        /// <returns>List of GIS Ticket ID, TIcket number and NJUNS Ticket ID</returns>
        internal List<Tuple<int, int, string>> GetTicketIdsAndNumber(int poleFid)
        {
            List<Tuple<int, int, string>> TicketsForPole = null;
            try
            {
                Recordset tmpsRs = GetRecordSet(string.Format("SELECT GIS_NJUNS_TICKET_ID, TICKET_NUMBER, NJUNS_TICKET_ID FROM GIS_ONC.NJUNS_TICKET WHERE POLE_FID = {0}", poleFid));
                if (tmpsRs != null && tmpsRs.RecordCount > 0)
                {
                    TicketsForPole = new List<Tuple<int, int, string>>();
                    tmpsRs.MoveFirst();
                    while (!tmpsRs.EOF)
                    {
                        TicketsForPole.Add(new Tuple<int, int, string>(Convert.ToInt32(tmpsRs.Fields["GIS_NJUNS_TICKET_ID"].Value), Convert.ToInt32(tmpsRs.Fields["TICKET_NUMBER"].Value), Convert.ToString(tmpsRs.Fields["NJUNS_TICKET_ID"].Value)));
                        tmpsRs.MoveNext();
                    }
                }
                return TicketsForPole;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Gets the pole owner list
        /// </summary>
        /// <returns>List of pole owners</returns>
        private List<string> GetPoleOwnerList()
        {
            try
            {
                List<string> ownersList = new List<string>();
                IGTKeyObject gTKeyObject = DataContext.OpenFeature(110, POLE_FID);
                Recordset rs = gTKeyObject.Components.GetComponent(11001).Recordset;
                rs.MoveFirst();
                while (!rs.EOF)
                {
                    ownersList.Add(Convert.ToString(rs.Fields["OWNING_COMPANY_C"].Value));
                    rs.MoveNext();
                }
                return ownersList;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Gets the list of member codes
        /// </summary>
        /// <returns></returns>
        private List<string> GetMemberCodes()
        {
            try
            {
                List<string> codesList = new List<string>();
                IGTKeyObject gTKeyObject = DataContext.OpenFeature(110, POLE_FID);
                Recordset rs = gTKeyObject.Components.GetComponent(1).Recordset;

                IGTGeometry geometry = gTKeyObject.Components.GetComponent(11002).Geometry;
                IGTPoint point = GTClassFactory.Create<IGTPoint>();
                point.X = geometry.FirstPoint.X;
                point.Y = geometry.FirstPoint.Y;
                point.Z = geometry.FirstPoint.Z;
                customBoundaryQuery boundaryQuery = new customBoundaryQuery(point, 235);// County boundary  FNO 235
                Recordset resultRs1 = boundaryQuery.PerformPointInPolygon();

                if (resultRs1 != null && resultRs1.RecordCount > 0)
                {
                    IGTKeyObject countyFeature = DataContext.OpenFeature(Convert.ToInt16(resultRs1.Fields["G3E_FNO"].Value), Convert.ToInt32(resultRs1.Fields["G3E_FID"].Value));
                    Recordset boundaryAttributesRs = countyFeature.Components.GetComponent(23501).Recordset; // // County Boundary Attributes
                    if (boundaryAttributesRs != null && boundaryAttributesRs.RecordCount > 0)
                    {
                        boundaryAttributesRs.MoveFirst();
                        if (!string.IsNullOrEmpty(Convert.ToString(boundaryAttributesRs.Fields["ID"].Value)))
                        {
                            string countyBoundaryId = Convert.ToString(boundaryAttributesRs.Fields["ID"].Value).Replace("TX", string.Empty).Trim();
                            Recordset memberCodeRS = GetRecordSet(string.Format(@"SELECT NJUNS_MEMBER FROM NJUNS_MEMBER A , NJUNS_CONTACTS B WHERE A.NJUNS_MEMBER_ID = B.NJUNS_MEMBER_ID AND
                                                                        B.COUNTY_N_ID = {0} ", countyBoundaryId));

                            if (memberCodeRS != null && memberCodeRS.RecordCount > 0)
                            {
                                memberCodeRS.MoveFirst();
                                while (!memberCodeRS.EOF)
                                {
                                    codesList.Add(Convert.ToString(memberCodeRS.Fields["NJUNS_MEMBER"].Value));
                                    memberCodeRS.MoveNext();
                                }
                                memberCodeRS.Close();
                                memberCodeRS = null;
                            }
                        }
                    }
                }
                return codesList;
            }
            catch (Exception)
            {

                throw;
            }

        }


        /// <summary>
        /// Returns recordset for the input query 
        /// </summary>
        /// <param name="sqlString">sql query</param>
        /// <returns>result recordset</returns>
        private Recordset GetRecordSet(string sqlString)
        {
            try
            {
                int outRecords = 0;
                ADODB.Command command = new ADODB.Command();
                command.CommandText = sqlString;
                ADODB.Recordset resultRs = DataContext.ExecuteCommand(command, out outRecords);
                return resultRs;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
