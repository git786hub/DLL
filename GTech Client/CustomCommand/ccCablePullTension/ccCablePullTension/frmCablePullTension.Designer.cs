namespace GTechnology.Oncor.CustomAPI
{
    partial class frmCablePullTension
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
            this.dgvSections = new System.Windows.Forms.DataGridView();
            this.SectionType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Length = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Angle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Radius = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Depth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ForwardTension = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ForwardSWBP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReverseTension = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReverseSWBP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ForwardCOF = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReverseCOF = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Section = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FromStructure = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ToStructure = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SegmentIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grpResults = new System.Windows.Forms.GroupBox();
            this.lblCheckClearance = new System.Windows.Forms.Label();
            this.lblCheckJamRatio = new System.Windows.Forms.Label();
            this.cmdReversePullDirection = new System.Windows.Forms.Button();
            this.lblMaxLength = new System.Windows.Forms.Label();
            this.txtResultsMaxLength = new System.Windows.Forms.TextBox();
            this.lblLength = new System.Windows.Forms.Label();
            this.txtResultsTotalLength = new System.Windows.Forms.TextBox();
            this.grpResultsReverse = new System.Windows.Forms.GroupBox();
            this.txtReverseResultsTension = new System.Windows.Forms.TextBox();
            this.txtReverseTensionMaxPcnt = new System.Windows.Forms.TextBox();
            this.lblReverseTensionMaxPcnt = new System.Windows.Forms.Label();
            this.lblReverseResultsTension = new System.Windows.Forms.Label();
            this.grpForward = new System.Windows.Forms.GroupBox();
            this.txtForwardResultsTension = new System.Windows.Forms.TextBox();
            this.txtForwardTensionMaxPcnt = new System.Windows.Forms.TextBox();
            this.lblForwardTensionMaxPcnt = new System.Windows.Forms.Label();
            this.lblForwardResultsTension = new System.Windows.Forms.Label();
            this.grpLengths = new System.Windows.Forms.GroupBox();
            this.cmdClose = new System.Windows.Forms.Button();
            this.cmdCalculate = new System.Windows.Forms.Button();
            this.cmdApply = new System.Windows.Forms.Button();
            this.cmdSaveReport = new System.Windows.Forms.Button();
            this.chkNotify = new System.Windows.Forms.CheckBox();
            this.lblCable = new System.Windows.Forms.Label();
            this.cboCable = new System.Windows.Forms.ComboBox();
            this.lblDuctSize = new System.Windows.Forms.Label();
            this.cboDuctSize = new System.Windows.Forms.ComboBox();
            this.grpVerticalBends = new System.Windows.Forms.GroupBox();
            this.chkEnding = new System.Windows.Forms.CheckBox();
            this.chkStarting = new System.Windows.Forms.CheckBox();
            this.grpPulleys = new System.Windows.Forms.GroupBox();
            this.cmdAddPulley = new System.Windows.Forms.Button();
            this.lblNonStd = new System.Windows.Forms.Label();
            this.lblCableWeight = new System.Windows.Forms.Label();
            this.lblMaxTension = new System.Windows.Forms.Label();
            this.lblMaxSWBP = new System.Windows.Forms.Label();
            this.lblCableOD = new System.Windows.Forms.Label();
            this.lblCableConfig = new System.Windows.Forms.Label();
            this.lblStdBendRadius = new System.Windows.Forms.Label();
            this.txtNonStd = new System.Windows.Forms.TextBox();
            this.txtCableWeight = new System.Windows.Forms.TextBox();
            this.txtMaxTension = new System.Windows.Forms.TextBox();
            this.txtMaxSWBP = new System.Windows.Forms.TextBox();
            this.txtCableOD = new System.Windows.Forms.TextBox();
            this.txtCableConfig = new System.Windows.Forms.TextBox();
            this.txtStdBendRadius = new System.Windows.Forms.TextBox();
            this.grpCableDuctSelection = new System.Windows.Forms.GroupBox();
            this.cmdCableCU = new System.Windows.Forms.Button();
            this.lblDuctCU = new System.Windows.Forms.Label();
            this.grpCableDuctCharacteristics = new System.Windows.Forms.GroupBox();
            this.cmdPrintReport = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSections)).BeginInit();
            this.grpResults.SuspendLayout();
            this.grpResultsReverse.SuspendLayout();
            this.grpForward.SuspendLayout();
            this.grpVerticalBends.SuspendLayout();
            this.grpPulleys.SuspendLayout();
            this.grpCableDuctSelection.SuspendLayout();
            this.grpCableDuctCharacteristics.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvSections
            // 
            this.dgvSections.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSections.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSections.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SectionType,
            this.Length,
            this.Angle,
            this.Radius,
            this.Depth,
            this.ForwardTension,
            this.ForwardSWBP,
            this.ReverseTension,
            this.ReverseSWBP,
            this.ForwardCOF,
            this.ReverseCOF,
            this.Section,
            this.FromStructure,
            this.ToStructure,
            this.SegmentIndex});
            this.dgvSections.Location = new System.Drawing.Point(12, 152);
            this.dgvSections.Name = "dgvSections";
            this.dgvSections.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSections.Size = new System.Drawing.Size(643, 136);
            this.dgvSections.TabIndex = 0;
            this.dgvSections.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSections_CellEndEdit);
            this.dgvSections.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSections_CellValueChanged);
            this.dgvSections.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvSections_CurrentCellDirtyStateChanged);
            this.dgvSections.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvSections_EditingControlShowing);
            this.dgvSections.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvSections_RowsAdded);
            this.dgvSections.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dgvSections_RowsRemoved);
            this.dgvSections.SelectionChanged += new System.EventHandler(this.dgvSections_SelectionChanged);
            this.dgvSections.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgvSections_UserDeletingRow);
            // 
            // SectionType
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro;
            this.SectionType.DefaultCellStyle = dataGridViewCellStyle1;
            this.SectionType.HeaderText = "Section Type";
            this.SectionType.Name = "SectionType";
            this.SectionType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.SectionType.Width = 120;
            // 
            // Length
            // 
            this.Length.HeaderText = "Length";
            this.Length.Name = "Length";
            this.Length.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Length.Width = 50;
            // 
            // Angle
            // 
            this.Angle.HeaderText = "Angle";
            this.Angle.Name = "Angle";
            this.Angle.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Angle.Width = 50;
            // 
            // Radius
            // 
            this.Radius.HeaderText = "Radius";
            this.Radius.Name = "Radius";
            this.Radius.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Radius.Width = 50;
            // 
            // Depth
            // 
            this.Depth.HeaderText = "Depth";
            this.Depth.Name = "Depth";
            this.Depth.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Depth.Width = 50;
            // 
            // ForwardTension
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Gainsboro;
            this.ForwardTension.DefaultCellStyle = dataGridViewCellStyle2;
            this.ForwardTension.HeaderText = "Forward Tension";
            this.ForwardTension.Name = "ForwardTension";
            this.ForwardTension.ReadOnly = true;
            this.ForwardTension.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ForwardTension.Width = 70;
            // 
            // ForwardSWBP
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Gainsboro;
            this.ForwardSWBP.DefaultCellStyle = dataGridViewCellStyle3;
            this.ForwardSWBP.HeaderText = "Forward SWBP";
            this.ForwardSWBP.Name = "ForwardSWBP";
            this.ForwardSWBP.ReadOnly = true;
            this.ForwardSWBP.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ForwardSWBP.Width = 70;
            // 
            // ReverseTension
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Gainsboro;
            this.ReverseTension.DefaultCellStyle = dataGridViewCellStyle4;
            this.ReverseTension.HeaderText = "Reverse Tension";
            this.ReverseTension.Name = "ReverseTension";
            this.ReverseTension.ReadOnly = true;
            this.ReverseTension.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ReverseTension.Width = 70;
            // 
            // ReverseSWBP
            // 
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.Gainsboro;
            this.ReverseSWBP.DefaultCellStyle = dataGridViewCellStyle5;
            this.ReverseSWBP.HeaderText = "Reverse SWBP";
            this.ReverseSWBP.Name = "ReverseSWBP";
            this.ReverseSWBP.ReadOnly = true;
            this.ReverseSWBP.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ReverseSWBP.Width = 70;
            // 
            // ForwardCOF
            // 
            this.ForwardCOF.HeaderText = "ForwardCOF";
            this.ForwardCOF.Name = "ForwardCOF";
            this.ForwardCOF.ReadOnly = true;
            this.ForwardCOF.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ForwardCOF.Visible = false;
            // 
            // ReverseCOF
            // 
            this.ReverseCOF.HeaderText = "ReverseCOF";
            this.ReverseCOF.Name = "ReverseCOF";
            this.ReverseCOF.ReadOnly = true;
            this.ReverseCOF.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ReverseCOF.Visible = false;
            // 
            // Section
            // 
            this.Section.HeaderText = "Section";
            this.Section.Name = "Section";
            this.Section.ReadOnly = true;
            this.Section.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.Section.Visible = false;
            // 
            // FromStructure
            // 
            this.FromStructure.HeaderText = "StructureFrom";
            this.FromStructure.Name = "FromStructure";
            this.FromStructure.ReadOnly = true;
            this.FromStructure.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.FromStructure.Visible = false;
            // 
            // ToStructure
            // 
            this.ToStructure.HeaderText = "StructureTo";
            this.ToStructure.Name = "ToStructure";
            this.ToStructure.ReadOnly = true;
            this.ToStructure.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ToStructure.Visible = false;
            // 
            // SegmentIndex
            // 
            this.SegmentIndex.HeaderText = "SegmentIndex";
            this.SegmentIndex.Name = "SegmentIndex";
            this.SegmentIndex.ReadOnly = true;
            this.SegmentIndex.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SegmentIndex.Visible = false;
            // 
            // grpResults
            // 
            this.grpResults.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpResults.Controls.Add(this.lblCheckClearance);
            this.grpResults.Controls.Add(this.lblCheckJamRatio);
            this.grpResults.Controls.Add(this.cmdReversePullDirection);
            this.grpResults.Controls.Add(this.lblMaxLength);
            this.grpResults.Controls.Add(this.txtResultsMaxLength);
            this.grpResults.Controls.Add(this.lblLength);
            this.grpResults.Controls.Add(this.txtResultsTotalLength);
            this.grpResults.Controls.Add(this.grpResultsReverse);
            this.grpResults.Controls.Add(this.grpForward);
            this.grpResults.Controls.Add(this.grpLengths);
            this.grpResults.Location = new System.Drawing.Point(12, 294);
            this.grpResults.Name = "grpResults";
            this.grpResults.Size = new System.Drawing.Size(641, 100);
            this.grpResults.TabIndex = 1;
            this.grpResults.TabStop = false;
            this.grpResults.Text = "Results";
            // 
            // lblCheckClearance
            // 
            this.lblCheckClearance.BackColor = System.Drawing.Color.Red;
            this.lblCheckClearance.ForeColor = System.Drawing.Color.Yellow;
            this.lblCheckClearance.Location = new System.Drawing.Point(478, 41);
            this.lblCheckClearance.Name = "lblCheckClearance";
            this.lblCheckClearance.Size = new System.Drawing.Size(157, 15);
            this.lblCheckClearance.TabIndex = 12;
            // 
            // lblCheckJamRatio
            // 
            this.lblCheckJamRatio.BackColor = System.Drawing.Color.Red;
            this.lblCheckJamRatio.ForeColor = System.Drawing.Color.Yellow;
            this.lblCheckJamRatio.Location = new System.Drawing.Point(478, 20);
            this.lblCheckJamRatio.Name = "lblCheckJamRatio";
            this.lblCheckJamRatio.Size = new System.Drawing.Size(157, 15);
            this.lblCheckJamRatio.TabIndex = 11;
            // 
            // cmdReversePullDirection
            // 
            this.cmdReversePullDirection.Location = new System.Drawing.Point(508, 72);
            this.cmdReversePullDirection.Name = "cmdReversePullDirection";
            this.cmdReversePullDirection.Size = new System.Drawing.Size(127, 23);
            this.cmdReversePullDirection.TabIndex = 9;
            this.cmdReversePullDirection.Text = "Reverse Pull Direction";
            this.cmdReversePullDirection.UseVisualStyleBackColor = true;
            this.cmdReversePullDirection.Click += new System.EventHandler(this.cmdReversePullDirection_Click);
            // 
            // lblMaxLength
            // 
            this.lblMaxLength.AutoSize = true;
            this.lblMaxLength.Location = new System.Drawing.Point(408, 41);
            this.lblMaxLength.Name = "lblMaxLength";
            this.lblMaxLength.Size = new System.Drawing.Size(63, 13);
            this.lblMaxLength.TabIndex = 8;
            this.lblMaxLength.Text = "Max Length";
            // 
            // txtResultsMaxLength
            // 
            this.txtResultsMaxLength.Location = new System.Drawing.Point(408, 65);
            this.txtResultsMaxLength.Name = "txtResultsMaxLength";
            this.txtResultsMaxLength.ReadOnly = true;
            this.txtResultsMaxLength.Size = new System.Drawing.Size(63, 20);
            this.txtResultsMaxLength.TabIndex = 7;
            // 
            // lblLength
            // 
            this.lblLength.AutoSize = true;
            this.lblLength.Location = new System.Drawing.Point(337, 41);
            this.lblLength.Name = "lblLength";
            this.lblLength.Size = new System.Drawing.Size(67, 13);
            this.lblLength.TabIndex = 6;
            this.lblLength.Text = "Total Length";
            // 
            // txtResultsTotalLength
            // 
            this.txtResultsTotalLength.Location = new System.Drawing.Point(338, 65);
            this.txtResultsTotalLength.Name = "txtResultsTotalLength";
            this.txtResultsTotalLength.ReadOnly = true;
            this.txtResultsTotalLength.Size = new System.Drawing.Size(63, 20);
            this.txtResultsTotalLength.TabIndex = 5;
            // 
            // grpResultsReverse
            // 
            this.grpResultsReverse.Controls.Add(this.txtReverseResultsTension);
            this.grpResultsReverse.Controls.Add(this.txtReverseTensionMaxPcnt);
            this.grpResultsReverse.Controls.Add(this.lblReverseTensionMaxPcnt);
            this.grpResultsReverse.Controls.Add(this.lblReverseResultsTension);
            this.grpResultsReverse.Location = new System.Drawing.Point(169, 20);
            this.grpResultsReverse.Name = "grpResultsReverse";
            this.grpResultsReverse.Size = new System.Drawing.Size(160, 74);
            this.grpResultsReverse.TabIndex = 4;
            this.grpResultsReverse.TabStop = false;
            this.grpResultsReverse.Text = "Reverse";
            // 
            // txtReverseResultsTension
            // 
            this.txtReverseResultsTension.Location = new System.Drawing.Point(14, 45);
            this.txtReverseResultsTension.Name = "txtReverseResultsTension";
            this.txtReverseResultsTension.ReadOnly = true;
            this.txtReverseResultsTension.Size = new System.Drawing.Size(63, 20);
            this.txtReverseResultsTension.TabIndex = 3;
            // 
            // txtReverseTensionMaxPcnt
            // 
            this.txtReverseTensionMaxPcnt.Location = new System.Drawing.Point(83, 45);
            this.txtReverseTensionMaxPcnt.Name = "txtReverseTensionMaxPcnt";
            this.txtReverseTensionMaxPcnt.ReadOnly = true;
            this.txtReverseTensionMaxPcnt.Size = new System.Drawing.Size(63, 20);
            this.txtReverseTensionMaxPcnt.TabIndex = 2;
            // 
            // lblReverseTensionMaxPcnt
            // 
            this.lblReverseTensionMaxPcnt.AutoSize = true;
            this.lblReverseTensionMaxPcnt.Location = new System.Drawing.Point(75, 21);
            this.lblReverseTensionMaxPcnt.Name = "lblReverseTensionMaxPcnt";
            this.lblReverseTensionMaxPcnt.Size = new System.Drawing.Size(79, 13);
            this.lblReverseTensionMaxPcnt.TabIndex = 1;
            this.lblReverseTensionMaxPcnt.Text = "Max Tension %";
            // 
            // lblReverseResultsTension
            // 
            this.lblReverseResultsTension.AutoSize = true;
            this.lblReverseResultsTension.Location = new System.Drawing.Point(11, 21);
            this.lblReverseResultsTension.Name = "lblReverseResultsTension";
            this.lblReverseResultsTension.Size = new System.Drawing.Size(45, 13);
            this.lblReverseResultsTension.TabIndex = 0;
            this.lblReverseResultsTension.Text = "Tension";
            // 
            // grpForward
            // 
            this.grpForward.Controls.Add(this.txtForwardResultsTension);
            this.grpForward.Controls.Add(this.txtForwardTensionMaxPcnt);
            this.grpForward.Controls.Add(this.lblForwardTensionMaxPcnt);
            this.grpForward.Controls.Add(this.lblForwardResultsTension);
            this.grpForward.Location = new System.Drawing.Point(6, 20);
            this.grpForward.Name = "grpForward";
            this.grpForward.Size = new System.Drawing.Size(160, 74);
            this.grpForward.TabIndex = 0;
            this.grpForward.TabStop = false;
            this.grpForward.Text = "Forward";
            // 
            // txtForwardResultsTension
            // 
            this.txtForwardResultsTension.Location = new System.Drawing.Point(13, 45);
            this.txtForwardResultsTension.Name = "txtForwardResultsTension";
            this.txtForwardResultsTension.ReadOnly = true;
            this.txtForwardResultsTension.Size = new System.Drawing.Size(63, 20);
            this.txtForwardResultsTension.TabIndex = 3;
            // 
            // txtForwardTensionMaxPcnt
            // 
            this.txtForwardTensionMaxPcnt.Location = new System.Drawing.Point(82, 45);
            this.txtForwardTensionMaxPcnt.Name = "txtForwardTensionMaxPcnt";
            this.txtForwardTensionMaxPcnt.ReadOnly = true;
            this.txtForwardTensionMaxPcnt.Size = new System.Drawing.Size(63, 20);
            this.txtForwardTensionMaxPcnt.TabIndex = 2;
            // 
            // lblForwardTensionMaxPcnt
            // 
            this.lblForwardTensionMaxPcnt.AutoSize = true;
            this.lblForwardTensionMaxPcnt.Location = new System.Drawing.Point(75, 21);
            this.lblForwardTensionMaxPcnt.Name = "lblForwardTensionMaxPcnt";
            this.lblForwardTensionMaxPcnt.Size = new System.Drawing.Size(79, 13);
            this.lblForwardTensionMaxPcnt.TabIndex = 1;
            this.lblForwardTensionMaxPcnt.Text = "Max Tension %";
            // 
            // lblForwardResultsTension
            // 
            this.lblForwardResultsTension.AutoSize = true;
            this.lblForwardResultsTension.Location = new System.Drawing.Point(10, 21);
            this.lblForwardResultsTension.Name = "lblForwardResultsTension";
            this.lblForwardResultsTension.Size = new System.Drawing.Size(45, 13);
            this.lblForwardResultsTension.TabIndex = 0;
            this.lblForwardResultsTension.Text = "Tension";
            // 
            // grpLengths
            // 
            this.grpLengths.Location = new System.Drawing.Point(331, 21);
            this.grpLengths.Name = "grpLengths";
            this.grpLengths.Size = new System.Drawing.Size(145, 74);
            this.grpLengths.TabIndex = 10;
            this.grpLengths.TabStop = false;
            this.grpLengths.Text = "Length";
            // 
            // cmdClose
            // 
            this.cmdClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdClose.Location = new System.Drawing.Point(580, 410);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(75, 23);
            this.cmdClose.TabIndex = 2;
            this.cmdClose.Text = "Close";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // cmdCalculate
            // 
            this.cmdCalculate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCalculate.Location = new System.Drawing.Point(499, 410);
            this.cmdCalculate.Name = "cmdCalculate";
            this.cmdCalculate.Size = new System.Drawing.Size(75, 23);
            this.cmdCalculate.TabIndex = 3;
            this.cmdCalculate.Text = "Calculate";
            this.cmdCalculate.UseVisualStyleBackColor = true;
            this.cmdCalculate.Click += new System.EventHandler(this.cmdCalculate_Click);
            // 
            // cmdApply
            // 
            this.cmdApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdApply.Location = new System.Drawing.Point(418, 410);
            this.cmdApply.Name = "cmdApply";
            this.cmdApply.Size = new System.Drawing.Size(75, 23);
            this.cmdApply.TabIndex = 4;
            this.cmdApply.Text = "Apply";
            this.cmdApply.UseVisualStyleBackColor = true;
            this.cmdApply.Click += new System.EventHandler(this.cmdApply_Click);
            // 
            // cmdSaveReport
            // 
            this.cmdSaveReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdSaveReport.Location = new System.Drawing.Point(337, 410);
            this.cmdSaveReport.Name = "cmdSaveReport";
            this.cmdSaveReport.Size = new System.Drawing.Size(75, 23);
            this.cmdSaveReport.TabIndex = 5;
            this.cmdSaveReport.Text = "Save Report";
            this.cmdSaveReport.UseVisualStyleBackColor = true;
            this.cmdSaveReport.Click += new System.EventHandler(this.cmdSaveReport_Click);
            // 
            // chkNotify
            // 
            this.chkNotify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkNotify.AutoSize = true;
            this.chkNotify.Location = new System.Drawing.Point(13, 414);
            this.chkNotify.Name = "chkNotify";
            this.chkNotify.Size = new System.Drawing.Size(98, 17);
            this.chkNotify.TabIndex = 6;
            this.chkNotify.Text = "Notify Changes";
            this.chkNotify.UseVisualStyleBackColor = true;
            // 
            // lblCable
            // 
            this.lblCable.AutoSize = true;
            this.lblCable.Location = new System.Drawing.Point(168, 16);
            this.lblCable.Name = "lblCable";
            this.lblCable.Size = new System.Drawing.Size(90, 13);
            this.lblCable.TabIndex = 7;
            this.lblCable.Text = "Cable Description";
            this.lblCable.Click += new System.EventHandler(this.lblCable_Click);
            // 
            // cboCable
            // 
            this.cboCable.BackColor = System.Drawing.SystemColors.Window;
            this.cboCable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCable.FormattingEnabled = true;
            this.cboCable.Location = new System.Drawing.Point(171, 38);
            this.cboCable.Name = "cboCable";
            this.cboCable.Size = new System.Drawing.Size(270, 21);
            this.cboCable.TabIndex = 8;
            this.cboCable.SelectedValueChanged += new System.EventHandler(this.cboCable_SelectedValueChanged);
            // 
            // lblDuctSize
            // 
            this.lblDuctSize.AutoSize = true;
            this.lblDuctSize.Location = new System.Drawing.Point(448, 18);
            this.lblDuctSize.Name = "lblDuctSize";
            this.lblDuctSize.Size = new System.Drawing.Size(53, 13);
            this.lblDuctSize.TabIndex = 11;
            this.lblDuctSize.Text = "Duct Size";
            // 
            // cboDuctSize
            // 
            this.cboDuctSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDuctSize.FormattingEnabled = true;
            this.cboDuctSize.Location = new System.Drawing.Point(447, 38);
            this.cboDuctSize.Name = "cboDuctSize";
            this.cboDuctSize.Size = new System.Drawing.Size(81, 21);
            this.cboDuctSize.TabIndex = 12;
            this.cboDuctSize.SelectedValueChanged += new System.EventHandler(this.cboDuctSize_SelectedValueChanged);
            // 
            // grpVerticalBends
            // 
            this.grpVerticalBends.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grpVerticalBends.Controls.Add(this.chkEnding);
            this.grpVerticalBends.Controls.Add(this.chkStarting);
            this.grpVerticalBends.Location = new System.Drawing.Point(552, 10);
            this.grpVerticalBends.Name = "grpVerticalBends";
            this.grpVerticalBends.Size = new System.Drawing.Size(103, 66);
            this.grpVerticalBends.TabIndex = 13;
            this.grpVerticalBends.TabStop = false;
            this.grpVerticalBends.Text = "Vertical Bends";
            // 
            // chkEnding
            // 
            this.chkEnding.AutoSize = true;
            this.chkEnding.Location = new System.Drawing.Point(16, 42);
            this.chkEnding.Name = "chkEnding";
            this.chkEnding.Size = new System.Drawing.Size(59, 17);
            this.chkEnding.TabIndex = 1;
            this.chkEnding.Text = "Ending";
            this.chkEnding.UseVisualStyleBackColor = true;
            this.chkEnding.CheckedChanged += new System.EventHandler(this.chkEnding_CheckedChanged);
            // 
            // chkStarting
            // 
            this.chkStarting.AutoSize = true;
            this.chkStarting.Location = new System.Drawing.Point(16, 19);
            this.chkStarting.Name = "chkStarting";
            this.chkStarting.Size = new System.Drawing.Size(62, 17);
            this.chkStarting.TabIndex = 0;
            this.chkStarting.Text = "Starting";
            this.chkStarting.UseVisualStyleBackColor = true;
            this.chkStarting.CheckedChanged += new System.EventHandler(this.chkStarting_CheckedChanged);
            // 
            // grpPulleys
            // 
            this.grpPulleys.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grpPulleys.Controls.Add(this.cmdAddPulley);
            this.grpPulleys.Location = new System.Drawing.Point(552, 84);
            this.grpPulleys.Name = "grpPulleys";
            this.grpPulleys.Size = new System.Drawing.Size(103, 62);
            this.grpPulleys.TabIndex = 14;
            this.grpPulleys.TabStop = false;
            // 
            // cmdAddPulley
            // 
            this.cmdAddPulley.Location = new System.Drawing.Point(16, 19);
            this.cmdAddPulley.Name = "cmdAddPulley";
            this.cmdAddPulley.Size = new System.Drawing.Size(75, 23);
            this.cmdAddPulley.TabIndex = 31;
            this.cmdAddPulley.Text = "Add Pulley";
            this.cmdAddPulley.UseVisualStyleBackColor = true;
            this.cmdAddPulley.Click += new System.EventHandler(this.cmdAddPulley_Click);
            // 
            // lblNonStd
            // 
            this.lblNonStd.AutoSize = true;
            this.lblNonStd.Location = new System.Drawing.Point(19, 17);
            this.lblNonStd.Name = "lblNonStd";
            this.lblNonStd.Size = new System.Drawing.Size(54, 13);
            this.lblNonStd.TabIndex = 15;
            this.lblNonStd.Text = "Non / Std";
            // 
            // lblCableWeight
            // 
            this.lblCableWeight.AutoSize = true;
            this.lblCableWeight.Location = new System.Drawing.Point(79, 17);
            this.lblCableWeight.Name = "lblCableWeight";
            this.lblCableWeight.Size = new System.Drawing.Size(71, 13);
            this.lblCableWeight.TabIndex = 16;
            this.lblCableWeight.Text = "Cable Weight";
            // 
            // lblMaxTension
            // 
            this.lblMaxTension.AutoSize = true;
            this.lblMaxTension.Location = new System.Drawing.Point(156, 17);
            this.lblMaxTension.Name = "lblMaxTension";
            this.lblMaxTension.Size = new System.Drawing.Size(68, 13);
            this.lblMaxTension.TabIndex = 17;
            this.lblMaxTension.Text = "Max Tension";
            // 
            // lblMaxSWBP
            // 
            this.lblMaxSWBP.AutoSize = true;
            this.lblMaxSWBP.Location = new System.Drawing.Point(230, 17);
            this.lblMaxSWBP.Name = "lblMaxSWBP";
            this.lblMaxSWBP.Size = new System.Drawing.Size(62, 13);
            this.lblMaxSWBP.TabIndex = 18;
            this.lblMaxSWBP.Text = "Max SWBP";
            // 
            // lblCableOD
            // 
            this.lblCableOD.AutoSize = true;
            this.lblCableOD.Location = new System.Drawing.Point(296, 17);
            this.lblCableOD.Name = "lblCableOD";
            this.lblCableOD.Size = new System.Drawing.Size(53, 13);
            this.lblCableOD.TabIndex = 19;
            this.lblCableOD.Text = "Cable OD";
            // 
            // lblCableConfig
            // 
            this.lblCableConfig.AutoSize = true;
            this.lblCableConfig.Location = new System.Drawing.Point(360, 17);
            this.lblCableConfig.Name = "lblCableConfig";
            this.lblCableConfig.Size = new System.Drawing.Size(67, 13);
            this.lblCableConfig.TabIndex = 20;
            this.lblCableConfig.Text = "Cable Config";
            // 
            // lblStdBendRadius
            // 
            this.lblStdBendRadius.AutoSize = true;
            this.lblStdBendRadius.Location = new System.Drawing.Point(444, 17);
            this.lblStdBendRadius.Name = "lblStdBendRadius";
            this.lblStdBendRadius.Size = new System.Drawing.Size(87, 13);
            this.lblStdBendRadius.TabIndex = 21;
            this.lblStdBendRadius.Text = "Std Bend Radius";
            // 
            // txtNonStd
            // 
            this.txtNonStd.Location = new System.Drawing.Point(22, 33);
            this.txtNonStd.Name = "txtNonStd";
            this.txtNonStd.ReadOnly = true;
            this.txtNonStd.Size = new System.Drawing.Size(51, 20);
            this.txtNonStd.TabIndex = 22;
            // 
            // txtCableWeight
            // 
            this.txtCableWeight.Location = new System.Drawing.Point(82, 33);
            this.txtCableWeight.Name = "txtCableWeight";
            this.txtCableWeight.ReadOnly = true;
            this.txtCableWeight.Size = new System.Drawing.Size(68, 20);
            this.txtCableWeight.TabIndex = 23;
            // 
            // txtMaxTension
            // 
            this.txtMaxTension.Location = new System.Drawing.Point(159, 33);
            this.txtMaxTension.Name = "txtMaxTension";
            this.txtMaxTension.ReadOnly = true;
            this.txtMaxTension.Size = new System.Drawing.Size(65, 20);
            this.txtMaxTension.TabIndex = 24;
            // 
            // txtMaxSWBP
            // 
            this.txtMaxSWBP.Location = new System.Drawing.Point(233, 33);
            this.txtMaxSWBP.Name = "txtMaxSWBP";
            this.txtMaxSWBP.ReadOnly = true;
            this.txtMaxSWBP.Size = new System.Drawing.Size(59, 20);
            this.txtMaxSWBP.TabIndex = 25;
            // 
            // txtCableOD
            // 
            this.txtCableOD.Location = new System.Drawing.Point(297, 33);
            this.txtCableOD.Name = "txtCableOD";
            this.txtCableOD.ReadOnly = true;
            this.txtCableOD.Size = new System.Drawing.Size(59, 20);
            this.txtCableOD.TabIndex = 26;
            // 
            // txtCableConfig
            // 
            this.txtCableConfig.Location = new System.Drawing.Point(363, 33);
            this.txtCableConfig.Name = "txtCableConfig";
            this.txtCableConfig.ReadOnly = true;
            this.txtCableConfig.Size = new System.Drawing.Size(75, 20);
            this.txtCableConfig.TabIndex = 27;
            // 
            // txtStdBendRadius
            // 
            this.txtStdBendRadius.Location = new System.Drawing.Point(447, 33);
            this.txtStdBendRadius.Name = "txtStdBendRadius";
            this.txtStdBendRadius.ReadOnly = true;
            this.txtStdBendRadius.Size = new System.Drawing.Size(81, 20);
            this.txtStdBendRadius.TabIndex = 28;
            // 
            // grpCableDuctSelection
            // 
            this.grpCableDuctSelection.Controls.Add(this.cmdCableCU);
            this.grpCableDuctSelection.Controls.Add(this.lblDuctCU);
            this.grpCableDuctSelection.Controls.Add(this.lblCable);
            this.grpCableDuctSelection.Controls.Add(this.cboCable);
            this.grpCableDuctSelection.Controls.Add(this.lblDuctSize);
            this.grpCableDuctSelection.Controls.Add(this.cboDuctSize);
            this.grpCableDuctSelection.Location = new System.Drawing.Point(12, 10);
            this.grpCableDuctSelection.Name = "grpCableDuctSelection";
            this.grpCableDuctSelection.Size = new System.Drawing.Size(534, 66);
            this.grpCableDuctSelection.TabIndex = 29;
            this.grpCableDuctSelection.TabStop = false;
            this.grpCableDuctSelection.Text = "Cable and Duct Selection";
            // 
            // cmdCableCU
            // 
            this.cmdCableCU.Location = new System.Drawing.Point(21, 36);
            this.cmdCableCU.Name = "cmdCableCU";
            this.cmdCableCU.Size = new System.Drawing.Size(139, 23);
            this.cmdCableCU.TabIndex = 14;
            this.cmdCableCU.UseVisualStyleBackColor = true;
            this.cmdCableCU.Click += new System.EventHandler(this.cmdDuctCU_Click);
            // 
            // lblDuctCU
            // 
            this.lblDuctCU.AutoSize = true;
            this.lblDuctCU.Location = new System.Drawing.Point(19, 16);
            this.lblDuctCU.Name = "lblDuctCU";
            this.lblDuctCU.Size = new System.Drawing.Size(34, 13);
            this.lblDuctCU.TabIndex = 13;
            this.lblDuctCU.Text = "Cable";
            // 
            // grpCableDuctCharacteristics
            // 
            this.grpCableDuctCharacteristics.Controls.Add(this.txtStdBendRadius);
            this.grpCableDuctCharacteristics.Controls.Add(this.txtCableConfig);
            this.grpCableDuctCharacteristics.Controls.Add(this.lblNonStd);
            this.grpCableDuctCharacteristics.Controls.Add(this.txtCableOD);
            this.grpCableDuctCharacteristics.Controls.Add(this.lblCableWeight);
            this.grpCableDuctCharacteristics.Controls.Add(this.txtMaxSWBP);
            this.grpCableDuctCharacteristics.Controls.Add(this.lblMaxTension);
            this.grpCableDuctCharacteristics.Controls.Add(this.txtMaxTension);
            this.grpCableDuctCharacteristics.Controls.Add(this.lblMaxSWBP);
            this.grpCableDuctCharacteristics.Controls.Add(this.txtCableWeight);
            this.grpCableDuctCharacteristics.Controls.Add(this.lblCableOD);
            this.grpCableDuctCharacteristics.Controls.Add(this.txtNonStd);
            this.grpCableDuctCharacteristics.Controls.Add(this.lblCableConfig);
            this.grpCableDuctCharacteristics.Controls.Add(this.lblStdBendRadius);
            this.grpCableDuctCharacteristics.Location = new System.Drawing.Point(12, 84);
            this.grpCableDuctCharacteristics.Name = "grpCableDuctCharacteristics";
            this.grpCableDuctCharacteristics.Size = new System.Drawing.Size(534, 62);
            this.grpCableDuctCharacteristics.TabIndex = 30;
            this.grpCableDuctCharacteristics.TabStop = false;
            this.grpCableDuctCharacteristics.Text = "Cable and Duct Characteristics";
            // 
            // cmdPrintReport
            // 
            this.cmdPrintReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdPrintReport.Location = new System.Drawing.Point(256, 410);
            this.cmdPrintReport.Name = "cmdPrintReport";
            this.cmdPrintReport.Size = new System.Drawing.Size(75, 23);
            this.cmdPrintReport.TabIndex = 31;
            this.cmdPrintReport.Text = "Print Report";
            this.cmdPrintReport.UseVisualStyleBackColor = true;
            this.cmdPrintReport.Click += new System.EventHandler(this.cmdPrintReport_Click);
            // 
            // frmCablePullTension
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(665, 442);
            this.Controls.Add(this.cmdPrintReport);
            this.Controls.Add(this.grpPulleys);
            this.Controls.Add(this.grpVerticalBends);
            this.Controls.Add(this.chkNotify);
            this.Controls.Add(this.cmdSaveReport);
            this.Controls.Add(this.cmdApply);
            this.Controls.Add(this.cmdCalculate);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.grpResults);
            this.Controls.Add(this.dgvSections);
            this.Controls.Add(this.grpCableDuctSelection);
            this.Controls.Add(this.grpCableDuctCharacteristics);
            this.MaximizeBox = false;
            this.Name = "frmCablePullTension";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Cable Pull Tension";
            this.Activated += new System.EventHandler(this.frmCablePullTension_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCablePullTension_FormClosing);
            this.Load += new System.EventHandler(this.frmCablePullTension_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSections)).EndInit();
            this.grpResults.ResumeLayout(false);
            this.grpResults.PerformLayout();
            this.grpResultsReverse.ResumeLayout(false);
            this.grpResultsReverse.PerformLayout();
            this.grpForward.ResumeLayout(false);
            this.grpForward.PerformLayout();
            this.grpVerticalBends.ResumeLayout(false);
            this.grpVerticalBends.PerformLayout();
            this.grpPulleys.ResumeLayout(false);
            this.grpCableDuctSelection.ResumeLayout(false);
            this.grpCableDuctSelection.PerformLayout();
            this.grpCableDuctCharacteristics.ResumeLayout(false);
            this.grpCableDuctCharacteristics.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvSections;
        private System.Windows.Forms.GroupBox grpResults;
        private System.Windows.Forms.GroupBox grpResultsReverse;
        private System.Windows.Forms.TextBox txtReverseResultsTension;
        private System.Windows.Forms.TextBox txtReverseTensionMaxPcnt;
        private System.Windows.Forms.Label lblReverseTensionMaxPcnt;
        private System.Windows.Forms.Label lblReverseResultsTension;
        private System.Windows.Forms.GroupBox grpForward;
        private System.Windows.Forms.TextBox txtForwardResultsTension;
        private System.Windows.Forms.TextBox txtForwardTensionMaxPcnt;
        private System.Windows.Forms.Label lblForwardTensionMaxPcnt;
        private System.Windows.Forms.Label lblForwardResultsTension;
        private System.Windows.Forms.Label lblMaxLength;
        private System.Windows.Forms.TextBox txtResultsMaxLength;
        private System.Windows.Forms.Label lblLength;
        private System.Windows.Forms.TextBox txtResultsTotalLength;
        private System.Windows.Forms.Button cmdReversePullDirection;
        private System.Windows.Forms.Button cmdClose;
        private System.Windows.Forms.Button cmdCalculate;
        private System.Windows.Forms.Button cmdApply;
        private System.Windows.Forms.Button cmdSaveReport;
        private System.Windows.Forms.CheckBox chkNotify;
        private System.Windows.Forms.Label lblCable;
        private System.Windows.Forms.ComboBox cboCable;
        private System.Windows.Forms.Label lblDuctSize;
        private System.Windows.Forms.ComboBox cboDuctSize;
        private System.Windows.Forms.GroupBox grpVerticalBends;
        private System.Windows.Forms.CheckBox chkEnding;
        private System.Windows.Forms.CheckBox chkStarting;
        private System.Windows.Forms.GroupBox grpPulleys;
        private System.Windows.Forms.Label lblNonStd;
        private System.Windows.Forms.Label lblCableWeight;
        private System.Windows.Forms.Label lblMaxTension;
        private System.Windows.Forms.Label lblMaxSWBP;
        private System.Windows.Forms.Label lblCableOD;
        private System.Windows.Forms.Label lblCableConfig;
        private System.Windows.Forms.Label lblStdBendRadius;
        private System.Windows.Forms.TextBox txtNonStd;
        private System.Windows.Forms.TextBox txtCableWeight;
        private System.Windows.Forms.TextBox txtMaxTension;
        private System.Windows.Forms.TextBox txtMaxSWBP;
        private System.Windows.Forms.TextBox txtCableOD;
        private System.Windows.Forms.TextBox txtCableConfig;
        private System.Windows.Forms.TextBox txtStdBendRadius;
        private System.Windows.Forms.GroupBox grpCableDuctSelection;
        private System.Windows.Forms.GroupBox grpCableDuctCharacteristics;
        private System.Windows.Forms.Label lblDuctCU;
        private System.Windows.Forms.GroupBox grpLengths;
        private System.Windows.Forms.Button cmdAddPulley;
        private System.Windows.Forms.Button cmdPrintReport;
        private System.Windows.Forms.Label lblCheckClearance;
        private System.Windows.Forms.Label lblCheckJamRatio;
        private System.Windows.Forms.Button cmdCableCU;
        private System.Windows.Forms.DataGridViewComboBoxColumn SectionType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Length;
        private System.Windows.Forms.DataGridViewTextBoxColumn Angle;
        private System.Windows.Forms.DataGridViewTextBoxColumn Radius;
        private System.Windows.Forms.DataGridViewTextBoxColumn Depth;
        private System.Windows.Forms.DataGridViewTextBoxColumn ForwardTension;
        private System.Windows.Forms.DataGridViewTextBoxColumn ForwardSWBP;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReverseTension;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReverseSWBP;
        private System.Windows.Forms.DataGridViewTextBoxColumn ForwardCOF;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReverseCOF;
        private System.Windows.Forms.DataGridViewTextBoxColumn Section;
        private System.Windows.Forms.DataGridViewTextBoxColumn FromStructure;
        private System.Windows.Forms.DataGridViewTextBoxColumn ToStructure;
        private System.Windows.Forms.DataGridViewTextBoxColumn SegmentIndex;
    }
}

