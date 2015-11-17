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

    'Private WithEvents oInteractionEvents As InteractionEvents
    'Private WithEvents oSelectEvents As SelectEvents
  
    'Private oEntityList As Inventor.ObjectsEnumerator

    Private Shared strProjectFolderPath As String
    Private oNewProjectDlg As NewProjectCmdDlg
    Private oBrowser As MidBrowser
    Private oServer As MidAddInServer

    ' constructor
    '***************************************************************************************************************
    Public Sub New(ByVal MidAddIn As Inventor.Application, _
                   ByVal oServer As MidAddInServer)

        ' call base class
        MyBase.New(MidAddIn)

        strProjectFolderPath = Nothing
        oNewProjectDlg = Nothing

        Me.oServer = oServer
        'oBrowser = New Browser(MidAddIn, oServer.ClientId)


    End Sub

    ' Initialize button execution
    '***************************************************************************************************************
    Protected Overrides Sub oButtonDefinition_OnExecute()
        ' Only execute if assemlby environment
        'Dim currEnvironment As Inventor.Environment = MidAddIn.UserInterfaceManager.ActiveEnvironment

        'If LCase$(currEnvironment.InternalName) <> LCase$("AMxAssemblyEnvironment") Then
        '    MessageBox.Show("This command applies only to assembly environment", _
        '                    "MIDProject", _
        '                    MessageBoxButtons.OK, _
        '                    MessageBoxIcon.Error)
        '    Exit Sub
        'End If

        If bCommandIsRunning Then
            StopCommand()
        End If

        'Start new command
        StartCommand()
    End Sub


    ' Start/Stop the command
    '***************************************************************************************************************
    Public Overrides Sub StartCommand()

        ' Create new form dialog
        oNewProjectDlg = New NewProjectCmdDlg(MidAddIn, Me)
        oNewProjectDlg.ShowInTaskbar() = True
        oNewProjectDlg.TopMost() = True

        ' Place dialog form in the middle of the view
        oNewProjectDlg.StartPosition = FormStartPosition.Manual
        Dim oView As Inventor.View = MidAddIn.ActiveView()
        oNewProjectDlg.Location = New System.Drawing.Point(oView.Left + 60, oView.Top + 60) 'oView.Width / 2 - oNewProjectDlg.Size.Width / 2, oView.Top + oView.Height - oNewProjectDlg.Size.Height / 2)

        ' Show dialog and get the path
        If SetProjectDirectoryPath() Then

            ' Save work directory to the commandcollection
            oServer.CommandCollection.WorkDirectory = strProjectFolderPath

            ' Return checkstate: unchecked --> new document
            'If oNewProjectDlg.currDocCheckBox.CheckState = CheckState.Checked Then

            'Close the current document

            Dim oOldDoc As AssemblyDocument = MidAddIn.ActiveDocument



            ' Create new assembly document
            Dim oAsmDoc As AssemblyDocument = MidAddIn.Documents.Add(DocumentTypeEnum.kAssemblyDocumentObject, , True)
            ' --> OnActivateDocument (MidApplicationEvents) is called here to create new browser

            ' Set document name (even if textbox is empty)
            If Not oNewProjectDlg.docNameTextBox.Text.Equals("") Then
                oAsmDoc.FullFileName = oNewProjectDlg.docNameTextBox.Text
            Else
                oAsmDoc.FullFileName = oNewProjectDlg.DefaultDocName
            End If

            oAsmDoc.EnvironmentManager.SetCurrentEnvironment(MidAddIn.UserInterfaceManager.Environments.Item("MidEnvironment"))
            ' --> OnChangeEnvironment (MidInterfceEvents) is called here to activate mid browser tree and disable commands

            oOldDoc.Close(True)

            'End If
            '

            ' Add new nodes to the top node
            oServer.Browser.AddNode(oAsmDoc, "Circuit Carrier", 1)
            oServer.Browser.AddNode(oAsmDoc, "Circuit Board", 1)
            oServer.Browser.AddNode(oAsmDoc, "Circuit Parts", 1)
            oServer.Browser.AddNode(oAsmDoc, "Circuit Net", 1)

            'oServer.Browser.AddNode(oAsmDoc, "Keep-Outs", 1)
            oServer.Browser.InsertChildNode(oAsmDoc, Nothing, "Circuit Carrier", "Keep-Outs")


            ' Enable mid place button
            Dim oControls As ControlDefinitions = MidAddIn.CommandManager.ControlDefinitions
            Dim oControl As ControlDefinition
            oControl = oControls.Item("placeMidIntern")
            oControl.Enabled = True

            ' Stop Command
            StopCommand()

        Else
            ' Stop Command
            StopCommand()

        End If

    End Sub

    'Stop Command
    '***************************************************************************************************************
    Public Overrides Sub StopCommand()

        ' Destroy the command dialog
        oNewProjectDlg = Nothing

    End Sub

    ' Obtain the path for the folder of the work directory
    '***************************************************************************************************************
    Public Function SetProjectDirectoryPath() As Boolean

        'Dim strPath As String = Nothing
        ' Show the dialog (modeless)
        Dim formResult As DialogResult = oNewProjectDlg.ShowDialog

        ' Process the results
        ' User clicks Ok button
        If formResult = Windows.Forms.DialogResult.OK Then

                ' Create new 
                strProjectFolderPath = oNewProjectDlg.pathTxtBox.Text & "\" & oNewProjectDlg.folderTxtBox.Text

                Dim strPathTmp As String = strProjectFolderPath
                Dim i As Integer = 0
                Try
                    While True
                        If Directory.Exists(strPathTmp) Then
                            i += 1
                            strPathTmp = strProjectFolderPath & "(" & (i.ToString) & ")" ' Add an extra tag
                        Else
                            If i <> 0 Then
                                strProjectFolderPath = strProjectFolderPath & "(" & (i.ToString) & ")"
                            End If
                            Exit While
                        End If

                    End While

                    Dim oDirectory As DirectoryInfo = Directory.CreateDirectory(strProjectFolderPath)
                    MessageBox.Show("New work directory has been successfully created", _
                                    "MIDProject", _
                                    MessageBoxButtons.OK, _
                                    MessageBoxIcon.Asterisk)

                Catch ex As Exception
                    MessageBox.Show("error: The process failed")
                End Try

                ' User clicks cancel button
                'ElseIf formResult = Windows.Forms.DialogResult.Cancel Then
                '    Return False

                'Else
                Return True

            End If


         

        Return False
    End Function

    Shared Function GetWorkDirPath() As String
        Throw New NotImplementedException
    End Function

End Class