Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Windows.Forms
Imports Intergraph.GTechnology.API
Imports Intergraph.GTechnology.Interfaces
Imports System.ComponentModel
Imports System.Globalization

Public Class NamedPlot

  Inherits MapWindow

  Private m_dSheet_Inset As Double
  Private m_iSheet_StyleNo As Integer
  Private m_dSheetHeigh As Single
  Private m_dSheetWidth As Single
  Private m_dSheetID As Long
  Private m_DRI_ID As Long

  Private mbln_HasFormations As Boolean
  Private mbln_PlaceFormations As Boolean
  Private mcol_Formations As New Collection
  Private mcol_DetailFeatureGroups As New Collection

  Private mbln_InsertActiveMapWindow As Boolean

  Private m_StyleSubstitution As String
  Private m_LegendOverrides As Collection


  Public Property LegendOverrides() As Collection
    Get
      LegendOverrides = m_LegendOverrides
    End Get
    Set(ByVal Value As Collection)
      m_LegendOverrides = Value
    End Set
  End Property

  Public Property StyleSubstitution() As String
    Get
      StyleSubstitution = m_StyleSubstitution
    End Get
    Set(ByVal value As String)
      m_StyleSubstitution = value
    End Set
  End Property

  < _
  BrowsableAttribute(False) _
  > _
  Public Property SheetInset() As Double
    Get
      SheetInset = m_dSheet_Inset
    End Get
    Set(ByVal value As Double)
      m_dSheet_Inset = value
    End Set
  End Property

  < _
  BrowsableAttribute(False) _
  > _
  Public Property SheetStyleNo() As Integer
    Get
      SheetStyleNo = m_iSheet_StyleNo
    End Get
    Set(ByVal value As Integer)
      m_iSheet_StyleNo = value
    End Set
  End Property

  < _
  BrowsableAttribute(False) _
  > _
  Public Property SheetHeigh() As Double
    Get
      SheetHeigh = m_dSheetHeigh
    End Get
    Set(ByVal value As Double)
      m_dSheetHeigh = value
    End Set
  End Property

  < _
  BrowsableAttribute(False) _
  > _
  Public Property SheetWidth() As Double
    Get
      SheetWidth = m_dSheetWidth
    End Get
    Set(ByVal value As Double)
      m_dSheetWidth = value
    End Set
  End Property

  < _
  BrowsableAttribute(False) _
  > _
  Public Property SheetID() As Long
    Get
      SheetID = m_dSheetID
    End Get
    Set(ByVal value As Long)
      m_dSheetID = value
    End Set
  End Property

  < _
  BrowsableAttribute(False) _
  > _
  Public Property DRI_ID() As Long
    Get
      DRI_ID = m_DRI_ID
    End Get
    Set(ByVal value As Long)
      m_DRI_ID = value
    End Set
  End Property

  < _
  BrowsableAttribute(False) _
  > _
  Public Property HasFormations() As Boolean
    Get
      HasFormations = mbln_HasFormations
    End Get
    Set(ByVal Value As Boolean)
      mbln_HasFormations = Value
    End Set
  End Property

  < _
  BrowsableAttribute(False) _
  > _
  Public Property PlaceFormations() As Boolean
    Get
      PlaceFormations = mbln_PlaceFormations
    End Get
    Set(ByVal Value As Boolean)
      mbln_PlaceFormations = Value
    End Set
  End Property

  < _
  BrowsableAttribute(False) _
  > _
  Public Property Formations() As Collection
    Get
      Formations = mcol_Formations
    End Get
    Set(ByVal Value As Collection)
      mcol_Formations = Value
    End Set
  End Property

  < _
  BrowsableAttribute(False) _
  > _
  Public Property DetailFeatureGroups() As Collection
    Get
      DetailFeatureGroups = mcol_DetailFeatureGroups
    End Get
    Set(ByVal Value As Collection)
      mcol_DetailFeatureGroups = Value
    End Set
  End Property

  < _
  BrowsableAttribute(False) _
  > _
  Public Property InsertActiveMapWindow() As Boolean
    Get
      InsertActiveMapWindow = mbln_InsertActiveMapWindow
    End Get
    Set(ByVal Value As Boolean)
      mbln_InsertActiveMapWindow = Value
    End Set
  End Property

End Class
