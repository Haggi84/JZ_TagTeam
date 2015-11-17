' import namespaces
Imports Inventor
Imports System.Runtime.InteropServices
Imports Microsoft.Win32

' TEXT
Imports System
Imports System.IO
Imports System.Text
Imports System.Collections.Generic
Imports System.Windows.Forms

'Namespace MIDProjectVB3
<ProgIdAttribute("MIDProjectVB3.MidAddInServer"), GuidAttribute("e4a46d1d-77db-4aa1-b097-6321b6783c26")> _
Public Class MidAddInServer
    Implements Inventor.ApplicationAddInServer


#Region "Data Member"
    ' Inventor application object.
    Public MidAddIn As Inventor.Application
    Public oXML As ReadNetlistCommand

    Public strMidAddInCLSID As String
    ' Button with event handle
    Private WithEvents MIDButtonKeepOuts As Inventor.ButtonDefinition
    Private WithEvents oPlaceButton As Inventor.ButtonDefinition
    Private WithEvents oImportButton As Inventor.ButtonDefinition
    Private WithEvents oExportButton As Inventor.ButtonDefinition

    Private MIDNewEnv As Inventor.Environment


    ' Public Shared sb As New StringBuilder()

    Private oCommands As Commands

    Private oClsUserInterfaceEvt As UserInterfaceEvents
    Private oClsUserInputEvts As UserInputEvents
    Private oClsApplicationEvts As ApplicationEvents
    Private oMidDocumentEvents As MidDocumentEvents

    'Private WithEvents oMidAsmEnvButton As ButtonDefinition

    Private oChangeManager As ChangeManager
    Private oChangeDefinitions As ChangeDefinitions

    Private oBrowser As Browser
    Private oMidDataTypes As MidData

#End Region


