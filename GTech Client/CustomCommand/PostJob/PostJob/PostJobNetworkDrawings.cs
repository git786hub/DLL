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
  public partial class PostJobNetworkDrawings : Form
  {

    private List<NetworkDrawing> drawingList = null;

    internal List<NetworkDrawing> DrawingList
    {
      get
      {
        return drawingList;
      }
      set
      {
        drawingList = value;
        dgvDrawings.DataSource = drawingList;
      }
    }

    public PostJobNetworkDrawings()
    {
      InitializeComponent();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }
  }
}
