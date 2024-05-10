Imports Newtonsoft.Json
Imports FactEngineForServices
Imports System.Reflection

Namespace GSJ

    'See Reference/Test Schemas at: https://github.com/neo4j/graph-schema-json-js-utils/tree/main/packages/graph-schema-utils/test/validation/test-schemas
    'GitHub Repository: https://github.com/neo4j/graph-schema-json-js-utils

    'GraphSchema nested within GraphSchemaRepresentation.
    Public Class GraphSchema
        <JsonProperty("nodeLabels")>
        Public Property nodeLabels As New List(Of NodeLabel)

        <JsonProperty("relationshipTypes")>
        Public Property relationshipTypes As New List(Of RelationshipType)

        <JsonProperty("nodeObjectTypes")>
        Public Property NodeObjectTypes As New List(Of NodeObjectType)

        <JsonProperty("relationshipObjectTypes")>
        Public Property RelationshipObjectTypes As New List(Of RelationshipObjectType)

        <JsonProperty("constraints")>
        Public Property Constraints As New List(Of Constraint)

        <JsonProperty("indexes")>
        Public Property Indexes As New List(Of Index)

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

    End Class

End Namespace