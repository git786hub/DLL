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

Module NewPlotWindowCreation

  '	Why am I increasing the metadata values by 100?
  ' I think that this was added To improve accuracy.
  ' If I remember correctly redlines can only handle a minimum number Of Decimal places possibly no decimals so before drawing
  ' the redlines they are multiplied by 100 to shift the Decimal allowing For support Of the Decimal places used in the redline metadata.
  ' This causes 
  Public Const dSCALE As Double = 100

  Public Sub InsertObjects(ByRef rsAttributeQuery As ADODB.Recordset, ByRef rsObjects As ADODB.Recordset, ByRef rsGroupsDRI As ADODB.Recordset, ByVal oPlotBoundary As PlotBoundary)

    Dim strDatatype As String = ""
    Dim sStatusBarTextTemp As String
    Dim oGTPlotFrame As IGTPlotFrame
    Dim sObjectFile As String
    Dim lFileLen As Long
    Dim sStatusBarText As String
    Dim oGTApplication As IGTApplication

    oGTApplication = GTClassFactory.Create(Of IGTApplication)()
    sStatusBarText = oGTApplication.GetStatusBarText(GTStatusPanelConstants.gtaspcMessage)

    Try

      sStatusBarTextTemp = oGTApplication.GetStatusBarText(GTStatusPanelConstants.gtaspcMessage)

      While Not rsObjects.EOF

        strDatatype = rsObjects.Fields("OB_DATATYPE").Value

        oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, sStatusBarText & String.Format(My.Resources.NewPlotWindow.InsertObjects_SetStatusBarText_Inserting, strDatatype, rsObjects.Fields("OB_NAME").Value))

        Select Case strDatatype

          Case "Object"
            sObjectFile = rsObjects.Fields("OB_FILE").Value
            lFileLen = FileSystem.FileLen(sObjectFile) 'Check if file exists

            ' Todo - Check for file in all other paths identified in FileUNCPaths.xml file if not explicitly defined.

            oGTPlotFrame = InsertObject(rsGroupsDRI, rsObjects, sObjectFile)
            oGTPlotFrame.Name = rsObjects.Fields("OB_NAME").Value
            'oGTPlotFrame.Locked = True

        End Select

        rsObjects.MoveNext()

      End While

    Catch ex As Exception
      MsgBox("NewPlotWindowCreation.InsertObjects: " & Err.Description & " for Redline DataType:" & strDatatype)
      Err.Clear()

    Finally
      oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, sStatusBarText)
      Err.Clear()

    End Try

  End Sub

  Public Sub InsertMapFrames(ByRef rsAttributeQuery As ADODB.Recordset, ByRef rsMapFrames As ADODB.Recordset, ByRef rsGroupsDRI As ADODB.Recordset, ByVal oPlotBoundary As PlotBoundary)

    Dim strDatatype As String = ""
    Dim oGTPlotMap As IGTPlotMap
    Dim oGTNamedPlot As IGTNamedPlot

    Dim sStatusBarText As String
    Dim sStatusBarTextTemp As String

    Dim oGTApplication As IGTApplication
    oGTApplication = GTClassFactory.Create(Of IGTApplication)()
    sStatusBarText = oGTApplication.GetStatusBarText(GTStatusPanelConstants.gtaspcMessage)

    Try
      oGTNamedPlot = oGTApplication.NamedPlots(oPlotBoundary.Name)


      While Not rsMapFrames.EOF

        strDatatype = rsMapFrames.Fields("MF_DATATYPE").Value

        oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, sStatusBarText & String.Format(My.Resources.NewPlotWindow.InsertMapFrames_SetStatusBarText_Inserting, strDatatype))

        Select Case strDatatype

          '
          ' Regular Map Frame
          '
          Case "Map Frame", "Key Map"
            Dim oGFramme As New GFramme
            'oGFramme.ZoomToFitPage(oPlotBoundary.Name)

            oGTPlotMap = InsertMapFrame(rsGroupsDRI, rsMapFrames)
                        oGTPlotMap.Frame.Name = rsMapFrames.Fields("MF_DATATYPE").Value
                        oPlotBoundary.MapFrameName = oGTPlotMap.Frame.Name ' Todo -May be issues if duplicate names, should add check for duplicates
                        If Not IsNothing(rsAttributeQuery) Then
              oGTPlotMap.Frame.FieldsQuery = rsAttributeQuery.Source
            End If

            If Not String.IsNullOrEmpty(oPlotBoundary.StyleSubstitution) Then 'PA Added because 10.2SP1 does not like passing nulls.
              oGTNamedPlot.StyleMap = oPlotBoundary.StyleSubstitution
            End If
            'oGTPlotMap.Frame.BorderRedline.AutomaticTextField
            'oGTPlotMap.DisplayService.GetNodeChildren(


            If Not oPlotBoundary.UserDefined Then


              ' RotateMapView
              oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, sStatusBarText & String.Format(My.Resources.NewPlotWindow.InsertMapFrames_SetStatusBarText_Inserting_RotateMapView, strDatatype))
              RotateMapView(oPlotBoundary)


              ' Load legend
              oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, sStatusBarText & String.Format(My.Resources.NewPlotWindow.InsertMapFrames_SetStatusBarText_Inserting_LoadLegend, strDatatype))
              LoadLegend(oGTPlotMap, oPlotBoundary)


              ' Window to Plot Boundary
              oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, sStatusBarText & String.Format(My.Resources.NewPlotWindow.InsertMapFrames_SetStatusBarText_Inserting_WindowPlotBoundary, strDatatype))
              sStatusBarTextTemp = oGTApplication.GetStatusBarText(GTStatusPanelConstants.gtaspcMessage)
              If Not IsDBNull(rsMapFrames.Fields("MF_DISPLAY_FACTOR").Value) Then
                ZoomToPlotBoundary(oGTPlotMap, oPlotBoundary, False, rsMapFrames.Fields("MF_DISPLAY_FACTOR").Value, rsMapFrames.Fields("MF_STYLE_NO").Value)
              Else
                ZoomToPlotBoundary(oGTPlotMap, oPlotBoundary)
              End If

              ' Store reference to main Map Frame DisplayScale
              If strDatatype = "Map Frame" Then
                oPlotBoundary.MapScale = oGTPlotMap.DisplayScale
              End If

              oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, sStatusBarTextTemp)


              ' Attach Query to Key Map legend to highlight the Plot Boundary.
              If strDatatype = "Key Map" Then
                oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, sStatusBarText & String.Format(My.Resources.NewPlotWindow.InsertMapFrames_SetStatusBarText_Inserting_HighlightPlotBoundary, strDatatype))

                ' Set Symbology
                Try
                  Dim oGTSymbology As IGTSymbology
                  oGTSymbology = GTClassFactory.Create(Of IGTSymbology)()
                  oGTSymbology.Width = 15
                  oGTSymbology.Color = 255 'RED
                  oGTSymbology.FillColor = 255 'RED

                  oGTPlotMap.DisplayService.AppendQuery(My.Resources.NewPlotWindow.InsertMapFrames_KeyMapAppendQueryGroup, My.Resources.NewPlotWindow.InsertMapFrames_KeyMapAppendQueryName, rsAttributeQuery, oGTSymbology)

                Catch ex As Exception
                  Debug.Print(ex.Message)
                  Err.Clear()

                End Try

                oGTApplication.RefreshWindows()
              End If


              ' Auto Insert Formations in one of the quads of the map frame.
              If oPlotBoundary.PlaceFormations Then
                oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, sStatusBarText & String.Format(My.Resources.NewPlotWindow.InsertMapFrames_SetStatusBarText_Inserting_InsertingFormations, strDatatype))

                'InsertFormationFramesInQuad(rsAttributeQuery, rsMapFrames, rsGroupsDRI, oPlotBoundary)

                InsertSelectedFormationFramesInQuad(rsAttributeQuery, rsMapFrames, rsGroupsDRI, oPlotBoundary)


                ' Todo verify is required.  Also one above in keymap insertion. Code to issue only a single refresh if required.
                oGTApplication.RefreshWindows()
              End If


              'Restore Status Bar Text
              oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, sStatusBarTextTemp)


              oGTPlotMap.Frame.Activate()

              ' Legend Overrides
              Dim oLegendOverrides As New LegendOverrides
              oLegendOverrides.ApplyLegendOverrides(oPlotBoundary)

              oGTPlotMap.Frame.Deactivate()


              ' InsertNorthArrow
              ' Todo check if the oGFramme object reference is valid here after placing formation and keymap frames.  May have to move this section to just under the main mapframe insertion.
              If Not IsDBNull(rsMapFrames.Fields("MF_NORTHARROW_SYMBOLFILE").Value) Then
                If oPlotBoundary.DetailID = 0 Then ' Only if Geo based map
                  oGFramme.InsertNorthArrow(oPlotBoundary, rsMapFrames.Fields("MF_NORTHARROW_SIZE").Value, rsMapFrames.Fields("MF_NORTHARROW_SYMBOLFILE").Value)
                End If
              End If

            Else 'User Defined placement use active window legend if exists
              If oPlotBoundary.InsertActiveMapWindow Then
                With oGTPlotMap
                  If oPlotBoundary.Legend = "" Then
                    oPlotBoundary.Legend = oPlotBoundary.ActiveMapWindow_LegendName
                    'oPlotBoundary.Legend = GetLegendName(GetTypesDefaultLegend(oPlotBoundary.Type, oPlotBoundary.PaperSize, oPlotBoundary.PaperOrientation))
                  End If

                  .DisplayService.ReplaceLegend(oPlotBoundary.Legend, oPlotBoundary.ActiveMapWindow_DetailID)
                  '.DisplayService.ReplaceLegend(oPlotBoundary.ActiveMapWindow_LegendName, oPlotBoundary.ActiveMapWindow_DetailID)

                  sStatusBarTextTemp = oGTApplication.GetStatusBarText(GTStatusPanelConstants.gtaspcMessage)
                  .Frame.Activate()
                  .ZoomArea(oPlotBoundary.ActiveMapWindow_Range)
                  '.DisplayScale = Mid(oPlotBoundary.MapScale, 3) replaced to support Imperial
                  If GetMeasurmentUnits(oPlotBoundary.MapScale) = MeasurmentUnits.Metric Then
                    .DisplayScale = GetScaleValue(oPlotBoundary.MapScale)
                  Else
                    .DisplayScale = GetScaleValue(oPlotBoundary.MapScale) * 12
                  End If
                  .Frame.Deactivate()
                  oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, sStatusBarTextTemp)
                End With
              End If
            End If
            'oGFramme.ZoomToFitPage(oPlotBoundary.Name)
            oGFramme = Nothing
            'oGTApplication.RefreshWindows()


            '  ' These classes are no longer used.  It was the old way of placing formations.
            '  '
            '  ' Formation Area 
            '  '
            'Case "Formation Area"
            '    InsertFormationFrames(rsAttributeQuery, rsMapFrames, rsGroupsDRI, oPlotBoundary)
            '    oGTApplication.RefreshWindows()

            '    '
            '    ' Formation Map Frame
            '    '
            'Case "Formation Map Frame"
            '    oGTPlotMap = InsertMapFrame(rsGroupsDRI, rsMapFrames)
            '    LoadLegend(oGTPlotMap, oPlotBoundary)
            '    ZoomToPlotBoundary(oGTPlotMap, oPlotBoundary)
            '    oGTApplication.RefreshWindows()

        End Select

        rsMapFrames.MoveNext()

      End While

    Catch ex As Exception
      MsgBox("NewPlotWindowCreation.InsertMapFrame: " & Err.Description & " for Redline DataType:" & strDatatype)
      Err.Clear()

    Finally
      oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, sStatusBarText)
      Err.Clear()

    End Try

  End Sub



  Public Sub PopulateRedlineGroupInfo(ByRef rsAttributeQuery As ADODB.Recordset, ByRef rsRedlines As ADODB.Recordset, ByRef rsGroupsDRI As ADODB.Recordset, ByVal oPlotBoundary As PlotBoundary, Optional ByVal bGroup As Boolean = True)

    Dim strDatatype As String = "Undefined"
    Dim strText As String
    Dim strFieldName As String = ""

    Dim oGTPoint As IGTPoint
    Dim oPolygonGeometry As IGTPolygonGeometry
    Dim oPolylineGeometry As IGTPolylineGeometry
    Dim oOrientedPointGeometry As IGTOrientedPointGeometry
    Dim oTextPointGeometry As IGTTextPointGeometry
    Dim oGTVector As IGTVector

    Dim oGTPlotRedline As IGTPlotRedline
    Dim oGTNamedPlot As IGTNamedPlot

    Dim iGroupNumber As Integer = -1

    Dim sStatusBarText As String
    Dim oGTApplication As IGTApplication
    oGTApplication = GTClassFactory.Create(Of IGTApplication)()
    sStatusBarText = oGTApplication.GetStatusBarText(GTStatusPanelConstants.gtaspcMessage)



    'On Error GoTo ErrorHandler
    Try

      oGTNamedPlot = oGTApplication.NamedPlots(oPlotBoundary.Name)

      While Not rsRedlines.EOF

        strDatatype = rsRedlines.Fields("RL_DATATYPE").Value

        Select Case strDatatype


          '
          ' Area redline
          '
          Case GTRedlineGroupDataTypeConstants.gtrgdtRedlineAreas

            oPolygonGeometry = GTClassFactory.Create(Of IGTPolygonGeometry)()

                        oGTPoint = GTClassFactory.Create(Of IGTPoint)()

                        ' dch commented out original code for two points
                        'oGTPoint.X = rsRedlines.Fields("RL_COORDINATE_X1").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_X").Value * dSCALE
                        'oGTPoint.Y = rsRedlines.Fields("RL_COORDINATE_Y1").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_Y").Value * dSCALE
                        'oGTPoint.Z = 0
                        'oPolygonGeometry.Points.Add(oGTPoint)

                        'oGTPoint.X = rsRedlines.Fields("RL_COORDINATE_X2").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_X").Value * dSCALE
                        'oGTPoint.Y = rsRedlines.Fields("RL_COORDINATE_Y2").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_Y").Value * dSCALE
                        'oGTPoint.Z = 0
                        'oPolygonGeometry.Points.Add(oGTPoint)

                        ' dch added below to create a rectangle from two points
                        'first point
                        oGTPoint.X = rsRedlines.Fields("RL_COORDINATE_X1").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_X").Value * dSCALE
                        oGTPoint.Y = rsRedlines.Fields("RL_COORDINATE_Y1").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_Y").Value * dSCALE
                        oGTPoint.Z = 0
                        oPolygonGeometry.Points.Add(oGTPoint)
                        'second point
                        oGTPoint.X = rsRedlines.Fields("RL_COORDINATE_X2").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_X").Value * dSCALE
                        oGTPoint.Y = rsRedlines.Fields("RL_COORDINATE_Y1").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_Y").Value * dSCALE
                        oGTPoint.Z = 0
                        oPolygonGeometry.Points.Add(oGTPoint)
                        'third point
                        oGTPoint.X = rsRedlines.Fields("RL_COORDINATE_X2").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_X").Value * dSCALE
                        oGTPoint.Y = rsRedlines.Fields("RL_COORDINATE_Y2").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_Y").Value * dSCALE
                        oGTPoint.Z = 0
                        oPolygonGeometry.Points.Add(oGTPoint)
                        'fourth point
                        oGTPoint.X = rsRedlines.Fields("RL_COORDINATE_X1").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_X").Value * dSCALE
                        oGTPoint.Y = rsRedlines.Fields("RL_COORDINATE_Y2").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_Y").Value * dSCALE
                        oGTPoint.Z = 0
                        oPolygonGeometry.Points.Add(oGTPoint)
                        ' should be closed with four points?
                        ' or do we still need a repeat of the first point to close?
                        ' dch - finished with rectangle by two points

                        If bGroup Then
              oGTPlotRedline = oGTNamedPlot.NewRedline(oPolygonGeometry, rsRedlines.Fields("RL_STYLE_NUMBER").Value, iGroupNumber)
              iGroupNumber = oGTPlotRedline.GroupNumber
            Else
              oGTPlotRedline = oGTNamedPlot.NewRedline(oPolygonGeometry, rsRedlines.Fields("RL_STYLE_NUMBER").Value)
            End If


            '
            ' Point redline
            '
          Case GTRedlineGroupDataTypeConstants.gtrgdtRedlinePoints

            oGTPoint = GTClassFactory.Create(Of IGTPoint)()
            oGTPoint.X = rsRedlines.Fields("RL_COORDINATE_X1").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_X").Value * dSCALE
            oGTPoint.Y = rsRedlines.Fields("RL_COORDINATE_Y1").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_Y").Value * dSCALE
            oGTPoint.Z = 0

            oOrientedPointGeometry = GTClassFactory.Create(Of IGTOrientedPointGeometry)()
            oOrientedPointGeometry.Origin = oGTPoint

            Dim dAngle As Double = rsRedlines.Fields("RL_ROTATION").Value
            oGTVector = GTClassFactory.Create(Of IGTVector)()
            oGTVector.I = Math.Cos(dAngle)
            oGTVector.J = Math.Sin(dAngle)
            oGTVector.K = 0
            oOrientedPointGeometry.Orientation = oGTVector

            ' Test if point symbol is beyond the edge of page.
            'Dim s As String = String.Format("Point.X={0}, Point.Y,{1}, PaperWidth={2}, PaperHeight={3}, Style = {4}", oGTPoint.X, oGTPoint.Y, oGTNamedPlot.PaperWidth, oGTNamedPlot.PaperHeight, rsRedlines.Fields("RL_STYLE_NUMBER").Value)
            'MessageBox.Show(s, "Point redline")

            If oGTPoint.X > oGTNamedPlot.PaperWidth Then
              MessageBox.Show("Skipping redline Point placement.  X Insertion is of point is off page Width", "Point redline")
              Exit Select
            End If
            If oGTPoint.Y > oGTNamedPlot.PaperHeight Then
              MessageBox.Show("Skipping redline Point placement.  Y Insertion is of point is off page Height", "Point redline")
              Exit Select
            End If
            'Dim dr As DialogResult = MessageBox.Show("Place redline Point?", "Point redline", MessageBoxButtons.YesNo)
            'If dr = DialogResult.No Then Exit Select


            If bGroup Then
              oGTPlotRedline = oGTNamedPlot.NewRedline(oOrientedPointGeometry, rsRedlines.Fields("RL_STYLE_NUMBER").Value, iGroupNumber)
              iGroupNumber = oGTPlotRedline.GroupNumber
            Else
              oGTPlotRedline = oGTNamedPlot.NewRedline(oOrientedPointGeometry, rsRedlines.Fields("RL_STYLE_NUMBER").Value)
            End If


            '
            ' Line redlines
            '
          Case GTRedlineGroupDataTypeConstants.gtrgdtRedlineLines

            oPolylineGeometry = GTClassFactory.Create(Of IGTPolylineGeometry)()

            oGTPoint = GTClassFactory.Create(Of IGTPoint)()
            oGTPoint.X = rsRedlines.Fields("RL_COORDINATE_X1").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_X").Value * dSCALE
            oGTPoint.Y = rsRedlines.Fields("RL_COORDINATE_Y1").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_Y").Value * dSCALE
            oGTPoint.Z = 0
            oPolylineGeometry.Points.Add(oGTPoint)

            oGTPoint = GTClassFactory.Create(Of IGTPoint)()
            oGTPoint.X = rsRedlines.Fields("RL_COORDINATE_X2").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_X").Value * dSCALE
            oGTPoint.Y = rsRedlines.Fields("RL_COORDINATE_Y2").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_Y").Value * dSCALE
            oGTPoint.Z = 0
            oPolylineGeometry.Points.Add(oGTPoint)

            If bGroup Then
              oGTPlotRedline = oGTNamedPlot.NewRedline(oPolylineGeometry, rsRedlines.Fields("RL_STYLE_NUMBER").Value, iGroupNumber)
              iGroupNumber = oGTPlotRedline.GroupNumber
            Else
              oGTPlotRedline = oGTNamedPlot.NewRedline(oPolylineGeometry, rsRedlines.Fields("RL_STYLE_NUMBER").Value)
            End If


            '
            ' Text redline
            '
          Case GTRedlineGroupDataTypeConstants.gtrgdtRedlineText

            strText = rsRedlines.Fields("RL_TEXT" & LangSuffix()).Value

            If Not (strText = vbNullString) Then

              ' Replace database fields with values from Query.
              strFieldName = GetField(strText, rsAttributeQuery)
              strText = ReplaceFields(strText, rsAttributeQuery)

              Dim dAngle As Double = rsRedlines.Fields("RL_ROTATION").Value
              oGTVector = GTClassFactory.Create(Of IGTVector)()
              oGTVector.I = Math.Cos(dAngle)
              oGTVector.J = Math.Sin(dAngle)
              oGTVector.K = 0

              oGTPoint = GTClassFactory.Create(Of IGTPoint)()
              oGTPoint.X = rsRedlines.Fields("RL_COORDINATE_X1").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_X").Value * dSCALE
              oGTPoint.Y = rsRedlines.Fields("RL_COORDINATE_Y1").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_Y").Value * dSCALE
              oGTPoint.Z = 0

              oTextPointGeometry = GTClassFactory.Create(Of IGTTextPointGeometry)()

              With oTextPointGeometry
                .Origin = oGTPoint
                .Normal = oGTVector
                .Rotation = rsRedlines.Fields("RL_ROTATION").Value
                '.Alignment = GTAlignmentConstants.gtalCenterLeft '  rsRedlines.Fields("RL_TEXT_ALIGNMENT").Value 'PA was hard coded for some reason.
                '  Public Enum GTAlignmentConstants As Short
                '    gtalCenterCenter = 0 	' Center Center
                '    gtalCenterLeft = 1 		' Center Left
                '    gtalCenterRight = 2 	' Center Right
                '    gtalTopCenter = 4 		' Top Center
                '    gtalTopLeft = 5 		' Top Left
                '    gtalTopRight = 6 		' Top Right
                '    gtalBottomCenter = 8 	' Bottom Center
                '    gtalBottomLeft = 9 		' Bottom Left
                '    gtalBottomRight = 10 	' Bottom Right
                '  End Enum
                '.Alignment = GTAlignmentConstants.gtalTopLeft    '  rsRedlines.Fields("RL_TEXT_ALIGNMENT").Value
                If Not IsDBNull(rsRedlines.Fields("RL_TEXT_ALIGNMENT").Value) Then
                  .Alignment = rsRedlines.Fields("RL_TEXT_ALIGNMENT").Value
                End If
                .Text = strText
              End With
              Dim iStyleID As Integer = rsRedlines.Fields("RL_STYLE_NUMBER").Value

              'On Error Resume Next
              Try
                If bGroup Then
                  oGTPlotRedline = oGTNamedPlot.NewRedline(oTextPointGeometry, iStyleID, iGroupNumber)
                  iGroupNumber = oGTPlotRedline.GroupNumber
                Else
                  oGTPlotRedline = oGTNamedPlot.NewRedline(oTextPointGeometry, iStyleID)
                End If

                ' Turn readline text that are attributes into AutomaticTextFields
                If Not strFieldName = "" Then
                  oGTPlotRedline.AutomaticTextField = strFieldName
                  oGTPlotRedline.AutomaticTextSource = GTPlotAutomaticTextSourceConstants.gtpatPlotByQuery
                End If
              Catch ex As Exception
                Debug.Write("NewPlotWindowCreation.PopulateRedlineGroupInfo:RedlineText" & vbCrLf & ex.Message)
                'MsgBox("NewPlotWindowCreation.PopulateRedlineGroupInfo:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
              Finally
              End Try


            End If

            '
            ' Multi Text redline 
            '
          Case "Redline Multi Text"

                        'Dim rsMultiText As ADODB.Recordset

                        Dim num_out As Integer = 0
            Dim Detail_Num As Integer = 0
            Dim Job_ID As String = ""
            Dim Plot_Name As String = ""
            Dim sqlTempTable As String

            Dim count As Integer = 0
            Dim maxY As Integer = 0
            Dim position As Integer = 0
            Dim parts() As String
            Dim NumberOfLines As Integer

            Dim objCommand As ADODB.Command     ' The command used to get the message with.
            Dim objParameter As ADODB.Parameter ' The parameters required to get the message with.

            'sqlString = ""
            'oPlotBoundary.
            'QueryAccessibilityHelpEventArgs()
            'strSQL = rsRedlines.Fields("RL_TEXT" & LangSuffix()).Value

            'oPlotBoundary.FID
            Job_ID = oGTApplication.DataContext.ActiveJob
            Plot_Name = oGTNamedPlot.Name
            Detail_Num = oPlotBoundary.DetailID

            strText = rsRedlines.Fields("RL_TEXT" & LangSuffix()).Value

            If Not (strText = vbNullString) Then

              objCommand = New ADODB.Command
              'objCommand.CommandText = "BEGIN GET_ARB_NOTES_PKG.POP_ARB_NOTES_TEMP_TABLE(?,?,?); END;"
              objCommand.CommandText = strText

              objCommand.CommandType = ADODB.CommandTypeEnum.adCmdText
              objParameter = objCommand.CreateParameter("Job_ID", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, Job_ID.Length, Job_ID)
              objCommand.Parameters.Append(objParameter)
              objParameter = objCommand.CreateParameter("Plot_Name", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, Plot_Name.Length, Plot_Name)
              objCommand.Parameters.Append(objParameter)
              objParameter = objCommand.CreateParameter("Detail_Num", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamInput, 0, Detail_Num)
              objCommand.Parameters.Append(objParameter)
              objCommand.Name = "POP_ARB_NOTES_TEMP_TABLE"

              oGTApplication.Application.DataContext.ExecuteCommand(objCommand, num_out, ADODB.ExecuteOptionEnum.adExecuteNoRecords + ADODB.CommandTypeEnum.adCmdText)


              sqlTempTable = "select NOTES from GT_PLOT_TMP_MULTI_TEXT"
              Dim rsTempTable As ADODB.Recordset

              rsTempTable = oGTApplication.Application.DataContext.OpenRecordset(sqlTempTable, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText)

              If Not rsTempTable.BOF Then
                'rsTempTable.Sort = "NOTES"
              End If

              While Not rsTempTable.EOF

                count = count + 1

                strText = rsTempTable.Fields("NOTES").Value.ToString()

                parts = strText.Split(Environment.NewLine)
                NumberOfLines = parts.Length + 1

                ' Replace database fields with values from Query.
                'strFieldName = GetField(strText, rsAttributeQuery)
                'strText = ReplaceFields(strText, rsAttributeQuery)

                Dim dAngle As Double = rsRedlines.Fields("RL_ROTATION").Value
                oGTVector = GTClassFactory.Create(Of IGTVector)()
                oGTVector.I = Math.Cos(dAngle)
                oGTVector.J = Math.Sin(dAngle)
                oGTVector.K = 0

                oGTPoint = GTClassFactory.Create(Of IGTPoint)()
                oGTPoint.X = rsRedlines.Fields("RL_COORDINATE_X1").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_X").Value * dSCALE + position
                If count = 1 Then
                  oGTPoint.Y = rsRedlines.Fields("RL_COORDINATE_Y1").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_Y").Value * dSCALE
                Else
                  oGTPoint.Y = maxY
                End If
                oGTPoint.Z = 0

                maxY = oGTPoint.Y + NumberOfLines * 380

                If oGTNamedPlot.PaperHeight - 4000 < maxY Then
                  position = position + 1000
                  maxY = rsRedlines.Fields("RL_COORDINATE_Y1").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_Y").Value * dSCALE + position
                End If

                oTextPointGeometry = GTClassFactory.Create(Of IGTTextPointGeometry)()

                With oTextPointGeometry
                  .Origin = oGTPoint
                  .Normal = oGTVector
                  .Rotation = rsRedlines.Fields("RL_ROTATION").Value
                  '  Public Enum GTAlignmentConstants As Short
                  '    gtalCenterCenter = 0 	' Center Center
                  '    gtalCenterLeft = 1 		' Center Left
                  '    gtalCenterRight = 2 	' Center Right
                  '    gtalTopCenter = 4 		' Top Center
                  '    gtalTopLeft = 5 		' Top Left
                  '    gtalTopRight = 6 		' Top Right
                  '    gtalBottomCenter = 8 	' Bottom Center
                  '    gtalBottomLeft = 9 		' Bottom Left
                  '    gtalBottomRight = 10 	' Bottom Right
                  '  End Enum
                  '.Alignment = GTAlignmentConstants.gtalTopLeft    '  rsRedlines.Fields("RL_TEXT_ALIGNMENT").Value
                  If Not IsDBNull(rsRedlines.Fields("RL_TEXT_ALIGNMENT").Value) Then
                    .Alignment = rsRedlines.Fields("RL_TEXT_ALIGNMENT").Value
                  End If
                  .Text = strText
                End With
                Dim iStyleID As Integer = rsRedlines.Fields("RL_STYLE_NUMBER").Value

                'On Error Resume Next
                Try
                  If bGroup Then
                    oGTPlotRedline = oGTNamedPlot.NewRedline(oTextPointGeometry, iStyleID, iGroupNumber)
                    iGroupNumber = oGTPlotRedline.GroupNumber
                  Else
                    oGTPlotRedline = oGTNamedPlot.NewRedline(oTextPointGeometry, iStyleID)
                  End If

                  ' Turn readline text that are attributes into AutomaticTextFields
                  If Not strFieldName = "" Then
                    oGTPlotRedline.AutomaticTextField = strFieldName
                    oGTPlotRedline.AutomaticTextSource = GTPlotAutomaticTextSourceConstants.gtpatPlotByQuery
                  End If
                Catch ex As Exception
                  Debug.Write("NewPlotWindowCreation.PopulateRedlineGroupInfo:RedlineText" & vbCrLf & ex.Message)
                  'MsgBox("NewPlotWindowCreation.PopulateRedlineGroupInfo:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
                Finally
                End Try

                rsTempTable.MoveNext()

              End While

            End If


        End Select

        rsRedlines.MoveNext()

      End While


    Catch ex As Exception
      MsgBox("NewPlotWindowCreation.PopulateRedlineGroupInfo:" & vbCrLf & _
             ex.Message & vbCrLf & " for Redline DataType:" & strDatatype, vbOKOnly + vbExclamation, ex.Source)
    Finally
    End Try

  End Sub

  Public Sub StartDrawingRedlines(ByVal oPlotBoundary As PlotBoundary)

    Dim strSql As String
    Dim rsAttributeQuery As ADODB.Recordset = Nothing

    Dim sStatusBarText As String
    Dim oGTApplication As IGTApplication

    oGTApplication = GTClassFactory.Create(Of IGTApplication)()
    sStatusBarText = oGTApplication.GetStatusBarText(GTStatusPanelConstants.gtaspcMessage)

    Try

      '
      ' Retrieve plot boundary info used for updating automated plot field
      '
      ' ADHOC Template vs generated from Plot Boundary
      '
      'Was If Not IsNothing(oPlotBoundary.Attributes) Then ' Must of added this to support print active area but causes issue with other call to this function.  So added a Source property to aid logic.
      If Not oPlotBoundary.Source = "PrintActiveMapWindow" Then 'This is used when StartDrawingRedlines is called from the PrintActiveMapWindow command.
        If oPlotBoundary.FNO = 0 Then
          Dim oAttribute As Attribute
          strSql = "SELECT "
          For Each oAttribute In oPlotBoundary.Attributes
            strSql = strSql & IIf(Trim(oAttribute.VALUE) = "", "NULL", "'" & oAttribute.VALUE & "'") & " " & oAttribute.G3E_FIELD & ", "
          Next oAttribute
          strSql = Left(strSql, Len(strSql) - 2) 'Remove trailing comma and space
          strSql = strSql & ", TO_CHAR (SYSDATE, 'YYYY/MM/DD') ""SYSDATE"" FROM DUAL"
        Else
          ' Built query from user defined view to retrieve required columns.  This way it's managed by the user.
          strSql = "SELECT *"
          strSql = strSql & " FROM " & GTPlotMetadata.Parameters.PlotBoundaryInfoView
          strSql = strSql & " WHERE " & GTPlotMetadata.Parameters.PlotBoundaryLinkedJobAttribute_FieldName & " = '" & oPlotBoundary.Job & "' "
          strSql = strSql & " AND " & GTPlotMetadata.Parameters.PlotBoundaryAttribute_Name & " = '" & oPlotBoundary.Name & "'"
        End If
        rsAttributeQuery = oGTApplication.DataContext.OpenRecordset(strSql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, ADODB.CommandTypeEnum.adCmdText)
        Call Repos(rsAttributeQuery)

        ' Store sql statement in FieldsQuery field for later use through automated plot field requery
        oGTApplication.NamedPlots(oPlotBoundary.Name).FieldsQuery = strSql 'rsAttributeQuery.Source
      End If


      '
      ' Generate redlines
      '
      Call ProcessDrawingInfoGroups(rsAttributeQuery, oPlotBoundary)



    Catch ex As Exception
      MsgBox("NewPlotWindowCreation.StartDrawingRedlines:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    Finally
    End Try

  End Sub



  Public Sub ProcessDrawingInfoGroups(ByRef rsAttributeQuery As ADODB.Recordset, ByVal oPlotBoundary As PlotBoundary)

    Dim oDataGrid As Windows.Forms.DataGrid

    Dim rsGroupsDRI As ADODB.Recordset = Nothing
    Dim rsRedlines As ADODB.Recordset = Nothing
    Dim rsMapFrames As ADODB.Recordset = Nothing
    Dim rsObjects As ADODB.Recordset = Nothing
    Dim sSql As String

    Dim sStatusBarText As String
    Dim oGTApplication As IGTApplication

    oGTApplication = GTClassFactory.Create(Of IGTApplication)()
    sStatusBarText = oGTApplication.GetStatusBarText(GTStatusPanelConstants.gtaspcMessage)

    On Error GoTo ErrorHandler


    '
    ' Add the border
    '
    If oPlotBoundary.SheetInset > 0 Then
      DrawInsetBorder(oPlotBoundary)
    End If




    '
    ' Look for groups of Maps, Redline & Objects to insert for the given drawing type.
    '
    sSql = "SELECT * FROM " & GTPlotMetadata.Parameters.GT_PLOT_GROUPS_DRI & " WHERE DRI_ID = " & oPlotBoundary.DRI_ID & " ORDER BY GROUP_NO"
    rsGroupsDRI = oGTApplication.DataContext.OpenRecordset(sSql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, ADODB.CommandTypeEnum.adCmdText)
    Call Repos(rsGroupsDRI)
    While Not rsGroupsDRI.EOF



      ' Insert Map Frame
      sSql = "SELECT * FROM " & GTPlotMetadata.Parameters.GT_PLOT_MAPFRAME & " WHERE GROUP_NO = " & rsGroupsDRI.Fields("GROUP_NO").Value
      'If oPlotBoundary.PlaceFormations Then
      '  sSql = sSql & " AND MF_DATATYPE != 'Map Frame'"
      'Else
      '  sSql = sSql & " AND MF_DATATYPE NOT LIKE('Formation %')"
      'End If
      sSql = sSql & " ORDER BY MF_NO"

      rsMapFrames = oGTApplication.DataContext.OpenRecordset(sSql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, ADODB.CommandTypeEnum.adCmdText)
      Call Repos(rsMapFrames)
      If rsMapFrames.RecordCount > 0 Then
        Call InsertMapFrames(rsAttributeQuery, rsMapFrames, rsGroupsDRI, oPlotBoundary)
      End If
      rsMapFrames = Nothing

      'dch test code
      'to-do :  setup checkbox on form, create global variable for forced-white-background
      If (oPlotBoundary.ForceMapBackgroundToWhite = "YES") Then 'PA -Added default parameter.
        Dim oGTNamedPlot As IGTNamedPlot
        oGTNamedPlot = oGTApplication.NamedPlots(oGTApplication.ActivePlotWindow.Caption)
        For iFrame As Integer = 1 To oGTNamedPlot.Frames.Count
          If (oGTNamedPlot.Frames.Item(iFrame - 1).Name = "Map Frame") Or (oGTNamedPlot.Frames.Item(iFrame - 1).Name = "Key Map") Then
            oGTNamedPlot.Frames.Item(iFrame - 1).Activate()
            If (oGTNamedPlot.Frames.Item(iFrame - 1).Type = GTPlotFrameTypeConstants.gtpftMap) Then
              If (oGTNamedPlot.Frames.Item(iFrame - 1).PlotMap.Frame.Type = GTPlotFrameTypeConstants.gtpftMap) Then
                oGTNamedPlot.Frames.Item(iFrame - 1).PlotMap.BackColor = Color.White
              End If
            End If
            oGTNamedPlot.Frames.Item(iFrame - 1).Deactivate()
          End If
        Next
      End If
      'end dch test code

      ' Draw Redlines
      sSql = "SELECT * FROM " & GTPlotMetadata.Parameters.GT_PLOT_REDLINES & " WHERE GROUP_NO = " & rsGroupsDRI.Fields("GROUP_NO").Value & " ORDER BY RL_NO"
      rsRedlines = oGTApplication.DataContext.OpenRecordset(sSql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, ADODB.CommandTypeEnum.adCmdText)
      Call Repos(rsRedlines)
            If rsRedlines.RecordCount > 0 Then
                Call PopulateRedlineGroupInfo(rsAttributeQuery, rsRedlines, rsGroupsDRI, oPlotBoundary, IIf(rsGroupsDRI.Fields("GROUP_REDLINES").Value = 1, True, False))
            End If
            rsRedlines = Nothing



      ' Insert Object
      sSql = "SELECT * FROM " & GTPlotMetadata.Parameters.GT_PLOT_OBJECTS & " WHERE GROUP_NO = " & rsGroupsDRI.Fields("GROUP_NO").Value & " ORDER BY OB_NO"
      rsObjects = oGTApplication.DataContext.OpenRecordset(sSql, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, ADODB.CommandTypeEnum.adCmdText)
      Call Repos(rsObjects)
      If rsObjects.RecordCount > 0 Then
        Call InsertObjects(rsAttributeQuery, rsObjects, rsGroupsDRI, oPlotBoundary)
      End If
      rsObjects = Nothing



      rsGroupsDRI.MoveNext()
    End While

        GoTo Finish


ErrorHandler:
    MsgBox("NewPlotWindowCreation.ProcessDrawingInfoGroups - " & Err.Description)
    Err.Clear()

Finish:
    If Not (rsGroupsDRI Is Nothing) Then
      rsGroupsDRI = Nothing
    End If
    If Not (rsRedlines Is Nothing) Then
      rsRedlines = Nothing
    End If
    If Not (rsObjects Is Nothing) Then
      rsObjects = Nothing
    End If

  End Sub



  Public Function GetFeatureRange(ByVal lngG3E_FNO As Long, ByVal lngG3E_FID As Long, ByVal lngG3E_CNO As Long) As IGTWorldRange

    Dim oGTKeyObject As IGTKeyObject
    Dim oGTComponent As IGTComponent
    Dim oGTComponents As IGTComponents
    Dim oGTWorldRange As IGTWorldRange
    Dim oGTApplication As IGTApplication

    On Error GoTo ErrorHandler

    oGTApplication = GTClassFactory.Create(Of IGTApplication)()

    oGTKeyObject = oGTApplication.DataContext.OpenFeature(lngG3E_FNO, lngG3E_FID)
    oGTComponents = oGTKeyObject.Components
    oGTComponent = oGTComponents.GetComponent(lngG3E_CNO)

    'oGTWorldRange = GTApplication.CreateService(GTServiceConstants.gtsvcWorldRange)
    oGTWorldRange = GTClassFactory.Create(Of IGTWorldRange)()


    oGTWorldRange = oGTComponent.Geometry.Range
    'GetRange(oGTComponent.Geometry, oGTWorldRange.BottomLeft, oGTWorldRange.TopRight)

    GetFeatureRange = oGTWorldRange

    Exit Function

ErrorHandler:
    MsgBox("NewPlotWindowCreation.GetFeatureRange: " & Err.Description)
    Err.Clear()

  End Function

  Public Sub InsertSelectedFormationFramesInQuadToScale(ByRef rsAttributeQuery As ADODB.Recordset, ByRef rsRedlines As ADODB.Recordset, ByRef rsGroupsDRI As ADODB.Recordset, ByVal oPlotBoundary As PlotBoundary)

    'Dim oGTPaperPointBL As IGTPoint
    'Dim oGTPaperPointTR As IGTPoint
    Dim oGTWorldPointBL As IGTPoint
    Dim oGTWorldPointTR As IGTPoint

    Dim oGTPaperRange As IGTPaperRange
    Dim oGTFormationRange As IGTPaperRange
    Dim oGTPlotMap As IGTPlotMap

    Dim oFormation As Formation
    Dim colFormations As New Collection

    Dim oGTWorldRange As IGTWorldRange
    Dim oGTApplication As IGTApplication

    Dim intFormation As Integer


    Try

      oGTPaperRange = GTClassFactory.Create(Of IGTPaperRange)()
      oGTFormationRange = GTClassFactory.Create(Of IGTPaperRange)()

      ' Get plot boundary map scale
      Dim sScale As String = oPlotBoundary.MapScale

      ' Get formation range
      colFormations = GetFormationsInDetail(oPlotBoundary.DetailID)
      For Each oFormation In colFormations




        ' Get the formation range
        oGTWorldPointBL = GTClassFactory.Create(Of IGTPoint)()
        oGTWorldPointTR = GTClassFactory.Create(Of IGTPoint)()
        oGTWorldPointBL = GTClassFactory.Create(Of IGTPoint)()
        oGTWorldPointTR = GTClassFactory.Create(Of IGTPoint)()

        oGTWorldRange = GetFormationRange(oFormation.FNO, oFormation.FID, oFormation.CNO)

        oGTWorldPointBL.X = oGTWorldRange.BottomLeft.X
        oGTWorldPointBL.Y = oGTWorldRange.BottomLeft.Y
        oGTWorldPointBL.Z = oGTWorldRange.BottomLeft.Z

        oGTWorldPointTR.X = oGTWorldRange.TopRight.X
        oGTWorldPointTR.Y = oGTWorldRange.TopRight.Y
        oGTWorldPointTR.Z = oGTWorldRange.TopRight.Z

        'oGTPaperPointBL = oGTPlotMap.WorldToPaper(oGTWorldPointBL)
        'oGTPaperPointTR = oGTPlotMap.WorldToPaper(oGTWorldPointTR)


        ' Inser the map
        oGTApplication = GTClassFactory.Create(Of IGTApplication)()
        oGTPlotMap = oGTApplication.ActivePlotWindow.InsertMap(oGTFormationRange)



        LoadLegend(oGTPlotMap, oPlotBoundary)


        ' Center the Formation inside the MapFrame
        ' Todo - Include ducts in range!!!
        oGTWorldRange = GetFormationRange(oFormation.FNO, oFormation.FID, oFormation.CNO)
        With oGTPlotMap
          .Frame.Activate()
          .DisplayScale = sScale
          .Frame.Deactivate()
        End With

        intFormation = intFormation + 1

      Next oFormation






    Catch ex As Exception
      MsgBox("NewPlotWindowCreation.InsertSelectedFormationFramesInQuadToScale:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    End Try

  End Sub

  Public Sub InsertSelectedFormationFramesInQuad(ByRef rsAttributeQuery As ADODB.Recordset, ByRef rsRedlines As ADODB.Recordset, ByRef rsGroupsDRI As ADODB.Recordset, ByVal oPlotBoundary As PlotBoundary)

    Dim oGTPaperRange As IGTPaperRange
    Dim oGTFormationRange As IGTPaperRange
    Dim oGTPlotMap As IGTPlotMap

    Dim dblHeight As Double
    Dim dblWidth As Double
    Dim dblMax As Double
    Dim dblMin As Double

    Dim oDetailFeatureGroup As DetailFeatureGroup
    Dim oDetail As Detail
    Dim oFormation As Formation

    Dim oGTWorldRange As IGTWorldRange
    Dim oGTApplication As IGTApplication

    Dim intFormation As Integer


    Try

      oGTPaperRange = GTClassFactory.Create(Of IGTPaperRange)()
      oGTFormationRange = GTClassFactory.Create(Of IGTPaperRange)()


      '
      ' Devide Formation Area into enough MapFrames to support all the formations
      '
      Dim oGTPointTL As IGTPoint
      oGTPointTL = GTClassFactory.Create(Of IGTPoint)()

      oGTPointTL.X = rsRedlines.Fields("MF_COORDINATE_X1").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_X").Value * dSCALE
      oGTPointTL.Y = rsRedlines.Fields("MF_COORDINATE_Y1").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_Y").Value * dSCALE
      oGTPointTL.Z = 0

      oGTPaperRange.TopLeft = oGTPointTL

      Dim oGTPointBR As IGTPoint
      oGTPointBR = GTClassFactory.Create(Of IGTPoint)()

      oGTPointBR.X = rsRedlines.Fields("MF_COORDINATE_X2").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_X").Value * dSCALE
      oGTPointBR.Y = rsRedlines.Fields("MF_COORDINATE_Y2").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_Y").Value * dSCALE
      oGTPointBR.Z = 0

      oGTPaperRange.BottomRight = oGTPointBR



      '
      ' Get min size and devide by half the formations then use that size squared to fit all formations in area
      '
      dblHeight = oGTPaperRange.BottomRight.Y - oGTPaperRange.TopLeft.Y
      dblWidth = oGTPaperRange.BottomRight.X - oGTPaperRange.TopLeft.X

      dblHeight = dblHeight / 4
      dblWidth = dblWidth / 4

      If dblHeight > dblWidth Then
        dblMax = dblHeight
        dblMin = dblWidth
      Else
        dblMax = dblWidth
        dblMin = dblHeight
      End If


      If oPlotBoundary.Formations.Count > 0 Then

        ' Get total number of formations to be placed based on user selection
        Dim iFormationCount As Integer = 0
        For Each oFormation In oPlotBoundary.Formations
          If oFormation.PlaceFormation Then
            iFormationCount = iFormationCount + 1
          End If
        Next oFormation

        ' Place formations
        For Each oFormation In oPlotBoundary.Formations
          If oFormation.PlaceFormation Then

            Dim oGTPointTL_Formation As IGTPoint
            oGTPointTL_Formation = GTClassFactory.Create(Of IGTPoint)()

            oGTPointTL_Formation.X = oGTPaperRange.TopLeft.X + ((dblWidth / iFormationCount) * intFormation)
            oGTPointTL_Formation.Y = oGTPaperRange.TopLeft.Y
            oGTPointTL_Formation.Z = 0

            oGTFormationRange.TopLeft = oGTPointTL_Formation

            Dim oGTPointBR_Formation As IGTPoint
            oGTPointBR_Formation = GTClassFactory.Create(Of IGTPoint)()

            oGTPointBR_Formation.X = oGTFormationRange.TopLeft.X + (dblWidth / iFormationCount)
            oGTPointBR_Formation.Y = oGTFormationRange.TopLeft.Y + (dblWidth / iFormationCount)
            oGTPointBR_Formation.Z = 0

            oGTFormationRange.BottomRight = oGTPointBR_Formation


            ' Inser the map
            oGTApplication = GTClassFactory.Create(Of IGTApplication)()
            oGTPlotMap = oGTApplication.ActivePlotWindow.InsertMap(oGTFormationRange)
            LoadLegend(oGTPlotMap, oPlotBoundary)

            ' Fit the Formation inside the MapFrame
            oGTWorldRange = GetFormationRange(oFormation.FNO, oFormation.FID, oFormation.CNO)
            With oGTPlotMap
              .Frame.Activate()
              .ZoomArea(oGTWorldRange)
              .DisplayScale = oPlotBoundary.MapScale
              .Frame.Deactivate()
            End With

            intFormation = intFormation + 1

          End If
        Next oFormation

      ElseIf oPlotBoundary.DetailFeatureGroups.Count > 0 Then

        ' Get total number of formations to be placed based on user selection
        Dim iFormationCount As Integer = 0
        For Each oDetailFeatureGroup In oPlotBoundary.DetailFeatureGroups
          For Each oDetail In oDetailFeatureGroup.Details
            For Each oFormation In oDetail.Formations
              If oFormation.PlaceFormation Then
                iFormationCount = iFormationCount + 1
              End If
            Next oFormation
          Next oDetail
        Next oDetailFeatureGroup

        ' Place formations
        For Each oDetailFeatureGroup In oPlotBoundary.DetailFeatureGroups
          For Each oDetail In oDetailFeatureGroup.Details
            For Each oFormation In oDetail.Formations
              If oFormation.PlaceFormation Then
                Dim oGTPointTL_Formation As IGTPoint
                oGTPointTL_Formation = GTClassFactory.Create(Of IGTPoint)()

                oGTPointTL_Formation.X = oGTPaperRange.TopLeft.X + ((dblWidth / iFormationCount) * intFormation)
                oGTPointTL_Formation.Y = oGTPaperRange.TopLeft.Y
                oGTPointTL_Formation.Z = 0

                oGTFormationRange.TopLeft = oGTPointTL_Formation

                Dim oGTPointBR_Formation As IGTPoint
                oGTPointBR_Formation = GTClassFactory.Create(Of IGTPoint)()

                oGTPointBR_Formation.X = oGTFormationRange.TopLeft.X + (dblWidth / iFormationCount)
                oGTPointBR_Formation.Y = oGTFormationRange.TopLeft.Y + (dblWidth / iFormationCount)
                oGTPointBR_Formation.Z = 0

                oGTFormationRange.BottomRight = oGTPointBR_Formation


                ' Inser the map
                oGTApplication = GTClassFactory.Create(Of IGTApplication)()
                oGTPlotMap = oGTApplication.ActivePlotWindow.InsertMap(oGTFormationRange)
                LoadLegend(oGTPlotMap, oDetail.DetailID, GetLegendName(oDetailFeatureGroup.Det_LNO))

                ' Fit the Formation inside the MapFrame
                oGTWorldRange = GetFormationRange(oFormation.FNO, oFormation.FID, oFormation.CNO)
                With oGTPlotMap
                  .Frame.Activate()
                  .ZoomArea(oGTWorldRange)
                  .DisplayScale = oPlotBoundary.MapScale
                  .Frame.Deactivate()
                End With

                intFormation = intFormation + 1
              End If
            Next oFormation
          Next oDetail
        Next oDetailFeatureGroup
      End If


    Catch ex As Exception
      MsgBox("NewPlotWindowCreation.InsertSelectedFormationFramesInQuad:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    End Try

  End Sub

  Public Sub InsertFormationFramesInQuad(ByRef rsAttributeQuery As ADODB.Recordset, ByRef rsRedlines As ADODB.Recordset, ByRef rsGroupsDRI As ADODB.Recordset, ByVal oPlotBoundary As PlotBoundary)

    Dim dblX As Double
    Dim dblY As Double
    Dim dblZ As Double

    Dim oGTPaperRange As IGTPaperRange
    Dim oGTFormationRange As IGTPaperRange
    Dim oGTPlotMap As IGTPlotMap

    Dim dblHeight As Double
    Dim dblWidth As Double
    Dim dblMax As Double
    Dim dblMin As Double
    Dim dblXPos As Double
    Dim dblYPos As Double

    Dim oFormation As Formation
    Dim colFormations As New Collection

    Dim oGTWorldRange As IGTWorldRange
    Dim oGTApplication As IGTApplication

    Dim intFormation As Integer


    On Error GoTo ErrorHandler


    oGTPaperRange = GTClassFactory.Create(Of IGTPaperRange)()
    oGTFormationRange = GTClassFactory.Create(Of IGTPaperRange)()


    '
    ' Devide Formation Area into enough MapFrames to support all the formations
    '
    Dim oGTPointTL As IGTPoint
    oGTPointTL = GTClassFactory.Create(Of IGTPoint)()

    oGTPointTL.X = rsRedlines.Fields("MF_COORDINATE_X1").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_X").Value * dSCALE
    oGTPointTL.Y = rsRedlines.Fields("MF_COORDINATE_Y1").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_Y").Value * dSCALE
    oGTPointTL.Z = 0

    oGTPaperRange.TopLeft = oGTPointTL

    Dim oGTPointBR As IGTPoint
    oGTPointBR = GTClassFactory.Create(Of IGTPoint)()

    oGTPointBR.X = rsRedlines.Fields("MF_COORDINATE_X2").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_X").Value * dSCALE
    oGTPointBR.Y = rsRedlines.Fields("MF_COORDINATE_Y2").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_Y").Value * dSCALE
    oGTPointBR.Z = 0

    oGTPaperRange.BottomRight = oGTPointBR



    '
    ' Get min size and devide by half the formations then use that size squared to fit all formations in area
    '
    dblHeight = oGTPaperRange.BottomRight.Y - oGTPaperRange.TopLeft.Y
    dblWidth = oGTPaperRange.BottomRight.X - oGTPaperRange.TopLeft.X

    dblHeight = dblHeight / 2
    dblWidth = dblWidth / 2

    If dblHeight > dblWidth Then
      dblMax = dblHeight
      dblMin = dblWidth
    Else
      dblMax = dblWidth
      dblMin = dblHeight
    End If

    colFormations = GetFormationsInDetail(oPlotBoundary.DetailID)
    For Each oFormation In colFormations





      Dim oGTPointTL_Formation As IGTPoint
      oGTPointTL_Formation = GTClassFactory.Create(Of IGTPoint)()

      oGTPointTL_Formation.X = oGTPaperRange.TopLeft.X + ((dblWidth / colFormations.Count) * intFormation)
      oGTPointTL_Formation.Y = oGTPaperRange.TopLeft.Y
      oGTPointTL_Formation.Z = 0

      oGTFormationRange.TopLeft = oGTPointTL_Formation

      Dim oGTPointBR_Formation As IGTPoint
      oGTPointBR_Formation = GTClassFactory.Create(Of IGTPoint)()

      oGTPointBR_Formation.X = oGTFormationRange.TopLeft.X + (dblWidth / colFormations.Count)
      oGTPointBR_Formation.Y = oGTFormationRange.TopLeft.Y + (dblWidth / colFormations.Count)
      oGTPointBR_Formation.Z = 0

      oGTFormationRange.BottomRight = oGTPointBR_Formation


      ' Inser the map
      oGTApplication = GTClassFactory.Create(Of IGTApplication)()
      oGTPlotMap = oGTApplication.ActivePlotWindow.InsertMap(oGTFormationRange)
      LoadLegend(oGTPlotMap, oPlotBoundary)

      ' Fit the Formation inside the MapFrame
      oGTWorldRange = GetFormationRange(oFormation.FNO, oFormation.FID, oFormation.CNO)
      With oGTPlotMap
        .Frame.Activate()
        .ZoomArea(oGTWorldRange)
        .Frame.Deactivate()
      End With

      intFormation = intFormation + 1

    Next oFormation


    Exit Sub

ErrorHandler:
    MsgBox("NewPlotWindowCreation.InsertFormationFramesInQuad: " & Err.Description)
    Err.Clear()

  End Sub

  Public Sub InsertFormationFrames(ByRef rsAttributeQuery As ADODB.Recordset, ByRef rsRedlines As ADODB.Recordset, ByRef rsGroupsDRI As ADODB.Recordset, ByVal oPlotBoundary As PlotBoundary)

    Dim dblX As Double
    Dim dblY As Double
    Dim dblZ As Double

    Dim oGTPaperRange As IGTPaperRange
    Dim oGTFormationRange As IGTPaperRange
    Dim oGTPlotMap As IGTPlotMap

    Dim dblHeight As Double
    Dim dblWidth As Double
    Dim dblMax As Double
    Dim dblMin As Double
    Dim dblXPos As Double
    Dim dblYPos As Double

    Dim oFormation As Formation
    Dim colFormations As New Collection

    Dim oGTWorldRange As IGTWorldRange
    Dim oGTApplication As IGTApplication

    Dim intFormation As Integer


    On Error GoTo ErrorHandler


    oGTPaperRange = GTClassFactory.Create(Of IGTPaperRange)()
    oGTFormationRange = GTClassFactory.Create(Of IGTPaperRange)()


    '
    ' Devide Formation Area into enough MapFrames to support all the formations
    '
    With oGTPaperRange.TopLeft
      .X = rsRedlines.Fields("RL_COORDINATE_X1").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_X").Value * dSCALE
      .Y = rsRedlines.Fields("RL_COORDINATE_Y1").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_Y").Value * dSCALE
      .Z = 0
    End With
    With oGTPaperRange.BottomRight
      .X = rsRedlines.Fields("RL_COORDINATE_X2").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_X").Value * dSCALE
      .Y = rsRedlines.Fields("RL_COORDINATE_Y2").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_Y").Value * dSCALE
      .Z = 0
    End With



    '
    ' Get min size and devide by half the formations then use that size squared to fit all formations in area
    '
    dblHeight = oGTPaperRange.BottomRight.Y - oGTPaperRange.TopLeft.Y
    dblWidth = oGTPaperRange.BottomRight.X - oGTPaperRange.TopLeft.X
    If dblHeight > dblWidth Then
      dblMax = dblHeight
      dblMin = dblWidth
    Else
      dblMax = dblWidth
      dblMin = dblHeight
    End If

    colFormations = GetFormationsInDetail(oPlotBoundary.DetailID)
    For Each oFormation In colFormations

      With oGTFormationRange.TopLeft
        .X = oGTPaperRange.TopLeft.X + ((dblWidth / colFormations.Count) * intFormation)
        .Y = oGTPaperRange.TopLeft.Y
        .Z = 0
      End With
      With oGTFormationRange.BottomRight
        .X = oGTFormationRange.TopLeft.X + (dblWidth / colFormations.Count)
        .Y = oGTFormationRange.TopLeft.Y + (dblWidth / colFormations.Count)
        .Z = 0
      End With
      oGTApplication = GTClassFactory.Create(Of IGTApplication)()
      oGTPlotMap = oGTApplication.ActivePlotWindow.InsertMap(oGTFormationRange)
      LoadLegend(oGTPlotMap, oPlotBoundary)

      ' Fit the Formation inside the MapFrame
      oGTWorldRange = GetFormationRange(oFormation.FNO, oFormation.FID, oFormation.CNO)
      With oGTPlotMap
        .Frame.Activate()
        .ZoomArea(oGTWorldRange)
        .Frame.Deactivate()
      End With

      intFormation = intFormation + 1

    Next oFormation


    Exit Sub

ErrorHandler:
    MsgBox("NewPlotWindowCreation.InsertFormationFrames: " & Err.Description)
    Err.Clear()

  End Sub


  Public Function InsertMapFrame(ByRef rsGroupsDRI As ADODB.Recordset, ByRef rsRedlines As ADODB.Recordset) As IGTPlotMap

    Dim oGTPoint As IGTPoint
    Dim oGTPaperRange As IGTPaperRange
    Dim oGTApplication As IGTApplication

    Try
      oGTApplication = GTClassFactory.Create(Of IGTApplication)()

      oGTPaperRange = GTClassFactory.Create(Of IGTPaperRange)()

      oGTPoint = GTClassFactory.Create(Of IGTPoint)()
      oGTPoint.X = rsRedlines.Fields("MF_COORDINATE_X1").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_X").Value * dSCALE
      oGTPoint.Y = rsRedlines.Fields("MF_COORDINATE_Y1").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_Y").Value * dSCALE
      oGTPoint.Z = 0

      oGTPaperRange.TopLeft = oGTPoint

      oGTPoint = GTClassFactory.Create(Of IGTPoint)()
      oGTPoint.X = rsRedlines.Fields("MF_COORDINATE_X2").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_X").Value * dSCALE
      oGTPoint.Y = rsRedlines.Fields("MF_COORDINATE_Y2").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_Y").Value * dSCALE
      oGTPoint.Z = 0

      oGTPaperRange.BottomRight = oGTPoint

            InsertMapFrame = oGTApplication.ActivePlotWindow.InsertMap(oGTPaperRange)

        Catch ex As Exception
      MsgBox("NewPlotWindowCreation.InsertMapFrame:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      InsertMapFrame = Nothing
    End Try

  End Function

  Public Function InsertObject(ByRef rsGroupsDRI As ADODB.Recordset, ByRef rsObjects As ADODB.Recordset, ByVal file As String) As IGTPlotFrame

    Dim bLinkFile As Boolean
    Dim oGTPoint As IGTPoint
    Dim oGTPoint2 As IGTPoint
    Dim oGTApplication As IGTApplication

    Try

      bLinkFile = IIf(rsObjects.Fields("OB_LINKFILE").Value = 1, True, False)

      oGTApplication = GTClassFactory.Create(Of IGTApplication)()

      '
      ' Coordinites seem to be backwards.  Set Y for X and X for Y
      '

      oGTPoint = GTClassFactory.Create(Of IGTPoint)()
      oGTPoint.X = rsObjects.Fields("OB_COORDINATE_X1").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_X").Value * dSCALE
      oGTPoint.Y = rsObjects.Fields("OB_COORDINATE_Y1").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_Y").Value * dSCALE
      oGTPoint.Z = 0

      If Not IsDBNull(rsObjects.Fields("OB_COORDINATE_X2").Value) Then
        oGTPoint2 = GTClassFactory.Create(Of IGTPoint)()
        oGTPoint2.X = rsObjects.Fields("OB_COORDINATE_X2").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_X").Value * dSCALE
        oGTPoint2.Y = rsObjects.Fields("OB_COORDINATE_Y2").Value * dSCALE + rsGroupsDRI.Fields("GROUP_OFFSET_Y").Value * dSCALE
        oGTPoint2.Z = 0

        InsertObject = oGTApplication.ActivePlotWindow.InsertObjectFromFile(file, bLinkFile, oGTPoint, oGTPoint2)
      Else
        InsertObject = oGTApplication.ActivePlotWindow.InsertObjectFromFile(file, bLinkFile, oGTPoint)
      End If

    Catch ex As Exception
      MsgBox("NewPlotWindowCreation.InsertObject:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      InsertObject = Nothing
    End Try

  End Function

  Public Sub RotateMapView(ByVal oPlotBoundary As PlotBoundary)

    Dim oGTKeyObject As IGTKeyObject
    Dim oGTComponent As IGTComponent
    Dim oGTComponents As IGTComponents
    Dim oGTGeometry As IGTGeometry

    Dim dSlope As Double
    Dim dAngle As Double
    Dim dAngleDeg As Double

    Dim oGFramme As GFramme
    Dim oGTApplication As IGTApplication

    Try

      oGTApplication = GTClassFactory.Create(Of IGTApplication)()
      oGTKeyObject = oGTApplication.DataContext.OpenFeature(oPlotBoundary.FNO, oPlotBoundary.FID)
      oGTComponents = oGTKeyObject.Components
      oGTComponent = oGTComponents.GetComponent(oPlotBoundary.CNO)
      oGTGeometry = oGTComponent.Geometry

      dSlope = (oGTGeometry.GetKeypointPosition(0).Y - oGTGeometry.GetKeypointPosition(1).Y) / (oGTGeometry.GetKeypointPosition(0).X - oGTGeometry.GetKeypointPosition(1).X)
      dAngle = System.Math.Atan(dSlope) 'm_dAngle = Atan2(m_pRotateAnchor.Y - UserPoint.Y, m_pRotateAnchor.X - UserPoint.X) ' Using the dSlope and Atan() instead of Atan2() keeps the polygon right reading.
      dAngleDeg = dAngle * (180 / Math.PI)

      oGFramme = New GFramme
      If Not dAngleDeg = 0 Then
        oGFramme.RotateMapView(oPlotBoundary, -dAngleDeg)
      End If

    Catch ex As Exception
      MsgBox("NewPlotWindowCreation.RotateMapView:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    Finally
      oGFramme = Nothing
    End Try

  End Sub


  Public Sub DrawInsetBorder(ByVal oPlotBoundary As PlotBoundary)

    Dim oPoint As IGTPoint = GTClassFactory.Create(Of IGTPoint)()
    Dim oGTPolylineGeometry As IGTPolylineGeometry
    Dim oGTPlotRedline As IGTPlotRedline

    Dim sStatusBarText As String
    Dim oGTApplication As IGTApplication

    oGTApplication = GTClassFactory.Create(Of IGTApplication)()
    sStatusBarText = oGTApplication.GetStatusBarText(GTStatusPanelConstants.gtaspcMessage)

    Try

      oGTPolylineGeometry = GTClassFactory.Create(Of IGTPolylineGeometry)()

      With oPoint
        .X = oPlotBoundary.SheetInset
        .Y = oPlotBoundary.SheetInset
        .Z = 0
      End With
      oGTPolylineGeometry.Points.Add(oPoint)

      With oPoint
        .X = oPlotBoundary.SheetWidth - oPlotBoundary.SheetInset
        .Y = oPlotBoundary.SheetInset
        .Z = 0
      End With
      oGTPolylineGeometry.Points.Add(oPoint)

      With oPoint
        .X = oPlotBoundary.SheetWidth - oPlotBoundary.SheetInset
        .Y = oPlotBoundary.SheetHeigh - oPlotBoundary.SheetInset
        .Z = 0
      End With
      oGTPolylineGeometry.Points.Add(oPoint)

      With oPoint
        .X = oPlotBoundary.SheetInset
        .Y = oPlotBoundary.SheetHeigh - oPlotBoundary.SheetInset
        .Z = 0
      End With
      oGTPolylineGeometry.Points.Add(oPoint)

      With oPoint
        .X = oPlotBoundary.SheetInset
        .Y = oPlotBoundary.SheetInset
        .Z = 0
      End With
      oGTPolylineGeometry.Points.Add(oPoint)


      oGTPlotRedline = oGTApplication.NamedPlots(oPlotBoundary.Name).NewRedline(oGTPolylineGeometry, oPlotBoundary.SheetStyleNo)


    Catch ex As Exception
      MsgBox("NewPlotWindowCreation.DrawInsetBorder:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    Finally
    End Try

  End Sub

  Public Sub DrawInsetBorder_PolygonGeometry(ByVal oPlotBoundary As PlotBoundary)

    ' This was used to generate borders however after the GTech 10.1 upgrade the polygons covered the map window preventing users from activating the map frame.
    Dim oPoint As IGTPoint = GTClassFactory.Create(Of IGTPoint)()

    Dim oGTPolygonGeometry As IGTPolygonGeometry
    Dim oGTPlotRedline As IGTPlotRedline

    Dim sStatusBarText As String
    Dim oGTApplication As IGTApplication

    oGTApplication = GTClassFactory.Create(Of IGTApplication)()
    sStatusBarText = oGTApplication.GetStatusBarText(GTStatusPanelConstants.gtaspcMessage)

    Try

      oGTPolygonGeometry = GTClassFactory.Create(Of IGTPolygonGeometry)()

      With oPoint
        .X = oPlotBoundary.SheetInset
        .Y = oPlotBoundary.SheetInset
        .Z = 0
      End With
      oGTPolygonGeometry.Points.Add(oPoint)

      With oPoint
        .X = oPlotBoundary.SheetWidth - oPlotBoundary.SheetInset
        .Y = oPlotBoundary.SheetInset
        .Z = 0
      End With
      oGTPolygonGeometry.Points.Add(oPoint)

      With oPoint
        .X = oPlotBoundary.SheetWidth - oPlotBoundary.SheetInset
        .Y = oPlotBoundary.SheetHeigh - oPlotBoundary.SheetInset
        .Z = 0
      End With
      oGTPolygonGeometry.Points.Add(oPoint)

      With oPoint
        .X = oPlotBoundary.SheetInset
        .Y = oPlotBoundary.SheetHeigh - oPlotBoundary.SheetInset
        .Z = 0
      End With
      oGTPolygonGeometry.Points.Add(oPoint)


      oGTPlotRedline = oGTApplication.NamedPlots(oPlotBoundary.Name).NewRedline(oGTPolygonGeometry, oPlotBoundary.SheetStyleNo)



    Catch ex As Exception
      MsgBox("NewPlotWindowCreation.DrawInsetBorder:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    Finally
    End Try

  End Sub

  Public Sub LoadLegend(ByVal oGTPlotMap As IGTPlotMap, ByVal oPlotBoundary As PlotBoundary)

    Try
      ' Load the legend
      LoadLegend(oGTPlotMap, oPlotBoundary.DetailID, oPlotBoundary.Legend)

    Catch ex As Exception
      MsgBox("NewPlotWindowCreation.LoadLegend:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
    End Try

  End Sub

  Public Sub LoadLegend(ByVal oGTPlotMap As IGTPlotMap, ByVal iDetailID As Integer, ByVal sLegend As String)

    Try
      ' Load the legend
      If iDetailID = 0 Then 'Plot Boundary is in Geo
        oGTPlotMap.DisplayService.ReplaceLegend(sLegend)
      Else
        oGTPlotMap.DisplayService.ReplaceLegend(sLegend, iDetailID)
      End If

    Catch ex As Exception
      MsgBox("NewPlotWindowCreation.LoadLegend:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)

    End Try

  End Sub

  ' Position Map in Frame
  Public Sub ZoomToPlotBoundary(ByVal oGTPlotMap As IGTPlotMap, ByVal oPlotBoundary As PlotBoundary, Optional ByVal blnUpdateScale As Boolean = True, Optional ByVal dDisplayFactor As Double = 1, Optional ByVal lStyleNo As Long = 0)

    Dim oGTWorldRange As IGTWorldRange
    Dim oGTWorldRangeScaled As IGTWorldRange
    Dim gtPointBL As IGTPoint = GTClassFactory.Create(Of IGTPoint)()
    Dim gtPointTR As IGTPoint = GTClassFactory.Create(Of IGTPoint)()


    On Error GoTo ErrorHandler


    oGTWorldRange = GetFeatureRange(oPlotBoundary.FNO, oPlotBoundary.FID, oPlotBoundary.CNO)

    With oGTPlotMap
      .Frame.Activate()
      If blnUpdateScale Then
        Select Case oPlotBoundary.MapScale
          Case My.Resources.NewPlotWindow.UpdatePlotBoundaryInfo_FitBoundary
            .ZoomArea(oGTWorldRange)
          Case My.Resources.NewPlotWindow.ActiveMapWindowScale
            .CenterSelectedObjects()
            '.ZoomArea oGTWorldRange
            '.DisplayScale = Application.ActiveMapWindow.DisplayScale
          Case Else
            .ZoomArea(oGTWorldRange)
            If GetMeasurmentUnits(oPlotBoundary.MapScale) = MeasurmentUnits.Metric Then
              .DisplayScale = GetScaleValue(oPlotBoundary.MapScale)
            Else
              .DisplayScale = GetScaleValue(oPlotBoundary.MapScale) * 12
            End If
        End Select
      Else
        ' Scale the feature by a factor within the Map Window
        If dDisplayFactor > 1 Or dDisplayFactor < -1 Then
          Dim dFeatureWidth As Double
          Dim dFeatureHeight As Double

          dFeatureWidth = oGTWorldRange.TopRight.Y - oGTWorldRange.BottomLeft.Y
          dFeatureHeight = oGTWorldRange.TopRight.Y - oGTWorldRange.BottomLeft.Y


          gtPointBL = GTClassFactory.Create(Of IGTPoint)()
          gtPointTR = GTClassFactory.Create(Of IGTPoint)()
          gtPointBL.X = oGTWorldRange.BottomLeft.X + ((dFeatureWidth * dDisplayFactor) / 2)
          gtPointBL.Y = oGTWorldRange.BottomLeft.Y + ((dFeatureHeight * dDisplayFactor) / 2)
          gtPointTR.X = oGTWorldRange.TopRight.X - ((dFeatureWidth * dDisplayFactor) / 2)
          gtPointTR.Y = oGTWorldRange.TopRight.Y - ((dFeatureHeight * dDisplayFactor) / 2)

          oGTWorldRange.BottomLeft = gtPointBL
          oGTWorldRange.TopRight = gtPointTR

          gtPointBL = Nothing
          gtPointTR = Nothing

        End If


        .ZoomArea(oGTWorldRange)


        ' Todo -Removed Draw Map Border in redline FUTURE add query to legend to highlight Plot Boundary.  Problem is persisting the plot boundary query after the session ends.
        'If lStyleNo > 0 Then
        '  oGTWorldRangeScaled = GetFeatureRange(oPlotBoundary.FNO, oPlotBoundary.FID, oPlotBoundary.CNO)
        '  gtPointBL = New GTPoint
        '  gtPointTR = New GTPoint
        '  gtPointBL = oGTPlotMap.WorldToPaper(oGTWorldRangeScaled.BottomLeft)
        '  gtPointTR = oGTPlotMap.WorldToPaper(oGTWorldRangeScaled.TopRight)

        '  Dim oGTNamedPlot As GTNamedPlot
        '  oGTNamedPlot = GTApplication.NamedPlots(oPlotBoundary.Name)


        '  ' DrawRectangleWithPolyline(gtPointBL, gtPointTR)
        '  Dim oPolylineGeometry As GTPolylineGeometry
        '  Dim oGTPlotRedline As GTPlotRedline
        '  Dim iGroupNumber As Integer
        '  Dim oGTPoint As GTPoint

        '  oPolylineGeometry = New GTPolylineGeometry
        '  oPolylineGeometry.Points.Add(gtPointBL)

        '  oGTPoint = New GTPoint
        '  oGTPoint.X = gtPointBL.X
        '  oGTPoint.Y = gtPointTR.Y
        '  oPolylineGeometry.Points.Add(oGTPoint)

        '  oPolylineGeometry.Points.Add(gtPointTR)

        '  oGTPoint = New GTPoint
        '  oGTPoint.Y = gtPointBL.Y
        '  oGTPoint.X = gtPointTR.X
        '  oPolylineGeometry.Points.Add(oGTPoint)

        '  oPolylineGeometry.Points.Add(gtPointBL)

        '  iGroupNumber = -1 ' Begin new group
        '  oGTPlotRedline = oGTNamedPlot.NewRedline(oPolylineGeometry, lStyleNo, iGroupNumber)
        '  iGroupNumber = oGTPlotRedline.GroupNumber


        '  oGTPoint = Nothing
        'End If

        gtPointBL = Nothing
        gtPointTR = Nothing

      End If
      .Frame.Deactivate()
    End With

    Exit Sub

ErrorHandler:
    MsgBox("NewPlotWindowCreation.ZoomToPlotBoundary: " & Err.Description)
    Err.Clear()

  End Sub


  Public Function GetFormationRange(ByVal lngG3E_FNO As Long, ByVal lngG3E_FID As Long, ByVal lngG3E_CNO As Long) As IGTWorldRange

    Dim oRelSvc As IGTRelationshipService

    Dim oGTComponent As IGTComponent
    Dim oGTComponents As IGTComponents
    Dim oGTWorldRange As IGTWorldRange
    Dim oGTWorldRangeDuct As IGTWorldRange

    Dim oGTKeyObjectFormation As IGTKeyObject
    Dim oGTKeyObjectDuct As IGTKeyObject
    Dim oGTKeyObject As IGTKeyObject
    Dim oGTKeyObjects As IGTKeyObjects

    Dim intRelCount As Integer

    'Dim oOrientedPointGeometry As New GTPointGeometry
    Dim oOrientedPointGeometry As IGTOrientedPointGeometry

    Dim oGTApplication As IGTApplication
    Dim oGTPoint As IGTPoint

    On Error GoTo ErrorHandler


    '
    ' Get Formation Range
    '
    oGTApplication = GTClassFactory.Create(Of IGTApplication)()
    oGTKeyObjectFormation = oGTApplication.DataContext.OpenFeature(lngG3E_FNO, lngG3E_FID)
    oGTComponents = oGTKeyObjectFormation.Components
    oGTComponent = oGTComponents.GetComponent(lngG3E_CNO)

    'oGTWorldRange = GTApplication.CreateService(GTServiceConstants.gtsvcWorldRange)
    oGTWorldRange = GTClassFactory.Create(Of IGTWorldRange)()
    oGTWorldRange = oGTComponent.Geometry.Range
    'GetRange(oGTComponent.Geometry, oGTWorldRange.BottomLeft, oGTWorldRange.TopRight)




    'oGTWorldRange = GTApplication.CreateService(GTServiceConstants.gtsvcWorldRange)
    oGTWorldRange = GTClassFactory.Create(Of IGTWorldRange)()


    oGTWorldRange = oGTComponent.Geometry.Range
    'GetRange(oGTComponent.Geometry, oGTWorldRange.BottomLeft, oGTWorldRange.TopRight)




    '
    ' Now include duct ranges
    '

    'oRelSvc = oGTApplication.CreateService(GTServiceConstants.gtsvcRelationshipService)
    oRelSvc = GTClassFactory.Create(Of IGTRelationshipService)()
    oRelSvc.DataContext = oGTApplication.DataContext
    oRelSvc.ActiveFeature = oGTKeyObjectFormation

    'Todo remove hard coded 7 value
    oGTKeyObjects = oRelSvc.GetRelatedFeatures(7)

    ' was For intRelCount = 1 To oGTKeyObjects.Count
    For intRelCount = 0 To oGTKeyObjects.Count - 1
      oGTKeyObject = oGTKeyObjects(intRelCount)
      oGTKeyObjectDuct = oGTApplication.DataContext.OpenFeature(oGTKeyObject.FNO, oGTKeyObject.FID)
      oGTComponents = oGTKeyObjectDuct.Components

      'Todo remove hard coded 2411 value
      If lngG3E_CNO = 2411 Then
        'Todo remove hard coded 2321 value
        oGTComponent = oGTComponents.GetComponent(2321)
        oOrientedPointGeometry = oGTComponent.Geometry
        If Not oGTComponent.Geometry Is Nothing Then

          ExtendWorldRange(oGTWorldRange, oOrientedPointGeometry)

        End If
      Else
        'Todo remove hard coded 2323 value
        oGTComponent = oGTComponents.GetComponent(2323)
        oOrientedPointGeometry = oGTComponent.Geometry
        If Not oGTComponent.Geometry Is Nothing Then

          ExtendWorldRange(oGTWorldRange, oOrientedPointGeometry)

        End If
      End If

    Next intRelCount


    '
    ' Add 10%
    '
    oGTWorldRange.BottomLeft.X = oGTWorldRange.BottomLeft.X - ((oGTWorldRange.TopRight.X - oGTWorldRange.BottomLeft.X) * 0.1)
    oGTWorldRange.BottomLeft.Y = oGTWorldRange.BottomLeft.Y - ((oGTWorldRange.TopRight.Y - oGTWorldRange.BottomLeft.Y) * 0.1)
    oGTWorldRange.TopRight.X = oGTWorldRange.TopRight.X + ((oGTWorldRange.TopRight.X - oGTWorldRange.BottomLeft.X) * 0.1)
    oGTWorldRange.TopRight.Y = oGTWorldRange.TopRight.Y + ((oGTWorldRange.TopRight.Y - oGTWorldRange.BottomLeft.Y) * 0.1)


    GetFormationRange = oGTWorldRange

    Exit Function

ErrorHandler:
    MsgBox("NewPlotWindowCreation.GetFormationRange: " & Err.Description)
    Err.Clear()

  End Function



  Public Sub ExtendWorldRange(ByRef oGTWorldRange As IGTWorldRange, ByVal oOrientedPointGeometry As IGTOrientedPointGeometry)

    Dim oGTPoint As IGTPoint
    Dim oGTPointBL As IGTPoint
    Dim oGTPointTR As IGTPoint

    oGTPoint = oOrientedPointGeometry.Origin
    oGTPointBL = oGTWorldRange.BottomLeft
    oGTPointTR = oGTWorldRange.TopRight

    If oGTPoint.X > oGTPointTR.X Then oGTPointTR.X = oGTPoint.X
    If oGTPoint.Y > oGTPointTR.Y Then oGTPointTR.Y = oGTPoint.Y

    If oGTPoint.X < oGTPointBL.X Then oGTPointBL.X = oGTPoint.X
    If oGTPoint.Y < oGTPointBL.Y Then oGTPointBL.Y = oGTPoint.Y

    oGTWorldRange.BottomLeft = oGTPointBL
    oGTWorldRange.TopRight = oGTPointTR

  End Sub



  Public Function GetField(ByVal strText As String, ByRef rsAttributeQuery As ADODB.Recordset) As String
    Dim strNewText As String
    Dim objField As ADODB.Field

    On Error GoTo ErrorHandler

    GetField = ""

    If InStr(strText, "[") = 0 Then
      Exit Function
    End If
    If rsAttributeQuery.RecordCount = 0 Then
      Exit Function
    End If

    For Each objField In rsAttributeQuery.Fields
      With objField
        If InStr(strText, "[" + .Name + "]") <> 0 Then
          GetField = .Name
        End If
      End With
    Next

    Exit Function

ErrorHandler:
    MsgBox("NewPlotWindowCreation.ReplaceFields - Error:" & Err.Description)
    Err.Clear()

  End Function

  Public Function ReplaceFields(ByVal strText As String, ByRef rsAttributeQuery As ADODB.Recordset) As String
    Dim strNewText As String
    Dim objField As ADODB.Field

    On Error GoTo ErrorHandler

    ReplaceFields = strText


    If InStr(strText, "[") = 0 Then
      Exit Function
    End If
    If rsAttributeQuery.RecordCount = 0 Then
      Exit Function
    End If

    For Each objField In rsAttributeQuery.Fields
      With objField
        If InStr(strText, "[" + .Name + "]") <> 0 Then

          If Not IsDBNull(objField.Value) Then 'If objField.Value <> vbNullString Then
            strText = Replace(strText, "[" + .Name + "]", .Value)
          Else
            strText = Replace(strText, "[" + .Name + "]", "")
          End If
        End If
      End With
    Next

    ReplaceFields = strText

    Exit Function

ErrorHandler:
    MsgBox("NewPlotWindowCreation.ReplaceFields - Error:" & Err.Description)
    Err.Clear()

  End Function

  Public Function CreateNewNamedPlot(ByVal oPlotBoundary As PlotBoundary) As IGTPlotWindow

    Dim oGTNamedPlot As IGTNamedPlot
    'Dim oGTPlotWindow As IGTPlotWindow
    Dim sStatusBarTextTemp As String
    Dim sStatusBarText As String
    Dim oGTApplication As IGTApplication

    oGTApplication = GTClassFactory.Create(Of IGTApplication)()
    sStatusBarText = oGTApplication.GetStatusBarText(GTStatusPanelConstants.gtaspcMessage)

    Try
      oGTNamedPlot = oGTApplication.NewNamedPlot(oPlotBoundary.Name)
      ' Set Plot Page Size
      oGTNamedPlot.PaperHeight = oPlotBoundary.SheetHeigh
      oGTNamedPlot.PaperWidth = oPlotBoundary.SheetWidth

      ' Auto open the new named plot - need to only do this if single named plot created otherwise ask user at the end using dialog FUTURE -Currently need to have PlotWindow open to Insert a Map Window
      sStatusBarTextTemp = oGTApplication.GetStatusBarText(GTStatusPanelConstants.gtaspcMessage)
      CreateNewNamedPlot = oGTApplication.NewPlotWindow(oGTNamedPlot)
      oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, sStatusBarTextTemp)

      ' Fit Page in window
      Dim oGFramme As New GFramme
      oGFramme.ZoomToFitPage(oPlotBoundary.Name)

      oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, sStatusBarTextTemp)

    Catch ex As Exception
      MsgBox("NewPlotWindowCreation.CreateNewNamedPlot: " & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      Err.Clear()
      CreateNewNamedPlot = Nothing

    Finally
      oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, sStatusBarText)
      Err.Clear()

    End Try

  End Function


End Module
