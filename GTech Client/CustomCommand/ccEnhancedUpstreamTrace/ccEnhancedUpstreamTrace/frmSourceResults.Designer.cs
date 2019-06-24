namespace GTechnology.Oncor.CustomAPI
{
    partial class frmSourceResults
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
            this.dgvSourceResults = new System.Windows.Forms.DataGridView();
            this.FeatureID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FeatureClass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FeederID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NetworkID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Fno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSourceResults)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvSourceResults
            // 
            this.dgvSourceResults.AllowUserToAddRows = false;
            this.dgvSourceResults.AllowUserToDeleteRows = false;
            this.dgvSourceResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSourceResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSourceResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FeatureID,
            this.FeatureClass,
            this.FeederID,
            this.NetworkID,
            this.Fno});
            this.dgvSourceResults.Location = new System.Drawing.Point(2, 2);
            this.dgvSourceResults.MultiSelect = false;
            this.dgvSourceResults.Name = "dgvSourceResults";
            this.dgvSourceResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSourceResults.Size = new System.Drawing.Size(474, 161);
            this.dgvSourceResults.TabIndex = 0;
            this.dgvSourceResults.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSourceResults_CellClick);
            this.dgvSourceResults.SelectionChanged += new System.EventHandler(this.dgvSourceResults_SelectionChanged);
            // 
            // FeatureID
            // 
            this.FeatureID.HeaderText = "Feature ID";
            this.FeatureID.Name = "FeatureID";
            this.FeatureID.ReadOnly = true;
            this.FeatureID.Width = 80;
            // 
            // FeatureClass
            // 
            this.FeatureClass.HeaderText = "Feature Class";
            this.FeatureClass.Name = "FeatureClass";
            this.FeatureClass.ReadOnly = true;
            this.FeatureClass.Width = 150;
            // 
            // FeederID
            // 
            this.FeederID.HeaderText = "Feeder ID";
            this.FeederID.Name = "FeederID";
            this.FeederID.ReadOnly = true;
            // 
            // NetworkID
            // 
            this.NetworkID.HeaderText = "Network ID";
            this.NetworkID.Name = "NetworkID";
            this.NetworkID.ReadOnly = true;
            // 
            // Fno
            // 
            this.Fno.HeaderText = "Fno";
            this.Fno.Name = "Fno";
            this.Fno.ReadOnly = true;
            this.Fno.Visible = false;
            // 
            // frmSourceResults
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 165);
            this.Controls.Add(this.dgvSourceResults);
            this.Name = "frmSourceResults";
            this.Text = "Enhanced Upstream Trace";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSourceResults_FormClosing);
            this.Load += new System.EventHandler(this.frmSourceResults_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSourceResults)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvSourceResults;
        private System.Windows.Forms.DataGridViewTextBoxColumn FeatureID;
        private System.Windows.Forms.DataGridViewTextBoxColumn FeatureClass;
        private System.Windows.Forms.DataGridViewTextBoxColumn FeederID;
        private System.Windows.Forms.DataGridViewTextBoxColumn NetworkID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Fno;
    }
}