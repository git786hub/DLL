Public Class DetailFeatureGroup

  Private mstr_Name As String
  Private mstr_View As String
  Private mstr_DetailView As String
  Private mint_Det_LNO As Integer
  Private mstr_Filter As String
  Private mstr_Description As String

  Private mcol_Details As New Collection


  'Name

  Public Property Name() As String
    Get
      Name = mstr_Name
    End Get
    Set(ByVal Value As String)
      mstr_Name = Value
    End Set
  End Property

  'View

  Public Property View() As String
    Get
      View = mstr_View
    End Get
    Set(ByVal Value As String)
      mstr_View = Value
    End Set
  End Property

  'Detail View

  Public Property DetailView() As String
    Get
      DetailView = mstr_DetailView
    End Get
    Set(ByVal Value As String)
      mstr_DetailView = Value
    End Set
  End Property

  'Det_LNO

  Public Property Det_LNO() As Integer
    Get
      Det_LNO = mint_Det_LNO
    End Get
    Set(ByVal Value As Integer)
      mint_Det_LNO = Value
    End Set
  End Property

  'Filter

  Public Property Filter() As String
    Get
      Filter = mstr_Filter
    End Get
    Set(ByVal Value As String)
      mstr_Filter = Value
    End Set
  End Property

  'Description

  Public Property Description() As String
    Get
      Description = mstr_Description
    End Get
    Set(ByVal Value As String)
      mstr_Description = Value
    End Set
  End Property

  'Details

  Public Property Details() As Collection
    Get
      Details = mcol_Details
    End Get
    Set(ByVal Value As Collection)
      mcol_Details = Value
    End Set
  End Property

End Class
