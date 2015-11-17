Option Explicit On


Imports Inventor
Imports System.Xml
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports System.Collections


'##############################################
' PART CLASS
'##############################################

Public Class CircuitPart
    Inherits Component






    Private strPath As String

    Private OccHeight = 0.04

    Private _Transformation As Matrix
    Private _width As Double
    Private _length As Double
    Private _id As String
    Private _x As Double
    Private _y As Double
    Private _z As Double
    Private _dev As String
    Private _value As String

    Private oAddIn As Inventor.Application
    Private oServer As MidAddInServer

    Private oPins As New List(Of CircuitPin)
    Private oBoard As CircuitBoard
    Private oOccurrence As ComponentOccurrence

    ' Private oPositionVector As Vector

    Private strPartFolderPath As String = "F:\Users\Paul\Bachelor's Thesis\CAD\Inventor Addin\MIDProjectVB3 - Copy 12.6\MIDProjectVB3\Models"

    ' Constructor
    Public Sub New(ByVal oAddIn As Inventor.Application, _
                   ByVal oServer As MidAddInServer, _
                   ByVal oBoard As CircuitBoard)

        MyBase.New()

        Me.oServer = oServer
        Me.oAddIn = oAddIn
        Me.oBoard = oBoard

        oPins = New List(Of CircuitPin)
        oOccurrence = Nothing

    End Sub

    ' Initialize part (--> add occurrence)
    Public Sub Initialze()

        '###check for valid x,y,z values here, as well as fro alid width and length
        ' Get current assembly document
        Dim oAsmDoc = oAddIn.ActiveDocument
        Dim oTG As TransientGeometry = oAddIn.TransientGeometry

        ' Calculate relative position
        _Transformation = oTG.CreateMatrix()
        ' Get the position of the board
        Dim oRelVector As Vector

        oRelVector = oTG.CreateVector(oBoard.Transformation.Cell(1, 4), oBoard.Transformation.Cell(2, 4), oBoard.Transformation.Cell(3, 4))
        ' Substract half of the width and length of the board
        oRelVector.AddVector(oTG.CreateVector(-oBoard.Length / 2, -oBoard.Width / 2, 0))

        oRelVector.AddVector(oTG.CreateVector(Length / 2, Width / 2, 0))
        ' add the relative Position, change y and z value here!
        oRelVector.AddVector(oTG.CreateVector(X, Y, Z))
        ' translate the matrix by oPositionVector
        _Transformation.SetTranslation(oRelVector)

        strPartFolderPath = strPartFolderPath & "\" & _dev & ".ipt"

        'If System.IO.File.Exists(strPartFolderPath) Then

        '    oOccurrence = oAsmDoc.ComponentDefinition.Occurrences.Add(strPartFolderPath, _Transformation)

        'Else

        ' Create box
        Dim minPoint(2) As Double
        Dim maxPoint(2) As Double
        minPoint(0) = -_width / 2 : minPoint(1) = -_length / 2 : minPoint(2) = 0
        maxPoint(0) = _width / 2 : maxPoint(1) = _length / 2 : maxPoint(2) = 0.04

        Dim oBox As Box = oAddIn.TransientGeometry.CreateBox()
        oBox.PutBoxData(minPoint, maxPoint)

        ' Create surface body and feature
        ' Add new part document
        Dim oPartDoc As PartDocument = oAddIn.Documents.Add(DocumentTypeEnum.kPartDocumentObject, , False)
        Dim oBody As SurfaceBody = oAddIn.TransientBRep.CreateSolidBlock(oBox)
        Dim oCompDef As PartComponentDefinition = oPartDoc.ComponentDefinition
        Dim oBaseFeature As NonParametricBaseFeature = oCompDef.Features.NonParametricBaseFeatures.Add(oBody)


        ' Add a name (shown on the folderbrowser)
        oPartDoc.FullFileName = _dev

        ' Load occurrence from part document
        oOccurrence = oAsmDoc.ComponentDefinition.Occurrences.AddByComponentDefinition(oCompDef, _Transformation)
        oAddIn.SilentOperation = True
        oPartDoc.Close(True)
        oAddIn.SilentOperation = False

        ' End If


        ' Insert circuit part in the mid-browser
        oServer.Browser.InsertChildNode(oAsmDoc, oOccurrence, "Circuit Parts")



        Dim oOccAttribSets As AttributeSets = oOccurrence.AttributeSets
        Dim oOccAttribSet As AttributeSet = oOccAttribSets.Add("part")
        Dim oOccAttrib As Attribute = oOccAttribSet.Add("part", ValueTypeEnum.kIntegerType, 2)
        'MsgBox(oOccAttribSet.NameIsUsed("part"))

        ' Initialize Pins
        For Each oPin As CircuitPin In oPins
            oPin.Initialize()
        Next

    End Sub

    ' Update pin positions relative to part
    '****************************************************************************************************************
    Public Sub UpdatePins()
        For Each oPin As CircuitPin In oPins
            oPin.Update()
        Next
    End Sub

    ' Add pins to the pin list
    '****************************************************************************************************************
    Public Sub AddPin(oPin As CircuitPin)
        oPins.Add(oPin)
    End Sub

    ' Find pin by Id
    '****************************************************************************************************************
    Public Function FindPinById(ByVal _pinId As String) As CircuitPin
        For Each oPin In Pins
            If oPin.Id.Equals(_pinId) Then
                Return oPin
            End If
        Next
        Return Nothing
    End Function


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

    Public Property Transformation() As Matrix
        Get
            Return oOccurrence.Transformation
        End Get
        Set(value As Matrix)
            oOccurrence.Transformation = value
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

    Public Property Length As Double
        Get
            Return _length
        End Get
        Set(value As Double)
            Me._length = value
        End Set
    End Property

    Public Property Width As Double
        Get
            Return _width
        End Get
        Set(value As Double)
            Me._width = value
        End Set
    End Property

    


    Public ReadOnly Property Pins As List(Of CircuitPin)
        Get
            Return oPins
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



End Class