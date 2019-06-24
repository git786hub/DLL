namespace GTechnology.Oncor.CustomAPI
{
    partial class frmEditNjunsTicket
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
            this.txtStructureId = new System.Windows.Forms.TextBox();
            this.lblStructureId = new System.Windows.Forms.Label();
            this.grpBxTicket = new System.Windows.Forms.GroupBox();
            this.txtWr = new System.Windows.Forms.TextBox();
            this.txtType = new System.Windows.Forms.TextBox();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.txtCounty = new System.Windows.Forms.TextBox();
            this.txtPlace = new System.Windows.Forms.TextBox();
            this.txtHouseNum = new System.Windows.Forms.TextBox();
            this.txtStreetName = new System.Windows.Forms.TextBox();
            this.txtState = new System.Windows.Forms.TextBox();
            this.cmbNjunsMemCode = new System.Windows.Forms.ComboBox();
            this.cmbPoleOwner = new System.Windows.Forms.ComboBox();
            this.cmbStartDate = new System.Windows.Forms.ComboBox();
            this.cmbWorkReqDate = new System.Windows.Forms.ComboBox();
            this.cmbPriority = new System.Windows.Forms.ComboBox();
            this.lblWr = new System.Windows.Forms.Label();
            this.lblType = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblNjunsMemCode = new System.Windows.Forms.Label();
            this.lblPoleOwner = new System.Windows.Forms.Label();
            this.StartDate = new System.Windows.Forms.Label();
            this.lblWorkReqDate = new System.Windows.Forms.Label();
            this.lblState = new System.Windows.Forms.Label();
            this.lblCounty = new System.Windows.Forms.Label();
            this.lblPlace = new System.Windows.Forms.Label();
            this.lblHouseNum = new System.Windows.Forms.Label();
            this.lblStreetName = new System.Windows.Forms.Label();
            this.lblPriority = new System.Windows.Forms.Label();
            this.grpBxSteps = new System.Windows.Forms.GroupBox();
            this.dgvSteps = new System.Windows.Forms.DataGridView();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.lblRemarks = new System.Windows.Forms.Label();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnExpSubmission = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpBxTicket.SuspendLayout();
            this.grpBxSteps.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSteps)).BeginInit();
            this.SuspendLayout();
            // 
            // txtStructureId
            // 
            this.txtStructureId.Location = new System.Drawing.Point(129, 21);
            this.txtStructureId.Name = "txtStructureId";
            this.txtStructureId.Size = new System.Drawing.Size(139, 20);
            this.txtStructureId.TabIndex = 0;
            // 
            // lblStructureId
            // 
            this.lblStructureId.AutoSize = true;
            this.lblStructureId.Location = new System.Drawing.Point(57, 24);
            this.lblStructureId.Name = "lblStructureId";
            this.lblStructureId.Size = new System.Drawing.Size(64, 13);
            this.lblStructureId.TabIndex = 1;
            this.lblStructureId.Text = "Structure ID";
            // 
            // grpBxTicket
            // 
            this.grpBxTicket.Controls.Add(this.lblPriority);
            this.grpBxTicket.Controls.Add(this.lblStreetName);
            this.grpBxTicket.Controls.Add(this.lblHouseNum);
            this.grpBxTicket.Controls.Add(this.lblPlace);
            this.grpBxTicket.Controls.Add(this.lblCounty);
            this.grpBxTicket.Controls.Add(this.lblState);
            this.grpBxTicket.Controls.Add(this.lblWorkReqDate);
            this.grpBxTicket.Controls.Add(this.StartDate);
            this.grpBxTicket.Controls.Add(this.lblPoleOwner);
            this.grpBxTicket.Controls.Add(this.lblNjunsMemCode);
            this.grpBxTicket.Controls.Add(this.lblStatus);
            this.grpBxTicket.Controls.Add(this.lblType);
            this.grpBxTicket.Controls.Add(this.lblWr);
            this.grpBxTicket.Controls.Add(this.cmbPriority);
            this.grpBxTicket.Controls.Add(this.cmbWorkReqDate);
            this.grpBxTicket.Controls.Add(this.cmbStartDate);
            this.grpBxTicket.Controls.Add(this.cmbPoleOwner);
            this.grpBxTicket.Controls.Add(this.cmbNjunsMemCode);
            this.grpBxTicket.Controls.Add(this.txtState);
            this.grpBxTicket.Controls.Add(this.txtStreetName);
            this.grpBxTicket.Controls.Add(this.txtHouseNum);
            this.grpBxTicket.Controls.Add(this.txtPlace);
            this.grpBxTicket.Controls.Add(this.txtCounty);
            this.grpBxTicket.Controls.Add(this.txtStatus);
            this.grpBxTicket.Controls.Add(this.txtType);
            this.grpBxTicket.Controls.Add(this.txtWr);
            this.grpBxTicket.Controls.Add(this.lblStructureId);
            this.grpBxTicket.Controls.Add(this.txtStructureId);
            this.grpBxTicket.Location = new System.Drawing.Point(12, 12);
            this.grpBxTicket.Name = "grpBxTicket";
            this.grpBxTicket.Size = new System.Drawing.Size(279, 395);
            this.grpBxTicket.TabIndex = 2;
            this.grpBxTicket.TabStop = false;
            this.grpBxTicket.Text = "Ticket (PENDING)";
            // 
            // txtWr
            // 
            this.txtWr.Location = new System.Drawing.Point(129, 47);
            this.txtWr.Name = "txtWr";
            this.txtWr.Size = new System.Drawing.Size(139, 20);
            this.txtWr.TabIndex = 2;
            // 
            // txtType
            // 
            this.txtType.Location = new System.Drawing.Point(129, 73);
            this.txtType.Name = "txtType";
            this.txtType.Size = new System.Drawing.Size(139, 20);
            this.txtType.TabIndex = 3;
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(129, 99);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(139, 20);
            this.txtStatus.TabIndex = 4;
            // 
            // txtCounty
            // 
            this.txtCounty.Location = new System.Drawing.Point(129, 259);
            this.txtCounty.Name = "txtCounty";
            this.txtCounty.Size = new System.Drawing.Size(139, 20);
            this.txtCounty.TabIndex = 5;
            // 
            // txtPlace
            // 
            this.txtPlace.Location = new System.Drawing.Point(129, 285);
            this.txtPlace.Name = "txtPlace";
            this.txtPlace.Size = new System.Drawing.Size(139, 20);
            this.txtPlace.TabIndex = 6;
            // 
            // txtHouseNum
            // 
            this.txtHouseNum.Location = new System.Drawing.Point(129, 311);
            this.txtHouseNum.Name = "txtHouseNum";
            this.txtHouseNum.Size = new System.Drawing.Size(139, 20);
            this.txtHouseNum.TabIndex = 7;
            // 
            // txtStreetName
            // 
            this.txtStreetName.Location = new System.Drawing.Point(129, 337);
            this.txtStreetName.Name = "txtStreetName";
            this.txtStreetName.Size = new System.Drawing.Size(139, 20);
            this.txtStreetName.TabIndex = 8;
            // 
            // txtState
            // 
            this.txtState.Location = new System.Drawing.Point(129, 233);
            this.txtState.Name = "txtState";
            this.txtState.Size = new System.Drawing.Size(139, 20);
            this.txtState.TabIndex = 9;
            // 
            // cmbNjunsMemCode
            // 
            this.cmbNjunsMemCode.FormattingEnabled = true;
            this.cmbNjunsMemCode.Location = new System.Drawing.Point(129, 125);
            this.cmbNjunsMemCode.Name = "cmbNjunsMemCode";
            this.cmbNjunsMemCode.Size = new System.Drawing.Size(139, 21);
            this.cmbNjunsMemCode.TabIndex = 10;
            // 
            // cmbPoleOwner
            // 
            this.cmbPoleOwner.FormattingEnabled = true;
            this.cmbPoleOwner.Location = new System.Drawing.Point(129, 152);
            this.cmbPoleOwner.Name = "cmbPoleOwner";
            this.cmbPoleOwner.Size = new System.Drawing.Size(139, 21);
            this.cmbPoleOwner.TabIndex = 11;
            // 
            // cmbStartDate
            // 
            this.cmbStartDate.FormattingEnabled = true;
            this.cmbStartDate.Location = new System.Drawing.Point(129, 179);
            this.cmbStartDate.Name = "cmbStartDate";
            this.cmbStartDate.Size = new System.Drawing.Size(139, 21);
            this.cmbStartDate.TabIndex = 12;
            // 
            // cmbWorkReqDate
            // 
            this.cmbWorkReqDate.FormattingEnabled = true;
            this.cmbWorkReqDate.Location = new System.Drawing.Point(129, 206);
            this.cmbWorkReqDate.Name = "cmbWorkReqDate";
            this.cmbWorkReqDate.Size = new System.Drawing.Size(139, 21);
            this.cmbWorkReqDate.TabIndex = 13;
            // 
            // cmbPriority
            // 
            this.cmbPriority.FormattingEnabled = true;
            this.cmbPriority.Location = new System.Drawing.Point(129, 363);
            this.cmbPriority.Name = "cmbPriority";
            this.cmbPriority.Size = new System.Drawing.Size(139, 21);
            this.cmbPriority.TabIndex = 14;
            // 
            // lblWr
            // 
            this.lblWr.AutoSize = true;
            this.lblWr.Location = new System.Drawing.Point(88, 50);
            this.lblWr.Name = "lblWr";
            this.lblWr.Size = new System.Drawing.Size(33, 13);
            this.lblWr.TabIndex = 15;
            this.lblWr.Text = "WR#";
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(90, 76);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(31, 13);
            this.lblType.TabIndex = 16;
            this.lblType.Text = "Type";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(84, 102);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(37, 13);
            this.lblStatus.TabIndex = 17;
            this.lblStatus.Text = "Status";
            // 
            // lblNjunsMemCode
            // 
            this.lblNjunsMemCode.AutoSize = true;
            this.lblNjunsMemCode.Location = new System.Drawing.Point(9, 128);
            this.lblNjunsMemCode.Name = "lblNjunsMemCode";
            this.lblNjunsMemCode.Size = new System.Drawing.Size(112, 13);
            this.lblNjunsMemCode.TabIndex = 18;
            this.lblNjunsMemCode.Text = "NJUNS Member Code";
            // 
            // lblPoleOwner
            // 
            this.lblPoleOwner.AutoSize = true;
            this.lblPoleOwner.Location = new System.Drawing.Point(59, 155);
            this.lblPoleOwner.Name = "lblPoleOwner";
            this.lblPoleOwner.Size = new System.Drawing.Size(62, 13);
            this.lblPoleOwner.TabIndex = 19;
            this.lblPoleOwner.Text = "Pole Owner";
            // 
            // StartDate
            // 
            this.StartDate.AutoSize = true;
            this.StartDate.Location = new System.Drawing.Point(66, 182);
            this.StartDate.Name = "StartDate";
            this.StartDate.Size = new System.Drawing.Size(55, 13);
            this.StartDate.TabIndex = 20;
            this.StartDate.Text = "Start Date";
            // 
            // lblWorkReqDate
            // 
            this.lblWorkReqDate.AutoSize = true;
            this.lblWorkReqDate.Location = new System.Drawing.Point(5, 209);
            this.lblWorkReqDate.Name = "lblWorkReqDate";
            this.lblWorkReqDate.Size = new System.Drawing.Size(116, 13);
            this.lblWorkReqDate.TabIndex = 21;
            this.lblWorkReqDate.Text = "Work Requested Code";
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.Location = new System.Drawing.Point(89, 236);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(32, 13);
            this.lblState.TabIndex = 22;
            this.lblState.Text = "State";
            // 
            // lblCounty
            // 
            this.lblCounty.AutoSize = true;
            this.lblCounty.Location = new System.Drawing.Point(81, 262);
            this.lblCounty.Name = "lblCounty";
            this.lblCounty.Size = new System.Drawing.Size(40, 13);
            this.lblCounty.TabIndex = 23;
            this.lblCounty.Text = "County";
            // 
            // lblPlace
            // 
            this.lblPlace.AutoSize = true;
            this.lblPlace.Location = new System.Drawing.Point(87, 288);
            this.lblPlace.Name = "lblPlace";
            this.lblPlace.Size = new System.Drawing.Size(34, 13);
            this.lblPlace.TabIndex = 24;
            this.lblPlace.Text = "Place";
            // 
            // lblHouseNum
            // 
            this.lblHouseNum.AutoSize = true;
            this.lblHouseNum.Location = new System.Drawing.Point(43, 314);
            this.lblHouseNum.Name = "lblHouseNum";
            this.lblHouseNum.Size = new System.Drawing.Size(78, 13);
            this.lblHouseNum.TabIndex = 25;
            this.lblHouseNum.Text = "House Number";
            // 
            // lblStreetName
            // 
            this.lblStreetName.AutoSize = true;
            this.lblStreetName.Location = new System.Drawing.Point(55, 340);
            this.lblStreetName.Name = "lblStreetName";
            this.lblStreetName.Size = new System.Drawing.Size(66, 13);
            this.lblStreetName.TabIndex = 26;
            this.lblStreetName.Text = "Street Name";
            // 
            // lblPriority
            // 
            this.lblPriority.AutoSize = true;
            this.lblPriority.Location = new System.Drawing.Point(83, 366);
            this.lblPriority.Name = "lblPriority";
            this.lblPriority.Size = new System.Drawing.Size(38, 13);
            this.lblPriority.TabIndex = 27;
            this.lblPriority.Text = "Priority";
            // 
            // grpBxSteps
            // 
            this.grpBxSteps.Controls.Add(this.btnDown);
            this.grpBxSteps.Controls.Add(this.btnUp);
            this.grpBxSteps.Controls.Add(this.dgvSteps);
            this.grpBxSteps.Location = new System.Drawing.Point(306, 13);
            this.grpBxSteps.Name = "grpBxSteps";
            this.grpBxSteps.Size = new System.Drawing.Size(623, 286);
            this.grpBxSteps.TabIndex = 3;
            this.grpBxSteps.TabStop = false;
            this.grpBxSteps.Text = "Steps";
            // 
            // dgvSteps
            // 
            this.dgvSteps.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSteps.Location = new System.Drawing.Point(6, 17);
            this.dgvSteps.Name = "dgvSteps";
            this.dgvSteps.Size = new System.Drawing.Size(558, 261);
            this.dgvSteps.TabIndex = 0;
            // 
            // txtRemarks
            // 
            this.txtRemarks.Location = new System.Drawing.Point(312, 324);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(406, 72);
            this.txtRemarks.TabIndex = 4;
            // 
            // lblRemarks
            // 
            this.lblRemarks.AutoSize = true;
            this.lblRemarks.Location = new System.Drawing.Point(309, 304);
            this.lblRemarks.Name = "lblRemarks";
            this.lblRemarks.Size = new System.Drawing.Size(49, 13);
            this.lblRemarks.TabIndex = 5;
            this.lblRemarks.Text = "Remarks";
            // 
            // btnUp
            // 
            this.btnUp.Font = new System.Drawing.Font("Wingdings 3", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnUp.Location = new System.Drawing.Point(571, 90);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(30, 28);
            this.btnUp.TabIndex = 1;
            this.btnUp.Text = "h";
            this.btnUp.UseVisualStyleBackColor = true;
            // 
            // btnDown
            // 
            this.btnDown.Font = new System.Drawing.Font("Wingdings 3", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnDown.Location = new System.Drawing.Point(571, 127);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(30, 28);
            this.btnDown.TabIndex = 2;
            this.btnDown.Text = "i";
            this.btnDown.UseVisualStyleBackColor = true;
            // 
            // btnExpSubmission
            // 
            this.btnExpSubmission.Location = new System.Drawing.Point(777, 322);
            this.btnExpSubmission.Name = "btnExpSubmission";
            this.btnExpSubmission.Size = new System.Drawing.Size(152, 23);
            this.btnExpSubmission.TabIndex = 6;
            this.btnExpSubmission.Text = "Expedite Submission";
            this.btnExpSubmission.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(777, 393);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(859, 393);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(70, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // frmEditNjunsTicket
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(939, 425);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnExpSubmission);
            this.Controls.Add(this.lblRemarks);
            this.Controls.Add(this.txtRemarks);
            this.Controls.Add(this.grpBxSteps);
            this.Controls.Add(this.grpBxTicket);
            this.Name = "frmEditNjunsTicket";
            this.ShowIcon = false;
            this.Text = "Edit NJUNS Ticket";
            this.Load += new System.EventHandler(this.frmEditNjunsTicket_Load);
            this.grpBxTicket.ResumeLayout(false);
            this.grpBxTicket.PerformLayout();
            this.grpBxSteps.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSteps)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtStructureId;
        private System.Windows.Forms.Label lblStructureId;
        private System.Windows.Forms.GroupBox grpBxTicket;
        private System.Windows.Forms.ComboBox cmbPriority;
        private System.Windows.Forms.ComboBox cmbWorkReqDate;
        private System.Windows.Forms.ComboBox cmbStartDate;
        private System.Windows.Forms.ComboBox cmbPoleOwner;
        private System.Windows.Forms.ComboBox cmbNjunsMemCode;
        private System.Windows.Forms.TextBox txtState;
        private System.Windows.Forms.TextBox txtStreetName;
        private System.Windows.Forms.TextBox txtHouseNum;
        private System.Windows.Forms.TextBox txtPlace;
        private System.Windows.Forms.TextBox txtCounty;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.TextBox txtType;
        private System.Windows.Forms.TextBox txtWr;
        private System.Windows.Forms.Label lblPriority;
        private System.Windows.Forms.Label lblStreetName;
        private System.Windows.Forms.Label lblHouseNum;
        private System.Windows.Forms.Label lblPlace;
        private System.Windows.Forms.Label lblCounty;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.Label lblWorkReqDate;
        private System.Windows.Forms.Label StartDate;
        private System.Windows.Forms.Label lblPoleOwner;
        private System.Windows.Forms.Label lblNjunsMemCode;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Label lblWr;
        private System.Windows.Forms.GroupBox grpBxSteps;
        private System.Windows.Forms.DataGridView dgvSteps;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Label lblRemarks;
        private System.Windows.Forms.Button btnExpSubmission;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}