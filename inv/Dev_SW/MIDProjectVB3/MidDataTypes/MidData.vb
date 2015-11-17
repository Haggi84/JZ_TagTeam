Option Explicit On

Imports System.Windows.Forms

'###########################################
' Circuit Carrier Class
'###########################################

Public Class MidData

    Private oAddIn As Inventor.Application
    Private oServer As MidAddInServer

    Private oCircuitCarrier As CircuitCarrier

    Private oBoard As CircuitBoard

    ' Constructor
    '**********************************************************************************************************
    Public Sub New(ByVal addIn As Inventor.Application, _
                   ByVal server As MidAddInServer)

        Me.oAddIn = addIn
        Me.oServer = server
        oBoard = Nothing
        oCircuitCarrier = Nothing

    End Sub

    ' Add CircuitCarrier (via occurrence)
    '**********************************************************************************************************
    Public Function AddCircuitCarrier(oOccurrence As ComponentOccurrence) As CircuitCarrier

        oCircuitCarrier = New CircuitCarrier(oAddIn, oServer)

        oCircuitCarrier.CreateIRep(oOccurrence)
        Return oCircuitCarrier

    End Function

    ' Add CircuitCarrier (via fileName)
    '**********************************************************************************************************
    Public Function AddCircuitCarrier(fileName As String) As CircuitCarrier

        oCircuitCarrier = New CircuitCarrier(oAddIn, oServer)

        ' Add Occurrence
        Try
            Dim oAsmDoc As AssemblyDocument = oAddIn.ActiveDocument()
            Dim oPosMatrix As Matrix = oAddIn.TransientGeometry.CreateMatrix()
            Dim oOccurrence As ComponentOccurrence = oAsmDoc.ComponentDefinition.Occurrences.Add(fileName, oPosMatrix)
            oOccurrence.Grounded = False
            oCircuitCarrier.CreateIRep(oOccurrence)
        Catch ex As Exception
            oCircuitCarrier = Nothing
            System.Windows.Forms.MessageBox.Show("There was an error creating the circuit carrier", "MID Project", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            Return Nothing
        End Try
        Return oCircuitCarrier

    End Function

    ' Properties
    '**********************************************************************************************************
    Public Property CircuitCarrier As CircuitCarrier
        Get
            Return oCircuitCarrier
        End Get
        Set(value As CircuitCarrier)
            oCircuitCarrier = value
        End Set
    End Property

    Public Property CircuitBoard As CircuitBoard
        Get
            Return oBoard
        End Get
        Set(value As CircuitBoard)
            oBoard = value
        End Set
    End Property

End Class
