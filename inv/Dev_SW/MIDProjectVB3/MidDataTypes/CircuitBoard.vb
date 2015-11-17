Imports Inventor
Imports System.Xml
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports System.Collections

'##############################################
' CIRCUIT BOARD CLASS
'##############################################

Public Class CircuitBoard

    Private strFilePath As String

    Private _width As Double
    Private _length As Double
    Private _id As String
    Private _denotion As String
    Private _unitOfLength As String

    Private offset As Double = 4
    Private oTransformation As Matrix
    Private oOccurrence As ComponentOccurrence

    Private oAddIn As Inventor.Application
    Private oServer As MidAddInServer

    Private oPartList As List(Of CircuitPart)
    Private oNetList As New List(Of CircuitNet)

    Private oCircuitPartsNode As BrowserNode

    Private oBoardNode As GraphicsNode
    Private oSurfaceBody As SurfaceBody

    ' Constructor
    '*******************************************************************************************************************************
    Public Sub New(ByVal addIn As Inventor.Application, _
                   ByVal server As MidAddInServer, _
                   ByVal transformation As Matrix)

        MyBase.New()

        Me.oAddIn = addIn
        Me.oServer = server

        oBoardNode = Nothing
        oSurfaceBody = Nothing

        oPartList = New List(Of CircuitPart)

        oTransformation = transformation

    End Sub

    ' Delete all board data
    '*******************************************************************************************************************************
    Public Sub Delete()

        If oBoardNode IsNot Nothing Then
            oBoardNode.Delete()
            oBoardNode = Nothing
        End If
        If oSurfaceBody IsNot Nothing Then
            oSurfaceBody.Delete()
            oSurfaceBody = Nothing
        End If
        If oPartList IsNot Nothing Then
            For Each oPart As CircuitPart In oPartList
                oPart.Delete()
                oPart = Nothing
            Next
            oPartList.Clear()
            oPartList = Nothing
        End If
        If oNetList IsNot Nothing Then
            oNetList.Clear()
            oNetList = Nothing
        End If
        If oOccurrence IsNot Nothing Then
            oOccurrence.Delete()
            oOccurrence = Nothing
        End If
        If oTransformation IsNot Nothing Then
            oTransformation = Nothing
        End If

        oAddIn.ActiveView.Update()

    End Sub

    '#######################################################
    ' Preview graphics
    '#######################################################

    ' Update preview client graphics
    '*********************************************************************************************************************
    Public Sub UpdatePreviewGraphics()

        oBoardNode.Transformation = oTransformation
        oAddIn.ActiveView.Update()
    End Sub

    ' Delete preview client graphics
    '*********************************************************************************************************************
    Public Sub DeletePreviewGraphics()
        oBoardNode.Delete()
        oBoardNode = Nothing
        oAddIn.ActiveView.Update()
    End Sub

    ' Create preview client graphics
    '*********************************************************************************************************************
    Public Sub CreatePreviewGraphics(ByVal interactionEvents As InteractionEvents, _
                                     ByVal style As String)

        ' Create Box
        Dim minPoint(2) As Double
        Dim maxPoint(2) As Double
        minPoint(0) = -Length / 2 : minPoint(1) = -Width / 2 : minPoint(2) = 0
        maxPoint(0) = Length / 2 : maxPoint(1) = Width / 2 : maxPoint(2) = 0.01

        Dim oBox As Box = oAddIn.TransientGeometry.CreateBox()
        oBox.PutBoxData(minPoint, maxPoint)
        oSurfaceBody = oAddIn.TransientBRep.CreateSolidBlock(oBox)

        ' Get interaction graphics object
        Dim oInteractionGraphics As InteractionGraphics = interactionEvents.InteractionGraphics
        Dim oClientGraphics As ClientGraphics = oInteractionGraphics.PreviewClientGraphics

        ' Add Node for occurrence copy
        oBoardNode = oClientGraphics.AddNode(1)

        ' set correct transformation
        oBoardNode.Transformation = oTransformation

        Dim oSurfaceGraphics As SurfaceGraphics = oBoardNode.AddSurfaceGraphics(oSurfaceBody)
        oSurfaceGraphics.DepthPriority = 3

        ' set material (transparent)
        Try
            Dim oAsmDoc As AssemblyDocument = oAddIn.ActiveDocument()
            Dim oStyle As RenderStyle = oAsmDoc.RenderStyles.Item(style)
            oBoardNode.RenderStyle = oStyle
        Catch ex As Exception
            System.Windows.Forms.MessageBox.Show("Could not find " & style & " -asset in the asset library")
        End Try

        ' update view to show new graphics object
        oAddIn.ActiveView.Update()

    End Sub

    '#######################################################
    ' Create BRep methods
    '#######################################################

    ' Add Net to netlist
    '*********************************************************************************************************************
    Public Sub AddNet(ByRef oNet As CircuitNet)
        oNetList.Add(oNet)
        'oNet.CreateClientGraphics(oInteractionEvents)
    End Sub

    ' Add part to partlist 
    '*********************************************************************************************************************
    Public Sub AddPart(ByRef oPart As CircuitPart)
        oPartList.Add(oPart)
    End Sub

    '#######################################################
    ' Other utility methods
    '#######################################################

    ' Get normal vector
    '*********************************************************************************************************************
    Public Function GetNormalVector() As Vector
        Dim oFace As Face = oSurfaceBody.Faces.Item(1)

        Dim oFaceMinPoint As Point = oFace.Evaluator.RangeBox.MinPoint
        'Dim oFaceMaxPoint As Point = oFace.Evaluator.RangeBox.MaxPoint

        Dim FaceMinPoint(2) As Double
        '*Dim FaceMaxPoint(2) As Double
        FaceMinPoint(0) = oFaceMinPoint.X
        FaceMinPoint(1) = oFaceMinPoint.Y
        FaceMinPoint(2) = oFaceMinPoint.Z

        ' Normal of the mid
        Dim FaceNormal(2) As Double
        oFace.Evaluator.GetNormalAtPoint(FaceMinPoint, FaceNormal)

        Return oAddIn.TransientGeometry.CreateVector(FaceNormal(0), FaceNormal(1), FaceNormal(2))

    End Function

    ' Get edge vector
    '*********************************************************************************************************************
    Public Function GetEdgeVector() As Vector

        Dim oEdge As Edge = oSurfaceBody.Faces.Item(1).Edges.Item(1)

        Dim oStartVertex As Vertex = oEdge.StartVertex
        Dim oStopVertex As Vertex = oEdge.StopVertex

        Dim startPoint(2) As Double
        Dim stopPoint(2) As Double

        oStartVertex.GetPoint(startPoint)
        oStopVertex.GetPoint(stopPoint)

        Return oAddIn.TransientGeometry.CreateVector(startPoint(0) - stopPoint(0), startPoint(1) - stopPoint(1), startPoint(2) - stopPoint(2))

    End Function

    Public Sub AddPartsNode()
        oCircuitPartsNode = oServer.Browser.CreateNode("CircuitParts", oServer.Commands.CircuitBoardNode)
    End Sub

    ' Set Transformation
    '*********************************************************************************************************************
    'Public Sub SetTransformation(ByVal oMatrix As Matrix)
    '    ' Position Matrix
    '    oTransformation = oMatrix.Copy()

    '    For Each oPart As CircuitPart In PartList
    '        oPart.SetTransformation(oTransformation)
    '    Next

    'End Sub

    ' Find part within partlist with id
    '*********************************************************************************************************************
    Public Function FindPartById(ByVal partId As String) As CircuitPart
        For Each _part In PartList
            If _part.Id.Equals(partId) Then
                Return _part
            End If
        Next
        Return Nothing
    End Function

    ' Find parent of part occurrence
    '*********************************************************************************************************************
    Public Function Parent(ByRef occurrence As ComponentOccurrence) As CircuitPart

        For Each oPart As CircuitPart In oPartList
            If oPart.Occurrence Is occurrence Then
                Return oPart
            End If
        Next
        Return Nothing
    End Function

    ' Check if face of sent occurrence is a circuit part
    '****************************************************************************************************************
    Public Function IsCircuitPart(ByVal oFace As Face) As Boolean
        For Each part As CircuitPart In oPartList
            For Each face As Face In part.Occurrence.SurfaceBodies.Item(1).Faces
                If face.InternalName.Equals(oFace.InternalName) Then
                    Return True
                End If
            Next
        Next

        Return False

    End Function

    ' Properties
    '*********************************************************************************************************************
    ' width Property (read only)

    Public ReadOnly Property BrowserNode As BrowserNode
        Get
            Return oCircuitPartsNode
        End Get
    End Property


    Public Property UnitOfLength As String
        Get
            Return _unitOfLength
        End Get
        Set(value As String)
            _unitOfLength = value
        End Set
    End Property

    Public Property Denotion As String
        Get
            Return _denotion
        End Get
        Set(value As String)
            _denotion = value
        End Set
    End Property

    Public Property Id() As String
        Get
            Return _id
        End Get
        Set(value As String)
            _id = value
        End Set
    End Property

    ' width Property (read only)
    Public Property Width() As Double
        Get
            Return _width
        End Get
        Set(value As Double)
            _width = value
        End Set
    End Property

    ' length property 
    Public Property Length() As Double
        Get
            Return _length
        End Get
        Set(value As Double)
            _length = value
        End Set
    End Property

    ' position property
    Public Property Transformation() As Matrix
        Get
            Return oTransformation.Copy
        End Get
        Set(value As Matrix)
            oTransformation = value.Copy
        End Set
    End Property

    ' Return occurrence
    Public ReadOnly Property Occurrence() As ComponentOccurrence
        Get
            Return oOccurrence
        End Get
    End Property

    ' Return the partlist of the board
    Public ReadOnly Property PartList As List(Of CircuitPart)
        Get
            Return oPartList
        End Get
    End Property

    Public ReadOnly Property NetList As List(Of CircuitNet)
        Get
            Return oNetList
        End Get
    End Property

    'Public Sub SetPosition(oPosMatrix As Matrix)
    '    Me.oPosMatrix = oPosMatrix
    '        oOccurrence.Transformation() = oPosMatrix
    'End Sub


    'Public Sub SetLength(ByVal length As Double)
    '    Me.Length = length
    'End Sub

    'Public Sub SetWidth(ByVal width As Double)
    '    Me._width = width
    'End Sub

    'Public Function GetLength() As Double
    '    Return _length
    'End Function

    'Public Function GetWidth() As Double
    '    Return _width
    'End Function


    Public Sub WriteXml(filePath As String)

        ' Exit if there are no parts on the board
        If oPartList.Count = 0 Then
            Exit Sub
        End If

        ' Try

        Dim oAsmDoc As AssemblyDocument = oAddIn.ActiveDocument

        'Dim filePath As String = oServer.CommandCollection.WorkDirectory 'NewProjectCommand.GetWorkDirPath()
        filePath = filePath & "\PartPositions.xml"

        ' Prepare KeepOutWriter for Garbage collector
        Using KeepOutWriter As XmlTextWriter = New XmlTextWriter(filePath, Nothing)

            ' Header / Start element
            '************************************************************************************
            KeepOutWriter.Formatting = Formatting.Indented

            KeepOutWriter.WriteStartDocument()

            KeepOutWriter.WriteStartElement("PartPositions")

            KeepOutWriter.WriteAttributeString("xsi:noNamespaceSchemaLocation", "filepath")
            KeepOutWriter.WriteAttributeString("xmlns:xsi", "http://www.w4.org/2001/XMLSchema-instance")

            ' Elements
            '************************************************************************************
           
            ' Write out all Attributes
            For i As Integer = 0 To oPartList.Count - 1

                Dim oPart As CircuitPart = oPartList.Item(i)

                KeepOutWriter.WriteStartElement("Part") ' Root

                KeepOutWriter.WriteAttributeString("isValid", "true")
                KeepOutWriter.WriteAttributeString("faceId", oPart.FaceId)
                KeepOutWriter.WriteAttributeString("isPlaced", oPart.IsPlaced)
                KeepOutWriter.WriteAttributeString("id", oPart.Id)

                KeepOutWriter.WriteStartElement("Pins")

                For j As Integer = 0 To oPart.PinList.Count - 1

                    Dim oPin As CircuitPin = oPart.PinList.Item(j)

                    KeepOutWriter.WriteStartElement("Pin")

                    KeepOutWriter.WriteAttributeString("x", oPin.Transformation.Cell(1, 4))
                    KeepOutWriter.WriteAttributeString("y", oPin.Transformation.Cell(2, 4))
                    KeepOutWriter.WriteAttributeString("z", oPin.Transformation.Cell(3, 4))
                    KeepOutWriter.WriteAttributeString("id", oPin.Id)

                    KeepOutWriter.WriteEndElement() ' Pin

                Next

                KeepOutWriter.WriteEndElement() ' Pins
                KeepOutWriter.WriteEndElement() ' Part
            Next

            KeepOutWriter.WriteEndElement() ' PartPositions

            KeepOutWriter.WriteEndDocument()

        End Using

        'Catch directoryNotFound As System.IO.DirectoryNotFoundException
        'MessageBox.Show("Directory has been deleted", "MID Project", MessageBoxButtons.OK, MessageBoxIcon.Error)
        'Catch ex As Exception
        'MessageBox.Show("There was an error writing to the specified location", "MID Project", MessageBoxButtons.OK, MessageBoxIcon.Error)
        ' End Try

    End Sub

