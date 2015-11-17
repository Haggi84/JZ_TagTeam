Option Strict Off
Option Explicit On

'############################################
' New Project Command
'############################################

Public Class NewProjectRequest
    Inherits ChangeRequest

    Private oAddIn As Application
    Private oServer As MidAddInServer

    Private oNewProjectCmd As NewProjectCommand

    ' Constructor
    '***************************************************************************************************************
    Public Sub New(ByVal addIn As Inventor.Application, _
                   ByVal server As MidAddInServer, _
                   ByVal newProjectCmd As NewProjectCommand)

        Me.oAddIn = addIn
        Me.oServer = server
        Me.oNewProjectCmd = newProjectCmd

    End Sub

    ' OnExecute
    '***************************************************************************************************************
    Public Overrides Sub OnExecute(document As Document, _
                                   context As NameValueMap, _
                                   ByRef succeeded As Boolean)

        'Create a new document and close the current document
        Dim oOldDoc As Document = oAddIn.ActiveDocument


        ' Create new assembly document
        Dim oAsmDoc As AssemblyDocument = oAddIn.Documents.Add(DocumentTypeEnum.kAssemblyDocumentObject, , True)
        oAsmDoc.DisplayName = oNewProjectCmd.DocumentName

        ' --> OnNewDocument (MidApplicationEvents) is called
        ' --> OnActivateDocument (MidApplicationEvents) is called here to create new browser

        oAsmDoc.EnvironmentManager.SetCurrentEnvironment(oAddIn.UserInterfaceManager.Environments.Item("OPTAVER"))
        ' --> OnChangeEnvironment (MidInterfceEvents) is called here to activate mid browser tree and disable commands

        ' Save work directory in the documents attributes
        Dim oString As String = oServer.Commands.WorkDirectory
        MsgBox(oServer.Commands.WorkDirectory)

        oAsmDoc.AttributeSets.Add("mydoc")
        ' Close old document
        ' oAddIn.SilentOperation = True

        'oAddIn.SilentOperation = False
        'oOldDoc.Close()
        ' Create basic browser nodes
        oServer.Commands.CircuitCarrierNode = oServer.Browser.CreateNode("Circuit Carrier")
        oServer.Commands.CircuitBoardNode = oServer.Browser.CreateNode("Circuit Board")

        ' Enable mid place button
        Dim oControls As ControlDefinitions = oAddIn.CommandManager.ControlDefinitions
        Dim oControl As ControlDefinition
        oControl = oControls.Item("placeMidIntern")
        oControl.Enabled = True
        oControl = oControls.Item("placeMidIntern")
        oControl.Enabled = True
        oControl = oControls.Item("placeMidIntern")
        oControl.Enabled = True
        oControl = oControls.Item("placeMidIntern")
        oControl.Enabled = True
        oControl = oControls.Item("placeMidIntern")
        oControl.Enabled = True


    End Sub

End Class

