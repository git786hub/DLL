Public Class DataGridViewAttributes

  Event CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs, ByVal oAttribute As Attribute)

  'Private mAttributes As Collection
  'Private m_Attributes As Collection
  Private m_Attributes As Attributes
  Private m_SuspendPaint As Boolean = False
  Private m_RowHeadersVisible As Boolean = False
  Private m_FontSize As Single = 0

  Public Overloads Property Attributes() As Attributes
    Get
      Attributes = m_Attributes
    End Get
    Set(ByVal value As Attributes)
      m_Attributes = value
    End Set
  End Property

  Private Property SuspendPaint() As Boolean
    Get
      SuspendPaint = m_SuspendPaint
    End Get
    Set(ByVal value As Boolean)
      m_SuspendPaint = value
    End Set
  End Property

  Public Property RowHeadersVisible() As Boolean
    Get
      RowHeadersVisible = m_RowHeadersVisible
    End Get
    Set(ByVal value As Boolean)
      m_RowHeadersVisible = value
    End Set
  End Property

  Public Property FontSize() As Single
    Get
      FontSize = m_FontSize
    End Get
    Set(ByVal value As Single)
      m_FontSize = value
    End Set
  End Property

  Public Sub ClearDataGridViewAttributes()

    SuspendPaint = True

    ' This is now done in the RefreshDataGridViewAttributes and CellPaint methods instead to improve clean refresh.  Call RefreshDataGridViewAttributes directly after if you want a blank container.
    '' Hide TreeView control if any
    'For Each oAttribute As Attribute In m_Attributes
    '  If oAttribute.VALUE_TYPE = Attribute.CellType.TreeView Then
    '    oAttribute.TreeView.Dispose()
    '    oAttribute.TreeView = Nothing
    '  End If
    'Next
    'DataGridView1.Rows.Clear()

    Attributes.Clear()


  End Sub

  Public Sub RefreshDataGridViewAttributes()

    Dim iRowIndex As Integer
    Dim oDGVCell As DataGridViewCell
    Dim oTreeView As TreeView = Nothing

    ' Remove all the existing rows
    DataGridView1.Rows.Clear()

    Dim i As Integer = DataGridView1.Rows.Count

    ' Add the new rows
    For Each oAttribute As Attribute In Attributes
      Debug.Print(oAttribute.G3E_USERNAME)
      iRowIndex = DataGridView1.Rows.Add(oAttribute.G3E_USERNAME, oAttribute.VALUE)
      oDGVCell = DataGridView1.Rows(iRowIndex).Cells(1)
      oDGVCell.ReadOnly = oAttribute.ValueReadOnly
      If oAttribute.VALUE_TYPE = Attribute.CellType.TreeView Then
        oAttribute.TreeView.Visible = False
        oTreeView = oAttribute.TreeView
      End If
    Next

    ' Remove previously used TreeViews if none defined to be used this time around
    If IsNothing(oTreeView) Then
      Dim oType As System.Type
      For Each oObject As Object In DataGridView1.Controls
        oType = oObject.GetType()
        If oType.Name = "TreeView" Then
          oObject.Dispose()
          oObject = Nothing
        End If
      Next
    End If

    DataGridView1.ClearSelection()
    SuspendPaint = False

  End Sub

  Private Sub DefaultDisplay()

    Dim oAttribute As Attribute

    Attributes.Add("Standard Cells")
    Attributes.Add("TextBox", "Sample Text", False, Attribute.CellType.TextBox)
    Attributes.Add("TextBox 2", "Sample Text 2", False, Attribute.CellType.TextBox)

    Attributes.Add("Custom Cells")
    Attributes.Add("CheckBox", Attribute.CheckBoxValue.Checked)
    Attributes.Add("MultiCheckBox", Attribute.MultiCheckBoxValue.Neutral)
    Attributes.Add("ComboBox", "ComboBox Item 1", False, Attribute.CellType.ComboBox, "ComboBox Item 1", "ComboBox Item 2", "ComboBox Item 3")

    oAttribute = New Attribute
    With oAttribute
      .VALUE_TYPE = Attribute.CellType.TextBox
      .G3E_USERNAME = "TextBox - ReadOnly"
      .VALUE = "Sample ReadOnly Text"
      .ValueReadOnly = True
    End With
    Attributes.Add(oAttribute)

    '
    ' Tree View Control
    '
    Attributes.Add("TreeView Samples")

    ' Create TreeView control
    Dim oTreeView As New TreeView
    oTreeView.CreateControl()
    oTreeView.Name = "TreeViewControlSample"

    ' Register Events
    AddHandler oTreeView.AfterCollapse, AddressOf TreeView_AfterExpand_Handler
    AddHandler oTreeView.AfterExpand, AddressOf TreeView_AfterExpand_Handler
    AddHandler oTreeView.AfterCheck, AddressOf TreeView_AfterCheck_Handler
    AddHandler oTreeView.AfterSelect, AddressOf TreeView_AfterSelect_Handler

    '
    'oTreeView
    '
    Dim TreeNode1 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("East -4 PVCD2")
    Dim TreeNode2 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("West -4 PVCD2")
    Dim TreeNode3 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("West -2 PVCD")
    Dim TreeNode4 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("1AHH1", New System.Windows.Forms.TreeNode() {TreeNode1, TreeNode2, TreeNode3})
    Dim TreeNode5 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("East -2 PVCD")
    Dim TreeNode6 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("West -2 PVCD")
    Dim TreeNode7 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("1AHH2", New System.Windows.Forms.TreeNode() {TreeNode5, TreeNode6})
    Dim TreeNode8 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Handhole", New System.Windows.Forms.TreeNode() {TreeNode4, TreeNode7})
    Dim TreeNode9 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("West -2 PVCD")
    Dim TreeNode10 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("PED 100 Paul St E", New System.Windows.Forms.TreeNode() {TreeNode9})
    Dim TreeNode11 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Ped", New System.Windows.Forms.TreeNode() {TreeNode10})
    '
    'oTreeView
    '
    TreeNode1.Name = "Node3"
    TreeNode1.Text = "East -4 PVCD2"
    TreeNode2.Name = "Node1"
    TreeNode2.Text = "West -4 PVCD2"
    TreeNode3.Name = "Node2"
    TreeNode3.Text = "West -2 PVCD"
    TreeNode4.Name = "Node3"
    TreeNode4.Text = "1AHH1"
    TreeNode5.Name = "Node6"
    TreeNode5.Text = "East -2 PVCD"
    TreeNode6.Name = "Node5"
    TreeNode6.Text = "West -2 PVCD"
    TreeNode7.Name = "Node5"
    TreeNode7.Text = "1AHH2"
    TreeNode8.Name = "Node2"
    TreeNode8.Text = "Handhole"
    TreeNode8.Expand()

    TreeNode9.Name = "Node4"
    TreeNode9.Text = "West -2 PVCD"
    TreeNode10.Name = "Node1"
    TreeNode10.Text = "PED 100 Paul St E"
    TreeNode11.Name = "Ped"
    TreeNode11.Text = "Ped"
    TreeNode11.Expand()

    With oTreeView
      .CheckBoxes = True
      .Scrollable = False ' If used and want the parent control to update its scroll bars you must call 
      .ShowRootLines = False
    End With
    oTreeView.Nodes.AddRange(New System.Windows.Forms.TreeNode() {TreeNode8, TreeNode11})

    oAttribute = New Attribute
    With oAttribute
      .VALUE_TYPE = Attribute.CellType.TreeView
      .G3E_USERNAME = "TreeView Control Sample Cell"
      .TreeView = oTreeView
    End With
    Attributes.Add(oAttribute)


    RefreshDataGridViewAttributes()

  End Sub

  ' Prevent selection of node.
  Private Sub TreeView_AfterSelect_Handler(ByVal sender As Object, ByVal e As TreeViewEventArgs)
    Dim oTreeView As TreeView
    oTreeView = sender
    oTreeView.SelectedNode = Nothing
  End Sub


  ' Updates all child tree nodes recursively.
  Private Sub CheckAllChildNodes(ByVal treeNode As TreeNode, ByVal nodeChecked As Boolean)
    Dim node As TreeNode
    For Each node In treeNode.Nodes
      node.Checked = nodeChecked
      If node.Nodes.Count > 0 Then
        ' If the current node has child nodes, call the CheckAllChildsNodes method recursively.
        Me.CheckAllChildNodes(node, nodeChecked)
      End If
    Next node
  End Sub

  '' NOTE   This code can be added to the BeforeCheck event handler instead of the AfterCheck event.
  '' After a tree node's Checked property is changed, all its child nodes are updated to the same value.
  Private Sub TreeView_AfterCheck_Handler(ByVal sender As Object, ByVal e As TreeViewEventArgs)
    ' The code only executes if the user caused the checked state to change.
    If e.Action <> TreeViewAction.Unknown Then
      If e.Node.Nodes.Count > 0 Then
        ' Calls the CheckAllChildNodes method, passing in the current 
        ' Checked value of the TreeNode whose checked state changed.
        Me.CheckAllChildNodes(e.Node, e.Node.Checked)
      End If
    End If
  End Sub

  Private Sub TreeView_AfterExpand_Handler(ByVal sender As Object, ByVal e As TreeViewEventArgs)

    Dim oTreeView As TreeView
    oTreeView = sender
    Debug.Print("TreeView_AfterExpand_Handler for " & oTreeView.Name)
    ' This will cause the DataGridView control to repaint
    oTreeView.Visible = False

  End Sub

  Private Sub TreeView_AfterCollapse_Handler(ByVal sender As Object, ByVal e As TreeViewEventArgs)
    Dim oTreeView As TreeView
    oTreeView = sender
    Debug.Print("TreeView_AfterCollapse_Handler for " & oTreeView.Name)
    ' This will cause the DataGridView control to repaint
    oTreeView.Visible = False
  End Sub


  Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    If Me.DesignMode Then
      DefaultDisplay()
    End If
  End Sub

  Private Sub DataGridView1_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellClick

    Dim oDGVCell, oDGVCellName As DataGridViewCell

    ' Exit if not a value cell
    If e.ColumnIndex <= 0 Or e.RowIndex = -1 Or m_Attributes.Count = 0 Then Exit Sub
    Debug.Print("ColumnIndex:" & e.ColumnIndex & " x RowIndex:" & e.RowIndex & " DisplayedRowIndex:" & e.RowIndex - DataGridView1.FirstDisplayedScrollingRowIndex)

    oDGVCell = DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex)
    oDGVCellName = DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex - 1)

    Select Case Attributes(oDGVCellName.Value).VALUE_TYPE
      Case Attribute.CellType.CheckBox
        Select Case oDGVCell.Value
          Case "1"
            oDGVCell.Value = "0"
          Case "0"
            oDGVCell.Value = "1"
        End Select
      Case Attribute.CellType.MultiCheckBox
        Select Case oDGVCell.Value
          Case "-1"
            oDGVCell.Value = "1"
          Case "1"
            oDGVCell.Value = "0"
          Case "0"
            oDGVCell.Value = "-1"
        End Select
      Case Attribute.CellType.ComboBox
        If IsNothing(oDGVCell.Value) Then
          ' This was causing the oComboBox to loose many of it's properties like Height.
          ' Default is Empty anywas so doing nothing seems to work.
          ' oComboBox.Text = ""
        Else
          If Not String.IsNullOrEmpty(oComboBox.Text) Then
            oComboBox.Text = oDGVCell.Value
          End If
        End If
        ' Added DataGridView1.FirstDisplayedScrollingRowIndex to resolve issue when DataGrid is scrolled down and top rows are not visable.
        Debug.Print("DGVCell.Size.Height=" & oDGVCell.Size.Height)
        oComboBox.Top = (oDGVCell.Size.Height * (e.RowIndex - DataGridView1.FirstDisplayedScrollingRowIndex)) + 2
        Debug.Print("oComboBox.Top=" & oComboBox.Top)
        oComboBox.Left = DataGridView1.Columns(0).Width + 1
        oComboBox.Size = DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex).Size
        oComboBox.FlatStyle = FlatStyle.Standard
        'oComboBox.Location = DataGridView1.Location

        oComboBox.Visible = True
        oComboBox.Parent = DataGridView1
        oComboBox.Items.Clear()

        Dim oAttribute As Attribute
        oAttribute = m_Attributes(e.RowIndex + 1)

        For Each str As String In oAttribute.VALUE_LIST()
          oComboBox.Items.Add(str)
        Next

        oComboBox.DroppedDown = True
    End Select


  End Sub

  Public Sub DataGridView1_CellPainting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles DataGridView1.CellPainting

    Dim newRect As New Rectangle(e.CellBounds.X + 1, e.CellBounds.Y + 1, e.CellBounds.Width - 4, e.CellBounds.Height - 4)
    Dim backColorBrush As New SolidBrush(e.CellStyle.BackColor)
    'Dim backColorBrushReadonly As New SolidBrush(e.CellStyle.BackColor)
    Dim gridBrush As New SolidBrush(Me.DataGridView1.GridColor)
    Dim gridLinePen As New Pen(gridBrush)

    Dim oDGVCell As DataGridViewCell
    Dim oDGVCellName As DataGridViewCell

    Try
      If m_SuspendPaint = True Then Exit Sub
      ' Exit if not a value cell
      If e.RowIndex = -1 Then Exit Sub
      If Not m_Attributes.Count > 0 Then Exit Sub
      If Attributes Is Nothing Then Exit Sub

      oDGVCell = DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex)

      Dim backColorBrushReadonly = New SolidBrush(oDGVCell.Style.BackColor)
      If FontSize = 0 Then
        FontSize = e.CellStyle.Font.Size
      End If

      If e.ColumnIndex = -1 Then Exit Sub

      Select Case e.ColumnIndex
        Case 0 ' Process 1st Column

          Select Case Attributes(oDGVCell.Value).VALUE_TYPE

            'Dim oAttribute As Attribute
            'oAttribute = m_Attributes(e.RowIndex)
            'Select Case oAttribute.VALUE_TYPE

            Case Attribute.CellType.TreeView
              ' Nothing to be done in this case, do all the work for the in the second column case




            Case Attribute.CellType.Group
              ' Erase the cell.
              e.Graphics.FillRectangle(Brushes.LightGray, e.CellBounds)

              '' Draw the grid lines (only the right and bottom lines;
              '' DataGridView takes care of the others).
              e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1)
              'e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom)

              ' Draw the text content of the cell, ignoring alignment.
              If Not (e.Value Is Nothing) Then
                'Dim oFont As New System.Drawing.Font
                'System.Drawing.FontStyle.Bold()

                'Dim f As Font
                'oFont.Bold = True

                Dim drawFont As New Font(e.CellStyle.Font.Name, e.CellStyle.Font.Size, FontStyle.Bold)
                'Dim drawFont As New Font("Microsoft Sans Serif", 10, FontStyle.Bold)
                'Dim drawFont As New Font("Tahoma", 10, FontStyle.Bold)

                e.Graphics.DrawString(CStr(e.Value), drawFont, Brushes.Black, e.CellBounds.X + 2, e.CellBounds.Y + 5, StringFormat.GenericDefault)

              End If
              e.Handled = True
            Case Else
              ' Global set RowHeadersVisible -Set 1st row to grey otherwise simply erase the cell.
              If RowHeadersVisible Then
                'e.Graphics.FillRectangle(Brushes.Gainsboro, e.CellBounds)
                'e.Graphics.FillRectangle(Brushes.LightBlue, e.CellBounds)
                'e.Graphics.FillRectangle(Brushes.PowderBlue, e.CellBounds)
                'e.Graphics.FillRectangle(Brushes.SteelBlue, e.CellBounds)
                e.Graphics.FillRectangle(Brushes.WhiteSmoke, e.CellBounds)
              Else
                e.Graphics.FillRectangle(backColorBrush, e.CellBounds)
              End If


              '' Draw the grid lines (only the right and bottom lines;
              '' DataGridView takes care of the others).
              e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1)
              e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom)

              ' Draw the text content of the cell, ignoring alignment.
              If Not (e.Value Is Nothing) Then

                If Attributes(oDGVCell.Value).G3E_REQUIRED Then ' If required attribute make text bold
                  Dim drawFont As New Font(e.CellStyle.Font.Name, FontSize, FontStyle.Bold)
                  e.Graphics.DrawString(CStr(e.Value), drawFont, Brushes.Black, e.CellBounds.X + 2, e.CellBounds.Y + 4, StringFormat.GenericDefault)
                Else
                  e.Graphics.DrawString(CStr(e.Value), e.CellStyle.Font, Brushes.Black, e.CellBounds.X + 2, e.CellBounds.Y + 4, StringFormat.GenericDefault)
                End If
              End If
              e.Handled = True

          End Select

        Case 1 ' Process 2nd Column
          'Debug.Print(e.ColumnIndex & " x " & e.RowIndex)
          oDGVCellName = DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex - 1)


          Select Case Attributes(oDGVCellName.Value).VALUE_TYPE
            Case Attribute.CellType.TreeView

              Static iec2 As Integer = 0
              iec2 = iec2 + 1
              Debug.Print("----DataGridView1_CellPainting - " & iec2)
              'Debug.Print("TreeViewRow " & e.RowIndex)
              'Debug.Print("Visible     " & DataGridView1.GetCellCount(DataGridViewElementStates.Visible))
              'Debug.Print("Displayed   " & DataGridView1.GetCellCount(DataGridViewElementStates.Displayed))
              'Debug.Print("FirstDisplayedScrollingRowIndex " & DataGridView1.FirstDisplayedScrollingRowIndex)
              'Debug.Print("DisplayedRowCount               " & DataGridView1.DisplayedRowCount(False))

              ' Get the height of all displayed rows up to the TreeView's DataGridView row.
              ' Todo -only supports a single TreeView control instance at the moment
              Dim oDGVCelltemp As DataGridViewCell
              Dim iTotalHeight As Integer = 0
              For i = 0 To e.RowIndex - 1
                oDGVCelltemp = DataGridView1.Rows(i).Cells(e.ColumnIndex)
                If i >= DataGridView1.FirstDisplayedScrollingRowIndex Then
                  iTotalHeight = iTotalHeight + oDGVCelltemp.Size.Height
                End If
              Next

              ' Remove previously used TreeViews
              Dim oType As System.Type
              For Each oObject As Object In DataGridView1.Controls
                oType = oObject.GetType()
                If oType.Name = "TreeView" Then
                  If Not ReferenceEquals(oObject, Attributes(oDGVCellName.Value).TreeView) Then
                    oObject.Dispose()
                    oObject = Nothing
                  End If
                End If
              Next





              Dim oTreeView As TreeView = Nothing
              oTreeView = Attributes(oDGVCellName.Value).TreeView
              If Not oTreeView Is Nothing Then

                oTreeView.Top = iTotalHeight + 1
                ' This is the old TOP that had issues... oTreeView.Top = (oDGVCell.Size.Height * (e.RowIndex - DataGridView1.FirstDisplayedScrollingRowIndex)) + 1
                oTreeView.Left = DataGridView1.Left + 1
                'oTreeView.Size = DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex).Size

                ' Expand TreeView Height to the number of visable items
                Dim oTreeNode As TreeNode
                Dim iExpandedNodes As Integer
                If oTreeView.Nodes.Count > 0 Then
                  oTreeNode = oTreeView.Nodes(0)
                  While Not IsNothing(oTreeNode)
                    iExpandedNodes = iExpandedNodes + 1
                    oTreeNode = oTreeNode.NextVisibleNode
                  End While
                End If
                oTreeView.Height = oTreeView.ItemHeight * (iExpandedNodes + 0.5)

                ' Set DataGridView Row Height to match
                DataGridView1.Rows(e.RowIndex).Height = oTreeView.Height

                ' Set width after height in order to adjest for if a scroll bar exists.
                oTreeView.Width = DataGridView1.Rows(e.RowIndex - 1).Cells(e.ColumnIndex - 1).Size.Width + DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex).Size.Width + 1 '- 10

                oTreeView.Parent = DataGridView1
                oTreeView.Visible = True
                oTreeView.Refresh()
                oTreeView.BringToFront()
              End If

              '' Place Container withing Cell
              'With Attributes(oDGVCell.Value).TreeView
              '  .Top = e.CellBounds.Top
              '  .Left = e.CellBounds.Left
              '  .Height = e.CellBounds.Height + 200
              '  .Width = e.CellBounds.Width + 200
              '  .Show()
              'End With
              'Attributes(oDGVCell.Value).ContainerControl.Show()

            Case Attribute.CellType.Group
              ' Erase the cell.
              e.Graphics.FillRectangle(Brushes.LightGray, e.CellBounds)

              '' Draw the grid lines (only the right and bottom lines;
              '' DataGridView takes care of the others).
              e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1)
              e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom)

              e.Handled = True

            Case Attribute.CellType.TextBox
              If Attributes(oDGVCellName.Value).ValueReadOnly Then
                ' Erase the cell.
                'e.Graphics.FillRectangle(Brushes.LightGray, e.CellBounds)
                e.Graphics.FillRectangle(backColorBrush, e.CellBounds)

                ' Draw the grid lines (only the right and bottom lines;
                ' DataGridView takes care of the others).
                e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1)
                e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom)

                ' Draw the text content of the cell, ignoring alignment.
                If Not (e.Value Is Nothing) Then
                  e.Graphics.DrawString(CStr(e.Value), e.CellStyle.Font, Brushes.Gray, e.CellBounds.X + 2, e.CellBounds.Y + 4, StringFormat.GenericDefault)
                End If
                e.Handled = True
              End If

            Case Attribute.CellType.CheckBox, Attribute.CellType.MultiCheckBox
              oDGVCell.ReadOnly = True

              ' Erase the cell.
              e.Graphics.FillRectangle(backColorBrush, e.CellBounds)

              ' Draw the grid lines (only the right and bottom lines;
              ' DataGridView takes care of the others).
              e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1)
              e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom)

              ' Draw the inset highlight box.
              'e.Graphics.DrawRectangle(Pens.Blue, newRect)


              Select Case e.Value
                Case "-1"
                  e.Graphics.DrawImageUnscaled(ImageList1.Images("CheckBox-Neutral.bmp"), (e.CellBounds.Location.X + e.CellBounds.Width / 2) - (ImageList1.Images("CheckBox-Neutral.bmp").Width / 2), e.CellBounds.Location.Y + 2)
                Case "1"
                  e.Graphics.DrawImageUnscaled(ImageList1.Images("CheckBox-Checked.bmp"), (e.CellBounds.Location.X + e.CellBounds.Width / 2) - (ImageList1.Images("CheckBox-Checked.bmp").Width / 2), e.CellBounds.Location.Y + 2)
                Case "0"
                  e.Graphics.DrawImageUnscaled(ImageList1.Images("CheckBox.bmp"), (e.CellBounds.Location.X + e.CellBounds.Width / 2) - (ImageList1.Images("CheckBox.bmp").Width / 2), e.CellBounds.Location.Y + 2)
              End Select
              'If e.Value = "True" Then
              '  e.Graphics.DrawImageUnscaled(ImageList1.Images("CheckBox-Checked.bmp"), (e.CellBounds.Location.X + e.CellBounds.Width / 2) - (ImageList1.Images("CheckBox-Checked.bmp").Width / 2), e.CellBounds.Location.Y + 2)
              'Else
              '  e.Graphics.DrawImageUnscaled(ImageList1.Images("CheckBox.bmp"), (e.CellBounds.Location.X + e.CellBounds.Width / 2) - (ImageList1.Images("CheckBox.bmp").Width / 2), e.CellBounds.Location.Y + 2)
              'End If

              ' Draw the text content of the cell, ignoring alignment.
              If Not (e.Value Is Nothing) Then
                'e.Graphics.DrawString(CStr(e.Value), e.CellStyle.Font, Brushes.Crimson, e.CellBounds.X + 2, e.CellBounds.Y + 4, StringFormat.GenericDefault)
              End If
              e.Handled = True

            Case Attribute.CellType.ComboBox
              oDGVCell.ReadOnly = True

              ' Erase the cell.
              e.Graphics.FillRectangle(backColorBrush, e.CellBounds)

              ' Draw the grid lines (only the right and bottom lines;
              ' DataGridView takes care of the others).
              e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1)
              e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom)

              ' Draw the inset highlight box.
              'e.Graphics.DrawRectangle(Pens.Blue, newRect)

              e.Graphics.DrawImageUnscaled(ImageList1.Images("DropDownList.bmp"), e.CellBounds.Location.X + e.CellBounds.Width - 19, e.CellBounds.Location.Y + 2)
              ' Draw the text content of the cell, ignoring alignment.
              If Not (e.Value Is Nothing) Then
                e.Graphics.DrawString(CStr(e.Value), e.CellStyle.Font, Brushes.Black, e.CellBounds.X + 2, e.CellBounds.Y + 4, StringFormat.GenericDefault)
              End If
              e.Handled = True

          End Select
      End Select




      'If e.ColumnIndex = 1 And e.RowIndex = 1 Then
      '  Dim g As Graphics = e.Graphics

      '  Dim clipBounds As System.Drawing.Rectangle
      '  Dim paintParts As DataGridViewPaintParts

      '  g.Clear(Color.FromKnownColor(KnownColor.Highlight)) ' Was using RGB=0,192,192 or DeepSkyBlue or CadetBlue
      '  g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
      '  g.PageUnit = GraphicsUnit.Pixel
      '  Dim oImage As Image
      '  oImage.

      '  clipBounds = e.CellBounds
      '  e.Paint(clipBounds, DataGridViewPaintParts.All)
      '  e.Paint(clipBounds, DataGridViewPaintParts.Background Or DataGridViewPaintParts.Border Or DataGridViewPaintParts.ContentBackground Or DataGridViewPaintParts.ContentForeground Or DataGridViewPaintParts.ErrorIcon)

      '  g.DrawImage(ImageList1.Images("DropDownList.jpg"), 0, 0)

      '  g.BeginContainer()
      '  g.SetClip(e.CellBounds)
      '  g.DrawImage(ImageList1.Images("DropDownList.jpg"), e.CellBounds.Location)
      '  g.EndContainer(e.)
      'End If


    Finally
      gridLinePen.Dispose()
      gridBrush.Dispose()
      backColorBrush.Dispose()
    End Try


  End Sub

  Private Sub DataGridView1_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellValueChanged

    Dim oDGVCell As DataGridViewCell
    Dim oAttribute As Attribute

    ' Exit if not a value cell
    If Attributes Is Nothing Or e.ColumnIndex <= 0 Or e.RowIndex = -1 Then Exit Sub
    'Debug.Print(e.ColumnIndex & " x " & e.RowIndex)

    oDGVCell = DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex)
    oAttribute = Attributes.Item(e.RowIndex + 1)
    oAttribute.VALUE = oDGVCell.Value

    RaiseEvent CellValueChanged(sender, e, oAttribute)

  End Sub

  Private Sub oComboBox_DropDownClosed(ByVal sender As Object, ByVal e As System.EventArgs) Handles oComboBox.DropDownClosed
    oComboBox.Hide()
  End Sub

  Private Sub oComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles oComboBox.SelectedIndexChanged
    oComboBox.Hide()
    DataGridView1.SelectedCells(0).Value = oComboBox.Text
  End Sub

  Public Sub New()

    ' This call is required by the Windows Form Designer.
    InitializeComponent()

    ' Add any initialization after the InitializeComponent() call.
    m_Attributes = New Attributes

  End Sub

  Private Sub DataGridView1_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles DataGridView1.Scroll



    Static i As Integer = 0
    'i = i + 1
    'Debug.Print("----DataGridView1_Scroll - " & i)

    'Debug.Print("Visible     " & DataGridView1.GetCellCount(DataGridViewElementStates.Visible))
    'Debug.Print("Displayed   " & DataGridView1.GetCellCount(DataGridViewElementStates.Displayed))
    'Debug.Print("FirstDisplayedScrollingRowIndex " & DataGridView1.FirstDisplayedScrollingRowIndex)
    'Debug.Print("DisplayedRowCount               " & DataGridView1.DisplayedRowCount(False))

    ' Get column TreeView is displayed on
    ' Todo -Only support having a single TreeView control at the moment.
    Dim iTVrow As Integer = 0
    Dim oTreeViewAttributes As Attribute = Nothing

    For Each oAttribute As Attribute In Attributes
      'Debug.Print(oAttribute.G3E_USERNAME)
      If oAttribute.VALUE_TYPE = Attribute.CellType.TreeView Then
        oTreeViewAttributes = oAttribute
        Exit For
      End If
      iTVrow = iTVrow + 1
    Next
    'Debug.Print("If " & DataGridView1.FirstDisplayedScrollingRowIndex + DataGridView1.DisplayedRowCount(False) & " <= " & iTVrow & "Hide Tree View Control")

    ' Hide the TreeView control always causing the DataGridView to refresh.
    If Not oTreeViewAttributes Is Nothing Then
      oTreeViewAttributes.TreeView.Visible = False
    End If
    Exit Sub


    ' Hide the TreeView control only if its DataGridView row is scrolled out of view.
    ' This had to be done because DataGridView1_CellPainting is not called when this happens
    If DataGridView1.FirstDisplayedScrollingRowIndex + DataGridView1.DisplayedRowCount(False) <= iTVrow Then
      oTreeViewAttributes.TreeView.Visible = False
    Else
      oTreeViewAttributes.TreeView.Visible = True
    End If





    ' '' Create function to loop through and redraw all cells
    ''Dim oDGVCell As DataGridViewCell
    ''Dim oFormattedValue As Object

    ' ''DataBindingComplete()
    ' ''DataGridView1_DataBindingComplete()

    ''oDGVCell = DataGridView1.Rows(iTVrow).Cells(1)
    ''oFormattedValue = oDGVCell.FormattedValue

    ''Dim oControlBindingsCollection As ControlBindingsCollection
    ''oControlBindingsCollection = DataGridView1.DataBindings
    ''oDGVCell = DataGridView1.Rows(iTVrow).Cells(1)

    ''If oDGVCell.Value = " + " Then
    ''  oDGVCell.Value = " - "
    ''Else
    ''  oDGVCell.Value = " + "
    ''End If

    ' ''InvalidateCell()
    ' ''InvalidateRow()

    ' ''DataGridView1_RowPrePaint(sender, oDGVCell)
    ' ''DataGridView1.DataBindingComplete( = p_dgvToManipulate_DataBindingComplete


    ' ''DataGridView1.Rows(iTVrow).Cells(1).Selected = True
    ' ''DataGridView1.NotifyCurrentCellDirty(True)
    ''Exit Sub




  End Sub


End Class
