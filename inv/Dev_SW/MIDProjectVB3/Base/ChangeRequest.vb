Imports Inventor

Public Class ChangeRequest

    Private oChangeProcessor As ChangeProcessor

    Public Sub New()
        oChangeProcessor = Nothing
    End Sub

    Public Overridable Sub OnExecute(ByVal document As Document, ByVal context As NameValueMap, ByVal succeeded As Boolean)
        '
    End Sub

    Public Sub Execute(ByVal application As Application, ByVal changeDefinition As Object, ByVal document As Inventor._Document)
        'Create instance of ChangeProcessor class
        oChangeProcessor = New ChangeProcessor

        'Set the parent to get the call back when change processor terminates
        oChangeProcessor.SetParentRequest(Me)

        'Connect change processor
        oChangeProcessor.Connect(application, changeDefinition, document)

    End Sub

    Public Sub Terminate()
        'Disconnect change processor
        If Not (oChangeProcessor Is Nothing) Then
            oChangeProcessor.Disconnect()
            oChangeProcessor = Nothing
        End If
    End Sub

End Class