
Imports System



Public Class CommandCollection

    Private oUserInterfaceEvents As UserInterfaceEvents

    Public oNewProjectCmd As Command
    Public oKeepOutCmd As Command
    Public oAddNetCmd As Command
    Public oPlaceCompCmd As Command
    Public oPlaceCmd As Command
    Private oMoveMidCmd As Command

    Private oAddIn As Inventor.Application

    Private _strWorkDirecotry As String

    Private Shared partDocName As String
    'Private strClientId As String
    'Private Shared strFilePath As String
    Private oServer As MidAddInServer

    Public Enum CommandEnableEnum
        kKeepOutEnable
        kExportEnable
        kImportEnable
        kPlaceComponentEnable
    End Enum


    ' Constructor
    '*********************************************************************************************
    Public Sub New(ByVal oAddIn As Inventor.Application, ByVal oServer As MidAddInServer)

        MyBase.New()

        Me.oAddIn = oAddIn
        'Me.strClientId = strClientId
        Me.oServer = oServer

    End Sub

    ' Return work directory
    '********************************************************************************************************************
    Public Property WorkDirectory() As String
        Get
            Return _strWorkDirecotry
        End Get
        Set(value As String)
            _strWorkDirecotry = value
        End Set
    End Property


    Public Shared ReadOnly Property GetWorkDirPath() As String
        Get
            Return partDocName
        End Get
    End Property

    Public ReadOnly Property GetImportNetListCmd() As AddNetCommand
        Get
            Return oAddNetCmd
        End Get
    End Property

    Public ReadOnly Property ClientId As String
        Get
            Return oServer.ClientId
        End Get
    End Property

    Public Sub initButtons()

        ' Get the command category
        Dim oCmdCategory As CommandCategory = oAddIn.CommandManager.CommandCategories.Add("MidProject", "MidProjectInternal", oServer.ClientId)

        ' Get control definitions collection
        Dim oControlDefs As Inventor.ControlDefinitions = oAddIn.CommandManager.ControlDefinitions


        ' New Project command
        '********************************************************************************************************************
        Dim placeButtonSmallPic As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.RibbonPlaceButtonSmall)
        Dim placeButtonLargePic As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.RibbonPlaceButtonLarge)

        oNewProjectCmd = New NewProjectCommand(oAddIn, oServer)
        oNewProjectCmd.CreateButton("New" & vbNewLine & "Project", "newProjectIntern", Inventor.CommandTypesEnum.kQueryOnlyCmdType, , , "Create a new MID-Project", placeButtonSmallPic, placeButtonLargePic)
        oCmdCategory.Add(oNewProjectCmd.ButtonDefinition)
        oNewProjectCmd.ButtonDefinition.Enabled = True

        ' Place cirucit carrier command
        '********************************************************************************************************************
        oPlaceCmd = New PlaceCommand(oAddIn, oServer)
        oPlaceCmd.CreateButton("Place" & vbNewLine & "MID", "placeMidIntern", Inventor.CommandTypesEnum.kQueryOnlyCmdType, , , , placeButtonSmallPic, placeButtonLargePic)
        oCmdCategory.Add(oPlaceCmd.ButtonDefinition)


        ' Move circuit carrier button


        ' Keep-Out command
        '********************************************************************************************************************
        Dim smallPicture As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.MySmallImage)
        Dim largePicture As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.MyLargeImage)

        oKeepOutCmd = New KeepOutCommand(oAddIn, oServer)
        oKeepOutCmd.CreateButton("Select" & vbNewLine & "KeepOuts", "KeepOutsIntern", Inventor.CommandTypesEnum.kShapeEditCmdType, "clientid", , , smallPicture, largePicture)
        oCmdCategory.Add(oKeepOutCmd.ButtonDefinition)

        ' Import command
        '********************************************************************************************************************
        Dim importPicSmall As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.RibbonImportSmall)
        Dim importPicLarge As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.RibbonImportLarge)

        oAddNetCmd = New AddNetCommand(oAddIn, oServer)
        oAddNetCmd.CreateButton("Import" & vbNewLine & "XML Netlist", "ImportIntern", Inventor.CommandTypesEnum.kFileOperationsCmdType, "clientid", , , importPicSmall, importPicLarge)
        oCmdCategory.Add(oAddNetCmd.ButtonDefinition)

        ' Place component command
        '********************************************************************************************************************
        'Dim importPicSmall As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.RibbonImportSmall)
        'Dim importPicLarge As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.RibbonImportLarge)

        oPlaceCompCmd = New PlaceCompCommand(oAddIn, oServer)
        oPlaceCompCmd.CreateButton("Place" & vbNewLine & "Component", "PlaceCompIntern", Inventor.CommandTypesEnum.kShapeEditCmdType)
        oCmdCategory.Add(oPlaceCompCmd.ButtonDefinition)


        ' Move mid command
        '********************************************************************************************************************
        oMoveMidCmd = New MoveMidCommand(oAddIn, oServer)
        oMoveMidCmd.CreateButton("Move" & vbNewLine & "MID Device", "MoveMidIntern", Inventor.CommandTypesEnum.kFileOperationsCmdType, oServer.ClientId)
        oCmdCategory.Add(oMoveMidCmd.ButtonDefinition)

        Dim oMoveMidChangeDefinition = oServer.ChangeDefinitions.Add("MoveMidChangeDefinition", "Move MID Device")

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


    End Sub

    Public ReadOnly Property KeepOutCommand() As KeepOutCommand
        Get
            Return oKeepOutCmd
        End Get
    End Property

    Public ReadOnly Property AddNetCommand() As AddNetCommand
        Get
            Return oAddNetCmd
        End Get
    End Property

    Public ReadOnly Property MoveMidCommand() As MoveMidCommand
        Get
            Return oMoveMidCmd
        End Get
    End Property


    '###Add other properties here!!!!



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


    Public Sub EnableDefinition(commandType As CommandEnableEnum)
        If commandType = CommandEnableEnum.kKeepOutEnable Then
            oKeepOutCmd.ButtonDefinition.Enabled = True

        End If



    End Sub


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

'###########################################################################
' Picture converter class
'###########################################################################
<System.ComponentModel.DesignerCategory("")> Friend Class PictureConverter
    Inherits System.Windows.Forms.AxHost


    Private Sub New()
        MyBase.New(String.Empty)
    End Sub

    Public Shared Function ImageToPictureDisp( _
                           ByVal image As System.Drawing.Image) As stdole.IPictureDisp
        Return CType(GetIPictureDispFromPicture(image), stdole.IPictureDisp)
    End Function
End Class




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
'    environmentMgr.SetCurrentEnvironment(dsEnv)
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
