Public Class frmCRUDAddEditRelationship

    Public mrRDSModel As FactEngineForServices.RDS.Model
    Public mrGSJRelationship As New GSJ.RelationshipObjectType
    Public mbIsAdd As Boolean = False

    Public mbIsManyToOne As Boolean = False
    Public mbIsManyToMany As Boolean = True

    Private Sub TPGSRelationship1_CancelClicked() Handles TPGSRelationship1.CancelClicked

        Me.Hide()
        Me.Close()
        Me.Dispose()

    End Sub

    Private Sub frmCRUDAddEditRelationship_Load(sender As Object, e As EventArgs) Handles Me.Load

        'Automatically disables if GSJRelationship isnot Nothing
        Me.TPGSRelationship1.GSJRelationship = Me.mrGSJRelationship

        If Me.mbIsAdd Then
            Me.TPGSRelationship1.ComboBoxNodeType1.Enabled = True
            Me.TPGSRelationship1.ComboBoxNodeType2.Enabled = True
        End If


        Call Me.SetupForm

    End Sub

    Private Sub SetupForm()

        '========================================================================================
        'Populate the ComboBox items for the candidate joined Node Types (RDS.Table) of the Relationship
        For Each lrRDSTable In Me.mrRDSModel.Table

            Dim lrNewComboBoxItem = New tComboboxItem(lrRDSTable.Name, lrRDSTable.Name, lrRDSTable)
            Me.TPGSRelationship1.ComboBoxNodeType1.Items.Add(lrNewComboBoxItem)

            lrNewComboBoxItem = New tComboboxItem(lrRDSTable.Name, lrRDSTable.Name, lrRDSTable)
            Me.TPGSRelationship1.ComboBoxNodeType2.Items.Add(lrNewComboBoxItem)

            Me.TPGSRelationship1.ComboBoxNodeType1.DropDownStyle = ComboBoxStyle.DropDown
            Me.TPGSRelationship1.ComboBoxNodeType2.DropDownStyle = ComboBoxStyle.DropDown

            Me.TPGSRelationship1.ComboBoxNodeType1.SelectedIndex = 0
            Me.TPGSRelationship1.ComboBoxNodeType2.SelectedIndex = 0
        Next

    End Sub

    Private Sub TPGSRelationship1_OkayClicked() Handles TPGSRelationship1.OkayClicked

        Me.DialogResult = DialogResult.OK
        Me.Hide()
        Me.Close()

    End Sub

    Private Sub TPGSRelationship1_GSJRelationshipSet() Handles TPGSRelationship1.GSJRelationshipSet

        'Must be in Edit mode.
        Me.TPGSRelationship1.ComboBoxNodeType1.Enabled = False
        Me.TPGSRelationship1.ComboBoxNodeType2.Enabled = False

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

    Private Sub RadioButtonIsManyToMany_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonIsManyToMany.CheckedChanged

        Me.mbIsManyToMany = Me.RadioButtonIsManyToMany.Checked

    End Sub

    Private Sub RadioButtonIsManyToOne_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonIsManyToOne.CheckedChanged

        Me.mbIsManyToOne = Me.RadioButtonIsManyToOne.Checked

    End Sub

End Class