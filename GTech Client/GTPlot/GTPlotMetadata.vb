Imports System
Imports Intergraph.GTechnology.API

Module GTPlotMetadata

  Public Parameters As New GTPlotParameters

  Public Function FormatStringSetSheetNameSize(ByVal sSheetName As String, ByVal sSheetSize As String) As String

    FormatStringSetSheetNameSize = sSheetName & " (" & sSheetSize & ")"

  End Function

  Public Function FormatStringGetSheetName(ByVal sSheetNameSize) As String

    Dim iPos As Integer

    iPos = InStr(sSheetNameSize, "(")
    If iPos = 0 Then
      FormatStringGetSheetName = ""
    Else
      FormatStringGetSheetName = Trim(Mid(sSheetNameSize, 1, iPos - 1))
    End If

  End Function

  Public Function FormatStringGetSheetSize(ByVal sSheetNameSize) As String

    Dim iPos As Integer
    Dim iPos2 As Integer

    iPos = InStr(sSheetNameSize, "(") + 1
    iPos2 = InStr(sSheetNameSize, ")")
    If iPos > iPos2 Then
      FormatStringGetSheetSize = sSheetNameSize
    Else
      FormatStringGetSheetSize = Trim(Mid(sSheetNameSize, iPos, iPos2 - iPos))
    End If

  End Function

  Public Sub GetSheetSize(ByVal sType As String, ByVal sSheetName As String, ByVal sSheetOrientation As String, ByRef lSheetId As Integer, ByRef dSheetHeight As Double, ByRef dSheetWidth As Double, ByRef iDRI_ID As Integer, ByRef dSheetInset As Double)
    Dim iSheetStyleNo As Integer
    Try
      GetSheetSize(sType, sSheetName, sSheetOrientation, lSheetId, dSheetHeight, dSheetWidth, iDRI_ID, dSheetInset, iSheetStyleNo)

    Catch ex As Exception
      MsgBox("GTPlotMetadata.GetSheetSize:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    End Try

  End Sub



  Public Function GetSheetOrientationFromKey(ByVal sSheetOrientation As String) As String

    Dim strSql As String
    Dim rsSheetName As ADODB.Recordset
    Dim vParam As Object = Nothing
    Dim oApplication As IGTApplication

    Try

      If GetLanguage() = "000C" Then

        strSql = " SELECT distinct(SHEET_ORIENTATION" & LangSuffix() & ") "
        strSql = strSql & "  FROM " & GTPlotMetadata.Parameters.GT_PLOT_SHEETS & " "
        strSql = strSql & " WHERE SHEET_ORIENTATION = '" & sSheetOrientation & "'"

        oApplication = GTClassFactory.Create(Of IGTApplication)()

        rsSheetName = oApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText, vParam)

        Repos(rsSheetName)
        If rsSheetName.BOF And rsSheetName.EOF Then
          Dim sMsg As String = "Could not find GTPlot Key " & GTPlotMetadata.Parameters.GT_PLOT_SHEETS & ".SHEET_ORIENTATION" & LangSuffix() & " from metadata for:" & vbCrLf & _
          "SHEET_ORIENTATION = '" & sSheetOrientation & "'" & vbCrLf & _
          "Please ask the administrator to check the database"

          GetSheetOrientationFromKey = ""

          Err.Raise(101, "GTPlot", sMsg)
        Else
          GetSheetOrientationFromKey = rsSheetName.Fields("SHEET_ORIENTATION" & LangSuffix()).Value
        End If
        rsSheetName.Close()

      Else
        GetSheetOrientationFromKey = sSheetOrientation
      End If

    Catch ex As Exception
      MsgBox("GTPlotMetadata.GetSheetOrientationFromKey:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      GetSheetOrientationFromKey = ""
    End Try

  End Function

  Public Function GetTypeFromKey(ByVal sType As String) As String

    Dim strSql As String
    Dim rsSheetName As ADODB.Recordset
    Dim vParam As Object = Nothing
    Dim oApplication As IGTApplication

    Try

      If sType = "" Then
        GetTypeFromKey = ""
        Exit Function
      End If

      If GetLanguage() = "000C" Then

        strSql = " SELECT distinct(DRI_TYPE" & LangSuffix() & ") "
        strSql = strSql & "  FROM " & GTPlotMetadata.Parameters.GT_PLOT_DRAWINGINFO & " "
        strSql = strSql & " WHERE DRI_TYPE = '" & sType & "'"

        oApplication = GTClassFactory.Create(Of IGTApplication)()

        rsSheetName = oApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText, vParam)

        Repos(rsSheetName)
        If rsSheetName.BOF And rsSheetName.EOF Then
          Dim sMsg As String = "Could not find GTPlot Key " & GTPlotMetadata.Parameters.GT_PLOT_DRAWINGINFO & ".DRI_TYPE" & LangSuffix() & " from metadata for:" & vbCrLf & _
          "DRI_TYPE = '" & sType & "'" & vbCrLf & _
          "Please ask the administrator to check the database"

          GetTypeFromKey = ""

          Err.Raise(101, "GTPlot", sMsg)
        Else
          GetTypeFromKey = rsSheetName.Fields("DRI_TYPE" & LangSuffix()).Value
        End If
        rsSheetName.Close()

      Else
        GetTypeFromKey = sType
      End If

    Catch ex As Exception
      MsgBox("GTPlotMetadata.GetTypeKey:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      GetTypeFromKey = ""
    End Try

  End Function

  Public Function GetSheetOrientationKey(ByVal sSheetOrientation As String) As String

    Dim strSql As String
    Dim rsSheetName As ADODB.Recordset
    Dim vParam As Object = Nothing
    Dim oApplication As IGTApplication

    Try

      If GetLanguage() = "000C" Then

        strSql = " SELECT distinct(SHEET_ORIENTATION) "
        strSql = strSql & "  FROM " & GTPlotMetadata.Parameters.GT_PLOT_SHEETS & " "
        strSql = strSql & " WHERE SHEET_ORIENTATION" & LangSuffix() & " = '" & sSheetOrientation & "'"

        oApplication = GTClassFactory.Create(Of IGTApplication)()

        rsSheetName = oApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText, vParam)

        Repos(rsSheetName)
        If rsSheetName.BOF And rsSheetName.EOF Then
          Dim sMsg As String = "Could not find GTPlot Key " & GTPlotMetadata.Parameters.GT_PLOT_SHEETS & ".SHEET_ORIENTATION from metadata for:" & vbCrLf & _
          "SHEET_ORIENTATION" & LangSuffix() & " = '" & sSheetOrientation & "'" & vbCrLf & _
          "Please ask the administrator to check the database"

          GetSheetOrientationKey = ""

          Err.Raise(101, "GTPlot", sMsg)
        Else
          GetSheetOrientationKey = rsSheetName.Fields("SHEET_ORIENTATION").Value
        End If
        rsSheetName.Close()

      Else
        GetSheetOrientationKey = sSheetOrientation
      End If

    Catch ex As Exception
      MsgBox("GTPlotMetadata.GetSheetOrientationKey:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      GetSheetOrientationKey = ""
    End Try

  End Function

  Public Function GetTypeKey(ByVal sType As String) As String

    Dim strSql As String
    Dim rsSheetName As ADODB.Recordset
    Dim vParam As Object = Nothing
    Dim oApplication As IGTApplication

    Try

      If sType = "" Then
        GetTypeKey = ""
        Exit Function
      End If

      If GetLanguage() = "000C" Then

        strSql = " SELECT distinct(DRI_TYPE) "
        strSql = strSql & "  FROM " & GTPlotMetadata.Parameters.GT_PLOT_DRAWINGINFO & " "
        strSql = strSql & " WHERE DRI_TYPE" & LangSuffix() & " = '" & sType & "'"

        oApplication = GTClassFactory.Create(Of IGTApplication)()

        rsSheetName = oApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText, vParam)

        Repos(rsSheetName)
        If rsSheetName.BOF And rsSheetName.EOF Then
          Dim sMsg As String = "Could not find GTPlot Key " & GTPlotMetadata.Parameters.GT_PLOT_DRAWINGINFO & ".DRI_TYPE from metadata for:" & vbCrLf & _
          "DRI_TYPE" & LangSuffix() & " = '" & sType & "'" & vbCrLf & _
          "Please ask the administrator to check the database"

          GetTypeKey = ""

          Err.Raise(101, "GTPlot", sMsg)
        Else
          GetTypeKey = rsSheetName.Fields("DRI_TYPE").Value
        End If
        rsSheetName.Close()

      Else
        GetTypeKey = sType
      End If

    Catch ex As Exception
      MsgBox("GTPlotMetadata.GetTypeKey:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      GetTypeKey = ""
    End Try

  End Function

  Public Function GetSheetNameFromSize(ByVal sSheetSize As String) As String

    Dim strSql As String
    Dim rsSheetName As ADODB.Recordset
    Dim vParam As Object = Nothing
    Dim oApplication As IGTApplication

    Try
      strSql = " SELECT distinct(SHEET_NAME" & LangSuffix() & ") "
      strSql = strSql & "  FROM " & GTPlotMetadata.Parameters.GT_PLOT_SHEETS & " "
      strSql = strSql & " WHERE sheet_size = '" & sSheetSize & "'"

      oApplication = GTClassFactory.Create(Of IGTApplication)()

      rsSheetName = oApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText, vParam)

      Repos(rsSheetName)
      If rsSheetName.BOF And rsSheetName.EOF Then
        Dim sMsg As String = "Could not find GTPlot sheet name from metadata for:" & vbCrLf & _
        "Sheet Size=" & sSheetSize & vbCrLf & _
        "Please ask the administrator to check the database"

        GetSheetNameFromSize = ""

        Err.Raise(101, "GTPlot", sMsg)
      Else
        GetSheetNameFromSize = rsSheetName.Fields("SHEET_NAME" & LangSuffix()).Value
      End If
      rsSheetName.Close()

    Catch ex As Exception
      MsgBox("GTPlotMetadata.GetSheetNameFromSize:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      GetSheetNameFromSize = ""
    End Try

  End Function

  Public Sub GetSheetSize(ByVal sType As String, ByVal sSheetName As String, ByVal sSheetOrientation As String, ByRef lSheetId As Integer, ByRef dSheetHeight As Double, ByRef dSheetWidth As Double, ByRef iDRI_ID As Integer, ByRef dSheetInset As Double, ByRef iSheetStyle As Integer)


    Dim strSql As String
    Dim rsSheetSize As ADODB.Recordset
    Dim vParam As Object = Nothing
    Dim oApplication As IGTApplication

    Try
      strSql = " SELECT s.SHEET_ID, s.SHEET_HEIGHT, s.SHEET_WIDTH, d.SHEET_INSET, d.SHEET_INSET_STYLE_NO, d.DRI_ID "
      strSql = strSql & "  FROM " & GTPlotMetadata.Parameters.GT_PLOT_SHEETS & " s, " & GTPlotMetadata.Parameters.GT_PLOT_DRAWINGINFO & " d "
      strSql = strSql & " WHERE s.sheet_id = d.sheet_id "
      If sType = "" Then
        strSql = strSql & "   AND d.dri_type" & LangSuffix() & " is null"
      Else
        strSql = strSql & "   AND upper(d.dri_type" & LangSuffix() & ") = upper('" & sType & "')"
      End If
      strSql = strSql & "   AND s.sheet_name" & LangSuffix() & " = '" & sSheetName & "'"
      strSql = strSql & "   AND upper(s.sheet_orientation" & LangSuffix() & ") = upper('" & sSheetOrientation & "')"

      oApplication = GTClassFactory.Create(Of IGTApplication)()

      rsSheetSize = oApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText, vParam)

      Repos(rsSheetSize)
      If rsSheetSize.BOF And rsSheetSize.EOF Then
        Dim sMsg As String = "Could not find GTPlot sheet size metadata for:" & vbCrLf & _
        "Type=" & sType & vbCrLf & _
        "Sheet Name=" & sSheetName & vbCrLf & _
        "Sheet Orientation=" & sSheetOrientation & vbCrLf & _
        "Please ask the administrator to check the database"

        Err.Raise(101, "GTPlot", sMsg)
      Else
        lSheetId = rsSheetSize.Fields("SHEET_ID").Value
        dSheetHeight = rsSheetSize.Fields("SHEET_HEIGHT").Value
        dSheetWidth = rsSheetSize.Fields("SHEET_WIDTH").Value
        dSheetInset = IIf(IsDBNull(rsSheetSize.Fields("SHEET_INSET").Value), 0, rsSheetSize.Fields("SHEET_INSET").Value)
        iDRI_ID = rsSheetSize.Fields("DRI_ID").Value
        iSheetStyle = IIf(IsDBNull(rsSheetSize.Fields("SHEET_INSET_STYLE_NO").Value), 0, rsSheetSize.Fields("SHEET_INSET_STYLE_NO").Value)

      End If

      rsSheetSize.Close()

    Catch ex As Exception
      MsgBox("GTPlotMetadata.GetSheetSize:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    End Try

  End Sub

  Public Function DoesSheetSizeExist(ByVal sType As String, ByVal sSheetName As String, ByVal sSheetOrientation As String) As Boolean


    Dim strSql As String
    Dim rsSheetSize As ADODB.Recordset
    Dim vParam As Object = Nothing
    Dim oApplication As IGTApplication
    Dim Results As Boolean = True

    Try
      strSql = " SELECT s.SHEET_ID, s.SHEET_HEIGHT, s.SHEET_WIDTH, d.SHEET_INSET, d.SHEET_INSET_STYLE_NO, d.DRI_ID "
      strSql = strSql & "  FROM " & GTPlotMetadata.Parameters.GT_PLOT_SHEETS & " s, " & GTPlotMetadata.Parameters.GT_PLOT_DRAWINGINFO & " d "
      strSql = strSql & " WHERE s.sheet_id = d.sheet_id "
      If sType = "" Then
        strSql = strSql & "   AND d.dri_type" & LangSuffix() & " is null"
      Else
        strSql = strSql & "   AND upper(d.dri_type" & LangSuffix() & ") = upper('" & sType & "')"
      End If
      strSql = strSql & "   AND s.sheet_name" & LangSuffix() & " = '" & sSheetName & "'"
      strSql = strSql & "   AND upper(s.sheet_orientation" & LangSuffix() & ") = upper('" & sSheetOrientation & "')"

      oApplication = GTClassFactory.Create(Of IGTApplication)()

      rsSheetSize = oApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText, vParam)

      Repos(rsSheetSize)
      If rsSheetSize.BOF And rsSheetSize.EOF Then
        Results = False
      End If
      rsSheetSize.Close()
      Return Results

    Catch ex As Exception
      MsgBox("GTPlotMetadata.DoesSheetSizeExist:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    End Try

  End Function

  Public Function GetMapFrames(ByVal iDRI_ID) As ADODB.Recordset
    Dim Sql As String
    Dim rsMapFrames As ADODB.Recordset
    Dim vParam As Object = Nothing
    Dim oApplication As IGTApplication
    Try
      Sql = " SELECT   g.dri_id, g.group_no, g.group_offset_x, g.group_offset_y, mf_coordinate_x1, mf_coordinate_y1, mf_coordinate_x2, mf_coordinate_y2"
      Sql = Sql & "     FROM " & GTPlotMetadata.Parameters.GT_PLOT_GROUPS_DRI & " g, " & GTPlotMetadata.Parameters.GT_PLOT_MAPFRAME & " r"
      Sql = Sql & "    WHERE g.dri_id = '" & iDRI_ID & "'"
      Sql = Sql & "      AND g.group_no = r.group_no"
      Sql = Sql & "      AND mf_datatype = 'Map Frame'"
      Sql = Sql & " ORDER BY group_no" ' Order by because the 1st one in the group is the main map frame

      oApplication = GTClassFactory.Create(Of IGTApplication)()
      rsMapFrames = oApplication.DataContext.OpenRecordset(Sql, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText, vParam)

      Repos(rsMapFrames)

      GetMapFrames = rsMapFrames

    Catch ex As Exception
      MsgBox("GTPlotMetadata.GetMapFrames:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      GetMapFrames = Nothing
    End Try

  End Function

  Public Function GetKeyMapFrames(ByVal iDRI_ID) As ADODB.Recordset
    Dim Sql As String
    Dim rsMapFrames As ADODB.Recordset
    Dim vParam As Object = Nothing
    Dim oApplication As IGTApplication
    Try
      Sql = " SELECT   g.dri_id, g.group_no, g.group_offset_x, g.group_offset_y, mf_coordinate_x1, mf_coordinate_y1, mf_coordinate_x2, mf_coordinate_y2"
      Sql = Sql & "     FROM " & GTPlotMetadata.Parameters.GT_PLOT_GROUPS_DRI & " g, " & GTPlotMetadata.Parameters.GT_PLOT_MAPFRAME & " r"
      Sql = Sql & "    WHERE g.dri_id = '" & iDRI_ID & "'"
      Sql = Sql & "      AND g.group_no = r.group_no"
      Sql = Sql & "      AND mf_datatype = 'Key Map'"
      Sql = Sql & " ORDER BY group_no"

      oApplication = GTClassFactory.Create(Of IGTApplication)()
      rsMapFrames = oApplication.DataContext.OpenRecordset(Sql, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText, vParam)

      Repos(rsMapFrames)

      GetKeyMapFrames = rsMapFrames

    Catch ex As Exception
      MsgBox("GTPlotMetadata.GetMapFrames:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      GetKeyMapFrames = Nothing
    End Try

  End Function

  Public Sub GetMapSize(ByVal iDRI_ID As Integer, ByRef dMapHeight As Double, ByRef dMapWidth As Double)

    Dim oMapTLPoint As IGTPoint
    Dim oMapBRPoint As IGTPoint

    oMapTLPoint = GTClassFactory.Create(Of IGTPoint)()
    oMapBRPoint = GTClassFactory.Create(Of IGTPoint)()

        Try
            GetMapLocation(iDRI_ID, oMapTLPoint, oMapBRPoint)

            dMapHeight = oMapBRPoint.Y - oMapTLPoint.Y
            dMapWidth = oMapBRPoint.X - oMapTLPoint.X

        Catch ex As Exception
            If (dMapHeight = 0 Or dMapWidth = 0) Then
                MsgBox("GTPlotMetadata.GetMapSize:" & " Invalid values or missing map frame for DRI_ID: " & iDRI_ID.ToString & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
            Else
                MsgBox("GTPlotMetadata.GetMapSize:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
            End If
        End Try

    End Sub

  Public Function GetSheetScales(ByVal sType As String, ByVal sSheetName As String, ByVal sSheetOrientation As String) As String

    Dim strSql As String
    Dim rsSheetScales As ADODB.Recordset
    Dim vParam As Object = Nothing
    Dim oApplication As IGTApplication
    oApplication = GTClassFactory.Create(Of IGTApplication)()

    Try
      strSql = " SELECT d.DRI_SCALES "
      strSql = strSql & "  FROM " & GTPlotMetadata.Parameters.GT_PLOT_SHEETS & " s, " & GTPlotMetadata.Parameters.GT_PLOT_DRAWINGINFO & " d "
      strSql = strSql & " WHERE s.sheet_id = d.sheet_id "
      If sType = "" Then
        strSql = strSql & "   AND d.dri_type" & LangSuffix() & " is null"
      Else
        strSql = strSql & "   AND d.dri_type" & LangSuffix() & " = '" & sType & "'"
      End If
      strSql = strSql & "   AND s.sheet_name" & LangSuffix() & " = '" & sSheetName & "'"
      strSql = strSql & "   AND s.sheet_orientation" & LangSuffix() & " = '" & sSheetOrientation & "'"

      oApplication = GTClassFactory.Create(Of IGTApplication)()
      rsSheetScales = oApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText, vParam)

      Repos(rsSheetScales)

      GetSheetScales = rsSheetScales.Fields("DRI_SCALES").Value

      rsSheetScales.Close()

    Catch ex As Exception
      MsgBox("GTPlotMetadata.GetSheetScales:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      GetSheetScales = String.Empty
    End Try

  End Function

  Public Function DoesSheetSupportOrientation(ByVal sType As String, ByVal sSheetName As String, ByVal sSheetOrientation As String) As Boolean


    Dim strSql As String
    Dim rsSheetScales As ADODB.Recordset
    Dim vParam As Object = Nothing
    Dim oApplication As IGTApplication
    oApplication = GTClassFactory.Create(Of IGTApplication)()

    Try
      strSql = " SELECT d.DRI_SCALES "
      strSql = strSql & "  FROM " & GTPlotMetadata.Parameters.GT_PLOT_SHEETS & " s, " & GTPlotMetadata.Parameters.GT_PLOT_DRAWINGINFO & " d "
      strSql = strSql & " WHERE s.sheet_id = d.sheet_id "
      If sType = "" Then
        strSql = strSql & "   AND d.dri_type" & LangSuffix() & " is null"
      Else
        strSql = strSql & "   AND d.dri_type" & LangSuffix() & " = '" & sType & "'"
      End If
      strSql = strSql & "   AND s.sheet_name" & LangSuffix() & " = '" & sSheetName & "'"
      strSql = strSql & "   AND s.sheet_orientation" & LangSuffix() & " = '" & sSheetOrientation & "'"

      oApplication = GTClassFactory.Create(Of IGTApplication)()
      rsSheetScales = oApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText, vParam)

      Repos(rsSheetScales)

      If (rsSheetScales.RecordCount > 0) Then
        rsSheetScales.Close()
        Return True
      End If
      rsSheetScales.Close()
      Return False

    Catch ex As Exception
      MsgBox("GTPlotMetadata.DoesSheetSupportOrientation:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      Return False
    End Try

  End Function

    Public Function GetPaperSizes() As ADODB.Recordset
        Dim strSql As String
        Dim rsPaperSizes As ADODB.Recordset
        Dim vParam As Object = Nothing
        Dim oApplication As IGTApplication

        Try

            strSql = "select distinct sheet_size, sheet_name" & LangSuffix() & " from " & GTPlotMetadata.Parameters.GT_PLOT_SHEETS & " order by sheet_name" & LangSuffix() 'to_number(substr(sheet_size,1,2))"

            oApplication = GTClassFactory.Create(Of IGTApplication)()
            rsPaperSizes = oApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText, vParam)
            Call Repos(rsPaperSizes)

            GetPaperSizes = rsPaperSizes

        Catch ex As Exception
            MsgBox("GTPlotMetadata.GetPaperSizes:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
            GetPaperSizes = Nothing
        End Try

    End Function

    Public Function GetPaperSizes(ByVal sType As String, ByVal sSheetOrientation As String, Optional ByVal ExcludeNullScaledSheets As Boolean = True) As ADODB.Recordset
        Dim strSql As String
        Dim rsPaperSizes As ADODB.Recordset
        Dim vParam As Object = Nothing
        Dim oApplication As IGTApplication
        oApplication = GTClassFactory.Create(Of IGTApplication)()

        Try
            If String.IsNullOrEmpty(sSheetOrientation) Then
                sSheetOrientation = "Portrait"
            End If

            strSql = "   SELECT   DISTINCT S.SHEET_SIZE, S.SHEET_NAME" & LangSuffix() & ", S.SHEET_ORIENTATION "
            strSql = strSql & "    FROM   " & GTPlotMetadata.Parameters.GT_PLOT_SHEETS & " S, " & GTPlotMetadata.Parameters.GT_PLOT_DRAWINGINFO & " D "
            strSql = strSql & "   WHERE       S.SHEET_ID = D.SHEET_ID "
            If String.IsNullOrEmpty(sType) Then
                strSql = strSql & "           AND DRI_TYPE" & LangSuffix() & " IS NULL "
            Else
                strSql = strSql & "           AND DRI_TYPE" & LangSuffix() & " = '" & sType & "'"
            End If
            strSql = strSql & "           AND SHEET_ORIENTATION" & LangSuffix() & " = '" & sSheetOrientation & "'"
            If ExcludeNullScaledSheets Then
                strSql = strSql & "           AND DRI_SCALES IS NOT NULL "
            End If
            strSql = strSql & "ORDER BY   S.SHEET_NAME" & LangSuffix() & " "

            oApplication = GTClassFactory.Create(Of IGTApplication)()
            rsPaperSizes = oApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText, vParam)
            Call Repos(rsPaperSizes)

            GetPaperSizes = rsPaperSizes

        Catch ex As Exception
            MsgBox("GTPlotMetadata.GetPaperSizes:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
            GetPaperSizes = Nothing
        End Try

    End Function

    Public Function GetTypes(Optional ByVal ExcludeNullScaledSheets As Boolean = True) As ADODB.Recordset
    Dim strSql As String
    Dim rsTypes As ADODB.Recordset
    Dim vParam As Object = Nothing
    Dim oApplication As IGTApplication

    Try

      strSql = " SELECT DISTINCT dri_type" & LangSuffix() & " "
      strSql = strSql & "           FROM " & GTPlotMetadata.Parameters.GT_PLOT_DRAWINGINFO & " "
      strSql = strSql & "          WHERE dri_type" & LangSuffix() & " IS NOT NULL "
      If ExcludeNullScaledSheets Then
        strSql = strSql & "           AND DRI_SCALES IS NOT NULL "
      End If
      strSql = strSql & "       ORDER BY dri_type" & LangSuffix() & " "

      oApplication = GTClassFactory.Create(Of IGTApplication)()
      rsTypes = oApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText, vParam)
      Call Repos(rsTypes)

      GetTypes = rsTypes

    Catch ex As Exception
      MsgBox("GTPlotMetadata.GetTypes:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      GetTypes = Nothing
    End Try

  End Function

  Public Sub GetMapLocation(ByVal iDRI_ID As Integer, ByRef oMapTLPoint As IGTPoint, ByRef oMapBRPoint As IGTPoint)
    Dim rsMapSize As ADODB.Recordset
    Dim vParam As Object = Nothing

    Try
      rsMapSize = GetPrimaryMapFrame(iDRI_ID)

      Repos(rsMapSize)

      oMapTLPoint = GTClassFactory.Create(Of IGTPoint)()
      oMapTLPoint.X = rsMapSize.Fields("MF_COORDINATE_X1").Value + rsMapSize.Fields("GROUP_OFFSET_X").Value
      oMapTLPoint.Y = rsMapSize.Fields("MF_COORDINATE_Y1").Value + rsMapSize.Fields("GROUP_OFFSET_Y").Value

      oMapBRPoint = GTClassFactory.Create(Of IGTPoint)()
      oMapBRPoint.X = rsMapSize.Fields("MF_COORDINATE_X2").Value + rsMapSize.Fields("GROUP_OFFSET_X").Value
      oMapBRPoint.Y = rsMapSize.Fields("MF_COORDINATE_Y2").Value + rsMapSize.Fields("GROUP_OFFSET_Y").Value

      rsMapSize.Close()

    Catch ex As Exception
      MsgBox("GTPlotMetadata.GetMapLocation:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    End Try

  End Sub

  Public Function GetPrimaryMapFrame(ByVal iDRI_ID) As ADODB.Recordset
    Dim Sql As String
    Dim rsMapFrames As ADODB.Recordset
    Dim vParam As Object = Nothing
    Dim oApplication As IGTApplication

    Try
      Sql = " SELECT   g.dri_id, g.group_no, g.group_offset_x, g.group_offset_y, mf_coordinate_x1, mf_coordinate_y1, mf_coordinate_x2, mf_coordinate_y2"
      Sql = Sql & "     FROM " & GTPlotMetadata.Parameters.GT_PLOT_GROUPS_DRI & " g, " & GTPlotMetadata.Parameters.GT_PLOT_MAPFRAME & " r"
      Sql = Sql & "    WHERE g.dri_id = '" & iDRI_ID & "'"
      Sql = Sql & "      AND g.group_no = r.group_no"
      Sql = Sql & "      AND mf_datatype = 'Map Frame'"
      Sql = Sql & "      AND userplace = 1 "
      Sql = Sql & " ORDER BY group_no" ' Order by because the 1st one in the group is the man map frame

      oApplication = GTClassFactory.Create(Of IGTApplication)()
      rsMapFrames = oApplication.DataContext.OpenRecordset(Sql, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText, vParam)

      Repos(rsMapFrames)

      If rsMapFrames.BOF And rsMapFrames.EOF Then
        MsgBox("GTPlotMetadata.GetPrimaryMapFrame:" & vbCrLf & "Unable to locate primary map frame for DRI_ID=" & iDRI_ID & vbCrLf & Sql, vbOKOnly + vbExclamation)
        GetPrimaryMapFrame = Nothing
      Else
        GetPrimaryMapFrame = rsMapFrames
      End If


    Catch ex As Exception
      MsgBox("GTPlotMetadata.GetPrimaryMapFrame:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      GetPrimaryMapFrame = Nothing
    End Try

  End Function

  Public Function GetRedlines(ByVal iDRI_ID As Integer) As ADODB.Recordset
    Dim Sql As String
    Dim vParam As Object = Nothing
    Dim oApplication As IGTApplication

    Try
      Sql = " SELECT   g.GROUP_NO, "
      Sql = Sql & "         g.GROUP_NAME, "
      Sql = Sql & "         g.DRI_ID, "
      Sql = Sql & "         g.USERPLACE, "
      Sql = Sql & "         g.GROUP_OFFSET_X, "
      Sql = Sql & "         g.GROUP_OFFSET_Y, "
      Sql = Sql & "         r.RL_NO, "
      Sql = Sql & "         r.RL_DATATYPE, "
      Sql = Sql & "         r.RL_COORDINATE_X1, "
      Sql = Sql & "         r.RL_COORDINATE_Y1, "
      Sql = Sql & "         r.RL_COORDINATE_X2, "
      Sql = Sql & "         r.RL_COORDINATE_Y2, "
      Sql = Sql & "         r.RL_STYLE_NUMBER, "
      Sql = Sql & "         r.RL_TEXT_ALIGNMENT, "
      Sql = Sql & "         r.RL_ROTATION, "
      Sql = Sql & "         r.RL_TEXT" & LangSuffix() & ", "
      Sql = Sql & "         r.RL_USERINPUT, "
      Sql = Sql & "         r.RL_NAME" & LangSuffix() & ", "
      Sql = Sql & "         r.RL_TEXT_DEFAULT" & LangSuffix() & " "
      Sql = Sql & "  FROM   " & GTPlotMetadata.Parameters.GT_PLOT_GROUPS_DRI & " g, " & GTPlotMetadata.Parameters.GT_PLOT_REDLINES & " r "
      Sql = Sql & " WHERE       g.dri_id = '" & iDRI_ID & "' "
      Sql = Sql & "         AND g.group_no = r.group_no "
      Sql = Sql & "         AND rl_datatype = 'Redline Lines' "

            oApplication = GTClassFactory.Create(Of IGTApplication)()
            GetRedlines = oApplication.DataContext.OpenRecordset(Sql, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText, vParam)

      Repos(GetRedlines)

    Catch ex As Exception
      MsgBox("GTPlotMetadata.GetRedlines:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      GetRedlines = Nothing
    End Try

  End Function

  ' Todo -GetNorthArrowSize function is incomplete -Seems to be getting legend numbers not Arrows Size.  Also doesn't seem to be used anywhere.
  Public Function GetNorthArrowSize(ByVal sType As String, ByVal sSheetSize As String, ByVal sSheetOrientation As String, Optional ByVal bDetailLegend As Boolean = False) As Integer

    Dim strSql As String
    Dim rs As ADODB.Recordset
    Dim vParam As Object = Nothing
    Dim oApplication As IGTApplication

    Try
      If bDetailLegend Then
        strSql = " SELECT mf_det_lno "
      Else
        strSql = " SELECT mf_geo_lno "
      End If
      strSql = strSql & "  FROM " & GTPlotMetadata.Parameters.GT_PLOT_MAPFRAME & " "
      strSql = strSql & " WHERE group_no IN ( "
      strSql = strSql & "          SELECT group_no "
      strSql = strSql & "            FROM " & GTPlotMetadata.Parameters.GT_PLOT_GROUPS_DRI & " "
      strSql = strSql & "           WHERE dri_id IN ( "
      strSql = strSql & "                    SELECT dri_id "
      strSql = strSql & "                      FROM " & GTPlotMetadata.Parameters.GT_PLOT_DRAWINGINFO & " "
      strSql = strSql & "                     WHERE dri_type" & LangSuffix() & " " & IIf(sType = "", "IS NULL", "='" & sType & "'")
      strSql = strSql & "                       AND sheet_id IN ( "
      strSql = strSql & "                              SELECT sheet_id "
      strSql = strSql & "                                FROM " & GTPlotMetadata.Parameters.GT_PLOT_SHEETS & " "
      strSql = strSql & "                               WHERE sheet_size = '" & sSheetSize & "' "
      strSql = strSql & "                                 AND sheet_orientation" & LangSuffix() & " = '" & sSheetOrientation & "'))) "

      oApplication = GTClassFactory.Create(Of IGTApplication)()
      rs = oApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText, vParam)
      Call Repos(rs)

      If bDetailLegend Then
        GetNorthArrowSize = rs.Fields("mf_det_lno").Value
      Else
        GetNorthArrowSize = rs.Fields("mf_geo_lno").Value
      End If

    Catch ex As Exception
      MsgBox("GTPlotMetadata.GetTypesDefaultLegend:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    End Try

  End Function

  Public Function GetTypesDefaultLegend(ByVal sType As String, ByVal sSheetSize As String, ByVal sSheetOrientation As String, Optional ByVal bDetailLegend As Boolean = False) As Integer

    Dim strSql As String
    Dim rs As ADODB.Recordset
    Dim vParam As Object = Nothing
    Dim oApplication As IGTApplication

    Try
      If bDetailLegend Then
        strSql = " SELECT mf_det_lno "
      Else
        strSql = " SELECT mf_geo_lno "
      End If
      strSql = strSql & "  FROM " & GTPlotMetadata.Parameters.GT_PLOT_MAPFRAME & " "
      strSql = strSql & " WHERE group_no IN ( "
      strSql = strSql & "          SELECT group_no "
      strSql = strSql & "            FROM " & GTPlotMetadata.Parameters.GT_PLOT_GROUPS_DRI & " "
      strSql = strSql & "           WHERE userplace=1 "
      strSql = strSql & "             AND dri_id IN ( "
      strSql = strSql & "                    SELECT dri_id "
      strSql = strSql & "                      FROM " & GTPlotMetadata.Parameters.GT_PLOT_DRAWINGINFO & " "
      strSql = strSql & "                     WHERE upper(dri_type" & LangSuffix() & ") " & IIf(sType = "", "IS NULL", "=upper('" & sType & "')")
      strSql = strSql & "                       AND sheet_id IN ( "
      strSql = strSql & "                              SELECT sheet_id "
      strSql = strSql & "                                FROM " & GTPlotMetadata.Parameters.GT_PLOT_SHEETS & " "
      strSql = strSql & "                               WHERE upper(sheet_size) = upper('" & sSheetSize & "') "
      strSql = strSql & "                                 AND upper(sheet_orientation" & LangSuffix() & ") = upper('" & sSheetOrientation & "')))) "

      oApplication = GTClassFactory.Create(Of IGTApplication)()
      rs = oApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText, vParam)
      Call Repos(rs)

      If bDetailLegend Then
        GetTypesDefaultLegend = rs.Fields("mf_det_lno").Value
      Else
        GetTypesDefaultLegend = rs.Fields("mf_geo_lno").Value
      End If

    Catch ex As Exception
      MsgBox("GTPlotMetadata.GetTypesDefaultLegend:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    End Try

  End Function

  Public Function GetLegendFilters(ByVal sType As String, ByVal iLNO As Integer) As ADODB.Recordset

    Dim strSql As String
    Dim rs As ADODB.Recordset
    Dim vParam As Object = Nothing
    Dim oApplication As IGTApplication

    Try
      strSql = " SELECT * "
      strSql = strSql & "  FROM " & GTPlotMetadata.Parameters.GT_PLOT_LEGEND_OVERRIDE_GROUPS & " "
      strSql = strSql & " WHERE dri_type " & IIf(sType = "", "IS NULL", "='" & GetTypeKey(sType) & "'")
      strSql = strSql & " AND mf_lno = " & iLNO
      strSql = strSql & " ORDER BY lo_group_id"

      oApplication = GTClassFactory.Create(Of IGTApplication)()
      rs = oApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText, vParam)
      Call Repos(rs)

      GetLegendFilters = rs

    Catch ex As Exception
      MsgBox("GTPlotMetadata.GetLegendFilters:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      GetLegendFilters = Nothing
    End Try

  End Function

End Module
