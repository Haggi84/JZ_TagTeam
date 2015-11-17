Option Explicit On

Imports Inventor
Imports System
Imports System.Xml
Imports System.Collections.Generic
Imports System.Windows.Forms

'#############################################
' KeepOuts Class
'############################################

Public Class KeepOuts
    Inherits List(Of KeepOut)

    Private oAddIn As Inventor.Application
    Private oServer As MidAddInServer

    Private oXmlReader As XmlTextReader
    Private oParent As CircuitCarrier

    ' Constructor
    '***************************************************************************************************************
    Public Sub New(parent As CircuitCarrier, _
                   addIn As Inventor.Application, _
                   server As MidAddInServer)

        MyBase.New()
        Me.oParent = parent
        Me.oAddIn = addIn
        Me.oServer = server

    End Sub

    ' Add KeepOuts to the KeepOuts list
    '***************************************************************************************************************
    Public Overloads Sub Add(ByRef face As FaceProxy, _
                             Optional _routingAllowed As Boolean = True, _
                             Optional _faceId As String = Nothing)

        ' Do only add the face if it does not already exist 
        If Me.IdExists(face.InternalName) = False Then
            ' Create new keep out
            Dim oKeepOut As KeepOut = New KeepOut(oParent, oAddIn, oServer, face, _routingAllowed, _faceId)
            MyBase.Add(oKeepOut)

            ' Add browser node
            oKeepOut.AddBrowserNode()

            ' Add attributes
            oKeepOut.AddAttributes()

            ' Change color to red
            Try
                Dim oAsmDoc As AssemblyDocument = oAddIn.ActiveDocument()
                Dim oStyle As RenderStyle = oAsmDoc.RenderStyles.Item("Smooth - Red")
                oKeepOut.Face.NativeObject.SetRenderStyle(StyleSourceTypeEnum.kOverrideRenderStyle, oStyle)
                'oAddIn.ActiveView.Update()
            Catch ex As Exception
                MessageBox.Show("The asset could not be found", "MID Project", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
            'oAddIn.ActiveView.Update()

        End If
    End Sub

    ' Remove: Search KeepOut belonging to the face, change color and finally remove it
    '***************************************************************************************************************
    Public Overloads Sub Remove(ByRef oFace As FaceProxy)

        ' Do only remove the face if it already exists
        If Me.IdExists(oFace.InternalName) Then

            Dim oAsmDoc As AssemblyDocument = oAddIn.ActiveDocument()
            For Each oKeepOut As KeepOut In Me
                If oKeepOut.Id.Equals(oFace.InternalName) Then

                    ' Change color back to default
                    Dim oStyle As RenderStyle = oKeepOut.Face.Parent.Parent.GetRenderStyle(StyleSourceTypeEnum.kOverrideRenderStyle)
                    oKeepOut.Face.NativeObject.SetRenderStyle(StyleSourceTypeEnum.kOverrideRenderStyle, oStyle)
                    'oAddIn.ActiveView.Update()

                    ' Delete browser node
                    oKeepOut.DeleteBrowserNode()

                    ' Delete attributes
                    oKeepOut.DeleteAttributes()

                    ' Remove from List
                    MyBase.Remove(oKeepOut)
                    oKeepOut = Nothing

                    Exit Sub
                End If
            Next

        End If

    End Sub

    ' Clear: Change color before deleting items
    '***************************************************************************************************************
    Public Overloads Sub Clear()
        Dim oAsmDoc As AssemblyDocument = oAddIn.ActiveDocument()
        For Each oKeepOut As KeepOut In Me

            Dim oStyle As RenderStyle = oKeepOut.Face.Parent.Parent.GetRenderStyle(StyleSourceTypeEnum.kOverrideRenderStyle)
            oKeepOut.Face.NativeObject.SetRenderStyle(StyleSourceTypeEnum.kOverrideRenderStyle, oStyle)
            'oAddIn.ActiveView.Update()

            ' Delete attributes
            oKeepOut.DeleteAttributes()

            ' Delete browser node
            oKeepOut.DeleteBrowserNode()

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
    Public Sub WriteXml(filePath As String)

        If Me.Count <= 0 Then
            Exit Sub
        End If

        ' Export only when checkbox is set 
        'If oKeepOutDlg.writeXmlCheck.CheckState() = False Then
        '    Exit Sub
        'End If

        Try

            Dim oAsmDoc As AssemblyDocument = oAddIn.ActiveDocument

            'Dim filePath As String = oServer.CommandCollection.WorkDirectory 'NewProjectCommand.GetWorkDirPath()
            filePath = filePath & "\KeepOuts.xml"

            ' Prepare KeepOutWriter for Garbage collector
            Using KeepOutWriter As XmlTextWriter = New XmlTextWriter(filePath, Nothing)

                ' Header
                '************************************************************************************
                KeepOutWriter.Formatting = Formatting.Indented

                KeepOutWriter.WriteStartDocument()

                KeepOutWriter.WriteStartElement("KeepOuts")

                KeepOutWriter.WriteAttributeString("project", oAsmDoc.DisplayName)
                KeepOutWriter.WriteAttributeString("denotation", Me.Item(1).Face.Parent.Parent.Name)
                KeepOutWriter.WriteAttributeString("cadSystem", "Autodesk Inventor 2013")
                KeepOutWriter.WriteAttributeString("xmlns:xsi", "http://www.w4.org/2001/XMLSchema-instance")
                KeepOutWriter.WriteAttributeString("xsi:noNamespaceSchemaLocation", "C:/Users") '++++Fragen!!!

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

        Catch directoryNotFound As System.IO.DirectoryNotFoundException
            MessageBox.Show("Directory has been deleted", "MID Project", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show("There was an error writing to the specified location", "MID Project", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

End Class
