Imports FactEngineForServices
Imports System.Reflection
Imports WeifenLuo.WinFormsUI.Docking

Public Class frmToolboxProperties
    Inherits DockContent

    Public zrSelectedObject As Object 'Used for ValueConstraints. There's a hard problem where the StringCollection editor
    'somehow sets the ValueConstraint to the wrong selected object. PropertyGrid.SelectedObject somehow changes.

    Public Function EqualsByName(ByVal other As Form) As Boolean
        If Me.Name = other.Name Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub frm_properties_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'VM-20180401-Moved the below to PropertyGrid_SelectedObjectsChanged. Leave this here to remind you.
        '  Had problems where ValueCOnstraints were being added to the wrong ValueType. This was part of the fix.
        AddHandler tStringCollectionEditor.MyFormClosed, AddressOf propertyGrid_CollectionFormClosed
        AddHandler tStringCollectionEditor.MyItemChanged, AddressOf propertyGrid_CollectionItemChanged

    End Sub

    Private Sub propertyGrid_CollectionFormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs)

        Try
            If PropertyGrid.SelectedObject IsNot Nothing Then
                Dim lrPropertyGridForm As frmToolboxProperties
                lrPropertyGridForm = frmMain.GetToolboxForm(Me.Name)
                If (Me.zrSelectedObject IsNot Nothing) And (lrPropertyGridForm IsNot Nothing) Then
                    'NB This is the only way I could get it to work. For some reason, otherwise, 
                    '  you'd get a ghost image of frmToolboxProperties/Me.zrSelectedObject and the 
                    '  ValueConstraint action would happen on the wrong ValueConstraint. e.g. It'd add a ValueConstraint to two VTs at once.
                    'Call Me.zrSelectedObject.RefreshShape()
                    Call lrPropertyGridForm.zrSelectedObject.RefreshShape()
                End If
            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,)
        End Try

    End Sub

    Private Sub propertyGrid_CollectionItemChanged(ByVal sender As Object, ByVal e As PropertyValueChangedEventArgs)

        If PropertyGrid.SelectedObject IsNot Nothing Then
            Dim lrPropertyGridForm As frmToolboxProperties
            lrPropertyGridForm = frmMain.GetToolboxForm(Me.Name)
            If (Me.zrSelectedObject IsNot Nothing) And (lrPropertyGridForm IsNot Nothing) Then
                'NB This is the only way I could get it to work. For some reason, otherwise, 
                '  you'd get a ghost image of frmToolboxProperties/Me.zrSelectedObject and the 
                '  ValueConstraint action would happen on the wrong ValueConstraint. e.g. It'd add a ValueConstraint to two VTs at once.
                If lrPropertyGridForm.zrSelectedObject IsNot Nothing Then
                    Call lrPropertyGridForm.zrSelectedObject.RefreshShape(e, Me.PropertyGrid.SelectedGridItem.Label)
                End If
            End If
        End If

    End Sub

    Public Sub SetSelectedObject(ByRef arObject As Object, Optional ByVal abSetCurrentSelection As Boolean = False)

        Try
            RemoveHandler PropertyGrid.SelectedGridItemChanged, AddressOf Me.PropertyGrid_SelectedGridItemChanged

            If arObject IsNot Nothing Then
                Me.PropertyGrid.SelectedObject = arObject
                Me.zrSelectedObject = arObject
            ElseIf Me.zrSelectedObject IsNot Nothing Then
                'Dim lrTempObject As Object = Me.zrSelectedObject
                Me.PropertyGrid.SelectedObject = Me.PropertyGrid.SelectedObject
                'Me.PropertyGrid.SelectedObject = lrTempObject
            End If
            AddHandler PropertyGrid.SelectedGridItemChanged, AddressOf Me.PropertyGrid_SelectedGridItemChanged

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try
    End Sub

    Private Sub frm_properties_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        frmMain.RightToolboxForms.RemoveAll(AddressOf Me.EqualsByName)

    End Sub

    Private Sub PropertyGrid_Enter(sender As Object, e As EventArgs) Handles PropertyGrid.Enter

        Me.PropertyGrid.Enabled = True

    End Sub

    Private Sub PropertyGrid_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles PropertyGrid.Leave

        If PropertyGrid.SelectedObject IsNot Nothing Then
            Select Case PropertyGrid.SelectedObject.GetType.ToString
                Case Is = pcenumConceptType.Model.ToString
                    '----------------
                    'Nothing to do
                    '----------------
                Case Else
                    Try
                        If PropertyGrid.SelectedObject.Page IsNot Nothing Then
                            Call PropertyGrid.SelectedObject.RefreshShape(e, Me.PropertyGrid.SelectedGridItem.Label)
                        End If
                    Catch ex As Exception

                    End Try
            End Select
        End If

    End Sub

    Private Sub PropertyGrid_PropertyValueChanged(ByVal s As Object, ByVal e As System.Windows.Forms.PropertyValueChangedEventArgs) Handles PropertyGrid.PropertyValueChanged

        Try
            Call PropertyGrid.SelectedObject.RefreshShape(e, Me.PropertyGrid.SelectedGridItem.Label)
        Catch ex As Exception
            Dim lsMessage As String
            lsMessage = "Error: frmToolboxProperties.PropertyGrid_PropertyValueChanged: "
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub


    Private Sub PropertyGrid_SelectedGridItemChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.SelectedGridItemChangedEventArgs) Handles PropertyGrid.SelectedGridItemChanged

        Select Case PropertyGrid.SelectedObject.GetType
            Case Is = GetType(FBM.Model), GetType(RDS.Model)
                '----------------
                'Nothing to do
                '----------------
            Case Else
                Try
                    Call PropertyGrid.SelectedObject.RefreshShape()
                Catch ex As Exception
                    Dim lsMessage As String
                    lsMessage = "Error: frmToolboxProperties.SelectedGridItemChanged"
                    lsMessage &= vbCrLf & vbCrLf & ex.Message
                    prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
                End Try
        End Select

    End Sub

End Class