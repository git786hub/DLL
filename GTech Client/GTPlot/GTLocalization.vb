Imports System
Imports Intergraph.GTechnology.API
Imports System.Globalization
Imports System.Security.Permissions
Imports System.Threading

Module GTLocalization

  Public Function GetCurrentCulture() As String

    ' Displays the name of the CurrentCulture of the current thread.
    Debug.Print("CurrentCulture is {0}.", CultureInfo.CurrentCulture.Name)

    ' Displays the name of the CurrentUICulture of the current thread.
    Debug.Print("CurrentUICulture is {0}.", CultureInfo.CurrentUICulture.Name)

    GetCurrentCulture = CultureInfo.CurrentCulture.Name

  End Function

  Public Sub SetCurrentCulture(Optional ByVal sCultureInfo As String = "fr-CA")

    Try

      ' Displays the name of the CurrentCulture of the current thread.
      ' Displays the name of the CurrentUICulture of the current thread.
      Debug.Print("CurrentCulture before change is {0}.", CultureInfo.CurrentCulture.Name)
      Debug.Print("CurrentUICulture before change is {0}.", CultureInfo.CurrentUICulture.Name)

      ' Changes the CurrentCulture of the current thread to fr-CA.
      ' Changes the CurrentUICulture of the current thread to fr.
      Thread.CurrentThread.CurrentCulture = New CultureInfo(sCultureInfo, False)
      Thread.CurrentThread.CurrentUICulture = New CultureInfo(sCultureInfo, False)

      ' Displays the name of the CurrentCulture of the current thread.
      ' Displays the name of the CurrentUICulture of the current thread.
      Debug.Print("CurrentCulture after change is now {0}.", CultureInfo.CurrentCulture.Name)
      Debug.Print("CurrentUICulture after change is now {0}.", CultureInfo.CurrentUICulture.Name)

    Catch ex As Exception
      MsgBox("GTLocalization.SetCurrentCulture:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)

    End Try

  End Sub

  Declare Function GetUserDefaultLCID Lib "Kernel32" () As Integer



  Public Function LangSuffix() As String

    Try
      If GetLanguage() = "000C" Then
        LangSuffix = "_FR"
      Else
        LangSuffix = ""
      End If

    Catch ex As Exception
      MsgBox("GTLocalization.LangSuffix:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      LangSuffix = ""
    End Try

  End Function



  Public Function GetLanguage() As String
    Dim LCID As Short
    Dim PLangId As Short
    Dim sLangId As Short
    Dim slang As String

    Try
      'Get current user LCID
      LCID = GetUserDefaultLCID
      'LCID's Primary language ID
      PLangId = (LCID And &H3FFS)
      sLangId = (LCID / (2 ^ 10))
      'Msgbox "Language = 000" & PLangId
      If PLangId = 9 Then
        slang = "000" & PLangId
      ElseIf PLangId = 12 Then
        slang = "000C"
      Else
        slang = "0009"
      End If

      GetLanguage = slang

    Catch ex As Exception
      MsgBox("GTLocalization.GetLanguage:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      GetLanguage = Nothing
    End Try

  End Function

End Module
