'footprint 1.2
' *************
' * TransEdge *
' *************

' Übertragung der Kanten
' ======================

Option Explicit



Imports Inventor

Module TransEdge

    Public Sub translateEdges(ByRef eList As Edges)
        openTag(Edges_)

        Dim n, i As Integer
        n = eList.Count
        Statistics.numEdges = n
        For i = 1 To n
            translateEdge(eList, i)
        Next i

        closeTag(Edges_)

        'Ax.switchToHourGlassCursor       'Nicht löschen!
    End Sub

    Private Sub translateEdge(ByRef eList As Edges, ByVal i As Integer)

        ' Initialisierungen
        ' =================
        Dim edge As Edge
        edge = eList(i)


        ' Start-Tag Edge
        ' ==============
        Dim attribEdge As String
        attribEdge = mkAttrib(id_, "E" & i)
        If (printKeys) Then
            attribEdge = attribEdge & mkAttrib(extRef_, edgeToKeyString(edge))
        End If
        openTag(Edge_, attribEdge)


        ' Edge-Struktur
        ' =============
        Dim sV As String = ""
        Dim sF As String = ""
        analyseIncidentVertices(edge, sV)
        analyseIncidentFaces(edge, sF)
        simpleTag(EdgeStructure_, sV & sF)


        ' Geometrie
        ' =========

        ' Start-Tag Edge-Geometrie
        ' ------------------------
        openTag(EdgeGeometry_)


        ' Konkrete Edge-Geometrie
        ' -----------------------
        Dim geoType As CurveTypeEnum
        geoType = Edge.GeometryType()

        Select Case geoType

            ' ----------------------------------
            ' Treten beim FAPS-Auto auf:
            ' ----------------------------------

            Case CurveTypeEnum.kLineSegmentCurve
                translateLineSegment(Edge)

            Case CurveTypeEnum.kCircleCurve
                translateCircle(Edge)

            Case CurveTypeEnum.kCircularArcCurve
                translateCircularArc(Edge)

            Case CurveTypeEnum.kEllipticalArcCurve
                translateEllipticalArc(Edge)

            Case CurveTypeEnum.kBSplineCurve
                translateBSplineCurve(Edge)

            Case CurveTypeEnum.kEllipseFullCurve
                translateEllipse(Edge)

                ' --------------------------------------------------------------------------
                ' Treten beim FAPS-Auto nicht auf, werden aber später hier auch abgehandelt
                ' --------------------------------------------------------------------------

            Case CurveTypeEnum.kLineCurve
                translateLine(Edge)

            Case CurveTypeEnum.kPolylineCurve
                translatePolyline(Edge)

            Case Else
                translateUnknownCurve(Edge)

        End Select


        ' End-Tag Edge-Geometrie
        ' ----------------------
        closeTag(EdgeGeometry_)


        ' End-Tag Edge
        ' ============
        closeTag(Edge_)

    End Sub


    ' Line-Segment wird als Line aufgefaßt und als solche nach XML übertragen

    Private Sub translateLineSegment(ByRef Edge As Edge)

        ' Statistik
        incrt(numLineSegments)

        ' Start-Tag
        openTag(Line_)

        ' Geometrie-Tags
        ' --------------
        Dim lS As lineSegment
        lS = Edge.geometry

        ' Startpunkt
        Dim startPoint As point
        startPoint = lS.startPoint
        Dim arr(2) As Double
        startPoint.GetPointData(arr)
        convPoint3d(arr)
        simpleTag(StartPoint_, Ax.point3dToS(arr))

        ' Endpunkt
        Dim endPoint As point
        endPoint = lS.endPoint
        endPoint.GetPointData(arr)
        convPoint3d(arr)
        simpleTag(EndPoint_, Ax.point3dToS(arr))

        ' End-Tag
        closeTag(Line_)

    End Sub

    Private Sub translateCircle(ByRef Edge As Edge)

        ' Statistik
        incrt(numCircles)

        ' Kreisgeometrie auslesen: cir
        Dim cir As Inventor.Circle
        cir = Edge.geometry

        ' Start-Tag
        Dim r As Double
        r = cir.radius
        r = convLength(r)
        openTag(Circle_, mkAttrib(radius_, Ax.dToS(r)))

        ' Mittelpunkt
        Dim c As point
        c = cir.center
        Dim arr(2) As Double
        c.GetPointData(arr)
        convPoint3d(arr)
        simpleTag(CenterPoint_, Ax.point3dToS(arr))

        ' Normalen-Vector
        Dim normal As vector
        normal = cir.normal.AsVector
        normal.GetVectorData(arr)
        normalize(arr)
        simpleTag(PlaneNormal_, Ax.point3dToS(arr))

        ' End-Tag
        closeTag(Circle_)
    End Sub

    Private Sub translateEllipse(ByRef Edge As Edge)

        ' Statistik
        incrt(numEllipses)

        ' Ellipsengeometrie auslesen
        ' --------------------------
        Dim ellips As Inventor.EllipseFull
        ellips = Edge.geometry
        Dim center() As Double = New Double() {}
        Dim axisVector() As Double = New Double() {}
        Dim majorAxis() As Double = New Double() {}
        Dim minorMajorRatio As Double = 0
        ellips.GetEllipseFullData(center, axisVector, majorAxis, minorMajorRatio)

        Dim majorRadius As Double = 0
        Dim minorRadius As Double = 0

        majorRadius = euclideanLength(majorAxis)
        minorRadius = majorRadius * minorMajorRatio
        majorRadius = convLength(majorRadius)
        minorRadius = convLength(minorRadius)

        ' Start-Tag
        openTag(Ellipse_, mkAttrib(majorRadius_, Ax.dToS(majorRadius)) & mkAttrib(minorRadius_, Ax.dToS(minorRadius)))

        ' Mittelpunkt
        convPoint3d(center)
        simpleTag(CenterPoint_, Ax.point3dToS(center))

        ' Normalen-Vector
        Dim arr() As Double = New Double() {}

        Dim normal As vector
        normal = ellips.normal.AsVector
        normal.GetVectorData(arr)
        normalize(arr)
        simpleTag(PlaneNormal_, Ax.point3dToS(arr))

        ' Major-Axis
        normalize(majorAxis)
        simpleTag(MajorAxis_, Ax.point3dToS(majorAxis))

        ' End-Tag
        closeTag(Ellipse_)

    End Sub

    ' Line kommt in unseren Beispielen nicht vor. Ergo Sub=dummy

    Private Sub translateLine(ByRef Edge As Edge)

        ' Statistik
        incrt(numLines)

        openTag(Line_)
        closeTag(Line_)

    End Sub

    ' Polyline kommt in unseren Beispielen nicht vor. Ergo Sub=dummy

    Private Sub translatePolyline(ByRef Edge As Edge)

        ' Statistik
        incrt(numPolylines)

        openTag(Polyline_)
        closeTag(Polyline_)

    End Sub

    Private Sub translateUnknownCurve(ByRef Edge As Edge)

        ' Statistik
        incrt(numUnknownCurves)

        openTag(UnknownCurve_)
        closeTag(UnknownCurve_)
    End Sub

    Private Sub translateCircularArc(ByRef Edge As Edge)

        ' Statistik
        incrt(numCircularArcs)

        ' Geometrie auslesen
        ' ------------------
        Dim arc As Arc3d
        arc = Edge.geometry

        ' Radius
        Dim r As Double
        r = arc.radius
        r = convLength(r)

        ' SweepAngle
        Dim arcus As Double
        arcus = arc.sweepAngle
        arcus = convAngle(arcus)

        ' Start-Tag
        Dim attribR, attribA As String
        attribR = mkAttrib(radius_, Ax.dToS(r))
        attribA = mkAttrib(sweepAngle_, Ax.dToS(arcus))
        openTag(CircularArc_, attribR & attribA)


        ' Geometrie-Tags
        ' --------------
        Dim p As point
        Dim arr() As Double = New Double() {}
        Dim err As Boolean
        err = False

        ' Mittelpunkt
        p = arc.center
        pointToPoint3d(p, arr)
        convPoint3d(arr)
        simpleTag(CenterPoint_, Ax.point3dToS(arr))

        ' Orthonormal-Vector
        Dim normal As vector
        normal = arc.normal.AsVector
        normal.GetVectorData(arr)
        convPoint3d(arr)
        normalize(arr)
        simpleTag(PlaneNormal_, Ax.point3dToS(arr))

        ' Startpunkt
        p = arc.startPoint
        pointToPoint3d(p, arr)
        convPoint3d(arr)
        simpleTag(StartPoint_, Ax.point3dToS(arr))

        ' Endpunkt
        p = arc.endPoint
        pointToPoint3d(p, arr)
        convPoint3d(arr)
        simpleTag(EndPoint_, Ax.point3dToS(arr))

        ' End-Tag
        closeTag(CircularArc_)

    End Sub

    Private Sub translateEllipticalArc(ByRef Edge As Edge)

        ' Statistik
        ' ---------
        incrt(numEllipticalArcs)

        ' Geometrie auslesen
        ' ------------------
        Dim elarc As EllipticalArc
        elarc = Edge.geometry

        ' Radien und Winkel
        Dim majorRadius, minorRadius, sweepAngle As Double
        majorRadius = elarc.majorRadius
        minorRadius = elarc.minorRadius
        sweepAngle = elarc.sweepAngle
        majorRadius = convLength(majorRadius)
        minorRadius = convLength(minorRadius)
        sweepAngle = convAngle(sweepAngle)

        ' Start-Tag
        ' ---------
        Dim attribMAR, attribMIR, attribANG As String
        attribMAR = mkAttrib(majorRadius_, Ax.dToS(majorRadius))
        attribMIR = mkAttrib(minorRadius_, Ax.dToS(minorRadius))
        attribANG = mkAttrib(sweepAngle_, Ax.dToS(sweepAngle))
        openTag(EllipticalArc_, attribMAR & attribMIR & attribANG)


        ' Geometrie-Tags
        ' --------------

        ' Mittelpunkt
        Dim p As point
        p = elarc.center
        Dim centerPoint() As Double = New Double() {}
        pointToPoint3d(p, centerPoint)
        convPoint3d(centerPoint)
        simpleTag(CenterPoint_, Ax.point3dToS(centerPoint))

        ' MajorAxis
        Dim majorAxis() As Double = New Double() {}
        elarc.majorAxis.AsVector.GetVectorData(majorAxis)
        Ax.normalize(majorAxis)
        simpleTag(MajorAxis_, Ax.point3dToS(majorAxis))

        ' MinorAxis
        Dim minorAxis() As Double = New Double() {}
        elarc.minorAxis.AsVector.GetVectorData(minorAxis)

        ' PlaneNormal
        Dim planeNormal() As Double = New Double() {}
        Ax.vectorProduct(majorAxis, minorAxis, planeNormal)
        Ax.normalize(planeNormal)
        simpleTag(PlaneNormal_, Ax.point3dToS(planeNormal))

        ' Startpunkt
        Dim startPoint As point
        startPoint = elarc.startPoint
        Dim sP() As Double = New Double() {}
        pointToPoint3d(startPoint, sP)
        convPoint3d(sP)
        simpleTag(StartPoint_, Ax.point3dToS(sP))

        ' Endpunkt
        Dim endPoint As point
        endPoint = elarc.endPoint
        Dim eP() As Double = New Double() {}
        pointToPoint3d(endPoint, eP)
        convPoint3d(eP)
        simpleTag(EndPoint_, Ax.point3dToS(eP))

        ' End-Tag
        ' -------
        closeTag(EllipticalArc_)

    End Sub

    Private Sub translateBSplineCurve(ByRef Edge As Edge)

        ' Geometrie auslesen
        ' ==================
        Dim spline As BSplineCurve
        spline = Edge.geometry

        Dim order As Integer
        Dim numPoles As Long
        Dim numKnots As Long
        Dim isRational As Boolean
        Dim isPeriodic As Boolean  ' wird nicht übertragen
        Dim isClosed As Boolean
        Dim isPlanar As Boolean

        Dim planeVector() As Double = New Double() {}

        spline.GetBSplineInfo(order, numPoles, numKnots, isRational, isPeriodic, isClosed, isPlanar, planeVector)

        Dim poles() As Double = New Double() {}
        Dim Knots() As Double = New Double() {}
        Dim Weights() As Double = New Double() {}    ' Weights werden keine geliefert.
        spline.GetBSplineData(poles, Knots, Weights)


        ' Statistik
        ' =========
        incrt(numBSplineCurves)
        incrt(numBSplineCurvesOfOrder(order))


        ' Start-Tag
        ' =========
        If (order < 1 Or 4 < order) Then
            MidMsgBoxInformation("translateBSplineCurve: Order of B-spline curve is out of range")
        End If
        Dim attribOrd, attribRat, attribClo, attribPlan As String
        attribOrd = mkAttrib(order_, order)
        attribRat = mkAttrib(rational_, Ax.bToS(isRational))
        If (isClosed) Then
            attribClo = mkAttrib(closed_, Ax.bToS(isClosed))
        Else
            attribClo = ""
        End If
        If (isPlanar) Then
            attribPlan = mkAttrib(planar_, Ax.bToS(isPlanar))
        Else
            attribPlan = ""
        End If
        openTag(BSplineCurve_, attribOrd & attribRat & attribClo & attribPlan)


        ' Geometrie-Tags
        ' ==============

        ' Kontroll-Punkte
        ' ---------------

        ' Start-Tag ControlPoints
        Dim attribN As String
        attribN = mkAttrib(n_, numPoles)
        openTag(CurveControlPoints_, attribN)

        ' Die einzelnen Kontroll-Punkte ausgeben
        Dim iP As Integer ' Index für das Feld Poles
        iP = 0
        Dim i As Integer  ' Index für den Durchlauf durch die Pol-Punkte (Kontrollpunkte)
        Dim attribI As String
        Dim arr(2) As Double

        For i = 1 To numPoles
            Ax.arrToPoint3d(poles, iP, arr)
            convPoint3d(arr)
            simpleTag(CurveControlPoint_, mkAttrib("i", i) & Ax.point3dToS(arr))
        Next i

        ' End-Tag ControlPoints
        closeTag(CurveControlPoints_)



        ' Knots
        ' -----

        ' Start-Tag der Knots
        openTag(KnotsOfCurve_, mkAttrib(numKnotsOfCurve_, numKnots))

        ' Die einzelnen Knots ausgeben
        For i = 1 To numKnots
            attribI = mkAttrib(indexOfKnotsOfCurve_, i)
            Dim attribKnot As String
            attribKnot = mkAttrib(val_, Ax.dToS(Knots(i - 1)))
            simpleTag(Knot_, attribI & attribKnot)
        Next i

        ' End-Tag der Knots
        closeTag(Knots_)


        ' Plane-Vektor
        ' ------------
        If isPlanar Then
            simpleTag(PlaneNormal_, Ax.point3dToS(planeVector))
        End If


        ' End-Tag
        ' =======
        closeTag(BSplineCurve_)

    End Sub

    Private Sub analyseIncidentVertices(ByRef Edge As Edge, ByRef s As String)
        Dim v As Vertex
        Dim key As String
        Dim n As Integer

        ' Start-Knoten
        ' ============
        v = Edge.startVertex
        key = vertexToKeyString(v)
        n = getNumberOfVertex(key)
        If (n >= 1) Then
            s = " startVertex=""V" & n & """"
        Else
            s = " startVertex=""ERROR"""
        End If

        ' End-Knoten
        ' ==========
        v = Edge.StopVertex
        key = vertexToKeyString(v)
        n = getNumberOfVertex(key)
        If (n >= 1) Then
            s = s & " endVertex=""V" & n & """"
        Else
            s = s & " endVertex=""ERROR"""
        End If
    End Sub

    Private Sub analyseIncidentFaces(ByRef Edge As Edge, ByRef sF As String)
        Dim Faces As Faces
        Dim a As Integer
        Dim f As Face
        Faces = Edge.Faces
        Dim key As String
        Dim n As Integer

        a = Faces.Count

        If (a < 1) Then
            sF = " face1=""ERROR"""
        Else
            f = Faces(1)
            key = faceToKeyString(f)
            n = getNumberOfFace(key)
            If (n >= 1) Then
                sF = " face1=""F" & n & """"
            Else
                sF = " face1=""ERROR"""
            End If
        End If

        If (a < 2) Then
            sF = sF & " face2=""ERROR"""
        Else
            f = Faces(2)
            key = faceToKeyString(f)
            n = getNumberOfFace(key)
            If (n >= 1) Then
                sF = sF & " face2=""F" & n & """"
            Else
                sF = sF & " face2=""ERROR"""
            End If
        End If

    End Sub

End Module



