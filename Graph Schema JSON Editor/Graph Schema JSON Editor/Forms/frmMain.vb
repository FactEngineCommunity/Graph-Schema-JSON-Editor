Imports FactEngineForServices
Imports System.Reflection

Public Class frmMain

    Public ToolboxForms As New List(Of WeifenLuo.WinFormsUI.Docking.DockContent)

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles Me.Load

        Call Me.SetupForm()

    End Sub

    Private Sub SetupForm()

        Try

            Dim lfrmSchema As New frmSchema
            lfrmSchema.Show(Me.DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockLeft)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Sub FactTypeReadingEditorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FactTypeReadingEditorToolStripMenuItem.Click

        Try
            Dim lfrmFactTypeReadingEditor As New frmToolboxORMReadingEditor

            Call lfrmFactTypeReadingEditor.Show(Me.DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockBottom)

            Me.ToolboxForms.AddUnique(lfrmFactTypeReadingEditor)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try
    End Sub
End Class