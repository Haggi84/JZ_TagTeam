'footprint 1.2
#If _useInventor = True Then
Imports System.Xml.XPath
Imports System.Xml

Module Routing_Result_XML_Reader3D


    Function Routing_Result_XML_Parser3D(ByRef path As String) As Boolean
        Dim i As Integer ' Hilfsvariablen
        Dim xml As XmlDocument ' Unser Document Container


        'GeometrieArrays
        ' 1) Line
        Dim Line_StartXYZ(2), Line_EndXYZ(2) As Double
        ' 2) Elliptical Arc
        Dim EllArc_SweepAngle, EllArc_CenterPoint(2), EllArc_MajorRadius, EllArc_MinorRadius, EllArc_MajorAxisVector(2), EllArc_StartPoint(2), EllArc_EndPoint(2), EllArc_PlaneNormal(2) As Double


        xml = New XmlDocument

        ' Wir könnten jetzt eine xml Datei laden

        xml.Load(path)

        Dim xpath As String

        xpath = "//Line"

        ' Dokumentgruppe,Dokument,Datei,Pfad
        Dim xmln, xmlntemp As XmlNode ' Container für unseren aktiven Knoten
        Dim xmlnColl As XmlNodeList
        Dim attr_Start, attr_End As XmlAttributeCollection
        Dim att As XmlAttribute

        xmln = xml.SelectSingleNode(xpath)

        i = xml.SelectNodes(xpath).Count

        If i > 0 Then

            For j = 0 To i - 1 Step 1

                xmln = xml.SelectNodes(xpath).Item(j)
                xmlntemp = xmln.FirstChild
                attr_Start = xmlntemp.Attributes()

                Line_StartXYZ(0) = Val(attr_Start(0).Value)
                Line_StartXYZ(1) = Val(attr_Start(1).Value)
                Line_StartXYZ(2) = Val(attr_Start(2).Value)

                xmlntemp = xmlntemp.NextSibling
                attr_End = xmlntemp.Attributes()

                Line_EndXYZ(0) = Val(attr_End(0).Value)
                Line_EndXYZ(1) = Val(attr_End(1).Value)
                Line_EndXYZ(2) = Val(attr_End(2).Value)


                DrawLine3D(Line_StartXYZ, Line_EndXYZ)

            Next


        End If

        xpath = "//EllipticalArc"


        xmln = xml.SelectSingleNode(xpath)

        i = xml.SelectNodes(xpath).Count



        If i > 0 Then

            For j = 0 To i - 1 Step 1

                xmln = xml.SelectNodes(xpath).Item(j)
                EllArc_MajorRadius = Val(xmln.Attributes.Item(0).Value)
                EllArc_MinorRadius = Val(xmln.Attributes.Item(1).Value)
                EllArc_SweepAngle = Val(xmln.Attributes.Item(2).Value)


                xmlnColl = xmln.ChildNodes


                For k = 0 To 2 Step 1

                    EllArc_CenterPoint(k) = Val(xmlnColl.Item(0).Attributes.Item(k).Value)
                    EllArc_MajorAxisVector(k) = Val(xmlnColl.Item(1).Attributes.Item(k).Value)
                    EllArc_PlaneNormal(k) = Val(xmlnColl.Item(2).Attributes.Item(k).Value)
                    EllArc_StartPoint(k) = Val(xmlnColl.Item(3).Attributes.Item(k).Value)
                    EllArc_EndPoint(k) = Val(xmlnColl.Item(4).Attributes.Item(k).Value)

                Next k

                DrawEllipticalArc3D(EllArc_MajorRadius, EllArc_MinorRadius, EllArc_SweepAngle, EllArc_CenterPoint, _
                        EllArc_MajorAxisVector, EllArc_StartPoint, EllArc_EndPoint, EllArc_PlaneNormal)

            Next


        End If



        Return True

    End Function
End Module
#End If

