<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlg_read_brep_from_cad_ai
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
      Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(dlg_read_brep_from_cad_ai))
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
      CType(Me.imgMID,System.ComponentModel.ISupportInitialize).BeginInit
      Me.SuspendLayout
      '
      'btnRun
      '
      Me.btnRun.DialogResult = System.Windows.Forms.DialogResult.OK
      Me.btnRun.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
      Me.btnRun.Location = New System.Drawing.Point(18, 268)
      Me.btnRun.Name = "btnRun"
      Me.btnRun.Size = New System.Drawing.Size(286, 30)
      Me.btnRun.TabIndex = 3
      Me.btnRun.Text = "Run"
      Me.btnRun.UseVisualStyleBackColor = true
      '
      'btnCanc
      '
      Me.btnCanc.DialogResult = System.Windows.Forms.DialogResult.Cancel
      Me.btnCanc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
      Me.btnCanc.Location = New System.Drawing.Point(415, 272)
      Me.btnCanc.Name = "btnCanc"
      Me.btnCanc.Size = New System.Drawing.Size(40, 23)
      Me.btnCanc.TabIndex = 4
      Me.btnCanc.Text = "Exit"
      Me.btnCanc.UseVisualStyleBackColor = true
      '
      'lblHeadline
      '
      Me.lblHeadline.AutoSize = true
      Me.lblHeadline.BackColor = System.Drawing.SystemColors.ButtonFace
      Me.lblHeadline.Font = New System.Drawing.Font("Tahoma", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
      Me.lblHeadline.Location = New System.Drawing.Point(12, 20)
      Me.lblHeadline.Name = "lblHeadline"
      Me.lblHeadline.Size = New System.Drawing.Size(346, 33)
      Me.lblHeadline.TabIndex = 8
      Me.lblHeadline.Text = "ReadBRepFromInventor"
      '
      'lblHeadline2
      '
      Me.lblHeadline2.AutoSize = true
      Me.lblHeadline2.Font = New System.Drawing.Font("Trebuchet MS", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
      Me.lblHeadline2.Location = New System.Drawing.Point(14, 65)
      Me.lblHeadline2.Name = "lblHeadline2"
      Me.lblHeadline2.Size = New System.Drawing.Size(353, 22)
      Me.lblHeadline2.TabIndex = 9
      Me.lblHeadline2.Text = "Import BRep from Inventor and write it to a file"
      '
      'Label1
      '
      Me.Label1.AutoSize = true
      Me.Label1.BackColor = System.Drawing.SystemColors.ButtonFace
      Me.Label1.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
      Me.Label1.Location = New System.Drawing.Point(14, 104)
      Me.Label1.Name = "Label1"
      Me.Label1.Size = New System.Drawing.Size(92, 23)
      Me.Label1.TabIndex = 10
      Me.Label1.Text = "Options:"
      '
      'chbWithKeys
      '
      Me.chbWithKeys.AutoSize = true
      Me.chbWithKeys.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
      Me.chbWithKeys.Location = New System.Drawing.Point(18, 137)
      Me.chbWithKeys.Name = "chbWithKeys"
      Me.chbWithKeys.Size = New System.Drawing.Size(202, 27)
      Me.chbWithKeys.TabIndex = 11
      Me.chbWithKeys.Text = "With Reference Keys"
      Me.chbWithKeys.UseVisualStyleBackColor = true
      '
      'cmbAddPrec
      '
      Me.cmbAddPrec.FormattingEnabled = true
      Me.cmbAddPrec.Location = New System.Drawing.Point(261, 143)
      Me.cmbAddPrec.Name = "cmbAddPrec"
      Me.cmbAddPrec.Size = New System.Drawing.Size(54, 21)
      Me.cmbAddPrec.TabIndex = 12
      Me.cmbAddPrec.Text = "0"
      '
      'lblAddPrec
      '
      Me.lblAddPrec.AutoSize = true
      Me.lblAddPrec.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
      Me.lblAddPrec.Location = New System.Drawing.Point(330, 141)
      Me.lblAddPrec.Name = "lblAddPrec"
      Me.lblAddPrec.Size = New System.Drawing.Size(171, 23)
      Me.lblAddPrec.TabIndex = 13
      Me.lblAddPrec.Text = "Additional Precision"
      '
      'lblCadSyst
      '
      Me.lblCadSyst.AutoSize = true
      Me.lblCadSyst.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
      Me.lblCadSyst.Location = New System.Drawing.Point(14, 189)
      Me.lblCadSyst.Name = "lblCadSyst"
      Me.lblCadSyst.Size = New System.Drawing.Size(230, 23)
      Me.lblCadSyst.TabIndex = 14
      Me.lblCadSyst.Text = "Autodesk Inventor Version"
      '
      'txbCadSyst
      '
      Me.txbCadSyst.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
      Me.txbCadSyst.Location = New System.Drawing.Point(18, 215)
      Me.txbCadSyst.Name = "txbCadSyst"
      Me.txbCadSyst.Size = New System.Drawing.Size(473, 23)
      Me.txbCadSyst.TabIndex = 15
      Me.txbCadSyst.Text = "Autodesk Inventor Professional 2013"
      '
      'imgMID
      '
      Me.imgMID.Image = CType(resources.GetObject("imgMID.Image"),System.Drawing.Image)
      Me.imgMID.InitialImage = CType(resources.GetObject("imgMID.InitialImage"),System.Drawing.Image)
      Me.imgMID.Location = New System.Drawing.Point(376, 12)
      Me.imgMID.Name = "imgMID"
      Me.imgMID.Size = New System.Drawing.Size(115, 80)
      Me.imgMID.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
      Me.imgMID.TabIndex = 7
      Me.imgMID.TabStop = false
      '
      'dlg_read_brep_from_cad_ai
      '
      Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
      Me.ClientSize = New System.Drawing.Size(508, 320)
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
      Me.MaximizeBox = false
      Me.MinimizeBox = false
      Me.Name = "dlg_read_brep_from_cad_ai"
      Me.ShowInTaskbar = false
      Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
      Me.Text = "MID-Layout"
      CType(Me.imgMID,System.ComponentModel.ISupportInitialize).EndInit
      Me.ResumeLayout(false)
      Me.PerformLayout

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
