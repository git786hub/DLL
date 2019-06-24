Imports System
Imports Microsoft.Win32
Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports Intergraph.GTechnology.API

Public Class PlotBoundaryForm

  Event AboutToPlaceGeometry(ByVal sender As System.Object, ByVal e As System.EventArgs, ByVal m_sType As String, ByVal m_sSheetSize As String, ByVal m_sSheetOrientation As String, ByVal m_sMapScale As String)

  Private m_bCancel As Boolean
  Private m_vListIndex As Object
  Private m_tbMapScaleCustomText As String

#Region "PlotBoundaryForm Misc Routines"

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

  ' Save settings
  Private Sub Save()
    Try
      Dim sUserSubKeyCulture = My.Resources.RegistryCurrentUserSubKey & "\\" & GetCurrentCulture()
      Dim RegKey As RegistryKey = Registry.CurrentUser.OpenSubKey(sUserSubKeyCulture, True)
      If IsNothing(RegKey) Then RegKey = Registry.CurrentUser.CreateSubKey(sUserSubKeyCulture, RegistryKeyPermissionCheck.ReadWriteSubTree)
      RegKey.SetValue(Me.Name & ".Top", Me.Top)
      RegKey.SetValue(Me.Name & ".Left", Me.Left)
      RegKey.SetValue(Me.Name & ".optPaperOrientationPortrait.Checked", Me.optPaperOrientationPortrait.Checked)
      RegKey.SetValue(Me.Name & ".cbType.Checked", cbType.Checked)
      RegKey.SetValue(Me.Name & ".lbType.Text", lbType.Text)
      RegKey.SetValue(Me.Name & ".lbPaperSize.Text", lbPaperSize.Text)
      RegKey.SetValue(Me.Name & ".optMapScalePreDefined.Checked", optMapScalePreDefined.Checked)
      RegKey.SetValue(Me.Name & ".lbMapScalePreDefined.Text", lbMapScalePreDefined.Text)
      RegKey.SetValue(Me.Name & ".tbMapScaleCustom.Text", tbMapScaleCustom.Text)
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
      End If

      If Not IsNothing(RegKey.GetValue(Me.Name & ".cbType.Checked")) Then
        cbType.Checked = RegKey.GetValue(Me.Name & ".cbType.Checked")
      Else
        cbType.Checked = True
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
        ' Get the first avalable scale from metadata...
        'tbMapScaleCustom.Text = "1:750"
        tbMapScaleCustom.Text = lbMapScalePreDefined.Text

        'Get the Map Scales
        'sScales = GetSheetScales(m_sType, m_sSheetName, m_sSheetOrientation)
      End If

      RegKey.Close()

    Catch ex As Exception
    End Try
  End Sub

  '
  ' When the Type Sheet or Orientation changes update the scales list
  '
  Private Sub UpdateAvailableSheets()

    Dim rsPaperSizes As ADODB.Recordset

    Try

      ' Skip out if values are empty
      If String.IsNullOrEmpty(m_sSheetOrientation) Then
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
        If lbPaperSize.Items.Count > 0 Then
          lbPaperSize.SetSelected(0, True)
        End If
      End If
        lbPaperSize.ResumeLayout()

    Catch ex As Exception
      MsgBox("PlotBoundaryForm.UpdateAvailableSheets:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    End Try
  End Sub


  '
  ' When the Type Sheet or Orientation changes update the scales list
  '
  Private Sub UpdateAvailableScales()

    Try

      ' Skip out if values are empty
      If String.IsNullOrEmpty(m_sSheetName) Or String.IsNullOrEmpty(m_sSheetOrientation) Then
        Exit Sub
      End If

      Dim sScales As String

      'Populate MapScalePreDefined listbox
      'Get the Map Size from Map Frame table
      sScales = GetSheetScales(m_sType, m_sSheetName, m_sSheetOrientation)

      ' Save current setting and attempt to set them back once lists refresh...
      lbMapScalePreDefined.SuspendLayout()
      Dim sMapScalePreDefined As String = lbMapScalePreDefined.Text

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
      MsgBox("PlotBoundaryForm.UpdateAvailableScales:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    End Try
  End Sub

  '
  ' When the WPB changes update the image page
  ''
  Private Sub WPB_Change()

        Try
            'If the SheetName or SheetOrientation is not set exit
            If m_sSheetName = "" Or m_sSheetOrientation = "" Or m_sMapScale = "" Then Exit Sub

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
            '  Disabled to avoid infinite loop triggered by checking the Type box after the form initially launches (Rich Adase, 15-JAN-2019)
            'If DoesSheetSizeExist(m_sType, m_sSheetName, m_sSheetOrientation) = False Then
            '    ' Try changing Sheet Orientation
            '    If (optPaperOrientationLandscape.Checked = True) Then
            '        optPaperOrientationPortrait.Checked = True
            '    Else
            '        optPaperOrientationLandscape.Checked = True
            '    End If
            'End If
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
            MsgBox("PlotBoundaryForm.WPB_Change:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    End Try

  End Sub

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
        dPgContainerScale = (imgPageContainer.Width / m_dSheetWidth) * 0.7
      Else
        dPgContainerScale = (imgPageContainer.Height / m_dSheetHeight) * 0.7
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
      MsgBox("PlotBoundaryForm.imgPageContainer_Paint:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    End Try

  End Sub



#End Region


#Region "PlotBoundaryForm Members"

  Private m_bPlaceComponment As Boolean = False
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

  Public Property PlaceComponent() As Boolean
    Get
      PlaceComponent = m_bPlaceComponment
    End Get
    Set(ByVal value As Boolean)
      m_bPlaceComponment = value
    End Set
  End Property

  Private Sub cmdPlace_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPlace.Click, cmdADHOC.Click

    Dim oGTApplication As IGTApplication

    oGTApplication = GTClassFactory.Create(Of IGTApplication)()

    Save()

    On Error Resume Next
    With oGTApplication.Properties
      .Remove("PlotBoundary.Type")
      .Remove("PlotBoundary.PaperSize")
      .Remove("PlotBoundary.SheetOrientation")
      .Remove("PlotBoundary.MapScale")
      On Error GoTo 0
      .Add("PlotBoundary.Type", m_sType)
      .Add("PlotBoundary.PaperSize", m_sSheetSize)
      .Add("PlotBoundary.SheetOrientation", m_sSheetOrientation)
      If optMapScalePreDefined.Checked Then
        .Add("PlotBoundary.MapScale", m_sMapScale)
      Else
        .Add("PlotBoundary.MapScale", m_sMapScaleCustom)
      End If
    End With


    'oGTComponent = components(20401)
    'MsgBox("3")
    'oTempRS = oGTComponent.Recordset
    'If Not (oTempRS.BOF And oTempRS.EOF) Then
    '  If oTempRS.RecordCount > 0 Then
    '    oTempRS.MoveFirst()
    '    'lCurrentFID = oTempRS.Fields("G3E_FID").Value
    '    'lCurrentFNO = oTempRS.Fields("G3E_FNO").Value
    '    If IsDBNull(oTempRS.Fields(GTPlotMetadata.Parameters.PlotBoundaryAttribute_Type).Value) Then ' Or Not oTempRS.Fields("PLOT_SIZE").Value = "ADHOC"
    '      oTempRS.Fields(GTPlotMetadata.Parameters.PlotBoundaryAttribute_Type).Value = m_sType
    '      oTempRS.Fields(GTPlotMetadata.Parameters.PlotBoundaryAttribute_PageSize).Value = m_sSheetSize
    '      oTempRS.Fields(GTPlotMetadata.Parameters.PlotBoundaryAttribute_Scale).Value = m_sSheetOrientation
    '      If optMapScalePreDefined.Checked Then
    '        oTempRS.Fields(GTPlotMetadata.Parameters.PlotBoundaryAttribute_Orientation).Value = m_sMapScale
    '      Else
    '        oTempRS.Fields(GTPlotMetadata.Parameters.PlotBoundaryAttribute_Orientation).Value = m_sMapScaleCustom
    '      End If
    '      oTempRS.Update()
    '    End If
    '  End If
    'End If


    RaiseEvent AboutToPlaceGeometry(sender, e, m_sType, m_sSheetSize, m_sSheetOrientation, m_sMapScale)


    m_bPlaceComponment = True
    Hide()
  End Sub

  Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
    m_bPlaceComponment = False
    Hide()
  End Sub



  Protected Overrides Sub Finalize()

    MyBase.Finalize()

    PlaceComponent = False

  End Sub

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
      m_sSheetOrientation = oRadioButton.Text
      UpdateAvailableSheets()
      'UpdateAvailableScales() 'This will happen automaticly with UpdateAvailableSheets
      WPB_Change()
    End If
  End Sub

  Private Sub optPaperOrientationPortrait_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optPaperOrientationPortrait.CheckedChanged
    Dim oRadioButton As RadioButton = sender
    If oRadioButton.Checked Then
      m_sSheetOrientation = oRadioButton.Text
      UpdateAvailableSheets()
      'UpdateAvailableScales() 'This will happen automaticly with UpdateAvailableSheets
      WPB_Change()
    End If
  End Sub

  Private Sub tbMapScaleCustom_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbMapScaleCustom.TextChanged
    lblSetMapScaleValue.Text = tbMapScaleCustom.Text
    m_sMapScaleCustom = tbMapScaleCustom.Text
    m_sMapScale = m_sMapScaleCustom
    WPB_Change()
  End Sub

#End Region

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



      ' Set global variable based on currently selected dialog items
      m_sSheetSize = FormatStringGetSheetSize(lbPaperSize.Text)
      m_sMapScaleCustom = tbMapScaleCustom.Text
      m_sMapScalePreDefined = lbMapScalePreDefined.Text
      If optMapScalePreDefined.Checked = True Then
        m_sMapScale = m_sMapScalePreDefined
      Else
        m_sMapScale = m_sMapScaleCustom
      End If
      If optPaperOrientationPortrait.Checked Then
        m_sSheetOrientation = optPaperOrientationPortrait.Text
      Else
        m_sSheetOrientation = optPaperOrientationLandscape.Text
      End If

      ' Refresh changes throughout to the rest of the dialog
      WPB_Change()



    Catch ex As Exception
      MsgBox("PlotBoundaryForm.LoadDefaults:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    Finally
      'cleaning
    End Try
  End Sub

  Private Sub PlotBoundaryForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    Static DefaultsAlreadyLoaded As Boolean = False

    Try

      If Not DefaultsAlreadyLoaded Then
        LoadDefaults()
        DefaultsAlreadyLoaded = True
      End If


    Catch ex As Exception
      MsgBox("PlotBoundaryForm.New:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    Finally
      'cleaning
    End Try

  End Sub

  Private Sub cbType_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbType.CheckedChanged
    lbType.Enabled = cbType.Checked
    If cbType.Checked Then
      m_sType = lbType.Text
    Else
      m_sType = ""
    End If
    UpdateAvailableSheets()
    'UpdateAvailableScales() 'This will happen automaticly with UpdateAvailableSheets
    WPB_Change()
  End Sub




  Private Sub lbPaperSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbPaperSize.SelectedIndexChanged
    'lbPaperSize.Text = FormatSheetNameSize()
    m_sSheetName = FormatStringGetSheetName(lbPaperSize.Text)
    m_sSheetSize = FormatStringGetSheetSize(lbPaperSize.Text)
    'm_sSheetName = Mid(lbPaperSize.Text, 2, 6)
    'm_sSheetSize = Mid(lbPaperSize.Text, 10, Len(lbPaperSize.Text))

    UpdateAvailableScales()
    WPB_Change()
  End Sub

  Private Sub lbType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbType.SelectedIndexChanged
    If cbType.Checked Then
      m_sType = lbType.Text
      UpdateAvailableSheets()
      'UpdateAvailableScales() 'This will happen automaticly with UpdateAvailableSheets
      WPB_Change()
    End If
  End Sub

  Private Sub lbMapScalePreDefined_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbMapScalePreDefined.SelectedIndexChanged
    m_sMapScalePreDefined = lbMapScalePreDefined.Text
    m_sMapScale = m_sMapScalePreDefined
    WPB_Change()
  End Sub

End Class