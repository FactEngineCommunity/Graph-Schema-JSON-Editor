<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCRUDAddEditRelationship
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
        Dim RelationshipObjectType1 As JSON_Graph_Schema_Editor.GSJ.RelationshipObjectType = New JSON_Graph_Schema_Editor.GSJ.RelationshipObjectType()
        Dim RefType1 As JSON_Graph_Schema_Editor.GSJ.RefType = New JSON_Graph_Schema_Editor.GSJ.RefType()
        Dim RefType2 As JSON_Graph_Schema_Editor.GSJ.RefType = New JSON_Graph_Schema_Editor.GSJ.RefType()
        Dim RefType3 As JSON_Graph_Schema_Editor.GSJ.RefType = New JSON_Graph_Schema_Editor.GSJ.RefType()
        Me.TPGSRelationship1 = New JSON_Graph_Schema_Editor.tPGSRelationship()
        Me.SuspendLayout()
        '
        'TPGSRelationship1
        '
        Me.TPGSRelationship1.AutoSize = True
        Me.TPGSRelationship1.Dock = System.Windows.Forms.DockStyle.Fill
        RefType1.ref = Nothing
        RelationshipObjectType1.from = RefType1
        RelationshipObjectType1.id = Nothing
        RefType2.ref = Nothing
        RelationshipObjectType1.to = RefType2
        RefType3.ref = Nothing
        RelationshipObjectType1.type = RefType3
        Me.TPGSRelationship1.GSJRelationship = RelationshipObjectType1
        Me.TPGSRelationship1.Location = New System.Drawing.Point(0, 0)
        Me.TPGSRelationship1.Name = "TPGSRelationship1"
        Me.TPGSRelationship1.Size = New System.Drawing.Size(509, 127)
        Me.TPGSRelationship1.TabIndex = 2
        '
        'frmCRUDAddEditRelationship
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(509, 127)
        Me.ControlBox = False
        Me.Controls.Add(Me.TPGSRelationship1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmCRUDAddEditRelationship"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Add/Edit PGS Relationship"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TPGSRelationship1 As tPGSRelationship
End Class
