Imports System
Imports Intergraph.GTechnology.API

Public Class GTJobDefinition

#Region "Properties"

  Dim m_sG3E_TABLE As String
  Dim m_sG3E_IDENTIFIERFIELD As String
  Dim m_sG3E_DESCRIPTIONFIELD As String
  Dim m_sG3E_OWNERFIELD As String

  Public Property G3E_TABLE() As String
    Get
      G3E_TABLE = m_sG3E_TABLE
    End Get
    Set(ByVal Value As String)
      m_sG3E_TABLE = Value
    End Set
  End Property

  Public Property G3E_IDENTIFIERFIELD() As String
    Get
      G3E_IDENTIFIERFIELD = m_sG3E_IDENTIFIERFIELD
    End Get
    Set(ByVal Value As String)
      m_sG3E_IDENTIFIERFIELD = Value
    End Set
  End Property

  Public Property G3E_DESCRIPTIONFIELD() As String
    Get
      G3E_DESCRIPTIONFIELD = m_sG3E_DESCRIPTIONFIELD
    End Get
    Set(ByVal Value As String)
      m_sG3E_DESCRIPTIONFIELD = Value
    End Set
  End Property

  Public Property G3E_OWNERFIELD() As String
    Get
      G3E_OWNERFIELD = m_sG3E_OWNERFIELD
    End Get
    Set(ByVal Value As String)
      m_sG3E_OWNERFIELD = Value
    End Set
  End Property

