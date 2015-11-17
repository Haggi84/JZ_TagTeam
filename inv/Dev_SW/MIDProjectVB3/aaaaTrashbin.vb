'***********'Public Sub SetAppearance(ByRef oDocument As AssemblyDocument, ByRef oObject As Face)

'    ' = oAssemblyDocument.Assets.Item("Smooth - Light Orange")

'    Dim assetLib As AssetLibrary = MidAddIn.AssetLibraries.Item("Autodesk Appearance Library")

'    Dim libAsset1 As Asset = assetLib.AppearanceAssets.Item("Smooth - Light Orange")

'    Dim localAsset1 As Asset = libAsset1.CopyTo(oDocument)

'    oObject.Appearance() = localAsset1

'End Sub

'Public Sub CreatePlane()
'    Dim oTransBRep As TransientBRep = MidAddIn.TransientBRep

'    Dim oSurfaceBodyDef As SurfaceBodyDefinition = oTransBRep.CreateSurfaceBodyDefinition

'    ' Create a lump.
'    Dim oLumpDef As LumpDefinition = oSurfaceBodyDef.LumpDefinitions.Add

'    ' Create a shell.
'    Dim oShell As FaceShellDefinition = oLumpDef.FaceShellDefinitions.Add


'    'MsgBox("b = " & boardSizeb & ", h = " & boardSizeh)
'    '###############################################################################################
'    Dim boardPoint(4) As Point
'    boardPoint(0) = oTG.CreatePoint(-oCircuitBoard.GetLength() / 2.0, 0, -oCircuitBoard.GetWidth() / 2.0)
'    boardPoint(1) = oTG.CreatePoint(oCircuitBoard.GetLength() / 2.0, 0, -oCircuitBoard.GetWidth() / 2.0)
'    boardPoint(2) = oTG.CreatePoint(oCircuitBoard.GetLength() / 2.0, 0, oCircuitBoard.GetWidth() / 2.0)
'    boardPoint(3) = oTG.CreatePoint(-oCircuitBoard.GetLength() / 2.0, 0, oCircuitBoard.GetWidth() / 2.0)

'    Dim boardVertex(4) As VertexDefinition
'    For i As Integer = 0 To boardPoint.Length - 2
'        'MsgBox(boardPoint.Length)
'        boardVertex(i) = oSurfaceBodyDef.VertexDefinitions.Add(boardPoint(i))
'    Next

'    Dim boardLineSeg(4) As LineSegment
'    boardLineSeg(0) = oTG.CreateLineSegment(boardPoint(0), boardPoint(1))
'    boardLineSeg(1) = oTG.CreateLineSegment(boardPoint(1), boardPoint(2))
'    boardLineSeg(2) = oTG.CreateLineSegment(boardPoint(2), boardPoint(3))
'    boardLineSeg(3) = oTG.CreateLineSegment(boardPoint(3), boardPoint(0))

'    Dim boardEdgeDef(4) As EdgeDefinition

'    boardEdgeDef(0) = oSurfaceBodyDef.EdgeDefinitions.Add(boardVertex(0), boardVertex(1), boardLineSeg(0))
'    boardEdgeDef(1) = oSurfaceBodyDef.EdgeDefinitions.Add(boardVertex(1), boardVertex(2), boardLineSeg(1))
'    boardEdgeDef(2) = oSurfaceBodyDef.EdgeDefinitions.Add(boardVertex(2), boardVertex(3), boardLineSeg(2))
'    boardEdgeDef(3) = oSurfaceBodyDef.EdgeDefinitions.Add(boardVertex(3), boardVertex(0), boardLineSeg(3))

'    Dim boardPlane As Plane = oTG.CreatePlane(boardPoint(0), oTG.CreateVector(0, 1, 0)) 'why (010)?=??
'    Dim boardFaceDef As FaceDefinition = oShell.FaceDefinitions.Add(boardPlane, False)

'    Dim oBoardEdgeLoop As EdgeLoopDefinition = boardFaceDef.EdgeLoopDefinitions.Add

'    oBoardEdgeLoop.EdgeUseDefinitions.Add(boardEdgeDef(0), False)
'    oBoardEdgeLoop.EdgeUseDefinitions.Add(boardEdgeDef(1), False)
'    oBoardEdgeLoop.EdgeUseDefinitions.Add(boardEdgeDef(2), False)
'    oBoardEdgeLoop.EdgeUseDefinitions.Add(boardEdgeDef(3), False)

