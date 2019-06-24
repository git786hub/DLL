namespace GTechnology.Oncor.CustomAPI
{
    partial class ManualLandbaseReviewFrm
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
            this.dgvManualLandbaseReview = new System.Windows.Forms.DataGridView();
            this.cmbFeatureList = new System.Windows.Forms.ComboBox();
            this.btnAccepted = new System.Windows.Forms.Button();
            this.btnArchive = new System.Windows.Forms.Button();
            this.chckPendingFilter = new System.Windows.Forms.CheckBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvManualLandbaseReview)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvManualLandbaseReview
            // 
            this.dgvManualLandbaseReview.Location = new System.Drawing.Point(12, 58);
            this.dgvManualLandbaseReview.Name = "dgvManualLandbaseReview";
            this.dgvManualLandbaseReview.ReadOnly = true;
            this.dgvManualLandbaseReview.Size = new System.Drawing.Size(557, 301);
            this.dgvManualLandbaseReview.TabIndex = 0;
            this.dgvManualLandbaseReview.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvManualLandbaseReview_ColumnHeaderMouseClick);
            this.dgvManualLandbaseReview.SelectionChanged += new System.EventHandler(this.dgvManualLandbaseReview_SelectionChanged);
            // 
            // cmbFeatureList
            // 
            this.cmbFeatureList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbFeatureList.FormattingEnabled = true;
            this.cmbFeatureList.Items.AddRange(new object[] {
            "Building - Manual",
            "Parcel - Manual",
            "Pipeline Manual",
            "Street Centerline - Manual"});
            this.cmbFeatureList.Location = new System.Drawing.Point(136, 21);
            this.cmbFeatureList.Name = "cmbFeatureList";
            this.cmbFeatureList.Size = new System.Drawing.Size(192, 21);
            this.cmbFeatureList.TabIndex = 1;
            this.cmbFeatureList.SelectedValueChanged += new System.EventHandler(this.cmbFeatureList_SelectedValueChanged);
            // 
            // btnAccepted
            // 
            this.btnAccepted.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAccepted.Location = new System.Drawing.Point(12, 377);
            this.btnAccepted.Name = "btnAccepted";
            this.btnAccepted.Size = new System.Drawing.Size(281, 32);
            this.btnAccepted.TabIndex = 2;
            this.btnAccepted.Text = "Accept";
            this.btnAccepted.UseVisualStyleBackColor = true;
            this.btnAccepted.Click += new System.EventHandler(this.btnAccepted_Click);
            // 
            // btnArchive
            // 
            this.btnArchive.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnArchive.Location = new System.Drawing.Point(299, 377);
            this.btnArchive.Name = "btnArchive";
            this.btnArchive.Size = new System.Drawing.Size(270, 32);
            this.btnArchive.TabIndex = 3;
            this.btnArchive.Text = "Archive";
            this.btnArchive.UseVisualStyleBackColor = true;
            this.btnArchive.Click += new System.EventHandler(this.btnArchive_Click);
            // 
            // chckPendingFilter
            // 
            this.chckPendingFilter.AutoSize = true;
            this.chckPendingFilter.Location = new System.Drawing.Point(12, 428);
            this.chckPendingFilter.Name = "chckPendingFilter";
            this.chckPendingFilter.Size = new System.Drawing.Size(210, 17);
            this.chckPendingFilter.TabIndex = 4;
            this.chckPendingFilter.Text = "Filter to display only PENDING features";
            this.chckPendingFilter.UseVisualStyleBackColor = true;
            this.chckPendingFilter.CheckedChanged += new System.EventHandler(this.chckPendingFilter_CheckedChanged);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(399, 422);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 5;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(494, 422);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 17);
            this.label1.TabIndex = 7;
            this.label1.Text = "Feature Class:";
            // 
            // ManualLandbaseReviewFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 457);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.chckPendingFilter);
            this.Controls.Add(this.btnArchive);
            this.Controls.Add(this.btnAccepted);
            this.Controls.Add(this.cmbFeatureList);
            this.Controls.Add(this.dgvManualLandbaseReview);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ManualLandbaseReviewFrm";
            this.Text = "Manual Landbase Review";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ManualLandbaseReviewFrm_FormClosed);
            this.Load += new System.EventHandler(this.ManualLandbaseReviewFrm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ManualLandbaseReviewFrm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvManualLandbaseReview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvManualLandbaseReview;
        private System.Windows.Forms.ComboBox cmbFeatureList;
        private System.Windows.Forms.Button btnAccepted;
        private System.Windows.Forms.Button btnArchive;
        private System.Windows.Forms.CheckBox chckPendingFilter;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label1;
    }
}