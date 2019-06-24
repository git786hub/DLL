Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Windows.Forms
Imports Intergraph.GTechnology.API
Imports Intergraph.GTechnology.Interfaces
Imports System.Drawing
Imports System.ComponentModel
Imports System.Globalization

Public Class MapWindow

  Private mstr_MapFrameName As String
  Private mstr_MapScale As String
  Private mstr_Legend As String
  Private mstr_ActiveMapWindow_DetailID As String
  Private mo_ActiveMapWindow_Range As IGTWorldRange
  Private mstr_ActiveMapWindow_LegendName As String
  Private mstr_ForceMapBackgroundToWhite As String

  Public Property MapFrameName() As String
    Get
      MapFrameName = mstr_MapFrameName
    End Get
    Set(ByVal Value As String)
      mstr_MapFrameName = Value
    End Set
  End Property

  < _
  BrowsableAttribute(False) _
  > _
  Public Property ActiveMapWindow_LegendName() As String
    Get
      ActiveMapWindow_LegendName = mstr_ActiveMapWindow_LegendName
    End Get
    Set(ByVal Value As String)
      mstr_ActiveMapWindow_LegendName = Value
    End Set
  End Property

  < _
  BrowsableAttribute(False) _
  > _
  Public Property ActiveMapWindow_DetailID() As String
    Get
      ActiveMapWindow_DetailID = mstr_ActiveMapWindow_DetailID
    End Get
    Set(ByVal Value As String)
      mstr_ActiveMapWindow_DetailID = Value
    End Set
  End Property

  < _
  BrowsableAttribute(False) _
  > _
  Public Property ActiveMapWindow_Range() As IGTWorldRange
    Get
      ActiveMapWindow_Range = mo_ActiveMapWindow_Range
    End Get
    Set(ByVal Value As IGTWorldRange)
      mo_ActiveMapWindow_Range = Value
    End Set
  End Property

  < _
  CategoryAttribute("Map Window"), _
  DisplayNameAttribute("Map Scale"), _
  DescriptionAttribute("Plot Boundary Map Scale"), _
  ReadOnlyAttribute(True) _
  > _
  Public Property MapScale() As String
    Get
      MapScale = mstr_MapScale
    End Get
    Set(ByVal Value As String)
      mstr_MapScale = Value
    End Set
  End Property

  < _
  CategoryAttribute("Map Window"), _
  DisplayNameAttribute("Legend"), _
  DescriptionAttribute("Defines the Legend to be used for the Map Window"), _
  DefaultValue("") _
  > _
  Public Property Legend() As String
    Get
      Legend = mstr_Legend
    End Get
    Set(ByVal Value As String)
      mstr_Legend = Value
    End Set
  End Property

  <
  CategoryAttribute("Map Window"),
  DisplayNameAttribute("Force Map Background To White"),
  DescriptionAttribute("Defines if the Map Window background color is to be forced to white"),
  DefaultValue("")
  >
  Public Property ForceMapBackgroundToWhite() As String
    Get
      ForceMapBackgroundToWhite = mstr_ForceMapBackgroundToWhite
    End Get
    Set(ByVal Value As String)
      mstr_ForceMapBackgroundToWhite = Value
    End Set
  End Property

End Class
