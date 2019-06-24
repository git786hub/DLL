using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using ADODB;
using Intergraph.GTechnology.API;
using System.Collections;

namespace ccImportAttachCSV
{
    /// <summary>
    /// Houses all the variables, class instances & functions that the command requires
    /// </summary>
    static class csGlobals
    {
        internal static StreamReader CSVStream;
        internal static StreamWriter CSVOutStream;
        internal static OpenFileDialog ofCsv;
        internal static SaveFileDialog svCsv;
        internal static Form frmTmpAttachments;
        internal static Boolean ValidatePassFail = false;
        internal static string csvFile;
        internal static IGTDataContext gDataCont;
        internal static IGTApplication gApp;
        internal static Boolean gCanTerm = true;
        internal static IGTTransactionManager gTransMgr;
        internal static Recordset gColDataRs;
        internal static IGTCustomCommandHelper gCcHelper;
        internal static string gHeaderRowOfCSV;
        internal static Recordset gCompanyPlRs = null;

        /// <summary>
        /// Loads the data from the CSV file into a DataGridView and closes the stream
        /// </summary> 
        internal static void loadAttachGrid()
        {
            string tmpStr;
            string[] tmpStrArr;
            int colCount = 0;
            DataGridView tmpGrid;
            try
            {

                tmpGrid = (DataGridView)frmTmpAttachments.Controls["dataGridView1"];
                tmpGrid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                tmpGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                colCount = tmpGrid.Columns.Count;
                //delete all columns in the grid.
                for (int i = colCount - 1; i >= 0; i = i - 1)
                {
                    tmpGrid.Columns.RemoveAt(i);
                }

                // read header line. Add columns to the grid. 
                tmpStr = CSVStream.ReadLine();
                csGlobals.gHeaderRowOfCSV = tmpStr;
                tmpStrArr = tmpStr.Split(',');
                colCount = tmpStrArr.Length;

                for (int i = 0; i < colCount; ++i)
                {   
                    tmpGrid.Columns.Add(tmpStrArr[i].ToString().Trim(), tmpStrArr[i].ToString().Trim());
                }

                tmpStr = CSVStream.ReadLine();

                while (tmpStr != null)
                {
                    tmpStrArr = tmpStr.Split(',');
                    tmpStrArr[2] = tmpStrArr[2].ToUpper();
                    tmpGrid.Rows.Add(tmpStrArr);


                    tmpStr = CSVStream.ReadLine();
                }

                CSVStream.Close();
                CSVStream.Dispose();
            }
            catch (Exception e)
            {
                MessageBox.Show(gApp.ApplicationWindow,"loadAttachGrid:" + e.Message, "Import Attachments - Error",
                                MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Updates a Component for a given feature. The Data from the component is passed into the Function as a DataGridViewRow 
        /// </summary>
        /// <param name="row">DataGridViewRow</param>
        /// <param name="feature">IGTKeyObject</param>
        /// <param name="fid">int</param>
        /// <param name="fno">short</param>
        /// <param name="wirelineOrEquipment">string</param>
        /// <returns></returns>
        internal static Boolean updateComponent(DataGridViewRow row, 
                                                int fid, short fno,int cid,
                                                string p_CompCd,
                                                string p_Maint1,
                                                string p_Maint2,
                                                string wirelineOrEquipment)
        {
            IGTKeyObject feature = null;
            Boolean tmpRetVal = true;

            try
            {
                tmpRetVal = true;
                // Begin the transaction
                gTransMgr.Begin("Update Feature");
                // Open the structure feature.
                feature = gDataCont.OpenFeature(fno, fid);
                // get to the correct attachment component and find the correct attachment record.
                feature.Components[wirelineOrEquipment].Recordset.Filter = "G3E_CID = " + cid;
                feature.Components[wirelineOrEquipment].Recordset.MoveFirst();

                // update the record.
                if (wirelineOrEquipment.Equals("ATTACH_WIRELINE_N"))
                {
                    object[] tmpCols = { "G3E_FID", "G3E_FNO", "W_ATTACH_COMPANY",
                                         "W_MAINTAINER_1", "W_MAINTAINER_2", "W_PERMIT_NUMBER",
                                         "W_ATTACH_HEIGHT_FT", "W_ATTACHMENT_STATUS",
                                         "W_ATTACH_SOURCE_C", "W_MESSENGER_C", "W_ATTACH_TYPE", "W_ATTACH_POSITION" };
                    object[] tmpVals = { fid, fno, p_CompCd, p_Maint1, p_Maint2,
                                         row.Cells["Permit Number"].Value,
                                         row.Cells["Attachment Height"].Value,
                                         row.Cells["Attachment Status"].Value.ToString(),
                                         row.Cells["Attachment Source"].Value.ToString(),
                                         row.Cells["Messenger"].Value.ToString(),
                                         row.Cells["Attachment Type"].Value.ToString(),
                                         row.Cells["Attachment Position"].Value.ToString() };

                    feature.Components[wirelineOrEquipment].Recordset.Update(tmpCols, tmpVals);

                    if (row.Cells["Initial String Tension (lbs)"].Value == null || 
                        row.Cells["Initial String Tension (lbs)"].Value.ToString().Equals("") || 
                        row.Cells["Initial String Tension (lbs)"].Value.ToString().Equals(" "))
                    {
                        feature.Components[wirelineOrEquipment].Recordset.Fields["W_INIT_STR_TENSION"].Value = DBNull.Value;
                    }
                    else
                    {
                        feature.Components[wirelineOrEquipment].Recordset.Fields["W_INIT_STR_TENSION"].Value = 
                                   Convert.ToInt16( row.Cells["Initial String Tension (lbs)"].Value.ToString());
                    }

                    if(row.Cells["Outside Diameter (inches)"].Value.ToString() == null ||
                       row.Cells["Outside Diameter (inches)"].Value.ToString().Trim() == "")
                    {
                        feature.Components[wirelineOrEquipment].Recordset.Fields["W_OUTSIDE_DIAM"].Value = DBNull.Value;
                    }
                    else
                    {
                        feature.Components[wirelineOrEquipment].Recordset.Fields["W_OUTSIDE_DIAM"].Value = 
                                    Convert.ToInt16( row.Cells["Outside Diameter (inches)"].Value.ToString());
                    }
                }
                else
                {
                    feature.Components[wirelineOrEquipment].Recordset.MoveFirst();
                    object[] tmpCols = { "G3E_FID", "G3E_FNO", "E_ATTACH_COMPANY", "E_MAINTAINER_1", "E_MAINTAINER_2", "E_PERMIT_NUMBER", "E_ATTACH_HEIGHT_FT", "E_ATTACHMENT_STATUS", "E_ATTACH_SOURCE_C", "E_ATTACH_TYPE_C", "E_ATTACH_POSITION_C", "E_BRACKET_ARM" };
                    object[] tmpVals = { fid, fno,p_CompCd, p_Maint1,p_Maint2, row.Cells["Permit Number"].Value.ToString(),
                                         row.Cells["Attachment Height"].Value.ToString(),
                                         row.Cells["Attachment Status"].Value.ToString(),
                                         row.Cells["Attachment Source"].Value.ToString(),
                                         row.Cells["Attachment Type"].Value.ToString(),
                                         row.Cells["Attachment Position"].Value.ToString(),
                                         row.Cells["Bracket"].Value.ToString() };

                    feature.Components[wirelineOrEquipment].Recordset.Update(tmpCols, tmpVals);
                    if (row.Cells["Weight (lbs/ft)"].Value.ToString().Equals("") || row.Cells["Weight (lbs/ft)"].Value.ToString().Equals(" ") || row.Cells["Weight (lbs/ft)"].Value == null)
                    {

                        feature.Components[wirelineOrEquipment].Recordset.Fields["E_WEIGHT"].Value = DBNull.Value;
                    }
                    else
                    {
                        feature.Components[wirelineOrEquipment].Recordset.Fields["E_WEIGHT"].Value = 
                          Convert.ToInt16(row.Cells["Weight (lbs/ft)"].Value.ToString());
                    }
                }
                // commit the transaction.
                gTransMgr.Commit();
                row.Cells["Process Status"].Value = "Existing Attachment - Updated";
            }
            catch (Exception e)
            {
                tmpRetVal = false;
                gTransMgr.Rollback();
                row.Cells["Process Status"].Value = "Error while updating Attachment: " + e;
               // MessageBox.Show(gApp.ApplicationWindow, "updateComponent:" + e.Message, "Import Attachments - Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            return tmpRetVal;
        }

        /// <summary>
        /// Adds a Component for a given feature. The Data from the component is passed into the Function as a DataGridViewRow 
        /// </summary>
        /// <param name="row">DataGridViewRow</param>
        /// <param name="feature">IGTKeyObject</param>
        /// <param name="fid">int</param>
        /// <param name="fno">fno</param>
        /// <param name="wirelineOrEquipment">string</param>
        /// <returns></returns>
        internal static Boolean addComponent(DataGridViewRow row, 
                                             int fid, 
                                             short fno,
                                             string p_CompCd,
                                             string p_Maint1,
                                             string p_Maint2, 
                                             string wirelineOrEquipment)
        {
            Boolean tmpRetVal;
            try
            {
                tmpRetVal = true;
                IGTKeyObject feature = null;

                // Begin the transaction.
                gTransMgr.Begin("Create Attachment");
                // Open the feature to edit.
                feature = gDataCont.OpenFeature(fno, fid);

                // Add the attachment record.
                if (wirelineOrEquipment.Equals("ATTACH_WIRELINE_N"))
                {
                    object[] tmpCols = { "G3E_FID", "G3E_FNO", "W_ATTACH_COMPANY", "W_MAINTAINER_1", "W_MAINTAINER_2",
                                         "W_PERMIT_NUMBER", "W_ATTACH_HEIGHT_FT",
                                         "W_ATTACHMENT_STATUS", "W_ATTACH_SOURCE_C",
                                         "W_MESSENGER_C", "W_ATTACH_TYPE",
                                         "W_ATTACH_POSITION" };

                    object[] tmpVals = { fid, fno, p_CompCd, p_Maint1, p_Maint2,
                                         row.Cells["Permit Number"].Value.ToString(), row.Cells["Attachment Height"].Value.ToString(),
                                         row.Cells["Attachment Status"].Value.ToString(), row.Cells["Attachment Source"].Value.ToString(),
                                         row.Cells["Messenger"].Value.ToString(), row.Cells["Attachment Type"].Value.ToString(),
                                         row.Cells["Attachment Position"].Value.ToString() };

                    feature.Components[wirelineOrEquipment].Recordset.AddNew(tmpCols, tmpVals);
                    if (row.Cells["Initial String Tension (lbs)"].Value == null || row.Cells["Initial String Tension (lbs)"].Value.ToString().Equals("") || row.Cells["Initial String Tension (lbs)"].Value.ToString().Equals(" "))
                    {
                        feature.Components[wirelineOrEquipment].Recordset.Fields["W_INIT_STR_TENSION"].Value = DBNull.Value;
                    }
                    else
                    {
                        feature.Components[wirelineOrEquipment].Recordset.Fields["W_INIT_STR_TENSION"].Value = 
                                          Convert.ToInt16(row.Cells["Initial String Tension (lbs)"].Value.ToString());
                    }

                    if (row.Cells["Outside Diameter (inches)"].Value == null || row.Cells["Outside Diameter (inches)"].Value.ToString().Equals("") || row.Cells["Outside Diameter (inches)"].Value.ToString().Equals(" "))
                    {
                        feature.Components[wirelineOrEquipment].Recordset.Fields["W_OUTSIDE_DIAM"].Value = DBNull.Value;
                    }
                    else
                    {
                        feature.Components[wirelineOrEquipment].Recordset.Fields["W_OUTSIDE_DIAM"].Value =
                                          Convert.ToInt16(row.Cells["Outside Diameter (inches)"].Value.ToString());
                    }
                }
                else
                {
                    object[] tmpCols = { "G3E_FID", "G3E_FNO", "E_ATTACH_COMPANY", "E_MAINTAINER_1",
                                          "E_MAINTAINER_2", "E_PERMIT_NUMBER", "E_ATTACH_HEIGHT_FT",
                                          "E_ATTACHMENT_STATUS", "E_ATTACH_SOURCE_C", "E_ATTACH_TYPE_C",
                                          "E_ATTACH_POSITION_C", "E_BRACKET_ARM" };
                    object[] tmpVals = { fid, fno, p_CompCd, p_Maint1, p_Maint2,
                                         row.Cells["Permit Number"].Value.ToString(),
                                         row.Cells["Attachment Height"].Value.ToString(),
                                         row.Cells["Attachment Status"].Value.ToString(),
                                         row.Cells["Attachment Source"].Value.ToString(),
                                         row.Cells["Attachment Type"].Value.ToString(),
                                         row.Cells["Attachment Position"].Value.ToString(),
                                         row.Cells["Bracket"].Value.ToString() };
                    feature.Components[wirelineOrEquipment].Recordset.AddNew(tmpCols, tmpVals);

                    if (row.Cells["Weight (lbs/ft)"].Value.ToString().Equals("") || row.Cells["Weight (lbs/ft)"].Value.ToString().Equals(" ") || row.Cells["Weight (lbs/ft)"].Value == null)
                    {
                        feature.Components[wirelineOrEquipment].Recordset.Fields["E_WEIGHT"].Value = null;
                    }
                    else
                    {
                        feature.Components[wirelineOrEquipment].Recordset.Fields["E_WEIGHT"].Value = row.Cells["Weight (lbs/ft)"].Value;
                    }
                }
                // commit the transaction.
                gTransMgr.Commit();
                row.Cells["Process Status"].Value = "Added";
            }
            catch (Exception e)
            {
                tmpRetVal = false;
                gTransMgr.Rollback();
                row.Cells["Process Status"].Value = "Error while adding Attachment: " + e;
               // MessageBox.Show(gApp.ApplicationWindow, "addComponent:" + e.Message, "Import Attachments - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return tmpRetVal;
        }

        /// <summary>
        ///  Validates the Header row of the CSV file according the rules specified in the documentation.
        /// </summary>
        /// <param name="dataGrid">DataGridView</param>
        /// <param name="theForm">Form</param>
        /// <returns>Returns a bool</returns>
        internal static bool validateHeader(DataGridView dataGrid, Form theForm)
        {
            try
            {
                // get a list of the columns defined in the JU_CSVIMPORT_FORMAT table
                string SQL = "select CSV_COLUMN_NAME from JU_CSVIMPORT_FORMAT order by COLUMN_ORDINAL";
                Recordset CSVColumnsConfig = csGlobals.ExecuteQuery(csGlobals.gDataCont, SQL);
                // Get the header row from the csv file.
                string[] tmpStrArr = csGlobals.gHeaderRowOfCSV.Split(','); // store in the header row in a string array
                if (CSVColumnsConfig.RecordCount != tmpStrArr.Length) // error b/c the number of columns in csv file don't match the config
                {
                    DataGridViewRow row = (DataGridViewRow)dataGrid.Rows[0].Clone();
                    foreach (string s in tmpStrArr)
                    {
                        row.Cells[0].Value = row.Cells[0].Value + "|" + s;
                    }
                    CSVColumnsConfig.MoveFirst();

                    while (!CSVColumnsConfig.EOF)
                    {
                        row.Cells[1].Value = row.Cells[1].Value + "|" + CSVColumnsConfig.Fields["CSV_COLUMN_NAME"].Value;
                        CSVColumnsConfig.MoveNext();
                    }
                    // display the error saying the number of columns do not match.
                    row.Cells[2].Value = "Invalid Columns- There are more or fewer columns in the CSV file that there should be";
                    theForm.Controls["lblMsg"].Text = "Invalid Columns- There are more or fewer columns in the CSV file that there should be";
                    dataGrid.Rows.Add(row);

                    return false;
                }
                else
                {
                    // check to see that the names of corresponding columns match.
                    CSVColumnsConfig.MoveFirst();
                    foreach (string s in tmpStrArr)
                    {
                        if (!(s.Trim().Equals(CSVColumnsConfig.Fields["CSV_COLUMN_NAME"].Value.ToString().Trim()))) //The column names don't match 
                        {
                            DataGridViewRow row = (DataGridViewRow)dataGrid.Rows[0].Clone();
                            foreach (string str in tmpStrArr)
                            {
                                row.Cells[0].Value = row.Cells[0].Value + "|" + str;
                            }
                            CSVColumnsConfig.MoveFirst();

                            while (!CSVColumnsConfig.EOF)
                            {
                                row.Cells[1].Value = row.Cells[1].Value + "|" + CSVColumnsConfig.Fields["CSV_COLUMN_NAME"].Value;
                                CSVColumnsConfig.MoveNext();
                            }

                            row.Cells[2].Value = "Invalid Columns- Column names do not match";
                            theForm.Controls["lblMsg"].Text = "Invalid Columns- Column names do not match";
                            dataGrid.Rows.Add(row);


                            return false;

                        }

                        CSVColumnsConfig.MoveNext();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(gApp.ApplicationWindow, "validateHeader" + e.Message, "Import Attachments - Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
            return true;
        }

        /// <summary>
        /// Returns the required columns according to the JU_CSV_IMPORT table as per the record type (WIRELINE or EQUIPMENT)
        /// </summary>
        /// <param name="recordType">string</param>
        /// <returns>Returns an array of strings that contain the required columns according to the recordType</returns>
        internal static string[] getRequiredColumns(string recordType)
        {
            List<string> reqColumns = new List<string>();
            try
            {
                if (recordType.ToUpper() == "EQUIPMENT")
                {
                    string SQL = "select CSV_COLUMN_NAME, EQUIPMENT_VALUE_LIST from JU_CSVIMPORT_FORMAT where REQUIRED_CODE in ('Y', 'E')";
                    
                    gColDataRs= ExecuteQuery(csGlobals.gDataCont, SQL);
                    gColDataRs.MoveFirst();

                    while (!gColDataRs.EOF)
                    {
                        reqColumns.Add(gColDataRs.Fields["CSV_COLUMN_NAME"].Value.ToString());
                        gColDataRs.MoveNext();
                    }
                }

                if (recordType.ToUpper() == "WIRELINE")
                {
                    string SQL = "select CSV_COLUMN_NAME, VALUE_LIST from JU_CSVIMPORT_FORMAT where REQUIRED_CODE in ('Y', 'W')";
                    
                    gColDataRs = ExecuteQuery(csGlobals.gDataCont, SQL);
                    gColDataRs.MoveFirst();
                    while (!gColDataRs.EOF)
                    {
                        reqColumns.Add(gColDataRs.Fields["CSV_COLUMN_NAME"].Value.ToString());
                        gColDataRs.MoveNext();
                    }
                }

                if (recordType.ToUpper() == "DEFAULT")
                {
                    string SQL = "select CSV_COLUMN_NAME, VALUE_LIST from JU_CSVIMPORT_FORMAT " +  
                                    "where REQUIRED_CODE in ('Y') and GTECH_WIRELINE_FIELD is null " + 
                                    "and GTECH_EQUIPMENT_FIELD is null ";

                   
                    gColDataRs = ExecuteQuery(csGlobals.gDataCont, SQL);
                    gColDataRs.MoveFirst();

                    while (!gColDataRs.EOF)
                    {
                        reqColumns.Add(gColDataRs.Fields["CSV_COLUMN_NAME"].Value.ToString());
                        gColDataRs.MoveNext();
                    }
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(gApp.ApplicationWindow, "getRequiredColumns: " + e.Message, "Import Attachments - Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
            return reqColumns.ToArray();
        }

        /// <summary>
        ///  Validates the Grid Data according to rules specified in the documentation
        /// </summary>
        /// <param name="inGrid">DataGridViewRow</param>
        /// <returns>Boolean</returns>
        internal static Boolean ValidateGridData(DataGridView inGrid)
        {
            Boolean returnVal = true;
            string discString = "";
            string[] reqCols = null;
            Boolean passed = true;
            int failedCount = 0;
            string tmpFilterStr = string.Empty;

            try {

                returnVal = true;
                // validate that data exists in the required fields for each row in the csv file.
                foreach (DataGridViewRow row in inGrid.Rows)
                {
                    passed = true;
                    if (row.Cells["Record Type"].Value.ToString().ToUpper() == "WIRELINE") //Validation for the wireline attachment 
                    {
                        reqCols = getRequiredColumns(row.Cells["Record Type"].Value.ToString());

                        foreach (string reqColumn in reqCols)
                        {
                            if (row.Cells[reqColumn].Value.Equals("") || row.Cells[reqColumn].Value.Equals(null))
                            {
                                discString += reqColumn + ":" + "Missing Required Data";
                                passed = false;
                                failedCount++;
                            }
                            else
                            {
                                if (reqColumn.Equals("Structure ID"))
                                {
                                    if (row.Cells[reqColumn].Value.ToString().Length > 16)
                                    {
                                        discString += reqColumn + ": is Invalid";
                                        passed = false;
                                        failedCount++;
                                    }
                                }

                                if (reqColumn == "Attaching Company" || reqColumn == "Maintainer 1")
                                {
                                    if (row.Cells[reqColumn].Value.Equals("") || row.Cells[reqColumn].Value.Equals(null))
                                    {
                                        discString += reqColumn + ":" + "Missing Required Data";
                                        passed = false;
                                        failedCount++;
                                    }
                                    string tmpStr = gGetAttachingCompany(row.Cells[reqColumn].Value.ToString());
                                    if (tmpStr == "")
                                    {
                                        discString += reqColumn + ":" + "Could not find Company Code in Attaching Company Picklist.";
                                        passed = false;
                                        failedCount++;
                                    }
                                }

                                tmpFilterStr = "CSV_COLUMN_NAME = '" + reqColumn + "'";
                                gColDataRs.Filter = tmpFilterStr;
                                gColDataRs.MoveFirst();

                                if (gColDataRs.Fields["VALUE_LIST"].Value.ToString() != "")
                                {
                                    if (!(gColDataRs.Fields["VALUE_LIST"].Value.ToString().ToUpper().Contains(row.Cells[reqColumn].Value.ToString().ToUpper())))
                                    {
                                        discString += reqColumn + ": value of " + reqColumn + " is not contained in the picklist";
                                        passed = false;
                                        failedCount++;
                                    }
                                }
                            }
                        }
                    }
                    else if (row.Cells["Record Type"].Value.ToString().ToUpper()== "EQUIPMENT") //Validation for the equipment attachment 
                    {
                        reqCols = getRequiredColumns(row.Cells["Record Type"].Value.ToString());
                        foreach (string reqColumn in reqCols)
                        {
                            if (row.Cells[reqColumn].Value.Equals("") || row.Cells[reqColumn].Value.Equals(null))
                            {
                                discString += reqColumn + ":" + "Missing Required Data";
                                passed = false;
                                failedCount++;
                            }
                            else
                            {
                                if (reqColumn.Equals("Structure ID"))
                                {
                                    if (row.Cells[reqColumn].Value.ToString().Length > 16)
                                    {
                                        discString += reqColumn + ": is Invalid";
                                        passed = false;
                                        failedCount++;
                                    }
                                }

                                if (reqColumn == "Attaching Company" || reqColumn == "Maintainer 1" )
                                {
                                    if (row.Cells[reqColumn].Value.Equals("") || row.Cells[reqColumn].Value.Equals(null))
                                    {
                                        discString += reqColumn + ":" + "Missing Required Data";
                                        passed = false;
                                        failedCount++;
                                    }
                                    string tmpStr = gGetAttachingCompany(row.Cells[reqColumn].Value.ToString());
                                    if (tmpStr == "")
                                    {
                                        discString += reqColumn + ":" + "Could not find Company Code in Attaching Company Picklist.";
                                        passed = false;
                                        failedCount++;
                                    }
                                }

                                tmpFilterStr = "CSV_COLUMN_NAME = '" + reqColumn + "'";
                                gColDataRs.Filter = tmpFilterStr;
                                gColDataRs.MoveFirst();

                                if (gColDataRs.Fields["EQUIPMENT_VALUE_LIST"].Value.ToString() != "")
                                {
                                    if (!(gColDataRs.Fields["EQUIPMENT_VALUE_LIST"].Value.ToString().ToUpper().Contains(row.Cells[reqColumn].Value.ToString().ToUpper())))
                                    {
                                        discString += reqColumn + ": value of " + reqColumn + " is not contained in the picklist";
                                        passed = false;
                                        failedCount++;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        discString += "Record Type: Value of Record Type is not contained in the picklist: record type must be provided to continue";
                        row.Cells["Process Status"].Value = discString;
                        passed = false;
                        failedCount++;

                    }

                    if (passed)
                    {
                        row.Cells["Process Status"].Value = "Valid";
                    }
                    else
                    {
                        row.Cells["Process Status"].Value = discString;
                    }

                    discString = "";
                }

                returnVal = passed;

            }
            catch (Exception e)
            {
                MessageBox.Show(gApp.ApplicationWindow, "ValidateGridData: " + e.Message, "Import Attachments - Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (failedCount == 0) failedCount =  1;
            }


            if (failedCount > 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }


        /// <summary>
        /// Saves the grid data to a CSV file.
        /// </summary>
        /// <param name="inGrid">DataGridView</param>
        static internal void saveCSVFile(DataGridView inGrid)
        {
            string tmpPath;
            string saveFileName = string.Empty;
            DataGridViewRowCollection tmpRows;
            string tmpString = String.Empty;
            DialogResult msgBoxResult = DialogResult.None;


            try
            {
                tmpRows = inGrid.Rows;

                tmpPath = csvFile;
                tmpPath = tmpPath.Remove(tmpPath.Length - 4);
                tmpPath = tmpPath + "_log.csv";

                //does the file exist?
                if (File.Exists(tmpPath))
                {
                    msgBoxResult = MessageBox.Show(gApp.ApplicationWindow, "The file " + tmpPath + " already exists. \n Do you wish to overwrite it?",
                                                   "Warning - File Exists",
                                                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (msgBoxResult == DialogResult.No)
                    {
                        svCsv.InitialDirectory = Path.GetDirectoryName(csvFile);
                        svCsv.FileName = Path.GetFileName(tmpPath);
                        svCsv.Filter = "CSV File (*.CSV)|*.CSV|Text File (*.txt)|*.txt";
                        svCsv.Title = "Save As";

                        if (svCsv.ShowDialog() == DialogResult.OK)
                        {
                            tmpPath = svCsv.FileName;
                        }
                        else
                        {
                            return;
                        }

                    }
                }

                CSVOutStream = new StreamWriter(tmpPath);

                for (int i = 0; i < inGrid.Columns.Count; ++i)
                {
                    tmpString = tmpString + inGrid.Columns[i].HeaderText + ",";
                }
                tmpString = tmpString.Remove(tmpString.Length - 1);
                CSVOutStream.WriteLine(tmpString);

                for (int i = 0; i < tmpRows.Count; ++i)
                {
                    tmpString = String.Empty;
                    for (int j = 0; j < tmpRows[i].Cells.Count; ++j)
                    {
                        if (tmpRows[i].Cells[j].Value == null)
                        {
                            tmpString = tmpString + ",";
                        }
                        else
                        {
                            tmpString = tmpString + tmpRows[i].Cells[j].Value + ",";
                        }
                    }

                    tmpString = tmpString.Remove(tmpString.Length - 1);
                    CSVOutStream.WriteLine(tmpString);
                }

                CSVOutStream.Flush();
                CSVOutStream.Close();
                CSVOutStream.Dispose();

            }
            catch (Exception e)
            {
                MessageBox.Show(gApp.ApplicationWindow, "saveCSVFile: " + e.Message, "Import Attachments - Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// simple function that executes an SQL query
        /// </summary>
        /// <param name="DataContext">IGTDataContext</param>
        /// <param name="SQL_String">String</param>
        /// <returns></returns>
        public static Recordset ExecuteQuery(IGTDataContext DataContext, String SQL_String)
        {
            Recordset oRS = null;
            int rEffected = -1;
            try
            {
                oRS = DataContext.Execute(SQL_String, out rEffected, (int)CommandTypeEnum.adCmdText, null);
            }
            catch (Exception e)
            {
                MessageBox.Show(gApp.ApplicationWindow, "ExecuteQuery: " + e.Message, "Import Attachments - Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            return oRS;
        }

        /// <summary>
        /// Creates or updates the attachment for the given data in the GridRow
        /// </summary>
        /// <param name="theGrid">DataGridView</param>
        /// <param name="theForm">Form</param>
        /// <returns></returns>
        static internal Boolean CreateAttachment(DataGridView theGrid, Form theForm)
        {

            int openFeatureCount = 0;
            Boolean returnVal = true;
            Boolean added = false;
            Boolean updated = false;
            Boolean addedAll = true;
            IGTKeyObject roFeature = null;
            string compPreFix = string.Empty;
            string compSuffixFix = string.Empty;
            int fid = 0;
            short fno = 0;
            int cid = 0;
            Recordset AttachRS = null;
            string tmpCompCd = string.Empty;
            string tmpMaint1 = string.Empty;
            string tmpMaint2 = string.Empty;

            try
            {
                foreach (DataGridViewRow row in theGrid.Rows)
                {
                    added = false;
                    updated = false;
                    //Process rows in the grid where the Process Status field is Valid.
                    if (row.Cells["Process Status"].Value.ToString().ToUpper() == "VALID")
                    {
                        // get company codes for the row
                        tmpCompCd = gGetAttachingCompany(row.Cells["Attaching Company"].Value.ToString());
                        tmpMaint1 = gGetAttachingCompany(row.Cells["Maintainer 1"].Value.ToString());
                        if (tmpCompCd == "" || tmpMaint1 == "")
                        {
                            if (tmpCompCd == "") row.Cells["Process Status"].Value = "Error: Attaching Company is invalid.";
                            if (tmpMaint1 == "") row.Cells["Process Status"].Value = row.Cells["Process Status"].Value + " Error: Attaching Company is invalid.";
                            continue;
                        }

                        if (row.Cells["Maintainer 2"].Value.ToString() != "")
                        { 
                            tmpMaint1 = gGetAttachingCompany(row.Cells["Maintainer 2"].Value.ToString());
                        }

                        // Find the pole or street light standard the has a matching Structure_id as the Structure Id in the current Grid Row
                        string SQL = "select * from common_n where structure_id = '" + row.Cells["Structure ID"].Value.ToString() +
                                    "' and feature_state_c in ('PPI', 'INI', 'CLS', 'ABI', 'PPR', 'PPA')" +
                                    "and g3e_fno in (110, 114)";
                        Recordset rs = ExecuteQuery(csGlobals.gDataCont, SQL);

                        // update the Process Status field in the grid if there is no match or 
                        //        if there is more than one match and go to the next row.
                        if (rs.RecordCount == 0 || rs.RecordCount > 1)
                        {
                            if (rs.RecordCount == 0)
                            {
                                row.Cells["Process Status"].Value = " Record not processed. – Structure is in the wrong state or it does not exist ";
                            }
                            if (rs.RecordCount > 1)
                            {
                                row.Cells["Process Status"].Value = " Record not processed. – More than one Structure exists with the same Structure Id.";
                            }
                        }
                        else // If one feature is found...
                        {
                            rs.MoveFirst();
                            // store the FNO and FID of the feature.
                            fid = Convert.ToInt32(rs.Fields["g3e_fid"].Value.ToString());
                            fno = Convert.ToInt16(rs.Fields["g3e_fno"].Value.ToString());

                            // Get the correct Attachment type.
                            openFeatureCount++;
                            string wirelineOrEquipment = "";

                            //assign table type accordingly 
                            switch (row.Cells["Record Type"].Value.ToString())
                            {
                                case "WIRELINE":
                                    wirelineOrEquipment = "ATTACH_WIRELINE_N";
                                    compPreFix = "W_";
                                    compSuffixFix = "";
                                    break;
                                case "EQUIPMENT":
                                    wirelineOrEquipment = "ATTACH_EQPT_N";
                                    compPreFix = "E_";
                                    compSuffixFix = "_C";
                                    break;
                            }

                            //Open the feature in read only mode.
                            roFeature = gDataCont.OpenFeature(fno, fid);
                            // get the correct attachment component 
                            AttachRS = roFeature.Components[wirelineOrEquipment].Recordset;

                            // If there are no records in the component add the attachment.
                            if (AttachRS.RecordCount < 1)
                            {

                                added = addComponent(row, fid, fno,tmpCompCd,tmpMaint1,tmpMaint2, wirelineOrEquipment);
                                if (!added) addedAll = false;
                                // go to the next row in the grid.
                                continue;
                            }

                            // If there are records in the Record set...
                            if (AttachRS.RecordCount > 0)
                            {
                                // see if a record/s in the component match the current record in the grid.
                                AttachRS.Filter = compPreFix + "ATTACH_COMPANY = '" + tmpCompCd
                                    + "' AND " + compPreFix + "ATTACH_TYPE" + compSuffixFix + " ='" + row.Cells["Attachment Type"].Value.ToString() + "'";

                                if (AttachRS.RecordCount == 0) // if no matches, add the attachment.
                                {
                                    // add a record
                                    added = addComponent(row, fid, fno, tmpCompCd, tmpMaint1, tmpMaint2, wirelineOrEquipment);
                                    if (!added) addedAll = false;
                                }
                                else if (AttachRS.RecordCount == 1) // if one match, update the attachmemt.
                                {
                                    string PositionField = compPreFix + "ATTACH_POSITION" + compSuffixFix;
                                    AttachRS.MoveFirst();
                                    if (AttachRS.Fields[PositionField].Value.ToString() == row.Cells["Attachment Position"].Value.ToString())
                                    {
                                        cid = Convert.ToInt32(AttachRS.Fields["G3E_CID"].Value.ToString());
                                        updated = updateComponent(row, fid, fno, cid, tmpCompCd, tmpMaint1, tmpMaint2, wirelineOrEquipment);
                                        if (!updated) addedAll = false;
                                    }
                                    else
                                    {
                                        added = addComponent(row, fid, fno, tmpCompCd, tmpMaint1, tmpMaint2, wirelineOrEquipment);
                                        if (!added) addedAll = false;
                                    }
                                }
                                else if (AttachRS.RecordCount > 1) // if more than one match, refine the search and try again.
                                {
                                    AttachRS.Filter = compPreFix + "ATTACH_COMPANY = '" + tmpCompCd
                                                + "' AND " + compPreFix + "ATTACH_TYPE" + compSuffixFix + " = '" + row.Cells["Attachment Type"].Value.ToString()
                                                + "' AND " + compPreFix + "ATTACH_POSITION" + compSuffixFix + " = '" + row.Cells["Attachment Position"].Value.ToString() + "'";

                                    if (AttachRS.RecordCount == 0) // if no matches, add the attachment.
                                    {
                                        // add a record
                                        added = addComponent(row, fid, fno, tmpCompCd, tmpMaint1, tmpMaint2, wirelineOrEquipment);
                                        if (!added) addedAll = false;
                                    }
                                    else if (AttachRS.RecordCount == 1) // if one match, update the record.
                                    {
                                        AttachRS.MoveFirst();
                                        cid = Convert.ToInt32(AttachRS.Fields["G3E_CID"].Value.ToString());
                                        updated = updateComponent(row, fid, fno, cid,tmpCompCd,tmpMaint1,tmpMaint2, wirelineOrEquipment);
                                        if (!updated) addedAll = false;
                                    }
                                    else // if more than one record, update the Process Status field. 
                                    {
                                        row.Cells["Process Status"].Value = "Attachment – Could not match attachment.";
                                        addedAll = false;
                                    }

                                }
                                else
                                {
                                    addedAll = false;
                                }

                            }
                        }
                    }

                    if (AttachRS != null)
                    {
                        AttachRS.Close();
                        AttachRS = null;
                    }

                    if (roFeature != null) roFeature = null;

                    cid = 0;
                    fno = 0;
                    fid = 0;
                }
 
                if (addedAll) 
                {
                    theForm.Controls["lblMsg"].Text = "Processed Records Sucessfully";
                }
                else
                {
                    theForm.Controls["lblMsg"].Text = "Processed Records With Errors.";
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(gApp.ApplicationWindow, "CreateAttachment: " + e.Message, "Import Attachments - Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (AttachRS != null) AttachRS = null;
                if (roFeature != null) roFeature = null;
                cid = 0;
                fno = 0;
                fid = 0;
            }
           

            return returnVal;

        }

        internal static void gLoadCompanyPlRs()
        {
            string tmpQry = "select vl_key, vl_value from VL_ATTACH_COMPANY where trc_code is not null";

            try
            {
                gCompanyPlRs = ExecuteQuery(gDataCont, tmpQry);
            }
            catch(Exception ex)
            {
                MessageBox.Show(gApp.ApplicationWindow, "gLoadCompanyPlRs: Error loading Attaching Company picklist. \n" + ex.Message, 
                               "Import Attachments - Error",  MessageBoxButtons.OK, MessageBoxIcon.Error);

                gCcHelper.Complete();
            }
        }

        internal static string gGetAttachingCompany(string p_CompName)
        {
            string tmpRetStr = string.Empty;
            try
            {

                gCompanyPlRs.Filter = "VL_VALUE = '" + p_CompName + "'";

                if(gCompanyPlRs.RecordCount == 1)
                {
                    tmpRetStr = gCompanyPlRs.Fields["VL_KEY"].Value.ToString();
                }
                else
                {
                    tmpRetStr = "";
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(gApp.ApplicationWindow, "gLoadCompanyPlRs: Error loading Attaching Company picklist. \n" + ex.Message,
                               "Import Attachments - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return tmpRetStr;
        }
    }
}
