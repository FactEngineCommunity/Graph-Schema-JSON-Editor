﻿Imports FactEngineForServices
Imports System.Xml.Serialization
Imports System.Xml.Linq
Imports System.IO
Imports System.Reflection
Imports System.Configuration
Imports System.Data.SQLite
Imports System.ComponentModel

Public Class frmCRUDModel

    Public WithEvents zrModel As FBM.Model

    Private mrODBCConnection As System.Data.Odbc.OdbcConnection

    Private ErrorCount As Integer = 0

    'Sample ConnectionStrings
    '   "Driver={SQL Server};Server=(local);Trusted_Connection=Yes;Database=AdventureWorks;"
    '   "Driver={Microsoft ODBC for Oracle};Server=ORACLE8i7;Persist Security Info=False;Trusted_Connection=Yes"
    '   "Driver={Microsoft Access Driver (*.mdb)};DBQ=c:\bin\Northwind.mdb"
    '   "Driver={Microsoft Excel Driver (*.xls)};DBQ=c:\bin\book1.xls"
    '   "Driver={Microsoft Text Driver (*.txt; *.csv)};DBQ=c:\bin"
    '   "DSN=dsnname"

    Private Sub frmCRUDModel_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Call Me.SetupForm()

    End Sub

    Private Sub SetupForm()

        Try
            Me.LabelModelName.Text = Me.zrModel.Name
            Me.LabelCoreVersion.Text = Me.zrModel.CoreVersionNumber

            Call Me.LoadDatabaseTypes()

            Me.TextBoxDatabaseConnectionString.Text = Me.zrModel.TargetDatabaseConnectionString
            Me.CheckBoxIsDatabaseSynchronised.Checked = Me.zrModel.IsDatabaseSynchronised

            If Me.zrModel.IsEmpty Then
                Me.GroupBoxReverseEngineering.Visible = True
                Me.ButtonReverseEngineerDatabase.Enabled = True
            End If

            Me.TextBoxServerName.Text = Me.zrModel.Server
            Me.TextBoxDatabaseName.Text = Me.zrModel.Database
            Me.TextBoxSchemaName.Text = Me.zrModel.Schema
            Me.TextBoxWarehouseName.Text = Me.zrModel.Warehouse
            Me.TextBoxRoleName.Text = Me.zrModel.DatabaseRole
            Me.TextBoxPort.Text = Me.zrModel.Port

            RemoveHandler Me.CheckBoxSaveToXML.CheckedChanged, AddressOf CheckBoxSaveToXML_CheckedChanged
            Me.CheckBoxSaveToXML.Checked = Me.zrModel.StoreAsXML
            AddHandler Me.CheckBoxSaveToXML.CheckedChanged, AddressOf CheckBoxSaveToXML_CheckedChanged

            If Me.CheckBoxSaveToXML.Checked Then Me.ButtonReplaceDatabaseModel.Visible = True

            Me.LabelModelId.Text = "Model Id: " & Me.zrModel.ModelId


            '===Model Settings - From ReferenceFieldTable/Value=======================================================================
            'UseNeo4jStyleEdgeLabels etc
            Me.CheckBoxUseNeo4jStyleEdgeLabels.Checked = Me.zrModel.UseNeo4jStyleEdgeLabels
            Me.CheckBoxAutomaticallyCreateReferenceMode.Checked = Me.zrModel.AutomaticallyCreateReferenceMode
            Me.TextBoxDefaultReferenceMode.Text = Trim(Me.zrModel.DefaultReferenceMode)
            Me.CheckBoxHideOtherwiseForeignKeyColumns.Checked = Me.zrModel.HideOtherwiseForeignKeyColumns
            Me.CheckBoxHideReferenceModesByDefault.Checked = Me.zrModel.HideReferenceModesByDefault


        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Sub Button_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Cancel.Click

        Me.Hide()
        Me.Close()

    End Sub

    Private Sub button_okay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button_okay.Click

        If check_fields() Then

            Me.zrModel.TargetDatabaseType = Me.ComboBoxDatabaseType.SelectedItem.Tag
            Me.zrModel.TargetDatabaseConnectionString = Trim(Me.TextBoxDatabaseConnectionString.Text)
            Me.zrModel.IsDatabaseSynchronised = Me.CheckBoxIsDatabaseSynchronised.Checked

            Me.zrModel.Server = Trim(Me.TextBoxServerName.Text)
            Me.zrModel.Database = Trim(Me.TextBoxDatabaseName.Text)
            Me.zrModel.Schema = Trim(Me.TextBoxSchemaName.Text)
            Me.zrModel.Warehouse = Trim(Me.TextBoxWarehouseName.Text)
            Me.zrModel.DatabaseRole = Trim(Me.TextBoxRoleName.Text)
            Me.zrModel.Port = Trim(Me.TextBoxPort.Text)
            Me.zrModel.StoreAsXML = Me.CheckBoxSaveToXML.Checked

            '======Settings===Stored In ReferenceFieldTable/Value============================================
            'UseNeo4jStyleEdgeLabels
#Region "UseNeo4jStyleEdgeLabels"
            Me.zrModel.UseNeo4jStyleEdgeLabels = Me.CheckBoxUseNeo4jStyleEdgeLabels.Checked

            Dim loSetting As New With {.ModelId = Me.zrModel.ModelId, .SettingName = "UseNeo4jStyleEdgeLabels", .Setting = Me.CheckBoxUseNeo4jStyleEdgeLabels.Checked.ToString}
            Dim larKeyFields() As Object = {}
            larKeyFields.Add(New With {.FieldId = 1, .FieldName = "ModelId", .Value = Me.zrModel.ModelId})
            larKeyFields.Add(New With {.FieldId = 2, .FieldName = "SettingName", .Value = "UseNeo4jStyleEdgeLabels"})
            Call TableReferenceTable.UpSert(39, loSetting, larKeyFields)
