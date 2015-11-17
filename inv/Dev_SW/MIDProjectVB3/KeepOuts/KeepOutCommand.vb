Option Strict Off
Option Explicit On

Imports Inventor
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports System.Xml

'############################################
' Keep Out Command
'############################################

Public Class KeepOutCommand
    Inherits Command

    Private oKeepOutDlg As KeepOutCmdDlg

    Private oMiniToolbar As Inventor.MiniToolbar
    Private oMtbButtonExit As MiniToolbarButton
    Private oMtbAddFaceToKeepOuts As MiniToolbarButton
    Private oMtbRmvFaceFromKeepOuts As MiniToolbarButton
    Private oMtbButtonReset As MiniToolbarButton


    Private offset As Integer = 60

    Private oServer As MidAddInServer

    Private oKeepOutFaces As List(Of Face)
    Private oPreviewNode As GraphicsNode
    Private oFaceHighSet As HighlightSet

    ' Constructor
    '***************************************************************************************************************
    Public Sub New(MidAddIn As Inventor.Application, _
                   server As MidAddInServer)
        ' call base class
        MyBase.New(MidAddIn)

        Me.oServer = server '###later in base class

        oKeepOutFaces = Nothing
        oKeepOutDlg = Nothing
        oMiniToolbar = Nothing

    End Sub

    ' Initialize button execution
    '***************************************************************************************************************
    Public Overrides Sub ButtonDefinition_OnExecute()
        ' Only execute if assemlby environment
        Dim currentEnvironment As Inventor.Environment = MidAddIn.UserInterfaceManager.ActiveEnvironment



        If LCase(currentEnvironment.InternalName) <> LCase("OPTAVER") Then
            MsgBox(currentEnvironment.InternalName)

            MessageBox.Show("This command works only for assembly environment", "MIDProject", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' Check if there is a circuitcarrier (new version)
        Dim oAttribMgr As AttributeManager = MidAddIn.ActiveDocument.AttributeManager
        Dim AttribSetsEnum As AttributeSetsEnumerator = oAttribMgr.FindAttributeSets("CircuitCarrier")
        If AttribSetsEnum.Count = 0 Then
            MessageBox.Show("Could not find a mid circuit carrier", "MIDProject", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' Check if there is a CircuitCarrier (old version)
        'If oServer.MidDataTypes.CircuitCarrier Is Nothing Then
        '    MessageBox.Show("Could not find a mid circuit carrier", "MIDProject", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    Exit Sub
        'End If

        ' Stop command if there is an other instance running
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

        ' Selection cursor
        oInteractionEvents.SetCursor(CursorTypeEnum.kCursorBuiltInSelectArrow)

        ' Subscribe to selection event
        MyBase.SubscribeToEvent(Interaction.InteractionTypeEnum.kSelection)

        ' Obtain KeepOuts from circuit carrier
        Dim oKeepOuts As KeepOuts = oServer.MidDataTypes.CircuitCarrier.KeepOuts

        ' Receive all KeepOutfaces
        'Dim oAttributeMgr As AttributeManager = MidAddIn.ActiveDocument.AttributeManager
        'Dim AttribEnum As AttributeSetsEnumerator = oAttributeMgr.FindAttributeSets("KeepOut")
        'Dim oObjColl As ObjectCollection = oAttributeMgr.FindObjects("KeepOut")

        '' Copy keepOut faces

        'For i As Integer = 1 To AttribEnum.Count
        '    If TypeOf AttribEnum(i) Is Inventor.Face Then
        '        oKeepOutFaces.Add(AttribEnum(i))
        '    End If
        'Next

        ' Create local face list
        oKeepOutFaces = New List(Of Face)

        ' Create a highlight set to paint selected faces
        oFaceHighSet = MidAddIn.ActiveDocument.CreateHighlightSet()
        oFaceHighSet.Color = MidAddIn.TransientObjects.CreateColor(255, 161, 0) ' orange

        For Each oKeepOut As KeepOut In oKeepOuts
            oKeepOutFaces.Add(oKeepOut.Face)
            oFaceHighSet.AddItem(oKeepOut.Face)
        Next

        ' Create new form dialog
        oKeepOutDlg = New KeepOutCmdDlg(MidAddIn, Me)

        If oKeepOutDlg IsNot Nothing Then
            oKeepOutDlg.TopMost() = True
            oKeepOutDlg.ShowInTaskbar() = True
            oKeepOutDlg.StartPosition = FormStartPosition.Manual
            Dim oView As Inventor.View = MidAddIn.ActiveView()
            oKeepOutDlg.Location = New System.Drawing.Point(oView.Left + 20, oView.Top + 20)
            oKeepOutDlg.Show()
        End If

        ' Create mini toolbar
        CreateMiniToolbar()

        ' Copy keepOut faces
        oKeepOutFaces = New List(Of Face)
        For Each oKeepOut As KeepOut In oKeepOuts
            oKeepOutFaces.Add(oKeepOut.Face)
        Next

        ' Create client graphics copy of circuit carrier
        CreatePreviewGraphics(oServer.MidDataTypes.CircuitCarrier.Occurrence.SurfaceBodies.Item(1))

        ' Enable command specific functions
        EnableInteraction()

    End Sub

    ' Stop command
    '***************************************************************************************************************
    Public Overrides Sub StopCommand()

        ' Destroy the command dialog
        oKeepOutDlg.Hide()
        oKeepOutDlg.Dispose()
        oKeepOutDlg = Nothing
        'End If
        ' Remove the mini toolbar
        oMiniToolbar.Delete()

        ' Delete the preview graphics
        oPreviewNode.Delete()
        oPreviewNode = Nothing

        'Delete the highlight set
        oFaceHighSet.Delete()
        oFaceHighSet = Nothing

        ' Disconnect events sink
        MyBase.StopCommand()

    End Sub

    ' Enable interaction
    '***************************************************************************************************************
    Public Overrides Sub EnableInteraction()

        ' Resubscribe all event handlers for the subscribed events (after disableInteraction)
        MyBase.EnableInteraction()

        oInteractionEvents.SelectionActive = True
        oInteractionEvents.InteractionDisabled = False

        oInteractionEvents.StatusBarText = "Select a face"

        ' Only faces are selectable
        oSelectEvents.AddSelectionFilter(SelectionFilterEnum.kPartFaceFilter)

        ' Make the minitoolbar visible
        oMiniToolbar.Visible = True

    End Sub

    'Disable Interaction
    '***************************************************************************************************************
    Public Overrides Sub DisableInteraction()

        MyBase.DisableInteraction()

        'Set the default cursor to notify that selection is not active
        oInteractionEvents.SetCursor(CursorTypeEnum.kCursorBuiltInArrow)

        'Clean up status text
        oInteractionEvents.StatusBarText = String.Empty

        ' Make the minitoolbar invisble
        oMiniToolbar.Visible = False

    End Sub

    ' Select: Change mini toolbar's position
    '***************************************************************************************************************
    Public Overrides Sub OnSelect(ByVal JustSelectedEntities As Inventor.ObjectsEnumerator, _
                                  ByVal SelectionDevice As Inventor.SelectionDeviceEnum, _
                                  ByVal ModelPosition As Inventor.Point, _
                                  ByVal ViewPosition As Inventor.Point2d, _
                                  ByVal View As Inventor.View)

        ' set the minitoolbar's position to the mouse position after face selection

        Dim oTranslationVec As Inventor.Vector2d = MidAddIn.TransientGeometry.CreateVector2d(0.0, -40.0)
        ViewPosition.TranslateBy(oTranslationVec)
        oMiniToolbar.Position = ViewPosition '+++valid view position?

    End Sub

    ' PreSelect face: Decider
    '***************************************************************************************************************
    Public Overrides Sub OnPreSelect(ByRef preSelectEntity As Object, _
                                     ByRef doHighlight As Boolean, _
                                     ByRef morePreSelectEntities As ObjectCollection, _
                                      selectionDevice As SelectionDeviceEnum, _
                                      modelPosition As Point, _
                                      viewPosition As Point2d, _
                                      view As Inventor.View)

        ' filter: over mid (--> faces), over parts (--> nothing)
        If TypeOf preSelectEntity Is FaceProxy Then

            'If preSelectEntity.Parent.Parent.AttributeSets.NameIsUsed("circuitcarrier") Then
            If oServer.MidDataTypes.CircuitCarrier.IsCircuitCarrier(preSelectEntity) Then '###eventuell reverse vergleich!!!
                oSelectEvents.ClearSelectionFilter()
                oSelectEvents.AddSelectionFilter(SelectionFilterEnum.kPartFaceFilter)
            Else
                doHighlight = False
            End If

        End If

    End Sub

    ' Create new occurrence client graphics
    '***************************************************************************************************************
    Private Sub CreatePreviewGraphics(ByVal surfaceBody As SurfaceBody)

        Dim oInteractionGraphics As InteractionGraphics = oInteractionEvents.InteractionGraphics
        Dim oClientGraphics As ClientGraphics = oInteractionGraphics.PreviewClientGraphics
        oPreviewNode = oClientGraphics.AddNode(2)

        Dim oSurfaceBody As SurfaceBody = MidAddIn.TransientBRep.Copy(surfaceBody)
        ' #2 create surface graphic
        Dim oSurfaceGraphics As SurfaceGraphics = oPreviewNode.AddSurfaceGraphics(oSurfaceBody)
        oSurfaceGraphics.DepthPriority = 5
        Dim oStyle As RenderStyle = oServer.MidDataTypes.CircuitCarrier.Occurrence.GetRenderStyle(StyleSourceTypeEnum.kOverrideRenderStyle)
        oPreviewNode.RenderStyle = oStyle

        Dim oOccMatrix As Matrix = oServer.MidDataTypes.CircuitCarrier.Occurrence.Transformation
        Dim oNodeMatrix As Matrix = oPreviewNode.Transformation
        oNodeMatrix.TransformBy(oOccMatrix)
        oPreviewNode.Transformation = oNodeMatrix

        MidAddIn.ActiveView.Update()

    End Sub


    ' Remove faces from KeepOuts
    '***************************************************************************************************************
    Public Sub RmvFaceFromKeepOuts()

        ' Save the selected entities first and delete the selection afterwards (otherwise highlightset won't work)
        Dim oSelectedEnts As Inventor.ObjectsEnumerator = oSelectEvents.SelectedEntities
        oSelectEvents.ResetSelections()

        ' Remove faces from keepOuts
        If oSelectedEnts.Count > 0 Then 'And oKeepOuts.Count > 0 Then
            'DeleteFaceGraphics()
            ' Remove faces from KeepOuts
            For i As Long = 1 To oSelectedEnts.Count

                If TypeOf (oSelectedEnts.Item(i)) Is FaceProxy Then ' type check is crucial because you could accidently select a browsernode or sth else --> error

                    If oKeepOutFaces.Contains(oSelectedEnts.Item(i)) Then

                        oKeepOutFaces.Remove(oSelectedEnts(i))
                        'oFaceHighSet.Remove(oSelectedEnts.Item(1))
                    End If
                End If
            Next

        End If


        oFaceHighSet.Clear()
        For Each oFace As Face In oKeepOutFaces
            oFaceHighSet.AddItem(oFace)
            'CreateFaceGraphics(oFace)
        Next

        ' Remove reference
        oSelectedEnts = Nothing

        ' Show number of keepouts in the dialog
        oKeepOutDlg.updateDlg()

    End Sub

    ' Add faces to KeepOuts
    '***************************************************************************************************************
    Public Sub AddFaceToKeepOuts()

        ' Save the selected entities first and delete the selection afterwards (otherwise highlightset won't work)
        Dim oSelectedEnts As Inventor.ObjectsEnumerator = oSelectEvents.SelectedEntities
        oSelectEvents.ResetSelections()

        ' Add faces to KeepOuts
        If oSelectedEnts.Count > 0 Then

            For i As Integer = 1 To oSelectedEnts.Count

                If TypeOf (oSelectedEnts.Item(i)) Is FaceProxy Then ' type is faceproxy because the face is selected in part context

                    If Not oKeepOutFaces.Contains(oSelectedEnts.Item(i)) Then

                        ' add to the keepOut list and paint the face via highset
                        oKeepOutFaces.Add(oSelectedEnts.Item(i))
                        oFaceHighSet.AddItem(oSelectedEnts.Item(i))

                    End If
                End If
            Next
        End If

        ' Remove reference
        oSelectedEnts = Nothing

        ' Show number of keepouts in the dialog
        oKeepOutDlg.updateDlg()

    End Sub

    ' Remove all keepOut from keepOuts
    '***************************************************************************************************************
    Public Sub ClearKeepOuts()

        ' Clear entire list
        oKeepOutFaces.Clear()
        oFaceHighSet.Clear()
        oKeepOutDlg.updateDlg()
        oSelectEvents.ResetSelections()

    End Sub


    ' Mini Toolbar
    '***************************************************************************************************************
    Public Sub CreateMiniToolbar()

        oMiniToolbar = MidAddIn.CommandManager.CreateMiniToolbar()

        ' Make default buttons invisible by default
        oMiniToolbar.ShowOK = False
        oMiniToolbar.ShowApply = False
        oMiniToolbar.ShowCancel = False

        Dim oControls As MiniToolbarControls = oMiniToolbar.Controls
        oControls.Item("MTB_Options").Visible = False

        Dim oDescriptionLabel As MiniToolbarControl = oControls.AddLabel("Description", "Select Keep-out Faces", "Add and Remove Keep-out Surfaces")
        oControls.AddNewLine()

        ' Button #1
        Dim mtbAddPicture As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.mtbAddSmall1)
        Dim mtbExitPicture As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.mtbExitSmall1)
        Dim mtbRmvPicture As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.mtbRmvSmall1)
        ' remark: use only 16x16px Images for StandardIcon

        ' Dim MIDButtonTwo As ButtonDefinition = MidAddIn.CommandManager.ControlDefinitions.Item("PartCoilCmd")

        ' Define the first center position
        oMtbAddFaceToKeepOuts = oControls.AddButton("AddFaceInternal", "", "Add Faces to Keep-Outs", mtbAddPicture, mtbAddPicture)
        oMtbRmvFaceFromKeepOuts = oControls.AddButton("RmvFaceInternal", "", "Remove Faces from Keep-Outs", mtbRmvPicture, mtbRmvPicture)
        oMtbButtonReset = oControls.AddButton("ResetSelectionInternal", "", "Reset selection")

        oControls.AddNewLine()

        'oMtbButtonExit = oControls.AddButton("ExitFaceInternal", "Ok", "Exit Keep-Out Selection", mtbExitPicture, mtbExitPicture)

        AddHandler oMtbAddFaceToKeepOuts.OnClick, AddressOf Me.AddFaceToKeepOuts
        AddHandler oMtbRmvFaceFromKeepOuts.OnClick, AddressOf Me.RmvFaceFromKeepOuts
        AddHandler oMtbButtonReset.OnClick, AddressOf Me.ClearKeepOuts
        'AddHandler oMtbButtonExit.OnClick, AddressOf Me.oAddFaceToKeepOuts

        Dim oPosition As Inventor.Point2d
        If oKeepOutDlg IsNot Nothing Then
            oPosition = MidAddIn.TransientGeometry.CreatePoint2d(oKeepOutDlg.Location.X + oKeepOutDlg.Size.Width + offset, offset)
        Else
            oPosition = MidAddIn.TransientGeometry.CreatePoint2d(0, 0)
        End If

        oMiniToolbar.Position = oPosition
        oMiniToolbar.Visible = False

    End Sub

    ' Execute command
    '***************************************************************************************************************
    Public Overrides Sub ExecuteCommand()

        MyBase.ExecuteCommand() 'empty
        StopCommand()

        Dim oKeepOutRequest As New KeepOutRequest(oKeepOutFaces, MidAddIn, oServer)
        MyBase.ExecuteChangeRequest(oKeepOutRequest, "MidAddIn:KeepOutReqeust", MidAddIn.ActiveDocument)

        ' Delete face list
        oKeepOutFaces.Clear()
        oKeepOutFaces = Nothing
    End Sub


    ' Properties
    '***************************************************************************************************************
    Public ReadOnly Property KeepOutFaces As List(Of Face)
        Get
            Return oKeepOutFaces
        End Get
    End Property

