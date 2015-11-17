'footprint 1.2
#If _useInventor Then


Imports Inventor
Imports System.Runtime.InteropServices
Imports System.Type
Imports System.Activator
Imports System.Math


Module Sketch_3D_Geometries

    Dim _invApp As Inventor.Application
    Dim oSketch3D As Sketch3D
    Dim oPartCompDef As PartComponentDefinition
    Dim startpointtemp() As Double
    Dim endpointtemp() As Double
    Dim oPartDoc As PartDocument
    Dim oPart As PartDocument
    Dim PartCompDef As PartComponentDefinition
    Dim oTG As TransientGeometry
    Dim oCompDef As PartComponentDefinition
    Dim wp As WorkPoint


    Public Sub initialize_environment3D()
        Try

            _invApp = Marshal.GetActiveObject("Inventor.Application")
            _invApp.Visible = True
            oPartCompDef = _invApp.ActiveDocument.ComponentDefinition
            oSketch3D = oPartCompDef.Sketches3D.Add
            oPart = _invApp.Documents.Add(DocumentTypeEnum.kPartDocumentObject)
            PartCompDef = oPart.ComponentDefinition
            oPartDoc = _invApp.ActiveDocument
            oTG = _invApp.TransientGeometry
            oCompDef = oPartDoc.ComponentDefinition

        Catch ex As Exception
            Try
                Dim invAppType As Type = _
                  GetTypeFromProgID("Inventor.Application")
                _invApp = CreateInstance(invAppType)

            Catch ex2 As Exception
                MsgBox(ex2.ToString())
                MsgBox("Unable to get or start Inventor")
            End Try
        End Try
    End Sub



    '#################test#############################

    Sub DrawEllipticalArc3D(ByVal majRad As Double, ByVal minRad As Double, ByVal swAng As Double, ByVal cePoi() As Double, _
                             ByVal majAxVec() As Double, ByVal staPoi() As Double, ByVal endPoi() As Double, ByVal pN() As Double)


        Dim oSketch As PlanarSketch
        Dim oWorkPlane As WorkPlane
        Dim oProfile As Profile
        Dim oP(0 To 1) As Point
        Dim cP As Point
        Dim oWP(0 To 1) As WorkPoint
        Dim oFitPoints As ObjectCollection
        Dim oPath As Path
        Dim ellarc_p As EllipticalArc



        oFitPoints = _invApp.TransientObjects.CreateObjectCollection '_invApp.TransientObjects.CreateObjectCollection

        'Workpoint for Plane needed for Sweepfeature
        oWP(0) = oCompDef.WorkPoints.AddFixed(oTG.CreatePoint(endPoi(2), endPoi(1), endPoi(0)))

        'Points for Elliptical Arc
        oP(0) = oTG.CreatePoint(staPoi(2), staPoi(1), staPoi(0))
        oP(1) = oTG.CreatePoint(endPoi(2), endPoi(1), endPoi(0))

        'Add Points for ObjectCollection



        'Create 3D-Sketch
        'oSketch3D = oPartDoc.ComponentDefinition.Sketches3D.Add
        ''Add Line to 3D-Sketch
        Dim ellArc As SketchEllipticalArc3D
        Dim minax(0 To 2) As Double
        Dim delta, XX, YY, ZZ As Double
        Dim angle0(0 To 9) As Double
        minax(0) = 1
        minax(1) = 0
        minax(2) = 0



        For i = 1 To 9


            angle0(i) = detEllArcStartAngle(majRad, minRad, staPoi, swAng, cePoi, pN, majAxVec)
            ' delta = swAng - angle0(i)
            '  computePointsOfEllipse(1, angle0(i), delta, XX, YY, ZZ)


            '
            'ellarc_p.Center.PutPointData(cePoi)
            'ellarc_p.CenterPoint.PutPointData(cePoi)
            'ellarc_p.EndPoint.PutPointData(endPoi)
            'ellarc_p.StartPoint.PutPointData(staPoi)
            'ellarc_p.PutEllipticalArcData(cePoi, majAxVec, minax, majRad, minRad, angle0(i), swAng)

            'oFitPoints.Add(oP(i))
        Next







        '   Dim ospline As SketchSpline3D = oSketch3D.SketchSplines3D.Add(oFitPoints)

        ' Dim oLine As SketchLine3D = oSketch3D.SketchLines3D.AddByTwoPoints(oP(0), oP(1))

        'Add Plane | normal to curve (Line) at startpoint
        'oWorkPlane = oCompDef.WorkPlanes.AddByNormalToCurve(oLine, oWP(0))
        'oSketch = PartCompDef.Sketches.Add(oWorkPlane)

        'Punkt am Startpunkt hinzufügen
        'With _invApp.TransientGeometry
        '    Call oSketch.SketchCircles.AddByCenterRadius(.CreatePoint2d(endPoi(0), endPoi(1)), 0.050000000000000003)
        'End With

        'oProfile = oSketch.Profiles.AddForSolid

        'oPath = oCompDef.Features.CreatePath(ospline)

        'If fkt_Create_Sweep_Feature(oPath, oProfile) = False Then MsgBox("Fehler in Modul Sketch_3D_Geometries - fkt_Create_Sweep_Feature")



    End Sub




    '###################test###########################
    'Sub DrawEllipticalArc3D(ByVal majRad As Double, ByVal minRad As Double, ByVal swAng As Double, ByVal cePoi() As Double, _
    '                       ByVal majAxVec() As Double, ByVal staPoi() As Double, ByVal endPoi() As Double, ByVal pN() As Double)








    '    Dim oSketch, oSketchProfile As PlanarSketch
    '    Dim oWorkPlane, oProfileWorkPlane As WorkPlane
    '    Dim oProfile As Profile
    '    Dim oP(0 To 1) As Point
    '    Dim cP As Point
    '    Dim oWP(0 To 2) As WorkPoint
    '    Dim oFitPoints As ObjectCollection
    '    Dim oPath As Path
    '    Dim ellipticalArcObject As EllipticalArc2d
    '    Dim WPpN As UnitVector
    '    Dim plane As Plane

    '    oFitPoints = _invApp.TransientObjects.CreateObjectCollection '_invApp.TransientObjects.CreateObjectCollection

    '    'Workpoint for Plane needed for Sweepfeature
    '    oWP(0) = oCompDef.WorkPoints.AddFixed(oTG.CreatePoint(staPoi(0), staPoi(1), staPoi(2)))
    '    oWP(1) = oCompDef.WorkPoints.AddFixed(oTG.CreatePoint(endPoi(0), endPoi(1), endPoi(2)))
    '    oWP(2) = oCompDef.WorkPoints.AddFixed(oTG.CreatePoint(cePoi(0), cePoi(1), cePoi(2)))

    '    oWorkPlane = oCompDef.WorkPlanes.AddByThreePoints(oWP(0), oWP(1), oWP(2))

    '    ' oWorkPlane = oCompDef.WorkPlanes.ad

    '    plane = oWorkPlane.Plane
    '    WPpN = plane.Normal


    '    oSketch = PartCompDef.Sketches.Add(oWorkPlane)


    '    Dim x, y, z, xtemp, ytemp, ztemp As Double
    '    Dim PN_decision As Integer

    '    x = WPpN.X
    '    y = WPpN.Y
    '    z = WPpN.Z

    '    xtemp = Math.Abs(x)
    '    ytemp = Math.Abs(y)
    '    ztemp = Math.Abs(z)


    '    'Index PN_decision:

    '    ' 1 = x und positiv
    '    ' 2 = x und negativ
    '    ' 3 = y und positiv
    '    ' 4 = y und negativ
    '    ' 5 = z und positiv
    '    ' 6 = z und negativ


    '    If xtemp > ytemp Then
    '        If xtemp > ztemp Then
    '            'x am größten PN = x
    '            If x > 0 Then
    '                PN_decision = 1
    '                DrawEllipticalArc2D(majRad, minRad, swAng, cePoi, _
    '                                          majAxVec, staPoi, endPoi, pN, PN_decision, oWorkPlane)
    '            Else
    '                PN_decision = 2
    '                DrawEllipticalArc2D(majRad, minRad, swAng, cePoi, _
    '                                          majAxVec, staPoi, endPoi, pN, PN_decision, oWorkPlane)
    '            End If
    '        Else
    '            'z am größten PN = z
    '            If z > 0 Then
    '                PN_decision = 5
    '                DrawEllipticalArc2D(majRad, minRad, swAng, cePoi, _
    '                                          majAxVec, staPoi, endPoi, pN, PN_decision, oWorkPlane)
    '            Else
    '                PN_decision = 6
    '                DrawEllipticalArc2D(majRad, minRad, swAng, cePoi, _
    '                                          majAxVec, staPoi, endPoi, pN, PN_decision, oWorkPlane)
    '            End If

    '        End If
    '    Else
    '        If ytemp > ztemp Then
    '            'y am größten PN = y
    '            If y > 0 Then
    '                PN_decision = 4
    '                DrawEllipticalArc2D(majRad, minRad, swAng, cePoi, _
    '                                      majAxVec, staPoi, endPoi, pN, PN_decision, oWorkPlane)
    '            Else
    '                PN_decision = 5
    '                DrawEllipticalArc2D(majRad, minRad, swAng, cePoi, _
    '                                      majAxVec, staPoi, endPoi, pN, PN_decision, oWorkPlane)
    '            End If

    '        Else
    '            'z am größten PN = z
    '            If z > 0 Then
    '                PN_decision = 5
    '                DrawEllipticalArc2D(majRad, minRad, swAng, cePoi, _
    '                                          majAxVec, staPoi, endPoi, pN, PN_decision, oWorkPlane)
    '            Else
    '                PN_decision = 6
    '                DrawEllipticalArc2D(majRad, minRad, swAng, cePoi, _
    '                                          majAxVec, staPoi, endPoi, pN, PN_decision, oWorkPlane)
    '            End If
    '        End If


    '    End If

    'End Sub

    ''2DSkizze erstellen
    'Function DrawEllipticalArc2D(ByVal majRad As Double, ByVal minRad As Double, ByVal swAng As Double, ByVal cePoi() As Double, _
    '                       ByVal majAxVec() As Double, ByVal staPoi() As Double, ByVal endPoi() As Double, ByVal pN() As Double, _
    '                       ByVal PN_decision As Integer, ByVal workPlane As WorkPlane)


    '    Dim oSketch As PlanarSketch
    '    Dim oTG As TransientGeometry = _invApp.TransientGeometry
    '    Dim oFitPoints As ObjectCollection = _invApp.TransientObjects.CreateObjectCollection

    '    oSketch = oPartCompDef.Sketches.Add(workPlane)

    '    If PN_decision = 6 Then 'negative z-achse

    '        Try
    '            'printEllArcToProtocol("EllArc Orig", majRad, minRad, swAng, cePoi, majAxVec, staPoi, endPoi, pN)       
    '            Dim majAxVec2 As UnitVector2d = oTG.CreateVector2d(majAxVec(0), majAxVec(1)).AsUnitVector
    '            Dim cePoi2 As Point2d = _invApp.TransientGeometry.CreatePoint2d(cePoi(0), cePoi(1))

    '            Dim swAngRad As Double = (swAng * PI) / 180  'Umwandlung von Grad in Rad
    '            Dim angle0 As Double

    '            If pN(2) > 0 Then
    '                angle0 = detEllArcStartAngle(majRad, minRad, staPoi, swAng, cePoi, pN, majAxVec)
    '            Else
    '                'printMessageToProtocol("Endpunkt wird als Start-Punkt angesehen")
    '                'pN(2) = -pN(2)
    '                pN(0) = 0
    '                pN(1) = 0
    '                pN(2) = 1
    '                angle0 = detEllArcStartAngle(majRad, minRad, endPoi, swAng, cePoi, pN, majAxVec)
    '                'printEllArcToProtocol("EllArc gespiegelt ", majRad, minRad, swAng, cePoi, majAxVec, endPoi, staPoi, pN)       
    '            End If

    '            'printDoubleToProtocol("StartAngle", angle0) 

    '            oSketch.SketchEllipticalArcs.Add(cePoi2, majAxVec2, majRad, minRad, angle0, swAngRad)

    '        Catch ex As Exception
    '            MsgBox(ex.ToString())
    '        End Try

    '        ReDim cePoi(0)

    '    End If

    'End Function

    'Linie zeichnen
    Sub DrawLine3D(ByVal startpoint() As Double, ByVal endpoint() As Double)

        Dim oSketch As PlanarSketch
        Dim oWorkPlane As WorkPlane
        Dim oProfile As Profile
        Dim oP(0 To 1) As Point
        Dim cP As Point
        Dim oWP(0 To 1) As WorkPoint
        Dim oFitPoints As ObjectCollection
        Dim oPath As Path


        oFitPoints = _invApp.TransientObjects.CreateObjectCollection '_invApp.TransientObjects.CreateObjectCollection

        'Workpoint for Plane needed for Sweepfeature
        oWP(0) = oCompDef.WorkPoints.AddFixed(oTG.CreatePoint(startpoint(0), startpoint(1), startpoint(2)))

        'Points for Lines
        oP(0) = oTG.CreatePoint(startpoint(0), startpoint(1), startpoint(2))
        oP(1) = oTG.CreatePoint(endpoint(0), endpoint(1), endpoint(2))

        'Add Points for ObjectCollection
        oFitPoints.Add(oP(0))
        oFitPoints.Add(oP(1))

        'Create 3D-Sketch
        oSketch3D = oPartDoc.ComponentDefinition.Sketches3D.Add
        'Add Line to 3D-Sketch
        Dim oLine As SketchLine3D = oSketch3D.SketchLines3D.AddByTwoPoints(oP(0), oP(1))


        'Dim ell As SketchEllipticalArc3DProxy = oSketch3D.SketchEllipses3D.Item(1)

        'Dim oSpline As SketchSpline3D = oSketch3D.SketchSplines3D.Add(oFitPoints)

        'Add Plane | normal to curve (Line) at startpoint
        oWorkPlane = oCompDef.WorkPlanes.AddByNormalToCurve(oLine, oWP(0))
        oSketch = PartCompDef.Sketches.Add(oWorkPlane)
        'oSketch = PartCompDef.Sketches.AddWithOrientation(oWorkPlane, PartCompDef.WorkAxes.Item(1), False, False, PartCompDef.WorkPoints.Item(1), True)

        'oWorkPlane = oCompDef.WorkPlanes.AddByThreePoints(

        'Punkt am Startpunkt hinzufügen



        With _invApp.TransientGeometry
            Call oSketch.SketchCircles.AddByCenterRadius(.CreatePoint2d(startpoint(1), startpoint(2)), 0.050000000000000003)
        End With



        oProfile = oSketch.Profiles.AddForSolid

        oPath = oCompDef.Features.CreatePath(oLine)

        If fkt_Create_Sweep_Feature(oPath, oProfile) = False Then MsgBox("Fehler in Modul Sketch_3D_Geometries - fkt_Create_Sweep_Feature")

    End Sub

    Function fkt_Create_Sweep_Feature(ByVal oPath As Path, ByVal oProfile As Profile) As Boolean
        Try
            Dim Sweep As SweepFeature = oCompDef.Features.SweepFeatures.AddUsingPath(oProfile, oPath, PartFeatureOperationEnum.kJoinOperation)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
End Module
#End If