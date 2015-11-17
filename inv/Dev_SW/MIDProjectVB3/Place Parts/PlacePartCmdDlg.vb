Imports System.Windows.Forms
Imports System.Drawing
Imports System.IO
Imports Microsoft.VisualBasic
Imports System

'########################################
' PlacePartCmdDlg
'########################################

Public Class PlacePartCmdDlg
    Inherits System.Windows.Forms.Form

    Private oAddin As Inventor.Application
    Private oPlaceCompCmd As PlacePartCommand

    Public moveStepSize As String
    Public rotateStepSize As String

    'Constructor
    '******************************************************************************************************************
    Public Sub New(ByVal oAddIn As Inventor.Application, _
                   ByVal oPlaceCompCmd As PlacePartCommand)

        MyBase.New()

        'This call is required by the Windows Form Designer.

        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        Me.oAddin = oAddIn
        Me.oPlaceCompCmd = oPlaceCompCmd

        ' Place in the middle of the active view


        ' Get units of measure object
        Dim oUOF As UnitsOfMeasure = oAddIn.ActiveDocument.UnitsOfMeasure

        ' Set current documents length units
        lengthUnitLabel.Text = oUOF.GetStringFromType(oUOF.LengthUnits)
        angleUnitLabel.Text = oUOF.GetStringFromType(oUOF.AngleUnits)

        ' Initialize default values
        moveStepSize = "0.05"
        rotateStepSize = "5.0"

        moveTextBox.Text = moveStepSize
        rotateTextBox.Text = rotateStepSize

    End Sub

    ' Ok button
    '******************************************************************************************************************
    Private Sub OkButton_Click(sender As Object, e As EventArgs) Handles okButton.Click
        oPlaceCompCmd.Ok()
    End Sub

    ' Cancel button
    '******************************************************************************************************************
    Private Sub cancelButton_Click(sender As Object, e As EventArgs) Handles cancelButton.Click
        oPlaceCompCmd.Cancel()
    End Sub

    Private Sub applyButton_Click(sender As Object, e As EventArgs) Handles applyButton.Click
        oPlaceCompCmd.Apply()
    End Sub

    ' moveTextbox --> text changed
    '******************************************************************************************************************
    Private Sub moveTextBox_TextChanged(sender As Object, e As EventArgs) Handles moveTextBox.TextChanged

        If Not InputIsValid(moveTextBox.Text.Trim()) Then
            moveTextBox.ForeColor = System.Drawing.Color.Red
            upButton.Enabled = False
            downButton.Enabled = False
            rightButton.Enabled = False
            leftButton.Enabled = False
        Else
            moveTextBox.ForeColor = System.Drawing.Color.Black
            moveStepSize = moveTextBox.Text
            upButton.Enabled = True
            downButton.Enabled = True
            rightButton.Enabled = True
            leftButton.Enabled = True
        End If

    End Sub

    ' rotateTextbox --> text changed
    '******************************************************************************************************************
    Private Sub rotateTextBox_TextChanged(sender As Object, e As EventArgs) Handles rotateTextBox.TextChanged

        If Not InputIsValid(rotateTextBox.Text.Trim()) Then
            rotateTextBox.ForeColor = System.Drawing.Color.Red
            rotateLeftButton.Enabled = False
            rotateRightButton.Enabled = False
        Else
            rotateTextBox.ForeColor = System.Drawing.Color.Black
            rotateStepSize = rotateTextBox.Text
            rotateLeftButton.Enabled = True
            rotateRightButton.Enabled = True
        End If
    End Sub

    ' Check value validity
    '******************************************************************************************************************
    Private Function InputIsValid(ByVal value As String) As Boolean

        ' Try to convert value to double, if process fails return false
        Try
            Dim output As Double = System.Convert.ToDouble(value)

            If output <= 0 Then
                Return False
            End If

        Catch ex As Exception
            Return False
        End Try

        Return True
    End Function

    ' Help button
    '******************************************************************************************************************
    Private Sub buttonHelp_Click(sender As Object, e As EventArgs) Handles buttonHelp.Click
        oAddin.HelpManager.DisplayHelpTopic("ADMAPI_12_0.chm", "")
    End Sub


    Private Sub rotateLeftButton_Click(sender As Object, e As EventArgs) Handles leftButton.Click
        oPlaceCompCmd.MoveLeft()
    End Sub

    Private Sub rightButton_Click(sender As Object, e As EventArgs) Handles rightButton.Click
        oPlaceCompCmd.MoveRight()
    End Sub

    Private Sub downButton_Click(sender As Object, e As EventArgs) Handles downButton.Click
        oPlaceCompCmd.MoveDown()
    End Sub

    Private Sub upButton_Click(sender As Object, e As EventArgs) Handles upButton.Click
        oPlaceCompCmd.MoveUp()
    End Sub

    Private Sub rotateLeftButton_Click_1(sender As Object, e As EventArgs) Handles rotateLeftButton.Click
        oPlaceCompCmd.RotateLeft()
    End Sub

    Private Sub rotateRightButton_Click(sender As Object, e As EventArgs) Handles rotateRightButton.Click
        oPlaceCompCmd.RotateRight()
    End Sub

    Private Sub alignComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles alignComboBox.SelectedIndexChanged
        If alignComboBox.Text.Equals("Align To Edge") Then
            oPlaceCompCmd.Align()
        End If
    End Sub

End Class