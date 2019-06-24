// =====================================================================================================================================================================
//  File Name: ccSynchronizeWP.cs
// 
// Description:  Command to synchronize work points
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  03/25/2019          Shubham                       Initial implementation
//=====================================================================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public class SynchronizeWP : IGTCustomCommandModal
    {
        private IGTTransactionManager m_oTransactionManager;
        public IGTTransactionManager TransactionManager { set => m_oTransactionManager = value; }

        private List<int> m_lstStructuresFNOs = new List<int> { 106, 107, 108, 109, 110, 113, 114, 115, 116, 117, 120, 2500 };
        List<int> lstSelectedStrcuture = new List<int>();
        IGTApplication m_oApp = GTClassFactory.Create<IGTApplication>();

        private bool ValidateJobType()
        {
           bool bReturn = true;
           string sql = "select G3E_JOBTYPE from G3E_JOB where G3E_IDENTIFIER=?";

          ADODB.Recordset  rsValidate = m_oApp.DataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly,
                    (int)ADODB.CommandTypeEnum.adCmdText, m_oApp.DataContext.ActiveJob);

            if (rsValidate.RecordCount > 0)
            {
                rsValidate.MoveFirst();
                if (!rsValidate.EOF && !rsValidate.BOF)
                {
                   string m_strJobtype = Convert.ToString(rsValidate.Fields[0].Value);     
                    if (m_strJobtype.Equals("NON-WR"))
                    {
                        bReturn = false;
                    }
                }
            }

            return bReturn;
        }
        public void Activate()
        {
           
            IGTJobManagementService oJobManagement = GTClassFactory.Create<IGTJobManagementService>();
            IGTKeyObjects oKeyObjects = GTClassFactory.Create<IGTKeyObjects>();
            Dictionary<IGTKeyObject, IGTComponents> AllCUModifiedRecords = new Dictionary<IGTKeyObject, IGTComponents>();
            ADODB.Recordset rs = null;
            Dictionary<int, Int16> dicWorkpoints = new Dictionary<int, short>();
            bool bStructureSelected = false;
          

            try
            {
                if (!ValidateJobType())
                {
                    MessageBox.Show("This command applies only to WR jobs", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                IGTDDCKeyObjects oDDCKeyObjects = m_oApp.SelectedObjects.GetObjects();

                oJobManagement.DataContext = m_oApp.DataContext;
                rs = oJobManagement.FindPendingEdits();

                if (m_oApp.SelectedObjects.GetObjects().Count > 0)
                {
                    if (!ValidateSelectSet(m_oApp.SelectedObjects.GetObjects()))
                    {
                        MessageBox.Show("Select only structures before calling this command, or clear the select set to synchronize all Work Points for the WR.", "G/Technology",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        return;
                    }                    

                    bStructureSelected = true;
                    foreach (IGTDDCKeyObject item in oDDCKeyObjects)
                    {
                        IGTKeyObject oKeyObj = m_oApp.DataContext.OpenFeature(item.FNO, item.FID);
                        lstSelectedStrcuture.Add(oKeyObj.FID);
                        Structure objStructure = new Structure(oKeyObj, m_oApp);
                        MergeDictionary(ref AllCUModifiedRecords, objStructure.GetOwnedFeatureCollection());
                    }
                }
                else
                {
                    if (rs != null && rs.RecordCount > 0)
                    {
                        Dictionary<int, Int16> dicEditedFeatureCollection = new Dictionary<int, Int16>();
                        dicEditedFeatureCollection = FindCUsEditedFeatures(rs);

                        foreach (KeyValuePair<int, Int16> item in dicEditedFeatureCollection)
                        {
                            IGTKeyObject oFeature = m_oApp.DataContext.OpenFeature(item.Value, item.Key);
                            oKeyObjects.Add(oFeature);
                        }

                        Features oFeatureCollection = new Features(oKeyObjects);
                        AllCUModifiedRecords = oFeatureCollection.GetFeatureComponentCollection();
                    }
                }               

                m_oTransactionManager.Begin("Synchronize Work Points...");
                WorkPointOperations oWorkOperations = new WorkPointOperations(null, null, m_oApp.DataContext, AllCUModifiedRecords);
                m_oApp.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Synchronizing Workpoints...");
                oWorkOperations.DoWorkpointOperations();

                GetWP(bStructureSelected, ref dicWorkpoints, false);

                WorkPoints oWWs = new WorkPoints(dicWorkpoints);
                oWorkOperations.SynchronizeWorkPointForObsoleteCUs(dicWorkpoints);
                //GetWP(bStructureSelected, ref dicWorkpoints, false);
                m_oTransactionManager.Commit();
                              
                GetWP(bStructureSelected, ref dicWorkpoints, true);
                oWWs = new WorkPoints(dicWorkpoints);
                oWWs.SynchronizeWorkPointsForDiscardedFeatures();

                m_oTransactionManager.RefreshDatabaseChanges();
                m_oApp.RefreshWindows();
                m_oApp.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Synchronizing Workpoints completed...");

                if (oDDCKeyObjects!=null && oDDCKeyObjects.Count>0)
                {
                    foreach (IGTDDCKeyObject item in oDDCKeyObjects)
                    {
                        item.Dispose();
                    }
                    oDDCKeyObjects.Clear();
                    oDDCKeyObjects = null;
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Synchronize Work Points Command " + ex.Message,"G/Technology", MessageBoxButtons.OK,MessageBoxIcon.Error);               
            }
            m_oApp.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Synchronizing Workpoints completed...");
        }

        private void GetWP(bool p_bStructureSelected, ref Dictionary<int, Int16> dicWorkpoints, bool p_DiscardScenario)
        {
            ADODB.Recordset rsWP = m_oApp.DataContext.OpenRecordset("select g3e_fno,g3e_fid from WORKPOINT_N where WR_NBR = ?", ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, (int)ADODB.CommandTypeEnum.adCmdText, m_oApp.DataContext.ActiveJob);
            dicWorkpoints = new Dictionary<int, short>();

            if (rsWP != null)
            {
                if (rsWP.RecordCount > 0)
                {
                    rsWP.Filter = "g3e_fno = 191";
                    if (rsWP.RecordCount > 0)
                    {
                        while (rsWP.EOF == false)
                        {
                            if (p_bStructureSelected && !p_DiscardScenario)
                            {                                
                                if (IsWPBelongToSelectedStructure((Convert.ToInt32(rsWP.Fields["g3e_fid"].Value))))
                                {
                                    if (!dicWorkpoints.ContainsKey(Convert.ToInt32(rsWP.Fields["g3e_fid"].Value)))
                                    {
                                        dicWorkpoints.Add(Convert.ToInt32(rsWP.Fields["g3e_fid"].Value), Convert.ToInt16(rsWP.Fields["g3e_fno"].Value));
                                    }
                                }
                            }
                            else
                            {
                                if (!dicWorkpoints.ContainsKey(Convert.ToInt32(rsWP.Fields["g3e_fid"].Value)))
                                {
                                    dicWorkpoints.Add(Convert.ToInt32(rsWP.Fields["g3e_fid"].Value), Convert.ToInt16(rsWP.Fields["g3e_fno"].Value));
                                }
                            }
                            rsWP.MoveNext();
                        }
                    }
                }
            }
        }

        private Int16 IsPrimaryGraphic(int p_FNO)
        {
            Int16 iCNO = 0;
            ADODB.Recordset rs = m_oApp.DataContext.MetadataRecordset("G3E_FEATURES_OPTABLE", "g3e_fno = " + p_FNO);
            iCNO = Convert.ToInt16(rs.Fields["G3E_PRIMARYGEOGRAPHICCNO"].Value);
            return iCNO;
        }

        private bool IsWPBelongToSelectedStructure(int p_WPFID)
        {
            bool bReturn = false;
            int iStrcuture = 0;

            ADODB.Recordset rs = m_oApp.DataContext.OpenRecordset(@"select a.g3e_fid
                                                                    from common_n a
                                                                    join workpoint_n b on a.structure_id = b.structure_id
                                                                    where b.g3e_fid = ?
                                                                    and nvl(a.OWNER1_ID, 0) = 0
                                                                    and nvl(a.OWNER2_ID, 0) = 0", ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, (int)ADODB.CommandTypeEnum.adCmdText, p_WPFID);
            if (rs!=null)
            {
                if (rs.RecordCount>0)
                {
                    rs.MoveFirst();
                    iStrcuture = Convert.ToInt32(rs.Fields["g3e_fid"].Value);

                }
            }

            if (lstSelectedStrcuture.Contains(iStrcuture)) bReturn = true;
            return bReturn;
        }
        private void MergeDictionary(ref Dictionary<IGTKeyObject, IGTComponents> p_first, Dictionary<IGTKeyObject, IGTComponents> p_second)
        {
            foreach (var item in p_second)
            {
                if (p_first.ContainsKey(item.Key))
                {
                    p_first[item.Key] = item.Value;
                }
                else
                {
                    p_first.Add(item.Key,item.Value);
                }
            }
        }

     
        private bool ValidateSelectSet(IGTDDCKeyObjects p_selectedFeatures)
        {
            bool bReturn = true;

            foreach (IGTDDCKeyObject item in p_selectedFeatures)
            {
               if (!m_lstStructuresFNOs.Contains(item.FNO))
                {
                    bReturn = false;
                    break;
                }
            }

            return bReturn;
        }
        private List<int> GetDistinctSelectedFIDs(IGTDDCKeyObjects p_ddcKeyObjects)
        {
            List<int> lstReturn = new List<int>();

            foreach (IGTDDCKeyObject item in p_ddcKeyObjects)
            {
                if (!lstReturn.Contains(item.FID))
                {
                    lstReturn.Add(item.FID);
                }
            }

            return lstReturn;
        }
        private Dictionary<Int32, Int16> FindCUsEditedFeatures(ADODB.Recordset p_RecordSet)
        {
            Dictionary<Int32, Int16> oDic = new Dictionary<Int32, Int16>();

            if (p_RecordSet != null)
            {
                p_RecordSet.MoveFirst();
                while (p_RecordSet.EOF == false)
                {
                    int FID = Convert.ToInt32(p_RecordSet.Fields["g3e_fid"].Value);
                    Int16 FNO = Convert.ToInt16(p_RecordSet.Fields["g3e_fno"].Value);
                    int CNO = Convert.ToInt32(p_RecordSet.Fields["g3e_cno"].Value);

                    if (!IsFeatureOwnedToPrimarySwitchGear(FID, FNO))
                    {
                        if (!oDic.Contains(new KeyValuePair<int, short>(FID, FNO)) && (CNO == 22 || CNO == 21 || CNO == 1 || (CNO.Equals(IsPrimaryGraphic(FNO)))))
                        {
                            oDic.Add(FID, FNO);
                        }
                    }
                   
                    p_RecordSet.MoveNext();
                }
            }
            return oDic;
        }

        private bool IsFeatureOwnedToPrimarySwitchGear(int p_FID, int p_FNO)
        {
            bool bReturn = false;
            string sSQL = "Select count(*) from common_n where g3e_id = (select owner1_id from common_n where g3e_fno =? and g3e_fid = ?) and g3e_fno = 19";
            ADODB.Recordset rs = m_oApp.DataContext.OpenRecordset(sSQL, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, (int)ADODB.CommandTypeEnum.adCmdText, p_FNO, p_FID);

            if (rs != null)
            {
                if (rs.RecordCount > 0)
                {
                    rs.MoveFirst();
                    int iCount = Convert.ToInt32(rs.Fields[0].Value);

                    bReturn = iCount == 1;
                }
            }

            return bReturn;
        }

    }
}

