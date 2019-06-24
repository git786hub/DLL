namespace ccImportAttachCSV
{
    partial class frmAttachCVS
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.colStructId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCompany = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btValidate = new System.Windows.Forms.Button();
            this.btAddAttactments = new System.Windows.Forms.Button();
            this.lblMsg = new System.Windows.Forms.Label();
            this.btClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colStructId,
            this.colCompany,
            this.colType,
            this.colDate,
            this.colPos,
            this.colStatus});
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.Size = new System.Drawing.Size(543, 177);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // colStructId
            // 
            this.colStructId.HeaderText = "Structure ID";
            this.colStructId.Name = "colStructId";
            // 
            // colCompany
            // 
            this.colCompany.HeaderText = "Company";
            this.colCompany.Name = "colCompany";
            // 
            // colType
            // 
            this.colType.HeaderText = "Type";
            this.colType.Name = "colType";
            // 
            // colDate
            // 
            this.colDate.HeaderText = "Date";
            this.colDate.Name = "colDate";
            // 
            // colPos
            // 
            this.colPos.HeaderText = "Position";
            this.colPos.Name = "colPos";
            // 
            // colStatus
            // 
            this.colStatus.HeaderText = "ProcessStatus";
            this.colStatus.Name = "colStatus";
            // 
            // btValidate
            // 
            this.btValidate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btValidate.Location = new System.Drawing.Point(362, 195);
            this.btValidate.Name = "btValidate";
            this.btValidate.Size = new System.Drawing.Size(75, 23);
            this.btValidate.TabIndex = 1;
            this.btValidate.Text = "Validate";
            this.btValidate.UseVisualStyleBackColor = true;
            this.btValidate.Click += new System.EventHandler(this.btValidate_Click);
            // 
            // btAddAttactments
            // 
            this.btAddAttactments.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btAddAttactments.Enabled = false;
            this.btAddAttactments.Location = new System.Drawing.Point(443, 195);
            this.btAddAttactments.Name = "btAddAttactments";
            this.btAddAttactments.Size = new System.Drawing.Size(112, 23);
            this.btAddAttactments.TabIndex = 2;
            this.btAddAttactments.Text = "Add Attachments";
            this.btAddAttactments.UseVisualStyleBackColor = true;
            this.btAddAttactments.Click += new System.EventHandler(this.btAddAttactments_Click);
            // 
            // lblMsg
            // 
            this.lblMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMsg.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblMsg.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.lblMsg.Location = new System.Drawing.Point(12, 195);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(344, 52);
            this.lblMsg.TabIndex = 3;
            // 
            // btClose
            // 
            this.btClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btClose.Location = new System.Drawing.Point(443, 224);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(112, 23);
            this.btClose.TabIndex = 4;
            this.btClose.Text = "Close and Save";
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // frmAttachCVS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 256);
            this.ControlBox = false;
            this.Controls.Add(this.btClose);
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.btAddAttactments);
            this.Controls.Add(this.btValidate);
            this.Controls.Add(this.dataGridView1);
            this.Name = "frmAttachCVS";
            this.Text = "Attachments CSV";
            this.Activated += new System.EventHandler(this.frmAttachCVS_Activated);
            this.Load += new System.EventHandler(this.frmAttachCVS_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btValidate;
        private System.Windows.Forms.Button btAddAttactments;
        internal System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.Button btClose;
        internal System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStructId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCompany;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPos;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
    }
}