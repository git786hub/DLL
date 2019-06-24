namespace GTechnology.Oncor.CustomAPI
{
    partial class FromDateDialog
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
            this.fromDatePicker = new System.Windows.Forms.DateTimePicker();
            this.cmdSubmit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // fromDatePicker
            // 
            this.fromDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.fromDatePicker.Location = new System.Drawing.Point(16, 15);
            this.fromDatePicker.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.fromDatePicker.Name = "fromDatePicker";
            this.fromDatePicker.Size = new System.Drawing.Size(337, 22);
            this.fromDatePicker.TabIndex = 0;
            // 
            // cmdSubmit
            // 
            this.cmdSubmit.Location = new System.Drawing.Point(141, 47);
            this.cmdSubmit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmdSubmit.Name = "cmdSubmit";
            this.cmdSubmit.Size = new System.Drawing.Size(100, 28);
            this.cmdSubmit.TabIndex = 1;
            this.cmdSubmit.Text = "Ok";
            this.cmdSubmit.UseVisualStyleBackColor = true;
            this.cmdSubmit.Click += new System.EventHandler(this.cmdSubmit_Click);
            // 
            // FromDateDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(371, 86);
            this.Controls.Add(this.cmdSubmit);
            this.Controls.Add(this.fromDatePicker);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FromDateDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "From Date";
            this.Load += new System.EventHandler(this.FromDateDialog_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DateTimePicker fromDatePicker;
        private System.Windows.Forms.Button cmdSubmit;
    }
}