#Region "ApplicationAddInServer Members"



    ' Activate: Here it all begins
    '********************************************************************************************************************
    Public Sub Activate(ByVal addInSiteObject As Inventor.ApplicationAddInSite, _
                        ByVal firstTime As Boolean) Implements Inventor.ApplicationAddInServer.Activate

        Try
            ' Init addin members
            MidAddIn = addInSiteObject.Application

            ' Connect to user interface events sink
            'Dim oUserInterfaceEvents As UserInterfaceEvents = MidAddIn.UserInterfaceManager.UserInterfaceEvents

            ' Retrieve the GUID for this class
            Dim MidAddInCLSID As GuidAttribute = CType(System.Attribute.GetCustomAttribute(GetType(MidAddInServer), GetType(GuidAttribute)), GuidAttribute)

            strMidAddInCLSID = "{" & MidAddInCLSID.Value & "}"

            ' Create new custom browser
            oBrowser = New Browser(MidAddIn, Me)

            ' Connect user interface event sinks
            oClsUserInterfaceEvt = New UserInterfaceEvents(MidAddIn, Me)

            ' Connect user input events sinks
            oClsUserInputEvts = New UserInputEvents(MidAddIn)

            ' Connect applicaton events sinks
            oClsApplicationEvts = New ApplicationEvents(MidAddIn, Me)

            ' Connect to assembly document events sink
            oMidDocumentEvents = New MidDocumentEvents(MidAddIn, Me)

            oMidDataTypes = New MidData(MidAddIn, Me)

            ' Create new buttons
            oCommands = New Commands(MidAddIn, Me)
            oCommands.Initialize()

            Dim oUserInterfaceMgr As Inventor.UserInterfaceManager
            Dim oEnvironments As Inventor.Environments
            Dim oOPTAVER As Inventor.Environment
            Dim oToolButton As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.Logo)


            If firstTime Then

                ' Get the user interface manager
                oUserInterfaceMgr = MidAddIn.UserInterfaceManager

                ' Add new environment 
                oEnvironments = oUserInterfaceMgr.Environments
                oOPTAVER = oEnvironments.Add("OPTAVER", "OPTAVER", , oToolButton, oToolButton)

                '#########
                Dim ParEnvs As EnvironmentList = MidAddIn.UserInterfaceManager.ParallelEnvironments
                ParEnvs.Add(oOPTAVER)
                'End If

                ' Environment contorl definiton
                '########################
                'oMidAsmEnvButton = MidAddIn.CommandManager.ControlDefinitions.Item("OPTAVER")
                '#########
                Dim oInterfaceStyle As InterfaceStyleEnum = oUserInterfaceMgr.InterfaceStyle

                ' Only compatible to new inventor versions (RibbonInterface)
                If oInterfaceStyle = Inventor.InterfaceStyleEnum.kRibbonInterface Then

                    'Retrieve the assembly ribbon ("Assembly" is the name of the ribbon corresponding to the assmebly document)
                    Dim oAsmRibbon As Ribbon = MidAddIn.UserInterfaceManager.Ribbons.Item("Assembly")

                    ' Insert new ribbon tab before the "Assemble" tab
                    Dim oRibbonTab As RibbonTab = oAsmRibbon.RibbonTabs.Add("OPTAVER", "MidInternAsm", Me.ClientId, "id_TabAssemble", True, True)

                    ' Add new ribbon panels to the ribbon tab
                    Dim oManagePanel As RibbonPanel = oRibbonTab.RibbonPanels.Add("Manage", "Manage" & Me.ClientId, Me.ClientId)
                    Dim oComponentPanel As RibbonPanel = oRibbonTab.RibbonPanels.Add("Circuit Carrier", "Component" & Me.ClientId, Me.ClientId)
                    Dim oPositionPanel As RibbonPanel = oRibbonTab.RibbonPanels.Add("Position ", "Position" & Me.ClientId, Me.ClientId)
                    Dim oToolPanel As RibbonPanel = oRibbonTab.RibbonPanels.Add("Restrictions", "Tool" & Me.ClientId, Me.ClientId)
                    Dim oConductor As RibbonPanel = oRibbonTab.RibbonPanels.Add("Conductor", "Conductor" & Me.ClientId, Me.ClientId)
                    Dim oOpticalComponent As RibbonPanel = oRibbonTab.RibbonPanels.Add("Optical Components", "Optical_Components" & Me.ClientId, Me.ClientId)
                    Dim oWaveguidePanel As RibbonPanel = oRibbonTab.RibbonPanels.Add("Waveguide", "Waveguide" & Me.ClientId, Me.ClientId)
                    Dim oExportToRaytracePanel As RibbonPanel = oRibbonTab.RibbonPanels.Add("Export to Raytrace", "Export" & Me.ClientId, Me.ClientId)

                    Dim oCallDatabasePanel As RibbonPanel = oRibbonTab.RibbonPanels.Add("Call Database", "Database" & Me.ClientId, Me.ClientId)

                    ' Manage panel
                    oManagePanel.CommandControls.AddButton(oCommands.NewProjectCommand.ButtonDefinition, True, True)
                    oManagePanel.CommandControls.AddButton(oCommands.LoadProjectCommand.ButtonDefinition, True, True)

                    ' Component panel
                    oComponentPanel.CommandControls.AddButton(oCommands.PlaceCommand.ButtonDefinition, True, True)
                    oComponentPanel.CommandControls.AddButton(oCommands.AddNetCommand.ButtonDefinition, True, True)

                    ' Pop-Up-button/Tool panel
                    Dim oKeepOutObjColl As ObjectCollection = MidAddIn.TransientObjects.CreateObjectCollection
                    oKeepOutObjColl.Add(oCommands.KeepOutCommand.ButtonDefinition)
                    oKeepOutObjColl.Add(oCommands.ReadKeepOutCommand.ButtonDefinition)
                    oToolPanel.CommandControls.AddButtonPopup(oKeepOutObjColl, True, True)

                    ' Position panel
                    oPositionPanel.CommandControls.AddButton(oCommands.PlacePartCommand.ButtonDefinition, True, True)
                    oPositionPanel.CommandControls.AddButton(oCommands.MoveMidCommand.ButtonDefinition, True, True)

                    ' Waveguide panel
                    oWaveguidePanel.CommandControls.AddButton(oCommands.OpticalWaveguideCommand.ButtonDefinition, True, True)

                    ' Export to Raytrace panel
                    oExportToRaytracePanel.CommandControls.AddButton(oCommands.ExportToRaytraceCommand.ButtonDefinition, True, True)
                    ' Call Database panel
                    oCallDatabasePanel.CommandControls.AddButton(oCommands.CallDatabaseCommand.ButtonDefinition, True, True)

                    ' Attach ribbon tab to the environment
                    Dim strTabs(0) As String
                    strTabs(0) = "MidInternAsm"
                    oOPTAVER.AdditionalVisibleRibbonTabs = strTabs

                    'Make the "MidInternAsm" tab default for the environment
                    oOPTAVER.DefaultRibbonTab = "MidInternAsm"

                    ' Mid environment can only be called in assembly document context
                    For Each oEnv As Inventor.Environment In oEnvironments
                        If Not oEnv.InternalName = "AMxAssemblyEnvironment" Then
                            'oEnv.DisabledCommandList.Add(oMidAsmEnvButton)
                        End If
                    Next

                Else
                    '###messagebox + quit
                    MessageBox.Show("This AddIn is not compatible to this Version", _
                                   "MIDProject", _
                                   MessageBoxButtons.OK, _
                                   MessageBoxIcon.Warning)
                End If
            End If


        Catch ex As Exception

            System.Windows.Forms.MessageBox.Show(ex.ToString())

        End Try
        ' This method is called by Inventor when it loads the AddIn.
        ' The AddInSiteObject provides access to the Inventor Application object.
        ' The FirstTime flag indicates if the AddIn is loaded for the first time.

        ' Initialize AddIn members.

        'get the Inventor application object


        ' TODO:  Add ApplicationAddInServer.Activate implementation.
        ' e.g. event initialization, command creation etc.



        'Call SelectionSample()
        ' Call Selection()
        'Call ActivateET()
    End Sub


    Public ReadOnly Property ChangeDefinitions As ChangeDefinitions
        Get
            Return oChangeDefinitions
        End Get
    End Property


    Public ReadOnly Property MidDataTypes As MidData
        Get
            Return oMidDataTypes
        End Get
    End Property


    Public Sub Deactivate() Implements Inventor.ApplicationAddInServer.Deactivate

        ' This method is called by Inventor when the AddIn is unloaded.
        ' The AddIn will be unloaded either manually by the user or
        ' when the Inventor session is terminated.

        ' TODO:  Add ApplicationAddInServer.Deactivate implementation

        ' Release objects.

        ' deactivate selection
        'SelectionOn = False

        MidAddIn = Nothing

        System.GC.Collect()
        System.GC.WaitForPendingFinalizers()
    End Sub

    Public ReadOnly Property Automation() As Object Implements Inventor.ApplicationAddInServer.Automation

        ' This property is provided to allow the AddIn to expose an API 
        ' of its own to other programs. Typically, this  would be done by
        ' implementing the AddIn's API interface in a class and returning 
        ' that class object through this property.

        Get
            Return Nothing
        End Get

    End Property

    Public Sub ExecuteCommand(ByVal commandID As Integer) Implements Inventor.ApplicationAddInServer.ExecuteCommand

        ' Note:this method is now obsolete, you should use the 
        ' ControlDefinition functionality for implementing commands.

    End Sub




