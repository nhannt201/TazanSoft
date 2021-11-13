Imports System
Imports System.IO
Public Class Form2
    Dim homnay As Date = Date.Today

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            FileSystem.Unlock(fr1.OpenFileDialog1.FileName)
            FileSystem.Kill(fr1.OpenFileDialog1.FileName)
            File.Delete(fr1.OpenFileDialog1.FileName)
            File.Delete(TextBox1.Text)
            fr1.ListBox2.Items.Add(TextBox1.Text)
            fr1.ListBox6.Items.Add("Đóng phát hiện virus:" + TimeOfDay + " ngày " + homnay + " tập tin " + TextBox1.Text)
            Me.Close()
        Catch
            MsgBox("Có lỗi xảy ra không thể đóng cửa sổ !", vbInformation, "TazanSoft")
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            fr1.ListBox6.Items.Add("Xóa virus phát hiện:" + TimeOfDay + " ngày " + homnay + " tập tin " + TextBox1.Text)
            Kill(TextBox1.Text)
            MsgBox("Mối đe dọa đã được loại bỏ!", MsgBoxStyle.Information)
        Catch ex As Exception
            MsgBox("Có lỗi xảy ra ,chương trình không thể loại bỏ virus này !", MsgBoxStyle.Information)
            fr1.ListBox6.Items.Add("Không thể xóa virus: " + TimeOfDay + " ngày " + homnay + " tập tin " + TextBox1.Text)
        End Try
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        TextBox1.Text = fr1.OpenFileDialog1.FileName
        Try
            FileSystem.Unlock(fr1.OpenFileDialog1.FileName)
            FileSystem.Kill(fr1.OpenFileDialog1.FileName)
            File.Delete(fr1.OpenFileDialog1.FileName)
            File.Delete(TextBox1.Text)
            Timer1.Enabled = False

        Catch ex As Exception
            Timer1.Enabled = False
        End Try
    End Sub
End Class