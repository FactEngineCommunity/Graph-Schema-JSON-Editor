Imports Newtonsoft.Json

Namespace JGS

    ' Index class used in an array of indexes.
    Public Class Index
        <JsonProperty("$id")>
        Public Property ID As String

        <JsonProperty("indexType")>
        Public Property IndexType As String

        <JsonProperty("entityType")>
        Public Property EntityType As String

        <JsonProperty("nodeLabel")>
        Public Property NodeLabel As RefType

        <JsonProperty("relationshipType")>
        Public Property RelationshipType As RefType

        <JsonProperty("name")>
        Public Property Name As String

        <JsonProperty("properties")>
        Public Property Properties As List(Of RefType)
    End Class

End Namespace