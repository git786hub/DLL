namespace GTechnology.Oncor.CustomAPI
{
  partial class PostJobNetworkDrawings
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if(disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.pnlTop = new System.Windows.Forms.Panel();
      this.label1 = new System.Windows.Forms.Label();
      this.pnlBottom = new System.Windows.Forms.Panel();
      this.btnOk = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.pnlCenter = new System.Windows.Forms.Panel();
      this.dgvDrawings = new System.Windows.Forms.DataGridView();
      this.colFeatureClass = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.colStructureID = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.colAction = new System.Windows.Forms.DataGridViewComboBoxColumn();
      this.colDwgLink = new System.Windows.Forms.DataGridViewLinkColumn();
      this.colDwgType = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.colDwgDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.pnlTop.SuspendLayout();
      this.pnlBottom.SuspendLayout();
      this.pnlCenter.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dgvDrawings)).BeginInit();
      this.SuspendLayout();
      // 
      // pnlTop
      // 
      this.pnlTop.Controls.Add(this.label1);
      this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
      this.pnlTop.Location = new System.Drawing.Point(0, 0);
      this.pnlTop.Name = "pnlTop";
      this.pnlTop.Size = new System.Drawing.Size(792, 32);
      this.pnlTop.TabIndex = 4;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(614, 17);
      this.label1.TabIndex = 4;
      this.label1.Text = "Verify whether the attached drawings are up to date, and specify the approprate a" +
    "ction for each.";
      // 
      // pnlBottom
      // 
      this.pnlBottom.Controls.Add(this.btnOk);
      this.pnlBottom.Controls.Add(this.btnCancel);
      this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.pnlBottom.Location = new System.Drawing.Point(0, 281);
      this.pnlBottom.Name = "pnlBottom";
      this.pnlBottom.Size = new System.Drawing.Size(792, 43);
      this.pnlBottom.TabIndex = 5;
      // 
      // btnOk
      // 
      this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnOk.Location = new System.Drawing.Point(624, 8);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new System.Drawing.Size(75, 23);
      this.btnOk.TabIndex = 3;
      this.btnOk.Text = "OK";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.Location = new System.Drawing.Point(705, 8);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      // 
      // pnlCenter
      // 
      this.pnlCenter.Controls.Add(this.dgvDrawings);
      this.pnlCenter.Dock = System.Windows.Forms.DockStyle.Fill;
      this.pnlCenter.Location = new System.Drawing.Point(0, 32);
      this.pnlCenter.Name = "pnlCenter";
      this.pnlCenter.Size = new System.Drawing.Size(792, 249);
      this.pnlCenter.TabIndex = 6;
      // 
      // dgvDrawings
      // 
      this.dgvDrawings.AllowUserToAddRows = false;
      this.dgvDrawings.AllowUserToDeleteRows = false;
      this.dgvDrawings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvDrawings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFeatureClass,
            this.colStructureID,
            this.colAction,
            this.colDwgLink,
            this.colDwgType,
            this.colDwgDesc});
      this.dgvDrawings.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dgvDrawings.Location = new System.Drawing.Point(0, 0);
      this.dgvDrawings.Name = "dgvDrawings";
      this.dgvDrawings.Size = new System.Drawing.Size(792, 249);
      this.dgvDrawings.TabIndex = 3;
      // 
      // colFeatureClass
      // 
      this.colFeatureClass.DataPropertyName = "FeatureClass";
      this.colFeatureClass.HeaderText = "Feature Class";
      this.colFeatureClass.Name = "colFeatureClass";
      this.colFeatureClass.ReadOnly = true;
      // 
      // colStructureID
      // 
      this.colStructureID.DataPropertyName = "StructureID";
      this.colStructureID.HeaderText = "Structure ID";
      this.colStructureID.Name = "colStructureID";
      this.colStructureID.ReadOnly = true;
      // 
      // colAction
      // 
      this.colAction.DataPropertyName = "Action";
      this.colAction.HeaderText = "Action";
      this.colAction.Items.AddRange(new object[] {
            "Keep",
            "Update",
            "Remove"});
      this.colAction.Name = "colAction";
      // 
      // colDwgLink
      // 
      this.colDwgLink.DataPropertyName = "Link";
      this.colDwgLink.HeaderText = "Link";
      this.colDwgLink.Name = "colDwgLink";
      this.colDwgLink.ReadOnly = true;
      this.colDwgLink.Width = 200;
      // 
      // colDwgType
      // 
      this.colDwgType.DataPropertyName = "DrawingType";
      this.colDwgType.HeaderText = "Drawing Type";
      this.colDwgType.Name = "colDwgType";
      this.colDwgType.ReadOnly = true;
      // 
      // colDwgDesc
      // 
      this.colDwgDesc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.colDwgDesc.DataPropertyName = "Description";
      this.colDwgDesc.HeaderText = "Description";
      this.colDwgDesc.Name = "colDwgDesc";
      this.colDwgDesc.ReadOnly = true;
      // 
      // PostJobNetworkDrawings
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(792, 324);
      this.Controls.Add(this.pnlCenter);
      this.Controls.Add(this.pnlBottom);
      this.Controls.Add(this.pnlTop);
      this.Name = "PostJobNetworkDrawings";
      this.Text = "Validate Network Drawings";
      this.pnlTop.ResumeLayout(false);
      this.pnlTop.PerformLayout();
      this.pnlBottom.ResumeLayout(false);
      this.pnlCenter.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.dgvDrawings)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion
    private System.Windows.Forms.Panel pnlTop;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Panel pnlBottom;
    private System.Windows.Forms.Button btnOk;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Panel pnlCenter;
    internal System.Windows.Forms.DataGridView dgvDrawings;
    private System.Windows.Forms.DataGridViewTextBoxColumn colFeatureClass;
    private System.Windows.Forms.DataGridViewTextBoxColumn colStructureID;
    private System.Windows.Forms.DataGridViewComboBoxColumn colAction;
    private System.Windows.Forms.DataGridViewLinkColumn colDwgLink;
    private System.Windows.Forms.DataGridViewTextBoxColumn colDwgType;
    private System.Windows.Forms.DataGridViewTextBoxColumn colDwgDesc;
  }
}