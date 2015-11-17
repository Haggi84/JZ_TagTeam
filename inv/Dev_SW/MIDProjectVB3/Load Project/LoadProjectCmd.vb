Option Strict Off
Option Explicit On

Imports Inventor
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports System.Xml
Imports System.Drawing
Imports System.IO
Imports Microsoft.VisualBasic

'############################################
' Load Project Command
'############################################

Public Class LoadProjectCommand
    Inherits Command

    Private oLoadProjectCmdDlg As LoadProjectCmdDlg
    Private oBrowser As Browser
    Private oServer As MidAddInServer

    Private strDocumentPath As String

    Private formOffset As Double

    ' Constructor
    '***************************************************************************************************************
    Public Sub New(ByVal MidAddIn As Inventor.Application, _
                   ByVal server As MidAddInServer)

        ' call base class
        MyBase.New(MidAddIn)
        Me.oServer = server

        oLoadProjectCmdDlg = Nothing
        strDocumentPath = Nothing

        formOffset = 60

    End Sub

    ' OnExecute button
    '***************************************************************************************************************
    Public Overrides Sub ButtonDefinition_OnExecute()
        ' Only execute if assemlby environment
        Dim currentEnvironment As Inventor.Environment = MidAddIn.UserInterfaceManager.ActiveEnvironment

        If LCase(currentEnvironment.InternalName) <> LCase("OPTAVER") Then
            MsgBox(currentEnvironment.InternalName)

            MessageBox.Show("This command works only for assembly environment", "MIDProject", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' stop other instance of this command
        If bCommandIsRunning Then
            StopCommand()
        End If

        'Start new command
        StartCommand()
    End Sub


    ' Start the command
    '***************************************************************************************************************
    Public Overrides Sub StartCommand()

        MyBase.StartCommand()

        ' Selection cursor
        oInteractionEvents.SetCursor(CursorTypeEnum.kCursorTypeDefault)

        ' Create new form dialog
        oLoadProjectCmdDlg = New LoadProjectCmdDlg(MidAddIn, Me)
        oLoadProjectCmdDlg.ShowInTaskbar() = True
        oLoadProjectCmdDlg.TopMost() = True

        ' Place dialog form in the middle of the view
        oLoadProjectCmdDlg.StartPosition = FormStartPosition.Manual
        Dim oView As Inventor.View = MidAddIn.ActiveView()
        oLoadProjectCmdDlg.Location = New System.Drawing.Point(oView.Left + formOffset, oView.Top + formOffset) 'oView.Width / 2 - oNewProjectDlg.Size.Width / 2, oView.Top + oView.Height - oNewProjectDlg.Size.Height / 2)

        oLoadProjectCmdDlg.Show()

        ' Disable interaction (no selection)
        DisableInteraction()

    End Sub

    'Enable Interaction (not used here)
    '***************************************************************************************************************
    Public Overrides Sub EnableInteraction()

        ' Resubscribe all event handlers for the subscribed events
        'MyBase.EnableInteraction()

        '' Disable interaction 
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

    'Stop Command
    '***************************************************************************************************************
    Public Overrides Sub StopCommand()

        ' Destroy the command dialog
        If oLoadProjectCmdDlg IsNot Nothing Then
            oLoadProjectCmdDlg.Hide()
            oLoadProjectCmdDlg.Dispose()
            oLoadProjectCmdDlg = Nothing

            ' Disconnect events sink
            MyBase.StopCommand()

        End If

    End Sub

    ' Obtain the path for the folder of the work directory
    '***************************************************************************************************************
    Public Sub SetPath(ByVal documentPath As String)

        ' Check for valid document path
        If Not My.Computer.FileSystem.FileExists(documentPath) Then
            MessageBox.Show("The selected directory exists already", "MIDProject", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            Exit Sub
        End If

        ' Set document Name 
        strDocumentPath = documentPath

        ExecuteCommand()

    End Sub

    ' Execute Command
    '***************************************************************************************************************
    Public Overrides Sub ExecuteCommand()

        MyBase.ExecuteCommand()
        StopCommand()

        Dim oLoadProjectRequest As New LoadProjectRequest(Me, MidAddIn, oServer)
        MyBase.ExecuteChangeRequest(oLoadProjectRequest, "MidAddIn:LoadProjectRequest", MidAddIn.ActiveDocument)

    End Sub

    ' Return name of the new document
    Public ReadOnly Property DocumentPath As String
        Get
            Return strDocumentPath
        End Get
    End Property


End Class
