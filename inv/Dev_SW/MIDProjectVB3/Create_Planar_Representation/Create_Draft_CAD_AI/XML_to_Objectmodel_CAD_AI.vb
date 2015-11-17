Module XML_to_Objectmodel_CAD_AI


    ' Wir benötigen einen XmlReader für das Auslesen der XML-Datei 
    Dim XMLReader As Xml.XmlReader _
      = New Xml.XmlTextReader("C:\Users\jozeitler\Desktop\Test_Min_XML.xml")

    Sub XMLReader_main(ByVal _invApp As Inventor.Application, ByVal oPartCompDef As Inventor.PartComponentDefinition, ByVal oSketch As Inventor.Sketch)

        Dim nodename As String
        Dim boolLine = False


        ' Es folgt das Auslesen der XML-Datei 
        With XMLReader

            Do While .Read ' Es sind noch Daten vorhanden 

                ' Welche Art von Daten liegt an? 
                Select Case .NodeType

                    ' Ein Element 
                    Case Xml.XmlNodeType.Element

                        Console.WriteLine("Es folgt ein Element vom Typ " & .Name)

                        Select Case .Name
                            Case "Line"
                                Call caseLine(_invApp, oPartCompDef, oSketch)
                            Case "Circle"
                                ' Call caseCircle()
                            Case "CircularArc"
                                'Call caseCircularArc()
                            Case "Ellipse"
                                ' Call caseEllipse()
                            Case "EllipticalArc"
                                ' Call caseEllipticalArc()
                            Case "BSplineCurve"
                                'Call caseLine BSplineCurve()
                        End Select

                        ' Ein Text 
                    Case Xml.XmlNodeType.Text
                        Console.WriteLine("Es folgt ein Text: " & .Value)

                        ' Ein Kommentar 
                    Case Xml.XmlNodeType.Comment
                        Console.WriteLine("Es folgt ein Kommentar: " & .Value)

                End Select

            Loop  ' Weiter nach Daten schauen 

            .Close()  ' XMLTextReader schließen 

        End With


    End Sub

    Private Sub caseLine(ByVal _invApp As Inventor.Application, ByVal oPartCompDef As Inventor.PartComponentDefinition, ByVal oSketch As Inventor.Sketch)
        Dim count As Integer
        Dim Startpoint(2) As Double
        Dim Endpoint(2) As Double

        With XMLReader
            Dim fname As String
            fname = XMLReader.Name

            Do While .Read

                If .Name = "StartPoint" Then


                    ' Alle Attribute (Name-Wert-Paare) abarbeiten 
                    If .AttributeCount > 0 Then
                        ' Es sind noch weitere Attribute vorhanden 
                        count = 0
                        While .MoveToNextAttribute ' nächstes 

                            Console.WriteLine("Feldname: " & .Name & _
                            " -> " & _
                            "Feldwert: " & .Value)

                            Startpoint(count) = .Value / 10
                            count = count + 1
                            'CheckType(nodename, fieldname, fieldvalue)
                        End While

                    End If
                End If

                If .Name = "EndPoint" Then
                    ' Alle Attribute (Name-Wert-Paare) abarbeiten 
                    If .AttributeCount > 0 Then
                        ' Es sind noch weitere Attribute vorhanden 
                        count = 0
                        While .MoveToNextAttribute ' nächstes 

                            Console.WriteLine("Feldname: " & .Name & _
                            " -> " & _
                            "Feldwert: " & .Value)

                            Endpoint(count) = .Value / 10
                            count = count + 1
                            'CheckType(nodename, fieldname, fieldvalue)
                        End While

                    End If
                End If
            Loop

        End With

        Call create_line(_invApp, oSketch, oPartCompDef, Startpoint, Endpoint)
    End Sub


End Module