End Class







'Public Class testclass

'    Public b As Integer
'    Private App As Inventor.Application
'    Private WithEvents oInteractionEvents As InteractionEvents
'    Private WithEvents oSelectEvents As SelectEvents

'    Public Sub New(ByVal a As Integer, MidAddIn As Inventor.Application)
'        MyBase.New()

'        Me.App = MidAddIn

'        Me.b = a
'        oInteractionEvents = MidAddIn.CommandManager.CreateInteractionEvents
'        oSelectEvents = MidAddIn.SelectEvents
'    End Sub

'    Public Function GetValue() As Object

'        Return b
'    End Function

'End Class



'Public Property BrowserNode As BrowserNode
'    Get
'        Return oKeepOutsNode
'    End Get
'    Set(value As BrowserNode)
'        oKeepOutsNode = value
'    End Set
'End Property


'Private Sub TransactionEvents_OnUndo(TransactionObject As Transaction, _
'    Context As NameValueMap, _
'    BeforeOrAfter As EventTimingEnum, _
'    ByRef HandlingCode As HandlingCodeEnum) Handles oTransactionEvents.OnUndo

'    MsgBox(TransactionObject.DisplayName)

'    If TransactionObject.DisplayName.Equals("Add Netlist") Then
'        MsgBox("dats it")
'    End If


