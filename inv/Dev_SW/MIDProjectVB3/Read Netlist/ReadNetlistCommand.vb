Option Explicit On

Imports Inventor
Imports System.Xml
Imports System.Collections.Generic
Imports System.Math
Imports System.Globalization
Imports System.Windows.Forms
Imports System.IO

'##############################################
' ReadNetListCommand Class
'##############################################

Public Class ReadNetlistCommand
    Inherits Command

#Region "Data"

    Private Enum SelectionModeEnum
        kFaceAlign
        kEdgeAlign
    End Enum

    Private oMiniToolbar As MiniToolbar

    Private WithEvents oMtbButtonApply As MiniToolbarButton
    Private WithEvents oMtbButtonUpX As MiniToolbarButton
    Private WithEvents oMtbButtonDownX As MiniToolbarButton
    Private WithEvents oMtbButtonUpY As MiniToolbarButton
    Private WithEvents oMtbButtonDownY As MiniToolbarButton
    Private WithEvents oMtbButtonUpZ As MiniToolbarButton
    Private WithEvents oMtbButtonDownZ As MiniToolbarButton
    Private WithEvents oMtbAlignButton As MiniToolbarDropdown

    Private dlgOffset As Integer

    Private oBoard As CircuitBoard

    Private oReadNetlistCmdDlg As ReadNetlistCmdDlg
    Private oServer As MidAddInServer

    Private strFilePath As String
    Private strFolderPath As String

    Private oAlignFace As Face

    Private oEdgeHighSet As HighlightSet

    Private oTriad As Triad

    Private selectionMode As SelectionModeEnum

