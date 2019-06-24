namespace OncDocManage
{
    partial class frmSPFileToCopy
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
            this.btOk = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.dgvFilesToCopy = new System.Windows.Forms.DataGridView();
            this.colFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDocType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilesToCopy)).BeginInit();
            this.SuspendLayout();
            // 
            // btOk
            // 
            this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOk.Enabled = false;
            this.btOk.Location = new System.Drawing.Point(308, 168);
            this.btOk.Margin = new System.Windows.Forms.Padding(4);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(100, 28);
            this.btOk.TabIndex = 0;
            this.btOk.Text = "OK";
            this.btOk.UseVisualStyleBackColor = true;
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(416, 168);
            this.btCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(100, 28);
            this.btCancel.TabIndex = 2;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // dgvFilesToCopy
            // 
            this.dgvFilesToCopy.AllowUserToAddRows = false;
            this.dgvFilesToCopy.AllowUserToDeleteRows = false;
            this.dgvFilesToCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvFilesToCopy.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFilesToCopy.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFile,
            this.colDocType});
            this.dgvFilesToCopy.Location = new System.Drawing.Point(12, 12);
            this.dgvFilesToCopy.MultiSelect = false;
            this.dgvFilesToCopy.Name = "dgvFilesToCopy";
            this.dgvFilesToCopy.ReadOnly = true;
            this.dgvFilesToCopy.RowHeadersVisible = false;
            this.dgvFilesToCopy.RowTemplate.Height = 24;
            this.dgvFilesToCopy.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFilesToCopy.Size = new System.Drawing.Size(504, 149);
            this.dgvFilesToCopy.TabIndex = 1;
            this.dgvFilesToCopy.SelectionChanged += new System.EventHandler(this.dgvFilesToCopy_SelectionChanged);
            // 
            // colFile
            // 
            this.colFile.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colFile.HeaderText = "File";
            this.colFile.MinimumWidth = 200;
            this.colFile.Name = "colFile";
            this.colFile.ReadOnly = true;
            // 
            // colDocType
            // 
            this.colDocType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colDocType.HeaderText = "Description";
            this.colDocType.MinimumWidth = 200;
            this.colDocType.Name = "colDocType";
            this.colDocType.ReadOnly = true;
            this.colDocType.Width = 300;
            // 
            // frmSPFileToCopy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(529, 204);
            this.ControlBox = false;
            this.Controls.Add(this.dgvFilesToCopy);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOk);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmSPFileToCopy";
            this.Text = "File to Copy";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilesToCopy)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.DataGridView dgvFilesToCopy;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFile;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDocType;
    }
}