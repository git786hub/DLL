using System;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI.View
{
    public partial class frmOverwriteDocument : Form
    {
        public string strResult = "";
        public frmOverwriteDocument()
        {
            InitializeComponent();
            txtDocName.Visible = false;
            txtDocName.Enabled = false;
        }

        private void rbDiffDoc_CheckedChanged(object sender, EventArgs e)
        {
            if(rbDiffDoc.Checked)
            {
                strResult = "N";
                txtDocName.Visible = true;
                txtDocName.Enabled = true;                
            }
        }

        private void rbCancel_CheckedChanged(object sender, EventArgs e)
        {
            if(rbCancel.Checked)
            {
                strResult = "C";
                txtDocName.Visible = false;
                txtDocName.Enabled = false;
            }
        }

        private void rbOverwrite_CheckedChanged(object sender, EventArgs e)
        {
            if(rbOverwrite.Checked)
            {
                strResult = "O";
                txtDocName.Visible = false;
                txtDocName.Enabled = false;                
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (rbDiffDoc.Checked)
            {
                if (!string.IsNullOrEmpty(txtDocName.Text))
                {
                    this.DialogResult = DialogResult.OK;
                    strResult = txtDocName.Text;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Please give document name.");
                }
            }
            else if (rbCancel.Checked)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
            else if(rbOverwrite.Checked)
            {
                this.DialogResult = DialogResult.Ignore;
                this.Close();
            }
        }
    }
}
