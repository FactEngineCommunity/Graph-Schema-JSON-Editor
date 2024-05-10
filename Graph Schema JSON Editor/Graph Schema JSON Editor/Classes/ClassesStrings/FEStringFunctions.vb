Imports Microsoft.VisualBasic
Imports System.Text.RegularExpressions

Namespace FEStrings

    Public Module StringFunctions

        Public Function Join(ByVal stringArray As String(), ByVal delimiter As String) As String
            Return Microsoft.VisualBasic.Strings.Join(stringArray, delimiter)
        End Function

        Public Function MakeLowerCapCamelCase(ByVal asString As String) As String

            Dim lsCharacter As String
            Dim lsPrecedingCharacter As Char = Nothing
            Dim lsString As String = ""
            Dim liInd As Integer = 0


            For Each lsCharacter In asString

                If (liInd = 0) Then
                    lsString &= LCase(lsCharacter)
                Else
                    If lsPrecedingCharacter <> "" And Not (lsPrecedingCharacter = " ") Then
                        '-----------------------------------------------------------------
                        'Nothing to do here. User might have 'AnimalType' as asString.
                        '  In that instance, must leave 'T' as uppercase.
                        '-----------------------------------------------------------------
                        'lsString &= LCase(lsCharacter)
                        lsString &= lsCharacter
                    ElseIf lsCharacter = " " Then
                        '------------------
                        'Leave out spaces
                        '------------------
                    Else
                        lsString &= UCase(lsCharacter)
                    End If
                End If
                lsPrecedingCharacter = lsCharacter
                liInd += 1
            Next

            Return RemoveWhiteSpace(lsString)

        End Function

        Public Function MakeCapCamelCase(ByVal asString As String,
                                         Optional ByVal abRemoveSpaces As Boolean = False) As String

            Dim lsCharacter As String
            Dim lsPrecedingCharacter As Char = Nothing
            Dim lsString As String = ""
            Dim liInd As Integer = 0


            For Each lsCharacter In asString

                If (liInd = 0) Then
                    lsString &= UCase(lsCharacter)
                Else
                    If lsPrecedingCharacter <> "" And Not ({" ", "_"}.Contains(lsPrecedingCharacter)) Then
                        '-----------------------------------------------------------------
                        'Nothing to do here. User might have 'AnimalType' as asString.
                        '  In that instance, must leave 'T' as uppercase.
                        '-----------------------------------------------------------------                        
                        lsString &= lsCharacter
                    ElseIf lsCharacter = " " Then
                        '------------------
                        'Leave out spaces
                        '------------------
                    ElseIf lsPrecedingCharacter = "_" Then
                        lsString &= UCase(lsCharacter)
                    Else
                        lsString &= UCase(lsCharacter)
                    End If
                End If
                lsPrecedingCharacter = lsCharacter
                liInd += 1
            Next

            If abRemoveSpaces Then
                lsString = New String(lsString.Where(Function(x) Not Char.IsWhiteSpace(x)).ToArray())
            End If

            Return Trim(lsString)

        End Function

        ' Methods
        Public Function checkUpper(ByVal as_string As String) As Boolean
            Return (StrComp(as_string, UCase(as_string), CompareMethod.Binary) = 0)
        End Function

        Public Function CountStringInString(ByVal as_string_to_check As String, ByVal as_look_for_string As String) As Integer
            Dim regex As New Regex(as_look_for_string, RegexOptions.IgnoreCase)
            Return regex.Matches(as_string_to_check).Count
        End Function

        Public Function ExistsStringListElementInString(ByVal aasStringList As List(Of String), ByVal asString As String) As Boolean
            Dim str As String
            For Each str In aasStringList
                If (InStr(asString, str, CompareMethod.Binary) > 0) Then
                    Return True
                End If
            Next
            Return False
        End Function

        Public Function GetFirstWord(ByVal asString As String) As String
            If (InStr(asString, " ", CompareMethod.Binary) > 0) Then
                Return Left(asString, InStr(asString, " ", CompareMethod.Binary))
            End If
            Return asString
        End Function

        Public Function ProperSpace(ByVal [text] As String) As String
            Return Regex.Replace([text], "[A-Z]", " $0").Trim
        End Function

        Public Function RemoveWhiteSpace(ByVal strText As String) As String
            Return Regex.Replace(strText, " ", String.Empty, RegexOptions.IgnoreCase)
        End Function

        Public Function RemoveUnderscores(ByVal asText As String) As String
            Return Regex.Replace(asText, "_", String.Empty)
        End Function

        Public Function ReplaceCharacters(ByRef as_string As String, ByVal as_find_character As String, ByVal as_replace_character As String) As String
            Dim start As Integer = 1
            Dim num2 As Integer = 0
            Dim str As String = ""
            num2 = Len(CStr(as_string))
            start = InStr(start, as_string, as_find_character, CompareMethod.Binary)
            Do While ((start > 0) And (start <= num2))
                str = Left(as_string, (InStr(start, as_string, as_find_character, CompareMethod.Binary) - 1))
                as_string = (str & as_replace_character & Right(as_string, (num2 - start)))
                num2 = Len(CStr(as_string))
                start = InStr((start + Len(as_replace_character)), as_string, as_find_character, CompareMethod.Binary)
            Loop
            Return as_string
        End Function

        Public Function CountWords(ByVal value As String) As Integer

            ' Count matches.
            Dim collection As MatchCollection = Regex.Matches(value, "\S+")
            Return collection.Count

        End Function

    End Module

End Namespace

