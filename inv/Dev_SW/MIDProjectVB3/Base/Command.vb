Imports System.Runtime.InteropServices
Imports Inventor
Imports MIDProjectVB3.Interaction1

Public MustInherit Class Command

    'Inventor application object
    Protected MidAddIn As Application
    'Button definition object 
    Protected oButtonDefinition As ButtonDefinition
    'Interaction object
    Protected oInteraction As Interaction1
    'InteractionEvents object
    Protected oInteractionEvents As InteractionEvents
    'SelectEvents object
    Protected oSelectEvents As SelectEvents
    'MouseEvents object
    Protected oMouseEvents As MouseEvents
    'TriadEvents object
    Protected oTriadEvents As TriadEvents
    'Command status
    Protected oKeyEvents As KeyboardEvents

    Protected bCommandIsRunning As Boolean

    ' Constructor
    Public Sub New(ByRef MidAddin As Inventor.Application)

        ' Register Addin
        Me.MidAddIn = MidAddin
        oButtonDefinition = Nothing
        oInteraction = Nothing
        oInteractionEvents = Nothing
        oMouseEvents = Nothing
        oSelectEvents = Nothing
        oTriadEvents = Nothing
        oKeyEvents = Nothing


        'If bCommandIsRunning <> True Then
        bCommandIsRunning = False
        'End If

    End Sub

    ' Return the button of the command
    '***************************************************************************************************************************
    Public ReadOnly Property ButtonDefinition() As Inventor.ButtonDefinition
        Get
            ButtonDefinition = oButtonDefinition
        End Get
    End Property


    ' Create Button for Command
    '***************************************************************************************************************************
    Public Sub CreateButton(ByVal displayName As String, _
                               ByVal internalName As String, _
                               ByVal commandType As CommandTypesEnum, _
                               Optional ByVal clientId As Object = Nothing, _
                               Optional ByVal description As String = "", _
                               Optional ByVal toolTip As String = "", _
                               Optional ByVal standardIcon As Object = Nothing, _
                               Optional ByVal largeIcon As Object = Nothing, _
                               Optional ByVal buttonDisplayType As ButtonDisplayEnum = ButtonDisplayEnum.kDisplayTextInLearningMode)

        oButtonDefinition = MidAddIn.CommandManager.ControlDefinitions.AddButtonDefinition(displayName, internalName, commandType, clientId, description, _
                                                                                            toolTip, standardIcon, largeIcon, buttonDisplayType)

        ' Connect the button to event sink
        AddHandler oButtonDefinition.OnExecute, AddressOf Me.oButtonDefinition_OnExecute

        ' Disable the button by default
        oButtonDefinition.Enabled() = False

        'Return oButtonDefinition

    End Sub

    ' Initialize the command
    '***************************************************************************************************************************
    Protected Overridable Sub oButtonDefinition_OnExecute()

        ' Security: If command was already started, stop it first
        If bCommandIsRunning Then

            StopCommand()

        End If

        'Start new command
        StartCommand()

    End Sub


    '#################################################################################
    '#overridables
    Public Overridable Sub initCommand()

    End Sub



    ' Enable interaction
    '*************************************************************************************************
    Public Overridable Sub EnableInteraction()
        'Enable interaction events
        oInteraction.EnableInteraction()
    End Sub
    '*************************************************************************************************

    Public Overridable Sub DisableInteraction()
        'Disable interaction events
        oInteraction.DisableInteraction()
    End Sub

    ' Start/Stop command
    '***************************************************************************************************************
    Public Overridable Sub StartCommand()
        ' Start interaction events
        StartInteraction()

        'Press the button
        oButtonDefinition.Pressed = True

        ' Set the command status to running
        bCommandIsRunning = True
    End Sub

    Public Overridable Sub StopCommand()
        Try
            UnsubscribeFromEvents()
            StopInteraction()

            ' Unpress the button
            oButtonDefinition.Pressed = False

            'Set the command status to not-running
            bCommandIsRunning = False

        Catch ex As Exception
            System.Windows.Forms.MessageBox.Show(ex.ToString())
        End Try
    End Sub
    '***************************************************************************************************************


    ' Start/Stop Interaction
    '*************************************************************************************************
    Public Sub StartInteraction()
        Try
            oInteraction = New Interaction1
            'Set the parent to get the call back when event is terminated,or interaction is completed
            oInteraction.SetParentCmd(Me)

            Dim buttonDefObjInternalName As String
            buttonDefObjInternalName = oButtonDefinition.InternalName ''oButtonDefinition.InternalName
            'Start interaction events
            oInteraction.StartInteraction(MidAddIn, buttonDefObjInternalName, oInteractionEvents)

        Catch ex As Exception
            System.Windows.Forms.MessageBox.Show(ex.ToString())
        End Try
    End Sub

    Public Sub StopInteraction()

        'Terminate interaction events
        oInteraction.StopInteraction()
        oInteraction = Nothing
        oInteractionEvents = Nothing
    End Sub
    '**************************************************************************************************

    ' Subscribe/UnSubscribe to/from specific/all event(s)
    '**************************************************************************************************
    Public Sub SubscribeToEvent(ByVal interactionType As InteractionTypeEnum)
        ' Subscribe to the specified event type (selection, mouse etc.)
        Dim eventType As Object = Nothing
        ' Set eventtype...
        eventType = oInteraction.SubscribeToEvent(interactionType)
        ' ...And save it for the command
        If Not (eventType Is Nothing) Then
            If TypeOf eventType Is SelectEvents Then
                oSelectEvents = eventType
            End If

            If TypeOf eventType Is MouseEvents Then
                oMouseEvents = eventType
            End If

            If TypeOf eventType Is TriadEvents Then
                oTriadEvents = eventType
            End If
        End If
    End Sub

    Public Sub UnsubscribeFromEvents()
        'Unsubscribe from all event objects (selection,mouse etc)
        oInteraction.UnsubscribeFromEvents()

        oSelectEvents = Nothing
        oMouseEvents = Nothing
        oTriadEvents = Nothing
        oKeyEvents = Nothing

    End Sub
    '**************************************************************************************************

    Public Sub ExecuteChangeRequest(ByVal changeRequest As ChangeRequest, ByVal oChangeDefinition As Object, ByVal oDoc As Inventor._Document)
        changeRequest.Execute(MidAddIn, oChangeDefinition, oDoc)
    End Sub


    '##################################################################################################
    ' Interaction Events
    '##################################################################################################

    Public Overridable Sub OnSelect(justSelectedEntities As ObjectsEnumerator, _
                                      selectionDevice As SelectionDeviceEnum, _
                                      modelPosition As Point, _
                                      viewPosition As Point2d, _
                                      view As View)
        ' empty header for derived class
    End Sub

    Public Overridable Sub OnPreSelect(ByRef preSelectEntity As Object, _
                            ByRef doHighlight As Boolean, _
                            ByRef morePreSelectEntities As ObjectCollection, _
                            selectionDevice As SelectionDeviceEnum, _
                            modelPosition As Point, _
                            viewPosition As Point2d, _
                            view As View)
        ' empty header for derived class
    End Sub

    Public Overridable Sub OnKeyPress(keyASCII As Long)
        ' empty header
    End Sub




    ' Triad Events
    '**************************************************************************************************
    Public Overridable Sub OnEndSequence(cancelled As Boolean, _
                                      coordinateSystem As Matrix, _
                                      context As NameValueMap, _
                                      ByRef handlingCode As HandlingCodeEnum)

        ' implemented in derived class


    End Sub

    Public Overridable Sub OnTerminate(cancelled As Boolean, _
                                        context As NameValueMap, _
                                        handlingCode As HandlingCodeEnum)

        ' implemented in derived class

    End Sub

    Public Overridable Sub OnMove(selectedTriadSegment As TriadSegmentEnum, _
                                  shiftKeys As ShiftStateEnum, _
                                  coordinateSystem As Matrix, _
                                  context As NameValueMap, _
                                  ByRef handlingCode As HandlingCodeEnum)

        ' implemented in derived class

    End Sub

    Public Overridable Sub OnMouseUp(button As MouseButtonEnum, _
                                     shiftKeys As ShiftStateEnum, _
                                     modelPosition As Point, _
                                     viewPosition As Point2d, _
                                     view As View)

        ' implemented in derived class
    End Sub

    Public Overridable Sub OnMouseDown(button As MouseButtonEnum, _
                    shiftKeys As ShiftStateEnum, _
                    modelPosition As Point, _
                    viewPosition As Point2d, _
                    view As View)

        ' implemented in derived class
    End Sub

    Public Overridable Sub OnMouseMove(button As MouseButtonEnum, _
                    shiftKeys As ShiftStateEnum, _
                    modelPosition As Point, _
                    viewPosition As Point2d, _
                    view As View)

        ' implemented in derived class

    End Sub

    Public Overridable Sub OnActivate(context As NameValueMap, _
                   ByRef handlingCode As HandlingCodeEnum)

        ' implemented in derived class
    End Sub

    Public Overridable Sub OnMoveTriadOnlyToggle(moveTriadOnly As Boolean, _
                                                 beforeOrAfter As EventTimingEnum, _
                                                 context As NameValueMap, _
                                                 ByRef handlingCode As HandlingCodeEnum)
        ' implemented in derived class
    End Sub

    Public Overridable Sub SubOnEndMove(selectedTriadSegment As TriadSegmentEnum, _
                                        shiftKeys As ShiftStateEnum, _
                                        coordinateSystem As Matrix, _
                                        context As NameValueMap, _
                                        ByRef handlingCode As HandlingCodeEnum)
        ' implemented in derived class
    End Sub

    Public Overridable Sub OnSegmentSelectionChange(selectedTriadSegment As TriadSegmentEnum, _
                                                    beforeOrAfter As EventTimingEnum, _
                                                    context As NameValueMap, _
                                                    ByRef handlingCode As HandlingCodeEnum)
        ' implemented in derived class
    End Sub

    Public Overridable Sub OnStartMove(selectedTriadSegment As TriadSegmentEnum, _
                                       shiftKeys As ShiftStateEnum, _
                                       coordinateSystem As Matrix, _
                                       context As NameValueMap, _
                                       ByRef handlingCode As HandlingCodeEnum)
        ' implemented in derived class
    End Sub

    Public Overridable Sub OnStartSequence(coordinateSystem As Matrix, _
                                           context As NameValueMap, _
                                           ByRef handlingCode As HandlingCodeEnum)
        ' implemented in derived class
    End Sub

    Public Overridable Sub OnEndMove(selectedTriadSegment As TriadSegmentEnum, _
                                     shiftKeys As ShiftStateEnum, _
                                     coordinateSystem As Matrix, _
                                     context As NameValueMap, _
                                     ByRef handlingCode As HandlingCodeEnum)
        ' implemented in derivded class
    End Sub

End Class



