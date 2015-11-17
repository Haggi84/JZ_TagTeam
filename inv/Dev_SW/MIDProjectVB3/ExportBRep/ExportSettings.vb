Imports System.Windows.Forms
Imports System.Drawing

Public Class ExportSettings

    Private MIDAddin As Inventor.Application

    Private WithEvents oCBUnitOfAngles As ComboBox
    Private WithEvents oCBUnitOfLength As ComboBox

    Private WithEvents oLabelUOF As Label
    Private WithEvents oLabelUOA As Label

    Private WithEvents oCBAnglePrec As ComboBox
    Private WithEvents oCBLengthPrec As ComboBox

    Private WithEvents oLabelAnglePrec As Label
    Private WithEvents oLabelLengthPrec As Label

    Private WithEvents oExportButton As Button
    Private WithEvents oCancelButton As Button

    Private WithEvents oCBDefault As CheckBox

    Private exportData(3) As String

    Private oWindow As DockableWindow

    ' Constructor
    Public Sub New(ByRef MIDAddin As Inventor.Application)

        ' This call is required by the designer.
        InitializeComponent()

        Me.MIDAddin = MIDAddin

    End Sub

    Public Sub initExport()

        ' Create new dockable window if there is none
        If oWindow Is Nothing Then
            ' Windows form 
            Me.exportData = exportData
            Me.Visible = True

            ' Inventor findow
            Dim oUserInterfaceMgr As UserInterfaceManager = MIDAddin.UserInterfaceManager

            oWindow = oUserInterfaceMgr.DockableWindows.Add("ClientID", "ExportWindowInternal", "Geometry export settings") ' CHANGECLIENTID HERE LATER
            Dim hwnd As Long = Me.Handle
            oWindow.AddChild(hwnd)

            oWindow.ShowTitleBar = True
            oWindow.Height = 300
            oWindow.Width = 400
            oWindow.DockingState() = DockingStateEnum.kDockBottom
            oWindow.SetMinimumSize(300, 400)

            ' Make the window visible
            oWindow.Visible() = True

        Else
            oWindow.Visible() = True
        End If



    End Sub

    Private Sub ExportSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.Visible = False
        Me.TopMost = True

        oCBUnitOfAngles = New ComboBox()
        'Me.ComboBox1 = New ComboBox
        oCBUnitOfAngles.Location = New Point(250, 50)
        oCBUnitOfAngles.Name = "oCBUnitOfAngles"
        oCBUnitOfAngles.Size = New Size(100, 21)
        oCBUnitOfAngles.TabIndex = 0
        oCBUnitOfAngles.DropDownStyle = ComboBoxStyle.DropDownList
        oCBUnitOfAngles.Text = "grad"
        Dim angleUnits() As String = {"grad", "rad"}
        oCBUnitOfAngles.Items.AddRange(angleUnits)
        Controls.Add(oCBUnitOfAngles)

        oCBUnitOfLength = New ComboBox()
        'Me.ComboBox1 = New ComboBox
        oCBUnitOfLength.Location = New Point(250, 150)
        oCBUnitOfLength.Name = "oCBUnitOfLength"
        oCBUnitOfLength.Size = New Size(100, 21)
        oCBUnitOfLength.TabIndex = 0
        oCBUnitOfLength.DropDownStyle = ComboBoxStyle.DropDownList
        oCBUnitOfLength.Text = "millimeter"
        Dim lengthUnits() As String = {"millimeter", "centimeter", "meter", "inch"}
        oCBUnitOfLength.Items.AddRange(lengthUnits)
        Controls.Add(oCBUnitOfLength)

        oLabelUOA = New Label
        oLabelUOA.Text = "Unit of Angles"
        oLabelUOA.Name = "oLabelUOA"
        oLabelUOA.Location = New System.Drawing.Point(50, 50)
        oLabelUOA.Visible = True
        oLabelUOA.Enabled = True
        Controls.Add(oLabelUOA)

        oLabelUOF = New Label
        oLabelUOF.Text = "Unit of Length"
        oLabelUOF.Name = "oLabelUOA"
        oLabelUOF.Location = New System.Drawing.Point(50, 150)
        oLabelUOF.Visible = True
        oLabelUOF.Enabled = True
        Controls.Add(oLabelUOF)

        oLabelAnglePrec = New Label
        oLabelAnglePrec.Text = "Maximum number of decimal places"
        oLabelAnglePrec.Name = "oLabelAnglePrec"
        oLabelAnglePrec.Location = New Point(60, 80)
        oLabelAnglePrec.Size = New Size(100, 40)
        oLabelAnglePrec.Visible = True
        oLabelAnglePrec.Enabled = True
        Controls.Add(oLabelAnglePrec)

        oLabelLengthPrec = New Label
        oLabelLengthPrec.Text = "Maximum number of decimal places"

        oLabelLengthPrec.Name = "oLabelLengthPrec"
        oLabelLengthPrec.Location = New Point(60, 180)
        oLabelLengthPrec.Size = New Size(100, 40)
        oLabelLengthPrec.Visible = True
        oLabelLengthPrec.Enabled = True
        Controls.Add(oLabelLengthPrec)

        oCBAnglePrec = New ComboBox()
        'Me.ComboBox1 = New ComboBox
        oCBAnglePrec.Location = New Point(250, 80)
        oCBAnglePrec.Name = "oCBAnglePrec"
        oCBAnglePrec.Size = New Size(50, 21)
        oCBAnglePrec.TabIndex = 0
        oCBAnglePrec.DropDownStyle = ComboBoxStyle.DropDownList
        oCBAnglePrec.Text = "3"
        Dim DecimalPlaces() As String = {"1", "2", "3", "4", "5", "6"}
        oCBAnglePrec.Items.AddRange(DecimalPlaces)
        Controls.Add(oCBAnglePrec)

        oCBLengthPrec = New ComboBox()
        'Me.ComboBox1 = New ComboBox
        oCBLengthPrec.Location = New Point(250, 180)
        oCBLengthPrec.Name = "oLabelLengthPrec"
        oCBLengthPrec.Size = New Size(60, 180)
        oCBLengthPrec.TabIndex = 0
        oCBLengthPrec.DropDownStyle = ComboBoxStyle.DropDownList
        oCBLengthPrec.Text = "2"
        Dim DecimalPlaces1() As String = {"1", "2", "3", "4"}
        oCBLengthPrec.Items.AddRange(DecimalPlaces1)
        Controls.Add(oCBLengthPrec)

        oCBDefault = New CheckBox()
        oCBDefault.Text = "Use default settings"
        oCBDefault.Location = New Point(50, 220)
        oCBDefault.Size = New Size(160, 21)
        Controls.Add(oCBDefault)

        oExportButton = New Button()
        oExportButton.Text = "Export"
        oExportButton.Location = New Point(100, 250)
        Controls.Add(oExportButton)

        oCancelButton = New Button()
        oCancelButton.Text = "Cancel"
        oCancelButton.Location = New Point(200, 250)
        Controls.Add(oCancelButton)


    End Sub

    ' Export the XML on click
    Private Sub oExportButton_OnClick() Handles oExportButton.Click

        exportData(0) = oCBUnitOfAngles.Text
        exportData(1) = oCBAnglePrec.Text
        exportData(2) = oCBUnitOfLength.Text
        exportData(3) = oCBLengthPrec.Text

        ' Start export process
        Dim oExport As New ExportBRep(MIDAddin)
        oExport.ReadBRepAI(exportData)


    End Sub

    ' Cancel Button --> make dockable window invisible
    Private Sub oCancelButton_OnClick() Handles oCancelButton.Click

        If oWindow IsNot Nothing Then
            oWindow.Visible() = False
        End If
    End Sub

    ' Disable or enable via check box
    Public Sub oCBDefault_OnClick() Handles oCBDefault.CheckStateChanged
        If oCBDefault.Checked = True Then
            oCBUnitOfAngles.Enabled = False
            oCBUnitOfAngles.Text = "grad"
            oCBAnglePrec.Enabled = False
            oCBAnglePrec.Text = "2"
            oCBUnitOfLength.Enabled = False
            oCBUnitOfLength.Text = "mm"
            oCBLengthPrec.Enabled = False
            oCBLengthPrec.Text = "3"

        Else
            oCBUnitOfAngles.Enabled = True
            oCBAnglePrec.Enabled = True
            oCBUnitOfLength.Enabled = True
            oCBLengthPrec.Enabled = True

        End If



    End Sub

End Class