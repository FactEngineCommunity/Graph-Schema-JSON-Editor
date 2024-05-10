Imports Newtonsoft.Json

Namespace GSJ

    ' NodeObjectType class used in an array of nodeObjectTypes.
    Public Class NodeObjectType
        <JsonProperty("$id")>
        Public Property ID As String

        <JsonProperty("labels")>
        Public Property Labels As List(Of Label)
    End Class

End Namespace