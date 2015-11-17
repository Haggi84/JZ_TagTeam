Option Strict Off
Option Explicit On

'############################################
' MID Browser
'############################################

Public Class Browser

    Private oAddIn As Inventor.Application
    Private oServer As MidAddInServer

    Private Const stdPictureId As Integer = 1
    'Private Const mlngFaceNodePictureId As Short = 2
    'Private Const mlngEdgeLoopNodePictureId As Short = 3
    'Private Const mlngEdgeNodePictureId As Short = 4

    Private nodeId As Integer
    Private oBrowserPanes As Inventor.BrowserPanes
    Private oBrowserPane As Inventor.BrowserPane

    'Private strLabel As String

    Private WithEvents oBrowserPanesEvents As Inventor.BrowserPanesEvents

    ' Constructor
    '*****************************************************************************************************************************
    Public Sub New(ByVal oAddIn As Inventor.Application, _
                   ByVal oServer As MidAddInServer)

        MyBase.New()

        Me.oAddIn = oAddIn
        Me.oServer = oServer
        Me.oServer = oServer

        nodeId = 1

    End Sub

    ' Initialize browser (OnChangeDocument --> after new Document has been loaded or activated)
    '*****************************************************************************************************************************
    Public Sub Initialize(ByRef document As Inventor.Document)

        Try
            ' Get the browser panes of the document
            oBrowserPanes = document.BrowserPanes

            ' Retrieve the browser panes events
            oBrowserPanesEvents = oBrowserPanes.BrowserPanesEvents

            ' Create new browser only once
            Dim browserPane As BrowserPane
            For Each browserPane In oBrowserPanes
                If browserPane.InternalName = "MID" & oServer.ClientId Then
                    Exit Sub
                End If
            Next

            ' Create the node resources
            If oBrowserPanes.ClientNodeResources.Count = 0 Then
                CreateNodeResources()

                ' Create the root node
                Dim oRootNodeDef As Inventor.ClientBrowserNodeDefinition = CreateRootNode(document.DisplayName)

                If oRootNodeDef IsNot Nothing Then

                    Dim oActiveBrowserPane As BrowserPane = oBrowserPanes.ActivePane

                    ' Create the BrowserPane tree
                    oBrowserPane = oBrowserPanes.AddTreeBrowserPane("Optronic Circuit Carrier", "MIDTree" & oServer.ClientId, oRootNodeDef)
                    oBrowserPane.Activate()
                    oBrowserPane.Visible = False

                    ' Keep the current browser running
                    oActiveBrowserPane.Activate()

                End If
            End If

        Catch ex As Exception
            ' not implemented yet
        End Try

    End Sub

    ' Restore the default assembly tree (OnEnvironmentChange --> back to assembly)
    '*****************************************************************************************************************************
    Public Function ActivateDefaultTree(ByRef document As Inventor.Document) As Boolean

        Try
            Dim oBrowserPanes = document.BrowserPanes
            Dim oBrowserPane As BrowserPane

            ' Make mid browser pane invisible
            For Each oBrowserPane In oBrowserPanes
                If oBrowserPane.InternalName = "MID" & oServer.ClientId Then
                    oBrowserPane.Visible = False
                End If
            Next

            ' Set default browser pane
            For Each oBrowserPane In oBrowserPanes
                If oBrowserPane.BuiltIn Then
                    If oBrowserPane.Default Then
                        oBrowserPane.Activate()
                        Return True
                    End If
                End If
            Next
        Catch ex As Exception
            Return False
        End Try

    End Function

    ' Activate tree (OnEnvironmentChange --> after MID- Environment has been activated)
    '*****************************************************************************************************************************
    Public Function ActivateMidTree(ByRef document As Inventor.Document) As Boolean

        ' Try
        Dim oBrowserPane As BrowserPane

        Dim oBrowserPanes As BrowserPanes = document.BrowserPanes

        ' Find builtIn browserpane and make it invisible
        For Each oBrowserPane In oBrowserPanes
            If oBrowserPane.BuiltIn Then
                If oBrowserPane.Default Then
                    oBrowserPane.Visible = False
                    Exit For
                End If
            End If
        Next

        ' Find MID-browserpane and make it visible
        For Each oBrowserPane In oBrowserPanes
            If oBrowserPane.InternalName = "MIDTree" & oServer.ClientId Then
                oBrowserPane.Activate()
                oBrowserPane.Visible = True
                Me.oBrowserPane = oBrowserPane
                Me.oBrowserPane.TopNode.BrowserNodeDefinition.SetLabel(document.DisplayName)
                Me.oBrowserPane.Update()
                Return True
            End If
        Next

        'Catch ex As Exception
        'Return False
        'End Try

    End Function

    ' ´Reset browser (if new document is loaded)
    '*****************************************************************************************************************************
    Public Sub ResetBrowser(ByRef oAsmDoc As Inventor.Document)

        oBrowserPanes = oAsmDoc.BrowserPanes

        ' Delete old browser
        Dim oBrowserPane As BrowserPane
        For Each oBrowserPane In oBrowserPanes
            If oBrowserPane.InternalName = "MID" & oServer.ClientId Then
                oBrowserPane.Delete()
                oBrowserPane = Nothing
                '
                Exit For
            End If
        Next

        ' Create new browser
        Me.Initialize(oAddIn.ActiveDocument)
        ' Activate new tree
        Me.ActivateMidTree(oAddIn.ActiveDocument)

    End Sub

    ' Create resources for the nodes
    '*****************************************************************************************************************************
    Private Sub CreateNodeResources()

        'On Error Resume Next
        Dim mtbAddPicture As stdole.IPictureDisp = MIDAddin.PictureConverter.ImageToPictureDisp(My.Resources.mtbAddSmall1)

        'If oBrowserPanes.ClientNodeResources.Count = 0 Then
        'MsgBox(oBrowserPanes.ClientNodeResources.Count)

        oBrowserPanes.ClientNodeResources.Add(oServer.ClientId, stdPictureId, mtbAddPicture)

        'MsgBox(oBrowserPanes.ClientNodeResources.Count)
        'End If

        'oBrowserPanes.ClientNodeResources.Add(_oServer.ClientId, mlngFaceNodePictureId, mtbAddPicture)
        'oBrowserPanes.ClientNodeResources.Add(_oServer.ClientId, mlngEdgeLoopNodePictureId, mtbAddPicture)
        'oBrowserPanes.ClientNodeResources.Add(_oServer.ClientId, mlngEdgeNodePictureId, mtbAddPicture)

    End Sub

    ' Create a new root node
    '*****************************************************************************************************************************
    Private Function CreateRootNode(ByRef browserNodeName As String) As Inventor.ClientBrowserNodeDefinition
        'On Error Resume Next
        ' Set image and create node
        'nodeId = 1
        CreateRootNode = CreateNode(browserNodeName, stdPictureId)
    End Function


    ' Find node (recursively)
    '*****************************************************************************************************************************
    Private Function FindNode(startNode As BrowserNode, _
                              nodeId As Long) As BrowserNode

        Try
            If startNode.BrowserNodeDefinition.Id = nodeId Then
                Return startNode
            End If
        Catch ex As Exception

        End Try

        Dim nd As BrowserNode = Nothing
        For Each nd In startNode.BrowserNodes
            nd = FindNode(nd, nodeId)
            If nd IsNot Nothing Then
                Return nd
            End If
        Next

        Return nd

    End Function

    ' Create a new node
    '*****************************************************************************************************************************
    Private Function CreateNode(ByRef strNodeName As String, _
                                ByRef NodePictureId As Integer) As Inventor.ClientBrowserNodeDefinition

        'On Error Resume Next
        Dim oClientNodeRes As Inventor.ClientNodeResources = oBrowserPanes.ClientNodeResources
        Dim oCl As ClientNodeResource = oClientNodeRes.ItemById(oServer.ClientId, NodePictureId)

        While (1)
            Try
                Return oBrowserPanes.CreateBrowserNodeDefinition(strNodeName, nodeId, oClientNodeRes.ItemById(oServer.ClientId, NodePictureId), "tooltip text", , BrowserNodeDisplayStateEnum.kDefaultDisplayState)
            Catch ex As Exception
                nodeId = nodeId + 1
            End Try
        End While
        Return Nothing

    End Function

    ' Delete node 
    '*****************************************************************************************************************************
    Public Sub DeleteNode(ByRef node As BrowserNode)
        'Dim oNode As BrowserNode = FindNode(oBrowserPane.TopNode, nodeId)
        node.Delete()
        node = Nothing

        oBrowserPane.Update()

    End Sub

    ' Create node
    '*****************************************************************************************************************************
    Public Function CreateNode(Optional ByVal strNodeName As String = "", _
                               Optional ByVal parentNode As BrowserNode = Nothing, _
                               Optional ByVal obj As Object = Nothing) As BrowserNode

        ' Create new node definition
        Dim oNodeDef As BrowserNodeDefinition
        If obj IsNot Nothing Then
            oNodeDef = oBrowserPanes.GetNativeBrowserNodeDefinition(obj)
        Else
            oNodeDef = CreateNode(strNodeName, 1)
        End If

        ' Create new node and insert it into the browser pane
        Dim oNode As BrowserNode
        If parentNode Is Nothing Then
            oNode = oBrowserPane.TopNode.AddChild(oNodeDef)
        Else
            oNode = parentNode.AddChild(oNodeDef)
        End If

        oBrowserPane.Update()

        Return oNode

    End Function


