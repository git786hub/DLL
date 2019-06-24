using System;
using System.Threading.Tasks;
using Intergraph.GTechnology.Interfaces;
using Intergraph.GTechnology.API;
using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
  internal partial class ConfirmComplete : Form
  {

    public bool enableOK
    {
      set { this.btnOK.Enabled = value; }
    }

    public string statusText
    {
      set { this.lblStatus.Text = value; }
    }

    public ConfirmComplete()
    {
      InitializeComponent();
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      IGTApplication app = GTClassFactory.Create<IGTApplication>();
      app.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, string.Empty);
      this.Close();
    }

        //private void InitializeComponent()
        //{
        //    this.SuspendLayout();
        //    // 
        //    // ConfirmComplete
        //    // 
        //    this.AutoSize = true;
        //    this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        //    this.ClientSize = new System.Drawing.Size(284, 261);
        //    this.Name = "ConfirmComplete";
        //    this.ResumeLayout(false);

        //}
    }
}
