﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlg_map_routing
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
      Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(dlg_map_routing))
      Me.lblHeadline = New System.Windows.Forms.Label()
      Me.lblHeadline2 = New System.Windows.Forms.Label()
      Me.imgMID = New System.Windows.Forms.PictureBox()
      Me.btnRun = New System.Windows.Forms.Button()
      Me.btnCanc = New System.Windows.Forms.Button()
      CType(Me.imgMID,System.ComponentModel.ISupportInitialize).BeginInit
      Me.SuspendLayout
      '
      'lblHeadline
      '
      Me.lblHeadline.AutoSize = true
      Me.lblHeadline.BackColor = System.Drawing.SystemColors.ButtonFace
      Me.lblHeadline.Font = New System.Drawing.Font("Tahoma", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
      Me.lblHeadline.Location = New System.Drawing.Point(12, 20)
      Me.lblHeadline.Name = "lblHeadline"
      Me.lblHeadline.Size = New System.Drawing.Size(187, 33)
      Me.lblHeadline.TabIndex = 17
      Me.lblHeadline.Text = "Map Routing"
      '
      'lblHeadline2
      '
      Me.lblHeadline2.AutoSize = true
      Me.lblHeadline2.Font = New System.Drawing.Font("Trebuchet MS", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
      Me.lblHeadline2.Location = New System.Drawing.Point(14, 65)
      Me.lblHeadline2.Name = "lblHeadline2"
      Me.lblHeadline2.Size = New System.Drawing.Size(300, 22)
      Me.lblHeadline2.TabIndex = 18
      Me.lblHeadline2.Text = "Map the planar routing result on 3D BRep"
      '
      'imgMID
      '
      Me.imgMID.Image = CType(resources.GetObject("imgMID.Image"),System.Drawing.Image)
      Me.imgMID.InitialImage = CType(resources.GetObject("imgMID.InitialImage"),System.Drawing.Image)
      Me.imgMID.Location = New System.Drawing.Point(347, 12)
      Me.imgMID.Name = "imgMID"
      Me.imgMID.Size = New System.Drawing.Size(115, 80)
      Me.imgMID.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
      Me.imgMID.TabIndex = 19
      Me.imgMID.TabStop = false
      '
      'btnRun
      '
      Me.btnRun.DialogResult = System.Windows.Forms.DialogResult.OK
      Me.btnRun.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
      Me.btnRun.Location = New System.Drawing.Point(18, 115)
      Me.btnRun.Name = "btnRun"
      Me.btnRun.Size = New System.Drawing.Size(286, 30)
      Me.btnRun.TabIndex = 20
      Me.btnRun.Text = "Run"
      Me.btnRun.UseVisualStyleBackColor = true
      '
      'btnCanc
      '
      Me.btnCanc.DialogResult = System.Windows.Forms.DialogResult.Cancel
      Me.btnCanc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
      Me.btnCanc.Location = New System.Drawing.Point(385, 119)
      Me.btnCanc.Name = "btnCanc"
      Me.btnCanc.Size = New System.Drawing.Size(40, 23)
      Me.btnCanc.TabIndex = 21
      Me.btnCanc.Text = "Exit"
      Me.btnCanc.UseVisualStyleBackColor = true
      '
      'dlg_map_routing
      '
      Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
      Me.ClientSize = New System.Drawing.Size(476, 166)
      Me.Controls.Add(Me.btnCanc)
      Me.Controls.Add(Me.btnRun)
      Me.Controls.Add(Me.imgMID)
      Me.Controls.Add(Me.lblHeadline2)
      Me.Controls.Add(Me.lblHeadline)
      Me.Name = "dlg_map_routing"
      Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
      Me.Text = "MID-Layout"
      CType(Me.imgMID,System.ComponentModel.ISupportInitialize).EndInit
      Me.ResumeLayout(false)
      Me.PerformLayout

End Sub
    Friend WithEvents lblHeadline As System.Windows.Forms.Label
    Friend WithEvents lblHeadline2 As System.Windows.Forms.Label
    Friend WithEvents imgMID As System.Windows.Forms.PictureBox
    Friend WithEvents btnRun As System.Windows.Forms.Button
    Friend WithEvents btnCanc As System.Windows.Forms.Button
End Class
