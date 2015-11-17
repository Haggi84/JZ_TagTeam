Option Explicit On
Imports Inventor
Imports System.Runtime.InteropServices
Imports Microsoft.Win32
Imports System.Drawing
Imports System.Windows.Forms


'Public oFace As Inventor.Face

Public Class Form_Start_LB


    ''Wichtig! Objekte in den Subs Instanzzieren. Nicht hier vor den Subs z.B.
    ''Dim oForm_Bahnerstellen As Form_Bahnerstellen = New Form_Bahnerstellen
    ''Sonst können WindowsForms nach dem schließen nicht mehr geöffnet werden
    Public decision As Integer


    Private Sub Button1_Click_1(sender As System.Object, e As System.EventArgs) Handles ButtonBahnerstellen.Click
        Dim oOptionSelectprofile As frm_option_select_profile = New frm_option_select_profile
        oOptionSelectprofile.Show()

        'Dim oForm_Bahnerstellen As Form_Bahnerstellen = New Form_Bahnerstellen
        'oForm_Bahnerstellen.Show()
    End Sub


    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles ButtonInfoZeichnen.Click
        Dim oInfoZeichnen As Form_Info_Zeichnen = New Form_Info_Zeichnen
        oInfoZeichnen.Show()
    End Sub


    Private Sub Button_Info_Bahnerstellen(sender As System.Object, e As System.EventArgs) Handles ButtonInfoBahnerstellen.Click
        ''Bei jedem Klick wird eine neue Instanz erzeugt
        ''Wenn der Konstruktor nicht innerhalb der Sub steht, wird einmalig pro inventor session, ein Form_Info_Bahnerstellen windows-form-objekt erzeugt. Wenn diese einmalig erstellte windows-form geschlossen wird, kann sie kein zweites mal geöffent werden. Steht der Konstruktor in der Sub gibt es keine Fehlermeldung mehr weil jedes mal eine neue Instanz erzeugt wird und nicht mehr nach der alten gesucht wird, die gelöscht wurde wenn man die Form schließt.
        Dim oInfoBahnerstellen As Form_Info_Bahnerstellen = New Form_Info_Bahnerstellen
        'oInfoBahnerstellen.Show(Me)
        oInfoBahnerstellen.Show()
    End Sub

    Private Sub ButtonZeichnen_Click(sender As System.Object, e As System.EventArgs) Handles ButtonZeichnen.Click
        Dim oZeichnen As Klasse_Linien_Zeichnen = New Klasse_Linien_Zeichnen
        oZeichnen.Sub_LinienZeichnen()
    End Sub

    Private Sub Form_Groß_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        MsgBox("Löscht die Leiterbahn-Linie die als letztes gezeichnet wurde")
    End Sub

    Private Sub Button_Loeschen_Click(sender As System.Object, e As System.EventArgs) Handles Button_Loeschen.Click
        Dim oLinien_Loeschen As Klasse_Linien_Loeschen = New Klasse_Linien_Loeschen
        oLinien_Loeschen.DeleteLinesFunction()
    End Sub

    Private Sub Button_Info_Ueberschn_Click(sender As System.Object, e As System.EventArgs) Handles Button_Info_Ueberschn.Click
        MsgBox("Prueft ob sich eine Leiterbahnlinie selbst schneidet")
    End Sub

    Private Sub Button_Ueberschneidung_Click(sender As System.Object, e As System.EventArgs) Handles Button_Ueberschneidung.Click
        Dim oUeberschneidung As Klasse_Ueberschneidung = New Klasse_Ueberschneidung
        oUeberschneidung.IntersectionFunction()
    End Sub

    Private Sub btn_import_carrier_Click(sender As Object, e As EventArgs)

        OpenDoc()

    End Sub

    Public Sub OpenDoc()
        Try

            Dim fd As New OpenFileDialog
            fd.ShowDialog()
            Dim fn As String = fd.FileName
            Dim oDoc As Document
            Dim _InvApplication As Inventor.Application
            _InvApplication = Marshal.GetActiveObject("Inventor.Application")
            _InvApplication.Visible = True
            oDoc = _InvApplication.Documents.Open(fn)

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try




    End Sub

    Private Sub btn_entrance_light_Click(sender As Object, e As EventArgs) Handles btn_entrance_light.Click
        Dim entrance As New Klasse_Eintritt_Festlegen
        entrance.Eintritt_festlegen()
    End Sub

    Private Sub btn_create_vector_Click(sender As Object, e As EventArgs) Handles btn_create_vector.Click

        Dim vect As New Klasse_Vektor_Erstellen
        vect.Vektor_erstellen()

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs)
        MsgBox("Hello World")

    End Sub
End Class