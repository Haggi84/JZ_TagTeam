'footprint 1.2
' *********
' * Files *
' *********

' File-Dialoge

Imports System.Windows.Forms
Imports System.IO

Module Files

   ' Globale Variable für diesen Modul

   ' Initiale Pfade für OpenFileDialog
    Public Const brepPath0 As String = "..\..\..\..\..\PM_Layout_CPP\application\bin\XML"
    Public Const brepPath1 As String = "../../../../../PM_Layout_CPP/application/bin/XML"
   Private Const schemeBrepPath0  As String = "..\..\..\..\..\PM_Layout_CPP\application\bin\XML"
   Private Const circuitPath0     As String = "..\..\..\..\..\PM_Layout_CPP\application\bin\XML"
   Private Const routingTaskPath0 As String = "..\..\..\..\..\PM_Layout_CPP\application\bin\XML"

   ' Dateinamen für OpenFileDialog 
   Private brepName0, schemeBrepName0, circuitName0, routingTaskName0 As String

   ' ***********************************
   ' * fileDialogsForReadBrepFromCadAI *
   ' ***********************************

   ' File-Dialog zum XSD-Scheme-File und zum XML-Output-File

   Public Function fileDialogsForReadBrepFromCadAI() As Boolean
      Dim suc As Boolean

      ' XSD-Dialog
      ' ==========
      suc = fileDialogBRepInputXSD()

      If Not(suc) Then         
         MidMsgBoxProblem("readBRep: XSD-file dialog failed")
         Return False
      End If

      ' XML-Output-Dialog
      ' =================
      suc = fileDialogBRepOutputXML()
      If Not(suc) Then
         MidMsgBoxProblem("readBRep: XML-file dialog failed")
         Return False
      End If

      Return True
   End Function


   ' **************************
   ' * fileDialogBRepInputXSD *
   ' **************************

   ' File-Dialog zum XSD-Scheme-File

   Private Function fileDialogBRepInputXSD() As Boolean

      schemeBrepName0 = "BRepMID.xsd"

      ' File-Dialog kreieren
      Dim fD As New Windows.Forms.OpenFileDialog

      ' Titel
      fD.Title = "Scheme File (XSD)"

      ' Filter
      Dim extension As String
      extension = "*.xsd"
      fD.Filter = "XSD-files|" + extension

      ' Initiales Directory und initialen File-Namen festlegen      
      fD.InitialDirectory = schemeBrepPath0
      fD.FileName         = schemeBrepName0

      ' Dialog zeigen und bedienen
      Dim result As DialogResult
      result = fD.ShowDialog()

      ' Auswertung
      If result = Windows.Forms.DialogResult.Cancel Then
         Return False
      End If

      fullSchemeName = fD.FileName
      fullSchemeName = Replace(fullSchemeName, "\", "/", 1)
      Return True

   End Function

   ' *******************************
   ' * fileDialogBRepOutputXML *
   ' *******************************

   ' File-Dialog zur Beeinflussung des Pfades und des Namen des XML-BRep-Files

   Private Function fileDialogBRepOutputXML() As Boolean

      brepName0 = "TWA.xml"

      Dim extension As String
      extension = "*.xml"

      ' File-Dialog kreieren
      Dim fD As New Windows.Forms.SaveFileDialog      

      ' the title for the dialog.
      fD.Title = "Output File BRep (XML)"

      ' Define the filter
      fD.Filter = "XML-files|" + extension

      ' the initial directory that will be displayed in the dialog
      fD.InitialDirectory = schemeBrepPath0
      fD.FileName         = brepName0

      ' Dialog zeigen und bedienen
      Dim result As DialogResult
      result = fD.ShowDialog()

      ' Auswertung
      If result = Windows.Forms.DialogResult.Cancel Then
         Return False
      End If
      GlobVar.full_FileName_BRep = fD.FileName
      GlobVar.full_FileName_BRep = Replace(GlobVar.full_FileName_BRep, "\", "/", 1)
      Return True

   End Function

   ' ******************************
   ' * file_dialog_BRep_Input_xml *
   ' ******************************

   ' File-Dialog zum XML-BRep-File (Input)

   Public Function fileDialogBRepInputFromFile() As Boolean

      brepName0 = "TWA.xml"

      ' File-Dialog kreieren
      Dim fD As New Windows.Forms.OpenFileDialog      

      ' Titel
      fD.Title = "BRep File (XML)"

      ' Filter
      Dim extension As String
      extension = "*.xml"
      fD.Filter = "XML-files|" + extension

      ' Initiales Directory und initialen File-Namen festlegen
      fD.InitialDirectory = brepPath0
      fD.FileName         = brepName0

      ' Dialog anzeigen und bedienen
      Dim result As DialogResult = fD.ShowDialog()

     ' Auswertung
      If result = Windows.Forms.DialogResult.Cancel Then
         MidMsgBoxProblem("ReadBRepFromFile: file dialog canceled by user")
         Return False
      Else
         GlobVar.full_FileName_BRep = fD.FileName         
         GlobVar.full_FileName_BRep = Replace(GlobVar.full_FileName_BRep, "\", "/", 1)
         ' In der globalen Variablen full_FileName_BRep steht das Resultat des File-Dialoges
         GlobVar.name_of_BRep = GlobVar.full_FileName_BRep
         extractNameFromFullFileName(GlobVar.name_of_BRep)
         Return True
      End If

   End Function

   ' *****************************
   ' * fileDialogCircuitInputXML *
   ' *****************************

   ' File-Dialog zum XML-Circuit-File (Input)

   Public Function fileDialogCircuitInputXML() As Boolean

      circuitName0 = "hexapod.xml"

      ' File-Dialog kreieren
      Dim fD As New Windows.Forms.OpenFileDialog      

      ' Titel
      fD.Title = "Circuit File (XML)"

      ' Filter
      Dim extension As String
      extension = "*.xml"
      fD.Filter = "XML-files|" + extension

      ' Initiales Directory und initialen File-Namen festlegen
      fD.InitialDirectory = circuitPath0
      fD.FileName         = circuitName0

      ' Dialog anzeigen und bedienen
      Dim result As DialogResult = fD.ShowDialog()

      ' Auswertung
      If result = Windows.Forms.DialogResult.Cancel Then
         MidMsgBoxProblem("readCircuitFromFile: file dialog canceled by user")
         Return False
      Else
         GlobVar.full_FileName_Circuit = fD.FileName
         GlobVar.full_FileName_Circuit = Replace(GlobVar.full_FileName_Circuit, "\", "/", 1)
         ' In der globalen Variablen fullCircuitName steht das Resultat des File-Dialoges
         Return True
      End If
 
   End Function

   ' *************************************
   ' * file_dialog_RoutingTask_Input_xml *
   ' *************************************

   ' File-Dialog zum XML-RoutingTask-File (Input)

   Public Function file_dialog_RoutingTask_Input_xml() As Boolean

      routingTaskName0 = "RoutingTask0.xml"

      ' File-Dialog kreieren
      Dim fD As New Windows.Forms.OpenFileDialog      

      ' Titel
      fD.Title = "RoutingTask File (XML)"

      ' Filter
      Dim extension As String
      extension = "*.xml"
      fD.Filter = "XML-files|" + extension

      ' Initiales Directory und initialen File-Namen festlegen
      fD.InitialDirectory = routingTaskPath0
      fD.FileName         = routingTaskName0

      ' Dialog anzeigen und bedienen
      Dim result As DialogResult = fD.ShowDialog()

      ' Auswertung
      If result = Windows.Forms.DialogResult.Cancel Then
         MidMsgBoxProblem("readRoutingTaskFromFile: file dialog canceled by user")
         Return False
      Else
         GlobVar.full_FileName_RoutingTask = fD.FileName
         GlobVar.full_FileName_RoutingTask = Replace(GlobVar.full_FileName_RoutingTask, "\", "/", 1)
         ' In der globalen Variablen fullRoutingTaskName steht das Resultat des File-Dialoges
         Return True
      End If

      Return False
   End Function

End Module
