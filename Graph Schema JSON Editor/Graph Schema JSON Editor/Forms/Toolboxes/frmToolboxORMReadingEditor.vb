Imports WeifenLuo.WinFormsUI
Imports System.Text.RegularExpressions
Imports FactEngineForServices
Imports System.Reflection

Public Class frmToolboxORMReadingEditor

    Public zrPage As FBM.Page

    '---------------------------------------------
    'EnterpriseAware (Intellisense like) code
    '---------------------------------------------
    Public ziCurrentTagStart As Integer = 0

    Private zrHashList As New Hashtable
    Private zrTermList As New List(Of String) 'List of ORMObjectTypes within the FactType for which the reading is being created.

    Private FTRScanner As FTR.Scanner
    Private FTRProcessor As New FTR.Processor 'Used for parsing FTR texts as input by the user. 
    Private FTRParser As New FTR.Parser(New FTR.Scanner)
    Private FTRParseTree As New FTR.ParseTree
    Public WithEvents zrTextHighlighter As FTR.TextHighlighter
    Public WithEvents zrGridTextHighlighter As FTR.TextHighlighter

    '----------------------------------------------------
    'Intellisense
    '----------------------------------------------------
    Public AutoComplete As frmAutoComplete
    Public zsIntellisenseBuffer As String = ""

    Private WithEvents _zrFactType As FBM.FactType
    Public Property zrFactType As FBM.FactType
        Get
            Return Me._zrFactType
        End Get
        Set(value As FBM.FactType)
            Me._zrFactType = value
        End Set
    End Property

    Public _FactTypeInstance As FBM.FactTypeInstance
    '20180425-The below screws up serialisation for Copy/Paste for some reason.
    Public Property zrFactTypeInstance() As FBM.FactTypeInstance
        Get
            Return Me._FactTypeInstance
        End Get
        Set(ByVal value As FBM.FactTypeInstance)
            Me._FactTypeInstance = value

            '--------------------------------------------------------------------------------------------------------------
            'zrFactType is used 'withevents' so that if a FactTypeReading is removed from the Model in Client/Server mode
            '  the event is triggered in this form (if this form referenes the FactType for which the FactTypeReading belonged)
            '  and consequently the FactTypeReading is removed from the grid in this form.
            If Me._FactTypeInstance IsNot Nothing Then
                If Me._FactTypeInstance.Model IsNot Nothing Then
                    Me.zrFactType = Me._FactTypeInstance.FactType
                End If
            End If

            If Me._FactTypeInstance Is Nothing Then
                Me.LabelFactTypeName.Text = "<No Fact Type Selected>"
            End If
        End Set
    End Property

    Public Function EqualsByName(ByVal other As Object) As Boolean
        If Me.Name = other.Name Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub frm_orm_reading_editor_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '------------------------------------------------------------------------------------------------
        'NB SetupForm is called from within the FrmORMModel...because the Docking process screws up the 
        '  RichTextbox
        '------------------------------------------------------------------------------------------------

        Me.AutoComplete = New frmAutoComplete(Me.TextboxReading)
        Me.AutoComplete.mbSpaceActionEqualsTabAction = True

    End Sub

    Sub SetupForm(Optional ByRef arPage As FBM.Page = Nothing, Optional ByRef arFactTypeInstance As FBM.FactTypeInstance = Nothing)

        Dim ls_joined_object_name As String = ""

        Try
            FTRScanner = New FTR.Scanner
            FTRParser = New FTR.Parser(FTRScanner)

            If Me.zrTextHighlighter Is Nothing Then
                Me.zrTextHighlighter = New FTR.TextHighlighter(
                               Me.TextboxReading,
                               FTRScanner,
                               FTRParser)
            End If

            '----------------
            'Setup the grid
            '----------------
            Dim colRichText As New tRichtextColumn()
            Me.DataGrid_Readings.Columns.Clear()
            Me.DataGrid_Readings.Columns.Add(colRichText)
            Me.DataGrid_Readings.Columns(0).HeaderText = "Fact Type Reading"
            Me.DataGrid_Readings.Width = Me.Width - (Me.DataGrid_Readings.Left)
            Me.DataGrid_Readings.Columns(0).Width = Me.Width - 160
            Me.DataGrid_Readings.Columns.Add("IsPreferred", "Is Preferred (FT)?")
            Me.DataGrid_Readings.Columns(1).Width = 130
            Me.DataGrid_Readings.Columns.Add("IsPreferredForPredicate ", "Is Preferred (Typed Predicate)?")
            Me.DataGrid_Readings.Columns(2).Width = 250

            Me.DataGrid_Readings.AllowUserToDeleteRows = True

            If Me.zrFactTypeInstance Is Nothing Then Exit Sub



            Me.LabelFactTypeReadingEditor.Text = "Write a Fact Type Reading such as '"
                Dim liInd As Integer
                For Each lrRoleInstance In Me.zrFactTypeInstance.RoleGroup
                    If lrRoleInstance.JoinedORMObject IsNot Nothing Then
                        Me.LabelFactTypeReadingEditor.Text &= lrRoleInstance.JoinedORMObject.Id
                        If liInd = 0 Then
                            Me.LabelFactTypeReadingEditor.Text &= " has "
                        Else
                            If Not liInd = Me.zrFactTypeInstance.RoleGroup.Count - 1 Then
                                Me.LabelFactTypeReadingEditor.Text &= " with "
                            End If
                        End If
                        liInd += 1
                    End If
                Next
                Me.LabelFactTypeReadingEditor.Text &= "'. Then press [Enter] to add the Fact Type Reading to the Fact Type."

                If Me.zrFactTypeInstance.FactType.HasAtLeastOneRolePointingToNothing Then
                    Exit Sub
                End If

                If Me.zrFactTypeInstance.FactType.RoleGroup.FindAll(Function(x) x.JoinedORMObject Is Nothing).Count > 0 Then
                    Me.LabelFactTypeName.Text = "Fact Type contains Roles that point to nothing."
                    Me.LabelFactTypeName.ForeColor = Color.Red
                    Call Me.PopulateDataGridWithFactTypeReadings() 'Their should be none.
                    Exit Sub
                End If

            Me.LabelFactTypeName.Text = Me.zrFactTypeInstance.Name
            Me.LabelFactTypeName.ForeColor = Color.Blue

            TextboxReading.DeselectAll()

            Call Me.PopulateDataGridWithFactTypeReadings()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub



    Private Sub frm_orm_reading_editor_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.zrTextHighlighter.Dispose()

    End Sub

    Private Sub MoveLeftToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MoveLeftToolStripMenuItem.Click

        '---------------------------------------------------
        'User can move items left or right within a Reading
        '---------------------------------------------------
        Dim lsReading As String = TextboxReading.Text
        Dim lsSelection As String = ""
        Dim ls_joined_object_name As String = ""
        Dim liInd As Integer = 0

        Try
            If TextboxReading.SelectionLength > 0 Then
                lsSelection = TextboxReading.SelectedText

                TextboxReading.SelectAll()
                TextboxReading.SelectionProtected = False
                TextboxReading.SelectionColor = Color.Black
                TextboxReading.Text = lsSelection & lsReading

                For liInd = 1 To Me.zrFactTypeInstance.Arity
                    Dim lrJoinedORMObject As Object = Me.zrFactTypeInstance.RoleGroup(liInd - 1).JoinedORMObject
                    ls_joined_object_name = " " & Trim(lrJoinedORMObject.shape.tag.name)
                    TextboxReading.Find(ls_joined_object_name, RichTextBoxFinds.Reverse)
                    TextboxReading.SelectionColor = Color.Blue
                    TextboxReading.SelectionProtected = True
                    TextboxReading.DeselectAll()
                Next
            End If

            TextboxReading.ForeColor = Color.Black
            Call Me.ProtectFactTypeTerms(Me.TextboxReading)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    ''' <summary>
    ''' Clears the Reading TextBox
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click

        Dim lo_sentence As Language.Sentence
        Dim lsWord As String = ""

        Try
            lo_sentence = New Language.Sentence(Me.TextboxReading.Text)

            For Each lsWord In lo_sentence.WordList
                'MsgBox("'" & lsWord & "'")
            Next

            TextboxReading.SelectAll()
            TextboxReading.SelectionProtected = False
            TextboxReading.Text = ""

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub


    ''' <summary>
    ''' 20221012-VM-Schedule for removal. Not called from anywhere.
    ''' </summary>
    Sub PopulateTermList()

        Dim liInd As Integer
        Dim lsJoinedObjectName As String = ""

        Me.zrTermList.Clear()

        Try
            For liInd = 1 To Me.zrFactTypeInstance.Arity
                Select Case Me.zrFactTypeInstance.RoleGroup(liInd - 1).JoinedORMObject.ConceptType
                    Case Is = pcenumConceptType.EntityType
                        lsJoinedObjectName = Trim(Me.zrFactTypeInstance.RoleGroup(liInd - 1).JoinedORMObject.Name)
                    Case Is = pcenumConceptType.FactType
                        lsJoinedObjectName = Trim(Me.zrFactTypeInstance.RoleGroup(liInd - 1).JoinedORMObject.Name)
                    Case Is = pcenumConceptType.ValueType
                        lsJoinedObjectName = Trim(Me.zrFactTypeInstance.RoleGroup(liInd - 1).JoinedORMObject.Name)
                End Select

                '----------------------------------------------------------------
                'Add to the list of ORMObjectTypes within the selected FactType
                '----------------------------------------------------------------
                Me.zrTermList.Add(lsJoinedObjectName)
            Next

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Sub PopulateDataGridWithFactTypeReadings()

        Try
            Dim lrFactTypeReading As FBM.FactTypeReading

            Me.DataGrid_Readings.DataSource = Nothing
            Me.DataGrid_Readings.Refresh()
            Me.DataGrid_Readings.RefreshEdit()
            Me.DataGrid_Readings.Rows.Clear()

            If Me.zrFactTypeInstance.FactType.HasAtLeastOneRolePointingToNothing Then Exit Sub

            'Call Me.PopulateTermList()

            For Each lrFactTypeReading In Me.zrFactTypeInstance.FactType.FactTypeReading
                '-----------------------------------------
                'Add the FactTypeReading to the DataGrid
                '-----------------------------------------
                Me.DataGrid_Readings.Rows.Add()
                Me.DataGrid_Readings.Rows(Me.DataGrid_Readings.Rows.Count - 1).Cells(0).Value = lrFactTypeReading.GetReadingText
                Me.DataGrid_Readings.Rows(Me.DataGrid_Readings.Rows.Count - 1).Cells(1).Value = lrFactTypeReading.IsPreferred
                Me.DataGrid_Readings.Rows(Me.DataGrid_Readings.Rows.Count - 1).Cells(2).Value = lrFactTypeReading.IsPreferredForPredicate
                Me.DataGrid_Readings.Rows(Me.DataGrid_Readings.Rows.Count - 1).Tag = lrFactTypeReading
                Dim loObject As Object = Me.DataGrid_Readings.Rows(Me.DataGrid_Readings.Rows.Count - 1).Cells(0)
                loObject.term_list = Me.zrTermList
            Next

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub


    ''' <summary>
    ''' 20180419-VM-Finally deleted the old code. See below circa 2016
    ''' VM-20160511-This is the new process for the new model that matches the FBMWG08 model for PredicateParts.
    ''' i.e. Using a parser to retrieve FrontText, PreBoundText, PostBoundText and PredicateParts.
    ''' The previous method (GetPredicatePartsFromReading) used a parser written by VM and isn't sufficiently strong enough
    ''' to retrieve FrontText, PreboundText and PostBoundText.
    ''' This new process (will use) uses the TinyPG parser to parse the reading and to retrieve the relevant pieces from the 
    ''' ReadingText.
    ''' <remarks></remarks>
    ''' </summary>
    ''' <param name="asReading">The Fact Type Reading text</param>
    ''' <param name="arFactTypeReading">The Fact Type Reading that is populated as output of this process.</param>
    ''' <returns>TRUE if the FactTypeReading can be gleaned from the supplied Reading text (asReading),
    '''   ELSE RETURNS FALSE.
    ''' arFactTypeReading returns populated if asReading is successfully parsed and all Object Types of the reading match those joined by the Fact Type.
    ''' </returns>
    Private Function GetPredicatePartsFromReadingUsingParser(ByVal asReading As String,
                                                             ByRef arFactTypeReading As FBM.FactTypeReading,
                                                             Optional ByRef aarRoleOrder As List(Of FBM.Role) = Nothing) As Boolean

        Dim lsMessage As String
        Dim lrPredicatePart As FBM.PredicatePart

        Try
            ''Testing
            Me.FTRProcessor.ProcessFTR(asReading, Me.FTRParseTree)

            'OR

            Me.FTRParseTree = Me.FTRParser.Parse(asReading)

            If Me.FTRParseTree.Errors.Count > 0 Then
                '---------------------------------------------------------------------------------------------------
                'Is either an incorrectly formatted FactTypeReading, or is not a FactTypeReading Statement at all.
                '---------------------------------------------------------------------------------------------------
                lsMessage = "That's not a well formatted Fact Type Reading."
                lsMessage &= vbCrLf
                lsMessage.AppendLine("The correct format to use is:")
                lsMessage.AppendLine("Object Types, words start with a capital. E.g. Person")
                lsMessage.AppendLine("Predicates are all lowercase. E.g. is married")
                MsgBox(lsMessage)
                Return False
            Else
                Me.FTRProcessor.FACTTYPEREADINGStatement.FRONTREADINGTEXT = New List(Of String)
                Me.FTRProcessor.FACTTYPEREADINGStatement.MODELELEMENT = New List(Of Object)
                Me.FTRProcessor.FACTTYPEREADINGStatement.PREDICATECLAUSE = New List(Of Object)
                Me.FTRProcessor.FACTTYPEREADINGStatement.UNARYPREDICATEPART = ""
                Me.FTRProcessor.FACTTYPEREADINGStatement.FOLLOWINGREADINGTEXT = ""
                Call Me.FTRProcessor.GetParseTreeTokensReflection(Me.FTRProcessor.FACTTYPEREADINGStatement, Me.FTRParseTree.Nodes(0))

                Dim lsFrontReadingTextWord As String = ""
                arFactTypeReading.FrontText = ""
                For Each lsFrontReadingTextWord In Me.FTRProcessor.FACTTYPEREADINGStatement.FRONTREADINGTEXT
                    arFactTypeReading.FrontText &= lsFrontReadingTextWord
                Next
                arFactTypeReading.FrontText = Trim(arFactTypeReading.FrontText)

                Dim lrModelElementNode As FTR.ParseNode
                Dim lrPredicateClauseNode As FTR.ParseNode
                Dim liInd As Integer = 0
                Dim lasModelObjectId As New List(Of String)

                For liInd = 1 To Me.FTRProcessor.FACTTYPEREADINGStatement.MODELELEMENT.Count

                    lrPredicatePart = New FBM.PredicatePart(arFactTypeReading.Model, arFactTypeReading)
                    lrPredicatePart.SequenceNr = liInd

                    lrModelElementNode = Me.FTRProcessor.FACTTYPEREADINGStatement.MODELELEMENT(liInd - 1)
                    Me.FTRProcessor.MODELELEMENTClause.PREBOUNDREADINGTEXT = ""
                    Me.FTRProcessor.MODELELEMENTClause.POSTBOUNDREADINGTEXT = ""
                    Me.FTRProcessor.MODELELEMENTClause.MODELELEMENTNAME = ""
                    Call Me.FTRProcessor.GetParseTreeTokensReflection(Me.FTRProcessor.MODELELEMENTClause, lrModelElementNode)

                    '------------------------------------------------------------------------------------------------------
                    'Check to see whether the MODELELEMENTNAME is an Object Type that is actually linked by the FactType.
                    '------------------------------------------------------------------------------------------------------
                    Dim lsModelElementName As String = Trim(Me.FTRProcessor.MODELELEMENTClause.MODELELEMENTNAME)
                    If arFactTypeReading.FactType.GetRoleByJoinedObjectTypeId(lsModelElementName) Is Nothing Then
                        MsgBox(lsModelElementName & " is not the name of an Object Type linkd by the Fact Type.")
                        Return False
                    End If

                    lrPredicatePart.PreBoundText = Trim(Me.FTRProcessor.MODELELEMENTClause.PREBOUNDREADINGTEXT)
                    lrPredicatePart.PostBoundText = Trim(Me.FTRProcessor.MODELELEMENTClause.POSTBOUNDREADINGTEXT)

                    Dim lrRole As New FBM.Role(arFactTypeReading.FactType, "TEMP", False, New FBM.ModelObject(lsModelElementName))
                    If arFactTypeReading.FactType.RoleGroup.FindAll(AddressOf lrRole.EqualsByJoinedModelObjectId).Count = 0 Then
                        Return False
                    End If

                    lasModelObjectId.Add(lsModelElementName)
                    Dim lsModelObjectId As String = lsModelElementName
                    Dim lsTempInd As Integer = lasModelObjectId.FindAll(AddressOf lsModelObjectId.Equals).Count

                    If aarRoleOrder Is Nothing Then
                        lrPredicatePart.Role = arFactTypeReading.FactType.GetRoleByJoinedObjectTypeId(lsModelElementName,
                                                                                                      lsTempInd)
                    ElseIf aarRoleOrder(liInd - 1).JoinedORMObject.Id = arFactTypeReading.FactType.GetRoleByJoinedObjectTypeId(lsModelElementName,
                                                                                                                               lsTempInd).JoinedORMObject.Id Then
                        lrPredicatePart.Role = aarRoleOrder(liInd - 1)
                    Else
                        lrPredicatePart.Role = arFactTypeReading.FactType.GetRoleByJoinedObjectTypeId(lsModelElementName,
                                                                                                      lsTempInd)
                    End If

                    arFactTypeReading.RoleList.Add(lrPredicatePart.Role)
                    Dim lsPredicatePartText As String = ""

                    If Me.FTRProcessor.FACTTYPEREADINGStatement.UNARYPREDICATEPART = "" Then
                        '----------------------------------------
                        'FactType is binary or greater in arity
                        '----------------------------------------
                        If liInd < Me.FTRProcessor.FACTTYPEREADINGStatement.MODELELEMENT.Count Then
                            lrPredicateClauseNode = Me.FTRProcessor.FACTTYPEREADINGStatement.PREDICATECLAUSE(liInd - 1)
                            Me.FTRProcessor.PREDICATEPARTClause.PREDICATEPART = New List(Of String)
                            Call Me.FTRProcessor.GetParseTreeTokensReflection(Me.FTRProcessor.PREDICATEPARTClause, lrPredicateClauseNode)

                            For Each lsPredicatePartText In Me.FTRProcessor.PREDICATEPARTClause.PREDICATEPART
                                lrPredicatePart.PredicatePartText &= lsPredicatePartText
                            Next
                        End If

                        lrPredicatePart.PredicatePartText = Trim(lrPredicatePart.PredicatePartText)
                    Else
                        '------------------------------
                        'FactType is a unary FactType
                        '------------------------------
                        lrPredicatePart.PredicatePartText = Trim(Me.FTRProcessor.FACTTYPEREADINGStatement.UNARYPREDICATEPART)
                    End If

                    lrPredicatePart.makeDirty()
                    arFactTypeReading.PredicatePart.Add(lrPredicatePart)
                Next

                '-----------------------------------------------
                'Get the FollowingReadingText if there is one.
                '-----------------------------------------------
                arFactTypeReading.FollowingText = Trim(Me.FTRProcessor.FACTTYPEREADINGStatement.FOLLOWINGREADINGTEXT)

                Return True

            End If

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)

            Return False
        End Try

    End Function

    Public Sub processFactTypeReading(Optional ByVal asFactTypeReading As String = Nothing, Optional ByVal abAskChangeName As Boolean = True)

        Try

            Dim lsFactTypeReading As String = asFactTypeReading

            If asFactTypeReading Is Nothing Then
                lsFactTypeReading = Trim(Me.TextboxReading.Text)
            End If

            'CodeSafe: Check that the FactTypeReading doesn't already exist.
            If Me.zrFactType.ExistsFactTypeReadingByText(lsFactTypeReading) Then
                Exit Sub 'FactTypeReading already exists for the Facttype.
            End If

            With New WaitCursor

                Dim lbFactTypeReadingSuccessfullyCreated As Boolean = False
                Dim lrFactTypeReading As New FBM.FactTypeReading(Me.zrFactTypeInstance.FactType)
                Dim lrFactType As FBM.FactType = Me.zrFactTypeInstance.FactType

                If Me.zrFactTypeInstance.FactType.FactTypeReading.Count = 0 Then
                    lrFactTypeReading.IsPreferred = True
                End If

                'E.g. For Employee reports to Employee on a ManyToOneBinaryFactType...make first Employee referenced Role that with an Internal Uniqueness Constraint.
                Dim larRoleOrder As List(Of FBM.Role) = Nothing
                If lrFactType.IsManyTo1BinaryFactType And
                    lrFactType.FactTypeReading.Count = 0 And
                    lrFactType.HasMoreThanOneRoleReferencingTheSameModelObject Then
                    larRoleOrder = New List(Of FBM.Role)
                    larRoleOrder.Add(lrFactType.RoleGroup.Find(Function(x) x.HasInternalUniquenessConstraint))
                    larRoleOrder.Add(lrFactType.RoleGroup.Find(Function(x) Not x.HasInternalUniquenessConstraint))
                End If

                If Me.GetPredicatePartsFromReadingUsingParser(lsFactTypeReading, lrFactTypeReading, larRoleOrder) Then
                    '-------------------------------------------------
                    'All good. The reading text parsed successfully.
                    '-------------------------------------------------

                    '-------------------------------------------------------------------------------
                    'The first FactTypeReading for a FactType is Preferred for that FactType
                    If Me.zrFactTypeInstance.FactType.FactTypeReading.Count = 0 Then
                        lrFactTypeReading.IsPreferred = True
                    End If

                    '---------------------------------------------------------------------------------------------------------------------
                    'Check FactType.ExistsFactTypeReadingByRoleSequence to see whether a Fact Type already exists for the Role Sequence.
                    '---------------------------------------------------------------------------------------------------------------------
                    If Me.zrFactTypeInstance.FactType.ExistsFactTypeReadingByRoleSequence(lrFactTypeReading) Then

                        lrFactTypeReading.TypedPredicateId = Me.zrFactTypeInstance.FactType.GetTypedPredicateIdByRoleSequence(lrFactTypeReading)

                        lrFactTypeReading.IsPreferredForPredicate = (Me.zrFactTypeInstance.FactType.FactTypeReading.FindAll(AddressOf lrFactTypeReading.EqualsByRoleSequence).Count = 0)

                        '---------------------------------------------------------------------------------------------
                        'Check to see whether the Fact Type has more than one Role referencing the same Object Type.
                        '---------------------------------------------------------------------------------------------
                        If Me.zrFactTypeInstance.FactType.HasMoreThanOneRoleReferencingTheSameModelObject Then
                            '---------------------------------------------------------------------------------------------------
                            'Need to check to see whether an alternate sequence of Roles (within the FTR) is possible for the 
                            '  FactType. It may well be that each possible combination of FTR Role Sequences has been filled
                            '  for the Fact Type, which is a good scenario. If this is the case, then simply ther is no new
                            '  FTR to apply to the Fact Type.
                            '  Otherwise, the algorithm is to select an unused FTR/Role Sequence combination and apply that
                            '  automatically for the Fact Type.
                            '---------------------------------------------------------------------------------------------------
                            If lrFactTypeReading.FactType.ExistsAvailablePermutation(lrFactTypeReading) Then
                                lrFactTypeReading = lrFactTypeReading.FactType.TransformFactTypeReadingToAvailablePermutation(lrFactTypeReading)
                                Me.zrFactTypeInstance.FactType.AddFactTypeReading(lrFactTypeReading, True, True)
                                lbFactTypeReadingSuccessfullyCreated = True
                            End If
                        Else
                            Me.zrFactTypeInstance.FactType.AddFactTypeReading(lrFactTypeReading, True, True)

                            lbFactTypeReadingSuccessfullyCreated = True
                        End If
                    Else
                        Me.zrFactTypeInstance.FactType.AddFactTypeReading(lrFactTypeReading, True, True)

                        lbFactTypeReadingSuccessfullyCreated = True
                    End If 'Preexisting FactTypeReading for Role sequence exists.
                End If 'ReadingText successfully parsed.

                If lbFactTypeReadingSuccessfullyCreated Then

                    '===================================================================================================================================
                    'IMPORTANT: Set the name of the FactType based on the FactTypeReadings of the FactType.
                    Dim lsNewName As String = Me.zrFactTypeInstance.FactType.MakeNameFromFactTypeReadings()
                    Dim lsOldName As String = Me.zrFactTypeInstance.FactType.Id
                    Dim lsMessage = "Change the Fact Type's name to, '" & lsNewName & "'?"
                    If lsNewName <> lsOldName And abAskChangeName Then
                        If MsgBox(lsMessage, MsgBoxStyle.DefaultButton2 + MsgBoxStyle.YesNo, "Rename Fact Type?") = DialogResult.Yes Then

                            Call Me.zrFactTypeInstance.FactType.setName(lsNewName, True)

                            Me.LabelFactTypeName.Text = lsNewName

                            '---------------------------------------------------------------------------------------------
                            'CMML
                            Dim lrRecordset As ORMQL.Recordset
                            Dim lsSQLQuery As String = ""

                            lsSQLQuery = "SELECT *"
                            lsSQLQuery.AppendString(" FROM " & pcenumCMMLRelations.CoreRelationIsForFactType.ToString)
                            lsSQLQuery.AppendString(" WHERE FactType = '" & lsOldName & "'")

                            lrRecordset = Me.zrFactTypeInstance.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            If Not lrRecordset.EOF Then
                                '20180611-VM-If the code never reaches this breakpoint....then FactType.SetName is modifying the appropriate FactData entries.
                                '  so can delete this.
                                '20210410-VM-Checked again and added break (in red Visual Studio). If not getting here, then can delete.
                                Dim lrNewDictionaryEntry As New FBM.DictionaryEntry(Me.zrFactTypeInstance.Model, lsNewName, pcenumConceptType.FactType)
                                lrNewDictionaryEntry = Me.zrFactTypeInstance.Model.AddModelDictionaryEntry(lrNewDictionaryEntry)
                                Call lrRecordset("FactType").SwitchConcept(lrNewDictionaryEntry.Concept, pcenumConceptType.FactType)
                            End If

                            Call Me.zrPage.Model.Save()
                        End If
                    End If

                    Call Me.zrPage.Form.EnableSaveButton()

                    Dim lrSuitableFactTypeReading As FBM.FactTypeReading
                    lrSuitableFactTypeReading = Me.zrFactTypeInstance.FindSuitableFactTypeReading 'Formerly by FactType...ByRoles(larRoles)


                    Me.TextboxReading.Text = ""
                End If
            End With

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    ''' <summary>
    ''' Protects the names of ORM Object Types within the Reading so that
    '''   the User doesn't accidentally delete or change them.
    '''   Makes it easier to structure the Fact Type Reading as a Predicate.
    '''     ''' </summary>
    ''' <param name="aoRichTextBox"></param>
    ''' <remarks></remarks>
    Sub ProtectFactTypeTerms(ByVal aoRichTextBox As Object)

        Dim liInd As Integer
        Dim liInd2 As Integer = 1
        Dim ls_joined_object_name As String = ""
        Dim lsDictionaryItem As System.Collections.DictionaryEntry

        Dim lasSortedModelNameObjectList As New List(Of String)

        Try
            For Each lsDictionaryItem In Me.zrHashList
                lasSortedModelNameObjectList.Add(lsDictionaryItem.Key)
            Next

            Dim lrStringLengthComparerDescending As New Strings.StringLengthComparerDescending
            lasSortedModelNameObjectList.Sort(lrStringLengthComparerDescending)


            aoRichTextBox.SelectAll()
            aoRichTextBox.SelectionProtected = False
            aoRichTextBox.SelectionColor = Color.Black
            aoRichTextBox.DeselectAll()
            aoRichTextBox.SelectionStart = aoRichTextBox.TextLength

            For liInd = 1 To lasSortedModelNameObjectList.Count 'Me.zrFactTypeInstance.Arity
                '------------------------------------------------------
                'Get the Name of the ModelObject within the FactType
                '------------------------------------------------------
                ls_joined_object_name = lasSortedModelNameObjectList(liInd - 1)

                While liInd2 < aoRichTextBox.TextLength
                    If liInd2 <= 0 Then liInd2 = 1

                    liInd2 = aoRichTextBox.Find(ls_joined_object_name, (liInd2 - 1), RichTextBoxFinds.WholeWord)
                    If liInd2 < 0 Then
                        liInd2 = 1
                        Exit While
                    End If

                    aoRichTextBox.SelectionColor = Color.Blue
                    aoRichTextBox.SelectionProtected = True

                    If (aoRichTextBox.SelectionStart + Len(ls_joined_object_name)) >= aoRichTextBox.TextLength Then
                        Exit While
                    ElseIf (aoRichTextBox.SelectionStart = 0) Then
                        Exit While
                    Else
                        liInd2 = aoRichTextBox.SelectionStart + Len(ls_joined_object_name)
                    End If
                End While
            Next

            aoRichTextBox.SelectionStart = aoRichTextBox.TextLength

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Sub DataGrid_Readings_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGrid_Readings.CellClick

        Try
            If e.ColumnIndex = 0 Then
                'Me.DataGrid_Readings.ContextMenuStrip = Nothing
            ElseIf e.ColumnIndex = 1 Then
                Me.DataGrid_Readings.ContextMenuStrip = ContextMenuStripIsPreferred
                Me.DataGrid_Readings.Rows(e.RowIndex).Selected = True
            ElseIf e.ColumnIndex = 2 Then
                Me.DataGrid_Readings.ContextMenuStrip = ContextMenuStripIsPreferredForPredicate
                Me.DataGrid_Readings.Rows(e.RowIndex).Selected = True
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Sub DataGrid_Readings_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGrid_Readings.CellContentClick

        'Me.DataGrid_Readings.SelectionMode = DataGridViewSelectionMode.FullRowSelect

    End Sub


    Private Sub DataGrid_Readings_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGrid_Readings.CellEndEdit

        Dim lrFactTypeReading As FBM.FactTypeReading
        Dim lrFactTypeReadingInstance As FBM.FactTypeReadingInstance

        Try
            lrFactTypeReading = New FBM.FactTypeReading(Me.zrFactTypeInstance.FactType, Me.DataGrid_Readings.Rows(e.RowIndex).Tag.Id)

            Dim lrExistingFactTypeReading As FBM.FactTypeReading = Me.DataGrid_Readings.Rows(e.RowIndex).Tag

            'CodeSafe
#Region "CodeSafe: Make sure the FactTypeReading has the right number of PredicateParts"
            If lrExistingFactTypeReading.PredicatePart.Count < Me.zrFactTypeInstance.FactType.RoleGroup.Count Then
                For Each lrRole In Me.zrFactTypeInstance.FactType.RoleGroup
                    If lrExistingFactTypeReading.PredicatePart.Find(Function(x) x.Role Is lrRole) Is Nothing Then
                        Dim lrPredicatePart As New FBM.PredicatePart(Me.zrFactTypeInstance.Model,
                                                                     lrExistingFactTypeReading,
                                                                     lrRole, True)
                        lrExistingFactTypeReading.AddPredicatePart(lrPredicatePart)
                    End If
                Next
            End If
            If lrExistingFactTypeReading.PredicatePart.Count > Me.zrFactTypeInstance.FactType.RoleGroup.Count Then
                For Each lrRole In Me.zrFactTypeInstance.FactType.RoleGroup
                    If lrExistingFactTypeReading.PredicatePart.Find(Function(x) x.Role Is lrRole) Is Nothing Then
                        Dim lrOffendingPredicatePart = lrExistingFactTypeReading.PredicatePart.Find(Function(x) x.Role Is lrRole)
                        lrExistingFactTypeReading.PredicatePart.Remove(lrOffendingPredicatePart)
                    End If
                Next
            End If
#End Region

            Dim larRoleOrder As New List(Of FBM.Role)
            For Each lrPredicatePart In lrExistingFactTypeReading.PredicatePart
                larRoleOrder.Add(lrPredicatePart.Role)
            Next

            If Me.GetPredicatePartsFromReadingUsingParser(Trim(Me.DataGrid_Readings.CurrentCell.Value), lrFactTypeReading, larRoleOrder) Then

                With New WaitCursor
                    '============================================================================================================================
                    'Code safe. Get rid of any extra PredicateParts 
                    '  in the existing FactTypeReading that should not be there.
                    '  It's not often that it would happen, but if a FactTypeReading ends up with one too many PredicateParts,
                    '  there must be a way to get rid of the extra PrediatePart. This code will get rid of the extra PredicatePart in the exiting
                    '  FactTypeReading.
                    '--------------------------------------------------------------------------
                    Dim lrTestingFactTypeReading As FBM.FactTypeReading
                    lrTestingFactTypeReading = Me.DataGrid_Readings.Rows(e.RowIndex).Tag
                    If lrTestingFactTypeReading.PredicatePart.Count > Me.zrFactTypeInstance.RoleGroup.Count Then
                        Dim lrPredicatePart As New FBM.PredicatePart(Me.zrFactTypeInstance.Model, New FBM.FactTypeReading(Me.zrFactTypeInstance.FactType, lrTestingFactTypeReading.Id))
                        lrPredicatePart.SequenceNr = lrTestingFactTypeReading.PredicatePart.Count
                    End If
                    '===========================================================================================================================

                    Me.DataGrid_Readings.Rows(e.RowIndex).Tag = lrFactTypeReading

                    Dim lrPage As New FBM.Page(Me.zrFactType.Model)

                    lrFactTypeReadingInstance = lrFactTypeReading.CloneInstance(lrPage)

                    Call Me.zrFactTypeInstance.FactType.SetFactTypeReading(lrFactTypeReading, True)

                    If Me.zrFactTypeInstance.FactTypeReading.Equals(lrFactTypeReadingInstance) Then

                        Me.zrFactTypeInstance.FactTypeReadingShape = lrFactTypeReadingInstance
                    End If

                    '===================================================================================================================================
                    'IMPORTANT: Set the name of the FactType based on the FactTypeReadings of the FactType.
                    Dim lsNewName As String = Me.zrFactTypeInstance.FactType.MakeNameFromFactTypeReadings()
                    Dim lsOldName As String = Me.zrFactTypeInstance.FactType.Id
                    If lsNewName <> lsOldName Then

                        If MsgBox("Do you want to change the name of the Fact Type to, " & lsNewName, MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2) = DialogResult.Yes Then
                            Call Me.zrFactTypeInstance.FactType.setName(lsNewName, True)

                            '---------------------------------------------------------------------------------------------
                            'CMML
                            Dim lrRecordset As ORMQL.Recordset
                            Dim lsSQLQuery As String = ""

                            lsSQLQuery = "SELECT *"
                            lsSQLQuery.AppendString(" FROM " & pcenumCMMLRelations.CoreRelationIsForFactType.ToString)
                            lsSQLQuery.AppendString(" WHERE FactType = '" & lsOldName & "'")

                            lrRecordset = Me.zrFactTypeInstance.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            If Not lrRecordset.EOF Then
                                '20180611-VM-If the code never reaches this breakpoint....then FactType.SetName is modifying the appropriate FactData entries.
                                '  so can delete this.
                                Dim lrNewDictionaryEntry As New FBM.DictionaryEntry(Me.zrFactTypeInstance.Model, lsNewName, pcenumConceptType.FactType)
                                lrNewDictionaryEntry = Me.zrFactTypeInstance.Model.AddModelDictionaryEntry(lrNewDictionaryEntry)
                                Call lrRecordset("FactType").SwitchConcept(lrNewDictionaryEntry.Concept, pcenumConceptType.FactType)
                            End If
                        End If
                    End If

                    '=============================================================================
                    'RDS - Change Column.Name if need be.
                    Dim larColumn = From Table In Me.zrFactType.Model.RDS.Table
                                    From Column In Table.Column
                                    Where Column.FactType Is Me.zrFactType
                                    Select Column

                    If larColumn.Count > 0 Then 'CodeSafe. Must have found a Column.
                        If lrFactTypeReading.PredicatePart(0).Role.JoinedORMObject.Id = larColumn.First.Table.Name Then
                            Dim lrColumn = larColumn.First
                            Dim lsNewColumnName As String = lrColumn.getAttributeName
                            Call lrColumn.setName(lsNewColumnName)
                        End If
                    End If

                    Me.zrFactTypeInstance.Model.MakeDirty()

                End With
            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Sub DataGrid_Readings_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGrid_Readings.CellMouseDown

        Try
            If e.ColumnIndex = 0 Then
                If Me.DataGrid_Readings.SelectedRows.Count > 0 Then
                    Me.DataGrid_Readings.ContextMenuStrip = ContextMenuFactTypeReading
                End If
            ElseIf e.ColumnIndex = 1 Then
                Me.DataGrid_Readings.ContextMenuStrip = ContextMenuStripIsPreferred
            ElseIf e.ColumnIndex = 2 Then
                Me.DataGrid_Readings.ContextMenuStrip = ContextMenuStripIsPreferredForPredicate
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub


    Private Sub DataGrid_Readings_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DataGrid_Readings.KeyDown

        Dim lrFactTypeReading As FBM.FactTypeReading

        Try
            If e.KeyCode = Keys.Delete Then
                If Me.DataGrid_Readings.SelectedRows.Count > 0 Then

                    lrFactTypeReading = Me.DataGrid_Readings.SelectedRows(0).Tag

                    Call lrFactTypeReading.RemoveFromModel(False, True, True)

                    Call Me.PopulateDataGridWithFactTypeReadings()
                    Call Me.zrPage.MakeDirty()

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

    Private Sub frmToolboxORMReadingEditor_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        Me.LabelHelpTips.Text = "Double Click over a Fact Type Reading to edit the Fact Type Reading."
        Me.LabelHelpTips.Text &= vbCrLf & "Select (Left Click leftmost column to highlight) a Fact Type Reading to Delete the selected Fact Type Reading."
        Me.LabelHelpTips.Text &= vbCrLf & "Select then press the [Delete] button to delete a selected Fact Type Reading"
        Me.Refresh()
    End Sub

    Private Sub TextboxReading_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextboxReading.KeyPress

        'For [Enter] see KeyDown. Calls Me.processFactTypeReading()

        '-------------------------------------------------------------------------------------------------------------------
        'Update the IntellisenseBuffer
        '  The IntellisenseBuffer is used to limit the number of options provided in the AutoComplete (Intellisense) form.
        '-------------------------------------------------------------------------------------------------------------------
        Dim loRegularExpression As Regex

        Try
            loRegularExpression = New Regex("[a-zA-Z0-9]")
            If loRegularExpression.IsMatch(e.KeyChar) Then
                zsIntellisenseBuffer &= LCase(e.KeyChar)
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Sub TextboxReading_KeyUp(sender As Object, e As KeyEventArgs) Handles TextboxReading.KeyUp

        'For [Enter] see KeyDown. Calls Me.processFactTypeReading()

        Try
            If e.KeyCode = Keys.Down Then
                Call Me.ProcessAutoComplete(e)
            End If

            If e.KeyCode = Keys.Escape Then
                Me.AutoComplete.Hide()
            End If

            If (e.KeyCode = Keys.Enter) And Me.TextboxReading.Text = "" Then
                Call Me.AutoComplete.Hide()
            End If

            e.Handled = True

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Sub TextboxReading_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextboxReading.LostFocus

        Me.LabelHelpTips.Text = ""

    End Sub

    Private Sub TextboxReading_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextboxReading.GotFocus

        Try
            Me.LabelHelpTips.Text = "[V] Down arrow button on your keyboard shows a list of the Object Types linked to the Roles of the selected Fact Type."
            Me.LabelHelpTips.Text &= vbCrLf & "[Enter] button adds the Fact Type Reading to the list of Fact Type Readings for the selected Fact Type."

            If Me.TextboxReading.Text = "Enter a Fact Type Reading for the selected Fact Type here." Then
                Me.TextboxReading.Text = ""
                Me.TextboxReading.ForeColor = Color.Black
            End If
            Me.Refresh()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,)
        End Try

    End Sub

    Private Sub TextboxReading_KeyDown1(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextboxReading.KeyDown

        'For [Enter] see KeyDown. Calls Me.processFactTypeReading()

        Try
            Dim lsMessage As String = ""

            If Me.zrFactTypeInstance Is Nothing Then
                lsMessage = "You must select a Fact Type within the Page before committing a Fact Type Reading."
                MsgBox(lsMessage)
                Exit Sub
            End If

            Select Case e.KeyCode
                Case Is = Keys.Down
                    e.SuppressKeyPress = True
                Case Is = Keys.Back
                    If zsIntellisenseBuffer.Length > 0 Then
                        zsIntellisenseBuffer = zsIntellisenseBuffer.Substring(0, zsIntellisenseBuffer.Length - 1)
                    End If
                Case Is = Keys.Space, Keys.Escape
                    Me.zsIntellisenseBuffer = ""
                    If Me.AutoComplete.ListBox.Items.Count = 1 Then
                        Me.AutoComplete.ListBox.SelectedIndex = 0
                    End If
            End Select

            If e.KeyCode = Keys.Enter Then
                e.SuppressKeyPress = True
                Call Me.processFactTypeReading()
            End If

            If Me.AutoComplete.ListBox.Items.Count = 0 Then
                Call Me.AutoComplete.Hide()
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Sub ShowAutoCompleteTool()

        Try
            Me.AutoComplete.Owner = Me
            Me.AutoComplete.zrCallingForm = Me
            Me.AutoComplete.Show()

            Dim lo_point As New Point(Me.TextboxReading.GetPositionFromCharIndex(Me.TextboxReading.SelectionStart))
            lo_point.X += Me.TextboxReading.Bounds.X
            lo_point.Y += Me.TextboxReading.Bounds.Y
            lo_point.Y += CInt(Me.TextboxReading.Font.GetHeight()) + 13
            Me.AutoComplete.Location = PointToScreen(lo_point)

            Me.AutoComplete.ListBox.ItemHeight = Me.AutoComplete.Font.GetHeight + 3

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Sub ProcessAutoComplete(ByVal e As System.Windows.Forms.KeyEventArgs)

        Dim lsExpectedToken As String = ""
        Dim liTokenType As FTR.TokenType
        Dim lsCurrentTokenType As Object

        Try
            '-------------------
            'Get the ParseTree
            '-------------------
            Me.zrTextHighlighter.Tree = Me.FTRParser.Parse(Me.TextboxReading.Text)

            If Me.AutoComplete.ListBox.Items.Count = 0 Then
                Me.AutoComplete.Hide()
            End If
            Me.AutoComplete.ListBox.Items.Clear()

            Dim lrLastToken As FTR.TokenType = Me.zrTextHighlighter.GetCurrentContext.Token.Type

            If (Me.zrTextHighlighter.Tree.Errors.Count > 0) Or (Me.zrTextHighlighter.Tree.Optionals.Count > 0) Or (lrLastToken = FTR.TokenType.EOF) Then

                If lrLastToken = FTR.TokenType.EOF Then
                    liTokenType = FTR.TokenType.EOF
                    GoTo ProcessToken
                ElseIf Me.zrTextHighlighter.Tree.Errors.Count > 0 Then
                    lsExpectedToken = Me.zrTextHighlighter.Tree.Errors(0).ExpectedToken
                Else
                    lsExpectedToken = Me.zrTextHighlighter.Tree.Optionals(0).ExpectedToken
                End If

                If lsExpectedToken <> "" Then
                    liTokenType = DirectCast([Enum].Parse(GetType(FTR.TokenType), lsExpectedToken), FTR.TokenType)
                    'MsgBox("Expecting: " & Me.zrScanner.Patterns(liTokenType).ToString)
                End If

                If Me.zrTextHighlighter.Tree.Optionals.Count > 0 Then
                    Call Me.PopulateEnterpriseAwareFromOptionals(Me.zrTextHighlighter.Tree.Optionals)
                End If

ProcessToken:
                Select Case liTokenType
                    Case Is = FTR.TokenType._NONE_
                        Me.AutoComplete.Visible = Me.CheckIfCanDisplayEnterpriseAwareBox
                    Case Is = FTR.TokenType.FOLLOWINGREADINGTEXT,
                              FTR.TokenType.FRONTREADINGTEXT
                    'Don't add anything 
                    Case Is = FTR.TokenType.SUBSCRIPT
                    'Don't add anything 
                    Case Is = FTR.TokenType.PREBOUNDREADINGTEXT,
                              FTR.TokenType.POSTBOUNDREADINGTEXT
                    'Don't add anything 
                    Case Is = FTR.TokenType.PREDICATEPART
                    'Don't add anything
                    Case Is = FTR.TokenType.EOF
                        Call Me.PopulateEnterpriseAwareWithObjectTypes()
                    Case Is = FTR.TokenType.PREDICATESPACE
                        Me.AutoComplete.Visible = Me.CheckIfCanDisplayEnterpriseAwareBox
                    Case Is = FTR.TokenType.MODELELEMENTNAME,
                              FTR.TokenType.UNARYPREDICATEPART
                        Me.AutoComplete.Enabled = True
                        Call Me.PopulateEnterpriseAwareWithObjectTypes()
                    Case Else
                        Me.AutoComplete.Enabled = True
                        Me.AddEnterpriseAwareItem(Me.FTRScanner.Patterns(liTokenType).ToString, liTokenType)
                End Select

                lsCurrentTokenType = Me.zrTextHighlighter.GetCurrentContext
                If lsCurrentTokenType IsNot Nothing And (Me.TextboxReading.Text.Length > 0) Then
                    lsCurrentTokenType = Me.zrTextHighlighter.GetCurrentContext.Token.Type.ToString

                    Select Case Me.zrTextHighlighter.GetCurrentContext.Token.Type
                        Case Is = FTR.TokenType.MODELELEMENTNAME
                            Me.AutoComplete.Enabled = True
                            Call Me.PopulateEnterpriseAwareWithObjectTypes()
                        Case Is = FTR.TokenType.PREDICATEPART,
                                  FTR.TokenType.PREDICATESPACE
                            Me.AutoComplete.Enabled = True
                            'Call Me.AddFactTypePredicatePartsToEnterpriseAware()
                    End Select
                End If

                '======================================================================================
                'Last effort.
                Call Me.PopulateEnterpriseAwareBasedOnOutstandingModelObjects()

                GoTo CreateSuggestions
            Else
                '=========================================================================================================================
                'Load all the MododelObjects of the FactType anyway, because Intellisense for FactTypeReadings doesn't always come up with
                '  MODELELEMENTNAME TokenType in the Optionals of the ParseTree.
                Me.AutoComplete.Enabled = True
                Call Me.PopulateEnterpriseAwareWithObjectTypes()

                Me.FTRProcessor.ProcessFTR(Trim(Me.TextboxReading.Text), Me.FTRParseTree)
                Me.FTRProcessor.FACTTYPEREADINGStatement.FRONTREADINGTEXT = New List(Of String)
                Me.FTRProcessor.FACTTYPEREADINGStatement.MODELELEMENT = New List(Of Object)
                Me.FTRProcessor.FACTTYPEREADINGStatement.PREDICATECLAUSE = New List(Of Object)
                Me.FTRProcessor.FACTTYPEREADINGStatement.UNARYPREDICATEPART = ""
                Me.FTRProcessor.FACTTYPEREADINGStatement.FOLLOWINGREADINGTEXT = ""
                Call Me.FTRProcessor.GetParseTreeTokensReflection(Me.FTRProcessor.FACTTYPEREADINGStatement, Me.FTRParseTree.Nodes(0))

                Dim lsModelElementName As String = ""
                Dim liModelElementCount As Integer = Me.FTRProcessor.FACTTYPEREADINGStatement.MODELELEMENT.Count

                Dim lrModelElementNode As FTR.ParseNode
                lrModelElementNode = Me.FTRProcessor.FACTTYPEREADINGStatement.MODELELEMENT(Me.FTRProcessor.FACTTYPEREADINGStatement.MODELELEMENT.Count - 1)
                Me.FTRProcessor.MODELELEMENTClause.PREBOUNDREADINGTEXT = ""
                Me.FTRProcessor.MODELELEMENTClause.POSTBOUNDREADINGTEXT = ""
                Me.FTRProcessor.MODELELEMENTClause.MODELELEMENTNAME = ""
                Call Me.FTRProcessor.GetParseTreeTokensReflection(Me.FTRProcessor.MODELELEMENTClause, lrModelElementNode)
                lsModelElementName = Me.FTRProcessor.MODELELEMENTClause.MODELELEMENTNAME
                If liModelElementCount = Me.zrFactTypeInstance.RoleGroup.Count _
                    And Me.zrFactTypeInstance.Model.ExistsModelElement(lsModelElementName) Then
                    Me.AutoComplete.Enabled = False
                End If
                '=========================================================================================================================

                lsCurrentTokenType = Me.zrTextHighlighter.GetCurrentContext
                If lsCurrentTokenType IsNot Nothing And (Me.TextboxReading.Text.Length > 0) Then
                    lsCurrentTokenType = Me.zrTextHighlighter.GetCurrentContext.Token.Type.ToString
                    Select Case Me.zrTextHighlighter.GetCurrentContext.Token.Type
                        Case Is = FTR.TokenType.PREDICATEPART,
                                  FTR.TokenType.PREDICATESPACE
                            Me.AutoComplete.Enabled = True
                            'Call Me.AddFactTypeReadingsToEnterpriseAware()
                        Case Else
                            'Me.AutoComplete.Enabled = False
                    End Select
                End If

            End If

CreateSuggestions:

#Region "Suggestions - AutoCreated"
            If Trim(Me.TextboxReading.Text).Length >= 0 Then

                Dim lsPredicatePart As String = ""
                Dim lrSentence As New Language.Sentence("", "")
                lrSentence.FrontText = ""
                lrSentence.FollowingText = ""

                Dim lsSentence As String = ""

                Dim larRole As New List(Of FBM.Role)
                For Each lrRole In Me.zrFactType.RoleGroup
                    larRole.Add(lrRole)
                Next

                Dim liInd = 1
                For Each lrRole As FBM.Role In larRole

                    lsSentence &= lrRole.JoinedORMObject.Id

                    Dim lrPredicatePart As New Language.PredicatePart

                    lrPredicatePart.PreboundText = ""
                    lrPredicatePart.PostboundText = ""
                    lrPredicatePart.ObjectName = lrRole.JoinedORMObject.Id
                    lrSentence.ModelElement.Add(lrPredicatePart.ObjectName)

                    lsPredicatePart = "has"
                    If lrRole.InternalUniquenessConstraint.Count = 0 And Me.zrFactType.Arity = 2 And liInd > 1 Then
                        lsPredicatePart = "is of"
                    End If

                    If liInd = Me.zrFactType.Arity Then
                        lsPredicatePart = ""
                    End If
                    lsSentence &= " " & lsPredicatePart & " "

                    lrPredicatePart.PredicatePartText = lsPredicatePart
                    lrPredicatePart.SequenceNr = liInd
                    lrSentence.PredicatePart.Add(lrPredicatePart)

                    liInd += 1
                Next

                lsSentence = Trim(lsSentence)

                lrSentence.Sentence = lsSentence
                lrSentence.OriginalSentence = lsSentence

                Dim lrFactTypeReading As FBM.FactTypeReading

                lrFactTypeReading = New FBM.FactTypeReading(Me.zrFactType, larRole, lrSentence)


                Dim lsReverseSentence As String = ""
                Dim lsReversePredicate As String = ""

                If Me.zrFactType.Arity = 2 Then
                    lsReverseSentence = lrSentence.ModelElement(1)
                    Select Case lsPredicatePart
                        Case Is = ""
                            lsReversePredicate = " is of "
                            lsReverseSentence &= lsReversePredicate
                        Case Else
                            lsReversePredicate = " has "
                            lsReverseSentence &= lsReversePredicate
                    End Select
                    lsReverseSentence &= lrSentence.ModelElement(0)

                    larRole.Reverse()
                    lrFactTypeReading.ReverseFactTypeReading = New FBM.FactTypeReading(Me.zrFactType, larRole, lrFactTypeReading.CreateFactTypeReadingSentence(larRole, New List(Of String) From {Trim(lsReversePredicate)}))
                End If

                Me.AddEnterpriseAwareItem(lsSentence & If(Me.zrFactType.Arity = 2, (", " & lsReverseSentence), ""), lrFactTypeReading)

            End If
#End Region

            If e.KeyCode = Keys.Down Then
                If Me.AutoComplete.ListBox.Items.Count > 0 Then
                    Me.AutoComplete.ListBox.Focus()
                    Me.AutoComplete.ListBox.SelectedIndex = 0
                    e.Handled = True
                End If
            End If

            If e.Control Then
                If e.KeyValue = Keys.J Then
                    'Call Me.AddFactTypeReadingsToEnterpriseAware()
                    Exit Sub
                End If
            End If

            If Me.AutoComplete.Enabled And Me.AutoComplete.ListBox.Items.Count > 0 Then
                Call Me.ShowAutoCompleteTool()
                'Me.AutoComplete.Owner = Me
                'Me.AutoComplete.Show()

                'Dim lo_point As New Point(Me.TextboxReading.GetPositionFromCharIndex(Me.TextboxReading.SelectionStart))
                'lo_point.X += Me.TextboxReading.Bounds.X
                'lo_point.Y += Me.TextboxReading.Bounds.Y
                'lo_point.Y += CInt(Me.TextboxReading.Font.GetHeight()) + 13
                'Me.AutoComplete.Location = PointToScreen(lo_point)
            End If

            If e.KeyCode <> Keys.Down Then
                Me.TextboxReading.Focus()
            End If

            If Me.AutoComplete.ListBox.Items.Count > 0 Then
                Me.LabelHelpTips.Text = ""
                Me.LabelHelpTips.Text.Append("Press the [V] down arrow key on your keyboard to goto the AutoComplete box.")
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Sub PopulateEnterpriseAwareBasedOnOutstandingModelObjects()

        Try
            If Me.zrFactTypeInstance Is Nothing Then Exit Sub

            Dim lrModelObject As FBM.ModelObject
            Dim larModelObject As New List(Of FBM.ModelObject)

            For Each lrRole In Me.zrFactTypeInstance.FactType.RoleGroup
                larModelObject.Add(lrRole.JoinedORMObject)
            Next

            'Use the follownig.
            'FactType.CountReferencesToModelObject

            For Each lrModelObject In larModelObject
                Try
                    If Me.TextboxReading.Text.CountSubstring(lrModelObject.Id) < Me.zrFactTypeInstance.FactType.CountReferencesToModelObject(lrModelObject) Then
                        Call Me.AddEnterpriseAwareItem(lrModelObject.Id, FTR.TokenType.MODELELEMENTNAME)
                    End If
                Catch ex As Exception
                    'Not a biggie
                End Try
            Next

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Sub PopulateEnterpriseAwareFromOptionals(ByVal aarParseErrors As FTR.ParseErrors)

        Dim lrParseError As FTR.ParseError
        Dim lsToken As String = ""
        Dim liTokenType As FTR.TokenType

        Try
            For Each lrParseError In aarParseErrors
                liTokenType = DirectCast([Enum].Parse(GetType(FTR.TokenType), lrParseError.ExpectedToken), FTR.TokenType)
                Select Case liTokenType
                    Case Is = FTR.TokenType.PREDICATEPART
                        'Dim lrModelElement As FBM.ModelObject
                        Dim lsModelElementName As String
                        lsModelElementName = Me.TextboxReading.Text.Trim.Split(" ").Last
                    ''lrModelElement = prApplication.WorkingModel.GetModelElementByName(lsModelElementName)
                    'If IsSomething(lrModelElement) Then
                    '    Call Me.AddPredicatePartsToEnterpriseAware(prBradfordApplication.Database.MetaDataManager.GetPredicatePartsForModelObject(lrModelElement))
                    'Else
                    '    Dim larCharBeginning() As Char = {"("}
                    '    Dim larCharEnd() As Char = {")"}
                    '    lsModelElementName = lsModelElementName.TrimStart(larCharBeginning).TrimEnd(larCharEnd)
                    '    'lrModelElement = prApplication .WorkingModel.GetModelElementByName(lsModelElementName)
                    '    If IsSomething(lrModelElement) Then
                    '        Call Me.AddPredicatePartsToEnterpriseAware(prBradfordApplication.Database.MetaDataManager.GetPredicatePartsForModelObject(lrModelElement))
                    '    End If
                    'End If
                    Case Is = FTR.TokenType.MODELELEMENTNAME
                        Call Me.PopulateEnterpriseAwareWithObjectTypes()
                    Case Is = FTR.TokenType.PREBOUNDREADINGTEXT,
                              FTR.TokenType.POSTBOUNDREADINGTEXT
                    'Don't add anything 
                    Case Is = FTR.TokenType.FOLLOWINGREADINGTEXT,
                              FTR.TokenType.FRONTREADINGTEXT
                    'Don't add anything 
                    Case Is = FTR.TokenType.UNARYPREDICATEPART
                    'Don't add anything 
                    Case Is = FTR.TokenType.SUBSCRIPT
                    'Don't add anything 
                    Case Is = FTR.TokenType.EOF
                        'Don't add anything 
                    Case Else
                        Call Me.AddEnterpriseAwareItem(Me.FTRScanner.Patterns(liTokenType).ToString, liTokenType)
                End Select
            Next

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Sub AddEnterpriseAwareItem(ByVal asEAItem As String, Optional ByVal aoTagObject As Object = Nothing)

        Try
            Dim lrListItem As tComboboxItem

            lrListItem = New tComboboxItem(asEAItem, asEAItem, aoTagObject)

            If (asEAItem <> "") And Not (Me.AutoComplete.ListBox.FindStringExact(asEAItem) >= 0) Then
                Me.AutoComplete.ListBox.Items.Add(lrListItem)
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Sub PopulateEnterpriseAwareWithObjectTypes()

        Dim lrValueType As FBM.ValueType
        Dim lrEntityType As FBM.EntityType
        Dim lrFactType As FBM.FactType

        '==================================
        'CodeSafe
        If Me.zrFactTypeInstance Is Nothing Then Exit Sub

        Try
            Dim lbStartsWith As Boolean = False
            lbStartsWith = "asdf".StartsWith(zsIntellisenseBuffer, True, System.Globalization.CultureInfo.CurrentUICulture)

            Dim larModelObject As New List(Of FBM.ModelObject)
            Dim lrRoleInstance As FBM.RoleInstance
            For Each lrRoleInstance In Me.zrFactTypeInstance.RoleGroup
                larModelObject.Add(lrRoleInstance.Role.JoinedORMObject)
            Next

            For Each lrValueType In larModelObject.FindAll(Function(x) x.ConceptType = pcenumConceptType.ValueType)
                If zsIntellisenseBuffer.Length > 0 Then
                    If lrValueType.Name.StartsWith(zsIntellisenseBuffer, True, System.Globalization.CultureInfo.CurrentUICulture) Then
                        Call Me.AddEnterpriseAwareItem(lrValueType.Name, FTR.TokenType.MODELELEMENTNAME)
                    End If
                Else
                    Call Me.AddEnterpriseAwareItem(lrValueType.Name, FTR.TokenType.MODELELEMENTNAME)
                End If
            Next

            For Each lrEntityType In larModelObject.FindAll(Function(x) x.ConceptType = pcenumConceptType.EntityType)
                If zsIntellisenseBuffer.Length > 0 Then
                    If lrEntityType.Name.StartsWith(zsIntellisenseBuffer, True, System.Globalization.CultureInfo.CurrentUICulture) Then
                        Call Me.AddEnterpriseAwareItem(lrEntityType.Name, FTR.TokenType.MODELELEMENTNAME)
                    End If
                Else
                    Call Me.AddEnterpriseAwareItem(lrEntityType.Name, FTR.TokenType.MODELELEMENTNAME)
                End If
            Next

            For Each lrFactType In larModelObject.FindAll(Function(x) x.ConceptType = pcenumConceptType.FactType)
                If zsIntellisenseBuffer.Length > 0 Then
                    If lrFactType.Name.StartsWith(zsIntellisenseBuffer, True, System.Globalization.CultureInfo.CurrentUICulture) Then
                        Call Me.AddEnterpriseAwareItem(lrFactType.Name, FTR.TokenType.MODELELEMENTNAME)
                    End If
                Else
                    Call Me.AddEnterpriseAwareItem(lrFactType.Name, FTR.TokenType.MODELELEMENTNAME)
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

    Private Function CheckIfCanDisplayEnterpriseAwareBox()

        If Me.AutoComplete Is Nothing Then
            Return False
        End If

        If Me.AutoComplete.ListBox.Items.Count > 0 Then
            Return True
        Else
            Return False
        End If

    End Function


    Private Sub frmToolboxORMReadingEditor_LostFocus(sender As Object, e As EventArgs) Handles Me.LostFocus

        Me.AutoComplete.Hide()

    End Sub

    Private Sub frmToolboxORMReadingEditor_Leave(sender As Object, e As EventArgs) Handles Me.Leave

        Me.AutoComplete.Hide()
        Me.TextboxReading.Text = ""

    End Sub

    Private Sub ToolStripMenuItemIsPreferred_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemIsPreferred.Click

        Try
            Me.ToolStripMenuItemIsPreferred.Checked = Not Me.ToolStripMenuItemIsPreferred.Checked

            Dim lrFactTypeReading As FBM.FactTypeReading

            lrFactTypeReading = Me.DataGrid_Readings.SelectedRows(0).Tag

            lrFactTypeReading.IsPreferred = Me.ToolStripMenuItemIsPreferred.Checked
            If lrFactTypeReading.IsPreferred Then
                Me.zrFactTypeInstance.FactType.SetIsPreferredFactTypeReadingForFactType(lrFactTypeReading)
            End If

            Me.zrPage.Model.MakeDirty(False, False)
            Call Me.zrPage.Form.EnableSaveButton()

            Call Me.PopulateDataGridWithFactTypeReadings()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Sub ContextMenuStripIsPreferred_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStripIsPreferred.Opening

        Dim lrFactTypeReading As FBM.FactTypeReading

        Try
            If Me.DataGrid_Readings.SelectedRows.Count = 0 Then
                Exit Sub
            End If

            lrFactTypeReading = Me.DataGrid_Readings.SelectedRows(0).Tag

            ToolStripMenuItemIsPreferred.Checked = lrFactTypeReading.IsPreferred

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Sub IsPreferredForPredicateToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemIsPreferredForPredicate.Click

        Try
            Dim lsMessage As String = ""

            lsMessage = "At least one Fact Type Reading must be 'Preferred' for its underlying Typed Predicate."
            lsMessage &= vbCrLf & vbCrLf
            lsMessage &= "Select another Fact Type Reading with the same Typed Predicate as the one selected, but where 'Is Preferred for Typed Predicate?' is False, and set that to True."

            Dim lrFactTypeReading As FBM.FactTypeReading

            lrFactTypeReading = Me.DataGrid_Readings.SelectedRows(0).Tag

            If lrFactTypeReading.IsPreferredForPredicate Then
                MsgBox(lsMessage)
                Exit Sub
            End If

            Me.ToolStripMenuItemIsPreferredForPredicate.Checked = Not Me.ToolStripMenuItemIsPreferredForPredicate.Checked



            lrFactTypeReading.IsPreferredForPredicate = Me.ToolStripMenuItemIsPreferredForPredicate.Checked

            If lrFactTypeReading.IsPreferredForPredicate Then
                Me.zrFactTypeInstance.FactType.SetIsPreferredFactTypeReadingForPredicate(lrFactTypeReading)
            End If

            Me.zrPage.Model.MakeDirty(False, False)
            Call Me.zrPage.Form.EnableSaveButton()

            Call Me.PopulateDataGridWithFactTypeReadings()


        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Sub ContextMenuStripIsPreferredForPredicate_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStripIsPreferredForPredicate.Opening

        Dim lrFactTypeReading As FBM.FactTypeReading

        Try
            If Me.DataGrid_Readings.SelectedRows.Count = 0 Then
                Exit Sub
            End If

            lrFactTypeReading = Me.DataGrid_Readings.SelectedRows(0).Tag

            ToolStripMenuItemIsPreferredForPredicate.Checked = lrFactTypeReading.IsPreferredForPredicate

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Sub zrFactType_FactTypeReadingAdded(ByRef arFactTypeReading As FBM.FactTypeReading) Handles _zrFactType.FactTypeReadingAdded

        Dim lbFound As Boolean = False

        Try
            For Each Row In Me.DataGrid_Readings.Rows
                If Row.Tag.Id = arFactTypeReading.Id Then
                    lbFound = True
                End If
            Next

            If Not lbFound Then
                Me.DataGrid_Readings.Rows.Add()
                Me.DataGrid_Readings.Rows(Me.DataGrid_Readings.Rows.Count - 1).Cells(0).Value = arFactTypeReading.GetReadingText
                Me.DataGrid_Readings.Rows(Me.DataGrid_Readings.Rows.Count - 1).Cells(1).Value = arFactTypeReading.IsPreferred
                Me.DataGrid_Readings.Rows(Me.DataGrid_Readings.Rows.Count - 1).Cells(2).Value = arFactTypeReading.IsPreferredForPredicate
                Me.DataGrid_Readings.Rows(Me.DataGrid_Readings.Rows.Count - 1).Tag = arFactTypeReading
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Sub zrFactType_FactTypeReadingRemoved(ByRef arFactTypeReading As FBM.FactTypeReading) Handles _zrFactType.FactTypeReadingRemoved

        Try
            Dim liRowToDelete As Integer = -1
            Dim liInd As Integer = 0

            For Each Row In Me.DataGrid_Readings.Rows
                If Row.Tag.Id = arFactTypeReading.Id Then
                    liRowToDelete = liInd
                    liInd += 1
                End If
            Next

            If liRowToDelete >= 0 Then
                Me.DataGrid_Readings.Rows.RemoveAt(liRowToDelete)
            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,)

        End Try

    End Sub

    Private Sub DeleteFactTypeReadingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteFactTypeReadingToolStripMenuItem.Click

        Try
            Dim lsMessage As String = ""
            Dim lrFactTypeReading As FBM.FactTypeReading

            If Me.DataGrid_Readings.SelectedRows.Count = 0 Then
                prApplication.ThrowErrorMessage("Select a Fact Type Reading in the grid.", pcenumErrorType.Warning,, False,)
                Exit Sub
            End If

            lrFactTypeReading = Me.DataGrid_Readings.SelectedRows(0).Tag

            lsMessage = "Delete Fact Type Reading:" & vbCrLf & vbCrLf
            lsMessage.AppendLine(lrFactTypeReading.GetReadingText)

            If MsgBox(lsMessage, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                Call Me.zrFactTypeInstance.FactType.RemoveFactTypeReading(lrFactTypeReading, True)
                Call lrFactTypeReading.RemoveFromModel()
                Call Me.PopulateDataGridWithFactTypeReadings()
            End If


        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Sub TextboxReading_MouseClick(sender As Object, e As MouseEventArgs) Handles TextboxReading.MouseClick

        Try

            Me.LabelHelpTips.Text = "Double Click over a Fact Type Reading to edit the Fact Type Reading."
            Me.LabelHelpTips.Text &= vbCrLf & "Select (Left Click leftmost column to highlight) a Fact Type Reading to Delete the selected Fact Type Reading."
            Me.LabelHelpTips.Text &= vbCrLf & "Select then press the [Delete] button to delete a selected Fact Type Reading"
            Me.Refresh()

            Dim rowIndex As Integer = DataGrid_Readings.HitTest(e.Location.X, e.Location.Y).RowIndex

            'If there was no DataGridViewRow under the cursor, return
            If (rowIndex >= 0) Then

                'Clear all other selections before making a New selection
                DataGrid_Readings.ClearSelection()

                'Select the found DataGridViewRow
                DataGrid_Readings.Rows(rowIndex).Selected = True
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Sub DataGrid_Readings_MouseClick(sender As Object, e As MouseEventArgs) Handles DataGrid_Readings.MouseClick

        Try
            Dim rowIndex As Integer = DataGrid_Readings.HitTest(e.Location.X, e.Location.Y).RowIndex

            'If there was no DataGridViewRow under the cursor, return
            If (rowIndex >= 0) Then

                'Clear all other selections before making a New selection
                DataGrid_Readings.ClearSelection()

                'Select the found DataGridViewRow
                DataGrid_Readings.Rows(rowIndex).Selected = True
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

    Private Sub DataGrid_Readings_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGrid_Readings.CellMouseClick

        Try
            Dim loPoint As Point = New Point(e.X, e.Y)

            Dim rowIndex As Integer = DataGrid_Readings.HitTest(loPoint.X, loPoint.Y).RowIndex

            'If there was no DataGridViewRow under the cursor, return
            If (rowIndex >= 0) Then

                'Clear all other selections before making a New selection
                DataGrid_Readings.ClearSelection()

                'Select the found DataGridViewRow
                DataGrid_Readings.Rows(rowIndex).Selected = True

            End If
            If e.Button = MouseButtons.Right Then
                Me.DataGrid_Readings.ContextMenuStrip = ContextMenuFactTypeReading
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
        End Try

    End Sub

End Class