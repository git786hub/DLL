namespace GTechnology.Oncor.CustomAPI
{
    partial class EmbeddedDT
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
            this.cmdPrintReport = new System.Windows.Forms.Button();
            this.cmdSaveReport = new System.Windows.Forms.Button();
            this.cmdClose = new System.Windows.Forms.Button();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // cmdPrintReport
            // 
            this.cmdPrintReport.Location = new System.Drawing.Point(13, 13);
            this.cmdPrintReport.Name = "cmdPrintReport";
            this.cmdPrintReport.Size = new System.Drawing.Size(75, 23);
            this.cmdPrintReport.TabIndex = 0;
            this.cmdPrintReport.Text = "Print Report";
            this.cmdPrintReport.UseVisualStyleBackColor = true;
            this.cmdPrintReport.Click += new System.EventHandler(this.cmdPrintReport_Click);
            // 
            // cmdSaveReport
            // 
            this.cmdSaveReport.Location = new System.Drawing.Point(94, 13);
            this.cmdSaveReport.Name = "cmdSaveReport";
            this.cmdSaveReport.Size = new System.Drawing.Size(75, 23);
            this.cmdSaveReport.TabIndex = 1;
            this.cmdSaveReport.Text = "Save Report";
            this.cmdSaveReport.UseVisualStyleBackColor = true;
            this.cmdSaveReport.Click += new System.EventHandler(this.cmdSaveReport_Click);
            // 
            // cmdClose
            // 
            this.cmdClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdClose.Location = new System.Drawing.Point(837, 12);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(75, 23);
            this.cmdClose.TabIndex = 2;
            this.cmdClose.Text = "Close";
            this.cmdClose.UseVisualStyleBackColor = true;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser1.Location = new System.Drawing.Point(13, 42);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(899, 456);
            this.webBrowser1.TabIndex = 0;
            this.webBrowser1.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.webBrowser1_Navigated);
            // 
            // EmbeddedDT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(924, 510);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.cmdSaveReport);
            this.Controls.Add(this.cmdPrintReport);
            this.Name = "EmbeddedDT";
            this.Text = "<set by calling command>";
            this.Load += new System.EventHandler(this.EmbeddedDT_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.WebBrowser webBrowser1;
        public System.Windows.Forms.Button cmdClose;
        private System.Windows.Forms.Button cmdPrintReport;
        public System.Windows.Forms.Button cmdSaveReport;
    }
}