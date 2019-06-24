using System;
using System.Windows.Forms;
using GTechnology.Oncor.CustomAPI.Presenter;

namespace GTechnology.Oncor.CustomAPI.View
{
    public partial class FormWithMSLA : Form
    {
        public SupplementalAgreementPresenter m_formPresenter;
        public FormWithMSLA()
        {
            InitializeComponent();            
            dtPicker.Value = DateTime.Now;            
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            m_formPresenter.btnGenerateClik(sender, e);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.FormClosing -= FormWithMSLA_FormClosing;
            this.Close();
            m_formPresenter.CompleteCC();
        }

        private void dgvMSLA_KeyDown(object sender, KeyEventArgs e)
        {
            m_formPresenter.dgvKeyDown(sender, e);
        }

        private void FormWithMSLA_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.FormClosing -= FormWithMSLA_FormClosing;
            m_formPresenter.CompleteCC();
        }

        private void FormWithMSLA_Load(object sender, EventArgs e)
        {
            txtWR.Text = m_formPresenter.ActiveWR;
            txtCustomer.Text = m_formPresenter.Customer;
            txtAgreementDate.Text = Convert.ToString(m_formPresenter.AgreementDate.Value);



            dgvMSLA.DataSource = m_formPresenter.GridDataTable;
            
            if (dgvMSLA != null && dgvMSLA.Columns.Count > 0)
            {
                dgvMSLA.Columns[0].ReadOnly = true;
                dgvMSLA.Columns[1].ReadOnly = false;
                dgvMSLA.Columns[2].ReadOnly = false;
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
