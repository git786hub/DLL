Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Windows.Forms
Imports Intergraph.GTechnology.API
Imports Intergraph.GTechnology.Interfaces
Imports System.Drawing
Imports System.ComponentModel
Imports System.Globalization

' Must have references to G3E.tlb, Mapview.tlb, PView.tlb

Public Class GFramme

  'Public WithEvents GMNorthArrowEvents As GTech.Interop.NorthArrow.GMNorthArrow

  Private iNorthArrowSize As Integer
  Private sNorthArrowSymbolFile As String
  Private m_oApplication As GTech.Interop.G3E.Application

  ReadOnly Property Application() As GTech.Interop.G3E.Application
    Get
      Application = m_oApplication
    End Get
  End Property

  Public Sub InsertNorthArrow(ByVal oPlotBoundary As PlotBoundary, ByVal iNorthArrowSize As Integer, ByVal sNorthArrowSymbolFile As String)

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


    Try
      oGTApplication = GTClassFactory.Create(Of IGTApplication)()
      sStatusBarText = oGTApplication.GetStatusBarText(GTStatusPanelConstants.gtaspcMessage)

      If sNorthArrowSymbolFile = "" Then
        Exit Sub
      End If



      Dim oWindow As Object
      For Each oWindow In Application.Windows
        If (oWindow.Caption = oPlotBoundary.Name) Then
          oPlotWindow = oWindow

          oNamedPlot = oPlotWindow.NamedPlot
          oPlotFrames = oNamedPlot.Frames
          oPlotFrame = oPlotFrames.Item(oPlotBoundary.MapFrameName)

          If oPlotFrame.Type = "PlotMapFrame" Then
            oPlotMapFrame = oPlotFrame
            oMapView = oPlotMapFrame.MapView
            Exit For
          End If

        End If
      Next

      If Not oPlotMapFrame Is Nothing Then

        ' Add North Arrow to Map
        oPlotSheet = New GTech.Interop.G3E.PlotSheet
        oPlotSheet = oPlotWindow.Sheet
        oPlotSheet.InsertNorthArrow()

        oNorthArrow = oPlotWindow.NamedPlot.NorthArrow
        With oNorthArrow
          '.MaintainRelativePosition = True
          '.UserDefinedAzimuth = False
          .SymbolFile = sNorthArrowSymbolFile
          .Size = iNorthArrowSize
          .MapViewID = oPlotMapFrame.MapView
          .Refresh()
        End With
        oMapView.Refresh()
        oPlotSheet.Refresh()

      End If

    Catch ex As Exception
      MsgBox("GFramme.InsertNorthArrow:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)

    Finally
      'GTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, sStatusBarText)
      oMapView = Nothing
      oWindows = Nothing
      oPlotWindow = Nothing
      oPlotFrames = Nothing
      oPlotFrame = Nothing
      oPlotMapFrame = Nothing
      oPlotSheet = Nothing
      oNamedPlot = Nothing
      oNamedPlots = Nothing
      oNorthArrow = Nothing
      oLegEntry = Nothing

    End Try

  End Sub

  Public Sub ZoomToFitPage(ByVal sPlotWindowCaption As String)

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

    Try
      oGTApplication = GTClassFactory.Create(Of IGTApplication)()
      sStatusBarText = oGTApplication.GetStatusBarText(GTStatusPanelConstants.gtaspcMessage)

      Dim oWindow As Object
      For Each oWindow In Application.Windows
        If (oWindow.Caption = sPlotWindowCaption) Then
          oPlotWindow = oWindow
          Exit For
        End If
      Next
      oPlotWindow.ZoomToFit() 'Fit Page
      'oPlotWindow.ZoomOut()

    Catch ex As Exception
      MsgBox("GFramme.ZoomToFit:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)

    Finally
      'GTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, sStatusBarText)
      oMapView = Nothing
      oWindows = Nothing
      oPlotWindow = Nothing
      oPlotFrames = Nothing
      oPlotFrame = Nothing
      oPlotMapFrame = Nothing
      oPlotSheet = Nothing
      oNamedPlot = Nothing
      oNamedPlots = Nothing
      oNorthArrow = Nothing
      oLegEntry = Nothing

    End Try

  End Sub

  Public Sub RotateMapView(ByVal oPlotBoundary As PlotBoundary, ByVal dAngle As Double)

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

    Try
      oGTApplication = GTClassFactory.Create(Of IGTApplication)()
      sStatusBarText = oGTApplication.GetStatusBarText(GTStatusPanelConstants.gtaspcMessage)

      Dim oWindow As Object
      For Each oWindow In Application.Windows
        If (oWindow.Caption = oPlotBoundary.Name) Then
          oPlotWindow = oWindow

          oNamedPlot = oPlotWindow.NamedPlot
          oPlotFrames = oNamedPlot.Frames
          oPlotFrame = oPlotFrames.Item(oPlotBoundary.MapFrameName)

          If oPlotFrame.Type = "PlotMapFrame" Then
            oPlotMapFrame = oPlotFrame
            oMapView = oPlotMapFrame.MapView
            Exit For
          End If

        End If
      Next

      If Not oPlotMapFrame Is Nothing Then
        oMapView.Rotate(dAngle)
        oMapView.Refresh()
        'oPlotSheet.Refresh()
      End If

    Catch ex As Exception
      MsgBox("GFramme.RotateMapView:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)

    Finally
      'GTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, sStatusBarText)
      oMapView = Nothing
      oWindows = Nothing
      oPlotWindow = Nothing
      oPlotFrames = Nothing
      oPlotFrame = Nothing
      oPlotMapFrame = Nothing
      oPlotSheet = Nothing
      oNamedPlot = Nothing
      oNamedPlots = Nothing
      oNorthArrow = Nothing
      oLegEntry = Nothing

    End Try

  End Sub

  Public Sub New()
    'm_oApplication = GTApplication.Application
    m_oApplication = GetObject(, "GFramme.Application")
  End Sub

  Protected Overrides Sub Finalize()
    m_oApplication = Nothing
    MyBase.Finalize()
  End Sub
End Class