#End Region

  Private Sub SetJobDefinition()

    Dim strSql As String
    Dim oApplication As IGTApplication
    Dim rs As ADODB.Recordset
    Dim vParam As Object = Nothing

    Try

      strSql = " SELECT   g3e_table, g3e_identifierfield, g3e_descriptionfield, g3e_ownerfield "
      strSql = strSql & "  FROM   g3e_jobdefinition_optlang "
      strSql = strSql & " WHERE   g3e_lcid = '" & GetLanguage() & "' "
      oApplication = GTClassFactory.Create(Of IGTApplication)()
      rs = oApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText, vParam)
      Repos(rs)

      m_sG3E_TABLE = rs.Fields("G3E_TABLE").Value
      m_sG3E_IDENTIFIERFIELD = rs.Fields("G3E_IDENTIFIERFIELD").Value
      m_sG3E_DESCRIPTIONFIELD = rs.Fields("G3E_DESCRIPTIONFIELD").Value
      m_sG3E_OWNERFIELD = rs.Fields("G3E_OWNERFIELD").Value

    Catch ex As Exception
      MsgBox("JobDefinition.SetJobDefinition:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    End Try

  End Sub

  Public Sub New()
    SetJobDefinition()
  End Sub

  Public Function GetJobAttributeUsernameFromField(ByVal sField As String) As String

    Dim rs As ADODB.Recordset

    Try
      rs = GetJobAttributes()
      Repos(rs)

      While Not rs.EOF
        If sField = rs.Fields("G3E_FIELD").Value Then
          GetJobAttributeUsernameFromField = rs.Fields("G3E_USERNAME").Value
          Exit Function
        End If
        rs.MoveNext()
      End While
      GetJobAttributeUsernameFromField = "User"

    Catch ex As Exception
      MsgBox("GTMetadata.GetJobAttributeUsernameFromField:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      GetJobAttributeUsernameFromField = Nothing
    End Try

  End Function

  Public Function GetJobAttributeValueFromField(ByVal sJobID As String, ByVal sField As String) As String

    Dim rs As ADODB.Recordset

    Try
      rs = GetJobAttributeValues(sJobID)
      Repos(rs)

      GetJobAttributeValueFromField = Nothing
      If Not rs.EOF Then
        If Not IsDBNull(rs.Fields(sField).Value) Then
          GetJobAttributeValueFromField = rs.Fields(sField).Value
        End If
      End If

    Catch ex As Exception
      MsgBox("GTMetadata.GetJobAttribute:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      GetJobAttributeValueFromField = Nothing
    End Try

  End Function

  Public Function GetJobAttributeValues(ByVal sJobID As String) As ADODB.Recordset

    Dim strSql As String
    Dim oApplication As IGTApplication
    Dim rs As ADODB.Recordset
    Dim vParam As Object = Nothing

    Try

      strSql = " SELECT   * "
      strSql = strSql & "  FROM " & G3E_TABLE & " "
      strSql = strSql & " WHERE   " & G3E_IDENTIFIERFIELD & " = '" & sJobID & "' "

      oApplication = GTClassFactory.Create(Of IGTApplication)()
      rs = oApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText, vParam)
      Repos(rs)

      GetJobAttributeValues = rs

    Catch ex As Exception
      MsgBox("GTMetadata.GetJobAttributeValues:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      GetJobAttributeValues = Nothing
    End Try

  End Function

  Public Function GetJobAttributes() As ADODB.Recordset

    Dim sLCID As String
    Dim strSql As String
    Dim oApplication As IGTApplication
    Dim rs As ADODB.Recordset
    Dim vParam As Object = Nothing

    Try

      sLCID = GetLanguage()

      strSql = " SELECT   * "
      strSql = strSql & "  FROM   g3e_attributeinfo_optlang "
      strSql = strSql & " WHERE   g3e_lcid = '" & sLCID & "' "
      strSql = strSql & "         AND g3e_ano IN "
      strSql = strSql & "                (SELECT   DISTINCT (g3e_ano) "
      strSql = strSql & "                   FROM   g3e_dialogattributes_optlang "
      strSql = strSql & "                  WHERE   g3e_lcid = '" & sLCID & "' "
      strSql = strSql & "                          AND g3e_dtno IN "
      strSql = strSql & "                                 (SELECT   g3e_dtno "
      strSql = strSql & "                                    FROM   g3e_dialogs_optlang "
      strSql = strSql & "                                   WHERE   g3e_lcid = '" & sLCID & "' "
      strSql = strSql & "                                           AND g3e_type IN "
      strSql = strSql & "                                                  ('G3E_NEWJOB', 'G3E_EDITJOB'))) "

      oApplication = GTClassFactory.Create(Of IGTApplication)()
      rs = oApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText, vParam)
      Repos(rs)

      GetJobAttributes = rs

    Catch ex As Exception
      MsgBox("GTMetadata.GetJobAttributes:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      GetJobAttributes = Nothing
    End Try

  End Function

  Public Function GetJobAttributesNotOnDialog() As ADODB.Recordset

    Dim sLCID As String
    Dim strSql As String
    Dim oApplication As IGTApplication
    Dim rs As ADODB.Recordset
    Dim vParam As Object = Nothing

    Try

      sLCID = GetLanguage()

      strSql = " SELECT * "
      strSql = strSql & "  FROM g3e_attributeinfo_optlang "
      strSql = strSql & " WHERE g3e_cno IN "
      strSql = strSql & "          (SELECT DISTINCT G3E_CNO "
      strSql = strSql & "             FROM g3e_dialogattributes_optlang "
      strSql = strSql & "            WHERE     g3e_lcid = '" & sLCID & "' "
      strSql = strSql & "                  AND g3e_dtno IN "
      strSql = strSql & "                         (SELECT g3e_dtno "
      strSql = strSql & "                            FROM g3e_dialogs_optlang "
      strSql = strSql & "                           WHERE     g3e_lcid = '" & sLCID & "' "
      strSql = strSql & "                                 AND g3e_type IN "
      strSql = strSql & "                                        ('G3E_NEWJOB', 'G3E_EDITJOB'))) "

      oApplication = GTClassFactory.Create(Of IGTApplication)()
      rs = oApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText, vParam)
      Repos(rs)

      GetJobAttributesNotOnDialog = rs

    Catch ex As Exception
      MsgBox("GTMetadata.GetJobAttributesNotOnDialog:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      GetJobAttributesNotOnDialog = Nothing
    End Try

  End Function

End Class
