Imports System.Collections.Generic

'##############################################
' CONNECTION CLASS
'##############################################

Public Class CircuitNet

    Private _id As String
    Private oContacts As New List(Of NetContact)
    Private oParent As CircuitBoard

    Private oAddIn As Inventor.Application
    Private oServer As MidAddInServer

    Private oOccurrence As ComponentOccurrence

    Private lineCoordinates() As Double
    Private oPreviewLineNode As GraphicsNode
    Private oPreviewLineCoordSet As GraphicsCoordinateSet
    Private oPreviewLineSet As LineStripGraphics
    Private oPreviewGraphics As ClientGraphics
    Private oNetLineCoordSet As GraphicsCoordinateSet

    ' Constructor
    '****************************************************************************************************************
    Public Sub New(addIn As Inventor.Application, _
                   server As MidAddInServer, _
                   board As CircuitBoard, _
                   id As String)

        Me.oAddIn = addIn
        Me.oParent = board
        Me.oServer = server

        _id = id

    End Sub

    '####################################################################
    ' PREVIEW GRAPHICS
    '####################################################################

    ' Delete the client graphics
    '****************************************************************************************************************
    Public Sub DeletePreviewGraphics()
        If oPreviewLineNode IsNot Nothing Then
            oPreviewLineNode.Delete()
            oPreviewLineNode = Nothing
        End If
    End Sub

    ' Update the client graphics
    '****************************************************************************************************************
    Public Sub UpdatePreviewGraphics()

        ' Update the line coordinate sets
        For i As Integer = 0 To oContacts.Count() - 1
            lineCoordinates(i * 3 + 0) = oContacts.Item(i).Transformation.Cell(1, 4)
            lineCoordinates(i * 3 + 1) = oContacts.Item(i).Transformation.Cell(2, 4)
            lineCoordinates(i * 3 + 2) = oContacts.Item(i).Transformation.Cell(3, 4)
        Next
        oPreviewLineCoordSet.PutCoordinates(lineCoordinates)

        ' Set
        oPreviewLineSet.CoordinateSet = oPreviewLineCoordSet
        'oAddIn.ActiveView.Update()

    End Sub

    ' Initialize the Netlist
    '****************************************************************************************************************
    Public Sub CreatePreviewGraphics(oInteractionEvents As InteractionEvents)

        Dim oAsmDoc As Document = oAddIn.ActiveDocument()

        If oContacts.Count >= 2 Then

            ' Get interaction graphics object
            Dim oInteractionGraphics As InteractionGraphics = oInteractionEvents.InteractionGraphics
            Dim oPreviewGraphics As ClientGraphics = oInteractionGraphics.PreviewClientGraphics
            Dim oPreviewDataSets As GraphicsDataSets = oInteractionGraphics.GraphicsDataSets

            ' Add a new line node
            oPreviewLineNode = oPreviewGraphics.AddNode(4)

            ' Create a new coordiante set and add the pin positions
            oPreviewLineCoordSet = oPreviewDataSets.CreateCoordinateSet(1)
            ReDim lineCoordinates(oContacts.Count() * 3)
            'Debug.WriteLine("Netid = " & Me.Id)
            For i As Integer = 0 To oContacts.Count() - 1
                ' Debug.WriteLine("Contact 1: Pinid = " & oContacts(i).PinId & ", Partid = " & oContacts(i).PartId)
                lineCoordinates(i * 3 + 0) = oContacts.Item(i).Transformation.Cell(1, 4)
                lineCoordinates(i * 3 + 1) = oContacts.Item(i).Transformation.Cell(2, 4)
                lineCoordinates(i * 3 + 2) = oContacts.Item(i).Transformation.Cell(3, 4)
            Next
            'Debug.WriteLine("________________________________________________________________________________________")
            oPreviewLineCoordSet.PutCoordinates(lineCoordinates)

            ' Use line strip graphics
            oPreviewLineSet = oPreviewLineNode.AddLineStripGraphics
            oPreviewLineSet.CoordinateSet = oPreviewLineCoordSet

            ' Add color to the line set (orange)
            Dim oColorSet As GraphicsColorSet = oPreviewDataSets.CreateColorSet(1)
            oColorSet.Add(1, 255, 161, 0)
            oPreviewLineSet.ColorSet = oColorSet

            oAddIn.ActiveView.Update()

        Else
            System.Windows.Forms.MessageBox.Show("Error: Cannot create connection, too less connections", "MID Project", Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Asterisk)
        End If

    End Sub

    '####################################################################
    ' VISUAL REPRESENTATION
    '####################################################################

    ' Recalculate the Inventor representation
    '****************************************************************************************************************
    Public Sub RecalculateIRep()
        oOccurrence.Delete()
        oOccurrence = Nothing
        CreateOccurrence()

    End Sub

    ' Create the Inventor representation
    '****************************************************************************************************************
    Public Sub CreateIRep()
        CreateOccurrence()
    End Sub

    ' Create the sweep feature and the occurrence
    '****************************************************************************************************************
    Private Sub CreateOccurrence()

        If oContacts.Count > 0 Then

            Dim oPartDoc As PartDocument = oAddIn.Documents.Add(DocumentTypeEnum.kPartDocumentObject, , False)
            Dim oPartCompDef As PartComponentDefinition = oPartDoc.ComponentDefinition

            Dim oTG As TransientGeometry = oAddIn.TransientGeometry

            For i As Integer = 0 To oContacts.Count - 2

                Dim oWorkPlanes(1) As WorkPoint

                ' Create a new 3D Sketch.
                Dim oSketch3D As Sketch3D = oPartDoc.ComponentDefinition.Sketches3D.Add

                ' Add workpoints for a sketch line
                oWorkPlanes(0) = oPartCompDef.WorkPoints.AddFixed(oTG.CreatePoint(oContacts.Item(i).Transformation.Cell(1, 4), oContacts.Item(i).Transformation.Cell(2, 4), oContacts.Item(i).Transformation.Cell(3, 4)))
                oWorkPlanes(1) = oPartCompDef.WorkPoints.AddFixed(oTG.CreatePoint(oContacts.Item(i + 1).Transformation.Cell(1, 4), oContacts.Item(i + 1).Transformation.Cell(2, 4), oContacts.Item(i + 1).Transformation.Cell(3, 4)))

                ' Create a new sketch line
                Dim oLine As SketchLine3D = oSketch3D.SketchLines3D.AddByTwoPoints(oWorkPlanes(0), oWorkPlanes(1), True, 1)

                Dim oWorkPlane As WorkPlane = oPartCompDef.WorkPlanes.AddByNormalToCurve(oLine, oLine.EndSketchPoint)

                ' Create a sketch containing a circle (for sweep profile).
                Dim oSketch As PlanarSketch = oPartCompDef.Sketches.Add(oWorkPlane)
                Dim oModelPoint As Point = oLine.EndSketchPoint.Geometry

                Dim oRectangleLines As SketchEntitiesEnumerator = oSketch.SketchLines.AddAsTwoPointRectangle(oTG.CreatePoint2d(-0.0002, -0.01), oTG.CreatePoint2d(0.002, 0.01))

                'Dim oSketchPoint1 As SketchPoint = oSketch.SketchPoints.Add(oSketch.ModelToSketchSpace(


                'oSketch.SketchCircles.AddByCenterRadius(oSketch.ModelToSketchSpace(oModelPoint), 0.01)

                ' Create a profile.
                Dim oProfile As Profile = oSketch.Profiles.AddForSolid
                ' Create a path
                Dim oPath As Path = oPartCompDef.Features.CreatePath(oLine)
                ' Create the sweep feature.
                Dim oSweep As SweepFeature = oPartCompDef.Features.SweepFeatures.AddUsingPath(oProfile, oPath, PartFeatureOperationEnum.kJoinOperation)

                oWorkPlane.Visible = False

            Next

            ' Create occurrence
            Dim oMatrix As Matrix = oTG.CreateMatrix()
            oOccurrence = oAddIn.ActiveDocument.ComponentDefinition.Occurrences.AddByComponentDefinition(oPartCompDef, oMatrix)

            Dim oBlueStyle As RenderStyle = oAddIn.ActiveDocument.RenderStyles.Item("Cyan")
            oOccurrence.SetRenderStyle(StyleSourceTypeEnum.kOverrideRenderStyle, oBlueStyle)
            ' Close part document
            oPartDoc.Close(True)

        End If
    End Sub

    '####################################################################
    ' OTHER
    '####################################################################

    ' Add a contact to the contact list of the connection
    '****************************************************************************************************************
    Public Sub AddContact(oContact As NetContact)
        oContacts.Add(oContact)
    End Sub

    ' Contains (check for contact containg the given part)
    '****************************************************************************************************************
    Public Function Contains(part As CircuitPart) As Boolean
        For Each oContact As NetContact In oContacts
            If part Is oContact.Part Then
                Return True
            End If
        Next
        Return False
    End Function

    'Properties
    '****************************************************************************************************************
    Public Property Id As String
        Get
            Return _id
        End Get
        Set(value As String)
            _id = value
        End Set
    End Property

