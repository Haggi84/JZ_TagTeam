Imports Inventor

Friend Class ChangeProcessor

    Private oChangeProcessor As Inventor.ChangeProcessor

    'Parent ChangeRequest object
    Private oParentRequest As ChangeRequest

    ' Constructor
    '***************************************************************************************************************
    Public Sub New()
        oChangeProcessor = Nothing
        oParentRequest = Nothing
    End Sub

    Public Sub Connect(ByVal oAddIn As Application, ByVal oCDef As Object, ByVal oDoc As Inventor._Document)

        'Get the change manager object
        Dim oChangeManager As ChangeManager = oAddIn.ChangeManager

        'Get the change definition collection for this AddIn
        Dim changeDefinitions As ChangeDefinitions = oChangeManager.Item("{e4a46d1d-77db-4aa1-b097-6321b6783c26}")

        'Create the change processor associated with the change definition
        oChangeProcessor = changeDefinitions(oCDef).CreateChangeProcessor()

        'Connect event handler in order to receive change processor events
        AddHandler oChangeProcessor.OnExecute, AddressOf Me.ChangeProcessorEvents_OnExecute
        AddHandler oChangeProcessor.OnTerminate, AddressOf Me.ChangeProcessorEvents_OnTerminate

        'Execute the change processor
        oChangeProcessor.Execute(oDoc)

    End Sub

    Public Sub Disconnect()
        'Disconnect change processor events sink
        If Not (oChangeProcessor Is Nothing) Then
            RemoveHandler oChangeProcessor.OnExecute, AddressOf Me.ChangeProcessorEvents_OnExecute
            RemoveHandler oChangeProcessor.OnTerminate, AddressOf Me.ChangeProcessorEvents_OnTerminate

            oChangeProcessor = Nothing
        End If
    End Sub

    Public Sub SetParentRequest(ByVal parentRequest As ChangeRequest)
        'Store the parent request object
        oParentRequest = parentRequest
    End Sub

    ' does nothing
    Public Sub ChangeProcessorEvents_OnExecute(ByVal document As Inventor._Document, ByVal context As NameValueMap, ByRef succeeded As Boolean)
        oParentRequest.OnExecute(document, context, succeeded)
    End Sub

    Public Sub ChangeProcessorEvents_OnTerminate()
        oParentRequest.Terminate()
    End Sub
End Class
