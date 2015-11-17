'footprint 1.2
' **************************
' * WritePlanarBRepToCadAI *
' **************************


' Schreibe die planarisierte BRep in das Cad-System Autodesk Inventor
#If _useInventor Then
Option Explicit
Imports System.IO
Imports System.Windows.Forms
Imports PM_Layout.Files
Imports System.Text




Module WritePlanarBRepToCadAI

    Public Function write_planar_brep_to_inventor() As Boolean

        ' Voraussetzungen testen
        Dim lmex As Boolean = layoutModel_LM_exists()
        If Not(lmex) Then
           MidMsgBoxProblem("writePlanarBRepToInventor failed - layout model doesn't exist")
           Return False
        End If

        ' Planare BRep zur Übergabe in temporären File schreiben

        ' Absolute Pfade für planarisierte BRep
        'Dim nameOfTempFile As String = "IHR PFAD"
        ' Dim nameOfTempFile As String = "D:\Projekte\Projekt MID-Layout\MID_Layout_DEV\PM_Layout_CPP\application\bin\XML\Planar_BRep.xml"
        Dim nameOfTempFile As String = brepPath0 & "\Planar_BRep.xml"
        outFile = New StreamWriter(nameOfTempFile, False, Encoding.ASCII)
        outFile.Close()

        Dim rc As Integer = writePlanarBRepToXML(nameOfTempFile)
        If rc <> 0 Then
            Return False
        End If

        ' Planare BRep aus temporärem File auslesen und nach Inventor übertragen
        Dim succ As Boolean = showPlanarBRepInInventor(nameOfTempFile)
        If Not (succ) Then
            Return False
        End If

        Return True
    End Function


    Private Function showPlanarBRepInInventor(ByRef nameOfTempFile As String) As Boolean
        Dim succ As Boolean

        initialize_environment()
        succ = PBRepXML_Reader(nameOfTempFile)

        If Not (succ) Then
            Return False
        End If

        Return True
    End Function


 
End Module

#End If