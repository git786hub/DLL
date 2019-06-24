namespace GTechnology.Oncor.CustomAPI.View
{
    partial class AgreementPlotForm
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
            this.grpBox = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.txtPlotName = new System.Windows.Forms.TextBox();
            this.txtSubdivision = new System.Windows.Forms.TextBox();
            this.txtCity = new System.Windows.Forms.TextBox();
            this.lblPlotName = new System.Windows.Forms.Label();
            this.lblSubdivision = new System.Windows.Forms.Label();
            this.lblCity = new System.Windows.Forms.Label();
            this.grpBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpBox
            // 
            this.grpBox.Controls.Add(this.btnCancel);
            this.grpBox.Controls.Add(this.btnOk);
            this.grpBox.Controls.Add(this.txtPlotName);
            this.grpBox.Controls.Add(this.txtSubdivision);
            this.grpBox.Controls.Add(this.txtCity);
            this.grpBox.Controls.Add(this.lblPlotName);
            this.grpBox.Controls.Add(this.lblSubdivision);
            this.grpBox.Controls.Add(this.lblCity);
            this.grpBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBox.Location = new System.Drawing.Point(0, 0);
            this.grpBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpBox.Name = "grpBox";
            this.grpBox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpBox.Size = new System.Drawing.Size(413, 145);
            this.grpBox.TabIndex = 0;
            this.grpBox.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(192, 110);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 28);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(300, 110);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 28);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // txtPlotName
            // 
            this.txtPlotName.Location = new System.Drawing.Point(105, 80);
            this.txtPlotName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtPlotName.Name = "txtPlotName";
            this.txtPlotName.Size = new System.Drawing.Size(295, 22);
            this.txtPlotName.TabIndex = 5;
            // 
            // txtSubdivision
            // 
            this.txtSubdivision.Location = new System.Drawing.Point(105, 48);
            this.txtSubdivision.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSubdivision.Name = "txtSubdivision";
            this.txtSubdivision.Size = new System.Drawing.Size(295, 22);
            this.txtSubdivision.TabIndex = 4;
            // 
            // txtCity
            // 
            this.txtCity.Location = new System.Drawing.Point(105, 17);
            this.txtCity.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(295, 22);
            this.txtCity.TabIndex = 3;
            // 
            // lblPlotName
            // 
            this.lblPlotName.AutoSize = true;
            this.lblPlotName.Location = new System.Drawing.Point(26, 80);
            this.lblPlotName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPlotName.Name = "lblPlotName";
            this.lblPlotName.Size = new System.Drawing.Size(71, 16);
            this.lblPlotName.TabIndex = 2;
            this.lblPlotName.Text = "Plot Name";
            // 
            // lblSubdivision
            // 
            this.lblSubdivision.AutoSize = true;
            this.lblSubdivision.Location = new System.Drawing.Point(19, 48);
            this.lblSubdivision.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSubdivision.Name = "lblSubdivision";
            this.lblSubdivision.Size = new System.Drawing.Size(78, 16);
            this.lblSubdivision.TabIndex = 1;
            this.lblSubdivision.Text = "Subdivision";
            // 
            // lblCity
            // 
            this.lblCity.AutoSize = true;
            this.lblCity.Location = new System.Drawing.Point(67, 20);
            this.lblCity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(30, 16);
            this.lblCity.TabIndex = 0;
            this.lblCity.Text = "City";
            // 
            // AgreementPlotForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(413, 145);
            this.Controls.Add(this.grpBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AgreementPlotForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Street Light Supplemental Plot";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AgreementPlotForm_FormClosing);
            this.grpBox.ResumeLayout(false);
            this.grpBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBox;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TextBox txtPlotName;
        private System.Windows.Forms.TextBox txtSubdivision;
        private System.Windows.Forms.TextBox txtCity;
        private System.Windows.Forms.Label lblPlotName;
        private System.Windows.Forms.Label lblSubdivision;
        private System.Windows.Forms.Label lblCity;
    }
}