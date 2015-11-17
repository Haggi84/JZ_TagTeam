Public Class Interaction2

    'Inventor application object
    Protected MidAddIn As Application

    Private m_rackFace As Face
    Private m_rackEdge As Edge

    Private oCompOcc As ComponentOccurrence

    Protected oInteractionEvents As InteractionEvents
    'SelectEvents object
    Protected oSelectEvents As SelectEvents
    Protected oTriadEvents As TriadEvents

    Private oMiniToolbar As MiniToolbar
    Private WithEvents oMtbCancelButton As MiniToolbarButton
    Private WithEvents oMtbApplyButton As MiniToolbarButton
    Private WithEvents oMtbExitButton As MiniToolbarButton

    Public Sub New(ByRef MidAddin As Inventor.Application)

        MyBase.New()
        'm_rackFaceCmdDlg = Nothing
        Me.MidAddIn = MidAddin
        m_rackFace = Nothing
        m_rackEdge = Nothing

        Me.CreateMiniToolbar()
        'm_previewClientGraphicsNode = Nothing
        'm_triangleStripGraphics = Nothing
        'm_graphicsCoordinateSet = Nothing
        'm_graphicsColorSet = Nothing
        'm_graphicsColorIndexSet = Nothing
    End Sub

    Public Sub init()
        'Make sure that the current environment is "Part Environment"
        'Dim currEnvironment As Inventor.Environment
        'currEnvironment = MidAddIn.UserInterfaceManager.ActiveEnvironment

        'If LCase$(currEnvironment.InternalName) <> LCase$("PMxPartEnvironment") Then
        '    System.Windows.Forms.MessageBox.Show("This command applies only to Part Environment")
        '    Exit Sub
        'End If

        'If commmand was already started,stop is first
        ' If bCommandIsRunning Then
        'StopCommand()
        'End If

        'Start new command
        'StartCommand()

        'Create the InteractionEvents object
        'If oInteractionEvents Is Nothing Then
        oInteractionEvents = MidAddIn.CommandManager.CreateInteractionEvents()
        'End If

        'Define that we want select events rather than mouse events
        oInteractionEvents.SelectionActive = True

        'Set the name for the interation events
        'oInteractionEvents.Name = interactionName
        'Connect the interaction event sink
        'ConnectInteractionEventsSink()

        'Set a reference to the select events


        oTriadEvents = oInteractionEvents.TriadEvents
        Dim oDoc As AssemblyDocument = MidAddIn.ActiveDocument()
        Dim oOcc As Object = oDoc.ComponentDefinition.Occurrences.Item(1)
        If oOcc IsNot Nothing Then
            'oTriadEvents.Reposition(TriadSegmentEnum.kAllSegments, oOcc)
        End If

        RemoveHandler oTriadEvents.OnEndSequence, AddressOf Me.TriadEvents_onSelect
        RemoveHandler oTriadEvents.OnTerminate, AddressOf Me.TriadEvents_OnTerminate
        oTriadEvents.Enabled = False
        'oTriadEvents.Repeat() = True
        oSelectEvents = oInteractionEvents.SelectEvents
        'oTriadEvents.Enabled() = True
        'Connect the select event sink
        oSelectEvents.AddSelectionFilter(SelectionFilterEnum.kAssemblyOccurrenceFilter)

        AddHandler oSelectEvents.OnSelect, AddressOf Me.SelectEvents_OnSelect
        AddHandler oSelectEvents.OnPreSelect, AddressOf Me.SelectEvents_OnPreSelect
        oInteractionEvents.Start() '--> fire onactivate

        'Dim DOF As Long = TriadSegmentEnum.kXAxisTranslationSegment Or _
        '   TriadSegmentEnum.kZAxisTranslationSegment Or _
        '   TriadSegmentEnum.kYAxisRotationSegment Or _
        '   TriadSegmentEnum.kOriginSegment

        'oTriadEvents.DegreesOfFreedom() = DOF

        oMiniToolbar.Visible = True
        'If oSelectEvents Is Nothing Then
        '    ''Set a reference to the select events
        '    oSelectEvents = oInteractionEvents.MouseEvents

        '    'Connect the select event sink
        '    AddHandler oSelectEvents.OnSelect, AddressOf Me.SelectEvents_OnSelect
        'End If

        'Specify bure through
        'oSelectEvents.PreSelectBurnThrough = True
        'eventType = oSelectEvents

    End Sub

    Private Sub TriadEvents_OnTerminate(Cancelled As Boolean, Context As NameValueMap, ByRef HandlingCode As HandlingCodeEnum)
        MsgBox("terminated")
        oInteractionEvents.Stop()
        'oInteractionEvents.Start()
        ' init()
    End Sub

    Private Sub TriadEvents_onSelect(Cancelled As Boolean, CoordinateSystem As Matrix, Context As NameValueMap, ByRef HandlingCode As HandlingCodeEnum)
        MsgBox("here")
        'oTriadEvents.Enabled = False

        'oSelectEvents = oInteractionEvents.SelectEvents
        'oInteractionEvents.Start()
        ' init()

    End Sub


    Private Sub SelectEvents_OnPreSelect(ByRef PreSelectEntity As Object, ByRef DoHighlight As Boolean, ByRef MorePreSelectEntities As ObjectCollection, SelectionDevice As SelectionDeviceEnum, ModelPosition As Point, ViewPosition As Point2d, View As View)

        MsgBox(PreSelectEntity.Name)
    End Sub

    Private Sub SelectEvents_OnSelect()
        MsgBox("selected")
        oSelectEvents.Enabled() = False
        'oTriadEvents = oInteractionEvents.TriadEvents
        oTriadEvents.Enabled() = True

        Dim DOF As Long = TriadSegmentEnum.kXAxisTranslationSegment Or _
          TriadSegmentEnum.kZAxisTranslationSegment Or _
          TriadSegmentEnum.kYAxisRotationSegment Or _
          TriadSegmentEnum.kOriginSegment

        oTriadEvents.DegreesOfFreedom() = DOF
    End Sub
    '#########################################################################################
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

        ' AddHandler oMtbExitButton.OnClick, AddressOf oMtbExitButton_OnClick

        ' Create Minitoolbar on the upper left of the window
        oMiniToolbar.Position = MidAddIn.TransientGeometry.CreatePoint2d(0, 0)
        oMiniToolbar.Visible = False

    End Sub

    '#############################################################
    ' EVENTS (Minitoolbar)
    '#############################################################

    Private Sub oMtbExitButton_OnClick() Handles oMtbExitButton.OnClick

        'oSelectEvents.ClearSelectionFilter()
        'oSelectEvents.ResetSelections()

        MsgBox("msg")
        'oMiniToolbar.Delete()
        'Me.DisableInteraction()
        'Me.UnsubscribeFromEvents()
        ' Me.

        oTriadEvents.Enabled = False
        'oInteractionEvents.Stop()
        oSelectEvents.Enabled = True
        'MyBase.SubscribeToEvent(Interaction1.InteractionTypeEnum.kTriad)
        'bStillSelecting = False

    End Sub

    Private Sub oMtbCancelButton_OnActivate() Handles oMtbCancelButton.OnClick

        'oCompOcc.Transformation() = oOccPosMatrix

        '' Go back to occurrence selection
        'oTriadEvents.Enabled = False
        'oSelectEvents.Enabled = True
        'oSelectEvents.ClearSelectionFilter()
        'oSelectEvents.AddSelectionFilter(SelectionFilterEnum.kAssemblyOccurrenceFilter)

        'oMiniToolbar.Visible() = False

    End Sub

    Private Sub oMtbApplyButton_OnActivate() Handles oMtbApplyButton.OnClick

        ' End selection process and delete toolbar after applying
        ' bStillSelecting = False
        ' oMiniToolbar.Delete()


    End Sub

   

  

    

End Class