'End Sub


'#########################################################
' selection methode
'#########################################################

'Public Function SelectEntity(ByRef filter As Inventor.SelectionFilterEnum) As Object

'    'On Error Resume Next

'    ' deselect every face in the beginning
'    SelectEntity = Nothing

'    If Not oInteractionEvents Is Nothing And Not oSelectEvents Is Nothing Then

'        ' define 'select events'
'        oInteractionEvents.SelectionActive = True

'        oInteractionEvents.InteractionDisabled = False

'        oInteractionEvents.StatusBarText = "Select an face"
'        'If filter = 15877 Then
'        'MsgBox("filter" & filter & "selected")
'        ' End If
'        ' set the filter passed in by methode
'        oSelectEvents.AddSelectionFilter(filter)

'        ' disable mouse move during selection
'        oSelectEvents.MouseMoveEnabled = False

'        'oSelectEvents.SingleSelectEnabled = True
'        ' start the InteractionEvents object...
'        oInteractionEvents.Start()
'        ' ...and create the Minitoolbar for the selection process
'        CreateMiniToolbar()
'        oMiniToolbar.Visible = True

'        Dim oSelectedEntities As Inventor.ObjectsEnumerator = oSelectEvents.SelectedEntities
'        ' selection process loop
'        Dim oldCount As Integer
'        Do While bStillSelecting
'            If oSelectEvents.SelectedEntities.Count > oldCount Then
'                MsgBox("neues Face ausgewählt")
'            End If
'            'System.Windows.Forms.Application.DoEvents()
'            MidAddIn.UserInterfaceManager.DoEvents()
'            oldCount = oSelectEvents.SelectedEntities.Count

