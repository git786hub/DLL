namespace GTechnology.Oncor.CustomAPI.View
{
    partial class FormWithMSLA
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
            this.dtPicker = new System.Windows.Forms.DateTimePicker();
            this.txtCost = new System.Windows.Forms.TextBox();
            this.txtComments = new System.Windows.Forms.TextBox();
            this.txtAgreementDate = new System.Windows.Forms.TextBox();
            this.txtCustomerAgent = new System.Windows.Forms.TextBox();
            this.txtCustomer = new System.Windows.Forms.TextBox();
            this.txtWR = new System.Windows.Forms.TextBox();
            this.lblComments = new System.Windows.Forms.Label();
            this.lblServiceDate = new System.Windows.Forms.Label();
            this.lblAgreementDate = new System.Windows.Forms.Label();
            this.lblCost = new System.Windows.Forms.Label();
            this.lblCustomerAgent = new System.Windows.Forms.Label();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.lblWR = new System.Windows.Forms.Label();
            this.dgvMSLA = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.grpBoxControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMSLA)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpBoxControls
            // 
            this.grpBoxControls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxControls.Controls.Add(this.dtPicker);
            this.grpBoxControls.Controls.Add(this.txtCost);
            this.grpBoxControls.Controls.Add(this.txtComments);
            this.grpBoxControls.Controls.Add(this.txtAgreementDate);
            this.grpBoxControls.Controls.Add(this.txtCustomerAgent);
            this.grpBoxControls.Controls.Add(this.txtCustomer);
            this.grpBoxControls.Controls.Add(this.txtWR);
            this.grpBoxControls.Controls.Add(this.lblComments);
            this.grpBoxControls.Controls.Add(this.lblServiceDate);
            this.grpBoxControls.Controls.Add(this.lblAgreementDate);
            this.grpBoxControls.Controls.Add(this.lblCost);
            this.grpBoxControls.Controls.Add(this.lblCustomerAgent);
            this.grpBoxControls.Controls.Add(this.lblCustomer);
            this.grpBoxControls.Controls.Add(this.lblWR);
            this.grpBoxControls.Location = new System.Drawing.Point(12, 12);
            this.grpBoxControls.Name = "grpBoxControls";
            this.grpBoxControls.Size = new System.Drawing.Size(1202, 119);
            this.grpBoxControls.TabIndex = 2;
            this.grpBoxControls.TabStop = false;
            // 
            // dtPicker
            // 
            this.dtPicker.Location = new System.Drawing.Point(132, 75);
            this.dtPicker.Name = "dtPicker";
            this.dtPicker.Size = new System.Drawing.Size(323, 22);
            this.dtPicker.TabIndex = 13;
            this.dtPicker.Value = new System.DateTime(2018, 3, 1, 14, 34, 54, 0);
            // 
            // txtCost
            // 
            this.txtCost.Location = new System.Drawing.Point(773, 21);
            this.txtCost.Name = "txtCost";
            this.txtCost.Size = new System.Drawing.Size(122, 22);
            this.txtCost.TabIndex = 12;
            this.txtCost.TextChanged += new System.EventHandler(this.txtCost_TextChanged);
            // 
            // txtComments
            // 
            this.txtComments.Location = new System.Drawing.Point(567, 47);
            this.txtComments.Multiline = true;
            this.txtComments.Name = "txtComments";
            this.txtComments.Size = new System.Drawing.Size(351, 65);
            this.txtComments.TabIndex = 11;
            // 
            // txtAgreementDate
            // 
            this.txtAgreementDate.Location = new System.Drawing.Point(132, 47);
            this.txtAgreementDate.Name = "txtAgreementDate";
            this.txtAgreementDate.ReadOnly = true;
            this.txtAgreementDate.Size = new System.Drawing.Size(323, 22);
            this.txtAgreementDate.TabIndex = 10;
            // 
            // txtCustomerAgent
            // 
            this.txtCustomerAgent.Location = new System.Drawing.Point(567, 21);
            this.txtCustomerAgent.Name = "txtCustomerAgent";
            this.txtCustomerAgent.Size = new System.Drawing.Size(149, 22);
            this.txtCustomerAgent.TabIndex = 9;
            // 
            // txtCustomer
            // 
            this.txtCustomer.Location = new System.Drawing.Point(293, 19);
            this.txtCustomer.Name = "txtCustomer";
            this.txtCustomer.ReadOnly = true;
            this.txtCustomer.Size = new System.Drawing.Size(144, 22);
            this.txtCustomer.TabIndex = 8;
            // 
            // txtWR
            // 
            this.txtWR.Location = new System.Drawing.Point(52, 19);
            this.txtWR.Name = "txtWR";
            this.txtWR.ReadOnly = true;
            this.txtWR.Size = new System.Drawing.Size(161, 22);
            this.txtWR.TabIndex = 7;
            // 
            // lblComments
            // 
            this.lblComments.AutoSize = true;
            this.lblComments.Location = new System.Drawing.Point(487, 50);
            this.lblComments.Name = "lblComments";
            this.lblComments.Size = new System.Drawing.Size(72, 16);
            this.lblComments.TabIndex = 6;
            this.lblComments.Text = "Comments";
            // 
            // lblServiceDate
            // 
            this.lblServiceDate.AutoSize = true;
            this.lblServiceDate.Location = new System.Drawing.Point(15, 75);
            this.lblServiceDate.Name = "lblServiceDate";
            this.lblServiceDate.Size = new System.Drawing.Size(86, 16);
            this.lblServiceDate.TabIndex = 5;
            this.lblServiceDate.Text = "Service Date";
            // 
            // lblAgreementDate
            // 
            this.lblAgreementDate.AutoSize = true;
            this.lblAgreementDate.Location = new System.Drawing.Point(15, 50);
            this.lblAgreementDate.Name = "lblAgreementDate";
            this.lblAgreementDate.Size = new System.Drawing.Size(106, 16);
            this.lblAgreementDate.TabIndex = 4;
            this.lblAgreementDate.Text = "Agreement Date";
            // 
            // lblCost
            // 
            this.lblCost.AutoSize = true;
            this.lblCost.Location = new System.Drawing.Point(731, 24);
            this.lblCost.Name = "lblCost";
            this.lblCost.Size = new System.Drawing.Size(35, 16);
            this.lblCost.TabIndex = 3;
            this.lblCost.Text = "Cost";
            // 
            // lblCustomerAgent
            // 
            this.lblCustomerAgent.AutoSize = true;
            this.lblCustomerAgent.Location = new System.Drawing.Point(452, 22);
            this.lblCustomerAgent.Name = "lblCustomerAgent";
            this.lblCustomerAgent.Size = new System.Drawing.Size(103, 16);
            this.lblCustomerAgent.TabIndex = 2;
            this.lblCustomerAgent.Text = "Customer Agent";
            // 
            // lblCustomer
            // 
            this.lblCustomer.AutoSize = true;
            this.lblCustomer.Location = new System.Drawing.Point(219, 22);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Size = new System.Drawing.Size(65, 16);
            this.lblCustomer.TabIndex = 1;
            this.lblCustomer.Text = "Customer";
            // 
            // lblWR
            // 
            this.lblWR.AutoSize = true;
            this.lblWR.Location = new System.Drawing.Point(15, 22);
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
            this.dgvMSLA.Location = new System.Drawing.Point(12, 137);
            this.dgvMSLA.Name = "dgvMSLA";
            this.dgvMSLA.RowTemplate.Height = 24;
            this.dgvMSLA.Size = new System.Drawing.Size(1345, 168);
            this.dgvMSLA.TabIndex = 6;
            this.dgvMSLA.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvMSLA_KeyDown);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnGenerate);
            this.panel1.Location = new System.Drawing.Point(12, 308);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1345, 33);
            this.panel1.TabIndex = 7;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(1243, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(102, 26);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(1135, 4);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(102, 26);
            this.btnGenerate.TabIndex = 9;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // FormWithMSLA
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1371, 357);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dgvMSLA);
            this.Controls.Add(this.grpBoxControls);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1387, 396);
            this.Name = "FormWithMSLA";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Generate Street Light Supplemental Agreement";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormWithMSLA_FormClosing);
            this.Load += new System.EventHandler(this.FormWithMSLA_Load);
            this.grpBoxControls.ResumeLayout(false);
            this.grpBoxControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMSLA)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox grpBoxControls;
        private System.Windows.Forms.DateTimePicker dtPicker;
        private System.Windows.Forms.TextBox txtCost;
        private System.Windows.Forms.TextBox txtComments;
        private System.Windows.Forms.TextBox txtAgreementDate;
        private System.Windows.Forms.TextBox txtCustomerAgent;
        private System.Windows.Forms.TextBox txtCustomer;
        private System.Windows.Forms.TextBox txtWR;
        private System.Windows.Forms.Label lblComments;
        private System.Windows.Forms.Label lblServiceDate;
        private System.Windows.Forms.Label lblAgreementDate;
        private System.Windows.Forms.Label lblCost;
        private System.Windows.Forms.Label lblCustomerAgent;
        private System.Windows.Forms.Label lblCustomer;
        private System.Windows.Forms.Label lblWR;
        private System.Windows.Forms.DataGridView dgvMSLA;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnGenerate;
    }
}