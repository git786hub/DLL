Imports System
Imports Intergraph.GTechnology.API

Module NewPlotWindowFormation
  Public Function GetFormationsInDetail(ByVal lDetailID As Long) As Collection

    Dim strSql As String
    Dim rsFromFormations As ADODB.Recordset
    Dim rsToFormations As ADODB.Recordset

    Dim colFormations As Collection
    Dim oFormation As Formation
    Dim oApplication As IGTApplication

    On Error GoTo ErrorHandler

    colFormations = New Collection
    oApplication = GTClassFactory.Create(Of IGTApplication)()

    ' Todo -Remove hard coded reference to DGC_FORMFROM_L
    strSql = "SELECT G3E_FNO, G3E_FID, G3E_CNO, G3E_CID FROM DGC_FORMFROM_L WHERE G3E_DETAILID=" & lDetailID
    rsFromFormations = oApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, ADODB.CommandTypeEnum.adCmdText)
    Repos(rsFromFormations)
    While Not rsFromFormations.EOF
      oFormation = New Formation
      With oFormation
        .FNO = rsFromFormations.Fields("G3E_FNO").Value
        .FID = rsFromFormations.Fields("G3E_FID").Value
        .CNO = rsFromFormations.Fields("G3E_CNO").Value
        .CID = rsFromFormations.Fields("G3E_CID").Value
      End With
      colFormations.Add(oFormation)
      rsFromFormations.MoveNext()
    End While


    ' Todo -Remove hard coded reference to DGC_FORMTO_L
    strSql = "SELECT G3E_FNO, G3E_FID, G3E_CNO, G3E_CID FROM DGC_FORMTO_L WHERE G3E_DETAILID=" & lDetailID
    rsToFormations = oApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, ADODB.CommandTypeEnum.adCmdText)
    Repos(rsToFormations)
    While Not rsToFormations.EOF
      oFormation = New Formation
      With oFormation
        .FNO = rsToFormations.Fields("G3E_FNO").Value
        .FID = rsToFormations.Fields("G3E_FID").Value
        .CNO = rsToFormations.Fields("G3E_CNO").Value
        .CID = rsToFormations.Fields("G3E_CID").Value
      End With
      colFormations.Add(oFormation)
      rsToFormations.MoveNext()
    End While

    GetFormationsInDetail = colFormations

    Exit Function

ErrorHandler:
    MsgBox("GTPlotMetadata.GetFormationsInDetail - Error: " & Err.Description)
    Err.Clear()

  End Function

  Public Function GetFeatureDetailFormationInfo(ByVal oPlotBoundary As PlotBoundary) As Collection

    Dim strSql As String
    Dim rsDetailFeature As ADODB.Recordset
    Dim oDetailFeatureGroup As DetailFeatureGroup
    Dim colDetailFeatureGroups As Collection

    Dim oApplication As IGTApplication

    On Error GoTo ErrorHandler

    oApplication = GTClassFactory.Create(Of IGTApplication)()

    ' Get the GT_PLOT_FORMATIONS metadata and populate the Details collection.
    strSql = " SELECT FRM_NAME" & LangSuffix() & ", FRM_VIEW, FRM_DETAIL, FRM_DET_LNO, FRM_FILTER FROM GT_PLOT_FORMATIONS"
    rsDetailFeature = oApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, ADODB.CommandTypeEnum.adCmdText)
    Repos(rsDetailFeature)
    colDetailFeatureGroups = New Collection
    While Not rsDetailFeature.EOF
      oDetailFeatureGroup = New DetailFeatureGroup
      With oDetailFeatureGroup
        .Name = rsDetailFeature.Fields("FRM_NAME" & LangSuffix()).Value
        .View = rsDetailFeature.Fields("FRM_VIEW").Value
        .DetailView = rsDetailFeature.Fields("FRM_DETAIL").Value
        .Det_LNO = rsDetailFeature.Fields("FRM_DET_LNO").Value
        If Not IsDBNull(rsDetailFeature.Fields("FRM_FILTER").Value) Then .Filter = rsDetailFeature.Fields("FRM_FILTER").Value
      End With
      colDetailFeatureGroups.Add(oDetailFeatureGroup)
      rsDetailFeature.MoveNext()
    End While

    GetFeatureDetailFormationInfo = colDetailFeatureGroups

    Exit Function

