// VBConversions Note: VB project level imports
using System.Collections.Generic;
using Intergraph.GTechnology;
using System;
using Intergraph.GTechnology.API;
using System.Diagnostics;
//using Microsoft.VisualBasic;
using System.Collections;
// End of VB project level imports


namespace GTechnology.Oncor.CustomAPI
{
	//[global::Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]public 
	partial class dlgAncCUSelection : System.Windows.Forms.Form
	{
		
		//Form overrides dispose to clean up the component list.
		[System.Diagnostics.DebuggerNonUserCode()]protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}
		
		//Required by the Windows Form Designer
		private System.ComponentModel.Container components = null;
		
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent()
		{
            this.Panel1 = new System.Windows.Forms.Panel();
            this.cmbAncillaryCategories = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grdFilter = new System.Windows.Forms.DataGridView();
            this.Panel2 = new System.Windows.Forms.Panel();
            this.btnMakePreferred = new System.Windows.Forms.Button();
            this.btnMoreInfo = new System.Windows.Forms.Button();
            this.chkShowPreferredCU = new System.Windows.Forms.CheckBox();
            this.OK_Button = new System.Windows.Forms.Button();
            this.Cancel_Button = new System.Windows.Forms.Button();
            this.SplitContainer1 = new System.Windows.Forms.SplitContainer();
            this.grdCU = new System.Windows.Forms.DataGridView();
            this.grdSelected = new System.Windows.Forms.DataGridView();
            this.Panel3 = new System.Windows.Forms.Panel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdFilter)).BeginInit();
            this.Panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer1)).BeginInit();
            this.SplitContainer1.Panel1.SuspendLayout();
            this.SplitContainer1.Panel2.SuspendLayout();
            this.SplitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCU)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSelected)).BeginInit();
            this.Panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // Panel1
            // 
            this.Panel1.Controls.Add(this.cmbAncillaryCategories);
            this.Panel1.Controls.Add(this.label1);
            this.Panel1.Controls.Add(this.grdFilter);
            this.Panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.Panel1.Location = new System.Drawing.Point(0, 0);
            this.Panel1.Margin = new System.Windows.Forms.Padding(4);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(673, 133);
            this.Panel1.TabIndex = 1;
            // 
            // cmbAncillaryCategories
            // 
            this.cmbAncillaryCategories.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAncillaryCategories.FormattingEnabled = true;
            this.cmbAncillaryCategories.Location = new System.Drawing.Point(196, 7);
            this.cmbAncillaryCategories.Margin = new System.Windows.Forms.Padding(4);
            this.cmbAncillaryCategories.Name = "cmbAncillaryCategories";
            this.cmbAncillaryCategories.Size = new System.Drawing.Size(241, 24);
            this.cmbAncillaryCategories.TabIndex = 2;
            this.cmbAncillaryCategories.SelectedIndexChanged += new System.EventHandler(this.cmbAncillaryCategories_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select Ancillary Category:";
            // 
            // grdFilter
            // 
            this.grdFilter.AllowUserToAddRows = false;
            this.grdFilter.AllowUserToDeleteRows = false;
            this.grdFilter.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.grdFilter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grdFilter.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grdFilter.Location = new System.Drawing.Point(0, 39);
            this.grdFilter.Margin = new System.Windows.Forms.Padding(4);
            this.grdFilter.Name = "grdFilter";
            this.grdFilter.RowHeadersWidth = 70;
            this.grdFilter.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.grdFilter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.grdFilter.Size = new System.Drawing.Size(673, 94);
            this.grdFilter.TabIndex = 0;
            this.grdFilter.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.grdFilter_ColumnWidthChanged);
            this.grdFilter.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.grdFilter_EditingControlShowing);
            // 
            // Panel2
            // 
            this.Panel2.Controls.Add(this.btnMakePreferred);
            this.Panel2.Controls.Add(this.btnMoreInfo);
            this.Panel2.Controls.Add(this.chkShowPreferredCU);
            this.Panel2.Controls.Add(this.OK_Button);
            this.Panel2.Controls.Add(this.Cancel_Button);
            this.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Panel2.Location = new System.Drawing.Point(0, 518);
            this.Panel2.Margin = new System.Windows.Forms.Padding(4);
            this.Panel2.Name = "Panel2";
            this.Panel2.Size = new System.Drawing.Size(673, 59);
            this.Panel2.TabIndex = 2;
            // 
            // btnMakePreferred
            // 
            this.btnMakePreferred.AutoSize = true;
            this.btnMakePreferred.Location = new System.Drawing.Point(236, 13);
            this.btnMakePreferred.Name = "btnMakePreferred";
            this.btnMakePreferred.Size = new System.Drawing.Size(122, 28);
            this.btnMakePreferred.TabIndex = 6;
            this.btnMakePreferred.Text = "Make Preferred";
            this.btnMakePreferred.UseVisualStyleBackColor = true;
            this.btnMakePreferred.Click += new System.EventHandler(this.btnMakePreferred_Click);
            // 
            // btnMoreInfo
            // 
            this.btnMoreInfo.AutoSize = true;
            this.btnMoreInfo.Location = new System.Drawing.Point(375, 13);
            this.btnMoreInfo.Name = "btnMoreInfo";
            this.btnMoreInfo.Size = new System.Drawing.Size(91, 28);
            this.btnMoreInfo.TabIndex = 5;
            this.btnMoreInfo.Text = "More Info...";
            this.btnMoreInfo.UseVisualStyleBackColor = true;
            this.btnMoreInfo.Click += new System.EventHandler(this.btnMoreInfo_Click);
            // 
            // chkShowPreferredCU
            // 
            this.chkShowPreferredCU.AutoSize = true;
            this.chkShowPreferredCU.Location = new System.Drawing.Point(13, 18);
            this.chkShowPreferredCU.Name = "chkShowPreferredCU";
            this.chkShowPreferredCU.Size = new System.Drawing.Size(187, 21);
            this.chkShowPreferredCU.TabIndex = 4;
            this.chkShowPreferredCU.Text = "Show preferred CUs only";
            this.chkShowPreferredCU.UseVisualStyleBackColor = true;
            this.chkShowPreferredCU.CheckedChanged += new System.EventHandler(this.chkShowPreferredCU_CheckedChanged);
            // 
            // OK_Button
            // 
            this.OK_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.OK_Button.AutoSize = true;
            this.OK_Button.Location = new System.Drawing.Point(483, 13);
            this.OK_Button.Margin = new System.Windows.Forms.Padding(4);
            this.OK_Button.Name = "OK_Button";
            this.OK_Button.Size = new System.Drawing.Size(89, 28);
            this.OK_Button.TabIndex = 3;
            this.OK_Button.Text = "OK";
            this.OK_Button.Click += new System.EventHandler(this.OK_Button_Click);
            // 
            // Cancel_Button
            // 
            this.Cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel_Button.AutoSize = true;
            this.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel_Button.Location = new System.Drawing.Point(579, 13);
            this.Cancel_Button.Margin = new System.Windows.Forms.Padding(4);
            this.Cancel_Button.Name = "Cancel_Button";
            this.Cancel_Button.Size = new System.Drawing.Size(89, 28);
            this.Cancel_Button.TabIndex = 2;
            this.Cancel_Button.Text = "Cancel";
            this.Cancel_Button.Click += new System.EventHandler(this.Cancel_Button_Click);
            // 
            // SplitContainer1
            // 
            this.SplitContainer1.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.SplitContainer1.Cursor = System.Windows.Forms.Cursors.SizeNS;
            this.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer1.Location = new System.Drawing.Point(0, 133);
            this.SplitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.SplitContainer1.Name = "SplitContainer1";
            this.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplitContainer1.Panel1
            // 
            this.SplitContainer1.Panel1.Controls.Add(this.grdCU);
            // 
            // SplitContainer1.Panel2
            // 
            this.SplitContainer1.Panel2.Controls.Add(this.grdSelected);
            this.SplitContainer1.Panel2.Controls.Add(this.Panel3);
            this.SplitContainer1.Size = new System.Drawing.Size(673, 385);
            this.SplitContainer1.SplitterDistance = 182;
            this.SplitContainer1.SplitterWidth = 7;
            this.SplitContainer1.TabIndex = 3;
            // 
            // grdCU
            // 
            this.grdCU.AllowUserToAddRows = false;
            this.grdCU.AllowUserToDeleteRows = false;
            this.grdCU.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.grdCU.Cursor = System.Windows.Forms.Cursors.Default;
            this.grdCU.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdCU.Location = new System.Drawing.Point(0, 0);
            this.grdCU.Margin = new System.Windows.Forms.Padding(4);
            this.grdCU.MultiSelect = false;
            this.grdCU.Name = "grdCU";
            this.grdCU.ReadOnly = true;
            this.grdCU.RowHeadersWidth = 70;
            this.grdCU.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdCU.Size = new System.Drawing.Size(673, 182);
            this.grdCU.TabIndex = 0;
            this.grdCU.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.grdCU_ColumnWidthChanged);
            this.grdCU.Click += new System.EventHandler(this.grdCU_Click);
            this.grdCU.DoubleClick += new System.EventHandler(this.grdCU_DoubleClick);
            // 
            // grdSelected
            // 
            this.grdSelected.AllowUserToAddRows = false;
            this.grdSelected.AllowUserToDeleteRows = false;
            this.grdSelected.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdSelected.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.grdSelected.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdSelected.Cursor = System.Windows.Forms.Cursors.Default;
            this.grdSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdSelected.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.grdSelected.Location = new System.Drawing.Point(0, 41);
            this.grdSelected.Margin = new System.Windows.Forms.Padding(4);
            this.grdSelected.Name = "grdSelected";
            this.grdSelected.RowHeadersWidth = 60;
            this.grdSelected.Size = new System.Drawing.Size(673, 155);
            this.grdSelected.TabIndex = 1;
            this.grdSelected.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.grdSelected_CellBeginEdit);
            this.grdSelected.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdSelected_CellEndEdit);
            this.grdSelected.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.grdSelected_DataBindingComplete);
            this.grdSelected.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.grdSelected_DataError);
            this.grdSelected.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.grdSelected_RowsAdded);
            this.grdSelected.SelectionChanged += new System.EventHandler(this.grdSelected_SelectionChanged);
            this.grdSelected.DoubleClick += new System.EventHandler(this.grdSelected_DoubleClick);
            // 
            // Panel3
            // 
            this.Panel3.BackColor = System.Drawing.SystemColors.Control;
            this.Panel3.Controls.Add(this.btnAdd);
            this.Panel3.Controls.Add(this.btnRemove);
            this.Panel3.Cursor = System.Windows.Forms.Cursors.Default;
            this.Panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.Panel3.Location = new System.Drawing.Point(0, 0);
            this.Panel3.Margin = new System.Windows.Forms.Padding(4);
            this.Panel3.Name = "Panel3";
            this.Panel3.Size = new System.Drawing.Size(673, 41);
            this.Panel3.TabIndex = 0;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAdd.Location = new System.Drawing.Point(256, 6);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(89, 28);
            this.btnAdd.TabIndex = 7;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnRemove.Location = new System.Drawing.Point(353, 6);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(4);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(89, 28);
            this.btnRemove.TabIndex = 6;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // dlgAncCUSelection
            // 
            this.AcceptButton = this.OK_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(673, 577);
            this.Controls.Add(this.SplitContainer1);
            this.Controls.Add(this.Panel2);
            this.Controls.Add(this.Panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(691, 624);
            this.Name = "dlgAncCUSelection";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Ancillary CU Selection";
            this.ResizeBegin += new System.EventHandler(this.dlgAncCUSelection_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.dlgAncCUSelection_ResizeEnd);
            this.Panel1.ResumeLayout(false);
            this.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdFilter)).EndInit();
            this.Panel2.ResumeLayout(false);
            this.Panel2.PerformLayout();
            this.SplitContainer1.Panel1.ResumeLayout(false);
            this.SplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer1)).EndInit();
            this.SplitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdCU)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSelected)).EndInit();
            this.Panel3.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		internal System.Windows.Forms.Button OK_Button;
		internal System.Windows.Forms.Button Cancel_Button;
		internal System.Windows.Forms.DataGridView grdFilter;
		internal System.Windows.Forms.Panel Panel1;
		internal System.Windows.Forms.Panel Panel2;
		internal System.Windows.Forms.SplitContainer SplitContainer1;
		internal System.Windows.Forms.DataGridView grdCU;
		internal System.Windows.Forms.Panel Panel3;
		internal System.Windows.Forms.Button btnAdd;
		internal System.Windows.Forms.Button btnRemove;
		internal System.Windows.Forms.DataGridView grdSelected;
        private System.Windows.Forms.ComboBox cmbAncillaryCategories;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnMakePreferred;
        private System.Windows.Forms.Button btnMoreInfo;
        private System.Windows.Forms.CheckBox chkShowPreferredCU;
    }
	
}
