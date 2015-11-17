<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LoadProjectCmdDlg
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
        Me.pathTxtBox = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.browseButton = New System.Windows.Forms.Button()
        Me.okButton = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.logoBox = New System.Windows.Forms.PictureBox()
        Me.cancelButton = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        CType(Me.logoBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pathTxtBox
        '
        Me.pathTxtBox.Location = New System.Drawing.Point(27, 178)
        Me.pathTxtBox.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.pathTxtBox.Name = "pathTxtBox"
        Me.pathTxtBox.Size = New System.Drawing.Size(607, 22)
        Me.pathTxtBox.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(23, 159)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(122, 17)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Inventor File Path:"
        '
        'browseButton
        '
        Me.browseButton.Location = New System.Drawing.Point(535, 210)
        Me.browseButton.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.browseButton.Name = "browseButton"
        Me.browseButton.Size = New System.Drawing.Size(100, 28)
        Me.browseButton.TabIndex = 5
        Me.browseButton.Text = "Browse..."
        Me.browseButton.UseVisualStyleBackColor = True
        '
        'okButton
        '
        Me.okButton.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.okButton.Location = New System.Drawing.Point(223, 290)
        Me.okButton.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.okButton.Name = "okButton"
        Me.okButton.Size = New System.Drawing.Size(100, 28)
        Me.okButton.TabIndex = 6
        Me.okButton.Text = "Ok"
        Me.okButton.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.logoBox)
        Me.GroupBox1.Controls.Add(Me.pathTxtBox)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.browseButton)
        Me.GroupBox1.Location = New System.Drawing.Point(16, 15)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox1.Size = New System.Drawing.Size(660, 260)
        Me.GroupBox1.TabIndex = 7
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Load Project Properties"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(23, 57)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(191, 17)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Open a existing assembly file" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'logoBox
        '
        Me.logoBox.Image = Global.MIDAddin.My.Resources.Resources.Logo_minimiert
        Me.logoBox.InitialImage = Global.MIDAddin.My.Resources.Resources.Fpslogo
        Me.logoBox.Location = New System.Drawing.Point(336, 23)
        Me.logoBox.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.logoBox.Name = "logoBox"
        Me.logoBox.Size = New System.Drawing.Size(298, 89)
        Me.logoBox.TabIndex = 7
        Me.logoBox.TabStop = False
        '
        'cancelButton
        '
        Me.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cancelButton.Location = New System.Drawing.Point(373, 290)
        Me.cancelButton.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cancelButton.Name = "cancelButton"
        Me.cancelButton.Size = New System.Drawing.Size(100, 28)
        Me.cancelButton.TabIndex = 8
        Me.cancelButton.Text = "Cancel"
        Me.cancelButton.UseVisualStyleBackColor = True
        '
        'LoadProjectCmdDlg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(692, 334)
        Me.Controls.Add(Me.cancelButton)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.okButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "LoadProjectCmdDlg"
        Me.Text = "Load MID Project"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.logoBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pathTxtBox As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents browseButton As System.Windows.Forms.Button
    Friend WithEvents okButton As System.Windows.Forms.Button
    Private WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cancelButton As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents logoBox As System.Windows.Forms.PictureBox
End Class
