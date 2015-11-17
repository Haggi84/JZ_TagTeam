'footprint 1.2
' *************
' * TransFace *
' *************

' Übertragung der Facetten
' ========================

Option Explicit



Imports Inventor

Module TransFace

    Public Sub translateFaces(ByRef fList As Faces)
        openTag(Faces_)

        Dim n, i As Integer
        n = fList.Count
        Statistics.numFaces = n
        For i = 1 To n
            translateFace(fList, i)
        Next i

        closeTag(Faces_)

        'Ax.switchToHourGlassCursor       'Nicht löschen!
    End Sub

    Private Sub translateFace(ByRef fList As Faces, ByVal i As Integer)

        Dim face As Face
        face = fList(i)

        ' Start-Tag
        Dim attribFace As String
        attribFace = mkAttrib(id_, "F" & i)
        If (printKeys) Then
            attribFace = attribFace & mkAttrib(extRef_, faceToKeyString(face))
        End If
        openTag(Face_, attribFace)

        ' Struktur der Facette
        openTag(FaceStructure_)
        translateLoops(face, i)
        closeTag(FaceStructure_)

        ' Geometrie
        translateFaceGeometry(face)

        ' End-Tag
        closeTag(Face_)

    End Sub

    Private Sub translateLoops(ByRef face As Face, ByVal i As Integer)
        openTag(Loops_)

        ' Listen der Loops bereitstellen
        Dim loopList As EdgeLoops
        loopList = face.EdgeLoops()
        Dim n, j As Integer
        n = loopList.Count

        For j = 1 To n
            translateLoop(loopList, i, n, j)
        Next j

        closeTag(Loops_)
    End Sub

    Private Sub translateLoop(ByRef loopList As EdgeLoops, ByVal i As Integer, ByVal n As Integer, ByVal j As Integer)

        ' Start-Tag
        Dim idLoop As String
        If (n > 1) Then
            idLoop = "L" & i & "_" & j
        Else
            idLoop = "L" & i
        End If

        'openTag(Loop_, mkAttrib(loopID_, idLoop))

        ' Edge-Uses
        Dim lp As EdgeLoop
        lp = loopList(j)
        Dim outer As Boolean
        outer = lp.IsOuterEdgeLoop
        Dim sOuter As String
        If (outer) Then
            sOuter = "+1"
        Else
            sOuter = "-1"
        End If

        Dim listOfEdgeUses As EdgeUses
        listOfEdgeUses = lp.EdgeUses()

        openTag(Loop_, mkAttrib(loopID_, idLoop) & mkAttrib(outer_, sOuter))

        Dim m, k As Integer
        m = listOfEdgeUses.Count
        For k = 1 To m
            translateEdgeUse(listOfEdgeUses, k)
        Next k

        ' End-Tag
        closeTag(Loop_)

    End Sub

    Private Sub translateFaceGeometry(ByRef Face As Face)

        ' Start-Tag
        ' =========
        openTag(FaceGeometry_)

        Dim geoType As SurfaceTypeEnum
        geoType = Face.SurfaceType()


        Select Case geoType

            ' ----------------------------------
            ' Treten beim FAPS-Auto auf:
            ' ----------------------------------

            Case SurfaceTypeEnum.kPlaneSurface      ' Ebene
                translatePlane(Face)

            Case SurfaceTypeEnum.kCylinderSurface   ' Zylinder
                translateCylinder(Face)

            Case SurfaceTypeEnum.kConeSurface       ' Kegel
                translateCone(Face)

            Case SurfaceTypeEnum.kTorusSurface      ' Torus
                translateTorus(Face)

            Case SurfaceTypeEnum.kSphereSurface     ' Kugeloberfläche
                translateSphere(Face)

            Case SurfaceTypeEnum.kBSplineSurface    ' B-Spline-Fläche
                translateBSplineSurface(Face)

            Case SurfaceTypeEnum.kEllipticalCylinderSurface
                translateEllipticalCylinder(Face)


                ' --------------------------------------------------------------------------
                ' Treten beim FAPS-Auto nicht auf, werden aber später hier auch abgehandelt
                ' --------------------------------------------------------------------------

            Case SurfaceTypeEnum.kEllipticalConeSurface
                translateEllipticalCone(Face)


            Case Else
                translateUnknownSurface(Face)

        End Select


        ' End-Tag
        ' =======
        closeTag(FaceGeometry_)
    End Sub

    Private Sub translatePlane(ByRef Face As Face)

        ' Statistik
        incrt(numPlanes)

        ' Start-Tag
        openTag(Plane_)

        ' Geometrie-Tags
        Dim Plane As Plane
        Plane = Face.geometry

        Dim rootPoint() As Double = New Double() {}
        Dim normalVector() As Double = New Double() {}
        Plane.GetPlaneData(rootPoint, normalVector)
        convPoint3d(rootPoint)
        simpleTag(Location_, Ax.point3dToS(rootPoint))
        simpleTag(PlaneNormal_, Ax.point3dToS(normalVector))

        ' End-Tag
        closeTag(Plane_)

    End Sub

    Private Sub translateCylinder(ByRef Face As Face)

        ' Statistik
        incrt(numCylinders)

        ' Geometrie ermitteln
        Dim Cylinder As Cylinder
        Dim basePoint() As Double = New Double() {}
        Dim axisVector() As Double = New Double() {}
        Dim r As Double = 0
        Cylinder = Face.geometry
        Cylinder.GetCylinderData(basePoint, axisVector, r)
        r = convLength(r)
        convPoint3d(basePoint)
        normalize(axisVector)

        ' Start-Tag
        openTag(Cylinder_, mkAttrib(radius_, Ax.dToS(r)))

        ' Geometrie-Tags
        simpleTag(BasePoint_, Ax.point3dToS(basePoint))
        simpleTag(AxisVector_, Ax.point3dToS(axisVector))

        ' End-Tag
        closeTag(Cylinder_)

    End Sub

    Private Sub translateCone(ByRef Face As Face)

        ' Statistik
        ' ---------
        incrt(numCones)

        ' Geometrie ermitteln
        ' -------------------
        Dim Cone As Cone
        Dim basePoint() As Double = New Double() {}  ' Base Point
        Dim axisVector() As Double = New Double() {}  ' Axis Vector
        Dim r As Double      ' Radius
        Dim hA As Double     ' Half Angle
        Dim expanded As Boolean     ' Expanded (bedeutet was?)
        Cone = Face.geometry
        Cone.GetConeData(basePoint, axisVector, r, hA, expanded)
        ' Apex ermitteln
        Dim Apex(2) As Double ' Kegelspitze
        Ax.detApex(hA, r, axisVector, basePoint, Apex)
        ' Axis Vector umkehren
        Ax.minusVector(axisVector)

        ' Konvertierungen und Normierungen
        convPoint3d(basePoint)
        convPoint3d(Apex)
        normalize(axisVector)
        r = convLength(r)
        hA = convAngle(hA)


        ' Start-Tag
        ' ---------
        Dim attribHA, attribE As String
        attribHA = mkAttrib(halfAngle_, Ax.aToS(hA))
        attribE = mkAttrib(expanding_, Ax.bToS(expanded))
        openTag(Cone_, attribHA & attribE)

        ' Geometrie-Tags
        ' --------------
        simpleTag(AxisVector_, Ax.point3dToS(axisVector))
        simpleTag(Apex_, Ax.point3dToS(Apex))

        ' End-Tag
        ' -------
        closeTag(Cone_)

    End Sub

    Private Sub translateBSplineSurface(ByRef Face As Face)

        ' Statistik
        ' ---------
        incrt(numBSplineSurfaces)


        ' Geometrie ermitteln
        ' -------------------
        Dim spline As BSplineSurface
        spline = Face.geometry
        Dim order(1) As Integer
        Dim numPoles(1) As Integer
        Dim numKnots(1) As Integer
        Dim isRational As Boolean
        Dim isPeriodic() As Boolean = New Boolean() {}
        Dim isClosed() As Boolean = New Boolean() {}
        Dim isPlanar As Boolean
        Dim planeVector(1) As Double

        spline.GetBSplineInfo(order, numPoles, numKnots, isRational, isPeriodic, isClosed, isPlanar, planeVector)

        ' Geometrie testen
        ' ----------------
        If (order(0) < 1 Or 4 < order(0)) Then
            MidMsgBoxInformation("translateBSplineSurface: Order of B-spline surface is out of range")
        End If
        If (order(1) < 1 Or 4 < order(1)) Then
            MidMsgBoxInformation("translateBSplineSurface: Order of B-spline surface is out of range")
        End If


        ' Attribute für Start-Tag erzeugen
        ' --------------------------------
        Dim attribCloU, attribCloV, attribOrU, attribOrV, attribPla, attribRat As String

        If (isClosed(0)) Then
            attribCloU = mkAttrib(closedU_, Ax.bToS(True))
        Else
            attribCloU = ""
        End If

        If (isClosed(1)) Then
            attribCloV = mkAttrib(closedV_, Ax.bToS(True))
        Else
            attribCloV = ""
        End If

        If (isPlanar) Then
            attribPla = mkAttrib(planar_, Ax.bToS(True))
        Else
            attribPla = ""
        End If

        attribOrU = mkAttrib(orderU_, order(0))
        attribOrV = mkAttrib(orderV_, order(1))
        attribRat = mkAttrib(rational_, Ax.bToS(isRational))

        openTag(BSplineSurface_, attribOrU & attribOrV & attribRat & attribCloU & attribCloV & attribPla)


        ' Geometrie-Tags
        ' ==============

        ' Spline-Geometrie an face auswerten
        ' ----------------------------------

        ' Spline-Infos
        Dim nPU As Integer
        Dim nPV As Integer
        Dim nKU As Integer
        Dim nKV As Integer
        nPU = numPoles(0)
        nPV = numPoles(1)
        nKU = numKnots(0)
        nKV = numKnots(1)

        'Spline-Data
        Dim poles() As Double = New Double() {}
        Dim KnotsU() As Double = New Double() {}
        Dim KnotsV() As Double = New Double() {}
        Dim Weights() As Double = New Double() {}
        spline.GetBSplineData(poles, KnotsU, KnotsV, Weights)
        Dim nw As Integer
        nw = UBound(Weights)


        ' Statistik
        incrt(numBSplineSurfacesOfOrder(order(0), order(1)))


        ' Kontroll-Punkte übertragen
        ' --------------------------

        ' Start-Tag ControlPoints
        Dim attribN, attribM As String
        attribN = mkAttrib(n_, nPU)
        attribM = mkAttrib(m_, nPV)

        openTag(SurfaceControlPoints_, attribN & attribM)


        ' Die einzelnen Kontroll-Punkte ausgeben
        Dim iP As Integer ' Index für das Feld Poles
        iP = 0
        Dim iu, iv As Integer  ' Indizes für den Durchlauf durch die Pol-Punkte (Kontrollpunkte)
        Dim attribIU, attribIV As String
        Dim arr(2) As Double

        For iv = 1 To nPV
            attribIV = mkAttrib("j", iv)
            For iu = 1 To nPU
                attribIU = mkAttrib("i", iu)
                ' Hole die nächsten drei Koordinatenwerte aus poles und bereite sie als Punktbeschreibung auf
                arrToPoint3d(poles, iP, arr)               ' (iP wird in der Funktion arrToPoint3d weitergestellt)
                convPoint3d(arr)
                simpleTag(SurfaceControlPoint_, attribIU & attribIV & point3dToS(arr))
            Next iu
        Next iv

        ' End-Tag ControlPoints
        closeTag(SurfaceControlPoints_)


        ' Gewichte ausgeben
        ' -----------------
        ' Start-Tag der Gewichte
        openTag(SurfaceWeights_, attribN & attribM)

        ' Die einzelnen Gewichte ausgeben
        Dim attribW As String
        Dim iW As Integer ' Index für das Feld der Gewichte
        iW = 0

        For iv = 1 To nPV
            attribIV = mkAttrib("j", iv)
            For iu = 1 To nPU
                attribIU = mkAttrib("i", iu)
                Try
                    attribW = mkAttrib(val_, dToS(Weights(iW)))
                Catch ex As Exception
                    '  MsgBox(ex.ToString)
                    Continue For
                End Try

                simpleTag(SurfaceWeight_, attribIU & attribIV & attribW)
                iW = iW + 1
            Next iu
        Next iv

        ' End-Tag der Gewichte
        closeTag(SurfaceWeights_)


        ' U-Knots ausgeben
        ' ----------------

        ' Start-Tag der U-Knots
        openTag(Knots_, mkAttrib(name_, nameOfKnotsU_) & mkAttrib(numberOfKnotsU_, nKU))

        ' Die einzelnen Knots ausgeben
        Dim attribI, attribVal As String
        Dim i As Integer

        For i = 1 To nKU
            attribI = mkAttrib(indexOfKnotsU_, i)
            attribVal = mkAttrib(val_, dToS(KnotsU(i - 1)))
            simpleTag(Knot_, attribI & attribVal)
        Next i

        ' End-Tag der U-Knots
        closeTag(Knots_)



        ' V-Knots ausgeben
        ' ----------------

        ' Start-Tag der V-Knots
        openTag(Knots_, mkAttrib(name_, nameOfKnotsV_) & mkAttrib(numberOfKnotsV_, nKV))

        ' Die einzelnen Knots ausgeben
        Dim attribJ As String
        Dim j As Integer
        For j = 1 To nKV
            attribJ = mkAttrib(indexOfKnotsV_, j)
            attribVal = mkAttrib(val_, dToS(KnotsV(j - 1)))
            simpleTag(Knot_, attribJ & attribVal)
        Next j

        ' End-Tag der V-Knots
        closeTag(Knots_)


        ' Plane-Vektor ausgeben
        ' ---------------------
        If isPlanar Then
            simpleTag(PlaneNormal_, Ax.point3dToS(planeVector))
        End If

        ' End-Tag
        ' =======
        closeTag(BSplineSurface_)
    End Sub

    Private Sub translateTorus(ByRef Face As Face)

        ' Statistik
        incrt(numTori)

        ' Geometrie ermitteln
        Dim Torus As Torus
        Torus = Face.geometry
        Dim centerPoint() As Double = New Double() {}
        Dim axisVector() As Double = New Double() {}
        Dim majorRadius As Double
        Dim minorRadius As Double
        Torus.GetTorusData(centerPoint, axisVector, majorRadius, minorRadius)
        minorRadius = convLength(minorRadius)
        majorRadius = convLength(majorRadius)
        convPoint3d(centerPoint)
        normalize(axisVector)

        ' Start-Tag
        Dim attribMIR, attribMAR As String
        attribMIR = mkAttrib(minorRadius_, Ax.dToS(minorRadius))
        attribMAR = mkAttrib(majorRadius_, Ax.dToS(majorRadius))
        openTag(Torus_, attribMIR & attribMAR)

        ' Geometrie-Tags
        simpleTag(CenterPoint_, Ax.point3dToS(centerPoint))
        simpleTag(AxisVector_, Ax.point3dToS(axisVector))

        ' End-Tag
        closeTag(Torus_)

    End Sub

    Private Sub translateSphere(ByRef Face As Face)

        ' Statistik
        incrt(numSpheres)

        ' Geometrie ermitteln
        Dim sphe As Sphere = Face.geometry
        Dim centerPoint() As Double = New Double() {}
        Dim radius As Double

        sphe.GetSphereData(centerPoint, radius)
        convPoint3d(centerPoint)
        radius = convLength(radius)

        ' Start-Tag
        openTag(Sphere_, mkAttrib(radius_, Ax.dToS(radius)))

        ' Geometrie-Tags
        simpleTag(CenterPoint_, Ax.point3dToS(centerPoint))

        ' End-Tag
        closeTag(Sphere_)
    End Sub

    Private Sub translateEllipticalCylinder(ByRef Face As Face)
        ' Statistik
        incrt(numEllipticalCylinders)

        ' Geometrie ermitteln
        Dim ellCyl As EllipticalCylinder
        Dim basePoint() As Double = New Double() {}
        Dim axisVector() As Double = New Double() {}
        Dim majorAxis() As Double = New Double() {}
        Dim ratio As Double = 0
        ellCyl = Face.geometry
        ellCyl.GetEllipticalCylinderData(basePoint, axisVector, majorAxis, ratio)
        convPoint3d(basePoint)
        normalize(axisVector)
        convPoint3d(majorAxis)
        Dim majorRadius As Double
        majorRadius = euclideanLength(majorAxis)
        Dim minorRadius As Double
        minorRadius = ratio * majorRadius
        normalize(majorAxis)

        ' Start-Tag
        openTag(EllipticalCylinder_, mkAttrib(majorRadius_, Ax.dToS(majorRadius)) & mkAttrib(minorRadius_, Ax.dToS(minorRadius)))

        ' Geometrie-Tags
        simpleTag(BasePoint_, Ax.point3dToS(basePoint))
        simpleTag(AxisVector_, Ax.point3dToS(axisVector))
        simpleTag(MajorAxis_, Ax.point3dToS(majorAxis))

        ' End-Tag
        closeTag(EllipticalCylinder_)

    End Sub

    Private Sub translateEllipticalCone(ByRef Face As Face)
        ' Statistik
        incrt(numEllipticalCones)

        openTag(EllipticalCone_)
        closeTag(EllipticalCone_)

        MidMsgBoxProblem("ReadBRepFromPM: EllipticalCone")
    End Sub

    Private Sub translateUnknownSurface(ByRef Face As Face)
        ' Statistik
        incrt(numUnknownSurfaces)

        openTag(UnknownSurface_)
        closeTag(UnknownSurface_)

        Msg.MidMsgBoxProblem("ReadBRepFromPM: Unknown Surface")
    End Sub

    Private Sub translateEdgeUse(ByRef listOfEdgeUses As EdgeUses, ByVal k As Integer)

        Dim eU As EdgeUse
        eU = listOfEdgeUses(k)

        Dim key As String
        Dim e As Edge
        e = eU.Edge()
        key = edgeToKeyString(e)

        simpleTag(EdgeUse_, mkAttrib(edgeID_, "E" & getNumberOfEdge(key)))

    End Sub

End Module


