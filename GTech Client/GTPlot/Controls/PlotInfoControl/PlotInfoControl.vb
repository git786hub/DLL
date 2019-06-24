Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Windows.Forms
Imports System.Drawing.Graphics
Imports Intergraph.GTechnology.API
Imports Intergraph.GTechnology.Interfaces

Public Class PlotInfoControl

  Public m_PlotInfo As New PlotInfo

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

  Enum MeasurmentUnits
    Metric
    Imperial
  End Enum

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

    Try

      ' Skip out if values are empty
      Dim oApplicaiton As IGTApplication
      oApplicaiton = GTClassFactory.Create(Of IGTApplication)()

      If String.IsNullOrEmpty(m_sSheetOrientation) Or IsNothing(oApplicaiton.Application) Then
        Exit Sub
      End If

      'Populate PaperSize listbox
      rsPaperSizes = GetPaperSizes(m_sType, m_sSheetOrientation)
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
      MsgBox("PlotInfoControl.UpdateAvailableSheets:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    End Try
  End Sub


  '
  ' When the Type Sheet or Orientation changes update the scales list
  '
  Private Sub UpdateAvailableScales()
    Dim oGTApplication As IGTApplication

    Try

      oGTApplication = GTClassFactory.Create(Of IGTApplication)()

      ' Skip out if values are empty
      If String.IsNullOrEmpty(m_sSheetName) Or String.IsNullOrEmpty(m_sSheetOrientation) Or IsNothing(oGTApplication.Application) Then
        Exit Sub
      End If

      Dim sScales As String

      'Get the Map Scales
      sScales = GetSheetScales(m_sType, m_sSheetName, m_sSheetOrientation)

      'Populate MapScalePreDefined listbox
      lbMapScalePreDefined.Items.Clear()
      If String.IsNullOrEmpty(sScales) Then
        lbMapScalePreDefined.Items.AddRange(GTPlotMetadata.Parameters.PlotBoundaryAvailableScales.Split(","))
      Else
        lbMapScalePreDefined.Items.AddRange(sScales.Split(","))
      End If

      lbMapScalePreDefined.SetSelected(0, True)


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
      oGTApplication = GTClassFactory.Create(Of IGTApplication)()
      'If the SheetName or SheetOrientation is not set exit
      If m_sSheetName = "" Or m_sSheetOrientation = "" Or m_sMapScale = "" Or IsNothing(oGTApplication.Application) Then Exit Sub

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



    Catch ex As Exception
      MsgBox("PlotInfoControl.WPB_Change:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    End Try

  End Sub



  Private Sub LoadDefaults()
    Dim rsTypes As ADODB.Recordset
    Dim oGTApplication As IGTApplication

    Try

      '      AddHandler imgPaper.Paint, AddressOf imgPaper_Paint

      'Populate Type listbox
      rsTypes = GetTypes()
      lbType.Items.Clear()
      While Not rsTypes.EOF
        lbType.Items.Add(rsTypes.Fields("DRI_TYPE").Value)
        rsTypes.MoveNext()
      End While
      rsTypes.Close()


      'Populate MapScaleCustom textbox
      If GTPlotMetadata.Parameters.MeasurementUnits = "Metric" Then
        tbMapScaleCustom.Text = "1:750"
      Else
        tbMapScaleCustom.Text = "1""=20'"
      End If

      ' Set defaults - Future enhancement to retrieve last used from registry
      m_sSheetSize = Mid(lbPaperSize.Text, 10, Len(lbPaperSize.Text))
      m_sMapScaleCustom = lbMapScalePreDefined.Text
      m_sMapScale = m_sMapScaleCustom
      m_sSheetOrientation = optPaperOrientationLandscape.Text


      ' Select the 1st item in each list.
      ' TODO: FUTURE rememeber last selected items
      lbType.SetSelected(0, True)
      If lbPaperSize.Items.Count > 0 Then
        lbPaperSize.SetSelected(0, True)
      End If
      optPaperOrientationLandscape.Enabled = True
      lbMapScalePreDefined.SetSelected(0, True)

    Catch ex As Exception
      MsgBox("PlotInfoControl.LoadDefaults:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
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
    m_sType = lbType.Text
    UpdateAvailableSheets()
    UpdateAvailableScales()
    WPB_Change()
  End Sub

  Private Sub lbMapScalePreDefined_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbMapScalePreDefined.SelectedIndexChanged
    m_sMapScalePreDefined = lbMapScalePreDefined.Text
    m_sMapScale = m_sMapScalePreDefined
    WPB_Change()
  End Sub

  Private Sub PlotInfoControl_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    'Static DefaultsAlreadyLoaded As Boolean = False

    'Try
    '  If Not DefaultsAlreadyLoaded Then
    '    LoadDefaults()
    '    DefaultsAlreadyLoaded = True
    '  End If

    'Catch ex As Exception
    '  MsgBox("PlotInfoControl.New:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    'Finally
    '  'cleaning
    'End Try

  End Sub

#End Region

End Class
