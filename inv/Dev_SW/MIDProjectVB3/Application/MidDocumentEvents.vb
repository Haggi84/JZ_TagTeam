Public Class MidDocumentEvents

    Private oAddin As Inventor.Application
    Private oServer As MidAddInServer

    Private WithEvents oDocumentEvents As Inventor.DocumentEvents


    Public Sub New(ByVal oAddIn As Inventor.Application, _
                   oServer As MidAddInServer)

        MyBase.New()

        Me.oServer = oServer
        Me.oAddin = oAddIn


    End Sub

    Public Sub Init(ByVal oAsmDoc As AssemblyDocument)

        ' Set Reference
        oDocumentEvents = oAsmDoc.DocumentEvents


    End Sub

    Private Sub oDocumentEvents_OnDelete(entity As Object, _
                                         beforeOrAfter As EventTimingEnum, _
                                         context As NameValueMap, _
                                         ByRef handlingCode As HandlingCodeEnum) Handles oDocumentEvents.OnDelete

        handlingCode = HandlingCodeEnum.kEventCanceled



        ' Delete circuit carrier
        'If beforeOrAfter = EventTimingEnum.kAfter Then
        '    If TypeOf (entity) Is ComponentOccurrence Then
        '        If entity Is oServer.MidDataTypes.CircuitCarrier.Occurrence Then
        '            oServer.MidDataTypes.CircuitCarrier = Nothing
        '        End If
        '    End If

        'End If

    End Sub


End Class