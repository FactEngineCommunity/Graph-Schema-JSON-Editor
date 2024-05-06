﻿Imports FactEngineForServices
Imports System.Text.RegularExpressions

Public Class frmMain

    Private mrFBMModel As New FBM.Model("MyJSONGraphSchema", System.Guid.NewGuid.ToString)

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles Me.Load

        Me.mrFBMModel.AddCore()
        Me.mrFBMModel.RDSCreated = True

        Call Me.SetupForm()

    End Sub

    Private Sub SetupForm()

        'Expand the TreeView
        Me.TreeView.ExpandAll()

        'Create a new FBM (FactBasedModel) to store the ObjectTypes and Relationships (FactTypes) for the Nodes/Relationships of the Model.
        Dim lrFBMModel = Me.mrFBMModel

        'Create a new RDS (RelationalDataStructure) Model to store Nodes/Table in.
        Me.mrFBMModel.RDS = New FactEngineForServices.RDS.Model(lrFBMModel)
        Dim lrRDSModel = Me.mrFBMModel.RDS

        'Store the RDS Model against the "Schema" TreeNode, so we can add Nodes/Tables to it.
        Me.TreeView.Nodes(0).Tag = lrRDSModel

    End Sub

    ''' <summary>
    ''' ContextMenuStrip management.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TreeView_MouseUp(sender As Object, e As MouseEventArgs) Handles TreeView.MouseUp

        Try
            If e.Button = MouseButtons.Right Then
                ' Determine the node at the position of the mouse cursor
                Dim nodeAtMousePosition As TreeNode = TreeView.GetNodeAt(e.X, e.Y)

                ' Select this node
                If nodeAtMousePosition IsNot Nothing And e.Button = MouseButtons.Right Then
                    TreeView.SelectedNode = nodeAtMousePosition

                    '=====================================================================
                    'Allocate the appropriate ContextMenuStrip to the TreeView/TreeNode.
                    If TreeView.SelectedNode.Name = "Schema" Then

                        ContextMenuStripSchema.Show(TreeView, e.Location)

                    ElseIf TreeView.SelectedNode.Name = "Nodes" Then

                        ContextMenuStripNodes.Show(TreeView, e.Location)

                    ElseIf TreeView.SelectedNode.Name = "Relationships" Then

                        ContextMenuStripRelationships.Show(TreeView, e.Location)

                    Else
                        Select Case TreeView.SelectedNode.Tag.GetType
                            Case Is = GetType(RDS.Table)

                                ContextMenuStripNode.Show(TreeView, e.Location)

                            Case Is = GetType(RDS.Column)

                                ContextMenuStripProperty.Show(TreeView, e.Location)

                            Case Is = GetType(tSchemaTreeMenuType)

                                Select Case CType(TreeView.SelectedNode.Tag, tSchemaTreeMenuType).MenuType

                                    Case Is = pcenumSchemaTreeMenuType.Properties

                                        ContextMenuStripProperties.Show(TreeView, e.Location)

                                End Select


                        End Select

                    End If

                End If
            End If

        Catch ex As Exception



        End Try

    End Sub

    ''' <summary>
    ''' Add Node to Schema
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub AddNodeToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles AddNodeToolStripMenuItem1.Click

        Dim loTreeNode As TreeNode = Me.TreeView.SelectedNode

        Dim loNewNodeTreeNode = New TreeNode("(New Node)", 1, 1)
        loTreeNode.Nodes.Add(loNewNodeTreeNode)

#Region "RDS (RelationalDataStructure) management"

        'Get the RelationalDataStructure
        Dim lrRDSModel As RDS.Model = Me.TreeView.Nodes(0).Tag

        '--------------------------------------------------------------
        'Create an EntityType/ObjectType for the FBM (FactBasedModel)
        Dim lrEntityType As New FBM.EntityType(lrRDSModel.Model, pcenumLanguage.ORMModel, "New Node")

        '---------------------------------------------------------
        'Add the Node/Table to the RDS (RelationalDataStructure)
        Dim lrRDSTable As New RDS.Table(Me.TreeView.Nodes(0).Tag, lrEntityType.Name, lrEntityType)
        loNewNodeTreeNode.Tag = lrRDSTable
