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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using ADODB;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;

namespace GTechnology.Oncor.CustomAPI
{

    public class ccDetectOverlappingAnalysis : IGTCustomCommandModeless
    {
        private IGTTransactionManager gtTransactionManager;
        IGTApplication gtApp = null;
        IGTCustomCommandHelper gtCustomCommandHelper;
        const string msgBoxCaption = "Detect Overlapping Spatial Analysis";
        const string InterfaceName = "DETECTOVERLAPPINGPOLYGONS";

        DetectOverlappingAnalysis detectOverlappingAnalysis = null;
        delegate void DoWorkDelegate(int g3eFno, bool selfOverlap);

        /// <summary>
        /// Intialize variables and check selected feature is valid.
        /// </summary>
        /// <param name="CustomCommandHelper"></param>
        public void Activate(Intergraph.GTechnology.API.IGTCustomCommandHelper CustomCommandHelper)
        {
            gtApp = GTClassFactory.Create<IGTApplication>();
            List<KeyValuePair<int, string>> featureTypes = null;
            try
            {
                gtCustomCommandHelper = CustomCommandHelper;
                gtCustomCommandHelper.MouseMove += gtCustomCommandHelper_MouseMove;
                featureTypes = GetFeatureClass();
                detectOverlappingAnalysis = new DetectOverlappingAnalysis(featureTypes);
               
                if (detectOverlappingAnalysis.ShowDialog()==DialogResult.OK)
                {
                //    RunProcess(detectOverlappingAnalysis.SelectedFeatureType, detectOverlappingAnalysis.IsSelfOverlap);
                    DetectOverlapping(detectOverlappingAnalysis.SelectedFeatureType, detectOverlappingAnalysis.IsSelfOverlap);
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

        private void gtCustomCommandHelper_MouseMove(object sender, GTMouseEventArgs e)
        {
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
            string sqlStmt = "Begin LBM_UTL.DectectOverlappingPolygons({0},{1}); end;";
            string mergeStatus = string.Empty;
            try
            {
                sqlStmt = string.Format(sqlStmt, g3eFno, selfOverlap);
                rs = Execute(sqlStmt);
           //    gtTransactionManager.Commit();
            }
            catch (Exception ex)
            {
             //   gtTransactionManager.Rollback();
                throw ex;
            }
            finally
            {
                rs = null;
             //   gtTransactionManager.RefreshDatabaseChanges();
                gtApp.RefreshWindows();
          //      CleanUp();
            }
            MessageBox.Show("Detecting Overlapping Analysis is Completed");
        }

        private string GetDatabaseName()
        {
           Recordset rs= Execute("select sys_context('userenv','instance_name') from dual");
            rs.MoveFirst();
            string dbName = rs.Fields[0].Value.ToString();
            rs = null;
            return dbName;

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
            gtCustomCommandHelper.MouseMove -= gtCustomCommandHelper_MouseMove;
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
            if (gtCustomCommandHelper != null)
            {
                gtCustomCommandHelper.Complete();
            }
            gtCustomCommandHelper = null;
        }
        
       
        private void RunProcess(int g3eFno, bool selfOverlap)
        {
            string sqlFile= Path.GetTempPath() + "\\RunDetectOverlapp.sql";
            StreamWriter sw = new StreamWriter(sqlFile);
            sw.WriteLine("Set Echo on");
            sw.WriteLine(string.Format("EXECUTE ltt_user.editjob('{0}');",gtApp.DataContext.ActiveJob));
            sw.WriteLine(string.Format("Execute LBM_UTL.DectectOverlappingPolygons({0},{1});", g3eFno,selfOverlap));
            sw.WriteLine("Execute LTT_USER.DONE;");
            sw.WriteLine("Set Echo OFF");
            sw.WriteLine("$PowerShell -Command \"Add-Type -AssemblyName PresentationFramework;[System.Windows.MessageBox]::Show('Detecting Overlapping analysis is completed','Detect Overlapping Analysis')\"");
            sw.WriteLine("EXIT;");
            sw.Close();

            string dbUserName = gtApp.DataContext.DatabaseUserName;
            string dbPassword = gtApp.DataContext.ViewerConnectionString.Substring(gtApp.DataContext.ViewerConnectionString.IndexOf("Password="), (gtApp.DataContext.ViewerConnectionString.Length - gtApp.DataContext.ViewerConnectionString.IndexOf("Password=")));
            dbPassword = dbPassword.Replace("Password=", "").Replace(";","").Trim(' ');
           string dataSource=GetDatabaseName();


            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = "sqlplus";



            var args = string.Format("{0}/{1}@{2} @\"{3}\"",dbUserName,dbPassword,dataSource, @sqlFile);
            MessageBox.Show("aRGS " + args);
            p.StartInfo.Arguments = args;
            p.StartInfo.CreateNoWindow = false;
            bool started = p.Start();
        }
        #endregion
    }
}
