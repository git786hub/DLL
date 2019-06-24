namespace GTechnology.Oncor.CustomAPI
{
    partial class SaveViewForm
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
            this.pnlSavedView = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtViewname = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlSavedView.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSavedView
            // 
            this.pnlSavedView.Controls.Add(this.btnCancel);
            this.pnlSavedView.Controls.Add(this.btnSave);
            this.pnlSavedView.Controls.Add(this.txtViewname);
            this.pnlSavedView.Controls.Add(this.label1);
            this.pnlSavedView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSavedView.Location = new System.Drawing.Point(0, 0);
            this.pnlSavedView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlSavedView.Name = "pnlSavedView";
            this.pnlSavedView.Size = new System.Drawing.Size(271, 84);
            this.pnlSavedView.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(180, 42);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 28);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(87, 42);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 28);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtViewname
            // 
            this.txtViewname.Location = new System.Drawing.Point(87, 12);
            this.txtViewname.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtViewname.Name = "txtViewname";
            this.txtViewname.Size = new System.Drawing.Size(173, 22);
            this.txtViewname.TabIndex = 1;
            this.txtViewname.TextChanged += new System.EventHandler(this.txtViewname_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "View Name";
            // 
            // SaveViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(271, 84);
            this.Controls.Add(this.pnlSavedView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SaveViewForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Save Named View";
            this.pnlSavedView.ResumeLayout(false);
            this.pnlSavedView.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlSavedView;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtViewname;
        private System.Windows.Forms.Label label1;
    }
}