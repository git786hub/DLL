// VBConversions Note: VB project level imports
using System.Collections.Generic;
using Intergraph.GTechnology;
using System;
using Intergraph.GTechnology.API;
using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Collections;
// End of VB project level imports


namespace GTechnology.Oncor.CustomAPI
{
	//[global::Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]public 
	partial class dlgPriCUSelection : System.Windows.Forms.Form
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grdFilter = new System.Windows.Forms.DataGridView();
            this.Panel2 = new System.Windows.Forms.Panel();
            this.btnMakePreferred = new System.Windows.Forms.Button();
            this.btnMoreInfor = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.OK_Button = new System.Windows.Forms.Button();
            this.Cancel_Button = new System.Windows.Forms.Button();
            this.Panel3 = new System.Windows.Forms.Panel();
            this.grdCU = new System.Windows.Forms.DataGridView();
            this.Panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.grdFilter)).BeginInit();
            this.Panel2.SuspendLayout();
            this.Panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCU)).BeginInit();
            this.Panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdFilter
            // 
            this.grdFilter.AllowUserToAddRows = false;
            this.grdFilter.AllowUserToDeleteRows = false;
            this.grdFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdFilter.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdFilter.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grdFilter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdFilter.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grdFilter.Location = new System.Drawing.Point(0, 0);
            this.grdFilter.Margin = new System.Windows.Forms.Padding(4);
            this.grdFilter.MultiSelect = false;
            this.grdFilter.Name = "grdFilter";
            this.grdFilter.RowHeadersWidth = 70;
            this.grdFilter.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.grdFilter.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.grdFilter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.grdFilter.Size = new System.Drawing.Size(764, 74);
            this.grdFilter.TabIndex = 0;
            this.grdFilter.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.grdFilter_ColumnWidthChanged);
            this.grdFilter.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.grdFilter_EditingControlShowing);
            // 
            // Panel2
            // 
            this.Panel2.Controls.Add(this.btnMakePreferred);
            this.Panel2.Controls.Add(this.btnMoreInfor);
            this.Panel2.Controls.Add(this.checkBox1);
            this.Panel2.Controls.Add(this.OK_Button);
            this.Panel2.Controls.Add(this.Cancel_Button);
            this.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Panel2.Location = new System.Drawing.Point(0, 593);
            this.Panel2.Margin = new System.Windows.Forms.Padding(4);
            this.Panel2.Name = "Panel2";
            this.Panel2.Size = new System.Drawing.Size(764, 52);
            this.Panel2.TabIndex = 2;
            // 
            // btnMakePreferred
            // 
            this.btnMakePreferred.AutoSize = true;
            this.btnMakePreferred.Location = new System.Drawing.Point(262, 10);
            this.btnMakePreferred.Name = "btnMakePreferred";
            this.btnMakePreferred.Size = new System.Drawing.Size(116, 27);
            this.btnMakePreferred.TabIndex = 6;
            this.btnMakePreferred.Text = "Make Preferred";
            this.btnMakePreferred.UseVisualStyleBackColor = true;
            this.btnMakePreferred.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnMoreInfor
            // 
            this.btnMoreInfor.AutoSize = true;
            this.btnMoreInfor.Location = new System.Drawing.Point(416, 9);
            this.btnMoreInfor.Name = "btnMoreInfor";
            this.btnMoreInfor.Size = new System.Drawing.Size(89, 27);
            this.btnMoreInfor.TabIndex = 5;
            this.btnMoreInfor.Text = "More Info...";
            this.btnMoreInfor.UseVisualStyleBackColor = true;
            this.btnMoreInfor.Click += new System.EventHandler(this.btnMoreInfo);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(43, 13);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(187, 21);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "Show preferred CUs only";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // OK_Button
            // 
            this.OK_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.OK_Button.AutoSize = true;
            this.OK_Button.Location = new System.Drawing.Point(565, 9);
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
            this.Cancel_Button.Location = new System.Drawing.Point(662, 8);
            this.Cancel_Button.Margin = new System.Windows.Forms.Padding(4);
            this.Cancel_Button.Name = "Cancel_Button";
            this.Cancel_Button.Size = new System.Drawing.Size(89, 28);
            this.Cancel_Button.TabIndex = 2;
            this.Cancel_Button.Text = "Cancel";
            this.Cancel_Button.Click += new System.EventHandler(this.Cancel_Button_Click);
            // 
            // Panel3
            // 
            this.Panel3.Controls.Add(this.grdCU);
            this.Panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel3.Location = new System.Drawing.Point(0, 74);
            this.Panel3.Margin = new System.Windows.Forms.Padding(4);
            this.Panel3.Name = "Panel3";
            this.Panel3.Size = new System.Drawing.Size(764, 519);
            this.Panel3.TabIndex = 3;
            // 
            // grdCU
            // 
            this.grdCU.AllowUserToAddRows = false;
            this.grdCU.AllowUserToDeleteRows = false;
            this.grdCU.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdCU.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.grdCU.Location = new System.Drawing.Point(0, 0);
            this.grdCU.Margin = new System.Windows.Forms.Padding(4);
            this.grdCU.MinimumSize = new System.Drawing.Size(764, 512);
            this.grdCU.MultiSelect = false;
            this.grdCU.Name = "grdCU";
            this.grdCU.ReadOnly = true;
            this.grdCU.RowHeadersWidth = 70;
            this.grdCU.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.grdCU.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdCU.Size = new System.Drawing.Size(764, 519);
            this.grdCU.TabIndex = 0;
            this.grdCU.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.grdCU_ColumnWidthChanged);
            this.grdCU.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.grdCU_DataError);
            this.grdCU.SelectionChanged += new System.EventHandler(this.grdCU_SelectionChanged);
            this.grdCU.DoubleClick += new System.EventHandler(this.grdCU_DoubleClick);
            // 
            // Panel1
            // 
            this.Panel1.Controls.Add(this.grdFilter);
            this.Panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.Panel1.Location = new System.Drawing.Point(0, 0);
            this.Panel1.Margin = new System.Windows.Forms.Padding(4);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(764, 74);
            this.Panel1.TabIndex = 1;
            // 
            // dlgPriCUSelection
            // 
            this.AcceptButton = this.OK_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(764, 645);
            this.Controls.Add(this.Panel3);
            this.Controls.Add(this.Panel2);
            this.Controls.Add(this.Panel1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(782, 692);
            this.Name = "dlgPriCUSelection";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Primary CU Selection";
            this.ResizeBegin += new System.EventHandler(this.dlgPriCUSelection_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.dlgPriCUSelection_ResizeEnd);
            ((System.ComponentModel.ISupportInitialize)(this.grdFilter)).EndInit();
            this.Panel2.ResumeLayout(false);
            this.Panel2.PerformLayout();
            this.Panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdCU)).EndInit();
            this.Panel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		internal System.Windows.Forms.Panel Panel2;
		internal System.Windows.Forms.Button OK_Button;
		internal System.Windows.Forms.Button Cancel_Button;
		internal System.Windows.Forms.Panel Panel3;
		internal System.Windows.Forms.DataGridView grdCU;
		internal System.Windows.Forms.DataGridView grdFilter;
        private System.Windows.Forms.Button btnMakePreferred;
        private System.Windows.Forms.Button btnMoreInfor;
        private System.Windows.Forms.CheckBox checkBox1;
        internal System.Windows.Forms.Panel Panel1;
    }
	
}
