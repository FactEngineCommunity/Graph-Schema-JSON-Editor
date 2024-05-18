Imports System.Runtime.Serialization
Imports Newtonsoft.Json

Namespace GSJ

    ' NodeLabel class used in an array of nodeLabels.
    Public Class NodeLabel

        <JsonProperty("$id")>
        Public Property id As String

        <JsonProperty("token")>
        Public Property token As String

        <JsonProperty("properties")>
        Public Property properties As New List(Of GSJ.Property)

        <OnDeserialized>
        Private Sub OnDeserializedMethod(context As StreamingContext)
            Debug.WriteLine($"Deserialized NodeLabel with Id: {properties}")
        End Sub

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