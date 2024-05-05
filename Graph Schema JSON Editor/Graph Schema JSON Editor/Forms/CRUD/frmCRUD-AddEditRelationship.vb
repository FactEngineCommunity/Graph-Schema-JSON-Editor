Public Class frmCRUDAddEditRelationship

    Public mrRDSModel As FactEngineForServices.RDS.Model
    Public mrPGSRelationship As New JGS.RelationshipObjectType

    Private Sub TPGSRelationship1_CancelClicked() Handles TPGSRelationship1.CancelClicked

        Me.Hide()
        Me.Close()
        Me.Dispose()

    End Sub

    Private Sub frmCRUDAddEditRelationship_Load(sender As Object, e As EventArgs) Handles Me.Load

        Call Me.SetupForm

    End Sub

    Private Sub SetupForm()

        Me.TPGSRelationship1.JGSRelationship = Me.mrPGSRelationship

    End Sub

    Private Sub TPGSRelationship1_OkayClicked() Handles TPGSRelationship1.OkayClicked

        Me.DialogResult = DialogResult.OK
        Me.Hide()
        Me.Close()

    End Sub

    Private Function CheckFields() As Boolean

        Try
            Select Case True
                Case Me.TPGSRelationship1.ComboBoxNodeType1.Text.Trim = "",
                     Me.TPGSRelationship1.ComboBoxNodeType2.Text.Trim = "",
                     Me.TPGSRelationship1.TextBoxRelationshipType.Text.Trim = ""

                    Return False

            End Select

            Return True

        Catch ex As Exception
            Return False
        End Try

    End Function

End Class