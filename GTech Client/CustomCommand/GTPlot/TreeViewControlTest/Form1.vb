Public Class Form1

  Private Sub cmdTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdTest.Click
    PopulateTreeViewControl()

  End Sub

  Private Sub PopulateTreeViewControl()

    Dim sKey As String
    Dim oTreeNode As TreeNode

    Try

      TreeViewControl1.BeginUpdate()



      ' Populate Available Plot Boundaries
      sKey = "Test1"
      oTreeNode = TreeViewControl1.Nodes.Add(sKey, sKey)
      oTreeNode.Name = sKey
      oTreeNode.ToolTipText = sKey
      TreeViewControl1.ImageList = ImageList1
      oTreeNode.ImageIndex = 0
      oTreeNode.SelectedImageIndex = 1

      sKey = "Test2"
      oTreeNode = TreeViewControl1.Nodes.Add(sKey, sKey)
      oTreeNode.Name = sKey
      oTreeNode.ToolTipText = sKey
      TreeViewControl1.ImageList = ImageList1
      oTreeNode.ImageIndex = 2
      oTreeNode.SelectedImageIndex = 1


      TreeViewControl1.EndUpdate()


    Catch ex As Exception
      MsgBox("Form1.PopulateTreeViewControl:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    Finally

    End Try

  End Sub

End Class
