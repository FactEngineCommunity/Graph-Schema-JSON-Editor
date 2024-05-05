Imports Newtonsoft.Json

Namespace JGS

    ' Constraint class used in an array of constraints.
    Public Class Constraint
        <JsonProperty("$id")>
        Public Property ID As String

        <JsonProperty("constraintType")>
        Public Property ConstraintType As String

        <JsonProperty("entityType")>
        Public Property EntityType As String

        <JsonProperty("nodeLabel")>
        Public Property NodeLabel As RefType

        <JsonProperty("relationshipType")>
        Public Property RelationshipType As RefType

        <JsonProperty("name")>
        Public Property Name As String

        <JsonProperty("properties")>
        Public Property Properties As List(Of RefType)
    End Class

End Namespace