End Class



'For i As Integer = 0 To oContacts.Count() - 1
'    lineCoordinates(i * 3 + 0) = oContacts.Item(i).Transformation.Cell(1, 4)
'    lineCoordinates(i * 3 + 1) = oContacts.Item(i).Transformation.Cell(2, 4)
'    lineCoordinates(i * 3 + 2) = oContacts.Item(i).Transformation.Cell(3, 4)
'Next
'oNetLineCoordSet.PutCoordinates(lineCoordinates)
'oNetLineSet.CoordinateSet = oNetLineCoordSet


'Dim partDoc As PartDocument = oAddIn.Documents.Add(DocumentTypeEnum.kPartDocumentObject, , False)
'Dim partDef As PartComponentDefinition = partDoc.ComponentDefinition

'Dim oTBRep As TransientBRep = oAddIn.TransientBRep
'Dim oTG As TransientGeometry = oAddIn.TransientGeometry

'For i As Integer = 0 To oContacts.Count() - 2
'    'Dim oPoint1 As Point = oTG.CreatePoint(o, 0, 0)
'    'Dim oPoint2 As Point = oTG.CreatePoint(4, 0, 0)
'    Dim partDoc As PartDocument = oAddIn.Documents.Add(DocumentTypeEnum.kPartDocumentObject, , False)
'    Dim partDef As PartComponentDefinition = partDoc.ComponentDefinition
'    Dim oBody As SurfaceBody = oTBRep.CreateSolidCylinderCone(oContacts.Item(i).GetPoint(), oContacts.Item(i + 1).GetPoint(), 0.01, 0.01, 0.01)
'    Dim baseBody As NonParametricBaseFeature = partDef.Features.NonParametricBaseFeatures.Add(oBody)
'    Dim oMatrix As Matrix = oTG.CreateMatrix()
'    Dim oCompOcc1 As ComponentOccurrence = oAsmDoc.ComponentDefinition.Occurrences.AddByComponentDefinition(partDef, oMatrix)
'    partDoc.Close(True)

