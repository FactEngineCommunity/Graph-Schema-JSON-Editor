''' <summary>
''' Added as Tag of TreeNode in the Schema TreeView, for those TreeNodes where need to identify the type of Menu for the TreeNode. I.e. e.g. ContextMenuStrip decision made on the type
''' </summary>
Public Class tSchemaTreeMenuType

    Public MenuType As pcenumSchemaTreeMenuType = pcenumSchemaTreeMenuType.None

    ''' <summary>
    ''' Constructor. Identify the type of Menu for the Schema TreeView, TreeNode. I.e. e.g. ContextMenuStrip decision made on the type.
    ''' </summary>
    ''' <param name="aiSchemaTreeMenuType">The Type of the Tree Menu. E.g. Properties</param>
    Public Sub New(ByVal aiSchemaTreeMenuType As pcenumSchemaTreeMenuType)

        Me.MenuType = aiSchemaTreeMenuType

    End Sub

End Class
