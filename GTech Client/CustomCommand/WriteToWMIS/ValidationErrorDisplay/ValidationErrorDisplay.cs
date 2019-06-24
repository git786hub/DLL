using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ADODB;
using Intergraph.GTechnology.Interfaces;
using Intergraph.GTechnology.Common;
using Intergraph.GTechnology.API;

namespace GTechnology.Oncor.CustomAPI
{
  public class ValidationErrorDisplay : Form
  {
    private Panel pnlBottom;
    private DataGridView dgvErrors;
    private DataGridViewTextBoxColumn colPriority;
    private DataGridViewTextBoxColumn colDescription;
    private DataGridViewTextBoxColumn colLocation;
    private DataGridViewTextBoxColumn colFeature;
    private DataGridViewTextBoxColumn colFNO;
    private DataGridViewTextBoxColumn colFID;
    private DataGridViewTextBoxColumn colComponent;
    private DataGridViewTextBoxColumn colCNO;
    private DataGridViewTextBoxColumn colCOD;
    private DataGridViewTextBoxColumn colConnection;
    private CheckBox chkFeatureExplorer;



    public ValidationErrorDisplay()
    {
      InitializeComponent();
    }


    private void InitializeComponent()
    {
      this.pnlBottom = new System.Windows.Forms.Panel();
      this.chkFeatureExplorer = new System.Windows.Forms.CheckBox();
      this.dgvErrors = new System.Windows.Forms.DataGridView();
      this.colPriority = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.colLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.colFeature = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.colFNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.colFID = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.colComponent = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.colCNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.colCOD = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.colConnection = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.pnlBottom.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dgvErrors)).BeginInit();
      this.SuspendLayout();
      // 
      // pnlBottom
      // 
      this.pnlBottom.Controls.Add(this.chkFeatureExplorer);
      this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.pnlBottom.Location = new System.Drawing.Point(0, 208);
      this.pnlBottom.Name = "pnlBottom";
      this.pnlBottom.Size = new System.Drawing.Size(1333, 37);
      this.pnlBottom.TabIndex = 0;
      // 
      // chkFeatureExplorer
      // 
      this.chkFeatureExplorer.AutoSize = true;
      this.chkFeatureExplorer.Location = new System.Drawing.Point(17, 13);
      this.chkFeatureExplorer.Name = "chkFeatureExplorer";
      this.chkFeatureExplorer.Size = new System.Drawing.Size(141, 17);
      this.chkFeatureExplorer.TabIndex = 0;
      this.chkFeatureExplorer.Text = "Update Feature Explorer";
      this.chkFeatureExplorer.UseVisualStyleBackColor = true;
      this.chkFeatureExplorer.CheckedChanged += new System.EventHandler(this.chkFeatureExplorer_CheckedChanged);
      // 
      // dgvErrors
      // 
      this.dgvErrors.AllowUserToAddRows = false;
      this.dgvErrors.AllowUserToDeleteRows = false;
      this.dgvErrors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvErrors.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPriority,
            this.colDescription,
            this.colLocation,
            this.colFeature,
            this.colFNO,
            this.colFID,
            this.colComponent,
            this.colCNO,
            this.colCOD,
            this.colConnection});
      this.dgvErrors.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dgvErrors.Location = new System.Drawing.Point(0, 0);
      this.dgvErrors.Name = "dgvErrors";
      this.dgvErrors.RowHeadersVisible = false;
      this.dgvErrors.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.dgvErrors.Size = new System.Drawing.Size(1333, 208);
      this.dgvErrors.TabIndex = 1;
      this.dgvErrors.SelectionChanged += new System.EventHandler(this.dgvErrors_SelectionChanged);
      // 
      // colPriority
      // 
      this.colPriority.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
      this.colPriority.HeaderText = "Priority";
      this.colPriority.Name = "colPriority";
      this.colPriority.Width = 63;
      // 
      // colDescription
      // 
      this.colDescription.HeaderText = "Description";
      this.colDescription.Name = "colDescription";
      this.colDescription.Width = 400;
      // 
      // colLocation
      // 
      this.colLocation.HeaderText = "Location";
      this.colLocation.Name = "colLocation";
      this.colLocation.Width = 250;
      // 
      // colFeature
      // 
      this.colFeature.HeaderText = "Feature";
      this.colFeature.Name = "colFeature";
      this.colFeature.Width = 150;
      // 
      // colFNO
      // 
      this.colFNO.HeaderText = "FNO";
      this.colFNO.Name = "colFNO";
      this.colFNO.Width = 40;
      // 
      // colFID
      // 
      this.colFID.HeaderText = "FID";
      this.colFID.Name = "colFID";
      this.colFID.Width = 70;
      // 
      // colComponent
      // 
      this.colComponent.HeaderText = "Component";
      this.colComponent.Name = "colComponent";
      this.colComponent.Width = 150;
      // 
      // colCNO
      // 
      this.colCNO.HeaderText = "CNO";
      this.colCNO.Name = "colCNO";
      this.colCNO.Width = 40;
      // 
      // colCOD
      // 
      this.colCOD.HeaderText = "CID";
      this.colCOD.Name = "colCOD";
      this.colCOD.Width = 40;
      // 
      // colConnection
      // 
      this.colConnection.HeaderText = "Connection";
      this.colConnection.Name = "colConnection";
      // 
      // ValidationErrorDisplay
      // 
      this.ClientSize = new System.Drawing.Size(1333, 245);
      this.Controls.Add(this.dgvErrors);
      this.Controls.Add(this.pnlBottom);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ValidationErrorDisplay";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Validation Errors";
      this.TopMost = true;
      this.pnlBottom.ResumeLayout(false);
      this.pnlBottom.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dgvErrors)).EndInit();
      this.ResumeLayout(false);

    }


    public Recordset Errors
    {
      get
      {
        return Errors;
      }
      set
      {
        this.dgvErrors.Rows.Clear();

        if(null == value || 0 == value.RecordCount)
        {
          return;
        }

        value.MoveFirst();
        do
        {
          // Populate the string array for each row to match the order of the columns where the data belongs.

          //Fields[0].Name="ErrorPriority"
          //Fields[1].Name="ErrorDescription"
          //Fields[2].Name= "ErrorLocation"
          // Feature Name
          //Fields[4].Name="G3E_FNO"
          //Fields[5].Name="G3E_FID"
          // Component Name
          //Fields[6].Name="G3E_CNO"
          //Fields[7].Name="G3E_CID" 
          //Fields[3].Name="Connection"

          string[] fieldvalues = new string[value.Fields.Count + 2];

          for(int i = 0;i < 3;i++)
          {
            fieldvalues[i] = value.Fields[i].Value.ToString();
          }

          short FNO = Convert.ToInt16(value.Fields[4].Value);
          fieldvalues[3] = FeatureNameByFNO(FNO);

          fieldvalues[4] = value.Fields[4].Value.ToString();
          fieldvalues[5] = value.Fields[5].Value.ToString();

          short CNO = Convert.ToInt16(value.Fields[6].Value);
          fieldvalues[6] = ComponentNameByCNO(CNO);

          //Alm-1899 Fix Changed Tostring to Convert.Tostring()
                    fieldvalues[7] = Convert.ToString(value.Fields[6].Value);
                    fieldvalues[8] = Convert.ToString(value.Fields[7].Value);

          fieldvalues[9] = value.Fields[3].Value.ToString(); // "Connection" is moved to last for this grid...

          this.dgvErrors.Rows.Add(fieldvalues);

          value.MoveNext();
        } while(!value.EOF);

        // Sort the grid by the Priority column
        this.dgvErrors.Sort(colPriority, System.ComponentModel.ListSortDirection.Ascending);

        this.dgvErrors.ClearSelection();
      }
    }

    private string FeatureNameByFNO(short FNO)
    {
      string retVal = string.Empty;

      try
      {
        IGTApplication app = GTClassFactory.Create<IGTApplication>();
        string sql = "select g3e_username from g3e_features_optable where g3e_fno=?";
        Recordset rs = app.DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, FNO);
        if(null != rs && 1 == rs.RecordCount)
        {
          retVal = rs.Fields[0].Value.ToString();
          rs.Close();
          rs = null;
        }
      }
      catch(Exception ex)
      {
        MessageBox.Show(string.Format("Exception caught in ValidationErrorDisplay.FeatureNameByFNO method: {0}", ex.Message));
      }
      return retVal;
    }

    private string ComponentNameByCNO(short CNO)
    {
      string retVal = string.Empty;

      try
      {
        IGTApplication app = GTClassFactory.Create<IGTApplication>();
        string sql = "select g3e_username from g3e_componentinfo_optable where g3e_cno=?";
        Recordset rs = app.DataContext.OpenRecordset(sql, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly, (int)CommandTypeEnum.adCmdText, CNO);
        if(null != rs && 1 == rs.RecordCount)
        {
          retVal = rs.Fields[0].Value.ToString();
          rs.Close();
          rs = null;
        }
      }
      catch(Exception ex)
      {
        MessageBox.Show(string.Format("Exception caught in ValidationErrorDisplay.ComponentNameByCNO method: {0}", ex.Message));
      }
      return retVal;
    }

    private void dgvErrors_SelectionChanged(object sender, EventArgs e)
    {
      if(this.chkFeatureExplorer.Checked)
      {
        this.ExploreFeatures();
      }
    }

    private void ExploreFeatures()
    {
      // Similar to the product's dialog, if multiple rows are selected,
      // the system does not explore or select the features.
      if(1 == this.dgvErrors.SelectedRows.Count)
      {
        IGTFeatureExplorerService fes = GTClassFactory.Create<IGTFeatureExplorerService>();
        IGTApplication app = GTClassFactory.Create<IGTApplication>();

        DataGridViewRow row = this.dgvErrors.SelectedRows[0];
        short FNO = Convert.ToInt16(row.Cells[colFNO.Index].Value);
        int FID = Convert.ToInt32(row.Cells[colFID.Index].Value);
        IGTKeyObject keyObject = app.DataContext.OpenFeature(FNO, FID);
        fes.ExploreFeature(keyObject, "Review");
        app.SelectedObjects.Clear();
        IGTDDCKeyObjects ddcKeyobjects = app.DataContext.GetDDCKeyObjects(FNO, FID, GTComponentGeometryConstants.gtddcgAllGeographic);
        app.SelectedObjects.Add(GTSelectModeConstants.gtsosmAllComponentsInActiveLegend, ddcKeyobjects[0]);
        fes.Visible = true;
      }

    }

    private void chkFeatureExplorer_CheckedChanged(object sender, EventArgs e)
    {
      if(this.chkFeatureExplorer.Checked)
      {
        this.ExploreFeatures();
      }
    }
  }
}
