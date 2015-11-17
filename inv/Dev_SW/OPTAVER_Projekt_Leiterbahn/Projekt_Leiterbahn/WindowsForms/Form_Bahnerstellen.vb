Option Explicit On
Imports Inventor
Imports System.Runtime.InteropServices
Imports Microsoft.Win32

Public Class Form_Bahnerstellen
    Dim oKlasse_Bahn_Erstellen As Klasse_Bahn_Erstellen = New Klasse_Bahn_Erstellen
    'Dim dblBreite As Double
    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Dim mApp As Inventor.Application
        mApp = Marshal.GetActiveObject("Inventor.Application")
        Dim oDoc As Inventor.PartDocument = mApp.ActiveDocument
        Dim oCompDef As PartComponentDefinition = oDoc.ComponentDefinition
        Try
            oCompDef.Features.SweepFeatures.Item(oCompDef.Features.SweepFeatures.Count).Delete()
            oCompDef.WorkPlanes.Item(oCompDef.WorkPlanes.Count).Delete()
            oCompDef.WorkPoints.Item(oCompDef.WorkPoints.Count).Delete()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        'Dim s As String
        's = TextBox1.Text
        'oKlasse_Bahn_Erstellen.Sub_Erstellen(w)
        ''s wieder leeren
        's = Nothing
    End Sub

    Private Sub Form_Leiterbahn_Erstellen_closing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Form_Bahnerstellen.ActiveForm.Hide()
        TextBox1.Clear()
    End Sub

End Class