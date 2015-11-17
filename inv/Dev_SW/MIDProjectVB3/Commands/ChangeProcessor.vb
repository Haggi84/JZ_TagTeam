Imports Inventor

'##############################################
' ChangeProcessor Class
'##############################################

Public Class ChangeProcessor

    Private oChangeProcessor As Inventor.ChangeProcessor
    Private oParentRequest As ChangeRequest

    ' Constructor
    '***************************************************************************************************************
    Public Sub New()
        oChangeProcessor = Nothing
        oParentRequest = Nothing
    End Sub

    ' Create new changeProcessor and execute it
    '***************************************************************************************************************
    Public Sub ConnectToProcessor(ByVal addIn As Application, _
                                  ByVal changeDefinition As Object, _
                                  ByVal document As Inventor.Document)


        Dim oChangeManager As ChangeManager = addIn.ChangeManager
        Dim changeDefinitions As ChangeDefinitions = oChangeManager.Item("{e4a46d1d-77db-4aa1-b097-6321b6783c26}")

        'Create the change processor associated with the change definition
        oChangeProcessor = changeDefinitions.Item(changeDefinition).CreateChangeProcessor()

        '### bug: throws COMException if new occurrence is loaded from file
        'oChangeProcessor.Transact = False

        'Connect event handler in order to receive change processor events
        AddHandler oChangeProcessor.OnExecute, AddressOf Me.ChangeProcessorEvents_OnExecute
        AddHandler oChangeProcessor.OnTerminate, AddressOf Me.ChangeProcessorEvents_OnTerminate

        'Execute the change processor
        oChangeProcessor.Execute(document)

    End Sub

    ' Disconnect from changeProcessor and delete it
    '***************************************************************************************************************
    Public Sub DisconnectFromProcessor()
        'Disconnect change processor events sink
        If Not (oChangeProcessor Is Nothing) Then
            RemoveHandler oChangeProcessor.OnExecute, AddressOf Me.ChangeProcessorEvents_OnExecute
            RemoveHandler oChangeProcessor.OnTerminate, AddressOf Me.ChangeProcessorEvents_OnTerminate

            oChangeProcessor = Nothing
        End If
    End Sub

    ' Set the the request of the change processor
    '******************************************************************************************************************
    Public Sub SetParentRequest(ByVal parentRequest As ChangeRequest)
        oParentRequest = parentRequest
    End Sub

    ' EVENTS
    '******************************************************************************************************************
    Public Sub ChangeProcessorEvents_OnExecute(ByVal document As Inventor._Document, _
                                               ByVal context As NameValueMap, _
                                               ByRef succeeded As Boolean)
        oParentRequest.OnExecute(document, context, succeeded)
    End Sub

    Public Sub ChangeProcessorEvents_OnTerminate()
        oParentRequest.Terminate()
    End Sub

End Class