'    'oProgressBar.UpdateProgress()

'    Dim oBoardError As NameValueMap
'    Dim oNewBoardBody As SurfaceBody = oSurfaceBodyDef.CreateTransientSurfaceBody(oBoardError)

'    'Dim oPartDocument As PartDocument = MidAddIn.ComponentDefinition.Add(DocumentTypeEnum.kPartDocumentObject)

'    ' create new part document to create the plane
'    Dim oPartDocument As PartDocument = MidAddIn.Documents.Add(DocumentTypeEnum.kPartDocumentObject)

'    'oProgressBar.UpdateProgress()

'    Dim oCompDef As PartComponentDefinition = oPartDocument.ComponentDefinition

'    'Dim oClientGraphics As ClientGraphics = oCompDef.ClientGraphicsCollection.Add("Sample3DGraphicsID")

'    'Dim oSurfacesNode As GraphicsNode = oClientGraphics.AddNode(1)

'    'Dim oSurfaceGraphics As SurfaceGraphics = oSurfacesNode.AddSurfaceGraphics(oNewBoardBody)

'    Dim oBoardFeatureDef As NonParametricBaseFeatureDefinition = oCompDef.Features.NonParametricBaseFeatures.CreateDefinition

'    Dim oCollection As ObjectCollection = MidAddIn.TransientObjects.CreateObjectCollection

'    oCollection.Add(oNewBoardBody)

'    oBoardFeatureDef.BRepEntities = oCollection
'    oBoardFeatureDef.OutputType = BaseFeatureOutputTypeEnum.kSurfaceOutputType


'    Dim oBoardBaseFeature As NonParametricBaseFeature = oCompDef.Features.NonParametricBaseFeatures.AddByDefinition(oBoardFeatureDef)

'    '################################
'    ' change this path if needed
'    Dim sFilePath As String = "F:\Users\Paul\Bachelor's Thesis\CAD\Inventor Addin\MIDProjectVB3\MIDProjectVB3\Models\"
'    '#################################

'    MidAddIn.SilentOperation = True
'    oPartDocument.SaveAs(sFilePath & "plane.ipt", False)
'    MidAddIn.SilentOperation = False
'    'MidAddIn.ActiveView.Update()
'    oPartDocument.Close(True)
'End Sub

''Public Function GetBoard() As circuitBoard
''    Return oCircuitBoard
''End Function



'###########################################################
' Start selection
'###########################################################

'Dim oPlacement As New Placement(MidAddIn)
'oPlacement.SelectEntity()




'MsgBox("here wegoo")

'Dim oPlacement As New Placement(MidAddIn, oParts)
'Dim oOccDef As ComponentOccurrence
'Dim oFace As Face
'oFace = oPlacement.placeComponent(SelectionFilterEnum.kPartFaceFilter)




'' Dim oStyle As RenderStyle = oAssemblyDocument.RenderStyles.Item("Cadet Blue")
'Dim oPartTemp As Part

'Dim assetLib As AssetLibrary = MidAddIn.AssetLibraries.Item("Autodesk Appearance Library")

'Dim libAsset As Asset = assetLib.AppearanceAssets.Item("Smooth - Red")

'Dim localAsset As Asset = libAsset.CopyTo(oAssemblyDocument)

'Me.SetAppearance(oAssemblyDocument, oSelectedOcc)

'oSelectedOcc.Appearance() = localAsset

'For i As Integer = 0 To oParts.Count() - 2

'    If oParts.Item(i).GetOccurrence() Is oSelectedOcc Then
'        oPartTemp = oParts.Item(i)
'        'oPartTemp.GetOccurrence().SetTransformWithoutConstraints(MidAddIn.TransientGeometry.CreateMatrix())
'        'oParts.Item(i).SetRenderStyle(StyleSourceTypeEnum.kOverrideRenderStyle, oStyle)
'        'Me.SetAppearance(oAssemblyDocument, oPartTemp.GetOccurrence())

'    End If
'Next

'MsgBox("here")


' Select a face
'Dim oInteraction As New Placement(MidAddIn)
'Dim oFaceProxy As Face = oInteraction.SelectEntity(SelectionFilterEnum.kPartFaceFilter)




'Dim oStyle As RenderStyle = oAssemblyDocument.RenderStyles.Item("Default")

