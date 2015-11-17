Public Class UserInputEvents

    Private oAddin As Inventor.Application

    Private WithEvents oUserInputEvents As Inventor.UserInputEvents

    Private Structure EnvCommand
        Dim EnvInternalName As String
        Dim CmdInternalNames() As String
    End Structure

    Private oEnvCommands() As EnvCommand


    Public Sub New(ByVal oAddIn As Inventor.Application)
        MyBase.New()

        Me.oAddin = oAddIn



        'initialize user interface events
        oUserInputEvents = oAddIn.CommandManager.UserInputEvents

    End Sub


    Public Sub AddCmdToEnv(ByRef oControlDef As ControlDefinition, _
                           ByRef strEnvInternalName As String)

        ' Get current number of Commands
        Dim EnvCommandSize As Integer = UBound(oEnvCommands)

        'Dim EnvCt As Integer
        'Dim bCommandExists As Boolean
        'Dim CommandCt As Integer
        'Dim i As Integer
        Dim bCommandExists As Boolean
        Dim j As Integer

        If EnvCommandSize = 0 Then

            ' Set environment's internal name 
            ReDim oEnvCommands(1)
            oEnvCommands(1).EnvInternalName = strEnvInternalName

            ReDim oEnvCommands(1).CmdInternalNames(1)
            oEnvCommands(1).CmdInternalNames(1) = oControlDef.InternalName

        Else

            For i As Integer = LBound(oEnvCommands) To UBound(oEnvCommands)

                If oEnvCommands(i).EnvInternalName = strEnvInternalName Then

                    For j = LBound(oEnvCommands(i).CmdInternalNames) To UBound(oEnvCommands(i).CmdInternalNames)
                        If oEnvCommands(i).CmdInternalNames(j) = oControlDef.InternalName Then
                            bCommandExists = True
                            Exit For
                        End If
                    Next

                    If bCommandExists = False Then
                        ' Add one element to the array
                        ReDim Preserve oEnvCommands(i).CmdInternalNames(UBound(oEnvCommands(i).CmdInternalNames) + 1)
                        ' Add command name to the last array element
                        oEnvCommands(i).CmdInternalNames(UBound(oEnvCommands(i).CmdInternalNames)) = oControlDef.InternalName
                    End If

                End If
            Next
        End If

    End Sub


    Private Sub oUserInputEvents_OnDrag(DragState As DragStateEnum, _
                                    ShiftKeys As ShiftStateEnum, _
                                    ModelPosition As Point, _
                                    ViewPosition As Point2d, _
                                    View As Inventor.View, AdditionalInfo As NameValueMap, _
                                    ByRef HandlingCode As HandlingCodeEnum) Handles oUserInputEvents.OnDrag
        Debug.WriteLine("itwokrs")

    End Sub

    Private Sub oUserInputEvents_OnPreSelect(ByRef PreSelectEntity As Object, _
                                              ByRef DoHighlight As Boolean, _
                                             ByRef MorePreSelectEntities As ObjectCollection, _
                                              SelectionDevice As SelectionDeviceEnum, _
                                              ModelPosition As Point, _
                                              ViewPosition As Point2d, _
                                              View As View) Handles oUserInputEvents.OnPreSelect

        DoHighlight = False
    End Sub


End Class
