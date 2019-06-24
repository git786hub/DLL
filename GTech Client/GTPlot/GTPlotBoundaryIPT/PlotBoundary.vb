Imports System
Imports System.Math
Imports System.Collections.Generic
Imports System.Text
Imports System.Windows.Forms
Imports Intergraph.GTechnology.API
Imports Intergraph.GTechnology.Interfaces
Imports System.Security



<Assembly: SecurityRules(SecurityRuleSet.Level1)>


Namespace GTPlacementTechnique


  Public Class PlotBoundaryIPT

    Implements IGTPlacementTechnique

    Private m_GTApplication As IGTApplication
    Private m_GTHelperService As IGTHelperService

    Private Sub m_PlotBoundaryForm_AboutToPlaceGeometry(ByVal sender As Object, ByVal e As System.EventArgs, ByVal m_sType As String, ByVal m_sSheetSize As String, ByVal m_sSheetOrientation As String, ByVal m_sMapScale As String) Handles m_PlotBoundaryForm.AboutToPlaceGeometry

      Dim rsComp As ADODB.Recordset
      Dim oneKey As IGTKeyObject = GTClassFactory.Create(Of IGTKeyObject)()
      Dim oneComp As IGTComponent = GTClassFactory.Create(Of IGTComponent)()
      Dim sSheetOrientationKey As String
      Dim sTypeKey As String

      m_GTApplication = GTClassFactory.Create(Of IGTApplication)()
      m_GTHelperService = GTClassFactory.Create(Of IGTHelperService)()
      m_GTHelperService.DataContext = m_GTApplication.DataContext

      Try

        ' Populate attribute values
        m_sErrorMsg = "Populate attribute values"

        'oneKey = getFeatureFromCollection(g3eFNO, m_KeyObjectCollection);
        For Each oneKey In m_KeyObjectCollection
          If oneKey.FNO = GTPlotMetadata.Parameters.PlotBoundary_G3E_FNO Then
            oneComp = oneKey.Components.GetComponent(GTPlotMetadata.Parameters.PlotBoundary_G3E_CNO)
            rsComp = oneComp.Recordset
            If Not rsComp.BOF = True Then rsComp.MoveFirst()

            ' Todo -Future -Add autonum textbox to the placement dialog.
            'rsComp.Fields(GTPlotMetadata.Parameters.PlotBoundaryAttribute_Name).Value = ""

            ' Sheet Orientation
            sSheetOrientationKey = GetSheetOrientationKey(m_PlotBoundaryForm.m_sSheetOrientation)

            If GTPlotMetadata.Parameters.PlotBoundaryStoreSingleCharacterForOrientation.ToUpper = "YES" Then
              rsComp.Fields(GTPlotMetadata.Parameters.PlotBoundaryAttribute_Orientation).Value = sSheetOrientationKey.Substring(0, 1)
            Else
              If GTPlotMetadata.Parameters.PlotBoundaryDataToUpper.ToUpper = "YES" Then
                rsComp.Fields(GTPlotMetadata.Parameters.PlotBoundaryAttribute_Orientation).Value = sSheetOrientationKey.ToUpper
              Else
                rsComp.Fields(GTPlotMetadata.Parameters.PlotBoundaryAttribute_Orientation).Value = sSheetOrientationKey
              End If
            End If

            ' Type
            sTypeKey = GetTypeKey(m_PlotBoundaryForm.m_sType)

            If GTPlotMetadata.Parameters.PlotBoundaryDataToUpper.ToUpper = "YES" Then
              rsComp.Fields(GTPlotMetadata.Parameters.PlotBoundaryAttribute_Type).Value = sTypeKey.ToUpper
              rsComp.Fields(GTPlotMetadata.Parameters.PlotBoundaryAttribute_PageSize).Value = FormatStringSetSheetNameSize(m_PlotBoundaryForm.m_sSheetName.ToUpper, m_PlotBoundaryForm.m_sSheetSize.ToUpper)
            Else
              rsComp.Fields(GTPlotMetadata.Parameters.PlotBoundaryAttribute_Type).Value = sTypeKey

              If GTPlotMetadata.Parameters.PlotBoundaryDataStoreSizeOnly.ToUpper = "YES" Then
                rsComp.Fields(GTPlotMetadata.Parameters.PlotBoundaryAttribute_PageSize).Value = m_PlotBoundaryForm.m_sSheetSize
              Else
                rsComp.Fields(GTPlotMetadata.Parameters.PlotBoundaryAttribute_PageSize).Value = FormatStringSetSheetNameSize(m_PlotBoundaryForm.m_sSheetName, m_PlotBoundaryForm.m_sSheetSize)
              End If
            End If

            rsComp.Fields(GTPlotMetadata.Parameters.PlotBoundaryAttribute_Scale).Value = m_PlotBoundaryForm.m_sMapScale

            ' Refresh feature explorer to show attribute value changes
            m_sErrorMsg = "Attempting to refresh attribute value updates in feature explorer"
            'svcFE.Clear()
            If Not IsCalledByDesignAssist() Then
              svcFE.ExploreFeatures(m_KeyObjectCollection, "Edit")
            End If
            'svcFE.Visible = True

          End If
        Next

      Catch ex As Exception
        MsgBox("PlotBoundaryIPT.m_PlotBoundaryForm_AboutToPlaceGeometry:" & vbCrLf & ex.Message & vbLf & m_sErrorMsg, vbOKOnly + vbExclamation, ex.Source)
      End Try

    End Sub

