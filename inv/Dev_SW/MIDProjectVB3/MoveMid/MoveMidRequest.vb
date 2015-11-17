Option Explicit On

Imports System.Windows.Forms

'###########################################
' Circuit Carrier Class
'###########################################

Public Class MoveMidRequest
    Inherits ChangeRequest

    Private oAddIn As Inventor.Application
    Private oServer As MidAddInServer

    ' Constructor
    '**********************************************************************************************************
    Public Sub New(ByVal addIn As Inventor.Application, _
                   ByVal server As MidAddInServer)

        Me.oAddIn = addIn
        Me.oServer = server

    End Sub

    ' Execute Undo command
    '**********************************************************************************************************
    Public Overrides Sub OnExecute(document As Document, _
                                   context As NameValueMap, _
                                   ByRef succeeded As Boolean)

        oServer.MidDataTypes.CircuitCarrier.Occurrence.Transformation = oServer.CommandCollection.MoveMidCommand.Matrix

    End Sub


End Class