'        Loop




'        'return the selected item. (just one, ignore the rest)
'        'If oSelectedEntities.Count > 0 Then
'        'SelectEntity = oSelectedEntities.Item(1)

'        ' Else
'        SelectEntity = Nothing
'        'End If

'        'stop the InteractionEvents object
'        oInteractionEvents.Stop()
'        'MsgBox("Selection canceled")

'        oMiniToolbar.Delete()
'        oSelectEvents = Nothing
'        oInteractionEvents = Nothing

'    End If

'End Function



' Event: after starting selection process

'Private Sub oInteractionEvents_OnActivate() Handles oInteractionEvents.OnActivate
'    ' initialize the flag to specify selection is still active
'    bStillSelecting = True
'End Sub

'set the flag to indicate selection is done
'     bStillSelecting = False

' End Sub

' Event: after canceling selection process
'Private Sub oInteractionEvents_OnTerminate() Handles oInteractionEvents.OnTerminate

'    ' set the flag to indicate selection is done
'    bStillSelecting = False
'End Sub


'Dim oFace As Face = SelectedEnts.Item(i)
' Dim oSurfacebody As SurfaceBody = oFace.Parent

'Dim Occurrence1 As ComponentOccurrence = oFace.Parent.Parent
'Occurrence1.Appearance = oAppearance
'Dim oPartDef1 As PartComponentDefinition = Occurrence1.Definition()
'Dim oPartDef2 As PartComponentDefinition = oPartDoc.ComponentDefinition()

