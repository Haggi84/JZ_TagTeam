'footprint 1.2
' ********************
' * GlobVar_Inventor *
' ********************

' Globale Variable, die mit Inventor zu tun haben
' ===============================================

Option Explicit On
Imports System.Math
Imports System.Windows.Forms
Imports System.IO
Imports System.Runtime.InteropServices

#If _useInventor Then

Imports Inventor

Module GlobVar_Inventor

    ' Applikation Inventor
    Public thisApplication As Inventor.application    ' Aktuelle Applikation

    Public uom As Inventor.UnitsOfMeasure ' Objekt der Maßeinheiten


    Public Function initialization_inventor() As Boolean

        Try
            thisApplication = Marshal.GetActiveObject("Inventor.Application")
        Catch ex As Exception
            MidMsgBoxProblem("Inventor is not running")
            Return False
        End Try

        ' Initialisierung mit Eintragungen aus dem Dokument
        ' =================================================
        Dim doc As document
        doc = thisApplication.ActiveDocument
        If doc Is Nothing Then
            MidMsgBoxProblem("In Inventor no document is open")
            Return False
        End If

        'Dim uom As unitsOfMeasure
        uom = doc.unitsOfMeasure
        Dim ute As UnitsTypeEnum

        ' Name
        ' ----
        GlobVar.name_of_BRep = doc.DisplayName    ' Mit Dateizusatz
        cutOffExtension(GlobVar.name_of_BRep)


        ' Koordinatenwerte
        ' ----------------
        ' Einheit
        ute = uom.LengthUnits
        unitOfLength = uom.GetStringFromType(ute)
        If (unitOfLength = "millimeter") Then
            unitOfLength = "mm"
        ElseIf (unitOfLength = "meter") Then
            unitOfLength = "m"
        ElseIf (unitOfLength = "zentimeter") Then
            unitOfLength = "cm"
        ElseIf (unitOfLength = "centimeter") Then
            unitOfLength = "cm"
        End If
        ' Präzision
        precisionOfLength = uom.LengthDisplayPrecision
        ' Format für die Darstellung von Koordinatenwerten
        Dim precL, i As Integer
        precL = CInt(precisionOfLength) + additionalPrecision
        formatOfCoordinateValues = "0"
        If (precL > 0) Then
            formatOfCoordinateValues = formatOfCoordinateValues & ".0"
        End If
        For i = 1 To (precL - 1)
            formatOfCoordinateValues = formatOfCoordinateValues & "#"
        Next i

        ' Winkel
        ' ------
        ' Einheit
        ute = uom.AngleUnits
        unitOfAngles = uom.GetStringFromType(ute)
        ' Präzision
        precisionOfAngles = uom.AngleDisplayPrecision
        ' Format für die Darstellung von Winkelwerten
        Dim precA As Integer
        precA = CInt(precisionOfAngles + additionalPrecision)
        formatOfAngleValues = "0"
        If (precA > 0) Then
            formatOfAngleValues = formatOfAngleValues & ".0"
        End If
        For i = 1 To (precA - 1)
            formatOfAngleValues = formatOfAngleValues & "#"
        Next i

        ' Arbeit mit den Referenz-Keys vorbereiten
        ' ----------------------------------------
        KeyMan.initKeyManaging()

        Return True

    End Function

End Module

#End If