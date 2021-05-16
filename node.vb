Public Class Node
    Public 主域 As 节点平面类
    Private 避免咚 As Boolean
    Private 按住坐标 As New Point
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
        If e.Control Then
            If e.KeyCode = Keys.S Then
                主域.编辑节点名(主域.当前编辑节点.名字, 节点名.Text)
                节点名_TextChanged(Nothing, Nothing)
                避免咚 = True
            ElseIf e.KeyCode = Keys.A Then
                节点名.SelectAll()
                避免咚 = True
            End If
        Else
            Select Case e.KeyCode
                Case Keys.Tab
                    节点内容.Focus()
            End Select
        End If
    End Sub
    Private Sub 节点类型_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles 节点类型.SelectionChangeCommitted
        主域.编辑节点类型(主域.当前编辑节点.名字, 节点类型.Text)
    End Sub

    Private Sub 节点内容_KeyDown(sender As Object, e As KeyEventArgs) Handles 节点内容.KeyDown
        If e.Control Then
            If e.KeyCode = Keys.S Then
                主域.编辑节点内容(主域.当前编辑节点.名字, 节点内容.Text)
                节点内容_TextChanged(Nothing, Nothing)
                避免咚 = True
            ElseIf e.KeyCode = Keys.A Then
                节点内容.SelectAll()
                避免咚 = True
            End If
        Else
            Select Case e.KeyCode
                Case Keys.Down, Keys.Up
                    If 主域.候选窗体.Visible Then
                        e.Handled = True
                    End If
            End Select
        End If
    End Sub

    Private Sub Node_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            If Visible Then
                If 主域.候选窗体.Visible Then
                    主域.候选窗体.Visible = False
                Else
                    Hide()
                End If
            End If
        ElseIf e.KeyCode = Keys.F5 Then
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
        End If
    End Sub

    Private Sub Node_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(27) Then
            e.Handled = True
        End If
    End Sub

    Private Sub 节点名_KeyPress(sender As Object, e As KeyPressEventArgs) Handles 节点名.KeyPress
        If 避免咚 Then
            e.Handled = True
            避免咚 = False
        End If
    End Sub

    Private Sub 节点内容_KeyPress(sender As Object, e As KeyPressEventArgs) Handles 节点内容.KeyPress
        If 避免咚 Then
            e.Handled = True
            避免咚 = False
        End If
    End Sub

    Private Sub 大小控制_中_Click(sender As Object, e As EventArgs) Handles 大小控制_中.Click
        大小控制颜色还原()
        sender.ForeColor = Color.MediumSpringGreen
        Width = Screen.PrimaryScreen.Bounds.Width * 0.382
        Height = Screen.PrimaryScreen.Bounds.Height * 0.618
        主域.节点编辑窗体居中对齐()
    End Sub

    Private Sub 大小控制_小_Click(sender As Object, e As EventArgs) Handles 大小控制_小.Click
        大小控制颜色还原()
        sender.ForeColor = Color.MediumSpringGreen
        Width = 306
        Height = 230
        主域.节点编辑窗体居中对齐()
    End Sub

    Private Sub 大小控制_大_Click(sender As Object, e As EventArgs) Handles 大小控制_大.Click
        大小控制颜色还原()
        sender.ForeColor = Color.MediumSpringGreen
        Left = 0
        Top = 0
        Width = Screen.PrimaryScreen.Bounds.Width
        Height = Screen.PrimaryScreen.Bounds.Height
    End Sub
    Private Sub 大小控制颜色还原()
        大小控制_小.ForeColor = Color.White
        大小控制_中.ForeColor = Color.White
        大小控制_大.ForeColor = Color.White
    End Sub

    Private Sub 大小控制_MouseEnter(sender As Object, e As EventArgs) Handles 大小控制_小.MouseEnter, 大小控制_中.MouseEnter, 大小控制_大.MouseEnter
        '大小控制_小.BackColor = Color.DarkGray
        '大小控制_中.BackColor = Color.DarkGray
        '大小控制_大.BackColor = Color.DarkGray
        sender.BackColor = Color.Gray
    End Sub

    Private Sub 大小控制_MouseLeave(sender As Object, e As EventArgs) Handles 大小控制_小.MouseLeave, 大小控制_中.MouseLeave, 大小控制_大.MouseLeave
        sender.BackColor = Color.DarkGray
    End Sub

    Private Sub Node_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        按住坐标 = e.Location
    End Sub

    Private Sub Node_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
        If e.Button = MouseButtons.Left And 按住坐标.X <> 0 And 按住坐标.Y <> 0 Then
            Left += e.Location.X - 按住坐标.X
            Top += e.Location.Y - 按住坐标.Y
        End If
    End Sub
End Class