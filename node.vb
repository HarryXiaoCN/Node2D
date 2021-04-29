Public Class Node
    Public 主域 As 节点平面类
    Private Sub 节点名_TextChanged(sender As Object, e As EventArgs) Handles 节点名.TextChanged
        If 节点名.Text <> 主域.当前编辑节点.名字 Then
            If 主域.本域节点.ContainsKey(节点名.Text) Then
                If 节点名.ForeColor <> Color.Orange Then
                    节点名.ForeColor = Color.Orange
                End If
            Else
                If 节点名.ForeColor <> Color.Red Then
                    节点名.ForeColor = Color.Red
                End If
            End If
        Else
            If 节点名.ForeColor <> Color.Green Then
                节点名.ForeColor = Color.Green
            End If
        End If
    End Sub

    Private Sub 节点内容_TextChanged(sender As Object, e As EventArgs) Handles 节点内容.TextChanged
        If 节点内容.Text <> 主域.当前编辑节点.内容 Then
            If 节点内容.ForeColor <> Color.Red Then
                节点内容.ForeColor = Color.Red
            End If
        Else
            If 节点内容.ForeColor <> Color.Green Then
                节点内容.ForeColor = Color.Green
            End If
        End If
    End Sub

    Private Sub 节点名_KeyDown(sender As Object, e As KeyEventArgs) Handles 节点名.KeyDown
        If e.Control And e.KeyCode = Keys.S Then
            主域.编辑节点名(主域.当前编辑节点.名字, 节点名.Text)
            节点名_TextChanged(Nothing, Nothing)
        End If
    End Sub
    Private Sub 节点类型_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles 节点类型.SelectionChangeCommitted
        主域.编辑节点类型(主域.当前编辑节点.名字, 节点类型.Text)
    End Sub

    Private Sub 节点内容_KeyDown(sender As Object, e As KeyEventArgs) Handles 节点内容.KeyDown
        If e.Control And e.KeyCode = Keys.S Then
            主域.编辑节点内容(主域.当前编辑节点.名字, 节点内容.Text)
            节点内容_TextChanged(Nothing, Nothing)
        End If
    End Sub

    Private Sub Node_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            If Visible Then
                Hide()
            End If
        ElseIf e.KeyCode = Keys.F5 Then
            If 主域.鼠标移动选中节点 IsNot Nothing Then
                If 主域.鼠标移动选中节点.类型 = "函数" Then
                    Dim 脚本 As New 节点脚本类
                    脚本.解释(主域.鼠标移动选中节点)
                    If 控制台.Visible = False Then 控制台.Visible = True
                End If
            End If
        End If
    End Sub

    Private Sub Node_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(27) Then
            e.Handled = True
        End If
    End Sub
End Class