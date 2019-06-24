Imports System
Imports Intergraph.GTechnology.API

Module GTTools

  Public Sub WriteRs(ByVal rs As ADODB.Recordset, Optional ByVal sFilenName As String = "c:\temp\rs.txt")
    Dim varOutput As Object

    MsgBox("In WriteRs")

    If rs.EOF And rs.BOF Then Exit Sub


    varOutput = rs.GetString(ADODB.StringFormatEnum.adClipString)

    My.Computer.FileSystem.WriteAllText(sFilenName, varOutput, False)

    'Open sFilenName For Append As #1
    'Write #1, varOutput
    'Close #1
    MsgBox("Exit WriteRs")

  End Sub

  Public Sub Repos(ByRef rs As ADODB.Recordset)
    On Error Resume Next
    rs.MoveLast()
    rs.MoveFirst()
    On Error GoTo 0
  End Sub

  ' Determine if using Metric or Imperial scaling...
  Enum MeasurmentUnits
    Metric
    Imperial
  End Enum

  Public Function GetMeasurmentUnits(scale As String) As MeasurmentUnits
    Dim eMeasurmentUnits As MeasurmentUnits
    If (Mid(scale, 1, 2) = "1:") Then
      eMeasurmentUnits = MeasurmentUnits.Metric
    ElseIf (Mid(scale, 1, 3) = "1""=") Then
      eMeasurmentUnits = MeasurmentUnits.Imperial
    End If
    Return eMeasurmentUnits
  End Function

  Public Function GetScaleValue(scale As String) As Int32
    Dim eMeasurmentUnits = GetMeasurmentUnits(scale)
    If (eMeasurmentUnits = MeasurmentUnits.Metric) Then
      ' Get Metric Map Scale & Scaled Map Size
      Return CInt(Mid(scale, 3, Len(scale)))
    ElseIf (eMeasurmentUnits = MeasurmentUnits.Imperial) Then
      ' Get Imperial in. to ft. Map Scale & Scaled Map Size
      Return CInt(Replace(Mid(scale, 4, Len(scale)), GTPlotMetadata.Parameters.MeasurementUnitsImperialLabelLarge, ""))
    End If
  End Function

  Public Function ValidScaleMetric(ByRef sScale As String) As Boolean
    Try
      ValidScaleMetric = False

      If Len(sScale) < 3 Then Exit Function
      If Not Mid(sScale, 1, 1) = "1" Then Exit Function
      If Not Mid(sScale, 2, 1) = ":" Then Exit Function
      If CInt(Mid(sScale, 3, Len(sScale))) = 0 Then Exit Function

      ValidScaleMetric = True

    Catch ex As Exception
      MsgBox("PlotInfoControl.ValidScale:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    End Try

  End Function

  Public Function ValidScaleImperial(ByRef sScale As String) As Boolean
    Try
      ValidScaleImperial = False

      If Len(sScale) < 4 Then Exit Function
      If Not Mid(sScale, 1, 2) = "1""" Then Exit Function
      If Not Mid(sScale, 3, 1) = "=" Then Exit Function
      If Not Mid(sScale, Len(sScale), 1) = "'" Then Exit Function
      If CInt(Mid(sScale, 4, Len(sScale) - 5)) = 0 Then Exit Function

      ValidScaleImperial = True

    Catch ex As Exception
      MsgBox("PlotBoundaryForm.ValidScaleImperial:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    End Try

  End Function

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
      MsgBox("NewPlotWindowForm.ActiveMapWindow:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    Finally
    End Try

  End Function

End Module
