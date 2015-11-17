Option Explicit On

Imports Inventor
Imports System.Windows.Forms
Imports System.Drawing
Imports System.IO
Imports Microsoft.VisualBasic

'############################################
' New Project Command Dialog
'############################################

Public Class NewProjectCmdDlg
    Inherits System.Windows.Forms.Form

    Private strDocumentDefaultName As String
    Private strFilePath As String

    Private oAddIn As Inventor.Application
    Private oNewProjectCmd As NewProjectCommand
    Private WithEvents oFoldDlg As FolderBrowserDialog

    ' Constructor
    '***************************************************************************************************************
    Public Sub New(ByVal addIn As Inventor.Application, ByVal newProjectCmd As NewProjectCommand)

        MyBase.New()

        'This call is required by the Windows Form Designer.

        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        Me.oAddIn = addIn
        Me.oNewProjectCmd = newProjectCmd

        ' Set default path for textbox
        pathTxtBox.Text = My.Computer.FileSystem.SpecialDirectories.MyDocuments

        ' Create new file dialog
        oFoldDlg = New FolderBrowserDialog()
        oFoldDlg.Description = "Select the directory you want to create the new project in."
        oFoldDlg.ShowNewFolderButton = False
        oFoldDlg.RootFolder = System.Environment.SpecialFolder.Personal

        ' Set default document name
        strDocumentDefaultName = "MIDAssembly"
        docNameTextBox.Text = strDocumentDefaultName

    End Sub

    ' Browse folders
    '***************************************************************************************************************
    Private Sub browseButton_Click(sender As Object, e As EventArgs) Handles browseButton.Click

        ' open dialog an wait for response
        Dim result As DialogResult = oFoldDlg.ShowDialog()

        If result = Windows.Forms.DialogResult.OK Then
            pathTxtBox.Text = oFoldDlg.SelectedPath
        ElseIf result = Windows.Forms.DialogResult.Cancel Then
            pathTxtBox.Text = System.Environment.SpecialFolder.Personal.ToString
        End If
    End Sub

    ' On close button
    '***************************************************************************************************************
    Private Sub NewProjectCmdDlg_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.FormClosed
        oNewProjectCmd.StopCommand()
    End Sub


    Public ReadOnly Property DefaultDocName As String
        Get
            Return strDocumentDefaultName
        End Get
    End Property


    ' OnOk: set data
    '***************************************************************************************************************
    Private Sub OkButton_Click(sender As Object, e As EventArgs) Handles okButton.Click

        ' Check existing directory and folder
        ' Dim strProjectFolderPath As String = 

        oNewProjectCmd.SetData(pathTxtBox.Text & "\" & folderTxtBox.Text, docNameTextBox.Text)

    End Sub

    ' OnCancel: Stop command
    '***************************************************************************************************************
    Private Sub cancelButton_Click(sender As Object, e As EventArgs) Handles cancelButton.Click
        oNewProjectCmd.StopCommand()
    End Sub

End Class
