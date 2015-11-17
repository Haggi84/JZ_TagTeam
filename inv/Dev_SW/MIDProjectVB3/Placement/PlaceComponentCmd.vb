'########################################
' PlaceCompCmd (Inherits Command)
'########################################

Imports System.Windows.Forms
Imports Inventor
Imports System.Collections.Generic

Public Class PlaceCompCommand
    Inherits Command

    Private oForm As PlaceCompForm


    'Dim oParts As List(Of CircuitPart)
    Dim oPart As CircuitPart
    Dim oConnections As List(Of CircuitNet)
    Dim oConnection As CircuitNet

    'Private m_rackFace As Face
    'Private m_rackEdge As Edge

    'Private oAsmDoc As AssemblyDocument
    Private oTG As TransientGeometry

    Private oCompOcc As ComponentOccurrence
    Private oOldPosMatrix As Matrix
    Private oNewPosMatrix As Matrix
    Private oUCS As UserCoordinateSystem
    Private oFace As Face

    Private oMiniToolbar As MiniToolbar
    Private WithEvents oMtbCancelButton As MiniToolbarButton
    Private WithEvents oMtbApplyButton As MiniToolbarButton
    Private WithEvents oMtbExitButton As MiniToolbarButton
    Private WithEvents oMtbRotateRight As MiniToolbarButton
    Private WithEvents oMtbRotateLeft As MiniToolbarButton

    Private WithEvents oMtbButtonLeft As MiniToolbarButton
    Private WithEvents oMtbButtonRight As MiniToolbarButton
    Private WithEvents oMtbButtonUp As MiniToolbarButton
    Private WithEvents oMtbButtonDown As MiniToolbarButton
    Private filterType As SelectionFilterEnum

    Private oServer As MidAddInServer
    Private oAsmDoc As AssemblyDocument

    Private oFaceNormalVector As Vector

    Private oPartOccStyle As RenderStyle

    Private oHighSet As HighlightSet

    ' Constructor
    Public Sub New(ByVal MidAddin As Inventor.Application, _
                   ByVal oServer As MidAddInServer)

        MyBase.New(MidAddin)

        Me.oServer = oServer

        oForm = Nothing
        oCompOcc = Nothing
        oPart = Nothing
        oFace = Nothing

        oAsmDoc = Nothing
        oTG = Nothing

    End Sub

    ' Initialize button execution
    '***************************************************************************************************************
    Protected Overrides Sub oButtonDefinition_OnExecute()

        ' Init important inventor classes

        oTG = MidAddIn.TransientGeometry

        oConnections = oServer.CommandCollection.AddNetCommand.CircuitBoard.Nets


        Dim oAsmDoc As AssemblyDocument = MidAddIn.ActiveDocument()


        ' Set Style for components
        oPartOccStyle = oAsmDoc.RenderStyles.Item("Smooth - Dark Forest Green")
        oHighSet = oAsmDoc.CreateHighlightSet()
        Dim oColor As Color = MidAddIn.TransientObjects.CreateColor(0, 255, 33)
        oHighSet.Color = oColor

        'Dim currEnvironment As Inventor.Environment = MidAddIn.UserInterfaceManager.ActiveEnvironment()

        'If LCase$(currEnvironment.InternalName) <> LCase$("AMxAssemblyEnvironment") Then
        '    MessageBox.Show("This command applies only to assembly environment", _
        '                    "MIDProject", _
        '                    MessageBoxButtons.OK, _
        '                    MessageBoxIcon.Error)
        '    Exit Sub
        'End If


        ' Check wheather there is an other instance running
        If bCommandIsRunning Then
            StopCommand()
        End If

        ' Check

        Dim oCompOccs As ComponentOccurrences = oAsmDoc.ComponentDefinition.Occurrences
        Dim oCompOcc As ComponentOccurrence = Nothing
        For Each oOcc As ComponentOccurrence In oCompOccs
            If oOcc.AttributeSets.NameIsUsed("part") Then
                oCompOcc = oOcc
                Exit For
            End If
        Next

        If oCompOcc Is Nothing Then
            System.Windows.Forms.MessageBox.Show("Could not find any circuit parts, please import Netlist first")
            Exit Sub
        End If

        'Start new command

        StartCommand()



    End Sub

    Public Overrides Sub StopCommand()

        If oForm IsNot Nothing Then

            'Destroy the command dialog
            oForm.Hide()
            oForm.Dispose() ' make it ready for garbage collector
            oForm = Nothing

            ' Delete mini toolbar
            oMiniToolbar.Delete()

            ' Delete user coordinate system
            oUCS.Delete()

            ' Clear and delete highlight set
            oHighSet.Clear()
            oHighSet.Delete()

            ' Disconnect interaction sink
            MyBase.StopCommand()
        End If

    End Sub

    ' StartCommand
    Public Overrides Sub StartCommand()

        ' Start Interaction / Interaction.start
        MyBase.StartCommand()

        ' Subscribe to desired interaction event(s)
        MyBase.SubscribeToEvent(Interaction1.InteractionTypeEnum.kSelection)

        'MyBase.SubscribeToEvent(Interaction1.InteractionTypeEnum.kKeyboard)

        ' Create new form
        oForm = New PlaceCompForm(MidAddIn, Me)

        If oForm IsNot Nothing Then
            oForm.TopMost() = True
            oForm.ShowInTaskbar() = True
            oForm.Show()
            Dim oView As Inventor.View = MidAddIn.ActiveView()
            oForm.Location = New System.Drawing.Point(oView.Left + 20, oView.Top + 20)
        End If

        ' Init a new user coordinate system
        CreateUCS()

        '### sichtbarkeit abschalten von workplanes!

        ' Create mini toolbar
        CreateMiniToolbar()


        EnableInteraction()

    End Sub


    ' Create user coordinate system
    '***************************************************************************************************************
    Private Sub CreateUCS()

        Dim oCompDef As AssemblyComponentDefinition = MidAddIn.ActiveDocument.ComponentDefinition
        Dim oUCSDef As UserCoordinateSystemDefinition = oCompDef.UserCoordinateSystems.CreateDefinition()
        oUCS = oCompDef.UserCoordinateSystems.Add(oUCSDef)
        oUCS.Name = "MidUCS"
        oUCS.Visible = False

    End Sub



    ' #3: ENABLE INTERACTION
    Public Overrides Sub EnableInteraction()
        ' Resubscribe all event handlers for the subscribed events
        MyBase.EnableInteraction()

        oSelectEvents.MouseMoveEnabled = True

        oSelectEvents.SingleSelectEnabled = True
        oSelectEvents.AddSelectionFilter(SelectionFilterEnum.kAssemblyOccurrenceFilter)

        oInteractionEvents.SetCursor(CursorTypeEnum.kCursorBuiltInCrosshair)


    End Sub

    Public Overrides Sub DisableInteraction()
        'base command button's DisableInteraction
        MyBase.DisableInteraction()

        'Implement this command speific functionality
        'Set the default cursor to notify that selection is not active
        oInteractionEvents.SetCursor(CursorTypeEnum.kCursorBuiltInArrow)

        'Clean up status text
        oInteractionEvents.StatusBarText = String.Empty
    End Sub




    ' Create mini toolbar
    '************************************************************************************************************************
    Private Sub CreateMiniToolbar()
        'Dim oActiveEnv As Environment = MIDApplication.UserInterfaceManager.ActiveEnvironment

        oMiniToolbar = MidAddIn.CommandManager.CreateMiniToolbar()

        ' Make default buttons invisible (they make no sense)
        oMiniToolbar.ShowOK = False
        oMiniToolbar.ShowApply = False
        oMiniToolbar.ShowCancel = False

        ' Set the controlDefinitions
        Dim oControls As MiniToolbarControls = oMiniToolbar.Controls
        oControls.Item("MTB_Options").Visible = False

        Dim oDescriptionLabel As MiniToolbarControl = oControls.AddLabel("Description", "Use arrow keys to move component", "")
        oControls.AddNewLine()

        ' Button #1
        'Dim mtbAddPicture As stdole.IPictureDisp = MIDProjectVB3.PictureConverter.ImageToPictureDisp(My.Resources.mtbAddSmall1)
        Dim mtbExitPicture As stdole.IPictureDisp = MIDProjectVB3.PictureConverter.ImageToPictureDisp(My.Resources.mtbExitSmall1)
        Dim mtbRmvPicture As stdole.IPictureDisp = MIDProjectVB3.PictureConverter.ImageToPictureDisp(My.Resources.mtbRmvSmall1)

        Dim mtbButtonUpPicL As stdole.IPictureDisp = MIDProjectVB3.PictureConverter.ImageToPictureDisp(My.Resources.MtbButtonUpLarge)
        Dim mtbButtonUpPicS As stdole.IPictureDisp = MIDProjectVB3.PictureConverter.ImageToPictureDisp(My.Resources.MtbButtonUpSmall)

        Dim mtbButtonDownPicL As stdole.IPictureDisp = MIDProjectVB3.PictureConverter.ImageToPictureDisp(My.Resources.MtbButtonDownLarge)
        Dim mtbButtonDownPicS As stdole.IPictureDisp = MIDProjectVB3.PictureConverter.ImageToPictureDisp(My.Resources.MtbButtonDownSmall)

        Dim mtbButtonLeftPicL As stdole.IPictureDisp = MIDProjectVB3.PictureConverter.ImageToPictureDisp(My.Resources.MtbButtonLeftLarge)
        Dim mtbButtonLeftPicS As stdole.IPictureDisp = MIDProjectVB3.PictureConverter.ImageToPictureDisp(My.Resources.MtbButtonLeftSmall)

        Dim mtbButtonRightPicL As stdole.IPictureDisp = MIDProjectVB3.PictureConverter.ImageToPictureDisp(My.Resources.MtbButtonRightLarge)
        Dim mtbButtonRightPicS As stdole.IPictureDisp = MIDProjectVB3.PictureConverter.ImageToPictureDisp(My.Resources.MtbButtonRightSmall)

        Dim mtbButtonRotateRightPic As stdole.IPictureDisp = MIDProjectVB3.PictureConverter.ImageToPictureDisp(My.Resources.MtbButtonRotateRight)

        Dim mtbButtonRotateLeftPic As stdole.IPictureDisp = MIDProjectVB3.PictureConverter.ImageToPictureDisp(My.Resources.MtbButtonRotateLeft)

        'Dim mtbButtonOkPicS As stdole.IPictureDisp = MIDProjectVB3.PictureConverter.ImageToPictureDisp(My.Resources.MtbOkButtonSmall)
        ' remark: use only 16x16px Images for StandardIcon

        ' Dim MIDButtonTwo As ButtonDefinition = oInventorAddin.CommandManager.ControlDefinitions.Item("PartCoilCmd")

        ' Define the first center position
        'oMtbCancelButton = oControls.AddButton("CancelOccInternal", "", "Discard", mtbRmvPicture, mtbRmvPicture)

        oMtbRotateLeft = oControls.AddButton("RotateLeftInternal", "", "Rotate counterclockwise", mtbButtonRotateLeftPic)
        oMtbButtonUp = oControls.AddButton("ArrowUpInternal", "", "Up", mtbButtonUpPicS, mtbButtonUpPicL)
        oMtbRotateRight = oControls.AddButton("RotateRightInternal", "", "Rotate clockwise", mtbButtonRotateRightPic)
        oControls.AddNewLine()

        oMtbButtonLeft = oControls.AddButton("ArrowLeftInternal", "", "Left", mtbButtonLeftPicS, mtbButtonLeftPicL)
        oMtbButtonDown = oControls.AddButton("ArrowDownInternal", "", "Down", mtbButtonDownPicS, mtbButtonDownPicL)
        oMtbButtonRight = oControls.AddButton("ArrowRightInternal", "", "Right", mtbButtonRightPicS, mtbButtonRightPicL)
        oControls.AddNewLine()
        oMtbApplyButton = oControls.AddButton("ApplyOccInternal", "Accept position", "Apply position", mtbExitPicture, mtbExitPicture)


        'oMtbExitButton = oControls.AddButton("ExitOccInternal", "Exit", "Exit")

        ' AddHandler oMtbExitButton.OnClick, AddressOf oMtbExitButton_OnClick

        ' Create Minitoolbar on the upper left of the window
        oMiniToolbar.Position = MidAddIn.TransientGeometry.CreatePoint2d(0, 0)
        oMiniToolbar.Visible = False

    End Sub


    ' Check for valid position (component on or off the boundaries)
    '************************************************************************************************************************
    Private Sub CheckValidPos(ByVal oCompOcc As ComponentOccurrence)

        Dim oAsmCompDef As AssemblyComponentDefinition = MidAddIn.ActiveDocument.ComponentDefinition

        Dim oVertices As Vertices = oCompOcc.SurfaceBodies.Item(1).Faces.Item(1).Vertices

        Dim oScanPoint As Point = oTG.CreatePoint(0, 10, 0)
        oFaceNormalVector.Normalize()
        Dim oRayDir As UnitVector = oTG.CreateUnitVector(0, -1, 0)
        Dim oObjEnum1 As ObjectsEnumerator
        Dim oObjEnum2 As ObjectsEnumerator

        For i As Integer = 1 To oVertices.Count
            oScanPoint = oVertices.Item(i).Point '###add tolerance
            oAsmCompDef.FindUsingRay(oScanPoint, oRayDir, 0.02, oObjEnum1, oObjEnum2, False)
            For j As Integer = 1 To oObjEnum1.Count()
                If TypeOf oObjEnum1 Is Face Then
                    Debug.WriteLine(oObjEnum1.Item(j).Parent.Parent.Name)

                End If
                If TypeOf oObjEnum1 Is Edge Then
                    Debug.WriteLine(oObjEnum1.Item(j).Parent.Paren.Parent.Name)

                End If
                If TypeOf oObjEnum1 Is Vertex Then
                    Debug.WriteLine(oObjEnum1.Item(j).Parent.Paren.Parent.Parent.Name)

                End If

            Next
            'oHighSet.AddItem(oObjEnum1.Item(1))
            'If (oObjEnum1.Item(1) Is oFace) Then
            '    MsgBox("hit")
            'End If
        Next



        'Dim oVertices As Vertices = oCompOcc.SurfaceBodies.Item(1).Faces.Item(1).Vertices
        'Dim cv As Point
        'Dim oPoint As Point
        'For Each oVertex As Vertex In oVertices

        '    oPoint = oVertex.Point()
        '    cv = oFace.GetClosestPointTo(oPoint)
        '    Debug.WriteLine("x = " & oPoint.X - cv.X, " y = " & oPoint.Y - cv.Y & " z = " & oPoint.Z - cv.Z)

        'Next



    End Sub

    '#############################################################
    ' EVENTS (Minitoolbar)
    '#############################################################
    Public Sub oRotateLeft_OnClick() Handles oMtbRotateLeft.OnClick

    End Sub

    Public Sub oRotateRight_OnClick() Handles oMtbRotateRight.OnClick
        RotateComponent(Math.PI / 2)
    End Sub

    Private Sub RotateComponent(angle As Double)

        Dim oPoint As Point = oTG.CreatePoint(oNewPosMatrix.Cell(1, 4), oNewPosMatrix.Cell(2, 4), oNewPosMatrix.Cell(3, 4))
        Dim oMatrix As Matrix = oTG.CreateMatrix()
        oMatrix.SetToRotation(angle, oTG.CreateVector(0, 1, 0), oTG.CreatePoint())
        For i As Integer = 1 To 4
            For j As Integer = 1 To 4
                Debug.Write(oNewPosMatrix.Cell(i, j))
                Debug.Write("    ")
            Next
            Debug.WriteLine(" ")
        Next
        oMatrix.TransformBy(oNewPosMatrix)
        oPart.Occurrence.Transformation = oMatrix



    End Sub

    ' Translate component
    '*************************************************************************************************************************
    Private Sub MoveComponent(oTransVector As Vector)

        oTransVector.TransformBy(oNewPosMatrix)

        oNewPosMatrix.Cell(1, 4) += oTransVector.X
        oNewPosMatrix.Cell(2, 4) += oTransVector.Y
        oNewPosMatrix.Cell(3, 4) += oTransVector.Z

        oCompOcc.Transformation = oNewPosMatrix
        oUCS.Transformation = oNewPosMatrix
        oPart.UpdatePins()
        For Each oCon As CircuitNet In oConnections
            oCon.Update()
        Next

        ' Add to highlight set
        'oHighSet.AddItem(oPart.Occurrence)

    End Sub

    ' Move up
    Private Sub MoveUp() Handles oMtbButtonUp.OnClick
        Dim oTransVector As Vector = MidAddIn.TransientGeometry.CreateVector(0.05, 0, 0)
        MoveComponent(oTransVector)
        CheckValidPos(oPart.Occurrence)

    End Sub

    ' Move down
    Private Sub MoveDown() Handles oMtbButtonDown.OnClick
        Dim oTransVector As Vector = MidAddIn.TransientGeometry.CreateVector(-0.05, 0, 0)
        MoveComponent(oTransVector)
        CheckValidPos(oPart.Occurrence)

    End Sub

    ' Move right
    Private Sub MoveRight() Handles oMtbButtonRight.OnClick
        Dim oTransVector As Vector = MidAddIn.TransientGeometry.CreateVector(0, 0.05, 0)
        MoveComponent(oTransVector)
        CheckValidPos(oPart.Occurrence)

    End Sub

    ' Move left
    Private Sub MoveLeft() Handles oMtbButtonLeft.OnClick
        Dim oTransVector As Vector = MidAddIn.TransientGeometry.CreateVector(0, -0.05, 0)
        MoveComponent(oTransVector)
        CheckValidPos(oPart.Occurrence)

    End Sub
    '*************************************************************************************************************************

    ' Apply button
    Private Sub oMtbApplyButton_OnClick() Handles oMtbApplyButton.OnClick

        MsgBox("itworks")
        oMiniToolbar.Visible = False
        oUCS.Visible = False
        oCompOcc = Nothing
        oPart = Nothing

    End Sub

    'Private Function Parent(ByRef oOcc As ComponentOccurrence) As CircuitPart
    '    Dim oParts As List(Of CircuitPart) = oServer.CommandCollection.AddNetCommand.CircuitBoard.Parts
    '    For Each oPart As CircuitPart In oParts
    '        If oPart.Occurrence Is oOcc Then
    '            Return oPart
    '        End If
    '    Next
    '    MsgBox("Error")
    '    Return Nothing
    'End Function

    '##################################################################################################
    ' Interaction Events
    '##################################################################################################


    ' SelectEvents: OnSelect
    '*************************************************************************************************************************
    Public Overrides Sub OnSelect(justSelectedEntities As ObjectsEnumerator, _
                                  selectionDevice As SelectionDeviceEnum, _
                                  modelPosition As Inventor.Point, _
                                  viewPosition As Point2d, _
                                  view As Inventor.View)

        'Dim oAttSet As AttributeSet = justSelectedEntities.Item(1).AttributeSets("mid")

        ' If part (occurrence) is selected save it
        If TypeOf justSelectedEntities.Item(1) Is ComponentOccurrence Then

            ' Reset position of the previous selection and update pin positions and connections
            If oPart IsNot Nothing Then
                oPart.Occurrence.Transformation() = oOldPosMatrix ' save old position to restore position later
                oPart.UpdatePins()
                For Each oConnnection As CircuitNet In oConnections
                    oConnnection.Update()
                Next
                oUCS.Visible = False
            End If

            ' Save occurrence
            oCompOcc = justSelectedEntities.Item(1)
            ' Retrieve parent part
            oPart = oServer.CommandCollection.AddNetCommand.CircuitBoard.Parent(oCompOcc)
            ' Save position to restore it later
            oOldPosMatrix = oPart.Occurrence.Transformation()
            ' Set Color
            oHighSet.AddItem(oPart.Occurrence)

            oInteractionEvents.SetCursor(CursorTypeEnum.kCursorBuiltInCursorSelTrail)

        End If

        ' If face AND part are selected, place the component on top of it
        If TypeOf justSelectedEntities.Item(1) Is Face Then

            If oPart IsNot Nothing Then

                oInteractionEvents.SetCursor(CursorTypeEnum.kCursorBuiltInSelectArrow)

                oFace = justSelectedEntities.Item(1)
                'oHighSet.Clear()
                'oHighSet.AddItem(oFace)
                'Dim oStyle As RenderStyle = oDoc.RenderStyles.Item("Gunmetal")
                'oFace.SetRenderStyle(StyleSourceTypeEnum.k, oStyle)

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

                'oPart.Occurrence.SurfaceBodies.Item(1).Faces.Item(1).Evaluator.GetNormal(FaceParams, OccFaceNormal)
                oServer.CommandCollection.AddNetCommand.CircuitBoard.Occurrence.SurfaceBodies.Item(1).Faces.Item(1).Evaluator.GetNormal(FaceParams, boardFaceNormal)

                'Debug.WriteLine("x = "& boardFaceNormal(0) & "y = " & boardFaceNormal(1) & "z = " & boardFaceNormal(2))
                ' Align both normal vectors
                Dim oPartNormalVector As Vector = oTG.CreateVector(boardFaceNormal(0), boardFaceNormal(1), boardFaceNormal(2))
                oFaceNormalVector = oTG.CreateVector(FaceNormal(0), FaceNormal(1), FaceNormal(2))
                oNewPosMatrix = oTG.CreateMatrix()
                oNewPosMatrix.SetToRotateTo(oPartNormalVector, oFaceNormalVector, Nothing) ' ###nothing herer???

                'MsgBox(oFace.SurfaceType)

                ' Set part to face position
                Dim FaceType As SurfaceTypeEnum = oFace.SurfaceType

                Select Case FaceType

                    Case SurfaceTypeEnum.kPlaneSurface

                        Dim FacePoint(2) As Double
                        FacePoint(0) = FaceMinPoint(0) + (FaceMaxPoint(0) - FaceMinPoint(0)) / 2
                        FacePoint(1) = FaceMinPoint(1) + (FaceMaxPoint(1) - FaceMinPoint(1)) / 2
                        FacePoint(2) = FaceMinPoint(2) + (FaceMaxPoint(2) - FaceMinPoint(2)) / 2
                        'oFace.Evaluator.GetPointAtParam(FaceParams, FacePoint)
                        Dim oTransVector As Vector = oTG.CreateVector(FacePoint(0), FacePoint(1), FacePoint(2))
                        oNewPosMatrix.SetTranslation(oTransVector)
                        oPart.Occurrence.Transformation() = oNewPosMatrix

                        ' Reposition user coordinate system
                        oUCS.Transformation() = oNewPosMatrix
                        oUCS.Visible() = True
                        oUCS.XYPlane().Visible = False
                        oUCS.XZPlane().Visible = False
                        oUCS.YZPlane().Visible = False
                        oUCS.XAxis().Visible = False
                        oUCS.YAxis().Visible = False
                        oUCS.ZAxis().Visible = False
                        oUCS.Origin.Visible = False
                        oUCS.XZPlane().Visible = False

                        ' Make toolbar visible and position it
                        oMiniToolbar.Visible = True
                        Dim oTranslationVec As Inventor.Vector2d = oTG.CreateVector2d(0.0, -140.0)
                        viewPosition.TranslateBy(oTranslationVec)
                        oMiniToolbar.Position = viewPosition

                        ' Update mini toolbar position
                        Dim oTransVec2d As Inventor.Vector2d = oTG.CreateVector2d(0, -40)
                        viewPosition.TranslateBy(oTransVec2d)
                        oMiniToolbar.Position = viewPosition

                        ' Update pin positions and connections
                        oPart.UpdatePins()
                        For Each oCon As CircuitNet In oConnections
                            oCon.Update()
                        Next
                        oHighSet.AddItem(oPart.Occurrence)


                    Case SurfaceTypeEnum.kCylinderSurface

                        MessageBox.Show("not supported (cylindrical surface)")

                    Case Else
                        MessageBox.Show("not supported")

                End Select





            End If
        End If



    End Sub

    ' SelectEvents: OnPreSelect
    '*************************************************************************************************************************
    Public Overrides Sub OnPreSelect(ByRef preSelectEntity As Object, _
                                     ByRef doHighlight As Boolean, _
                                     ByRef morePreSelectEntities As ObjectCollection, _
                                     selectionDevice As SelectionDeviceEnum, _
                                     modelPosition As Point, _
                                     viewPosition As Point2d, _
                                     view As Inventor.View)
        'doHighlight = False
        ' filter: over mid (--> faces), over parts (--> occurrences), default: occurrence
        If TypeOf preSelectEntity Is ComponentOccurrence Then

            If preSelectEntity.AttributeSets.NameIsUsed("circuitcarrier") Then
                oSelectEvents.ClearSelectionFilter()
                filterType = SelectionFilterEnum.kPartFaceFilter
                oSelectEvents.AddSelectionFilter(filterType)

            End If

        ElseIf TypeOf preSelectEntity Is Face Then

            ' retrieve component occurrence the face belongs to
            Dim oObj As Object = preSelectEntity.Parent.Parent

            ' KeepOuts are not selectable
            If TypeOf oObj Is ComponentOccurrence Then
                If oObj.AttributeSets.NameIsUsed("circuitcarrier") Then

                    Dim oKeepOuts As KeepOuts = oServer.CommandCollection.KeepOutCommand.KeepOuts
                    If oKeepOuts.IdExists(preSelectEntity.InternalName) Then
                        doHighlight = False
                    End If
                End If
            End If

            If TypeOf oObj Is ComponentOccurrence Then

                If oObj.AttributeSets.NameIsUsed("part") Then
                    oSelectEvents.ClearSelectionFilter()
                    filterType = SelectionFilterEnum.kAssemblyOccurrenceFilter
                    oSelectEvents.AddSelectionFilter(filterType)

                    'ElseIf oObj.AttributeSets.NameIsUsed("mid") Then
                    '    Dim oAttribSets As AttributeSets = preSelectEntity.AttributeSets
                    '    If oAttribSets.Item("mid").Item("isKeepOut").Value = 1 Then
                    '        oSelectEvents.ClearSelectionFilter()
                    '    End If

                End If
            End If



        Else
            ' non-mid-components are not selectable
            doHighlight = False

        End If

    End Sub


    ' Keyboard events
    '*************************************************************************************************************************
    Public Overrides Sub OnKeyPress(keyASCII As Long)

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
'If Not (oCompOcc Is Nothing) Then
'    oSelectEvents.AddToSelectedEntities(oCompOcc)
'End If

'oTriadEvents.Enabled = True







'Set the selection filter to a linear edge
'oSelectEvents.AddSelectionFilter(SelectionFilterEnum.kPartEdgeFilter)

'Set a cursor to specify edge selection
'oInteractionEvents.SetCursor(CursorTypeEnum.kCursorBuiltInArrowCursor)

'Set the status bar message
'oInteractionEvents.StatusBarText = "Select a linear edge on the selected face"
'End If