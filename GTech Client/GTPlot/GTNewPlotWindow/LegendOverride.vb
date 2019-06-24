Public Class LegendOverride

  Private m_iGroup_Id As Integer
  Private m_sUsername As String
  Private m_bUser_Option As Boolean
  Private m_iUser_Option_Default As Integer
  Private m_iUser_Option_Value As Integer

  Public Property Group_Id() As Integer
    Get
      Group_Id = m_iGroup_Id
    End Get
    Set(ByVal value As Integer)
      m_iGroup_Id = value
    End Set
  End Property

  Public Property Username() As String
    Get
      Username = m_sUsername
    End Get
    Set(ByVal value As String)
      m_sUsername = value
    End Set
  End Property

  Public Property User_Option() As Boolean
    Get
      User_Option = m_bUser_Option
    End Get
    Set(ByVal value As Boolean)
      m_bUser_Option = value
    End Set
  End Property

  Public Property User_Option_Default() As Integer
    Get
      User_Option_Default = m_iUser_Option_Default
    End Get
    Set(ByVal value As Integer)
      m_iUser_Option_Default = value
      m_iUser_Option_Value = value
    End Set
  End Property

  Public Property User_Option_Value() As Integer
    Get
      User_Option_Value = m_iUser_Option_Value
    End Get
    Set(ByVal value As Integer)
      m_iUser_Option_Value = value
    End Set
  End Property

End Class
