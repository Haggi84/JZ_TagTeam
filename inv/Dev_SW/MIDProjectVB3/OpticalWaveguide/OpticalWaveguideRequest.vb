Option Explicit On

'############################################
' Load Project Command
'############################################

Public Class OpticalWaveguideRequest
    Inherits ChangeRequest

    Private oAddIn As Inventor.Application
    Private oServer As MidAddInServer

    Private oOpticalWaveguideCmd As OpticalWaveguideCommand


    ' Constructor
    '***************************************************************************************************************
    Public Sub New(ByVal OpticalWaveguideCmd As OpticalWaveguideCommand, _
                   ByVal addIn As Inventor.Application, _
                   ByVal server As MidAddInServer)

        Me.oAddIn = addIn
        Me.oServer = server
        Me.oOpticalWaveguideCmd = OpticalWaveguideCmd

    End Sub

    ' OnExecute
    '***************************************************************************************************************
    Public Overrides Sub OnExecute(document As Document, _
                                   context As NameValueMap, _
                                   ByRef succeeded As Boolean)

        ' Create new assembly document
        'Dim  As AssemblyDocument
        'oAsmDoc = oAddIn.Documents.Open(oLoadProjectCmd.DocumentPath, True)
        ' --> OnNewDocument (MidApplicationEvents) is called
        ' --> OnActivateDocument (MidApplicationEvents) is called here to create new browser

        'oAsmDoc.EnvironmentManager.SetCurrentEnvironment(oAddIn.UserInterfaceManager.Environments.Item("OPTAVER"))
        ' --> OnChangeEnvironment (MidInterfceEvents) is called here to activate mid browser tree and disable commands

        'oServer.Browser.ResetBrowser(oAsmDoc)

        '' Create basic browser nodes
        'oServer.Commands.CircuitCarrierNode = oServer.Browser.CreateNode("Circuit Carrier")
        'oServer.Commands.CircuitBoardNode = oServer.Browser.CreateNode("Circuit Board")

        '' Force Inventor to create Attribute Cache
        'Dim oAttributeMgr As AttributeManager = oAddIn.ActiveDocument.AttributeManager
        'Dim oAttributeEnum As AttributesEnumerator = oAttributeMgr.FindAttributes()

        '' Load items
        'LoadCircuitCarrier()
        'LoadKeepOuts()
        'LoadCircuitBoard()

    End Sub

    ' Load circuit carrier
    '**********************************************************************************************************
    Private Sub LoadCircuitCarrier()

        ' Find occurrence tagged with "CircuitCarrier"
        'Dim oAsmDoc As AssemblyDocument = oAddIn.ActiveDocument
        'Dim oAttributeMgr As AttributeManager = oAddIn.ActiveDocument.AttributeManager
        'Dim oObjColl As ObjectCollection = oAttributeMgr.FindObjects("CircuitCarrier")

        '' Add reference to circuitcarrier
        'If oObjColl.Count > 0 Then
        '    If TypeOf oObjColl.Item(1) Is ComponentOccurrence Then
        '        Dim oOcc As ComponentOccurrence = oObjColl.Item(1)
        '        oServer.MidDataTypes.AddCircuitCarrier(oOcc)
        '    End If
        'End If

    End Sub

    ' Load KeepOuts
    '**********************************************************************************************************
    Private Sub LoadKeepOuts()

        ' Find faces tagged with "KeepOut"
        'Dim oAsmDoc As AssemblyDocument = oAddIn.ActiveDocument
        'Dim oAttributeMgr As AttributeManager = oAddIn.ActiveDocument.AttributeManager
        'Dim oAttributeEnum As AttributesEnumerator = oAttributeMgr.FindAttributes("KeepOuts")

        '' Add reference to keepOuts
        'If oAttributeEnum.Count > 0 Then
        '    For i As Integer = 1 To oAttributeEnum.Count
        '        Dim oFace As FaceProxy = oServer.MidDataTypes.CircuitCarrier.GetFaceById(oAttributeEnum.Item(i).Value)
        '        oServer.MidDataTypes.CircuitCarrier.KeepOuts.Add(oFace)
        '    Next
        'End If


    End Sub

    Private Sub LoadCircuitBoard()

    End Sub

End Class