#End Region

    ' Constructor
    '***************************************************************************************************************
    Public Sub New(addIn As Inventor.Application, _
                   server As MidAddInServer)

        MyBase.New(addIn)

        Me.oServer = server

        oMiniToolbar = Nothing
        oReadNetlistCmdDlg = Nothing
        oAlignFace = Nothing
        oEdgeHighSet = Nothing
        oTriad = Nothing
        oBoard = Nothing

        dlgOffset = 40

    End Sub

    ' Initialize the button execution
    '***************************************************************************************************************
    Public Overrides Sub ButtonDefinition_OnExecute()
        ' Only execute if assemlby environment
        Dim currentEnvironment As Inventor.Environment = MidAddIn.UserInterfaceManager.ActiveEnvironment

        If LCase(currentEnvironment.InternalName) <> LCase("OPTAVER") Then
            MessageBox.Show("This command works only for assembly environment", "MIDProject", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If oServer.MidDataTypes.CircuitBoard IsNot Nothing Then
            MessageBox.Show("There already is a circuit in this assembly", "MIDProject", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            Exit Sub
        End If

        If bCommandIsRunning Then
            StopCommand()
        End If

        'Start new command
        StartCommand()
    End Sub


    ' Start the command
    '***************************************************************************************************************
    Public Overrides Sub StartCommand()

        ' Start Interaction /Interaction.start
        MyBase.StartCommand()

        ' default cursor
        oInteractionEvents.SetCursor(CursorTypeEnum.kCursorTypeDefault)

        ' Subscribe to mouse event
        MyBase.SubscribeToEvent(Interaction.InteractionTypeEnum.kSelection)

        ' Create new form dialog
        oReadNetlistCmdDlg = New ReadNetlistCmdDlg(MidAddIn, Me)

        If oReadNetlistCmdDlg IsNot Nothing Then
            oReadNetlistCmdDlg.TopMost() = True
            oReadNetlistCmdDlg.ShowInTaskbar() = True
            oReadNetlistCmdDlg.StartPosition = FormStartPosition.Manual
            Dim oView As Inventor.View = MidAddIn.ActiveView()
            oReadNetlistCmdDlg.Location = New System.Drawing.Point(oView.Left + dlgOffset, oView.Top + dlgOffset)
            oReadNetlistCmdDlg.Show()
        End If

        ' Disable buttons by default
        oReadNetlistCmdDlg.okButton.Enabled = False
        oReadNetlistCmdDlg.importButton.Enabled = False

        ' Create highlight set for edges
        oEdgeHighSet = MidAddIn.ActiveDocument.CreateHighlightSet()
        oEdgeHighSet.Color = MidAddIn.TransientObjects.CreateColor(255, 123, 0)

        ' Create minitoolbar
        CreateMiniToolbar()

        ' Create client graphics triad
        oTriad = New Triad(MidAddIn, oInteractionEvents)

        ' Enable command specific functions
        DisableInteraction()

    End Sub

    ' Stop Command (clean up)
    '***************************************************************************************************************
    Public Overrides Sub StopCommand()
      
        'Destroy the command dialog
        oReadNetlistCmdDlg.Hide()
        oReadNetlistCmdDlg.Dispose() ' make it ready for garbage collector
        oReadNetlistCmdDlg = Nothing

        ' Delete the highlight set
        oEdgeHighSet.Clear()
        oEdgeHighSet = Nothing

        ' Delete the mini toolbar
        oMiniToolbar.Delete()

        ' Delete the traid
        oTriad.Delete()
        oTriad = Nothing

        'Call base command button's StopCommand (to disconnect interaction sink)
        MyBase.StopCommand()

    End Sub


    'Enable Interaction
    '***************************************************************************************************************
    Public Overrides Sub EnableInteraction()

        ' Resubscribe all event handlers for the subscribed events
        MyBase.EnableInteraction()

        Select Case selectionMode
            Case SelectionModeEnum.kFaceAlign

                oSelectEvents.SingleSelectEnabled = True
                oSelectEvents.ResetSelections()
                oSelectEvents.ClearSelectionFilter()
                oSelectEvents.AddSelectionFilter(SelectionFilterEnum.kPartFacePlanarFilter)

                oInteractionEvents.StatusBarText = "Select face to align to"

            Case SelectionModeEnum.kEdgeAlign

                oSelectEvents.SingleSelectEnabled = True
                oSelectEvents.ResetSelections()
                oSelectEvents.ClearSelectionFilter()
                oSelectEvents.AddSelectionFilter(SelectionFilterEnum.kPartEdgeFilter)

                oInteractionEvents.StatusBarText = "Select edge to align to"

        End Select


        ' Disable selection
        'oInteractionEvents.SelectionActive = False
        'oInteractionEvents.InteractionDisabled = True


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
    '***************************************************************************************************************
    Public Overrides Sub ExecuteCommand()
        MyBase.ExecuteCommand()

        ' save the reference to MidDataTypes before it is removed in StopCommand()
        oServer.MidDataTypes.CircuitBoard = oBoard
        StopCommand()

        ' Add new change request
        Dim oAddNetlistRequest As New ReadNetlistRequest(MidAddIn, oServer)
        MyBase.ExecuteChangeRequest(oAddNetlistRequest, "MidAddIn:AddNetlistRequest", MidAddIn.ActiveDocument)

    End Sub

    ' Validate file
    '***************************************************************************************************************
    Private Function FileIsValid(ByVal filePath As String) As Boolean

        Try
            Dim oXmlReader As XmlTextReader = New XmlTextReader(filePath)

            If Not oXmlReader.IsStartElement("Circuit") Then
                MessageBox.Show("The selected file does not contain a netlist", "MID Project", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
                oXmlReader.Close()
                oXmlReader = Nothing
                Return False
            End If
            oXmlReader.Close()
            oXmlReader = Nothing
            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

    '###########################################################
    ' SELECT CALLBACKS
    '###########################################################

    ' OnPreSelect
    Public Overrides Sub OnPreSelect(ByRef preSelectEntity As Object, _
                                     ByRef doHighlight As Boolean, _
                                     ByRef morePreSelectEntities As ObjectCollection, _
                                     selectionDevice As SelectionDeviceEnum, _
                                     modelPosition As Point, _
                                     viewPosition As Point2d, _
                                     view As Inventor.View)

        If selectionMode = SelectionModeEnum.kEdgeAlign Then

            If selectionDevice = SelectionDeviceEnum.kGraphicsWindowSelection Then

                If TypeOf preSelectEntity Is Edge Then

                    If Not Parent(preSelectEntity) Then
                        doHighlight = False
                    End If

                End If

            End If
        End If

    End Sub

    Private Function Parent(oEdge As Edge) As Boolean

        For Each edge As Edge In oAlignFace.Edges
            If edge Is oEdge Then
                Return True
            End If
        Next
        Return False
    End Function

    ' OnSelect
    '*************************************************************************************************************************
    Public Overrides Sub OnSelect(justSelectedEntities As ObjectsEnumerator, _
                                  selectionDevice As SelectionDeviceEnum, _
                                  modelPosition As Point, _
                                  viewPosition As Point2d, _
                                  view As Inventor.View)

        If selectionDevice = SelectionDeviceEnum.kGraphicsWindowSelection Then

            ' FaceAlign mode
            If TypeOf (justSelectedEntities.Item(1)) Is Inventor.Face And selectionMode = SelectionModeEnum.kFaceAlign Then
                AlignToFace(justSelectedEntities.Item(1))
            End If

            ' EdgeAlign mode
            If TypeOf (justSelectedEntities.Item(1)) Is Inventor.Edge And selectionMode = SelectionModeEnum.kEdgeAlign Then
                AlignToEdge(justSelectedEntities.Item(1))
            End If

            ' Disable interaction
            oSelectEvents.ResetSelections()
            oMtbAlignButton.Pressed = False
            DisableInteraction()

        End If
    End Sub

    '###########################################################
    ' EDIT MODE
    '###########################################################

    ' Edit mode
    '***************************************************************************************************************
    Private Sub EnableEdit()

        ' Create board clientgraphics
        oBoard.CreatePreviewGraphics(oInteractionEvents, "Clear - Light")

        ' Make toolbar visible
        oMiniToolbar.Visible = True

        ' Make triad visible
        oTriad.Visible = False

        ' re-position user cooridnate system
        Move(MidAddIn.TransientGeometry.CreateVector())

    End Sub

    ' Disable edit mode
    '***************************************************************************************************************
    Private Sub DisableEdit()

        ' Delete triad graphics
        oTriad.Visible = False

        ' Hide toolbar and UCS
        oMiniToolbar.Visible = False

        ' Make client graphics invisible
        oBoard.DeletePreviewGraphics()
    End Sub

    ' Align to face
    '*************************************************************************************************************************
    Private Sub AlignToEdge(edge As Edge)

        oEdgeHighSet.Clear()

        Dim oTG As TransientGeometry = MidAddIn.TransientGeometry

        ' Get direction vector from matrix
        'Dim oBoardVector As Vector = oTG.CreateVector(oBoard.Transformation.Cell(1, 2), oBoard.Transformation.Cell(2, 2), oBoard.Transformation.Cell(3, 2))
        'oBoardVector.Normalize()

        ' Get coordinate axis from edge
        Dim oStartVertex As Vertex = edge.StartVertex
        Dim oEndVertex As Vertex = edge.StopVertex
        Dim startPoint(2) As Double
        Dim stopPoint(2) As Double
        oStartVertex.GetPoint(startPoint)
        oEndVertex.GetPoint(stopPoint)
        Dim oEdgeVector As Vector = oTG.CreateVector(startPoint(0) - stopPoint(0), startPoint(1) - stopPoint(1), startPoint(2) - stopPoint(2))
        oEdgeVector.Normalize()

        ' Calculate angle and rotate part
        Dim angle = Math.Acos(oEdgeVector.DotProduct(oBoard.GetEdgeVector))

        ' Rotate matrix according to the angle
        Dim oMatrix As Matrix = MidAddIn.TransientGeometry.CreateMatrix()
        oMatrix.SetToRotation(angle, oTG.CreateVector(0, 0, 1), MidAddIn.TransientGeometry.CreatePoint())
        oMatrix.TransformBy(oBoard.Transformation)

        ' Set new transformation
        oBoard.Transformation = oMatrix
        oBoard.UpdatePreviewGraphics()
        oTriad.Transformation = oMatrix

    End Sub

    ' Align to face
    '*************************************************************************************************************************
    Private Sub AlignToFace(face As Face)

        'Set reference to face
        oAlignFace = face

        Dim oTG As TransientGeometry = MidAddIn.TransientGeometry

        ' Calculate normal of the selected face
        Dim oFaceMinPoint As Point = face.Evaluator.RangeBox.MinPoint
        Dim FaceMinPoint(2) As Double
        FaceMinPoint(0) = oFaceMinPoint.X
        FaceMinPoint(1) = oFaceMinPoint.Y
        FaceMinPoint(2) = oFaceMinPoint.Z
        Dim faceNormal(2) As Double
        face.Evaluator.GetNormalAtPoint(FaceMinPoint, faceNormal)
        Dim oFaceNormalVector As Vector = oTG.CreateVector(faceNormal(0), faceNormal(1), faceNormal(2))
        oFaceNormalVector.Normalize()

        ' Align both normals
        Dim oAlignMatrix As Matrix = oTG.CreateMatrix()
        Debug.WriteLine(oBoard.GetEdgeVector.X & "   " & oBoard.GetNormalVector.Y & " " & oBoard.GetNormalVector.Z)
        oAlignMatrix.SetToRotateTo(oBoard.GetNormalVector, oFaceNormalVector, Nothing)

        ' Set transformation
        oBoard.Transformation = oAlignMatrix
        oBoard.UpdatePreviewGraphics()
        oTriad.Transformation = oAlignMatrix

    End Sub

    ' Execute align command
    '*************************************************************************************************************************
    Private Sub oMtbAlignButton_OnSelect(listItem As MiniToolbarListItem) Handles oMtbAlignButton.OnSelect

        If listItem.InternalName.Equals("AlignToFaceInternal") Then
            oEdgeHighSet.Clear()
            selectionMode = SelectionModeEnum.kFaceAlign
        End If

        If listItem.InternalName.Equals("AlignToEdgeInternal") Then
            If oAlignFace Is Nothing Then
                MessageBox.Show("Align to Face first", "MID Project", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
                Exit Sub
            End If
            ' Add edges of the align-face to the highlight set
            For Each oEdge As Edge In oAlignFace.Edges
                oEdgeHighSet.AddItem(oEdge)
            Next
            selectionMode = SelectionModeEnum.kEdgeAlign
        End If

        oMtbAlignButton.Pressed = True
        EnableInteraction()

    End Sub

    ' Translate component
    '*************************************************************************************************************************
    Private Sub Move(transVector As Vector)

        Dim oNewPosMatrix As Matrix = oBoard.Transformation
        transVector.TransformBy(oNewPosMatrix)

        oNewPosMatrix.Cell(1, 4) += transVector.X
        oNewPosMatrix.Cell(2, 4) += transVector.Y
        oNewPosMatrix.Cell(3, 4) += transVector.Z

        oBoard.Transformation = oNewPosMatrix
        oTriad.Transformation = oNewPosMatrix
        oBoard.UpdatePreviewGraphics()

    End Sub

    ' Arrow buttons
    '*************************************************************************************************************************
    Private Sub oMtbButtonUpX_OnClick() Handles oMtbButtonUpX.OnClick
        Dim oTransVector As Vector = MidAddIn.TransientGeometry.CreateVector(0.3, 0, 0)
        Move(oTransVector)
    End Sub

    Private Sub oMtbButtonDownX_OnClick() Handles oMtbButtonDownX.OnClick
        Dim oTransVector As Vector = MidAddIn.TransientGeometry.CreateVector(-0.3, 0, 0)
        Move(oTransVector)
    End Sub

    Private Sub oMtbButtonUpZ_OnClick() Handles oMtbButtonUpZ.OnClick
        Dim oTransVector As Vector = MidAddIn.TransientGeometry.CreateVector(0, 0, 0.3)
        Move(oTransVector)
    End Sub

    Private Sub oMtbButtonDownZ_OnClick() Handles oMtbButtonDownZ.OnClick
        Dim oTransVector As Vector = MidAddIn.TransientGeometry.CreateVector(0, 0, -0.3)
        Move(oTransVector)
    End Sub

    Private Sub oMtbButtonUpY_OnClick() Handles oMtbButtonUpY.OnClick
        Dim oTransVector As Vector = MidAddIn.TransientGeometry.CreateVector(0, 0.3, 0)
        Move(oTransVector)
    End Sub

    Private Sub oMtbButtonDownY_OnClick() Handles oMtbButtonDownY.OnClick
        Dim oTransVector As Vector = MidAddIn.TransientGeometry.CreateVector(0, -0.3, 0)
        Move(oTransVector)
    End Sub

    '###########################################################
    ' READ NETLIST
    '###########################################################

    ' Read BRep
    '***************************************************************************************************************
    Public Sub ReadBRep()

        ' Check if there already is a circuitboard imported, otherwise delete old one
        If oBoard IsNot Nothing Then
            oBoard.Delete()

        End If

        ' Check whether the file exists
        'If Not System.IO.File.Exists(oReadNetlistCmdDlg.strFilePath) Then
        '    MessageBox.Show("File does not exist, please try another file", "MIDProject", MessageBoxButtons.OK, MessageBoxIcon.Information)
        '    Exit Sub
        'End If

        ' Check whether the directory exists
        If Not System.IO.Directory.Exists(oReadNetlistCmdDlg.strFolderPath) Then
            MessageBox.Show("Folder does not exist, please choose another folder", "MIDProject", MessageBoxButtons.OK, MessageBoxIcon.Information)
            oReadNetlistCmdDlg.partBrowseButton.Enabled = True
            oReadNetlistCmdDlg.browseButton.Enabled = True
            oReadNetlistCmdDlg.importButton.Enabled = True
            oReadNetlistCmdDlg.cancelButton.Enabled = True
            Exit Sub
        End If

        ''Check validity
        ''If Not FileIsValid(oReadNetlistCmdDlg.strFilePath) Then
        ''    Exit Sub
        ''End If

        ' Open once to cover some IO Exceptions
        Try
            Dim oXmlReader As XmlTextReader = New XmlTextReader(oReadNetlistCmdDlg.strFilePath)
        Catch dirNotFound As DirectoryNotFoundException
            MessageBox.Show("File does not exist, please choose another file", "MIDProject", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        Catch fileNotFound As FileNotFoundException
            MessageBox.Show("The Folder does not exist", "MIDProject", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        Catch ex As Exception
            MessageBox.Show("There was an error reading the file from the specified location", "MIDProject", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        Finally
            oReadNetlistCmdDlg.partBrowseButton.Enabled = True
            oReadNetlistCmdDlg.browseButton.Enabled = True
            oReadNetlistCmdDlg.importButton.Enabled = True
            oReadNetlistCmdDlg.cancelButton.Enabled = True
        End Try

        ' Disable all buttons
        oReadNetlistCmdDlg.partBrowseButton.Enabled = False
        oReadNetlistCmdDlg.browseButton.Enabled = False
        oReadNetlistCmdDlg.importButton.Enabled = False
        oReadNetlistCmdDlg.cancelButton.Enabled = False

        ' Save xml file path and part folder path
        Me.strFilePath = oReadNetlistCmdDlg.strFilePath
        Me.strFolderPath = oReadNetlistCmdDlg.strFolderPath

        ' Parse board
        ParseBoard()

        ' Change in edit mode
        EnableEdit()

        '' Check Interference with other occurrences
        'CheckInterference(oBoard.Occurrence) '+++++ later

    End Sub

    ' Accept button
    '*************************************************************************************************************************
    Private Sub oMtbButtonApply_OnClick() Handles oMtbButtonApply.OnClick

        DisableEdit()
        ' Parse PartList
        ParseParts()
        ' Create client graphics for PartList
        'oBoard.SetTransformation()

        For Each oPart As CircuitPart In oBoard.PartList
            oPart.SetTransformation()
            oPart.CreatePreviewGraphics(oInteractionEvents, "Clear", strFolderPath)
        Next

        ' Parse netlist
        ParseNetlist()
        ' Create client graphics for NetList
        For Each oNet As CircuitNet In oBoard.NetList
            oNet.CreatePreviewGraphics(oInteractionEvents)
        Next

        ' Re-enable the Dialog buttons
        oReadNetlistCmdDlg.partBrowseButton.Enabled = True
        oReadNetlistCmdDlg.browseButton.Enabled = True
        oReadNetlistCmdDlg.importButton.Enabled = True
        oReadNetlistCmdDlg.cancelButton.Enabled = True
        oReadNetlistCmdDlg.okButton.Enabled = True

    End Sub

    ' Convert units to to inventor internal units (cm)
    '**********************************************************************************************************************
    Private Function ConvertUnit(value As String) As Double

        Dim oUOF As UnitsOfMeasure = MidAddIn.ActiveDocument.UnitsOfMeasure

        Select Case oBoard.UnitOfLength
            Case "mm"
                Return oUOF.ConvertUnits(Convert.ToDouble(value, System.Globalization.CultureInfo.InvariantCulture), _
                                         UnitsTypeEnum.kMillimeterLengthUnits, UnitsTypeEnum.kDatabaseLengthUnits)
            Case "cm"
                Return oUOF.ConvertUnits(Convert.ToDouble(value, System.Globalization.CultureInfo.InvariantCulture), _
                                         UnitsTypeEnum.kCentimeterLengthUnits, UnitsTypeEnum.kDatabaseLengthUnits)
            Case "in"
                Return oUOF.ConvertUnits(Convert.ToDouble(value, System.Globalization.CultureInfo.InvariantCulture), _
                                         UnitsTypeEnum.kInchLengthUnits, UnitsTypeEnum.kDatabaseLengthUnits)
            Case "m"
                Return oUOF.ConvertUnits(Convert.ToDouble(value, System.Globalization.CultureInfo.InvariantCulture), _
                                        UnitsTypeEnum.kInchLengthUnits, UnitsTypeEnum.kDatabaseLengthUnits)
            Case Else
                MessageBox.Show("The application could not recognize the length unit of the xml-file")
        End Select
    End Function

    ' Parse header and board data
    '***************************************************************************************************************
    Private Sub ParseBoard()

        Try

            ' Create new board placed in the center of the coordinate system
            Dim oUnitMatrix As Matrix = MidAddIn.TransientGeometry.CreateMatrix()
            oBoard = New CircuitBoard(MidAddIn, oServer, oUnitMatrix)

            Using oXmlReader As XmlTextReader = New XmlTextReader(strFilePath)

                Do While (oXmlReader.Read())
                    Select Case oXmlReader.NodeType

                        ' Node element
                        Case XmlNodeType.Element
                            If oXmlReader.HasAttributes() Then

                                ' Read header
                                If String.Equals(oXmlReader.Name, "Circuit") Then
                                    Debug.WriteLine(oXmlReader.Name)

                                    While oXmlReader.MoveToNextAttribute()
                                        If String.Equals(oXmlReader.Name, "denotion") Then
                                            oBoard.Denotion = oXmlReader.Value
                                            Debug.WriteLine(oXmlReader.Name & " = " & oXmlReader.Value)
                                        End If

                                        If String.Equals(oXmlReader.Name, "unitOfLength") Then
                                            oBoard.UnitOfLength = oXmlReader.Value
                                            Debug.WriteLine(oXmlReader.Name & " = " & oXmlReader.Value)

                                        End If
                                    End While
                                End If

                                ' Read board data
                                If String.Equals(oXmlReader.Name, "Board") Then
                                    Debug.WriteLine(oXmlReader.Name)

                                    While oXmlReader.MoveToNextAttribute()
                                        If String.Equals(oXmlReader.Name, "b") Then
                                            oBoard.Length = ConvertUnit(oXmlReader.Value)
                                            Debug.WriteLine(oBoard.Length)

                                        End If
                                        If String.Equals(oXmlReader.Name, "h") Then
                                            oBoard.Width = ConvertUnit(oXmlReader.Value)
                                            Debug.WriteLine(oBoard.Width)
                                            Exit Do
                                        End If

                                    End While

                                End If

                                ' Stop the loop after reading board properties
                                If String.Equals(oXmlReader.Name, "PartList") Then
                                    Exit Do
                                End If

                            End If

                    End Select
                Loop
                oXmlReader.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try
    End Sub

    ' Parse PartList
    '***************************************************************************************************************
    Private Sub ParseParts()

        Try
            Dim dev, value, id, origin, figure As String
            Dim x, y, z, length, width As Double
            ' Create new xml-reader
            Using oXmlReader As New XmlTextReader(strFilePath)

                Dim oPart As CircuitPart = Nothing 'New CircuitPart(MidAddIn, oServer, oBoard)
                Dim oPin As CircuitPin = Nothing

                Do While (oXmlReader.Read())

                    Select Case oXmlReader.NodeType

                        ' Node Element
                        Case XmlNodeType.Element
                            If oXmlReader.HasAttributes() Then

                                Select Case oXmlReader.Name

                                    Case "Part"

                                        Debug.WriteLine(oXmlReader.Name)
                                        While oXmlReader.MoveToNextAttribute()
                                            If String.Equals(oXmlReader.Name, "dev") Then
                                                Debug.WriteLine(oXmlReader.Name & " = " & oXmlReader.Value)
                                                dev = oXmlReader.Value
                                            End If
                                            If String.Equals(oXmlReader.Name, "value") Then
                                                Debug.WriteLine(oXmlReader.Name & " = " & oXmlReader.Value)
                                                value = oXmlReader.Value
                                            End If
                                            If String.Equals(oXmlReader.LocalName, "id") Then
                                                Debug.WriteLine(oXmlReader.Name & " = " & oXmlReader.Value)
                                                id = oXmlReader.Value
                                            End If
                                        End While

                                    Case "Position"
                                        Debug.WriteLine(oXmlReader.Name)
                                        While oXmlReader.MoveToNextAttribute()
                                            If String.Equals(oXmlReader.Name, "x") Then
                                                Debug.WriteLine(oXmlReader.Name & " = " & oXmlReader.Value)
                                                x = ConvertUnit(oXmlReader.Value)
                                            End If
                                            If String.Equals(oXmlReader.Name, "y") Then
                                                Debug.WriteLine(oXmlReader.Name & " = " & oXmlReader.Value)
                                                y = ConvertUnit(oXmlReader.Value)
                                            End If
                                            If String.Equals(oXmlReader.Name, "z") Then
                                                Debug.WriteLine(oXmlReader.Name & " = " & oXmlReader.Value)
                                                z = ConvertUnit(oXmlReader.Value)
                                            End If
                                        End While

                                    Case "Shape"
                                        Debug.WriteLine(oXmlReader.Name)
                                        While oXmlReader.MoveToNextAttribute()
                                            If String.Equals(oXmlReader.Name, "b") Then
                                                Debug.WriteLine(oXmlReader.Name & " = " & oXmlReader.Value)
                                                length = ConvertUnit(oXmlReader.Value)
                                            End If
                                            If String.Equals(oXmlReader.Name, "h") Then
                                                Debug.WriteLine(oXmlReader.Name & " = " & oXmlReader.Value)
                                                width = ConvertUnit(oXmlReader.Value)
                                            End If
                                            If String.Equals(oXmlReader.Name, "origin") Then
                                                Debug.WriteLine(oXmlReader.Name & " = " & oXmlReader.Value)
                                                origin = oXmlReader.Value
                                            End If
                                            If String.Equals(oXmlReader.Name, "figure") Then
                                                Debug.WriteLine(oXmlReader.Name & " = " & oXmlReader.Value)
                                                figure = oXmlReader.Value
                                            End If
                                        End While

                                        ' Add new part and put it into the part list
                                        oPart = New CircuitPart(MidAddIn, oServer, oBoard, dev, value, id, x, y, z, width, length, origin, figure)
                                        oBoard.AddPart(oPart)

                                    Case "Pin"

                                        Debug.WriteLine(oXmlReader.Name)
                                        While oXmlReader.MoveToNextAttribute()
                                            If String.Equals(oXmlReader.Name, "id") Then
                                                Debug.WriteLine(oXmlReader.Name & " = " & oXmlReader.Value)
                                                id = oXmlReader.Value
                                            End If

                                            If String.Equals(oXmlReader.Name, "x") Then
                                                Debug.WriteLine(oXmlReader.Name & " = " & oXmlReader.Value)
                                                x = ConvertUnit(oXmlReader.Value)
                                            End If

                                            If String.Equals(oXmlReader.Name, "y") Then
                                                y = ConvertUnit(oXmlReader.Value)
                                                Debug.WriteLine(oXmlReader.Name & " = " & oXmlReader.Value)
                                            End If
                                        End While

                                        ' Add new pin and add pin to the current part
                                        oPin = New CircuitPin(MidAddIn, oServer, oPart, id, x, y)
                                        oPart.AddPin(oPin)

                                End Select

                            End If

                        Case XmlNodeType.EndElement

                            'If oXmlReader.Name.Equals("Part") Then
                            '    Debug.WriteLine("_____________________________________________________")

                            '    ' Add the part to the part list
                            '    ' --> initialize part
                            '    ' --> create client graphics
                            '    'oBoard.AddPart(oPart, InteractionEvents)
                            '    'oPart = Nothing
                            '    ' Create new part (delete the old one)
                            '    'oPart = New CircuitPart(MidAddIn, oServer, oBoard)
                            'End If

                            ' Exit Reader after partlist
                            If oXmlReader.Name.Equals("PartList") Then
                                Exit Do
                            End If

                    End Select
                Loop

                oXmlReader.Close()


                For i As Integer = 0 To oBoard.PartList.Count - 1
                    For j As Integer = 0 To oBoard.PartList.Item(i).PinList.Count - 1
                        Debug.Write(oBoard.PartList.Item(i).PinList.Item(j).Id)
                        Debug.Write(", ")
                    Next
                    Debug.WriteLine("")
                    Debug.WriteLine("_________________________________________")
                Next
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub

    ' Parse Netlist
    '***************************************************************************************************************
    Private Sub ParseNetlist()

        Try
            Dim id, partId, pinId As String
            Dim oPin As CircuitPin
            Dim oPart As CircuitPart

            Using oXmlReader As XmlReader = New XmlTextReader(strFilePath)

                Dim oNet As CircuitNet = Nothing 'New CircuitNet(MidAddIn, oServer, oBoard)
                Dim oContact As NetContact = Nothing

                Do While (oXmlReader.Read())

                    Select Case oXmlReader.NodeType

                        ' Node Element
                        Case XmlNodeType.Element
                            If oXmlReader.HasAttributes() Then

                                Select Case oXmlReader.Name

                                    Case "Net"

                                        Debug.WriteLine(oXmlReader.Name)
                                        While oXmlReader.MoveToNextAttribute()
                                            If String.Equals(oXmlReader.Name, "id") Then
                                                Debug.WriteLine(oXmlReader.Name & " = " & oXmlReader.Value)
                                                id = oXmlReader.Value
                                            End If
                                        End While

                                        ' Create new circuit net and add it to the netlist
                                        oNet = New CircuitNet(MidAddIn, oServer, oBoard, id)
                                        oBoard.AddNet(oNet)

                                    Case "Contact"

                                        Debug.WriteLine(oXmlReader.Name)
                                        While oXmlReader.MoveToNextAttribute()
                                            If String.Equals(oXmlReader.Name, "part") Then
                                                Debug.WriteLine(oXmlReader.Name & " = " & oXmlReader.Value)
                                                partId = oXmlReader.Value
                                            End If
                                            If String.Equals(oXmlReader.Name, "pin") Then
                                                Debug.WriteLine(oXmlReader.Name & " = " & oXmlReader.Value)
                                                pinId = oXmlReader.Value
                                            End If
                                        End While

                                        ' Create new contact and add it to the contactlist
                                        oPart = oBoard.FindPartById(partId)
                                        oPin = oPart.FindPinById(pinId)
                                        oContact = New NetContact(MidAddIn, oPart, oPin)
                                        oNet.AddContact(oContact)

                                End Select
                            End If

                        Case XmlNodeType.EndElement
                            'If oXmlReader.Name.Equals("Net") Then
                            '    Debug.WriteLine("_____________________________________________________")

                            '    ' Add the new created connection to the connection list
                            '    'oBoard.AddNet(oNet, oInteractionEvents)
                            '    'oNets.Add(oNet)

                            '    ' Create a new connection
                            '    'oNet = New CircuitNet(MidAddIn, oServer, oBoard)

                            'End If
                            If oXmlReader.Name.Equals("NetList") Then
                                Debug.WriteLine("_____________________________________________________")

                                ' Add the new created connection to the connection list
                                'oBoard.AddNet(oNet, oInteractionEvents)
                                'oNets.Add(oNet)

                                ' Create a new connection
                                'oNet = New CircuitNet(MidAddIn, oServer, oBoard)
                                Exit Do
                            End If
                    End Select
                Loop

                oXmlReader.Close()
            End Using



            'For i As Integer = 0 To oNets.Count() - 1
            '    oNets.Item(i).Initialize()
            'Next

            'For i As Integer = 0 To oNets.Count - 1
            '    For j As Integer = 0 To oNets.Item(i).GetContacts.Count - 1
            '        'Debug.Write(oNets.Item(i).GetContacts.Item(j).GetPartId)
            '        Debug.Write(", ")
            '    Next
            '    Debug.WriteLine("")
            '    Debug.WriteLine("_________________________________________")
            'Next
            ' --> import finished here!

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub

    '##################################################
    ' OTHER
    '##################################################

    ' Create minitoolbar
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

        Dim oDescriptionLabel As MiniToolbarControl = oControls.AddLabel("Description", "Use arrow buttons to move the circuit board", "")
        oControls.AddNewLine()

        ' Button #1
        'Dim mtbAddPicture As stdole.IPictureDisp = MIDProjectVB3.PictureConverter.ImageToPictureDisp(My.Resources.mtbAddSmall1)
        Dim mtbExitPicture As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.mtbExitSmall1)
        Dim mtbRmvPicture As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.mtbRmvSmall1)

        Dim mtbButtonUpPicL As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.MtbButtonUpLarge)
        Dim mtbButtonUpPicS As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.MtbButtonUpSmall)

        Dim mtbButtonDownPicL As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.MtbButtonDownLarge)
        Dim mtbButtonDownPicS As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.MtbButtonDownSmall)

        Dim mtbButtonLeftPicL As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.MtbButtonLeftLarge)
        Dim mtbButtonLeftPicS As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.MtbButtonLeftSmall)

        Dim mtbButtonRightPicL As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.MtbButtonRightLarge)
        Dim mtbButtonRightPicS As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.MtbButtonRightSmall)


        ' remark: use only 16x16px Images for StandardIcon
        oControls.AddLabel("XInternal", "X ", "move board in x direction")
        oMtbButtonDownX = oControls.AddButton("moveXDownInternal", "", "Down", mtbButtonLeftPicS, mtbButtonLeftPicL)
        oMtbButtonUpX = oControls.AddButton("moveXUpInternal", "", "Up", mtbButtonRightPicS, mtbButtonRightPicL)
        oControls.AddSeparator()
        oControls.AddLabel("YInternal", "Y ", "move board in y direction")
        oMtbButtonDownY = oControls.AddButton("moveYDownInternal", "", "Down", mtbButtonLeftPicS, mtbButtonLeftPicL)
        oMtbButtonUpY = oControls.AddButton("moveYUpInternal", "", "Up", mtbButtonRightPicS, mtbButtonRightPicL)
        oControls.AddSeparator()
        oControls.AddLabel("ZInternal", "Z ", "move board in z direction")
        oMtbButtonDownZ = oControls.AddButton("moveZDownInternal", "", "Down", mtbButtonLeftPicS, mtbButtonLeftPicL)
        oMtbButtonUpZ = oControls.AddButton("moveZUpInternal", "", "Up", mtbButtonRightPicS, mtbButtonRightPicL)
        oControls.AddNewLine()
        oMtbAlignButton = oControls.AddDropdown("AlignInternal", False, True, True, True)

        oMtbAlignButton.AddItem("Align To Face", "Align To Face", "AlignToFaceInternal", False, False, , , 1)
        oMtbAlignButton.AddItem("Align To Edge", "Align To Edge", "AlignToEdgeInternal", False, False, , , 2)
        oControls.AddNewLine()

        oMtbButtonApply = oControls.AddButton("ApplyOccInternal", "Accept", "Accept to place import components", mtbExitPicture, mtbExitPicture)

        ' Create Minitoolbar on the upper left of the window
        Dim oPosition As Point2d

        If oReadNetlistCmdDlg IsNot Nothing Then
            oPosition = MidAddIn.TransientGeometry.CreatePoint2d(oReadNetlistCmdDlg.Location.X + dlgOffset, dlgOffset)
        Else
            oPosition = MidAddIn.TransientGeometry.CreatePoint2d(0, 0)
        End If

    End Sub




    'Public ReadOnly Property FolderPath() As String
    '    Get
    '        Return strFolderPath
    '    End Get
    'End Property

    'Public ReadOnly Property CircuitBoard() As CircuitBoard
    '    Get
    '        Return oBoard
    '    End Get
    'End Property

    'Public ReadOnly Property InteractionEvents As InteractionEvents
    '    Get
    '        Return oInteractionEvents
    '    End Get
    'End Property


