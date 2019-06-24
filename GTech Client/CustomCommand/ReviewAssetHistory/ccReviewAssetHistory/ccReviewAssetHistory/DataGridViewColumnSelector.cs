// ===================================================
//  Copyright 2018 Intergraph Corp.
//  File Name: DataGridViewColumnSelector.cs
// 
//  Description: Datagridview filter opeartions are done based on this class.
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
using System.Windows.Forms;
using System.Data;
using System.Drawing;

namespace GTechnology.Oncor.CustomAPI
{
    public class DataGridViewColumnSelector
    {
        // the DataGridView to which the DataGridViewColumnSelector is attached
        private DataGridView mDataGridView = null;

        // a ComboBox containing the column header text and checkboxes
        public ComboBox mDropdownListBoxUser;
        public ComboBox mDropdownListBoxOperation;
        public ComboBox mDropdownListBoxFeatureClass;
        public ComboBox mDropdownListBoxComponent;
        public ComboBox mDropdownListBoxAttribute;
        public ComboBox mDropdownListBoxOldValue;
        public ComboBox mDropdownListBoxNewValue;
        public ComboBox mDropdownListBoxDesigner;

        // a ToolStripDropDown object used to show the popup
        public ToolStripDropDown mPopupUser;
        public ToolStripDropDown mPopupOperation;
        public ToolStripDropDown mPopupFeatureClass;
        public ToolStripDropDown mPopupComponent;
        public ToolStripDropDown mPopupAttribute;
        public ToolStripDropDown mPopupOldValue;
        public ToolStripDropDown mPopupNewValue;
        public ToolStripDropDown mPopupDesigner;


        //Reffering the asset history model data.
        private AssetHistoryModel mModel;


        private List<ccSortedData> _mSortedDataList;

        //To track the sorted columns on datagridview. (Sorted column data logic is reused to support multi column sort/single column sort by changing the single line in the code.
        //Presently the code supports single column sorting.)
        public List<ccSortedData> mSortedDataList
        {
            get
            {
                return _mSortedDataList;
            }
            set
            {
                _mSortedDataList = value;
            }
        }

        public DataGridView DataGridView
        {
            get { return mDataGridView; }
            set
            {
                mDataGridView = value;
            }
        }

