Public Class tPGSRelationship

    Public JGSRelationship As New JGS.RelationshipObjectType

    Public Event OkayClicked()
    Public Event CancelClicked()

    Private Sub tPGSRelationship_Load(sender As Object, e As EventArgs) Handles Me.Load

        Me.JGSRelationship.From = New JGS.RefType
        Me.JGSRelationship.From.Ref = JGSRelationship.From.Ref

        Me.JGSRelationship.Type = New JGS.RefType
        Me.JGSRelationship.Type.Ref = JGSRelationship.Type.Ref


        Me.JGSRelationship.To = New JGS.RefType
        Me.JGSRelationship.To.Ref = JGSRelationship.To.Ref

        Me.TabIndex = 2

    End Sub


    Private Sub ComboBoxNodeType1_TextChanged(sender As Object, e As EventArgs) Handles ComboBoxNodeType1.TextChanged

        Me.JGSRelationship.From.Ref = Me.ComboBoxNodeType1.Text

    End Sub

    Private Sub ComboxBoxNodeType2_TextChanged(sender As Object, e As EventArgs) Handles ComboBoxNodeType2.TextChanged

        Me.JGSRelationship.To.Ref = Me.ComboBoxNodeType2.Text.Trim

    End Sub

    Private Sub TextBoxRelationshipType_TabIndexChanged(sender As Object, e As EventArgs) Handles TextBoxRelationshipType.TextChanged

        Me.JGSRelationship.Type.Ref = Me.TextBoxRelationshipType.Text.Trim

    End Sub

    Private Sub ButtonOkay_Click(sender As Object, e As EventArgs) Handles ButtonOkay.Click

        RaiseEvent OkayClicked()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        RaiseEvent CancelClicked()

    End Sub

End Class
