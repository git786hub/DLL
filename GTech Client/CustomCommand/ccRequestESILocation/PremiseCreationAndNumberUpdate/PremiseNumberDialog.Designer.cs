namespace GTechnology.Oncor.CustomAPI
{
    partial class PremiseNumberDialog 
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvPremises = new System.Windows.Forms.DataGridView();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.chkUpdateMapWindow = new System.Windows.Forms.CheckBox();
            this.cmdSubmit = new System.Windows.Forms.Button();
            this.cmdSave = new System.Windows.Forms.Button();
            this.cmdExit = new System.Windows.Forms.Button();
            this.bindingSource2 = new System.Windows.Forms.BindingSource(this.components);
            this.Request = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.FID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RequestDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HouseNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HouseFraction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Direction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StreetName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StreetType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DirectionalTrailing = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.City = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ZipCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InsideCL = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Subdivision = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Underground = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.MarkedOrSubbed = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Pool = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.PathClear = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.StageOfCONST = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.PremiseNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EditedCells = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPremises)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource2)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvPremises
            // 
            this.dgvPremises.AllowUserToAddRows = false;
            this.dgvPremises.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPremises.AutoGenerateColumns = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPremises.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvPremises.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPremises.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Request,
            this.FID,
            this.RequestDate,
            this.HouseNumber,
            this.HouseFraction,
            this.Direction,
            this.StreetName,
            this.StreetType,
            this.DirectionalTrailing,
            this.Unit,
            this.City,
            this.ZipCode,
            this.InsideCL,
            this.Subdivision,
            this.Underground,
            this.MarkedOrSubbed,
            this.Pool,
            this.PathClear,
            this.StageOfCONST,
            this.PremiseNumber,
            this.EditedCells});
            this.dgvPremises.DataSource = this.bindingSource1;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPremises.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvPremises.Location = new System.Drawing.Point(12, 12);
            this.dgvPremises.Name = "dgvPremises";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPremises.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvPremises.RowHeadersWidth = 10;
            this.dgvPremises.Size = new System.Drawing.Size(1046, 241);
            this.dgvPremises.TabIndex = 0;
            this.dgvPremises.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPremises_CellClick);
            this.dgvPremises.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPremises_CellContentClick);
            this.dgvPremises.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPremises_CellEnter);
            this.dgvPremises.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvPremises_CellFormatting);
            this.dgvPremises.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPremises_CellLeave);
            this.dgvPremises.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvPremises_CellMouseDown);
            this.dgvPremises.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvPremises_CellMouseUp);
            this.dgvPremises.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPremises_CellValueChanged);
            this.dgvPremises.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvPremises_CurrentCellDirtyStateChanged);
            this.dgvPremises.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvPremises_DataError);
            this.dgvPremises.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvPremises_KeyDown);
            this.dgvPremises.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dgvPremises_KeyUp);
            // 
            // chkUpdateMapWindow
            // 
            this.chkUpdateMapWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkUpdateMapWindow.AutoSize = true;
            this.chkUpdateMapWindow.Location = new System.Drawing.Point(13, 264);
            this.chkUpdateMapWindow.Name = "chkUpdateMapWindow";
            this.chkUpdateMapWindow.Size = new System.Drawing.Size(127, 17);
            this.chkUpdateMapWindow.TabIndex = 1;
            this.chkUpdateMapWindow.Text = "Update Map Window";
            this.chkUpdateMapWindow.UseVisualStyleBackColor = true;
            this.chkUpdateMapWindow.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged_1);
            // 
            // cmdSubmit
            // 
            this.cmdSubmit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdSubmit.Enabled = false;
            this.cmdSubmit.Location = new System.Drawing.Point(821, 260);
            this.cmdSubmit.Name = "cmdSubmit";
            this.cmdSubmit.Size = new System.Drawing.Size(75, 23);
            this.cmdSubmit.TabIndex = 2;
            this.cmdSubmit.Text = "Submit";
            this.cmdSubmit.UseVisualStyleBackColor = true;
            this.cmdSubmit.Click += new System.EventHandler(this.cmdSubmit_Click);
            // 
            // cmdSave
            // 
            this.cmdSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdSave.Location = new System.Drawing.Point(902, 260);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(75, 23);
            this.cmdSave.TabIndex = 3;
            this.cmdSave.Text = "Save";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // cmdExit
            // 
            this.cmdExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdExit.Location = new System.Drawing.Point(983, 260);
            this.cmdExit.Name = "cmdExit";
            this.cmdExit.Size = new System.Drawing.Size(75, 23);
            this.cmdExit.TabIndex = 4;
            this.cmdExit.Text = "Exit";
            this.cmdExit.UseVisualStyleBackColor = true;
            this.cmdExit.Click += new System.EventHandler(this.cmdExit_Click);
            // 
            // Request
            // 
            this.Request.DataPropertyName = "Request";
            this.Request.FalseValue = "false";
            this.Request.HeaderText = "Request";
            this.Request.Name = "Request";
            this.Request.ReadOnly = true;
            this.Request.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Request.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Request.TrueValue = "true";
            this.Request.Width = 55;
            // 
            // FID
            // 
            this.FID.DataPropertyName = "G3E_FID";
            this.FID.HeaderText = "FID";
            this.FID.Name = "FID";
            this.FID.ReadOnly = true;
            this.FID.Visible = false;
            // 
            // RequestDate
            // 
            this.RequestDate.DataPropertyName = "PREMISE_REQUEST_DT";
            this.RequestDate.HeaderText = "Request Date";
            this.RequestDate.MaxInputLength = 10;
            this.RequestDate.Name = "RequestDate";
            this.RequestDate.ReadOnly = true;
            this.RequestDate.Width = 80;
            // 
            // HouseNumber
            // 
            this.HouseNumber.DataPropertyName = "HOUSE_NBR";
            this.HouseNumber.HeaderText = "House Number";
            this.HouseNumber.MaxInputLength = 10;
            this.HouseNumber.Name = "HouseNumber";
            this.HouseNumber.Width = 55;
            // 
            // HouseFraction
            // 
            this.HouseFraction.DataPropertyName = "HOUSE_FRACTION_NBR";
            this.HouseFraction.HeaderText = "House Fraction";
            this.HouseFraction.MaxInputLength = 10;
            this.HouseFraction.Name = "HouseFraction";
            this.HouseFraction.Width = 55;
            // 
            // Direction
            // 
            this.Direction.DataPropertyName = "DIR_LEADING_C";
            this.Direction.HeaderText = "Leading Direction";
            this.Direction.MaxInputLength = 3;
            this.Direction.Name = "Direction";
            // 
            // StreetName
            // 
            this.StreetName.DataPropertyName = "STREET_NM";
            this.StreetName.HeaderText = "Street Name";
            this.StreetName.MaxInputLength = 30;
            this.StreetName.Name = "StreetName";
            // 
            // StreetType
            // 
            this.StreetType.DataPropertyName = "STREET_TYPE_C";
            this.StreetType.HeaderText = "Street Type";
            this.StreetType.MaxInputLength = 10;
            this.StreetType.Name = "StreetType";
            this.StreetType.Width = 50;
            // 
            // DirectionalTrailing
            // 
            this.DirectionalTrailing.DataPropertyName = "DIR_TRAINLING_C";
            this.DirectionalTrailing.HeaderText = "Trailing Direction";
            this.DirectionalTrailing.MaxInputLength = 3;
            this.DirectionalTrailing.Name = "DirectionalTrailing";
            this.DirectionalTrailing.Width = 80;
            // 
            // Unit
            // 
            this.Unit.DataPropertyName = "UNIT_NBR";
            this.Unit.HeaderText = "Unit";
            this.Unit.MaxInputLength = 5;
            this.Unit.Name = "Unit";
            this.Unit.Width = 50;
            // 
            // City
            // 
            this.City.DataPropertyName = "CITY_C";
            this.City.HeaderText = "City";
            this.City.MaxInputLength = 30;
            this.City.Name = "City";
            // 
            // ZipCode
            // 
            this.ZipCode.DataPropertyName = "ZIP_C";
            this.ZipCode.HeaderText = "Zip Code";
            this.ZipCode.MaxInputLength = 10;
            this.ZipCode.Name = "ZipCode";
            // 
            // InsideCL
            // 
            this.InsideCL.DataPropertyName = "INSIDE_CITY_LIMITS_YN";
            this.InsideCL.FalseValue = "N";
            this.InsideCL.HeaderText = "Inside City Limits?";
            this.InsideCL.Name = "InsideCL";
            this.InsideCL.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.InsideCL.TrueValue = "Y";
            this.InsideCL.Width = 55;
            // 
            // Subdivision
            // 
            this.Subdivision.DataPropertyName = "Subdivision";
            this.Subdivision.HeaderText = "Subdivision";
            this.Subdivision.Name = "Subdivision";
            this.Subdivision.Width = 75;
            // 
            // Underground
            // 
            this.Underground.DataPropertyName = "Underground";
            this.Underground.HeaderText = "Underground?";
            this.Underground.Name = "Underground";
            this.Underground.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Underground.Width = 95;
            // 
            // MarkedOrSubbed
            // 
            this.MarkedOrSubbed.DataPropertyName = "MarkedOrSubbed";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle2.NullValue = false;
            this.MarkedOrSubbed.DefaultCellStyle = dataGridViewCellStyle2;
            this.MarkedOrSubbed.HeaderText = "Marked or Stubbed?";
            this.MarkedOrSubbed.Name = "MarkedOrSubbed";
            this.MarkedOrSubbed.ReadOnly = true;
            this.MarkedOrSubbed.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.MarkedOrSubbed.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.MarkedOrSubbed.Width = 70;
            // 
            // Pool
            // 
            this.Pool.DataPropertyName = "Pool";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle3.NullValue = false;
            this.Pool.DefaultCellStyle = dataGridViewCellStyle3;
            this.Pool.HeaderText = "Pool?";
            this.Pool.Name = "Pool";
            this.Pool.ReadOnly = true;
            this.Pool.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Pool.Width = 50;
            // 
            // PathClear
            // 
            this.PathClear.DataPropertyName = "PathClear";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle4.NullValue = false;
            this.PathClear.DefaultCellStyle = dataGridViewCellStyle4;
            this.PathClear.HeaderText = "Path Clear?";
            this.PathClear.Name = "PathClear";
            this.PathClear.ReadOnly = true;
            this.PathClear.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.PathClear.Width = 55;
            // 
            // StageOfCONST
            // 
            this.StageOfCONST.DataPropertyName = "StageOfConstruction";
            this.StageOfCONST.HeaderText = "Stage of CONST";
            this.StageOfCONST.Items.AddRange(new object[] {
            "Slab",
            "Frame",
            "Brick",
            "Complete"});
            this.StageOfCONST.Name = "StageOfCONST";
            this.StageOfCONST.ReadOnly = true;
            this.StageOfCONST.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // PremiseNumber
            // 
            this.PremiseNumber.DataPropertyName = "PREMISE_NBR";
            this.PremiseNumber.HeaderText = "ESI Location";
            this.PremiseNumber.Name = "PremiseNumber";
            this.PremiseNumber.ReadOnly = true;
            this.PremiseNumber.Visible = false;
            // 
            // EditedCells
            // 
            this.EditedCells.DataPropertyName = "EditedCells";
            this.EditedCells.HeaderText = "Edited Cells";
            this.EditedCells.Name = "EditedCells";
            this.EditedCells.ReadOnly = true;
            this.EditedCells.Visible = false;
            // 
            // PremiseNumberDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1070, 293);
            this.Controls.Add(this.cmdExit);
            this.Controls.Add(this.cmdSave);
            this.Controls.Add(this.cmdSubmit);
            this.Controls.Add(this.chkUpdateMapWindow);
            this.Controls.Add(this.dgvPremises);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PremiseNumberDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " Request ESI Location";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PremiseNumberDialog_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPremises)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvPremises;
        private System.Windows.Forms.CheckBox chkUpdateMapWindow;
        private System.Windows.Forms.Button cmdSubmit;
        private System.Windows.Forms.Button cmdSave;
        private System.Windows.Forms.Button cmdExit;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.BindingSource bindingSource2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Request;
        private System.Windows.Forms.DataGridViewTextBoxColumn FID;
        private System.Windows.Forms.DataGridViewTextBoxColumn RequestDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn HouseNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn HouseFraction;
        private System.Windows.Forms.DataGridViewTextBoxColumn Direction;
        private System.Windows.Forms.DataGridViewTextBoxColumn StreetName;
        private System.Windows.Forms.DataGridViewTextBoxColumn StreetType;
        private System.Windows.Forms.DataGridViewTextBoxColumn DirectionalTrailing;
        private System.Windows.Forms.DataGridViewTextBoxColumn Unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn City;
        private System.Windows.Forms.DataGridViewTextBoxColumn ZipCode;
        private System.Windows.Forms.DataGridViewCheckBoxColumn InsideCL;
        private System.Windows.Forms.DataGridViewTextBoxColumn Subdivision;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Underground;
        private System.Windows.Forms.DataGridViewCheckBoxColumn MarkedOrSubbed;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Pool;
        private System.Windows.Forms.DataGridViewCheckBoxColumn PathClear;
        private System.Windows.Forms.DataGridViewComboBoxColumn StageOfCONST;
        private System.Windows.Forms.DataGridViewTextBoxColumn PremiseNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn EditedCells;
    }
}