' For this example, the Attribute class has no members.
Option Strict Off
Option Explicit On

'Friend Class Attribute
'End Class

Public Class Attributes

  Implements System.Collections.IEnumerable

  'local variable to hold collection
  Private mCol As Collection

  Public Sub Add(ByVal oAttribute As Attribute)
    mCol.Add(oAttribute, oAttribute.G3E_USERNAME)
  End Sub

  Public Function Add(ByVal sName As String, ByVal sValue As String, ByVal bReadonly As Boolean, ByVal eCellType As Attribute.CellType, ByVal ParamArray argList() As String) As Attribute

    Dim oAttribute As New Attribute

    With oAttribute
      .G3E_USERNAME = sName
      .VALUE = sValue
      .VALUE_TYPE = eCellType
      .ValueReadOnly = bReadonly
      For Each sItem As String In argList
        .VALUE_LIST.Add(sItem)
      Next
    End With
    mCol.Add(oAttribute, oAttribute.G3E_USERNAME)

    'return the object created
    Add = oAttribute

    oAttribute = Nothing
  End Function

  Public Function Add(ByVal sGroupName As String) As Attribute

    Dim oAttribute As New Attribute

    With oAttribute
      .G3E_USERNAME = sGroupName
      .VALUE_TYPE = Attribute.CellType.Group
      .ValueReadOnly = True
    End With
    mCol.Add(oAttribute, oAttribute.G3E_USERNAME)

    'return the object created
    Add = oAttribute

    oAttribute = Nothing
  End Function

  Public Function Add(ByVal sCheckBoxName As String, ByVal sValue As Attribute.CheckBoxValue, Optional ByVal bReadonly As Boolean = False) As Attribute

    Dim oAttribute As New Attribute

    With oAttribute
      .G3E_USERNAME = sCheckBoxName
      .VALUE = sValue
      .VALUE_TYPE = Attribute.CellType.CheckBox
      .ValueReadOnly = bReadonly
    End With
    mCol.Add(oAttribute, oAttribute.G3E_USERNAME)

    'return the object created
    Add = oAttribute

    oAttribute = Nothing
  End Function

  Public Function Add(ByVal sCheckBoxName As String, ByVal sValue As Attribute.MultiCheckBoxValue, Optional ByVal bReadonly As Boolean = False) As Attribute

    Dim oAttribute As New Attribute

    With oAttribute
      .G3E_USERNAME = sCheckBoxName
      .VALUE = sValue
      .VALUE_TYPE = Attribute.CellType.MultiCheckBox
      .ValueReadOnly = bReadonly
    End With
    mCol.Add(oAttribute, oAttribute.G3E_USERNAME)

    'return the object created
    Add = oAttribute

    oAttribute = Nothing
  End Function

  'Private Function Add(Optional ByRef sKey As String = "") As Attribute
  '  'create a new object
  '  Dim objNewMember As Attribute
  '  objNewMember = New Attribute

  '  'set the properties passed into the method
  '  If Len(sKey) = 0 Then
  '    mCol.Add(objNewMember)
  '  Else
  '    objNewMember.G3E_USERNAME = sKey
  '    mCol.Add(objNewMember, sKey)
  '  End If
  '  'return the object created

  '  Add = objNewMember

  '  objNewMember = Nothing
  'End Function

  Default Public ReadOnly Property Item(ByVal vntIndexKey As Object) As Attribute
    Get
      'used when referencing an element in the collection
      'vntIndexKey contains either the Index or Key to the collection,
      'this is why it is declared as a Variant
      'Syntax: Set foo = x.Item(xyz) or Set foo = x.Item(5)
      Try
        Item = mCol.Item(vntIndexKey)
      Catch ex As Exception
        Item = Nothing

      End Try
    End Get
  End Property

  Public ReadOnly Property Count() As Integer
    Get
      'used when retrieving the number of elements in the
      'collection. Syntax: Debug.Print x.Count
      Count = mCol.Count()
    End Get
  End Property

  Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
    Return mCol.GetEnumerator
  End Function

  Public Sub Remove(ByRef vntIndexKey As Object)
    'used when removing an element from the collection
    'vntIndexKey contains either the Index or Key, which is why
    'it is declared as a Variant
    'Syntax: x.Remove(xyz)
    mCol.Remove(vntIndexKey)
  End Sub

  Public Sub Clear()
    mCol.Clear()
  End Sub

  Private Sub Class_Initialize_Renamed()
    'creates the collection when this class is created
    mCol = New Collection
  End Sub
  Public Sub New()
    MyBase.New()
    Class_Initialize_Renamed()
  End Sub

  Private Sub Class_Terminate_Renamed()
    'destroys collection when this class is terminated
    mCol = Nothing
  End Sub
  Protected Overrides Sub Finalize()
    Class_Terminate_Renamed()
    MyBase.Finalize()
  End Sub

End Class
