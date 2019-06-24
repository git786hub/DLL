Imports System
Imports Microsoft.Win32
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports Intergraph.GTechnology.API
Imports System.Drawing



Public Class PrintActiveMapWindowForm


  ' Setup dialog labels based on metadata
  Dim oJobDefinition As New GTJobDefinition

  Private m_colPlotBoundaries As Collection
  Private m_colAttributes As Collection

  Private m_HideDialog As Boolean = False

  Enum MeasurmentUnits
    Metric
    Imperial
  End Enum

  Public Property HideDialog() As Boolean
    Get
      HideDialog = m_HideDialog
    End Get
    Set(ByVal value As Boolean)
      m_HideDialog = value
    End Set
  End Property

  ' Save settings
  Private Sub Save()
    Try
      Dim RegKey As RegistryKey = Registry.CurrentUser.OpenSubKey(My.Resources.RegistryCurrentUserSubKey, True)
      If IsNothing(RegKey) Then RegKey = Registry.CurrentUser.CreateSubKey(My.Resources.RegistryCurrentUserSubKey, RegistryKeyPermissionCheck.ReadWriteSubTree)
      RegKey.SetValue(Me.Name & ".Top", Me.Top)
      RegKey.SetValue(Me.Name & ".Left", Me.Left)
      RegKey.SetValue(Me.Name & ".optPaperOrientationPortrait.Checked", Me.optPaperOrientationPortrait.Checked)
      RegKey.SetValue(Me.Name & ".optPaperOrientationPortrait.Checked", optPaperOrientationPortrait.Checked)
      RegKey.SetValue(Me.Name & ".lbPaperSize.Text", lbPaperSize.Text)
      RegKey.SetValue(Me.Name & ".lbLegend.Text", lbLegend.Text)
      RegKey.SetValue(Me.Name & ".chkBoxMapBackgroundToWhite.Checked", chkBoxMapBackgroundToWhite.Checked)
      RegKey.Close()

    Catch ex As Exception
    End Try
  End Sub

    ' Restore settings
    Private Sub Restore()

        Dim oGTApplication As IGTApplication

        Try
            Dim RegKey As RegistryKey = Registry.CurrentUser.OpenSubKey(My.Resources.RegistryCurrentUserSubKey)
            If IsNothing(RegKey) Then RegKey = Registry.CurrentUser.CreateSubKey(My.Resources.RegistryCurrentUserSubKey, RegistryKeyPermissionCheck.ReadWriteSubTree)

            If Not IsNothing(RegKey.GetValue(Me.Name & ".Top")) Then
                Me.Top = RegKey.GetValue(Me.Name & ".Top")
                Me.Left = RegKey.GetValue(Me.Name & ".Left")
            End If

            If Not IsNothing(RegKey.GetValue(Me.Name & ".optPaperOrientationPortrait.Checked")) Then
                If RegKey.GetValue(Me.Name & ".optPaperOrientationPortrait.Checked") Then
                    Me.optPaperOrientationPortrait.Checked = True
                    m_sSheetOrientation = optPaperOrientationPortrait.Text
                Else
                    Me.optPaperOrientationLandscape.Checked = True
                    m_sSheetOrientation = optPaperOrientationLandscape.Text
                End If
            End If

            If Not IsNothing(RegKey.GetValue(Me.Name & ".lbPaperSize.Text")) Then
                lbPaperSize.Text = RegKey.GetValue(Me.Name & ".lbPaperSize.Text")
            Else
                If lbPaperSize.Items.Count > 0 Then
                    lbPaperSize.SetSelected(0, True)
                End If
            End If

            Try
                If Not IsNothing(RegKey.GetValue(Me.Name & ".lbLegend.Text")) Then
                    lbLegend.Text = RegKey.GetValue(Me.Name & ".lbLegend.Text")
                Else
                    oGTApplication = GTClassFactory.Create(Of IGTApplication)()
                    lbLegend.Text = oGTApplication.ActiveMapWindow.LegendName()
                End If
            Catch ex As Exception
            End Try

            Try
                If Not IsNothing(RegKey.GetValue(Me.Name & ".chkBoxMapBackgroundToWhite.Checked")) Then
                    If RegKey.GetValue(Me.Name & ".chkBoxMapBackgroundToWhite.Checked") Then
                        Me.chkBoxMapBackgroundToWhite.Checked = True
                    Else
                        Me.chkBoxMapBackgroundToWhite.Checked = True
                    End If
                End If
            Catch ex As Exception
            End Try

            RegKey.Close()

        Catch ex As Exception
        End Try
    End Sub


    Private Sub PrintActiveMapWindowForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim rsJobData As ADODB.Recordset = Nothing
        Dim strSql As String = ""
        Dim rsUserName As ADODB.Recordset = Nothing
        Dim rsUserNames As ADODB.Recordset = Nothing
        Dim strActiveJobOwner As String = ""

        Dim oGTApplication As IGTApplication

        Try

