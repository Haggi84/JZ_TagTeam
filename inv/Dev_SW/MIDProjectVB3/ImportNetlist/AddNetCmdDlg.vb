Imports System.Windows.Forms
Imports System.Drawing
Imports System.IO
Imports Microsoft.VisualBasic
Imports System

Public Class AddNetCmdDlg
    Inherits System.Windows.Forms.Form

    Private oFileDlg As OpenFileDialog
    Private strFilePath As String

    Private oImportNetCmd As AddNetCommand
    Private oAddin As Inventor.Application
    Private oPlaceCompCmd As PlaceCompCommand

    Public Sub New(ByVal oAddIn As Inventor.Application, ByVal oImportNetCmd As AddNetCommand)

        MyBase.New()

        'This call is required by the Windows Form Designer.

        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        Me.oAddin = oAddIn
        Me.oImportNetCmd = oImportNetCmd
        'Me.oPlaceCompCmd = oPlaceCompCmd

        ' Set mydocument folder as standard folder for textbox
        Me.filePathBox.Text = My.Computer.FileSystem.SpecialDirectories.MyDocuments

        ' Create new file dialog
        oFileDlg = New OpenFileDialog()

    End Sub



    Private Sub filePathBox_TextChanged(sender As Object, e As EventArgs) Handles filePathBox.TextChanged

    End Sub

    ' Browse for Netlist file
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

    End Sub

    ' Import button
    Private Sub importButton_Click(sender As Object, e As EventArgs) Handles importButton.Click

        ' Check whether the file exists
        If Not File.Exists(strFilePath) Then
            MessageBox.Show("File does not exist, please try another file", "MIDProject", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            ' Deactivate browse and import button
            browseButton.Enabled = False
            importButton.Enabled = False

            ' Start BRep import
            oImportNetCmd.ImportBRep(strFilePath)

            ' make buttons unselectable
            buttonOk.Enabled() = False
            buttonCancel.Enabled() = False
        End If

    End Sub

    Private Sub buttonCancel_Click(sender As Object, e As EventArgs) Handles buttonCancel.Click

        oImportNetCmd.StopCommand()

    End Sub


    Private Sub buttonOk_Click(sender As Object, e As EventArgs) Handles buttonOk.Click

        oImportNetCmd.StopCommand()
    End Sub

 
    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged

    End Sub
End Class