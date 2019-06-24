namespace GTechnology.Oncor.CustomAPI
{
    partial class frmCopyAncillaryCUs
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
            this.grdACUs = new System.Windows.Forms.DataGridView();
            this.cbxDeleteExistingACUs = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grdACUs)).BeginInit();
            this.SuspendLayout();
            // 
            // grdACUs
            // 
            this.grdACUs.AllowUserToAddRows = false;
            this.grdACUs.AllowUserToDeleteRows = false;
            this.grdACUs.AllowUserToOrderColumns = true;
            this.grdACUs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdACUs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdACUs.Location = new System.Drawing.Point(5, 4);
            this.grdACUs.MultiSelect = false;
            this.grdACUs.Name = "grdACUs";
            this.grdACUs.RowHeadersVisible = false;
            this.grdACUs.Size = new System.Drawing.Size(523, 180);
            this.grdACUs.TabIndex = 0;
            this.grdACUs.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.grdACUs_DataBindingComplete);
            // 
            // cbxDeleteExistingACUs
            // 
            this.cbxDeleteExistingACUs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbxDeleteExistingACUs.AutoSize = true;
            this.cbxDeleteExistingACUs.Location = new System.Drawing.Point(12, 197);
            this.cbxDeleteExistingACUs.Name = "cbxDeleteExistingACUs";
            this.cbxDeleteExistingACUs.Size = new System.Drawing.Size(161, 17);
            this.cbxDeleteExistingACUs.TabIndex = 1;
            this.cbxDeleteExistingACUs.Text = "Delete Existing Ancillary CUs";
            this.cbxDeleteExistingACUs.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(412, 193);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(55, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopy.Location = new System.Drawing.Point(473, 193);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(49, 23);
            this.btnCopy.TabIndex = 3;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // frmCopyAncillaryCUs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 223);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.cbxDeleteExistingACUs);
            this.Controls.Add(this.grdACUs);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(550, 262);
            this.Name = "frmCopyAncillaryCUs";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Copy Ancillary CUs";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.grdACUs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView grdACUs;
        private System.Windows.Forms.CheckBox cbxDeleteExistingACUs;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnCopy;
    }
}