Imports Newtonsoft.Json

Namespace JGS

    ' GraphSchema nested within GraphSchemaRepresentation.
    Public Class GraphSchema
        <JsonProperty("nodeLabels")>
        Public Property NodeLabels As List(Of NodeLabel)

        <JsonProperty("relationshipTypes")>
        Public Property RelationshipTypes As List(Of RelationshipType)

        <JsonProperty("nodeObjectTypes")>
        Public Property NodeObjectTypes As List(Of NodeObjectType)

        <JsonProperty("relationshipObjectTypes")>
        Public Property RelationshipObjectTypes As List(Of RelationshipObjectType)

        <JsonProperty("constraints")>
        Public Property Constraints As List(Of Constraint)

        <JsonProperty("indexes")>
        Public Property Indexes As List(Of Index)
    End Class


End Namespace