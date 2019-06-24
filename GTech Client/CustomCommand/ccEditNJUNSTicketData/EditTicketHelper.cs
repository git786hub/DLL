//----------------------------------------------------------------------------+
//        Class: EditTicketHelper
//  Description: This class is used to help in performing edit ticket operation 
//
//----------------------------------------------------------------------------+
//     $Author:: hkonda                                   $
//       $Date:: 20/03/18                                 $
//   $Revision:: 1                                        $
//----------------------------------------------------------------------------+
//    $History:: EditTicketHelper.cs                     $
// 
// *****************  Version 1  *****************
// User: hkonda     Date: 20/03/18   Time: 18:00  Desc : Created
//----------------------------------------------------------------------------+
using ADODB;
using Intergraph.GTechnology.API;
using OncorTicketCreation;
using System;
using System.Collections.Generic;
using System.Text;

namespace GTechnology.Oncor.CustomAPI
{
    public class EditTicketHelper
    {
        internal IGTDataContext DataContext;
       
        ///// <summary>
        ///// Method to save ticket details
        ///// </summary>
        ///// <param name="ticket">Ticket object</param>
        ///// <returns>True- If Saving is successful.Else false</returns>
        //public bool SaveTicketAndStepDetails(Ticket ticket)
        //{
        //    try
        //    {
        //        StringBuilder query = new StringBuilder();
        //        query.AppendFormat(" BEGIN ");
        //        query.AppendFormat(@"UPDATE GIS_ONC.NJUNS_TICKET SET 
        //                                             TICKET_TYPE = '{0}',
        //                                             TICKET_STATUS ='{1}' ,
        //                                             POLE_FID = {2},
        //                                             POLE_NUMBER = '{3}',
        //                                             MISCELLANEOUS_ID = '{4}',
        //                                             NJUNS_MEMBER_CODE = '{5}',
        //                                             POLE_OWNER = '{6}',
        //                                             CONTACT_NAME = '{7}',
        //                                             CONTACT_PHONE = '{8}',
        //                                             STATE = '{9}',
        //                                             COUNTY ='{10}',
        //                                             PLACE = '{11}',
        //                                             LATITUDE = '{12}',
        //                                             LONGITUDE = '{13}',
        //                                             HOUSE_NUMBER = '{14}',
        //                                             STREET_NAME = '{15}',
        //                                             PRIORITY_CODE = '{16}',
        //                                             JOB_TYPE = '{17}',
        //                                             REMARKS = '{18}',
        //                                             NUMBER_OF_POLES = '{19}',
        //                                             DAYS_INTERVAL = {20} WHERE GIS_NJUNS_TICKET_ID = '{21}' ;",
        //                                             ticket.Type,
        //                                             ticket.Status,
        //                                             ticket.PoleFid,
        //                                             ticket.StructureId,
        //                                             ticket.WR,
        //                                             ticket.NJunsMemberCode,
        //                                             ticket.PoleOwner,
        //                                             ticket.ContactName,
        //                                             ticket.ContactPhone,
        //                                             ticket.State,
        //                                             ticket.County,
        //                                             ticket.Place,
        //                                             ticket.Latitude,
        //                                             ticket.Longitude,
        //                                             ticket.HouseNumber,
        //                                             ticket.StreetName,
        //                                             ticket.PriorityCode,
        //                                             ticket.JobType,
        //                                             ticket.Remarks,
        //                                             ticket.NumberOfPoles,
        //                                             ticket.DaysInterval,
        //                                             ticket.GisNujnsTicketId);

        //        Recordset tmpRs = GetRecordSet(string.Format("DELETE FROM GIS_ONC.NJUNS_STEP WHERE GIS_NJUNS_TICKET_ID = '{0}'", ticket.GisNujnsTicketId));
        //        foreach (TicketStepType step in ticket.TicketStepTypeList)
        //        {
        //            if (step == null)
        //            {
        //                continue;
        //            }
        //            query.AppendFormat(@"INSERT INTO GIS_ONC.NJUNS_STEP(
        //                                             GIS_NJUNS_TICKET_ID,
        //                                             NJUNS_TICKET_ID,
        //                                             JOB_TYPE,
        //                                             NUMBER_POLES,
        //                                             DAYS_INTERVAL,
        //                                             REMARKS,
        //                                             NJUNS_MEMBER,  
        //                                             NJUNS_STEP_ID
        //                                             )
        //                                             VALUES({0},{1},'{2}','{3}','{4}','{5}','{6}',{7});",
        //                                            ticket.GisNujnsTicketId,
        //                                            ticket.NjunsTicketId,
        //                                            step.JobType,
        //                                            step.NumberOfPoles,
        //                                            step.DaysInterval,
        //                                            step.Remarks,
        //                                            step.NjunsMemberCode.Value,
        //                                            step.ReferenceId
        //                                         );
        //        }
        //        query.AppendFormat("  COMMIT ; ");
        //        query.AppendFormat("  END ; ");
        //        DataContext.Execute(query.ToString(), out int iRecordsAffected, (int)CommandTypeEnum.adCmdText);
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        internal List<Tuple<int,int,string>> GetTicketIdsAndNumber(int poleFid)
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
        /// Returns recordset for the input query 
        /// </summary>
        /// <param name="sqlString">sql query</param>
        /// <returns>result recordset</returns>
        private Recordset GetRecordSet(string sqlString)
        {
            try
            {
                int outRecords = 0;
                Command command = new Command
                {
                    CommandText = sqlString
                };
                Recordset resultRs = DataContext.ExecuteCommand(command, out outRecords);
                return resultRs;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
