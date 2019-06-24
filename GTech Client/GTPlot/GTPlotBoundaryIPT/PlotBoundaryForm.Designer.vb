<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PlotBoundaryForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
    Me.components = New System.ComponentModel.Container()
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PlotBoundaryForm))
    Me.cmdCancel = New System.Windows.Forms.Button()
    Me.cmdPlace = New System.Windows.Forms.Button()
    Me.GroupBox1 = New System.Windows.Forms.GroupBox()
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
    Me.lblPaperSize = New System.Windows.Forms.Label()
    Me.gbMapScale = New System.Windows.Forms.GroupBox()
    Me.lbMapScalePreDefined = New System.Windows.Forms.ListBox()
    Me.tbMapScaleCustom = New System.Windows.Forms.TextBox()
    Me.optMapScaleCustom = New System.Windows.Forms.RadioButton()
    Me.optMapScalePreDefined = New System.Windows.Forms.RadioButton()
    Me.gbType = New System.Windows.Forms.GroupBox()
    Me.cbType = New System.Windows.Forms.CheckBox()
    Me.lbType = New System.Windows.Forms.ListBox()
    Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
    Me.cmdADHOC = New System.Windows.Forms.Button()
    Me.GroupBox1.SuspendLayout()
    CType(Me.imgPageContainer, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.gbPaper.SuspendLayout()
    Me.gbPaperOrientation.SuspendLayout()
    Me.gbMapScale.SuspendLayout()
    Me.gbType.SuspendLayout()
    Me.SuspendLayout()
    '
    'cmdCancel
    '
    Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
    resources.ApplyResources(Me.cmdCancel, "cmdCancel")
    Me.cmdCancel.Name = "cmdCancel"
    Me.cmdCancel.UseVisualStyleBackColor = True
    '
    'cmdPlace
    '
    resources.ApplyResources(Me.cmdPlace, "cmdPlace")
    Me.cmdPlace.Name = "cmdPlace"
    Me.cmdPlace.UseVisualStyleBackColor = True
    '
    'GroupBox1
    '
    Me.GroupBox1.Controls.Add(Me.lblSetMapSizeValue)
    Me.GroupBox1.Controls.Add(Me.lblSetMapScaleValue)
    Me.GroupBox1.Controls.Add(Me.lblSetBorderInsetValue)
    Me.GroupBox1.Controls.Add(Me.lblSetPaperValue)
    Me.GroupBox1.Controls.Add(Me.lblSetMapSize)
    Me.GroupBox1.Controls.Add(Me.lblSetMapScale)
    Me.GroupBox1.Controls.Add(Me.lblBorderInset)
    Me.GroupBox1.Controls.Add(Me.lblSetPaper)
    Me.GroupBox1.Controls.Add(Me.imgPageContainer)
    resources.ApplyResources(Me.GroupBox1, "GroupBox1")
    Me.GroupBox1.Name = "GroupBox1"
    Me.GroupBox1.TabStop = False
    '
    'lblSetMapSizeValue
    '
    resources.ApplyResources(Me.lblSetMapSizeValue, "lblSetMapSizeValue")
    Me.lblSetMapSizeValue.Name = "lblSetMapSizeValue"
    '
    'lblSetMapScaleValue
    '
    resources.ApplyResources(Me.lblSetMapScaleValue, "lblSetMapScaleValue")
    Me.lblSetMapScaleValue.Name = "lblSetMapScaleValue"
    '
    'lblSetBorderInsetValue
    '
    resources.ApplyResources(Me.lblSetBorderInsetValue, "lblSetBorderInsetValue")
    Me.lblSetBorderInsetValue.Name = "lblSetBorderInsetValue"
    '
    'lblSetPaperValue
    '
    resources.ApplyResources(Me.lblSetPaperValue, "lblSetPaperValue")
    Me.lblSetPaperValue.Name = "lblSetPaperValue"
    '
    'lblSetMapSize
    '
    resources.ApplyResources(Me.lblSetMapSize, "lblSetMapSize")
    Me.lblSetMapSize.Name = "lblSetMapSize"
    '
    'lblSetMapScale
    '
    resources.ApplyResources(Me.lblSetMapScale, "lblSetMapScale")
    Me.lblSetMapScale.Name = "lblSetMapScale"
    '
    'lblBorderInset
    '
    resources.ApplyResources(Me.lblBorderInset, "lblBorderInset")
    Me.lblBorderInset.Name = "lblBorderInset"
    '
    'lblSetPaper
    '
    resources.ApplyResources(Me.lblSetPaper, "lblSetPaper")
    Me.lblSetPaper.Name = "lblSetPaper"
    '
    'imgPageContainer
    '
    Me.imgPageContainer.BackColor = System.Drawing.SystemColors.Highlight
    Me.imgPageContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
    resources.ApplyResources(Me.imgPageContainer, "imgPageContainer")
    Me.imgPageContainer.Name = "imgPageContainer"
    Me.imgPageContainer.TabStop = False
    '
    'gbPaper
    '
    Me.gbPaper.Controls.Add(Me.gbPaperOrientation)
    Me.gbPaper.Controls.Add(Me.lbPaperSize)
    Me.gbPaper.Controls.Add(Me.lblPaperSize)
    resources.ApplyResources(Me.gbPaper, "gbPaper")
    Me.gbPaper.Name = "gbPaper"
    Me.gbPaper.TabStop = False
    '
    'gbPaperOrientation
    '
    Me.gbPaperOrientation.Controls.Add(Me.optPaperOrientationLandscape)
    Me.gbPaperOrientation.Controls.Add(Me.optPaperOrientationPortrait)
    resources.ApplyResources(Me.gbPaperOrientation, "gbPaperOrientation")
    Me.gbPaperOrientation.Name = "gbPaperOrientation"
    Me.gbPaperOrientation.TabStop = False
    '
    'optPaperOrientationLandscape
    '
    resources.ApplyResources(Me.optPaperOrientationLandscape, "optPaperOrientationLandscape")
    Me.optPaperOrientationLandscape.Checked = True
    Me.optPaperOrientationLandscape.Name = "optPaperOrientationLandscape"
    Me.optPaperOrientationLandscape.TabStop = True
    Me.optPaperOrientationLandscape.UseVisualStyleBackColor = True
    '
    'optPaperOrientationPortrait
    '
    resources.ApplyResources(Me.optPaperOrientationPortrait, "optPaperOrientationPortrait")
    Me.optPaperOrientationPortrait.Name = "optPaperOrientationPortrait"
    Me.optPaperOrientationPortrait.UseVisualStyleBackColor = True
    '
    'lbPaperSize
    '
    Me.lbPaperSize.FormattingEnabled = True
    Me.lbPaperSize.Items.AddRange(New Object() {resources.GetString("lbPaperSize.Items"), resources.GetString("lbPaperSize.Items1"), resources.GetString("lbPaperSize.Items2"), resources.GetString("lbPaperSize.Items3"), resources.GetString("lbPaperSize.Items4"), resources.GetString("lbPaperSize.Items5"), resources.GetString("lbPaperSize.Items6")})
    resources.ApplyResources(Me.lbPaperSize, "lbPaperSize")
    Me.lbPaperSize.Name = "lbPaperSize"
    '
    'lblPaperSize
    '
    resources.ApplyResources(Me.lblPaperSize, "lblPaperSize")
    Me.lblPaperSize.Name = "lblPaperSize"
    '
    'gbMapScale
    '
    Me.gbMapScale.Controls.Add(Me.lbMapScalePreDefined)
    Me.gbMapScale.Controls.Add(Me.tbMapScaleCustom)
    Me.gbMapScale.Controls.Add(Me.optMapScaleCustom)
    Me.gbMapScale.Controls.Add(Me.optMapScalePreDefined)
    resources.ApplyResources(Me.gbMapScale, "gbMapScale")
    Me.gbMapScale.Name = "gbMapScale"
    Me.gbMapScale.TabStop = False
    '
    'lbMapScalePreDefined
    '
    Me.lbMapScalePreDefined.FormattingEnabled = True
    Me.lbMapScalePreDefined.Items.AddRange(New Object() {resources.GetString("lbMapScalePreDefined.Items"), resources.GetString("lbMapScalePreDefined.Items1"), resources.GetString("lbMapScalePreDefined.Items2")})
    resources.ApplyResources(Me.lbMapScalePreDefined, "lbMapScalePreDefined")
    Me.lbMapScalePreDefined.Name = "lbMapScalePreDefined"
    '
    'tbMapScaleCustom
    '
    resources.ApplyResources(Me.tbMapScaleCustom, "tbMapScaleCustom")
    Me.tbMapScaleCustom.Name = "tbMapScaleCustom"
    '
    'optMapScaleCustom
    '
    resources.ApplyResources(Me.optMapScaleCustom, "optMapScaleCustom")
    Me.optMapScaleCustom.Name = "optMapScaleCustom"
    Me.optMapScaleCustom.UseVisualStyleBackColor = True
    '
    'optMapScalePreDefined
    '
    resources.ApplyResources(Me.optMapScalePreDefined, "optMapScalePreDefined")
    Me.optMapScalePreDefined.Checked = True
    Me.optMapScalePreDefined.Name = "optMapScalePreDefined"
    Me.optMapScalePreDefined.TabStop = True
    Me.optMapScalePreDefined.UseVisualStyleBackColor = True
    '
    'gbType
    '
    Me.gbType.Controls.Add(Me.cbType)
    Me.gbType.Controls.Add(Me.lbType)
    resources.ApplyResources(Me.gbType, "gbType")
    Me.gbType.Name = "gbType"
    Me.gbType.TabStop = False
    '
    'cbType
    '
    resources.ApplyResources(Me.cbType, "cbType")
    Me.cbType.Checked = True
    Me.cbType.CheckState = System.Windows.Forms.CheckState.Checked
    Me.cbType.Name = "cbType"
    Me.cbType.UseVisualStyleBackColor = True
    '
    'lbType
    '
    Me.lbType.FormattingEnabled = True
    Me.lbType.Items.AddRange(New Object() {resources.GetString("lbType.Items"), resources.GetString("lbType.Items1"), resources.GetString("lbType.Items2"), resources.GetString("lbType.Items3"), resources.GetString("lbType.Items4")})
    resources.ApplyResources(Me.lbType, "lbType")
    Me.lbType.Name = "lbType"
    '
    'ImageList1
    '
    Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
    Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
    Me.ImageList1.Images.SetKeyName(0, "Map.jpg")
    '
    'cmdADHOC
    '
    resources.ApplyResources(Me.cmdADHOC, "cmdADHOC")
    Me.cmdADHOC.Name = "cmdADHOC"
    Me.cmdADHOC.UseVisualStyleBackColor = True
    '
    'PlotBoundaryForm
    '
    Me.AcceptButton = Me.cmdPlace
    resources.ApplyResources(Me, "$this")
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.CancelButton = Me.cmdCancel
    Me.Controls.Add(Me.gbType)
    Me.Controls.Add(Me.gbMapScale)
    Me.Controls.Add(Me.gbPaper)
    Me.Controls.Add(Me.GroupBox1)
    Me.Controls.Add(Me.cmdADHOC)
    Me.Controls.Add(Me.cmdPlace)
    Me.Controls.Add(Me.cmdCancel)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
    Me.KeyPreview = True
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "PlotBoundaryForm"
    Me.ShowIcon = False
    Me.ShowInTaskbar = False
    Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
    Me.GroupBox1.ResumeLayout(False)
    Me.GroupBox1.PerformLayout()
    CType(Me.imgPageContainer, System.ComponentModel.ISupportInitialize).EndInit()
    Me.gbPaper.ResumeLayout(False)
    Me.gbPaper.PerformLayout()
    Me.gbPaperOrientation.ResumeLayout(False)
    Me.gbPaperOrientation.PerformLayout()
    Me.gbMapScale.ResumeLayout(False)
    Me.gbMapScale.PerformLayout()
    Me.gbType.ResumeLayout(False)
    Me.gbType.PerformLayout()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents cmdCancel As System.Windows.Forms.Button
  Friend WithEvents cmdPlace As System.Windows.Forms.Button
  Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
  Friend WithEvents lblSetMapSize As System.Windows.Forms.Label
  Friend WithEvents lblSetMapScale As System.Windows.Forms.Label
  Friend WithEvents lblBorderInset As System.Windows.Forms.Label
  Friend WithEvents lblSetPaper As System.Windows.Forms.Label
  Friend WithEvents imgPageContainer As System.Windows.Forms.PictureBox
  Friend WithEvents lblSetMapSizeValue As System.Windows.Forms.Label
  Friend WithEvents lblSetMapScaleValue As System.Windows.Forms.Label
  Friend WithEvents lblSetBorderInsetValue As System.Windows.Forms.Label
  Friend WithEvents lblSetPaperValue As System.Windows.Forms.Label
  Friend WithEvents gbPaper As System.Windows.Forms.GroupBox
  Friend WithEvents gbMapScale As System.Windows.Forms.GroupBox
  Friend WithEvents gbPaperOrientation As System.Windows.Forms.GroupBox
  Friend WithEvents optPaperOrientationPortrait As System.Windows.Forms.RadioButton
  Friend WithEvents lblPaperSize As System.Windows.Forms.Label
  Friend WithEvents optPaperOrientationLandscape As System.Windows.Forms.RadioButton
  Friend WithEvents gbType As System.Windows.Forms.GroupBox
  Friend WithEvents lbType As System.Windows.Forms.ListBox
  Friend WithEvents optMapScaleCustom As System.Windows.Forms.RadioButton
  Friend WithEvents optMapScalePreDefined As System.Windows.Forms.RadioButton
  Friend WithEvents lbMapScalePreDefined As System.Windows.Forms.ListBox
  Friend WithEvents lbPaperSize As System.Windows.Forms.ListBox
  Friend WithEvents tbMapScaleCustom As System.Windows.Forms.TextBox
  Friend WithEvents cbType As System.Windows.Forms.CheckBox
  Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
  Friend WithEvents cmdADHOC As System.Windows.Forms.Button
End Class
