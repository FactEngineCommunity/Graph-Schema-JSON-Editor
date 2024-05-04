Imports Newtonsoft.Json

Namespace JGS

    ' NodeLabel class used in an array of nodeLabels.
    Public Class NodeLabel
        <JsonProperty("$id")>
        Public Property ID As String

        <JsonProperty("token")>
        Public Property Token As String

        <JsonProperty("properties")>
        Public Property Properties As New List(Of JGS.Property)

    End Class

End Namespace