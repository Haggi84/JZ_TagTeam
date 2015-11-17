'footprint 1.2
' **********
' * KeyMan *
' **********

' Key-Managing

' Hilfsmittel für die Arbeit mit Referenz-Keys von BRep-Objekten in Autodesk Inventor
' ===================================================================================

Option Explicit


Imports Inventor

Module KeyMan

    ' -------------------------------------------------------------------------------------------------

    ' ANMERKUNGEN:

    ' A)Die Ref-Keys der BRep-Objekten können (unter Zuhilfenahme eines sogenannten Key-Contextes)
    ' benutzt werden, um die BRep-Objekte im Objektmodell eines Dokumentes wieder zu finden.
    ' Dabei wird der Key-Context in einer Long-Größe "keyContextNumber" gemerkt.

    ' B) Dieses Auffinden der BRep-Objekte funktioniert mit folgenden Einschränkungen:
    ' 1) Es ist nicht möglich, sich den Key-Context auf andere Weise zu merken,
    '    etwa als Feld oder String-Code eines Feldes.
    ' 2) Die Objekt-Keys gelten genau so lange, wie das Dokument (Projekt) geöffnet ist.
    ' Diese Einschränkungen stehen im Widerspruch zu
    ' Brian Ekins: Odds and Ends of the Autodesk Inventor API, Autodesk University, 2012, Code-CP3547

    ' C) Da das erneute Auffinden von BRep-Objekten im Objekt-Modell für die Übertragung der BRep
    '    in das Produkt-Modell von "MID-Layout" (XML) noch nicht erforderlich ist, wird diese
    '    Funktionalität in der aktuellen Version noch nicht ausgeführt. Die entsprechenden Funktionen
    '    wurden in den Modul der Programmbeispiele verschoben.

    ' D) In Inventor sind die Ref-Keys Byte-Felder. Sie können durch handlichere Strings codiert werden.
    '    Dazu wird in der Inventor-API im sogenannten Ref-Key-Manager eine Methode "KeyToString"
    '    angeboten - und für die Decodierung die Methode "StringToKey".


    ' -------------------------------------------------------------------------
    '                           Public Globale Variable
    ' -------------------------------------------------------------------------

    Public keyContextNumber As Long                 ' Key-Kontext

    ' -------------------------------------------------------------------------
    '                         Globale Variable des Moduls
    ' -------------------------------------------------------------------------

    Private refKeyManager As ReferenceKeyManager    ' Referenz-Key-Manager

    ' Public Funktionen und Sub-Programme

    Public Sub initKeyManaging()
        Dim doc As document
        doc = ThisApplication.ActiveDocument
        refKeyManager = doc.ReferenceKeyManager
        keyContextNumber = refKeyManager.CreateKeyContext
        Dim keyContextArray() As Byte = New Byte() {}
        refKeyManager.SaveContextToArray(keyContextNumber, keyContextArray)
        initVertexMap()
        initEdgeMap()
        initFaceMap()
    End Sub

    ' KeyString eines Objektes (Vertex, Edge oder Face) aus dem Objektmodell von Inventor bestimmen

    ' Object ====> KeyString

    Public Function vertexToKeyString(ByRef v As Vertex) As String
        Dim keyArray() As Byte = New Byte() {}
        v.GetReferenceKey(keyArray, keyContextNumber)
        Return refKeyManager.KeyToString(keyArray)
    End Function

    Public Function edgeToKeyString(ByRef e As Edge) As String
        Dim keyArray() As Byte = New Byte() {}
        e.GetReferenceKey(keyArray, keyContextNumber)
        Return refKeyManager.KeyToString(keyArray)
    End Function

    Public Function faceToKeyString(ByRef f As face) As String
        Dim keyArray() As Byte = New Byte() {}
        f.GetReferenceKey(keyArray, keyContextNumber)
        Return refKeyManager.KeyToString(keyArray)
    End Function

    ' Füllen der drei Maps für Knoten, Kanten und Facetten

    Public Sub fillMaps(ByRef Vertices As Vertices, ByRef Edges As Edges, ByRef Faces As Faces)

        Dim i As Integer
        Dim keyString As String
        Dim vertex As Inventor.Vertex
        Dim edge As Edge
        Dim face As Face
        Dim n As Integer = Vertices.Count

        For i = 1 To n
            vertex = Vertices(i)
            keyString = vertexToKeyString(vertex)
            appendVertex(keyString, i)
        Next i

        n = Edges.Count
        For i = 1 To n
            edge = Edges(i)
            keyString = edgeToKeyString(edge)
            appendEdge(keyString, i)
        Next i

        n = Faces.Count
        For i = 1 To n
            face = Faces(i)
            keyString = faceToKeyString(face)
            appendFace(keyString, i)
        Next i

        'Ax.switchToHourGlassCursor       'Nicht löschen!
    End Sub

End Module

