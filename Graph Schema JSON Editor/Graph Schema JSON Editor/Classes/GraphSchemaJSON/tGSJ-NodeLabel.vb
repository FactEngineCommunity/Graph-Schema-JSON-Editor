Imports Newtonsoft.Json

Namespace GSJ

    ' NodeLabel class used in an array of nodeLabels.
    Public Class NodeLabel
        <JsonProperty("$id")>
        Public Property id As String = System.Guid.NewGuid.ToString

        <JsonProperty("token")>
        Public Property token As String

        <JsonProperty("properties")>
        Public Property properties As New List(Of GSJ.Property)

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByVal asToken As String)

            Me.token = asToken

        End Sub

    End Class

End Namespace