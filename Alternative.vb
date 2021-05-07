Public Class Alternative
    Public 父域 As 节点平面类
    Public WithEvents 编辑面 As Node
    Public Sub New(ByRef parent As 节点平面类)

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        父域 = parent
        编辑面 = 父域.节点编辑窗体
    End Sub
    Public Sub 候选构建(前缀 As String)
        候选表.Items.Clear()

        For i As Integer = 0 To 父域.当前编辑节点.连接.Count - 1
            If 前缀 = "" Or 父域.当前编辑节点.连接(i).名字.StartsWith(前缀) Then
                候选表.Items.Add(父域.当前编辑节点.连接(i).名字)
            End If
        Next
        For i As Integer = 0 To 父域.全局窗体.全局节点列表.Items.Count - 1
            If 父域.本域节点.ContainsKey(父域.全局窗体.全局节点列表.Items(i)) Then
                If 前缀 = "" Or 父域.全局窗体.全局节点列表.Items(i).名字.StartsWith(前缀) Then
                    候选表.Items.Add(父域.全局窗体.全局节点列表.Items(i))
                End If
            End If
        Next
        For i As Integer = 0 To 父域.全局平面.Count - 1
            If 前缀 = "" Or 父域.全局平面.Keys(i).StartsWith(前缀) Then
                候选表.Items.Add(父域.全局平面.Keys(i))
            End If
        Next
        For i As Integer = 0 To 父域.当前编辑节点.连接.Count - 1
            If Not 候选表.Items.Contains(父域.当前编辑节点.连接(i).名字) Then
                候选表.Items.Add(父域.当前编辑节点.连接(i).名字)
            End If
        Next
        For i As Integer = 0 To 父域.全局窗体.全局节点列表.Items.Count - 1
            If 父域.本域节点.ContainsKey(父域.全局窗体.全局节点列表.Items(i)) Then
                If Not 候选表.Items.Contains(父域.全局窗体.全局节点列表.Items(i)) Then
                    候选表.Items.Add(父域.全局窗体.全局节点列表.Items(i))
                End If
            End If
        Next
        For i As Integer = 0 To 父域.全局平面.Count - 1
            If Not 候选表.Items.Contains(父域.全局平面.Keys(i)) Then
                候选表.Items.Add(父域.全局平面.Keys(i))
            End If
        Next
        候选表.SelectedIndex = 0
    End Sub
    Public Function 获得前缀(文本域 As TextBox) As String
        Dim s() As String = Split(文本域.Text, vbCrLf)
        Dim 已长 As Long, 行 As Long, 前缀 As String = "", 句首长 As Long, 句内已长 As Long
        For i As Long = 0 To UBound(s)
            句首长 = 已长
            已长 += s(i).Length + 1
            If 已长 >= 文本域.SelectionStart Then
                行 = i
                Dim s2() As String = Split(s(i), " ")
                For j As Long = 0 To UBound(s2)
                    句内已长 += s2(j).Length + 1
                    If 句首长 + 句内已长 >= 文本域.SelectionStart Then
                        前缀 = s2(j)
                    End If
                Next
                Exit For
            End If
        Next
        Return 前缀
    End Function
    Public Sub 出现()
        Top = 编辑面.Top + 编辑面.Height
        If Top + Height > Screen.PrimaryScreen.Bounds.Height Then
            Top = 编辑面.Top - Height
        End If
        Left = 编辑面.Left
        Visible = True

    End Sub
    Private Sub 编辑窗体隐藏(sender As Object, e As EventArgs) Handles 编辑面.VisibleChanged
        If Not 编辑面.Visible Then
            Visible = False
        End If
    End Sub
    Public Sub 编辑窗体内容改变(sender As Object, e As KeyPressEventArgs) Handles 编辑面.KeyPress
        If 父域.当前编辑节点 Is Nothing Then Exit Sub
        Select Case e.KeyChar
            Case " "
                候选构建(获得前缀(编辑面.节点内容))
                出现()
            Case vbTab
                If Visible Then
                    编辑面.节点内容.SelectedText = 候选表.SelectedItem
                    Visible = False
                    e.Handled = True
                End If
            Case Else
                If Visible Then
                    候选构建(获得前缀(编辑面.节点内容))
                End If
        End Select
    End Sub
End Class