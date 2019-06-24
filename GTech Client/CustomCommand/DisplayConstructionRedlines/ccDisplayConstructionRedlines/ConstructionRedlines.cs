using System;
using System.Windows.Forms;
using Intergraph.GTechnology.API;
using System.Data;
using System.IO;

namespace GTechnology.Oncor.CustomAPI
{
  public partial class ConstructionRedlines : Form
  {
    #region Variables
    private IGTApplication m_gtApplication;
    private DataTable m_dtRedlinesInfo;
    #endregion

    #region Constructor
    public ConstructionRedlines(DataTable dtRedlinesInfo)
    {
      InitializeComponent();
      m_gtApplication = GTClassFactory.Create<IGTApplication>();
      this.m_dtRedlinesInfo = dtRedlinesInfo;
    }
    #endregion

    #region Methods
    /// <summary>
    /// Method to adjust UI settings
    /// </summary>
    private void AdjustGridViewUI()
    {
      grdRedlineAttachments.Columns[0].ReadOnly = true;
      grdRedlineAttachments.Columns[1].ReadOnly = true;
      grdRedlineAttachments.Columns[2].Visible = false;
      grdRedlineAttachments.ClearSelection();
      grdRedlineAttachments.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
    }
    #endregion


    #region Events
    /// <summary>
    /// Display click event handler
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnDisplay_Click(object sender, EventArgs e)
    {
      try
      {
        if(grdRedlineAttachments.SelectedRows.Count > 0)
        {
          // No need to copy the file locally.
          //sFileName = Path.GetTempPath() + Convert.ToString(grdRedlineAttachments.SelectedRows[0].Cells[0].Value);
          //File.Copy(Convert.ToString(grdRedlineAttachments.SelectedRows[0].Cells[2].Value) + Convert.ToString(grdRedlineAttachments.SelectedRows[0].Cells[0].Value), sFileName, true);
          DataGridViewRow row = grdRedlineAttachments.SelectedRows[0];
          string sFileName = row.Cells[2].Value.ToString() + row.Cells[0].Value.ToString();
          m_gtApplication.ActiveMapWindow.DisplayService.AppendRedlines("Construction Redlines", sFileName, 0, true, true, GTDisplayPositionConstants.gtdsdpFront);
          m_gtApplication.RefreshWindows();
          this.DialogResult = DialogResult.Cancel;
          this.Close();
          m_gtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Display Construction Redlines completed.");
        }
        else
        {
          m_gtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "No Redline is selected.");
        }
      }
      catch(Exception)
      {
        throw;
      }
    }

    /// <summary>
    /// Cancel click event handler
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    /// <summary>
    /// Event to deselect all once after data is loaded
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void grdRedlineAttachments_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
    {
      grdRedlineAttachments.ClearSelection();
    }

    #endregion

    /// <summary>
    /// Close the form when ESC is press
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ConstructionRedlines_KeyDown(object sender, KeyEventArgs e)
    {
      try
      {
        if((Keys)e.KeyValue == Keys.Escape)
        {
          this.Close();
        }
      }
      catch(Exception)
      {
        throw;
      }
    }

    /// <summary>
    /// Form load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ConstructionRedlines_Load(object sender, EventArgs e)
    {
      lblNoRedlines.Visible = false;
      btnDisplay.Enabled = false;
      grdRedlineAttachments.DataSource = m_dtRedlinesInfo;
      if(m_dtRedlinesInfo.Rows.Count > 0)
      {
        AdjustGridViewUI();
      }
      else
      {
        m_gtApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "No Redline files attached to Active Job Design Area.");
        lblNoRedlines.Text = "No Construction Redlines are" + Environment.NewLine + "attached to Active Job Design Area.";
        lblNoRedlines.Visible = true;
      }
    }
    /// <summary>
    /// If a non-header-row cell is clicked, select the associated row and enable the Display button; else disable the Display button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void grdRedlineAttachments_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      if(-1 < e.RowIndex)
      {
        grdRedlineAttachments.Rows[e.RowIndex].Selected = true;
        btnDisplay.Enabled = true;
      }
      else
      {
        btnDisplay.Enabled = false;
      }
    }

  }
}
