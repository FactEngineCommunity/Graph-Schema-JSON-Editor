﻿Imports FactEngineForServices

Public Class frmMain
    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles Me.Load

        Call Me.SetupForm()

    End Sub

    Private Sub SetupForm()

        'Create a new FBM (FactBasedModel) to store the ObjectTypes and Relationships (FactTypes) for the Nodes/Relationships of the Model.
        Dim lrFBMModel As New FBM.Model("MyJSONGraphSchema", System.Guid.NewGuid.ToString)

        'Create a new RDS (RelationalDataStructure) Model to store Nodes/Table in.
        Dim lrRDSModel = New FactEngineForServices.RDS.Model(lrFBMModel)

        'Store the RDS Model against the "Schema" TreeNode, so we can add Nodes/Tables to it.
        Me.TreeView.Nodes(0).Tag = lrRDSModel

    End Sub

    ''' <summary>
    ''' ContextMenuStrip management.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TreeView_MouseUp(sender As Object, e As MouseEventArgs) Handles TreeView.MouseUp

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

                End If

            End If
        End If


    End Sub

    ''' <summary>
    ''' Add Node to Schema
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub AddNodeToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles AddNodeToolStripMenuItem1.Click

        Dim loTreeNode As TreeNode = Me.TreeView.SelectedNode

        loTreeNode.Nodes.Add(New TreeNode("(New Node)"))

#Region "RDS (RelationalDataStructure) management"

        'Get the RelationalDataStructure
        Dim lrRDSModel As RDS.Model = Me.TreeView.Nodes(0).Tag

        '--------------------------------------------------------------
        'Create an EntityType/ObjectType for the FBM (FactBasedModel)
        Dim lrEntityType As New FBM.EntityType(lrRDSModel.Model, pcenumLanguage.ORMModel, "New Node")

        '---------------------------------------------------------
        'Add the Node/Table to the RDS (RelationalDataStructure)
        Dim lrTable As New RDS.Table(Me.TreeView.Nodes(0).Tag, lrEntityType.Name, lrEntityType)
        loTreeNode.Tag = lrTable
#End Region

    End Sub

    ''' <summary>
    ''' Add Relationship to Schema
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub AddRelationshipToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles AddRelationshipToolStripMenuItem1.Click

        Dim loTreeNode As TreeNode = Me.TreeView.SelectedNode

        Dim loRelationshipTreeNode As New TreeNode("(Node 1)-[:RELATES_TO]->(Node 2)")
        loRelationshipTreeNode.Tag = Nothing 'No RelationalDataStructure Relationship at this stage.

        loTreeNode.Nodes.Add(loRelationshipTreeNode)

    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click

        Me.Hide()
        Me.Close()
        Me.Dispose()

    End Sub

End Class
