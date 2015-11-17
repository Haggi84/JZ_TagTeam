Public Class KeepOut

    Private _routingAllowed As Boolean
    Private _faceId As String
    Private _id As String ' Inventor Id
    Private _face As Inventor.FaceProxy
    Private oAddIn As Inventor.Application


    Public Sub New(ByVal oAddIn As Inventor.Application, _
                   _face As Inventor.Face,
                   Optional _routingAllowed As Boolean = True, _
                   Optional _faceId As String = Nothing)

        Me.oAddIn = oAddIn

        Me.Face = _face
        Me._id = _face.InternalName
        Me._routingAllowed = _routingAllowed
        If _faceId IsNot Nothing Then
            Me._faceId = _faceId
        Else
            Dim oFaces As Faces
            detObjectLists(, , oFaces)
            For i As Integer = 1 To oFaces.Count
                If _face Is oFaces(i) Then
                    _faceId = "F" & i
                End If
            Next
        End If
       

    End Sub

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
            Return _face
        End Get
        Set(value As Inventor.FaceProxy)
            _face = value
        End Set
    End Property



    Private Function detObjectLists(Optional ByRef vl As Vertices = Nothing, _
                                    Optional ByRef el As Edges = Nothing, _
                                    Optional ByRef fl As Faces = Nothing) As Boolean

        Dim oDocument As Document = oAddIn.ActiveDocument
        Dim oCompDef As AssemblyComponentDefinition = oDocument.ComponentDefinition

        Dim oCompOccs As ComponentOccurrences = oCompDef.Occurrences
        If oCompOccs.Count() = 0 Then
            'Msg.problem("ReadBRep", "No body")
            detObjectLists = False
            Exit Function
        End If
        If oCompOccs.Count() > 1 Then
            'Msg.problem("ReadBRep", "More then one body")
            detObjectLists = False
            Exit Function
        End If
        'Ax.analyseRangeBox(body)

        Dim oCompOcc As ComponentOccurrence = oCompDef.Occurrences.Item(1)
        Dim oBodies As SurfaceBodies = oCompOcc.SurfaceBodies

        If oBodies.Count() = 0 Then
            detObjectLists = False
            Exit Function
        End If
        If oBodies.Count() > 1 Then
            detObjectLists = False
            Exit Function
        End If

        Dim oBody As SurfaceBody = oBodies.Item(1)
        Dim oShells As FaceShells = oBody.FaceShells

        'n = oShells.Count()
        If oShells.Count() = 0 Then
            'Msg.problem("ReadBRep", "No shell")
            detObjectLists = False
            Exit Function
        End If
        If oShells.Count() > 1 Then
            'Msg.problem("ReadBRep", "More then one shell")
            detObjectLists = False
            Exit Function
        End If
        Dim oShell As FaceShell = oShells.Item(1)

        ' Objekt-Listen
        vl = oBody.Vertices
        el = oShell.Edges
        fl = oShell.Faces

        detObjectLists = True

    End Function

End Class
