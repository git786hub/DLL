Imports System
Imports Microsoft.Win32
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports Intergraph.GTechnology.API
Imports System.Drawing

Public Class NewPlotWindowForm

  ' Setup dialog labels based on metadata
  Dim oJobDefinition As New GTJobDefinition

  Private m_colPlotBoundaries As Collection
  Private m_colAttributes As Collection

  Enum MeasurmentUnits
    Metric
    Imperial
  End Enum

  ' Save settings
  Private Sub Save()
    Try
      Dim sUserSubKeyCulture = My.Resources.RegistryCurrentUserSubKey & "\\" & GetCurrentCulture()
      Dim RegKey As RegistryKey = Registry.CurrentUser.OpenSubKey(sUserSubKeyCulture, True)
      If IsNothing(RegKey) Then RegKey = Registry.CurrentUser.CreateSubKey(sUserSubKeyCulture, RegistryKeyPermissionCheck.ReadWriteSubTree)
      RegKey.SetValue(Me.Name & ".Top", Me.Top)
      RegKey.SetValue(Me.Name & ".Left", Me.Left)
      RegKey.SetValue(Me.Name & ".optPaperOrientationPortrait.Checked", Me.optPaperOrientationPortrait.Checked)
      RegKey.SetValue(Me.Name & ".optPaperOrientationPortrait.Checked", optPaperOrientationPortrait.Checked)
      RegKey.SetValue(Me.Name & ".cbType.Checked", cbType.Checked)
      RegKey.SetValue(Me.Name & ".lbType.Text", lbType.Text)
      RegKey.SetValue(Me.Name & ".lbPaperSize.Text", lbPaperSize.Text)
      RegKey.SetValue(Me.Name & ".optMapScalePreDefined.Checked", optMapScalePreDefined.Checked)
      RegKey.SetValue(Me.Name & ".lbMapScalePreDefined.Text", lbMapScalePreDefined.Text)
      RegKey.SetValue(Me.Name & ".tbMapScaleCustom.Text", tbMapScaleCustom.Text)
      RegKey.SetValue(Me.Name & ".TabControl1.SelectedIndex", TabControl1.SelectedIndex)
      RegKey.SetValue(Me.Name & ".chkInsertActiveMapWindow.Checked", chkInsertActiveMapWindow.Checked)
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

      If Not IsNothing(RegKey.GetValue(Me.Name & ".TabControl1.SelectedIndex")) Then
        TabControl1.SelectedIndex = RegKey.GetValue(Me.Name & ".TabControl1.SelectedIndex")
      End If

      If Not IsNothing(RegKey.GetValue(Me.Name & ".Top")) Then
        Me.Top = RegKey.GetValue(Me.Name & ".Top")
        Me.Left = RegKey.GetValue(Me.Name & ".Left")
      End If

      If Not IsNothing(RegKey.GetValue(Me.Name & ".cbType.Checked")) Then
        cbType.Checked = RegKey.GetValue(Me.Name & ".cbType.Checked")
      End If

      If Not IsNothing(RegKey.GetValue(Me.Name & ".lbType.Text")) Then
        lbType.Text = RegKey.GetValue(Me.Name & ".lbType.Text")
      Else
        lbType.SetSelected(0, True)
      End If

      If Not IsNothing(RegKey.GetValue(Me.Name & ".optPaperOrientationPortrait.Checked")) Then
        If RegKey.GetValue(Me.Name & ".optPaperOrientationPortrait.Checked") Then
          Me.optPaperOrientationPortrait.Checked = True
        Else
          Me.optPaperOrientationLandscape.Checked = True
        End If
      End If

      If Not IsNothing(RegKey.GetValue(Me.Name & ".lbPaperSize.Text")) Then
        lbPaperSize.Text = RegKey.GetValue(Me.Name & ".lbPaperSize.Text")
      Else
        If lbPaperSize.Items.Count > 0 Then
          lbPaperSize.SetSelected(0, True)
        End If
      End If

      If Not IsNothing(RegKey.GetValue(Me.Name & ".optMapScalePreDefined.Checked")) Then
        If RegKey.GetValue(Me.Name & ".optMapScalePreDefined.Checked") Then
          Me.optMapScalePreDefined.Checked = True
        Else
          Me.optMapScaleCustom.Checked = True
        End If
      End If

      If Not IsNothing(RegKey.GetValue(Me.Name & ".lbMapScalePreDefined.Text")) Then
        lbMapScalePreDefined.Text = RegKey.GetValue(Me.Name & ".lbMapScalePreDefined.Text")
      Else
        lbMapScalePreDefined.SetSelected(0, True)
      End If

      If Not IsNothing(RegKey.GetValue(Me.Name & ".tbMapScaleCustom.Text")) Then
        tbMapScaleCustom.Text = RegKey.GetValue(Me.Name & ".tbMapScaleCustom.Text")
      Else
        If GTPlotMetadata.Parameters.MeasurementUnits = "Metric" Then
          tbMapScaleCustom.Text = "1:750"
        Else
          tbMapScaleCustom.Text = "1""=20'"
        End If
      End If

      If Not IsNothing(RegKey.GetValue(Me.Name & ".chkInsertActiveMapWindow.Checked")) Then
        chkInsertActiveMapWindow.Checked = RegKey.GetValue(Me.Name & ".chkInsertActiveMapWindow.Checked")
      Else
        chkInsertActiveMapWindow.Checked = CheckState.Checked
      End If

      RegKey.Close()

    Catch ex As Exception
    End Try
  End Sub


  Private Sub cmdCreatePlotWindows_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreatePlotWindows.Click

    Dim oTreeNode As TreeNode
    Dim oPlotBoundary As PlotBoundary
    Dim sStatusBarText As String = ""
    Dim oGTApplication As IGTApplication = Nothing
    Dim oGTPlotWindow As IGTPlotWindow

    Try
      oGTApplication = GTClassFactory.Create(Of IGTApplication)()
      sStatusBarText = oGTApplication.GetStatusBarText(GTStatusPanelConstants.gtaspcMessage)

      oGTApplication.BeginWaitCursor()

      'Save the user entered data to the registry
      Save()

      If TabControl1.SelectedIndex = 0 Then ' If Click from the Plot Boundary tab

        Hide()

        '
        ' Loop through each selected plan ID
        '
        For Each oTreeNode In tvSelectedPlotBoundaries.Nodes

          ' Validate if a named plot with the same name exists
          If Not DoesNamedPlotExist(oTreeNode.Text) Then

            ' User feedback
            oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Creating Plot: " & oTreeNode.Text)

            ' Call UpdatePlotBoundaryInfo just incase the user never selected it causing default values to get set.
            UpdatePlotBoundaryInfo(oTreeNode.Name)

            ' Set some additional Plot Boundary properties
            oPlotBoundary = m_colPlotBoundaries(oTreeNode.Name)
            With oPlotBoundary
              If oPlotBoundary.Legend = "" Then
                oPlotBoundary.Legend = GetLegendName(GetTypesDefaultLegend(oPlotBoundary.Type, oPlotBoundary.PaperSize, oPlotBoundary.PaperOrientation, IIf(oPlotBoundary.DetailID > 0, True, False)))
              End If
              .InsertActiveMapWindow = True

              GTPlotMetadata.GetSheetSize(.Type, .PaperName, .PaperOrientation, .SheetID, .SheetHeigh, .SheetWidth, .DRI_ID, .SheetInset, .SheetStyleNo)
              .SheetHeigh = .SheetHeigh * dSCALE
              .SheetWidth = .SheetWidth * dSCALE
              .SheetInset = .SheetInset * dSCALE
            End With

            ' Create a new plot window named the same as the PlanID
            oGTPlotWindow = CreateNewNamedPlot(oPlotBoundary)

            ' Create the redline template for the new plot window and insert the map frame(s)
            Call StartDrawingRedlines(oPlotBoundary)

          End If

        Next oTreeNode

      Else ' user defined plot

        ' Create a new PlotBoundary object.
        oPlotBoundary = New PlotBoundary

        ' Retrieve the user entered values.
        Dim sPlotNameUserName As String = ""

        ' Get attributes and store user entered plot name
        m_colAttributes.Clear()
        For Each oAttribute As Attribute In DataGridViewAttributes2.Attributes
          m_colAttributes.Add(oAttribute)
          If oAttribute.G3E_FIELD.ToUpper = GTPlotMetadata.Parameters.PlotBoundaryAttribute_Name.ToUpper Then
            oPlotBoundary.Name = oAttribute.VALUE
            sPlotNameUserName = oAttribute.G3E_USERNAME
          End If
        Next oAttribute

        ' Store the collection of attributes and values for later use
        oPlotBoundary.Attributes = m_colAttributes

        ' Verify that the plot name has been defined.
        If oPlotBoundary.Name = "" Then
          Beep()
          MsgBox(String.Format(My.Resources.NewPlotWindow.cmdCreatePlotWindows_Click_PleaseProvidePlotName, sPlotNameUserName), vbOKOnly, My.Resources.NewPlotWindow.cmdCreatePlotWindows_Click_PlotNameTitle)
          Exit Sub
        End If

        ' Verify that the a named plot with the same name doesn't already exist.
        If GTPlotWindow.DoesNamedPlotExist(oPlotBoundary.Name) Then
          Beep()
          MsgBox(String.Format(My.Resources.NewPlotWindow.cmdCreatePlotWindows_Click_NamedPlotAlreadyExists, oPlotBoundary.Name, sPlotNameUserName), vbOKOnly, My.Resources.NewPlotWindow.cmdCreatePlotWindows_Click_NamedPlotAlreadyExistsTitle)
          Exit Sub
        End If

        ' Setup the PlotBoundary object.
        With oPlotBoundary
          .UserDefined = True
          If cbType.Checked Then
            .Type = lbType.Text
          Else
            .Type = ""
          End If
          .PaperName = FormatStringGetSheetName(lbPaperSize.Text)
          .PaperSize = FormatStringGetSheetSize(lbPaperSize.Text)
          .PaperOrientation = IIf(optPaperOrientationPortrait.Checked, optPaperOrientationPortrait.Text, optPaperOrientationLandscape.Text)
          .InsertActiveMapWindow = chkInsertActiveMapWindow.Checked
          If optMapScalePreDefined.Checked Then
            .MapScale = lbMapScalePreDefined.Text
          Else
            .MapScale = tbMapScaleCustom.Text
          End If

          If .InsertActiveMapWindow Then
            If oGTApplication.ActiveMapWindow Is Nothing Then
              ' Selected to Insert Active Map Window but a Map Window is not active.
              MsgBox(My.Resources.NewPlotWindow.cmdCreatePlotWindows_Click_MapWindowNotActive)
              Exit Sub
            End If
            .ActiveMapWindow_LegendName = oGTApplication.ActiveMapWindow.LegendName
            .ActiveMapWindow_DetailID = oGTApplication.ActiveMapWindow.DetailID
            .ActiveMapWindow_Range = oGTApplication.ActiveMapWindow.GetRange

          End If

          GTPlotMetadata.GetSheetSize(.Type, .PaperName, .PaperOrientation, .SheetID, .SheetHeigh, .SheetWidth, .DRI_ID, .SheetInset, .SheetStyleNo)
          .SheetHeigh = .SheetHeigh * dSCALE
          .SheetWidth = .SheetWidth * dSCALE
          .SheetInset = .SheetInset * dSCALE
        End With

        ' Hide the form while creating the plot
        Hide()

        ' Create a new plot window named the same as the PlanID
        oGTPlotWindow = CreateNewNamedPlot(oPlotBoundary)

        ' Create the redline template for the new plot window and insert the map frame(s)
        Call StartDrawingRedlines(oPlotBoundary)

      End If

      Dispose()

    Catch ex As Exception
      MsgBox("NewPlotWindowForm.cmdCreatePlotWindows:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)

    Finally
      oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, sStatusBarText)
      oGTApplication.EndWaitCursor()

    End Try

  End Sub

  Private Sub NewPlotWindowForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Dim rsJobData As ADODB.Recordset = Nothing
    Dim strSql As String
    Dim rsUserName As ADODB.Recordset = Nothing
    Dim rsUserNames As ADODB.Recordset = Nothing
    Dim bUserFound As Boolean = False
    Dim bDatabaseUserFound As Boolean = False
    Dim strActiveJobOwner As String = ""

    Dim oGTApplication As IGTApplication=nothing
    Dim sStatusBarText As String = ""

    Try

#If DEBUG Then
      ' Used to enable or display additional objects while in debug mode.
      Test.Enabled = True
      Test.Visible = True
      Test.Text = "Test"

      'Dim s As String = GTLocalization.GetCurrentCulture()
      'GTLocalization.SetCurrentCulture("en-US")
      'GTLocalization.SetCurrentCulture("en-CA")
      'GTLocalization.SetCurrentCulture("fr-CA")

      lblPath.Visible = True
      lblPath.Text = "DirectoryPath = " & My.Application.Info.DirectoryPath

