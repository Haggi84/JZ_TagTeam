Option Explicit On

'##############################################
' PIN CLASS
'##############################################

Public Class CircuitPin

    Private _id As String
    Private _x As Double
    Private _y As Double

    Private oAddIn As Inventor.Application
    Private oParent As CircuitPart

    Private oPreviewPinNode As GraphicsNode
    Private oPinNode As GraphicsNode
    Private oTransformation As Matrix

    Private oServer As MidAddInServer
    Dim oPointCoordSet As GraphicsCoordinateSet
    Dim oPointGraphics As PointGraphics

    Private oPinCoordSet As GraphicsCoordinateSet
    Private oPinPointSet As PointGraphics

    Private oPointCoord(2) As Double

    Private oWorkPoint As WorkPoint

    ' Constructor
    '*****************************************************************************************************************************
    Public Sub New(ByVal oAddIn As Inventor.Application, _
                   ByVal oServer As MidAddInServer, _
                   ByVal oParent As CircuitPart, _
                   ByVal id As String, _
                   ByVal x As Double, _
                   ByVal y As Double)

        MyBase.New()
        Me.oAddIn = oAddIn
        Me.oParent = oParent
        Me.oServer = oServer

        _id = id
        _x = x
        _y = y

        SetTransformation()

    End Sub

    ' Delete all board data
    '*******************************************************************************************************************************
    Public Sub Delete()

        If oPreviewPinNode IsNot Nothing Then
            oPreviewPinNode.Delete()
            oPreviewPinNode = Nothing
        End If

        If oPinNode IsNot Nothing Then
            oPinNode.Delete()
            oPinNode = Nothing
        End If

        If oPreviewPinNode IsNot Nothing Then
            oPreviewPinNode.Delete()
            oPreviewPinNode = Nothing
        End If

        If oPinCoordSet IsNot Nothing Then
            oPinCoordSet.Delete()
            oPinCoordSet = Nothing
        End If

        If oPinCoordSet IsNot Nothing Then
            oPinCoordSet.Delete()
            oPinCoordSet = Nothing
        End If

        oTransformation = Nothing

        oAddIn.ActiveView.Update()

    End Sub

    '#######################################################
    ' VISUAL REPRESENTATION
    '#######################################################

    ' Set transformation (position)
    '*****************************************************************************************************************************
    Public Sub SetTransformation()

        Debug.WriteLine("pin ID: " & _id)
        Debug.WriteLine("x = " & _x & "y = " & _y)
        Debug.WriteLine("part ID: " & oParent.Id)
        Debug.WriteLine("_________________________________________________________________________________________________")

        oTransformation = oParent.Transformation
        Dim oRelVector As Vector = oAddIn.TransientGeometry.CreateVector(-oParent.Length / 2 + _x, -oParent.Width / 2 + _y, 0)
        oRelVector.TransformBy(oParent.Transformation)
        oTransformation.Cell(1, 4) += oRelVector.X
        oTransformation.Cell(2, 4) += oRelVector.Y
        oTransformation.Cell(3, 4) += oRelVector.Z

    End Sub

    ' Create point
    '*****************************************************************************************************************************
    Public Sub CreateIRep()


        Dim oAsmDoc As AssemblyDocument = oAddIn.ActiveDocument()
        Dim oPoint As Point = oAddIn.TransientGeometry.CreatePoint(oTransformation.Cell(1, 4), oTransformation.Cell(2, 4), oTransformation.Cell(3, 4))
        oWorkPoint = oAsmDoc.ComponentDefinition.WorkPoints.AddFixed(oPoint)

      
        'Dim oCompDef As ComponentDefinition = oAddIn.ActiveDocument.ComponentDefinition

        '' Get the graphics data sets collection
        'Dim oAsmDoc As Document = oAddIn.ActiveDocument
        'Dim oDataSets As GraphicsDataSets = oAsmDoc.GraphicsDataSetsCollection.Add(oParent.Id & "_" & _id)

        '' Add the coordinate set
        'oPinCoordSet = oDataSets.CreateCoordinateSet(1)

        '' Put the coordinates
        ''Dim oPointCoord(2) As Double
        'oPointCoord(0) = oTransformation.Cell(1, 4) : oPointCoord(1) = oTransformation.Cell(2, 4) : oPointCoord(2) = oTransformation.Cell(3, 4)
        'oPinCoordSet.PutCoordinates(oPointCoord)

        '' Create the client graphics
        'Dim oClientGraphics As ClientGraphics = oCompDef.ClientGraphicsCollection.Add(oParent.Id & "_" & _id)

        '' Add the node
        'oPinNode = oClientGraphics.AddNode(1)
        'oPinPointSet = oPinNode.AddPointGraphics
        'oPinPointSet.CoordinateSet = oPinCoordSet

        '' oPinPointSet.PointRenderStyle = PointRenderStyleEnum.kCrossPointStyle
        'oAddIn.ActiveView.Update()

    End Sub

    ' Update point
    '*****************************************************************************************************************************
    Public Sub UpdateIRep()
        oWorkPoint.SetFixed(oAddIn.TransientGeometry.CreatePoint(oTransformation.Cell(1, 4), oTransformation.Cell(2, 4), oTransformation.Cell(3, 4)))
        'Dim oPointCoord(2) As Double
        'oPointCoord(0) = oTransformation.Cell(1, 4) : oPointCoord(1) = oTransformation.Cell(2, 4) : oPointCoord(2) = oTransformation.Cell(3, 4)
        'oPinCoordSet.PutCoordinates(oPointCoord)
        'oPinPointSet.CoordinateSet = oPinCoordSet
    End Sub


    '#######################################################
    ' PREVIEW GRAPHICS
    '#######################################################

    ' Add preview graphics 
    '*****************************************************************************************************************************
    Public Sub CreatePreviewGraphics(oInteractionEvents As InteractionEvents)

        ' Create new client graphics
        Dim oInteractionGraphics As InteractionGraphics = oInteractionEvents.InteractionGraphics
        Dim oPreviewGraphics As ClientGraphics = oInteractionGraphics.PreviewClientGraphics
        Dim oPreviewDataSets As GraphicsDataSets = oInteractionGraphics.GraphicsDataSets

        ' Add Node 
        oPreviewPinNode = oPreviewGraphics.AddNode(3)

        ' Add coordinate set
        oPointCoordSet = oPreviewDataSets.CreateCoordinateSet(1)

        ' Put coordinates
        'Dim oPointCoord(2) As Double
        oPointCoord(0) = 0 : oPointCoord(1) = 0 : oPointCoord(2) = 0
        oPointCoordSet.PutCoordinates(oPointCoord)

        ' Add point graphics
        oPointGraphics = oPreviewPinNode.AddPointGraphics
        oPointGraphics.CoordinateSet = oPointCoordSet
        oPointGraphics.PointRenderStyle = PointRenderStyleEnum.kOnCurvePointStyle
        oPointGraphics.BurnThrough = True

        ' Update position
        oPreviewPinNode.Transformation = oTransformation

        'oAddIn.ActiveView.Update()

    End Sub

    ' Update preview graphics
    '*****************************************************************************************************************************
    Public Sub UpdateClientGraphics()
        oPreviewPinNode.Transformation = oTransformation
        'oAddIn.ActiveView.Update()
    End Sub

    ' Delete pin preview graphics
    '*****************************************************************************************************************************
    Public Sub DeleteClientGraphics()
        oPreviewPinNode.Delete()
        oPreviewPinNode = Nothing
        'oAddIn.ActiveView.Update()
    End Sub

    '#######################################################
    ' PROPERTIES
    '#######################################################

    Public Property Transformation As Matrix
        Get
            Return oTransformation.Copy
        End Get
        Set(value As Matrix)
            oTransformation = value.Copy
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

