Imports System.Windows.Forms
Imports System.Drawing
Imports System.IO
Imports Microsoft.VisualBasic
Imports System


Public Class PlaceMidCmdDlg

    Private oAddIn As Inventor.Application
    Private oPlaceMidCmd As PlaceMidCommand

    Public Sub New(ByVal addIn As Inventor.Application, _
                   ByVal placeMidCmd As PlaceMidCommand)

        MyBase.New()

        'This call is required by the Windows Form Designer.

        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        Me.oAddIn = addIn
        Me.oPlaceMidCmd = placeMidCmd

        ' Set mydocument folder as standard folder for textbox
        Me.filePathBox.Text = My.Computer.FileSystem.SpecialDirectories.MyDocuments

        ' Disable comboboxes by default
        unitOfAnglesCB.Enabled = False
        anglePrecCB.Enabled = False
        unitOfLengthCB.Enabled = False
        lengthPrecCB.Enabled = False

        ' Set inventor default values
        Dim UOM As UnitsOfMeasure = oAddIn.ActiveDocument.UnitsOfMeasure
        unitOfAnglesCB.Text = UOM.GetStringFromType(UOM.AngleUnits)
        anglePrecCB.Text = UOM.AngleDisplayPrecision.ToString
        unitOfLengthCB.Text = UOM.GetStringFromType(UOM.LengthUnits)
        lengthPrecCB.Text = UOM.LengthDisplayPrecision.ToString

        ' Set collection of available units of measure
        Dim LengthUnitsCollection(4) As String
        LengthUnitsCollection(0) = UOM.GetStringFromType(UnitsTypeEnum.kCentimeterLengthUnits)
        LengthUnitsCollection(1) = UOM.GetStringFromType(UnitsTypeEnum.kFootLengthUnits)
        LengthUnitsCollection(2) = UOM.GetStringFromType(UnitsTypeEnum.kInchLengthUnits)
        LengthUnitsCollection(3) = UOM.GetStringFromType(UnitsTypeEnum.kMillimeterLengthUnits)
        LengthUnitsCollection(4) = UOM.GetStringFromType(UnitsTypeEnum.kYardLengthUnits)
        unitOfLengthCB.Items.AddRange(LengthUnitsCollection)

        Dim AngleUnitsCollection(2) As String
        AngleUnitsCollection(0) = UOM.GetStringFromType(UnitsTypeEnum.kDegreeAngleUnits)
        AngleUnitsCollection(1) = UOM.GetStringFromType(UnitsTypeEnum.kRadianAngleUnits)
        AngleUnitsCollection(2) = UOM.GetStringFromType(UnitsTypeEnum.kGradAngleUnits)
        unitOfAnglesCB.Items.AddRange(AngleUnitsCollection)

    End Sub

    ' Browse for Netlist file
    '***************************************************************************************************************
    Private Sub browseButton_Click(sender As Object, e As EventArgs) Handles browseButton.Click

        ' Get the inventor file dialog
        Dim oFileDlg As Inventor.FileDialog
        oAddIn.CreateFileDialog(oFileDlg)

        ' set filter to ipt-files only
        oFileDlg.Filter = "Inventor Part File |*ipt"
        oFileDlg.DialogTitle = "Open MID-Part"
        oFileDlg.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        oFileDlg.CancelError = False

        oFileDlg.ShowOpen()
        filePathBox.Text = oFileDlg.FileName

        ' Save file name to command
        oPlaceMidCmd.FileName = oFileDlg.FileName

    End Sub

    ' OnOk: Execute command 
    '***************************************************************************************************************
    Private Sub buttonOk_Click(sender As Object, e As EventArgs) Handles buttonOk.Click

        oPlaceMidCmd.SetExportData(anglePrecCB.Text, lengthPrecCB.Text, unitOfAnglesCB.Text, unitOfLengthCB.Text)
        ' Check validity
        If Not File.Exists(filePathBox.Text) Then
            MessageBox.Show("Pleas choose a valid .ipt file", "MIDProject", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            oPlaceMidCmd.ExecuteCommand()
        End If
    End Sub

    ' Set default values
    '***************************************************************************************************************
    Private Sub defaultCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles defaultCheckBox.CheckStateChanged

        If defaultCheckBox.Checked = True Then
            ' Set inventor default values

            ' Dim UOM As UnitsOfMeasure = Me.oAddIn.ActiveDocument.UnitsOfMeasure
            unitOfAnglesCB.Enabled = False
            ' unitOfAnglesCB.Text = UOM.GetStringFromType(UOM.AngleUnits)
            anglePrecCB.Enabled = False
            'anglePrecCB.Text = UOM.AngleDisplayPrecision.ToString
            unitOfLengthCB.Enabled = False
            'unitOfLengthCB.Text = UOM.GetStringFromType(UOM.LengthUnits)
            lengthPrecCB.Enabled = False
            'lengthPrecCB.Text = UOM.LengthDisplayPrecision.ToString

        Else
            unitOfAnglesCB.Enabled = True
            anglePrecCB.Enabled = True
            unitOfLengthCB.Enabled = True
            lengthPrecCB.Enabled = True

        End If

    End Sub

    ' OnCancel: stop command
    '***************************************************************************************************************
    Private Sub buttonCancel_Click(sender As Object, e As EventArgs) Handles buttonCancel.Click
        oPlaceMidCmd.StopCommand()
    End Sub

    ' OnClose: stop command
    '***************************************************************************************************************
    Private Sub NewProjectCmdDlg_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.FormClosed
        oPlaceMidCmd.StopCommand()
    End Sub


End Class