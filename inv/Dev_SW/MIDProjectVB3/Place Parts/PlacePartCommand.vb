

Imports System.Windows.Forms
Imports Inventor
Imports System.Collections.Generic
Imports System

'########################################
' PlacePartCmd (Inherits Command)
'########################################

Public Class PlacePartCommand
    Inherits Command

    ' Selection Mode Enum
    Private Enum SelectionModeEnum
        kDefault
        kEdgeAlign
    End Enum

    Private oServer As MidAddInServer
    Private oPlacePartCmdDlg As PlacePartCmdDlg

    ' Command vars
    Dim oPart As CircuitPart
    Dim oFace As Face

    Private oOldPosMatrix As Matrix
    Private selectionMode As SelectionModeEnum

    ' Minitoolbar
    Private oMiniToolbar As MiniToolbar
    Private oMtbCancelButton As MiniToolbarButton
    Private oMtbExitButton As MiniToolbarButton
    Private oMtbRotateRightButton As MiniToolbarButton
    Private oMtbRotateLeftButton As MiniToolbarButton

    Private oMtbLeftButton As MiniToolbarButton
    Private oMtbRightButton As MiniToolbarButton
    Private oMtbUpButton As MiniToolbarButton
    Private oMtbDownButton As MiniToolbarButton

    Private oMtbAlignButton As MiniToolbarDropdown

    Private oPartHighSet As HighlightSet
    Private oFaceHighSet As HighlightSet
    Private oEdgeHighSet As HighlightSet

    Private oTriad As Triad

    ' Constructor
    '***************************************************************************************************************
    Public Sub New(ByVal MidAddin As Inventor.Application, _
                   ByVal oServer As MidAddInServer)

        MyBase.New(MidAddin)

        Me.oServer = oServer
        oPlacePartCmdDlg = Nothing

        oPart = Nothing
        oTriad = Nothing
        oPartHighSet = Nothing
        oFaceHighSet = Nothing
        oEdgeHighSet = Nothing

    End Sub

    ' Initialize button execution
    '***************************************************************************************************************
    Public Overrides Sub ButtonDefinition_OnExecute()

        ' Only execute if assemlby environment
        Dim currentEnvironment As Inventor.Environment = MidAddIn.UserInterfaceManager.ActiveEnvironment

        If LCase(currentEnvironment.InternalName) <> LCase("OPTAVER") Then
            MessageBox.Show("This command works only for assembly environment", "MIDProject", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' Check if there is a part
        If oServer.MidDataTypes.CircuitBoard.PartList.Item(1) Is Nothing Then
            MessageBox.Show("There is no circuit part, please read netlist first", "MIDProject", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

        ' Check wheather there is an other instance running
        If bCommandIsRunning Then
            StopCommand()
        End If

        'Start new command
        StartCommand()

    End Sub

    ' Stop commmand
    '************************************************************************************************************************
    Public Overrides Sub StopCommand()

        If oPlacePartCmdDlg IsNot Nothing Then

            ' Reset part information
            If oPart IsNot Nothing Then
                oPart.Transformation = oOldPosMatrix
                For Each oPin As CircuitPin In oPart.PinList
                    oPin.SetTransformation()
                Next
                oPart.DeletePreviewGraphics()
                oPart.DeleteNetClientGraphics()
                DisableEdit()
                oPart = Nothing
            End If

            'Destroy the command dialog
            oPlacePartCmdDlg.Hide()
            oPlacePartCmdDlg.Dispose()
            oPlacePartCmdDlg = Nothing

            ' Delete highlightset
            oPartHighSet.Clear()
            oPartHighSet.Delete()
            oPartHighSet = Nothing

            oFaceHighSet.Clear()
            oFaceHighSet.Delete()
            oFaceHighSet = Nothing

            oEdgeHighSet.Clear()
            oEdgeHighSet.Delete()
            oEdgeHighSet = Nothing

            ' Delete mini toolbar
            DisconnectMtbEventSink()
            oMiniToolbar.Delete()
            oMiniToolbar = Nothing

            ' Disconnect interaction sink
            MyBase.StopCommand()
        End If

    End Sub

    ' Start command
    '************************************************************************************************************************
    Public Overrides Sub StartCommand()

        ' Reset references
        oPart = Nothing
        oFace = Nothing

        ' Start Interaction / Interaction.start
        MyBase.StartCommand()

        ' Subscribe to desired interaction event(s)
        MyBase.SubscribeToEvent(Interaction.InteractionTypeEnum.kSelection)
        MyBase.SubscribeToEvent(Interaction.InteractionTypeEnum.kKeyboard)

        ' Create new form
        oPlacePartCmdDlg = New PlacePartCmdDlg(MidAddIn, Me)

        If oPlacePartCmdDlg IsNot Nothing Then
            oPlacePartCmdDlg.TopMost() = True
            oPlacePartCmdDlg.ShowInTaskbar() = True
            oPlacePartCmdDlg.StartPosition = FormStartPosition.Manual
            Dim oView As Inventor.View = MidAddIn.ActiveView()
            oPlacePartCmdDlg.Location = New System.Drawing.Point(oView.Left + 20, oView.Top + 20)
            oPlacePartCmdDlg.Show()

            oPlacePartCmdDlg.okButton.Enabled = False
            oPlacePartCmdDlg.applyButton.Enabled = False

            oPlacePartCmdDlg.upButton.Enabled = False
            oPlacePartCmdDlg.downButton.Enabled = False
            oPlacePartCmdDlg.rightButton.Enabled = False
            oPlacePartCmdDlg.leftButton.Enabled = False

            oPlacePartCmdDlg.rotateRightButton.Enabled = False
            oPlacePartCmdDlg.rotateLeftButton.Enabled = False

            oPlacePartCmdDlg.alignComboBox.Enabled = False
            oPlacePartCmdDlg.moveTextBox.Enabled = False
            oPlacePartCmdDlg.rotateTextBox.Enabled = False

        End If

        ' Create the triad
        oTriad = New Triad(MidAddIn, oInteractionEvents)

        ' Create the mini toolbar
        CreateMiniToolbar()

        ' Create  the highlight sets
        oPartHighSet = MidAddIn.ActiveDocument.CreateHighlightSet()
        oPartHighSet.Color = MidAddIn.TransientObjects.CreateColor(255, 123, 0)
        oFaceHighSet = MidAddIn.ActiveDocument.CreateHighlightSet()
        oFaceHighSet.Color = MidAddIn.TransientObjects.CreateColor(255, 161, 0)
        oEdgeHighSet = MidAddIn.ActiveDocument.CreateHighlightSet()
        oEdgeHighSet.Color = MidAddIn.TransientObjects.CreateColor(255, 123, 0)

        ' Normal selection mode
        selectionMode = SelectionModeEnum.kDefault

        ' Enable interaction
        EnableInteraction()

    End Sub

    ' Enable interaction
    '************************************************************************************************************************
    Public Overrides Sub EnableInteraction()
        ' Resubscribe all event handlers for the subscribed events
        MyBase.EnableInteraction()
        Select Case selectionMode

            Case SelectionModeEnum.kDefault
                ' Default selection mode

                oSelectEvents.MouseMoveEnabled = True
                oSelectEvents.SingleSelectEnabled = True

                ' Set occurrence filter
                oSelectEvents.ClearSelectionFilter()
                oSelectEvents.AddSelectionFilter(SelectionFilterEnum.kAssemblyOccurrenceFilter)

                oInteractionEvents.SetCursor(CursorTypeEnum.kCursorBuiltInCrosshair)

                oInteractionEvents.StatusBarText = "Select circuit part and place it on the circuit carrier"

            Case SelectionModeEnum.kEdgeAlign
                ' Alignment selection mode

                oSelectEvents.MouseMoveEnabled = True
                oSelectEvents.SingleSelectEnabled = True

                oSelectEvents.ClearSelectionFilter()
                oSelectEvents.AddSelectionFilter(SelectionFilterEnum.kPartEdgeLinearFilter)

                oInteractionEvents.SetCursor(CursorTypeEnum.kCursorBuiltInSelectArrow)

                oInteractionEvents.StatusBarText = "Select face to align to"

        End Select

    End Sub

    ' Enable interaction
    '************************************************************************************************************************
    Public Overrides Sub DisableInteraction()

        MyBase.DisableInteraction()

        'Implement this command speific functionality
        'Set the default cursor to notify that selection is not active
        oInteractionEvents.SetCursor(CursorTypeEnum.kCursorBuiltInArrow)

        'Clean up status text
        oInteractionEvents.StatusBarText = String.Empty
    End Sub

    ' Execute command
    ' ***************************************************************************************************************
    Public Overrides Sub ExecuteCommand()

        MyBase.ExecuteCommand()

        DisableEdit()

        oTriad.Delete()
        oTriad = Nothing

        ' Execute change request --> replace part occurrence/set new net position
        Dim oPlacePartRequest As New PlacePartRequest(MidAddIn, oServer, oPart, oFace)
        MyBase.ExecuteChangeRequest(oPlacePartRequest, "MidAddIn:PlacePartRequest", MidAddIn.ActiveDocument)

        oTriad = New Triad(MidAddIn, oInteractionEvents)
        oPart = Nothing

    End Sub

    '##################################################
    '
    '##################################################

    ' Check for valid position (component on or off the boundaries)
    '************************************************************************************************************************
    Private Sub CheckValidPos(ByVal oOccurrence As ComponentOccurrence)

        Dim oAsmCompDef As AssemblyComponentDefinition = MidAddIn.ActiveDocument.ComponentDefinition

        Dim oVertices As Vertices = oOccurrence.SurfaceBodies.Item(1).Faces.Item(1).Vertices

        oFaceHighSet.AddItem(oOccurrence.SurfaceBodies.Item(1).Faces.Item(4))


        'Dim oTG As TransientGeometry = MidAddIn.TransientGeometry
        'Dim oScanPoint As Point = oTG.CreatePoint(0, 10, 0)
        '' oFaceNormalVector.Normalize()
        'Dim oRayDir As UnitVector = oTG.CreateUnitVector(0, -1, 0)
        'Dim oObjEnum1 As ObjectsEnumerator
        'Dim oObjEnum2 As ObjectsEnumerator

        'For i As Integer = 1 To oVertices.Count
        '    oScanPoint = oVertices.Item(i).Point '###add tolerance
        '    oAsmCompDef.FindUsingRay(oScanPoint, oRayDir, 0.02, oObjEnum1, oObjEnum2, False)
        '    For j As Integer = 1 To oObjEnum1.Count()
        '        If TypeOf oObjEnum1 Is Face Then
        '            Debug.WriteLine(oObjEnum1.Item(j).Parent.Parent.Name)

        '        End If
        '        If TypeOf oObjEnum1 Is Edge Then
        '            Debug.WriteLine(oObjEnum1.Item(j).Parent.Paren.Parent.Name)

        '        End If
        '        If TypeOf oObjEnum1 Is Vertex Then
        '            Debug.WriteLine(oObjEnum1.Item(j).Parent.Paren.Parent.Parent.Name)

        '        End If

        '    Next
        '    'oHighSet.AddItem(oObjEnum1.Item(1))
        '    'If (oObjEnum1.Item(1) Is oFace) Then
        '    '    MsgBox("hit")
        '    'End If
        'Next



        'Dim oVertices As Vertices = oOccurrence.SurfaceBodies.Item(1).Faces.Item(1).Vertices
        'Dim cv As Point
        'Dim oPoint As Point
        'For Each oVertex As Vertex In oVertices

        '    oPoint = oVertex.Point()
        '    cv = oFace.GetClosestPointTo(oPoint)
        '    Debug.WriteLine("x = " & oPoint.X - cv.X, " y = " & oPoint.Y - cv.Y & " z = " & oPoint.Z - cv.Z)

        'Next



    End Sub



    '#############################################################
    ' EDIT MODE 
    '#############################################################

    ' Get angle value from string
    '*************************************************************************************************************************
    Public Function GetAngleValue(ByVal expression As String) As Double

        Dim UOM As UnitsOfMeasure = MidAddIn.ActiveDocument.UnitsOfMeasure

        Dim angleUnitsType As UnitsTypeEnum = UOM.AngleUnits

        Try
            Return UOM.GetValueFromExpression(expression, angleUnitsType)
        Catch ex As Exception
            Return 0.0
        End Try

    End Function

    ' Get length value from string
    '*************************************************************************************************************************
    Public Function GetLengthValue(ByVal expression As String) As Double

        'Get the units of measure object
        Dim UOM As UnitsOfMeasure = MidAddIn.ActiveDocument.UnitsOfMeasure

        'Get the current lenght units of the user
        Dim lengthUnitsType As UnitsTypeEnum = UOM.LengthUnits

        'Convert the expression to the current length units of user
        Try
            Return UOM.GetValueFromExpression(expression, lengthUnitsType)
        Catch
            Return 0.0
        End Try

    End Function

    ' Rotate component
    '*************************************************************************************************************************
    Private Sub RotatePart(angle As Double)

        'Dim oPoint As Point = oTG.CreatePoint(oNewPosMatrix.Cell(1, 4), oNewPosMatrix.Cell(2, 4), oNewPosMatrix.Cell(3, 4))
        Dim s As String
        ' do a rotation around z-axis with given angle
        Dim oNewTransformation As Matrix = MidAddIn.TransientGeometry.CreateMatrix()
        oNewTransformation.SetToRotation(angle, MidAddIn.TransientGeometry.CreateVector(0, 0, 1), MidAddIn.TransientGeometry.CreatePoint())

        ' transform this rotation to part coordinate system
        oNewTransformation.TransformBy(oPart.Transformation)

        ' Set part transformation
        oPart.Transformation = oNewTransformation
        For Each oPin As CircuitPin In oPart.PinList
            oPin.SetTransformation()
            s = oPin.ToString()

        Next

        ' Set triad transformation
        oTriad.Transformation = oPart.Transformation

        ' Update part client graphics
        oPart.UpdatePreviewGraphics()
        oPart.UpdateNetClientGraphics()

    End Sub

    ' Translate component
    '*************************************************************************************************************************
    Private Sub MovePart(transVector As Vector)

        ' Calculate translation in part coordinate system
        transVector.TransformBy(oPart.Transformation)
        Dim oNewTransformation As Matrix = oPart.Transformation

        oNewTransformation.Cell(1, 4) += transVector.X
        oNewTransformation.Cell(2, 4) += transVector.Y
        oNewTransformation.Cell(3, 4) += transVector.Z

        ' Apply translation to part and pins
        oPart.Transformation = oNewTransformation
        For Each oPin As CircuitPin In oPart.PinList
            oPin.SetTransformation()
        Next

        ' Apply translation to triad
        oTriad.Transformation = oPart.Transformation

        ' Update preview graphics
        oPart.UpdatePreviewGraphics()
        oPart.UpdateNetClientGraphics()

    End Sub

    ' Rotate left
    Public Sub RotateLeft()
        RotatePart(-GetAngleValue(oPlacePartCmdDlg.rotateTextBox.Text)) ' Convert.ToDouble(oPlacePartCmdDlg.rotateTextBox, System.Globalization.CultureInfo.InvariantCulture) * (-Math.PI / 360))
    End Sub

    ' Rotate right
    Public Sub RotateRight()
        RotatePart(GetAngleValue(oPlacePartCmdDlg.rotateTextBox.Text))
    End Sub

    ' Move up
    Public Sub MoveUp()
        Dim oTransVector As Vector = MidAddIn.TransientGeometry.CreateVector(GetLengthValue(oPlacePartCmdDlg.moveTextBox.Text), 0, 0)
        MovePart(oTransVector)
    End Sub

    ' Move down
    Public Sub MoveDown()
        Dim oTransVector As Vector = MidAddIn.TransientGeometry.CreateVector(-GetLengthValue(oPlacePartCmdDlg.moveTextBox.Text), 0, 0)
        MovePart(oTransVector)
    End Sub

    ' Move right
    Public Sub MoveRight()
        Dim oTransVector As Vector = MidAddIn.TransientGeometry.CreateVector(0, GetLengthValue(oPlacePartCmdDlg.moveTextBox.Text), 0)
        MovePart(oTransVector)
    End Sub

    ' Move left
    Public Sub MoveLeft()
        Dim oTransVector As Vector = MidAddIn.TransientGeometry.CreateVector(0, -GetLengthValue(oPlacePartCmdDlg.moveTextBox.Text), 0)
        MovePart(oTransVector)
    End Sub

    ' Enable edit mode
    '*************************************************************************************************************************
    Private Sub EnableEdit(ByVal viewPosition As Point2d)

        ' Enable edit in form window
        oPlacePartCmdDlg.okButton.Enabled = True
        oPlacePartCmdDlg.applyButton.Enabled = True

        oPlacePartCmdDlg.upButton.Enabled = True
        oPlacePartCmdDlg.downButton.Enabled = True
        oPlacePartCmdDlg.rightButton.Enabled = True
        oPlacePartCmdDlg.leftButton.Enabled = True

        oPlacePartCmdDlg.rotateRightButton.Enabled = True
        oPlacePartCmdDlg.rotateLeftButton.Enabled = True

        oPlacePartCmdDlg.alignComboBox.Enabled = True
        oPlacePartCmdDlg.moveTextBox.Enabled = True
        oPlacePartCmdDlg.rotateTextBox.Enabled = True

        ' Update positions and PartList (part intern, part graphics, triad graphics)
        oPart.Transformation = CoordinateTransformation(oFace, oPart)
        For Each oPin As CircuitPin In oPart.PinList
            oPin.SetTransformation()
        Next

        ' Update the preview graphics
        oPart.UpdatePreviewGraphics()
        oPart.UpdateNetClientGraphics()

        ' Update the triad
        ' Init traid
        oTriad.Transformation = oPart.Transformation
        oTriad.Visible = True

        ' Make the toolbar visible and set it to new position
        oMiniToolbar.Visible = True
        'Dim oTranslationVec As Inventor.Vector2d = MidAddIn.TransientGeometry.CreateVector2d(0.0, -140.0)
        'viewPosition.TranslateBy(oTranslationVec)
        'oMiniToolbar.Position = viewPosition

        ' Update the mini toolbar position
        Dim oTransVec2d As Inventor.Vector2d = MidAddIn.TransientGeometry.CreateVector2d(0, -140)
        viewPosition.TranslateBy(oTransVec2d)
        oMiniToolbar.Position = viewPosition

    End Sub

    ' Disable edit mode
    '*************************************************************************************************************************
    Private Sub DisableEdit()

        ' Enable edit in form window
        ' oPlacePartCmdDlg.okButton.Enabled = False
        oPlacePartCmdDlg.applyButton.Enabled = False

        oPlacePartCmdDlg.upButton.Enabled = False
        oPlacePartCmdDlg.downButton.Enabled = False
        oPlacePartCmdDlg.rightButton.Enabled = False
        oPlacePartCmdDlg.leftButton.Enabled = False

        oPlacePartCmdDlg.rotateRightButton.Enabled = False
        oPlacePartCmdDlg.rotateLeftButton.Enabled = False

        oPlacePartCmdDlg.alignComboBox.Enabled = False
        oPlacePartCmdDlg.moveTextBox.Enabled = False
        oPlacePartCmdDlg.rotateTextBox.Enabled = False

        oTriad.Visible = False
        oMiniToolbar.Visible = False

    End Sub


    ' Align part to edge
    '*************************************************************************************************************************
    Private Sub AlignToEdge(edge As Edge)

        Dim oTG As TransientGeometry = MidAddIn.TransientGeometry
        Dim oVector As Vector = oTG.CreateVector(oPart.Transformation.Cell(1, 2), oPart.Transformation.Cell(2, 2), oPart.Transformation.Cell(3, 2))
        oVector.Normalize()
        ' Get coordinate axis from edge
        Dim oStartVertex As Vertex = edge.StartVertex
        Dim oEndVertex As Vertex = edge.StopVertex
        Dim startPoint(2) As Double
        Dim stopPoint(2) As Double
        oStartVertex.GetPoint(startPoint)
        oEndVertex.GetPoint(stopPoint)
        Dim oEdgeVector As Vector = oTG.CreateVector(startPoint(0) - stopPoint(0), startPoint(1) - stopPoint(1), startPoint(2) - stopPoint(2))
        oEdgeVector.Normalize()

        ' Calculate angle and rotate part
        Dim angle = Math.Acos(oEdgeVector.DotProduct(oVector))

        If angle > Math.PI / 2 Then
            angle = Math.PI - angle
        End If
        RotatePart(angle)

    End Sub

    '#############################################################
    ' SELECTEVENT CALLBACKS
    '#############################################################

    ' SelectEvents: OnSelect
    '*************************************************************************************************************************
    Public Overrides Sub OnSelect(justSelectedEntities As ObjectsEnumerator, _
                                  selectionDevice As SelectionDeviceEnum, _
                                  modelPosition As Inventor.Point, _
                                  viewPosition As Point2d, _
                                  view As Inventor.View)

        Try
            Select Case selectionMode

                Case SelectionModeEnum.kDefault

                    ' If part (occurrence) is selected save it
                    If TypeOf justSelectedEntities.Item(1) Is ComponentOccurrence Then

                        ' Reset the position of the previous selection and delete the preview graphics
                        If oPart IsNot Nothing Then
                            oPart.Transformation = oOldPosMatrix
                            For Each oPin As CircuitPin In oPart.PinList
                                oPin.SetTransformation()
                            Next
                            oPart.DeletePreviewGraphics()
                            oPart.DeleteNetClientGraphics()
                            DisableEdit()
                        End If

                        ' Retrieve parent part of the selected occurrence
                        oPart = oServer.MidDataTypes.CircuitBoard.Parent(justSelectedEntities.Item(1))

                        ' Create part preview graphics 
                        oPart.CreatePreviewGraphics(oInteractionEvents)

                        ' Create net preview graphics (net that belongs to part only)
                        oPart.CreateNetClientGraphics(oInteractionEvents)

                        ' Add the part to the part highlight set
                        oFaceHighSet.Clear()
                        oPartHighSet.Clear()
                        oPartHighSet.AddItem(oPart.GraphicsNode)

                        ' Save position to restore it later
                        oOldPosMatrix = oPart.Transformation

                    End If

                    ' If face AND part are selected, place the component on top of the face
                    If TypeOf justSelectedEntities.Item(1) Is Face Then

                        If oPart IsNot Nothing Then


                            ' Save face 
                            oFace = justSelectedEntities.Item(1)

                            ' Add the face to the face highlight set
                            oFaceHighSet.Clear()
                            oFaceHighSet.AddItem(oFace)

                            ' Clear Selection
                            oSelectEvents.ResetSelections()

                            ' Set part to face position
                            Dim FaceType As SurfaceTypeEnum = oFace.SurfaceType

                            Select Case FaceType

                                ' flat plane surface
                                Case SurfaceTypeEnum.kPlaneSurface

                                    ' Enable position edit
                                    EnableEdit(viewPosition)

                                Case SurfaceTypeEnum.kCylinderSurface

                                    MessageBox.Show("not supported yet (cylindrical surface)")

                                Case Else
                                    MessageBox.Show("not supported yet")

                            End Select

                        End If
                    End If

                Case SelectionModeEnum.kEdgeAlign

                    ' Align to selected edge
                    If TypeOf justSelectedEntities.Item(1) Is Edge Then
                        AlignToEdge(justSelectedEntities.Item(1))
                    End If
                    'oSelectEvents.ResetSelections()

                    ' unpress button
                    oMtbAlignButton.Pressed = False

                    ' Delete the edges in the highlight set
                    oEdgeHighSet.Clear()

                    ' Get back to default mode
                    DisableInteraction()
                    selectionMode = SelectionModeEnum.kDefault
                    EnableInteraction()

            End Select


        Catch ex As Exception
            oPart = Nothing
            oFace = Nothing
        Finally
            oSelectEvents.ResetSelections()
        End Try


    End Sub

    ' SelectEvents: OnPreSelect (filter)
    '*************************************************************************************************************************
    Public Overrides Sub OnPreSelect(ByRef preSelectEntity As Object, _
                                     ByRef doHighlight As Boolean, _
                                     ByRef morePreSelectEntities As ObjectCollection, _
                                     selectionDevice As SelectionDeviceEnum, _
                                     modelPosition As Point, _
                                     viewPosition As Point2d, _
                                     view As Inventor.View)

        Select Case selectionMode

            Case SelectionModeEnum.kDefault
                ' filter: over mid (--> faces), over PartList (--> occurrences), default: occurrence
                If TypeOf preSelectEntity Is ComponentOccurrence Then

                    'If preSelectEntity.AttributeSets.NameIsUsed("circuitcarrier") Then ++++ improve speed of search!!!!!
                    If oServer.MidDataTypes.CircuitCarrier.IsCircuitCarrier(preSelectEntity.Surfacebodies.Item(1).Faces.Item(1)) And oPart IsNot Nothing Then

                        ' Change selection filter
                        oSelectEvents.ClearSelectionFilter()
                        oSelectEvents.AddSelectionFilter(SelectionFilterEnum.kPartFacePlanarFilter)

                        Exit Sub
                    End If

                ElseIf TypeOf preSelectEntity Is Face Then

                    ' Retrieve component occurrence this face belongs to
                    Dim oObj As Object = preSelectEntity.Parent.Parent

                    ' KeepOuts are not selectable
                    If TypeOf oObj Is ComponentOccurrence Then

                        ' If keepOut --> do not highlight
                        If oServer.MidDataTypes.CircuitCarrier.IsCircuitCarrier(preSelectEntity) Then

                            Dim oKeepOuts As KeepOuts = oServer.MidDataTypes.CircuitCarrier.KeepOuts
                            If oKeepOuts.IdExists(preSelectEntity.InternalName) Then
                                doHighlight = False
                                Exit Sub
                            End If
                        End If

                        ' If circuitpart --> change filter to occurrence filter
                        If oServer.MidDataTypes.CircuitBoard.IsCircuitPart(preSelectEntity) Then

                            ' Change selection filter
                            oSelectEvents.ClearSelectionFilter()
                            oSelectEvents.AddSelectionFilter(SelectionFilterEnum.kAssemblyOccurrenceFilter)
                            Exit Sub
                        End If

                    End If

                Else
                    ' non-mid-components are not selectable
                    doHighlight = False

                End If

            Case SelectionModeEnum.kEdgeAlign

                If TypeOf preSelectEntity Is Edge Then

                    Dim oEdge As Edge = preSelectEntity

                    If Not Parent(oEdge) Then
                        doHighlight = False
                    End If

                End If
        End Select


    End Sub

    Private Function Parent(oEdge As Edge) As Boolean

        For Each edge As Edge In oFace.Edges
            If edge Is oEdge Then
                Return True
            End If
        Next
        Return False
    End Function

    ' Coordinate transformation
    '*************************************************************************************************************************
    Private Function CoordinateTransformation(oFace As Face, _
                                              oPart As CircuitPart) As Matrix

        Dim oFaceMinPoint As Point = oFace.Evaluator.RangeBox.MinPoint
        Dim oFaceMaxPoint As Point = oFace.Evaluator.RangeBox.MaxPoint

        Dim FaceMinPoint(2) As Double
        Dim FaceMaxPoint(2) As Double
        FaceMinPoint(0) = oFaceMinPoint.X
        FaceMinPoint(1) = oFaceMinPoint.Y
        FaceMinPoint(2) = oFaceMinPoint.Z

        FaceMaxPoint(0) = oFaceMaxPoint.X
        FaceMaxPoint(1) = oFaceMaxPoint.Y
        FaceMaxPoint(2) = oFaceMaxPoint.Z

        ' Normal of the mid
        Dim FaceNormal(2) As Double
        Dim FaceParams(1) As Double
        FaceParams(0) = 0.5
        FaceParams(1) = 0.5
        oFace.Evaluator.GetNormalAtPoint(FaceMinPoint, FaceNormal)

        ' Normal of the part face
        Dim boardFaceNormal(2) As Double
        boardFaceNormal(0) = 0
        boardFaceNormal(1) = 0
        boardFaceNormal(2) = 1
        'oPart.Occurrence.SurfaceBodies.Item(1).Faces.Item(1).Evaluator.GetNormal(FaceParams, OccFaceNormal)
        'oServer.MidDataTypes.CircuitBoard.Occurrence.SurfaceBodies.Item(1).Faces.Item(1).Evaluator.GetNormal(FaceParams, boardFaceNormal)

        'Debug.WriteLine("x = "& boardFaceNormal(0) & "y = " & boardFaceNormal(1) & "z = " & boardFaceNormal(2))
        ' Align both normal vectors
        Dim oTG As TransientGeometry = MidAddIn.TransientGeometry
        Dim oPartNormalVector As Vector = oTG.CreateVector(boardFaceNormal(0), boardFaceNormal(1), boardFaceNormal(2))
        Dim oFaceNormalVector As Vector = oTG.CreateVector(FaceNormal(0), FaceNormal(1), FaceNormal(2))
        Dim oNewPosMatrix As Matrix = oTG.CreateMatrix()
        oNewPosMatrix.SetToRotateTo(oPartNormalVector, oFaceNormalVector, Nothing)

        ' Position in the middle of the face
        Dim FacePoint(2) As Double
        FacePoint(0) = FaceMinPoint(0) + (FaceMaxPoint(0) - FaceMinPoint(0)) / 2
        FacePoint(1) = FaceMinPoint(1) + (FaceMaxPoint(1) - FaceMinPoint(1)) / 2
        FacePoint(2) = FaceMinPoint(2) + (FaceMaxPoint(2) - FaceMinPoint(2)) / 2
        'oFace.Evaluator.GetPointAtParam(FaceParams, FacePoint)
        Dim oTransVector As Vector = oTG.CreateVector(FacePoint(0), FacePoint(1), FacePoint(2))
        oNewPosMatrix.SetTranslation(oTransVector)

        Return oNewPosMatrix

    End Function

    ' Align
    '*************************************************************************************************************************
    Public Sub Align()

        ' Add edges to highlightSet
        For Each oEdge As Edge In oFace.Edges
            If oEdge.GeometryType = CurveTypeEnum.kLineSegmentCurve Then
                oEdgeHighSet.AddItem(oEdge)
            End If
        Next

        ' start Align command only if there a line curves
        If oEdgeHighSet.Count > 0 Then
            ' Check curve type
            oMtbAlignButton.Pressed = True

            'EnableInteraction()
            DisableInteraction()

            selectionMode = SelectionModeEnum.kEdgeAlign

            EnableInteraction()
        Else
            MessageBox.Show("This face is not supported")
        End If
    End Sub


    ' Keyboard events
    '*************************************************************************************************************************
    Public Overrides Sub OnKeyPress(keyASCII As Integer)

        If oMiniToolbar.Visible = True Then

            If keyASCII = 119 Then
                MoveUp()
            ElseIf keyASCII = 115 Then
                MoveDown()
            ElseIf keyASCII = 100 Then
                MoveRight()
            ElseIf keyASCII = 97 Then
                MoveLeft()
            End If

        End If
    End Sub

    ' Apply button (accept new part position)
    '*************************************************************************************************************************
    Public Sub Apply()

        ' Execute command
        ExecuteCommand()

    End Sub

    Public Sub Cancel()

        ' Stop command without execution
        StopCommand()

    End Sub

    Public Sub Ok()

        ' Stop command and execute change request
        ExecuteCommand()
        StopCommand()


    End Sub

    ' Create mini toolbar
    '************************************************************************************************************************
    Private Sub CreateMiniToolbar()
        'Dim oActiveEnv As Environment = MIDApplication.UserInterfaceManager.ActiveEnvironment

        oMiniToolbar = MidAddIn.CommandManager.CreateMiniToolbar()

        ' Make default buttons invisible (they make no sense)
        oMiniToolbar.ShowOK = True
        oMiniToolbar.ShowApply = True
        oMiniToolbar.ShowCancel = True

        ' Set the controlDefinitions
        Dim oControls As MiniToolbarControls = oMiniToolbar.Controls
        oControls.Item("MTB_Options").Visible = False

        'Dim oDescriptionLabel As MiniToolbarControl = oControls.AddLabel("Description", "Place Parts", "Click into the view to use wsad keys to move part")
        'oControls.AddNewLine()

        ' Button #1
        'Dim mtbAddPicture As stdole.IPictureDisp = MIDProjectVB3.PictureConverter.ImageToPictureDisp(My.Resources.mtbAddSmall1)
        Dim mtbExitPicture As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.mtbExitSmall1)
        Dim mtbRmvPicture As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.mtbRmvSmall1)

        Dim mtbButtonUpPicL As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.MtbButtonUpLarge)
        Dim mtbButtonUpPicS As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.MtbButtonUpSmall)

        Dim mtbButtonDownPicL As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.MtbButtonDownLarge)
        Dim mtbButtonDownPicS As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.MtbButtonDownSmall)

        Dim mtbButtonLeftPicL As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.MtbButtonLeftLarge)
        Dim mtbButtonLeftPicS As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.MtbButtonLeftSmall)

        Dim mtbButtonRightPicL As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.MtbButtonRightLarge)
        Dim mtbButtonRightPicS As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.MtbButtonRightSmall)

        Dim mtbButtonRotateRightPic As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.MtbButtonRotateRight)

        Dim mtbButtonRotateLeftPic As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.MtbButtonRotateLeft)

        'Dim mtbButtonOkPicS As stdole.IPictureDisp = MIDProjectVB3.PictureConverter.ImageToPictureDisp(My.Resources.MtbOkButtonSmall)
        ' remark: use only 16x16px Images for StandardIcon

        ' Dim MIDButtonTwo As ButtonDefinition = oInventorAddin.CommandManager.ControlDefinitions.Item("PartCoilCmd")

        ' Define the first center position
        'oMtbCancelButton = oControls.AddButton("CancelOccInternal", "", "Discard", mtbRmvPicture, mtbRmvPicture)

        'Dim oMtbLabelX As MiniToolbarControl = oControls.AddLabel("XLabel", "X", "X Position")
        'Dim oMtbValueEditorX As MiniToolbarValueEditor = oControls.AddValueEditor("XValueEditor", "X Position", ValueUnitsTypeEnum.kLengthUnits, "1,0", 20)

        'Dim oMtbLabelY As MiniToolbarControl = oControls.AddLabel("YLabel", "Y", "Y Position")
        'Dim oMtbValueEditorY As MiniToolbarValueEditor = oControls.AddValueEditor("YValueEditor", "Y Position", ValueUnitsTypeEnum.kLengthUnits, "1,0", 20)

        'Dim oMtbLabelZ As MiniToolbarControl = oControls.AddLabel("ZLabel", "Z", "Z Position")
        'Dim oMtbValueEditorZ As MiniToolbarValueEditor = oControls.AddValueEditor("ZValueEditor", "Z Position", ValueUnitsTypeEnum.kLengthUnits, "1,0", 20)
        'Dim oMtbLabelEnd As MiniToolbarControl = oControls.AddLabel("EndLabel", "", "X Position")
        'oControls.AddNewLine()


        oMtbRotateLeftButton = oControls.AddButton("RotateLeftInternal", "", "Rotate counterclockwise", mtbButtonRotateLeftPic)
        oMtbUpButton = oControls.AddButton("ArrowUpInternal", "", "Up", mtbButtonUpPicS, mtbButtonUpPicL)
        oMtbRotateRightButton = oControls.AddButton("RotateRightInternal", "", "Rotate clockwise", mtbButtonRotateRightPic)
        oControls.AddSeparator()
        oMtbAlignButton = oControls.AddDropdown("AlignInternal", False, True, True, True)
        oMtbAlignButton.AddItem("Align To Edge", "Align To Edge")
        oMtbAlignButton.AddItem("Align To MID", "Align To MID Circuit Carrier")


        oControls.AddNewLine()

        oMtbLeftButton = oControls.AddButton("ArrowLeftInternal", "", "Left", mtbButtonLeftPicS, mtbButtonLeftPicL)
        oMtbDownButton = oControls.AddButton("ArrowDownInternal", "", "Down", mtbButtonDownPicS, mtbButtonDownPicL)
        oMtbRightButton = oControls.AddButton("ArrowRightInternal", "", "Right", mtbButtonRightPicS, mtbButtonRightPicL)
        oControls.AddNewLine()

        ' Create Minitoolbar on the upper left of the window
        oMiniToolbar.Position = MidAddIn.TransientGeometry.CreatePoint2d(0, 0)
        oMiniToolbar.Visible = False

        ConnectMtbEventSink()

    End Sub


    Private Sub ConnectMtbEventSink()

        AddHandler oMiniToolbar.OnApply, AddressOf Me.Apply
        AddHandler oMiniToolbar.OnCancel, AddressOf Me.Cancel
        AddHandler oMiniToolbar.OnOK, AddressOf Me.Ok
        AddHandler oMtbRotateLeftButton.OnClick, AddressOf Me.RotateLeft
        AddHandler oMtbRotateRightButton.OnClick, AddressOf Me.RotateRight
        AddHandler oMtbUpButton.OnClick, AddressOf Me.MoveUp
        AddHandler oMtbDownButton.OnClick, AddressOf Me.MoveDown
        AddHandler oMtbRightButton.OnClick, AddressOf Me.MoveRight
        AddHandler oMtbLeftButton.OnClick, AddressOf Me.MoveLeft
        AddHandler oMtbAlignButton.OnSelect, AddressOf Me.Align

    End Sub

    Private Sub DisconnectMtbEventSink()
        RemoveHandler oMiniToolbar.OnApply, AddressOf Me.Apply
        RemoveHandler oMiniToolbar.OnCancel, AddressOf Me.Cancel
        RemoveHandler oMiniToolbar.OnOK, AddressOf Me.Ok
        RemoveHandler oMtbRotateLeftButton.OnClick, AddressOf Me.RotateLeft
        RemoveHandler oMtbRotateRightButton.OnClick, AddressOf Me.RotateRight
        RemoveHandler oMtbUpButton.OnClick, AddressOf Me.MoveUp
        RemoveHandler oMtbDownButton.OnClick, AddressOf Me.MoveDown
        RemoveHandler oMtbRightButton.OnClick, AddressOf Me.MoveRight
        RemoveHandler oMtbLeftButton.OnClick, AddressOf Me.MoveLeft
        RemoveHandler oMtbAlignButton.OnSelect, AddressOf Me.Align

    End Sub

