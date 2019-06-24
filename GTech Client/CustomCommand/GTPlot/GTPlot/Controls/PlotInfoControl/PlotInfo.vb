Public Class PlotInfo

  Private mstr_Name As String
  Private mstr_PaperName As String
  Private mstr_Type As String
  Private mstr_PaperSize As String
  Private mstr_PaperOrientation As String
  Private mstr_Scale As String

  Public Property Name() As String
    Get
      Name = mstr_Name
    End Get
    Set(ByVal Value As String)
      mstr_Name = Value
    End Set
  End Property

  Public Property PaperName() As String
    Get
      PaperName = mstr_PaperName
    End Get
    Set(ByVal Value As String)
      mstr_PaperName = Value
    End Set
  End Property

  Public Property Type() As String
    Get
      Type = mstr_Type
    End Get
    Set(ByVal Value As String)
      mstr_Type = Value
    End Set
  End Property

  Public Property PaperSize() As String
    Get
      PaperSize = mstr_PaperSize
    End Get
    Set(ByVal Value As String)
      mstr_PaperSize = Value
    End Set
  End Property

  Public Property PaperOrientation() As String
    Get
      PaperOrientation = mstr_PaperOrientation
    End Get
    Set(ByVal Value As String)
      mstr_PaperOrientation = Value
    End Set
  End Property

  Public Property Scale() As String
    Get
      Scale = mstr_Scale
    End Get
    Set(ByVal Value As String)
      mstr_Scale = Value
    End Set
  End Property

End Class
