<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NewPlotWindowForm
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(NewPlotWindowForm))
    Dim Attributes1 As Intergraph.GTechnology.GTPlot.Attributes = New Intergraph.GTechnology.GTPlot.Attributes()
    Dim Attributes2 As Intergraph.GTechnology.GTPlot.Attributes = New Intergraph.GTechnology.GTPlot.Attributes()
    Me.TabControl1 = New System.Windows.Forms.TabControl()
    Me.tabPlotBoundary = New System.Windows.Forms.TabPage()
    Me.cmdRemoveAll = New System.Windows.Forms.Button()
    Me.cmdSelectAll = New System.Windows.Forms.Button()
    Me.cmdRemoveSingle = New System.Windows.Forms.Button()
    Me.cmdSelectSingle = New System.Windows.Forms.Button()
    Me.gbActiveSessionInfo = New System.Windows.Forms.GroupBox()
    Me.lblActiveSessionInfoCapitalWorkOrderNumberValue = New System.Windows.Forms.Label()
    Me.lblActiveSessionInfoJobNumberValue = New System.Windows.Forms.Label()
    Me.lblActiveSessionInfoUserValue = New System.Windows.Forms.Label()
    Me.lblActiveSessionInfoCapitalWorkOrderNumber = New System.Windows.Forms.Label()
    Me.lblActiveSessionInfoJobNumber = New System.Windows.Forms.Label()
    Me.lblActiveSessionInfoUser = New System.Windows.Forms.Label()
    Me.gbSelectedPlotBoundaries = New System.Windows.Forms.GroupBox()
    Me.tvSelectedPlotBoundaries = New System.Windows.Forms.TreeView()
    Me.GroupBox1 = New System.Windows.Forms.GroupBox()
    Me.tvAvailablePlotBoundaries = New System.Windows.Forms.TreeView()
    Me.gbPlotBoundaryFilter = New System.Windows.Forms.GroupBox()
    Me.lblPlotBoundaryFilterPlotBoundaryName = New System.Windows.Forms.Label()
    Me.cbPlotBoundaryFilterCapitalWorkOrderNumber = New System.Windows.Forms.ComboBox()
    Me.cbPlotBoundaryFilterUser = New System.Windows.Forms.ComboBox()
    Me.lblPlotBoundaryFilterCapitalWorkOrderNumber = New System.Windows.Forms.Label()
    Me.lblPlotBoundaryFilterUser = New System.Windows.Forms.Label()
    Me.tabUser = New System.Windows.Forms.TabPage()
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
    Me.gbType = New System.Windows.Forms.GroupBox()
    Me.cbType = New System.Windows.Forms.CheckBox()
    Me.lbType = New System.Windows.Forms.ListBox()
    Me.gbMapScale = New System.Windows.Forms.GroupBox()
    Me.lbMapScalePreDefined = New System.Windows.Forms.ListBox()
    Me.tbMapScaleCustom = New System.Windows.Forms.TextBox()
    Me.optMapScaleCustom = New System.Windows.Forms.RadioButton()
    Me.chkInsertActiveMapWindow = New System.Windows.Forms.CheckBox()
    Me.optMapScalePreDefined = New System.Windows.Forms.RadioButton()
    Me.gbPaper = New System.Windows.Forms.GroupBox()
    Me.gbPaperOrientation = New System.Windows.Forms.GroupBox()
    Me.optPaperOrientationLandscape = New System.Windows.Forms.RadioButton()
    Me.optPaperOrientationPortrait = New System.Windows.Forms.RadioButton()
    Me.lbPaperSize = New System.Windows.Forms.ListBox()
    Me.Label1 = New System.Windows.Forms.Label()
    Me.gbDrawingInfo = New System.Windows.Forms.GroupBox()
    Me.gbSelectedPlotBoundaryData = New System.Windows.Forms.GroupBox()
    Me.lblPlotType = New System.Windows.Forms.Label()
    Me.lblMapScale = New System.Windows.Forms.Label()
    Me.lblPaperOrientation = New System.Windows.Forms.Label()
    Me.cbPaperOrientation = New System.Windows.Forms.ComboBox()
    Me.cbPaperSize = New System.Windows.Forms.ComboBox()
    Me.cbPlotType = New System.Windows.Forms.ComboBox()
    Me.lblPaperSize = New System.Windows.Forms.Label()
    Me.cmdCancel = New System.Windows.Forms.Button()
    Me.cmdCreatePlotWindows = New System.Windows.Forms.Button()
    Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
    Me.Test = New System.Windows.Forms.Button()
    Me.ImageList2 = New System.Windows.Forms.ImageList(Me.components)
    Me.lblVersion = New System.Windows.Forms.Label()
    Me.lblPath = New System.Windows.Forms.Label()
    Me.DataGridViewAttributes1 = New Intergraph.GTechnology.GTPlot.DataGridViewAttributes()
    Me.DataGridViewAttributes2 = New Intergraph.GTechnology.GTPlot.DataGridViewAttributes()
    Me.TabControl1.SuspendLayout()
    Me.tabPlotBoundary.SuspendLayout()
    Me.gbActiveSessionInfo.SuspendLayout()
    Me.gbSelectedPlotBoundaries.SuspendLayout()
    Me.GroupBox1.SuspendLayout()
    Me.gbPlotBoundaryFilter.SuspendLayout()
    Me.tabUser.SuspendLayout()
    Me.GroupBox2.SuspendLayout()
    CType(Me.imgPageContainer, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.gbType.SuspendLayout()
    Me.gbMapScale.SuspendLayout()
    Me.gbPaper.SuspendLayout()
    Me.gbPaperOrientation.SuspendLayout()
    Me.gbDrawingInfo.SuspendLayout()
    Me.gbSelectedPlotBoundaryData.SuspendLayout()
    Me.SuspendLayout()
    '
    'TabControl1
    '
    Me.TabControl1.Controls.Add(Me.tabPlotBoundary)
    Me.TabControl1.Controls.Add(Me.tabUser)
    resources.ApplyResources(Me.TabControl1, "TabControl1")
    Me.TabControl1.Name = "TabControl1"
    Me.TabControl1.SelectedIndex = 0
    '
    'tabPlotBoundary
    '
    Me.tabPlotBoundary.Controls.Add(Me.cmdRemoveAll)
    Me.tabPlotBoundary.Controls.Add(Me.cmdSelectAll)
    Me.tabPlotBoundary.Controls.Add(Me.cmdRemoveSingle)
    Me.tabPlotBoundary.Controls.Add(Me.cmdSelectSingle)
    Me.tabPlotBoundary.Controls.Add(Me.gbActiveSessionInfo)
    Me.tabPlotBoundary.Controls.Add(Me.gbSelectedPlotBoundaries)
    Me.tabPlotBoundary.Controls.Add(Me.GroupBox1)
    Me.tabPlotBoundary.Controls.Add(Me.gbPlotBoundaryFilter)
    resources.ApplyResources(Me.tabPlotBoundary, "tabPlotBoundary")
    Me.tabPlotBoundary.Name = "tabPlotBoundary"
    Me.tabPlotBoundary.UseVisualStyleBackColor = True
    '
    'cmdRemoveAll
    '
    resources.ApplyResources(Me.cmdRemoveAll, "cmdRemoveAll")
    Me.cmdRemoveAll.Name = "cmdRemoveAll"
    Me.cmdRemoveAll.UseVisualStyleBackColor = True
    '
    'cmdSelectAll
    '
    resources.ApplyResources(Me.cmdSelectAll, "cmdSelectAll")
    Me.cmdSelectAll.Name = "cmdSelectAll"
    Me.cmdSelectAll.UseVisualStyleBackColor = True
    '
    'cmdRemoveSingle
    '
    resources.ApplyResources(Me.cmdRemoveSingle, "cmdRemoveSingle")
    Me.cmdRemoveSingle.Name = "cmdRemoveSingle"
    Me.cmdRemoveSingle.UseVisualStyleBackColor = True
    '
    'cmdSelectSingle
    '
    resources.ApplyResources(Me.cmdSelectSingle, "cmdSelectSingle")
    Me.cmdSelectSingle.Name = "cmdSelectSingle"
    Me.cmdSelectSingle.UseVisualStyleBackColor = True
    '
    'gbActiveSessionInfo
    '
    Me.gbActiveSessionInfo.Controls.Add(Me.lblActiveSessionInfoCapitalWorkOrderNumberValue)
    Me.gbActiveSessionInfo.Controls.Add(Me.lblActiveSessionInfoJobNumberValue)
    Me.gbActiveSessionInfo.Controls.Add(Me.lblActiveSessionInfoUserValue)
    Me.gbActiveSessionInfo.Controls.Add(Me.lblActiveSessionInfoCapitalWorkOrderNumber)
    Me.gbActiveSessionInfo.Controls.Add(Me.lblActiveSessionInfoJobNumber)
    Me.gbActiveSessionInfo.Controls.Add(Me.lblActiveSessionInfoUser)
    resources.ApplyResources(Me.gbActiveSessionInfo, "gbActiveSessionInfo")
    Me.gbActiveSessionInfo.Name = "gbActiveSessionInfo"
    Me.gbActiveSessionInfo.TabStop = False
    '
    'lblActiveSessionInfoCapitalWorkOrderNumberValue
    '
    resources.ApplyResources(Me.lblActiveSessionInfoCapitalWorkOrderNumberValue, "lblActiveSessionInfoCapitalWorkOrderNumberValue")
    Me.lblActiveSessionInfoCapitalWorkOrderNumberValue.ForeColor = System.Drawing.SystemColors.Highlight
    Me.lblActiveSessionInfoCapitalWorkOrderNumberValue.Name = "lblActiveSessionInfoCapitalWorkOrderNumberValue"
    '
    'lblActiveSessionInfoJobNumberValue
    '
    resources.ApplyResources(Me.lblActiveSessionInfoJobNumberValue, "lblActiveSessionInfoJobNumberValue")
    Me.lblActiveSessionInfoJobNumberValue.ForeColor = System.Drawing.SystemColors.Highlight
    Me.lblActiveSessionInfoJobNumberValue.Name = "lblActiveSessionInfoJobNumberValue"
    '
    'lblActiveSessionInfoUserValue
    '
    resources.ApplyResources(Me.lblActiveSessionInfoUserValue, "lblActiveSessionInfoUserValue")
    Me.lblActiveSessionInfoUserValue.ForeColor = System.Drawing.SystemColors.Highlight
    Me.lblActiveSessionInfoUserValue.Name = "lblActiveSessionInfoUserValue"
    '
    'lblActiveSessionInfoCapitalWorkOrderNumber
    '
    resources.ApplyResources(Me.lblActiveSessionInfoCapitalWorkOrderNumber, "lblActiveSessionInfoCapitalWorkOrderNumber")
    Me.lblActiveSessionInfoCapitalWorkOrderNumber.Name = "lblActiveSessionInfoCapitalWorkOrderNumber"
    '
    'lblActiveSessionInfoJobNumber
    '
    resources.ApplyResources(Me.lblActiveSessionInfoJobNumber, "lblActiveSessionInfoJobNumber")
    Me.lblActiveSessionInfoJobNumber.Name = "lblActiveSessionInfoJobNumber"
    '
    'lblActiveSessionInfoUser
    '
    resources.ApplyResources(Me.lblActiveSessionInfoUser, "lblActiveSessionInfoUser")
    Me.lblActiveSessionInfoUser.Name = "lblActiveSessionInfoUser"
    '
    'gbSelectedPlotBoundaries
    '
    Me.gbSelectedPlotBoundaries.Controls.Add(Me.DataGridViewAttributes1)
    Me.gbSelectedPlotBoundaries.Controls.Add(Me.tvSelectedPlotBoundaries)
    resources.ApplyResources(Me.gbSelectedPlotBoundaries, "gbSelectedPlotBoundaries")
    Me.gbSelectedPlotBoundaries.Name = "gbSelectedPlotBoundaries"
    Me.gbSelectedPlotBoundaries.TabStop = False
    '
    'tvSelectedPlotBoundaries
    '
    resources.ApplyResources(Me.tvSelectedPlotBoundaries, "tvSelectedPlotBoundaries")
    Me.tvSelectedPlotBoundaries.FullRowSelect = True
    Me.tvSelectedPlotBoundaries.HideSelection = False
    Me.tvSelectedPlotBoundaries.Name = "tvSelectedPlotBoundaries"
    Me.tvSelectedPlotBoundaries.ShowLines = False
    Me.tvSelectedPlotBoundaries.ShowPlusMinus = False
    Me.tvSelectedPlotBoundaries.ShowRootLines = False
    '
    'GroupBox1
    '
    Me.GroupBox1.Controls.Add(Me.tvAvailablePlotBoundaries)
    resources.ApplyResources(Me.GroupBox1, "GroupBox1")
    Me.GroupBox1.Name = "GroupBox1"
    Me.GroupBox1.TabStop = False
    '
    'tvAvailablePlotBoundaries
    '
    Me.tvAvailablePlotBoundaries.FullRowSelect = True
    Me.tvAvailablePlotBoundaries.HideSelection = False
    resources.ApplyResources(Me.tvAvailablePlotBoundaries, "tvAvailablePlotBoundaries")
    Me.tvAvailablePlotBoundaries.Name = "tvAvailablePlotBoundaries"
    Me.tvAvailablePlotBoundaries.ShowLines = False
    Me.tvAvailablePlotBoundaries.ShowNodeToolTips = True
    Me.tvAvailablePlotBoundaries.ShowPlusMinus = False
    Me.tvAvailablePlotBoundaries.ShowRootLines = False
    '
    'gbPlotBoundaryFilter
    '
    Me.gbPlotBoundaryFilter.Controls.Add(Me.lblPlotBoundaryFilterPlotBoundaryName)
    Me.gbPlotBoundaryFilter.Controls.Add(Me.cbPlotBoundaryFilterCapitalWorkOrderNumber)
    Me.gbPlotBoundaryFilter.Controls.Add(Me.cbPlotBoundaryFilterUser)
    Me.gbPlotBoundaryFilter.Controls.Add(Me.lblPlotBoundaryFilterCapitalWorkOrderNumber)
    Me.gbPlotBoundaryFilter.Controls.Add(Me.lblPlotBoundaryFilterUser)
    resources.ApplyResources(Me.gbPlotBoundaryFilter, "gbPlotBoundaryFilter")
    Me.gbPlotBoundaryFilter.Name = "gbPlotBoundaryFilter"
    Me.gbPlotBoundaryFilter.TabStop = False
    '
    'lblPlotBoundaryFilterPlotBoundaryName
    '
    resources.ApplyResources(Me.lblPlotBoundaryFilterPlotBoundaryName, "lblPlotBoundaryFilterPlotBoundaryName")
    Me.lblPlotBoundaryFilterPlotBoundaryName.Name = "lblPlotBoundaryFilterPlotBoundaryName"
    '
    'cbPlotBoundaryFilterCapitalWorkOrderNumber
    '
    Me.cbPlotBoundaryFilterCapitalWorkOrderNumber.FormattingEnabled = True
    resources.ApplyResources(Me.cbPlotBoundaryFilterCapitalWorkOrderNumber, "cbPlotBoundaryFilterCapitalWorkOrderNumber")
    Me.cbPlotBoundaryFilterCapitalWorkOrderNumber.Name = "cbPlotBoundaryFilterCapitalWorkOrderNumber"
    '
    'cbPlotBoundaryFilterUser
    '
    Me.cbPlotBoundaryFilterUser.FormattingEnabled = True
    resources.ApplyResources(Me.cbPlotBoundaryFilterUser, "cbPlotBoundaryFilterUser")
    Me.cbPlotBoundaryFilterUser.Name = "cbPlotBoundaryFilterUser"
    '
    'lblPlotBoundaryFilterCapitalWorkOrderNumber
    '
    resources.ApplyResources(Me.lblPlotBoundaryFilterCapitalWorkOrderNumber, "lblPlotBoundaryFilterCapitalWorkOrderNumber")
    Me.lblPlotBoundaryFilterCapitalWorkOrderNumber.Name = "lblPlotBoundaryFilterCapitalWorkOrderNumber"
    '
    'lblPlotBoundaryFilterUser
    '
    resources.ApplyResources(Me.lblPlotBoundaryFilterUser, "lblPlotBoundaryFilterUser")
    Me.lblPlotBoundaryFilterUser.Name = "lblPlotBoundaryFilterUser"
    '
    'tabUser
    '
    Me.tabUser.Controls.Add(Me.GroupBox2)
    Me.tabUser.Controls.Add(Me.gbType)
    Me.tabUser.Controls.Add(Me.gbMapScale)
    Me.tabUser.Controls.Add(Me.gbPaper)
    Me.tabUser.Controls.Add(Me.gbDrawingInfo)
    resources.ApplyResources(Me.tabUser, "tabUser")
    Me.tabUser.Name = "tabUser"
    Me.tabUser.UseVisualStyleBackColor = True
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
    resources.ApplyResources(Me.GroupBox2, "GroupBox2")
    Me.GroupBox2.Name = "GroupBox2"
    Me.GroupBox2.TabStop = False
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
    'gbMapScale
    '
    Me.gbMapScale.Controls.Add(Me.lbMapScalePreDefined)
    Me.gbMapScale.Controls.Add(Me.tbMapScaleCustom)
    Me.gbMapScale.Controls.Add(Me.optMapScaleCustom)
    Me.gbMapScale.Controls.Add(Me.chkInsertActiveMapWindow)
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
    'chkInsertActiveMapWindow
    '
    resources.ApplyResources(Me.chkInsertActiveMapWindow, "chkInsertActiveMapWindow")
    Me.chkInsertActiveMapWindow.Checked = True
    Me.chkInsertActiveMapWindow.CheckState = System.Windows.Forms.CheckState.Checked
    Me.chkInsertActiveMapWindow.Name = "chkInsertActiveMapWindow"
    Me.chkInsertActiveMapWindow.UseVisualStyleBackColor = True
    '
    'optMapScalePreDefined
    '
    resources.ApplyResources(Me.optMapScalePreDefined, "optMapScalePreDefined")
    Me.optMapScalePreDefined.Checked = True
    Me.optMapScalePreDefined.Name = "optMapScalePreDefined"
    Me.optMapScalePreDefined.TabStop = True
    Me.optMapScalePreDefined.UseVisualStyleBackColor = True
    '
    'gbPaper
    '
    Me.gbPaper.Controls.Add(Me.gbPaperOrientation)
    Me.gbPaper.Controls.Add(Me.lbPaperSize)
    Me.gbPaper.Controls.Add(Me.Label1)
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
    'Label1
    '
    resources.ApplyResources(Me.Label1, "Label1")
    Me.Label1.Name = "Label1"
    '
    'gbDrawingInfo
    '
    Me.gbDrawingInfo.Controls.Add(Me.DataGridViewAttributes2)
    resources.ApplyResources(Me.gbDrawingInfo, "gbDrawingInfo")
    Me.gbDrawingInfo.Name = "gbDrawingInfo"
    Me.gbDrawingInfo.TabStop = False
    '
    'gbSelectedPlotBoundaryData
    '
    Me.gbSelectedPlotBoundaryData.Controls.Add(Me.lblPlotType)
    Me.gbSelectedPlotBoundaryData.Controls.Add(Me.lblMapScale)
    Me.gbSelectedPlotBoundaryData.Controls.Add(Me.lblPaperOrientation)
    Me.gbSelectedPlotBoundaryData.Controls.Add(Me.cbPaperOrientation)
    Me.gbSelectedPlotBoundaryData.Controls.Add(Me.cbPaperSize)
    Me.gbSelectedPlotBoundaryData.Controls.Add(Me.cbPlotType)
    Me.gbSelectedPlotBoundaryData.Controls.Add(Me.lblPaperSize)
    resources.ApplyResources(Me.gbSelectedPlotBoundaryData, "gbSelectedPlotBoundaryData")
    Me.gbSelectedPlotBoundaryData.Name = "gbSelectedPlotBoundaryData"
    Me.gbSelectedPlotBoundaryData.TabStop = False
    '
    'lblPlotType
    '
    resources.ApplyResources(Me.lblPlotType, "lblPlotType")
    Me.lblPlotType.Name = "lblPlotType"
    '
    'lblMapScale
    '
    resources.ApplyResources(Me.lblMapScale, "lblMapScale")
    Me.lblMapScale.Name = "lblMapScale"
    '
    'lblPaperOrientation
    '
    resources.ApplyResources(Me.lblPaperOrientation, "lblPaperOrientation")
    Me.lblPaperOrientation.Name = "lblPaperOrientation"
    '
    'cbPaperOrientation
    '
    resources.ApplyResources(Me.cbPaperOrientation, "cbPaperOrientation")
    Me.cbPaperOrientation.FormattingEnabled = True
    Me.cbPaperOrientation.Name = "cbPaperOrientation"
    '
    'cbPaperSize
    '
    resources.ApplyResources(Me.cbPaperSize, "cbPaperSize")
    Me.cbPaperSize.FormattingEnabled = True
    Me.cbPaperSize.Name = "cbPaperSize"
    '
    'cbPlotType
    '
    resources.ApplyResources(Me.cbPlotType, "cbPlotType")
    Me.cbPlotType.FormattingEnabled = True
    Me.cbPlotType.Name = "cbPlotType"
    '
    'lblPaperSize
    '
    resources.ApplyResources(Me.lblPaperSize, "lblPaperSize")
    Me.lblPaperSize.Name = "lblPaperSize"
    '
    'cmdCancel
    '
    Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
    resources.ApplyResources(Me.cmdCancel, "cmdCancel")
    Me.cmdCancel.Name = "cmdCancel"
    Me.cmdCancel.UseVisualStyleBackColor = True
    '
    'cmdCreatePlotWindows
    '
    resources.ApplyResources(Me.cmdCreatePlotWindows, "cmdCreatePlotWindows")
    Me.cmdCreatePlotWindows.Name = "cmdCreatePlotWindows"
    Me.cmdCreatePlotWindows.UseVisualStyleBackColor = True
    '
    'ImageList1
    '
    Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
    Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
    Me.ImageList1.Images.SetKeyName(0, "Map.jpg")
    '
    'Test
    '
    resources.ApplyResources(Me.Test, "Test")
    Me.Test.Name = "Test"
    Me.Test.UseVisualStyleBackColor = True
    '
    'ImageList2
    '
    Me.ImageList2.ImageStream = CType(resources.GetObject("ImageList2.ImageStream"), System.Windows.Forms.ImageListStreamer)
    Me.ImageList2.TransparentColor = System.Drawing.Color.Transparent
    Me.ImageList2.Images.SetKeyName(0, "blank.bmp")
    Me.ImageList2.Images.SetKeyName(1, "ConduitFormation.bmp")
    Me.ImageList2.Images.SetKeyName(2, "ConduitFormation.bmp")
    Me.ImageList2.Images.SetKeyName(3, "expAll.ico")
    Me.ImageList2.Images.SetKeyName(4, "icoBook.ico")
    Me.ImageList2.Images.SetKeyName(5, "icoLibrary.ico")
    Me.ImageList2.Images.SetKeyName(6, "icoNote.ico")
    Me.ImageList2.Images.SetKeyName(7, "icoPage.ico")
    Me.ImageList2.Images.SetKeyName(8, "selBook.ico")
    Me.ImageList2.Images.SetKeyName(9, "selLibrary.ico")
    '
    'lblVersion
    '
    resources.ApplyResources(Me.lblVersion, "lblVersion")
    Me.lblVersion.ForeColor = System.Drawing.SystemColors.InactiveCaption
    Me.lblVersion.Name = "lblVersion"
    '
    'lblPath
    '
    resources.ApplyResources(Me.lblPath, "lblPath")
    Me.lblPath.ForeColor = System.Drawing.SystemColors.InactiveCaption
    Me.lblPath.Name = "lblPath"
    '
    'DataGridViewAttributes1
    '
    Me.DataGridViewAttributes1.Attributes = Attributes1
    resources.ApplyResources(Me.DataGridViewAttributes1, "DataGridViewAttributes1")
    Me.DataGridViewAttributes1.FontSize = 8.25!
    Me.DataGridViewAttributes1.Name = "DataGridViewAttributes1"
    Me.DataGridViewAttributes1.RowHeadersVisible = False
    '
    'DataGridViewAttributes2
    '
    resources.ApplyResources(Me.DataGridViewAttributes2, "DataGridViewAttributes2")
    Me.DataGridViewAttributes2.Attributes = Attributes2
    Me.DataGridViewAttributes2.FontSize = 8.25!
    Me.DataGridViewAttributes2.Name = "DataGridViewAttributes2"
    Me.DataGridViewAttributes2.RowHeadersVisible = False
    '
    'NewPlotWindowForm
    '
    resources.ApplyResources(Me, "$this")
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.CancelButton = Me.cmdCancel
    Me.Controls.Add(Me.lblPath)
    Me.Controls.Add(Me.lblVersion)
    Me.Controls.Add(Me.Test)
    Me.Controls.Add(Me.cmdCreatePlotWindows)
    Me.Controls.Add(Me.cmdCancel)
    Me.Controls.Add(Me.TabControl1)
    Me.Controls.Add(Me.gbSelectedPlotBoundaryData)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
    Me.KeyPreview = True
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "NewPlotWindowForm"
    Me.ShowIcon = False
    Me.ShowInTaskbar = False
    Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
    Me.TabControl1.ResumeLayout(False)
    Me.tabPlotBoundary.ResumeLayout(False)
    Me.gbActiveSessionInfo.ResumeLayout(False)
    Me.gbActiveSessionInfo.PerformLayout()
    Me.gbSelectedPlotBoundaries.ResumeLayout(False)
    Me.GroupBox1.ResumeLayout(False)
    Me.gbPlotBoundaryFilter.ResumeLayout(False)
    Me.gbPlotBoundaryFilter.PerformLayout()
    Me.tabUser.ResumeLayout(False)
    Me.GroupBox2.ResumeLayout(False)
    Me.GroupBox2.PerformLayout()
    CType(Me.imgPageContainer, System.ComponentModel.ISupportInitialize).EndInit()
    Me.gbType.ResumeLayout(False)
    Me.gbType.PerformLayout()
    Me.gbMapScale.ResumeLayout(False)
    Me.gbMapScale.PerformLayout()
    Me.gbPaper.ResumeLayout(False)
    Me.gbPaper.PerformLayout()
    Me.gbPaperOrientation.ResumeLayout(False)
    Me.gbPaperOrientation.PerformLayout()
    Me.gbDrawingInfo.ResumeLayout(False)
    Me.gbSelectedPlotBoundaryData.ResumeLayout(False)
    Me.gbSelectedPlotBoundaryData.PerformLayout()
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
  Friend WithEvents tabPlotBoundary As System.Windows.Forms.TabPage
  Friend WithEvents tabUser As System.Windows.Forms.TabPage
  Friend WithEvents gbActiveSessionInfo As System.Windows.Forms.GroupBox
  Friend WithEvents lblActiveSessionInfoCapitalWorkOrderNumber As System.Windows.Forms.Label
  Friend WithEvents lblActiveSessionInfoJobNumber As System.Windows.Forms.Label
  Friend WithEvents lblActiveSessionInfoUser As System.Windows.Forms.Label
  Friend WithEvents cmdCancel As System.Windows.Forms.Button
  Friend WithEvents cmdCreatePlotWindows As System.Windows.Forms.Button
  Friend WithEvents gbPlotBoundaryFilter As System.Windows.Forms.GroupBox
  Friend WithEvents cbPlotBoundaryFilterCapitalWorkOrderNumber As System.Windows.Forms.ComboBox
  Friend WithEvents cbPlotBoundaryFilterUser As System.Windows.Forms.ComboBox
  Friend WithEvents lblPlotBoundaryFilterCapitalWorkOrderNumber As System.Windows.Forms.Label
  Friend WithEvents lblPlotBoundaryFilterUser As System.Windows.Forms.Label
  Friend WithEvents lblPlotBoundaryFilterPlotBoundaryName As System.Windows.Forms.Label
  Friend WithEvents gbSelectedPlotBoundaryData As System.Windows.Forms.GroupBox
  Friend WithEvents cbPlotType As System.Windows.Forms.ComboBox
  Friend WithEvents lblPaperSize As System.Windows.Forms.Label
  Friend WithEvents lblPlotType As System.Windows.Forms.Label
  Friend WithEvents lblMapScale As System.Windows.Forms.Label
  Friend WithEvents lblPaperOrientation As System.Windows.Forms.Label
  Friend WithEvents cbPaperOrientation As System.Windows.Forms.ComboBox
  Friend WithEvents cbPaperSize As System.Windows.Forms.ComboBox
  Friend WithEvents gbSelectedPlotBoundaries As System.Windows.Forms.GroupBox
  Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
  Friend WithEvents cmdSelectAll As System.Windows.Forms.Button
  Friend WithEvents cmdRemoveSingle As System.Windows.Forms.Button
  Friend WithEvents cmdSelectSingle As System.Windows.Forms.Button
  Friend WithEvents cmdRemoveAll As System.Windows.Forms.Button
  Friend WithEvents tvAvailablePlotBoundaries As System.Windows.Forms.TreeView
  Friend WithEvents tvSelectedPlotBoundaries As System.Windows.Forms.TreeView
  Friend WithEvents lblActiveSessionInfoUserValue As System.Windows.Forms.Label
  Friend WithEvents lblActiveSessionInfoCapitalWorkOrderNumberValue As System.Windows.Forms.Label
  Friend WithEvents lblActiveSessionInfoJobNumberValue As System.Windows.Forms.Label
  Friend WithEvents gbDrawingInfo As System.Windows.Forms.GroupBox
  Friend WithEvents chkInsertActiveMapWindow As System.Windows.Forms.CheckBox
  Friend WithEvents DataGridViewAttributes1 As DataGridViewAttributes
  Friend WithEvents gbType As System.Windows.Forms.GroupBox
  Friend WithEvents cbType As System.Windows.Forms.CheckBox
  Friend WithEvents lbType As System.Windows.Forms.ListBox
  Friend WithEvents gbMapScale As System.Windows.Forms.GroupBox
  Friend WithEvents lbMapScalePreDefined As System.Windows.Forms.ListBox
  Friend WithEvents tbMapScaleCustom As System.Windows.Forms.TextBox
  Friend WithEvents optMapScaleCustom As System.Windows.Forms.RadioButton
  Friend WithEvents optMapScalePreDefined As System.Windows.Forms.RadioButton
  Friend WithEvents gbPaper As System.Windows.Forms.GroupBox
  Friend WithEvents gbPaperOrientation As System.Windows.Forms.GroupBox
  Friend WithEvents optPaperOrientationLandscape As System.Windows.Forms.RadioButton
  Friend WithEvents optPaperOrientationPortrait As System.Windows.Forms.RadioButton
  Friend WithEvents lbPaperSize As System.Windows.Forms.ListBox
  Friend WithEvents Label1 As System.Windows.Forms.Label
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
  Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
  Friend WithEvents Test As System.Windows.Forms.Button
  Friend WithEvents ImageList2 As System.Windows.Forms.ImageList
  Friend WithEvents DataGridViewAttributes2 As Intergraph.GTechnology.GTPlot.DataGridViewAttributes
  Friend WithEvents lblVersion As System.Windows.Forms.Label
  Friend WithEvents lblPath As System.Windows.Forms.Label
End Class
