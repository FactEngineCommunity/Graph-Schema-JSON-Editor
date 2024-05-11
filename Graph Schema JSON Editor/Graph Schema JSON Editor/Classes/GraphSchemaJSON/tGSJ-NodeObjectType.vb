﻿Imports Newtonsoft.Json

Namespace GSJ

    ' NodeObjectType class used in an array of nodeObjectTypes.
    Public Class NodeObjectType
        <JsonProperty("$id")>
        Public Property id As String

        <JsonProperty("labels")>
        Public Property labels As New List(Of Label)

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByVal asId As String)

            Me.id = asId
        End Sub


    End Class

End Namespace