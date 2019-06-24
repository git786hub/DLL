namespace GTechnology.Oncor.CustomAPI
{
    partial class ReviewAssetHistoryForm
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
            this.grbQueryby = new System.Windows.Forms.GroupBox();
            this.txtWr = new System.Windows.Forms.TextBox();
            this.btnGo = new System.Windows.Forms.Button();
            this.lblWR = new System.Windows.Forms.Label();
            this.txtSid = new System.Windows.Forms.TextBox();
            this.txtFid = new System.Windows.Forms.TextBox();
            this.lblSID = new System.Windows.Forms.Label();
            this.lblFid = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.lblNamedView = new System.Windows.Forms.Label();
            this.cmbNamedView = new System.Windows.Forms.ComboBox();
            this.btnSaveView = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.dgHistory = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grbQueryby.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgHistory)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grbQueryby
            // 
            this.grbQueryby.Controls.Add(this.txtWr);
            this.grbQueryby.Controls.Add(this.btnGo);
            this.grbQueryby.Controls.Add(this.lblWR);
            this.grbQueryby.Controls.Add(this.txtSid);
            this.grbQueryby.Controls.Add(this.txtFid);
            this.grbQueryby.Controls.Add(this.lblSID);
            this.grbQueryby.Controls.Add(this.lblFid);
            this.grbQueryby.Location = new System.Drawing.Point(8, 4);
            this.grbQueryby.Name = "grbQueryby";
            this.grbQueryby.Size = new System.Drawing.Size(742, 84);
            this.grbQueryby.TabIndex = 1;
            this.grbQueryby.TabStop = false;
            this.grbQueryby.Text = "Query by:";
            // 
            // txtWr
            // 
            this.txtWr.Location = new System.Drawing.Point(565, 22);
            this.txtWr.Name = "txtWr";
            this.txtWr.Size = new System.Drawing.Size(165, 22);
            this.txtWr.TabIndex = 5;
            this.txtWr.TextChanged += new System.EventHandler(this.txtWr_TextChanged);
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(630, 49);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(100, 25);
            this.btnGo.TabIndex = 6;
            this.btnGo.Text = "Go";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // lblWR
            // 
            this.lblWR.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWR.AutoSize = true;
            this.lblWR.Location = new System.Drawing.Point(449, 22);
            this.lblWR.Name = "lblWR";
            this.lblWR.Size = new System.Drawing.Size(110, 17);
            this.lblWR.TabIndex = 4;
            this.lblWR.Text = "Work Request #";
            // 
            // txtSid
            // 
            this.txtSid.Location = new System.Drawing.Point(215, 50);
            this.txtSid.Name = "txtSid";
            this.txtSid.Size = new System.Drawing.Size(165, 22);
            this.txtSid.TabIndex = 3;
            this.txtSid.TextChanged += new System.EventHandler(this.txtSid_TextChanged);
            // 
            // txtFid
            // 
            this.txtFid.Location = new System.Drawing.Point(215, 22);
            this.txtFid.Name = "txtFid";
            this.txtFid.Size = new System.Drawing.Size(165, 22);
            this.txtFid.TabIndex = 2;
            this.txtFid.TextChanged += new System.EventHandler(this.txtFid_TextChanged);
            // 
            // lblSID
            // 
            this.lblSID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSID.AutoSize = true;
            this.lblSID.Location = new System.Drawing.Point(114, 55);
            this.lblSID.Name = "lblSID";
            this.lblSID.Size = new System.Drawing.Size(83, 17);
            this.lblSID.TabIndex = 1;
            this.lblSID.Text = "Structure ID";
            // 
            // lblFid
            // 
            this.lblFid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFid.AutoSize = true;
            this.lblFid.Location = new System.Drawing.Point(123, 22);
            this.lblFid.Name = "lblFid";
            this.lblFid.Size = new System.Drawing.Size(74, 17);
            this.lblFid.TabIndex = 0;
            this.lblFid.Text = "Feature ID";
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Location = new System.Drawing.Point(1037, 53);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(100, 25);
            this.btnExport.TabIndex = 4;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // lblNamedView
            // 
            this.lblNamedView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNamedView.AutoSize = true;
            this.lblNamedView.Location = new System.Drawing.Point(762, 26);
            this.lblNamedView.Name = "lblNamedView";
            this.lblNamedView.Size = new System.Drawing.Size(86, 17);
            this.lblNamedView.TabIndex = 1;
            this.lblNamedView.Text = "Named View";
            // 
            // cmbNamedView
            // 
            this.cmbNamedView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbNamedView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNamedView.FormattingEnabled = true;
            this.cmbNamedView.Location = new System.Drawing.Point(854, 23);
            this.cmbNamedView.Name = "cmbNamedView";
            this.cmbNamedView.Size = new System.Drawing.Size(283, 24);
            this.cmbNamedView.TabIndex = 2;
            this.cmbNamedView.SelectedIndexChanged += new System.EventHandler(this.cmbNamedView_SelectedIndexChanged);
            this.cmbNamedView.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbNamedView_KeyPress);
            // 
            // btnSaveView
            // 
            this.btnSaveView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveView.Location = new System.Drawing.Point(854, 53);
            this.btnSaveView.Name = "btnSaveView";
            this.btnSaveView.Size = new System.Drawing.Size(100, 25);
            this.btnSaveView.TabIndex = 3;
            this.btnSaveView.Text = "Save View";
            this.btnSaveView.UseVisualStyleBackColor = true;
            this.btnSaveView.Click += new System.EventHandler(this.btnSaveView_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(1041, 591);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 25);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // dgHistory
            // 
            this.dgHistory.AllowUserToAddRows = false;
            this.dgHistory.AllowUserToDeleteRows = false;
            this.dgHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgHistory.Location = new System.Drawing.Point(4, 105);
            this.dgHistory.Name = "dgHistory";
            this.dgHistory.ReadOnly = true;
            this.dgHistory.RowTemplate.Height = 24;
            this.dgHistory.Size = new System.Drawing.Size(1140, 472);
            this.dgHistory.TabIndex = 8;
            this.dgHistory.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgHistory_ColumnHeaderMouseClick);
            this.dgHistory.ColumnHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgHistory_ColumnHeaderMouseDoubleClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Controls.Add(this.grbQueryby);
            this.panel1.Controls.Add(this.btnSaveView);
            this.panel1.Controls.Add(this.lblNamedView);
            this.panel1.Controls.Add(this.cmbNamedView);
            this.panel1.Location = new System.Drawing.Point(4, 8);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1140, 91);
            this.panel1.TabIndex = 10;
            // 
            // ReviewAssetHistoryForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1149, 628);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dgHistory);
            this.Controls.Add(this.btnClose);
            this.ForeColor = System.Drawing.Color.Black;
            this.Margin = new System.Windows.Forms.Padding(45, 22, 45, 22);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1163, 576);
            this.Name = "ReviewAssetHistoryForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Asset History";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ReviewAssetHistoryForm_FormClosing);
            this.Load += new System.EventHandler(this.ReviewAssetHistoryForm_Load);
            this.grbQueryby.ResumeLayout(false);
            this.grbQueryby.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgHistory)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grbQueryby;
        private System.Windows.Forms.TextBox txtWr;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Label lblWR;
        private System.Windows.Forms.TextBox txtSid;
        private System.Windows.Forms.TextBox txtFid;
        private System.Windows.Forms.Label lblSID;
        private System.Windows.Forms.Label lblFid;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Label lblNamedView;
        private System.Windows.Forms.ComboBox cmbNamedView;
        private System.Windows.Forms.Button btnSaveView;
        private System.Windows.Forms.DataGridView dgHistory;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panel1;

        public System.Windows.Forms.DataGridView dgvReviewHistory
        {
            get { return dgHistory; }
        }
    }
}