#End If

      lblVersion.Text = "Version " & My.Application.Info.Version.ToString()

      oGTApplication = GTClassFactory.Create(Of IGTApplication)()

      ' Save Status Bar Text and Begin Wait Cursor
      sStatusBarText = oGTApplication.GetStatusBarText(GTStatusPanelConstants.gtaspcMessage)
      oGTApplication.BeginWaitCursor()


      ' User feedback
      oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, String.Format(My.Resources.NewPlotWindow.Load_SetStatusBarText_Launching, Me.Text))


      '
      ' Clear the DataGridViewAttributes1
      '
      DataGridViewAttributes1.Attributes = New Attributes
      DataGridViewAttributes1.RefreshDataGridViewAttributes()


      '
      ' Active Session Information
      '
      oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, String.Format(My.Resources.NewPlotWindow.Load_SetStatusBarText_Launching, Me.Text) & " -" & My.Resources.NewPlotWindow.Load_SetStatusBarText_GetActiveSessionInfo & "...")

      lblPlotBoundaryFilterUser.Text = oJobDefinition.GetJobAttributeUsernameFromField(oJobDefinition.G3E_OWNERFIELD) & ":"
      lblActiveSessionInfoUser.Text = lblPlotBoundaryFilterUser.Text
      lblActiveSessionInfoJobNumber.Text = oJobDefinition.GetJobAttributeUsernameFromField(oJobDefinition.G3E_IDENTIFIERFIELD) & ":"
      lblPlotBoundaryFilterCapitalWorkOrderNumber.Text = oJobDefinition.GetJobAttributeUsernameFromField(GTPlotMetadata.Parameters.PlotBoundaryLinkedJobAttribute_FieldName) & ":"
      lblActiveSessionInfoCapitalWorkOrderNumber.Text = lblPlotBoundaryFilterCapitalWorkOrderNumber.Text


      If oGTApplication.DataContext.ActiveJob = "" Then
        lblActiveSessionInfoUserValue.Text = My.Resources.NewPlotWindow.Load_SetStatusBarText_ActiveJobNotSet
        lblActiveSessionInfoJobNumberValue.Text = My.Resources.NewPlotWindow.Load_SetStatusBarText_ActiveJobNotSet
        lblActiveSessionInfoCapitalWorkOrderNumberValue.Text = My.Resources.NewPlotWindow.Load_SetStatusBarText_ActiveJobNotSet
      Else
        lblActiveSessionInfoJobNumberValue.Text = oGTApplication.DataContext.ActiveJob

        ' Get Active Job's Owner
        oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, String.Format(My.Resources.NewPlotWindow.Load_SetStatusBarText_Launching, Me.Text) & " -" & My.Resources.NewPlotWindow.Load_SetStatusBarText_GetActiveJobOwner & "...")
        strActiveJobOwner = oJobDefinition.GetJobAttributeValueFromField(oGTApplication.DataContext.ActiveJob, oJobDefinition.G3E_OWNERFIELD)
        lblActiveSessionInfoUserValue.Text = strActiveJobOwner


        strSql = " SELECT " & GTPlotMetadata.Parameters.PlotBoundaryLinkedJobAttribute_FieldName & " "
        strSql = strSql & "  FROM " & oJobDefinition.G3E_TABLE & " "
        ' Fixed when oGTApplication.DataContext.ActiveJob could have single quote.  Get an ORA-01756 quote string not properly terminated error.
        strSql = strSql & " WHERE " & oJobDefinition.G3E_IDENTIFIERFIELD & " = '" & Replace(oGTApplication.DataContext.ActiveJob, "'", "''") & "' "
        rsJobData = oGTApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText)
        Call Repos(rsJobData)
        If rsJobData.RecordCount > 0 Then
          If IsDBNull(rsJobData.Fields(GTPlotMetadata.Parameters.PlotBoundaryLinkedJobAttribute_FieldName).Value) Then
            MsgBox(String.Format(My.Resources.NewPlotWindow.Load_SetStatusBarText_UserNotDefined, oJobDefinition.GetJobAttributeUsernameFromField(GTPlotMetadata.Parameters.PlotBoundaryLinkedJobAttribute_FieldName)), vbOKOnly + vbExclamation)
            lblActiveSessionInfoCapitalWorkOrderNumberValue.Text = My.Resources.NewPlotWindow.Load_SetStatusBarText_NotSet
          Else
            'lblActiveSessionInfoCapitalWorkOrderNumberValue.Text = rsJobData.Fields(GTPlotMetadata.Parameters.PlotBoundaryLinkedJobAttribute_FieldName).Value
            lblActiveSessionInfoCapitalWorkOrderNumberValue.Visible = False
            lblActiveSessionInfoCapitalWorkOrderNumber.Visible = False
          End If
        End If
        rsJobData.Close()
      End If



      '
      ' Plot Boundary Filter
      '

      ' Set Default Owner
      If String.IsNullOrEmpty(oGTApplication.AOILocationIdentifier) Then

        If Not String.IsNullOrEmpty(strActiveJobOwner) Then
          ' Add Active Job Owner if one.
          bUserFound = True
          cbPlotBoundaryFilterUser.Items.Add(strActiveJobOwner)

          ' Add Database User Name if not the same as the Active Job Owner.
          If Not strActiveJobOwner = oGTApplication.DataContext.DatabaseUserName Then
            bDatabaseUserFound = True
            cbPlotBoundaryFilterUser.Items.Add(oGTApplication.DataContext.DatabaseUserName)
          End If
        Else
          ' Always at least add the Database User Name
          bDatabaseUserFound = True
          cbPlotBoundaryFilterUser.Items.Add(oGTApplication.DataContext.DatabaseUserName)
        End If

        ' Always add "<AOI Not Set>" as last item.
        cbPlotBoundaryFilterUser.Items.Add("<AOI Not Set>")
      Else
        ' Populate User Picklist with users that have plot boundaries placed within the active area
        oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, String.Format(My.Resources.NewPlotWindow.Load_SetStatusBarText_Launching, Me.Text) & " -" & My.Resources.NewPlotWindow.Load_SetStatusBarText_PopulateUserPicklist & "...")
        strSql = " SELECT DISTINCT (" & oJobDefinition.G3E_OWNERFIELD & ") "
        strSql = strSql & "  FROM " & GTPlotMetadata.Parameters.PlotBoundaryInfoView & " "
        ' TODO -remove hard coding of SWITCH_CENTRE_CLLI, get from metadata.
        strSql = strSql & " WHERE SWITCH_CENTRE_CLLI='" & oGTApplication.AOILocationIdentifier & "' "
        strSql = strSql & "   AND " & oJobDefinition.G3E_OWNERFIELD & " is not null "
        strSql = strSql & " ORDER BY " & oJobDefinition.G3E_OWNERFIELD
        ' Changed LockType to LockReadOnly mode.

        rsUserNames = oGTApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText)
        Call Repos(rsUserNames)
        bUserFound = False
        If rsUserNames.RecordCount > 0 Then
          While Not rsUserNames.EOF
            cbPlotBoundaryFilterUser.Items.Add(rsUserNames.Fields(oJobDefinition.G3E_OWNERFIELD).Value)
            If rsUserNames.Fields(oJobDefinition.G3E_OWNERFIELD).Value = strActiveJobOwner Then
              bUserFound = True
            End If
            If rsUserNames.Fields(oJobDefinition.G3E_OWNERFIELD).Value = oGTApplication.DataContext.DatabaseUserName Then
              bDatabaseUserFound = True
            End If
            rsUserNames.MoveNext()
          End While
        End If
        rsUserNames.Close()
      End If

      ' Set the default user
      ' PA -version 1.7.0.6, Fixed to properly set default Job User, Active User or First list item.
      oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, String.Format(My.Resources.NewPlotWindow.Load_SetStatusBarText_Launching, Me.Text) & " -" & My.Resources.NewPlotWindow.Load_SetStatusBarText_PopulateUsersJobs & "...")
      If bUserFound Then ' Set to active job owner if found
        cbPlotBoundaryFilterUser.Text = strActiveJobOwner
      Else
        If bDatabaseUserFound Then ' Set to active user if found
          cbPlotBoundaryFilterUser.Text = oGTApplication.DataContext.DatabaseUserName
        Else ' otherwise set to 1st item in list
          If cbPlotBoundaryFilterUser.Items.Count > 0 Then 'Fixed SR# 1-468455816 -When no plot boundaries are placed in a given AOI
            cbPlotBoundaryFilterUser.Text = cbPlotBoundaryFilterUser.Items.Item(0)
          End If
        End If
      End If

      ' Call function to Move selectset plans to selected list FUTURE
      'MoveSelectSetPlanID() 

      ' User Defined Plot
      oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, String.Format(My.Resources.NewPlotWindow.Load_SetStatusBarText_Launching, Me.Text) & " -" & My.Resources.NewPlotWindow.Load_SetStatusBarText_LoadingUserDefinedPlotDefaults & "...")
      LoadDefaults()

      ' Update button states
      oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, String.Format(My.Resources.NewPlotWindow.Load_SetStatusBarText_Launching, Me.Text) & " -" & My.Resources.NewPlotWindow.Load_SetStatusBarText_UpdateButtonStates & "...")
      UpdateButtonsStates()

    Catch ex As Exception
      MsgBox("NewPlotWindowForm.Load:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      Dispose()

    Finally

      ' Clear Status Bar Text Message and End Wait Cursor
      oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, sStatusBarText)
      oGTApplication.EndWaitCursor()

      If Not (rsJobData Is Nothing) Then
        rsJobData = Nothing
      End If

    End Try

  End Sub


  Public Sub PopulateCapitalWorkOrderNumberPicklist()

    Dim strSql As String
    Dim rsCapitalWorkOrderNumbers As ADODB.Recordset
    Dim bCapitalWorkOrderNumberFound As Boolean
    Dim bActiveCapitalWorkOrderNumberFound As Boolean
    Dim oGTApplication As IGTApplication
    Dim sStatusBarText As String = ""

    oGTApplication = GTClassFactory.Create(Of IGTApplication)()

    sStatusBarText = oGTApplication.GetStatusBarText(GTStatusPanelConstants.gtaspcMessage)
    oGTApplication.BeginWaitCursor()

    oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, My.Resources.NewPlotWindow.Load_SetStatusBarText_PopulateUsersJobs & "...")

    tvAvailablePlotBoundaries.Nodes.Clear()
    UpdateButtonsStates()
    cbPlotBoundaryFilterCapitalWorkOrderNumber.Items.Clear()

    ' Populate CapitalWorkOrderNumber Picklist
    bCapitalWorkOrderNumberFound = False

    strSql = " SELECT DISTINCT (" & GTPlotMetadata.Parameters.PlotBoundaryLinkedJobAttribute_FieldName & ") "
    strSql = strSql & "           FROM " & GTPlotMetadata.Parameters.PlotBoundaryInfoView & " "

    If String.IsNullOrEmpty(oGTApplication.AOILocationIdentifier) Then
      strSql = strSql & "          WHERE " & oJobDefinition.G3E_OWNERFIELD & " = '" & cbPlotBoundaryFilterUser.Text & "' "
    Else
      ' TODO -remove hard coding of SWITCH_CENTRE_CLLI, get from metadata.
      strSql = strSql & " WHERE SWITCH_CENTRE_CLLI='" & oGTApplication.AOILocationIdentifier & "' "
      strSql = strSql & "       AND " & oJobDefinition.G3E_OWNERFIELD & " = '" & cbPlotBoundaryFilterUser.Text & "' "
    End If


    rsCapitalWorkOrderNumbers = oGTApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, ADODB.CommandTypeEnum.adCmdText)
    Call Repos(rsCapitalWorkOrderNumbers)

    If rsCapitalWorkOrderNumbers.RecordCount > 0 Then
      While Not rsCapitalWorkOrderNumbers.EOF
        cbPlotBoundaryFilterCapitalWorkOrderNumber.Items.Add(rsCapitalWorkOrderNumbers.Fields(GTPlotMetadata.Parameters.PlotBoundaryLinkedJobAttribute_FieldName).Value)
        bCapitalWorkOrderNumberFound = True
        If rsCapitalWorkOrderNumbers.Fields(GTPlotMetadata.Parameters.PlotBoundaryLinkedJobAttribute_FieldName).Value = lblActiveSessionInfoCapitalWorkOrderNumberValue.Text Then
          bActiveCapitalWorkOrderNumberFound = True
        End If
        rsCapitalWorkOrderNumbers.MoveNext()
      End While
    End If
    rsCapitalWorkOrderNumbers.Close()

    ' Set the default CapitalWorkOrderNumber value
    If bActiveCapitalWorkOrderNumberFound Then
      cbPlotBoundaryFilterCapitalWorkOrderNumber.Text = lblActiveSessionInfoCapitalWorkOrderNumberValue.Text
    Else
      If bCapitalWorkOrderNumberFound Then
        cbPlotBoundaryFilterCapitalWorkOrderNumber.Text = cbPlotBoundaryFilterCapitalWorkOrderNumber.Items.Item(0)
      Else
        cbPlotBoundaryFilterCapitalWorkOrderNumber.Text = ""
      End If
    End If

    ' Clear Status Bar Text Message and End Wait Cursor
    oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, sStatusBarText)
    oGTApplication.EndWaitCursor()


  End Sub

  Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
    Dispose()
  End Sub

  Private Sub cbPlotBoundaryFilterCapitalWorkOrderNumber_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbPlotBoundaryFilterCapitalWorkOrderNumber.SelectedIndexChanged
    cmdRemoveAll_Click(sender, e)
    UpdateAvailablePlotBoundaries()
  End Sub

  Private Sub UpdateAvailablePlotBoundaries()

    Dim strSql As String
    Dim sKey As String
    Dim rsPlotBoundaries As ADODB.Recordset = Nothing
    Dim oTreeNode As TreeNode
    Dim oPlotBoundary As PlotBoundary
    Dim oGTApplication As IGTApplication
    Dim sStatusBarText As String

    oGTApplication = GTClassFactory.Create(Of IGTApplication)()
    sStatusBarText = oGTApplication.GetStatusBarText(GTStatusPanelConstants.gtaspcMessage)
    oGTApplication.BeginWaitCursor()

    Try
      oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, My.Resources.NewPlotWindow.SetStatusBarText_UpdateAvailablePlotBoundaries & "...")

      m_colPlotBoundaries = New Collection
      tvAvailablePlotBoundaries.Nodes.Clear()
      If cbPlotBoundaryFilterCapitalWorkOrderNumber.Text = "" Then Exit Sub

      strSql = " SELECT * "
      strSql = strSql & "    FROM " & GTPlotMetadata.Parameters.PlotBoundaryInfoView & " "
            strSql = strSql & "   WHERE DESIGNER_UID = '" & cbPlotBoundaryFilterUser.Text & "' "
            strSql = strSql & "     AND " & GTPlotMetadata.Parameters.PlotBoundaryLinkedJobAttribute_FieldName & " = '" & cbPlotBoundaryFilterCapitalWorkOrderNumber.Text & "' "
      strSql = strSql & "ORDER BY " & GTPlotMetadata.Parameters.PlotBoundaryAttribute_Name

      rsPlotBoundaries = oGTApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText)

      Call Repos(rsPlotBoundaries)
      If rsPlotBoundaries.RecordCount > 0 Then

        ' Set the imagelist to be used to draw the icons left of the plot name
        tvAvailablePlotBoundaries.ImageList = ImageList2
        tvSelectedPlotBoundaries.ImageList = ImageList2


        While Not rsPlotBoundaries.EOF

          ' Store Plot Boundary info for later use
          oPlotBoundary = New PlotBoundary
          With oPlotBoundary

            .Name = rsPlotBoundaries.Fields(GTPlotMetadata.Parameters.PlotBoundaryAttribute_Name).Value

            oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, My.Resources.NewPlotWindow.SetStatusBarText_UpdateAvailablePlotBoundaries & " -" &
                                                                    String.Format(My.Resources.NewPlotWindow.SetStatusBarText_UpdateAvailablePlotBoundaries_Adding, .Name) & "...")

            .FNO = rsPlotBoundaries.Fields("G3E_FNO").Value
            .FID = rsPlotBoundaries.Fields("G3E_FID").Value
            .Job = cbPlotBoundaryFilterCapitalWorkOrderNumber.Text

            ' Check if detail id field exists
            Dim bDetailId As Boolean = False
            Dim Field As ADODB.Field
            For Each Field In rsPlotBoundaries.Fields
              If Field.Name = "G3E_DETAILID" Then
                bDetailId = True
                Exit For
              End If
            Next Field

            ' If detail id field exists get detail cno and cid otherwise 
            If bDetailId Then
              .CNO = IIf(IsDBNull(rsPlotBoundaries.Fields("G3E_DETAILID").Value), rsPlotBoundaries.Fields("G3E_CNO").Value, rsPlotBoundaries.Fields("G3E_CNO_D").Value)
              .CID = IIf(IsDBNull(rsPlotBoundaries.Fields("G3E_DETAILID").Value), rsPlotBoundaries.Fields("G3E_CID").Value, rsPlotBoundaries.Fields("G3E_CID_D").Value)
              .DetailID = IIf(IsDBNull(rsPlotBoundaries.Fields("G3E_DETAILID").Value), "0", rsPlotBoundaries.Fields("G3E_DETAILID").Value)
            Else
              .CNO = rsPlotBoundaries.Fields("G3E_CNO").Value
              .CID = rsPlotBoundaries.Fields("G3E_CID").Value
              .DetailID = "0"
            End If

            If rsPlotBoundaries.Fields(GTPlotMetadata.Parameters.PlotBoundaryAttribute_Orientation).Value = "ADHOC" Then
              .Adhoc = True ' User selects MapScale, PaperSize and PaperOrientation
              ' Todo -Verify that setting to Landscape is used when ADHOC
              .PaperOrientation = optPaperOrientationLandscape.Text
            Else
              .Type = IIf(IsDBNull(rsPlotBoundaries.Fields(GTPlotMetadata.Parameters.PlotBoundaryAttribute_Type).Value), "", rsPlotBoundaries.Fields(GTPlotMetadata.Parameters.PlotBoundaryAttribute_Type).Value)

              .Type = GetTypeFromKey(.Type)

              .MapScale = rsPlotBoundaries.Fields(GTPlotMetadata.Parameters.PlotBoundaryAttribute_Scale).Value

              Dim sOrientation As String
              sOrientation = rsPlotBoundaries.Fields(GTPlotMetadata.Parameters.PlotBoundaryAttribute_Orientation).Value
              If sOrientation.Substring(0, 1) = "P" Then
                .PaperOrientation = optPaperOrientationPortrait.Text
              Else
                .PaperOrientation = optPaperOrientationLandscape.Text
              End If

              If GTPlotMetadata.Parameters.PlotBoundaryDataStoreSizeOnly.ToUpper = "YES" Then
                .PaperName = GetSheetNameFromSize(rsPlotBoundaries.Fields(GTPlotMetadata.Parameters.PlotBoundaryAttribute_PageSize).Value)
              Else
                .PaperName = FormatStringGetSheetName(rsPlotBoundaries.Fields(GTPlotMetadata.Parameters.PlotBoundaryAttribute_PageSize).Value)
              End If
              .PaperSize = FormatStringGetSheetSize(rsPlotBoundaries.Fields(GTPlotMetadata.Parameters.PlotBoundaryAttribute_PageSize).Value)


            End If

            ' Test for Formations in detail
            oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, My.Resources.NewPlotWindow.SetStatusBarText_UpdateAvailablePlotBoundaries & " -" &
                                                                    String.Format(My.Resources.NewPlotWindow.SetStatusBarText_UpdateAvailablePlotBoundaries_Adding, .Name) & " -" &
                                                                                  My.Resources.NewPlotWindow.SetStatusBarText_UpdateAvailablePlotBoundaries_LocatingFormations & "...")

            If FieldExists(rsPlotBoundaries, "G3E_DETAILID") Then
              If Not IsDBNull(rsPlotBoundaries.Fields("G3E_DETAILID").Value) Then
                If FormationsInDetail(rsPlotBoundaries.Fields("G3E_DETAILID").Value) Then
                  .HasFormations = True
                End If
              Else ' Check if Geographic polygon has structure containing formations

                ' Display the Feature Groups having Details having Formations in a TreeView
                Dim oDetailFeatureGroup As DetailFeatureGroup
                Dim colDetailFeatureGroups As Collection
                Dim oDetail As Detail

                colDetailFeatureGroups = GetDetailDrawingsWithinPolygon(oPlotBoundary)
                For Each oDetailFeatureGroup In colDetailFeatureGroups
                  ' Check for formations within details
                  For Each oDetail In oDetailFeatureGroup.Details
                    If FormationsInDetail(oDetail.DetailID) Then
                      .HasFormations = True
                      Exit For
                    End If
                  Next oDetail
                  If .HasFormations = True Then
                    Exit For
                  End If
                Next oDetailFeatureGroup

                oPlotBoundary.DetailFeatureGroups = colDetailFeatureGroups

              End If
            End If
          End With

          ' Add Plot Boundary to the public collection
          sKey = oPlotBoundary.FNO & " " & oPlotBoundary.FID & " " & oPlotBoundary.CNO & " " & oPlotBoundary.CID & " " & oPlotBoundary.DetailID & " " & oPlotBoundary.Name
          m_colPlotBoundaries.Add(oPlotBoundary, sKey)

          ' Populate Available Plot Boundaries
          oTreeNode = tvAvailablePlotBoundaries.Nodes.Add(sKey, oPlotBoundary.Name)
          oTreeNode.Name = sKey

          ' ToDo -Localize ADHOC !!!!
          If rsPlotBoundaries.Fields(GTPlotMetadata.Parameters.PlotBoundaryAttribute_Orientation).Value = "ADHOC" Then
            oTreeNode.ToolTipText = "ADHOC"
          Else
            oTreeNode.ToolTipText = oPlotBoundary.Type & " - " & FormatStringSetSheetNameSize(oPlotBoundary.PaperName, oPlotBoundary.PaperSize) & " - " & oPlotBoundary.PaperOrientation & " - " & oPlotBoundary.MapScale
          End If

          ' Test to see named plot already exists.
          If DoesNamedPlotExist(oTreeNode.Text) Then
            oTreeNode.ForeColor = Color.Gray
            oTreeNode.ToolTipText = oTreeNode.ToolTipText & vbCr & My.Resources.NewPlotWindow.UpdateAvailablePlotBoundaries_oTreeNode_ToolTipText ' Named plot already exists! Rename or Delete existing named plot to re-create.
          Else
            If oPlotBoundary.Adhoc Then
              oTreeNode.ForeColor = Color.Blue
            End If
          End If

          ' Test to see if detail and formations exist.  If so add formation icon
          ' Identify that formations exist to Auto Insert by showing a formation icon beside the plot boundary list
          If oPlotBoundary.HasFormations = True Then
            'oTreeNode.TreeView.CreateGraphics()
            oTreeNode.ImageIndex = 1
            oTreeNode.SelectedImageIndex = 1
          Else
            oTreeNode.ImageIndex = 0
            oTreeNode.SelectedImageIndex = 0
          End If

          rsPlotBoundaries.MoveNext()

        End While
      End If
      rsPlotBoundaries.Close()

      UpdateButtonsStates()



    Catch ex As Exception
      MsgBox("NewPlotWindowForm.UpdateAvailablePlotBoundaries:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    Finally
      If Not (rsPlotBoundaries Is Nothing) Then
        rsPlotBoundaries = Nothing
      End If
      ' Clear Status Bar Text Message and End Wait Cursor
      oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, sStatusBarText)
      oGTApplication.EndWaitCursor()

    End Try

  End Sub

  Public Function FieldExists(rs As ADODB.Recordset, sFieldName As String) As Boolean
    Dim fld As ADODB.Field
    For Each fld In rs.Fields
      If UCase(fld.Name) = UCase(sFieldName) Then
        FieldExists = True
        Exit Function
      End If
    Next fld
  End Function

  Private Sub NewPlotWindowForm_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
    If e.KeyCode = Keys.Escape Then
      Dispose()
    End If
  End Sub

  Private Sub RemoveSingle()
    Dim oTreeNode As TreeNode
    Dim colRemoveTreeNodes As New Collection

    For Each oTreeNode In tvSelectedPlotBoundaries.Nodes
      If oTreeNode.IsSelected = True Then
        tvAvailablePlotBoundaries.Nodes.Add(oTreeNode.Clone)
        'oTreeNode2 = tvAvailablePlotBoundaries.Nodes.Add(oTreeNode.Name, oTreeNode.Text) ', oTreeNode.Image)

        'If Not DoesNamedPlotExist(oTreeNode2.Text) Then
        '  oTreeNode2.Bold = True
        'End If
        colRemoveTreeNodes.Add(oTreeNode) 'Store for later removal
      End If
    Next oTreeNode

    For Each oTreeNode In colRemoveTreeNodes
      tvSelectedPlotBoundaries.Nodes.Remove(oTreeNode)
    Next oTreeNode

    'tvPlanIDs.SetFocus()
    'tvPlanIDs_Click()

    UpdateButtonsStates()
  End Sub

  Private Sub SelectSingle()
    Dim oTreeNode As TreeNode
    Dim colRemoveTreeNodes As New Collection

    Dim oPlotBoundary As PlotBoundary = Nothing

    For Each oTreeNode In tvAvailablePlotBoundaries.Nodes
      If oTreeNode.IsSelected = True Then
        If Not DoesNamedPlotExist(oTreeNode.Text) Then

          tvSelectedPlotBoundaries.Nodes.Add(oTreeNode.Clone)

          'oTreeNode2 = tvSelectedPlotBoundaries.Nodes.Add(oTreeNode.Text) ', oTreeNode.Image)
          'oTreeNode2.Name = oTreeNode.Name

          'tvSelectedPlotBoundaries.SelectedNode = oTreeNode2
          'tvSelectedPlanIDs_Click()

          'If oPlotBoundary.MapScale = "" Or oPlotBoundary.PaperSize = "" Or oPlotBoundary.PaperOrientation = "" Then
          '  oTreeNode2.ForeColor = Color.Red
          '  'oTreeNode2.Bold = True
          'End If


          colRemoveTreeNodes.Add(oTreeNode) 'Store for later removal

        End If

        Exit For
      End If
    Next oTreeNode

    For Each oTreeNode In colRemoveTreeNodes
      tvAvailablePlotBoundaries.Nodes.Remove(oTreeNode)
    Next oTreeNode

    'tvSelectedPlanIDs.SetFocus()
    'tvSelectedPlanIDs_Click()

    UpdateButtonsStates()
  End Sub

  Public Sub UpdateButtonsStates()

    Dim oTreeNode As TreeNode
    Dim blnEnableCreatePlotWindowButton As Boolean



    '
    ' Update the InsertActiveMapWindow check box state
    '
    'If Not MultiPage1.Value = 0 Then
    '  ' Check if active map window present
    '  If ActiveMapWindow Is Nothing Then
    '    chkInsertActiveMapWindow.Value = 0
    '    chkInsertActiveMapWindow.Enabled = False
    '  Else
    '    chkInsertActiveMapWindow.Enabled = True
    '    chkInsertActiveMapWindow.Value = 1
    '  End If
    'End If



    '
    ' Update Create Plot button
    '
    blnEnableCreatePlotWindowButton = True

    If TabControl1.SelectedIndex = 0 Then ' If Click from the Plot Boundary tab

      If tvSelectedPlotBoundaries.Nodes.Count = 0 Then
        blnEnableCreatePlotWindowButton = False
        DataGridViewAttributes1.ClearDataGridViewAttributes()
        DataGridViewAttributes1.RefreshDataGridViewAttributes()
      Else
        For Each oTreeNode In tvSelectedPlotBoundaries.Nodes
          If oTreeNode.ForeColor = Color.Red Then
            blnEnableCreatePlotWindowButton = False
            Exit For
          End If
        Next oTreeNode
      End If
    Else
      'DataGridViewAttributes2.ClearDataGridViewAttributes()
      'DataGridViewAttributes2.RefreshDataGridViewAttributes()

      'blnEnableCreatePlotWindowButton = False

      '  If chkIncludeTitleBlock.Value Then
      '    If txtPlanId2.text = "" Then
      '      blnEnableCreatePlotWindowButton = False
      '    End If
      '  Else
      '    If txtPlotName2.text = "" Then
      '      blnEnableCreatePlotWindowButton = False
      '    End If
      '  End If
    End If


    If Not blnEnableCreatePlotWindowButton Then
      cmdCreatePlotWindows.Enabled = False
      'tvPlanIDs.ControlTipText = "One or more Named Plots already exist and/or needs their Paper Size/Orientation set."
    Else
      cmdCreatePlotWindows.Enabled = True
      'tvPlanIDs.ControlTipText = ""
    End If



    '
    ' Update tv buttons
    '
    If TabControl1.SelectedIndex = 0 Then

      If tvAvailablePlotBoundaries.SelectedNode Is Nothing Then
        cmdSelectSingle.Enabled = False
      Else
        cmdSelectSingle.Enabled = True
      End If
      If tvSelectedPlotBoundaries.SelectedNode Is Nothing Then
        cmdRemoveSingle.Enabled = False
      Else
        cmdRemoveSingle.Enabled = True
      End If

      If tvAvailablePlotBoundaries.Nodes.Count = 0 Then
        cmdSelectAll.Enabled = False
      Else
        cmdSelectAll.Enabled = True
      End If
      If tvSelectedPlotBoundaries.Nodes.Count = 0 Then
        cmdRemoveAll.Enabled = False
      Else
        cmdRemoveAll.Enabled = True
      End If

      ' Formation button FUTURE
      'cmdFormation.Enabled = False
      'If Not tvSelectedPlanIDs.SelectedItem Is Nothing Then
      '  If Not tvSelectedPlanIDs.SelectedItem.Image = "" Then
      '    cmdFormation.Enabled = True
      '  End If
      'End If

    End If

  End Sub

  Private Sub cmdRemoveSingle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRemoveSingle.Click
    RemoveSingle()
  End Sub

  Private Sub tvSelectedPlotBoundaries_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvSelectedPlotBoundaries.AfterSelect
    UpdatePlotBoundaryInfo(e.Node.Name)
  End Sub

  Private Sub DataGridViewAttributes1_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs, ByVal oAttribute As Attribute) Handles DataGridViewAttributes1.CellValueChanged

    ' ForceMapBackgroundToWhite
    If oAttribute.G3E_USERNAME = My.Resources.NewPlotWindow.DataGridViewAttributes1_ForceMapBackgroundToWhite Then
      Dim oPlotBoundary As PlotBoundary
      oPlotBoundary = m_colPlotBoundaries(tvSelectedPlotBoundaries.SelectedNode.Name)
      If (oAttribute.VALUE = 1) Then
        oPlotBoundary.ForceMapBackgroundToWhite = "YES"
      Else
        oPlotBoundary.ForceMapBackgroundToWhite = "NO"
      End If
      oPlotBoundary.LegendOverrides = Nothing
      UpdatePlotBoundaryInfo(tvSelectedPlotBoundaries.SelectedNode.Name)
    End If
    ' Legend
    If oAttribute.G3E_USERNAME = My.Resources.NewPlotWindow.DataGridViewAttributes1_Legend Then
      Dim oPlotBoundary As PlotBoundary
      oPlotBoundary = m_colPlotBoundaries(tvSelectedPlotBoundaries.SelectedNode.Name)
      oPlotBoundary.Legend = oAttribute.VALUE
      oPlotBoundary.LegendOverrides = Nothing
      UpdatePlotBoundaryInfo(tvSelectedPlotBoundaries.SelectedNode.Name)
    End If
    ' Type
    If oAttribute.G3E_USERNAME = My.Resources.NewPlotWindow.DataGridViewAttributes1_Type Then
      Dim oPlotBoundary As PlotBoundary
      oPlotBoundary = m_colPlotBoundaries(tvSelectedPlotBoundaries.SelectedNode.Name)
      oPlotBoundary.Type = oAttribute.VALUE
      oPlotBoundary.LegendOverrides = Nothing
      UpdatePlotBoundaryInfo(tvSelectedPlotBoundaries.SelectedNode.Name)
    End If
    ' Paper Size
    If oAttribute.G3E_USERNAME = My.Resources.NewPlotWindow.DataGridViewAttributes1_PaperSize Then
      Dim oPlotBoundary As PlotBoundary
      oPlotBoundary = m_colPlotBoundaries(tvSelectedPlotBoundaries.SelectedNode.Name)
      oPlotBoundary.PaperName = FormatStringGetSheetName(oAttribute.VALUE)
      oPlotBoundary.PaperSize = FormatStringGetSheetSize(oAttribute.VALUE)
      oPlotBoundary.LegendOverrides = Nothing
      UpdatePlotBoundaryInfo(tvSelectedPlotBoundaries.SelectedNode.Name)
    End If
    ' Paper Orientation
    If oAttribute.G3E_USERNAME = My.Resources.NewPlotWindow.DataGridViewAttributes1_PaperOrientation Then
      Dim oPlotBoundary As PlotBoundary
      oPlotBoundary = m_colPlotBoundaries(tvSelectedPlotBoundaries.SelectedNode.Name)
      oPlotBoundary.PaperOrientation = oAttribute.VALUE
      oPlotBoundary.LegendOverrides = Nothing
      UpdatePlotBoundaryInfo(tvSelectedPlotBoundaries.SelectedNode.Name)
    End If
    ' Map Scale
    If oAttribute.G3E_USERNAME = My.Resources.NewPlotWindow.UpdatePlotBoundaryInfo_MapScale Then
      Dim oPlotBoundary As PlotBoundary
      oPlotBoundary = m_colPlotBoundaries(tvSelectedPlotBoundaries.SelectedNode.Name)
      oPlotBoundary.MapScale = oAttribute.VALUE
      oPlotBoundary.LegendOverrides = Nothing
      UpdatePlotBoundaryInfo(tvSelectedPlotBoundaries.SelectedNode.Name)
    End If

  End Sub

  Private Sub UpdatePlotBoundaryInfo(ByVal sPlotBoundaryName As String)

    Dim rs As New ADODB.Recordset
    Dim oPlotBoundary As PlotBoundary
    Dim oAttribute As Attribute
    Dim colAttributesDgvaCtrl As New Attributes  'Collection
    Dim oApplication As IGTApplication

    Try

      oApplication = GTClassFactory.Create(Of IGTApplication)()

      UpdateButtonsStates()



      ' Populate DataGridView
      colAttributesDgvaCtrl = DataGridViewAttributes1.Attributes
      DataGridViewAttributes1.ClearDataGridViewAttributes()
      'colAttributesDgvaCtrl.Clear() 'Never use this use ClearDataGridViewAttributes() instead




      oPlotBoundary = m_colPlotBoundaries(sPlotBoundaryName)



      '
      ' Plot Boundary Info 
      '
      colAttributesDgvaCtrl.Add(My.Resources.NewPlotWindow.UpdatePlotBoundaryInfo_PlotBoundaryInfo)
      colAttributesDgvaCtrl.Add(My.Resources.NewPlotWindow.UpdatePlotBoundaryInfo_Name, oPlotBoundary.Name, True, Attribute.CellType.TextBox)

      If oPlotBoundary.Adhoc Then

        ' Type
        Dim sFirstValue As String = ""
        oAttribute = colAttributesDgvaCtrl.Add(My.Resources.NewPlotWindow.UpdatePlotBoundaryInfo_Type, oPlotBoundary.Type, False, Attribute.CellType.ComboBox)
        oAttribute.VALUE_LIST.Add("")
        rs = GetTypes()
        While Not rs.EOF
          oAttribute.VALUE_LIST.Add(rs.Fields("DRI_TYPE" & LangSuffix()).Value)
          If String.IsNullOrEmpty(sFirstValue) Then
            sFirstValue = rs.Fields("DRI_TYPE" & LangSuffix()).Value
          End If
          rs.MoveNext()
        End While
        rs.Close()

        If String.IsNullOrEmpty(oPlotBoundary.Type) Then ' If nothing set to the first item.  FUTURE: Set to parameter defined default.  Must support multiple language default values
          oPlotBoundary.Type = sFirstValue
          oAttribute.VALUE = sFirstValue
        End If



        ' Default PaperOrientation to Landscape if not set
        ' If ADHOC automaticly gets set to Landscape ahead if this.
        If oPlotBoundary.PaperOrientation = "" Then oPlotBoundary.PaperOrientation = optPaperOrientationLandscape.Text

        ' Paper Size
        Dim bPaperFound = False
        oAttribute = colAttributesDgvaCtrl.Add(My.Resources.NewPlotWindow.UpdatePlotBoundaryInfo_PaperSize, "", False, Attribute.CellType.ComboBox)
        rs = GetPaperSizes(IIf(String.IsNullOrEmpty(oPlotBoundary.Type), "", oPlotBoundary.Type), oPlotBoundary.PaperOrientation)
        While Not rs.EOF
          bPaperFound = True
          If String.IsNullOrEmpty(oPlotBoundary.PaperName) Then
            oPlotBoundary.PaperName = rs.Fields("SHEET_NAME" & LangSuffix()).Value
            oPlotBoundary.PaperSize = rs.Fields("SHEET_SIZE").Value
          End If
          oAttribute.VALUE_LIST.Add(FormatStringSetSheetNameSize(rs.Fields("SHEET_NAME" & LangSuffix()).Value, rs.Fields("SHEET_SIZE").Value))
          rs.MoveNext()
        End While
        rs.Close()
        If bPaperFound Then
          oAttribute.VALUE = FormatStringSetSheetNameSize(oPlotBoundary.PaperName, oPlotBoundary.PaperSize)
        End If

        ' Paper Orientation
        oAttribute = colAttributesDgvaCtrl.Add(My.Resources.NewPlotWindow.UpdatePlotBoundaryInfo_PaperOrientation, oPlotBoundary.PaperOrientation, False, Attribute.CellType.ComboBox, optPaperOrientationPortrait.Text, optPaperOrientationLandscape.Text)

        ' Map Frame Group
        colAttributesDgvaCtrl.Add(My.Resources.NewPlotWindow.UpdatePlotBoundaryInfo_MapFrame, "", True, Attribute.CellType.Group)

        ' Scale
        Dim sScales As String
        Dim var() As String
        Dim bValidScale As Boolean = False

        ' Check to see if the scale already set is valid then use that.
        sScales = GetSheetScales(IIf(String.IsNullOrEmpty(oPlotBoundary.Type), "", oPlotBoundary.Type), oPlotBoundary.PaperName, oPlotBoundary.PaperOrientation)

        var = sScales.Split(",")
        For i = 0 To var.Length - 1
          If var(i) = oPlotBoundary.MapScale Then
            bValidScale = True
            Exit For
          End If
        Next i

        ' If existing scale is not valid set to default of "Fit Boundary"
        If Not bValidScale Then
          ' Set default to "Fit Boundary" when ADHOC and load the rest of the scales for the given pager and orientation
          oPlotBoundary.MapScale = My.Resources.NewPlotWindow.UpdatePlotBoundaryInfo_FitBoundary
        End If

        oAttribute = colAttributesDgvaCtrl.Add(My.Resources.NewPlotWindow.UpdatePlotBoundaryInfo_MapScale, oPlotBoundary.MapScale, False, Attribute.CellType.ComboBox, My.Resources.NewPlotWindow.UpdatePlotBoundaryInfo_FitBoundary)

        If String.IsNullOrEmpty(sScales) Then
          var = GTPlotMetadata.Parameters.PlotBoundaryAvailableScales.Split(",")
          For i = 0 To var.Length - 1
            oAttribute.VALUE_LIST.Add(var(i))
          Next i
        Else
          var = sScales.Split(",")
          For i = 0 To var.Length - 1
            oAttribute.VALUE_LIST.Add(var(i))
          Next i
        End If
        ' Removed set default to first item
        'oAttribute.VALUE = var(0)

      Else
        colAttributesDgvaCtrl.Add(My.Resources.NewPlotWindow.UpdatePlotBoundaryInfo_Type, oPlotBoundary.Type, True, Attribute.CellType.TextBox)
        colAttributesDgvaCtrl.Add(My.Resources.NewPlotWindow.UpdatePlotBoundaryInfo_PaperSize, FormatStringSetSheetNameSize(oPlotBoundary.PaperName, oPlotBoundary.PaperSize), True, Attribute.CellType.TextBox)
        'Todo: Add Landscape when 'L' and Portrait when 'P'
        colAttributesDgvaCtrl.Add(My.Resources.NewPlotWindow.UpdatePlotBoundaryInfo_PaperOrientation, oPlotBoundary.PaperOrientation, True, Attribute.CellType.TextBox)

        colAttributesDgvaCtrl.Add(My.Resources.NewPlotWindow.UpdatePlotBoundaryInfo_MapFrame, "", True, Attribute.CellType.Group)
        colAttributesDgvaCtrl.Add(My.Resources.NewPlotWindow.UpdatePlotBoundaryInfo_MapScale, oPlotBoundary.MapScale, True, Attribute.CellType.TextBox)
      End If



      '
      ' Legend
      '
      If String.IsNullOrEmpty(oPlotBoundary.Legend) Then
        Dim sDefaultLegend As String = GetTypesDefaultLegend(oPlotBoundary.Type, oPlotBoundary.PaperSize, oPlotBoundary.PaperOrientation, IIf(oPlotBoundary.DetailID = 0, False, True))
        oPlotBoundary.Legend = GetLegendName(sDefaultLegend)
      End If

      oAttribute = colAttributesDgvaCtrl.Add(My.Resources.NewPlotWindow.UpdatePlotBoundaryInfo_Legend, oPlotBoundary.Legend, True, Attribute.CellType.ComboBox)
      ' Get list of available legends
      rs = GetLegends(IIf(oPlotBoundary.DetailID = 0, False, True))
      While Not rs.EOF
        If oApplication.DataContext.IsRoleGranted(rs.Fields("g3e_role").Value) Then
          oAttribute.VALUE_LIST.Add(rs.Fields("g3e_username").Value)
        End If
        rs.MoveNext()
      End While
      rs.Close()



      '
      ' Style Substitution 
      '
      oAttribute = colAttributesDgvaCtrl.Add(My.Resources.NewPlotWindow.UpdatePlotBoundaryInfo_StyleSubstitutions, oPlotBoundary.StyleSubstitution, False, Attribute.CellType.ComboBox)
      oAttribute.VALUE_LIST.Add("") ' Add blank line
      rs = GetStyleSubstitution()
      While Not rs.EOF
        oAttribute.VALUE_LIST.Add(rs.Fields("g3e_username").Value)
        rs.MoveNext()
      End While
      rs.Close()




      '
      ' Force White Background
      '
      If (GTPlotMetadata.Parameters.NewPlotWindowForceMapBackgroundToWhite = "YES") Then 'PA -Added default parameter.
        If String.IsNullOrEmpty(oPlotBoundary.ForceMapBackgroundToWhite) Then
          Dim sDefaultForceMapBackgroundToWhite As String = GTPlotMetadata.Parameters.NewPlotWindowForceMapBackgroundToWhite
          oPlotBoundary.ForceMapBackgroundToWhite = sDefaultForceMapBackgroundToWhite
        End If

        If (oPlotBoundary.ForceMapBackgroundToWhite = "YES") Then
          oAttribute = colAttributesDgvaCtrl.Add(My.Resources.NewPlotWindow.UpdatePlotBoundaryInfo_ForceMapBackgroundToWhite, 1, True, Attribute.CellType.CheckBox)
        Else
          oAttribute = colAttributesDgvaCtrl.Add(My.Resources.NewPlotWindow.UpdatePlotBoundaryInfo_ForceMapBackgroundToWhite, 0, True, Attribute.CellType.CheckBox)
        End If
      End If



      '
      ' Legend Override 
      '
      Dim oLegendOverride As LegendOverride
      If oPlotBoundary.LegendOverrides Is Nothing Then
        oPlotBoundary.LegendOverrides = New Collection
        rs = GetLegendFilters(oPlotBoundary.Type, GetLegendLNO(oPlotBoundary.Legend))
        While Not rs.EOF

          ' Store LegendOverrides for later use
          oLegendOverride = New LegendOverride
          With oLegendOverride
            .Group_Id = rs.Fields("LO_GROUP_ID").Value
            .Username = rs.Fields("LO_USERNAME").Value
            .User_Option = rs.Fields("LO_USER_OPTION").Value
            .User_Option_Default = rs.Fields("LO_USER_OPTION_DEFAULT").Value
          End With
          oPlotBoundary.LegendOverrides.Add(oLegendOverride, oLegendOverride.Username)

          rs.MoveNext()
        End While
        rs.Close()
      Else
      End If

      If oPlotBoundary.LegendOverrides.Count > 0 Then
        Dim b1stPass As Boolean = True
        For Each oLegendOverride In oPlotBoundary.LegendOverrides
          If oLegendOverride.User_Option Then
            If b1stPass Then
              colAttributesDgvaCtrl.Add(My.Resources.NewPlotWindow.UpdatePlotBoundaryInfo_LegendOverrides)
              b1stPass = False
            End If
            colAttributesDgvaCtrl.Add(oLegendOverride.Username, oLegendOverride.User_Option_Value, False, Attribute.CellType.MultiCheckBox)
          End If
        Next
      End If



      '
      ' Auto Insert Formations
      '
      If oPlotBoundary.HasFormations Then
        ' Add formations header
        colAttributesDgvaCtrl.Add(My.Resources.NewPlotWindow.UpdatePlotBoundaryInfo_Formations)

        ' Create TreeView control
        Dim oTreeView As New TreeView
        oTreeView.CreateControl()

        ' Register Events
        AddHandler oTreeView.AfterCollapse, AddressOf TreeView_AfterExpand_Handler
        AddHandler oTreeView.AfterExpand, AddressOf TreeView_AfterExpand_Handler
        AddHandler oTreeView.AfterCheck, AddressOf TreeView_AfterCheck_Handler
        AddHandler oTreeView.AfterSelect, AddressOf TreeView_AfterSelect_Handler

        ' Set properties
        With oTreeView
          .Name = My.Resources.NewPlotWindow.UpdatePlotBoundaryInfo_AutoInsertFormations
          .CheckBoxes = True
          .Scrollable = False ' If used and want the parent control to update its scroll bars you must call 
          .ShowRootLines = False
        End With
        oAttribute = New Attribute
        With oAttribute
          .VALUE_TYPE = Attribute.CellType.TreeView
          .G3E_USERNAME = "" '"Auto Insert Formations"
          .TreeView = oTreeView
        End With
        DataGridViewAttributes1.Attributes.Add(oAttribute)


        Dim oDetailFeatureGroup As DetailFeatureGroup
        Dim oDetail As Detail
        Dim nodeDetailFeatureGroup As TreeNode
        Dim nodeDetail As TreeNode
        Dim nodeFormation As TreeNode
        Dim oFormation As Formation

        Dim oKeyObject As IGTKeyObject
        Dim oIGTHelperService As IGTHelperService

        oIGTHelperService = GTClassFactory.Create(Of IGTHelperService)()
        oIGTHelperService.DataContext = oApplication.DataContext

        ' If plot boundary and formation are in a detail...
        If Not oPlotBoundary.DetailID = 0 Then

          ' Get detail drawing's owner info if not previously retreived
          Dim oFeature As Feature
          If String.IsNullOrEmpty(oPlotBoundary.DetailOwnerLabel) Then
            oFeature = GetDetailOwner(oPlotBoundary.DetailID)
            oPlotBoundary.DetailOwnerFNO = oFeature.FNO
            oPlotBoundary.DetailOwnerFID = oFeature.FID
            ' Get the feature label
            oKeyObject = oApplication.DataContext.OpenFeature(oPlotBoundary.DetailOwnerFNO, oPlotBoundary.DetailOwnerFID)
            oPlotBoundary.DetailOwnerLabel = oIGTHelperService.GetFeatureLabel(oKeyObject)
          End If

          ' Add root detail owner node
          nodeDetail = New TreeNode()
          nodeDetail.Text = oPlotBoundary.DetailOwnerLabel
          nodeDetail.Name = oPlotBoundary.FNO & "." & oPlotBoundary.FID
          oTreeView.Nodes.Add(nodeDetail)

          ' Get formations if not previously retreived
          If oPlotBoundary.Formations.Count = 0 Then
            oPlotBoundary.Formations = GetFormationsInDetail(oPlotBoundary.DetailID)

            ' Default to selected - If user doesn't select the boundary to display its info this will not work.
            'For Each oFormation In oPlotBoundary.Formations
            '  oFormation.PlaceFormation = True
            'Next

          End If
          For Each oFormation In oPlotBoundary.Formations

            nodeFormation = New TreeNode()

            ' Get the feature label
            oKeyObject = oApplication.DataContext.OpenFeature(oFormation.FNO, oFormation.FID)
            oFormation.Description = oIGTHelperService.GetFeatureLabel(oKeyObject)

            nodeFormation.Text = oFormation.Description
            nodeFormation.Name = nodeDetail.Name & "." & oFormation.FNO & "." & oFormation.FID

            ' Persist Auto Insert option if removed from list then re-added later.
            If oFormation.PlaceFormation = True Then
              nodeFormation.Checked = CheckState.Checked
            Else
              nodeFormation.Checked = CheckState.Unchecked
            End If

            nodeFormation.Expand()
            nodeDetail.Nodes.Add(nodeFormation)
          Next
          nodeDetail.Expand()

          ' If plot boundary in geo and formation are in a detail contained withing geo boundary...
        ElseIf oPlotBoundary.DetailFeatureGroups.Count > 0 Then

          For Each oDetailFeatureGroup In oPlotBoundary.DetailFeatureGroups

            nodeDetailFeatureGroup = New TreeNode()
            nodeDetailFeatureGroup.Text = oDetailFeatureGroup.Name
            nodeDetailFeatureGroup.Name = oDetailFeatureGroup.Name
            oTreeView.Nodes.Add(nodeDetailFeatureGroup)

            For Each oDetail In oDetailFeatureGroup.Details

              nodeDetail = New TreeNode()

              ' Get the feature label
              oKeyObject = oApplication.DataContext.OpenFeature(oDetail.FNO, oDetail.FID)
              oDetail.FeatureLabel = oIGTHelperService.GetFeatureLabel(oKeyObject)
              nodeDetail.Text = oIGTHelperService.GetFeatureLabel(oKeyObject)
              nodeDetail.Name = oDetailFeatureGroup.Name & "." & oDetail.FNO & "." & oDetail.FID

              If oDetail.Formations.Count = 0 Then
                oDetail.Formations = GetFormationsInDetail(oDetail.DetailID)
              End If

              For Each oFormation In oDetail.Formations

                nodeFormation = New TreeNode()

                ' Get the feature label
                oKeyObject = oApplication.DataContext.OpenFeature(oFormation.FNO, oFormation.FID)
                oFormation.Description = oIGTHelperService.GetFeatureLabel(oKeyObject)

                nodeFormation.Text = oFormation.Description
                nodeFormation.Name = nodeDetail.Name & "." & oFormation.FNO & "." & oFormation.FID

                ' Persist Auto Insert option if removed from list then re-added later.
                If oFormation.PlaceFormation = True Then
                  nodeFormation.Checked = CheckState.Checked
                Else
                  nodeFormation.Checked = CheckState.Unchecked
                End If

                nodeFormation.Expand()
                nodeDetail.Nodes.Add(nodeFormation)
              Next

              If oDetail.Formations.Count > 0 Then
                nodeDetail.Expand()
                nodeDetailFeatureGroup.Nodes.Add(nodeDetail)
              End If
            Next oDetail

            ' Delete group is empty
            If nodeDetailFeatureGroup.GetNodeCount(True) = 0 Then
              oTreeView.Nodes.Remove(nodeDetailFeatureGroup)
            Else
              nodeDetailFeatureGroup.Expand()
            End If
          Next oDetailFeatureGroup

        End If

      End If



      ' Refresh the Data Grid View Attributes
      DataGridViewAttributes1.RefreshDataGridViewAttributes()



    Catch ex As Exception
      MsgBox("NewPlotWindowForm.UpdatePlotBoundaryInfo:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)

      Debug.Assert(Not ex Is Nothing)

    End Try

  End Sub

  Private Sub tvSelectedPlotBoundaries_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tvSelectedPlotBoundaries.DoubleClick
    RemoveSingle()
  End Sub

  Private Sub tvAvailablePlotBoundaries_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvAvailablePlotBoundaries.AfterSelect
    UpdateButtonsStates()
  End Sub

  Private Sub tvAvailablePlotBoundaries_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tvAvailablePlotBoundaries.DoubleClick
    SelectSingle()
  End Sub

  Private Sub cmdSelectSingle_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSelectSingle.Click
    SelectSingle()
  End Sub

  Private Sub cmdSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSelectAll.Click
    Dim oTreeNode As TreeNode
    Dim colRemoveTreeNodes As New Collection

    For Each oTreeNode In tvAvailablePlotBoundaries.Nodes
      If Not DoesNamedPlotExist(oTreeNode.Text) Then

        tvSelectedPlotBoundaries.Nodes.Add(oTreeNode.Clone)

        colRemoveTreeNodes.Add(oTreeNode) 'Store for later removal

      End If
    Next oTreeNode

    For Each oTreeNode In colRemoveTreeNodes
      tvAvailablePlotBoundaries.Nodes.Remove(oTreeNode)
    Next oTreeNode

    UpdateButtonsStates()

  End Sub

  Private Sub cmdRemoveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRemoveAll.Click
    Dim oTreeNode As TreeNode
    Dim colRemoveTreeNodes As New Collection

    tvSelectedPlotBoundaries.SelectedNode = Nothing

    For Each oTreeNode In tvSelectedPlotBoundaries.Nodes
      tvAvailablePlotBoundaries.Nodes.Add(oTreeNode.Clone)
      colRemoveTreeNodes.Add(oTreeNode) 'Store for later removal
    Next oTreeNode

    For Each oTreeNode In colRemoveTreeNodes
      tvSelectedPlotBoundaries.Nodes.Remove(oTreeNode)
    Next oTreeNode

    UpdateButtonsStates()

  End Sub

  Private Sub cbPlotBoundaryFilterUser_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbPlotBoundaryFilterUser.SelectedIndexChanged
    cmdRemoveAll_Click(sender, e)
    PopulateCapitalWorkOrderNumberPicklist()
  End Sub

  Private Sub TabControl1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged
    UpdateButtonsStates()
  End Sub

  Private Sub DataGridViewAttributes1_Leave1(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridViewAttributes1.Leave
    Dim oTreeNode As TreeNode
    Dim oPlotBoundary As PlotBoundary = Nothing
    Dim colAttributesDgvaCtrl As New Attributes  'Collection


    '
    ' Get selected node if one otherwise EXIT!
    '
    For Each oTreeNode In tvSelectedPlotBoundaries.Nodes
      If oTreeNode.IsSelected = True Then
        oPlotBoundary = m_colPlotBoundaries(oTreeNode.Name)
        Exit For
      End If
    Next oTreeNode
    If oPlotBoundary Is Nothing Then Exit Sub


    '
    ' Get DataGridView Attributes if any otherwise EXIT!
    '
    colAttributesDgvaCtrl = DataGridViewAttributes1.Attributes
    If colAttributesDgvaCtrl.Count <= 0 Then Exit Sub


    '
    ' Save changes before leaving control
    '

    'Save Page Info
    oPlotBoundary.Type = colAttributesDgvaCtrl(My.Resources.NewPlotWindow.UpdatePlotBoundaryInfo_Type).VALUE
    oPlotBoundary.PaperName = FormatStringGetSheetName(colAttributesDgvaCtrl(My.Resources.NewPlotWindow.UpdatePlotBoundaryInfo_PaperSize).VALUE)
    oPlotBoundary.PaperSize = FormatStringGetSheetSize(colAttributesDgvaCtrl(My.Resources.NewPlotWindow.UpdatePlotBoundaryInfo_PaperSize).VALUE)
    oPlotBoundary.PaperOrientation = colAttributesDgvaCtrl(My.Resources.NewPlotWindow.UpdatePlotBoundaryInfo_PaperOrientation).VALUE

    'Save Legend
    oPlotBoundary.Legend = colAttributesDgvaCtrl(My.Resources.NewPlotWindow.UpdatePlotBoundaryInfo_Legend).VALUE

    'Save Style StyleSubstitution
    oPlotBoundary.StyleSubstitution = colAttributesDgvaCtrl(My.Resources.NewPlotWindow.UpdatePlotBoundaryInfo_StyleSubstitutions).VALUE

    'Save Legend Override
    For Each oLegendOverride As LegendOverride In oPlotBoundary.LegendOverrides
      If oLegendOverride.User_Option Then
        oLegendOverride.User_Option_Value = colAttributesDgvaCtrl(oLegendOverride.Username).VALUE
      End If
    Next

    ' Save PlaceFormations
    If oPlotBoundary.Formations.Count > 0 Or oPlotBoundary.DetailFeatureGroups.Count > 0 Then
      ' Get a handle to the TreeView control on the DataGridViewAttributes1 control
      Dim oTreeView As TreeView = Nothing
      Dim oAttribute As Attribute

      For Each oAttribute In DataGridViewAttributes1.Attributes
        If oAttribute.VALUE_TYPE = Attribute.CellType.TreeView Then
          oTreeView = oAttribute.TreeView
          Exit For
        End If
      Next

      ' Loop through formations and save user settings
      If Not IsNothing(oTreeView) Then
        Dim oDetailFeatureGroup As DetailFeatureGroup
        Dim oDetail As Detail
        Dim oFormation As Formation
        Dim oFoundTreeNodes As TreeNode()
        Dim oFoundTreeNode As TreeNode

        If oPlotBoundary.Formations.Count > 0 Then

          oPlotBoundary.PlaceFormations = False
          For Each oFormation In oPlotBoundary.Formations

            oFoundTreeNodes = oTreeView.Nodes.Find(oPlotBoundary.FNO & "." & oPlotBoundary.FID & "." & oFormation.FNO & "." & oFormation.FID, True)

            For Each oFoundTreeNode In oFoundTreeNodes
              If oFoundTreeNode.Checked = True Then
                oFormation.PlaceFormation = True
                oPlotBoundary.PlaceFormations = True
              Else
                oFormation.PlaceFormation = False
              End If
            Next
          Next oFormation

          ' Check if formations found withing polygon
        ElseIf oPlotBoundary.DetailFeatureGroups.Count > 0 Then

          oPlotBoundary.PlaceFormations = False
          For Each oDetailFeatureGroup In oPlotBoundary.DetailFeatureGroups
            For Each oDetail In oDetailFeatureGroup.Details
              For Each oFormation In oDetail.Formations

                oFoundTreeNodes = oTreeView.Nodes.Find(oDetailFeatureGroup.Name & "." & oDetail.FNO & "." & oDetail.FID & "." & oFormation.FNO & "." & oFormation.FID, True)

                For Each oFoundTreeNode In oFoundTreeNodes
                  'oTreeView.Focus()
                  'oTreeView.SelectedNode = oFoundTreeNode

                  If oFoundTreeNode.Checked = True Then
                    oFormation.PlaceFormation = True
                    oPlotBoundary.PlaceFormations = True
                  Else
                    oFormation.PlaceFormation = False
                  End If
                Next
              Next oFormation
            Next oDetail
          Next oDetailFeatureGroup

        End If ' Formations?
      End If ' TreeView?
    End If ' Formations to save?


  End Sub

#Region "User Defined Plot Tab"

  Public m_iDRI_ID As Long
  Public m_sType As String
  Public m_sMapScale As String
  Public m_sMapScaleCustom As String
  Public m_sMapScalePreDefined As String
  Public m_lMapScale As Integer
  Public m_sSheetName As String
  Public m_sSheetSize As String
  Public m_sSheetOrientation As String
  Public m_lSheetId As Integer
  Public m_dSheetHeight As Double
  Public m_dSheetWidth As Double
  Public m_dSheet_Inset As Double
  Public m_dCaptionStampTLX As Double
  Public m_dCaptionStampTLY As Double

  Public m_dMapTLX As Double
  Public m_dMapTLY As Double
  Public m_dMapBRX As Double
  Public m_dMapBRY As Double

  Public m_dMapHeight As Double
  Public m_dMapWidth As Double

  Public m_oMapTLPoint As IGTPoint = GTClassFactory.Create(Of IGTPoint)()
  Public m_oMapBRPoint As IGTPoint = GTClassFactory.Create(Of IGTPoint)()

  Public m_rsMapFrames As ADODB.Recordset
  Public m_rsKeyMapFrames As ADODB.Recordset
  Public m_rsRedlines As ADODB.Recordset


  ' Properties
  Private m_dMapHeightScaled As Double
  Private m_dMapWidthScaled As Double

  Public Property MapHeightScaled() As Double
    Get
      MapHeightScaled = m_dMapHeightScaled
    End Get
    Set(ByVal value As Double)
      m_dMapHeightScaled = value
    End Set
  End Property

  Public Property MapWidthScaled() As Double
    Get
      MapWidthScaled = m_dMapWidthScaled
    End Get
    Set(ByVal value As Double)
      m_dMapWidthScaled = value
    End Set
  End Property

  '
  ' When the Type Sheet or Orientation changes update the Paper Size list
  '
  Private Sub UpdateAvailableSheets()

    Dim rsPaperSizes As ADODB.Recordset
    Dim oGTApplication As IGTApplication

    Try

      ' Skip out if values are empty
      oGTApplication = GTClassFactory.Create(Of IGTApplication)()
      If String.IsNullOrEmpty(m_sSheetOrientation) Or IsNothing(oGTApplication.Application) Then
        Exit Sub
      End If

      'Populate PaperSize listbox
      rsPaperSizes = GetPaperSizes(m_sType, m_sSheetOrientation)

      ' Save current setting and attempt to set them back once lists refresh...
      lbPaperSize.SuspendLayout()
      Dim sPaperSize As String = lbPaperSize.Text

      lbPaperSize.Items.Clear()
      While Not rsPaperSizes.EOF
        lbPaperSize.Items.Add(FormatStringSetSheetNameSize(rsPaperSizes.Fields("SHEET_NAME" & LangSuffix()).Value, rsPaperSizes.Fields("SHEET_SIZE").Value))
        rsPaperSizes.MoveNext()
      End While
      rsPaperSizes.Close()

      ' Save current setting and attempt to set them back once lists refresh...
      If lbPaperSize.Items.Contains(sPaperSize) Then
        lbPaperSize.Text = sPaperSize
      Else
        Try
          If lbPaperSize.Items.Count > 0 Then
            lbPaperSize.SetSelected(0, True)
          End If
        Catch ex As Exception
        End Try
      End If
      lbPaperSize.ResumeLayout()


    Catch ex As Exception
      MsgBox("PlotInfoControl.UpdateAvailableSheets:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    End Try
  End Sub


  '
  ' When the Type Sheet or Orientation changes update the scales list
  '
  Private Sub UpdateAvailableScales()

    Dim oGTApplication As IGTApplication

    Try

      ' Skip out if values are empty
      oGTApplication = GTClassFactory.Create(Of IGTApplication)()
      If String.IsNullOrEmpty(m_sSheetName) Or String.IsNullOrEmpty(m_sSheetOrientation) Or IsNothing(oGTApplication.Application) Then
        Exit Sub
      End If

      ' Verify that Orientation exists for Sheet otherwise auto change Orientation
      If (DoesSheetSupportOrientation(m_sType, m_sSheetName, m_sSheetOrientation) = False) Then
        If (m_sSheetOrientation = optPaperOrientationLandscape.Text) Then
          m_sSheetOrientation = optPaperOrientationPortrait.Text
          optPaperOrientationPortrait.Checked = True
        Else
          m_sSheetOrientation = optPaperOrientationLandscape.Text
          optPaperOrientationLandscape.Checked = True
        End If
      End If

      Dim sScales As String

      'Get the Map Size from Map Frame table
      sScales = GetSheetScales(m_sType, m_sSheetName, m_sSheetOrientation)

      ' Save current setting and attempt to set them back once lists refresh...
      lbMapScalePreDefined.SuspendLayout()
      Dim sMapScalePreDefined As String = lbMapScalePreDefined.Text

      'Populate MapScalePreDefined listbox
      lbMapScalePreDefined.Items.Clear()
      If String.IsNullOrEmpty(sScales) Then
        lbMapScalePreDefined.Items.AddRange(GTPlotMetadata.Parameters.PlotBoundaryAvailableScales.Split(","))
      Else
        lbMapScalePreDefined.Items.AddRange(sScales.Split(","))
      End If

      ' Save current setting and attempt to set them back once lists refresh...
      If lbMapScalePreDefined.Items.Contains(sMapScalePreDefined) Then
        lbMapScalePreDefined.Text = sMapScalePreDefined
      Else
        lbMapScalePreDefined.SetSelected(0, True)
      End If
      lbMapScalePreDefined.ResumeLayout()



    Catch ex As Exception
      MsgBox("PlotInfoControl.UpdateAvailableScales:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    End Try
  End Sub

  '
  ' When the WPB changes update the image page
  '
  Private Sub WPB_Change()

    Dim oGTApplication As IGTApplication

    Try
      If 1 Then
        m_sMapScale = m_sMapScale
      Else
        m_sMapScale = m_sMapScaleCustom
      End If

      'If the SheetName or SheetOrientation is not set exit
      oGTApplication = GTClassFactory.Create(Of IGTApplication)()
      'If m_sSheetName = "" Or m_sSheetOrientation = "" Or m_sMapScale = "" Or IsNothing(oGTApplication.Application) Then Exit Sub
      If String.IsNullOrEmpty(m_sSheetName) Or String.IsNullOrEmpty(m_sSheetOrientation) Or String.IsNullOrEmpty(m_sMapScale) Or IsNothing(oGTApplication.Application) Then Exit Sub
      'DCH 02272018
      If Not ValidScaleMetric(m_sMapScale) And Not ValidScaleImperial(m_sMapScale) Then
        'tbMapScaleCustom.Font.Bold = True
        tbMapScaleCustom.ForeColor = Color.Red ' System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red)
        Exit Sub
      End If
      'tbMapScaleCustom.Font.Bold = False
      tbMapScaleCustom.ForeColor = System.Drawing.SystemColors.WindowText ' System.Drawing.ColorTranslator.ToOle(System.Drawing.SystemColors.WindowText)



      '
      ' Get info
      '
      'Get the Sheet info
      If DoesSheetSizeExist(m_sType, m_sSheetName, m_sSheetOrientation) = False Then
        ' Try changing Sheet Orientation
        If (optPaperOrientationLandscape.Checked = True) Then
          optPaperOrientationPortrait.Checked = True
        Else
          optPaperOrientationLandscape.Checked = True
        End If
      End If
      GetSheetSize(m_sType, m_sSheetName, m_sSheetOrientation, m_lSheetId, m_dSheetHeight, m_dSheetWidth, m_iDRI_ID, m_dSheet_Inset)

      'Get the Primary Map Size from Map Frame table
      GetMapSize(m_iDRI_ID, m_dMapHeight, m_dMapWidth)

      'Get the Primary Map Location from Map Frame table
      GetMapLocation(m_iDRI_ID, m_oMapTLPoint, m_oMapBRPoint)


      ' Get All MapFrames and Redline Recocordsets used to paint preview
      If Not m_rsMapFrames Is Nothing Then m_rsMapFrames.Close() ' Close previous open recordset
      m_rsMapFrames = New ADODB.Recordset
      m_rsMapFrames = GetMapFrames(m_iDRI_ID)

      If Not m_rsKeyMapFrames Is Nothing Then m_rsKeyMapFrames.Close() ' Close previous open recordset
      m_rsKeyMapFrames = New ADODB.Recordset
      m_rsKeyMapFrames = GetKeyMapFrames(m_iDRI_ID)

      If Not m_rsRedlines Is Nothing Then m_rsRedlines.Close() ' Close previous open recordset
      m_rsRedlines = New ADODB.Recordset
      m_rsRedlines = GetRedlines(m_iDRI_ID)



      ' Determine if using Metric or Imperial scaling...
      Dim eMeasurmentUnits = GetMeasurmentUnits(m_sMapScale)
      Dim dConvertMmToIn = 0.0393701



      m_lMapScale = GetScaleValue(m_sMapScale)
      If (eMeasurmentUnits = MeasurmentUnits.Metric) Then
        ' Get Metric Map Scale & Scaled Map Size
        m_dMapHeightScaled = (m_dMapHeight) * (m_lMapScale / 1000)
        m_dMapWidthScaled = (m_dMapWidth) * (m_lMapScale / 1000)
      ElseIf (eMeasurmentUnits = MeasurmentUnits.Imperial) Then
        ' Get Imperial in. to ft. Map Scale & Scaled Map Size
        m_dMapHeightScaled = (m_dMapHeight * dConvertMmToIn) * (m_lMapScale)
        m_dMapWidthScaled = (m_dMapWidth * dConvertMmToIn) * (m_lMapScale)
      End If



      '
      ' Show user the paper size, orientation, border inset in mm and actual map size in meters
      '
      If optPaperOrientationLandscape.Checked = True Then
        lblSetPaperValue.Text = m_sSheetName & " (" & m_sSheetSize & ") - " & optPaperOrientationLandscape.Text
      Else
        lblSetPaperValue.Text = m_sSheetName & " (" & m_sSheetSize & ") - " & optPaperOrientationPortrait.Text
      End If
      If (eMeasurmentUnits = MeasurmentUnits.Metric) Then
        lblSetBorderInsetValue.Text = m_dSheet_Inset & GTPlotMetadata.Parameters.MeasurementUnitsMetricLabelSmall
        lblSetMapSizeValue.Text = Trim(Str(System.Math.Round(m_dMapWidthScaled, 1)) & GTPlotMetadata.Parameters.MeasurementUnitsMetricLabellLarge & " x " & Str(System.Math.Round(m_dMapHeightScaled, 1)) & GTPlotMetadata.Parameters.MeasurementUnitsMetricLabellLarge)
      ElseIf (eMeasurmentUnits = MeasurmentUnits.Imperial) Then
        lblSetBorderInsetValue.Text = Str(System.Math.Round(m_dSheet_Inset * dConvertMmToIn, 3)) & GTPlotMetadata.Parameters.MeasurementUnitsImperialLabelSmall
        lblSetMapSizeValue.Text = Trim(Str(System.Math.Round(m_dMapWidthScaled, 1)) & GTPlotMetadata.Parameters.MeasurementUnitsImperialLabelLarge & " x " & Str(System.Math.Round(m_dMapHeightScaled, 1)) & GTPlotMetadata.Parameters.MeasurementUnitsImperialLabelLarge)
      End If
      If optMapScalePreDefined.Checked Then
        lblSetMapScaleValue.Text = m_sMapScale
      Else
        lblSetMapScaleValue.Text = m_sMapScaleCustom
      End If



      ' Repaint the preview using newly set variables
      imgPageContainer.Refresh()



    Catch ex As Exception
      MsgBox("PlotInfoControl.WPB_Change:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    End Try

  End Sub

  Private Sub LoadDefaults()
    Dim rsTypes As ADODB.Recordset

    Try

      'Populate Type listbox
      rsTypes = GetTypes()
      lbType.Items.Clear()
      While Not rsTypes.EOF
        lbType.Items.Add(rsTypes.Fields("DRI_TYPE" & LangSuffix()).Value)
        rsTypes.MoveNext()
      End While
      rsTypes.Close()


      Restore()


      'Populate MapScaleCustom textbox
      Dim oGTApplication As IGTApplication
      oGTApplication = GTClassFactory.Create(Of IGTApplication)()


      chkInsertActiveMapWindow.Checked = ActiveMapWindow()
      If chkInsertActiveMapWindow.Checked Then
        optMapScaleCustom.Checked = True
        If GTPlotMetadata.Parameters.MeasurementUnits = "Metric" Then
          tbMapScaleCustom.Text = "1:" & Math.Round(oGTApplication.ActiveMapWindow.DisplayScale, 0)
        Else
          tbMapScaleCustom.Text = "1""=" & Math.Round(oGTApplication.ActiveMapWindow.DisplayScale / 12, 0) & "'"
        End If
      Else
        optMapScalePreDefined.Checked = True
        If GTPlotMetadata.Parameters.MeasurementUnits = "Metric" Then
          tbMapScaleCustom.Text = "1:750"
        Else
          tbMapScaleCustom.Text = "1""=20'"
        End If
      End If


    Catch ex As Exception
      MsgBox("NewPlotWindowForm.LoadDefaults:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    Finally
      'cleaning
    End Try
  End Sub



#Region "Control Events"

  Private Sub optMapScalePreDefined_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optMapScalePreDefined.CheckedChanged
    lbMapScalePreDefined.Enabled = True

    tbMapScaleCustom.Enabled = False
    If optMapScalePreDefined.Checked = False Then
      m_sMapScalePreDefined = lbMapScalePreDefined.Text
    End If
    m_sMapScale = lbMapScalePreDefined.Text
    WPB_Change()
  End Sub

  Private Sub optMapScaleCustom_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optMapScaleCustom.CheckedChanged
    lbMapScalePreDefined.Enabled = False
    tbMapScaleCustom.Enabled = True
    m_sMapScaleCustom = tbMapScaleCustom.Text
    m_sMapScale = m_sMapScaleCustom
    WPB_Change()
  End Sub

  Private Sub optPaperOrientationLandscape_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optPaperOrientationLandscape.CheckedChanged
    Dim oRadioButton As RadioButton = sender
    If oRadioButton.Checked Then
      m_sSheetOrientation = optPaperOrientationLandscape.Text
      UpdateAvailableSheets()
      UpdateAvailableScales()
      WPB_Change()
    End If
  End Sub

  Private Sub optPaperOrientationPortrait_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optPaperOrientationPortrait.CheckedChanged
    Dim oRadioButton As RadioButton = sender
    If oRadioButton.Checked Then
      m_sSheetOrientation = optPaperOrientationPortrait.Text
      UpdateAvailableSheets()
      UpdateAvailableScales()
      WPB_Change()
    End If
  End Sub

  Private Sub tbMapScaleCustom_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbMapScaleCustom.TextChanged
    m_sMapScaleCustom = tbMapScaleCustom.Text
    m_sMapScale = m_sMapScaleCustom
    WPB_Change()
  End Sub

  Private Sub lbPaperSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbPaperSize.SelectedIndexChanged
    m_sSheetName = FormatStringGetSheetName(lbPaperSize.Text)
    m_sSheetSize = FormatStringGetSheetSize(lbPaperSize.Text)
    UpdateAvailableScales()
    WPB_Change()
  End Sub

  Private Sub lbType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbType.SelectedIndexChanged
    If cbType.Checked Then
      m_sType = lbType.Text
    End If
    UpdateAvailableSheets()
    UpdateAvailableScales()
    WPB_Change()
    UpdateAvailableAttributeValues()
  End Sub

  Private Sub lbMapScalePreDefined_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbMapScalePreDefined.SelectedIndexChanged
    m_sMapScalePreDefined = lbMapScalePreDefined.Text
    m_sMapScale = m_sMapScalePreDefined
    WPB_Change()
  End Sub

  Private Sub cbType_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbType.CheckedChanged
    lbType.Enabled = cbType.Checked
    If cbType.Checked Then
      m_sType = lbType.Text
    Else
      m_sType = ""
    End If
    UpdateAvailableSheets()
    UpdateAvailableScales()
    WPB_Change()
    UpdateAvailableAttributeValues()
  End Sub

  Private Sub chkInsertActiveMapWindow_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkInsertActiveMapWindow.CheckStateChanged
    WPB_Change()
  End Sub

  Private Sub UpdateAvailableAttributeValues()

    Dim strSql As String
    Dim rsRL_TEXT As ADODB.Recordset
    Dim oAttribute As Attribute
    Dim bFound As Boolean = False
    Dim oGTApplication As IGTApplication
    'Dim fontBold As New Font(dgvAttributes.DefaultCellStyle.Font.FontFamily, dgvAttributes.DefaultCellStyle.Font.Size, FontStyle.Bold)
    'Dim iRowIndex As Integer

    Try

      If String.IsNullOrEmpty(lbType.Text) Then Exit Sub


      ' If Type is not selected simply add to enter plot name
      If Not cbType.Checked Then
        DataGridViewAttributes2.ClearDataGridViewAttributes()
        DataGridViewAttributes2.RowHeadersVisible = True

        'iRowIndex = dgvAttributes.Rows.Add(Replace(StrConv(Replace(GTPlotMetadata.Parameters.PlotBoundaryAttribute_Name, "_", " "), vbProperCase), " Id", " ID"), "")
        'dgvAttributes.CurrentCell.Style.Font = fontBold

        ' Add plot name attribute placeholder
        ' Add the required Plot Name to the collection first

        m_colAttributes = New Collection
        oAttribute = New Attribute
        oAttribute = GetAttributeInfo("[" + GTPlotMetadata.Parameters.PlotBoundaryAttribute_Name + "]")
        With oAttribute
          '.G3E_FIELD = GTPlotMetadata.Parameters.PlotBoundaryAttribute_Name
          '.G3E_USERNAME = GetAttributeInfo("[" + GTPlotMetadata.Parameters.PlotBoundaryAttribute_Name + "]") 'Replace(StrConv(Replace(GTPlotMetadata.Parameters.PlotBoundaryAttribute_Name, "_", " "), vbProperCase), " Id", " ID")
          .G3E_REQUIRED = 1
        End With
        'm_colAttributes.Add(oAttribute, GTPlotMetadata.Parameters.PlotBoundaryAttribute_Name)
        m_colAttributes.Add(oAttribute, oAttribute.G3E_FIELD)
        DataGridViewAttributes2.Attributes.Add(oAttribute)

        DataGridViewAttributes2.RefreshDataGridViewAttributes()

        Exit Sub
      End If



      '
      ' Get all the attributes used by the selected Type & Sheet values
      '
      strSql = " SELECT rl_text" & LangSuffix() & ",  rl_name" & LangSuffix() & ", rl_text_default" & LangSuffix() & ",  rl_text_triggered_by "
      strSql = strSql & "  FROM " & GTPlotMetadata.Parameters.GT_PLOT_REDLINES & " "
      strSql = strSql & " WHERE rl_datatype = 'Redline Text' "
      strSql = strSql & "   AND rl_text" & LangSuffix() & " LIKE '%[%]%' "
      strSql = strSql & "   AND group_no IN ( "
      strSql = strSql & "          SELECT group_no "
      strSql = strSql & "            FROM " & GTPlotMetadata.Parameters.GT_PLOT_GROUPS_DRI & " "
      strSql = strSql & "           WHERE dri_id IN ( "
      strSql = strSql & "                    SELECT dri_id "
      strSql = strSql & "                      FROM " & GTPlotMetadata.Parameters.GT_PLOT_DRAWINGINFO & " "
      strSql = strSql & "                     WHERE dri_type" & LangSuffix() & " " & IIf(lbType.Text = "", "IS NULL", "='" & lbType.Text & "'")
      strSql = strSql & "                       AND sheet_id IN ( "
      strSql = strSql & "                              SELECT sheet_id "
      strSql = strSql & "                                FROM " & GTPlotMetadata.Parameters.GT_PLOT_SHEETS & " "
      strSql = strSql & "                               WHERE sheet_size = '" & FormatStringGetSheetSize(lbPaperSize.Text) & "' "
      strSql = strSql & "                                 AND sheet_orientation" & LangSuffix() & " = '" & IIf(optPaperOrientationLandscape.Checked, optPaperOrientationLandscape.Text, optPaperOrientationPortrait.Text) & "'))) "
      strSql = strSql & " ORDER BY RL_TEXT_ORDINAL"

      oGTApplication = GTClassFactory.Create(Of IGTApplication)()
      rsRL_TEXT = oGTApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, ADODB.CommandTypeEnum.adCmdText)



      '
      ' Look for defenition of each attribute as defined by GTech metadata for
      ' the Plot Boundary feature and it's components and/or Job Properties.
      '
      m_colAttributes = New Collection


      ' Add the required Plot Name to the collection first
      'Todo -!Get localized PlotBoundaryAttribute_Name
      oAttribute = GetAttributeInfo("[" + GTPlotMetadata.Parameters.PlotBoundaryAttribute_Name + "]")
      If Not IsNothing(oAttribute) Then
        m_colAttributes.Add(oAttribute, oAttribute.G3E_FIELD)
      End If


      ' Add redline text to DataGridView for user to enter values
      Call Repos(rsRL_TEXT)

      While Not rsRL_TEXT.EOF

        ' Find multiple fields in string...
        'oAttribute = GetAttributeInfo(rsRL_TEXT.Fields("RL_TEXT" & LangSuffix()).Value)
        Dim iFieldStart As Int16
        Dim iFieldEnd As Int16
        Dim sStartOfField As String = "["
        Dim sEndOfField As String = "]"
        Dim strText = rsRL_TEXT.Fields("RL_TEXT" & LangSuffix()).Value
        Dim strField As String
        iFieldStart = strText.IndexOf(sStartOfField)
        iFieldEnd = strText.IndexOf(sEndOfField)
        Do While (iFieldStart >= 0 And iFieldEnd > 0)
          strField = strText.Substring(iFieldStart, iFieldEnd + 1 - iFieldStart)

          ' Process each field..
          oAttribute = GetAttributeInfo(strField)
          If Not IsNothing(oAttribute) Then
            ' Check if attribute already defined - Used to filter out duplicate attributes that may exist in common components of the feature and required PlotBoundaryAttribute_Name that has already been added.
            bFound = False
            For Each oAttributeExists As Attribute In m_colAttributes
              If oAttributeExists.G3E_FIELD = oAttribute.G3E_FIELD Then bFound = True
            Next
            If bFound = False Then
              If oAttribute.G3E_USERNAME = "" Then
                oAttribute.G3E_USERNAME = IIf(IsDBNull(rsRL_TEXT.Fields("RL_NAME" & LangSuffix()).Value), rsRL_TEXT.Fields("RL_TEXT" & LangSuffix()).Value, rsRL_TEXT.Fields("RL_NAME" & LangSuffix()).Value)
              End If
              If Not IsDBNull(rsRL_TEXT.Fields("RL_TEXT_DEFAULT" & LangSuffix()).Value) Then
                oAttribute.DATA_DEFAULT = rsRL_TEXT.Fields("RL_TEXT_DEFAULT" & LangSuffix()).Value
              End If
              m_colAttributes.Add(oAttribute, oAttribute.G3E_FIELD)
            End If
          Else
            ' Use what is defined in the 
            oAttribute = New Attribute
            With oAttribute
              ' Remove leading and trailing square brackets
              strField = rsRL_TEXT.Fields("RL_TEXT" & LangSuffix()).Value
              strField = strField.Substring(1, Len(strField) - 2)
              .G3E_FIELD = strField
              .G3E_USERNAME = IIf(IsDBNull(rsRL_TEXT.Fields("RL_NAME" & LangSuffix()).Value), "", rsRL_TEXT.Fields("RL_NAME" & LangSuffix()).Value)
              .G3E_NAME = ""
              .DATA_DEFAULT = IIf(IsDBNull(rsRL_TEXT.Fields("RL_TEXT_DEFAULT" & LangSuffix()).Value), "", rsRL_TEXT.Fields("RL_TEXT_DEFAULT" & LangSuffix()).Value)
            End With
            m_colAttributes.Add(oAttribute, oAttribute.G3E_FIELD)
          End If

          strText = strText.Remove(0, iFieldEnd + 1)
          iFieldStart = strText.IndexOf(sStartOfField)
          iFieldEnd = strText.IndexOf(sEndOfField)
        Loop

        rsRL_TEXT.MoveNext()
      End While



      ' Check to see if attributes need to have a picklist populated.
      For Each oAttribute In m_colAttributes
        If Not String.IsNullOrEmpty(oAttribute.G3E_PICKLISTTABLE) Then
          oAttribute.VALUE_TYPE = Attribute.CellType.ComboBox
          oAttribute = SetPicklistValues(oAttribute)
        End If
      Next oAttribute



      ' Set triggering attributes on cell change


      ' Set default values
      For Each oAttribute In m_colAttributes
        If Not String.IsNullOrEmpty(oAttribute.DATA_DEFAULT) Then
          oAttribute.VALUE = oAttribute.DATA_DEFAULT
        End If
      Next oAttribute



      ' Add attributes to DataGridView
      DataGridViewAttributes2.ClearDataGridViewAttributes()
            DataGridViewAttributes2.Attributes.Clear()
            DataGridViewAttributes2.RowHeadersVisible = True
            'DataGridViewAttributes2.Font = New Font(DataGridViewAttributes2.Font.Name, DataGridViewAttributes2.Font.Size - 1, FontStyle.Regular)
            For Each oAttribute In m_colAttributes
                Try                       'dch
                    If Not String.IsNullOrEmpty(oAttribute.G3E_NAME) Then     'dch
                        DataGridViewAttributes2.Attributes.Add(oAttribute)
                    End If                'dch
                Catch tmpEx As Exception  'dch
                    MsgBox("Error adding to datagrid:" & oAttribute.G3E_NAME.ToString & ":" & tmpEx.Message, vbOKOnly, tmpEx.Source)   'dch
                End Try                   'dch
            Next oAttribute
            DataGridViewAttributes2.RefreshDataGridViewAttributes()

        Catch ex As Exception
      MsgBox("NewPlotWindowForm.UpdateAvailableAttributeValues:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)

    Finally
      'fontBold.Dispose()

    End Try
  End Sub

  Private Function SetPicklistValues(ByVal oAttribute As Attribute) As Attribute

    Dim strSql As String
    Dim rsAttributes As ADODB.Recordset
    Dim oGTApplication As IGTApplication

    Try

      ' Get picklist values
      strSql = " SELECT " & oAttribute.G3E_VALUEFIELD & IIf(String.IsNullOrEmpty(oAttribute.G3E_ADDITIONALFIELDS), " ", ", " & oAttribute.G3E_ADDITIONALFIELDS & " ")
      strSql = strSql & "  FROM " & oAttribute.G3E_PICKLISTTABLE & " "
      IIf(oAttribute.G3E_ORDERBYKEY = "", strSql = strSql & "", strSql = strSql & " ORDER BY " & oAttribute.G3E_ORDERBYKEY)

      oGTApplication = GTClassFactory.Create(Of IGTApplication)()

      ' Todo -retreive publish value to select from DDC files instead.
      'If oAttribute.G3E_PUBLISH = 1 Then
      'rsAttributes = oGTApplication.DataContext.MetadataRecordset(strSql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText)
      'Else
      rsAttributes = oGTApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText)
      'End If
      Repos(rsAttributes)

      ' Retreive filter value
      rsAttributes.Filter = oAttribute.G3E_FILTER

      While Not rsAttributes.EOF
        If String.IsNullOrEmpty(oAttribute.G3E_ADDITIONALFIELDS) Then
          oAttribute.VALUE_LIST.Add(rsAttributes.Fields(oAttribute.G3E_VALUEFIELD).Value)
        Else
          ' Todo -Create new drop down list control that support multiple columns
          oAttribute.VALUE_LIST.Add(rsAttributes.Fields(oAttribute.G3E_VALUEFIELD).Value) ' & " | " & rsAttributes.Fields(oAttribute.G3E_ADDITIONALFIELDS).Value)
        End If
        rsAttributes.MoveNext()
      End While


    Catch ex As Exception
      MsgBox("NewPlotWindowForm.SetPicklistValues:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      oAttribute = Nothing

    Finally
      SetPicklistValues = oAttribute
    End Try
  End Function


  Private Function GetAttributeInfo(ByVal strText As String) As Attribute

    Static strSql As String
    Static rsAttributes As ADODB.Recordset
    Static oGTJobDefenition As GTJobDefinition
    Static rsJobAttributes As ADODB.Recordset
    Dim strField As String
    Dim oAttribute As Attribute = Nothing
    Dim oGTApplication As IGTApplication

    Try

      oGTApplication = GTClassFactory.Create(Of IGTApplication)()

      If String.IsNullOrEmpty(strSql) Then

        ' Get Attribute info from metadata
        strSql = " SELECT * "
        strSql = strSql & "  FROM g3e_attributeinfo_optlang "
        strSql = strSql & " WHERE g3e_cno IN (SELECT g3e_cno "
        strSql = strSql & "                     FROM g3e_featurecomps_optable "
        strSql = strSql & "                    WHERE g3e_fno = " & GTPlotMetadata.Parameters.PlotBoundary_G3E_FNO & ") "
        strSql = strSql & " AND G3E_LCID = '" & GetLanguage() & "' "
        strSql = strSql & " ORDER BY G3E_REQUIRED DESC"

        rsAttributes = oGTApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText)

      End If


      Repos(rsAttributes)

      ' Remove leading and trailing square brackets
      strField = strText.Substring(1, Len(strText) - 2)


      oAttribute = GetAttributeInfoForField(strField, rsAttributes)

      ' Set Active CLLI
      'Todo -Store in metadate the active CLLI column throughout the system. Add GT_PLOT_AOILOCATIONIDENTIFIER_FIELDNAME to the GT_PLOT_PARAMETER metadata table.
      If Not oAttribute Is Nothing Then
        If strField = "SWITCH_CENTRE_CLLI" Then
          oAttribute.DATA_DEFAULT = oGTApplication.AOILocationIdentifier
        End If
      End If


      If oAttribute Is Nothing Then
        ' Check for attribute in Job Properties
        If oGTJobDefenition Is Nothing Then oGTJobDefenition = New GTJobDefinition

        rsJobAttributes = oGTJobDefenition.GetJobAttributes()
        oAttribute = GetAttributeInfoForField(strField, rsJobAttributes)

        If Not oAttribute Is Nothing Then
          ' Todo -!!!! Set Active Job Attributes, use current selected Job and not Active.
          oAttribute.DATA_DEFAULT = oGTJobDefenition.GetJobAttributeValueFromField(oGTApplication.DataContext.ActiveJob, oAttribute.G3E_FIELD)
        End If

      End If

      If oAttribute Is Nothing Then
        ' Check for attribute in Job Properties component not on dialog
        If oGTJobDefenition Is Nothing Then oGTJobDefenition = New GTJobDefinition

        rsJobAttributes = oGTJobDefenition.GetJobAttributesNotOnDialog()
        oAttribute = GetAttributeInfoForField(strField, rsJobAttributes)

        If Not oAttribute Is Nothing Then
          ' Todo -!!!! Set Active Job Attributes, use current selected Job and not Active.
          oAttribute.DATA_DEFAULT = oGTJobDefenition.GetJobAttributeValueFromField(oGTApplication.DataContext.ActiveJob, oAttribute.G3E_FIELD)
        End If

      End If

    Catch ex As Exception
      MsgBox("NewPlotWindowForm.GetAttributeInfo:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      oAttribute = Nothing

    Finally
      GetAttributeInfo = oAttribute
    End Try
  End Function

  Private Function GetAttributeInfoForField(ByVal strField As String, ByVal rsAttributes As ADODB.Recordset) As Attribute

    Dim oAttribute As Attribute = Nothing

    Try
      Repos(rsAttributes)
      rsAttributes.Filter = "G3E_FIELD='" & strField & "'"
      If Not rsAttributes.EOF Then
        oAttribute = New Attribute
        With oAttribute
          .G3E_FIELD = IIf(IsDBNull(rsAttributes.Fields("G3E_FIELD").Value), "", rsAttributes.Fields("G3E_FIELD").Value)
          .G3E_USERNAME = IIf(IsDBNull(rsAttributes.Fields("G3E_USERNAME").Value), "", rsAttributes.Fields("G3E_USERNAME").Value)
          .G3E_FORMAT = IIf(IsDBNull(rsAttributes.Fields("G3E_FORMAT").Value), "", rsAttributes.Fields("G3E_FORMAT").Value)
          .G3E_PRECISION = IIf(IsDBNull(rsAttributes.Fields("G3E_PRECISION").Value), 0, rsAttributes.Fields("G3E_PRECISION").Value)
          If Not IsDBNull(rsAttributes.Fields("G3E_PNO").Value) Then .G3E_PRECISION = rsAttributes.Fields("G3E_PNO").Value
          .G3E_ADDITIONALREFFIELDS = IIf(IsDBNull(rsAttributes.Fields("G3E_ADDITIONALREFFIELDS").Value), "", rsAttributes.Fields("G3E_ADDITIONALREFFIELDS").Value)
          .G3E_PICKLISTTABLE = IIf(IsDBNull(rsAttributes.Fields("G3E_PICKLISTTABLE").Value), "", rsAttributes.Fields("G3E_PICKLISTTABLE").Value)
          .G3E_KEYFIELD = IIf(IsDBNull(rsAttributes.Fields("G3E_KEYFIELD").Value), "", rsAttributes.Fields("G3E_KEYFIELD").Value)
          .G3E_VALUEFIELD = IIf(IsDBNull(rsAttributes.Fields("G3E_VALUEFIELD").Value), "", rsAttributes.Fields("G3E_VALUEFIELD").Value)
          .G3E_FILTER = IIf(IsDBNull(rsAttributes.Fields("G3E_FILTER").Value), "", rsAttributes.Fields("G3E_FILTER").Value)
          .G3E_ADDITIONALFIELDS = IIf(IsDBNull(rsAttributes.Fields("G3E_ADDITIONALFIELDS").Value), "", rsAttributes.Fields("G3E_ADDITIONALFIELDS").Value)
          .G3E_NAME = IIf(IsDBNull(rsAttributes.Fields("G3E_NAME").Value), "", rsAttributes.Fields("G3E_NAME").Value)
          .DATA_DEFAULT = IIf(IsDBNull(rsAttributes.Fields("DATA_DEFAULT").Value), "", rsAttributes.Fields("DATA_DEFAULT").Value)
          .G3E_REQUIRED = IIf(IsDBNull(rsAttributes.Fields("G3E_REQUIRED").Value), 0, rsAttributes.Fields("G3E_REQUIRED").Value)
          .G3E_ORDERBYKEY = IIf(IsDBNull(rsAttributes.Fields("G3E_ORDERBYKEY").Value), "", rsAttributes.Fields("G3E_ORDERBYKEY").Value)
          .G3E_PUBLISH = IIf(IsDBNull(rsAttributes.Fields("G3E_PUBLISH").Value), 0, rsAttributes.Fields("G3E_PUBLISH").Value)
        End With
      End If

    Catch ex As Exception
      MsgBox("NewPlotWindowForm.GetAttributeInfoForField:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      oAttribute = Nothing

    Finally
      GetAttributeInfoForField = oAttribute
    End Try
  End Function

#End Region

  Private Sub imgPageContainer_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles imgPageContainer.Paint


    Try


      '' Create a local version of the graphics object for the PictureBox.
      Dim g As Graphics = e.Graphics
      Dim oPoint1 As System.Drawing.PointF
      Dim oPoint2 As System.Drawing.PointF

      g.Clear(Color.FromKnownColor(KnownColor.Highlight)) ' Was using RGB=0,192,192 or DeepSkyBlue or CadetBlue
      g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
      g.PageUnit = GraphicsUnit.Pixel

      Dim dPgContainerScale As Double
      If m_sSheetOrientation = optPaperOrientationLandscape.Text Then
        dPgContainerScale = (imgPageContainer.Width / m_dSheetWidth) * 0.45
      Else
        dPgContainerScale = (imgPageContainer.Height / m_dSheetHeight) * 0.9
      End If
      'g.PageScale = dPgContainerScale

      Dim oRectPaper As System.Drawing.Rectangle
      Dim oRectBorder As System.Drawing.Rectangle
      Dim oRectMapWindow As System.Drawing.Rectangle

      ' Draw Page
      oRectPaper = New System.Drawing.Rectangle
      oRectPaper.X = (imgPageContainer.Width - (m_dSheetWidth * dPgContainerScale)) / 2
      oRectPaper.Y = (imgPageContainer.Height - (m_dSheetHeight * dPgContainerScale)) / 2
      oRectPaper.Height = m_dSheetHeight * dPgContainerScale
      oRectPaper.Width = m_dSheetWidth * dPgContainerScale
      g.DrawRectangle(Pens.Black, oRectPaper)
      g.FillRectangle(Brushes.White, oRectPaper)

      ' Draw Border
      If Not m_dSheet_Inset = 0 Then
        oRectBorder = New System.Drawing.Rectangle
        oRectBorder.X = oRectPaper.X + (m_dSheet_Inset * dPgContainerScale)
        oRectBorder.Y = oRectPaper.Y + (m_dSheet_Inset * dPgContainerScale)
        oRectBorder.Height = oRectPaper.Height - ((m_dSheet_Inset * 2) * dPgContainerScale)
        oRectBorder.Width = oRectPaper.Width - ((m_dSheet_Inset * 2) * dPgContainerScale)
        g.DrawRectangle(Pens.Black, oRectBorder)
      End If


      ' Draw Map Frames
      Repos(m_rsMapFrames)
      While Not m_rsMapFrames.EOF

        oRectMapWindow = New System.Drawing.Rectangle

        m_oMapTLPoint.X = m_rsMapFrames.Fields("MF_COORDINATE_X1").Value + m_rsMapFrames.Fields("GROUP_OFFSET_X").Value
        m_oMapTLPoint.Y = m_rsMapFrames.Fields("MF_COORDINATE_Y1").Value + m_rsMapFrames.Fields("GROUP_OFFSET_Y").Value
        m_oMapBRPoint.X = m_rsMapFrames.Fields("MF_COORDINATE_X2").Value + m_rsMapFrames.Fields("GROUP_OFFSET_X").Value
        m_oMapBRPoint.Y = m_rsMapFrames.Fields("MF_COORDINATE_Y2").Value + m_rsMapFrames.Fields("GROUP_OFFSET_Y").Value

        oRectMapWindow.X = oRectPaper.X + (m_oMapTLPoint.X * dPgContainerScale)
        oRectMapWindow.Y = oRectPaper.Y + (m_oMapTLPoint.Y * dPgContainerScale)
        oRectMapWindow.Height = (m_oMapBRPoint.Y - m_oMapTLPoint.Y) * dPgContainerScale
        oRectMapWindow.Width = (m_oMapBRPoint.X - m_oMapTLPoint.X) * dPgContainerScale

        If Not (TabControl1.SelectedIndex = 1 And chkInsertActiveMapWindow.Checked = False) Then
          ' Remove the painting of the Map in the preview if no map is being inserted.
          g.DrawImage(ImageList1.Images(0), oRectMapWindow)
        End If
        g.DrawRectangle(Pens.Black, oRectMapWindow)

        m_rsMapFrames.MoveNext()
      End While




      ' Draw Key Map Frames
      Repos(m_rsKeyMapFrames)
      While Not m_rsKeyMapFrames.EOF

        oRectMapWindow = New System.Drawing.Rectangle

        m_oMapTLPoint.X = m_rsKeyMapFrames.Fields("MF_COORDINATE_X1").Value + m_rsKeyMapFrames.Fields("GROUP_OFFSET_X").Value
        m_oMapTLPoint.Y = m_rsKeyMapFrames.Fields("MF_COORDINATE_Y1").Value + m_rsKeyMapFrames.Fields("GROUP_OFFSET_Y").Value
        m_oMapBRPoint.X = m_rsKeyMapFrames.Fields("MF_COORDINATE_X2").Value + m_rsKeyMapFrames.Fields("GROUP_OFFSET_X").Value
        m_oMapBRPoint.Y = m_rsKeyMapFrames.Fields("MF_COORDINATE_Y2").Value + m_rsKeyMapFrames.Fields("GROUP_OFFSET_Y").Value

        oRectMapWindow.X = oRectPaper.X + (m_oMapTLPoint.X * dPgContainerScale)
        oRectMapWindow.Y = oRectPaper.Y + (m_oMapTLPoint.Y * dPgContainerScale)
        oRectMapWindow.Height = (m_oMapBRPoint.Y - m_oMapTLPoint.Y) * dPgContainerScale
        oRectMapWindow.Width = (m_oMapBRPoint.X - m_oMapTLPoint.X) * dPgContainerScale

        g.DrawImage(ImageList1.Images(0), oRectMapWindow)
        g.DrawRectangle(Pens.Black, oRectMapWindow)

        m_rsKeyMapFrames.MoveNext()
      End While




      ' Draw the border a little thicker
      oRectBorder.Inflate(-0.25 * dPgContainerScale, -0.25 * dPgContainerScale)
      g.DrawRectangle(Pens.Black, oRectBorder)
      oRectBorder.Inflate(0.5 * dPgContainerScale, 0.5 * dPgContainerScale)
      g.DrawRectangle(Pens.Black, oRectBorder)

      Repos(m_rsRedlines)
      While Not m_rsRedlines.EOF
        oPoint1.X = oRectPaper.X + (m_rsRedlines.Fields("GROUP_OFFSET_X").Value + m_rsRedlines.Fields("RL_COORDINATE_X1").Value) * dPgContainerScale
        oPoint1.Y = oRectPaper.Y + (m_rsRedlines.Fields("GROUP_OFFSET_Y").Value + m_rsRedlines.Fields("RL_COORDINATE_Y1").Value) * dPgContainerScale
        oPoint2.X = oRectPaper.X + (m_rsRedlines.Fields("GROUP_OFFSET_X").Value + m_rsRedlines.Fields("RL_COORDINATE_X2").Value) * dPgContainerScale
        oPoint2.Y = oRectPaper.Y + (m_rsRedlines.Fields("GROUP_OFFSET_Y").Value + m_rsRedlines.Fields("RL_COORDINATE_Y2").Value) * dPgContainerScale
        g.DrawLine(Pens.Black, oPoint1, oPoint2)
        m_rsRedlines.MoveNext()
      End While
      g.Flush()


      '' FUTURE
      '' Draw a string on the PictureBox.
      'g.DrawString("This is a diagonal line drawn on the control", _
      '    New Font("Arial", 10), Brushes.Red, New PointF(0.0F, 0.0F))
      '' Draw a line in the PictureBox.
      'g.DrawLine(System.Drawing.Pens.Red, imgPaper.Left, _
      '    imgPaper.Top, imgPaper.Right, imgPaper.Bottom)


    Catch ex As Exception
      MsgBox("PlotBoundaryForm.imgPageContainer_Paint:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    End Try

  End Sub


#End Region


  Private Sub lblActiveSessionInfoCapitalWorkOrderNumberValue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblActiveSessionInfoCapitalWorkOrderNumberValue.Click

  End Sub

  Private Sub cmdTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Test.Click

    Dim oGTApplication As IGTApplication
    Dim oGTPoint As IGTPoint
    Dim oGTVector As IGTVector
    Dim dAngle As Double = 45
    Dim oGTPlotRedline As IGTPlotRedline

    Try
      Hide()

      oGTApplication = GTClassFactory.Create(Of IGTApplication)()
      oGTApplication.BeginWaitCursor()

      ' Generate GUID
      Dim sGUID As String
      sGUID = System.Guid.NewGuid.ToString()

      ' Name Plot and Map Frame
      Dim sName As String
      sName = "Test-" & sGUID

      ' Get the Active Map Window's Detail ID
      Dim iDetailID As Integer
      Dim oMapWindow As IGTMapWindow
      oMapWindow = oGTApplication.ActiveMapWindow
      iDetailID = oMapWindow.DetailID

      ' Create New Named Plot
      Dim oGTNamedPlot As IGTNamedPlot
      oGTNamedPlot = oGTApplication.NewNamedPlot(sName)

      ' Set Plot Page Size
      oGTNamedPlot.PaperHeight = 11000
      oGTNamedPlot.PaperWidth = 15000

      ' Create New Plot Window
      Dim oGTPlotWindow As IGTPlotWindow
      oGTPlotWindow = oGTApplication.NewPlotWindow(oGTNamedPlot)


      If (0) Then

        '
        ' IGTPolylineGeometry
        '
        Dim oPolylineGeometry As IGTPolylineGeometry
        oPolylineGeometry = GTClassFactory.Create(Of IGTPolylineGeometry)()

        oGTPoint = GTClassFactory.Create(Of IGTPoint)()
        oGTPoint.X = 1000
        oGTPoint.Y = 500
        oGTPoint.Z = 0
        oPolylineGeometry.Points.Add(oGTPoint)

        oGTPoint = GTClassFactory.Create(Of IGTPoint)()
        oGTPoint.X = 15000
        oGTPoint.Y = 5000
        oGTPoint.Z = 0
        oPolylineGeometry.Points.Add(oGTPoint)

        oGTPlotRedline = oGTNamedPlot.NewRedline(oPolylineGeometry, 27302111)

      End If



      '
      ' IGTPoint
      '
      oGTPoint = GTClassFactory.Create(Of IGTPoint)()
      oGTPoint.X = 3000
      oGTPoint.Y = 3000
      oGTPoint.Z = 0

      Dim oOrientedPointGeometry As IGTOrientedPointGeometry
      oOrientedPointGeometry = GTClassFactory.Create(Of IGTOrientedPointGeometry)()
      oOrientedPointGeometry.Origin = oGTPoint

      oGTVector = GTClassFactory.Create(Of IGTVector)()
      oGTVector.I = Math.Cos(dAngle)
      oGTVector.J = Math.Sin(dAngle)
      oGTVector.K = 0
      oOrientedPointGeometry.Orientation = oGTVector

      'oGTPlotRedline = oGTNamedPlot.NewRedline(oOrientedPointGeometry, 4022001)
      'oGTPlotRedline = oGTNamedPlot.NewRedline(oOrientedPointGeometry, 5020)
      'oGTPlotRedline = oGTNamedPlot.NewRedline(oOrientedPointGeometry, 220029)
      oGTPlotRedline = oGTNamedPlot.NewRedline(oOrientedPointGeometry, 7020007)



      If (0) Then
        '
        ' IGTTextPointGeometry
        '
        oGTPoint = GTClassFactory.Create(Of IGTPoint)()
        oGTPoint.X = 5000
        oGTPoint.Y = 5000
        oGTPoint.Z = 0

        oGTVector = GTClassFactory.Create(Of IGTVector)()
        oGTVector.I = Math.Cos(dAngle)
        oGTVector.J = Math.Sin(dAngle)
        oGTVector.K = 0

        Dim oTextPointGeometry As IGTTextPointGeometry
        oTextPointGeometry = GTClassFactory.Create(Of IGTTextPointGeometry)()
        With oTextPointGeometry
          .Origin = oGTPoint
          .Normal = oGTVector
          .Rotation = 45
          .Alignment = GTAlignmentConstants.gtalCenterLeft
          .Text = "IGTTextPointGeometry Test"
        End With

        oGTPlotRedline = oGTNamedPlot.NewRedline(oTextPointGeometry, 101)


        '
        ' Insert Map Frame
        '
        Dim oPlotMap As IGTPlotMap
        Dim oGTPaperRange As IGTPaperRange

        oGTApplication = GTClassFactory.Create(Of IGTApplication)()

        oGTPaperRange = GTClassFactory.Create(Of IGTPaperRange)()

        oGTPoint = GTClassFactory.Create(Of IGTPoint)()
        oGTPoint.X = 1010
        oGTPoint.Y = 510 + 5000
        oGTPoint.Z = 0

        oGTPaperRange.TopLeft = oGTPoint

        oGTPoint = GTClassFactory.Create(Of IGTPoint)()
        oGTPoint.X = 14990
        oGTPoint.Y = 4990 + 5000
        oGTPoint.Z = 0

        oGTPaperRange.BottomRight = oGTPoint

        oPlotMap = oGTApplication.ActivePlotWindow.InsertMap(oGTPaperRange)
        oPlotMap.Frame.Name = sName


        '
        ' Insert Map Frame Content
        '
        If iDetailID = 0 Then 'Plot Boundary is in Geo
          oPlotMap.DisplayService.ReplaceLegend("Electric Design Legend")
        Else
          oPlotMap.DisplayService.ReplaceLegend("Electric Detail Legend", iDetailID)
        End If

      End If


      Dispose()

    Catch ex As Exception
      MsgBox("NewPlotWindowForm.cmdTest:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)

    Finally
      oGTApplication = GTClassFactory.Create(Of IGTApplication)()
      oGTApplication.EndWaitCursor()
      Dispose()

    End Try

  End Sub


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


  Private Sub DataGridViewAttributes2_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs, ByVal oAttribute As Attribute) Handles DataGridViewAttributes2.CellValueChanged

    If oAttribute.G3E_USERNAME = My.Resources.NewPlotWindow.UpdatePlotBoundaryInfo_Legend Then
      Dim oPlotBoundary As PlotBoundary
      oPlotBoundary = m_colPlotBoundaries(tvSelectedPlotBoundaries.SelectedNode.Name)
      oPlotBoundary.Legend = oAttribute.VALUE
      oPlotBoundary.LegendOverrides = Nothing
      UpdatePlotBoundaryInfo(tvSelectedPlotBoundaries.SelectedNode.Name)
    End If

  End Sub

End Class