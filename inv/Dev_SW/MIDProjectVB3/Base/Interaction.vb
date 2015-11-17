'#####################################################
' Interaction class
'#####################################################

Imports System.Collections.Generic

Public Class Interaction1

    'Interaction Types collection
    Private oInteractionTypes As List(Of InteractionTypeEnum)
    'InteractionEvents object
    Private oInteractionEvents As InteractionEvents
    'SelectEvents object
    Private oSelectEvents As SelectEvents
    'MouseEvents object
    Private oMouseEvents As MouseEvents
    'TriadEvents object
    Private oTriadEvents As TriadEvents
    ' KeyboardEvents object
    Private oKeyEvents As KeyboardEvents

    'Parent Command object
    Private oParentCmd As Command

    ' create enum for all event types (except measure)
    Public Enum InteractionTypeEnum
        kSelection
        kMouse
        kKeyboard
        kTriad
    End Enum

    ' Constructor
    Public Sub New()

        oInteractionTypes = New List(Of InteractionTypeEnum)
        oInteractionEvents = Nothing
        oSelectEvents = Nothing
        oMouseEvents = Nothing
        oTriadEvents = Nothing

        oParentCmd = Nothing
    End Sub

    ' Set parent command from 
    Public Sub SetParentCmd(ByVal parentCmd As Command)
        'Store a copy of the parent command
        oParentCmd = parentCmd
    End Sub


    

    ' Enable interaction / reregister/resubscribe all event sinks
    '************************************************************************************************
    Public Sub EnableInteraction()

        Dim interactionType As InteractionTypeEnum
        ' go through the entire list and register the events in it
        For i As Integer = 0 To oInteractionTypes.Count - 1
            interactionType = oInteractionTypes(i)

            Select Case interactionType
                Case InteractionTypeEnum.kSelection
                    'Re-subscribe to selection events
                    If oSelectEvents Is Nothing Then

                        oSelectEvents = oInteractionEvents.SelectEvents
                        ConnectSelectEventsSink()

                        'Specify burn through 
                        oSelectEvents.PreSelectBurnThrough = True
                    End If

                Case InteractionTypeEnum.kMouse
                    'Re-subscribe to mouse events
                    If oMouseEvents Is Nothing Then

                        oMouseEvents = oInteractionEvents.MouseEvents
                        ConnectMouseEventsSink()
                    End If

                Case InteractionTypeEnum.kTriad
                    'Re-subscribe to triad events
                    If oTriadEvents Is Nothing Then

                        oTriadEvents = oInteractionEvents.TriadEvents
                        ConnectTriadEventsSink()
                    End If
                Case InteractionTypeEnum.kKeyboard

                    If oTriadEvents Is Nothing Then
                        oKeyEvents = oInteractionEvents.KeyboardEvents
                        ConnectKeyEventsSink()
                    End If
            End Select
        Next
    End Sub
    
    Public Sub DisableInteraction()
        'Disable subscribed to events
        Dim interactionType As InteractionTypeEnum
        Dim i As Integer
        ' go through list and remove events and handlers
        For i = 0 To oInteractionTypes.Count - 1
            interactionType = oInteractionTypes(i)

            Select Case interactionType
                Case InteractionTypeEnum.kSelection
                    'Un-subscribe and delete selection events
                    If Not (oSelectEvents Is Nothing) Then
                        DisConnectSelectEventsSink()
                        oSelectEvents = Nothing
                    End If

                Case InteractionTypeEnum.kMouse
                    'Un-subscribe and delete mouse events
                    If Not (oMouseEvents Is Nothing) Then
                        DisConnectMouseEventsSink()
                        oMouseEvents = Nothing
                    End If

                Case InteractionTypeEnum.kTriad
                    'Un-subscribe and delete triad events
                    If Not (oTriadEvents Is Nothing) Then
                        DisConnectTriadEventsSink()
                        oTriadEvents = Nothing
                    End If

                Case InteractionTypeEnum.kKeyboard

                    If Not (oKeyEvents Is Nothing) Then
                        DisConnectKeyEventsSink()
                        oKeyEvents = Nothing
                    End If
            End Select
        Next
    End Sub
    '************************************************************************************************

    ' Start Interaction --> Return interactionevents/ set all event references
    '************************************************************************************************
    Public Sub StartInteraction(ByRef application As Application, ByVal interactionName As String, ByRef interactionEvents As InteractionEvents)
        Try
            oInteractionEvents = application.CommandManager.CreateInteractionEvents()
            'Define that we want select events rather than mouse events
            oInteractionEvents.SelectionActive = True

            oInteractionEvents.Name = interactionName
            ConnectInteractionEventsSink()

            ' #1 Set a reference to the select events
            oSelectEvents = oInteractionEvents.SelectEvents
            ConnectSelectEventsSink()

            ' #2 Set a reference to the mouse events
            oMouseEvents = oInteractionEvents.MouseEvents
            ConnectMouseEventsSink()

            ' #3 Set a reference to the triad events
            
            oTriadEvents = oInteractionEvents.TriadEvents
           
            ConnectTriadEventsSink()

            ' #4 Set a reference to the keyboard events
            oKeyEvents = oInteractionEvents.KeyboardEvents
            ConnectKeyEventsSink()

            oInteractionEvents.Start() '--> fire onactivate

        Catch ex As Exception
            System.Windows.Forms.MessageBox.Show(ex.ToString())
        Finally
            interactionEvents = oInteractionEvents
        End Try
    End Sub

    Public Sub StopInteraction()
        'Disconnect interaction events sink
        If Not (oInteractionEvents Is Nothing) Then

            DisConnectInteractionEventsSink()
            oInteractionEvents.Stop()
            oInteractionEvents = Nothing
        End If
    End Sub
    '************************************************************************************************


    ' Subscribe to special event --> return the requested event
    '************************************************************************************************
    Public Function SubscribeToEvent(ByVal interactionType As InteractionTypeEnum) As Object
       
        ' Exit sub if interaction type is already subscribed
        For i As Integer = 0 To oInteractionTypes.Count - 1
            If oInteractionTypes.Item(i) = interactionType Then
                Return Nothing
            End If
        Next

        ' Add the event to the interaction type list
        oInteractionTypes.Add(interactionType)

        Select Case interactionType
            Case InteractionTypeEnum.kSelection
                If oSelectEvents Is Nothing Then

                    oSelectEvents = oInteractionEvents.MouseEvents
                    ConnectSelectEventsSink()
                End If

                'Specify bure through
                oSelectEvents.PreSelectBurnThrough = True
                Return oSelectEvents

            Case InteractionTypeEnum.kMouse
                If oMouseEvents Is Nothing Then

                    oMouseEvents = oInteractionEvents.MouseEvents
                    ConnectMouseEventsSink()
                End If
                Return oMouseEvents

            Case InteractionTypeEnum.kTriad
                If oTriadEvents Is Nothing Then
                   
                    oTriadEvents = oInteractionEvents.TriadEvents
                    ConnectTriadEventsSink()
                End If
                Return oTriadEvents

            Case InteractionTypeEnum.kKeyboard
                If oKeyEvents Is Nothing Then

                    oKeyEvents = oInteractionEvents.KeyboardEvents
                    ConnectKeyEventsSink()
                End If
                Return oKeyEvents

        End Select
        ' Default
        Return Nothing
    End Function

    Public Sub UnsubscribeFromEvents()
        Dim i As Integer
        Dim interactionType As InteractionTypeEnum

        ' it would be esaier to set each value to nothing but nicer this way
        For i = 0 To oInteractionTypes.Count - 1
            interactionType = oInteractionTypes(i)
            Select Case interactionType
                Case InteractionTypeEnum.kSelection
                    'Disconnect selection events sink
                    If Not (oSelectEvents Is Nothing) Then
                        DisConnectSelectEventsSink()
                        oSelectEvents = Nothing
                    End If

                Case InteractionTypeEnum.kMouse
                    'Disconnect mouse events sink
                    If Not (oMouseEvents Is Nothing) Then
                        DisConnectMouseEventsSink()
                        oMouseEvents = Nothing
                    End If

                Case InteractionTypeEnum.kTriad
                    'Disconnect triad events sink
                    If Not (oTriadEvents Is Nothing) Then
                        DisConnectTriadEventsSink()
                        oTriadEvents = Nothing
                    End If

                Case InteractionTypeEnum.kKeyboard
                    'Disconnect keyboard event sink
                    DisConnectKeyEventsSink()
                    oKeyEvents = Nothing

            End Select

        Next
        ' Clear list with subscribed events
        oInteractionTypes.Clear()

    End Sub
    '************************************************************************************************

    '##########################################
    ' EVENT SINKS
    '#########################################
    Private Sub ConnectKeyEventsSink()
        AddHandler oKeyEvents.OnKeyPress, AddressOf Me.oKeyEvents_OnKeyPress
    End Sub

    Private Sub DisConnectKeyEventsSink()
        RemoveHandler oKeyEvents.OnKeyPress, AddressOf Me.oKeyEvents_OnKeyPress
    End Sub

    Private Sub ConnectInteractionEventsSink()
        AddHandler oInteractionEvents.OnTerminate, AddressOf Me.InteractionEvents_OnTerminate
        'AddHandler oInteractionEvents.OnHelp, AddressOf Me.InteractionEvents_OnHelp
    End Sub

    Private Sub DisConnectInteractionEventsSink()
        RemoveHandler oInteractionEvents.OnTerminate, AddressOf Me.InteractionEvents_OnTerminate
        'RemoveHandler oInteractionEvents.OnHelp, AddressOf Me.InteractionEvents_OnHelp
    End Sub

    Private Sub ConnectMouseEventsSink()
        AddHandler oMouseEvents.OnMouseUp, AddressOf Me.MouseEvents_OnMouseUp
        AddHandler oMouseEvents.OnMouseDown, AddressOf Me.MouseEvents_OnMouseDown
        'AddHandler oMouseEvents.OnMouseClick, AddressOf Me.MouseEvents_OnMouseClick
        'AddHandler oMouseEvents.OnMouseDoubleClick, AddressOf Me.MouseEvents_OnMouseDoubleClick
        AddHandler oMouseEvents.OnMouseMove, AddressOf Me.MouseEvents_OnMouseMove
        'AddHandler oMouseEvents.OnMouseLeave, AddressOf Me.MouseEvents_OnMouseLeave
    End Sub

    Private Sub DisConnectMouseEventsSink()
        'RemoveHandler oMouseEvents.OnMouseUp, AddressOf Me.MouseEvents_OnMouseUp
        'RemoveHandler oMouseEvents.OnMouseDown, AddressOf Me.MouseEvents_OnMouseDown
        'RemoveHandler oMouseEvents.OnMouseClick, AddressOf Me.MouseEvents_OnMouseClick
        'RemoveHandler oMouseEvents.OnMouseDoubleClick, AddressOf Me.MouseEvents_OnMouseDoubleClick
        'RemoveHandler oMouseEvents.OnMouseMove, AddressOf Me.MouseEvents_OnMouseMove
        'RemoveHandler oMouseEvents.OnMouseLeave, AddressOf Me.MouseEvents_OnMouseLeave
    End Sub

    Private Sub ConnectSelectEventsSink()
        AddHandler oSelectEvents.OnPreSelect, AddressOf Me.SelectEvents_OnPreSelect
        'AddHandler oSelectEvents.OnPreSelectMouseMove, AddressOf Me.SelectEvents_OnPreSelectMouseMove
        'AddHandler oSelectEvents.OnStopPreSelect, AddressOf Me.SelectEvents_OnStopPreSelect
        AddHandler oSelectEvents.OnSelect, AddressOf Me.SelectEvents_OnSelect
        'AddHandler oSelectEvents.OnUnSelect, AddressOf Me.SelectEvents_OnUnSelect
    End Sub

    Private Sub DisConnectSelectEventsSink()
        RemoveHandler oSelectEvents.OnPreSelect, AddressOf Me.SelectEvents_OnPreSelect
        'RemoveHandler oSelectEvents.OnPreSelectMouseMove, AddressOf Me.SelectEvents_OnPreSelectMouseMove
        'RemoveHandler oSelectEvents.OnStopPreSelect, AddressOf Me.SelectEvents_OnStopPreSelect
        RemoveHandler oSelectEvents.OnSelect, AddressOf Me.SelectEvents_OnSelect
        'RemoveHandler oSelectEvents.OnUnSelect, AddressOf Me.SelectEvents_OnUnSelect
    End Sub

    Private Sub ConnectTriadEventsSink()
        AddHandler oTriadEvents.OnActivate, AddressOf Me.TriadEvents_OnActivate
        AddHandler oTriadEvents.OnEndMove, AddressOf Me.TriadEvents_OnEndMove
        AddHandler oTriadEvents.OnEndSequence, AddressOf Me.TriadEvents_OnEndSequence
        AddHandler oTriadEvents.OnMove, AddressOf Me.TriadEvents_OnMove
        AddHandler oTriadEvents.OnMoveTriadOnlyToggle, AddressOf Me.TriadEvents_OnMoveTriadOnlyToggle
        AddHandler oTriadEvents.OnSegmentSelectionChange, AddressOf Me.TriadEvents_OnSegmentSelectionChange
        AddHandler oTriadEvents.OnStartMove, AddressOf Me.TriadEvents_OnStartMove
        AddHandler oTriadEvents.OnStartSequence, AddressOf Me.TriadEvents_OnStartSequence
        AddHandler oTriadEvents.OnTerminate, AddressOf Me.TriadEvents_OnTerminate
    End Sub

    Private Sub DisConnectTriadEventsSink()
        'RemoveHandler oTriadEvents.OnActivate, AddressOf Me.TriadEvents_OnActivate
        'RemoveHandler oTriadEvents.OnEndMove, AddressOf Me.TriadEvents_OnEndMove
        RemoveHandler oTriadEvents.OnEndSequence, AddressOf Me.TriadEvents_OnEndSequence
        'RemoveHandler oTriadEvents.OnMove, AddressOf Me.TriadEvents_OnMove
        'RemoveHandler oTriadEvents.OnMoveTriadOnlyToggle, AddressOf Me.TriadEvents_OnMoveTriadOnlyToggle
        'RemoveHandler oTriadEvents.OnSegmentSelectionChange, AddressOf Me.TriadEvents_OnSegmentSelectionChange
        'RemoveHandler oTriadEvents.OnStartMove, AddressOf Me.TriadEvents_OnStartMove
        'RemoveHandler oTriadEvents.OnStartSequence, AddressOf Me.TriadEvents_OnStartSequence
        'RemoveHandler oTriadEvents.OnTerminate, AddressOf Me.TriadEvents_OnTerminate
    End Sub

    ' SelectEvents: OnTerminate
    Public Sub InteractionEvents_OnTerminate()
        oParentCmd.StopCommand()
    End Sub

    ' SelectEvents: OnSelect
    '*****************************************************************************************************************
    Private Sub SelectEvents_OnSelect(justSelectedEntities As ObjectsEnumerator, _
                                      selectionDevice As SelectionDeviceEnum, _
                                      modelPosition As Point, _
                                      viewPosition As Point2d, _
                                      view As View)

        oParentCmd.OnSelect(justSelectedEntities, selectionDevice, modelPosition, viewPosition, view)

    End Sub
    '*****************************************************************************************************************

    Private Sub SelectEvents_OnPreSelect(ByRef preSelectEntity As Object, _
                                         ByRef doHighlight As Boolean, _
                                         ByRef morePreSelectEntities As ObjectCollection, _
                                         selectionDevice As SelectionDeviceEnum, _
                                         modelPosition As Point, _
                                         viewPosition As Point2d, _
                                         view As View)
        oParentCmd.OnPreSelect(preSelectEntity, doHighlight, morePreSelectEntities, selectionDevice, modelPosition, viewPosition, view)
    End Sub

    Private Sub oKeyEvents_OnKeyPress(keyASCII As Long)
        oParentCmd.OnKeyPress(keyASCII)
    End Sub

    Private Sub TriadEvents_OnEndSequence(cancelled As Boolean, _
                                         coordinateSystem As Matrix, _
                                    context As NameValueMap, _
                                            ByRef handlingCode As HandlingCodeEnum)

        oParentCmd.OnEndSequence(cancelled, coordinateSystem, context, handlingCode)

    End Sub

    ' SelectEvents: OnSelect
    '*****************************************************************************************************************
    Private Sub TriadEvents_OnTerminate(cancelled As Boolean, _
                                        context As NameValueMap, _
                                        ByRef handlingCode As HandlingCodeEnum)

        oParentCmd.OnTerminate(cancelled, context, handlingCode)

    End Sub

    Private Sub TriadEvents_OnMove(selectedTriadSegment As TriadSegmentEnum, _
                                   shiftKeys As ShiftStateEnum, _
                                   coordinateSystem As Matrix, _
                                   context As NameValueMap, _
                                   ByRef handlingCode As HandlingCodeEnum)

        oParentCmd.OnMove(selectedTriadSegment, shiftKeys, coordinateSystem, context, handlingCode)

    End Sub

    Private Sub MouseEvents_OnMouseUp(button As MouseButtonEnum, _
                                      shiftKeys As ShiftStateEnum, _
                                      modelPosition As Point, _
                                      viewPosition As Point2d, _
                                      view As View)


        oParentCmd.OnMouseUp(button, shiftKeys, modelPosition, viewPosition, view)

        'Throw New NotImplementedException
    End Sub

    Private Sub MouseEvents_OnMouseDown(button As MouseButtonEnum, _
                                        shiftKeys As ShiftStateEnum, _
                                        modelPosition As Point, _
                                        viewPosition As Point2d, _
                                        view As View)

        oParentCmd.OnMouseDown(button, shiftKeys, modelPosition, viewPosition, view)
    End Sub

    Private Sub MouseEvents_OnMouseClick()
        Throw New NotImplementedException
    End Sub

    Private Sub MouseEvents_OnMouseDoubleClick()
        Throw New NotImplementedException
    End Sub

   

    Private Sub MouseEvents_OnMouseMove(button As MouseButtonEnum, _
                                         shiftKeys As ShiftStateEnum, _
                                         modelPosition As Point, _
                                         viewPosition As Point2d, _
                                         view As View)

        oParentCmd.OnMouseMove(button, shiftKeys, modelPosition, viewPosition, view)

    End Sub

    Private Sub TriadEvents_OnActivate(context As NameValueMap, _
                                       ByRef handlingCode As HandlingCodeEnum)

        oParentCmd.OnActivate(context, handlingCode)
    End Sub

    Private Sub TriadEvents_OnEndMove(selectedTriadSegment As TriadSegmentEnum, _
                                      shiftKeys As ShiftStateEnum, _
                                      coordinateSystem As Matrix, _
                                      context As NameValueMap, _
                                      ByRef handlingCode As HandlingCodeEnum)
        oParentCmd.OnEndMove(selectedTriadSegment, shiftKeys, coordinateSystem, context, handlingCode)

    End Sub

    Private Sub TriadEvents_OnMoveTriadOnlyToggle(moveTriadOnly As Boolean, _
                                                  beforeOrAfter As EventTimingEnum, _
                                                  context As NameValueMap, _
                                                  ByRef handlingCode As HandlingCodeEnum)

        oParentCmd.OnMoveTriadOnlyToggle(moveTriadOnly, beforeOrAfter, context, handlingCode)
    End Sub

    Private Sub TriadEvents_OnSegmentSelectionChange(selectedTriadSegment As TriadSegmentEnum, _
                                                     beforeOrAfter As EventTimingEnum, _
                                                     context As NameValueMap, _
                                                     ByRef handlingCode As HandlingCodeEnum)
        oParentCmd.OnSegmentSelectionChange(selectedTriadSegment, beforeOrAfter, context, handlingCode)
    End Sub

    Private Sub TriadEvents_OnStartMove(selectedTriadSegment As TriadSegmentEnum, _
                                        shiftKeys As ShiftStateEnum, _
                                        coordinateSystem As Matrix, _
                                        context As NameValueMap, _
                                        ByRef handlingCode As HandlingCodeEnum)
        oParentCmd.OnStartMove(selectedTriadSegment, shiftKeys, coordinateSystem, context, handlingCode)

    End Sub

    Private Sub TriadEvents_OnStartSequence(coordinateSystem As Matrix, _
                                            context As NameValueMap, _
                                            ByRef handlingCode As HandlingCodeEnum)
        oParentCmd.OnStartSequence(coordinateSystem, context, handlingCode)
    End Sub




End Class
