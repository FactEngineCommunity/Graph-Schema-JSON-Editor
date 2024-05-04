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
        Me.TPGSRelationship1 = New JSON_Graph_Schema_Editor.tPGSRelationship()
        Me.SuspendLayout()
        '
        'TPGSRelationship1
        '
        Me.TPGSRelationship1.AutoSize = True
        Me.TPGSRelationship1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TPGSRelationship1.Location = New System.Drawing.Point(0, 0)
        Me.TPGSRelationship1.Name = "TPGSRelationship1"
        Me.TPGSRelationship1.Size = New System.Drawing.Size(509, 127)
        Me.TPGSRelationship1.TabIndex = 0
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
