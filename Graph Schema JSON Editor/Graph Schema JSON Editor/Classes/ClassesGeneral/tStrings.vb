Imports System.Collections.Specialized

Namespace Strings
    <Serializable()>
    Public Class StringCollection
        Inherits System.Collections.Specialized.StringCollection
        Implements ICloneable

        Public Sub New()

        End Sub

        Public Overloads Function Clone() As Object Implements ICloneable.Clone
            Dim strings As New StringCollection
            Dim current As String = ""
            Dim enumerator As StringEnumerator = Me.GetEnumerator
            Do While enumerator.MoveNext
                current = enumerator.Current
                strings.Add(current)
            Loop
            Return strings
        End Function

    End Class

    Public Class StringLengthComparerDescending
        Implements IComparer(Of String)
        ' Methods
        <DebuggerNonUserCode()>
        Public Sub New()

        End Sub

        Public Function [Compare](ByVal x As String, ByVal y As String) As Integer Implements IComparer(Of String).Compare
            If (Len(x) < Len(y)) Then
                Return 1
            End If
            If (Len(x) = Len(y)) Then
                Return 0
            End If
            Return -1
        End Function

    End Class

    Public Enum enumStringComparer
        ' Fields
        [Like] = 0
    End Enum

End Namespace

