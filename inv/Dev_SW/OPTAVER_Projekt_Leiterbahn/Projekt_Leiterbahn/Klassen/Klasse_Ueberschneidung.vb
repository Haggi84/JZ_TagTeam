Option Explicit On
Imports Inventor
Imports System.Runtime.InteropServices
Imports Microsoft.Win32

Public Class Klasse_Ueberschneidung

    Public Sub IntersectionFunction()
        Dim mApp As Inventor.Application = Marshal.GetActiveObject("Inventor.Application")
        Dim oDoc As Inventor.PartDocument = mApp.ActiveDocument
        Dim oSketches As PlanarSketches = oDoc.ComponentDefinition.Sketches
        Dim oLastSketch As PlanarSketch = oSketches.Item(oSketches.Count)
        'Dim oPartCompDef As PartComponentDefinition = oDoc.ComponentDefinition
        'Dim oSketch As PlanarSketch = oPartCompDef.Sketches.Add(oDoc.ComponentDefinition.WorkPlanes.Item(oDoc.ComponentDefinition.WorkPlanes.Count))


        'Check if sketched lines intersect with each other or with the edges of the face
        '---------------------------------------------------------------
        Dim Zähler As Integer = oLastSketch.SketchLines.Count
        Dim Zähler2 As Integer = oLastSketch.SketchLines.Count - 2
        ''Dim Zähler2 As Integer = oLastSketch.SketchLines.Count - 1
        'Dim Vergleiche As String()
        Do
            'Exception: Item(Zähler) kann nicht in Line1 umgewandelt werden, nur in SketchLine, aber SketchLine besitzt die IntersectWithCurve Methode nicht
            Dim oLineSegment2dLetzte As LineSegment2d
            Dim oLineSegment2dVorletzte As LineSegment2d
            'SketchLine ist ein 2d objekt auf einer SektchEbene, SketchLine3D ist frei im Raum
            Dim oSketchLine2d As SketchLine = oLastSketch.SketchLines.Item(Zähler)
            'Neuer Versuch, diesmal werden eine Linie mit der vorletzten Verglichen. Denn wenn Linien an ihren Endepunkten aneinander Anliegen gilt das auch als Überschneidung
            'Try
            'If Zähler2 < 2 Then
            '    MsgBox("Keine Überschneidungen vorhanden")
            '    Exit Do
            'End If
            Dim oSketchLine2dVorletzte As SketchLine = oLastSketch.SketchLines.Item(Zähler2)


            Dim oPoint2d1 As Point2d = oSketchLine2d.StartSketchPoint.Geometry
            Dim oPoint2d2 As Point2d = oSketchLine2d.EndSketchPoint.Geometry


            'Dim oLine As SketchLine = oLastSketch.SketchLines.Item(Zähler)
            'If (oLineSegment Is Nothing) Then
            '    MsgBox("Is Nothing Vergleich funktioniert")
            'End If
            'Dim oPoint1 As Point = oLine.StartSketchPoint.Geometry



            oLineSegment2dVorletzte = mApp.TransientGeometry.CreateLineSegment2d(oSketchLine2dVorletzte.StartSketchPoint.Geometry, oSketchLine2dVorletzte.EndSketchPoint.Geometry)

            oLineSegment2dLetzte = mApp.TransientGeometry.CreateLineSegment2d(oSketchLine2d.StartSketchPoint.Geometry, oSketchLine2d.EndSketchPoint.Geometry)

            'Catch ex As Exception
            '    MsgBox("Minimum drei Linien")
            '    Exit Do
            'End Try
            'oLineSegment2dVorletzte = mApp.TransientGeometry.CreateLineSegment2d(oSketchLine2d.StartSketchPoint.Geometry, oSketchLine2d.EndSketchPoint.Geometry)

            'If (oLineSegment2dLetzte.IntersectWithCurve(oLineSegment2dVorletzte) Is Nothing) Then
            '    'MsgBox("keine überschneidung")

            'Else
            '    MsgBox("Überschneidung vorhanden")
            '    Exit Do
            'End If

            If (oLineSegment2dLetzte.IntersectWithCurve(oLineSegment2dVorletzte) Is Nothing) Then
                If Zähler2 = 1 Then
                    MsgBox("Keine Überschneidung vorhanden")
                    Exit Do
                Else
                    Zähler2 -= 1

                    If Zähler2 = 1 Then
                        If Zähler = 2 Then '' voher = 2
                            MsgBox("Keine Überschneidung vorhanden")
                            Exit Do
                        Else
                            Zähler = Zähler - 1
                            Zähler2 = Zähler - 2 '' vorher -2
                        End If
                    End If
                End If
            Else

                'Dim Schnittpunkt As Point = mApp.TransientGeometry.CreatePoint
                'Schnittpunkt.
                'oLineSegment.IntersectWithCurve(oLastSketch.SketchLines.Item(Zähler - 1))

                MsgBox("Die aktuelle Leiterbahnstrecke schneidet sich selbst")
                'MsgBox("Linie " & Zähler & " und Linie " & Zähler2 & " überschneiden sich (Umrandungslinien der Zeichenfläche mitgezählt)")
                Exit Do
            End If

            'Dim oSchnittPunkt As Point2d = mApp.TransientGeometry.CreatePoint2d(oLineSegment2d.IntersectWithCurve(oLastSketch.SketchLines.Item(Zähler - 1)).Item(1))

            'If (oLineSegment2d.IntersectWithCurve(oLastSketch.SketchLines.Item(Zähler - 1)) Is Nothing) Then
            '    Zähler -= 1
            'Else
            '    'Dim Schnittpunkt As Point = mApp.TransientGeometry.CreatePoint
            '    'Schnittpunkt.
            '    MsgBox("Leiterbahnen dürfen keine anderen Linien schneiden")
            '    'oLineSegment.IntersectWithCurve(oLastSketch.SketchLines.Item(Zähler - 1))
            'End If

            ''Zähler bis 3, da eine Fläche mindestens aus 3 Linien besteht und es sind ja nur die Linien auf der Fläche interessant, also alle Linien mit der Linienzahl >3
            'Zähler = 3
        Loop Until Zähler = 0

        '---------------------------------------------------------------


    End Sub

End Class
