Imports System.Windows.Forms
Imports System.Drawing
Imports System.IO
Imports Microsoft.VisualBasic
Imports System

'#########################################
' ReadNetlistCmdDlg Class
'#########################################

Public Class ReadNetlistCmdDlg
    Inherits System.Windows.Forms.Form

    Private oFileDlg As OpenFileDialog
    Private oFolderDlg As FolderBrowserDialog

    Public strFilePath As String
    Public strFolderPath As String

    Private oReadNetlistCmd As ReadNetlistCommand
    Private oAddin As Inventor.Application

    ' Constructor
    '**********************************************************************************************************************************
    Public Sub New(ByVal addIn As Inventor.Application, _
                   ByVal readNetlistCmd As ReadNetlistCommand)

        MyBase.New()

        'This call is required by the Windows Form Designer.

        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        Me.oAddin = addIn
        Me.oReadNetlistCmd = readNetlistCmd

        ' Set mydocument folder as standard folder for textbox
        Me.filePathBox.Text = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        Me.folderPathBox.Text = My.Computer.FileSystem.SpecialDirectories.MyDocuments

        ' Create new file dialog
        oFileDlg = New OpenFileDialog()
        oFolderDlg = New FolderBrowserDialog()
        oFolderDlg.ShowNewFolderButton = False

    End Sub

    ' Browse for part library
    '**********************************************************************************************************************************
    Private Sub partBrowseButton_Click(sender As Object, e As EventArgs) Handles partBrowseButton.Click

        oFolderDlg.RootFolder = System.Environment.SpecialFolder.Desktop

        Dim result As DialogResult = oFolderDlg.ShowDialog()

        If (result = Windows.Forms.DialogResult.OK) Then

            ' Save file path
            folderPathBox.Text = oFolderDlg.SelectedPath
            strFolderPath = oFolderDlg.SelectedPath

        End If

    End Sub

    ' Browse for netlist file
    '**********************************************************************************************************************************
    Private Sub browseButton_Click(sender As Object, e As EventArgs) Handles browseButton.Click

        oFileDlg.FileName = "*.xml"
        oFileDlg.Filter = "XML files (*.xml)|*.xml" ' only xml files can be choosen
        oFileDlg.InitialDirectory = System.Environment.SpecialFolder.Personal
        oFileDlg.Title = "Select XML file"

        Dim result As DialogResult = oFileDlg.ShowDialog()

        If (result = Windows.Forms.DialogResult.OK) Then

            ' Save file path
            filePathBox.Text = oFileDlg.FileName
            strFilePath = oFileDlg.FileName

        End If

        ' Enable import button now
        importButton.Enabled = True

    End Sub

    ' Import button
    '**********************************************************************************************************************************
    Private Sub importButton_Click(sender As Object, e As EventArgs) Handles importButton.Click

        ' Start BRep import
        oReadNetlistCmd.ReadBRep()

    End Sub

    ' Cancel button
    '**********************************************************************************************************************************
    Private Sub buttonCancel_Click(sender As Object, e As EventArgs) Handles cancelButton.Click

        oReadNetlistCmd.StopCommand()

    End Sub

    ' Ok button
    '**********************************************************************************************************************************
    Private Sub buttonOk_Click(sender As Object, e As EventArgs) Handles okButton.Click

        'oReadNetlistCmd.StopCommand()
        oReadNetlistCmd.ExecuteCommand()
    End Sub

End Class