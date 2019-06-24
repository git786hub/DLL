<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PrintActiveMapWindowForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
    Me.components = New System.ComponentModel.Container()
    Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
    Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
    Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
    Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PrintActiveMapWindowForm))
    Me.GroupBox2 = New System.Windows.Forms.GroupBox()
    Me.lblSetMapSizeValue = New System.Windows.Forms.Label()
    Me.lblSetMapScaleValue = New System.Windows.Forms.Label()
    Me.lblSetBorderInsetValue = New System.Windows.Forms.Label()
    Me.lblSetPaperValue = New System.Windows.Forms.Label()
    Me.lblSetMapSize = New System.Windows.Forms.Label()
    Me.lblSetMapScale = New System.Windows.Forms.Label()
    Me.lblBorderInset = New System.Windows.Forms.Label()
    Me.lblSetPaper = New System.Windows.Forms.Label()
    Me.imgPageContainer = New System.Windows.Forms.PictureBox()
    Me.gbPaper = New System.Windows.Forms.GroupBox()
    Me.gbPaperOrientation = New System.Windows.Forms.GroupBox()
    Me.optPaperOrientationLandscape = New System.Windows.Forms.RadioButton()
    Me.optPaperOrientationPortrait = New System.Windows.Forms.RadioButton()
    Me.lbPaperSize = New System.Windows.Forms.ListBox()
    Me.Label1 = New System.Windows.Forms.Label()
    Me.gbDrawingInfo = New System.Windows.Forms.GroupBox()
    Me.dgvAttributes = New System.Windows.Forms.DataGridView()
    Me.ColumnAttributeName = New System.Windows.Forms.DataGridViewTextBoxColumn()
    Me.ColumnAttributeValue = New System.Windows.Forms.DataGridViewTextBoxColumn()
    Me.cmdPrint = New System.Windows.Forms.Button()
    Me.cmdCancel = New System.Windows.Forms.Button()
    Me.gbLegend = New System.Windows.Forms.GroupBox()
    Me.lbLegend = New System.Windows.Forms.ListBox()
    Me.gbActiveSessionInfo = New System.Windows.Forms.GroupBox()
    Me.lblDateValue = New System.Windows.Forms.Label()
    Me.lblActiveSessionInfoUserValue = New System.Windows.Forms.Label()
    Me.lblDate = New System.Windows.Forms.Label()
    Me.lblActiveSessionInfoUser = New System.Windows.Forms.Label()
    Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
    Me.PrintDialog1 = New System.Windows.Forms.PrintDialog()
    Me.chkBoxMapBackgroundToWhite = New System.Windows.Forms.CheckBox()
    Me.GroupBox2.SuspendLayout()
    CType(Me.imgPageContainer, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.gbPaper.SuspendLayout()
    Me.gbPaperOrientation.SuspendLayout()
    Me.gbDrawingInfo.SuspendLayout()
    CType(Me.dgvAttributes, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.gbLegend.SuspendLayout()
    Me.gbActiveSessionInfo.SuspendLayout()
    Me.SuspendLayout()
    '
    'GroupBox2
    '
    Me.GroupBox2.Controls.Add(Me.lblSetMapSizeValue)
    Me.GroupBox2.Controls.Add(Me.lblSetMapScaleValue)
    Me.GroupBox2.Controls.Add(Me.lblSetBorderInsetValue)
    Me.GroupBox2.Controls.Add(Me.lblSetPaperValue)
    Me.GroupBox2.Controls.Add(Me.lblSetMapSize)
    Me.GroupBox2.Controls.Add(Me.lblSetMapScale)
    Me.GroupBox2.Controls.Add(Me.lblBorderInset)
    Me.GroupBox2.Controls.Add(Me.lblSetPaper)
    Me.GroupBox2.Controls.Add(Me.imgPageContainer)
    Me.GroupBox2.Location = New System.Drawing.Point(188, 12)
    Me.GroupBox2.Name = "GroupBox2"
    Me.GroupBox2.Size = New System.Drawing.Size(389, 332)
    Me.GroupBox2.TabIndex = 24
    Me.GroupBox2.TabStop = False
    '
    'lblSetMapSizeValue
    '
    Me.lblSetMapSizeValue.AutoSize = True
    Me.lblSetMapSizeValue.Location = New System.Drawing.Point(162, 303)
    Me.lblSetMapSizeValue.Name = "lblSetMapSizeValue"
    Me.lblSetMapSizeValue.Size = New System.Drawing.Size(76, 13)
    Me.lblSetMapSizeValue.TabIndex = 8
    Me.lblSetMapSizeValue.Text = "61.4m x 39.1m"
    '
    'lblSetMapScaleValue
    '
    Me.lblSetMapScaleValue.AutoSize = True
    Me.lblSetMapScaleValue.Location = New System.Drawing.Point(162, 290)
    Me.lblSetMapScaleValue.Name = "lblSetMapScaleValue"
    Me.lblSetMapScaleValue.Size = New System.Drawing.Size(34, 13)
    Me.lblSetMapScaleValue.TabIndex = 7
    Me.lblSetMapScaleValue.Text = "1:250"
    '
    'lblSetBorderInsetValue
    '
    Me.lblSetBorderInsetValue.AutoSize = True
    Me.lblSetBorderInsetValue.Location = New System.Drawing.Point(162, 277)
    Me.lblSetBorderInsetValue.Name = "lblSetBorderInsetValue"
    Me.lblSetBorderInsetValue.Size = New System.Drawing.Size(35, 13)
    Me.lblSetBorderInsetValue.TabIndex = 6
    Me.lblSetBorderInsetValue.Text = "17mm"
    '
    'lblSetPaperValue
    '
    Me.lblSetPaperValue.AutoSize = True
    Me.lblSetPaperValue.Location = New System.Drawing.Point(162, 264)
    Me.lblSetPaperValue.Name = "lblSetPaperValue"
    Me.lblSetPaperValue.Size = New System.Drawing.Size(107, 13)
    Me.lblSetPaperValue.TabIndex = 5
    Me.lblSetPaperValue.Text = "8.5 x 11 (Landscape)"
    '
    'lblSetMapSize
    '
    Me.lblSetMapSize.AutoSize = True
    Me.lblSetMapSize.Location = New System.Drawing.Point(6, 303)
    Me.lblSetMapSize.Name = "lblSetMapSize"
    Me.lblSetMapSize.Size = New System.Drawing.Size(51, 13)
    Me.lblSetMapSize.TabIndex = 4
    Me.lblSetMapSize.Text = "Map Size"
    '
    'lblSetMapScale
    '
    Me.lblSetMapScale.AutoSize = True
    Me.lblSetMapScale.Location = New System.Drawing.Point(6, 290)
    Me.lblSetMapScale.Name = "lblSetMapScale"
    Me.lblSetMapScale.Size = New System.Drawing.Size(58, 13)
    Me.lblSetMapScale.TabIndex = 3
    Me.lblSetMapScale.Text = "Map Scale"
    '
    'lblBorderInset
    '
    Me.lblBorderInset.AutoSize = True
    Me.lblBorderInset.Location = New System.Drawing.Point(6, 277)
    Me.lblBorderInset.Name = "lblBorderInset"
    Me.lblBorderInset.Size = New System.Drawing.Size(67, 13)
    Me.lblBorderInset.TabIndex = 2
    Me.lblBorderInset.Text = "Border Inset:"
    '
    'lblSetPaper
    '
    Me.lblSetPaper.AutoSize = True
    Me.lblSetPaper.Location = New System.Drawing.Point(6, 264)
    Me.lblSetPaper.Name = "lblSetPaper"
    Me.lblSetPaper.Size = New System.Drawing.Size(38, 13)
    Me.lblSetPaper.TabIndex = 1
    Me.lblSetPaper.Text = "Paper:"
    '
    'imgPageContainer
    '
    Me.imgPageContainer.BackColor = System.Drawing.SystemColors.Highlight
    Me.imgPageContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
    Me.imgPageContainer.Location = New System.Drawing.Point(9, 16)
    Me.imgPageContainer.Name = "imgPageContainer"
    Me.imgPageContainer.Size = New System.Drawing.Size(372, 245)
    Me.imgPageContainer.TabIndex = 0
    Me.imgPageContainer.TabStop = False
    '
    'gbPaper
    '
    Me.gbPaper.Controls.Add(Me.gbPaperOrientation)
    Me.gbPaper.Controls.Add(Me.lbPaperSize)
    Me.gbPaper.Controls.Add(Me.Label1)
    Me.gbPaper.Location = New System.Drawing.Point(12, 12)
    Me.gbPaper.Name = "gbPaper"
    Me.gbPaper.Size = New System.Drawing.Size(170, 212)
    Me.gbPaper.TabIndex = 23
    Me.gbPaper.TabStop = False
    Me.gbPaper.Text = "Paper:"
    '
    'gbPaperOrientation
    '
    Me.gbPaperOrientation.Controls.Add(Me.optPaperOrientationLandscape)
    Me.gbPaperOrientation.Controls.Add(Me.optPaperOrientationPortrait)
    Me.gbPaperOrientation.Location = New System.Drawing.Point(10, 138)
    Me.gbPaperOrientation.Name = "gbPaperOrientation"
    Me.gbPaperOrientation.Size = New System.Drawing.Size(148, 60)
    Me.gbPaperOrientation.TabIndex = 2
    Me.gbPaperOrientation.TabStop = False
    Me.gbPaperOrientation.Text = "Orientation:"
    '
    'optPaperOrientationLandscape
    '
    Me.optPaperOrientationLandscape.AutoSize = True
    Me.optPaperOrientationLandscape.Checked = True
    Me.optPaperOrientationLandscape.Location = New System.Drawing.Point(7, 37)
    Me.optPaperOrientationLandscape.Name = "optPaperOrientationLandscape"
    Me.optPaperOrientationLandscape.Size = New System.Drawing.Size(78, 17)
    Me.optPaperOrientationLandscape.TabIndex = 0
    Me.optPaperOrientationLandscape.TabStop = True
    Me.optPaperOrientationLandscape.Text = "Landscape"
    Me.optPaperOrientationLandscape.UseVisualStyleBackColor = True
    '
    'optPaperOrientationPortrait
    '
    Me.optPaperOrientationPortrait.AutoSize = True
    Me.optPaperOrientationPortrait.Location = New System.Drawing.Point(7, 20)
    Me.optPaperOrientationPortrait.Name = "optPaperOrientationPortrait"
    Me.optPaperOrientationPortrait.Size = New System.Drawing.Size(58, 17)
    Me.optPaperOrientationPortrait.TabIndex = 0
    Me.optPaperOrientationPortrait.Text = "Portrait"
    Me.optPaperOrientationPortrait.UseVisualStyleBackColor = True
    '
    'lbPaperSize
    '
    Me.lbPaperSize.FormattingEnabled = True
    Me.lbPaperSize.Items.AddRange(New Object() {"A-Sizeee (8.5 x 11)", "B-Size (11 x 17)", "C-Size (17 x 22)", "D-Size (22 x 34)", "E-Size (34 x 44)", "F-Size (8.5 x 30)"})
    Me.lbPaperSize.Location = New System.Drawing.Point(10, 37)
    Me.lbPaperSize.Name = "lbPaperSize"
    Me.lbPaperSize.Size = New System.Drawing.Size(148, 95)
    Me.lbPaperSize.TabIndex = 1
    '
    'Label1
    '
    Me.Label1.AutoSize = True
    Me.Label1.Location = New System.Drawing.Point(7, 20)
    Me.Label1.Name = "Label1"
    Me.Label1.Size = New System.Drawing.Size(30, 13)
    Me.Label1.TabIndex = 0
    Me.Label1.Text = "Size:"
    '
    'gbDrawingInfo
    '
    Me.gbDrawingInfo.Controls.Add(Me.dgvAttributes)
    Me.gbDrawingInfo.Location = New System.Drawing.Point(12, 350)
    Me.gbDrawingInfo.Name = "gbDrawingInfo"
    Me.gbDrawingInfo.Size = New System.Drawing.Size(565, 105)
    Me.gbDrawingInfo.TabIndex = 22
    Me.gbDrawingInfo.TabStop = False
    Me.gbDrawingInfo.Text = "Plot Fields"
    '
    'dgvAttributes
    '
    Me.dgvAttributes.AllowUserToAddRows = False
    Me.dgvAttributes.AllowUserToDeleteRows = False
    Me.dgvAttributes.AllowUserToResizeRows = False
    Me.dgvAttributes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
    DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
    DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
    DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
    DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
    DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
    DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
    Me.dgvAttributes.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
    Me.dgvAttributes.ColumnHeadersHeight = 20
    Me.dgvAttributes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
    Me.dgvAttributes.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ColumnAttributeName, Me.ColumnAttributeValue})
    DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
    DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window
    DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText
    DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
    DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
    DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
    Me.dgvAttributes.DefaultCellStyle = DataGridViewCellStyle3
    Me.dgvAttributes.Dock = System.Windows.Forms.DockStyle.Fill
    Me.dgvAttributes.Location = New System.Drawing.Point(3, 16)
    Me.dgvAttributes.MultiSelect = False
    Me.dgvAttributes.Name = "dgvAttributes"
    DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
    DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
    DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
    DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
    DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
    DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
    Me.dgvAttributes.RowHeadersDefaultCellStyle = DataGridViewCellStyle4
    Me.dgvAttributes.RowHeadersVisible = False
    Me.dgvAttributes.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders
    Me.dgvAttributes.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
    Me.dgvAttributes.Size = New System.Drawing.Size(559, 86)
    Me.dgvAttributes.TabIndex = 1
    '
    'ColumnAttributeName
    '
    Me.ColumnAttributeName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
    DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ButtonFace
    Me.ColumnAttributeName.DefaultCellStyle = DataGridViewCellStyle2
    Me.ColumnAttributeName.HeaderText = "Name"
    Me.ColumnAttributeName.Name = "ColumnAttributeName"
    Me.ColumnAttributeName.ReadOnly = True
    Me.ColumnAttributeName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
    '
    'ColumnAttributeValue
    '
    Me.ColumnAttributeValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
    Me.ColumnAttributeValue.HeaderText = "Value"
    Me.ColumnAttributeValue.Name = "ColumnAttributeValue"
    Me.ColumnAttributeValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
    '
    'cmdPrint
    '
    Me.cmdPrint.Location = New System.Drawing.Point(418, 461)
    Me.cmdPrint.Name = "cmdPrint"
    Me.cmdPrint.Size = New System.Drawing.Size(75, 23)
    Me.cmdPrint.TabIndex = 26
    Me.cmdPrint.Text = "Preview"
    Me.cmdPrint.UseVisualStyleBackColor = True
    '
    'cmdCancel
    '
    Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.cmdCancel.Location = New System.Drawing.Point(499, 461)
    Me.cmdCancel.Name = "cmdCancel"
    Me.cmdCancel.Size = New System.Drawing.Size(75, 23)
    Me.cmdCancel.TabIndex = 25
    Me.cmdCancel.Text = "Cancel"
    Me.cmdCancel.UseVisualStyleBackColor = True
    '
    'gbLegend
    '
    Me.gbLegend.Controls.Add(Me.lbLegend)
    Me.gbLegend.Location = New System.Drawing.Point(12, 230)
    Me.gbLegend.Name = "gbLegend"
    Me.gbLegend.Size = New System.Drawing.Size(170, 114)
    Me.gbLegend.TabIndex = 24
    Me.gbLegend.TabStop = False
    Me.gbLegend.Text = "Legend:"
    '
    'lbLegend
    '
    Me.lbLegend.FormattingEnabled = True
    Me.lbLegend.Items.AddRange(New Object() {"Electric Design Legend", "Overview Legend", "Electric-Black and White Legend", "Electric Operations Legend", "Color on White Legend", "Black and White Legend"})
    Me.lbLegend.Location = New System.Drawing.Point(10, 19)
    Me.lbLegend.Name = "lbLegend"
    Me.lbLegend.Size = New System.Drawing.Size(148, 82)
    Me.lbLegend.TabIndex = 1
    '
    'gbActiveSessionInfo
    '
    Me.gbActiveSessionInfo.Controls.Add(Me.lblDateValue)
    Me.gbActiveSessionInfo.Controls.Add(Me.lblActiveSessionInfoUserValue)
    Me.gbActiveSessionInfo.Controls.Add(Me.lblDate)
    Me.gbActiveSessionInfo.Controls.Add(Me.lblActiveSessionInfoUser)
    Me.gbActiveSessionInfo.Location = New System.Drawing.Point(12, 461)
    Me.gbActiveSessionInfo.Name = "gbActiveSessionInfo"
    Me.gbActiveSessionInfo.Size = New System.Drawing.Size(170, 102)
    Me.gbActiveSessionInfo.TabIndex = 27
    Me.gbActiveSessionInfo.TabStop = False
    Me.gbActiveSessionInfo.Text = "Active Session Information:"
    Me.gbActiveSessionInfo.Visible = False
    '
    'lblDateValue
    '
    Me.lblDateValue.AutoSize = True
    Me.lblDateValue.ForeColor = System.Drawing.SystemColors.Highlight
    Me.lblDateValue.Location = New System.Drawing.Point(44, 40)
    Me.lblDateValue.Name = "lblDateValue"
    Me.lblDateValue.Size = New System.Drawing.Size(57, 13)
    Me.lblDateValue.TabIndex = 7
    Me.lblDateValue.Text = "DateValue"
    '
    'lblActiveSessionInfoUserValue
    '
    Me.lblActiveSessionInfoUserValue.AutoSize = True
    Me.lblActiveSessionInfoUserValue.ForeColor = System.Drawing.SystemColors.Highlight
    Me.lblActiveSessionInfoUserValue.Location = New System.Drawing.Point(44, 22)
    Me.lblActiveSessionInfoUserValue.Name = "lblActiveSessionInfoUserValue"
    Me.lblActiveSessionInfoUserValue.Size = New System.Drawing.Size(56, 13)
    Me.lblActiveSessionInfoUserValue.TabIndex = 6
    Me.lblActiveSessionInfoUserValue.Text = "UserValue"
    '
    'lblDate
    '
    Me.lblDate.AutoSize = True
    Me.lblDate.Location = New System.Drawing.Point(6, 41)
    Me.lblDate.Name = "lblDate"
    Me.lblDate.Size = New System.Drawing.Size(33, 13)
    Me.lblDate.TabIndex = 1
    Me.lblDate.Text = "Date:"
    '
    'lblActiveSessionInfoUser
    '
    Me.lblActiveSessionInfoUser.AutoSize = True
    Me.lblActiveSessionInfoUser.Location = New System.Drawing.Point(6, 22)
    Me.lblActiveSessionInfoUser.Name = "lblActiveSessionInfoUser"
    Me.lblActiveSessionInfoUser.Size = New System.Drawing.Size(32, 13)
    Me.lblActiveSessionInfoUser.TabIndex = 0
    Me.lblActiveSessionInfoUser.Text = "User:"
    '
    'ImageList1
    '
    Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
    Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
    Me.ImageList1.Images.SetKeyName(0, "Map.jpg")
    '
    'PrintDialog1
    '
    Me.PrintDialog1.AllowPrintToFile = False
    Me.PrintDialog1.UseEXDialog = True
    '
    'chkBoxMapBackgroundToWhite
    '
    Me.chkBoxMapBackgroundToWhite.AutoSize = True
    Me.chkBoxMapBackgroundToWhite.Location = New System.Drawing.Point(197, 467)
    Me.chkBoxMapBackgroundToWhite.Name = "chkBoxMapBackgroundToWhite"
    Me.chkBoxMapBackgroundToWhite.Size = New System.Drawing.Size(139, 17)
    Me.chkBoxMapBackgroundToWhite.TabIndex = 28
    Me.chkBoxMapBackgroundToWhite.Text = "Map Background White"
    Me.chkBoxMapBackgroundToWhite.UseVisualStyleBackColor = True
    '
    'PrintActiveMapWindowForm
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.CancelButton = Me.cmdCancel
    Me.ClientSize = New System.Drawing.Size(589, 493)
    Me.Controls.Add(Me.chkBoxMapBackgroundToWhite)
    Me.Controls.Add(Me.gbActiveSessionInfo)
    Me.Controls.Add(Me.gbLegend)
    Me.Controls.Add(Me.cmdPrint)
    Me.Controls.Add(Me.cmdCancel)
    Me.Controls.Add(Me.GroupBox2)
    Me.Controls.Add(Me.gbPaper)
    Me.Controls.Add(Me.gbDrawingInfo)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
    Me.KeyPreview = True
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "PrintActiveMapWindowForm"
    Me.ShowIcon = False
    Me.ShowInTaskbar = False
    Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
    Me.Text = "Print Active Map Window"
    Me.GroupBox2.ResumeLayout(False)
    Me.GroupBox2.PerformLayout()
    CType(Me.imgPageContainer, System.ComponentModel.ISupportInitialize).EndInit()
    Me.gbPaper.ResumeLayout(False)
    Me.gbPaper.PerformLayout()
    Me.gbPaperOrientation.ResumeLayout(False)
    Me.gbPaperOrientation.PerformLayout()
    Me.gbDrawingInfo.ResumeLayout(False)
    CType(Me.dgvAttributes, System.ComponentModel.ISupportInitialize).EndInit()
    Me.gbLegend.ResumeLayout(False)
    Me.gbActiveSessionInfo.ResumeLayout(False)
    Me.gbActiveSessionInfo.PerformLayout()
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
  Friend WithEvents lblSetMapSizeValue As System.Windows.Forms.Label
  Friend WithEvents lblSetMapScaleValue As System.Windows.Forms.Label
  Friend WithEvents lblSetBorderInsetValue As System.Windows.Forms.Label
  Friend WithEvents lblSetPaperValue As System.Windows.Forms.Label
  Friend WithEvents lblSetMapSize As System.Windows.Forms.Label
  Friend WithEvents lblSetMapScale As System.Windows.Forms.Label
  Friend WithEvents lblBorderInset As System.Windows.Forms.Label
  Friend WithEvents lblSetPaper As System.Windows.Forms.Label
  Friend WithEvents imgPageContainer As System.Windows.Forms.PictureBox
  Friend WithEvents gbPaper As System.Windows.Forms.GroupBox
  Friend WithEvents gbPaperOrientation As System.Windows.Forms.GroupBox
  Friend WithEvents optPaperOrientationLandscape As System.Windows.Forms.RadioButton
  Friend WithEvents optPaperOrientationPortrait As System.Windows.Forms.RadioButton
  Friend WithEvents lbPaperSize As System.Windows.Forms.ListBox
  Friend WithEvents Label1 As System.Windows.Forms.Label
  Friend WithEvents gbDrawingInfo As System.Windows.Forms.GroupBox
  Friend WithEvents dgvAttributes As System.Windows.Forms.DataGridView
  Friend WithEvents ColumnAttributeName As System.Windows.Forms.DataGridViewTextBoxColumn
  Friend WithEvents ColumnAttributeValue As System.Windows.Forms.DataGridViewTextBoxColumn
  Friend WithEvents cmdPrint As System.Windows.Forms.Button
  Friend WithEvents cmdCancel As System.Windows.Forms.Button
  Friend WithEvents gbLegend As System.Windows.Forms.GroupBox
  Friend WithEvents lbLegend As System.Windows.Forms.ListBox
  Friend WithEvents gbActiveSessionInfo As System.Windows.Forms.GroupBox
  Friend WithEvents lblDateValue As System.Windows.Forms.Label
  Friend WithEvents lblActiveSessionInfoUserValue As System.Windows.Forms.Label
  Friend WithEvents lblDate As System.Windows.Forms.Label
  Friend WithEvents lblActiveSessionInfoUser As System.Windows.Forms.Label
  Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
  Friend WithEvents PrintDialog1 As System.Windows.Forms.PrintDialog
  Friend WithEvents chkBoxMapBackgroundToWhite As CheckBox
End Class
