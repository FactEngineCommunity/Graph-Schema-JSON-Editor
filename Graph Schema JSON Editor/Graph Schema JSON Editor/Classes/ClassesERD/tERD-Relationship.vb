Imports System.ComponentModel
Imports FactEngineForServices
Imports System.Reflection

Public Class ERDRelationship
    Inherits ERD.Relation

    Public Shadows WithEvents RDSRelation As RDS.Relation

    Public Shadows _GraphLabel As New FEStrings.StringCollection

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
    ''' <param name="aiDestinationMultiplicity"></param>
    ''' <param name="abDestinationMandatory"></param>
    ''' <param name="abCorrespondingTable">If the Relation is a PGSRelation, then has a corresponding Table.</param>
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
                       Optional ByRef abCorrespondingTable As RDS.Table = Nothing)

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
                                    Call Me.RDSRelation.ResponsibleFactType.ModifyGraphLabel(aoChangedPropertyItem.OldValue, aoChangedPropertyItem.ChangedItem.Value.ToString)
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
                        Call Me.RDSRelation.ResponsibleFactType.GraphLabel.RemoveAll(Function(x) x.ModelElement.Id = Me.RDSRelation.ResponsibleFactType.Id And x.Label = lsGraphLabel)
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
End Class
