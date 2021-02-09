Imports System.ComponentModel
Imports System.Text
Imports System.Text.RegularExpressions

Public Class Form1
    Dim accounts As String() = IO.File.ReadAllLines("accs.txt")
    Dim codes As New StringBuilder
    Dim index As Integer = 0
    Dim username, password As String
    Dim desktop = My.Computer.FileSystem.SpecialDirectories.Desktop
    Private Declare Function SetCursorPos Lib "user32" (ByVal x As Integer, ByVal y As Integer) As Integer
    Private Const MOUSEEVENTF_RIGHTDOWN = &H2
    Private Const MOUSEEVENTF_RIGHTUP = &H4
    Private Declare Sub mouse_event Lib "user32" (ByVal dwFlags As Integer, ByVal dx As Integer, ByVal dy As Integer, ByVal cButtons As Integer, ByVal dwExtraInfo As Integer)

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer1.Start()
        clear_cookies()
        Me.Location = New Point(200, 180)
        WebBrowser1.Navigate("https://www.facebook.com/dialog/oauth?client_id=124024574287414&redirect_uri=https%3A%2F%2Fwww.instagram.com%2Faccounts%2Fsignup%2F&state=%7B%22fbLoginKey%22%3A%221137ph01o0lwjn3774s0jj8p2v14ulbimpp5pvk3l28ildzry6j%22%2C%22fbLoginReturnURL%22%3A%22%2F%22%7D&scope=email&response_type=code%2Cgranted_scopes&locale=en_US&ret=login&fbapp_pres=0&logger_id=b5c29836-786f-43d5-89ec-bb66cff61a04&tp=unspecified&cbt=1612042222312&ext=1612045831&hash=AeY5n4BQYXIOF6jtqiY")
    End Sub
    Private Sub WebBrowser1_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles WebBrowser1.DocumentCompleted
        Try
            WebBrowser1.ScriptErrorsSuppressed() = True
            Dim line As String = accounts(index)
            username = line.Split("|")(0)
            password = line.Split("|")(1)
            Dim str As String = WebBrowser1.DocumentText
            If str.Contains("/dialog/oauth/") Then
                confirm_click()
            ElseIf str.Contains("family") Then
                Try
                    WebBrowser1.Document.GetElementById("email").InnerText = username
                    WebBrowser1.Document.GetElementById("pass").InnerText = password
                    WebBrowser1.Document.GetElementById("login").InvokeMember("click")
                Catch ex As Exception
                End Try
            ElseIf str.Contains("?code=") Then
                Dim code As String = Regex.Match(str, "code=(.*?)&amp").Groups.Item(1).Value
                If Not codes.ToString.Contains(code) Then
                    codes.AppendLine(code)
                    ListBox1.Items.Add(code)
                    clear_cookies()
                End If
            Else
                MsgBox(str)
            End If
            If str.Contains("enter the 6-digit code we sent") Then
                If index > accounts.Length - 1 Then
                    MsgBox("Finished accounts")
                End If
                index += 1
                clear_cookies()
                reload()
            End If
            If str.Contains("Your Account is Temporarily Locked") Then
                If index > accounts.Length - 1 Then
                    MsgBox("Finished accounts")
                End If
                index += 1
                clear_cookies()
                reload()
            End If
            If WebBrowser1.Url.ToString.Contains("checkpoint") Then
                If index > accounts.Length - 1 Then
                    MsgBox("Finished accounts")
                End If
                index += 1
                clear_cookies()
                reload()
            End If
            If str.Contains("working on getting this fixed") Then
                If index > accounts.Length - 1 Then
                    MsgBox("Finished accounts")
                End If
                clear_cookies()
                MsgBox($"{index.ToString}/{accounts.Length.ToString} - that weird freaking error!")
            End If
            If str.Contains("Did you forget your password?") Then
                If index > accounts.Length - 1 Then
                    MsgBox("Finished accounts")
                End If
                index += 1
                clear_cookies()
                reload()
            End If
            If WebBrowser1.Url.ToString = "https://www.instagram.com/" Then
                If index > accounts.Length - 1 Then
                    MsgBox("Finished accounts")
                End If
                index += 1
                reload()
            End If
        Catch ex As Exception
        End Try
        Threading.Thread.Sleep(750)
    End Sub
    Private Sub btn_Export_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Dim str As New StringBuilder
            For Each item As String In ListBox1.Items
                str.AppendLine(item)
            Next
            Dim rndpath As String = desktop + $"\{New Random().Next(0, 99999).ToString}_users.txt"
            IO.File.WriteAllText(desktop + $"\{New Random().Next(0, 99999).ToString}_users.txt", str.ToString)
            MsgBox("Exported to " + rndpath)
            Process.Start(rndpath)
        Catch ex As Exception
        End Try
    End Sub
    Private Sub btn_reload_Click(sender As Object, e As EventArgs) Handles refresh.Click
        WebBrowser1.Navigate("https://www.facebook.com/dialog/oauth?client_id=124024574287414&redirect_uri=https%3A%2F%2Fwww.instagram.com%2Faccounts%2Fsignup%2F&state=%7B%22fbLoginKey%22%3A%221137ph01o0lwjn3774s0jj8p2v14ulbimpp5pvk3l28ildzry6j%22%2C%22fbLoginReturnURL%22%3A%22%2F%22%7D&scope=email&response_type=code%2Cgranted_scopes&locale=en_US&ret=login&fbapp_pres=0&logger_id=b5c29836-786f-43d5-89ec-bb66cff61a04&tp=unspecified&cbt=1612042222312&ext=1612045831&hash=AeY5n4BQYXIOF6jtqiY")
    End Sub
    Sub confirm_click()
        SetCursorPos(500, 500)
        Threading.Thread.Sleep(1000)
        RightDown()
        RightUp()
    End Sub
    Sub clear_cookies()
        System.Diagnostics.Process.Start("rundll32.exe", "InetCpl.cpl,ClearMyTracksByProcess 2")
    End Sub
    Sub reload()
        WebBrowser1.Navigate("https://www.facebook.com/dialog/oauth?client_id=124024574287414&redirect_uri=https%3A%2F%2Fwww.instagram.com%2Faccounts%2Fsignup%2F&state=%7B%22fbLoginKey%22%3A%221137ph01o0lwjn3774s0jj8p2v14ulbimpp5pvk3l28ildzry6j%22%2C%22fbLoginReturnURL%22%3A%22%2F%22%7D&scope=email&response_type=code%2Cgranted_scopes&locale=en_US&ret=login&fbapp_pres=0&logger_id=b5c29836-786f-43d5-89ec-bb66cff61a04&tp=unspecified&cbt=1612042222312&ext=1612045831&hash=AeY5n4BQYXIOF6jtqiY")
    End Sub
    Private Sub btn_Clear_Click(sender As Object, e As EventArgs) Handles clear.Click
        System.Diagnostics.Process.Start("rundll32.exe", "InetCpl.cpl,ClearMyTracksByProcess 2")
    End Sub
    Public Sub RightDown()
        mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0)
    End Sub
    Public Sub RightUp()
        mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0)
    End Sub
    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Try
            Clipboard.SetText(codes.ToString)
            IO.File.WriteAllText($"codes_{New Random().Next(0, 9999).ToString}.txt", codes.ToString)
            clear_cookies()
        Catch ex As Exception
        End Try
    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Label1.Text = $"Count = {ListBox1.Items.Count.ToString}"
        Me.Text = $"Token Extractor | {index}/{accounts.Length - 1}"
    End Sub
End Class