End Class




'oMiniToolbar.Visible = True
'---->Implement this command specific functionality<----
'Clear selection filter


'oSelectEvents.ClearSelectionFilter()
'oSelectEvents.ResetSelections()


'Specify selection filter and cursor
'If m_rackFaceCmdDlg.checkBoxFace.Checked Then
'If Not (m_rackEdge Is Nothing) Then
'oSelectEvents.AddToSelectedEntities(m_rackEdge)
'End If

'Set the selection filter to a planar face




'Set the status bar message
'oInteractionEvents.StatusBarText = "Select an occurrence"
'Else
'If Not (oOccurrence Is Nothing) Then
'    oSelectEvents.AddToSelectedEntities(oOccurrence)
'End If

'oTriadEvents.Enabled = True

' Reset position of the previous selection and update pin positions and connections
'If oPart IsNot Nothing Then
'    oPart.Transformation = oOldPosMatrix ' save old position to restore position later
'    oPart.UpdatePins()
'    For Each oConnnection As CircuitNet In oConnections
'        oConnnection.Update()
'    Next
'    'oUCS.Visible = False
'End If


' Reposition user coordinate system
'oUCS.Transformation() = oNewPosMatrix
'oUCS.Visible() = True
'oUCS.XYPlane().Visible = False
'oUCS.XZPlane().Visible = False
'oUCS.YZPlane().Visible = False
'oUCS.XAxis().Visible = False
'oUCS.YAxis().Visible = False
'oUCS.ZAxis().Visible = False
'oUCS.Origin.Visible = False
'oUCS.XZPlane().Visible = False


