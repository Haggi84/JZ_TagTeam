'footprint 1.2
Imports System.Windows.Forms

Public Class dlg_planarisation_of_brep

   Private Sub buttonOk_Click(sender As Object, e As EventArgs) Handles btnRun.Click   
  
      Me.cursor = Cursors.waitcursor

      Dim rc As Integer = brepProcessing()
 
      If rc <> 0 Then
         MidMsgBoxProblem("BRepPlanarisation failed")
         Me.DialogResult = Windows.Forms.DialogResult.NO
      Else
         MidMsgBoxInformation("BRepPlanarisation successful")
         Me.DialogResult = Windows.Forms.DialogResult.OK
      End If
  
   End Sub

   Private Sub buttonCanc(sender As Object, e As EventArgs) Handles btnCanc.Click
      MidMsgBoxCanceled("BRepPlanarisation")      
   End Sub

   Private Sub Form_Load(sender As Object, e As System.EventArgs) Handles MyBase.Load

      Dim toolTip As New ToolTip()
   
      toolTip.AutoPopDelay = 5000
      toolTip.InitialDelay = 500
      toolTip.ReshowDelay  = 500
      toolTip.ShowAlways   = True
   
      toolTip.SetToolTip(Me,         "Planarization of BRep within the internal model")
      toolTip.SetToolTip(Me.btnCanc, "Terminates the function")
   End Sub

End Class