Option Explicit On
Imports Inventor
Imports System.Runtime.InteropServices
Imports Microsoft.Win32

Public Class Klasse_Linien_Loeschen

    Public Sub DeleteLinesFunction()
        Dim mApp As Inventor.Application = Marshal.GetActiveObject("Inventor.Application")
        Dim oDoc As Inventor.PartDocument = mApp.ActiveDocument
        Dim oSketches As PlanarSketches = oDoc.ComponentDefinition.Sketches
        Dim oLastSketch As PlanarSketch = oSketches.Item(oSketches.Count)
        Dim oLine As SketchLine
        Try
            oLine = oLastSketch.SketchLines.Item(oLastSketch.SketchLines.Count)
        Catch ex As Exception
            MsgBox("Es sind keine löschbaren Linien in dieser Skizze mehr vorhanden")
            'form_DeleteLines.Close()
        End Try
        'oLine = oLastSketch.SketchLines.Item(oLastSketch.SketchLines.Count)

        If oLastSketch.SketchLines.Count = 0 Then
            'form_DeleteLines.Close()
            'MsgBox("There are no lines left to delete")
        Else
            oLine.Delete()
            MsgBox("Die letzte Linie wurde gelöscht")
            'form_DeleteLines.Show()
        End If

    End Sub

End Class
