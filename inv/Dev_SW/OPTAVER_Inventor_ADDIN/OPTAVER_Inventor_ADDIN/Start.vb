Imports GraphPlotter
Imports Projekt_Leiterbahn
Imports System.Windows.Forms

'#################################### Main Modul ####################################
Module Start
    Sub Main()



        '#################################### GraphPlotter-Fenster ####################################
        'Try
        ' Dim gp As New GraphPlotter.Main
        'gp.ShowDialog()
        'Catch ex As Exception
        'MsgBox("Error while creating GraphPlotter-Window")
        'End Try
        '#################################### GraphPlotter-Fenster ####################################



        '#################################### BRep-Fenster ####################################
        'Try
        'Dim rbrep As New frm_read_brep_from_cad_ai
        'rbrep.ShowDialog()
        'Catch ex As Exception
        'MsgBox("Error while creating Brep-Window")
        'End Try

        '#################################### BRep-Fenster ####################################



        '#################################### Main-Fenster ####################################
        Try
            Dim frm_main As New frm_main
            frm_main.ShowDialog()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        '#################################### Main-Fenster ####################################



    End Sub
End Module
'#################################### Main Modul ####################################
