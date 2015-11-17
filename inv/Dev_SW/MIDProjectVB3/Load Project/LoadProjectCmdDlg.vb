Imports Inventor
Imports System.Windows.Forms
Imports System.Drawing
Imports System.IO
Imports Microsoft.VisualBasic
'Imports System

'############################################
' New Project Command Dialog
'############################################

Public Class LoadProjectCmdDlg
    Inherits System.Windows.Forms.Form

    Private strDefaultDocName As String
    Private strFilePath As String

    Private oAddIn As Inventor.Application
    Private oLoadProjectCmd As LoadProjectCommand
    Private WithEvents oFileDlg As OpenFileDialog

    ' Constructor
    '***************************************************************************************************************
    Public Sub New(ByVal oAddIn As Inventor.Application, ByVal loadProjectCmd As LoadProjectCommand)

        MyBase.New()

        'This call is required by the Windows Form Designer.

        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        Me.oAddIn = oAddIn
        Me.oLoadProjectCmd = loadProjectCmd

        ' Set default path for textbox
        pathTxtBox.Text = My.Computer.FileSystem.SpecialDirectories.MyDocuments

        ' Create new file dialog
        oFileDlg = New OpenFileDialog()
        oFileDlg.Title = "Select Inventor Assembly File."
        oFileDlg.Filter = "Inventor Assembly Files (*.iam)|*.iam" ' set correct filter for assembly files
        oFileDlg.InitialDirectory = System.Environment.SpecialFolder.Personal ' MyDocument folder as initial directory

    End Sub

    ' Browse folders
    '***************************************************************************************************************
    Private Sub browseButton_Click_1(sender As Object, e As EventArgs) Handles browseButton.Click

        ' open dialog an wait for response
        Dim result As DialogResult = oFileDlg.ShowDialog()

        If result = Windows.Forms.DialogResult.OK Then
            pathTxtBox.Text = oFileDlg.FileName
        ElseIf result = Windows.Forms.DialogResult.Cancel Then
            pathTxtBox.Text = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        End If
    End Sub

    ' On close button
    '***************************************************************************************************************
    Private Sub NewProjectCmdDlg_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.FormClosed
        oLoadProjectCmd.StopCommand()
    End Sub

    ' OnOk: Check for valid input values
    '***************************************************************************************************************
    Private Sub OkButton_Click_1(sender As Object, e As EventArgs) Handles okButton.Click
        oLoadProjectCmd.SetPath(pathTxtBox.Text)
    End Sub

    ' OnCancel: Exit command
    '***************************************************************************************************************
    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles cancelButton.Click
        oLoadProjectCmd.StopCommand()
    End Sub

End Class
