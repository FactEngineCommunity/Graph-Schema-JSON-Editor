Imports Newtonsoft.Json

Namespace GSJ

    ' RelationshipType class used in an array of relationshipTypes.
    Public Class RelationshipType
        <JsonProperty("$id")>
        Public Property ID As String

        <JsonProperty("token")>
        Public Property Token As String

        <JsonProperty("properties")>
        Public Property Properties As GSJ.Property

    End Class

End Namespace