using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADODB;
using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI 
{
    public class NJUNSAutomationHelper : IGISAutoPlugin
    {
        private IGTApplication  gtApp = GTClassFactory.Create<IGTApplication>();
        private IGTDataContext gtDataContext;
        private IGTRelationshipService gtRelationshipService = GTClassFactory.Create<IGTRelationshipService>();
        private IGTTransactionManager transactionManager;
        private IGTJobManagementService jobManagement = GTClassFactory.Create<IGTJobManagementService>();
        public Recordset OpenRecords;
        public Recordset CloseRecords;

        public string SystemName => throw new NotImplementedException();

        public NJUNSAutomationHelper(IGTTransactionManager TransactionManager)
        {
            transactionManager = TransactionManager;
            gtDataContext = gtApp.DataContext;
        }

        public void Execute(GISAutoRequest autoRequest)
        {
            throw new NotImplementedException();
        }

        private void ProcessCommand()
        {
            string wrNumber = gtDataContext.ActiveJob;
            /*
            This is the section of code designed for after the NJUNS client stuff has been fully
            developed
            try{
                TicketCollection tickets = njunsHelper.CheckTicketStatus(wrNumber, "B");

                String openJoinTables = "SELECT * FROM COMMON_N INNER JOIN NJUNS_TICKET_N WHERE COMMON_N.G3E_FID = NJUNS_TICKET_N.G3E_FID AND NJUNS_TICKET_N.TICKET_STATUS = '?'";
                Recordset OpenRecords = gtDataContext.OpenRecordset(openJoinTables, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, -1, "OPEN");
                Recordset ClosedRecords = gtDataContext.OpenRecordset(openJoinTables, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, -1, "CLOSED");



            }catch(Exception e){
            }
             */

        }

        private void UpdatePoles()
        {
            transactionManager.Begin("Update Poles in LIP");
            string poleQuery = "SELECT * FROM POLE_N INNER JOIN NJUNS_TICKET_N WHERE POLE_N.G3E_FID = NJUNS_TICKET_N.G3E_FID WHERE POLE_N.LTT_STATUS = ?";
            Recordset LIPPoles = gtDataContext.OpenRecordset(poleQuery, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, -1, "LIP");
            if (!LIPPoles.BOF && !LIPPoles.EOF)
            {
                LIPPoles.MoveFirst();
                while (!LIPPoles.EOF)
                {
                    DeleteFeature(Convert.ToInt16(LIPPoles.Fields["G3E_FNO"].Value), Convert.ToInt32(LIPPoles.Fields["G3E_FID"].Value));
                    LIPPoles.MoveNext();
                }
            }
            transactionManager.Commit();

        }

        private bool DeleteFeature(short FNO, int FID)
        {
            try
            {
                IGTKeyObject removeObject = gtDataContext.OpenFeature(FNO, FID);
                Recordset deleteOrder = gtDataContext.MetadataRecordset("G3E_FEATURECOMPS_OPTABLE", "G3E_FNO = " + FNO);
                deleteOrder.MoveFirst();
                while (!deleteOrder.EOF && !deleteOrder.BOF)
                {
                    for (int i = 0; i < removeObject.Components.Count; i++)
                    {
                        if (removeObject.Components[i].CNO == Convert.ToInt16(deleteOrder.Fields["G3E_CNO"].Value))
                        {
                            if (!removeObject.Components[i].Recordset.EOF && !removeObject.Components[i].Recordset.BOF)
                            {
                                removeObject.Components[i].Recordset.MoveFirst();
                                removeObject.Components[i].Recordset.Delete();
                            }
                        }
                    }
                    deleteOrder.MoveNext();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
