//----------------------------------------------------------------------------+
//        Class: ccEditNJUNSTicketData
//  Description: This modeless customcommand is used to edit and submit the ticket for a pole, which is not submitted to NJUNS already or, view a ticket in read-only mode that is associated with a pole.
//
//----------------------------------------------------------------------------+
//     $Author:: hkonda                                   $
//       $Date:: 20/03/18                                 $
//   $Revision:: 1                                        $
//----------------------------------------------------------------------------+
//    $History:: ccEditNJUNSTicketData.cs                     $
// 
// *****************  Version 1  *****************
// User: hkonda     Date: 20/03/18   Time: 18:00  Desc : Created
// User: hkonda     Date: 30/09/18   Time: 18:00  Desc : Code changes made to support latest schema changes
//----------------------------------------------------------------------------+
using System;
using System.Windows.Forms;
using Intergraph.GTechnology.Interfaces;
using Intergraph.GTechnology.API;
using System.Collections.Generic;

namespace GTechnology.Oncor.CustomAPI
{
    public class ccEditNJUNSTicketData : IGTCustomCommandModal
    {
        private int m_poleFid;

        IGTTransactionManager m_oGTTransactionManager = null;
        List<IGTDDCKeyObject> m_selectedObjects = new List<IGTDDCKeyObject>();
        IGTApplication m_iGtApplication = GTClassFactory.Create<IGTApplication>();
        frmEditNjunsTicket objFrmEditNjunsTicket = null;
        IGTDDCKeyObject m_originalObject;
        IGTDDCKeyObjects m_ooddcKeyObjects = null;
        public bool CanTerminate { get => true; }

        public IGTTransactionManager TransactionManager
        {
            get => m_oGTTransactionManager;
            set => m_oGTTransactionManager = value;
        }

        public void Activate()
        {
            try
            {
                IGTDDCKeyObjects selectedObjects = GTClassFactory.Create<IGTDDCKeyObjects>();
                List<int> fidList = new List<int>();
                m_ooddcKeyObjects = m_iGtApplication.SelectedObjects.GetObjects();
                foreach (IGTDDCKeyObject ddcKeyObject in m_ooddcKeyObjects)
                {
                    if (!fidList.Contains(ddcKeyObject.FID))
                    {
                        fidList.Add(ddcKeyObject.FID);
                        selectedObjects.Add(ddcKeyObject);
                    }
                }
                m_originalObject = selectedObjects[0];
                if (!ValidatePoleFeature())
                {
                    MessageBox.Show("This command applies only to pole feature.");
                    return;
                }
                CheckForTicketsAndShowForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during execution of Edit NJUNS ticket custom command." + Environment.NewLine + "Transition failed for selected features." + Environment.NewLine + ex.Message, "G/Techonology", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Method to check for local tickets and show the UI.
        /// If there are no local tickets for the selected pole, message is shown to the user stating "no local tickets found for the pole"
        /// If a single ticket exists then Edit NJUNS Ticket UI is shown in eother read-only or read-write per business rules
        /// If more than a single ticket exits for a pole, additional form is shown to user that lists all the available local tickets. User can click select a ticket from the list to see the ticket details.
        /// </summary>
        private void CheckForTicketsAndShowForm()
        {
            Ticket ticket = new Ticket();
            try
            {
                if (objFrmEditNjunsTicket == null)
                {
                    objFrmEditNjunsTicket = new frmEditNjunsTicket()
                    {
                        DataContext = m_iGtApplication.DataContext,
                        PoleFid = m_poleFid
                    };
                }

                ticket.DataContext = m_iGtApplication.DataContext;
                objFrmEditNjunsTicket.objTicket = ticket;
                objFrmEditNjunsTicket.PoleFid = m_poleFid;

                short ticketCount = objFrmEditNjunsTicket.GetTicketCount();
                if (ticketCount == 0)
                {
                    MessageBox.Show(m_iGtApplication.ApplicationWindow, "No NJUNS tickets were found for this pole.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    objFrmEditNjunsTicket.Dispose();
                    return;
                }

                else if (ticketCount == 1)
                {
                    ticket.GIS_NJUNS_TICKET_ID = objFrmEditNjunsTicket.ticketsForPole[0].Item1;
                    ticket.TICKET_NUMBER = objFrmEditNjunsTicket.ticketsForPole[0].Item2;
                    ticket.NJUNS_TICKET_ID = objFrmEditNjunsTicket.ticketsForPole[0].Item3;
                }

                else if (ticketCount > 1)
                {
                    // Show the GIS tickets in a grid 
                    poleTicketsForm poleTicketsForm = new poleTicketsForm(objFrmEditNjunsTicket.GetLocalTickets());
                    if (poleTicketsForm.ShowDialog(m_iGtApplication.ApplicationWindow) == DialogResult.OK)
                    {
                        foreach (Tuple<int, int, string> item in objFrmEditNjunsTicket.ticketsForPole)
                        {
                            if (item.Item1 == poleTicketsForm.selectedTicket)
                            {
                                ticket.GIS_NJUNS_TICKET_ID = item.Item1;
                                ticket.TICKET_NUMBER = item.Item2;
                                ticket.NJUNS_TICKET_ID = item.Item3;
                                break;
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                }

                ticket.POLE_FID = m_poleFid;
                objFrmEditNjunsTicket.objTicket = ticket;
                objFrmEditNjunsTicket.ShowDialog(m_iGtApplication.ApplicationWindow);

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to check whether or not the selected feature is a pole
        /// </summary>
        /// <returns>true, if feature is a pole. else false</returns>
        private bool ValidatePoleFeature()
        {
            try
            {
                m_poleFid = m_originalObject.FID;
                return m_originalObject.FNO == 110;  // return true when selected feature is a pole
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Pause()
        {
        }

        public void Resume()
        {
        }

        public void Terminate()
        {
            m_oGTTransactionManager = null;
        }
    }
}
