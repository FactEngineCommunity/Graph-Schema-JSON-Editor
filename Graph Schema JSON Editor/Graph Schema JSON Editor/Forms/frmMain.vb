Imports FactEngineForServices
Imports System.Reflection

Public Class frmMain

    Public ToolboxForms As New List(Of WeifenLuo.WinFormsUI.Docking.DockContent)
    Public RightToolboxForms As New List(Of WeifenLuo.WinFormsUI.Docking.DockContent)

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles Me.Load

        Call Me.SetupForm()

    End Sub

    Private Sub SetupForm()

        Try
            Dim lrChildForm As New frmStartup
            lrChildForm.Show(Me.DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document)

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

    Public Function GetToolboxForm(ByVal asFormName As String) As WeifenLuo.WinFormsUI.Docking.DockContent

        Dim lrForm As WeifenLuo.WinFormsUI.Docking.DockContent

        Select Case asFormName
            Case Is = "frmToolbox", frmToolboxProperties.Name
                For Each lrForm In Me.RightToolboxForms
                    If lrForm.Name = asFormName Then
                        Return lrForm
                    End If
                Next
            Case Else
                For Each lrForm In Me.ToolboxForms
                    If lrForm.Name = asFormName Then
                        Return lrForm
                    End If
                Next
        End Select

        Return Nothing

    End Function

    Private Sub PropertiesGridToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PropertiesGridToolStripMenuItem.Click

        Try

            Dim lfrmPropertiesGrid As New frmToolboxProperties

            lfrmPropertiesGrid.Show(Me.DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockRight)

            Me.RightToolboxForms.AddUnique(lfrmPropertiesGrid)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub
End Class