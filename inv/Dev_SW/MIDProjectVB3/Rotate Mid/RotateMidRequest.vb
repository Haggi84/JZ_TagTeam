Option Explicit On

Imports System.Windows.Forms

'###########################################
' Rotate Mid Request
'###########################################

Public Class RotateMidRequest
    Inherits ChangeRequest

    Private oAddIn As Inventor.Application
    Private oServer As MidAddInServer
    Private oRotateMidCommand As RotateMidCommand

    ' Constructor
    '**********************************************************************************************************
    Public Sub New(ByVal rotateMidCmd As RotateMidCommand, _
                   ByVal addIn As Inventor.Application, _
                   ByVal server As MidAddInServer)

        Me.oAddIn = addIn
        Me.oServer = server
        Me.oRotateMidCommand = rotateMidCmd

    End Sub

    ' Execute Undo command
    '**********************************************************************************************************
    Public Overrides Sub OnExecute(document As Document, _
                                   context As NameValueMap, _
                                   ByRef succeeded As Boolean)

        oServer.MidDataTypes.CircuitCarrier.Occurrence.Transformation = oRotateMidCommand.Transformation

    End Sub


End Class
