Public Class frm_main

    Private Sub btn_new_project_Click(sender As System.Object, e As System.EventArgs) Handles btn_new_project.Click

    End Sub

    Private Sub btn_cmpnt_send_Click(sender As Object, e As EventArgs) Handles btn_cmpnt_send.Click

    End Sub

    Private Sub ToolTip1_Popup(sender As Object, e As Windows.Forms.PopupEventArgs) Handles ToolTip1.Popup

    End Sub

    Private Sub PictureBox1_MouseEnter(sender As Object, e As EventArgs) Handles btn_new_project.MouseEnter
    End Sub


    Private Sub btn_conductor_Click(sender As Object, e As EventArgs) Handles btn_conductor.Click

        Dim frm_option_select_profile As New frm_option_select_profile
        frm_option_select_profile.ShowDialog()


    End Sub

    Private Sub btn_BRep_Click(sender As Object, e As EventArgs) Handles btn_BRep.Click
        Dim rbrep As New frm_read_brep_from_cad_ai
        rbrep.ShowDialog()
    End Sub
End Class