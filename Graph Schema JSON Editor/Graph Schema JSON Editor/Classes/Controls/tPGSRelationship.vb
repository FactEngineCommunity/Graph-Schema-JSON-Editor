﻿Public Class tPGSRelationship

    Private WithEvents _GSJRelationship As New GSJ.RelationshipObjectType

    Public Property GSJRelationship As GSJ.RelationshipObjectType
        Get
            Return Me._GSJRelationship
        End Get
        Set(value As GSJ.RelationshipObjectType)
            Me._GSJRelationship = value

            'NB Not the actual values that are exported when the Schema is serialised/exported. See tGSJ-GraphSchemaRepresentation.MapFromFBMModel.
            Me.ComboBoxNodeType1.Text = Me._GSJRelationship.from.ref
            Me.ComboBoxNodeType2.Text = Me._GSJRelationship.to.ref
            Me.TextBoxRelationshipType.Text = Me._GSJRelationship.type.ref

            RaiseEvent GSJRelationshipSet()
        End Set
    End Property

    Public Event OkayClicked()
    Public Event CancelClicked()
    Public Event GSJRelationshipSet()

    Private Sub tPGSRelationship_Load(sender As Object, e As EventArgs) Handles Me.Load

        RemoveHandler ComboBoxNodeType1.TextChanged, AddressOf ComboBoxNodeType1_TextChanged
        RemoveHandler ComboBoxNodeType2.TextChanged, AddressOf ComboBoxNodeType2_TextChanged

        Me.GSJRelationship.from = New GSJ.RefType
        Me.GSJRelationship.from.ref = GSJRelationship.from.ref

        Me.GSJRelationship.Type = New GSJ.RefType
        Me.GSJRelationship.Type.Ref = GSJRelationship.Type.ref

        Me.GSJRelationship.To = New GSJ.RefType
        Me.GSJRelationship.To.Ref = GSJRelationship.To.ref

        Me.TabIndex = 2

        AddHandler ComboBoxNodeType1.TextChanged, AddressOf ComboBoxNodeType1_TextChanged
        AddHandler ComboBoxNodeType2.TextChanged, AddressOf ComboBoxNodeType2_TextChanged

    End Sub

    Public Sub ComboBoxNodeType1_TextChanged(sender As Object, e As EventArgs) Handles ComboBoxNodeType1.TextChanged

        Me.GSJRelationship.from.ref = Me.ComboBoxNodeType1.Text.Trim

    End Sub

    Public Sub ComboBoxNodeType2_TextChanged(sender As Object, e As EventArgs) Handles ComboBoxNodeType2.TextChanged

        Me.GSJRelationship.to.ref = Me.ComboBoxNodeType2.Text.Trim

    End Sub

    Public Sub ComboBoxNodeType1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxNodeType1.SelectedIndexChanged

        Me.GSJRelationship.from.ref = Me.ComboBoxNodeType1.Text.Trim

    End Sub

    Public Sub ComboBoxNodeType2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxNodeType2.SelectedIndexChanged

        Me.GSJRelationship.to.ref = Me.ComboBoxNodeType2.Text.Trim

    End Sub

    Private Sub TextBoxRelationshipType_TabIndexChanged(sender As Object, e As EventArgs) Handles TextBoxRelationshipType.TextChanged

        Me.GSJRelationship.type.ref = Me.TextBoxRelationshipType.Text.Trim

    End Sub

    Private Sub ButtonOkay_Click(sender As Object, e As EventArgs) Handles ButtonOkay.Click

        RaiseEvent OkayClicked()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        RaiseEvent CancelClicked()

    End Sub

End Class