        /// <summary>
        /// Loading all filters data on datagrid.
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="model"></param>
        internal DataGridViewColumnSelector(DataGridView dgv, AssetHistoryModel model)
        {
            _mSortedDataList = null;
            mSortedDataList = new List<ccSortedData>();
            this.DataGridView = dgv;

            //-------------

            mDropdownListBoxUser = new ComboBox();
            mDropdownListBoxUser.DropDownStyle = ComboBoxStyle.DropDownList;
            mDropdownListBoxUser.SelectedIndexChanged += MComboBoxUser_SelectedIndexChanged;
            mDropdownListBoxUser.DropDown += MDropdownListBoxUser_DropDown;

            ToolStripControlHost mControlHostUser = new ToolStripControlHost(mDropdownListBoxUser);
            mControlHostUser.Padding = Padding.Empty;
            mControlHostUser.Margin = Padding.Empty;
            mControlHostUser.AutoSize = true;

            mPopupUser = new ToolStripDropDown();
            mPopupUser.Padding = Padding.Empty;
            mPopupUser.Items.Add(mControlHostUser);

            //-------------
            mDropdownListBoxAttribute = new ComboBox();
            mDropdownListBoxAttribute.DropDownStyle = ComboBoxStyle.DropDownList;
            mDropdownListBoxAttribute.SelectedIndexChanged += MComboBoxAttribute_SelectedIndexChanged;
            mDropdownListBoxAttribute.DropDown += MDropdownListBoxAttribute_DropDown;

            ToolStripControlHost mControlHostAttribute = new ToolStripControlHost(mDropdownListBoxAttribute);
            mControlHostAttribute.Padding = Padding.Empty;
            mControlHostAttribute.Margin = Padding.Empty;
            mControlHostAttribute.AutoSize = true;

            mPopupAttribute = new ToolStripDropDown();
            mPopupAttribute.Padding = Padding.Empty;
            mPopupAttribute.Items.Add(mControlHostAttribute);

            //-------------

            mDropdownListBoxComponent = new ComboBox();
            mDropdownListBoxComponent.DropDownStyle = ComboBoxStyle.DropDownList;
            mDropdownListBoxComponent.SelectedIndexChanged += MComboBoxComponent_SelectedIndexChanged;
            mDropdownListBoxComponent.DropDown += MDropdownListBoxComponent_DropDown;

            ToolStripControlHost mControlHostComponent = new ToolStripControlHost(mDropdownListBoxComponent);
            mControlHostComponent.Padding = Padding.Empty;
            mControlHostComponent.Margin = Padding.Empty;
            mControlHostComponent.AutoSize = true;

            mPopupComponent = new ToolStripDropDown();
            mPopupComponent.Padding = Padding.Empty;
            mPopupComponent.Items.Add(mControlHostComponent);

            //-------------

            mDropdownListBoxDesigner = new ComboBox();
            mDropdownListBoxDesigner.DropDownStyle = ComboBoxStyle.DropDownList;
            mDropdownListBoxDesigner.SelectedIndexChanged += MComboBoxDesigner_SelectedIndexChanged;
            mDropdownListBoxDesigner.DropDown += MDropdownListBoxDesigner_DropDown;

            ToolStripControlHost mControlHostDesigner = new ToolStripControlHost(mDropdownListBoxDesigner);
            mControlHostDesigner.Padding = Padding.Empty;
            mControlHostDesigner.Margin = Padding.Empty;
            mControlHostDesigner.AutoSize = true;

            mPopupDesigner = new ToolStripDropDown();
            mPopupDesigner.Padding = Padding.Empty;
            mPopupDesigner.Items.Add(mControlHostDesigner);

            //-------------

            mDropdownListBoxFeatureClass = new ComboBox();
            mDropdownListBoxFeatureClass.DropDownStyle = ComboBoxStyle.DropDownList;
            mDropdownListBoxFeatureClass.SelectedIndexChanged += MComboBoxFeatureClass_SelectedIndexChanged;
            mDropdownListBoxFeatureClass.DropDown += MDropdownListBoxFeatureClass_DropDown;

            ToolStripControlHost mControlHostFeatureClass = new ToolStripControlHost(mDropdownListBoxFeatureClass);
            mControlHostFeatureClass.Padding = Padding.Empty;
            mControlHostFeatureClass.Margin = Padding.Empty;
            mControlHostFeatureClass.AutoSize = true;

            mPopupFeatureClass = new ToolStripDropDown();
            mPopupFeatureClass.Padding = Padding.Empty;
            mPopupFeatureClass.Items.Add(mControlHostFeatureClass);

            //-------------

            mDropdownListBoxNewValue = new ComboBox();
            mDropdownListBoxNewValue.DropDownStyle = ComboBoxStyle.DropDownList;
            mDropdownListBoxNewValue.SelectedIndexChanged += MComboBoxNewValue_SelectedIndexChanged;
            mDropdownListBoxNewValue.DropDown += MDropdownListBoxNewValue_DropDown;

            ToolStripControlHost mControlHostNewValue = new ToolStripControlHost(mDropdownListBoxNewValue);
            mControlHostNewValue.Padding = Padding.Empty;
            mControlHostNewValue.Margin = Padding.Empty;
            mControlHostNewValue.AutoSize = true;

            mPopupNewValue = new ToolStripDropDown();
            mPopupNewValue.Padding = Padding.Empty;
            mPopupNewValue.Items.Add(mControlHostNewValue);


            //-------------
            mDropdownListBoxOldValue = new ComboBox();
            mDropdownListBoxOldValue.DropDownStyle = ComboBoxStyle.DropDownList;
            mDropdownListBoxOldValue.SelectedIndexChanged += MComboBoxOldValue_SelectedIndexChanged;
            mDropdownListBoxOldValue.DropDown += MDropdownListBoxOldValue_DropDown;

            ToolStripControlHost mControlHostOldValue = new ToolStripControlHost(mDropdownListBoxOldValue);
            mControlHostOldValue.Padding = Padding.Empty;
            mControlHostOldValue.Margin = Padding.Empty;
            mControlHostOldValue.AutoSize = true;

            mPopupOldValue = new ToolStripDropDown();
            mPopupOldValue.Padding = Padding.Empty;
            mPopupOldValue.Items.Add(mControlHostOldValue);

            //-------------
            mDropdownListBoxOperation = new ComboBox();
            mDropdownListBoxOperation.DropDownStyle = ComboBoxStyle.DropDownList;
            mDropdownListBoxOperation.SelectedIndexChanged += MComboBoxOperation_SelectedIndexChanged;
            mDropdownListBoxOperation.DropDown += MDropdownListBoxOperation_DropDown;

            ToolStripControlHost mControlHostOperation = new ToolStripControlHost(mDropdownListBoxOperation);
            mControlHostOperation.Padding = Padding.Empty;
            mControlHostOperation.Margin = Padding.Empty;
            mControlHostOperation.AutoSize = true;

            mPopupOperation = new ToolStripDropDown();
            mPopupOperation.Padding = Padding.Empty;
            mPopupOperation.Items.Add(mControlHostOperation);


            this.mModel = model;

            if (mModel.m_GridDataTable.Rows.Count > 0)
            {
                mDropdownListBoxUser.Items.Add("");
                mDropdownListBoxUser.Items.AddRange(mModel.m_UserData);
                mDropdownListBoxUser.Name = "User";

                mDropdownListBoxOperation.Items.Add("");
                mDropdownListBoxOperation.Items.AddRange(mModel.m_OperationData);
                mDropdownListBoxOperation.Name = "Operation";

                mDropdownListBoxFeatureClass.Items.Add("");
                mDropdownListBoxFeatureClass.Items.AddRange(mModel.m_FeatureClassData);
                mDropdownListBoxFeatureClass.Name = "Feature Class";

                mDropdownListBoxComponent.Items.Add("");
                mDropdownListBoxComponent.Items.AddRange(mModel.m_ComponentData);
                mDropdownListBoxComponent.Name = "Component";

                mDropdownListBoxAttribute.Items.Add("");
                mDropdownListBoxAttribute.Items.AddRange(mModel.m_AttributeData);
                mDropdownListBoxAttribute.Name = "Attribute";

                mDropdownListBoxOldValue.Items.Add("");
                mDropdownListBoxOldValue.Items.AddRange(mModel.m_OldValueData);
                mDropdownListBoxOldValue.Name = "Old Value";

                mDropdownListBoxNewValue.Items.Add("");
                mDropdownListBoxNewValue.Items.AddRange(mModel.m_NewValueData);
                mDropdownListBoxNewValue.Name = "New Value";

                mDropdownListBoxDesigner.Items.Add("");
                mDropdownListBoxDesigner.Items.AddRange(mModel.m_DesignerData);
                mDropdownListBoxDesigner.Name = "Designer";
            }

        }

        private void MDropdownListBoxOperation_DropDown(object sender, EventArgs e)
        {
            AdjustStyleOfComboBox(sender, e);
        }

        private void MDropdownListBoxOldValue_DropDown(object sender, EventArgs e)
        {
            AdjustStyleOfComboBox(sender, e);
        }

        private void MDropdownListBoxNewValue_DropDown(object sender, EventArgs e)
        {
            AdjustStyleOfComboBox(sender, e);
        }

