Imports System.Globalization
Public Class Form1

    ' State variables
    Private firstNumber As Double = 0
    Private currentOp As String = ""
    Private isNewEntry As Boolean = True   ' true jika textbox siap diisi angka baru (misal setelah tekan operator)

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtDisplay.Text = "0"
    End Sub

    ' --- Digit (0-9) dan titik handler ---
    Private Sub btnDigit_Click(sender As Object, e As EventArgs) Handles btn0.Click, btn1.Click, btn2.Click, btn3.Click, btn4.Click, btn5.Click, btn6.Click, btn7.Click, btn8.Click, btn9.Click
        Dim btn As Button = DirectCast(sender, Button)
        Dim digit As String = btn.Text

        If isNewEntry OrElse txtDisplay.Text = "0" Then
            txtDisplay.Text = digit
            isNewEntry = False
        Else
            txtDisplay.Text &= digit
        End If
    End Sub

    Private Sub btnDot_Click(sender As Object, e As EventArgs) Handles btnDot.Click
        Dim decSep As String = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator

        If isNewEntry Then
            txtDisplay.Text = "0" & decSep
            isNewEntry = False
            Return
        End If

        ' Jangan tambahkan jika sudah ada separator
        If Not txtDisplay.Text.Contains(decSep) Then
            txtDisplay.Text &= decSep
        End If
    End Sub


    Private Sub btnOperator_Click(sender As Object, e As EventArgs) Handles btnPlus.Click, btnMinus.Click, btnMul.Click, btnDiv.Click
        Dim btn As Button = DirectCast(sender, Button)
        Double.TryParse(txtDisplay.Text, firstNumber)

        Select Case btn.Text
            Case "+"
                currentOp = "+"
            Case "-"
                currentOp = "-"
            Case "x"
                currentOp = "*"
            Case "÷"
                currentOp = "/"
        End Select

        isNewEntry = True
    End Sub


    ' --- Equal handler ---
    Private Sub btnEqual_Click(sender As Object, e As EventArgs) Handles btnEqual.Click
        Dim secondNumber As Double = 0
        If Not Double.TryParse(txtDisplay.Text, secondNumber) Then
            txtDisplay.Text = "0"
            Return
        End If

        Dim result As Double = 0
        Try
            Select Case currentOp
                Case "+"
                    result = firstNumber + secondNumber
                Case "-"
                    result = firstNumber - secondNumber
                Case "*"
                    result = firstNumber * secondNumber
                Case "/"
                    If secondNumber = 0 Then
                        MessageBox.Show("Error: Pembagian dengan nol", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        txtDisplay.Text = "0"
                        currentOp = ""
                        isNewEntry = True
                        Return
                    End If
                    result = firstNumber / secondNumber
                Case Else
                    ' Jika tidak ada operator, biarkan apa adanya
                    result = secondNumber
            End Select
        Catch ex As Exception
            MessageBox.Show("Kesalahan: " & ex.Message)
            txtDisplay.Text = "0"
            currentOp = ""
            isNewEntry = True
            Return
        End Try

        ' Tampilkan hasil (hilangkan .0 jika integer)
        If Math.Truncate(result) = result Then
            txtDisplay.Text = result.ToString("F0")
        Else
            txtDisplay.Text = result.ToString()
        End If

        ' Reset agar user bisa melanjutkan
        firstNumber = result
        currentOp = ""
        isNewEntry = True
    End Sub

    ' --- Clear (C) ---
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        txtDisplay.Text = "0"
        firstNumber = 0
        currentOp = ""
        isNewEntry = True
    End Sub

    ' --- Backspace (hapus 1 digit) ---
    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        If isNewEntry Then
            ' jika di state baru, back = 0
            txtDisplay.Text = "0"
            isNewEntry = True
            Return
        End If

        If txtDisplay.Text.Length > 1 Then
            txtDisplay.Text = txtDisplay.Text.Substring(0, txtDisplay.Text.Length - 1)
            ' jika hasilnya hanya "-" atau kosong, set ke 0
            If txtDisplay.Text = "-" Or txtDisplay.Text = "" Then
                txtDisplay.Text = "0"
                isNewEntry = True
            End If
        Else
            txtDisplay.Text = "0"
            isNewEntry = True
        End If
    End Sub

    ' --- Negate (+/-) ---
    Private Sub btnNeg_Click(sender As Object, e As EventArgs) Handles btnNeg.Click
        If txtDisplay.Text = "0" OrElse txtDisplay.Text = "" Then
            Return
        End If

        If txtDisplay.Text.StartsWith("-") Then
            txtDisplay.Text = txtDisplay.Text.Substring(1)
        Else
            txtDisplay.Text = "-" & txtDisplay.Text
        End If
    End Sub

    ' (Opsional) support keyboard input
    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        Select Case keyData
            Case Keys.D0, Keys.NumPad0
                btn0.PerformClick()
            Case Keys.D1, Keys.NumPad1
                btn1.PerformClick()
            Case Keys.D2, Keys.NumPad2
                btn2.PerformClick()
            Case Keys.D3, Keys.NumPad3
                btn3.PerformClick()
            Case Keys.D4, Keys.NumPad4
                btn4.PerformClick()
            Case Keys.D5, Keys.NumPad5
                btn5.PerformClick()
            Case Keys.D6, Keys.NumPad6
                btn6.PerformClick()
            Case Keys.D7, Keys.NumPad7
                btn7.PerformClick()
            Case Keys.D8, Keys.NumPad8
                btn8.PerformClick()
            Case Keys.D9, Keys.NumPad9
                btn9.PerformClick()
            Case Keys.OemPeriod, Keys.Decimal
                btnDot.PerformClick()
            Case Keys.Add
                btnPlus.PerformClick()
            Case Keys.Subtract
                btnMinus.PerformClick()
            Case Keys.Multiply
                btnMul.PerformClick()
            Case Keys.Divide
                btnDiv.PerformClick()
            Case Keys.Enter
                btnEqual.PerformClick()
            Case Keys.Back
                btnBack.PerformClick()
        End Select

        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function

End Class