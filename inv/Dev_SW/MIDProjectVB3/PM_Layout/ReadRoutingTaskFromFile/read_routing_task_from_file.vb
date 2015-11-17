'footprint 1.2
' ***************************
' * ReadRoutingTaskFromFile *
' ***************************

' Übertragung einer Routing-Aufgabe aus dem Produkt-
' Modell von MID-Layout (XML) in das OO-Modell

Option Explicit
Imports System.IO

Module ReadRoutingTaskFromFile
   
   Public Function read_routing_task_from_file() As Boolean

      ' XML-Dialog
      ' ==========
      Dim succ As Boolean = file_dialog_RoutingTask_Input_xml()
      If Not(succ)  Then
         MidMsgBoxProblem("ReadRoutingTaskFromFile: file dialog failed")
         Return False
      End If

      ' Routing-Task lesen
      ' ==================
      Dim rc As Integer
      rc = readRoutingTaskFrom_XML(GlobVar.full_FileName_RoutingTask)
      If rc <> 0 Then
         MidMsgBoxProblem("ReadRoutingTaskFromFile found error")
         Return False
      End If

      Return True
   End Function

End Module