        private void MDropdownListBoxFeatureClass_DropDown(object sender, EventArgs e)
        {
            AdjustStyleOfComboBox(sender, e);
        }

        private void MDropdownListBoxDesigner_DropDown(object sender, EventArgs e)
        {
            AdjustStyleOfComboBox(sender, e);
        }

        private void MDropdownListBoxComponent_DropDown(object sender, EventArgs e)
        {
            AdjustStyleOfComboBox(sender, e);
        }

        private void MDropdownListBoxAttribute_DropDown(object sender, EventArgs e)
        {
            AdjustStyleOfComboBox(sender, e);
        }

        private void MDropdownListBoxUser_DropDown(object sender, EventArgs e)
        {
            AdjustStyleOfComboBox(sender, e);
        }
        private void AdjustStyleOfComboBox(object sender, System.EventArgs e)
        {
            ComboBox senderComboBox = (ComboBox)sender;
            int width = senderComboBox.DropDownWidth;
            Graphics g = senderComboBox.CreateGraphics();
            Font font = senderComboBox.Font;
            int vertScrollBarWidth =
                (senderComboBox.Items.Count > senderComboBox.MaxDropDownItems)
                ? SystemInformation.VerticalScrollBarWidth : 0;

            int newWidth;
            foreach (string s in ((ComboBox)sender).Items)
            {
                newWidth = (int)g.MeasureString(s, font).Width
                    + vertScrollBarWidth;
                if (width < newWidth)
                {
                    width = newWidth;
                }
            }
            senderComboBox.DropDownWidth = width;
        }

        #region Datagridview Checked List Box Filter Events

