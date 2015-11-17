'footprint 1.2
#If _useInventor Then



Imports System.Windows.Forms
Imports PM_Layout.WritePlanarRoutingResultToCadAi


Public Class dlg_show_routing_result_in_CAD_AI

    Private Sub buttonOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRun.Click

        Dim tryAgain As Boolean = True
        Dim succ As Boolean

        While tryAgain

            'succ = write_planar_routing_result_to_XML()

            'If Not (succ) Then
            '    tryAgain = MidMsgBoxQuestion("WritingRoutingResult failed. Try it again?")
            '    If Not (tryAgain) Then
            '        Me.DialogResult = Windows.Forms.DialogResult.No
            '    End If

            'Else

            '    MidMsgBoxInformation("ReadRoutingTaskFromFile successful")
            '    tryAgain = False
            '    Me.DialogResult = Windows.Forms.DialogResult.OK

            'End If
        End While


    End Sub

    Private Sub buttonCanc(ByVal sender As Object, ByVal e As EventArgs) Handles btnCanc.Click
    End Sub


    Private Sub imgMID_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgMID.Click

    End Sub
    Private Sub lblHeadline_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblHeadline.Click

    End Sub
    Private Sub lblHeadline2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblHeadline2.Click

    End Sub
End Class
#End If
