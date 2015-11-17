Option Explicit On

Imports Inventor
Imports System.Runtime.InteropServices
Imports Microsoft.Win32
Imports System.Math

Public Class Klasse_Eintritt_Festlegen
#Region "Deklarationen"


    Dim mApp As Inventor.Application
    ' Declare a variable and create a new instance of the select class.
    Dim oSelect As New Klasse_Auswahl
    Dim oDoc As Inventor.AssemblyDocument
    Dim oCompDef As AssemblyComponentDefinition
    Dim oPartDoc As PartDocument
    Dim oPartCompDef As PartComponentDefinition
    Dim oPartSketches As PlanarSketches
    Dim oSketch As PlanarSketch
    Dim oSketch3d As Sketch3D 
    Dim oView As View
    Dim oCam As Camera
    Dim oRange As Box2d
    Dim oEval As SurfaceEvaluator
    Dim params(0 To 1) As Double
    Dim cenPt(2) As Double
    Dim X, Y, Z As Double
    Dim oPoint As Point
    Dim oTG As TransientGeometry
    Dim FaceNormal As UnitVector
    'Vector FaceNormal der sich an der Flächen Normalen Koordinaten NormalCoord orientiert, damit die UpVector-Kamera senkrecht auf die Fläche zeigt
    Dim NormalCoord(2) As Double
#End Region
    Sub Eintritt_festlegen()
        '################################### Allgemeine Zuweisungen #####################################

        mApp = Marshal.GetActiveObject("Inventor.Application")
        oPartDoc = mApp.ActiveDocument
        oPartCompDef = oPartDoc.ComponentDefinition
        'oPartDoc = mApp.Documents.Add(DocumentTypeEnum.kPartDocumentObject, , False)
        'oPartCompDef = oPartDoc.ComponentDefinition
        oPartSketches = oPartDoc.ComponentDefinition.Sketches
        oView = mApp.ActiveView
        'Kamera Ausrichten. Funktioniert momentan nur über oCam.UpVecor, siehe ausgeklammerten Code für oCam.Eye oCam.Target Versuche 
        oCam = oView.Camera
        oTG = mApp.TransientGeometry
        '################################### Allgemeine Zuweisungen #####################################



        '################################### Planare Fläche wird ausgewählt #####################################



        ' Call the Pick method of the clsSelect object and set the filter to pick any face. Veralteter Parameter Pick(kPartFaceFilter) wurde ausgetauscht
        Dim oface As Inventor.Face = oSelect.Pick(SelectionFilterEnum.kPartFaceFilter)

        oEval = oface.Evaluator
        oRange = oEval.ParamRangeRect

        'Mittelpunktkoordinaten der neue Skizze nach denen sich kamera ausrichten soll

        params(0) = oRange.MinPoint.X + (oRange.MaxPoint.X - oRange.MinPoint.X) * 0.5
        params(1) = oRange.MinPoint.Y + (oRange.MaxPoint.Y - oRange.MinPoint.Y) * 0.5

        oEval.GetPointAtParam(params, cenPt)

        'Get a Camera Point with oFace.PointOnFace.X
        X = oface.PointOnFace.X
        Y = oface.PointOnFace.Y
        Z = oface.PointOnFace.Z

        oPoint = mApp.TransientGeometry.CreatePoint(X, Y, Z)
        '################################### Planare Fläche wird ausgewählt #####################################


        'NormalCoord die Normal-Koordinaten übergeben
        oface.Evaluator.GetNormal(params, NormalCoord)






        'Unit Vektor mit NormalCoord Koordinaten
        'NormalenKoordinaten stimmen wenn man die Werte mit Haltepunkten im Debugger anschaut und verschiedene Würfelseiten testet
        FaceNormal = mApp.TransientGeometry.CreateUnitVector(NormalCoord(0), NormalCoord(1), NormalCoord(2))

        'Siehe API Help (nach Boundary Representation suchen) beschreibung von Eye und  Target:
        'Eye müsste in dieselbe richtung zeigen wie UpVector nur weiter entfernt
        'Target müsste auf einen centralen Punkt auf der Oberfläche zeigen
        'oCam.UpVector = FaceNormal
        ' oCam.Apply()



        oSketch = oPartDoc.ComponentDefinition.Sketches.Add(oface, True)
        oSketch.Edit()

        'oSketch3d = oPartDoc.ComponentDefinition.Sketches3D.Add()
        'oSketch3d.Edit()

        Dim oCommandMgr As CommandManager
        oCommandMgr = mApp.CommandManager
        Dim oControlDef As ControlDefinition
        'Übersicht für Controldefinitions unter: http://forums.autodesk.com/autodesk/attachments/autodesk/78/427463/1/InventorCommands.txt


        oControlDef = oCommandMgr.ControlDefinitions.Item("SketchHoleCenterPointCmd")
        oControlDef.Execute()
        '  oSketch.Solve()

        '  oSketch.ExitEdit()



    End Sub




End Class
