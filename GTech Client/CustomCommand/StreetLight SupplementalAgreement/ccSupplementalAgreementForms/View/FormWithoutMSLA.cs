using System.Windows.Forms;
using System;
using GTechnology.Oncor.CustomAPI.Presenter;

namespace GTechnology.Oncor.CustomAPI.View
{
    public partial class FormWithoutMSLA : Form
    {
        public SupplementalAgreementPresenter m_formPresenter;
        public FormWithoutMSLA()
        {
            InitializeComponent();            
        }

        private void btnGenerate_Click(object sender, System.EventArgs e)
        {
            m_formPresenter.btnGenerateClik(sender, e);
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.FormClosing -= FormWithoutMSLA_FormClosing;
            this.Close();
            m_formPresenter.CompleteCC();
        }

        private void dgvMSLA_KeyDown(object sender, KeyEventArgs e)
        {
            m_formPresenter.dgvKeyDown(sender, e);

            txtAddition.Text = Convert.ToString(m_formPresenter.m_Addition);
            txtRemoval.Text = Convert.ToString(m_formPresenter.m_Removal);
        }

        private void FormWithoutMSLA_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.FormClosing -= FormWithoutMSLA_FormClosing;
            this.Close();
            m_formPresenter.CompleteCC();
        }

        private void dgvMSLA_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            m_formPresenter.CellEndEvent(sender, e);

            txtAddition.Text = Convert.ToString(m_formPresenter.m_Addition);
            txtRemoval.Text = Convert.ToString(m_formPresenter.m_Removal);
        }

        private void FormWithoutMSLA_Load(object sender, System.EventArgs e)
        {
            txtWr.Text = m_formPresenter.ActiveWR;
            
            txtCity.Text = Convert.ToString(m_formPresenter.Customer);
            dgvMSLA.DataSource = m_formPresenter.GridDataTable;

            txtAddition.Text = Convert.ToString(m_formPresenter.m_Addition);
            txtRemoval.Text = Convert.ToString(m_formPresenter.m_Removal);

            if (dgvMSLA != null && dgvMSLA.Columns.Count > 0)
            {
                dgvMSLA.Columns[0].Visible = false;
                dgvMSLA.Columns[0].ReadOnly = true;

                dgvMSLA.Columns[1].ReadOnly = false;

                dgvMSLA.Columns[2].Visible = false;
                dgvMSLA.Columns[2].ReadOnly = true;

                dgvMSLA.Columns[3].ReadOnly = true;
                dgvMSLA.Columns[4].ReadOnly = true;
                dgvMSLA.Columns[5].ReadOnly = true;
                dgvMSLA.Columns[6].ReadOnly = true;
                dgvMSLA.Columns[7].ReadOnly = true;
                dgvMSLA.Columns[8].ReadOnly = true;
                dgvMSLA.Columns[9].ReadOnly = true;

                dgvMSLA.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvMSLA.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvMSLA.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvMSLA.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvMSLA.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvMSLA.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvMSLA.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvMSLA.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvMSLA.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvMSLA.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }          
            dgvMSLA.Columns["Address"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvMSLA.Columns["Identifying Type"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }

        private void txtCost_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtCost.Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers.");
                txtCost.Text = txtCost.Text.Remove(txtCost.Text.Length - 1);
            }
        }
    }
}
