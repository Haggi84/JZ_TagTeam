<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AddNetCmdDlg
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AddNetCmdDlg))
        Me.buttonCancel = New System.Windows.Forms.Button()
        Me.importButton = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.filePathBox = New System.Windows.Forms.TextBox()
        Me.browseButton = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.buttonOk = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'buttonCancel
        '
        Me.buttonCancel.Location = New System.Drawing.Point(335, 236)
        Me.buttonCancel.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.buttonCancel.Name = "buttonCancel"
        Me.buttonCancel.Size = New System.Drawing.Size(100, 28)
        Me.buttonCancel.TabIndex = 0
        Me.buttonCancel.Text = "Cancel"
        Me.buttonCancel.UseVisualStyleBackColor = True
        '
        'importButton
        '
        Me.importButton.Location = New System.Drawing.Point(197, 178)
        Me.importButton.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.importButton.Name = "importButton"
        Me.importButton.Size = New System.Drawing.Size(100, 28)
        Me.importButton.TabIndex = 1
        Me.importButton.Text = "Import"
        Me.importButton.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.TextBox1)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.filePathBox)
        Me.GroupBox1.Controls.Add(Me.importButton)
        Me.GroupBox1.Controls.Add(Me.browseButton)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(16, 15)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox1.Size = New System.Drawing.Size(439, 214)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Import Netlist"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(25, 98)
        Me.TextBox1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(379, 22)
        Me.TextBox1.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(21, 79)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(263, 17)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Set reference folder path for part library:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(21, 34)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(215, 17)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Click Import to read from Xml-file."
        '
        'filePathBox
        '
        Me.filePathBox.Location = New System.Drawing.Point(25, 146)
        Me.filePathBox.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.filePathBox.Name = "filePathBox"
        Me.filePathBox.Size = New System.Drawing.Size(379, 22)
        Me.filePathBox.TabIndex = 2
        '
        'browseButton
        '
        Me.browseButton.Location = New System.Drawing.Point(305, 178)
        Me.browseButton.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.browseButton.Name = "browseButton"
        Me.browseButton.Size = New System.Drawing.Size(100, 28)
        Me.browseButton.TabIndex = 1
        Me.browseButton.Text = "Browse..."
        Me.browseButton.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(21, 127)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(174, 17)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Define file path for xml file:"
        '
        'buttonOk
        '
        Me.buttonOk.Location = New System.Drawing.Point(227, 236)
        Me.buttonOk.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.buttonOk.Name = "buttonOk"
        Me.buttonOk.Size = New System.Drawing.Size(100, 28)
        Me.buttonOk.TabIndex = 3
        Me.buttonOk.Text = "Ok"
        Me.buttonOk.UseVisualStyleBackColor = True
        '
        'AddNetCmdDlg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(483, 294)
        Me.Controls.Add(Me.buttonOk)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.buttonCancel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AddNetCmdDlg"
        Me.Text = "Import Netlist"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents buttonCancel As System.Windows.Forms.Button
    Friend WithEvents importButton As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents browseButton As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents filePathBox As System.Windows.Forms.TextBox
    Friend WithEvents buttonOk As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
End Class
