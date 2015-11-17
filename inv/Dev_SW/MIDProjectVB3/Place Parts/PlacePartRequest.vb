
'############################################
' PlacePartRequest (Inherits ChangeRequest)
'############################################


Public Class PlacePartRequest
    Inherits ChangeRequest


    Private oAddIn As Inventor.Application
    Private oServer As MidAddInServer
    Private oPart As CircuitPart
    Private oFace As Face

    ' Constructor
    '**********************************************************************************************************
    Public Sub New(ByVal addIn As Inventor.Application, _
                   ByVal server As MidAddInServer, _
                   ByRef part As CircuitPart, _
                   ByRef face As Face)

        Me.oAddIn = addIn
        Me.oServer = server
        Me.oPart = part
        Me.oFace = face

    End Sub

    ' Execute Undo command
    '**********************************************************************************************************
    Public Overrides Sub OnExecute(document As Document, _
                                   context As NameValueMap, _
                                   ByRef succeeded As Boolean)


        oPart.UpdateOccurrence()
        oPart.UpdateNetLines()
        'oPart.CircuitBoard.UpdateNetLines(oPart)

        oPart.FaceId = "F3"
        oPart.IsPlaced = True
        oPart.IsValid = True

        oPart.Parent.WriteXml(oServer.Commands.WorkDirectory)
        oPart = Nothing

    End Sub


    Private Sub WriteXml()



    End Sub

End Class


