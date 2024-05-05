<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class tPGSRelationship
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.GroupBoxMain = New System.Windows.Forms.GroupBox()
        Me.ComboBoxNodeType2 = New System.Windows.Forms.ComboBox()
        Me.ComboBoxNodeType1 = New System.Windows.Forms.ComboBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.ButtonOkay = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBoxRelationshipType = New System.Windows.Forms.TextBox()
        Me.LabelPromptBeginLabel = New System.Windows.Forms.Label()
        Me.GroupBoxMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBoxMain
        '
        Me.GroupBoxMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.GroupBoxMain.Controls.Add(Me.ComboBoxNodeType2)
        Me.GroupBoxMain.Controls.Add(Me.ComboBoxNodeType1)
        Me.GroupBoxMain.Controls.Add(Me.Button1)
        Me.GroupBoxMain.Controls.Add(Me.ButtonOkay)
        Me.GroupBoxMain.Controls.Add(Me.Label1)
        Me.GroupBoxMain.Controls.Add(Me.TextBoxRelationshipType)
        Me.GroupBoxMain.Controls.Add(Me.LabelPromptBeginLabel)
        Me.GroupBoxMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBoxMain.Location = New System.Drawing.Point(0, 0)
        Me.GroupBoxMain.Name = "GroupBoxMain"
        Me.GroupBoxMain.Size = New System.Drawing.Size(513, 99)
        Me.GroupBoxMain.TabIndex = 0
        Me.GroupBoxMain.TabStop = False
        '
        'ComboBoxNodeType2
        '
        Me.ComboBoxNodeType2.FormattingEnabled = True
        Me.ComboBoxNodeType2.Location = New System.Drawing.Point(300, 37)
        Me.ComboBoxNodeType2.Name = "ComboBoxNodeType2"
        Me.ComboBoxNodeType2.Size = New System.Drawing.Size(121, 24)
        Me.ComboBoxNodeType2.TabIndex = 3
        Me.ComboBoxNodeType2.Text = "Node Type 2"
        '
        'ComboBoxNodeType1
        '
        Me.ComboBoxNodeType1.FormattingEnabled = True
        Me.ComboBoxNodeType1.Location = New System.Drawing.Point(17, 38)
        Me.ComboBoxNodeType1.Name = "ComboBoxNodeType1"
        Me.ComboBoxNodeType1.Size = New System.Drawing.Size(121, 24)
        Me.ComboBoxNodeType1.TabIndex = 1
        Me.ComboBoxNodeType1.Text = "Node Type 1"
        '
        'Button1
        '
        Me.Button1.Image = Global.JSON_Graph_Schema_Editor.My.Resources.Resources.delete16x16
        Me.Button1.Location = New System.Drawing.Point(454, 38)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(25, 23)
        Me.Button1.TabIndex = 6
        Me.Button1.UseVisualStyleBackColor = True
        '
        'ButtonOkay
        '
        Me.ButtonOkay.Image = Global.JSON_Graph_Schema_Editor.My.Resources.Resources.tick_button
        Me.ButtonOkay.Location = New System.Drawing.Point(431, 38)
        Me.ButtonOkay.Name = "ButtonOkay"
        Me.ButtonOkay.Size = New System.Drawing.Size(22, 23)
        Me.ButtonOkay.TabIndex = 5
        Me.ButtonOkay.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial Narrow", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(272, 37)
        Me.Label1.Margin = New System.Windows.Forms.Padding(0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(28, 22)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "]->"
        '
        'TextBoxRelationshipType
        '
        Me.TextBoxRelationshipType.Location = New System.Drawing.Point(164, 37)
        Me.TextBoxRelationshipType.Name = "TextBoxRelationshipType"
        Me.TextBoxRelationshipType.Size = New System.Drawing.Size(108, 22)
        Me.TextBoxRelationshipType.TabIndex = 2
        Me.TextBoxRelationshipType.Text = "RELATES_TO"
        '
        'LabelPromptBeginLabel
        '
        Me.LabelPromptBeginLabel.AutoSize = True
        Me.LabelPromptBeginLabel.Font = New System.Drawing.Font("Arial Narrow", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelPromptBeginLabel.Location = New System.Drawing.Point(141, 37)
        Me.LabelPromptBeginLabel.Margin = New System.Windows.Forms.Padding(0)
        Me.LabelPromptBeginLabel.Name = "LabelPromptBeginLabel"
        Me.LabelPromptBeginLabel.Size = New System.Drawing.Size(25, 22)
        Me.LabelPromptBeginLabel.TabIndex = 1
        Me.LabelPromptBeginLabel.Text = "-[:"
        '
        'tPGSRelationship
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.AutoSize = True
        Me.Controls.Add(Me.GroupBoxMain)
        Me.Name = "tPGSRelationship"
        Me.Size = New System.Drawing.Size(513, 99)
        Me.GroupBoxMain.ResumeLayout(False)
        Me.GroupBoxMain.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GroupBoxMain As GroupBox
    Friend WithEvents Button1 As Button
    Friend WithEvents ButtonOkay As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents TextBoxRelationshipType As TextBox
    Friend WithEvents LabelPromptBeginLabel As Label
    Friend WithEvents ComboBoxNodeType2 As ComboBox
    Friend WithEvents ComboBoxNodeType1 As ComboBox
End Class
