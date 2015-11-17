Public Class frm_properties_waveguide


    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

    End Sub

    Private Sub Properties_Waveguide_Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

 

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click


        Dim height, width As Double

        If IsNumeric(tb_height.Text) Then
            height = CDbl(tb_height.Text)
        Else
            MsgBox("value height is not numeric!")
        End If

        If IsNumeric(tb_width.Text) Then
            width = CDbl(tb_width.Text)
        Else
            MsgBox("value width is not numeric!")
        End If
       
        'MsgBox("Height: ", height & vbNewLine & "Width: ", width)
        ActiveForm.Close()
        Dim kbe As New Klasse_Bahn_Erstellen
        kbe.Sub_Erstellen(width, height)

    End Sub


    Private Sub tb_height_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tb_height.KeyPress
        ' If InStr("0123456789" & Chr(8), e.KeyChar) = False Then e.Handled = True
    End Sub
End Class