#End Region

        loTreeNode.Expand()

        Dim loPropertiesTreeNode = New TreeNode("Properties", 4, 4)
        loPropertiesTreeNode.Tag = New tSchemaTreeMenuType(pcenumSchemaTreeMenuType.Properties)
        loNewNodeTreeNode.Nodes.Add(loPropertiesTreeNode)
        loNewNodeTreeNode.Expand()

    End Sub

    ''' <summary>
    ''' Add Node to Schema
    ''' </summary>
    Private Sub AddNodeToTreeView(ByRef arRDSTable As RDS.Table)

        Dim loTreeNode As TreeNode = Me.TreeView.Nodes(0).Nodes(0)

        Dim loNewNodeTreeNode = New TreeNode(arRDSTable.Name, 1, 1)
        loTreeNode.Nodes.Add(loNewNodeTreeNode)
        loNewNodeTreeNode.Tag = arRDSTable
        loTreeNode.Expand()

        Dim loPropertiesTreeNode = New TreeNode("Properties", 4, 4)
        loPropertiesTreeNode.Tag = New tSchemaTreeMenuType(pcenumSchemaTreeMenuType.Properties)
        loNewNodeTreeNode.Nodes.Add(loPropertiesTreeNode)

        '==========================================================
        'Properties
        For Each lrRDSColumn In arRDSTable.Column

            Dim lsPropertyEmbellishment = lrRDSColumn.Name & " { ""type"": """ & If(lrRDSColumn.DataType Is Nothing, "string", lrRDSColumn.DataType.DataType) & """, ""nullable"": """ & LCase(lrRDSColumn.IsNullable.ToString) & """}"

            Dim lrNewPropertyTreeNode = New TreeNode(lsPropertyEmbellishment, 4, 4)
            lrNewPropertyTreeNode.Tag = lrRDSColumn
            loPropertiesTreeNode.Nodes.Add(lrNewPropertyTreeNode)
        Next

    End Sub


    ''' <summary>
    ''' Add Relationship to Schema
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub AddRelationshipToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles AddRelationshipToolStripMenuItem1.Click

        Dim loTreeNode As TreeNode = Me.TreeView.SelectedNode

        Dim lrModel As FBM.Model = loTreeNode.Parent.Tag.Model

        Dim lfrmCRUDAddEditPGSRelationship As New frmCRUDAddEditRelationship
        lfrmCRUDAddEditPGSRelationship.mrRDSModel = lrModel.RDS
        Dim lrPGSRelationship = New JGS.RelationshipObjectType
        lrPGSRelationship.From.Ref = "Node Type 1"
        lrPGSRelationship.Type.Ref = "RELATES_TO"
        lrPGSRelationship.To.Ref = "Node Type 2"
        lfrmCRUDAddEditPGSRelationship.mrPGSRelationship = lrPGSRelationship

        If lfrmCRUDAddEditPGSRelationship.ShowDialog() = DialogResult.OK Then

            Dim lsFromModelElementName = lfrmCRUDAddEditPGSRelationship.mrPGSRelationship.From.Ref
            Dim lsToModelElementName = lfrmCRUDAddEditPGSRelationship.mrPGSRelationship.To.Ref
            Dim lsGraphLabel = lfrmCRUDAddEditPGSRelationship.mrPGSRelationship.Type.Ref

            Dim loRelationshipTreeNode As New TreeNode($"({lsFromModelElementName})-[:{lsGraphLabel}]->({lsToModelElementName})", 2, 2)
            loRelationshipTreeNode.Tag = lfrmCRUDAddEditPGSRelationship.mrPGSRelationship

            loTreeNode.Nodes.Add(loRelationshipTreeNode)
            loTreeNode.Expand()

        End If

    End Sub

    ''' <summary>
    ''' Add Relationship to Schema
    ''' </summary>
    Private Sub AddRelationshipToTreeView(ByRef arRDSRelationship As RDS.Relation)

        Dim loTreeNode As TreeNode = Me.TreeView.Nodes(0).Nodes(1)

        Dim lrModel As FBM.Model = loTreeNode.Parent.Tag.Model


        Dim lsFromModelElementName = arRDSRelationship.OriginTable.Name
        Dim lsToModelElementName = arRDSRelationship.DestinationTable.Name
        Dim lsGraphLabel = "HAS"

        Dim loRelationshipTreeNode As New TreeNode($"({lsFromModelElementName})-[:{lsGraphLabel}]->({lsToModelElementName})", 2, 2)
        loRelationshipTreeNode.Tag = arRDSRelationship

        loTreeNode.Nodes.Add(loRelationshipTreeNode)

    End Sub


    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click

        Me.Hide()
        Me.Close()
        Me.Dispose()

    End Sub

    Private Sub DrawCustomText(graphics As Graphics, text As String, font As Font, startAt As Point, patterns As Dictionary(Of String, Color))
        Dim currentPosition As Integer = 0
        Dim drawPositionX As Integer = startAt.X

        ' Clear the area to prevent shadow effect
        graphics.FillRectangle(Brushes.White, New Rectangle(startAt, New Size(text.Length * font.Size, font.Height)))

        ' Prepare to handle each segment of text
        Dim segments As New List(Of Tuple(Of String, Color))
        Dim regex As New Regex(String.Join("|", patterns.Keys))

        ' Identify all matches
        For Each match As Match In regex.Matches(text)
            If match.Index > currentPosition Then
                ' Add preceding black text
                segments.Add(Tuple.Create(text.Substring(currentPosition, match.Index - currentPosition), Color.Black))
            End If
            ' Add colored text
            segments.Add(Tuple.Create(match.Value, patterns.FirstOrDefault(Function(p) New Regex(p.Key).IsMatch(match.Value)).Value))
            currentPosition = match.Index + match.Length
        Next

        ' Add the remaining text if any
        If currentPosition < text.Length Then
            segments.Add(Tuple.Create(text.Substring(currentPosition), Color.Black))
        End If

        ' Draw each segment
        For Each segment In segments
            Using brush As New SolidBrush(segment.Item2)
                graphics.DrawString(segment.Item1, font, brush, New PointF(drawPositionX, startAt.Y))
                ' Measure text width precisely to update position
                drawPositionX += graphics.MeasureString(segment.Item1, font).Width - 2 'Was TextRendered.MeasureString
            End Using
        Next
    End Sub


    'Private Sub DrawCustomText(graphics As Graphics, text As String, font As Font, startAt As Point, patterns As Dictionary(Of String, Color))
    '    Dim currentPosition As Integer = 0
    '    Dim drawPositionX As Integer = startAt.X

    '    ' Clear the area to prevent shadow effect
    '    graphics.FillRectangle(Brushes.White, New Rectangle(startAt, New Size(text.Length * font.Size, font.Height)))

    '    ' Prepare to handle each segment of text
    '    Dim segments As New List(Of Tuple(Of String, Color))
    '    Dim regex As New Regex(String.Join("|", patterns.Keys))

    '    ' Identify all matches
    '    For Each match As Match In regex.Matches(text)
    '        If match.Index > currentPosition Then
    '            ' Add preceding black text
    '            segments.Add(Tuple.Create(text.Substring(currentPosition, match.Index - currentPosition), Color.Black))
    '        End If
    '        ' Add colored text
    '        segments.Add(Tuple.Create(match.Value, patterns.FirstOrDefault(Function(p) New Regex(p.Key).IsMatch(match.Value)).Value))
    '        currentPosition = match.Index + match.Length
    '    Next

    '    ' Add the remaining text if any
    '    If currentPosition < text.Length Then
    '        segments.Add(Tuple.Create(text.Substring(currentPosition), Color.Black))
    '    End If

    '    ' Draw each segment
    '    For Each segment In segments
    '        Using brush As New SolidBrush(segment.Item2)
    '            graphics.DrawString(segment.Item1, font, brush, drawPositionX, startAt.Y)
    '            drawPositionX += TextRenderer.MeasureText(graphics, segment.Item1, font).Width
    '        End Using
    '    Next
    'End Sub

    Private Sub TreeView1_DrawNode(sender As Object, e As DrawTreeNodeEventArgs) Handles TreeView.DrawNode
        'e.DrawDefault = False  ' Prevent default drawing

        'Dim patterns As New Dictionary(Of String, Color) From {
        '{"\([^\)]*\)", Color.Purple},     ' Text in parentheses in purple
        '{"\[:[^\]]*\]", Color.DarkGreen}  ' Text between "[: " and "]" in dark green
        '}

        '' Draw the node text with custom coloring
        'DrawCustomText(e.Graphics, e.Node.Text, Me.TreeView.Font, New Point(e.Bounds.X, e.Bounds.Y), patterns)

        e.DrawDefault = False  ' Prevent default drawing

        ' Define the drawing patterns and corresponding colors
        Dim patterns As New Dictionary(Of String, Color) From {
            {"\([^\)]*\)", Color.Purple},     ' Text in parentheses in purple
            {"\[:[^\]]*\]", Color.DarkGreen}, ' Text between "[: " and "]" in dark green
            {"""[a-z]*"":", Color.Salmon},
            {"""[a-z]*""", Color.SteelBlue}
        }

        ' Set a clipping region to constrain drawing to the node bounds
        Dim savedClip As Region = e.Graphics.Clip
        Dim loSize = New Size(e.Bounds.Size.Width + 28, e.Bounds.Size.Height)
        e.Graphics.SetClip(New Rectangle(e.Bounds.Location, loSize))

        ' Draw the node text with custom coloring
        DrawCustomText(e.Graphics, e.Node.Text, Me.TreeView.Font, New Point(e.Bounds.X, e.Bounds.Y), patterns)

        ' Restore the original clipping region
        e.Graphics.Clip = savedClip
    End Sub

    Private Sub PropertiesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PropertiesToolStripMenuItem.Click

        Dim lrRDSTable As RDS.Table = Me.TreeView.SelectedNode.Tag

        Dim lrFBMModel As FBM.Model = lrRDSTable.Model.Model

        Dim lrFBMValueType As New FBM.ValueType(lrRDSTable.Model.Model, pcenumLanguage.ORMModel, "New Property", True)

        Dim larFBMModelElement As New List(Of FBM.ModelObject)
        larFBMModelElement.Add(lrRDSTable.FBMModelElement)
        larFBMModelElement.Add(lrFBMValueType)

        Dim lsNewFactTypeName = larFBMModelElement(0).Id & "Has" & larFBMModelElement(1).Id
        Dim lrFBMFactType = lrRDSTable.Model.Model.CreateFactType(lsNewFactTypeName, larFBMModelElement, False, True, False, Nothing, True, Nothing, True, False)

        Dim lrRDSColumn As New RDS.Column(lrRDSTable, "New Property", lrFBMFactType.RoleGroup(0), lrFBMFactType.RoleGroup(1), False)

        Dim lrNewPropertyTreeNode = New TreeNode(lrFBMValueType.Name, 4, 4)
        Me.TreeView.SelectedNode.Nodes(0).Nodes.Add(lrNewPropertyTreeNode)
        Me.TreeView.SelectedNode.Nodes(0).Expand()

    End Sub

    Private Sub AddPropertyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddPropertyToolStripMenuItem.Click

        Try
            Dim lrRDSTable As RDS.Table = Me.TreeView.SelectedNode.Parent.Tag

            Dim lrFBMModel As FBM.Model = lrRDSTable.Model.Model

            Dim lrFBMValueType As New FBM.ValueType(lrRDSTable.Model.Model, pcenumLanguage.ORMModel, "New Property", True)

            Dim larFBMModelElement As New List(Of FBM.ModelObject)
            larFBMModelElement.Add(lrRDSTable.FBMModelElement)
            larFBMModelElement.Add(lrFBMValueType)

            Dim lsNewFactTypeName = larFBMModelElement(0).Id & "Has" & larFBMModelElement(1).Id
            Dim lrFBMFactType = lrRDSTable.Model.Model.CreateFactType(lsNewFactTypeName, larFBMModelElement, False, True, False, Nothing, True, Nothing, True, False)

            Dim lrRDSColumn As New RDS.Column(lrRDSTable, "New Property", lrFBMFactType.RoleGroup(0), lrFBMFactType.RoleGroup(1), False)

            Dim lsPropertyEmbellishment = lrFBMValueType.Name & " { ""type"": """ & If(lrRDSColumn.DataType Is Nothing, "string", lrRDSColumn.DataType.DataType) & """, ""nullable"": """ & LCase(lrRDSColumn.IsNullable.ToString) & """}"

            Dim lrNewPropertyTreeNode = New TreeNode(lsPropertyEmbellishment, 4, 4)
            lrNewPropertyTreeNode.Tag = lrRDSColumn
            Me.TreeView.SelectedNode.Parent.Nodes(0).Nodes.Add(lrNewPropertyTreeNode)
            Me.TreeView.SelectedNode.Parent.Nodes(0).Expand()

        Catch ex As Exception

        End Try

    End Sub

    Private Sub AddPropertyToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles AddPropertyToolStripMenuItem1.Click

        Try
            Dim lfrmCRUDAddEditPGSProperty = New frmCRUDAddEditProperty

            If lfrmCRUDAddEditPGSProperty.ShowDialog = DialogResult.OK Then


            End If


        Catch ex As Exception

        End Try

    End Sub

    Private Sub SQLiteConnectToToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SQLiteConnectToToolStripMenuItem.Click

        Try
            Dim lrOpenFileDialog As New OpenFileDialog

            lrOpenFileDialog.Filter = "SQLite database file (*.db)|*.db"
            lrOpenFileDialog.FilterIndex = 0
            lrOpenFileDialog.RestoreDirectory = True

            Dim lsConnectionString As String = ""

            If (lrOpenFileDialog.ShowDialog() = DialogResult.OK) Then
                If System.IO.File.Exists(lrOpenFileDialog.FileName()) Then
                    lsConnectionString = "Data Source=" & lrOpenFileDialog.FileName & ";Version=3;"
                    Me.ToolStripLabelDatabaseName.Text = lrOpenFileDialog.FileName
                    Me.ToolStripLabelPromptSourceDatabase.Visible = True
                    Me.ToolStripLabelDatabaseName.Visible = True

                    Me.mrFBMModel.DatabaseConnectionString = lsConnectionString
                    Me.mrFBMModel.TargetDatabaseType = pcenumDatabaseType.SQLite
                    Me.mrFBMModel.TargetDatabaseConnectionString = lsConnectionString

                    Me.mrFBMModel.DatabaseManager.establishConnection(Me.mrFBMModel.TargetDatabaseType, Me.mrFBMModel.TargetDatabaseType)

                    Call Me.mrFBMModel.connectToDatabase()

                    Dim lrBackgroundWorker As New System.ComponentModel.BackgroundWorker()
                    lrBackgroundWorker.WorkerReportsProgress = True
                    Dim lrReverseEngineerTool As New ODBCDatabaseReverseEngineer(Me.mrFBMModel, lsConnectionString, False, lrBackgroundWorker)

                    pbReverseEngineeringKeepDatabaseColumnNames = True

                    Dim lsErrorMessage As String = ""
                    Call lrReverseEngineerTool.ReverseEngineerDatabase(lsErrorMessage)


                    For Each lrRDSTable In Me.mrFBMModel.RDS.Table
                        Call Me.AddNodeToTreeView(lrRDSTable)
                    Next

                    For Each lrRDSRelationship In Me.mrFBMModel.RDS.Relation
                        Call Me.AddRelationshipToTreeView(lrRDSRelationship)
                    Next

                End If
            End If

        Catch ex As Exception

        End Try
    End Sub
End Class