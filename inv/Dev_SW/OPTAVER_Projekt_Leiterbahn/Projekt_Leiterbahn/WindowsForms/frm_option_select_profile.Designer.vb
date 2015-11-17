<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frm_option_select_profile
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frm_option_select_profile))
        Me.btnPIProfile = New System.Windows.Forms.Button()
        Me.btnPCProfile = New System.Windows.Forms.Button()
        Me.btnGlassProfile = New System.Windows.Forms.Button()
        Me.btnCustomProfile = New System.Windows.Forms.Button()
        Me.pb_custom = New System.Windows.Forms.PictureBox()
        Me.pb_PI = New System.Windows.Forms.PictureBox()
        Me.pb_PC = New System.Windows.Forms.PictureBox()
        Me.pb_glass = New System.Windows.Forms.PictureBox()
        CType(Me.pb_custom, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pb_PI, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pb_PC, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pb_glass, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnPIProfile
        '
        Me.btnPIProfile.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnPIProfile.Location = New System.Drawing.Point(38, 168)
        Me.btnPIProfile.Name = "btnPIProfile"
        Me.btnPIProfile.Size = New System.Drawing.Size(173, 36)
        Me.btnPIProfile.TabIndex = 14
        Me.btnPIProfile.Text = "PI-Profile"
        Me.btnPIProfile.UseVisualStyleBackColor = False
        '
        'btnPCProfile
        '
        Me.btnPCProfile.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnPCProfile.Location = New System.Drawing.Point(289, 168)
        Me.btnPCProfile.Name = "btnPCProfile"
        Me.btnPCProfile.Size = New System.Drawing.Size(173, 36)
        Me.btnPCProfile.TabIndex = 15
        Me.btnPCProfile.Text = "PC-Profile"
        Me.btnPCProfile.UseVisualStyleBackColor = False
        '
        'btnGlassProfile
        '
        Me.btnGlassProfile.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnGlassProfile.Location = New System.Drawing.Point(545, 168)
        Me.btnGlassProfile.Name = "btnGlassProfile"
        Me.btnGlassProfile.Size = New System.Drawing.Size(173, 36)
        Me.btnGlassProfile.TabIndex = 16
        Me.btnGlassProfile.Text = "Glass-Profile"
        Me.btnGlassProfile.UseVisualStyleBackColor = False
        '
        'btnCustomProfile
        '
        Me.btnCustomProfile.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnCustomProfile.Location = New System.Drawing.Point(798, 168)
        Me.btnCustomProfile.Name = "btnCustomProfile"
        Me.btnCustomProfile.Size = New System.Drawing.Size(162, 36)
        Me.btnCustomProfile.TabIndex = 17
        Me.btnCustomProfile.Text = "Custom Profile"
        Me.btnCustomProfile.UseVisualStyleBackColor = False
        '
        'pb_custom
        '
        Me.pb_custom.Image = CType(resources.GetObject("pb_custom.Image"), System.Drawing.Image)
        Me.pb_custom.Location = New System.Drawing.Point(798, 20)
        Me.pb_custom.Name = "pb_custom"
        Me.pb_custom.Size = New System.Drawing.Size(162, 130)
        Me.pb_custom.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pb_custom.TabIndex = 3
        Me.pb_custom.TabStop = False
        '
        'pb_PI
        '
        Me.pb_PI.Image = Global.Projekt_Leiterbahn.My.Resources.Resources.LB_profile_glass_curve_new
        Me.pb_PI.Location = New System.Drawing.Point(545, 20)
        Me.pb_PI.Name = "pb_PI"
        Me.pb_PI.Size = New System.Drawing.Size(173, 127)
        Me.pb_PI.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pb_PI.TabIndex = 2
        Me.pb_PI.TabStop = False
        '
        'pb_PC
        '
        Me.pb_PC.Image = CType(resources.GetObject("pb_PC.Image"), System.Drawing.Image)
        Me.pb_PC.Location = New System.Drawing.Point(289, 20)
        Me.pb_PC.Name = "pb_PC"
        Me.pb_PC.Size = New System.Drawing.Size(235, 138)
        Me.pb_PC.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pb_PC.TabIndex = 1
        Me.pb_PC.TabStop = False
        '
        'pb_glass
        '
        Me.pb_glass.Image = Global.Projekt_Leiterbahn.My.Resources.Resources.LB_profile_PI_curve_new
        Me.pb_glass.Location = New System.Drawing.Point(36, 20)
        Me.pb_glass.Name = "pb_glass"
        Me.pb_glass.Size = New System.Drawing.Size(176, 138)
        Me.pb_glass.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pb_glass.TabIndex = 0
        Me.pb_glass.TabStop = False
        '
        'frm_option_select_profile
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1026, 222)
        Me.Controls.Add(Me.btnCustomProfile)
        Me.Controls.Add(Me.btnGlassProfile)
        Me.Controls.Add(Me.btnPCProfile)
        Me.Controls.Add(Me.btnPIProfile)
        Me.Controls.Add(Me.pb_custom)
        Me.Controls.Add(Me.pb_PI)
        Me.Controls.Add(Me.pb_PC)
        Me.Controls.Add(Me.pb_glass)
        Me.Name = "frm_option_select_profile"
        Me.Text = "Select Profile"
        CType(Me.pb_custom, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pb_PI, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pb_PC, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pb_glass, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pb_custom As System.Windows.Forms.PictureBox
    Friend WithEvents pb_PI As System.Windows.Forms.PictureBox
    Friend WithEvents pb_glass As System.Windows.Forms.PictureBox
    Friend WithEvents pb_PC As System.Windows.Forms.PictureBox
    Friend WithEvents btnPIProfile As System.Windows.Forms.Button
    Friend WithEvents btnPCProfile As System.Windows.Forms.Button
    Friend WithEvents btnGlassProfile As System.Windows.Forms.Button
    Friend WithEvents btnCustomProfile As System.Windows.Forms.Button
End Class