'oFaceProxy.Appearance() = localAsset
'oFaceProxy.SetRenderStyle(StyleSourceTypeEnum.kOverrideRenderStyle, oStyle)
' Set Face Color
'SetAppearance(oAssemblyDocument, oFace)


'oFaceProxy.Appearance() = localAsset



' create a vector out of the vertex position
'    Dim oPoint1(3) As Double
'    Dim oPoint2(3) As Double
'    Dim oPoint3(3) As Double
'    Dim oPoint4(3) As Double

'    oFaceProxy.Vertices().Item(1).GetPoint(oPoint1)
'    oFaceProxy.Vertices().Item(2).GetPoint(oPoint2)
'    oFaceProxy.Vertices().Item(3).GetPoint(oPoint3)
'    oFaceProxy.Vertices().Item(4).GetPoint(oPoint4)

'    Dim vector1 As Vector = oTG.CreateVector(oPoint1(0), oPoint1(1), oPoint1(2))
'    Dim vector2 As Vector = oTG.CreateVector(oPoint2(0), oPoint2(1), oPoint2(2))
'    Dim vector3 As Vector = oTG.CreateVector(oPoint3(0), oPoint3(1), oPoint3(2))
'    Dim vector4 As Vector = oTG.CreateVector(oPoint4(0), oPoint4(1), oPoint4(2))

'    Debug.WriteLine("x = " & oPoint1(0) & ", y = " & oPoint1(1) & ", z = " & oPoint1(2))
'    Debug.WriteLine("x = " & oPoint2(0) & ", y = " & oPoint2(1) & ", z = " & oPoint2(2))
'    Debug.WriteLine("x = " & oPoint3(0) & ", y = " & oPoint3(1) & ", z = " & oPoint3(2))
'    Debug.WriteLine("x = " & oPoint4(0) & ", y = " & oPoint4(1) & ", z = " & oPoint4(2))

'    ' Copy the position of the Occurrence in a Matrix
'    Dim oOccPositionMatrix As Matrix = oSelectedOcc.Transformation()

'    ' Translate the matrix by the selected face position
'    oOccPositionMatrix.SetTranslation(vector3)

'    ' Retrieve the angle by substracting the Points (Vertex Positions of the faces) and calculating the dot product
'    Dim oVector5 As Vector = oTG.CreateVector(oPoint1(0) - oPoint4(0), oPoint1(1) - oPoint4(1), oPoint1(2) - oPoint4(2))
'    oVector5.Normalize()

'    ' Create Vector pointing in x-direction
'    Dim oVector6 As Vector = oTG.CreateVector(1, 0, 0)

'    Dim alpha As Double = Math.Acos(oVector6.DotProduct(oVector5))

'    Dim oTranslationMatrix As Matrix = oTG.CreateMatrix()

'    oTranslationMatrix.SetToRotation(alpha, oTG.CreateVector(0, 0, 1), oTG.CreatePoint(0, 0, 0))

'    oTranslationMatrix.TransformBy(oOccPositionMatrix)

'    oSelectedOcc.SetTransformWithoutConstraints(oTranslationMatrix)

'    ' Calculate the dot porduct

'    'Dim oVec1(3) As Double
'    'oVec1(0) = oPoint1(0) - oPoint4(0)
'    'oVec1(1) = oPoint1(1) - oPoint4(1)
'    'oVec1(2) = oPoint1(2) - oPoint4(2)
'    '
'    'Debug.WriteLine("x = " & oVec1(0) & ", y = " & oVec1(1) & ", z = " & oVec1(2))

'    'Dim oVec2(3) As Double
'    'oVec2(0) = 1
'    'oVec2(1) = 0
'    'oVec2(2) = 0

'    'Dim oVec1Length As Double = Sqrt(Pow(oVec1(0), 2) + Pow(oVec1(1), 2) + Pow(oVec1(2), 2))
'    'Dim oVec2Length As Double = Sqrt(Pow(oVec2(0), 2) + Pow(oVec2(1), 2) + Pow(oVec2(2), 2))

'    'oVec1(0) = oVec1(0) / Sqrt(Pow(oVec1(0), 2) + Pow(oVec1(1), 2) + Pow(oVec1(2), 2))
'    'oVec1(1) = oVec1(1) / Sqrt(Pow(oVec1(0), 2) + Pow(oVec1(1), 2) + Pow(oVec1(2), 2))
'    'oVec1(2) = oVec1(2) / Sqrt(Pow(oVec1(0), 2) + Pow(oVec1(1), 2) + Pow(oVec1(2), 2))

