Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Security.Cryptography
Imports System.Text
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System.Windows.Forms
Imports System.Net.NetworkInformation
Imports System.Net
Imports Microsoft.Win32 'Assists in Registry Reading & Writing
Imports System.Management
Imports System.Security
Imports System.Security.AccessControl


Public Class fr1
    Dim cpu As Integer
    Dim ram As Integer
    Dim req As HttpWebRequest = WebRequest.Create("http://icanhazip.com/")
    Dim res As HttpWebResponse = req.GetResponse()
    Dim stream As Stream = res.GetResponseStream
    Dim sr As New StreamReader(stream)
    Dim W As IO.StreamWriter
    Dim strHostName As String
    Dim strIPAddress As String
    Dim homnay As Date = Date.Today

    Private Declare Function SHEmptyRecycleBin Lib "shell32.dll" Alias "SHEmptyRecycleBinA" (ByVal hWnd As Int32, ByVal pszRootPath As String, ByVal dwFlags As Int32) As Int32
    Private Declare Function SHUpdateRecycleBinIcon Lib "shell32.dll" () As Int32
    Private Const SHERB_NOCONFIRMATION = &H1
    Private Const SHERB_NOPROGRESSUI = &H2
    Private Const SHERB_NOSOUND = &H4
    Private strUninstallStrings() As String 'Array to hold Uninstall commands for each program

    Private NewUninstallStrArr 'Filtered array containing ONLY valid uninstall comman

    Private Property FSO As Object

    Declare Auto Function SendMessage Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    Enum ProgressBarColor
        Green = &H1
        Red = &H2
        Yellow = &H3
    End Enum
    Private Shared Sub ChangeProgBarColor(ByVal ProgressBar_Name As Windows.Forms.ProgressBar, ByVal ProgressBar_Color As ProgressBarColor)
        SendMessage(ProgressBar_Name.Handle, &H410, ProgressBar_Color, 0)
    End Sub


    Private Sub fr1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try


            ' khoi dong cung win
            My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True).SetValue(Application.ProductName, Application.ExecutablePath)
        Catch

        End Try
    End Sub



    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click
        Try
            Dim qwe As Integer
            W = New IO.StreamWriter(Application.StartupPath & "\nhatki.log")
            For qwe = 0 To ListBox6.Items.Count - 1
                W.WriteLine(ListBox6.Items.Item(qwe))
            Next
            W.Close()
            Me.Hide()
           
        Catch
        End Try
    End Sub


    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click
        Try
            Me.WindowState = FormWindowState.Minimized
            If Me.WindowState = FormWindowState.Minimized Then
                If Me IsNot Nothing Then
                    If Me.Tag Is "StorePage" Then
                        Me.WindowState = FormWindowState.Minimized
                    End If
                End If
            End If
        Catch
        End Try
        Try
            If Me.WindowState = FormWindowState.Maximized Then
                If Me IsNot Nothing Then
                    If Me.Tag Is "StorePage" Then
                        Me.WindowState = FormWindowState.Maximized
                    End If
                End If
            End If
        Catch
        End Try
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        FolderBrowserDialog1.ShowDialog()
        ListBox1.Items.Clear()
        ListBox2.Items.Clear()
        TabControl1.SelectTab(1)
        Try
            For Each strDir As String In System.IO.Directory.GetDirectories(FolderBrowserDialog1.SelectedPath)


                For Each strFile As String In System.IO.Directory.GetFiles(strDir)

                    ListBox1.Items.Add(strFile)

                Next

            Next
        Catch ex As Exception
        End Try

        Timer1.Start()





    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        FolderBrowserDialog1.SelectedPath = ("C:\")
        FolderBrowserDialog2.SelectedPath = ("D:\")
        TabControl1.SelectTab(1)
        '  Dim allDrives() As DriveInfo = DriveInfo.GetDrives()

        '   Dim d As DriveInfo
        '   For Each d In allDrives
        '  Console.WriteLine("Drive {0}", d.Name)

        '   Next
        Try
            For Each strDir As String In System.IO.Directory.GetDirectories(FolderBrowserDialog1.SelectedPath)


                For Each strFile As String In System.IO.Directory.GetFiles(strDir)

                    ListBox1.Items.Add(strFile)

                Next

            Next
        Catch ex As Exception
        End Try

        Timer1.Start()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>

    Private Sub FileSystemWatcher1_Changed(ByVal sender As System.Object, ByVal e As System.IO.FileSystemEventArgs) Handles FileSystemWatcher1.Changed

        Try
            labellastreal.Text = e.FullPath
            ListBox3.Items.Add(labellastreal.Text)
            ListBox4.Items.Add(labellastreal.Text)
            Me.OpenFileDialog1.FileName = ""
            Dim scanbox As New TextBox
            scanbox.Text = My.Computer.FileSystem.ReadAllText("viruslist.pak").ToString
            Dim md5 As New MD5CryptoServiceProvider
            Dim f As New FileStream(e.FullPath, FileMode.Open, FileAccess.Read, FileShare.Read, &H2000)
            f = New FileStream(e.FullPath, FileMode.Open, FileAccess.Read, FileShare.Read, &H2000)
            md5.ComputeHash(f)
            Dim hash As Byte() = md5.Hash
            Dim buff As New StringBuilder
            Dim hashByte As Byte
            For Each hashByte In hash
                buff.Append(String.Format("{0:X2}", hashByte))
            Next
            f.Close()
            If scanbox.Text.Contains(buff.ToString) Then
                Me.OpenFileDialog1.FileName = e.FullPath
                phathien()

            End If

        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            Dim ex As Exception = exception1
            ProjectData.ClearProjectError()
        End Try

    End Sub

    Sub phathien()

        Dim tile As String = 0
        tile = tile + 1
        TextBox3.Text = OpenFileDialog1.FileName
        Try
            FileSystem.Unlock(OpenFileDialog1.FileName)
            FileSystem.Kill(OpenFileDialog1.FileName)
            File.Delete(OpenFileDialog1.FileName)
            File.Delete(TextBox3.Text)
            ListBox2.Items.Add(TextBox3.Text)
            ListBox6.Items.Add("Phá hiện virus : " + TimeOfDay + " ngày " + homnay + " tập tin " + TextBox3.Text)
            vir.Visible = True
            vir.Text = "Phát hiện " + tile + " Virus , Click vào đây để diệt loại bỏ !"
            MsgBox("Phát hiện" + tile + "Virus !")
            Me.Show()
        Catch ex As Exception

        End Try

    End Sub
    Private Sub FileSystemWatcher1_Created(ByVal sender As System.Object, ByVal e As System.IO.FileSystemEventArgs) Handles FileSystemWatcher1.Created

        Try
            labellastreal.Text = e.FullPath
            ListBox3.Items.Add(labellastreal.Text)
            Me.OpenFileDialog1.FileName = ""
            Dim scanbox As New TextBox
            scanbox.Text = My.Computer.FileSystem.ReadAllText("viruslist.pak").ToString
            Dim md5 As New MD5CryptoServiceProvider
            Dim f As New FileStream(e.FullPath, FileMode.Open, FileAccess.Read, FileShare.Read, &H2000)
            f = New FileStream(e.FullPath, FileMode.Open, FileAccess.Read, FileShare.Read, &H2000)
            md5.ComputeHash(f)
            Dim hash As Byte() = md5.Hash
            Dim buff As New StringBuilder
            Dim hashByte As Byte
            For Each hashByte In hash
                buff.Append(String.Format("{0:X2}", hashByte))
            Next
            f.Close()
            If scanbox.Text.Contains(buff.ToString) Then
                Me.OpenFileDialog1.FileName = e.FullPath
                phathien()

            End If

        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            Dim ex As Exception = exception1
            ProjectData.ClearProjectError()
        End Try

    End Sub

    Private Sub FileSystemWatcher1_Renamed(ByVal sender As System.Object, ByVal e As System.IO.RenamedEventArgs) Handles FileSystemWatcher1.Renamed

        Try
            labellastreal.Text = e.FullPath
            ListBox3.Items.Add(labellastreal.Text)
            Me.OpenFileDialog1.FileName = ""
            Dim scanbox As New TextBox
            scanbox.Text = My.Computer.FileSystem.ReadAllText("viruslist.pak").ToString
            Dim md5 As New MD5CryptoServiceProvider
            Dim f As New FileStream(e.FullPath, FileMode.Open, FileAccess.Read, FileShare.Read, &H2000)
            f = New FileStream(e.FullPath, FileMode.Open, FileAccess.Read, FileShare.Read, &H2000)
            md5.ComputeHash(f)
            Dim hash As Byte() = md5.Hash
            Dim buff As New StringBuilder
            Dim hashByte As Byte
            For Each hashByte In hash
                buff.Append(String.Format("{0:X2}", hashByte))
            Next
            f.Close()
            If scanbox.Text.Contains(buff.ToString) Then
                Me.OpenFileDialog1.FileName = e.FullPath
                phathien()

            End If

        Catch exception1 As Exception
            ProjectData.SetProjectError(exception1)
            Dim ex As Exception = exception1
            ProjectData.ClearProjectError()
        End Try

    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Try
            FileSystemWatcher1.EnableRaisingEvents = True
            realtime.Text = "Kích hoạt bảo vệ thời gian thực"
            Label14.Text = realtime.Text
            Label14.ForeColor = Color.Green
        Catch

        End Try
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        Try
            FileSystemWatcher1.EnableRaisingEvents = False
            realtime.Text = "Vô hiệu hóa bảo vệ thời gian thực"
            Label14.Text = realtime.Text
            Label14.ForeColor = Color.Red
        Catch
        End Try
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click

        Try
            ListBox6.Items.Add("Xóa virus : " + TimeOfDay + " ngày " + homnay + " tập tin " + ListBox2.SelectedItem)
            Kill(ListBox2.SelectedItem)
            ListBox2.Items.Remove(ListBox2.SelectedItem)



            MsgBox("Mối đe dọa đã được loại bỏ!", MsgBoxStyle.Information)


        Catch ex As Exception

        End Try
    End Sub

    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        Try
            If Not ListBox2.Items.Count = 0 Then
                ListBox2.SelectedIndex += 1
                Kill(ListBox1.SelectedItem)
                ListBox2.Items.Remove(ListBox2.SelectedItem)
            Else
                Timer1.Stop()
                Timer2.Stop()

                MsgBox("Mối đe dọa đã được loại bỏ!", MsgBoxStyle.Information)

            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        ListBox6.Items.Add("Xóa hết virus : " + TimeOfDay + " ngày " + homnay + " tập tin " + ListBox1.SelectedItem)
        vir.Text = "Phát hiện 0 Virus , Click vào đây để diệt loại bỏ !"
        vir.Visible = False
        Timer2.Start()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        ProgressBar1.Maximum = Conversions.ToString(ListBox1.Items.Count)
        total.Text = Conversions.ToString(ListBox1.Items.Count)

        If Not ProgressBar1.Value = ProgressBar1.Maximum Then
            Try

                ListBox1.SelectedIndex = ListBox1.SelectedIndex + 1
                TextBox1.Text = ListBox1.SelectedItem.ToString

            Catch ex As Exception
            End Try



            Try

                Dim scanbox As New TextBox
                Dim read As String = My.Computer.FileSystem.ReadAllText("viruslist.pak")
                ProgressBar1.Increment(1)
                detected.Text = Conversions.ToString(ListBox2.Items.Count)
                files.Text = Conversions.ToString(ProgressBar1.Value)
                scanbox.Text = read.ToString
                Dim md5 As MD5CryptoServiceProvider = New MD5CryptoServiceProvider
                Dim f As FileStream = New FileStream(ListBox1.SelectedItem, FileMode.Open, FileAccess.Read, FileShare.Read, 8192)
                f = New FileStream(ListBox1.SelectedItem, FileMode.Open, FileAccess.Read, FileShare.Read, 8192)
                md5.ComputeHash(f)
                Dim hash As Byte() = md5.Hash
                Dim buff As StringBuilder = New StringBuilder
                Dim hashByte As Byte
                For Each hashByte In hash
                    buff.Append(String.Format("{0:X2}", hashByte))
                Next

                If scanbox.Text.Contains(buff.ToString) Then



                    ListBox2.Items.Add(ListBox1.SelectedItem)
                    ListBox6.Items.Add("Phá hiện virus : " + TimeOfDay + " ngày " + homnay + " tập tin " + ListBox1.SelectedItem)
                End If
            Catch ex As Exception
            End Try
        Else
            Timer1.Stop()
            MsgBox("Đã quét xong !")
            TabControl1.SelectTab(3)
            If ListBox1.Items.Count = 0 Then
                MsgBox("Không có mối đe dọa nào được phát hiện, quá trình quét virus hoàn thành!", MsgBoxStyle.Information)

            End If
        End If
    End Sub

    Private Sub ListBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox2.SelectedIndexChanged
        lk.Text = ListBox2.Text
    End Sub



    Private Sub Timer4_Tick(sender As Object, e As EventArgs) Handles Timer4.Tick
        Try
            cpu = PerformanceCounter1.NextValue
            ram = PerformanceCounter2.NextValue

            If ProgressBar3.Value >= 50 Then
                ChangeProgBarColor(ProgressBar3, ProgressBarColor.Yellow)
            ElseIf ProgressBar3.Value <= 50 Then
                ChangeProgBarColor(ProgressBar3, ProgressBarColor.Green)
            ElseIf ProgressBar3.Value <= 60 Then
                ChangeProgBarColor(ProgressBar3, ProgressBarColor.Red)
            End If
            If ProgressBar2.Value >= 50 Then
                ChangeProgBarColor(ProgressBar2, ProgressBarColor.Yellow)
            ElseIf ProgressBar2.Value <= 50 Then
                ChangeProgBarColor(ProgressBar2, ProgressBarColor.Green)
            ElseIf ProgressBar2.Value >= 60 Then
                ChangeProgBarColor(ProgressBar2, ProgressBarColor.Red)
            End If
        Catch
        End Try
    End Sub

 
    Private Sub Timer5_Tick(sender As Object, e As EventArgs) Handles Timer5.Tick
        Try
            If ProgressBar2.Value < cpu Then
                ProgressBar2.Value += 1
            ElseIf ProgressBar2.Value > cpu Then
                ProgressBar2.Value -= 1
            End If
            If ProgressBar3.Value < ram Then
                ProgressBar3.Value += 1
            ElseIf ProgressBar3.Value > ram Then
                ProgressBar3.Value -= 1
            End If
            Label10.Text = ProgressBar2.Value.ToString + "%"
            Label11.Text = ProgressBar3.Value.ToString + "%"
        Catch
        End Try
    End Sub

    Private Sub NotifyIcon1_Click(sender As Object, e As EventArgs) Handles NotifyIcon1.Click
        Me.Show()
    End Sub

    Private Sub NotifyIcon1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick
        If e.Button = MouseButtons.Right Then NotifyIcon1.ContextMenuStrip.Show()
    End Sub

    Private Sub HiệnToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HiệnToolStripMenuItem.Click
        Timer4.Enabled = True
        Timer5.Enabled = True
        Me.Show()
    End Sub

    Private Sub ẨnToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ẨnToolStripMenuItem.Click
        Timer4.Enabled = False
        Timer5.Enabled = False
        Me.Hide()
    End Sub

    Private Sub ThoátToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ThoátToolStripMenuItem.Click
        Dim qwe As Integer
        W = New IO.StreamWriter(Application.StartupPath & "\nhatki.log")
        For qwe = 0 To ListBox6.Items.Count - 1
            W.WriteLine(ListBox6.Items.Item(qwe))
        Next
        W.Close()
        End
    End Sub

    Private Function IsConnectionAvailable() As Boolean

        Dim url As New System.Uri("http://www.coccoc.vn/")
        Dim req As System.Net.WebRequest
        req = System.Net.WebRequest.Create(url)
        Dim resp As System.Net.WebResponse
        Try
            resp = req.GetResponse()
            resp.Close()
            req = Nothing
            Return True
        Catch ex As Exception
            req = Nothing
            Return False
        End Try
    End Function


    

  

    Private Sub Label3_MouseMove(sender As Object, e As MouseEventArgs) Handles Label3.MouseMove
        Label3.BackColor = Color.Red
    End Sub
    Private Sub Label4_MouseMove(sender As Object, e As MouseEventArgs) Handles Label4.MouseMove
        Label4.BackColor = Color.White
    End Sub

    Private Sub Panel1_MouseMove(sender As Object, e As MouseEventArgs) Handles Panel1.MouseMove
        Label4.BackColor = Color.Transparent
        Label3.BackColor = Color.Transparent
        Label7.ForeColor = Color.Transparent
    End Sub

    Private Sub Label7_MouseMove(sender As Object, e As MouseEventArgs)
        Label7.ForeColor = Color.Red
    End Sub

    Private Sub gocaidat_Click(sender As Object, e As EventArgs) Handles gocaidat.Click

    End Sub

   
    

   

 

    Private Sub lvApps_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lvApps.SelectedIndexChanged
        btnUninstall.Visible = True 'Upon itemn selection make these controls visible

        ten.Visible = True
        lblPublisher.Visible = True
        lblSize.Visible = True
        lblVersion.Visible = True

        'Display each Installed program's properties
        ten.Text = "Chương trình :" & lvApps.FocusedItem.SubItems(0).Text
        lblPublisher.Text = "Nhà phát hành :" & lvApps.FocusedItem.SubItems(1).Text
        lblSize.Text = "Dung lượng: " & lvApps.FocusedItem.SubItems(3).Text
        lblVersion.Text = "Phiên bản sản phẩm: " & lvApps.FocusedItem.SubItems(4).Text
    End Sub

    

    Private Sub btnUninstall_Click(sender As Object, e As EventArgs) Handles btnUninstall.Click
        Try
            'Use Shell to execute uninstall command
            Dim procID As Integer

            procID = Shell(NewUninstallStrArr(lvApps.FocusedItem.Index), AppWinStyle.NormalFocus)
        Catch
            End
        End Try
    End Sub


    
    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        FolderBrowserDialog1.SelectedPath = ("C:\Windows\")
        FolderBrowserDialog2.SelectedPath = ("D:\")
        TabControl1.SelectTab(1)
        Try
            For Each strDir As String In System.IO.Directory.GetDirectories(FolderBrowserDialog1.SelectedPath)


                For Each strFile As String In System.IO.Directory.GetFiles(strDir)

                    ListBox1.Items.Add(strFile)

                Next

            Next
        Catch ex As Exception
        End Try

        Timer1.Start()
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        FolderBrowserDialog1.SelectedPath = ("C:\Program Files\")
        FolderBrowserDialog2.SelectedPath = ("D:\")
        TabControl1.SelectTab(1)
        Try
            For Each strDir As String In System.IO.Directory.GetDirectories(FolderBrowserDialog1.SelectedPath)


                For Each strFile As String In System.IO.Directory.GetFiles(strDir)

                    ListBox1.Items.Add(strFile)

                Next

            Next
        Catch ex As Exception
        End Try

        Timer1.Start()
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Try
            ofd.ShowDialog()
            ListBox1.Items.Clear()
            TabControl1.SelectTab(1)
            TextBox1.Text = ofd.FileName
            ListBox1.Items.Add(ofd.FileName)
            Timer1.Start()
        Catch
            End
        End Try
    End Sub

   
    ' Private Sub Timer6_Tick(sender As Object, e As EventArgs) Handles Timer6.Tick
    '  s.Text = s.Text + 1
    ' Dim FILE_NAME As String = Application.StartupPath & "\s.txt"

    ' If System.IO.File.Exists(FILE_NAME) = True Then


    ' Dim objReader As New System.IO.StreamReader(FILE_NAME)
    '    s.Text = objReader.ReadToEnd
    '      objReader.Close()

    '  Else

    '  End If
    ' s.Text = s.Text + 1
    '  My.Computer.FileSystem.WriteAllText(Application.StartupPath & "\s.txt", s.Text, False)

    'End Sub

    Private Sub EmptyRecycleBin()
        Try
            SHEmptyRecycleBin(Me.Handle.ToInt32, vbNullString, SHERB_NOCONFIRMATION + SHERB_NOSOUND)
            SHUpdateRecycleBinIcon()
        Catch
        End Try
    End Sub




    Sub Clear_Temp_Files()
        Try
            Shell("RunDll32.exe InetCpl.cpl,ClearMyTracksByProcess 8") 'if this one does not work-then add a space after the 8
        Catch
            End
        End Try
    End Sub

    Sub Clear_Cookies()
        Try
            Shell("RunDll32.exe InetCpl.cpl,ClearMyTracksByProcess 2")
        Catch
            End
        End Try
    End Sub

    Sub Clear_History()
        Try
            Shell("RunDll32.exe InetCpl.cpl,ClearMyTracksByProcess 1")
        Catch
            End
        End Try
    End Sub

    Sub Clear_Form_Data()
        Try
            Shell("RunDll32.exe InetCpl.cpl,ClearMyTracksByProcess 16")
        Catch
            End
        End Try
    End Sub

    Sub Clear_Saved_Passwords()
        Try
            Shell("RunDll32.exe InetCpl.cpl,ClearMyTracksByProcess 32")
        Catch
            End
        End Try
    End Sub

    Sub Clear_All()
        Try
            Shell("RunDll32.exe InetCpl.cpl,ClearMyTracksByProcess 255")
        Catch
            End
        End Try
    End Sub

    Sub Clear_Clear_Add_ons_Settings()
        Try
            Shell("RunDll32.exe InetCpl.cpl,ClearMyTracksByProcess 4351")
        Catch
            End
        End Try
    End Sub

    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click
        Clear_Temp_Files()
    End Sub

    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        Clear_Cookies()
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        Clear_History()
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        Clear_Form_Data()
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        Clear_Saved_Passwords()
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        Clear_Clear_Add_ons_Settings()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Clear_All()
        EmptyRecycleBin()
    End Sub

    Private Sub quetdon_Click(sender As Object, e As EventArgs) Handles quetdon.Click

    End Sub

    Private Sub Button17_Click(sender As Object, e As EventArgs) Handles Button17.Click
        Dim selected As String = ListBox5.SelectedItem

        Dim regKey As RegistryKey

        Dim newkey As RegistryKey

        Dim subkey As RegistryKey

        regKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Run", True)

        newkey = Registry.LocalMachine.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Run", True)

        subkey = Registry.LocalMachine.OpenSubKey("Software\Wow6432Node\Microsoft\Windows\CurrentVersion\Run", True)



        'Using try and catch to avoid getting errors if the selected entry is not in this locaiton, it will then move to the next location until it find the entry

        Try

            regKey.DeleteValue(selected)

        Catch ex As Exception

        End Try

        Try

            newkey.DeleteValue(selected)

        Catch ex As Exception

        End Try

        Try

            subkey.DeleteValue(selected)

        Catch ex As Exception

        End Try

        ListBox5.Items.Clear()

        Try

            regKey.GetValueNames()

            For i = 0 To UBound(regKey.GetValueNames())

                Me.ListBox5.Items.Add(regKey.GetValueNames(i))

            Next

        Catch ex As Exception



        End Try

        Try

            newkey.GetValueNames()

            For i = 0 To UBound(newkey.GetValueNames())

                Me.ListBox5.Items.Add(newkey.GetValueNames(i))

            Next

        Catch ex As Exception



        End Try

        Try

            subkey.GetValueNames()

            For i = 0 To UBound(subkey.GetValueNames())

                Me.ListBox5.Items.Add(subkey.GetValueNames(i))

            Next

        Catch ex As Exception



        End Try

    End Sub

    Private Sub Button18_Click(sender As Object, e As EventArgs) Handles Button18.Click
        ListBox5.Items.Clear()
        Dim regKey As RegistryKey   'new Reg key for each location within the registry

        Dim newkey As RegistryKey

        Dim subkey As RegistryKey

        regKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Run", True)

        'Using try and catch to avoid getting errors if there are no entries in the locations

        Try

            regKey.GetValueNames()

            For i = 0 To UBound(regKey.GetValueNames())

                Me.ListBox5.Items.Add(regKey.GetValueNames(i))

            Next

        Catch ex As Exception

        End Try



        newkey = Registry.LocalMachine.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Run", True)

        Try

            newkey.GetValueNames()

            For i = 0 To UBound(newkey.GetValueNames())

                Me.ListBox5.Items.Add(newkey.GetValueNames(i))

            Next

        Catch ex As Exception

        End Try



        subkey = Registry.LocalMachine.OpenSubKey("Software\Wow6432Node\Microsoft\Windows\CurrentVersion\Run", True)

        Try

            subkey.GetValueNames()

            For i = 0 To UBound(subkey.GetValueNames())

                Me.ListBox5.Items.Add(subkey.GetValueNames(i))

            Next

        Catch ex As Exception

        End Try

    End Sub


    

    Private Sub Button19_Click(sender As Object, e As EventArgs) Handles Button19.Click
        Try
            Dim FileToDelete As String

            FileToDelete = Application.StartupPath & "\nhatki.log"

            If System.IO.File.Exists(FileToDelete) = True Then

                System.IO.File.Delete(FileToDelete)
                MsgBox("Đã xóa nhật kí !")
                ListBox6.Items.Clear()
            End If
        Catch

        End Try
    End Sub

    Dim IsMouseDown = False
    Dim startPoint
    Private Sub TitleBar_MouseUp_1(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TitleBar.MouseUp
        Try
            IsMouseDown = False
        Catch
        End Try
    End Sub
    Private Sub TitleBar_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TitleBar.MouseDown
        Try
            startPoint = e.Location
            IsMouseDown = True
        Catch
        End Try
    End Sub
    Private Sub TitleBar_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TitleBar.MouseMove
        Try
            If IsMouseDown Then
                Dim p1 = New Point(e.X, e.Y)
                Dim p2 = PointToScreen(p1)
                Dim p3 = New Point(p2.X - startPoint.X, p2.Y - startPoint.Y)
                Location = p3
            End If
        Catch
        End Try
    End Sub

   
    Private Sub Timer3_Tick(sender As Object, e As EventArgs) Handles Timer3.Tick
        Try
            '  ChangeProgBarColor(ProgressBar1, ProgressBarColor.Yellow)
            Timer4.Enabled = True
            Timer5.Enabled = True
            NotifyIcon1.ContextMenuStrip = ContextMenuStrip1
            ' kiểm tra inetrnet tới coccoc.vn
            If IsConnectionAvailable() = True Then
                sp.Text = "Có kết nối Innternet!"
                sp.ForeColor = Color.Green
            Else
                sp.Text = "Không có kết nối Internet!"
                sp.ForeColor = Color.Red
            End If

            ' gỡ cài đặt
            Dim myNA() As NetworkInterface = NetworkInterface.GetAllNetworkInterfaces

            td.Text = myNA(0).Speed

            ip.Text = sr.ReadToEnd


            'Location in Registry where all uninstall "settings" are stored
            Dim ParentKey As RegistryKey = _
                Registry.LocalMachine.OpenSubKey("SOFTWARE\MICROSOFT\Windows\CurrentVersion\Installer\UserData\S-1-5-18\Products")

            Dim count As Integer = 0 'Loop Counter

            Dim ChildKey As RegistryKey 'Sub key of Parent key, to read necessary Uninstall properties

            'Loop through each GUID listed
            For Each child As String In ParentKey.GetSubKeyNames()

                ChildKey = ParentKey.OpenSubKey(child).OpenSubKey("InstallProperties") 'Read InstallProperties value(s)

                If Not ChildKey Is Nothing Then 'If not empty, display inside ListView

                    Dim LItem As New ListViewItem() 'Create new ListView item

                    LItem.Text = ChildKey.GetValue("DisplayName").ToString 'First Column ( Main Item ) Text - Display Name
                    LItem.SubItems.Add(ChildKey.GetValue("Publisher").ToString) 'Publisher
                    LItem.SubItems.Add(ChildKey.GetValue("InstallDate").ToString) 'Install Date
                    LItem.SubItems.Add(ChildKey.GetValue("EstimatedSize").ToString) 'Estimated Size
                    LItem.SubItems.Add(ChildKey.GetValue("DisplayVersion").ToString) 'Display Version

                    ReDim Preserve strUninstallStrings(count) 'Redim Array

                    If ChildKey.GetValue("UninstallString") IsNot Nothing Then 'Determine Uninstall Command(s)

                        strUninstallStrings(count) = ChildKey.GetValue("UninstallString") 'Store each command for each program

                        lvApps.Items.Add(LItem) 'Add ListItem

                    Else 'If No Uninstall Command Present, Identify it

                        strUninstallStrings(count) = "No Uninstall String"

                    End If

                End If

                count += 1 'Increment Counter for each item

            Next

            'Use LINQ to filter strUninstallStrings array, to only get valid programs with valid uninstall strings
            NewUninstallStrArr = (From str In strUninstallStrings
                  Where Not {"No Uninstall String"}.Contains(str)).ToArray() 'New array to be used


            Dim FILE_NAME2 As String = Application.StartupPath & "\dtvr.txt"

            If System.IO.File.Exists(FILE_NAME2) = True Then


                Dim objReader2 As New System.IO.StreamReader(FILE_NAME2)
                Label19.Text = objReader2.ReadToEnd
                objReader2.Close()

            Else

            End If


            ' Phan dọn dep khởi động cùng máy tính
            Dim regKey As RegistryKey   'new Reg key for each location within the registry

            Dim newkey As RegistryKey

            Dim subkey As RegistryKey

            regKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Run", True)

            'Using try and catch to avoid getting errors if there are no entries in the locations

            Try

                regKey.GetValueNames()

                For i = 0 To UBound(regKey.GetValueNames())

                    Me.ListBox5.Items.Add(regKey.GetValueNames(i))

                Next

            Catch ex As Exception

            End Try



            newkey = Registry.LocalMachine.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Run", True)

            Try

                newkey.GetValueNames()

                For i = 0 To UBound(newkey.GetValueNames())

                    Me.ListBox5.Items.Add(newkey.GetValueNames(i))

                Next

            Catch ex As Exception

            End Try



            subkey = Registry.LocalMachine.OpenSubKey("Software\Wow6432Node\Microsoft\Windows\CurrentVersion\Run", True)

            Try

                subkey.GetValueNames()

                For i = 0 To UBound(subkey.GetValueNames())

                    Me.ListBox5.Items.Add(subkey.GetValueNames(i))

                Next

            Catch ex As Exception

            End Try
            ' cập nhật bản mới
            Try
                Dim sourceString As String = New System.Net.WebClient().DownloadString("http://kho.luutru360.com/tazansoft/update.txt")
                Dim sourceString2 As String = New System.Net.WebClient().DownloadString("http://kho.luutru360.com/tazansoft/virus.txt")
                If sourceString = Label18.Text Then
                    cn.Visible = False
                Else
                    cn.Visible = True
                    Timer3.Enabled = False
                End If
                If sourceString2 = Label19.Text Then
                Else
                    My.Computer.FileSystem.WriteAllText(Application.StartupPath & "\dtvr.txt", sourceString2, False)
                    update2.Show()
                    Me.Hide()
                End If
            Catch

            End Try
            ' nhat ki log vao
            Try
                Dim lines() As String = IO.File.ReadAllLines(Application.StartupPath & "\nhatki.log")
                If System.IO.File.Exists(Application.StartupPath & "\nhatki.log") = True Then
                    ListBox6.Items.AddRange(lines)
                Else
                End If
            Catch
            End Try
            Timer3.Enabled = False
        Catch
            Timer3.Enabled = False
        End Try
    End Sub



    Private Sub Button20_Click(sender As Object, e As EventArgs) Handles Button20.Click
        Timer1.Enabled = False
        Button21.Enabled = True
        Button20.Enabled = False
    End Sub
    Private Sub nhatki_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button21_Click(sender As Object, e As EventArgs) Handles Button21.Click
        Timer1.Enabled = True
        Button20.Enabled = True
        Button21.Enabled = False
    End Sub

    Private Sub Label16_Click(sender As Object, e As EventArgs) Handles vir.Click
        DotNetBarTabcontrol1.SelectTab(2)
        TabControl1.SelectTab(3)
    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBox3.TextChanged

    End Sub
End Class