End Class
' Attach Color
'Dim oBoardAsset As Assets = oAsmDoc.Assets
'Dim oAppearance As Asset = oBoardAsset.Add(AssetTypeEnum.kAssetTypeAppearance, _
'                                           "Generic", _
'                                           "GreyTransparentIntern", _
'                                           "GreyTransparent")
'Dim oColor As ColorAssetValue = oAppearance.Item("plastic")
'oColor.Value = oAddIn.TransientObjects.CreateColor(160, 160, 160, 0.1)

'Dim assetLib1 As AssetLibrary = oAddIn.AssetLibraries.Item("Autodesk Appearance Library")
'Dim locAsset1 As Asset = assetLib1.AppearanceAssets.Item("Clear")
'Dim oAppearance As Asset = locAsset1.CopyTo(oAsmDoc)
'oBoardOcc.Appearance = oAppearance


'If My.Computer.FileSystem.FileExists(strFilePath & "\" & "circuitboard" & ".ipt") Then
' Load occurrence from file (fast)
' oOccurrence = oAsmDoc.ComponentDefinition.Occurrences.Add(NewProjectCommand.GetWorkDirPath & "\" & "circuitboard" & ".ipt", _transformation)
'Else
' Load occurrence from document (fast)

'oBoardNode.Visible = False
' Set render style (transparent asset)



