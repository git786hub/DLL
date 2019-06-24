namespace GTechnology.Oncor.CustomAPI
{
    partial class frmStreetLightVoltageDrop
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvStreetLights = new System.Windows.Forms.DataGridView();
            this.cmdApply = new System.Windows.Forms.Button();
            this.cmdCalculate = new System.Windows.Forms.Button();
            this.cmdClose = new System.Windows.Forms.Button();
            this.lblNominalVoltage = new System.Windows.Forms.Label();
            this.txtActualSourceVoltage = new System.Windows.Forms.TextBox();
            this.lblActualSourceVoltage = new System.Windows.Forms.Label();
            this.txtAllowedMinimumVoltage = new System.Windows.Forms.TextBox();
            this.lblAllowedMinimumVoltage = new System.Windows.Forms.Label();
            this.cmdSaveReport = new System.Windows.Forms.Button();
            this.cmdPrintReport = new System.Windows.Forms.Button();
            this.cmdOptions = new System.Windows.Forms.Button();
            this.cboNominalVoltage = new System.Windows.Forms.ComboBox();
            this.slGridFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.slGridTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.slGridCable = new System.Windows.Forms.DataGridViewButtonColumn();
            this.slGridBallast = new System.Windows.Forms.DataGridViewButtonColumn();
            this.slGridLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.slGridNumberOfLights = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.slGridAmpsLight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.slGridLoadAmps = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.slGridVlmagVolts = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.slGridAllowedMin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.slGridVoltDrop = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.slGridTotalVDrop = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.slGridSecFID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SecFNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.slGridFID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.slGridCableAmpacity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CableCU = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LightCU = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CablePendingEdit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LightPendingEdit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LengthPendingEdit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CableAllowCUEdit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LightAllowCUEdit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStreetLights)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvStreetLights
            // 
            this.dgvStreetLights.AllowUserToAddRows = false;
            this.dgvStreetLights.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvStreetLights.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStreetLights.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.slGridFrom,
            this.slGridTo,
            this.slGridCable,
            this.slGridBallast,
            this.slGridLength,
            this.slGridNumberOfLights,
            this.slGridAmpsLight,
            this.slGridLoadAmps,
            this.slGridVlmagVolts,
            this.slGridAllowedMin,
            this.slGridVoltDrop,
            this.slGridTotalVDrop,
            this.slGridSecFID,
            this.SecFNO,
            this.slGridFID,
            this.slGridCableAmpacity,
            this.CableCU,
            this.LightCU,
            this.CablePendingEdit,
            this.LightPendingEdit,
            this.LengthPendingEdit,
            this.CableAllowCUEdit,
            this.LightAllowCUEdit});
            this.dgvStreetLights.Location = new System.Drawing.Point(12, 62);
            this.dgvStreetLights.Name = "dgvStreetLights";
            this.dgvStreetLights.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvStreetLights.Size = new System.Drawing.Size(695, 161);
            this.dgvStreetLights.TabIndex = 1;
            this.dgvStreetLights.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStreetLights_CellClick);
            this.dgvStreetLights.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStreetLights_CellValueChanged);
            this.dgvStreetLights.SelectionChanged += new System.EventHandler(this.dgvStreetLights_SelectionChanged);
            // 
            // cmdApply
            // 
            this.cmdApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdApply.Location = new System.Drawing.Point(472, 33);
            this.cmdApply.Name = "cmdApply";
            this.cmdApply.Size = new System.Drawing.Size(75, 23);
            this.cmdApply.TabIndex = 18;
            this.cmdApply.Text = "Apply";
            this.cmdApply.UseVisualStyleBackColor = true;
            this.cmdApply.Click += new System.EventHandler(this.cmdApply_Click);
            // 
            // cmdCalculate
            // 
            this.cmdCalculate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCalculate.Location = new System.Drawing.Point(553, 33);
            this.cmdCalculate.Name = "cmdCalculate";
            this.cmdCalculate.Size = new System.Drawing.Size(75, 23);
            this.cmdCalculate.TabIndex = 17;
            this.cmdCalculate.Text = "Calculate";
            this.cmdCalculate.UseVisualStyleBackColor = true;
            this.cmdCalculate.Click += new System.EventHandler(this.cmdCalculate_Click);
            // 
            // cmdClose
            // 
            this.cmdClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdClose.Location = new System.Drawing.Point(634, 33);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(75, 23);
            this.cmdClose.TabIndex = 16;
            this.cmdClose.Text = "Close";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // lblNominalVoltage
            // 
            this.lblNominalVoltage.AutoSize = true;
            this.lblNominalVoltage.Location = new System.Drawing.Point(12, 9);
            this.lblNominalVoltage.Name = "lblNominalVoltage";
            this.lblNominalVoltage.Size = new System.Drawing.Size(87, 13);
            this.lblNominalVoltage.TabIndex = 19;
            this.lblNominalVoltage.Text = "Nominal Voltage:";
            // 
            // txtActualSourceVoltage
            // 
            this.txtActualSourceVoltage.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtActualSourceVoltage.Location = new System.Drawing.Point(400, 6);
            this.txtActualSourceVoltage.Name = "txtActualSourceVoltage";
            this.txtActualSourceVoltage.Size = new System.Drawing.Size(37, 20);
            this.txtActualSourceVoltage.TabIndex = 22;
            this.txtActualSourceVoltage.Text = "252";
            this.txtActualSourceVoltage.TextChanged += new System.EventHandler(this.txtActualSourceVoltage_TextChanged);
            this.txtActualSourceVoltage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtActualSourceVoltage_KeyPress);
            // 
            // lblActualSourceVoltage
            // 
            this.lblActualSourceVoltage.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblActualSourceVoltage.AutoSize = true;
            this.lblActualSourceVoltage.Location = new System.Drawing.Point(257, 10);
            this.lblActualSourceVoltage.Name = "lblActualSourceVoltage";
            this.lblActualSourceVoltage.Size = new System.Drawing.Size(137, 13);
            this.lblActualSourceVoltage.TabIndex = 21;
            this.lblActualSourceVoltage.Text = "Actual Source Voltage (Vs):";
            // 
            // txtAllowedMinimumVoltage
            // 
            this.txtAllowedMinimumVoltage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAllowedMinimumVoltage.Location = new System.Drawing.Point(677, 7);
            this.txtAllowedMinimumVoltage.Name = "txtAllowedMinimumVoltage";
            this.txtAllowedMinimumVoltage.Size = new System.Drawing.Size(29, 20);
            this.txtAllowedMinimumVoltage.TabIndex = 24;
            this.txtAllowedMinimumVoltage.Text = "95";
            this.txtAllowedMinimumVoltage.TextChanged += new System.EventHandler(this.txtAllowedMinimumVoltage_TextChanged);
            this.txtAllowedMinimumVoltage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAllowedMinimumVoltage_KeyPress);
            // 
            // lblAllowedMinimumVoltage
            // 
            this.lblAllowedMinimumVoltage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAllowedMinimumVoltage.AutoSize = true;
            this.lblAllowedMinimumVoltage.Location = new System.Drawing.Point(530, 10);
            this.lblAllowedMinimumVoltage.Name = "lblAllowedMinimumVoltage";
            this.lblAllowedMinimumVoltage.Size = new System.Drawing.Size(141, 13);
            this.lblAllowedMinimumVoltage.TabIndex = 23;
            this.lblAllowedMinimumVoltage.Text = "Allowed Minimum Voltage %:";
            // 
            // cmdSaveReport
            // 
            this.cmdSaveReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdSaveReport.Location = new System.Drawing.Point(391, 33);
            this.cmdSaveReport.Name = "cmdSaveReport";
            this.cmdSaveReport.Size = new System.Drawing.Size(75, 23);
            this.cmdSaveReport.TabIndex = 26;
            this.cmdSaveReport.Text = "Save Report";
            this.cmdSaveReport.UseVisualStyleBackColor = true;
            this.cmdSaveReport.Click += new System.EventHandler(this.cmdSaveReport_Click);
            // 
            // cmdPrintReport
            // 
            this.cmdPrintReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdPrintReport.Location = new System.Drawing.Point(310, 33);
            this.cmdPrintReport.Name = "cmdPrintReport";
            this.cmdPrintReport.Size = new System.Drawing.Size(75, 23);
            this.cmdPrintReport.TabIndex = 25;
            this.cmdPrintReport.Text = "Print Report";
            this.cmdPrintReport.UseVisualStyleBackColor = true;
            this.cmdPrintReport.Click += new System.EventHandler(this.cmdPrintReport_Click);
            // 
            // cmdOptions
            // 
            this.cmdOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOptions.Location = new System.Drawing.Point(229, 33);
            this.cmdOptions.Name = "cmdOptions";
            this.cmdOptions.Size = new System.Drawing.Size(75, 23);
            this.cmdOptions.TabIndex = 27;
            this.cmdOptions.Text = "Options";
            this.cmdOptions.UseVisualStyleBackColor = true;
            this.cmdOptions.Click += new System.EventHandler(this.cmdOptions_Click);
            // 
            // cboNominalVoltage
            // 
            this.cboNominalVoltage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNominalVoltage.FormattingEnabled = true;
            this.cboNominalVoltage.Location = new System.Drawing.Point(105, 6);
            this.cboNominalVoltage.Name = "cboNominalVoltage";
            this.cboNominalVoltage.Size = new System.Drawing.Size(48, 21);
            this.cboNominalVoltage.TabIndex = 28;
            this.cboNominalVoltage.SelectedValueChanged += new System.EventHandler(this.cboNominalVoltage_SelectedValueChanged);
            // 
            // slGridFrom
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro;
            this.slGridFrom.DefaultCellStyle = dataGridViewCellStyle1;
            this.slGridFrom.HeaderText = "From";
            this.slGridFrom.Name = "slGridFrom";
            this.slGridFrom.ReadOnly = true;
            this.slGridFrom.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.slGridFrom.Width = 32;
            // 
            // slGridTo
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Gainsboro;
            this.slGridTo.DefaultCellStyle = dataGridViewCellStyle2;
            this.slGridTo.HeaderText = "To";
            this.slGridTo.Name = "slGridTo";
            this.slGridTo.ReadOnly = true;
            this.slGridTo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.slGridTo.Width = 25;
            // 
            // slGridCable
            // 
            this.slGridCable.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.slGridCable.HeaderText = "Cable";
            this.slGridCable.Name = "slGridCable";
            this.slGridCable.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.slGridCable.Width = 140;
            // 
            // slGridBallast
            // 
            this.slGridBallast.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.slGridBallast.HeaderText = "Ballast";
            this.slGridBallast.Name = "slGridBallast";
            this.slGridBallast.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.slGridBallast.Width = 140;
            // 
            // slGridLength
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            this.slGridLength.DefaultCellStyle = dataGridViewCellStyle3;
            this.slGridLength.HeaderText = "Length (ft)";
            this.slGridLength.Name = "slGridLength";
            this.slGridLength.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.slGridLength.Width = 45;
            // 
            // slGridNumberOfLights
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Gainsboro;
            this.slGridNumberOfLights.DefaultCellStyle = dataGridViewCellStyle4;
            this.slGridNumberOfLights.HeaderText = "# of Lights";
            this.slGridNumberOfLights.Name = "slGridNumberOfLights";
            this.slGridNumberOfLights.ReadOnly = true;
            this.slGridNumberOfLights.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.slGridNumberOfLights.Width = 40;
            // 
            // slGridAmpsLight
            // 
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle5.Format = "N2";
            dataGridViewCellStyle5.NullValue = null;
            this.slGridAmpsLight.DefaultCellStyle = dataGridViewCellStyle5;
            this.slGridAmpsLight.HeaderText = "Amps / Light";
            this.slGridAmpsLight.Name = "slGridAmpsLight";
            this.slGridAmpsLight.ReadOnly = true;
            this.slGridAmpsLight.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.slGridAmpsLight.Width = 45;
            // 
            // slGridLoadAmps
            // 
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle6.Format = "N2";
            this.slGridLoadAmps.DefaultCellStyle = dataGridViewCellStyle6;
            this.slGridLoadAmps.HeaderText = "Load Amps";
            this.slGridLoadAmps.Name = "slGridLoadAmps";
            this.slGridLoadAmps.ReadOnly = true;
            this.slGridLoadAmps.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.slGridLoadAmps.Width = 40;
            // 
            // slGridVlmagVolts
            // 
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.Gainsboro;
            this.slGridVlmagVolts.DefaultCellStyle = dataGridViewCellStyle7;
            this.slGridVlmagVolts.HeaderText = "Vlmag Volts";
            this.slGridVlmagVolts.Name = "slGridVlmagVolts";
            this.slGridVlmagVolts.ReadOnly = true;
            this.slGridVlmagVolts.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.slGridVlmagVolts.Width = 45;
            // 
            // slGridAllowedMin
            // 
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle8.Format = "N2";
            this.slGridAllowedMin.DefaultCellStyle = dataGridViewCellStyle8;
            this.slGridAllowedMin.HeaderText = "Allowed Min";
            this.slGridAllowedMin.Name = "slGridAllowedMin";
            this.slGridAllowedMin.ReadOnly = true;
            this.slGridAllowedMin.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.slGridAllowedMin.Width = 45;
            // 
            // slGridVoltDrop
            // 
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle9.Format = "P2";
            dataGridViewCellStyle9.NullValue = null;
            this.slGridVoltDrop.DefaultCellStyle = dataGridViewCellStyle9;
            this.slGridVoltDrop.HeaderText = "Vdrop";
            this.slGridVoltDrop.Name = "slGridVoltDrop";
            this.slGridVoltDrop.ReadOnly = true;
            this.slGridVoltDrop.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.slGridVoltDrop.Width = 45;
            // 
            // slGridTotalVDrop
            // 
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle10.Format = "P2";
            dataGridViewCellStyle10.NullValue = null;
            this.slGridTotalVDrop.DefaultCellStyle = dataGridViewCellStyle10;
            this.slGridTotalVDrop.HeaderText = "Total Vdrop";
            this.slGridTotalVDrop.Name = "slGridTotalVDrop";
            this.slGridTotalVDrop.ReadOnly = true;
            this.slGridTotalVDrop.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.slGridTotalVDrop.Width = 40;
            // 
            // slGridSecFID
            // 
            this.slGridSecFID.HeaderText = "SecFID";
            this.slGridSecFID.Name = "slGridSecFID";
            this.slGridSecFID.ReadOnly = true;
            this.slGridSecFID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.slGridSecFID.Visible = false;
            // 
            // SecFNO
            // 
            this.SecFNO.HeaderText = "SecFNO";
            this.SecFNO.Name = "SecFNO";
            this.SecFNO.ReadOnly = true;
            this.SecFNO.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SecFNO.Visible = false;
            // 
            // slGridFID
            // 
            this.slGridFID.HeaderText = "StreetLightFID";
            this.slGridFID.Name = "slGridFID";
            this.slGridFID.ReadOnly = true;
            this.slGridFID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.slGridFID.Visible = false;
            // 
            // slGridCableAmpacity
            // 
            this.slGridCableAmpacity.HeaderText = "CableAmpacity";
            this.slGridCableAmpacity.Name = "slGridCableAmpacity";
            this.slGridCableAmpacity.ReadOnly = true;
            this.slGridCableAmpacity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.slGridCableAmpacity.Visible = false;
            // 
            // CableCU
            // 
            this.CableCU.HeaderText = "CableCU";
            this.CableCU.Name = "CableCU";
            this.CableCU.ReadOnly = true;
            this.CableCU.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CableCU.Visible = false;
            // 
            // LightCU
            // 
            this.LightCU.HeaderText = "LightCU";
            this.LightCU.Name = "LightCU";
            this.LightCU.ReadOnly = true;
            this.LightCU.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LightCU.Visible = false;
            // 
            // CablePendingEdit
            // 
            this.CablePendingEdit.HeaderText = "CablePendingEdit";
            this.CablePendingEdit.Name = "CablePendingEdit";
            this.CablePendingEdit.ReadOnly = true;
            this.CablePendingEdit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CablePendingEdit.Visible = false;
            // 
            // LightPendingEdit
            // 
            this.LightPendingEdit.HeaderText = "LightPendingEdit";
            this.LightPendingEdit.Name = "LightPendingEdit";
            this.LightPendingEdit.ReadOnly = true;
            this.LightPendingEdit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LightPendingEdit.Visible = false;
            // 
            // LengthPendingEdit
            // 
            this.LengthPendingEdit.HeaderText = "LengthPendingEdit";
            this.LengthPendingEdit.Name = "LengthPendingEdit";
            this.LengthPendingEdit.ReadOnly = true;
            this.LengthPendingEdit.Visible = false;
            // 
            // CableAllowCUEdit
            // 
            this.CableAllowCUEdit.HeaderText = "CableAllowCUEdit";
            this.CableAllowCUEdit.Name = "CableAllowCUEdit";
            this.CableAllowCUEdit.Visible = false;
            // 
            // LightAllowCUEdit
            // 
            this.LightAllowCUEdit.HeaderText = "LightAllowCUEdit";
            this.LightAllowCUEdit.Name = "LightAllowCUEdit";
            this.LightAllowCUEdit.Visible = false;
            // 
            // frmStreetLightVoltageDrop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(713, 229);
            this.Controls.Add(this.cboNominalVoltage);
            this.Controls.Add(this.cmdOptions);
            this.Controls.Add(this.cmdSaveReport);
            this.Controls.Add(this.cmdPrintReport);
            this.Controls.Add(this.txtAllowedMinimumVoltage);
            this.Controls.Add(this.lblAllowedMinimumVoltage);
            this.Controls.Add(this.txtActualSourceVoltage);
            this.Controls.Add(this.lblActualSourceVoltage);
            this.Controls.Add(this.lblNominalVoltage);
            this.Controls.Add(this.cmdApply);
            this.Controls.Add(this.cmdCalculate);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.dgvStreetLights);
            this.MaximizeBox = false;
            this.Name = "frmStreetLightVoltageDrop";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Street Light Voltage Drop";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmStreetLightVoltageDrop_FormClosing);
            this.Load += new System.EventHandler(this.frmStreetLightVoltageDrop_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStreetLights)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvStreetLights;
        private System.Windows.Forms.Button cmdApply;
        private System.Windows.Forms.Button cmdCalculate;
        private System.Windows.Forms.Button cmdClose;
        private System.Windows.Forms.Label lblNominalVoltage;
        private System.Windows.Forms.TextBox txtActualSourceVoltage;
        private System.Windows.Forms.Label lblActualSourceVoltage;
        private System.Windows.Forms.TextBox txtAllowedMinimumVoltage;
        private System.Windows.Forms.Label lblAllowedMinimumVoltage;
        private System.Windows.Forms.Button cmdSaveReport;
        private System.Windows.Forms.Button cmdPrintReport;
        private System.Windows.Forms.Button cmdOptions;
        private System.Windows.Forms.ComboBox cboNominalVoltage;
        private System.Windows.Forms.DataGridViewTextBoxColumn slGridFrom;
        private System.Windows.Forms.DataGridViewTextBoxColumn slGridTo;
        private System.Windows.Forms.DataGridViewButtonColumn slGridCable;
        private System.Windows.Forms.DataGridViewButtonColumn slGridBallast;
        private System.Windows.Forms.DataGridViewTextBoxColumn slGridLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn slGridNumberOfLights;
        private System.Windows.Forms.DataGridViewTextBoxColumn slGridAmpsLight;
        private System.Windows.Forms.DataGridViewTextBoxColumn slGridLoadAmps;
        private System.Windows.Forms.DataGridViewTextBoxColumn slGridVlmagVolts;
        private System.Windows.Forms.DataGridViewTextBoxColumn slGridAllowedMin;
        private System.Windows.Forms.DataGridViewTextBoxColumn slGridVoltDrop;
        private System.Windows.Forms.DataGridViewTextBoxColumn slGridTotalVDrop;
        private System.Windows.Forms.DataGridViewTextBoxColumn slGridSecFID;
        private System.Windows.Forms.DataGridViewTextBoxColumn SecFNO;
        private System.Windows.Forms.DataGridViewTextBoxColumn slGridFID;
        private System.Windows.Forms.DataGridViewTextBoxColumn slGridCableAmpacity;
        private System.Windows.Forms.DataGridViewTextBoxColumn CableCU;
        private System.Windows.Forms.DataGridViewTextBoxColumn LightCU;
        private System.Windows.Forms.DataGridViewTextBoxColumn CablePendingEdit;
        private System.Windows.Forms.DataGridViewTextBoxColumn LightPendingEdit;
        private System.Windows.Forms.DataGridViewTextBoxColumn LengthPendingEdit;
        private System.Windows.Forms.DataGridViewTextBoxColumn CableAllowCUEdit;
        private System.Windows.Forms.DataGridViewTextBoxColumn LightAllowCUEdit;
    }
}