Option Strict Off
Option Explicit On

Imports Inventor
Imports System.Windows.Forms
Imports System.Xml

'############################################
' Read Keep Out Command
'############################################

Public Class ReadKeepOutCommand
    Inherits Command


    Private oReadKeepOutDlg As ReadKeepOutsCmdDlg
    Private oServer As MidAddInServer

    Private offset As Double = 20

    Private _filePath As String

    ' Constructor
    '***************************************************************************************************************
    Public Sub New(MidAddIn As Inventor.Application, oServer As MidAddInServer)
        ' call base class
        MyBase.New(MidAddIn)

        Me.oServer = oServer '###later in base class

    End Sub

    ' Initialize button execution
    '***************************************************************************************************************
    Public Overrides Sub ButtonDefinition_OnExecute()
        ' Only execute if assemlby environment
        Dim currentEnvironment As Inventor.Environment = MidAddIn.UserInterfaceManager.ActiveEnvironment

        If LCase(currentEnvironment.InternalName) <> LCase("OPTAVER") Then
            MessageBox.Show("This command works only for assembly environment", "MIDProject", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' Check if there is a circuitcarrier (new version)
        Dim oAttribMgr As AttributeManager = MidAddIn.ActiveDocument.AttributeManager
        Dim AttribSetsEnum As AttributeSetsEnumerator = oAttribMgr.FindAttributeSets("CircuitCarrier")
        If AttribSetsEnum.Count = 0 Then
            MessageBox.Show("Could not find a mid circuit carrier", "MIDProject", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' Stop command if there is an other instance running
        If bCommandIsRunning Then
            StopCommand()
        End If

        'Start new command
        StartCommand()
    End Sub

    ' Start/Stop the command
    '***************************************************************************************************************
    Public Overrides Sub StartCommand()

        ' Start Interaction /Interaction.start
        MyBase.StartCommand()

        ' Selection cursor
        oInteractionEvents.SetCursor(CursorTypeEnum.kCursorBuiltInSelectArrow)

        ' Subscribe to selection event
        MyBase.SubscribeToEvent(Interaction.InteractionTypeEnum.kSelection)

        ' Create new form dialog
        oReadKeepOutDlg = New ReadKeepOutsCmdDlg(MidAddIn, Me)

        If oReadKeepOutDlg IsNot Nothing Then
            oReadKeepOutDlg.TopMost() = True
            oReadKeepOutDlg.ShowInTaskbar() = True
            oReadKeepOutDlg.StartPosition = FormStartPosition.Manual
            Dim oView As Inventor.View = MidAddIn.ActiveView()
            oReadKeepOutDlg.Location = New System.Drawing.Point(oView.Left + 20, oView.Top + 20)
            oReadKeepOutDlg.Show()
        End If

        ' Enable command specific functions
        DisableInteraction()

    End Sub

    ' Stop command
    '***************************************************************************************************************
    Public Overrides Sub StopCommand()

        ' Remove the command dialog
        oReadKeepOutDlg.Hide()
        oReadKeepOutDlg.Dispose()
        oReadKeepOutDlg = Nothing

        ' Disconnect events sink
        MyBase.StopCommand()

    End Sub

    ' Enable interaction (not used here)
    '***************************************************************************************************************
    Public Overrides Sub EnableInteraction()

        '' Resubscribe all event handlers for the subscribed events
        'MyBase.EnableInteraction()

        'oInteractionEvents.SelectionActive = False
        'oInteractionEvents.InteractionDisabled = False

    End Sub

    'Disable Interaction
    '***************************************************************************************************************
    Public Overrides Sub DisableInteraction()

        'Call base command button's DisableInteraction
        MyBase.DisableInteraction()

        'Set the default cursor to notify that selection is not active
        oInteractionEvents.SetCursor(CursorTypeEnum.kCursorBuiltInArrow)

        'Clean up status text
        oInteractionEvents.StatusBarText = String.Empty

    End Sub

    ' Execute command
    '***************************************************************************************************************
    Public Overrides Sub ExecuteCommand()

        MyBase.ExecuteCommand()
        StopCommand()

        ' Create new change request
        Dim oKeepOutRequest As New ReadKeepOutsRequest(Me, MidAddIn, oServer)
        MyBase.ExecuteChangeRequest(oKeepOutRequest, "MidAddIn:ReadKeepOutRequest", MidAddIn.ActiveDocument)

    End Sub

    ' File path property
    '***************************************************************************************************************
    Public ReadOnly Property FilePath As String
        Get
            Return _filePath
        End Get
    End Property

    ' Set file path
    '***************************************************************************************************************
    Public Sub SetPath(filePath)

        ' Check whether the file exists
        If Not System.IO.File.Exists(FilePath) Then
            MessageBox.Show("File path not valid", "MID Project", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        Else
            ' Notification for the user
            Dim result As DialogResult = MessageBox.Show("Already selected KeepOuts will be overridden", "MID Project", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
            If result = Windows.Forms.DialogResult.OK Then

                ' Set path
                _filePath = FilePath
                ' Execute command
                ExecuteCommand()

            End If
        End If

    End Sub

End Class
