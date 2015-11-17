Imports System.Xml
Imports System.IO
Imports System.Text
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports Inventor

Public Class ExportKeepOuts


    Private oKeepOutList As New List(Of Face)
    Private MIDAddin As Inventor.Application

    Public Sub New(ByRef MIDAddin As Inventor.Application, ByRef oKeepOutList As List(Of Face))

        MyBase.New()


    End Sub




    Public Sub WriteXML()

        '#######################################################
        ' File dialog
        '#######################################################
        'Dim oFileDlg As FileDialog

        'MIDAddin.CreateFileDialog(oFileDlg)

        'oFileDlg.Filter = ""

        'oFileDlg.DialogTitle = "Select file location"

        'oFileDlg.InitialDirectory = "F:\Users\Paul\Bachelor's Thesis\CAD\Inventor Addin\MIDProjectVB3\MIDProjectVB3\Models\"

        'oFileDlg.CancelError = False
        'oFileDlg.MultiSelectEnabled = False
        'oFileDlg.OptionsEnabled = False

        'oFileDlg.ShowSave()

        'sgBox("File " & oFileDlg.FileName & " was selected")

        Dim sFilePath As String = NewProjectCommand.GetWorkDirPath()
        sFilePath = sFilePath & "\KeepOuts.xml"

        'Dim oFileStream As FileStream = File.Create(sFilePath)

        Using KeepOutWriter As XmlTextWriter = New XmlTextWriter(sFilePath, Nothing)

            KeepOutWriter.Formatting = Formatting.Indented

            KeepOutWriter.WriteStartDocument()

            KeepOutWriter.WriteStartElement("KeepOuts")

            KeepOutWriter.WriteAttributeString("project", "MID-Auto_stp")
            KeepOutWriter.WriteAttributeString("denotation", "MID-XML-BRepModel")
            KeepOutWriter.WriteAttributeString("cadSystem", "Autodesk Inventor 2013")
            KeepOutWriter.WriteAttributeString("xmlns:xsi", "http://www.w4.org/2001/XMLSchema-instance")
            KeepOutWriter.WriteAttributeString("xsi:noNamespaceSchemaLocation", "C:/Users")

            KeepOutWriter.WriteStartElement("FaceKeepOuts") ' Root

            ' Write all KeepOut faces
            For i As Integer = 0 To oKeepOutList.Count - 1

                KeepOutWriter.WriteStartElement("FaceKeepOut")
                KeepOutWriter.WriteAttributeString("faceID", oKeepOutList.Item(i).InternalName)
                KeepOutWriter.WriteAttributeString("routingAllowed", "true")
                KeepOutWriter.WriteEndElement()
            Next

            KeepOutWriter.WriteEndElement()
            KeepOutWriter.WriteEndElement()
            KeepOutWriter.WriteEndDocument()

        End Using
        ' (Using: clear memory after export)
        MessageBox.Show("KeepOuts have been successfully exported to" & vbNewLine & sFilePath, _
                        "MIDProject", _
                        MessageBoxButtons.OK, _
                        MessageBoxIcon.Asterisk)


    End Sub

End Class
