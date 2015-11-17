'footprint 1.2
Imports System.Windows.Forms
Imports System.Runtime.InteropServices

Public Class dlg_start_pm_layout

   Private Sub buttonReadBRepFromAI(sender As Object, e As EventArgs) Handles btnRun1.Click
    
#If _useInventor Then
        Dim dlg As dlg_read_brep_from_cad_ai = New dlg_read_brep_from_cad_ai

        dlg.cmbAddPrec.Items.Add("0")
        dlg.cmbAddPrec.Items.Add("1")
        dlg.cmbAddPrec.Items.Add("2")
        dlg.cmbAddPrec.Items.Add("3")

        Dim result As DialogResult
        result = dlg.ShowDialog()
        If result = Windows.Forms.DialogResult.OK Then

        Else

        End If
        dlg.Close()
#Else
        MidMsgBoxProblem("The program is switched to work without Inventor")
        Exit Sub
#End If
   End Sub

   Private Sub buttonReadBRepFromFile(sender As Object, e As EventArgs) Handles btnRun2.Click

      ' Test der Voraussetzungen und ev. Löschen der Häkchen der Nachfolgeaktivitäten
      Dim doIt As Boolean = True
      If (Me.chb2.CheckState = CheckState.Checked) Then
         doIt = MidMsgBoxQuestion("You already red a Brep from file. Will you replace it?")
         If Not(doIt) Then
            Exit Sub
         Else
            If (Me.chb3.CheckState = CheckState.Checked) Then
               Me.chb3.CheckState = CheckState.Unchecked
            End If
            If (Me.chb6.CheckState = CheckState.Checked) Then
               Me.chb6.CheckState = CheckState.Unchecked
            End If
            If (Me.chb7.CheckState = CheckState.Checked) Then
               Me.chb7.CheckState = CheckState.Unchecked
            End If                        
         End If
      End If

      ' Sub-Dialog
      Dim dlg As dlg_read_brep_from_file = New dlg_read_brep_from_file     
      Dim result As DialogResult
      Me.cursor = Cursors.waitcursor
      result = dlg.ShowDialog()
      dlg.Close()
      Me.cursor = Cursors.default

      ' Reaktion auf Resultat 
      If result = Windows.Forms.DialogResult.OK Then
          Me.chb2.CheckState = CheckState.Checked
      End If

   End Sub

   Private Sub buttonBRepPlanarisation(sender As Object, e As EventArgs) Handles btnRun3.Click
   
      ' Test der Voraussetzungen
      If (Me.chb2.CheckState = CheckState.Unchecked) Then
         MidMsgBoxInformation("You have to read BRep from file, first!")
         Exit Sub
      End If
      If (Me.chb3.CheckState = CheckState.Checked) Then
         MidMsgBoxInformation("You already run the planarization of Brep!")
         Exit Sub
      End If

     ' Sub-Dialog
      Dim dlg As dlg_planarisation_of_brep = New dlg_planarisation_of_brep      
      Dim result As DialogResult
      Me.cursor = Cursors.waitcursor
      result = dlg.ShowDialog()
      dlg.Close() 
      Me.cursor = Cursors.default 

      ' Reaktion auf Resultat 
      If result = Windows.Forms.DialogResult.OK Then
         Me.chb3.CheckState = CheckState.Checked
      End If
 
   End Sub


   Private Sub buttonShowPlanarBRepInCadAI(sender As Object, e As EventArgs) Handles btnRun4.Click

     ' Test der Voraussetzungen
      If (Me.chb3.CheckState = CheckState.Unchecked) Then
         MidMsgBoxInformation("You have to run the planarization of Brep, first!")
         Exit Sub
      End If
   
     ' Sub-Dialog
      Dim dlg As dlg_show_planar_brep_in_inventor = New dlg_show_planar_brep_in_inventor      
      Dim result As DialogResult
      Me.cursor = Cursors.waitcursor
      result = dlg.ShowDialog()
      dlg.Close()
      Me.cursor = Cursors.default 

      ' Reaktion auf Resultat 
      If result <> Windows.Forms.DialogResult.OK Then
         '''''''
      End If
   
   End Sub

   Private Sub buttonReadCircuit(sender As Object, e As EventArgs) Handles btnRun5.Click

     ' Test der Voraussetzungen
      Dim doIt As Boolean = True
      If (Me.chb5.CheckState = CheckState.Checked) Then
         doIt = MidMsgBoxQuestion("You already red a circuit. Will you replace it?")
         If Not(doIt) Then
            Exit Sub 
         End If
      End If
    
     ' Sub-Dialog
      Dim dlg    As dlgStartReadCircuitFromXML = New dlgStartReadCircuitFromXML
      Dim result As DialogResult      
      Me.cursor = Cursors.waitcursor
      result = dlg.ShowDialog()
      dlg.Close()
      Me.cursor = Cursors.default 

      ' Reaktion auf Resultat 
      If result = Windows.Forms.DialogResult.OK Then
         Me.chb5.CheckState = CheckState.Checked
      End If

   End Sub

   Private Sub buttonReadRoutingTaskFromFile(sender As Object, e As EventArgs) Handles btnRun6.Click

      ' Test der Voraussetzungen und ev. Löschen der Häkchen der Nachfolgeaktivitäten      
      Dim doIt As Boolean = True
      If (Me.chb2.CheckState = CheckState.Unchecked) Then
         MidMsgBoxInformation("You have to read BRep from file, first!")
         Exit Sub
      End If

      If (Me.chb6.CheckState = CheckState.Checked) Then
         doIt = MidMsgBoxQuestion("You already red a routing task. Will you replace it?")
         If Not(doIt) Then
            Exit Sub 
         Else
            Me.chb6.CheckState = CheckState.Unchecked
            If (Me.chb7.CheckState = CheckState.Checked) Then
               Me.chb7.CheckState = CheckState.Unchecked
            End If
            If (Me.chb8.CheckState = CheckState.Checked) Then
               Me.chb8.CheckState = CheckState.Unchecked
            End If
            If (Me.chb10.CheckState = CheckState.Checked) Then
               Me.chb10.CheckState = CheckState.Unchecked
            End If
         End If
      End If

     ' Sub-Dialog
      Dim dlg As dlg_read_routing_task_from_file = New dlg_read_routing_task_from_file     
      Dim result As DialogResult
      Me.cursor = Cursors.waitcursor
      result = dlg.ShowDialog()
      dlg.Close()
      Me.cursor = Cursors.default

      ' Reaktion auf Resultat 
      If result = Windows.Forms.DialogResult.OK Then
         Me.chb6.CheckState = CheckState.Checked
      End If

   End Sub

   Private Sub buttonPlanRoutingTask(sender As Object, e As EventArgs) Handles btnRun7.Click

      ' Test der Voraussetzungen
      If (Me.chb7.CheckState = CheckState.Checked) Then
         MidMsgBoxInformation("You already run the planarization of routing task!")
         Exit Sub
      End If
      If (Me.chb6.CheckState = CheckState.Unchecked) Then
         MidMsgBoxInformation("You have to read routing task from file, first!")
         Exit Sub
      End If
      If (Me.chb3.CheckState = CheckState.Unchecked) Then
         MidMsgBoxInformation("You have to run the planarization of BRep, first!")
         Exit Sub
      End If

     ' Sub-Dialog      
      Dim dlg As dlg_planarisation_of_routing_task = New dlg_planarisation_of_routing_task     
      Dim result As DialogResult
      Me.cursor = Cursors.waitcursor
      result = dlg.ShowDialog()
      dlg.Close()
      Me.cursor = Cursors.default

      ' Reaktion auf Resultat 
      If result = Windows.Forms.DialogResult.OK Then
         Me.chb7.CheckState = CheckState.Checked
      End If

   End Sub

   Private Sub buttonPlanarRouting(sender As Object, e As EventArgs) Handles btnRun8.Click

     ' Test der Voraussetzungen
      If (Me.chb7.CheckState = CheckState.Unchecked) Then
         MidMsgBoxInformation("You have to run planarization of routing task, first!")
         Exit Sub
      End If 
      If (Me.chb8.CheckState = CheckState.Checked) Then
         MidMsgBoxInformation("You already run global routing on planar BRep!")
         Exit Sub
      End If

     ' Sub-Dialog      
      Dim dlg As dlg_global_routing_plan = New dlg_global_routing_plan     
      Dim result As DialogResult
      Me.cursor = Cursors.waitcursor
      result = dlg.ShowDialog()
      dlg.Close()
      Me.cursor = Cursors.default

      ' Reaktion auf Resultat 
      If result = Windows.Forms.DialogResult.OK Then
         Me.chb8.CheckState = CheckState.Checked
      End If

   End Sub

