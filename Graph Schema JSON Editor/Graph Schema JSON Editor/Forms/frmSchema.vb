Imports FactEngineForServices
Imports System.Text.RegularExpressions
Imports System.IO
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Xml.Serialization
Imports System.Reflection
Imports System.ComponentModel

Public Class frmSchema

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles Me.Load

        Call Me.SetupForm()

    End Sub

    Private Sub SetupForm()

        'Expand the TreeView
        Me.TreeView.ExpandAll()

    End Sub

    ''' <summary>
    ''' Adds a Node Type to the Schema of the Selected TreeNode, or the given TreeNode.
    ''' </summary>
    ''' <param name="arTreeNode">Only provide fo the Schema TreeNode.</param>
    Private Sub AddNodeTypeToSelectedTreeNode(Optional ByRef arSchemaTreeNode As TreeNode = Nothing)

        Try
            Dim loTreeNode As TreeNode

            'Get the RelationalDataStructure that stores the Schema.
            '  NB GSJ Editor stores Schemas in the GraphRelational/RelationalKnowledgeGraph format in a Fact-Based Model with an associated Relational Data Structure.
            '  I.e. Node Types are the same as RDS Tables under the RelationalKnowledgeGraph/Fact-Based Modelling Schema.
            Dim lrRDSModel As RDS.Model

            If arSchemaTreeNode Is Nothing Then
                loTreeNode = Me.TreeView.SelectedNode
                lrRDSModel = loTreeNode.Parent.Tag
            Else
                loTreeNode = arSchemaTreeNode.Nodes(0)
                lrRDSModel = arSchemaTreeNode.Tag
            End If

            '--------------------------------------------------------------
            'Create an EntityType/ObjectType for the FBM (FactBasedModel)
            'I.e. Is the Node Type being created.
            Dim lsEntityTypeName = "New Node Type"
            'Create a unique NodeType Name.
            lsEntityTypeName = lrRDSModel.Model.CreateUniqueEntityTypeName(lsEntityTypeName, 0, False)
            Dim lrEntityType As New FBM.EntityType(lrRDSModel.Model, pcenumLanguage.ORMModel, lsEntityTypeName)

            Dim loNewNodeTreeNode = New TreeNode($"({lsEntityTypeName})", 1, 1)
            loTreeNode.Nodes.Add(loNewNodeTreeNode)

#Region "RDS (RelationalDataStructure) management"
            '---------------------------------------------------------
            'Add the Node Table/Table to the RDS (RelationalDataStructure)
            Dim lrRDSTable As New RDS.Table(lrRDSModel, lrEntityType.Name, lrEntityType)
            'Add the RDS Table to the RDS Schema
            lrRDSModel.addTable(lrRDSTable) 'NB Has no Properties (Columns) at this stage.
            loNewNodeTreeNode.Tag = lrRDSTable
#End Region

#Region "Properties - Collection Placeholder TreeNode"
            Dim loPropertiesTreeNode = New TreeNode("Properties", 4, 4)
            loPropertiesTreeNode.Tag = New tSchemaTreeMenuType(pcenumSchemaTreeMenuType.Properties)
            loNewNodeTreeNode.Nodes.Add(loPropertiesTreeNode)
