Option Explicit On

Imports System.Collections.Generic

'############################################
' KEEP OUT REQUEST
'############################################

Public Class KeepOutRequest
    Inherits ChangeRequest


    Private oAddIn As Application
    Private oServer As MidAddInServer
    Private oKeepOutFaces As List(Of Face)


    Public Sub New(ByVal keepOutFaces As System.Collections.Generic.List(Of Face), _
                   ByVal addIn As Inventor.Application, _
                   ByVal server As MidAddInServer)

        Me.oKeepOutFaces = keepOutFaces
        Me.oAddIn = addIn
        Me.oServer = server

    End Sub

    ' On Execute
    '**********************************************************************************************************************
    Public Overrides Sub OnExecute(ByVal document As Document, _
                                   ByVal context As NameValueMap, _
                                   ByRef succeeded As Boolean)

        ' Retrieve the KeepOuts object
        Dim oKeepOuts As KeepOuts = oServer.MidDataTypes.CircuitCarrier.KeepOuts

        ' Clear old KeepOuts
        oServer.MidDataTypes.CircuitCarrier.KeepOuts.Clear()

        ' Add new Keep-Out-faces
        For Each oFace As FaceProxy In oKeepOutFaces
            oKeepOuts.Add(oFace)
        Next

        ' Update view to make colored faces visible (faster than doing this for each face individually)
        oAddIn.ActiveView.Update()

        ' write XML
        oKeepOuts.WriteXml(oServer.Commands.WorkDirectory)

    End Sub

End Class
