//----------------------------------------------------------------------------+
//        Class: TicketAttributes
//  Description: This class holds Ticket Attributes and necessary methods to Insert new Ticket record in NJUNS_TICKET table
//                                                                  
//----------------------------------------------------------------------------+
//     $Author:: hkonda                                   $
//       $Date:: 07/02/18                                 $
//   $Revision:: 1                                        $
//----------------------------------------------------------------------------+
//    $History:: TicketAttributes.cs                     $
// 
// *****************  Version 1  *****************
// User: hkonda     Date: 07/02/18   Time: 18:00  Desc : Created
//----------------------------------------------------------------------------+

using ADODB;
using Intergraph.GTechnology.API;
using System;
using System.Text;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public class TicketAttributes
    {
        internal IGTDataContext DataContext;

        public int GIS_NJUNS_TICKET_ID { get; set; }
        public int NJUNS_TICKET_ID { get; set; }
        public string TICKET_NUMBER { get; set; }
        public string TICKET_TYPE { get; set; }
        public string TICKET_STATUS { get; set; }
        public int POLE_FID { get; set; }
        public string POLE_NUMBER { get; set; }
        public string MISCELLANEOUS_ID { get; set; }
        public string CREATED_MEMBER { get; set; }
        public string POLE_OWNER { get; set; }
        public string START_DATE { get; set; }
        public string WORK_REQUESTED_DATE { get; set; }
        public string CONTACT_NAME { get; set; }
        public string CONTACT_PHONE { get; set; }
        public string STATE { get; set; }
        public string COUNTY { get; set; }
        public string PLACE { get; set; }
        public string LATITUDE { get; set; }
        public string LONGITUDE { get; set; }
        public string HOUSE_NUMBER { get; set; }
        public string STREET_NAME { get; set; }
        public string PRIORITY_CODE { get; set; }
        public string JOB_TYPE { get; set; }
        public string NUMBER_OF_POLES { get; set; }
        public string DAYS_INTERVAL { get; set; }
        public string REMARKS { get; set; }
        public string PLOT { get; set; }
        public string INVOICE { get; set; }

        public bool CreateNewTicket()
        {
            try
            {
                if (InsertNewTicketRecord())
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Custom Create Ticket Error", MessageBoxButtons.OK);
                return false;
            }
        }

        private bool InsertNewTicketRecord()
        {
            int iRecordsAffected = 0;
            try
            {
                StringBuilder insertQuery = new StringBuilder();
                insertQuery.AppendFormat(" BEGIN ");
                insertQuery.AppendFormat(@"INSERT INTO NJUNS_TICKET(
                                                     NJUNS_TICKET_ID,
                                                     TICKET_NUMBER,
                                                     TICKET_TYPE,
                                                     TICKET_STATUS,
                                                     POLE_FID,
                                                     POLE_NUMBER,
                                                     MISCELLANEOUS_ID,
                                                     CREATED_MEMBER,
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
                                                     DAYS_INTERVAL
                                                     )
                                                     VALUES({0},'{1}','{2}','{3}',{4},'{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}');", 
                                                     NJUNS_TICKET_ID,
                                                     TICKET_NUMBER,
                                                     TICKET_TYPE,
                                                     TICKET_STATUS,
                                                     POLE_FID,
                                                     POLE_NUMBER,
                                                     MISCELLANEOUS_ID,
                                                     CREATED_MEMBER,
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
                                                     DAYS_INTERVAL
                                                     );
                //insertQuery.AppendFormat(" COMMIT ; ");  // COMMIT AFTER SUCCESSFUL STEP CREATION
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
    }
}
