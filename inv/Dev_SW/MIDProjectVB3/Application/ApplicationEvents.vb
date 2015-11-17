Option Strict Off
Option Explicit On

'############################################
' ApplicationEvents
'############################################

Public Class ApplicationEvents

    Private oAddIn As Inventor.Application

    Private WithEvents oApplicationEvents As Inventor.ApplicationEvents
    ' Private oCustomBrowserPane As Browser

    Private oServer As MidAddInServer

    Public Sub New(ByVal oAddIn As Inventor.Application, _
                   ByVal oServer As MidAddInServer)

        MyBase.New()

        Me.oAddIn = oAddIn
        Me.oServer = oServer

        ' Initialize application events
        oApplicationEvents = oAddIn.ApplicationEvents

    End Sub

    ' EVENT: fires when document is activated
    '****************************************************************************************************************
    Private Sub ApplicationEvents_OnActivateDocument(documentObject As Document, _
                                                     beforeOrAfter As EventTimingEnum, _
                                                     context As NameValueMap, _
                                                     ByRef handlingCode As HandlingCodeEnum) Handles oApplicationEvents.OnActivateDocument

        If beforeOrAfter = Inventor.EventTimingEnum.kAfter Then
            If documentObject.DocumentType = Inventor.DocumentTypeEnum.kAssemblyDocumentObject Then

                ' Initialize the browser (create a new one if no browser exists)
                oServer.Browser.Initialize(documentObject)

            End If
        End If

    End Sub


    ' EVENT: not implemented yet
    '****************************************************************************************************************
    'Private Sub ApplicationEvents_OnNewDocument(documentObject As Document, _
    '                                             beforeOrAfter As EventTimingEnum, _
    '                                             context As NameValueMap, _
    '                                             ByRef handlingCode As HandlingCodeEnum) Handles oApplicationEvents.OnNewDocument

    '    If beforeOrAfter = Inventor.EventTimingEnum.kAfter Then
    '        If documentObject.DocumentType = Inventor.DocumentTypeEnum.kAssemblyDocumentObject Then

    '            ' Create new browser

    '            '  documentObject.Activate()
    '        End If


    '    End If

    'End Sub

    'Private Sub ApplicationEvents_OnOpenDocument(documentObject As Document, _
    '                                             beforeOrAfter As EventTimingEnum, _
    '                                             context As NameValueMap, _
    '                                             ByRef handlingCode As HandlingCodeEnum) Handles oApplicationEvents.OnNewDocument

    '    If beforeOrAfter = EventTimingEnum.kAfter Then
    '        'oServer.Browser.ResetBrowser(documentObject)
    '    End If


    'End Sub


    'Private Sub ApplicationEvents_OnNewDocument(documentObject As Document, _
    '                                            beforeOrAfter As EventTimingEnum, _
    '                                            context As NameValueMap, _
    '                                            ByRef HandlingCode As HandlingCodeEnum) Handles oApplicationEvents.OnNewDocument

    '    'If beforeOrAfter = EventTimingEnum.kAfter And oServer.CommandCollection.NewProjectCommand.DocumentName IsNot Nothing Then
    '    '    documentObject.DisplayName = oServer.CommandCollection.NewProjectCommand.DocumentName
    '    'End If

    'End Sub

End Class
