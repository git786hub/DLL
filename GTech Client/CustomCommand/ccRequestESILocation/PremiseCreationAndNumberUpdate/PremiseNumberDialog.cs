using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Data.OleDb;

namespace GTechnology.Oncor.CustomAPI
{
    public partial class PremiseNumberDialog : Form
    {
        OleDbDataAdapter odaListMaker;
        DataTable dtList;
        List<int> changedRows = new List<int>();
        object dataCheck;
        int dataCheckRow;
        string dataColumnEdit;
        int dataCheckColumn;
        bool updateMap = false;
        public PremiseNumberDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Loads the data into the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
            dataProvider.LoadData();
            if (dataProvider.PremiseRecords.State != (int)ADODB.ObjectStateEnum.adStateClosed && dataProvider.ErrorMessage == null)
            {
                dataProvider.PremiseRecords.MoveFirst();
                dataProvider.PremiseRecords.MoveLast();
                odaListMaker = new OleDbDataAdapter();
                dtList = new DataTable();
                bindingSource1.DataSource = dataProvider.PremiseRecords;

                if (dataProvider.PremiseRecords.RecordCount > 0)
                {
                    odaListMaker.Fill(dtList, dataProvider.PremiseRecords);
                    dtList.Columns.Add("Request", Type.GetType("System.Boolean"));
                    dtList.Columns.Add("Subdivision", Type.GetType("System.String"));
                    dtList.Columns.Add("Underground", Type.GetType("System.Boolean"));
                    dtList.Columns.Add("MarkedOrSubbed", Type.GetType("System.Boolean"));
                    dtList.Columns.Add("Pool", Type.GetType("System.Boolean"));
                    dtList.Columns.Add("PathClear", Type.GetType("System.Boolean"));
                    dtList.Columns.Add("StageOfConstruction", Type.GetType("System.String"));
                    dtList.Columns.Add("EditedCells", Type.GetType("System.String"));//maybe just do the column name instead of the indexes. double digits caused issues.
                    bindingSource1.DataSource = dtList;
                    foreach (DataGridViewColumn column in dgvPremises.Columns)
                    {
                        if (dataProvider.ColumnRules.ContainsKey(column.DataPropertyName) && column.GetType().ToString().Contains("TextBox"))
                        {
                            if (dataProvider.ColumnRules[column.DataPropertyName].ElementAt((int)dataProvider.MetaValue.DATA_TYPE) == "VARCHAR2")
                            {
                                ((DataGridViewTextBoxColumn)column).MaxInputLength = Convert.ToInt32(dataProvider.ColumnRules[column.DataPropertyName].ElementAt((int)dataProvider.MetaValue.DATA_LEGNTH));
                            }
                            else if (dataProvider.ColumnRules[column.DataPropertyName].ElementAt((int)dataProvider.MetaValue.DATA_TYPE) == "NUMBER")
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
                    dgvPremises.DataSource = bindingSource1;
                    dtList.AcceptChanges();
                    bool requiredFulfilled = true;
                    bool readOnly = false;
                    foreach (DataGridViewRow row in dgvPremises.Rows)
                    {
                        requiredFulfilled = true;
                        readOnly = false;
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            if (dataProvider.RequiredColumns.Contains(cell.OwningColumn.DataPropertyName) && (cell.Value.ToString() == "" || cell.Value.ToString() == "{}"))
                            {
                                requiredFulfilled = false;
                                readOnly = true;
                            }
                        }
                        if (row.Cells["RequestDate"].Value.ToString() != "" && requiredFulfilled)
                        {
                            requiredFulfilled = false;
                        }
                        row.Cells["Request"].Value = requiredFulfilled;
                        row.Cells["Request"].ReadOnly = readOnly;
                        row.Cells["RequestDate"].Style.BackColor = Color.Gainsboro;

                    }
                        dgvPremises.EndEdit();

                }
                else
                {
                    MessageBox.Show(dataProvider.ErrorMessage, "G/Technology");
                    this.Close();
                }
            }
            else
            {
                if (dataProvider.ErrorMessage == null)
                {
                    dataProvider.ErrorMessage = "All selected Service Points have ESI Locations";
                }
                MessageBox.Show(dataProvider.ErrorMessage, "G/Technology");
                this.Close();
            }
        }
            catch (Exception error)
            {
                MessageBox.Show("Error in Form1_Load (" + error.Message + ")", "G/Technology");
            }
        }