#End Region

            'AutomaticallyCreateReferenceMode
#Region "AutomaticallyCreateReferenceMode"
            Me.zrModel.AutomaticallyCreateReferenceMode = Me.CheckBoxAutomaticallyCreateReferenceMode.Checked

            loSetting = New With {.ModelId = Me.zrModel.ModelId, .SettingName = "AutomaticallyCreateReferenceMode", .Setting = Me.CheckBoxAutomaticallyCreateReferenceMode.Checked.ToString}
            larKeyFields = {}
            larKeyFields.Add(New With {.FieldId = 1, .FieldName = "ModelId", .Value = Me.zrModel.ModelId})
            larKeyFields.Add(New With {.FieldId = 2, .FieldName = "SettingName", .Value = "AutomaticallyCreateReferenceMode"})
            Call TableReferenceTable.UpSert(39, loSetting, larKeyFields)
#End Region

            'DefaultReferenceMode
#Region "DefaultReferenceMode"
            Me.zrModel.DefaultReferenceMode = Trim(Me.TextBoxDefaultReferenceMode.Text)

            loSetting = New With {.ModelId = Me.zrModel.ModelId, .SettingName = "DefaultReferenceMode", .Setting = Me.zrModel.DefaultReferenceMode}
            larKeyFields = {}
            larKeyFields.Add(New With {.FieldId = 1, .FieldName = "ModelId", .Value = Me.zrModel.ModelId})
            larKeyFields.Add(New With {.FieldId = 2, .FieldName = "SettingName", .Value = "DefaultReferenceMode"})
            Call TableReferenceTable.UpSert(39, loSetting, larKeyFields)
#End Region

            'AutomaticallyCreateReferenceMode
#Region "HideOtherwiseForeignKeyColumns"
            Me.zrModel.HideOtherwiseForeignKeyColumns = Me.CheckBoxHideOtherwiseForeignKeyColumns.Checked

            loSetting = New With {.ModelId = Me.zrModel.ModelId, .SettingName = "HideOtherwiseForeignKeyColumns", .Setting = Me.CheckBoxHideOtherwiseForeignKeyColumns.Checked.ToString}
            larKeyFields = {}
            larKeyFields.Add(New With {.FieldId = 1, .FieldName = "ModelId", .Value = Me.zrModel.ModelId})
            larKeyFields.Add(New With {.FieldId = 2, .FieldName = "SettingName", .Value = "HideOtherwiseForeignKeyColumns"})
            Call TableReferenceTable.UpSert(39, loSetting, larKeyFields)
#End Region

            'HideReferenceModesByDefault
#Region "HideReferenceModesByDefault"
            Me.zrModel.HideReferenceModesByDefault = Me.CheckBoxHideReferenceModesByDefault.Checked

            loSetting = New With {.ModelId = Me.zrModel.ModelId, .SettingName = "HideReferenceModesByDefault", .Setting = Me.CheckBoxHideReferenceModesByDefault.Checked.ToString}
            larKeyFields = {}
            larKeyFields.Add(New With {.FieldId = 1, .FieldName = "ModelId", .Value = Me.zrModel.ModelId})
            larKeyFields.Add(New With {.FieldId = 2, .FieldName = "SettingName", .Value = "HideReferenceModesByDefault"})
            Call TableReferenceTable.UpSert(39, loSetting, larKeyFields)
