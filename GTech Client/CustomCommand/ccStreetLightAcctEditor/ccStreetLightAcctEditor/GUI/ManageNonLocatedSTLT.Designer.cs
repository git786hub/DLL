namespace GTechnology.Oncor.CustomAPI.GUI
{
    partial class ManageNonLocatedSTLT
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
            this.txtStreetLightAcct = new System.Windows.Forms.TextBox();
            this.dtGridViewNonLocated = new System.Windows.Forms.DataGridView();
            this.btnEditStreetLight = new System.Windows.Forms.Button();
            this.btnStructure = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.StltIdentifier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ConnectionStatus = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.DisconnectDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ConnectDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Location = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AdditionalLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dtGridViewNonLocated)).BeginInit();
            this.SuspendLayout();
            // 
            // txtStreetLightAcct
            // 
            this.txtStreetLightAcct.HideSelection = false;
            this.txtStreetLightAcct.Location = new System.Drawing.Point(4, 2);
            this.txtStreetLightAcct.Name = "txtStreetLightAcct";
            this.txtStreetLightAcct.ReadOnly = true;
            this.txtStreetLightAcct.Size = new System.Drawing.Size(821, 20);
            this.txtStreetLightAcct.TabIndex = 99;
            this.txtStreetLightAcct.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // dtGridViewNonLocated
            // 
            this.dtGridViewNonLocated.AllowUserToAddRows = false;
            this.dtGridViewNonLocated.AllowUserToDeleteRows = false;
            this.dtGridViewNonLocated.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtGridViewNonLocated.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.StltIdentifier,
            this.ConnectionStatus,
            this.DisconnectDate,
            this.ConnectDate,
            this.Location,
            this.AdditionalLocation});
            this.dtGridViewNonLocated.Location = new System.Drawing.Point(4, 28);
            this.dtGridViewNonLocated.Name = "dtGridViewNonLocated";
            this.dtGridViewNonLocated.ReadOnly = true;
            this.dtGridViewNonLocated.Size = new System.Drawing.Size(821, 344);
            this.dtGridViewNonLocated.TabIndex = 1;
            // 
            // btnEditStreetLight
            // 
            this.btnEditStreetLight.Enabled = false;
            this.btnEditStreetLight.Location = new System.Drawing.Point(442, 378);
            this.btnEditStreetLight.Name = "btnEditStreetLight";
            this.btnEditStreetLight.Size = new System.Drawing.Size(120, 30);
            this.btnEditStreetLight.TabIndex = 2;
            this.btnEditStreetLight.Text = "Edit Street Light";
            this.btnEditStreetLight.UseVisualStyleBackColor = true;
            this.btnEditStreetLight.Click += new System.EventHandler(this.btnEditStreetLight_Click);
            // 
            // btnStructure
            // 
            this.btnStructure.Location = new System.Drawing.Point(568, 378);
            this.btnStructure.Name = "btnStructure";
            this.btnStructure.Size = new System.Drawing.Size(120, 30);
            this.btnStructure.TabIndex = 3;
            this.btnStructure.Text = "Place Structure";
            this.btnStructure.UseVisualStyleBackColor = true;
            this.btnStructure.Click += new System.EventHandler(this.btnStructure_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(694, 376);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(57, 30);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(757, 376);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(57, 30);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // StltIdentifier
            // 
            this.StltIdentifier.HeaderText = "Street Light Identifier";
            this.StltIdentifier.Name = "StltIdentifier";
            this.StltIdentifier.ReadOnly = true;
            // 
            // ConnectionStatus
            // 
            this.ConnectionStatus.HeaderText = "Connection Status";
            this.ConnectionStatus.Name = "ConnectionStatus";
            this.ConnectionStatus.ReadOnly = true;
            this.ConnectionStatus.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ConnectionStatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // DisconnectDate
            // 
            this.DisconnectDate.HeaderText = "Disconnect Date";
            this.DisconnectDate.Name = "DisconnectDate";
            this.DisconnectDate.ReadOnly = true;
            // 
            // ConnectDate
            // 
            this.ConnectDate.HeaderText = "Connect Date";
            this.ConnectDate.Name = "ConnectDate";
            this.ConnectDate.ReadOnly = true;
            // 
            // Location
            // 
            this.Location.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Location.HeaderText = "Location";
            this.Location.Name = "Location";
            this.Location.ReadOnly = true;
            // 
            // AdditionalLocation
            // 
            this.AdditionalLocation.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.AdditionalLocation.HeaderText = "Additional Location";
            this.AdditionalLocation.Name = "AdditionalLocation";
            this.AdditionalLocation.ReadOnly = true;
            // 
            // ManageNonLocatedSTLT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(826, 418);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnStructure);
            this.Controls.Add(this.btnEditStreetLight);
            this.Controls.Add(this.dtGridViewNonLocated);
            this.Controls.Add(this.txtStreetLightAcct);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ManageNonLocatedSTLT";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Manage Non-Located Street Lights";
            this.Shown += new System.EventHandler(this.ManageNonLocatedSTLT_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dtGridViewNonLocated)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtStreetLightAcct;
        private System.Windows.Forms.DataGridView dtGridViewNonLocated;
        private System.Windows.Forms.Button btnEditStreetLight;
        private System.Windows.Forms.Button btnStructure;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridViewTextBoxColumn LocationSTLT;
        private System.Windows.Forms.DataGridViewTextBoxColumn StltIdentifier;
        private System.Windows.Forms.DataGridViewComboBoxColumn ConnectionStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn DisconnectDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ConnectDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Location;
        private System.Windows.Forms.DataGridViewTextBoxColumn AdditionalLocation;
    }
}