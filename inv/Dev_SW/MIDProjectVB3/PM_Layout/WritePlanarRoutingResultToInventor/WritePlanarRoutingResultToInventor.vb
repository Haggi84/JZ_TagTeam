'footprint 1.2
#If _useInventor Then

Option Explicit On
Imports System.IO
Imports System.Windows.Forms
Imports PM_Layout.Files
Imports System.Text
Imports PM_Layout.Planar_Routing_Result_XML_Reader

Module WritePlanarRoutingResultToInventor

    Function write_planar_routing_result_to_inventor() As Boolean

        ' Planares RoutingErgebnis zur Übergabe in temporären File schreiben

        Dim nameOfTempFile As String = brepPath0 & "\PlanarRoutingResult_CAD.xml"
        outFile = New StreamWriter(nameOfTempFile, False, Encoding.ASCII)
        outFile.Close()

        Dim rc As Integer = writePlanarRoutingResultToXML(nameOfTempFile)
        If rc = 0 Then
            If showPlanarRoutingResultInInventor(nameOfTempFile) = True Then
                Return True
            Else : Return False

            End If

        Else
            Return False
        End If

        'Dim succ As Boolean = showPlanarRoutingResultInInventor(nameOfTempFile)

        'If Not (succ) Then
        '    Return False
        'End If

    End Function

    Private Function showPlanarRoutingResultInInventor(ByRef nameOfTempFile As String) As Boolean
        Dim succ As Boolean


        initialize_environment()

        succ = Planar_Routing_Result_XML_Parser(nameOfTempFile)

        If Not (succ) Then
            Return False
        End If

        Return True
    End Function

End Module

#End If
