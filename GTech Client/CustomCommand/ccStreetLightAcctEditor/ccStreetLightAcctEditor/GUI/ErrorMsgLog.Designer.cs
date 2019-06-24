namespace GTechnology.Oncor.CustomAPI.GUI
{
    partial class ErrorMsgLog
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dtGridViewErrLog = new System.Windows.Forms.DataGridView();
            this.btnExit = new System.Windows.Forms.Button();
            this.Tab = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ErrorMsg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dtGridViewErrLog)).BeginInit();
            this.SuspendLayout();
            // 
            // dtGridViewErrLog
            // 
            this.dtGridViewErrLog.AllowUserToAddRows = false;
            this.dtGridViewErrLog.AllowUserToDeleteRows = false;
            this.dtGridViewErrLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtGridViewErrLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Tab,
            this.ErrorMsg});
            this.dtGridViewErrLog.Location = new System.Drawing.Point(1, 3);
            this.dtGridViewErrLog.Name = "dtGridViewErrLog";
            this.dtGridViewErrLog.ReadOnly = true;
            this.dtGridViewErrLog.Size = new System.Drawing.Size(652, 311);
            this.dtGridViewErrLog.TabIndex = 0;
            // 
            // btnExit
            // 
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Location = new System.Drawing.Point(567, 320);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // Tab
            // 
            this.Tab.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Tab.HeaderText = "Error In";
            this.Tab.Name = "Tab";
            this.Tab.ReadOnly = true;
            this.Tab.Width = 66;
            // 
            // ErrorMsg
            // 
            this.ErrorMsg.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ErrorMsg.DefaultCellStyle = dataGridViewCellStyle1;
            this.ErrorMsg.HeaderText = "Error Message";
            this.ErrorMsg.Name = "ErrorMsg";
            this.ErrorMsg.ReadOnly = true;
            // 
            // ErrorMsgLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(654, 351);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.dtGridViewErrLog);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ErrorMsgLog";
            this.Text = "ErrorLog";
            ((System.ComponentModel.ISupportInitialize)(this.dtGridViewErrLog)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dtGridViewErrLog;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Tab;
        private System.Windows.Forms.DataGridViewTextBoxColumn ErrorMsg;
    }
}