'Private Function Parent(ByRef oOcc As ComponentOccurrence) As CircuitPart
'    Dim oParts As List(Of CircuitPart) = oServer.CommandCollection.AddNetCommand.CircuitBoard.PartList
'    For Each oPart As CircuitPart In oParts
'        If oPart.Occurrence Is oOcc Then
'            Return oPart
'        End If
'    Next
'    MsgBox("Error")
'    Return Nothing
'End Function

'Set the selection filter to a linear edge
'oSelectEvents.AddSelectionFilter(SelectionFilterEnum.kPartEdgeFilter)

'Set a cursor to specify edge selection
'oInteractionEvents.SetCursor(CursorTypeEnum.kCursorBuiltInArrowCursor)

'Set the status bar message
'oInteractionEvents.StatusBarText = "Select a linear edge on the selected face"
'End If#

'Dim oCompOccs As ComponentOccurrences = oAsmDoc.ComponentDefinition.Occurrences
'Dim oOccurrence As ComponentOccurrence = Nothing
'For Each oOcc As ComponentOccurrence In oCompOccs
'    If oOcc.AttributeSets.NameIsUsed("part") Then
'        oOccurrence = oOcc
'        Exit For
'    End If
'Next

'ElseIf oObj.AttributeSets.NameIsUsed("mid") Then
'    Dim oAttribSets As AttributeSets = preSelectEntity.AttributeSets
'    If oAttribSets.Item("mid").Item("isKeepOut").Value = 1 Then
'        oSelectEvents.ClearSelectionFilter()
'    End If


