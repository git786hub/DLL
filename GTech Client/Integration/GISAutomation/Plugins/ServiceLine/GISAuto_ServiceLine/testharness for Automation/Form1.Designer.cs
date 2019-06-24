namespace GTechnology.Oncor.CustomAPI
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
            this.GetTransaction_btn = new System.Windows.Forms.Button();
            this.FileLocation = new System.Windows.Forms.TextBox();
            this.CorrectStructureID_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // GetTransaction_btn
            // 
            this.GetTransaction_btn.Location = new System.Drawing.Point(25, 43);
            this.GetTransaction_btn.Name = "GetTransaction_btn";
            this.GetTransaction_btn.Size = new System.Drawing.Size(143, 23);
            this.GetTransaction_btn.TabIndex = 0;
            this.GetTransaction_btn.Text = "Get Transaction";
            this.GetTransaction_btn.UseVisualStyleBackColor = true;
            this.GetTransaction_btn.Click += new System.EventHandler(this.GetTransaction_btn_Click);
            // 
            // FileLocation
            // 
            this.FileLocation.Location = new System.Drawing.Point(174, 45);
            this.FileLocation.Name = "FileLocation";
            this.FileLocation.Size = new System.Drawing.Size(450, 20);
            this.FileLocation.TabIndex = 1;
            // 
            // CorrectStructureID_btn
            // 
            this.CorrectStructureID_btn.Location = new System.Drawing.Point(25, 72);
            this.CorrectStructureID_btn.Name = "CorrectStructureID_btn";
            this.CorrectStructureID_btn.Size = new System.Drawing.Size(143, 23);
            this.CorrectStructureID_btn.TabIndex = 2;
            this.CorrectStructureID_btn.Text = "Correct Structure";
            this.CorrectStructureID_btn.UseVisualStyleBackColor = true;
            this.CorrectStructureID_btn.Click += new System.EventHandler(this.CorrectStructureID_btn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(731, 131);
            this.Controls.Add(this.CorrectStructureID_btn);
            this.Controls.Add(this.FileLocation);
            this.Controls.Add(this.GetTransaction_btn);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button GetTransaction_btn;
        private System.Windows.Forms.TextBox FileLocation;
        private System.Windows.Forms.Button CorrectStructureID_btn;
    }
}

