Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Windows.Forms
Imports Intergraph.GTechnology.API
Imports Intergraph.GTechnology.Interfaces

Namespace GTCustomCommands

  Public Class PrintActiveMapWindow
    
    Implements IGTCustomCommandModeless

    Public WithEvents m_oCustomCommandHelper As IGTCustomCommandHelper
    Private WithEvents m_PrintActiveMapWindowForm As PrintActiveMapWindowForm

    Private m_oTransactionManager As IGTTransactionManager


    Public Sub Activate(ByVal CustomCommandHelper As IGTCustomCommandHelper) Implements IGTCustomCommandModeless.Activate
      Try
        m_oCustomCommandHelper = CustomCommandHelper
        m_PrintActiveMapWindowForm = New PrintActiveMapWindowForm

        Dim objWinWrapper As New WinWrapper

        'Dim instance As IWin32Window
        'Dim value As IntPtr

        'instance.Handle

        'value = GTApplication.hWnd


        'instance.Handle = GTApplication.hWnd
        'Dim a As Handle

        m_PrintActiveMapWindowForm.HideDialog = True
        m_PrintActiveMapWindowForm.Show(objWinWrapper)

        m_PrintActiveMapWindowForm.PrintActiveMapWindowPrint()

      Catch ex As Exception
        MsgBox("NewPlotWindow.Activate:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
        Application.Exit()
      Finally
        'cleaning
      End Try

    End Sub

    Public ReadOnly Property CanTerminate() As Boolean Implements IGTCustomCommandModeless.CanTerminate
      Get
        Try

          CanTerminate = True

        Catch ex As Exception
          MsgBox("NewPlotWindow.CanTerminate:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
        Finally
          'cleaning
        End Try
      End Get
    End Property

    Public Sub Pause() Implements IGTCustomCommandModeless.Pause
      Try

      Catch ex As Exception
        MsgBox("NewPlotWindow.Pause:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      End Try
    End Sub

    Public Sub [Resume]() Implements IGTCustomCommandModeless.Resume
      Try

      Catch ex As Exception
        MsgBox("NewPlotWindow.[Resume]:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      End Try
    End Sub

    Public Sub Terminate() Implements IGTCustomCommandModeless.Terminate
      Dim oGTApplication As IGTApplication
      Try
        oGTApplication = GTClassFactory.Create(Of IGTApplication)()
        oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "")
        oGTApplication.RefreshWindows()
        GC.Collect()

      Catch ex As Exception
        MsgBox("NewPlotWindow.Terminate:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)

      Finally
        m_PrintActiveMapWindowForm = Nothing
        m_oTransactionManager = Nothing

      End Try
    End Sub


    'Public WriteOnly Property TransactionManager() As API.IGTTransactionManager Implements Interfaces.IGTCustomCommandModeless.TransactionManager
    Public WriteOnly Property ITransactionManager() As IGTTransactionManager Implements IGTCustomCommandModeless.TransactionManager
      Set(ByVal value As IGTTransactionManager)
        Try
          m_oTransactionManager = value

        Catch ex As Exception
          MsgBox("NewPlotWindow.ITransactionManager:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
        End Try
      End Set
    End Property

    Private Sub m_oCustomCommandHelper_KeyDown(ByVal sender As Object, ByVal e As GTKeyEventArgs) Handles m_oCustomCommandHelper.KeyDown
      If e.KeyCode = Keys.Escape Then
        m_oCustomCommandHelper.Complete()
      End If
    End Sub

    Private Sub m_PrintActiveMapWindowForm_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_PrintActiveMapWindowForm.Disposed
      Try
        m_oCustomCommandHelper.Complete()
      Catch ex As Exception
        MsgBox("NewPlotWindow.m_MyWorkspacePlotsForm_Disposed:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      Finally
      End Try
    End Sub

  End Class
End Namespace