'Dim oSurfaceBody As SurfaceBody = oPartDef1.SurfaceBodies.Item(1)
'Dim oSurfaceBodyProxy As SurfaceBodyProxy
'Occurrence1.CreateGeometryProxy(oSurfaceBody, oSurfaceBodyProxy)

'Dim oFeatureDef As NonParametricBaseFeatureDefinition = oPartDef2.Features.NonParametricBaseFeatures.CreateDefinition()

'Dim oCollection As ObjectCollection = MidAddIn.TransientObjects.CreateObjectCollection()

'oCollection.Add(oSurfaceBodyProxy)

'oFeatureDef.BRepEntities = oCollection
''oFeatureDef.OutputType = BaseFeatureOutputTypeEnum.kSurfaceOutputType
''oFeatureDef.IsAssociative = True

''Dim oBaseFeature1 As NonParametricBaseFeature = oPartDef2.Features.NonParametricBaseFeatures.AddByDefinition(oFeatureDef)


'Dim oBaseFeature As NonParametricBaseFeature = oPartDef2.Features.NonParametricBaseFeatures.Add(oSurfaceBody)
'If oPartDef2.SurfaceBodies.Item(1).Faces.Item(1).AttributeSets.NameIsUsed("midFaces") Then
'    MsgBox("works")
'End If

