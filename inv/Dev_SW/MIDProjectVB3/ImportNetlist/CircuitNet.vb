Imports System.Collections.Generic

'##############################################
' CONNECTION CLASS
'##############################################

Public Class CircuitNet

    Private oContacts As New List(Of CircuitContact)

    Private _id As String
    Private oAddIn As Inventor.Application


    Private oPointcoords() As Double

    Private oCompDef As ComponentDefinition
    Private oDataSets As GraphicsDataSets
    Private oCoordSet As GraphicsCoordinateSet

    Private oClientGraphics As ClientGraphics
    Private oLineNode As GraphicsNode
    Private oLineSet As LineGraphics

    Dim oAsmDoc As AssemblyDocument
    Private oBoard As CircuitBoard

    Public Sub New(oAddIn As Inventor.Application, _
                   oBoard As CircuitBoard)

        Me.oAddIn = oAddIn
        Me.oBoard = oBoard

    End Sub

    ' Initialize Netlist
    '****************************************************************************************************************
    Public Sub Initialize()

        oAsmDoc = oAddIn.ActiveDocument()
        'For i As Integer = 0 To oContacts.Count() - 1
        '    oContacts.Item(i).EvalPointCoord()
        'Next

        If oContacts.Count >= 2 Then


            oCompDef = oAddIn.ActiveDocument.ComponentDefinition
            oDataSets = oAsmDoc.GraphicsDataSetsCollection.Add(Id)

            oCoordSet = oDataSets.CreateCoordinateSet(1)

            ReDim oPointcoords(oContacts.Count() * 3)

            For i As Integer = 0 To oContacts.Count() - 1
                oPointcoords(i * 3 + 0) = oContacts.Item(i).Position.X
                oPointcoords(i * 3 + 1) = oContacts.Item(i).Position.Y
                oPointcoords(i * 3 + 2) = oContacts.Item(i).Position.Z
            Next

            oCoordSet.PutCoordinates(oPointcoords)

            ' Graphics set
            oClientGraphics = oCompDef.ClientGraphicsCollection.Add(Id)
            oLineNode = oClientGraphics.AddNode(1)
            oLineSet = oLineNode.AddLineGraphics
            oLineSet.CoordinateSet = oCoordSet
            'make the lines visible to the mouse
            oLineNode.Selectable = True


            Dim oColorSet As GraphicsColorSet
            oColorSet = oDataSets.CreateColorSet(1)

            oColorSet.Add(1, 255, 216, 0)

            ' Update the view to see the resulting spiral.
            oAddIn.ActiveView.Update()

            oLineSet.ColorSet = oColorSet

            'oCoordSet.Delete()

        Else
            MsgBox("Error: Cannot create connection -" & vbNewLine & "too less connections")
        End If

    End Sub

    ' Add a contact to the contact list of the connection
    Public Sub AddContact(oContact As CircuitContact)
        oContacts.Add(oContact)
    End Sub


    Public Sub Update()

        'oCoordSet.Delete()
        'oCoordSet = oDataSets.CreateCoordinateSet(1)


        'For i As Integer = 0 To oContacts.Count() - 1
        '    oContacts.Item(i).EvalPointCoord()
        'Next

        'If oContacts.Count >= 2 Then
        '    ReDim oPointcoords(oContacts.Count() * 3)
        '    For i As Integer = 0 To oContacts.Count() - 1
        '        oPointcoords(i * 3 + 0) = oContacts.Item(i).Point.X
        '        oPointcoords(i * 3 + 1) = oContacts.Item(i).Point.Y
        '        oPointcoords(i * 3 + 2) = oContacts.Item(i).Point.Z
        '    Next

        '    ' If oCoordSet IsNot Nothing Then
        '    ' oDataSets = oAsmDoc.GraphicsDataSetsCollection.Add(id)
        '    'oCoordSet = oDataSets.Item(1)
        '    oCoordSet.PutCoordinates(oPointcoords)
        '    ' End If


        '    oLineSet.CoordinateSet = oCoordSet

        '    oAddIn.ActiveView().Update()
        'End If

    End Sub

    'Properties

    Public Property Id As String
        Get
            Return _id
        End Get
        Set(value As String)
            _id = value
        End Set
    End Property







    ' Get the contact list reference
    Public Function GetContacts() As List(Of CircuitContact)
        Return oContacts
    End Function

End Class




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