namespace GTechnology.Oncor.CustomAPI
{
    partial class frmSelectCU
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
            this.dgvCUs = new System.Windows.Forms.DataGridView();
            this.colCU = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCUs)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvCUs
            // 
            this.dgvCUs.AllowUserToAddRows = false;
            this.dgvCUs.AllowUserToDeleteRows = false;
            this.dgvCUs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCUs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCUs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCU,
            this.colDescription});
            this.dgvCUs.Location = new System.Drawing.Point(13, 13);
            this.dgvCUs.MultiSelect = false;
            this.dgvCUs.Name = "dgvCUs";
            this.dgvCUs.ReadOnly = true;
            this.dgvCUs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCUs.Size = new System.Drawing.Size(364, 197);
            this.dgvCUs.TabIndex = 0;
            this.dgvCUs.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCUs_CellContentDoubleClick);
            this.dgvCUs.SelectionChanged += new System.EventHandler(this.dgvCUs_SelectionChanged);
            // 
            // colCU
            // 
            this.colCU.HeaderText = "CU";
            this.colCU.Name = "colCU";
            this.colCU.ReadOnly = true;
            // 
            // colDescription
            // 
            this.colDescription.HeaderText = "Description";
            this.colDescription.Name = "colDescription";
            this.colDescription.ReadOnly = true;
            this.colDescription.Width = 200;
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.Location = new System.Drawing.Point(221, 216);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 1;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.Location = new System.Drawing.Point(302, 216);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 2;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // frmSelectCU
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 246);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.dgvCUs);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSelectCU";
            this.Text = "CU Selection";
            ((System.ComponentModel.ISupportInitialize)(this.dgvCUs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvCUs;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCU;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
    }
}