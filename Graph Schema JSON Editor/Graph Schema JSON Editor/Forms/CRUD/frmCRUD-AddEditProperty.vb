Public Class frmCRUDAddEditProperty

    Public mrRDSModel As FactEngineForServices.RDS.Model
    Public mrPGSProperty As New GSJ.Property

    Private Sub TPGSRelationship1_CancelClicked() Handles TPGSProperty1.CancelClicked

        Me.Hide()
        Me.Close()
        Me.Dispose()

    End Sub

    Private Sub frmCRUDAddEditRelationship_Load(sender As Object, e As EventArgs) Handles Me.Load

        Call Me.SetupForm()

    End Sub

    Private Sub SetupForm()

        'Me.TPGSProperty.GSJRelationship = Me.mrPGSRelationship

    End Sub

    Private Sub TPGSRelationship1_OkayClicked() Handles TPGSProperty1.OkayClicked

        Me.DialogResult = DialogResult.OK
        Me.Hide()
        Me.Close()

    End Sub

    Private Function CheckFields() As Boolean

        Try
            'Select Case True
            '    Case Me.TPGSProperty.ComboBoxNodeType1.Text.Trim = "",
            '         Me.TPGSProperty.ComboBoxNodeType2.Text.Trim = "",
            '         Me.TPGSProperty.TextBoxRelationshipType.Text.Trim = ""

            '        Return False

            'End Select

            Return True

        Catch ex As Exception
            Return False
        End Try

    End Function

End Class