'Dim oPartDoc As PartDocument = MidAddIn.Documents.Add(DocumentTypeEnum.kPartDocumentObject, , False)
'Dim oCompDef As PartComponentDefinition = oPartDoc.ComponentDefinition

'' Create Box
'Dim oBox As Box = MidAddIn.TransientGeometry.CreateBox()

'Dim minPoint(2) As Double
'Dim maxPoint(2) As Double

'Dim oFaceMinPoint As Point = oFace.Evaluator.RangeBox.MinPoint
'Dim oFaceMaxPoint As Point = oFace.Evaluator.RangeBox.MaxPoint

'                        minPoint(0) = oFaceMinPoint.X : minPoint(1) = oFaceMinPoint.Y : minPoint(2) = oFaceMinPoint.Z

'                        maxPoint(0) = oFaceMaxPoint.X : maxPoint(1) = oFaceMaxPoint.Y + 0.01 : maxPoint(2) = oFaceMaxPoint.Z

'                        oBox.PutBoxData(minPoint, maxPoint)

'' Create surfacebody and feature
'Dim oBody As SurfaceBody = MidAddIn.TransientBRep.CreateSolidBlock(oBox)

'Dim oBaseFeature As NonParametricBaseFeature = oCompDef.Features.NonParametricBaseFeatures.Add(oBody)
'Dim oPosMatrix As Matrix = MidAddIn.TransientGeometry.CreateMatrix()

'Dim oBoardOcc As ComponentOccurrence = oAsmDoc.ComponentDefinition.Occurrences.AddByComponentDefinition(oCompDef, oPosMatrix)
'                        oBoardOcc.Appearance = oAppearance

'Dim oCompOcc As ComponentOccurrence = oFace.Parent.Parent
'Dim oSurfBody As SurfaceBody = oCompOcc.SurfaceBodies.Item(1)
'Dim oSurfBodyProxy As SurfaceBodyProxy
'oCompOcc.CreateGeometryProxy(oSurfBody, oSurfBodyProxy)
'oSurfBodyProxy.Faces.Item(1).Appearance = oAppearance



' Create new face client graphics
'***************************************************************************************************************
'Private Sub CreateFaceGraphics(face As Face)

'    Dim oInteractionGraphics As InteractionGraphics = oInteractionEvents.InteractionGraphics
'    Dim oClientGraphics As ClientGraphics = oInteractionGraphics.PreviewClientGraphics
'    If oFaceNode Is Nothing Then
'        oFaceNode = oClientGraphics.AddNode(1)
'    End If

'    'Dim oClientGraphics As ClientGraphics = oAsmDoc.ComponentDefinition.ClientGraphicsCollection.Add(oServer.ClientId)
'    ' Add Node for occurrence copy

'    ' #1 define surfacebody copy
'    Dim oSurfaceBody As SurfaceBody = MidAddIn.TransientBRep.Copy(face)
'    ' #2 create surface graphic
'    Dim oSurfaceGraphics As SurfaceGraphics = oFaceNode.AddSurfaceGraphics(oSurfaceBody)
'    oSurfaceGraphics.DepthPriority = 3
'    ' Set render style (red)

'    Try
'        Dim oAsmDoc As AssemblyDocument = MidAddIn.ActiveDocument()
'        Dim oStyle As RenderStyle = oAsmDoc.RenderStyles.Item("Smooth - Red")
'        oFaceNode.RenderStyle = oStyle
'    Catch ex As Exception
'        System.Windows.Forms.MessageBox.Show("Could not find 'Smooth - Red'-asset in the asset library")
'    End Try

'    ' Set to correct position
'    Dim oOccMatrix As Matrix = oServer.MidDataTypes.CircuitCarrier.Occurrence.Transformation
'    Dim oNodeMatrix As Matrix = oFaceNode.Transformation
'    oNodeMatrix.TransformBy(oOccMatrix)
'    oFaceNode.Transformation = oNodeMatrix
'    ' Update view
'    MidAddIn.ActiveView.Update()

'End Sub

'Private oTransactionManager As TransactionManager
'Private WithEvents oTransactionEvents As TransactionEvents

'' Remove client graphics
''***************************************************************************************************************
'Public Sub DeleteFaceGraphics()

'    If oFaceNode IsNot Nothing Then
'        oFaceNode.Delete()
'        oFaceNode = Nothing
'    End If

'    MidAddIn.ActiveView.Update()

'End Sub