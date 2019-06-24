using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using Intergraph.GTechnology.API;
using Intergraph.GTechnology.Interfaces;
using Microsoft.Office.Interop.Excel;
using System.DirectoryServices.AccountManagement;
using ADODB;
using System.Windows.Forms;
using System.CodeDom.Compiler;

namespace GTechnology.Oncor.CustomAPI
{
    public class ccRequestESILocation : IGTCustomCommandModeless
    {

        public IGTTransactionManager TransactionManager {
            set {
                dataProvider.transactionManager = value;
            }
        }

        bool IGTCustomCommandModeless.CanTerminate
        {
            get
            {
                return true;
            }
        }

        public void Activate(IGTCustomCommandHelper CustomCommandHelper) {
            PremiseNumberDialog dialogWindow = new PremiseNumberDialog();
            dataProvider.ccHelper = CustomCommandHelper;
            if(dataProvider.ErrorMessage == null)
                dialogWindow.Show(dataProvider.gtApp.ApplicationWindow);
        }

        public void Pause()
        {
            //throw new NotImplementedException();
        }

        public void Resume()
        {
            //throw new NotImplementedException();
        }

        public void Terminate()
        {
            //throw new NotImplementedException();
        }
    }

    internal static class dataProvider //we may want to use singleton here, just to be fancy
    {
        public static IGTApplication gtApp = GTClassFactory.Create<IGTApplication>();
        public static IGTTransactionManager transactionManager;
        public static IGTCustomCommandHelper ccHelper;
        static IGTDDCKeyObjects selectedObjects = gtApp.SelectedObjects.GetObjects();
        public static IGTDataContext gtDataContext = gtApp.DataContext;
        static readonly short FNO = 55;
        public static string ErrorMessage;
        public static Recordset PremiseRecords = new Recordset();
        static string toEmail = "joseph.lundy@hexagonsi.com";
        static string emailSubject = "GIS ESI Location Request";
        static string[] requiredColumns = { "HOUSE_NBR", "STREET_NM", "STREET_TYPE_C", "CITY_C", "ZIP_C" };
        public static readonly List<string> RequiredColumns = new List<string>(requiredColumns);
        static Recordset PremiseMetaRecords = new Recordset();
        public static Dictionary<string, List<string>> ColumnRules = new Dictionary<string, List<string>>(); 
        public enum MetaValue { DATA_TYPE, DATA_LEGNTH, DATA_PRECISION, DATA_SCALE};

