Imports Inventor
Imports System.Windows.Forms
Imports System.Drawing
Imports System.IO
Imports Microsoft.VisualBasic
'Imports System

'############################################
' New Project Command Dialog
'############################################

Public Class NewProjectCmdDlg
    Inherits System.Windows.Forms.Form

    Private strDefaultDocName As String
    Private strFilePath As String

    Private oAddIn As Inventor.Application
    Private oNewProjectCmd As NewProjectCommand
    Private WithEvents oFoldDlg As FolderBrowserDialog

    ' Constructor
    '***************************************************************************************************************
    Public Sub New(ByVal oAddIn As Inventor.Application, ByVal oNewProjectCmd As NewProjectCommand)

        MyBase.New()

        'This call is required by the Windows Form Designer.

        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        Me.oAddIn = oAddIn
        Me.oNewProjectCmd = oNewProjectCmd

        ' Set default path for textbox
        pathTxtBox.Text = My.Computer.FileSystem.SpecialDirectories.MyDocuments

        ' Create new file dialog
        oFoldDlg = New FolderBrowserDialog()
        oFoldDlg.Description = "Select the directory you want to create the new project in."
        oFoldDlg.ShowNewFolderButton = False
        oFoldDlg.RootFolder = System.Environment.SpecialFolder.Personal

        ' Deactivate occurrence list box by default
        occListBox.Enabled = False
        currDocCheckBox.Checked = False

        ' Set default document name
        strDefaultDocName = "MIDAssembly"
        docNameTextBox.Text = strDefaultDocName

        ' Set default label text
        InfoTextBox.Text = "Note: After creating a new document the" & vbNewLine & "state of your current document will be" & vbNewLine & " lost. Click Cancel and save the document" & vbNewLine & " before you continue"

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
        ' oNewProjectCmd.StopCommand()
    End Sub

    ' Change check state
    '***************************************************************************************************************
    Private Sub currDocCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles currDocCheckBox.CheckStateChanged

        If currDocCheckBox.CheckState = CheckState.Checked Then
            occListBox.Enabled = True
            docNameTextBox.Enabled = False

        Else
            occListBox.Enabled = False
            docNameTextBox.Enabled = True
        End If

    End Sub

    Public ReadOnly Property DefaultDocName As String
        Get
            Return strDefaultDocName
        End Get
    End Property

End Class