End Class


'Private Sub mobjBrowserPanesEvents_OnBrowserNodeActivate(ByVal browserNodeDefinition As Object, _
'                                                         ByVal context As Inventor.NameValueMap, _
'                                                         ByRef handlingCode As Inventor.HandlingCodeEnum) Handles oBrowserPanesEvents.OnBrowserNodeActivate

'    '' Circuit carrier node '++++enum here?
'    'If browserNodeDefinition.Id = 10 Then
'    '    oServer.CommandCollection.PlaceCommand.oButtonDefinition_OnExecute()
'    'End If

'    'If browserNodeDefinition.Id = 11 Then
'    '    oServer.CommandCollection.AddNetCommand.oButtonDefinition_OnExecute()
'    'End If

'End Sub


' Add Mid folder structure to the top node
'*****************************************************************************************************************************
'Public Function InsertChildNode(ByVal oAsmDoc As Inventor.Document, _
'                                ByVal oObj As Object, _
'                                ByVal strParentNode As String, _
'                                Optional ByVal strNodeName As String = Nothing) As Boolean

'    ' Search parent node
'    Dim oParentNode As BrowserNode = FindNode(oBrowserPane.TopNode, strParentNode)

'    ' Get node from object and insert it below the parent node
'    ' If parent node exists, add a new child node
'    If oParentNode IsNot Nothing Then

