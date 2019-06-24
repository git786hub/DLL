namespace GTechnology.Oncor.CustomAPI
{
    partial class VoucherFERCAccountForm
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
            this.gridVoucherFERCAcct = new System.Windows.Forms.DataGridView();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridVoucherFERCAcct)).BeginInit();
            this.SuspendLayout();
            // 
            // gridVoucherFERCAcct
            // 
            this.gridVoucherFERCAcct.AllowUserToAddRows = false;
            this.gridVoucherFERCAcct.AllowUserToDeleteRows = false;
            this.gridVoucherFERCAcct.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridVoucherFERCAcct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridVoucherFERCAcct.Location = new System.Drawing.Point(12, 12);
            this.gridVoucherFERCAcct.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gridVoucherFERCAcct.MultiSelect = false;
            this.gridVoucherFERCAcct.Name = "gridVoucherFERCAcct";
            this.gridVoucherFERCAcct.RowTemplate.Height = 24;
            this.gridVoucherFERCAcct.Size = new System.Drawing.Size(745, 232);
            this.gridVoucherFERCAcct.TabIndex = 0;
            this.gridVoucherFERCAcct.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridVoucherFERCAcct_CellClick);
            this.gridVoucherFERCAcct.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gridVoucherFERCAcct_RowHeaderMouseClick);
            this.gridVoucherFERCAcct.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gridVoucherFERCAcct_RowHeaderMouseDoubleClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(657, 264);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 28);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(552, 264);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 28);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // VoucherFERCAccountForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(772, 305);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.gridVoucherFERCAcct);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(790, 308);
            this.Name = "VoucherFERCAccountForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Voucher FERC Account";
            this.Load += new System.EventHandler(this.VoucherFERCAccountForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridVoucherFERCAcct)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.DataGridView gridVoucherFERCAcct;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
    }
}