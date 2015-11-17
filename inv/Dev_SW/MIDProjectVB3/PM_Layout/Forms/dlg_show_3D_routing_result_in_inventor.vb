'footprint 1.2
Public Class dlg_show_3D_routing_result_in_inventor

    Private Sub btnRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRun.Click

        Dim tryAgain As Boolean = True
        Dim succ As Boolean

        While tryAgain

            succ = write_3D_routing_result_to_inventor()

            If Not (succ) Then
                tryAgain = MidMsgBoxQuestion("WritingRoutingResult failed. Try it again?")
                If Not (tryAgain) Then
                    Me.DialogResult = Windows.Forms.DialogResult.No
                End If

            Else

                MidMsgBoxInformation("ReadRoutingTaskFromFile successful")
                tryAgain = False
                Me.DialogResult = Windows.Forms.DialogResult.OK

            End If
        End While

    End Sub
End Class