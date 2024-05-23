Imports FactEngineForServices
Imports System.Reflection

Public Class frmAbout

    Private Sub about_frm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Call SetupForm()

    End Sub

    Private Sub SetupForm()

        Dim ls_message As String = ""

        ls_message = "Version: v" & prApplication.ApplicationVersionNr
        ls_message.AppendLine("Boston database version: v" & prApplication.DatabaseVersionNr)

        label_versioning.Text = ls_message

        Me.LabelCategory.Text = prApplication.SoftwareCategory.ToString

        ls_message = "Written by FactEngine and Victor Morgante."

        label_details.Text = ls_message

    End Sub

    Private Sub ButtonCloseClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button_close.Click

        Me.Close()
        Me.Dispose()

    End Sub

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click

        Try
            'System.Diagnostics.Process.Start(String.Format("mailto:{0}?Subject=my report&Body={1}&Attachment={2}", "support@factengine.ai", lsText, "file://C:\Users\Viev\Desktop\Commands.txt"))
            System.Diagnostics.Process.Start(String.Format("mailto:{0}", "support@factengine.ai"))
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: You might not have a default email application setup in Windows."
            lsMessage &= vbCrLf & vbCrLf
            lsMessage &= "Email support@factengine.ai for support on Boston"
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try


    End Sub

    Private Sub LabelPromptLicenses_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelPromptLicenses.Click

        'Dim lrChildForm As New frmLicences

        'lrChildForm.Show(frmMain.DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document)

    End Sub

End Class