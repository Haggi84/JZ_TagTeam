'Imports GraphPlotter
Imports Inventor
Imports System.Drawing




Public Class frm_option_select_profile

    'irgendwas anderes laden....
    'Private Sub frm_option_select_profile_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    'End Sub
  

    '################################ PI-Profil ###################################

    Private Sub btnPIProfile_Click(sender As Object, e As EventArgs) Handles btnPIProfile.Click
        ActiveForm.Close()
        Dim frm_prop As frm_properties_waveguide = New frm_properties_waveguide

        frm_prop.PictureBox1.Height = 138
        frm_prop.PictureBox1.Width = 176
        frm_prop.PictureBox1.Image = img_PI_curve
        frm_prop.PictureBox1.SizeMode = Windows.Forms.PictureBoxSizeMode.StretchImage
        frm_prop.Show()

        'erstellt neue Profile Klasse
        'Dim profile = New Profile

        'ruft Methode auf, welche die Inventor Umgebung lädt

        'If profile.inv_initialize_environment() = 0 Then
        '    'lädt das PI-Profil als 2D Sketch, Prüfung ob Funktion ausgeführt werden konnte
        '    If profile.inv_create_PI_profile = 0 Then

        '        'Fehlerbehandlung falls PI-Profil nicht geladen werden konnte
        '    Else
        '        MsgBox("PI-profile couldn't be loaded")
        '    End If
        'Wenn Rückgabewert = 1 --> Fehlermeldung das INVENTOR nicht initialisiert werden konnte
        'Else
        'MsgBox("Unable to intitalize INVENTOR")
        'End If
    End Sub
    '################################ Custom-Profil ###################################

    Private Sub btnCustomProfile_Click(sender As Object, e As EventArgs) Handles btnCustomProfile.Click
        ActiveForm.Close()
        'Aufruf des C# Funktionsgenerators
        Dim graph As New GraphPlotter.Main

        graph.ShowDialog()

        'frm_option_select_profile.ActiveForm.Activate()
        'frm_option_select_profile.ActiveForm.Close()
    End Sub


    Private Sub btnPCProfile_Click(sender As Object, e As EventArgs) Handles btnPCProfile.Click
        ActiveForm.Close()
        Dim frm_prop As frm_properties_waveguide = New frm_properties_waveguide
        frm_prop.PictureBox1.Height = 138
        frm_prop.PictureBox1.Width = 235
        frm_prop.PictureBox1.Image = img_PC_curve
        frm_prop.PictureBox1.SizeMode = Windows.Forms.PictureBoxSizeMode.StretchImage

        frm_prop.Show()
    End Sub

    Private Sub btnGlassProfile_Click(sender As Object, e As EventArgs) Handles btnGlassProfile.Click
        ActiveForm.Close()
        Dim frm_prop As frm_properties_waveguide = New frm_properties_waveguide
        frm_prop.PictureBox1.Height = 127
        frm_prop.PictureBox1.Width = 173
        frm_prop.PictureBox1.Image = img_Glass_curve
        frm_prop.PictureBox1.SizeMode = Windows.Forms.PictureBoxSizeMode.StretchImage

        frm_prop.Show()
    End Sub
End Class