#End Region
            '================================================================================================

            Try
                If Me.zrModel.TreeNode IsNot Nothing Then
                    If 1 = 1 Then 'My.Settings.FactEngineShowDatabaseLogoInModelExplorer - Just show for GraphSchemaJSON Editor.
                        Select Case Me.zrModel.TargetDatabaseType
                            Case Is = pcenumDatabaseType.MongoDB
                                Me.zrModel.TreeNode.ImageIndex = 6
                                Me.zrModel.TreeNode.SelectedImageIndex = 6
                            Case Is = pcenumDatabaseType.SQLServer
                                Me.zrModel.TreeNode.ImageIndex = 9
                                Me.zrModel.TreeNode.SelectedImageIndex = 9
                            Case Is = pcenumDatabaseType.MSJet
                                Me.zrModel.TreeNode.ImageIndex = 7
                                Me.zrModel.TreeNode.SelectedImageIndex = 7
                            Case Is = pcenumDatabaseType.SQLite
                                Me.zrModel.TreeNode.ImageIndex = 8
                                Me.zrModel.TreeNode.SelectedImageIndex = 8
                            Case Is = pcenumDatabaseType.ODBC
                                Me.zrModel.TreeNode.ImageIndex = 10
                                Me.zrModel.TreeNode.SelectedImageIndex = 10
                            Case Is = pcenumDatabaseType.Neo4j
                                Me.zrModel.TreeNode.ImageIndex = 14
                                Me.zrModel.TreeNode.SelectedImageIndex = 14
                            Case Is = pcenumDatabaseType.KuzuDB
                                Me.zrModel.TreeNode.ImageIndex = 22
                                Me.zrModel.TreeNode.SelectedImageIndex = 22
                            Case Is = pcenumDatabaseType.RelationalAI
                                Me.zrModel.TreeNode.ImageIndex = 21
                                Me.zrModel.TreeNode.SelectedImageIndex = 21
                            Case Is = pcenumDatabaseType.EdgeDB
                                Me.zrModel.TreeNode.ImageIndex = 23
                                Me.zrModel.TreeNode.SelectedImageIndex = 23
                            Case Is = pcenumDatabaseType.None
                                Me.zrModel.TreeNode.ImageIndex = 1
                                Me.zrModel.TreeNode.SelectedImageIndex = 1
                            Case Is = pcenumDatabaseType.FactEngineAbstractionLayer
                                Me.zrModel.TreeNode.ImageIndex = 10
                                Me.zrModel.TreeNode.SelectedImageIndex = 10
                        End Select
                    End If
                End If
            Catch ex As Exception
            End Try

            Me.zrModel.Save()

            Me.Hide()
            Me.Close()
            Me.Dispose()
        End If

    End Sub

    Private Function check_fields() As Boolean

        Return True

    End Function

    Sub LoadDatabaseTypes()

        Dim loWorkingClass As New Object
        Dim larDatabaseType As New List(Of Object)
        Dim liReferenceTableId As Integer = 0
        Dim liInd As Integer = 0
        Dim liNewIndex As Integer = 0

        Try
            Me.ComboBoxDatabaseType.Items.Add(New tComboboxItem(pcenumDatabaseType.None, pcenumDatabaseType.None.ToString, pcenumDatabaseType.None))
            Me.ComboBoxDatabaseType.SelectedIndex = 0

            If pdbConnection.State <> 0 Then
                liReferenceTableId = TableReferenceTable.GetReferenceTableIdByName("DatabaseType")
                larDatabaseType = TableReferenceFieldValue.GetReferenceFieldValueTuples(liReferenceTableId, loWorkingClass)

                For liInd = 1 To larDatabaseType.Count
                    Dim liDatabaseType = CType([Enum].Parse(GetType(pcenumDatabaseType), NullVal(larDatabaseType(liInd - 1).DatabaseType, pcenumDatabaseType.None)), pcenumDatabaseType)
                    Dim lrComboboxItem As New tComboboxItem(larDatabaseType(liInd - 1).DatabaseType, larDatabaseType(liInd - 1).DatabaseType, liDatabaseType)
                    Select Case prApplication.SoftwareCategory
                        Case Is = pcenumSoftwareCategory.Student
                            Select Case liDatabaseType
                                Case Is = pcenumDatabaseType.SQLite
                                    liNewIndex = Me.ComboBoxDatabaseType.Items.Add(lrComboboxItem)
                                Case Else
                                    'Sorry, not supported.
                            End Select
                        Case Else
                            liNewIndex = Me.ComboBoxDatabaseType.Items.Add(lrComboboxItem)
                    End Select

                    If larDatabaseType(liInd - 1).DatabaseType = Trim(Me.zrModel.TargetDatabaseType.ToString) Then
                        Me.ComboBoxDatabaseType.SelectedIndex = liNewIndex
                    End If
                Next
            Else
                Select Case prApplication.SoftwareCategory
                    Case Is = pcenumSoftwareCategory.Student
                        Me.ComboBoxDatabaseType.Items.Add(New tComboboxItem(pcenumDatabaseType.SQLite, pcenumDatabaseType.SQLite.ToString, pcenumDatabaseType.None))
                    Case Else
                        Me.ComboBoxDatabaseType.Items.Add(New tComboboxItem(pcenumDatabaseType.MSJet, pcenumDatabaseType.MSJet.ToString, pcenumDatabaseType.None))
                        Me.ComboBoxDatabaseType.Items.Add(New tComboboxItem(pcenumDatabaseType.SQLServer, pcenumDatabaseType.SQLServer.ToString, pcenumDatabaseType.None))
                        Me.ComboBoxDatabaseType.Items.Add(New tComboboxItem(pcenumDatabaseType.Neo4j, pcenumDatabaseType.Neo4j.ToString, pcenumDatabaseType.None))
                End Select

                Me.ComboBoxDatabaseType.SelectedIndex = Me.ComboBoxDatabaseType.FindString(Me.zrModel.TargetDatabaseType.ToString)
            End If

            'Add ISO GQL as a Database Type.
            Dim liISOGQLIndex = Me.ComboBoxDatabaseType.Items.Add(New tComboboxItem(pcenumDatabaseType.ISOGQL, pcenumDatabaseType.ISOGQL.ToString, pcenumDatabaseType.ISOGQL))
            If Me.zrModel.TargetDatabaseType = pcenumDatabaseType.ISOGQL Then
                Me.ComboBoxDatabaseType.SelectedIndex = liISOGQLIndex
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Function checkDatabaseConnectionString(ByRef asReturnMessage As String,
                                                   Optional ByVal asConnectionString As String = Nothing) As Boolean

        Dim lsDatabaseLocation As String
        Dim lsConnectionString As String

        If asConnectionString IsNot Nothing Then
            lsConnectionString = asConnectionString
        Else
            lsConnectionString = Trim(Me.TextBoxDatabaseConnectionString.Text)
        End If

        Try

            Dim lrSQLConnectionStringBuilder As System.Data.Common.DbConnectionStringBuilder = Nothing

            Try
                lrSQLConnectionStringBuilder = New System.Data.Common.DbConnectionStringBuilder(True) With {
                   .ConnectionString = lsConnectionString
                }

                lsDatabaseLocation = lrSQLConnectionStringBuilder("Data Source")

            Catch ex As Exception
                asReturnMessage = "Please fix the Database Connection String and try again." & vbCrLf & vbCrLf & ex.Message
                Return False
            End Try

            If Not System.IO.File.Exists(lsDatabaseLocation) Then
                asReturnMessage = "The database source of the Database Connection String you provided points to a file that does not exist."
                asReturnMessage &= vbCrLf & vbCrLf
                asReturnMessage &= "Please fix the Database Connection String and try again."
                Return False
            End If

            Try
                Select Case Me.ComboBoxDatabaseType.SelectedItem.Tag
                    Case Is = pcenumDatabaseType.SQLite
                        If FactEngineForServices.Database.SQLiteDatabase.CreateConnection(lsConnectionString) Is Nothing Then
                            Throw New Exception("Can't connect to the database with that connection string.")
                        End If
                    Case Is = pcenumDatabaseType.MSJet
                        'Dim ldbConnection As New ADODB.Connection
                        'Call ldbConnection.Open(lsConnectionString)
                End Select
            Catch ex As Exception
                asReturnMessage &= "Please fix the Database Connection String and try again." & vbCrLf & vbCrLf & ex.Message
                Return False
            End Try

            Return True

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning, ex.StackTrace,,)

            Return False
        End Try

    End Function

    Private Sub ButtonTestConnection_Click(sender As Object, e As EventArgs) Handles ButtonTestConnection.Click

        Try
            Call Me.TestConnection()
        Catch ex As Exception

        End Try

    End Sub

    Private Function TestConnection() As Boolean

        Dim lsMessage As String = ""
        Try
            If Me.zrModel.RequiresConnectionString Then
                If Trim(Me.TextBoxDatabaseConnectionString.Text) = "" Then
                    MsgBox("Please provide a Connection String for the database.")
                    Return False
                End If
            End If


            With New WaitCursor
                Select Case Me.ComboBoxDatabaseType.SelectedItem.Tag
                    Case Is = pcenumDatabaseType.SQLite
                        Dim lsReturnMessage As String = Nothing
                        If Not Me.checkDatabaseConnectionString(lsReturnMessage) Then
                            MsgBox(lsReturnMessage)
                            Return False
                        End If


                        Me.LabelOpenSuccessfull.Visible = True
                        If FactEngineForServices.Database.SQLiteDatabase.CreateConnection(Me.TextBoxDatabaseConnectionString.Text) Is Nothing Then
                            Me.LabelOpenSuccessfull.ForeColor = Color.Red
                            Me.LabelOpenSuccessfull.Text = "Fail"
                        Else
                            Me.LabelOpenSuccessfull.ForeColor = Color.Green
                            Me.LabelOpenSuccessfull.Text = "Success"
                        End If

                    Case Is = pcenumDatabaseType.EdgeDB
                        Throw New NotImplementedException("Need to set up tester for EdgeDB")

                    Case Is = pcenumDatabaseType.KuzuDB
