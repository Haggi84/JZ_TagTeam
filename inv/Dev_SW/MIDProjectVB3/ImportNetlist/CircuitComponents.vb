Imports Inventor
Imports System.Xml
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports System.IO
Imports System.Collections

'#############################################
' CIRCUIT BOARD CLASS
'#############################################
Public MustInherit Class Component

End Class













'Dim oTransBRep As TransientBRep = oInventorAddin.TransientBRep

'Dim oSurfaceBodyDef As SurfaceBodyDefinition = oTransBRep.CreateSurfaceBodyDefinition

'' Create a lump.
'Dim oLumpDef As LumpDefinition = oSurfaceBodyDef.LumpDefinitions.Add

'' Create a shell.
'Dim oShell As FaceShellDefinition = oLumpDef.FaceShellDefinitions.Add

'Dim oTG As TransientGeometry = oInventorAddin.TransientGeometry

'Dim boardPoint(3) As Point
'boardPoint(0) = oTG.CreatePoint(-length / 2.0, 0, -width / 2.0)
'boardPoint(1) = oTG.CreatePoint(length / 2.0, 0, -width / 2.0)
'boardPoint(2) = oTG.CreatePoint(length / 2.0, 0, width / 2.0)
'boardPoint(3) = oTG.CreatePoint(-length / 2.0, 0, width / 2.0)

'Dim boardVertex(3) As VertexDefinition
'For i As Integer = 0 To boardPoint.Length - 1
'    boardVertex(i) = oSurfaceBodyDef.VertexDefinitions.Add(boardPoint(i))
'Next

'Dim boardLineSeg(3) As LineSegment
''For i As Integer = 0 To boardLineSeg.Length() - 1
''    boardLineSeg(i) = oTG.CreateLineSegment(boardPoint(i Mod (boardLineSeg.Length() - 1)), boardLineSeg((i + 1) Mod (boardLineSeg.Length() - 1)))
''Next

'boardLineSeg(0) = oTG.CreateLineSegment(boardPoint(0), boardPoint(1))
'boardLineSeg(1) = oTG.CreateLineSegment(boardPoint(1), boardPoint(2))
'boardLineSeg(2) = oTG.CreateLineSegment(boardPoint(2), boardPoint(3))
'boardLineSeg(3) = oTG.CreateLineSegment(boardPoint(3), boardPoint(0))

'Dim boardEdgeDef(3) As EdgeDefinition
'boardEdgeDef(0) = oSurfaceBodyDef.EdgeDefinitions.Add(boardVertex(0), boardVertex(1), boardLineSeg(0))
'boardEdgeDef(1) = oSurfaceBodyDef.EdgeDefinitions.Add(boardVertex(1), boardVertex(2), boardLineSeg(1))
'boardEdgeDef(2) = oSurfaceBodyDef.EdgeDefinitions.Add(boardVertex(2), boardVertex(3), boardLineSeg(2))
'boardEdgeDef(3) = oSurfaceBodyDef.EdgeDefinitions.Add(boardVertex(3), boardVertex(0), boardLineSeg(3))

'Dim boardPlane As Plane = oTG.CreatePlane(boardPoint(0), oTG.CreateVector(0, 1, 0))

'Dim boardFaceDef As FaceDefinition = oShell.FaceDefinitions.Add(boardPlane, False)

'Dim oBoardEdgeLoop As EdgeLoopDefinition = boardFaceDef.EdgeLoopDefinitions.Add
'oBoardEdgeLoop.EdgeUseDefinitions.Add(boardEdgeDef(0), False)
'oBoardEdgeLoop.EdgeUseDefinitions.Add(boardEdgeDef(1), False)
'oBoardEdgeLoop.EdgeUseDefinitions.Add(boardEdgeDef(2), False)
'oBoardEdgeLoop.EdgeUseDefinitions.Add(boardEdgeDef(3), False)

'Dim oBoardError As NameValueMap
'Dim oNewBoardBody As SurfaceBody = oSurfaceBodyDef.CreateTransientSurfaceBody(oBoardError)

'Dim oBaseFeature As NonParametricBaseFeature = oCompDef.Features.NonParametricBaseFeatures.Add(oNewBoardBody)

'Dim oBoardFeatureDef As NonParametricBaseFeatureDefinition = oCompDef.Features.NonParametricBaseFeatures.CreateDefinition

'Dim oCollection As ObjectCollection = oInventorAddin.TransientObjects.CreateObjectCollection

'oCollection.Add(oNewBoardBody)

'oBoardFeatureDef.BRepEntities = oCollection
'oBoardFeatureDef.OutputType = BaseFeatureOutputTypeEnum.kSurfaceOutputType

'Dim oBoardBaseFeature As NonParametricBaseFeature = oCompDef.Features.NonParametricBaseFeatures.AddByDefinition(oBoardFeatureDef)




'MsgBox(oBoardOcc.AttributeSets.Item(1).Item(1).Value)
' Determine reference key
'Dim key(1) As Byte
'oBoardOcc.GetReferenceKey(key)
'MsgBox("Referenz Key Length: " & key.Length & vbNewLine & "Reference Key : " & _
'       key(0) & key(1) & key(2) & key(3) & key(4) & key(5) & key(6) & key(7) & key(8) & key(9) & key(10) & key(11))

'For i As Integer = 1 To key.Length - 1
'    key(i) = 1
'Next

'oBoardOcc.GetReferenceKey(key)
'MsgBox("Referenz Key Length: " & key.Length & vbNewLine & "Reference Key : " & _
'       key(0) & key(1) & key(2) & key(3) & key(4) & key(5) & key(6) & key(7) & key(8) & key(9) & key(10) & key(11))