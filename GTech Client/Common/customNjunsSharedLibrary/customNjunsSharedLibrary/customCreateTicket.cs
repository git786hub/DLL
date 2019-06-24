//----------------------------------------------------------------------------+
//        Class: customCreateTicket
//  Description: This custom dll exposes method to create a ticket in GIS database
//                                                                  
//----------------------------------------------------------------------------+
//     $Author:: hkonda                                   $
//       $Date:: 07/02/18                                 $
//   $Revision:: 1                                        $
//----------------------------------------------------------------------------+
//    $History:: customCreateTicket.cs                     $
// 
// *****************  Version 1  *****************
// User: hkonda     Date: 07/02/18   Time: 18:00  Desc : Created
//----------------------------------------------------------------------------+
using System;
using System.Collections.Generic;
using System.Linq;
using Intergraph.GTechnology.API;
using ADODB;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public class customCreateTicket
    {
        const int m_DistanceFromPoleInFeet = 300;
        string m_WrNumber;
        int m_PoleFid;
        int m_GisTicketId = 0;
        IGTDataContext m_DataContext;

        /// <summary>
        /// Constructor for customCreateTicket
        /// </summary>
        /// <param name="wrNumber">Work Request number</param>
        /// <param name="poleFid">Pole Feature Identifier</param>
        public customCreateTicket(string wrNumber, int poleFid)
        {
            this.m_WrNumber = wrNumber;
            this.m_PoleFid = poleFid;
            m_DataContext = GTClassFactory.Create<IGTApplication>().DataContext;
        }

        #region Public Methods
        /// <summary>
        /// Method to Create Ticket
        /// </summary>
        /// <returns></returns>
        public bool CreateTicket()
        {
            try
            {
                if (!CheckForJUAttachments())
                {
                    return false;
                }
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
                    m_TicketAttributes.DataContext = m_DataContext;
                    return m_TicketAttributes.CreateNewTicket();
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
                m_TicketAttributes = new TicketAttributes();
                IGTKeyObject gTKeyObject = m_DataContext.OpenFeature(110, m_PoleFid);
                Recordset rs = gTKeyObject.Components.GetComponent(1).Recordset;
                if (rs != null && rs.RecordCount > 0)
                {
                    rs.MoveFirst();
                    m_TicketAttributes.POLE_FID = m_PoleFid;
                    m_TicketAttributes.POLE_NUMBER = Convert.ToString(rs.Fields["STRUCTURE_ID"].Value);
                    m_TicketAttributes.MISCELLANEOUS_ID = m_WrNumber;
                    m_TicketAttributes.LATITUDE = Convert.ToString(rs.Fields["LATITUDE"].Value);
                    m_TicketAttributes.LONGITUDE = Convert.ToString(rs.Fields["LONGITUDE"].Value);
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
                        m_TicketAttributes.COUNTY = Convert.ToString(boundaryAttributesRs.Fields["NAME"].Value);
                        if (!string.IsNullOrEmpty(Convert.ToString(boundaryAttributesRs.Fields["ID"].Value)))
                        {
                            countyBoundaryId = Convert.ToString(boundaryAttributesRs.Fields["ID"].Value).Replace("TX", string.Empty).Trim();

                        }
                    }
                }
                if (!string.IsNullOrEmpty(countyBoundaryId))
                {
                    int nJunsMemberId = 0;

                    Recordset contactRs = GetRecordSet(string.Format("SELECT NJUNS_MEMBER_ID,CONTACT_NAME,CONTACT_PHONE FROM NJUNS_CONTACTS WHERE COUNTY_N_ID = {0}", countyBoundaryId));
                    if (contactRs != null && contactRs.RecordCount > 0)
                    {
                        contactRs.MoveFirst();
                        m_TicketAttributes.CONTACT_NAME = Convert.ToString(contactRs.Fields["CONTACT_NAME"].Value);
                        m_TicketAttributes.CONTACT_PHONE = Convert.ToString(contactRs.Fields["CONTACT_PHONE"].Value);
                        // m_TicketAttributes.PLACE = Convert.ToString(contactRs.Fields["NJUNS_PLACE_NAME"].Value);
                        nJunsMemberId = Convert.ToInt32(contactRs.Fields["NJUNS_MEMBER_ID"].Value);
                        contactRs.Close();
                        contactRs = null;
                    }
                    contactRs = GetRecordSet(string.Format("SELECT NJUNS_MEMBER FROM NJUNS_MEMBER WHERE NJUNS_MEMBER_ID = {0}", nJunsMemberId));
                    if (contactRs != null && contactRs.RecordCount > 0)
                    {
                        contactRs.MoveFirst();
                        m_TicketAttributes.CREATED_MEMBER = Convert.ToString(contactRs.Fields["NJUNS_MEMBER"].Value);
                        m_TicketAttributes.POLE_OWNER = Convert.ToString(contactRs.Fields["NJUNS_MEMBER"].Value);
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
                        m_TicketAttributes.HOUSE_NUMBER = Convert.ToString(premiseAttributesRs.Fields["HOUSE_NBR"].Value);
                        m_TicketAttributes.STREET_NAME = Convert.ToString(premiseAttributesRs.Fields["DIR_LEADING_C"].Value) + " "
                            + Convert.ToString(premiseAttributesRs.Fields["STREET_NM"].Value) + " "
                            + Convert.ToString(premiseAttributesRs.Fields["STREET_TYPE_C"].Value) + " "
                            + Convert.ToString(premiseAttributesRs.Fields["DIR_TRAILING_C"].Value);
                    }
                }

                // Set default vaues
                m_TicketAttributes.TICKET_NUMBER = string.Empty;
                m_TicketAttributes.TICKET_TYPE = "Transfer";
                m_TicketAttributes.TICKET_STATUS = "OPEN";
                m_TicketAttributes.STATE = "TX";
                m_TicketAttributes.PRIORITY_CODE = "3";
                m_TicketAttributes.NJUNS_TICKET_ID = -1;
                m_TicketAttributes.DAYS_INTERVAL = "30";
                m_TicketAttributes.JOB_TYPE = "Transfer";
                m_TicketAttributes.NUMBER_OF_POLES = "1";
                m_TicketAttributes.REMARKS = string.Empty;
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
                resultRs = GetRecordSet(string.Format("SELECT GIS_NJUNS_TICKET_ID FROM NJUNS_TICKET WHERE POLE_FID = {0}", m_TicketAttributes.POLE_FID));
                if (resultRs != null && resultRs.RecordCount > 0)
                {
                    resultRs.MoveFirst();
                    m_GisTicketId = Convert.ToInt32(resultRs.Fields["GIS_NJUNS_TICKET_ID"].Value);
                }
                CreateInitialStep();
                CreateStepsForEachAttachment();
                CreateFinalStep();

                return true;
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

        /// <summary>
        /// Method to Create Steps for Each Attachement on the pole
        /// </summary>
        private void CreateStepsForEachAttachment()
        {
            try
            {
                IGTKeyObject gTKeyObject = m_DataContext.OpenFeature(110, m_PoleFid);
                Recordset wireLineAttachmentRs = gTKeyObject.Components.GetComponent(34).Recordset;
                if (wireLineAttachmentRs != null && wireLineAttachmentRs.RecordCount > 0)
                {
                    wireLineAttachmentRs.MoveFirst();
                    while (!wireLineAttachmentRs.EOF)
                    {
                        InsertStep("");
                        wireLineAttachmentRs.MoveNext();
                    }
                }
                Recordset equipmentAttachmentRs = gTKeyObject.Components.GetComponent(35).Recordset;
                if (equipmentAttachmentRs != null && equipmentAttachmentRs.RecordCount > 0)
                {
                    equipmentAttachmentRs.MoveFirst();
                    while (!equipmentAttachmentRs.EOF)
                    {
                        InsertStep("");
                        equipmentAttachmentRs.MoveNext();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to Create Initial step
        /// </summary>
        private void CreateInitialStep()
        {
            InsertStep("Initial work");
        }

        /// <summary>
        /// Method to Create Final step
        /// </summary>
        private void CreateFinalStep()
        {
            InsertStep("Final work");
        }

        /// <summary>
        /// Method to Insert Step record in table
        /// </summary>
        /// <param name="remarks"></param>
        /// <returns></returns>
        private bool InsertStep(string remarks)
        {
            try
            {
                oStep = new StepAttributes();
                oStep.DataContext = m_DataContext;
                oStep.GIS_NJUNS_TICKET_ID = m_GisTicketId;
                oStep.NJUNS_TICKET_ID = -1;
                oStep.NJUNS_MEMBER = m_TicketAttributes.CREATED_MEMBER;
                oStep.JOB_TYPE = "Transfer";
                oStep.NUMBER_POLES = "1";
                oStep.DAYS_INTERVAL = "30";
                oStep.REMARKS = remarks;
                oStep.InsertStepRecords();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Common Methods

        /// <summary>
        /// Method to check whether any Joint Use attachments exists on a pole
        /// </summary>
        /// <returns>True, if JU attachemnts exists. Else returns false, with a message to User</returns>
        private bool CheckForJUAttachments()
        {
            IGTKeyObject gTKeyObject = null;
            try
            {
                gTKeyObject = m_DataContext.OpenFeature(110, m_PoleFid);
                Recordset wireLineAttachmentRs = gTKeyObject.Components.GetComponent(34).Recordset;
                Recordset equipmentAttachmentRs = gTKeyObject.Components.GetComponent(35).Recordset;
                if (wireLineAttachmentRs == null || equipmentAttachmentRs == null || (wireLineAttachmentRs != null && wireLineAttachmentRs.RecordCount == 0) || (equipmentAttachmentRs != null && equipmentAttachmentRs.RecordCount == 0))
                {
                    MessageBox.Show("Specified Pole has no Joint Use attachments; cannot create ticket.", "Custom Create Ticket Error", MessageBoxButtons.OK);
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

        #endregion
    }
}

