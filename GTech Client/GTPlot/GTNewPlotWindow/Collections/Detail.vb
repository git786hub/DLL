Public Class Detail

  Private mlng_FNO As Integer
  Private mlng_FID As Integer
  Private mlng_CNO As Integer
  Private mlng_CID As Integer
  Private mlng_DetailID As Integer
  Private mstr_DetailName As String
  Private mstr_FeatureLabel As String

  Private mbln_HasFormations As Boolean
  Private mcol_Formations As New Collection


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

  'DetailID

  Public Property DetailID() As Integer
    Get
      DetailID = mlng_DetailID
    End Get
    Set(ByVal Value As Integer)
      mlng_DetailID = Value
    End Set
  End Property

  'DetailName

  Public Property DetailName() As String
    Get
      DetailName = mstr_DetailName
    End Get
    Set(ByVal Value As String)
      mstr_DetailName = Value
    End Set
  End Property

  'FeatureLabel

  Public Property FeatureLabel() As String
    Get
      FeatureLabel = mstr_FeatureLabel
    End Get
    Set(ByVal Value As String)
      mstr_FeatureLabel = Value
    End Set
  End Property

  'Formations

  Public Property Formations() As Collection
    Get
      Formations = mcol_Formations
    End Get
    Set(ByVal Value As Collection)
      mcol_Formations = Value
    End Set
  End Property

End Class
