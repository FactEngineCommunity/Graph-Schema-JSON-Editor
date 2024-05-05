Imports Newtonsoft.Json

Namespace JGS

    ' PropertyTypesOneOf class representing an array or object type based on the schema definition.
    Public Class [Property]

        <JsonProperty("$ref")>
        Public Property Ref As String

        <JsonProperty("token")>
        Public Property Token As String

        <JsonProperty("nullable")>
        Public Property Nullable As Boolean

        <JsonProperty("type")>
        Public Property [Type] As DataType


    End Class

    Public Class DataType

        <JsonProperty("type")>
        Public Property type As String

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByVal asTypeName As String)
            Me.type = asTypeName
        End Sub

    End Class

End Namespace