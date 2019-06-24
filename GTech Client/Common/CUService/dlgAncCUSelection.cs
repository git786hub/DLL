using System.Collections.Generic;
using System;
using Intergraph.GTechnology.API;
using System.Windows.Forms;
using System.Data;

namespace GTechnology.Oncor.CustomAPI
{
	partial class dlgAncCUSelection
	{
		public dlgAncCUSelection()
		{
			InitializeComponent();
		}
		private IGTDataContext m_oDataContext = null;
		private CUService m_oCUSvc = null;
		private int m_iFNO;
        private string m_sFilterValue = null;
        private string m_sSelectedCU = null;
        private string m_sSelectedCUQty = null;
        private string m_sSelectedCUActivity = null;
        private string m_sSelectedCUType = null;
        private bool m_bSystemIsResizingColumns = false;
		private ADODB.Recordset m_oCURS = null;
        private string m_sSelectedAMCU =string.Empty;

        #region Public Methods
        public DataTable CUGridBindingWithFilter { get; set; }
        public List<string> AncillaryCategories { get; set; }
        public string CUTypeSelected { get; set; }

        public void OK_Button_Click(System.Object sender, System.EventArgs e)
		{
			DataGridViewRow oRow = null;
            try
            {
                if (0 == grdSelected.Rows.Count)
                {
                    MessageBox.Show("Please add at least one CU/MCU before selecting OK");
                    return;
                }

                foreach (DataGridViewRow tempLoopVar_oRow in grdSelected.Rows)
                {
                    oRow = tempLoopVar_oRow;
                    if(m_sSelectedCUActivity == null)
                    {
                        m_sSelectedCUActivity = oRow.Cells[0].Value.ToString();
                    }
                    else
                    {
                        m_sSelectedCUActivity = m_sSelectedCUActivity + "," + oRow.Cells[0].Value.ToString();
                    }
                    if (m_sSelectedCUQty == null)
                    {
                        m_sSelectedCUQty = oRow.Cells[1].Value.ToString();
                        m_sSelectedCU = oRow.Cells[3].Value.ToString();
                        m_sSelectedCUType = oRow.Cells[2].Value.ToString();

                        if (oRow.Cells[4].Value.GetType()!=typeof(DBNull))
                        {
                            m_sSelectedAMCU = oRow.Cells[4].Value.ToString();
                        }
                        else
                        {
                            m_sSelectedAMCU = "*";
                        }

                    }
                    else
                    {
                        m_sSelectedCUQty = m_sSelectedCUQty + "," + oRow.Cells[1].Value.ToString();
                        m_sSelectedCU = m_sSelectedCU + "," + oRow.Cells[3].Value.ToString();
                        m_sSelectedCUType = m_sSelectedCUType + "," + oRow.Cells[2].Value.ToString();

                        if (oRow.Cells[4].Value.GetType() != typeof(DBNull))
                        {
                            m_sSelectedAMCU = m_sSelectedAMCU + "," + oRow.Cells[4].Value.ToString();
                        }
                        else
                        {
                            m_sSelectedAMCU = m_sSelectedAMCU + "," + "*";
                        }
                    }
                }
                
                CUTypeSelected = m_sSelectedCUType;

                this.DialogResult = System.Windows.Forms.DialogResult.OK;               
                this.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }						
		}

