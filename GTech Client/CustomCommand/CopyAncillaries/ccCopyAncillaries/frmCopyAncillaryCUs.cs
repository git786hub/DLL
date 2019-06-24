using Intergraph.GTechnology.API;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public partial class frmCopyAncillaryCUs : Form
    {
        IGTApplication m_iGtApplication;
        IGTDDCKeyObject m_originalObject;
        List<CuInformation> m_oCuInformationList = new List<CuInformation>();
        bool m_canDeleteAllExistingAcus;

        public bool CanDeleteAllExistigAcus
        {
            get
            {
                return m_canDeleteAllExistingAcus;
            }
            set
            {
                m_canDeleteAllExistingAcus = value;
            }
        }
        public List<CuInformation> CuInformationList
        {
            get
            {
                return m_oCuInformationList;
            }
            set
            {
                m_oCuInformationList = value;
            }
        }

        public frmCopyAncillaryCUs(IGTDDCKeyObject initialFeature, DataTable dataTable)
        {
            InitializeComponent();
            m_iGtApplication = GTClassFactory.Create<IGTApplication>();
            m_originalObject = initialFeature;
            grdACUs.DataSource = dataTable;

            AdjustGridViewUI();
        }

        /// <summary>
        /// Method to adjust UI settings
        /// </summary>
        private void AdjustGridViewUI()
        {
            try
            {
                grdACUs.Columns[1].ReadOnly = true;
                grdACUs.Columns[2].ReadOnly = true;
                grdACUs.Columns[3].Visible = false;
                grdACUs.ClearSelection();
                for (int i = 0; i < grdACUs.Rows.Count; i++)
                {
                    grdACUs.Rows[i].Cells[0].Value = true;
                }
                grdACUs.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Cancel click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// Copy click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                IEnumerable<DataGridViewRow> rowsNotSelected = from DataGridViewRow row in grdACUs.Rows where Convert.ToBoolean(((DataGridViewRow)row).Cells[0].Value) == false select row;
                IEnumerable<DataGridViewRow> rowsSelected = from DataGridViewRow row in grdACUs.Rows where Convert.ToBoolean(((DataGridViewRow)row).Cells[0].Value) == true select row;

                m_canDeleteAllExistingAcus = cbxDeleteExistingACUs.Checked;
                if (rowsNotSelected != null && rowsNotSelected.Count() == grdACUs.Rows.Count)
                {
                    if (cbxDeleteExistingACUs.Checked)
                    {
                        m_canDeleteAllExistingAcus = true;
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        return;
                    }
                    else if (!cbxDeleteExistingACUs.Checked)
                    {
                        MessageBox.Show("The current settings on the form would result in no action being taken on the targeted features.", "G/Technology",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    foreach (DataGridViewRow row in rowsSelected)
                    {
                        m_oCuInformationList.Add(new CuInformation { CuCode = Convert.ToString(row.Cells[1].Value), CuDescription = Convert.ToString(row.Cells[2].Value), G3eCid = Convert.ToString(row.Cells[3].Value) });
                    }
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    return;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Data binding complete handler method that will clear any selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdACUs_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            grdACUs.ClearSelection();
        }
    }

}
