Option Explicit On

Imports System.Xml
Imports System.Windows.Forms
Imports Inventor

'############################################
' Read Keep Out Request
'############################################

Public Class ReadKeepOutsRequest
    Inherits ChangeRequest

    Private oAddIn As Inventor.Application
    Private oServer As MidAddInServer
    Private oReadKeepOutCmd As ReadKeepOutCommand

    ' Constructor
    '**********************************************************************************************************************
    Public Sub New(ByVal readkeepOutCmd As ReadKeepOutCommand, _
                   ByVal addIn As Inventor.Application, _
                   ByVal server As MidAddInServer)

        Me.oReadKeepOutCmd = readkeepOutCmd
        Me.oAddIn = addIn
        Me.oServer = server

    End Sub

    ' On Execute
    '**********************************************************************************************************************
    Public Overrides Sub OnExecute(ByVal document As Document, _
                                   ByVal context As NameValueMap, _
                                   ByRef succeeded As Boolean)

        ' Clear KeepOut list
        Dim oKeepOuts As KeepOuts = oServer.MidDataTypes.CircuitCarrier.KeepOuts
        oKeepOuts.Clear()

        ' Read keepOuts from file
        ReadKeepOuts(oKeepOuts, oReadKeepOutCmd.FilePath)

        'Update view
        oAddIn.ActiveView.Update()

    End Sub

    ' Read KeepOuts from XML
    '***************************************************************************************************************
    Public Sub ReadKeepOuts(ByVal keepOuts As KeepOuts, _
                            ByVal strFilePath As String)

        'Dim oKeepOuts As KeepOuts = oServer.MidDataTypes.CircuitCarrier.KeepOuts
        ' Clear the KeepOut List
        ' oKeepOuts.Free()
        'oKeepOutDlg.updateDlg()

        ' Create new xml reader
        ' Note: No error handling since strFilePath was validated before
        Dim oXmlReader As XmlTextReader = New XmlTextReader(strFilePath)

        ' Check if the selected file is a KeepOut file
        If Not oXmlReader.IsStartElement("KeepOuts") Then
            MessageBox.Show("The selected file does not contain KeepOuts", "MID Project", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Asterisk)
            oXmlReader.Close()
            oXmlReader = Nothing
            Exit Sub
        End If

        Dim _routingAllowed As Boolean
        Dim _faceId As String
        Dim _Id As String
        ' Start reading line by line
        Try
            Do While (oXmlReader.Read())

                Select Case oXmlReader.NodeType

                    Case XmlNodeType.Element
                        If oXmlReader.HasAttributes() Then

                            Select Case oXmlReader.Name

                                Case "FaceKeepOut"

                                    Debug.WriteLine(oXmlReader.Name)

                                    While oXmlReader.MoveToNextAttribute()

                                        If String.Equals(oXmlReader.Name, "routingAllowed") Then

                                            Debug.WriteLine(oXmlReader.Name & " = " & oXmlReader.Value)
                                            _routingAllowed = Convert.ToBoolean(oXmlReader.Value)
                                        End If

                                        If String.Equals(oXmlReader.Name, "faceID") Then
                                            Debug.WriteLine(oXmlReader.Name & " = " & oXmlReader.Value)
                                            _faceId = oXmlReader.Value
                                        End If

                                        If String.Equals(oXmlReader.Name, "Id") Then
                                            Debug.WriteLine(oXmlReader.Name & " = " & oXmlReader.Value)
                                            _Id = oXmlReader.Value

                                        End If
                                    End While

                                    ' Find face within circuit carrier model
                                    Dim oFace As FaceProxy = oServer.MidDataTypes.CircuitCarrier.GetFaceById(_Id)

                                    ' +++ evtl mit attributes

                                    If oFace IsNot Nothing Then

                                        keepOuts.Add(oFace, _routingAllowed, _faceId)
                                    Else
                                        MessageBox.Show("The KeepOuts do not belong to the circuit carrier", "MID Project", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
                                        'oKeepOuts.Clear()
                                        'oKeepOutDlg.updateDlg()
                                    End If

                            End Select

                        End If

                End Select
            Loop

        Catch ex As XmlException
            ' the xml file has an syntax error
            MessageBox.Show("There is an error in Line " & ex.LineNumber & ", " & ex.LinePosition & " of the Xml-file")
        Catch ex As Exception
            ' other exceptions, not classified
            MessageBox.Show("There was an error reading from xml file", "MID Project", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ' deallocate resources

            oXmlReader.Close()
            oXmlReader = Nothing
        End Try
    End Sub


End Class