End Class


' Check Interference
'***************************************************************************************************************
'Private Sub CheckInterference(ByVal oSurfaceBody1 As SurfaceBody, _
' ByVal oSurfaceBody2 As SurfaceBody)


'Dim oBox1 As Box = oSurfaceBody1.RangeBox
'Dim oBox2 As Box = oSurfaceBody2.RangeBox

'Dim oMaxPoint1 As Point = oBox1.MaxPoint
'Dim oMinPoint1 As Point = oBox1.MinPoint

'Dim oMaxPoint2 As Point = oBox2.MaxPoint
'Dim oMinPoint2 As Point = oBox2.MinPoint

'If oMaxPoint1.X < oMaxPoint2.X Then


'' Calculate interference
'Dim oObj1 As ObjectCollection = MidAddIn.TransientObjects.CreateObjectCollection
'Dim oObj2 As ObjectCollection = MidAddIn.TransientObjects.CreateObjectCollection

'oObj1.Add(oCompOcc)
'For Each oOcc As ComponentOccurrence In oAsmDoc.ComponentDefinition.Occurrences
'    If oOcc IsNot oCompOcc Then
'        oObj2.Add(oAsmDoc.ComponentDefinition.Occurrences.Item(1))
'    End If
'Next

'Dim oResults As InterferenceResults = oAsmDoc.ComponentDefinition.AnalyzeInterference(oObj1, oObj2)
'' Create highlight set
'Dim oHS As HighlightSet = oAsmDoc.CreateHighlightSet()
'Dim oColor As Color

