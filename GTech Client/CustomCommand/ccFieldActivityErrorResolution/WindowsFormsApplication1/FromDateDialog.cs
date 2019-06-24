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
    public partial class FromDateDialog : Form
    {
        internal DateTime fromDate;
        public FromDateDialog()
        {
            InitializeComponent();
        }

        private void FromDateDialog_Load(object sender, EventArgs e)
        {

        }

        private void cmdSubmit_Click(object sender, EventArgs e)
        {
            fromDate = fromDatePicker.Value;
            this.Hide();
        }
    }
}
