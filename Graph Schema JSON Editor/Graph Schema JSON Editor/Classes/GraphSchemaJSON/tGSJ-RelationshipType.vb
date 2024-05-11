Imports Newtonsoft.Json

Namespace GSJ

    ' RelationshipType class used in an array of relationshipTypes.
    Public Class RelationshipType
        <JsonProperty("$id")>
        Public Property id As String

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
        ''' 
        ''' </summary>
        ''' <param name="asToken"></param>
        ''' <param name="asId">Use the GUID of the FBM.FactType responsible for this RelationshipType.</param>
        Public Sub New(ByVal asToken As String, ByVal asId As String)

            Me.ID = asId
            Me.Token = asToken

        End Sub

    End Class

End Namespace