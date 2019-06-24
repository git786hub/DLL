using System;
using System.Windows.Forms;
using Intergraph.GTechnology.API;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using ADODB;

namespace GTechnology.Oncor.CustomAPI
{
    public class ReportDT
    {
        private IGTApplication m_Application = GTClassFactory.Create<IGTApplication>();
        private string m_ReportFile = string.Empty;
        private string m_ReportName = string.Empty;
        private string m_ReportPDF = string.Empty;

        // Full path to the report template
        public string ReportFile
        {
            get
            {
                return m_ReportFile;
            }
            set
            {
                m_ReportFile = value;
            }
        }

        // Name of the worksheet
        public string ReportName
        {
            get
            {
                return m_ReportName;
            }
            set
            {
                m_ReportName = value;
            }
        }

        // Full path to the pdf report
        public string ReportPDF
        {
            get
            {
                return m_ReportPDF;
            }
        }

        public bool CreateReport(string commandName, string wrNumber, int fid, List<KeyValuePair<string, string>> reportValues, DataGridView dgv1, DataGridView dgv2)
        {
            bool bReturnValue = false;            

            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;

            try
            {
                object m_MissingValue = Missing.Value;
                string newFileName = string.Empty;

                // Skip alert if file already exists
                xlApp.DisplayAlerts = false;

                newFileName = Path.GetTempPath() + commandName + " " + wrNumber + "-" + fid;

                xlWorkBook = xlApp.Workbooks.Open(ReportFile, 0, false, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets[ReportName];

                // Populate the values in the spreadsheet matching the parameter name with the named fields in the spreadsheet template
                foreach (KeyValuePair<string, string> kvp in reportValues)
                {
                    xlWorkSheet.Names.Item(kvp.Key).RefersToRange.Value = kvp.Value;
                }

                int rowCount = 0;

                // Populate a row in the spreadsheet for each record in the datagrid
                // The column names in the grid match a named field in the spreadsheet template
                // rowCount is used to advance to the next row in the spreadsheet
                Excel.Range r = xlWorkSheet.Names.Item("Grid1FirstRow").RefersToRange;

                foreach (DataGridViewRow row in dgv1.Rows)
                {
                    if (rowCount > 0)
                    {
                        // Add a row in spreadsheet. Keep the formatting.   
                        r = xlWorkSheet.Range[xlWorkSheet.Names.Item("Grid1FirstRow").RefersToRange.Offset[rowCount, 0], xlWorkSheet.Names.Item("Grid1FirstRow").RefersToRange.Offset[rowCount, 0]];
                        r.Insert(Excel.XlDirection.xlDown, Excel.XlInsertFormatOrigin.xlFormatFromRightOrBelow);
                    }
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        try
                        {
                            xlWorkSheet.Names.Item(cell.OwningColumn.Name).RefersToRange.Offset[rowCount, 0].Value = cell.Value;
                        }
                        catch
                        {
                            // Not all fields in the grid are used in the report. If field does not exist then skip.
                        }
                    }

                    rowCount++;
                }

                if (rowCount > 0)
                {
                    // Delete the unused rows in report grid.
                    // Unused rows are the ones between the current row and the row that contains the named field LastGrid1Row.
                    Excel.Range deleteRange = xlWorkSheet.Range[xlWorkSheet.Names.Item("Grid1FirstRow").RefersToRange.Offset[rowCount, 0], xlWorkSheet.Names.Item("Grid1LastRow").RefersToRange.Offset[-1, 0]];
                    deleteRange.EntireRow.Delete(Excel.XlDirection.xlUp);
                }

                if (dgv2 != null)
                {
                    r = xlWorkSheet.Names.Item("Grid2FirstRow").RefersToRange;
                    rowCount = 0;

                    // Populate a row in the spreadsheet for each record in the datagrid
                    // The column names in the grid match a named field in the spreadsheet template
                    // rowCount is used to advance to the next row in the spreadsheet
                    foreach (DataGridViewRow row in dgv2.Rows)
                    {
                        if (rowCount > 0)
                        {
                            // Add a row in spreadsheet. Keep the formatting.   
                            r = xlWorkSheet.Range[xlWorkSheet.Names.Item("Grid2FirstRow").RefersToRange.Offset[rowCount, 0], xlWorkSheet.Names.Item("Grid2FirstRow").RefersToRange.Offset[rowCount, 0]];
                            r.Insert(Excel.XlDirection.xlDown, Excel.XlInsertFormatOrigin.xlFormatFromRightOrBelow);
                        }

                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            try
                            {
                                xlWorkSheet.Names.Item(cell.OwningColumn.Name).RefersToRange.Offset[rowCount, 0].Value = cell.Value;
                            }
                            catch
                            {
                                // Not all fields in the grid are used in the report. If field does not exist then skip.
                            }
                        }
                        rowCount++;
                    }

                    if (rowCount > 0)
                    {
                        // Delete the unused rows in report grid.
                        // Unused rows are the ones between the current row and the row that contains the named field LastGrid1Row.
                        Excel.Range deleteRange = xlWorkSheet.Range[xlWorkSheet.Names.Item("Grid2FirstRow").RefersToRange.Offset[rowCount, 0], xlWorkSheet.Names.Item("Grid2LastRow").RefersToRange.Offset[-1, 0]];
                        deleteRange.EntireRow.Delete(Excel.XlDirection.xlUp);
                    }
                }

                // Keep only the active worksheet
                foreach (Excel.Worksheet worksheet in xlWorkBook.Worksheets)
                {
                    if (worksheet.Name != ReportName)
                    {
                        worksheet.Delete();
                    }
                }

                // Save the report as an xls file in the user's temp directory
                xlWorkBook.SaveAs(newFileName + ".xls", Excel.XlFileFormat.xlWorkbookNormal, m_MissingValue, m_MissingValue, m_MissingValue, m_MissingValue,
                                  Excel.XlSaveAsAccessMode.xlExclusive, m_MissingValue, m_MissingValue, m_MissingValue, m_MissingValue, m_MissingValue);

                bool deleteError = false;

                // Check if the user has the file open from a previous run of the report.
                if (File.Exists(newFileName + ".pdf"))
                {
                    try
                    {
                        File.Delete(newFileName + ".pdf");
                    }
                    catch (Exception ex)
                    {
                        deleteError = true;
                        MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_REPORT_CREATION + ": " + ex.Message, commandName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }

                if (!deleteError)
                {
                    // Export the xls to a pdf
                    xlWorkBook.ExportAsFixedFormat(Excel.XlFixedFormatType.xlTypePDF, newFileName + ".pdf");
                    m_ReportPDF = newFileName + ".pdf";
                    // Display the pdf to the user
                    System.Diagnostics.Process.Start(newFileName + ".pdf");
                }

                xlWorkBook.Close(true, m_MissingValue, m_MissingValue);
                xlApp.Quit();

                ReleaseObjects(xlWorkSheet);
                ReleaseObjects(xlWorkBook);
                ReleaseObjects(xlApp);
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_REPORT_CREATION + ": " + ex.Message, commandName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return bReturnValue;
        }

        /// <summary>
        /// Uploads the report file to SharePoint
        /// </summary>
        /// <param name="fileName">The absolute path to the file</param>
        /// <param name="fileType">The file type</param>
        /// <param name="url">URL for the document management file</param>
        /// <param name="docMgmtFileName">Filename for the document management file</param>
        /// <returns>Boolean indicating status</returns>
        public bool UploadReport(string fileName, string fileType, ref string url, ref string docMgmtFileName)
        {
            bool bReturnValue = false;

            try
            {
                m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, ConstantsDT.MESSAGE_REPORT_UPLOADING);
                m_Application.BeginWaitCursor();

                string tmpQry = "select param_name, param_value from sys_generalparameter " +
                    "where SUBSYSTEM_NAME = ?";
                Recordset tmpRs = m_Application.DataContext.OpenRecordset(tmpQry, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, "Doc_Management");
                if (!(tmpRs.BOF && tmpRs.EOF))
                {
                    tmpRs.MoveFirst();
                    OncDocManage.OncDocManage rptToSave = new OncDocManage.OncDocManage();
                    
                    while (!tmpRs.EOF)
                    {
                        if (tmpRs.Fields["PARAM_NAME"].Value.ToString() == "JOBWO_REL_PATH")
                            rptToSave.SPRelPath = tmpRs.Fields["PARAM_VALUE"].Value.ToString();
                        if (tmpRs.Fields["PARAM_NAME"].Value.ToString() == "SP_URL")
                            rptToSave.SPSiteURL = tmpRs.Fields["PARAM_VALUE"].Value.ToString();
                        if (tmpRs.Fields["PARAM_NAME"].Value.ToString() == "ROOT_PATH")
                            rptToSave.SPRootPath = tmpRs.Fields["PARAM_VALUE"].Value.ToString();
                        tmpRs.MoveNext();
                    }
                    rptToSave.SrcFilePath = fileName;
                    rptToSave.WrkOrd_Job = m_Application.DataContext.ActiveJob;
                    rptToSave.SPFileName = fileName.Substring(fileName.LastIndexOf("\\") + 1);
                    rptToSave.SPFileType = fileType;
                    bool bSpFileAdded = rptToSave.AddSPFile(true);

                    if (bSpFileAdded == false)
                    {
                        MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_REPORT_SAVING + ": " +
                                    "Error adding " + rptToSave.SPFileName + " to SharePoint.",
                                    ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        bReturnValue = false;
                    }
                    else
                    {
                        bReturnValue = true;
                    }
                    url = rptToSave.RetFileURL;
                    docMgmtFileName = rptToSave.RetFileName;

                    tmpRs = null;
                }
                else
                {
                    MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_REPORT_SAVING + ": " +
                           "Error finding General Parameters JOBWO_REL_PATH or 'SP_URL",
                           ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    bReturnValue = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_REPORT_SAVING + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                bReturnValue = false;
            }

            m_Application.EndWaitCursor();
            m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");

            return bReturnValue;
        }

        /// <summary>
        /// Adds a Hyperlink component to each of the selected fids
        /// </summary>
        /// <param name="fid">The G3E_FID to add a hyperlink component</param>
        /// <param name="fno">The G3E_FNO of the feature to add a hyperlink component</param>
        /// <param name="url">The url to the file</param>
        /// <param name="fileType">The file type</param>
        /// <param name="fileDescription">The file description</param>
        /// <returns>Boolean indicating status</returns>
        public bool AddHyperlinkComponent(int fid, short fno,  string url, string fileType, string fileDescription)
        {
            bool returnValue = false;

            try
            {
                m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, ConstantsDT.MESSAGE_CREATING_HYPERLINK);
                m_Application.BeginWaitCursor();

                IGTKeyObject tmpKeyObj = GTClassFactory.Create<IGTKeyObject>();
                tmpKeyObj = m_Application.DataContext.OpenFeature(fno, fid);
                Recordset tmpHypLnk = null;
                if (fno == ConstantsDT.FNO_DESIGN_AREA)
                {
                    tmpHypLnk = tmpKeyObj.Components["JOB_HYPERLINK_N"].Recordset;
                }
                else
                {
                    tmpHypLnk = tmpKeyObj.Components["HYPERLINK_N"].Recordset;
                }
                
                tmpHypLnk.AddNew(Type.Missing, Type.Missing);
                tmpHypLnk.Fields["G3E_FNO"].Value = fno;
                tmpHypLnk.Fields["G3E_FID"].Value = fid;                    
                tmpHypLnk.Fields["TYPE_C"].Value = fileType;
                tmpHypLnk.Fields["DESCRIPTION_T"].Value = fileDescription;
                tmpHypLnk.Fields["HYPERLINK_T"].Value = url;
                tmpHypLnk.Fields["FILENAME_T"].Value = url.Substring(url.LastIndexOf("/") + 1);
                m_Application.DataContext.UpdateBatch(tmpHypLnk);
                tmpHypLnk.Update();
                tmpKeyObj = null;

                returnValue = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(m_Application.ApplicationWindow, ConstantsDT.ERROR_ADDING_HYPERLINK + ": " + ex.Message, ConstantsDT.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                returnValue = false;
            }

            m_Application.EndWaitCursor();
            m_Application.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "");

            return returnValue;
        }

        private void ReleaseObjects (object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