End Class


'Public Property X As Double
'    Get
'        Return _x
'    End Get
'    Set(value As Double)
'        Me._x = value
'    End Set
'End Property

'Public Property Y As Double
'    Get
'        Return _y
'    End Get
'    Set(value As Double)
'        Me._y = value
'    End Set
'End Property

'oTransformation = oParent.Transformation
'Dim oPosVector As Vector = oAddIn.TransientGeometry.CreateVector(-oParent.Length / 2 + _x, -oParent.Width / 2, 0 + _y)
''oPosVector.AddVector(oAddIn.TransientGeometry.CreateVector(-oParent.Length / 2 + _x, -oParent.Width / 2, 0 + _y))
'oPosVector.TransformBy(oParent.Transformation)

''oPosVector.AddVector(oPosVector)
'oTransformation.Cell(1, 4) += oPosVector.X
'oTransformation.Cell(2, 4) += oPosVector.Y
'oTransformation.Cell(3, 4) += oPosVector.Z

''oPreviewPinNode.Transformation = oTransformation
'For i As Integer = 1 To 4
'    For j As Integer = 1 To 4
'        Debug.Write(oPreviewPinNode.Transformation.Cell(i, j))
'    Next
'    Debug.WriteLine("")
'Next
'Dim oTransVector As Vector = oAddIn.TransientGeometry.CreateVector(oParent.Transformation.Cell(1, 4), oParent.Transformation.Cell(2, 4), oParent.Transformation.Cell(3, 4))
'Dim oPosVector As Vector = oAddIn.TransientGeometry.CreateVector()
'oPosVector.AddVector(oAddIn.TransientGeometry.CreateVector(-oParent.Length / 2, -oParent.Width / 2, 0))
'oPosVector.AddVector(oAddIn.TransientGeometry.CreateVector(X, Y, 0))
'oPosVector.TransformBy(oParent.Transformation)


