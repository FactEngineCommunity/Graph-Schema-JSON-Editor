Public Class tPGSProperty

    Public JGSProperty As New JGS.Property

    Public Event OkayClicked()
    Public Event CancelClicked()

    Private Sub tPGSRelationship_Load(sender As Object, e As EventArgs) Handles Me.Load

        Me.JGSProperty.Ref = "NewProperty"

        Me.JGSProperty.Type = New JGS.DataType
        Me.JGSProperty.Ref = "string"


        Me.JGSProperty.Nullable = True

        Me.TabIndex = 2

    End Sub


    Private Sub ComboBoxDataType_TextChanged(sender As Object, e As EventArgs) Handles ComboBoxDataType.TextChanged

        Try
            Me.JGSProperty.Type = Me.ComboBoxDataType.SelectedItem.ItemData
        Catch
            Me.JGSProperty.Type = New JGS.DataType(Me.ComboBoxDataType.Text)
        End Try

    End Sub

    Private Sub TextBoxPropertyName_TextChanged(sender As Object, e As EventArgs) Handles TextBoxPropertyName.TextChanged

        Me.JGSProperty.Ref = Me.TextBoxPropertyName.Text.Trim

    End Sub

    Private Sub CheckBoxAllowNulls_CheckChanged(sender As Object, e As EventArgs) Handles CheckBoxAllowNulls.CheckedChanged

        Me.JGSProperty.Nullable = Me.CheckBoxAllowNulls.Checked

    End Sub

    Private Sub ButtonOkay_Click(sender As Object, e As EventArgs) Handles ButtonOkay.Click

        RaiseEvent OkayClicked()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        RaiseEvent CancelClicked()

    End Sub

End Class
