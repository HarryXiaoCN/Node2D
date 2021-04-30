Imports System.ComponentModel
Imports System.IO

Public Class Form1
    Private 主域 As 节点平面类
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
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
            "控制台每次执行自动清空=" & 控制台每次运行时清空.Enabled
        }
        File.WriteAllText("程序设置.ini", Join(设置.ToArray, vbCrLf))
    End Sub
    Private Sub 设置解释(s As String)
        If s.IndexOf("=") <> -1 Then
            Dim sT() As String = Split(s, "=")
            Select Case sT(0).ToLower
                Case "控制台每次执行自动清空"
                    If sT(1).ToLower = "true" Or sT(1) = "1" Then
                        控制台每次运行时清空.Checked = True
                    End If
            End Select
        End If
    End Sub
    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        主域.结束标识 = True
        保存设置文件()
    End Sub

    Private Sub Form1_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        End
    End Sub

    Private Sub 绘制空间_Click(sender As Object, e As EventArgs) Handles 绘制空间.Click

    End Sub

    Private Sub 文件退出菜单_Click(sender As Object, e As EventArgs) Handles 文件退出菜单.Click
        Close()
    End Sub

    Private Sub 绘制空间_MouseMove(sender As Object, e As MouseEventArgs) Handles 绘制空间.MouseMove

    End Sub

    Private Sub 编辑右键点击创建值节点_Click(sender As Object, e As EventArgs) Handles 编辑右键点击创建值节点.Click
        编辑右键点击创建值节点.Checked = Not 编辑右键点击创建值节点.Checked
        If 编辑右键点击创建值节点.Checked Then
            编辑右键点击创建引用点.Checked = False
            编辑右键点击创建函数点.Checked = False
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
            主域.节点创建模式 = "引用"
        Else
            检查编辑右键点击创建节点全空情况()
        End If
    End Sub

    Private Sub 检查编辑右键点击创建节点全空情况()
        If Not (编辑右键点击创建值节点.Checked Or 编辑右键点击创建引用点.Checked Or 编辑右键点击创建函数点.Checked) Then
            主域.节点创建模式 = ""
        End If
    End Sub

    Private Sub 绘制空间_MouseDown(sender As Object, e As MouseEventArgs) Handles 绘制空间.MouseDown

    End Sub

    Private Sub 绘制空间_MouseUp(sender As Object, e As MouseEventArgs) Handles 绘制空间.MouseUp

    End Sub

    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown

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
            主域.结束标识 = True
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
            主域.结束标识 = True
            主域 = New 节点平面类(Me, 绘制空间)
            主域.加载(filePath)
        End If
    End Sub

    Private Sub 控制台每次运行时清空_Click(sender As Object, e As EventArgs) Handles 控制台每次运行时清空.Click
        控制台每次运行时清空.Checked = Not 控制台每次运行时清空.Checked
    End Sub
End Class