'    'oVec2(0) = oVec2(0) / oVec2Length
'    'oVec2(1) = oVec2(1) / oVec2Length
'    'oVec2(2) = oVec2(2) / oVec2Length

'    ' calculate dot product
'    'Dim Alpha As Double = oVec1(0) / Sqrt(Pow(oVec1(0), 2) + Pow(oVec1(1), 2) + Pow(oVec1(2), 2)) * oVec2(0) _
'    '                      + oVec1(1) / Sqrt(Pow(oVec1(0), 2) + Pow(oVec1(1), 2) + Pow(oVec1(2), 2)) * oVec2(1) _
'    '                      + oVec1(2) / Sqrt(Pow(oVec1(0), 2) + Pow(oVec1(1), 2) + Pow(oVec1(2), 2)) * oVec2(2)

'    'Debug.WriteLine(Alpha * 180 / 3.14)
'    'Alpha = 12
'    ' identity matrix


'    'MsgBox(oVertex.Count())

'    'MsgBox("Herre we are")


'End Sub

'Public Sub SetAppearance(ByRef oDocument As AssemblyDocument, ByRef oObject As ComponentOccurrence)

'    ' Get an appearance from the document.  To assign an appearance is must
'    ' exist in the document.  This looks for a local appearance and if that
'    ' fails it copies the appearance from a library to the document.
'    Dim localAsset As Asset
'    'On Error Resume Next

'    'If Err Then
'    'On Error GoTo 0

'    ' Failed to get the appearance in the document, so import it.

'    ' Get an asset library by name.  Either the displayed name (which
'    ' can changed based on the current language) or the internal name
'    ' (which is always the same) can be used.
'    Dim assetLib As AssetLibrary = MidAddIn.AssetLibraries.Item("Autodesk Appearance Library")
'    'Set assetLib = MidAddIn.AssetLibraries.Item("314DE259-5443-4621-BFBD-1730C6CC9AE9")

'    ' Get an asset in the library.  Again, either the displayed name or the internal
'    ' name can be used.
'    Dim libAsset As Asset = assetLib.AppearanceAssets.Item("Smooth - Light Orange")
'    'Set libAsset = assetLib.AppearanceAssets.Item("ACADGen-082")

'    ' Copy the asset locally.
'    localAsset = libAsset.CopyTo(oDocument)
'    'End If
'    'On Error GoTo 0

'    ' Have an occurrence selected.
'    'Dim occ As ComponentOccurrence = MidAddIn.CommandManager.Pick(SelectionFilterEnum.kAssemblyOccurrenceFilter, "Select an occurrence.")

'    ' Assign the asset to the occurrence.
'    'If TypeOf oObject Is ComponentOccurrence Then
'    oObject.Appearance = localAsset
'    'End If

'    'If TypeOf oObject Is Face Then
'    'oObject.Appearance = localAsset
'    'End If




'#######################################################
' File dialog
'#######################################################
'Dim oFileDlg As FileDialog

'MidAddIn.CreateFileDialog(oFileDlg)

'oFileDlg.Filter = "XML-Text-Files |*.xml"

'oFileDlg.DialogTitle = "Open Netlist"

'oFileDlg.InitialDirectory = "F:\Users\Paul\Bachelor's Thesis\CAD\Inventor Addin\MIDProjectVB3\MIDProjectVB3\Models\"

'oFileDlg.CancelError = False
'oFileDlg.MultiSelectEnabled = False
'oFileDlg.OptionsEnabled = False

'oFileDlg.ShowOpen()

'MsgBox("File " & oFileDlg.FileName & " was selected")

'Dim sFilePath As String


'######################################################
' Display options
'######################################################
'Dim oDplyOpts As DisplayOptions = MidAddIn.DisplayOptions

'Dim oColor As Color = MidAddIn.TransientObjects.CreateColor(0, 0, 0)

'oDplyOpts.SolidLinesForHiddenEdges() = True

'oDplyOpts.EdgeColor() = oColor



'Dim oProgressBar As ProgressBar = MidAddIn.CreateProgressBar(True, 3, "Import Base Plate", False)




'oTriadEvents.Enabled() = True


'Dim oTriadEvents As New InteractionTriad(MidAddIn)

'oTriadEvents.Sta****************************************************************************************************