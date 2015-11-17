<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PlacePartCmdDlg
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PlacePartCmdDlg))
        Me.okButton = New System.Windows.Forms.Button()
        Me.cancelButton = New System.Windows.Forms.Button()
        Me.applyButton = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.rotateTextBox = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.moveTextBox = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.upButton = New System.Windows.Forms.Button()
        Me.downButton = New System.Windows.Forms.Button()
        Me.rightButton = New System.Windows.Forms.Button()
        Me.rotateLeftButton = New System.Windows.Forms.Button()
        Me.rotateRightButton = New System.Windows.Forms.Button()
        Me.leftButton = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.buttonHelp = New System.Windows.Forms.Button()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.alignComboBox = New System.Windows.Forms.ComboBox()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.angleUnitLabel = New System.Windows.Forms.Label()
        Me.lengthUnitLabel = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.SuspendLayout()
        '
        'okButton
        '
        Me.okButton.Location = New System.Drawing.Point(116, 204)
        Me.okButton.Name = "okButton"
        Me.okButton.Size = New System.Drawing.Size(75, 23)
        Me.okButton.TabIndex = 0
        Me.okButton.Text = "OK"
        Me.okButton.UseVisualStyleBackColor = True
        '
        'cancelButton
        '
        Me.cancelButton.Location = New System.Drawing.Point(197, 204)
        Me.cancelButton.Name = "cancelButton"
        Me.cancelButton.Size = New System.Drawing.Size(75, 23)
        Me.cancelButton.TabIndex = 1
        Me.cancelButton.Text = "Cancel"
        Me.cancelButton.UseVisualStyleBackColor = True
        '
        'applyButton
        '
        Me.applyButton.Location = New System.Drawing.Point(278, 204)
        Me.applyButton.Name = "applyButton"
        Me.applyButton.Size = New System.Drawing.Size(75, 23)
        Me.applyButton.TabIndex = 5
        Me.applyButton.Text = "Apply"
        Me.applyButton.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lengthUnitLabel)
        Me.GroupBox1.Controls.Add(Me.angleUnitLabel)
        Me.GroupBox1.Controls.Add(Me.rotateTextBox)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.moveTextBox)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Location = New System.Drawing.Point(145, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(208, 88)
        Me.GroupBox1.TabIndex = 3
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Step Size"
        '
        'rotateTextBox
        '
        Me.rotateTextBox.Location = New System.Drawing.Point(73, 50)
        Me.rotateTextBox.Name = "rotateTextBox"
        Me.rotateTextBox.Size = New System.Drawing.Size(52, 20)
        Me.rotateTextBox.TabIndex = 10
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(15, 53)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(42, 13)
        Me.Label6.TabIndex = 9
        Me.Label6.Text = "Rotate "
        '
        'moveTextBox
        '
        Me.moveTextBox.Location = New System.Drawing.Point(73, 22)
        Me.moveTextBox.Name = "moveTextBox"
        Me.moveTextBox.Size = New System.Drawing.Size(52, 20)
        Me.moveTextBox.TabIndex = 8
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(15, 25)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(37, 13)
        Me.Label5.TabIndex = 7
        Me.Label5.Text = "Move:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(275, 174)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(12, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "z"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(194, 174)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(12, 13)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "y"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(110, 174)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(12, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "x"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(128, 171)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(63, 20)
        Me.TextBox1.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(19, 174)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(85, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Absolut Position:"
        '
        'upButton
        '
        Me.upButton.Location = New System.Drawing.Point(52, 17)
        Me.upButton.Name = "upButton"
        Me.upButton.Size = New System.Drawing.Size(28, 28)
        Me.upButton.TabIndex = 6
        Me.upButton.UseVisualStyleBackColor = True
        '
        'downButton
        '
        Me.downButton.Location = New System.Drawing.Point(52, 47)
        Me.downButton.Name = "downButton"
        Me.downButton.Size = New System.Drawing.Size(28, 28)
        Me.downButton.TabIndex = 7
        Me.downButton.UseVisualStyleBackColor = True
        '
        'rightButton
        '
        Me.rightButton.Location = New System.Drawing.Point(86, 47)
        Me.rightButton.Name = "rightButton"
        Me.rightButton.Size = New System.Drawing.Size(28, 28)
        Me.rightButton.TabIndex = 8
        Me.rightButton.UseVisualStyleBackColor = True
        '
        'rotateLeftButton
        '
        Me.rotateLeftButton.Location = New System.Drawing.Point(18, 15)
        Me.rotateLeftButton.Name = "rotateLeftButton"
        Me.rotateLeftButton.Size = New System.Drawing.Size(45, 28)
        Me.rotateLeftButton.TabIndex = 9
        Me.rotateLeftButton.UseVisualStyleBackColor = True
        '
        'rotateRightButton
        '
        Me.rotateRightButton.Location = New System.Drawing.Point(69, 15)
        Me.rotateRightButton.Name = "rotateRightButton"
        Me.rotateRightButton.Size = New System.Drawing.Size(45, 28)
        Me.rotateRightButton.TabIndex = 10
        Me.rotateRightButton.UseVisualStyleBackColor = True
        '
        'leftButton
        '
        Me.leftButton.Location = New System.Drawing.Point(18, 47)
        Me.leftButton.Name = "leftButton"
        Me.leftButton.Size = New System.Drawing.Size(28, 28)
        Me.leftButton.TabIndex = 11
        Me.leftButton.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.upButton)
        Me.GroupBox2.Controls.Add(Me.leftButton)
        Me.GroupBox2.Controls.Add(Me.downButton)
        Me.GroupBox2.Controls.Add(Me.rightButton)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(127, 88)
        Me.GroupBox2.TabIndex = 11
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Move"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.rotateLeftButton)
        Me.GroupBox3.Controls.Add(Me.rotateRightButton)
        Me.GroupBox3.Location = New System.Drawing.Point(12, 106)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(127, 54)
        Me.GroupBox3.TabIndex = 12
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Rotate"
        '
        'buttonHelp
        '
        Me.buttonHelp.Image = CType(resources.GetObject("buttonHelp.Image"), System.Drawing.Image)
        Me.buttonHelp.Location = New System.Drawing.Point(22, 208)
        Me.buttonHelp.Name = "buttonHelp"
        Me.buttonHelp.Size = New System.Drawing.Size(29, 25)
        Me.buttonHelp.TabIndex = 13
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.alignComboBox)
        Me.GroupBox4.Location = New System.Drawing.Point(145, 106)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(208, 54)
        Me.GroupBox4.TabIndex = 13
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Align"
        '
        'alignComboBox
        '
        Me.alignComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.alignComboBox.FormattingEnabled = True
        Me.alignComboBox.Items.AddRange(New Object() {"Align To Edge", "Align To MID"})
        Me.alignComboBox.Location = New System.Drawing.Point(10, 19)
        Me.alignComboBox.Name = "alignComboBox"
        Me.alignComboBox.Size = New System.Drawing.Size(127, 21)
        Me.alignComboBox.TabIndex = 0
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(209, 171)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(63, 20)
        Me.TextBox2.TabIndex = 14
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(288, 171)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(63, 20)
        Me.TextBox3.TabIndex = 15
        '
        'angleUnitLabel
        '
        Me.angleUnitLabel.AutoSize = True
        Me.angleUnitLabel.Location = New System.Drawing.Point(128, 53)
        Me.angleUnitLabel.Name = "angleUnitLabel"
        Me.angleUnitLabel.Size = New System.Drawing.Size(40, 13)
        Me.angleUnitLabel.TabIndex = 11
        Me.angleUnitLabel.Text = "degree"
        '
        'lengthUnitLabel
        '
        Me.lengthUnitLabel.AutoSize = True
        Me.lengthUnitLabel.Location = New System.Drawing.Point(128, 25)
        Me.lengthUnitLabel.Name = "lengthUnitLabel"
        Me.lengthUnitLabel.Size = New System.Drawing.Size(15, 13)
        Me.lengthUnitLabel.TabIndex = 12
        Me.lengthUnitLabel.Text = "in"
        '
        'PlacePartCmdDlg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(370, 243)
        Me.Controls.Add(Me.TextBox3)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.buttonHelp)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.applyButton)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cancelButton)
        Me.Controls.Add(Me.okButton)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TextBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "PlacePartCmdDlg"
        Me.Text = "Place Parts"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents okButton As System.Windows.Forms.Button
    Friend WithEvents cancelButton As System.Windows.Forms.Button
    Friend WithEvents applyButton As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents rotateTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents moveTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents upButton As System.Windows.Forms.Button
    Friend WithEvents downButton As System.Windows.Forms.Button
    Friend WithEvents rightButton As System.Windows.Forms.Button
    Friend WithEvents rotateLeftButton As System.Windows.Forms.Button
    Friend WithEvents rotateRightButton As System.Windows.Forms.Button
    Friend WithEvents leftButton As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Private WithEvents buttonHelp As System.Windows.Forms.Button
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents alignComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents lengthUnitLabel As System.Windows.Forms.Label
    Friend WithEvents angleUnitLabel As System.Windows.Forms.Label
End Class
