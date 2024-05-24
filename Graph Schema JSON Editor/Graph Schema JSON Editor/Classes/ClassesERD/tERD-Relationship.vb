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
            '------------------------------------------------
            'Set the values in the underlying RDS.Relation
            '------------------------------------------------
            If aoChangedPropertyItem IsNot Nothing Then
                Select Case aoChangedPropertyItem.ChangedItem.PropertyDescriptor.Name

                    'Case Is = "EnforcesOnCascadeUpdate"
                    '    Call Me.RDSRelation.SetEnforcesOnCascadeUpdate(Me.EnforcesOnCascadeUpdate)

                    'Case Is = "EnforcesOnCascadeDelete"
                    '    Call Me.RDSRelation.SetEnforcesOnCascadeDelete(Me.EnforcesOnCascadeDelete)

                    'Case Is = "EnforcesReferentialIntegrity"
                    '    Call Me.RDSRelation.SetEnforcesReferentialIntegrity(Me.EnforcesReferentialIntegrity)

                    Case Is = "Value"
                        With New WaitCursor
                            Select Case asSelectedGridItemLabel
                                Case Is = "GraphLabel"

                                    'GraphLabel processing.
                                    Select Case Me.ModelElement.GetType
                                        Case Is = GetType(RDS.Relation)
                                            Call Me.RDSRelation.ResponsibleFactType.ModifyGraphLabel(aoChangedPropertyItem.OldValue, aoChangedPropertyItem.ChangedItem.Value.ToString)
                                        Case Is = GetType(RDS.Table)
                                            Call Me.RDSTable.FBMModelElement.ModifyGraphLabel(aoChangedPropertyItem.OldValue, aoChangedPropertyItem.ChangedItem.Value.ToString)
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
            For Each lsGraphLabel In Me.RDSRelation.ResponsibleFactType.GraphLabel.Select(Function(x) x.Label).ToArray
                If lsGraphLabel IsNot Nothing Then
                    If Not Me._GraphLabel.Contains(lsGraphLabel) Then
                        Select Case Me.ModelElement.GetType
                            Case Is = GetType(RDS.Relation)
                                Call Me.RDSRelation.ResponsibleFactType.GraphLabel.RemoveAll(Function(x) x.ModelElement.Id = Me.RDSRelation.ResponsibleFactType.Id And x.Label = lsGraphLabel)
                            Case Is = GetType(RDS.Table)
                                Call Me.RDSTable.FBMModelElement.GraphLabel.RemoveAll(Function(x) x.ModelElement.Id = Me.RDSRelation.ResponsibleFactType.Id And x.Label = lsGraphLabel)
                        End Select

                    End If
                End If
            Next

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