#End Region



    Public ReadOnly Property ClientId As String
        Get
            Return strMidAddInCLSID
        End Get
    End Property

    ' Properties 
    '********************************************************************************************************************
    Public Property Browser() As Browser
        Get
            Return oBrowser
        End Get
        Set(value As Browser)
            oBrowser = value
        End Set
    End Property

    Public ReadOnly Property Commands() As Commands
        Get
            Return oCommands
        End Get
    End Property

    ' Return mid document events
    Public ReadOnly Property MidDocumentEvents() As MidDocumentEvents
        Get
            Return oMidDocumentEvents
        End Get
    End Property

End Class

' oOPTAVER = oEnvironments.Item("OPTAVER")
'If oOPTAVER Is Nothing Then
' Set active Environment
'**************************************************************************************************
'Private Sub oEnvButton_OnExecute() Handles oMidAsmEnvButton.OnExecute

'    Dim oEnvironments As Environments = MidAddIn.UserInterfaceManager.Environments

'    Dim oEnv As Inventor.Environment

'    For Each oEnv In oEnvironments
'        If oEnv.InternalName.Equals("OPTAVER") Then
'            Exit For
'        End If
'    Next

'    Dim EnvironmentMrg As EnvironmentManager = MidAddIn.ActiveDocument.EnvironmentManager
'    EnvironmentMrg.SetCurrentEnvironment(oEnv)

