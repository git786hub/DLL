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
    public partial class ToDateDialog : Form
    {
        public ToDateDialog()
        {
            InitializeComponent();
        }

        internal DateTime toDate;
        private void ToDateDialog_Load(object sender, EventArgs e)
        {

        }

        private void cmdSubmit_Click(object sender, EventArgs e)
        {
            toDate = ToDatePicker.Value;
            this.Hide();
        }
    }
}
