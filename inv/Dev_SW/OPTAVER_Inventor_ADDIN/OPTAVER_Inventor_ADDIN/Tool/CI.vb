'footprint 1.2
' ******
' * CI *
' ******

' Deklaration von Funktionen, die aus (C++)-Projekten über C-Schnittstellen rausgereicht werden

Option Explicit On

Imports System.Runtime.InteropServices


Module CI

   ' -------------------------------------------------------------------------
   '                               C-Interface für PM_Layout
   ' -------------------------------------------------------------------------

   ' Explizites Laden und Entladen von DLLs
   ' --------------------------------------
   Declare Function LoadLibraryA Lib "kernel32.dll" (ByVal name   As String) As IntPtr
   Declare Function FreeLibrary  Lib "kernel32.dll" (ByVal handle As IntPtr) As Boolean


   #If Debug Then
         
      ' Verwendung von STL-Maps
      ' -----------------------

      ' Initialisierung einer Map zur Verwaltung der Knoten
      Declare Sub initVertexMap Lib "c_interfaced.dll" ()
      
      ' Initialisierung einer Map zur Verwaltung der Kanten
      Declare Sub initEdgeMap Lib "c_interfaced.dll" ()
      
      ' Initialisierung einer Map zur Verwaltung der Facetten
      Declare Sub initFaceMap Lib "c_interfaced.dll" ()
      
      ' Hinzufügen von Paaren (Key,Number) für Knoten, Kanten bzw. Facetten zu den entsprechenden Maps
      Declare Sub appendVertex Lib "c_interfaced.dll" (ByVal key As String, ByVal num As Integer)
      Declare Sub appendEdge   Lib "c_interfaced.dll" (ByVal key As String, ByVal num As Integer)
      Declare Sub appendFace   Lib "c_interfaced.dll" (ByVal key As String, ByVal num As Integer)
      
      ' Funktionen zum Finden der Nummern zum Key von Knoten, Kanten bzw. Facetten
      Declare Function getNumberOfVertex Lib "c_interfaced.dll" (ByVal key As String) As Integer
      Declare Function getNumberOfEdge   Lib "c_interfaced.dll" (ByVal key As String) As Integer
      Declare Function getNumberOfFace   Lib "c_interfaced.dll" (ByVal key As String) As Integer
      

      ' Produktmodell PM und Layout-Modell LM
      ' -------------------------------------
      
      ' Initialisierung des objektorientierten Produktmodells (PM)   
      Declare Function init_OO_ProductModel Lib "c_interfaced.dll" () As Integer
      
      ' Löschen von PM
      Declare Sub delete_OO_ProductModel Lib "c_interfaced.dll" ()

      ' Löschen von LM
      Declare Sub delete_OO_LayoutModel  Lib "c_interfaced.dll" ()

      ' Löschen der BRep aus PM
      Declare Function delete_BRep_In_OO_ProductModel Lib "c_interfaced.dll" () As Integer
      
      ' Existiert PM?
      Declare Function productModel_PM_exists Lib "c_interfaced.dll" () As Integer
      
      ' Existiert LM?
      Declare Function layoutModel_LM_exists Lib "c_interfaced.dll" () As Integer
      
      ' Lesen der BRep aus einer Datei
      Declare Function readBRepFrom_XML_ProductModel Lib "c_interfaced.dll" (ByVal full_FileName_BRep As String) As Integer
      
      ' Lesen einer Routing-Task aus einer Datei
      Declare Function readRoutingTaskFrom_XML Lib "c_interfaced.dll" (ByVal fullRoutingTaskName As String) As Integer
      
      ' Einstellen der Parameter für eventuelle Test-Visualisierungen, die im Verlaufe des
      ' BRep-Processing erzeugt werden sollen
      Declare Function setNameOfBRep_for_TestVisualizations Lib "c_interfaced.dll" (ByVal name_of_BRep As String) As Integer

      ' Einstellen von Parametern für die Testphase (Protokoll, Geometrie-Ausgabe für Final Surface)
      Declare Function configureTestTools Lib "c_interfaced.dll" () As Integer

      ' Initiierung des Druckes von Print-Protokollen, falls dies in configureTestTools so festgelegt wurde
      Declare Function initPrintProtocol Lib "c_interfaced.dll" () As Integer
      
      ' Planarisierung der BRep im OO-Modell
      Declare Function brepProcessing Lib "c_interfaced.dll" () As Integer
      
      ' Ausgabe der planarisierten BRep in eine XML-Datei
      Declare Function writePlanarBRepToXML Lib "c_interfaced.dll" (ByVal full_XML_FileNamePlanarBRep As String) As Integer
      
      ' Planarisierung der Routing-Task
      Declare Function planarizationOfRoutingTask Lib "c_interfaced.dll" () As Integer
      
      ' Lesen der Schaltung aus dem PM (XML) in das OO-PM
      Declare Function readCircuitFrom_XML_ProductModel Lib "c_interfaced.dll" (ByVal full_XML_FileNameCircuit As String) As Integer

      ' Planares Routing auf der planarisierten BRep in LM
      Declare Function planarRouting Lib "c_interfaced.dll" () As Integer 

      ' Bestimmung des Startwinkels eines Ellipsenbogens in Rad
    Declare Function detEllArcStartAngle Lib "c_interfaced.dll" (ByVal maRad As Double, ByVal miRad As Double, ByVal sP() As Double, ByVal sAng As Double, ByVal cP() As Double, ByVal pN() As Double, ByVal mAV() As Double) As Double


    Declare Function computePointsOfEllipse Lib "c_interfaced.dll" (ByVal n As Integer, ByVal alpha0 As Double, ByVal delta As Double, ByVal XX As Double, ByVal YY As Double, ByVal ZZ As Double) As Integer


    ' Ausgabe des planarisierten Routings in eine XML-Datei
    Declare Function writePlanarRoutingResultToXML Lib "c_interfaced.dll" (ByVal full_XML_FileNameroutingResult As String) As Integer


    'Ausgabe des 3D Routings in eine XML-Datei
    Declare Function write3DRoutingResultToXML Lib "c_interfaced.dll" (ByVal full_XML_FileNameroutingResult As String) As Integer

    ' Mappen des planaren Routing-Resultates nach 3D
    Declare Function mapPlanarRoutingResultTo3D Lib "c_interfaced.dll" () As Integer 



      ' Ausgaben ins Protokoll
      Declare Sub printMessageToProtocol Lib "c_interfaced.dll" (ByVal name As String)
      Declare Sub printDoubleToProtocol  Lib "c_interfaced.dll" (ByVal name As String, ByVal val As Double)
      Declare Sub printVectorToProtocol  Lib "c_interfaced.dll" (ByVal name As String, ByVal vec() As Double)

   #Else

      ' Verwendung von STL-Maps
      ' -----------------------

      ' Initialisierung einer Map zur Verwaltung der Knoten
      Declare Sub initVertexMap Lib "c_interface.dll" ()
      
      ' Initialisierung einer Map zur Verwaltung der Kanten
      Declare Sub initEdgeMap Lib "c_interface.dll" ()
      
      ' Initialisierung einer Map zur Verwaltung der Facetten
      Declare Sub initFaceMap Lib "c_interface.dll" ()
      
      ' Hinzufügen von Paaren (Key,Number) für Knoten, Kanten bzw. Facetten zu den entsprechenden Maps
      Declare Sub appendVertex Lib "c_interface.dll" (ByVal key As String, ByVal num As Integer)
      Declare Sub appendEdge   Lib "c_interface.dll" (ByVal key As String, ByVal num As Integer)
      Declare Sub appendFace   Lib "c_interface.dll" (ByVal key As String, ByVal num As Integer)
      
      ' Funktionen zum Finden der Nummern zum Key von Knoten, Kanten bzw. Facetten
      Declare Function getNumberOfVertex Lib "c_interface.dll" (ByVal key As String) As Integer
      Declare Function getNumberOfEdge   Lib "c_interface.dll" (ByVal key As String) As Integer
      Declare Function getNumberOfFace   Lib "c_interface.dll" (ByVal key As String) As Integer
      

      ' Produktmodell PM und Layout-Modell LM
      ' -------------------------------------
      
      ' Initialisierung des objektorientierten Produktmodells (PM)   
      Declare Function init_OO_ProductModel Lib "c_interface.dll" () As Integer
      
      ' Löschen von PM
      Declare Sub delete_OO_ProductModel Lib "c_interface.dll" ()

      ' Löschen von LM
      Declare Sub delete_OO_LayoutModel  Lib "c_interface.dll" ()

      ' Löschen der BRep aus PM
      Declare Function delete_BRep_In_OO_ProductModel Lib "c_interface.dll" () As Integer
      
      ' Existiert PM?
      Declare Function productModel_PM_exists Lib "c_interface.dll" () As Integer
      
      ' Existiert LM?
      Declare Function layoutModel_LM_exists Lib "c_interface.dll" () As Integer
      
      ' Lesen der BRep aus einer Datei
      Declare Function readBRepFrom_XML_ProductModel Lib "c_interface.dll" (ByVal full_FileName_BRep As String) As Integer
      
      ' Lesen einer Routing-Task aus einer Datei
      Declare Function readRoutingTaskFrom_XML Lib "c_interface.dll" (ByVal fullRoutingTaskName As String) As Integer
      
      ' Einstellen der Parameter für eventuelle Test-Visualisierungen, die im Verlaufe des
      ' BRep-Processing erzeugt werden sollen
      Declare Function setNameOfBRep_for_TestVisualizations Lib "c_interface.dll" (ByVal name_of_BRep As String) As Integer

      ' Einstellen von Parametern für die Testphase (Protokoll, Geometrie-Ausgabe für Final Surface)
      Declare Function configureTestTools Lib "c_interface.dll" () As Integer

      ' Initiierung des Druckes von Print-Protokollen, falls dies in configureTestTools so festgelegt wurde
      Declare Function initPrintProtocol Lib "c_interface.dll" () As Integer
      
      ' Planarisierung der BRep im OO-Modell
      Declare Function brepProcessing Lib "c_interface.dll" () As Integer
      
      ' Ausgabe der planarisierten BRep in eine XML-Datei
      Declare Function writePlanarBRepToXML Lib "c_interface.dll" (ByVal full_XML_FileNamePlanarBRep As String) As Integer
      
      ' Planarisierung der Routing-Task
      Declare Function planarizationOfRoutingTask Lib "c_interface.dll" () As Integer
      
      ' Lesen der Schaltung aus dem PM (XML) in das OO-PM
      Declare Function readCircuitFrom_XML_ProductModel Lib "c_interface.dll" (ByVal full_XML_FileNameCircuit As String) As Integer

      ' Planares Routing auf der planarisierten BRep in LM
      Declare Function planarRouting Lib "c_interface.dll" () As Integer 

     ' Bestimmung des Startwinkels eines Ellipsenbogens in Rad
      Declare Function detEllArcStartAngle Lib "c_interface.dll" (ByVal maRad As Double, ByVal miRad As Double, ByVal sP() As Double, ByVal sAng As Double, ByVal cP() As Double, ByVal pN() As Double, ByVal mAV() As Double) As Double

     ' Ausgabe des planarisierten Routings in eine XML-Datei
      Declare Function writePlanarRoutingResultToXML Lib "c_interface.dll" (ByVal full_XML_FileNameroutingResult As String) As Integer

     'Ausgabe des 3D Routings in eine XML-Datei
    Declare Function write3DRoutingResultToXML Lib "c_interfaced.dll" (ByVal full_XML_FileNameroutingResult As String) As Integer

      ' Mappen des planaren Routing-Resultates nach 3D
      Declare Function mapPlanarRoutingResultTo3D Lib "c_interface.dll" () As Integer 

      ' Ausgaben ins Protokoll
      Declare Sub printMessageToProtocol Lib "c_interfaced.dll" (ByVal name As String)
      Declare Sub printDoubleToProtocol  Lib "c_interface.dll" (ByVal name As String, ByVal val As Double)
      Declare Sub printVectorToProtocol  Lib "c_interface.dll" (ByVal name As String, ByVal vec() As Double)

   #End If

End Module