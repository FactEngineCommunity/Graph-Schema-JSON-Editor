Imports Newtonsoft.Json

Namespace GSJ

    ' Root class representing the entire JSON object.
    Public Class RootObject
        <JsonProperty("$schema")>
        Public Property Schema As String

        <JsonProperty("graphSchemaRepresentation")>
        Public Property GraphSchemaRepresentation As GraphSchemaRepresentation
    End Class

End Namespace
