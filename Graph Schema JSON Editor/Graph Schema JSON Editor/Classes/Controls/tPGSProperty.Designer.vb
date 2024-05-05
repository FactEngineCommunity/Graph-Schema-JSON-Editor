<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class tPGSProperty
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.GroupBoxMain = New System.Windows.Forms.GroupBox()
        Me.CheckBoxAllowNulls = New System.Windows.Forms.CheckBox()
        Me.ComboBoxDataType = New System.Windows.Forms.ComboBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.ButtonOkay = New System.Windows.Forms.Button()
        Me.TextBoxPropertyName = New System.Windows.Forms.TextBox()
        Me.GroupBoxMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBoxMain
        '
        Me.GroupBoxMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.GroupBoxMain.Controls.Add(Me.CheckBoxAllowNulls)
        Me.GroupBoxMain.Controls.Add(Me.ComboBoxDataType)
        Me.GroupBoxMain.Controls.Add(Me.Button1)
        Me.GroupBoxMain.Controls.Add(Me.ButtonOkay)
        Me.GroupBoxMain.Controls.Add(Me.TextBoxPropertyName)
        Me.GroupBoxMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBoxMain.Location = New System.Drawing.Point(0, 0)
        Me.GroupBoxMain.Name = "GroupBoxMain"
        Me.GroupBoxMain.Size = New System.Drawing.Size(513, 99)
        Me.GroupBoxMain.TabIndex = 0
        Me.GroupBoxMain.TabStop = False
        '
        'CheckBoxAllowNulls
        '
        Me.CheckBoxAllowNulls.AutoSize = True
        Me.CheckBoxAllowNulls.Checked = True
        Me.CheckBoxAllowNulls.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxAllowNulls.Location = New System.Drawing.Point(303, 40)
        Me.CheckBoxAllowNulls.Name = "CheckBoxAllowNulls"
        Me.CheckBoxAllowNulls.Size = New System.Drawing.Size(97, 21)
        Me.CheckBoxAllowNulls.TabIndex = 7
        Me.CheckBoxAllowNulls.Text = "Allow Nulls"
        Me.CheckBoxAllowNulls.UseVisualStyleBackColor = True
        '
        'ComboBoxDataType
        '
        Me.ComboBoxDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxDataType.FormattingEnabled = True
        Me.ComboBoxDataType.Items.AddRange(New Object() {"integer", "string", "float", "boolean", "point", "date", "datetime", "time", "localtime", "localdatetime", "duration"})
        Me.ComboBoxDataType.Location = New System.Drawing.Point(166, 38)
        Me.ComboBoxDataType.Name = "ComboBoxDataType"
        Me.ComboBoxDataType.Size = New System.Drawing.Size(121, 24)
        Me.ComboBoxDataType.TabIndex = 1
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
        'TextBoxPropertyName
        '
        Me.TextBoxPropertyName.Location = New System.Drawing.Point(14, 38)
        Me.TextBoxPropertyName.Name = "TextBoxPropertyName"
        Me.TextBoxPropertyName.Size = New System.Drawing.Size(146, 22)
        Me.TextBoxPropertyName.TabIndex = 2
        Me.TextBoxPropertyName.Text = "PropertyName"
        '
        'tPGSProperty
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.AutoSize = True
        Me.Controls.Add(Me.GroupBoxMain)
        Me.Name = "tPGSProperty"
        Me.Size = New System.Drawing.Size(513, 99)
        Me.GroupBoxMain.ResumeLayout(False)
        Me.GroupBoxMain.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GroupBoxMain As GroupBox
    Friend WithEvents Button1 As Button
    Friend WithEvents ButtonOkay As Button
    Friend WithEvents TextBoxPropertyName As TextBox
    Friend WithEvents ComboBoxDataType As ComboBox
    Friend WithEvents CheckBoxAllowNulls As CheckBox
End Class
