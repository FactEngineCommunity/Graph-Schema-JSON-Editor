<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAbout
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAbout))
        Me.button_close = New System.Windows.Forms.Button()
        Me.labelprompt_rosters = New System.Windows.Forms.Label()
        Me.label_versioning = New System.Windows.Forms.Label()
        Me.label_details = New System.Windows.Forms.Label()
        Me.LabelPromptLicenses = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.LabelProjectPrompt = New System.Windows.Forms.Label()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'button_close
        '
        Me.button_close.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.button_close.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.button_close.Location = New System.Drawing.Point(286, 180)
        Me.button_close.Name = "button_close"
        Me.button_close.Size = New System.Drawing.Size(80, 23)
        Me.button_close.TabIndex = 0
        Me.button_close.Text = "&Close"
        Me.button_close.UseVisualStyleBackColor = True
        '
        'labelprompt_rosters
        '
        Me.labelprompt_rosters.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelprompt_rosters.ForeColor = System.Drawing.Color.Black
        Me.labelprompt_rosters.Location = New System.Drawing.Point(12, 9)
        Me.labelprompt_rosters.Name = "labelprompt_rosters"
        Me.labelprompt_rosters.Size = New System.Drawing.Size(230, 57)
        Me.labelprompt_rosters.TabIndex = 3
        Me.labelprompt_rosters.Text = "Graph Schema JSON Editor"
        Me.labelprompt_rosters.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'label_versioning
        '
        Me.label_versioning.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label_versioning.ForeColor = System.Drawing.Color.Gray
        Me.label_versioning.Location = New System.Drawing.Point(16, 66)
        Me.label_versioning.Name = "label_versioning"
        Me.label_versioning.Size = New System.Drawing.Size(257, 45)
        Me.label_versioning.TabIndex = 5
        Me.label_versioning.Text = "label_versioning"
        '
        'label_details
        '
        Me.label_details.ForeColor = System.Drawing.Color.SteelBlue
        Me.label_details.Location = New System.Drawing.Point(12, 125)
        Me.label_details.Name = "label_details"
        Me.label_details.Size = New System.Drawing.Size(343, 52)
        Me.label_details.TabIndex = 6
        Me.label_details.Text = "label_details"
        '
        'LabelPromptLicenses
        '
        Me.LabelPromptLicenses.AutoSize = True
        Me.LabelPromptLicenses.Cursor = System.Windows.Forms.Cursors.Hand
        Me.LabelPromptLicenses.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelPromptLicenses.ForeColor = System.Drawing.Color.Blue
        Me.LabelPromptLicenses.Location = New System.Drawing.Point(12, 180)
        Me.LabelPromptLicenses.Name = "LabelPromptLicenses"
        Me.LabelPromptLicenses.Size = New System.Drawing.Size(49, 13)
        Me.LabelPromptLicenses.TabIndex = 9
        Me.LabelPromptLicenses.Text = "Licenses"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(12, 217)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(342, 13)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "https://github.com/FactEngineCommunity/Graph-Schema-JSON-Editor"
        '
        'LabelProjectPrompt
        '
        Me.LabelProjectPrompt.AutoSize = True
        Me.LabelProjectPrompt.Location = New System.Drawing.Point(12, 204)
        Me.LabelProjectPrompt.Name = "LabelProjectPrompt"
        Me.LabelProjectPrompt.Size = New System.Drawing.Size(43, 13)
        Me.LabelProjectPrompt.TabIndex = 12
        Me.LabelProjectPrompt.Text = "Project:"
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(248, 9)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(118, 39)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox2.TabIndex = 8
        Me.PictureBox2.TabStop = False
        '
        'frmAbout
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(378, 239)
        Me.ControlBox = False
        Me.Controls.Add(Me.LabelProjectPrompt)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.LabelPromptLicenses)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.label_versioning)
        Me.Controls.Add(Me.labelprompt_rosters)
        Me.Controls.Add(Me.button_close)
        Me.Controls.Add(Me.label_details)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "frmAbout"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents button_close As System.Windows.Forms.Button
    Friend WithEvents labelprompt_rosters As System.Windows.Forms.Label
    Friend WithEvents label_versioning As System.Windows.Forms.Label
    Friend WithEvents label_details As System.Windows.Forms.Label
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents LabelPromptLicenses As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents LabelProjectPrompt As Label
End Class
