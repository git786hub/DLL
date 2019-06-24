Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Windows.Forms
Imports Intergraph.GTechnology.API
Imports Intergraph.GTechnology.Interfaces

Namespace GTCustomCommands

  Public Class WorkspacePlots

    Implements IGTCustomCommandModeless

    Public WithEvents m_oCustomCommandHelper As IGTCustomCommandHelper
    Private WithEvents m_MyWorkspacePlotsForm As WorkspacePlotsForm

    Private m_oTransactionManager As IGTTransactionManager

    Public Sub Activate(ByVal CustomCommandHelper As IGTCustomCommandHelper) Implements IGTCustomCommandModeless.Activate

      Dim objWinWrapper As WinWrapper

      Try

        m_oCustomCommandHelper = CustomCommandHelper
        m_MyWorkspacePlotsForm = New WorkspacePlotsForm()

        objWinWrapper = New WinWrapper

        m_MyWorkspacePlotsForm.ShowDialog(objWinWrapper)

      Catch ex As Exception
        MsgBox("WorkspacePlots.Activate:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      Finally
        'cleaning
      End Try
    End Sub

    Public ReadOnly Property CanTerminate() As Boolean Implements IGTCustomCommandModeless.CanTerminate
      Get
        Try

          CanTerminate = True

        Catch ex As Exception
          MsgBox("WorkspacePlots.CanTerminate:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
        Finally
          'cleaning
        End Try
      End Get
    End Property

    Public Sub Pause() Implements IGTCustomCommandModeless.Pause
      Try

      Catch ex As Exception
        MsgBox("WorkspacePlots.Pause:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      End Try
    End Sub

    Public Sub [Resume]() Implements IGTCustomCommandModeless.Resume
      Try

      Catch ex As Exception
        MsgBox("WorkspacePlots.Resume:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      End Try
    End Sub

    Public Sub Terminate() Implements IGTCustomCommandModeless.Terminate
      Try
        Dim oGTApplication As IGTApplication
        oGTApplication = GTClassFactory.Create(Of IGTApplication)()
        oGTApplication.SetStatusBarText(GTStatusPanelConstants.gtaspcMessage, "")
        oGTApplication.RefreshWindows()
        'GC.Collect()

      Catch ex As Exception
        MsgBox("WorkspacePlots.Terminate:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)

      Finally
        m_MyWorkspacePlotsForm = Nothing
        m_oTransactionManager = Nothing

      End Try
    End Sub

    Public WriteOnly Property TransactionManager() As IGTTransactionManager Implements IGTCustomCommandModeless.TransactionManager
      Set(ByVal RHS As IGTTransactionManager)
        Try
          m_oTransactionManager = RHS

        Catch ex As Exception
          MsgBox("WorkspacePlots.TransactionManager:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
        End Try
      End Set
    End Property

    Private Sub m_oCustomCommandHelper_Activate(ByVal sender As Object, ByVal e As GTActivateEventArgs) Handles m_oCustomCommandHelper.Activate
      m_MyWorkspacePlotsForm.PopulateList()
    End Sub

    Private Sub m_oCustomCommandHelper_GainedFocus(ByVal sender As Object, ByVal e As GTGainedFocusEventArgs) Handles m_oCustomCommandHelper.GainedFocus
      m_MyWorkspacePlotsForm.PopulateList()
    End Sub


    Private Sub m_oCustomCommandHelper_KeyDown(ByVal sender As Object, ByVal e As GTKeyEventArgs) Handles m_oCustomCommandHelper.KeyDown
      If e.KeyCode = Keys.Escape Then
        m_oCustomCommandHelper.Complete()
      End If
    End Sub

    Private Sub m_MyWorkspacePlotsForm_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_MyWorkspacePlotsForm.Disposed
      Try
        m_oCustomCommandHelper.Complete()

      Catch ex As Exception
        MsgBox("WorkspacePlots.m_MyWorkspacePlotsForm_Disposed:" & vbCrLf & ex.Message, vbOKOnly + vbExclamation, ex.Source)
      Finally
      End Try
    End Sub

  End Class
End Namespace


