Public Class Triad

    Private oInteractionEvents As InteractionEvents

    Private bodyLength As Double = 0.4
    Private tipLength As Double = 0.5
    Private bodyRadius As Double = 0.02
    Private tipRadius As Double = 0.04

    Private oONode As GraphicsNode
    Private oXNode As GraphicsNode
    Private oYNode As GraphicsNode
    Private oZNode As GraphicsNode

    Private oAddIn As Application

    Private oRenderStyle As RenderStyle

    ' Constructor
    '***********************************************************************************************************************
    Public Sub New(addIn As Application, _
                   interactionEvents As InteractionEvents)

        oInteractionEvents = interactionEvents
        oAddIn = addIn

        Create()

    End Sub

    ' Delete triad client graphics
    '***********************************************************************************************************************
    Public Sub Delete()

        oXNode.Delete()
        oYNode.Delete()
        oZNode.Delete()
        oONode.Delete()
        oONode = Nothing
        oXNode = Nothing
        oYNode = Nothing
        oZNode = Nothing
        oAddIn.ActiveView.Update()

    End Sub

    ' Create triad client graphics
    '***********************************************************************************************************************
    Private Sub Create()
        'Dim oAsmDoc As AssemblyDocument = oAddIn.ActiveDocument()
        'oRenderStyle = oAsmDoc.RenderStyles.Add("NewStyle")

        ''oRenderStyle.SetEmissiveColor(151, 193, 57)
        'oRenderStyle.SetAmbientColor(95, 95, 95)
        'oRenderStyle.SetDiffuseColor(95, 95, 95)
        'oRenderStyle.SetSpecularColor(225, 225, 225)

        'Dim oRenderStyle1 = oAsmDoc.RenderStyles.Add("NewStyle1")

        'oRenderStyle.SetAmbientColor(143, 212, 34)
        Dim oAsmDoc As Document = oAddIn.ActiveDocument()
        Dim oRedStyle As RenderStyle = oAsmDoc.RenderStyles.Item("Red")
        Dim oGreenStyle As RenderStyle = oAsmDoc.RenderStyles.Item("Dark Green")
        Dim oBlueStyle As RenderStyle = oAsmDoc.RenderStyles.Item("Cyan")
        Dim oBlackStyle As RenderStyle = oAsmDoc.RenderStyles.Item("Black")

        ' Create new client graphics
        Dim oInteractionGraphics As InteractionGraphics = oInteractionEvents.InteractionGraphics
        Dim oClientGraphics As ClientGraphics = oInteractionGraphics.PreviewClientGraphics

        oONode = oClientGraphics.AddNode(1000)
        oXNode = oClientGraphics.AddNode(2000)
        oYNode = oClientGraphics.AddNode(3000)
        oZNode = oClientGraphics.AddNode(4000)
        oONode.Visible = False
        oXNode.Visible = False
        oYNode.Visible = False
        oZNode.Visible = False

        ' Create surfacebodies
        Dim oTG As TransientGeometry = oAddIn.TransientGeometry
        Dim oTBRep As TransientBRep = oAddIn.TransientBRep
        Dim oOriginPoint As Point = oTG.CreatePoint(0, 0, 0)
        Dim oXPoint As Point = oTG.CreatePoint(bodyLength, 0, 0)
        Dim oYPoint As Point = oTG.CreatePoint(0, bodyLength, 0)
        Dim oZPoint As Point = oTG.CreatePoint(0, 0, bodyLength)

        Dim oXPoint1 As Point = oTG.CreatePoint(tipLength, 0, 0)
        Dim oYPoint1 As Point = oTG.CreatePoint(0, tipLength, 0)
        Dim oZPoint1 As Point = oTG.CreatePoint(0, 0, tipLength)


        Dim oSphereBody As SurfaceBody = oTBRep.CreateSolidSphere(oOriginPoint, tipRadius)

        Dim oXArrowBody As SurfaceBody = oTBRep.CreateSolidCylinderCone(oOriginPoint, oXPoint, bodyRadius, bodyRadius, bodyRadius)
        Dim oXArrowTip As SurfaceBody = oTBRep.CreateSolidCylinderCone(oXPoint, oXPoint1, tipRadius, tipRadius, 0)

        Dim oYArrowBody As SurfaceBody = oTBRep.CreateSolidCylinderCone(oOriginPoint, oYPoint, bodyRadius, bodyRadius, bodyRadius)
        Dim oYArrowTip As SurfaceBody = oTBRep.CreateSolidCylinderCone(oYPoint, oYPoint1, tipRadius, tipRadius, 0)

        Dim oZArrowBody As SurfaceBody = oTBRep.CreateSolidCylinderCone(oOriginPoint, oZPoint, bodyRadius, bodyRadius, bodyRadius)
        Dim oZArrowTip As SurfaceBody = oTBRep.CreateSolidCylinderCone(oZPoint, oZPoint1, tipRadius, tipRadius, 0)

        ' Do boolean operations to join surfacebodies
        oTBRep.DoBoolean(oXArrowBody, oXArrowTip, BooleanTypeEnum.kBooleanTypeUnion)
        oTBRep.DoBoolean(oYArrowBody, oYArrowTip, BooleanTypeEnum.kBooleanTypeUnion)
        oTBRep.DoBoolean(oZArrowBody, oZArrowTip, BooleanTypeEnum.kBooleanTypeUnion)

        ' Add surface graphics to the nodes
        Dim oXGraphics As SurfaceGraphics = oXNode.AddSurfaceGraphics(oXArrowBody)
        Dim oYGraphics As SurfaceGraphics = oYNode.AddSurfaceGraphics(oYArrowBody)
        Dim oZGraphics As SurfaceGraphics = oZNode.AddSurfaceGraphics(oZArrowBody)
        Dim oSGraphics As SurfaceGraphics = oONode.AddSurfaceGraphics(oSphereBody)

        oXGraphics.DepthPriority = 5
        oYGraphics.DepthPriority = 5
        oZGraphics.DepthPriority = 5
        oSGraphics.DepthPriority = 5

        oXGraphics.BurnThrough = True
        oYGraphics.BurnThrough = True
        oZGraphics.BurnThrough = True
        oSGraphics.BurnThrough = True


        'oZArrowBody.SetRenderStyle(

        ' Join the arrows to generate a triad

        'oTBRep.DoBoolean(oXArrowTip, oXArrowBody, BooleanTypeEnum.kBooleanTypeUnion)
        ' oTBRep.DoBoolean(XBody, , BooleanTypeEnum.kBooleanTypeUnion)
        'Try
        'Dim oAsmDoc As AssemblyDocument = oAddIn.ActiveDocument()
        ' Dim oStyle As RenderStyle = oAsmDoc.RenderStyles.Item("Smooth - Red")
        'oXArrowTip.NativeObject.SetRenderStyle(StyleSourceTypeEnum.kOverrideRenderStyle, oStyle)
        'Catch ex As Exception
        '    System.Windows.Forms.MessageBox.Show("Could not find 'Smooth - Dark Forest Green'-asset in the asset library")
        'End Try

        Try

            'Dim oStyle As RenderStyle = oAsmDoc.RenderStyles.Item("oRenderStyle")
            oONode.RenderStyle = oBlackStyle
            oXNode.RenderStyle = oRedStyle
            oYNode.RenderStyle = oGreenStyle
            oZNode.RenderStyle = oBlueStyle
        Catch ex As Exception
            System.Windows.Forms.MessageBox.Show("Could not find 'Smooth - Dark Forest Green'-asset in the asset library")
        End Try

    End Sub

    Public WriteOnly Property Visible As Boolean
        Set(value As Boolean)
            oXNode.Visible = value
            oYNode.Visible = value
            oZNode.Visible = value
            oONode.Visible = value
            oAddIn.ActiveView.Update()
        End Set
    End Property

    Public Property Transformation As Matrix
        Get
            Return oONode.Transformation
        End Get
        Set(value As Matrix)
            oXNode.Transformation = value.Copy
            oYNode.Transformation = value.Copy
            oZNode.Transformation = value.Copy
            oONode.Transformation = value.Copy
        End Set
    End Property
    
End Class