'oPosVector.AddVector(oTransVector)



'Dim oPointCoord(2) As Double
'oPointCoord(0) = 0 : oPointCoord(1) = 0 : oPointCoord(2) = 0
'oPosition = oAddIn.TransientGeometry.CreatePoint(oPosVector.X, oPosVector.Y, oPosVector.Z)
' oPointCoordSet.PutCoordinates(oPointCoord)
'oPointGraphics.CoordinateSet = oPointCoordSet

'Dim oMatrix As Matrix = oParent.Transformation

'oMatrix.Cell(1, 4) += oPosVector.X
'oMatrix.Cell(2, 4) += oPosVector.Y
'oMatrix.Cell(3, 4) += oPosVector.Z

'oPreviewPinNode.Transformation = oMatrix

'For i As Integer = 1 To 4
'    For j As Integer = 1 To 4
'        Debug.Write(oPreviewPinNode.Transformation.Cell(i, j))
'    Next
'    Debug.WriteLine("")
'Next

'Dim oMatrix As Matrix = oAddIn.TransientGeometry.CreateMatrix()
'oMatrix.Cell(1, 4) = Position.X
'oMatrix.Cell(2, 4) = Position.Y
'oMatrix.Cell(3, 4) = Position.Z
'oMatrix.SetTranslation(oAddIn.TransientGeometry.CreateVector(Position.X, Position.Y, Position.Z))

'oPreviewPinNode.Transformation = oMatrix
' Recalculate pin position
'Dim oTransVector As Vector = oAddIn.TransientGeometry.CreateVector(oParent.Transformation.Cell(1, 4), oParent.Transformation.Cell(2, 4), oParent.Transformation.Cell(3, 4))
'Dim oPosVec As Vector = oAddIn.TransientGeometry.CreateVector(Y, 0, X)
'oPosVec.TransformBy(oParent.Transformation)
'oPosVec.AddVector(oTransVector)
' Make recalculated pin positin visible

' Create client graphics
'Dim oInteractionGraphics As InteractionGraphics = oInteractionEvents.InteractionGraphics

'' Create new client graphics
'Dim oClientGraphics As ClientGraphics = oInteractionGraphics.PreviewClientGraphics
''Dim oClientGraphics As ClientGraphics = oAsmDoc.ComponentDefinition.ClientGraphicsCollection.Add(oServer.ClientId)
'' Add Node for occurrence copy
'Dim oOccNode As GraphicsNode = oClientGraphics.AddNode(1)


'oPoint = oAddIn.TransientGeometry.CreatePoint(oPosVec.X, oPosVec.Y, oPosVec.Z)

'oWorkPoint.SetFixed(oPoint)


'oTransformation = oParent.Transformation
'oTransformation.Cell(1, 4) = oTransformation.Cell(1, 4) - oParent.Length / 2 + _x    
'oTransformation.Cell(2, 4) = oTransformation.Cell(2, 4) - oParent.Width / 2 + _y
'Debug.WriteLine("Transformation part " & _id & " : x =" & oParent.Transformation.Cell(1, 4) & " y = " & oParent.Transformation.Cell(2, 4) & " z = " & oParent.Transformation.Cell(3, 4))
'Debug.WriteLine("oVektor " & _id & " : x =" & oVector.X & " y = " & oVector.Y & " z = " & oVector.Z)

' Debug.WriteLine("Transformation before " & _id & " : x =" & oTransformation.Cell(1, 4) & " y = " & oTransformation.Cell(2, 4) & " z = " & oTransformation.Cell(3, 4))
'Dim oRelVector As Vector = oAddIn.TransientGeometry.CreateVector(oParent.Transformation.Cell(1, 4), oParent.Transformation.Cell(2, 4), oParent.Transformation.Cell(3, 4))
'oRelVector.AddVector(oAddIn.TransientGeometry.CreateVector(-oParent.Length / 2, -oParent.Width / 2, 0))
'oRelVector.AddVector(oAddIn.TransientGeometry.CreateVector(X, Y, 0))
'oPosition = oAddIn.TransientGeometry.CreatePoint(oRelVector.X, oRelVector.Y, oRelVector.Z)
'Debug.WriteLine("Point before " & _id & " : x =" & oPoint.X & " y = " & oPoint.Y & " z = " & oPoint.Z)
' CreateClientGraphics(oInteractionEvents)