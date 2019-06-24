namespace GTechnology.Oncor.CustomAPI
{
    partial class ESIAccountSelection
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel = new System.Windows.Forms.Panel();
            this.lblstatusMsg = new System.Windows.Forms.Label();
            this.lblMsg1 = new System.Windows.Forms.Label();
            this.gridGrpbox = new System.Windows.Forms.GroupBox();
            this.txtGridHeader = new System.Windows.Forms.TextBox();
            this.dtGridViewFilter = new System.Windows.Forms.DataGridView();
            this.txt_ESILocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txt_Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txt_RateSchedule = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txt_Wattage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txt_LampType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txt_LuminareStyle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkRemoveGeofilter = new System.Windows.Forms.CheckBox();
            this.dtGridViewStreetAcct = new System.Windows.Forms.DataGridView();
            this.ESI_LOCATION = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RATE_SCHEDULE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WATTAGE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LAMP_TYPE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Luminare_Style = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblMsg = new System.Windows.Forms.Label();
            this.panel.SuspendLayout();
            this.gridGrpbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtGridViewFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtGridViewStreetAcct)).BeginInit();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.Controls.Add(this.lblstatusMsg);
            this.panel.Controls.Add(this.lblMsg1);
            this.panel.Controls.Add(this.gridGrpbox);
            this.panel.Controls.Add(this.lblMsg);
            this.panel.Location = new System.Drawing.Point(2, -4);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(660, 527);
            this.panel.TabIndex = 0;
            // 
            // lblstatusMsg
            // 
            this.lblstatusMsg.AutoSize = true;
            this.lblstatusMsg.ForeColor = System.Drawing.Color.Red;
            this.lblstatusMsg.Location = new System.Drawing.Point(10, 71);
            this.lblstatusMsg.Name = "lblstatusMsg";
            this.lblstatusMsg.Size = new System.Drawing.Size(0, 13);
            this.lblstatusMsg.TabIndex = 5;
            // 
            // lblMsg1
            // 
            this.lblMsg1.AutoSize = true;
            this.lblMsg1.Location = new System.Drawing.Point(3, 13);
            this.lblMsg1.Name = "lblMsg1";
            this.lblMsg1.Size = new System.Drawing.Size(610, 26);
            this.lblMsg1.TabIndex = 4;
            this.lblMsg1.Text = "The list of ESI Locations has been filtered only by Street Light Accounts matchin" +
    "g the Lamp Type,Wattage, and Luminaire  style \r\nand geographic location of the S" +
    "teet Light";
            // 
            // gridGrpbox
            // 
            this.gridGrpbox.Controls.Add(this.txtGridHeader);
            this.gridGrpbox.Controls.Add(this.dtGridViewFilter);
            this.gridGrpbox.Controls.Add(this.btnOK);
            this.gridGrpbox.Controls.Add(this.btnCancel);
            this.gridGrpbox.Controls.Add(this.chkRemoveGeofilter);
            this.gridGrpbox.Controls.Add(this.dtGridViewStreetAcct);
            this.gridGrpbox.Location = new System.Drawing.Point(3, 101);
            this.gridGrpbox.Name = "gridGrpbox";
            this.gridGrpbox.Size = new System.Drawing.Size(657, 426);
            this.gridGrpbox.TabIndex = 2;
            this.gridGrpbox.TabStop = false;
            // 
            // txtGridHeader
            // 
            this.txtGridHeader.BackColor = System.Drawing.Color.Silver;
            this.txtGridHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGridHeader.Location = new System.Drawing.Point(5, 9);
            this.txtGridHeader.Name = "txtGridHeader";
            this.txtGridHeader.ReadOnly = true;
            this.txtGridHeader.Size = new System.Drawing.Size(649, 20);
            this.txtGridHeader.TabIndex = 12;
            this.txtGridHeader.Text = "Filters";
            // 
            // dtGridViewFilter
            // 
            this.dtGridViewFilter.AllowUserToAddRows = false;
            this.dtGridViewFilter.AllowUserToDeleteRows = false;
            this.dtGridViewFilter.AllowUserToResizeColumns = false;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtGridViewFilter.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dtGridViewFilter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtGridViewFilter.ColumnHeadersVisible = false;
            this.dtGridViewFilter.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.txt_ESILocation,
            this.txt_Description,
            this.txt_RateSchedule,
            this.txt_Wattage,
            this.txt_LampType,
            this.txt_LuminareStyle});
            this.dtGridViewFilter.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dtGridViewFilter.Location = new System.Drawing.Point(5, 29);
            this.dtGridViewFilter.Name = "dtGridViewFilter";
            this.dtGridViewFilter.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dtGridViewFilter.RowHeadersWidth = 30;
            this.dtGridViewFilter.RowTemplate.Height = 30;
            this.dtGridViewFilter.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.dtGridViewFilter.Size = new System.Drawing.Size(649, 31);
            this.dtGridViewFilter.TabIndex = 11;
            // 
            // txt_ESILocation
            // 
            dataGridViewCellStyle9.NullValue = "\"\"";
            this.txt_ESILocation.DefaultCellStyle = dataGridViewCellStyle9;
            this.txt_ESILocation.Frozen = true;
            this.txt_ESILocation.HeaderText = "ESI Location";
            this.txt_ESILocation.Name = "txt_ESILocation";
            this.txt_ESILocation.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.txt_ESILocation.Width = 75;
            // 
            // txt_Description
            // 
            dataGridViewCellStyle10.NullValue = "\"\"";
            this.txt_Description.DefaultCellStyle = dataGridViewCellStyle10;
            this.txt_Description.Frozen = true;
            this.txt_Description.HeaderText = "Description";
            this.txt_Description.Name = "txt_Description";
            this.txt_Description.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.txt_Description.Width = 250;
            // 
            // txt_RateSchedule
            // 
            dataGridViewCellStyle11.NullValue = "\"\"";
            this.txt_RateSchedule.DefaultCellStyle = dataGridViewCellStyle11;
            this.txt_RateSchedule.Frozen = true;
            this.txt_RateSchedule.HeaderText = "Rate Schedule";
            this.txt_RateSchedule.Name = "txt_RateSchedule";
            this.txt_RateSchedule.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.txt_RateSchedule.Width = 75;
            // 
            // txt_Wattage
            // 
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.ControlDark;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.Color.Gray;
            this.txt_Wattage.DefaultCellStyle = dataGridViewCellStyle12;
            this.txt_Wattage.Frozen = true;
            this.txt_Wattage.HeaderText = "Wattage";
            this.txt_Wattage.Name = "txt_Wattage";
            this.txt_Wattage.ReadOnly = true;
            this.txt_Wattage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.txt_Wattage.Width = 50;
            // 
            // txt_LampType
            // 
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.ControlDark;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.Color.Gray;
            this.txt_LampType.DefaultCellStyle = dataGridViewCellStyle13;
            this.txt_LampType.Frozen = true;
            this.txt_LampType.HeaderText = "Lamp Type";
            this.txt_LampType.Name = "txt_LampType";
            this.txt_LampType.ReadOnly = true;
            this.txt_LampType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.txt_LampType.Width = 75;
            // 
            // txt_LuminareStyle
            // 
            dataGridViewCellStyle14.BackColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.ControlDark;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.Color.Gray;
            this.txt_LuminareStyle.DefaultCellStyle = dataGridViewCellStyle14;
            this.txt_LuminareStyle.Frozen = true;
            this.txt_LuminareStyle.HeaderText = "Luminaire Style";
            this.txt_LuminareStyle.Name = "txt_LuminareStyle";
            this.txt_LuminareStyle.ReadOnly = true;
            this.txt_LuminareStyle.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.txt_LuminareStyle.Width = 90;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(550, 393);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(460, 393);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chkRemoveGeofilter
            // 
            this.chkRemoveGeofilter.AutoSize = true;
            this.chkRemoveGeofilter.Location = new System.Drawing.Point(6, 393);
            this.chkRemoveGeofilter.Name = "chkRemoveGeofilter";
            this.chkRemoveGeofilter.Size = new System.Drawing.Size(163, 17);
            this.chkRemoveGeofilter.TabIndex = 8;
            this.chkRemoveGeofilter.Text = "Remove Geographic Filtering";
            this.chkRemoveGeofilter.UseVisualStyleBackColor = true;
            // 
            // dtGridViewStreetAcct
            // 
            this.dtGridViewStreetAcct.AllowUserToAddRows = false;
            this.dtGridViewStreetAcct.AllowUserToDeleteRows = false;
            this.dtGridViewStreetAcct.AllowUserToResizeColumns = false;
            this.dtGridViewStreetAcct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtGridViewStreetAcct.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ESI_LOCATION,
            this.Description,
            this.RATE_SCHEDULE,
            this.WATTAGE,
            this.LAMP_TYPE,
            this.Luminare_Style});
            this.dtGridViewStreetAcct.Location = new System.Drawing.Point(5, 60);
            this.dtGridViewStreetAcct.MultiSelect = false;
            this.dtGridViewStreetAcct.Name = "dtGridViewStreetAcct";
            this.dtGridViewStreetAcct.ReadOnly = true;
            this.dtGridViewStreetAcct.RowHeadersWidth = 30;
            this.dtGridViewStreetAcct.Size = new System.Drawing.Size(649, 322);
            this.dtGridViewStreetAcct.TabIndex = 0;
            this.dtGridViewStreetAcct.SelectionChanged += new System.EventHandler(this.dtGridViewStreetAcct_SelectionChanged);
            // 
            // ESI_LOCATION
            // 
            this.ESI_LOCATION.HeaderText = "ESI Location";
            this.ESI_LOCATION.Name = "ESI_LOCATION";
            this.ESI_LOCATION.ReadOnly = true;
            this.ESI_LOCATION.Width = 75;
            // 
            // Description
            // 
            this.Description.HeaderText = "Description";
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            this.Description.Width = 250;
            // 
            // RATE_SCHEDULE
            // 
            this.RATE_SCHEDULE.HeaderText = "Rate Schedule";
            this.RATE_SCHEDULE.Name = "RATE_SCHEDULE";
            this.RATE_SCHEDULE.ReadOnly = true;
            this.RATE_SCHEDULE.Width = 75;
            // 
            // WATTAGE
            // 
            this.WATTAGE.HeaderText = "Wattage";
            this.WATTAGE.Name = "WATTAGE";
            this.WATTAGE.ReadOnly = true;
            this.WATTAGE.Width = 50;
            // 
            // LAMP_TYPE
            // 
            this.LAMP_TYPE.HeaderText = "Lamp Type";
            this.LAMP_TYPE.Name = "LAMP_TYPE";
            this.LAMP_TYPE.ReadOnly = true;
            this.LAMP_TYPE.Width = 75;
            // 
            // Luminare_Style
            // 
            this.Luminare_Style.HeaderText = "Luminaire Style";
            this.Luminare_Style.Name = "Luminare_Style";
            this.Luminare_Style.ReadOnly = true;
            this.Luminare_Style.Width = 90;
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Location = new System.Drawing.Point(3, 48);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(248, 13);
            this.lblMsg.TabIndex = 0;
            this.lblMsg.Text = "Select an ESI Location to assign to the Street Light";
            // 
            // ESIAccountSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(661, 522);
            this.Controls.Add(this.panel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ESIAccountSelection";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Street Light ESI Location Selection";
            this.Load += new System.EventHandler(this.ESIAccountSelection_Load);
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.gridGrpbox.ResumeLayout(false);
            this.gridGrpbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtGridViewFilter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtGridViewStreetAcct)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.GroupBox gridGrpbox;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkRemoveGeofilter;
        private System.Windows.Forms.DataGridView dtGridViewFilter;
        private System.Windows.Forms.Label lblMsg1;
        private System.Windows.Forms.DataGridView dtGridViewStreetAcct;
        private System.Windows.Forms.TextBox txtGridHeader;
        private System.Windows.Forms.Label lblstatusMsg;
        private System.Windows.Forms.DataGridViewTextBoxColumn txt_ESILocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn txt_Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn txt_RateSchedule;
        private System.Windows.Forms.DataGridViewTextBoxColumn txt_Wattage;
        private System.Windows.Forms.DataGridViewTextBoxColumn txt_LampType;
        private System.Windows.Forms.DataGridViewTextBoxColumn txt_LuminareStyle;
        private System.Windows.Forms.DataGridViewTextBoxColumn ESI_LOCATION;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn RATE_SCHEDULE;
        private System.Windows.Forms.DataGridViewTextBoxColumn WATTAGE;
        private System.Windows.Forms.DataGridViewTextBoxColumn LAMP_TYPE;
        private System.Windows.Forms.DataGridViewTextBoxColumn Luminare_Style;

    }
}