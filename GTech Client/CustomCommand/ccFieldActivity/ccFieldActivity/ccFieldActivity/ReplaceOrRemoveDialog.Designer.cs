namespace GTechnology.Oncor.CustomAPI
{
    partial class replaceOrRemoveDialog
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
            this.ReplaceOrRemoveDialogMessage = new System.Windows.Forms.Label();
            this.cancelCmd_btn = new System.Windows.Forms.Button();
            this.removeCmd_btn = new System.Windows.Forms.Button();
            this.replaceCmd_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ReplaceOrRemoveDialogMessage
            // 
            this.ReplaceOrRemoveDialogMessage.AutoSize = true;
            this.ReplaceOrRemoveDialogMessage.Location = new System.Drawing.Point(74, 27);
            this.ReplaceOrRemoveDialogMessage.Name = "ReplaceOrRemoveDialogMessage";
            this.ReplaceOrRemoveDialogMessage.Size = new System.Drawing.Size(250, 13);
            this.ReplaceOrRemoveDialogMessage.TabIndex = 0;
            this.ReplaceOrRemoveDialogMessage.Text = "Do you wish to replace or remove selected feature?";
            // 
            // cancelCmd_btn
            // 
            this.cancelCmd_btn.Location = new System.Drawing.Point(232, 60);
            this.cancelCmd_btn.Name = "cancelCmd_btn";
            this.cancelCmd_btn.Size = new System.Drawing.Size(75, 23);
            this.cancelCmd_btn.TabIndex = 2;
            this.cancelCmd_btn.Text = "Cancel";
            this.cancelCmd_btn.UseVisualStyleBackColor = true;
            this.cancelCmd_btn.Click += new System.EventHandler(this.cancelCmd_btn_Click);
            // 
            // removeCmd_btn
            // 
            this.removeCmd_btn.Location = new System.Drawing.Point(151, 60);
            this.removeCmd_btn.Name = "removeCmd_btn";
            this.removeCmd_btn.Size = new System.Drawing.Size(75, 23);
            this.removeCmd_btn.TabIndex = 1;
            this.removeCmd_btn.Text = "Remove";
            this.removeCmd_btn.UseVisualStyleBackColor = true;
            this.removeCmd_btn.Click += new System.EventHandler(this.removeCmd_btn_Click);
            // 
            // replaceCmd_btn
            // 
            this.replaceCmd_btn.Location = new System.Drawing.Point(70, 60);
            this.replaceCmd_btn.Name = "replaceCmd_btn";
            this.replaceCmd_btn.Size = new System.Drawing.Size(75, 23);
            this.replaceCmd_btn.TabIndex = 0;
            this.replaceCmd_btn.Text = "Replace";
            this.replaceCmd_btn.UseVisualStyleBackColor = true;
            this.replaceCmd_btn.Click += new System.EventHandler(this.replaceCmd_btn_Click);
            // 
            // replaceOrRemoveDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 95);
            this.Controls.Add(this.cancelCmd_btn);
            this.Controls.Add(this.replaceCmd_btn);
            this.Controls.Add(this.ReplaceOrRemoveDialogMessage);
            this.Controls.Add(this.removeCmd_btn);
            this.Name = "replaceOrRemoveDialog";
            this.Text = "Replace or Remove";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ReplaceOrRemoveDialogMessage;
        private System.Windows.Forms.Button cancelCmd_btn;
        private System.Windows.Forms.Button removeCmd_btn;
        private System.Windows.Forms.Button replaceCmd_btn;
    }
}