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
    public partial class EditDateDialog : Form
    {
        internal DateTime fltrEditDate;
        public EditDateDialog()
        {
            InitializeComponent();
        }

        private void cmdSubmit_Click(object sender, EventArgs e)
        {
            fltrEditDate = editDatePicker.Value;
            this.Hide();
        }
    }
}
