Option Explicit On

Imports System.Windows.Forms


Public Class MoveMidCommand
    Inherits Command


    Private oKeepOutDlg As KeepOutCmdDlg

    Private oMiniToolbar As Inventor.MiniToolbar
    Private oMtbButtonExit As MiniToolbarButton
    Private oMtbAddFaceToKeepOuts As MiniToolbarButton
    Private oMtbRmvFaceFromKeepOuts As MiniToolbarButton
    Private oMtbButtonReset As MiniToolbarButton
    Private oMatrix As Matrix
    Private oTG As TransientGeometry

    Private oOccurrence As ComponentOccurrence
    'Private oClientGraphics As ClientGraphics
    ' Private oDataSets As GraphicsDataSets




    'Private oOccNode As GraphicsNode
    'Private oLineNode As GraphicsNode
    'Private oSurfaceBody As SurfaceBody
    'Private oSurfaceGraphics As SurfaceGraphics
    'Private oLineCoordSet As GraphicsCoordinateSet
    'Private oLineGraphics As LineGraphics
    'Private oColorSet As GraphicsColorSet
    'Private oRenderStyle As RenderStyle


    'Private oTransformation As Matrix
    ' Private oUCS As UserCoordinateSystem


    Private oServer As MidAddInServer

    ' constructor
    '***************************************************************************************************************
    Public Sub New(MidAddIn As Inventor.Application, _Server As MidAddInServer)
        ' call base class
        MyBase.New(MidAddIn)

        oServer = _Server '###later in base class

        oOccurrence = Nothing
        'oClientGraphics = Nothing
        'oDataSets = Nothing
        'oUCS = Nothing

    End Sub

    ' Initialize button execution
    '***************************************************************************************************************
    Public Overrides Sub ButtonDefinition_OnExecute()
        ' Only execute if assemlby environment
        Dim currentEnvironment As Inventor.Environment = MidAddIn.UserInterfaceManager.ActiveEnvironment

        If LCase(currentEnvironment.InternalName) <> LCase("MidEnvironment") Then
            MessageBox.Show("This command works only for assembly environment", _
                            "MIDProject", _
                            MessageBoxButtons.OK, _
                            MessageBoxIcon.Error)
            Exit Sub
        End If


        '' Find circuit carrier
        If oServer.MidDataTypes.CircuitCarrier.Occurrence IsNot Nothing Then
            oOccurrence = oServer.MidDataTypes.CircuitCarrier.Occurrence
        Else
            System.Windows.Forms.MessageBox.Show("Could not find a circuit carrier.")
            Exit Sub
        End If

        ' Dim oAsmDoc As AssemblyDocument = MidAddIn.ActiveDocument()
        '   Dim oCompOccs As ComponentOccurrences = oAsmDoc.ComponentDefinition.Occurrences

        '   For Each oOcc As ComponentOccurrence In oCompOccs
        '       If oOcc.AttributeSets.NameIsUsed("circuitcarrier") Then
        '           oOccurrence = oOcc
        '           Exit For
        '       End If
        '   Next

        'Exit if no circuit carrier was found
        '   If oOccurrence Is Nothing Then
        '     
        '       Exit Sub
        '   End If

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

        CreateClientGraphics()

        ' Enable interaction
        EnableInteraction()

    End Sub

    'Stop Command
    '***************************************************************************************************************
    Public Overrides Sub StopCommand()

        ' Delete UCS
        'oUCS.Visible = False
        'oUCS.Delete()
        'oUCS = Nothing

        ' Delete client graphics
        'oClientGraphics = Nothing

        ' Delete data sets
        'oDataSets.Delete()
        'oDataSets = Nothing

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

    'Create client graphics
    '***************************************************************************************************************
    Private Sub CreateClientGraphics()

        ' Create client graphics
        Dim oInteractionGraphics As InteractionGraphics = oInteractionEvents.InteractionGraphics

        ' Create new client graphics
        Dim oClientGraphics As ClientGraphics = oInteractionGraphics.PreviewClientGraphics
        'Dim oClientGraphics As ClientGraphics = oAsmDoc.ComponentDefinition.ClientGraphicsCollection.Add(oServer.ClientId)
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
        'oOccNode.Visible = False
        ' Set render style (transparent asset)
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
        ' oDataSets = oAsmDoc.GraphicsDataSetsCollection.Add(oServer.ClientId)
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
        oColorSet.Add(178, 0, 255, 0)
        oLineGraphics.ColorSet = oColorSet
        oLineGraphics.BurnThrough = True
        oLineGraphics.DepthPriority = 4

        ' Update view
        MidAddIn.ActiveView.Update()

    End Sub

    Private Sub UpdateClientGraphics(ByRef coordinateSystem As Matrix)

        Dim oInteractionGraphics As InteractionGraphics = oInteractionEvents.InteractionGraphics
        Dim oClientGraphics As ClientGraphics = oInteractionGraphics.PreviewClientGraphics

        ' Set transformation
        'If oClientGraphics IsNot Nothing Then
        Dim oOccNode As GraphicsNode = oClientGraphics.Item(1)
        oOccNode.Transformation = coordinateSystem

        MidAddIn.ActiveView.Update()
        'End If

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



    ' OnEndSequence
    ' ***************************************************************************************************************
    Public Overrides Sub OnEndSequence(cancelled As Boolean, _
                                       coordinateSystem As Matrix, _
                                       context As NameValueMap, _
                                       ByRef handlingCode As HandlingCodeEnum)

        ' The user clicks apply or ok/exit
        If cancelled = False Then
            ' Apply transformation


            ' oOccurrence.Transformation = coordinateSystem
            oMatrix = coordinateSystem
            ExecuteCommand()
            'Else
            ' Reset transformation
            'oOccurrence.Transformation = oTransformation
        End If

        handlingCode = HandlingCodeEnum.kEventHandled

    End Sub

    ' Return position matrix
    ' ***************************************************************************************************************
    Public ReadOnly Property Matrix As Matrix
        Get
            Return oMatrix
        End Get
    End Property

    ' Execute command
    ' ***************************************************************************************************************
    Public Overrides Sub ExecuteCommand()

        MyBase.ExecuteCommand()

        StopCommand()

        Dim oMidMoveRequest As New MoveMidRequest(MidAddIn, oServer)

        MyBase.ExecuteChangeRequest(oMidMoveRequest, "MidAddIn:MoveMidRequest", MidAddIn.ActiveDocument)

    End Sub

    ' OnMove
    ' ***************************************************************************************************************
    Public Overrides Sub OnMove(selectedTriadSegment As TriadSegmentEnum, _
                                shiftKeys As ShiftStateEnum, _
                                coordinateSystem As Matrix, _
                                context As NameValueMap, _
                                ByRef handlingCode As HandlingCodeEnum)

        ' Set triad position to client graphics position
        UpdateClientGraphics(coordinateSystem)


    End Sub


    ' OnActivate
    ' ***************************************************************************************************************
    Public Overrides Sub OnActivate(context As NameValueMap, _
                                    ByRef handlingCode As HandlingCodeEnum)
        ' remark: degreesOfFreendom can only be set in the OnActivate methode
        ' won't work on other places!
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



'' Create user coordinate system
''*************************************************************************************************************************
'Private Function CreateUCS() As UserCoordinateSystem

'    Dim oCompDef As AssemblyComponentDefinition = MidAddIn.ActiveDocument.ComponentDefinition
'    Dim oUCSDef As UserCoordinateSystemDefinition = oCompDef.UserCoordinateSystems.CreateDefinition()
'    Dim oUCS As UserCoordinateSystem = oCompDef.UserCoordinateSystems.Add(oUCSDef)
'    oUCS.Name = "MoveUCS"
'    oUCS.Visible = False
'    Dim oTG As TransientGeometry = MidAddIn.TransientGeometry
'    oUCS.XAxis.SetSize(oTG.CreatePoint(4, 0, 0), oTG.CreatePoint(-4, 0, 0))
'    oUCS.YAxis.SetSize(oTG.CreatePoint(0, 4, 0), oTG.CreatePoint(0, -4, 0))
'    oUCS.ZAxis.SetSize(oTG.CreatePoint(0, 0, 4), oTG.CreatePoint(0, 0, -4))
'    Return oUCS

'End Function