'End Sub

''######################################################
'' Button Events
''######################################################

'Private Sub MIDButtonKeepOuts_OnExecute(ByVal Context As NameValueMap) Handles MIDButtonKeepOuts.OnExecute
'    If SelectionOn = True Then
'        Exit Sub
'    End If
'    'On Error Resume Next
'    SelectionOn = True

'    ' Dim oXML As New MidXmlImport(MidAddIn)

'    'oXML.ImportBRep()

'    ' Get the transient B-Rep and MID objects.
'    Dim oInteraction As New KeepOutSelection(MidAddIn)

'    oInteraction.SelectEntity(SelectionFilterEnum.kPartFaceFilter)

'    'Dim oTriadEvents As New InteractionTriad(MidAddIn)

'    'oTriadEvents.UserForm_Initialize()

'    'Dim oDocument = MidAddIn.ActiveDocument

'    'Dim oPartDoc As PartDocument = MidAddIn.Documents.Add(DocumentTypeEnum.kPartDocumentObject, _
'    '                                                            MidAddIn.FileManager.GetTemplateFile(DocumentTypeEnum.kPartDocumentObject), False)

'    'Dim oCompDef As PartComponentDefinition = oPartDoc.ComponentDefinition

'    'Dim oTransientBRep As TransientBRep = MidAddIn.TransientBRep

'    'Dim oTG As TransientGeometry = MidAddIn.TransientGeometry

'    'Dim oBox As Box = oTG.CreateBox()

'    'Dim minPoint(2) As Double
'    'Dim maxPoint(2) As Double

'    'minPoint(0) = 0
'    'minPoint(1) = 0
'    'minPoint(2) = 0

'    'maxPoint(0) = 0.5
'    'maxPoint(1) = 0.4
'    'maxPoint(2) = 0.3

'    'oBox.PutBoxData(minPoint, maxPoint)

'    'Dim oBody As SurfaceBody = oTransientBRep.CreateSolidBlock(oBox)

'    'Dim oBaseFeature As NonParametricBaseFeature = oCompDef.Features.NonParametricBaseFeatures.Add(oBody)



'    'Dim oPosMatrix As Matrix = MidAddIn.TransientGeometry.CreateMatrix()

'    ''MidAddIn.SilentOperation = True
'    '' oPartDoc.SaveAs(sFilePath & id & ".ipt", False)
'    ''MidAddIn.SilentOperation = False
'    ''MidAddIn.ActiveView.Update()
'    '' oPartDoc.Close(True)
'    'Dim oAssyDoc As AssemblyDocument = MidAddIn.ActiveDocument()
'    '' Generate new part occurrence
'    ''oPartOcc = oDocument.ComponentDefinition.Occurrences.Add(sFilePath & id & ".ipt", oPosMatrix)
'    'Dim oPartOcc As ComponentOccurrence = oAssyDoc.ComponentDefinition.Occurrences.AddByComponentDefinition( _
'    '                            oCompDef, oPosMatrix)


'End Sub

'Private Sub ImportButton_OnExecute(ByVal Context As NameValueMap) Handles oImportButton.OnExecute
'    'If SelectionOn = True Then
'    'Exit Sub
'    'End If
'    'On Error Resume Next
'    ' Part Document

'    ' Make sure a part document is active
'    Dim oPartDoc As AssemblyDocument
'    oPartDoc = MidAddIn.ActiveDocument