'If oResults.Count > 0 Then
'    oColor = MidAddIn.TransientObjects.CreateColor(255, 33, 0) '###global color definition
'    oHS.Color = oColor
'    ' Disable apply button
'    oMtbButtonApply.Enabled = False
'Else
'    oColor = MidAddIn.TransientObjects.CreateColor(33, 200, 0)
'    oHS.Color = oColor
'    ' Enable apply button
'    oMtbButtonApply.Enabled = True
'End If

'oHS.AddItem(oCompOcc)

'End Sub



'Private Sub CreateComponentDirectory()
'Try
'    ' Get predefined path
'    strFilePath = _Server.CommandCollection.WorkDirectory & "\Components"
'    ' If the directory does not exist already, create a new one
'    If Not (Directory.Exists(strFilePath)) Then
'        If Directory.Exists(_Server.CommandCollection.WorkDirectory) Then

'            ' Create directory only once
'            Dim oDirectory As DirectoryInfo = Directory.CreateDirectory(strFilePath)

'            ' Inform the user that a new directory has been created
'            MessageBox.Show("A new component direcotry has been created.", "MID Project", MessageBoxButtons.OK, MessageBoxIcon.Information)
'        Else
'            MessageBox.Show("The work directory has been deleted, please restore it", "MIDProject", MessageBoxButtons.OK, MessageBoxIcon.Error)
'        End If
'    End If

