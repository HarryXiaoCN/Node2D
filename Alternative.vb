Public Class Alternative
    Public 父域 As 节点平面类
    Public WithEvents 编辑面 As Node
    Private 忽略构建 As Boolean
    Public Sub New(ByRef parent As 节点平面类)

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        父域 = parent
        编辑面 = 父域.节点编辑窗体
    End Sub
    Public Sub 候选构建(前缀 As String)
        候选表.Items.Clear()
        Dim 前缀集() As String = Split(前缀, ".")
        Dim 源节点 As 节点平面类.节点类
        If 前缀集.Length > 1 Then
            源节点 = 获得节点(前缀.Substring(0, 前缀.IndexOf(".")), 父域.当前编辑节点)
        Else
            源节点 = 父域.当前编辑节点
        End If
        For i As Integer = 0 To 源节点.连接.Count - 1
            If 前缀集.Last = "" Or 源节点.连接(i).名字.StartsWith(前缀集.Last) Then
                候选表.Items.Add(源节点.连接(i).名字)
            End If
        Next
        For i As Integer = 0 To 源节点.父域.全局窗体.全局节点列表.Items.Count - 1
            If 源节点.父域.本域节点.ContainsKey(源节点.父域.全局窗体.全局节点列表.Items(i)) Then
                If 前缀集.Last = "" Or 源节点.父域.全局窗体.全局节点列表.Items(i).名字.StartsWith(前缀集.Last) Then
                    候选表.Items.Add(源节点.父域.全局窗体.全局节点列表.Items(i))
                End If
            End If
        Next
        For i As Integer = 0 To 源节点.父域.全局平面.Count - 1
            If 前缀集.Last = "" Or 源节点.父域.全局平面.Keys(i).StartsWith(前缀集.Last) Then
                候选表.Items.Add(源节点.父域.全局平面.Keys(i))
            End If
        Next
        For i As Integer = 0 To 源节点.连接.Count - 1
            If Not 候选表.Items.Contains(源节点.连接(i).名字) Then
                候选表.Items.Add(源节点.连接(i).名字)
            End If
        Next
        For i As Integer = 0 To 源节点.父域.全局窗体.全局节点列表.Items.Count - 1
            If 源节点.父域.本域节点.ContainsKey(源节点.父域.全局窗体.全局节点列表.Items(i)) Then
                If Not 候选表.Items.Contains(源节点.父域.全局窗体.全局节点列表.Items(i)) Then
                    候选表.Items.Add(源节点.父域.全局窗体.全局节点列表.Items(i))
                End If
            End If
        Next
        For i As Integer = 0 To 源节点.父域.全局平面.Count - 1
            If Not 候选表.Items.Contains(源节点.父域.全局平面.Keys(i)) Then
                候选表.Items.Add(源节点.父域.全局平面.Keys(i))
            End If
        Next
        If 候选表.Items.Count > 0 Then
            候选表.SelectedIndex = 0
        End If
    End Sub
    Public Function 获得前缀(文本域 As TextBox) As String
        Dim s() As String = Split(文本域.Text, vbCrLf)
        Dim 已长 As Long, 前缀 As String = "", 句首长 As Long, 句内已长 As Long ', 行 As Long
        For i As Long = 0 To UBound(s)
            句首长 = 已长
            已长 += s(i).Length + 1
            If 已长 >= 文本域.SelectionStart Then
                '行 = i
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
        编辑面.Focus()
    End Sub
    Private Sub 编辑窗体隐藏(sender As Object, e As EventArgs) Handles 编辑面.VisibleChanged
        If Not 编辑面.Visible Then
            Visible = False
        End If
    End Sub
    Public Sub 编辑窗体内容改变(sender As Object, e As KeyPressEventArgs) Handles 编辑面.KeyPress
        If 父域.当前编辑节点 Is Nothing Then Exit Sub
        Select Case e.KeyChar
            Case vbTab
                If Visible Then
                    Dim 插入点缓存 As Long = 编辑面.节点内容.SelectionStart
                    Dim 空格距离 As Long = 编辑面.节点内容.Text.LastIndexOf(" ") + 1
                    Dim 点距离 As Long = 编辑面.节点内容.Text.LastIndexOf(".") + 1
                    If 点距离 > 空格距离 Then
                        编辑面.节点内容.SelectionStart = 编辑面.节点内容.Text.LastIndexOf(".") + 1
                    Else
                        编辑面.节点内容.SelectionStart = 编辑面.节点内容.Text.LastIndexOf(" ") + 1
                    End If
                    编辑面.节点内容.SelectionLength = 插入点缓存 - 编辑面.节点内容.SelectionStart
                    编辑面.节点内容.SelectedText = 候选表.SelectedItem
                    Visible = False
                    e.Handled = True
                End If
                忽略构建 = True
        End Select
    End Sub
    Public Sub 编辑窗体内容键盘按下(sender As Object, e As KeyEventArgs) Handles 编辑面.KeyDown
        Select Case e.KeyCode
            Case Keys.Space
                候选构建(获得前缀(编辑面.节点内容))
                出现()
                忽略构建 = True
            Case Keys.Up
                If 候选表.SelectedIndex > 0 Then
                    候选表.SelectedIndex -= 1
                Else
                    候选表.SelectedIndex = 候选表.Items.Count - 1
                End If
                忽略构建 = True
            Case Keys.Down
                If 候选表.SelectedIndex < 候选表.Items.Count - 1 Then
                    候选表.SelectedIndex += 1
                Else
                    候选表.SelectedIndex = 0
                End If
                忽略构建 = True
        End Select
    End Sub

    Private Sub 编辑窗体内容键盘弹起(sender As Object, e As KeyEventArgs) Handles 编辑面.KeyUp
        Select Case e.KeyCode
            Case Keys.Decimal, Keys.OemPeriod
                候选构建(获得前缀(编辑面.节点内容))
                出现()
            Case Else
                If Visible And Not 忽略构建 Then
                    候选构建(获得前缀(编辑面.节点内容))
                End If
        End Select
        忽略构建 = False
    End Sub
End Class