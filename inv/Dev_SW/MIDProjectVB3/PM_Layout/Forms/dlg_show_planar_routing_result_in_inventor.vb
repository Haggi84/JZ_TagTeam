'footprint 1.2
#If _useInventor Then



Imports System.Windows.Forms
Imports PM_Layout.WritePlanarRoutingResultToInventor



Public Class dlg_show_planar_routing_result_in_inventor

    Private Sub btnRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRun.Click

        'Private Sub buttonOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRun.Click

        Dim tryAgain As Boolean = True
        Dim succ As Boolean

        While tryAgain

            succ = write_planar_routing_result_to_inventor()

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
#End If