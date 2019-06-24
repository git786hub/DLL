namespace testApp2ccImportAttachCSV
{
    partial class frmOpenCsv
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
            this.ofdCsv = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // ofdCsv
            // 
            this.ofdCsv.AutoUpgradeEnabled = false;
            this.ofdCsv.DefaultExt = "*.csv";
            this.ofdCsv.FileName = "*.csv";
            this.ofdCsv.Filter = "CSV File (*.CSV)|*.CSV|Text File (*.txt)|*.txt";
            this.ofdCsv.InitialDirectory = "d:\\";
            this.ofdCsv.ShowReadOnly = true;
            this.ofdCsv.Title = "Open Attachment CSV";
            // 
            // frmOpenCsv
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.ControlBox = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOpenCsv";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "frmOpenCsv";
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.OpenFileDialog ofdCsv;

    }
}