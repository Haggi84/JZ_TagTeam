Option Explicit On

Imports System
Imports System.Drawing
Imports System.Reflection


'###########################################
' Commands 
'###########################################

Public Class Commands

    Private oUserInterfaceEvents As UserInterfaceEvents

    ' Collecion of all commands
    Private oNewProjectCmd As Command
    Private oKeepOutCmd As Command
    Private oAddNetCmd As Command
    Private oPlacePartCmd As Command
    Private oPlaceMidCmd As Command
    Private oMoveMidCmd As Command
    Private oReadKeepOutCmd As Command
    Private oLoadProjectCmd As Command
    Private oOpticalWaveguideCmd As Command
    Private oExportToRaytraceCmd As Command
    Private oCallDatabaseCmd As Command


    Private oCircuitCarrierNode As BrowserNode
    Private oCircuitBoardNode As BrowserNode

    Private oAddIn As Inventor.Application

    Private _WorkDirectory As String
    Private oServer As MidAddInServer

    ' Constructor
    '*********************************************************************************************
    Public Sub New(ByVal oAddIn As Inventor.Application, ByVal oServer As MidAddInServer)

        MyBase.New()

        Me.oAddIn = oAddIn
        'Me.strClientId = strClientId
        Me.oServer = oServer

    End Sub

    ' Init commands
    '********************************************************************************************************************
    Public Sub Initialize()

        ' Get the command category
        Dim oCmdCategory As CommandCategory = oAddIn.CommandManager.CommandCategories.Add("MidProject", "MidProjectInternal", oServer.ClientId)
        ' Create change definition
        Dim oChangeManager As ChangeManager = oAddIn.ChangeManager
        ' Create new change definition collection
        Dim oChangeDefinitions As ChangeDefinitions = oChangeManager.Add(oServer.ClientId)

        ' New Project command
        '********************************************************************************************************************
        Dim newProjectButton As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.NewProjectButton)

        oNewProjectCmd = New NewProjectCommand(oAddIn, oServer)
        oNewProjectCmd.CreateButton("New" & vbNewLine & "Project", "newProjectIntern", Inventor.CommandTypesEnum.kQueryOnlyCmdType, , , "Create a new MID-Project", newProjectButton, newProjectButton)
        oCmdCategory.Add(oNewProjectCmd.ButtonDefinition)
        oNewProjectCmd.ButtonDefinition.Enabled = True

        Dim oNewProjectChangeDefinition As ChangeDefinition = oChangeDefinitions.Add("MidAddIn:NewProjectRequest", "New Project")

        ' Load Project command
        '********************************************************************************************************************
        Dim loadProjectButton As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.ReadButton)

        oLoadProjectCmd = New LoadProjectCommand(oAddIn, oServer)
        oLoadProjectCmd.CreateButton("Load" & vbNewLine & "Project", "loadProjectIntern", Inventor.CommandTypesEnum.kQueryOnlyCmdType, , , "Load a MID-Project", loadProjectButton, loadProjectButton)
        oCmdCategory.Add(oLoadProjectCmd.ButtonDefinition)
        oLoadProjectCmd.ButtonDefinition.Enabled = True

        Dim oLoadProjectChangeDefinition As ChangeDefinition = oChangeDefinitions.Add("MidAddIn:LoadProjectRequest", "Load Project")

        ' Place cirucit carrier command
        '********************************************************************************************************************
        Dim placeMidButton As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.PlaceMidButton)

        oPlaceMidCmd = New PlaceMidCommand(oAddIn, oServer)
        oPlaceMidCmd.CreateButton("Place" & vbNewLine & "MID", "placeMidIntern", Inventor.CommandTypesEnum.kQueryOnlyCmdType, , "Place circuit carrier", , placeMidButton, placeMidButton)
        oCmdCategory.Add(oPlaceMidCmd.ButtonDefinition)

        Dim oPlaceMidChangeDefinition As ChangeDefinition = oChangeDefinitions.Add("MidAddIn:PlaceMidRequest", "Place MID")

        ' Keep-Out from file
        '********************************************************************************************************************
        Dim smallPicture As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.MySmallImage)
        Dim largePicture As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.MyLargeImage)

        oReadKeepOutCmd = New ReadKeepOutCommand(oAddIn, oServer)
        oReadKeepOutCmd.CreateButton("Load" & vbNewLine & "KeepOuts", "LoadKeepOutsIntern", Inventor.CommandTypesEnum.kShapeEditCmdType, oServer.ClientId, , , smallPicture, largePicture)
        oCmdCategory.Add(oReadKeepOutCmd.ButtonDefinition)

        Dim oReadKeepOutChangeDef As ChangeDefinition = oChangeDefinitions.Add("MidAddIn:ReadKeepOutRequest", "Add KeepOuts From File")

        ' Keep-Out command
        '********************************************************************************************************************
        Dim keepOutsButton As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.KeepOutsButton)

        oKeepOutCmd = New KeepOutCommand(oAddIn, oServer)
        oKeepOutCmd.CreateButton("Select" & vbNewLine & "KeepOuts", "KeepOutsIntern", Inventor.CommandTypesEnum.kShapeEditCmdType, oServer.ClientId, , "Select Keep-Outs", keepOutsButton, keepOutsButton)
        oCmdCategory.Add(oKeepOutCmd.ButtonDefinition)

        Dim oKeepOutChangeDef As ChangeDefinition = oChangeDefinitions.Add("MidAddIn:KeepOutReqeust", "Add KeepOuts")

        ' Read netliste command
        '********************************************************************************************************************
        Dim readNetlistButton As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.ReadButton)

        oAddNetCmd = New ReadNetlistCommand(oAddIn, oServer)
        oAddNetCmd.CreateButton("Read" & vbNewLine & "Netlist", "ReadNetlistIntern", Inventor.CommandTypesEnum.kFileOperationsCmdType, oServer.ClientId, , "Read netlist from xml-file", readNetlistButton, readNetlistButton)
        oCmdCategory.Add(oAddNetCmd.ButtonDefinition)

        Dim oAddNetlistChangeDef As ChangeDefinition = oChangeDefinitions.Add("MidAddIn:AddNetlistRequest", "Add Netlist")

        ' Place part command
        '********************************************************************************************************************
        Dim placePartButton As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.PlacePartButton)

        oPlacePartCmd = New PlacePartCommand(oAddIn, oServer)
        oPlacePartCmd.CreateButton("Place" & vbNewLine & "Parts", "PlacePartIntern", Inventor.CommandTypesEnum.kShapeEditCmdType, oServer.ClientId, , "Place parts ontop of the circuit carrier", placePartButton, placePartButton)
        oCmdCategory.Add(oPlacePartCmd.ButtonDefinition)

        Dim oPlacePartCmdChangeDefinition As ChangeDefinition = oChangeDefinitions.Add("MidAddIn:PlacePartRequest", "Place Parts")

        ' Rotate mid command
        '********************************************************************************************************************
        Dim rotateMidButton As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.MovePartButton)

        oMoveMidCmd = New RotateMidCommand(oAddIn, oServer)
        oMoveMidCmd.CreateButton("Rotate" & vbNewLine & "MID", "RotateMidIntern", Inventor.CommandTypesEnum.kFileOperationsCmdType, oServer.ClientId, , "Rotate MID", rotateMidButton, rotateMidButton)
        oCmdCategory.Add(oMoveMidCmd.ButtonDefinition)

        Dim oMoveMidChangeDefinition As ChangeDefinition = oChangeDefinitions.Add("MidAddIn:MoveMidRequest", "Move MID Device")


        ' Optical Waveguide command
        '********************************************************************************************************************
        Dim OpticalWaveguideButton As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.Interconnect)
        ' Dim OpticalWaveguideButton As New Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("Interconnect"))
        oOpticalWaveguideCmd = New OpticalWaveguideCommand(oAddIn, oServer)
        oOpticalWaveguideCmd.CreateButton("Optical" & vbNewLine & "Waveguide", "OpticalWaveguideIntern", Inventor.CommandTypesEnum.kQueryOnlyCmdType, , , "Create an Optical Waveguide", OpticalWaveguideButton, OpticalWaveguideButton)
        oCmdCategory.Add(oOpticalWaveguideCmd.ButtonDefinition)
        ' oNewProjectCmd.ButtonDefinition.Enabled = True

        Dim oOpticalWaveguideChangeDefinition As ChangeDefinition = oChangeDefinitions.Add("MidAddIn:OpticalWaveguideRequest", "Create Optical Waveguide")

        ' Export to Raytrace command
        '********************************************************************************************************************
        Dim ExportToRaytraceButton As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.Raytrace)
        'Dim ExportToRaytraceButton As New Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("RAYTRACE.ico"))


        oExportToRaytraceCmd = New ExportToRaytraceCommand(oAddIn, oServer)
        oExportToRaytraceCmd.CreateButton("Export to" & vbNewLine & "Raytrace", "ExportToRaytraceIntern", Inventor.CommandTypesEnum.kQueryOnlyCmdType, , , "Export Waveguide Geometry to AUT", ExportToRaytraceButton, ExportToRaytraceButton)
        oCmdCategory.Add(oExportToRaytraceCmd.ButtonDefinition)
        oExportToRaytraceCmd.ButtonDefinition.Enabled = True

        Dim oExportToRaytraceChangeDefinition As ChangeDefinition = oChangeDefinitions.Add("MidAddIn:ExportToRaytraceRequest", "Export Waveguide to AUT format")

        'Dim CallDatabaseButton As New Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("ContextCenter.ico"))
        Dim CallDatabaseButton As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.ContextCenter)
        oCallDatabaseCmd = New CallDatabaseCommand(oAddIn, oServer)
        oCallDatabaseCmd.CreateButton("Call" & vbNewLine & "Database", "CallDatabaseIntern", Inventor.CommandTypesEnum.kQueryOnlyCmdType, , , "Call Database", CallDatabaseButton, CallDatabaseButton)
        oCmdCategory.Add(oCallDatabaseCmd.ButtonDefinition)
        oCallDatabaseCmd.ButtonDefinition.Enabled = True


        Dim oCallDatabaseDefinition As ChangeDefinition = oChangeDefinitions.Add("MidAddIn:CallDataBaseRequest", "Call Database")
    End Sub

    ' PROPERTIES
    '********************************************************************************************************************
    Public Property WorkDirectory() As String
        Get
            Return _WorkDirectory
        End Get
        Set(value As String)
            _WorkDirectory = value
        End Set
    End Property

    Public Property CircuitCarrierNode As BrowserNode
        Get
            Return oCircuitCarrierNode
        End Get
        Set(value As BrowserNode)
            oCircuitCarrierNode = value
        End Set
    End Property

    Public Property CircuitBoardNode As BrowserNode
        Get
            Return oCircuitBoardNode
        End Get
        Set(value As BrowserNode)
            oCircuitBoardNode = value
        End Set
    End Property

    Public ReadOnly Property KeepOutCommand() As KeepOutCommand
        Get
            Return oKeepOutCmd
        End Get
    End Property

    Public ReadOnly Property AddNetCommand() As ReadNetlistCommand
        Get
            Return oAddNetCmd
        End Get
    End Property

    Public ReadOnly Property MoveMidCommand() As RotateMidCommand
        Get
            Return oMoveMidCmd
        End Get
    End Property

    Public ReadOnly Property PlaceCommand() As PlaceMidCommand
        Get
            Return oPlaceMidCmd
        End Get
    End Property

    Public ReadOnly Property NewProjectCommand() As NewProjectCommand
        Get
            Return oNewProjectCmd
        End Get
    End Property

    Public ReadOnly Property PlacePartCommand() As PlacePartCommand
        Get
            Return oPlacePartCmd
        End Get
    End Property

    Public ReadOnly Property ReadKeepOutCommand() As ReadKeepOutCommand
        Get
            Return oReadKeepOutCmd
        End Get
    End Property

    Public ReadOnly Property LoadProjectCommand() As LoadProjectCommand
        Get
            Return oLoadProjectCmd
        End Get
    End Property

    Public ReadOnly Property OpticalWaveguideCommand() As OpticalWaveguideCommand
        Get
            Return oOpticalWaveguideCmd
        End Get
    End Property

    Public ReadOnly Property ExportToRaytraceCommand() As ExportToRaytraceCommand
        Get
            Return oExportToRaytraceCmd
        End Get
    End Property

    Public ReadOnly Property CallDatabaseCommand() As CallDatabaseCommand
        Get
            Return oCallDatabaseCmd
        End Get
    End Property
    'Sub AddNodes()



    'End Sub

    'Sub recurse(node As BrowserNode)
    '    If node.Visible Then
    '        Debug.WriteLine(node.BrowserNodeDefinition.Label)
    '        Dim bn As BrowserNode
    '        For Each bn In node.BrowserNodes
    '            recurse(bn)
    '        Next
    '    End If
    'End Sub
    '######################################################
    ' Button Events
    '######################################################

    ' Place Button
    'Private Sub oPlaceButton_onExecute(ByVal Context As NameValueMap) Handles oPlaceButton.OnExecute




    'End Sub

    ' New Project Button
    'Private Sub oProjectButton_OnExecute(ByVal Context As NameValueMap) Handles oProjectButton.OnExecute

    '    Dim oFolderDialog As New FolderDialog(oAddIn)
    '    oFolderDialog.SetPath(strFilePath)
    '    Dim oAsmDoc As AssemblyDocument = oAddIn.ActiveDocument
    '    If oAsmDoc IsNot Nothing Then
    '        oAsmDoc.Close(False)
    '    End If
    '    oAsmDoc = oAddIn.Documents.Add(DocumentTypeEnum.kAssemblyDocumentObject)
    '    oAsmDoc.EnvironmentManager.SetCurrentEnvironment(oAddIn.UserInterfaceManager.Environments.Item("MidAsmEnv"))
    '    'oAsmDoc.EnvironmentManager.OverrideEnvironment = oAddIn.UserInterfaceManager.Environments.Item("MidAsmEnv")
    '    'oRibbonTab.Active = True
    '    oPlaceButton.Enabled = True



    '    'MsgBox("executed")

    'End Sub

    'Private Sub oNewProjectButton_OnExecute(ByVal Context As NameValueMap) Handles oNewProjectButton.OnExecute




    'End Sub

    'Private Sub oKeepOutButton_OnExecute(ByVal Context As NameValueMap) Handles oKeepOutButton.OnExecute

    '    ' Get the transient B-Rep and MID objects.
    '    Dim oPartDoc As AssemblyDocument = oAddIn.ActiveDocument()
    '    oPartDoc.EnvironmentManager.OverrideEnvironment = oMidAsmEnv

    '    Dim oInteraction As New KeepOutSelection(oAddIn)

    '    oInteraction.SelectEntity(SelectionFilterEnum.kPartFaceFilter)


    'End Sub

    ' Import button
    '******************************************************************************************************
    'Private Sub ImportButton_OnExecute(ByVal Context As NameValueMap) Handles oImportButton.OnExecute

    '    Dim oXML As ImportNetCmd = New ImportNetCmd(oAddIn)
    '    oXML.initCommand()

    'End Sub
    '******************************************************************************************************

    'Private Sub oExportButton_OnExecute() Handles oExportButton.OnExecute

    '    Dim oExpSet As ExportSettings = New ExportSettings(oAddIn)
    '    oExpSet.initExport()

    'End Sub

    '' On reset ribbon (for the case the user uses this command)
    'Private Sub UserInterfaceEvents_OnResetRibbonInterface()
    '    'Dim MIDParEnvs As EnvironmentList = oAddIn.UserInterfaceManager.ParallelEnvironments
    '    'oMidAsmEnv = oEnvs.Add("Moulded Interconnected Devices", "MidAsmEnv", , smallEnvPic, largeEnvPic)

    '    Dim oMidAsmEnvButton As ControlDefinition = oAddIn.CommandManager.ControlDefinitions.Item("MidAsmEnv")

    '    strTabs(0) = "MidInternAsm"

    '    ' Attach ribbon tab to the environment
    '    oMidAsmEnv.AdditionalVisibleRibbonTabs = strTabs

    '    'Make the "SomeAnalysis" tab default for the environment
    '    oMidAsmEnv.DefaultRibbonTab = "MidInternAsm"

    '    ' Remove the environment if no part or assembly environment open
    '    Dim oEnvs As Environments = oAddIn.UserInterfaceManager.Environments
    '    For Each oEnv As Inventor.Environment In oEnvs
    '        If Not oEnv.InternalName = "PMxPartEnvironment" Then
    '            'On Error Resume Next
    '            ' oEnv.DisabledCommandList.Add(oPartEnvButton)
    '        End If
    '        If Not oEnv.InternalName = "AMxAssemblyEnvironment" Then
    '            'oEnv.DisabledCommandList.Add(oMidAsmEnvButton)
    '        End If
    '    Next
    'End Sub

