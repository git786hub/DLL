namespace GTechnology.Oncor.CustomAPI
{
    partial class DetectOverlappingAnalysis
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblFeatureType = new System.Windows.Forms.Label();
            this.cmbBoxFeatureType = new System.Windows.Forms.ComboBox();
            this.rdBtnYes = new System.Windows.Forms.RadioButton();
            this.rdBtnNo = new System.Windows.Forms.RadioButton();
            this.lblOverlaps = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Controls.Add(this.lblOverlaps);
            this.panel1.Controls.Add(this.rdBtnNo);
            this.panel1.Controls.Add(this.rdBtnYes);
            this.panel1.Controls.Add(this.cmbBoxFeatureType);
            this.panel1.Controls.Add(this.lblFeatureType);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(279, 122);
            this.panel1.TabIndex = 0;
            // 
            // lblFeatureType
            // 
            this.lblFeatureType.AutoSize = true;
            this.lblFeatureType.Location = new System.Drawing.Point(15, 18);
            this.lblFeatureType.Name = "lblFeatureType";
            this.lblFeatureType.Size = new System.Drawing.Size(70, 13);
            this.lblFeatureType.TabIndex = 0;
            this.lblFeatureType.Text = "Feature Type";
            // 
            // cmbBoxFeatureType
            // 
            this.cmbBoxFeatureType.FormattingEnabled = true;
            this.cmbBoxFeatureType.Location = new System.Drawing.Point(92, 18);
            this.cmbBoxFeatureType.Name = "cmbBoxFeatureType";
            this.cmbBoxFeatureType.Size = new System.Drawing.Size(174, 21);
            this.cmbBoxFeatureType.TabIndex = 1;
            // 
            // rdBtnYes
            // 
            this.rdBtnYes.AutoSize = true;
            this.rdBtnYes.Location = new System.Drawing.Point(92, 56);
            this.rdBtnYes.Name = "rdBtnYes";
            this.rdBtnYes.Size = new System.Drawing.Size(43, 17);
            this.rdBtnYes.TabIndex = 2;
            this.rdBtnYes.TabStop = true;
            this.rdBtnYes.Text = "Yes";
            this.rdBtnYes.UseVisualStyleBackColor = true;
            this.rdBtnYes.CheckedChanged += new System.EventHandler(this.rdBtnYes_CheckedChanged);
            // 
            // rdBtnNo
            // 
            this.rdBtnNo.AutoSize = true;
            this.rdBtnNo.Location = new System.Drawing.Point(141, 56);
            this.rdBtnNo.Name = "rdBtnNo";
            this.rdBtnNo.Size = new System.Drawing.Size(39, 17);
            this.rdBtnNo.TabIndex = 3;
            this.rdBtnNo.TabStop = true;
            this.rdBtnNo.Text = "No";
            this.rdBtnNo.UseVisualStyleBackColor = true;
            this.rdBtnNo.CheckedChanged += new System.EventHandler(this.rdBtnNo_CheckedChanged);
            // 
            // lblOverlaps
            // 
            this.lblOverlaps.AutoSize = true;
            this.lblOverlaps.Location = new System.Drawing.Point(15, 58);
            this.lblOverlaps.Name = "lblOverlaps";
            this.lblOverlaps.Size = new System.Drawing.Size(70, 13);
            this.lblOverlaps.TabIndex = 4;
            this.lblOverlaps.Text = "Self Overlaps";
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(60, 92);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(141, 92);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // DetectOverlappingAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(301, 143);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DetectOverlappingAnalysis";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Detect Overlapping Analysis";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lblOverlaps;
        private System.Windows.Forms.RadioButton rdBtnNo;
        private System.Windows.Forms.RadioButton rdBtnYes;
        private System.Windows.Forms.ComboBox cmbBoxFeatureType;
        private System.Windows.Forms.Label lblFeatureType;
    }
}