Imports Inventor
Imports System.Collections.Generic

Public Class InteractionTriad

    Private WithEvents oInteractionEvents As InteractionEvents
    Private WithEvents oTriadEvents As TriadEvents

    Private oMiniToolbar As MiniToolbar

    Private oInventorAddin As Inventor.Application

    Private oMatrix As Matrix

    Private oBoard As circuitBoard

    Private bTriadRunning As Boolean

    'Private oPartList As List(Of Part)

    ' constructor

    Public Sub New(ByRef oInventorAddin1 As Inventor.Application)
        ' call base class (implicitly)
        MyBase.New()

        Me.oInventorAddin = oInventorAddin1

        

    End Sub

    '#############################################################
    ' EVENT: Ok-button
    '#############################################################

    Public Sub Triad_OnEndSequence(Cancelled As Boolean, _
                                   CoordinateSystem As Matrix, _
                                   Context As NameValueMap, _
                                   ByRef HandlingCode As HandlingCodeEnum) Handles oTriadEvents.OnEndSequence

        oBoard.Transformation = CoordinateSystem
        ' Dim i As Long
        'For i = 1 To Context.Count
        'Debug.WriteLine("Context: ", Context.Name(i), Context.Value(Context.Name(i)))
        ' Next


        bTriadRunning = False



    End Sub

    Public Sub Triad_OnSegmentSelectionChange(SelectedTriadSegment As TriadSegmentEnum, _
                                              BeforeOrAfter As EventTimingEnum, _
                                              Context As NameValueMap, _
                                              ByRef HandlingCode As HandlingCodeEnum) Handles oTriadEvents.OnSegmentSelectionChange

        'MsgBox(SelectedTriadSegment)

    End Sub

    '##########################################################
    ' ON MOVE EVENT
    '##########################################################

    Public Sub Triad_OnMove(SelectedTriadSegment As TriadSegmentEnum, _
                            ShiftKeys As ShiftStateEnum, _
                            CoordinateSystem As Matrix, _
                            Context As NameValueMap, _
                            ByRef oHandlingCode As HandlingCodeEnum) Handles oTriadEvents.OnMove

        ' set board to current triad position
        oBoard.Transformation = CoordinateSystem

        ' set parts to current triad position


    End Sub


    '##############################################################
    ' Enable Triad
    '##############################################################

    Public Sub StartTriad(ByRef oBoard As circuitBoard)

        'oMiniToolbar = oInventorAddin.CommandManager.CreateMiniToolbar()

        'oMiniToolbar.ShowOK = False
        'oMiniToolbar.ShowApply = False
        'oMiniToolbar.ShowCancel = False

        'Dim oControls As MiniToolbarControls = oMiniToolbar.Controls

        bTriadRunning = True

        oInteractionEvents = oInventorAddin.CommandManager.CreateInteractionEvents()

        oTriadEvents = oInteractionEvents.TriadEvents

        Me.oBoard = oBoard

        oInteractionEvents.SelectionActive = False
        oInteractionEvents.InteractionDisabled = True


        'oTriadEvents.Reposition(TriadSegmentEnum.kAllSegments, oBoard)
        'oTriadEvents.DegreesOfFreedom(TriadSegmentEnum.kXYPlaneTranslationSegment)
        oTriadEvents.Repeat() = True
        oTriadEvents.Enabled() = True
        'oTriadEvents.MoveTriadOnlyEnabled() = True
        'oTriadEvents.MoveTriadOnly() = True
        'Dim oRestricion As TriadSegmentEnum =



        'oTriadEvents.Reposition(TriadSegmentEnum.kAllSegments, oCompOcc)

        'oTriadEvents.Repeat = True
        'oTriadEvents.True()

        'oMiniToolbar.Visible = True

        oInteractionEvents.Start()

        oInteractionEvents.StatusBarText = "Define the import position of the components"

        ' Set degreesOfFreedom AFTER Interaction.Start()
        Dim DOF = TriadSegmentEnum.kXAxisTranslationSegment Or _
            TriadSegmentEnum.kYAxisTranslationSegment Or _
            TriadSegmentEnum.kZAxisTranslationSegment Or _
            TriadSegmentEnum.kOriginSegment

        oTriadEvents.DegreesOfFreedom() = DOF

        oTriadEvents.GlobalTransform() = oBoard.Transformation()

        'oMatrix = oTriadEvents.GlobalTransform().GetMatrixData
        'oCompOcc.SetTransformWithoutConstraints(oMatrix)a

        While bTriadRunning
            oInventorAddin.UserInterfaceManager.DoEvents()
            'oTriadEvents.GlobalTransform() = oBoard.GetOccurrence.Transformation()
        End While

        oInteractionEvents.Stop()


    End Sub




    Private Sub UserForm1_Terminate()
        oInteractionEvents.Stop()
        oTriadEvents = Nothing
        oInteractionEvents = Nothing
    End Sub



    Public Sub TriadEventsTest()
        'UserForm1.Show(vbModeless)
    End Sub



End Class