#Region "IGTPlacementTechnique Members"
    'Public WithEvents m_oCustomCommandHelper As GTCustomCommandHelper

    Private WithEvents m_PTHelper As IGTPlacementTechniqueHelper
    Private m_GComps As IGTGraphicComponents
    Private m_GComp As IGTGraphicComponent
    Private m_KeyObject As IGTKeyObject
    Private m_KeyObjectCollection As IGTKeyObjects
    Private m_bSilent As Boolean = False

    Private WithEvents m_PlotBoundaryForm As PlotBoundaryForm

    Private m_dAngle As Double = 0
    Private m_dAngleDeg As Double = 0
    Private m_bRotateMode As Boolean = False
    Private m_bControlKeyHeld As Boolean = False
        Private m_pRotateAnchor As IGTPoint = Nothing

    Private feVisable As Boolean = False
        Private svcFE As IGTFeatureExplorerService

    Public m_sErrorMsg As String


    Public Sub AbortPlacement() Implements IGTPlacementTechnique.AbortPlacement
      m_PlotBoundaryForm.Dispose()
      m_PTHelper.AbortPlacement()
    End Sub

    Public Property GraphicComponent As IGTGraphicComponent Implements IGTPlacementTechnique.GraphicComponent
      Get
        GraphicComponent = m_GComp
      End Get
      Set(value As IGTGraphicComponent)
        m_GComp = value
      End Set
    End Property

    Public Property GraphicComponents As IGTGraphicComponents Implements IGTPlacementTechnique.GraphicComponents
      Get
        GraphicComponents = m_GComps
      End Get
      Set(value As IGTGraphicComponents)
        m_GComps = value
      End Set
    End Property


    Public Sub MouseDblClick(MapWindow As IGTMapWindow, UserPoint As IGTPoint, ShiftState As Integer, LocatedObjects As IGTDDCKeyObjects) Implements IGTPlacementTechnique.MouseDblClick

      m_PTHelper.MouseDblClick(UserPoint, ShiftState)

      ' UnSubscribe to m_PTHelper events
      'm_PTHelper.c.ArcComplete = m_PTHelper_ArcComplete()
      'm_PTHelper.ConstructionAidComplete = m_PTHelper_ConstructionAidComplete()

      ' End Placement
      'm_PTHelper.EndPlacement()

    End Sub

    Private Function CreatePolygonGeometry(ByVal UserPoint As IGTPoint, Optional ByVal dAngle As Double = 0) As IGTPolygonGeometry

            Dim angle As Double = Math.PI * dAngle / 180.0
            Dim angle90 As Double = Math.PI * 90 / 180.0
            Dim angle180 As Double = Math.PI * 180 / 180.0

            Dim sinAngle As Double = Math.Sin(angle)
            Dim cosAngle As Double = Math.Cos(angle)

            Dim oPolygonGeometry As IGTPolygonGeometry
            Dim oPoint1 As IGTPoint
            Dim oPoint2 As IGTPoint
            Dim oPoint3 As IGTPoint

            m_GTApplication = GTClassFactory.Create(Of IGTApplication)()
            m_GTHelperService = GTClassFactory.Create(Of IGTHelperService)()
            m_GTHelperService.DataContext = m_GTApplication.DataContext

            ' This is hard coded for now because I cant seem to get the GeographicPointToStorage() working.
            '  Corrected to default to double 1 instead of integer 0 (Rich Adase, 15-JAN-2019)
            Dim dPointToStorage As Double = 1.0
            '  Disabled since imperial conversion was resulting in too-small polygons (Rich Adase, 15-JAN-2019)
            'If GTPlotMetadata.Parameters.MeasurementUnits = "Imperial" Then
            '    dPointToStorage = 0.3048 'Feet to Meters conversion
            'End If

            ' Geometry construction process.
            oPolygonGeometry = GTClassFactory.Create(Of IGTPolygonGeometry)()

            oPolygonGeometry.Points.Add(UserPoint)

            oPoint1 = GTClassFactory.Create(Of IGTPoint)()
            oPoint1.X = UserPoint.X + Cos(angle) * m_PlotBoundaryForm.MapWidthScaled * dPointToStorage
            oPoint1.Y = UserPoint.Y + Sin(angle) * m_PlotBoundaryForm.MapWidthScaled * dPointToStorage
            oPolygonGeometry.Points.Add(oPoint1)

            'Dim oSPoint1 = GTClassFactory.Create(Of IGTPoint)()
            'oSPoint1 = m_GTHelperService.GeographicPointToStorage(oPoint1)
            'oPolygonGeometry.Points.Add(oSPoint1)

            oPoint2 = GTClassFactory.Create(Of IGTPoint)()
            oPoint2.X = oPoint1.X + Cos(angle + angle90) * m_PlotBoundaryForm.MapHeightScaled * dPointToStorage
            oPoint2.Y = oPoint1.Y + Sin(angle + angle90) * m_PlotBoundaryForm.MapHeightScaled * dPointToStorage
            oPolygonGeometry.Points.Add(oPoint2)

            'Dim oSPoint2 = GTClassFactory.Create(Of IGTPoint)()
            'oSPoint2 = m_GTHelperService.GeographicPointToStorage(oPoint2)
            'oPolygonGeometry.Points.Add(oSPoint2)

            oPoint3 = GTClassFactory.Create(Of IGTPoint)()
            oPoint3.X = oPoint2.X + Cos(angle + angle180) * m_PlotBoundaryForm.MapWidthScaled * dPointToStorage
            oPoint3.Y = oPoint2.Y + Sin(angle + angle180) * m_PlotBoundaryForm.MapWidthScaled * dPointToStorage
            oPolygonGeometry.Points.Add(oPoint3)

            'Dim oSPoint3 = GTClassFactory.Create(Of IGTPoint)()
            'oSPoint3 = m_GTHelperService.GeographicPointToStorage(oPoint3)
            'oPolygonGeometry.Points.Add(oSPoint3)

            oPolygonGeometry.Points.Add(UserPoint)

            CreatePolygonGeometry = oPolygonGeometry

        End Function

        Private Function IsCalledByDesignAssist() As Boolean
            Dim oGTApplication As IGTApplication

            Try
                oGTApplication = GTClassFactory.Create(Of IGTApplication)()
                If oGTApplication.Properties.Item("IsCalledByDesignAssist") Then
                    Return True
                End If
                Return False
            Catch ex As Exception
                Return False
            Finally
                oGTApplication = Nothing
            End Try

        End Function


        Public Sub StartPlacement(PTHelper As IGTPlacementTechniqueHelper, KeyObject As IGTKeyObject, KeyObjectCollection As IGTKeyObjects) Implements IGTPlacementTechnique.StartPlacement

            Dim objWinWrapper As New WinWrapper
            Dim oGTApplication As IGTApplication

            m_PTHelper = PTHelper
            m_KeyObject = KeyObject
            m_KeyObjectCollection = KeyObjectCollection

            ' Interactive placement technique processing

            'm_PTHelper.ConstructionAidsEnabled = GTConstructionAidsEnabledConstants.gtptConstructionAidsAll
            'm_PTHelper.ConstructionAidDynamicsEnabled = True
            m_PTHelper.ConstructionAidsEnabled = GTConstructionAidsEnabledConstants.gtptConstructionAidsNone
            m_PTHelper.ConstructionAidDynamicsEnabled = False
            m_PTHelper.StatusBarPromptsEnabled = False
            m_PTHelper.StartPlacement(m_KeyObject, m_KeyObjectCollection)


            ' Show dialog box
            oGTApplication = GTClassFactory.Create(Of IGTApplication)()
            oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, My.Resources.PlotBoundary.StartPlacement_SetStatusBarText)


            'm_sUsername = "Plot Boundary";

            ' Clear Feature Explorer (will be reassigned after Attributes adjusted
            If Not IsCalledByDesignAssist() Then
                svcFE = GTClassFactory.Create(Of IGTFeatureExplorerService)() ' oGTApplication.CreateService(GTServiceConstants.gtsvcFeatureExplorerService)
                svcFE.ExploreFeatures(m_KeyObjectCollection, "Edit")
            End If

            'If Not svcFE.Visible = False Then
            '  svcFE.Visible = False
            '  feVisable = True
            'End If

            m_PlotBoundaryForm = New PlotBoundaryForm
            m_PlotBoundaryForm.Text = GetUsername(GraphicComponent.ComponentName)
            m_PlotBoundaryForm.ShowDialog(objWinWrapper)
            If Not m_PlotBoundaryForm.PlaceComponent Then
                m_PTHelper.AbortPlacement()
            Else
                oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, String.Format(My.Resources.PlotBoundary.StartPlacement_SetStatusBarText_AnglePrompt, m_PlotBoundaryForm.Text, m_dAngleDeg))

                ' Subscribe to m_PTHelper events
                'm_PTHelper.ArcComplete += New EventHandler(m_PTHelper_ArcComplete)
                'm_PTHelper.ConstructionAidComplete += New EventHandler < GTConstructionAidCompleteEventArgs > (m_PTHelper_ConstructionAidComplete())
            End If


        End Sub

        Private Sub m_PTHelper_ArcComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_PTHelper.ArcComplete
    End Sub

    Private Sub m_PTHelper_ConstructionAidComplete(ByVal sender As Object, ByVal e As GTConstructionAidCompleteEventArgs) Handles m_PTHelper.ConstructionAidComplete
    End Sub

    Protected Overrides Sub Finalize()
      m_PlotBoundaryForm.Dispose()
      MyBase.Finalize()
    End Sub

