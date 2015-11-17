Imports Inventor

Module Basic_Geometries

    Sub create_circularArc(ByVal _invApp As Inventor.Application, ByVal osketch As Sketch, ByVal oPartCompDef As PartComponentDefinition, ByVal pointCenter As Point2d, ByVal pointA As Inventor.Point2d, _
                            ByVal pointB As Inventor.Point2d)

        With _invApp.TransientGeometry

            osketch.SketchArcs.AddByCenterStartEndPoint(.pointCenter, .pointA, .pointB)

        End With

    End Sub

    Sub create_line(ByVal _invApp As Inventor.Application, ByVal osketch As Sketch, ByVal oPartCompDef As PartComponentDefinition, ByRef startpoint() As Double, ByRef endpoint() As Double) 'ByVal pointA As Inventor.Point2d, ByVal pointB As Inventor.Point2d)

        'Dim StartPointInv, EndPointInv As Point2d
        'StartPointInv.X = startpoint(0)
        'StartPointInv.Y = startpoint(1)

        'EndPointInv.X = endpoint(0)
        'EndPointInv.X = endpoint(1)


        With _invApp.TransientGeometry
            '  osketch.SketchLines.AddByTwoPoints(.pointA, .pointB)


            osketch.SketchLines.AddByTwoPoints(.CreatePoint2d(startpoint(0), startpoint(1)), .CreatePoint2d(endpoint(0), endpoint(1)))


        End With
    End Sub

    Sub create_circle(ByVal _invApp As Inventor.Application, ByVal osketch As Sketch, ByVal oPartCompDef As PartComponentDefinition, ByVal pointCenter As Inventor.Point2d, ByVal radius As Double)
        With _invApp.TransientGeometry
            osketch.SketchCircles.AddByCenterRadius(.pointCenter, .radius)

        End With
    End Sub

    Sub create_ellipse(ByVal _invApp As Inventor.Application, ByVal osketch As Sketch, ByVal oPartCompDef As PartComponentDefinition, ByVal pointCenter As Inventor.Point2d, _
                        ByVal radiusMj As Double, ByVal radiusMn As Double, ByVal Vector As Inventor.UnitVector2d)
        With _invApp.TransientGeometry
            osketch.SketchEllipses.Add(.pointCenter, .MajorAxisVectorby, .radiusMj, .radiusMn)


        End With
    End Sub

    Sub create_ellipticalArc(ByVal _invApp As Inventor.Application, ByVal osketch As Sketch, ByVal oPartCompDef As PartComponentDefinition, ByVal pointCenter As Inventor.Point2d, _
                              ByVal radiusMj As Double, ByVal radiusMn As Double, ByVal MaVector As Vector, ByVal angleSweep As Double, ByVal angleStart As Double)
        With _invApp.TransientGeometry
            osketch.SketchEllipticalArcs.Add(pointCenter, MaVector, radiusMj, radiusMn, angleStart, angleSweep)
        End With
    End Sub
    Sub create_BSplineCurve(ByVal _invApp As Inventor.Application, ByVal osketch As Sketch, ByVal oPartCompDef As PartComponentDefinition, ByVal fitPointCollection As Inventor.ObjectCollection)
        With _invApp.TransientGeometry
            osketch.SketchSplines.Add(fitPointCollection)
        End With
    End Sub
End Module
