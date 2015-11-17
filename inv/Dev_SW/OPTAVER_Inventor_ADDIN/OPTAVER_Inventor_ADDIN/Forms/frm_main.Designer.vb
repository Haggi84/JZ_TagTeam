<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frm_main
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
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

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frm_main))
        Me.btn_conductor = New System.Windows.Forms.Button()
        Me.btn_new_project = New System.Windows.Forms.Button()
        Me.btn_cmpnt_send = New System.Windows.Forms.Button()
        Me.btn_cmpnt_receiver = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btn_BRep = New System.Windows.Forms.Button()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btn_conductor
        '
        Me.btn_conductor.Location = New System.Drawing.Point(12, 150)
        Me.btn_conductor.Name = "btn_conductor"
        Me.btn_conductor.Size = New System.Drawing.Size(190, 45)
        Me.btn_conductor.TabIndex = 2
        Me.btn_conductor.Text = "Strecke"
        Me.btn_conductor.UseVisualStyleBackColor = True
        '
        'btn_new_project
        '
        Me.btn_new_project.Location = New System.Drawing.Point(13, 22)
        Me.btn_new_project.Name = "btn_new_project"
        Me.btn_new_project.Size = New System.Drawing.Size(190, 43)
        Me.btn_new_project.TabIndex = 0
        Me.btn_new_project.Text = "Neues Projekt"
        Me.btn_new_project.UseVisualStyleBackColor = True
        '
        'btn_cmpnt_send
        '
        Me.btn_cmpnt_send.Location = New System.Drawing.Point(13, 86)
        Me.btn_cmpnt_send.Name = "btn_cmpnt_send"
        Me.btn_cmpnt_send.Size = New System.Drawing.Size(190, 45)
        Me.btn_cmpnt_send.TabIndex = 1
        Me.btn_cmpnt_send.Text = "Sendeelement"
        Me.btn_cmpnt_send.UseVisualStyleBackColor = True
        '
        'btn_cmpnt_receiver
        '
        Me.btn_cmpnt_receiver.Location = New System.Drawing.Point(13, 217)
        Me.btn_cmpnt_receiver.Name = "btn_cmpnt_receiver"
        Me.btn_cmpnt_receiver.Size = New System.Drawing.Size(190, 45)
        Me.btn_cmpnt_receiver.TabIndex = 3
        Me.btn_cmpnt_receiver.Text = "Empfänger"
        Me.btn_cmpnt_receiver.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(252, 22)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(223, 90)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 4
        Me.PictureBox1.TabStop = False
        '
        'ToolTip1
        '
        Me.ToolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info
        '
        'btn_BRep
        '
        Me.btn_BRep.Location = New System.Drawing.Point(13, 279)
        Me.btn_BRep.Name = "btn_BRep"
        Me.btn_BRep.Size = New System.Drawing.Size(190, 51)
        Me.btn_BRep.TabIndex = 5
        Me.btn_BRep.Text = "BRep"
        Me.btn_BRep.UseVisualStyleBackColor = True
        '
        'frm_main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(505, 351)
        Me.Controls.Add(Me.btn_BRep)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.btn_cmpnt_receiver)
        Me.Controls.Add(Me.btn_conductor)
        Me.Controls.Add(Me.btn_cmpnt_send)
        Me.Controls.Add(Me.btn_new_project)
        Me.Name = "frm_main"
        Me.Text = "Hauptmenü"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btn_new_project As System.Windows.Forms.Button
    Friend WithEvents btn_cmpnt_send As System.Windows.Forms.Button
    Friend WithEvents btn_cmpnt_receiver As System.Windows.Forms.Button
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents btn_conductor As System.Windows.Forms.Button
    Friend WithEvents btn_BRep As System.Windows.Forms.Button
End Class
