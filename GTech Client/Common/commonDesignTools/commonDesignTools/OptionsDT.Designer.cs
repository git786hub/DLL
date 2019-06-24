namespace GTechnology.Oncor.CustomAPI
{
    partial class OptionsDT
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsDT));
            this.grpMapOutput = new System.Windows.Forms.GroupBox();
            this.GMProblemColorButton = new AxSTYCTLLib.AxGMColorButton();
            this.GMFillColorButton = new AxSTYCTLLib.AxGMColorButton();
            this.GMTraceColorButton = new AxSTYCTLLib.AxGMColorButton();
            this.lblProblemColor = new System.Windows.Forms.Label();
            this.lblFillcolor = new System.Windows.Forms.Label();
            this.lblTraceColor = new System.Windows.Forms.Label();
            this.cboLineweight = new AxSTYCTLLib.AxGMWeightCombobox();
            this.lblLineWeight = new System.Windows.Forms.Label();
            this.chkOverrideStyle = new System.Windows.Forms.CheckBox();
            this.optAllWindows = new System.Windows.Forms.RadioButton();
            this.optActiveWindow = new System.Windows.Forms.RadioButton();
            this.chkOutputMapWindow = new System.Windows.Forms.CheckBox();
            this.fraMapWindowSettings = new System.Windows.Forms.GroupBox();
            this.spnZoom = new System.Windows.Forms.HScrollBar();
            this.lblZoom = new System.Windows.Forms.Label();
            this.txtZoom = new System.Windows.Forms.TextBox();
            this.optFitAndZoomOut = new System.Windows.Forms.RadioButton();
            this.optCenterAtCurrentScale = new System.Windows.Forms.RadioButton();
            this.optViewAtCurrentScale = new System.Windows.Forms.RadioButton();
            this.chkOverrideMapWindowSettings = new System.Windows.Forms.CheckBox();
            this.chkNotifyChanges = new System.Windows.Forms.CheckBox();
            this.cmdApply = new System.Windows.Forms.Button();
            this.cmdClose = new System.Windows.Forms.Button();
            this.grpMapOutput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GMProblemColorButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GMFillColorButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GMTraceColorButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboLineweight)).BeginInit();
            this.fraMapWindowSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpMapOutput
            // 
            this.grpMapOutput.Controls.Add(this.GMProblemColorButton);
            this.grpMapOutput.Controls.Add(this.GMFillColorButton);
            this.grpMapOutput.Controls.Add(this.GMTraceColorButton);
            this.grpMapOutput.Controls.Add(this.lblProblemColor);
            this.grpMapOutput.Controls.Add(this.lblFillcolor);
            this.grpMapOutput.Controls.Add(this.lblTraceColor);
            this.grpMapOutput.Controls.Add(this.cboLineweight);
            this.grpMapOutput.Controls.Add(this.lblLineWeight);
            this.grpMapOutput.Controls.Add(this.chkOverrideStyle);
            this.grpMapOutput.Controls.Add(this.optAllWindows);
            this.grpMapOutput.Controls.Add(this.optActiveWindow);
            this.grpMapOutput.Controls.Add(this.chkOutputMapWindow);
            this.grpMapOutput.Location = new System.Drawing.Point(13, 13);
            this.grpMapOutput.Name = "grpMapOutput";
            this.grpMapOutput.Size = new System.Drawing.Size(250, 210);
            this.grpMapOutput.TabIndex = 0;
            this.grpMapOutput.TabStop = false;
            this.grpMapOutput.Text = "Map output";
            // 
            // GMProblemColorButton
            // 
            this.GMProblemColorButton.ForeColor = System.Drawing.Color.Red;
            this.GMProblemColorButton.Location = new System.Drawing.Point(174, 169);
            this.GMProblemColorButton.Name = "GMProblemColorButton";
            this.GMProblemColorButton.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("GMProblemColorButton.OcxState")));
            this.GMProblemColorButton.Size = new System.Drawing.Size(34, 34);
            this.GMProblemColorButton.TabIndex = 19;
            this.GMProblemColorButton.ColorChanged += new AxSTYCTLLib._DColorBtnEvents_ColorChangedEventHandler(this.GMProblemColorButton_ColorChanged);
            // 
            // GMFillColorButton
            // 
            this.GMFillColorButton.ForeColor = System.Drawing.Color.Red;
            this.GMFillColorButton.Location = new System.Drawing.Point(104, 169);
            this.GMFillColorButton.Name = "GMFillColorButton";
            this.GMFillColorButton.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("GMFillColorButton.OcxState")));
            this.GMFillColorButton.Size = new System.Drawing.Size(34, 34);
            this.GMFillColorButton.TabIndex = 18;
            this.GMFillColorButton.ColorChanged += new AxSTYCTLLib._DColorBtnEvents_ColorChangedEventHandler(this.GMFillColorButton_ColorChanged);
            // 
            // GMTraceColorButton
            // 
            this.GMTraceColorButton.ForeColor = System.Drawing.Color.Red;
            this.GMTraceColorButton.Location = new System.Drawing.Point(34, 169);
            this.GMTraceColorButton.Name = "GMTraceColorButton";
            this.GMTraceColorButton.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("GMTraceColorButton.OcxState")));
            this.GMTraceColorButton.Size = new System.Drawing.Size(34, 34);
            this.GMTraceColorButton.TabIndex = 17;
            this.GMTraceColorButton.ColorChanged += new AxSTYCTLLib._DColorBtnEvents_ColorChangedEventHandler(this.GMTraceColorButton_ColorChanged);
            // 
            // lblProblemColor
            // 
            this.lblProblemColor.AutoSize = true;
            this.lblProblemColor.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProblemColor.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblProblemColor.Location = new System.Drawing.Point(171, 151);
            this.lblProblemColor.Name = "lblProblemColor";
            this.lblProblemColor.Size = new System.Drawing.Size(71, 13);
            this.lblProblemColor.TabIndex = 16;
            this.lblProblemColor.Text = "Problem color";
            this.lblProblemColor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblFillcolor
            // 
            this.lblFillcolor.AutoSize = true;
            this.lblFillcolor.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFillcolor.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblFillcolor.Location = new System.Drawing.Point(101, 151);
            this.lblFillcolor.Name = "lblFillcolor";
            this.lblFillcolor.Size = new System.Drawing.Size(45, 13);
            this.lblFillcolor.TabIndex = 15;
            this.lblFillcolor.Text = "Fill color";
            this.lblFillcolor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTraceColor
            // 
            this.lblTraceColor.AutoSize = true;
            this.lblTraceColor.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTraceColor.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblTraceColor.Location = new System.Drawing.Point(20, 151);
            this.lblTraceColor.Name = "lblTraceColor";
            this.lblTraceColor.Size = new System.Drawing.Size(60, 13);
            this.lblTraceColor.TabIndex = 14;
            this.lblTraceColor.Text = "Trace color";
            this.lblTraceColor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboLineweight
            // 
            this.cboLineweight.Enabled = true;
            this.cboLineweight.Location = new System.Drawing.Point(84, 118);
            this.cboLineweight.Name = "cboLineweight";
            this.cboLineweight.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cboLineweight.OcxState")));
            this.cboLineweight.Size = new System.Drawing.Size(151, 24);
            this.cboLineweight.TabIndex = 13;
            this.cboLineweight.WeightChanged += new AxSTYCTLLib._DWeightEvents_WeightChangedEventHandler(this.cboLineweight_WeightChanged);
            // 
            // lblLineWeight
            // 
            this.lblLineWeight.AutoSize = true;
            this.lblLineWeight.Location = new System.Drawing.Point(19, 122);
            this.lblLineWeight.Name = "lblLineWeight";
            this.lblLineWeight.Size = new System.Drawing.Size(64, 13);
            this.lblLineWeight.TabIndex = 4;
            this.lblLineWeight.Text = "Line weight:";
            // 
            // chkOverrideStyle
            // 
            this.chkOverrideStyle.AutoSize = true;
            this.chkOverrideStyle.Checked = true;
            this.chkOverrideStyle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOverrideStyle.Location = new System.Drawing.Point(19, 93);
            this.chkOverrideStyle.Name = "chkOverrideStyle";
            this.chkOverrideStyle.Size = new System.Drawing.Size(90, 17);
            this.chkOverrideStyle.TabIndex = 3;
            this.chkOverrideStyle.Text = "Override style";
            this.chkOverrideStyle.UseVisualStyleBackColor = true;
            this.chkOverrideStyle.CheckedChanged += new System.EventHandler(this.chkOverrideStyle_CheckedChanged);
            // 
            // optAllWindows
            // 
            this.optAllWindows.AutoSize = true;
            this.optAllWindows.Location = new System.Drawing.Point(36, 66);
            this.optAllWindows.Name = "optAllWindows";
            this.optAllWindows.Size = new System.Drawing.Size(83, 17);
            this.optAllWindows.TabIndex = 2;
            this.optAllWindows.Text = "All Windows";
            this.optAllWindows.UseVisualStyleBackColor = true;
            this.optAllWindows.CheckedChanged += new System.EventHandler(this.optAllWindows_CheckedChanged);
            // 
            // optActiveWindow
            // 
            this.optActiveWindow.AutoSize = true;
            this.optActiveWindow.Checked = true;
            this.optActiveWindow.Location = new System.Drawing.Point(36, 43);
            this.optActiveWindow.Name = "optActiveWindow";
            this.optActiveWindow.Size = new System.Drawing.Size(121, 17);
            this.optActiveWindow.TabIndex = 1;
            this.optActiveWindow.TabStop = true;
            this.optActiveWindow.Text = "Active Window Only";
            this.optActiveWindow.UseVisualStyleBackColor = true;
            this.optActiveWindow.CheckedChanged += new System.EventHandler(this.optActiveWindow_CheckedChanged);
            // 
            // chkOutputMapWindow
            // 
            this.chkOutputMapWindow.AutoSize = true;
            this.chkOutputMapWindow.Checked = true;
            this.chkOutputMapWindow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOutputMapWindow.Location = new System.Drawing.Point(19, 20);
            this.chkOutputMapWindow.Name = "chkOutputMapWindow";
            this.chkOutputMapWindow.Size = new System.Drawing.Size(132, 17);
            this.chkOutputMapWindow.TabIndex = 0;
            this.chkOutputMapWindow.Text = "Output to map window";
            this.chkOutputMapWindow.UseVisualStyleBackColor = true;
            this.chkOutputMapWindow.CheckedChanged += new System.EventHandler(this.chkOutputMapWindow_CheckedChanged);
            // 
            // fraMapWindowSettings
            // 
            this.fraMapWindowSettings.Controls.Add(this.spnZoom);
            this.fraMapWindowSettings.Controls.Add(this.lblZoom);
            this.fraMapWindowSettings.Controls.Add(this.txtZoom);
            this.fraMapWindowSettings.Controls.Add(this.optFitAndZoomOut);
            this.fraMapWindowSettings.Controls.Add(this.optCenterAtCurrentScale);
            this.fraMapWindowSettings.Controls.Add(this.optViewAtCurrentScale);
            this.fraMapWindowSettings.Location = new System.Drawing.Point(274, 40);
            this.fraMapWindowSettings.Name = "fraMapWindowSettings";
            this.fraMapWindowSettings.Size = new System.Drawing.Size(242, 99);
            this.fraMapWindowSettings.TabIndex = 6;
            this.fraMapWindowSettings.TabStop = false;
            this.fraMapWindowSettings.Text = "Map Window Settings";
            // 
            // spnZoom
            // 
            this.spnZoom.Location = new System.Drawing.Point(176, 72);
            this.spnZoom.Name = "spnZoom";
            this.spnZoom.Size = new System.Drawing.Size(35, 20);
            this.spnZoom.TabIndex = 10;
            this.spnZoom.Scroll += new System.Windows.Forms.ScrollEventHandler(this.spnZoom_Scroll);
            // 
            // lblZoom
            // 
            this.lblZoom.AutoSize = true;
            this.lblZoom.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblZoom.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblZoom.Location = new System.Drawing.Point(214, 75);
            this.lblZoom.Name = "lblZoom";
            this.lblZoom.Size = new System.Drawing.Size(18, 13);
            this.lblZoom.TabIndex = 9;
            this.lblZoom.Text = "%";
            this.lblZoom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtZoom
            // 
            this.txtZoom.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtZoom.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtZoom.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.txtZoom.Location = new System.Drawing.Point(134, 75);
            this.txtZoom.Name = "txtZoom";
            this.txtZoom.Size = new System.Drawing.Size(40, 14);
            this.txtZoom.TabIndex = 3;
            this.txtZoom.Text = "105";
            this.txtZoom.TextChanged += new System.EventHandler(this.txtZoom_TextChanged);
            this.txtZoom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtZoom_KeyPress);
            // 
            // optFitAndZoomOut
            // 
            this.optFitAndZoomOut.AutoSize = true;
            this.optFitAndZoomOut.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optFitAndZoomOut.Location = new System.Drawing.Point(6, 72);
            this.optFitAndZoomOut.Name = "optFitAndZoomOut";
            this.optFitAndZoomOut.Size = new System.Drawing.Size(105, 17);
            this.optFitAndZoomOut.TabIndex = 2;
            this.optFitAndZoomOut.Text = "Fit and zoom out";
            this.optFitAndZoomOut.UseVisualStyleBackColor = true;
            this.optFitAndZoomOut.CheckedChanged += new System.EventHandler(this.optFitandzoomout_CheckedChanged);
            // 
            // optCenterAtCurrentScale
            // 
            this.optCenterAtCurrentScale.AutoSize = true;
            this.optCenterAtCurrentScale.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optCenterAtCurrentScale.Location = new System.Drawing.Point(6, 45);
            this.optCenterAtCurrentScale.Name = "optCenterAtCurrentScale";
            this.optCenterAtCurrentScale.Size = new System.Drawing.Size(136, 17);
            this.optCenterAtCurrentScale.TabIndex = 1;
            this.optCenterAtCurrentScale.Text = "Center at current scale";
            this.optCenterAtCurrentScale.UseVisualStyleBackColor = true;
            this.optCenterAtCurrentScale.CheckedChanged += new System.EventHandler(this.optCentreatcurrentscale_CheckedChanged);
            // 
            // optViewAtCurrentScale
            // 
            this.optViewAtCurrentScale.AutoSize = true;
            this.optViewAtCurrentScale.Checked = true;
            this.optViewAtCurrentScale.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optViewAtCurrentScale.Location = new System.Drawing.Point(6, 19);
            this.optViewAtCurrentScale.Name = "optViewAtCurrentScale";
            this.optViewAtCurrentScale.Size = new System.Drawing.Size(125, 17);
            this.optViewAtCurrentScale.TabIndex = 0;
            this.optViewAtCurrentScale.TabStop = true;
            this.optViewAtCurrentScale.Text = "View at current scale";
            this.optViewAtCurrentScale.UseVisualStyleBackColor = true;
            this.optViewAtCurrentScale.CheckedChanged += new System.EventHandler(this.optViewatcurrentscale_CheckedChanged);
            // 
            // chkOverrideMapWindowSettings
            // 
            this.chkOverrideMapWindowSettings.AutoSize = true;
            this.chkOverrideMapWindowSettings.Checked = true;
            this.chkOverrideMapWindowSettings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOverrideMapWindowSettings.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkOverrideMapWindowSettings.Location = new System.Drawing.Point(280, 12);
            this.chkOverrideMapWindowSettings.Name = "chkOverrideMapWindowSettings";
            this.chkOverrideMapWindowSettings.Size = new System.Drawing.Size(174, 17);
            this.chkOverrideMapWindowSettings.TabIndex = 7;
            this.chkOverrideMapWindowSettings.Text = "Override Map Window Settings";
            this.chkOverrideMapWindowSettings.UseVisualStyleBackColor = true;
            this.chkOverrideMapWindowSettings.CheckedChanged += new System.EventHandler(this.chkOverrideMapWindowSettings_CheckedChanged);
            // 
            // chkNotifyChanges
            // 
            this.chkNotifyChanges.AutoSize = true;
            this.chkNotifyChanges.Checked = true;
            this.chkNotifyChanges.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNotifyChanges.Location = new System.Drawing.Point(280, 159);
            this.chkNotifyChanges.Name = "chkNotifyChanges";
            this.chkNotifyChanges.Size = new System.Drawing.Size(98, 17);
            this.chkNotifyChanges.TabIndex = 8;
            this.chkNotifyChanges.Text = "Notify Changes";
            this.chkNotifyChanges.UseVisualStyleBackColor = true;
            this.chkNotifyChanges.CheckedChanged += new System.EventHandler(this.chkNotifyChanges_CheckedChanged);
            // 
            // cmdApply
            // 
            this.cmdApply.Location = new System.Drawing.Point(360, 200);
            this.cmdApply.Name = "cmdApply";
            this.cmdApply.Size = new System.Drawing.Size(75, 23);
            this.cmdApply.TabIndex = 9;
            this.cmdApply.Text = "Apply";
            this.cmdApply.UseVisualStyleBackColor = true;
            this.cmdApply.Click += new System.EventHandler(this.cmdApply_Click);
            // 
            // cmdClose
            // 
            this.cmdClose.Location = new System.Drawing.Point(441, 200);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(75, 23);
            this.cmdClose.TabIndex = 10;
            this.cmdClose.Text = "Close";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // OptionsDT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(528, 228);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.cmdApply);
            this.Controls.Add(this.chkNotifyChanges);
            this.Controls.Add(this.fraMapWindowSettings);
            this.Controls.Add(this.chkOverrideMapWindowSettings);
            this.Controls.Add(this.grpMapOutput);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsDT";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.grpMapOutput.ResumeLayout(false);
            this.grpMapOutput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GMProblemColorButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GMFillColorButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GMTraceColorButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboLineweight)).EndInit();
            this.fraMapWindowSettings.ResumeLayout(false);
            this.fraMapWindowSettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpMapOutput;
        private System.Windows.Forms.CheckBox chkOverrideStyle;
        private System.Windows.Forms.RadioButton optAllWindows;
        private System.Windows.Forms.RadioButton optActiveWindow;
        private System.Windows.Forms.CheckBox chkOutputMapWindow;
        public AxSTYCTLLib.AxGMColorButton GMProblemColorButton;
        public AxSTYCTLLib.AxGMColorButton GMFillColorButton;
        public AxSTYCTLLib.AxGMColorButton GMTraceColorButton;
        public System.Windows.Forms.Label lblProblemColor;
        public System.Windows.Forms.Label lblFillcolor;
        public System.Windows.Forms.Label lblTraceColor;
        public AxSTYCTLLib.AxGMWeightCombobox cboLineweight;
        private System.Windows.Forms.Label lblLineWeight;
        public System.Windows.Forms.GroupBox fraMapWindowSettings;
        public System.Windows.Forms.HScrollBar spnZoom;
        public System.Windows.Forms.Label lblZoom;
        public System.Windows.Forms.TextBox txtZoom;
        public System.Windows.Forms.RadioButton optFitAndZoomOut;
        public System.Windows.Forms.RadioButton optCenterAtCurrentScale;
        public System.Windows.Forms.RadioButton optViewAtCurrentScale;
        public System.Windows.Forms.CheckBox chkOverrideMapWindowSettings;
        private System.Windows.Forms.CheckBox chkNotifyChanges;
        private System.Windows.Forms.Button cmdApply;
        private System.Windows.Forms.Button cmdClose;
    }
}