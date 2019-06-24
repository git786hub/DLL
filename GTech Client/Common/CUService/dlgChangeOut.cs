using System;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public partial class dlgChangeOut : Form
    {

        public RemovalActivity RemovalActivitySet { get; set; }
        public InstallActivity InstallActivitySet { get; set; }
        public bool CancelClicked { get; set; }
        public dlgChangeOut(bool bReplaceCU)
        {
            InitializeComponent();
            CancelClicked = false;
            //ALM-1592-- Added a parameter to disable the "Replace with same CU" option if the CU is not found
            rdoInstallActivityReplaceWithSame.Enabled = bReplaceCU;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            CancelClicked = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            RemovalActivitySet = (rdoRemovalActivityRemove.Checked == true ? RemovalActivity.Remove : RemovalActivity.Salvage);

            if (rdoInstallActivityDoNotInstall.Checked)
            {
                InstallActivitySet = InstallActivity.DoNotInstall;
            }
            else if (rdoInstallActivityReplaceWithSame.Checked)
            {
                InstallActivitySet = InstallActivity.Replace;
            }
            else if (rdoInstallActivitySelect.Checked)
            {
                InstallActivitySet = InstallActivity.Select;
            }
            this.Close();
        }
    }
}
