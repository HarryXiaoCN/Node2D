Imports System.ComponentModel

Public Class Form1
    Private 主域 As 节点平面类
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        主域 = New 节点平面类(Me, 绘制空间)
    End Sub
    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        主域.结束标识 = True
    End Sub

    Private Sub Form1_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        End
    End Sub

    Private Sub 绘制空间_Click(sender As Object, e As EventArgs) Handles 绘制空间.Click

    End Sub

    Private Sub 文件退出菜单_Click(sender As Object, e As EventArgs) Handles 文件退出菜单.Click
        Close()
    End Sub
End Class
