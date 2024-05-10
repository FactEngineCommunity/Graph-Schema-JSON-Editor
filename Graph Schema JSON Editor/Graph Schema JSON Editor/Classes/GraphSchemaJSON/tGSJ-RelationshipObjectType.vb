Imports Newtonsoft.Json

Namespace GSJ

    ' RelationshipObjectType class used in an array of relationshipObjectTypes.
    Public Class RelationshipObjectType
        <JsonProperty("$id")>
        Public Property ID As String

        <JsonProperty("type")>
        Public Property Type As New RefType

        <JsonProperty("from")>
        Public Property From As New RefType

        <JsonProperty("to")>
        Public Property [To] As New RefType
    End Class

End Namespace