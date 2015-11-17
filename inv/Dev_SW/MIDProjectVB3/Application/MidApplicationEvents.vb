Public Class MidApplicationEvents

    Private oAddIn As Inventor.Application

    Private WithEvents oApplicationEvents As Inventor.ApplicationEvents
    ' Private oCustomBrowserPane As Browser

    Private oServer As MidAddInServer

    Public Sub New(ByVal oAddIn As Inventor.Application, ByVal oServer As MidAddInServer)

        MyBase.New()

        Me.oAddIn = oAddIn
        Me.oServer = oServer
        ' Initialize application events
        oApplicationEvents = oAddIn.ApplicationEvents

        'initialize custom browser pane
        'oCustomBrowserPane = New Browser(oAddIn, strClientID)

    End Sub

    Private Sub ApplicationEvents_OnActivateDocument(documentObject As Document, _
                                                     beforeOrAfter As EventTimingEnum, _
                                                     context As NameValueMap, _
                                                     ByRef handlingCode As HandlingCodeEnum) Handles oApplicationEvents.OnActivateDocument

        'Dim oEnvironments As Environments = oAddIn.UserInterfaceManager.Environments

        'Dim oEnv As Inventor.Environment

        'For Each oEnv In oEnvironments
        '    If oEnv.InternalName.Equals("MidEnvironment") Then
        '        Exit For
        '    End If
        'Next

        'Dim EnvironmentMrg As EnvironmentManager = documentObject.EnvironmentManager
        'EnvironmentMrg.SetCurrentEnvironment(oEnv)


        'MsgBox(oAddIn.UserInterfaceManager.ActiveEnvironment.InternalName())

        If beforeOrAfter = Inventor.EventTimingEnum.kBefore Then
            If documentObject.DocumentType = Inventor.DocumentTypeEnum.kAssemblyDocumentObject Then
                'If oAddIn.UserInterfaceManager.ActiveEnvironment Is Then

                ' Create new browser
                oServer.Browser.InitBrowser(documentObject)
            End If
        End If
        'End If

    End Sub

End Class
