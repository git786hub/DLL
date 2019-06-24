namespace GTechnology.Oncor.CustomAPI.View
{
    partial class frmOverwriteDocument
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtDocName = new System.Windows.Forms.TextBox();
            this.rbCancel = new System.Windows.Forms.RadioButton();
            this.rbOverwrite = new System.Windows.Forms.RadioButton();
            this.rbDiffDoc = new System.Windows.Forms.RadioButton();
            this.lblMess = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtDocName);
            this.groupBox1.Controls.Add(this.rbCancel);
            this.groupBox1.Controls.Add(this.rbOverwrite);
            this.groupBox1.Controls.Add(this.rbDiffDoc);
            this.groupBox1.Location = new System.Drawing.Point(15, 29);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(376, 86);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // txtDocName
            // 
            this.txtDocName.Enabled = false;
            this.txtDocName.Location = new System.Drawing.Point(207, 13);
            this.txtDocName.Name = "txtDocName";
            this.txtDocName.Size = new System.Drawing.Size(163, 20);
            this.txtDocName.TabIndex = 3;
            this.txtDocName.Visible = false;
            // 
            // rbCancel
            // 
            this.rbCancel.AutoSize = true;
            this.rbCancel.Location = new System.Drawing.Point(13, 59);
            this.rbCancel.Name = "rbCancel";
            this.rbCancel.Size = new System.Drawing.Size(141, 19);
            this.rbCancel.TabIndex = 2;
            this.rbCancel.TabStop = true;
            this.rbCancel.Text = "Cancel the operation";
            this.rbCancel.UseVisualStyleBackColor = true;
            this.rbCancel.CheckedChanged += new System.EventHandler(this.rbCancel_CheckedChanged);
            // 
            // rbOverwrite
            // 
            this.rbOverwrite.AutoSize = true;
            this.rbOverwrite.Location = new System.Drawing.Point(13, 36);
            this.rbOverwrite.Name = "rbOverwrite";
            this.rbOverwrite.Size = new System.Drawing.Size(312, 19);
            this.rbOverwrite.TabIndex = 1;
            this.rbOverwrite.TabStop = true;
            this.rbOverwrite.Text = "Overwrite the existing attachment by the same name";
            this.rbOverwrite.UseVisualStyleBackColor = true;
            this.rbOverwrite.CheckedChanged += new System.EventHandler(this.rbOverwrite_CheckedChanged);
            // 
            // rbDiffDoc
            // 
            this.rbDiffDoc.AutoSize = true;
            this.rbDiffDoc.Location = new System.Drawing.Point(13, 13);
            this.rbDiffDoc.Name = "rbDiffDoc";
            this.rbDiffDoc.Size = new System.Drawing.Size(198, 19);
            this.rbDiffDoc.TabIndex = 0;
            this.rbDiffDoc.TabStop = true;
            this.rbDiffDoc.Text = "Give Different Document Name";
            this.rbDiffDoc.UseVisualStyleBackColor = true;
            this.rbDiffDoc.CheckedChanged += new System.EventHandler(this.rbDiffDoc_CheckedChanged);
            // 
            // lblMess
            // 
            this.lblMess.AutoSize = true;
            this.lblMess.Location = new System.Drawing.Point(12, 13);
            this.lblMess.Name = "lblMess";
            this.lblMess.Size = new System.Drawing.Size(518, 15);
            this.lblMess.TabIndex = 1;
            this.lblMess.Text = "Document name already exists as an attachment to the active job,Please select bel" +
    "ow options.";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(397, 63);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(121, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // frmOverwriteDocument
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 120);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblMess);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOverwriteDocument";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "OverwriteDocument";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbCancel;
        private System.Windows.Forms.RadioButton rbOverwrite;
        private System.Windows.Forms.RadioButton rbDiffDoc;
        private System.Windows.Forms.TextBox txtDocName;
        private System.Windows.Forms.Label lblMess;
        private System.Windows.Forms.Button btnOk;
    }
}