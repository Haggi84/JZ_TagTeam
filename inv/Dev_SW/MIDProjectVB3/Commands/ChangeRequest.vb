Imports Inventor

'##############################################
' ChangeProcessor Class
'##############################################

Public MustInherit Class ChangeRequest

    Private oChangeProcessor As MIDAddin.ChangeProcessor

    ' Constructor 
    '*************************************************************************************************************************
    Public Sub New()
        oChangeProcessor = Nothing
    End Sub

    Public Overridable Sub OnExecute(ByVal document As Document, _
                                     ByVal context As NameValueMap, _
                                     ByRef succeeded As Boolean)
        ' implemented in derived class
    End Sub

    ' Execute 
    '*************************************************************************************************************************
    Public Sub Execute(ByVal addIn As Application, _
                       ByVal changeDefinition As Object, _
                       ByVal document As Inventor.Document)

        oChangeProcessor = New ChangeProcessor

        'Set the parent to get the call back when change processor terminates
        oChangeProcessor.SetParentRequest(Me)

        'Connect change processor
        oChangeProcessor.ConnectToProcessor(addIn, changeDefinition, document)

    End Sub

    ' Change processor is done with the current execution
    '*************************************************************************************************************************
    Public Sub Terminate()
        'Disconnect change processor
        If Not (oChangeProcessor Is Nothing) Then
            oChangeProcessor.DisconnectFromProcessor()
            oChangeProcessor = Nothing
        End If
    End Sub

End Class