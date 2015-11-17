Option Explicit On
Imports Inventor
Imports System.Runtime.InteropServices
Imports Microsoft.Win32

Public Class Klasse_Auswahl

    ' Declare the event objects
    Private WithEvents oInteractEv As InteractionEvents
    Private WithEvents oSelectEv As SelectEvents

    ' Declare a flag that's used to determine when selection stops.
    Private bStillSelecting As Boolean

    'VB.net Vorschlag aus cad.de -vb.net
    'Public Function Pick(ByVal filter As SelectionFilterEnum, ByVal oAppOselect As Application) As Object
    Public Function Pick(filter As SelectionFilterEnum) As Object

        Dim mApp As Inventor.Application
        mApp = Marshal.GetActiveObject("Inventor.Application")

        bStillSelecting = True

        'Laut cad.de über oAppOSelect
        ' Create an InteractionEvents object.
        'oInteractEv = mApp.CommandManager.CreateInteractionEvents
        oInteractEv = mApp.CommandManager.CreateInteractionEvents

        ' Ensure interaction is enabled.
        oInteractEv.InteractionDisabled = False

        ' Define that we want select events
        oInteractEv.SelectionActive = True

        ' Set a reference to the select events.
        oSelectEv = oInteractEv.SelectEvents

        ' Set the filter using the value passed in.
        oSelectEv.AddSelectionFilter(filter)

        ' Start the InteractionEvents object.
        'Hier ändert sich der Mauszeiger zum Auswahl Cursor
        'es lässt sich aber keine Fläche auswählen
        oInteractEv.Start()

        'MsgBox("Select the face you want to measure")
        'Mit  Pick lässt sich eine Fläche auswählen, aber 
        'ich weiß nicht wie man sie einem OBjekt zuweist, um
        'die Fläche weiter verwenden zu können
        'mApp.CommandManager.Pick(Inventor.SelectionFilterEnum.kAllPlanarEntities, "Pick planar")

        ' Loop until a selection is made.
        Do While bStillSelecting
            'Was macht DoEvents()?
            Try
                'mApp.CommandManager.Pick(Inventor.SelectionFilterEnum.kAllPlanarEntities, "Pick planar")
                System.Windows.Forms.Application.DoEvents()
                'mApp.UserInterfaceManager.DoEvents()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try


        Loop

        ' Get the selected item. If more than one thing was selected,
        'get the first item and ignore the rest.
        Dim oSelectedEnts As ObjectsEnumerator

        oSelectedEnts = oSelectEv.SelectedEntities
        If oSelectedEnts.Count > 0 Then
            Pick = oSelectedEnts.Item(1)
        Else
            Pick = Nothing
        End If

        ' Stop the InteractionEvents object.
        oInteractEv.Stop()

        ' Clean up.
        oSelectEv = Nothing
        oInteractEv = Nothing
    End Function

    Private Sub oInteractEvents_OnTerminate()
        ' Set the flag to indicate we're done.
        bStillSelecting = False
    End Sub

    Private Sub oSelectEvents_OnSelect(ByVal JustSelectedEntities As ObjectsEnumerator, ByVal SelectionDevice As SelectionDeviceEnum, ByVal ModelPosition As Point, ByVal ViewPosition As Point2d, ByVal View As View) Handles oSelectEv.OnSelect
        ' Set the flag to indicate we're done.
        bStillSelecting = False
    End Sub

End Class