'    Dim oEnvironments As Environments
'    oEnvironments = MidAddIn.UserInterfaceManager.Environments

'    ' Create a new environment
'    Dim oOverrideEnv As Inventor.Environment
'    oOverrideEnv = oEnvironments.Add("Override", "OverrideEnvironment")

'    ' Get the part ribbon
'    Dim oPartRibbon As Ribbon
'    oPartRibbon = MidAddIn.UserInterfaceManager.Ribbons.Item("Assembly")

'    ' Create a contextual tab to be used as the default for the override environment
'    Dim oTabOne As RibbonTab = oPartRibbon.RibbonTabs.Add("Tab One", "TabOne", "ClientId123", , True, False)

'    ' Create panels with the tab
'    Dim oPanelOne As RibbonPanel
'    oPanelOne = oTabOne.RibbonPanels.Add("Panel One", "PanelOne", "ClientId123")

'    Dim oDef1 As ButtonDefinition
'    oDef1 = MidAddIn.CommandManager.ControlDefinitions.Item("PartExtrudeCmd")

'    Call oPanelOne.CommandControls.AddButton(oDef1, True)

'    Dim oPanelTwo As RibbonPanel
'    oPanelTwo = oTabOne.RibbonPanels.Add("Panel Two", "PanelTwo", "ClientId123")

'    Dim oDef2 As ButtonDefinition
'    oDef2 = MidAddIn.CommandManager.ControlDefinitions.Item("PartRevolveCmd")

'    Call oPanelTwo.CommandControls.AddButton(oDef2, True)

'    Dim strTabs(0) As String
'    strTabs(0) = "TabOne"

'    oOverrideEnv.InheritAllRibbonTabs = False
'    oOverrideEnv.AdditionalVisibleRibbonTabs = strTabs
'    oOverrideEnv.DefaultRibbonTab = "TabOne"

'    ' Set the override environment on the active part
'    oPartDoc.EnvironmentManager.OverrideEnvironment = oOverrideEnv



'    'oXML = New MidXMLReader(MidAddIn)

'    'oXML.ImportBRep()
'    'Dim oAsmDoc As AssemblyDocument = MidAddIn.ActiveDocument

'    'Dim assetLib As AssetLibrary = MidAddIn.AssetLibraries.Item("Autodesk Appearance Library")

'    'Dim locAsset As Asset = assetLib.AppearanceAssets.Item("Smooth - Red")

'    'Dim LibAssetDefault As Asset = locAsset.CopyTo(oAsmDoc)

'    'Dim oCompDef As ComponentDefinition = oAsmDoc.ComponentDefinition

'    'Dim oCompOcc As ComponentOccurrence

'    'For Each oCompOcc In oCompDef.Occurrences
'    '    Debug.WriteLine(oCompOcc.Name)
'    '    'oCompOcc.Appearance = LibAssetDefault
'    '    Dim oSurfaceBody As SurfaceBody = oCompOcc.SurfaceBodies.Item(1)
'    '    If oSurfaceBody Is Nothing Then
'    '        MsgBox("is nothing")
'    '    Else
'    '        'oSurfaceBody.Appearance = LibAssetDefault
'    '    End If
'    '    Dim oFace As Face = oSurfaceBody.Faces.Item(3)
'    '    If oFace Is Nothing Then
'    '        MsgBox("is nothing")
'    '    Else
'    '        Dim oStyle As RenderStyle = oAsmDoc.RenderStyles.Item("Smooth - Red")
'    '        oFace.SetRenderStyle(StyleSourceTypeEnum.kOverrideRenderStyle, oStyle)
'    '        'oFace.Appearance = LibAssetDefault
'    '    End If

'    'Next



'    ' Get the transient B-Rep and MID objects.
'    'Dim oInteraction As New Interaction(MidAddIn)

'    ' oInteraction.SelectEntity(SelectionFilterEnum.kPartFaceFilter)

'    'Dim oTriadEvents As New InteractionTriad(MidAddIn)

