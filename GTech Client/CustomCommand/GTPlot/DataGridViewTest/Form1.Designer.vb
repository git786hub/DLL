<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Dim Attributes1 As Intergraph.GTechnology.GTPlot.Attributes = New Intergraph.GTechnology.GTPlot.Attributes
        Me.cmdClose = New System.Windows.Forms.Button
    Me.DataGridViewAttributes1 = New Intergraph.GTechnology.GTPlot.DataGridViewAttributes
    Me.SuspendLayout()
    '
    'cmdClose
    '
    resources.ApplyResources(Me.cmdClose, "cmdClose")
    Me.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.cmdClose.Name = "cmdClose"
    Me.cmdClose.UseVisualStyleBackColor = True
    '
    'DataGridViewAttributes1
    '
    resources.ApplyResources(Me.DataGridViewAttributes1, "DataGridViewAttributes1")
    Me.DataGridViewAttributes1.Attributes = Attributes1
    Me.DataGridViewAttributes1.FontSize = 8.25!
    Me.DataGridViewAttributes1.Name = "DataGridViewAttributes1"
    Me.DataGridViewAttributes1.RowHeadersVisible = False
    '
    'Form1
    '
    Me.CancelButton = Me.cmdClose
    resources.ApplyResources(Me, "$this")
    Me.Controls.Add(Me.cmdClose)
    Me.Controls.Add(Me.DataGridViewAttributes1)
    Me.Name = "Form1"
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents cmdCancel As System.Windows.Forms.Button
  Friend WithEvents cmdClear As System.Windows.Forms.Button
  Friend WithEvents cmdDefaultDisplay As System.Windows.Forms.Button
    Friend WithEvents DataGridViewAttributes1 As Intergraph.GTechnology.GTPlot.DataGridViewAttributes
    Friend WithEvents cmdClose As System.Windows.Forms.Button

End Class