End Class


'oPanelSelect.CommandControls.AddButton(oPlaceCompButton, True)

'oImportButton.Enabled = False
'AddHandler oPlaceCompButton.OnExecute, AddressOf Me.oPlaceCompButton_OnExecute
'********************************************************************************************************************

'Add the Assmebly Place Button
' Dim MIDPanelTwo As RibbonPanel = oRibbonTab.RibbonPanels.Add("Panel Two", "PanelTwo", m_clientID)
'Dim oPlaceButton As ButtonDefinition = MIDApplication.CommandManager.ControlDefinitions.Item("AssemblyPlaceComponentCmd")
'oPanelManage.CommandControls.AddButton(oPlaceButton, True)


' Export Button
'********************************************************************************************************************
'Dim exportPicSmall As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.RibbonExportSmall)
'Dim exportPicLarge As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.RibbonExportLarge)

'oControlDefs.AddButtonDefinition("Export" & vbNewLine & "finished project", "ExportIntern", Inventor.CommandTypesEnum.kQueryOnlyCmdType, "clientid", "Translate the finished project to destination folder", , exportPicSmall, exportPicLarge)
''oExportButton.Enabled = False
'oCmdCategory.Add(oExportButton)

'oPanelExport.CommandControls.AddButton(oExportButton, True)
'********************************************************************************************************************

