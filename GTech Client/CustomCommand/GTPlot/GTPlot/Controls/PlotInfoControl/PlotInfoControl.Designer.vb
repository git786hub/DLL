<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PlotInfoControl
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
Me.gbType = New System.Windows.Forms.GroupBox
Me.cbType = New System.Windows.Forms.CheckBox
Me.lbType = New System.Windows.Forms.ListBox
Me.gbMapScale = New System.Windows.Forms.GroupBox
Me.lbMapScalePreDefined = New System.Windows.Forms.ListBox
Me.tbMapScaleCustom = New System.Windows.Forms.TextBox
Me.optMapScaleCustom = New System.Windows.Forms.RadioButton
Me.optMapScalePreDefined = New System.Windows.Forms.RadioButton
Me.gbPaper = New System.Windows.Forms.GroupBox
Me.gbPaperOrientation = New System.Windows.Forms.GroupBox
    Me.optPaperOrientationLandscape = New System.Windows.Forms.RadioButton
    Me.optPaperOrientationPortrait = New System.Windows.Forms.RadioButton
    Me.lbPaperSize = New System.Windows.Forms.ListBox
    Me.lblPaperSize = New System.Windows.Forms.Label
    Me.gbType.SuspendLayout()
    Me.gbMapScale.SuspendLayout()
    Me.gbPaper.SuspendLayout()
    Me.gbPaperOrientation.SuspendLayout()
    Me.SuspendLayout()
    '
    'gbType
    '
    Me.gbType.Controls.Add(Me.cbType)
    Me.gbType.Controls.Add(Me.lbType)
    Me.gbType.Location = New System.Drawing.Point(3, 3)
    Me.gbType.Name = "gbType"
    Me.gbType.Size = New System.Drawing.Size(170, 100)
    Me.gbType.TabIndex = 8
    Me.gbType.TabStop = False
    Me.gbType.Text = "Type:"
    '
    'cbType
    '
    Me.cbType.AutoSize = True
    Me.cbType.Checked = True
    Me.cbType.CheckState = System.Windows.Forms.CheckState.Checked
    Me.cbType.Location = New System.Drawing.Point(10, 0)
    Me.cbType.Name = "cbType"
    Me.cbType.Size = New System.Drawing.Size(50, 17)
    Me.cbType.TabIndex = 6
    Me.cbType.Text = "Type"
    Me.cbType.UseVisualStyleBackColor = True
    '
    'lbType
    '
    Me.lbType.FormattingEnabled = True
    Me.lbType.Items.AddRange(New Object() {"Construction", "Construction SLD", "UG Construction Print", "Pipe Line Crossing", "RCM Trace Map"})
    Me.lbType.Location = New System.Drawing.Point(10, 19)
    Me.lbType.Name = "lbType"
    Me.lbType.Size = New System.Drawing.Size(148, 69)
    Me.lbType.TabIndex = 0
    '
    'gbMapScale
    '
    Me.gbMapScale.Controls.Add(Me.lbMapScalePreDefined)
    Me.gbMapScale.Controls.Add(Me.tbMapScaleCustom)
    Me.gbMapScale.Controls.Add(Me.optMapScaleCustom)
    Me.gbMapScale.Controls.Add(Me.optMapScalePreDefined)
    Me.gbMapScale.Location = New System.Drawing.Point(3, 329)
    Me.gbMapScale.Name = "gbMapScale"
    Me.gbMapScale.Size = New System.Drawing.Size(170, 176)
    Me.gbMapScale.TabIndex = 7
    Me.gbMapScale.TabStop = False
    Me.gbMapScale.Text = "Map Scale:"
    '
    'lbMapScalePreDefined
    '
    Me.lbMapScalePreDefined.FormattingEnabled = True
    Me.lbMapScalePreDefined.Items.AddRange(New Object() {"1:250", "1:350", "1:500"})
    Me.lbMapScalePreDefined.Location = New System.Drawing.Point(10, 39)
    Me.lbMapScalePreDefined.Name = "lbMapScalePreDefined"
    Me.lbMapScalePreDefined.Size = New System.Drawing.Size(148, 82)
    Me.lbMapScalePreDefined.TabIndex = 2
    '
    'tbMapScaleCustom
    '
    Me.tbMapScaleCustom.Location = New System.Drawing.Point(10, 145)
    Me.tbMapScaleCustom.Name = "tbMapScaleCustom"
    Me.tbMapScaleCustom.Size = New System.Drawing.Size(148, 20)
    Me.tbMapScaleCustom.TabIndex = 1
    '
    'optMapScaleCustom
    '
    Me.optMapScaleCustom.AutoSize = True
    Me.optMapScaleCustom.Location = New System.Drawing.Point(10, 127)
    Me.optMapScaleCustom.Name = "optMapScaleCustom"
    Me.optMapScaleCustom.Size = New System.Drawing.Size(60, 17)
    Me.optMapScaleCustom.TabIndex = 0
    Me.optMapScaleCustom.Text = "Custom"
    Me.optMapScaleCustom.UseVisualStyleBackColor = True
    '
    'optMapScalePreDefined
    '
    Me.optMapScalePreDefined.AutoSize = True
    Me.optMapScalePreDefined.Checked = True
    Me.optMapScalePreDefined.Location = New System.Drawing.Point(10, 21)
    Me.optMapScalePreDefined.Name = "optMapScalePreDefined"
    Me.optMapScalePreDefined.Size = New System.Drawing.Size(82, 17)
    Me.optMapScalePreDefined.TabIndex = 0
    Me.optMapScalePreDefined.TabStop = True
    Me.optMapScalePreDefined.Text = "Pre-defined:"
    Me.optMapScalePreDefined.UseVisualStyleBackColor = True
    '
    'gbPaper
    '
    Me.gbPaper.Controls.Add(Me.gbPaperOrientation)
    Me.gbPaper.Controls.Add(Me.lbPaperSize)
    Me.gbPaper.Controls.Add(Me.lblPaperSize)
    Me.gbPaper.Location = New System.Drawing.Point(3, 109)
    Me.gbPaper.Name = "gbPaper"
    Me.gbPaper.Size = New System.Drawing.Size(170, 212)
    Me.gbPaper.TabIndex = 6
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
    Me.lbPaperSize.Items.AddRange(New Object() {"A-Size 8.5 x 11", "B-Size 11 x 17", "C-Size 17 x 22", "D-Size 22 x 34", "E-Size 34 x 44", "F-Size 8.5 x 30", "E-Size 17 x 34"})
    Me.lbPaperSize.Location = New System.Drawing.Point(10, 37)
    Me.lbPaperSize.Name = "lbPaperSize"
    Me.lbPaperSize.Size = New System.Drawing.Size(148, 95)
    Me.lbPaperSize.TabIndex = 1
    '
    'lblPaperSize
    '
    Me.lblPaperSize.AutoSize = True
    Me.lblPaperSize.Location = New System.Drawing.Point(7, 20)
    Me.lblPaperSize.Name = "lblPaperSize"
    Me.lblPaperSize.Size = New System.Drawing.Size(30, 13)
    Me.lblPaperSize.TabIndex = 0
    Me.lblPaperSize.Text = "Size:"
    '
    'PlotInfoControl
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.Controls.Add(Me.gbType)
    Me.Controls.Add(Me.gbMapScale)
    Me.Controls.Add(Me.gbPaper)
    Me.Name = "PlotInfoControl"
    Me.Size = New System.Drawing.Size(177, 508)
    Me.gbType.ResumeLayout(False)
    Me.gbType.PerformLayout()
    Me.gbMapScale.ResumeLayout(False)
    Me.gbMapScale.PerformLayout()
    Me.gbPaper.ResumeLayout(False)
    Me.gbPaper.PerformLayout()
    Me.gbPaperOrientation.ResumeLayout(False)
    Me.gbPaperOrientation.PerformLayout()
    Me.ResumeLayout(False)

  End Sub
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
    Friend WithEvents lblPaperSize As System.Windows.Forms.Label

End Class
