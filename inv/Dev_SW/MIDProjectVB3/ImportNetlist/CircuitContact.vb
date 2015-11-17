﻿Imports Inventor

'##############################################
' CONTACT CLASS
'##############################################

Public Class CircuitContact

    Private _partId As String
    Private _pinId As String
    Private oPart As CircuitPart
    Private oPin As CircuitPin

    Private oAddIn As Inventor.Application
    Private oPoint As Point

    ' Constructor
    '****************************************************************************************************************
    Public Sub New(oAddIn As Inventor.Application)

        Me.oAddIn = oAddIn

        oPart = Nothing
        oPin = Nothing

    End Sub

    ' Properties
    '*****************************************************************************************************************************
    Public ReadOnly Property Position() As Point
        Get
            ' MsgBox(Pin.Point.X & " " & Pin.Point.Y & " " & Pin.Point.Z)
            Return Pin.Point
        End Get
    End Property

    Public Property Pin() As CircuitPin
        Get
            Return oPin
        End Get
        Set(value As CircuitPin)
            oPin = value
        End Set
    End Property

    Public Property Part() As CircuitPart
        Get
            Return oPart
        End Get
        Set(value As CircuitPart)
            oPart = value
        End Set
    End Property

    Public Property PartId As String
        Get
            Return _partId
        End Get
        Set(value As String)
            _partId = value
        End Set
    End Property

    Public Property PinId As String
        Get
            Return _pinId
        End Get
        Set(value As String)
            _pinId = value
        End Set
    End Property

End Class


'Public Sub EvalPointCoord()
'    For i As Integer = 0 To oParts.Count() - 1
'        ' find the part with the partId

'        If String.Equals(oParts.Item(i).GetId, partId) Then
'            'now find the pin with the pinId
'            For j As Integer = 0 To oParts.Item(i).Pins.Count() - 1
'                If String.Equals(oParts.Item(i).Pins.Item(j).Id, pinId) Then
'                    ' save the point of the contact of the part
'                    oPoint = oParts.Item(i).Pins.Item(j).GetPoint()

'                End If
'            Next

'        End If
'    Next

'End Sub

'Public ReadOnly Property Point() As Point
'    Get
'        Return oPoint
'    End Get

'End Property