﻿Imports FactEngineForServices
Imports Newtonsoft.Json
Imports System.Reflection

Namespace GSJ


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

#Region "RDS (Tables)-> PGS (Nodes/Properties)"

                For Each lrRDSTable In arFBMModel.RDS.Table.FindAll(Function(x) Not x.isPGSRelation)

                    Dim lrGSJNodeLabel As New GSJ.NodeLabel(lrRDSTable.Name, lrRDSTable.FBMModelElement.GUID)
                    Me.graphSchema.nodeLabels.Add(lrGSJNodeLabel)

                    For Each lrRDSColumn In lrRDSTable.Column

                        Dim lrGSJDataType = New GSJ.DataType(lrRDSColumn.DBDataType)
                        Dim lrGSJProperty As New GSJ.Property(lrRDSColumn.Name, lrRDSColumn.Id, Not lrRDSColumn.IsMandatory, lrGSJDataType)
                        lrGSJNodeLabel.properties.Add(lrGSJProperty)

                    Next

#Region "NodeLabel for each Property Graph Label"

                    For Each lrGraphLabel In lrRDSTable.FBMModelElement.GraphLabel
                        lrGSJNodeLabel = New GSJ.NodeLabel(lrGraphLabel.Label, lrGraphLabel.Label)
                        Me.graphSchema.nodeLabels.Add(lrGSJNodeLabel)

                        For Each lrRDSColumn In lrRDSTable.Column

                            Dim lrGSJDataType = New GSJ.DataType(lrRDSColumn.DBDataType)
                            Dim lrGSJProperty As New GSJ.Property(lrRDSColumn.Name, lrRDSColumn.Id, Not lrRDSColumn.IsMandatory, lrGSJDataType)
                            lrGSJNodeLabel.properties.Add(lrGSJProperty)

                        Next
                    Next
#End Region

                Next
#End Region

#Region "RDS (Relationships)-> PGS (RelationshipTypes)"

                    For Each lrRDSRelationship In arFBMModel.RDS.Relation

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

#Region "nodeObjectTypes"


                For Each lrRDSTable In arFBMModel.RDS.Table

                    Dim lrGSNodeObjectType = New GSJ.NodeObjectType(lrRDSTable.Name)
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

#Region "relationshipObjectTypes"

                For Each lrRDSRelationship In arFBMModel.RDS.Relation

                    Dim lrGSJRelationshipObjectType = New GSJ.RelationshipObjectType(lrRDSRelationship.Id,
                                                                                     "#" & lrRDSRelationship.ResponsibleFactType.GUID,
                                                                                     "#" & lrRDSRelationship.OriginTable.Name,
                                                                                     "#" & lrRDSRelationship.DestinationTable.Name)

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

            Try

#Region "Entity Types"

#End Region

#Region "Fact Types"

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