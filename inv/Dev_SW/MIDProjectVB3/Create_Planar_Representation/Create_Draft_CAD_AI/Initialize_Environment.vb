Imports Inventor
Imports System
Imports System.Type
Imports System.Activator
Imports System.Runtime.InteropServices
Imports System.Xml.XmlReader




Module Initialize_Environment
    Sub main()
        Dim _invApp As Inventor.Application
        Try
            _invApp = Marshal.GetActiveObject("Inventor.Application")
            ' Set a reference to the component definition.
            Dim oPartCompDef As PartComponentDefinition
            oPartCompDef = _invApp.ActiveDocument.ComponentDefinition

            ' Create a new sketch.
            Dim oSketch As Sketch
            oSketch = oPartCompDef.Sketches.Add(oPartCompDef.WorkPlanes.Item(3))
            Call XMLReader_main(_invApp, oPartCompDef, oSketch)

            'Direktaufruf der Extrude Funktion



            'Call create_line(_invApp, oSketch, oPartCompDef) ' pointA, pointB)

        Catch ex As Exception
            Try
                Dim invAppType As Type = _
                GetTypeFromProgID("Inventor.Application")
                _invApp = CreateInstance(invAppType)
                _invApp.Visible = True
            Catch ex2 As Exception
                MsgBox(ex2.ToString())
                MsgBox("Unable to get or start Inventor")
            End Try
        End Try
    End Sub

    Sub identifyType()

    End Sub

 
End Module

