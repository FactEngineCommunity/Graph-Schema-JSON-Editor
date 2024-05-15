Imports System.IO
Imports FactEngineForServices
Imports System.Reflection

Public Class frmMain

    Public ToolboxForms As New List(Of WeifenLuo.WinFormsUI.Docking.DockContent)
    Public RightToolboxForms As New List(Of WeifenLuo.WinFormsUI.Docking.DockContent)

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim lsMessage As String

        Try
            prApplication.DebugMode = pcenumDebugMode.DebugCriticalErrorsOnly
            prApplication.ThrowCriticalDebugMessagesToScreen = True

            Call Me.SetupForm()

#Region "Database Location - For embedded Boston database."
            '-------------------------------------------------------------------------------------------------------------
            Dim lsDatabaseLocation As String = ""
            Dim lsDatabaseName As String = ""
            Dim lsDatabaseLocationDirectory As String = ""
            Dim lsDatabaseType As String = "" 'Jet, SQLServer

            Dim lsConnectionString As String
            lsConnectionString = Trim(My.Settings.DatabaseConnectionString)
            Dim lsCommonDatabaseFileLocation As String = ""

            Dim lbFirstRun As Boolean = My.Settings.FirstRun

            Dim lrSQLConnectionStringBuilder As New System.Data.Common.DbConnectionStringBuilder(True)
            lrSQLConnectionStringBuilder.ConnectionString = lsConnectionString

            lsDatabaseLocation = lrSQLConnectionStringBuilder("Data Source")
            lsDatabaseName = Path.GetFileName(lsDatabaseLocation)
            lsDatabaseLocationDirectory = Path.GetDirectoryName(lsDatabaseLocation)

#Region "First Run. For Non-Upgrade installations. Copy boston database out of the Program Files application folder."
            If My.Settings.FirstRun = True Then
                '----------------------------------------------------------------------------------------
                'Move the database to My.Computer.FileSystem.SpecialDirectories.AllUsersApplicationData
                '  and update the ConnectionString for the database.
                '----------------------------------------------------------------------------------------
#Region "AppData - boston.db to AppData\Local\FactEngine\Boston\database\boston.db"
                Dim sourceDatabaseFilePath As String = "Unknown"
                Try
                    ' Get the local AppData folder specific to your company and application
                    Dim localAppDataFolder As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Application.CompanyName, Application.ProductName)

                    ' Create the destination folder if it doesn't exist
                    Directory.CreateDirectory(localAppDataFolder)
                    Directory.CreateDirectory(Path.Combine(localAppDataFolder, "database"))

                    ' Set the source and destination file paths
                    sourceDatabaseFilePath = Path.Combine(MyPath, "database\boston.db")
                    lsCommonDatabaseFileLocation = Path.Combine(localAppDataFolder, "database\boston.db")

                    ' Move the file
                    File.Copy(sourceDatabaseFilePath, lsCommonDatabaseFileLocation)

                    If Not File.Exists(lsCommonDatabaseFileLocation) AndAlso My.Settings.DebugMode = pcenumDebugMode.Debug.ToString Then
                        lsMessage = "Failed to move the boston.db sqlite database to AppData\Local"
                        lsMessage.AppendLine("sourceDatabaseFilePath: " & sourceDatabaseFilePath)
                        lsMessage.AppendLine("CommonDatabaseFileLocation: " & lsCommonDatabaseFileLocation)
                        prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning)
                    End If

                Catch ex As Exception
                    lsMessage = "Failed to move the boston.sqlite database to AppData\Local"
                    lsMessage.AppendLine("sourceDatabaseFilePath: " & sourceDatabaseFilePath)
                    lsMessage.AppendLine("CommonDatabaseFileLocation: " & lsCommonDatabaseFileLocation)
                    lsMessage.AppendDoubleLineBreak("Error Message: " & ex.Message)
                    prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning)
                End Try
#End Region

                lrSQLConnectionStringBuilder = New System.Data.Common.DbConnectionStringBuilder(True)
                lrSQLConnectionStringBuilder.Add("Data Source", lsCommonDatabaseFileLocation)
                lrSQLConnectionStringBuilder.Add("Version", 3)

                '---------------------------
                'Update Configuration File
                My.Settings.DatabaseConnectionString = lrSQLConnectionStringBuilder.ConnectionString
                My.Settings.FirstRun = False
                My.Settings.Save()

                If pbLogStartup Then
                    lsMessage = "FirstRun-Moved database To " & lsCommonDatabaseFileLocation
                    prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Information)
                End If
            End If
#End Region

#End Region

#Region "Open the database & Upgrade If necessary"
            'Force Save of Settings (such that in Debug Mode in Visual Studio we can at least modify default setting on the machine)
            My.Settings.Save()

            If prFactEngine.OpenBostonDatabase(My.Settings.DatabaseConnectionString) Then

            End If

#End Region

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Sub SetupForm()

        Try
            Dim lrChildForm As New frmStartup
            lrChildForm.Show(Me.DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document)

            Dim lfrmSchema As New frmSchema
            lfrmSchema.Show(Me.DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockLeft)

            Dim lfrmPropertiesGrid As New frmToolboxProperties
            lfrmPropertiesGrid.Show(Me.DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockRight)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Sub FactTypeReadingEditorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FactTypeReadingEditorToolStripMenuItem.Click

        Try
            Dim lfrmFactTypeReadingEditor As New frmToolboxORMReadingEditor

            Call lfrmFactTypeReadingEditor.Show(Me.DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockBottom)

            Me.ToolboxForms.AddUnique(lfrmFactTypeReadingEditor)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try
    End Sub

    Public Function GetToolboxForm(ByVal asFormName As String) As WeifenLuo.WinFormsUI.Docking.DockContent

        Dim lrForm As WeifenLuo.WinFormsUI.Docking.DockContent

        Select Case asFormName
            Case Is = "frmToolbox", frmToolboxProperties.Name
                For Each lrForm In Me.RightToolboxForms
                    If lrForm.Name = asFormName Then
                        Return lrForm
                    End If
                Next
            Case Else
                For Each lrForm In Me.ToolboxForms
                    If lrForm.Name = asFormName Then
                        Return lrForm
                    End If
                Next
        End Select

        Return Nothing

    End Function

    Private Sub PropertiesGridToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PropertiesGridToolStripMenuItem.Click

        Try

            Dim lfrmPropertiesGrid As New frmToolboxProperties

            lfrmPropertiesGrid.Show(Me.DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockRight)

            Me.RightToolboxForms.AddUnique(lfrmPropertiesGrid)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Sub SchemaViewerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SchemaViewerToolStripMenuItem.Click

        Dim lfrmSchemaViewer = New frmSchema

        lfrmSchemaViewer.Show(Me.DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockLeft)

    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click

        Me.Hide()
        Me.Close()
        Me.Dispose()

    End Sub

End Class