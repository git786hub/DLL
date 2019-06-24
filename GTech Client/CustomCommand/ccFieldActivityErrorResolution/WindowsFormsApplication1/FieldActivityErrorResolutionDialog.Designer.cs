namespace GTechnology.Oncor.CustomAPI
{
    partial class FieldActivityErrorResolutionDialog
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
      this.components = new System.ComponentModel.Container();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FieldActivityErrorResolutionDialog));
      this.dgvErrorRecords = new System.Windows.Forms.DataGridView();
      this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
      this.ExitCmd = new System.Windows.Forms.Button();
      this.SubmitCmd = new System.Windows.Forms.Button();
      this.RefreshCmd = new System.Windows.Forms.Button();
      this.UpdateMap = new System.Windows.Forms.CheckBox();
      this.locateType = new System.Windows.Forms.Panel();
      this.optTransformer = new System.Windows.Forms.RadioButton();
      this.optStructureID = new System.Windows.Forms.RadioButton();
      this.optMeterGeocode = new System.Windows.Forms.RadioButton();
      this.optAddress = new System.Windows.Forms.RadioButton();
      this.optPremiseNumber = new System.Windows.Forms.RadioButton();
      this.FilterTitle = new System.Windows.Forms.Panel();
      this.FiltersLabel = new System.Windows.Forms.Label();
      this.dgv_Filters = new System.Windows.Forms.DataGridView();
      this.panel2 = new System.Windows.Forms.Panel();
      this.locationNavigator = new System.Windows.Forms.BindingNavigator(this.components);
      this.bindingNavigatorCountItem1 = new System.Windows.Forms.ToolStripLabel();
      this.bindingNavigatorMoveFirstItem1 = new System.Windows.Forms.ToolStripButton();
      this.bindingNavigatorMovePreviousItem1 = new System.Windows.Forms.ToolStripButton();
      this.bindingNavigatorSeparator3 = new System.Windows.Forms.ToolStripSeparator();
      this.bindingNavigatorPositionItem1 = new System.Windows.Forms.ToolStripTextBox();
      this.bindingNavigatorSeparator4 = new System.Windows.Forms.ToolStripSeparator();
      this.bindingNavigatorMoveNextItem1 = new System.Windows.Forms.ToolStripButton();
      this.bindingNavigatorMoveLastItem1 = new System.Windows.Forms.ToolStripButton();
      this.bindingNavigatorSeparator5 = new System.Windows.Forms.ToolStripSeparator();
      this.bindingSource2 = new System.Windows.Forms.BindingSource(this.components);
      this.ServiceActivityCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Status = new System.Windows.Forms.DataGridViewComboBoxColumn();
      this.CrewHQ = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Comment = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.OverrideTolerance = new System.Windows.Forms.DataGridViewCheckBoxColumn();
      this.StructureID = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.TransCompanyNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Error = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.EditDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.UserID = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.TransactionDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.TransactionType = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ActivityCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.MeterLatitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.MeterLongitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.LocateMethod = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.CU = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.PremiseNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Direction = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.HouseNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.HouseFractionNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.StreetName = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.StreetType = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.DirectionTrailing = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ManagementActivityCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.DwellingType = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Remarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.EditedCells = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.FLNX_H = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.FLNY_H = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ServiceActivityCodeFltr = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.fltrStatus = new System.Windows.Forms.DataGridViewButtonColumn();
      this.fltrCrewHQ = new System.Windows.Forms.DataGridViewComboBoxColumn();
      this.fltrComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.fltrOverrideTolerance = new System.Windows.Forms.DataGridViewCheckBoxColumn();
      this.fltrStructureID = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.fltrTransCompanyNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.fltrError = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.fltrEditDate = new System.Windows.Forms.DataGridViewButtonColumn();
      this.fltrUserID = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.FromTransactionDate = new System.Windows.Forms.DataGridViewButtonColumn();
      this.ToTransactionDate = new System.Windows.Forms.DataGridViewButtonColumn();
      this.fltrTransactionType = new System.Windows.Forms.DataGridViewComboBoxColumn();
      this.fltrActivityCode = new System.Windows.Forms.DataGridViewComboBoxColumn();
      this.fltrMeterLatitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.fltrMeterLongitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.fltrLocateMethod = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.fltrCU = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.fltrPremiseNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.fltrDirection = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.fltrHouseNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.fltrHouseFractionNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.fltrStreetName = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.fltrStreetType = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.fltrDirectionTrailing = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.fltrUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.fltrManagementActivityCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.fltrDwellingType = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.fltrRemarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
      ((System.ComponentModel.ISupportInitialize)(this.dgvErrorRecords)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
      this.locateType.SuspendLayout();
      this.FilterTitle.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dgv_Filters)).BeginInit();
      this.panel2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.locationNavigator)).BeginInit();
      this.locationNavigator.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.bindingSource2)).BeginInit();
      this.SuspendLayout();
      // 
      // dgvErrorRecords
      // 
      this.dgvErrorRecords.AllowUserToAddRows = false;
      this.dgvErrorRecords.AllowUserToDeleteRows = false;
      this.dgvErrorRecords.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.dgvErrorRecords.AutoGenerateColumns = false;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dgvErrorRecords.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.dgvErrorRecords.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvErrorRecords.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ServiceActivityCode,
            this.Status,
            this.CrewHQ,
            this.Comment,
            this.OverrideTolerance,
            this.StructureID,
            this.TransCompanyNum,
            this.Error,
            this.EditDate,
            this.UserID,
            this.TransactionDate,
            this.TransactionType,
            this.ActivityCode,
            this.MeterLatitude,
            this.MeterLongitude,
            this.LocateMethod,
            this.CU,
            this.PremiseNumber,
            this.Direction,
            this.HouseNumber,
            this.HouseFractionNumber,
            this.StreetName,
            this.StreetType,
            this.DirectionTrailing,
            this.Unit,
            this.ManagementActivityCode,
            this.DwellingType,
            this.Remarks,
            this.EditedCells,
            this.FLNX_H,
            this.FLNY_H});
      this.dgvErrorRecords.DataSource = this.bindingSource1;
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.dgvErrorRecords.DefaultCellStyle = dataGridViewCellStyle2;
      this.dgvErrorRecords.Location = new System.Drawing.Point(12, 53);
      this.dgvErrorRecords.Name = "dgvErrorRecords";
      dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dgvErrorRecords.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
      this.dgvErrorRecords.Size = new System.Drawing.Size(986, 226);
      this.dgvErrorRecords.TabIndex = 0;
      this.dgvErrorRecords.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvErrorRecords_CellClick);
      this.dgvErrorRecords.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvErrorRecords_CellEnter);
      this.dgvErrorRecords.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvErrorRecords_CellFormatting);
      this.dgvErrorRecords.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvErrorRecords_CellLeave);
      this.dgvErrorRecords.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvErrorRecords_CellPainting);
      this.dgvErrorRecords.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvErrorRecords_CellValueChanged);
      this.dgvErrorRecords.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgvErrorRecords_ColumnWidthChanged);
      this.dgvErrorRecords.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvErrorRecords_CurrentCellDirtyStateChanged);
      this.dgvErrorRecords.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvErrorRecords_Scroll);
      this.dgvErrorRecords.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvErrorRecords_Paint);
      // 
      // ExitCmd
      // 
      this.ExitCmd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.ExitCmd.Location = new System.Drawing.Point(920, 318);
      this.ExitCmd.Name = "ExitCmd";
      this.ExitCmd.Size = new System.Drawing.Size(75, 23);
      this.ExitCmd.TabIndex = 2;
      this.ExitCmd.Text = "Exit";
      this.ExitCmd.UseVisualStyleBackColor = true;
      this.ExitCmd.Click += new System.EventHandler(this.ExitCmd_Click);
      // 
      // SubmitCmd
      // 
      this.SubmitCmd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.SubmitCmd.Location = new System.Drawing.Point(839, 318);
      this.SubmitCmd.Name = "SubmitCmd";
      this.SubmitCmd.Size = new System.Drawing.Size(75, 23);
      this.SubmitCmd.TabIndex = 3;
      this.SubmitCmd.Text = "Submit";
      this.SubmitCmd.UseVisualStyleBackColor = true;
      this.SubmitCmd.Click += new System.EventHandler(this.SubmitCmd_Click);
      // 
      // RefreshCmd
      // 
      this.RefreshCmd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.RefreshCmd.Location = new System.Drawing.Point(758, 318);
      this.RefreshCmd.Name = "RefreshCmd";
      this.RefreshCmd.Size = new System.Drawing.Size(75, 23);
      this.RefreshCmd.TabIndex = 4;
      this.RefreshCmd.Text = "Refresh";
      this.RefreshCmd.UseVisualStyleBackColor = true;
      this.RefreshCmd.Click += new System.EventHandler(this.RefreshCmd_Click);
      // 
      // UpdateMap
      // 
      this.UpdateMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.UpdateMap.AutoSize = true;
      this.UpdateMap.Location = new System.Drawing.Point(12, 293);
      this.UpdateMap.Name = "UpdateMap";
      this.UpdateMap.Size = new System.Drawing.Size(127, 17);
      this.UpdateMap.TabIndex = 5;
      this.UpdateMap.Text = "Update Map Window";
      this.UpdateMap.UseVisualStyleBackColor = true;
      // 
      // locateType
      // 
      this.locateType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.locateType.Controls.Add(this.optTransformer);
      this.locateType.Controls.Add(this.optStructureID);
      this.locateType.Controls.Add(this.optMeterGeocode);
      this.locateType.Controls.Add(this.optAddress);
      this.locateType.Controls.Add(this.optPremiseNumber);
      this.locateType.Location = new System.Drawing.Point(6, 317);
      this.locateType.Name = "locateType";
      this.locateType.Size = new System.Drawing.Size(466, 35);
      this.locateType.TabIndex = 8;
      // 
      // optTransformer
      // 
      this.optTransformer.AutoSize = true;
      this.optTransformer.Location = new System.Drawing.Point(360, 5);
      this.optTransformer.Name = "optTransformer";
      this.optTransformer.Size = new System.Drawing.Size(81, 17);
      this.optTransformer.TabIndex = 10;
      this.optTransformer.TabStop = true;
      this.optTransformer.Text = "Transformer";
      this.optTransformer.UseVisualStyleBackColor = true;
      // 
      // optStructureID
      // 
      this.optStructureID.AutoSize = true;
      this.optStructureID.Location = new System.Drawing.Point(272, 5);
      this.optStructureID.Name = "optStructureID";
      this.optStructureID.Size = new System.Drawing.Size(82, 17);
      this.optStructureID.TabIndex = 9;
      this.optStructureID.TabStop = true;
      this.optStructureID.Text = "Structure ID";
      this.optStructureID.UseVisualStyleBackColor = true;
      // 
      // optMeterGeocode
      // 
      this.optMeterGeocode.AutoSize = true;
      this.optMeterGeocode.Location = new System.Drawing.Point(167, 5);
      this.optMeterGeocode.Name = "optMeterGeocode";
      this.optMeterGeocode.Size = new System.Drawing.Size(99, 17);
      this.optMeterGeocode.TabIndex = 8;
      this.optMeterGeocode.TabStop = true;
      this.optMeterGeocode.Text = "Meter Geocode";
      this.optMeterGeocode.UseVisualStyleBackColor = true;
      // 
      // optAddress
      // 
      this.optAddress.AutoSize = true;
      this.optAddress.Location = new System.Drawing.Point(98, 5);
      this.optAddress.Name = "optAddress";
      this.optAddress.Size = new System.Drawing.Size(63, 17);
      this.optAddress.TabIndex = 7;
      this.optAddress.TabStop = true;
      this.optAddress.Text = "Address";
      this.optAddress.UseVisualStyleBackColor = true;
      // 
      // optPremiseNumber
      // 
      this.optPremiseNumber.AutoSize = true;
      this.optPremiseNumber.Location = new System.Drawing.Point(6, 5);
      this.optPremiseNumber.Name = "optPremiseNumber";
      this.optPremiseNumber.Size = new System.Drawing.Size(86, 17);
      this.optPremiseNumber.TabIndex = 6;
      this.optPremiseNumber.TabStop = true;
      this.optPremiseNumber.Text = "ESI Location";
      this.optPremiseNumber.UseVisualStyleBackColor = false;
      // 
      // FilterTitle
      // 
      this.FilterTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.FilterTitle.BackColor = System.Drawing.SystemColors.ScrollBar;
      this.FilterTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.FilterTitle.Controls.Add(this.FiltersLabel);
      this.FilterTitle.Location = new System.Drawing.Point(12, 12);
      this.FilterTitle.Name = "FilterTitle";
      this.FilterTitle.Size = new System.Drawing.Size(983, 19);
      this.FilterTitle.TabIndex = 9;
      // 
      // FiltersLabel
      // 
      this.FiltersLabel.AutoSize = true;
      this.FiltersLabel.Location = new System.Drawing.Point(3, 4);
      this.FiltersLabel.Name = "FiltersLabel";
      this.FiltersLabel.Size = new System.Drawing.Size(34, 13);
      this.FiltersLabel.TabIndex = 0;
      this.FiltersLabel.Text = "Filters";
      // 
      // dgv_Filters
      // 
      this.dgv_Filters.AllowUserToAddRows = false;
      this.dgv_Filters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dgv_Filters.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
      this.dgv_Filters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgv_Filters.ColumnHeadersVisible = false;
      this.dgv_Filters.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ServiceActivityCodeFltr,
            this.fltrStatus,
            this.fltrCrewHQ,
            this.fltrComment,
            this.fltrOverrideTolerance,
            this.fltrStructureID,
            this.fltrTransCompanyNum,
            this.fltrError,
            this.fltrEditDate,
            this.fltrUserID,
            this.FromTransactionDate,
            this.ToTransactionDate,
            this.fltrTransactionType,
            this.fltrActivityCode,
            this.fltrMeterLatitude,
            this.fltrMeterLongitude,
            this.fltrLocateMethod,
            this.fltrCU,
            this.fltrPremiseNumber,
            this.fltrDirection,
            this.fltrHouseNumber,
            this.fltrHouseFractionNumber,
            this.fltrStreetName,
            this.fltrStreetType,
            this.fltrDirectionTrailing,
            this.fltrUnit,
            this.fltrManagementActivityCode,
            this.fltrDwellingType,
            this.fltrRemarks});
      dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.dgv_Filters.DefaultCellStyle = dataGridViewCellStyle5;
      this.dgv_Filters.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
      this.dgv_Filters.Location = new System.Drawing.Point(12, 30);
      this.dgv_Filters.Name = "dgv_Filters";
      dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dgv_Filters.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
      this.dgv_Filters.ScrollBars = System.Windows.Forms.ScrollBars.None;
      this.dgv_Filters.Size = new System.Drawing.Size(983, 25);
      this.dgv_Filters.TabIndex = 10;
      this.dgv_Filters.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_Filters_CellContentClick);
      this.dgv_Filters.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_Filters_CellValueChanged);
      this.dgv_Filters.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgv_Filters_ColumnWidthChanged);
      this.dgv_Filters.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgv_Filters_CurrentCellDirtyStateChanged);
      // 
      // panel2
      // 
      this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.panel2.Controls.Add(this.locationNavigator);
      this.panel2.Location = new System.Drawing.Point(556, 317);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(188, 35);
      this.panel2.TabIndex = 13;
      // 
      // locationNavigator
      // 
      this.locationNavigator.AddNewItem = null;
      this.locationNavigator.AllowMerge = false;
      this.locationNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.locationNavigator.CountItem = this.bindingNavigatorCountItem1;
      this.locationNavigator.DeleteItem = null;
      this.locationNavigator.Dock = System.Windows.Forms.DockStyle.None;
      this.locationNavigator.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
      this.locationNavigator.ImageScalingSize = new System.Drawing.Size(20, 20);
      this.locationNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem1,
            this.bindingNavigatorMovePreviousItem1,
            this.bindingNavigatorSeparator3,
            this.bindingNavigatorPositionItem1,
            this.bindingNavigatorCountItem1,
            this.bindingNavigatorSeparator4,
            this.bindingNavigatorMoveNextItem1,
            this.bindingNavigatorMoveLastItem1,
            this.bindingNavigatorSeparator5});
      this.locationNavigator.Location = new System.Drawing.Point(0, -2);
      this.locationNavigator.MoveFirstItem = this.bindingNavigatorMoveFirstItem1;
      this.locationNavigator.MoveLastItem = this.bindingNavigatorMoveLastItem1;
      this.locationNavigator.MoveNextItem = this.bindingNavigatorMoveNextItem1;
      this.locationNavigator.MovePreviousItem = this.bindingNavigatorMovePreviousItem1;
      this.locationNavigator.Name = "locationNavigator";
      this.locationNavigator.PositionItem = this.bindingNavigatorPositionItem1;
      this.locationNavigator.Size = new System.Drawing.Size(204, 27);
      this.locationNavigator.TabIndex = 0;
      this.locationNavigator.Text = "bindingNavigator2";
      this.locationNavigator.MouseUp += new System.Windows.Forms.MouseEventHandler(this.bindingNavigator2_MouseClickTHING);
      // 
      // bindingNavigatorCountItem1
      // 
      this.bindingNavigatorCountItem1.Name = "bindingNavigatorCountItem1";
      this.bindingNavigatorCountItem1.Size = new System.Drawing.Size(35, 24);
      this.bindingNavigatorCountItem1.Text = "of {0}";
      this.bindingNavigatorCountItem1.ToolTipText = "Total number of items";
      // 
      // bindingNavigatorMoveFirstItem1
      // 
      this.bindingNavigatorMoveFirstItem1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.bindingNavigatorMoveFirstItem1.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem1.Image")));
      this.bindingNavigatorMoveFirstItem1.Name = "bindingNavigatorMoveFirstItem1";
      this.bindingNavigatorMoveFirstItem1.RightToLeftAutoMirrorImage = true;
      this.bindingNavigatorMoveFirstItem1.Size = new System.Drawing.Size(24, 24);
      this.bindingNavigatorMoveFirstItem1.Text = "Move first";
      // 
      // bindingNavigatorMovePreviousItem1
      // 
      this.bindingNavigatorMovePreviousItem1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.bindingNavigatorMovePreviousItem1.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem1.Image")));
      this.bindingNavigatorMovePreviousItem1.Name = "bindingNavigatorMovePreviousItem1";
      this.bindingNavigatorMovePreviousItem1.RightToLeftAutoMirrorImage = true;
      this.bindingNavigatorMovePreviousItem1.Size = new System.Drawing.Size(24, 24);
      this.bindingNavigatorMovePreviousItem1.Text = "Move previous";
      // 
      // bindingNavigatorSeparator3
      // 
      this.bindingNavigatorSeparator3.Name = "bindingNavigatorSeparator3";
      this.bindingNavigatorSeparator3.Size = new System.Drawing.Size(6, 27);
      // 
      // bindingNavigatorPositionItem1
      // 
      this.bindingNavigatorPositionItem1.AccessibleName = "Position";
      this.bindingNavigatorPositionItem1.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
      this.bindingNavigatorPositionItem1.AutoSize = false;
      this.bindingNavigatorPositionItem1.Name = "bindingNavigatorPositionItem1";
      this.bindingNavigatorPositionItem1.Size = new System.Drawing.Size(50, 23);
      this.bindingNavigatorPositionItem1.Text = "0";
      this.bindingNavigatorPositionItem1.ToolTipText = "Current position";
      // 
      // bindingNavigatorSeparator4
      // 
      this.bindingNavigatorSeparator4.Name = "bindingNavigatorSeparator4";
      this.bindingNavigatorSeparator4.Size = new System.Drawing.Size(6, 27);
      // 
      // bindingNavigatorMoveNextItem1
      // 
      this.bindingNavigatorMoveNextItem1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.bindingNavigatorMoveNextItem1.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem1.Image")));
      this.bindingNavigatorMoveNextItem1.Name = "bindingNavigatorMoveNextItem1";
      this.bindingNavigatorMoveNextItem1.RightToLeftAutoMirrorImage = true;
      this.bindingNavigatorMoveNextItem1.Size = new System.Drawing.Size(24, 24);
      this.bindingNavigatorMoveNextItem1.Text = "Move next";
      // 
      // bindingNavigatorMoveLastItem1
      // 
      this.bindingNavigatorMoveLastItem1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.bindingNavigatorMoveLastItem1.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem1.Image")));
      this.bindingNavigatorMoveLastItem1.Name = "bindingNavigatorMoveLastItem1";
      this.bindingNavigatorMoveLastItem1.RightToLeftAutoMirrorImage = true;
      this.bindingNavigatorMoveLastItem1.Size = new System.Drawing.Size(24, 24);
      this.bindingNavigatorMoveLastItem1.Text = "Move last";
      // 
      // bindingNavigatorSeparator5
      // 
      this.bindingNavigatorSeparator5.Name = "bindingNavigatorSeparator5";
      this.bindingNavigatorSeparator5.Size = new System.Drawing.Size(6, 27);
      // 
      // bindingSource2
      // 
      this.bindingSource2.CurrentChanged += new System.EventHandler(this.bindingSource2_CurrentChanged);
      // 
      // ServiceActivityCode
      // 
      this.ServiceActivityCode.DataPropertyName = "SERVICE_ACTIVITY_ID";
      this.ServiceActivityCode.HeaderText = "SVCActivityCode";
      this.ServiceActivityCode.Name = "ServiceActivityCode";
      this.ServiceActivityCode.Visible = false;
      this.ServiceActivityCode.Width = 5;
      // 
      // Status
      // 
      this.Status.DataPropertyName = "STATUS_C";
      this.Status.HeaderText = "Status";
      this.Status.Items.AddRange(new object[] {
            "FAILED",
            "QUEUED",
            "DELETED"});
      this.Status.Name = "Status";
      this.Status.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
      this.Status.Width = 77;
      // 
      // CrewHQ
      // 
      this.CrewHQ.DataPropertyName = "SERVICE_CENTER_CODE";
      this.CrewHQ.HeaderText = "Service Center";
      this.CrewHQ.Name = "CrewHQ";
      this.CrewHQ.ReadOnly = true;
      this.CrewHQ.Resizable = System.Windows.Forms.DataGridViewTriState.True;
      this.CrewHQ.Width = 119;
      // 
      // Comment
      // 
      this.Comment.DataPropertyName = "CORRECTION_COMMENTS";
      this.Comment.HeaderText = "Comment";
      this.Comment.Name = "Comment";
      this.Comment.Resizable = System.Windows.Forms.DataGridViewTriState.True;
      this.Comment.Width = 96;
      // 
      // OverrideTolerance
      // 
      this.OverrideTolerance.DataPropertyName = "OVERRIDE_TOLERANCE";
      this.OverrideTolerance.HeaderText = "Override Tolerance";
      this.OverrideTolerance.Name = "OverrideTolerance";
      this.OverrideTolerance.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
      this.OverrideTolerance.Width = 146;
      // 
      // StructureID
      // 
      this.StructureID.DataPropertyName = "STRUCT_ID";
      this.StructureID.HeaderText = "Structure ID";
      this.StructureID.Name = "StructureID";
      this.StructureID.Resizable = System.Windows.Forms.DataGridViewTriState.True;
      this.StructureID.Width = 103;
      // 
      // TransCompanyNum
      // 
      this.TransCompanyNum.DataPropertyName = "TRF_CO_H";
      this.TransCompanyNum.HeaderText = "Transformer Company #";
      this.TransCompanyNum.Name = "TransCompanyNum";
      this.TransCompanyNum.Width = 173;
      // 
      // Error
      // 
      this.Error.DataPropertyName = "MSG_T";
      this.Error.HeaderText = "Error";
      this.Error.Name = "Error";
      this.Error.ReadOnly = true;
      this.Error.Width = 150;
      // 
      // EditDate
      // 
      this.EditDate.DataPropertyName = "EDIT_DATE";
      this.EditDate.HeaderText = "Edit Date";
      this.EditDate.Name = "EditDate";
      this.EditDate.ReadOnly = true;
      this.EditDate.Width = 88;
      // 
      // UserID
      // 
      this.UserID.DataPropertyName = "USER_ID";
      this.UserID.HeaderText = "User ID";
      this.UserID.Name = "UserID";
      this.UserID.ReadOnly = true;
      this.UserID.Width = 78;
      // 
      // TransactionDate
      // 
      this.TransactionDate.DataPropertyName = "TRANS_DATE";
      this.TransactionDate.HeaderText = "Transaction Date";
      this.TransactionDate.Name = "TransactionDate";
      this.TransactionDate.ReadOnly = true;
      this.TransactionDate.Width = 150;
      // 
      // TransactionType
      // 
      this.TransactionType.DataPropertyName = "O_OR_U_CODE";
      this.TransactionType.HeaderText = "Transaction Type";
      this.TransactionType.Name = "TransactionType";
      this.TransactionType.ReadOnly = true;
      this.TransactionType.Width = 136;
      // 
      // ActivityCode
      // 
      this.ActivityCode.DataPropertyName = "SERVICE_INFO_CODE";
      this.ActivityCode.HeaderText = "Service Info Code";
      this.ActivityCode.Name = "ActivityCode";
      this.ActivityCode.ReadOnly = true;
      this.ActivityCode.Resizable = System.Windows.Forms.DataGridViewTriState.True;
      this.ActivityCode.Width = 120;
      // 
      // MeterLatitude
      // 
      this.MeterLatitude.DataPropertyName = "METER_LATITUDE";
      this.MeterLatitude.HeaderText = "Meter Latitude";
      this.MeterLatitude.Name = "MeterLatitude";
      this.MeterLatitude.ReadOnly = true;
      this.MeterLatitude.Width = 118;
      // 
      // MeterLongitude
      // 
      this.MeterLongitude.DataPropertyName = "METER_LONGITUDE";
      this.MeterLongitude.HeaderText = "Meter Longitude";
      this.MeterLongitude.Name = "MeterLongitude";
      this.MeterLongitude.ReadOnly = true;
      this.MeterLongitude.Width = 128;
      // 
      // LocateMethod
      // 
      this.LocateMethod.DataPropertyName = "GIS_LOCATE_METHOD";
      this.LocateMethod.HeaderText = "Locate Method";
      this.LocateMethod.Name = "LocateMethod";
      this.LocateMethod.ReadOnly = true;
      this.LocateMethod.Width = 120;
      // 
      // CU
      // 
      this.CU.DataPropertyName = "CU_ID";
      this.CU.HeaderText = "CU";
      this.CU.Name = "CU";
      this.CU.Width = 80;
      // 
      // PremiseNumber
      // 
      this.PremiseNumber.DataPropertyName = "ESI_LOCATION";
      this.PremiseNumber.HeaderText = "ESI Location";
      this.PremiseNumber.Name = "PremiseNumber";
      this.PremiseNumber.ReadOnly = true;
      this.PremiseNumber.Width = 107;
      // 
      // Direction
      // 
      this.Direction.DataPropertyName = "ADDR_LEAD_DIR_IND";
      this.Direction.HeaderText = "Leading Direction";
      this.Direction.Name = "Direction";
      this.Direction.ReadOnly = true;
      this.Direction.Width = 136;
      // 
      // HouseNumber
      // 
      this.HouseNumber.DataPropertyName = "HOUSE_NO";
      this.HouseNumber.HeaderText = "House Number";
      this.HouseNumber.Name = "HouseNumber";
      this.HouseNumber.ReadOnly = true;
      this.HouseNumber.Width = 121;
      // 
      // HouseFractionNumber
      // 
      this.HouseFractionNumber.DataPropertyName = "HOUSE_NO_FRACTION";
      this.HouseFractionNumber.HeaderText = "House Fraction Number";
      this.HouseFractionNumber.Name = "HouseFractionNumber";
      this.HouseFractionNumber.ReadOnly = true;
      this.HouseFractionNumber.Width = 126;
      // 
      // StreetName
      // 
      this.StreetName.DataPropertyName = "STREET_NAME";
      this.StreetName.HeaderText = "Street Name";
      this.StreetName.Name = "StreetName";
      this.StreetName.ReadOnly = true;
      this.StreetName.Width = 107;
      // 
      // StreetType
      // 
      this.StreetType.DataPropertyName = "STREET_TYPE";
      this.StreetType.HeaderText = "Street Type";
      this.StreetType.Name = "StreetType";
      this.StreetType.ReadOnly = true;
      this.StreetType.Width = 102;
      // 
      // DirectionTrailing
      // 
      this.DirectionTrailing.DataPropertyName = "ADDR_TRAIL_DIR_IND";
      this.DirectionTrailing.HeaderText = "Trailing Direction";
      this.DirectionTrailing.Name = "DirectionTrailing";
      this.DirectionTrailing.ReadOnly = true;
      this.DirectionTrailing.Width = 132;
      // 
      // Unit
      // 
      this.Unit.DataPropertyName = "UNIT_H";
      this.Unit.HeaderText = "Unit";
      this.Unit.Name = "Unit";
      this.Unit.ReadOnly = true;
      this.Unit.Width = 62;
      // 
      // ManagementActivityCode
      // 
      this.ManagementActivityCode.DataPropertyName = "MGMT_ACTIVITY_CODE";
      this.ManagementActivityCode.HeaderText = "Management Activity Code";
      this.ManagementActivityCode.Name = "ManagementActivityCode";
      this.ManagementActivityCode.ReadOnly = true;
      this.ManagementActivityCode.Width = 156;
      // 
      // DwellingType
      // 
      this.DwellingType.DataPropertyName = "DWELL_TYPE_C";
      this.DwellingType.HeaderText = "Dwelling Type";
      this.DwellingType.Name = "DwellingType";
      this.DwellingType.ReadOnly = true;
      this.DwellingType.Width = 115;
      // 
      // Remarks
      // 
      this.Remarks.DataPropertyName = "REMARKS_MOBILE";
      this.Remarks.HeaderText = "Remarks";
      this.Remarks.Name = "Remarks";
      this.Remarks.ReadOnly = true;
      this.Remarks.Width = 125;
      // 
      // EditedCells
      // 
      this.EditedCells.DataPropertyName = "EDITED_CELLS";
      this.EditedCells.HeaderText = "Edited Cells";
      this.EditedCells.Name = "EditedCells";
      this.EditedCells.ReadOnly = true;
      this.EditedCells.Visible = false;
      // 
      // FLNX_H
      // 
      this.FLNX_H.DataPropertyName = "FLNX_H";
      this.FLNX_H.HeaderText = "FLNX_H";
      this.FLNX_H.Name = "FLNX_H";
      this.FLNX_H.ReadOnly = true;
      this.FLNX_H.Visible = false;
      // 
      // FLNY_H
      // 
      this.FLNY_H.DataPropertyName = "FLNY_H";
      this.FLNY_H.HeaderText = "FLNY_H";
      this.FLNY_H.Name = "FLNY_H";
      this.FLNY_H.ReadOnly = true;
      this.FLNY_H.Visible = false;
      // 
      // ServiceActivityCodeFltr
      // 
      this.ServiceActivityCodeFltr.HeaderText = "SVCActivityCode";
      this.ServiceActivityCodeFltr.Name = "ServiceActivityCodeFltr";
      this.ServiceActivityCodeFltr.Visible = false;
      this.ServiceActivityCodeFltr.Width = 5;
      // 
      // fltrStatus
      // 
      this.fltrStatus.HeaderText = "Status";
      this.fltrStatus.Name = "fltrStatus";
      this.fltrStatus.Resizable = System.Windows.Forms.DataGridViewTriState.True;
      this.fltrStatus.Text = "FAILED";
      this.fltrStatus.UseColumnTextForButtonValue = true;
      this.fltrStatus.Width = 77;
      // 
      // fltrCrewHQ
      // 
      this.fltrCrewHQ.HeaderText = "Crew HQ";
      this.fltrCrewHQ.Name = "fltrCrewHQ";
      this.fltrCrewHQ.Resizable = System.Windows.Forms.DataGridViewTriState.True;
      this.fltrCrewHQ.Width = 119;
      // 
      // fltrComment
      // 
      this.fltrComment.HeaderText = "Comment";
      this.fltrComment.Name = "fltrComment";
      this.fltrComment.Resizable = System.Windows.Forms.DataGridViewTriState.True;
      this.fltrComment.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      this.fltrComment.Width = 96;
      // 
      // fltrOverrideTolerance
      // 
      this.fltrOverrideTolerance.HeaderText = "Override Tolerance";
      this.fltrOverrideTolerance.Name = "fltrOverrideTolerance";
      this.fltrOverrideTolerance.Resizable = System.Windows.Forms.DataGridViewTriState.True;
      this.fltrOverrideTolerance.Width = 146;
      // 
      // fltrStructureID
      // 
      this.fltrStructureID.HeaderText = "Structure ID";
      this.fltrStructureID.Name = "fltrStructureID";
      this.fltrStructureID.Resizable = System.Windows.Forms.DataGridViewTriState.True;
      this.fltrStructureID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      this.fltrStructureID.Width = 103;
      // 
      // fltrTransCompanyNum
      // 
      this.fltrTransCompanyNum.HeaderText = "Transformer Company #";
      this.fltrTransCompanyNum.Name = "fltrTransCompanyNum";
      this.fltrTransCompanyNum.Width = 173;
      // 
      // fltrError
      // 
      this.fltrError.HeaderText = "Error";
      this.fltrError.Name = "fltrError";
      this.fltrError.Width = 150;
      // 
      // fltrEditDate
      // 
      this.fltrEditDate.HeaderText = "Edit Date";
      this.fltrEditDate.Name = "fltrEditDate";
      this.fltrEditDate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
      this.fltrEditDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
      this.fltrEditDate.Text = "Edit Date";
      this.fltrEditDate.UseColumnTextForButtonValue = true;
      this.fltrEditDate.Width = 88;
      // 
      // fltrUserID
      // 
      this.fltrUserID.HeaderText = "User ID";
      this.fltrUserID.Name = "fltrUserID";
      this.fltrUserID.Width = 78;
      // 
      // FromTransactionDate
      // 
      this.FromTransactionDate.HeaderText = "From Transaction Date";
      this.FromTransactionDate.Name = "FromTransactionDate";
      this.FromTransactionDate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
      this.FromTransactionDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
      this.FromTransactionDate.Text = "From Date";
      this.FromTransactionDate.UseColumnTextForButtonValue = true;
      this.FromTransactionDate.Width = 75;
      // 
      // ToTransactionDate
      // 
      this.ToTransactionDate.HeaderText = "To Transaction Date";
      this.ToTransactionDate.Name = "ToTransactionDate";
      this.ToTransactionDate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
      this.ToTransactionDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
      this.ToTransactionDate.Text = "To Date";
      this.ToTransactionDate.UseColumnTextForButtonValue = true;
      this.ToTransactionDate.Width = 75;
      // 
      // fltrTransactionType
      // 
      this.fltrTransactionType.HeaderText = "Transaction Type";
      this.fltrTransactionType.Name = "fltrTransactionType";
      this.fltrTransactionType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
      this.fltrTransactionType.Width = 136;
      // 
      // fltrActivityCode
      // 
      this.fltrActivityCode.HeaderText = "Activity Code";
      this.fltrActivityCode.Name = "fltrActivityCode";
      this.fltrActivityCode.Resizable = System.Windows.Forms.DataGridViewTriState.True;
      this.fltrActivityCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
      this.fltrActivityCode.Width = 120;
      // 
      // fltrMeterLatitude
      // 
      this.fltrMeterLatitude.HeaderText = "Meter Latitude";
      this.fltrMeterLatitude.Name = "fltrMeterLatitude";
      this.fltrMeterLatitude.Width = 118;
      // 
      // fltrMeterLongitude
      // 
      this.fltrMeterLongitude.HeaderText = "Meter Longitude";
      this.fltrMeterLongitude.Name = "fltrMeterLongitude";
      this.fltrMeterLongitude.Width = 128;
      // 
      // fltrLocateMethod
      // 
      this.fltrLocateMethod.HeaderText = "Locate Method";
      this.fltrLocateMethod.Name = "fltrLocateMethod";
      this.fltrLocateMethod.Width = 120;
      // 
      // fltrCU
      // 
      this.fltrCU.HeaderText = "CU";
      this.fltrCU.Name = "fltrCU";
      this.fltrCU.Width = 80;
      // 
      // fltrPremiseNumber
      // 
      this.fltrPremiseNumber.HeaderText = "Premise Number";
      this.fltrPremiseNumber.Name = "fltrPremiseNumber";
      this.fltrPremiseNumber.Width = 107;
      // 
      // fltrDirection
      // 
      this.fltrDirection.HeaderText = "Direction";
      this.fltrDirection.Name = "fltrDirection";
      this.fltrDirection.Width = 136;
      // 
      // fltrHouseNumber
      // 
      this.fltrHouseNumber.HeaderText = "House Number";
      this.fltrHouseNumber.Name = "fltrHouseNumber";
      this.fltrHouseNumber.Width = 121;
      // 
      // fltrHouseFractionNumber
      // 
      this.fltrHouseFractionNumber.HeaderText = "House Fraction Number";
      this.fltrHouseFractionNumber.Name = "fltrHouseFractionNumber";
      this.fltrHouseFractionNumber.Width = 126;
      // 
      // fltrStreetName
      // 
      this.fltrStreetName.HeaderText = "Street Name";
      this.fltrStreetName.Name = "fltrStreetName";
      this.fltrStreetName.Width = 107;
      // 
      // fltrStreetType
      // 
      this.fltrStreetType.HeaderText = "Street Type";
      this.fltrStreetType.Name = "fltrStreetType";
      this.fltrStreetType.Width = 102;
      // 
      // fltrDirectionTrailing
      // 
      this.fltrDirectionTrailing.HeaderText = "Direction Trailing";
      this.fltrDirectionTrailing.Name = "fltrDirectionTrailing";
      this.fltrDirectionTrailing.Width = 132;
      // 
      // fltrUnit
      // 
      this.fltrUnit.HeaderText = "Unit";
      this.fltrUnit.Name = "fltrUnit";
      this.fltrUnit.Width = 62;
      // 
      // fltrManagementActivityCode
      // 
      this.fltrManagementActivityCode.HeaderText = "Management Activity Code";
      this.fltrManagementActivityCode.Name = "fltrManagementActivityCode";
      this.fltrManagementActivityCode.Width = 156;
      // 
      // fltrDwellingType
      // 
      this.fltrDwellingType.HeaderText = "Dwelling Type";
      this.fltrDwellingType.Name = "fltrDwellingType";
      this.fltrDwellingType.Width = 115;
      // 
      // fltrRemarks
      // 
      this.fltrRemarks.HeaderText = "Remarks";
      this.fltrRemarks.Name = "fltrRemarks";
      this.fltrRemarks.Width = 125;
      // 
      // FieldActivityErrorResolutionDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1009, 359);
      this.Controls.Add(this.panel2);
      this.Controls.Add(this.dgv_Filters);
      this.Controls.Add(this.FilterTitle);
      this.Controls.Add(this.locateType);
      this.Controls.Add(this.UpdateMap);
      this.Controls.Add(this.RefreshCmd);
      this.Controls.Add(this.SubmitCmd);
      this.Controls.Add(this.ExitCmd);
      this.Controls.Add(this.dgvErrorRecords);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(1025, 397);
      this.Name = "FieldActivityErrorResolutionDialog";
      this.ShowIcon = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Field Activity Error Resolution";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FieldActivityErrorResolutionDialog_FormClosing);
      this.Load += new System.EventHandler(this.FieldActivityErrorResolutionDialog_Load);
      ((System.ComponentModel.ISupportInitialize)(this.dgvErrorRecords)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
      this.locateType.ResumeLayout(false);
      this.locateType.PerformLayout();
      this.FilterTitle.ResumeLayout(false);
      this.FilterTitle.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dgv_Filters)).EndInit();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.locationNavigator)).EndInit();
      this.locationNavigator.ResumeLayout(false);
      this.locationNavigator.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.bindingSource2)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvErrorRecords;
        private System.Windows.Forms.Button ExitCmd;
        private System.Windows.Forms.Button SubmitCmd;
        private System.Windows.Forms.Button RefreshCmd;
        private System.Windows.Forms.CheckBox UpdateMap;
        private System.Windows.Forms.Panel locateType;
        private System.Windows.Forms.RadioButton optTransformer;
        private System.Windows.Forms.RadioButton optStructureID;
        private System.Windows.Forms.RadioButton optMeterGeocode;
        private System.Windows.Forms.RadioButton optAddress;
        private System.Windows.Forms.RadioButton optPremiseNumber;
        private System.Windows.Forms.Panel FilterTitle;
        private System.Windows.Forms.Label FiltersLabel;
        private System.Windows.Forms.DataGridView dgv_Filters;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.BindingNavigator locationNavigator;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem1;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator3;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem1;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator4;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem1;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator5;
        private System.Windows.Forms.BindingSource bindingSource2;
    private System.Windows.Forms.DataGridViewTextBoxColumn ServiceActivityCode;
    private System.Windows.Forms.DataGridViewComboBoxColumn Status;
    private System.Windows.Forms.DataGridViewTextBoxColumn CrewHQ;
    private System.Windows.Forms.DataGridViewTextBoxColumn Comment;
    private System.Windows.Forms.DataGridViewCheckBoxColumn OverrideTolerance;
    private System.Windows.Forms.DataGridViewTextBoxColumn StructureID;
    private System.Windows.Forms.DataGridViewTextBoxColumn TransCompanyNum;
    private System.Windows.Forms.DataGridViewTextBoxColumn Error;
    private System.Windows.Forms.DataGridViewTextBoxColumn EditDate;
    private System.Windows.Forms.DataGridViewTextBoxColumn UserID;
    private System.Windows.Forms.DataGridViewTextBoxColumn TransactionDate;
    private System.Windows.Forms.DataGridViewTextBoxColumn TransactionType;
    private System.Windows.Forms.DataGridViewTextBoxColumn ActivityCode;
    private System.Windows.Forms.DataGridViewTextBoxColumn MeterLatitude;
    private System.Windows.Forms.DataGridViewTextBoxColumn MeterLongitude;
    private System.Windows.Forms.DataGridViewTextBoxColumn LocateMethod;
    private System.Windows.Forms.DataGridViewTextBoxColumn CU;
    private System.Windows.Forms.DataGridViewTextBoxColumn PremiseNumber;
    private System.Windows.Forms.DataGridViewTextBoxColumn Direction;
    private System.Windows.Forms.DataGridViewTextBoxColumn HouseNumber;
    private System.Windows.Forms.DataGridViewTextBoxColumn HouseFractionNumber;
    private System.Windows.Forms.DataGridViewTextBoxColumn StreetName;
    private System.Windows.Forms.DataGridViewTextBoxColumn StreetType;
    private System.Windows.Forms.DataGridViewTextBoxColumn DirectionTrailing;
    private System.Windows.Forms.DataGridViewTextBoxColumn Unit;
    private System.Windows.Forms.DataGridViewTextBoxColumn ManagementActivityCode;
    private System.Windows.Forms.DataGridViewTextBoxColumn DwellingType;
    private System.Windows.Forms.DataGridViewTextBoxColumn Remarks;
    private System.Windows.Forms.DataGridViewTextBoxColumn EditedCells;
    private System.Windows.Forms.DataGridViewTextBoxColumn FLNX_H;
    private System.Windows.Forms.DataGridViewTextBoxColumn FLNY_H;
    private System.Windows.Forms.DataGridViewTextBoxColumn ServiceActivityCodeFltr;
    private System.Windows.Forms.DataGridViewButtonColumn fltrStatus;
    private System.Windows.Forms.DataGridViewComboBoxColumn fltrCrewHQ;
    private System.Windows.Forms.DataGridViewTextBoxColumn fltrComment;
    private System.Windows.Forms.DataGridViewCheckBoxColumn fltrOverrideTolerance;
    private System.Windows.Forms.DataGridViewTextBoxColumn fltrStructureID;
    private System.Windows.Forms.DataGridViewTextBoxColumn fltrTransCompanyNum;
    private System.Windows.Forms.DataGridViewTextBoxColumn fltrError;
    private System.Windows.Forms.DataGridViewButtonColumn fltrEditDate;
    private System.Windows.Forms.DataGridViewTextBoxColumn fltrUserID;
    private System.Windows.Forms.DataGridViewButtonColumn FromTransactionDate;
    private System.Windows.Forms.DataGridViewButtonColumn ToTransactionDate;
    private System.Windows.Forms.DataGridViewComboBoxColumn fltrTransactionType;
    private System.Windows.Forms.DataGridViewComboBoxColumn fltrActivityCode;
    private System.Windows.Forms.DataGridViewTextBoxColumn fltrMeterLatitude;
    private System.Windows.Forms.DataGridViewTextBoxColumn fltrMeterLongitude;
    private System.Windows.Forms.DataGridViewTextBoxColumn fltrLocateMethod;
    private System.Windows.Forms.DataGridViewTextBoxColumn fltrCU;
    private System.Windows.Forms.DataGridViewTextBoxColumn fltrPremiseNumber;
    private System.Windows.Forms.DataGridViewTextBoxColumn fltrDirection;
    private System.Windows.Forms.DataGridViewTextBoxColumn fltrHouseNumber;
    private System.Windows.Forms.DataGridViewTextBoxColumn fltrHouseFractionNumber;
    private System.Windows.Forms.DataGridViewTextBoxColumn fltrStreetName;
    private System.Windows.Forms.DataGridViewTextBoxColumn fltrStreetType;
    private System.Windows.Forms.DataGridViewTextBoxColumn fltrDirectionTrailing;
    private System.Windows.Forms.DataGridViewTextBoxColumn fltrUnit;
    private System.Windows.Forms.DataGridViewTextBoxColumn fltrManagementActivityCode;
    private System.Windows.Forms.DataGridViewTextBoxColumn fltrDwellingType;
    private System.Windows.Forms.DataGridViewTextBoxColumn fltrRemarks;
  }
}

