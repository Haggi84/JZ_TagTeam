'footprint 1.2
#If _useInventor Then


Imports System.IO
Imports System.Windows.Forms
Imports PM_Layout.Files
Imports System.Text
Module Write3DRoutingResultToInventor
    Function write_3D_routing_result_to_inventor() As Boolean

        ' 3D RoutingErgebnis zur Übergabe in temporären File schreiben

        Dim nameOfTempFile As String = brepPath0 & "\3DRoutingResult_CAD.xml"
        'outFile = New StreamWriter(nameOfTempFile, False, Encoding.ASCII)
        'outFile.Close()

        'Dim rc As Integer = write3DRoutingResultToXML(nameOfTempFile)
        Dim rc As Integer = 0
        If rc = 0 Then
            If show3DRoutingResultInInventor(nameOfTempFile) = True Then
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

    Private Function show3DRoutingResultInInventor(ByRef nameOfTempFile As String) As Boolean
        Dim succ As Boolean


        initialize_environment3D()

        succ = Routing_Result_XML_Parser3D(nameOfTempFile)

        If Not (succ) Then
            Return False
        End If

        Return True
    End Function
End Module
#End If
