Imports System.ComponentModel
Imports System.IO

Public Class Form1
    Private 主域 As 节点平面类
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        程序版本.Text = "版本：" & Application.ProductVersion
        控制台 = New NodeConsole(Me)
        加载设置文件()
        绘制空间.Image = New Bitmap(10, 10)
        主域 = New 节点平面类(Me, 绘制空间)
        控制台.Show()
    End Sub
    Private Sub 加载设置文件()
        If File.Exists("程序设置.ini") Then
            Dim 设置() As String = Split(File.ReadAllText("程序设置.ini"), vbCrLf)
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
            "接口点背景色=" & 接口点背景色.BackColor.ToArgb
        }
        File.WriteAllText("程序设置.ini", Join(设置.ToArray, vbCrLf))
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
            End Select
        End If
    End Sub
    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        主域.结束()
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
                Dim 脚本 As New 节点脚本类
                脚本.解释(主域.鼠标移动选中节点)
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
End Class
