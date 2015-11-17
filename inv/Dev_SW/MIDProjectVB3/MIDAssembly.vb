'Imports Inventor

'Public Class MIDAssembly

'    Private oInventorAddin As Inventor.Application

'    Private initalYVal As Double = 4.0

'    Private sFilePath As String = "F:\Users\Paul\Bachelor's Thesis\CAD\Inventor Addin\MIDProjectVB3\MIDProjectVB3\Models\"

'    Public Sub New(ByRef oInventorAddin As Inventor.Application)

'        Me.oInventorAddin = oInventorAddin

'    End Sub


'    Public Sub createAssembly()

'        Dim oAssemblyDocument As AssemblyDocument = oInventorAddin.ActiveDocument



'         add the circuit board plane to the assembly
'        Dim oBoardMatrix As Matrix = oInventorAddin.TransientGeometry.CreateMatrix() 'sa

'        Dim oTransVec As Vector = oInventorAddin.TransientGeometry.CreateVector(0, initalYVal, 0)

'        oBoardMatrix.SetTranslation(oTransVec)

'        Dim oBoard As ComponentOccurrence = oAssemblyDocument.ComponentDefinition.Occurrences.Add(sFilePath & "plane.ipt", oBoardMatrix)

'        Dim oDisplayOptions As DisplayOptions = oInventorAddin.DisplayOptions

'        oDisplayOptions.Show3DIndicator

'        XML: b = breite, h = höhe entspricht x = b und z = h in inventor und x = b und y = h bei den Bauteilen

'         add the SMD components to the assembly
'        Dim oPartMatrix As Matrix = oInventorAddin.TransientGeometry.CreateMatrix() 'identity matrix 



'        Dim oPartTransVec As Vector = oInventorAddin.TransientGeometry.CreateVector(-MidXmlImport.convertedb / 2.0, 0, -MidXmlImport.convertedh / 2.0)


'        oPartMatrix.SetTranslation(oPartTransVec)



'        oPartTransVec = oInventorAddin.TransientGeometry.CreateVector(0, 4, 0)
'        oPartMatrix.SetTranslation(oPartTransVec)


'        Dim oPart As ComponentOccurrence = oAssemblyDocument.ComponentDefinition.Occurrences.Add(sFilePath & "part.ipt", oPartMatrix)

'        Dim oPartTransVec2 As Vector = oInventorAddin.TransientGeometry.CreateVector((MidXmlImport.GetPartList.Item(0).GetPosition())(0), _
'                                                                                     (MidXmlImport.GetPartList.Item(0).GetPosition())(2), _
'                                                                                     (MidXmlImport.GetPartList.Item(0).GetPosition())(1))

'        oPartTransVec2.AddVector(oPartTransVec)

'        oPartMatrix.SetTranslation(oPartTransVec2)
'        oPart.SetTransformWithoutConstraints(oPartMatrix)
'        ##########################################################
'        start selection
'        ###########################################################
'        Dim oTriadEvents As New InteractionTriad(oInventorAddin)

'        oBoard = oTriadEvents.StartTriad(oBoard, oPartList)


'    End Sub

'End Class
