// =======================================================================================================
//  File Name: ccDetectPoylgonEdgeMismatch.cs
// 
//  Description:   Custom command Detect Polygon Edge Mismatch
//
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  02/05/2018          Pramod                      Implemented to  Detect Polygon Edge Mismatch.                   
// ========================================================================================================


using ADODB;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public class ccDetectPoylgonEdgeMismatch : IGTCustomCommandModal
    {

        private IGTTransactionManager gtTransactionManager;
        IGTApplication gtApp = null;
        const string msgBoxCaption = "Detech Polygon Edge Mismacth";

        void IGTCustomCommandModal.Activate()
        {
            try
            {
                gtApp = GTClassFactory.Create<IGTApplication>();
                if (gtApp.DataContext.IsRoleGranted("PRIV_MGMT_LAND"))
                {

                    DetectPolygonEdgeMismatch();
                    MessageBox.Show("Detecting Polygon Edge Mismatch is Completed", msgBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Command Access Denied.  Please contact System Administrator", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                var errMsg = ex.Message;
                MessageBox.Show("Error " + errMsg, msgBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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


        #region Private Methods

        /// <summary>
        /// Execute Procudure to Detect Polygon Edge Mismatch
        /// </summary>
        private void DetectPolygonEdgeMismatch()
        {
            Recordset rs = null;
            string sqlStmt = "Begin LBM_UTL.DetectPolygonEdgeMismatch; end;";
            string mergeStatus = string.Empty;
            try
            {
                gtApp.BeginWaitCursor();
                gtApp.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Detecting Polygon Edge Mismatch....");
                gtTransactionManager.Begin("Detect Polygon Edge Mismatch");
                rs = Execute(sqlStmt);
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
                gtTransactionManager.RefreshDatabaseChanges();
                gtApp.RefreshWindows();
                gtApp.EndWaitCursor();
            }

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
        #endregion
    }
}
