Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic
Imports System.ComponentModel
Imports System.Text.RegularExpressions
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.IO
Imports System.Runtime.Serialization
Imports System.Runtime.InteropServices
Imports System.Globalization
Imports System.Dynamic
Imports System.Reflection

Public Module MyMethodExtensions

    <Extension()>
    Public Function AddUnique(Of T)(list As List(Of T), item As T)

        If Not list.Contains(item) Then list.Add(item)
        Return list
    End Function

    <Extension()>
    Public Function AppendString(ByRef asString As String, ByVal asStringExtension As String) As String

        asString = asString & asStringExtension
        Return asString

    End Function

    <Extension()>
    Public Function AppendStringInColor(ByRef aoRichtextbox As RichTextBox,
                                         ByVal asStringExtension As String,
                                         ByVal aiColor As Color,
                                         Optional ByVal abInBold As Boolean = False) As RichTextBox

        Dim liOriginalColor As Color = aoRichtextbox.SelectionColor
        aoRichtextbox.Select(aoRichtextbox.TextLength, 0)
        aoRichtextbox.SelectionColor = aiColor

        If abInBold Then
            aoRichtextbox.SelectionFont = New Font(aoRichtextbox.Font, FontStyle.Bold)
        Else
            aoRichtextbox.SelectionFont = New Font(aoRichtextbox.Font, FontStyle.Regular)
        End If
        aoRichtextbox.AppendText(asStringExtension)

        'Return back to original color
        aoRichtextbox.Select(aoRichtextbox.TextLength, 0)
        aoRichtextbox.SelectionColor = liOriginalColor

        Return aoRichtextbox

    End Function

    <Extension()>
    Public Function AppendDoubleLineBreak(ByRef asString As String, ByVal asStringExtension As String) As String

        asString = asString & vbCrLf & vbCrLf & asStringExtension
        Return asString

    End Function

    <Extension()>
    Public Function AppendLine(ByRef asString As String, ByVal asStringExtension As String) As String

        asString = asString & vbCrLf & asStringExtension
        Return asString

    End Function

    <Extension()>
    Public Function CountSubstring(ByVal asString As String, ByVal asSubstring As String) As Integer

        Return asString.Split(asSubstring).Length - 1

    End Function

    <Extension()>
    Public Function RemoveDoubleWhiteSpace(ByRef asString As String) As String

        While asString.Contains("  ")
            asString = asString.Replace("  ", " ")
        End While

        Return asString

    End Function

End Module
