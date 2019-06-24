//----------------------------------------------------------------------------+
//        Class: SharedWriteBackLibrary
//  Description: This class holds events and methods to process Update Job Status and Writeback.
//                                                                  
//----------------------------------------------------------------------------+
//          $Author:: Shubham Agarwal                                       $
//          $Date:: 25/03/18                                                $
//          $Revision:: 1                                                   $
//----------------------------------------------------------------------------+
//    $History:: CustomSharedCodeClass.cs                     $
// 
// *****************  Version 1  *****************
// User: sagarwal     Date: 25/03/18   Time: 18:00  Desc : Created
//----------------------------------------------------------------------------+

using ADODB;
using Intergraph.GTechnology.API;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Windows.Forms;

namespace CustomWriteBackLibrary
{
    public class SharedWriteBackLibrary 
    {

        /// <summary>
        /// Event that is raised when Update Job Status processing is completed. Client needs to register this event to get notification of Update Job Status process completion
        /// </summary>
        public event OnUpdateJobStatusCompletion UpdateJobStatusProcessCompleted;

        /// <summary>
        /// Event that is raised when Writeback processing is completed. Client needs to register this event to get notification of Writeback  process completion
        /// </summary>
        public event OnWriteBackCompletion WriteBackProcessCompleted;

        private DataTable GetDataTable(IGTDataContext oDC, Recordset oRS)
        {
            try
            {
                //  Recordset oRS = oDC.OpenRecordset(sSQL, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText);
                OleDbDataAdapter oDA = new System.Data.OleDb.OleDbDataAdapter();
                DataTable oDT = new DataTable();
                oDA.Fill(oDT, oRS);
                oDA.Dispose();
                oRS.Close();
                return oDT;
            }
            catch (Exception oEx)
            {
                throw oEx;
            }
        }
        public bool ValidateWorkPoints()
        {
            bool bReturn = true;
            string sSql = "";
            ADODB.Recordset rs = null;
            ADODB.Recordset rsWP = null;

            sSql = @"select g3e_fno, g3e_fid,wpCUs.STRUCTURE_ID, count(g3e_fid)
                    from (
                      select 
                        COALESCE(comm.STRUCTURE_ID, owner.STRUCTURE_ID) as STRUCTURE_ID
                      , cu.G3E_FID, cu.CU_C, cu.ACTIVITY_C, cu.g3e_fno g3e_fno
                      , count(*) as cuCount
                      from      COMP_UNIT_N cu
                      join      COMMON_N    comm  on cu.G3E_FID = comm.G3E_FID
                      left join COMMON_N    owner on (COALESCE(comm.OWNER1_ID,comm.OWNER2_ID) = owner.G3E_ID)
                      where (cu.WR_ID = ? OR cu.WR_EDITED =?) and (owner.g3e_fno<>19 or owner.g3e_fno is null)
                      group by COALESCE(comm.STRUCTURE_ID, owner.STRUCTURE_ID), cu.G3E_FID, cu.CU_C, cu.ACTIVITY_C, cu.g3e_fno
                      ) CUs
                    left join (
                      select 
                        wp.STRUCTURE_ID
                      , wcu.ASSOC_FID, wcu.CU_C, wcu.ACTIVITY_C, wp.g3e_fno WPfno
                      , count(*) as cuCount
                      from WORKPOINT_N    wp
                      join WORKPOINT_CU_N wcu on wp.G3E_FID = wcu.G3E_FID
                      where wp.WR_NBR = ?
                      group by wp.STRUCTURE_ID, wcu.ASSOC_FID, wcu.CU_C, wcu.ACTIVITY_C, wp.g3e_fno
                      ) wpCUs on (    CUs.G3E_FID = wpCUs.ASSOC_FID 
                                  and CUs.STRUCTURE_ID = wpCUs.STRUCTURE_ID 
                                  and CUs.CU_C = wpCUs.CU_C) group by wpCUs.STRUCTURE_ID, g3e_fid,g3e_fno";

            Dictionary<int, Int16> FIDs = new Dictionary<int, Int16>();
            Dictionary<int, Int16> MissingWPFeatures = new Dictionary<int, short>();
            Dictionary<int, Int16> MissingWPCUsFeatures = new Dictionary<int, short>();
            IGTApplication m_oApp = GTClassFactory.Create<IGTApplication>();

            rs = m_oApp.DataContext.OpenRecordset(sSql, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)ADODB.CommandTypeEnum.adCmdText, m_oApp.DataContext.ActiveJob, m_oApp.DataContext.ActiveJob, m_oApp.DataContext.ActiveJob);
            rsWP = m_oApp.DataContext.OpenRecordset("select g3e_fid,STRUCTURE_ID from WORKPOINT_N where WR_NBR = ?", CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)ADODB.CommandTypeEnum.adCmdText, m_oApp.DataContext.ActiveJob);
            DataTable dtWP = GetDataTable(m_oApp.DataContext, rsWP);

