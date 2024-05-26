Imports System.ComponentModel
Imports FactEngineForServices
Imports System.Reflection

Public Class ERDAttribute
    Inherits ERD.Attribute


    Public TreeNode As TreeNode 'The TreeNode within the Schema Viewer that represents the Property/Column/Attribute.

    Public Sub New(ByRef arRDSColumn As RDS.Column)

        Try
            Me.Column = arRDSColumn
            Me.Name = arRDSColumn.Name

            Me.DataType = arRDSColumn.getMetamodelDataType
            Me.DataTypeLength = arRDSColumn.getMetamodelDataTypeLength
            Me.DataTypePrecision = arRDSColumn.getMetamodelDataTypePrecision
            Me.Model = arRDSColumn.Model.Model
            Me.Id = arRDSColumn.Id
            Me.Entity = Nothing
            Me.AttributeName = arRDSColumn.Name
            Me.ResponsibleRole = arRDSColumn.Role
            Me.ActiveRole = arRDSColumn.ActiveRole
            Me.ResponsibleFactType = Me.ResponsibleRole.FactType
            Me.Mandatory = arRDSColumn.IsMandatory
            'Me.OrdinalPosition = arRDSColumn.OrdinalPosition
            Me.PartOfPrimaryKey = arRDSColumn.isPartOfPrimaryKey
            Me.IsDerivationParameter = arRDSColumn.IsDerivationParameter
            Me.Page = Me.Page

            Me.Column = arRDSColumn
            Me.SupertypeColumn = arRDSColumn.SupertypeColumn
            Me.DBName = arRDSColumn.DBName 'ActiveRole.JoinedORMObject.DBName

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Public Sub RefreshShape(Optional ByVal aoChangedPropertyItem As PropertyValueChangedEventArgs = Nothing,
                            Optional ByVal asSelectedGridItemLabel As String = "")

        Try
            '------------------------------------------------
            'Set the values in the underlying RDS.Relation
            '------------------------------------------------
            If aoChangedPropertyItem IsNot Nothing Then
                Select Case aoChangedPropertyItem.ChangedItem.PropertyDescriptor.Name

                    Case Is = "Name"

                        'Check to see if the New name already exists
                        Dim lasColumnName = Me.Column.Table.Column.FindAll(Function(x) x.Name <> Me.Name).Select(Function(x) x.Name)
                        Dim lsNewColumnName = aoChangedPropertyItem.ChangedItem.Value.ToString.Trim

                        If lasColumnName.Contains(lsNewColumnName) Then
                            MsgBox($"Sorry, a Property/Attribute with the name, {lsNewColumnName}, already exists.", MsgBoxStyle.Critical)
                            'CodeSafe
                            Me.Name = Me.Column.Name
                            Exit Sub
                        Else
                            Call Me.Column.setName(lsNewColumnName)
                        End If

                End Select
            End If

            If Me.TreeNode IsNot Nothing Then

                Dim lrRDSColumn As RDS.Column = Me.Column

                Dim lsPropertyEmbellishment = lrRDSColumn.Name & " { ""type"": """ & If(lrRDSColumn.DataType Is Nothing, "string", lrRDSColumn.DataType.DataType) & """, ""nullable"": """ & LCase(lrRDSColumn.IsNullable.ToString) & """}"

                Me.TreeNode.Text = lsPropertyEmbellishment

            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

End Class
