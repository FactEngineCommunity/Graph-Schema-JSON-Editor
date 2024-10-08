﻿Imports FactEngineForServices
Imports Newtonsoft.Json
Imports System.Reflection

Namespace GSJ

    Public Class GraphSchemaRepresentationExport

        Public Property graphSchemaRepresentation As GraphSchemaRepresentation

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arGraphSchemaRepresentation As GSJ.GraphSchemaRepresentation)
            Me.graphSchemaRepresentation = arGraphSchemaRepresentation
        End Sub

    End Class

    'See Reference/Test Schemas at: https://github.com/neo4j/graph-schema-json-js-utils/tree/main/packages/graph-schema-utils/test/validation/test-schemas
    'GitHub Repository: https://github.com/neo4j/graph-schema-json-js-utils

    ' GraphSchemaRepresentation nested within the RootObject.
    Public Class GraphSchemaRepresentation
        <JsonProperty("version")>
        Public Property Version As String = "1.0.1"

        <JsonProperty("graphSchema")>
        Public Property graphSchema As New GraphSchema


        Public Function MapFromFBMModel(ByRef arFBMModel As FBM.Model) As Boolean

            Try

#Region "nodeLabels: RDS (Tables)-> PGS (Nodes/Properties)"

                'NB See also: nodeObjectTypes (Region below)

                For Each lrRDSTable In arFBMModel.RDS.Table.FindAll(Function(x) Not x.FBMModelElement.IsCandidatePGSRelationshipNode)

                    Dim lrGSJNodeLabel As New GSJ.NodeLabel(lrRDSTable.Name, lrRDSTable.FBMModelElement.GUID)
                    Me.graphSchema.nodeLabels.Add(lrGSJNodeLabel)

                    For Each lrRDSColumn In lrRDSTable.Column

                        Dim lrGSJDataType = New GSJ.DataType(lrRDSColumn.DBDataType)
                        Dim lrGSJProperty As New GSJ.Property(lrRDSColumn.Name, lrRDSColumn.Id, Not lrRDSColumn.IsMandatory, lrGSJDataType)
                        lrGSJNodeLabel.properties.Add(lrGSJProperty)

                    Next

#Region "NodeLabel for each Property Graph Label"

                    If Not lrRDSTable.FBMModelElement.GetType = GetType(FBM.FactType) Then
                        For Each lrGraphLabel In lrRDSTable.FBMModelElement.GraphLabel
                            lrGSJNodeLabel = New GSJ.NodeLabel(lrGraphLabel.Label, lrGraphLabel.Label)
                            Me.graphSchema.nodeLabels.Add(lrGSJNodeLabel)

                            For Each lrRDSColumn In lrRDSTable.Column

                                Dim lrGSJDataType = New GSJ.DataType(lrRDSColumn.DBDataType)
                                Dim lrGSJProperty As New GSJ.Property(lrRDSColumn.Name, lrRDSColumn.Id, Not lrRDSColumn.IsMandatory, lrGSJDataType)
                                lrGSJNodeLabel.properties.Add(lrGSJProperty)

                            Next
                        Next
                    End If
#End Region

                Next
#End Region

#Region "RDS (Relationships)-> PGS (RelationshipTypes)"

                For Each lrRDSRelationship In arFBMModel.RDS.Relation.FindAll(Function(x) Not x.ResponsibleFactType.IsLinkFactType Or Not (x.ResponsibleFactType.IsLinkFactType AndAlso x.ResponsibleFactType.LinkFactTypeRole.FactType.IsCandidatePGSRelationshipNode))

                    Dim lrGSJRelationshipType = New GSJ.RelationshipType(lrRDSRelationship.ResponsibleFactType.PropertyGraphLabel, lrRDSRelationship.ResponsibleFactType.GUID)
                    Me.graphSchema.relationshipTypes.Add(lrGSJRelationshipType)

                    If lrRDSRelationship.ResponsibleFactType.isRDSTable Then

                        For Each lrRDSColumn In lrRDSRelationship.ResponsibleFactType.getCorrespondingRDSTable.Column
                            Dim lrGSJDataType = New GSJ.DataType(lrRDSColumn.DBDataType)
                            Dim lrGSJProperty As New GSJ.Property(lrRDSColumn.Name, lrRDSColumn.Id, Not lrRDSColumn.IsMandatory, lrGSJDataType)
                            lrGSJRelationshipType.properties.Add(lrGSJProperty)

                        Next

                    End If

                Next

#End Region

