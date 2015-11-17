Option Explicit On

'##############################################
' PIN CLASS
'##############################################

Public Class CircuitPin

    Private _id As String
    Private _x As Double
    Private _y As Double

    Private oAddIn As Inventor.Application
    Private oPart As CircuitPart
    Private oWorkPoint As WorkPoint
    Private oPoint As Point

    ' Constructor
    Public Sub New(ByRef oAddIn As Inventor.Application, ByRef oPart As CircuitPart)

        MyBase.New()
        Me.oAddIn = oAddIn
        Me.oPart = oPart

    End Sub

    ' Activate Pin
    '*****************************************************************************************************************************
    Public Sub Initialize()

        Dim oAsmDoc As AssemblyDocument = oAddIn.ActiveDocument

        Dim oRelVector As Vector = oAddIn.TransientGeometry.CreateVector(oPart.Transformation.Cell(1, 4), oPart.Transformation.Cell(2, 4), oPart.Transformation.Cell(3, 4))
        oRelVector.AddVector(oAddIn.TransientGeometry.CreateVector(-oPart.Length / 2, -oPart.Width / 2, 0))
        oRelVector.AddVector(oAddIn.TransientGeometry.CreateVector(X, Y, 0))


        'Create point and workpoint(visible representation)
        oPoint = oAddIn.TransientGeometry.CreatePoint(oRelVector.X, oRelVector.Y, oRelVector.Z)
        oWorkPoint = oAsmDoc.ComponentDefinition.WorkPoints.AddFixed(oPoint)

    End Sub

    ' Update Pin Position
    '*****************************************************************************************************************************
    Public Sub Update()

        ' Recalculate pin position
        Dim oTransVector As Vector = oAddIn.TransientGeometry.CreateVector(oPart.Transformation.Cell(1, 4), oPart.Transformation.Cell(2, 4), oPart.Transformation.Cell(3, 4))
        Dim oPosVec As Vector = oAddIn.TransientGeometry.CreateVector(Y, 0, X)
        oPosVec.TransformBy(oPart.Transformation)
        oPosVec.AddVector(oTransVector)
        ' Make recalculated pin positin visible
        oPoint = oAddIn.TransientGeometry.CreatePoint(oPosVec.X, oPosVec.Y, oPosVec.Z)

        oWorkPoint.SetFixed(oPoint)

    End Sub

    ' Properties
    '*****************************************************************************************************************************
    Public Property Point As Point
        Get
            Return oPoint
        End Get
        Set(value As Point)
            oPoint = value
        End Set
    End Property

    Public Property Id As String
        Get
            Return _id
        End Get
        Set(value As String)
            Me._id = value
        End Set
    End Property

    Public Property X As Double
        Get
            Return _x
        End Get
        Set(value As Double)
            Me._x = value
        End Set
    End Property

    Public Property Y As Double
        Get
            Return _y
        End Get
        Set(value As Double)
            Me._y = value
        End Set
    End Property

End Class