'footprint
Option Explicit On
Imports Inventor
Imports System.Runtime.InteropServices
Imports Microsoft.Win32
Imports System.Math




Public Class Klasse_Linien_Zeichnen
    Public Sub Sub_LinienZeichnen()

        Dim mApp As Inventor.Application
        mApp = Marshal.GetActiveObject("Inventor.Application")

        ' Declare a variable and create a new instance of the select class.
        Dim oSelect As New Klasse_Auswahl
        'Dim oDoc As Inventor.PartDocument = mApp.ActiveDocument

        'AssemblyDocuments werden eingeführt
        Dim oDoc As Inventor.AssemblyDocument = mApp.ActiveDocument
        Dim oCompDef As AssemblyComponentDefinition = oDoc.ComponentDefinition

        'PartDocument wird eingeführt
        Dim oPartDoc As PartDocument = mApp.Documents.Add(DocumentTypeEnum.kPartDocumentObject, , False)
        Dim oPartCompDef As PartComponentDefinition = oPartDoc.ComponentDefinition
        Dim pathName = oDoc.File.ReferencedFiles.Item(1).FullFileName
        oPartDoc = TryCast(mApp.Documents.Open(pathName, True), PartDocument)

        'Sketch wird hinzugefügt (Part)
        Dim oPartSketches As PlanarSketches = oPartDoc.ComponentDefinition.Sketches
        Dim oSketch As PlanarSketch

        ' Call the Pick method of the clsSelect object and set the filter to pick any face. Veralteter Parameter Pick(kPartFaceFilter) wurde ausgetauscht
        oFace = oSelect.Pick(SelectionFilterEnum.kPartFaceFilter)


        'Kamera Ausrichten. Funktioniert momentan nur über oCam.UpVecor, siehe ausgeklammerten Code für oCam.Eye oCam.Target Versuche
        '---------------------------------------------------------
        Dim oView As View = mApp.ActiveView
        Dim oCam As Camera = oView.Camera

        Dim oEval As SurfaceEvaluator
        oEval = oFace.Evaluator

        Dim oRange As Box2d
        oRange = oEval.ParamRangeRect


        'Mittelpunktkoordinaten der neue Skizze nach denen sich kamera ausrichten soll
        Dim params(0 To 1) As Double
        params(0) = oRange.MinPoint.X + (oRange.MaxPoint.X - oRange.MinPoint.X) * 0.5
        params(1) = oRange.MinPoint.Y + (oRange.MaxPoint.Y - oRange.MinPoint.Y) * 0.5

        'Double Array
        Dim cenPt(2) As Double
        oEval.GetPointAtParam(params, cenPt)

        'Get a Camera Point with oFace.PointOnFace.X
        Dim X As Double = oFace.PointOnFace.X
        Dim Y As Double = oFace.PointOnFace.Y
        Dim Z As Double = oFace.PointOnFace.Z
        Dim oPoint As Point
        oPoint = mApp.TransientGeometry.CreatePoint(X, Y, Z)

        'Vector FaceNormal der sich an der Flächen Normalen Koordinaten NormalCoord orientiert, damit die UpVector-Kamera senkrecht auf die Fläche zeigt
        Dim NormalCoord(2) As Double

        'NormalCoord die Normal-Koordinaten übergeben
        oFace.Evaluator.GetNormal(params, NormalCoord)

        Dim oTG As TransientGeometry
        oTG = mApp.TransientGeometry


        'Unit Vektor mit NormalCoord Koordinaten
        'NormalenKoordinaten stimmen wenn man die Werte mit Haltepunkten im Debugger anschaut und verschiedene Würfelseiten testet
        Dim FaceNormal As UnitVector = mApp.TransientGeometry.CreateUnitVector(NormalCoord(0), NormalCoord(1), NormalCoord(2))

        'Siehe API Help (nach Boundary Representation suchen) beschreibung von Eye und  Target:
        'Eye müsste in dieselbe richtung zeigen wie UpVector nur weiter entfernt
        'Target müsste auf einen centralen Punkt auf der Oberfläche zeigen
        oCam.UpVector = FaceNormal
        oCam.Apply()
        '----------------------------------------------------------

        ' Prüft das nur eine Oberfläche angewählt wurde
        If Not oFace Is Nothing Then
            'Part
            oSketch = oPartDoc.ComponentDefinition.Sketches.Add(oFace, True)
            oSketch.Edit()

            'Call the SketchLines command
            Dim oCommandMgr As CommandManager
            oCommandMgr = mApp.CommandManager
            Dim oControlDef As ControlDefinition
            oControlDef = oCommandMgr.ControlDefinitions.Item("SketchLineCmd")
            oControlDef.Execute()

        End If


        'Die aktuellste Skizze (=die linie von SketchLineCmd) holen
        '---------------------------------------------------------------
        Dim oLastSketch As PlanarSketch = oPartSketches.Item(oPartSketches.Count)
        'Dim line As SketchLine = oLastSketch.SketchLines.Item(1)
        '  Dim point As Point = line.EndSketchPoint

        Dim xN, yN, zN As Double
        xN = Nothing
        yN = Nothing
        zN = Nothing

    End Sub

End Class
