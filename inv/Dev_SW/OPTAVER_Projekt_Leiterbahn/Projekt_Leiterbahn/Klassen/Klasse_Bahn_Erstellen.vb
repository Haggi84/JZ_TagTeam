Option Explicit On
Imports Inventor
Imports System.Runtime.InteropServices
Imports Microsoft.Win32

Public Class Klasse_Bahn_Erstellen
    'Inherits Form_Breite_Eingabe
    Public Sub Sub_Erstellen(w As Double, h As Double) 's as String)

        Dim mApp As Inventor.Application
        mApp = Marshal.GetActiveObject("Inventor.Application")
        Dim oDoc As Inventor.PartDocument = mApp.ActiveDocument
        Dim oSketches As PlanarSketches = oDoc.ComponentDefinition.Sketches
        Dim oCompDef As PartComponentDefinition = oDoc.ComponentDefinition

        Dim oTransGeom As TransientGeometry
        oTransGeom = mApp.TransientGeometry

        Dim oSketch As PlanarSketch

        'Dim oFace As Face
        Dim x As Double = oCompDef.SurfaceBodies.Count
        ''Die erste Oberfläche im aktuellen Modell wird referenziert.
        ' oCompDef.WorkPlanes.Item(1)

        'oFace = oCompDef.SurfaceBodies.Item(1).Faces.Item(1)

        'Die aktuellste Skizze (=die linie von SketchLineCmd) holen
        '---------------------------------------------------------------
        Dim oLastSketch As Sketch = oSketches.Item(oSketches.Count)
        'MsgBox(oLastSketch.Name)
        '---------------------------------------------------------------

        'Workplane als Grundlage für Kreisfläche einfügen
        '---------------------------------------------------------
        Dim last_line As Integer = oLastSketch.SketchLines.Count
        Dim oStartpunkt As WorkPoint = oCompDef.WorkPoints.AddByPoint(oLastSketch.SketchLines.Item(last_line).EndSketchPoint)
        ' Dim oStartpunkt As WorkPoint = oCompDef.WorkPoints.AddByPoint(oLastSketch.SketchArcs.Item(oLastSketch.SketchArcs.Count).EndSketchPoint)




        Dim oWorkPlane As WorkPlane
        'auf true setzen wenn die Arbeitsebene ausgeblendet sein soll
        oWorkPlane = oCompDef.WorkPlanes.AddByNormalToCurve(oLastSketch.SketchLines.Item(last_line), oStartpunkt, False)


        Dim overtices(0 To 2) As Point
        '-----------------------------------------------------

        'Kreis auf Workplane setzen
        '-----------------------------------------------------
        Dim oXCoord As Double = oStartpunkt.Point.X
        Dim oYCoord As Double = oStartpunkt.Point.Y
        Dim oZCoord As Double = oStartpunkt.Point.Z
        Dim oCircleCenter As Point = mApp.TransientGeometry.CreatePoint(
        oXCoord, oYCoord, oZCoord)

        '    Dim oForm_Breite_Eingabe As Form_Bahnerstellen = New Form_Bahnerstellen()
        Try
            oCompDef.Features.SweepFeatures.Item(oCompDef.Features.SweepFeatures.Count).Delete()
            oCompDef.WorkPlanes.Item(oCompDef.WorkPlanes.Count).Delete()
            oCompDef.WorkPoints.Item(oCompDef.WorkPoints.Count).Delete()
        Catch ex As Exception
        End Try




        ' Dim oRadius As Double = s 'System.Double.Parse(s)
        ' Dim oKreisNormale As UnitVector = mApp.TransientGeometry.CreateUnitVector(oXCoord, oYCoord, oZCoord)
        ' Dim oCircle As Circle = mApp.TransientGeometry.CreateCircle(oCircleCenter, oKreisNormale, oRadius)

        ''Skizze mit Kreis erstellen
        oSketch = oCompDef.Sketches.Add(oWorkPlane)

        Dim oPoints(6) As Point2d
        ' Create the collection that will contain the fit points for the regular spline.
        Dim oFitPoints As ObjectCollection
        oFitPoints = mApp.TransientObjects.CreateObjectCollection



        oPoints(1) = oTransGeom.CreatePoint2d(0, ((w / 2) * -1) / 10000)
        oPoints(2) = oTransGeom.CreatePoint2d((h / 2) / 10000, ((w / 4) * -1) / 10000)
        oPoints(3) = oTransGeom.CreatePoint2d(h / 10000, 0)
        oPoints(4) = oTransGeom.CreatePoint2d((h / 2) / 10000, ((w / 4)) / 10000)
        oPoints(5) = oTransGeom.CreatePoint2d(0, (w / 2) / 10000)


        oFitPoints.Add(oPoints(1))
        oFitPoints.Add(oPoints(2))
        oFitPoints.Add(oPoints(3))
        oFitPoints.Add(oPoints(4))
        oFitPoints.Add(oPoints(5))

        'oSketch.SketchSplines.Add(oFitPoints)
        ' oSketch.SketchSplines.Add(oFitPoints, SplineFitMethodEnum.kSweetSplineFit)

        'oSketch.SketchCircles.AddByCenterRadius(oSketch.ModelToSketchSpace(oCircleCenter), oRadius)

        Dim spline As SketchSpline = oSketch.SketchSplines.Add(oFitPoints, SplineFitMethodEnum.kSweetSplineFit)
        Dim line As SketchLine = oSketch.SketchLines.AddByTwoPoints(spline.EndSketchPoint, spline.StartSketchPoint)

        'Dim oSketch2 As PlanarSketch
        'oSketch2 = oSketches.Item("Sketch1")

        'Dim test As Inventor.PartDocument
        'MsgBox(oTransGeom.ToString)




        Dim oPathSegments As ObjectCollection
        oPathSegments = mApp.TransientObjects.CreateObjectCollection

        'Dim oEntity As SketchEntity

        oPathSegments.Add(spline)
        oPathSegments.Add(line)


        ''Aus dem Kreis eine "solide" Kreisscheibe machen 'Aufpassen! Doppeltbennenung von "Profile" --> es gibt eine Inventor Klasse und eine Klasse im Projekt --> umbenennen bei gelegenheit!
        ' Dim oProfile As Inventor.Profile = oSketch.Profiles.AddForSolid

        Dim oprofile As Inventor.Profile = oSketch.Profiles.AddForSolid()


        'Dim oprofile As Inventor.Profile = oSketch.Profiles.AddForSolid(True, oPathSegments)


        ''Einen Pfad zum ziehen der Bahn erstellen
        Dim oPath As Path = oCompDef.Features.CreatePath(oLastSketch.SketchLines.Item(last_line))





        ''Die Bahn mithilfe von "Sweep" ziehen
        Dim oSweep As SweepFeature
        oSweep = oCompDef.Features.SweepFeatures.AddUsingPath(oProfile, oPath, PartFeatureOperationEnum.kJoinOperation)
        '-----------------------------------------------------
        Dim oRenderStyle As RenderStyle
        oRenderStyle = oSweep.GetRenderStyle(Inventor.StyleSourceTypeEnum.kPartRenderStyle)

        ' Assign the render style to the part.
        'oSweep.SetRenderStyle(StyleSourceTypeEnum.kBodyRenderStyle, "Red")

        '' Force the view to update to see the change.

        'mApp.ActiveView.Update()






        'Leiterbahn einfärben
        Dim oBlueStyle As RenderStyle = mApp.ActiveDocument.RenderStyles.Item("Cyan")
        oSweep.SetRenderStyle(StyleSourceTypeEnum.kOverrideRenderStyle, oBlueStyle)

        'alles bis auf letzten Sweep entfernen
        'Dim allsketches As ExtrudeFeatures = oDoc.ComponentDefinition.Features.ExtrudeFeatures
        'Dim counter As Double = allsketches.Count
        'Dim exfeat = oDoc.ComponentDefinition.Features.Item(counter)
        'exfeat.Delete()

        stegeErstellen(w, h)
        stegeErstellen2(w, h)
        claddingErstellen(w, h)
    End Sub

    Private Sub stegeErstellen(w As Double, h As Double)
        Dim mApp As Inventor.Application
        mApp = Marshal.GetActiveObject("Inventor.Application")
        Dim oDoc As Inventor.PartDocument = mApp.ActiveDocument
        Dim oSketches As PlanarSketches = oDoc.ComponentDefinition.Sketches
        Dim oCompDef As PartComponentDefinition = oDoc.ComponentDefinition

        Dim oTransGeom As TransientGeometry
        oTransGeom = mApp.TransientGeometry

        Dim oSketch As PlanarSketch

        'Dim oFace As Face
        Dim x As Double = oCompDef.SurfaceBodies.Count
        ''Die erste Oberfläche im aktuellen Modell wird referenziert.
        ' oCompDef.WorkPlanes.Item(1)

        'oFace = oCompDef.SurfaceBodies.Item(1).Faces.Item(1)

        'Die aktuellste Skizze (=die linie von SketchLineCmd) holen
        '---------------------------------------------------------------
        Dim oLastSketch As Sketch = oSketches.Item(2)
        'MsgBox(oLastSketch.Name)
        '---------------------------------------------------------------

        'Workplane als Grundlage für Kreisfläche einfügen
        '---------------------------------------------------------
        Dim last_line As Integer = oLastSketch.SketchLines.Count
        Dim oStartpunkt As WorkPoint = oCompDef.WorkPoints.AddByPoint(oLastSketch.SketchLines.Item(last_line).EndSketchPoint)
        '    Dim oStartpunkt As WorkPoint = oCompDef.WorkPoints.AddByPoint(oLastSketch.SketchArcs.Item(oLastSketch.SketchArcs.Count).EndSketchPoint)




        Dim oWorkPlane As WorkPlane
        'auf true setzen wenn die Arbeitsebene ausgeblendet sein soll
        oWorkPlane = oCompDef.WorkPlanes.AddByNormalToCurve(oLastSketch.SketchLines.Item(last_line), oStartpunkt, False)


        Dim overtices(0 To 2) As Point
        '-----------------------------------------------------

        'Kreis auf Workplane setzen
        '-----------------------------------------------------
        Dim oXCoord As Double = oStartpunkt.Point.X
        Dim oYCoord As Double = oStartpunkt.Point.Y
        Dim oZCoord As Double = oStartpunkt.Point.Z
        Dim oCircleCenter As Point = mApp.TransientGeometry.CreatePoint(
        oXCoord, oYCoord, oZCoord)

        '    Dim oForm_Breite_Eingabe As Form_Bahnerstellen = New Form_Bahnerstellen()
        'Try
        '    oCompDef.Features.SweepFeatures.Item(oCompDef.Features.SweepFeatures.Count).Delete()
        '    oCompDef.WorkPlanes.Item(oCompDef.WorkPlanes.Count).Delete()
        '    oCompDef.WorkPoints.Item(oCompDef.WorkPoints.Count).Delete()
        'Catch ex As Exception
        'End Try




        ' Dim oRadius As Double = s 'System.Double.Parse(s)
        ' Dim oKreisNormale As UnitVector = mApp.TransientGeometry.CreateUnitVector(oXCoord, oYCoord, oZCoord)
        ' Dim oCircle As Circle = mApp.TransientGeometry.CreateCircle(oCircleCenter, oKreisNormale, oRadius)

        ''Skizze mit Kreis erstellen
        oSketch = oCompDef.Sketches.Add(oWorkPlane)

        Dim oPoints(6) As Point2d
        ' Create the collection that will contain the fit points for the regular spline.
        Dim oFitPoints As ObjectCollection
        oFitPoints = mApp.TransientObjects.CreateObjectCollection


        'h = 10 Mikrometer für Steg w = 200 Mikrometer
        oPoints(1) = oTransGeom.CreatePoint2d(0, ((w / 2) * -1) / 10000)
        oPoints(2) = oTransGeom.CreatePoint2d(5 / 10000, ((w / 2) * -1 - 60) / 10000)
        oPoints(3) = oTransGeom.CreatePoint2d(10 / 10000, ((w / 2) * -1 - 100) / 10000)
        oPoints(4) = oTransGeom.CreatePoint2d(5 / 10000, ((w / 2) * -1 - 140) / 10000)
        oPoints(5) = oTransGeom.CreatePoint2d(0, ((w / 2) * -1 - 200) / 10000)


        oFitPoints.Add(oPoints(1))
        oFitPoints.Add(oPoints(2))
        oFitPoints.Add(oPoints(3))
        oFitPoints.Add(oPoints(4))
        oFitPoints.Add(oPoints(5))

        'oSketch.SketchSplines.Add(oFitPoints)
        ' oSketch.SketchSplines.Add(oFitPoints, SplineFitMethodEnum.kSweetSplineFit)

        'oSketch.SketchCircles.AddByCenterRadius(oSketch.ModelToSketchSpace(oCircleCenter), oRadius)

        Dim spline As SketchSpline = oSketch.SketchSplines.Add(oFitPoints, SplineFitMethodEnum.kSweetSplineFit)
        Dim line As SketchLine = oSketch.SketchLines.AddByTwoPoints(spline.EndSketchPoint, spline.StartSketchPoint)

        'Dim oSketch2 As PlanarSketch
        'oSketch2 = oSketches.Item("Sketch1")

        'Dim test As Inventor.PartDocument
        'MsgBox(oTransGeom.ToString)




        Dim oPathSegments As ObjectCollection
        oPathSegments = mApp.TransientObjects.CreateObjectCollection

        'Dim oEntity As SketchEntity

        oPathSegments.Add(spline)
        oPathSegments.Add(line)


        ''Aus dem Kreis eine "solide" Kreisscheibe machen 'Aufpassen! Doppeltbennenung von "Profile" --> es gibt eine Inventor Klasse und eine Klasse im Projekt --> umbenennen bei gelegenheit!
        ' Dim oProfile As Inventor.Profile = oSketch.Profiles.AddForSolid

        Dim oprofile As Inventor.Profile = oSketch.Profiles.AddForSolid()


        'Dim oprofile As Inventor.Profile = oSketch.Profiles.AddForSolid(True, oPathSegments)


        ''Einen Pfad zum ziehen der Bahn erstellen
        Dim oPath As Path = oCompDef.Features.CreatePath(oLastSketch.SketchLines.Item(last_line))





        ''Die Bahn mithilfe von "Sweep" ziehen
        Dim oSweep As SweepFeature
        oSweep = oCompDef.Features.SweepFeatures.AddUsingPath(oprofile, oPath, PartFeatureOperationEnum.kJoinOperation)
        '-----------------------------------------------------
        Dim oRenderStyle As RenderStyle
        oRenderStyle = oSweep.GetRenderStyle(Inventor.StyleSourceTypeEnum.kPartRenderStyle)

        ' Assign the render style to the part.
        'oSweep.SetRenderStyle(StyleSourceTypeEnum.kBodyRenderStyle, "Red")

        '' Force the view to update to see the change.

        'mApp.ActiveView.Update()






        'Leiterbahn einfärben
        Dim oBlueStyle As RenderStyle = mApp.ActiveDocument.RenderStyles.Item("Red")
        oSweep.SetRenderStyle(StyleSourceTypeEnum.kOverrideRenderStyle, oBlueStyle)
    End Sub
    Private Sub stegeErstellen2(w As Double, h As Double)
        Dim mApp As Inventor.Application
        mApp = Marshal.GetActiveObject("Inventor.Application")
        Dim oDoc As Inventor.PartDocument = mApp.ActiveDocument
        Dim oSketches As PlanarSketches = oDoc.ComponentDefinition.Sketches
        Dim oCompDef As PartComponentDefinition = oDoc.ComponentDefinition

        Dim oTransGeom As TransientGeometry
        oTransGeom = mApp.TransientGeometry

        Dim oSketch As PlanarSketch

        'Dim oFace As Face
        Dim x As Double = oCompDef.SurfaceBodies.Count
        ''Die erste Oberfläche im aktuellen Modell wird referenziert.
        ' oCompDef.WorkPlanes.Item(1)

        'oFace = oCompDef.SurfaceBodies.Item(1).Faces.Item(1)

        'Die aktuellste Skizze (=die linie von SketchLineCmd) holen
        '---------------------------------------------------------------
        Dim oLastSketch As Sketch = oSketches.Item(2)
        'MsgBox(oLastSketch.Name)
        '---------------------------------------------------------------

        'Workplane als Grundlage für Kreisfläche einfügen
        '---------------------------------------------------------
        Dim last_line As Integer = oLastSketch.SketchLines.Count
        Dim oStartpunkt As WorkPoint = oCompDef.WorkPoints.AddByPoint(oLastSketch.SketchLines.Item(last_line).EndSketchPoint)
        '    Dim oStartpunkt As WorkPoint = oCompDef.WorkPoints.AddByPoint(oLastSketch.SketchArcs.Item(oLastSketch.SketchArcs.Count).EndSketchPoint)




        Dim oWorkPlane As WorkPlane
        'auf true setzen wenn die Arbeitsebene ausgeblendet sein soll
        oWorkPlane = oCompDef.WorkPlanes.AddByNormalToCurve(oLastSketch.SketchLines.Item(last_line), oStartpunkt, False)


        Dim overtices(0 To 2) As Point
        '-----------------------------------------------------

        'Kreis auf Workplane setzen
        '-----------------------------------------------------
        Dim oXCoord As Double = oStartpunkt.Point.X
        Dim oYCoord As Double = oStartpunkt.Point.Y
        Dim oZCoord As Double = oStartpunkt.Point.Z
        Dim oCircleCenter As Point = mApp.TransientGeometry.CreatePoint(
        oXCoord, oYCoord, oZCoord)

        '    Dim oForm_Breite_Eingabe As Form_Bahnerstellen = New Form_Bahnerstellen()
        'Try
        '    oCompDef.Features.SweepFeatures.Item(oCompDef.Features.SweepFeatures.Count).Delete()
        '    oCompDef.WorkPlanes.Item(oCompDef.WorkPlanes.Count).Delete()
        '    oCompDef.WorkPoints.Item(oCompDef.WorkPoints.Count).Delete()
        'Catch ex As Exception
        'End Try




        ' Dim oRadius As Double = s 'System.Double.Parse(s)
        ' Dim oKreisNormale As UnitVector = mApp.TransientGeometry.CreateUnitVector(oXCoord, oYCoord, oZCoord)
        ' Dim oCircle As Circle = mApp.TransientGeometry.CreateCircle(oCircleCenter, oKreisNormale, oRadius)

        ''Skizze mit Kreis erstellen
        oSketch = oCompDef.Sketches.Add(oWorkPlane)

        Dim oPoints(6) As Point2d
        ' Create the collection that will contain the fit points for the regular spline.
        Dim oFitPoints As ObjectCollection
        oFitPoints = mApp.TransientObjects.CreateObjectCollection


        'h = 10 Mikrometer für Steg w = 200 Mikrometer
        oPoints(1) = oTransGeom.CreatePoint2d(0, ((w / 2)) / 10000)
        oPoints(2) = oTransGeom.CreatePoint2d(5 / 10000, ((w / 2) + 60) / 10000)
        oPoints(3) = oTransGeom.CreatePoint2d(10 / 10000, ((w / 2) + 100) / 10000)
        oPoints(4) = oTransGeom.CreatePoint2d(5 / 10000, ((w / 2) + 140) / 10000)
        oPoints(5) = oTransGeom.CreatePoint2d(0, ((w / 2) + 200) / 10000)


        oFitPoints.Add(oPoints(1))
        oFitPoints.Add(oPoints(2))
        oFitPoints.Add(oPoints(3))
        oFitPoints.Add(oPoints(4))
        oFitPoints.Add(oPoints(5))

        'oSketch.SketchSplines.Add(oFitPoints)
        ' oSketch.SketchSplines.Add(oFitPoints, SplineFitMethodEnum.kSweetSplineFit)

        'oSketch.SketchCircles.AddByCenterRadius(oSketch.ModelToSketchSpace(oCircleCenter), oRadius)

        Dim spline As SketchSpline = oSketch.SketchSplines.Add(oFitPoints, SplineFitMethodEnum.kSweetSplineFit)
        Dim line As SketchLine = oSketch.SketchLines.AddByTwoPoints(spline.EndSketchPoint, spline.StartSketchPoint)

        'Dim oSketch2 As PlanarSketch
        'oSketch2 = oSketches.Item("Sketch1")

        'Dim test As Inventor.PartDocument
        'MsgBox(oTransGeom.ToString)




        Dim oPathSegments As ObjectCollection
        oPathSegments = mApp.TransientObjects.CreateObjectCollection

        'Dim oEntity As SketchEntity

        oPathSegments.Add(spline)
        oPathSegments.Add(line)


        ''Aus dem Kreis eine "solide" Kreisscheibe machen 'Aufpassen! Doppeltbennenung von "Profile" --> es gibt eine Inventor Klasse und eine Klasse im Projekt --> umbenennen bei gelegenheit!
        ' Dim oProfile As Inventor.Profile = oSketch.Profiles.AddForSolid

        Dim oprofile As Inventor.Profile = oSketch.Profiles.AddForSolid()


        'Dim oprofile As Inventor.Profile = oSketch.Profiles.AddForSolid(True, oPathSegments)


        ''Einen Pfad zum ziehen der Bahn erstellen
        Dim oPath As Path = oCompDef.Features.CreatePath(oLastSketch.SketchLines.Item(last_line))





        ''Die Bahn mithilfe von "Sweep" ziehen
        Dim oSweep As SweepFeature
        oSweep = oCompDef.Features.SweepFeatures.AddUsingPath(oprofile, oPath, PartFeatureOperationEnum.kJoinOperation)
        '-----------------------------------------------------
        Dim oRenderStyle As RenderStyle
        oRenderStyle = oSweep.GetRenderStyle(Inventor.StyleSourceTypeEnum.kPartRenderStyle)

        ' Assign the render style to the part.
        'oSweep.SetRenderStyle(StyleSourceTypeEnum.kBodyRenderStyle, "Red")

        '' Force the view to update to see the change.

        'mApp.ActiveView.Update()






        'Leiterbahn einfärben
        Dim oBlueStyle As RenderStyle = mApp.ActiveDocument.RenderStyles.Item("Red")
        oSweep.SetRenderStyle(StyleSourceTypeEnum.kOverrideRenderStyle, oBlueStyle)
    End Sub
    Private Sub CladdingErstellen(w As Double, h As Double)
        Dim mApp As Inventor.Application
        mApp = Marshal.GetActiveObject("Inventor.Application")
        Dim oDoc As Inventor.PartDocument = mApp.ActiveDocument
        Dim oSketches As PlanarSketches = oDoc.ComponentDefinition.Sketches
        Dim oCompDef As PartComponentDefinition = oDoc.ComponentDefinition

        Dim oTransGeom As TransientGeometry
        oTransGeom = mApp.TransientGeometry

        Dim oSketch As PlanarSketch

        'Dim oFace As Face
        Dim x As Double = oCompDef.SurfaceBodies.Count
        ''Die erste Oberfläche im aktuellen Modell wird referenziert.
        ' oCompDef.WorkPlanes.Item(1)

        'oFace = oCompDef.SurfaceBodies.Item(1).Faces.Item(1)

        'Die aktuellste Skizze (=die linie von SketchLineCmd) holen
        '---------------------------------------------------------------
        Dim oLastSketch As Sketch = oSketches.Item(2)
        'MsgBox(oLastSketch.Name)
        '---------------------------------------------------------------

        'Workplane als Grundlage für Kreisfläche einfügen
        '---------------------------------------------------------
        Dim last_line As Integer = oLastSketch.SketchLines.Count
        Dim oStartpunkt As WorkPoint = oCompDef.WorkPoints.AddByPoint(oLastSketch.SketchLines.Item(last_line).EndSketchPoint)
        '    Dim oStartpunkt As WorkPoint = oCompDef.WorkPoints.AddByPoint(oLastSketch.SketchArcs.Item(oLastSketch.SketchArcs.Count).EndSketchPoint)




        Dim oWorkPlane As WorkPlane
        'auf true setzen wenn die Arbeitsebene ausgeblendet sein soll
        oWorkPlane = oCompDef.WorkPlanes.AddByNormalToCurve(oLastSketch.SketchLines.Item(last_line), oStartpunkt, False)


        Dim overtices(0 To 2) As Point
        '-----------------------------------------------------

        'Kreis auf Workplane setzen
        '-----------------------------------------------------
        Dim oXCoord As Double = oStartpunkt.Point.X
        Dim oYCoord As Double = oStartpunkt.Point.Y
        Dim oZCoord As Double = oStartpunkt.Point.Z
        Dim oCircleCenter As Point = mApp.TransientGeometry.CreatePoint(
        oXCoord, oYCoord, oZCoord)

        '    Dim oForm_Breite_Eingabe As Form_Bahnerstellen = New Form_Bahnerstellen()
        'Try
        '    oCompDef.Features.SweepFeatures.Item(oCompDef.Features.SweepFeatures.Count).Delete()
        '    oCompDef.WorkPlanes.Item(oCompDef.WorkPlanes.Count).Delete()
        '    oCompDef.WorkPoints.Item(oCompDef.WorkPoints.Count).Delete()
        'Catch ex As Exception
        'End Try




        ' Dim oRadius As Double = s 'System.Double.Parse(s)
        ' Dim oKreisNormale As UnitVector = mApp.TransientGeometry.CreateUnitVector(oXCoord, oYCoord, oZCoord)
        ' Dim oCircle As Circle = mApp.TransientGeometry.CreateCircle(oCircleCenter, oKreisNormale, oRadius)

        ''Skizze mit Kreis erstellen
        oSketch = oCompDef.Sketches.Add(oWorkPlane)

        Dim oPoints(11) As Point2d
        ' Create the collection that will contain the fit points for the regular spline.
        Dim oFitPoints, oFitPoints2 As ObjectCollection
        oFitPoints = mApp.TransientObjects.CreateObjectCollection
        oFitPoints2 = mApp.TransientObjects.CreateObjectCollection



        'h = 10 Mikrometer für Steg w = 200 Mikrometer
        oPoints(1) = oTransGeom.CreatePoint2d((0 + 1) / 10000, (((w / 2) * -1) - 1) / 10000)
        oPoints(2) = oTransGeom.CreatePoint2d(((h / 2) + 1) / 10000, (((w / 4) * -1) - 1) / 10000)
        oPoints(3) = oTransGeom.CreatePoint2d((h + 1) / 10000, 0)
        oPoints(4) = oTransGeom.CreatePoint2d(((h / 2) + 1) / 10000, ((w / 4) + 1) / 10000)
        oPoints(5) = oTransGeom.CreatePoint2d((0 + 1) / 10000, (w / 2 + 1) / 10000)

        oPoints(6) = oTransGeom.CreatePoint2d((0 + 1) / 10000, ((w / 2) + 10) / 10000)
        oPoints(7) = oTransGeom.CreatePoint2d(((h / 2) + 10) / 10000, ((w / 4) + 10) / 10000)
        oPoints(8) = oTransGeom.CreatePoint2d((h + 10) / 10000, 0)
        oPoints(9) = oTransGeom.CreatePoint2d(((h / 2) + 10) / 10000, (((w / 4) * -1) - 10) / 10000)
        oPoints(10) = oTransGeom.CreatePoint2d((0 + 1) / 10000, ((w / 2) * -1 - 10) / 10000)

        oFitPoints.Add(oPoints(1))
        oFitPoints.Add(oPoints(2))
        oFitPoints.Add(oPoints(3))
        oFitPoints.Add(oPoints(4))
        oFitPoints.Add(oPoints(5))

        oFitPoints2.Add(oPoints(6))
        oFitPoints2.Add(oPoints(7))
        oFitPoints2.Add(oPoints(8))
        oFitPoints2.Add(oPoints(9))
        oFitPoints2.Add(oPoints(10))

        'oSketch.SketchSplines.Add(oFitPoints)
        ' oSketch.SketchSplines.Add(oFitPoints, SplineFitMethodEnum.kSweetSplineFit)

        'oSketch.SketchCircles.AddByCenterRadius(oSketch.ModelToSketchSpace(oCircleCenter), oRadius)

        Dim spline As SketchSpline = oSketch.SketchSplines.Add(oFitPoints, SplineFitMethodEnum.kSweetSplineFit)
        Dim spline2 As SketchSpline = oSketch.SketchSplines.Add(oFitPoints2, SplineFitMethodEnum.kSweetSplineFit)
        Dim line As SketchLine = oSketch.SketchLines.AddByTwoPoints(spline.EndSketchPoint, spline2.StartSketchPoint)
        Dim line2 As SketchLine = oSketch.SketchLines.AddByTwoPoints(spline.StartSketchPoint, spline2.EndSketchPoint)

        'Dim oSketch2 As PlanarSketch
        'oSketch2 = oSketches.Item("Sketch1")

        'Dim test As Inventor.PartDocument
        'MsgBox(oTransGeom.ToString)




        Dim oPathSegments As ObjectCollection
        oPathSegments = mApp.TransientObjects.CreateObjectCollection

        'Dim oEntity As SketchEntity

        oPathSegments.Add(spline)
        oPathSegments.Add(line)
        oPathSegments.Add(spline2)
        oPathSegments.Add(line2)



        ''Aus dem Kreis eine "solide" Kreisscheibe machen 'Aufpassen! Doppeltbennenung von "Profile" --> es gibt eine Inventor Klasse und eine Klasse im Projekt --> umbenennen bei gelegenheit!
        ' Dim oProfile As Inventor.Profile = oSketch.Profiles.AddForSolid

        ' Dim oprofile As Inventor.Profile = oSketch.Profiles.AddForSolid()


        Dim oprofile As Inventor.Profile = oSketch.Profiles.AddForSolid(True, oPathSegments)


        ''Einen Pfad zum ziehen der Bahn erstellen
        Dim oPath As Path = oCompDef.Features.CreatePath(oLastSketch.SketchLines.Item(last_line))





        ''Die Bahn mithilfe von "Sweep" ziehen
        Dim oSweep As SweepFeature
        oSweep = oCompDef.Features.SweepFeatures.AddUsingPath(oprofile, oPath, PartFeatureOperationEnum.kJoinOperation)
        '-----------------------------------------------------
        Dim oRenderStyle As RenderStyle
        oRenderStyle = oSweep.GetRenderStyle(Inventor.StyleSourceTypeEnum.kPartRenderStyle)

        ' Assign the render style to the part.
        'oSweep.SetRenderStyle(StyleSourceTypeEnum.kBodyRenderStyle, "Red")

        '' Force the view to update to see the change.

        'mApp.ActiveView.Update()






        'Leiterbahn einfärben
        Dim oBlueStyle As RenderStyle = mApp.ActiveDocument.RenderStyles.Item("Yellow")
        oSweep.SetRenderStyle(StyleSourceTypeEnum.kOverrideRenderStyle, oBlueStyle)

        'Dim oWorkPlanes As WorkPlane

        For i = 1 To oCompDef.WorkPlanes.Count

            oCompDef.WorkPlanes.Item(i).Visible = False

        Next i

        'Dim sw As SweepFeature = oCompDef.Features.SweepFeatures.Item(1).

        'auf true setzen wenn die Arbeitsebene ausgeblendet sein soll
        '   oWorkPlane = oCompDef.WorkPlanes.AddByNormalToCurve(oLastSketch.SketchLines.Item(last_line), oStartpunkt, False)

    End Sub
End Class