'' Export Settings Button
''********************************************************************************************************************
'Dim exportSetPicSmall As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.RibbonESettingsButtonSmall)
'Dim exportSetPicLarge As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.RibbonESettingsButtonLarge)

'oExportSetButton = oControlDefs.AddButtonDefinition("Export" & vbNewLine & "Settings", "ExportSettingsIntern", _
'                                               CommandTypesEnum.kFileOperationsCmdType, _
'                                               "clientid", "Translate the finished project to destination folder", , _
'                                               exportSetPicSmall, exportSetPicLarge)

'oExportSetButton.Enabled = False
'oCmdCategory.Add(oExportButto

'###########################################################################
' Picture converter class
'###########################################################################
'<System.ComponentModel.DesignerCategory("")> Friend Class PictureConverter
'    Inherits System.Windows.Forms.AxHost


'    Private Sub New()
'        MyBase.New(String.Empty)
'    End Sub

'    Public Shared Function ImageToPictureDisp( _
'                           ByVal image As System.Drawing.Image) As stdole.IPictureDisp
'        Return CType(GetIPictureDispFromPicture(image), stdole.IPictureDisp)
'    End Function
'End Class




' Dim Button2 As Inventor.ComboBoxDefinition = oControlDefs.AddComboBoxDefinition("Stinktier", "internalst", CommandTypesEnum.kNonShapeEditCmdType, 12, "clientid", "asjfö", "asdf", smallPicture, largePicture, ButtonDisplayEnum.kDisplayTextInLearningMode)