'    'oTriadEvents.UserForm_Initialize()


'End Sub

'Private Sub oExportButton_OnExecute() Handles oExportButton.OnExecute


'    Dim oExpSet As ExportSettings = New ExportSettings(MidAddIn)
'    oExpSet.initExport()

'End Sub

'#######################################################




'Sub AddParallelEnv1()
'    PrintRibbon()

'    receive the Environment collection
'    Dim smallEnvPic As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.EnvButtonSmall)
'    Dim largeEnvPic As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.EnvButtonLarge)

'    Dim MIDEnvs As Environments = MidAddIn.UserInterfaceManager.Environments
'    use build-in enironment as a basis
'    Dim MIDEnv As Environment = MIDEnvs("MBxSheetMetalEnvironment")
'    add New invironment
'    MIDNewEnv = MIDEnvs.Add("Moulded Interconnected Devices", _
'                                                        "MIDInventEnvIntern", _
'                                                        m_clientID, _
'                                                        smallEnvPic, _
'                                                        largeEnvPic)

'    Dim MIDParEnvButton As ControlDefinition = MidAddIn.CommandManager.ControlDefinitions.Item("MIDInventEnvIntern")


'    Dim controlDefs As ControlDefinitions = MidAddIn.CommandManager.ControlDefinitions
'    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'    Part Document
'    Dim PartRibbon As Ribbon = MidAddIn.UserInterfaceManager.Ribbons.Item("Part")

'    Dim PartRibbonTab As RibbonTab = PartRibbon.RibbonTabs.Add("MID", "MIDPartIntern", m_clientID, , , True)

'    Dim PartPanel As RibbonPanel = PartRibbonTab.RibbonPanels.Add("Select", "PartSelect", m_clientID)

'    Dim oPartSelectButton As ButtonDefinition = controlDefs.AddButtonDefinition("sel", "internsel", CommandTypesEnum.kFileOperationsCmdType, )

'    PartPanel.CommandControls.AddButton(oPartSelectButton, True)

'    Dim strRibbon(0) As String
'    strRibbon(0) = "MIDPartIntern"

'    MIDNewEnv.AdditionalVisibleRibbonTabs() = strRibbon

'    MIDNewEnv.DefaultRibbonTab = "MIDPartIntern"


'    Ribbon()
'    Dim MIDRibbon As Ribbon = MidAddIn.UserInterfaceManager.Ribbons.Item("Assembly")
'    Add Ribbon Tab #1
'    Dim MIDRibbonTabOne As RibbonTab = MIDRibbon.RibbonTabs.Add("MID", "MIDTabIntern", m_clientID, , , True)

'    Add panel with a button
'    Dim MIDPanelOne As RibbonPanel = MIDRibbonTabOne.RibbonPanels.Add("Select", "PanelOne", m_clientID)

'    convert image to IPictureDisp (for Inventor) and add a ControlDefinition
'    Dim smallPicture As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.MySmallImage)
'    Dim largePicture As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.MyLargeImage)

'     create the commands for the environment
'    MIDButtonKeepOuts = controlDefs.AddButtonDefinition("Face" & vbNewLine & "KeepOuts", "KeepOutsIntern", _
'                                                            CommandTypesEnum.kNonShapeEditCmdType, _
'                                                            m_clientID, , , smallPicture, largePicture)

'    Dim Button2 As Inventor.ComboBoxDefinition = controlDefs.AddComboBoxDefinition("Stinktier", "internalst", CommandTypesEnum.kNonShapeEditCmdType, 12, m_clientID, "asjfö", "asdf", smallPicture, largePicture, ButtonDisplayEnum.kDisplayTextInLearningMode)





'    command category
'    Dim objCommandCategory As Inventor.CommandCategory = MidAddIn.CommandManager.CommandCategories.Add("Select", "AB", m_clientID)
'    objCommandCategory.Add(MIDButtonKeepOuts)

'    Dim MIDButtonOne As ButtonDefinition = MidAddIn.CommandManager.ControlDefinitions.Item("PartExtrudeCmd")
'    MIDPanelOne.CommandControls.AddButton(MIDButtonKeepOuts, True)
'    MIDPanelOne.CommandControls.AddComboBox(Button2, , True)

