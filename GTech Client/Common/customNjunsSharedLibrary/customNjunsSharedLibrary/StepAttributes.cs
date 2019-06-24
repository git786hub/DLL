//----------------------------------------------------------------------------+
//        Class: StepAttributes
//  Description: This class holds Step Attributes and necessary methods to Insert new Step record in NJUNS_STEP table
//                                                                  
//----------------------------------------------------------------------------+
//     $Author:: hkonda                                   $
//       $Date:: 07/02/18                                 $
//   $Revision:: 1                                        $
//----------------------------------------------------------------------------+
//    $History:: StepAttributes.cs                     $
// 
// *****************  Version 1  *****************
// User: hkonda     Date: 07/02/18   Time: 18:00  Desc : Created
//----------------------------------------------------------------------------+

using System;
using System.Text;
using System.Windows.Forms;
using ADODB;
using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI
{
    public class StepAttributes
    {
        internal IGTDataContext DataContext;
        public int GIS_NJUNS_TICKET_ID { get; set; }
        public int NJUNS_TICKET_ID { get; set; }
        public int NJUNS_STEP_ID { get; set; }
        public string NJUNS_MEMBER { get; set; }
        public string JOB_TYPE { get; set; }
        public string NUMBER_POLES { get; set; }
        public string DAYS_INTERVAL { get; set; }
        public string REMARKS { get; set; }

        public bool InsertStepRecords()
        {
            int iRecordsAffected = 0;
            try
            {
                StringBuilder insertQuery = new StringBuilder();
                insertQuery.AppendFormat(" BEGIN ");
                insertQuery.AppendFormat(@"INSERT INTO NJUNS_STEP(
                                                     GIS_NJUNS_TICKET_ID,
                                                     NJUNS_TICKET_ID,
                                                     JOB_TYPE,
                                                     NUMBER_POLES,
                                                     DAYS_INTERVAL,
                                                     REMARKS
                                                     )
                                                     VALUES({0},{1},'{2}','{3}','{4}','{5}');",
                                                     GIS_NJUNS_TICKET_ID,
                                                     NJUNS_TICKET_ID,
                                                     JOB_TYPE,
                                                     NUMBER_POLES,
                                                     DAYS_INTERVAL,
                                                     REMARKS
                                                      );
                insertQuery.AppendFormat(" COMMIT ; ");
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

    }
}
