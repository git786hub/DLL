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
    public partial class frmSPFileToCopy : Form
    {
        public frmSPFileToCopy()
        {
            InitializeComponent();
        }



        private void dgvFilesToCopy_SelectionChanged(object sender, EventArgs e)
        {
            btOk.Enabled = true;
        }
    }
}
