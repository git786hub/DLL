// ===================================================
//  Copyright 2017 Intergraph Corp.
//  File Name: StreetLightImportWorkSheet.cs
//
//  Description:    Class to get the data from excel and import data to excel after processing.
//  Remarks: 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  03/04/2018          Prathyusha                  Created 
//  12/04/2018          Sitara                      Modified
// ======================================================
using System.Data;
using System;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    class StreetLightImportWorkSheet
    {
        #region Variables

        private Excel.Worksheet xlWorkSheet = null;
        private Excel.Workbook xlWorkBook = null;
        private Excel.Application xlApp = null;
        private DataTable dtexcel = null;

        #endregion

        #region Properties
        public DataTable ExcelTable
        {
            get
            {
                return dtexcel;
            }
            set
            {
                dtexcel = value;
            }
        }

        #endregion

        #region Methods
        public void InitializeExcel(string p_importStreetLightFile)
        {
            try
            {

                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(p_importStreetLightFile);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.Item[1];

                Excel.Range excelRange = xlWorkSheet.UsedRange; 

                xlWorkSheet = null; 

                //Reading Excel file.               
                object[,] valueArray = (object[,])excelRange.get_Value(Microsoft.Office.Interop.Excel.XlRangeValueDataType.xlRangeValueDefault);

                excelRange = null; 

                ExcelTable = ProcessObjects(valueArray);

                xlApp.DisplayAlerts = false;
                xlWorkBook.Close();
                xlApp.Quit();

                Marshal.ReleaseComObject(xlWorkBook);
                xlWorkBook = null;
                Marshal.ReleaseComObject(xlApp);
                xlApp = null;
                
            }
            catch
            {
                xlApp.DisplayAlerts = false;
                xlWorkBook.Close();
                xlApp.Quit();

                Marshal.ReleaseComObject(xlWorkBook);
                xlWorkBook = null;
                Marshal.ReleaseComObject(xlApp);
                xlApp = null;
                throw;
            }
        }

        private System.Data.DataTable ProcessObjects(object[,] valueArray)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            try
            {
                #region Get the COLUMN names

                for (int k = 1; k <= valueArray.GetLength(1); k++)
                {
                    dt.Columns.Add((string)valueArray[1, k]);
                }
                #endregion

                #region Load Excel SHEET DATA into data table

                object[] singleDValue = new object[valueArray.GetLength(1)];

                for (int i = 2; i <= valueArray.GetLength(0); i++)
                {
                    for (int j = 0; j < valueArray.GetLength(1); j++)
                    {
                        if (valueArray[i, j + 1] != null)
                        {
                            singleDValue[j] = valueArray[i, j + 1].ToString();
                        }
                        else
                        {
                            singleDValue[j] = valueArray[i, j + 1];
                        }
                    }
                    dt.LoadDataRow(singleDValue, System.Data.LoadOption.PreserveChanges);
                }
                #endregion
            }
            catch
            {
                throw;
            }
            return (dt);
        }
    
    public void DataTableTOExcel(string p_importStreetLightFile, DataTable p_dataTable)
        {
            try
            {
                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(p_importStreetLightFile);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.Item[1];
                
                for (int i = 0; i < p_dataTable.Rows.Count; i++)
                {
                    for (int j = 0; j < p_dataTable.Columns.Count; j++)
                    {
                        xlWorkSheet.Cells[i + 2, j + 1] = p_dataTable.Rows[i][j].ToString();
                    }
                }
                
                xlApp.DisplayAlerts = false;
                xlWorkBook.Save();
                xlWorkBook.Close();
                xlApp.Quit();
                
                Marshal.ReleaseComObject(xlWorkBook);
                xlWorkBook = null;
                Marshal.ReleaseComObject(xlApp);
                xlApp = null;
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