ErrorHandler:
    MsgBox("GTPlotMetadata.GetFeatureDetailFormationInfo - Error: " & Err.Description)
    Err.Clear()

  End Function



  ' The fucntion superseeds the previous GetDetailDrawingsWithinPolygonOracleSpatialMethod() saved below as reference only.
  Public Function GetDetailDrawingsWithinPolygon(ByVal oPlotBoundary As PlotBoundary) As Collection

    Dim strSql As String
    Dim rsFeatureWithDetails As ADODB.Recordset

    Dim oDetailFeatureGroup As DetailFeatureGroup
    Dim colDetailFeatureGroups As Collection

    Dim oDetail As Detail
    Dim colDetails As Collection

    Dim oGTApplication As IGTApplication

    Dim iPriGrfComp As Integer
    Dim sPriGrfCompView As String

    Dim otempDDCKeyObjects As IGTDDCKeyObjects = GTClassFactory.Create(Of IGTDDCKeyObjects)()
    Dim oDDCKeyObjects As IGTDDCKeyObjects = GTClassFactory.Create(Of IGTDDCKeyObjects)()
    Dim oKeyObject As IGTKeyObject = GTClassFactory.Create(Of IGTKeyObject)()
    Dim oSpatialService As IGTSpatialService = GTClassFactory.Create(Of IGTSpatialService)()
    Dim rsFeatureResult As New ADODB.Recordset
    Dim rsBoundaries As ADODB.Recordset
    Dim oHelperService As IGTHelperService = GTClassFactory.Create(Of IGTHelperService)()
    Dim rsMeta As New ADODB.Recordset
    Dim lRPMNO As Long

    Dim oIGTHelperService As IGTHelperService

    On Error GoTo ErrorHandler

    oGTApplication = GTClassFactory.Create(Of IGTApplication)()

    oIGTHelperService = GTClassFactory.Create(Of IGTHelperService)()
    oIGTHelperService.DataContext = oGTApplication.DataContext

    oSpatialService.DataContext = oGTApplication.DataContext
    'Set the operator type to include all features touching the boundary
    oSpatialService.Operator = GTSpatialOperatorConstants.gtsoTouches

    colDetailFeatureGroups = GetFeatureDetailFormationInfo(oPlotBoundary)

    iPriGrfComp = GetPrimaryCNOofFNO(GTPlotMetadata.Parameters.PlotBoundary_G3E_FNO)
    sPriGrfCompView = GetComponentViewofCNO(iPriGrfComp)

    ' Select from each table in the GT_PLOT_FORMATIONS metadata table with given filter applied if one.
    For Each oDetailFeatureGroup In colDetailFeatureGroups

      'Get the primary graphic component
      oDDCKeyObjects = oGTApplication.DataContext.GetDDCKeyObjects(oPlotBoundary.FNO, oPlotBoundary.FID, GTComponentGeometryConstants.gtddcgPrimaryGeographic)

      'Set the filter geometry to that of the primary geo component
      oSpatialService.FilterGeometry = oDDCKeyObjects.Item(0).Geometry

      'Store the resulting features into an ADO recordset
      rsFeatureResult = oSpatialService.GetResultsByComponentView(oDetailFeatureGroup.View)
      If rsFeatureResult.RecordCount > 0 Then
        rsFeatureResult.MoveFirst()
        While Not rsFeatureResult.EOF

          'oKeyObject = oGTApplication.DataContext.OpenFeature(rsFeatureResult.Fields.Item("G3E_FNO").Value, rsFeatureResult.Fields.Item("G3E_FID").Value)
          'Dim iPriGrfComp2 As Integer
          'Dim sPriGrfCompView2 As String
          'Dim sPriGrfComp2 As String
          'iPriGrfComp2 = GetPrimaryCNOofFNO(oKeyObject.FNO)
          'sPriGrfCompView2 = GetComponentViewofCNO(iPriGrfComp2)
          'sPriGrfComp2 = GetComponentofCNO(iPriGrfComp2)
          'Dim rsComponent As ADODB.Recordset
          'rsComponent = oKeyObject.Components.Item(sPriGrfComp2).Recordset

          'strSql = "SELECT B.G3E_ID, "
          'strSql = strSql & "B.G3E_FNO, "
          'strSql = strSql & "B.G3E_FID, "
          'strSql = strSql & "B.G3E_CNO, "
          strSql = "SELECT B.G3E_CNO, "
          strSql = strSql & "B.G3E_CID, "
          strSql = strSql & "D.G3E_DETAILID, "
          strSql = strSql & "D.DETAIL_USERNAME "
          strSql = strSql & "FROM " & oDetailFeatureGroup.View & " B, " & oDetailFeatureGroup.DetailView & " D "
          strSql = strSql & "WHERE B.G3E_FNO = " & rsFeatureResult.Fields.Item("G3E_FNO").Value & " "
          strSql = strSql & "AND B.G3E_FID = " & rsFeatureResult.Fields.Item("G3E_FID").Value & " "
          If Not IsNothing(oDetailFeatureGroup.Filter) Then
            strSql = strSql & "AND B." & oDetailFeatureGroup.Filter & " "
          End If
          strSql = strSql & "AND D.G3E_FID = B.G3E_FID"

          rsFeatureWithDetails = oGTApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText)
          Repos(rsFeatureWithDetails)
          While Not rsFeatureWithDetails.EOF
            oDetail = New Detail
            With oDetail
              .FNO = rsFeatureResult.Fields.Item("G3E_FNO").Value
              .FID = rsFeatureResult.Fields.Item("G3E_FID").Value
              '.FNO = rsFeatureWithDetails.Fields("G3E_FNO").Value
              '.FID = rsFeatureWithDetails.Fields("G3E_FID").Value
              .CNO = rsFeatureWithDetails.Fields("G3E_CNO").Value
              .CID = rsFeatureWithDetails.Fields("G3E_CID").Value
              .DetailID = rsFeatureWithDetails.Fields("G3E_DETAILID").Value
              .DetailName = rsFeatureWithDetails.Fields("DETAIL_USERNAME").Value
            End With
            oDetailFeatureGroup.Details.Add(oDetail)
            rsFeatureWithDetails.MoveNext()
          End While

          rsFeatureResult.MoveNext()
        End While
      End If
    Next oDetailFeatureGroup

    GetDetailDrawingsWithinPolygon = colDetailFeatureGroups

    Exit Function