'        Dim oNodeDef As BrowserNodeDefinition

'        If oObj IsNot Nothing Then

'            'Dim oNode As BrowserNode = oBrowserPane.GetBrowserNodeFromObject(oObj)
'            oNodeDef = oBrowserPanes.GetNativeBrowserNodeDefinition(oObj)

'            'Dim oNode As BrowserNode = oBrowserPane.TopNode.AllReferencedNodes(oNodeDef).Item(1)
'        Else
'            oNodeDef = CreateNode(strNodeName, 1)
'        End If
'        Dim oNode As BrowserNode = oParentNode.AddChild(oNodeDef)
'        'oNode.Expanded = True
'        oBrowserPane.Update()
'    End If

'End Function


'get the brep information and add it to the tree
'Public Sub AddNode(ByVal oDoc As Inventor.Document, _
'                   ByVal strNodeName As String,
'                   NodePictureId As Integer)

'    'On Error Resume Next

'    'oBrowserPanes = oAsmDoc.BrowserPanes
'    '_BrowserPane = oBrowserPanes.ActivePane

'    ' = oBrowserPanes.CreateBrowserNodeDefinition()



'    'Dim oClientNodeRes As Inventor.ClientNodeResources = oBrowserPanes.ClientNodeResources
'    'Dim mtbAddPicture As stdole.IPictureDisp = MIDProjectVB3.PictureConverter.ImageToPictureDisp(My.Resources.mtbAddSmall1)
'    'oClientNodeRes.Add(_oServer.ClientId, 5, mtbAddPicture)

