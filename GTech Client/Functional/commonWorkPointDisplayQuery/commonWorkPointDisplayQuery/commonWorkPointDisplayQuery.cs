//----------------------------------------------------------------------------+
//        Class: commonWorkPointDisplayQuery
//  Description: This common dll refreshes the display of Work Points for the active job by displaying graphic components as query results.
//----------------------------------------------------------------------------+
//     $Author:: Prathyusha Lella (pnlella)                                                      $
//       $Date:: 6/12/17                                                     $
//   $Revision:: 1                                                            $
//----------------------------------------------------------------------------+
//    $History::                                         $
// 
// *****************  Version 1  *****************
// User: pnlella     Date: 6/12/17    Time: 11:00
// User: pnlella     Date: 18/1/18    Time: 11:00 Modified the code as per code review comments given by Barry
// User: skamaraj    Date: 14/03/18    Modified the code to redisplay the workpoints which are creted from backend/from Code.
//----------------------------------------------------------------------------+

using System;
using System.Collections.Generic;
using ADODB;
using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI
{
    /// <summary>
    /// CommonWorkPointDisplayQuery
    /// </summary>
    public class CommonWorkPointDisplayQuery
    {
        #region Private Members
        private IGTApplication m_gtApplication;
        private IGTDisplayService m_gtDisplayService;
        private string m_gtWorkPtQueryDisplayName;
        private const string m_gtWORKPOINT_QUERY = "WorkPoint Query";
        private Recordset m_gtRecordset = null;
        private bool ? m_bDeleteAction = null;
        private Recordset rsOldQueryRecord = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="application">The current G/Technology application object.</param>
        /// <param name="currentWorkPointAttributeComponentRecord">Current Recordset of the Workpoint feature record</param>//This Recordset needs to be passed for the Workpoint attribute component that is under transaction and is not yet commited to the DB. In the cases where FI uses this shared component, the Workpoint attribute component recordset that fired the FI needs to be passed, and in all other cases this can be null 
        /// <param name="IsDeleteOperation">This parameter is needs to be passed when FI uses this component. In case when FI is fired for addition, pass false and if FI is fired during the delete operation, pass it as true.</param>
        public CommonWorkPointDisplayQuery(IGTApplication application, Recordset currentWorkPointAttributeComponentRecord, bool IsDeleteOperation)
        {
            m_gtApplication = application;
            m_gtWorkPtQueryDisplayName = "WR" + m_gtApplication.DataContext.ActiveJob;
            m_gtDisplayService = m_gtApplication.ActiveMapWindow.DisplayService;
            m_gtRecordset = currentWorkPointAttributeComponentRecord;
            m_bDeleteAction = IsDeleteOperation;
        }

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="application">The current G/Technology application object.</param>
        public CommonWorkPointDisplayQuery(IGTApplication application)
        {
            m_gtApplication = application;
            m_gtWorkPtQueryDisplayName = "WR" + m_gtApplication.DataContext.ActiveJob;
            m_gtDisplayService = m_gtApplication.ActiveMapWindow.DisplayService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Method to display off the work point queries of other WR and attach the results of all Work Point features query in the active WR to the display control.
        /// </summary>
        public void RedisplayWorkPoints()
        {
            OldWorkPtsDisplayOff();
            WorkPointDisplayQuery();
        }

        /// <summary>
        /// This method runs a query for all Work Point features in the active WR and attach the results to the display control
        /// </summary>
        private void WorkPointDisplayQuery()
        {
            Recordset workPointRS = null;
            List<int> iListFids = new List<int>();
            try
            {

                workPointRS = m_gtApplication.DataContext.OpenRecordset("Select * from WORKPOINT_N where WR_NBR=:1"
                  , CursorTypeEnum.adOpenStatic,
                           LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, m_gtApplication.DataContext.ActiveJob);

                if (workPointRS != null && workPointRS.RecordCount > 0)
                {
                    workPointRS.MoveFirst();
                    while (!workPointRS.EOF)
                    {
                        if (workPointRS.Fields["G3E_FID"] != null && workPointRS.Fields["G3E_FID"].Value != null &&
                            !String.IsNullOrEmpty(workPointRS.Fields["G3E_FID"].Value.ToString()))
                        {
                            iListFids.Add(Convert.ToInt32(workPointRS.Fields["G3E_FID"].Value));
                        }
                        workPointRS.MoveNext();
                    }
                }

                if (rsOldQueryRecord != null && rsOldQueryRecord.RecordCount > 0 && m_bDeleteAction == false)
                {
                    rsOldQueryRecord.MoveFirst();
                    while (!rsOldQueryRecord.EOF)
                    {
                        if (rsOldQueryRecord.Fields["G3E_FID"] != null &&
                            !iListFids.Contains(Convert.ToInt32(rsOldQueryRecord.Fields["G3E_FID"].Value)))
                        {
                            workPointRS.AddNew();
                            for (int i = 0; i < rsOldQueryRecord.Fields.Count; i++)
                            {
                                workPointRS.Fields[rsOldQueryRecord.Fields[i].Name].Value = rsOldQueryRecord.Fields[i].Value;
                            }
                            workPointRS.Update();
                        }
                        rsOldQueryRecord.MoveNext();
                    }



                }



                if (m_bDeleteAction == true)
                {
                    if (workPointRS != null && workPointRS.RecordCount > 0)
                    {
                        for (workPointRS.MoveFirst(); !workPointRS.EOF; workPointRS.MoveNext())
                        {
                            if (workPointRS.Fields["g3e_fid"].Value.Equals(m_gtRecordset.Fields["g3e_fid"].Value))
                            {
                                workPointRS.Delete();
                                break;
                            }
                        }
                    }
                }
                else if (m_bDeleteAction == false)
                {
                    if (CheckActiveRecord(workPointRS) && Convert.ToString(m_gtRecordset.Fields["WR_NBR"].Value) == m_gtApplication.DataContext.ActiveJob)
                    {
                        workPointRS.AddNew();
                        for (int i = 0; i < m_gtRecordset.Fields.Count; i++)
                        {
                            workPointRS.Fields[m_gtRecordset.Fields[i].Name].Value = m_gtRecordset.Fields[i].Value;
                        }
                        workPointRS.Update();
                    }
                }
                if (workPointRS != null && workPointRS.RecordCount > 0)
                {
                    workPointRS.MoveFirst();
                    m_gtDisplayService.AppendQuery(m_gtWORKPOINT_QUERY, m_gtWorkPtQueryDisplayName, workPointRS, null, false);
                    m_gtApplication.RefreshWindows();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                workPointRS = null;
            }
        }
        /// <summary>
        /// This method display off the work point queries of other WR and remove the active WR query.
        /// </summary>
        private void OldWorkPtsDisplayOff()
        {
            IGTDisplayControlNodes displayCnodes;
            try
            {
                if (m_gtDisplayService.GetNodeChildren(m_gtWORKPOINT_QUERY) != DBNull.Value)
                {
                    displayCnodes = m_gtDisplayService.GetDisplayControlNodes(m_gtWORKPOINT_QUERY);
                    foreach (IGTDisplayControlNode node in displayCnodes)
                    {
                        if (node.DisplayName == m_gtWorkPtQueryDisplayName)
                        {
                            rsOldQueryRecord = ((IGTQueryLegendEntry)node.LegendEntry).Recordset;
                            m_gtDisplayService.Remove(m_gtWORKPOINT_QUERY, m_gtWorkPtQueryDisplayName);
                        }
                        else if (node.DisplayName != m_gtWORKPOINT_QUERY && node.LegendEntry.Displayable)
                        {
                            node.LegendEntry.Displayable = false;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }



        /// <summary>
        /// Method to Check for the active record is already placed data or new data.
        /// </summary>
        /// <param name="rs">Recorset of the all workpoints present in active WR</param>
        /// <returns>false, if current workpoint attribute record is already present in given parameter recordset rs</returns>
        private bool CheckActiveRecord(Recordset rs)
        {
            bool updateRec = true;
            try
            {
                if (rs != null && rs.RecordCount > 0)
                {
                    rs.MoveFirst();
                    while (!rs.EOF)
                    {
                        if (rs.Fields["g3e_fid"].Value.Equals(m_gtRecordset.Fields["g3e_fid"].Value))
                        {
                            updateRec = false;
                            break;
                        }
                        rs.MoveNext();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return updateRec;
        }

        #endregion
    }
}