#End Region

        Public Sub KeyUp(MapWindow As IGTMapWindow, KeyCode As Integer, ShiftState As Integer, EventMode As IGTPlacementTechniqueEventMode) Implements IGTPlacementTechnique.KeyUp
            Select Case KeyCode
                Case Keys.F2 ' Skip Component Placement
                    m_PTHelper.AbortPlacement()
                    Exit Sub
                Case Keys.Tab, Keys.Back  'Present user with WPB dialog again
                    If ShiftState = 0 Then
                        'm_PlotBoundaryForm = New PlotBoundaryForm()
                        m_PlotBoundaryForm.ShowDialog()
                        If Not m_PlotBoundaryForm.PlaceComponent Then
                            m_PTHelper.AbortPlacement()
                        End If
                    End If
                Case Else
                    m_PTHelper.KeyUp(KeyCode, ShiftState)
            End Select
        End Sub

        Public Sub MouseClick(MapWindow As IGTMapWindow, UserPoint As IGTPoint, Button As Integer, ShiftState As Integer, LocatedObjects As IGTDDCKeyObjects, EventMode As IGTPlacementTechniqueEventMode) Implements IGTPlacementTechnique.MouseClick
            Try

                ' The graphics displayed on the screen are not updated until SetGeometry is called.
                m_sErrorMsg = "Setting geometry rotation"
                If m_bRotateMode Then
                    m_PTHelper.SetGeometry(CreatePolygonGeometry(m_pRotateAnchor, m_dAngleDeg))
                    m_PTHelper.MouseClick(m_pRotateAnchor, Button, ShiftState)
                Else
                    m_PTHelper.SetGeometry(CreatePolygonGeometry(UserPoint, m_dAngleDeg))
                    m_PTHelper.MouseClick(UserPoint, Button, ShiftState)
                End If



                m_sErrorMsg = "Calling EndPlacement"
                m_PTHelper.EndPlacement() 'call EndPlacement to notify the placement command that the custom technique has completed geometry construction.


                ' Implement when right click is available:
                'Select Case Button
                '  Case 1
                '    'StoreValues m_sSheetSize, m_sSheetOrientation, m_sMapScale

                '    ' Can't use because there is no right mouse button to skip rotate.  Used Shift and Control to rotate instead.
                '    If m_bRotateMode Then
                '      ' The graphics displayed on the screen are not updated until SetGeometry is called.
                '      m_PTHelper.SetGeometry(CreatePolygonGeometry(m_pRotateAnchor, m_dAngleDeg))
                '      m_PTHelper.EndPlacement() 'call EndPlacement to notify the placement command that the custom technique has completed geometry construction.
                '    Else
                '      m_bRotateMode = True
                '      m_pRotateAnchor = New GTPoint
                '      m_pRotateAnchor = UserPoint
                '    End If
                '  Case 2 ' Plecement Techniques do not allow for right mouse click according to help!
                '    m_PlotBoundaryForm.ShowDialog() ' Reload form
                '    If Not m_PlotBoundaryForm.PlaceComponent Then m_PTHelper.AbortPlacement()
                'End Select

            Catch ex As Exception
                MsgBox("PlotBoundary.MouseClick:" & vbCrLf & ex.Message & vbLf & m_sErrorMsg, vbOKOnly + vbExclamation, ex.Source)

            End Try
        End Sub

        Public Sub MouseMove(MapWindow As IGTMapWindow, UserPoint As IGTPoint, ShiftState As Integer, LocatedObjects As IGTDDCKeyObjects, EventMode As IGTPlacementTechniqueEventMode) Implements IGTPlacementTechnique.MouseMove
            Dim dSlope As Double
            Dim oGTApplication As IGTApplication

            ' If user holds the Shift and/or Ctrl key(s) place the polygon in rotate mode until they are released.
            If ShiftState = 0 Then
                If m_bControlKeyHeld = True And m_bRotateMode = True Then
                    m_bRotateMode = False
                    m_bControlKeyHeld = False
                End If
            Else
                m_bRotateMode = True
                If Not m_bControlKeyHeld Then
                    m_bControlKeyHeld = True
                    m_pRotateAnchor = UserPoint
                End If
            End If

            If m_bRotateMode Then
                dSlope = (m_pRotateAnchor.Y - UserPoint.Y) / (m_pRotateAnchor.X - UserPoint.X)
                If Not System.Double.IsNaN(Atan(dSlope)) Then
                    m_dAngle = Atan(dSlope) 'm_dAngle = Atan2(m_pRotateAnchor.Y - UserPoint.Y, m_pRotateAnchor.X - UserPoint.X) ' Using the dSlope and Atan() instead of Atan2() keeps the polygon right reading.
                    m_dAngleDeg = m_dAngle * (180 / Math.PI)
                    Select Case ShiftState
                        Case 2 ' Round to the nearest degree
                            m_dAngleDeg = Round(m_dAngleDeg, 0)
                        Case 3 ' Snap to nearest 45deg
                            If m_dAngleDeg > 0 - 22.5 And m_dAngleDeg <= 0 + 22.5 Then m_dAngleDeg = 0
                            If m_dAngleDeg > 45 - 22.5 And m_dAngleDeg <= 45 + 22.5 Then m_dAngleDeg = 45
                            If m_dAngleDeg > 90 - 22.5 And m_dAngleDeg <= 90 + 22.5 Then m_dAngleDeg = 90
                            If m_dAngleDeg > -45 - 22.5 And m_dAngleDeg <= -45 + 22.5 Then m_dAngleDeg = -45
                            If m_dAngleDeg > -90 - 22.5 And m_dAngleDeg <= -90 + 22.5 Then m_dAngleDeg = -90
                    End Select
                End If
                m_PTHelper.SetGeometry(CreatePolygonGeometry(m_pRotateAnchor, m_dAngleDeg))
                m_PTHelper.MouseMove(m_pRotateAnchor, ShiftState)
            Else
                ' The graphics displayed on the screen are not updated until SetGeometry is called.
                m_PTHelper.SetGeometry(CreatePolygonGeometry(UserPoint, m_dAngleDeg))
                m_PTHelper.MouseMove(UserPoint, ShiftState)
            End If

            oGTApplication = GTClassFactory.Create(Of IGTApplication)()
            oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, String.Format(My.Resources.PlotBoundary.StartPlacement_SetStatusBarText_AnglePrompt, m_PlotBoundaryForm.Text, m_dAngleDeg))
            'GTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, UserPoint.X & " x " & UserPoint.Y & " shift= " & ShiftState & " angle= " & m_dAngleDeg)

        End Sub
    End Class
End Namespace