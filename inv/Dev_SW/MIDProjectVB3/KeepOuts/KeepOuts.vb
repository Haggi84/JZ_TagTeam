Imports Inventor
Imports System
Imports System.Xml
Imports System.Collections.Generic
Imports System.Windows.Forms


'#############################################
' KeepOuts Class
'############################################

Public Class KeepOuts
    Inherits System.Collections.Generic.List(Of KeepOut)


    'Private _KeepOuts As List(Of KeepOut)
    Private oAddIn As Inventor.Application
    Private oStyle As RenderStyle
    Private oAsmDoc As AssemblyDocument
    Private oXmlReader As XmlTextReader

    Private oServer As MidAddInServer

    ' Constructor
    '***************************************************************************************************************
    Public Sub New(oAddIn As Inventor.Application, oServer As MidAddInServer)

        MyBase.New()
        Me.oAddIn = oAddIn
        Me.oServer = oServer
    End Sub

    ' Add KeepOuts to the KeepOuts list
    '***************************************************************************************************************
    Public Shadows Sub Add(ByRef oFace As FaceProxy, _
                           Optional _routingAllowed As Boolean = True, _
                           Optional _faceId As String = Nothing)

        If Me.IdExists(oFace.InternalName) = False Then
            ' Create new keep out
            Dim oKeepOut As KeepOut = New KeepOut(oAddIn, oFace, _routingAllowed, _faceId)
            MyBase.Add(oKeepOut)

            ' Change Color
            Try
                oAsmDoc = oAddIn.ActiveDocument()
                oStyle = oAsmDoc.RenderStyles.Item("Smooth - Red")
                oKeepOut.Face.NativeObject.SetRenderStyle(StyleSourceTypeEnum.kOverrideRenderStyle, oStyle)
                oAddIn.ActiveView.Update()
            Catch ex As Exception
                MessageBox.Show("The asset could not be found", _
                                "MID Project", _
                                MessageBoxButtons.OK, _
                                MessageBoxIcon.Error)
            End Try

            ' Add to browser
            oServer.Browser.InsertChildNode(oAsmDoc, Nothing, "Keep-Outs", oKeepOut.Id)
        End If
    End Sub

    ' Remove: Search KeepOut belonging to the face, change color and finally remove it
    '***************************************************************************************************************
    Public Shadows Sub Remove(ByRef oFace As FaceProxy)

        If Me.IdExists(oFace.InternalName) Then
            Dim oKeepOut As KeepOut
            oAsmDoc = oAddIn.ActiveDocument()
            For Each oKeepOut In Me
                If oKeepOut.Id.Equals(oFace.InternalName) Then

                    ' Attach default color
                    oStyle = oKeepOut.Face.Parent.Parent.GetRenderStyle(StyleSourceTypeEnum.kOverrideRenderStyle)
                    oKeepOut.Face.NativeObject.SetRenderStyle(StyleSourceTypeEnum.kOverrideRenderStyle, oStyle)
                    oAddIn.ActiveView.Update()

                    ' Remove from browser
                    oServer.Browser.RemoveNode(oKeepOut.Id)

                    Exit For
                End If
            Next
            MyBase.Remove(oKeepOut)
        End If

    End Sub

    ' Clear: Change color before deleting items
    '***************************************************************************************************************
    Public Shadows Sub Clear()
        oAsmDoc = oAddIn.ActiveDocument()
        For Each oKeepOut As KeepOut In Me

            ' Attach default color
            oStyle = oKeepOut.Face.Parent.Parent.GetRenderStyle(StyleSourceTypeEnum.kOverrideRenderStyle)
            oKeepOut.Face.NativeObject.SetRenderStyle(StyleSourceTypeEnum.kOverrideRenderStyle, oStyle)
            oAddIn.ActiveView.Update()

            ' Remove from browser
            oServer.Browser.RemoveNode(oKeepOut.Id)

        Next
        MyBase.Clear()
    End Sub

    ' IdExists
    '***************************************************************************************************************
    Public Function IdExists(_id As String) As Boolean
        For Each oKeepOut As KeepOut In Me
            If oKeepOut.Id.Equals(_id) Then
                Return True
            End If
        Next
        Return False
    End Function

    ' Write keepOuts to Xml
    '***************************************************************************************************************
    Public Sub WriteXml()

        If Me.Count <= 0 Then
            Exit Sub
        End If

        ' Export only when checkbox is set 
        'If oKeepOutDlg.writeXmlCheck.CheckState() = False Then
        '    Exit Sub
        'End If

        Try

            Dim sFilePath As String = NewProjectCommand.GetWorkDirPath()
            sFilePath = sFilePath & "\KeepOuts.xml"

            ' Prepare KeepOutWriter for Garbage collector
            Using KeepOutWriter As XmlTextWriter = New XmlTextWriter(sFilePath, Nothing)

                ' Header
                '************************************************************************************
                KeepOutWriter.Formatting = Formatting.Indented

                KeepOutWriter.WriteStartDocument()

                KeepOutWriter.WriteStartElement("KeepOuts")

                KeepOutWriter.WriteAttributeString("project", oAsmDoc.DisplayName)
                KeepOutWriter.WriteAttributeString("denotation", Me.Item(1).Face.Parent.Parent.Name)
                KeepOutWriter.WriteAttributeString("cadSystem", "Autodesk Inventor 2013")
                KeepOutWriter.WriteAttributeString("xmlns:xsi", "http://www.w4.org/2001/XMLSchema-instance")
                KeepOutWriter.WriteAttributeString("xsi:noNamespaceSchemaLocation", "C:/Users") '###Fragen!!!

                ' Elements
                '************************************************************************************
                KeepOutWriter.WriteStartElement("FaceKeepOuts") ' Root

                ' Write out all Attributes
                For i As Integer = 0 To Me.Count - 1

                    KeepOutWriter.WriteStartElement("FaceKeepOut")

                    ' Inventor's internal face name
                    KeepOutWriter.WriteAttributeString("Id", Me.Item(i).Id)
                    KeepOutWriter.WriteAttributeString("faceID", Me.Item(i).FaceId)
                    KeepOutWriter.WriteAttributeString("routingAllowed", Me.Item(i).RoutingAllowed.ToString)

                    KeepOutWriter.WriteEndElement() ' FaceKeepOut
                Next

                KeepOutWriter.WriteEndElement() ' FaceKeepOuts
                KeepOutWriter.WriteEndElement() ' KeepOuts
                KeepOutWriter.WriteEndDocument()

            End Using

        Catch ex As Exception
            MessageBox.Show("There was an error writing to the specified location", _
                            "MID Project", _
                            MessageBoxButtons.OK, _
                            MessageBoxIcon.Error)

        End Try

    End Sub

End Class