'    Dim importPicSmall As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.RibbonImportSmall)
'    Dim importPicLarge As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.RibbonImportLarge)

'    oImportButton = controlDefs.AddButtonDefinition("Import" & vbNewLine & "XML Netlist", "ImportIntern", _
'                                                    CommandTypesEnum.kNonShapeEditCmdType, _
'                                                    m_clientID, , , importPicSmall, importPicLarge)
'    MIDPanelOne.CommandControls.AddButton(oImportButton, True)

'    Add the Assmebly Place Button
'    Dim MIDPanelTwo As RibbonPanel = MIDRibbonTabOne.RibbonPanels.Add("Panel Two", "PanelTwo", m_clientID)

'    Dim oPlaceButton As ButtonDefinition = MidAddIn.CommandManager.ControlDefinitions.Item("AssemblyPlaceComponentCmd")

'    MIDPanelOne.CommandControls.AddButton(oPlaceButton, True)


'    Export Button
'    Dim exportPicSmall As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.RibbonExportSmall)
'    Dim exportPicLarge As stdole.IPictureDisp = PictureConverter.ImageToPictureDisp(My.Resources.RibbonExportLarge)

'    oExportButton = controlDefs.AddButtonDefinition("Export" & vbNewLine & "finished project", "ExportIntern", _
'                                                   CommandTypesEnum.kFileOperationsCmdType, _
'                                                   m_clientID, "Translate the finished project to destination folder", , _
'                                                   exportPicSmall, exportPicLarge)
'    MIDPanelOne.CommandControls.AddButton(oExportButton, True)
'    #######################################################################

'    Add Ribbon Tab #2
'    Dim MIDRibbonTabTwo As RibbonTab = MIDRibbon.RibbonTabs.Add("Some Analysis Extras", "SomeAnalysisExtras", m_clientID, , , True)

'    Dim MIDPanelThree As RibbonPanel = MIDRibbonTabTwo.RibbonPanels.Add("Panel Three", "PanelThree", m_clientID)
'    Dim MIDButtonThree As ButtonDefinition = MidAddIn.CommandManager.ControlDefinitions.Item("PartCoilCmd")
'    MIDPanelThree.CommandControls.AddButton(MIDButtonThree, True)




'    Associate the contextual tabs with the newly created environment
'    The contextual tabs will only be displayed when this environment is active
'    Dim strTabs(1) As String
'    strTabs(0) = "MIDTabIntern"
'    strTabs(1) = "SomeAnalysisExtras"

'    MIDNewEnv.AdditionalVisibleRibbonTabs = strTabs

'    Make the "SomeAnalysis" tab default for the environment
'    MIDNewEnv.DefaultRibbonTab = "MIDTabIntern"

'    get the collection of parallel environments and add the new environment
'    Dim MIDParEnvs As EnvironmentList = MidAddIn.UserInterfaceManager.ParallelEnvironments
'    MIDParEnvs.Add(MIDNewEnv)



'     Remove the environment if no part or assembly environment open
'    Dim Env As Inventor.Environment
'    For Each Env In MIDEnvs
'        If Not (Env.InternalName = "PMxPartEnvironment" Or Env.InternalName = "AMxAssemblyEnvironment") Then
'            On Error Resume Next
'            Env.DisabledCommandList.Add(MIDParEnvButton)
'        End If
'    Next



'    MsgBox(MidAddIn.CommandManager.ControlDefinitions.Count)
'    For i As Long = 1 To MidAddIn.CommandManager.ControlDefinitions.Count
'        If MidAddIn.CommandManager.ControlDefinitions.Type = 50371584 Then
'            MsgBox(MidAddIn.CommandManager.ControlDefinitions.ToString)
'        End If

'    Next
'    cpy the metal ribbon tab (for testing)
'    MIDNewEnv.DefaultRibbonTab = MIDEnv.DefaultRibbonTab
'    Dim sTabs(0 To 1) As String
'    sTabs(0) = "id_TabSheetMetal"
'    sTabs(1) = "id_TabFlatPattern"