        /// <summary>
        /// Old Value filter event : Getting filter exp and applying to the datagridview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MComboBoxOldValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataView dvFilter = null;
            DataTable dtFilterTable = null;
            try
            {
                if (mDropdownListBoxOldValue.SelectedIndex > -1)
                {
                    string strFilterExp = "";

                    if (!string.IsNullOrEmpty(Convert.ToString(mDropdownListBoxOldValue.SelectedItem)))
                    {
                        if (string.IsNullOrEmpty(strFilterExp))
                            strFilterExp += "(" + "[" + mDropdownListBoxOldValue.Name + "]" + " IN ('" + mDropdownListBoxOldValue.SelectedItem + "')";
                        else
                        {
                            strFilterExp += " OR " + "" + "[" + mDropdownListBoxOldValue.Name + "]" + " IN ('" + mDropdownListBoxOldValue.SelectedItem + "')";
                        }
                    }
                    if (!string.IsNullOrEmpty(strFilterExp))
                        strFilterExp += ")";


                    strFilterExp = GetFilterString(strFilterExp, mDropdownListBoxOldValue.Name);

                    if (this.DataGridView.DataSource is DataTable)
                    {
                        dtFilterTable = (DataTable)this.DataGridView.DataSource;
                        dvFilter = dtFilterTable.DefaultView;
                    }
                    else if (this.DataGridView.DataSource is DataView)
                    {
                        dvFilter = (DataView)DataGridView.DataSource;
                    }


                    dvFilter.RowFilter = strFilterExp;
                    this.DataGridView.DataSource = null;
                    this.DataGridView.DataSource = typeof(DataView);

                    this.DataGridView.DataSource = dvFilter;
                    this.DataGridView.Columns["Designer"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    this.DataGridView.EndEdit();
                    this.DataGridView.Refresh();


                    foreach (DataGridViewColumn dc in this.DataGridView.Columns)
                    {
                        if (dc.SortMode != DataGridViewColumnSortMode.NotSortable)
                        {
                            dc.SortMode = DataGridViewColumnSortMode.Programmatic;
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
        /// New Value filter event : Getting filter exp and applying to the datagridview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MComboBoxNewValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataView dvFilter = null;
            DataTable dtFilterTable = null;
            try
            {
                if (mDropdownListBoxNewValue.SelectedIndex > -1)
                {
                    string strFilterExp = "";

                    if (!string.IsNullOrEmpty(Convert.ToString(mDropdownListBoxNewValue.SelectedItem)))
                    {
                        if (string.IsNullOrEmpty(strFilterExp))
                            strFilterExp += "(" + "[" + mDropdownListBoxNewValue.Name + "]" + " IN ('" + mDropdownListBoxNewValue.SelectedItem + "')";
                        else
                        {
                            strFilterExp += " OR " + "" + "[" + mDropdownListBoxNewValue.Name + "]" + " IN ('" + mDropdownListBoxNewValue.SelectedItem + "')";
                        }
                    }
                    if (!string.IsNullOrEmpty(strFilterExp))
                        strFilterExp += ")";

                    strFilterExp = GetFilterString(strFilterExp, mDropdownListBoxNewValue.Name);


                    if (this.DataGridView.DataSource is DataTable)
                    {
                        dtFilterTable = (DataTable)this.DataGridView.DataSource;
                        dvFilter = dtFilterTable.DefaultView;
                    }
                    else if (this.DataGridView.DataSource is DataView)
                    {
                        dvFilter = (DataView)DataGridView.DataSource;
                    }


                    dvFilter.RowFilter = strFilterExp;
                    this.DataGridView.DataSource = null;
                    this.DataGridView.DataSource = typeof(DataView);

                    this.DataGridView.DataSource = dvFilter;
                    this.DataGridView.Columns["Designer"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    this.DataGridView.EndEdit();
                    this.DataGridView.Refresh();



                    foreach (DataGridViewColumn dc in this.DataGridView.Columns)
                    {
                        if (dc.SortMode != DataGridViewColumnSortMode.NotSortable)
                        {
                            dc.SortMode = DataGridViewColumnSortMode.Programmatic;
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
        /// Feature Class filter event : Getting filter exp and applying to the datagridview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MComboBoxFeatureClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataView dvFilter = null;
            DataTable dtFilterTable = null;
            try
            {
                if (mDropdownListBoxFeatureClass.SelectedIndex > -1)
                {
                    string strFilterExp = "";

                    if (!string.IsNullOrEmpty(Convert.ToString(mDropdownListBoxFeatureClass.SelectedItem)))
                    {
                        if (string.IsNullOrEmpty(strFilterExp))
                            strFilterExp += "(" + "[" + mDropdownListBoxFeatureClass.Name + "]" + " IN ('" + mDropdownListBoxFeatureClass.SelectedItem + "')";
                        else
                        {
                            strFilterExp += " OR " + "" + "[" + mDropdownListBoxFeatureClass.Name + "]" + " IN ('" + mDropdownListBoxFeatureClass.SelectedItem + "')";
                        }
                    }
                    if (!string.IsNullOrEmpty(strFilterExp))
                        strFilterExp += ")";

                    strFilterExp = GetFilterString(strFilterExp, mDropdownListBoxFeatureClass.Name);


                    if (this.DataGridView.DataSource is DataTable)
                    {
                        dtFilterTable = (DataTable)this.DataGridView.DataSource;
                        dvFilter = dtFilterTable.DefaultView;
                    }
                    else if (this.DataGridView.DataSource is DataView)
                    {
                        dvFilter = (DataView)DataGridView.DataSource;
                    }


                    dvFilter.RowFilter = strFilterExp;
                    this.DataGridView.DataSource = null;
                    this.DataGridView.DataSource = typeof(DataView);

                    this.DataGridView.DataSource = dvFilter;
                    this.DataGridView.Columns["Designer"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    this.DataGridView.EndEdit();
                    this.DataGridView.Refresh();


                    foreach (DataGridViewColumn dc in this.DataGridView.Columns)
                    {
                        if (dc.SortMode != DataGridViewColumnSortMode.NotSortable)
                        {
                            dc.SortMode = DataGridViewColumnSortMode.Programmatic;
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
        /// Designer filter event : Getting filter exp and applying to the datagridview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MComboBoxDesigner_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataView dvFilter = null;
            DataTable dtFilterTable = null;
            try
            {
                if (mDropdownListBoxDesigner.SelectedIndex > -1)
                {
                    string strFilterExp = "";
                    if (!string.IsNullOrEmpty(Convert.ToString(mDropdownListBoxDesigner.SelectedItem)))
                    {
                        if (string.IsNullOrEmpty(strFilterExp))
                            strFilterExp += "(" + "[" + mDropdownListBoxDesigner.Name + "]" + " IN ('" + mDropdownListBoxDesigner.SelectedItem + "')";
                        else
                        {
                            strFilterExp += " OR " + "" + "[" + mDropdownListBoxDesigner.Name + "]" + " IN ('" + mDropdownListBoxDesigner.SelectedItem + "')";
                        }
                    }
                    if (!string.IsNullOrEmpty(strFilterExp))
                        strFilterExp += ")";

                    strFilterExp = GetFilterString(strFilterExp, mDropdownListBoxDesigner.Name);

                    if (this.DataGridView.DataSource is DataTable)
                    {
                        dtFilterTable = (DataTable)this.DataGridView.DataSource;
                        dvFilter = dtFilterTable.DefaultView;
                    }
                    else if (this.DataGridView.DataSource is DataView)
                    {
                        dvFilter = (DataView)DataGridView.DataSource;
                    }


                    dvFilter.RowFilter = strFilterExp;
                    this.DataGridView.DataSource = null;
                    this.DataGridView.DataSource = typeof(DataView);

                    this.DataGridView.DataSource = dvFilter;
                    this.DataGridView.Columns["Designer"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                    this.DataGridView.EndEdit();
                    this.DataGridView.Refresh();


                    foreach (DataGridViewColumn dc in this.DataGridView.Columns)
                    {
                        if (dc.SortMode != DataGridViewColumnSortMode.NotSortable)
                        {
                            dc.SortMode = DataGridViewColumnSortMode.Programmatic;
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
        /// Component filter event : Getting filter exp and applying to the datagridview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MComboBoxComponent_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataView dvFilter = null;
            DataTable dtFilterTable = null;
            try
            {
                if (mDropdownListBoxComponent.SelectedIndex > -1)
                {
                    string strFilterExp = "";

                    if (!string.IsNullOrEmpty(Convert.ToString(mDropdownListBoxComponent.SelectedItem)))
                    {
                        if (string.IsNullOrEmpty(strFilterExp))
                            strFilterExp += "(" + "[" + mDropdownListBoxComponent.Name + "]" + " IN ('" + mDropdownListBoxComponent.SelectedItem + "')";
                        else
                        {
                            strFilterExp += " OR " + "" + "[" + mDropdownListBoxComponent.Name + "]" + " IN ('" + mDropdownListBoxComponent.SelectedItem + "')";
                        }
                    }
                    if (!string.IsNullOrEmpty(strFilterExp))
                        strFilterExp += ")";

                    strFilterExp = GetFilterString(strFilterExp, mDropdownListBoxComponent.Name);


                    if (this.DataGridView.DataSource is DataTable)
                    {
                        dtFilterTable = (DataTable)this.DataGridView.DataSource;
                        dvFilter = dtFilterTable.DefaultView;
                    }
                    else if (this.DataGridView.DataSource is DataView)
                    {
                        dvFilter = (DataView)DataGridView.DataSource;
                    }


                    dvFilter.RowFilter = strFilterExp;
                    this.DataGridView.DataSource = null;
                    this.DataGridView.DataSource = typeof(DataView);

                    this.DataGridView.DataSource = dvFilter;
                    this.DataGridView.Columns["Designer"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    this.DataGridView.EndEdit();
                    this.DataGridView.Refresh();


                    foreach (DataGridViewColumn dc in this.DataGridView.Columns)
                    {
                        if (dc.SortMode != DataGridViewColumnSortMode.NotSortable)
                        {
                            dc.SortMode = DataGridViewColumnSortMode.Programmatic;
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
        /// Attribute filter event : Getting filter exp and applying to the datagridview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MComboBoxAttribute_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataView dvFilter = null;
            DataTable dtFilterTable = null;
            try
            {
                if (mDropdownListBoxAttribute.SelectedIndex > -1)
                {
                    string strFilterExp = "";

                    if (!string.IsNullOrEmpty(Convert.ToString(mDropdownListBoxAttribute.SelectedItem)))
                    {
                        if (string.IsNullOrEmpty(strFilterExp))
                            strFilterExp += "(" + "[" + mDropdownListBoxAttribute.Name + "]" + " IN ('" + mDropdownListBoxAttribute.SelectedItem + "')";
                        else
                        {
                            strFilterExp += " OR " + "" + "[" + mDropdownListBoxAttribute.Name + "]" + " IN ('" + mDropdownListBoxAttribute.SelectedItem + "')";
                        }
                    }
                    if (!string.IsNullOrEmpty(strFilterExp))
                        strFilterExp += ")";

                    strFilterExp = GetFilterString(strFilterExp, mDropdownListBoxAttribute.Name);


                    if (this.DataGridView.DataSource is DataTable)
                    {
                        dtFilterTable = (DataTable)this.DataGridView.DataSource;
                        dvFilter = dtFilterTable.DefaultView;
                    }
                    else if (this.DataGridView.DataSource is DataView)
                    {
                        dvFilter = (DataView)DataGridView.DataSource;
                    }


                    dvFilter.RowFilter = strFilterExp;
                    this.DataGridView.DataSource = null;
                    this.DataGridView.DataSource = typeof(DataView);

                    this.DataGridView.DataSource = dvFilter;
                    this.DataGridView.Columns["Designer"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    this.DataGridView.EndEdit();
                    this.DataGridView.Refresh();


                    foreach (DataGridViewColumn dc in this.DataGridView.Columns)
                    {
                        if (dc.SortMode != DataGridViewColumnSortMode.NotSortable)
                        {
                            dc.SortMode = DataGridViewColumnSortMode.Programmatic;
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
        /// Uset filter event : Getting filter exp and applying to the datagridview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MComboBoxUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataView dvFilter = null;
            DataTable dtFilterTable = null;
            try
            {
                if (mDropdownListBoxUser.SelectedIndex > -1)
                {
                    string strFilterExp = "";

                    if (!string.IsNullOrEmpty(Convert.ToString(mDropdownListBoxUser.SelectedItem)))
                    {
                        if (string.IsNullOrEmpty(strFilterExp))
                            strFilterExp += "(" + "[" + mDropdownListBoxUser.Name + "]" + " IN ('" + mDropdownListBoxUser.SelectedItem + "')";
                        else
                        {
                            strFilterExp += " OR " + "" + "[" + mDropdownListBoxUser.Name + "]" + " IN ('" + mDropdownListBoxUser.SelectedItem + "')";
                        }
                    }
                    if (!string.IsNullOrEmpty(strFilterExp))
                        strFilterExp += ")";

                    strFilterExp = GetFilterString(strFilterExp, mDropdownListBoxUser.Name);


                    if (this.DataGridView.DataSource is DataTable)
                    {
                        dtFilterTable = (DataTable)this.DataGridView.DataSource;
                        dvFilter = dtFilterTable.DefaultView;
                    }
                    else if (this.DataGridView.DataSource is DataView)
                    {
                        dvFilter = (DataView)DataGridView.DataSource;
                    }


                    dvFilter.RowFilter = strFilterExp;
                    this.DataGridView.DataSource = null;
                    this.DataGridView.DataSource = typeof(DataView);

                    this.DataGridView.DataSource = dvFilter;
                    this.DataGridView.Columns["Designer"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    this.DataGridView.EndEdit();
                    this.DataGridView.Refresh();

                    foreach (DataGridViewColumn dc in this.DataGridView.Columns)
                    {
                        if (dc.SortMode != DataGridViewColumnSortMode.NotSortable)
                        {
                            dc.SortMode = DataGridViewColumnSortMode.Programmatic;
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
        /// Operation filter event : Getting filter exp and applying to the datagridview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MComboBoxOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataView dvFilter = null;
            DataTable dtFilterTable = null;
            try
            {
                if (mDropdownListBoxOperation.SelectedIndex > -1)
                {
                    string strFilterExp = "";

                    if (!string.IsNullOrEmpty(Convert.ToString(mDropdownListBoxOperation.SelectedItem)))
                    {
                        if (string.IsNullOrEmpty(strFilterExp))
                            strFilterExp += "(" + "[" + mDropdownListBoxOperation.Name + "]" + " IN ('" + mDropdownListBoxOperation.SelectedItem + "')";
                        else
                        {
                            strFilterExp += " OR " + "" + "[" + mDropdownListBoxOperation.Name + "]" + " IN ('" + mDropdownListBoxOperation.SelectedItem + "')";
                        }
                    }
                    if (!string.IsNullOrEmpty(strFilterExp))
                        strFilterExp += ")";

                    strFilterExp = GetFilterString(strFilterExp, mDropdownListBoxOperation.Name);


                    if (this.DataGridView.DataSource is DataTable)
                    {
                        dtFilterTable = (DataTable)this.DataGridView.DataSource;
                        dvFilter = dtFilterTable.DefaultView;
                    }
                    else if (this.DataGridView.DataSource is DataView)
                    {
                        dvFilter = (DataView)DataGridView.DataSource;
                    }


                    dvFilter.RowFilter = strFilterExp;
                    this.DataGridView.DataSource = null;
                    this.DataGridView.DataSource = typeof(DataView);

                    this.DataGridView.DataSource = dvFilter;
                    this.DataGridView.Columns["Designer"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    this.DataGridView.EndEdit();
                    this.DataGridView.Refresh();


                    foreach (DataGridViewColumn dc in this.DataGridView.Columns)
                    {
                        if (dc.SortMode != DataGridViewColumnSortMode.NotSortable)
                        {
                            dc.SortMode = DataGridViewColumnSortMode.Programmatic;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        /// <summary>
        /// Getting filter expression to apply on datagridview.
        /// </summary>
        /// <param name="strFilterExp">Filter expression</param>
        /// <param name="chkListName">Checked list box name</param>
        /// <returns></returns>
        private string GetFilterString(string strFilterExp, string chkListName)
        {
            int count = 0;

            if (!string.IsNullOrEmpty(strFilterExp))
            {
                strFilterExp += " AND (";
            }
            else
            {
                strFilterExp = "(";
            }
            if (chkListName != mDropdownListBoxOldValue.Name)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(mDropdownListBoxOldValue.SelectedItem)))
                {
                    count = 1;
                    if (string.IsNullOrEmpty(strFilterExp))
                        strFilterExp += "" + "[" + mDropdownListBoxOldValue.Name + "]" + " IN ( '" + mDropdownListBoxOldValue.SelectedItem + "')";
                    else
                    {
                        strFilterExp += " OR " + "" + "[" + mDropdownListBoxOldValue.Name + "]" + " IN ( '" + mDropdownListBoxOldValue.SelectedItem + "')";
                    }
                }
            }

            if (count > 0)
            {
                if (!string.IsNullOrEmpty(strFilterExp))
                {
                    strFilterExp += " ) AND (";
                    count = 0;
                }
            }

            if (chkListName != mDropdownListBoxOperation.Name)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(mDropdownListBoxOperation.SelectedItem)))
                {
                    count = 1;
                    if (string.IsNullOrEmpty(strFilterExp))
                        strFilterExp = "" + mDropdownListBoxOperation.Name + " IN ( '" + mDropdownListBoxOperation.SelectedItem + "')";
                    else
                        strFilterExp += " OR " + "" + mDropdownListBoxOperation.Name + " IN ( '" + mDropdownListBoxOperation.SelectedItem + "')";
                }
            }

            if (count > 0)
            {
                if (!string.IsNullOrEmpty(strFilterExp))
                {
                    strFilterExp += " ) AND (";
                    count = 0;
                }
            }

            if (chkListName != mDropdownListBoxUser.Name)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(mDropdownListBoxUser.SelectedItem)))
                {
                    count = 1;
                    if (string.IsNullOrEmpty(strFilterExp))
                        strFilterExp += "" + mDropdownListBoxUser.Name + " IN ( '" + mDropdownListBoxUser.SelectedItem + "')";
                    else
                        strFilterExp += " OR " + "" + mDropdownListBoxUser.Name + " IN ( '" + mDropdownListBoxUser.SelectedItem + "')";
                }
            }

            if (count > 0)
            {
                if (!string.IsNullOrEmpty(strFilterExp))
                {
                    strFilterExp += " ) AND (";
                    count = 0;
                }
            }

            if (chkListName != mDropdownListBoxAttribute.Name)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(mDropdownListBoxAttribute.SelectedItem)))
                {
                    count = 1;
                    if (string.IsNullOrEmpty(strFilterExp))
                        strFilterExp += "" + mDropdownListBoxAttribute.Name + " IN ( '" + mDropdownListBoxAttribute.SelectedItem + "')";
                    else
                        strFilterExp += " OR " + "" + mDropdownListBoxAttribute.Name + " IN ( '" + mDropdownListBoxAttribute.SelectedItem + "')";
                }
            }

            if (count > 0)
            {
                if (!string.IsNullOrEmpty(strFilterExp))
                {
                    strFilterExp += " ) AND (";
                    count = 0;
                }
            }

            if (chkListName != mDropdownListBoxComponent.Name)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(mDropdownListBoxComponent.SelectedItem)))
                {
                    count = 1;
                    if (string.IsNullOrEmpty(strFilterExp))
                        strFilterExp += "" + mDropdownListBoxComponent.Name + " IN ( '" + mDropdownListBoxComponent.SelectedItem + "')";
                    else
                        strFilterExp += " OR " + "" + mDropdownListBoxComponent.Name + " IN ( '" + mDropdownListBoxComponent.SelectedItem + "')";
                }
            }

            if (count > 0)
            {
                if (!string.IsNullOrEmpty(strFilterExp))
                {
                    strFilterExp += " ) AND (";
                    count = 0;
                }
            }

            if (chkListName != mDropdownListBoxDesigner.Name)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(mDropdownListBoxDesigner.SelectedItem)))
                {
                    count = 1;
                    if (string.IsNullOrEmpty(strFilterExp))
                        strFilterExp += "" + mDropdownListBoxDesigner.Name + " IN ( '" + mDropdownListBoxDesigner.SelectedItem + "')";
                    else
                        strFilterExp += " OR " + "" + mDropdownListBoxDesigner.Name + " IN ( '" + mDropdownListBoxDesigner.SelectedItem + "')";
                }
            }

            if (count > 0)
            {
                if (!string.IsNullOrEmpty(strFilterExp))
                {
                    strFilterExp += " ) AND (";
                    count = 0;
                }
            }

            if (chkListName != mDropdownListBoxFeatureClass.Name)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(mDropdownListBoxFeatureClass.SelectedItem)))
                {
                    count = 1;
                    if (string.IsNullOrEmpty(strFilterExp))
                        strFilterExp += "" + "[" + mDropdownListBoxFeatureClass.Name + "]" + " IN ( '" + mDropdownListBoxFeatureClass.SelectedItem + "')";
                    else
                        strFilterExp += " OR " + "" + "[" + mDropdownListBoxFeatureClass.Name + "]" + " IN ( '" + mDropdownListBoxFeatureClass.SelectedItem + "')";
                }
            }

            if (count > 0)
            {
                if (!string.IsNullOrEmpty(strFilterExp))
                {
                    strFilterExp += " ) AND (";
                    count = 0;
                }
            }

            if (chkListName != mDropdownListBoxNewValue.Name)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(mDropdownListBoxNewValue.SelectedItem)))
                {
                    count = 1;
                    if (string.IsNullOrEmpty(strFilterExp))
                        strFilterExp += "" + "[" + mDropdownListBoxNewValue.Name + "]" + " IN ( '" + mDropdownListBoxNewValue.SelectedItem + "')";
                    else
                        strFilterExp += " OR " + "" + "[" + mDropdownListBoxNewValue.Name + "]" + " IN ( '" + mDropdownListBoxNewValue.SelectedItem + "')";
                }
            }

            if (count > 0)
            {
                if (!string.IsNullOrEmpty(strFilterExp))
                {
                    strFilterExp += " ) AND (";
                    count = 0;
                }
            }

            if (!string.IsNullOrEmpty(strFilterExp))
                strFilterExp += ")";


            strFilterExp = strFilterExp.Replace("AND ()", "");
            strFilterExp = strFilterExp.Replace("AND ( OR", "AND (");
            strFilterExp = strFilterExp.Replace("( OR", "(");
            strFilterExp = strFilterExp.Replace("()", "");
            return strFilterExp;
        }

        /// <summary>
        /// Formating the single save query to insert all at a time into named view tables.(Code needs to changed in this method ,If we want multi sorting. Currently code supports single column sort)
        /// </summary>
        /// <param name="viewId"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        internal string BuildSaveQuery(int viewId, string viewName)
        {
            string sql = "begin ";
            try
            {
                #region NewView

                sql += string.Format(" insert into asset_history_view(VIEW_ID,VIEW_NM) VALUES({0},\'{1}\')", viewId, viewName);

                #endregion

                #region Filter expression

                if (!string.IsNullOrEmpty(Convert.ToString(mDropdownListBoxAttribute.SelectedItem)))
                {
                    if (string.IsNullOrEmpty(sql))
                    {
                        sql = string.Format("insert into ASSET_HISTORY_VIEWFILTER(VIEW_ID,FILTER_COLUMN,FILTER_VALUE) VALUES({0},\'{1}\',\'{2}\')", viewId, mDropdownListBoxAttribute.Name, mDropdownListBoxAttribute.SelectedItem);
                    }
                    else
                    {
                        sql += ";" + string.Format("insert into ASSET_HISTORY_VIEWFILTER(VIEW_ID,FILTER_COLUMN,FILTER_VALUE) VALUES({0},\'{1}\',\'{2}\')", viewId, mDropdownListBoxAttribute.Name, mDropdownListBoxAttribute.SelectedItem);
                    }
                }

                if (!string.IsNullOrEmpty(Convert.ToString(mDropdownListBoxComponent.SelectedItem)))
                {
                    if (string.IsNullOrEmpty(sql))
                    {
                        sql = string.Format("insert into ASSET_HISTORY_VIEWFILTER(VIEW_ID,FILTER_COLUMN,FILTER_VALUE) VALUES({0},\'{1}\',\'{2}\')", viewId, mDropdownListBoxComponent.Name, mDropdownListBoxComponent.SelectedItem);
                    }
                    else
                    {
                        sql += ";" + string.Format("insert into ASSET_HISTORY_VIEWFILTER(VIEW_ID,FILTER_COLUMN,FILTER_VALUE) VALUES({0},\'{1}\',\'{2}\')", viewId, mDropdownListBoxComponent.Name, mDropdownListBoxComponent.SelectedItem);
                    }
                }

                if (!string.IsNullOrEmpty(Convert.ToString(mDropdownListBoxDesigner.SelectedItem)))
                {
                    if (string.IsNullOrEmpty(sql))
                    {
                        sql = string.Format("insert into ASSET_HISTORY_VIEWFILTER(VIEW_ID,FILTER_COLUMN,FILTER_VALUE) VALUES({0},\'{1}\',\'{2}\')", viewId, mDropdownListBoxDesigner.Name, mDropdownListBoxDesigner.SelectedItem);
                    }
                    else
                    {
                        sql += ";" + string.Format("insert into ASSET_HISTORY_VIEWFILTER(VIEW_ID,FILTER_COLUMN,FILTER_VALUE) VALUES({0},\'{1}\',\'{2}\')", viewId, mDropdownListBoxDesigner.Name, mDropdownListBoxDesigner.SelectedItem);
                    }
                }

                if (!string.IsNullOrEmpty(Convert.ToString(mDropdownListBoxFeatureClass.SelectedItem)))
                {
                    if (string.IsNullOrEmpty(sql))
                    {
                        sql = string.Format("insert into ASSET_HISTORY_VIEWFILTER(VIEW_ID,FILTER_COLUMN,FILTER_VALUE) VALUES({0},\'{1}\',\'{2}\')", viewId, mDropdownListBoxFeatureClass.Name, mDropdownListBoxFeatureClass.SelectedItem);
                    }
                    else
                    {
                        sql += ";" + string.Format("insert into ASSET_HISTORY_VIEWFILTER(VIEW_ID,FILTER_COLUMN,FILTER_VALUE) VALUES({0},\'{1}\',\'{2}\')", viewId, mDropdownListBoxFeatureClass.Name, mDropdownListBoxFeatureClass.SelectedItem);
                    }
                }

                if (!string.IsNullOrEmpty(Convert.ToString(mDropdownListBoxNewValue.SelectedItem)))
                {
                    if (string.IsNullOrEmpty(sql))
                    {
                        sql = string.Format("insert into ASSET_HISTORY_VIEWFILTER(VIEW_ID,FILTER_COLUMN,FILTER_VALUE) VALUES({0},\'{1}\',\'{2}\')", viewId, mDropdownListBoxNewValue.Name, mDropdownListBoxNewValue.SelectedItem);
                    }
                    else
                    {
                        sql += ";" + string.Format("insert into ASSET_HISTORY_VIEWFILTER(VIEW_ID,FILTER_COLUMN,FILTER_VALUE) VALUES({0},\'{1}\',\'{2}\')", viewId, mDropdownListBoxNewValue.Name, mDropdownListBoxNewValue.SelectedItem);
                    }
                }

                if (!string.IsNullOrEmpty(Convert.ToString(mDropdownListBoxOldValue.SelectedItem)))
                {
                    if (string.IsNullOrEmpty(sql))
                    {
                        sql = string.Format("insert into ASSET_HISTORY_VIEWFILTER(VIEW_ID,FILTER_COLUMN,FILTER_VALUE) VALUES({0},\'{1}\',\'{2}\')", viewId, mDropdownListBoxOldValue.Name, mDropdownListBoxOldValue.SelectedItem);
                    }
                    else
                    {
                        sql += ";" + string.Format("insert into ASSET_HISTORY_VIEWFILTER(VIEW_ID,FILTER_COLUMN,FILTER_VALUE) VALUES({0},\'{1}\',\'{2}\')", viewId, mDropdownListBoxOldValue.Name, mDropdownListBoxOldValue.SelectedItem);
                    }
                }

                if (!string.IsNullOrEmpty(Convert.ToString(mDropdownListBoxOperation.SelectedItem)))
                {
                    if (string.IsNullOrEmpty(sql))
                    {
                        sql = string.Format("insert into ASSET_HISTORY_VIEWFILTER(VIEW_ID,FILTER_COLUMN,FILTER_VALUE) VALUES({0},\'{1}\',\'{2}\')", viewId, mDropdownListBoxOperation.Name, mDropdownListBoxOperation.SelectedItem);
                    }
                    else
                    {
                        sql += ";" + string.Format("insert into ASSET_HISTORY_VIEWFILTER(VIEW_ID,FILTER_COLUMN,FILTER_VALUE) VALUES({0},\'{1}\',\'{2}\')", viewId, mDropdownListBoxOperation.Name, mDropdownListBoxOperation.SelectedItem);
                    }
                }

                if (!string.IsNullOrEmpty(Convert.ToString(mDropdownListBoxUser.SelectedItem)))
                {
                    if (string.IsNullOrEmpty(sql))
                    {
                        sql = string.Format("insert into ASSET_HISTORY_VIEWFILTER(VIEW_ID,FILTER_COLUMN,FILTER_VALUE) VALUES({0},\'{1}\',\'{2}\')", viewId, mDropdownListBoxUser.Name, mDropdownListBoxUser.SelectedItem);
                    }
                    else
                    {
                        sql += ";" + string.Format("insert into ASSET_HISTORY_VIEWFILTER(VIEW_ID,FILTER_COLUMN,FILTER_VALUE) VALUES({0},\'{1}\',\'{2}\')", viewId, mDropdownListBoxUser.Name, mDropdownListBoxUser.SelectedItem);
                    }
                }

                #endregion

                #region Sort expression

                if (mSortedDataList.Count > 0)
                {
                    // We are taking last column of the sorted list i.e last sorted column on datagridview.If we want to support multi sort please loop the list. 
                    //While inserting also i commented the code gave 1 as default sorted priority.
                    ccSortedData sorted = mSortedDataList[mSortedDataList.Count - 1];
                    if (sorted != null)
                    {
                        if (string.IsNullOrEmpty(sql))
                        {
                            //To support multi sorting please umcomment below sql insert sort priority
                            //sql = string.Format("insert into ASSET_HISTORY_VIEWSORT(VIEW_ID,SORT_COLUMN,SORT_PRIORITY,SORT_DIRECTION) VALUES({0},\'{1}\',{2},\'{3}\')",
                            //    viewId, sorted.m_SortedColumn, sorted.m_SortedPriority, sorted.m_SortedDirection);

                            sql = string.Format("insert into ASSET_HISTORY_VIEWSORT(VIEW_ID,SORT_COLUMN,SORT_PRIORITY,SORT_DIRECTION) VALUES({0},\'{1}\',{2},\'{3}\')",
                               viewId, sorted.m_SortedColumn, 1, sorted.m_SortedDirection);
                        }
                        else
                        {
                            //To support multi sorting please umcomment below sql insert sort priority
                            //sql += ";" + string.Format("insert into ASSET_HISTORY_VIEWSORT(VIEW_ID,SORT_COLUMN,SORT_PRIORITY,SORT_DIRECTION) VALUES({0},\'{1}\',{2},\'{3}\')",
                            //    viewId, sorted.m_SortedColumn, sorted.m_SortedPriority, sorted.m_SortedDirection);

                            sql += ";" + string.Format("insert into ASSET_HISTORY_VIEWSORT(VIEW_ID,SORT_COLUMN,SORT_PRIORITY,SORT_DIRECTION) VALUES({0},\'{1}\',{2},\'{3}\')",
                                viewId, sorted.m_SortedColumn, 1, sorted.m_SortedDirection);
                        }
                    }
                }

                #endregion

                if (!string.IsNullOrEmpty(sql))
                {
                    sql += ";COMMIT;end;";
                }
            }
            catch
            {
                throw;
            }
            return sql;
        }
    }
}
