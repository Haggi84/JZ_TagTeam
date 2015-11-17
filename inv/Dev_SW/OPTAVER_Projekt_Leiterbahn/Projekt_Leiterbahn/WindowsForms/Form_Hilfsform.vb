Option Explicit On
Imports Inventor
Imports System.Runtime.InteropServices
Imports Microsoft.Win32
Imports System.Drawing
Imports System.Windows.Forms

Public Class Form_Hilfsform
    Dim oFormGroß As New Form_Start_LB

    Public Sub Funktion_FormGroßVerstecken()
        oFormGroß.Hide()
    End Sub

    Public Sub Funktion_Form_GroßAufrufen()
        oFormGroß.Show()

    End Sub
End Class