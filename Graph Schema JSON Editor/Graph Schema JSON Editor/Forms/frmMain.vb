﻿Imports System.IO
Imports FactEngineForServices
Imports System.Linq
Imports System.Reflection

Public Class frmMain

    Public ToolboxForms As New List(Of WeifenLuo.WinFormsUI.Docking.DockContent)
    Public RightToolboxForms As New List(Of WeifenLuo.WinFormsUI.Docking.DockContent)

    Public mfrmSchemaManager As frmSchema = Nothing

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim lsMessage As String

        Try
            prApplication.DebugMode = pcenumDebugMode.DebugCriticalErrorsOnly
            prApplication.ThrowCriticalDebugMessagesToScreen = True
            prApplication.ApplicationVersionNr = "0.9"
            FactEngineForServices.psDatabaseConnectionString = My.Settings.DatabaseConnectionString

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
                'Great!
            Else
                Throw New Exception("Cannot open the Boston database.")
            End If

            prApplication.DatabaseVersionNr = TableReferenceFieldValue.GetReferenceFieldValue(1, 1)
#End Region

            Call Me.LoadCoreModel() 'The Core model is used to store the RDS (Relational Data Structure) within FBM (Fact-Based Modelling) Models.

            Call Me.SetupForm()

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

            '=============================================
            'Schema Viewer
            Dim lfrmSchema As New frmSchema
            lfrmSchema.Application = prApplication
            Me.ToolboxForms.AddUnique(lfrmSchema)
            lfrmSchema.Show(Me.DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockLeft)
            Me.mfrmSchemaManager = lfrmSchema

            Dim lfrmPropertiesGrid As New frmToolboxProperties
            Me.RightToolboxForms.AddUnique(lfrmPropertiesGrid)
            lfrmPropertiesGrid.Show(Me.DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockRight)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Sub LoadCoreModel()

        Try
            prApplication.CMML.Core = New FBM.Model(pcenumLanguage.ORMModel, pcenumCMMLCoreModel.Core.ToString, True)

            'WriteToStatusBar("Loading the Core MetaMetaModel")
            Call TableModel.GetModelDetails(prApplication.CMML.Core)
            prApplication.CMML.Core.Load(True, False)
            prApplication.CMML.Core.LoadPreLoadedXMLPagesFromXML()

            FactEngineForServices.prApplication.CMML.Core = prApplication.CMML.Core

            'Speed up loading of the Core Model.
            If Not prApplication.CMML.Core.StoreAsXML Then
                'CodeSafe - Make sure the Pages are loaded.
                For Each lrPage In CType(prApplication.CMML.Core, FBM.Model).Page.Where(Function(page) Not page.Loaded).ToList()
                    Call lrPage.Load(False)
                Next

                With New WaitCursor
                    prApplication.CMML.Core.SetStoreAsXML(True, True)
                End With
            End If

            If pbLogStartup Then
                prApplication.ThrowErrorMessage("Successfully loaded the Core Model", pcenumErrorType.Information)
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Public Function loadPropertiesGrid() As frmToolboxProperties

        Dim lfrmToolboxProperties As New frmToolboxProperties

        lfrmToolboxProperties = Me.GetToolboxForm(lfrmToolboxProperties.Name)

        If lfrmToolboxProperties Is Nothing Then

            lfrmToolboxProperties = New frmToolboxProperties

            lfrmToolboxProperties.Show(Me.DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockRight)

            Me.RightToolboxForms.AddUnique(lfrmToolboxProperties)
        Else
            lfrmToolboxProperties.BringToFront()
        End If

        Return lfrmToolboxProperties

    End Function

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

        Me.mfrmSchemaManager = lfrmSchemaViewer

        lfrmSchemaViewer.Show(Me.DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockLeft)

    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click

        Me.Hide()
        Me.Close()
        Me.Dispose()

    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click

        frmAbout.ShowDialog()

    End Sub


End Class