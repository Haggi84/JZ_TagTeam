'footprint 1.2
Imports System.Windows.Forms

Public Class dlg_planarisation_of_routing_task

   Private Sub buttonOk_Click(sender As Object, e As EventArgs) Handles btnRun.Click  
    
      Me.cursor = Cursors.waitcursor

      Dim rc As Integer = planarizationOfRoutingTask()

      Dim message As String

      message = ""

      Select Case rc

         Case 0
            message = ""
         Case 1
            message = "PlanRoutingTask failed: No product model"
         Case 2
            message = "PlanRoutingTask failed: No layout model"
         Case 3
            message = "PlanRoutingTask failed: No original BRep"
         Case 4
            message = "PlanRoutingTask failed: No planar BRep"
         Case 5
            message = "PlanRoutingTask failed: No original Routing-Task"
         Case 6
            message = "PlanRoutingTask failed: in layout model already exists planar routing task"
         Case 7
            message = "PlanRoutingTask failed: in routing task is a wrong BRep name stored"
         Case 8
            message = "PlanRoutingTask failed: planarization of a net-routing-task failed"
      End Select
 
      If rc <> 0 Then
         MidMsgBoxProblem(message)
         Me.DialogResult = Windows.Forms.DialogResult.No
      Else
         MidMsgBoxInformation("PlanRoutingTask successful")
         Me.DialogResult = Windows.Forms.DialogResult.OK
      End If
  
   End Sub

   Private Sub Form_Load(sender As Object, e As System.EventArgs) Handles MyBase.Load

      Dim toolTip As New ToolTip()
      
      toolTip.AutoPopDelay = 5000
      toolTip.InitialDelay = 500
      toolTip.ReshowDelay  = 500
      toolTip.ShowAlways   = True
      
      toolTip.SetToolTip(Me,         "Planarization of routing task within the internal model")
      toolTip.SetToolTip(Me.btnCanc, "Terminates the function")
   End Sub

End Class