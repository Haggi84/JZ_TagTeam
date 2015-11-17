'footprint 1.2
Module ReadCircuitFromPmXML

   ' -------------------------------------------------------------------------
   '                   Public Prozedur "readCircuitFromFile"
   ' -------------------------------------------------------------------------
   ' Übertragung der Schaltung aus dem Produkt-Modell
   ' von MID-Layout (XML) in das OO-Modell
   ' -------------------------------------------------------------------------

   Public Function readCircuitFromFile() As Boolean

      ' XML-Dialog
      ' ==========
      Dim succ As Boolean = fileDialogCircuitInputXML()
      If Not(succ)  Then
         Return False
      End If

      ' Circuit lesen
      ' =============
      Dim rc As Integer = readCircuitFrom_XML_ProductModel(GlobVar.full_FileName_Circuit)
      If rc <> 0 Then
         MidMsgBoxProblem("readCircuitFromFile: function found error in XML-File.")
         Return False
      End If

      Return True

   End Function

End Module


