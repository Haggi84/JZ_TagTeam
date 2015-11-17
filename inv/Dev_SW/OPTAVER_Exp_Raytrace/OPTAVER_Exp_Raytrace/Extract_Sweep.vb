Imports System
Imports Inventor
Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Windows.Forms





Public Class Extract_Sweep
    Dim mApp As Inventor.Application = Marshal.GetActiveObject("Inventor.Application")
    Dim oDoc As Inventor.PartDocument = mApp.ActiveDocument
    Dim oCompDef As Inventor.ComponentDefinition = oDoc.ComponentDefinition

    'extrudierte körper werden entfernt
    Public Sub delete_extrudes()
        Dim exFeat_sum As ExtrudeFeatures = oDoc.ComponentDefinition.Features.ExtrudeFeatures
        Dim exfeat_single As ExtrudeFeature
        Try
            For i = 1 To exFeat_sum.Count
                exfeat_single = exFeat_sum(i)
                exfeat_single.Delete()
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Public Sub extract_geometry()
        Dim sw_feat_sum As SweepFeatures = oDoc.ComponentDefinition.Features.SweepFeatures
        Dim sw_feat As SweepFeature = sw_feat_sum.Item(1)


    End Sub

    Public Sub file_write_head(fn As String)
        Try
            Dim day As String = Now.Day & "/" & Now.Month & "/" & Now.Year & ", "
            Dim time As String = Now.TimeOfDay.Hours & ":" & Now.TimeOfDay.Minutes & ":" & Now.TimeOfDay.Seconds
            Dim dat As String = day & time

            ' MsgBox(dat)
            ' Dim fd As OpenFileDialog = New OpenFileDialog()
            'Dim strFileName As String

            'fd.Title = "Open File Dialog"
            'fd.InitialDirectory = "C:\"
            'fd.Filter = "All files (*.*)|*.*|All files (*.*)|*.*"
            'fd.FilterIndex = 2
            'fd.RestoreDirectory = True

            Dim writeFile As System.IO.TextWriter = New StreamWriter(fn)
            writeFile.WriteLine("RAYTRACE 0.9 (17) generated text file" & vbNewLine & dat & vbNewLine & "!AUFBAU" & vbNewLine & "N_ELEMENTS	5" & vbNewLine & "MATERIAL_CATALOGUE	2" & vbNewLine & "{" & vbNewLine & "[" & vbNewLine & "NAME	BK7" & vbNewLine & "SCHOTT_1	2.2718929	SCHOTT_2	-0.010108077	SCHOTT_3	0.010592509	" & vbNewLine & "SCHOTT_4	0.00020816965	SCHOTT_5	-7.6472538e-006	SCHOTT_6	4.9240991e-007	" & vbNewLine & "]" & vbNewLine & "" & vbNewLine & "[" & vbNewLine & "NAME	Air" & vbNewLine & "SCHOTT_1	1	SCHOTT_2	0	SCHOTT_3	0	" & vbNewLine & "SCHOTT_4	0	SCHOTT_5	0	SCHOTT_6	0	" & vbNewLine & "]" & vbNewLine & "}" & vbNewLine & "" & vbNewLine & "SPOT_WINDOW" & vbNewLine & "{" & vbNewLine & "THETA	0	PHI	0	ALPHA	0" & vbNewLine & "POS_X	0	POS_Y	0	POS_Z	50" & vbNewLine & "DIAMETER	150" & vbNewLine & "}" & vbNewLine & "" & vbNewLine & "" & vbNewLine & "SPOT_WINDOW_2" & vbNewLine & "{" & vbNewLine & "THETA	0	PHI	0	ALPHA	0" & vbNewLine & "POS_X	0	POS_Y	0	POS_Z	0" & vbNewLine & "DIAMETER	2" & vbNewLine & "}" & vbNewLine & "" & vbNewLine & "" & vbNewLine & "SPOT_WINDOW_3" & vbNewLine & "{" & vbNewLine & "THETA	0	PHI	0	ALPHA	0" & vbNewLine & "POS_X	0	POS_Y	0	POS_Z	0" & vbNewLine & "DIAMETER	2" & vbNewLine & "}" & vbNewLine & "" & vbNewLine & "" & vbNewLine & "SPOT_WINDOW_4" & vbNewLine & "{" & vbNewLine & "THETA	0	PHI	0	ALPHA	0" & vbNewLine & "POS_X	0	POS_Y	0	POS_Z	0" & vbNewLine & "DIAMETER	2" & vbNewLine & "}" & vbNewLine & "" & vbNewLine & "ZX_VIEW" & vbNewLine & "{" & vbNewLine & "C_HORIZONTAL	0	C_VERTICAL	0" & vbNewLine & "D_HORIZONTAL	128	D_VERTICAL	96" & vbNewLine & "}" & vbNewLine & "" & vbNewLine & "ZY_VIEW" & vbNewLine & "{" & vbNewLine & "C_HORIZONTAL	109.32295719844	C_VERTICAL	-32.08421052632" & vbNewLine & "D_HORIZONTAL	390.4747081712	D_VERTICAL	363.78947368422" & vbNewLine & "}" & vbNewLine & "" & vbNewLine & "XY_VIEW" & vbNewLine & "{" & vbNewLine & "C_HORIZONTAL	-0.794262744157	C_VERTICAL	0.589349437159" & vbNewLine & "D_HORIZONTAL	168.1461049207	D_VERTICAL	151.40261280991" & vbNewLine & "}" & vbNewLine & "" & vbNewLine & "CP_VIEW" & vbNewLine & "{" & vbNewLine & "PX	-457.496	PY	457.496	PZ	762.493" & vbNewLine & "TX	0	TY	0	TZ	0" & vbNewLine & "THETA	139.684	PHI	315	ALPHA	320" & vbNewLine & "BETA	8	DXYZ	1	DANGLE	1	DBETA	1" & vbNewLine & "}" & vbNewLine & "" & vbNewLine & "WAVEPROP_Z	-10000000" & vbNewLine & "" & vbNewLine & "_______________________________ELEMENT_0" & vbNewLine & "!!LIGHTSOURCE" & vbNewLine & "MATERIAL_L	Air" & vbNewLine & "MATERIAL_R	Air" & vbNewLine & "N_POINTS	1	N_RAYS	1024" & vbNewLine & "SELECTED_EP	0" & vbNewLine & "_______________" & vbNewLine & "!POINT	1" & vbNewLine & "LAMBDA	0.000488" & vbNewLine & "RECTANGULAR	REAL" & vbNewLine & "POS_X	0	POS_Y	0	POS_Z	-75" & vbNewLine & "POS_EP_X	0	POS_EP_Y	0	POS_EP_Z	0" & vbNewLine & "THETA_EP	0	PHI_EP	0	ALPHA_EP	0" & vbNewLine & "RADIUS_EP	10	XWAIST_EP	10	WAISTRATIO_XY_EP	1" & vbNewLine & "_______________" & vbNewLine & "!SHARED_EP" & vbNewLine & "ELLIPTIC" & vbNewLine & "POS_EP_X	0	POS_EP_Y	0	POS_EP_Z	-50" & vbNewLine & "THETA_EP	0	PHI_EP	0	ALPHA_EP	0" & vbNewLine & "RADIUS_EP	10	XWAIST_EP	10	WAISTRATIO_XY_EP	1" & vbNewLine & "" & vbNewLine & "_______________________________ELEMENT_1" & vbNewLine & "!!SINGLE" & vbNewLine & "MATERIAL_L	Air" & vbNewLine & "MATERIAL_R	BK7" & vbNewLine & "STRAY_REFLECTION	0.000000" & vbNewLine & "STRAY_TRANSMISSION	0.000000" & vbNewLine & "CIRCLE	REFRACTIV	NOARRAY" & vbNewLine & "POS_X	0	POS_Y	-25	POS_Z	-50" & vbNewLine & "THETA	0	PHI	0	ALPHA	0" & vbNewLine & "DIAMETER	100" & vbNewLine & "" & vbNewLine & "COATING" & vbNewLine & "{" & vbNewLine & "N	300	SIDE	LEFT" & vbNewLine & "N_A	1.800000	N_B	1.000000" & vbNewLine & "D_A	100.000000	D_B	0.000000" & vbNewLine & "}" & vbNewLine & "" & vbNewLine & "" & vbNewLine & "_______________________________ELEMENT_2" & vbNewLine & "!!SINGLE" & vbNewLine & "MATERIAL_L	BK7" & vbNewLine & "MATERIAL_R	Air" & vbNewLine & "STRAY_REFLECTION	0.000000" & vbNewLine & "STRAY_TRANSMISSION	0.000000" & vbNewLine & "CIRCLE	REFRACTIV	NOARRAY" & vbNewLine & "POS_X	0	POS_Y	-25	POS_Z	200" & vbNewLine & "THETA	0	PHI	0	ALPHA	0" & vbNewLine & "DIAMETER	100" & vbNewLine & "" & vbNewLine & "COATING" & vbNewLine & "{" & vbNewLine & "N	300	SIDE	LEFT" & vbNewLine & "N_A	1.800000	N_B	1.000000" & vbNewLine & "D_A	100.000000	D_B	0.000000" & vbNewLine & "}" & vbNewLine & "" & vbNewLine & "" & vbNewLine & "_______________________________ELEMENT_3" & vbNewLine & "!!SINGLE" & vbNewLine & "MATERIAL_L	Air" & vbNewLine & "MATERIAL_R	BK7" & vbNewLine & "STRAY_REFLECTION	0.000000" & vbNewLine & "STRAY_TRANSMISSION	0.000000" & vbNewLine & "PLANE	FRESNEL_TE	NOARRAY" & vbNewLine & "POS_X	0	POS_Y	-25	POS_Z	75" & vbNewLine & "THETA	90	PHI	90	ALPHA	90" & vbNewLine & "DIAMETER_X	100	DIAMETER_Y	250" & vbNewLine & "" & vbNewLine & "COATING" & vbNewLine & "{" & vbNewLine & "N	300	SIDE	LEFT" & vbNewLine & "N_A	1.800000	N_B	1.000000" & vbNewLine & "D_A	100.000000	D_B	0.000000" & vbNewLine & "}" & vbNewLine & "" & vbNewLine & "" & vbNewLine & "_______________________________ELEMENT_4" & vbNewLine & "!!SINGLE" & vbNewLine & "MATERIAL_L	BK7" & vbNewLine & "MATERIAL_R	Air" & vbNewLine & "STRAY_REFLECTION	0.000000" & vbNewLine & "STRAY_TRANSMISSION	0.000000" & vbNewLine & "PARSER	FRESNEL_TE	NOARRAY" & vbNewLine & "HOLE_X	0	HOLE_Y	0" & vbNewLine & "POS_X	0	POS_Y	-25	POS_Z	75" & vbNewLine & "THETA	90	PHI	90	ALPHA	90" & vbNewLine & "DIAMETER_X	100	DIAMETER_Y	250	RECT_BASE	" & vbNewLine & "" & vbNewLine & "PARSERFUNC" & vbNewLine & "{" & vbNewLine & "V_0	" + Chr(34) + "" + Chr(34) + "" & vbNewLine & "V_1	" + Chr(34) + "                                                                          " + Chr(34) + "" & vbNewLine & "V_2	" + Chr(34) + "                                                                          " + Chr(34) + "" & vbNewLine & "V_3	" + Chr(34) + "                                                                          " + Chr(34) + "" & vbNewLine & "V_4	" + Chr(34) + "                                                                          " + Chr(34) + "" & vbNewLine & "V_5	" + Chr(34) + "                                                                          " + Chr(34) + "" & vbNewLine & "V_6	" + Chr(34) + "                                                                          " + Chr(34) + "" & vbNewLine & "V_7	" + Chr(34) + "                                                                          " + Chr(34) + "" & vbNewLine & "V_8	" + Chr(34) + "                                                                          " + Chr(34) + "" & vbNewLine & "V_9	" + Chr(34) + "                                                                          " + Chr(34) + "" & vbNewLine & "FUNCTION" & vbNewLine & "50*exp(-x*x/500)" & vbNewLine & "}" & vbNewLine & "" & vbNewLine & "COATING" & vbNewLine & "{" & vbNewLine & "N	300	SIDE	LEFT" & vbNewLine & "N_A	1.800000	N_B	1.000000" & vbNewLine & "D_A	100.000000	D_B	0.000000" & vbNewLine & "}" & vbNewLine & "" & vbNewLine & "!END" & vbNewLine & "!CSM	45189" & vbNewLine & "")

            writeFile.Flush()
            writeFile.Close()
            writeFile = Nothing
        Catch ex As IOException
            MsgBox(ex.ToString)
        End Try
    End Sub
End Class
