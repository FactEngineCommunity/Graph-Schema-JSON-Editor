﻿Imports Newtonsoft.Json

Namespace JGS

    ' Label class for nested array within NodeObjectType.
    Public Class Label
        <JsonProperty("$ref")>
        Public Property Ref As String
    End Class

End Namespace

