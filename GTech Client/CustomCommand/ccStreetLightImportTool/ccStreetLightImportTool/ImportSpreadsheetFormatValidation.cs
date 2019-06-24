// ===================================================
//  Copyright 2017 Intergraph Corp.
//  File Name: ImportSpreadsheetFormatValidation.cs
//
//  Description: Validate the correct format of WorkSheet before any record processing is done.
//  Remarks: 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  03/04/2018          Prathyusha                  Created 
// ==========================================================

using System;
using ADODB;
using System.Collections.Generic;
using System.Linq; 
using Intergraph.GTechnology.API;
using System.Windows.Forms;
using System.Data;

namespace GTechnology.Oncor.CustomAPI
{
    class ImportSpreadsheetFormatValidation
    {
        #region Variables

        private IGTDataContext m_oGTDataContext;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_dataContext">The current G/Technology application object.</param>
        public ImportSpreadsheetFormatValidation(IGTDataContext p_dataContext)
        {
            this.m_oGTDataContext = p_dataContext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Method to validate the correct format of WorkSheet before any record processing is done.
        /// </summary>
        /// <param name="p_xlWorksheet">First sheet in the Excel workbook</param>
        /// <returns></returns>
        public bool ValidateSpreadsheetFormat(DataTable p_dataTable)
        {
            bool formatValidate = false;
            List<string> workSheetColumnNames = new List<string>();
            List<string> requiredColumnNames= new List<string> { "TRANSACTION TYPE",
                "ESI LOCATION", "STREETLIGHT ID", "GPS X", "GPS Y", "LAMP TYPE", "WATTAGE",
                "LUMINAIRE STYLE", "ONCOR STRUCTURE", "ONCOR STRUCTURE ID", "LOCATION DESCRIPTION",
                "TRANSACTION STATUS", "TRANSACTION COMMENT", "TRANSACTION DATE" };
            try
            {
                foreach (DataColumn column in p_dataTable.Columns)
                {
                    workSheetColumnNames.Add(column.ColumnName);
                }
                if (CheckRequiredColumns(requiredColumnNames, workSheetColumnNames) && CheckDupilateColumns(workSheetColumnNames) && CheckAdditionalUniqueColumn(workSheetColumnNames.Where(item => !requiredColumnNames.Contains(item)).ToList()))
                {
                    formatValidate=true;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                workSheetColumnNames = null;
                requiredColumnNames = null;
            }
            return formatValidate;
         }

        /// <summary>
        /// Method to ensure whether WorkSheet Dupilate all required column “headers”.
        /// </summary>
        /// <param name="p_workSheetColumnNames">List of worksheet column headers </param>
        /// <returns></returns>
        private bool CheckDupilateColumns(List<string> p_workSheetColumnNames)
        {
            string errrorMessage = null;
            bool checkDupColmns = true;
            try
            {
                if (p_workSheetColumnNames.GroupBy(i=>i).Any(c => c.Count() > 1))
                {
                    errrorMessage = "WorkSheet contains duplicate column names.";
                    DisplayErrorMessage(errrorMessage);
                    checkDupColmns = false;
                }
            }
            catch
            {
                checkDupColmns = false;
                throw;
            }
            return checkDupColmns;
        }

        /// <summary>
        /// Method to ensure whether WorkSheet contains all required column “headers”.
        /// </summary>
        /// <param name="p_requiredColumnNames">List of all required column headers</param>
        /// <param name="p_workSheetColumnNames">List of worksheet column headers </param>
        /// <returns></returns>
        private bool CheckRequiredColumns(List<string> p_requiredColumnNames, List<string> p_workSheetColumnNames)
        {
            string missingColumnsList = null;
            string errrorMessage = null;
            bool checkReqColmns = true;
            try
            {
                foreach (string columnName in p_requiredColumnNames)
                {
                    if (!p_workSheetColumnNames.Any(item => item.ToUpper() == columnName.ToUpper()))
                    {
                        missingColumnsList = missingColumnsList == null ? columnName : missingColumnsList + Environment.NewLine + columnName;
                    }
                }
                if (!String.IsNullOrEmpty(missingColumnsList))
                {
                    errrorMessage = "Spreadsheet does not contain the following required columns." + Environment.NewLine + missingColumnsList;
                    DisplayErrorMessage(errrorMessage);
                    checkReqColmns = false;
                }
            }
            catch
            {
                checkReqColmns = false;
                throw;
            }
            return checkReqColmns;
        }

        /// <summary>
        /// Method to ensure whether WorkSheet contains additional column name matches to a column name for any Street Light attribute and is duplicate with other components.
        /// </summary>
        /// <param name="p_workSheetColumnNames">List of worksheet column headers </param>
        /// <returns></returns>
        private bool CheckAdditionalUniqueColumn(List<string> p_workSheetColumnNames)
        {
            Recordset oRS = null;
            List<string> alreadyExistsList = new List<string>();
            bool alreadyExists = false;
            string errrorMessage = null;
            bool checkUniqColmn = true;
            try
            {
                DataAccess dataAccess = new DataAccess(m_oGTDataContext);
                oRS = dataAccess.GetMetadataRecordset("G3E_ATTRIBUTEINFO_OPTABLE");
                oRS.Filter = "g3e_cno = " + 1 + " or g3e_cno = " + 2 + " or g3e_cno = " + 11 + " or g3e_cno = " + 21 + " or g3e_cno = " + 5601;
                if (oRS != null)
                {
                    oRS.MoveFirst();
                    while (!oRS.EOF)
                    {
                        if(p_workSheetColumnNames.Any(i=>i.ToUpper()==oRS.Fields["G3E_USERNAME"].Value.ToString().ToUpper()))
                        {
                            if (alreadyExistsList.Contains(oRS.Fields["G3E_USERNAME"].Value.ToString()))
                            {
                                alreadyExists = true;
                                break;
                            }
                            else
                            {
                                alreadyExistsList.Add(oRS.Fields["G3E_USERNAME"].Value.ToString());
                            }
                        }
                        oRS.MoveNext();
                    }
                    if(alreadyExists)
                    {
                        errrorMessage = "Spreadsheet cannot contain column name "+ oRS.Fields["G3E_USERNAME"].Value.ToString() + " as it is not unique across all components of the Street Light feature.";
                        DisplayErrorMessage(errrorMessage);
                        checkUniqColmn = false;
                    }
                }
            }
            catch
            {
                checkUniqColmn = false;
                throw;
            }
            finally
            {
                oRS = null;
            }
            return checkUniqColmn;
        }

        /// <summary>
        /// Method to Display the error Message.
        /// </summary>
        /// <param name="p_errorMessage">Error Message</param>
        private void DisplayErrorMessage(string p_errorMessage)
        {
            MessageBox.Show(p_errorMessage, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #endregion
    }
}
