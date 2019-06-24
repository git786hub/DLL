Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Intergraph.GTechnology.API
Imports Intergraph.GTechnology.Interfaces

Namespace GTPlacementTechnique

  Public Class PlotBoundaryLabelSPT

    Implements IGTPlacementTechnique

    Private WithEvents m_PTHelper As IGTPlacementTechniqueHelper
    Private m_GComps As IGTGraphicComponents
    Private m_GComp As IGTGraphicComponent
    Private m_KeyObject As IGTKeyObject
    Private m_KeyObjectCollection As IGTKeyObjects
    Private m_bSilent As Boolean = False

    Private WithEvents m_oHelperPT As IGTPlacementTechniqueHelper

    Public Sub AbortPlacement() Implements IGTPlacementTechnique.AbortPlacement
      m_PTHelper.AbortPlacement()
    End Sub

        Public Property GraphicComponent() As IGTGraphicComponent Implements IGTPlacementTechnique.GraphicComponent
            Get
                GraphicComponent = m_GComp
            End Get
            Set(ByVal value As IGTGraphicComponent)
                m_GComp = value
            End Set
        End Property

        Public Property GraphicComponents() As IGTGraphicComponents Implements IGTPlacementTechnique.GraphicComponents
            Get
                GraphicComponents = m_GComps
            End Get
            Set(ByVal value As IGTGraphicComponents)
                m_GComps = value
            End Set
        End Property

        Public Sub StartPlacement(ByVal PTHelper As IGTPlacementTechniqueHelper, ByVal KeyObject As IGTKeyObject, ByVal KeyObjectCollection As IGTKeyObjects) Implements IGTPlacementTechnique.StartPlacement

            Dim oMinPoint As IGTPoint = GTClassFactory.Create(Of IGTPoint)()
            Dim oMaxPoint As IGTPoint = GTClassFactory.Create(Of IGTPoint)()

            Dim oPolygonGeometry As IGTPolygonGeometry
            Dim oTextPointGeometry As IGTTextPointGeometry

            m_oHelperPT = PTHelper

            m_oHelperPT.ConstructionAidsEnabled = GTConstructionAidsEnabledConstants.gtptConstructionAidsNone
            'm_oHelperPT.ConstructionAidDynamicsEnabled = False
            m_oHelperPT.StatusBarPromptsEnabled = False

            m_oHelperPT.StartPlacement(KeyObject, KeyObjectCollection)

            ' Use the geometry for the relative component from the GraphicComponents collection as a reference to construct the geometry for this component.
            If m_GComps.Count = 0 Then
                m_oHelperPT.AbortPlacement()
                Exit Sub
            End If

            ' Skip if detail
            If m_GComps.Item(0).DetailComponent = True Then
                m_oHelperPT.AbortPlacement()
                Exit Sub
            End If

            ' Get the polygon geometry
            oPolygonGeometry = m_GComps.Item(0).Geometry
            'Set oPolygonGeometry = m_oGraphicComponents("GC_WRKBND_P").Geometry

            ' Get the center of the PolygonGeometry
            Dim iPoint As Integer = 0
            Dim oPoint As IGTPoint = Nothing
            For Each oPoint In oPolygonGeometry.Points
                If iPoint = 0 Then
                    iPoint = 1
                    oMaxPoint.X = oPoint.X
                    oMaxPoint.Y = oPoint.Y
                    oMinPoint.X = oPoint.X
                    oMinPoint.Y = oPoint.Y
                Else
                    If oPoint.X > oMaxPoint.X Then oMaxPoint.X = oPoint.X
                    If oPoint.Y > oMaxPoint.Y Then oMaxPoint.Y = oPoint.Y
                    If oPoint.X < oMinPoint.X Then oMinPoint.X = oPoint.X
                    If oPoint.Y < oMinPoint.Y Then oMinPoint.Y = oPoint.Y
                End If
            Next oPoint

            ' Add a call to the GTPlacementTechniqueHelper CreateGeometry method to create a new geometry object.
            oTextPointGeometry = GTClassFactory.Create(Of IGTTextPointGeometry)()
            oTextPointGeometry = m_oHelperPT.CreateGeometry
            Dim oGTVector As IGTVector = GTClassFactory.Create(Of IGTVector)()
            With oTextPointGeometry
                .Alignment = GTAlignmentConstants.gtalCenterCenter
                .Normal = oGTVector
                .Rotation = 0
                oPoint.X = oMinPoint.X + ((oMaxPoint.X - oMinPoint.X) / 2)
                oPoint.Y = oMinPoint.Y + ((oMaxPoint.Y - oMinPoint.Y) / 2)
                .Origin = oPoint
            End With

            'Use the GTPlacementTechniqueHelper SetGeometry method to get it displayed on the screen.
            m_oHelperPT.SetGeometry(oTextPointGeometry)

            'To signal that the silent technique has completed, call the GTPlacementTechniqueHelper EndPlacement method.
            m_oHelperPT.EndPlacement()

        End Sub

        Public Sub KeyUp(ByVal MapWindow As IGTMapWindow, ByVal KeyCode As Integer, ByVal ShiftState As Integer, ByVal EventMode As IGTPlacementTechniqueEventMode) Implements IGTPlacementTechnique.KeyUp

        End Sub

        Public Sub MouseClick(ByVal MapWindow As IGTMapWindow, ByVal UserPoint As IGTPoint, ByVal Button As Integer, ByVal ShiftState As Integer, ByVal LocatedObjects As IGTDDCKeyObjects, ByVal EventMode As IGTPlacementTechniqueEventMode) Implements IGTPlacementTechnique.MouseClick

        End Sub

        Public Sub MouseDblClick(ByVal MapWindow As IGTMapWindow, ByVal UserPoint As IGTPoint, ByVal ShiftState As Integer, ByVal LocatedObjects As IGTDDCKeyObjects) Implements IGTPlacementTechnique.MouseDblClick

        End Sub

        Public Sub MouseMove(ByVal MapWindow As IGTMapWindow, ByVal UserPoint As IGTPoint, ByVal ShiftState As Integer, ByVal LocatedObjects As IGTDDCKeyObjects, ByVal EventMode As IGTPlacementTechniqueEventMode) Implements IGTPlacementTechnique.MouseMove

        End Sub

    End Class
End Namespace