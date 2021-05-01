Imports System.ComponentModel

Public Class GlobalImportForm
    Public 父域 As 节点平面类
    Public Sub New(parent As 节点平面类)

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        父域 = parent
    End Sub
    Private Sub GlobalImportForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Visible = False
        e.Cancel = True
    End Sub

    Private Sub 添加全局平面_Click(sender As Object, e As EventArgs) Handles 添加全局平面.Click
        If Not 全局平面列表.Items.Contains(新全局平面路径.Text) And 新全局平面路径.Text <> "" Then
            父域.添加全局平面(新全局平面路径.Text)
            新全局平面路径.Text = ""
        End If
    End Sub

    Private Sub 移除全局平面_Click(sender As Object, e As EventArgs) Handles 移除全局平面.Click
        父域.移除全局平面()
    End Sub

    Private Sub 添加全局节点_Click(sender As Object, e As EventArgs) Handles 添加全局节点.Click
        If Not 全局节点列表.Items.Contains(新全局节点路径.Text) And 新全局节点路径.Text <> "" Then
            全局节点列表.Items.Add(新全局节点路径.Text)
            新全局节点路径.Text = ""
        End If
    End Sub

    Private Sub 移除全局节点_Click(sender As Object, e As EventArgs) Handles 移除全局节点.Click
        If 全局节点列表.SelectedIndex >= 0 And 全局节点列表.SelectedIndex < 全局节点列表.Items.Count Then
            全局节点列表.Items.RemoveAt(全局节点列表.SelectedIndex)
        End If
    End Sub
End Class