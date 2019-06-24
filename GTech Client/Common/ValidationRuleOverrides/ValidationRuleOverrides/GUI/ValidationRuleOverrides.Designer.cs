namespace GTechnology.Oncor.CustomAPI.GUI
{
    partial class ValidationRuleOverrides
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
            this.Just = new System.Windows.Forms.Panel();
            this.btnPanel = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnContinue = new System.Windows.Forms.Button();
            this.txtJustification = new System.Windows.Forms.TextBox();
            this.txtError = new System.Windows.Forms.TextBox();
            this.lblJustification = new System.Windows.Forms.Label();
            this.lblError = new System.Windows.Forms.Label();
            this.dtGridViewValidationErr = new System.Windows.Forms.DataGridView();
            this.Error_MSG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FeatureClass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.G3e_Fid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Override_Comments = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Just.SuspendLayout();
            this.btnPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtGridViewValidationErr)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Just
            // 
            this.Just.Controls.Add(this.btnPanel);
            this.Just.Controls.Add(this.txtJustification);
            this.Just.Controls.Add(this.txtError);
            this.Just.Controls.Add(this.lblJustification);
            this.Just.Controls.Add(this.lblError);
            this.Just.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Just.Location = new System.Drawing.Point(0, 243);
            this.Just.Name = "Just";
            this.Just.Size = new System.Drawing.Size(781, 161);
            this.Just.TabIndex = 1;
            // 
            // btnPanel
            // 
            this.btnPanel.Controls.Add(this.btnCancel);
            this.btnPanel.Controls.Add(this.btnContinue);
            this.btnPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnPanel.Location = new System.Drawing.Point(0, 121);
            this.btnPanel.Name = "btnPanel";
            this.btnPanel.Size = new System.Drawing.Size(781, 40);
            this.btnPanel.TabIndex = 8;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(691, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnContinue
            // 
            this.btnContinue.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnContinue.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnContinue.Location = new System.Drawing.Point(610, 5);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(75, 23);
            this.btnContinue.TabIndex = 6;
            this.btnContinue.Text = "Continue";
            this.btnContinue.UseVisualStyleBackColor = true;
            // 
            // txtJustification
            // 
            this.txtJustification.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtJustification.Location = new System.Drawing.Point(125, 62);
            this.txtJustification.Multiline = true;
            this.txtJustification.Name = "txtJustification";
            this.txtJustification.Size = new System.Drawing.Size(644, 50);
            this.txtJustification.TabIndex = 5;
            this.txtJustification.TextChanged += new System.EventHandler(this.txtJustification_TextChanged);
            // 
            // txtError
            // 
            this.txtError.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtError.Location = new System.Drawing.Point(122, 6);
            this.txtError.Multiline = true;
            this.txtError.Name = "txtError";
            this.txtError.ReadOnly = true;
            this.txtError.Size = new System.Drawing.Size(644, 50);
            this.txtError.TabIndex = 4;
            // 
            // lblJustification
            // 
            this.lblJustification.AutoSize = true;
            this.lblJustification.Location = new System.Drawing.Point(54, 82);
            this.lblJustification.Name = "lblJustification";
            this.lblJustification.Size = new System.Drawing.Size(62, 13);
            this.lblJustification.TabIndex = 1;
            this.lblJustification.Text = "Justification";
            this.lblJustification.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.Location = new System.Drawing.Point(54, 29);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(29, 13);
            this.lblError.TabIndex = 0;
            this.lblError.Text = "Error";
            this.lblError.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtGridViewValidationErr
            // 
            this.dtGridViewValidationErr.AllowUserToAddRows = false;
            this.dtGridViewValidationErr.AllowUserToDeleteRows = false;
            this.dtGridViewValidationErr.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtGridViewValidationErr.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtGridViewValidationErr.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Error_MSG,
            this.FeatureClass,
            this.G3e_Fid,
            this.Override_Comments});
            this.dtGridViewValidationErr.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dtGridViewValidationErr.Location = new System.Drawing.Point(12, 12);
            this.dtGridViewValidationErr.Name = "dtGridViewValidationErr";
            this.dtGridViewValidationErr.ReadOnly = true;
            this.dtGridViewValidationErr.Size = new System.Drawing.Size(757, 225);
            this.dtGridViewValidationErr.TabIndex = 2;
            this.dtGridViewValidationErr.SelectionChanged += new System.EventHandler(this.dtGridViewValidationErr_SelectionChanged);
            // 
            // Error_MSG
            // 
            this.Error_MSG.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Error_MSG.HeaderText = "Error";
            this.Error_MSG.Name = "Error_MSG";
            this.Error_MSG.ReadOnly = true;
            // 
            // FeatureClass
            // 
            this.FeatureClass.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.FeatureClass.HeaderText = "Feature Class";
            this.FeatureClass.Name = "FeatureClass";
            this.FeatureClass.ReadOnly = true;
            this.FeatureClass.Width = 5;
            // 
            // G3e_Fid
            // 
            this.G3e_Fid.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.G3e_Fid.HeaderText = "FID";
            this.G3e_Fid.Name = "G3e_Fid";
            this.G3e_Fid.ReadOnly = true;
            this.G3e_Fid.Width = 5;
            // 
            // Override_Comments
            // 
            this.Override_Comments.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Override_Comments.HeaderText = "Override Justification";
            this.Override_Comments.Name = "Override_Comments";
            this.Override_Comments.ReadOnly = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dtGridViewValidationErr);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(781, 243);
            this.panel1.TabIndex = 2;
            // 
            // ValidationRuleOverrides
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(781, 404);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.Just);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ValidationRuleOverrides";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Validation Rule Overrides";
            this.Shown += new System.EventHandler(this.ValidationRuleOverrides_Shown);
            this.Just.ResumeLayout(false);
            this.Just.PerformLayout();
            this.btnPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtGridViewValidationErr)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Just;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnContinue;
        private System.Windows.Forms.TextBox txtJustification;
        private System.Windows.Forms.TextBox txtError;
        private System.Windows.Forms.Label lblJustification;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.DataGridView dtGridViewValidationErr;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Error_MSG;
        private System.Windows.Forms.DataGridViewTextBoxColumn FeatureClass;
        private System.Windows.Forms.DataGridViewTextBoxColumn G3e_Fid;
        private System.Windows.Forms.DataGridViewTextBoxColumn Override_Comments;
        private System.Windows.Forms.Panel btnPanel;
    }
}