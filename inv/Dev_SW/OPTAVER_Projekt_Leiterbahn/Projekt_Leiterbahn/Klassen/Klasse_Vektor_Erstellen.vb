Imports Inventor
Imports System.Runtime.InteropServices


Public Class Klasse_Vektor_Erstellen
    Sub Vektor_erstellen()

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
        'Dim sp As WorkPoint  = oLastSketch.SketchPoints.Item(oLastSketch.Count)
        '  Dim wp As WorkPoint = 

        'Dim uv As UnitVector = sp.Geometry



    End Sub
End Class
