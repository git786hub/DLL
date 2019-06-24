// =======================================================================================================
//  File Name: ccDetectOverlappingAnalysis.cs
// 
//  Description:   Custom command Detect Overlapping Polygons
//
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  02/05/2018          Pramod                      Implemented to Detect Overlapping Polygons.                   
// ========================================================================================================

using ADODB;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{

    public class ccDetectOverlappingAnalysis : IGTCustomCommandModal
    {

        private IGTTransactionManager gtTransactionManager;
        IGTApplication gtApp = null;
        const string msgBoxCaption = "Detect Overlapping Boundarys Analysis";
        const string InterfaceName = "DETECTOVERLAPPINGPOLYGONS";

        DetectOverlappingAnalysis detectOverlappingAnalysis = null;


        public void Activate()
        {
            List<KeyValuePair<int, string>> featureTypes = null;
            try
            {
                gtApp = GTClassFactory.Create<IGTApplication>();
                if (gtApp.DataContext.IsRoleGranted("PRIV_MGMT_LAND"))
                {

                    featureTypes = GetFeatureClass();
                    detectOverlappingAnalysis = new DetectOverlappingAnalysis(featureTypes);

                    if (detectOverlappingAnalysis.ShowDialog() == DialogResult.OK)
                    {
                        DetectOverlapping(detectOverlappingAnalysis.SelectedFeatureType, detectOverlappingAnalysis.IsSelfOverlap);
                    }
                }
                else
                {
                    MessageBox.Show("Command Access Denied.  Please contact System Administrator", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message, msgBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                CleanUp();
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

        private List<KeyValuePair<int, string>> GetFeatureClass()
        {
            string sqlStmt = "Select DISTINCT F.G3E_FNO AS Key,F.G3E_USERNAME AS VALUE from G3E_FEATURES_OPTABLE F,LBM_FEATURE LB WHERE F.G3E_FNO=LB.SOURCE_FNO AND LB.LBM_INTERFACE='{0}' ORDER BY F.G3E_FNO";
            return ConvertRSToKeyValue<int, string>(Execute(string.Format(sqlStmt,InterfaceName)));
        }


        /// <summary>
        /// Execute Procudure to Purge Expired Archived Landbase
        /// </summary>
        private void DetectOverlapping(int g3eFno,bool selfOverlap)
        {
            ADODB.Recordset rs = null;
            string sqlStmt = "Begin LBM_UTL.DetectOverlappingPolygons({0},{1}); end;";
            try
            {
                gtApp.BeginWaitCursor();
                sqlStmt = string.Format(sqlStmt, g3eFno, selfOverlap);
                gtApp.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Detecting Overlapping Boundarys....");
                gtTransactionManager.Begin("Detect Overlapping Boundarys");
                rs = Execute(sqlStmt);
                 gtTransactionManager.Commit();
                MessageBox.Show("Detecting Overlapping Boundarys Analysis is Completed", msgBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
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


        private List<KeyValuePair<TKey, TValue>> ConvertRSToKeyValue<TKey, TValue>(Recordset rs)
        {

            List<KeyValuePair<TKey, TValue>> keyValues = new List<KeyValuePair<TKey, TValue>>();
            while (!rs.EOF)
            {
                keyValues.Add(new KeyValuePair<TKey, TValue>((TKey)Convert.ChangeType(rs.Fields["Key"].Value, typeof(TKey)), (TValue)Convert.ChangeType(rs.Fields["Value"].Value, typeof(TValue))));
                rs.MoveNext();
            }
            return keyValues;
        }


        /// <summary>
        /// Dispose all method before command exit
        /// </summary>
        private void CleanUp()
        {
            if (detectOverlappingAnalysis!=null)
            {
                detectOverlappingAnalysis.Dispose();
            }
            detectOverlappingAnalysis = null;
            if (gtTransactionManager != null)
            {
                if (gtTransactionManager.TransactionInProgress)
                    gtTransactionManager.Rollback();
            }
            gtTransactionManager = null;
        }
       
        #endregion
    }
}
