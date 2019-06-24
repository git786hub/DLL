Imports System
Imports System.Diagnostics
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms

Public Class TreeViewControl

  'Inherits System.Windows.Forms.TreeView

  'Public Const NOIMAGE As Integer = -1

  ''  Public Sub TreeViewControl : base()
  ''    ' .NET Bug: Unless LineColor is set, Win32 treeview returns -1 (default), .NET returns Color.Black!
  ''      base.LineColor = SystemColors.GrayText;

  ''      base.DrawMode = TreeViewDrawMode.OwnerDrawAll;

  ''End Sub


  'Protected Overrides Sub OnDrawNode(ByVal e As System.Windows.Forms.DrawTreeNodeEventArgs)

  '  Const SPACE_IL As Integer = 3  'space between Image and Label

  '  ' we only do additional drawing
  '  e.DrawDefault = True

  '  Me.OnDrawNode(e)

  '  If Me.ShowLines And Not IsNothing(Me.ImageList) And e.Node.ImageIndex = NOIMAGE And (Me.ShowRootLines Or e.Node.Level > 0) Then

  '    ' exclude root nodes, if root lines are disabled
  '    '&& (me.ShowRootLines || e.Node.Level > 0))

  '    ' Using lines & images, but this node has none: fill up missing treelines

  '    ' Image size
  '    Dim imgW As Integer = Me.ImageList.ImageSize.Width
  '    Dim imgH As Integer = Me.ImageList.ImageSize.Height

  '    ' Image center
  '    Dim xPos As Integer = e.Node.Bounds.Left - SPACE_IL - imgW / 2
  '    Dim yPos As Integer = (e.Node.Bounds.Top + e.Node.Bounds.Bottom) / 2

  '    ' Image rect
  '    'Rectangle(imgRect = New Rectangle(xPos, yPos, 0, 0))
  '    Dim imgRect = New Rectangle(xPos, yPos, 0, 0)

  '    imgRect.Inflate(imgW / 2, imgH / 2)

  '    Dim p As Pen

  '    Try
  '      p = New Pen(Me.LineColor, 1)

  '      p.DashStyle = DashStyle.Dot

  '      ' account uneven Indent for both lines
  '      p.DashOffset = Me.Indent * 1.02

  '      ' Horizontal treeline across width of image
  '      ' account uneven half of delta ItemHeight & ImageHeight
  '      Dim yHor As Integer = yPos + ((Me.ItemHeight - imgRect.Height) / 2) * 1.02

  '      'if (me.ShowRootLines || e.Node.Level > 0)
  '      '{
  '      '    e.Graphics.DrawLine(p, imgRect.Left, yHor, imgRect.Right, yHor);
  '      '}
  '      'else
  '      '{
  '      '    ' for root nodes, if root lines are disabled, start at center
  '      '    e.Graphics.DrawLine(p, xPos - (int)p.DashOffset, yHor, imgRect.Right, yHor);
  '      '}
  '      Dim x1 As Integer
  '      If (Me.ShowRootLines Or e.Node.Level > 0) Then
  '        x1 = imgRect.Left
  '      Else
  '        x1 = xPos - Int(p.DashOffset)
  '      End If
  '      e.Graphics.DrawLine(p, x1, yHor, imgRect.Right, yHor)

  '      If Not Me.CheckBoxes And e.Node.IsExpanded Then
  '        ' Vertical treeline , offspring from NodeImage center to e.Node.Bounds.Bottom
  '        ' yStartPos: account uneven Indent and uneven half of delta ItemHeight & ImageHeight
  '        Dim yVer As Integer = yHor + Int(p.DashOffset)

  '        e.Graphics.DrawLine(p, xPos, yVer, xPos, e.Node.Bounds.Bottom)
  '      End If

  '    Catch ex As Exception
  '      MsgBox("TreeViewControl.OnDrawNode:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)

  '    Finally
  '      p.Dispose()
  '      p = Nothing
  '    End Try


  '  End If

  'End Sub

  'Protected Overrides Sub OnAfterCollapse(ByVal e As System.Windows.Forms.TreeViewEventArgs)
  '  Me.OnAfterCollapse(e)
  '  If Not Me.CheckBoxes And Not IsNothing(Me.ImageList) And e.Node.ImageIndex = NOIMAGE Then
  '    ' DrawNode event not raised: redraw node with collapsed treeline
  '    Me.Invalidate(e.Node.Bounds)
  '  End If

  'End Sub


  ' Removed because duplicate sub in Designer code
  'Private Sub InitializeComponent()

  '  Me.SuspendLayout()
  '  '
  '  'TreeViewControl
  '  '
  '  Me.Name = "TreeViewControl"
  '  Me.ResumeLayout(False)

  'End Sub

End Class
