using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IsFIMUp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCheckHealth_Click(object sender, EventArgs e)
        {
            string sHealthy;
            if (AEGIS_Sample_DLL.FIM.IsFimUp(txtServerName.Text))
            {
                sHealthy = "UP";
            }
            else
            {
                sHealthy = "DOWN";
            }

            MessageBox.Show("The FIM server " + txtServerName.Text + " is currently " + sHealthy, "FIM Server Health", MessageBoxButtons.OK);
        }
    }
}
