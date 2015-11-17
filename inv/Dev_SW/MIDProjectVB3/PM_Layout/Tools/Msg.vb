'footprint 1.2
Option Explicit On

Module Msg

   Public Function MidMsgBoxQuestion(ByVal questionText As String) As Boolean
      Dim title As String = "MID-Layout"
      Dim style As MsgBoxStyle
      style = MsgBoxStyle.DefaultButton1 Or MsgBoxStyle.Critical Or MsgBoxStyle.YesNo
      Dim response As MsgBoxResult = MsgBox(questionText, style, title)
      If response = 6 Then
         Return True
      End If
      Return False
   End Function

   Public Sub MidMsgBoxInformation(ByVal text As String)
      Dim title As String = "MID-Layout"
      Dim style As MsgBoxStyle
      style = MsgBoxStyle.Information
      MsgBox(text, style, title)
   End Sub

   Public Sub MidMsgBoxCanceled(ByVal name As String)
      Dim title As String = "MID-Layout"
      Dim style As MsgBoxStyle
      style = MsgBoxStyle.Information
      MsgBox(name + " finished by user", style, title)
   End Sub

   Public Sub MidMsgBoxFinale(ByVal name As String)
      Dim title As String = "MID-Layout"
      Dim style As MsgBoxStyle
      style = MsgBoxStyle.Information
      MsgBox(name + " finishes", style, title)
   End Sub

   Public Sub MidMsgBoxProblem(ByVal text As String)
      Dim title As String      = "MID-Layout: Problem"
      Dim style As MsgBoxStyle = MsgBoxStyle.Critical
      MsgBox(text, style, title)
   End Sub

   Public Sub MidMsgBoxWarning(ByVal text As String)
      Dim title As String      = "MID-Layout: Warning"
      Dim style As MsgBoxStyle = MsgBoxStyle.Information
      MsgBox(text, style, title)
   End Sub

End Module