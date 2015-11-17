'footprint 1.2
' ***********
' * GlobVar *
' ***********

' Globale Variable
' ================

Option Explicit On

'Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Text

Module GlobVar

' Globale Variable der Projektmappe

' Konstante
' =========
Public Const decimalPointSign As String = "."  ' Zeichen für Dezimalpunkt (Punkt oder Komma)
                                               ' Punkt wird in XML verlangt.

Public Const PI      As Double = 3.14159265358979
Public Const TWO_PI  As Double = 6.28318530717959
Public Const HALF_PI As Double = 1.5707963267949


' Durch Dialog bestimmbare Variable
' =================================
Public printKeys                       As Boolean  ' Keys der Objekte aus BRep ausgeben?
Public additionalPrecision             As Integer  ' Anzahl der zusätzlichen Dezimalziffern bei Längen und Winkeln
Public withBRepProcessing              As Boolean  ' Mit BRep-Verarbeitung, einschliesslich Planarisierung?
Public showPlanarBRepInCadAI           As Boolean  ' Soll planarisierte BRep nach Inventor ausgegeben werden?
Public withPlanarizationOfRoutingTask  As Boolean  ' Mit Planarisierung der Routing-Task
Public cadSystem                       As String   ' Cad-System




' Aus vorangegangenen Variablen ermittelte bzw. berechnete Variable
' =================================================================
Public name_of_BRep              As String   ' Name des BRep-Beispiels von Inventor oder von XML-File
Public formatOfCoordinateValues  As String   ' Ausgabeformat für Koordinatenwerte und Gewichtskoeffizienten
Public formatOfAngleValues       As String   ' Ausgabeformat für Winkelangaben
Public full_FileName_BRep        As String   ' Voller Name der BRep-XML-Datei
Public full_FileName_Circuit     As String   ' Voller Name der Circuit-XML-Datei
Public full_FileName_RoutingTask As String   ' Voller Name der Routing-Task-XML-Datei
Public fullSchemeName            As String   ' Voller Name der XSD-Datei
Public unitOfLength              As String   ' Längeneinheit
Public unitOfAngles              As String   ' Winkeleinheit
Public precisionOfLength         As String   ' Genauigkeit der Längenmaße
Public precisionOfAngles         As String   ' Genauigkeit der Winkelangaben




' File-Variable
' =============
Public Dim outFile As StreamWriter               ' Aktueller File, in den etwas geschrieben wird

' ******************
' * initialization *
' ******************

Public Function initialization() As Boolean

#If _useInventor Then
        Dim succ As Boolean = initialization_inventor()
        If Not (succ) Then
            Return False
        End If
#End If
    
    ' Einzüge für das XML-Dokument vorbereiten
    ' ----------------------------------------
    Ax.fillIndention
    level = 0
    
    
    ' Statistik initialisieren
    ' ------------------------
    Statistics.InitStatistics


    outFile = Nothing
    
    'Ax.switchToHourGlassCursor       'Nicht löschen!

    Return True

End Function

End Module