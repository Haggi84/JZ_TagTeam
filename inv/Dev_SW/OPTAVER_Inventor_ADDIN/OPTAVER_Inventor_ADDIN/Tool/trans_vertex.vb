'footprint 1.2
' ***************
' * TransVertex *
' ***************

' Übertragung der Knoten
' ======================

Option Explicit

Imports Inventor

Module TransVertex

    Public Sub translateVertices(ByRef Vertices As Vertices)
        openTag(Vertices_)
        Dim n, i As Integer
        n = Vertices.Count
        Statistics.numVertices = n
        For i = 1 To n
            TranslateVertex(Vertices, i)
        Next i
        closeTag(Vertices_)
        'Ax.switchToHourGlassCursor       'Nicht löschen!
    End Sub

    Private Sub TranslateVertex(ByRef vList As Vertices, ByVal i As Integer)

        ' Initialisierungen
        ' =================
        Dim v As Vertex
        v = vList(i)

        ' Start-Tag
        ' =========
        Dim idVertex, keyString, attributes As String
        idVertex = "V" & i
        If (printKeys) Then
            keyString = vertexToKeyString(v)
            attributes = mkAttrib(id_, idVertex) & mkAttrib(extRef_, keyString)
        Else
            attributes = mkAttrib(id_, idVertex)
        End If

        openTag(Vertex_, attributes)

        ' Location-Tag
        ' ============
        Dim location(2) As Double
        v.GetPoint(location)
        convPoint3d(location)
        simpleTag(Location_, Ax.point3dToS(location))

        ' End-Tag
        ' =======
        closeTag(Vertex_)

    End Sub

End Module


