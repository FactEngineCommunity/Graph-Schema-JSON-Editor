﻿Imports System.ComponentModel
Imports FactEngineForServices
Imports System.Reflection

Public Class ERDAttribute
    Inherits ERD.Attribute

    Public TreeNode As TreeNode 'The TreeNode within the Schema Viewer that represents the Property/Column/Attribute.

    <CategoryAttribute("Property/Attribute"),
     Browsable(False),
     [ReadOnly](False),
     DefaultValueAttribute(GetType(String), ""),
     DescriptionAttribute("True if the Attribute is a Parameter to a Derived Fact Type.")>
    Public Overrides Property IsDerivationParameter() As Boolean
        Get
            Return Nothing 'Because cannot be seen in the PropertiesGrid, this won't get called.
        End Get
        Set(ByVal value As Boolean)
            'Do Nothing, because cannot be seen in the PropertiesGrid, this won't get called.
        End Set
    End Property

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
                        Dim lsNewColumnName = Me.Name.Trim

                        If lasColumnName.Contains(lsNewColumnName) Then
                            MsgBox($"Sorry, a Property/Attribute with the name, {lsNewColumnName}, already exists.", MsgBoxStyle.Critical)
                            'CodeSafe
                            Me.Name = Me.Column.Name
                            Exit Sub
                        Else
                            Call Me.Column.setName(lsNewColumnName)
                        End If

                    Case Is = "DataType"

                        Me._DBDataType = Nothing
                        Call Me.Column.SetDataType(Me.DataType)

                    Case Is = "DataTypeLength"

                        Call Me.Column.SetDataTypeLength(Me.DataTypeLength)

                    Case Is = "DataTypePrecision"

                        Call Me.Column.SetDataTypePrecision(Me.DataTypePrecision)

                End Select
            End If

            If Me.TreeNode IsNot Nothing Then

                Dim lrRDSColumn As RDS.Column = Me.Column
                Dim lsDataType As String = lrRDSColumn.DBDataType

#Region "Data Type Length/Precision"
                Dim lsDataTypeLengthPrecision As String = ""

                Select Case lrRDSColumn.getMetamodelDataType
                    Case pcenumORMDataType.NumericDecimal, pcenumORMDataType.NumericFloatCustomPrecision,
                         pcenumORMDataType.NumericFloatDoublePrecision, pcenumORMDataType.NumericFloatSinglePrecision,
                         pcenumORMDataType.NumericMoney
                        ' Data types that require both length and precision
                        lsDataTypeLengthPrecision = $"({lrRDSColumn.getMetamodelDataTypeLength},{lrRDSColumn.getMetamodelDataTypePrecision})"

                    Case pcenumORMDataType.Boolean, pcenumORMDataType.LogicalTrueFalse, pcenumORMDataType.LogicalYesNo,
                         pcenumORMDataType.NumericAutoCounter, pcenumORMDataType.AutoUUID, pcenumORMDataType.NumericSignedBigInteger,
                         pcenumORMDataType.NumericSignedInteger, pcenumORMDataType.NumericSignedSmallInteger,
                         pcenumORMDataType.NumericUnsignedBigInteger, pcenumORMDataType.NumericUnsignedInteger,
                         pcenumORMDataType.NumericUnsignedSmallInteger, pcenumORMDataType.NumericUnsignedTinyInteger,
                         pcenumORMDataType.OtherObjectID, pcenumORMDataType.OtherRowID, pcenumORMDataType.RawDataFixedLength,
                         pcenumORMDataType.RawDataLargeLength, pcenumORMDataType.RawDataOLEObject, pcenumORMDataType.RawDataPicture,
                         pcenumORMDataType.RawDataVariableLength, pcenumORMDataType.TemporalAutoTimestamp,
                         pcenumORMDataType.TemporalDate, pcenumORMDataType.TemporalDateAndTime, pcenumORMDataType.TemporalTime
                        ' Data types that do not require length or precision specifications
                        lsDataTypeLengthPrecision = ""

                    Case pcenumORMDataType.TextFixedLength, pcenumORMDataType.TextLargeLength, pcenumORMDataType.TextVariableLength
                        ' Data types that require only length
                        lsDataTypeLengthPrecision = $"({lrRDSColumn.getMetamodelDataTypeLength})"

                    Case Else
                        ' Default or unknown data type
                        lsDataTypeLengthPrecision = "<Data Type Not Set>"
                End Select
#End Region

                Dim lsPropertyEmbellishment = lrRDSColumn.Name & " { ""type"": """ & lsDataType & lsDataTypeLengthPrecision & """, ""nullable"": """ & LCase(lrRDSColumn.IsNullable.ToString) & """}"

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
