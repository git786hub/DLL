namespace OncDocManage
{
    partial class frmRenameFile
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
            this.lblCurFileName = new System.Windows.Forms.Label();
            this.txtNewName = new System.Windows.Forms.TextBox();
            this.lblNewFileName = new System.Windows.Forms.Label();
            this.btUpdName = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.lblMsg = new System.Windows.Forms.Label();
            this.lblReplaceOrRename = new System.Windows.Forms.Label();
            this.cbReplcRenam = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // lblCurFileName
            // 
            this.lblCurFileName.AutoSize = true;
            this.lblCurFileName.Location = new System.Drawing.Point(12, 66);
            this.lblCurFileName.Name = "lblCurFileName";
            this.lblCurFileName.Size = new System.Drawing.Size(114, 13);
            this.lblCurFileName.TabIndex = 0;
            this.lblCurFileName.Text = "Current SP File Name: ";
            // 
            // txtNewName
            // 
            this.txtNewName.Location = new System.Drawing.Point(120, 99);
            this.txtNewName.Name = "txtNewName";
            this.txtNewName.Size = new System.Drawing.Size(273, 20);
            this.txtNewName.TabIndex = 1;
            // 
            // lblNewFileName
            // 
            this.lblNewFileName.AutoSize = true;
            this.lblNewFileName.Location = new System.Drawing.Point(12, 102);
            this.lblNewFileName.Name = "lblNewFileName";
            this.lblNewFileName.Size = new System.Drawing.Size(102, 13);
            this.lblNewFileName.TabIndex = 2;
            this.lblNewFileName.Text = "New SP File Name: ";
            // 
            // btUpdName
            // 
            this.btUpdName.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btUpdName.Location = new System.Drawing.Point(120, 132);
            this.btUpdName.Name = "btUpdName";
            this.btUpdName.Size = new System.Drawing.Size(89, 23);
            this.btUpdName.TabIndex = 3;
            this.btUpdName.Text = "Update Name";
            this.btUpdName.UseVisualStyleBackColor = true;
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(231, 132);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(89, 23);
            this.btCancel.TabIndex = 4;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsg.Location = new System.Drawing.Point(12, 9);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(213, 13);
            this.lblMsg.TabIndex = 5;
            this.lblMsg.Text = "The file already exist in SharePoint. \r\n";
            // 
            // lblReplaceOrRename
            // 
            this.lblReplaceOrRename.AutoSize = true;
            this.lblReplaceOrRename.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReplaceOrRename.Location = new System.Drawing.Point(12, 39);
            this.lblReplaceOrRename.Name = "lblReplaceOrRename";
            this.lblReplaceOrRename.Size = new System.Drawing.Size(247, 13);
            this.lblReplaceOrRename.TabIndex = 6;
            this.lblReplaceOrRename.Text = "Do you wish to replace or rename the file?";
            // 
            // cbReplcRenam
            // 
            this.cbReplcRenam.FormattingEnabled = true;
            this.cbReplcRenam.Items.AddRange(new object[] {
            "Rename",
            "Replace"});
            this.cbReplcRenam.Location = new System.Drawing.Point(272, 36);
            this.cbReplcRenam.Name = "cbReplcRenam";
            this.cbReplcRenam.Size = new System.Drawing.Size(121, 21);
            this.cbReplcRenam.TabIndex = 7;
            this.cbReplcRenam.Text = "Rename";
            this.cbReplcRenam.SelectedIndexChanged += new System.EventHandler(this.cbReplcRenam_SelectedIndexChanged);
            // 
            // frmRenameFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 175);
            this.Controls.Add(this.cbReplcRenam);
            this.Controls.Add(this.lblReplaceOrRename);
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btUpdName);
            this.Controls.Add(this.lblNewFileName);
            this.Controls.Add(this.txtNewName);
            this.Controls.Add(this.lblCurFileName);
            this.Name = "frmRenameFile";
            this.Text = "Rename SharePoint File";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblNewFileName;
        private System.Windows.Forms.Button btUpdName;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Label lblMsg;
        internal System.Windows.Forms.Label lblCurFileName;
        internal System.Windows.Forms.TextBox txtNewName;
        private System.Windows.Forms.Label lblReplaceOrRename;
        private System.Windows.Forms.ComboBox cbReplcRenam;
    }
}