Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Windows.Forms
Imports Intergraph.GTechnology.API
Imports Intergraph.GTechnology.Interfaces
Imports System.Drawing
Imports System.ComponentModel
Imports System.Globalization

'Imports GTech.Interop
'Imports GTech.Interop.G3E
'Imports GTech.Interop.MapView
'Imports GTech.Interop.PView
'Imports GTech.Interop.NorthArrow

Public Class LegendOverrides

  Private Sub AddLegendOverrides(ByVal oLegendEntries As GTech.Interop.PView.LegendEntries, ByVal sLegendOverrideName As String, Optional ByVal bChecked As Boolean = True)

    Dim oLegEntry As GTech.Interop.PView.[_DGMBaseLegendEntry] = Nothing
    Dim rs As ADODB.Recordset
    Dim sStatusBarText As String
    Dim oGTApplication As IGTApplication
    oGTApplication = GTClassFactory.Create(Of IGTApplication)()
    sStatusBarText = oGTApplication.GetStatusBarText(GTStatusPanelConstants.gtaspcMessage)

    Try

      For Each oLegEntry In oLegendEntries
        rs = GetLegendItemsToFilters(sLegendOverrideName)
        'rs.MoveFirst()
        'rs.Find("LO_LEGENDITEM=" & oLegEntry.Name)
        While Not rs.EOF
          If rs.Fields("LO_LEGENDITEM").Value = oLegEntry.Name Then

            ' User feedback
            oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, sStatusBarText & " - " & oLegEntry.Name)

            ' Set DisplayMode
            If bChecked Then 'Checked
              If Not IsDBNull(rs.Fields("LO_DISPLAYMODE").Value) Then oLegEntry.DisplayMode = IIf(rs.Fields("LO_DISPLAYMODE").Value = 0, False, True)
            Else 'Unchecked
              If Not IsDBNull(rs.Fields("LO_DISPLAYMODE_UNCHECKED").Value) Then oLegEntry.DisplayMode = IIf(rs.Fields("LO_DISPLAYMODE_UNCHECKED").Value = 0, False, True)
            End If


            ' Set Filter
            If bChecked Then 'Checked
              If Not IsDBNull(rs.Fields("LO_FILTER").Value) Then
                If Not Trim(rs.Fields("LO_FILTER").Value) = "" Then
                  If oLegEntry.Filter = "" Then
                    oLegEntry.Filter = rs.Fields("LO_FILTER").Value
                  Else
                    oLegEntry.Filter = oLegEntry.Filter & " OR " & rs.Fields("LO_FILTER").Value
                  End If
                End If
              End If
            Else 'Unchecked
              If Not IsDBNull(rs.Fields("LO_FILTER_UNCHECKED").Value) Then
                If Not Trim(rs.Fields("LO_FILTER_UNCHECKED").Value) = "" Then
                  If oLegEntry.Filter = "" Then
                    oLegEntry.Filter = rs.Fields("LO_FILTER_UNCHECKED").Value
                  Else
                    oLegEntry.Filter = oLegEntry.Filter & " AND " & rs.Fields("LO_FILTER_UNCHECKED").Value
                  End If
                End If
              End If
            End If

            Try
              oLegEntry.PrepareRecordset()
            Catch ex As Exception
              ' TODO: Look to see if legend item is a Catagory of legend items.  If so set all legend item filters for Catagory.
              ' Look through the objects to find where the filter is set at Level 1 etc.
              MsgBox("Problem occurred while applying the filter " & oLegEntry.Filter & " to the Legend Entry " & oLegEntry.Name & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source & " - " & "LegendOverrides")
            End Try
          End If
          rs.MoveNext()
        End While
        rs.Close()
      Next

    Catch ex As Exception
      MsgBox("LegendOverrides.AddLegendOverrides:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)

    Finally
      oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, sStatusBarText)

    End Try

  End Sub

  Private Function GetLegendItemsToFilters(ByVal sLegendOverrideName As String) As ADODB.Recordset

    Dim strSql As String
    Dim rs As ADODB.Recordset
    Dim vParam As Object = Nothing
    Dim oGTApplication As IGTApplication

    Try
      strSql = " SELECT * "
      strSql = strSql & "  FROM " & GTPlotMetadata.Parameters.GT_PLOT_LEGEND_OVERRIDE_ITEMS & " "
      strSql = strSql & " WHERE lo_group_id IN (SELECT lo_group_id "
      strSql = strSql & "                         FROM " & GTPlotMetadata.Parameters.GT_PLOT_LEGEND_OVERRIDE_GROUPS & " "
      strSql = strSql & "                        WHERE lo_username = '" & sLegendOverrideName & "')"

      oGTApplication = GTClassFactory.Create(Of IGTApplication)()
      rs = oGTApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText, vParam)
      Call Repos(rs)

      GetLegendItemsToFilters = rs

    Catch ex As Exception
      MsgBox("PlotBoundaryForm.GetLegendItemsToFilters:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      GetLegendItemsToFilters = Nothing
    End Try

  End Function

  Public Sub Repos(ByRef rs As ADODB.Recordset)
    On Error Resume Next
    If rs.BOF And rs.EOF Then Exit Sub
    rs.MoveLast()
    rs.MoveFirst()
    On Error GoTo 0
  End Sub

  Public Sub ApplyLegendOverrides(ByVal oPlotBoundary As PlotBoundary)

    ' Must have references to G3E.tlb, Mapview.tlb, PView.tlb

    Dim oApp As GTech.Interop.G3E.Application
    Dim oMapView As GTech.Interop.MapView.GMMapView = Nothing ' MapviewLib.GMMapView
    Dim oWindows As GTech.Interop.G3E.Windows = Nothing ' .MapView..GMMapView ' MapviewLib.GMMapView
    'Dim eWindowConstants As GTech.Interop.G3E.WindowConstants
    Dim oPlotWindow As GTech.Interop.G3E.PlotWindow = Nothing
    Dim oPlotFrames As GTech.Interop.G3E.PlotFrames = Nothing
    Dim oPlotFrame As GTech.Interop.G3E.PlotFrame = Nothing
    Dim oPlotMapFrame As GTech.Interop.G3E.PlotMapFrame = Nothing
    Dim oPlotSheet As GTech.Interop.G3E.PlotSheet = Nothing
    Dim oNamedPlot As GTech.Interop.G3E.NamedPlot = Nothing
    Dim oNamedPlots As GTech.Interop.G3E.NamedPlots = Nothing
    Dim oNorthArrow As GTech.Interop.NorthArrow.GMNorthArrow = Nothing

    Dim oLegEntry As GTech.Interop.PView.[_DGMBaseLegendEntry] = Nothing

    Dim sStatusBarText As String
    Dim oGTApplication As IGTApplication
    oGTApplication = GTClassFactory.Create(Of IGTApplication)()
    sStatusBarText = oGTApplication.GetStatusBarText(GTStatusPanelConstants.gtaspcMessage)

    Try

      'oApp = GTApplication.Application
      oApp = GetObject(, "GFramme.Application")

      Dim oWindow As Object
      For Each oWindow In oApp.Windows
        If (oWindow.Caption = oPlotBoundary.Name) Then
          oPlotWindow = oWindow
          oNamedPlot = oPlotWindow.NamedPlot
          oPlotFrames = oNamedPlot.Frames
          oPlotFrame = oPlotFrames.Item(oPlotBoundary.MapFrameName)
          If oPlotFrame.Type = "PlotMapFrame" Then
            oPlotMapFrame = oPlotFrame
          End If
        End If
      Next

      If Not oPlotMapFrame Is Nothing Then

        ' User feedback
        oGTApplication.BeginProgressBar()
        oGTApplication.SetProgressBarRange(0, oPlotBoundary.LegendOverrides.Count)
        oGTApplication.GetStatusBarText(GTStatusPanelConstants.gtaspcMessage)

        Dim i As Integer = 1
        For Each oLegendOverride As LegendOverride In oPlotBoundary.LegendOverrides
          ' User feedback
          oGTApplication.SetProgressBarPosition(i)
          oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, sStatusBarText & " - Applying Legend Override: " & oLegendOverride.Username)
          If oLegendOverride.User_Option_Value = 0 Or oLegendOverride.User_Option_Value = 1 Then
            AddLegendOverrides(oPlotMapFrame.MapView.Legend.LegendEntries, oLegendOverride.Username, oLegendOverride.User_Option_Value)
          End If
          i = i + 1
        Next
        oPlotMapFrame.MapView.Refresh()
      End If

      oGTApplication.EndProgressBar()

    Catch ex As Exception
      MsgBox("LegendOverrides.ApplyLegendOverrides:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)

    Finally
      oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, sStatusBarText)

    End Try

  End Sub


  '
  '
  ' N O T   U S E D
  '
  ' For Testing Only
  '
  Public Sub ApplyLegendOverrides2(ByVal oPlotBoundary As PlotBoundary)

    ' Must have references to G3E.tlb, Mapview.tlb, PView.tlb

    Dim oApp As GTech.Interop.G3E.Application
    Dim oMapView As GTech.Interop.MapView.GMMapView = Nothing ' MapviewLib.GMMapView
    Dim oWindows As GTech.Interop.G3E.Windows = Nothing ' .MapView..GMMapView ' MapviewLib.GMMapView
    'Dim eWindowConstants As GTech.Interop.G3E.WindowConstants
    Dim oPlotWindow As GTech.Interop.G3E.PlotWindow = Nothing
    Dim oPlotFrames As GTech.Interop.G3E.PlotFrames = Nothing
    Dim oPlotFrame As GTech.Interop.G3E.PlotFrame = Nothing
    Dim oPlotMapFrame As GTech.Interop.G3E.PlotMapFrame = Nothing
    Dim oPlotSheet As GTech.Interop.G3E.PlotSheet = Nothing
    Dim oNamedPlot As GTech.Interop.G3E.NamedPlot = Nothing
    Dim oNamedPlots As GTech.Interop.G3E.NamedPlots = Nothing

    Dim oLegEntry As GTech.Interop.PView.[_DGMBaseLegendEntry] = Nothing
    Dim oGTApplication As IGTApplication

    'oApp = GTApplication.Application
    oApp = GetObject(, "GFramme.Application")
    'oMapView = oApp.ActiveWindow.MapView
    ''oMapView = oApp.RegUsername


    oGTApplication = GTClassFactory.Create(Of IGTApplication)()
    oPlotMapFrame = oGTApplication.ActivePlotWindow
    Dim oWindow As Object
    For Each oWindow In oApp.Windows
      If (oWindow.Caption = "NamedPlot1") Then
        oPlotWindow = oWindow
      End If
    Next
    oNamedPlot = oPlotWindow.NamedPlot
    oPlotFrames = oNamedPlot.Frames

    For Each oPlotFrame In oPlotFrames
      Debug.Print(oPlotFrame.Name)

      If oPlotFrame.IsActive Then
        MsgBox("Active Mape Frame")
      End If
      If oPlotFrame.Type = "PlotMapFrame" Then
        oPlotMapFrame = oPlotFrame

        'oPlotMapFrame.Activate(oPlotSheet)

        'oPlotMapFrame.Deactivate(oPlotSheet)


        For Each oLegEntry In oPlotMapFrame.MapView.Legend.LegendEntries

          Debug.Print(oLegEntry.Name)

          Select Case oLegEntry.Name
            Case "V_POLE_PT"
              oLegEntry.DisplayMode = True
              oLegEntry.Filter = "CLASS<>'5'"
              oLegEntry.PrepareRecordset()
            Case "V_PRICOND_FAC_LN"
              oLegEntry.DisplayMode = True
              oLegEntry.Filter = "STATE<>'INS'"
              oLegEntry.PrepareRecordset()
            Case "V_PRICOND_SLD_LN"
              oLegEntry.DisplayMode = True
              oLegEntry.Filter = "STATE<>'INS'"
              oLegEntry.PrepareRecordset()
            Case Is = "V_PLOTBNDY_AR"
              oLegEntry.DisplayMode = False
              oLegEntry.PrepareRecordset()
          End Select
        Next

        oPlotMapFrame.MapView.Refresh()

      End If
    Next


    ''For Each oNamedPlot In oNamedPlots
    ''  If oNamedPlot.Name = "101" Then
    ''    oPlotFrames = oNamedPlot.Frames
    ''    For Each oPlotFrame In oPlotFrames
    ''      Debug.Print(oPlotFrame.Name)

    ''      'oPlotMapFrame.MapView.Legend.LegendEntries

    ''      'oPlotFrame.Activate(oPlotSheet)
    ''      'oPlotSheet.Refresh

    ''      oPlotFrame.Deactivate(oPlotSheet)
    ''    Next
    ''  End If
    ''Next
    ''oMapView.Refresh()
  End Sub

End Class