'Dim oCompDef As PartComponentDefinition = oPartDoc.ComponentDefinition
'Dim oBaseFeature As NonParametricBaseFeature = oCompDef.Features.NonParametricBaseFeatures.Add(oSurfaceBody)

'' Add a name (shown on the folderbrowser)
'oPartDoc.FullFileName = "circuit carrier"

'Dim oAsmDoc As AssemblyDocument = oAddIn.ActiveDocument
'oOccurrence = oAsmDoc.ComponentDefinition.Occurrences.AddByComponentDefinition(oCompDef, _transformation)

'' Save file
'oAddIn.SilentOperation = True
''oPartDoc.SaveAs(strFilePath & "\" & _id & ".ipt", False)
'oPartDoc.Close(True)
'oAddIn.SilentOperation = False
'' End If

'' Insert circuit board in the mid-browser
''_Server.Browser.InsertChildNode(oAsmDoc, oOccurrence, "Circuit Board")

'' Add a unique attribute for the board
'Dim oOccAttSets As AttributeSets = oOccurrence.AttributeSets
'Dim oOccAttribSet As AttributeSet = oOccAttSets.Add("circuitboard")
'Dim oOccAttrib As Attribute = oOccAttribSet.Add("board", ValueTypeEnum.kIntegerType, 1)

' Create line client graphics for the net
'*********************************************************************************************************************
'Public Sub CreateNetClientGraphics(ByRef interactionEvents As InteractionEvents)
'    For Each oNet As CircuitNet In oNetList
'        oNet.CreateClientGraphics(interactionEvents)
'    Next
'End Sub

' Initialize NetList
'*********************************************************************************************************************
'Public Sub CreateNetLines()
'    For Each oNet In oNetList
'        oNet.CreateNetLine()
'    Next
'End Sub

'Public Sub UpdateNetClientGraphics(partId As String)
'    For Each oNet As CircuitNet In oNetList
'        If oNet.Contains(partId) Then
'            oNet.UpdateClientGraphics()
'        End If
'    Next
'End Sub

'Public Sub CreateNetClientGraphics(partId As String, _
'                                   oInteractionEvents As InteractionEvents)

'    For Each oNet As CircuitNet In oNetList
'        If oNet.Contains(partId) Then
'            oNet.CreateClientGraphics(oInteractionEvents)
'        End If
'    Next
'End Sub

'Public Sub DeleteNetClientGraphics()
'    For Each oNet In oNetList
'        oNet.DeleteClientGraphics()
'    Next
'End Sub

'Public Sub UpdateNetLines(oPart As CircuitPart)
'    For Each oNet As CircuitNet In oNetList
'        If oNet.Contains(oPart.Id) Then
'            oNet.UpdateNetLine()
'        End If
'    Next
'End Sub

'Dim oInteractionGraphics As InteractionGraphics = oServer.CommandCollection.AddNetCommand.InteractionEvents.InteractionGraphics
'Dim oClientGraphics As ClientGraphics = oInteractionGraphics.PreviewClientGraphics

' Set transformation
'If oClientGraphics IsNot Nothing Then
'Dim oBoardNode As GraphicsNode = oClientGraphics.Item(1)