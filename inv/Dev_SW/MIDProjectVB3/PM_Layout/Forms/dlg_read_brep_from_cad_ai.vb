'footprint 1.2
Imports System.Windows.Forms

Public Class dlg_read_brep_from_cad_ai

   Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRun.Click      

      Me.cursor = Cursors.waitcursor

      #If Not(_useInventor) Then
         MidMsgBoxProblem("The program is switched to work without Inventor")
         Me.DialogResult = Windows.Forms.DialogResult.No
         Exit Sub
      #End if
      
#If _useInventor Then

        Dim tryAgain As Boolean = True
        Dim succ As Boolean

        GlobVar.printKeys = Me.chbWithKeys.Checked
        GlobVar.additionalPrecision = Me.cmbAddPrec.Text

        While tryAgain
            Me.Cursor = Cursors.WaitCursor
            succ = ReadBrepFromCadAI.readBrepFromCadAI()
            Me.Cursor = Cursors.Default
            If Not (succ) Then
                tryAgain = MidMsgBoxQuestion("ReadBrepAI failed. Try it again?")
                If Not (tryAgain) Then
                    Me.DialogResult = Windows.Forms.DialogResult.No
                End If
            Else
                MidMsgBoxInformation("ReadBrepAI successful")
                tryAgain = False
                Me.DialogResult = Windows.Forms.DialogResult.OK
            End If
        End While
#End If

    End Sub

    Private Sub buttonCanc(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCanc.Click
#If _useInventor Then
        MidMsgBoxCanceled("ReadBrepAI")
#End If
    End Sub

   Private Sub Form_Load(sender As Object, e As System.EventArgs) Handles MyBase.Load

      Dim toolTip As New ToolTip()
   
      toolTip.AutoPopDelay = 5000
      toolTip.InitialDelay = 500
      toolTip.ReshowDelay  = 500
      toolTip.ShowAlways   = True
   
      toolTip.SetToolTip(Me,             "Imports BRep from running Autodesk Inventor project" + vbCrLf + "and writes it to a file") 
      toolTip.SetToolTip(Me.chbWithKeys, "Transfer the reference keys of BRep objects into the XML-file")
      toolTip.SetToolTip(Me.btnCanc,     "Terminates the function")
      Dim TTAC As String = "Transfer additional decimal places" + vbCrLf + "for length and angles into the XML-file" 
      toolTip.SetToolTip(Me.cmbAddPrec,  TTAC)
      toolTip.SetToolTip(Me.lblAddPrec,  TTAC)

   End Sub

End Class




