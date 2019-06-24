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
    Me.components = New System.ComponentModel.Container
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
    Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
    Me.cmdTest = New System.Windows.Forms.Button
    Me.SuspendLayout()
    '
    'ImageList1
    '
    Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
    Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
    Me.ImageList1.Images.SetKeyName(0, "icoLibrary.ico")
    Me.ImageList1.Images.SetKeyName(1, "selLibrary.ico")
    Me.ImageList1.Images.SetKeyName(2, "expAll.ico")
    Me.ImageList1.Images.SetKeyName(3, "icoBook.ico")
    Me.ImageList1.Images.SetKeyName(4, "icoNote.ico")
    Me.ImageList1.Images.SetKeyName(5, "icoPage.ico")
    Me.ImageList1.Images.SetKeyName(6, "selBook.ico")
    '
    'cmdTest
    '
    Me.cmdTest.Location = New System.Drawing.Point(290, 472)
    Me.cmdTest.Name = "cmdTest"
    Me.cmdTest.Size = New System.Drawing.Size(75, 23)
    Me.cmdTest.TabIndex = 2
    Me.cmdTest.Text = "Test"
    Me.cmdTest.UseVisualStyleBackColor = True
    '
    'Form1
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(377, 507)
    Me.Controls.Add(Me.cmdTest)
    Me.Name = "Form1"
    Me.Text = "TreeViewControlTest"
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
  'Intergraph.GTechnology.GTPlot.TreeViewControl
  Friend WithEvents cmdTest As System.Windows.Forms.Button

End Class
