Imports Newtonsoft.Json

Namespace JGS

    ' RelationshipType class used in an array of relationshipTypes.
    Public Class RelationshipType
        <JsonProperty("$id")>
        Public Property ID As String

        <JsonProperty("token")>
        Public Property Token As String

        <JsonProperty("properties")>
        Public Property Properties As JGS.Property

    End Class

End Namespace