#End Region

            loTreeNode.Expand()
            loNewNodeTreeNode.Expand()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    ''' <summary>
    ''' ContextMenuStrip management.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TreeView_MouseUp(sender As Object, e As MouseEventArgs) Handles TreeView.MouseUp

        Try
            If e.Button = MouseButtons.Right Then
                ' Determine the node at the position of the mouse cursor
                Dim nodeAtMousePosition As TreeNode = TreeView.GetNodeAt(e.X, e.Y)

                ' Select this node
                If nodeAtMousePosition IsNot Nothing And e.Button = MouseButtons.Right Then
                    TreeView.SelectedNode = nodeAtMousePosition

                    '=====================================================================
                    'Allocate the appropriate ContextMenuStrip to the TreeView/TreeNode.
                    If TreeView.SelectedNode.Name = "Schemas" Then

                        ContextMenuStripSchemas.Show(TreeView, e.Location)

                    ElseIf TreeView.SelectedNode.Name = "Node Types" Then

                        ContextMenuStripNodes.Show(TreeView, e.Location)

                    ElseIf TreeView.SelectedNode.Name = "Relationship Types" Then

                        ContextMenuStripRelationships.Show(TreeView, e.Location)

                    Else
                        Select Case TreeView.SelectedNode.Tag.GetType

                            Case Is = GetType(RDS.Model)

                                ContextMenuStripSchema.Show(TreeView, e.Location)

                            Case Is = GetType(RDS.Table)

                                ContextMenuStripNode.Show(TreeView, e.Location)

                            Case Is = GetType(RDS.Column)

                                ContextMenuStripProperty.Show(TreeView, e.Location)

                            Case Is = GetType(tSchemaTreeMenuType)

                                Select Case CType(TreeView.SelectedNode.Tag, tSchemaTreeMenuType).MenuType

                                    Case Is = pcenumSchemaTreeMenuType.Properties

                                        ContextMenuStripProperties.Show(TreeView, e.Location)

                                    Case Is = pcenumSchemaTreeMenuType.NodeTypes

                                        ContextMenuStripNodes.Show(TreeView, e.Location)

                                    Case Is = pcenumSchemaTreeMenuType.Relationships

                                        ContextMenuStripRelationships.Show(TreeView, e.Location)

                                End Select


                        End Select

                    End If

                End If
            End If

        Catch ex As Exception



        End Try

    End Sub

    ''' <summary>
    ''' Add Node to Schema
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub AddNodeToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles AddNodeToolStripMenuItem1.Click

        Try

            Call Me.AddNodeTypeToSelectedTreeNode

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub


    ''' <summary>
    ''' Add Node to Schema
    ''' </summary>
    Private Sub AddNodeToTreeView(ByRef arSchemaTreeNode As TreeNode, ByRef arRDSTable As RDS.Table)

        Dim loTreeNode As TreeNode = arSchemaTreeNode.Nodes(0)

        Dim loNewNodeTreeNode = New TreeNode("(" & arRDSTable.Name & ")", 1, 1)
        loTreeNode.Nodes.Add(loNewNodeTreeNode)
        loNewNodeTreeNode.Tag = arRDSTable
        loTreeNode.Expand()

        Dim loPropertiesTreeNode = New TreeNode("Properties", 4, 4)
        loPropertiesTreeNode.Tag = New tSchemaTreeMenuType(pcenumSchemaTreeMenuType.Properties)
        loNewNodeTreeNode.Nodes.Add(loPropertiesTreeNode)

        '==========================================================
        'Properties
        For Each lrRDSColumn In arRDSTable.Column

            Dim lsPropertyEmbellishment = lrRDSColumn.Name & " { ""type"": """ & If(lrRDSColumn.DataType Is Nothing, "string", lrRDSColumn.DataType.DataType) & """, ""nullable"": """ & LCase(lrRDSColumn.IsNullable.ToString) & """}"

            Dim lrNewPropertyTreeNode = New TreeNode(lsPropertyEmbellishment, 4, 4)
            lrNewPropertyTreeNode.Tag = lrRDSColumn
            loPropertiesTreeNode.Nodes.Add(lrNewPropertyTreeNode)
        Next

    End Sub


    ''' <summary>
    ''' Add Relationship to Schema
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub AddRelationshipToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles AddRelationshipToolStripMenuItem1.Click

        Try

            Dim loTreeNode As TreeNode = Me.TreeView.SelectedNode

            Dim lrModel As FBM.Model = loTreeNode.Parent.Tag.Model

            'Get the Add/Edit Relationship form.
            Dim lfrmCRUDAddEditPGSRelationship As New frmCRUDAddEditRelationship

            'Link the form to the Relational Data Structure, that stores the Schema.
            lfrmCRUDAddEditPGSRelationship.mrRDSModel = lrModel.RDS

            'Create a new Relationship Type.
            Dim lrPGSRelationship = New GSJ.RelationshipObjectType
            lrPGSRelationship.from.ref = "Node Type 1" 'Just use the Ref field as a place holder for the NodeType/Label name.
            lrPGSRelationship.type.ref = "RELATES_TO"
            lrPGSRelationship.to.ref = "Node Type 2" 'Just use the Ref field as a place holder for the NodeType/Label name.

            'Link the form to the new Relationship Type
            lfrmCRUDAddEditPGSRelationship.mrPGSRelationship = lrPGSRelationship
            'Let the form know it is being used for Adding a new Relationship Type. Enables the dropdown CompboBoxes to select Node Types.
            lfrmCRUDAddEditPGSRelationship.mbIsAdd = True

            If lfrmCRUDAddEditPGSRelationship.ShowDialog() = DialogResult.OK Then

                'Get the ModelElement (Node Type Names/Labels)
                Dim lsFromModelElementName = lfrmCRUDAddEditPGSRelationship.mrPGSRelationship.from.ref 'E.g. Row
                Dim lsToModelElementName = lfrmCRUDAddEditPGSRelationship.mrPGSRelationship.to.ref 'E.g. Cinema
                'Get the Label/Type of the Relationship. E.g. IS_IN
                Dim lsGraphLabel = lfrmCRUDAddEditPGSRelationship.mrPGSRelationship.type.ref

                'Create a new TreeNode
                Dim loRelationshipTreeNode As New TreeNode($"({lsFromModelElementName})-[:{lsGraphLabel}]->({lsToModelElementName})", 2, 2)
                'Set the tag to the PGSRelationship (Property Graph Schema, Relationship).
                loRelationshipTreeNode.Tag = lfrmCRUDAddEditPGSRelationship.mrPGSRelationship

                loTreeNode.Nodes.Add(loRelationshipTreeNode)
                loTreeNode.Expand()

            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    ''' <summary>
    ''' Add Relationship to Schema
    ''' </summary>
    Private Sub AddRelationshipToTreeView(ByRef arSchemaTreeNode As TreeNode, ByRef arRDSRelationship As RDS.Relation)

        Try

            Dim loTreeNode As TreeNode = arSchemaTreeNode.Nodes(1)

            Dim lrModel As FBM.Model = loTreeNode.Parent.Tag.Model


            Dim lsFromModelElementName = arRDSRelationship.OriginTable.Name
            Dim lsToModelElementName = arRDSRelationship.DestinationTable.Name
            Dim lsGraphLabel = arRDSRelationship.ResponsibleFactType.GraphLabel(0).Label

            Dim loRelationshipTreeNode As New TreeNode($"({lsFromModelElementName})-[:{lsGraphLabel}]->({lsToModelElementName})", 2, 2)
            loRelationshipTreeNode.Tag = arRDSRelationship

            loTreeNode.Nodes.Add(loRelationshipTreeNode)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    ''' <summary>
    ''' Add Relationship to Schema
    ''' </summary>
    Private Sub AddRelationshipToTreeView(ByRef arSchemaTreeNode As TreeNode, ByRef arFBMFactType As FBM.FactType)

        Try
            Dim lrFBMFactType As FBM.FactType = arFBMFactType

            Dim loTreeNode As TreeNode = arSchemaTreeNode.Nodes(1)

            Dim lrModel As FBM.Model = loTreeNode.Parent.Tag.Model

            For Each lrGraphLabel In arFBMFactType.GraphLabel

                Dim larRelationshipRole = (From Role In lrFBMFactType.RoleGroup.FindAll(Function(x) x.JoinsValueType Is Nothing)
                                           Select Role).ToList

                Dim lsFromModelElementName = larRelationshipRole(0).JoinedORMObject.Id
                Dim lsToModelElementName = larRelationshipRole(1).JoinedORMObject.Id
                Dim lsGraphLabel = lrGraphLabel.Label

                Dim loRelationshipTreeNode As New TreeNode($"({lsFromModelElementName})-[:{lsGraphLabel}]->({lsToModelElementName})", 2, 2)
                loRelationshipTreeNode.Tag = arFBMFactType

                loTreeNode.Nodes.Add(loRelationshipTreeNode)

                '==========================================================
                'Properties
                Dim loPropertiesTreeNode As New TreeNode("Properties")
                loPropertiesTreeNode.Tag = pcenumSchemaTreeMenuType.Properties
                loRelationshipTreeNode.Nodes.Add(loPropertiesTreeNode)

                Dim lrRDSTable As RDS.Table = arFBMFactType.getCorrespondingRDSTable

                For Each lrRDSColumn In lrRDSTable.Column

                    Dim lsPropertyEmbellishment = lrRDSColumn.Name & " { ""type"": """ & If(lrRDSColumn.DataType Is Nothing, "string", lrRDSColumn.DataType.DataType) & """, ""nullable"": """ & LCase(lrRDSColumn.IsNullable.ToString) & """}"

                    Dim lrNewPropertyTreeNode = New TreeNode(lsPropertyEmbellishment, 4, 4)
                    lrNewPropertyTreeNode.Tag = lrRDSColumn
                    loPropertiesTreeNode.Nodes.Add(lrNewPropertyTreeNode)
                Next

            Next

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Sub DrawCustomText(graphics As Graphics, text As String, font As Font, startAt As Point, patterns As Dictionary(Of String, Color))
        Dim currentPosition As Integer = 0
        Dim drawPositionX As Integer = startAt.X

        ' Clear the area to prevent shadow effect
        graphics.FillRectangle(Brushes.White, New Rectangle(startAt, New Size(text.Length * font.Size, font.Height)))

        ' Prepare to handle each segment of text
        Dim segments As New List(Of Tuple(Of String, Color))
        Dim regex As New Regex(String.Join("|", patterns.Keys))

        ' Identify all matches
        For Each match As Match In regex.Matches(text)
            If match.Index > currentPosition Then
                ' Add preceding black text
                segments.Add(Tuple.Create(text.Substring(currentPosition, match.Index - currentPosition), Color.Black))
            End If
            ' Add colored text
            segments.Add(Tuple.Create(match.Value, patterns.FirstOrDefault(Function(p) New Regex(p.Key).IsMatch(match.Value)).Value))
            currentPosition = match.Index + match.Length
        Next

        ' Add the remaining text if any
        If currentPosition < text.Length Then
            segments.Add(Tuple.Create(text.Substring(currentPosition), Color.Black))
        End If

        ' Draw each segment
        For Each segment In segments
            Using brush As New SolidBrush(segment.Item2)
                graphics.DrawString(segment.Item1, font, brush, New PointF(drawPositionX, startAt.Y))
                ' Measure text width precisely to update position
                drawPositionX += graphics.MeasureString(segment.Item1, font).Width - 2 'Was TextRendered.MeasureString
            End Using
        Next
    End Sub


    'Private Sub DrawCustomText(graphics As Graphics, text As String, font As Font, startAt As Point, patterns As Dictionary(Of String, Color))
    '    Dim currentPosition As Integer = 0
    '    Dim drawPositionX As Integer = startAt.X

    '    ' Clear the area to prevent shadow effect
    '    graphics.FillRectangle(Brushes.White, New Rectangle(startAt, New Size(text.Length * font.Size, font.Height)))

    '    ' Prepare to handle each segment of text
    '    Dim segments As New List(Of Tuple(Of String, Color))
    '    Dim regex As New Regex(String.Join("|", patterns.Keys))

    '    ' Identify all matches
    '    For Each match As Match In regex.Matches(text)
    '        If match.Index > currentPosition Then
    '            ' Add preceding black text
    '            segments.Add(Tuple.Create(text.Substring(currentPosition, match.Index - currentPosition), Color.Black))
    '        End If
    '        ' Add colored text
    '        segments.Add(Tuple.Create(match.Value, patterns.FirstOrDefault(Function(p) New Regex(p.Key).IsMatch(match.Value)).Value))
    '        currentPosition = match.Index + match.Length
    '    Next

    '    ' Add the remaining text if any
    '    If currentPosition < text.Length Then
    '        segments.Add(Tuple.Create(text.Substring(currentPosition), Color.Black))
    '    End If

    '    ' Draw each segment
    '    For Each segment In segments
    '        Using brush As New SolidBrush(segment.Item2)
    '            graphics.DrawString(segment.Item1, font, brush, drawPositionX, startAt.Y)
    '            drawPositionX += TextRenderer.MeasureText(graphics, segment.Item1, font).Width
    '        End Using
    '    Next
    'End Sub

    Private Sub TreeView1_DrawNode(sender As Object, e As DrawTreeNodeEventArgs) Handles TreeView.DrawNode
        'e.DrawDefault = False  ' Prevent default drawing

        'Dim patterns As New Dictionary(Of String, Color) From {
        '{"\([^\)]*\)", Color.Purple},     ' Text in parentheses in purple
        '{"\[:[^\]]*\]", Color.DarkGreen}  ' Text between "[: " and "]" in dark green
        '}

        '' Draw the node text with custom coloring
        'DrawCustomText(e.Graphics, e.Node.Text, Me.TreeView.Font, New Point(e.Bounds.X, e.Bounds.Y), patterns)

        e.DrawDefault = False  ' Prevent default drawing

        ' Define the drawing patterns and corresponding colors
        Dim patterns As New Dictionary(Of String, Color) From {
            {"\([^\)]*\)", Color.Purple},     ' Text in parentheses in purple
            {"\[:[^\]]*\]", Color.DarkGreen}, ' Text between "[: " and "]" in dark green
            {"""[a-z]*"":", Color.Salmon},
            {"""[a-z]*""", Color.SteelBlue}
        }

        ' Set a clipping region to constrain drawing to the node bounds
        Dim savedClip As Region = e.Graphics.Clip
        Dim loSize = New Size(e.Bounds.Size.Width + 28, e.Bounds.Size.Height)
        e.Graphics.SetClip(New Rectangle(e.Bounds.Location, loSize))

        ' Draw the node text with custom coloring
        DrawCustomText(e.Graphics, e.Node.Text, Me.TreeView.Font, New Point(e.Bounds.X, e.Bounds.Y), patterns)

        ' Restore the original clipping region
        e.Graphics.Clip = savedClip
    End Sub

    Private Sub PropertiesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PropertiesToolStripMenuItem.Click

        Dim lrRDSTable As RDS.Table = Me.TreeView.SelectedNode.Tag

        Dim lrFBMModel As FBM.Model = lrRDSTable.Model.Model

        Dim lrFBMValueType As New FBM.ValueType(lrRDSTable.Model.Model, pcenumLanguage.ORMModel, "New Property", True)

        Dim larFBMModelElement As New List(Of FBM.ModelObject)
        larFBMModelElement.Add(lrRDSTable.FBMModelElement)
        larFBMModelElement.Add(lrFBMValueType)

        Dim lsNewFactTypeName = larFBMModelElement(0).Id & "Has" & larFBMModelElement(1).Id
        Dim lrFBMFactType = lrRDSTable.Model.Model.CreateFactType(lsNewFactTypeName, larFBMModelElement, False, True, False, Nothing, True, Nothing, True, False)

        Dim lrRDSColumn As New RDS.Column(lrRDSTable, "New Property", lrFBMFactType.RoleGroup(0), lrFBMFactType.RoleGroup(1), False)

        Dim lrNewPropertyTreeNode = New TreeNode(lrFBMValueType.Name, 4, 4)
        Me.TreeView.SelectedNode.Nodes(0).Nodes.Add(lrNewPropertyTreeNode)
        Me.TreeView.SelectedNode.Nodes(0).Expand()

    End Sub

    Private Sub AddPropertyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddPropertyToolStripMenuItem.Click

        Try
            Dim lrRDSTable As RDS.Table = Me.TreeView.SelectedNode.Parent.Tag

            Dim lrFBMModel As FBM.Model = lrRDSTable.Model.Model

            Dim lrFBMValueType As New FBM.ValueType(lrRDSTable.Model.Model, pcenumLanguage.ORMModel, "New Property", True)

            Dim larFBMModelElement As New List(Of FBM.ModelObject)
            larFBMModelElement.Add(lrRDSTable.FBMModelElement)
            larFBMModelElement.Add(lrFBMValueType)

            Dim lsNewFactTypeName = larFBMModelElement(0).Id & "Has" & larFBMModelElement(1).Id
            Dim lrFBMFactType = lrRDSTable.Model.Model.CreateFactType(lsNewFactTypeName, larFBMModelElement, False, True, False, Nothing, True, Nothing, True, False)

            Dim lrRDSColumn As New RDS.Column(lrRDSTable, "New Property", lrFBMFactType.RoleGroup(0), lrFBMFactType.RoleGroup(1), False)

            Dim lsPropertyEmbellishment = lrFBMValueType.Name & " { ""type"": """ & If(lrRDSColumn.DataType Is Nothing, "string", lrRDSColumn.DataType.DataType) & """, ""nullable"": """ & LCase(lrRDSColumn.IsNullable.ToString) & """}"

            Dim lrNewPropertyTreeNode = New TreeNode(lsPropertyEmbellishment, 4, 4)
            lrNewPropertyTreeNode.Tag = lrRDSColumn
            Me.TreeView.SelectedNode.Parent.Nodes(0).Nodes.Add(lrNewPropertyTreeNode)
            Me.TreeView.SelectedNode.Parent.Nodes(0).Expand()

        Catch ex As Exception

        End Try

    End Sub

    Private Sub AddPropertyToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles AddPropertyToolStripMenuItem1.Click

        Try
            Dim lfrmCRUDAddEditPGSProperty = New frmCRUDAddEditProperty

            If lfrmCRUDAddEditPGSProperty.ShowDialog = DialogResult.OK Then


            End If


        Catch ex As Exception

        End Try

    End Sub

    Private Sub TreeView_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TreeView.AfterSelect

        Try
            'CodeSafe
            If Me.TreeView.SelectedNode.Tag Is Nothing Then Exit Sub

            Select Case Me.TreeView.SelectedNode.Tag.GetType
                Case Is = GetType(RDS.Relation)

                    Dim lfrmFactTypeReadingEditor As New frmToolboxORMReadingEditor

                    lfrmFactTypeReadingEditor = frmMain.ToolboxForms.Find(AddressOf lfrmFactTypeReadingEditor.EqualsByName)

                    Dim lrRDSRelation As RDS.Relation = Me.TreeView.SelectedNode.Tag

                    '=============================================================
                    'FactType Reading Editor
                    If lfrmFactTypeReadingEditor IsNot Nothing Then

                        lfrmFactTypeReadingEditor.zrFactType = lrRDSRelation.ResponsibleFactType

                        Dim lrFactTypeInstance = New FBM.FactTypeInstance
                        lrFactTypeInstance.FactType = lrRDSRelation.ResponsibleFactType
                        lrFactTypeInstance.Model = lrRDSRelation.Model.Model
                        lfrmFactTypeReadingEditor.zrFactTypeInstance = lrFactTypeInstance

                        Call lfrmFactTypeReadingEditor.SetupForm()

                    End If

                    '=============================================================
                    'Properties Grid
                    Dim lrERDRelation As New ERDRelationship(lrRDSRelation.Model.Model,
                                                          Nothing,
                                                          lrRDSRelation.Id, Nothing,
                                                          lrRDSRelation.OriginMultiplicity,
                                                          lrRDSRelation.OriginColumns(0).IsMandatory,
                                                          lrRDSRelation.OriginColumns(0).isPartOfPrimaryKey,
                                                          Nothing,
                                                          lrRDSRelation.DestinationMultiplicity,
                                                          False,
                                                          lrRDSRelation.OriginTable
                                                          )
                    lrERDRelation.RDSRelation = lrRDSRelation

                    Dim lfrmPropertiesGrid As New frmToolboxProperties
                    lfrmPropertiesGrid = frmMain.GetToolboxForm(lfrmPropertiesGrid.Name)

                    If lfrmPropertiesGrid IsNot Nothing Then
                        lfrmPropertiesGrid.SetSelectedObject(lrERDRelation)
                    End If

            End Select


        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Sub AsJSONGraphSchemaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AsJSONGraphSchemaToolStripMenuItem.Click

        Dim lsFolderLocation As String = ""
        Dim lsFileName As String = ""
        'Dim loStreamWriter As StreamWriter ' Create file by FileStream class
        'Dim loXMLSerialiser As XmlSerializer ' Create binary object
        Dim lrModel As FBM.Model
        Dim lrGraphSchemaRepresentation As New GSJ.GraphSchemaRepresentation

        Try
            '-----------------------------------------
            'Get the Model from the selected TreeNode
            '-----------------------------------------
            lrModel = Me.TreeView.SelectedNode.Tag.Model

            '====================================================================
            'Map the Graph Schema from the Fact-Based Model.
            If Not lrGraphSchemaRepresentation.MapFromFBMModel(lrModel) Then
                MsgBox("Fix the model errors, then try again.")
                Exit Sub
            End If

            Dim lsFileLocationName As String = ""


            Dim lrSaveFileDialog As New SaveFileDialog()

            lsFileName = lrModel.Name & ".json"
            lsFileLocationName = lsFileName

            lrSaveFileDialog.Filter = "Graph Schema JSON file (*.json)|*.json"
            lrSaveFileDialog.FilterIndex = 0
            lrSaveFileDialog.RestoreDirectory = True
            lrSaveFileDialog.FileName = lsFileLocationName

            If lrSaveFileDialog.ShowDialog() = DialogResult.OK Then
                lsFileLocationName = lrSaveFileDialog.FileName
            Else
                Exit Sub
            End If

            Dim lrGraphSchemaRepresentationExport As New GSJ.GraphSchemaRepresentationExport(lrGraphSchemaRepresentation)

            Dim json As String = JsonConvert.SerializeObject(lrGraphSchemaRepresentationExport, Formatting.Indented)
            File.WriteAllText(lsFileLocationName, json)

            Console.WriteLine("Object serialized successfully.")


        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Sub TreeView_NodeMouseDoubleClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles TreeView.NodeMouseDoubleClick

        Try
            'CodeSafe
            If Me.TreeView.SelectedNode Is Nothing OrElse Me.TreeView.SelectedNode.Tag Is Nothing Then Exit Sub

            Select Case Me.TreeView.SelectedNode.Tag.GetType
                Case Is = GetType(RDS.Relation)
#Region "Relationship"

                    Dim lrRDSRelation As RDS.Relation = Me.TreeView.SelectedNode.Tag

                    '=============================================================
                    'Properties Grid
#Region "Properties Grid - Do first"
                    Dim lrERDRelation As New ERDRelationship(lrRDSRelation.Model.Model,
                                                          Nothing,
                                                          lrRDSRelation.Id, Nothing,
                                                          lrRDSRelation.OriginMultiplicity,
                                                          lrRDSRelation.OriginColumns(0).IsMandatory,
                                                          lrRDSRelation.OriginColumns(0).isPartOfPrimaryKey,
                                                          Nothing,
                                                          lrRDSRelation.DestinationMultiplicity,
                                                          False,
                                                          lrRDSRelation.OriginTable
                                                          )
                    lrERDRelation.RDSRelation = lrRDSRelation

                    Dim lfrmPropertiesGrid As New frmToolboxProperties
                    lfrmPropertiesGrid = frmMain.GetToolboxForm(lfrmPropertiesGrid.Name)

                    If lfrmPropertiesGrid IsNot Nothing Then
                        lfrmPropertiesGrid.SetSelectedObject(lrERDRelation)
                    End If
#End Region

#Region "FactTypeReading Editor"
                    Dim lfrmFactTypeReadingEditor As New frmToolboxORMReadingEditor

                    lfrmFactTypeReadingEditor = frmMain.ToolboxForms.Find(AddressOf lfrmFactTypeReadingEditor.EqualsByName)

                    '=============================================================
                    'FactType Reading Editor
                    If lfrmFactTypeReadingEditor IsNot Nothing Then

                        lfrmFactTypeReadingEditor.zrFactType = lrRDSRelation.ResponsibleFactType

                        Dim lrFactTypeInstance = New FBM.FactTypeInstance
                        lrFactTypeInstance.FactType = lrRDSRelation.ResponsibleFactType
                        lrFactTypeInstance.Model = lrRDSRelation.Model.Model
                        lfrmFactTypeReadingEditor.zrFactTypeInstance = lrFactTypeInstance

                        Call lfrmFactTypeReadingEditor.SetupForm()

                    End If
#End Region

                    Dim lrModel As FBM.Model = Me.TreeView.SelectedNode.Parent.Parent.Tag.Model

                    Dim lfrmCRUDAddEditPGSRelationship As New frmCRUDAddEditRelationship
                    lfrmCRUDAddEditPGSRelationship.mrRDSModel = lrModel.RDS

                    Dim lrPGSRelationship = New GSJ.RelationshipObjectType
                    lrPGSRelationship.from.ref = lrRDSRelation.OriginTable.Name
                    lrPGSRelationship.type.ref = lrRDSRelation.ResponsibleFactType.PropertyGraphLabel
                    lrPGSRelationship.to.ref = lrRDSRelation.DestinationTable.Name

                    lfrmCRUDAddEditPGSRelationship.mrPGSRelationship = lrPGSRelationship

                    If lfrmCRUDAddEditPGSRelationship.ShowDialog() = DialogResult.OK Then

                        Dim lsFromModelElementName = lfrmCRUDAddEditPGSRelationship.mrPGSRelationship.from.ref
                        Dim lsToModelElementName = lfrmCRUDAddEditPGSRelationship.mrPGSRelationship.to.ref
                        Dim lsGraphLabel = lfrmCRUDAddEditPGSRelationship.mrPGSRelationship.type.ref

                        Me.TreeView.SelectedNode.Text = $"({lsFromModelElementName})-[:{lsGraphLabel}]->({lsToModelElementName})"

                        If Not lrRDSRelation.ResponsibleFactType.PropertyGraphLabel = lsGraphLabel Then
                            lrRDSRelation.ResponsibleFactType.GraphLabel.RemoveAt(0)
                            lrRDSRelation.ResponsibleFactType.GraphLabel.Add(New RDS.GraphLabel(lrRDSRelation.ResponsibleFactType, lsGraphLabel))
                        End If

                    End If
#End Region
                Case Is = GetType(FBM.FactType)
#Region "FactType"

                    Dim lrFBMFactType As FBM.FactType = Me.TreeView.SelectedNode.Tag

#Region "FactTypeReading Editor"
                    Dim lfrmFactTypeReadingEditor As New frmToolboxORMReadingEditor

                    lfrmFactTypeReadingEditor = frmMain.ToolboxForms.Find(AddressOf lfrmFactTypeReadingEditor.EqualsByName)

                    '=============================================================
                    'FactType Reading Editor
                    If lfrmFactTypeReadingEditor IsNot Nothing Then

                        lfrmFactTypeReadingEditor.zrFactType = lrFBMFactType

                        Dim lrFactTypeInstance = New FBM.FactTypeInstance
                        lrFactTypeInstance.FactType = lrFBMFactType
                        lrFactTypeInstance.Model = lrFBMFactType.Model
                        lfrmFactTypeReadingEditor.zrFactTypeInstance = lrFactTypeInstance

                        Call lfrmFactTypeReadingEditor.SetupForm()

                    End If
#End Region

                    Dim lrModel As FBM.Model = Me.TreeView.SelectedNode.Parent.Parent.Tag.Model

                    Dim lfrmCRUDAddEditPGSRelationship As New frmCRUDAddEditRelationship
                    lfrmCRUDAddEditPGSRelationship.mrRDSModel = lrModel.RDS

                    Dim larModelElement = From Role In lrFBMFactType.RoleGroup
                                          Where Role.JoinsValueType Is Nothing
                                          Select Role.JoinedORMObject

                    Dim lrOriginTable = larModelElement(0).getCorrespondingRDSTable
                    Dim lrDestinationTable = larModelElement(1).getCorrespondingRDSTable

                    Dim lrRDSRelation As New RDS.Relation(System.Guid.NewGuid.ToString,
                                                           lrOriginTable,
                                                           pcenumCMMLMultiplicity.Many,
                                                           False,
                                                           False,
                                                           "Relates To",
                                                           lrDestinationTable,
                                                           pcenumCMMLMultiplicity.Many,
                                                           False,
                                                           "Relates To",
                                                           lrFBMFactType
                                                           )

                    Dim lrPGSRelationship = New GSJ.RelationshipObjectType
                    lrPGSRelationship.from.ref = lrRDSRelation.OriginTable.Name
                    lrPGSRelationship.type.ref = lrRDSRelation.ResponsibleFactType.PropertyGraphLabel
                    lrPGSRelationship.to.ref = lrRDSRelation.DestinationTable.Name

                    lfrmCRUDAddEditPGSRelationship.mrPGSRelationship = lrPGSRelationship

                    If lfrmCRUDAddEditPGSRelationship.ShowDialog() = DialogResult.OK Then

                        Dim lsFromModelElementName = lfrmCRUDAddEditPGSRelationship.mrPGSRelationship.from.ref
                        Dim lsToModelElementName = lfrmCRUDAddEditPGSRelationship.mrPGSRelationship.to.ref
                        Dim lsGraphLabel = lfrmCRUDAddEditPGSRelationship.mrPGSRelationship.type.ref

                        Me.TreeView.SelectedNode.Text = $"({lsFromModelElementName})-[:{lsGraphLabel}]->({lsToModelElementName})"

                        If Not lrRDSRelation.ResponsibleFactType.PropertyGraphLabel = lsGraphLabel Then
                            lrRDSRelation.ResponsibleFactType.GraphLabel.RemoveAt(0)
                            lrRDSRelation.ResponsibleFactType.GraphLabel.Add(New RDS.GraphLabel(lrRDSRelation.ResponsibleFactType, lsGraphLabel))
                        End If

                    End If
#End Region
            End Select


        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Sub SourceDatabaseSQLiteConnectTo()

        Try
            Dim lrOpenFileDialog As New OpenFileDialog

            lrOpenFileDialog.Filter = "SQLite database file (*.db)|*.db"
            lrOpenFileDialog.FilterIndex = 0
            lrOpenFileDialog.RestoreDirectory = True

            Dim lsConnectionString As String = ""

            If (lrOpenFileDialog.ShowDialog() = DialogResult.OK) Then
                If System.IO.File.Exists(lrOpenFileDialog.FileName()) Then
                    lsConnectionString = "Data Source=" & lrOpenFileDialog.FileName & ";Version=3;"
                    Me.ToolStripLabelDatabaseName.Text = lrOpenFileDialog.FileName
                    Me.ToolStripLabelPromptSourceDatabase.Visible = True
                    Me.ToolStripLabelDatabaseName.Visible = True

                    Dim lrFBMModel As New FBM.Model(Path.GetFileName(lrOpenFileDialog.FileName), System.Guid.NewGuid.ToString)

                    lrFBMModel.AddCore()
                    lrFBMModel.RDSCreated = True
                    lrFBMModel.StoreAsXML = True


                    lrFBMModel.DatabaseConnectionString = lsConnectionString
                    lrFBMModel.TargetDatabaseType = pcenumDatabaseType.SQLite
                    lrFBMModel.TargetDatabaseConnectionString = lsConnectionString

                    lrFBMModel.DatabaseManager.establishConnection(lrFBMModel.TargetDatabaseType, lrFBMModel.TargetDatabaseConnectionString)

                    lrFBMModel.connectToDatabase()
                    lrFBMModel.DatabaseConnection.getDatabaseDataTypes()

                    '===================================================================
                    'Reverse Engineer the Database to a Fact-Based Model.
                    Dim lrBackgroundWorker As New System.ComponentModel.BackgroundWorker()
                    lrBackgroundWorker.WorkerReportsProgress = True
                    Dim lrReverseEngineerTool As New ODBCDatabaseReverseEngineer(lrFBMModel, lsConnectionString, False, lrBackgroundWorker)

                    pbReverseEngineeringKeepDatabaseColumnNames = True

                    Dim lsErrorMessage As String = ""
                    Call lrReverseEngineerTool.ReverseEngineerDatabase(lsErrorMessage)

                    Call Me.AddSchemaByFBMModel(lrFBMModel)

                End If
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try


    End Sub

    Private Sub AddSchemaByFBMModel(ByRef arFBMModel As FBM.Model)

        Try
            Dim loSchemaTreeNode = New TreeNode(arFBMModel.Name)

            loSchemaTreeNode.Tag = arFBMModel.RDS

            Me.TreeView.Nodes(0).Nodes.Add(loSchemaTreeNode)

            Dim loTreeNode As TreeNode = New TreeNode("Node Types", 7, 7)
            loTreeNode.Tag = New tSchemaTreeMenuType(pcenumSchemaTreeMenuType.NodeTypes)
            loSchemaTreeNode.Nodes.Add(loTreeNode)

            loTreeNode = New TreeNode("Relationship Types", 8, 8)
            loTreeNode.Tag = New tSchemaTreeMenuType(pcenumSchemaTreeMenuType.Relationships)
            loSchemaTreeNode.Nodes.Add(loTreeNode)

            For Each lrRDSTable In arFBMModel.RDS.Table.FindAll(Function(x) Not x.FBMModelElement.IsCandidatePGSRelationshipNode)
                Call Me.AddNodeToTreeView(loSchemaTreeNode, lrRDSTable)
            Next

            For Each lrRDSRelationship In arFBMModel.RDS.Relation.FindAll(Function(x) Not x.ResponsibleFactType.IsLinkFactType Or Not (x.ResponsibleFactType.IsLinkFactType AndAlso x.ResponsibleFactType.LinkFactTypeRole.FactType.IsCandidatePGSRelationshipNode))
                Call Me.AddRelationshipToTreeView(loSchemaTreeNode, lrRDSRelationship)
            Next

            For Each lrPGSRelationshipNodeFactType In arFBMModel.FactType.FindAll(Function(x) x.IsCandidatePGSRelationshipNode)

                Call Me.AddRelationshipToTreeView(loSchemaTreeNode, lrPGSRelationshipNodeFactType)

            Next

            loSchemaTreeNode.Expand()
            loSchemaTreeNode.EnsureVisible()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub SQLiteConnectToToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles SQLiteConnectToToolStripMenuItem1.Click

        Call Me.SourceDatabaseSQLiteConnectTo()

    End Sub

    Private Sub TreeView_AfterLabelEdit(sender As Object, e As NodeLabelEditEventArgs) Handles TreeView.AfterLabelEdit

        Dim lsMessage As String

        Try
            Dim lrTreeNode As TreeNode = e.Node

            'CodeSafe
            If e.Label = "" Then
                Exit Sub
            End If

            Dim lsNewModelElementName As String = FEStrings.MakeCapCamelCase(e.Label.Trim, True)

            Select Case lrTreeNode.Tag.GetType

                Case Is = GetType(RDS.Table)

                    Dim lrRDsTable As RDS.Table = lrTreeNode.Tag

                    Dim lrFBMModel As FBM.Model = lrRDsTable.Model.Model

                    Dim lrFBMModelElement = lrFBMModel.GetModelObjectByName(lsNewModelElementName)

                    If lrFBMModelElement Is Nothing Then

                        Call lrRDsTable.FBMModelElement.setName(lsNewModelElementName, False)
                        e.CancelEdit = True
                        lrTreeNode.Text = "(" & lsNewModelElementName & ")"

                    ElseIf lrFBMModelElement.Id = lrRDsTable.FBMModelElement.Id Then
                        lsMessage = "The model element, " & lrTreeNode.Text.Trim & ", already exists in the Model"
                        ShowFlashCard(lsMessage, Color.Salmon)
                        e.CancelEdit = True
                        Exit Sub
                    Else
                        'Changed to self. Nothing to do here.
                    End If
            End Select



        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Sub TreeView_BeforeLabelEdit(sender As Object, e As NodeLabelEditEventArgs) Handles TreeView.BeforeLabelEdit

        Try
            Dim lrTreeNode As TreeNode = e.Node

            'CodeSafe
            If lrTreeNode.Tag Is Nothing Then
                e.CancelEdit = True
                Exit Sub
            End If

            Select Case lrTreeNode.Tag.GetType
                Case Is = GetType(RDS.Table)
                    Dim lrRDSTable As RDS.Table = lrTreeNode.Tag
                    lrTreeNode.Text = lrRDSTable.Name
                Case Else
                    e.CancelEdit = True
            End Select

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,)
        End Try
    End Sub

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click

        Try
            Dim lrTreeNode As TreeNode = Me.TreeView.SelectedNode

            Dim lrRDSTable As RDS.Table = lrTreeNode.Tag

            lrTreeNode.Text = lrRDSTable.Name.Trim
            Call lrTreeNode.BeginEdit()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Sub DeleteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteToolStripMenuItem.Click

        Dim lsMessage As String

        Try
            Dim lrFBMModel As FBM.Model = Me.TreeView.SelectedNode.Tag.Model

            lsMessage = "Are you sure you want to delete the Schema, " & lrFBMModel.Name & "?"

            If MsgBox(lsMessage, MsgBoxStyle.YesNoCancel) = MsgBoxResult.Yes Then

                'Delete the Fact-Based Model (representing the Graph Schema) from the database.
                TableModel.DeleteModel(lrFBMModel)

                Me.TreeView.Nodes.Remove(Me.TreeView.SelectedNode)

            End If

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try
    End Sub

    Private Sub FactBasedModelfbmToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FactBasedModelfbmToolStripMenuItem.Click

        Dim lsMessage As String

        Try

            Dim loDialogOpenFile = New OpenFileDialog

            loDialogOpenFile.DefaultExt = "xml"
            loDialogOpenFile.Filter = "FBM Files (*.fbm)|*.fbm"
            loDialogOpenFile.Filter &= "|XML Files (*.xml)|*.xml"

            If loDialogOpenFile.ShowDialog() = System.Windows.Forms.DialogResult.OK Then

                Try
                    With New WaitCursor
                        '=====================================================================================================
                        'Find the version of the XML's XSD                        
                        Call Me.loadFBMXMLFile2(loDialogOpenFile.FileName)
                    End With

                Catch ex As Exception
                    Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                    frmMain.Cursor = Cursors.Default

                    lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                    lsMessage &= vbCrLf & vbCrLf & ex.Message
                    prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
                End Try

            End If

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub



    Public Sub loadFBMXMLFile2(ByVal asFileName As String)

        Try
            Dim xml As XDocument = Nothing
            Dim lsXSDVersionNr As String = ""
            Dim lrModel As New FBM.Model
            Dim lsMessage As String

            If asFileName = "" Then
                Exit Sub
            End If

            'Deserialize text file to a new object.
            Dim objStreamReader As New StreamReader(asFileName)

            xml = XDocument.Load(asFileName)

            'WriteToStatusBar("Loading model.", True)

            lsXSDVersionNr = xml.<Model>.@XSDVersionNr
            '=====================================================================================================
            Dim lrSerializer As XmlSerializer = Nothing
            Select Case lsXSDVersionNr
                Case Is = "0.81"
                    lrSerializer = New XmlSerializer(GetType(XMLModelv081.Model))
                    Dim lrXMLModel As New XMLModelv081.Model
                    lrXMLModel = lrSerializer.Deserialize(objStreamReader)
                    objStreamReader.Close()
                    lrModel = lrXMLModel.MapToFBMModel
                Case Is = "1"
                    lrSerializer = New XmlSerializer(GetType(XMLModel1.Model))
                    Dim lrXMLModel As New XMLModel1.Model
                    lrXMLModel = lrSerializer.Deserialize(objStreamReader)
                    objStreamReader.Close()
                    lrModel = lrXMLModel.MapToFBMModel
                Case Is = "1.1"
                    lrSerializer = New XmlSerializer(GetType(XMLModel11.Model))
                    Dim lrXMLModel As New XMLModel11.Model
                    lrXMLModel = lrSerializer.Deserialize(objStreamReader)
                    objStreamReader.Close()
                    lrModel = lrXMLModel.MapToFBMModel
                Case Is = "1.2"
                    lrSerializer = New XmlSerializer(GetType(XMLModel12.Model))
                    Dim lrXMLModel As New XMLModel12.Model
                    lrXMLModel = lrSerializer.Deserialize(objStreamReader)
                    objStreamReader.Close()
                    lrModel = lrXMLModel.MapToFBMModel
                Case Is = "1.3"
                    lrSerializer = New XmlSerializer(GetType(XMLModel13.Model))
                    Dim lrXMLModel As New XMLModel13.Model
                    lrXMLModel = lrSerializer.Deserialize(objStreamReader)
                    objStreamReader.Close()

                    lrModel = lrXMLModel.MapToFBMModel
                Case Is = "1.4"
                    lrSerializer = New XmlSerializer(GetType(XMLModel14.Model))
                    Dim lrXMLModel As New XMLModel14.Model
                    lrXMLModel = lrSerializer.Deserialize(objStreamReader)
                    objStreamReader.Close()
                    lrModel = lrXMLModel.MapToFBMModel
                Case Is = "1.5"
                    lrSerializer = New XmlSerializer(GetType(XMLModel15.Model))
                    Dim lrXMLModel As New XMLModel15.Model
                    lrXMLModel = lrSerializer.Deserialize(objStreamReader)
                    objStreamReader.Close()
                    lrModel = lrXMLModel.MapToFBMModel
                Case Is = "1.6"
                    lrSerializer = New XmlSerializer(GetType(XMLModel16.Model))
                    Dim lrXMLModel As New XMLModel16.Model
                    lrXMLModel = lrSerializer.Deserialize(objStreamReader)
                    objStreamReader.Close()
                    lrModel = lrXMLModel.MapToFBMModel
                Case Is = "1.7"
                    lrSerializer = New XmlSerializer(GetType(XMLModel.Model))
                    Dim lrXMLModel As New XMLModel.Model
                    lrXMLModel = lrSerializer.Deserialize(objStreamReader)
                    objStreamReader.Close()
                    lrModel = lrXMLModel.MapToFBMModel
            End Select

            If TableModel.ExistsModelById(lrModel.ModelId) Then
                lsMessage = "A Model with the Id: " & lrModel.ModelId
                lsMessage &= vbCrLf & "already exists in the database."
                lsMessage &= vbCrLf & vbCrLf
                lsMessage &= "The Model that you are loading will be given a new Id. All Pages within the Model will also be given a new Id."
                lsMessage &= vbCrLf & "NB The name of the Model ('" & lrModel.Name & "') will stay the same if there is no other Model in the database with the same name."
                lrModel.ModelId = System.Guid.NewGuid.ToString
                '---------------------------------------------------------------------------------------------
                'All of the Page.Ids must be updated as well, as each PageId is unique in the database.
                '  i.e. If the Model is not unique, there's a very good chance that neither are the PageIds.
                '---------------------------------------------------------------------------------------------
                Dim lrPage As FBM.Page
                For Each lrPage In lrModel.Page
                    lrPage.PageId = System.Guid.NewGuid.ToString
                Next

                lrModel.MakeDirty(False, True)

                'MsgBox(lsMessage)
            End If

            If TableModel.ExistsModelByName(lrModel.Name) Then
                lsMessage = "A Model with the Name: " & lrModel.Name
                lsMessage &= vbCrLf & "already exists in the database."
                lsMessage &= vbCrLf & vbCrLf
                lrModel.Name = lrModel.CreateUniqueModelName(lrModel.Name, 0)
                lsMessage &= "The Model that you are loading will be given the new Name: " & lrModel.Name
                'MsgBox(lsMessage)
            End If

            '-----------------------------------------
            'Update the TreeView
            '-----------------------------------------
            Dim lrNewTreeNode = Me.AddFBMModelAsSchemaToTree(lrModel)

            lrNewTreeNode.Expand()
            Me.TreeView.Nodes(0).Nodes(Me.TreeView.Nodes(0).Nodes.Count - 1).EnsureVisible()

            Dim lfrmFlashCard As New frmFlashCard

            '================================================================================================================
            'RDS
            If (lrModel.ModelId <> "Core") And lrModel.HasCoreModel Then
                Call lrModel.performCoreManagement(False)
                Call lrModel.PopulateAllCoreStructuresFromCoreMDAElements()
                lrModel.RDSCreated = True
            ElseIf (lrModel.ModelId <> "Core") Then
                Call lrModel.performCoreManagement(False)
                Call lrModel.createEntityRelationshipArtifacts()
                Call lrModel.PopulateAllCoreStructuresFromCoreMDAElements()
                lrModel.RDSCreated = True
            End If

            frmMain.Cursor = Cursors.Default

            'Baloon Tooltip
            lsMessage = "Loaded"

            '----------------------------------------------------------------------------------------------------------------
            'Saving the Model
            Dim lrCustomMessageBox As New frmCustomMessageBox

            lsMessage = "Your Model has been successfully loaded into Boston." & vbCrLf & vbCrLf
            lsMessage &= "Save the model now? (Recommended)"

            lrCustomMessageBox.Message = lsMessage
            lrCustomMessageBox.ButtonText.Add("No")
            lrCustomMessageBox.ButtonText.Add("Save to database")
            lrCustomMessageBox.ButtonText.Add("Store as XML")

            lfrmFlashCard = New frmFlashCard
            lfrmFlashCard.ziIntervalMilliseconds = 3500
            lfrmFlashCard.zsText = "Saving model."

            Select Case lrCustomMessageBox.ShowDialog
                Case Is = "Store as XML"
                    lrModel.StoreAsXML = True
                    'WriteToStatusBar("Saving Model: " & lrModel.Name)
                    Call lrModel.Save(True, False)
                    'WriteToStatusBar("Model Saved")
                Case Is = "Save to database"
                    With New WaitCursor
                        'WriteToStatusBar("Saving Model: " & lrModel.Name)
                        lfrmFlashCard.Show(Me)
                        lrModel.StoreAsXML = False
                        For Each lrPage In lrModel.Page
                            Call lrPage.LoadFromXMLConceptInstances()
                        Next
                        Call lrModel.Save(True, True)
                        'WriteToStatusBar("Model Saved")
                    End With
            End Select

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            frmMain.Cursor = Cursors.Default

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Function AddFBMModelAsSchemaToTree(ByRef arFRMModel As FBM.Model) As TreeNode

        Try
            Dim lrNewTreeNode = New TreeNode("Schema: " & arFRMModel.Name)

            Me.TreeView.Nodes(0).Nodes.Add(lrNewTreeNode)

            Return lrNewTreeNode

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return Nothing
        End Try

    End Function

    Private Sub GraphSchemaJSONToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GraphSchemaJSONToolStripMenuItem.Click

        Dim lsMessage As String

        Try
            Call Me.ImportGraphSchemaJSONFile()

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Sub ImportGraphSchemaJSONFile()

        Dim lsMessage As String

        Try
            Dim loDialogOpenFile = New OpenFileDialog

            loDialogOpenFile.DefaultExt = "json"
            loDialogOpenFile.Filter = "JSON Files (*.json)|*.json"

            If loDialogOpenFile.ShowDialog() = System.Windows.Forms.DialogResult.OK Then

                Try
                    With New WaitCursor
                        '=====================================================================================================
                        'Load the appropriate version of the Graph Schema JSON based on its json schema/POCO class structure.                        
                        '  NB Converts the Graph Schema JSON into a Fact-Based Model, as used by this software.
                        'See for reference: Call Me.loadFBMXMLFile2(loDialogOpenFile.FileName)
                        Dim filePath As String = loDialogOpenFile.FileName
                        Dim jsonString As String = File.ReadAllText(filePath)

                        'For testing
                        Dim loJSONObject = JObject.Parse(jsonString)

                        Dim settings As New JsonSerializerSettings() With {
                                                .PreserveReferencesHandling = PreserveReferencesHandling.All,
                                                .MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                                                .Formatting = Formatting.Indented,
                                                .NullValueHandling = NullValueHandling.Include
                                            }
                        Dim lrGraphSchemaRepresentationExport As GSJ.GraphSchemaRepresentationExport = JsonConvert.DeserializeObject(Of GSJ.GraphSchemaRepresentationExport)(jsonString, settings)

                        '================================================
                        'Map to Fact-Based Model.
                        Dim lrFBMModel = lrGraphSchemaRepresentationExport.graphSchemaRepresentation.MapToFBMModel()
                        lrFBMModel.Name = Path.GetFileName(filePath)

                        Call Me.AddSchemaByFBMModel(lrFBMModel)

                    End With

                Catch ex As Exception
                    Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                    frmMain.Cursor = Cursors.Default

                    lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                    lsMessage &= vbCrLf & vbCrLf & ex.Message
                    prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
                End Try

            End If


        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click

        Call Me.ImportGraphSchemaJSONFile()

    End Sub

    Private Sub FromSQLiteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FromSQLiteToolStripMenuItem.Click

        Call Me.SourceDatabaseSQLiteConnectTo()

    End Sub

    Private Sub AddSchemaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddSchemaToolStripMenuItem.Click

        Call Me.AddNewSchema()

    End Sub

    ''' <summary>
    ''' Adds a new Schema to the TreeView
    ''' </summary>
    Public Sub AddNewSchema()

        Try
            '=====================================================================================================
            'Create a Fact-Based Model within which to store the Schema.
            Dim lrFBMModel As New FBM.Model(pcenumLanguage.ORMModel, "New Schema", System.Guid.NewGuid.ToString)

            '============================================================
            'Add the Core MetaModel for Relational Data Structures etc
            lrFBMModel.AddCore()
            lrFBMModel.RDSCreated = True 'Core has been added.
            lrFBMModel.StoreAsXML = True 'FBM Model is stored as XML when it is saved.

            Call Me.AddSchemaByFBMModel(lrFBMModel)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click

        Call Me.AddNewSchema()

    End Sub

    Private Sub frmSchema_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing

        frmMain.mfrmSchemaManager = Nothing

    End Sub

    Private Sub AddNodeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddNodeToolStripMenuItem.Click

        Call Me.AddNodeTypeToSelectedTreeNode(Me.TreeView.SelectedNode)

    End Sub


End Class
