<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmCRUDAddEditProperty
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.TPGSProperty1 = New JSON_Graph_Schema_Editor.tPGSProperty()
        Me.SuspendLayout()
        '
        'TPGSProperty1
        '
        Me.TPGSProperty1.AutoSize = True
        Me.TPGSProperty1.Location = New System.Drawing.Point(6, 6)
        Me.TPGSProperty1.Name = "TPGSProperty1"
        Me.TPGSProperty1.Size = New System.Drawing.Size(513, 99)
        Me.TPGSProperty1.TabIndex = 2
        '
        'frmCRUDAddEditProperty
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(525, 114)
        Me.ControlBox = False
        Me.Controls.Add(Me.TPGSProperty1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmCRUDAddEditProperty"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Add/Edit PGS Property"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TPGSProperty1 As tPGSProperty
End Class
