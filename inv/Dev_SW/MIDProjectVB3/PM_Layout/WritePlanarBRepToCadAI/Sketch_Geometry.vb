'footprint 1.2
#If _useInventor Then
Imports Inventor
Imports System.Runtime.InteropServices
Imports System.Type
Imports System.Activator
Imports System.Math


Module Sketch_Geometry
    Dim _invApp As Inventor.Application
    Dim oSketch As Sketch
    Dim oPartCompDef As PartComponentDefinition
    Dim startpointtemp() As Double
    Dim endpointtemp() As Double

    Public Sub initialize_environment()

        Try
            _invApp = Marshal.GetActiveObject("Inventor.Application")
            _invApp.Visible = True
            oPartCompDef = _invApp.ActiveDocument.ComponentDefinition
            oSketch = oPartCompDef.Sketches.Add(oPartCompDef.WorkPlanes.Item(3))

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




    'Linie zeichnen
    Sub DrawLine(ByVal startpoint() As Double, ByVal endpoint() As Double)
        ' Linie erstellen
        Try
            'If (startpoint(2) = endpoint(2)) Then
            'If (startpoint(1) = endpoint(1)) Then
            'End If
            'Else

            With _invApp.TransientGeometry
                Call oSketch.SketchLines.AddByTwoPoints(.CreatePoint2d(startpoint(0), startpoint(1)), .CreatePoint2d(endpoint(0), endpoint(1)))
            End With
            'End If
        Catch ex As Exception
            ' MsgBox(ex.ToString())
        End Try

    End Sub

    ' **************************
    ' * Ellipsenbogen zeichnen *
    ' **************************

    Sub Draw_EllipticalArc(ByVal majRad As Double, ByVal minRad As Double, ByVal swAng As Double, ByVal cePoi() As Double, _
                           ByVal majAxVec() As Double, ByVal staPoi() As Double, ByVal endPoi() As Double, ByVal pN() As Double)

        Dim oTG As TransientGeometry = _invApp.TransientGeometry
        Dim oFitPoints As ObjectCollection = _invApp.TransientObjects.CreateObjectCollection

        'Falls ein elliptischer Bogen 360 Grad hat, wird eine Ellipse gezeichnet
        If swAng = 360 Then
            Draw_Ellipse(majRad, minRad, cePoi, majAxVec)
            Exit Sub
        End If

        Try
            'printEllArcToProtocol("EllArc Orig", majRad, minRad, swAng, cePoi, majAxVec, staPoi, endPoi, pN)       
            Dim majAxVec2 As UnitVector2d = oTG.CreateVector2d(majAxVec(0), majAxVec(1)).AsUnitVector
            Dim cePoi2 As Point2d = _invApp.TransientGeometry.CreatePoint2d(cePoi(0), cePoi(1))

            Dim swAngRad As Double = (swAng * PI) / 180  'Umwandlung von Grad in Rad
            Dim angle0 As Double

            If pN(2) > 0 Then
                angle0 = detEllArcStartAngle(majRad, minRad, staPoi, swAng, cePoi, pN, majAxVec)
            Else
                'printMessageToProtocol("Endpunkt wird als Start-Punkt angesehen")
                pN(2) = -pN(2)
                angle0 = detEllArcStartAngle(majRad, minRad, endPoi, swAng, cePoi, pN, majAxVec)
                'printEllArcToProtocol("EllArc gespiegelt ", majRad, minRad, swAng, cePoi, majAxVec, endPoi, staPoi, pN)       
            End If

            'printDoubleToProtocol("StartAngle", angle0) 

            oSketch.SketchEllipticalArcs.Add(cePoi2, majAxVec2, majRad, minRad, angle0, swAngRad)

        Catch ex As Exception
            MsgBox(ex.ToString())
        End Try

        ReDim cePoi(0)

    End Sub


    Sub printEllArcToProtocol(ByVal name As String, ByVal majRad As Double, ByVal minRad As Double, ByVal swAng As Double, ByVal cePoi() As Double, _
                              ByVal majAxVec() As Double, ByVal staPoi() As Double, ByVal endPoi() As Double, ByVal pN() As Double)
        printMessageToProtocol("Elliptical Arc")
        printDoubleToProtocol("majRad", majRad)
        printDoubleToProtocol("minRad", minRad)
        printDoubleToProtocol("swAng", swAng)
        printVectorToProtocol("cePoi", cePoi)
        printVectorToProtocol("majAxVec", majAxVec)
        printVectorToProtocol("staPoi", staPoi)
        printVectorToProtocol("endPoi", endPoi)
        printVectorToProtocol("pN", pN)
    End Sub

    ' Ellipse zeichnen
    Sub Draw_Ellipse(ByVal Ell_MajorRadius As Double, ByVal Ell_MinorRadius As Double, ByVal Ell_CenterPoint() As Double, ByVal Ell_MajorAxisVector() As Double)
        Dim oTG As TransientGeometry = _invApp.TransientGeometry
        Try
            Dim oFitPoints As ObjectCollection = _invApp.TransientObjects.CreateObjectCollection
            Dim MajorAxisVector As UnitVector2d = oTG.CreateVector2d(Ell_MajorAxisVector(0), Ell_MajorAxisVector(1)).AsUnitVector
            Dim CenterPoint As Point2d = _invApp.TransientGeometry.CreatePoint2d(Ell_CenterPoint(0), Ell_CenterPoint(1))
            Dim Ellipse As SketchEllipse = oSketch.SketchEllipses.Add(CenterPoint, MajorAxisVector, Ell_MajorRadius, Ell_MinorRadius)
        Catch ex As Exception
            'MsgBox(ex.ToString())
        End Try


    End Sub


End Module

#End If