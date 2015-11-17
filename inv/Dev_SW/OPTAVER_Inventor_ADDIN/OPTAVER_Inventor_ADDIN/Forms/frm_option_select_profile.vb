Imports GraphPlotter
Imports Inventor


Public Class frm_option_select_profile

    'irgendwas anderes laden....
    Private Sub frm_option_select_profile_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
    '################################ Custom-Profil ###################################
    Private Sub pb_custom_Click(sender As Object, e As EventArgs) Handles pb_custom.Click

        'Aufruf des C# Funktionsgenerators
        Dim graph As New GraphPlotter.Main
        graph.ShowDialog()

        'frm_option_select_profile.ActiveForm.Activate()
        'frm_option_select_profile.ActiveForm.Close()

    End Sub

    '################################ PI-Profil ###################################
    Private Sub pb_PI_Click(sender As Object, e As EventArgs) Handles pb_PI.Click

        'erstellt neue Profile Klasse
        Dim profile = New Profile

        'ruft Methode auf, welche die Inventor Umgebung lädt

        If profile.inv_initialize_environment() = 0 Then
            'lädt das PI-Profil als 2D Sketch, Prüfung ob Funktion ausgeführt werden konnte
            If profile.inv_create_PI_profile = 0 Then

                'Fehlerbehandlung falls PI-Profil nicht geladen werden konnte
            Else
                MsgBox("PI-profile couldn't be loaded")
            End If
            'Wenn Rückgabewert = 1 --> Fehlermeldung das INVENTOR nicht initialisiert werden konnte
        Else
            MsgBox("Unable to intitalize INVENTOR")
        End If

    End Sub
End Class