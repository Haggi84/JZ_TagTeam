'footprint 1.2
#If _useInventor Then
Option Explicit On
Imports Inventor
Imports System.Xml.XPath
Imports System.Xml


Module PBRep_XML_Reader
    'Dim path As String = "C:\Users\jozeitler\Desktop\TWA.xml"
    Function PBRepXML_Reader(ByVal path As String) As Boolean

        Dim i, j As Integer    ' Hilfsvariablen
        Dim tmp As String      ' Hilfsvariablen
        Dim xml As XmlDocument ' Unser Document Container

        xml = New XmlDocument

        ' Wir könnten jetzt eine xml Datei laden

        xml.Load(path)

        Dim xpath As String

        xpath = "//Faces"

        ' Dokumentgruppe,Dokument,Datei,Pfad
        Dim xmln As XmlNode ' Container für unseren aktiven Knoten
        Dim looplist, edgelist As XmlNodeList
        Dim attr As XmlAttributeCollection
        Dim att As XmlAttribute
        Dim EdgeName() As String
        Dim counter, size As Integer


        xmln = xml.SelectSingleNode(xpath)

        ' Für den Fall das wir mehrere Knoten haben auf die XPath zutrifft
        j = xml.SelectNodes(xpath).Count
        If j > 0 Then

            For i = 0 To j - 1 Step 1
                xmln = xml.SelectNodes(xpath).Item(i)
                For k = 0 To xmln.ChildNodes.Count - 1 Step 1
                    'loop ebene
                    looplist = xmln.ChildNodes(k).FirstChild.FirstChild.ChildNodes
                    For l = 0 To looplist.Count - 1 Step 1
                        'kanten ebene
                        edgelist = looplist(l).ChildNodes
                        counter = -1
                        size = edgelist.Count - 1
                        ReDim EdgeName(size)

                        For m = 0 To edgelist.Count - 1 Step 1
                            'attributebene
                            attr = edgelist(m).Attributes
                            att = attr(0)
                            counter = counter + 1
                            EdgeName(counter) = att.Value
                        Next
                        Call FindEdges(EdgeName, path)
                    Next
                Next
            Next
        Else
            tmp = "Kein Knoten vorhanden"
        End If

        Return True

    End Function
    Sub FindEdges(ByRef EdgeID() As String, ByVal path As String)

        Dim i, j As Integer    ' Hilfsvariablen

        'GeometrieArrays
        ' 1) Line
        Dim Line_StartXYZ(2), Line_EndXYZ(2) As Double
        ' 2) EllipticalArc
        Dim EllArc_SweepAngle, EllArc_CenterPoint(2), EllArc_MajorRadius, EllArc_MinorRadius, EllArc_MajorAxisVector(2), EllArc_StartPoint(2), EllArc_EndPoint(2), EllArc_PlaneNormal(2) As Double
        ' 3) Ellipse
        Dim Ell_MajorRadius, Ell_MinorRadius, Ell_CenterPoint(2), Ell_MajorAxisVector(2) As Double

        Dim tmp As String      ' Hilfsvariablen
        Dim xml As XmlDocument ' Unser Document Container



        ' Wir könnten jetzt eine xml Datei laden



        Dim findEdge, findGeometry As String
        Dim edgeAtt As XmlAttributeCollection
        Dim AttID As XmlAttribute
        Dim node As XmlNode
        Dim ID As String

        xml = New XmlDocument
        xml.Load(path)

        findEdge = "//Edge"
        findGeometry = "//EdgeGeometry"


        ' Für den Fall das wir mehrere Knoten haben auf die XPath zutrifft
        For count = 0 To EdgeID.Length - 1


            j = xml.SelectNodes(findEdge).Count

            'Wenn Knoten vorhanden
            If j > 0 Then

                'Zähle alle Knoten durch
                For i = 0 To j - 1 Step 1

                    node = xml.SelectNodes(findEdge).Item(i)
                    edgeAtt = node.Attributes
                    AttID = edgeAtt(0)
                    ID = AttID.Value

                    'Wenn eine übereinstimmung vorliegt zwischen  der durchgezählten ID und der Hinterlegten EdgeID
                    If EdgeID(count) = ID Then
                        'müssen die entsprechenden Geometrien ausgewählt werden:
                        Select Case node.FirstChild.NextSibling.FirstChild.Name
                            'Edge ist ein.....
                            Case "Line"
                                'Attribute aus XML auslesen für...
                                '1) startpunkt
                                For LineAtt = 0 To node.FirstChild.NextSibling.FirstChild.ChildNodes(0).Attributes.Count - 1
                                    Line_StartXYZ(LineAtt) = Val(node.FirstChild.NextSibling.FirstChild.ChildNodes(0).Attributes(LineAtt).Value)
                                    'ZYX in XYZ umwandeln
                                    Array.Reverse(Line_StartXYZ)
                                Next LineAtt
                                '2) endpunkt
                                For LineAtt = 0 To node.FirstChild.NextSibling.FirstChild.ChildNodes(1).Attributes.Count - 1
                                    Line_EndXYZ(LineAtt) = Val(node.FirstChild.NextSibling.FirstChild.ChildNodes(1).Attributes(LineAtt).Value)
                                    'ZYX in XYZ umwandeln
                                    Array.Reverse(Line_EndXYZ)
                                Next
                                'Übergabe der Punkte an Zeichnungsfunktion
                                DrawLine(Line_StartXYZ, Line_EndXYZ)


                            Case "EllipticalArc"

                                For EllArc_Att = 0 To node.FirstChild.NextSibling.FirstChild.Attributes.Count - 1
                                    ' Top Level Attribute
                                    ' Elliptischer Bogen: 2= Major Radius, 1 = Minor Radius, 0 = Öffnungswinkel
                                    Select Case EllArc_Att
                                        Case 2
                                            EllArc_MajorRadius = Val(node.FirstChild.NextSibling.FirstChild.Attributes(EllArc_Att).Value)
                                        Case 1
                                            EllArc_MinorRadius = Val(node.FirstChild.NextSibling.FirstChild.Attributes(EllArc_Att).Value)
                                        Case 0
                                            EllArc_SweepAngle = Val(node.FirstChild.NextSibling.FirstChild.Attributes(EllArc_Att).Value)
                                    End Select
                                Next

                                ' Low level Attribute
                                ' Elliptischer Bogen: 0=CenterPoint 1=Achsvektor 3=Startpunkt 4=Endpunkt
                                For EllArc_Nodes = 0 To node.FirstChild.NextSibling.FirstChild.ChildNodes.Count - 1
                                    For EllArc_Att = 0 To node.FirstChild.NextSibling.FirstChild.ChildNodes(EllArc_Nodes).Attributes.Count - 1
                                        Select Case EllArc_Nodes
                                            Case 0
                                                Dim s As String = node.FirstChild.NextSibling.FirstChild.ChildNodes(EllArc_Nodes).Attributes(EllArc_Att).Value
                                                EllArc_CenterPoint(EllArc_Att) = Val(node.FirstChild.NextSibling.FirstChild.ChildNodes(EllArc_Nodes).Attributes(EllArc_Att).Value)
                                                'ZYX in XYZ umordnen
                                                Array.Reverse(EllArc_CenterPoint)
                                            Case 1
                                                EllArc_MajorAxisVector(EllArc_Att) = Val(node.FirstChild.NextSibling.FirstChild.ChildNodes(EllArc_Nodes).Attributes(EllArc_Att).Value)
                                                'ZYX in XYZ umordnen
                                                Array.Reverse(EllArc_MajorAxisVector)
                                            Case 2
                                                EllArc_PlaneNormal(EllArc_Att) = Val(node.FirstChild.NextSibling.FirstChild.ChildNodes(EllArc_Nodes).Attributes(EllArc_Att).Value)
                                                'ZYX in XYZ umordnen
                                                Array.Reverse(EllArc_PlaneNormal)
                                            Case 3
                                                EllArc_StartPoint(EllArc_Att) = Val(node.FirstChild.NextSibling.FirstChild.ChildNodes(EllArc_Nodes).Attributes(EllArc_Att).Value)
                                                'ZYX in XYZ umordnen
                                                Array.Reverse(EllArc_StartPoint)
                                            Case 4
                                                EllArc_EndPoint(EllArc_Att) = Val(node.FirstChild.NextSibling.FirstChild.ChildNodes(EllArc_Nodes).Attributes(EllArc_Att).Value)
                                                'ZYX in XYZ umordnen
                                                Array.Reverse(EllArc_EndPoint)
                                        End Select
                                    Next
                                Next
                                Draw_EllipticalArc(EllArc_MajorRadius, EllArc_MinorRadius, EllArc_SweepAngle, EllArc_CenterPoint, EllArc_MajorAxisVector, EllArc_StartPoint, EllArc_EndPoint, EllArc_PlaneNormal)

                            Case "Ellipse"
                                For Ell_Att = 0 To node.FirstChild.NextSibling.FirstChild.Attributes.Count - 1
                                    'Top Level Attribute
                                    'Ellipse Bogen: Major Radius = 0 Minor Radius = 1
                                    Select Case Ell_Att
                                        Case 0
                                            Ell_MajorRadius = Val(node.FirstChild.NextSibling.FirstChild.Attributes(Ell_Att).Value)
                                        Case 1
                                            Ell_MinorRadius = Val(node.FirstChild.NextSibling.FirstChild.Attributes(Ell_Att).Value)
                                    End Select
                                Next
                                'Low Level Attribute
                                For Ell_Nodes = 0 To node.FirstChild.NextSibling.FirstChild.ChildNodes.Count - 1
                                    For Ell_Att = 0 To node.FirstChild.NextSibling.FirstChild.ChildNodes(Ell_Nodes).Attributes.Count - 1
                                        Select Case Ell_Nodes
                                            'Ellipse Bogen: CenterPoint = 0 Achsenvektor = 2
                                            Case 0
                                                Ell_CenterPoint(Ell_Att) = Val(node.FirstChild.NextSibling.FirstChild.ChildNodes(Ell_Nodes).Attributes(Ell_Att).Value)
                                                'ZYX in XYZ umwandeln
                                                Array.Reverse(Ell_CenterPoint)
                                            Case 1
                                            Case 2
                                                Ell_MajorAxisVector(Ell_Att) = Val(node.FirstChild.NextSibling.FirstChild.ChildNodes(Ell_Nodes).Attributes(Ell_Att).Value)
                                                'ZYX in XYZ umwandeln
                                                Array.Reverse(Ell_MajorAxisVector)
                                            Case 3
                                        End Select
                                    Next
                                Next
                                'Ellipse Zeichnen
                                Draw_Ellipse(Ell_MajorRadius, Ell_MinorRadius, Ell_CenterPoint, Ell_MajorAxisVector)
                        End Select



                    End If
                Next
            Else

                tmp = "Kein Knoten vorhanden"

            End If


        Next count



    End Sub
