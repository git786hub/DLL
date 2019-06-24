Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Windows.Forms
Imports Intergraph.GTechnology.API
Imports Intergraph.GTechnology.Interfaces
Imports System.Drawing
Imports System.ComponentModel
Imports System.Globalization

< _
DefaultPropertyAttribute("PaperSize") _
> _
Public Class PlotBoundary

  Inherits NamedPlot

  Private mo_Attributes As Collection
  Private mlng_FNO As Integer
  Private mlng_FID As Integer
  Private mlng_CNO As Integer
  Private mlng_CID As Integer
  Private mlng_DetailID As Integer
  Private mlng_DetailOwnerFNO As Integer
  Private mlng_DetailOwnerFID As Integer
  Private mstr_DetailOwnerLabel As String
  Private mstr_Name As String
  Private mstr_Type As String
  Private mstr_PaperSize As String
  Private mstr_PaperName As String
  Private mstr_PaperOrientation As String
  Private mbln_Adhoc As Boolean = False
  Private mbln_UserDefined As Boolean = False
  Private mstr_Job As String
  Private mstr_Source As String


  'Private _windowSize As Size = New Size(100, 100)
  'Private _windowFont As Font = New Font("Arial", 8, FontStyle.Regular)


  < _
  BrowsableAttribute(False) _
  > _
  Public Property Attributes() As Collection
    Get
      Attributes = mo_Attributes
    End Get
    Set(ByVal Value As Collection)
      mo_Attributes = Value
    End Set
  End Property

  < _
  CategoryAttribute("Plot Boundary"), _
  DisplayNameAttribute("Name"), _
  DescriptionAttribute("Plot Boundary Name"), _
  ReadOnlyAttribute(True) _
  > _
  Public Property Name() As String
    Get
      Name = mstr_Name
    End Get
    Set(ByVal Value As String)
      mstr_Name = Value
    End Set
  End Property


  < _
  CategoryAttribute("Plot Boundary"), _
  DisplayNameAttribute("Type"), _
  DescriptionAttribute("Plot Boundary Type - Used to drive the default Title Block and Legend used etc."), _
  ReadOnlyAttribute(True) _
  > _
  Public Property Type() As String
    Get
      Type = mstr_Type
    End Get
    Set(ByVal Value As String)
      mstr_Type = Value
    End Set
  End Property


  < _
  CategoryAttribute("Plot Boundary"), _
  DisplayNameAttribute("Paper Size"), _
  DescriptionAttribute("Plot Boundary Paper Size"), _
  ReadOnlyAttribute(True) _
  > _
  Public Property PaperSize() As String
    Get
      PaperSize = mstr_PaperSize
    End Get
    Set(ByVal Value As String)
      mstr_PaperSize = Value
    End Set
  End Property

  < _
  CategoryAttribute("Plot Boundary"), _
  DisplayNameAttribute("Paper Name"), _
  DescriptionAttribute("Plot Boundary Paper Name"), _
  ReadOnlyAttribute(True) _
  > _
  Public Property PaperName() As String
    Get
      PaperName = mstr_PaperName
    End Get
    Set(ByVal Value As String)
      mstr_PaperName = Value
    End Set
  End Property

  < _
  CategoryAttribute("Plot Boundary"), _
  DisplayNameAttribute("Paper Orientation"), _
  DescriptionAttribute("Plot Boundary Paper Orientation"), _
  ReadOnlyAttribute(True) _
  > _
  Public Property PaperOrientation() As String
    Get
      PaperOrientation = mstr_PaperOrientation
    End Get
    Set(ByVal Value As String)
      mstr_PaperOrientation = Value
    End Set
  End Property

  < _
  BrowsableAttribute(False) _
  > _
  Public Property Adhoc() As Boolean
    Get
      Adhoc = mbln_Adhoc
    End Get
    Set(ByVal Value As Boolean)
      mbln_Adhoc = Value
    End Set
  End Property

  < _
  BrowsableAttribute(False) _
  > _
  Public Property UserDefined() As Boolean
    Get
      UserDefined = mbln_UserDefined
    End Get
    Set(ByVal Value As Boolean)
      mbln_UserDefined = Value
    End Set
  End Property

  < _
  BrowsableAttribute(False) _
  > _
  Public Property FNO() As Integer
    Get
      FNO = mlng_FNO
    End Get
    Set(ByVal Value As Integer)
      mlng_FNO = Value
    End Set
  End Property

  < _
  BrowsableAttribute(False) _
  > _
  Public Property FID() As Integer
    Get
      FID = mlng_FID
    End Get
    Set(ByVal Value As Integer)
      mlng_FID = Value
    End Set
  End Property

  < _
  BrowsableAttribute(False) _
  > _
  Public Property CNO() As Short
    Get
      CNO = mlng_CNO
    End Get
    Set(ByVal Value As Short)
      mlng_CNO = Value
    End Set
  End Property

  < _
  BrowsableAttribute(False) _
  > _
  Public Property CID() As Integer
    Get
      CID = mlng_CID
    End Get
    Set(ByVal Value As Integer)
      mlng_CID = Value
    End Set
  End Property

  < _
  BrowsableAttribute(False) _
  > _
  Public Property DetailID() As Integer
    Get
      DetailID = mlng_DetailID
    End Get
    Set(ByVal Value As Integer)
      mlng_DetailID = Value
    End Set
  End Property

  < _
  CategoryAttribute("Plot Boundary"), _
  DisplayNameAttribute("Job"), _
  DescriptionAttribute("Plot Boundary Job"), _
  ReadOnlyAttribute(True) _
  > _
  Public Property Job() As String
    Get
      Job = mstr_Job
    End Get
    Set(ByVal Value As String)
      mstr_Job = Value
    End Set
  End Property

  < _
  CategoryAttribute("Plot Boundary"), _
  DisplayNameAttribute("Source"), _
  DescriptionAttribute("Plot Boundary Source"), _
  ReadOnlyAttribute(True) _
  > _
  Public Property Source() As String
    Get
      Source = mstr_Source
    End Get
    Set(ByVal Value As String)
      mstr_Source = Value
    End Set
  End Property

  < _
  BrowsableAttribute(False) _
  > _
  Public Property DetailOwnerFNO() As Integer
    Get
      DetailOwnerFNO = mlng_DetailOwnerFNO
    End Get
    Set(ByVal Value As Integer)
      mlng_DetailOwnerFNO = Value
    End Set
  End Property

  < _
  BrowsableAttribute(False) _
  > _
  Public Property DetailOwnerFID() As Integer
    Get
      DetailOwnerFID = mlng_DetailOwnerFID
    End Get
    Set(ByVal Value As Integer)
      mlng_DetailOwnerFID = Value
    End Set
  End Property

  < _
  BrowsableAttribute(False) _
  > _
  Public Property DetailOwnerLabel() As String
    Get
      DetailOwnerLabel = mstr_DetailOwnerLabel
    End Get
    Set(ByVal Value As String)
      mstr_DetailOwnerLabel = Value
    End Set
  End Property

End Class
