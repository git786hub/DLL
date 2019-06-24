namespace GTechnology.Oncor.CustomAPI.View
{
    partial class FormWithoutMSLA
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
            this.grpBoxControls = new System.Windows.Forms.GroupBox();
            this.txtRemoval = new System.Windows.Forms.TextBox();
            this.lblRemoval = new System.Windows.Forms.Label();
            this.txtAddition = new System.Windows.Forms.TextBox();
            this.lblAddition = new System.Windows.Forms.Label();
            this.txtCost = new System.Windows.Forms.TextBox();
            this.lblCost = new System.Windows.Forms.Label();
            this.txtCity = new System.Windows.Forms.TextBox();
            this.lblCity = new System.Windows.Forms.Label();
            this.txtWr = new System.Windows.Forms.TextBox();
            this.lblWR = new System.Windows.Forms.Label();
            this.dgvMSLA = new System.Windows.Forms.DataGridView();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grpBoxControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMSLA)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpBoxControls
            // 
            this.grpBoxControls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxControls.Controls.Add(this.txtRemoval);
            this.grpBoxControls.Controls.Add(this.lblRemoval);
            this.grpBoxControls.Controls.Add(this.txtAddition);
            this.grpBoxControls.Controls.Add(this.lblAddition);
            this.grpBoxControls.Controls.Add(this.txtCost);
            this.grpBoxControls.Controls.Add(this.lblCost);
            this.grpBoxControls.Controls.Add(this.txtCity);
            this.grpBoxControls.Controls.Add(this.lblCity);
            this.grpBoxControls.Controls.Add(this.txtWr);
            this.grpBoxControls.Controls.Add(this.lblWR);
            this.grpBoxControls.Location = new System.Drawing.Point(3, 2);
            this.grpBoxControls.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpBoxControls.Name = "grpBoxControls";
            this.grpBoxControls.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpBoxControls.Size = new System.Drawing.Size(1360, 59);
            this.grpBoxControls.TabIndex = 0;
            this.grpBoxControls.TabStop = false;
            // 
            // txtRemoval
            // 
            this.txtRemoval.Location = new System.Drawing.Point(1053, 14);
            this.txtRemoval.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtRemoval.Name = "txtRemoval";
            this.txtRemoval.ReadOnly = true;
            this.txtRemoval.Size = new System.Drawing.Size(137, 22);
            this.txtRemoval.TabIndex = 9;
            // 
            // lblRemoval
            // 
            this.lblRemoval.AutoSize = true;
            this.lblRemoval.Location = new System.Drawing.Point(984, 17);
            this.lblRemoval.Name = "lblRemoval";
            this.lblRemoval.Size = new System.Drawing.Size(63, 16);
            this.lblRemoval.TabIndex = 8;
            this.lblRemoval.Text = "Removal";
            // 
            // txtAddition
            // 
            this.txtAddition.Location = new System.Drawing.Point(807, 14);
            this.txtAddition.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtAddition.Name = "txtAddition";
            this.txtAddition.ReadOnly = true;
            this.txtAddition.Size = new System.Drawing.Size(145, 22);
            this.txtAddition.TabIndex = 7;
            // 
            // lblAddition
            // 
            this.lblAddition.AutoSize = true;
            this.lblAddition.Location = new System.Drawing.Point(741, 17);
            this.lblAddition.Name = "lblAddition";
            this.lblAddition.Size = new System.Drawing.Size(57, 16);
            this.lblAddition.TabIndex = 6;
            this.lblAddition.Text = "Addition";
            // 
            // txtCost
            // 
            this.txtCost.Location = new System.Drawing.Point(567, 14);
            this.txtCost.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtCost.Name = "txtCost";
            this.txtCost.Size = new System.Drawing.Size(137, 22);
            this.txtCost.TabIndex = 5;
            this.txtCost.TextChanged += new System.EventHandler(this.txtCost_TextChanged);
            // 
            // lblCost
            // 
            this.lblCost.AutoSize = true;
            this.lblCost.Location = new System.Drawing.Point(525, 17);
            this.lblCost.Name = "lblCost";
            this.lblCost.Size = new System.Drawing.Size(35, 16);
            this.lblCost.TabIndex = 4;
            this.lblCost.Text = "Cost";
            // 
            // txtCity
            // 
            this.txtCity.Location = new System.Drawing.Point(248, 14);
            this.txtCity.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtCity.Name = "txtCity";
            this.txtCity.ReadOnly = true;
            this.txtCity.Size = new System.Drawing.Size(249, 22);
            this.txtCity.TabIndex = 3;
            // 
            // lblCity
            // 
            this.lblCity.AutoSize = true;
            this.lblCity.Location = new System.Drawing.Point(211, 17);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(30, 16);
            this.lblCity.TabIndex = 2;
            this.lblCity.Text = "City";
            // 
            // txtWr
            // 
            this.txtWr.Location = new System.Drawing.Point(47, 14);
            this.txtWr.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtWr.Name = "txtWr";
            this.txtWr.ReadOnly = true;
            this.txtWr.Size = new System.Drawing.Size(140, 22);
            this.txtWr.TabIndex = 1;
            // 
            // lblWR
            // 
            this.lblWR.AutoSize = true;
            this.lblWR.Location = new System.Drawing.Point(11, 17);
            this.lblWR.Name = "lblWR";
            this.lblWR.Size = new System.Drawing.Size(31, 16);
            this.lblWR.TabIndex = 0;
            this.lblWR.Text = "WR";
            // 
            // dgvMSLA
            // 
            this.dgvMSLA.AllowUserToAddRows = false;
            this.dgvMSLA.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvMSLA.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvMSLA.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvMSLA.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMSLA.Location = new System.Drawing.Point(3, 68);
            this.dgvMSLA.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvMSLA.Name = "dgvMSLA";
            this.dgvMSLA.RowTemplate.Height = 24;
            this.dgvMSLA.Size = new System.Drawing.Size(1360, 459);
            this.dgvMSLA.TabIndex = 8;
            this.dgvMSLA.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMSLA_CellEndEdit);
            this.dgvMSLA.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvMSLA_KeyDown);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(1235, 2);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(125, 32);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(1104, 2);
            this.btnGenerate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(125, 32);
            this.btnGenerate.TabIndex = 9;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnGenerate);
            this.panel1.Location = new System.Drawing.Point(3, 533);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1360, 39);
            this.panel1.TabIndex = 9;
            // 
            // FormWithoutMSLA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1367, 594);
            this.Controls.Add(this.dgvMSLA);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.grpBoxControls);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1383, 633);
            this.Name = "FormWithoutMSLA";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Generate Street Light Supplemental Agreement";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormWithoutMSLA_FormClosing);
            this.Load += new System.EventHandler(this.FormWithoutMSLA_Load);
            this.grpBoxControls.ResumeLayout(false);
            this.grpBoxControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMSLA)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBoxControls;
        private System.Windows.Forms.TextBox txtRemoval;
        private System.Windows.Forms.Label lblRemoval;
        private System.Windows.Forms.TextBox txtAddition;
        private System.Windows.Forms.Label lblAddition;
        private System.Windows.Forms.TextBox txtCost;
        private System.Windows.Forms.Label lblCost;
        private System.Windows.Forms.TextBox txtCity;
        private System.Windows.Forms.Label lblCity;
        private System.Windows.Forms.TextBox txtWr;
        private System.Windows.Forms.Label lblWR;
        private System.Windows.Forms.DataGridView dgvMSLA;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Panel panel1;
    }
}