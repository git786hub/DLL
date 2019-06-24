namespace GTechnology.Oncor.CustomAPI
{
    partial class dlgChangeOut
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
        private void InitializeComponent()
        {
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.rdoRemovalActivityRemove = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoRemovalActivitySalvage = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdoInstallActivityDoNotInstall = new System.Windows.Forms.RadioButton();
            this.rdoInstallActivityReplaceWithSame = new System.Windows.Forms.RadioButton();
            this.rdoInstallActivitySelect = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(248, 125);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(329, 125);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // rdoRemovalActivityRemove
            // 
            this.rdoRemovalActivityRemove.AutoSize = true;
            this.rdoRemovalActivityRemove.Checked = true;
            this.rdoRemovalActivityRemove.Location = new System.Drawing.Point(6, 21);
            this.rdoRemovalActivityRemove.Name = "rdoRemovalActivityRemove";
            this.rdoRemovalActivityRemove.Size = new System.Drawing.Size(81, 21);
            this.rdoRemovalActivityRemove.TabIndex = 2;
            this.rdoRemovalActivityRemove.TabStop = true;
            this.rdoRemovalActivityRemove.Text = "Remove";
            this.rdoRemovalActivityRemove.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoRemovalActivitySalvage);
            this.groupBox1.Controls.Add(this.rdoRemovalActivityRemove);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 107);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Removal Activity";
            // 
            // rdoRemovalActivitySalvage
            // 
            this.rdoRemovalActivitySalvage.AutoSize = true;
            this.rdoRemovalActivitySalvage.Location = new System.Drawing.Point(7, 49);
            this.rdoRemovalActivitySalvage.Name = "rdoRemovalActivitySalvage";
            this.rdoRemovalActivitySalvage.Size = new System.Drawing.Size(80, 21);
            this.rdoRemovalActivitySalvage.TabIndex = 3;
            this.rdoRemovalActivitySalvage.Text = "Salvage";
            this.rdoRemovalActivitySalvage.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdoInstallActivityDoNotInstall);
            this.groupBox2.Controls.Add(this.rdoInstallActivityReplaceWithSame);
            this.groupBox2.Controls.Add(this.rdoInstallActivitySelect);
            this.groupBox2.Location = new System.Drawing.Point(218, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(186, 107);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Install Activity";
            // 
            // rdoInstallActivityDoNotInstall
            // 
            this.rdoInstallActivityDoNotInstall.AutoSize = true;
            this.rdoInstallActivityDoNotInstall.Location = new System.Drawing.Point(7, 76);
            this.rdoInstallActivityDoNotInstall.Name = "rdoInstallActivityDoNotInstall";
            this.rdoInstallActivityDoNotInstall.Size = new System.Drawing.Size(111, 21);
            this.rdoInstallActivityDoNotInstall.TabIndex = 2;
            this.rdoInstallActivityDoNotInstall.TabStop = true;
            this.rdoInstallActivityDoNotInstall.Text = "Do not install";
            this.rdoInstallActivityDoNotInstall.UseVisualStyleBackColor = true;
            // 
            // rdoInstallActivityReplaceWithSame
            // 
            this.rdoInstallActivityReplaceWithSame.AutoSize = true;
            this.rdoInstallActivityReplaceWithSame.Location = new System.Drawing.Point(7, 48);
            this.rdoInstallActivityReplaceWithSame.Name = "rdoInstallActivityReplaceWithSame";
            this.rdoInstallActivityReplaceWithSame.Size = new System.Drawing.Size(170, 21);
            this.rdoInstallActivityReplaceWithSame.TabIndex = 1;
            this.rdoInstallActivityReplaceWithSame.TabStop = true;
            this.rdoInstallActivityReplaceWithSame.Text = "Replace with same CU";
            this.rdoInstallActivityReplaceWithSame.UseVisualStyleBackColor = true;
            // 
            // rdoInstallActivitySelect
            // 
            this.rdoInstallActivitySelect.AutoSize = true;
            this.rdoInstallActivitySelect.Checked = true;
            this.rdoInstallActivitySelect.Location = new System.Drawing.Point(7, 21);
            this.rdoInstallActivitySelect.Name = "rdoInstallActivitySelect";
            this.rdoInstallActivitySelect.Size = new System.Drawing.Size(173, 21);
            this.rdoInstallActivitySelect.TabIndex = 0;
            this.rdoInstallActivitySelect.TabStop = true;
            this.rdoInstallActivitySelect.Text = "Select replacement CU";
            this.rdoInstallActivitySelect.UseVisualStyleBackColor = true;
            // 
            // dlgChangeOut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(417, 160);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dlgChangeOut";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Changeout";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }
#endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.RadioButton rdoRemovalActivityRemove;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoRemovalActivitySalvage;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdoInstallActivityDoNotInstall;
        private System.Windows.Forms.RadioButton rdoInstallActivityReplaceWithSame;
        private System.Windows.Forms.RadioButton rdoInstallActivitySelect;
    }
}