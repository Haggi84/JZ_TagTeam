'footprint 1.2
Option Explicit On


Imports Inventor


Imports System.IO

Module Statistics

' Knoten
' ------
Public numVertices As Integer
Public xMin, xMax, yMin, yMax, zMin, zMax As Double

' Kanten
' ------
Public numEdges                     As Integer
Public numLineSegments              As Integer
Public numCircles                   As Integer
Public numCircularArcs              As Integer
Public numEllipticalArcs            As Integer
Public numBSplineCurves             As Integer
Public numBSplineCurvesOfOrder(4)   As Integer
Public numEllipses                  As Integer
Public numLines                     As Integer
Public numPolylines                 As Integer
Public numUnknownCurves             As Integer


' Facetten
' --------
Public numFaces                        As Integer            
Public numPlanes                       As Integer
Public numCylinders                    As Integer
Public numCones                        As Integer
Public numTori                         As Integer
Public numSpheres                      As Integer
Public numBSplineSurfaces              As Integer
Public numBSplineSurfacesOfOrder(4, 4) As Integer
Public numEllipticalCones              As Integer
Public numEllipticalCylinders          As Integer
Public numUnknownSurfaces              As Integer


Public Sub InitStatistics()

   Dim i, j As Integer
   
   ' Knoten
   ' ------
   numVertices = 0

   ' Kanten
   ' ------
   numEdges          = 0
   numLineSegments   = 0
   numCircles        = 0
   numCircularArcs   = 0
   numEllipticalArcs = 0
   numBSplineCurves  = 0
   numEllipses       = 0
   numLines          = 0
   numPolylines      = 0
   numUnknownCurves  = 0
   
   For i = 1 To 4
      numBSplineCurvesOfOrder(i) = 0
   Next i

   ' Facetten
   ' --------
   numFaces                = 0            
   numPlanes               = 0
   numCylinders            = 0
   numCones                = 0
   numTori                 = 0
   numSpheres              = 0
   numBSplineSurfaces      = 0
   numEllipticalCones      = 0
   numEllipticalCylinders  = 0
   numUnknownSurfaces      = 0
   
   For i = 1 To 4
      For j = 1 To 4
         numBSplineSurfacesOfOrder(i, j) = 0
      Next j
   Next i
End Sub

Public Sub writeStatistics()
   openTag(Statistics_)
    
   ' Knoten
   simpleTag(VertexStatistics_, mkAttrib("n", numVertices))
      
   ' Kanten
   writeEdgeStatistics()
      
   ' Facetten
   writeFaceStatistics()
      
   ' Abschluss Statistik
   closeTag(Statistics_)
   
   'Ax.switchToHourGlassCursor       'Nicht löschen!
End Sub

Private Sub writeEdgeStatistics()

   ' Start-Tag
   ' ---------
   Dim attributes As String
   attributes = mkAttrib("n",                numEdges) _
              & mkAttrib("lineSegments",     numLineSegments) _
              & mkAttrib("circles",          numCircles) _
              & mkAttrib("circularArcs",     numCircularArcs) _
              & mkAttrib("ellipticalArcs",   numEllipticalArcs) _
              & mkAttrib("bSplineCurves",    numBSplineCurves) _
              & mkAttrib("ellipses",         numEllipses) _
              & mkAttrib("lines",            numLines) _
              & mkAttrib("polylines",        numPolylines) _
              & mkAttrib("unknownCurves",    numUnknownCurves)
              
    openTag(EdgeStatistics_, attributes)
   
   ' Differenzierende Statistik zu B-Spline-Kurven
   ' ---------------------------------------------
   writeBSplineCurveStatistics()
      
   ' End-Tag
   ' -------
   closeTag(EdgeStatistics_)
   
End Sub

Private Sub writeFaceStatistics()

   ' Kopf Facetten
   ' =============
   Dim attributes As String
   attributes = mkAttrib("n", numFaces) & mkAttrib("planes", numPlanes) & mkAttrib("cylinders", numCylinders) _
              & mkAttrib("cones", numCones) & mkAttrib("tori", numTori) & mkAttrib("spheres", numSpheres) _
              & mkAttrib("bSplineSurfaces", numBSplineSurfaces) & mkAttrib("ellipticalCones", numEllipticalCones) _
              & mkAttrib("ellipticalCylinders", numEllipticalCylinders) & mkAttrib("unknownSurfaces", numUnknownSurfaces)
   openTag(FaceStatistics_, attributes)
                    
   ' Spezielle Statistik zu verschiedenen B-Spline-Kurven
   ' ====================================================
   If numBSplineSurfaces <> 0 Then
      writeBSplineSurfaceStatistics()
   End If
    
   ' Abschuss Facetten
   ' =================
   closeTag(FaceStatistics_)
End Sub

Private Sub writeBSplineCurveStatistics()
   If (numBSplineCurves = 0) Then
      Exit Sub
   End If
      
   ' Start-Tag
   ' ---------
   openTag(BSplineCurveStatistics_, mkAttrib("n", numBSplineCurves))

   ' Inhalt
   ' ======
   Dim i As Integer
   For i = 1 To 4
      If (numBSplineCurvesOfOrder(i) <> 0) Then
         simpleTag("Order", mkAttrib("ord", i) & mkAttrib("n", numBSplineCurvesOfOrder(i)))
      End If
   Next i
   
   ' Abschluss
   ' =========
   closeTag(BSplineCurveStatistics_)

End Sub

Private Sub writeBSplineSurfaceStatistics()

   openTag(BSplineSurfaceStatistics_, mkAttrib("n", numBSplineSurfaces))
   
   Dim i, j As Integer
   incrt(level)
   For i = 1 To 4
      For j = 1 To 4
         If (numBSplineSurfacesOfOrder(i, j) <> 0) Then
            simpleTag("OrderUV", mkAttrib("ordU", i) & mkAttrib("ordV", j) & mkAttrib("n", numBSplineSurfacesOfOrder(i, j)))
         End If
      Next j
   Next i
   decrt(level)
   
   closeTag(BSplineSurfaceStatistics_)
End Sub

End Module

