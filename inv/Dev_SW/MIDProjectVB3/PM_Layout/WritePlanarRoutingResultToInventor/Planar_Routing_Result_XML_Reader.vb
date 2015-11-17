'footprint 1.2
Option Explicit On

#If _useInventor Then
Imports Inventor

Imports System.Xml.XPath
Imports System.Xml

Module Planar_Routing_Result_XML_Reader

    Function Planar_Routing_Result_XML_Parser(ByRef path As String) As Boolean
        Dim i As Integer ' Hilfsvariablen
        Dim xml As XmlDocument ' Unser Document Container


        'GeometrieArrays
        ' 1) Line
        Dim Line_StartXYZ(2), Line_EndXYZ(2) As Double

        xml = New XmlDocument

        ' Wir könnten jetzt eine xml Datei laden

        xml.Load(path)

        Dim xpath As String

        xpath = "//Line"

        ' Dokumentgruppe,Dokument,Datei,Pfad
        Dim xmln, xmlntemp As XmlNode ' Container für unseren aktiven Knoten
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


                DrawLine(Line_StartXYZ, Line_EndXYZ)

            Next


        End If

        Return True

    End Function
End Module

#End If