        /// <summary>
        /// This function gathers all premises that should be displayed. Any premise w/out a number that 
        /// is attached to a selected service point or, if no service points are found, all premises without 
        /// numbers. This should get data in a format that cn be used in a data grid. 
        /// </summary>
        public static void LoadData() {
            try
            {
                IGTKeyObjects servicePoints = GTClassFactory.Create<IGTKeyObjects>();
                IGTJobHelper jobHelper = GTClassFactory.Create<IGTJobHelper>();
                selectedObjects = gtApp.SelectedObjects.GetObjects();
                string PremiseMetaDataQuery = "SELECT COLUMN_NAME, DATA_TYPE, DATA_LENGTH, DATA_PRECISION, DATA_SCALE FROM ALL_TAB_COLUMNS  WHERE TABLE_NAME = 'PREMISE_N'";
                PremiseMetaRecords = gtDataContext.OpenRecordset(PremiseMetaDataQuery, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, -1);
                PremiseMetaRecords.MoveFirst();
                while (!PremiseMetaRecords.EOF && PremiseMetaRecords.RecordCount > 0)
                {
                    string[] values = { PremiseMetaRecords.Fields["DATA_TYPE"].Value.ToString(),
                                    PremiseMetaRecords.Fields["DATA_LENGTH"].Value.ToString(),
                                    PremiseMetaRecords.Fields["DATA_PRECISION"].Value.ToString(),
                                    PremiseMetaRecords.Fields["DATA_SCALE"].Value.ToString()};
                    ColumnRules.Add(PremiseMetaRecords.Fields["COLUMN_NAME"].Value.ToString(), new List<string>(values));
                    PremiseMetaRecords.MoveNext();
                }
                //If we have objects selected
                if (selectedObjects.Count > 0)
                {//this code is a bit broken, instead of doing 1 call it calls multiple times.
                    List<int> FIDs = new List<int>();
                    int badObject = 0;
                    foreach (IGTDDCKeyObject x in selectedObjects)
                    {
                        if (x.FNO == FNO)
                        {
                            if (!FIDs.Contains(x.FID))//don't want this to trigger the else statement
                            {
                                FIDs.Add(x.FID);
                            }
                        }
                        else
                        {
                            badObject++;
                        }
                    }
                    //If we have Service Points selected
                    if (FIDs.Count > 0 && badObject == 0)
                    {
                        //LOGIC to detect service points and if they have premise records w/out ESI Locations
                        string FIDsString = string.Join(", ", FIDs);
                        //if we have service points
                        if (FIDs.Count > 0)
                        {
                            //LOGIC to detect service points and if they have premise records w/out ESI Locations
                            string PremiseQuery = "SELECT * FROM PREMISE_N WHERE G3E_FID IN (" + FIDsString + " ) AND PREMISE_NBR IS null";
                            PremiseRecords = gtDataContext.OpenRecordset(PremiseQuery, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, -1);
                            if (PremiseRecords.EOF && PremiseRecords.BOF)
                            {
                                ErrorMessage = "All selected Service Points have ESI Locations." ;
                            }
                        }
                    }
                    //Anything other than Service Points selected
                    else
                    {
                        ErrorMessage = "The select set contains features other than Service Points." + Environment.NewLine +  Environment.NewLine + "Before running the command select a Service Point or unselect all features to process all Service Points in the active Job.";
                    }
                }
                // If no objects are selected
                else
                {
                    Recordset edits;
                    edits = jobHelper.FindPendingEdits();
                    List<int> FIDs = new List<int>();
                    //if we have edits in the current job
                    if (edits != null && !edits.EOF)
                    {
                        edits.MoveFirst();
                        if (edits.Fields["G3E_FNO"].Value == FNO)
                        {//hard coding in column index
                            FIDs.Add(Convert.ToInt32(edits.Fields["G3E_FID"].Value.ToString()));
                            edits.MoveNext();
                        }
                        while (!edits.EOF)
                        {
                            if (edits.Fields["G3E_FNO"].Value == FNO)
                            {
                                FIDs.Add(Convert.ToInt32(edits.Fields["G3E_FID"].Value.ToString()));
                            }
                            edits.MoveNext();
                        }
                        //We need to make a string from our list of FIDS
                        string FIDsString = string.Join(", ", FIDs);
                        //if we have service points
                        if (FIDs.Count > 0)
                        {
                            //LOGIC to detect service points and if they have premise records w/out ESI Locations
                            string PremiseQuery = "SELECT * FROM PREMISE_N WHERE G3E_FID IN (" + FIDsString + " ) AND PREMISE_NBR IS null";
                            PremiseRecords = gtDataContext.OpenRecordset(PremiseQuery, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic, -1);
                            if (PremiseRecords.EOF && PremiseRecords.BOF)
                            {
                                ErrorMessage = "All Service Points have ESI Locations." ;
                            }
                        }
                        //no service points edited in job
                        else
                        {
                            ErrorMessage = "No Service Points in active job.";
                        }
                    }
                    else
                    {
                        ErrorMessage = "No Service Points in active job.";
                    }
                }
            }
            catch(Exception error)
            {
                MessageBox.Show("Error in LoadData (" + error.Message + ")", "G/Technology");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="edits"></param>
        public static void SaveData(System.Data.DataTable edits)
        {
            gtApp.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Saving edits…");
            try
            {
                foreach(System.Data.DataRow row in edits.Rows)
                {
                    transactionManager.Begin("Edit Feature");
                    IGTKeyObject editedFeature = gtDataContext.OpenFeature(FNO, Convert.ToInt32(row["G3E_FID"]));
                    IGTComponent editedComponenent = editedFeature.Components.GetComponent(Convert.ToInt16(row["G3E_CNO"]));
                    editedComponenent.Recordset.Filter = "G3E_CID = " + row["G3E_CID"].ToString();
                    editedComponenent.Recordset.MoveFirst();
                    foreach (Field field in editedComponenent.Recordset.Fields)
                    {
                        if (!DBNull.Value.Equals(field.Value) && field.Type == DataTypeEnum.adVarChar)
                        {
                            if (field.Value != row[field.Name].ToString())
                            {
                                field.Value = row[field.Name];
                            }
                        }
                        else if (!DBNull.Value.Equals(field.Value) && field.Type == DataTypeEnum.adNumeric)
                        {
                            if (field.Value != Convert.ToDecimal(row[field.Name]))
                            {
                                field.Value = row[field.Name];
                            }
                        }else if (!DBNull.Value.Equals(field.Value) && field.Type == DataTypeEnum.adDBTimeStamp)
                        {
                            if (field.Value != Convert.ToDateTime(row[field.Name]))
                            {
                                field.Value = row[field.Name];
                            }
                        }
                        else if (DBNull.Value.Equals(field.Value) && !DBNull.Value.Equals(row[field.Name]))
                        {
                            field.Value = row[field.Name];
                        }
                    }
                    transactionManager.Commit();
                    editedFeature = null;
                    gtApp.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");
                }
            }catch(Exception error)
            {
                transactionManager.Rollback();
                MessageBox.Show("Error in SaveData (" + error.Message + ")", "G/Technology");
            }
        }

        /// <summary>
        /// This function will take the Recordset(may change how this works) and convert it to an excel document. Then it will send it to the call center via email.
        /// </summary>
        public static void ExportData(DataGridView submital) {
            try
            {
                TempFileCollection tempFiles = new TempFileCollection();
                Microsoft.Office.Interop.Excel.Application Excel = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel._Workbook workbook = Excel.Workbooks.Add(Type.Missing);
                Worksheet worksheet = null;
                worksheet = workbook.Sheets["Sheet1"];
                worksheet = workbook.ActiveSheet;
                Microsoft.Office.Interop.Excel.Range cutRange;
                Microsoft.Office.Interop.Excel.Range insertRange;
                Microsoft.Office.Interop.Excel.Range deleteRange;
                

                for (int i = 1; i < submital.Columns.Count; i++)
                {
                    worksheet.Cells[1, i] = submital.Columns[i].HeaderText;
                }
                List<DataGridViewRow> rowsForSubmit = new List<DataGridViewRow>();
                System.Data.DataTable test1 = new System.Data.DataTable();
                foreach (DataGridViewColumn column in submital.Columns)
                {
                    test1.Columns.Add(column.Name);
                }
                int excelRowIndex = 2;
                foreach (DataGridViewRow row in submital.Rows)
                {
                    DataGridViewCheckBoxCell checkbox = row.Cells[0] as DataGridViewCheckBoxCell;
                    bool bChecked = ("" != checkbox.Value.ToString() && null != checkbox && null != checkbox.Value && true == (bool)checkbox.Value);
                    if (bChecked)
                    {
                        for (int i = 1; i < submital.Columns.Count - 2; i++)
                        {
                            row.Cells[2].Value = System.DateTime.Today;
                            if (row.Cells[i].Value != null)
                            {
                                worksheet.Cells[excelRowIndex, i] = row.Cells[i].Value.ToString();
                            }
                        }
                        excelRowIndex++;
                    }
                }

                deleteRange = worksheet.Range["B:B"];
                deleteRange.Delete(Microsoft.Office.Interop.Excel.XlDeleteShiftDirection.xlShiftToLeft);
                deleteRange = worksheet.Range["S:S"];
                deleteRange.Delete(Microsoft.Office.Interop.Excel.XlDeleteShiftDirection.xlShiftToLeft);
                cutRange = worksheet.Range["A:A"];
                insertRange = worksheet.Range["R:R"];
                insertRange.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftToRight, cutRange.Cut());

                //may need to temporarily save the file inorder to rename it to the proper name.
                workbook.SaveAs("C:\\SharedTest\\ESI_Location_Request_" + Environment.UserName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"));
                tempFiles.AddFile(workbook.Path,false);
                SendEmail(workbook.Name);
                Excel.Quit();
            }
            catch(Exception error)
            {
                MessageBox.Show("Error in ExportData (" + error.Message + ")", "G/Technology");
            }
        }

        /// <summary>
        /// Disposal method to clear lists and close streams
        /// </summary>
        public static void Dispose()
        {
            try
            {
                if (PremiseRecords.State != (int)ADODB.ObjectStateEnum.adStateClosed)
                {
                    PremiseRecords.Close();
                }
                PremiseMetaRecords.Close();
                ColumnRules.Clear();
                ErrorMessage = null;
            }
            catch(Exception error)
            {
                MessageBox.Show("Error in Dispose (" + error.Message + ")", "G/Technology");
            }
        }

        /// <summary>
        /// Method to display the feature attached to the selected premise record.
        /// </summary>
        /// <param name="FID"></param>
        /// <param name="FNO"></param>
        public static void SelectedServicePoint(int FID, short FNO)
        {
            try
            {
                gtApp.SelectedObjects.Clear();
                double zoomFactorSave = gtApp.ActiveMapWindow.SelectBehaviorZoomFactor;
                var selectBehaviorSave = gtApp.ActiveMapWindow.SelectBehavior;
                bool activeBehaviorSave = gtApp.ActiveMapWindow.ApplySelectBehaviorWhenActive;
                gtApp.ActiveMapWindow.ApplySelectBehaviorWhenActive = true;
                gtApp.ActiveMapWindow.SelectBehavior = GTSelectBehaviorConstants.gtmwsbHighlightAndCenter;
                gtApp.ActiveMapWindow.SelectBehaviorZoomFactor = 1.25;
                gtApp.ActiveMapWindow.Activate();
                gtApp.SelectedObjects.Add(GTSelectModeConstants.gtsosmSelectedComponentsOnly, gtApp.DataContext.GetDDCKeyObjects(FNO, FID, GTComponentGeometryConstants.gtddcgAllGeographic)[0]);
                gtApp.ActiveMapWindow.CenterSelectedObjects();
                gtApp.ActiveMapWindow.SelectBehaviorZoomFactor = zoomFactorSave;
                gtApp.ActiveMapWindow.SelectBehavior = selectBehaviorSave;
                gtApp.ActiveMapWindow.ApplySelectBehaviorWhenActive = activeBehaviorSave;
            }
            catch(Exception error)
            {
                MessageBox.Show("Error in SelectedServicePoint (" + error.Message + ")", "G/Technology");
            }
        }

        private static bool SendEmail(string fileName)
        {
            bool returnValue = false;

            try
            {
                string commandName = "Request ESI Location";
                string fromAddress = UserPrincipal.Current.EmailAddress;
                //needs to be a variable, all the way.
                string toAddress = "richard.gabrys@hexagonsi.com;" + UserPrincipal.Current.EmailAddress;
                string subject = "GIS ESI Location Request";
                string attachment = "\\\\LUNDY6800\\SharedTest\\Premise_Number_Request.xlsx";

                string message = "<eMailRequest>" +
                                 "<SendingApp>" + commandName + "</SendingApp>" +
                                 "<DateTimeSent>" + DateTime.Now + "</DateTimeSent>" +
                                 "<ToAddr>" + toAddress + "</ToAddr>" +
                                 "<FromAddr>" + fromAddress + "</FromAddr>" +
                                 "<Subject>" + subject + "</Subject>" +
                                 "<Message>test</Message>" +
                                 "<Attachments>" + attachment + "</Attachments>" +
                                 "</eMailRequest>";

                try
                {
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://uc-vm-oncor-03:8089/interface");
                    req.ContentType = "text/xml";
                    req.Method = "POST";

                    using (System.IO.Stream stm = req.GetRequestStream())
                    {
                        using (StreamWriter stmw = new StreamWriter(stm))
                        {
                            stmw.Write(message);
                        }
                    }

                    WebResponse response = req.GetResponse();
                    System.IO.Stream responseStream = response.GetResponseStream();

                    returnValue = true;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to send email: " + ex.Message);
                    returnValue = false;
                }
            }
            catch
            {
                MessageBox.Show("Unable to get current user's email address. Need to be on the network.");
                returnValue = false;
            }
            return returnValue;
        }

    }
}