ErrorHandler:
    MsgBox("GTPlotMetadata.GetDetailDrawingsWithinPolygon - Error: " & Err.Description)
    Err.Clear()

  End Function



  ' The above procedure superseeds this one.
  Public Function GetDetailDrawingsWithinPolygonOracleSpatialMethod(ByVal oPlotBoundary As PlotBoundary) As Collection

    Dim strSql As String
    Dim rsFeatureWithDetails As ADODB.Recordset

    Dim oDetailFeatureGroup As DetailFeatureGroup
    Dim colDetailFeatureGroups As Collection

    Dim oDetail As Detail
    Dim colDetails As Collection

    Dim oGTApplication As IGTApplication

    Dim iPriGrfComp As Integer
    Dim sPriGrfCompView As String

    On Error GoTo ErrorHandler


    oGTApplication = GTClassFactory.Create(Of IGTApplication)()

    colDetailFeatureGroups = GetFeatureDetailFormationInfo(oPlotBoundary)


    iPriGrfComp = GetPrimaryCNOofFNO(GTPlotMetadata.Parameters.PlotBoundary_G3E_FNO)
    sPriGrfCompView = GetComponentViewofCNO(iPriGrfComp)

    ' Select from each table in the GT_PLOT_FORMATIONS metadata table with given filter applied if one.
    For Each oDetailFeatureGroup In colDetailFeatureGroups

      strSql = "SELECT B.G3E_ID, "
      strSql = strSql & "B.G3E_FNO, "
      strSql = strSql & "B.G3E_FID, "
      strSql = strSql & "B.G3E_CNO, "
      strSql = strSql & "B.G3E_CID, "
      strSql = strSql & "D.G3E_DETAILID, "
      strSql = strSql & "D.DETAIL_USERNAME "
      strSql = strSql & "FROM " & sPriGrfCompView & " A, " & oDetailFeatureGroup.View & " B, " & oDetailFeatureGroup.DetailView & " D "
      strSql = strSql & "WHERE A.G3E_FNO = " & oPlotBoundary.FNO & " "
      strSql = strSql & "AND A.G3E_FID = " & oPlotBoundary.FID & " "

      ' Not all components like PEDS has a SWITCH_CENTRE_CLLI
      'If Not String.IsNullOrEmpty(oGTApplication.AOILocationIdentifier) Then
      ' ' TODO -remove hard coding of SWITCH_CENTRE_CLLI, get from metadata.
      ' strSql = strSql & "       AND B.SWITCH_CENTRE_CLLI='" & oGTApplication.AOILocationIdentifier & "' "
      'End If
      If Not IsNothing(oDetailFeatureGroup.Filter) Then
        strSql = strSql & "AND B." & oDetailFeatureGroup.Filter & " "
      End If
      strSql = strSql & "AND D.G3E_FID = B.G3E_FID "
      strSql = strSql & "AND SDO_RELATE (B.G3E_GEOMETRY, "
      strSql = strSql & "A.G3E_GEOMETRY, "
      strSql = strSql & "'mask=ANYINTERACT querytype=WINDOW')='TRUE'"

      rsFeatureWithDetails = oGTApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText)
      Repos(rsFeatureWithDetails)
      While Not rsFeatureWithDetails.EOF
        oDetail = New Detail
        With oDetail
          .FNO = rsFeatureWithDetails.Fields("G3E_FNO").Value
          .FID = rsFeatureWithDetails.Fields("G3E_FID").Value
          .CNO = rsFeatureWithDetails.Fields("G3E_CNO").Value
          .CID = rsFeatureWithDetails.Fields("G3E_CID").Value
          .DetailID = rsFeatureWithDetails.Fields("G3E_DETAILID").Value
          .DetailName = rsFeatureWithDetails.Fields("DETAIL_USERNAME").Value
        End With

        oDetailFeatureGroup.Details.Add(oDetail)

        rsFeatureWithDetails.MoveNext()
      End While

    Next oDetailFeatureGroup

    GetDetailDrawingsWithinPolygonOracleSpatialMethod = colDetailFeatureGroups

    Exit Function

