<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.MenuStripMain = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LoadToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.JSONGraphSchemaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveAsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.DatabaseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SQLiteConnectToToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
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
        Me.NodesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddNodeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RelationshipsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddRelationshipToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.EportToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AsJSONGraphSchemaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStripNode = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.EditNodeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteNodeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStripProperty = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AddPropertyToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeletePropertyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStripNodes = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AddNodeToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStripRelationships = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AddRelationshipToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStripRelationship = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ContextMenuStripProperties = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AddPropertyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripButtonSave = New System.Windows.Forms.ToolStripButton()
        Me.PropertiesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemRelationshipAddProperty = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStripMain.SuspendLayout()
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
        'MenuStripMain
        '
        Me.MenuStripMain.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuStripMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.MenuStripMain.Location = New System.Drawing.Point(0, 0)
        Me.MenuStripMain.Name = "MenuStripMain"
        Me.MenuStripMain.Size = New System.Drawing.Size(967, 28)
        Me.MenuStripMain.TabIndex = 0
        Me.MenuStripMain.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenToolStripMenuItem, Me.LoadToolStripMenuItem, Me.SaveAsToolStripMenuItem, Me.ToolStripSeparator3, Me.DatabaseToolStripMenuItem, Me.ToolStripSeparator2, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(46, 24)
        Me.FileToolStripMenuItem.Text = "&File"
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(204, 26)
        Me.OpenToolStripMenuItem.Text = "&Open"
        '
        'LoadToolStripMenuItem
        '
        Me.LoadToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.JSONGraphSchemaToolStripMenuItem})
        Me.LoadToolStripMenuItem.Name = "LoadToolStripMenuItem"
        Me.LoadToolStripMenuItem.Size = New System.Drawing.Size(204, 26)
        Me.LoadToolStripMenuItem.Text = "&Load"
        '
        'JSONGraphSchemaToolStripMenuItem
        '
        Me.JSONGraphSchemaToolStripMenuItem.Name = "JSONGraphSchemaToolStripMenuItem"
        Me.JSONGraphSchemaToolStripMenuItem.Size = New System.Drawing.Size(227, 26)
        Me.JSONGraphSchemaToolStripMenuItem.Text = "&JSON Graph Schema"
        '
        'SaveAsToolStripMenuItem
        '
        Me.SaveAsToolStripMenuItem.Name = "SaveAsToolStripMenuItem"
        Me.SaveAsToolStripMenuItem.Size = New System.Drawing.Size(204, 26)
        Me.SaveAsToolStripMenuItem.Text = "Save &As"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(201, 6)
        '
        'DatabaseToolStripMenuItem
        '
        Me.DatabaseToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SQLiteConnectToToolStripMenuItem})
        Me.DatabaseToolStripMenuItem.Name = "DatabaseToolStripMenuItem"
        Me.DatabaseToolStripMenuItem.Size = New System.Drawing.Size(204, 26)
        Me.DatabaseToolStripMenuItem.Text = "Source &Database"
        '
        'SQLiteConnectToToolStripMenuItem
        '
        Me.SQLiteConnectToToolStripMenuItem.Name = "SQLiteConnectToToolStripMenuItem"
        Me.SQLiteConnectToToolStripMenuItem.Size = New System.Drawing.Size(223, 26)
        Me.SQLiteConnectToToolStripMenuItem.Text = "SQLite (Connect To)"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(201, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(204, 26)
        Me.ExitToolStripMenuItem.Text = "E&xit"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AboutToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(55, 24)
        Me.HelpToolStripMenuItem.Text = "&Help"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(133, 26)
        Me.AboutToolStripMenuItem.Text = "&About"
        '
        'GroupBoxMain
        '
        Me.GroupBoxMain.Controls.Add(Me.TableLayoutPanel1)
        Me.GroupBoxMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBoxMain.Location = New System.Drawing.Point(0, 28)
        Me.GroupBoxMain.Name = "GroupBoxMain"
        Me.GroupBoxMain.Size = New System.Drawing.Size(967, 472)
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
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 18)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(961, 451)
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
        Me.TreeView.Size = New System.Drawing.Size(955, 415)
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
        Me.ToolStripMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButtonSave, Me.ToolStripLabelPromptSourceDatabase, Me.ToolStripLabelDatabaseName})
        Me.ToolStripMain.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripMain.Name = "ToolStripMain"
        Me.ToolStripMain.Size = New System.Drawing.Size(961, 27)
        Me.ToolStripMain.TabIndex = 1
        Me.ToolStripMain.Text = "ToolStrip1"
        '
        'ToolStripLabelPromptSourceDatabase
        '
        Me.ToolStripLabelPromptSourceDatabase.Name = "ToolStripLabelPromptSourceDatabase"
        Me.ToolStripLabelPromptSourceDatabase.Size = New System.Drawing.Size(124, 24)
        Me.ToolStripLabelPromptSourceDatabase.Text = "Source Database:"
        Me.ToolStripLabelPromptSourceDatabase.Visible = False
        '
        'ToolStripLabelDatabaseName
        '
        Me.ToolStripLabelDatabaseName.Name = "ToolStripLabelDatabaseName"
        Me.ToolStripLabelDatabaseName.Size = New System.Drawing.Size(136, 24)
        Me.ToolStripLabelDatabaseName.Text = "<Database Name>"
        Me.ToolStripLabelDatabaseName.Visible = False
        '
        'ContextMenuStripSchemas
        '
        Me.ContextMenuStripSchemas.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ContextMenuStripSchemas.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddSchemaToolStripMenuItem})
        Me.ContextMenuStripSchemas.Name = "ContextMenuStripSchemas"
        Me.ContextMenuStripSchemas.Size = New System.Drawing.Size(163, 28)
        '
        'AddSchemaToolStripMenuItem
        '
        Me.AddSchemaToolStripMenuItem.Name = "AddSchemaToolStripMenuItem"
        Me.AddSchemaToolStripMenuItem.Size = New System.Drawing.Size(162, 24)
        Me.AddSchemaToolStripMenuItem.Text = "&Add Schema"
        '
        'ContextMenuStripSchema
        '
        Me.ContextMenuStripSchema.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ContextMenuStripSchema.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NodesToolStripMenuItem, Me.RelationshipsToolStripMenuItem, Me.ToolStripSeparator1, Me.EportToolStripMenuItem})
        Me.ContextMenuStripSchema.Name = "ContextMenuStripSchema"
        Me.ContextMenuStripSchema.Size = New System.Drawing.Size(167, 82)
        '
        'NodesToolStripMenuItem
        '
        Me.NodesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddNodeToolStripMenuItem})
        Me.NodesToolStripMenuItem.Name = "NodesToolStripMenuItem"
        Me.NodesToolStripMenuItem.Size = New System.Drawing.Size(166, 24)
        Me.NodesToolStripMenuItem.Text = "&Nodes"
        '
        'AddNodeToolStripMenuItem
        '
        Me.AddNodeToolStripMenuItem.Name = "AddNodeToolStripMenuItem"
        Me.AddNodeToolStripMenuItem.Size = New System.Drawing.Size(161, 26)
        Me.AddNodeToolStripMenuItem.Text = "&Add Node"
        '
        'RelationshipsToolStripMenuItem
        '
        Me.RelationshipsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddRelationshipToolStripMenuItem})
        Me.RelationshipsToolStripMenuItem.Name = "RelationshipsToolStripMenuItem"
        Me.RelationshipsToolStripMenuItem.Size = New System.Drawing.Size(166, 24)
        Me.RelationshipsToolStripMenuItem.Text = "&Relationships"
        '
        'AddRelationshipToolStripMenuItem
        '
        Me.AddRelationshipToolStripMenuItem.Name = "AddRelationshipToolStripMenuItem"
        Me.AddRelationshipToolStripMenuItem.Size = New System.Drawing.Size(206, 26)
        Me.AddRelationshipToolStripMenuItem.Text = "&Add Relationship"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(163, 6)
        '
        'EportToolStripMenuItem
        '
        Me.EportToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AsJSONGraphSchemaToolStripMenuItem})
        Me.EportToolStripMenuItem.Name = "EportToolStripMenuItem"
        Me.EportToolStripMenuItem.Size = New System.Drawing.Size(166, 24)
        Me.EportToolStripMenuItem.Text = "&Export"
        '
        'AsJSONGraphSchemaToolStripMenuItem
        '
        Me.AsJSONGraphSchemaToolStripMenuItem.Name = "AsJSONGraphSchemaToolStripMenuItem"
        Me.AsJSONGraphSchemaToolStripMenuItem.Size = New System.Drawing.Size(245, 26)
        Me.AsJSONGraphSchemaToolStripMenuItem.Text = "as &JSON Graph Schema"
        '
        'ContextMenuStripNode
        '
        Me.ContextMenuStripNode.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ContextMenuStripNode.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PropertiesToolStripMenuItem, Me.ToolStripSeparator4, Me.EditNodeToolStripMenuItem, Me.DeleteNodeToolStripMenuItem})
        Me.ContextMenuStripNode.Name = "ContextMenuStripNode"
        Me.ContextMenuStripNode.Size = New System.Drawing.Size(171, 88)
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(167, 6)
        '
        'EditNodeToolStripMenuItem
        '
        Me.EditNodeToolStripMenuItem.Name = "EditNodeToolStripMenuItem"
        Me.EditNodeToolStripMenuItem.Size = New System.Drawing.Size(170, 26)
        Me.EditNodeToolStripMenuItem.Text = "&Edit Node"
        '
        'DeleteNodeToolStripMenuItem
        '
        Me.DeleteNodeToolStripMenuItem.Name = "DeleteNodeToolStripMenuItem"
        Me.DeleteNodeToolStripMenuItem.Size = New System.Drawing.Size(170, 26)
        Me.DeleteNodeToolStripMenuItem.Text = "&Delete Node"
        '
        'ContextMenuStripProperty
        '
        Me.ContextMenuStripProperty.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ContextMenuStripProperty.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddPropertyToolStripMenuItem1, Me.DeletePropertyToolStripMenuItem})
        Me.ContextMenuStripProperty.Name = "ContextMenuStripProperty"
        Me.ContextMenuStripProperty.Size = New System.Drawing.Size(183, 52)
        '
        'AddPropertyToolStripMenuItem1
        '
        Me.AddPropertyToolStripMenuItem1.Name = "AddPropertyToolStripMenuItem1"
        Me.AddPropertyToolStripMenuItem1.Size = New System.Drawing.Size(182, 24)
        Me.AddPropertyToolStripMenuItem1.Text = "&Edit Property"
        '
        'DeletePropertyToolStripMenuItem
        '
        Me.DeletePropertyToolStripMenuItem.Name = "DeletePropertyToolStripMenuItem"
        Me.DeletePropertyToolStripMenuItem.Size = New System.Drawing.Size(182, 24)
        Me.DeletePropertyToolStripMenuItem.Text = "&Delete Property"
        '
        'ContextMenuStripNodes
        '
        Me.ContextMenuStripNodes.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ContextMenuStripNodes.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddNodeToolStripMenuItem1})
        Me.ContextMenuStripNodes.Name = "ContextMenuStripNodes"
        Me.ContextMenuStripNodes.Size = New System.Drawing.Size(148, 28)
        '
        'AddNodeToolStripMenuItem1
        '
        Me.AddNodeToolStripMenuItem1.Name = "AddNodeToolStripMenuItem1"
        Me.AddNodeToolStripMenuItem1.Size = New System.Drawing.Size(147, 24)
        Me.AddNodeToolStripMenuItem1.Text = "&Add Node"
        '
        'ContextMenuStripRelationships
        '
        Me.ContextMenuStripRelationships.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ContextMenuStripRelationships.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddRelationshipToolStripMenuItem1})
        Me.ContextMenuStripRelationships.Name = "ContextMenuStripRelationships"
        Me.ContextMenuStripRelationships.Size = New System.Drawing.Size(193, 28)
        '
        'AddRelationshipToolStripMenuItem1
        '
        Me.AddRelationshipToolStripMenuItem1.Name = "AddRelationshipToolStripMenuItem1"
        Me.AddRelationshipToolStripMenuItem1.Size = New System.Drawing.Size(192, 24)
        Me.AddRelationshipToolStripMenuItem1.Text = "&Add Relationship"
        '
        'ContextMenuStripRelationship
        '
        Me.ContextMenuStripRelationship.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ContextMenuStripRelationship.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemRelationshipAddProperty})
        Me.ContextMenuStripRelationship.Name = "ContextMenuStripRelationship"
        Me.ContextMenuStripRelationship.Size = New System.Drawing.Size(171, 30)
        '
        'ContextMenuStripProperties
        '
        Me.ContextMenuStripProperties.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ContextMenuStripProperties.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddPropertyToolStripMenuItem})
        Me.ContextMenuStripProperties.Name = "ContextMenuStripProperties"
        Me.ContextMenuStripProperties.Size = New System.Drawing.Size(211, 56)
        '
        'AddPropertyToolStripMenuItem
        '
        Me.AddPropertyToolStripMenuItem.Name = "AddPropertyToolStripMenuItem"
        Me.AddPropertyToolStripMenuItem.Size = New System.Drawing.Size(210, 24)
        Me.AddPropertyToolStripMenuItem.Text = "&Add Property"
        '
        'ToolStripButtonSave
        '
        Me.ToolStripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonSave.Image = Global.JSON_Graph_Schema_Editor.My.Resources.Resources.Save16x16
        Me.ToolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonSave.Name = "ToolStripButtonSave"
        Me.ToolStripButtonSave.Size = New System.Drawing.Size(29, 24)
        Me.ToolStripButtonSave.Text = "ToolStripButton1"
        '
        'PropertiesToolStripMenuItem
        '
        Me.PropertiesToolStripMenuItem.Image = Global.JSON_Graph_Schema_Editor.My.Resources.Resources.AddAttribute16x16
        Me.PropertiesToolStripMenuItem.Name = "PropertiesToolStripMenuItem"
        Me.PropertiesToolStripMenuItem.Size = New System.Drawing.Size(170, 26)
        Me.PropertiesToolStripMenuItem.Text = "&Add Property"
        '
        'ToolStripMenuItemRelationshipAddProperty
        '
        Me.ToolStripMenuItemRelationshipAddProperty.Image = Global.JSON_Graph_Schema_Editor.My.Resources.Resources.AddAttribute16x16
        Me.ToolStripMenuItemRelationshipAddProperty.Name = "ToolStripMenuItemRelationshipAddProperty"
        Me.ToolStripMenuItemRelationshipAddProperty.Size = New System.Drawing.Size(170, 26)
        Me.ToolStripMenuItemRelationshipAddProperty.Text = "&Add Property"
        '
        'frmMain
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(967, 500)
        Me.Controls.Add(Me.GroupBoxMain)
        Me.Controls.Add(Me.MenuStripMain)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStripMain
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "JSON Graph Schema Editor"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.MenuStripMain.ResumeLayout(False)
        Me.MenuStripMain.PerformLayout()
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
        Me.PerformLayout()

    End Sub

    Friend WithEvents MenuStripMain As MenuStrip
    Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As ToolStripMenuItem
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
    Friend WithEvents OpenToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents LoadToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents JSONGraphSchemaToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SaveAsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents DatabaseToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SQLiteConnectToToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents ToolStripMain As ToolStrip
    Friend WithEvents ToolStripLabelPromptSourceDatabase As ToolStripLabel
    Friend WithEvents ToolStripLabelDatabaseName As ToolStripLabel
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents EportToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AsJSONGraphSchemaToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
    Friend WithEvents ContextMenuStripRelationship As ContextMenuStrip
    Friend WithEvents ToolStripMenuItemRelationshipAddProperty As ToolStripMenuItem
    Friend WithEvents ImageListMain As ImageList
    Friend WithEvents TreeView As TreeView
    Friend WithEvents ToolStripButtonSave As ToolStripButton
    Friend WithEvents ContextMenuStripProperties As ContextMenuStrip
    Friend WithEvents AddPropertyToolStripMenuItem As ToolStripMenuItem
End Class
