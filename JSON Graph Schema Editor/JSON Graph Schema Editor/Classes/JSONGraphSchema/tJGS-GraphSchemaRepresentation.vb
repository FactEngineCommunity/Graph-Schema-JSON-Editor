Imports Newtonsoft.Json

Namespace JGS

    ' GraphSchemaRepresentation nested within the RootObject.
    Public Class GraphSchemaRepresentation
        <JsonProperty("version")>
        Public Property Version As String

        <JsonProperty("graphSchema")>
        Public Property GraphSchema As GraphSchema
    End Class

End Namespace