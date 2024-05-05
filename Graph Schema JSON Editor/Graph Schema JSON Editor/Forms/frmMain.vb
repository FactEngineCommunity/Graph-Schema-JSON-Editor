Imports FactEngineForServices

Public Class frmMain
    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles Me.Load

        Call Me.SetupForm()

    End Sub

    Private Sub SetupForm()

        'Expand the TreeView
        Me.TreeView.ExpandAll()

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
    ''' Add Relationship to Schema
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub AddRelationshipToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles AddRelationshipToolStripMenuItem1.Click

        Dim loTreeNode As TreeNode = Me.TreeView.SelectedNode

        Dim loRelationshipTreeNode As New TreeNode("(Node 1)-[:RELATES_TO]->(Node 2)", 2, 2)
        loRelationshipTreeNode.Tag = Nothing 'No RelationalDataStructure Relationship at this stage.

        loTreeNode.Nodes.Add(loRelationshipTreeNode)

        Dim lfrmCRUDAddEditPGSRelationship As New frmCRUDAddEditRelationship
        lfrmCRUDAddEditPGSRelationship.ShowDialog()

    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click

        Me.Hide()
        Me.Close()
        Me.Dispose()

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

            Dim lrNewPropertyTreeNode = New TreeNode(lrFBMValueType.Name, 4, 4)
            Me.TreeView.SelectedNode.Parent.Nodes(0).Nodes.Add(lrNewPropertyTreeNode)
            Me.TreeView.SelectedNode.Parent.Nodes(0).Expand()

        Catch ex As Exception

        End Try

    End Sub


End Class
