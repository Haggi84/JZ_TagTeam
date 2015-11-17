<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class KeepOutCmdDlg
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(KeepOutCmdDlg))
        Me.writeXmlCheck = New System.Windows.Forms.CheckBox()
        Me.infoLabel = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.filterCombobox = New System.Windows.Forms.ComboBox()
        Me.buttonCancel = New System.Windows.Forms.Button()
        Me.buttonOk = New System.Windows.Forms.Button()
        Me.buttonRmv = New System.Windows.Forms.Button()
        Me.buttonAdd = New System.Windows.Forms.Button()
        Me.buttonReset = New System.Windows.Forms.Button()
        Me.buttonHelp = New System.Windows.Forms.Button()
        Me.KeepOutsTB = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.KeepOutsTB.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.SuspendLayout()
        '
        'writeXmlCheck
        '
        Me.writeXmlCheck.AutoSize = True
        Me.writeXmlCheck.Location = New System.Drawing.Point(141, 131)
        Me.writeXmlCheck.Name = "writeXmlCheck"
        Me.writeXmlCheck.Size = New System.Drawing.Size(146, 17)
        Me.writeXmlCheck.TabIndex = 1
        Me.writeXmlCheck.Text = "automatically write xml file"
        Me.writeXmlCheck.UseVisualStyleBackColor = True
        '
        'infoLabel
        '
        Me.infoLabel.AutoSize = True
        Me.infoLabel.Location = New System.Drawing.Point(63, 40)
        Me.infoLabel.Name = "infoLabel"
        Me.infoLabel.Size = New System.Drawing.Size(191, 13)
        Me.infoLabel.TabIndex = 4
        Me.infoLabel.Text = "0 KeepOut faces are currently selected"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(63, 79)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(113, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Choose selection filter:"
        '
        'filterCombobox
        '
        Me.filterCombobox.FormattingEnabled = True
        Me.filterCombobox.Items.AddRange(New Object() {"All face types", "Plane face type", "Cylincrical face type"})
        Me.filterCombobox.Location = New System.Drawing.Point(66, 95)
        Me.filterCombobox.Name = "filterCombobox"
        Me.filterCombobox.Size = New System.Drawing.Size(221, 21)
        Me.filterCombobox.TabIndex = 2
        Me.filterCombobox.Text = "All face types"
        '
        'buttonCancel
        '
        Me.buttonCancel.Location = New System.Drawing.Point(262, 202)
        Me.buttonCancel.Name = "buttonCancel"
        Me.buttonCancel.Size = New System.Drawing.Size(75, 25)
        Me.buttonCancel.TabIndex = 1
        Me.buttonCancel.Text = "Cancel"
        Me.buttonCancel.UseVisualStyleBackColor = True
        '
        'buttonOk
        '
        Me.buttonOk.Location = New System.Drawing.Point(176, 202)
        Me.buttonOk.Name = "buttonOk"
        Me.buttonOk.Size = New System.Drawing.Size(75, 25)
        Me.buttonOk.TabIndex = 2
        Me.buttonOk.Text = "Ok"
        Me.buttonOk.UseVisualStyleBackColor = True
        '
        'buttonRmv
        '
        Me.buttonRmv.Image = Global.MIDAddin.My.Resources.Resources.mtbRmvSmall1
        Me.buttonRmv.Location = New System.Drawing.Point(14, 59)
        Me.buttonRmv.Name = "buttonRmv"
        Me.buttonRmv.Size = New System.Drawing.Size(30, 30)
        Me.buttonRmv.TabIndex = 3
        Me.buttonRmv.UseVisualStyleBackColor = True
        '
        'buttonAdd
        '
        Me.buttonAdd.Image = Global.MIDAddin.My.Resources.Resources.mtbAddSmall1
        Me.buttonAdd.Location = New System.Drawing.Point(14, 23)
        Me.buttonAdd.Name = "buttonAdd"
        Me.buttonAdd.Size = New System.Drawing.Size(30, 30)
        Me.buttonAdd.TabIndex = 4
        Me.buttonAdd.UseVisualStyleBackColor = True
        '
        'buttonReset
        '
        Me.buttonReset.Location = New System.Drawing.Point(14, 95)
        Me.buttonReset.Name = "buttonReset"
        Me.buttonReset.Size = New System.Drawing.Size(30, 30)
        Me.buttonReset.TabIndex = 5
        Me.buttonReset.UseVisualStyleBackColor = True
        '
        'buttonHelp
        '
        Me.buttonHelp.Image = CType(resources.GetObject("buttonHelp.Image"), System.Drawing.Image)
        Me.buttonHelp.Location = New System.Drawing.Point(29, 202)
        Me.buttonHelp.Name = "buttonHelp"
        Me.buttonHelp.Size = New System.Drawing.Size(29, 25)
        Me.buttonHelp.TabIndex = 6
        '
        'KeepOutsTB
        '
        Me.KeepOutsTB.Controls.Add(Me.TabPage1)
        Me.KeepOutsTB.Location = New System.Drawing.Point(12, 14)
        Me.KeepOutsTB.Name = "KeepOutsTB"
        Me.KeepOutsTB.SelectedIndex = 0
        Me.KeepOutsTB.Size = New System.Drawing.Size(338, 182)
        Me.KeepOutsTB.TabIndex = 8
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.filterCombobox)
        Me.TabPage1.Controls.Add(Me.writeXmlCheck)
        Me.TabPage1.Controls.Add(Me.infoLabel)
        Me.TabPage1.Controls.Add(Me.buttonReset)
        Me.TabPage1.Controls.Add(Me.Label1)
        Me.TabPage1.Controls.Add(Me.buttonRmv)
        Me.TabPage1.Controls.Add(Me.buttonAdd)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(330, 156)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Select KeepOuts"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'KeepOutCmdDlg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(362, 239)
        Me.Controls.Add(Me.KeepOutsTB)
        Me.Controls.Add(Me.buttonHelp)
        Me.Controls.Add(Me.buttonOk)
        Me.Controls.Add(Me.buttonCancel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "KeepOutCmdDlg"
        Me.Text = "Select KeepOuts"
        Me.KeepOutsTB.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents buttonCancel As System.Windows.Forms.Button
    Friend WithEvents buttonOk As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents filterCombobox As System.Windows.Forms.ComboBox
    Friend WithEvents infoLabel As System.Windows.Forms.Label
    Friend WithEvents buttonRmv As System.Windows.Forms.Button
    Friend WithEvents buttonAdd As System.Windows.Forms.Button
    Friend WithEvents buttonReset As System.Windows.Forms.Button
    Private WithEvents buttonHelp As System.Windows.Forms.Button
    Friend WithEvents writeXmlCheck As System.Windows.Forms.CheckBox
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Private WithEvents KeepOutsTB As System.Windows.Forms.TabControl
End Class
