namespace GTechnology.Oncor.CustomAPI
{
    partial class StatusFilterDialog
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
            this.OkCmd = new System.Windows.Forms.Button();
            this.optFailed = new System.Windows.Forms.CheckBox();
            this.optQueud = new System.Windows.Forms.CheckBox();
            this.optDeleted = new System.Windows.Forms.CheckBox();
            this.FilterOptionsBox = new System.Windows.Forms.GroupBox();
            this.FilterOptionsBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // OkCmd
            // 
            this.OkCmd.Location = new System.Drawing.Point(129, 82);
            this.OkCmd.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.OkCmd.Name = "OkCmd";
            this.OkCmd.Size = new System.Drawing.Size(100, 28);
            this.OkCmd.TabIndex = 1;
            this.OkCmd.Text = "Ok";
            this.OkCmd.UseVisualStyleBackColor = true;
            this.OkCmd.Click += new System.EventHandler(this.OkCmd_Click);
            // 
            // optFailed
            // 
            this.optFailed.AutoSize = true;
            this.optFailed.Checked = true;
            this.optFailed.CheckState = System.Windows.Forms.CheckState.Checked;
            this.optFailed.Location = new System.Drawing.Point(8, 23);
            this.optFailed.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.optFailed.Name = "optFailed";
            this.optFailed.Size = new System.Drawing.Size(77, 21);
            this.optFailed.TabIndex = 3;
            this.optFailed.Text = "FAILED";
            this.optFailed.UseVisualStyleBackColor = true;
            // 
            // optQueud
            // 
            this.optQueud.AutoSize = true;
            this.optQueud.Location = new System.Drawing.Point(100, 23);
            this.optQueud.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.optQueud.Name = "optQueud";
            this.optQueud.Size = new System.Drawing.Size(89, 21);
            this.optQueud.TabIndex = 4;
            this.optQueud.Text = "QUEUED";
            this.optQueud.UseVisualStyleBackColor = true;
            // 
            // optDeleted
            // 
            this.optDeleted.AutoSize = true;
            this.optDeleted.Location = new System.Drawing.Point(204, 23);
            this.optDeleted.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.optDeleted.Name = "optDeleted";
            this.optDeleted.Size = new System.Drawing.Size(94, 21);
            this.optDeleted.TabIndex = 5;
            this.optDeleted.Text = "DELETED";
            this.optDeleted.UseVisualStyleBackColor = true;
            // 
            // FilterOptionsBox
            // 
            this.FilterOptionsBox.Controls.Add(this.optFailed);
            this.FilterOptionsBox.Controls.Add(this.optQueud);
            this.FilterOptionsBox.Controls.Add(this.optDeleted);
            this.FilterOptionsBox.Location = new System.Drawing.Point(16, 15);
            this.FilterOptionsBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.FilterOptionsBox.Name = "FilterOptionsBox";
            this.FilterOptionsBox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.FilterOptionsBox.Size = new System.Drawing.Size(321, 60);
            this.FilterOptionsBox.TabIndex = 7;
            this.FilterOptionsBox.TabStop = false;
            this.FilterOptionsBox.Text = "Please Select Status Filters";
            // 
            // StatusFilterDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(353, 117);
            this.Controls.Add(this.FilterOptionsBox);
            this.Controls.Add(this.OkCmd);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StatusFilterDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Status Filter";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.StatusFilterDialog_FormClosed);
            this.Load += new System.EventHandler(this.StatusFilterDialog_Load);
            this.FilterOptionsBox.ResumeLayout(false);
            this.FilterOptionsBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button OkCmd;
        private System.Windows.Forms.CheckBox optFailed;
        private System.Windows.Forms.CheckBox optQueud;
        private System.Windows.Forms.CheckBox optDeleted;
        private System.Windows.Forms.GroupBox FilterOptionsBox;
    }
}