End Module

#End If

'    Sub XMLReader(ByVal XML_Filename As String, ByVal _invApp As Inventor.Application)

'        ' Wir benötigen einen XmlReader für das Auslesen der XML-Datei 
'        Dim XMLReader As Xml.XmlReader _
'          = New Xml.XmlTextReader(XML_Filename)

'        ' Es folgt das Auslesen der XML-Datei 
'        With XMLReader

'            Do While .Read ' Es sind noch Daten vorhanden 

'                ' Welche Art von Daten liegt an? 
'                Select Case .NodeType

'                    ' Ein Element 
'                    Case Xml.XmlNodeType.Element

'                        'If .Name = "Edge" Then MsgBox(.Value)

'                        '            '  Case "Edge id"

'                        If .Name = "Line" Then
'                            Get_Line(XMLReader, _invApp)

'                        End If


'                        'If .Name =  "EllipticalArc" 



'                End Select


'            Loop  ' Weiter nach Daten schauen 

'            .Close()  ' XMLTextReader schließen 

'        End With

'    End Sub



'    Private Sub Get_Line(ByVal XMLReader As Xml.XmlReader, ByVal _invApp As Inventor.Application)

'        Dim temp As Double
'        Dim count As Integer
'        Dim StartPoint(3), EndPoint(3) As Double
'        Dim PointStart As Point2d
'        Dim PointEnd As Point2d

