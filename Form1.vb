﻿Imports System.ComponentModel

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
End Class
