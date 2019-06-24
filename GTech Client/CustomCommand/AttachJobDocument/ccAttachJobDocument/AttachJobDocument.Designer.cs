namespace GTechnology.Oncor.CustomAPI
{
	partial class AttachJobDocument
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
            this.lblFile = new System.Windows.Forms.Label();
            this.lblType = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnAttach = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.openJobDcoument = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // lblFile
            // 
            this.lblFile.AutoSize = true;
            this.lblFile.Location = new System.Drawing.Point(61, 14);
            this.lblFile.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(30, 16);
            this.lblFile.TabIndex = 0;
            this.lblFile.Text = "File";
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(51, 54);
            this.lblType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(40, 16);
            this.lblType.TabIndex = 1;
            this.lblType.Text = "Type";
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(12, 91);
            this.lblDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(76, 16);
            this.lblDescription.TabIndex = 2;
            this.lblDescription.Text = "Description";
            // 
            // txtFile
            // 
            this.txtFile.Location = new System.Drawing.Point(103, 14);
            this.txtFile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(423, 22);
            this.txtFile.TabIndex = 1;
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(103, 91);
            this.txtDescription.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(423, 22);
            this.txtDescription.TabIndex = 4;
            // 
            // cmbType
            // 
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Items.AddRange(new object[] {
            "Text Files (*.txt)",
            "PDF files (*.pdf)",
            "Microsoft Word Files (*.doc;*.docx)",
            "Microsoft Excel Files (*.xls;*.xlsx)",
            "All types (*.*)"});
            this.cmbType.Location = new System.Drawing.Point(103, 53);
            this.cmbType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(237, 24);
            this.cmbType.TabIndex = 3;
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(541, 6);
            this.btnSelect.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(100, 36);
            this.btnSelect.TabIndex = 2;
            this.btnSelect.Text = "Select";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnAttach
            // 
            this.btnAttach.Location = new System.Drawing.Point(541, 126);
            this.btnAttach.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAttach.Name = "btnAttach";
            this.btnAttach.Size = new System.Drawing.Size(100, 36);
            this.btnAttach.TabIndex = 5;
            this.btnAttach.Text = "Attach";
            this.btnAttach.UseVisualStyleBackColor = true;
            this.btnAttach.Click += new System.EventHandler(this.btnAttach_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(433, 126);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 36);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // openJobDcoument
            // 
            this.openJobDcoument.FileName = "openJobDcoument";
            this.openJobDcoument.RestoreDirectory = true;
            this.openJobDcoument.Title = "Select a File";
            // 
            // AttachJobDocument
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(655, 166);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAttach);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.txtFile);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.lblFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AttachJobDocument";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Attach Job Document";
            this.Load += new System.EventHandler(this.AttachJobDocument_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.StrandPortFrm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblFile;
		private System.Windows.Forms.Label lblType;
		private System.Windows.Forms.Label lblDescription;
		private System.Windows.Forms.TextBox txtFile;
		private System.Windows.Forms.TextBox txtDescription;
		private System.Windows.Forms.ComboBox cmbType;
		private System.Windows.Forms.Button btnSelect;
		private System.Windows.Forms.Button btnAttach;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.OpenFileDialog openJobDcoument;
	}
}