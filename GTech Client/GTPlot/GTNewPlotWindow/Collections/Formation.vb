Public Class Formation


  Private mlng_FNO As Integer
  Private mlng_FID As Integer
  Private mlng_CNO As Integer
  Private mlng_CID As Integer
  Private mstr_Description As String
  Private mbln_PlaceFormation As Boolean

  'FNO

  Public Property FNO() As Integer
    Get
      FNO = mlng_FNO
    End Get
    Set(ByVal Value As Integer)
      mlng_FNO = Value
    End Set
  End Property

  'FID

  Public Property FID() As Integer
    Get
      FID = mlng_FID
    End Get
    Set(ByVal Value As Integer)
      mlng_FID = Value
    End Set
  End Property

  'CNO

  Public Property CNO() As Short
    Get
      CNO = mlng_CNO
    End Get
    Set(ByVal Value As Short)
      mlng_CNO = Value
    End Set
  End Property

  'CID

  Public Property CID() As Integer
    Get
      CID = mlng_CID
    End Get
    Set(ByVal Value As Integer)
      mlng_CID = Value
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


  'PlaceFormation

  Public Property PlaceFormation() As Boolean
    Get
      PlaceFormation = mbln_PlaceFormation
    End Get
    Set(ByVal Value As Boolean)
      mbln_PlaceFormation = Value
    End Set
  End Property


End Class
