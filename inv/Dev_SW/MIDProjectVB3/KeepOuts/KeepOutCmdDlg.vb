Option Explicit On

Imports System.Windows.Forms
Imports System.Drawing
Imports System.IO
Imports Microsoft.VisualBasic
Imports System

'############################################
' KeepOutCmdDlg
'############################################

Public Class KeepOutCmdDlg
    Inherits System.Windows.Forms.Form

    'Private oFileDlg As OpenFileDialog

    Private oKeepOutCmd As KeepOutCommand
    Private oAddin As Inventor.Application


    Public Sub New(ByVal addIn As Inventor.Application, _
                   ByVal keepOutCmd As KeepOutCommand)

        MyBase.New()

        'This call is required by the Windows Form Designer.

        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        Me.oAddin = addIn
        Me.oKeepOutCmd = keepOutCmd

        ' default: write xml automatically
        Me.writeXmlCheck.Checked = True

        ' Set mydocument folder as standard folder for textbox
        'Me.filePathBox.Text = My.Computer.FileSystem.SpecialDirectories.MyDocuments

        ' Create new file dialog
        'oFileDlg = New OpenFileDialog()

        buttonCancel.Enabled = False

        updateDlg()

    End Sub

    ' Ok button
    '***************************************************************************************************************
    Private Sub buttonOk_Click(sender As Object, e As EventArgs) Handles buttonOk.Click

        oKeepOutCmd.ExecuteCommand()
        'oKeepOutCmd.StopCommand()

    End Sub

    ' Cancel button
    '***************************************************************************************************************
    Private Sub buttonCancel_Click(sender As Object, e As EventArgs) Handles buttonCancel.Click
        oKeepOutCmd.StopCommand()
    End Sub

    ' Update number of KeepOut faces shown in the dialog
    '***************************************************************************************************************
    Public Sub updateDlg()
        ' Set number of keep outs in the beginning
        infoLabel.Text = oKeepOutCmd.KeepOutFaces.Count & " KeepOut face(s) is/are currently selected"
    End Sub

    ' Handle buttons
    '***************************************************************************************************************
    Private Sub buttonAdd_Click(sender As Object, e As EventArgs) Handles buttonAdd.Click
        oKeepOutCmd.AddFaceToKeepOuts()
    End Sub

    Private Sub buttonRmv_Click(sender As Object, e As EventArgs) Handles buttonRmv.Click
        oKeepOutCmd.RmvFaceFromKeepOuts()
    End Sub


    Private Sub filterCombobox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles filterCombobox.SelectedIndexChanged

    End Sub

    ' Reset button
    '***************************************************************************************************************
    Private Sub buttonReset_Click(sender As Object, e As EventArgs) Handles buttonReset.Click
        oKeepOutCmd.ClearKeepOuts()
    End Sub

    Private Sub buttonHelp_Click(sender As Object, e As EventArgs) Handles buttonHelp.Click

        'Get the help manager object and display the start page of the "Inventor API help"
        oAddin.HelpManager.DisplayHelpTopic("ADMAPI_12_0.chm", "")

    End Sub

    Private Sub RackFaceCmdDlg_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
        oKeepOutCmd.StopCommand()
    End Sub


    ' Select Path
    '***************************************************************************************************************
    'Private Sub browseButton_Click(sender As Object, e As EventArgs) Handles browseButton.Click

    '    oFileDlg.FileName = "*.xml"
    '    oFileDlg.Filter = "XML files (*.xml)|*.xml" ' only xml files can be choosen
    '    oFileDlg.InitialDirectory = System.Environment.SpecialFolder.Personal
    '    oFileDlg.Title = "Select XML file"

    '    Dim result As DialogResult = oFileDlg.ShowDialog()

    '    If (result = Windows.Forms.DialogResult.OK) Then

    '        ' Save file path
    '        filePathBox.Text = oFileDlg.FileName
    '        strFilePath = oFileDlg.FileName

    '    End If
    'End Sub

    ' Load Xml
    '***************************************************************************************************************
    'Private Sub loadButton_Click(sender As Object, e As EventArgs) Handles loadButton.Click

    '    ' Check whether the file exists
    '    If Not File.Exists(strFilePath) Then
    '        MessageBox.Show("File path not valid", "MID Project", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '    Else
    '        ' Notification for the user
    '        Dim result As DialogResult = MessageBox.Show("Already selected KeepOuts will be overridden", "MID Project", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
    '        If result = Windows.Forms.DialogResult.OK Then
    '            ' Deactivate buttons temporary
    '            browseButton.Enabled = False
    '            loadButton.Enabled = False
    '            buttonOk.Enabled() = False
    '            buttonCancel.Enabled() = False

    '            ' Start BRep import
    '            'oKeepOutCmd.ReadKeepOuts(strFilePath)

    '            ' Activate buttons again
    '            buttonCancel.Enabled = True
    '            buttonOk.Enabled = True
    '            browseButton.Enabled = True
    '            loadButton.Enabled = True

    '        End If
    '    End If
    'End Sub

   
End Class