            if (rs != null)
            {
                if (rs.RecordCount > 0)
                {
                    rs.MoveFirst();
                    DataTable dt = GetDataTable(m_oApp.DataContext, rs);
                    if (dt != null)
                    {
                        DataRow[] rowsWithNull = dt.Select("STRUCTURE_ID is null");
                        if (rowsWithNull.Count<DataRow>() > 0)
                        {
                            foreach (var item in rowsWithNull)
                            {
                                if (!FIDs.ContainsKey(Convert.ToInt32(item["g3e_fid"])))
                                {
                                    FIDs.Add(Convert.ToInt32(item["g3e_fid"]), Convert.ToInt16(item["g3e_fno"]));
                                }
                            }
                        }

                        foreach (KeyValuePair<int, Int16> item in FIDs)
                        {
                            DataRow[] rows = dt.Select("g3e_fid = " + item.Key + " and STRUCTURE_ID is not null");

                            if (rows != null && rows.Count<DataRow>() > 0)
                            {
                                if (!MissingWPCUsFeatures.ContainsKey(Convert.ToInt32(rows[0]["G3E_FID"])))
                                {
                                    string iWPFID = Convert.ToString(dtWP.Select("STRUCTURE_ID = '" + Convert.ToString(rows[0]["STRUCTURE_ID"]) + "'")[0]["G3E_FID"]);

                                    MissingWPCUsFeatures.Add(Convert.ToInt32(iWPFID), 191);
                                }
                            }
                            else
                            {
                                if (!MissingWPFeatures.ContainsKey(item.Key))
                                {
                                    MissingWPFeatures.Add(item.Key, item.Value);
                                }
                            }
                        }
                    }
                }
            }

