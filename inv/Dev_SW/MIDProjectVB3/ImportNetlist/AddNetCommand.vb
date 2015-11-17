Option Explicit On



Imports Inventor
Imports System.Xml
Imports System.Collections.Generic
Imports System.Math
Imports System.Globalization
Imports System.Windows.Forms
Imports System.IO

'##############################################
' CIRCUIT BOARD CLASS
'##############################################

Public Class MyXmlNameTable
    Inherits XmlNameTable

    Public Overloads Overrides Function Add(array() As Char, offset As Integer, length As Integer) As String

    End Function

    Public Overloads Overrides Function Add(array As String) As String

    End Function

    Public Overloads Overrides Function [Get](array() As Char, offset As Integer, length As Integer) As String

    End Function

    Public Overloads Overrides Function [Get](array As String) As String

    End Function
End Class

Public Class AddNetCommand
    Inherits Command

    'Private oXmlReader As XmlTextReader

    Private oMiniToolbar As MiniToolbar
    Private WithEvents oMtbButtonApply As MiniToolbarButton

    Private WithEvents oMtbButtonUpX As MiniToolbarButton
    Private WithEvents oMtbButtonDownX As MiniToolbarButton
    Private WithEvents oMtbButtonUpY As MiniToolbarButton
    Private WithEvents oMtbButtonDownY As MiniToolbarButton
    Private WithEvents oMtbButtonUpZ As MiniToolbarButton
    Private WithEvents oMtbButtonDownZ As MiniToolbarButton

    Private WithEvents oUserInputEvents As UserInputEvents

    Private dlgOffset As Integer = 20

    Private oNewPosMatrix As Matrix
    Private oUCS As UserCoordinateSystem
    ' the board where the components (parts) are located


    Private oBoard As CircuitBoard


    'Private oContact As CircuitContact
    'Private oContacts As List(Of CircuitContact)

    Private oAsmDoc As AssemblyDocument
    Private oTG As TransientGeometry
    Private oUOF As UnitsOfMeasure
    Private oDlg As AddNetCmdDlg

    Private strFilePath As String

    Private initalYVal As Double = 4.0

    Private oServer As MidAddInServer

    'Private sFilePath As String

    ' constructor
    Public Sub New(AddIn As Inventor.Application, server As MidAddInServer)

        MyBase.New(AddIn)

        oServer = server

       

    End Sub



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

        ' Init important inventor classes
        oTG = MidAddIn.TransientGeometry
        oUOF = MidAddIn.ActiveDocument.UnitsOfMeasure
        oAsmDoc = MidAddIn.ActiveDocument

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


        oInteractionEvents.SetCursor(CursorTypeEnum.kCursorBuiltInSelectArrow)



        ' Subscribe to desired interaction event(s)
        MyBase.SubscribeToEvent(Interaction1.InteractionTypeEnum.kMouse)

        'MyBase.SubscribeToEvent(Interaction1.InteractionTypeEnum.kSelection)

        ' Create new form dialog
        oDlg = New AddNetCmdDlg(MidAddIn, Me)

        If oDlg IsNot Nothing Then
            oDlg.TopMost() = True
            oDlg.ShowInTaskbar() = True
            oDlg.Show()
            Dim oView As Inventor.View = MidAddIn.ActiveView()
            oDlg.Location = New System.Drawing.Point(oView.Left + dlgOffset, oView.Top + dlgOffset)
        End If

        ' Create minitoolbar
        CreateMiniToolbar()

        ' Create user coordinate System
        CreateUCS()

        ' Create component directory
        'CreateComponentDirectory()

        ' Enable command specific functions
        EnableInteraction()

    End Sub


    Private Sub CreateComponentDirectory()
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
    End Sub

    ' Stop Command (clean up)
    '***************************************************************************************************************
    Public Overrides Sub StopCommand()
        'Implement this command specific functionality

        'Destroy the command dialog
        oDlg.Hide()
        oDlg.Dispose() ' make it ready for garbage collector
        oDlg = Nothing

        ' Delete mini toolbar
        oMiniToolbar.Delete()

        ' Delete UCS
        oUCS.Delete()

        'base command button's StopCommand (to disconnect interaction sink)
        MyBase.StopCommand()

    End Sub


    'Enable/Disable Interaction
    '***************************************************************************************************************
    Public Overrides Sub EnableInteraction()
        ' Resubscribe all event handlers for the subscribed events
        MyBase.EnableInteraction()

        oUserInputEvents = MidAddIn.CommandManager.UserInputEvents

        oMouseEvents.MouseMoveEnabled = True
        'oSelectEvents.SingleSelectEnabled = True
        'oSelectEvents.AddSelectionFilter(SelectionFilterEnum.kAssemblyOccurrenceFilter)
        ' Disable selection
        oInteractionEvents.SelectionActive = False



    End Sub


    Public Overrides Sub DisableInteraction()
        'base command button's DisableInteraction
        MyBase.DisableInteraction()

        'Implement this command speific functionality
        'Set the default cursor to notify that selection is not active
        oInteractionEvents.SetCursor(CursorTypeEnum.kCursorBuiltInArrow)

        'Clean up status text
        oInteractionEvents.StatusBarText = String.Empty
    End Sub

    ' Check Interference
    '***************************************************************************************************************
    Private Sub CheckInterference(ByRef oCompOcc As ComponentOccurrence)

        ' Calculate interference
        Dim oObj1 As ObjectCollection = MidAddIn.TransientObjects.CreateObjectCollection
        Dim oObj2 As ObjectCollection = MidAddIn.TransientObjects.CreateObjectCollection

        oObj1.Add(oCompOcc)
        For Each oOcc As ComponentOccurrence In oAsmDoc.ComponentDefinition.Occurrences
            If oOcc IsNot oCompOcc Then
                oObj2.Add(oAsmDoc.ComponentDefinition.Occurrences.Item(1))
            End If
        Next

        Dim oResults As InterferenceResults = oAsmDoc.ComponentDefinition.AnalyzeInterference(oObj1, oObj2)
        ' Create highlight set
        Dim oHS As HighlightSet = oAsmDoc.CreateHighlightSet()
        Dim oColor As Color

        If oResults.Count > 0 Then
            oColor = MidAddIn.TransientObjects.CreateColor(255, 33, 0) '###global color definition
            oHS.Color = oColor
            ' Disable apply button
            oMtbButtonApply.Enabled = False
        Else
            oColor = MidAddIn.TransientObjects.CreateColor(33, 200, 0)
            oHS.Color = oColor
            ' Enable apply button
            oMtbButtonApply.Enabled = True
        End If

        oHS.AddItem(oCompOcc)

    End Sub



    ' Import BRep
    '***************************************************************************************************************
    Public Sub ImportBRep(strFilePath As String)

        ' Save xml file path
        Me.strFilePath = strFilePath


        ParseBoard()



        'oBoard = New CircuitBoard(MidAddIn, _Server, strFilePath)


        ' Read circuitboard properties from xml
        'CreateBoard()


        ' Make toolbar visible
        oMiniToolbar.Visible = True

        ' Make UCS visible
        oUCS.Visible = True
        oUCS.Visible() = True
        oUCS.XYPlane().Visible = False
        oUCS.XZPlane().Visible = False
        oUCS.YZPlane().Visible = False
        oUCS.XAxis().Visible = False
        oUCS.YAxis().Visible = False
        oUCS.ZAxis().Visible = False
        oUCS.XZPlane().Visible = False
        oUCS.Origin.Visible = False






        ' re-position user cooridnate system
        MoveComponent(oTG.CreateVector())

        '' Check Interference with other occurrences
        'CheckInterference(oBoard.Occurrence)

    End Sub

    '#############################################################
    ' EVENTS (Minitoolbar)
    '#############################################################

    ' Translate component
    '*************************************************************************************************************************
    Private Sub MoveComponent(oTransVector As Vector)

        oNewPosMatrix = oBoard.Transformation
        oTransVector.TransformBy(oNewPosMatrix)

        oNewPosMatrix.Cell(1, 4) += oTransVector.X
        oNewPosMatrix.Cell(2, 4) += oTransVector.Y
        oNewPosMatrix.Cell(3, 4) += oTransVector.Z

        oBoard.Transformation = oNewPosMatrix

        ' Position UCS in the center of the board    
        oUCS.Transformation = oBoard.Transformation

        ' Check collision of board with other occurrences
        CheckInterference(oBoard.Occurrence)

    End Sub


    Private Sub oMtbButtonUpX_OnClick() Handles oMtbButtonUpX.OnClick
        Dim oTransVector As Vector = MidAddIn.TransientGeometry.CreateVector(0.3, 0, 0)
        MoveComponent(oTransVector)
        'CheckValidPos()

    End Sub

    Private Sub oMtbButtonDownX_OnClick() Handles oMtbButtonDownX.OnClick
        Dim oTransVector As Vector = MidAddIn.TransientGeometry.CreateVector(-0.3, 0, 0)
        MoveComponent(oTransVector)
        'CheckValidPos()
        'CheckInterference(oBoard.Occurrence)

    End Sub

    Private Sub oMtbButtonUpZ_OnClick() Handles oMtbButtonUpZ.OnClick
        Dim oTransVector As Vector = MidAddIn.TransientGeometry.CreateVector(0, 0, 0.3)
        MoveComponent(oTransVector)
        'CheckValidPos()
        'CheckInterference(oBoard.Occurrence)
    End Sub


    Private Sub oMtbButtonDownZ_OnClick() Handles oMtbButtonDownZ.OnClick
        Dim oTransVector As Vector = MidAddIn.TransientGeometry.CreateVector(0, 0, -0.3)
        MoveComponent(oTransVector)
        'CheckValidPos()
        'CheckInterference(oBoard.Occurrence)
    End Sub

    Private Sub oMtbButtonUpY_OnClick() Handles oMtbButtonUpY.OnClick
        Dim oTransVector As Vector = MidAddIn.TransientGeometry.CreateVector(0, 0.3, 0)
        MoveComponent(oTransVector)
        'CheckValidPos()
        'CheckInterference(oBoard.Occurrence)
    End Sub


    Private Sub oMtbButtonDownY_OnClick() Handles oMtbButtonDownY.OnClick
        Dim oTransVector As Vector = MidAddIn.TransientGeometry.CreateVector(0, -0.3, 0)
        MoveComponent(oTransVector)
        'CheckValidPos()
        'CheckInterference(oBoard.Occurrence)
    End Sub


    ' Accept button
    '*************************************************************************************************************************
    Private Sub oMtbButtonApply_OnClick() Handles oMtbButtonApply.OnClick

        ' Hide toolbar and UCS
        oMiniToolbar.Visible = False
        oUCS.Visible = False

        'DisableInteraction()

        ' Make board invisible for easier assembling
        oBoard.Occurrence.Visible = False

        ' Parse parts
        ParseParts()

        ' Parse netlist
        ParseNetlist()

        ' Re-enable the Dialog buttons
        oDlg.buttonOk.Enabled = True
        oDlg.buttonCancel.Enabled = True


    End Sub


    ' Create user coordinate system
    '*************************************************************************************************************************
    Private Sub CreateUCS()
        Dim oCompDef As AssemblyComponentDefinition = oAsmDoc.ComponentDefinition
        Dim oUCSDef As UserCoordinateSystemDefinition = oCompDef.UserCoordinateSystems.CreateDefinition()
        oUCS = oCompDef.UserCoordinateSystems.Add(oUCSDef)
        oUCS.Name = "UCS"
        oUCS.Visible = False

    End Sub

    ' Create mini toolbar
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

        Dim oDescriptionLabel As MiniToolbarControl = oControls.AddLabel("Description", "Use arrow keys to move component", "")
        oControls.AddNewLine()

        ' Button #1
        'Dim mtbAddPicture As stdole.IPictureDisp = MIDProjectVB3.PictureConverter.ImageToPictureDisp(My.Resources.mtbAddSmall1)
        Dim mtbExitPicture As stdole.IPictureDisp = MIDProjectVB3.PictureConverter.ImageToPictureDisp(My.Resources.mtbExitSmall1)
        Dim mtbRmvPicture As stdole.IPictureDisp = MIDProjectVB3.PictureConverter.ImageToPictureDisp(My.Resources.mtbRmvSmall1)

        Dim mtbButtonUpPicL As stdole.IPictureDisp = MIDProjectVB3.PictureConverter.ImageToPictureDisp(My.Resources.MtbButtonUpLarge)
        Dim mtbButtonUpPicS As stdole.IPictureDisp = MIDProjectVB3.PictureConverter.ImageToPictureDisp(My.Resources.MtbButtonUpSmall)

        Dim mtbButtonDownPicL As stdole.IPictureDisp = MIDProjectVB3.PictureConverter.ImageToPictureDisp(My.Resources.MtbButtonDownLarge)
        Dim mtbButtonDownPicS As stdole.IPictureDisp = MIDProjectVB3.PictureConverter.ImageToPictureDisp(My.Resources.MtbButtonDownSmall)

        Dim mtbButtonLeftPicL As stdole.IPictureDisp = MIDProjectVB3.PictureConverter.ImageToPictureDisp(My.Resources.MtbButtonLeftLarge)
        Dim mtbButtonLeftPicS As stdole.IPictureDisp = MIDProjectVB3.PictureConverter.ImageToPictureDisp(My.Resources.MtbButtonLeftSmall)

        Dim mtbButtonRightPicL As stdole.IPictureDisp = MIDProjectVB3.PictureConverter.ImageToPictureDisp(My.Resources.MtbButtonRightLarge)
        Dim mtbButtonRightPicS As stdole.IPictureDisp = MIDProjectVB3.PictureConverter.ImageToPictureDisp(My.Resources.MtbButtonRightSmall)


        ' remark: use only 16x16px Images for StandardIcon
        oControls.AddLabel("XInternal", "X ", "move board in x direction")
        oMtbButtonDownX = oControls.AddButton("moveXDownInternal", "", "Down", mtbButtonLeftPicS, mtbButtonLeftPicL)
        oMtbButtonUpX = oControls.AddButton("moveXUpInternal", "", "Up", mtbButtonRightPicS, mtbButtonRightPicL)
        oControls.AddNewLine()
        oControls.AddLabel("YInternal", "Y ", "move board in y direction")
        oMtbButtonDownY = oControls.AddButton("moveYDownInternal", "", "Down", mtbButtonLeftPicS, mtbButtonLeftPicL)
        oMtbButtonUpY = oControls.AddButton("moveYUpInternal", "", "Up", mtbButtonRightPicS, mtbButtonRightPicL)
        oControls.AddNewLine()
        oControls.AddLabel("ZInternal", "Z ", "move board in z direction")
        oMtbButtonDownZ = oControls.AddButton("moveZDownInternal", "", "Down", mtbButtonLeftPicS, mtbButtonLeftPicL)
        oMtbButtonUpZ = oControls.AddButton("moveZUpInternal", "", "Up", mtbButtonRightPicS, mtbButtonRightPicL)
        oControls.AddNewLine()
        oMtbButtonApply = oControls.AddButton("ApplyOccInternal", "Accept", "Accept to place import components", mtbExitPicture, mtbExitPicture)

        ' Create Minitoolbar on the upper left of the window
        Dim oPosition As Point2d

        If oDlg IsNot Nothing Then
            oPosition = MidAddIn.TransientGeometry.CreatePoint2d(oDlg.Location.X + dlgOffset, dlgOffset)
        Else
            oPosition = MidAddIn.TransientGeometry.CreatePoint2d(0, 0)
        End If

    End Sub


    ' Convert units
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


    ' Parse board data from xml
    '***************************************************************************************************************
    Private Sub ParseBoard()

        oBoard = New CircuitBoard(MidAddIn, oServer)

        Dim oXmlReader As XmlTextReader = New XmlTextReader(strFilePath)

        Do While (oXmlReader.Read())
            Select Case oXmlReader.NodeType

                ' Node Element
                Case XmlNodeType.Element
                    If oXmlReader.HasAttributes() Then

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

                    End If

            End Select
        Loop

        oXmlReader.Close()
        ' Initialize Board (--> create occurrence)
        oBoard.Initialize()

    End Sub


    ' Parse parts
    '***************************************************************************************************************
    Private Sub ParseParts()

        Dim oXmlReader As New XmlTextReader(strFilePath)

        Dim oPart As New CircuitPart(MidAddIn, oServer, oBoard)
        Dim oPin As New CircuitPin(MidAddIn, oPart)

        'Dim id As Object = oXmlReader.NameTable.Add("id")

        Do While (oXmlReader.Read())

            Select Case oXmlReader.NodeType
                'IMPROVE: READ ONLY TILL <Netlist>

                ' Node Element
                Case XmlNodeType.Element
                    If oXmlReader.HasAttributes() Then

                        Select Case oXmlReader.Name

                            Case "Part"
                                Debug.WriteLine(oXmlReader.Name)
                                While oXmlReader.MoveToNextAttribute()              
                                    If String.Equals(oXmlReader.Name, "dev") Then
                                        Debug.WriteLine(oXmlReader.Name & " = " & oXmlReader.Value)
                                        oPart.Dev = oXmlReader.Value
                                    End If
                                    If String.Equals(oXmlReader.Name, "value") Then
                                        Debug.WriteLine(oXmlReader.Name & " = " & oXmlReader.Value)
                                        oPart.Value = oXmlReader.Value
                                    End If
                                    If String.Equals(oXmlReader.LocalName, "id") Then
                                        Debug.WriteLine(oXmlReader.Name & " = " & oXmlReader.Value)
                                        oPart.Id = oXmlReader.Value
                                    End If
                                End While

                            Case "Position"
                                Debug.WriteLine(oXmlReader.Name)
                                While oXmlReader.MoveToNextAttribute()
                                    If String.Equals(oXmlReader.Name, "x") Then
                                        Debug.WriteLine(oXmlReader.Name & " = " & oXmlReader.Value)
                                        oPart.X = ConvertUnit(oXmlReader.Value)
                                    End If
                                    If String.Equals(oXmlReader.Name, "y") Then
                                        Debug.WriteLine(oXmlReader.Name & " = " & oXmlReader.Value)
                                        oPart.Y = ConvertUnit(oXmlReader.Value)
                                    End If
                                    If String.Equals(oXmlReader.Name, "z") Then
                                        Debug.WriteLine(oXmlReader.Name & " = " & oXmlReader.Value)
                                        oPart.Z = ConvertUnit(oXmlReader.Value)
                                    End If
                                End While

                            Case "Shape"
                                Debug.WriteLine(oXmlReader.Name)
                                While oXmlReader.MoveToNextAttribute()
                                    If String.Equals(oXmlReader.Name, "b") Then
                                        Debug.WriteLine(oXmlReader.Name & " = " & oXmlReader.Value)
                                        oPart.Length = ConvertUnit(oXmlReader.Value)
                                    End If
                                    If String.Equals(oXmlReader.Name, "h") Then
                                        Debug.WriteLine(oXmlReader.Name & " = " & oXmlReader.Value)
                                        oPart.Width = ConvertUnit(oXmlReader.Value)
                                    End If
                                End While

                            Case "Pin"
                                Debug.WriteLine(oXmlReader.Name)
                                While oXmlReader.MoveToNextAttribute()
                                    If String.Equals(oXmlReader.Name, "id") Then
                                        Debug.WriteLine(oXmlReader.Name & " = " & oXmlReader.Value)
                                        oPin.Id = oXmlReader.Value
                                    End If

                                    If String.Equals(oXmlReader.Name, "x") Then
                                        Debug.WriteLine(oXmlReader.Name & " = " & oXmlReader.Value)
                                        oPin.X = ConvertUnit(oXmlReader.Value)
                                        Debug.WriteLine(oPin.X)
                                    End If

                                    If String.Equals(oXmlReader.Name, "y") Then
                                        oPin.Y = ConvertUnit(oXmlReader.Value)
                                        Debug.WriteLine(oXmlReader.Name & " = " & oXmlReader.Value)
                                        Debug.WriteLine(oPin.Y)
                                    End If
                                End While

                                ' Add a pin to the current part
                                oPart.AddPin(oPin)

                                ' Create new pin (delete the old one)
                                oPin = New CircuitPin(MidAddIn, oPart)

                        End Select

                    End If

                Case XmlNodeType.EndElement

                    If oXmlReader.Name.Equals("Part") Then
                        Debug.WriteLine("_____________________________________________________")

                        ' Add the part to the part list
                        oBoard.AddPart(oPart)

                        ' Create new part (delete the old one)
                        oPart = New CircuitPart(MidAddIn, oServer, oBoard)
                    End If
            End Select
        Loop

        oXmlReader.Close()

        For i As Integer = 0 To oBoard.Parts.Count - 1
            For j As Integer = 0 To oBoard.Parts.Item(i).Pins.Count - 1
                Debug.Write(oBoard.Parts.Item(i).Pins.Item(j).Id)
                Debug.Write(", ")
            Next
            Debug.WriteLine("")
            Debug.WriteLine("_________________________________________")
        Next

        ' Initialize Parts (--> create occurrences)
        oBoard.InitializeParts()

    End Sub

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
    '    For Each _part In oBoard.Parts
    '        If _part.Id.Equals(_partId) Then
    '            Return _part
    '        End If
    '    Next
    '    Return Nothing
    'End Function

    ' Parse Netlist
    '***************************************************************************************************************
    Private Sub ParseNetlist()

        Dim oXmlReader As XmlReader = New XmlTextReader(strFilePath)

        Dim oNet As New CircuitNet(MidAddIn, oBoard)
        Dim oContact As New CircuitContact(MidAddIn)

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
                                        oNet.Id = oXmlReader.Value

                                    End If
                                End While

                            Case "Contact"
                                Debug.WriteLine(oXmlReader.Name)
                                While oXmlReader.MoveToNextAttribute()
                                    If String.Equals(oXmlReader.Name, "part") Then
                                        Debug.WriteLine(oXmlReader.Name & " = " & oXmlReader.Value)
                                        oContact.PartId = oXmlReader.Value
                                        oContact.Part = oBoard.FindPartById(oContact.PartId)

                                    End If
                                    If String.Equals(oXmlReader.Name, "pin") Then
                                        Debug.WriteLine(oXmlReader.Name & " = " & oXmlReader.Value)
                                        oContact.PinId = oXmlReader.Value
                                        oContact.Pin = oContact.Part.FindPinById(oContact.PinId)
                                    End If
                                End While

                                ' Add the contact to the contact list of a specific connection
                                oNet.AddContact(oContact)

                                ' Create new contact object
                                oContact = New CircuitContact(MidAddIn)

                        End Select


                    End If

                Case XmlNodeType.EndElement
                    If oXmlReader.Name.Equals("Net") Then
                        Debug.WriteLine("_____________________________________________________")

                        ' Add the new created connection to the connection list
                        oBoard.AddNet(oNet)
                        'oNets.Add(oNet)

                        ' Create a new connection
                        oNet = New CircuitNet(MidAddIn, oBoard)

                    End If
            End Select
        Loop

        oXmlReader.Close()

        oBoard.InitializeNets()

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

      


    End Sub


    Public Overrides Sub OnMouseUp(button As MouseButtonEnum, _
                                     shiftKeys As ShiftStateEnum, _
                                     modelPosition As Point, _
                                     viewPosition As Point2d, _
                                     view As Inventor.View)
        Debug.WriteLine("mouse Up")

        'If button = MouseButtonEnum.kLeftMouseButton Then

        '    If oSelectEvents.SelectedEntities.Count > 0 Then

        '        Dim oCompOcc As ComponentOccurrence = oSelectEvents.SelectedEntities.Item(1)

        '        oCompOcc.Transformation.Cell(1, 4) = modelPosition.X
        '        oCompOcc.Transformation.Cell(2, 4) = modelPosition.Y
        '        oCompOcc.Transformation.Cell(3, 4) = modelPosition.Z


        '    End If

        'End If

    End Sub

    Public Overrides Sub OnMouseDown(button As MouseButtonEnum, _
                 shiftKeys As ShiftStateEnum, _
                 modelPosition As Point, _
                 viewPosition As Point2d, _
                 view As Inventor.View)
        Debug.WriteLine("mouse down")
    End Sub


    Public Overrides Sub OnMouseMove(button As MouseButtonEnum, _
                  shiftKeys As ShiftStateEnum, _
                  modelPosition As Point, _
                  viewPosition As Point2d, _
                  view As Inventor.View)



        ' If button = MouseButtonEnum.kLeftMouseButton Then

        'If oSelectEvents.SelectedEntities.Count > 0 Then

        'Dim oCompOcc As ComponentOccurrence = oSelectEvents.SelectedEntities.Item(1)
        ' Debug.WriteLine("it works perfektely")
        'oBoard.Transformation.Cell(1, 4) += 0.2

        'oBoard.Occurrence.Transformation.Cell(2, 4) = modelPosition.Y
        'oBoard.Occurrence.Transformation.Cell(3, 4) = modelPosition.Z


        'End If

        ' End If

    End Sub

    Public ReadOnly Property CircuitBoard() As CircuitBoard
        Get
            Return oBoard
        End Get
    End Property



End Class



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
'wireDef2.WireEdgeDefinitions.Add(vertexDefs(0), vertexDefs(1), tg.CreateLineSegment(pnts(0), pnts(1)))
'wireDef2.WireEdgeDefinitions.Add(vertexDefs(1), vertexDefs(2), tg.CreateLineSegment(pnts(1), pnts(2)))

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








