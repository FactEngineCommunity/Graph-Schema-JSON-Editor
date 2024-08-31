Imports System.ComponentModel
Imports FactEngineForServices
Imports System.Reflection

''' <summary>
''' PropertiesGrid Toolbox use only. See GSJ.RelationshipObjectType and GSJ.RelationshipType.
'''   Also see (FEFS) RDS.Table (Node Types and Relationships when Many-to-Many table) and RDS.Relation (Foreign Keys).
''' </summary>
Public Class ERDRelationship
    Inherits ERD.Relation 'From FactEngine for Services.

    ''' <summary>
    ''' Can be either an RDS.Relation (Foreign Key Reference) or a RDS.Table (E.g. A Many-to-Many table as a Property Graph Edge Type equivalent).
    ''' </summary>
    Public ModelElement As Object

    Public Shadows WithEvents RDSRelation As RDS.Relation

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
    Browsable(False),
    [ReadOnly](False),
    DescriptionAttribute("The equivalent Graph label in the Graph View."),
    Editor(GetType(tStringCollectionEditor), GetType(System.Drawing.Design.UITypeEditor))>
    Public Shadows Property GraphLabel() As FEStrings.StringCollection  'NB This is what is edited in the PropertyGrid
        Get
            Dim lasGraphLabel() As String = {}

            If Me.RDSTable IsNot Nothing Then
                lasGraphLabel = (From MEGraphLabel In Me.RDSTable.FBMModelElement.GraphLabel
                                 Select MEGraphLabel.Label).ToArray
            ElseIf Me.RDSRelation IsNot Nothing Then
                lasGraphLabel = (From MEGraphLabel In Me.RDSRelation.ResponsibleFactType.GraphLabel
                                 Select MEGraphLabel.Label).ToArray
            End If

            Me._GraphLabel.Clear()
            Me._GraphLabel.AddRange(lasGraphLabel)

            Return Me._GraphLabel

        End Get
        Set(ByVal Value As FEStrings.StringCollection)

            Me._GraphLabel = Value

            Dim lrModelElement As FBM.ModelObject = Nothing

            If Me.RDSTable IsNot Nothing Then
                lrModelElement = Me.RDSTable.FBMModelElement
            ElseIf Me.RDSRelation IsNot Nothing Then
                lrModelElement = Me.RDSRelation.ResponsibleFactType
            End If

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

    Private _RelationshipType As String = "HAS" 'Relationship Type as in Property Graph Schema RT...e.g. (Person)-[:LIKES]->(Film). HAS, IS_FOR, IS_IN etc

    <Browsable(True),
     Description("The type of relationship in the graph."),
     Category("Relationship"),
     DefaultValue("HAS"),
     DisplayName("Relationship Type")>
    Public Property RelationshipType As String
        Get
            'Code Safe
            If Me._GraphLabel.Count = 0 Then
                Me._GraphLabel.Add("HAS")
            End If

            Return Me._GraphLabel(0)
        End Get
        Set(value As String)
            'Code Safe
            If Me._GraphLabel.Count = 0 Then
                Me._GraphLabel.Add(value)
            Else
                Me._GraphLabel(0) = value 'Relationship Types are singular in Property Graph Schemas, as opposed Node Types that can have multiple Labels.
            End If
        End Set
    End Property

    ''' <summary>
    ''' Parameterless Constructor.
    ''' </summary>
    Public Sub New()
    End Sub

    ''' <summary>
    ''' Object constructor.
    ''' </summary>
    ''' <param name="arModel"></param>
    ''' <param name="arPage"></param>
    ''' <param name="asRelationId"></param>
    ''' <param name="arOriginEntity"></param>
    ''' <param name="aiOriginMultiplicity"></param>
    ''' <param name="abOriginMandatory"></param>
    ''' <param name="abOriginContributesToPrimaryKey"></param>
    ''' <param name="arDestinationEntity"></param>
    ''' <param name="aiDestinationMultiplicity">Destination Multiplicity. E.g. Many, One.</param>
    ''' <param name="abDestinationMandatory">True if the Destination of the Relationship is mandatory.</param>
    ''' <param name="arCorrespondingRDSTable">If the Relation is a (FEFS) PGSRelation, then has a corresponding Table.</param>
    ''' <param name="arCorrespondingRDSRelation">If the Relationship is a RDS.Relation(ship) (Foreign Key), then the RDS.Relation.</param>
    Public Sub New(ByRef arModel As FBM.Model,
                       ByRef arPage As FBM.Page,
                       ByVal asRelationId As String,
                       ByRef arOriginEntity As FBM.FactDataInstance,
                       ByVal aiOriginMultiplicity As pcenumCMMLMultiplicity,
                       ByVal abOriginMandatory As Boolean,
                       ByVal abOriginContributesToPrimaryKey As Boolean,
                       ByRef arDestinationEntity As FBM.FactDataInstance,
                       ByVal aiDestinationMultiplicity As pcenumCMMLMultiplicity,
                       ByVal abDestinationMandatory As Boolean,
                       Optional ByRef arCorrespondingRDSTable As RDS.Table = Nothing,
                       Optional ByRef arCorrespondingRDSRelation As RDS.Relation = Nothing)

        Me.Model = arModel
        Me.Page = arPage
        Me.Id = asRelationId

        Me.OriginEntity = arOriginEntity
        Me.OriginMultiplicity = aiOriginMultiplicity
        Me.OriginMandatory = abOriginMandatory
        Me.OriginContributesToPrimaryKey = abOriginContributesToPrimaryKey

        Me.DestinationEntity = arDestinationEntity
        Me.DestinationMultiplicity = aiDestinationMultiplicity
        Me.DestinationMandatory = abDestinationMandatory

        If arCorrespondingRDSTable IsNot Nothing Then
            Me.ModelElement = arCorrespondingRDSTable
            Me.RDSTable = arCorrespondingRDSTable
        Else
            Me.ModelElement = arCorrespondingRDSRelation
            Me.RDSRelation = arCorrespondingRDSRelation
        End If


    End Sub

    Public Sub RefreshShape(Optional ByVal aoChangedPropertyItem As PropertyValueChangedEventArgs = Nothing,
                            Optional ByVal asSelectedGridItemLabel As String = "")

        Try
            Dim lrResponsibleFactType As FBM.FactType = Nothing 'The FactType responsible for the EdgeType/Relationship.

            '------------------------------------------------
            'Set the values in the underlying RDS.Relation
            '------------------------------------------------
            If aoChangedPropertyItem IsNot Nothing Then
                Select Case aoChangedPropertyItem.ChangedItem.PropertyDescriptor.Name

                    'Case Is = "EnforcesOnCascadeUpdate"
                    '    Call Me.RDSRelation.SetEnforcesOnCascadeUpdate(Me.EnforcesOnCascadeUpdate)

                    'Case Is = "EnforcesOnCascadeDelete"
                    '    Call Me.RDSRelation.SetEnforcesOnCascadeDelete(Me.EnforcesOnCascadeDelete)

                    Case Is = "RelationshipType"

                        'GraphLabel processing.
                        Select Case Me.ModelElement.GetType
                            Case Is = GetType(RDS.Relation)

                                lrResponsibleFactType = Me.RDSRelation.ResponsibleFactType

                                'If Me.RDSRelation.ResponsibleFactType.IsLinkFactType Then

                                '    Dim lrObjectifyingFactType = (From FactType In Me.Model.FactType
                                '                                  Where FactType.getLinkFactTypes.Contains(Me.RDSRelation.ResponsibleFactType)
                                '                                  Select FactType).First

                                '    lrResponsibleFactType = lrObjectifyingFactType
                                'End If

                                Call lrResponsibleFactType.ModifyorAddGraphLabel(aoChangedPropertyItem.OldValue, aoChangedPropertyItem.ChangedItem.Value.ToString)
                            Case Is = GetType(RDS.Table)
                                Call Me.RDSTable.FBMModelElement.ModifyorAddGraphLabel(aoChangedPropertyItem.OldValue, aoChangedPropertyItem.ChangedItem.Value.ToString)
                        End Select

                    Case Is = "Value"
                        With New WaitCursor
                            Select Case asSelectedGridItemLabel
                                Case Is = "GraphLabel"

                                    'GraphLabel processing.
                                    Select Case Me.ModelElement.GetType
                                        Case Is = GetType(RDS.Relation)
                                            Call Me.RDSRelation.ResponsibleFactType.ModifyorAddGraphLabel(aoChangedPropertyItem.OldValue, aoChangedPropertyItem.ChangedItem.Value.ToString)
                                        Case Is = GetType(RDS.Table)
                                            Call Me.RDSTable.FBMModelElement.ModifyorAddGraphLabel(aoChangedPropertyItem.OldValue, aoChangedPropertyItem.ChangedItem.Value.ToString)
                                    End Select

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

            Select Case Me.ModelElement.GetType
                Case Is = GetType(RDS.Relation)
                    lrResponsibleFactType = Me.RDSRelation.ResponsibleFactType
                Case Is = GetType(RDS.Table)
                    lrResponsibleFactType = Me.RDSTable.FBMModelElement
            End Select

            For Each lsGraphLabel In lrResponsibleFactType.GraphLabel.Select(Function(x) x.Label).ToArray
                If lsGraphLabel IsNot Nothing Then
                    If Not Me._GraphLabel.Contains(lsGraphLabel) Then
                        Select Case Me.ModelElement.GetType
                            Case Is = GetType(RDS.Relation)
                                Call Me.RDSRelation.ResponsibleFactType.GraphLabel.RemoveAll(Function(x) x.ModelElement.Id = Me.RDSRelation.ResponsibleFactType.Id And x.Label = lsGraphLabel)
                            Case Is = GetType(RDS.Table)
                                Call Me.RDSTable.FBMModelElement.GraphLabel.RemoveAll(Function(x) x.ModelElement.Id = Me.RDSTable.FBMModelElement.Id And x.Label = lsGraphLabel)
                        End Select

                    End If
                End If
            Next

            '=======================================================================================
            'Graphics
            If Me.TreeNode IsNot Nothing Then

                Dim lrOriginTable As RDS.Table = Nothing
                Dim lrDestinationTable As RDS.Table = Nothing

                Select Case Me.ModelElement.GetType
                    Case Is = GetType(RDS.Relation)
                        If Me.RDSRelation.ResponsibleFactType.IsLinkFactType Then

                            Dim lrObjectifyingFactType = (From FactType In Me.Model.FactType
                                                          Where FactType.getLinkFactTypes.Contains(Me.RDSRelation.ResponsibleFactType)
                                                          Select FactType).First

                            'lrResponsibleFactType = lrObjectifyingFactType 'Keep LinkFactType

                            Dim larRelationshipRole = (From Role In lrObjectifyingFactType.RoleGroup.FindAll(Function(x) x.JoinsValueType Is Nothing)
                                                       Select Role).ToList

                            lrOriginTable = larRelationshipRole(0).JoinedORMObject.getCorrespondingRDSTable
                            lrDestinationTable = larRelationshipRole(1).JoinedORMObject.getCorrespondingRDSTable
                        Else
                            lrOriginTable = Me.RDSRelation.OriginTable
                            lrDestinationTable = Me.RDSRelation.DestinationTable
                        End If
                    Case Is = GetType(RDS.Table)
                        Dim larRDSTable = From Column In Me.RDSTable.Column
                                          Where Column.Relation.Count <> 0
                                          Select Column.Relation(0).DestinationTable

                        'If the RDS Table joins more than two RDS Tables, then something will have gone wrong. But is tightly controlled within this app.
                        lrOriginTable = larRDSTable(0)
                        lrDestinationTable = larRDSTable(1)
                End Select

                Me.TreeNode.Text = $"({lrOriginTable.Name})-[:{lrResponsibleFactType.GraphLabel(0).Label}]->({lrDestinationTable.Name})"
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Sub RDSRelation_GraphLabelAdded(asNewGraphLabel As String) Handles RDSRelation.GraphLabelAdded
        Me._GraphLabel.Add(asNewGraphLabel)
    End Sub

    Private Sub RDSTable_GraphLabelAdded(asNewGraphLabel As String) Handles RDSTable.GraphLabelAdded
        Me._GraphLabel.Add(asNewGraphLabel)
    End Sub

End Class
