Option Explicit On

Imports Inventor

'##############################################
' ReadNetListRequest Class
'##############################################

Public Class ReadNetlistRequest
    Inherits ChangeRequest

    Private oAddIn As Application
    Private oServer As MidAddInServer

    ' Constructor
    '***************************************************************************************************************
    Public Sub New(ByVal oAddIn As Inventor.Application, _
                   ByVal oServer As MidAddInServer)

        Me.oAddIn = oAddIn
        Me.oServer = oServer
       
    End Sub

    ' OnExecute
    '***************************************************************************************************************
    Public Overrides Sub OnExecute(ByVal document As Document, _
                                   ByVal context As NameValueMap, _
                                   ByRef succeeded As Boolean)


        ' Create new browser node "Circuit Parts" below top node
        'oServer.Browser.CreateNode(, , "Circuit Parts")

        ' Create part occurrences
        Dim oBoard As CircuitBoard = oServer.MidDataTypes.CircuitBoard

        oBoard.AddPartsNode()

        For Each oPart As CircuitPart In oBoard.PartList
            oPart.CreateIRep()
        Next

        ' Create net line
        For Each oNet As CircuitNet In oBoard.NetList
            oNet.CreateIRep()
        Next

        'oBoard.CreatePartOccurrences()
        'oBoard.CreateNetLines()

    End Sub

End Class
