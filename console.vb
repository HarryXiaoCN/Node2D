Imports System.ComponentModel

Public Class NodeConsole
    Private Delegate Sub 添加消息委托(info As String)

    Public Sub 添加消息(info As String)
        Dim d As New 添加消息委托(AddressOf 添加消息执行)
        Invoke(d, info)
    End Sub
    Private Sub 添加消息执行(info As String)
        If info <> "" Then
            控制输出.SelectionStart = 控制输出.TextLength
            控制输出.SelectedText = info & vbCrLf
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
        控制输出.SelectionStart = 0
        控制输出.SelectionLength = 控制输出.TextLength
    End Sub

    Private Sub 清空文本_Click(sender As Object, e As EventArgs) Handles 清空文本.Click
        控制输出.Text = ""
    End Sub
End Class