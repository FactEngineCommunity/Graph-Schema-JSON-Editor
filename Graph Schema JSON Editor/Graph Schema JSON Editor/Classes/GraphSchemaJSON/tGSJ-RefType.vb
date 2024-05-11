Imports Newtonsoft.Json

Namespace GSJ

    ' RefType used for "type", "from", and "to" properties.
    Public Class RefType
        <JsonProperty("$ref")>
        Public Property ref As String

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByVal asRef As String)
            Me.ref = asRef
        End Sub

    End Class

End Namespace