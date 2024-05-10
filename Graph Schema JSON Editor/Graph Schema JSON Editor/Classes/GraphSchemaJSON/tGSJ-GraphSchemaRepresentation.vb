Imports FactEngineForServices
Imports Newtonsoft.Json
Imports System.Reflection

Namespace GSJ

    ' GraphSchemaRepresentation nested within the RootObject.
    Public Class GraphSchemaRepresentation
        <JsonProperty("version")>
        Public Property Version As String = "1.0.1"

        <JsonProperty("graphSchema")>
        Public Property graphSchema As New GraphSchema


        Public Sub FromFBM(ByRef arFBMModel As FBM.Model)

            Try

#Region "RDS Tables"

                For Each lrRDSTable In arFBMModel.RDS.Table.FindAll(Function(x) Not x.isPGSRelation)

                    Dim lrGSJNodeLabel As New GSJ.NodeLabel(lrRDSTable.Name)
                    Me.graphSchema.nodeLabels.Add(lrGSJNodeLabel)

                    For Each lrRDSColumn In lrRDSTable.Column

                        Dim lrGSJProperty As New GSJ.Property(lrRDSColumn.Name, Not lrRDSColumn.IsMandatory, New GSJ.DataType(lrRDSColumn.DBDataType))

                    Next

                Next
#End Region

#Region "Fact Types"

#End Region

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)
            End Try

        End Sub

        Public Function ToFBMModel() As FBM.Model

            Dim lrFBMModel As New FBM.Model

            Try

#Region "Entity Types"

#End Region

#Region "Fact Types"

#End Region


                Return lrFBMModel

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,)

                Return Nothing
            End Try

        End Function


    End Class

End Namespace