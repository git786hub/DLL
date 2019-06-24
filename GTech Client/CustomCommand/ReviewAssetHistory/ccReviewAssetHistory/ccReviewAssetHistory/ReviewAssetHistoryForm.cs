// ===================================================
//  Copyright 2018 Intergraph Corp.
//  File Name: ReviewAssetHistoryForm.cs
// 
//  Description: ReviewAssetHistoryForm form is used to show the asset history.
//
//  Remarks:
// 
//  Modification History
//  Date                Author                      Comments
//  --------            --------------              --------
//  09/01/2018          Sithara                     
// ======================================================
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Intergraph.GTechnology.API;
using System.IO;
using ADODB;
using Excel = Microsoft.Office.Interop.Excel;

namespace GTechnology.Oncor.CustomAPI
{
    public partial class ReviewAssetHistoryForm : Form
    {

        AssetHistoryModel m_assetHistoryModel;
        IGTTransactionManager m_oGTTransactionManager;
        IGTCustomCommandHelper m_GTCustomCommandHelper;
        DataGridViewColumnSelector m_dataGridViewColumnSelector;
        public ReviewAssetHistoryForm(AssetHistoryModel assetHistoryModel, IGTTransactionManager oGTTransactionManager, IGTCustomCommandHelper GTCustomCommandHelper)
        {
            try
            {
                m_dataGridViewColumnSelector = null;
                m_assetHistoryModel = assetHistoryModel;
                m_oGTTransactionManager = oGTTransactionManager;
                m_GTCustomCommandHelper = GTCustomCommandHelper;

                InitializeComponent();

                if (!assetHistoryModel.m_DataContext.IsRoleGranted("ADMINISTRATOR"))
                {
                    btnSaveView.Visible = false;
                }
                if (m_assetHistoryModel.m_FID != 0 && !m_assetHistoryModel.m_isStructure)
                {
                    txtFid.Text = Convert.ToString(m_assetHistoryModel.m_FID);
                }

                LoadDatagridView(null);

                LoadComboBox();

                cmbNamedView.SelectedIndex = -1;

                m_dataGridViewColumnSelector = new DataGridViewColumnSelector(dgHistory, m_assetHistoryModel);

                if (dgHistory.Rows.Count > 0)
                {
                    btnSaveView.Enabled = true;
                    btnExport.Enabled = true;
                }
                else
                {
                    btnSaveView.Enabled = false;
                    btnExport.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during execution of Review Asset History custom command." + Environment.NewLine + ex.Message,
                 "G/Techonology", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReviewAssetHistoryForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (m_assetHistoryModel.m_isStructure)
                {
                    txtSid.Text = m_assetHistoryModel.m_StructureID;
                }
                else if (!string.IsNullOrEmpty(m_assetHistoryModel.m_WRID))
                {
                    txtWr.Text = m_assetHistoryModel.m_WRID;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during execution of Review Asset History custom command." + Environment.NewLine + ex.Message,
                   "G/Techonology", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Loads the combo box view data.
        /// </summary>
        private void LoadComboBox()
        {
            Recordset rsViews = null;
            try
            {
                rsViews = m_assetHistoryModel.LoadAssetHistoryNamedViews();
                cmbNamedView.DisplayMember = "Text";
                cmbNamedView.ValueMember = "Value";

                if (rsViews != null && rsViews.RecordCount > 0)
                {
                    rsViews.MoveFirst();
                    cmbNamedView.Items.Clear();
                    while (!rsViews.EOF)
                    {
                        cmbNamedView.Items.Add(new { Text = Convert.ToString(rsViews.Fields[1].Value), Value = Convert.ToInt16(rsViews.Fields[0].Value) });
                        rsViews.MoveNext();
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (rsViews != null)
                {
                    rsViews.Close();
                    rsViews = null;
                }
            }
        }

        /// <summary>
        /// Go button will load datagridview based on quer by group box data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
                m_dataGridViewColumnSelector = null;

                LoadDatagridView(null);
                cmbNamedView.SelectedIndex = -1;

                m_dataGridViewColumnSelector = new DataGridViewColumnSelector(dgHistory, m_assetHistoryModel);

                if (dgHistory.Rows.Count > 0)
                {
                    btnSaveView.Enabled = true;
                    btnExport.Enabled = true;
                }
                else
                {
                    btnSaveView.Enabled = false;
                    btnExport.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during execution of Review Asset History custom command." + Environment.NewLine + ex.Message,
                  "G/Techonology", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Loads the datagridview.
        /// </summary>
        private void LoadDatagridView(DataView dv)
        {
            dgHistory.DataSource = null;

            if (dv == null)
            {
                dgHistory.DataSource = typeof(DataTable);

                dgHistory.DataSource = m_assetHistoryModel.LoadGridData();
                dgHistory.Columns["Designer"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            else
            {
                dgHistory.DataSource = typeof(DataView);

                dgHistory.DataSource = dv;
                dgHistory.Columns["Designer"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            dgHistory.EndEdit();
            dgHistory.Refresh();

            dgHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgHistory.AutoResizeColumns();



            foreach (DataGridViewColumn dc in dgHistory.Columns)
            {
                dc.SortMode = DataGridViewColumnSortMode.Programmatic;
            }
        }

        /// <summary>
        /// Save view open the Save view form to save the operation on datagridview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveView_Click(object sender, EventArgs e)
        {
            try
            {

                SaveViewForm ccSaveViewFrm = new SaveViewForm(m_assetHistoryModel.m_DataContext, m_dataGridViewColumnSelector);
                ccSaveViewFrm.StartPosition = FormStartPosition.CenterParent;

                if (ccSaveViewFrm.ShowDialog(this) == DialogResult.OK)
                {
                    LoadComboBox();
                    cmbNamedView.SelectedIndex = cmbNamedView.Items.Count - 1;

                    if (m_dataGridViewColumnSelector != null)
                    {
                        m_dataGridViewColumnSelector.mSortedDataList = new List<ccSortedData>();
                        m_dataGridViewColumnSelector = new DataGridViewColumnSelector(dgHistory, m_assetHistoryModel);
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during execution of Review Asset History custom command." + Environment.NewLine + ex.Message,
                   "G/Techonology", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Exporting datagridview data to excel sheet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgHistory.Rows.Count > 0)
                {

                    //Getting the location and file name of the excel to save from user. 
                    SaveFileDialog saveDialog = new SaveFileDialog();
                    saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
                    saveDialog.FilterIndex = 2;


                    if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        m_assetHistoryModel.m_Application.BeginWaitCursor();
                        Excel.Application xlApp;
                        Excel.Workbook xlWorkBook  = null; 
                        Excel.Worksheet xlWorkSheet =null;
                        string strFilepath = "";
                        object misValue = System.Reflection.Missing.Value;
                        bool bFileExists = false;                       

                        xlApp = new Excel.Application();

                        if (File.Exists(saveDialog.FileName))
                        {
                            xlWorkBook = xlApp.Workbooks.Open(saveDialog.FileName);
                            if (xlWorkBook.ReadOnly)
                            {
                                MessageBox.Show("File is locked by other process, close and continue", "G/Techonology");
                                return;
                            }
                            bFileExists = true;
                        }
                        else
                        {
                            xlWorkBook = xlApp.Workbooks.Add(misValue);
                        }
                       
                        xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                        xlWorkSheet.Cells.Clear();
                        xlWorkSheet.Name = "Asset History";
                        int i = 0;
                        int j = 0;

                        foreach (DataGridViewColumn dc in dgHistory.Columns)
                        {
                            xlWorkSheet.Cells[1, dc.Index + 1] = dc.Name;
                            xlWorkSheet.Columns.AutoFit();
                        }
                        for (i = 0; i <= dgHistory.RowCount - 1; i++)
                        {
                            for (j = 0; j <= dgHistory.ColumnCount - 1; j++)
                            {
                                DataGridViewCell cell = dgHistory[j, i];
                                xlWorkSheet.Cells[i + 2, j + 1] = cell.Value;
                                xlWorkSheet.Columns.AutoFit();
                            }
                        }
                        xlWorkSheet.Range["A1", "Z1"].EntireRow.Font.Bold = true;

                        if (bFileExists)
                        {
                            xlWorkBook.Save(); //Avoiding SaveAs dialog as it throws error if No or Cancel is clicked
                        }
                        else
                        {
                            xlWorkBook.SaveAs(saveDialog.FileName); 
                        }
                     
                        xlWorkBook.Close(true, misValue, misValue);
                        strFilepath = Path.GetFullPath(saveDialog.FileName);
                        MessageBox.Show("Export is completed.", "G/Techonology", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        xlApp.Visible = true;
                        Excel.Workbooks xlWorkBooks = xlApp.Workbooks;
                        xlWorkBook = xlWorkBooks.Open(strFilepath);
                        releaseObject(xlWorkSheet);
                        releaseObject(xlWorkBook);
                        releaseObject(xlApp);

                        m_assetHistoryModel.m_Application.EndWaitCursor();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during execution of Review Asset History custom command." + Environment.NewLine + ex.Message,
                 "G/Techonology", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                m_assetHistoryModel.m_Application.EndWaitCursor();
            }
        }

        /// <summary>
        /// Releases all Excel objects.
        /// </summary>
        /// <param name="obj"></param>
        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Error during execution of Review Asset History custom command." + Environment.NewLine + ex.Message,
                 "G/Techonology", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                GC.Collect();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.FormClosing -= ReviewAssetHistoryForm_FormClosing;
                CloseForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during execution of Review Asset History custom command." + Environment.NewLine + ex.Message,
                 "G/Techonology", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReviewAssetHistoryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.FormClosing -= ReviewAssetHistoryForm_FormClosing;
                CloseForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during execution of Review Asset History custom command." + Environment.NewLine + ex.Message,
                 "G/Techonology", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Release all objects / all instances and dispose datagridview and the datatable.
        /// </summary>
        private void CloseForm()
        {
            try
            {
                dgvReviewHistory.Dispose();
                m_dataGridViewColumnSelector = null;
                if (m_assetHistoryModel.m_GridDataTable != null)
                    m_assetHistoryModel.m_GridDataTable.Dispose();
                m_assetHistoryModel.ExitCommand(m_oGTTransactionManager, m_GTCustomCommandHelper);
                m_assetHistoryModel = null;

                this.Close();
            }
            catch
            {
                throw;
            }
        }

        #region Textbox events
        private void txtFid_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtFid.Text) && IsNumeric(txtFid.Text))
            {
                m_assetHistoryModel.m_FID = Convert.ToInt32(txtFid.Text);
                m_assetHistoryModel.m_StructureID = null;
                m_assetHistoryModel.m_WRID = null;
                m_assetHistoryModel.m_isStructure = false;

                txtSid.Text = "";
                txtWr.Text = "";
            }
            else if (!String.IsNullOrEmpty(txtFid.Text) && !IsNumeric(txtFid.Text))
            {
                txtFid.Text = null;
                MessageBox.Show("Please enter numeric value.", "G/Techonology", MessageBoxButtons.OK, MessageBoxIcon.Warning,
        MessageBoxDefaultButton.Button1);
            }
            else if (String.IsNullOrEmpty(txtFid.Text))
            {
                m_assetHistoryModel.m_FID = 0;
            }
        }

        private void txtWr_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtWr.Text))
            {
                txtSid.Text = "";
                txtFid.Text = "";

                m_assetHistoryModel.m_FID = 0;
                m_assetHistoryModel.m_StructureID = null;
                m_assetHistoryModel.m_WRID = txtWr.Text;
                m_assetHistoryModel.m_isStructure = false;
            }
            else if (String.IsNullOrEmpty(txtWr.Text))
            {
                m_assetHistoryModel.m_WRID = null;
            }
        }

        private void txtSid_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtSid.Text))
            {
                txtWr.Text = "";
                txtFid.Text = "";

                m_assetHistoryModel.m_FID = 0;
                m_assetHistoryModel.m_StructureID = txtSid.Text;
                m_assetHistoryModel.m_WRID = null;
                m_assetHistoryModel.m_isStructure = true;
            }
            else if (String.IsNullOrEmpty(txtSid.Text))
            {
                m_assetHistoryModel.m_StructureID = null;
            }
        }

        #endregion

        /// <summary>
        /// This method validates that the given string is numeric or not.
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        private bool IsNumeric(string txt)
        {
            bool isNumeric = true;

            foreach (char c in txt)
            {
                if (!Char.IsNumber(c))
                {
                    isNumeric = false;
                    break;
                }
            }

            return isNumeric;
        }

        /// <summary>
        /// Combobox event: It will apply the saved filters and sorted column order on the current datagridview based on combo box value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbNamedView_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strFilterExp = "";
            string strSortOrder = "";
            DataView dv;
            Recordset rs = null;
            short viewId = 0;
            try
            {
                if (m_dataGridViewColumnSelector != null)
                {
                    m_dataGridViewColumnSelector = null;
                    m_dataGridViewColumnSelector = new DataGridViewColumnSelector(dgHistory, m_assetHistoryModel);
                }

                //LoadDatagridView(null);

                if (cmbNamedView.SelectedItem != null)
                {
                    viewId = Convert.ToInt16((cmbNamedView.SelectedItem as dynamic).Value);
                }
                if (viewId != 0)
                {
                    //dv = m_assetHistoryModel.m_GridDataTable.DefaultView;
                    dv = m_assetHistoryModel.LoadGridData().DefaultView;

                    rs = ExecuteCommand(string.Format("select FILTER_COLUMN,FILTER_VALUE from asset_history_viewfilter where view_id= {0}", viewId));
                    if (rs != null && rs.RecordCount > 0)
                    {
                        strFilterExp = GetFiltersExpressionOfView(strFilterExp, rs);
                    }

                    rs = null;
                    rs = ExecuteCommand(string.Format("select SORT_COLUMN,SORT_DIRECTION,SORT_PRIORITY from ASSET_HISTORY_VIEWSORT where VIEW_ID={0} order by SORT_PRIORITY", viewId));
                    if (rs != null && rs.RecordCount > 0)
                    {
                        strSortOrder = GetSortOrderExpressionOfView(strSortOrder, rs);
                    }

                    if (!string.IsNullOrEmpty(strFilterExp))
                    {
                        dv.RowFilter = strFilterExp;
                    }
                    if (!string.IsNullOrEmpty(strSortOrder))
                    {
                        dv.Sort = strSortOrder;
                    }
                    if (!string.IsNullOrEmpty(strSortOrder) || !string.IsNullOrEmpty(strFilterExp))
                    {
                        LoadDatagridView(dv);
                    }
                    else if (string.IsNullOrEmpty(strSortOrder) && string.IsNullOrEmpty(strFilterExp))
                    {
                        LoadDatagridView(null);
                    }
                }

                if (dgHistory.Rows.Count > 0)
                {
                    btnSaveView.Enabled = true;
                    btnExport.Enabled = true;
                }
                else
                {
                    btnSaveView.Enabled = false;
                    btnExport.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during execution of Review Asset History custom command." + Environment.NewLine + ex.Message,
                    "G/Techonology", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (rs != null)
                {
                    if (rs.State == 1)
                    {
                        rs.Close();
                        rs.ActiveConnection = null;
                    }
                    rs = null;
                }
            }
        }

        /// <summary>
        /// Getting sort expression of named view while changing the named view combo box to apply on the datagridview.
        /// </summary>
        /// <param name="strSortOrder"></param>
        /// <param name="rs"></param>
        /// <returns></returns>
        private static string GetSortOrderExpressionOfView(string strSortOrder, Recordset rs)
        {
            if (rs != null && rs.RecordCount > 0)
            {
                rs.MoveFirst();
                while (!rs.EOF)
                {
                    if (string.IsNullOrEmpty(strSortOrder))
                    {
                        strSortOrder = rs.Fields["sort_column"].Value + " " + rs.Fields["sort_direction"].Value;
                    }
                    else
                    {
                        strSortOrder += "," + rs.Fields["sort_column"].Value + " " + rs.Fields["sort_direction"].Value;
                    }
                    rs.MoveNext();
                }
            }

            return strSortOrder;
        }

        /// <summary>
        /// Getting filter expression of named view while changing the named view combo box to apply on the datagridview.
        /// </summary>
        /// <param name="strFilterExp"></param>
        /// <param name="rs"></param>
        /// <returns></returns>
        private string GetFiltersExpressionOfView(string strFilterExp, Recordset rs)
        {
            string strOldFilterColumn = "";

            if (rs != null && rs.RecordCount > 0)
            {
                rs.MoveFirst();

                while (!rs.EOF)
                {
                    if (!string.IsNullOrEmpty(strOldFilterColumn) && strOldFilterColumn != Convert.ToString(rs.Fields["filter_column"].Value))
                    {
                        strFilterExp += ") AND (";
                    }

                    strOldFilterColumn = Convert.ToString(rs.Fields["filter_column"].Value);
                    if (String.IsNullOrEmpty(strFilterExp))
                    {
                        strFilterExp = "(" + "[" + rs.Fields["filter_column"].Value + "]" + " IN ( '" + rs.Fields["filter_value"].Value + "')";
                    }
                    else
                    {
                        strFilterExp += " OR " + "" + "[" + rs.Fields["filter_column"].Value + "]" + " IN ( '" + rs.Fields["filter_value"].Value + "')";
                    }

                    CheckTheListBoxFiltersOfNamedView(rs);
                    rs.MoveNext();
                }
            }
            strFilterExp += ")";
            strFilterExp = strFilterExp.Replace("AND ( OR", "AND (");
            return strFilterExp;
        }

        /// <summary>
        /// While restoring the named view we are checking the filters already stored in the view. 
        /// </summary>
        /// <param name="rs"></param>
        private void CheckTheListBoxFiltersOfNamedView(Recordset rs)
        {
            if (Convert.ToString(rs.Fields["filter_column"].Value) == "User")
            {
                for (int i = 0; i < m_dataGridViewColumnSelector.mDropdownListBoxUser.Items.Count; i++)
                {
                    if (Convert.ToString(m_dataGridViewColumnSelector.mDropdownListBoxUser.Items[i]) == Convert.ToString(rs.Fields["filter_value"].Value))
                    {
                        m_dataGridViewColumnSelector.mDropdownListBoxUser.SelectedIndex = i;
                    }
                }
            }
            else if (Convert.ToString(rs.Fields["filter_column"].Value) == "Operation")
            {
                for (int i = 0; i < m_dataGridViewColumnSelector.mDropdownListBoxOperation.Items.Count; i++)
                {
                    if (Convert.ToString(m_dataGridViewColumnSelector.mDropdownListBoxOperation.Items[i]) == Convert.ToString(rs.Fields["filter_value"].Value))
                    {
                        m_dataGridViewColumnSelector.mDropdownListBoxOperation.SelectedIndex = i;
                    }
                }
            }
            else if (Convert.ToString(rs.Fields["filter_column"].Value) == "Feature Class")
            {
                for (int i = 0; i < m_dataGridViewColumnSelector.mDropdownListBoxFeatureClass.Items.Count; i++)
                {
                    if (Convert.ToString(m_dataGridViewColumnSelector.mDropdownListBoxFeatureClass.Items[i]) == Convert.ToString(rs.Fields["filter_value"].Value))
                    {
                        m_dataGridViewColumnSelector.mDropdownListBoxFeatureClass.SelectedIndex = i;
                    }
                }
            }
            else if (Convert.ToString(rs.Fields["filter_column"].Value) == "Component")
            {
                for (int i = 0; i < m_dataGridViewColumnSelector.mDropdownListBoxComponent.Items.Count; i++)
                {
                    if (Convert.ToString(m_dataGridViewColumnSelector.mDropdownListBoxComponent.Items[i]) == Convert.ToString(rs.Fields["filter_value"].Value))
                    {
                        m_dataGridViewColumnSelector.mDropdownListBoxComponent.SelectedIndex = i;
                    }
                }
            }
            else if (Convert.ToString(rs.Fields["filter_column"].Value) == "Attribute")
            {
                for (int i = 0; i < m_dataGridViewColumnSelector.mDropdownListBoxAttribute.Items.Count; i++)
                {
                    if (Convert.ToString(m_dataGridViewColumnSelector.mDropdownListBoxAttribute.Items[i]) == Convert.ToString(rs.Fields["filter_value"].Value))
                    {
                        m_dataGridViewColumnSelector.mDropdownListBoxAttribute.SelectedIndex = i;
                    }
                }
            }
            else if (Convert.ToString(rs.Fields["filter_column"].Value) == "Old Value")
            {
                for (int i = 0; i < m_dataGridViewColumnSelector.mDropdownListBoxOldValue.Items.Count; i++)
                {
                    if (Convert.ToString(m_dataGridViewColumnSelector.mDropdownListBoxOldValue.Items[i]) == Convert.ToString(rs.Fields["filter_value"].Value))
                    {
                        m_dataGridViewColumnSelector.mDropdownListBoxOldValue.SelectedIndex = i;
                    }
                }
            }
            else if (Convert.ToString(rs.Fields["filter_column"].Value) == "New Value")
            {
                for (int i = 0; i < m_dataGridViewColumnSelector.mDropdownListBoxNewValue.Items.Count; i++)
                {
                    if (Convert.ToString(m_dataGridViewColumnSelector.mDropdownListBoxNewValue.Items[i]) == Convert.ToString(rs.Fields["filter_value"].Value))
                    {
                        m_dataGridViewColumnSelector.mDropdownListBoxNewValue.SelectedIndex = i;
                    }
                }
            }
            else if (Convert.ToString(rs.Fields["filter_column"].Value) == "Designer")
            {
                for (int i = 0; i < m_dataGridViewColumnSelector.mDropdownListBoxDesigner.Items.Count; i++)
                {
                    if (Convert.ToString(m_dataGridViewColumnSelector.mDropdownListBoxDesigner.Items[i]) == Convert.ToString(rs.Fields["filter_value"].Value))
                    {
                        m_dataGridViewColumnSelector.mDropdownListBoxDesigner.SelectedIndex = i;
                    }
                }
            }
        }

        private Recordset ExecuteCommand(string sql)
        {
            try
            {
                Recordset rs = m_assetHistoryModel.m_DataContext.OpenRecordset(sql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText,
                        null);
                return rs;
            }
            catch
            {
                throw;
            }
        }


        #region Datagridview events

        /// <summary>
        /// To show filter in the grid by right-clicking on a column header.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgHistory_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right && e.RowIndex == -1)
                {
                    if (dgHistory.Columns[e.ColumnIndex].Name == "User")
                    {
                        if (m_dataGridViewColumnSelector.mDropdownListBoxUser.Items.Count > 0)
                        {
                            m_dataGridViewColumnSelector.mDropdownListBoxUser.ClientSize = new Size(m_dataGridViewColumnSelector.mDropdownListBoxUser.ClientSize.Width, m_dataGridViewColumnSelector.mDropdownListBoxUser.GetItemHeight(0) * m_dataGridViewColumnSelector.mDropdownListBoxUser.Items.Count);
                            m_dataGridViewColumnSelector.mPopupUser.Height = m_dataGridViewColumnSelector.mDropdownListBoxUser.ClientSize.Height;
                            m_dataGridViewColumnSelector.mPopupUser.Width = m_dataGridViewColumnSelector.mDropdownListBoxUser.ClientSize.Width;
                            m_dataGridViewColumnSelector.mPopupUser.Show(Control.MousePosition);
                        }
                    }

                    if (dgHistory.Columns[e.ColumnIndex].Name == "Operation")
                    {
                        if (m_dataGridViewColumnSelector.mDropdownListBoxOperation.Items.Count > 0)
                        {
                            m_dataGridViewColumnSelector.mDropdownListBoxOperation.ClientSize = new Size(m_dataGridViewColumnSelector.mDropdownListBoxOperation.ClientSize.Width, m_dataGridViewColumnSelector.mDropdownListBoxOperation.GetItemHeight(0) * m_dataGridViewColumnSelector.mDropdownListBoxOperation.Items.Count);
                            m_dataGridViewColumnSelector.mPopupOperation.Height = m_dataGridViewColumnSelector.mDropdownListBoxOperation.ClientSize.Height;
                            m_dataGridViewColumnSelector.mPopupOperation.Width = m_dataGridViewColumnSelector.mDropdownListBoxOperation.ClientSize.Width;
                            m_dataGridViewColumnSelector.mPopupOperation.Show(Control.MousePosition);
                        }
                    }

                    if (dgHistory.Columns[e.ColumnIndex].Name == "Feature Class")
                    {
                        if (m_dataGridViewColumnSelector.mDropdownListBoxFeatureClass.Items.Count > 0)
                        {
                            m_dataGridViewColumnSelector.mDropdownListBoxFeatureClass.ClientSize = new Size(m_dataGridViewColumnSelector.mDropdownListBoxFeatureClass.ClientSize.Width, m_dataGridViewColumnSelector.mDropdownListBoxFeatureClass.GetItemHeight(0) * m_dataGridViewColumnSelector.mDropdownListBoxFeatureClass.Items.Count);
                            m_dataGridViewColumnSelector.mPopupFeatureClass.Height = m_dataGridViewColumnSelector.mDropdownListBoxFeatureClass.ClientSize.Height;
                            m_dataGridViewColumnSelector.mPopupFeatureClass.Width = m_dataGridViewColumnSelector.mDropdownListBoxFeatureClass.ClientSize.Width;
                            m_dataGridViewColumnSelector.mPopupFeatureClass.Show(Control.MousePosition);
                        }
                    }

                    if (dgHistory.Columns[e.ColumnIndex].Name == "Component")
                    {
                        if (m_dataGridViewColumnSelector.mDropdownListBoxComponent.Items.Count > 0)
                        {
                            m_dataGridViewColumnSelector.mDropdownListBoxComponent.ClientSize = new Size(m_dataGridViewColumnSelector.mDropdownListBoxComponent.ClientSize.Width, m_dataGridViewColumnSelector.mDropdownListBoxComponent.GetItemHeight(0) * m_dataGridViewColumnSelector.mDropdownListBoxComponent.Items.Count);
                            m_dataGridViewColumnSelector.mPopupComponent.Height = m_dataGridViewColumnSelector.mDropdownListBoxComponent.ClientSize.Height;
                            m_dataGridViewColumnSelector.mPopupComponent.Width = m_dataGridViewColumnSelector.mDropdownListBoxComponent.ClientSize.Width;
                            m_dataGridViewColumnSelector.mPopupComponent.Show(Control.MousePosition);
                        }
                    }

                    if (dgHistory.Columns[e.ColumnIndex].Name == "Attribute")
                    {
                        if (m_dataGridViewColumnSelector.mDropdownListBoxAttribute.Items.Count > 0)
                        {
                            m_dataGridViewColumnSelector.mDropdownListBoxAttribute.ClientSize = new Size(m_dataGridViewColumnSelector.mDropdownListBoxAttribute.ClientSize.Width, m_dataGridViewColumnSelector.mDropdownListBoxAttribute.GetItemHeight(0) * m_dataGridViewColumnSelector.mDropdownListBoxAttribute.Items.Count);
                            m_dataGridViewColumnSelector.mPopupAttribute.Height = m_dataGridViewColumnSelector.mDropdownListBoxAttribute.ClientSize.Height;
                            m_dataGridViewColumnSelector.mPopupAttribute.Width = m_dataGridViewColumnSelector.mDropdownListBoxAttribute.ClientSize.Width;
                            m_dataGridViewColumnSelector.mPopupAttribute.Show(Control.MousePosition);
                        }
                    }

                    if (dgHistory.Columns[e.ColumnIndex].Name == "Old Value")
                    {
                        if (m_dataGridViewColumnSelector.mDropdownListBoxOldValue.Items.Count > 0)
                        {
                            m_dataGridViewColumnSelector.mDropdownListBoxOldValue.ClientSize = new Size(m_dataGridViewColumnSelector.mDropdownListBoxOldValue.ClientSize.Width, m_dataGridViewColumnSelector.mDropdownListBoxOldValue.GetItemHeight(0) * m_dataGridViewColumnSelector.mDropdownListBoxOldValue.Items.Count);
                            m_dataGridViewColumnSelector.mPopupOldValue.Height = m_dataGridViewColumnSelector.mDropdownListBoxOldValue.ClientSize.Height;
                            m_dataGridViewColumnSelector.mPopupOldValue.Width = m_dataGridViewColumnSelector.mDropdownListBoxOldValue.ClientSize.Width;
                            m_dataGridViewColumnSelector.mPopupOldValue.Show(Control.MousePosition);
                        }
                    }

                    if (dgHistory.Columns[e.ColumnIndex].Name == "New Value")
                    {
                        if (m_dataGridViewColumnSelector.mDropdownListBoxNewValue.Items.Count > 0)
                        {
                            m_dataGridViewColumnSelector.mDropdownListBoxNewValue.ClientSize = new Size(m_dataGridViewColumnSelector.mDropdownListBoxNewValue.ClientSize.Width, m_dataGridViewColumnSelector.mDropdownListBoxNewValue.GetItemHeight(0) * m_dataGridViewColumnSelector.mDropdownListBoxNewValue.Items.Count);
                            m_dataGridViewColumnSelector.mPopupNewValue.Height = m_dataGridViewColumnSelector.mDropdownListBoxNewValue.ClientSize.Height;
                            m_dataGridViewColumnSelector.mPopupNewValue.Width = m_dataGridViewColumnSelector.mDropdownListBoxNewValue.ClientSize.Width;
                            m_dataGridViewColumnSelector.mPopupNewValue.Show(Control.MousePosition);
                        }
                    }

                    if (dgHistory.Columns[e.ColumnIndex].Name == "Designer")
                    {
                        if (m_dataGridViewColumnSelector.mDropdownListBoxDesigner.Items.Count > 0)
                        {
                            m_dataGridViewColumnSelector.mDropdownListBoxDesigner.ClientSize = new Size(m_dataGridViewColumnSelector.mDropdownListBoxDesigner.ClientSize.Width, m_dataGridViewColumnSelector.mDropdownListBoxDesigner.GetItemHeight(0) * m_dataGridViewColumnSelector.mDropdownListBoxDesigner.Items.Count);
                            m_dataGridViewColumnSelector.mPopupDesigner.Height = m_dataGridViewColumnSelector.mDropdownListBoxDesigner.ClientSize.Height;
                            m_dataGridViewColumnSelector.mPopupDesigner.Width = m_dataGridViewColumnSelector.mDropdownListBoxDesigner.ClientSize.Width;
                            m_dataGridViewColumnSelector.mPopupDesigner.Show(Control.MousePosition);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// On user double click column will sorted based on the conditions.(We are tracking all the coulmns sorted history to support multi column sorting if we require.)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgHistory_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            try
            {
                if (dgHistory.Columns[e.ColumnIndex].SortMode != DataGridViewColumnSortMode.NotSortable)
                {
                    string strSortOrder = "";
                    DataTable dtSortTable;
                    DataView dtSortView = null;
                    bool inCollection = false;
                    int count = 0;
                    int sortCount = 1;

                    if (dgHistory.DataSource is DataTable)
                    {
                        dtSortTable = (DataTable)dgHistory.DataSource;
                        dtSortView = dtSortTable.DefaultView;
                    }
                    else if (this.dgHistory.DataSource is DataView)
                    {
                        dtSortView = (DataView)dgHistory.DataSource;
                    }


                    foreach (ccSortedData sorted in m_dataGridViewColumnSelector.mSortedDataList)
                    {
                        count++;
                        if (sorted.m_SortedColumn == dgHistory.Columns[e.ColumnIndex].Name)
                        {
                            inCollection = true;
                            if (sorted.m_SortedDirection == "ASC")
                            {
                                sortCount = 2;
                            }
                            else if (sorted.m_SortedDirection == "DESC")
                            {
                                sortCount = 3;
                            }
                            break;
                        }
                    }

                    ///On first double click column comes to ASC order ,On second click DESC order and we are loading the initial datagridview on third click.


                    if (inCollection && sortCount == 3)
                    {
                        m_dataGridViewColumnSelector.mSortedDataList.Clear();
                        LoadDatagridView(null);
                    }
                    else if (inCollection && sortCount == 2)
                    {
                        m_dataGridViewColumnSelector.mSortedDataList.RemoveAt(count - 1);
                        m_dataGridViewColumnSelector.mSortedDataList.Add(new ccSortedData(dgHistory.Columns[e.ColumnIndex].Name, "DESC",
                            m_dataGridViewColumnSelector.mSortedDataList.Count + 1));
                        strSortOrder = dgHistory.Columns[e.ColumnIndex].Name + " " + "DESC";

                        ApplySortingOnDatagridview(strSortOrder, dtSortView);
                    }
                    else if (!inCollection && sortCount == 1)
                    {
                        m_dataGridViewColumnSelector.mSortedDataList.Add(new ccSortedData(dgHistory.Columns[e.ColumnIndex].Name, "ASC",
                            m_dataGridViewColumnSelector.mSortedDataList.Count));
                        strSortOrder = dgHistory.Columns[e.ColumnIndex].Name + " " + "ASC";

                        ApplySortingOnDatagridview(strSortOrder, dtSortView);
                    }

                }

            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Apply sort on datagridview column.
        /// </summary>
        /// <param name="strSortOrder"></param>
        /// <param name="dtSortView"></param>
        private void ApplySortingOnDatagridview(string strSortOrder, DataView dtSortView)
        {
            dtSortView.Sort = strSortOrder;
            this.dgHistory.DataSource = null;
            this.dgHistory.DataSource = typeof(DataTable);

            this.dgHistory.DataSource = dtSortView;
            dgHistory.Columns["Designer"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgHistory.EndEdit();
            this.dgHistory.Refresh();

            foreach (DataGridViewColumn dc in this.dgHistory.Columns)
            {
                if (dc.SortMode != DataGridViewColumnSortMode.NotSortable)
                {
                    dc.SortMode = DataGridViewColumnSortMode.Programmatic;
                }
            }
        }

        #endregion

        private void cmbNamedView_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
