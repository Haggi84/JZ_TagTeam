'footprint 1.2
' ***********
' * BRepVoc *
' ***********

' Vokabular der XML-Dokumente zur Beschreibung von BReps von MID-Schaltungsträgern
' (entsprechend BRepMID.xsd)

Option Explicit
'Option Private Module

Module BRepVoc

' Tag-Namen
' =========
Public Const BRep_                     As String = "BRep"
Public Const Vertices_                 As String = "Vertices"
Public Const Edges_                    As String = "Edges"
Public Const Faces_                    As String = "Faces"
Public Const Statistics_               As String = "Statistics"
Public Const Vertex_                   As String = "Vertex"
Public Const Edge_                     As String = "Edge"
Public Const Face_                     As String = "Face"
Public Const VertexStatistics_         As String = "VertexStatistics"
Public Const EdgeStatistics_           As String = "EdgeStatistics"
Public Const FaceStatistics_           As String = "FaceStatistics"
Public Const BSplineCurveStatistics_   As String = "BSplineCurveStatistics"
Public Const BSplineSurfaceStatistics_ As String = "BSplineSurfaceStatistics"
Public Const Location_                 As String = "Location"
Public Const EdgeStructure_            As String = "EdgeStructure"
Public Const EdgeGeometry_             As String = "EdgeGeometry"
Public Const Line_                     As String = "Line"
Public Const StartPoint_               As String = "StartPoint"
Public Const EndPoint_                 As String = "EndPoint"
Public Const CircularArc_              As String = "CircularArc"
Public Const CenterPoint_              As String = "CenterPoint"
Public Const PlaneNormal_              As String = "PlaneNormal"
Public Const FaceStructure_            As String = "FaceStructure"
Public Const FaceGeometry_             As String = "FaceGeometry"
Public Const Loops_                    As String = "Loops"
Public Const Sphere_                   As String = "Sphere"
Public Const Cylinder_                 As String = "Cylinder"
Public Const EllipticalCylinder_       As String = "EllipticalCylinder"
Public Const Cone_                     As String = "Cone"
Public Const EllipticalCone_           As String = "EllipticalCone"
Public Const Torus_                    As String = "Torus"
Public Const CubicSplineSurface_       As String = "CubicSplineSurface"
Public Const BSplineSurface_           As String = "BSplineSurface"
Public Const Plane_                    As String = "Plane"
Public Const BasePoint_                As String = "BasePoint"
Public Const AxisVector_               As String = "AxisVector"
Public Const MajorAxis_                As String = "MajorAxis"
Public Const Apex_                     As String = "Apex"
Public Const KnotsOfCurve_             As String = "Knots"
Public Const KnotsU_                   As String = "KnotsU"
Public Const KnotsV_                   As String = "KnotsV"
Public Const SurfaceControlPoint_      As String = "ControlPoint"
Public Const BiCubicPolynoms_          As String = "BiCubicPolynoms"
Public Const DoubleIndexedReal_        As String = "DoubleIndexedReal"
Public Const CoefficientVectorFeeld_   As String = "CoefficientVectorFeeld"
Public Const SurfaceWeights_           As String = "SurfaceWeights"
Public Const SurfaceWeight_            As String = "Weight"
Public Const Ellipse_                  As String = "Ellipse"
Public Const Polyline_                 As String = "Polyline"
Public Const UnknownCurve_             As String = "UnknownCurve"
Public Const UnknownSurface_           As String = "UnknownSurface"
Public Const SurfaceControlPoints_     As String = "SurfaceControlPoints"
Public Const DoubleIndexedVector_      As String = "DoubleIndexedVector"
Public Const EllipticalArc_            As String = "EllipticalArc"
Public Const Loop_                     As String = "Loop"
Public Const EdgeUse_                  As String = "EdgeUse"
Public Const Circle_                   As String = "Circle"
Public Const CubicSplineCurve_         As String = "CubicSplineCurve"
Public Const BSplineCurve_             As String = "BSplineCurve"
Public Const Weights_                  As String = "Weights"
Public Const Knots_                    As String = "Knots"
Public Const Knot_                     As String = "Knot"
Public Const CurveControlPoints_       As String = "CurveControlPoints"
Public Const CurveControlPoint_        As String = "ControlPoint"
Public Const CubicPolynoms_            As String = "CubicPolynoms"
Public Const IndexedReal_              As String = "IndexedReal"
Public Const IndexedVector_            As String = "IndexedVector"


' Attribut-Namen
' ==============
Public Const denotion_                 As String = "denotion"
Public Const cadSystem_                As String = "cadSystem"
Public Const unitOfLength_             As String = "unitOfLength"
Public Const precisionOfLength_        As String = "precisionOfLength"
Public Const unitOfAngles_             As String = "unitOfAngles"
Public Const precisionOfAngles_        As String = "precisionOfAngles"
Public Const keyContext_               As String = "keyContext"
Public Const xsi_                      As String = "xmlns:xsi"
Public Const schemaLocation_           As String = "xsi:noNamespaceSchemaLocation"
Public Const id_                       As String = "id"
Public Const extRef_                   As String = "extRef"
Public Const face1_                    As String = "face1"
Public Const face2_                    As String = "face2"
Public Const startVertex_              As String = "startVertex"
Public Const endVertex_                As String = "endVertex"
Public Const radius_                   As String = "radius"
Public Const minorMajorRatio_          As String = "minorMajorRatio"
Public Const halfAngle_                As String = "halfAngle"
Public Const expanding_                As String = "expanding"
Public Const minorRadius_              As String = "minorRadius"
Public Const majorRadius_              As String = "majorRadius"
Public Const name_                     As String = "name"
Public Const orderU_                   As String = "orderU"
Public Const orderV_                   As String = "orderV"
Public Const rational_                 As String = "rational"
Public Const closedU_                  As String = "closedU"
Public Const closedV_                  As String = "closedV"
Public Const planar_                   As String = "planar"
Public Const n_                        As String = "n"
Public Const m_                        As String = "m"
Public Const loopID_                   As String = "loopID"
Public Const outer_                    As String = "outer"
Public Const edgeID_                   As String = "edgeID"
Public Const sweepAngle_               As String = "sweepAngle"
Public Const order_                    As String = "order"
Public Const closed_                   As String = "closed"
Public Const val_                      As String = "val"

Public Const indexOfKnotsOfCurve_      As String = "i"
Public Const indexOfKnotsU_            As String = "i"
Public Const indexOfKnotsV_            As String = "j"
Public Const numKnotsOfCurve_          As String = "n"
Public Const numberOfKnotsU_           As String = "n"
Public Const numberOfKnotsV_           As String = "m"


' Standard-Attribut-Werte
' =======================
Public Const xsi_val_                  As String = "http://www.w3.org/2001/XMLSchema-instance"
Public Const nameOfKnotsU_             As String = "KnotsU"
Public Const nameOfKnotsV_             As String = "KnotsV"







End Module