#Region "RDS (Table Relationships)-> PGS (RelationshipTypes)"

                For Each lrRDSTable In arFBMModel.RDS.Table.FindAll(Function(x) x.FBMModelElement.IsCandidatePGSRelationshipNode)

                    Dim lrGSJRelationshipType = New GSJ.RelationshipType(lrRDSTable.FBMModelElement.PropertyGraphLabel, lrRDSTable.FBMModelElement.GUID)
                    Me.graphSchema.relationshipTypes.Add(lrGSJRelationshipType)

                    For Each lrRDSColumn In lrRDSTable.Column.FindAll(Function(x) Not x.isPartOfPrimaryKey Or x.Role.JoinedORMObject.GetType = GetType(FBM.ValueType))
                        Dim lrGSJDataType = New GSJ.DataType(lrRDSColumn.DBDataType)
                        Dim lrGSJProperty As New GSJ.Property(lrRDSColumn.Name, lrRDSColumn.Id, Not lrRDSColumn.IsMandatory, lrGSJDataType)
                        lrGSJRelationshipType.properties.Add(lrGSJProperty)

                    Next



                Next

#End Region

#Region "nodeObjectTypes"


                For Each lrRDSTable In arFBMModel.RDS.Table.FindAll(Function(x) Not x.FBMModelElement.IsCandidatePGSRelationshipNode)

                    Dim lrGSNodeObjectType = New GSJ.NodeObjectType(lrRDSTable.Name & lrRDSTable.FBMModelElement.GUID)
                    lrGSNodeObjectType.labels.Add(New GSJ.Label("#" & lrRDSTable.Name))

                    Me.graphSchema.NodeObjectTypes.Add(lrGSNodeObjectType)

                    For Each lrPGSLabel In lrRDSTable.FBMModelElement.GraphLabel
                        lrGSNodeObjectType = New GSJ.NodeObjectType(lrPGSLabel.Label)

                        lrGSNodeObjectType.labels.Add(New GSJ.Label("#" & lrRDSTable.Name))
                        lrGSNodeObjectType.labels.Add(New GSJ.Label("#" & lrPGSLabel.Label))

                        Me.graphSchema.NodeObjectTypes.Add(lrGSNodeObjectType)
                    Next

                Next

#End Region

#Region "relationshipObjectTypes - From RDS Relationships - I.e. Foreign Key Relationships"

                For Each lrRDSRelationship In arFBMModel.RDS.Relation.FindAll(Function(x) Not x.ResponsibleFactType.IsLinkFactType Or Not (x.ResponsibleFactType.IsLinkFactType AndAlso x.ResponsibleFactType.LinkFactTypeRole.FactType.IsCandidatePGSRelationshipNode))

                    Dim lrGSJRelationshipObjectType = New GSJ.RelationshipObjectType(lrRDSRelationship.Id,
                                                                                     "#" & lrRDSRelationship.ResponsibleFactType.GUID,
                                                                                     "#" & lrRDSRelationship.OriginTable.Name,
                                                                                     "#" & lrRDSRelationship.DestinationTable.Name)

                    Me.graphSchema.RelationshipObjectTypes.Add(lrGSJRelationshipObjectType)

                Next

#End Region

#Region "relationshipObjectTypes - From RDS Tables - I.e. Many-to-Many Tables - PGS Relationships"

                For Each lrRDSTable In arFBMModel.RDS.Table.FindAll(Function(x) x.FBMModelElement.IsCandidatePGSRelationshipNode)

                    Dim larLinkedTables = (From Column In lrRDSTable.getPrimaryKeyColumns
                                           From Relation In Column.Relation
                                           Select Relation.DestinationTable).Distinct

                    Dim lrGSJRelationshipObjectType = New GSJ.RelationshipObjectType("R" & lrRDSTable.Name,
                                                                                     "#" & lrRDSTable.FBMModelElement.GUID,
                                                                                     "#" & larLinkedTables(0).FBMModelElement.Id,
                                                                                     "#" & larLinkedTables(1).FBMModelElement.Id)

                    Me.graphSchema.RelationshipObjectTypes.Add(lrGSJRelationshipObjectType)

                Next

#End Region

                Return True

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)

                Return False
            End Try

        End Function

        Public Function MapToFBMModel() As FBM.Model

            Dim lrFBMModel As New FBM.Model
            Dim lrFBMFactType As FBM.FactType

            Try
                lrFBMModel.AddCore()
                lrFBMModel.RDSCreated = True
                lrFBMModel.StoreAsXML = True

