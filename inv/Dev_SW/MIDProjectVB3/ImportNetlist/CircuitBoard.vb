Imports Inventor
Imports System.Xml
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports System.Collections

'##############################################
' CIRCUIT BOARD CLASS
'##############################################

Public Class CircuitBoard

    'Implements Inventor.App

    Private strFilePath As String

    Private _AddIn As Inventor.Application
    Private _width As Double
    Private _length As Double
    Private _id As String
    Private offset As Double = 4
    Private _transformation As Matrix
    Private oOccurrence As ComponentOccurrence

    Private _Server As MidAddInServer

    Private oParts As List(Of CircuitPart)
    Private oNets As New List(Of CircuitNet)

    Private _denotion As String
    Private _unitOfLength As String

    Private _strFilePath As String



    ' Constructor
    '*********************************************************************************************************************
    'Public Sub New(ByVal oAddIn As Inventor.Application, _
    '               ByVal oServer As MidAddInServer, _
    '               ByVal strFilePath As String)

    '    Me._AddIn = oAddIn
    '    Me._Server = oServer

    '    'Me._strFilePath = oServer.CommandCollection.WorkDirectory & "\Components"
    '    MsgBox(_strFilePath)
    '    Me._strFilePath = strFilePath
    '    Me._id = "cirucitboard" '###change later
    '    ' Parse xml from file

    '    If ParseXml() Then
    '        Init()
    '    End If
    '    ' Create new representation




    'End Sub


    Public Sub New(ByVal oAddIn As Inventor.Application, _
                   ByVal oServer As MidAddInServer)

        MyBase.New()

        Me._AddIn = oAddIn
        Me._Server = oServer

        oParts = New List(Of CircuitPart)

    End Sub

    ' Initialze: Create visible representation and save part
    '*********************************************************************************************************************
    Public Sub Initialize()

        ' Position Matrix
        _transformation = _AddIn.TransientGeometry.CreateMatrix()

        ' Create Box
        Dim minPoint(2) As Double
        Dim maxPoint(2) As Double
        minPoint(0) = -Length / 2 : minPoint(1) = -Width / 2 : minPoint(2) = 0
        maxPoint(0) = Length / 2 : maxPoint(1) = Width / 2 : maxPoint(2) = 0.01

        Dim oBox As Box = _AddIn.TransientGeometry.CreateBox()
        oBox.PutBoxData(minPoint, maxPoint)

        ' Create surface body and feature
        ' Open new part document to create a visible representation
        Dim oPartDoc As PartDocument = _AddIn.Documents.Add(DocumentTypeEnum.kPartDocumentObject, , False)
        Dim oBody As SurfaceBody = _AddIn.TransientBRep.CreateSolidBlock(oBox)
        Dim oCompDef As PartComponentDefinition = oPartDoc.ComponentDefinition
        Dim oBaseFeature As NonParametricBaseFeature = oCompDef.Features.NonParametricBaseFeatures.Add(oBody)

        ' Add a name (shown on the folderbrowser)
        oPartDoc.FullFileName = "circuit carrier"

        Dim oAsmDoc As AssemblyDocument = _AddIn.ActiveDocument
        oOccurrence = oAsmDoc.ComponentDefinition.Occurrences.AddByComponentDefinition(oCompDef, _transformation)

        ' Save file
        _AddIn.SilentOperation = True
        'oPartDoc.SaveAs(strFilePath & "\" & _id & ".ipt", False)
        oPartDoc.Close(True)
        _AddIn.SilentOperation = False
        ' End If

        ' Insert circuit board in the mid-browser
        _Server.Browser.InsertChildNode(oAsmDoc, oOccurrence, "Circuit Board")

        ' Add a unique attribute for the board
        Dim oOccAttSets As AttributeSets = oOccurrence.AttributeSets
        Dim oOccAttribSet As AttributeSet = oOccAttSets.Add("circuitboard")
        Dim oOccAttrib As Attribute = oOccAttribSet.Add("board", ValueTypeEnum.kIntegerType, 1)

    End Sub

    ' Initialize parts
    '*********************************************************************************************************************
    Public Sub InitializeParts()
        For Each oPart In oParts
            oPart.Initialze()
        Next
    End Sub

    ' Initialize nets
    '*********************************************************************************************************************
    Public Sub InitializeNets()
        For Each oNet In oNets
            oNet.Initialize()
        Next
    End Sub

    ' Add part to part list
    '*********************************************************************************************************************
    Public Sub AddPart(oPart As CircuitPart)
        oParts.Add(oPart)
    End Sub

    ' Add Net to Net list
    '*********************************************************************************************************************
    Public Sub AddNet(oNet As CircuitNet)
        oNets.Add(oNet)
    End Sub

    ' Find part within partlist with id
    '*********************************************************************************************************************
    Public Function FindPartById(ByVal _partId As String) As CircuitPart
        For Each _part In Parts
            If _part.Id.Equals(_partId) Then
                Return _part
            End If
        Next
        Return Nothing
    End Function

    ' Find parent of occurrence
    '*********************************************************************************************************************
    Public Function Parent(ByRef oOccurrence As ComponentOccurrence) As CircuitPart

        For Each oPart As CircuitPart In oParts
            If oPart.Occurrence Is oOccurrence Then
                Return oPart
            End If
        Next
        Return Nothing
    End Function

    ' Properties
    '*********************************************************************************************************************
    ' width Property (read only)

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
            Return Me.Occurrence.Transformation
        End Get
        Set(value As Matrix)
            Me.Occurrence.Transformation = value
        End Set
    End Property

    ' Return occurrence
    Public ReadOnly Property Occurrence() As ComponentOccurrence
        Get
            Return oOccurrence
        End Get
    End Property

    ' Return the partlist of the board
    Public ReadOnly Property Parts As List(Of CircuitPart)
        Get
            Return oParts
        End Get
    End Property

    Public ReadOnly Property Nets As List(Of CircuitNet)
        Get
            Return oNets
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