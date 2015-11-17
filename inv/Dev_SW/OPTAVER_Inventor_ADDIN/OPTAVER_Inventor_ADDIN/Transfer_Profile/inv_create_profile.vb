#Region "Namespaces"
Imports Inventor
Imports System.Runtime.InteropServices
Imports System.Type
Imports System.Activator
Imports Projekt_Leiterbahn
Imports System.Text
Imports System.Linq
Imports System.Xml
Imports System.Reflection
Imports System.ComponentModel
Imports System.Collections
Imports System.Collections.Generic
Imports System.Windows
Imports System.Windows.Media.Imaging
Imports System.Windows.Forms
Imports System.Drawing
Imports System.IO
Imports Microsoft.Win32
#End Region



Module inv_create_profile
#Region "Global Variables"
    Public PartFileName As String = ""

#End Region

    '###################################Klasse Profile #####################################

    Public Class Profile


        '############### Einstellen der InventorOberfläche #################################
        Public Function inv_initialize_environment() As Integer

            'Deklaration der "Inventor" variable
            Dim _invApp As Inventor.Application

            '        Dim oSketch As Sketch

            Try
                '"Abholen" der aktuellen Inventor Session
                _invApp = Marshal.GetActiveObject("Inventor.Application")
                _invApp.Visible = True

                'testen ob Part-Datei geöffnet ist
                If test_if_part_exists(_invApp) = 0 Then

                Else
                    If create_part_file(_invApp) = 0 Then
                    Else
                        MsgBox("Part File couldn't be created")

                    End If

                End If

                'oSketch = oPartCompDef.Sketches.Add(oPartCompDef.WorkPlanes.Item(3))

                Return 0

            Catch ex As Exception
                Try
                    Dim invAppType As Type = _
                      GetTypeFromProgID("Inventor.Application")
                    _invApp = CreateInstance(invAppType)
                    Return 1
                Catch ex2 As Exception
                    MsgBox(ex2.ToString())
                    MsgBox("Unable to get or start Inventor")
                End Try
            End Try

        End Function
        '############### Einstellen der InventorOberfläche #################################

        '############################### PI-Profil #########################################
        Public Function inv_create_PI_profile() As Integer

            Dim start_form As New Form_Start_LB
            start_form.ShowDialog()

            Return 0

        End Function
        '############################### PI-Profil #########################################


        '############################### PC-Profil #########################################
        Public Function inv_create_PC_profile() As Integer

            Dim start_form As New Form_Start_LB
            start_form.ShowDialog()

            Return 0
        End Function
        '############################### PC-Profil #########################################


        '############################### Glass-Profil ######################################
        Public Function inv_create_glass_profile() As Integer

            Dim start_form As New Form_Start_LB
            start_form.ShowDialog()

            Return 0

        End Function
        '############################### Glass-Profil ######################################


        '############################### Custom-Profil #####################################
        Public Function inv_create_custom_profile() As Integer

            Dim start_form As New Form_Start_LB
            start_form.ShowDialog()

            Return 0

        End Function
        '############################### Custom-Profil #####################################


        Private Function test_if_part_exists(_invApp As Inventor.Application) As Integer
            Try
                Dim oPartCompDef As Inventor.PartComponentDefinition
                oPartCompDef = _invApp.ActiveDocument.ComponentDefinition
                Return 0
            Catch ex As Exception
                MsgBox("New empty Part-File will be created")
                Return 1
            End Try
        End Function
        '############################### Custom-Profil #####################################


        '############################### Teile Datei anlegen ###############################
        Private Function create_part_file(_invApp As Inventor.Application) As Integer
            Try
                'Teiledokument anlegen
                Dim partDoc As PartDocument = Nothing
                Dim part_creation As New PartCreation
                part_creation.PartFileName = "c:\test.ipt"
                part_creation.CreateOrOpenPart(_invApp)
                Return 0
            Catch ex As Exception
                Return 1
            End Try
        End Function
        '############################### Teile Datei anlegen ###############################
    End Class
    '###################################Klasse Profile #####################################




    '########################### Klasse zum Anlegen einer neuen Part-Datei ###################
    Public Class PartCreation
        Public PartFileName As String
        Public Function CreateOrOpenPart(_invApp As Inventor.Application) As PartDocument
            Dim partDoc As PartDocument = Nothing

            If System.IO.File.Exists(PartFileName) Then
                partDoc = TryCast(_invApp.Documents.Open(PartFileName, True), PartDocument)
                Return partDoc
            Else
                Try
                    partDoc = TryCast(_invApp.Documents.Add(DocumentTypeEnum.kPartDocumentObject, "", True), PartDocument)
                    ' partDoc.SaveAs(PartFileName, False)
                    Return partDoc
                Catch ex As Exception
                    MsgBox("Fehler in Funktion CreateOrOpenPart ")
                End Try

            End If

        End Function

    End Class
    '########################### Klasse zum Anlegen einer neuen Part-Datei ###################

End Module