'Catch ex As Exception
'    MessageBox.Show("Could not create work directory", "MIDProject", MessageBoxButtons.OK, MessageBoxIcon.Error)
'End Try
'End Sub


' Triad events
'***************************************************************************************************************
'Public Overrides Sub OnEndSequence(cancelled As Boolean, _
'                                  coordinateSystem As Matrix, _
'                                  context As NameValueMap, _
'                                  ByRef handlingCode As HandlingCodeEnum)
'    'MsgBox("OnEndSequence")
'    'DisableInteraction()
'    'oBoard.GetOccurrence.Visible = False
'    'createParts()

'End Sub

'Public Overrides Sub OnTerminate(cancelled As Boolean, _
'                                   context As NameValueMap, _
'                                   handlingCode As HandlingCodeEnum)

'    MsgBox("OnTerminate")
'    'DisableInteraction()
'    oBoard.GetOccurrence.Visible = False
'    createParts()

'End Sub


'Public Overrides Sub OnMove(selectedTriadSegment As TriadSegmentEnum, _
'                            shiftKeys As ShiftStateEnum, _
'                            coordinateSystem As Matrix, _
'                            context As NameValueMap, _
'                            handlingCode As HandlingCodeEnum)

'    coordinateSystem.Cell(1, 4) += oBoard.Length / 2
'    coordinateSystem.Cell(3, 4) += oBoard.Width / 2
'    oBoard.Position = coordinateSystem


