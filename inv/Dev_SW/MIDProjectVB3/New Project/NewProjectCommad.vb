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
' New Project Command
'############################################

Public Class NewProjectCommand
    Inherits Command

    ' Strings for folder path and document name
    Private strDirectoryPath As String
    Private strDocumentName As String

    Private oNewProjectDlg As NewProjectCmdDlg
    Private oServer As MidAddInServer
    Private formOffset As Double

    ' Constructor
    '***************************************************************************************************************
    Public Sub New(ByVal MidAddIn As Inventor.Application, _
                   ByVal server As MidAddInServer)

        ' call base class
        MyBase.New(MidAddIn)

        strDirectoryPath = Nothing
        oNewProjectDlg = Nothing

        Me.oServer = server

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


    ' Start/Stop the command
    '***************************************************************************************************************
    Public Overrides Sub StartCommand()

        ' for debugging purposes
        'Dim oAttributeMgr As AttributeManager = MidAddIn.ActiveDocument.AttributeManager
        'Dim AttribEnum As AttributeSetsEnumerator = oAttributeMgr.FindAttributeSets("KeepOut")
        'Dim AttributeEnum As AttributesEnumerator = oAttributeMgr.FindAttributes("CircuitCarrier", "MID")
        'If AttribEnum.Count > 0 Then
        '    MsgBox("funzt 1x")
        'End If
        'If AttributeEnum.Count > 0 Then
        '    MsgBox("funzt 2x")
        'End If

        MyBase.StartCommand()

        ' Selection cursor
        oInteractionEvents.SetCursor(CursorTypeEnum.kCursorTypeDefault)

        ' Create new form dialog
        oNewProjectDlg = New NewProjectCmdDlg(MidAddIn, Me)
        oNewProjectDlg.ShowInTaskbar() = True
        oNewProjectDlg.TopMost() = True
        oNewProjectDlg.StartPosition = FormStartPosition.Manual
        Dim oView As Inventor.View = MidAddIn.ActiveView()
        oNewProjectDlg.Location = New System.Drawing.Point(oView.Left + formOffset, oView.Top + formOffset) 'oView.Width / 2 - oNewProjectDlg.Size.Width / 2, oView.Top + oView.Height - oNewProjectDlg.Size.Height / 2)
        '+++place in the center of the view
        oNewProjectDlg.Show()

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
        If oNewProjectDlg IsNot Nothing Then
            oNewProjectDlg.Hide()
            oNewProjectDlg.Dispose()
            oNewProjectDlg = Nothing

            ' Disconnect events sink
            MyBase.StopCommand()

        End If

    End Sub

    ' Check for allowed characters in file name
    '***************************************************************************************************************
    Public Function IsInvalidFileName(fileName As String) As Boolean

        ' Dim oFileName = System.IO.Path.GetFileName(FileName)
        For Each character In System.IO.Path.GetInvalidFileNameChars()
            If fileName.Contains(character) Then
                Return True
            End If
        Next
        Return False


    End Function

    ' Check for allowed characters in folder name
    '***************************************************************************************************************
    Public Function IsInvalidDirName(dirName As String) As Boolean

        'Dim oDirectoryName = System.IO.Path.GetDirectoryName(DirName)
        For Each character In System.IO.Path.GetInvalidPathChars()
            If dirName.Contains(character) Then
                Return True
            End If
        Next
        Return False
    End Function

    ' Obtain the path for the folder of the work directory
    '***************************************************************************************************************
    Public Sub SetData(ByVal directoryPath As String, _
                       ByVal documentName As String)

        ' Check for valid document name
        If IsInvalidFileName(documentName) Then
            MessageBox.Show("Please insert a valid filename", "MIDProject", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            Exit Sub
        End If

        ' Check for valid directory path
        If Directory.Exists(directoryPath) Then
            MessageBox.Show("The selected directory exists already", "MIDProject", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            Exit Sub
        End If

        ' Check for valid directory name
        If IsInvalidDirName(directoryPath) Then
            MessageBox.Show("Please insert a valid directory name", "MIDProject", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            Exit Sub
        End If

        ' Create new directory (catch exceptions: http://msdn.microsoft.com/de-de/library/54a0at6s%28v=vs.110%29.aspx)
        Try
            Dim oDirectory As DirectoryInfo = Directory.CreateDirectory(directoryPath)
        Catch ex As PathTooLongException
            MessageBox.Show("The selected path exceeds the allowed length", "Mid Project", MessageBoxButtons.OK)
            Exit Sub
        Catch ex As UnauthorizedAccessException
            MessageBox.Show("You have no permission to write to the selected location", "Mid Project", MessageBoxButtons.OK)
            Exit Sub
        Catch ec As Exception
            MessageBox.Show("An error occurred", "Mid Project", MessageBoxButtons.OK)
            Exit Sub
        Finally
            strDirectoryPath = directoryPath
            MessageBox.Show("New work directory has been successfully created", "MIDProject", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
        End Try

        ' Set document Name 
        strDocumentName = documentName

        ' Save work directory to the commandcollection
        oServer.Commands.WorkDirectory = strDirectoryPath

        ' Execute command
        ExecuteCommand()

    End Sub

    ' Execute command
    '***************************************************************************************************************
    Public Overrides Sub ExecuteCommand()

        MyBase.ExecuteCommand()
        StopCommand()

        Dim oNewProjectRequest As New NewProjectRequest(MidAddIn, oServer, Me)
        MyBase.ExecuteChangeRequest(oNewProjectRequest, "MidAddIn:NewProjectRequest", MidAddIn.ActiveDocument)

    End Sub

    ' Return name of the new document
    Public ReadOnly Property DocumentName As String
        Get
            Return strDocumentName
        End Get
    End Property

End Class