' command category
'Dim objCommandCategory As Inventor.CommandCategory = MIDApplication.CommandManager.CommandCategories.Add("Select", "AB", m_clientID)
'objCommandCategory.Add(oKeepOutButton)

'Dim MIDButtonOne As ButtonDefinition = MIDApplication.CommandManager.ControlDefinitions.Item("PartExtrudeCmd")

'oPanelManage.CommandControls.AddComboBox(Button2, , True)

'If SelectionOn = True Then
'    Exit Sub
'End If
''On Error Resume Next
'SelectionOn = True
'' Get the active document.  This assumes it is an assembly.
'Dim asmDoc As AssemblyDocument
'asmDoc = oAddIn.ActiveDocument

'' The Dynamic Simulation environment must be active.
'Dim UIManager As UserInterfaceManager
'UIManager = oAddIn.UserInterfaceManager
'If UIManager.ActiveEnvironment.InternalName <> "DynamicSimulationEnvironmentInternalName" Then
'    ' Get the environment manager.
'    Dim environmentMgr As EnvironmentManager
'    environmentMgr = asmDoc.EnvironmentManager

'    Dim dsEnv As Inventor.Environment
'    dsEnv = UIManager.Environments.Item("DynamicSimulationEnvironmentInternalName")
'    Call environmentMgr.SetCurrentEnvironment(dsEnv)
'End If

