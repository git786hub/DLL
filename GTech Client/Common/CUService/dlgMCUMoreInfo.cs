using System;
using System.Data;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public partial class dlgMCUMoreInfo : Form
    {
        public string MUID { get; set; }
        public string MUDescription { get; set; }
        public string Status { get; set; }
        public string EffectiveDate { get; set; }
        public string ExpirationDate { get; set; }
        public DataTable GridData { get; set; }

        public dlgMCUMoreInfo()
        {
            InitializeComponent();
        }

        private void MCUMoreInfoForm_Load(object sender, EventArgs e)
        {
            lblDynamicDescription.Text = MUDescription;
            lblDynamicEffectiveDate.Text = EffectiveDate;
            lblDynamicExpirationDate.Text = ExpirationDate;
            lblDynamicMUID.Text = MUID;
            grdData.DataSource = GridData;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
