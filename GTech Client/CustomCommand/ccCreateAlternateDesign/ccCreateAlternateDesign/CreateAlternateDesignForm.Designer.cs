namespace GTechnology.Oncor.CustomAPI
{
    partial class frmCreateAlternateDesign
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblWorkRequest = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblNewAlternate = new System.Windows.Forms.Label();
            this.chkCopyJobEdits = new System.Windows.Forms.CheckBox();
            this.jobRenamedLabel = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.newDescriptionTextbox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Work Request #:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 43);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Description:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 97);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "New Alternate:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 132);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(114, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "New Description:";
            // 
            // lblWorkRequest
            // 
            this.lblWorkRequest.AutoSize = true;
            this.lblWorkRequest.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWorkRequest.Location = new System.Drawing.Point(143, 11);
            this.lblWorkRequest.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWorkRequest.Name = "lblWorkRequest";
            this.lblWorkRequest.Size = new System.Drawing.Size(52, 17);
            this.lblWorkRequest.TabIndex = 5;
            this.lblWorkRequest.Text = "label6";
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription.Location = new System.Drawing.Point(143, 43);
            this.lblDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(52, 17);
            this.lblDescription.TabIndex = 6;
            this.lblDescription.Text = "label7";
            // 
            // lblNewAlternate
            // 
            this.lblNewAlternate.AutoSize = true;
            this.lblNewAlternate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNewAlternate.Location = new System.Drawing.Point(143, 97);
            this.lblNewAlternate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNewAlternate.Name = "lblNewAlternate";
            this.lblNewAlternate.Size = new System.Drawing.Size(52, 17);
            this.lblNewAlternate.TabIndex = 7;
            this.lblNewAlternate.Text = "label8";
            // 
            // chkCopyJobEdits
            // 
            this.chkCopyJobEdits.AutoSize = true;
            this.chkCopyJobEdits.Location = new System.Drawing.Point(477, 10);
            this.chkCopyJobEdits.Margin = new System.Windows.Forms.Padding(4);
            this.chkCopyJobEdits.Name = "chkCopyJobEdits";
            this.chkCopyJobEdits.Size = new System.Drawing.Size(124, 21);
            this.chkCopyJobEdits.TabIndex = 9;
            this.chkCopyJobEdits.Text = "Copy Job Edits";
            this.chkCopyJobEdits.UseVisualStyleBackColor = true;
            // 
            // jobRenamedLabel
            // 
            this.jobRenamedLabel.AutoSize = true;
            this.jobRenamedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.jobRenamedLabel.ForeColor = System.Drawing.Color.Red;
            this.jobRenamedLabel.Location = new System.Drawing.Point(225, 97);
            this.jobRenamedLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.jobRenamedLabel.Name = "jobRenamedLabel";
            this.jobRenamedLabel.Size = new System.Drawing.Size(282, 17);
            this.jobRenamedLabel.TabIndex = 10;
            this.jobRenamedLabel.Text = "(Current job will be renamed as alternate A)";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(477, 160);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 28);
            this.btnOK.TabIndex = 11;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(585, 160);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 28);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // newDescriptionTextbox
            // 
            this.newDescriptionTextbox.Location = new System.Drawing.Point(147, 128);
            this.newDescriptionTextbox.Margin = new System.Windows.Forms.Padding(4);
            this.newDescriptionTextbox.MaxLength = 60;
            this.newDescriptionTextbox.Name = "newDescriptionTextbox";
            this.newDescriptionTextbox.Size = new System.Drawing.Size(537, 22);
            this.newDescriptionTextbox.TabIndex = 13;
            // 
            // frmCreateAlternateDesign
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(695, 197);
            this.Controls.Add(this.newDescriptionTextbox);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.jobRenamedLabel);
            this.Controls.Add(this.chkCopyJobEdits);
            this.Controls.Add(this.lblNewAlternate);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblWorkRequest);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCreateAlternateDesign";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create Alternate Design";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCreateAlternateDesign_FormClosing);
            this.Load += new System.EventHandler(this.frmCreateAlternateDesign_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblWorkRequest;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblNewAlternate;
        private System.Windows.Forms.CheckBox chkCopyJobEdits;
        private System.Windows.Forms.Label jobRenamedLabel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox newDescriptionTextbox;
    }
}