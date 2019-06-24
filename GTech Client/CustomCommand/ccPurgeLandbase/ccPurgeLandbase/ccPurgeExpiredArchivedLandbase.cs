// =======================================================================================================
//  File Name: ccPurgeExpiredArchivedLandbase.cs
// 
//  Description:   Custom command Purge Expired Archived Landbase data
//
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  18/01/2018          Pramod                      Implemented changes in Execute method.                   
// ========================================================================================================

using ADODB;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public  class ccPurgeExpiredArchivedLandbase : IGTCustomCommandModal
    {

        private IGTTransactionManager gtTransactionManager;
        IGTApplication gtApp = null;
        const string msgBoxCaption = "Purge Expired Archived Landbase";

        void IGTCustomCommandModal.Activate()
        {
            gtApp = GTClassFactory.Create<IGTApplication>();
            try
            {
                if (gtApp.DataContext.IsRoleGranted("PRIV_MGMT_LAND"))
                {
                    PurgeLandbase();
                    MessageBox.Show("Sucessfully purge expired archived landbase", msgBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Command Access Denied.  Please contact System Administrator", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                var errMsg = ex.Message;
                if (ex.Message.Contains("ORA-20001"))
                {
                    errMsg = "Parameter ArchivedLandbaseExpireDays missing in SYS_GENERALPARAMETER TABLE";
                }

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
        /// Execute Procudure to Purge Expired Archived Landbase
        /// </summary>
        private void PurgeLandbase()
        {
            ADODB.Recordset rs = null;
            string sqlStmt = "Begin LBM_UTL.PurgeExpiredArchivedLandbase; end;";
            string mergeStatus = string.Empty;
            try
            {

                gtTransactionManager.Begin("Purge Expired Landbase");
                gtApp.BeginWaitCursor();
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
