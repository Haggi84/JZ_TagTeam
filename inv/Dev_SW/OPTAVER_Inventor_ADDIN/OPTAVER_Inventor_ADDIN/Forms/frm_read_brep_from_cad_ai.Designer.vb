<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frm_read_brep_from_cad_ai
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frm_read_brep_from_cad_ai))
        Me.btnRun = New System.Windows.Forms.Button()
        Me.btnCanc = New System.Windows.Forms.Button()
        Me.lblHeadline = New System.Windows.Forms.Label()
        Me.lblHeadline2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.chbWithKeys = New System.Windows.Forms.CheckBox()
        Me.cmbAddPrec = New System.Windows.Forms.ComboBox()
        Me.lblAddPrec = New System.Windows.Forms.Label()
        Me.lblCadSyst = New System.Windows.Forms.Label()
        Me.txbCadSyst = New System.Windows.Forms.TextBox()
        Me.imgMID = New System.Windows.Forms.PictureBox()
        CType(Me.imgMID, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnRun
        '
        Me.btnRun.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnRun.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRun.Location = New System.Drawing.Point(24, 330)
        Me.btnRun.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnRun.Name = "btnRun"
        Me.btnRun.Size = New System.Drawing.Size(381, 37)
        Me.btnRun.TabIndex = 3
        Me.btnRun.Text = "Run"
        Me.btnRun.UseVisualStyleBackColor = True
        '
        'btnCanc
        '
        Me.btnCanc.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCanc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCanc.Location = New System.Drawing.Point(553, 335)
        Me.btnCanc.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnCanc.Name = "btnCanc"
        Me.btnCanc.Size = New System.Drawing.Size(53, 28)
        Me.btnCanc.TabIndex = 4
        Me.btnCanc.Text = "Exit"
        Me.btnCanc.UseVisualStyleBackColor = True
        '
        'lblHeadline
        '
        Me.lblHeadline.AutoSize = True
        Me.lblHeadline.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.lblHeadline.Font = New System.Drawing.Font("Tahoma", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHeadline.Location = New System.Drawing.Point(16, 25)
        Me.lblHeadline.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblHeadline.Name = "lblHeadline"
        Me.lblHeadline.Size = New System.Drawing.Size(454, 41)
        Me.lblHeadline.TabIndex = 8
        Me.lblHeadline.Text = "Read BRep from Inventor"
        '
        'lblHeadline2
        '
        Me.lblHeadline2.AutoSize = True
        Me.lblHeadline2.Font = New System.Drawing.Font("Trebuchet MS", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHeadline2.Location = New System.Drawing.Point(19, 80)
        Me.lblHeadline2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblHeadline2.Name = "lblHeadline2"
        Me.lblHeadline2.Size = New System.Drawing.Size(436, 26)
        Me.lblHeadline2.TabIndex = 9
        Me.lblHeadline2.Text = "Import BRep from Inventor and write it to XML"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(19, 128)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(114, 29)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "Options:"
        '
        'chbWithKeys
        '
        Me.chbWithKeys.AutoSize = True
        Me.chbWithKeys.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chbWithKeys.Location = New System.Drawing.Point(24, 169)
        Me.chbWithKeys.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.chbWithKeys.Name = "chbWithKeys"
        Me.chbWithKeys.Size = New System.Drawing.Size(257, 33)
        Me.chbWithKeys.TabIndex = 11
        Me.chbWithKeys.Text = "With Reference Keys"
        Me.chbWithKeys.UseVisualStyleBackColor = True
        '
        'cmbAddPrec
        '
        Me.cmbAddPrec.FormattingEnabled = True
        Me.cmbAddPrec.Location = New System.Drawing.Point(348, 176)
        Me.cmbAddPrec.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cmbAddPrec.Name = "cmbAddPrec"
        Me.cmbAddPrec.Size = New System.Drawing.Size(71, 24)
        Me.cmbAddPrec.TabIndex = 12
        Me.cmbAddPrec.Text = "0"
        '
        'lblAddPrec
        '
        Me.lblAddPrec.AutoSize = True
        Me.lblAddPrec.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAddPrec.Location = New System.Drawing.Point(440, 174)
        Me.lblAddPrec.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblAddPrec.Name = "lblAddPrec"
        Me.lblAddPrec.Size = New System.Drawing.Size(216, 29)
        Me.lblAddPrec.TabIndex = 13
        Me.lblAddPrec.Text = "Additional Precision"
        '
        'lblCadSyst
        '
        Me.lblCadSyst.AutoSize = True
        Me.lblCadSyst.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCadSyst.Location = New System.Drawing.Point(19, 233)
        Me.lblCadSyst.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCadSyst.Name = "lblCadSyst"
        Me.lblCadSyst.Size = New System.Drawing.Size(294, 29)
        Me.lblCadSyst.TabIndex = 14
        Me.lblCadSyst.Text = "Autodesk Inventor Version"
        '
        'txbCadSyst
        '
        Me.txbCadSyst.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txbCadSyst.Location = New System.Drawing.Point(24, 265)
        Me.txbCadSyst.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txbCadSyst.Name = "txbCadSyst"
        Me.txbCadSyst.Size = New System.Drawing.Size(629, 27)
        Me.txbCadSyst.TabIndex = 15
        Me.txbCadSyst.Text = "Autodesk Inventor Professional 2015"
        '
        'imgMID
        '
        Me.imgMID.Image = CType(resources.GetObject("imgMID.Image"), System.Drawing.Image)
        Me.imgMID.InitialImage = CType(resources.GetObject("imgMID.InitialImage"), System.Drawing.Image)
        Me.imgMID.Location = New System.Drawing.Point(501, 15)
        Me.imgMID.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.imgMID.Name = "imgMID"
        Me.imgMID.Size = New System.Drawing.Size(175, 80)
        Me.imgMID.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.imgMID.TabIndex = 7
        Me.imgMID.TabStop = False
        '
        'dlg_read_brep_from_cad_ai
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(677, 394)
        Me.Controls.Add(Me.txbCadSyst)
        Me.Controls.Add(Me.lblCadSyst)
        Me.Controls.Add(Me.lblAddPrec)
        Me.Controls.Add(Me.cmbAddPrec)
        Me.Controls.Add(Me.chbWithKeys)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblHeadline2)
        Me.Controls.Add(Me.lblHeadline)
        Me.Controls.Add(Me.imgMID)
        Me.Controls.Add(Me.btnCanc)
        Me.Controls.Add(Me.btnRun)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dlg_read_brep_from_cad_ai"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "OPTAVER"
        CType(Me.imgMID, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnRun As System.Windows.Forms.Button
    Friend WithEvents btnCanc As System.Windows.Forms.Button
    Friend WithEvents imgMID As System.Windows.Forms.PictureBox
    Friend WithEvents lblHeadline As System.Windows.Forms.Label
    Friend WithEvents lblHeadline2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chbWithKeys As System.Windows.Forms.CheckBox
    Friend WithEvents cmbAddPrec As System.Windows.Forms.ComboBox
    Friend WithEvents lblAddPrec As System.Windows.Forms.Label
    Friend WithEvents lblCadSyst As System.Windows.Forms.Label
    Friend WithEvents txbCadSyst As System.Windows.Forms.TextBox

End Class
