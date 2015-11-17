Option Strict Off
Option Explicit On

<System.Runtime.InteropServices.ProgId("clsCustomBrowserPane_NET.clsCustomBrowserPane")> Public Class MidBrowser

    Private _oAddIn As Inventor.Application
    Private _strClientId As String

    'constants
    Private Const mlngBodyNodePictureId As Short = 1
    'Private Const mlngFaceNodePictureId As Short = 2
    'Private Const mlngEdgeLoopNodePictureId As Short = 3
    'Private Const mlngEdgeNodePictureId As Short = 4

    'data members
    Private nodeId As Integer

    Private oBrowserPanes As Inventor.BrowserPanes
    Private oBrowserPane As Inventor.BrowserPane

    Private strLabel As String

    Private oDocument As Inventor.Document


    Private WithEvents oBrowserPanesEvents As Inventor.BrowserPanesEvents

    Public Sub New(ByVal oAddIn As Inventor.Application, ByVal strClientID As String)

        MyBase.New()

        _oAddIn = oAddIn
        _strClientId = strClientID

    End Sub

    ' Add new browser for the document
    '*****************************************************************************************************************************
    Public Sub InitBrowser(ByRef oDocument As Inventor.Document)
        ' ###Fehlerbehandlung

        ' Set document the browser is for (the active document)
        Me.oDocument = oDocument
        ' Retrieve the browser panes of the document
        oBrowserPanes = oDocument.BrowserPanes
        ' Retrieve the browser panes events
        oBrowserPanesEvents = oBrowserPanes.BrowserPanesEvents

        ' Activate browser if it already exists (switching between documents)
        Dim oBrowserPane As BrowserPane
        For Each oBrowserPane In oBrowserPanes
            If oBrowserPane.InternalName = "MID" & _strClientId Then
                oBrowserPane.Activate()
                Exit Sub
            End If
        Next

        ' First time:
        '*************************************************************
        ' Create the node resources
        CreateNodeResources()

        ' Create the root node
        Dim oRootNodeDef As Inventor.ClientBrowserNodeDefinition = CreateRootNode((oDocument.DisplayName))

        Dim oActiveBrowserPane As Inventor.BrowserPane

        If Not oRootNodeDef Is Nothing Then

            oActiveBrowserPane = oBrowserPanes.ActivePane

            ' Create the BrowserPane tree
            oBrowserPane = oBrowserPanes.AddTreeBrowserPane("MID", "MID" & _strClientId, oRootNodeDef)

            ' Keep the current browser running
            oActiveBrowserPane.Activate()



        End If

    End Sub

    ' Activate tree
    '*****************************************************************************************************************************
    Public Function ActivateMidTree(ByRef oAsmDoc As Inventor.Document) As Boolean

        Try
            Dim _BrowserPane As BrowserPane

            'Dim oBrowserPane As Inventor.BrowserPane

            For Each _BrowserPane In oBrowserPanes
                If _BrowserPane.BuiltIn Then
                    If _BrowserPane.Default Then
                        _BrowserPane.Visible = False
                        Exit For
                    End If
                End If
            Next

            For Each _BrowserPane In oBrowserPanes
                If _BrowserPane.InternalName = "MID" & _strClientId Then
                    _BrowserPane.Activate()
                    oBrowserPane = _BrowserPane
                    Return True
                End If
            Next
        Catch ex As Exception
            Return False
        End Try

    End Function


    ' Restore the default assembly tree
    '*****************************************************************************************************************************
    Public Function ActivateDefaultTree(ByRef oAsmDoc As Inventor.Document) As Boolean

        Try

            ' Make mid browser pane invisible
            oBrowserPane.Visible = False

            ' Set default browser pane
            For Each _BrowserPane In oBrowserPanes

                If _BrowserPane.BuiltIn Then
                    If _BrowserPane.Default Then
                        _BrowserPane.Activate()
                        Return True
                    End If
                End If
            Next
        Catch ex As Exception
            Return False
        End Try

    End Function

    ' Create resources for the nodes
    '*****************************************************************************************************************************
    Private Sub CreateNodeResources()

        'On Error Resume Next
        Dim mtbAddPicture As stdole.IPictureDisp = MIDProjectVB3.PictureConverter.ImageToPictureDisp(My.Resources.mtbAddSmall1)
        oBrowserPanes.ClientNodeResources.Add(_strClientId, mlngBodyNodePictureId, mtbAddPicture)


        'oBrowserPanes.ClientNodeResources.Add(_strClientId, mlngFaceNodePictureId, mtbAddPicture)
        'oBrowserPanes.ClientNodeResources.Add(_strClientId, mlngEdgeLoopNodePictureId, mtbAddPicture)
        'oBrowserPanes.ClientNodeResources.Add(_strClientId, mlngEdgeNodePictureId, mtbAddPicture)

    End Sub


    ' Create a new root node
    '*****************************************************************************************************************************
    Private Function CreateRootNode(ByRef strBrowserNodeName As String) As Inventor.ClientBrowserNodeDefinition
        'On Error Resume Next
        ' Set image and create node
        nodeId = 1
        CreateRootNode = CreateNode(strBrowserNodeName, mlngBodyNodePictureId)
    End Function


    ' Find node (recursively)
    '*****************************************************************************************************************************
    Private Function FindNode(startNode As BrowserNode, strNodeName As String) As BrowserNode

        If startNode.BrowserNodeDefinition.Label.Equals(strNodeName) Then
            Return startNode
        End If

        Dim nd As BrowserNode = Nothing
        For Each nd In startNode.BrowserNodes
            nd = FindNode(nd, strNodeName)
            If nd IsNot Nothing Then
                Return nd
            End If
        Next

        Return nd

    End Function

    Public Function RemoveNode(ByVal strNodeName As String) As Boolean

        Dim oNode As BrowserNode = FindNode(oBrowserPane.TopNode, strNodeName)

        If oNode IsNot Nothing Then
            oNode.Delete()
            oBrowserPane.Update()
            Return True
        End If

        Return False
    End Function

    ' Add Mid folder structure to the top node
    '*****************************************************************************************************************************
    Public Function InsertChildNode(ByVal oAsmDoc As Inventor.Document, _
                                    ByVal oObj As Object, _
                                    ByVal strParentNode As String, _
                                    Optional ByVal strNodeName As String = Nothing) As Boolean

        ' Search parent node
        Dim oParentNode As BrowserNode = FindNode(oBrowserPane.TopNode, strParentNode)

        ' Get node from object and insert it below the parent node
        ' If parent node exists, add a new child node
        If oParentNode IsNot Nothing Then

            Dim oNodeDef As BrowserNodeDefinition

            If oObj IsNot Nothing Then

                'Dim oNode As BrowserNode = oBrowserPane.GetBrowserNodeFromObject(oObj)
                oNodeDef = oBrowserPanes.GetNativeBrowserNodeDefinition(oObj)

                'Dim oNode As BrowserNode = oBrowserPane.TopNode.AllReferencedNodes(oNodeDef).Item(1)
            Else
                oNodeDef = CreateNode(strNodeName, 1)
            End If
            Dim oNode As BrowserNode = oParentNode.AddChild(oNodeDef)
            'oNode.Expanded = True
            oBrowserPane.Update()
        End If

    End Function

    ' Create a new node
    '*****************************************************************************************************************************
    Private Function CreateNode(ByRef strNodeName As String, _
                                ByRef NodePictureId As Integer) As Inventor.ClientBrowserNodeDefinition

        'On Error Resume Next

        Dim oClientNodeRes As Inventor.ClientNodeResources = oBrowserPanes.ClientNodeResources
        CreateNode = oBrowserPanes.CreateBrowserNodeDefinition(strNodeName, nodeId, oClientNodeRes.ItemById(_strClientId, NodePictureId))
        nodeId = nodeId + 1

    End Function

    'get the brep information and add it to the tree
    Public Sub AddNode(ByVal oDoc As Inventor.Document, _
                       ByVal strNodeName As String,
                       NodePictureId As Integer)

        'On Error Resume Next

        'oBrowserPanes = oAsmDoc.BrowserPanes
        '_BrowserPane = oBrowserPanes.ActivePane

        ' = oBrowserPanes.CreateBrowserNodeDefinition()



        'Dim oClientNodeRes As Inventor.ClientNodeResources = oBrowserPanes.ClientNodeResources
        'Dim mtbAddPicture As stdole.IPictureDisp = MIDProjectVB3.PictureConverter.ImageToPictureDisp(My.Resources.mtbAddSmall1)
        'oClientNodeRes.Add(_strClientId, 5, mtbAddPicture)

        If oDoc Is oDocument Then
            'Dim oTopNode As BrowserNode = oBrowserPane.TopNode
            Dim oNodeDef As Inventor.ClientBrowserNodeDefinition = CreateNode(strNodeName, NodePictureId)
            If (oBrowserPane IsNot Nothing) Then
                Dim oNode As BrowserNode = oBrowserPane.TopNode.AddChild(oNodeDef)
                'oNode.Expanded = True
            End If
            oBrowserPane.Update()
        End If





        'oBrowserPanes.CreateBrowserNodeDefinition(strNodeName, 3, oClientNodeRes.ItemById(_strClientId, 5))
        ' CreateNode("Mid Circuit Carrier", nodeId)

        'Dim bfEnum As BrowserFoldersEnumerator = _BrowserPane.TopNode.BrowserFolders
        'Dim oFolder As BrowserFolder = bfEnum.Item("circuit carrier")





        'Dim oNode1 As BrowserNode = _BrowserPane.GetBrowserNodeFromObject(oObj)
        'Dim oOccurrenceNodes As ObjectCollection
        'oOccurrenceNodes = _oAddIn.TransientObjects.CreateObjectCollection

        'oOccurrenceNodes.Add(oNode1)
        'oFolder.AllowAddRemove = True
        'MsgBox(oFolder.Name)
        'If oNode IsNot Nothing Then
        '    oFolder.Add(oNode1, , False)
        'End If


    End Sub

    Private Sub BrowserPanes_OnBrowserNodeLabelEdit(browserNodeDefinition As Object, _
                                                    label As String, _
                                                    beforeOrAfter As EventTimingEnum, _
                                                    context As NameValueMap, _
                                                    ByRef handlingCode As HandlingCodeEnum) Handles oBrowserPanesEvents.OnBrowserNodeLabelEdit



        'handlingCode = HandlingCodeEnum.kEventHandled


        If beforeOrAfter = EventTimingEnum.kBefore Then
            strLabel = label
            Debug.WriteLine(label)
        End If

        If beforeOrAfter = EventTimingEnum.kAfter Then
            label = strLabel
            'Debug.WriteLine(label)
        End If

    End Sub


    Private Function GetBrepEntity(ByRef oPartDoc As Inventor.PartDocument, ByRef lngBrowserNodeId As Integer) As Object

        On Error Resume Next

        GetBrepEntity = Nothing

        Dim objPartCompDef As Inventor.PartComponentDefinition
        objPartCompDef = oPartDoc.ComponentDefinition

        Dim objSurfaceBody As Inventor.SurfaceBody
        Dim lngBrepEntityId As Integer
        Dim objFaces As Inventor.Faces
        Dim objFace As Inventor.Face
        Dim objEdgeLoops As Inventor.EdgeLoops
        Dim objEdgeLoop As Inventor.EdgeLoop
        Dim objEdges As Inventor.Edges
        Dim objEdge As Inventor.Edge
        If Not objPartCompDef Is Nothing Then

            objSurfaceBody = oPartDoc.ComponentDefinition.SurfaceBodies(1)

            If Not objSurfaceBody Is Nothing Then

                lngBrepEntityId = 1

                If lngBrepEntityId = lngBrowserNodeId Then
                    GetBrepEntity = objSurfaceBody
                    Exit Function
                End If

                'get the Faces collection from the SurfaceBody
                objFaces = objSurfaceBody.Faces

                'iterate through the faces in the current body
                For Each objFace In objFaces

                    lngBrepEntityId = lngBrepEntityId + 1

                    If lngBrepEntityId = lngBrowserNodeId Then
                        GetBrepEntity = objFace
                        Exit Function
                    End If

                    'get the EdgeLoops collection from the Face
                    objEdgeLoops = objFace.EdgeLoops

                    'iterate through the loops of the current face
                    For Each objEdgeLoop In objEdgeLoops

                        lngBrepEntityId = lngBrepEntityId + 1

                        If lngBrepEntityId = lngBrowserNodeId Then
                            GetBrepEntity = objEdgeLoop
                            Exit Function
                        End If

                        'get the Edges collection from the EdgeLoop
                        objEdges = objEdgeLoop.Edges

                        'iterate through the edges of the current loop
                        For Each objEdge In objEdges

                            lngBrepEntityId = lngBrepEntityId + 1

                            If lngBrepEntityId = lngBrowserNodeId Then
                                GetBrepEntity = objEdge
                                Exit Function
                            End If
                        Next objEdge
                    Next objEdgeLoop
                Next objFace
            End If
        End If
    End Function

    Private Sub oBrowserPanesEvents_OnBrowserNodeActivate(ByVal BrowserNodeDefinition As Object, ByVal Context As Inventor.NameValueMap, ByRef HandlingCode As Inventor.HandlingCodeEnum) Handles oBrowserPanesEvents.OnBrowserNodeActivate

        On Error Resume Next

        Dim oPartDoc As Inventor.PartDocument
        Dim objBrepEntity As Object
        ' Dim frmEntityInfo As New frmEntityInformation
        If BrowserNodeDefinition.Type = Inventor.ObjectTypeEnum.kClientBrowserNodeDefinitionObject Then

            oPartDoc = _oAddIn.ActiveDocument

            objBrepEntity = GetBrepEntity(oPartDoc, BrowserNodeDefinition.Id)

            If Not objBrepEntity Is Nothing Then

                If objBrepEntity.Type = Inventor.ObjectTypeEnum.kSurfaceBodyObject Then
                    ' frmEntityInfo.DisplaySurfaceBodyInformation(objBrepEntity)
                End If

                If objBrepEntity.Type = Inventor.ObjectTypeEnum.kFaceObject Then
                    ' frmEntityInfo.DisplayFaceInformation(objBrepEntity)
                End If

                If objBrepEntity.Type = Inventor.ObjectTypeEnum.kEdgeObject Then
                    ' frmEntityInfo.DisplayEdgeInformation(objBrepEntity)
                End If

            End If

        End If

    End Sub

    Private Sub oBrowserPanesEvents_OnBrowserNodeGetDisplayObjects(ByVal BrowserNodeDefinition As Object, ByRef Objects As Inventor.ObjectCollection, ByVal Context As Inventor.NameValueMap, ByRef HandlingCode As Inventor.HandlingCodeEnum) Handles oBrowserPanesEvents.OnBrowserNodeGetDisplayObjects

        On Error Resume Next

        Dim oPartDoc As Inventor.PartDocument
        Dim objBrepEntity As Object
        If BrowserNodeDefinition.Type = Inventor.ObjectTypeEnum.kClientBrowserNodeDefinitionObject Then

            oPartDoc = _oAddIn.ActiveDocument

            objBrepEntity = GetBrepEntity(oPartDoc, BrowserNodeDefinition.Id)

            If Not objBrepEntity Is Nothing And (objBrepEntity.Type = Inventor.ObjectTypeEnum.kFaceObject Or objBrepEntity.Type = Inventor.ObjectTypeEnum.kEdgeObject) Then

                Objects = _oAddIn.TransientObjects.CreateObjectCollection
                Objects.Add(objBrepEntity)

                HandlingCode = Inventor.HandlingCodeEnum.kEventHandled
            End If

        End If

    End Sub

    Protected Overrides Sub Finalize()

        oBrowserPanes = Nothing

        oBrowserPanesEvents = Nothing

        MyBase.Finalize()

    End Sub
End Class




'Dim oTopNode As BrowserNode = oBrowserPane.TopNode



' Dim oCompNode As BrowserNode = _BrowserPane.GetBrowserNodeFromObject(oCompOcc)
'oCCNodes.Add(oCompNode)


'oFaceFolder.Add(oCompNode)

'Dim i As Integer = 0
'For Each oFace As FaceProxy In oCompOcc.SurfaceBodies.Item(1).Faces
'    Dim oFaceNodeDef As Inventor.ClientBrowserNodeDefinition = oBrowserPanes.CreateBrowserNodeDefinition("balbalb", i, oClientNodeRes.ItemById(_strClientId, 5))
'    Dim oFaceNode As BrowserNode = oCompNode.InsertChild(oFaceNodeDef, oCompNode, False)
'    CCNodes.Add(_BrowserPane.GetBrowserNodeFromObject(oFace.NativeObject))
'    i += 1
'Next




'Dim oNode As Inventor.BrowserNode = _BrowserPane.TopNode.AddChild(oNodeDef)
'oCCNodes.Add(oNode)

'Dim oFaceFolder As BrowserFolder = _BrowserPane.AddBrowserFolder("circuit carrier", oCCNodes)