'    If oDoc Is oDocument Then
'        'Dim oTopNode As BrowserNode = oBrowserPane.TopNode
'        Dim oNodeDef As Inventor.ClientBrowserNodeDefinition = CreateNode(strNodeName, NodePictureId)
'        If (oBrowserPane IsNot Nothing) Then
'            Dim oNode As BrowserNode = oBrowserPane.TopNode.AddChild(oNodeDef)
'            'oNode.Expanded = True
'        End If
'        oBrowserPane.Update()
'    End If





'    'oBrowserPanes.CreateBrowserNodeDefinition(strNodeName, 3, oClientNodeRes.ItemById(_oServer.ClientId, 5))
'    ' CreateNode("Mid Circuit Carrier", nodeId)

'    'Dim bfEnum As BrowserFoldersEnumerator = _BrowserPane.TopNode.BrowserFolders
'    'Dim oFolder As BrowserFolder = bfEnum.Item("circuit carrier")





'    'Dim oNode1 As BrowserNode = _BrowserPane.GetBrowserNodeFromObject(oObj)
'    'Dim oOccurrenceNodes As ObjectCollection
'    'oOccurrenceNodes = _oAddIn.TransientObjects.CreateObjectCollection

'    'oOccurrenceNodes.Add(oNode1)
'    'oFolder.AllowAddRemove = True
'    'MsgBox(oFolder.Name)
'    'If oNode IsNot Nothing Then
'    '    oFolder.Add(oNode1, , False)
'    'End If


'End Sub

'Private Sub delete_OnBrowserNodeDeleteEntry(BrowserNodeDefinition As Object, _
'                                            BeforeOrAfter As EventTimingEnum, _
'                                            Context As NameValueMap, _
'                                            ByRef HandlingCode As HandlingCodeEnum) Handles oBrowserPanesEvents.OnBrowserNodeDeleteEntry


'    MsgBox("ndoe deleted")

'End Sub

'Private Sub BrowserPanes_OnBrowserNodeLabelEdit(browserNodeDefinition As Object, _
'                                                label As String, _
'                                                beforeOrAfter As EventTimingEnum, _
'                                                context As NameValueMap, _
'                                                ByRef handlingCode As HandlingCodeEnum) Handles oBrowserPanesEvents.OnBrowserNodeLabelEdit



'    'handlingCode = HandlingCodeEnum.kEventHandled


'    If beforeOrAfter = EventTimingEnum.kBefore Then
'        strLabel = label
'        Debug.WriteLine(label)
'    End If

'    If beforeOrAfter = EventTimingEnum.kAfter Then
'        ' browserNodeDefinition.SetLabel(label)
'        'Debug.WriteLine(label)
'    End If

'End Sub


'Dim oTopNode As BrowserNode = oBrowserPane.TopNode



' Dim oCompNode As BrowserNode = _BrowserPane.GetBrowserNodeFromObject(oCompOcc)
'oCCNodes.Add(oCompNode)


'oFaceFolder.Add(oCompNode)

'Dim i As Integer = 0
'For Each oFace As FaceProxy In oCompOcc.SurfaceBodies.Item(1).Faces
'    Dim oFaceNodeDef As Inventor.ClientBrowserNodeDefinition = oBrowserPanes.CreateBrowserNodeDefinition("balbalb", i, oClientNodeRes.ItemById(_oServer.ClientId, 5))
'    Dim oFaceNode As BrowserNode = oCompNode.InsertChild(oFaceNodeDef, oCompNode, False)
'    CCNodes.Add(_BrowserPane.GetBrowserNodeFromObject(oFace.NativeObject))
'    i += 1
'Next

'Private Sub DeleteNodeResources()

'    For i As Integer = 1 To oBrowserPanes.ClientNodeResources.Count
'        oBrowserPanes.ClientNodeResources.Item(i).Delete()
'    Next

'End Sub


'Dim oNode As Inventor.BrowserNode = _BrowserPane.TopNode.AddChild(oNodeDef)
'oCCNodes.Add(oNode)

'Dim oFaceFolder As 

'Public Function RemoveNode(ByVal strNodeName As String) As Boolean

'    Dim oNode As BrowserNode = FindNode(oBrowserPane.TopNode, strNodeName)

'    If oNode IsNot Nothing Then
'        oNode.Delete()
'        oBrowserPane.Update()
'        Return True
'    End If

'    Return False
'End Function
'BrowserFolder = _BrowserPane.AddBrowserFolder("circuit carrier", oCCNodes)