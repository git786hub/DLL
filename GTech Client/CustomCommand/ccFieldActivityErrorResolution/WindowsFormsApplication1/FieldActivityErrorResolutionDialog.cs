using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public partial class FieldActivityErrorResolutionDialog : Form
    {
        bool loadRun = false;
        bool widthChanged = false;
        List<int> changedRows = new List<int>();
        List<string> crews = new List<string>();
        List<string> transactionTypes = new List<string>() { "Overhead", "Underground", "*" };
        List<string> serviceInfoCodes = new List<string>()
                                {
                                    "Structure ID correction",
                                    "New service gang base installation",
                                    "Premise gang base installation",
                                    "New service installation",
                                    "Service removal",
                                    "Service replacement",
                                    "*"
                                };
        StatusFilterDialog StatusDialog = new StatusFilterDialog();
        FromDateDialog FromDateDialog = new FromDateDialog();
        ToDateDialog ToDateDialog = new ToDateDialog();
        EditDateDialog EditDateDialog = new EditDateDialog();
        string valueCheck;
        string oldEditCells;
        DataTable allRecords = null;
        //List<int> localCache = new List<int>();

        public FieldActivityErrorResolutionDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Loads the data from dataprovider into the dgvErrorRecords grid. Also deals with creating the StructID using FLNH_X and FLNH_Y.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FieldActivityErrorResolutionDialog_Load(object sender, EventArgs e)
        {
            try
            {
                //dataProvider.LoadData();
                if (dataProvider.ErrorMessage == null)
                {
                    bindingSource1.DataSource = dataProvider.dtList;
                    //Programatically setting up the constraints on the editable columns, only textboxes are important for this.
                    foreach (DataGridViewColumn column in dgvErrorRecords.Columns)
                    {
                        if (!column.ReadOnly && dataProvider.ColumnRules.ContainsKey(column.DataPropertyName) && column.GetType().ToString().Contains("TextBox"))
                        {
                            if (dataProvider.ColumnRules[column.DataPropertyName].ElementAt((int)dataProvider.MetaValue.DATA_TYPE) == "VARCHAR2")
                            {
                                ((DataGridViewTextBoxColumn)column).MaxInputLength = Convert.ToInt32(dataProvider.ColumnRules[column.DataPropertyName].ElementAt((int)dataProvider.MetaValue.DATA_LEGNTH));
                            }
                            else if (!column.DataPropertyName.Equals("SERVICE_ACTIVITY_ID") && dataProvider.ColumnRules[column.DataPropertyName].ElementAt((int)dataProvider.MetaValue.DATA_TYPE) == "NUMBER")
                            {
                                ((DataGridViewTextBoxColumn)column).MaxInputLength = Convert.ToInt32(dataProvider.ColumnRules[column.DataPropertyName].ElementAt((int)dataProvider.MetaValue.DATA_PRECISION));
                                string numberFormat = "";
                                for (int i = 0; i < Convert.ToInt32(dataProvider.ColumnRules[column.DataPropertyName].ElementAt((int)dataProvider.MetaValue.DATA_PRECISION)); i++)
                                {
                                    numberFormat += "#";
                                }
                                int decimalPlaces = Convert.ToInt32(dataProvider.ColumnRules[column.DataPropertyName].ElementAt((int)dataProvider.MetaValue.DATA_SCALE));
                                if (decimalPlaces > 0)
                                {
                                    numberFormat.Insert(decimalPlaces, ".");
                                }
                                column.DefaultCellStyle.Format = numberFormat;
                            }
                        }
                    }
                    //finally giving our grid some data to display.
                    dgvErrorRecords.DataSource = bindingSource1;
                    dgvErrorRecords.Columns["TransactionDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
                    dataProvider.dtList.AcceptChanges();
                }
                bindingSource2.DataSource = dataProvider.locateFeatures;
                locationNavigator.BindingSource = bindingSource2;
                loadRun = true;
                dgv_Filters.Rows.Add();
                BindingSource tempBS = (BindingSource)dgvErrorRecords.DataSource;
                allRecords = (DataTable)tempBS.DataSource;
                filterStringCreation();
                //load the dropdown menu 
                foreach (DataRow row in dataProvider.dtList.Rows)
                {
                    if (!crews.Contains(row.Field<string>("SERVICE_CENTER_CODE")))
                    {
                        crews.Add(row.Field<string>("SERVICE_CENTER_CODE"));
                        CheckBox crewHQCode = new CheckBox();
                        crewHQCode.Text = row.Field<string>("SERVICE_CENTER_CODE");
                    }
                }

                crews.Insert(0, "No Filter");
                DataGridViewComboBoxColumn crewCbColumn = (dgv_Filters.Columns["fltrCrewHQ"] as DataGridViewComboBoxColumn);
                crewCbColumn.Items.AddRange(crews.ToArray());
                dgv_Filters.Rows[0].Cells["fltrCrewHQ"].Value = "No Filter";


                transactionTypes.Insert(0, "No Filter");
                DataGridViewComboBoxColumn transactionCbColumn = (dgv_Filters.Columns["fltrTransactionType"] as DataGridViewComboBoxColumn);
                transactionCbColumn.Items.AddRange(transactionTypes.ToArray());
                dgv_Filters.Rows[0].Cells["fltrTransactionType"].Value = "No Filter";

                serviceInfoCodes.Insert(0, "No Filter");
                DataGridViewComboBoxColumn serviceInfoCodeCbColumn = (dgv_Filters.Columns["fltrActivityCode"] as DataGridViewComboBoxColumn);
                serviceInfoCodeCbColumn.Items.AddRange(serviceInfoCodes.ToArray());
                dgv_Filters.Rows[0].Cells["fltrActivityCode"].Value = "No Filter";

            }

            catch (Exception error)
            {
                MessageBox.Show("Error in FieldActivityErrorResolutionDialog_Load (" + error.Message + ")", "G/Technology");
            }
        }

        private void dgvErrorRecords_Scroll(object sender, ScrollEventArgs e)
        {
            dgv_Filters.HorizontalScrollingOffset = dgvErrorRecords.HorizontalScrollingOffset;
        }

        /// <summary>
        /// Colors the cells according to the ddd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvErrorRecords_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (dataProvider.requiredColumns.Contains(dgvErrorRecords.Columns[e.ColumnIndex].Name) && (dgvErrorRecords.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null || dgvErrorRecords.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == ""))
                {
                    dgvErrorRecords.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Red;
                }
                else if (dgvErrorRecords.Rows[e.RowIndex].Cells["EditedCells"].Value != null)
                {
                    string[] editedList = dgvErrorRecords.Rows[e.RowIndex].Cells["EditedCells"].Value.ToString().Split(',');
                    if (editedList.Contains(e.ColumnIndex.ToString()))
                    {
                        dgvErrorRecords.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Yellow;
                    }
                }

                if (dgvErrorRecords.Columns[e.ColumnIndex].Name == "TransactionType" && e.RowIndex >= 0)
                {
                    switch ((string)dgvErrorRecords["TransactionType", e.RowIndex].Value)
                    {
                        case "O":
                            e.Value = "Overhead";
                            e.FormattingApplied = true;
                            break;
                        case "U":
                            e.Value = "Underground";
                            e.FormattingApplied = true;
                            break;
                    }
                }
                if (dgvErrorRecords.Columns[e.ColumnIndex].Name == "ActivityCode" && e.RowIndex >= 0)
                {
                    switch ((string)dgvErrorRecords["ActivityCode", e.RowIndex].Value)
                    {
                        case "ES":
                            e.Value = "Structure ID correction";
                            e.FormattingApplied = true;
                            break;
                        case "GB":
                            e.Value = "New service gang base installation";
                            e.FormattingApplied = true;
                            break;
                        case "MO":
                            e.Value = "Premise gang base installation";
                            e.FormattingApplied = true;
                            break;
                        case "NS":
                            e.Value = "New service installation";
                            e.FormattingApplied = true;
                            break;
                        case "RM":
                            e.Value = "Service removal";
                            e.FormattingApplied = true;
                            break;
                        case "RP":
                            e.Value = "Service replacement";
                            e.FormattingApplied = true;
                            break;
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Error in dgvErrorRecords_CellFormatting (" + error.Message + ")", "G/Technology");
            }
        }

        /// <summary>
        /// Captures changes in the records to assist with the submit function. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvErrorRecords_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                int uniqueRowID = Convert.ToInt32(dgvErrorRecords.Rows[e.RowIndex].Cells["ServiceActivityCode"].Value);
                //changedRows.Add(e.RowIndex);
                if (!changedRows.Contains(uniqueRowID))
                    changedRows.Add(uniqueRowID);
            }
            if (e.RowIndex > -1 && e.ColumnIndex > -1 && changedRows.Count > 0 && e.ColumnIndex != dgvErrorRecords.Columns["EditedCells"].Index)
            {
                dgvErrorRecords.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Yellow;
                if (dgvErrorRecords.Rows[e.RowIndex].Cells["EditedCells"].Value == null || dgvErrorRecords.Rows[e.RowIndex].Cells["EditedCells"].Value.ToString() == "")
                {
                    dgvErrorRecords.Rows[e.RowIndex].Cells["EditedCells"].Value += e.ColumnIndex.ToString();
                }
                else
                {
                    if (!dgvErrorRecords.Rows[e.RowIndex].Cells["EditedCells"].Value.ToString().Split(',').Contains(e.ColumnIndex.ToString()))
                    {
                        dgvErrorRecords.Rows[e.RowIndex].Cells["EditedCells"].Value += "," + e.ColumnIndex.ToString();
                    }
                }
            }
            else if (e.RowIndex > -1)
            {
                dgvErrorRecords.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
            }

            if (e.RowIndex > -1 && !dgvErrorRecords.Rows[e.RowIndex].Cells["EditedCells"].Value.ToString().Split(',').Contains(dgvErrorRecords.CurrentCell.ColumnIndex.ToString()))
            {
                dgvErrorRecords.CurrentCell.Style.BackColor = Color.White;
            }
        }

        //this is the function that will do our map update, will be more complex than our previous one. 
        /// <summary>
        /// This is the function to help with the locate functionality.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bindingNavigator2_MouseClickTHING(object sender, MouseEventArgs e)
        {
            if (UpdateMap.Checked)
            {
                //switch station for radio button group.
                dataProvider.locate(1, 1);
            }
        }

        /// <summary>
        /// Gets the data changes from the table and sends them to the dataProvider.v
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitCmd_Click(object sender, EventArgs e)
        {
            List<int> conflictedRecords = new List<int>();
            try
            {
                bool dateMatch = true;
                //foreach (int rowIndex in changedRows)
                foreach (int uniqueRowId in changedRows)
                {
                    DataRow row = allRecords.Rows.Find(uniqueRowId);

                    if (row != null && row.ItemArray.Count() > 0)
                    {
                        //string structureId = Convert.ToString(row.ItemArray[16]);
                        string structureId = Convert.ToString(row.Field<string>("STRUCT_ID"));

                        if (!string.IsNullOrEmpty(structureId) && !structureId.Equals("-"))
                        {
                          dataProvider.dtList.Rows.Find(uniqueRowId).SetField<string>("SVC_STRUCTURE_ID", structureId);
                        }

                        dataProvider.dtList.Rows.Find(uniqueRowId).SetField<string>("USER_ID", Environment.UserName);
                        dateMatch = dataProvider.dateChecker(uniqueRowId);
                        if (!dateMatch)
                        {
                            conflictedRecords.Add(uniqueRowId);
                        }
                    }
                }

                if (conflictedRecords.Count > 0)
                {
                    dataProvider.refresh(conflictedRecords);
                    MessageBox.Show("Field Activity Error Resolution command - The highlighted record(s) have been modified by another user. The submitted changes for the highlighted record(s) have been discarded.", "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                DataTable changeTable = dataProvider.dtList.GetChanges();
                if (dateMatch && changeTable != null)
                {
                    //var a = changeTable.Columns[0]; 
                    dataProvider.submit(changeTable);
                    changedRows.Clear();
                    dgvErrorRecords.DefaultCellStyle.BackColor = Color.White;
                    foreach (DataGridViewRow row in dgvErrorRecords.Rows)
                    {
                        if (row.Index != -1)
                        {
                            //   row.Cells["StructureID"].Value = dataProvider.dtList.Rows[row.Index].Field<string>("FLNX_H") + "-" + dataProvider.dtList.Rows[row.Index].Field<string>("FLNY_H");
                            //row.Cells["StructureID"].Value = dataProvider.dtList.Rows.Find(row.Cells["ServiceActivityCode"].Value).Field<string>("FLNX_H") + "-" + dataProvider.dtList.Rows.Find(row.Cells["ServiceActivityCode"].Value).Field<string>("FLNY_H");
                            row.Cells["StructureID"].Value = dataProvider.dtList.Rows.Find(row.Cells["ServiceActivityCode"].Value).Field<string>("SVC_STRUCTURE_ID");
                        }
                        row.Cells["StructureID"].Style.BackColor = Color.White;
                        row.Cells["EditedCells"].Value = "";
                        changedRows.Clear();
                    }

                }
                else
                {
                    changedRows.Clear();
                    dataProvider.refresh(conflictedRecords);
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show("Field Activity Error Resolution command - Could not save the edits. " + ex.Message, "G/Technology", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Filter functionality for the top table. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_Filters_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (loadRun)
            {
                filterStringCreation();
            }
        }

        private void RefreshCmd_Click(object sender, EventArgs e)
        {
            //re create struct id's after refresh. check for changes.
            if (changedRows.Count > 0)
            {
                bool cellsChanged = false;
                foreach (DataGridViewRow row in dgvErrorRecords.Rows)
                {
                    //if (row.Cells["EditedCells"].Value != "" && row.Cells["EditedCells"].Value.ToString() != "")
                    if (row.Cells["EditedCells"].Value != null && !string.IsNullOrEmpty(row.Cells["EditedCells"].Value.ToString()))
                    {
                        cellsChanged = true;
                    }
                }
                if (cellsChanged)
                {
                    string editsExist = "There are pending changes. Do you want to apply the changes?";
                    string editCaption = "G/Technology";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show(editsExist, editCaption, buttons);
                    if (result == DialogResult.Yes)
                    {
                        SubmitCmd_Click(sender, e);
                        dataProvider.LoadData();
                        return;
                    }

                }


            }

            dataProvider.LoadData();
            FieldActivityErrorResolutionDialog_Load(null, EventArgs.Empty);
            return;
        }

        /// <summary>
        /// Capture method for the filter datagrid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_Filters_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                StatusDialog.ShowDialog();
                if (StatusDialog.selectedFilters.Count == 1)
                {
                    (dgv_Filters.Columns[e.ColumnIndex] as DataGridViewButtonColumn).Text = StatusDialog.selectedFilters[0];
                }
                else if (StatusDialog.selectedFilters.Count == 0)
                {
                    (dgv_Filters.Columns[e.ColumnIndex] as DataGridViewButtonColumn).Text = "No Filter";
                }
                else
                {
                    (dgv_Filters.Columns[e.ColumnIndex] as DataGridViewButtonColumn).Text = "Multiple";
                }
            }
            if (e.ColumnIndex == dgv_Filters.Columns["ToTransactionDate"].Index)
            {
                ToDateDialog.ShowDialog();
                if (ToDateDialog.toDate != null && ToDateDialog.toDate != DateTime.MinValue)
                {
                    (dgv_Filters.Columns[e.ColumnIndex] as DataGridViewButtonColumn).Text = ToDateDialog.toDate.ToShortDateString();
                }
            }
            if (e.ColumnIndex == dgv_Filters.Columns["FromTransactionDate"].Index)
            {
                FromDateDialog.ShowDialog();
                if (FromDateDialog.fromDate != null && FromDateDialog.fromDate != DateTime.MinValue)
                {
                    (dgv_Filters.Columns[e.ColumnIndex] as DataGridViewButtonColumn).Text = FromDateDialog.fromDate.ToShortDateString();
                }

            }

            if (e.ColumnIndex == dgv_Filters.Columns["fltrEditDate"].Index)
            {
                EditDateDialog.ShowDialog();
                if (EditDateDialog.fltrEditDate != null && EditDateDialog.fltrEditDate != DateTime.MinValue)
                {
                    (dgv_Filters.Columns[e.ColumnIndex] as DataGridViewButtonColumn).Text = EditDateDialog.fltrEditDate.ToShortDateString();
                }
            }
            filterStringCreation();
        }

        /// <summary>
        /// Method to create the filter string. Takes all the inputs from the filter grid. 
        /// </summary>
        private void filterStringCreation()
        {
            bindingSource1.Filter = "";
            foreach (DataGridViewCell cell in dgv_Filters.Rows[0].Cells)
            {
                if (cell.Value != null && cell.Value.ToString() != "" && dgv_Filters.Columns[cell.ColumnIndex].Name != "fltrStatus"
                    && dgv_Filters.Columns[cell.ColumnIndex].Name != "ToTransactionDate" && dgv_Filters.Columns[cell.ColumnIndex].Name != "FromTransactionDate" && dgv_Filters.Columns[cell.ColumnIndex].Name != "fltrEditDate")
                {
                    string filterInput = string.Empty;
                    int columnIndex = cell.ColumnIndex < 12 ? cell.ColumnIndex : cell.ColumnIndex - 1;
                    string columnName = dgvErrorRecords.Columns[columnIndex].DataPropertyName;
                    if (!cell.Value.ToString().Equals("No Filter"))
                    {
                        filterInput = cell.Value.ToString();

                        switch (filterInput)
                        {
                            case "Overhead":
                                filterInput = "O";
                                break;
                            case "Underground":
                                filterInput = "U";
                                break;
                            case "Structure ID correction":
                                filterInput = "ES";
                                break;
                            case "New service gang base installation":
                                filterInput = "GB";
                                break;
                            case "Premise gang base installation":
                                filterInput = "MO";
                                break;
                            case "New service installation":
                                filterInput = "NS";
                                break;
                            case "Service removal":
                                filterInput = "RM";
                                break;
                            case "Service replacement":
                                filterInput = "RP";
                                break;
                        }
                    }
                    else
                    {
                        filterInput = cell.Value.ToString();
                    }

                    if (!string.IsNullOrEmpty(filterInput) && filterInput.Equals("No Filter"))
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(filterInput) && (bindingSource1.Filter == null || bindingSource1.Filter == "") &&
                        dgvErrorRecords.Columns[cell.ColumnIndex].Name != "StructureID" && dgvErrorRecords.Columns[cell.ColumnIndex].Name != "OverrideTolerance")
                    {
                        #region
                        //if (!cell.Value.ToString().Equals("No Filter"))
                        //{
                        //    string filterInput = cell.Value.ToString();

                        //    switch (filterInput)
                        //    {
                        //        case "Overhead":
                        //            filterInput = "O";
                        //            break;
                        //        case "Underground":
                        //            filterInput = "U";
                        //            break;
                        //        case "Structure ID correction":
                        //            filterInput = "ES";
                        //            break;
                        //        case "New service gang base installation":
                        //            filterInput = "GB";
                        //            break;
                        //        case "Premise gang base installation":
                        //            filterInput = "MO";
                        //            break;
                        //        case "New service installation":
                        //            filterInput = "NS";
                        //            break;
                        //        case "Service removal":
                        //            filterInput = "RM";
                        //            break;
                        //        case "Service replacement":
                        //            filterInput = "RP";
                        //            break;
                        //    }
                        #endregion
                        if (filterInput.Equals("*"))
                            bindingSource1.Filter = "Convert(" + columnName + ", 'System.String') = '" + filterInput + "'";
                        else
                            bindingSource1.Filter = "Convert(" + columnName + ", 'System.String') Like '%" + filterInput + "%'";
                    }
                    else if (dgvErrorRecords.Columns[columnIndex].Name != "StructureID" && dgvErrorRecords.Columns[columnIndex].Name != "OverrideTolerance")
                    {
                        if (filterInput.Equals("*"))
                            bindingSource1.Filter = "Convert(" + columnName + ", 'System.String') = '" + filterInput + "'";
                        else
                            bindingSource1.Filter += " AND " + "Convert(" + columnName + ", 'System.String') Like '%" + filterInput + "%'";
                    }
                    else if (dgvErrorRecords.Columns[columnIndex].Name == "OverrideTolerance")
                    {
                        if (cell.Value.ToString() == "True")
                        {
                            if (bindingSource1.Filter == null || bindingSource1.Filter == "")
                            {
                                bindingSource1.Filter = "OVERRIDE_TOLERANCE = 1";
                            }
                            else
                            {
                                bindingSource1.Filter += " AND OVERRIDE_TOLERANCE = 1";
                            }
                        }
                    }
                    else if ((bindingSource1.Filter == null || bindingSource1.Filter == "") && !cell.Value.ToString().Contains("-"))
                    {
                        //bindingSource1.Filter = "(FLNX_H LIKE '*" + cell.Value.ToString() + "*' OR FLNY_H LIKE '*" + cell.Value.ToString() + "*')";
                        bindingSource1.Filter = "SVC_STRUCTURE_ID LIKE '*" + filterInput + "*'";
                    }
                    else if ((bindingSource1.Filter == null || bindingSource1.Filter == "") && filterInput.Contains("-") && filterInput.Length != 1)
                    {
                        bindingSource1.Filter = "SVC_STRUCTURE_ID LIKE '*" + filterInput + "*'";
                    }
                    else if (filterInput.Contains("-") && filterInput.Length != 1)
                    {
                        bindingSource1.Filter = " AND SVC_STRUCTURE_ID LIKE '*" + filterInput + "*'";
                    }
                    else if (!(filterInput.Contains("-") && filterInput.Length == 1) && !filterInput.Equals("No Filter"))
                    {
                        //bindingSource1.Filter += " AND (FLNX_H LIKE '*" + cell.Value.ToString() + "*' OR FLNY_H LIKE '*" + cell.Value.ToString() + "*')";
                        bindingSource1.Filter += "AND SVC_STRUCTURE_ID LIKE '*" + filterInput + "*')";
                    }
                }
            }
            //adding the special status filtes.
            if (StatusDialog.selectedFilters.Count != 0)
            {
                if (bindingSource1.Filter == null || bindingSource1.Filter == "")
                {
                    string FilterCreation = "";
                    foreach (string filter in StatusDialog.selectedFilters)
                    {
                        if ((bindingSource1.Filter == null || bindingSource1.Filter == "") && (FilterCreation == null || FilterCreation == ""))
                        {
                            FilterCreation = "(STATUS_C LIKE '*" + filter + "*'";
                        }
                        else
                        {
                            FilterCreation += " OR STATUS_C LIKE '*" + filter + "*'";
                        }
                    }
                    FilterCreation += ")";
                    bindingSource1.Filter += FilterCreation;
                }
                else
                {
                    string FilterCreation = "";
                    foreach (string filter in StatusDialog.selectedFilters)
                    {//make the string THEN add it to the filters

                        if (!FilterCreation.Contains("STATUS_C"))
                        {
                            FilterCreation += " AND (STATUS_C LIKE '*" + filter + "*'";
                        }
                        else
                        {
                            FilterCreation += "OR STATUS_C LIKE '*" + filter + "*'";
                        }
                    }
                    FilterCreation += ")";
                    bindingSource1.Filter += FilterCreation;
                }
            }
            string dateRangeFilter = "";
            if (ToDateDialog.toDate != DateTime.MinValue && FromDateDialog.fromDate != DateTime.MinValue)
            {
                dateRangeFilter = "(TRANS_DATE >= #" + FromDateDialog.fromDate.ToShortDateString() + "# and TRANS_DATE <= #" + ToDateDialog.toDate.ToShortDateString() + "#)";
                if (bindingSource1.Filter != "")
                {
                    dateRangeFilter = dateRangeFilter.Insert(0, " AND ");
                }
                bindingSource1.Filter += dateRangeFilter;
            }
            else if (ToDateDialog.toDate != DateTime.MinValue)
            {
                //dateRangeFilter = "TRANS_DATE <= #" + ToDateDialog.toDate.ToShortDateString() + "#";
                dateRangeFilter = "TRANS_DATE < #" + ToDateDialog.toDate.ToShortDateString() + "#";
                if (bindingSource1.Filter != "")
                {
                    dateRangeFilter = dateRangeFilter.Insert(0, " AND ");
                }
                bindingSource1.Filter += dateRangeFilter;
            }
            else if (FromDateDialog.fromDate != DateTime.MinValue)
            {
                //dateRangeFilter = "TRANS_DATE >= #" + FromDateDialog.fromDate.ToShortDateString() + "#";
                dateRangeFilter = "TRANS_DATE > #" + FromDateDialog.fromDate.ToShortDateString() + "#";
                if (bindingSource1.Filter != "")
                {
                    dateRangeFilter = dateRangeFilter.Insert(0, " AND ");
                }
                bindingSource1.Filter += dateRangeFilter;
            }

            dateRangeFilter = "";
            if (EditDateDialog.fltrEditDate != DateTime.MinValue)
            {
                //dateRangeFilter = " EDIT_DATE >= #" + EditDateDialog.fltrEditDate.ToShortDateString() + "#";
                dateRangeFilter = " EDIT_DATE > #" + EditDateDialog.fltrEditDate.ToShortDateString() + "#";
                if (bindingSource1.Filter != "")
                {
                    dateRangeFilter = dateRangeFilter.Insert(0, " AND ");
                }
                bindingSource1.Filter += dateRangeFilter;
            }
            dgvErrorRecords.DataSource = null;
            dgvErrorRecords.DataSource = bindingSource1;
            var tempRows = changedRows;

            foreach (DataGridViewRow row in dgvErrorRecords.Rows)
            {
                string temp = row.Cells["EditedCells"].Value as string;
                //if (row.Index != -1)
                //{
                //    row.Cells["StructureID"].Value = row.Cells["FLNX_H"].Value.ToString() + "-" + row.Cells["FLNY_H"].Value.ToString();
                //}
                row.Cells["StructureID"].Style.BackColor = Color.White;
                row.Cells["EditedCells"].Value = temp;
            }
            changedRows = tempRows;
        }


        private void ExitCmd_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception error)
            {
                MessageBox.Show("Error in ExitCmd_Click (" + error.Message + ")", "G/Technology");
            }
        }

        /// <summary>
        /// Currently this allows the dialog to close and calls the methods to clear out all the remaining data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// TODO 
        ///   - add saving functionality
        ///   - thats pretty much it
        private void FieldActivityErrorResolutionDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool cellsChanged = false;
            foreach (DataGridViewRow row in dgvErrorRecords.Rows)
            {
                if (row.Cells["EditedCells"].Value != "" && row.Cells["EditedCells"].Value.ToString() != "")
                {
                    cellsChanged = true;
                }
            }
            if (cellsChanged)
            {
                string editsExist = "There are unsaved changes to the Error Records, would you like to save them now?";
                string editCaption = "G/Technology";
                MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
                DialogResult result = MessageBox.Show(editsExist, editCaption, buttons);
                if (result == DialogResult.Yes)
                {
                    SubmitCmd_Click(sender, e);
                    changedRows.Clear();
                    dataProvider.dispose();
                }
                else if (result == DialogResult.No)
                {
                    changedRows.Clear();
                    dataProvider.dispose();
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            }
            else
            {
                changedRows.Clear();
                dataProvider.dispose();
            }
            dataProvider.ccHelper.Complete();
        }

        /// <summary>
        /// Causes the changed method to fire earlier in the process. Prevents changedvalue from getting skipped when sorting.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvErrorRecords_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvErrorRecords.IsCurrentCellDirty)
            {
                dgvErrorRecords.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        /// <summary>
        /// Manages column widths between the two tables.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvErrorRecords_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (loadRun && !widthChanged)
            {
                if (dgvErrorRecords.Columns["TransactionDate"].Index == e.Column.Index)
                {
                    int oldWidth = dgv_Filters.Columns["ToTransactionDate"].Width + dgv_Filters.Columns["FromTransactionDate"].Width;
                    int deltaWidth = (e.Column.Width - oldWidth) / 2;
                    widthChanged = true;
                    dgv_Filters.Columns["ToTransactionDate"].Width += deltaWidth;
                    widthChanged = true;
                    dgv_Filters.Columns["FromTransactionDate"].Width += deltaWidth;
                    dgv_Filters.HorizontalScrollingOffset = dgvErrorRecords.HorizontalScrollingOffset;
                }
                else if (e.Column.Name == "DwellingType")
                {
                    widthChanged = true;
                    dgv_Filters.Columns[e.Column.Index + 1].Width = e.Column.Width + 17;
                    dgv_Filters.HorizontalScrollingOffset = dgvErrorRecords.HorizontalScrollingOffset;
                }
                else
                {
                    if (e.Column.Index < dgvErrorRecords.Columns["TransactionDate"].Index)
                    {
                        widthChanged = true;
                        dgv_Filters.Columns[e.Column.Index].Width = e.Column.Width;
                        dgv_Filters.HorizontalScrollingOffset = dgvErrorRecords.HorizontalScrollingOffset;
                    }
                    else
                    {
                        widthChanged = true;
                        dgv_Filters.Columns[e.Column.Index + 1].Width = e.Column.Width;
                        dgv_Filters.HorizontalScrollingOffset = dgvErrorRecords.HorizontalScrollingOffset;
                    }
                }
            }
            else
            {
                widthChanged = false;
            }
        }

        private void dgvErrorRecords_Paint(object sender, PaintEventArgs e)
        {
        }

        /// <summary>
        /// Colors the read-only cells.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvErrorRecords_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if ((e.RowIndex > -1 && e.ColumnIndex > -1) && dgvErrorRecords.Columns[e.ColumnIndex].ReadOnly)
            {
                dgvErrorRecords.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Gainsboro;
            }
        }

        /// <summary>
        /// Manages column widths between the two tables.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_Filters_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (loadRun && !widthChanged)
            {
                if (e.Column.Index == dgv_Filters.Columns["FromTransactionDate"].Index)
                {
                    int oldWidth = dgvErrorRecords.Columns["TransactionDate"].Width;
                    int deltaWidth = ((dgv_Filters.Columns["ToTransactionDate"].Width + dgv_Filters.Columns["FromTransactionDate"].Width) - oldWidth) / 2;
                    widthChanged = true;
                    dgvErrorRecords.Columns["TransactionDate"].Width += deltaWidth;
                    dgv_Filters.HorizontalScrollingOffset = dgvErrorRecords.HorizontalScrollingOffset;
                }
                else if (e.Column.Index == dgv_Filters.Columns["ToTransactionDate"].Index)
                {
                    int oldWidth = dgvErrorRecords.Columns["TransactionDate"].Width;
                    int deltaWidth = oldWidth - e.Column.Width;
                    widthChanged = true;
                    dgv_Filters.Columns["FromTransactionDate"].Width = deltaWidth;
                    dgv_Filters.HorizontalScrollingOffset = dgvErrorRecords.HorizontalScrollingOffset;
                }
                else if (e.Column.Name == "fltrDwellingType")
                {
                    widthChanged = true;
                    dgvErrorRecords.Columns[e.Column.Index - 1].Width = e.Column.Width - 17;
                    dgv_Filters.HorizontalScrollingOffset = dgvErrorRecords.HorizontalScrollingOffset;
                }
                else if (e.Column.Index != dgv_Filters.Columns["ToTransactionDate"].Index && e.Column.Index != dgv_Filters.Columns["FromTransactionDate"].Index)
                {
                    if (e.Column.Index < dgvErrorRecords.Columns["TransactionDate"].Index)
                    {
                        widthChanged = true;
                        dgvErrorRecords.Columns[e.Column.Index].Width = e.Column.Width;
                        dgv_Filters.HorizontalScrollingOffset = dgvErrorRecords.HorizontalScrollingOffset;
                    }
                    else
                    {
                        widthChanged = true;
                        dgvErrorRecords.Columns[e.Column.Index - 1].Width = e.Column.Width;
                        dgv_Filters.HorizontalScrollingOffset = dgvErrorRecords.HorizontalScrollingOffset;
                    }
                }
            }
            else
            {
                widthChanged = false;
            }
        }

        /// <summary>
        /// Causes filtering to work live.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_Filters_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgv_Filters.IsCurrentCellDirty)
            {
                dgv_Filters.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        /// <summary>
        /// Runs the locate process for the selected record.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvErrorRecords_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (UpdateMap.Checked)
            {
                //switch station for radio button group.
                string locateMethod = "";
                int index = e.RowIndex;
                foreach (var control in locateType.Controls)
                {
                    RadioButton radioButton = control as RadioButton;

                    if (radioButton != null && radioButton.Checked)
                    {
                        locateMethod = radioButton.Name;
                    }
                }
                if (locateMethod != "")
                {
                    switch (locateMethod)
                    {
                        case "optPremiseNumber":
                            if (dgvErrorRecords.Rows[index].Cells["PremiseNumber"].Value != null)
                            {
                                dataProvider.LocateByPremiseNumber(dgvErrorRecords.Rows[index].Cells["PremiseNumber"].Value.ToString());
                                bindingSource2.DataSource = dataProvider.locateFeatures;
                                locationNavigator.Refresh();
                            }
                            else
                            {
                                MessageBox.Show("No Premise number for location", "G/Technology");
                            }
                            break;
                        case "optAddress":
                            //call locate method from dataProvider
                            DataGridViewCellCollection cells = dgvErrorRecords.Rows[index].Cells;
                            if (cells["HouseNumber"].Value != null || cells["StreetName"].Value != null || cells["StreetType"].Value != null || cells["HouseNumberFraction"].Value != null ||
                                cells["Direction"].Value != null || cells["DirectionTrailing"].Value != null || cells["Unit"].Value != null)
                            {
                                dataProvider.LocateByAddress(Int32.Parse(cells["HouseNumber"].Value.ToString()), cells["Streetname"].Value.ToString(), cells["StreetType"].Value.ToString(),
                                    cells["HouseFractionNumber"].Value.ToString(), cells["Direction"].Value.ToString(), cells["DirectionTrailing"].Value.ToString(),
                                    cells["Unit"].Value.ToString());
                                bindingSource2.DataSource = dataProvider.locateFeatures;
                                locationNavigator.Refresh();
                            }
                            else
                            {
                                MessageBox.Show("No address information found for selected record.", "G/Technology");
                            }
                            break;
                        case "optMeterGeocode":
                            //call locate method from dataProvider
                            DataGridViewCellCollection geoCells = dgvErrorRecords.Rows[index].Cells;
                            if (geoCells["MeterLongitude"].Value != null && geoCells["MeterLatitude"].Value != null)
                            {
                                dataProvider.geoLocate(Convert.ToDouble(geoCells["MeterLongitude"].Value), Convert.ToDouble(geoCells["MeterLatitude"].Value));
                            }
                            else
                            {
                                MessageBox.Show("No Meter Geocode information found for selected record.", "G/Technology");
                            }
                            break;
                        case "optStructureID":
                            //call locate method from dataProvider
                            if (dgvErrorRecords.Rows[index].Cells["StructureID"].Value != null)
                            {
                                dataProvider.LocateByStructID(dgvErrorRecords.Rows[index].Cells["StructureID"].Value.ToString());
                                bindingSource2.DataSource = dataProvider.locateFeatures;
                                locationNavigator.Refresh();
                            }
                            else
                            {
                                MessageBox.Show("No Structure ID found for selected record.", "G/Technology");
                            }
                            break;
                        case "optTransformer":
                            //call locate method from dataProvider
                            if (dgvErrorRecords.Rows[index].Cells["TransCompanyNum"].Value != null)
                            {
                                dataProvider.LocateByTransformerID(dgvErrorRecords.Rows[index].Cells["TransCompanyNum"].Value.ToString());
                                bindingSource2.DataSource = dataProvider.locateFeatures;
                                locationNavigator.Refresh();
                            }
                            else
                            {
                                MessageBox.Show("No Transformer Company ID found for selected record.", "G/Technology");
                            }
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Changes which feature is selected from the features found by the locate.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bindingSource2_CurrentChanged(object sender, EventArgs e)
        {
          if(bindingSource2.Count > 0)
          {
            int FID = (int)(bindingSource2.Current as DataRowView).Row.Field<decimal>(0);
            short FNO = (short)(bindingSource2.Current as DataRowView).Row.Field<decimal>(1);
            dataProvider.locate(FID, FNO);
          }            
        }

        /// <summary>
        /// Support for the Transformer Company Number and Structure ID checking.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvErrorRecords_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvErrorRecords.CurrentCell.ColumnIndex == dgvErrorRecords.Columns["StructureID"].Index || dgvErrorRecords.CurrentCell.ColumnIndex == dgvErrorRecords.Columns["TransCompanyNum"].Index)
            {
                valueCheck = dgvErrorRecords.CurrentCell.Value as string;
                oldEditCells = dgvErrorRecords.CurrentRow.Cells["EditedCells"].Value as string;
            }
        }

        /// <summary>
        ///  Support for the Transformer Company Number and Structure ID checking.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvErrorRecords_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvErrorRecords.Columns["StructureID"].Index)
            {
                if (!dataProvider.checkStructID(dgvErrorRecords.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) && (dgvErrorRecords.CurrentRow.Cells["EditedCells"].Value.ToString().Contains(" " + dgvErrorRecords.Columns["StructureID"].Index + ",") || dgvErrorRecords.CurrentRow.Cells["EditedCells"].Value.ToString().Contains(dgvErrorRecords.Columns["StructureID"].Index.ToString())))
                {
                    MessageBox.Show("Structure ID not found", "G/Technology");
                    dgvErrorRecords.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = valueCheck;
                    dgvErrorRecords.Rows[e.RowIndex].Cells["EditedCells"].Value = oldEditCells;
                }
            }
            if (e.ColumnIndex == dgvErrorRecords.Columns["TransCompanyNum"].Index)
            {
                if (!dataProvider.checkTransCompanyID(dgvErrorRecords.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) && (dgvErrorRecords.CurrentRow.Cells["EditedCells"].Value.ToString().Contains(" " + dgvErrorRecords.Columns["TransCompanyNum"].Index + ",") || dgvErrorRecords.CurrentRow.Cells["EditedCells"].Value.ToString().Contains(dgvErrorRecords.Columns["TransCompanyNum"].Index.ToString())))
                {
                    MessageBox.Show("Transformer Company Number not found", "G/Technology");
                    dgvErrorRecords.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = valueCheck;
                    dgvErrorRecords.Rows[e.RowIndex].Cells["EditedCells"].Value = oldEditCells;
                }
            }
        }
    }
}
