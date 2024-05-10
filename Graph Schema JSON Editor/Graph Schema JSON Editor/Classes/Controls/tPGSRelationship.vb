Public Class tPGSRelationship

    Public GSJRelationship As New GSJ.RelationshipObjectType

    Public Event OkayClicked()
    Public Event CancelClicked()

    Private Sub tPGSRelationship_Load(sender As Object, e As EventArgs) Handles Me.Load

        Me.GSJRelationship.From = New GSJ.RefType
        Me.GSJRelationship.From.Ref = GSJRelationship.From.Ref

        Me.GSJRelationship.Type = New GSJ.RefType
        Me.GSJRelationship.Type.Ref = GSJRelationship.Type.Ref


        Me.GSJRelationship.To = New GSJ.RefType
        Me.GSJRelationship.To.Ref = GSJRelationship.To.Ref

        Me.TabIndex = 2

    End Sub


    Private Sub ComboBoxNodeType1_TextChanged(sender As Object, e As EventArgs) Handles ComboBoxNodeType1.TextChanged

        Me.GSJRelationship.From.Ref = Me.ComboBoxNodeType1.Text

    End Sub

    Private Sub ComboxBoxNodeType2_TextChanged(sender As Object, e As EventArgs) Handles ComboBoxNodeType2.TextChanged

        Me.GSJRelationship.To.Ref = Me.ComboBoxNodeType2.Text.Trim

    End Sub

    Private Sub TextBoxRelationshipType_TabIndexChanged(sender As Object, e As EventArgs) Handles TextBoxRelationshipType.TextChanged

        Me.GSJRelationship.Type.Ref = Me.TextBoxRelationshipType.Text.Trim

    End Sub

    Private Sub ButtonOkay_Click(sender As Object, e As EventArgs) Handles ButtonOkay.Click

        RaiseEvent OkayClicked()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        RaiseEvent CancelClicked()

    End Sub

End Class
