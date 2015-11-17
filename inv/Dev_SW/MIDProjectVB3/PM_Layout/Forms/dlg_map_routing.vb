'footprint 1.2
Imports System.Windows.Forms

Public Class dlg_map_routing
   Private Sub buttonOk_Click(sender As Object, e As EventArgs) Handles btnRun.Click   
  
      Me.cursor = Cursors.waitcursor

      Dim rc As Integer = map_planar_routing_result_to_3D()
 
      If rc <> 0 Then
         MidMsgBoxProblem("Map planar routing result to 3D failed")
         Me.DialogResult = Windows.Forms.DialogResult.NO
      Else
         MidMsgBoxInformation("Map planar routing result to 3D is successful")
         Me.DialogResult = Windows.Forms.DialogResult.OK
      End If
  
   End Sub

   Private Sub buttonCanc(sender As Object, e As EventArgs) Handles btnCanc.Click
      MidMsgBoxCanceled("Map Routing")      
   End Sub

   Private Sub Form_Load(sender As Object, e As System.EventArgs) Handles MyBase.Load

      Dim toolTip As New ToolTip()
   
      toolTip.AutoPopDelay = 5000
      toolTip.InitialDelay = 500
      toolTip.ReshowDelay  = 500
      toolTip.ShowAlways   = True
   
      toolTip.SetToolTip(Me,         "Map planar routing result on 3D-BRep")
      toolTip.SetToolTip(Me.btnCanc, "Terminates the function")
   End Sub
End Class