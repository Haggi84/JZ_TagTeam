Option Explicit On

Imports System.Xml
Imports System.Text
Imports Inventor
Imports System.Windows.Forms
Imports System.Math

'###########################################
' Circuit Carrier Class
'###########################################

Public Class PlaceMidRequest
    Inherits ChangeRequest

    Private oAddIn As Inventor.Application
    Private oServer As MidAddInServer
    Private oPlaceMidCmd As PlaceMidCommand


    Private oXMLWriter As XmlTextWriter

    ' not used
    '#############################################
    Public oKeyContextNumber As Long
    Private oRefKeyManager As ReferenceKeyManager
    '#############################################

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

    Private workDir, fileName, unitOfAngles, unitOfLengths, precisionOfAngles, precisionOfLengths As String

    ' Constructor
    '**********************************************************************************************************
    Public Sub New(ByVal placeMidCmd As PlaceMidCommand, _
                   ByVal addIn As Inventor.Application, _
                   ByVal server As MidAddInServer)

        Me.oPlaceMidCmd = placeMidCmd
        Me.oAddIn = addIn
        Me.oServer = server

    End Sub

    ' Execute Undo command
    '**********************************************************************************************************
    Public Overrides Sub OnExecute(document As Document, _
                                   context As NameValueMap, _
                                   ByRef succeeded As Boolean)

        oServer.MidDataTypes.AddCircuitCarrier(oPlaceMidCmd.FileName)

        ' Export BRep
        ReadBRep(oServer.MidDataTypes.CircuitCarrier.Occurrence)

        ' later the export is better preserved in an extra class
        'Dim oBRepExport As BRepExport = New BRepExport(oAddIn, oServer)
        'oBRepExport.ReadBRepAI(oServer.MidDataTypes.CircuitCarrier.Occurrence)

        ' Enable other commands
        Dim oControls As ControlDefinitions = oAddIn.CommandManager.ControlDefinitions
        Dim oControl As ControlDefinition
        oControl = oControls.Item("ReadNetlistIntern")
        oControl.Enabled = True
        oControl = oControls.Item("KeepOutsIntern")
        oControl.Enabled = True
        oControl = oControls.Item("RotateMidIntern")
        oControl.Enabled = True
        oControl = oControls.Item("PlacePartIntern")
        oControl.Enabled = True
        oControl = oControls.Item("LoadKeepOutsIntern")
        oControl.Enabled = True
        oControl = oControls.Item("OpticalWaveguideIntern")
        oControl.Enabled = True


    End Sub


    ' Convert from default length units
    '**********************************************************************************************************
    Private Function ConvertLength(value As Double) As Double

        Dim UOM As UnitsOfMeasure = oAddIn.ActiveDocument.UnitsOfMeasure
        Return UOM.ConvertUnits(Round(value, Convert.ToByte(precisionOfLengths)), UOM.LengthUnits, UOM.GetTypeFromString(unitOfLengths))

    End Function

    ' Convert from default angle units
    '**********************************************************************************************************
    Private Function ConvertAngle(value As Double) As Double

        Dim UOM As UnitsOfMeasure = oAddIn.ActiveDocument.UnitsOfMeasure
        Return UOM.ConvertUnits(Round(value, Convert.ToByte(precisionOfAngles)), UOM.AngleUnits, UOM.GetTypeFromString(unitOfAngles))

    End Function

    ' ReadBRepAI (based ReadBRepAI from GFaI)
    '**********************************************************************************************************
    Public Sub ReadBRep(oOccurrence As ComponentOccurrence)

        ' Get export data
        oPlaceMidCmd.GetExportData(precisionOfAngles, precisionOfLengths, unitOfAngles, unitOfLengths)

        ' Get work direcotry
        workDir = oServer.Commands.WorkDirectory

        ' Obtain the faces, edges and vertices of the BRep
        Dim oVertices As Vertices
        Dim oEdges As Edges
        Dim oFaces As Faces
        If Not (detObjectLists(oOccurrence, oVertices, oEdges, oFaces)) Then
            Exit Sub
        End If

        ' Key Managing (not used here)
        'initKeyManaging()

        ' Assosiate internal Ids with vertices, edges and faces
        SetAttributes(oVertices, oEdges, oFaces)

        Try
            ' Generate new XML-document
            Dim oEncoding As Encoding = New UTF8Encoding()

            oXMLWriter = New XmlTextWriter(workDir & "\MidBRep.xml", oEncoding)

            oXMLWriter.Formatting = Formatting.Indented

            '#################################################################################################
            ' CREATE HEADER
            '#################################################################################################
            oXMLWriter.WriteStartDocument()
            oXMLWriter.WriteStartElement("BRep")

            oXMLWriter.WriteAttributeString("denotion", oOccurrence.Name)
            oXMLWriter.WriteAttributeString("cadSystem", "Autodesk Inventor Professional 2013")
            oXMLWriter.WriteAttributeString("unitOfLength", unitOfLengths)
            oXMLWriter.WriteAttributeString("precisionOfLength", precisionOfLengths)
            oXMLWriter.WriteAttributeString("unitOfAngles", unitOfAngles)
            oXMLWriter.WriteAttributeString("precisionOfAngles", precisionOfAngles)
            oXMLWriter.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance")
            oXMLWriter.WriteAttributeString("xsi:noNamespaceSchemaLocation", "BrepMID.xsd") '++++ Add scheme here

            '#################################################################################################
            ' TRANSLATE VERTICES
            '#################################################################################################

            oXMLWriter.WriteStartElement("Vertices")

            For i As Integer = 1 To oVertices.Count()
                oXMLWriter.WriteStartElement("Vertex")
                oXMLWriter.WriteAttributeString("id", "V" & i)

                Dim oVertex As Vertex = oVertices.Item(i)
                Dim point(2) As Double
                oVertex.GetPoint(point)

                oXMLWriter.WriteStartElement("Location")
                oXMLWriter.WriteAttributeString("x", ConvertLength(point(0)))
                oXMLWriter.WriteAttributeString("y", ConvertLength(point(1)))
                oXMLWriter.WriteAttributeString("z", ConvertLength(point(2)))
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

                ' Edge structure --> the internal vertex and face number were saved before
                '**************************************************************************************************************************
                oXMLWriter.WriteStartElement("EdgeStructure")
                oXMLWriter.WriteAttributeString("startVertex", oEdges(i).StartVertex.AttributeSets.Item("MIDVertex").Item("vertexId").Value)
                oXMLWriter.WriteAttributeString("endVertex", oEdges(i).StopVertex.AttributeSets.Item("MIDVertex").Item("vertexId").Value)
                For j As Integer = 1 To oEdges(i).Faces.Count
                    oXMLWriter.WriteAttributeString("face" & j, oEdges(i).Faces.Item(j).AttributeSets.Item("MIDFace").Item("faceId").Value)
                Next
                oXMLWriter.WriteEndElement() 'EdgeStructure
                '**************************************************************************************************************************

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
                        oXMLWriter.WriteAttributeString("x", ConvertLength(oLineSeg.StartPoint.X))
                        oXMLWriter.WriteAttributeString("y", ConvertLength(oLineSeg.StartPoint.Y))
                        oXMLWriter.WriteAttributeString("z", ConvertLength(oLineSeg.StartPoint.Z))
                        oXMLWriter.WriteEndElement() ' StartPoint

                        oXMLWriter.WriteStartElement("EndPoint")
                        oXMLWriter.WriteAttributeString("x", ConvertLength(oLineSeg.EndPoint.X))
                        oXMLWriter.WriteAttributeString("y", ConvertLength(oLineSeg.EndPoint.Y))
                        oXMLWriter.WriteAttributeString("z", ConvertLength(oLineSeg.EndPoint.Z))
                        oXMLWriter.WriteEndElement() ' EndPoint

                        oXMLWriter.WriteEndElement() ' Line

                    Case CurveTypeEnum.kCircleCurve
                        'translateCircle(oEdge)
                        Dim oCircle As Inventor.Circle = oEdge.Geometry
                        oXMLWriter.WriteStartElement("Circle")
                        oXMLWriter.WriteAttributeString("radius", ConvertLength(oCircle.Radius))

                        oXMLWriter.WriteStartElement("CenterPoint")
                        oXMLWriter.WriteAttributeString("x", ConvertLength(oCircle.Center.X))
                        oXMLWriter.WriteAttributeString("y", ConvertLength(oCircle.Center.Y))
                        oXMLWriter.WriteAttributeString("z", ConvertLength(oCircle.Center.Z))
                        oXMLWriter.WriteEndElement() ' CenterPoint

                        oXMLWriter.WriteStartElement("PlaneNormal")
                        oXMLWriter.WriteAttributeString("x", ConvertLength(oCircle.Normal.X))
                        oXMLWriter.WriteAttributeString("y", ConvertLength(oCircle.Normal.Y))
                        oXMLWriter.WriteAttributeString("z", ConvertLength(oCircle.Normal.Z))
                        oXMLWriter.WriteEndElement() ' CenterPoint

                        oXMLWriter.WriteEndElement() ' Circle

                    Case CurveTypeEnum.kCircularArcCurve
                        'translateCircularArc(oEdge)

                        Dim oArc As Arc3d = oEdge.Geometry
                        oXMLWriter.WriteStartElement("CircularArc")
                        oXMLWriter.WriteAttributeString("radius", ConvertLength(oArc.Radius))
                        oXMLWriter.WriteAttributeString("sweepAngle", ConvertAngle(oArc.SweepAngle))

                        oXMLWriter.WriteStartElement("StartPoint")
                        oXMLWriter.WriteAttributeString("x", ConvertLength(oArc.StartPoint.X))
                        oXMLWriter.WriteAttributeString("y", ConvertLength(oArc.StartPoint.Y))
                        oXMLWriter.WriteAttributeString("z", ConvertLength(oArc.StartPoint.Z))
                        oXMLWriter.WriteEndElement() ' StartPoint

                        oXMLWriter.WriteStartElement("CenterPoint")
                        oXMLWriter.WriteAttributeString("x", ConvertLength(oArc.Center.X))
                        oXMLWriter.WriteAttributeString("y", ConvertLength(oArc.Center.Y))
                        oXMLWriter.WriteAttributeString("z", ConvertLength(oArc.Center.Z))
                        oXMLWriter.WriteEndElement() ' CenterPoint

                        oXMLWriter.WriteStartElement("EndPoint")
                        oXMLWriter.WriteAttributeString("x", ConvertLength(oArc.EndPoint.X))
                        oXMLWriter.WriteAttributeString("y", ConvertLength(oArc.EndPoint.Y))
                        oXMLWriter.WriteAttributeString("z", ConvertLength(oArc.EndPoint.Z))
                        oXMLWriter.WriteEndElement() ' EndPoint

                        oXMLWriter.WriteStartElement("PlaneNormal")
                        oXMLWriter.WriteAttributeString("x", ConvertLength(oArc.Normal.X))
                        oXMLWriter.WriteAttributeString("y", ConvertLength(oArc.Normal.Y))
                        oXMLWriter.WriteAttributeString("z", ConvertLength(oArc.Normal.Z))
                        oXMLWriter.WriteEndElement() ' PlaneNormal

                        oXMLWriter.WriteEndElement() 'CircularArc

                    Case CurveTypeEnum.kEllipticalArcCurve
                        'translateEllipticalArc(oEdge)
                        Dim oEArc As EllipticalArc = oEdge.Geometry

                        oXMLWriter.WriteStartElement("EllipticalArc")
                        oXMLWriter.WriteAttributeString("majorRadius", ConvertLength(oEArc.MajorRadius))
                        oXMLWriter.WriteAttributeString("minorRadius", ConvertLength(oEArc.MinorRadius))
                        oXMLWriter.WriteAttributeString("sweepAngle", ConvertAngle(oEArc.SweepAngle))

                        oXMLWriter.WriteStartElement("CenterPoint")
                        oXMLWriter.WriteAttributeString("x", ConvertLength(oEArc.Center.X))
                        oXMLWriter.WriteAttributeString("y", ConvertLength(oEArc.Center.Y))
                        oXMLWriter.WriteAttributeString("z", ConvertLength(oEArc.Center.Z))
                        oXMLWriter.WriteEndElement()

                        Dim oNormal As UnitVector = oEArc.MinorAxis.CrossProduct(oEArc.MajorAxis)
                        oXMLWriter.WriteStartElement("PlaneNormal")
                        oXMLWriter.WriteAttributeString("x", ConvertLength(oNormal.X))
                        oXMLWriter.WriteAttributeString("y", ConvertLength(oNormal.Y))
                        oXMLWriter.WriteAttributeString("z", ConvertLength(oNormal.Z))
                        oXMLWriter.WriteEndElement()

                        oXMLWriter.WriteStartElement("MajorAxis")
                        oXMLWriter.WriteAttributeString("x", ConvertLength(oEArc.MajorAxis.X))
                        oXMLWriter.WriteAttributeString("y", ConvertLength(oEArc.MajorAxis.Y))
                        oXMLWriter.WriteAttributeString("z", ConvertLength(oEArc.MajorAxis.Z))
                        oXMLWriter.WriteEndElement()

                        oXMLWriter.WriteStartElement("StartPoint")
                        oXMLWriter.WriteAttributeString("x", ConvertLength(oEArc.StartPoint.X))
                        oXMLWriter.WriteAttributeString("y", ConvertLength(oEArc.StartPoint.Y))
                        oXMLWriter.WriteAttributeString("z", ConvertLength(oEArc.StartPoint.Z))
                        oXMLWriter.WriteEndElement()

                        oXMLWriter.WriteStartElement("EndPoint")
                        oXMLWriter.WriteAttributeString("x", ConvertLength(oEArc.EndPoint.X))
                        oXMLWriter.WriteAttributeString("y", ConvertLength(oEArc.EndPoint.Y))
                        oXMLWriter.WriteAttributeString("z", ConvertLength(oEArc.EndPoint.Z))
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
                            oXMLWriter.WriteAttributeString("x", ConvertLength(oSpline.PoleAtIndex(j).X))
                            oXMLWriter.WriteAttributeString("y", ConvertLength(oSpline.PoleAtIndex(j).Y))
                            oXMLWriter.WriteAttributeString("z", ConvertLength(oSpline.PoleAtIndex(j).Z))
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
                        oXMLWriter.WriteAttributeString("x", ConvertLength(oEllipse.Center.X))
                        oXMLWriter.WriteAttributeString("y", ConvertLength(oEllipse.Center.Y))
                        oXMLWriter.WriteAttributeString("z", ConvertLength(oEllipse.Center.Z))
                        oXMLWriter.WriteEndElement()

                        oXMLWriter.WriteStartElement("PlaneNormal")
                        oXMLWriter.WriteAttributeString("x", ConvertLength(oEllipse.Normal.X))
                        oXMLWriter.WriteAttributeString("y", ConvertLength(oEllipse.Normal.Y))
                        oXMLWriter.WriteAttributeString("z", ConvertLength(oEllipse.Normal.Z))
                        oXMLWriter.WriteEndElement()

                        oXMLWriter.WriteStartElement("MajorAxis")
                        oXMLWriter.WriteAttributeString("x", ConvertLength(oEllipse.MajorAxisVector.X))
                        oXMLWriter.WriteAttributeString("y", ConvertLength(oEllipse.MajorAxisVector.Y))
                        oXMLWriter.WriteAttributeString("z", ConvertLength(oEllipse.MajorAxisVector.Z))
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
                        oXMLWriter.WriteAttributeString("x", ConvertLength(rootPoint(0)))
                        oXMLWriter.WriteAttributeString("y", ConvertLength(rootPoint(1)))
                        oXMLWriter.WriteAttributeString("z", ConvertLength(rootPoint(2)))
                        oXMLWriter.WriteEndElement() ' Location

                        oXMLWriter.WriteStartElement("PlaneNormal")
                        oXMLWriter.WriteAttributeString("x", ConvertLength(normalVector(0)))
                        oXMLWriter.WriteAttributeString("y", ConvertLength(normalVector(1)))
                        oXMLWriter.WriteAttributeString("z", ConvertLength(normalVector(2)))
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
                        oXMLWriter.WriteAttributeString("halfAngle", ConvertAngle(halfAngle))
                        oXMLWriter.WriteAttributeString("expanding", isExpanding)
                        oXMLWriter.WriteAttributeString("radius", ConvertLength(radius))

                        Dim apex(2) As Double
                        GetApex(halfAngle, radius, axisVector, basePoint, apex)
                        oXMLWriter.WriteStartElement("Apex")
                        oXMLWriter.WriteAttributeString("x", ConvertLength(apex(0)))
                        oXMLWriter.WriteAttributeString("y", ConvertLength(apex(1)))
                        oXMLWriter.WriteAttributeString("z", ConvertLength(apex(2)))
                        oXMLWriter.WriteEndElement() ' Apex

                        oXMLWriter.WriteStartElement("AxisVector")
                        oXMLWriter.WriteAttributeString("x", ConvertLength(axisVector(0)))
                        oXMLWriter.WriteAttributeString("y", ConvertLength(axisVector(1)))
                        oXMLWriter.WriteAttributeString("z", ConvertLength(axisVector(2)))
                        oXMLWriter.WriteEndElement() ' AxisVector

                        oXMLWriter.WriteStartElement("BasePoint")
                        oXMLWriter.WriteAttributeString("x", ConvertLength(basePoint(0)))
                        oXMLWriter.WriteAttributeString("y", ConvertLength(basePoint(1)))
                        oXMLWriter.WriteAttributeString("z", ConvertLength(basePoint(2)))
                        oXMLWriter.WriteEndElement() ' BasePoint

                        oXMLWriter.WriteEndElement() ' Cone


                    Case SurfaceTypeEnum.kCylinderSurface
                        'translateCylinder(Face)
                        Dim oCylinder As Cylinder = oFace.Geometry
                        Dim basePoint(2), axisVector(2), radius As Double
                        oCylinder.GetCylinderData(basePoint, axisVector, radius)

                        oXMLWriter.WriteStartElement("Cylinder")
                        oXMLWriter.WriteAttributeString("radius", ConvertLength(radius))

                        oXMLWriter.WriteStartElement("BasePoint")
                        oXMLWriter.WriteAttributeString("x", ConvertLength(basePoint(0)))
                        oXMLWriter.WriteAttributeString("y", ConvertLength(basePoint(1)))
                        oXMLWriter.WriteAttributeString("z", ConvertLength(basePoint(2)))
                        oXMLWriter.WriteEndElement() ' BasePoint

                        oXMLWriter.WriteStartElement("AxisVector")
                        oXMLWriter.WriteAttributeString("x", ConvertLength(axisVector(0)))
                        oXMLWriter.WriteAttributeString("y", ConvertLength(axisVector(1)))
                        oXMLWriter.WriteAttributeString("z", ConvertLength(axisVector(2)))
                        oXMLWriter.WriteEndElement() ' AxisVector

                        oXMLWriter.WriteEndElement() ' Cylinder

                    Case SurfaceTypeEnum.kTorusSurface
                        'translateTorus(Face)

                        ' Geometrie ermitteln
                        Dim oTorus As Torus = oFace.Geometry
                        Dim centerPoint(2), axisVector(2), majorRadius, minorRadius As Double
                        oTorus.GetTorusData(centerPoint, axisVector, majorRadius, minorRadius)

                        oXMLWriter.WriteStartElement("Torus")
                        oXMLWriter.WriteAttributeString("majorRadius", ConvertLength(majorRadius))
                        oXMLWriter.WriteAttributeString("minorRadius", ConvertLength(minorRadius))

                        oXMLWriter.WriteStartElement("CenterPoint")
                        oXMLWriter.WriteAttributeString("x", ConvertLength(centerPoint(0)))
                        oXMLWriter.WriteAttributeString("y", ConvertLength(centerPoint(1)))
                        oXMLWriter.WriteAttributeString("z", ConvertLength(centerPoint(2)))
                        oXMLWriter.WriteEndElement() ' CenterPoint

                        oXMLWriter.WriteStartElement("AxisVector")
                        oXMLWriter.WriteAttributeString("x", ConvertLength(axisVector(0)))
                        oXMLWriter.WriteAttributeString("y", ConvertLength(axisVector(1)))
                        oXMLWriter.WriteAttributeString("z", ConvertLength(axisVector(2)))
                        oXMLWriter.WriteEndElement() ' AxisVector

                        oXMLWriter.WriteEndElement() ' Torus


                    Case SurfaceTypeEnum.kSphereSurface
                        'translateSphere(Face)

                        Dim oSphere As Sphere = oFace.Geometry
                        Dim centerPoint(2), radius As Double
                        oSphere.GetSphereData(centerPoint, radius)

                        oXMLWriter.WriteStartElement("Sphere")
                        oXMLWriter.WriteAttributeString("radius", ConvertLength(radius))

                        oXMLWriter.WriteStartElement("CenterPoint")
                        oXMLWriter.WriteAttributeString("x", ConvertLength(centerPoint(0)))
                        oXMLWriter.WriteAttributeString("y", ConvertLength(centerPoint(1)))
                        oXMLWriter.WriteAttributeString("z", ConvertLength(centerPoint(2)))
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

                                oXMLWriter.WriteAttributeString("x", ConvertLength(oSpline.PoleAtIndex(k, j).X))
                                oXMLWriter.WriteAttributeString("y", ConvertLength(oSpline.PoleAtIndex(k, j).Y))
                                oXMLWriter.WriteAttributeString("z", ConvertLength(oSpline.PoleAtIndex(k, j).Z))
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
                            oXMLWriter.WriteAttributeString("x", ConvertLength(planeNormal(0)))
                            oXMLWriter.WriteAttributeString("y", ConvertLength(planeNormal(1)))
                            oXMLWriter.WriteAttributeString("z", ConvertLength(planeNormal(2)))
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
                        oXMLWriter.WriteAttributeString("minorMajorRation", ConvertLength(ratio))

                        oXMLWriter.WriteStartElement("BasePoint")
                        oXMLWriter.WriteAttributeString("x", ConvertLength(basePoint(0)))
                        oXMLWriter.WriteAttributeString("y", ConvertLength(basePoint(1)))
                        oXMLWriter.WriteAttributeString("z", ConvertLength(basePoint(2)))
                        oXMLWriter.WriteEndElement() ' BasePoint

                        oXMLWriter.WriteStartElement("AxisVector")
                        oXMLWriter.WriteAttributeString("x", ConvertLength(axisVector(0)))
                        oXMLWriter.WriteAttributeString("y", ConvertLength(axisVector(1)))
                        oXMLWriter.WriteAttributeString("z", ConvertLength(axisVector(2)))
                        oXMLWriter.WriteEndElement() ' AxisVector

                        oXMLWriter.WriteStartElement("MajorAxis")
                        oXMLWriter.WriteAttributeString("x", ConvertLength(majorAxis(0)))
                        oXMLWriter.WriteAttributeString("y", ConvertLength(majorAxis(1)))
                        oXMLWriter.WriteAttributeString("z", ConvertLength(majorAxis(2)))
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

            'Call openTag("BSplineSurfaceStatistics", mkAttrib("n", numBSplineSurfaces))

            'Dim i, j As Integer
            'incrt level
            'For i = 1 To 4
            '    For j = 1 To 4
            '        If (numBSplineSurfacesOfOrder(i, j) <> 0) Then
            '            Call simpleTag("OrderUV", mkAttrib("ordU", i) & mkAttrib("ordV", j) & mkAttrib("n", numBSplineSurfacesOfOrder(i, j)))
            '        End If
            '    Next j
            'Next i
            'decrt level

            'Call closeTag("BSplineSurfaceStatistics")



            oXMLWriter.WriteEndElement() ' FaceStatistics
            oXMLWriter.WriteEndElement() ' Statistics
            'End Faces

            oXMLWriter.WriteEndElement() ' BRep
            oXMLWriter.WriteEndDocument()

        Catch ex As System.IO.DirectoryNotFoundException
            System.Windows.Forms.MessageBox.Show("Could not export BRep successfully (missing directory)", "MID Project", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            System.Windows.Forms.MessageBox.Show("Something went wrong during the export process", "MID Project", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            oXMLWriter.Flush()
            oXMLWriter.Close()
        End Try

        ' Feedback for the user (causes error message when clicking on browser --> bug??)
        'MessageBox.Show("BRep successfully exported", "MID Project", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)

    End Sub

    ' Calculate apex of a cone
    '********************************************************************************************************************
    Private Sub GetApex(ByRef halfAngle As Double, ByRef radius As Double, ByRef axisVector() As Double, ByRef BasePoint() As Double, ByRef Apex() As Double)
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


    ' For edge statistics
    '********************************************************************************************************************
    Private Sub AddEdgeToStats(ByRef edge As Edge)

        Dim geoType As CurveTypeEnum = edge.GeometryType()
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

    ' For face statistics
    '********************************************************************************************************************
    Private Sub AddFaceToStats(ByRef face As Face)

        Dim geoType As SurfaceTypeEnum = face.SurfaceType()
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

    ' Get the faces, edges and vertices of the given occurrence
    '********************************************************************************************************************
    Private Function detObjectLists(ByVal occurrence As ComponentOccurrence, _
                                    ByRef vertices As Vertices, _
                                    ByRef edges As Edges, _
                                    ByRef faces As Faces) As Boolean

        Dim oBodies As SurfaceBodies = occurrence.SurfaceBodies

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
            'Call Msg.problem("ReadBRep", "No shell")
            detObjectLists = False
            Exit Function
        End If
        If oShells.Count() > 1 Then
            'Call Msg.problem("ReadBRep", "More then one shell")
            detObjectLists = False
            Exit Function
        End If
        Dim oShell As FaceShell = oShells.Item(1)

        vertices = oBody.Vertices
        edges = oShell.Edges
        faces = oShell.Faces

        Return True

    End Function

    ' Initialize key manager (not used here)
    '********************************************************************************************************************
    Private Sub initKeyManaging()
        Dim oDocument As Document = oAddIn.ActiveDocument
        oRefKeyManager = oDocument.ReferenceKeyManager
        oKeyContextNumber = oRefKeyManager.CreateKeyContext
        MsgBox(oKeyContextNumber) '+++check length of number
        Dim keyContextArray(12) As Byte
        oRefKeyManager.SaveContextToArray(oKeyContextNumber, keyContextArray)

        'initVertexMap()
        'initEdgeMap()
        'initFaceMap()
    End Sub

    ' Read key from object (not used here)
    '********************************************************************************************************************
    Private Function ObjectToKeyString(ByRef oObject As Object) As String
        Dim keyArray(44) As Byte
        oObject.GetReferenceKey(keyArray, oKeyContextNumber)
        Return oRefKeyManager.KeyToString(keyArray)
    End Function

    ' Set Attributes
    '********************************************************************************************************************
    Private Sub SetAttributes(ByRef vertices As Vertices, _
                              ByRef edges As Edges, _
                              ByRef faces As Faces)

        'Dim keyString As String
        Dim oVertex As Vertex
        Dim oEdge As Edge
        Dim oFace As Face
        Dim oAttribSets As AttributeSets
        Dim oAttribSet As AttributeSet

        For i As Integer = 1 To vertices.Count()
            oVertex = vertices.Item(i)
            'keyString = ObjectToKeyString(oVertex)
            oAttribSets = oVertex.AttributeSets
            oAttribSet = oAttribSets.Add("MIDVertex")
            oAttribSet.Add("vertexId", ValueTypeEnum.kStringType, "V" & i.ToString)
            ' appendVertex(keyString, i)
        Next

        For i As Integer = 1 To edges.Count()
            oEdge = edges.Item(i)
            'keyString = ObjectToKeyString(oEdge)
            oAttribSets = oEdge.AttributeSets
            oAttribSet = oAttribSets.Add("MIDEdge")
            oAttribSet.Add("edgeId", ValueTypeEnum.kStringType, "E" & i.ToString)
            'appendEdge(keyString, i)
        Next

        For i = 1 To faces.Count()
            oFace = faces.Item(i)
            'keyString = ObjectToKeyString(oFace)
            oAttribSets = oFace.AttributeSets
            oAttribSet = oAttribSets.Add("MIDFace")
            oAttribSet.Add("faceId", ValueTypeEnum.kStringType, "F" & i.ToString)
            'appendFace(keyString, i)
        Next

    End Sub



End Class