'' Get the simulation manager from the assembly.
'Dim simManager As SimulationManager
'simManager = asmDoc.ComponentDefinition.SimulationManager

'' Get the first simulation.  Currently there is only ever one.
'Dim sim As DynamicSimulation
'sim = simManager.DynamicSimulations.Item(1)

'' Check to see if the simulation has already been computed.
'If sim.LastComputedTimeStep < sim.NumberOfTimeSteps Then
'    ' Compute the simulation, which will also play it.
'    sim.ComputeSimulation()
'Else
'    ' Play the computed simulation.
'    sim.PlaySimulation()
'End If


'oUserInterfaceEvents = oAddIn.UserInterfaceManager.UserInterfaceEvents

''AddHandler oUserInterfaceEvents.OnResetCommandBars, AddressOf Me.UserInterfaceEvents_OnResetCommandBars
''AddHandler oUserInterfaceEvents.OnEnvironmentChange, AddressOf Me.UserInterfaceEvents_OnEnvironmentChange
'AddHandler oUserInterfaceEvents.OnResetRibbonInterface, AddressOf Me.UserInterfaceEvents_OnResetRibbonInterface


'' Case #1: Part Document open
'Dim oControlDefs As ControlDefinitions = oAddIn.CommandManager.ControlDefinitions

