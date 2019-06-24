namespace GTechnology.Oncor.CustomAPI
{
    partial class frmSelectReplacementStatus
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
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnSalvage = new System.Windows.Forms.Button();
            this.btnAbandon = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblSelectReplacementStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(6, 29);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(57, 23);
            this.btnRemove.TabIndex = 0;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnSalvage
            // 
            this.btnSalvage.Location = new System.Drawing.Point(69, 29);
            this.btnSalvage.Name = "btnSalvage";
            this.btnSalvage.Size = new System.Drawing.Size(54, 23);
            this.btnSalvage.TabIndex = 1;
            this.btnSalvage.Text = "Salvage";
            this.btnSalvage.UseVisualStyleBackColor = true;
            this.btnSalvage.Click += new System.EventHandler(this.btnSalvage_Click);
            // 
            // btnAbandon
            // 
            this.btnAbandon.Location = new System.Drawing.Point(125, 29);
            this.btnAbandon.Name = "btnAbandon";
            this.btnAbandon.Size = new System.Drawing.Size(61, 23);
            this.btnAbandon.TabIndex = 2;
            this.btnAbandon.Text = "Abandon";
            this.btnAbandon.UseVisualStyleBackColor = true;
            this.btnAbandon.Click += new System.EventHandler(this.btnAbandon_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(125, 58);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(61, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblSelectReplacementStatus
            // 
            this.lblSelectReplacementStatus.AutoSize = true;
            this.lblSelectReplacementStatus.Location = new System.Drawing.Point(3, 5);
            this.lblSelectReplacementStatus.Name = "lblSelectReplacementStatus";
            this.lblSelectReplacementStatus.Size = new System.Drawing.Size(132, 13);
            this.lblSelectReplacementStatus.TabIndex = 4;
            this.lblSelectReplacementStatus.Text = "Select replacement status:";
            // 
            // frmSelectReplacementStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(194, 86);
            this.Controls.Add(this.lblSelectReplacementStatus);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAbandon);
            this.Controls.Add(this.btnSalvage);
            this.Controls.Add(this.btnRemove);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSelectReplacementStatus";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Replace Feature";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnSalvage;
        private System.Windows.Forms.Button btnAbandon;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblSelectReplacementStatus;
    }
}