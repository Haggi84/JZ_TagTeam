'footprint 1.2
' ********************
' * ReadBRepFromFile *
' ********************

' Übertragung der BRep eines Schaltungsträgers aus dem Produkt-
' Modell von MID-Layout (XML) in das OO-Modell

Option Explicit
Imports System.IO
Imports System.Windows.Forms


Module ReadBRepFromFile
   
   Public Function read_brep_from_file() As Boolean

      ' File-Dialog
      ' ===========
      Dim succ As Boolean = fileDialogBRepInputFromFile()
      If Not(succ)  Then
         Return False
      End If
      ' Das Resultat des File-Dialoges steht in den globalen Variablen name_of_BRep und full_FileName_BRep

                                       
      ' Einstellen der Parameters "name_of_BRep" für eventuelle Test-Visualisierungen
      ' =============================================================================
      Dim rc As Integer = setNameOfBRep_for_TestVisualizations(GlobVar.name_of_BRep)
      If rc <> 0 Then
         MidMsgBoxProblem("ReadBRepFromFile: technical problem")
         Return False
      End If
         
      ' BRep lesen
      ' ==========
      rc = readBRepFrom_XML_ProductModel(GlobVar.full_FileName_BRep)
      If rc <> 0 Then
         MidMsgBoxProblem("ReadBRepFromFile found error in XML-File")
         Return False
      End If
      
      Return True
   End Function

End Module