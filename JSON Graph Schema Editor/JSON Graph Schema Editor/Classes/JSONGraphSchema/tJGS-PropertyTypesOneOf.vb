Imports Newtonsoft.Json

Namespace JGS

    ' PropertyTypesOneOf class representing an array or object type based on the schema definition.
    Public Class PropertyTypesOneOf
        <JsonProperty("$ref")>
        Public Property Ref As String
    End Class

End Namespace