'        count = 1

'        Dim XMLReaderTemp As Xml.XmlReader
'        XMLReaderTemp = XMLReader


'        Try
'            With XMLReaderTemp

'                Console.WriteLine("Es folgt ein Element vom Typ " & .Name)

'                Do While XMLReaderTemp.Read

'                    Select Case .Name
'                        Case "StartPoint"
'                            If .AttributeCount > 0 Then
'                                ' Es sind noch weitere Attribute vorhanden 
'                                While .MoveToNextAttribute ' nächstes 

'                                    temp = Val(.Value)
'                                    StartPoint(count) = temp
'                                    count = count + 1

'                                End While

'                            End If

'                            count = Nothing
'                            count = 1

'                        Case "EndPoint"

'                            If .AttributeCount > 0 Then
'                                ' Es sind noch weitere Attribute vorhanden 
'                                While .MoveToNextAttribute ' nächstes 
'                                    temp = Val(.Value)
'                                    EndPoint(count) = temp
'                                    count = count + 1

'                                End While

'                                count = Nothing

'                            End If
'                    End Select


'                    'PointStart.X = StartPoint(1)
'                    'PointStart.Y = StartPoint(2)
'                    'PointStart.Z = StartPoint(3)

'                    'PointEnd.X = EndPoint(0)
'                    'PointEnd.Y = EndPoint(1)
'                    'PointEnd.Z = EndPoint(2)



'                    'DrawLine(PointStart, PointEnd)


'                Loop
'                DrawLine(StartPoint, EndPoint, _invApp)
'                StartPoint = Nothing
'                EndPoint = Nothing

'            End With
'        Catch ex As Exception
'            MsgBox(ex)
'        End Try
'    End Sub

'    Private Sub DrawLine(ByRef StartPoint() As Double, ByRef EndPoint() As Double, ByVal _invApp As Application)

'        Dim oPartCompDef As PartComponentDefinition
'        Dim oSketch As Sketch

'        oPartCompDef = _invApp.ActiveDocument.ComponentDefinition
'        oSketch = oPartCompDef.Sketches.Add(oPartCompDef.WorkPlanes.Item(3))

'        ' Linie erstellen
'        With _invApp.TransientGeometry
'            Call oSketch.SketchLines.AddByTwoPoints(.CreatePoint2d(StartPoint(1), StartPoint(2)), .CreatePoint2d(EndPoint(1), EndPoint(2)))

'        End With
'    End Sub

'End Module
