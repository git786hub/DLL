using System.Windows.Forms;

namespace GTechnology.Oncor.CustomAPI
{
    partial class ccReviewVouchersForm
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
            this.gridReviewVouchers = new System.Windows.Forms.DataGridView();
            this.panel = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridReviewVouchers)).BeginInit();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridReviewVouchers
            // 
            this.gridReviewVouchers.AllowUserToAddRows = false;
            this.gridReviewVouchers.AllowUserToDeleteRows = false;
            this.gridReviewVouchers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridReviewVouchers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridReviewVouchers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridReviewVouchers.Location = new System.Drawing.Point(0, 0);
            this.gridReviewVouchers.Margin = new System.Windows.Forms.Padding(2);
            this.gridReviewVouchers.Name = "gridReviewVouchers";
            this.gridReviewVouchers.Size = new System.Drawing.Size(895, 213);
            this.gridReviewVouchers.TabIndex = 1;
            this.gridReviewVouchers.ColumnHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gridReviewVouchers_ColumnHeaderMouseDoubleClick);
            // 
            // panel
            // 
            this.panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel.CausesValidation = false;
            this.panel.Controls.Add(this.gridReviewVouchers);
            this.panel.Location = new System.Drawing.Point(9, 10);
            this.panel.Margin = new System.Windows.Forms.Padding(2);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(895, 213);
            this.panel.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(848, 234);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(56, 19);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ccReviewVouchersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(915, 260);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.panel);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(920, 270);
            this.Name = "ccReviewVouchersForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Review Vouchers";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ccReviewVouchersForm_FormClosed);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ccReviewVouchersForm_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.gridReviewVouchers)).EndInit();
            this.panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        
        #endregion

        private System.Windows.Forms.DataGridView gridReviewVouchers;
        private Panel panel;
        private Button btnClose;
    }
}