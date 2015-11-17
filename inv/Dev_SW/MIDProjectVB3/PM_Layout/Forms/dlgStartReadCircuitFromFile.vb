'footprint 1.2
Imports System.Windows.Forms

Public Class dlgStartReadCircuitFromXML

   Private Sub buttonOk_Click(sender As Object, e As EventArgs) Handles btnRun.Click

      Me.cursor = Cursors.waitcursor

      Dim tryAgain As Boolean = True
      Dim succ     As Boolean

      While tryAgain
         succ = readCircuitFromFile()
         If Not(succ) Then
            tryAgain = MidMsgBoxQuestion("ReadCircuitFromFile failed. Try it again?")
            If Not(tryAgain) Then
               Me.DialogResult = Windows.Forms.DialogResult.No
            End If  
         Else
            MidMsgBoxInformation("ReadCircuitFromFile successful")
            tryAgain = False
            Me.DialogResult = Windows.Forms.DialogResult.OK
         End If
      End While      
   End Sub

   Private Sub buttonCanc(sender As Object, e As EventArgs) Handles btnCanc.Click
      MidMsgBoxCanceled("ReadCircuitFromFile")      
   End Sub

   Private Sub Form_Load(sender As Object, e As System.EventArgs) Handles MyBase.Load

      Dim toolTip As New ToolTip()
   
      toolTip.AutoPopDelay = 5000
      toolTip.InitialDelay = 500
      toolTip.ReshowDelay  = 500
      toolTip.ShowAlways   = True   

      toolTip.SetToolTip(Me,         "Read Circuit from a XML file")
      toolTip.SetToolTip(Me.btnCanc, "Terminates the function")
   End Sub

End Class