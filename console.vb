Imports System.ComponentModel

Public Class NodeConsole
    Private Declare Auto Function RegisterHotKey Lib "user32.dll" Alias "RegisterHotKey" (hwnd As IntPtr, id As Integer, fsModifiers As Integer, vk As Integer) As Boolean
    Private Declare Auto Function UnRegisterHotKey Lib "user32.dll" Alias "UnregisterHotKey" (hwnd As IntPtr, id As Integer) As Boolean

    Private Delegate Sub 添加消息委托(info As String)
    Private Delegate Sub 设定消息委托(info As String)
    Public 父窗体 As Form1
    Private Delegate Sub 显示委托(v As String)
    Private Delegate Sub 热键注册委托(hwnd As IntPtr, id As Integer, fsModifiers As Integer, vk As Integer)
    Private Delegate Sub 热键卸载委托(hwnd As IntPtr, id As Integer)
    Private Delegate Function 获得窗体句柄委托() As IntPtr

    Public Function 获得窗体句柄() As IntPtr
        Dim d As New 获得窗体句柄委托(AddressOf 获得窗体句柄执行)
        Return Invoke(d)
    End Function
    Public Function 获得窗体句柄执行() As IntPtr
        Return Handle
    End Function

    Public Sub 热键注册(hwnd As IntPtr, id As Integer, fsModifiers As Integer, vk As Integer)
        Dim d As New 热键注册委托(AddressOf RegisterHotKey)
        Invoke(d, hwnd, id, fsModifiers, vk)
    End Sub
    Public Sub 热键卸载(hwnd As IntPtr, id As Integer)
        Dim d As New 热键卸载委托(AddressOf UnRegisterHotKey)
        Invoke(d, hwnd, id)
    End Sub

    Public Sub 显示(v As String)
        Dim d As New 显示委托(AddressOf 显示执行)
        Invoke(d, v)
    End Sub

    Private Sub 显示执行(v As String)
        If v = "" Then
            Visible = False
        Else
            Visible = True
        End If
    End Sub
    Public Sub New(ByRef parent As Form1)

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        父窗体 = parent
    End Sub
    Protected Overrides Sub WndProc(ByRef m As Message)
        If m.Msg = 786 Then
            Dim tid As Integer = m.WParam.ToInt32
            If 主界面.主域.脚本.全局等待锁.Contains(tid) Then
                主界面.主域.脚本.全局等待锁.Remove(tid)
            End If
        End If
        MyBase.WndProc(m)
    End Sub
    Public Sub 添加消息(info As String)
        Dim d As New 添加消息委托(AddressOf 添加消息执行)
        Invoke(d, info)
    End Sub
    Public Sub 设定消息(info As String)
        Dim d As New 设定消息委托(AddressOf 设定消息执行)
        Invoke(d, info)
    End Sub
    Private Sub 设定消息执行(info As String)
        If info <> "" Then
            If 父窗体.控制台输出时间戳.Checked Then
                控制输出.Text = String.Format("{0}: {1}", Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), info) & vbCrLf
            Else
                控制输出.Text = info & vbCrLf
            End If
        End If
    End Sub
    Private Sub 添加消息执行(info As String)
        If info <> "" Then
            控制输出.SelectionStart = 控制输出.TextLength
            If 父窗体.控制台输出时间戳.Checked Then
                控制输出.SelectedText = String.Format("{0}: {1}", Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), info) & vbCrLf
            Else
                控制输出.SelectedText = info & vbCrLf
            End If
        End If
    End Sub
    Private Sub NodeConsole_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        e.Cancel = True
        Visible = False
    End Sub

    Private Sub 复制文本_Click(sender As Object, e As EventArgs) Handles 复制文本.Click
        If 控制输出.SelectedText = "" Then
            Clipboard.SetText(控制输出.Text)
        Else
            Clipboard.SetText(控制输出.SelectedText)
        End If
    End Sub

    Private Sub 全选文本_Click(sender As Object, e As EventArgs) Handles 全选文本.Click
        '控制输出.SelectionStart = 0
        '控制输出.SelectionLength = 控制输出.TextLength
        控制输出.SelectAll()
    End Sub

    Private Sub 清空文本_Click(sender As Object, e As EventArgs) Handles 清空文本.Click
        控制输出.Text = ""
    End Sub

    Private Sub NodeConsole_Load(sender As Object, e As EventArgs) Handles Me.Load
        Top = Screen.PrimaryScreen.Bounds.Height - Height
        Left = (Screen.PrimaryScreen.Bounds.Width - Width) \ 2
    End Sub
End Class