'End Sub
' Get the transient B-Rep and Geometry objects.
'Dim tBRep As TransientBRep
'tBRep = MidAddIn.TransientBRep

'Dim tg As TransientGeometry
'tg = MidAddIn.TransientGeometry

'' Create a new surface body definition.
'Dim bodyDef As SurfaceBodyDefinition= tBRep.CreateSurfaceBodyDefinition

'' Add a lump, shell, and wire.
'Dim lumpDef As LumpDefinition = bodyDef.LumpDefinitions.Add()

'Dim shellDef As FaceShellDefinition = lumpDef.FaceShellDefinitions.Add()

'Dim wireDef As WireDefinition = shellDef.WireDefinitions.Add()

'' Create coordinate points and use those to create vertex definitions.
'Dim pnts(2) As Point
'pnts(0) = tg.CreatePoint(0, 0, 0)
'pnts(1) = tg.CreatePoint(10, 3, 0)
'pnts(2) = tg.CreatePoint(20, 0, 0)

'Dim vertexDefs(2) As VertexDefinition
'vertexDefs(0) = bodyDef.VertexDefinitions.Add(pnts(0))
'vertexDefs(1) = bodyDef.VertexDefinitions.Add(pnts(1))
'vertexDefs(2) = bodyDef.VertexDefinitions.Add(pnts(2))

