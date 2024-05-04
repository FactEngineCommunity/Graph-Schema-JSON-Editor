Imports Newtonsoft.Json

Namespace JGS

    ' RelationshipObjectType class used in an array of relationshipObjectTypes.
    Public Class RelationshipObjectType
        <JsonProperty("$id")>
        Public Property ID As String

        <JsonProperty("type")>
        Public Property Type As RefType

        <JsonProperty("from")>
        Public Property From As RefType

        <JsonProperty("to")>
        Public Property To As RefType
    End Class

End Namespace