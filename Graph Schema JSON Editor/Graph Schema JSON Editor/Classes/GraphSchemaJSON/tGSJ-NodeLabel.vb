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

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="asToken"></param>
        ''' <param name="asId">Use the GUID of the FBMModelElement of the RDS.Table responsible for this NodeLabel.</param>
        Public Sub New(ByVal asToken As String, ByVal asId As String)

            Me.id = asId
            Me.token = asToken

        End Sub

    End Class

End Namespace