        public void dlgAncCUSelection_Load(object sender, System.EventArgs e)
        {
            try
            {
                InitializeDataSource();

                //Set the form's caption to include the file version.
                object[] sAssemblyInfo = System.Reflection.Assembly.GetExecutingAssembly().FullName.Split(',');
                System.String sAssemblyName = (string)(sAssemblyInfo[0]);
                string sAssemblyVer = sAssemblyInfo[1].ToString().ToLower().Replace("version=", "v.").Trim();

                this.Text = "Ancillary CU Selection for " + System.Convert.ToString(this.GetFeatureNameByFNO()) + " " + System.Convert.ToString(sAssemblyVer);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void dlgSelection_Shown(object sender, System.EventArgs e)
        {
            this.TopMost = true;
            this.TopMost = false;
            this.OK_Button.Enabled = false;
            this.btnRemove.Enabled = false;
        }

        #endregion

        #region Properties
        internal CUService CUService
		{
			set
			{
				m_oCUSvc = value;
			}
		}
        internal IGTDataContext DataContext
        {
            set
            {
                m_oDataContext = value;
            }
        }
        public int FNO
		{
			set
			{
				m_iFNO = value;
			}
		}
        internal string CU
		{
			get
			{
				return m_sSelectedCU;
			}
		}
        internal string Qty
		{
			get
			{
				return m_sSelectedCUQty;
			}
		}

        internal string Activity
        {
            get
            {
                return m_sSelectedCUActivity;
            }
        }

        internal ADODB.Recordset CURecordset
        {
            set
            {
                m_oCURS = value;
            }
        }
        internal string AMCU
        {
            get
            {
                return m_sSelectedAMCU;
            }
        }
        public Dictionary<string, List<PickiListData>> ColumnPickListData { get; set; }
        #endregion

        #region Private Methods
        private DataTable PopulateSelectedGrid(DataTable p_CUData)
        {
            DataTable dtSelectedCuData;
            dtSelectedCuData = p_CUData;
            if (!dtSelectedCuData.Columns.Contains("Quantity"))
            {
                dtSelectedCuData.Columns.Add("Quantity", typeof(Int32));               
            }
            if (!dtSelectedCuData.Columns.Contains("MCU"))
            {
                dtSelectedCuData.Columns.Add("MCU", typeof(string));
            }

            dtSelectedCuData.Columns["Quantity"].SetOrdinal(0);
            dtSelectedCuData.Columns["M/C"].SetOrdinal(1);
            dtSelectedCuData.Columns["CU"].SetOrdinal(2);
            dtSelectedCuData.Columns["MCU"].SetOrdinal(3);
            dtSelectedCuData.Columns["description"].SetOrdinal(4);
           
            return dtSelectedCuData;
        }

        private void grdCU_ColumnWidthChanged(object sender, System.Windows.Forms.DataGridViewColumnEventArgs e)
        {
            if (!m_bSystemIsResizingColumns)
            {
                ResizeFilterTextBoxes(grdCU, grdFilter, grdSelected);
            }
        }
        private void grdFilter_ColumnWidthChanged(object sender, System.Windows.Forms.DataGridViewColumnEventArgs e)
        {
            if (!m_bSystemIsResizingColumns)
            {
                ResizeFilterTextBoxes(grdFilter, grdCU, grdSelected);
            }
        }

        private void grdSelected_CellBeginEdit(object sender, System.Windows.Forms.DataGridViewCellCancelEventArgs e)
        {
            DataGridView oGrd = (DataGridView)sender;
            //Cache the current cell value.
            //If the user enters an invalid value, this cached value will be stored back into the cell.
            oGrd.Tag = oGrd.CurrentCell.Value;
        }

        DataGridViewSelectedRowCollection m_SelectedGridIndex = null;
        private void grdSelected_CellEndEdit(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            m_SelectedGridIndex = grdSelected.SelectedRows;
                ;
            //Validate the value for Qty.

            DataGridView oGrd = null;
            DataGridViewRow oRow = null;
            string sVal = null;
            int iVal = 0;

            oGrd = (System.Windows.Forms.DataGridView)sender;
            //Only interested in validating the Qty column--ALM-1042
            if (oGrd.CurrentCell != null)
            {
                if (1 == oGrd.CurrentCell.ColumnIndex)
                {

                    sVal = oGrd.CurrentCell.Value.ToString();
                    if (!Information.IsNumeric(sVal))
                    {
                        goto RetErrExit;
                    }

                    //Eliminate and decimals or commas (these pass IsNumeric() but we don't want them here).
                    if (-1 < sVal.IndexOf(".") || -1 < sVal.IndexOf(","))
                    {
                        goto RetErrExit;
                    }

                    iVal = System.Convert.ToInt32(sVal);
                    //AMCUs must have quantity set to 1.
                    oRow = oGrd.Rows[oGrd.CurrentCell.RowIndex];
                    if (oRow.Cells[2].Value.ToString().ToUpper() == "AMCU" && 2 != iVal)
                    {
                        goto RetErrExit1;
                    }

                    ////Only want integers between 1 and 999 inclusive.
                    //if (1 > iVal | 999 < iVal)
                    //{
                    //    goto RetErrExit;
                    //}

                }
            }

            WrapUp:
            return;

            RetErrExit:
            MessageBox.Show("Quantity must be an integer between 1 and 999 inclusive.", "G/Technology", MessageBoxButtons.OK);
            //Interaction.MsgBox("Quantity must be an integer between 1 and 999 inclusive.", MsgBoxStyle.OkOnly, "G/Technology"); //shubham
            oGrd.CurrentCell.Value = oGrd.Tag;
            goto WrapUp;

            RetErrExit1:
            MessageBox.Show("Quantity must be 1 for a Macro CU.", "G/Technology", MessageBoxButtons.OK);
            oGrd.CurrentCell.Value = oGrd.Tag;
            goto WrapUp;
        }

        private void btnAdd_Click(object sender, System.EventArgs e)
        {
            AddCU();
        }

        private void btnRemove_Click(object sender, System.EventArgs e)
        {
            RemoveCU();
        }
        private void grdCU_DoubleClick(object sender, System.EventArgs e)
        {
            AddCU();
            this.OK_Button.Enabled = 0 < grdSelected.Rows.Count;
        }

        private void grdSelected_DoubleClick(object sender, System.EventArgs e)
        {
            RemoveCU();
            this.OK_Button.Enabled = 0 < grdSelected.Rows.Count;
        }
        private void Cancel_Button_Click(System.Object sender, System.EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
        private void MakeQuantityEditable()
        {
            grdSelected.Columns["Quantity"].ReadOnly = false;        
            grdSelected.Columns[1].ReadOnly = true;
            grdSelected.Columns[2].ReadOnly = true;
            grdSelected.Columns[3].ReadOnly = true;            

        }
        public List<CUDataModel> CUGridBindingWholeData { get; set; }
        DataTable m_selectedCUs = null;
        private void InitializeDataSource()
        {
            m_bSystemIsResizingColumns = true;

            try
            {
                grdCU.DataSource = CUGridBindingWholeData[0].CUDataWithStandardAttributes;              
                CurrentSelectedCU = CUGridBindingWholeData[0].CUDataWithStandardAttributes;

                CUGridBindingWithFilter = CUGridBindingWholeData[0].CUDataWithStandardAttributes.Copy();
                grdCU.DataSource = CUGridBindingWithFilter;

                DataTable dtCuColumnGrid = CUGridBindingWholeData[0].CUDataWithStandardAttributes.Clone();
                grdCU.ColumnHeadersVisible = false;

                dtCuColumnGrid.Rows.Clear();
                dtCuColumnGrid.Rows.Add();             

                PopulateFilterGrid(dtCuColumnGrid);

                CUTypeSelected = m_sFilterValue;
                grdFilter.Rows[0].HeaderCell.Value = "Filter";

                m_selectedCUs = CUGridBindingWholeData[0].CUDataWithStandardAttributes.Clone();
                m_selectedCUs.Rows.Clear();

                grdSelected.DataSource = PopulateSelectedGrid(m_selectedCUs);

                MakeQuantityEditable();

                m_bSystemIsResizingColumns = false;
                if (AncillaryCategories != null && AncillaryCategories.Count > 0)
                {
                    cmbAncillaryCategories.DataSource = AncillaryCategories;
                }
                else
                {
                    cmbAncillaryCategories.Enabled = false;
                }

                grdFilter.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                grdFilter.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

                grdCU.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                grdCU.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

                //ALM-1042

                DataTable dataTableActivity = new DataTable();
                dataTableActivity.Columns.Add("Key");
                dataTableActivity.Columns.Add("Value");
                dataTableActivity.Rows.Add("I", "Install");
                dataTableActivity.Rows.Add("R", "Remove");
                dataTableActivity.Rows.Add("T", "Transfer");

                DataGridViewComboBoxColumn comboboxActivity = new DataGridViewComboBoxColumn();
                comboboxActivity.HeaderText = "Activity";
                comboboxActivity.DropDownWidth = 100;
                comboboxActivity.Width = 50;
                comboboxActivity.MaxDropDownItems = 3;
                comboboxActivity.FlatStyle = FlatStyle.Flat;
                comboboxActivity.DataSource = dataTableActivity;
                comboboxActivity.DisplayMember = "Value";
                comboboxActivity.ValueMember = "Key";
                grdSelected.Columns.Insert(0, comboboxActivity);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }           
        }
      
		private void ResizeFilterTextBoxes(DataGridView grdSource, DataGridView grdTarget1, DataGridView grdTarget2)
		{
            //Keep the width of two datagrid columns same.
            
            if (grdSource.Columns.Count > 0 && grdTarget1.Columns.Count > 0)
            {
                int iWidth = 0;

                m_bSystemIsResizingColumns = true;

                for (int i = 0; i < grdSource.Columns.Count; i++)
                {
                    iWidth = grdSource.Columns[i].Width;
                    grdTarget1.Columns[i].Width = iWidth;
                    // grdTarget2.Columns[i].Width = iWidth;
                }

                m_bSystemIsResizingColumns = false;
            }
			
		}

        DataTable dt1 = null;
        private void AddCU()
		{
            try
            {
                string[] strActivityCollection = CollectActivity(grdSelected);
                DataTable dt = ((DataTable)grdSelected.DataSource); 
                foreach (DataGridViewRow row in grdCU.SelectedRows)
                {
                    dt.ImportRow(((DataTable)grdCU.DataSource).Rows[row.Index]);
                }
                dt.AcceptChanges();

                grdSelected.DataSource = dt;
                //ALM-1042 -Increased all the row indexes
                this.OK_Button.Enabled = 0 < grdSelected.Rows.Count;
                this.btnRemove.Enabled = 0 < grdSelected.SelectedRows.Count;
                foreach (DataGridViewRow row in grdSelected.Rows)
                {
                    if (string.IsNullOrEmpty(Convert.ToString(row.Cells[1].Value)))
                    {
                        row.Cells[1].Value = 1;
                    }
                    if (!Convert.ToString(row.Cells[2].Value).Equals("AMCU"))
                    {
                        if (IsPropertyUnit(Convert.ToString(row.Cells[3].Value)))
                        {
                            row.Cells[1].ReadOnly = true;
                        }
                    }
                    else
                    {
                        row.Cells[4].Value = Convert.ToString(row.Cells[3].Value);
                    }
                }
                dt1 = dt;

                List<int> macroCUCollection = new List<int>();

                foreach (DataGridViewRow row in grdSelected.Rows)
                {                   
                    if (Convert.ToString(row.Cells[2].Value).Equals("AMCU"))
                    {
                        macroCUCollection.Add(row.Index);
                        ExpandMacroWithinForm(row.Cells[4].Value.ToString());
                    }                    
                }

                grdSelected.DataSource = dt1;

                foreach (int item in macroCUCollection)
                {
                    grdSelected.Rows.RemoveAt(item);
                }



                for (int i = 0; i < grdSelected.Rows.Count; i++)
                {
                    try
                    {
                        grdSelected.Rows[i].Cells[0].Value = strActivityCollection[i];
                    }
                    catch
                    {
                        grdSelected.Rows[i].Cells[0].Value = "I";
                    }
                }
                
              
                

                
                grdSelected.ClearSelection();
            }
            catch (Exception ex)
            {
                throw ex;
            }		         
        }

       

        private string[] CollectActivity(DataGridView dataGridView)
        {
            string[] strActivityCollection = null;

            if (dataGridView.Rows.Count > 0)
            {
                strActivityCollection = new string[dataGridView.Rows.Count];
                for (int i = 0; i < dataGridView.Rows.Count; i++)
                {
                    strActivityCollection[i] = Convert.ToString(dataGridView.Rows[i].Cells[0].Value);
                }
                
            }
  
            return strActivityCollection;
        }
        private void ExpandMacroWithinForm(string p_MUID)
        {
            ADODB.Recordset RS = m_oDataContext.OpenRecordset("select culib_macrounit.cu_id, mu_id, cu_qty, cu_desc from culib_macrounit, culib_unit where mu_id = ? and culib_macrounit.cu_id = culib_unit. cu_id", ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic, (int)ADODB.CommandTypeEnum.adCmdText, p_MUID);
           // DataTable dt = ((DataTable)grdSelected.DataSource);

            if (RS!=null)
            {
                if (RS.RecordCount>0)
                {
                    RS.MoveFirst();
                    while(RS.EOF == false)
                    {                                             
                       dt1.Rows.Add(RS.Fields["cu_qty"].Value, "ACU", RS.Fields["cu_id"].Value, RS.Fields["mu_id"].Value, RS.Fields["cu_desc"].Value);                        
                        RS.MoveNext();
                    }
                }

                dt1.AcceptChanges();           
            }
        }

        private void RemoveCU()
		{
            try
            {
                if (0 < grdSelected.Rows.Count)
                {
                    foreach (DataGridViewRow item in grdSelected.SelectedRows)
                    {
                        grdSelected.Rows.Remove(item);
                    }                   
                }

                this.OK_Button.Enabled = 0 < grdSelected.Rows.Count;
                this.btnRemove.Enabled = 0 < grdSelected.SelectedRows.Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }					
		}
		private string GetFeatureNameByFNO()
		{
			ADODB.Recordset oRS = null;
			System.String sSQL = string.Empty;
			sSQL = "select g3e_username from g3e_features_optlang where g3e_fno=?";
			oRS = m_oDataContext.OpenRecordset(sSQL, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, (System.Int32) ADODB.CommandTypeEnum.adCmdText, m_iFNO);
			return oRS.Fields["g3e_username"].Value.ToString();
		}


        private void PopulateFilterGrid(DataTable dtCuColumnGrid)
        {
            try
            {
                grdFilter.Rows.Clear();
                grdFilter.Columns.Clear();

                for (int i = 0; i < dtCuColumnGrid.Columns.Count; i++)
                {
                    if (ColumnPickListData.ContainsKey(dtCuColumnGrid.Columns[i].ColumnName))
                    {
                        var column = new DataGridViewComboBoxColumn();
                        List<PickiListData> asd;
                        ColumnPickListData.TryGetValue(dtCuColumnGrid.Columns[i].ColumnName, out asd);

                        asd.RemoveAll(p => p.KeyField.Equals(""));
                        asd.Add(new PickiListData("", ""));
                        column.DataSource = asd;
                        column.DataPropertyName = "ValueField";
                        column.DisplayMember = "ValueField";
                        column.Name = dtCuColumnGrid.Columns[i].ColumnName;
                        grdFilter.Columns.Add(column);
                    }
                    else
                    {
                        grdFilter.Columns.Add(dtCuColumnGrid.Columns[i].ColumnName, dtCuColumnGrid.Columns[i].ColumnName);
                    }
                }
                grdFilter.Rows.Add();

                grdFilter.Rows[0].HeaderCell.Value = "Filter";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        DataTable CurrentSelectedCU = new DataTable();
        private void cmbAncillaryCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (CUGridBindingWholeData.Find(p => p.Category == cmbAncillaryCategories.Text) != null && CUGridBindingWholeData.Find(p => p.Category == cmbAncillaryCategories.Text).Category != null)
                {
                    CurrentSelectedCU = CUGridBindingWholeData.Find(p => p.Category == cmbAncillaryCategories.Text).CUDataWithStandardAttributes;
                    CUGridBindingWithFilter = CurrentSelectedCU;
                    grdCU.DataSource = CurrentSelectedCU;

                    DataTable dtCuColumnGrid = CurrentSelectedCU.Clone();                
                    dtCuColumnGrid.Rows.Clear();
                    dtCuColumnGrid.Rows.Add();
                    PopulateFilterGrid(dtCuColumnGrid);
                   // grdFilter.DataSource = dtCuColumnGrid;
                }
                else
                {
                    grdCU.DataSource = null;
                    CurrentSelectedCU = null;
                }
                ResizeFilterTextBoxes(grdCU, grdFilter,grdSelected);
                grdFilter.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                grdFilter.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

                grdCU.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                grdCU.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                EnableDisableHandling();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void grdFilter_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            //TextBox cColumnControl = (TextBox)e.Control;
            //cColumnControl.TextChanged -= CColumnControl_TextChanged;
            //cColumnControl.TextChanged += CColumnControl_TextChanged;

            if (e.Control.GetType() == typeof(DataGridViewComboBoxEditingControl))
            {
                ComboBox cColumnControl = (ComboBox)e.Control;
                cColumnControl.SelectedIndexChanged -= CColumnControl_SelectedIndexChanged;
                cColumnControl.SelectedIndexChanged += CColumnControl_SelectedIndexChanged;
            }
            else
            {
                TextBox cColumnControl = (TextBox)e.Control;
                cColumnControl.TextChanged -= CColumnControl_TextChanged;
                cColumnControl.TextChanged += CColumnControl_TextChanged;
            }
        }

        private void CColumnControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProcessFiltering(sender,e);
            ResizeFilterTextBoxes(grdCU, grdFilter, grdSelected);
        }

        private string m_existingFilter = string.Empty;

        private void ProcessFiltering(object sender, EventArgs e)
        {
            try
            {
                if (sender.GetType() == typeof(DataGridViewComboBoxEditingControl))
                {
                    if (((DataGridViewComboBoxEditingControl)(sender)).Text == "GTechnology.Oncor.CustomAPI.PickiListData")
                    {
                        return;
                    }
                }
                    string filter = string.Empty;
                    for (int i = 0; i < grdFilter.Columns.Count; i++)
                    {
                        if (sender.GetType() == typeof(DataGridViewComboBoxEditingControl))
                        {
                            if (i !=
                            ((DataGridViewComboBoxEditingControl)(sender)).EditingControlDataGridView.CurrentCell.ColumnIndex)
                            {
                                if (grdFilter.Rows[0].Cells[i].Value != null && grdFilter.Rows[0].Cells[i].Value != DBNull.Value)
                                {
                                    filter = filter + " and [" + grdFilter.Columns[i].Name.ToString() + "] like '%" +
                                                 grdFilter.Rows[0].Cells[i].Value.ToString() + "%'";

                                }
                            }
                        }

                        else if (i !=
                            ((DataGridViewTextBoxEditingControl)(sender)).EditingControlDataGridView.CurrentCell.ColumnIndex)
                        {
                            if (grdFilter.Rows[0].Cells[i].Value != null && grdFilter.Rows[0].Cells[i].Value != DBNull.Value)
                            {
                                filter = filter + " and [" + grdFilter.Columns[i].Name.ToString() + "] like '%" +
                                         grdFilter.Rows[0].Cells[i].Value.ToString() + "%'";
                            }
                            ;
                        }
                    }

                    if (sender.GetType() == typeof(DataGridViewComboBoxEditingControl))
                    {
                        if (((DataGridViewComboBoxEditingControl)(sender)).Text != "")
                            filter = filter + " and [" +
                                    ((DataGridViewComboBoxEditingControl)(sender)).EditingControlDataGridView.CurrentCell
                                    .OwningColumn.Name + "] like '%" + ((DataGridViewComboBoxEditingControl)(sender)).Text + "%'";
                    }
                    else
                    {
                        filter = filter + " and [" +
                          ((DataGridViewTextBoxEditingControl)(sender)).EditingControlDataGridView.CurrentCell.OwningColumn.Name + "] like '%" + ((DataGridViewTextBoxEditingControl)(sender)).Text + "%'";
                    }
                    m_existingFilter = filter;
                    HandleCUDataFilters();               
            }
            catch (Exception ex)
            {
                throw ex;
            }         
        }
        private void ProcessFiltering1(object sender)
        {
            string filter = string.Empty;
            try
            {
                for (int i = 0; i < grdFilter.Columns.Count; i++)
                {

                    if (i !=
                        ((DataGridViewTextBoxEditingControl)(sender)).EditingControlDataGridView.CurrentCell.ColumnIndex)
                    {
                        if (grdFilter.Rows[0].Cells[i].Value != null && grdFilter.Rows[0].Cells[i].Value != DBNull.Value)
                        {
                            filter = filter + " and [" + grdFilter.Columns[i].Name.ToString() + "] like '%" +
                                     grdFilter.Rows[0].Cells[i].Value.ToString() + "%'";
                        }
                        ;
                    }

                }
                filter = filter + " and [" +
                            ((DataGridViewTextBoxEditingControl)(sender)).EditingControlDataGridView.CurrentCell
                            .OwningColumn.Name + "] like '%" + ((DataGridViewTextBoxEditingControl)(sender)).Text + "%'";

                if (filter.StartsWith(" and"))
                {
                    filter = filter.Remove(0, 4);
                }

                DataTable test = new DataTable();

                if (CurrentSelectedCU.Select(filter).Length > 0)
                {
                    m_existingFilter = filter;

                    if (!string.IsNullOrEmpty(m_PreferredCUFilter))
                    {
                        filter = filter + " and CU in (" + m_PreferredCUFilter + ")";
                    }
                    CUGridBindingWithFilter = CurrentSelectedCU.Select(filter).CopyToDataTable();
                }
                else
                {
                    CUGridBindingWithFilter = null;
                }
                grdCU.DataSource = CUGridBindingWithFilter;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void CColumnControl_TextChanged(object sender, EventArgs e)
        {
            ProcessFiltering(sender,e);
        }

        private bool IsPropertyUnit(string p_CU)
        {
            bool bReturn = false;
           ADODB.Recordset rs =  m_oDataContext.OpenRecordset("select RETIREMENT_NBR from CULIB_UNIT  where CU_ID =?", ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly, (int)ADODB.CommandTypeEnum.adCmdText, p_CU);
            rs.MoveFirst();
            if (Convert.ToString(rs.Fields[0].Value).Equals("1")) bReturn = true;
            return bReturn;
        }
        private void grdSelected_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            grdSelected.ClearSelection();
        }

        private void btnMakePreferred_Click(object sender, EventArgs e)
        {
            try
            {
                IGTApplication oApp = GTClassFactory.Create<IGTApplication>();
                CuCommonForms obj = new CuCommonForms(oApp);

                obj.MakePreferredCU(Convert.ToString(grdCU.SelectedRows[0].Cells[1].Value), Convert.ToString(cmbAncillaryCategories.SelectedValue), oApp.DataContext.DatabaseUserName);
            }
            catch (Exception ex)
            {
                throw ex;
            }          
        }

        string m_PreferredCUFilter = string.Empty;
      

        private void chkShowPreferredCU_CheckedChanged(object sender, EventArgs e)
        {

            try
            {
                string filter = string.Empty;
                if (chkShowPreferredCU.Checked == true)
                {
                    IGTApplication oApp = GTClassFactory.Create<IGTApplication>();
                    CuCommonForms obj = new CuCommonForms(oApp);
                    ADODB.Recordset rs = obj.GetPreferredCU(cmbAncillaryCategories.SelectedValue.ToString());
                    m_PreferredCUFilter = obj.GetPreferredCuFilterString(rs);

                    if (string.IsNullOrEmpty(m_PreferredCUFilter))
                    {
                        m_PreferredCUFilter = "'***NOTDefined***'";
                    }
                    HandleCUDataFilters();
                }
                else
                {
                    m_PreferredCUFilter = null;
                    HandleCUDataFilters();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void grdSelected_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Enter a numeric value");
        }

        private void dlgAncCUSelection_ResizeBegin(object sender, EventArgs e)
        {
            grdCU.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grdCU.ColumnWidthChanged -= grdCU_ColumnWidthChanged;
            grdFilter.ColumnWidthChanged -= grdFilter_ColumnWidthChanged;
        }
        
        private void dlgAncCUSelection_ResizeEnd(object sender, EventArgs e)
        {
            try
            {
                grdCU.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                grdCU.ColumnWidthChanged += grdCU_ColumnWidthChanged;
                grdFilter.ColumnWidthChanged += grdFilter_ColumnWidthChanged;
                ResizeFilterTextBoxes(grdCU, grdFilter, grdSelected);
            }
            catch (Exception ex)
            {
                throw ex;
            }           
        }

        private void btnMoreInfo_Click(object sender, EventArgs e)
        {
            try
            {
                IGTApplication oApp = GTClassFactory.Create<IGTApplication>();
                CuCommonForms obj = new CuCommonForms(oApp);
                obj.ShowMoreInfoForm(Convert.ToString(grdCU.SelectedRows[0].Cells["CU"].Value), false, ((Convert.ToString(grdCU.SelectedRows[0].Cells["M/C"].Value) == "CU" || (Convert.ToString(grdCU.SelectedRows[0].Cells["M/C"].Value) == "ACU")) ? true : false));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void grdSelected_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (grdSelected.SelectedRows.Count>0)
                {                   
                    btnRemove.Enabled = true;
                }
                else
                {
                    if (m_SelectedGridIndex!=null)
                    {
                        this.btnRemove.Enabled = 0 < m_SelectedGridIndex.Count;
                    }
                    else
                    {
                        btnRemove.Enabled = false;
                    }                       
                }
            }
            catch (Exception EX)
            {
                throw EX;
            }
        }
        private void grdSelected_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
           // grdSelected.ClearSelection();
        }
        private void EnableDisableHandling()
        {
            this.btnMoreInfo.Enabled = 0 < grdCU.SelectedRows.Count;
            this.btnMakePreferred.Enabled = 0 < grdCU.SelectedRows.Count;
            this.btnAdd.Enabled = 0 < grdCU.SelectedRows.Count;
        }
        private void grdCU_Click(object sender, EventArgs e)
        {
            EnableDisableHandling();
        }

        private void HandleCUDataFilters()
        {
            string filter = string.Empty;
            if (!string.IsNullOrEmpty(m_existingFilter) && string.IsNullOrEmpty(m_PreferredCUFilter))
            {
                //case 1 - Grid filter exists and Preferred CU filter does not exists
                filter = m_existingFilter;
            }
            else if (!string.IsNullOrEmpty(m_existingFilter) && !string.IsNullOrEmpty(m_PreferredCUFilter))
            {
                //case 2 - Grid filter existis and Preferrred CU also exists
                filter = m_existingFilter + " and CU in (" + m_PreferredCUFilter + ")";
            }

            else if (string.IsNullOrEmpty(m_existingFilter) && !string.IsNullOrEmpty(m_PreferredCUFilter))
            {
                //case 3 - Grid filter does not exists and Preferred CU exists
                filter = "CU in (" + m_PreferredCUFilter + ")";
            }
            else


            // case 4 - Grid filter does not exists and Preferreed CU does not exists
            filter = null;


            if (!string.IsNullOrEmpty(filter))
            {
                if (filter.StartsWith(" and"))
                {
                    filter = filter.Remove(0, 4);
                }

                DataTable test = new DataTable();
               // m_existingFilter = filter;

                if (CurrentSelectedCU != null)
                {
                    if (CurrentSelectedCU.Select(filter).Length > 0)
                    {
                        CUGridBindingWithFilter = CurrentSelectedCU.Select(filter).CopyToDataTable();                     
                    }
                    else
                    {
                        CUGridBindingWithFilter = null;
                    }

                    grdCU.DataSource = CUGridBindingWithFilter;
                }
            }
            else
            {
                CUGridBindingWithFilter = CurrentSelectedCU;
                grdCU.DataSource = CUGridBindingWithFilter;
            }

            EnableDisableHandling();
            grdCU.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grdCU.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            ResizeFilterTextBoxes(grdCU, grdFilter, grdSelected);
        }

        #endregion

    }
}