'Dim smallEnvPic As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.EnvButtonSmall)
'Dim largeEnvPic As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.EnvButtonLarge)

'Dim oEnvs As Inventor.Environments = oAddIn.UserInterfaceManager.Environments

'oPartEnv = oEnvs.Add("Moulded Interconnected Devices", "MidPartEnv", , smallEnvPic, largeEnvPic)


'' Add the Button to control definition
'Dim oPartEnvButton As ControlDefinition = oControlDefs.Item("MidPartEnv")

'' Retrieve the Part-Ribbon
'Dim oPartRibbon As Ribbon = oAddIn.UserInterfaceManager.Ribbons.Item("Part")
'Dim oPartRibbonTab As RibbonTab = oPartRibbon.RibbonTabs.Add("MID", "MIDInternPart", "clientId", , , True)
'Dim oPartPanel As RibbonPanel = oPartRibbonTab.RibbonPanels.Add("Select", "PartSelect", "clientId")

'' Create new button for ribbon
'oNewProjectButton = oControlDefs.AddButtonDefinition("New" & vbNewLine & "Project", "NewProjectIntern", CommandTypesEnum.kFileOperationsCmdType, )
'' Add button to the ribbon panel
'oPartPanel.CommandControls.AddButton(oNewProjectButton, True)

'Dim partRibbon(0) As String
'partRibbon(0) = "MIDInternPart"

'oPartEnv.AdditionalVisibleRibbonTabs() = partRibbon

'oPartEnv.DefaultRibbonTab = "MIDInternPart"

''get the collection of parallel environments and add the new environment
'Dim oEnvList As EnvironmentList = oAddIn.UserInterfaceManager.ParallelEnvironments
'oEnvList.Add(oPartEnv)











'Public oNewProjectButton As ButtonDefinition