#If DEBUG Then
            'Test.Enabled = True
            'Test.Visible = True
            'Test.Text = "Test"
#End If

            If HideDialog Then Me.Visible = False


            oGTApplication = GTClassFactory.Create(Of IGTApplication)()

            '
            ' Clear the DataGridViewAttributes1
            '
            'DataGridViewAttributes1.Attributes = New Attributes
            'DataGridViewAttributes1.RefreshDataGridViewAttributes()


            '
            ' Active Session Information
            '
            lblActiveSessionInfoUserValue.Text = oGTApplication.DataContext.DatabaseUserName
            'lblDateValue.Text = Date
            'cbPlotBoundaryFilterUser.Text = cbPlotBoundaryFilterUser.Items.Item(0)
            chkBoxMapBackgroundToWhite.Text = My.Resources.NewPlotWindow.UpdatePlotBoundaryInfo_ForceMapBackgroundToWhite

            '
            ' User Defined Print Settings
            '
            Static DefaultsAlreadyLoaded As Boolean = False

            If Not DefaultsAlreadyLoaded Then
                'LoadDefaults()

                'Get First Sheet
                Dim bPaperFound As Boolean
                Dim rsPaperSizes As ADODB.Recordset
                m_sType = "PrintActiveMapWindow"
                rsPaperSizes = GetPaperSizes(m_sType, "", False)
                While Not rsPaperSizes.EOF
                    bPaperFound = True
                    m_sSheetName = rsPaperSizes.Fields("SHEET_NAME" & LangSuffix()).Value
                    m_sSheetOrientation = rsPaperSizes.Fields("SHEET_ORIENTATION").Value
                    Exit While
                End While

                rsPaperSizes.Close()

                DefaultsAlreadyLoaded = True
            End If

            If m_sSheetOrientation = "Portrait" Then
                optPaperOrientationPortrait.Checked = True
            Else
                optPaperOrientationLandscape.Checked = True
            End If


            UpdateAvailableSheets()

            LoadDefaults()

            UpdateButtonsStates()

        Catch ex As Exception
            MsgBox("PrintActiveMapWindowForm.Load:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)

        Finally
            If Not (rsJobData Is Nothing) Then
                rsJobData = Nothing
            End If

        End Try

    End Sub

    Private Sub cmdPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPrint.Click
        PrintActiveMapWindowPrint()
    End Sub

    Public Sub PrintActiveMapWindowPrint()

    Dim sStatusBarText As String = ""
    Dim oGTApplication As IGTApplication = Nothing
    Dim oPlotBoundary As PlotBoundary
    Dim oAttribute As Attribute
    Dim oDataGridViewRow As DataGridViewRow

    Try

      oGTApplication = GTClassFactory.Create(Of IGTApplication)()
      sStatusBarText = oGTApplication.GetStatusBarText(GTStatusPanelConstants.gtaspcMessage)

      oGTApplication.BeginWaitCursor()


      'Save the user entered data to the registry
      oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Saving Settings...") ' User feedback
      Save()


      ' Create a new PlotBoundary object.
      oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "Creating Plot Window: " & "TempPlotWindow") ' User feedback
      oPlotBoundary = New PlotBoundary
      oPlotBoundary.Source = "PrintActiveMapWindow"
      oPlotBoundary.Name = m_sType
      Dim sPlotNameUserName As String = "Plot Name"

      If (chkBoxMapBackgroundToWhite.Checked) Then
        oPlotBoundary.ForceMapBackgroundToWhite = "YES"
      Else
        oPlotBoundary.ForceMapBackgroundToWhite = "NO"
      End If


      ' Retrieve the user entered values.
      For Each oDataGridViewRow In dgvAttributes.Rows
        oAttribute = m_colAttributes.Item(oDataGridViewRow.Index + 1)
        oAttribute.VALUE = oDataGridViewRow.Cells(1).Value

        ' Store user entered plot name
        If oAttribute.G3E_FIELD.ToUpper = GTPlotMetadata.Parameters.PlotBoundaryAttribute_Name.ToUpper Then
          oPlotBoundary.Name = oDataGridViewRow.Cells(1).Value
          sPlotNameUserName = oAttribute.G3E_USERNAME
        End If

      Next oDataGridViewRow

      ' Store the collection of attributes and values for later use
      oPlotBoundary.Attributes = m_colAttributes

      ' Verify that the plot name has been defined.
      If oPlotBoundary.Name = "" Then
        Beep()
        MsgBox("Please provide a " & sPlotNameUserName & ".", vbOKOnly, "Plot Name")
        Exit Sub
      End If

      ' Verify that the a named plot with the same name doesn't already exist.
      If DoesNamedPlotExist(oPlotBoundary.Name) Then
        Beep()
        MsgBox("The named plot " & oPlotBoundary.Name & " already exists.  Please provide a different " & sPlotNameUserName & ".", vbOKOnly, "Named Plot Already Exists")
        Exit Sub
      End If

      ' Setup the PlotBoundary object.
      With oPlotBoundary
        .UserDefined = True
        .Type = m_sType
        .PaperName = FormatStringGetSheetName(lbPaperSize.Text)
        .PaperSize = FormatStringGetSheetSize(lbPaperSize.Text)
        .PaperOrientation = IIf(optPaperOrientationPortrait.Checked, optPaperOrientationPortrait.Text, optPaperOrientationLandscape.Text)
        .InsertActiveMapWindow = True
        .MapScale = lblSetMapScaleValue.Text

        If .InsertActiveMapWindow Then
          If oGTApplication.ActiveMapWindow Is Nothing Then
            MsgBox("Selected to Insert Active Map Window but a Map Window is not active.")
            Exit Sub
          End If
          .Legend = lbLegend.Text
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
      Dim oGTPlotWindow As IGTPlotWindow
      oGTPlotWindow = CreateNewNamedPlot(oPlotBoundary)

      ' Create the redline template for the new plot window and insert the map frame(s)
      Call StartDrawingRedlines(oPlotBoundary)

      'dch test code
      'to-do :  setup checkbox on form, create global variable for forced-white-background
      If (oPlotBoundary.ForceMapBackgroundToWhite = "YES") Then 'PA -Added default parameter.
        Dim oGTNamedPlot As IGTNamedPlot
        oGTNamedPlot = oGTApplication.NamedPlots(oGTApplication.ActivePlotWindow.Caption)
        For iFrame As Integer = 1 To oGTNamedPlot.Frames.Count
          oGTNamedPlot.Frames.Item(iFrame - 1).Activate()
          oGTNamedPlot.Frames.Item(iFrame - 1).PlotMap.BackColor = Color.White
          oGTNamedPlot.Frames.Item(iFrame - 1).Deactivate()
        Next
      End If
      'end dch test code

      ' Print
      If MsgBox("Would you like to print the active plot window?", MsgBoxStyle.OkCancel, "Print Preview") = MsgBoxResult.Ok Then
        'MsgBox("Printing...", MsgBoxStyle.Information, "Printing...")
        PrintActiveMapWindow()
      End If

      'Close and Delete Plot
      oGTPlotWindow.Close()
      'Application.DoEvents() crashes gtech!
      oGTApplication.NamedPlots.Remove(oPlotBoundary.Name)

      Dispose()

    Catch ex As Exception
      MsgBox("PrintActiveMapWindowForm.PrintActiveMapWindowPrint:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)

    Finally
      oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, sStatusBarText)
      oGTApplication.EndWaitCursor()

    End Try

  End Sub

  Public Sub PrintActiveMapWindow()

    Dim oGTNamedPlot As IGTNamedPlot
    Dim oGTPlotWindow As IGTPlotWindow = Nothing
    Dim oPrintProperties As IGTPrintProperties
    Dim oGTApplication As IGTApplication

    Try

      ' Present the user with the Print Dialog
      'If Not PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then Exit Sub

      ' PrintDialog1.PrinterSettings.PrinterName store the default printer name
      'MsgBox(PrintDialog1.PrinterSettings.PrinterName, MsgBoxStyle.Information, "Default Printer Name")

      oGTApplication = GTClassFactory.Create(Of IGTApplication)()
            oPrintProperties = GTClassFactory.Create(Of IGTPrintProperties)()
            'oPrintProperties.Copies = 1
            oGTNamedPlot = oGTApplication.NamedPlots(oGTApplication.ActivePlotWindow.Caption)
      PrintNamedPlot(oGTNamedPlot, oPrintProperties, PrintDialog1)

    Catch ex As Exception
      MsgBox("PrintActiveMapWindowForm.PrintActiveMapWindow:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    Finally
      'oListItem = Nothing
      'oGTNamedPlot = Nothing
      'oGTPlotWindow = Nothing
      'oPrintProperties = Nothing
    End Try
  End Sub

  Public Function ActiveMapWindow() As Boolean

    Dim bActiveMapWindow As Boolean = False
    Dim oGTMapWindow As IGTMapWindow
    Dim oGTMapWindows As IGTMapWindows
    Dim oGTApplication As IGTApplication

    Try

      oGTApplication = GTClassFactory.Create(Of IGTApplication)()
      oGTMapWindows = oGTApplication.GetMapWindows(GTMapWindowTypeConstants.gtapmtAll)

      For Each oGTMapWindow In oGTMapWindows
        If Not oGTMapWindow.WindowState = GTWindowStateConstants.gtmwwsRestore Then
          bActiveMapWindow = True
        End If
      Next oGTMapWindow

      If Not oGTMapWindows.Count = 0 Then
        If bActiveMapWindow Then 'GTApplication.ActivePlotWindow Is Nothing Then DOES NOT WORK
          ActiveMapWindow = True
        End If
      End If

    Catch ex As Exception
      MsgBox("PrintActiveMapWindowForm.ActiveMapWindow:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    Finally
    End Try

  End Function

  Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
    Dispose()
  End Sub

  Private Sub PrintActiveMapWindowForm_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
    If e.KeyCode = Keys.Escape Then
      Dispose()
    End If
  End Sub

  Public Sub UpdateButtonsStates()

    Dim blnEnablePrintButton As Boolean

    '
    ' Update Print button
    '
    blnEnablePrintButton = True
    cmdPrint.Visible = True

    If lbPaperSize.Text = "" Then blnEnablePrintButton = False
    If lbLegend.Text = "" Then blnEnablePrintButton = False

    If Not blnEnablePrintButton Then
      cmdPrint.Enabled = False
    Else
      cmdPrint.Enabled = True
    End If

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
  ' When the Type Sheet or Orientation changes update the scales list
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
      rsPaperSizes = GetPaperSizes(m_sType, m_sSheetOrientation, False)

      ' Save current setting and attempt to set them back once lists refresh...
      lbPaperSize.SuspendLayout()
      Dim sPaperSize As String = lbPaperSize.Text

      lbPaperSize.Items.Clear()
      While Not rsPaperSizes.EOF
        lbPaperSize.Items.Add(FormatStringSetSheetNameSize(rsPaperSizes.Fields("SHEET_NAME" & LangSuffix()).Value, rsPaperSizes.Fields("SHEET_SIZE").Value))
        rsPaperSizes.MoveNext()
      End While
      rsPaperSizes.Close()
            lbPaperSize.Refresh() '''''''''''''''''''''''''''''''''''
            ' Save current setting and attempt to set them back once lists refresh...
            If lbPaperSize.Items.Contains(sPaperSize) Then
        lbPaperSize.Text = sPaperSize
      Else
        If lbPaperSize.Items.Count > 0 Then
          lbPaperSize.SetSelected(0, True)
        End If
      End If
      lbPaperSize.ResumeLayout()

        Catch ex As Exception
      MsgBox("PrintActiveMapWindowForm.UpdateAvailableSheets:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    End Try
  End Sub

  '
  ' When the Type Sheet or Orientation changes update the Paper Size list
  '
  Private Sub UpdateAvailableSheetsOld()

    Dim rsPaperSizes As ADODB.Recordset
    Dim oGTApplication As IGTApplication

    Try

      ' Skip out if values are empty
      oGTApplication = GTClassFactory.Create(Of IGTApplication)()
      If String.IsNullOrEmpty(m_sSheetOrientation) Or IsNothing(oGTApplication.Application) Then
        Exit Sub
      End If

      'Populate PaperSize listbox
      rsPaperSizes = GetPaperSizes(m_sType, m_sSheetOrientation, False)
      lbPaperSize.Items.Clear()
      While Not rsPaperSizes.EOF
        lbPaperSize.Items.Add(FormatStringSetSheetNameSize(rsPaperSizes.Fields("SHEET_NAME").Value, rsPaperSizes.Fields("SHEET_SIZE").Value))
        rsPaperSizes.MoveNext()
      End While
      rsPaperSizes.Close()

      If lbPaperSize.Items.Count > 0 Then
        lbPaperSize.SetSelected(0, True)
      End If



    Catch ex As Exception
      MsgBox("PrintActiveMapWindowForm.UpdateAvailableSheets:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    End Try
  End Sub


  '
  ' When the WPB changes update the image page
  '
  Private Sub WPB_Change()

    Dim oGTApplication As IGTApplication

    Try
      'If the SheetName or SheetOrientation is not set exit
      oGTApplication = GTClassFactory.Create(Of IGTApplication)()
      If m_sSheetName = "" Or m_sSheetOrientation = "" Or m_sMapScale = "" Or IsNothing(oGTApplication.Application) Then Exit Sub
      'If String.IsNullOrEmpty(m_sSheetName) Or String.IsNullOrEmpty(m_sSheetOrientation) Or String.IsNullOrEmpty(m_sMapScale) Or IsNothing(oGTApplication.Application) Then Exit Sub
      'DCH 02272018 Originally changed this, then changed it back....
      If Not ValidScaleMetric(m_sMapScale) And Not ValidScaleImperial(m_sMapScale) Then
        'lblSetMapScaleValue.Font.Bold = True
        lblSetMapScaleValue.ForeColor = Color.Red ' System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red)
        Exit Sub
      End If
      'lblSetMapScaleValue.Font.Bold = False
      lblSetMapScaleValue.ForeColor = System.Drawing.SystemColors.WindowText ' System.Drawing.ColorTranslator.ToOle(System.Drawing.SystemColors.WindowText)



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
      lblSetMapScaleValue.Text = m_sMapScale

      ' Repaint the preview using newly set variables
      imgPageContainer.Refresh()


    Catch ex As Exception
      MsgBox("PrintActiveMapWindowForm.WPB_Change:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    End Try

  End Sub

  Private Sub LoadDefaults()

    Dim rs As ADODB.Recordset
    Dim oGTApplication As IGTApplication

    Try
      If String.IsNullOrEmpty(m_sSheetOrientation) Then
        Exit Sub
      End If

      'Define Type as none
      m_sType = "PrintActiveMapWindow"

      UpdateAvailableSheets()

      'Populate Legends listbox
      ' Get list of available legends
      oGTApplication = GTClassFactory.Create(Of IGTApplication)()
      rs = GetLegends(IIf(oGTApplication.ActiveMapWindow.DetailID = 0, False, True))
      lbLegend.Items.Clear()
      While Not rs.EOF
        If oGTApplication.DataContext.IsRoleGranted(rs.Fields("g3e_role").Value) Then
          lbLegend.Items.Add(rs.Fields("g3e_username").Value)
        End If
        rs.MoveNext()
      End While
      rs.Close()

      lbLegend.SetSelected(0, True)

      Restore()


      'Populate MapScale label
      If GTPlotMetadata.Parameters.MeasurementUnits = "Metric" Then
        lblSetMapScaleValue.Text = "1:" & Math.Round(oGTApplication.ActiveMapWindow.DisplayScale, 0)
      Else
        lblSetMapScaleValue.Text = "1""=" & Math.Round(oGTApplication.ActiveMapWindow.DisplayScale / 12, 0) & "'"
      End If

      ' Todo -No required if all events fire correctly... Remove
      WPB_Change()
      UpdateAvailableAttributeValues()
      'dch added above back in for debugging purposes

    Catch ex As Exception
      MsgBox("PrintActiveMapWindowForm.LoadDefaults:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    Finally
      'cleaning
    End Try
  End Sub



#Region "Control Events"

  Private Sub optPaperOrientationLandscape_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optPaperOrientationLandscape.CheckedChanged
    Dim oRadioButton As RadioButton = sender
    If oRadioButton.Checked Then
      m_sSheetOrientation = optPaperOrientationLandscape.Text
      UpdateAvailableSheets()
      WPB_Change()
    End If
  End Sub

  Private Sub optPaperOrientationPortrait_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optPaperOrientationPortrait.CheckedChanged
    Dim oRadioButton As RadioButton = sender
    If oRadioButton.Checked Then
      m_sSheetOrientation = optPaperOrientationPortrait.Text
      UpdateAvailableSheets()
      WPB_Change()
    End If
  End Sub

  Private Sub lblSetMapScaleValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblSetMapScaleValue.TextChanged
    m_sMapScaleCustom = lblSetMapScaleValue.Text
    m_sMapScale = m_sMapScaleCustom
    WPB_Change()
  End Sub

  Private Sub lbPaperSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbPaperSize.SelectedIndexChanged
    m_sSheetName = FormatStringGetSheetName(lbPaperSize.Text)
    m_sSheetSize = FormatStringGetSheetSize(lbPaperSize.Text)
    WPB_Change()
  End Sub

  Private Sub UpdateAvailableAttributeValues()

    Dim strSql As String = ""
    Dim rsRL_TEXT As ADODB.Recordset

    Dim oAttribute As Attribute
    Dim iRowIndex As Integer

    Dim bFound As Boolean = False
    Dim oGTApplication As IGTApplication

    Dim fontBold As New Font(dgvAttributes.DefaultCellStyle.Font.FontFamily, dgvAttributes.DefaultCellStyle.Font.Size, FontStyle.Bold)

    Try

      dgvAttributes.SuspendLayout()
      dgvAttributes.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
      dgvAttributes.AllowUserToResizeRows = False
      dgvAttributes.AllowUserToResizeColumns = True
      dgvAttributes.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing
      dgvAttributes.RowHeadersDefaultCellStyle.SelectionBackColor = Color.Empty
      dgvAttributes.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
      dgvAttributes.AutoSize = True
      dgvAttributes.Rows.Clear()

      '
      ' Get all the attributes used by the selected Type & Sheet values
      '
      strSql = " SELECT rl_text "
      strSql = strSql & "  FROM " & GTPlotMetadata.Parameters.GT_PLOT_REDLINES & " "
      strSql = strSql & " WHERE rl_datatype = 'Redline Text' "
      strSql = strSql & "   AND rl_text LIKE '%[%]%' "
      strSql = strSql & "   AND group_no IN ( "
      strSql = strSql & "          SELECT group_no "
      strSql = strSql & "            FROM " & GTPlotMetadata.Parameters.GT_PLOT_GROUPS_DRI & " "
      strSql = strSql & "           WHERE dri_id IN ( "
      strSql = strSql & "                    SELECT dri_id "
      strSql = strSql & "                      FROM " & GTPlotMetadata.Parameters.GT_PLOT_DRAWINGINFO & " "
      strSql = strSql & "                     WHERE dri_type = 'PrintActiveMapWindow'"
      strSql = strSql & "                       AND sheet_id IN ( "
      strSql = strSql & "                              SELECT sheet_id "
      strSql = strSql & "                                FROM " & GTPlotMetadata.Parameters.GT_PLOT_SHEETS & " "
      strSql = strSql & "                               WHERE sheet_size = '" & FormatStringGetSheetSize(lbPaperSize.Text) & "' "
      strSql = strSql & "                                 AND sheet_orientation = '" & IIf(optPaperOrientationLandscape.Checked, "Landscape", "Portrait") & "'))) "
      ' Todo -Remove hard coded "Landscape", "Portrait" values above.  Also values are sometimes only the 1st letter.

      oGTApplication = GTClassFactory.Create(Of IGTApplication)()
      rsRL_TEXT = oGTApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, ADODB.CommandTypeEnum.adCmdText)


      m_colAttributes = New Collection


      ' Add redline text to DataGridView for user to enter values
      Call Repos(rsRL_TEXT)
      While Not rsRL_TEXT.EOF
        oAttribute = GetAttributeInfo(rsRL_TEXT.Fields("RL_TEXT").Value)
        If Not IsNothing(oAttribute) Then
          ' Check if attribute already defined - Used to filter out duplicate attributes that may exist in common components of the feature
          bFound = False
          For Each oAttributeExists As Attribute In m_colAttributes
            If oAttributeExists.G3E_FIELD = oAttribute.G3E_FIELD Then bFound = True
          Next
          If bFound = False Then m_colAttributes.Add(oAttribute, oAttribute.G3E_FIELD)
        End If
        rsRL_TEXT.MoveNext()
      End While

      ' Add attributes to DataGridView
      For Each oAttribute In m_colAttributes
        iRowIndex = dgvAttributes.Rows.Add(oAttribute.G3E_USERNAME, oAttribute.DATA_DEFAULT)
        If iRowIndex = 0 Then dgvAttributes.CurrentCell.Style.Font = fontBold
        'dgvAttributes.Rows(iRowIndex).DefaultCellStyle.Font = fontBold
      Next oAttribute

      dgvAttributes.ClearSelection()
      'dgvAttributes.Rows(0).ReadOnly = False
      'dgvAttributes.Rows(0).Resizable = False
      'dgvAttributes.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
      'dgvAttributes.AutoSize = True
      'dgvAttributes.AutoResizeRows()
      dgvAttributes.AutoResizeColumn(0)
      dgvAttributes.ResumeLayout()


    Catch ex As Exception
      MsgBox("PrintActiveMapWindowForm.UpdateAvailableAttributeValues:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)

    Finally
      fontBold.Dispose()

    End Try
  End Sub



  Private Function GetAttributeInfo(ByVal strText As String) As Attribute

    Static strSql As String
    Static rsAttributes As ADODB.Recordset
    Dim strText2 As String
    Dim strText3 As String
    Dim oAttribute As Attribute = Nothing
    Dim oGTApplication As IGTApplication

    Try

      If String.IsNullOrEmpty(strSql) Then

        ' Get Attribute info from metadata
        strSql = " SELECT * "
        strSql = strSql & "  FROM g3e_attributeinfo_optlang "
        strSql = strSql & " WHERE g3e_cno IN (SELECT g3e_cno "
        strSql = strSql & "                     FROM g3e_featurecomps_optable "
        strSql = strSql & "                    WHERE g3e_fno = " & GTPlotMetadata.Parameters.PlotBoundary_G3E_FNO & ") "
        strSql = strSql & " AND G3E_LCID = '" & GetLanguage() & "' "
        strSql = strSql & " ORDER BY G3E_REQUIRED DESC"

        oGTApplication = GTClassFactory.Create(Of IGTApplication)()
        rsAttributes = oGTApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText)

      End If


      Repos(rsAttributes)

      While Not rsAttributes.EOF
        strText2 = rsAttributes.Fields("G3E_FIELD").Value
        strText3 = "[" + rsAttributes.Fields("G3E_FIELD").Value + "]"

        'Debug.Print(strText3 & " in " & strText)

        If InStr(strText, strText3) <> 0 Then
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
          End With

          Exit While

        End If
        rsAttributes.MoveNext()
      End While

    Catch ex As Exception
      MsgBox("PrintActiveMapWindowForm.GetAttributeInfo:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      oAttribute = Nothing

    Finally
      GetAttributeInfo = oAttribute
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
                dPgContainerScale = (imgPageContainer.Width / m_dSheetWidth) * 0.6
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

                g.DrawImage(ImageList1.Images(0), oRectMapWindow)
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
            'dch added code for div/0 errors
            If (m_dSheetHeight = 0 Or m_dSheetWidth = 0) Then
                'MsgBox("PrintActiveMapWindowForm.imgPageContainer_Paint:" & vbCrLf & "avoiding divide by 0, check gtplot metadata", vbOKOnly, ex.Source)
            Else
                MsgBox("PrintActiveMapWindowForm.imgPageContainer_Paint:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
            End If
        End Try

    End Sub


#End Region


    Public Sub New()

    ' This call is required by the Windows Form Designer.
    InitializeComponent()

    ' Add any initialization after the InitializeComponent() call.

  End Sub
End Class