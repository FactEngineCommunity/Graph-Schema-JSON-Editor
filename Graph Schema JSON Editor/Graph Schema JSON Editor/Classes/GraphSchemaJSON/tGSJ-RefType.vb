Imports Newtonsoft.Json

Namespace GSJ

    ' RefType used for "type", "from", and "to" properties.
    Public Class RefType
        <JsonProperty("$ref")>
        Public Property Ref As String
    End Class

End Namespace