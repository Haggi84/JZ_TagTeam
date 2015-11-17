Option Strict Off
Option Explicit On

Imports Inventor
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports System.Xml

'############################################
' New Project Command
'############################################

Public Class PlaceMidCommand
    Inherits Command

    Private formOffset As Integer
    Private oServer As MidAddInServer

    Private strFileName As String
    Private oPlaceMidCmdDlg As PlaceMidCmdDlg
    Private unitOfLengths As String
    Private unitOfAngles As String
    Private PrecisionOfAngles As String
    Private PrecisionOfLength As String

    ' Constructor
    '***************************************************************************************************************
    Public Sub New(ByVal MidAddIn As Inventor.Application, _
                   ByVal _Server As MidAddInServer)

        ' call base class constructor
        MyBase.New(MidAddIn)

        Me.oServer = _Server

        oPlaceMidCmdDlg = Nothing
        formOffset = 40

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

        If oServer.MidDataTypes.CircuitCarrier IsNot Nothing Then
            MessageBox.Show("There is already a MID circuit carrier. Please remove it before placing a new one", "MID Project", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        ' stop other instance of this command
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

        ' Selection cursor
        oInteractionEvents.SetCursor(CursorTypeEnum.kCursorTypeDefault)

        ' Create new form dialog
        oPlaceMidCmdDlg = New PlaceMidCmdDlg(MidAddIn, Me)

        If oPlaceMidCmdDlg IsNot Nothing Then
            'oPlaceMidCmdDlg.TopMost() = True
            oPlaceMidCmdDlg.ShowInTaskbar() = True
            oPlaceMidCmdDlg.StartPosition = FormStartPosition.Manual
            Dim oView As Inventor.View = MidAddIn.ActiveView()
            oPlaceMidCmdDlg.Location = New System.Drawing.Point(oView.Left + formOffset, oView.Top + formOffset)
            oPlaceMidCmdDlg.Show()

        End If

        ' Enable command specific functions
        EnableInteraction()

    End Sub

    'Enable Interaction
    '***************************************************************************************************************
    Public Overrides Sub EnableInteraction()

        ' Resubscribe all event handlers for the subscribed events
        MyBase.EnableInteraction()

        ' Disable interaction 
        oInteractionEvents.SelectionActive = False
        oInteractionEvents.InteractionDisabled = False

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

    'Stop Command
    '***************************************************************************************************************
    Public Overrides Sub StopCommand()

        ' Destroy the command dialog
        oPlaceMidCmdDlg.Hide()
        oPlaceMidCmdDlg.Dispose()
        oPlaceMidCmdDlg = Nothing

        ' Stop interaction
        MyBase.StopCommand()

    End Sub


    ' Set export data
    '***************************************************************************************************************
    Public Sub SetExportData(PrecisionOfAngles As String, _
                             PrecisionOfLength As String, _
                             unitOfAngles As String, _
                             unitOfLengths As String)

        Me.PrecisionOfAngles = PrecisionOfAngles
        Me.PrecisionOfLength = PrecisionOfLength
        Me.unitOfAngles = unitOfAngles
        Me.unitOfLengths = unitOfLengths

    End Sub


    ' Get export data
    '***************************************************************************************************************
    Public Sub GetExportData(ByRef precOfAngles As String, _
                             ByRef precOfLengths As String, _
                             ByRef unitOfAngles As String, _
                             ByRef unitOfLengths As String)

        precOfAngles = Me.PrecisionOfAngles
        precOfLengths = Me.PrecisionOfLength
        unitOfLengths = Me.unitOfLengths
        unitOfAngles = Me.unitOfAngles

    End Sub

    ' Execute command
    '***************************************************************************************************************
    Public Overrides Sub ExecuteCommand()

        MyBase.ExecuteCommand()

        ' Stop command
        StopCommand()

        ' Execute change request 
        Dim oPlaceMidRequest As New PlaceMidRequest(Me, MidAddIn, oServer)
        MyBase.ExecuteChangeRequest(oPlaceMidRequest, "MidAddIn:PlaceMidRequest", MidAddIn.ActiveDocument)

    End Sub

    ' Check if an circuitcarrier already exists
    '***************************************************************************************************************
    Private Function FindCircuitCarrier() As Boolean

        Dim oAsmDoc As AssemblyDocument = MidAddIn.ActiveDocument
        Dim oAttribSets As AttributeSets


        For Each oOcc As ComponentOccurrence In oAsmDoc.ComponentDefinition.Occurrences
            oAttribSets = oOcc.AttributeSets

            If oAttribSets.NameIsUsed("circuitcarrier") Then
                Return True
            End If

        Next
        Return False

    End Function

    ' Return Filename for circuit carrier occurrence
    '***************************************************************************************************************
    Public Property FileName As String
        Get
            Return strFileName
        End Get
        Set(value As String)
            strFileName = value
        End Set
    End Property

End Class





'Dim oFileDlg As Inventor.FileDialog
'MidAddIn.CreateFileDialog(oFileDlg)

'oFileDlg.Filter = "Inventor Part File |*ipt"
'oFileDlg.DialogTitle = "Open MID-Part"
'oFileDlg.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
'oFileDlg.CancelError = False


'Dim oFaces As Faces = oCompOcc.SurfaceBodies.Item(1).Faces
'For Each oFace As Face In oFaces

'    oAttribSets = oFace.AttributeSets
'    oAttribSet = oAttribSets.Add("midFace")
'    ' Set all faces available for placing components
'    oAttrib = oAttribSet.Add("isKeepOut", ValueTypeEnum.kIntegerTypfilene, 0) 'Bug: kBoolean doesnt work
'    ' Set routing allowed by default.
'    oAttrib = oAttribSet.Add("routingAllowed", ValueTypeEnum.kIntegerType, 1)
'Next
' oPosMatrix)
'#### check for number of surfacebodies!!! only one allowed

'Dim oFaceStyle As RenderStyle = oAsmDoc.RenderStyles.Item("Default")
' oCompOcc.SetRenderStyle(StyleSourceTypeEnum.kOverrideRenderStyle, oFaceStyle)

'Dim oControls As ControlDefinitions = MidAddIn.CommandManager.ControlDefinitions
'Dim oControl As ControlDefinition
'oControl = oControls.Item("AssemblyMirrorComponentCmd")
'oControl.Enabled = True

'oCmdColl.EnableDefinition(CommandCollection.CommandEnableEnum.kKeepOutEnable)



'InsertChildNode(oAsmDoc, oCompOcc, "Circuit Carrier")

' Enable other commands
'Dim oControls As ControlDefinitions = MidAddIn.CommandManager.ControlDefinitions
'Dim oControl As ControlDefinition
'oControl = oControls.Item("ImportIntern")
'oControl.Enabled = True
'oControl = oControls.Item("KeepOutsIntern")
'oControl.Enabled = True
'oControl = oControls.Item("MoveMidIntern")
'oControl.Enabled = True


'oKeepOutButton.Enabled = True
'oExportButton.Enabled = True
'oImportButton.Enabled = True
''oExportSetButton.Enabled = True
'oPlaceCompButton.Enabled = True

' Create new form dialog
'Dim oDlg As New NewProjectCmdDlg(MidAddIn, Me)
'oDlg.ShowInTaskbar() = True
'oDlg.TopMost() = True

' Show dialog and get the path
'strFolderPath = oDlg.SetPath()

'Dim oAsmDoc As AssemblyDocument = MidAddIn.ActiveDocument
'If oAsmDoc IsNot Nothing Then
'    oAsmDoc.Close(False)
'End If

'oAsmDoc = MidAddIn.Documents.Add(DocumentTypeEnum.kAssemblyDocumentObject)
'oAsmDoc.EnvironmentManager.SetCurrentEnvironment(MidAddIn.UserInterfaceManager.Environments.Item("OPTAVER"))


' Stop Command
'StopCommand()

'oPlaceButton.Enabled = True

' Not supportet yet
'Dim assetLib1 As AssetLibrary = MidAddIn.AssetLibraries.Item("Autodesk Appearance Library")
'Dim locAsset1 As Asset = assetLib1.AppearanceAssets.Item("Dark Red")
'oAppearance = locAsset1.CopyTo(oAsmDoc)
