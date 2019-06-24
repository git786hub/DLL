namespace IsFIMUp
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtServerName = new System.Windows.Forms.TextBox();
            this.btnCheckHealth = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(110, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(195, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "FIM Health Checker";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(43, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Enter FIM Server:";
            // 
            // txtServerName
            // 
            this.txtServerName.Location = new System.Drawing.Point(140, 46);
            this.txtServerName.Name = "txtServerName";
            this.txtServerName.Size = new System.Drawing.Size(211, 20);
            this.txtServerName.TabIndex = 2;
            // 
            // btnCheckHealth
            // 
            this.btnCheckHealth.Location = new System.Drawing.Point(152, 87);
            this.btnCheckHealth.Name = "btnCheckHealth";
            this.btnCheckHealth.Size = new System.Drawing.Size(109, 30);
            this.btnCheckHealth.TabIndex = 3;
            this.btnCheckHealth.Text = "Check Health";
            this.btnCheckHealth.UseVisualStyleBackColor = true;
            this.btnCheckHealth.Click += new System.EventHandler(this.btnCheckHealth_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(408, 144);
            this.Controls.Add(this.btnCheckHealth);
            this.Controls.Add(this.txtServerName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtServerName;
        private System.Windows.Forms.Button btnCheckHealth;
    }
}

