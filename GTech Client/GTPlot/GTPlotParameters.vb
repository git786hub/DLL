Imports System
Imports Intergraph.GTechnology.API

Public Class GTPlotParameters

#Region "Properties"

  Dim m_sGT_PLOT_DRAWINGINFO As String
  Dim m_sGT_PLOT_GROUPS_DRI As String
  Dim m_sGT_PLOT_LEGEND_OVERRIDE_GROUPS As String
  Dim m_sGT_PLOT_LEGEND_OVERRIDE_ITEMS As String
  Dim m_sGT_PLOT_MAPFRAME As String
  Dim m_sGT_PLOT_OBJECTS As String
  Dim m_sGT_PLOT_REDLINES As String
  Dim m_sGT_PLOT_SHEETS As String
  Dim m_sPlotBoundary_G3E_CNO As String
  Dim m_sPlotBoundary_G3E_FNO As String
  Dim m_sPlotBoundaryAttribute_Name As String
  Dim m_sPlotBoundaryAttribute_Orientation As String
  Dim m_sPlotBoundaryAttribute_PageSize As String
  Dim m_sPlotBoundaryAttribute_Scale As String
  Dim m_sPlotBoundaryAttribute_Type As String
  Dim m_sPlotBoundaryLinkedJobAttribute_FieldName As String
  Dim m_sPlotBoundaryInfoView As String
  Dim m_sPlotBoundaryStoreSingleCharacterForOrientation As String
  Dim m_sPlotBoundaryDataToUpper As String
  Dim m_sPlotBoundaryAvailableScales As String
  Dim m_sNewPlotWindowForceMapBackgroundToWhite As String
  Dim m_sMeasurementUnitsMetricLabelSmall As String
  Dim m_sMeasurementUnitsMetricLabellLarge As String
  Dim m_sMeasurementUnitsImperialLabelSmall As String
  Dim m_sMeasurementUnitsImperialLabelLarge As String
  Dim m_sMeasurementUnits As String
  Dim m_sPlotBoundaryDataStoreSizeOnly As String
  Dim m_sGT_PLOT_TMP_MULTI_TEXT As String


  Public ReadOnly Property GT_PLOT_DRAWINGINFO() As String
    Get
      If String.IsNullOrEmpty(m_sGT_PLOT_DRAWINGINFO) Then
        m_sGT_PLOT_DRAWINGINFO = GetParameter("GT_PLOT_DRAWINGINFO")
      End If
      GT_PLOT_DRAWINGINFO = m_sGT_PLOT_DRAWINGINFO
    End Get
  End Property
  Public ReadOnly Property GT_PLOT_GROUPS_DRI() As String
    Get
      If String.IsNullOrEmpty(m_sGT_PLOT_GROUPS_DRI) Then
        m_sGT_PLOT_GROUPS_DRI = GetParameter("GT_PLOT_GROUPS_DRI")
      End If
      GT_PLOT_GROUPS_DRI = m_sGT_PLOT_GROUPS_DRI
    End Get
  End Property
  Public ReadOnly Property GT_PLOT_LEGEND_OVERRIDE_GROUPS() As String
    Get
      If String.IsNullOrEmpty(m_sGT_PLOT_LEGEND_OVERRIDE_GROUPS) Then
        m_sGT_PLOT_LEGEND_OVERRIDE_GROUPS = GetParameter("GT_PLOT_LEGEND_OVERRIDE_GROUPS")
      End If
      GT_PLOT_LEGEND_OVERRIDE_GROUPS = m_sGT_PLOT_LEGEND_OVERRIDE_GROUPS
    End Get
  End Property
  Public ReadOnly Property GT_PLOT_LEGEND_OVERRIDE_ITEMS() As String
    Get
      If String.IsNullOrEmpty(m_sGT_PLOT_LEGEND_OVERRIDE_ITEMS) Then
        m_sGT_PLOT_LEGEND_OVERRIDE_ITEMS = GetParameter("GT_PLOT_LEGEND_OVERRIDE_ITEMS")
      End If
      GT_PLOT_LEGEND_OVERRIDE_ITEMS = m_sGT_PLOT_LEGEND_OVERRIDE_ITEMS
    End Get
  End Property
  Public ReadOnly Property GT_PLOT_MAPFRAME() As String
    Get
      If String.IsNullOrEmpty(m_sGT_PLOT_MAPFRAME) Then
        m_sGT_PLOT_MAPFRAME = GetParameter("GT_PLOT_MAPFRAME")
      End If
      GT_PLOT_MAPFRAME = m_sGT_PLOT_MAPFRAME
    End Get
  End Property
  Public ReadOnly Property GT_PLOT_OBJECTS() As String
    Get
      If String.IsNullOrEmpty(m_sGT_PLOT_OBJECTS) Then
        m_sGT_PLOT_OBJECTS = GetParameter("GT_PLOT_OBJECTS")
      End If
      GT_PLOT_OBJECTS = m_sGT_PLOT_OBJECTS
    End Get
  End Property
  Public ReadOnly Property GT_PLOT_REDLINES() As String
    Get
      If String.IsNullOrEmpty(m_sGT_PLOT_REDLINES) Then
        m_sGT_PLOT_REDLINES = GetParameter("GT_PLOT_REDLINES")
      End If
      GT_PLOT_REDLINES = m_sGT_PLOT_REDLINES
    End Get
  End Property
  Public ReadOnly Property GT_PLOT_SHEETS() As String
    Get
      If String.IsNullOrEmpty(m_sGT_PLOT_SHEETS) Then
        m_sGT_PLOT_SHEETS = GetParameter("GT_PLOT_SHEETS")
      End If
      GT_PLOT_SHEETS = m_sGT_PLOT_SHEETS
    End Get
  End Property
  Public ReadOnly Property PlotBoundary_G3E_CNO() As String
    Get
      If String.IsNullOrEmpty(m_sPlotBoundary_G3E_CNO) Then
        m_sPlotBoundary_G3E_CNO = GetParameter("PlotBoundary_G3E_CNO")
      End If
      PlotBoundary_G3E_CNO = m_sPlotBoundary_G3E_CNO
    End Get
  End Property
  Public ReadOnly Property PlotBoundary_G3E_FNO() As String
    Get
      If String.IsNullOrEmpty(m_sPlotBoundary_G3E_FNO) Then
        m_sPlotBoundary_G3E_FNO = GetParameter("PlotBoundary_G3E_FNO")
      End If
      PlotBoundary_G3E_FNO = m_sPlotBoundary_G3E_FNO
    End Get
  End Property
  Public ReadOnly Property PlotBoundaryAttribute_Name() As String
    Get
      If String.IsNullOrEmpty(m_sPlotBoundaryAttribute_Name) Then
        m_sPlotBoundaryAttribute_Name = GetParameter("PlotBoundaryAttribute_Name")
      End If
      PlotBoundaryAttribute_Name = m_sPlotBoundaryAttribute_Name
    End Get
  End Property
  Public ReadOnly Property PlotBoundaryAttribute_Orientation() As String
    Get
      If String.IsNullOrEmpty(m_sPlotBoundaryAttribute_Orientation) Then
        m_sPlotBoundaryAttribute_Orientation = GetParameter("PlotBoundaryAttribute_Orientation")
      End If
      PlotBoundaryAttribute_Orientation = m_sPlotBoundaryAttribute_Orientation
    End Get
  End Property
  Public ReadOnly Property PlotBoundaryAttribute_PageSize() As String
    Get
      If String.IsNullOrEmpty(m_sPlotBoundaryAttribute_PageSize) Then
        m_sPlotBoundaryAttribute_PageSize = GetParameter("PlotBoundaryAttribute_PageSize")
      End If
      PlotBoundaryAttribute_PageSize = m_sPlotBoundaryAttribute_PageSize
    End Get
  End Property
  Public ReadOnly Property PlotBoundaryAttribute_Scale() As String
    Get
      If String.IsNullOrEmpty(m_sPlotBoundaryAttribute_Scale) Then
        m_sPlotBoundaryAttribute_Scale = GetParameter("PlotBoundaryAttribute_Scale")
      End If
      PlotBoundaryAttribute_Scale = m_sPlotBoundaryAttribute_Scale
    End Get
  End Property
  Public ReadOnly Property PlotBoundaryAttribute_Type() As String
    Get
      If String.IsNullOrEmpty(m_sPlotBoundaryAttribute_Type) Then
        m_sPlotBoundaryAttribute_Type = GetParameter("PlotBoundaryAttribute_Type")
      End If
      PlotBoundaryAttribute_Type = m_sPlotBoundaryAttribute_Type
    End Get
  End Property
  Public ReadOnly Property PlotBoundaryLinkedJobAttribute_FieldName() As String
    Get
      If String.IsNullOrEmpty(m_sPlotBoundaryLinkedJobAttribute_FieldName) Then
        m_sPlotBoundaryLinkedJobAttribute_FieldName = GetParameter("PlotBoundaryLinkedJobAttribute_FieldName")
      End If
      PlotBoundaryLinkedJobAttribute_FieldName = m_sPlotBoundaryLinkedJobAttribute_FieldName
    End Get
  End Property
  Public ReadOnly Property PlotBoundaryInfoView() As String
    Get
      If String.IsNullOrEmpty(m_sPlotBoundaryInfoView) Then
        m_sPlotBoundaryInfoView = GetParameter("PlotBoundaryInfoView")
      End If
      PlotBoundaryInfoView = m_sPlotBoundaryInfoView
    End Get
  End Property
  Public ReadOnly Property PlotBoundaryStoreSingleCharacterForOrientation() As String
    Get
      If String.IsNullOrEmpty(m_sPlotBoundaryStoreSingleCharacterForOrientation) Then
        m_sPlotBoundaryStoreSingleCharacterForOrientation = GetParameter("PlotBoundaryStoreSingleCharacterForOrientation")
      End If
      PlotBoundaryStoreSingleCharacterForOrientation = m_sPlotBoundaryStoreSingleCharacterForOrientation
    End Get
  End Property
  Public ReadOnly Property PlotBoundaryDataToUpper() As String
    Get
      If String.IsNullOrEmpty(m_sPlotBoundaryDataToUpper) Then
        m_sPlotBoundaryDataToUpper = GetParameter("PlotBoundaryDataToUpper")
      End If
      PlotBoundaryDataToUpper = m_sPlotBoundaryDataToUpper
    End Get
  End Property
  Public ReadOnly Property PlotBoundaryAvailableScales() As String
    Get
      If String.IsNullOrEmpty(m_sPlotBoundaryAvailableScales) Then
        m_sPlotBoundaryAvailableScales = GetParameter("PlotBoundaryAvailableScales")
      End If
      PlotBoundaryAvailableScales = m_sPlotBoundaryAvailableScales
    End Get
  End Property
  Public ReadOnly Property NewPlotWindowForceMapBackgroundToWhite() As String
    Get
      If String.IsNullOrEmpty(m_sNewPlotWindowForceMapBackgroundToWhite) Then
        m_sNewPlotWindowForceMapBackgroundToWhite = GetParameter("NewPlotWindowForceMapBackgroundToWhite")
      End If
      NewPlotWindowForceMapBackgroundToWhite = m_sNewPlotWindowForceMapBackgroundToWhite
    End Get
  End Property
  Public ReadOnly Property MeasurementUnitsMetricLabelSmall() As String
    Get
      If String.IsNullOrEmpty(m_sMeasurementUnitsMetricLabelSmall) Then
        m_sMeasurementUnitsMetricLabelSmall = GetParameter("MeasurementUnitsMetricLabelSmall")
      End If
      MeasurementUnitsMetricLabelSmall = m_sMeasurementUnitsMetricLabelSmall
    End Get
  End Property
  Public ReadOnly Property MeasurementUnitsMetricLabellLarge() As String
    Get
      If String.IsNullOrEmpty(m_sMeasurementUnitsMetricLabellLarge) Then
        m_sMeasurementUnitsMetricLabellLarge = GetParameter("MeasurementUnitsMetricLabellLarge")
      End If
      MeasurementUnitsMetricLabellLarge = m_sMeasurementUnitsMetricLabellLarge
    End Get
  End Property
  Public ReadOnly Property MeasurementUnitsImperialLabelSmall() As String
    Get
      If String.IsNullOrEmpty(m_sMeasurementUnitsImperialLabelSmall) Then
        m_sMeasurementUnitsImperialLabelSmall = GetParameter("MeasurementUnitsImperialLabelSmall")
      End If
      MeasurementUnitsImperialLabelSmall = m_sMeasurementUnitsImperialLabelSmall
    End Get
  End Property
  Public ReadOnly Property MeasurementUnitsImperialLabelLarge() As String
    Get
      If String.IsNullOrEmpty(m_sMeasurementUnitsImperialLabelLarge) Then
        m_sMeasurementUnitsImperialLabelLarge = GetParameter("MeasurementUnitsImperialLabelLarge")
      End If
      MeasurementUnitsImperialLabelLarge = m_sMeasurementUnitsImperialLabelLarge
    End Get
  End Property
  Public ReadOnly Property MeasurementUnits() As String
    Get
      If String.IsNullOrEmpty(m_sMeasurementUnits) Then
        m_sMeasurementUnits = GetParameter("MeasurementUnits")
      End If
      MeasurementUnits = m_sMeasurementUnits
    End Get
  End Property
  Public ReadOnly Property GT_PLOT_TMP_MULTI_TEXT() As String
    Get
      If String.IsNullOrEmpty(m_sGT_PLOT_TMP_MULTI_TEXT) Then
        m_sGT_PLOT_TMP_MULTI_TEXT = GetParameter("GT_PLOT_TMP_MULTI_TEXT")
      End If
      GT_PLOT_TMP_MULTI_TEXT = m_sGT_PLOT_TMP_MULTI_TEXT
    End Get
  End Property
  Public ReadOnly Property PlotBoundaryDataStoreSizeOnly() As String
    Get
      If String.IsNullOrEmpty(m_sPlotBoundaryDataStoreSizeOnly) Then
        m_sPlotBoundaryDataStoreSizeOnly = GetParameter("PlotBoundaryDataStoreSizeOnly")
      End If
      PlotBoundaryDataStoreSizeOnly = m_sPlotBoundaryDataStoreSizeOnly
    End Get
  End Property

#End Region

  Private Function GetParameter(ByVal parameterName As String) As String

    Dim strSql As String
    Dim rs As ADODB.Recordset
    Dim vParam As Object = Nothing
    Dim oApplication As IGTApplication

    Try
      strSql = " SELECT   PAR_VALUE "
      strSql = strSql & "    FROM " & My.Resources.GT_PLOT_PARAMETER & " "
      strSql = strSql & "   WHERE PAR_NAME = '" & parameterName & "'"
      oApplication = GTClassFactory.Create(Of IGTApplication)()
      rs = oApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText, vParam)
      Call Repos(rs)

      GetParameter = rs.Fields("PAR_VALUE").Value

    Catch ex As Exception
      MsgBox("GTPlot.Parameters.GetParameter:" & vbCrLf & "Problem getting parameter [" & parameterName & "] from the " & My.Resources.GT_PLOT_PARAMETER & " table." & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      GetParameter = ""
    End Try

  End Function

  Private Sub Repos(ByRef rs As ADODB.Recordset)
    On Error Resume Next
    If rs.BOF And rs.EOF Then Exit Sub
    rs.MoveLast()
    rs.MoveFirst()
    On Error GoTo 0
  End Sub

End Class
