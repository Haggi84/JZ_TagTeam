'footprint 1.2
' ******
' * Ax *
' ******

' Auxilliary Functions, Subs and Variables
' ========================================

Option Explicit On
Imports System.Math
Imports System.Windows.Forms
Imports System.IO

Module Ax

   Private Declare Function apiLoadCursor Lib "User32" Alias "LoadCursorA" (ByVal hInstance As Long, ByVal lpCursorName As Long) As Long
   Private Declare Sub apiSetCursor Lib "User32" Alias "SetCursor" (ByVal hCursor As Long)

   Public level As Integer         ' Einzugslevel
   Public Indention() As String    ' Einzüge

   ' ************
   ' * mkAttrib *
   ' ************

   Public Function mkAttrib(ByVal name As String, ByVal value As String) As String
      Return (" " & name & "=""" & value & """")
   End Function

   ' ***********
   ' * openTag *
   ' ***********

  Public Sub openTag(ByVal name As String, Optional ByVal attributes As String = "")
      Dim Line As String
      If (String.IsNullOrEmpty(attributes)) Then
         Line = Indention(level) & "<" & name & ">"
      Else
         Line = Indention(level) & "<" & name & attributes & ">"
      End If

      outFile.WriteLine(Line)
      incrt(level)
   End Sub

   ' ************
   ' * closeTag *
   ' ************

   Public Sub closeTag(ByVal name As String)
      decrt(level)
      Dim Line As String
      Line = Indention(level) & "</" & name & ">"
      outFile.WriteLine(Line)
   End Sub

   ' ************
   ' * simpleTag *
   ' ************

   Public Sub simpleTag(ByVal name As String, Optional ByVal attributes As String = "")
      Dim Line As String
      If (String.IsNullOrEmpty(attributes)) Then
         Line = Indention(level) & "<" & name & "/>"
      Else
         Line = Indention(level) & "<" & name & attributes & "/>"
      End If

      outFile.WriteLine(Line)
   End Sub

   ' **********************************
   ' * startDialogOfReadCircuitFromXML *
   ' **********************************

    'Public Function startDialogOfReadCircuitFromXML() As Boolean
    '   Dim dlg As dlgStartReadCircuitFromXML
    '   dlg = New dlgStartReadCircuitFromXML
    '   Dim result As DialogResult
    '   result = dlg.ShowDialog()

    '   If result = Windows.Forms.DialogResult.Cancel Then
    '      MidMsgBoxCanceled("ReadCircuitFromXML")
    '      dlg.Close()
    '      return False
    '   Else
    '      return True
    '   End If
    'End Function

   ' *****************
   ' * fillIndention *
   ' *****************

   ' Texteinschübe instanziieren

   Public Sub fillIndention()
      Dim n As Integer
      n = 10
      ReDim Indention(n)
      Indention(0) = ""
      Indention(1) = "   "
      Dim i As Integer
      For i = 2 To (n - 1)
         Indention(i) = Indention(i - 1) & Indention(1)
      Next i
   End Sub

   ' ********
   ' * dToS *
   ' ********

   ' Double To String

   Public Function dToS(ByVal r As Double) As String
      Dim s As String
      If (r = 0) Then
         s = "0"
      Else
         s = Format(r, formatOfCoordinateValues)
         If (decimalPointSign = ".") Then
            s = Replace(s, ",", ".", 1, 1)
         Else
            s = Replace(s, ".", ",", 1, 1)
         End If
      End If
      Return s
   End Function


   ' *******************
   ' * cutOffExtension *
   ' *******************

   ' Von einem Dateinamen bzw. Pfad die Namenserweiterung (z.B. ".xsd") abschneiden

   Public Sub cutOffExtension(ByRef name As String)
      Dim p() As String
      p = Split(name, ".")
      name = p(0)
   End Sub

   ' *******************************
   ' * extractNameFromFullFileName *
   ' *******************************

   ' Extrahiert den eigentlichen Namen aus dem vollen Pfadnamen heraus, indem der 
   ' Verzeichnisspfad und die Extension abgeschnitten werden

   Public Sub extractNameFromFullFileName(ByRef name As String)
      Dim startOfName  As Integer = InStrRev(GlobVar.full_FileName_BRep, "/") + 1
      Dim posPoint     As Integer = InStrRev(GlobVar.full_FileName_BRep, ".")
      Dim lengthOfName As Integer = posPoint - startOfName
      name = Mid(GlobVar.full_FileName_BRep, startOfName, lengthOfName)
   End Sub

   ' ********
   ' * aToS *
   ' ********

   ' Angle To String

   Public Function aToS(ByVal a As Double) As String
      Dim s As String
      If (a = 0) Then
         s = "0"
      Else
         s = Format(a, formatOfAngleValues)
         If (decimalPointSign = ".") Then
            s = Replace(s, ",", ".", 1, 1)
         Else
            s = Replace(s, ".", ",", 1, 1)
         End If
      End If
      Return s
   End Function

   ' ********
   ' * bToS *
   ' ********

   ' Boolean To String

   Public Function bToS(ByVal v As Boolean) As String
      If (v) Then
         Return "true"
      Else
         Return "false"
      End If
   End Function

   ' **************
   ' * point3dToS *
   ' **************

   ' 3D-Punkt zu String

   Public Function point3dToS(ByRef point() As Double) As String
      Return mkAttrib("x", dToS(point(0))) & mkAttrib("y", dToS(point(1))) & mkAttrib("z", dToS(point(2)))
   End Function

   ' ****************
   ' * arrToPoint3d *
   ' ****************

   ' Aus einem großen Koordinaten-Array ab Index i einen 3D-Point auslesen

   ' In einem Koordinaten-Array sind fortlaufend die x-, y- und z-Koordinaten von 3D-Punkten
   ' abgespeichert. Der Index i zeigt auf die x-Koordinate eines Punktes P.
   ' Die Routine nimmt die 3 Koordinatenwerte von P und schreibt sie in ein einen 3D-Punkt.
   ' Danach wird der Index i auf die x-Koordinate des nächsten Punktes weitergestellt.

   Public Sub arrToPoint3d(ByRef a() As Double, ByRef i As Integer, ByRef arr() As Double)
      arr(0) = a(i)
      arr(1) = a(i + 1)
      arr(2) = a(i + 2)
      i = i + 3
   End Sub


   ' *********
   ' * incrt *
   ' *********

   ' Increment Integer

   Public Sub incrt(ByRef n As Integer)
      n = n + 1
   End Sub

   ' *********
   ' * decrt *
   ' *********

   ' Decrement Integer

   Public Sub decrt(ByRef n As Integer)
      n = n - 1
   End Sub

   ' ***************************
   ' * switchToHourGlassCursor *
   ' ***************************

   Public Sub switchToHourGlassCursor()
      Dim hCursor As Long
      hCursor = apiLoadCursor(0, 32514&)
      apiSetCursor(hCursor)
   End Sub

   ' ************************
   ' * switchToNormalCursor *
   ' ************************

   Public Sub switchToNormalCursor()
      Dim hCursor As Long
      hCursor = apiLoadCursor(0, 32512&)
      apiSetCursor(hCursor)
   End Sub

   ' *****************
   ' * vectorProduct *
   ' *****************

   ' Bestimmung des Vektorproduktes c = a x b im 3D

   Public Sub vectorProduct(ByRef a() As Double, ByRef b() As Double, ByRef c() As Double)
      ReDim c(2)
      c(0) = a(1) * b(2) - a(2) * b(1)
      c(1) = a(2) * b(0) - a(0) * b(2)
      c(2) = a(0) * b(1) - a(1) * b(0)
   End Sub

   ' *************
   ' * vectorSum *
   ' *************

   ' Bestimmung der Vektorsumme c = a + b im 3D

   Public Sub vectorSum(ByRef a() As Double, ByRef b() As Double, ByRef c() As Double)
      c(0) = a(0) + b(0)
      c(1) = a(1) + b(1)
      c(2) = a(2) + b(2)
   End Sub

   ' *******************
   ' * euclideanLength *
   ' *******************

   ' Euklidische Norm (Länge) eines 3D-Vektors

   Public Function euclideanLength(ByRef a() As Double) As Double
      Dim qNorm As Double
      qNorm = a(0) * a(0) + a(1) * a(1) + a(2) * a(2)
      Return Sqrt(qNorm)
   End Function

   ' *************
   ' * normalize *
   ' *************

   ' Normalisierung eines 3D-Vektors

   Public Sub normalize(ByRef a() As Double)
      Dim norm As Double
      norm = euclideanLength(a)
      a(0) = a(0) / norm
      a(1) = a(1) / norm
      a(2) = a(2) / norm
   End Sub

   ' ***********************
   ' * scalarVectorProduct *
   ' ***********************

   ' Multiplikation eines 3D-Vektors mit einem skalaren Faktor: w=f*v

   Public Sub scalarVectorProduct(ByRef f As Double, ByRef v() As Double, ByRef w() As Double)
      w(0) = f * v(0)
      w(1) = f * v(1)
      w(2) = f * v(2)
   End Sub

   ' ***********************
   ' * scalarVectorDivision *
   ' ***********************

   ' Division eines 3D-Vektors durch einen skalaren divisor: w=v/d

   Public Sub scalarVectorDivision(ByRef v() As Double, ByRef d As Double, ByRef w() As Double)
      w(0) = v(0) / d
      w(1) = v(1) / d
      w(2) = v(2) / d
   End Sub

   ' ***************
   ' * minusVector *
   ' ***************

   ' Richtung eines 3D-Vektors umkehren

   Public Sub minusVector(ByRef v() As Double)
      v(0) = -v(0)
      v(1) = -v(1)
      v(2) = -v(2)
   End Sub

   ' ***********
   ' * detApex *
   ' ***********

   ' Bestimmung der Spitze eines Kegels

   Public Sub detApex(ByRef halfAngle As Double, ByRef radius As Double, ByRef axisVector() As Double, ByRef basePoint() As Double, ByRef Apex() As Double)
      Dim t As Double   ' Für Tangens
      Dim alpha As Double
      Dim PI As Double
      PI = 3.14159265358979
      alpha = (halfAngle * PI) / 180
      t = Math.Tan(alpha)
      Dim h As Double
      h = radius / t
      Dim p(3) As Double
      scalarVectorProduct(h, axisVector, p)
      vectorSum(basePoint, p, Apex)
   End Sub

   ' **************
   ' * convLength *
   ' **************

   ' Konvertierung einer Länge in die gewünschte Masseinheit

   Public Function convLength(ByVal x As Double) As Double
      If (unitOfLength = "cm") Then
         Return x
      ElseIf (unitOfLength = "mm") Then
         Return (10 * x)
      ElseIf (unitOfLength = "m") Then
         Return (x / 100)
      Else
         MidMsgBoxProblem("ReadBRepFromPM: unknown unitOfLength")
         Return 0
      End If
   End Function

   ' *************
   ' * convAngle *
   ' ************+

   ' Konvertierung eines Winkels in die gewünschte Masseinheit

   Public Function convAngle(ByVal x As Double) As Double
      If (unitOfAngles = "Radian") Then
         Return x
      Else
         Return ((x / PI) * 180)
      End If
   End Function

   ' ***************
   ' * convPoint3d *
   ' ***************

   ' Konvertierung eines 3D-Punktes in die gewünschte Masseinheit

   Public Sub convPoint3d(ByRef p() As Double)
      p(0) = convLength(p(0))
      p(1) = convLength(p(1))
      p(2) = convLength(p(2))
   End Sub


End Module
