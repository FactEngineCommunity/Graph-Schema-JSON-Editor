Imports FactEngineForServices
Imports System.IO
Imports System.Reflection



Public Module publicFunctions

    Public Function MyPath() As String
        'get the app path
        Dim fullAppName As String = [Assembly].GetExecutingAssembly().GetName().CodeBase
        'This strips off the exe name
        Dim FullAppPath As String = Path.GetDirectoryName(fullAppName)

        FullAppPath = Mid(FullAppPath, Len("file:\\"))

        Return FullAppPath
    End Function

    ''' <summary>
    ''' Displays a FlashCard on the screen.
    ''' </summary>
    ''' <param name="asText">Text to display.</param>
    ''' <param name="aoColor">The color of the FlashCard</param>
    ''' <param name="aiInterval">The interval, in milliseconds, to display the FlashCard for.</param>
    Public Sub ShowFlashCard(ByVal asText As String,
                             ByVal aoColor? As Color,
                             Optional ByVal aiInterval As Integer = 2500,
                             Optional ByVal aiFontSize As Single = 10)

        Try
            Dim lfrmFlashCard As New frmFlashCard
            lfrmFlashCard.ziIntervalMilliseconds = aiInterval
            lfrmFlashCard.zsText = asText
            lfrmFlashCard.Show(frmMain, aoColor)
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try
    End Sub

End Module
