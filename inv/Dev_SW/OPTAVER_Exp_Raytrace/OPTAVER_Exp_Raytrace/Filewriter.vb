Imports System.Runtime.InteropServices
Imports Inventor

Module Filewriter
    Sub Main()

        'hilfefunktion um ins Partdoc zu wechseln
        Dim App As Inventor.Application = Marshal.GetActiveObject("Inventor.Application")
      
        Dim Doc As Inventor.PartDocument = App.Documents.Item(App.Documents.Count - 1)

        Doc.Activate()
        'Dim fd As frm_save_path = New frm_save_path
        'Dim fn As String = fd.SaveFileDialog1.FileName
        'fd.SaveFileDialog1.ShowDialog()

        MsgBox("test")


        Dim Ex_sw As New Extract_Sweep
        Ex_sw.file_write_head("C:\Users\jozeitler\Desktop\test.atf")



    End Sub
End Module
