Public Class tPGSProperty

    Public GSJProperty As New GSJ.Property

    Public Event OkayClicked()
    Public Event CancelClicked()

    Private Sub tPGSRelationship_Load(sender As Object, e As EventArgs) Handles Me.Load

        Me.GSJProperty.id = System.Guid.NewGuid.ToString

        Me.GSJProperty.token = "New Property"

        Me.GSJProperty.type = New GSJ.DataType
        Me.GSJProperty.type.type = "string"

        Me.GSJProperty.nullable = True

        Me.TabIndex = 2

    End Sub


    Private Sub ComboBoxDataType_TextChanged(sender As Object, e As EventArgs) Handles ComboBoxDataType.TextChanged

        Try
            Me.GSJProperty.type = Me.ComboBoxDataType.SelectedItem.ItemData
        Catch
            Me.GSJProperty.type = New GSJ.DataType(Me.ComboBoxDataType.Text)
        End Try

    End Sub

    Private Sub TextBoxPropertyName_TextChanged(sender As Object, e As EventArgs) Handles TextBoxPropertyName.TextChanged

        Me.GSJProperty.token = Me.TextBoxPropertyName.Text.Trim

    End Sub

    Private Sub CheckBoxAllowNulls_CheckChanged(sender As Object, e As EventArgs) Handles CheckBoxAllowNulls.CheckedChanged

        Me.GSJProperty.nullable = Me.CheckBoxAllowNulls.Checked

    End Sub

    Private Sub ButtonOkay_Click(sender As Object, e As EventArgs) Handles ButtonOkay.Click

        RaiseEvent OkayClicked()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        RaiseEvent CancelClicked()

    End Sub

End Class
