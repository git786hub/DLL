// =======================================================================================================
//  File Name: ccMergeLandbase.cs
// 
//  Description:   Custom command Merge two selected boundaries on Map window
//
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  18/01/2017          Pramod                      Implemented changes in Execute method.                   
// ========================================================================================================

using System;
using System.Windows.Forms;
using ADODB;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;

namespace GTechnology.Oncor.CustomAPI
{

    public class ccMergeLandbase : IGTCustomCommandModeless
    {
        private IGTTransactionManager gtTransactionManager;
        IGTApplication gtApp = null;
        IGTCustomCommandHelper gtCustomCommandHelper;
        const string msgBoxCaption = "Merge Landbase Boundaries";
        const string ValidationMsg = "The selected feature cannot be merged because it is not an updatable landbase boundary";
        const string InterfaceName = "MergeLandbase";
        const string StatusBarMsg = "Select second landbase boundary to merge";
        int g3eFno_src, g3eFid_src, g3eFno_trg, g3eFid_trg;

        /// <summary>
        /// Intialize variables and check selected feature is valid.
        /// </summary>
        /// <param name="CustomCommandHelper"></param>
        public void Activate(Intergraph.GTechnology.API.IGTCustomCommandHelper CustomCommandHelper)
        {
            IGTDDCKeyObjects ddcKeyObjects = GTClassFactory.Create<IGTDDCKeyObjects>();
            gtApp = GTClassFactory.Create<IGTApplication>();
            try
            {
                gtCustomCommandHelper = CustomCommandHelper;
                gtApp.SelectedObjectsChanged += GtApp_SelectedObjectsChanged;
                gtCustomCommandHelper.MouseMove += new EventHandler<GTMouseEventArgs>(gtCustomCommandHelper_MouseMove);
                if (gtApp.DataContext.IsRoleGranted("PRIV_MGMT_LAND"))
                {
                    if (gtApp.SelectedObjects.FeatureCount == 1)
                    {
                        g3eFno_trg = gtApp.SelectedObjects.GetObjects()[0].FNO;
                        g3eFid_trg = gtApp.SelectedObjects.GetObjects()[0].FID;
                        if (!ValidateSelectedFeature(g3eFno_trg))
                        {
                            MessageBox.Show(ValidationMsg, msgBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            CleanUp();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Command Access Denied.  Please contact System Administrator", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CleanUp();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message, msgBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                CleanUp();
            }
        }

        private void GtApp_SelectedObjectsChanged(object sender, EventArgs e)
        {
            try
            {
                if (gtApp.SelectedObjects.FeatureCount > 0)
                {
                    g3eFno_src = gtApp.SelectedObjects.GetObjects()[0].FNO;
                    g3eFid_src = gtApp.SelectedObjects.GetObjects()[0].FID;
                    if (g3eFid_src != g3eFid_trg)
                    {
                        gtApp.BeginWaitCursor();
                        if (g3eFno_src == g3eFno_trg)
                        {
                            //check Selected Feature are Valid for merge
                            if (CheckFeaturesAreValidForMerge(g3eFno_trg, g3eFid_trg, g3eFno_src, g3eFid_src))
                            {
                                // Merge Selected Landbase Boundaries
                                MergeLandbaseBoundary(g3eFno_trg, g3eFid_trg, g3eFno_src, g3eFid_src);
                            }
                        }
                        else
                        {
                            MessageBox.Show(ValidationMsg, msgBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        gtApp.EndWaitCursor();
                        CleanUp();
                    }
                    else
                    {
                        gtApp.Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, StatusBarMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                gtApp.EndWaitCursor();
                MessageBox.Show("Error " + ex.Message, msgBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                CleanUp();
            }
        }

        public bool CanTerminate
        {
            get { return true; }
        }

        public void Pause()
        {

        }

        public void Resume()
        {
        }

        public void Terminate()
        {
            if (gtTransactionManager != null)
            {
                if (gtTransactionManager.TransactionInProgress)
                    gtTransactionManager.Rollback();
            }
            gtTransactionManager = null;
        }

        public Intergraph.GTechnology.API.IGTTransactionManager TransactionManager
        {
            set { gtTransactionManager = value; }
        }

        #region Events

        /// <summary>
        /// Custom command mouse move event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void gtCustomCommandHelper_MouseMove(object sender, GTMouseEventArgs e)
        {
            gtApp.Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, StatusBarMsg);
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Mcheck Selected features are Valid for Merge
        /// </summary>
        /// <param name="g3eFno_trg"></param>
        /// <param name="g3eFid_trg"></param>
        /// <param name="g3eFno_src"></param>
        /// <param name="g3eFid_src"></param>
        /// <returns></returns>
        private bool CheckFeaturesAreValidForMerge(int g3eFno_trg, int g3eFid_trg, int g3eFno_src, int g3eFid_src)
        {
            ADODB.Recordset rs = null;
            bool flag = false;
            string sqlStmt = "SELECT LBM_UTL.FeaturesAreValidForMerge({0},{1},{2},{3}) FROM DUAL";
            string mergeStatus = string.Empty;
            try
            {
                rs = Execute(string.Format(sqlStmt, g3eFno_trg, g3eFid_trg, g3eFno_src, g3eFid_src));
                if (rs != null)
                {
                    rs.MoveFirst();
                    mergeStatus = Convert.ToString(rs.Fields[0].Value.ToString());
                    if (mergeStatus == "TRUE") { flag = true; } else { flag = false; }
                }

                if (!flag)
                {
                    MessageBox.Show(mergeStatus, msgBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message, msgBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return flag;

        }


        /// <summary>
        /// Merge Selected landbase Boundaries
        /// </summary>
        /// <param name="g3eFno_trg"></param>
        /// <param name="g3eFid_trg"></param>
        /// <param name="g3eFno_src"></param>
        /// <param name="g3eFid_src"></param>
        private void MergeLandbaseBoundary(int g3eFno_trg, int g3eFid_trg, int g3eFno_src, int g3eFid_src)
        {
            ADODB.Recordset rs = null;
            string sqlStmt = "Begin LBM_UTL.MergeLandbaseBoundaries({0},{1},{2},{3}); end;";
            string mergeStatus = string.Empty;
            try
            {

                gtTransactionManager.Begin("Merging Landbase Boundries");
                rs = Execute(string.Format(sqlStmt, g3eFno_trg, g3eFid_trg, g3eFno_src, g3eFid_src));
                gtTransactionManager.Commit();
            }
            catch (Exception ex)
            {
                gtTransactionManager.Rollback();
                throw ex;
            }
            finally
            {
                rs = null;
            }
            gtTransactionManager.RefreshDatabaseChanges();
        }

        /// <summary>
        /// Validate Selected feature is valid
        /// </summary>
        /// <param name="g3efno"></param>
        /// <returns></returns>
        private bool ValidateSelectedFeature(int g3efno)
        {
            bool flag = false;
            Recordset rs = null;
            try
            {

                string sqlStmt = "select count(*) as cnt from LBM_FEATURE WHERE lbm_interface='{0}' and SOURCE_FNO={1}";
                rs = Execute(string.Format(sqlStmt, InterfaceName, g3efno));
                if (rs != null)
                {
                    rs.MoveFirst();
                    if (Convert.ToInt32(rs.Fields["cnt"].Value) > 0) { flag = true; }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                rs = null;
            }
            return flag;
        }

        /// <summary>
        /// Execute SQL statement
        /// </summary>
        /// <param name="sqlStmt"></param>
        /// <returns></returns>
        private Recordset Execute(string sqlStmt)
        {
            int recordsAffected;
            ADODB.Recordset rs = null;
            rs = gtApp.DataContext.Execute(sqlStmt, out recordsAffected, (int)ADODB.CommandTypeEnum.adCmdText);
            return rs;
        }


        /// <summary>
        /// Dispose all method before command exit
        /// </summary>
        private void CleanUp()
        {
            gtApp.SelectedObjectsChanged -= GtApp_SelectedObjectsChanged;
            gtCustomCommandHelper.MouseMove -= gtCustomCommandHelper_MouseMove;
            if (gtCustomCommandHelper != null)
            {
                gtCustomCommandHelper.Complete();
            }
            gtCustomCommandHelper = null;
        }

        #endregion
    }
}
