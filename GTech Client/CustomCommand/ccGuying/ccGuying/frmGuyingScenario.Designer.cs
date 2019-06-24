namespace GTechnology.Oncor.CustomAPI
{
    partial class frmGuyingScenario
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
            this.lblPrompt = new System.Windows.Forms.Label();
            this.cboGuyingScenarios = new System.Windows.Forms.ComboBox();
            this.cmdCreateNew = new System.Windows.Forms.Button();
            this.cmdUseExisting = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblPrompt
            // 
            this.lblPrompt.AutoSize = true;
            this.lblPrompt.Location = new System.Drawing.Point(13, 13);
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Size = new System.Drawing.Size(162, 26);
            this.lblPrompt.TabIndex = 0;
            this.lblPrompt.Text = "Use an existing guying scenario, \r\nor create a new one?";
            // 
            // cboGuyingScenarios
            // 
            this.cboGuyingScenarios.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGuyingScenarios.FormattingEnabled = true;
            this.cboGuyingScenarios.Location = new System.Drawing.Point(13, 43);
            this.cboGuyingScenarios.Name = "cboGuyingScenarios";
            this.cboGuyingScenarios.Size = new System.Drawing.Size(121, 21);
            this.cboGuyingScenarios.TabIndex = 1;
            // 
            // cmdCreateNew
            // 
            this.cmdCreateNew.Location = new System.Drawing.Point(13, 70);
            this.cmdCreateNew.Name = "cmdCreateNew";
            this.cmdCreateNew.Size = new System.Drawing.Size(75, 23);
            this.cmdCreateNew.TabIndex = 2;
            this.cmdCreateNew.Text = "Create New";
            this.cmdCreateNew.UseVisualStyleBackColor = true;
            this.cmdCreateNew.Click += new System.EventHandler(this.cmdCreateNew_Click);
            // 
            // cmdUseExisting
            // 
            this.cmdUseExisting.Location = new System.Drawing.Point(94, 70);
            this.cmdUseExisting.Name = "cmdUseExisting";
            this.cmdUseExisting.Size = new System.Drawing.Size(75, 23);
            this.cmdUseExisting.TabIndex = 3;
            this.cmdUseExisting.Text = "Use Existing";
            this.cmdUseExisting.UseVisualStyleBackColor = true;
            this.cmdUseExisting.Click += new System.EventHandler(this.cmdUseExisting_Click);
            // 
            // frmGuyingScenario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(181, 103);
            this.Controls.Add(this.cmdUseExisting);
            this.Controls.Add(this.cmdCreateNew);
            this.Controls.Add(this.cboGuyingScenarios);
            this.Controls.Add(this.lblPrompt);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmGuyingScenario";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Guying Scenario";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPrompt;
        private System.Windows.Forms.Button cmdCreateNew;
        private System.Windows.Forms.Button cmdUseExisting;
        public System.Windows.Forms.ComboBox cboGuyingScenarios;
    }
}