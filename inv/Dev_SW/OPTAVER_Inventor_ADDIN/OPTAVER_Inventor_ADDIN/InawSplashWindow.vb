Imports System.Windows.Forms
Imports System.Drawing

Public Class InawSplashWindow
    Private form As Form = Nothing
    Private timer As Timer = Nothing

    Public Sub New(ByVal width As Integer, ByVal height As Integer, ByVal interval_milisecs As Integer, ByVal image As Image)
        form = New Form()

        form.Width = width
        form.Height = height

        form.BackgroundImage = image
        form.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch

        form.StartPosition = FormStartPosition.CenterScreen
        form.FormBorderStyle = FormBorderStyle.None
        form.TopMost = True

        timer = New Timer()
        timer.Interval = interval_milisecs
        AddHandler timer.Tick, New EventHandler(AddressOf timer_Tick)
        timer.Start()

        form.Show()
    End Sub

    Private Sub timer_Tick(ByVal sender As Object, ByVal args As EventArgs)
        timer.Stop()
        form.Close()
    End Sub
End Class


