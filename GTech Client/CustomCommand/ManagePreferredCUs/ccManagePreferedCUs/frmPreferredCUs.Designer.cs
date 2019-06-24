namespace GTechnology.Oncor.CustomAPI
{
    partial class frmPreferredCUs
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
            this.listboxAvailable = new System.Windows.Forms.ListBox();
            this.btnToPreferred = new System.Windows.Forms.Button();
            this.btnToAvailableMany = new System.Windows.Forms.Button();
            this.btnToPreferredMany = new System.Windows.Forms.Button();
            this.btnToAvailable = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlPreferred = new System.Windows.Forms.Panel();
            this.lblPreferred = new System.Windows.Forms.Label();
            this.pnlCategory = new System.Windows.Forms.Panel();
            this.lblCategory = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.pnlAvailable = new System.Windows.Forms.Panel();
            this.lblAvailable = new System.Windows.Forms.Label();
            this.listboxPreferred = new System.Windows.Forms.ListBox();
            this.panel1.SuspendLayout();
            this.pnlPreferred.SuspendLayout();
            this.pnlCategory.SuspendLayout();
            this.pnlAvailable.SuspendLayout();
            this.SuspendLayout();
            // 
            // listboxAvailable
            // 
            this.listboxAvailable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.listboxAvailable.FormattingEnabled = true;
            this.listboxAvailable.HorizontalScrollbar = true;
            this.listboxAvailable.ItemHeight = 16;
            this.listboxAvailable.Location = new System.Drawing.Point(507, 95);
            this.listboxAvailable.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listboxAvailable.Name = "listboxAvailable";
            this.listboxAvailable.ScrollAlwaysVisible = true;
            this.listboxAvailable.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listboxAvailable.Size = new System.Drawing.Size(376, 420);
            this.listboxAvailable.TabIndex = 2;
            this.listboxAvailable.SelectedIndexChanged += new System.EventHandler(this.listboxAvailable_SelectedIndexChanged);
            // 
            // btnToPreferred
            // 
            this.btnToPreferred.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnToPreferred.Location = new System.Drawing.Point(425, 135);
            this.btnToPreferred.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnToPreferred.Name = "btnToPreferred";
            this.btnToPreferred.Size = new System.Drawing.Size(60, 28);
            this.btnToPreferred.TabIndex = 3;
            this.btnToPreferred.Text = "<";
            this.btnToPreferred.UseVisualStyleBackColor = true;
            this.btnToPreferred.Click += new System.EventHandler(this.btnToPreferred_Click);
            // 
            // btnToAvailableMany
            // 
            this.btnToAvailableMany.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnToAvailableMany.Location = new System.Drawing.Point(425, 242);
            this.btnToAvailableMany.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnToAvailableMany.Name = "btnToAvailableMany";
            this.btnToAvailableMany.Size = new System.Drawing.Size(60, 28);
            this.btnToAvailableMany.TabIndex = 4;
            this.btnToAvailableMany.Text = ">>";
            this.btnToAvailableMany.UseVisualStyleBackColor = true;
            this.btnToAvailableMany.Click += new System.EventHandler(this.btnToAvailableMany_Click);
            // 
            // btnToPreferredMany
            // 
            this.btnToPreferredMany.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnToPreferredMany.Location = new System.Drawing.Point(425, 207);
            this.btnToPreferredMany.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnToPreferredMany.Name = "btnToPreferredMany";
            this.btnToPreferredMany.Size = new System.Drawing.Size(60, 28);
            this.btnToPreferredMany.TabIndex = 5;
            this.btnToPreferredMany.Text = "<<";
            this.btnToPreferredMany.UseVisualStyleBackColor = true;
            this.btnToPreferredMany.Click += new System.EventHandler(this.btnToPreferredMany_Click);
            // 
            // btnToAvailable
            // 
            this.btnToAvailable.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnToAvailable.Location = new System.Drawing.Point(425, 171);
            this.btnToAvailable.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnToAvailable.Name = "btnToAvailable";
            this.btnToAvailable.Size = new System.Drawing.Size(60, 28);
            this.btnToAvailable.TabIndex = 6;
            this.btnToAvailable.Text = ">";
            this.btnToAvailable.UseVisualStyleBackColor = true;
            this.btnToAvailable.Click += new System.EventHandler(this.btnToAvailable_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(797, 528);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(89, 30);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(704, 528);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(85, 30);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pnlPreferred);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.pnlCategory);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.pnlAvailable);
            this.panel1.Controls.Add(this.listboxAvailable);
            this.panel1.Controls.Add(this.listboxPreferred);
            this.panel1.Controls.Add(this.btnToAvailableMany);
            this.panel1.Controls.Add(this.btnToPreferredMany);
            this.panel1.Controls.Add(this.btnToAvailable);
            this.panel1.Controls.Add(this.btnToPreferred);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.MinimumSize = new System.Drawing.Size(913, 574);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(913, 574);
            this.panel1.TabIndex = 9;
            // 
            // pnlPreferred
            // 
            this.pnlPreferred.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPreferred.Controls.Add(this.lblPreferred);
            this.pnlPreferred.Location = new System.Drawing.Point(23, 58);
            this.pnlPreferred.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlPreferred.Name = "pnlPreferred";
            this.pnlPreferred.Size = new System.Drawing.Size(377, 36);
            this.pnlPreferred.TabIndex = 13;
            // 
            // lblPreferred
            // 
            this.lblPreferred.AutoSize = true;
            this.lblPreferred.Location = new System.Drawing.Point(4, 6);
            this.lblPreferred.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPreferred.Name = "lblPreferred";
            this.lblPreferred.Size = new System.Drawing.Size(68, 17);
            this.lblPreferred.TabIndex = 11;
            this.lblPreferred.Text = "Preferred";
            // 
            // pnlCategory
            // 
            this.pnlCategory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCategory.Controls.Add(this.lblCategory);
            this.pnlCategory.Controls.Add(this.cmbCategory);
            this.pnlCategory.Location = new System.Drawing.Point(23, 1);
            this.pnlCategory.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlCategory.Name = "pnlCategory";
            this.pnlCategory.Size = new System.Drawing.Size(861, 56);
            this.pnlCategory.TabIndex = 12;
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.Location = new System.Drawing.Point(4, 18);
            this.lblCategory.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(65, 17);
            this.lblCategory.TabIndex = 10;
            this.lblCategory.Text = "Category";
            // 
            // cmbCategory
            // 
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(77, 15);
            this.cmbCategory.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(595, 24);
            this.cmbCategory.TabIndex = 11;
            this.cmbCategory.SelectedIndexChanged += new System.EventHandler(this.cmbCategory_SelectedIndexChanged);
            // 
            // pnlAvailable
            // 
            this.pnlAvailable.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pnlAvailable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAvailable.Controls.Add(this.lblAvailable);
            this.pnlAvailable.Location = new System.Drawing.Point(507, 58);
            this.pnlAvailable.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlAvailable.Name = "pnlAvailable";
            this.pnlAvailable.Size = new System.Drawing.Size(377, 36);
            this.pnlAvailable.TabIndex = 14;
            // 
            // lblAvailable
            // 
            this.lblAvailable.AutoSize = true;
            this.lblAvailable.Location = new System.Drawing.Point(4, 6);
            this.lblAvailable.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAvailable.Name = "lblAvailable";
            this.lblAvailable.Size = new System.Drawing.Size(65, 17);
            this.lblAvailable.TabIndex = 12;
            this.lblAvailable.Text = "Available";
            // 
            // listboxPreferred
            // 
            this.listboxPreferred.FormattingEnabled = true;
            this.listboxPreferred.HorizontalScrollbar = true;
            this.listboxPreferred.ItemHeight = 16;
            this.listboxPreferred.Location = new System.Drawing.Point(23, 95);
            this.listboxPreferred.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listboxPreferred.Name = "listboxPreferred";
            this.listboxPreferred.ScrollAlwaysVisible = true;
            this.listboxPreferred.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listboxPreferred.Size = new System.Drawing.Size(376, 420);
            this.listboxPreferred.TabIndex = 2;
            this.listboxPreferred.SelectedIndexChanged += new System.EventHandler(this.listboxPreferred_SelectedIndexChanged);
            // 
            // frmPreferredCUs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(913, 582);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(929, 619);
            this.Name = "frmPreferredCUs";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Preferred CUs";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPreferredCUs_FormClosing);
            this.Resize += new System.EventHandler(this.frmPreferredCUs_Resize);
            this.panel1.ResumeLayout(false);
            this.pnlPreferred.ResumeLayout(false);
            this.pnlPreferred.PerformLayout();
            this.pnlCategory.ResumeLayout(false);
            this.pnlCategory.PerformLayout();
            this.pnlAvailable.ResumeLayout(false);
            this.pnlAvailable.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListBox listboxAvailable;
        private System.Windows.Forms.Button btnToPreferred;
        private System.Windows.Forms.Button btnToAvailableMany;
        private System.Windows.Forms.Button btnToPreferredMany;
        private System.Windows.Forms.Button btnToAvailable;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblAvailable;
        private System.Windows.Forms.Label lblPreferred;
        private System.Windows.Forms.Panel pnlAvailable;
        private System.Windows.Forms.Panel pnlPreferred;
        private System.Windows.Forms.ListBox listboxPreferred;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.Panel pnlCategory;
    }
}