'footprint 1.2
' ***************
' * Ax_Inventor *
' ***************

' Auxilliary Funktionen, Subs und Variable, die mit Inventor zu tun haben
' =======================================================================

Option Explicit On
Imports System.Math
Imports System.Windows.Forms
Imports System.IO

#If _useInventor Then

Imports Inventor

Module Ax_Inventor

    ' ******************
    ' * pointToPoint3d *
    ' ******************

    ' Von einem Inventor-Point von VBA zu einem 3D-Point

    Public Sub pointToPoint3d(ByRef p As Inventor.Point, ByRef a() As Double)
        ReDim a(2)
        a(0) = p.x
        a(1) = p.y
        a(2) = p.z
    End Sub

    ' *******************
    ' * analyseRangeBox *
    ' *******************

    ' Analyse der umschreibenden Box des Bodies

    Public Sub analyseRangeBox(ByRef body As Inventor.SurfaceBody)
        Dim maPoi, miPoi As point
        maPoi = body.RangeBox.MaxPoint
        miPoi = body.RangeBox.MinPoint
        Dim xMax, yMax, zMax, xMin, yMin, zMin, dX, dY, dZ As Double
        xMax = maPoi.x
        yMax = maPoi.y
        zMax = maPoi.z
        xMin = miPoi.x
        yMin = miPoi.y
        zMin = miPoi.z
        dX = xMax - xMin
        dY = yMax - yMin
        dZ = zMax - zMin
        Exit Sub
    End Sub

End Module

#End If