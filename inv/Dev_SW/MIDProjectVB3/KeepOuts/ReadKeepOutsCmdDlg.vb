Imports System.Windows.Forms
Imports System.Drawing
Imports System.IO
Imports Microsoft.VisualBasic
Imports System

'############################################
' Read Keep Out Command Dialog
'############################################

Public Class ReadKeepOutsCmdDlg
    Inherits System.Windows.Forms.Form

    Private _filePath As String
    Private oReadKeepOutCmd As ReadKeepOutCommand
    Private oFileDlg As OpenFileDialog

    Private oAddin As Inventor.Application

    ' Constructor
    '***************************************************************************************************************
    Public Sub New(ByVal oAddIn As Inventor.Application, ByVal readKeepOutCmd As ReadKeepOutCommand)

        MyBase.New()

        'This call is required by the Windows Form Designer.

        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        Me.oAddin = oAddIn
        Me.oReadKeepOutCmd = readKeepOutCmd

        ' Set mydocument folder as standard folder for textbox
        Me.filePathBox.Text = My.Computer.FileSystem.SpecialDirectories.MyDocuments

        ' Create new file dialog
        oFileDlg = New OpenFileDialog()

        buttonCancel.Enabled = False

    End Sub

    ' Select Path
    '***************************************************************************************************************
    Private Sub browseButton_Click(sender As Object, e As EventArgs) Handles browseButton.Click

        oFileDlg.FileName = "*.xml"
        oFileDlg.Filter = "XML files (*.xml)|*.xml" ' only xml files can be choosen
        oFileDlg.InitialDirectory = System.Environment.SpecialFolder.Personal
        oFileDlg.Title = "Select XML file"

        Dim result As DialogResult = oFileDlg.ShowDialog()

        If (result = Windows.Forms.DialogResult.OK) Then

            ' Save file path
            filePathBox.Text = oFileDlg.FileName
            _filePath = oFileDlg.FileName

        End If
    End Sub

    ' Load Xml
    '***************************************************************************************************************
    Private Sub loadButton_Click(sender As Object, e As EventArgs) Handles loadButton.Click

        oReadKeepOutCmd.SetPath(_filePath)

    End Sub

End Class