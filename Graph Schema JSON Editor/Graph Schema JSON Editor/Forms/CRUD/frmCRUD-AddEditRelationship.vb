Imports FactEngineForServices
Imports System.Reflection

Public Class frmCRUDAddEditRelationship

    Public mrRDSModel As FactEngineForServices.RDS.Model
    Public mrGSJRelationship As New GSJ.RelationshipObjectType
    Public mbIsAdd As Boolean = False

    Public mbIsManyToOne As Boolean = False
    Public mbIsManyToMany As Boolean = True

    Private Sub TPGSRelationship1_CancelClicked() Handles TPGSRelationship.CancelClicked

        Me.Hide()
        Me.Close()
        Me.Dispose()

    End Sub

    Private Sub frmCRUDAddEditRelationship_Load(sender As Object, e As EventArgs) Handles Me.Load

        Try
            'Automatically disables if GSJRelationship isnot Nothing
            Me.TPGSRelationship.GSJRelationship = Me.mrGSJRelationship

            Call Me.SetupForm()

            If Not Me.mbIsAdd Then
                Me.TPGSRelationship.ComboBoxNodeType1.Text = Me.mrGSJRelationship.from.ref
                Me.TPGSRelationship.ComboBoxNodeType2.Text = Me.mrGSJRelationship.to.ref
            End If

            Me.TPGSRelationship.ComboBoxNodeType1.Enabled = Me.mbIsAdd
            Me.TPGSRelationship.ComboBoxNodeType2.Enabled = Me.mbIsAdd

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Sub SetupForm()

        '========================================================================================
        'Populate the ComboBox items for the candidate joined Node Types (RDS.Table) of the Relationship
#Region "Comboboxes for Node Types"
        RemoveHandler TPGSRelationship.ComboBoxNodeType1.TextChanged, AddressOf TPGSRelationship.ComboBoxNodeType1_TextChanged
        RemoveHandler TPGSRelationship.ComboBoxNodeType2.TextChanged, AddressOf TPGSRelationship.ComboBoxNodeType2_TextChanged

        For Each lrRDSTable In Me.mrRDSModel.Table

            Dim lrNewComboBoxItem = New tComboboxItem(lrRDSTable.Name, lrRDSTable.Name, lrRDSTable)
            Me.TPGSRelationship.ComboBoxNodeType1.Items.Add(lrNewComboBoxItem)

            lrNewComboBoxItem = New tComboboxItem(lrRDSTable.Name, lrRDSTable.Name, lrRDSTable)
            Dim liNewIndex = Me.TPGSRelationship.ComboBoxNodeType2.Items.Add(lrNewComboBoxItem)

            Me.TPGSRelationship.ComboBoxNodeType1.DropDownStyle = ComboBoxStyle.DropDown
            Me.TPGSRelationship.ComboBoxNodeType2.DropDownStyle = ComboBoxStyle.DropDown

            Me.TPGSRelationship.ComboBoxNodeType1.SelectedIndex = 0
            Me.TPGSRelationship.ComboBoxNodeType2.SelectedIndex = 0

            If lrRDSTable.Name.Trim = Me.mrGSJRelationship.from.ref Then
                Me.TPGSRelationship.ComboBoxNodeType1.SelectedIndex = liNewIndex
            End If

            If lrRDSTable.Name.Trim = Me.mrGSJRelationship.to.ref Then
                Me.TPGSRelationship.ComboBoxNodeType2.SelectedIndex = liNewIndex
            End If
        Next

        AddHandler TPGSRelationship.ComboBoxNodeType1.TextChanged, AddressOf TPGSRelationship.ComboBoxNodeType1_TextChanged
        AddHandler TPGSRelationship.ComboBoxNodeType2.TextChanged, AddressOf TPGSRelationship.ComboBoxNodeType2_TextChanged
#End Region

    End Sub

    Private Sub TPGSRelationship1_OkayClicked() Handles TPGSRelationship.OkayClicked

        'Return results are collected by the TPGSRelationship control on this form.
        '  but CodeSafe to be sure.

        If Me.CheckFields Then

            'CodeSafe
            Me.mrGSJRelationship.from.ref = Me.TPGSRelationship.ComboBoxNodeType1.Text.Trim
            Me.mrGSJRelationship.type.ref = Me.TPGSRelationship.TextBoxRelationshipType.Text.Trim
            Me.mrGSJRelationship.to.ref = Me.TPGSRelationship.ComboBoxNodeType2.Text.Trim

            Me.DialogResult = DialogResult.OK
            Me.Hide()
            Me.Close()
        End If

    End Sub

    Private Sub TPGSRelationship1_GSJRelationshipSet() Handles TPGSRelationship.GSJRelationshipSet

        'Must be in Edit mode.
        Me.TPGSRelationship.ComboBoxNodeType1.Enabled = False
        Me.TPGSRelationship.ComboBoxNodeType2.Enabled = False

    End Sub

    Private Function CheckFields() As Boolean

        Try
            Select Case True
                Case Me.TPGSRelationship.ComboBoxNodeType1.Text.Trim = "",
                     Me.TPGSRelationship.ComboBoxNodeType2.Text.Trim = "",
                     Me.TPGSRelationship.TextBoxRelationshipType.Text.Trim = ""

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