using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ccImportAttachCSV 
{
    public partial class frmAttachCVS : Form
    {
        public frmAttachCVS()
        {
            InitializeComponent();
        }

        private void frmAttachCVS_Load(object sender, EventArgs e)
        {
            csGlobals.loadAttachGrid();
        }

        private void frmAttachCVS_Activated(object sender, EventArgs e)
        {
           // csGlobals.loadAttachGrid();
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            DataGridView tmpGrid;
            // Save the data into the log xlsx file.
            tmpGrid = (DataGridView)csGlobals.frmTmpAttachments.Controls["dataGridView1"];
            csGlobals.saveCSVFile(tmpGrid);
            this.Close();
            // end the command.
            csGlobals.gCcHelper.Complete();

        }

        private void btValidate_Click(object sender, EventArgs e)
        {
            DataGridView tmpGrid;
            tmpGrid = (DataGridView) csGlobals.frmTmpAttachments.Controls["dataGridView1"];
            // add the process status column if it does not exist.
            if (tmpGrid.Columns[tmpGrid.Columns.Count - 1].HeaderText != "Process Status")
            {
                tmpGrid.Columns.Add("colStatus", "Process Status");
            }
            // Validate the header columns against the data in the JU_CSVIMPORT_FORMAT table.
            csGlobals.ValidatePassFail = csGlobals.validateHeader(tmpGrid, this);
            if (!csGlobals.ValidatePassFail)
            {
                this.Controls["lblMsg"].Text = "Validation failed." + this.Controls["lblMsg"].Text;
                this.lblMsg.Refresh();
            }
            else
            {
                // validate the Grid data.
                csGlobals.ValidatePassFail = csGlobals.ValidateGridData(tmpGrid);

                if (csGlobals.ValidatePassFail == true)
                {
                    this.Controls["btAddAttactments"].Enabled = true;
                    this.Controls["lblMsg"].Text = "Validation succeeded.";
                }
                else
                {
                    this.Controls["lblMsg"].Text = "Validation of the data failed.";
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btAddAttactments_Click(object sender, EventArgs e)
        {
            // add or update the attachments as defined in the grid.
            DataGridView tmpGrid;
            this.lblMsg.Text = "Processing the records.";
            this.lblMsg.Refresh();
            csGlobals.gApp.BeginWaitCursor();
            tmpGrid = (DataGridView) csGlobals.frmTmpAttachments.Controls["dataGridView1"];
            csGlobals.CreateAttachment(tmpGrid, this);
            this.lblMsg.Refresh();
            csGlobals.gApp.EndWaitCursor();
        }

    }
}
