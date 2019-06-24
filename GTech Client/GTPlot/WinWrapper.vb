Imports Intergraph.GTechnology.API
Imports Intergraph.GTechnology.Interfaces

Public Class WinWrapper
  Implements System.Windows.Forms.IWin32Window
  Overridable ReadOnly Property Handle() As System.IntPtr Implements System.Windows.Forms.IWin32Window.Handle
    Get

      Dim oGTApplication As IGTApplication
      oGTApplication = GTClassFactory.Create(Of IGTApplication)()

      'Dim iptr As New System.IntPtr(ctype(GTApplication.hWnd)
      Dim iptr As New Object
      iptr = CType(oGTApplication.hWnd, System.IntPtr)
      Return iptr
    End Get
  End Property
End Class
