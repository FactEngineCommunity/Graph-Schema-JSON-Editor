Public Class tPGSRelationship

    Public JGSRelationship As New JGS.RelationshipObjectType

    Public Event OkayClicked()
    Public Event CancelClicked()

    Private Sub tPGSRelationship_Load(sender As Object, e As EventArgs) Handles Me.Load

        Me.JGSRelationship.From = New JGS.RefType
        Me.JGSRelationship.From.Ref = "Node Type 1"

        Me.JGSRelationship.Type = New JGS.RefType
        Me.JGSRelationship.Type.Ref = "RELATES_TO"


        Me.JGSRelationship.To = New JGS.RefType
        Me.JGSRelationship.To.Ref = "Node Type 2"

    End Sub


    Private Sub TextBoxNode1_TextChanged(sender As Object, e As EventArgs) Handles TextBoxNodeType1.TextChanged

        Me.JGSRelationship.From.Ref = Me.TextBoxNodeType2.Text

    End Sub

    Private Sub TextBoxNodeType2_TextChanged(sender As Object, e As EventArgs) Handles TextBoxNodeType2.TextChanged

        Me.JGSRelationship.To.Ref = Me.TextBoxNodeType2.Text.Trim

    End Sub

    Private Sub TextBoxRelationshipType_TabIndexChanged(sender As Object, e As EventArgs) Handles TextBoxRelationshipType.TabIndexChanged

        Me.JGSRelationship.Type.Ref = Me.TextBoxRelationshipType.Text.Trim

    End Sub

    Private Sub ButtonOkay_Click(sender As Object, e As EventArgs) Handles ButtonOkay.Click

        RaiseEvent OkayClicked()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        RaiseEvent CancelClicked()

    End Sub

End Class
