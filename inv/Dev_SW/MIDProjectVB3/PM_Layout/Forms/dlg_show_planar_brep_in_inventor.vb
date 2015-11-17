'footprint 1.2
Imports System.Windows.Forms

#If _useInventor Then
Public Class dlg_show_planar_brep_in_inventor

   Private Sub buttonOk_Click(sender As Object, e As EventArgs) Handles btnRun.Click

      Me.cursor = Cursors.waitcursor

      

      #If Not(_useInventor) Then
         MidMsgBoxProblem("The program is switched to work without Inventor")
         Me.DialogResult = Windows.Forms.DialogResult.No
         Exit Sub
      #End if

      Dim succ As Boolean
      succ = WritePlanarBRepToCadAI.write_planar_brep_to_inventor()

      If Not(succ) Then
         MidMsgBoxProblem("ShowPlanBRepInCadAI failed.")
         Me.DialogResult = Windows.Forms.DialogResult.No
      Else
         MidMsgBoxInformation("ShowPlanBRepInCadAI successful")
         Me.DialogResult = Windows.Forms.DialogResult.OK
      End If
  
   End Sub

   Private Sub buttonCanc(sender As Object, e As EventArgs) Handles btnCanc.Click
      MidMsgBoxCanceled("ShowPlanBRepInCadAI")      
   End Sub

   Private Sub Form_Load(sender As Object, e As System.EventArgs) Handles MyBase.Load      
   
      Dim toolTip As New ToolTip()
      
      toolTip.AutoPopDelay = 5000
      toolTip.InitialDelay = 500
      toolTip.ReshowDelay  = 500
      toolTip.ShowAlways   = True      
   
      toolTip.SetToolTip(Me,         "Export planar BRep to running Autodesk Inventor")   
      toolTip.SetToolTip(Me.btnCanc, "Terminates the function")
   End Sub

End Class

#End If