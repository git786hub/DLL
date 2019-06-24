namespace ccImportAttachCSV
{
    partial class frmSaveCSV
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
            this.sfdCSV = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // sfdCSV
            // 
            this.sfdCSV.FileName = "CSV File (*.CSV)|*.CSV|Text File (*.txt)|*.txt";
            // 
            // frmSaveCSV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(17F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 622);
            this.ControlBox = false;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Name = "frmSaveCSV";
            this.Text = "Save As";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SaveFileDialog sfdCSV;
    }
}