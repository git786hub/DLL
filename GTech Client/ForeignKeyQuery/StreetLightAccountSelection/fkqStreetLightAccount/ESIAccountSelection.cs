using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public partial class ESIAccountSelection : Form
    {

        int _formWidth = 710;
        int _formHeight = 660;
        StreetLightAccount selectedSTLTAccount;
        public IList<StreetLightAccount> GeoStreetLightAccounts { get; set; }
        public IList<StreetLightAccount> StreetLightAccounts { get; set; }
        public StreetLightAccount SelectedSTLTAccount { get { return this.selectedSTLTAccount; } }

        /// <summary>
        /// 
        /// </summary>
        public ESIAccountSelection()
        {
            InitializeComponent();
            //this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;

            _formWidth = this.Size.Width;
            _formHeight = this.Size.Height;

            dtGridViewStreetAcct.AutoGenerateColumns = false;
            dtGridViewFilter.AllowUserToResizeColumns = false;
            dtGridViewStreetAcct.AllowUserToResizeColumns = true;

            dtGridViewStreetAcct.ColumnWidthChanged += dtGridViewStreetAcct_ColumnWidthChanged;
            dtGridViewStreetAcct.Columns["ESI_Location"].DataPropertyName = "ESI_Location";
            dtGridViewStreetAcct.Columns["Description"].DataPropertyName = "Description";
            dtGridViewStreetAcct.Columns["Rate_Schedule"].DataPropertyName = "Rate_Schedule";
            dtGridViewStreetAcct.Columns["Wattage"].DataPropertyName = "Wattage";
            dtGridViewStreetAcct.Columns["Lamp_Type"].DataPropertyName = "Lamp_Type";
            dtGridViewStreetAcct.Columns["Luminare_Style"].DataPropertyName = "Luminare_Style";

            dtGridViewStreetAcct.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            this.ResizeEnd += ESIAccountSelection_ResizeEnd;
            chkRemoveGeofilter.CheckedChanged += chkRemoveGeofilter_CheckedChanged;
        }

        #region Public Methods
        /// <summary>
        /// Load Street Light Account data into grid
        /// </summary>
        public void LoadData()
        {
            if (GeoStreetLightAccounts.Count > 0)
            {
                dtGridViewStreetAcct.DataSource = this.GeoStreetLightAccounts;
            }
            else
            {
                lblstatusMsg.Text = "No geographic filtering has been applied";
                dtGridViewStreetAcct.DataSource = this.StreetLightAccounts;
                chkRemoveGeofilter.Checked = true;
                chkRemoveGeofilter.Enabled = false;
            }
        }

        #endregion

        #region events

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (dtGridViewStreetAcct.SelectedRows.Count > 0)
            {
                selectedSTLTAccount = (StreetLightAccount)dtGridViewStreetAcct.SelectedRows[0].DataBoundItem;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            selectedSTLTAccount = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ESIAccountSelection_Load(object sender, EventArgs e)
        {
            dtGridViewFilter.Columns["txt_ESILocation"].DataPropertyName = "ESI_Location";
            dtGridViewFilter.Columns["txt_Description"].DataPropertyName = "Description";
            dtGridViewFilter.Columns["txt_RateSchedule"].DataPropertyName = "Rate_Schedule";
            dtGridViewFilter.Columns["txt_Wattage"].DataPropertyName = "Wattage";
            dtGridViewFilter.Columns["txt_LampType"].DataPropertyName = "Lamp_Type";
            dtGridViewFilter.Columns["txt_LuminareStyle"].DataPropertyName = "Luminare_Style";
            var filter = new List<StreetLightAccount>();

            filter.Add(new StreetLightAccount
            {
                ESI_Location = "",
                Description = "",
                Rate_Schedule = "",
                Wattage = "",
                Lamp_Type = "",
                Luminare_Style = ""
            });

            dtGridViewFilter.DataSource = filter;
            dtGridViewFilter.CellValueChanged += dtGridViewFilter_CellValueChanged;
            dtGridViewFilter.SelectionMode = DataGridViewSelectionMode.ColumnHeaderSelect;
        }

        /// <summary>
        /// fires when form resize and update controls size and location relatively
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ESIAccountSelection_ResizeEnd(object sender, EventArgs e)
        {
            Control control = (Control)sender;

            control.Size = new Size(control.Width, _formHeight);
            int _widthDelta = control.Size.Width - _formWidth;
            panel.Size = new Size(panel.Size.Width + _widthDelta, panel.Size.Height);
            gridGrpbox.Size = new Size(gridGrpbox.Size.Width + _widthDelta, gridGrpbox.Size.Height);

            txtGridHeader.Width = txtGridHeader.Size.Width + _widthDelta;
            dtGridViewFilter.Size = new Size(dtGridViewFilter.Size.Width + _widthDelta, dtGridViewFilter.Size.Height);
            dtGridViewStreetAcct.Size = new Size(dtGridViewStreetAcct.Size.Width + _widthDelta, dtGridViewStreetAcct.Size.Height);
            btnCancel.Location = new Point(btnCancel.Location.X + _widthDelta, btnCancel.Location.Y);
            btnOK.Location = new Point(btnOK.Location.X + _widthDelta, btnOK.Location.Y);
            _formWidth = control.Size.Width;
            foreach (DataGridViewColumn col in dtGridViewStreetAcct.Columns)
            {
                col.Width = col.Width + _widthDelta / 5;
            }
        }



        /// <summary>
        /// fires when user enter criteria to filter rows
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dtGridViewFilter_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dtGridViewFilter.Rows.Count > 0)
            {
                List<StreetLightAccount> filterAcct = ((List<StreetLightAccount>)dtGridViewFilter.DataSource);
                //checks filter criteria
                if (filterAcct[0].ESI_Location == "" && filterAcct[0].Description == "" && filterAcct[0].Rate_Schedule == "")
                {
                    if (chkRemoveGeofilter.Checked)
                    {
                        dtGridViewStreetAcct.DataSource = this.StreetLightAccounts;
                    }
                    else
                    {
                        dtGridViewStreetAcct.DataSource = this.GeoStreetLightAccounts;
                    }
                }
                else
                {
                    //Filter Street Light Accounts as per user inpput criteria
                    FilterStlAccountByCriteria(filterAcct[0].ESI_Location, filterAcct[0].Description, filterAcct[0].Rate_Schedule);
                }
            }
        }

        /// <summary>
        /// update  dtgridviewfilter column width
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtGridViewStreetAcct_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            dtGridViewFilter.Columns[e.Column.Index].Width = e.Column.Width;
        }

        /// <summary>
        /// enable/disable OK button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtGridViewStreetAcct_SelectionChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = dtGridViewStreetAcct.SelectedRows.Count > 0 ? true : false;
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// filter Street Light Account as per inpput criteria
        /// </summary>
        /// <param name="esiLocation"></param>
        /// <param name="desc"></param>
        /// <param name="rateSchedule"></param>
        private void FilterStlAccountByCriteria(string esiLocation, string desc, string rateSchedule)
        {
            if (chkRemoveGeofilter.Checked)
            {
                dtGridViewStreetAcct.DataSource = this.StreetLightAccounts.Where<StreetLightAccount>(a => a.ESI_Location.Contains(esiLocation)
                && a.Description.Contains(desc)
                && a.Rate_Schedule.Contains(rateSchedule)
                ).ToList<StreetLightAccount>();
            }
            else
            {
                dtGridViewStreetAcct.DataSource = this.GeoStreetLightAccounts.Where<StreetLightAccount>(a => a.ESI_Location.Contains(esiLocation)
                && a.Description.Contains(desc)
                && a.Rate_Schedule.Contains(rateSchedule)
                ).ToList<StreetLightAccount>();
            }
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkRemoveGeofilter_CheckedChanged(object sender, EventArgs e)
        {
            if (dtGridViewFilter.Rows.Count > 0)
            {
                dtGridViewFilter.Rows[0].Cells["txt_ESILocation"].Value = string.Empty;
                dtGridViewFilter.Rows[0].Cells["txt_Description"].Value = string.Empty;
                dtGridViewFilter.Rows[0].Cells["txt_RateSchedule"].Value = string.Empty;
            }
            if (!chkRemoveGeofilter.Checked)
            {
                dtGridViewStreetAcct.DataSource = this.GeoStreetLightAccounts;
            }
            else
            {
                dtGridViewStreetAcct.DataSource = this.StreetLightAccounts;
            }
        }
        #endregion

    }
}
