Option Explicit On

Imports Inventor
Imports System.Collections.Generic

'##############################################
' PART CLASS
'##############################################

Public Class CircuitPart

    Private OccHeight As Double = 0.04
    Private oTransformation As Matrix

    ' import attributes
    Private _id As String
    Private _x As Double
    Private _y As Double
    Private _z As Double
    Private _dev As String
    Private _value As String

    ' other attributes
    Private _isPlaced As Boolean
    Private _faceId As String
    Private _isValid As Boolean

    Private oAddIn As Inventor.Application
    Private oServer As MidAddInServer

    Private oPinList As New List(Of CircuitPin)
    Private oParent As CircuitBoard
    Private oOccurrence As ComponentOccurrence

    Private oPreviewPartNode As GraphicsNode

    Private oSurfaceBodyIntern As SurfaceBody
    Private oSurfaceBody As SurfaceBody
    Private oCircuitPartNode As BrowserNode

    Private strPartFolderPath As String

    ' Strucktur PackageShape
    ' ##########################################################################
    Private Structure PackageShape
        Private _width As Double
        Private _length As Double
        Private _origin As String
        Private _figure As String

        Public Property Width As Double
            Get
                Return _width
            End Get
            Set(value As Double)
                _width = value
            End Set
        End Property

        Public Property Length As Double
            Get
                Return _length
            End Get
            Set(value As Double)
                _length = value
            End Set
        End Property

        Public Property Origin As String
            Get
                Return _origin
            End Get
            Set(value As String)
                _origin = value
            End Set
        End Property

        Public Property Figure As String
            Get
                Return _figure
            End Get
            Set(value As String)
                _figure = value
            End Set
        End Property

    End Structure
    '###########################################################################

    Private oPackageShape As PackageShape

    ' Constructor
    '*****************************************************************************************************************************
    Public Sub New(ByVal oAddIn As Inventor.Application, _
                   ByVal oServer As MidAddInServer, _
                   ByVal oParent As CircuitBoard, _
                   ByVal dev As String, _
                   ByVal value As String, _
                   ByVal id As String, _
                   ByVal x As Double, _
                   ByVal y As Double, _
                   ByVal z As Double, _
                   ByVal width As Double, _
                   ByVal length As Double, _
                   ByVal origin As String, _
                   ByVal figure As String)

        MyBase.New()

        Me.oServer = oServer
        Me.oAddIn = oAddIn
        Me.oParent = oParent
        Me.oPackageShape = New PackageShape()

        ' import data
        _dev = dev
        _value = value
        _id = id
        _x = x
        _y = y
        _z = z

        oPackageShape.Width = width
        oPackageShape.Length = length
        oPackageShape.Origin = origin
        oPackageShape.Figure = figure

        ' other attributes
        oPinList = New List(Of CircuitPin)
        oOccurrence = Nothing

        oSurfaceBody = Nothing
        oSurfaceBodyIntern = Nothing

        ' Set position relative to the circuitboard
        SetTransformation()

    End Sub

    ' Delete all board data
    '*******************************************************************************************************************************
    Public Sub Delete()

        If oSurfaceBody IsNot Nothing Then
            'oSurfaceBody.Delete()
            oSurfaceBody = Nothing
        End If
        If oPreviewPartNode IsNot Nothing Then
            oPreviewPartNode.Delete()
            oPreviewPartNode = Nothing
        End If

        If oPinList IsNot Nothing Then
            oPinList.Clear()
            oPinList = Nothing
        End If
        If oSurfaceBodyIntern IsNot Nothing Then
            'oSurfaceBodyIntern.Delete()
            oSurfaceBodyIntern = Nothing
        End If
        If oOccurrence IsNot Nothing Then
            oOccurrence.Delete()
            oOccurrence = Nothing
        End If
        If oTransformation IsNot Nothing Then
            oTransformation = Nothing
        End If
        oParent = Nothing
        oPackageShape = Nothing
        oAddIn.ActiveView.Update()

    End Sub

    '#######################################################
    '   GEOMETRY
    '#######################################################

    ' Set the position of all part related components (part, pins)
    '*****************************************************************************************************************************
    Public Sub SetTransformation()
        ' Get current assembly document
        Dim oTG As TransientGeometry = oAddIn.TransientGeometry
        ' Calculate relative position
        'oTG.CreateMatrix()
        ' Get the position of the board
        Dim oRelVector As Vector = oTG.CreateVector() 'oTG.CreateVector(oParent.Transformation.Cell(1, 4), oParent.Transformation.Cell(2, 4), oParent.Transformation.Cell(3, 4))
        ' Substract half of the width and length of the board
        oRelVector.AddVector(oTG.CreateVector(-oParent.Length / 2, -oParent.Width / 2, 0))
        oRelVector.AddVector(oTG.CreateVector(Length / 2, Width / 2, 0))
        ' add the relative Position, change y and z value here!
        oRelVector.AddVector(oTG.CreateVector(X, Y, Z))

        ' multiply by transformation matrix
        oRelVector.TransformBy(oParent.Transformation)
        ' Add transformed vector to board position
        oTransformation = oParent.Transformation
        oTransformation.Cell(1, 4) += oRelVector.X
        oTransformation.Cell(2, 4) += oRelVector.Y
        oTransformation.Cell(3, 4) += oRelVector.Z

        ' set transformation of pins related to this part
        For Each oPin As CircuitPin In oPinList
            oPin.SetTransformation()
        Next
    End Sub

    '#######################################################
    ' CLIENT GRAPHICS
    '#######################################################

    ' Create client preview graphics (part)
    '*****************************************************************************************************************************
    Public Sub CreatePreviewGraphics(interactionEvents As InteractionEvents, _
                                    Optional style As String = "Default", _
                                    Optional folderPath As String = Nothing)

        If oOccurrence Is Nothing Then

            ' Create box from part data
            Dim minPoint(2) As Double
            Dim maxPoint(2) As Double
            minPoint(0) = -oPackageShape.Length / 2 : minPoint(1) = -oPackageShape.Width / 2 : minPoint(2) = 0
            maxPoint(0) = oPackageShape.Length / 2 : maxPoint(1) = oPackageShape.Width / 2 : maxPoint(2) = 0.04
            Dim oBox As Box = oAddIn.TransientGeometry.CreateBox()
            oBox.PutBoxData(minPoint, maxPoint)

            ' Try to load part from file, otherwise create box
            Try
                Dim strPartFilePath As String = folderPath & "\" & _dev & ".ipt"
                ' Load surface body from part
                oAddIn.SilentOperation = True
                Dim oPartDoc As PartDocument = oAddIn.Documents.Open(strPartFilePath, False)
                Dim oCompDef As PartComponentDefinition = oPartDoc.ComponentDefinition
                oSurfaceBody = oCompDef.SurfaceBodies.Item(1)
                'oPartDoc.Close()
                oAddIn.SilentOperation = False
            Catch ex As Exception
                oSurfaceBody = oAddIn.TransientBRep.CreateSolidBlock(oBox)
            End Try

            oSurfaceBodyIntern = oAddIn.TransientBRep.CreateSolidBlock(oBox)

        Else
            oSurfaceBody = oAddIn.TransientBRep.Copy(oOccurrence.SurfaceBodies.Item(1))

        End If

        ' Get the interaction graphics object
        Dim oInteractionGraphics As InteractionGraphics = interactionEvents.InteractionGraphics
        Dim oPreviewGraphics As ClientGraphics = oInteractionGraphics.PreviewClientGraphics

        ' Add a new node
        oPreviewPartNode = oPreviewGraphics.AddNode(2)

        ' Set position
        oPreviewPartNode.Transformation = oTransformation

        ' Create surface graphics object
        Dim oPreviewSurfaceGraphics As SurfaceGraphics = oPreviewPartNode.AddSurfaceGraphics(oSurfaceBody)
        oPreviewSurfaceGraphics.DepthPriority = 3
        oPreviewPartNode.Visible = True

        ' Set the render style object
        Try
            Dim oAsmDoc As AssemblyDocument = oAddIn.ActiveDocument()
            Dim oStyle As RenderStyle = oAsmDoc.RenderStyles.Item(style)
            oPreviewPartNode.RenderStyle = oStyle
        Catch ex As Exception
            'System.Windows.Forms.MessageBox.Show("Could not find 'Clear'-asset in the asset library")
        End Try

        ' Create pin client graphics associated with the part
        For Each oPin As CircuitPin In oPinList
            oPin.CreatePreviewGraphics(interactionEvents)
        Next

        oAddIn.ActiveView.Update()
    End Sub

    ' Delete preview graphics
    '*****************************************************************************************************************************
    Public Sub DeletePreviewGraphics()

        If oPreviewPartNode IsNot Nothing Then
            oPreviewPartNode.Delete()
            oPreviewPartNode = Nothing

            For Each oPin As CircuitPin In oPinList
                oPin.DeleteClientGraphics()
            Next
        End If
        oAddIn.ActiveView.Update()
    End Sub

    ' Update preview graphics
    '*****************************************************************************************************************************
    Public Sub UpdatePreviewGraphics()
        oPreviewPartNode.Transformation = oTransformation
        For Each oPin As CircuitPin In oPinList
            oPin.UpdateClientGraphics()
        Next
        oAddIn.ActiveView.Update()
    End Sub

    ' Create net preview graphics
    '*****************************************************************************************************************************
    Public Sub CreateNetClientGraphics(interactionEvents As InteractionEvents)

        For Each oNet As CircuitNet In oParent.NetList
            If oNet.Contains(Me) Then
                oNet.CreatePreviewGraphics(interactionEvents)
            End If
        Next

    End Sub

    ' Update net preview graphics
    '*****************************************************************************************************************************
    Public Sub UpdateNetClientGraphics()
        For Each oNet As CircuitNet In oParent.NetList
            If oNet.Contains(Me) Then
                oNet.UpdatePreviewGraphics()
            End If
        Next
    End Sub

    ' Delete net preview graphics
    '*****************************************************************************************************************************
    Public Sub DeleteNetClientGraphics()

        For Each oNet In oParent.NetList
            If oNet.Contains(Me) Then
                oNet.DeletePreviewGraphics()
            End If
        Next

    End Sub

    '#######################################################
    ' Net related methods: client graphics
    '#######################################################

    ' Update NetLines
    '*****************************************************************************************************************************
    Public Sub UpdateNetLines()
        For Each oNet As CircuitNet In oParent.NetList
            If oNet.Contains(Me) Then
                oNet.RecalculateIRep()
            End If
        Next
    End Sub

    ' Create part occurrence (visual representation)
    '*****************************************************************************************************************************
    Public Sub CreateIRep()

        ' Add new part document
        Dim oPartDoc As PartDocument = oAddIn.Documents.Add(DocumentTypeEnum.kPartDocumentObject, , False)
        Dim oCompDef As PartComponentDefinition = oPartDoc.ComponentDefinition
        Dim oBaseFeature As NonParametricBaseFeature = oCompDef.Features.NonParametricBaseFeatures.Add(oSurfaceBody)

        ' Add a name (shown on the folderbrowser)
        oPartDoc.DisplayName = _dev

        ' Load occurrence from part document
        oOccurrence = oAddIn.ActiveDocument.ComponentDefinition.Occurrences.AddByComponentDefinition(oCompDef, oTransformation)

        ' close document
        oPartDoc.Close(True)

        ' Insert circuit part in the mid-browser
        oCircuitPartNode = oServer.Browser.CreateNode(, oParent.BrowserNode, oOccurrence)

        ' Create visual representation for Pin (workpoint)
        For Each oPin As CircuitPin In PinList
            oPin.CreateIRep()
        Next

    End Sub

    ' Reposition the occurrence
    '****************************************************************************************************************
    Public Sub UpdateOccurrence()
        oOccurrence.Transformation = oTransformation
        For Each oPin As CircuitPin In oPinList
            oPin.UpdateIRep()
        Next
    End Sub

    '#######################################################
    ' Pin related methods
    '#######################################################

    ' Add Pin to PinList
    '****************************************************************************************************************
    Public Sub AddPin(oPin As CircuitPin)
        oPinList.Add(oPin)
    End Sub

    Public Function GetPreviewGraphicsEdgeVector() As Vector

        'Try
        Dim oPreviewSurfaceGraphics As SurfaceGraphics = oPreviewPartNode.Item(1)
        Dim oEdge As Edge = oPreviewSurfaceGraphics.DisplayedEdges.Item(1).Edge

        Dim oStartVertex As Vertex = oEdge.StartVertex
        Dim oStopVertex As Vertex = oEdge.StopVertex

        Dim startPoint(2) As Double
        Dim stopPoint(2) As Double

        oStartVertex.GetPoint(startPoint)
        oStopVertex.GetPoint(stopPoint)

        Return oAddIn.TransientGeometry.CreateVector(startPoint(0) - stopPoint(0), startPoint(1) - stopPoint(1), startPoint(2) - stopPoint(2))
        ' Catch Ex As Exception
        Return oAddIn.TransientGeometry.CreateVector()
        'End Try
    End Function

    ' Find pin by Id
    '****************************************************************************************************************
    Public ReadOnly Property GraphicsNode As GraphicsNode
        Get
            Return Me.oPreviewPartNode
        End Get

    End Property

    Public Function FindPinById(ByVal _pinId As String) As CircuitPin
        For Each oPin In PinList
            If oPin.Id.Equals(_pinId) Then
                Return oPin
            End If
        Next
        Return Nothing
    End Function

    Public Property FaceId As String
        Get
            Return _faceId
        End Get
        Set(value As String)
            _faceId = value
        End Set
    End Property

    Public Property IsValid As Boolean
        Get
            Return _isValid
        End Get
        Set(value As Boolean)
            _isValid = value
        End Set
    End Property

    Public Property IsPlaced As Boolean
        Get
            Return _isPlaced
        End Get
        Set(value As Boolean)
            _isPlaced = value
        End Set
    End Property




    Public Property Dev As String
        Get
            Return _dev
        End Get
        Set(value As String)
            _dev = value
        End Set
    End Property

    Public Property Value As String
        Get
            Return _value
        End Get
        Set(value As String)
            _value = value
        End Set
    End Property

    ' Update part and pin position
    Public Property Transformation() As Matrix
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

    Public Property Z As Double
        Get
            Return _z
        End Get
        Set(value As Double)
            Me._z = value
        End Set
    End Property

    'Public Property Shape As PackageShape
    '    Get
    '        Return oPackageShape
    '    End Get
    '    Set(value As PackageShape)
    '        Me.oPackageShape = value
    '    End Set
    'End Property

    Public Property Length As Double
        Get
            Return oPackageShape.Length
        End Get
        Set(value As Double)
            Me.oPackageShape.Length = value
        End Set
    End Property

    Public Property Width As Double
        Get
            Return oPackageShape.Width
        End Get
        Set(value As Double)
            Me.oPackageShape.Width = value
        End Set
    End Property

    Public Property Origin As String
        Get
            Return oPackageShape.Origin
        End Get
        Set(value As String)
            Me.oPackageShape.Origin = value
        End Set
    End Property

    Public Property Figure As String
        Get
            Return oPackageShape.Figure
        End Get
        Set(value As String)
            Me.oPackageShape.Figure = value
        End Set
    End Property

    Public ReadOnly Property Parent As CircuitBoard
        Get
            Return oParent
        End Get
    End Property



    Public ReadOnly Property PinList As List(Of CircuitPin)
        Get
            Return oPinList
        End Get
    End Property

    ' Return occurrence
    '****************************************************************************************************************
    Public ReadOnly Property Occurrence As ComponentOccurrence
        Get
            Return oOccurrence
        End Get
    End Property



    Public Function GetWidth() As Double
        Return Me.Width
    End Function



    'Public Sub SetTransVector(X As Double, Y As Double, Z As Double)

    '    Me.oPositionVector = oAddIn.TransientGeometry.CreateVector(X, Y, Z)
    'End Sub

    'Public Function GetTransVector() As Vector
    '    Return Me.oPositionVector
    'End Function

    ' create a new position matrix for the board


    '' return the position matrix of the part
    'Public Function GetPosMatrix() As Matrix
    '    Return Me.oPartOcc.Transformation()
    'End Function



    'Dim oOccAttribSets As AttributeSets = oOccurrence.AttributeSets
    'Dim oOccAttribSet As AttributeSet = oOccAttribSets.Add("part")
    'Dim oOccAttrib As Attribute = oOccAttribSet.Add("part", ValueTypeEnum.kIntegerType, 2)
    'MsgBox(oOccAttribSet.NameIsUsed("part"))


End Class