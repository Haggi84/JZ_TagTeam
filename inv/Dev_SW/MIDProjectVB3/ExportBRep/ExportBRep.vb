Option Explicit On

Imports System.Xml
Imports System.Text
Imports Inventor
Imports System.Windows.Forms
Imports System.Math

Public Class ExportBRep

    Private oXMLWriter As XmlTextWriter

    Private MIDAddin As Inventor.Application

    Public oKeyContextNumber As Long
    Private oRefKeyManager As ReferenceKeyManager

    ' Edges
    Private numEdges As Integer

    Private numLineSegments As Integer
    Private numCircles As Integer
    Private numCircularArcs As Integer
    Private numEllipticalArcs As Integer
    Private numBSplineCurves As Integer
    Private numBSplineCurvesOfOrder(4) As Integer
    Private numEllipses As Integer
    Private numLines As Integer
    Private numPolylines As Integer
    Private numUnknownCurves As Integer

    ' Surfaces
    Private numPlanes As Integer
    Private numCylinders As Integer
    Private numCones As Integer
    Private numTori As Integer
    Private numSpheres As Integer
    Private numBSplineSurfaces As Integer
    Private numBSplineSurfacesOfOrder(4, 4) As Integer
    Private numEllipticalCones As Integer
    Private numEllipticalCylinders As Integer
    Private numUnknownSurfaces As Integer

    ' Constructor
    Public Sub New(ByRef MIDAddin As Inventor.Application)

        MyBase.New()

        Me.MIDAddin = MIDAddin

    End Sub

    Public Sub ReadBRepAI(ByRef exportData() As String)

        ' Provide lists of vertices, edges and faces
        Dim oVertices As Vertices
        Dim oEdges As Edges
        Dim oFaces As Faces

        ' Convert maximum decimal places to integer
        Dim decLen As Integer = Convert.ToInt32(exportData(3))
        Dim decAng As Integer = Convert.ToInt32(exportData(1))

        ' Obtain the faces, edges and vertices of the BRep
        If Not (detObjectLists(oVertices, oEdges, oFaces)) Then
            Exit Sub
        End If

        ' Generate new XML-document
        Dim oEnc As Encoding = New UTF8Encoding()
        oXMLWriter = New XmlTextWriter("C:\Users\Paul\Documents\export1.xml", oEnc)
        oXMLWriter.Formatting = Formatting.Indented

        oXMLWriter.WriteStartDocument()
        oXMLWriter.WriteStartElement("BRep")

        oXMLWriter.WriteAttributeString("denotion", "FILENAME")
        oXMLWriter.WriteAttributeString("cadSystem", "Autodesk Inventor Professional 2014")
        oXMLWriter.WriteAttributeString("unitOfLength", exportData(2))
        oXMLWriter.WriteAttributeString("precisionOfLength", exportData(3))
        oXMLWriter.WriteAttributeString("unitOfAngles", exportData(0))
        oXMLWriter.WriteAttributeString("precisionOfAngles", exportData(1))
        oXMLWriter.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance")
        oXMLWriter.WriteAttributeString("xsi:noNamespaceSchemaLocation", "BrepMID.xsd")

        '#################################################################################################
        ' TRANSLATE VERTICES
        '#################################################################################################







        '################
        'TODO
        '#############
        ' Round all values beneath







        oXMLWriter.WriteStartElement("Vertices")

        For i As Integer = 1 To oVertices.Count()
            oXMLWriter.WriteStartElement("Vertex")
            oXMLWriter.WriteAttributeString("id", "V" & i)

            Dim oVertex As Vertex = oVertices.Item(i)
            Dim point(2) As Double
            oVertex.GetPoint(point)

            oXMLWriter.WriteStartElement("Location")
            oXMLWriter.WriteAttributeString("x", Round(point(0), decLen))
            oXMLWriter.WriteAttributeString("y", Round(point(1), decLen))
            oXMLWriter.WriteAttributeString("z", Round(point(2), decLen))
            oXMLWriter.WriteEndElement() ' Location

            oXMLWriter.WriteEndElement() ' Vertex
        Next

        oXMLWriter.WriteEndElement() ' Vertices

        '################################################################################################
        ' TRANSLATE EDGES
        '################################################################################################

        oXMLWriter.WriteStartElement("Edges")

        For i As Integer = 1 To oEdges.Count()

            oXMLWriter.WriteStartElement("Edge")
            oXMLWriter.WriteAttributeString("id", "E" & i)

            ' Edge structure
            oXMLWriter.WriteStartElement("EdgeStructure")
            oXMLWriter.WriteAttributeString("startVertex", "V1")
            oXMLWriter.WriteAttributeString("endVertex", "V2")
            oXMLWriter.WriteAttributeString("face1", "F1")
            oXMLWriter.WriteAttributeString("face2", "F2")
            oXMLWriter.WriteEndElement() 'EdgeStructure

            oXMLWriter.WriteStartElement("EdgeGeometry")


            Dim oEdge As Edge = oEdges.Item(i)
            AddEdgeToStats(oEdge)
            Dim geoType As CurveTypeEnum = oEdge.GeometryType()

            Select Case geoType

                ' ----------------------------------
                ' Treten beim FAPS-Auto auf:
                ' ----------------------------------
                Case CurveTypeEnum.kLineSegmentCurve
                    'translateLineSegment(oEdge)
                    oXMLWriter.WriteStartElement("Line")

                    Dim oLineSeg As LineSegment = oEdge.Geometry
                    oXMLWriter.WriteStartElement("StartPoint")
                    oXMLWriter.WriteAttributeString("x", oLineSeg.StartPoint.X.ToString)
                    oXMLWriter.WriteAttributeString("y", oLineSeg.StartPoint.Y.ToString)
                    oXMLWriter.WriteAttributeString("z", oLineSeg.StartPoint.Z.ToString)
                    oXMLWriter.WriteEndElement() ' StartPoint

                    oXMLWriter.WriteStartElement("EndPoint")
                    oXMLWriter.WriteAttributeString("x", oLineSeg.EndPoint.X.ToString)
                    oXMLWriter.WriteAttributeString("y", oLineSeg.EndPoint.Y.ToString)
                    oXMLWriter.WriteAttributeString("z", oLineSeg.EndPoint.Z.ToString)
                    oXMLWriter.WriteEndElement() ' EndPoint

                    oXMLWriter.WriteEndElement() ' Line

                Case CurveTypeEnum.kCircleCurve
                    'translateCircle(oEdge)
                    Dim oCircle As Inventor.Circle = oEdge.Geometry
                    oXMLWriter.WriteStartElement("Circle")
                    oXMLWriter.WriteAttributeString("radius", oCircle.Radius.ToString)

                    oXMLWriter.WriteStartElement("CenterPoint")
                    oXMLWriter.WriteAttributeString("x", oCircle.Center.X.ToString)
                    oXMLWriter.WriteAttributeString("y", oCircle.Center.Y.ToString)
                    oXMLWriter.WriteAttributeString("z", oCircle.Center.Z.ToString)
                    oXMLWriter.WriteEndElement() ' CenterPoint

                    oXMLWriter.WriteStartElement("PlaneNormal")
                    oXMLWriter.WriteAttributeString("x", oCircle.Normal.X.ToString)
                    oXMLWriter.WriteAttributeString("y", oCircle.Normal.Y.ToString)
                    oXMLWriter.WriteAttributeString("z", oCircle.Normal.Z.ToString)
                    oXMLWriter.WriteEndElement() ' CenterPoint

                    oXMLWriter.WriteEndElement() ' Circle

                Case CurveTypeEnum.kCircularArcCurve
                    'translateCircularArc(oEdge)

                    Dim oArc As Arc3d = oEdge.Geometry
                    oXMLWriter.WriteStartElement("CircularArc")
                    oXMLWriter.WriteAttributeString("radius", oArc.Radius.ToString)
                    oXMLWriter.WriteAttributeString("sweepAngle", oArc.SweepAngle.ToString)

                    oXMLWriter.WriteStartElement("StartPoint")
                    oXMLWriter.WriteAttributeString("x", oArc.StartPoint.X.ToString)
                    oXMLWriter.WriteAttributeString("y", oArc.StartPoint.Y.ToString)
                    oXMLWriter.WriteAttributeString("z", oArc.StartPoint.Z.ToString)
                    oXMLWriter.WriteEndElement() ' StartPoint

                    oXMLWriter.WriteStartElement("CenterPoint")
                    oXMLWriter.WriteAttributeString("x", oArc.Center.X.ToString)
                    oXMLWriter.WriteAttributeString("y", oArc.Center.Y.ToString)
                    oXMLWriter.WriteAttributeString("z", oArc.Center.Z.ToString)
                    oXMLWriter.WriteEndElement() ' CenterPoint

                    oXMLWriter.WriteStartElement("EndPoint")
                    oXMLWriter.WriteAttributeString("x", oArc.EndPoint.X.ToString)
                    oXMLWriter.WriteAttributeString("y", oArc.EndPoint.Y.ToString)
                    oXMLWriter.WriteAttributeString("z", oArc.EndPoint.Z.ToString)
                    oXMLWriter.WriteEndElement() ' EndPoint

                    oXMLWriter.WriteStartElement("PlaneNormal")
                    oXMLWriter.WriteAttributeString("x", oArc.Normal.X.ToString)
                    oXMLWriter.WriteAttributeString("y", oArc.Normal.Y.ToString)
                    oXMLWriter.WriteAttributeString("z", oArc.Normal.Z.ToString)
                    oXMLWriter.WriteEndElement() ' PlaneNormal

                    oXMLWriter.WriteEndElement() 'CircularArc

                Case CurveTypeEnum.kEllipticalArcCurve
                    'translateEllipticalArc(oEdge)
                    Dim oEArc As EllipticalArc = oEdge.Geometry

                    oXMLWriter.WriteStartElement("EllipticalArc")
                    oXMLWriter.WriteAttributeString("majorRadius", oEArc.MajorRadius.ToString)
                    oXMLWriter.WriteAttributeString("minorRadius", oEArc.MinorRadius.ToString)
                    oXMLWriter.WriteAttributeString("sweepAngle", oEArc.SweepAngle.ToString)

                    oXMLWriter.WriteStartElement("CenterPoint")
                    oXMLWriter.WriteAttributeString("x", oEArc.Center.X.ToString)
                    oXMLWriter.WriteAttributeString("y", oEArc.Center.Y.ToString)
                    oXMLWriter.WriteAttributeString("z", oEArc.Center.Z.ToString)
                    oXMLWriter.WriteEndElement()

                    Dim oNormal As UnitVector = oEArc.MinorAxis.CrossProduct(oEArc.MajorAxis)
                    oXMLWriter.WriteStartElement("PlaneNormal")
                    oXMLWriter.WriteAttributeString("x", oNormal.X.ToString)
                    oXMLWriter.WriteAttributeString("y", oNormal.Y.ToString)
                    oXMLWriter.WriteAttributeString("z", oNormal.Z.ToString)
                    oXMLWriter.WriteEndElement()

                    oXMLWriter.WriteStartElement("MajorAxis")
                    oXMLWriter.WriteAttributeString("x", oEArc.MajorAxis.X.ToString)
                    oXMLWriter.WriteAttributeString("y", oEArc.MajorAxis.Y.ToString)
                    oXMLWriter.WriteAttributeString("z", oEArc.MajorAxis.Z.ToString)
                    oXMLWriter.WriteEndElement()

                    oXMLWriter.WriteStartElement("StartPoint")
                    oXMLWriter.WriteAttributeString("x", oEArc.StartPoint.X.ToString)
                    oXMLWriter.WriteAttributeString("y", oEArc.StartPoint.Y.ToString)
                    oXMLWriter.WriteAttributeString("z", oEArc.StartPoint.Z.ToString)
                    oXMLWriter.WriteEndElement()

                    oXMLWriter.WriteStartElement("EndPoint")
                    oXMLWriter.WriteAttributeString("x", oEArc.EndPoint.X.ToString)
                    oXMLWriter.WriteAttributeString("y", oEArc.EndPoint.Y.ToString)
                    oXMLWriter.WriteAttributeString("z", oEArc.EndPoint.Z.ToString)
                    oXMLWriter.WriteEndElement()

                    oXMLWriter.WriteEndElement() ' EllipticalArc

                Case CurveTypeEnum.kBSplineCurve
                    'translateBSplineCurve(oEdge)
                    Dim oSpline As BSplineCurve = oEdge.Geometry

                    Dim order, numPoles, numKnots As Long
                    Dim isRational, isPeriodic, isClosed, isPlanar As Boolean
                    Dim planeVector(2) As Double
                    'Dim isPeriodic As Boolean  ' wird nicht übertragen


                    oSpline.GetBSplineInfo(order, numPoles, numKnots, isRational, isPeriodic, isClosed, isPlanar, planeVector)
                    Dim poles(numPoles) As Double
                    Dim knots(numKnots) As Double
                    Dim weights(numPoles) As Double    ' Weights werden keine geliefert.
                    oSpline.GetBSplineData(poles, knots, weights)

                    If (order < 1 Or 4 < order) Then
                        MsgBox("Order of B-spline curve is out of range")
                        'Msg.warning("Order of B-spline curve is out of range")
                    End If

                    oXMLWriter.WriteStartElement("BSplineCurve")
                    oXMLWriter.WriteAttributeString("order", order)
                    oXMLWriter.WriteAttributeString("rational", isRational)
                    If (isClosed) Then
                        oXMLWriter.WriteAttributeString("closed", isClosed)
                    End If
                    If (isPlanar) Then
                        oXMLWriter.WriteAttributeString("planar", isPlanar)
                    End If

                    ' ControlPoints
                    oXMLWriter.WriteStartElement("CurveControlPoints")
                    oXMLWriter.WriteAttributeString("n", numPoles)
                    For j As Long = 1 To numPoles
                        oXMLWriter.WriteStartElement("CurveControlPoint")
                        oXMLWriter.WriteAttributeString("i", j)
                        oXMLWriter.WriteAttributeString("x", oSpline.PoleAtIndex(j).X.ToString)
                        oXMLWriter.WriteAttributeString("y", oSpline.PoleAtIndex(j).Y.ToString)
                        oXMLWriter.WriteAttributeString("z", oSpline.PoleAtIndex(j).Z.ToString)
                        oXMLWriter.WriteEndElement() ' CurveControlPoint
                    Next
                    oXMLWriter.WriteEndElement() ' CurveControlPoints

                    ' Knots
                    oXMLWriter.WriteStartElement("Knots")
                    oXMLWriter.WriteAttributeString("n", numKnots)

                    For j As Integer = 1 To numKnots
                        oXMLWriter.WriteStartElement("IndexedReal")
                        oXMLWriter.WriteAttributeString("val", knots(j - 1))
                        oXMLWriter.WriteAttributeString("i", j)
                        oXMLWriter.WriteEndElement() ' IndexedReal
                    Next

                    oXMLWriter.WriteEndElement() ' Knots 
                    oXMLWriter.WriteEndElement() ' BSplineCurve

                Case CurveTypeEnum.kEllipseFullCurve
                    'translateEllipse(oEdge)
                    Dim oEllipse As Inventor.EllipseFull = oEdge.Geometry

                    Dim center(2), axisVector(2), majorAxis(2), minorMajorRatio As Double
                    oEllipse.GetEllipseFullData(center, axisVector, majorAxis, minorMajorRatio)

                    oXMLWriter.WriteStartElement("Ellipse")
                    oXMLWriter.WriteAttributeString("majorRadius", majorAxis.Length)
                    oXMLWriter.WriteAttributeString("minorRadius", minorMajorRatio * majorAxis.Length)

                    oXMLWriter.WriteStartElement("CenterPoint")
                    oXMLWriter.WriteAttributeString("x", oEllipse.Center.X)
                    oXMLWriter.WriteAttributeString("y", oEllipse.Center.Y)
                    oXMLWriter.WriteAttributeString("z", oEllipse.Center.Z)
                    oXMLWriter.WriteEndElement()

                    oXMLWriter.WriteStartElement("PlaneNormal")
                    oXMLWriter.WriteAttributeString("x", oEllipse.Normal.X)
                    oXMLWriter.WriteAttributeString("y", oEllipse.Normal.Y)
                    oXMLWriter.WriteAttributeString("z", oEllipse.Normal.Z)
                    oXMLWriter.WriteEndElement()

                    oXMLWriter.WriteStartElement("MajorAxis")
                    oXMLWriter.WriteAttributeString("x", oEllipse.MajorAxisVector.X)
                    oXMLWriter.WriteAttributeString("y", oEllipse.MajorAxisVector.Y)
                    oXMLWriter.WriteAttributeString("z", oEllipse.MajorAxisVector.Z)
                    oXMLWriter.WriteEndElement()

                    oXMLWriter.WriteEndElement()

            End Select

            oXMLWriter.WriteEndElement() ' EdgeGeometry
            oXMLWriter.WriteEndElement() ' Edge

        Next

        oXMLWriter.WriteEndElement() ' Edges

        '##########################################################################################
        ' Translate faces
        '##########################################################################################

        oXMLWriter.WriteStartElement("Faces")

        For i As Integer = 1 To oFaces.Count()

            oXMLWriter.WriteStartElement("Face")
            oXMLWriter.WriteAttributeString("id", "F" & i)

            ' FACE STRUCTURE
            oXMLWriter.WriteStartElement("FaceStructure")
            oXMLWriter.WriteStartElement("Loops")

            Dim oLoops As EdgeLoops = oFaces(i).EdgeLoops() ' edge loops the face contains

            For j As Integer = 1 To oLoops.Count()

                oXMLWriter.WriteStartElement("Loop")
                If (oLoops.Count > 1) Then
                    oXMLWriter.WriteAttributeString("loopID", "L" & i & "_" & j)
                Else
                    oXMLWriter.WriteAttributeString("loopID", "L" & i)
                End If

                Dim oEdgeUses As EdgeUses = oLoops(j).EdgeUses()

                For k As Integer = 1 To oEdgeUses.Count()
                    oXMLWriter.WriteStartElement("EdgeUse")
                    oXMLWriter.WriteAttributeString("edgeID", "E" & k)
                    oXMLWriter.WriteEndElement() ' EdgeUse
                Next

                oXMLWriter.WriteEndElement() ' Loop
            Next

            oXMLWriter.WriteEndElement() ' Loops
            oXMLWriter.WriteEndElement() ' FaceStructure

            ' FACE GEOMETRY
            oXMLWriter.WriteStartElement("FaceGeometry")

            Dim oFace As Face = oFaces.Item(i)
            AddFaceToStats(oFace) ' for statistics
            Dim geoType As SurfaceTypeEnum = oFace.SurfaceType()

            Select Case geoType

                Case SurfaceTypeEnum.kPlaneSurface
                    'translatePlane(Face)
                    Dim oPlane As Plane = oFace.Geometry
                    oXMLWriter.WriteStartElement("Plane")

                    Dim rootPoint(2) As Double
                    Dim normalVector(2) As Double
                    oPlane.GetPlaneData(rootPoint, normalVector)

                    oXMLWriter.WriteStartElement("Location")
                    oXMLWriter.WriteAttributeString("x", rootPoint(0))
                    oXMLWriter.WriteAttributeString("y", rootPoint(1))
                    oXMLWriter.WriteAttributeString("z", rootPoint(2))
                    oXMLWriter.WriteEndElement() ' Location

                    oXMLWriter.WriteStartElement("PlaneNormal")
                    oXMLWriter.WriteAttributeString("x", normalVector(0))
                    oXMLWriter.WriteAttributeString("y", normalVector(1))
                    oXMLWriter.WriteAttributeString("z", normalVector(2))
                    oXMLWriter.WriteEndElement() 'PlaneNormal

                    oXMLWriter.WriteEndElement() ' Plane

                Case SurfaceTypeEnum.kConeSurface
                    'translateCone(oFace)
                    Dim oCone As Cone = oFace.Geometry

                    Dim basePoint(2), axisVector(2), radius, halfAngle As Double
                    Dim isExpanding As Boolean

                    oCone.GetConeData(basePoint, axisVector, radius, halfAngle, isExpanding)
                    ' Apex ermitteln
                    oXMLWriter.WriteStartElement("Cone")
                    oXMLWriter.WriteAttributeString("halfAngle", halfAngle)
                    oXMLWriter.WriteAttributeString("expanding", isExpanding)
                    oXMLWriter.WriteAttributeString("radius", radius)

                    Dim apex(2) As Double
                    GetApex(halfAngle, radius, axisVector, basePoint, apex)
                    oXMLWriter.WriteStartElement("Apex")
                    oXMLWriter.WriteAttributeString("x", apex(0))
                    oXMLWriter.WriteAttributeString("y", apex(1))
                    oXMLWriter.WriteAttributeString("z", apex(2))
                    oXMLWriter.WriteEndElement() ' Apex

                    oXMLWriter.WriteStartElement("AxisVector")
                    oXMLWriter.WriteAttributeString("x", axisVector(0))
                    oXMLWriter.WriteAttributeString("y", axisVector(1))
                    oXMLWriter.WriteAttributeString("z", axisVector(2))
                    oXMLWriter.WriteEndElement() ' AxisVector

                    oXMLWriter.WriteStartElement("BasePoint")
                    oXMLWriter.WriteAttributeString("x", basePoint(0))
                    oXMLWriter.WriteAttributeString("y", basePoint(1))
                    oXMLWriter.WriteAttributeString("z", basePoint(2))
                    oXMLWriter.WriteEndElement() ' BasePoint

                    oXMLWriter.WriteEndElement() ' Cone


                Case SurfaceTypeEnum.kCylinderSurface
                    'translateCylinder(Face)
                    Dim oCylinder As Cylinder = oFace.Geometry
                    Dim basePoint(2), axisVector(2), radius As Double
                    oCylinder.GetCylinderData(basePoint, axisVector, radius)

                    oXMLWriter.WriteStartElement("Cylinder")
                    oXMLWriter.WriteAttributeString("radius", radius)

                    oXMLWriter.WriteStartElement("BasePoint")
                    oXMLWriter.WriteAttributeString("x", basePoint(0))
                    oXMLWriter.WriteAttributeString("y", basePoint(1))
                    oXMLWriter.WriteAttributeString("z", basePoint(2))
                    oXMLWriter.WriteEndElement() ' BasePoint

                    oXMLWriter.WriteStartElement("AxisVector")
                    oXMLWriter.WriteAttributeString("x", axisVector(0))
                    oXMLWriter.WriteAttributeString("y", axisVector(1))
                    oXMLWriter.WriteAttributeString("z", axisVector(2))
                    oXMLWriter.WriteEndElement() ' AxisVector

                    oXMLWriter.WriteEndElement() ' Cylinder

                Case SurfaceTypeEnum.kTorusSurface
                    'translateTorus(Face)

                    ' Geometrie ermitteln
                    Dim oTorus As Torus = oFace.Geometry
                    Dim centerPoint(2), axisVector(2), majorRadius, minorRadius As Double
                    oTorus.GetTorusData(centerPoint, axisVector, majorRadius, minorRadius)

                    oXMLWriter.WriteStartElement("Torus")
                    oXMLWriter.WriteAttributeString("majorRadius", majorRadius)
                    oXMLWriter.WriteAttributeString("minorRadius", minorRadius)

                    oXMLWriter.WriteStartElement("CenterPoint")
                    oXMLWriter.WriteAttributeString("x", centerPoint(0))
                    oXMLWriter.WriteAttributeString("y", centerPoint(1))
                    oXMLWriter.WriteAttributeString("z", centerPoint(2))
                    oXMLWriter.WriteEndElement() ' CenterPoint

                    oXMLWriter.WriteStartElement("AxisVector")
                    oXMLWriter.WriteAttributeString("x", axisVector(0))
                    oXMLWriter.WriteAttributeString("y", axisVector(1))
                    oXMLWriter.WriteAttributeString("z", axisVector(2))
                    oXMLWriter.WriteEndElement() ' AxisVector

                    oXMLWriter.WriteEndElement() ' Torus


                Case SurfaceTypeEnum.kSphereSurface
                    'translateSphere(Face)

                    Dim oSphere As Sphere = oFace.Geometry
                    Dim centerPoint(2), radius As Double
                    oSphere.GetSphereData(centerPoint, radius)

                    oXMLWriter.WriteStartElement("Sphere")
                    oXMLWriter.WriteAttributeString("radius", radius)

                    oXMLWriter.WriteStartElement("CenterPoint")
                    oXMLWriter.WriteAttributeString("x", centerPoint(0))
                    oXMLWriter.WriteAttributeString("y", centerPoint(1))
                    oXMLWriter.WriteAttributeString("z", centerPoint(2))
                    oXMLWriter.WriteEndElement() ' CenterPoint

                    oXMLWriter.WriteEndElement() ' Sphere

                Case SurfaceTypeEnum.kBSplineSurface
                    'translateBSplineSurface(Face)
                    ' -------------------
                    Dim oSpline As BSplineSurface = oFace.Geometry
                    Dim order(1), numPoles(1), numKnots(1) As Integer
                    Dim isRational, isPeriodic(1), isClosed(1), isPlanar As Boolean
                    Dim planeVector(1) As Double

                    oSpline.GetBSplineInfo(order, numPoles, numKnots, isRational, isPeriodic, isClosed, isPlanar, planeVector)

                    If (order(0) < 1 Or 4 < order(0)) Then
                        MsgBox("Order of B-spline surface is out of range")
                    End If
                    If (order(1) < 1 Or 4 < order(1)) Then
                        MsgBox("Order of B-spline surface is out of range")
                    End If

                    oXMLWriter.WriteStartElement("BSplineSurface")

                    If (isClosed(0)) Then
                        oXMLWriter.WriteAttributeString("closedU", isClosed(0)) ' True

                    End If
                    If (isClosed(1)) Then
                        oXMLWriter.WriteAttributeString("closedV", isClosed(1)) ' True
                    End If

                    If (isPlanar) Then
                        oXMLWriter.WriteAttributeString("planar", isPlanar) ' True
                    End If

                    oXMLWriter.WriteAttributeString("orderU", order(0))
                    oXMLWriter.WriteAttributeString("orderV", order(1))
                    oXMLWriter.WriteAttributeString("rational", isRational)

                    Dim numPolesU As Integer = numPoles(0)
                    Dim numPolesV As Integer = numPoles(1)
                    Dim numKnotsU As Integer = numKnots(0)
                    Dim numKnotsV As Integer = numKnots(1)


                    Dim poles(1) As Double ' lenght changes after GetBSplineData automatically
                    Dim knotsU(numKnotsU) As Double
                    Dim knotsV(numKnotsV) As Double
                    Dim weights(1) As Double ' lenght changes after GetBSplineData automatically
                    oSpline.GetBSplineData(poles, knotsU, knotsV, weights)
                    Dim numWeigts As Integer = UBound(weights) ' find the highest subscript

                    oXMLWriter.WriteStartElement("SurfaceControlPoints")
                    oXMLWriter.WriteAttributeString("m", numPolesV)
                    oXMLWriter.WriteAttributeString("n", numPolesU)

                    ' Poles
                    For j As Integer = 1 To numPolesV
                        For k As Integer = 1 To numPolesU
                            oXMLWriter.WriteStartElement("SurfaceControlPoint")
                            oXMLWriter.WriteAttributeString("j", j)
                            oXMLWriter.WriteAttributeString("i", k)

                            oXMLWriter.WriteAttributeString("x", oSpline.PoleAtIndex(k, j).X)
                            oXMLWriter.WriteAttributeString("y", oSpline.PoleAtIndex(k, j).Y)
                            oXMLWriter.WriteAttributeString("z", oSpline.PoleAtIndex(k, j).Z)
                            oXMLWriter.WriteEndElement() ' SurfaceControlPoint
                        Next
                    Next

                    oXMLWriter.WriteEndElement() 'SurfaceControlPoints

                    ' Weights
                    oXMLWriter.WriteStartElement("SurfaceWeights")
                    oXMLWriter.WriteAttributeString("m", numPolesV)
                    oXMLWriter.WriteAttributeString("n", numPolesU)

                    Dim index As Integer = 0
                    For j As Integer = 1 To numPolesV
                        For k As Integer = 1 To numPolesU
                            oXMLWriter.WriteStartElement("DoubleIndexedReal")
                            oXMLWriter.WriteAttributeString("j", j)
                            oXMLWriter.WriteAttributeString("val", weights(index))
                            oXMLWriter.WriteAttributeString("i", k)
                            oXMLWriter.WriteEndElement() ' DoubleIndexedReal
                            index += 1
                        Next
                    Next

                    oXMLWriter.WriteEndElement() ' SurfaceWeights

                    ' KnotsU
                    oXMLWriter.WriteStartElement("KnotsU")
                    oXMLWriter.WriteAttributeString("n", numKnotsU)

                    For j As Integer = 1 To numKnotsU
                        oXMLWriter.WriteStartElement("IndexedReal")
                        oXMLWriter.WriteAttributeString("val", knotsU(j - 1))
                        oXMLWriter.WriteAttributeString("i", j)
                        oXMLWriter.WriteEndElement() 'IndexedReal
                    Next

                    oXMLWriter.WriteEndElement() ' KnotsU

                    ' KnotsV
                    oXMLWriter.WriteStartElement("KnotsV")
                    oXMLWriter.WriteAttributeString("n", numKnotsV)

                    For j As Integer = 1 To numKnotsV
                        oXMLWriter.WriteStartElement("IndexedReal")
                        oXMLWriter.WriteAttributeString("val", knotsV(j - 1))
                        oXMLWriter.WriteAttributeString("i", j)
                        oXMLWriter.WriteEndElement() 'IndexedReal
                    Next

                    oXMLWriter.WriteEndElement() ' KnotsV

                    ' Plane vector
                    If isPlanar Then
                        Dim planeNormal(2) As Double
                        Dim params(1) As Double : params(0) = 0.5 : params(1) = 0.5
                        oSpline.Evaluator.GetNormal(params, planeNormal)
                        oXMLWriter.WriteStartElement("PlaneNormal")
                        oXMLWriter.WriteAttributeString("x", planeNormal(0))
                        oXMLWriter.WriteAttributeString("y", planeNormal(1))
                        oXMLWriter.WriteAttributeString("z", planeNormal(2))
                        oXMLWriter.WriteEndElement() ' PlaneNormal
                    End If

                    oXMLWriter.WriteEndElement() ' BSplineSurface


                Case SurfaceTypeEnum.kEllipticalCylinderSurface
                    'translateEllipticalCylinder(oFace)

                    ' Geometrie ermitteln
                    Dim oECylinder As EllipticalCylinder = oFace.Geometry
                    Dim basePoint(2), axisVector(2), majorAxis(2), ratio As Double
                    oECylinder.GetEllipticalCylinderData(basePoint, axisVector, majorAxis, ratio)

                    oXMLWriter.WriteStartElement("EllipticalCylinder")
                    oXMLWriter.WriteAttributeString("minorMajorRation", ratio)

                    oXMLWriter.WriteStartElement("BasePoint")
                    oXMLWriter.WriteAttributeString("x", basePoint(0))
                    oXMLWriter.WriteAttributeString("y", basePoint(1))
                    oXMLWriter.WriteAttributeString("z", basePoint(2))
                    oXMLWriter.WriteEndElement() ' BasePoint

                    oXMLWriter.WriteStartElement("AxisVector")
                    oXMLWriter.WriteAttributeString("x", axisVector(0))
                    oXMLWriter.WriteAttributeString("y", axisVector(1))
                    oXMLWriter.WriteAttributeString("z", axisVector(2))
                    oXMLWriter.WriteEndElement() ' AxisVector

                    oXMLWriter.WriteStartElement("MajorAxis")
                    oXMLWriter.WriteAttributeString("x", majorAxis(0))
                    oXMLWriter.WriteAttributeString("y", majorAxis(1))
                    oXMLWriter.WriteAttributeString("z", majorAxis(2))
                    oXMLWriter.WriteEndElement() ' MajorAxis

                    oXMLWriter.WriteEndElement() ' EllipticalCylinder

                    '########################
                    ' Not relevant yet
                    '########################

                Case SurfaceTypeEnum.kEllipticalConeSurface

                    oXMLWriter.WriteStartElement("EllipticalCone")
                    oXMLWriter.WriteEndElement() ' EllipticalCone

                    MsgBox("EllipticalCone")

                Case Else
                    'translateUnknownSurface(oFace)

                    oXMLWriter.WriteStartElement("UnknownSurface")
                    oXMLWriter.WriteEndElement() ' UnknownSurface

                    ' MsgBox("UnknownSurface")
                    ' 

            End Select

            oXMLWriter.WriteEndElement() ' FaceGeometry

            oXMLWriter.WriteEndElement() ' Face

        Next

        oXMLWriter.WriteEndElement() ' Faces


        '##################################################
        ' Statistics
        '##################################################

        oXMLWriter.WriteStartElement("Statistics")

        ' Vertices
        oXMLWriter.WriteStartElement("VertexStatistics")
        oXMLWriter.WriteAttributeString("n", oVertices.Count())
        oXMLWriter.WriteEndElement() ' VertexStatistics

        ' Edges
        oXMLWriter.WriteStartElement("EdgeStatistics")
        oXMLWriter.WriteAttributeString("lineSegments", numLineSegments)
        oXMLWriter.WriteAttributeString("circles", numCircles)
        oXMLWriter.WriteAttributeString("circularArcs", numCircularArcs)
        oXMLWriter.WriteAttributeString("ellipticalArc", numEllipticalArcs)
        oXMLWriter.WriteAttributeString("bSplineCurves", numBSplineCurves)
        oXMLWriter.WriteAttributeString("ellipses", numEllipses)
        oXMLWriter.WriteAttributeString("lines", numLines)
        oXMLWriter.WriteAttributeString("polylines", numPolylines)
        oXMLWriter.WriteAttributeString("unknownCurves", numUnknownCurves)
        oXMLWriter.WriteAttributeString("n", oEdges.Count())

        oXMLWriter.WriteStartElement("BSplineCurveStatistics")
        oXMLWriter.WriteAttributeString("n", numBSplineCurves)
        oXMLWriter.WriteEndElement() ' BSplineCurveStatistics

        oXMLWriter.WriteEndElement() ' EdgeStatistics

        ' Faces
        oXMLWriter.WriteStartElement("FaceStatistics")
        oXMLWriter.WriteAttributeString("planes", numPlanes)
        oXMLWriter.WriteAttributeString("cylinders", numCylinders)
        oXMLWriter.WriteAttributeString("cones", numCones)
        oXMLWriter.WriteAttributeString("tori", numTori)
        oXMLWriter.WriteAttributeString("spheres", numSpheres)
        oXMLWriter.WriteAttributeString("bSplineSurfaces", numBSplineSurfaces)
        oXMLWriter.WriteAttributeString("ellipticalCones", numEllipticalCones)
        oXMLWriter.WriteAttributeString("unknownSurfaces", numUnknownSurfaces)
        oXMLWriter.WriteAttributeString("n", oFaces.Count())

        oXMLWriter.WriteStartElement("BSplineSurfaceStatistics")
        oXMLWriter.WriteAttributeString("n", numBSplineSurfaces)
        oXMLWriter.WriteEndElement() ' BSplineSurfaceStatistics

        'For i As Integer = 1 To 4
        '    For j As Integer = 1 To 4
        '        If (numBSplineSurfacesOfOrder(i, j) <> 0) Then
        '            oXMLWriter.WriteStartElement("OrderUV")
        '            oXMLWriter.WriteAttributeString("n", numBSplineSurfacesOfOrder(i, j))
        '            oXMLWriter.WriteAttributeString("ordU", i)
        '            oXMLWriter.WriteAttributeString("ordV", j)
        '            oXMLWriter.WriteEndElement() ' OrderUV

        '        End If
        '    Next
        'Next



        'oXMLWriter.WriteEndElement() ' BSplineSurfaceStatistics

        'openTag("BSplineSurfaceStatistics", mkAttrib("n", numBSplineSurfaces))

        'Dim i, j As Integer
        'incrt level
        'For i = 1 To 4
        '    For j = 1 To 4
        '        If (numBSplineSurfacesOfOrder(i, j) <> 0) Then
        '            simpleTag("OrderUV", mkAttrib("ordU", i) & mkAttrib("ordV", j) & mkAttrib("n", numBSplineSurfacesOfOrder(i, j)))
        '        End If
        '    Next j
        'Next i
        'decrt level

        'closeTag("BSplineSurfaceStatistics")



        oXMLWriter.WriteEndElement() ' FaceStatistics
        oXMLWriter.WriteEndElement() ' Statistics
        'End Faces

        oXMLWriter.WriteEndElement() ' BRep
        oXMLWriter.WriteEndDocument()

        oXMLWriter.Flush()
        oXMLWriter.Close()

        ' Feedback for the user
        MessageBox.Show("BRep successfully exported")

    End Sub

    '#################################
    ' Calculate apex of a cone
    '#################################

    Public Sub GetApex(ByRef halfAngle As Double, ByRef radius As Double, ByRef axisVector() As Double, ByRef BasePoint() As Double, ByRef Apex() As Double)
        Dim t As Double   ' Für Tangens
        Dim alpha As Double
        alpha = (halfAngle * Math.PI) / 180
        t = Math.Tan(alpha)
        Dim h As Double
        h = radius / t
        Dim p(2) As Double
        p(0) = h * axisVector(0) : p(1) = h * axisVector(1) : p(2) = h * axisVector(2)
        Apex(0) = p(0) + BasePoint(0) : Apex(1) = p(1) + BasePoint(1) : Apex(2) = p(2) + BasePoint(2)
    End Sub


    '######################################################################################
    ' Statistics
    '##########################################################################################

    Private Sub AddEdgeToStats(ByRef oEdge As Edge)

        Dim geoType As CurveTypeEnum = oEdge.GeometryType()
        Select Case geoType
            Case CurveTypeEnum.kBSplineCurve
                numBSplineCurves += 1
            Case CurveTypeEnum.kCircleCurve
                numCircles += 1
            Case CurveTypeEnum.kCircularArcCurve
                numCircularArcs += 1
            Case CurveTypeEnum.kEllipseFullCurve
                numEllipses += 1
            Case CurveTypeEnum.kEllipticalArcCurve
                numEllipticalArcs += 1
            Case CurveTypeEnum.kLineCurve
                numLines += 1
            Case CurveTypeEnum.kLineSegmentCurve
                numLineSegments += 1
            Case CurveTypeEnum.kPolylineCurve
                numPolylines += 1
            Case Else
                numUnknownCurves += 1

        End Select

    End Sub

    Private Sub AddFaceToStats(ByRef oFace As Face)

        Dim geoType As SurfaceTypeEnum = oFace.SurfaceType()
        Select Case geoType
            Case SurfaceTypeEnum.kBSplineSurface
                numBSplineSurfaces += 1

            Case SurfaceTypeEnum.kConeSurface
                numCones += 1
            Case SurfaceTypeEnum.kCylinderSurface
                numCylinders += 1
            Case SurfaceTypeEnum.kEllipticalConeSurface
                numEllipticalCones += 1
            Case SurfaceTypeEnum.kEllipticalCylinderSurface
                numEllipticalCylinders += 1
            Case SurfaceTypeEnum.kPlaneSurface
                numPlanes += 1
            Case SurfaceTypeEnum.kSphereSurface
                numSpheres += 1
            Case SurfaceTypeEnum.kTorusSurface
                numTori += 1
            Case Else
                numUnknownSurfaces += 1
        End Select

    End Sub


    Sub DockableWindow()
       
    End Sub


    '' Knoten
    '' ------
    'Public numVertices As Integer
    'Public xMin, xMax, yMin, yMax, zMin, zMax As Double

    '' Kanten
    '' ------
    'Public numEdges As Integer

    'Public numLineSegments As Integer
    'Public numCircles As Integer
    'Public numCircularArcs As Integer
    'Public numEllipticalArcs As Integer
    'Public numBSplineCurves As Integer
    'Public numBSplineCurvesOfOrder(4) As Integer
    'Public numEllipses As Integer
    'Public numLines As Integer
    'Public numPolylines As Integer
    'Public numUnknownCurves As Integer


    '' Facetten
    '' --------
    'Public numFaces As Integer

    'Public numPlanes As Integer
    'Public numCylinders As Integer
    'Public numCones As Integer
    'Public numTori As Integer
    'Public numSpheres As Integer
    'Public numBSplineSurfaces As Integer
    'Public numBSplineSurfacesOfOrder(4, 4) As Integer
    'Public numEllipticalCones As Integer
    'Public numEllipticalCylinders As Integer
    'Public numUnknownSurfaces As Integer


    'Public Sub InitStatistics()

    '    Dim i, j As Integer

    '    ' Knoten
    '    ' ------
    '    numVertices = 0

    '    ' Kanten
    '    ' ------
    '    numEdges = 0

    '    numLineSegments = 0
    '    numCircles = 0
    '    numCircularArcs = 0
    '    numEllipticalArcs = 0
    '    numBSplineCurves = 0
    '    numEllipses = 0
    '    numLines = 0
    '    numPolylines = 0
    '    numUnknownCurves = 0

    '    For i = 1 To 4
    '        numBSplineCurvesOfOrder(i) = 0
    '    Next i

    '    ' Facetten
    '    ' --------
    '    numFaces = 0

    '    numPlanes = 0
    '    numCylinders = 0
    '    numCones = 0
    '    numTori = 0
    '    numSpheres = 0
    '    numBSplineSurfaces = 0
    '    numEllipticalCones = 0
    '    numEllipticalCylinders = 0
    '    numUnknownSurfaces = 0

    '    For i = 1 To 4
    '        For j = 1 To 4
    '            numBSplineSurfacesOfOrder(i, j) = 0
    '        Next j
    '    Next i
    'End Sub

    'Public Sub writeStatistics()
    '    openTag("Statistics")

    '    ' Knoten
    '    simpleTag("VertexStatistics", mkAttrib("n", numVertices))

    '    ' Kanten
    '    writeEdgeStatistics()

    '    ' Facetten
    '    writeFaceStatistics()

    '    ' Abschluss Statistik
    '    closeTag("Statistics")

    '    Ax.switchToHourGlassCursor()       'Nicht löschen!
    'End Sub

    'Private Sub writeEdgeStatistics()

    '    ' Start-Tag
    '    ' ---------
    '    Dim attributes As String
    '    attributes = mkAttrib("n", numEdges) _
    '               & mkAttrib("lineSegments", numLineSegments) _
    '               & mkAttrib("circles", numCircles) _
    '               & mkAttrib("circularArcs", numCircularArcs) _
    '               & mkAttrib("ellipticalArcs", numEllipticalArcs) _
    '               & mkAttrib("bSplineCurves", numBSplineCurves) _
    '               & mkAttrib("ellipses", numEllipses) _
    '               & mkAttrib("lines", numLines) _
    '               & mkAttrib("polylines", numPolylines) _
    '               & mkAttrib("unknownCurves", numUnknownCurves)

    '    openTag("EdgeStatistics", attributes)

    '    ' Differenzierende Statistik zu B-Spline-Kurven
    '    ' ---------------------------------------------
    '    writeBSplineCurveStatistics()

    '    ' End-Tag
    '    ' -------
    '    closeTag("EdgeStatistics")

    'End Sub

    'Private Sub writeFaceStatistics()

    '    ' Kopf Facetten
    '    ' =============
    '    Dim attributes As String
    '    attributes = mkAttrib("n", numFaces) & mkAttrib("planes", numPlanes) & mkAttrib("cylinders", numCylinders) _
    '               & mkAttrib("cones", numCones) & mkAttrib("tori", numTori) & mkAttrib("spheres", numSpheres) _
    '               & mkAttrib("bSplineSurfaces", numBSplineSurfaces) & mkAttrib("ellipticalCones", numEllipticalCones) _
    '               & mkAttrib("ellipticalCylinders", numEllipticalCylinders) & mkAttrib("unknownSurfaces", numUnknownSurfaces)
    '    openTag("FaceStatistics", attributes)

    '    ' Spezielle Statistik zu verschiedenen B-Spline-Kurven
    '    ' ====================================================
    '    If numBSplineSurfaces <> 0 Then
    '        writeBSplineSurfaceStatistics()
    '    End If

    '    ' Abschuss Facetten
    '    ' =================
    '    closeTag("FaceStatistics")
    'End Sub

    'Private Sub writeBSplineCurveStatistics()
    '    If (numBSplineCurves = 0) Then
    '        Exit Sub
    '    End If

    '    ' Start-Tag
    '    ' ---------
    '    openTag("BSplineCurveStatistics", mkAttrib("n", numBSplineCurves))

    '    ' Inhalt
    '    ' ======
    '    Dim i As Integer
    '    For i = 1 To 4
    '        If (numBSplineCurvesOfOrder(i) <> 0) Then
    '            simpleTag("Order", mkAttrib("ord", i) & mkAttrib("n", numBSplineCurvesOfOrder(i)))
    '        End If
    '    Next i

    '    ' Abschluss
    '    ' =========
    '    closeTag("BSplineCurveStatistics")

    'End Sub

    'Private Sub writeBSplineSurfaceStatistics()

    '    openTag("BSplineSurfaceStatistics", mkAttrib("n", numBSplineSurfaces))

    '    Dim i, j As Integer
    '    incrt level
    '    For i = 1 To 4
    '        For j = 1 To 4
    '            If (numBSplineSurfacesOfOrder(i, j) <> 0) Then
    '                simpleTag("OrderUV", mkAttrib("ordU", i) & mkAttrib("ordV", j) & mkAttrib("n", numBSplineSurfacesOfOrder(i, j)))
    '            End If
    '        Next j
    '    Next i
    '    decrt level

    '    closeTag("BSplineSurfaceStatistics")
    'End Sub



    '#####################################
    ' Translate faces
    '##############################################

    ' *************
    ' * TransFace *
    ' *************

    ' Übertragung der Facetten
    ' ========================

    'Option Explicit
    'Option Private Module

    Public Sub translateFaces(ByRef Faces As Faces)
        'openTag(Faces_)

        ' Dim n, i As Integer
        ' n = Faces.Count
        'Statistics.numFaces = n
        For i As Integer = 1 To Faces.Count
            translateFace(Faces, i)
        Next

        'closeTag(Faces_)

        'Ax.switchToHourGlassCursor()       'Nicht löschen!
    End Sub

    Private Sub translateFace(ByRef Faces As Faces, ByVal i As Integer)

        Dim Face As Face = Faces.Item(i)

        ' Start-Tag
        Dim attribFace As String
        'attribFace = mkAttrib(id_, "F" & i)
        'If (printKeys) Then
        'attribFace = attribFace & mkAttrib(extRef_, objectToKeyString(Face))
        'End If
        'openTag(Face_, attribFace)

        ' Struktur der Facette
        'openTag(FaceStructure_)
        translateLoops(Face, i)
        'closeTag(FaceStructure_)

        ' Geometrie
        translateFaceGeometry(Face)

        ' End-Tag
        'closeTag(Face_)

    End Sub

    Private Sub translateLoops(ByRef Face As Face, ByVal i As Integer)
        'openTag(Loops_)

        ' Listen der Loops bereitstellen
        Dim Loops As EdgeLoops
        Loops = Face.EdgeLoops()
        Dim n, j As Integer
        'n = Loops.Count

        For j = 1 To Loops.Count
            translateLoop(Loops, i, n, j)
        Next j

        'closeTag(Loops_)
    End Sub

    Private Sub translateLoop(ByRef Loops As EdgeLoops, ByVal i As Integer, ByVal n As Integer, ByVal j As Integer)

        ' Start-Tag
        Dim idLoop As String
        If (n > 1) Then
            idLoop = "L" & i & "_" & j
        Else
            idLoop = "L" & i
        End If

        'openTag(Loop_, mkAttrib(loopID_, idLoop))

        ' Edge-Uses
        Dim lp As EdgeLoop = Loops(j)
        Dim listOfEdgeUses As EdgeUses = lp.EdgeUses()

        'Dim m, k As Integer
        For k = 1 To listOfEdgeUses.Count
            'translateEdgeUse(listOfEdgeUses, k)
        Next

        ' End-Tag
        'closeTag(Loop_)

    End Sub

    Private Sub translateFaceGeometry(ByRef Face As Face)

        ' Start-Tag
        ' =========
        'openTag(FaceGeometry_)

        Dim geoType As SurfaceTypeEnum
        geoType = Face.SurfaceType()


        Select Case geoType

            ' ----------------------------------
            ' Treten beim FAPS-Auto auf:
            ' ----------------------------------

            Case SurfaceTypeEnum.kPlaneSurface      ' Ebene
                translatePlane(Face)

                'Case kCylinderSurface   ' Zylinder
                '    translateCylinder(Face)

                'Case kConeSurface       ' Kegel
                '    translateCone(Face)

                'Case kTorusSurface      ' Torus
                '    translateTorus(Face)

                'Case kSphereSurface     ' Kugeloberfläche
                '    translateSphere(Face)

                'Case kBSplineSurface    ' B-Spline-Fläche
                '    translateBSplineSurface(Face)

                'Case kEllipticalCylinderSurface
                '    translateEllipticalCylinder(Face)


                ' --------------------------------------------------------------------------
                ' Treten beim FAPS-Auto nicht auf, werden aber später hier auch abgehandelt
                ' --------------------------------------------------------------------------

                'Case kEllipticalConeSurface
                '    translateEllipticalCone(Face)


                'Case Else
                '    translateUnknownSurface(Face)

        End Select


        ' End-Tag
        ' =======
        'closeTag(FaceGeometry_)
    End Sub

    Private Sub translatePlane(ByRef Face As Face)

        ' Statistik
        'incrt numPlanes

        ' Start-Tag
        'openTag(Plane_)

        ' Geometrie-Tags
        Dim Plane As Plane
        Dim rootPoint() As Double
        Dim normalVector() As Double
        Plane = Face.Geometry
        Plane.GetPlaneData(rootPoint, normalVector)
        'simpleTag(Location_, Ax.vToS(rootPoint))
        'simpleTag(PlaneNormal_, Ax.vToS(normalVector))

        ' End-Tag
        ' closeTag(Plane_)

    End Sub

    Private Sub translateCylinder(ByRef Face As Face)

        ' Statistik
        'incrt numCylinders

        ' Geometrie ermitteln
        Dim Cylinder As Cylinder
        Dim bP() As Double
        Dim aV() As Double
        Dim r As Double
        Cylinder = Face.Geometry
        Cylinder.GetCylinderData(bP, aV, r)

        ' Start-Tag
        'openTag(Cylinder_, mkAttrib(radius_, Ax.cToS(r)))

        ' Geometrie-Tags
        'simpleTag(BasePoint_, Ax.vToS(bP))
        'simpleTag(AxisVector_, Ax.vToS(aV))

        ' End-Tag
        ' closeTag(Cylinder_)

    End Sub

    '    Private Sub translateCone(ByRef Face As Face)

    '        ' Statistik
    '        ' ---------
    '        incrt numCones

    '        ' Geometrie ermitteln
    '        ' -------------------
    '        Dim Cone As Cone
    '        Dim bP() As Double   ' Base Point
    '        Dim aV() As Double   ' Axis Vector
    '        Dim r As Double      ' Radius
    '        Dim hA As Double     ' Half Angle
    '        Dim e As Boolean     ' Expanded (bedeutet was?)
    '        Cone = Face.geometry
    '        Cone.GetConeData(bP, aV, r, hA, e)
    '        ' Apex ermitteln
    '        Dim Apex(2) As Double ' Kegelspitze
    '        Ax.detApex(hA, r, aV, bP, Apex)
    '        ' Axis Vector umkehren
    '        Ax.minusVector(aV)

    '        ' Start-Tag
    '        ' ---------
    '        Dim attribHA, attribE As String
    '        attribHA = mkAttrib(halfAngle_, Ax.aToS(hA))
    '        attribE = mkAttrib(expanding_, Ax.bToS(e))
    '        openTag(Cone_, attribHA & attribE)

    '        ' Geometrie-Tags
    '        ' --------------
    '        simpleTag(AxisVector_, Ax.vToS(aV))
    '        simpleTag(Apex_, Ax.vToS(Apex))

    '        ' End-Tag
    '        ' -------
    '        closeTag(Cone_)

    '    End Sub

    'Private Sub translateBSplineSurface(ByRef Face As Face)

    '    ' Statistik
    '    ' ---------
    '    incrt numBSplineSurfaces


    '    ' Geometrie ermitteln
    '    ' -------------------
    '    Dim spline As BSplineSurface
    '    spline = Face.geometry
    '    Dim order(1) As Long
    '    Dim numPoles(1) As Long
    '    Dim numKnots(1) As Long
    '    Dim isRational As Boolean
    '    Dim isPeriodic() As Boolean
    '    Dim isClosed() As Boolean
    '    Dim isPlanar As Boolean
    '    Dim planeVector(1) As Double

    '    spline.GetBSplineInfo(order, numPoles, numKnots, isRational, isPeriodic, isClosed, isPlanar, planeVector)

    '    ' Geometrie testen
    '    ' ----------------
    '    If (order(0) < 1 Or 4 < order(0)) Then
    '        Msg.warning("Order of B-spline surface is out of range")
    '    End If
    '    If (order(1) < 1 Or 4 < order(1)) Then
    '        Msg.warning("Order of B-spline surface is out of range")
    '    End If


    '    ' Attribute für Start-Tag erzeugen
    '    ' --------------------------------
    '    Dim attribCloU, attribCloV, attribOrU, attribOrV, attribPla, attribRat As String

    '    If (isClosed(0)) Then
    '        attribCloU = mkAttrib(closedU_, Ax.bToS(True))
    '    Else
    '        attribCloU = ""
    '    End If

    '    If (isClosed(1)) Then
    '        attribCloV = mkAttrib(closedV_, Ax.bToS(True))
    '    Else
    '        attribCloU = ""
    '    End If

    '    If (isPlanar) Then
    '        attribPla = mkAttrib(planar_, Ax.bToS(True))
    '    Else
    '        attribPla = ""
    '    End If

    '    attribOrU = mkAttrib(orderU_, order(0))
    '    attribOrV = mkAttrib(orderV_, order(1))
    '    attribRat = mkAttrib(rational_, Ax.bToS(isRational))

    '    openTag(BSplineSurface_, attribOrU & attribOrV & attribRat & attribCloU & attribCloV & attribPla)


    '    ' Geometrie-Tags
    '    ' ==============

    '    ' Spline-Geometrie an face auswerten
    '    ' ----------------------------------

    '    ' Spline-Infos
    '    Dim nPU As Integer
    '    Dim nPV As Integer
    '    Dim nKU As Integer
    '    Dim nKV As Integer
    '    nPU = numPoles(0)
    '    nPV = numPoles(1)
    '    nKU = numKnots(0)
    '    nKV = numKnots(1)

    '    'Spline-Data
    '    Dim poles() As Double
    '    Dim KnotsU() As Double
    '    Dim KnotsV() As Double
    '    Dim Weights() As Double
    '    spline.GetBSplineData(poles, KnotsU, KnotsV, Weights)
    '    Dim nW As Integer
    '    nW = UBound(Weights)


    '    ' Statistik
    '    incrt numBSplineSurfacesOfOrder(order(0), order(1))


    '    ' Kontroll-Punkte übertragen
    '    ' --------------------------

    '    ' Start-Tag ControlPoints
    '    Dim attribN, attribM As String
    '    attribN = mkAttrib(n_, nPU)
    '    attribM = mkAttrib(m_, nPV)

    '    openTag(SurfaceControlPoints_, attribN & attribM)


    '    ' Die einzelnen Kontroll-Punkte ausgeben
    '    Dim iP As Integer ' Index für das Feld Poles
    '    iP = 0
    '    Dim iu, iv As Integer  ' Indizes für den Durchlauf durch die Pol-Punkte (Kontrollpunkte)
    '    Dim attribIU, attribIV As String

    '    For iv = 1 To nPV
    '        attribIV = mkAttrib("j", iv)
    '        For iu = 1 To nPU
    '            attribIU = mkAttrib("i", iu)
    '            ' Hole die nächsten drei Koordinatenwerte aus poles und bereite sie als Punktbeschreibung auf
    '            simpleTag(SurfaceControlPoint_, attribIU & attribIV & Ax.pcaToS(poles, iP)) ' (iP wird in der Funktion pcaToS weitergestellt)
    '        Next iu
    '    Next iv

    '    ' End-Tag ControlPoints
    '    closeTag(SurfaceControlPoints_)


    '    ' Gewichte ausgeben
    '    ' -----------------
    '    ' Start-Tag der Gewichte
    '    openTag(SurfaceWeights_, attribN & attribM)

    '    ' Die einzelnen Gewichte ausgeben
    '    Dim attribW As String
    '    Dim iW As Integer ' Index für das Feld der Gewichte
    '    iW = 0

    '    For iv = 1 To nPV
    '        attribIV = mkAttrib("j", iv)
    '        For iu = 1 To nPU
    '            attribIU = mkAttrib("i", iu)
    '            attribW = mkAttrib(val_, cToS(Weights(iW)))
    '            simpleTag(SurfaceWeight_, attribIU & attribIV & attribW)
    '            iW = iW + 1
    '        Next iu
    '    Next iv

    '    ' End-Tag der Gewichte
    '    closeTag(SurfaceWeights_)


    '    ' U-Knots ausgeben
    '    ' ----------------

    '    ' Start-Tag der U-Knots
    '    openTag(Knots_, mkAttrib(name_, nameOfKnotsU_) & mkAttrib(numberOfKnotsU_, nKU))

    '    ' Die einzelnen Knots ausgeben
    '    Dim attribI, attribVal As String
    '    Dim i As Integer

    '    For i = 1 To nKU
    '        attribI = mkAttrib(indexOfKnotsU_, i)
    '        attribVal = mkAttrib(val_, Ax.cToS(KnotsU(i - 1)))
    '        simpleTag(Knot_, attribI & attribVal)
    '    Next i

    '    ' End-Tag der U-Knots
    '    closeTag(Knots_)



    '    ' V-Knots ausgeben
    '    ' ----------------

    '    ' Start-Tag der V-Knots
    '    openTag(Knots_, mkAttrib(name_, nameOfKnotsV_) & mkAttrib(numberOfKnotsV_, nKV))

    '    ' Die einzelnen Knots ausgeben
    '    Dim attribJ As String
    '    Dim j As Integer
    '    For j = 1 To nKV
    '        attribJ = mkAttrib(indexOfKnotsV_, j)
    '        attribVal = mkAttrib(val_, Ax.cToS(KnotsV(j - 1)))
    '        simpleTag(Knot_, attribJ & attribVal)
    '    Next j

    '    ' End-Tag der V-Knots
    '    closeTag(Knots_)


    '    ' Plane-Vektor ausgeben
    '    ' ---------------------
    '    If isPlanar Then
    '        simpleTag(PlaneNormal_, Ax.vToS(planeVector))
    '    End If

    '    ' End-Tag
    '    ' =======
    '    closeTag(BSplineSurface_)
    'End Sub

    Private Sub translateTorus(ByRef Face As Face)

        ' Statistik
        ' incrt numTori

        ' Geometrie ermitteln
        Dim Torus As Torus
        Torus = Face.Geometry
        Dim CenterPoint() As Double
        Dim axisVector() As Double
        Dim majorRadius As Double
        Dim minorRadius As Double
        Torus.GetTorusData(CenterPoint, axisVector, majorRadius, minorRadius)

        ' Start-Tag
        Dim attribMIR, attribMAR As String
        ' attribMIR = mkAttrib(minorRadius_, Ax.cToS(minorRadius))
        ' attribMAR = mkAttrib(majorRadius_, Ax.cToS(majorRadius))
        ' openTag(Torus_, attribMIR & attribMAR)

        ' Geometrie-Tags
        ' simpleTag(CenterPoint_, Ax.vToS(CenterPoint))
        ' simpleTag(AxisVector_, Ax.vToS(axisVector))

        ' End-Tag
        ' closeTag(Torus_)

    End Sub

    Private Sub translateSphere(ByRef Face As Face)

        ' Statistik
        ' incrt numSpheres

        ' Geometrie ermitteln
        Dim sphe As Sphere
        sphe = Face.Geometry
        Dim CenterPoint() As Double
        Dim radius As Double
        sphe.GetSphereData(CenterPoint, radius)

        ' Start-Tag
        'openTag(Sphere_, mkAttrib(radius_, Ax.cToS(radius)))

        ' Geometrie-Tags
        'simpleTag(CenterPoint_, Ax.vToS(CenterPoint))

        ' End-Tag
        'closeTag(Sphere_)
    End Sub

    Private Sub translateEllipticalCylinder(ByRef Face As Face)
        ' Statistik
        'incrt numEllipticalCylinders

        ' Geometrie ermitteln
        Dim ellCyl As EllipticalCylinder
        Dim bP() As Double
        Dim aV() As Double
        Dim MajAx() As Double
        Dim ratio As Double

        ellCyl = Face.Geometry
        ellCyl.GetEllipticalCylinderData(bP, aV, MajAx, ratio)

        ' Start-Tag
        ' openTag(EllipticalCylinder_, mkAttrib(minorMajorRatio_, Ax.cToS(ratio)))

        ' Geometrie-Tags
        ' simpleTag(BasePoint_, Ax.vToS(bP))
        'simpleTag(AxisVector_, Ax.vToS(aV))
        'simpleTag(MajorAxis_, Ax.vToS(MajAx))

        ' End-Tag
        'closeTag(EllipticalCylinder_)

    End Sub

    '    Private Sub translateEllipticalCone(ByRef Face As Face)
    '        ' Statistik
    '        incrt numEllipticalCones

    '        openTag(EllipticalCone_)
    '        closeTag(EllipticalCone_)

    '        Msg.problem("ReadBRepFromPM", "EllipticalCone")
    '    End Sub

    '    Private Sub translateUnknownSurface(ByRef Face As Face)
    '        ' Statistik
    '        incrt numUnknownSurfaces

    '        openTag(UnknownSurface_)
    '        closeTag(UnknownSurface_)

    '        Msg.problem("ReadBRepFromPM", "Unknown Surface")
    '    End Sub

    Private Sub translateEdgeUse(ByRef listOfEdgeUses As EdgeUses, ByVal k As Integer)

        Dim eU As EdgeUse
        eU = listOfEdgeUses(k)

        Dim key As String
        Dim e As Edge
        e = eU.Edge()
        key = objectToKeyString(e)

        'simpleTag(EdgeUse_, mkAttrib(edgeID_, "E" & getNumberOfEdge(key)))

    End Sub





    Private Sub initBRepXmlFile()



    End Sub

    '###############################################
    ' Transfer edges to xml file
    '###############################################

    ' *************
    ' * TransEdge *
    ' *************

    ' Übertragung der Kanten
    ' ======================

    Public Sub translateEdges(ByRef oEdges As Edges)
        'openTag(Edges_)
        ' Dim n, i As Integer
        'n = Edges.Count
        'Statistics.numEdges = n
        For i As Integer = 1 To oEdges.Count()
            translateEdge(oEdges, i)
        Next
        'closeTag(Edges_)
        'Ax.switchToHourGlassCursor()       'Nicht löschen!
    End Sub


    Private Sub translateEdge(ByRef oEdges As Edges, ByVal i As Integer)
        ' Initialisierungen
        ' =================
        Dim oEdge As Edge = oEdges.Item(i)
        ' Start-Tag Edge
        ' ==============
        'Dim attribEdge As String
        'attribEdge = mkAttrib(id_, "E" & i)
        'If (printKeys) Then
        '    attribEdge = attribEdge & mkAttrib(extRef_, objectToKeyString(Edge))
        'End If
        'openTag(Edge_, attribEdge)
        ' Edge-Struktur
        ' =============
        Dim sV As String
        Dim sF As String
        'analyseIncidentVertices(Edge, sV)
        'analyseIncidentFaces(Edge, sF)
        'simpleTag(EdgeStructure_, sV & sF)


        ' Geometrie
        ' =========

        ' Start-Tag Edge-Geometrie
        ' ------------------------
        'openTag(EdgeGeometry_)


        ' Konkrete Edge-Geometrie
        ' -----------------------
        Dim geoType As CurveTypeEnum = oEdge.GeometryType()
        Select Case geoType

            ' ----------------------------------
            ' Treten beim FAPS-Auto auf:
            ' ----------------------------------
            Case CurveTypeEnum.kLineSegmentCurve
                translateLineSegment(oEdge)

            Case CurveTypeEnum.kCircleCurve
                translateCircle(oEdge)

            Case CurveTypeEnum.kCircularArcCurve
                translateCircularArc(oEdge)

            Case CurveTypeEnum.kEllipticalArcCurve
                translateEllipticalArc(oEdge)

            Case CurveTypeEnum.kBSplineCurve
                translateBSplineCurve(oEdge)

            Case CurveTypeEnum.kEllipseFullCurve
                translateEllipse(oEdge)

                '    ' --------------------------------------------------------------------------
                '    ' Treten beim FAPS-Auto nicht auf, werden aber später hier auch abgehandelt
                '    ' --------------------------------------------------------------------------

                'Case CurveTypeEnum.kLineCurve
                '    translateLine(oEdge)

                'Case CurveTypeEnum.kPolylineCurve
                '    translatePolyline(oEdge)

                'Case Else
                '    translateUnknownCurve(oEdge)

        End Select


        ' End-Tag Edge-Geometrie
        ' ----------------------
        'closeTag(EdgeGeometry_)


        ' End-Tag Edge
        ' ============
        'closeTag(Edge_)

    End Sub


    ' Line-Segment wird als Line aufgefaßt und als solche nach XML übertragen

    Private Sub translateLineSegment(ByRef Edge As Edge)

        ' Statistik
        'incrt numLineSegments



        ' Start-Tag
        'openTag(Line_)

        ' Geometrie-Tags
        Dim lS As LineSegment
        lS = Edge.Geometry
        'simpleTag(StartPoint_, Ax.pToS(lS.StartPoint))
        'simpleTag(EndPoint_, Ax.pToS(lS.EndPoint))

        ' End-Tag
        'closeTag(Line_)

    End Sub

    Private Sub translateCircle(ByRef Edge As Edge)

        ' Statistik
        'incrt numCircles

        ' Kreisgeometrie auslesen: cir
        Dim cir As Inventor.Circle
        cir = Edge.Geometry

        ' Start-Tag
        'openTag(Circle_, mkAttrib(radius_, Ax.cToS(cir.radius)))

        ' Mittelpunkt
        Dim c As Point
        c = cir.Center
        'simpleTag(CenterPoint_, Ax.pToS(c))

        ' Normalen-Vector
        Dim nv() As Double
        Dim normal As Vector
        normal = cir.Normal.AsVector
        normal.GetVectorData(nv)
        'simpleTag(PlaneNormal_, Ax.vToS(nv))

        ' End-Tag
        'closeTag(Circle_)
    End Sub

    Private Sub translateEllipse(ByRef Edge As Edge)

        ' Statistik
        'incrt numEllipses

        ' Ellipsengeometrie auslesen: ellips
        Dim ellips As Inventor.EllipseFull
        ellips = Edge.Geometry
        Dim center() As Double
        Dim axisVector() As Double
        Dim majorAxis() As Double
        Dim minorMajorRatio As Double

        ellips.GetEllipseFullData(center, axisVector, majorAxis, minorMajorRatio)

        Dim majorRadius As Double
        Dim minorRadius As Double

        'majorRadius = euclideanLength(majorAxis)
        'minorRadius = majorRadius * minorMajorRatio


        ' Start-Tag
        'openTag(Ellipse_, mkAttrib(majorRadius_, Ax.cToS(majorRadius)) & mkAttrib(minorRadius_, Ax.cToS(minorRadius)))

        ' Mittelpunkt
        'simpleTag(CenterPoint_, Ax.vToS(center))

        ' Normalen-Vector
        Dim nv() As Double
        Dim normal As Vector
        normal = ellips.Normal.AsVector
        normal.GetVectorData(nv)
        'simpleTag(PlaneNormal_, Ax.vToS(nv))

        ' Major-Axis
        'simpleTag(MajorAxis_, Ax.vToS(majorAxis))

        ' End-Tag
        'closeTag(Ellipse_)

    End Sub

    'Private Sub translateLine(ByRef Edge As Edge)

    '    ' Statistik
    '    incrt numLines

    '    openTag(Line_)
    '    closeTag(Line_)

    'End Sub

    'Private Sub translatePolyline(ByRef Edge As Edge)

    '    ' Statistik
    '    ' =========
    '    incrt numPolylines

    '    openTag(Polyline_)
    '    closeTag(Polyline_)

    'End Sub

    'Private Sub translateUnknownCurve(ByRef Edge As Edge)

    '    ' Statistik
    '    ' =========
    '    incrt numUnknownCurves

    '    openTag(UnknownCurve_)
    '    closeTag(UnknownCurve_)
    'End Sub

    Private Sub translateCircularArc(ByRef Edge As Edge)

        ' Statistik
        'incrt numCircularArcs

        ' Geometrie auslesen: arc
        Dim arc As Arc3d
        arc = Edge.Geometry

        ' Start-Tag
        Dim attribR, attribA As String
        'attribR = mkAttrib(radius_, Ax.cToS(arc.radius))
        ' attribA = mkAttrib(sweepAngle_, Ax.cToS(arc.sweepAngle))
        'openTag(CircularArc_, attribR & attribA)

        ' Geometrie-Tags
        ' --------------
        Dim p As Point
        Dim message As String
        Dim num As Integer
        Dim err As Boolean
        err = False

        ' Mittelpunkt
        p = arc.Center
        'simpleTag(CenterPoint_, Ax.pToS(p))

        ' Orthonormal-Vector
        Dim nv() As Double
        Dim normal As Vector
        normal = arc.Normal.AsVector
        normal.GetVectorData(nv)
        'simpleTag(PlaneNormal_, Ax.vToS(nv))

        ' Startpunkt
        p = arc.StartPoint
        'simpleTag(StartPoint_, Ax.pToS(p))

        ' Endpunkt
        p = arc.EndPoint
        'simpleTag(EndPoint_, Ax.pToS(p))

        ' End-Tag
        ' closeTag(CircularArc_)

    End Sub

    Private Sub translateEllipticalArc(ByRef Edge As Edge)

        ' Statistik
        ' ---------
        'incrt numEllipticalArcs

        ' Geometrie auslesen: elarc
        ' -------------------------
        Dim elarc As EllipticalArc
        elarc = Edge.Geometry

        ' Start-Tag
        ' ---------
        Dim attribMAR, attribMIR, attribA As String

        ' attribMAR = mkAttrib(majorRadius_, Ax.cToS(elarc.majorRadius))
        ' attribMIR = mkAttrib(minorRadius_, Ax.cToS(elarc.minorRadius))
        ' attribA = mkAttrib(sweepAngle_, Ax.cToS(elarc.sweepAngle))

        'openTag(EllipticalArc_, attribMAR & attribMIR & attribA)

        ' Geometrie-Tags
        ' --------------
        'simpleTag(CenterPoint_, Ax.pToS(elarc.center))


        ' MajorAxis und MinorAxis einlesen
        Dim majAxis() As Double
        elarc.MajorAxis.AsVector.GetVectorData(majAxis)
        Dim minAxis() As Double
        elarc.MinorAxis.AsVector.GetVectorData(minAxis)

        ' PlaneNormal berechnen
        Dim plaNor(2) As Double
        ' Ax.vectorProduct(majAxis, minAxis, plaNor)
        ' Ax.normalize(plaNor)         Hier wird geglaubt, dass majAxis und minAxis normiert sind.
        'simpleTag(PlaneNormal_, Ax.vToS(plaNor))
        'simpleTag(MajorAxis_, Ax.vToS(majAxis))
        'simpleTag(StartPoint_, Ax.pToS(elarc.StartPoint))
        'simpleTag(EndPoint_, Ax.pToS(elarc.EndPoint))

        ' End-Tag
        ' ' -------
        'closeTag(EllipticalArc_)

    End Sub

    Private Sub translateBSplineCurve(ByRef Edge As Edge)

        ' Geometrie auslesen: elarc
        ' =========================
        Dim spline As BSplineCurve
        spline = Edge.Geometry
        Dim order As Long
        Dim numPoles As Long
        Dim numKnots As Long
        Dim isRational As Boolean
        Dim isPeriodic As Boolean  ' wird nicht übertragen
        Dim isClosed As Boolean
        Dim isPlanar As Boolean
        Dim planeVector() As Double
        spline.GetBSplineInfo(order, numPoles, numKnots, isRational, isPeriodic, isClosed, isPlanar, planeVector)
        Dim poles() As Double
        Dim Knots() As Double
        Dim Weights() As Double    ' Weights werden keine geliefert.
        spline.GetBSplineData(poles, Knots, Weights)


        ' Statistik
        ' =========
        'incrt numBSplineCurves
        'incrt numBSplineCurvesOfOrder(order)


        ' Start-Tag
        ' =========
        If (order < 1 Or 4 < order) Then
            'Msg.warning("Order of B-spline curve is out of range")
        End If
        Dim attribOrd, attribRat, attribClo, attribPlan As String
        'attribOrd = mkAttrib(order_, order)
        'attribRat = mkAttrib(rational_, Ax.bToS(isRational))
        If (isClosed) Then
            'attribClo = mkAttrib(closed_, Ax.bToS(isClosed))
        Else
            attribClo = ""
        End If
        If (isPlanar) Then
            'attribPlan = mkAttrib(planar_, Ax.bToS(isPlanar))
        Else
            attribPlan = ""
        End If
        'openTag(BSplineCurve_, attribOrd & attribRat & attribClo & attribPlan)


        ' Geometrie-Tags
        ' ==============

        ' Kontroll-Punkte
        ' ---------------

        ' Start-Tag ControlPoints
        Dim attribN As String
        ' attribN = mkAttrib(n_, numPoles)
        ' openTag(CurveControlPoints_, attribN)

        ' Die einzelnen Kontroll-Punkte ausgeben
        Dim iP As Integer ' Index für das Feld Poles
        iP = 0
        Dim i As Integer  ' Index für den Durchlauf durch die Pol-Punkte (Kontrollpunkte)
        Dim attribI As String

        For i = 1 To numPoles
            'simpleTag(CurveControlPoint_, mkAttrib("i", i) & Ax.pcaToS(poles, iP))
        Next i

        ' End-Tag ControlPoints
        'closeTag(CurveControlPoints_)



        ' Knots
        ' -----

        ' Start-Tag der Knots
        'openTag(KnotsOfCurve_, mkAttrib(numKnotsOfCurve_, numKnots))

        ' Die einzelnen Knots ausgeben
        For i = 1 To numKnots
            'attribI = mkAttrib(indexOfKnotsOfCurve_, i)
            Dim attribKnot As String
            'attribKnot = mkAttrib(val_, Ax.cToS(Knots(i - 1)))
            'simpleTag(Knot_, attribI & attribKnot)
        Next i

        ' End-Tag der Knots
        'closeTag(Knots_)


        ' Plane-Vektor
        ' ------------
        If isPlanar Then
            ' simpleTag(PlaneNormal_, Ax.vToS(planeVector))
        End If


        ' End-Tag
        ' =======
        'closeTag(BSplineCurve_)

    End Sub

    Private Sub analyseIncidentVertices(ByRef oEdge As Edge, ByRef s As String)
        Dim v As Vertex
        Dim key As String
        Dim n As Integer

        ' Start-Knoten
        ' ============
        v = oEdge.StartVertex
        key = objectToKeyString(v)
        'n = getNumberOfVertex(key)
        If (n >= 1) Then
            s = " startVertex=""V" & n & """"
        Else
            s = " startVertex=""ERROR"""
        End If

        ' End-Knoten
        ' ==========
        ' v = Edge.StopVertex
        key = objectToKeyString(v)
        ' n = getNumberOfVertex(key)
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
            key = objectToKeyString(f)
            'n = getNumberOfFace(key)
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
            key = objectToKeyString(f)
            ' n = getNumberOfFace(key)
            If (n >= 1) Then
                sF = sF & " face2=""F" & n & """"
            Else
                sF = sF & " face2=""ERROR"""
            End If
        End If

    End Sub




    '###############################################
    ' Transfer vertices to xml file
    '###############################################

    Public Sub translateVertices(ByRef oVertices As Vertices)
        'openTag(Vertices_)
        'Dim n, i As Integer
        'n = oVertices.Count
        'Statistics.numVertices = n
        For i As Integer = 1 To oVertices.Count()
            TranslateVertex(oVertices, i)
        Next
        'closeTag(Vertices_)
        'Ax.switchToHourGlassCursor()       'Nicht löschen!
    End Sub

    Private Sub TranslateVertex(ByRef oVertices As Vertices, ByVal i As Integer)

        Dim oVertex As Vertex = oVertices.Item(i)

        ' Start-Tag
        ' =========
        'oXMLWriter.WriteStartElement("Vertices")

        oXMLWriter.WriteStartElement("Vertex")
        oXMLWriter.WriteAttributeString("id", "V" & i)

        Dim point(2) As Double
        oVertex.GetPoint(point)
        oXMLWriter.WriteAttributeString("x", point(0))
        oXMLWriter.WriteAttributeString("y", point(1))
        oXMLWriter.WriteAttributeString("z", point(2))
        oXMLWriter.WriteStartElement("Location")

        'oXMLWriter.WriteEndElement()
        oXMLWriter.WriteEndElement()
        oXMLWriter.WriteEndElement()
        'oXMLWriter.Flush()

        'Dim idVertex, startTag, keyString, attributes As String
        'idVertex = "V" & i
        'If (printKeys) Then
        '    keyString = objectToKeyString(Vertex)
        '    attributes = mkAttrib(id_, idVertex) & mkAttrib(extRef_, keyString)
        'Else
        '    attributes = mkAttrib(id_, idVertex)
        'End If

        'openTag(Vertex_, attributes)

        '' Location-Tag
        '' ============
        'Dim p() As Double
        'Vertex.GetPoint(p)
        'simpleTag(Location_, Ax.vToS(p))

        '' End-Tag
        '' =======
        'closeTag(Vertex_)

    End Sub

    '##############################################
    '
    '###############################################

    Private Function detObjectLists(ByRef vl As Vertices, ByRef el As Edges, ByRef fl As Faces) As Boolean

        ' Find the 
        Dim oDocument As Document = MIDAddin.ActiveDocument
        Dim oCompDef As AssemblyComponentDefinition = oDocument.ComponentDefinition

        Dim oCompOccs As ComponentOccurrences = oCompDef.Occurrences
        If oCompOccs.Count() = 0 Then
            'Msg.problem("ReadBRep", "No body")
            detObjectLists = False
            Exit Function
        End If
        If oCompOccs.Count() > 1 Then
            'Msg.problem("ReadBRep", "More then one body")
            detObjectLists = False
            Exit Function
        End If
        'Ax.analyseRangeBox(body)

        Dim oCompOcc As ComponentOccurrence = oCompDef.Occurrences.Item(1)
        Dim oBodies As SurfaceBodies = oCompOcc.SurfaceBodies

        If oBodies.Count() = 0 Then
            detObjectLists = False
            Exit Function
        End If
        If oBodies.Count() > 1 Then
            detObjectLists = False
            Exit Function
        End If

        Dim oBody As SurfaceBody = oBodies.Item(1)
        Dim oShells As FaceShells = oBody.FaceShells

        'n = oShells.Count()
        If oShells.Count() = 0 Then
            'Msg.problem("ReadBRep", "No shell")
            detObjectLists = False
            Exit Function
        End If
        If oShells.Count() > 1 Then
            'Msg.problem("ReadBRep", "More then one shell")
            detObjectLists = False
            Exit Function
        End If
        Dim oShell As FaceShell = oShells.Item(1)

        ' Objekt-Listen
        vl = oBody.Vertices
        el = oShell.Edges
        fl = oShell.Faces

        detObjectLists = True

        'Ax.switchToHourGlassCursor()       'Nicht löschen!
    End Function

    '###################################################################
    ' Key Man
    '###################################################################

    Public Sub initKeyManaging()
        Dim oDocument As Document
        oDocument = MIDAddin.ActiveDocument
        oRefKeyManager = oDocument.ReferenceKeyManager
        oKeyContextNumber = oRefKeyManager.CreateKeyContext
        Dim keyContextArray() As Byte
        oRefKeyManager.SaveContextToArray(oKeyContextNumber, keyContextArray)

        'initVertexMap()
        'initEdgeMap()
        'initFaceMap()
    End Sub

    'Public Function objectToKeyString(ByRef oObject As Object) As String
    '    Dim keyArray() As Byte
    '    oObject.GetReferenceKey(keyArray, oKeyContextNumber)
    '    objectToKeyString = oRefKeyManager.KeyToString(keyArray)
    'End Function

    'Public Function vertexToKeyString(ByRef v As Inventor.Vertex) As String
    '    Dim keyArray() As Byte
    '    v.GetReferenceKey(keyArray, oKeyContextNumber)
    '    vertexToKeyString = oRefKeyManager.KeyToString(keyArray)
    'End Function

    Public Sub fillMaps(ByRef oVertices As Vertices, ByRef oEdges As Edges, ByRef oFaces As Faces)

        Dim keyString As String
        Dim oVertex As Vertex
        Dim oEdge As Edge
        Dim oFace As Face

        For i As Integer = 1 To oEdges.Count()
            oVertex = oVertices.Item(i)
            keyString = objectToKeyString(oVertex)
            ' appendVertex(keyString, i)
        Next

        For i As Integer = 1 To oEdges.Count()
            oEdge = oEdges.Item(i)
            keyString = objectToKeyString(oEdge)
            'appendEdge(keyString, i)
        Next

        For i = 1 To oFaces.Count()
            oFace = oFaces.Item(i)
            keyString = objectToKeyString(oFace)
            'appendFace(keyString, i)
        Next

        'Ax.switchToHourGlassCursor()       'Nicht löschen!
    End Sub
    Public Function objectToKeyString(ByRef oObject As Object) As String
        Dim keyArray() As Byte
        oObject.GetReferenceKey(keyArray, oKeyContextNumber)
        objectToKeyString = oRefKeyManager.KeyToString(keyArray)
    End Function


End Class
