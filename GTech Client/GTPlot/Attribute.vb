Public Class Attribute

  Public Enum CellType
    TextBox
    ComboBox
    CheckBox
    MultiCheckBox
    ContainerControl
    TreeView
    Group
  End Enum

  Public Enum CheckBoxValue
    Unchecked = 0
    Checked = 1
  End Enum

  Public Enum MultiCheckBoxValue
    Neutral = -1
    Unchecked = 0
    Checked = 1
  End Enum


  Dim m_sG3E_FIELD As String
  Dim m_sG3E_USERNAME As String
  Dim m_sG3E_FORMAT As String
  Dim m_iG3E_PRECISION As Integer
  Dim m_iG3E_PNO As Integer
  Dim m_sG3E_ADDITIONALREFFIELDS As String
  Dim m_sG3E_PICKLISTTABLE As String
  Dim m_sG3E_KEYFIELD As String
  Dim m_sG3E_VALUEFIELD As String
  Dim m_sG3E_FILTER As String
  Dim m_sG3E_ADDITIONALFIELDS As String
  Dim m_sG3E_NAME As String
  Dim m_iG3E_REQUIRED As Integer
  Dim m_sG3E_ORDERBYKEY As String
  Dim m_iG3E_PUBLISH As Integer

  Dim m_sDATA_DEFAULT As String

  Dim m_sVALUE_TYPE As CellType
  Dim m_sVALUE As String
  Dim m_sVALUE_CheckBox As String
  Dim m_sVALUE_MultiCheckBox As String
  Dim m_sVALUE_LIST As Collection
  Dim m_oContainerControl As ContainerControl
  Dim m_oTreeView As TreeView
  Dim m_bValueReadOnly As Boolean = False


  Public Property VALUE_CheckBox() As CheckBoxValue
    Get
      VALUE_CheckBox = m_sVALUE_CheckBox
    End Get
    Set(ByVal Value As CheckBoxValue)
      m_sVALUE_CheckBox = Value
    End Set
  End Property

  Public Property VALUE_MultiCheckBox() As MultiCheckBoxValue
    Get
      VALUE_MultiCheckBox = m_sVALUE_MultiCheckBox
    End Get
    Set(ByVal Value As MultiCheckBoxValue)
      m_sVALUE_MultiCheckBox = Value
    End Set
  End Property

  Public Property ValueReadOnly() As Boolean
    Get
      ValueReadOnly = m_bValueReadOnly
    End Get
    Set(ByVal Value As Boolean)
      m_bValueReadOnly = Value
    End Set
  End Property

  Public Property G3E_FIELD() As String
    Get
      G3E_FIELD = m_sG3E_FIELD
    End Get
    Set(ByVal Value As String)
      m_sG3E_FIELD = Value
    End Set
  End Property

  Public Property G3E_USERNAME() As String
    Get
      G3E_USERNAME = m_sG3E_USERNAME
    End Get
    Set(ByVal Value As String)
      m_sG3E_USERNAME = Value
    End Set
  End Property

  Public Property G3E_FORMAT() As String
    Get
      G3E_FORMAT = m_sG3E_FORMAT
    End Get
    Set(ByVal Value As String)
      m_sG3E_FORMAT = Value
    End Set
  End Property

  Public Property G3E_PRECISION() As Integer
    Get
      G3E_PRECISION = m_iG3E_PRECISION
    End Get
    Set(ByVal Value As Integer)
      m_iG3E_PRECISION = Value
    End Set
  End Property

  Public Property G3E_PNO() As Integer
    Get
      G3E_PNO = m_iG3E_PNO
    End Get
    Set(ByVal Value As Integer)
      m_iG3E_PNO = Value
    End Set
  End Property

  Public Property G3E_ADDITIONALREFFIELDS() As String
    Get
      G3E_ADDITIONALREFFIELDS = m_sG3E_ADDITIONALREFFIELDS
    End Get
    Set(ByVal Value As String)
      m_sG3E_ADDITIONALREFFIELDS = Value
    End Set
  End Property

  Public Property G3E_PICKLISTTABLE() As String
    Get
      G3E_PICKLISTTABLE = m_sG3E_PICKLISTTABLE
    End Get
    Set(ByVal Value As String)
      m_sG3E_PICKLISTTABLE = Value
    End Set
  End Property

  Public Property G3E_KEYFIELD() As String
    Get
      G3E_KEYFIELD = m_sG3E_KEYFIELD
    End Get
    Set(ByVal Value As String)
      m_sG3E_KEYFIELD = Value
    End Set
  End Property

  Public Property G3E_VALUEFIELD() As String
    Get
      G3E_VALUEFIELD = m_sG3E_VALUEFIELD
    End Get
    Set(ByVal Value As String)
      m_sG3E_VALUEFIELD = Value
    End Set
  End Property

  Public Property G3E_FILTER() As String
    Get
      G3E_FILTER = m_sG3E_FILTER
    End Get
    Set(ByVal Value As String)
      m_sG3E_FILTER = Value
    End Set
  End Property

  Public Property G3E_ADDITIONALFIELDS() As String
    Get
      G3E_ADDITIONALFIELDS = m_sG3E_ADDITIONALFIELDS
    End Get
    Set(ByVal Value As String)
      m_sG3E_ADDITIONALFIELDS = Value
    End Set
  End Property

  Public Property G3E_NAME() As String
    Get
      G3E_NAME = m_sG3E_NAME
    End Get
    Set(ByVal Value As String)
      m_sG3E_NAME = Value
    End Set
  End Property

  Public Property G3E_REQUIRED() As Integer
    Get
      G3E_REQUIRED = m_iG3E_REQUIRED
    End Get
    Set(ByVal Value As Integer)
      m_iG3E_REQUIRED = Value
    End Set
  End Property

  Public Property G3E_ORDERBYKEY() As String
    Get
      G3E_ORDERBYKEY = m_sG3E_ORDERBYKEY
    End Get
    Set(ByVal Value As String)
      m_sG3E_ORDERBYKEY = Value
    End Set
  End Property

  Public Property G3E_PUBLISH() As Integer
    Get
      G3E_PUBLISH = m_iG3E_PUBLISH
    End Get
    Set(ByVal Value As Integer)
      m_iG3E_PUBLISH = Value
    End Set
  End Property

  Public Property DATA_DEFAULT() As String
    Get
      DATA_DEFAULT = m_sDATA_DEFAULT
    End Get
    Set(ByVal Value As String)
      m_sDATA_DEFAULT = Value
    End Set
  End Property


  Public Property VALUE_TYPE() As CellType
    Get
      VALUE_TYPE = m_sVALUE_TYPE
    End Get
    Set(ByVal Value As CellType)
      m_sVALUE_TYPE = Value
    End Set
  End Property

  Public Property VALUE() As String
    Get
      VALUE = m_sVALUE
    End Get
    Set(ByVal Value As String)
      m_sVALUE = Value
    End Set
  End Property

  Public Property VALUE_LIST() As Collection
    Get
      VALUE_LIST = m_sVALUE_LIST
    End Get
    Set(ByVal Value As Collection)
      m_sVALUE_LIST = Value
    End Set
  End Property

  Public Property ContainerControl() As ContainerControl
    Get
      Return m_oContainerControl
    End Get
    Set(ByVal Value As ContainerControl)
      m_oContainerControl = Value
    End Set
  End Property

  Public Property TreeView() As TreeView
    Get
      Return m_oTreeView
    End Get
    Set(ByVal Value As TreeView)
      m_oTreeView = Value
    End Set
  End Property

  Public Sub New()
    m_sVALUE_LIST = New Collection
  End Sub
End Class
