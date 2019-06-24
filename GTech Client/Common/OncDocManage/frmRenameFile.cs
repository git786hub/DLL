using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OncDocManage
{
    public partial class frmRenameFile : Form
    {
        public frmRenameFile()
        {
            InitializeComponent();
        }

        private void cbReplcRenam_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {


                switch (cbReplcRenam.Text)
                {
                    case "Replace":
                        btUpdName.Text = "Replace";
                        btUpdName.Refresh();
                        txtNewName.Text = lblCurFileName.Text.Substring(lblCurFileName.Text.LastIndexOf(":")+2);
                        txtNewName.Enabled = false;
                        txtNewName.Refresh();
                        break;
                    case "Rename":
                        btUpdName.Text = "Update Name";
                        btUpdName.Refresh();
                        txtNewName.Enabled = true;
                        txtNewName.Refresh();
                        break;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "cbReplcRenam_SelectedIndexChanged Error", MessageBoxButtons.OK);
            }
        }
    }
}
