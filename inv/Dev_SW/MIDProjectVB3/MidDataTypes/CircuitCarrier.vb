Option Explicit On

'###########################################
' Circuit Carrier Class
'###########################################

Public Class CircuitCarrier

    Private oOccurrence As ComponentOccurrence
    Private oCarrierNode As BrowserNode

    Private oAddIn As Inventor.Application
    Private oServer As MidAddInServer

    Private oKeepOuts As KeepOuts

    Private oKeepOutsNode As BrowserNode
    Private oOccurrenceNode As BrowserNode

    ' Constructor
    '**********************************************************************************************************
    Public Sub New(oAddIn As Inventor.Application, _
                   oServer As MidAddInServer)
        MyBase.New()

        Me.oAddIn = oAddIn
        Me.oServer = oServer

    End Sub

    ' Add Occurrence and Nodes
    '**********************************************************************************************************
    Public Function CreateIRep(oOccurrence As ComponentOccurrence) As Boolean

        ' Set reference to Circuitcarrier occurrence
        Me.oOccurrence = oOccurrence

        'Check if the occurrence consists of just one surfacebody
        If oOccurrence.SurfaceBodies.Count > 1 Or oOccurrence.SurfaceBodies.Count = 0 Then
            Return False
        End If

        ' Add new browser nodes
        oOccurrenceNode = oServer.Browser.CreateNode(, oServer.Commands.CircuitCarrierNode, oOccurrence)
        oKeepOutsNode = oServer.Browser.CreateNode("KeepOuts", oServer.Commands.CircuitCarrierNode)

        ' Add new KeepOuts
        oKeepOuts = New KeepOuts(Me, oAddIn, oServer)

        ' Save data to occurrence
        If Not oOccurrence.AttributeSets.NameIsUsed("CircuitCarrier") Then
            Dim oAttribSets As AttributeSets = oOccurrence.AttributeSets
            'Dim oAttribSet As AttributeSet = 
            oAttribSets.Add("CircuitCarrier")
            oAttribSets.Add("KeepOuts")
        End If

    End Function

    ' PROPERTIES
    '**********************************************************************************************************

    Public Property KeepOuts As KeepOuts
        Get
            Return oKeepOuts
        End Get
        Set(value As KeepOuts)
            oKeepOuts = value
        End Set
    End Property

    ' Return Keep Outs Node
    Public ReadOnly Property KeepOutsNode As BrowserNode
        Get
            Return oKeepOutsNode
        End Get
    End Property

    ' Return occurrence
    Public ReadOnly Property Occurrence As ComponentOccurrence
        Get
            Return oOccurrence
        End Get
    End Property

    ' Is circuit carrier
    '**********************************************************************************************************
    Public Function IsCircuitCarrier(ByVal oOcc As ComponentOccurrence) As Boolean
        For Each face As Face In oOcc.SurfaceBodies.Item(1).Faces
            If face.InternalName.Equals(oOcc.SurfaceBodies.Item(1).Faces.Item(1).InternalName) Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Function GetFaceById(ByVal faceId As String) As Face

        For Each face As Face In oOccurrence.SurfaceBodies.Item(1).Faces
            If face.InternalName.Equals(faceId) Then
                Return face
            End If
        Next

        Return Nothing
    End Function

    Public Function IsCircuitCarrier(ByVal oFace As Face) As Boolean

        For Each face As Face In oOccurrence.SurfaceBodies.Item(1).Faces
            If face.InternalName.Equals(oFace.InternalName) Then
                Return True
            End If
        Next

        Return False

    End Function



End Class
