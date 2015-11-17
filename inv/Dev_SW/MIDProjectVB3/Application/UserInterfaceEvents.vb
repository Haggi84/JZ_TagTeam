Option Strict Off
Option Explicit On

'############################################
' ApplicationEvents
'############################################

Public Class UserInterfaceEvents

    Private oAddIn As Inventor.Application
    Private oServer As MidAddInServer

    Private WithEvents oUserInterfaceEvents As Inventor.UserInterfaceEvents

    ' Constructor
    '********************************************************************************************************************
    Public Sub New(ByVal oAddIn As Inventor.Application, _
                   ByVal oServer As MidAddInServer)

        MyBase.New()

        Me.oAddIn = oAddIn
        Me.oServer = oServer
      
        ' Get the user interface manager
        Dim oUserInterfaceMgr As Inventor.UserInterfaceManager = oAddIn.UserInterfaceManager

        'initialize user interface events
        oUserInterfaceEvents = oUserInterfaceMgr.UserInterfaceEvents

    End Sub

    ' This event is called four times! 
    ' First time: Before active environment is canceled (EventTimingEnum.kBefore)
    ' Second time: After active enviroment is canceled (EventTimingEnum.kAfter)
    ' third time: Before new environment is activated (EventTimingEnum.kBefore)
    ' fourth time: After new enviroment is activated (EventTimingEnum.kAfter)
    '********************************************************************************************************************
    Private Sub oUserInterfaceEvents_OnEnvChange(ByVal environment As Inventor.Environment, _
                                                 ByVal environmentState As EnvironmentStateEnum, _
                                                 ByVal beforeOrAfter As Inventor.EventTimingEnum, _
                                                 ByVal context As Inventor.NameValueMap, _
                                                 ByRef handlingCode As Inventor.HandlingCodeEnum) Handles oUserInterfaceEvents.OnEnvironmentChange


        If environment.InternalName = "OPTAVER" Then
            'MsgBox("assemlby is chaning")

            If (environmentState = EnvironmentStateEnum.kActivateEnvironmentState Or environmentState = EnvironmentStateEnum.kResumeEnvironmentState) And beforeOrAfter = EventTimingEnum.kAfter Then

                ' Activate new mid browser tree
                oServer.Browser.ActivateMidTree(oAddIn.ActiveDocument)

                ' Disable all interfering command
                '### add all controls later here
                Dim oControls As ControlDefinitions = oAddIn.CommandManager.ControlDefinitions
                Dim oControl As ControlDefinition
                oControl = oControls.Item("AssemblyMirrorComponentCmd")
                oControl.Enabled = False
                oControl = oControls.Item("AssemblyMoveComponentCmd")
                oControl.Enabled = False
                oControl = oControls.Item("AssemblyPatternComponentCmd")
                oControl.Enabled = False
                oControl = oControls.Item("AssemblyPlaceComponentCmd")
                oControl.Enabled = False
                oControl = oControls.Item("AssemblyPromoteCmd")
                oControl.Enabled = False

            End If

            If (environmentState = EnvironmentStateEnum.kTerminateEnvironmentState Or environmentState = EnvironmentStateEnum.kSuspendEnvironmentState) And beforeOrAfter = EventTimingEnum.kAfter Then

                ' Activate new mid browser tree
                If Not oServer.Browser.ActivateDefaultTree(oAddIn.ActiveDocument) Then
                    Dim oErrorMgr As ErrorManager = oAddIn.ErrorManager
                    oErrorMgr.AddMessage("The application was not able to restore default browser", False)
                End If

                ' Enable all commands
                Dim oControls As ControlDefinitions = oAddIn.CommandManager.ControlDefinitions
                Dim oControl As ControlDefinition
                oControl = oControls.Item("AssemblyMirrorComponentCmd")
                oControl.Enabled = True
                oControl = oControls.Item("AssemblyMoveComponentCmd")
                oControl.Enabled = True
                oControl = oControls.Item("AssemblyPatternComponentCmd")
                oControl.Enabled = True
                oControl = oControls.Item("AssemblyPlaceComponentCmd")
                oControl.Enabled = True
                oControl = oControls.Item("AssemblyPromoteCmd")
                oControl.Enabled = True

            End If

        Else
            ' not implemented
        End If

    End Sub

    ' Reset Ribbon Interface
    '********************************************************************************************************************
    Private Sub oUserInterfaceEvents_OnResetRibbonInterface(ByVal context As Inventor.NameValueMap) Handles oUserInterfaceEvents.OnResetRibbonInterface
        Try
            Dim oUserInterfaceMgr As Inventor.UserInterfaceManager = oAddIn.UserInterfaceManager

            Dim oOPTAVER As Inventor.Environment = oAddIn.UserInterfaceManager.Environments("OPTAVER")

            'Retrieve the assembly ribbon
            Dim oAsmRibbon As Ribbon = oAddIn.UserInterfaceManager.Ribbons.Item("Assembly")

            ' Insert new ribbon tab before the "Assemble" tab
            Dim oRibbonTab As RibbonTab = oAsmRibbon.RibbonTabs.Add("MID", "MidInternAsm", "clientId", "id_TabAssemble", True, True)

            ' Add new ribbon panels to the tab
            Dim oPanelManage As RibbonPanel = oRibbonTab.RibbonPanels.Add("Manage", "manageIntern", "clientId")
            Dim oPanelSelect As RibbonPanel = oRibbonTab.RibbonPanels.Add("Select", "selectIntern", "clientId")
            Dim oPanelExport As RibbonPanel = oRibbonTab.RibbonPanels.Add("Export", "exportIntern", "clientId")
            Dim oPanelImport As RibbonPanel = oRibbonTab.RibbonPanels.Add("Import", "importIntern", "clientId")

            ' ###Add commandcontrols here
            'oPanelManage.CommandControls.AddButton(oAddIn.CommandManager.ControlDefinitions.Item("

            Dim strTabs(0) As String
            strTabs(0) = "MidInternAsm"

            ' Attach ribbon tab to the environment
            oOPTAVER.AdditionalVisibleRibbonTabs = strTabs

            'Make the "SomeAnalysis" tab default for the environment
            oOPTAVER.DefaultRibbonTab = "MidInternAsm"
        Catch ex As Exception
            System.Windows.Forms.MessageBox.Show(ex.ToString())
        End Try

    End Sub

End Class