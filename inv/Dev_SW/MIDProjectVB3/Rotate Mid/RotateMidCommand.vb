Option Explicit On

Imports System.Windows.Forms

'######################################
' Rotate Mid Command
'######################################

Public Class RotateMidCommand
    Inherits Command

    Private oMiniToolbar As Inventor.MiniToolbar
    Private oMtbButtonExit As MiniToolbarButton
    Private oMtbAddFaceToKeepOuts As MiniToolbarButton
    Private oMtbRmvFaceFromKeepOuts As MiniToolbarButton
    Private oMtbButtonReset As MiniToolbarButton

    Private oTransformation As Matrix
    Private oOccurrence As ComponentOccurrence

    Private oServer As MidAddInServer

    ' constructor
    '***************************************************************************************************************
    Public Sub New(MidAddIn As Inventor.Application, _
                   server As MidAddInServer)

        ' call base class
        MyBase.New(MidAddIn)

        oServer = server

        oOccurrence = Nothing
        oTransformation = Nothing

    End Sub

    ' Initialize button execution
    '***************************************************************************************************************
    Public Overrides Sub ButtonDefinition_OnExecute()

        ' Only execute if assemlby environment
        Dim currentEnvironment As Inventor.Environment = MidAddIn.UserInterfaceManager.ActiveEnvironment

        If LCase(currentEnvironment.InternalName) <> LCase("OPTAVER") Then
            MessageBox.Show("This command works only for assembly environment", _
                            "MIDProject", _
                            MessageBoxButtons.OK, _
                            MessageBoxIcon.Error)
            Exit Sub
        End If

        ' Find circuit carrier
        If oServer.MidDataTypes.CircuitCarrier.Occurrence IsNot Nothing Then
            oOccurrence = oServer.MidDataTypes.CircuitCarrier.Occurrence
        Else
            System.Windows.Forms.MessageBox.Show("Could not find a circuit carrier.")
            Exit Sub
        End If

        ' If an other instance is running, stop the command
        If bCommandIsRunning Then
            StopCommand()
        End If

        'Start new command
        StartCommand()
    End Sub

    ' Start/Stop the command
    '***************************************************************************************************************
    Public Overrides Sub StartCommand()

        ' Start Interaction /Interaction.start
        MyBase.StartCommand()

        ' Built-in cursor
        oInteractionEvents.SetCursor(CursorTypeEnum.kCursorBuiltInArrow)

        ' Subscribe to selection event
        MyBase.SubscribeToEvent(Interaction.InteractionTypeEnum.kTriad)

        ' Create local copy of the circuit carrier
        CreatePreviewGraphics()

        ' Enable interaction
        EnableInteraction()

    End Sub

    'Stop Command
    '***************************************************************************************************************
    Public Overrides Sub StopCommand()

        ' Disconnect events sink
        MyBase.StopCommand()

    End Sub

    'Enable Interaction
    '***************************************************************************************************************
    Public Overrides Sub EnableInteraction()

        ' Resubscribe all event handlers for the subscribed events
        MyBase.EnableInteraction()

        oInteractionEvents.StatusBarText = "Rotate triad"

        oTriadEvents.MoveTriadOnly = True
        'oTriadEvents.Repeat = True
        oTriadEvents.Enabled = True

    End Sub

    'Disable Interaction
    '***************************************************************************************************************
    Public Overrides Sub DisableInteraction()

        'Call base command button's DisableInteraction
        MyBase.DisableInteraction()

        'Set the default cursor to notify that selection is not active
        oInteractionEvents.SetCursor(CursorTypeEnum.kCursorBuiltInArrow)

        'Clean up status text
        oInteractionEvents.StatusBarText = String.Empty

    End Sub

    ' Execute command
    ' ***************************************************************************************************************
    Public Overrides Sub ExecuteCommand()

        MyBase.ExecuteCommand()

        StopCommand()

        Dim oMidMoveRequest As New RotateMidRequest(Me, MidAddIn, oServer)

        MyBase.ExecuteChangeRequest(oMidMoveRequest, "MidAddIn:MoveMidRequest", MidAddIn.ActiveDocument)

    End Sub

    'Create preview graphics
    '***************************************************************************************************************
    Private Sub CreatePreviewGraphics()

        ' Create client graphics
        Dim oInteractionGraphics As InteractionGraphics = oInteractionEvents.InteractionGraphics
        Dim oClientGraphics As ClientGraphics = oInteractionGraphics.PreviewClientGraphics


        ' Add Node for occurrence copy
        Dim oOccNode As GraphicsNode = oClientGraphics.AddNode(1)
        ' Add Node for coordinate axes
        Dim oLineNode As GraphicsNode = oClientGraphics.AddNode(2)

        ' occurrence client graphics
        ' Create transient BRep body from occurrence
        ' #1 define surfacebody copy
        Dim oSurfaceBody As SurfaceBody = MidAddIn.TransientBRep.Copy(oOccurrence.SurfaceBodies.Item(1))
        ' #2 create surface graphic
        Dim oSurfaceGraphics As SurfaceGraphics = oOccNode.AddSurfaceGraphics(oSurfaceBody)
        oSurfaceGraphics.DepthPriority = 3

        ' Set render style (transparent)
        Try
            Dim oAsmDoc As AssemblyDocument = MidAddIn.ActiveDocument()
            Dim oStyle As RenderStyle = oAsmDoc.RenderStyles.Item("Clear - Light")
            oOccNode.RenderStyle = oStyle
        Catch ex As Exception
            System.Windows.Forms.MessageBox.Show("Could not find 'Clear'-asset in the asset library")
        End Try

        ' axes client graphics
        ' #1 define dataset
        Dim oDataSets As GraphicsDataSets = oInteractionGraphics.GraphicsDataSets

        Dim oLineCoordSet As GraphicsCoordinateSet = oDataSets.CreateCoordinateSet(1)

        Dim oPointCoord(17) As Double
        oPointCoord(0) = 4 : oPointCoord(1) = 0 : oPointCoord(2) = 0 : oPointCoord(3) = -4 : oPointCoord(4) = 0 : oPointCoord(5) = 0
        oPointCoord(6) = 0 : oPointCoord(7) = 4 : oPointCoord(8) = 0 : oPointCoord(9) = 0 : oPointCoord(10) = -4 : oPointCoord(11) = 0
        oPointCoord(12) = 0 : oPointCoord(13) = 0 : oPointCoord(14) = 4 : oPointCoord(15) = 0 : oPointCoord(16) = 0 : oPointCoord(17) = -4
        oLineCoordSet.PutCoordinates(oPointCoord)

        ' #2 Create line graphic
        Dim oLineGraphics As LineGraphics = oLineNode.AddLineGraphics
        oLineGraphics.CoordinateSet = oLineCoordSet
        Dim oColorSet As GraphicsColorSet = oDataSets.CreateColorSet(1)
        oColorSet.Add(1, 255, 161, 0)
        oLineGraphics.ColorSet = oColorSet
        oLineGraphics.BurnThrough = True
        oLineGraphics.DepthPriority = 4

        ' Update view
        MidAddIn.ActiveView.Update()

    End Sub

    ' Update Preview Graphics
    ' ***************************************************************************************************************
    Private Sub UpdatePreviewGraphics(ByRef coordinateSystem As Matrix)

        Dim oInteractionGraphics As InteractionGraphics = oInteractionEvents.InteractionGraphics
        Dim oClientGraphics As ClientGraphics = oInteractionGraphics.PreviewClientGraphics

        ' Set transformation
        'If oClientGraphics IsNot Nothing Then
        Dim oOccNode As GraphicsNode = oClientGraphics.Item(1)
        oOccNode.Transformation = coordinateSystem

        MidAddIn.ActiveView.Update()
        'End If

    End Sub

    ' OnEndSequence
    ' ***************************************************************************************************************
    Public Overrides Sub OnEndSequence(cancelled As Boolean, _
                                       coordinateSystem As Matrix, _
                                       context As NameValueMap, _
                                       ByRef handlingCode As HandlingCodeEnum)

        ' The user clicks apply or ok/exit
        If cancelled = False Then
            
            oTransformation = coordinateSystem
            ExecuteCommand()
          
        End If

        handlingCode = HandlingCodeEnum.kEventHandled

    End Sub

    ' Return position matrix
    ' ***************************************************************************************************************
    Public ReadOnly Property Transformation As Matrix
        Get
            Return oTransformation
        End Get
    End Property

    ' OnMove
    ' ***************************************************************************************************************
    Public Overrides Sub OnMove(selectedTriadSegment As TriadSegmentEnum, _
                                shiftKeys As ShiftStateEnum, _
                                coordinateSystem As Matrix, _
                                context As NameValueMap, _
                                ByRef handlingCode As HandlingCodeEnum)

        ' Set triad position to client graphics position
        UpdatePreviewGraphics(coordinateSystem)

    End Sub


    ' OnActivate
    ' ***************************************************************************************************************
    Public Overrides Sub OnActivate(context As NameValueMap, _
                                    ByRef handlingCode As HandlingCodeEnum)
        ' remark: degreesOfFreendom can only be set in the OnActivate methode
        ' won't work in other places!
        Dim DOF = TriadSegmentEnum.kXAxisRotationSegment Or _
                  TriadSegmentEnum.kYAxisRotationSegment Or _
                  TriadSegmentEnum.kZAxisRotationSegment

        oTriadEvents.DegreesOfFreedom = DOF

        ' Set triad to the position of the ciruit carrier
        oTriadEvents.GlobalTransform = oOccurrence.Transformation

    End Sub

    ' OnTerminate (not used)
    ' ***************************************************************************************************************
    'Public Overrides Sub OnTerminate(cancelled As Boolean, _
    '                                   context As NameValueMap, _
    '                                   handlingCode As HandlingCodeEnum)

    '    'StopCommand()

    'End Sub

    'Public Overrides Sub OnSegmentSelectionChange(selectedTriadSegment As TriadSegmentEnum, _
    '                                               beforeOrAfter As EventTimingEnum, _
    '                                               context As NameValueMap, _
    '                                               ByRef handlingCode As HandlingCodeEnum)
    '    ' Make surface graphic visible
    '    'oClientGraphics.Item(1).Visible = True

    'End Sub


    'Public Overrides Sub OnMoveTriadOnlyToggle(moveTriadOnly As Boolean, _
    '                                             beforeOrAfter As EventTimingEnum, _
    '                                             context As NameValueMap, _
    '                                             ByRef handlingCode As HandlingCodeEnum)
    '    'MsgBox("OnMoveTriadOnlyToggle")
    'End Sub

    'Public Overrides Sub SubOnEndMove(selectedTriadSegment As TriadSegmentEnum, _
    '                                    shiftKeys As ShiftStateEnum, _
    '                                    coordinateSystem As Matrix, _
    '                                    context As NameValueMap, _
    '                                    ByRef handlingCode As HandlingCodeEnum)
    '    'MsgBox("SubOnEndMove")
    'End Sub



    'Public Overrides Sub OnStartMove(selectedTriadSegment As TriadSegmentEnum, _
    '                                   shiftKeys As ShiftStateEnum, _
    '                                   coordinateSystem As Matrix, _
    '                                   context As NameValueMap, _
    '                                   ByRef handlingCode As HandlingCodeEnum)
    '    'MsgBox("OnStartMove")
    'End Sub

    'Public Overrides Sub OnStartSequence(coordinateSystem As Matrix, _
    '                                       context As NameValueMap, _
    '                                      ByRef handlingCode As HandlingCodeEnum)
    '    'MsgBox("OnStartSequence")
    'End Sub

    'Public Overrides Sub OnEndMove(selectedTriadSegment As TriadSegmentEnum, _
    '                                 shiftKeys As ShiftStateEnum, _
    '                                 coordinateSystem As Matrix, _
    '                                 context As NameValueMap, _
    '                                 ByRef handlingCode As HandlingCodeEnum)
    '    'MsgBox("OnEndMove")
    'End Sub

End Class