#Region "Entity Types"
                For Each lrNodeLabel In Me.graphSchema.nodeLabels

                    Dim lrFBMEntityType As New FBM.EntityType(lrFBMModel, pcenumLanguage.ORMModel, lrNodeLabel.token, Nothing, True)
                    lrFBMEntityType.GUID = lrNodeLabel.id
                    lrFBMModel.AddEntityType(lrFBMEntityType, True, False, Nothing, True, False)

                    For Each lrGSJProperty In lrNodeLabel.properties

                        Dim lrFBMValueType = New FBM.ValueType(lrFBMModel, pcenumLanguage.ORMModel, lrGSJProperty.token, True, pcenumORMDataType.DataTypeNotSet)

                        Dim lrFoundFBMValueType = lrFBMModel.GetModelObjectByName(lrFBMValueType.Id)

                        If lrFoundFBMValueType Is Nothing Then
                            lrFBMModel.AddValueType(lrFBMValueType, True, False, Nothing, True)
                            lrFoundFBMValueType = lrFBMValueType
                        End If

                        Dim larFBMModelElement As List(Of FBM.ModelObject) = {lrFBMEntityType, lrFoundFBMValueType}.ToList

                        Dim lsFBMFactTypeName = lrFBMEntityType.Id & "Has" & lrFoundFBMValueType.Id
                        lrFBMFactType = lrFBMModel.CreateFactType(lsFBMFactTypeName, larFBMModelElement, False, True, False, Nothing, False, Nothing, True, False)
                        lrFBMModel.AddFactType(lrFBMFactType, True, False, Nothing)

                        'Create FBM InternalUniquenessConstraint, which creates the Property/Column within the RDS.Model for the appropriate RDS.Table (Node Type in our Property Graph Schema)
                        lrFBMFactType.CreateInternalUniquenessConstraint({lrFBMFactType.RoleGroup(0)}.ToList, False, True, True, False, Nothing, False, False)

                    Next

                Next
#End Region

#Region "Fact Types"
                For Each lrRelationshipObjectType In Me.graphSchema.RelationshipObjectTypes

                    Dim lrFBMModelElement1 = (From ModelElement In lrFBMModel.ModelElements
                                              Where ModelElement.Id = lrRelationshipObjectType.from.ref.TrimStart("#"c)
                                              Select ModelElement).First

                    Dim lrFBMModelElement2 = (From ModelElement In lrFBMModel.ModelElements
                                              Where ModelElement.Id = lrRelationshipObjectType.to.ref.TrimStart("#"c)
                                              Select ModelElement).First

                    Dim lrGSJRelationshipType = Me.graphSchema.relationshipTypes.Find(Function(x) x.id = lrRelationshipObjectType.type.ref.TrimStart("#"c))

                    Dim lsFBMFactTypeName = lrFBMModelElement1.Id & "Has" & lrFBMModelElement2.Id

                    lrFBMFactType = lrFBMModel.CreateFactType(lsFBMFactTypeName, {lrFBMModelElement1, lrFBMModelElement2}.ToList, False, True, False, Nothing, False, Nothing, True, False)
                    lrFBMModel.AddFactType(lrFBMFactType, True, False, Nothing)

                    'Create FBM InternalUniquenessConstraint, which creates the Property/Column within the RDS.Model for the appropriate RDS.Table (Node Type in our Property Graph Schema)
                    lrFBMFactType.CreateInternalUniquenessConstraint(lrFBMFactType.RoleGroup.ToList, False, True, True, False, Nothing, False, False)

                    lrFBMFactType.Objectify(True, True, Nothing, True)

                    lrFBMFactType.GraphLabel.Add(New RDS.GraphLabel(lrFBMFactType, lrGSJRelationshipType.token))

                    Dim lrRDSTable As RDS.Table = lrFBMFactType.getCorrespondingRDSTable
                    lrRDSTable.setIsPGSRelation(True)

                    For Each loProperty In lrGSJRelationshipType.properties

                        Dim lsValueTypeName = loProperty.token

                        Dim larFBMValueType = From ValueType In lrFBMModel.ValueType
                                              Where ValueType.Id = lsValueTypeName
                                              Select ValueType

                        Dim lrFBMValueType As FBM.ValueType = Nothing
                        If larFBMValueType.Count > 0 Then
                            lrFBMValueType = larFBMValueType.First
                        Else
                            lrFBMValueType = lrFBMModel.CreateValueType(lsValueTypeName, True)
                        End If

                        Dim lsPropertyFBMFactTypeName = lrFBMFactType.Id & "Has" & lsValueTypeName
                        lsPropertyFBMFactTypeName = lrFBMModel.CreateUniqueFactTypeName(lsPropertyFBMFactTypeName, 0)
                        Dim lrPropertyFBMFactType = lrFBMModel.CreateFactType(lsPropertyFBMFactTypeName, New List(Of FBM.ModelObject) From {lrFBMFactType, lrFBMValueType}, False, True, False, Nothing, False, Nothing, True, False)
                        lrFBMModel.AddFactType(lrPropertyFBMFactType, True, False, Nothing)

                        'Create FBM InternalUniquenessConstraint, which creates the Property/Column within the RDS.Model for the appropriate RDS.Table (Node Type in our Property Graph Schema)
                        lrFBMFactType.CreateInternalUniquenessConstraint({lrPropertyFBMFactType.RoleGroup(0)}.ToList, False, True, True, False, Nothing, False, False)

                    Next

                Next
#End Region


                Return lrFBMModel

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)

                Return Nothing
            End Try

        End Function


    End Class

End Namespace