'    MIDNewEnv.AdditionalVisibleRibbonTabs = sTabs
'    MIDNewEnv.InheritAllRibbonTabs = True

'    MIDNewEnv.PanelBar.CommandBarList.InheritAll = True
'    MIDNewEnv.ContextMenuList.InheritAll = True


'End Sub


'Public Sub PrintRibbon()
'    Open "C:\temp\RibbonNames.txt" For Output As #1

'    Debug.Write("File Controls (Application Menu)")
'    PrintControls(MidAddIn.UserInterfaceManager.FileBrowserControls, "", 1)
'    Debug.WriteLine("------------------------------------------------------------------")

'    Debug.WriteLine("Help Controls")
'    PrintControls(MidAddIn.UserInterfaceManager.HelpControls, "", 1)
'    Debug.WriteLine("------------------------------------------------------------------")

'    Dim oRibbon As Ribbon
'    For Each oRibbon In MidAddIn.UserInterfaceManager.Ribbons
'        Debug.WriteLine("Ribbon: " & oRibbon.InternalName)

'        Debug.WriteLine("    QAT controls")
'        PrintControls(oRibbon.QuickAccessControls, "            ", 0)

'        Dim oTab As RibbonTab
'        For Each oTab In oRibbon.RibbonTabs
'            Debug.WriteLine("    Tab: " & oTab.DisplayName & ", " & oTab.InternalName & ", Visible: " & oTab.Visible)

'            Dim oPanel As RibbonPanel
'            For Each oPanel In oTab.RibbonPanels
'                Debug.WriteLine("        Panel: " & oPanel.DisplayName & ", " & oPanel.InternalName & ", Visible: " & oPanel.Visible)

'                PrintControls(oPanel.CommandControls, "            ", 0)

'                If oPanel.SlideoutControls.Count > 0 Then
'                    Debug.WriteLine("            --- Slideout Controls ---")
'                    PrintControls(oPanel.SlideoutControls, "            ", 0)
'                End If
'            Next
'        Next

'        Debug.WriteLine("------------------------------------------------------------------")
'    Next
'    On Error GoTo 0

'      Close #1

'    MsgBox "Result written to: C:\temp\RibbonNames.txt"
'End Sub

'Private Sub PrintControls(Controls As CommandControls, LeadingSpace As String, Level As Integer)
'    Dim oControl As CommandControl
'    For Each oControl In Controls
'        If oControl.ControlType = ControlTypeEnum.kSeparatorControl Then
'            Debug.WriteLine(LeadingSpace & Space(Level * 4) & "Control: Seperator")
'        Else
'            Debug.WriteLine(LeadingSpace & Space(Level * 4) & "Control: " & oControl.DisplayName & ", " & oControl.InternalName & ", Visible: " & oControl.Visible)

'            If Not oControl.ChildControls Is Nothing Then
'                Call PrintControls(oControl.ChildControls, LeadingSpace, Level + 1)
'            End If
'        End If
'    Next
'End Sub





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



'End Namespace

'Get the ribbons associated with part documents
'Dim ribbons As Ribbons = oUserInterfaceMgr.Ribbons
'Dim partRibbon As Ribbon = ribbons("Assembly")

''Get the tabs associated with part ribbon
'Dim ribbonTabs As RibbonTabs = partRibbon.RibbonTabs
'Dim modelRibbonTab As RibbonTab = ribbonTabs("id_TabAssemble")

''Get the panels within Model tab
'Dim ribbonPanels As RibbonPanels = modelRibbonTab.RibbonPanels
'Dim modifyRibbonPanel As RibbonPanel = ribbonPanels("id_PanelA_AssembleComponent")

''Add controls to the Modify panel
'Dim modifyRibbonPanelCtrls As CommandControls = modifyRibbonPanel.CommandControls

''Add button to the Modify panel
'modifyRibbonPanelCtrls.AddButton(oMenu.oKeepOutButton)