            if (MissingWPCUsFeatures.Count > 0)
            {
                bReturn = false;

                foreach (KeyValuePair<int, Int16> item in MissingWPCUsFeatures)
                {
                    m_oApp.SelectedObjects.Clear();
                    IGTDDCKeyObjects oDDCs = m_oApp.DataContext.GetDDCKeyObjects(item.Value, item.Key, GTComponentGeometryConstants.gtddcgAllGeographic);

                    foreach (IGTDDCKeyObject item1 in oDDCs)
                    {
                        m_oApp.SelectedObjects.Add(GTSelectModeConstants.gtsosmAllComponentsInActiveLegend, item1);
                    }
                }

                MessageBox.Show("Unable to write to WMIS; highlighted Work Points do not reflect all CU activity.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return bReturn;
            }
            if (MissingWPFeatures.Count > 0)
            {
                m_oApp.SelectedObjects.Clear();
                bReturn = false;
                foreach (KeyValuePair<int, Int16> item in MissingWPFeatures)
                {
                    IGTDDCKeyObjects oDDCs = m_oApp.DataContext.GetDDCKeyObjects(item.Value, item.Key, GTComponentGeometryConstants.gtddcgAllGeographic);

                    foreach (IGTDDCKeyObject item1 in oDDCs)
                    {
                        m_oApp.SelectedObjects.Add(GTSelectModeConstants.gtsosmAllComponentsInActiveLegend, item1);
                    }
                }
                MessageBox.Show("Unable to write to WMIS; highlighted features have no Work Point.”", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return bReturn;
            }

            string sDiscardedCUs = @"select  g3e_fid,wpCUs.STRUCTURE_ID, count(g3e_fid)
                    from (
                      select 
                        COALESCE(comm.STRUCTURE_ID, owner.STRUCTURE_ID) as STRUCTURE_ID
                      , cu.G3E_FID, cu.CU_C, cu.ACTIVITY_C, cu.g3e_fno g3e_fno
                      , count(*) as cuCount
                      from      COMP_UNIT_N cu
                      join      COMMON_N    comm  on cu.G3E_FID = comm.G3E_FID
                      left join COMMON_N    owner on  (COALESCE(comm.OWNER1_ID,comm.OWNER2_ID) = owner.G3E_ID)
                      where (cu.WR_ID = ? OR cu.WR_EDITED =?) 
                      group by COALESCE(comm.STRUCTURE_ID, owner.STRUCTURE_ID), cu.G3E_FID, cu.CU_C, cu.ACTIVITY_C, cu.g3e_fno
                      ) CUs
                    right join (
                      select 
                        wp.STRUCTURE_ID
                      , wcu.ASSOC_FID, wcu.CU_C, wcu.ACTIVITY_C, wp.g3e_fno WPfno
                      , count(*) as cuCount
                      from WORKPOINT_N    wp
                      join WORKPOINT_CU_N wcu on wp.G3E_FID = wcu.G3E_FID
                      where wp.WR_NBR = ?
                      group by wp.STRUCTURE_ID, wcu.ASSOC_FID, wcu.CU_C, wcu.ACTIVITY_C, wp.g3e_fno
                      ) wpCUs on (    CUs.G3E_FID = wpCUs.ASSOC_FID 
                                  and CUs.STRUCTURE_ID = wpCUs.STRUCTURE_ID 
                                  and CUs.CU_C = wpCUs.CU_C) group by wpCUs.STRUCTURE_ID, g3e_fid having count(g3e_fid) = 0 and g3e_fid is null";
            rs = m_oApp.DataContext.OpenRecordset(sDiscardedCUs, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, (int)ADODB.CommandTypeEnum.adCmdText, m_oApp.DataContext.ActiveJob, m_oApp.DataContext.ActiveJob, m_oApp.DataContext.ActiveJob);
            if (rs != null)
            {
                if (rs.RecordCount > 0)
                {
                    bReturn = false;
                    MessageBox.Show("Work Point(s) are out of synchronization", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return bReturn;
                }
            }
            return bReturn;
        }

        /// <summary>
        /// Method to initiate Update Writeback processing. This method is an asynchronous call and client will get notification of completion of the process through event UpdateJobStatusProcessCompleted
        /// </summary>
        /// <param name="p_sJobIdentifier">G3E_JOB.G3E_JOBIDENTIFIER</param>
        /// <param name="taskID"> Unique task ID that needs to be passed through caller. Example  Guid taskID = Guid.NewGuid();</param>
        public void UpdateWriteBack(string p_sJobIdentifier,object taskID)
        {
            WritebackClass oWriteBack = new WritebackClass(WriteBackProcessCompleted);          
            oWriteBack.UpdateWriteBack(p_sJobIdentifier, taskID);
        }
       
        /// <summary>
        /// Method to initiate Update Job Status processing. This method is an asynchronous call and client will get notification of completion of the process through event WriteBackProcessCompleted
        /// </summary>
        /// <param name="p_wrNumber">G3E_JOB.WR_NBR</param>
        /// <param name="p_jobStatus">G3E_JOB.JOB_STATUS</param>
        /// <param name="taskId">Unique task ID that needs to be passed through caller. Example  Guid taskID = Guid.NewGuid();</param>
        public void UpdateJobStatus(string p_wrNumber, string p_jobStatus, object taskId)
        {
            IGTApplication oApp = GTClassFactory.Create<IGTApplication>();
            JobStatusUpdate oJobStatus = new JobStatusUpdate(oApp, UpdateJobStatusProcessCompleted);
            oJobStatus.UpdateJobStatus(p_wrNumber, p_jobStatus, taskId);
        }
    }
}
