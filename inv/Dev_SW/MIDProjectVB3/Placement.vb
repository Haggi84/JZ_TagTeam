Option Strict Off
Option Explicit On

Imports Inventor
Imports System.Collections.Generic

Public Class Placement

    Public Class cmd

    End Class

    Private oInteractionEvents As InteractionEvents
    Private oSelectEvents As SelectEvents
    Private bStillSelecting As Boolean


    Private WithEvents oFaceIntEvents As InteractionEvents
    Private WithEvents oFaceSelEvents As SelectEvents
    Private bFaceStillSelecting As Boolean

    Private oFace As FaceProxy
    Private oInventorAddin As Inventor.Application

    Private oMiniToolbar As Inventor.MiniToolbar
    Private WithEvents oMtbApplyButton As MiniToolbarButton
    Private WithEvents oMtbCancelButton As MiniToolbarButton
    Private WithEvents oMtbExitButton As MiniToolbarButton

    Private oMtbPosition As Inventor.Point2d

    Private oOccPosMatrix As Matrix
    'Private oEntityList As Inventor.ObjectsEnumerator
    ' flag that is used to determine when selection stops

    Public count As Integer = 0

    Private oAsmDoc As AssemblyDocument

    'Private assetLib As AssetLibrary

    'Private libAssetRed As Asset
    'Private LibAssetDefault As Asset

    'Private LocAssetRed As Asset
    'Private LocAssetDefault As Asset

    Private oCompOcc As ComponentOccurrence

    Private oTG As TransientGeometry

    ' Triad Event variables

    Private bTriadRunning As Boolean

    Private WithEvents oTriadEvents As TriadEvents



    '#################################################
    ' constructor
    '#################################################

    Public Sub New(oInventorAddin1 As Inventor.Application)
        ' call base class (implicitly)
        MyBase.New()

        Me.oInventorAddin = oInventorAddin1

        Me.oAsmDoc = oInventorAddin.ActiveDocument

        Me.oTG = oInventorAddin1.TransientGeometry

    End Sub


    ''#################################################################
    '' EVENT (oFaceInteractionEvent) OnHover
    ''#################################################################

    Private Sub oSelectiEvent_OnPreSelect(ByRef PreSelectEntity As Object, _
                                          ByRef DoHighlight As Boolean, _
                                          ByRef MorePreSelectEntities As ObjectCollection, _
                                          SelectionDevice As SelectionDeviceEnum, _
                                          ModelPosition As Point, _
                                          ViewPosition As Point2d, _
                                          View As View) 'Handles oSelectEvents.OnPreSelect

        DoHighlight = True
        'MsgBox("preslect")

    End Sub




    '##################################################################
    ' EVENT (oInteractionEvent) OnSelect
    '##################################################################

    Private Sub oSelectEvents_OnSelect(ByVal JustSelectedEntities As Inventor.ObjectsEnumerator, _
                                       ByVal SelectionDevice As Inventor.SelectionDeviceEnum, _
                                       ByVal ModelPosition As Inventor.Point, _
                                       ByVal ViewPosition As Inventor.Point2d, _
                                       ByVal View As Inventor.View) 'Handles oSelectEvents.OnSelect

        ' behavior depends on selected entity (Face or componentOccurrence)

        If TypeOf JustSelectedEntities.Item(1) Is ComponentOccurrence Then

            oCompOcc = JustSelectedEntities.Item(1)
            oOccPosMatrix = oCompOcc.Transformation()

          

            'Dim oSurfacebody As SurfaceBody = oCompOcc.SurfaceBodies.Item(1)

            'Dim tmpFace As Face = oSurfacebody.Faces.Item(1)

            'LibAssetDefault = tmpFace.Appearance()

            'tmpFace.Appearance() = libAssetRed

            ' Color the occurrence
            'oCompOcc.Appearance() = libAssetRed


            ' Change the selection filter to face selection
            oSelectEvents.ClearSelectionFilter()
            oSelectEvents.AddSelectionFilter(SelectionFilterEnum.kPartFaceFilter)



        End If

        If TypeOf JustSelectedEntities.Item(1) Is FaceProxy Then
            ' save the selected face
            oFace = JustSelectedEntities.Item(1)

            If oFace.SurfaceType = SurfaceTypeEnum.kPlaneSurface Then

                ' set the minitoolbar position to the mouse position
                oMiniToolbar.Visible = True
                Dim oTranslationVec As Inventor.Vector2d = oInventorAddin.TransientGeometry.CreateVector2d(0.0, -40.0)
                ViewPosition.TranslateBy(oTranslationVec)
                oMiniToolbar.Position = ViewPosition

                ' Show triad
                'CreateTriad()

                ' Reset component occurrence (SMD component)
                oCompOcc.SetTransformWithoutConstraints(oTG.CreateMatrix())

                ' create a vector out of the vertex position
                Dim oPoint1(3) As Double
                Dim oPoint2(3) As Double
                Dim oPoint3(3) As Double
                Dim oPoint4(3) As Double

                'Dim oPoint1 As Point
                'Dim oPoint2 As Point
                'Dim oPoint3 As Point
                'Dim oPoint4 As Point

                ' oPoint1 = oFace.Evaluator.RangeBox.MaxPoint
                'oPoint2 = oFace.Evaluator.RangeBox.MaxPoint


                'oFace.Vertices().Item(1).GetPoint(oPoint1)
                'oFace.Vertices().Item(2).GetPoint(oPoint2)
                'oFace.Vertices().Item(3).GetPoint(oPoint3)
                'oFace.Vertices().Item(4).GetPoint(oPoint4)

                'Dim vector1 As Vector = oTG.CreateVector(oPoint1(0), oPoint1(1), oPoint1(2))
                'Dim vector2 As Vector = oTG.CreateVector(oPoint2(0), oPoint2(1), oPoint2(2))
                'Dim vector3 As Vector = oTG.CreateVector(oPoint3(0), oPoint3(1), oPoint3(2))
                'Dim vector4 As Vector = oTG.CreateVector(oPoint4(0), oPoint4(1), oPoint4(2))





                'Debug.WriteLine("x = " & oPoint1(0) & ", y = " & oPoint1(1) & ", z = " & oPoint1(2))
                'Debug.WriteLine("x = " & oPoint2(0) & ", y = " & oPoint2(1) & ", z = " & oPoint2(2))
                'Debug.WriteLine("x = " & oPoint3(0) & ", y = " & oPoint3(1) & ", z = " & oPoint3(2))
                'Debug.WriteLine("x = " & oPoint4(0) & ", y = " & oPoint4(1) & ", z = " & oPoint4(2))
                Debug.WriteLine("Number of Vertices: " & oFace.Vertices.Count)
                Debug.WriteLine("Surface Type: " & oFace.Type.ToString)
                Debug.WriteLine("Surface Surface-Type: " & oFace.SurfaceType.ToString)

                Dim oPoint100 As Point = oFace.Evaluator.RangeBox.MaxPoint
                Dim oPoint101 As Point = oFace.Evaluator.RangeBox.MinPoint
                Debug.WriteLine("xMax = " & oPoint100.X & ", yMax = " & oPoint100.Y & ", zMax = " & oPoint100.Z)
                Debug.WriteLine("xMin = " & oPoint101.X & ", yMin = " & oPoint101.Y & ", zMin = " & oPoint101.Z)

                Dim normal(2) As Double
                Dim params(1) As Double

                params(0) = 0.1
                params(1) = 0.1

                Dim temp(2) As Double
                temp(0) = oPoint101.X
                temp(1) = oPoint101.Y
                temp(2) = oPoint101.Z

                oFace.Evaluator.GetNormalAtPoint(temp, normal)
                Debug.WriteLine("Face normal: x = " & normal(0) & ", y = " & normal(1) & ", y = " & normal(2))
                '#############################################################################

                ' Normal of the occurrence face
                Dim faceNormal(2) As Double
                oCompOcc.SurfaceBodies.Item(1).Faces.Item(1).Evaluator.GetNormal(params, faceNormal)

                Debug.WriteLine("OccfaceNormal: x = " & faceNormal(0) & ", y = " & faceNormal(1) & ", y = " & faceNormal(2))

                Dim oOccNormalVector As Vector = oTG.CreateVector(0, 1, 0) '(faceNormal(0), faceNormal(1), faceNormal(2))

                Dim oNormalVector As Vector = oTG.CreateVector(normal(0), normal(1), normal(2))

                Dim oMatrix3 As Matrix = oTG.CreateMatrix()

                oMatrix3.SetToRotateTo(oNormalVector, oOccNormalVector)
                oMatrix3.Cell(1, 4) = oPoint101.X
                oMatrix3.Cell(2, 4) = oPoint101.Y
                oMatrix3.Cell(3, 4) = oPoint101.Z
                Debug.WriteLine("oMatrix3")
                For i As Integer = 1 To 4
                    For j As Integer = 1 To 4
                        Debug.Write(oMatrix3.Cell(i, j))
                        Debug.Write(" ")
                    Next
                    Debug.WriteLine(" ")
                Next

                Dim oXAxis As Vector = oTG.CreateVector(1, 0, 0)
                Dim oYAxis As Vector = oTG.CreateVector(0, 1, 0)
                Dim oZAxis As Vector = oTG.CreateVector(0, 0, 1)


                Dim alpha As Double = Math.Acos(oNormalVector.DotProduct(oXAxis))
                If alpha > Math.PI / 2 Then
                    alpha = Math.PI - alpha
                End If
                alpha = Math.PI / 2 - alpha

                Dim beta As Double = Math.Acos(oNormalVector.DotProduct(oYAxis))
                If beta > Math.PI / 2 Then
                    beta = Math.PI - beta
                End If
                beta = Math.PI / 2 - beta


                Dim gamma As Double = Math.Acos(oNormalVector.DotProduct(oZAxis))
                If gamma > Math.PI / 2 Then
                    gamma = Math.PI - gamma
                End If
                gamma = Math.PI / 2 - gamma






                Dim oTransVector As Vector = oTG.CreateVector(oPoint101.X, oPoint101.Y, oPoint101.Z)

                ' Copy the position of the Occurrence in a Matrix
                Dim oOccPositionMatrix As Matrix = oCompOcc.Transformation()


                'For i As Integer = 1 To 4
                '    For j As Integer = 1 To 4
                '        Debug.Write(oOccPositionMatrix.Cell(i, j))
                '        Debug.Write(" ")
                '    Next
                '    Debug.WriteLine(" ")
                'Next

                ' Translate the matrix by the selected face position
                'oOccPositionMatrix.SetTranslation(oTransVector)

                'Debug.WriteLine("Alpha: " & alpha * 180 / Math.PI)
                'Debug.WriteLine("Beta: " & beta * 180 / Math.PI)
                'Debug.WriteLine("Gamma: " & gamma * 180 / Math.PI)

                '#############################################################################
                ' Retrieve the angle by substracting the Points (Vertex Positions of the faces) and calculating the dot product
                'Dim oVector5 As Vector = oTG.CreateVector(oPoint1(0) - oPoint4(0), oPoint1(1) - oPoint4(1), oPoint1(2) - oPoint4(2))
                ' oVector5.Normalize()

                ' Create Vector pointing in x-direction
                'Dim oVector6 As Vector = oTG.CreateVector(1, 0, 0)

                'Dim alpha As Double = Math.Acos(oVector6.DotProduct(oVector5))

                Dim oTestVector1 As Vector = oTG.CreateVector(oPoint100.X - oPoint101.X, oPoint100.Y - oPoint101.Y, oPoint100.Z - oPoint101.Z)
                oTestVector1.Normalize()

                'Debug.WriteLine("oTestVector1: x = " & oTestVector1.X & ", y = " & oTestVector1.Y & ", y = " & oTestVector1.Z)


                Dim oTestVector2 As Vector = oTestVector1.CrossProduct(oNormalVector)
                oTestVector2.Normalize()

                'Debug.WriteLine("oTestVector1: x = " & oTestVector1.X & ", y = " & oTestVector1.Y & ", y = " & oTestVector1.Z)
                'Debug.WriteLine("oTestVector2: x = " & oTestVector2.X & ", y = " & oTestVector2.Y & ", y = " & oTestVector2.Z)

                ' Create matrix that maps form the standard to the face coordinate system
                Dim oTransMatrix As Matrix = oTG.CreateMatrix()
                oTransMatrix.SetCoordinateSystem(oPoint101, oTestVector2, oNormalVector, oTestVector1)

                Debug.WriteLine("oOccPositionMatrix")
                For i As Integer = 1 To 4
                    For j As Integer = 1 To 4
                        Debug.Write(oTransMatrix.Cell(i, j))
                        Debug.Write(" ")
                    Next
                    Debug.WriteLine(" ")
                Next

                '######################################################################
                Dim oTransMatrixZ As Matrix = oTG.CreateMatrix()

                'Dim oTransMatrixY As Matrix = oTG.CreateMatrix()

                'Dim oTransMatrixX As Matrix = oTG.CreateMatrix()

                'oTransMatrixZ.SetToRotation(alpha, oTG.CreateVector(0, 0, 1), oTG.CreatePoint(0, 0, 0))

                'oTransMatrixY.SetToRotation(beta, oTG.CreateVector(1, 0, 0), oTG.CreatePoint(0, 0, 0))

                'oTransMatrixX.SetToRotation(gamma, oTG.CreateVector(0, 0, 1), oTG.CreatePoint(0, 0, 0))

                '####################
                ' Other technique

                ' oTransMatrixZ.TransformBy(oTransMatrixY)

                'Dim oWorkplane As WorkPlane = oAsmDoc.ComponentDefinition.WorkPlanes.Item(1)

                'oWorkplane.SetByPlaneAndPoint(oFace, oPoint100)
                'oTransMatrixY.TransformBy(oOccPositionMatrix)

                ' oTransMatrixZ.TransformBy(oOccPositionMatrix)

                'oTransMatrixY.TransformBy(oTransMatrixX)
                'Dim oDoc As PartDocument
                'oDoc = oInventorAddin.Documents.Add(DocumentTypeEnum.kPartDocumentObject)

                Dim oCompDef As AssemblyComponentDefinition = oAsmDoc.ComponentDefinition

                Dim oWorkPoint1 As WorkPoint = oCompDef.WorkPoints.AddFixed(oTG.CreatePoint(oPoint101.X + oTestVector1.X, oPoint101.Y + oTestVector1.Y, oPoint101.Z + oTestVector1.Z))

                Dim oWorkPoint2 As WorkPoint = oCompDef.WorkPoints.AddFixed(oTG.CreatePoint(oPoint101.X + oTestVector2.X, oPoint101.Y + oTestVector2.Y, oPoint101.Z + oTestVector2.Z))

                Dim oWorkPoint3 As WorkPoint = oCompDef.WorkPoints.AddFixed(oTG.CreatePoint(oPoint101.X + normal(0), oPoint101.Y + normal(1), oPoint101.Z + normal(2)))

                Dim oWorkPoint4 As WorkPoint = oCompDef.WorkPoints.AddFixed(oTG.CreatePoint(oPoint101.X, oPoint101.Y, oPoint101.Z))

                'Dim oUCSDef As UserCoordinateSystemDefinition = oCompDef.UserCoordinateSystems.CreateDefinition()

                'oUCSDef.SetByThreePoints(oWorkPoint1, oWorkPoint2, oWorkPoint3)

                'Dim oTransMatrix As Matrix = oTG.CreateMatrix()

                'oTransMatrix.Cell(1, 1) = oTestVector1.X
                'oTransMatrix.Cell(2, 1) = oTestVector1.Y
                'oTransMatrix.Cell(3, 1) = oTestVector1.Z

                'oTransMatrix.Cell(1, 2) = oTestVector2.X
                'oTransMatrix.Cell(2, 2) = oTestVector2.Y
                'oTransMatrix.Cell(3, 2) = oTestVector2.Z

                'oTransMatrix.Cell(1, 3) = oNormalVector.X
                'oTransMatrix.Cell(2, 3) = oNormalVector.Y
                'oTransMatrix.Cell(3, 3) = oNormalVector.Z

                'For i As Integer = 1 To 4
                '    For j As Integer = 1 To 4
                '        Debug.Write(oTransMatrix.Cell(i, j))
                '        Debug.Write(" ")
                '    Next
                '    Debug.WriteLine(" ")
                'Next

                ' oCompOcc.SetTransformWithoutConstraints(oTransMatrix)
                ' Dim oUCS As UserCoordinateSystemProxy = oCompDef.UserCoordinateSystems.Add(oUCSDef)

                ' Dim oTransMatrix3 As Matrix = oTG.CreateMatrix
                'oTransMatrix3.TransformBy(oTransMatrix)

                'For i As Integer = 1 To 4
                '    For j As Integer = 1 To 4
                '        Debug.Write(oTransMatrix3.Cell(i, j))
                '        Debug.Write(" ")
                '    Next
                '    Debug.WriteLine(" ")
                'Next
                Dim oPoint200(2) As Double
                'oFace.Evaluator.GetPointAtParam(params, oPoint200)

                'oFace.Vertices.Item(3).GetPoint()


                oCompOcc.Transformation() = oMatrix3

                '#########################################################################################
                '' TRIAD 
                'oTriadEvents.GlobalTransform() = oCompOcc.Transformation
                'Dim DOF = TriadSegmentEnum.kXAxisTranslationSegment Or _
                'TriadSegmentEnum.kZAxisTranslationSegment Or _
                'TriadSegmentEnum.kYAxisRotationSegment Or _
                'TriadSegmentEnum.kOriginSegment

                'oTriadEvents.DegreesOfFreedom() = DOF

                '' Disable selection, enable triad
                'oSelectEvents.Enabled = False
                'oTriadEvents.Enabled() = True

                '' Make minitoolbar visible
                'oMiniToolbar.Visible = True

            Else
                MsgBox("You cannot place components on this surface type")
            End If

        End If

    End Sub


    '##################################################################
    ' EVENT OnActivate
    '##################################################################

    Private Sub oInteractionEvents_OnActivate() 'Handles oInteractionEvents.OnActivate
        ' initialize the flag to specify selection is still active
        'bStillSelecting = True
        MsgBox("Activated")
    End Sub

    Private Sub oInteractionEvents_OnTerminate() 'Handles oInteractionEvents.OnTerminate
        MsgBox("Terminated")
        ' set the flag to indicate selection is done
        'bStillSelecting = False
    End Sub

    '#######################################################################################################
    ' INIT INTERACTION EVENTS
    '#######################################################################################################

    Public Sub SelectEntity()

        ' initialize the interaction events objects
        oInteractionEvents = oInventorAddin.CommandManager.CreateInteractionEvents()
        ' set a reference to the select events
        oSelectEvents = oInteractionEvents.SelectEvents
        ' set a reference to the triad events
        oTriadEvents = oInteractionEvents.TriadEvents
        ' Initialize minitoolbar
        CreateMiniToolbar()

        'AddHandler oInteractionEvents.OnActivate, AddressOf oInteractionEvents_OnActivate
        'AddHandler oInteractionEvents.OnTerminate, AddressOf oInteractionEvents_OnTerminate

        'AddHandler oSelectEvents.OnSelect, AddressOf oSelectEvents_OnSelect

        If Not oInteractionEvents Is Nothing And Not oSelectEvents Is Nothing Then

            'assetLib = oInventorAddin.AssetLibraries.Item("Autodesk Appearance Library")
            'libAssetRed = assetLib.AppearanceAssets.Item("Smooth - Red")
            'LocAssetRed = libAssetRed.CopyTo(oAsmDoc)

            'oMiniToolbar = oInventorAddin.CommandManager.CreateMiniToolbar()

            'oMiniToolbar.ShowOK = False
            'oMiniToolbar.ShowApply = False
            'oMiniToolbar.ShowCancel = False

            'Dim oControls As MiniToolbarControls = oMiniToolbar.Controls



            'define() 'select events'
            'oInteractionEvents.SelectionActive = True

            'oInteractionEvents.InteractionDisabled = False

            oInteractionEvents.StatusBarText = "Select an occurrence"
            oSelectEvents.AddSelectionFilter(SelectionFilterEnum.kAssemblyOccurrenceFilter)
            oSelectEvents.MouseMoveEnabled = False
            oSelectEvents.SingleSelectEnabled = True
            ' start the InteractionEvents object...

            bStillSelecting = True

            ' Start Interaction
            oInteractionEvents.Start()

            ' Set degreesOfFreedom AFTER Interaction.Start()

            'oTriadEvents.GlobalTransform() = oCompOcc.Transformation

            'oMatrix = oTriadEvents.GlobalTransform().GetMatrixData
            'oCompOcc.SetTransformWithoutConstraints(oMatrix)a

            'While bStillSelecting
            '    oInventorAddin.UserInterfaceManager.DoEvents()
            '    'oTriadEvents.GlobalTransform() = oBoard.GetOccurrence.Transformation()
            'End While

            'oInteractionEvents.Stop()





            'oInteractionEvents.Start()

            Do While bStillSelecting
                oInventorAddin.UserInterfaceManager.DoEvents()
            Loop

            ''Dim oSelectedEntities As Inventor.ObjectsEnumerator = oSelectEvents.SelectedEntities

            ''oCompOcc = oSelectedEntities.Item(1)

            'oInteractionEvents.Stop()

            oSelectEvents = Nothing
            oInteractionEvents = Nothing

        End If

    End Sub

    '#############################################################
    ' EVENTS (Minitoolbar)
    '#############################################################

    Private Sub oMtbExitButton_OnActivate() Handles oMtbExitButton.OnClick

        oMiniToolbar.Delete()

        'bStillSelecting = False

    End Sub

    Private Sub oMtbCancelButton_OnActivate() Handles oMtbCancelButton.OnClick

        oCompOcc.Transformation() = oOccPosMatrix

        ' Go back to occurrence selection
        oTriadEvents.Enabled = False
        oSelectEvents.Enabled = True
        oSelectEvents.ClearSelectionFilter()
        oSelectEvents.AddSelectionFilter(SelectionFilterEnum.kAssemblyOccurrenceFilter)

        oMiniToolbar.Visible() = False

    End Sub

    Private Sub oMtbApplyButton_OnActivate() Handles oMtbApplyButton.OnClick

        ' End selection process and delete toolbar after applying
        ' bStillSelecting = False
        oMiniToolbar.Delete()


    End Sub

    '#############################################################
    ' Mini Toolbar
    '#############################################################

    Private Sub CreateMiniToolbar()
        'Dim oActiveEnv As Environment = MIDApplication.UserInterfaceManager.ActiveEnvironment

        oMiniToolbar = oInventorAddin.CommandManager.CreateMiniToolbar()

        oMiniToolbar.ShowOK = False
        oMiniToolbar.ShowApply = False
        oMiniToolbar.ShowCancel = False

        Dim oControls As MiniToolbarControls = oMiniToolbar.Controls
        oControls.Item("MTB_Options").Visible = False



        Dim oDescriptionLabel As MiniToolbarControl = oControls.AddLabel("Description", "Place here?", "")
        oControls.AddNewLine()

        ' Button #1
        'Dim mtbAddPicture As stdole.IPictureDisp = MIDProjectVB3.PictureConverter.ImageToPictureDisp(My.Resources.mtbAddSmall1)
        Dim mtbExitPicture As stdole.IPictureDisp = MIDProjectVB3.PictureConverter.ImageToPictureDisp(My.Resources.mtbExitSmall1)
        Dim mtbRmvPicture As stdole.IPictureDisp = MIDProjectVB3.PictureConverter.ImageToPictureDisp(My.Resources.mtbRmvSmall1)
        ' remark: use only 16x16px Images for StandardIcon

        ' Dim MIDButtonTwo As ButtonDefinition = oInventorAddin.CommandManager.ControlDefinitions.Item("PartCoilCmd")

        ' Define the first center position
        oMtbCancelButton = oControls.AddButton("CancelOccInternal", "", "Discard", mtbRmvPicture, mtbRmvPicture)
        oMtbApplyButton = oControls.AddButton("ApplyOccInternal", "", "Apply", mtbExitPicture, mtbExitPicture)
        oMtbExitButton = oControls.AddButton("ExitOccInternal", "Exit", "Exit")

        ' Create Minitoolbar on the upper left of the window
        oMtbPosition = oInventorAddin.TransientGeometry.CreatePoint2d(0, 0)
        oMiniToolbar.Position = oMtbPosition
        oMiniToolbar.Visible = False


        Dim bMtbActive As Boolean = True

    End Sub

    '##########################################################
    ' TRIAD: ON MOVE EVENT
    '##########################################################

    Public Sub Triad_OnMove(SelectedTriadSegment As TriadSegmentEnum, _
                            ShiftKeys As ShiftStateEnum, _
                            CoordinateSystem As Matrix, _
                            Context As NameValueMap, _
                            ByRef oHandlingCode As HandlingCodeEnum) Handles oTriadEvents.OnMove

        ' set board to current triad position
        oCompOcc.Transformation = CoordinateSystem

        ' set parts to current triad position


    End Sub

End Class