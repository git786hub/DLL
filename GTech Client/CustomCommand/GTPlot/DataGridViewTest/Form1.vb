Imports Intergraph.GTechnology.GTPlot

Public Class Form1

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

    GTLocalization.SetCurrentCulture("fr-CA")

    LoadTestData()
    'LoadSampleData()

    Debug.Print("Culture is: {0}", GTLocalization.GetCurrentCulture)


  End Sub

  Private Sub LoadTestData()
    Dim oAttribute As Attribute


    DataGridViewAttributes1.ClearDataGridViewAttributes()

    'DataGridViewAttributes1.Attributes.Clear() 'Use the above instead

    DataGridViewAttributes1.Attributes.Add("TextBox Samples")

    oAttribute = New Attribute
    With oAttribute
      .VALUE_TYPE = Attribute.CellType.TextBox
      .G3E_USERNAME = "TextBox"
      .VALUE = "Sample Text"
    End With
    DataGridViewAttributes1.Attributes.Add(oAttribute)

    oAttribute = New Attribute
    With oAttribute
      .VALUE_TYPE = Attribute.CellType.TextBox
      .G3E_USERNAME = "TextBox - ReadOnly"
      .VALUE = "Sample ReadOnly Text"
      .ValueReadOnly = True
    End With
    DataGridViewAttributes1.Attributes.Add(oAttribute)

    DataGridViewAttributes1.Attributes.Add("ComboBox Samples")

    oAttribute = New Attribute
    With oAttribute
      .VALUE_TYPE = Attribute.CellType.ComboBox
      .G3E_USERNAME = "ComboBox With Space"
      .VALUE = ""
      .VALUE_LIST.Add("")
      .VALUE_LIST.Add("Paul Adams")
      .VALUE_LIST.Add("Rob")
      .VALUE_LIST.Add("Stuff")
      .VALUE_LIST.Add("Stuff1")
      .VALUE_LIST.Add("Stuff2")
      .VALUE_LIST.Add("Stuff3")
    End With
    DataGridViewAttributes1.Attributes.Add(oAttribute)


    oAttribute = New Attribute
    With oAttribute
      .VALUE_TYPE = Attribute.CellType.ComboBox
      .G3E_USERNAME = "ComboBox"
      .VALUE = "Paul"
      .VALUE_LIST.Add("Paul")
      .VALUE_LIST.Add("Rob")
      .VALUE_LIST.Add("Stuff")
      .VALUE_LIST.Add("Stuff1")
      .VALUE_LIST.Add("Stuff2")
      .VALUE_LIST.Add("Stuff3")
    End With
    DataGridViewAttributes1.Attributes.Add(oAttribute)

    oAttribute = New Attribute
    With oAttribute
      .VALUE_TYPE = Attribute.CellType.ComboBox
      .G3E_USERNAME = "ComboBox2"
      .VALUE = "Paul2"
      .VALUE_LIST.Add("Paul2")
      .VALUE_LIST.Add("Rob2")
      .VALUE_LIST.Add("Stuff2")
      .VALUE_LIST.Add("Stuff21")
      .VALUE_LIST.Add("Stuff22")
      .VALUE_LIST.Add("Stuff23")
    End With
    DataGridViewAttributes1.Attributes.Add(oAttribute)



    DataGridViewAttributes1.Attributes.Add("TreeView Samples")

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

    Dim TreeNode12 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("East -5 PVCD2")
    Dim TreeNode13 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("West -5 PVCD2")
    Dim TreeNode14 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("West -5 PVCD")
    Dim TreeNode15 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("1AHH3", New System.Windows.Forms.TreeNode() {TreeNode12, TreeNode13, TreeNode14})
    Dim TreeNode16 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("East -3 PVCD")
    Dim TreeNode17 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("West -3 PVCD")
    Dim TreeNode18 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("1AHH4", New System.Windows.Forms.TreeNode() {TreeNode16, TreeNode17})
    Dim TreeNode19 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Manhole", New System.Windows.Forms.TreeNode() {TreeNode15, TreeNode18})
    Dim TreeNode20 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("West -3 PVCD")
    Dim TreeNode21 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("1 Wood Pole St E", New System.Windows.Forms.TreeNode() {TreeNode20})
    Dim TreeNode22 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Pole", New System.Windows.Forms.TreeNode() {TreeNode21})
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
    TreeNode4.Expand()
    TreeNode5.Name = "Node6"
    TreeNode5.Text = "East -2 PVCD"
    TreeNode6.Name = "Node5"
    TreeNode6.Text = "West -2 PVCD"
    TreeNode7.Name = "Node5"
    TreeNode7.Text = "1AHH2"
    TreeNode7.Expand()
    TreeNode8.Name = "Node2"
    TreeNode8.Text = "Handhole"
    TreeNode8.Expand()

    TreeNode9.Name = "Node4"
    TreeNode9.Text = "West -2 PVCD"
    TreeNode10.Name = "Node1"
    TreeNode10.Text = "PED 100 Paul St E"
    TreeNode10.Expand()
    TreeNode11.Name = "Ped"
    TreeNode11.Text = "Ped"
    TreeNode11.Expand()

    'TreeNode12.Name = "Node32"
    'TreeNode12.Text = "East -4 PVCD2"
    'TreeNode13.Name = "Node1"
    'TreeNode13.Text = "West -4 PVCD2"
    'TreeNode14.Name = "Node22"
    'TreeNode14.Text = "West -2 PVCD"
    'TreeNode15.Name = "Node32"
    'TreeNode15.Text = "1AHH1"
    TreeNode15.Expand()
    'TreeNode16.Name = "Node62"
    'TreeNode16.Text = "East -2 PVCD"
    'TreeNode17.Name = "Node52"
    'TreeNode17.Text = "West -2 PVCD"
    'TreeNode18.Name = "Node52"
    'TreeNode18.Text = "1AHH2"
    TreeNode18.Expand()
    'TreeNode19.Name = "Node22"
    'TreeNode19.Text = "Handhole"
    TreeNode19.Expand()

    'TreeNode20.Name = "Node42"
    'TreeNode20.Text = "West -2 PVCD"
    'TreeNode21.Name = "Node12"
    'TreeNode21.Text = "PED 100 Paul St E"
    TreeNode21.Expand()
    'TreeNode22.Name = "Ped"
    'TreeNode22.Text = "Ped"
    TreeNode22.Expand()

    With oTreeView
      .CheckBoxes = True
      .Scrollable = False ' If used and want the parent control to update its scroll bars you must call 
      .ShowRootLines = False
    End With
    'oTreeView.Nodes.AddRange(New System.Windows.Forms.TreeNode() {TreeNode8, TreeNode11})
    oTreeView.Nodes.AddRange(New System.Windows.Forms.TreeNode() {TreeNode8, TreeNode11, TreeNode19, TreeNode22})

    oAttribute = New Attribute
    With oAttribute
      .VALUE_TYPE = Attribute.CellType.TreeView
      .G3E_USERNAME = "TreeView Control Sample Cell"
      .TreeView = oTreeView
    End With
    DataGridViewAttributes1.Attributes.Add(oAttribute)

    'DataGridViewAttributes1.Attributes.Add("TreeView End")

    DataGridViewAttributes1.RefreshDataGridViewAttributes()
  End Sub

  Private Sub LoadSampleData()
    Dim oAttribute As Attribute


    DataGridViewAttributes1.ClearDataGridViewAttributes()

    'DataGridViewAttributes1.Attributes.Clear() 'Use the above instead


    DataGridViewAttributes1.Attributes.Add("TextBox Samples")

    oAttribute = New Attribute
    With oAttribute
      .VALUE_TYPE = Attribute.CellType.TextBox
      .G3E_USERNAME = "TextBox"
      .VALUE = "Sample Text"
    End With
    DataGridViewAttributes1.Attributes.Add(oAttribute)

    oAttribute = New Attribute
    With oAttribute
      .VALUE_TYPE = Attribute.CellType.TextBox
      .G3E_USERNAME = "TextBox - ReadOnly"
      .VALUE = "Sample ReadOnly Text"
      .ValueReadOnly = True
    End With
    DataGridViewAttributes1.Attributes.Add(oAttribute)

    DataGridViewAttributes1.Attributes.Add("CheckBox Samples")

    oAttribute = New Attribute
    With oAttribute
      .VALUE_TYPE = Attribute.CellType.CheckBox
      .G3E_USERNAME = "CheckBox"
      .VALUE = Attribute.CheckBoxValue.Checked
    End With
    DataGridViewAttributes1.Attributes.Add(oAttribute)

    oAttribute = New Attribute
    With oAttribute
      .VALUE_TYPE = Attribute.CellType.MultiCheckBox
      '.MultiCheckBoxValue.Neutral = Attribute.MultiCheckBoxValue.Neutral
      .G3E_USERNAME = "MultiCheckBox"
      .VALUE = Attribute.MultiCheckBoxValue.Neutral
    End With
    DataGridViewAttributes1.Attributes.Add(oAttribute)

    DataGridViewAttributes1.Attributes.Add("ComboBox Samples")

    oAttribute = New Attribute
    With oAttribute
      .VALUE_TYPE = Attribute.CellType.ComboBox
      .G3E_USERNAME = "ComboBox"
      .VALUE = "Paul"
      .VALUE_LIST.Add("Paul")
      .VALUE_LIST.Add("Rob")
      .VALUE_LIST.Add("Stuff")
      .VALUE_LIST.Add("Stuff1")
      .VALUE_LIST.Add("Stuff2")
      .VALUE_LIST.Add("Stuff3")
    End With
    DataGridViewAttributes1.Attributes.Add(oAttribute)

    oAttribute = New Attribute
    With oAttribute
      .VALUE_TYPE = Attribute.CellType.ComboBox
      .G3E_USERNAME = "ComboBox With Space"
      .VALUE = ""
      .VALUE_LIST.Add("")
      .VALUE_LIST.Add("Paul Adams")
      .VALUE_LIST.Add("Rob")
      .VALUE_LIST.Add("Stuff")
      .VALUE_LIST.Add("Stuff1")
      .VALUE_LIST.Add("Stuff2")
      .VALUE_LIST.Add("Stuff3")
    End With
    DataGridViewAttributes1.Attributes.Add(oAttribute)


    DataGridViewAttributes1.Attributes.Add("TreeView Samples")

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
    DataGridViewAttributes1.Attributes.Add(oAttribute)

    DataGridViewAttributes1.RefreshDataGridViewAttributes()
  End Sub

  Private Sub cmdTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
    Me.Close()
  End Sub


  Private Sub cmdDefaultDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDefaultDisplay.Click
    LoadSampleData()
  End Sub

  Private Sub cmdClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClear.Click
    DataGridViewAttributes1.ClearDataGridViewAttributes()
    DataGridViewAttributes1.RefreshDataGridViewAttributes()
  End Sub


  Private Sub DataGridViewAttributes1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridViewAttributes1.Load

  End Sub

  Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
    Me.Close()
    Me.Dispose()
  End Sub

End Class
