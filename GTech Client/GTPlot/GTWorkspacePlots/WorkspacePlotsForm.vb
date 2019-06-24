Imports System
Imports Microsoft.Win32
Imports Microsoft.VisualBasic
Imports Intergraph.GTechnology.API

Public Class WorkspacePlotsForm

  Private strBeforeLabelEditValue As String
  Private m_bNamedPlots_Escape_KeyUp As Boolean = False
  Private m_iPlotWindows_ClosedPlots As Integer = -1
  Private m_aPlotWindows_Closed() As String

  ' Save settings
  Private Sub Save()
    Try
      Dim sUserSubKeyCulture = My.Resources.RegistryCurrentUserSubKey & "\\" & GetCurrentCulture()
      Dim RegKey As RegistryKey = Registry.CurrentUser.OpenSubKey(sUserSubKeyCulture, True)
      If IsNothing(RegKey) Then RegKey = Registry.CurrentUser.CreateSubKey(sUserSubKeyCulture, RegistryKeyPermissionCheck.ReadWriteSubTree)
      RegKey.SetValue(Me.Name & ".Top", Me.Top)
      RegKey.SetValue(Me.Name & ".Left", Me.Left)
      RegKey.SetValue(Me.Name & ".Height", Me.Height)
      RegKey.SetValue(Me.Name & ".Width", Me.Width)
      RegKey.Close()
    Catch ex As Exception
    End Try
  End Sub

  ' Restore settings
  Private Sub Restore()
    Try
      Dim sUserSubKeyCulture = My.Resources.RegistryCurrentUserSubKey & "\\" & GetCurrentCulture()
      Dim RegKey As RegistryKey = Registry.CurrentUser.OpenSubKey(sUserSubKeyCulture)
      If IsNothing(RegKey) Then RegKey = Registry.CurrentUser.CreateSubKey(sUserSubKeyCulture, RegistryKeyPermissionCheck.ReadWriteSubTree)
      If Not IsNothing(RegKey.GetValue(Me.Name & ".Top")) Then
        Me.Top = RegKey.GetValue(Me.Name & ".Top")
        Me.Left = RegKey.GetValue(Me.Name & ".Left")
        Me.Height = RegKey.GetValue(Me.Name & ".Height")
        Me.Width = RegKey.GetValue(Me.Name & ".Width")
      End If
      RegKey.Close()
    Catch ex As Exception
    End Try
  End Sub

  Private Sub WorkspacePlotsForm_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
    Try
      If e.KeyCode = Keys.Escape Then
        If m_bNamedPlots_Escape_KeyUp Then
          m_bNamedPlots_Escape_KeyUp = False
          e.SuppressKeyPress = True
          e.Handled = True
        Else
          Dispose()
        End If
      End If

    Catch ex As Exception
      MsgBox("WorkspacePlotsForm.WorkspacePlotsForm_KeyUp:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    Finally
    End Try

  End Sub


  Private Sub WorkspacePlotsForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Try



#If DEBUG Then
            ButtonBlank3.Enabled = True
            ButtonBlank3.Visible = True
            ButtonBlank3.Text = "Test Open"
            ButtonBlank4.Enabled = True
            ButtonBlank4.Visible = True
            ButtonBlank4.Text = "Test Close"
            'Debug.Print("Culture: {0}", GetCurrentCulture())
#End If

            If System.Diagnostics.Debugger.IsAttached = False Then
                ButtonBlank3.Enabled = False
                ButtonBlank3.Visible = False
                ButtonBlank3.Text = "Test Open"
                ButtonBlank4.Enabled = False
                ButtonBlank4.Visible = False
                ButtonBlank4.Text = "Test Close"
                'Debug.Print("Culture: {0}", GetCurrentCulture())
            End If

            Restore()

      PopulateList()

    Catch ex As Exception
      MsgBox("WorkspacePlotsForm.WorkspacePlotsForm_Load:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    Finally
    End Try

  End Sub

  Private Sub WorkspacePlotsForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    'If MyOKButtonIsPressed = False Then Dispose()
    Save()
    Dispose()
  End Sub

  Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
    Try
      If m_bNamedPlots_Escape_KeyUp Then
        m_bNamedPlots_Escape_KeyUp = False
      Else
        Dispose()
      End If

    Catch ex As Exception
      MsgBox("WorkspacePlotsForm.cmdClose_Click:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    Finally
    End Try
  End Sub

  Public Sub PopulateList()

    Dim oListItem As ListViewItem

    Dim oNamedPlot As IGTNamedPlot
    Dim oGTPlotredlines As IGTPlotRedlines

    Dim oGTPlotFrames As IGTPlotFrames
    Dim oGTPlotFrame As IGTPlotFrame

    Dim intMaps As Integer
    Dim intObjects As Integer

    Dim oGTApplication As IGTApplication

    Try
      lvNamedPlots.Items.Clear()

      oGTApplication = GTClassFactory.Create(Of IGTApplication)()

      'Dim oTimer1 As New Timer
      'oTimer1.Interval = 200
      'oTimer1.Start()

      For Each oNamedPlot In oGTApplication.NamedPlots

        oListItem = lvNamedPlots.Items.Add("")
        'Debug.Print oListItem.Index
        oListItem.Selected = False
        oListItem.Text = oNamedPlot.Name

        intMaps = 0
        intObjects = 0
        oGTPlotFrames = oNamedPlot.Frames
        For Each oGTPlotFrame In oGTPlotFrames
          If oGTPlotFrame.Type = GTPlotFrameTypeConstants.gtpftMap Then
            intMaps = intMaps + 1
          Else
            intObjects = intObjects + 1
          End If
        Next oGTPlotFrame

        oListItem.SubItems.Add("")
        oListItem.SubItems.Add(intMaps)
        oListItem.SubItems.Add(intObjects)

        oGTPlotredlines = oNamedPlot.GetRedlines(GTPlotRedlineCollectionConstants.gtprcAll)
        oListItem.SubItems.Add(oGTPlotredlines.Count)
      Next oNamedPlot

      lvNamedPlots.Sorting = SortOrder.Descending
      lvNamedPlots.Sorting = SortOrder.Ascending
      lvNamedPlots.Sort()

    Catch ex As Exception
      MsgBox("WorkspacePlotsForm.PopulateList:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    Finally
      'oListItem = Nothing
      'oNamedPlot = Nothing
      'oGTPlotredlines = Nothing
      'oGTPlotFrames = Nothing
      'oGTPlotFrame = Nothing

      RefreshListItemStatus()
      UpdateButtonStates()
    End Try

  End Sub


  Private Sub RefreshListItemStatus()

    Dim oListItem As ListViewItem
    Dim oGTPlotWindow As IGTPlotWindow
    Dim oGTActivePlotWindow As IGTPlotWindow
    Dim oGTPlotWindows As IGTPlotWindows
    Dim oGTMapWindow As IGTMapWindow
    Dim oGTMapWindows As IGTMapWindows
    Dim oGTNamedPlot As IGTNamedPlot
    Dim bActivePlotWindow As Boolean = False
    Dim bActiveMapWindow As Boolean = False
    Dim oGTApplication As IGTApplication
    Dim bNamedPlotOpen As Boolean

    oGTApplication = GTClassFactory.Create(Of IGTApplication)()

    Try

      ' Save Status Bar Text and Begin Wait Cursor
      'Dim sStatusBarText As String = oGTApplication.GetStatusBarText(GTStatusPanelConstants.gtaspcMessage)
      oGTApplication.BeginWaitCursor()

      ' User feedback
      'oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "L1")


      ' Clear List
      For Each oListItem In lvNamedPlots.Items
        oListItem.SubItems(1).Text = ""
        'oListItem.text = ""
      Next oListItem

      oGTPlotWindows = GTClassFactory.Create(Of IGTPlotWindows)()
      oGTPlotWindows = oGTApplication.GetPlotWindows

      oGTMapWindows = oGTApplication.GetMapWindows(GTMapWindowTypeConstants.gtapmtAll)

      ' Test to see if a MapWindow is Active
      For Each oGTMapWindow In oGTMapWindows
        If oGTMapWindow.WindowState = GTWindowStateConstants.gtmwwsMaximize Then
          bActiveMapWindow = True
        End If
      Next

      ' It was decided to just leave the status blank
      ' Mark all as closed
      'For Each oListItem In lvNamedPlots.Items
      '  oListItem.SubItems(1).Text = "Closed"
      'Next oListItem

      ' Identify what PlotWindows are Open
      For Each oGTPlotWindow In oGTPlotWindows
        For Each oListItem In lvNamedPlots.Items
          oGTNamedPlot = oGTPlotWindow.NamedPlot
          If oGTNamedPlot.Name = oListItem.Text Then
            If Not oGTPlotWindow.WindowState = GTWindowStateConstants.gtmwwsMinimize Then
              bActivePlotWindow = True
            End If
            'If oGTPlotWindow.WindowState = GTWindowStateConstants.gtmwwsMaximize Then
            '  oListItem.SubItems(1).Text = "Maximized"
            'Else
            '  oListItem.SubItems(1).Text = "Minimized"
            'End If

            ' Work Around for SR#1-495315529 -Test if named plot has just been closed then omit from open plot windows
            bNamedPlotOpen = True
            For i = 0 To m_iPlotWindows_ClosedPlots
              If m_aPlotWindows_Closed(i) = oGTNamedPlot.Name Then
                bNamedPlotOpen = False
                Exit For
              End If
            Next

            If bNamedPlotOpen Then
              oListItem.SubItems(1).Text = "Open"
            End If

          End If
        Next oListItem
      Next oGTPlotWindow




      ' Identify what PlotWindow is Active
      If Not oGTPlotWindows.Count = 0 Then
        If bActivePlotWindow Then
          If Not bActiveMapWindow Then ' Test to see if a MapWindow is Active not Plot Window
            oGTActivePlotWindow = oGTApplication.ActivePlotWindow
            If Not IsNothing(oGTActivePlotWindow) Then
              oGTNamedPlot = oGTActivePlotWindow.NamedPlot
              For Each oListItem In lvNamedPlots.Items
                If oGTNamedPlot.Name = oListItem.Text Then

                  ' Work Around for SR#1-495315529 -Test if named plot has just been closed then omit from active plot windows
                  bNamedPlotOpen = True
                   For i = 0 To m_iPlotWindows_ClosedPlots
                    If m_aPlotWindows_Closed(i) = oGTNamedPlot.Name Then
                      bNamedPlotOpen = False
                      Exit For
                    End If
                  Next

                  If bNamedPlotOpen Then
                    oListItem.SubItems(1).Text = "Active"
                  End If

                End If
              Next oListItem
            End If
          End If
        End If
      End If

      lvNamedPlots.Focus()

    Catch ex As Exception
      MsgBox("WorkspacePlotsForm.RefreshListItemStatus:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    Finally

      ' Clear array of named plot that were closed.
      If m_iPlotWindows_ClosedPlots >= 0 Then
        m_iPlotWindows_ClosedPlots = -1
        ReDim m_aPlotWindows_Closed(0)
      End If

      'oListItem = Nothing
      'oGTPlotWindow = Nothing
      'oGTPlotWindows = Nothing
      'oGTMapWindow = Nothing
      'oGTMapWindows = Nothing
      'oGTNamedPlot = Nothing
      oGTApplication.EndWaitCursor()
    End Try

  End Sub

  Private Sub cmdRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRefresh.Click
    PopulateList()
  End Sub

  Private Sub ButtonBlank4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonBlank4.Click

    Dim oGTPlotWindow As IGTPlotWindow
    Dim oGTPlotWindows As IGTPlotWindows
    Dim oGTApplication As IGTApplication
    Try

      oGTPlotWindow = GTClassFactory.Create(Of IGTPlotWindow)() ' Tried with and without this line.
      oGTApplication = GTClassFactory.Create(Of IGTApplication)()
      oGTPlotWindows = GTClassFactory.Create(Of IGTPlotWindows)()

      oGTPlotWindows = oGTApplication.GetPlotWindows
      For Each oGTPlotWindow In oGTPlotWindows
        'If oGTPlotWindow.NamedPlot.Name = "100" Then
        oGTPlotWindow.Close()
        'End If
      Next oGTPlotWindow

      Application.DoEvents() 'Causing GTech to get an error saving workspace ofter closing the dialog.

      ' Identify what PlotWindows are Open still open
      Dim oGTNamedPlot As IGTNamedPlot
      Dim sOpenPlotNames As String = ""
      oGTPlotWindows = oGTApplication.GetPlotWindows
      For Each oGTPlotWindow In oGTPlotWindows
        oGTNamedPlot = oGTPlotWindow.NamedPlot
        If Not IsNothing(oGTPlotWindow.WindowState) Then
          Select Case oGTPlotWindow.WindowState
            Case GTWindowStateConstants.gtmwwsMaximize
              sOpenPlotNames = sOpenPlotNames & oGTNamedPlot.Name & "=Maximize " & vbLf
            Case GTWindowStateConstants.gtmwwsMinimize
              sOpenPlotNames = sOpenPlotNames & oGTNamedPlot.Name & "=Minimize " & vbLf
            Case GTWindowStateConstants.gtmwwsRestore
              sOpenPlotNames = sOpenPlotNames & oGTNamedPlot.Name & "=Restore " & vbLf
          End Select
        End If
      Next oGTPlotWindow
      If oGTPlotWindows.Count > 0 Then
        MsgBox(sOpenPlotNames, MsgBoxStyle.Information, "Current Plot Window States")
      Else
        MsgBox("No Open Plot Windows Found", MsgBoxStyle.Information, "Current Plot Window States")
      End If

    Catch ex As Exception
      MsgBox("WorkspacePlotsForm.ButtonBlank4_Click:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    Finally
    End Try

  End Sub

  Private Sub cmdClosePlot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClosePlot.Click

    Dim oGTApplication As IGTApplication
    Dim oListItem As ListViewItem
    Dim oGTPlotWindows As IGTPlotWindows
    Dim oGTPlotWindow As IGTPlotWindow
    Dim oGTNamedPlot As IGTNamedPlot
    Dim oGTNamedPlotOpen As IGTNamedPlot

    oGTApplication = GTClassFactory.Create(Of IGTApplication)()

    Try

      'oGTPlotWindow = GTClassFactory.Create(Of IGTPlotWindow)() ' Tried with and without this line.
      'oGTPlotWindows = GTClassFactory.Create(Of IGTPlotWindows)()
      'oGTNamedPlot = GTClassFactory.Create(Of IGTNamedPlot)() ' Tried with and without this line.
      'oGTNamedPlotOpen = GTClassFactory.Create(Of IGTNamedPlot)() ' Tried with and without this line.

      oGTPlotWindows = oGTApplication.GetPlotWindows

      For Each oGTPlotWindow In oGTPlotWindows
        For Each oListItem In lvNamedPlots.Items
          If oListItem.Selected Then
            oGTNamedPlot = oGTApplication.NamedPlots(oListItem.Text)
            oGTNamedPlotOpen = oGTPlotWindow.NamedPlot
            If oGTNamedPlot.Name = oGTNamedPlotOpen.Name Then
              Try ' Active map window on close causes crash so Deactivate before closing
                If Not oGTPlotWindow.ActiveFrame.Name = "" Then
                  oGTPlotWindow.ActiveFrame.Deactivate()
                End If
              Catch ex As Exception
                'MsgBox("WorkspacePlotsForm.cmdClosePlot_Click:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
              End Try

              ' Work Around for SR#1-495315529 -Save plot names to later omit from open plot window collection
              m_iPlotWindows_ClosedPlots += 1
              ReDim Preserve m_aPlotWindows_Closed(m_iPlotWindows_ClosedPlots)
              m_aPlotWindows_Closed(m_iPlotWindows_ClosedPlots) = oGTNamedPlot.Name

              oGTPlotWindow.Close()

              'Application.DoEvents() 'Causing GTech to get an error saving workspace ofter closing the dialog. SR#1-495315529
              Exit For
            End If
          End If
        Next oListItem
      Next oGTPlotWindow

      'oGTNamedPlot = Nothing ' Tried with and without this line.
      'oGTNamedPlotOpen = Nothing ' Tried with and without this line.
      'oGTPlotWindow = Nothing
      'oGTPlotWindows = Nothing

    Catch ex As Exception
      MsgBox("WorkspacePlotsForm.cmdClosePlot_Click:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    Finally
      'oGTNamedPlotOpen = Nothing
      'oGTNamedPlot = Nothing
      'oListItem = Nothing
      'oGTPlotWindow = Nothing
      'oGTPlotWindows = Nothing
      'oGTApplication = Nothing
      'GC.Collect()

      'oGTApplication.RefreshWindows()
      'Application.DoEvents() 'Causing GTech to get an error saving workspace ofter closing the dialog. SR#1-495315529
      RefreshListItemStatus()

    End Try
  End Sub



  Private Sub cmdCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCopy.Click

    Dim I As Integer
    Dim oListItem As ListViewItem
    Dim oGTNamedPlot As IGTNamedPlot = Nothing
    Dim oGTNamedPlot2 As IGTNamedPlot
    Dim oGTNamedPlotNew As IGTNamedPlot
    Dim colGTNamedPlot As New Collection
    Dim oGTApplication As IGTApplication

    Try
      oGTApplication = GTClassFactory.Create(Of IGTApplication)()

      I = 1
      For Each oListItem In lvNamedPlots.Items
        If oListItem.Selected Then
          oGTNamedPlot = oGTApplication.NamedPlots(oListItem.Text)
          'check WO obj list to see if copy already exists and increment counter if found
          For Each oGTNamedPlot2 In oGTApplication.NamedPlots
            If oGTNamedPlot2.Name = "Copy(" & I & ") " & oGTNamedPlot.Name Then
              I = I + 1
            End If
          Next oGTNamedPlot2
        End If
      Next oListItem

      'Add new item to WO plan list
      oGTNamedPlotNew = oGTNamedPlot.CopyPlot("Copy(" & I & ") " & oGTNamedPlot.Name)

      'rebuild the list
      PopulateList()

    Catch ex As Exception
      MsgBox("WorkspacePlotsForm.cmdCopy_Click:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    Finally
      'oListItem = Nothing
      'oGTNamedPlot = Nothing
      'oGTNamedPlot2 = Nothing
      'oGTNamedPlotNew = Nothing
      colGTNamedPlot = Nothing
    End Try

  End Sub

  Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click

    Dim oListItem As ListViewItem
    Dim oGTNamedPlot As IGTNamedPlot
    Dim colGTNamedPlot As New Collection
    Dim oGTPlotWindow As IGTPlotWindow = Nothing
    Dim bNamedPlotRemoved As Boolean
    Dim oGTApplication As IGTApplication

    Try
      oGTApplication = GTClassFactory.Create(Of IGTApplication)()

      For Each oListItem In lvNamedPlots.Items
        If oListItem.Selected Then
          If Not IsNamedPlotOpen(oListItem.Text, oGTPlotWindow) Then
            oGTNamedPlot = oGTApplication.NamedPlots(oListItem.Text)
            colGTNamedPlot.Add(oGTNamedPlot)
          End If
        End If
      Next oListItem

      bNamedPlotRemoved = False
      For Each oGTNamedPlot In colGTNamedPlot
        oGTApplication.NamedPlots.Remove(oGTNamedPlot.Name)
        bNamedPlotRemoved = True
      Next oGTNamedPlot

      If bNamedPlotRemoved Then PopulateList()

    Catch ex As Exception
      MsgBox("WorkspacePlotsForm.cmdDelete_Click:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    Finally
      'oListItem = Nothing
      'oGTNamedPlot = Nothing
      'colGTNamedPlot = Nothing
      'oGTPlotWindow = Nothing
      'oGTApplication = Nothing
      'GC.Collect()
    End Try

  End Sub

  Private Sub ChangeSelectedNamedPlotWindowState(ByVal GTWindowState As GTWindowStateConstants)

    Dim oListItem As ListViewItem
    Dim oGTPlotWindow As IGTPlotWindow
    Dim oGTPlotWindows As IGTPlotWindows
    Dim oGTNamedPlot As IGTNamedPlot
    Dim oGTNamedPlotOpen As IGTNamedPlot
    Dim oGTApplication As IGTApplication

    Try
      oGTApplication = GTClassFactory.Create(Of IGTApplication)()
      oGTPlotWindows = GTClassFactory.Create(Of IGTPlotWindows)()

      oGTPlotWindows = oGTApplication.GetPlotWindows

      For Each oGTPlotWindow In oGTPlotWindows
        For Each oListItem In lvNamedPlots.Items
          If oListItem.Selected Then
            oGTNamedPlot = oGTApplication.NamedPlots(oListItem.Text)
            oGTNamedPlotOpen = oGTPlotWindow.NamedPlot
            If oGTNamedPlot.Name = oGTNamedPlotOpen.Name Then
              oGTPlotWindow.WindowState = GTWindowState
            End If
          End If
        Next oListItem
      Next oGTPlotWindow


    Catch ex As Exception
      MsgBox("WorkspacePlotsForm.ChangeSelectedNamedPlotWindowState:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    Finally
      'oListItem = Nothing
      'oGTPlotWindow = Nothing
      'oGTPlotWindows = Nothing
      'oGTNamedPlot = Nothing
      'oGTNamedPlotOpen = Nothing

    End Try

  End Sub

  Private Sub cmdMaximize_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdMaximize.Click
    ChangeSelectedNamedPlotWindowState(GTWindowStateConstants.gtmwwsMaximize)
  End Sub

  Private Sub cmdMinimize_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdMinimize.Click
    ChangeSelectedNamedPlotWindowState(GTWindowStateConstants.gtmwwsMinimize)
  End Sub

  Private Sub cmdRestore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRestore.Click
    ChangeSelectedNamedPlotWindowState(GTWindowStateConstants.gtmwwsRestore)
  End Sub

  Private Sub cmdOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOpen.Click
    OpenSelectedNamedPlot()
  End Sub

  Private Sub lvNamedPlots_AfterLabelEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.LabelEditEventArgs) Handles lvNamedPlots.AfterLabelEdit

    Dim strNewPlotName As String = ""
    Dim oListItem As ListViewItem
    Dim oGTNamedPlot As IGTNamedPlot
    Dim oGTNamedPlot2 As IGTNamedPlot = Nothing
    Dim oGTPlotWindow As IGTPlotWindow = Nothing
    Dim colGTNamedPlot As New Collection
    Dim oGTApplication As IGTApplication

    Try
      oListItem = lvNamedPlots.SelectedItems.Item(0) ' Only one item can be selected to rename
      oGTApplication = GTClassFactory.Create(Of IGTApplication)()
      oGTNamedPlot = oGTApplication.NamedPlots(oListItem.Text)

      If e.Label = Nothing Or Trim(e.Label) = "" Then ' No change made or NamedPlot can not be blank, so skip
        e.CancelEdit = True
      Else
        strNewPlotName = e.Label
        If DoesNamedPlotExist(strNewPlotName, oGTNamedPlot2) Then
          ' "That name already exists, please try another.", "Rename"
          MessageBox.Show(My.Resources.WorkspacePlots.lvNamedPlots_AfterLabelEdit_MessageBox_Text, My.Resources.WorkspacePlots.lvNamedPlots_AfterLabelEdit_MessageBox_Caption, MessageBoxButtons.OK)
          strNewPlotName = strBeforeLabelEditValue
          Exit Sub
        End If

        If IsNamedPlotOpen(oListItem.Text, oGTPlotWindow) Then oGTPlotWindow.Caption = strNewPlotName
        oGTNamedPlot.Name = strNewPlotName
      End If

    Catch ex As Exception
      MsgBox("WorkspacePlotsForm.lvNamedPlots_AfterLabelEdit:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      '  If Err.Number = -2147024809 Then ' Named plot exists
      '    strNewPlotName = strBeforeLabelEditValue
      '  End If
    Finally
      'oListItem = Nothing
      'oGTNamedPlot = Nothing
      'oGTNamedPlot2 = Nothing
      'oGTPlotWindow = Nothing
      colGTNamedPlot = Nothing
    End Try

  End Sub

  Private Sub lvNamedPlots_BeforeLabelEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.LabelEditEventArgs) Handles lvNamedPlots.BeforeLabelEdit

    Dim oListItem As ListViewItem
    Dim colGTNamedPlot As New Collection

    Try

      For Each oListItem In lvNamedPlots.Items
        If oListItem.Selected Then
          strBeforeLabelEditValue = oListItem.Text
          Exit For
        End If
      Next oListItem

    Catch ex As Exception
      MsgBox("WorkspacePlotsForm.lvNamedPlots_BeforeLabelEdit:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      '  If Err.Number = -2147024809 Then ' Named plot exists
      '    strNewPlotName = strBeforeLabelEditValue
      '  End If
    Finally
      'oListItem = Nothing
      colGTNamedPlot = Nothing
    End Try

  End Sub

  Private Sub lvNamedPlots_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvNamedPlots.Click
    UpdateButtonStates()
  End Sub


  Private Sub UpdateButtonStates()

    Dim I As Integer
    Dim oListItem As ListViewItem

    Try

      If lvNamedPlots.Items.Count > 0 Then
        cmdSelectAll.Enabled = True
        cmdUnselectAll.Enabled = True
      Else
        cmdSelectAll.Enabled = False
        cmdUnselectAll.Enabled = False
      End If

      ' Get the number of items selected
      I = 0
      For Each oListItem In lvNamedPlots.Items
        If oListItem.Selected Then I = I + 1
      Next oListItem

      Select Case I
        Case 0
          cmdActivate.Enabled = False
          cmdClose.Enabled = True
          cmdClosePlot.Enabled = False
          cmdCopy.Enabled = False
          cmdDelete.Enabled = False
          cmdMaximize.Enabled = False
          cmdMinimize.Enabled = False
          cmdNew.Enabled = True
          cmdOpen.Enabled = False
          cmdPrint.Enabled = False
          cmdRefresh.Enabled = True
          cmdRename.Enabled = False
          cmdRestore.Enabled = False
        Case 1
          cmdActivate.Enabled = True
          cmdClose.Enabled = True
          cmdClosePlot.Enabled = True
          cmdCopy.Enabled = True
          cmdDelete.Enabled = True
          cmdMaximize.Enabled = True
          cmdMinimize.Enabled = True
          cmdNew.Enabled = True
          cmdOpen.Enabled = True
          cmdPrint.Enabled = True
          cmdRefresh.Enabled = True
          cmdRename.Enabled = True
          cmdRestore.Enabled = True
        Case Else
          cmdActivate.Enabled = False
          cmdClose.Enabled = True
          cmdClosePlot.Enabled = True
          cmdCopy.Enabled = False ' Only COPY one at a time
          cmdDelete.Enabled = True
          cmdMaximize.Enabled = True
          cmdMinimize.Enabled = True
          cmdNew.Enabled = True
          cmdOpen.Enabled = True
          cmdPrint.Enabled = True
          cmdRefresh.Enabled = True
          cmdRename.Enabled = False ' Only RENAME one at a time
          cmdRestore.Enabled = True
      End Select


    Catch ex As Exception
      MsgBox("WorkspacePlotsForm.UpdateButtonStates:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    Finally
      'oListItem = Nothing
    End Try

  End Sub

  Private Sub OpenSelectedNamedPlot()

    Dim oListItem As ListViewItem
    Dim oGTNamedPlot As IGTNamedPlot
    Dim oGTPlotWindow As IGTPlotWindow = Nothing
    Dim oGTPlotWindows As IGTPlotWindows
    Dim oGTApplication As IGTApplication

    Try
      oGTApplication = GTClassFactory.Create(Of IGTApplication)()
      oGTPlotWindows = GTClassFactory.Create(Of IGTPlotWindows)()

      For Each oListItem In lvNamedPlots.Items
        If oListItem.Selected Then

          oGTNamedPlot = oGTApplication.NamedPlots(oListItem.Text)
          If IsNamedPlotOpen(oListItem.text, oGTPlotWindow) Then
            oGTPlotWindow.Activate()
          Else
            oGTPlotWindow = oGTApplication.NewPlotWindow(oGTNamedPlot)
            oGTPlotWindow.Activate()


            '
            ' Begin -Repair polygon borders masking the map windows by converting them to polylines.
            '
            Dim oGTPlotRedline As IGTPlotRedline
            Dim oGTPlotRedlines As IGTPlotRedlines
            Dim oGTPolygonGeometry As IGTPolygonGeometry
            Dim oGTPoint1 As IGTPoint
            Dim oGTPoint2 As IGTPoint
            Dim oGTPoint3 As IGTPoint
            Dim oGTPoint4 As IGTPoint

            oGTPlotRedlines = oGTPlotWindow.NamedPlot.GetRedlines(GTPlotRedlineCollectionConstants.gtprcAreasOnly)

            For Each oGTPlotRedline In oGTPlotRedlines
              ' If area
              If oGTPlotRedline.Type = GTPlotRedlineTypeConstants.gtprtArea Then
                oGTPolygonGeometry = oGTPlotRedline.Geometry

                ' If 4 point
                If oGTPolygonGeometry.Points.Count = 4 Then

                  oGTPoint1 = oGTPolygonGeometry.Points.Item(0)
                  oGTPoint2 = oGTPolygonGeometry.Points.Item(1)
                  oGTPoint3 = oGTPolygonGeometry.Points.Item(2)
                  oGTPoint4 = oGTPolygonGeometry.Points.Item(3)

                  ' Test for horizontal and vertical lines drawn in the direction of the old area border
                  If oGTPoint1.X = oGTPoint4.X _
                     And oGTPoint1.Y = oGTPoint2.Y Then

                    ' Verify that Polygon Border Height and Width = paper size - ((border inset of 17 * 2) * 100)
                    If oGTPoint2.X - oGTPoint1.X = (oGTNamedPlot.PaperWidth - ((17 * 2) * 100)) _
                       And oGTPoint4.Y - oGTPoint1.Y = (oGTNamedPlot.PaperHeight - (17 * 2) * 100) Then

                      ' Prompt user to replace polygon border with polyline border. (DEBUG MODE ONLY!)
                      Dim oVbMsgBoxResult As MsgBoxResult
                      oVbMsgBoxResult = vbOK
#If DEBUG Then
                      oVbMsgBoxResult = MsgBox("Located Polygon Border!" + vbCr + _
                                                     "Would you like it REPAIRED with a polyline border that doesn't mask MapFrames?", _
                                                    vbOKCancel, "Locate Polygon Border")
#End If
                      If oVbMsgBoxResult = vbOK Then

                        Dim dStyleID As Double
                        Dim oGTPoint As IGTPoint
                        Dim oGTPolylineGeometry As IGTPolylineGeometry

                        dStyleID = oGTPlotRedline.StyleID

                        oGTPolylineGeometry = GTClassFactory.Create(Of IGTPolylineGeometry)()

                        oGTPoint = GTClassFactory.Create(Of IGTPoint)()
                        oGTPolylineGeometry.Points.Add(oGTPoint1)
                        oGTPolylineGeometry.Points.Add(oGTPoint2)
                        oGTPolylineGeometry.Points.Add(oGTPoint3)
                        oGTPolylineGeometry.Points.Add(oGTPoint4)
                        oGTPolylineGeometry.Points.Add(oGTPoint1)

                        ' Remove old polygon geometry
                        oGTPlotRedline.Delete()

                        ' Replace with new polyline geometry
                        oGTPlotRedline = oGTNamedPlot.NewRedline(oGTPolylineGeometry, dStyleID)
                      End If
                    End If
                  End If
                End If
              End If
            Next oGTPlotRedline
            '
            ' End -Repair polygon borders masking the map windows by converting them to polylines.
            '




          End If

        End If
      Next oListItem
      'oGTApplication.RefreshWindows()

      RefreshListItemStatus()

    Catch ex As Exception
      MsgBox("WorkspacePlotsForm.OpenSelectedNamedPlot:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    Finally
      'oListItem = Nothing
      'oGTNamedPlot = Nothing
      'oGTPlotWindow = Nothing
      'oGTPlotWindows = Nothing
    End Try

  End Sub


  Private Sub lvNamedPlots_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvNamedPlots.DoubleClick
    OpenSelectedNamedPlot()
  End Sub

  Private Sub cmdPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPrint.Click

    Dim oListItem As ListViewItem
    Dim oGTNamedPlot As IGTNamedPlot
    Dim oGTPlotWindow As IGTPlotWindow = Nothing
    Dim oPrintProperties As IGTPrintProperties
    Dim iCopies As Integer
    Dim oGTApplication As IGTApplication

    Try
      ' Present the user with the Print Dialog
      If Not PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then Exit Sub

      oGTApplication = GTClassFactory.Create(Of IGTApplication)()
      'oPrintProperties = oGTApplication.CreateService(GTServiceConstants.gtsvcPrintProperties))
      oPrintProperties = GTClassFactory.Create(Of IGTPrintProperties)()

      If PrintDialog1.PrinterSettings.Collate Then
        iCopies = PrintDialog1.PrinterSettings.Copies
        PrintDialog1.PrinterSettings.Copies = 1
        oPrintProperties.Copies = PrintDialog1.PrinterSettings.Copies
        For i As Integer = 1 To iCopies
          For Each oListItem In lvNamedPlots.Items
            If oListItem.Selected Then
              oGTNamedPlot = oGTApplication.NamedPlots(oListItem.Text)
              PrintNamedPlot(oGTNamedPlot, oPrintProperties, PrintDialog1)
            End If
          Next oListItem
        Next i
      Else
        oPrintProperties.Copies = PrintDialog1.PrinterSettings.Copies
        For Each oListItem In lvNamedPlots.Items
          If oListItem.Selected Then
            oGTNamedPlot = oGTApplication.NamedPlots(oListItem.Text)
            PrintNamedPlot(oGTNamedPlot, oPrintProperties, PrintDialog1)
          End If
        Next oListItem
      End If


    Catch ex As Exception
      MsgBox("WorkspacePlotsForm.cmdPrint_Click:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    Finally
      'oListItem = Nothing
      'oGTNamedPlot = Nothing
      'oGTPlotWindow = Nothing
      'oPrintProperties = Nothing
    End Try

  End Sub

  Private Sub cmdSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSelectAll.Click

    Dim oListItem As ListViewItem

    Try
      For Each oListItem In lvNamedPlots.Items
        If Not oListItem.Selected Then oListItem.Selected = True
      Next oListItem
      lvNamedPlots.Focus()
      UpdateButtonStates()

    Catch ex As Exception
      MsgBox("WorkspacePlotsForm.cmdSelectAll_Click:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    Finally
      'oListItem = Nothing
    End Try

  End Sub

  Private Sub cmdUnselectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUnselectAll.Click

    Dim oListItem As ListViewItem

    Try
      For Each oListItem In lvNamedPlots.Items
        If oListItem.Selected Then oListItem.Selected = False
      Next oListItem
      lvNamedPlots.Focus()
      UpdateButtonStates()

    Catch ex As Exception
      MsgBox("WorkspacePlotsForm.cmdUnselectAll_Click:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    Finally
      'oListItem = Nothing
    End Try

  End Sub

  Private Sub lvNamedPlots_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lvNamedPlots.KeyUp
    Try
      If e.KeyCode = Keys.Escape Then
        m_bNamedPlots_Escape_KeyUp = True
        'e.SuppressKeyPress = True
        'e.Handled = True
      End If
    Catch ex As Exception
      MsgBox("WorkspacePlotsForm.lvNamedPlots_KeyUp:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    Finally
    End Try
  End Sub

  Private Sub lvNamedPlots_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvNamedPlots.SelectedIndexChanged
    UpdateButtonStates()
  End Sub

  Private Sub ButtonBlank3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonBlank3.Click

    Dim oGTNamedPlot As IGTNamedPlot
    Dim oGTPlotWindow As IGTPlotWindow
    Dim oGTApplication As IGTApplication

    Try
      oGTPlotWindow = GTClassFactory.Create(Of IGTPlotWindow)()
      oGTApplication = GTClassFactory.Create(Of IGTApplication)()
      oGTNamedPlot = GTClassFactory.Create(Of IGTNamedPlot)()

      oGTNamedPlot = oGTApplication.NamedPlots("NewPlot")
      oGTPlotWindow = oGTApplication.NewPlotWindow(oGTNamedPlot)

      oGTApplication.RefreshWindows()

    Catch ex As Exception
      MsgBox("WorkspacePlotsForm.ButtonBlank3_Click:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    Finally
    End Try

  End Sub

  Private Sub WorkspacePlotsForm_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
    Me.lvNamedPlots.Columns(0).Width = lvNamedPlots.Width - (60 * 4) - 4
  End Sub



End Class
