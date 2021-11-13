Imports System.Net

Public Class update2

  

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Dim client As WebClient = New WebClient
        AddHandler client.DownloadProgressChanged, AddressOf client_ProgressChanged

        AddHandler client.DownloadFileCompleted, AddressOf client_DownloadCompleted

        client.DownloadFileAsync(New Uri("http://kho.luutru360.com/tazansoft/viruslist.pak"), Application.StartupPath & "\viruslist.pak")

        Timer1.Enabled = False
    End Sub

    Private Sub client_ProgressChanged(ByVal sender As Object, ByVal e As DownloadProgressChangedEventArgs)

        Dim bytesIn As Double = Double.Parse(e.BytesReceived.ToString())

        Dim totalBytes As Double = Double.Parse(e.TotalBytesToReceive.ToString())

        Dim percentage As Double = bytesIn / totalBytes * 100



        ProgressBar.Value = Int32.Parse(Math.Truncate(percentage).ToString())

    End Sub

    Private Sub client_DownloadCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs)
      
        ' đếm giây mở soft


        MessageBox.Show("Đã cập nhật xong chương trình sẽ thoát , vui lòng khởi động lại chương trình !")
        End
    End Sub

   
    Private Sub update2_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class