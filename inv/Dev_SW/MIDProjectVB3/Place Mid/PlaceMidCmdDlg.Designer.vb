<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PlaceMidCmdDlg
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PlaceMidCmdDlg))
        Me.TabControl = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.browseButton = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.filePathBox = New System.Windows.Forms.TextBox()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.unitOfAnglesCB = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.anglePrecCB = New System.Windows.Forms.ComboBox()
        Me.lengthPrecCB = New System.Windows.Forms.ComboBox()
        Me.unitOfLengthCB = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.defaultCheckBox = New System.Windows.Forms.CheckBox()
        Me.buttonOk = New System.Windows.Forms.Button()
        Me.buttonCancel = New System.Windows.Forms.Button()
        Me.TabControl.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl
        '
        Me.TabControl.Controls.Add(Me.TabPage1)
        Me.TabControl.Controls.Add(Me.TabPage2)
        Me.TabControl.Location = New System.Drawing.Point(12, 12)
        Me.TabControl.Name = "TabControl"
        Me.TabControl.SelectedIndex = 0
        Me.TabControl.Size = New System.Drawing.Size(338, 197)
        Me.TabControl.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.Label2)
        Me.TabPage1.Controls.Add(Me.browseButton)
        Me.TabPage1.Controls.Add(Me.Label1)
        Me.TabPage1.Controls.Add(Me.filePathBox)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(330, 171)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Place MID"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(14, 25)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(154, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Choose a MID circuit carrier file" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'browseButton
        '
        Me.browseButton.Location = New System.Drawing.Point(239, 119)
        Me.browseButton.Name = "browseButton"
        Me.browseButton.Size = New System.Drawing.Size(75, 23)
        Me.browseButton.TabIndex = 2
        Me.browseButton.Text = "Browse..."
        Me.browseButton.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(14, 72)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(50, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "File path:"
        '
        'filePathBox
        '
        Me.filePathBox.Location = New System.Drawing.Point(17, 92)
        Me.filePathBox.Name = "filePathBox"
        Me.filePathBox.Size = New System.Drawing.Size(297, 20)
        Me.filePathBox.TabIndex = 0
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.unitOfAnglesCB)
        Me.TabPage2.Controls.Add(Me.Label8)
        Me.TabPage2.Controls.Add(Me.anglePrecCB)
        Me.TabPage2.Controls.Add(Me.lengthPrecCB)
        Me.TabPage2.Controls.Add(Me.unitOfLengthCB)
        Me.TabPage2.Controls.Add(Me.Label7)
        Me.TabPage2.Controls.Add(Me.Label6)
        Me.TabPage2.Controls.Add(Me.Label4)
        Me.TabPage2.Controls.Add(Me.Label3)
        Me.TabPage2.Controls.Add(Me.defaultCheckBox)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(330, 171)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Export Settings"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'unitOfAnglesCB
        '
        Me.unitOfAnglesCB.Location = New System.Drawing.Point(209, 46)
        Me.unitOfAnglesCB.Name = "unitOfAnglesCB"
        Me.unitOfAnglesCB.Size = New System.Drawing.Size(87, 21)
        Me.unitOfAnglesCB.TabIndex = 10
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(18, 49)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(73, 13)
        Me.Label8.TabIndex = 9
        Me.Label8.Text = "Unit of Angles"
        '
        'anglePrecCB
        '
        Me.anglePrecCB.AllowDrop = True
        Me.anglePrecCB.Items.AddRange(New Object() {"1", "2", "3", "4"})
        Me.anglePrecCB.Location = New System.Drawing.Point(209, 22)
        Me.anglePrecCB.Name = "anglePrecCB"
        Me.anglePrecCB.Size = New System.Drawing.Size(60, 21)
        Me.anglePrecCB.TabIndex = 8
        '
        'lengthPrecCB
        '
        Me.lengthPrecCB.AllowDrop = True
        Me.lengthPrecCB.Items.AddRange(New Object() {"1", "2", "3", "4", "5", "6", "7"})
        Me.lengthPrecCB.Location = New System.Drawing.Point(209, 85)
        Me.lengthPrecCB.Name = "lengthPrecCB"
        Me.lengthPrecCB.Size = New System.Drawing.Size(60, 21)
        Me.lengthPrecCB.TabIndex = 7
        '
        'unitOfLengthCB
        '
        Me.unitOfLengthCB.AllowDrop = True
        Me.unitOfLengthCB.Location = New System.Drawing.Point(209, 108)
        Me.unitOfLengthCB.Name = "unitOfLengthCB"
        Me.unitOfLengthCB.Size = New System.Drawing.Size(87, 21)
        Me.unitOfLengthCB.TabIndex = 6
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(18, 111)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(79, 13)
        Me.Label7.TabIndex = 5
        Me.Label7.Text = "Unit of Lengths"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(18, 88)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(86, 13)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "Length Precision"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(18, 25)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(80, 13)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Angle Precision"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(18, 14)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(0, 13)
        Me.Label3.TabIndex = 1
        '
        'defaultCheckBox
        '
        Me.defaultCheckBox.AutoSize = True
        Me.defaultCheckBox.Checked = True
        Me.defaultCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.defaultCheckBox.Location = New System.Drawing.Point(21, 142)
        Me.defaultCheckBox.Name = "defaultCheckBox"
        Me.defaultCheckBox.Size = New System.Drawing.Size(119, 17)
        Me.defaultCheckBox.TabIndex = 0
        Me.defaultCheckBox.Text = "Use default settings"
        Me.defaultCheckBox.UseVisualStyleBackColor = True
        '
        'buttonOk
        '
        Me.buttonOk.Location = New System.Drawing.Point(155, 215)
        Me.buttonOk.Name = "buttonOk"
        Me.buttonOk.Size = New System.Drawing.Size(75, 23)
        Me.buttonOk.TabIndex = 1
        Me.buttonOk.Text = "Open"
        Me.buttonOk.UseVisualStyleBackColor = True
        '
        'buttonCancel
        '
        Me.buttonCancel.Location = New System.Drawing.Point(255, 215)
        Me.buttonCancel.Name = "buttonCancel"
        Me.buttonCancel.Size = New System.Drawing.Size(75, 23)
        Me.buttonCancel.TabIndex = 2
        Me.buttonCancel.Text = "Cancel"
        Me.buttonCancel.UseVisualStyleBackColor = True
        '
        'PlaceMidCmdDlg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(362, 243)
        Me.Controls.Add(Me.buttonCancel)
        Me.Controls.Add(Me.buttonOk)
        Me.Controls.Add(Me.TabControl)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "PlaceMidCmdDlg"
        Me.Text = "Place Circuit Carrier"
        Me.TabControl.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TabControl As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents buttonOk As System.Windows.Forms.Button
    Friend WithEvents buttonCancel As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents browseButton As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents filePathBox As System.Windows.Forms.TextBox
    Friend WithEvents defaultCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents anglePrecCB As System.Windows.Forms.ComboBox
    Friend WithEvents lengthPrecCB As System.Windows.Forms.ComboBox
    Friend WithEvents unitOfLengthCB As System.Windows.Forms.ComboBox
    Friend WithEvents unitOfAnglesCB As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
End Class
