namespace GTechnology.Oncor.CustomAPI
{
	partial class ConstructionRedlines
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
			if (disposing && (components != null))
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
      this.grdRedlineAttachments = new System.Windows.Forms.DataGridView();
      this.btnDisplay = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.lblNoRedlines = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.grdRedlineAttachments)).BeginInit();
      this.SuspendLayout();
      // 
      // grdRedlineAttachments
      // 
      this.grdRedlineAttachments.AllowUserToAddRows = false;
      this.grdRedlineAttachments.AllowUserToDeleteRows = false;
      this.grdRedlineAttachments.AllowUserToOrderColumns = true;
      this.grdRedlineAttachments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.grdRedlineAttachments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.grdRedlineAttachments.Location = new System.Drawing.Point(5, 3);
      this.grdRedlineAttachments.MultiSelect = false;
      this.grdRedlineAttachments.Name = "grdRedlineAttachments";
      this.grdRedlineAttachments.Size = new System.Drawing.Size(350, 164);
      this.grdRedlineAttachments.TabIndex = 0;
      this.grdRedlineAttachments.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdRedlineAttachments_CellClick);
      this.grdRedlineAttachments.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.grdRedlineAttachments_DataBindingComplete);
      // 
      // btnDisplay
      // 
      this.btnDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnDisplay.Enabled = false;
      this.btnDisplay.Location = new System.Drawing.Point(199, 173);
      this.btnDisplay.Name = "btnDisplay";
      this.btnDisplay.Size = new System.Drawing.Size(75, 23);
      this.btnDisplay.TabIndex = 1;
      this.btnDisplay.Text = "Display";
      this.btnDisplay.UseVisualStyleBackColor = true;
      this.btnDisplay.Click += new System.EventHandler(this.btnDisplay_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.Location = new System.Drawing.Point(280, 173);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      // 
      // lblNoRedlines
      // 
      this.lblNoRedlines.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblNoRedlines.AutoSize = true;
      this.lblNoRedlines.Location = new System.Drawing.Point(12, 173);
      this.lblNoRedlines.Name = "lblNoRedlines";
      this.lblNoRedlines.Size = new System.Drawing.Size(72, 13);
      this.lblNoRedlines.TabIndex = 3;
      this.lblNoRedlines.Text = "lblNoRedlines";
      // 
      // ConstructionRedlines
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(359, 209);
      this.Controls.Add(this.lblNoRedlines);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnDisplay);
      this.Controls.Add(this.grdRedlineAttachments);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(375, 247);
      this.Name = "ConstructionRedlines";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Construction Redlines";
      this.Load += new System.EventHandler(this.ConstructionRedlines_Load);
      this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ConstructionRedlines_KeyDown);
      ((System.ComponentModel.ISupportInitialize)(this.grdRedlineAttachments)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView grdRedlineAttachments;
		private System.Windows.Forms.Button btnDisplay;
		private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Label lblNoRedlines;
  }
}