﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSchema
    Inherits WeifenLuo.WinFormsUI.Docking.DockContent

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim TreeNode1 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Nodes", 1, 1)
        Dim TreeNode2 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Relationships", 2, 2)
        Dim TreeNode3 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Schema", New System.Windows.Forms.TreeNode() {TreeNode1, TreeNode2})
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSchema))
        Me.GroupBoxMain = New System.Windows.Forms.GroupBox()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.TreeView = New System.Windows.Forms.TreeView()
        Me.ImageListMain = New System.Windows.Forms.ImageList(Me.components)
        Me.ToolStripMain = New System.Windows.Forms.ToolStrip()
        Me.ToolStripLabelPromptSourceDatabase = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripLabelDatabaseName = New System.Windows.Forms.ToolStripLabel()
        Me.ContextMenuStripSchemas = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AddSchemaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStripSchema = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.RelationshipsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ContextMenuStripNode = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.EditNodeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteNodeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStripProperty = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AddPropertyToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeletePropertyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStripNodes = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ContextMenuStripRelationships = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ContextMenuStripRelationship = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ContextMenuStripProperties = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AddPropertyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripButtonSave = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripDropDownButton1 = New System.Windows.Forms.ToolStripDropDownButton()
        Me.SourceDatabaseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SQLiteConnectToToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.NodesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddNodeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddRelationshipToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EportToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AsJSONGraphSchemaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PropertiesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddNodeToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddRelationshipToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemRelationshipAddProperty = New System.Windows.Forms.ToolStripMenuItem()
        Me.GroupBoxMain.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.ToolStripMain.SuspendLayout()
        Me.ContextMenuStripSchemas.SuspendLayout()
        Me.ContextMenuStripSchema.SuspendLayout()
        Me.ContextMenuStripNode.SuspendLayout()
        Me.ContextMenuStripProperty.SuspendLayout()
        Me.ContextMenuStripNodes.SuspendLayout()
        Me.ContextMenuStripRelationships.SuspendLayout()
        Me.ContextMenuStripRelationship.SuspendLayout()
        Me.ContextMenuStripProperties.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBoxMain
        '
        Me.GroupBoxMain.Controls.Add(Me.TableLayoutPanel1)
        Me.GroupBoxMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBoxMain.Location = New System.Drawing.Point(0, 0)
        Me.GroupBoxMain.Name = "GroupBoxMain"
        Me.GroupBoxMain.Size = New System.Drawing.Size(967, 500)
        Me.GroupBoxMain.TabIndex = 1
        Me.GroupBoxMain.TabStop = False
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.TreeView, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.ToolStripMain, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 16)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(961, 481)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'TreeView
        '
        Me.TreeView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TreeView.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText
        Me.TreeView.ImageIndex = 0
        Me.TreeView.ImageList = Me.ImageListMain
        Me.TreeView.Location = New System.Drawing.Point(3, 33)
        Me.TreeView.Name = "TreeView"
        TreeNode1.ImageIndex = 1
        TreeNode1.Name = "Nodes"
        TreeNode1.SelectedImageIndex = 1
        TreeNode1.Text = "Nodes"
        TreeNode2.ImageIndex = 2
        TreeNode2.Name = "Relationships"
        TreeNode2.SelectedImageIndex = 2
        TreeNode2.Text = "Relationships"
        TreeNode3.Name = "Schema"
        TreeNode3.Text = "Schema"
        Me.TreeView.Nodes.AddRange(New System.Windows.Forms.TreeNode() {TreeNode3})
        Me.TreeView.SelectedImageIndex = 0
        Me.TreeView.Size = New System.Drawing.Size(955, 445)
        Me.TreeView.TabIndex = 0
        '
        'ImageListMain
        '
        Me.ImageListMain.ImageStream = CType(resources.GetObject("ImageListMain.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageListMain.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageListMain.Images.SetKeyName(0, "PGS16x16.png")
        Me.ImageListMain.Images.SetKeyName(1, "PGSNode.png")
        Me.ImageListMain.Images.SetKeyName(2, "Relationship16x16.png")
        Me.ImageListMain.Images.SetKeyName(3, "AddAttribute16x16.png")
        Me.ImageListMain.Images.SetKeyName(4, "Attribute.png")
        Me.ImageListMain.Images.SetKeyName(5, "Attribute-PrimaryKey.png")
        Me.ImageListMain.Images.SetKeyName(6, "Index.png")
        '
        'ToolStripMain
        '
        Me.ToolStripMain.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ToolStripMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButtonSave, Me.ToolStripDropDownButton1, Me.ToolStripLabelPromptSourceDatabase, Me.ToolStripLabelDatabaseName})
        Me.ToolStripMain.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripMain.Name = "ToolStripMain"
        Me.ToolStripMain.Size = New System.Drawing.Size(961, 25)
        Me.ToolStripMain.TabIndex = 1
        Me.ToolStripMain.Text = "ToolStrip1"
        '
        'ToolStripLabelPromptSourceDatabase
        '
        Me.ToolStripLabelPromptSourceDatabase.Name = "ToolStripLabelPromptSourceDatabase"
        Me.ToolStripLabelPromptSourceDatabase.Size = New System.Drawing.Size(97, 22)
        Me.ToolStripLabelPromptSourceDatabase.Text = "Source Database:"
        Me.ToolStripLabelPromptSourceDatabase.Visible = False
        '
        'ToolStripLabelDatabaseName
        '
        Me.ToolStripLabelDatabaseName.Name = "ToolStripLabelDatabaseName"
        Me.ToolStripLabelDatabaseName.Size = New System.Drawing.Size(106, 22)
        Me.ToolStripLabelDatabaseName.Text = "<Database Name>"
        Me.ToolStripLabelDatabaseName.Visible = False
        '
        'ContextMenuStripSchemas
        '
        Me.ContextMenuStripSchemas.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ContextMenuStripSchemas.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddSchemaToolStripMenuItem})
        Me.ContextMenuStripSchemas.Name = "ContextMenuStripSchemas"
        Me.ContextMenuStripSchemas.Size = New System.Drawing.Size(142, 26)
        '
        'AddSchemaToolStripMenuItem
        '
        Me.AddSchemaToolStripMenuItem.Name = "AddSchemaToolStripMenuItem"
        Me.AddSchemaToolStripMenuItem.Size = New System.Drawing.Size(141, 22)
        Me.AddSchemaToolStripMenuItem.Text = "&Add Schema"
        '
        'ContextMenuStripSchema
        '
        Me.ContextMenuStripSchema.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ContextMenuStripSchema.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NodesToolStripMenuItem, Me.RelationshipsToolStripMenuItem, Me.ToolStripSeparator1, Me.EportToolStripMenuItem})
        Me.ContextMenuStripSchema.Name = "ContextMenuStripSchema"
        Me.ContextMenuStripSchema.Size = New System.Drawing.Size(145, 76)
        '
        'RelationshipsToolStripMenuItem
        '
        Me.RelationshipsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddRelationshipToolStripMenuItem})
        Me.RelationshipsToolStripMenuItem.Image = Global.JSON_Graph_Schema_Editor.My.Resources.Resources.Relationships16x16
        Me.RelationshipsToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.RelationshipsToolStripMenuItem.Name = "RelationshipsToolStripMenuItem"
        Me.RelationshipsToolStripMenuItem.Size = New System.Drawing.Size(144, 22)
        Me.RelationshipsToolStripMenuItem.Text = "&Relationships"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(141, 6)
        '
        'ContextMenuStripNode
        '
        Me.ContextMenuStripNode.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ContextMenuStripNode.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PropertiesToolStripMenuItem, Me.ToolStripSeparator4, Me.EditNodeToolStripMenuItem, Me.DeleteNodeToolStripMenuItem})
        Me.ContextMenuStripNode.Name = "ContextMenuStripNode"
        Me.ContextMenuStripNode.Size = New System.Drawing.Size(149, 88)
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(145, 6)
        '
        'EditNodeToolStripMenuItem
        '
        Me.EditNodeToolStripMenuItem.Name = "EditNodeToolStripMenuItem"
        Me.EditNodeToolStripMenuItem.Size = New System.Drawing.Size(148, 26)
        Me.EditNodeToolStripMenuItem.Text = "&Edit Node"
        '
        'DeleteNodeToolStripMenuItem
        '
        Me.DeleteNodeToolStripMenuItem.Name = "DeleteNodeToolStripMenuItem"
        Me.DeleteNodeToolStripMenuItem.Size = New System.Drawing.Size(148, 26)
        Me.DeleteNodeToolStripMenuItem.Text = "&Delete Node"
        '
        'ContextMenuStripProperty
        '
        Me.ContextMenuStripProperty.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ContextMenuStripProperty.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddPropertyToolStripMenuItem1, Me.DeletePropertyToolStripMenuItem})
        Me.ContextMenuStripProperty.Name = "ContextMenuStripProperty"
        Me.ContextMenuStripProperty.Size = New System.Drawing.Size(156, 48)
        '
        'AddPropertyToolStripMenuItem1
        '
        Me.AddPropertyToolStripMenuItem1.Name = "AddPropertyToolStripMenuItem1"
        Me.AddPropertyToolStripMenuItem1.Size = New System.Drawing.Size(155, 22)
        Me.AddPropertyToolStripMenuItem1.Text = "&Edit Property"
        '
        'DeletePropertyToolStripMenuItem
        '
        Me.DeletePropertyToolStripMenuItem.Name = "DeletePropertyToolStripMenuItem"
        Me.DeletePropertyToolStripMenuItem.Size = New System.Drawing.Size(155, 22)
        Me.DeletePropertyToolStripMenuItem.Text = "&Delete Property"
        '
        'ContextMenuStripNodes
        '
        Me.ContextMenuStripNodes.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ContextMenuStripNodes.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddNodeToolStripMenuItem1})
        Me.ContextMenuStripNodes.Name = "ContextMenuStripNodes"
        Me.ContextMenuStripNodes.Size = New System.Drawing.Size(129, 26)
        '
        'ContextMenuStripRelationships
        '
        Me.ContextMenuStripRelationships.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ContextMenuStripRelationships.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddRelationshipToolStripMenuItem1})
        Me.ContextMenuStripRelationships.Name = "ContextMenuStripRelationships"
        Me.ContextMenuStripRelationships.Size = New System.Drawing.Size(165, 26)
        '
        'ContextMenuStripRelationship
        '
        Me.ContextMenuStripRelationship.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ContextMenuStripRelationship.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemRelationshipAddProperty})
        Me.ContextMenuStripRelationship.Name = "ContextMenuStripRelationship"
        Me.ContextMenuStripRelationship.Size = New System.Drawing.Size(149, 30)
        '
        'ContextMenuStripProperties
        '
        Me.ContextMenuStripProperties.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ContextMenuStripProperties.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddPropertyToolStripMenuItem})
        Me.ContextMenuStripProperties.Name = "ContextMenuStripProperties"
        Me.ContextMenuStripProperties.Size = New System.Drawing.Size(145, 26)
        '
        'AddPropertyToolStripMenuItem
        '
        Me.AddPropertyToolStripMenuItem.Name = "AddPropertyToolStripMenuItem"
        Me.AddPropertyToolStripMenuItem.Size = New System.Drawing.Size(144, 22)
        Me.AddPropertyToolStripMenuItem.Text = "&Add Property"
        '
        'ToolStripButtonSave
        '
        Me.ToolStripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonSave.Image = Global.JSON_Graph_Schema_Editor.My.Resources.Resources.Save16x16
        Me.ToolStripButtonSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ToolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonSave.Name = "ToolStripButtonSave"
        Me.ToolStripButtonSave.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButtonSave.Text = "ToolStripButton1"
        '
        'ToolStripDropDownButton1
        '
        Me.ToolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripDropDownButton1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SourceDatabaseToolStripMenuItem})
        Me.ToolStripDropDownButton1.Image = Global.JSON_Graph_Schema_Editor.My.Resources.Resources.Database16x16
        Me.ToolStripDropDownButton1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ToolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripDropDownButton1.Name = "ToolStripDropDownButton1"
        Me.ToolStripDropDownButton1.Size = New System.Drawing.Size(29, 22)
        Me.ToolStripDropDownButton1.Text = "ToolStripDropDownButton1"
        '
        'SourceDatabaseToolStripMenuItem
        '
        Me.SourceDatabaseToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SQLiteConnectToToolStripMenuItem1})
        Me.SourceDatabaseToolStripMenuItem.Name = "SourceDatabaseToolStripMenuItem"
        Me.SourceDatabaseToolStripMenuItem.Size = New System.Drawing.Size(161, 22)
        Me.SourceDatabaseToolStripMenuItem.Text = "&Source Database"
        '
        'SQLiteConnectToToolStripMenuItem1
        '
        Me.SQLiteConnectToToolStripMenuItem1.Name = "SQLiteConnectToToolStripMenuItem1"
        Me.SQLiteConnectToToolStripMenuItem1.Size = New System.Drawing.Size(179, 22)
        Me.SQLiteConnectToToolStripMenuItem1.Text = "SQLite (Connect To)"
        '
        'NodesToolStripMenuItem
        '
        Me.NodesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddNodeToolStripMenuItem})
        Me.NodesToolStripMenuItem.Image = Global.JSON_Graph_Schema_Editor.My.Resources.Resources.PGSNodes16x16
        Me.NodesToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.NodesToolStripMenuItem.Name = "NodesToolStripMenuItem"
        Me.NodesToolStripMenuItem.Size = New System.Drawing.Size(144, 22)
        Me.NodesToolStripMenuItem.Text = "&Nodes"
        '
        'AddNodeToolStripMenuItem
        '
        Me.AddNodeToolStripMenuItem.Image = Global.JSON_Graph_Schema_Editor.My.Resources.Resources.PGSNode_Add16x16
        Me.AddNodeToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.AddNodeToolStripMenuItem.Name = "AddNodeToolStripMenuItem"
        Me.AddNodeToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.AddNodeToolStripMenuItem.Text = "&Add Node"
        '
        'AddRelationshipToolStripMenuItem
        '
        Me.AddRelationshipToolStripMenuItem.Image = Global.JSON_Graph_Schema_Editor.My.Resources.Resources.Relationship_Add16x16
        Me.AddRelationshipToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.AddRelationshipToolStripMenuItem.Name = "AddRelationshipToolStripMenuItem"
        Me.AddRelationshipToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.AddRelationshipToolStripMenuItem.Text = "&Add Relationship"
        '
        'EportToolStripMenuItem
        '
        Me.EportToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AsJSONGraphSchemaToolStripMenuItem})
        Me.EportToolStripMenuItem.Image = Global.JSON_Graph_Schema_Editor.My.Resources.Resources.Export_JSON16x16
        Me.EportToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.EportToolStripMenuItem.Name = "EportToolStripMenuItem"
        Me.EportToolStripMenuItem.Size = New System.Drawing.Size(144, 22)
        Me.EportToolStripMenuItem.Text = "&Export"
        '
        'AsJSONGraphSchemaToolStripMenuItem
        '
        Me.AsJSONGraphSchemaToolStripMenuItem.Image = Global.JSON_Graph_Schema_Editor.My.Resources.Resources.JSONFile16x16
        Me.AsJSONGraphSchemaToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.AsJSONGraphSchemaToolStripMenuItem.Name = "AsJSONGraphSchemaToolStripMenuItem"
        Me.AsJSONGraphSchemaToolStripMenuItem.Size = New System.Drawing.Size(209, 22)
        Me.AsJSONGraphSchemaToolStripMenuItem.Text = "as Graph Schema in &JSON"
        '
        'PropertiesToolStripMenuItem
        '
        Me.PropertiesToolStripMenuItem.Image = Global.JSON_Graph_Schema_Editor.My.Resources.Resources.AddAttribute16x16
        Me.PropertiesToolStripMenuItem.Name = "PropertiesToolStripMenuItem"
        Me.PropertiesToolStripMenuItem.Size = New System.Drawing.Size(148, 26)
        Me.PropertiesToolStripMenuItem.Text = "&Add Property"
        '
        'AddNodeToolStripMenuItem1
        '
        Me.AddNodeToolStripMenuItem1.Image = Global.JSON_Graph_Schema_Editor.My.Resources.Resources.PGSNode_Add16x16
        Me.AddNodeToolStripMenuItem1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.AddNodeToolStripMenuItem1.Name = "AddNodeToolStripMenuItem1"
        Me.AddNodeToolStripMenuItem1.Size = New System.Drawing.Size(128, 22)
        Me.AddNodeToolStripMenuItem1.Text = "&Add Node"
        '
        'AddRelationshipToolStripMenuItem1
        '
        Me.AddRelationshipToolStripMenuItem1.Image = Global.JSON_Graph_Schema_Editor.My.Resources.Resources.Relationship_Add16x16
        Me.AddRelationshipToolStripMenuItem1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.AddRelationshipToolStripMenuItem1.Name = "AddRelationshipToolStripMenuItem1"
        Me.AddRelationshipToolStripMenuItem1.Size = New System.Drawing.Size(164, 22)
        Me.AddRelationshipToolStripMenuItem1.Text = "&Add Relationship"
        '
        'ToolStripMenuItemRelationshipAddProperty
        '
        Me.ToolStripMenuItemRelationshipAddProperty.Image = Global.JSON_Graph_Schema_Editor.My.Resources.Resources.AddAttribute16x16
        Me.ToolStripMenuItemRelationshipAddProperty.Name = "ToolStripMenuItemRelationshipAddProperty"
        Me.ToolStripMenuItemRelationshipAddProperty.Size = New System.Drawing.Size(148, 26)
        Me.ToolStripMenuItemRelationshipAddProperty.Text = "&Add Property"
        '
        'frmSchema
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(967, 500)
        Me.Controls.Add(Me.GroupBoxMain)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmSchema"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Graph Schema Editor"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.GroupBoxMain.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.ToolStripMain.ResumeLayout(False)
        Me.ToolStripMain.PerformLayout()
        Me.ContextMenuStripSchemas.ResumeLayout(False)
        Me.ContextMenuStripSchema.ResumeLayout(False)
        Me.ContextMenuStripNode.ResumeLayout(False)
        Me.ContextMenuStripProperty.ResumeLayout(False)
        Me.ContextMenuStripNodes.ResumeLayout(False)
        Me.ContextMenuStripRelationships.ResumeLayout(False)
        Me.ContextMenuStripRelationship.ResumeLayout(False)
        Me.ContextMenuStripProperties.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBoxMain As GroupBox
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents ContextMenuStripSchemas As ContextMenuStrip
    Friend WithEvents AddSchemaToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ContextMenuStripSchema As ContextMenuStrip
    Friend WithEvents NodesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AddNodeToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RelationshipsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AddRelationshipToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ContextMenuStripNode As ContextMenuStrip
    Friend WithEvents PropertiesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents EditNodeToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DeleteNodeToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ContextMenuStripProperty As ContextMenuStrip
    Friend WithEvents AddPropertyToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents DeletePropertyToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ContextMenuStripNodes As ContextMenuStrip
    Friend WithEvents AddNodeToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents ContextMenuStripRelationships As ContextMenuStrip
    Friend WithEvents AddRelationshipToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents ToolStripMain As ToolStrip
    Friend WithEvents ToolStripLabelPromptSourceDatabase As ToolStripLabel
    Friend WithEvents ToolStripLabelDatabaseName As ToolStripLabel
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents EportToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AsJSONGraphSchemaToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
    Friend WithEvents ContextMenuStripRelationship As ContextMenuStrip
    Friend WithEvents ToolStripMenuItemRelationshipAddProperty As ToolStripMenuItem
    Friend WithEvents ImageListMain As ImageList
    Friend WithEvents TreeView As TreeView
    Friend WithEvents ToolStripButtonSave As ToolStripButton
    Friend WithEvents ContextMenuStripProperties As ContextMenuStrip
    Friend WithEvents AddPropertyToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripDropDownButton1 As ToolStripDropDownButton
    Friend WithEvents SourceDatabaseToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SQLiteConnectToToolStripMenuItem1 As ToolStripMenuItem
End Class