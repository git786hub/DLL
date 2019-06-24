Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Windows.Forms
Imports Intergraph.GTechnology.API
Imports Intergraph.GTechnology.Interfaces
Imports Intergraph.GTechnology.GTPlot
Imports Intergraph.GTechnology

Imports System.Drawing
Imports System.ComponentModel
Imports System.Globalization

Module GTPlotWindow
  Public Function DoesNamedPlotExist(ByVal strPlotName As String) As Boolean

    Dim oGTNamedPlotReturn As IGTNamedPlot

    Dim oGTNamedPlot As IGTNamedPlot
    Dim oGTNamedPlots As IGTNamedPlots
    Dim blnNamedPlotExists As Boolean
    Dim oGTApplication As IGTApplication

    Try
      oGTApplication = GTClassFactory.Create(Of IGTApplication)()
      oGTNamedPlots = oGTApplication.NamedPlots

      blnNamedPlotExists = False
      For Each oGTNamedPlot In oGTNamedPlots
        If oGTNamedPlot.Name = strPlotName Then
          oGTNamedPlotReturn = oGTNamedPlot
          blnNamedPlotExists = True
          Exit For
        End If
      Next oGTNamedPlot

      DoesNamedPlotExist = blnNamedPlotExists

    Catch ex As Exception
      MsgBox("WorkspacePlotsForm.DoesNamedPlotExist:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    Finally
    End Try

  End Function

  Public Function IsNamedPlotOpen(ByVal strPlotName As String, ByRef oGTOpenPlotWindow As IGTPlotWindow) As Boolean

    Dim oGTPlotWindow As IGTPlotWindow
    Dim oGTPlotWindows As IGTPlotWindows
    Dim blnNamedPlotOpen As Boolean
    Dim oGTApplication As IGTApplication

    Try
      oGTApplication = GTClassFactory.Create(Of IGTApplication)()
      oGTPlotWindows = GTClassFactory.Create(Of IGTPlotWindows)()

      oGTPlotWindows = oGTApplication.GetPlotWindows

      blnNamedPlotOpen = False
      For Each oGTPlotWindow In oGTPlotWindows
        If oGTPlotWindow.NamedPlot.Name = strPlotName Then
          oGTOpenPlotWindow = oGTPlotWindow
          blnNamedPlotOpen = True
          Exit For
        End If
      Next oGTPlotWindow

      IsNamedPlotOpen = blnNamedPlotOpen

    Catch ex As Exception
      MsgBox("WorkspacePlotsForm.IsNamedPlotOpen:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    Finally
      'oGTPlotWindow = Nothing
      'oGTPlotWindows = Nothing
    End Try

  End Function


  Public Function DoesNamedPlotExist(ByVal strPlotName As String, ByVal oGTNamedPlotReturn As IGTNamedPlot) As Boolean

    Dim oGTNamedPlot As IGTNamedPlot
    Dim oGTNamedPlots As IGTNamedPlots
    Dim blnNamedPlotExists As Boolean
    Dim oGTApplication As IGTApplication

    Try
      oGTApplication = GTClassFactory.Create(Of IGTApplication)()
      oGTNamedPlots = oGTApplication.NamedPlots

      blnNamedPlotExists = False
      For Each oGTNamedPlot In oGTNamedPlots
        If oGTNamedPlot.Name = strPlotName Then
          oGTNamedPlotReturn = oGTNamedPlot
          blnNamedPlotExists = True
          Exit For
        End If
      Next oGTNamedPlot

      DoesNamedPlotExist = blnNamedPlotExists

    Catch ex As Exception
      MsgBox("WorkspacePlotsForm.DoesNamedPlotExist:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    Finally
      'oGTNamedPlot = Nothing
      'oGTNamedPlots = Nothing
    End Try

  End Function

  Public Sub PrintNamedPlot(ByVal oGTNamedPlot As IGTNamedPlot, ByRef oPrintProperties As IGTPrintProperties, ByRef PrintDialog1 As PrintDialog)

    Dim oGTPlotWindow As IGTPlotWindow = Nothing
    Dim blnPreviouslyClosed As Boolean
    Dim oGTApplication As IGTApplication

    Try

      ' Open the named plot or make it active
      blnPreviouslyClosed = False
      If IsNamedPlotOpen(oGTNamedPlot.Name, oGTPlotWindow) Then
        'oGTPlotWindow.Activate
      Else
        blnPreviouslyClosed = True
        oGTApplication = GTClassFactory.Create(Of IGTApplication)()
        oGTPlotWindow = oGTApplication.NewPlotWindow(oGTNamedPlot)
      End If

      ' Print to the selected printer
      With oPrintProperties
        .DocumentName = oGTPlotWindow.Caption

        ' Fix Plot Orientation - Cant locate the GTPrintOrientationConstants - Hard coded the value for now
        If oGTPlotWindow.NamedPlot.PaperHeight > oGTPlotWindow.NamedPlot.PaperWidth Then
          .Orientation = 1 'gtPortrait  GTPrintOrientationConstants.gtpoPortrait
        Else
          .Orientation = 2 'gtpoLandscape
        End If

        Select Case oGTPlotWindow.NamedPlot.PaperHeight & IIf(PrintDialog1.PrinterSettings.IsPlotter, " use custom size ", " x ") & oGTPlotWindow.NamedPlot.PaperWidth
          Case "27940 x 21590" '  8.5 x 11  A-Size  Portrait
            .PaperSize = 1
          Case "21590 x 27940" '  8.5 x 11  A-Size  Landscape
            .PaperSize = 1
          Case "43180 x 27940" '  11 x 17 B-Size  Portrait
            .PaperSize = 3
          Case "27940 x 43180" '  11 x 17 B-Size  Landscape
            .PaperSize = 3
          Case "55880 x 43180" '  17 x 22 C-Size  Portrait
            .PaperSize = 24
          Case "43180 x 55880" '  17 x 22 C-Size  Landscape
            .PaperSize = 24
          Case "86360 x 55880" '  22 x 34 D-Size  Portrait
            .PaperSize = 25
          Case "55880 x 86360" '  22 x 34 D-Size  Landscape
            .PaperSize = 25
          Case "111760 x 86360" ' 34 x 44 E-Size  Portrait
            .PaperSize = 26
          Case "86360 x 111760" ' 34 x 44 E-Size  Landscape
            .PaperSize = 26
          Case Else 'Custom Size
            If oGTPlotWindow.NamedPlot.PaperHeight > oGTPlotWindow.NamedPlot.PaperWidth Then
              .Orientation = 1 'gtpoPortrait
              .PaperHeight = oGTPlotWindow.NamedPlot.PaperHeight
              .PaperWidth = oGTPlotWindow.NamedPlot.PaperWidth
            Else
              .Orientation = 2 'gtpoLandscape
              .PaperHeight = oGTPlotWindow.NamedPlot.PaperWidth
              .PaperWidth = oGTPlotWindow.NamedPlot.PaperHeight
            End If
        End Select
      End With

      oGTPlotWindow.PrintPlot(PrintDialog1.PrinterSettings.PrinterName, oPrintProperties)

      ' If PlotWindow was previously closed then close it
      If blnPreviouslyClosed Then oGTPlotWindow.Close()

    Catch ex As Exception
      MsgBox("WorkspacePlotsForm.PrintNamedPlot:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    Finally
      'oGTPlotWindow = Nothing
    End Try
  End Sub

End Module
