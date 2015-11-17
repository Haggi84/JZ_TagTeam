Option Explicit On

Imports Inventor
Imports System

Module Intitalize_LB

    Public oFace As Face
    Public oLastSketch As PlanarSketch
    Public oFaceNormal As UnitVector


    Sub Main()
        Dim start_form As New Form_Start_LB
        start_form.ShowDialog()
    End Sub

End Module
