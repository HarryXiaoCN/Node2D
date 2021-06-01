Imports System.ComponentModel
Imports System.IO

Public Class Form1
    Public 主域 As 节点平面类
    Private 已注册 As Boolean

    Private Delegate Sub 显示委托(v As String)
    Private Delegate Sub 托盘显示委托(v As String, t As String, i As String)

    Public Sub 托盘显示(v As String, t As String, i As String)
        Dim d As New 托盘显示委托(AddressOf 托盘显示执行)
        Invoke(d, v, t, i)
    End Sub

    Private Sub 托盘显示执行(v As String, t As String, i As String)
        If v = "" Then
            托盘.Visible = False
        Else
            托盘.Visible = True
        End If
        If t <> "" Then
            托盘.Text = t
        End If
        If i <> "" And File.Exists(i) Then
            主界面.托盘.Icon = Icon.ExtractAssociatedIcon(i)
        End If
    End Sub
    Public Sub 显示(v As String)
        Dim d As New 显示委托(AddressOf 显示执行)
        Invoke(d, v)
    End Sub

    Private Sub 显示执行(v As String)
        If v = "" Then
            Visible = False
            主域.节点编辑窗体.Visible = False
        Else
            Visible = True
        End If
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        主界面 = Me
        程序版本.Text = "版本：" & Application.ProductVersion
        控制台 = New NodeConsole(Me)
        浏览器 = New MyWebbrowser
        加载设置文件()
        程序注册()
        绘制空间.Image = New Bitmap(10, 10)
        主域 = New 节点平面类(Me, 绘制空间)
        启动命令处理()
        控制台.Show()
    End Sub
    Private Sub 程序注册()
        Try
            If 已注册 Then Exit Sub
            Dim mKey As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.ClassesRoot
            Dim mSubKey As Microsoft.Win32.RegistryKey = mKey.OpenSubKey(".n2d")
            Dim shellNewkey As Microsoft.Win32.RegistryKey
            If mSubKey Is Nothing Then
                shellNewkey = mKey.CreateSubKey(".n2d", True)
                shellNewkey.SetValue("", "Node2D")
                shellNewkey = mKey.CreateSubKey(".n2d\ShellNew", True)
                shellNewkey.SetValue("NullFile", "")
            End If
            mSubKey = mKey.OpenSubKey("Node2D", True)
            If mSubKey Is Nothing Then
                shellNewkey = mKey.CreateSubKey("Node2D", True)
                shellNewkey.SetValue("", "节点平面")
                If File.Exists(Application.StartupPath & "Node2D_File_Ico.ico") Then
                    shellNewkey = mKey.CreateSubKey("Node2D\DefaultIcon", True)
                    shellNewkey.SetValue("", Application.StartupPath & "node2d_file_ico.ico")
                End If
                mKey.CreateSubKey("Node2D\Shell", True)
                mKey.CreateSubKey("Node2D\Shell\Open", True)
                shellNewkey = mKey.CreateSubKey("Node2D\Shell\Open\Command", True)
                shellNewkey.SetValue("", Application.ExecutablePath & " %1")
                'mKey.CreateSubKey("Node2D\Shell\Edit", True)
                'shellNewkey = mKey.CreateSubKey("Node2D\Shell\Edit\Command", True)
                'shellNewkey.SetValue("", Application.ExecutablePath & " %1")
            End If
            已注册 = True
            保存设置文件()
        Catch ex As Exception
            If MsgBox(String.Format("文件关联失败！是否尝试管理员权限重新运行程序注册？" & vbCrLf & "(错误提示：{0})", ex.Message), MsgBoxStyle.YesNo + vbExclamation, "节点平面") = MsgBoxResult.Yes Then
                管理员权限运行自己()
            Else
                已注册 = True
            End If
        End Try
    End Sub

    Private Sub 启动命令处理()
        Dim cmd As String = Replace(Command(), """", "")
        If cmd <> "" Then
            If File.Exists(cmd) Then
                主域.加载(cmd)
            Else
                MsgBox("未知启动参数：" & cmd, 64, "节点平面")
            End If
        End If
    End Sub
    Private Sub 加载设置文件()
        Dim configPath As String = Application.StartupPath & "程序设置.ini"
        If File.Exists(configPath) Then
            Dim 设置() As String = Split(File.ReadAllText(configPath), vbCrLf)
            For i As Integer = 0 To UBound(设置)
                If 设置(i) <> "" Then
                    设置解释(设置(i))
                End If
            Next
        End If
    End Sub
    Private Sub 保存设置文件()
        Dim 设置 As New List(Of String) From {
            "控制台每次执行自动清空=" & 控制台每次运行时清空.Checked,
            "控制台消息增加时间戳=" & 控制台输出时间戳.Checked,
            "字体样式=" & Font.Name,
            "字体大小=" & Font.Size,
            "文字居中=" & 主窗体文字居中.Checked,
            "显示内容=" & 主窗体显示内容.Checked,
            "值节点背景色=" & 值节点背景色.BackColor.ToArgb,
            "引用点背景色=" & 引用点背景色.BackColor.ToArgb,
            "函数点背景色=" & 函数点背景色.BackColor.ToArgb,
            "接口点背景色=" & 接口点背景色.BackColor.ToArgb,
            "已注册=" & 已注册,
            "激活节点变色=" & 激活节点变色.Checked
        }
        File.WriteAllText(Application.StartupPath & "程序设置.ini", Join(设置.ToArray, vbCrLf))
    End Sub
    Private Sub 设置解释(s As String)
        If s.IndexOf("=") <> -1 Then
            Dim sT() As String = Split(s, "=")
            Dim sBool As Boolean
            If sT(1).ToLower = "true" Or sT(1) = "1" Then
                sBool = True
            End If
            Dim sInt As Integer = Val(sT(1))
            Select Case sT(0).ToLower
                Case "控制台每次执行自动清空"
                    控制台每次运行时清空.Checked = sBool
                Case "控制台消息增加时间戳"
                    控制台输出时间戳.Checked = sBool
                Case "字体样式"
                    Font = New Font(sT(1), Font.Size)
                Case "字体大小"
                    Font = New Font(Font.Name, Val(sT(1)))
                Case "文字居中"
                    主窗体文字居中.Checked = sBool
                Case "显示内容"
                    主窗体显示内容.Checked = sBool
                Case "值节点背景色"
                    值节点背景色.BackColor = Color.FromArgb(sInt)
                Case "引用点背景色"
                    引用点背景色.BackColor = Color.FromArgb(sInt)
                Case "函数点背景色"
                    函数点背景色.BackColor = Color.FromArgb(sInt)
                Case "接口点背景色"
                    接口点背景色.BackColor = Color.FromArgb(sInt)
                Case "已注册"
                    已注册 = sBool
                Case "激活节点变色"
                    激活节点变色.Checked = sBool
            End Select
        End If
    End Sub
    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If 主域 IsNot Nothing Then 主域.结束()
        托盘.Visible = False
        保存设置文件()
    End Sub

    Private Sub Form1_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        End
    End Sub

    Private Sub 文件退出菜单_Click(sender As Object, e As EventArgs) Handles 文件退出菜单.Click
        Close()
    End Sub

    Private Sub 编辑右键点击创建值节点_Click(sender As Object, e As EventArgs) Handles 编辑右键点击创建值节点.Click
        编辑右键点击创建值节点.Checked = Not 编辑右键点击创建值节点.Checked
        If 编辑右键点击创建值节点.Checked Then
            编辑右键点击创建引用点.Checked = False
            编辑右键点击创建函数点.Checked = False
            编辑右键点击创建接口点.Checked = False
            主域.节点创建模式 = "值"
        Else
            检查编辑右键点击创建节点全空情况()
        End If
    End Sub

    Private Sub 编辑右键点击创建函数点_Click(sender As Object, e As EventArgs) Handles 编辑右键点击创建函数点.Click
        编辑右键点击创建函数点.Checked = Not 编辑右键点击创建函数点.Checked
        If 编辑右键点击创建函数点.Checked Then
            编辑右键点击创建引用点.Checked = False
            编辑右键点击创建值节点.Checked = False
            编辑右键点击创建接口点.Checked = False
            主域.节点创建模式 = "函数"
        Else
            检查编辑右键点击创建节点全空情况()
        End If
    End Sub

    Private Sub 编辑右键点击创建引用点_Click(sender As Object, e As EventArgs) Handles 编辑右键点击创建引用点.Click
        编辑右键点击创建引用点.Checked = Not 编辑右键点击创建引用点.Checked
        If 编辑右键点击创建引用点.Checked Then
            编辑右键点击创建函数点.Checked = False
            编辑右键点击创建值节点.Checked = False
            编辑右键点击创建接口点.Checked = False
            主域.节点创建模式 = "引用"
        Else
            检查编辑右键点击创建节点全空情况()
        End If
    End Sub

    Private Sub 编辑右键点击创建接口点_Click(sender As Object, e As EventArgs) Handles 编辑右键点击创建接口点.Click
        编辑右键点击创建接口点.Checked = Not 编辑右键点击创建接口点.Checked
        If 编辑右键点击创建接口点.Checked Then
            编辑右键点击创建引用点.Checked = False
            编辑右键点击创建函数点.Checked = False
            编辑右键点击创建值节点.Checked = False
            主域.节点创建模式 = "接口"
        Else
            检查编辑右键点击创建节点全空情况()
        End If
    End Sub

    Private Sub 检查编辑右键点击创建节点全空情况()
        If Not (编辑右键点击创建值节点.Checked Or 编辑右键点击创建引用点.Checked Or 编辑右键点击创建函数点.Checked Or 编辑右键点击创建接口点.Checked) Then
            主域.节点创建模式 = ""
        End If
    End Sub

    Private Sub 运行菜单执行当前节点_Click(sender As Object, e As EventArgs) Handles 运行菜单执行当前节点.Click
        If 主域.鼠标移动选中节点 IsNot Nothing Then
            If 主域.鼠标移动选中节点.类型 = "函数" Then
                主域.脚本.解释(主域.鼠标移动选中节点)
                If 主域.主窗体.控制台每次运行时清空.Checked Then
                    控制台.控制输出.Text = ""
                End If
                If 控制台.Visible = False Then 控制台.Visible = True
            End If
        End If
    End Sub

    Private Sub 保存平面_Click(sender As Object, e As EventArgs) Handles 保存平面.Click
        Dim filePath As String = 主域.获得平面路径
        If File.Exists(filePath) Then
            主域.保存(filePath)
        Else
            If SFD.ShowDialog = DialogResult.OK Then
                主域.保存(SFD.FileName)
            End If
        End If
    End Sub

    Private Sub 载入平面_Click(sender As Object, e As EventArgs) Handles 载入平面.Click
        If OFD.ShowDialog = DialogResult.OK Then
            主域.结束()
            主域 = New 节点平面类(Me, 绘制空间)
            主域.加载(OFD.FileName)
        End If
    End Sub

    Private Sub 另存为平面_Click(sender As Object, e As EventArgs) Handles 另存为平面.Click
        If SFD.ShowDialog = DialogResult.OK Then
            主域.保存(SFD.FileName)
        End If
    End Sub

    Private Sub 重载平面_Click(sender As Object, e As EventArgs) Handles 重载平面.Click
        Dim filePath As String = 主域.获得平面路径
        If File.Exists(filePath) Then
            主域.结束()
            主域 = New 节点平面类(Me, 绘制空间)
            主域.加载(filePath)
        End If
    End Sub

    Private Sub 控制台每次运行时清空_Click(sender As Object, e As EventArgs) Handles 控制台每次运行时清空.Click
        控制台每次运行时清空.Checked = Not 控制台每次运行时清空.Checked
    End Sub

    Private Sub 打开全局引用窗体_Click(sender As Object, e As EventArgs) Handles 打开全局引用窗体.Click
        主域.全局窗体.Visible = True
    End Sub

    Private Sub 新建平面_Click(sender As Object, e As EventArgs) Handles 新建平面.Click
        主域.结束()
        主域 = New 节点平面类(Me, 绘制空间)
    End Sub

    Private Sub 控制台输出时间戳_Click(sender As Object, e As EventArgs) Handles 控制台输出时间戳.Click
        控制台输出时间戳.Checked = Not 控制台输出时间戳.Checked
    End Sub

    Private Sub 帮助主页_Click(sender As Object, e As EventArgs) Handles 帮助主页.Click
        Process.Start("explorer.exe", "http://harryxiao.cn/node2d.html")
    End Sub

    Private Sub 主窗体字体设置_Click(sender As Object, e As EventArgs) Handles 主窗体字体设置.Click
        FontD.Font = Font
        If FontD.ShowDialog = DialogResult.OK Then
            Font = FontD.Font
        End If
    End Sub

    Private Sub 主窗体文字居中_Click(sender As Object, e As EventArgs) Handles 主窗体文字居中.Click
        主窗体文字居中.Checked = Not 主窗体文字居中.Checked
    End Sub

    Private Sub 主窗体显示内容_Click(sender As Object, e As EventArgs) Handles 主窗体显示内容.Click
        主窗体显示内容.Checked = Not 主窗体显示内容.Checked
    End Sub

    Private Sub 接口点背景色_Click(sender As Object, e As EventArgs) Handles 接口点背景色.Click, 函数点背景色.Click, 值节点背景色.Click, 引用点背景色.Click
        If ColorD.ShowDialog = DialogResult.OK Then
            sender.BackColor = ColorD.Color
            主域.初始化颜色()
        End If
    End Sub

    Private Sub 解除文件关联_Click(sender As Object, e As EventArgs) Handles 解除文件关联.Click
        程序卸载()
    End Sub

    Private Sub Form1_DragDrop(sender As Object, e As DragEventArgs) Handles MyBase.DragDrop
        Dim filePath As String = e.Data.GetData("FileNameW")(0)
        If File.Exists(filePath) Then
            主域.加载(filePath)
        End If
    End Sub

    Private Sub Form1_DragOver(sender As Object, e As DragEventArgs) Handles MyBase.DragOver
        e.Effect = DragDropEffects.Copy
    End Sub

    Private Sub 托盘打开平面_Click(sender As Object, e As EventArgs) Handles 托盘打开平面.Click
        Visible = True
    End Sub

    Private Sub 托盘退出平面_Click(sender As Object, e As EventArgs) Handles 托盘退出平面.Click
        Close()
    End Sub

    Private Sub 托盘_DoubleClick(sender As Object, e As EventArgs) Handles 托盘.DoubleClick
        Visible = True
    End Sub

    Private Sub 激活节点变色_Click(sender As Object, e As EventArgs) Handles 激活节点变色.Click
        激活节点变色.Checked = Not 激活节点变色.Checked
    End Sub
End Class
