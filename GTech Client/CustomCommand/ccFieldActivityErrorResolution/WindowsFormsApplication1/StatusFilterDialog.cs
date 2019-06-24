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
    public partial class StatusFilterDialog : Form
    {
        internal List<string> selectedFilters = new List<string>();
        public StatusFilterDialog()
        {
            InitializeComponent();
            foreach (CheckBox checkBox in FilterOptionsBox.Controls)
            {
                if (checkBox.Checked)
                {
                    selectedFilters.Add(checkBox.Text);
                }
            }
        }

        private void OkCmd_Click(object sender, EventArgs e)
        {
            selectedFilters.Clear();
            foreach(CheckBox checkBox in FilterOptionsBox.Controls)
            {
                if (checkBox.Checked)
                {
                    selectedFilters.Add(checkBox.Text);
                }
            }
            this.Hide();
        }

        private void StatusFilterDialog_Load(object sender, EventArgs e)
        {

        }

        private void StatusFilterDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (CheckBox checkbox in FilterOptionsBox.Controls)
            {
                if (selectedFilters.Contains(checkbox.Text))
                {
                    checkbox.Checked = true;
                }else
                {
                    checkbox.Checked = false;
                }
              
            }
        }
    }
}