'' Create two wire edges, passing through the three vertices.
'wireDef.WireEdgeDefinitions.Add(vertexDefs(0), vertexDefs(1), tg.CreateLineSegment(pnts(0), pnts(1)))
'wireDef.WireEdgeDefinitions.Add(vertexDefs(1), vertexDefs(2), tg.CreateLineSegment(pnts(1), pnts(2)))

'' Create a second wire definition.
'Dim wireDef2 As WireDefinition = shellDef.WireDefinitions.Add

'' Create coordinate points and use those to create vertex definitions.
'pnts(0) = tg.CreatePoint(-5, 0, 10)
'pnts(1) = tg.CreatePoint(10, 6, 10)
'pnts(2) = tg.CreatePoint(25, 0, 10)

'vertexDefs(0) = bodyDef.VertexDefinitions.Add(pnts(0))
'vertexDefs(1) = bodyDef.VertexDefinitions.Add(pnts(1))
'vertexDefs(2) = bodyDef.VertexDefinitions.Add(pnts(2))

'' Create two edges, passing through the three vertices.
'Call wireDef2.WireEdgeDefinitions.Add(vertexDefs(0), vertexDefs(1), tg.CreateLineSegment(pnts(0), pnts(1)))
'Call wireDef2.WireEdgeDefinitions.Add(vertexDefs(1), vertexDefs(2), tg.CreateLineSegment(pnts(1), pnts(2)))

'' Create a body using the defined wires.
'Dim errors As NameValueMap = MidAddIn.TransientObjects.CreateNameValueMap
'Dim body1 As SurfaceBody = bodyDef.CreateTransientSurfaceBody(errors)

'' Create a ruled surface between the two wire bodies.
'Dim ruled As SurfaceBody = tBRep.CreateRuledSurface(body1.Wires.Item(1), body1.Wires.Item(2))

'' Get the part component definition of the active document.
'Dim partDoc As PartDocument = MidAddIn.Documents.Add(DocumentTypeEnum.kPartDocumentObject, , False)
'Dim partDef As PartComponentDefinition = partDoc.ComponentDefinition

'' Create a base body feature of the transient body.
'Dim baseBody As NonParametricBaseFeature = partDef.Features.NonParametricBaseFeatures.Add(ruled)

'' Change the result work surface so it's not translucent.
'baseBody.SurfaceBodies.Item(1).Parent.Translucent = False

'MidAddIn.ActiveView.Fit()

'Dim tBRep As TransientBRep
'tBRep = MidAddIn.TransientBRep
'Dim tg As TransientGeometry
'tg = MidAddIn.TransientGeometry

'Dim oPoint1 As Point = oTG.CreatePoint(-4, 0, 0)

'Dim oPoint2 As Point = oTG.CreatePoint(4, 0, 0)
'Dim oBody As SurfaceBody = tBRep.CreateSolidCylinderCone(oPoint1, oPoint2, 0.001, 0.001, 0.001)

'Dim baseBody As NonParametricBaseFeature = partDef.Features.NonParametricBaseFeatures.Add(oBody)



'Dim oPartDoc As PartDocument = MidAddIn.Documents.Add(DocumentTypeEnum.kPartDocumentObject, , True)
'' Test
'Dim oTransBRep As TransientBRep = MidAddIn.TransientBRep

'Dim oSurfaceBodyDef As SurfaceBodyDefinition = oTransBRep.CreateSurfaceBodyDefinition

' '' Create a lump.
'Dim oLumpDef As LumpDefinition = oSurfaceBodyDef.LumpDefinitions.Add

' '' Create a shell.
'Dim oShell As FaceShellDefinition = oLumpDef.FaceShellDefinitions.Add

'Dim oTG As TransientGeometry = MidAddIn.TransientGeometry

'Dim boardPoint(1) As Point
'boardPoint(0) = oTG.CreatePoint(-4, 0, 0)
'boardPoint(1) = oTG.CreatePoint(4, 0, 0)

'Dim boardVertex(1) As VertexDefinition
'boardVertex(0) = oSurfaceBodyDef.VertexDefinitions.Add(boardPoint(0))
'boardVertex(1) = oSurfaceBodyDef.VertexDefinitions.Add(boardPoint(1))

'Dim boardLineSeg As LineSegment
'boardLineSeg = oTG.CreateLineSegment(boardPoint(0), boardPoint(1))

'Dim boardEdgeDef As EdgeDefinition
'boardEdgeDef = oSurfaceBodyDef.EdgeDefinitions.Add(boardVertex(0), boardVertex(1), boardLineSeg)

'Dim oBoardError As NameValueMap
'Dim oNewBoardBody As SurfaceBody = oSurfaceBodyDef.CreateTransientSurfaceBody(oBoardError)

'Dim oBaseFeature As NonParametricBaseFeature = oPartDoc.ComponentDefinition.Features.NonParametricBaseFeatures.Add(oNewBoardBody)

'Dim oBoardFeatureDef As NonParametricBaseFeatureDefinition = oPartDoc.ComponentDefinition.Features.NonParametricBaseFeatures.CreateDefinition

'Dim oCollection As ObjectCollection = MidAddIn.TransientObjects.CreateObjectCollection

'oCollection.Add(oNewBoardBody)

'oBoardFeatureDef.BRepEntities = oCollection
'oBoardFeatureDef.OutputType = BaseFeatureOutputTypeEnum.kSurfaceOutputType

