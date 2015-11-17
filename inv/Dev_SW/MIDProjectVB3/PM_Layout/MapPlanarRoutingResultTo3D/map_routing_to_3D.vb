'footprint 1.2
' ******************
' * MapRoutingTo3D *
' ******************

' Mappen des planaren Routing-Resultates aus LM
' zum 3D-Routing-Resultat in PM

Option Explicit
Imports System.IO
Imports System.Windows.Forms

Module map_routing_to_3D

   Public Function map_planar_routing_result_to_3D() As Integer

      Dim rc As Integer
      rc = mapPlanarRoutingResultTo3D()
 
      If rc <> 0 Then
         MidMsgBoxProblem("map_planar_routing_result_to_3D failed")
         Return 1
      End If

      Return 0 
   End Function

End Module
