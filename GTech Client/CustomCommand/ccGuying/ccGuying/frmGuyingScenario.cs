using System;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    public partial class frmGuyingScenario : Form
    {
        private bool m_CreateNewGuyingScenario;
        private string m_GuyingScenario;

        // Return true if new guying scenario is to be created
        public bool CreateNewGuyingScenario
        {
            get
            {
                return m_CreateNewGuyingScenario;
            }
        }

        // Return selected guying scenario
        public string GuyingScenario
        {
            get
            {
                return m_GuyingScenario;
            }
        }

        public frmGuyingScenario()
        {
            InitializeComponent();
        }

        private void cmdCreateNew_Click(object sender, EventArgs e)
        {
            m_CreateNewGuyingScenario = true;
            this.Close();
        }

        private void cmdUseExisting_Click(object sender, EventArgs e)
        {
            m_CreateNewGuyingScenario = false;
            m_GuyingScenario = cboGuyingScenarios.SelectedItem.ToString();
            this.Close();
        }
    }
}
