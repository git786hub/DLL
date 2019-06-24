using Intergraph.GTechnology.API;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public partial class frmSelectReplacementStatus : Form
    {
        private string m_replacementStatus;
        public string ReplacementStatus
        {
            get { return m_replacementStatus; }
            set { m_replacementStatus = value; }
        }

      

        public frmSelectReplacementStatus()
        {
            InitializeComponent();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            m_replacementStatus = "Remove";
            this.DialogResult = DialogResult.OK;
            return;
        }

        private void btnSalvage_Click(object sender, EventArgs e)
        {
            m_replacementStatus = "Salvage";
            this.DialogResult = DialogResult.OK;
            return;
        }

        private void btnAbandon_Click(object sender, EventArgs e)
        {
            m_replacementStatus = "Abandon";
            this.DialogResult = DialogResult.OK;
            return;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_replacementStatus = "Cancel";
            return;
        }
    }
}