'Private Sub CreateGraphics(oPart As CircuitPart)
'    Dim oSurfaceBody As SurfaceBody = MidAddIn.TransientBRep.Copy(oPart.Occurrence.SurfaceBodies.Item(1))
'    Dim oInteractionGraphics As InteractionGraphics = oInteractionEvents.InteractionGraphics
'    ' Create new client graphics
'    Dim oClientGraphics As ClientGraphics = oInteractionGraphics.PreviewClientGraphics
'    'Dim oClientGraphics As ClientGraphics = oAsmDoc.ComponentDefinition.ClientGraphicsCollection.Add(oServer.ClientId)
'    ' Add Node for occurrence copy
'    Dim oOccNode As GraphicsNode = oClientGraphics.AddNode(2)
'    'CircuitPart.NodeId += 1
'    ' Debug.WriteLine("node id  = " & NodeId)
'    'oOccNode.Transformation = oTransformation
'    Dim oSurfaceGraphics As SurfaceGraphics = oOccNode.AddSurfaceGraphics(oSurfaceBody)
'    oSurfaceGraphics.DepthPriority = 3
'    oOccNode.Visible = True
'    Try
'        Dim oAsmDoc As AssemblyDocument = MidAddIn.ActiveDocument()
'        Dim oStyle As RenderStyle = oAsmDoc.RenderStyles.Item("Smooth - Red")
'        oOccNode.RenderStyle = oStyle
'    Catch ex As Exception
'        System.Windows.Forms.MessageBox.Show("Could not find 'Clear'-asset in the asset library")
'    End Try

'    MidAddIn.ActiveView.Update()


'End Sub

'For i As Integer = 1 To 4
'    For j As Integer = 1 To 4
'        Debug.Write(oNewPosMatrix.Cell(i, j))
'        Debug.Write("    ")
'    Next
'    Debug.WriteLine(" ")
'Next