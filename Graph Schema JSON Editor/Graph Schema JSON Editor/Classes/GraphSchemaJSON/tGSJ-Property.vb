Imports Newtonsoft.Json

Namespace GSJ

    ' PropertyTypesOneOf class representing an array or object type based on the schema definition.
    Public Class [Property]

        <JsonProperty("$id")>
        Public Property id As String

        <JsonProperty("token")>
        Public Property token As String

        <JsonProperty("nullable")>
        Public Property nullable As Boolean

        <JsonProperty("type")>
        Public Property [type] As DataType

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByVal asPropertyName As String, ByVal abIsNullable As Boolean, ByVal arGSJDatatype As GSJ.DataType)

            Me.token = asPropertyName
            Me.nullable = abIsNullable
            Me.type = arGSJDatatype

        End Sub

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