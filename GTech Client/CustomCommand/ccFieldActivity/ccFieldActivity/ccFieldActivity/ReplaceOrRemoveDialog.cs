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
    public partial class replaceOrRemoveDialog : Form
    {
        short featureType;
        public string activityCode = "";
        const short g_Service_Line_FNO = 54;
        const short g_Secondary_Box_FNO = 113;
        //same fno as area light, but will need to look at type
        const short g_Guard_Light_FNO = 61;


        public replaceOrRemoveDialog(short FNO)
        {
            featureType = FNO;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            switch (featureType)
            {
                case g_Service_Line_FNO:
                    ReplaceOrRemoveDialogMessage.Text.Replace("feature", "service line");
                    break;
                case g_Secondary_Box_FNO:
                    ReplaceOrRemoveDialogMessage.Text.Replace("or remove", "");
                    ReplaceOrRemoveDialogMessage.Text.Replace("feature", "secondary box");
                    break;
                case g_Guard_Light_FNO:
                    ReplaceOrRemoveDialogMessage.Text.Replace("feature", "guard light");
                    break;
            }

        }

        private void replaceCmd_btn_Click(object sender, EventArgs e)
        {
            activityCode = "UX";
            this.Hide();
        }

        private void removeCmd_btn_Click(object sender, EventArgs e)
        {
            activityCode = "UR";
            this.Hide();
        }

        private void cancelCmd_btn_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

    }
}
