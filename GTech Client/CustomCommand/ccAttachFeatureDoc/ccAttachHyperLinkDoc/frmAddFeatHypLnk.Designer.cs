namespace GTechnology.Oncor.CustomAPI
{
    partial class frmAddFeatHypLnk
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
            this.ofdGetHypLnkFile = new System.Windows.Forms.OpenFileDialog();
            this.lblFile = new System.Windows.Forms.Label();
            this.txtHyperlinkFile = new System.Windows.Forms.TextBox();
            this.btHyperBrowse = new System.Windows.Forms.Button();
            this.lblDocManageType = new System.Windows.Forms.Label();
            this.cbSPDocTyp = new System.Windows.Forms.ComboBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescr = new System.Windows.Forms.TextBox();
            this.btAttach = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ofdGetHypLnkFile
            // 
            this.ofdGetHypLnkFile.Title = "Select Document";
            // 
            // lblFile
            // 
            this.lblFile.AutoSize = true;
            this.lblFile.Location = new System.Drawing.Point(49, 9);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(26, 13);
            this.lblFile.TabIndex = 0;
            this.lblFile.Text = "File:";
            // 
            // txtHyperlinkFile
            // 
            this.txtHyperlinkFile.Location = new System.Drawing.Point(88, 6);
            this.txtHyperlinkFile.Name = "txtHyperlinkFile";
            this.txtHyperlinkFile.Size = new System.Drawing.Size(325, 20);
            this.txtHyperlinkFile.TabIndex = 1;
            // 
            // btHyperBrowse
            // 
            this.btHyperBrowse.Location = new System.Drawing.Point(419, 4);
            this.btHyperBrowse.Name = "btHyperBrowse";
            this.btHyperBrowse.Size = new System.Drawing.Size(75, 23);
            this.btHyperBrowse.TabIndex = 2;
            this.btHyperBrowse.TabStop = false;
            this.btHyperBrowse.Text = "Select ...";
            this.btHyperBrowse.UseVisualStyleBackColor = true;
            this.btHyperBrowse.Click += new System.EventHandler(this.btHyperBrowse_Click);
            // 
            // lblDocManageType
            // 
            this.lblDocManageType.AutoSize = true;
            this.lblDocManageType.Location = new System.Drawing.Point(41, 43);
            this.lblDocManageType.Name = "lblDocManageType";
            this.lblDocManageType.Size = new System.Drawing.Size(34, 13);
            this.lblDocManageType.TabIndex = 3;
            this.lblDocManageType.Text = "Type:";
            // 
            // cbSPDocTyp
            // 
            this.cbSPDocTyp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSPDocTyp.FormattingEnabled = true;
            this.cbSPDocTyp.Location = new System.Drawing.Point(88, 40);
            this.cbSPDocTyp.Name = "cbSPDocTyp";
            this.cbSPDocTyp.Size = new System.Drawing.Size(196, 21);
            this.cbSPDocTyp.TabIndex = 4;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(12, 73);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(63, 13);
            this.lblDescription.TabIndex = 5;
            this.lblDescription.Text = "Description:";
            // 
            // txtDescr
            // 
            this.txtDescr.Location = new System.Drawing.Point(88, 73);
            this.txtDescr.Name = "txtDescr";
            this.txtDescr.Size = new System.Drawing.Size(325, 20);
            this.txtDescr.TabIndex = 6;
            // 
            // btAttach
            // 
            this.btAttach.Location = new System.Drawing.Point(419, 111);
            this.btAttach.Name = "btAttach";
            this.btAttach.Size = new System.Drawing.Size(75, 23);
            this.btAttach.TabIndex = 7;
            this.btAttach.TabStop = false;
            this.btAttach.Text = "Attach";
            this.btAttach.UseVisualStyleBackColor = true;
            this.btAttach.Click += new System.EventHandler(this.btAttach_Click);
            // 
            // btCancel
            // 
            this.btCancel.Location = new System.Drawing.Point(338, 111);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 8;
            this.btCancel.TabStop = false;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // frmAddFeatHypLnk
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(504, 145);
            this.ControlBox = false;
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btAttach);
            this.Controls.Add(this.txtDescr);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.cbSPDocTyp);
            this.Controls.Add(this.lblDocManageType);
            this.Controls.Add(this.btHyperBrowse);
            this.Controls.Add(this.txtHyperlinkFile);
            this.Controls.Add(this.lblFile);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddFeatHypLnk";
            this.Text = "Attach Feature Document";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmAddFeatHypLnk_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.OpenFileDialog ofdGetHypLnkFile;
        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.TextBox txtHyperlinkFile;
        private System.Windows.Forms.Button btHyperBrowse;
        private System.Windows.Forms.Label lblDocManageType;
        private System.Windows.Forms.ComboBox cbSPDocTyp;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescr;
        private System.Windows.Forms.Button btAttach;
        private System.Windows.Forms.Button btCancel;
    }
}