#Region "KuzuDB"
                        Throw New NotImplementedException("KuzuDB not yet implemented.")
                        'Dim lsReturnMessage As String = Nothing

                        'Me.LabelOpenSuccessfull.Visible = True

                        'Try
                        '    Dim lrConnection = New FactEngine.KuzuDBConnection(Me.zrModel,
                        '                                                       Trim(Me.TextBoxDatabaseConnectionString.Text),
                        '                                                       0,
                        '                                                       False)
                        '    Me.LabelOpenSuccessfull.ForeColor = Color.Green
                        '    Me.LabelOpenSuccessfull.Text = "Success"
                        '    Return True

                        'Catch ex As Exception
                        '    GoTo ConnectionFailedKuzuDB
                        'End Try
ConnectionFailedKuzuDB:
                        Me.LabelOpenSuccessfull.ForeColor = Color.Red
                        Me.LabelOpenSuccessfull.Text = "Fail"
#End Region

                    Case Is = pcenumDatabaseType.Neo4j
                        Dim lsReturnMessage As String = Nothing

                        Me.LabelOpenSuccessfull.Visible = True

                        Try
                            Dim lrConnection = New FactEngine.Neo4jConnection(Me.zrModel,
                                                                          Trim(Me.TextBoxDatabaseConnectionString.Text),
                                                                          0,
                                                                          False)
                            Me.LabelOpenSuccessfull.ForeColor = Color.Green
                            Me.LabelOpenSuccessfull.Text = "Success"
                            Return True

                        Catch ex As Exception
                            GoTo ConnectionFailed
                        End Try
ConnectionFailed:
                        Me.LabelOpenSuccessfull.ForeColor = Color.Red
                        Me.LabelOpenSuccessfull.Text = "Fail"

                    Case Is = pcenumDatabaseType.RelationalAI
'                        Dim lsReturnMessage As String = Nothing

'                        Me.LabelOpenSuccessfull.Visible = True

'                        Try
'                            Dim lrConnection = New FactEngine.RelationalAIConnection(Me.zrModel,
'                                                                                     Trim(Me.TextBoxDatabaseConnectionString.Text),
'                                                                                     0,
'                                                                                     False)
'                            Me.LabelOpenSuccessfull.ForeColor = Color.Green
'                            Me.LabelOpenSuccessfull.Text = "Success"

'                            Return True