        /// <summary>
        /// Saves the data in the dialog to the db. May need to be moved to the static class.
        /// </summary>
        private void saveFunction()
        {
            //try
            {
                DataTable changeTable = dtList.GetChanges();
                if (changeTable == null)
                    return;
                dataProvider.SaveData(changeTable);
                changedRows.Clear();
                dtList.AcceptChanges();
                bool requiredFulfilled = true;
                bool readOnly = false;
                foreach (DataGridViewRow row in dgvPremises.Rows)
                {
                    requiredFulfilled = true;
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (dataProvider.RequiredColumns.Contains(cell.OwningColumn.DataPropertyName) && (cell.Value.ToString() == "" || cell.Value.ToString() == "{}"))
                        {
                            requiredFulfilled = false;
                            readOnly = true;
                        }
                    }
                    if (row.Cells["RequestDate"].Value.ToString() != "" && requiredFulfilled)
                    {
                        requiredFulfilled = false;
                    }
                    row.Cells["Request"].Value = requiredFulfilled;
                    row.Cells["Request"].ReadOnly = readOnly;
                    row.Cells["EditedCells"].Value = "";
                    readOnly = false;
                }
                cmdSave.Enabled = false;
            }
            //catch(Exception error)
            //{
            //    MessageBox.Show("Error in SaveFunction (" + error.Message + ")", "G/Technology");
            //}
        }

        /// <summary>
        /// Listener for the save button. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSave_Click(object sender, EventArgs e)
        {
            saveFunction();
        }

        /// <summary>
        /// Listener for the export button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSubmit_Click(object sender, EventArgs e)
        {
            dataProvider.ExportData(dgvPremises);
            saveFunction();
        }

        /// <summary>
        /// Listener for the exit button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Method to update the map whenever a row is interacted with, as long as the update map checkbox is checked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPremises_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (updateMap)
                {
                    dataProvider.SelectedServicePoint(Convert.ToInt32(dtList.Rows[e.RowIndex].Field<decimal>("G3E_FID")), Convert.ToInt16(dtList.Rows[e.RowIndex].Field<decimal>("G3E_FNO")));
                }
            }
            catch(Exception error)
            {
                MessageBox.Show("Error in dgvPremises_CellContentClick (" + error.Message + ")", "G/Technology");
            }
        }

        private void dataProviderBindingSource_CurrentChanged(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Closes the dialog window. Should also clear out all data and close any open streams.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PremiseNumberDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (dtList != null && dtList.GetChanges() != null && dtList.GetChanges().Rows.Count > 0 && changedRows.Count > 0)
                {
                    string editsExist = "There are unsaved changes to Premises, would you like to save them now?";
                    string editCaption = "Warning";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
                    DialogResult result = MessageBox.Show(editsExist, editCaption, buttons);
                    if (result == DialogResult.Yes)
                    {
                        saveFunction();
                        changedRows.Clear();
                        dataProvider.Dispose();
                    }
                    else if (result == DialogResult.No)
                    {
                        changedRows.Clear();
                        dataProvider.Dispose();
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
                else
                {
                    changedRows.Clear();
                    dataProvider.Dispose();
                }
                dataProvider.ccHelper.Complete();
            }
            catch(Exception error)
            {
                MessageBox.Show("Error in dataGridView1_CellContentClick (" + error.Message + ")", "G/Technology");
            }
        }

        /// <summary>
        /// Reacts whenever a cell is changed. Stores the location of the changes in a hidden column as a string
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPremises_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1 && dataProvider.ColumnRules.Keys.Contains(dgvPremises.Columns[e.ColumnIndex].DataPropertyName))
                {
                    changedRows.Add(e.RowIndex);
                }
                if (e.RowIndex > -1 && e.ColumnIndex > -1 && dataProvider.RequiredColumns.Contains(dgvPremises.Columns[e.ColumnIndex].DataPropertyName) && changedRows.Count > 0)
                {
                    dgvPremises.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Yellow;
                    if (dgvPremises.Rows[e.RowIndex].Cells["EditedCells"].Value.ToString() == "")
                    {
                        dgvPremises.Rows[e.RowIndex].Cells["EditedCells"].Value += e.ColumnIndex.ToString();
                    }
                    else
                    {
                        dgvPremises.Rows[e.RowIndex].Cells["EditedCells"].Value += "," + e.ColumnIndex.ToString();
                    }
                }
                bool enableSubmit = false;
                foreach (DataGridViewRow row in dgvPremises.Rows)
                {
                    if (row.Cells["Request"].Value.ToString() == "True")
                    {
                        enableSubmit = true;
                    }
                }
                cmdSubmit.Enabled = enableSubmit;
                bool enableSave = false;
                if(changedRows != null && changedRows.Count > 0 && !( dgvPremises.Rows[e.RowIndex].Cells["EditedCells"].Value == null || dgvPremises.Rows[e.RowIndex].Cells["EditedCells"].Value as string == "" ))
                {
                    enableSave = true;
                }
                cmdSave.Enabled = enableSave;
                if (dgvPremises.Rows.Count > 0 && e.ColumnIndex == dgvPremises.Columns["Underground"].Index)
                {
                    dgvPremises.Rows[e.RowIndex].Cells["MarkedOrSubbed"].ReadOnly = !Convert.ToBoolean(dgvPremises.Rows[e.RowIndex].Cells["Underground"].Value);
                    dgvPremises.Rows[e.RowIndex].Cells["MarkedOrSubbed"].Style.BackColor = dgvPremises.Rows[e.RowIndex].Cells["MarkedOrSubbed"].ReadOnly ? Color.Gainsboro : Color.White;
                    dgvPremises.Rows[e.RowIndex].Cells["Pool"].ReadOnly = !Convert.ToBoolean(dgvPremises.Rows[e.RowIndex].Cells["Underground"].Value);
                    dgvPremises.Rows[e.RowIndex].Cells["Pool"].Style.BackColor = dgvPremises.Rows[e.RowIndex].Cells["Pool"].ReadOnly ? Color.Gainsboro : Color.White;
                    dgvPremises.Rows[e.RowIndex].Cells["PathClear"].ReadOnly = !Convert.ToBoolean(dgvPremises.Rows[e.RowIndex].Cells["Underground"].Value);
                    dgvPremises.Rows[e.RowIndex].Cells["PathClear"].Style.BackColor = dgvPremises.Rows[e.RowIndex].Cells["PathClear"].ReadOnly ? Color.Gainsboro : Color.White;
                    dgvPremises.Rows[e.RowIndex].Cells["StageOfCONST"].ReadOnly = !Convert.ToBoolean(dgvPremises.Rows[e.RowIndex].Cells["Underground"].Value);
                    dgvPremises.Rows[e.RowIndex].Cells["StageOfCONST"].Style.BackColor = dgvPremises.Rows[e.RowIndex].Cells["StageofCONST"].ReadOnly ? Color.Gainsboro : Color.White;
                }
            }
            catch(Exception error)
            {
                MessageBox.Show("Error in dgvPremises_CellContentChange (" + error.Message + ")", "G/Technology");
            }
        }

        /// <summary>
        /// Method for handling errors with the data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPremises_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Context.HasFlag(DataGridViewDataErrorContexts.Parsing) || e.Context.HasFlag(DataGridViewDataErrorContexts.Commit))
            {
                MessageBox.Show("Invalid data entered. A " + dgvPremises.Columns[e.ColumnIndex].ValueType.ToString().Remove(0, 7) + " is required.", "G/Technology");
            }
            else
                MessageBox.Show("Something has gone wrong, Please contact support", "G/Technology");
        }

        /// <summary>
        /// Formats cells pretty much anytime anything happens, manages the cell colors after the first change. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPremises_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (dataProvider.RequiredColumns.Contains(dgvPremises.Columns[e.ColumnIndex].DataPropertyName) && dgvPremises.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "")
                {
                    dgvPremises.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Red;
                }
                else if (dgvPremises.Rows[e.RowIndex].Cells["EditedCells"].Value != null && dgvPremises.Rows[e.RowIndex].Cells["EditedCells"].Value as string != "")
                {
                    string[] editedList = dgvPremises.Rows[e.RowIndex].Cells["EditedCells"].Value.ToString().Split(',');
                    if (editedList.Contains(e.ColumnIndex.ToString()))
                    {
                        dgvPremises.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Yellow;
                        cmdSubmit.Enabled = false;
                    }
                }
                else
                {
                        dgvPremises.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
                }
            }
            catch(Exception error)
            {
                MessageBox.Show("Error in dgvPremises_CellFormatting (" + error.Message + ")", "G/Technology");
            }
        }

        /// <summary>
        /// Handler for the Update Map Checkbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            updateMap = !updateMap;
        }

        /// <summary>
        /// Handler for updating the map upon clicking an entry in our data grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPremises_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (updateMap)
                {
                    dataProvider.SelectedServicePoint(Convert.ToInt32(dtList.Rows[e.RowIndex].Field<decimal>("G3E_FID")), Convert.ToInt16(dtList.Rows[e.RowIndex].Field<decimal>("G3E_FNO")));
                }
            }
            catch(Exception error)
            {
                MessageBox.Show("Error in dataGridView1_CellContentClick (" + error.Message + ")", "G/Technology");
            }
        }

        /// <summary>
        /// Ends edit so that checkbox based changes will work correctly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPremises_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == dgvPremises.Columns["Request"].Index || e.ColumnIndex == dgvPremises.Columns["Underground"].Index)
            {
                dgvPremises.EndEdit();
            }
        }

        /// <summary>
        /// Another method to facilitate checkbox interactions, this one is focused on Request column.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPremises_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvPremises.Columns["Request"].Index && e.RowIndex > -1)
            {
                DataGridViewCheckBoxCell checkbox = dgvPremises.Rows[e.RowIndex].Cells["Request"] as DataGridViewCheckBoxCell;
                bool bChecked = ("" != checkbox.Value.ToString() && null != checkbox && null != checkbox.Value && true == (bool)checkbox.Value);
                if (dgvPremises.Rows[e.RowIndex].Cells["RequestDate"].Value.ToString() != "" && !bChecked)
                {
                    MessageBox.Show("Record has already been submitted, please uncheck if you do not wish to resubmit.", "G/Technology");
                    checkbox.Value = true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPremises_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            dataCheck = dgvPremises.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            dataCheckRow = e.RowIndex;
            dataColumnEdit = dgvPremises.Rows[e.RowIndex].Cells["EditedCells"].Value as string;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPremises_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvPremises.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                if (dataCheck == null || dataCheck.ToString() != dgvPremises.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString())
                {
                    dgvPremises_CellValueChanged(sender, e);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPremises_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvPremises.IsCurrentCellDirty)
            {
                dgvPremises.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgvPremises_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && dataColumnEdit != null)
            {
                dgvPremises.Rows[dataCheckRow].Cells["EditedCells"].Value = dataColumnEdit;
            }
        }

        private void dgvPremises_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Console.Write("Testboy");
            }
        }
    }
}