namespace GTechnology.Oncor.CustomAPI
{
    partial class frmSecondaryCalculator
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvSecondary = new System.Windows.Forms.DataGridView();
            this.grpPowerFactor = new System.Windows.Forms.GroupBox();
            this.lblPFWinter = new System.Windows.Forms.Label();
            this.lblPFSummer = new System.Windows.Forms.Label();
            this.txtPFWinter = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPFSummer = new System.Windows.Forms.TextBox();
            this.lblSummerPct = new System.Windows.Forms.Label();
            this.cmdTransformerSizing = new System.Windows.Forms.Button();
            this.lblLargestACTonnage = new System.Windows.Forms.Label();
            this.txtFutureServiceLoad = new System.Windows.Forms.TextBox();
            this.lblFutureServiceLoad = new System.Windows.Forms.Label();
            this.txtFutureServiceLength = new System.Windows.Forms.TextBox();
            this.lblFutureServiceLength = new System.Windows.Forms.Label();
            this.cmdOptions = new System.Windows.Forms.Button();
            this.cmdSaveReport = new System.Windows.Forms.Button();
            this.cmdApply = new System.Windows.Forms.Button();
            this.cmdCalculate = new System.Windows.Forms.Button();
            this.cmdClose = new System.Windows.Forms.Button();
            this.grpActualEstimated = new System.Windows.Forms.GroupBox();
            this.cboLargestACTonnage = new System.Windows.Forms.ComboBox();
            this.txtOverrideLoad = new System.Windows.Forms.TextBox();
            this.lblOverride = new System.Windows.Forms.Label();
            this.optLoadEstimated = new System.Windows.Forms.RadioButton();
            this.optLoadActual = new System.Windows.Forms.RadioButton();
            this.lblNeutralVDrop = new System.Windows.Forms.Label();
            this.cboNeutralVDrop = new System.Windows.Forms.ComboBox();
            this.grpTransformer = new System.Windows.Forms.GroupBox();
            this.cmdXfmrCU = new System.Windows.Forms.Button();
            this.txtXfmrVoltage = new System.Windows.Forms.TextBox();
            this.txtXfmrSize = new System.Windows.Forms.TextBox();
            this.txtXfmrType = new System.Windows.Forms.TextBox();
            this.txtXfmrVoltageDrop = new System.Windows.Forms.TextBox();
            this.lblXfmrVoltageDrop = new System.Windows.Forms.Label();
            this.lblXfmrVoltage = new System.Windows.Forms.Label();
            this.lblXfmrSize = new System.Windows.Forms.Label();
            this.lblXfmrType = new System.Windows.Forms.Label();
            this.lblXfmrCU = new System.Windows.Forms.Label();
            this.dgvService = new System.Windows.Forms.DataGridView();
            this.cmdPrintReport = new System.Windows.Forms.Button();
            this.imgGridDivider = new System.Windows.Forms.PictureBox();
            this.grpCommercial = new System.Windows.Forms.GroupBox();
            this.cboCablesPerPhase = new System.Windows.Forms.ComboBox();
            this.lblCablesPerPhase = new System.Windows.Forms.Label();
            this.SecSpan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SecType = new System.Windows.Forms.DataGridViewButtonColumn();
            this.SecLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SecVoltage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SecSeason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SecFlicker = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SecFutureSrvcCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SecFID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SecSourceFID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SecFutureSrvcLoad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SecFutureSrvcLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SecVoltageDropPct = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SecFNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SecPendingEdit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SecCU = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SecLengthEdit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SecAllowCUEdit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SrvcSpan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SrvcType = new System.Windows.Forms.DataGridViewButtonColumn();
            this.SrvcLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SrvcVoltage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SrvcSeason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SrvcFlicker = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SrvcLoadSummerActual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SrvcLoadSummerEstimate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SrvcLoadWinterActual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SrvcLoadWinterEstimate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SrvcFID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SrvcSourceFID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SrvcOverrideLoad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SrvcLoadUsed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SrvcVoltageDropPct = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SrvcPendingEdit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SrvcCU = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SrvcPtFID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SrvcLengthEdit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SrvcAllowCUEdit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSecondary)).BeginInit();
            this.grpPowerFactor.SuspendLayout();
            this.grpActualEstimated.SuspendLayout();
            this.grpTransformer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvService)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgGridDivider)).BeginInit();
            this.grpCommercial.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvSecondary
            // 
            this.dgvSecondary.AllowUserToAddRows = false;
            this.dgvSecondary.AllowUserToDeleteRows = false;
            this.dgvSecondary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSecondary.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSecondary.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSecondary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSecondary.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SecSpan,
            this.SecType,
            this.SecLength,
            this.SecVoltage,
            this.SecSeason,
            this.SecFlicker,
            this.SecFutureSrvcCount,
            this.SecFID,
            this.SecSourceFID,
            this.SecFutureSrvcLoad,
            this.SecFutureSrvcLength,
            this.SecVoltageDropPct,
            this.SecFNO,
            this.SecPendingEdit,
            this.SecCU,
            this.SecLengthEdit,
            this.SecAllowCUEdit});
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSecondary.DefaultCellStyle = dataGridViewCellStyle7;
            this.dgvSecondary.Location = new System.Drawing.Point(8, 198);
            this.dgvSecondary.Name = "dgvSecondary";
            this.dgvSecondary.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSecondary.Size = new System.Drawing.Size(562, 104);
            this.dgvSecondary.TabIndex = 0;
            this.dgvSecondary.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSecondary_CellClick);
            this.dgvSecondary.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSecondary_CellEndEdit);
            this.dgvSecondary.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSecondary_CellValueChanged);
            this.dgvSecondary.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvSecondary_EditingControlShowing);
            this.dgvSecondary.SelectionChanged += new System.EventHandler(this.dgvSecondary_SelectionChanged);
            // 
            // grpPowerFactor
            // 
            this.grpPowerFactor.Controls.Add(this.lblPFWinter);
            this.grpPowerFactor.Controls.Add(this.lblPFSummer);
            this.grpPowerFactor.Controls.Add(this.txtPFWinter);
            this.grpPowerFactor.Controls.Add(this.label1);
            this.grpPowerFactor.Controls.Add(this.txtPFSummer);
            this.grpPowerFactor.Controls.Add(this.lblSummerPct);
            this.grpPowerFactor.Location = new System.Drawing.Point(12, 2);
            this.grpPowerFactor.Name = "grpPowerFactor";
            this.grpPowerFactor.Size = new System.Drawing.Size(112, 88);
            this.grpPowerFactor.TabIndex = 1;
            this.grpPowerFactor.TabStop = false;
            this.grpPowerFactor.Text = "Power Factor";
            // 
            // lblPFWinter
            // 
            this.lblPFWinter.AutoSize = true;
            this.lblPFWinter.Location = new System.Drawing.Point(70, 20);
            this.lblPFWinter.Name = "lblPFWinter";
            this.lblPFWinter.Size = new System.Drawing.Size(38, 13);
            this.lblPFWinter.TabIndex = 7;
            this.lblPFWinter.Text = "Winter";
            // 
            // lblPFSummer
            // 
            this.lblPFSummer.AutoSize = true;
            this.lblPFSummer.Location = new System.Drawing.Point(16, 20);
            this.lblPFSummer.Name = "lblPFSummer";
            this.lblPFSummer.Size = new System.Drawing.Size(45, 13);
            this.lblPFSummer.TabIndex = 6;
            this.lblPFSummer.Text = "Summer";
            // 
            // txtPFWinter
            // 
            this.txtPFWinter.Location = new System.Drawing.Point(73, 39);
            this.txtPFWinter.Name = "txtPFWinter";
            this.txtPFWinter.ShortcutsEnabled = false;
            this.txtPFWinter.Size = new System.Drawing.Size(25, 20);
            this.txtPFWinter.TabIndex = 4;
            this.txtPFWinter.Text = "90";
            this.txtPFWinter.TextChanged += new System.EventHandler(this.txtPFWinter_TextChanged);
            this.txtPFWinter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPFWinter_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(96, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "%";
            // 
            // txtPFSummer
            // 
            this.txtPFSummer.Location = new System.Drawing.Point(19, 39);
            this.txtPFSummer.Name = "txtPFSummer";
            this.txtPFSummer.ShortcutsEnabled = false;
            this.txtPFSummer.Size = new System.Drawing.Size(25, 20);
            this.txtPFSummer.TabIndex = 1;
            this.txtPFSummer.Text = "95";
            this.txtPFSummer.TextChanged += new System.EventHandler(this.txtPFSummer_TextChanged);
            this.txtPFSummer.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPFSummer_KeyPress);
            // 
            // lblSummerPct
            // 
            this.lblSummerPct.AutoSize = true;
            this.lblSummerPct.Location = new System.Drawing.Point(42, 43);
            this.lblSummerPct.Name = "lblSummerPct";
            this.lblSummerPct.Size = new System.Drawing.Size(15, 13);
            this.lblSummerPct.TabIndex = 2;
            this.lblSummerPct.Text = "%";
            // 
            // cmdTransformerSizing
            // 
            this.cmdTransformerSizing.Location = new System.Drawing.Point(8, 169);
            this.cmdTransformerSizing.Name = "cmdTransformerSizing";
            this.cmdTransformerSizing.Size = new System.Drawing.Size(75, 23);
            this.cmdTransformerSizing.TabIndex = 2;
            this.cmdTransformerSizing.Text = "Xfmr Sizing";
            this.cmdTransformerSizing.UseVisualStyleBackColor = true;
            this.cmdTransformerSizing.Click += new System.EventHandler(this.cmdTransformerSizing_Click);
            // 
            // lblLargestACTonnage
            // 
            this.lblLargestACTonnage.AutoSize = true;
            this.lblLargestACTonnage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLargestACTonnage.Location = new System.Drawing.Point(163, 20);
            this.lblLargestACTonnage.Name = "lblLargestACTonnage";
            this.lblLargestACTonnage.Size = new System.Drawing.Size(57, 39);
            this.lblLargestACTonnage.TabIndex = 3;
            this.lblLargestACTonnage.Text = "Largest\r\nAC \r\nTonnage";
            // 
            // txtFutureServiceLoad
            // 
            this.txtFutureServiceLoad.Location = new System.Drawing.Point(236, 63);
            this.txtFutureServiceLoad.Name = "txtFutureServiceLoad";
            this.txtFutureServiceLoad.ShortcutsEnabled = false;
            this.txtFutureServiceLoad.Size = new System.Drawing.Size(44, 20);
            this.txtFutureServiceLoad.TabIndex = 6;
            this.txtFutureServiceLoad.TextChanged += new System.EventHandler(this.txtFutureServiceLoad_TextChanged);
            this.txtFutureServiceLoad.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFutureServiceLoad_KeyPress);
            // 
            // lblFutureServiceLoad
            // 
            this.lblFutureServiceLoad.AutoSize = true;
            this.lblFutureServiceLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFutureServiceLoad.Location = new System.Drawing.Point(236, 20);
            this.lblFutureServiceLoad.Name = "lblFutureServiceLoad";
            this.lblFutureServiceLoad.Size = new System.Drawing.Size(46, 39);
            this.lblFutureServiceLoad.TabIndex = 5;
            this.lblFutureServiceLoad.Text = "Future \r\nService \r\nLoad";
            // 
            // txtFutureServiceLength
            // 
            this.txtFutureServiceLength.Location = new System.Drawing.Point(298, 63);
            this.txtFutureServiceLength.Name = "txtFutureServiceLength";
            this.txtFutureServiceLength.ShortcutsEnabled = false;
            this.txtFutureServiceLength.Size = new System.Drawing.Size(44, 20);
            this.txtFutureServiceLength.TabIndex = 8;
            this.txtFutureServiceLength.TextChanged += new System.EventHandler(this.txtFutureServiceLength_TextChanged);
            this.txtFutureServiceLength.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFutureServiceLength_KeyPress);
            // 
            // lblFutureServiceLength
            // 
            this.lblFutureServiceLength.AutoSize = true;
            this.lblFutureServiceLength.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFutureServiceLength.Location = new System.Drawing.Point(298, 20);
            this.lblFutureServiceLength.Name = "lblFutureServiceLength";
            this.lblFutureServiceLength.Size = new System.Drawing.Size(46, 39);
            this.lblFutureServiceLength.TabIndex = 7;
            this.lblFutureServiceLength.Text = "Future \r\nService \r\nLength";
            // 
            // cmdOptions
            // 
            this.cmdOptions.Location = new System.Drawing.Point(89, 169);
            this.cmdOptions.Name = "cmdOptions";
            this.cmdOptions.Size = new System.Drawing.Size(75, 23);
            this.cmdOptions.TabIndex = 9;
            this.cmdOptions.Text = "Options";
            this.cmdOptions.UseVisualStyleBackColor = true;
            this.cmdOptions.Click += new System.EventHandler(this.cmdOptions_Click);
            // 
            // cmdSaveReport
            // 
            this.cmdSaveReport.Location = new System.Drawing.Point(250, 169);
            this.cmdSaveReport.Name = "cmdSaveReport";
            this.cmdSaveReport.Size = new System.Drawing.Size(75, 23);
            this.cmdSaveReport.TabIndex = 10;
            this.cmdSaveReport.Text = "Save Report";
            this.cmdSaveReport.UseVisualStyleBackColor = true;
            this.cmdSaveReport.Click += new System.EventHandler(this.cmdSaveReport_Click);
            // 
            // cmdApply
            // 
            this.cmdApply.Location = new System.Drawing.Point(331, 169);
            this.cmdApply.Name = "cmdApply";
            this.cmdApply.Size = new System.Drawing.Size(75, 23);
            this.cmdApply.TabIndex = 13;
            this.cmdApply.Text = "Apply";
            this.cmdApply.UseVisualStyleBackColor = true;
            this.cmdApply.Click += new System.EventHandler(this.cmdApply_Click);
            // 
            // cmdCalculate
            // 
            this.cmdCalculate.Location = new System.Drawing.Point(412, 169);
            this.cmdCalculate.Name = "cmdCalculate";
            this.cmdCalculate.Size = new System.Drawing.Size(75, 23);
            this.cmdCalculate.TabIndex = 12;
            this.cmdCalculate.Text = "Calculate";
            this.cmdCalculate.UseVisualStyleBackColor = true;
            this.cmdCalculate.Click += new System.EventHandler(this.cmdCalculate_Click);
            // 
            // cmdClose
            // 
            this.cmdClose.Location = new System.Drawing.Point(493, 169);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(75, 23);
            this.cmdClose.TabIndex = 11;
            this.cmdClose.Text = "Close";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // grpActualEstimated
            // 
            this.grpActualEstimated.Controls.Add(this.cboLargestACTonnage);
            this.grpActualEstimated.Controls.Add(this.txtOverrideLoad);
            this.grpActualEstimated.Controls.Add(this.lblOverride);
            this.grpActualEstimated.Controls.Add(this.optLoadEstimated);
            this.grpActualEstimated.Controls.Add(this.optLoadActual);
            this.grpActualEstimated.Controls.Add(this.lblFutureServiceLoad);
            this.grpActualEstimated.Controls.Add(this.txtFutureServiceLoad);
            this.grpActualEstimated.Controls.Add(this.txtFutureServiceLength);
            this.grpActualEstimated.Controls.Add(this.lblFutureServiceLength);
            this.grpActualEstimated.Controls.Add(this.lblLargestACTonnage);
            this.grpActualEstimated.Location = new System.Drawing.Point(129, 2);
            this.grpActualEstimated.Name = "grpActualEstimated";
            this.grpActualEstimated.Size = new System.Drawing.Size(352, 88);
            this.grpActualEstimated.TabIndex = 6;
            this.grpActualEstimated.TabStop = false;
            this.grpActualEstimated.Text = "Load";
            // 
            // cboLargestACTonnage
            // 
            this.cboLargestACTonnage.AutoCompleteCustomSource.AddRange(new string[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cboLargestACTonnage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLargestACTonnage.FormattingEnabled = true;
            this.cboLargestACTonnage.Location = new System.Drawing.Point(163, 63);
            this.cboLargestACTonnage.Name = "cboLargestACTonnage";
            this.cboLargestACTonnage.Size = new System.Drawing.Size(54, 21);
            this.cboLargestACTonnage.TabIndex = 17;
            this.cboLargestACTonnage.SelectedValueChanged += new System.EventHandler(this.cboLargestACTonnage_SelectedValueChanged);
            // 
            // txtOverrideLoad
            // 
            this.txtOverrideLoad.Location = new System.Drawing.Point(100, 63);
            this.txtOverrideLoad.Name = "txtOverrideLoad";
            this.txtOverrideLoad.ShortcutsEnabled = false;
            this.txtOverrideLoad.Size = new System.Drawing.Size(44, 20);
            this.txtOverrideLoad.TabIndex = 10;
            this.txtOverrideLoad.TextChanged += new System.EventHandler(this.txtOverrideLoad_TextChanged);
            this.txtOverrideLoad.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtOverrideLoad_KeyPress);
            // 
            // lblOverride
            // 
            this.lblOverride.AutoSize = true;
            this.lblOverride.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOverride.Location = new System.Drawing.Point(100, 20);
            this.lblOverride.Name = "lblOverride";
            this.lblOverride.Size = new System.Drawing.Size(47, 26);
            this.lblOverride.TabIndex = 9;
            this.lblOverride.Text = "Override\r\nLoad";
            // 
            // optLoadEstimated
            // 
            this.optLoadEstimated.AutoSize = true;
            this.optLoadEstimated.Location = new System.Drawing.Point(17, 43);
            this.optLoadEstimated.Name = "optLoadEstimated";
            this.optLoadEstimated.Size = new System.Drawing.Size(71, 17);
            this.optLoadEstimated.TabIndex = 3;
            this.optLoadEstimated.Text = "Estimated";
            this.optLoadEstimated.UseVisualStyleBackColor = true;
            this.optLoadEstimated.CheckedChanged += new System.EventHandler(this.optLoadEstimated_CheckedChanged);
            // 
            // optLoadActual
            // 
            this.optLoadActual.AutoSize = true;
            this.optLoadActual.Checked = true;
            this.optLoadActual.Location = new System.Drawing.Point(17, 20);
            this.optLoadActual.Name = "optLoadActual";
            this.optLoadActual.Size = new System.Drawing.Size(55, 17);
            this.optLoadActual.TabIndex = 0;
            this.optLoadActual.TabStop = true;
            this.optLoadActual.Text = "Actual";
            this.optLoadActual.UseVisualStyleBackColor = true;
            this.optLoadActual.CheckedChanged += new System.EventHandler(this.optLoadActual_CheckedChanged);
            // 
            // lblNeutralVDrop
            // 
            this.lblNeutralVDrop.AutoSize = true;
            this.lblNeutralVDrop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNeutralVDrop.Location = new System.Drawing.Point(445, 19);
            this.lblNeutralVDrop.Name = "lblNeutralVDrop";
            this.lblNeutralVDrop.Size = new System.Drawing.Size(44, 26);
            this.lblNeutralVDrop.TabIndex = 15;
            this.lblNeutralVDrop.Text = "Neutral \r\nV-Drop";
            // 
            // cboNeutralVDrop
            // 
            this.cboNeutralVDrop.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNeutralVDrop.FormattingEnabled = true;
            this.cboNeutralVDrop.Items.AddRange(new object[] {
            "Yes",
            "No"});
            this.cboNeutralVDrop.Location = new System.Drawing.Point(446, 47);
            this.cboNeutralVDrop.Name = "cboNeutralVDrop";
            this.cboNeutralVDrop.Size = new System.Drawing.Size(45, 21);
            this.cboNeutralVDrop.TabIndex = 16;
            this.cboNeutralVDrop.SelectedValueChanged += new System.EventHandler(this.cboNeutralVDrop_SelectedValueChanged);
            // 
            // grpTransformer
            // 
            this.grpTransformer.Controls.Add(this.cmdXfmrCU);
            this.grpTransformer.Controls.Add(this.txtXfmrVoltage);
            this.grpTransformer.Controls.Add(this.txtXfmrSize);
            this.grpTransformer.Controls.Add(this.txtXfmrType);
            this.grpTransformer.Controls.Add(this.txtXfmrVoltageDrop);
            this.grpTransformer.Controls.Add(this.cboNeutralVDrop);
            this.grpTransformer.Controls.Add(this.lblXfmrVoltageDrop);
            this.grpTransformer.Controls.Add(this.lblNeutralVDrop);
            this.grpTransformer.Controls.Add(this.lblXfmrVoltage);
            this.grpTransformer.Controls.Add(this.lblXfmrSize);
            this.grpTransformer.Controls.Add(this.lblXfmrType);
            this.grpTransformer.Controls.Add(this.lblXfmrCU);
            this.grpTransformer.Location = new System.Drawing.Point(12, 90);
            this.grpTransformer.Name = "grpTransformer";
            this.grpTransformer.Size = new System.Drawing.Size(556, 74);
            this.grpTransformer.TabIndex = 17;
            this.grpTransformer.TabStop = false;
            this.grpTransformer.Text = "Transformer";
            // 
            // cmdXfmrCU
            // 
            this.cmdXfmrCU.Location = new System.Drawing.Point(16, 47);
            this.cmdXfmrCU.Name = "cmdXfmrCU";
            this.cmdXfmrCU.Size = new System.Drawing.Size(182, 21);
            this.cmdXfmrCU.TabIndex = 22;
            this.cmdXfmrCU.UseVisualStyleBackColor = true;
            this.cmdXfmrCU.Click += new System.EventHandler(this.cmdXfmrCU_Click);
            // 
            // txtXfmrVoltage
            // 
            this.txtXfmrVoltage.Location = new System.Drawing.Point(357, 47);
            this.txtXfmrVoltage.Name = "txtXfmrVoltage";
            this.txtXfmrVoltage.ReadOnly = true;
            this.txtXfmrVoltage.Size = new System.Drawing.Size(83, 20);
            this.txtXfmrVoltage.TabIndex = 21;
            // 
            // txtXfmrSize
            // 
            this.txtXfmrSize.Location = new System.Drawing.Point(280, 47);
            this.txtXfmrSize.Name = "txtXfmrSize";
            this.txtXfmrSize.ReadOnly = true;
            this.txtXfmrSize.Size = new System.Drawing.Size(65, 20);
            this.txtXfmrSize.TabIndex = 20;
            // 
            // txtXfmrType
            // 
            this.txtXfmrType.Location = new System.Drawing.Point(205, 47);
            this.txtXfmrType.Name = "txtXfmrType";
            this.txtXfmrType.ReadOnly = true;
            this.txtXfmrType.Size = new System.Drawing.Size(65, 20);
            this.txtXfmrType.TabIndex = 19;
            // 
            // txtXfmrVoltageDrop
            // 
            this.txtXfmrVoltageDrop.Location = new System.Drawing.Point(500, 47);
            this.txtXfmrVoltageDrop.Name = "txtXfmrVoltageDrop";
            this.txtXfmrVoltageDrop.ReadOnly = true;
            this.txtXfmrVoltageDrop.Size = new System.Drawing.Size(43, 20);
            this.txtXfmrVoltageDrop.TabIndex = 18;
            // 
            // lblXfmrVoltageDrop
            // 
            this.lblXfmrVoltageDrop.AutoSize = true;
            this.lblXfmrVoltageDrop.Location = new System.Drawing.Point(497, 19);
            this.lblXfmrVoltageDrop.Name = "lblXfmrVoltageDrop";
            this.lblXfmrVoltageDrop.Size = new System.Drawing.Size(46, 26);
            this.lblXfmrVoltageDrop.TabIndex = 8;
            this.lblXfmrVoltageDrop.Text = "Voltage \r\nDrop";
            // 
            // lblXfmrVoltage
            // 
            this.lblXfmrVoltage.AutoSize = true;
            this.lblXfmrVoltage.Location = new System.Drawing.Point(354, 19);
            this.lblXfmrVoltage.Name = "lblXfmrVoltage";
            this.lblXfmrVoltage.Size = new System.Drawing.Size(66, 26);
            this.lblXfmrVoltage.TabIndex = 3;
            this.lblXfmrVoltage.Text = "Transformer \r\nVoltage";
            // 
            // lblXfmrSize
            // 
            this.lblXfmrSize.AutoSize = true;
            this.lblXfmrSize.Location = new System.Drawing.Point(279, 19);
            this.lblXfmrSize.Name = "lblXfmrSize";
            this.lblXfmrSize.Size = new System.Drawing.Size(66, 26);
            this.lblXfmrSize.TabIndex = 2;
            this.lblXfmrSize.Text = "Transformer \r\nSize";
            // 
            // lblXfmrType
            // 
            this.lblXfmrType.AutoSize = true;
            this.lblXfmrType.Location = new System.Drawing.Point(202, 19);
            this.lblXfmrType.Name = "lblXfmrType";
            this.lblXfmrType.Size = new System.Drawing.Size(66, 26);
            this.lblXfmrType.TabIndex = 1;
            this.lblXfmrType.Text = "Transformer \r\nType";
            // 
            // lblXfmrCU
            // 
            this.lblXfmrCU.AutoSize = true;
            this.lblXfmrCU.Location = new System.Drawing.Point(13, 31);
            this.lblXfmrCU.Name = "lblXfmrCU";
            this.lblXfmrCU.Size = new System.Drawing.Size(63, 13);
            this.lblXfmrCU.TabIndex = 0;
            this.lblXfmrCU.Text = "Transformer";
            // 
            // dgvService
            // 
            this.dgvService.AllowUserToAddRows = false;
            this.dgvService.AllowUserToDeleteRows = false;
            this.dgvService.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvService.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvService.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvService.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvService.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SrvcSpan,
            this.SrvcType,
            this.SrvcLength,
            this.SrvcVoltage,
            this.SrvcSeason,
            this.SrvcFlicker,
            this.SrvcLoadSummerActual,
            this.SrvcLoadSummerEstimate,
            this.SrvcLoadWinterActual,
            this.SrvcLoadWinterEstimate,
            this.SrvcFID,
            this.SrvcSourceFID,
            this.SrvcOverrideLoad,
            this.SrvcLoadUsed,
            this.SrvcVoltageDropPct,
            this.SrvcPendingEdit,
            this.SrvcCU,
            this.SrvcPtFID,
            this.SrvcLengthEdit,
            this.SrvcAllowCUEdit});
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle18.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle18.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvService.DefaultCellStyle = dataGridViewCellStyle18;
            this.dgvService.Location = new System.Drawing.Point(8, 308);
            this.dgvService.Name = "dgvService";
            this.dgvService.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvService.Size = new System.Drawing.Size(562, 163);
            this.dgvService.TabIndex = 18;
            this.dgvService.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvService_CellClick);
            this.dgvService.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvService_CellValueChanged);
            this.dgvService.SelectionChanged += new System.EventHandler(this.dgvService_SelectionChanged);
            // 
            // cmdPrintReport
            // 
            this.cmdPrintReport.Location = new System.Drawing.Point(170, 169);
            this.cmdPrintReport.Name = "cmdPrintReport";
            this.cmdPrintReport.Size = new System.Drawing.Size(75, 23);
            this.cmdPrintReport.TabIndex = 19;
            this.cmdPrintReport.Text = "Print Report";
            this.cmdPrintReport.UseVisualStyleBackColor = true;
            this.cmdPrintReport.Click += new System.EventHandler(this.cmdPrintReport_Click);
            // 
            // imgGridDivider
            // 
            this.imgGridDivider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imgGridDivider.Location = new System.Drawing.Point(8, 302);
            this.imgGridDivider.Name = "imgGridDivider";
            this.imgGridDivider.Size = new System.Drawing.Size(562, 5);
            this.imgGridDivider.TabIndex = 20;
            this.imgGridDivider.TabStop = false;
            this.imgGridDivider.MouseMove += new System.Windows.Forms.MouseEventHandler(this.imgGridDivider_MouseMove);
            // 
            // grpCommercial
            // 
            this.grpCommercial.Controls.Add(this.cboCablesPerPhase);
            this.grpCommercial.Controls.Add(this.lblCablesPerPhase);
            this.grpCommercial.Enabled = false;
            this.grpCommercial.Location = new System.Drawing.Point(487, 2);
            this.grpCommercial.Name = "grpCommercial";
            this.grpCommercial.Size = new System.Drawing.Size(81, 88);
            this.grpCommercial.TabIndex = 21;
            this.grpCommercial.TabStop = false;
            this.grpCommercial.Text = "Commercial";
            // 
            // cboCablesPerPhase
            // 
            this.cboCablesPerPhase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCablesPerPhase.Enabled = false;
            this.cboCablesPerPhase.FormattingEnabled = true;
            this.cboCablesPerPhase.Location = new System.Drawing.Point(16, 63);
            this.cboCablesPerPhase.Name = "cboCablesPerPhase";
            this.cboCablesPerPhase.Size = new System.Drawing.Size(57, 21);
            this.cboCablesPerPhase.TabIndex = 1;
            this.cboCablesPerPhase.SelectedValueChanged += new System.EventHandler(this.cboCablesPerPhase_SelectedValueChanged);
            // 
            // lblCablesPerPhase
            // 
            this.lblCablesPerPhase.AutoSize = true;
            this.lblCablesPerPhase.Enabled = false;
            this.lblCablesPerPhase.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCablesPerPhase.Location = new System.Drawing.Point(16, 20);
            this.lblCablesPerPhase.Name = "lblCablesPerPhase";
            this.lblCablesPerPhase.Size = new System.Drawing.Size(57, 39);
            this.lblCablesPerPhase.TabIndex = 0;
            this.lblCablesPerPhase.Text = "# Cables\r\nper\r\nPhase";
            // 
            // SecSpan
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Gainsboro;
            this.SecSpan.DefaultCellStyle = dataGridViewCellStyle2;
            this.SecSpan.HeaderText = "#";
            this.SecSpan.Name = "SecSpan";
            this.SecSpan.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SecSpan.Width = 20;
            // 
            // SecType
            // 
            this.SecType.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.SecType.HeaderText = "Type";
            this.SecType.Name = "SecType";
            this.SecType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.SecType.Width = 140;
            // 
            // SecLength
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            this.SecLength.DefaultCellStyle = dataGridViewCellStyle3;
            this.SecLength.HeaderText = "Length (ft)";
            this.SecLength.Name = "SecLength";
            this.SecLength.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SecLength.Width = 41;
            // 
            // SecVoltage
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle4.Format = "N1";
            this.SecVoltage.DefaultCellStyle = dataGridViewCellStyle4;
            this.SecVoltage.HeaderText = "Voltage";
            this.SecVoltage.Name = "SecVoltage";
            this.SecVoltage.ReadOnly = true;
            this.SecVoltage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SecVoltage.Width = 45;
            // 
            // SecSeason
            // 
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.Gainsboro;
            this.SecSeason.DefaultCellStyle = dataGridViewCellStyle5;
            this.SecSeason.HeaderText = "Season";
            this.SecSeason.Name = "SecSeason";
            this.SecSeason.ReadOnly = true;
            this.SecSeason.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SecSeason.Width = 47;
            // 
            // SecFlicker
            // 
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle6.Format = "P2";
            dataGridViewCellStyle6.NullValue = null;
            this.SecFlicker.DefaultCellStyle = dataGridViewCellStyle6;
            this.SecFlicker.HeaderText = "Flicker";
            this.SecFlicker.Name = "SecFlicker";
            this.SecFlicker.ReadOnly = true;
            this.SecFlicker.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SecFlicker.Width = 42;
            // 
            // SecFutureSrvcCount
            // 
            this.SecFutureSrvcCount.HeaderText = "Future Service Count";
            this.SecFutureSrvcCount.Name = "SecFutureSrvcCount";
            this.SecFutureSrvcCount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SecFutureSrvcCount.Width = 195;
            // 
            // SecFID
            // 
            this.SecFID.HeaderText = "SecFID";
            this.SecFID.Name = "SecFID";
            this.SecFID.ReadOnly = true;
            this.SecFID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SecFID.Visible = false;
            // 
            // SecSourceFID
            // 
            this.SecSourceFID.HeaderText = "SecSourceFID";
            this.SecSourceFID.Name = "SecSourceFID";
            this.SecSourceFID.ReadOnly = true;
            this.SecSourceFID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SecSourceFID.Visible = false;
            // 
            // SecFutureSrvcLoad
            // 
            this.SecFutureSrvcLoad.HeaderText = "SecFutureSrvcLoad";
            this.SecFutureSrvcLoad.Name = "SecFutureSrvcLoad";
            this.SecFutureSrvcLoad.ReadOnly = true;
            this.SecFutureSrvcLoad.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SecFutureSrvcLoad.Visible = false;
            // 
            // SecFutureSrvcLength
            // 
            this.SecFutureSrvcLength.HeaderText = "SecFutureSrvcLength";
            this.SecFutureSrvcLength.Name = "SecFutureSrvcLength";
            this.SecFutureSrvcLength.ReadOnly = true;
            this.SecFutureSrvcLength.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SecFutureSrvcLength.Visible = false;
            // 
            // SecVoltageDropPct
            // 
            this.SecVoltageDropPct.HeaderText = "SecVoltageDropPct";
            this.SecVoltageDropPct.Name = "SecVoltageDropPct";
            this.SecVoltageDropPct.ReadOnly = true;
            this.SecVoltageDropPct.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SecVoltageDropPct.Visible = false;
            // 
            // SecFNO
            // 
            this.SecFNO.HeaderText = "SecFNO";
            this.SecFNO.Name = "SecFNO";
            this.SecFNO.ReadOnly = true;
            this.SecFNO.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SecFNO.Visible = false;
            // 
            // SecPendingEdit
            // 
            this.SecPendingEdit.HeaderText = "SecPendingEdit";
            this.SecPendingEdit.Name = "SecPendingEdit";
            this.SecPendingEdit.ReadOnly = true;
            this.SecPendingEdit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SecPendingEdit.Visible = false;
            // 
            // SecCU
            // 
            this.SecCU.HeaderText = "SecCU";
            this.SecCU.Name = "SecCU";
            this.SecCU.ReadOnly = true;
            this.SecCU.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SecCU.Visible = false;
            // 
            // SecLengthEdit
            // 
            this.SecLengthEdit.HeaderText = "SecLengthEdit";
            this.SecLengthEdit.Name = "SecLengthEdit";
            this.SecLengthEdit.ReadOnly = true;
            this.SecLengthEdit.Visible = false;
            // 
            // SecAllowCUEdit
            // 
            this.SecAllowCUEdit.HeaderText = "SecAllowCUEdit";
            this.SecAllowCUEdit.Name = "SecAllowCUEdit";
            this.SecAllowCUEdit.Visible = false;
            // 
            // SrvcSpan
            // 
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.Gainsboro;
            this.SrvcSpan.DefaultCellStyle = dataGridViewCellStyle9;
            this.SrvcSpan.HeaderText = "#";
            this.SrvcSpan.Name = "SrvcSpan";
            this.SrvcSpan.ReadOnly = true;
            this.SrvcSpan.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SrvcSpan.Width = 20;
            // 
            // SrvcType
            // 
            this.SrvcType.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.SrvcType.HeaderText = "Type";
            this.SrvcType.Name = "SrvcType";
            this.SrvcType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.SrvcType.Width = 140;
            // 
            // SrvcLength
            // 
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.White;
            this.SrvcLength.DefaultCellStyle = dataGridViewCellStyle10;
            this.SrvcLength.HeaderText = "Length (ft)";
            this.SrvcLength.Name = "SrvcLength";
            this.SrvcLength.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SrvcLength.Width = 41;
            // 
            // SrvcVoltage
            // 
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle11.Format = "N1";
            this.SrvcVoltage.DefaultCellStyle = dataGridViewCellStyle11;
            this.SrvcVoltage.HeaderText = "Voltage";
            this.SrvcVoltage.Name = "SrvcVoltage";
            this.SrvcVoltage.ReadOnly = true;
            this.SrvcVoltage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SrvcVoltage.Width = 45;
            // 
            // SrvcSeason
            // 
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.Gainsboro;
            this.SrvcSeason.DefaultCellStyle = dataGridViewCellStyle12;
            this.SrvcSeason.HeaderText = "Season";
            this.SrvcSeason.Name = "SrvcSeason";
            this.SrvcSeason.ReadOnly = true;
            this.SrvcSeason.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SrvcSeason.Width = 47;
            // 
            // SrvcFlicker
            // 
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle13.Format = "P2";
            dataGridViewCellStyle13.NullValue = null;
            this.SrvcFlicker.DefaultCellStyle = dataGridViewCellStyle13;
            this.SrvcFlicker.HeaderText = "Flicker";
            this.SrvcFlicker.Name = "SrvcFlicker";
            this.SrvcFlicker.ReadOnly = true;
            this.SrvcFlicker.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SrvcFlicker.Width = 42;
            // 
            // SrvcLoadSummerActual
            // 
            dataGridViewCellStyle14.BackColor = System.Drawing.Color.Gainsboro;
            this.SrvcLoadSummerActual.DefaultCellStyle = dataGridViewCellStyle14;
            this.SrvcLoadSummerActual.HeaderText = "Load Summer Actual";
            this.SrvcLoadSummerActual.Name = "SrvcLoadSummerActual";
            this.SrvcLoadSummerActual.ReadOnly = true;
            this.SrvcLoadSummerActual.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SrvcLoadSummerActual.Width = 47;
            // 
            // SrvcLoadSummerEstimate
            // 
            dataGridViewCellStyle15.BackColor = System.Drawing.Color.Gainsboro;
            this.SrvcLoadSummerEstimate.DefaultCellStyle = dataGridViewCellStyle15;
            this.SrvcLoadSummerEstimate.HeaderText = "Load Summer Estimated";
            this.SrvcLoadSummerEstimate.Name = "SrvcLoadSummerEstimate";
            this.SrvcLoadSummerEstimate.ReadOnly = true;
            this.SrvcLoadSummerEstimate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SrvcLoadSummerEstimate.Width = 55;
            // 
            // SrvcLoadWinterActual
            // 
            dataGridViewCellStyle16.BackColor = System.Drawing.Color.Gainsboro;
            this.SrvcLoadWinterActual.DefaultCellStyle = dataGridViewCellStyle16;
            this.SrvcLoadWinterActual.HeaderText = "Load Winter Actual";
            this.SrvcLoadWinterActual.Name = "SrvcLoadWinterActual";
            this.SrvcLoadWinterActual.ReadOnly = true;
            this.SrvcLoadWinterActual.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SrvcLoadWinterActual.Width = 40;
            // 
            // SrvcLoadWinterEstimate
            // 
            dataGridViewCellStyle17.BackColor = System.Drawing.Color.Gainsboro;
            this.SrvcLoadWinterEstimate.DefaultCellStyle = dataGridViewCellStyle17;
            this.SrvcLoadWinterEstimate.HeaderText = "Load Winter Estimated";
            this.SrvcLoadWinterEstimate.Name = "SrvcLoadWinterEstimate";
            this.SrvcLoadWinterEstimate.ReadOnly = true;
            this.SrvcLoadWinterEstimate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SrvcLoadWinterEstimate.Width = 55;
            // 
            // SrvcFID
            // 
            this.SrvcFID.HeaderText = "SrvcFID";
            this.SrvcFID.Name = "SrvcFID";
            this.SrvcFID.ReadOnly = true;
            this.SrvcFID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SrvcFID.Visible = false;
            // 
            // SrvcSourceFID
            // 
            this.SrvcSourceFID.HeaderText = "SrvcSourceFID";
            this.SrvcSourceFID.Name = "SrvcSourceFID";
            this.SrvcSourceFID.ReadOnly = true;
            this.SrvcSourceFID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SrvcSourceFID.Visible = false;
            // 
            // SrvcOverrideLoad
            // 
            this.SrvcOverrideLoad.HeaderText = "SrvcOverrideLoad";
            this.SrvcOverrideLoad.Name = "SrvcOverrideLoad";
            this.SrvcOverrideLoad.ReadOnly = true;
            this.SrvcOverrideLoad.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SrvcOverrideLoad.Visible = false;
            // 
            // SrvcLoadUsed
            // 
            this.SrvcLoadUsed.HeaderText = "SrvcLoadUsed";
            this.SrvcLoadUsed.Name = "SrvcLoadUsed";
            this.SrvcLoadUsed.ReadOnly = true;
            this.SrvcLoadUsed.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SrvcLoadUsed.Visible = false;
            // 
            // SrvcVoltageDropPct
            // 
            this.SrvcVoltageDropPct.HeaderText = "SrvcVoltageDropPct";
            this.SrvcVoltageDropPct.Name = "SrvcVoltageDropPct";
            this.SrvcVoltageDropPct.ReadOnly = true;
            this.SrvcVoltageDropPct.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SrvcVoltageDropPct.Visible = false;
            // 
            // SrvcPendingEdit
            // 
            this.SrvcPendingEdit.HeaderText = "SrvcPendingEdit";
            this.SrvcPendingEdit.Name = "SrvcPendingEdit";
            this.SrvcPendingEdit.ReadOnly = true;
            this.SrvcPendingEdit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SrvcPendingEdit.Visible = false;
            // 
            // SrvcCU
            // 
            this.SrvcCU.HeaderText = "SrvcCU";
            this.SrvcCU.Name = "SrvcCU";
            this.SrvcCU.ReadOnly = true;
            this.SrvcCU.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SrvcCU.Visible = false;
            // 
            // SrvcPtFID
            // 
            this.SrvcPtFID.HeaderText = "SrvcPtFID";
            this.SrvcPtFID.Name = "SrvcPtFID";
            this.SrvcPtFID.ReadOnly = true;
            this.SrvcPtFID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SrvcPtFID.Visible = false;
            // 
            // SrvcLengthEdit
            // 
            this.SrvcLengthEdit.HeaderText = "SrvcLengthEdit";
            this.SrvcLengthEdit.Name = "SrvcLengthEdit";
            this.SrvcLengthEdit.ReadOnly = true;
            this.SrvcLengthEdit.Visible = false;
            // 
            // SrvcAllowCUEdit
            // 
            this.SrvcAllowCUEdit.HeaderText = "SrvcAllowCUEdit";
            this.SrvcAllowCUEdit.Name = "SrvcAllowCUEdit";
            this.SrvcAllowCUEdit.Visible = false;
            // 
            // frmSecondaryCalculator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(579, 478);
            this.Controls.Add(this.grpCommercial);
            this.Controls.Add(this.imgGridDivider);
            this.Controls.Add(this.cmdPrintReport);
            this.Controls.Add(this.dgvService);
            this.Controls.Add(this.grpTransformer);
            this.Controls.Add(this.cmdApply);
            this.Controls.Add(this.cmdCalculate);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.cmdSaveReport);
            this.Controls.Add(this.cmdOptions);
            this.Controls.Add(this.cmdTransformerSizing);
            this.Controls.Add(this.grpPowerFactor);
            this.Controls.Add(this.dgvSecondary);
            this.Controls.Add(this.grpActualEstimated);
            this.MaximizeBox = false;
            this.Name = "frmSecondaryCalculator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Secondary Calculator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSecondaryCalculator_FormClosing);
            this.Load += new System.EventHandler(this.frmSecondaryCalculator_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSecondary)).EndInit();
            this.grpPowerFactor.ResumeLayout(false);
            this.grpPowerFactor.PerformLayout();
            this.grpActualEstimated.ResumeLayout(false);
            this.grpActualEstimated.PerformLayout();
            this.grpTransformer.ResumeLayout(false);
            this.grpTransformer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvService)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgGridDivider)).EndInit();
            this.grpCommercial.ResumeLayout(false);
            this.grpCommercial.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvSecondary;
        private System.Windows.Forms.GroupBox grpPowerFactor;
        private System.Windows.Forms.TextBox txtPFWinter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPFSummer;
        private System.Windows.Forms.Label lblSummerPct;
        private System.Windows.Forms.Button cmdTransformerSizing;
        private System.Windows.Forms.Label lblLargestACTonnage;
        private System.Windows.Forms.TextBox txtFutureServiceLoad;
        private System.Windows.Forms.Label lblFutureServiceLoad;
        private System.Windows.Forms.TextBox txtFutureServiceLength;
        private System.Windows.Forms.Label lblFutureServiceLength;
        private System.Windows.Forms.Button cmdOptions;
        private System.Windows.Forms.Button cmdSaveReport;
        private System.Windows.Forms.Button cmdApply;
        private System.Windows.Forms.Button cmdCalculate;
        private System.Windows.Forms.Button cmdClose;
        private System.Windows.Forms.Label lblPFWinter;
        private System.Windows.Forms.Label lblPFSummer;
        private System.Windows.Forms.GroupBox grpActualEstimated;
        private System.Windows.Forms.TextBox txtOverrideLoad;
        private System.Windows.Forms.Label lblOverride;
        private System.Windows.Forms.RadioButton optLoadEstimated;
        private System.Windows.Forms.RadioButton optLoadActual;
        private System.Windows.Forms.Label lblNeutralVDrop;
        private System.Windows.Forms.ComboBox cboNeutralVDrop;
        private System.Windows.Forms.ComboBox cboLargestACTonnage;
        private System.Windows.Forms.GroupBox grpTransformer;
        private System.Windows.Forms.Label lblXfmrVoltage;
        private System.Windows.Forms.Label lblXfmrSize;
        private System.Windows.Forms.Label lblXfmrType;
        private System.Windows.Forms.Label lblXfmrCU;
        private System.Windows.Forms.TextBox txtXfmrVoltageDrop;
        private System.Windows.Forms.Label lblXfmrVoltageDrop;
        private System.Windows.Forms.DataGridView dgvService;
        private System.Windows.Forms.Button cmdPrintReport;
        private System.Windows.Forms.TextBox txtXfmrVoltage;
        private System.Windows.Forms.TextBox txtXfmrSize;
        private System.Windows.Forms.TextBox txtXfmrType;
        private System.Windows.Forms.PictureBox imgGridDivider;
        private System.Windows.Forms.Button cmdXfmrCU;
        private System.Windows.Forms.GroupBox grpCommercial;
        private System.Windows.Forms.ComboBox cboCablesPerPhase;
        private System.Windows.Forms.Label lblCablesPerPhase;
        private System.Windows.Forms.DataGridViewTextBoxColumn SecSpan;
        private System.Windows.Forms.DataGridViewButtonColumn SecType;
        private System.Windows.Forms.DataGridViewTextBoxColumn SecLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn SecVoltage;
        private System.Windows.Forms.DataGridViewTextBoxColumn SecSeason;
        private System.Windows.Forms.DataGridViewTextBoxColumn SecFlicker;
        private System.Windows.Forms.DataGridViewTextBoxColumn SecFutureSrvcCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn SecFID;
        private System.Windows.Forms.DataGridViewTextBoxColumn SecSourceFID;
        private System.Windows.Forms.DataGridViewTextBoxColumn SecFutureSrvcLoad;
        private System.Windows.Forms.DataGridViewTextBoxColumn SecFutureSrvcLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn SecVoltageDropPct;
        private System.Windows.Forms.DataGridViewTextBoxColumn SecFNO;
        private System.Windows.Forms.DataGridViewTextBoxColumn SecPendingEdit;
        private System.Windows.Forms.DataGridViewTextBoxColumn SecCU;
        private System.Windows.Forms.DataGridViewTextBoxColumn SecLengthEdit;
        private System.Windows.Forms.DataGridViewTextBoxColumn SecAllowCUEdit;
        private System.Windows.Forms.DataGridViewTextBoxColumn SrvcSpan;
        private System.Windows.Forms.DataGridViewButtonColumn SrvcType;
        private System.Windows.Forms.DataGridViewTextBoxColumn SrvcLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn SrvcVoltage;
        private System.Windows.Forms.DataGridViewTextBoxColumn SrvcSeason;
        private System.Windows.Forms.DataGridViewTextBoxColumn SrvcFlicker;
        private System.Windows.Forms.DataGridViewTextBoxColumn SrvcLoadSummerActual;
        private System.Windows.Forms.DataGridViewTextBoxColumn SrvcLoadSummerEstimate;
        private System.Windows.Forms.DataGridViewTextBoxColumn SrvcLoadWinterActual;
        private System.Windows.Forms.DataGridViewTextBoxColumn SrvcLoadWinterEstimate;
        private System.Windows.Forms.DataGridViewTextBoxColumn SrvcFID;
        private System.Windows.Forms.DataGridViewTextBoxColumn SrvcSourceFID;
        private System.Windows.Forms.DataGridViewTextBoxColumn SrvcOverrideLoad;
        private System.Windows.Forms.DataGridViewTextBoxColumn SrvcLoadUsed;
        private System.Windows.Forms.DataGridViewTextBoxColumn SrvcVoltageDropPct;
        private System.Windows.Forms.DataGridViewTextBoxColumn SrvcPendingEdit;
        private System.Windows.Forms.DataGridViewTextBoxColumn SrvcCU;
        private System.Windows.Forms.DataGridViewTextBoxColumn SrvcPtFID;
        private System.Windows.Forms.DataGridViewTextBoxColumn SrvcLengthEdit;
        private System.Windows.Forms.DataGridViewTextBoxColumn SrvcAllowCUEdit;
    }
}