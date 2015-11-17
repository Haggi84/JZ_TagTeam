Option Explicit On

'##############################################
' PIN CLASS
'##############################################

Public Class KeepOut

    Private _routingAllowed As Boolean
    Private _faceId As String
    Private _id As String

    Private oAddIn As Inventor.Application
    Private oServer As MidAddInServer

    Private oFace As Inventor.FaceProxy
    Private oKeepOutNode As BrowserNode
    Private oParent As CircuitCarrier
    Private oAttribSet As AttributeSet

    ' Constructor
    '************************************************************************************************************************
    Public Sub New(ByVal parent As CircuitCarrier, _
                   ByVal addIn As Inventor.Application, _
                   ByVal server As MidAddInServer, _
                   ByRef face As Inventor.FaceProxy, _
                   Optional routingAllowed As Boolean = True, _
                   Optional faceId As String = Nothing)

        ' Set KeepOut data
        Me.oParent = parent
        Me.oAddIn = addIn
        Me.oServer = server
        Me.oFace = face
        Me._id = oFace.InternalName ' distinct GUID of the face 
        Me._routingAllowed = routingAllowed

        ' MsgBox(oFace.AttributeSets.Item("MIDFace").Name) ' consecutive faceId created during export

        If _faceId IsNot Nothing Then
            Me._faceId = faceId
        Else
            Dim oFaces As Faces
            detFaceList(oFace.Parent.Parent, oFaces)
            For i As Integer = 1 To oFaces.Count
                If oFace Is oFaces(i) Then
                    _faceId = "F" & i
                End If
            Next
        End If

    End Sub

    ' Add attributes
    '************************************************************************************************************************
    Public Sub AddAttributes()

        'If Not oFace.Parent.Parent.AttributeSets.NameIsUsed("KeepOuts") Then
        ' Dim oAttribSet As AttributeSet = oFace.Parent.Parent.AttributeSets.Item("KeepOuts")
        Dim oAttribMgr As AttributeManager = oAddIn.ActiveDocument.AttributeManager
        If oAttribMgr.FindAttributes("KeepOuts", Me._faceId, Me._id).Count = 0 Then
            Dim oAttribSet As AttributeSet = oFace.Parent.Parent.AttributeSets("KeepOuts")
            oAttribSet.Add(Me._faceId, ValueTypeEnum.kStringType, Me._id)
        End If
        'oAttribSet.Add("routingAllowed", ValueTypeEnum.kBooleanType, Me._routingAllowed)

    End Sub

    ' Delete attributes
    '************************************************************************************************************************
    Public Sub DeleteAttributes()
        Dim oAttrib As Inventor.Attribute = oFace.Parent.Parent.AttributeSets.Item("KeepOuts").Item(Me._faceId)
        oAttrib.Delete()
        oAttrib = Nothing
        '
    End Sub

    ' Add browser node
    '************************************************************************************************************************
    Public Sub AddBrowserNode()
        oKeepOutNode = oServer.Browser.CreateNode("FaceId: " & Me.FaceId, oServer.MidDataTypes.CircuitCarrier.KeepOutsNode)
    End Sub

    ' Delete browser node
    '************************************************************************************************************************
    Public Sub DeleteBrowserNode()
        oServer.Browser.DeleteNode(oKeepOutNode)
    End Sub

    ' Properties
    '************************************************************************************************************************
    Public Property KeepOutNode As BrowserNode
        Get
            Return oKeepOutNode
        End Get
        Set(value As BrowserNode)
            oKeepOutNode = value
        End Set
    End Property

    Public Property RoutingAllowed() As Boolean
        Get
            Return _routingAllowed
        End Get
        Set(value As Boolean)
            _routingAllowed = value
        End Set
    End Property

    Public Property FaceId() As String
        Get
            Return _faceId
        End Get
        Set(value As String)
            _faceId = value
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

    Public Property Face() As Inventor.FaceProxy
        Get
            Return oFace
        End Get
        Set(value As Inventor.FaceProxy)
            oFace = value
        End Set
    End Property

    ' Properties
    '************************************************************************************************************************
    Private Sub detFaceList(ByVal occurrence As ComponentOccurrence, _
                            ByRef faces As Faces)

        Dim oBodies As SurfaceBodies = occurrence.SurfaceBodies

        Dim oBody As SurfaceBody = oBodies.Item(1)
        Dim oShells As FaceShells = oBody.FaceShells

        Dim oShell As FaceShell = oShells.Item(1)

        faces = oShell.Faces

    End Sub

End Class
