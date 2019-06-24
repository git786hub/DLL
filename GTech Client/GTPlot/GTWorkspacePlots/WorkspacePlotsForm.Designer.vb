<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WorkspacePlotsForm
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(WorkspacePlotsForm))
    Me.lvNamedPlots = New System.Windows.Forms.ListView()
    Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
    Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
    Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
    Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
    Me.ColumnHeader5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
    Me.cmdOpen = New System.Windows.Forms.Button()
    Me.cmdClosePlot = New System.Windows.Forms.Button()
    Me.cmdUnselectAll = New System.Windows.Forms.Button()
    Me.cmdPrint = New System.Windows.Forms.Button()
    Me.ButtonBlank1 = New System.Windows.Forms.Button()
    Me.cmdSelectAll = New System.Windows.Forms.Button()
    Me.ButtonBlank2 = New System.Windows.Forms.Button()
    Me.cmdDelete = New System.Windows.Forms.Button()
    Me.cmdCopy = New System.Windows.Forms.Button()
    Me.ButtonBlank3 = New System.Windows.Forms.Button()
    Me.cmdRefresh = New System.Windows.Forms.Button()
    Me.cmdClose = New System.Windows.Forms.Button()
    Me.cmdMaximize = New System.Windows.Forms.Button()
    Me.cmdMinimize = New System.Windows.Forms.Button()
    Me.cmdRestore = New System.Windows.Forms.Button()
    Me.cmdActivate = New System.Windows.Forms.Button()
    Me.cmdRename = New System.Windows.Forms.Button()
    Me.cmdNew = New System.Windows.Forms.Button()
    Me.ButtonBlank5 = New System.Windows.Forms.Button()
    Me.ButtonBlank4 = New System.Windows.Forms.Button()
    Me.PrintDialog1 = New System.Windows.Forms.PrintDialog()
    Me.SuspendLayout()
    '
    'lvNamedPlots
    '
    resources.ApplyResources(Me.lvNamedPlots, "lvNamedPlots")
    Me.lvNamedPlots.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5})
    Me.lvNamedPlots.Cursor = System.Windows.Forms.Cursors.Default
    Me.lvNamedPlots.FullRowSelect = True
    Me.lvNamedPlots.HideSelection = False
    Me.lvNamedPlots.LabelEdit = True
    Me.lvNamedPlots.Name = "lvNamedPlots"
    Me.lvNamedPlots.UseCompatibleStateImageBehavior = False
    Me.lvNamedPlots.View = System.Windows.Forms.View.Details
    '
    'ColumnHeader1
    '
    resources.ApplyResources(Me.ColumnHeader1, "ColumnHeader1")
    '
    'ColumnHeader2
    '
    resources.ApplyResources(Me.ColumnHeader2, "ColumnHeader2")
    '
    'ColumnHeader3
    '
    resources.ApplyResources(Me.ColumnHeader3, "ColumnHeader3")
    '
    'ColumnHeader4
    '
    resources.ApplyResources(Me.ColumnHeader4, "ColumnHeader4")
    '
    'ColumnHeader5
    '
    resources.ApplyResources(Me.ColumnHeader5, "ColumnHeader5")
    '
    'cmdOpen
    '
    resources.ApplyResources(Me.cmdOpen, "cmdOpen")
    Me.cmdOpen.Name = "cmdOpen"
    Me.cmdOpen.UseVisualStyleBackColor = True
    '
    'cmdClosePlot
    '
    resources.ApplyResources(Me.cmdClosePlot, "cmdClosePlot")
    Me.cmdClosePlot.Name = "cmdClosePlot"
    Me.cmdClosePlot.UseVisualStyleBackColor = True
    '
    'cmdUnselectAll
    '
    resources.ApplyResources(Me.cmdUnselectAll, "cmdUnselectAll")
    Me.cmdUnselectAll.Name = "cmdUnselectAll"
    Me.cmdUnselectAll.UseVisualStyleBackColor = True
    '
    'cmdPrint
    '
    resources.ApplyResources(Me.cmdPrint, "cmdPrint")
    Me.cmdPrint.Name = "cmdPrint"
    Me.cmdPrint.UseVisualStyleBackColor = True
    '
    'ButtonBlank1
    '
    resources.ApplyResources(Me.ButtonBlank1, "ButtonBlank1")
    Me.ButtonBlank1.Name = "ButtonBlank1"
    Me.ButtonBlank1.TabStop = False
    Me.ButtonBlank1.UseVisualStyleBackColor = True
    '
    'cmdSelectAll
    '
    resources.ApplyResources(Me.cmdSelectAll, "cmdSelectAll")
    Me.cmdSelectAll.Name = "cmdSelectAll"
    Me.cmdSelectAll.UseVisualStyleBackColor = True
    '
    'ButtonBlank2
    '
    resources.ApplyResources(Me.ButtonBlank2, "ButtonBlank2")
    Me.ButtonBlank2.Name = "ButtonBlank2"
    Me.ButtonBlank2.TabStop = False
    Me.ButtonBlank2.UseVisualStyleBackColor = True
    '
    'cmdDelete
    '
    resources.ApplyResources(Me.cmdDelete, "cmdDelete")
    Me.cmdDelete.Name = "cmdDelete"
    Me.cmdDelete.UseVisualStyleBackColor = True
    '
    'cmdCopy
    '
    resources.ApplyResources(Me.cmdCopy, "cmdCopy")
    Me.cmdCopy.Name = "cmdCopy"
    Me.cmdCopy.UseVisualStyleBackColor = True
    '
    'ButtonBlank3
    '
    resources.ApplyResources(Me.ButtonBlank3, "ButtonBlank3")
    Me.ButtonBlank3.Name = "ButtonBlank3"
    Me.ButtonBlank3.TabStop = False
    Me.ButtonBlank3.UseVisualStyleBackColor = True
    '
    'cmdRefresh
    '
    resources.ApplyResources(Me.cmdRefresh, "cmdRefresh")
    Me.cmdRefresh.Name = "cmdRefresh"
    Me.cmdRefresh.UseVisualStyleBackColor = True
    '
    'cmdClose
    '
    resources.ApplyResources(Me.cmdClose, "cmdClose")
    Me.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.cmdClose.Name = "cmdClose"
    Me.cmdClose.UseVisualStyleBackColor = True
    '
    'cmdMaximize
    '
    resources.ApplyResources(Me.cmdMaximize, "cmdMaximize")
    Me.cmdMaximize.Name = "cmdMaximize"
    Me.cmdMaximize.TabStop = False
    Me.cmdMaximize.UseVisualStyleBackColor = True
    '
    'cmdMinimize
    '
    resources.ApplyResources(Me.cmdMinimize, "cmdMinimize")
    Me.cmdMinimize.Name = "cmdMinimize"
    Me.cmdMinimize.TabStop = False
    Me.cmdMinimize.UseVisualStyleBackColor = True
    '
    'cmdRestore
    '
    resources.ApplyResources(Me.cmdRestore, "cmdRestore")
    Me.cmdRestore.Name = "cmdRestore"
    Me.cmdRestore.TabStop = False
    Me.cmdRestore.UseVisualStyleBackColor = True
    '
    'cmdActivate
    '
    resources.ApplyResources(Me.cmdActivate, "cmdActivate")
    Me.cmdActivate.Name = "cmdActivate"
    Me.cmdActivate.TabStop = False
    Me.cmdActivate.UseVisualStyleBackColor = True
    '
    'cmdRename
    '
    resources.ApplyResources(Me.cmdRename, "cmdRename")
    Me.cmdRename.Name = "cmdRename"
    Me.cmdRename.TabStop = False
    Me.cmdRename.UseVisualStyleBackColor = True
    '
    'cmdNew
    '
    resources.ApplyResources(Me.cmdNew, "cmdNew")
    Me.cmdNew.Name = "cmdNew"
    Me.cmdNew.TabStop = False
    Me.cmdNew.UseVisualStyleBackColor = True
    '
    'ButtonBlank5
    '
    resources.ApplyResources(Me.ButtonBlank5, "ButtonBlank5")
    Me.ButtonBlank5.Name = "ButtonBlank5"
    Me.ButtonBlank5.TabStop = False
    Me.ButtonBlank5.UseVisualStyleBackColor = True
    '
    'ButtonBlank4
    '
    resources.ApplyResources(Me.ButtonBlank4, "ButtonBlank4")
    Me.ButtonBlank4.Name = "ButtonBlank4"
    Me.ButtonBlank4.TabStop = False
    Me.ButtonBlank4.UseVisualStyleBackColor = True
    '
    'PrintDialog1
    '
    Me.PrintDialog1.AllowPrintToFile = False
    Me.PrintDialog1.UseEXDialog = True
    '
    'WorkspacePlotsForm
    '
    Me.AcceptButton = Me.cmdOpen
    resources.ApplyResources(Me, "$this")
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.CancelButton = Me.cmdClose
    Me.Controls.Add(Me.cmdUnselectAll)
    Me.Controls.Add(Me.cmdNew)
    Me.Controls.Add(Me.cmdRename)
    Me.Controls.Add(Me.cmdActivate)
    Me.Controls.Add(Me.cmdRestore)
    Me.Controls.Add(Me.cmdMinimize)
    Me.Controls.Add(Me.cmdMaximize)
    Me.Controls.Add(Me.cmdClose)
    Me.Controls.Add(Me.ButtonBlank5)
    Me.Controls.Add(Me.ButtonBlank4)
    Me.Controls.Add(Me.cmdRefresh)
    Me.Controls.Add(Me.ButtonBlank3)
    Me.Controls.Add(Me.cmdCopy)
    Me.Controls.Add(Me.cmdDelete)
    Me.Controls.Add(Me.ButtonBlank2)
    Me.Controls.Add(Me.cmdSelectAll)
    Me.Controls.Add(Me.ButtonBlank1)
    Me.Controls.Add(Me.cmdPrint)
    Me.Controls.Add(Me.cmdClosePlot)
    Me.Controls.Add(Me.cmdOpen)
    Me.Controls.Add(Me.lvNamedPlots)
    Me.KeyPreview = True
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "WorkspacePlotsForm"
    Me.ShowIcon = False
    Me.ShowInTaskbar = False
    Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents lvNamedPlots As System.Windows.Forms.ListView
  Friend WithEvents cmdOpen As System.Windows.Forms.Button
  Friend WithEvents cmdClosePlot As System.Windows.Forms.Button
  Friend WithEvents cmdUnselectAll As System.Windows.Forms.Button
  Friend WithEvents cmdPrint As System.Windows.Forms.Button
  Friend WithEvents ButtonBlank1 As System.Windows.Forms.Button
  Friend WithEvents cmdSelectAll As System.Windows.Forms.Button
  Friend WithEvents ButtonBlank2 As System.Windows.Forms.Button
  Friend WithEvents cmdDelete As System.Windows.Forms.Button
  Friend WithEvents cmdCopy As System.Windows.Forms.Button
  Friend WithEvents ButtonBlank3 As System.Windows.Forms.Button
  Friend WithEvents cmdRefresh As System.Windows.Forms.Button
  Friend WithEvents cmdClose As System.Windows.Forms.Button
  Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
  Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
  Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
  Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
  Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
  Friend WithEvents cmdMaximize As System.Windows.Forms.Button
  Friend WithEvents cmdMinimize As System.Windows.Forms.Button
  Friend WithEvents cmdRestore As System.Windows.Forms.Button
  Friend WithEvents cmdActivate As System.Windows.Forms.Button
  Friend WithEvents cmdRename As System.Windows.Forms.Button
  Friend WithEvents cmdNew As System.Windows.Forms.Button
  Friend WithEvents ButtonBlank5 As System.Windows.Forms.Button
  Friend WithEvents ButtonBlank4 As System.Windows.Forms.Button
  Friend WithEvents PrintDialog1 As System.Windows.Forms.PrintDialog
End Class