'                        Catch ex As Exception
'                            lsMessage = ex.Message
'                            GoTo RAIConnectionFailed
'                        End Try
'RAIConnectionFailed:
'                        Me.LabelOpenSuccessfull.ForeColor = Color.Red
'                        Me.LabelOpenSuccessfull.Text = "Fail" & lsMessage

                    Case Is = pcenumDatabaseType.MSJet

                        Throw New NotImplementedException("MSJet not yet implemented.")
                        'Dim ldbConnection As New ADODB.Connection
                        'Me.LabelOpenSuccessfull.Text = "Testing Connection"
                        'Me.LabelOpenSuccessfull.Visible = True

                        'ldbConnection.Open(Me.TextBoxDatabaseConnectionString.Text)

                        'Me.LabelOpenSuccessfull.ForeColor = Color.Green
                        'Me.LabelOpenSuccessfull.Text = "Success"

                        'ldbConnection.Close()

                    Case Is = pcenumDatabaseType.SQLServer
                        Dim lrODBCConnection As New System.Data.Odbc.OdbcConnection(Me.TextBoxDatabaseConnectionString.Text)

                        Me.LabelOpenSuccessfull.Text = "Testing Connection"
                        Me.LabelOpenSuccessfull.Visible = True

                        lrODBCConnection.Open()

                        Me.LabelOpenSuccessfull.ForeColor = Color.Green
                        Me.LabelOpenSuccessfull.Text = "Success"

                        lrODBCConnection.Close()

                    Case Is = pcenumDatabaseType.MongoDB
                        'Dim connectionString As String = ConfigurationManager.ConnectionStrings("mongosqld --mongo-uri 'mongodb: //university-shard-00-02.8dmfw.azure.mongodb.net:27017,university-shard-00-00.8dmfw.azure.mongodb.net:27017,university-shard-00-01.8dmfw.azure.mongodb.net:27017/?ssl=true&replicaSet=atlas-7kqhl6-shard-0&retryWrites=true&w=majority' --auth - u Viev -p Viev").ConnectionString
                        Dim lrODBCConnection As New System.Data.Odbc.OdbcConnection(Me.TextBoxDatabaseConnectionString.Text)

                        lrODBCConnection.Open()

                        Me.LabelOpenSuccessfull.ForeColor = Color.Green
                        Me.LabelOpenSuccessfull.Text = "Success"
                        Me.LabelOpenSuccessfull.Visible = True

                        lrODBCConnection.Close()

                    Case Is = pcenumDatabaseType.PostgreSQL
                        Dim lrODBCConnection As New System.Data.Odbc.OdbcConnection(Me.TextBoxDatabaseConnectionString.Text)

                        lrODBCConnection.Open()

                        Me.LabelOpenSuccessfull.ForeColor = Color.Green
                        Me.LabelOpenSuccessfull.Text = "Success"
                        Me.LabelOpenSuccessfull.Visible = True

                        lrODBCConnection.Close()

                    Case Is = pcenumDatabaseType.Snowflake
                        Dim lrODBCConnection As New System.Data.Odbc.OdbcConnection(Me.TextBoxDatabaseConnectionString.Text)

                        lrODBCConnection.Open()

                        Me.LabelOpenSuccessfull.ForeColor = Color.Green
                        Me.LabelOpenSuccessfull.Text = "Success"
                        Me.LabelOpenSuccessfull.Visible = True

                        lrODBCConnection.Close()

                    Case Is = pcenumDatabaseType.ODBC
                        Dim lrODBCConnection As New System.Data.Odbc.OdbcConnection(Me.TextBoxDatabaseConnectionString.Text)

                        lrODBCConnection.Open()

                        Me.LabelOpenSuccessfull.ForeColor = Color.Green
                        Me.LabelOpenSuccessfull.Text = "Success"
                        Me.LabelOpenSuccessfull.Visible = True

                        lrODBCConnection.Close()

                    Case Is = pcenumDatabaseType.TypeDB

                        If Not Me.zrModel.connectToDatabase Then
                            Throw New Exception("Failed to connect to database")
                        End If

                        Me.LabelOpenSuccessfull.ForeColor = Color.Green
                        Me.LabelOpenSuccessfull.Text = "Success"
                        Me.LabelOpenSuccessfull.Visible = True

                    Case Is = pcenumDatabaseType.FactEngineAbstractionLayer
                        ShowFlashCard("Not currently implemented", Color.LightGray, 2500, 10)

                    Case Else

                        Me.LabelOpenSuccessfull.ForeColor = Color.Red
                        Me.LabelOpenSuccessfull.Text = "Unknown database type, '" & prApplication.WorkingModel.TargetDatabaseType.ToString & "'."
                        Me.LabelOpenSuccessfull.Visible = True
                End Select
            End With

            Return True
        Catch ex As Exception

            Me.LabelOpenSuccessfull.ForeColor = Color.Red
            Me.LabelOpenSuccessfull.Text = "Fail: " & ex.Message
            Me.LabelOpenSuccessfull.Visible = True

            Return False
        End Try

    End Function



    Private Sub AddREMessage(ByVal asMessage As String,
                             Optional ByVal aiColor As Color = Nothing,
                             Optional ByVal abInBold As Boolean = False)

        Try
            If aiColor = Nothing Then
                Me.RichTextBoxREMessages.AppendText(vbCrLf & asMessage)
            Else
                Me.RichTextBoxREMessages.AppendStringInColor(vbCrLf & asMessage, aiColor, abInBold)
            End If
            Me.RichTextBoxREMessages.ScrollToCaret()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try
    End Sub



    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles ButtonReverseEngineerDatabase.Click

        Try
            Dim lasSchemaName As New List(Of String)

            If Not Me.TestConnection Then
                MsgBox("Connection to the database failed.")
                Exit Sub
            End If

            '------------------------------------------------------------------------------------------
            'Check to see that Reverse Engineering is supported for the datatabase type of the model.
            Dim larSupportedDatabases = {pcenumDatabaseType.SQLite, pcenumDatabaseType.Snowflake, pcenumDatabaseType.TypeDB, pcenumDatabaseType.KuzuDB, pcenumDatabaseType.PostgreSQL}

            If Not larSupportedDatabases.Contains(Me.zrModel.TargetDatabaseType) Then
                MsgBox("The database type of this model is not supported. Please contact support.")
                Exit Sub
            End If

            With New WaitCursor

                Me.ProgressBarReverseEngineering.Visible = True

                Me.RichTextBoxREMessages.Clear()
                Me.ProgressBarReverseEngineering.Value = 0

                If Not Me.TestConnection Then
                    Call Me.AddREMessage("- Failed to connect to database. Have you set the Database Type and its Connection String?")
                    Exit Sub
                End If

                Me.zrModel.RDS.TargetDatabaseType = Me.ComboBoxDatabaseType.SelectedItem.Tag 'DirectCast(System.[Enum].Parse(GetType(pcenumDatabaseType), Me.ComboBoxDatabaseType.SelectedItem), pcenumDatabaseType)

                Dim lrReverseEngineer As New ODBCDatabaseReverseEngineer(Me.zrModel,
                                                                         Trim(Me.TextBoxDatabaseConnectionString.Text),
                                                                         True,
                                                                         Me.BackgroundWorker,
                                                                         Me.CheckBoxReverseEngineeringShowExtraInformation.Checked)

                Call Me.AddREMessage("- Reverse engineering started.")
                Dim lsErrorMessage As String = ""
                If Not lrReverseEngineer.ReverseEngineerDatabase(lsErrorMessage) Then
                    Me.ErrorProvider.SetError(Me.ButtonReverseEngineerDatabase, lsErrorMessage)
                    Call Me.AddREMessage("- Failed.", Color.Red, True)
                Else
                    Call Me.AddREMessage("- Finished reverse engineering the database.")
                    Call Me.AddREMessage("- Saving the model.")
                    Call Me.zrModel.MakeDirty(False, False)
                    Call Me.zrModel.Save()
                    Call Me.AddREMessage("- Complete.", Color.Green, True)
                End If

                Me.ButtonReverseEngineerDatabase.Enabled = False
            End With

            Me.ProgressBarReverseEngineering.Visible = False

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try
    End Sub


    Private Sub ComboBoxDatabaseType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxDatabaseType.SelectedIndexChanged

        Select Case Me.ComboBoxDatabaseType.SelectedItem.Tag
            Case Is = pcenumDatabaseType.SQLite,
                      pcenumDatabaseType.MSJet
                Me.ButtonCreateDatabase.Visible = True
                Me.ButtonFileSelect.Visible = True
                Me.ButtonFileSelect.Enabled = True
                If Trim(Me.TextBoxDatabaseConnectionString.Text) = "" Then
                    Me.ButtonCreateDatabase.Enabled = True
                Else
                    Me.ButtonCreateDatabase.Enabled = False
                End If
            Case Else
                Me.ButtonCreateDatabase.Visible = False
                Me.ButtonCreateDatabase.Enabled = False
                Me.ButtonFileSelect.Visible = False
                Me.ButtonFileSelect.Enabled = False
        End Select

        Me.ButtonApply.Enabled = True


    End Sub

    Private Sub ButtonCreateDatabase_Click(sender As Object, e As EventArgs) Handles ButtonCreateDatabase.Click

        Dim lrSaveFileDialog As New SaveFileDialog()

        Select Case ComboBoxDatabaseType.SelectedItem.Tag
            Case Is = pcenumDatabaseType.SQLite
                lrSaveFileDialog.Filter = "SQLite database file (*.db)|*.db"
                lrSaveFileDialog.FilterIndex = 0
                lrSaveFileDialog.RestoreDirectory = True

                If (lrSaveFileDialog.ShowDialog() = DialogResult.OK) Then
                    If Not System.IO.File.Exists(lrSaveFileDialog.FileName()) Then
                        SQLiteConnection.CreateFile(lrSaveFileDialog.FileName)
                        Dim lsConnectionString = "Data Source=" & lrSaveFileDialog.FileName & ";Version=3;"
                        Me.TextBoxDatabaseConnectionString.Text = lsConnectionString
                        Me.ButtonCreateDatabase.Enabled = False
                    End If
                End If
            Case Is = pcenumDatabaseType.MSJet
                lrSaveFileDialog.Filter = "MSJet database file (*.mdb)|*.mdb"
                lrSaveFileDialog.FilterIndex = 0
                lrSaveFileDialog.RestoreDirectory = True

                If (lrSaveFileDialog.ShowDialog() = DialogResult.OK) Then
                    If Not System.IO.File.Exists(lrSaveFileDialog.FileName()) Then

                        Call System.IO.File.Copy(MyPath() & "\emptydatabases\emptymdbdatabase.mdb", lrSaveFileDialog.FileName)

                        Dim lsConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & lrSaveFileDialog.FileName

                        Me.TextBoxDatabaseConnectionString.Text = lsConnectionString
                        Me.ButtonCreateDatabase.Enabled = False
                    End If
                End If
        End Select

    End Sub

    Private Sub TextBoxDatabaseConnectionString_TextChanged(sender As Object, e As EventArgs) Handles TextBoxDatabaseConnectionString.TextChanged

        If Trim(Me.TextBoxDatabaseConnectionString.Text) = "" Then
            Select Case Me.ComboBoxDatabaseType.SelectedItem.Tag
                Case Is = pcenumDatabaseType.SQLite
                    Me.ButtonCreateDatabase.Visible = True
                    Me.ButtonCreateDatabase.Enabled = True
                    Me.ButtonFileSelect.Visible = True
            End Select
        End If

        Me.LabelOpenSuccessfull.Text = ""

        Me.ButtonApply.Enabled = True

    End Sub

    Private Sub ButtonFileSelect_Click(sender As Object, e As EventArgs) Handles ButtonFileSelect.Click

        Select Case Me.ComboBoxDatabaseType.SelectedItem.Tag
            Case Is = pcenumDatabaseType.SQLite
                Using lrOpenFileDialog As New OpenFileDialog

                    If lrOpenFileDialog.ShowDialog = DialogResult.OK Then
                        Dim lsReturnMessage As String = Nothing
                        Dim lsConnectionString = "Data Source=" & lrOpenFileDialog.FileName & ";Version=3;"
                        If Me.checkDatabaseConnectionString(lsReturnMessage, lsConnectionString) Then
                            Me.TextBoxDatabaseConnectionString.Text = lsConnectionString
                        Else
                            MsgBox("The file you selected is not a SQLite database.")
                        End If
                    End If

                End Using
            Case Is = pcenumDatabaseType.MSJet
                Using lrOpenFileDialog As New OpenFileDialog

                    If lrOpenFileDialog.ShowDialog = DialogResult.OK Then
                        Dim lsReturnMessage As String = Nothing
                        Dim lsConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & lrOpenFileDialog.FileName
                        If Me.checkDatabaseConnectionString(lsReturnMessage, lsConnectionString) Then
                            Me.TextBoxDatabaseConnectionString.Text = lsConnectionString
                        Else
                            MsgBox("The file you selected is not a MSJet database.")
                        End If
                    End If

                End Using
        End Select

    End Sub

    Private Sub ButtonApply_Click(sender As Object, e As EventArgs) Handles ButtonApply.Click

        Try
            If Me.check_fields() Then

                Me.zrModel.TargetDatabaseType = Me.ComboBoxDatabaseType.SelectedItem.Tag
                Me.zrModel.TargetDatabaseConnectionString = Trim(Me.TextBoxDatabaseConnectionString.Text)
                Me.zrModel.IsDatabaseSynchronised = Me.CheckBoxIsDatabaseSynchronised.Checked

                Me.zrModel.Server = Trim(Me.TextBoxServerName.Text)
                Me.zrModel.Database = Trim(Me.TextBoxDatabaseName.Text)
                Me.zrModel.Schema = Trim(Me.TextBoxSchemaName.Text)
                Me.zrModel.Warehouse = Trim(Me.TextBoxWarehouseName.Text)
                Me.zrModel.DatabaseRole = Trim(Me.TextBoxRoleName.Text)
                Me.zrModel.Port = Trim(Me.TextBoxPort.Text)

                Try
                    If Me.zrModel.TreeNode IsNot Nothing Then
                        If 1 = 1 Then 'MySettings.FactEngineShowDatabaseLogoInModelExplorer' Just do it for GraphSchemaJSON Editor.
                            Select Case Me.zrModel.TargetDatabaseType
                                Case Is = pcenumDatabaseType.MongoDB
                                    Me.zrModel.TreeNode.ImageIndex = 6
                                    Me.zrModel.TreeNode.SelectedImageIndex = 6
                                Case Is = pcenumDatabaseType.SQLServer
                                    Me.zrModel.TreeNode.ImageIndex = 9
                                    Me.zrModel.TreeNode.SelectedImageIndex = 9
                                Case Is = pcenumDatabaseType.MSJet
                                    Me.zrModel.TreeNode.ImageIndex = 7
                                    Me.zrModel.TreeNode.SelectedImageIndex = 7
                                Case Is = pcenumDatabaseType.SQLite
                                    Me.zrModel.TreeNode.ImageIndex = 8
                                    Me.zrModel.TreeNode.SelectedImageIndex = 8
                                Case Is = pcenumDatabaseType.SQLServer
                                    Me.zrModel.TreeNode.ImageIndex = 9
                                    Me.zrModel.TreeNode.SelectedImageIndex = 9
                                Case Is = pcenumDatabaseType.ODBC
                                    Me.zrModel.TreeNode.ImageIndex = 10
                                    Me.zrModel.TreeNode.SelectedImageIndex = 10
                                Case Is = pcenumDatabaseType.PostgreSQL
                                    Me.zrModel.TreeNode.ImageIndex = 11
                                    Me.zrModel.TreeNode.SelectedImageIndex = 11
                                Case Is = pcenumDatabaseType.Snowflake
                                    Me.zrModel.TreeNode.ImageIndex = 12
                                    Me.zrModel.TreeNode.SelectedImageIndex = 12
                                Case Is = pcenumDatabaseType.TypeDB
                                    Me.zrModel.TreeNode.ImageIndex = 13
                                    Me.zrModel.TreeNode.SelectedImageIndex = 13
                                Case Is = pcenumDatabaseType.Neo4j
                                    Me.zrModel.TreeNode.ImageIndex = 14
                                    Me.zrModel.TreeNode.SelectedImageIndex = 14
                                Case Is = pcenumDatabaseType.RelationalAI
                                    Me.zrModel.TreeNode.ImageIndex = 21
                                    Me.zrModel.TreeNode.SelectedImageIndex = 21
                                Case Is = pcenumDatabaseType.KuzuDB
                                    Me.zrModel.TreeNode.ImageIndex = 22
                                    Me.zrModel.TreeNode.SelectedImageIndex = 22
                                Case Is = pcenumDatabaseType.EdgeDB
                                    Me.zrModel.TreeNode.ImageIndex = 23
                                    Me.zrModel.TreeNode.SelectedImageIndex = 23
                                Case Is = pcenumDatabaseType.FactEngineAbstractionLayer
                                    Me.zrModel.TreeNode.ImageIndex = 10
                                    Me.zrModel.TreeNode.SelectedImageIndex = 10
                                Case Is = pcenumDatabaseType.None
                                    Me.zrModel.TreeNode.ImageIndex = 1
                                    Me.zrModel.TreeNode.SelectedImageIndex = 1
                            End Select
                        End If
                    End If
                Catch ex As Exception
                End Try

                Me.zrModel.Save()

            End If
            Me.ButtonApply.Enabled = False

        Catch ex As Exception

        End Try
    End Sub

    Private Sub zrModel_Deleting() Handles zrModel.Deleting

        Me.Hide()
        Me.Close()

    End Sub

    Private Sub BackgroundWorker_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles BackgroundWorker.ProgressChanged

        Try
            Me.ProgressBarReverseEngineering.Value = e.ProgressPercentage

            Dim lrProgressObject As ProgressObject = CType(e.UserState, ProgressObject)

            If lrProgressObject.Message IsNot Nothing Then
                If lrProgressObject.SimpleAppend Then
                    Me.RichTextBoxREMessages.AppendText(lrProgressObject.Message)

                ElseIf lrProgressObject.IsError Then
                    Me.RichTextBoxREErrorMessages.AppendStringInColor(vbCrLf & "- " & lrProgressObject.Message, Color.Orange)
                    Me.ErrorCount += 1

                    If Me.ErrorCount > 0 Then
                        Me.LabelPromptErrorMessages.Visible = True
                    End If
                Else
                    Me.RichTextBoxREMessages.AppendStringInColor(vbCrLf & "- " & lrProgressObject.Message, Color.Black)
                End If
            End If

            Me.RichTextBoxREMessages.ScrollToCaret()
            Me.Invalidate()

        Catch ex As Exception

        End Try

    End Sub

    ''' <summary>
    ''' 20220907-VM-Possible that Page data was not being saved when changed from StoreAsXML to StoreInDatabase. Fix below.
    ''' Call Me.zrModel.Save(True, True,True ) '20220907-VM-Was Save(True,False).
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CheckBoxSaveToXML_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxSaveToXML.CheckedChanged

        Try
            Dim lsMessage As String = ""
            If Me.CheckBoxSaveToXML.Checked Then
                lsMessage = "Are you sure you want to store the Model as an XML file? If the Model is already saved in the database it may take a moment to remove the Model from the database."
                If MsgBox(lsMessage, MsgBoxStyle.YesNoCancel) = MsgBoxResult.Yes Then

                    'CodeSafe - Make sure the Pages are loaded.
                    For Each lrPage In Me.zrModel.Page.Where(Function(page) Not page.Loaded).ToList()
                        Call lrPage.Load(False)
                    Next

                    With New WaitCursor
                        Me.zrModel.SetStoreAsXML(True, True)
                    End With
                Else
                    RemoveHandler Me.CheckBoxSaveToXML.CheckedChanged, AddressOf CheckBoxSaveToXML_CheckedChanged
                    Me.CheckBoxSaveToXML.Checked = False
                    AddHandler Me.CheckBoxSaveToXML.CheckedChanged, AddressOf CheckBoxSaveToXML_CheckedChanged
                    Me.zrModel.StoreAsXML = False
                End If
            Else
                lsMessage = "Are you sure you want to store the Model within the Boston database?"
                If MsgBox(lsMessage, MsgBoxStyle.YesNoCancel) = MsgBoxResult.Yes Then
                    With New WaitCursor
                        Try
                            Call Database.CompactAndRepairDatabase()
                            Call Me.zrModel.RapidEmpty(True)
                            For Each lrPage In Me.zrModel.Page
                                Call lrPage.Load(False)
                            Next
                            Me.zrModel.StoreAsXML = False
                            Call Me.zrModel.Save(True, True, True) '20220907-VM-Was Save(True,False).
                        Catch ex As Exception
                            lsMessage = "Couldn't successfully save the Model to the Boston database. The Model will remain stored as XML."
                            Me.zrModel.StoreAsXML = True
                            Call Me.zrModel.Save(False, False)
                            GoTo CouldntSaveToBostonDatabase
                        End Try
                        'Only remove the XML file if safely saved in the database.
                        Call Me.zrModel.RemoveCorrespondingXMLFile()
CouldntSaveToBostonDatabase:
                    End With
                Else
                    RemoveHandler Me.CheckBoxSaveToXML.CheckedChanged, AddressOf CheckBoxSaveToXML_CheckedChanged
                    Me.CheckBoxSaveToXML.Checked = True
                    RemoveHandler Me.CheckBoxSaveToXML.CheckedChanged, AddressOf CheckBoxSaveToXML_CheckedChanged
                    Me.zrModel.StoreAsXML = True
                End If
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Sub ButtonReplaceDatabaseModel_Click(sender As Object, e As EventArgs) Handles ButtonReplaceDatabaseModel.Click

        Try
            Dim lsMessage As String

            lsMessage = "Are you sure you want to replace the database copy of the Model?"
            lsMessage.AppendDoubleLineBreak("The Model is currently stored as XML. If you proceed a copy of the Model (in its current state) will be made to the Boston database.")
            lsMessage.AppendDoubleLineBreak("There is no inherent risk in this action and copying the Model to the Boston database will give you a backup of the Model.")

            If MsgBox(lsMessage, MsgBoxStyle.Information + MsgBoxStyle.YesNoCancel) = DialogResult.Yes Then

                With New WaitCursor
                    If Not Me.zrModel.Loaded Then Me.zrModel.Load(True, True, Nothing)
                    Me.zrModel.RapidEmpty(True)
                    Me.zrModel.Save(True, True, True)
                End With

            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Sub ButtonCopyModelIdToClipboard_Click(sender As Object, e As EventArgs) Handles ButtonCopyModelIdToClipboard.Click

        Try
            System.Windows.Forms.Clipboard.SetText(Me.zrModel.ModelId)

            Dim lfrmFlashCard As New frmFlashCard
            lfrmFlashCard.ziIntervalMilliseconds = 1500
            lfrmFlashCard.zsText = "Saved to clipboard."
            lfrmFlashCard.Show(frmMain)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Sub zrModel_StoreAsXMLChanged(abStoreAsXML As Boolean) Handles zrModel.StoreAsXMLChanged

        Try
            Me.CheckBoxSaveToXML.Checked = abStoreAsXML

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub
End Class