Option Strict Off
Option Explicit On

Imports Inventor
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports System.Xml
Imports MIDProjectVB3.ExportBRep


'############################################
' New Project Command
'############################################

Public Class PlaceCommand
    Inherits Command

    'Private WithEvents oInteractionEvents As InteractionEvents
    'Private WithEvents oSelectEvents As SelectEvents

    'Private oEntityList As Inventor.ObjectsEnumerator

    Private Shared strFolderPath As String

    Private oCmdColl As CommandCollection

    Dim oServer As MidAddInServer

    ' constructor
    '***************************************************************************************************************
    Public Sub New(ByVal MidAddIn As Inventor.Application, _
                   ByVal _Server As MidAddInServer)

        ' call base class
        MyBase.New(MidAddIn)

        Me.oServer = _Server

        strFolderPath = Nothing

        ' Create new Browser object
        'oBrowser = New MidBrowser(MidAddIn, oCmdColl.ClientId)

    End Sub

    ' Get the work directory path
    '***************************************************************************************************************
    Public Shared ReadOnly Property GetWorkDirPath() As String
        Get
            Return strFolderPath
        End Get
    End Property

    ' Check if an circuitcarrier already exists
    '***************************************************************************************************************
    Private Function FindCircuitCarrier() As Boolean

        Dim oAsmDoc As AssemblyDocument = MidAddIn.ActiveDocument
        Dim oAttribSets As AttributeSets

        For Each oOcc As ComponentOccurrence In oAsmDoc.ComponentDefinition.Occurrences
            oAttribSets = oOcc.AttributeSets

            If oAttribSets.NameIsUsed("circuitcarrier") Then
                Return True
            End If

        Next
        Return False

    End Function

    ' Initialize button execution
    '***************************************************************************************************************
    Protected Overrides Sub oButtonDefinition_OnExecute()
        ' Only execute if assemlby environment
        'Dim currEnvironment As Inventor.Environment = MidAddIn.UserInterfaceManager.ActiveEnvironment

        'If LCase$(currEnvironment.InternalName) <> LCase$("AMxAssemblyEnvironment") Then
        '    MessageBox.Show("This command applies only to assembly environment", _
        '                    "MIDProject", _
        '                    MessageBoxButtons.OK, _
        '                    MessageBoxIcon.Error)
        '    Exit Sub
        'End If

        ' Init important inventor classes
        'oTG = MidAddIn.TransientGeometry
        'oUOF = MidAddIn.ActiveDocument.UnitsOfMeasure

       

        'If bCommandIsRunning Then
        '    StopCommand()
        'End If

        If FindCircuitCarrier() Then
            MessageBox.Show("There is already a MID circuit carrier. Please remove it before placing a new one", "MID Project", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        'Start new command
        StartCommand()
    End Sub


    ' Start/Stop the command
    '***************************************************************************************************************
    Public Overrides Sub StartCommand()

        Dim oFileDlg As Inventor.FileDialog
        MidAddIn.CreateFileDialog(oFileDlg)

        oFileDlg.Filter = "Inventor Part File |*ipt"
        oFileDlg.DialogTitle = "Open MID-Part"
        oFileDlg.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        oFileDlg.CancelError = False

        oFileDlg.ShowOpen()
        Dim oFileName As String = oFileDlg.FileName


        Dim oAsmDoc As AssemblyDocument = MidAddIn.ActiveDocument()

        Dim oPosMatrix As Matrix = MidAddIn.TransientGeometry.CreateMatrix()

        Dim oCompOcc As ComponentOccurrence = oAsmDoc.ComponentDefinition.Occurrences.Add(oFileName, oPosMatrix)
        oCompOcc.Grounded = False

        'Dim oCompOcc As ComponentOccurrence = oAsmDoc.ComponentDefinition.Occurrences.AddByComponentDefinition(oCompDef, oPosMatrix) '.Add(oFileName, _

        ' Apply attribute to the Mid
        Dim oAttribSets As AttributeSets = oCompOcc.AttributeSets
        Dim oAttribSet As AttributeSet = oAttribSets.Add("circuitcarrier")
        Dim oAttrib As Inventor.Attribute

        'Dim oFaces As Faces = oCompOcc.SurfaceBodies.Item(1).Faces
        'For Each oFace As Face In oFaces

        '    oAttribSets = oFace.AttributeSets
        '    oAttribSet = oAttribSets.Add("midFace")
        '    ' Set all faces available for placing components
        '    oAttrib = oAttribSet.Add("isKeepOut", ValueTypeEnum.kIntegerType, 0) 'Bug: kBoolean doesnt work
        '    ' Set routing allowed by default.
        '    oAttrib = oAttribSet.Add("routingAllowed", ValueTypeEnum.kIntegerType, 1)
        'Next
        ' oPosMatrix)
        '#### check for number of surfacebodies!!! only one allowed

        Dim oFaceStyle As RenderStyle = oAsmDoc.RenderStyles.Item("Default")
        ' oCompOcc.SetRenderStyle(StyleSourceTypeEnum.kOverrideRenderStyle, oFaceStyle)

        'Dim oControls As ControlDefinitions = MidAddIn.CommandManager.ControlDefinitions
        'Dim oControl As ControlDefinition
        'oControl = oControls.Item("AssemblyMirrorComponentCmd")
        'oControl.Enabled = True

        'oCmdColl.EnableDefinition(CommandCollection.CommandEnableEnum.kKeepOutEnable)


        oServer.Browser.InsertChildNode(oAsmDoc, oCompOcc, "Circuit Carrier")

        ' Enable other commands
        Dim oControls As ControlDefinitions = MidAddIn.CommandManager.ControlDefinitions
        Dim oControl As ControlDefinition
        oControl = oControls.Item("ImportIntern")
        oControl.Enabled = True
        oControl = oControls.Item("KeepOutsIntern")
        oControl.Enabled = True
        oControl = oControls.Item("MoveMidIntern")
        oControl.Enabled = True


        'oKeepOutButton.Enabled = True
        'oExportButton.Enabled = True
        'oImportButton.Enabled = True
        ''oExportSetButton.Enabled = True
        'oPlaceCompButton.Enabled = True

        ' Create new form dialog
        'Dim oDlg As New NewProjectCmdDlg(MidAddIn, Me)
        'oDlg.ShowInTaskbar() = True
        'oDlg.TopMost() = True

        ' Show dialog and get the path
        'strFolderPath = oDlg.SetPath()

        'Dim oAsmDoc As AssemblyDocument = MidAddIn.ActiveDocument
        'If oAsmDoc IsNot Nothing Then
        '    oAsmDoc.Close(False)
        'End If

        'oAsmDoc = MidAddIn.Documents.Add(DocumentTypeEnum.kAssemblyDocumentObject)
        'oAsmDoc.EnvironmentManager.SetCurrentEnvironment(MidAddIn.UserInterfaceManager.Environments.Item("MidEnvironment"))


        ' Stop Command
        StopCommand()

        'oPlaceButton.Enabled = True

    End Sub

    'Stop Command
    '***************************************************************************************************************
    Public Overrides Sub StopCommand()

        ' Destroy the command dialog
        'oDlg = Nothing

    End Sub




End Class