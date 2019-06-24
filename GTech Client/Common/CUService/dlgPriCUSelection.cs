
using System.Collections.Generic;
using System;
using Intergraph.GTechnology.API;
using System.Data;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
	partial class dlgPriCUSelection
	{
		public dlgPriCUSelection()
		{
			InitializeComponent();
		}
		private IGTDataContext m_oDataContext = null;
		private CUService m_oCUSvc = null;
		private int m_iFNO;
		private String m_sFilterField = string.Empty;
		private String m_sFilterValue = string.Empty;
		private String m_sSelectedCU = string.Empty;
		private int m_iQty = 1;
		private ADODB.Recordset m_oCURS = null;		
		private const string M_SCU = "CU";
		private const string M_SMCU = "MCU";
		private const string M_SACU = "ACU";
		private const string M_SAMCU = "AMCU";
        private string m_PreferredCUFilter = null;
        private string m_sExistingCUGridFilter = string.Empty;
        private string m_cCurrentCategorySelected;
        private DataTable CUCurrentData = new DataTable();
        private string txtQty = "1";
        private bool m_raisedFromEvent = false;
        bool m_bRaisedFromAdjust = false;
        //private bool m_bSystemIsResizingColumns = false;

        #region Properties
        public List<CUDataModel> CUGridBindingWholeData { get; set; }
        public DataTable CUGridBindingWithFilter { get; set; }
        public string CUTypeSelected { get; set; }
        public Dictionary<string,List<PickiListData>> ColumnPickListData { get; set; }
        public DataRow m_selectedCUDataRecord { get; set; }
        public List<PickiListData> CategoryList { get; set; }

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
        internal int Qty
        {
            get
            {
                int returnValue = default(int);
                returnValue = m_iQty;
                return returnValue;
            }
        }
        internal int FNO
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
                string returnValue = default(string);
                returnValue = m_sSelectedCU;
                return returnValue;
            }
        }
        internal ADODB.Recordset CURecordset
        {
            set
            {
                m_oCURS = value;
            }
        }
        #endregion

        #region Public Methods
        public void OK_Button_Click(System.Object sender, System.EventArgs e)
        {
            OkayExit();
        }

        public void Cancel_Button_Click(System.Object sender, System.EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
        public void dlgPriCUSelection_Load(object sender, System.EventArgs e)
        {
            InitializeDataSources();

            //Set the form's caption to include the file version.
            object[] sAssemblyInfo = System.Reflection.Assembly.GetExecutingAssembly().FullName.Split(',');
            System.String sAssemblyName = (string)(sAssemblyInfo[0]);
            string sAssemblyVer = sAssemblyInfo[1].ToString().ToLower().Replace("version=", "v.").Trim();
            this.Text = "Primary CU Selection for " + System.Convert.ToString(this.GetFeatureNameByFNO()) + " " + System.Convert.ToString(sAssemblyVer);
        }
        public void grdCU_DoubleClick(object sender, System.EventArgs e)
        {
            OkayExit();
        }
        #endregion

        #region Private Methods
        private void OkayExit()
        {
            try
            {
                int iTmp = 0;
                bool bInvalidQty = false;

                if (0 == grdCU.SelectedRows.Count)
                {
                    MessageBox.Show("Please select a CU from the grid.");
                    return;
                }

                if (!System.String.IsNullOrEmpty(txtQty))
                {
                    try
                    {
                        iTmp = System.Convert.ToInt32(txtQty);
                        m_iQty = iTmp;
                    }
                    catch
                    {
                        bInvalidQty = true;
                    }
                }
                else
                {
                    bInvalidQty = true;
                }

                if (bInvalidQty)
                {
                    MessageBox.Show("Please enter a non-zero integer value for quantity.");
                    return;
                }

                m_sSelectedCU = grdCU.CurrentRow.Cells[2].Value.ToString();
                m_selectedCUDataRecord = (DataRow)((DataRowView)grdCU.Rows[grdCU.CurrentRow.Index].DataBoundItem).Row;
                CUTypeSelected = grdCU.CurrentRow.Cells[1].Value.ToString();
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void SetStyle(DataGridView p_dataGrid)
        {
            foreach (var item in p_dataGrid.Columns)
            {
                if (item.GetType() == typeof(DataGridViewComboBoxColumn))
                {
                    ((DataGridViewComboBoxColumn)(item)).FlatStyle = FlatStyle.Flat;
                }
            }            
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
                        if (dtCuColumnGrid.Columns[i].ColumnName == "Category")
                        {
                            var column = new DataGridViewComboBoxColumn();
                            column.DataSource = CategoryList;
                            column.DataPropertyName = "ValueField";
                            column.DisplayMember = "ValueField";
                            column.Name = dtCuColumnGrid.Columns[i].ColumnName;
                            grdFilter.Columns.Add(column);
                        }
                        else
                        {
                            var column = new DataGridViewComboBoxColumn();
                            List<PickiListData> asd;
                            ColumnPickListData.TryGetValue(dtCuColumnGrid.Columns[i].ColumnName, out asd);

                            if (!asd.Exists(p => p.KeyField == "" && p.ValueField ==""))
                            {
                                asd.Add(new PickiListData("", ""));
                            }
                            column.DataSource = asd;
                            column.DataPropertyName = "ValueField";
                            column.DisplayMember = "ValueField";
                            column.Name = dtCuColumnGrid.Columns[i].ColumnName;
                            grdFilter.Columns.Add(column);
                        }
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
       
        private void InitializeDataSources()
        {
            try
            {
                CUGridBindingWithFilter = CUGridBindingWholeData[0].CUDataWithStandardAttributes.Copy();
                grdCU.DataSource = CUGridBindingWithFilter;
                grdCU.Scroll += GrdCU_Scroll;
                CUCurrentData = (DataTable) grdCU.DataSource; //Set initial value
                DataTable dtCuColumnGrid = CUGridBindingWholeData[0].CUDataWithStandardAttributes.Clone();
                grdCU.ColumnHeadersVisible = false;
                dtCuColumnGrid.Rows.Clear();
                dtCuColumnGrid.Rows.Add();

                CUTypeSelected = m_sFilterValue;
                
                PopulateFilterGrid(dtCuColumnGrid);

                grdFilter.Rows[0].Cells[0].Value = CUGridBindingWholeData[0].Category;
                ResizeMainForm();
                m_raisedFromEvent = true;
                grdFilter.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                m_raisedFromEvent = false;
                grdFilter.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                ResizeFilterTextBoxes(grdFilter, grdCU);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ResizeMainForm()
        {
            try
            {
                int widthOfAllColumns = 0;
                grdFilter.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.ColumnHeader);
                for (int i = 0; i < grdFilter.Columns.GetColumnCount(DataGridViewElementStates.None); i++)
                {
                    widthOfAllColumns = widthOfAllColumns + grdFilter.Columns[i].Width;
                }

                if (widthOfAllColumns + 100 > this.Width)
                {
                    this.Width = widthOfAllColumns + 100;
                    this.grdCU.Width = widthOfAllColumns + 100;
                    this.Panel3.Width = widthOfAllColumns + 100;
                }
                ResizeFilterTextBoxes(grdFilter, grdCU);
                try
                {
                    grdFilter.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.None);
                }
                catch (Exception)
                {

                }
                SetStyle(grdFilter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void GrdCU_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation== ScrollOrientation.HorizontalScroll)
            {
                grdFilter.HorizontalScrollingOffset = e.NewValue;
            }
        }

        private void grdCU_ColumnWidthChanged(object sender, System.Windows.Forms.DataGridViewColumnEventArgs e)
		{
            if (!m_raisedFromEvent)
            {
                ResizeFilterTextBoxes(grdCU, grdFilter);
            }
        }
        private void grdFilter_ColumnWidthChanged(object sender, System.Windows.Forms.DataGridViewColumnEventArgs e)
		{

            if (!m_raisedFromEvent)
            {
                ResizeFilterTextBoxes(grdFilter, grdCU);
            }
		}
		private void ResizeFilterTextBoxes(DataGridView grdSource, DataGridView grdTarget)
		{
            
			int iWidth = 0;
            if (grdTarget.Columns.GetColumnCount(DataGridViewElementStates.Visible) > 0)
            {
                m_raisedFromEvent = true;
                for (int i = 0; i < grdSource.Columns.Count; i++)
                {
                    iWidth = grdSource.Columns[i].Width;
                    grdTarget.Columns[i].Width = iWidth;
                }
                m_raisedFromEvent = false;
            }
        }		
		private string GetFeatureNameByFNO()
		{
			ADODB.Recordset oRS = null;
			System.String sSQL = string.Empty;
			sSQL = "select g3e_username from G3E_FEATURES_OPTABLE where g3e_fno=?";
			oRS = m_oDataContext.OpenRecordset(sSQL, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, (System.Int32) ADODB.CommandTypeEnum.adCmdText, m_iFNO);
			return oRS.Fields["g3e_username"].Value.ToString();
		}
        private void grdFilter_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
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
            try
            {
                if (m_cCurrentCategorySelected == null)
                {
                    m_cCurrentCategorySelected = Convert.ToString(grdFilter.Rows[0].Cells[0].Value);
                }
                if (m_bRaisedFromAdjust == true) return;

                ProcessFiltering(sender, e); 
            }
            catch (Exception ex)
            {
                throw ex;
            }   
        }
        private void EnableDisableHandling()
        {
            btnMoreInfor.Enabled = 0 < grdCU.SelectedRows.Count;
            btnMakePreferred.Enabled = 0 < grdCU.SelectedRows.Count;
            OK_Button.Enabled = 0 < grdCU.SelectedRows.Count;
        }

        private void HandleCUDataFilters()
        {
            string filter = string.Empty;
            if (!string.IsNullOrEmpty(m_sExistingCUGridFilter) && string.IsNullOrEmpty(m_PreferredCUFilter))
            {
                //case 1 - Grid filter exists and Preferred CU filter does not exists
                filter = m_sExistingCUGridFilter;
            }
            else if (!string.IsNullOrEmpty(m_sExistingCUGridFilter) && !string.IsNullOrEmpty(m_PreferredCUFilter))
            {
                //case 2 - Grid filter existis and Preferrred CU also exists
                filter = m_sExistingCUGridFilter + " and CU in (" + m_PreferredCUFilter + ")";
            }

            else if (string.IsNullOrEmpty(m_sExistingCUGridFilter) && !string.IsNullOrEmpty(m_PreferredCUFilter))
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

                if (CUCurrentData != null)
                {
                    if (CUCurrentData.Columns.Count == 0)
                    {
                        CUCurrentData = (DataTable)grdCU.DataSource;
                    }

                    if (CUCurrentData.Select(filter).Length > 0)
                    {
                        CUGridBindingWithFilter = CUCurrentData.Select(filter).CopyToDataTable();
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
                CUGridBindingWithFilter = CUCurrentData;
                grdCU.DataSource = CUGridBindingWithFilter;
            }

            m_raisedFromEvent = true;
            grdCU.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            m_raisedFromEvent = false;
            grdCU.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            ResizeFilterTextBoxes(grdCU, grdFilter);
            EnableDisableHandling(); ;
        }

        private void ProcessFiltering(object sender, EventArgs e)
        {
            int iSelectedCategoryData = 0;

            try
            {
                    if (((DataGridViewComboBoxEditingControl)(sender)).Text != "GTechnology.Oncor.CustomAPI.PickiListData")
                {
                    if (Convert.ToString(grdFilter.Rows[0].Cells[0].Value) == ((DataGridViewComboBoxEditingControl)(sender)).Text) return;
                    if (((DataGridViewComboBoxEditingControl)(sender)).EditingControlDataGridView.CurrentCell.ColumnIndex == 0)
                    {
                        for (int i = 0; i < CUGridBindingWholeData.Count; i++)
                        {
                            if (((DataGridViewComboBoxEditingControl)(sender)).Text != "" && ((DataGridViewComboBoxEditingControl)(sender)).Text != "GTechnology.Oncor.CustomAPI.PickiListData")
                            {
                                if (CUGridBindingWholeData[i].Category == ((DataGridViewComboBoxEditingControl)(sender)).Text)
                                {
                                    m_bRaisedFromAdjust = true;
                                    m_cCurrentCategorySelected = CUGridBindingWholeData[i].Category;
                                    iSelectedCategoryData = i;
                                    CUCurrentData = CUGridBindingWholeData[iSelectedCategoryData].CUDataWithStandardAttributes;
                                    grdCU.DataSource = null;
                                    grdCU.DataSource = CUCurrentData;
                                    PopulateFilterGrid(CUCurrentData);
                                    grdFilter.Rows[0].Cells[0].Value = m_cCurrentCategorySelected;
                                    ((DataGridViewComboBoxEditingControl)(sender)).Text = m_cCurrentCategorySelected;
                                    ResizeMainForm();
                                    m_bRaisedFromAdjust = false;
                                    ResizeFilterTextBoxes(grdFilter, grdCU);

                                    m_sExistingCUGridFilter = "[category] LIKE '" + m_cCurrentCategorySelected + "%'";          SetPreferredFilter();

                                    dlgPriCUSelection_ResizeBegin(null, null);
                                    dlgPriCUSelection_ResizeEnd(null, null);

                                    HandleCUDataFilters();
                                    return;
                                    //break;
                                }
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                }
                else 
                    {
                    return;
                }
            }
            catch (Exception)
            {

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
                            if (grdFilter.Columns[i].Name.ToString().Equals("M/C"))
                            {
                                filter = filter + " and [" + grdFilter.Columns[i].Name.ToString() + "] like '" +
                                         grdFilter.Rows[0].Cells[i].Value.ToString() + "%'";
                            }
                            else
                            {
                                if (grdFilter.Columns[i].GetType() == typeof(DataGridViewComboBoxCell))
                                {
                                    filter = filter + " and [" + grdFilter.Columns[i].Name.ToString() + "] = '" +
                                             grdFilter.Rows[0].Cells[i].Value.ToString() + "'";
                                }
                                else
                                {
                                    filter = filter + " and [" + grdFilter.Columns[i].Name.ToString() + "] like '%" +
                                             grdFilter.Rows[0].Cells[i].Value.ToString() + "%'";
                                }
                            }

                        }
                    }
                }

                else if (i !=
                    ((DataGridViewTextBoxEditingControl)(sender)).EditingControlDataGridView.CurrentCell.ColumnIndex)
                {
                    if (grdFilter.Rows[0].Cells[i].Value != null && grdFilter.Rows[0].Cells[i].Value != DBNull.Value)
                    {
                        if (grdFilter.Rows[0].Cells[i].GetType() == typeof(DataGridViewComboBoxCell))
                        {

                            filter = filter + " and [" + grdFilter.Columns[i].Name.ToString() + "] = '" +
                                     grdFilter.Rows[0].Cells[i].Value.ToString() + "'";
                        }
                        else
                        {
                            filter = filter + " and [" + grdFilter.Columns[i].Name.ToString() + "] like '%" +
                                     grdFilter.Rows[0].Cells[i].Value.ToString() + "%'";
                        }
                    }
                    ;
                }
            }

            if (sender.GetType() == typeof(DataGridViewComboBoxEditingControl))
            {
                if (((DataGridViewComboBoxEditingControl)(sender)).EditingControlDataGridView.CurrentCell
                            .OwningColumn.Name.Equals("M/C"))
                {
                    if (((DataGridViewComboBoxEditingControl)(sender)).Text != "")
                        filter = filter + " and [" +
                                ((DataGridViewComboBoxEditingControl)(sender)).EditingControlDataGridView.CurrentCell
                                .OwningColumn.Name + "] like '" + ((DataGridViewComboBoxEditingControl)(sender)).Text + "%'";
                }
                else
                {
                    if (((DataGridViewComboBoxEditingControl)(sender)).Text != "")
                        filter = filter + " and [" +
                                ((DataGridViewComboBoxEditingControl)(sender)).EditingControlDataGridView.CurrentCell
                                .OwningColumn.Name + "] = '" + ((DataGridViewComboBoxEditingControl)(sender)).Text + "'";
                }
               
            }
            else
            {
                filter = filter + " and [" +
                  ((DataGridViewTextBoxEditingControl)(sender)).EditingControlDataGridView.CurrentCell.OwningColumn.Name + "] like '%" + ((DataGridViewTextBoxEditingControl)(sender)).Text + "%'";
            }
            m_sExistingCUGridFilter = filter;
            HandleCUDataFilters();
            /*
            if (filter != "")
            {
                if (filter.StartsWith(" and"))
                {
                    filter = filter.Remove(0, 4);
                }

                DataTable test = new DataTable();

                if (filter != "" )
                {
                    m_sExistingCUGridFilter = filter;

                    if (!string.IsNullOrEmpty(m_PreferredCUFilter))
                    {
                        filter = filter + " and CU in (" + m_PreferredCUFilter + ")";
                    }
                    if (CUCurrentData.Columns.Count==0)
                    {
                        CUCurrentData = (DataTable)grdCU.DataSource;
                    }
                    if (CUCurrentData.Select(filter).Length > 0)
                    {
                        CUGridBindingWithFilter = CUCurrentData.Select(filter).CopyToDataTable();
                    }
                    else
                    {
                        CUGridBindingWithFilter = null;
                    }
                    grdCU.DataSource = CUGridBindingWithFilter;
                    ResizeFilterTextBoxes(grdFilter, grdCU);
                }
            }*/
        }
    
        private void CColumnControl_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ProcessFiltering(sender, e);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void grdCU_SelectionChanged(object sender, EventArgs e)
        {
            m_selectedCUDataRecord = (DataRow) ((DataRowView)grdCU.Rows[grdCU.CurrentRow.Index].DataBoundItem).Row;
            grdFilter.ClearSelection();
        }

        private void btnMoreInfo(object sender, EventArgs e)
        {
            try
            {
                IGTApplication oApp = GTClassFactory.Create<IGTApplication>();
                CuCommonForms obj = new CuCommonForms(oApp);
                obj.ShowMoreInfoForm(Convert.ToString(m_selectedCUDataRecord["CU"]), true, (Convert.ToString(m_selectedCUDataRecord["M/C"]) == "CU" ? true : false));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IGTApplication oApp = GTClassFactory.Create<IGTApplication>();
            CuCommonForms obj = new CuCommonForms(oApp);

            obj.MakePreferredCU(Convert.ToString(grdCU.SelectedRows[0].Cells[2].Value), Convert.ToString(grdCU.SelectedRows[0].Cells[0].Value), oApp.DataContext.DatabaseUserName);
        }
        private void ResetDataSource()
        {
            string filter = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(m_PreferredCUFilter))
                {
                    if (!string.IsNullOrEmpty(m_sExistingCUGridFilter))
                    {
                        filter = m_sExistingCUGridFilter + " and CU in (" + m_PreferredCUFilter + ")";
                    }
                    else
                    {
                        filter = "CU in (" + m_PreferredCUFilter + ")";
                    }
                }
                else
                {
                    filter = m_sExistingCUGridFilter;
                }
                if (CUCurrentData.Columns.Count == 0)
                {
                    CUCurrentData = (DataTable)grdCU.DataSource;
                }
                if (CUCurrentData.Select(filter).Length > 0)
                {
                    CUGridBindingWithFilter = CUCurrentData.Select(filter).CopyToDataTable();
                }
                else
                {
                    CUGridBindingWithFilter = null;
                }
                grdCU.DataSource = CUGridBindingWithFilter;
                ResizeFilterTextBoxes(grdFilter, grdCU);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                SetPreferredFilter();
                if (string.IsNullOrEmpty(m_PreferredCUFilter))
                {
                    m_PreferredCUFilter = "'**NOTDefined**'";
                }
                HandleCUDataFilters();
                //ResetDataSource();
            }
            else
            {
                m_PreferredCUFilter = string.Empty;
                HandleCUDataFilters();
                //ResetDataSource();
            }
        }
        private void SetPreferredFilter()
        {
            IGTApplication oApp = GTClassFactory.Create<IGTApplication>();
            CuCommonForms obj = new CuCommonForms(oApp);
            ADODB.Recordset rs = obj.GetPreferredCU(grdFilter.Rows[0].Cells[0].Value.ToString());
            m_PreferredCUFilter = obj.GetPreferredCuFilterString(rs);
        }
        private void dlgPriCUSelection_ResizeBegin(object sender, EventArgs e)
        {           
            grdCU.ColumnWidthChanged -= grdCU_ColumnWidthChanged;
            grdFilter.ColumnWidthChanged -= grdFilter_ColumnWidthChanged;
            grdCU.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grdFilter.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void dlgPriCUSelection_ResizeEnd(object sender, EventArgs e)
        {          
            grdCU.ColumnWidthChanged += grdCU_ColumnWidthChanged;
            grdFilter.ColumnWidthChanged += grdFilter_ColumnWidthChanged;
            grdCU.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            ResizeFilterTextBoxes(grdFilter, grdCU);
        }
        #endregion

        private void grdCU_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }
    }

}
