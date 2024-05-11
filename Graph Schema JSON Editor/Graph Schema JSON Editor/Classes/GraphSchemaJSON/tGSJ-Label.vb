Imports Newtonsoft.Json

Namespace GSJ

    ' Label class for nested array within NodeObjectType.
    Public Class Label
        <JsonProperty("$ref")>
        Public Property ref As String

        ''' <summary>
        ''' Parameterless Constructor.
        ''' </summary>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="asRef">Use the GraphLabel from the FBM Model Element responsible for the base NodeType, OR the actual GUID of the FBM Model Element if is responsible for the base NodeType</param>
        Public Sub New(ByVal asRef As String)
            Me.ref = asRef
        End Sub


    End Class

End Namespace

