Imports System.Windows.Forms
Imports System.Drawing
Imports System.IO
Imports Microsoft.VisualBasic
Imports System

Public Class PlaceCompForm
    Inherits System.Windows.Forms.Form

    Private oAddin As Inventor.Application
    Private oPlaceCompCmd As PlaceCompCommand

    Public Sub New(ByVal oAddIn As Inventor.Application, ByVal oPlaceCompCmd As PlaceCompCommand)

        MyBase.New()

        'This call is required by the Windows Form Designer.

        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        Me.oAddin = oAddIn
        Me.oPlaceCompCmd = oPlaceCompCmd

        ' Place in the middle of the active view


        ' Me.Visible = True

    End Sub



 
    Private Sub OK_Click(sender As Object, e As EventArgs) Handles buttonOK.Click

        oPlaceCompCmd.StopCommand()




    End Sub




End Class