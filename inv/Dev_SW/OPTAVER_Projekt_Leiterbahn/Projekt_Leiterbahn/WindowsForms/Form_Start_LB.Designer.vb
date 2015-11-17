<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_Start_LB
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_Start_LB))
        Me.ButtonInfoZeichnen = New System.Windows.Forms.Button()
        Me.ButtonInfoBahnerstellen = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button_Info_Ueberschn = New System.Windows.Forms.Button()
        Me.Button_Ueberschneidung = New System.Windows.Forms.Button()
        Me.Button_Loeschen = New System.Windows.Forms.Button()
        Me.ButtonBahnerstellen = New System.Windows.Forms.Button()
        Me.ButtonZeichnen = New System.Windows.Forms.Button()
        Me.btn_entrance_light = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.btn_create_vector = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ButtonInfoZeichnen
        '
        Me.ButtonInfoZeichnen.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.ButtonInfoZeichnen.Location = New System.Drawing.Point(316, 41)
        Me.ButtonInfoZeichnen.Margin = New System.Windows.Forms.Padding(4)
        Me.ButtonInfoZeichnen.Name = "ButtonInfoZeichnen"
        Me.ButtonInfoZeichnen.Size = New System.Drawing.Size(65, 52)
        Me.ButtonInfoZeichnen.TabIndex = 3
        Me.ButtonInfoZeichnen.Text = "Info"
        Me.ButtonInfoZeichnen.UseVisualStyleBackColor = True
        '
        'ButtonInfoBahnerstellen
        '
        Me.ButtonInfoBahnerstellen.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.ButtonInfoBahnerstellen.Location = New System.Drawing.Point(316, 297)
        Me.ButtonInfoBahnerstellen.Margin = New System.Windows.Forms.Padding(4)
        Me.ButtonInfoBahnerstellen.Name = "ButtonInfoBahnerstellen"
        Me.ButtonInfoBahnerstellen.Size = New System.Drawing.Size(65, 52)
        Me.ButtonInfoBahnerstellen.TabIndex = 4
        Me.ButtonInfoBahnerstellen.Text = "Info"
        Me.ButtonInfoBahnerstellen.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Button2.Location = New System.Drawing.Point(316, 126)
        Me.Button2.Margin = New System.Windows.Forms.Padding(4)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(65, 50)
        Me.Button2.TabIndex = 6
        Me.Button2.Text = "Info"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button_Info_Ueberschn
        '
        Me.Button_Info_Ueberschn.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Button_Info_Ueberschn.Location = New System.Drawing.Point(316, 215)
        Me.Button_Info_Ueberschn.Margin = New System.Windows.Forms.Padding(4)
        Me.Button_Info_Ueberschn.Name = "Button_Info_Ueberschn"
        Me.Button_Info_Ueberschn.Size = New System.Drawing.Size(65, 47)
        Me.Button_Info_Ueberschn.TabIndex = 8
        Me.Button_Info_Ueberschn.Text = "Info"
        Me.Button_Info_Ueberschn.UseVisualStyleBackColor = True
        '
        'Button_Ueberschneidung
        '
        Me.Button_Ueberschneidung.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Button_Ueberschneidung.Image = Global.Projekt_Leiterbahn.My.Resources.Resources.Icon_Ueberschneidung7
        Me.Button_Ueberschneidung.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button_Ueberschneidung.Location = New System.Drawing.Point(37, 215)
        Me.Button_Ueberschneidung.Margin = New System.Windows.Forms.Padding(4)
        Me.Button_Ueberschneidung.Name = "Button_Ueberschneidung"
        Me.Button_Ueberschneidung.Size = New System.Drawing.Size(237, 47)
        Me.Button_Ueberschneidung.TabIndex = 7
        Me.Button_Ueberschneidung.Text = "Check for crossover"
        Me.Button_Ueberschneidung.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button_Ueberschneidung.UseVisualStyleBackColor = True
        '
        'Button_Loeschen
        '
        Me.Button_Loeschen.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Button_Loeschen.Image = CType(resources.GetObject("Button_Loeschen.Image"), System.Drawing.Image)
        Me.Button_Loeschen.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button_Loeschen.Location = New System.Drawing.Point(37, 126)
        Me.Button_Loeschen.Margin = New System.Windows.Forms.Padding(4)
        Me.Button_Loeschen.Name = "Button_Loeschen"
        Me.Button_Loeschen.Size = New System.Drawing.Size(237, 50)
        Me.Button_Loeschen.TabIndex = 5
        Me.Button_Loeschen.Text = "Delete Path"
        Me.Button_Loeschen.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button_Loeschen.UseVisualStyleBackColor = True
        '
        'ButtonBahnerstellen
        '
        Me.ButtonBahnerstellen.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.ButtonBahnerstellen.Image = CType(resources.GetObject("ButtonBahnerstellen.Image"), System.Drawing.Image)
        Me.ButtonBahnerstellen.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ButtonBahnerstellen.Location = New System.Drawing.Point(37, 297)
        Me.ButtonBahnerstellen.Margin = New System.Windows.Forms.Padding(4)
        Me.ButtonBahnerstellen.Name = "ButtonBahnerstellen"
        Me.ButtonBahnerstellen.Size = New System.Drawing.Size(237, 52)
        Me.ButtonBahnerstellen.TabIndex = 2
        Me.ButtonBahnerstellen.Text = "Create shape"
        Me.ButtonBahnerstellen.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonBahnerstellen.UseVisualStyleBackColor = True
        '
        'ButtonZeichnen
        '
        Me.ButtonZeichnen.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.ButtonZeichnen.Image = CType(resources.GetObject("ButtonZeichnen.Image"), System.Drawing.Image)
        Me.ButtonZeichnen.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ButtonZeichnen.Location = New System.Drawing.Point(37, 41)
        Me.ButtonZeichnen.Margin = New System.Windows.Forms.Padding(4)
        Me.ButtonZeichnen.Name = "ButtonZeichnen"
        Me.ButtonZeichnen.Size = New System.Drawing.Size(237, 52)
        Me.ButtonZeichnen.TabIndex = 1
        Me.ButtonZeichnen.Text = "Create Path "
        Me.ButtonZeichnen.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonZeichnen.UseVisualStyleBackColor = True
        '
        'btn_entrance_light
        '
        Me.btn_entrance_light.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.btn_entrance_light.Image = CType(resources.GetObject("btn_entrance_light.Image"), System.Drawing.Image)
        Me.btn_entrance_light.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_entrance_light.Location = New System.Drawing.Point(37, 386)
        Me.btn_entrance_light.Margin = New System.Windows.Forms.Padding(4)
        Me.btn_entrance_light.Name = "btn_entrance_light"
        Me.btn_entrance_light.Size = New System.Drawing.Size(237, 52)
        Me.btn_entrance_light.TabIndex = 9
        Me.btn_entrance_light.Text = "Position of lightsource"
        Me.btn_entrance_light.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_entrance_light.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Button3.Location = New System.Drawing.Point(316, 386)
        Me.Button3.Margin = New System.Windows.Forms.Padding(4)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(65, 52)
        Me.Button3.TabIndex = 10
        Me.Button3.Text = "Info"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Button1.Location = New System.Drawing.Point(316, 475)
        Me.Button1.Margin = New System.Windows.Forms.Padding(4)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(65, 52)
        Me.Button1.TabIndex = 12
        Me.Button1.Text = "Info"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'btn_create_vector
        '
        Me.btn_create_vector.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.btn_create_vector.Image = CType(resources.GetObject("btn_create_vector.Image"), System.Drawing.Image)
        Me.btn_create_vector.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btn_create_vector.Location = New System.Drawing.Point(37, 475)
        Me.btn_create_vector.Margin = New System.Windows.Forms.Padding(4)
        Me.btn_create_vector.Name = "btn_create_vector"
        Me.btn_create_vector.Size = New System.Drawing.Size(237, 52)
        Me.btn_create_vector.TabIndex = 11
        Me.btn_create_vector.Text = "Create vector"
        Me.btn_create_vector.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_create_vector.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(415, 41)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(200, 85)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 13
        Me.PictureBox1.TabStop = False
        '
        'Form_Start_LB
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ClientSize = New System.Drawing.Size(648, 552)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.btn_create_vector)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.btn_entrance_light)
        Me.Controls.Add(Me.Button_Info_Ueberschn)
        Me.Controls.Add(Me.Button_Ueberschneidung)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button_Loeschen)
        Me.Controls.Add(Me.ButtonInfoBahnerstellen)
        Me.Controls.Add(Me.ButtonInfoZeichnen)
        Me.Controls.Add(Me.ButtonBahnerstellen)
        Me.Controls.Add(Me.ButtonZeichnen)
        Me.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "Form_Start_LB"
        Me.Text = "Design Path"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ButtonZeichnen As System.Windows.Forms.Button
    Friend WithEvents ButtonBahnerstellen As System.Windows.Forms.Button
    Friend WithEvents ButtonInfoZeichnen As System.Windows.Forms.Button
    Friend WithEvents ButtonInfoBahnerstellen As System.Windows.Forms.Button
    Friend WithEvents Button_Loeschen As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button_Ueberschneidung As System.Windows.Forms.Button
    Friend WithEvents Button_Info_Ueberschn As System.Windows.Forms.Button
    Friend WithEvents btn_entrance_light As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents btn_create_vector As System.Windows.Forms.Button
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
End Class