ErrorHandler:
    MsgBox("GTPlotMetadata.GetDetailDrawingsWithinPolygonOracleSpatialMethod - Error: " & Err.Description)
    Err.Clear()

  End Function



  Public Function FormationsInDetail(ByVal lDetailID As Long) As Boolean

    Dim strSql As String
    Dim rsFormations As ADODB.Recordset
    Dim oApplication As IGTApplication

    On Error GoTo ErrorHandler

    oApplication = GTClassFactory.Create(Of IGTApplication)()

    FormationsInDetail = False

    strSql = "SELECT count(*) FROM DGC_FORMFROM_L WHERE G3E_DETAILID=" & lDetailID
    rsFormations = oApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, ADODB.CommandTypeEnum.adCmdText)
    If Not rsFormations.Fields("COUNT(*)").Value = 0 Then
      FormationsInDetail = True
    Else
      strSql = "SELECT count(*) FROM DGC_FORMTO_L WHERE G3E_DETAILID=" & lDetailID
      rsFormations = oApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, ADODB.CommandTypeEnum.adCmdText)
      If Not rsFormations.Fields("COUNT(*)").Value = 0 Then
        FormationsInDetail = True
      End If
    End If

    Exit Function

ErrorHandler:
    MsgBox("GTPlotMetadata.FormationsInDetail - Error: " & Err.Description)
    Err.Clear()

  End Function

End Module
