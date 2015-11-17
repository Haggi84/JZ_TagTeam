Option Strict Off
Option Explicit On

Imports Inventor
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports System.Xml
Imports MIDProjectVB3.ExportBRep

'############################################
' Keep Out Command
'############################################

Public Class KeepOutCommand
    Inherits Command

    Private oAsmDoc As AssemblyDocument

    Private oKeepOutDlg As KeepOutCmdDlg

    Private oMiniToolbar As Inventor.MiniToolbar
    Private oMtbButtonExit As MiniToolbarButton
    Private oMtbAddFaceToKeepOuts As MiniToolbarButton
    Private oMtbRmvFaceFromKeepOuts As MiniToolbarButton
    Private oMtbButtonReset As MiniToolbarButton

    ' Not supported yet
    'Private oAppearance As Asset

    Private oTG As TransientGeometry

    Private offset As Double = 20
    Private oXmlReader As XmlTextReader
    Private oKeepOuts As KeepOuts

    Private oServer As MidAddInServer

    ' constructor
    '***************************************************************************************************************
    Public Sub New(MidAddIn As Inventor.Application, oServer As MidAddInServer)
        ' call base class
        MyBase.New(MidAddIn)

        Me.oServer = oServer '###later in base class

        oKeepOuts = New KeepOuts(MidAddIn, oServer)

        oKeepOutDlg = Nothing
        oMiniToolbar = Nothing

    End Sub

    ' Return the number of keep out faces
    '***************************************************************************************************************
    Public ReadOnly Property NumOfKeepOutFaces As Integer
        Get
            Return oKeepOuts.Count
        End Get
    End Property


    ' Initialize button execution
    '***************************************************************************************************************
    Protected Overrides Sub oButtonDefinition_OnExecute()
        ' Only execute if assemlby environment
        'Dim currEnvironment As Inventor.Environment = MidAddIn.UserInterfaceManager.ActiveEnvironment

        'If LCase$(currEnvironment.InternalName) <> LCase$("AMxAssemblyEnvironment") Then
        '    MessageBox.Show("This command applies only to assembly environment", _
        '                    "MIDProject", _
        '                    MessageBoxButtons.OK, _
        '                    MessageBoxIcon.Error)
        '    Exit Sub
        'End If

        ' Change processor
        'Dim oChangeManager As ChangeManager = MidAddIn.ChangeManager
        'Dim oChangeDefs As ChangeDefinitions = oChangeManager.Add("clientid")
        'Dim oChangeDef As ChangeDefinition = oChangeDefs.Add("Autodesk:ChangeDef:KeepOuts", "KeepOuts")

        ' For Debugging
        'Dim ComMan As CommandManager = MidAddIn.CommandManager
        'MsgBox(ComMan.ActiveCommand)
   

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
        MyBase.SubscribeToEvent(Interaction1.InteractionTypeEnum.kSelection)

        ' Create new form dialog
        oKeepOutDlg = New KeepOutCmdDlg(MidAddIn, Me)

        If oKeepOutDlg IsNot Nothing Then
            oKeepOutDlg.TopMost() = True
            oKeepOutDlg.ShowInTaskbar() = True
            oKeepOutDlg.Show()
            Dim oView As Inventor.View = MidAddIn.ActiveView()
            oKeepOutDlg.Location = New System.Drawing.Point(oView.Left + 20, oView.Top + 20)
        End If

        ' Create mini toolbar
        CreateMiniToolbar()

        ' Not supportet yet
        'Dim assetLib1 As AssetLibrary = MidAddIn.AssetLibraries.Item("Autodesk Appearance Library")
        'Dim locAsset1 As Asset = assetLib1.AppearanceAssets.Item("Dark Red")
        'oAppearance = locAsset1.CopyTo(oAsmDoc)

        ' Enable command specific functions
        EnableInteraction()

    End Sub

    'Stop Command
    '***************************************************************************************************************
    Public Overrides Sub StopCommand()
        ' Stop command is usually called twice: user and end of interaction events
        ' thus check for empty pointers
        'If oKeepOutDlg Is Nothing Then
        ' Destroy the command dialog
        oKeepOutDlg.Hide()
        oKeepOutDlg.Dispose() ' make it ready for garbage collector
        oKeepOutDlg = Nothing
        'End If
        ' Remove mini toolbar
        oMiniToolbar.Delete()

        ' Disconnect events sink
        MyBase.StopCommand()

    End Sub

    'Enable Interaction
    '***************************************************************************************************************
    Public Overrides Sub EnableInteraction()

        ' Resubscribe all event handlers for the subscribed events
        MyBase.EnableInteraction()

        oInteractionEvents.SelectionActive = True
        oInteractionEvents.InteractionDisabled = False

        oInteractionEvents.StatusBarText = "Select a face"

        ' Only faces selectable
        oSelectEvents.AddSelectionFilter(SelectionFilterEnum.kPartFaceFilter)

        oMiniToolbar.Visible = True

    End Sub

    'Disable Interaction
    '***************************************************************************************************************
    Public Overrides Sub DisableInteraction()

        'base command button's DisableInteraction
        MyBase.DisableInteraction()

        'Set the default cursor to notify that selection is not active
        oInteractionEvents.SetCursor(CursorTypeEnum.kCursorBuiltInArrow)

        'Clean up status text
        oInteractionEvents.StatusBarText = String.Empty

        ' Make minitoolbar invisble
        oMiniToolbar.Visible = False

    End Sub

    Public Function FindFaceById(ByVal _id As String) As Inventor.Face

        Dim oOccurrences As ComponentOccurrences = MidAddIn.ActiveDocument.ComponentDefinition.Occurrences

        For Each Occ As ComponentOccurrence In oOccurrences
            If Occ.AttributeSets.NameIsUsed("circuitcarrier") Then
                Dim SurfaceBodies As SurfaceBodies = Occ.SurfaceBodies
                For Each Body As SurfaceBody In SurfaceBodies
                    Dim oFaces As Faces = Body.Faces
                    For Each oFace As Face In oFaces
                        If oFace.InternalName.Equals(_id) Then
                            Return oFace
                        End If
                    Next
                Next
            End If
        Next

        Return Nothing

    End Function

    ' Read KeepOuts from XML
    '***************************************************************************************************************
    Public Sub ReadKeepOuts(ByVal strFilePath As String)

        Try
            ' Clear the KeepOut List
            oKeepOuts.Clear()
            oKeepOutDlg.updateDlg()

            oXmlReader = New XmlTextReader(strFilePath)

            Dim _routingAllowed As Boolean
            Dim _faceId As String
            Dim _Id As String
            ' Start reading line by line

            Do While (oXmlReader.Read())

                Select Case oXmlReader.NodeType
                    ' Node Element
                    Case XmlNodeType.Element
                        If oXmlReader.HasAttributes() Then

                            Select Case oXmlReader.Name

                                Case "FaceKeepOut"
                                    Debug.WriteLine(oXmlReader.Name)
                                    While oXmlReader.MoveToNextAttribute()
                                        If String.Equals(oXmlReader.Name, "routingAllowed") Then

                                            Debug.WriteLine(oXmlReader.Name & " = " & oXmlReader.Value)
                                            _routingAllowed = Convert.ToBoolean(oXmlReader.Value)
                                        End If

                                        If String.Equals(oXmlReader.Name, "faceID") Then
                                            Debug.WriteLine(oXmlReader.Name & " = " & oXmlReader.Value)
                                            _faceId = oXmlReader.Value
                                        End If

                                        If String.Equals(oXmlReader.Name, "Id") Then
                                            Debug.WriteLine(oXmlReader.Name & " = " & oXmlReader.Value)
                                            _Id = oXmlReader.Value


                                        End If
                                    End While

                                    Dim oFace As Face = FindFaceById(_Id)

                                    If oFace IsNot Nothing Then
                                        oKeepOuts.Add(oFace, _routingAllowed, _faceId)
                                    Else
                                        MessageBox.Show("The KeepOuts do not belong to the circuit carrier")
                                        oKeepOuts.Clear()
                                        oKeepOutDlg.updateDlg()
                                    End If

                            End Select

                        End If

                End Select
            Loop

        Catch ex As Exception
            MessageBox.Show("There was an error reading from xml file", "MID Project", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            oKeepOutDlg.updateDlg()
            oXmlReader.Close()
            oXmlReader = Nothing
        End Try

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
        oMiniToolbar.Position = ViewPosition

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
        If TypeOf preSelectEntity Is Face Then

            If preSelectEntity.Parent.Parent.AttributeSets.NameIsUsed("circuitcarrier") Then
                oSelectEvents.ClearSelectionFilter()
                oSelectEvents.AddSelectionFilter(SelectionFilterEnum.kPartFaceFilter)
            Else
                oSelectEvents.ClearSelectionFilter()
                oSelectEvents.AddSelectionFilter(SelectionFilterEnum.kSketch3DCurveEllipseFilter)
            End If

        End If

    End Sub


    '#############################################################
    ' Mini Toolbar EVENTS
    '#############################################################

    ' Remove faces from KeepOuts
    '***************************************************************************************************************
    Public Sub RmvFaceFromKeepOuts()

        ' Get list of selected entities
        Dim oSelectedEnts As Inventor.ObjectsEnumerator = oSelectEvents.SelectedEntities
        If oSelectedEnts.Count > 0 And oKeepOuts.Count > 0 Then

            ' Remove faces from KeepOuts
            For i As Long = 1 To oSelectedEnts.Count
                oKeepOuts.Remove(oSelectedEnts.Item(i))
            Next

        End If

        ' Reset Selection
        oSelectEvents.ResetSelections()
        oSelectedEnts = Nothing

        ' Show number of keepouts in the dialog
        oKeepOutDlg.updateDlg()

    End Sub

    ' Add faces to KeepOuts
    '***************************************************************************************************************
    Public Sub AddFaceToKeepOuts()

        Dim oSelectedEnts As Inventor.ObjectsEnumerator = oSelectEvents.SelectedEntities
        If oSelectedEnts.Count > 0 Then

            ' Add faces to KeepOuts
            For i As Long = 1 To oSelectedEnts.Count
                oKeepOuts.Add(oSelectedEnts.Item(i))
            Next
        End If

        ' Reset face selection
        oSelectEvents.ResetSelections()
        oSelectedEnts = Nothing

        ' Show number of keepouts in the dialog
        oKeepOutDlg.updateDlg()

    End Sub

    ' Remove all keepOut from keepOuts
    '***************************************************************************************************************
    Public Sub ResetSelection()

        oKeepOuts.Clear()
        oKeepOutDlg.updateDlg()

        oSelectEvents.ResetSelections()

    End Sub

    ' Write all keepOuts to xml file
    '***************************************************************************************************************
    Public Sub WriteXml()

        oKeepOuts.WriteXml()
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
        Dim mtbAddPicture As stdole.IPictureDisp = MIDProjectVB3.PictureConverter.ImageToPictureDisp(My.Resources.mtbAddSmall1)
        Dim mtbExitPicture As stdole.IPictureDisp = MIDProjectVB3.PictureConverter.ImageToPictureDisp(My.Resources.mtbExitSmall1)
        Dim mtbRmvPicture As stdole.IPictureDisp = MIDProjectVB3.PictureConverter.ImageToPictureDisp(My.Resources.mtbRmvSmall1)
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
        AddHandler oMtbButtonReset.OnClick, AddressOf Me.ResetSelection
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

    Public ReadOnly Property KeepOuts() As KeepOuts
        Get
            Return oKeepOuts
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