'Public oKeepOutButton As ButtonDefinition



'Public WithEvents oImportButton As ButtonDefinition

'Public WithEvents oExportButton As ButtonDefinition
'Public WithEvents oExportSetButton As ButtonDefinition

'Public WithEvents oPlaceButton As ButtonDefinition
'Private WithEvents oProjectButton As ButtonDefinition

'Public oPlaceCompButton As ButtonDefinition


'####################################################################################
' Case #2: Assembly document open
'####################################################################################

'' Retrieve the assembly ribbon
'Dim oAsmRibbon As Ribbon = oAddIn.UserInterfaceManager.Ribbons.Item("Assembly")

'' Insert new ribbon tab before the "Assemble" tab
'oRibbonTab = oAsmRibbon.RibbonTabs.Add("MID", "MidInternAsm", "clientId", "id_TabAssemble", True, True)

'' add new ribbon panels to the tab
'Dim oPanelManage As RibbonPanel = oRibbonTab.RibbonPanels.Add("Manage", "manageIntern", "clientId")
'Dim oPanelSelect As RibbonPanel = oRibbonTab.RibbonPanels.Add("Select", "selectIntern", "clientId")
'Dim oPanelExport As RibbonPanel = oRibbonTab.RibbonPanels.Add("Export", "exportIntern", "clientId")
'Dim oPanelImport As RibbonPanel = oRibbonTab.RibbonPanels.Add("Import", "importIntern", "clientId")






'oPanelManage.CommandControls.AddButton(oNewProjectButton, True)
'oPanelManage.CommandControls.AddSeparator("newProjectInterern", False)
'oProjectButton.ToolTipText = "Create a new MID-Project"

'oPanelManage.CommandControls.AddButton(oPlaceButton, True)
'oPanelManage.CommandControls.AddSeparator("placeMidIntern", False)


'oPanelExport.CommandControls.AddButton(oExportSetButton, False, True)
'#######################################################################

'Add Ribbon Tab #2
'Dim MIDRibbonTabTwo As RibbonTab = MIDRibbon.RibbonTabs.Add("Some Analysis Extras", "SomeAnalysisExtras", m_clientID, , , True)

' Dim MIDPanelThree As RibbonPanel = MIDRibbonTabTwo.RibbonPanels.Add("Panel Three", "PanelThree", m_clientID)
'Dim MIDButtonThree As ButtonDefinition = MIDApplication.CommandManager.ControlDefinitions.Item("PartCoilCmd")
'MIDPanelThree.CommandControls.AddButton(MIDButtonThree, True)




'Associate the contextual tabs with the newly created environment
'The contextual tabs will only be displayed when this environment is active

' Add environment and put it in the parallel environment list
'oMidAsmEnv = oEnvs.Add("Moulded Interconnected Devices", "MidAsmEnv", , smallEnvPic, largeEnvPic)
'Dim MIDParEnvs As EnvironmentList = oAddIn.UserInterfaceManager.ParallelEnvironments
'MIDParEnvs.Add(oMidAsmEnv)

'Dim oMidAsmEnvButton As ControlDefinition = oControlDefs.Item("MidAsmEnv")

'strTabs(0) = "MidInternAsm"

'' Attach ribbon tab to the environment
'oMidAsmEnv.AdditionalVisibleRibbonTabs = strTabs

''Make the "SomeAnalysis" tab default for the environment
'oMidAsmEnv.DefaultRibbonTab = "MidInternAsm"

''get the collection of parallel environments and add the new environment




'' Remove the environment if no part or assembly environment open

'For Each oEnv As Inventor.Environment In oEnvs
'    If Not oEnv.InternalName = "PMxPartEnvironment" Then
'        'On Error Resume Next
'        oEnv.DisabledCommandList.Add(oPartEnvButton)
'    End If
'    If Not oEnv.InternalName = "AMxAssemblyEnvironment" Then
'        oEnv.DisabledCommandList.Add(oMidAsmEnvButton)
'    End If
'Next

' Remove the environment if no part environment open
'For Each Env1 As Inventor.Environment In oEnvList

'On Error Resume Next


' Next
