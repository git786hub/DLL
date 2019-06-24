Imports System
Imports Intergraph.GTechnology.API

Module GTMetadata

  Public Function GetDetailOwner(ByVal iDetailID As Integer) As Feature

    Dim Sql As String
    Dim vParam As Object = Nothing
    Dim rsComponent As ADODB.Recordset
    Dim oApplication As IGTApplication
    Dim oFeature As Feature = Nothing

    Try

      Sql = " SELECT g3e_fno, g3e_fid"
      ' Todo - Add hard coded GC_DETAIL value to metadata
      Sql = Sql & "   FROM GC_DETAIL"
      Sql = Sql & "  WHERE G3E_DETAILID = '" & iDetailID & "'"

      oApplication = GTClassFactory.Create(Of IGTApplication)()
      rsComponent = oApplication.DataContext.OpenRecordset(Sql, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText, vParam)
      'Repos(rsComponent)

      oFeature = New Feature
      With oFeature
        .FNO = rsComponent.Fields("G3E_FNO").Value
        .FID = rsComponent.Fields("G3E_FID").Value
      End With
      GetDetailOwner = oFeature

    Catch ex As Exception
      MsgBox("GTMetadata.GetDetailOwner:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      GetDetailOwner = Nothing
    End Try

  End Function

  Public Function GetUsername(ByVal sComponentName As String) As String

    Dim Sql As String
    Dim vParam As Object = Nothing
    Dim rsComponent As ADODB.Recordset
    Dim oApplication As IGTApplication

    Try
      Sql = " SELECT g3e_username"
      Sql = Sql & "   FROM g3e_componentinfo_optlang"
      Sql = Sql & "  WHERE g3e_name = '" & sComponentName & "' AND g3e_lcid = '" & GetLanguage() & "'"

      oApplication = GTClassFactory.Create(Of IGTApplication)()
      rsComponent = oApplication.DataContext.OpenRecordset(Sql, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText, vParam)
      Repos(rsComponent)

      GetUsername = rsComponent.Fields("g3e_username").Value

    Catch ex As Exception
      MsgBox("GTMetadata.GetUsername:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      GetUsername = Nothing
    End Try

  End Function

  Public Function GetPrimaryCNOofFNO(ByVal iFNO As Integer) As Integer

    Dim rsMetadata As ADODB.Recordset
    Dim oApplication As IGTApplication

    Try
      oApplication = GTClassFactory.Create(Of IGTApplication)()
      rsMetadata = oApplication.DataContext.MetadataRecordset("G3E_COMPONENTVIEWS_OPTABLE")
      rsMetadata.MoveFirst()

      rsMetadata.Filter = "G3E_FNO = " & iFNO
      While rsMetadata.BOF = False And rsMetadata.EOF = False
        If Not IsDBNull(rsMetadata.Fields("G3E_PRIMARYGEOGRAPHICCNO").Value) Then
          GetPrimaryCNOofFNO = rsMetadata.Fields("G3E_PRIMARYGEOGRAPHICCNO").Value
          Exit Function
        End If
        rsMetadata.MoveNext()
      End While

    Catch ex As Exception
      MsgBox("GTMetadata::GetPrimaryCNOofFNO" & vbCrLf & "Error : " & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      GetPrimaryCNOofFNO = Nothing
    Finally
      rsMetadata = Nothing
    End Try

  End Function


  Public Function GetComponentViewofCNO(ByVal iCNO As Integer) As String

    Dim rsMetadata As ADODB.Recordset
    Dim oApplication As IGTApplication

    Try
      oApplication = GTClassFactory.Create(Of IGTApplication)()
      rsMetadata = oApplication.DataContext.MetadataRecordset("G3E_COMPONENTINFO_OPTABLE")
      rsMetadata.MoveFirst()
      rsMetadata.Filter = "G3E_CNO = " & iCNO

      GetComponentViewofCNO = rsMetadata.Fields("G3E_VIEW").Value

    Catch ex As Exception
      MsgBox("GTMetadata::GetComponentViewofCNO" & vbCrLf & "Error : " & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      GetComponentViewofCNO = Nothing
    Finally
      rsMetadata = Nothing
    End Try

  End Function

  Public Function GetComponentofCNO(ByVal iCNO As Integer) As String

    Dim rsMetadata As ADODB.Recordset
    Dim oApplication As IGTApplication

    Try
      oApplication = GTClassFactory.Create(Of IGTApplication)()
      rsMetadata = oApplication.DataContext.MetadataRecordset("G3E_COMPONENTINFO_OPTABLE")
      rsMetadata.MoveFirst()
      rsMetadata.Filter = "G3E_CNO = " & iCNO

      GetComponentofCNO = rsMetadata.Fields("G3E_TABLE").Value

    Catch ex As Exception
      MsgBox("GTMetadata::GetComponentofCNO" & vbCrLf & "Error : " & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      GetComponentofCNO = Nothing
    Finally
      rsMetadata = Nothing
    End Try

  End Function

  Public Function GetComponentCNO(ByVal sComponentName As String) As Integer

    Dim rsMetadata As ADODB.Recordset
    Dim oApplication As IGTApplication

    Try
      oApplication = GTClassFactory.Create(Of IGTApplication)()
      rsMetadata = oApplication.DataContext.MetadataRecordset("G3E_COMPONENTINFO_OPTABLE")
      rsMetadata.MoveFirst()
      rsMetadata.Filter = "G3E_NAME = '" & sComponentName & "'"


      GetComponentCNO = CInt(rsMetadata.Fields("G3E_CNO").Value)

    Catch ex As Exception
      MsgBox("GTMetadata::GetAttributeANO" & vbCrLf & "Error : " & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      GetComponentCNO = Nothing
    Finally
      rsMetadata = Nothing
    End Try

  End Function

  Public Function GetLegendName(ByVal iLNO As Integer) As String

    Dim strSql As String
    Dim rs As ADODB.Recordset
    Dim vParam As Object = Nothing
    Dim oApplication As IGTApplication

    Try

      oApplication = GTClassFactory.Create(Of IGTApplication)()

      strSql = " SELECT   g3e_username, g3e_role"
      strSql = strSql & "    FROM g3e_legends_optlang "
      strSql = strSql & "   WHERE g3e_lno = " & iLNO
      strSql = strSql & "     AND g3e_lcid = '" & GetLanguage() & "' "

      rs = oApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText, vParam)
      Call Repos(rs)

      ' Only get legend that user has role to view...
      While Not rs.EOF
        If oApplication.DataContext.IsRoleGranted(rs.Fields("g3e_role").Value) Then
          GetLegendName = rs.Fields("g3e_username").Value
          Exit While
        End If
      rs.MoveNext()
      End While
      rs.Close()



    Catch ex As Exception
      MsgBox("GTMetadata.GetLegendName:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      GetLegendName = ""
    End Try

  End Function

  Public Function GetLegendLNO(ByVal sLegendName As String) As Integer

    Dim strSql As String
    Dim rs As ADODB.Recordset
    Dim vParam As Object = Nothing
    Dim oApplication As IGTApplication

    Try
      oApplication = GTClassFactory.Create(Of IGTApplication)()

      strSql = " SELECT   g3e_lno "
      strSql = strSql & "    FROM g3e_legends_optlang "
      strSql = strSql & "   WHERE g3e_username = '" & sLegendName & "'"
      strSql = strSql & "     AND G3E_LCID = '" & GetLanguage() & "' "

      rs = oApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText, vParam)
      Call Repos(rs)

      GetLegendLNO = rs.Fields("g3e_lno").Value

    Catch ex As Exception
      MsgBox("GTMetadata.GetLegendLNO:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    End Try

  End Function

  Public Function GetLegends(Optional ByVal bDetail As Boolean = False) As ADODB.Recordset

    Dim strSql As String
    Dim rs As ADODB.Recordset
    Dim vParam As Object = Nothing
    Dim oApplication As IGTApplication

    Try

      oApplication = GTClassFactory.Create(Of IGTApplication)()

      ' Use the new G3E_LEGENDSETTINGS.G3E_USERNAME if exists instead of G3E_LEGEND.G3E_USERNAME to get legends. 
      strSql = "SELECT table_name FROM tabs WHERE table_name = 'G3E_LEGENDSETTINGS'"
      rs = oApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText, vParam)
      Call Repos(rs)

      If Not rs.EOF Then
        strSql = "   SELECT   s.g3e_lno, l.g3e_username, s.g3e_role"
        strSql = strSql & "    FROM   g3e_legendsettings s, g3e_legends_optlang l "
        strSql = strSql & "   WHERE   s.g3e_lno = l.g3e_lno AND l.g3e_detail = " & IIf(bDetail, 1, 0) & " "
        strSql = strSql & "     AND   l.g3e_lcid = '" & GetLanguage() & "' "
        strSql = strSql & "ORDER BY   s.g3e_username "
      Else
        strSql = " SELECT   g3e_lno, g3e_username, g3e_role "
        strSql = strSql & "    FROM g3e_legends_optlang "
        strSql = strSql & "   WHERE g3e_detail = " & IIf(bDetail, 1, 0) & " "
        strSql = strSql & "     AND g3e_lcid = '" & GetLanguage() & "' "
        strSql = strSql & "ORDER BY g3e_username"
      End If

      rs = oApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText, vParam)
      Call Repos(rs)

      GetLegends = rs

    Catch ex As Exception
      MsgBox("GTMetadata.GetLegends:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      GetLegends = Nothing
    End Try

  End Function

  Public Function GetStyleSubstitution() As ADODB.Recordset

    Dim strSql As String
    Dim rs As ADODB.Recordset
    Dim vParam As Object = Nothing
    Dim oApplication As IGTApplication

    Try
      strSql = " SELECT   g3e_username "
      strSql = strSql & "    FROM G3E_STYLETYPE_OPTLANG "
      strSql = strSql & "   WHERE g3e_lcid = '" & GetLanguage() & "' "
      strSql = strSql & "ORDER BY g3e_username"

      oApplication = GTClassFactory.Create(Of IGTApplication)()
      rs = oApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText, vParam)
      Call Repos(rs)

      GetStyleSubstitution = rs

    Catch ex As Exception
      MsgBox("GTMetadata.GetStyleSubstitution:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      GetStyleSubstitution = Nothing
    End Try

  End Function

End Module