'Next

'Dim oAsmDoc As AssemblyDocument = oAddIn.ActiveDocument

' Graphics data set



' Prepare data set
'Dim oAsmDoc As Document = oAddIn.ActiveDocument
'Dim oPreviewDataSets As GraphicsDataSets = oAsmDoc.GraphicsDataSetsCollection.Add(_id)
'oNetLineCoordSet = oPreviewDataSets.CreateCoordinateSet(1)
'ReDim lineCoordinates(oContacts.Count() * 3)
'For i As Integer = 0 To oContacts.Count() - 1

'    lineCoordinates(i * 3 + 0) = oContacts.Item(i).Transformation.Cell(1, 4)
'    lineCoordinates(i * 3 + 1) = oContacts.Item(i).Transformation.Cell(2, 4)
'    lineCoordinates(i * 3 + 2) = oContacts.Item(i).Transformation.Cell(3, 4)
'Next
'oNetLineCoordSet.PutCoordinates(lineCoordinates)

'' Create graphics set
'Dim oPreviewGraphics As ClientGraphics = oCompDef.ClientGraphicsCollection.Add(_id)
'oNetLineNode = oPreviewGraphics.AddNode(1)
'oNetLineSet = oNetLineNode.AddLineStripGraphics
'oNetLineSet.CoordinateSet = oNetLineCoordSet

'' Add color to the line set (FAPS color scheme)
'Dim oColorSet As GraphicsColorSet = oPreviewDataSets.CreateColorSet(1)
'oColorSet.Add(1, 221, 221, 221)
'oNetLineSet.ColorSet = oColorSet
'oAddIn.ActiveView.Update()

'Dim oModelVector As Vector = oTG.CreateVector(oModelPoint.X, oModelPoint.Y, oModelPoint.Z)

'Dim oPointVector1 As Vector = oTG.CreateVector(uTangents(0) * 0.01, uTangents(1) * 0.01, uTangents(2) * 0.01)

'oPointVector1.AddVector(oModelVector)