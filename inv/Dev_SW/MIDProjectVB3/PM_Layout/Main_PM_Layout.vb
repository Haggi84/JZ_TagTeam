'footprint 1.2
Imports System.Windows.Forms

Module Main_PM_Layout

   Sub Main()

      ' Laden der erforderlichen DLLs 
      Dim handle As IntPtr = LoadLibraryA("c_interfaced.dll")

      ' Konfiguration von Testparametern
        Dim succ As Integer = configureTestTools()
        If succ <> 0 Then
            MidMsgBoxProblem("Missing file TestParameters.ini" + vbLf + "in directory PM_Layout_CPP/application/bin/ini")
            Exit Sub
        End If

        ' Initiierung des Druckes von Print-Protokollen, falls dies in TestParameters.ini so festgelegt wurde
        ' Bitte nicht auskommentieren:

        '!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        succ = initPrintProtocol()                       '!!!
        If succ <> 0 Then                                '!!!
            MidMsgBoxProblem("initPrintProtocol failed") '!!!
            Exit Sub                                     '!!!
        End If                                           '!!!
        '!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


      ' Hauptdialog
      Dim dlg As New dlg_start_pm_layout 
      dlg.ShowDialog()
      MidMsgBoxCanceled("""Product Model and Layout""")
            
   End Sub

End Module
