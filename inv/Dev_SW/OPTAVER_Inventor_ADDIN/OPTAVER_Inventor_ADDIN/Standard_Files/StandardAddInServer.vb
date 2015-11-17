Imports Inventor
Imports System.Runtime.InteropServices
Imports Microsoft.Win32


Namespace OPTAVER_Inventor_ADDIN
    <ProgIdAttribute("OPTAVER_Inventor_ADDIN.StandardAddInServer"), _
    GuidAttribute("32b8e711-661e-44f9-a066-ed1e5da7d1c9")> _
    Public Class StandardAddInServer
        Implements Inventor.ApplicationAddInServer

        ' Inventor application object.
        Private m_inventorApplication As Inventor.Application
        Private oAddIn As Inventor.Application
        Private WithEvents oButtonDefinition As ButtonDefinition
        Public strMidAddInCLSID As String

#Region "ApplicationAddInServer Members"

        Public Sub Activate(ByVal addInSiteObject As Inventor.ApplicationAddInSite, ByVal firstTime As Boolean) Implements Inventor.ApplicationAddInServer.Activate
            Dim sw As New InawSplashWindow(600, 400, 10000, System.Drawing.Image.FromStream(Me.GetType().Assembly.GetManifestResourceStream(System.Environment.GetEnvironmentVariable("OPTAVER_DEV") + "\Ressources\Pictures\Logo_groﬂ_komplett.bmp")))

            ' This method is called by Inventor when it loads the AddIn.
            ' The AddInSiteObject provides access to the Inventor Application object.
            ' The FirstTime flag indicates if the AddIn is loaded for the first time.

            ' Initialize AddIn members.
            m_inventorApplication = addInSiteObject.Application

            ' TODO:  Add ApplicationAddInServer.Activate implementation.
            ' e.g. event initialization, command creation etc.

            ' Add new button
            oButtonDefinition = oAddIn.CommandManager.ControlDefinitions.AddButtonDefinition("New Button", "NewButtonIntern", CommandTypesEnum.kEditMaskCmdType, strMidAddInCLSID)

            If firstTime Then
                'Add the button to the part features toolbar
                Dim userInterfaceMgr As UserInterfaceManager = oAddIn.UserInterfaceManager
 


                'Get the ribbons associated with part documents
                Dim oRibbons As Ribbons = userInterfaceMgr.Ribbons
                Dim oZeroRibbon As Ribbon = oRibbons.Item("ZeroDoc")

                'Get the tabs associated with part ribbon
                Dim oRibbonTabs As RibbonTabs = oZeroRibbon.RibbonTabs
                Dim oRibbonTab As RibbonTab = oRibbonTabs("id_GetStarted")

                'Get the panels within Model tab
                Dim oRibbonPanels As RibbonPanels = oRibbonTab.RibbonPanels
                Dim oRibbonPanel As RibbonPanel = oRibbonPanels("id_Panel_GetStartedWhatsNew")

                'Add controls to the Modify panel
                Dim oRibbonPanelCtrls As CommandControls = oRibbonPanel.CommandControls

                'Add button to the Modify panel
                oRibbonPanelCtrls.AddButton(oButtonDefinition)

            End If


        End Sub

        Public Sub Deactivate() Implements Inventor.ApplicationAddInServer.Deactivate

            ' This method is called by Inventor when the AddIn is unloaded.
            ' The AddIn will be unloaded either manually by the user or
            ' when the Inventor session is terminated.

            ' TODO:  Add ApplicationAddInServer.Deactivate implementation

            ' Release objects.
            m_inventorApplication = Nothing

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

    End Class

End Namespace

