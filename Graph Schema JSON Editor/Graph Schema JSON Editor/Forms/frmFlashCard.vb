﻿Imports FactEngineForServices
Imports System.Reflection

Public Class frmFlashCard

    Public zsText As String = ""
    Public ziIntervalMilliseconds As Nullable(Of Integer) = Nothing

    Public Overloads Function Show(ByVal asOwner As IWin32Window)

        Try
            Dim loColor As Color = Color.LightGray
            Me.Label1.BackColor = loColor

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

        Return MyBase.ShowDialog()

    End Function

    Public Overloads Function Show(ByVal asOwner As IWin32Window, Optional ByVal asColor As String = "Gray")

        Try
            Dim loColor As Color
            Select Case asColor
                Case Is = "LightGray"
                    loColor = Color.LightGray
                Case Is = "White"
                    loColor = Color.White
                Case Else
                    loColor = Color.LightGray
            End Select

            Me.Label1.BackColor = loColor

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

        Return MyBase.ShowDialog()

    End Function

    Public Overloads Function Show(ByVal asOwner As IWin32Window, Optional ByVal asColor As Color? = Nothing) As DialogResult
        Try
            Dim loColor As Color
            If asColor.HasValue Then
                loColor = asColor.Value
            Else
                loColor = Color.LightGray
            End If

            Me.Label1.BackColor = loColor
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

        Return MyBase.ShowDialog()
    End Function

    Public Overloads Function ShowDialog(ByVal asOwner As IWin32Window, Optional ByVal asColor As String = "Gray")

        Try
            Dim loColor As Color
            Select Case asColor
                Case Is = "LightGray"
                    loColor = Color.LightGray
                Case Is = "White"
                    loColor = Color.White
                Case Else
                    loColor = Color.LightGray
            End Select

            Me.Label1.BackColor = loColor

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

        Return MyBase.ShowDialog()

    End Function

    Private Sub frmFlashCard_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        If Me.ziIntervalMilliseconds IsNot Nothing Then
            Me.Timer.Interval = Me.ziIntervalMilliseconds
        End If

        Me.Width = Me.CreateGraphics.MeasureString(Me.zsText & "MMMMMM", Me.Font).Width
        Me.CenterToParent()
        Call Me.MakeRoundedCorners()
        Me.Label1.Text = Me.zsText

        Me.Timer.Start()

        Me.Button.Visible = False

    End Sub

    Public Sub resizeToText()

        Dim liTextWidth As Integer = 0
        liTextWidth = TextRenderer.MeasureText(Me.zsText, Me.Font).Width
        Me.Width = Math.Max(Me.Button.Width, liTextWidth) + 20

        Dim liNewButtonX As Integer = (Me.Width / 2) - (Me.Button.Width / 2)

        Me.Button.Location = New Point(liNewButtonX, Me.Button.Location.Y)

    End Sub

    Private Sub Timer_Tick(sender As Object, e As EventArgs) Handles Timer.Tick

        'Me.MdiParent = frmMain
        'Me.Hide()
        Me.Close()

    End Sub

    Private Sub Button_Click(sender As Object, e As EventArgs) Handles Button.Click

        Me.Hide()
        Me.Close()

    End Sub

    Private Sub MakeRoundedCorners()

        Try
            Dim liArcRadius As Integer = 35

            Me.FormBorderStyle = FormBorderStyle.None
            Dim p As New Drawing2D.GraphicsPath()
            p.StartFigure()
            p.AddArc(New Rectangle(0, 0, liArcRadius, liArcRadius), 180, 90)
            p.AddLine(liArcRadius, 0, Me.Width - liArcRadius, 0)
            p.AddArc(New Rectangle(Me.Width - liArcRadius, 0, liArcRadius, liArcRadius), -90, 90)
            p.AddLine(Me.Width, liArcRadius, Me.Width, Me.Height - liArcRadius)
            p.AddArc(New Rectangle(Me.Width - liArcRadius, Me.Height - liArcRadius, liArcRadius, liArcRadius), 0, 90)
            p.AddLine(Me.Width - liArcRadius, Me.Height, liArcRadius, Me.Height)
            p.AddArc(New Rectangle(0, Me.Height - liArcRadius, liArcRadius, liArcRadius), 90, 90)
            p.CloseFigure()
            Me.Region = New Region(p)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning, ex.StackTrace)
        End Try

    End Sub

End Class