Private Sub buttonShowPlanRouting(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRun9.Click

        ' Test der Voraussetzungen
        If (Me.chb8.CheckState = CheckState.Unchecked) Then
            MidMsgBoxInformation("You have to run planar routing, first!")
            Exit Sub
        End If

        Dim dlg As dlg_show_planar_routing_result_in_inventor = New dlg_show_planar_routing_result_in_inventor

        Dim result As DialogResult
        Me.Cursor = Cursors.WaitCursor
        result = dlg.ShowDialog()
        dlg.Close()
        Me.Cursor = Cursors.Default

        ' Reaktion auf Resultat 
        If result <> Windows.Forms.DialogResult.OK Then
            ''
        End If
    End Sub


     Private Sub buttonMapRouting(sender As Object, e As EventArgs) Handles btnRun10.Click

     ' Test der Voraussetzungen
      If (Me.chb8.CheckState = CheckState.Unchecked) Then
            MidMsgBoxInformation("You have to run planar routing, first!")
            Exit Sub
      End If
    

     ' Sub-Dialog      
      Dim dlg As dlg_map_routing = New dlg_map_routing     
      Dim result As DialogResult
      Me.cursor = Cursors.waitcursor
      result = dlg.ShowDialog()
      dlg.Close()
      Me.cursor = Cursors.default

      ' Reaktion auf Resultat 
      If result = Windows.Forms.DialogResult.OK Then
         Me.chb10.CheckState = CheckState.Checked
      End If

   End Sub

   Private Sub buttonCanc(sender As Object, e As EventArgs) Handles btnCanc.Click

   End Sub

   Private Sub MainForm_Load(sender As Object, e As System.EventArgs) Handles MyBase.Load

      ' Create the ToolTip and associate with the Form container
      Dim toolTip As New ToolTip()
   
      toolTip.AutoPopDelay = 5000
      toolTip.InitialDelay = 500
      toolTip.ReshowDelay  = 500
      toolTip.ShowAlways   = True
   
      Dim TTM As String = "Test components of MID-Layout"
      toolTip.SetToolTip(Me,             TTM)
      toolTip.SetToolTip(Me.lblHeadLine, TTM)
      toolTip.SetToolTip(Me.imgMID,      TTM) 
      toolTip.SetToolTip(Me.gbxBRep,     TTM) 
      toolTip.SetToolTip(Me.gbxRouting,  TTM) 
      
      Dim TTCB As String = "Click Run-Button!"
      toolTip.SetToolTip(Me.chb2, TTCB)
      toolTip.SetToolTip(Me.chb3, TTCB)
      toolTip.SetToolTip(Me.chb5, TTCB)
      toolTip.SetToolTip(Me.chb6, TTCB)
      toolTip.SetToolTip(Me.chb7, TTCB)
      toolTip.SetToolTip(Me.chb8, TTCB)
      toolTip.SetToolTip(Me.chb10,TTCB)
      
      Dim TTL1 As String = "Imports BRep from running Autodesk Inventor project" + vbCrLf + "and writes it to a file"
      toolTip.SetToolTip(Me.lblHeadLine1, TTL1)
      toolTip.SetToolTip(Me.btnRun1,      TTL1)

      Dim TTL2 As String = "Read BRep from a XML file"
      toolTip.SetToolTip(Me.lblHeadLine2, TTL2)
      toolTip.SetToolTip(Me.btnRun2,      TTL2)

      Dim TTL3 As String = "Planarization of BRep within the internal model"
      toolTip.SetToolTip(Me.lblHeadLine3, TTL3)
      toolTip.SetToolTip(Me.btnRun3,      TTL3)

      Dim TTL4 As String = "Exports planar BRep to running Autodesk Inventor"
      toolTip.SetToolTip(Me.lblHeadLine4, TTL4)
      toolTip.SetToolTip(Me.btnRun4,      TTL4)

      Dim TTL5 As String = "Read Circuit from a XML file"
      toolTip.SetToolTip(Me.lblHeadLine5, TTL5)
      toolTip.SetToolTip(Me.btnRun5,      TTL5)

      Dim TTL6 As String = "Read routing task from a XML file"
      toolTip.SetToolTip(Me.lblHeadLine6, TTL6)
      toolTip.SetToolTip(Me.btnRun6,      TTL6)

      Dim TTL7 As String = "Planarization of routing task within the internal model"
      toolTip.SetToolTip(Me.lblHeadLine7, TTL7)
      toolTip.SetToolTip(Me.btnRun7,      TTL7)

      Dim TTL8 As String = "Routing on planar BRep within the internal model"
      toolTip.SetToolTip(Me.lblHeadLine8, TTL8)
      toolTip.SetToolTip(Me.btnRun8,      TTL8)

      Dim TTL9 As String = "Exports planar BRep to running Autodesk Inventor"
      toolTip.SetToolTip(Me.lblHeadLine9, TTL9)
      toolTip.SetToolTip(Me.btnRun9,      TTL9)

      Dim TTL10 As String = "Maps the planar routing result to the 3D-Product Model"
      toolTip.SetToolTip(Me.lblHeadLine10, TTL10)
      toolTip.SetToolTip(Me.btnRun10,      TTL10)

      Dim TTL11 As String = "Exports the 3D routing result to running Autodesk Inventor"
      toolTip.SetToolTip(Me.lblHeadLine11, TTL11)
      toolTip.SetToolTip(Me.btnRun11,      TTL11)

      toolTip.SetToolTip(Me.btnCanc, "Terminates MID-Layout")
   End Sub


    
    Private Sub btnRun11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRun11.Click
        'If (Me.chb10.CheckState = CheckState.Unchecked) Then
        '    MidMsgBoxInformation("You have to run Map planar routing result to 3D first")
        '    Exit Sub
        'End If

        Dim dlg_show_3D_routing_result_in_inventor As New dlg_show_3D_routing_result_in_inventor
        dlg_show_3D_routing_result_in_inventor.ShowDialog()


    End Sub


End Class