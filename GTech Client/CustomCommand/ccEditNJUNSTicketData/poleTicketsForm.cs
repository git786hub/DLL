using System;
using System.Data;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public partial class poleTicketsForm : Form
    {
        DataTable dtLocalTickets= null;
        internal int selectedTicket = 0;
        public poleTicketsForm(DataTable list)
        {
            dtLocalTickets = list;
            InitializeComponent();
            dgvTickets.DataSource = dtLocalTickets;
        }

        /// <summary>
        /// Form load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void poleTicketsForm_Load(object sender, EventArgs e)
        {
            try
            {
                dgvTickets.Columns[0].ReadOnly = true;
                dgvTickets.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvTickets.CurrentCell.Selected = false;
                btnOk.Enabled = false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Ticket grid row header click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvTickets_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewRow row = ((DataGridView)sender).CurrentRow;
            if (row != null && row.Selected)
            {
                selectedTicket = Convert.ToInt32(row.Cells[0].Value);
                btnOk.Enabled = true;
            }
            else
            {
                btnOk.Enabled = false;
            }
        }

        /// <summary>
        /// Ok button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            return;
        }

        /// <summary>
        /// Tickets grid cell click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvTickets_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = ((DataGridView)sender).CurrentRow;
            if (row != null && row.Selected)
            {
                selectedTicket = Convert.ToInt32(row.Cells[0].Value);
                btnOk.Enabled = true;
            }
            else
            {
                btnOk.Enabled = false;
            }
        }
    }
}