'Dim oBoardBaseFeature As NonParametricBaseFeature = oPartDoc.ComponentDefinition.Features.NonParametricBaseFeatures.AddByDefinition(oBoardFeatureDef)


'boardPoint(2) = oTG.CreatePoint(length / 2.0, 0, width / 2.0)
'boardPoint(3) = oTG.CreatePoint(-length / 2.0, 0, width / 2.0)

'Dim boardVertex(3) As VertexDefinition
'For i As Integer = 0 To boardPoint.Length - 1
'    boardVertex(i) = oSurfaceBodyDef.VertexDefinitions.Add(boardPoint(i))
'Next

'Dim boardLineSeg(3) As LineSegment
''For i As Integer = 0 To boardLineSeg.Length() - 1
''    boardLineSeg(i) = oTG.CreateLineSegment(boardPoint(i Mod (boardLineSeg.Length() - 1)), boardLineSeg((i + 1) Mod (boardLineSeg.Length() - 1)))
''Next

'boardLineSeg(0) = oTG.CreateLineSegment(boardPoint(0), boardPoint(1))
'boardLineSeg(1) = oTG.CreateLineSegment(boardPoint(1), boardPoint(2))
'boardLineSeg(2) = oTG.CreateLineSegment(boardPoint(2), boardPoint(3))
'boardLineSeg(3) = oTG.CreateLineSegment(boardPoint(3), boardPoint(0))

'Dim boardEdgeDef(3) As EdgeDefinition
'boardEdgeDef(0) = oSurfaceBodyDef.EdgeDefinitions.Add(boardVertex(0), boardVertex(1), boardLineSeg(0))
'boardEdgeDef(1) = oSurfaceBodyDef.EdgeDefinitions.Add(boardVertex(1), boardVertex(2), boardLineSeg(1))
'boardEdgeDef(2) = oSurfaceBodyDef.EdgeDefinitions.Add(boardVertex(2), boardVertex(3), boardLineSeg(2))
'boardEdgeDef(3) = oSurfaceBodyDef.EdgeDefinitions.Add(boardVertex(3), boardVertex(0), boardLineSeg(3))

'Dim boardPlane As Plane = oTG.CreatePlane(boardPoint(0), oTG.CreateVector(0, 1, 0))

'Dim boardFaceDef As FaceDefinition = oShell.FaceDefinitions.Add(boardPlane, False)

'Dim oBoardEdgeLoop As EdgeLoopDefinition = boardFaceDef.EdgeLoopDefinitions.Add
'oBoardEdgeLoop.EdgeUseDefinitions.Add(boardEdgeDef(0), False)
'oBoardEdgeLoop.EdgeUseDefinitions.Add(boardEdgeDef(1), False)
'oBoardEdgeLoop.EdgeUseDefinitions.Add(boardEdgeDef(2), False)
'oBoardEdgeLoop.EdgeUseDefinitions.Add(boardEdgeDef(3), False)

'Dim oBoardError As NameValueMap
'Dim oNewBoardBody As SurfaceBody = oSurfaceBodyDef.CreateTransientSurfaceBody(oBoardError)

'Dim oBaseFeature As NonParametricBaseFeature = oCompDef.Features.NonParametricBaseFeatures.Add(oNewBoardBody)

'Dim oBoardFeatureDef As NonParametricBaseFeatureDefinition = oCompDef.Features.NonParametricBaseFeatures.CreateDefinition

'Dim oCollection As ObjectCollection = oInventorAddin.TransientObjects.CreateObjectCollection

'oCollection.Add(oNewBoardBody)

'oBoardFeatureDef.BRepEntities = oCollection
'oBoardFeatureDef.OutputType = BaseFeatureOutputTypeEnum.kSurfaceOutputType

'Dim oBoardBaseFeature As NonParametricBaseFeature = oCompDef.Features.NonParametricBaseFeatures.AddByDefinition(oBoardFeatureDef)






' Create user coordinate system
'*************************************************************************************************************************
'Private Sub CreateUCS()
'    Dim oCompDef As AssemblyComponentDefinition = oAsmDoc.ComponentDefinition
'    Dim oUCSDef As UserCoordinateSystemDefinition = oCompDef.UserCoordinateSystems.CreateDefinition()
'    oUCS = oCompDef.UserCoordinateSystems.Add(oUCSDef)
'    oUCS.Name = "UCS"
'    oUCS.Visible = False

'End Sub

'Private Function FindPinById(ByRef oPart As CircuitPart, _
'                             ByVal _pinId As String) As CircuitPin
'    For Each oPin In oPart.Pins
'        If oPin.Id.Equals(_pinId) Then
'            Return oPin
'        End If
'    Next
'    Return Nothing
'End Function

'Private Function FindPartById(ByRef oBoard As CircuitBoard, _
'                              ByVal _partId As String) As CircuitPart
'    For Each _part In oBoard.PartList
'        If _part.Id.Equals(_partId) Then
'            Return _part
'        End If
'    Next
'    Return Nothing
'End Function



'Public Overrides Sub OnMouseUp(button As MouseButtonEnum, _
'                                 shiftKeys As ShiftStateEnum, _
'                                 modelPosition As Point, _
'                                 viewPosition As Point2d, _
'                                 view As Inventor.View)
'    Debug.WriteLine("mouse Up")

'    'If button = MouseButtonEnum.kLeftMouseButton Then

'    '    If oSelectEvents.SelectedEntities.Count > 0 Then

'    '        Dim oCompOcc As ComponentOccurrence = oSelectEvents.SelectedEntities.Item(1)

'    '        oCompOcc.Transformation.Cell(1, 4) = modelPosition.X
'    '        oCompOcc.Transformation.Cell(2, 4) = modelPosition.Y
'    '        oCompOcc.Transformation.Cell(3, 4) = modelPosition.Z


'    '    End If

'    'End If

'End Sub

'Public Overrides Sub OnMouseDown(button As MouseButtonEnum, _
'             shiftKeys As ShiftStateEnum, _
'             modelPosition As Point, _
'             viewPosition As Point2d, _
'             view As Inventor.View)
'    Debug.WriteLine("mouse down")
'End Sub


'Public Overrides Sub OnMouseMove(button As MouseButtonEnum, _
'              shiftKeys As ShiftStateEnum, _
'              modelPosition As Point, _
'              viewPosition As Point2d, _
'              view As Inventor.View)



'    ' If button = MouseButtonEnum.kLeftMouseButton Then

'    'If oSelectEvents.SelectedEntities.Count > 0 Then

'    'Dim oCompOcc As ComponentOccurrence = oSelectEvents.SelectedEntities.Item(1)
'    ' Debug.WriteLine("it works perfektely")
'    'oBoard.Transformation.Cell(1, 4) += 0.2

'    'oBoard.Occurrence.Transformation.Cell(2, 4) = modelPosition.Y
'    'oBoard.Occurrence.Transformation.Cell(3, 4) = modelPosition.Z


'    'End If

'    ' End If