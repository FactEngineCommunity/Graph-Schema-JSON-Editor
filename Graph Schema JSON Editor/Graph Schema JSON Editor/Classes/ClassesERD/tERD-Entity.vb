Imports System.ComponentModel
Imports FactEngineForServices
Imports System.Reflection

''' <summary>
''' PropertiesGrid Toolbox use only. See GSJ.NodeObjectType and GSJ.NodeLabel.
'''   Also see (FEFS) RDS.Table.
''' </summary>
Public Class ERDEntity
    Inherits ERD.Entity 'From FactEngine for Services.

    ''' <summary>
    ''' Can be either an RDS.Relation (Foreign Key Reference) or a RDS.Table (E.g. A Many-to-Many table as a Property Graph Edge Type equivalent).
    ''' </summary>
    Public ModelElement As Object

    Public Shadows WithEvents RDSTable As RDS.Table

    Public Shadows _GraphLabel As New FEStrings.StringCollection

    ''' <summary>
    ''' The TreeNode within the Schema TreeView.
    ''' </summary>
    Public TreeNode As TreeNode

    ''' <summary>
    ''' The Graph Label for the Relationship/Edge Type. NB This may need to be changed to a single String member for Relationships/Edge Types.
    ''' </summary>
    ''' <returns></returns>
    <CategoryAttribute("Relation"),
    Browsable(True),
    [ReadOnly](False),
    DescriptionAttribute("The equivalent Graph label in the Graph View."),
    Editor(GetType(tStringCollectionEditor), GetType(System.Drawing.Design.UITypeEditor))>
    Public Shadows Property GraphLabel() As FEStrings.StringCollection  'NB This is what is edited in the PropertyGrid
        Get
            Dim lasGraphLabel() As String = {}

            lasGraphLabel = (From MEGraphLabel In Me.RDSTable.FBMModelElement.GraphLabel
                             Where MEGraphLabel.Label <> Me.RDSTable.Name
                             Select MEGraphLabel.Label).ToArray


            Me._GraphLabel.Clear()
            'The name of the Node Type is always a Label
            Me._GraphLabel.Add(Me.RDSTable.Name)
            Me._GraphLabel.AddRange(lasGraphLabel)

            Return Me._GraphLabel

        End Get
        Set(ByVal Value As FEStrings.StringCollection)

            Me._GraphLabel = Value

            Dim lrModelElement As FBM.ModelObject = Nothing

            lrModelElement = Me.RDSTable.FBMModelElement

            ' Find synonyms that are in the model but not in the new value
            Dim graphLabelsToRemove = (From MEGraphLabel In lrModelElement.GraphLabel
                                       Where MEGraphLabel.ModelElementId = Me.Id AndAlso Not Value.Contains(MEGraphLabel.Label)).ToList()

            ' Remove synonyms that are no longer present in the new value
            For Each graphLabelToRemove In graphLabelsToRemove
                lrModelElement.GraphLabel.Remove(graphLabelToRemove)
            Next

            ' Add new synonyms that are not in the model
            For Each graphLabelToAdd In Value
                If Not lrModelElement.GraphLabel.Any(Function(s) s.ModelElementId = lrModelElement.Id AndAlso s.Label = graphLabelToAdd) Then
                    lrModelElement.GraphLabel.Add(New RDS.GraphLabel(lrModelElement, graphLabelToAdd))
                End If
            Next

        End Set
    End Property


    ''' <summary>
    ''' Object constructor.
    ''' </summary>
    ''' <param name="arModel"></param>
    ''' <param name="arPage"></param>
    ''' <param name="arCorrespondingRDSTable">If the Relation is a (FEFS) PGSRelation, then has a corresponding Table.</param>
    Public Sub New(ByRef arModel As FBM.Model,
                       ByRef arPage As FBM.Page,
                       ByRef arCorrespondingRDSTable As RDS.Table)

        Me.Model = arModel
        Me.Page = arPage
        Me.RDSTable = arCorrespondingRDSTable
        Me.Id = arCorrespondingRDSTable.Name
        Me.Name = arCorrespondingRDSTable.Name
        Me.ModelElement = arCorrespondingRDSTable '20240525-VM-No sure if used.

    End Sub

    Public Sub RefreshShape(Optional ByVal aoChangedPropertyItem As PropertyValueChangedEventArgs = Nothing,
                            Optional ByVal asSelectedGridItemLabel As String = "")

        Try
            '------------------------------------------------
            'Set the values in the underlying RDS.Relation
            '------------------------------------------------
            If aoChangedPropertyItem IsNot Nothing Then
                Select Case aoChangedPropertyItem.ChangedItem.PropertyDescriptor.Name

                    'Case Is = "EnforcesOnCascadeUpdate"
                    '    Call Me.RDSRelation.SetEnforcesOnCascadeUpdate(Me.EnforcesOnCascadeUpdate)

                    'Case Is = "EnforcesOnCascadeDelete"
                    '    Call Me.RDSRelation.SetEnforcesOnCascadeDelete(Me.EnforcesOnCascadeDelete)

                    Case Is = "Name"

                        'Check to see if the new name already exists
                        Dim lasTableName = Me.RDSTable.Model.Table.FindAll(Function(x) x.Name <> Me.RDSTable.Name).Select(Function(x) x.Name)
                        Dim lsNewTableName = aoChangedPropertyItem.ChangedItem.Value.ToString.Trim

                        If lasTableName.Contains(lsNewTableName) Then
                            MsgBox($"Sorry, a Node Type/Entity with the name, {lsNewTableName}, already exists.", MsgBoxStyle.Critical)
                            'CodeSafe
                            Me.Name = Me.RDSTable.Name
                            Exit Sub
                        Else
                            'CodeSafe
                            Me.RDSTable.FBMModelElement.GraphLabel.RemoveAll(Function(x) x.ModelElement.Id = Me.RDSTable.FBMModelElement.Id And x.Label = Me.RDSTable.Name)

                            Call Me.RDSTable.FBMModelElement.setName(lsNewTableName, False)
                        End If

                    Case Is = "Value"
                        With New WaitCursor
                            Select Case asSelectedGridItemLabel
                                Case Is = "GraphLabel"

                                    'GraphLabel processing.
                                    ' NB ModifyGraphLabel adds a new GraphLabel if not exists
                                    Call Me.RDSTable.FBMModelElement.ModifyorAddGraphLabel(aoChangedPropertyItem.OldValue, aoChangedPropertyItem.ChangedItem.Value.ToString)
                                    If Me.RDSTable.Name = aoChangedPropertyItem.OldValue Then
                                        Call Me.RDSTable.FBMModelElement.setName(aoChangedPropertyItem.ChangedItem.Value.ToString, False) 'Modifying TreeNode below.
                                    End If

                                Case Else
                                    'No other collections at this stage.
                            End Select
                        End With
                End Select
            End If

            '-------------------------------------------------------------------------------------------------------------------------------
            'Removing an item using the UITypeEditor does not trigger a return of aoChangedPropertyItem (As PropertyValueChangedEventArgs).
            '  So we must check each time (back here) whether there is an item to remove from the GraphLabels list for the [ModelElement]Instance.
            Dim lrDataStore As New DataStore.Store
            For Each lsGraphLabel In Me.RDSTable.FBMModelElement.GraphLabel.Select(Function(x) x.Label).ToArray
                If lsGraphLabel IsNot Nothing Then
                    If Not Me._GraphLabel.Contains(lsGraphLabel) Then
                        Call Me.RDSTable.FBMModelElement.GraphLabel.RemoveAll(Function(x) x.ModelElement.Id = Me.RDSTable.FBMModelElement.Id And x.Label = lsGraphLabel)
                    End If
                End If
            Next

            If Me.TreeNode IsNot Nothing Then
                Me.TreeNode.Text = $"({Me.RDSTable.Name})"
                If aoChangedPropertyItem IsNot Nothing AndAlso aoChangedPropertyItem.OldValue IsNot Nothing AndAlso aoChangedPropertyItem.OldValue = Me.RDSTable.Name Then
                    Me.TreeNode.Text = $"({aoChangedPropertyItem.ChangedItem.Value.ToString})"
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

    Private Sub RDSTable_GraphLabelAdded(asNewGraphLabel As String) Handles RDSTable.GraphLabelAdded
        Me._GraphLabel.Add(asNewGraphLabel)
    End Sub

End Class
