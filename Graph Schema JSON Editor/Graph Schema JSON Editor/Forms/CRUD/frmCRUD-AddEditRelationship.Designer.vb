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
        Me.TPGSRelationship = New JSON_Graph_Schema_Editor.tPGSRelationship()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.RadioButtonIsManyToMany = New System.Windows.Forms.RadioButton()
        Me.RadioButtonIsManyToOne = New System.Windows.Forms.RadioButton()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TPGSRelationship
        '
        Me.TPGSRelationship.AutoSize = True
        Me.TPGSRelationship.Dock = System.Windows.Forms.DockStyle.Fill
        RefType1.ref = Nothing
        RelationshipObjectType1.from = RefType1
        RelationshipObjectType1.id = Nothing
        RefType2.ref = Nothing
        RelationshipObjectType1.to = RefType2
        RefType3.ref = Nothing
        RelationshipObjectType1.type = RefType3
        Me.TPGSRelationship.GSJRelationship = RelationshipObjectType1
        Me.TPGSRelationship.Location = New System.Drawing.Point(3, 3)
        Me.TPGSRelationship.Name = "TPGSRelationship"
        Me.TPGSRelationship.Size = New System.Drawing.Size(540, 94)
        Me.TPGSRelationship.TabIndex = 2
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.TPGSRelationship, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Panel1, 0, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(546, 159)
        Me.TableLayoutPanel1.TabIndex = 3
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.RadioButtonIsManyToMany)
        Me.Panel1.Controls.Add(Me.RadioButtonIsManyToOne)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 103)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(540, 53)
        Me.Panel1.TabIndex = 3
        '
        'RadioButtonIsManyToMany
        '
        Me.RadioButtonIsManyToMany.AutoSize = True
        Me.RadioButtonIsManyToMany.Checked = True
        Me.RadioButtonIsManyToMany.Location = New System.Drawing.Point(101, 12)
        Me.RadioButtonIsManyToMany.Name = "RadioButtonIsManyToMany"
        Me.RadioButtonIsManyToMany.Size = New System.Drawing.Size(92, 17)
        Me.RadioButtonIsManyToMany.TabIndex = 1
        Me.RadioButtonIsManyToMany.TabStop = True
        Me.RadioButtonIsManyToMany.Text = "Many to Many"
        Me.RadioButtonIsManyToMany.UseVisualStyleBackColor = True
        '
        'RadioButtonIsManyToOne
        '
        Me.RadioButtonIsManyToOne.AutoSize = True
        Me.RadioButtonIsManyToOne.Location = New System.Drawing.Point(9, 12)
        Me.RadioButtonIsManyToOne.Name = "RadioButtonIsManyToOne"
        Me.RadioButtonIsManyToOne.Size = New System.Drawing.Size(86, 17)
        Me.RadioButtonIsManyToOne.TabIndex = 0
        Me.RadioButtonIsManyToOne.Text = "Many to One"
        Me.RadioButtonIsManyToOne.UseVisualStyleBackColor = True
        '
        'frmCRUDAddEditRelationship
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(546, 159)
        Me.ControlBox = False
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmCRUDAddEditRelationship"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Add/Edit PGS Relationship"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TPGSRelationship As tPGSRelationship
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents RadioButtonIsManyToMany As RadioButton
    Friend WithEvents RadioButtonIsManyToOne As RadioButton
End Class
