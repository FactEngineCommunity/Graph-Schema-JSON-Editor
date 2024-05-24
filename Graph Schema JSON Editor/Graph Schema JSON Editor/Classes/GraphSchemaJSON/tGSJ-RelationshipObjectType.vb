Imports Newtonsoft.Json

Namespace GSJ

    ' RelationshipObjectType class used in an array of relationshipObjectTypes.
    Public Class RelationshipObjectType
        <JsonProperty("$id")>
        Public Property id As String

        <JsonProperty("type")>
        Public Property type As New RefType("RELATES_TO")

        <JsonProperty("from")>
        Public Property from As New RefType("Node Type 1")

        <JsonProperty("to")>
        Public Property [to] As New RefType("Node Type 2")

        ''' <summary>
        ''' GSJ Editor use only. Stores the RDS.Relation(ship) or the RDS.Table that represent the Relationship/EdgeType in the RDS Schema.
        '''   RDS.Relation(ships) are Foreign Keys. RDS.Tables are Many-to-Many RDS tables that represent Edge Types/Relationships.
        ''' </summary>
        Public ModelElement As Object = Nothing

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByVal asId As String,
                       ByVal asType As String,
                       ByVal asFrom As String,
                       ByVal asTo As String)
            Me.id = asId
            Me.type = New GSJ.RefType(asType)
            Me.from = New GSJ.RefType(asFrom)
            Me.to = New GSJ.RefType(asTo)
        End Sub


    End Class

End Namespace