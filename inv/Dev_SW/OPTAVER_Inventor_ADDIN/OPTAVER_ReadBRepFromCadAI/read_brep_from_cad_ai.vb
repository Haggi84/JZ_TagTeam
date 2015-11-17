'footprint 1.2
' *********************
' * readBrepFromCadAI *
' *********************

' Übertragung der BRep eines Schaltungsträgers aus dem Objektmodell
' von Autodesk Inventor in das Produkt-Modell von MID-Layout (XML)
' =================================================================

Option Explicit
Imports System.IO
Imports System.Text

'#Const _useInventor = True
#If _useInventor Then

Imports Inventor

Module ReadBrepFromCadAI

    ' -------------------------------------------------------------------------
    '                         Public Function "readBrepFromCadAI"
    ' -------------------------------------------------------------------------

    Public Function readBrepFromCadAI() As Boolean
        Dim suc As Boolean

        suc = Files.fileDialogsForReadBrepFromCadAI()
        If Not (suc) Then
            Return False
        End If

        'Ax.switchToHourGlassCursor

        ' Initialisierungen
        suc = GlobVar.initialization()
        If Not (suc) Then
            Return False
        End If


        ' Listen der Knoten, Kanten, und Facetten bereitstellen
        Dim vList As Vertices = Nothing
        Dim eList As Edges = Nothing
        Dim fList As Faces = Nothing

        suc = detObjectLists(vList, eList, fList)
        If Not (suc) Then
            Return False
        End If

        ' Die Maps für Knoten, Kanten, und Facetten füllen
        KeyMan.fillMaps(vList, eList, fList)

        ' BRep-XML-Output-Datei öffnen und beginnen
        outFile = New StreamWriter(GlobVar.full_FileName_BRep, False, Encoding.ASCII)
        initBRepXmlFile()

        ' Liste der Knoten übertragen
        translateVertices(vList)

        ' Liste der Kanten übertragen
        translateEdges(eList)

        ' Liste der Faces übertragen
        translateFaces(fList)

        ' Statistik
        writeStatistics()

        ' BRep-XML-Output-Datei abschließen
        finishBRepXmlFile()

        'Ax.switchToNormalCursor

        Return True
    End Function

    ' -------------------------------------------------------------------------
    '                Private Prozeduren und Funktionen "ReadBRep"
    ' -------------------------------------------------------------------------


    Private Function detObjectLists(ByRef vList As Vertices, ByRef eList As Edges, ByRef fList As Faces) As Boolean

        ' Vom Dokument zur (einzigen) Shell des (einzigen) Bodys durchhangeln
        Dim doc As Document
        doc = (GlobVar_Inventor.thisApplication).ActiveDocument
        Dim compDef As PartComponentDefinition
        compDef = doc.ComponentDefinition
        Dim bodies As SurfaceBodies
        bodies = compDef.SurfaceBodies
        Dim n As Integer
        n = bodies.Count
        If n = 0 Then
            MidMsgBoxProblem("In BRep no body")
            Return False
        End If
        If n > 1 Then
            MidMsgBoxProblem("In BRep more then one body")
            Return False
        End If
        Dim body As SurfaceBody
        body = bodies(1)
        Ax_Inventor.analyseRangeBox(body)
        Dim shells As FaceShells
        shells = body.FaceShells
        n = shells.Count
        If n = 0 Then
            MidMsgBoxProblem("In the body of BRep no shell")
            Return False
        End If
        If n > 1 Then
            MidMsgBoxProblem("In the body of BRep  more then one shell")
            Return False
        End If
        Dim shell As FaceShell
        shell = shells(1)

        ' Objekt-Listen
        vList = body.Vertices
        eList = shell.Edges
        fList = shell.Faces

        'Ax.switchToHourGlassCursor       'Nicht löschen!
        Return True
    End Function

    Private Sub initBRepXmlFile()


        ' Head schreiben
        outFile.WriteLine("<?xml version=""1.0"" encoding=""UTF-8""?>")

        ' Start-Tag der BRep rausschreiben
        Dim keyContextString As String
        If (printKeys) Then
            keyContextString = mkAttrib(keyContext_, keyContextNumber) & vbCrLf
        Else
            keyContextString = ""
        End If

        Dim attributes As String
        attributes = mkAttrib(denotion_, GlobVar.name_of_BRep) & mkAttrib(cadSystem_, cadSystem) & vbCrLf & _
                     mkAttrib(unitOfLength_, unitOfLength) & mkAttrib(precisionOfLength_, precisionOfLength) & _
                     mkAttrib(unitOfAngles_, unitOfAngles) & mkAttrib(precisionOfAngles_, precisionOfAngles) & vbCrLf & _
                     keyContextString & _
                     mkAttrib(xsi_, xsi_val_) & vbCrLf & _
                     mkAttrib(schemaLocation_, "file:///" & fullSchemeName)

        openTag(BRep_, attributes)

        'Ax.switchToHourGlassCursor       'Nicht löschen!

    End Sub

    Private Sub finishBRepXmlFile()
        closeTag(BRep_)
        outFile.Close()
        outFile = Nothing
    End Sub

End Module

#End If
