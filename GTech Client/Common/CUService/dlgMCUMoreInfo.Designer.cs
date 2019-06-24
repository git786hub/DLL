namespace GTechnology.Oncor.CustomAPI
{
    partial class dlgMCUMoreInfo
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
            this.lblStaticMUID = new System.Windows.Forms.Label();
            this.lblDynamicMUID = new System.Windows.Forms.Label();
            this.lblStaticEffectiveDate = new System.Windows.Forms.Label();
            this.lblStaticExpirationDate = new System.Windows.Forms.Label();
            this.lblDynamicEffectiveDate = new System.Windows.Forms.Label();
            this.lblDynamicExpirationDate = new System.Windows.Forms.Label();
            this.lblDynamicDescription = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.grdData = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).BeginInit();
            this.SuspendLayout();
            // 
            // lblStaticMUID
            // 
            this.lblStaticMUID.AutoSize = true;
            this.lblStaticMUID.Location = new System.Drawing.Point(16, 14);
            this.lblStaticMUID.Name = "lblStaticMUID";
            this.lblStaticMUID.Size = new System.Drawing.Size(50, 17);
            this.lblStaticMUID.TabIndex = 0;
            this.lblStaticMUID.Text = "MU ID:";
            // 
            // lblDynamicMUID
            // 
            this.lblDynamicMUID.AutoSize = true;
            this.lblDynamicMUID.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDynamicMUID.Location = new System.Drawing.Point(135, 4);
            this.lblDynamicMUID.Name = "lblDynamicMUID";
            this.lblDynamicMUID.Size = new System.Drawing.Size(85, 29);
            this.lblDynamicMUID.TabIndex = 1;
            this.lblDynamicMUID.Text = "label2";
            // 
            // lblStaticEffectiveDate
            // 
            this.lblStaticEffectiveDate.AutoSize = true;
            this.lblStaticEffectiveDate.Location = new System.Drawing.Point(12, 48);
            this.lblStaticEffectiveDate.Name = "lblStaticEffectiveDate";
            this.lblStaticEffectiveDate.Size = new System.Drawing.Size(100, 17);
            this.lblStaticEffectiveDate.TabIndex = 4;
            this.lblStaticEffectiveDate.Text = "Effective Date:";
            // 
            // lblStaticExpirationDate
            // 
            this.lblStaticExpirationDate.AutoSize = true;
            this.lblStaticExpirationDate.Location = new System.Drawing.Point(12, 75);
            this.lblStaticExpirationDate.Name = "lblStaticExpirationDate";
            this.lblStaticExpirationDate.Size = new System.Drawing.Size(108, 17);
            this.lblStaticExpirationDate.TabIndex = 5;
            this.lblStaticExpirationDate.Text = "Expiration Date:";
            // 
            // lblDynamicEffectiveDate
            // 
            this.lblDynamicEffectiveDate.AutoSize = true;
            this.lblDynamicEffectiveDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDynamicEffectiveDate.Location = new System.Drawing.Point(136, 48);
            this.lblDynamicEffectiveDate.Name = "lblDynamicEffectiveDate";
            this.lblDynamicEffectiveDate.Size = new System.Drawing.Size(66, 24);
            this.lblDynamicEffectiveDate.TabIndex = 6;
            this.lblDynamicEffectiveDate.Text = "label7";
            // 
            // lblDynamicExpirationDate
            // 
            this.lblDynamicExpirationDate.AutoSize = true;
            this.lblDynamicExpirationDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDynamicExpirationDate.Location = new System.Drawing.Point(136, 75);
            this.lblDynamicExpirationDate.Name = "lblDynamicExpirationDate";
            this.lblDynamicExpirationDate.Size = new System.Drawing.Size(66, 24);
            this.lblDynamicExpirationDate.TabIndex = 7;
            this.lblDynamicExpirationDate.Text = "label8";
            // 
            // lblDynamicDescription
            // 
            this.lblDynamicDescription.AutoSize = true;
            this.lblDynamicDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDynamicDescription.Location = new System.Drawing.Point(347, 9);
            this.lblDynamicDescription.Name = "lblDynamicDescription";
            this.lblDynamicDescription.Size = new System.Drawing.Size(60, 24);
            this.lblDynamicDescription.TabIndex = 8;
            this.lblDynamicDescription.Text = "label9";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(680, 300);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // grdData
            // 
            this.grdData.AllowUserToAddRows = false;
            this.grdData.AllowUserToDeleteRows = false;
            this.grdData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdData.Location = new System.Drawing.Point(12, 102);
            this.grdData.MinimumSize = new System.Drawing.Size(743, 192);
            this.grdData.Name = "grdData";
            this.grdData.ReadOnly = true;
            this.grdData.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.grdData.RowTemplate.Height = 24;
            this.grdData.Size = new System.Drawing.Size(743, 192);
            this.grdData.TabIndex = 10;
            // 
            // dlgMCUMoreInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(761, 325);
            this.Controls.Add(this.grdData);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblDynamicDescription);
            this.Controls.Add(this.lblDynamicExpirationDate);
            this.Controls.Add(this.lblDynamicEffectiveDate);
            this.Controls.Add(this.lblStaticExpirationDate);
            this.Controls.Add(this.lblStaticEffectiveDate);
            this.Controls.Add(this.lblDynamicMUID);
            this.Controls.Add(this.lblStaticMUID);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(779, 372);
            this.Name = "dlgMCUMoreInfo";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Macro CU Information";
            this.Load += new System.EventHandler(this.MCUMoreInfoForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStaticMUID;
        private System.Windows.Forms.Label lblDynamicMUID;
        private System.Windows.Forms.Label lblStaticEffectiveDate;
        private System.Windows.Forms.Label lblStaticExpirationDate;
        private System.Windows.Forms.Label lblDynamicEffectiveDate;
        private System.Windows.Forms.Label lblDynamicExpirationDate;
        private System.Windows.Forms.Label lblDynamicDescription;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView grdData;

    }
}