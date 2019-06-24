Public Class Feature

  Private mlng_FNO As Integer
  Private mlng_FID As Integer
  Private mlng_CNO As Integer
  Private mlng_CID As Integer
  'Private mstr_FeatureLabel As String
  'Private mlng_DetailID As Integer
  'Private mstr_DetailName As String


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

  ''DetailID

  'Public Property DetailID() As Integer
  '  Get
  '    DetailID = mlng_DetailID
  '  End Get
  '  Set(ByVal Value As Integer)
  '    mlng_DetailID = Value
  '  End Set
  'End Property

  ''DetailName

  'Public Property DetailName() As String
  '  Get
  '    DetailName = mstr_DetailName
  '  End Get
  '  Set(ByVal Value As String)
  '    mstr_DetailName = Value
  '  End Set
  'End Property

  ''FeatureLabel

  'Public Property FeatureLabel() As String
  '  Get
  '    FeatureLabel = mstr_FeatureLabel
  '  End Get
  '  Set(ByVal Value As String)
  '    mstr_FeatureLabel = Value
  '  End Set
  'End Property

End Class