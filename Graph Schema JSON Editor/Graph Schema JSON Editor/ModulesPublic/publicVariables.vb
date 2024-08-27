Imports FactEngineForServices
Imports System.ComponentModel

Module publicVariables

    Public prApplication As tApplication = New FactEngineForServices.tApplication()
    Public prFactEngine As New FactEngineForServices.FactEngine.Base()

    Public pbAutoCompleteSingleClickSelects As Boolean = True

    Public pbModelingUseThreadedXMLPageLoading As Boolean = False

    Public pbExportFBMExcludeMDAModelElements As Boolean = False

    Public pbModelLoadPagesUseThreading As Boolean = False
    Public poBackgroundWorkerModelLoader As BackgroundWorker = Nothing

End Module
