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
    Public Sub 候选构建(前缀类 As 文本定位类)
        Dim 前缀 As String = 前缀类.前缀
        候选表.Items.Clear()
        Dim 前缀集() As String = Split(前缀, ".")
        Dim 源节点 As 节点平面类.节点类
        If 前缀集.Length > 1 Then
            源节点 = 获得节点(前缀.Substring(0, 前缀.LastIndexOf(".")), 父域.当前编辑节点)
        Else
            源节点 = 父域.当前编辑节点
        End If
        If 源节点 Is Nothing Then
            If 前缀集.Length > 1 Then
                Dim 源平面 As 节点平面类 = 获得平面(前缀.Substring(0, 前缀.LastIndexOf(".")), 父域.当前编辑节点)
                If 源平面 IsNot Nothing Then
                    源平面候选表构建(前缀集, 源平面)
                End If
            End If
        Else
            源节点候选表构建(前缀集, 源节点)
        End If
    End Sub
    Private Sub 源平面候选表构建(前缀集() As String, 源平面 As 节点平面类)
        For i As Integer = 0 To 源平面.本域节点.Count - 1
            If 前缀集.Last = "" Or 源平面.本域节点.Keys(i).StartsWith(前缀集.Last) Then
                候选表.Items.Add(源平面.本域节点.Keys(i))
            End If
        Next
        For i As Integer = 0 To 源平面.全局平面.Count - 1
            If 前缀集.Last = "" Or 源平面.全局平面.Keys(i).StartsWith(前缀集.Last) Then
                候选表.Items.Add(源平面.全局平面.Keys(i))
            End If
        Next
        For i As Integer = 0 To 源平面.本域节点.Count - 1
            If 前缀集.Last = "" And Not 候选表.Items.Contains(源平面.本域节点.Keys(i)) Then
                候选表.Items.Add(源平面.本域节点.Keys(i))
            End If
        Next
        For i As Integer = 0 To 源平面.全局平面.Count - 1
            If 前缀集.Last = "" And Not 候选表.Items.Contains(源平面.全局平面.Keys(i)) Then
                候选表.Items.Add(源平面.全局平面.Keys(i))
            End If
        Next
        If 候选表.Items.Count > 0 Then
            候选表.SelectedIndex = 0
        ElseIf Visible Then
            Hide()
        End If
    End Sub
    Private Sub 源节点候选表构建(前缀集() As String, 源节点 As 节点平面类.节点类)
        For i As Integer = 0 To 源节点.连接.Count - 1
            If 前缀集.Last = "" Or 源节点.连接(i).名字.StartsWith(前缀集.Last) Then
                候选表.Items.Add(源节点.连接(i).名字)
            End If
        Next
        If 源节点.类型 = "引用" Then
            For i As Integer = 0 To 源节点.空间.Count - 1
                If 前缀集.Last = "" Or 源节点.空间.Keys(i).StartsWith(前缀集.Last) Then
                    候选表.Items.Add(源节点.空间.Keys(i))
                End If
            Next
        End If
        For i As Integer = 0 To 源节点.父域.全局窗体.全局节点列表.Items.Count - 1
            If 源节点.父域.本域节点.ContainsKey(源节点.父域.全局窗体.全局节点列表.Items(i)) Then
                If 前缀集.Last = "" Or 源节点.父域.全局窗体.全局节点列表.Items(i).StartsWith(前缀集.Last) Then
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
            If 前缀集.Last = "" And Not 候选表.Items.Contains(源节点.连接(i).名字) Then
                候选表.Items.Add(源节点.连接(i).名字)
            End If
        Next
        If 源节点.类型 = "引用" Then
            For i As Integer = 0 To 源节点.空间.Count - 1
                If 前缀集.Last = "" And Not 候选表.Items.Contains(源节点.空间.Keys(i)) Then
                    候选表.Items.Add(源节点.空间.Keys(i))
                End If
            Next
        End If
        For i As Integer = 0 To 源节点.父域.全局窗体.全局节点列表.Items.Count - 1
            If 前缀集.Last = "" And 源节点.父域.本域节点.ContainsKey(源节点.父域.全局窗体.全局节点列表.Items(i)) Then
                If Not 候选表.Items.Contains(源节点.父域.全局窗体.全局节点列表.Items(i)) Then
                    候选表.Items.Add(源节点.父域.全局窗体.全局节点列表.Items(i))
                End If
            End If
        Next
        For i As Integer = 0 To 源节点.父域.全局平面.Count - 1
            If 前缀集.Last = "" And Not 候选表.Items.Contains(源节点.父域.全局平面.Keys(i)) Then
                候选表.Items.Add(源节点.父域.全局平面.Keys(i))
            End If
        Next
        If 候选表.Items.Count > 0 Then
            候选表.SelectedIndex = 0
        ElseIf Visible Then
            Hide()
        End If
    End Sub
    Public Sub 出现()
        If 候选表.Items.Count > 0 Then
            Top = 编辑面.Top + 编辑面.Height
            If Top + Height > Screen.PrimaryScreen.Bounds.Height Then
                Top = 编辑面.Top - Height
            End If
            Left = 编辑面.Left
            Visible = True
            编辑面.Focus()
        End If
    End Sub
    Private Sub 编辑窗体隐藏(sender As Object, e As EventArgs) Handles 编辑面.VisibleChanged
        If Not 编辑面.Visible Then
            Visible = False
        End If
    End Sub
    Private Sub 插入内容(内容 As String, 后缀 As String)
        If Visible Then
            If 编辑面.节点内容.Text.IndexOf(".") <> -1 Or 编辑面.节点内容.Text.IndexOf(" ") <> -1 Then
                Dim 插入点缓存 As Long = 编辑面.节点内容.SelectionStart
                Dim 空格距离 As Long = 编辑面.节点内容.Text.Substring(0, 插入点缓存).LastIndexOf(" ") + 1
                Dim 点距离 As Long = 编辑面.节点内容.Text.Substring(0, 插入点缓存).LastIndexOf(".") + 1
                Dim 换行距离 As Long = 编辑面.节点内容.Text.Substring(0, 插入点缓存).LastIndexOf(vbCrLf) + 1
                Dim 开始位置 As Long
                If 点距离 > 空格距离 Then
                    开始位置 = 点距离
                Else
                    开始位置 = 空格距离
                End If
                If 换行距离 > 开始位置 Then
                    开始位置 = 换行距离 + 1
                End If
                编辑面.节点内容.SelectionStart = 开始位置
                编辑面.节点内容.SelectionLength = 插入点缓存 - 编辑面.节点内容.SelectionStart
            End If
            编辑面.节点内容.SelectedText = 内容 & 后缀
            Visible = False
        Else
            编辑面.节点内容.SelectedText = 后缀
        End If
    End Sub
    Public Sub 编辑窗体内容键入(sender As Object, e As KeyPressEventArgs) Handles 编辑面.KeyPress
        Select Case e.KeyChar
            Case vbTab, ".", " ", vbCrLf
                e.Handled = True
        End Select
    End Sub
    Public Sub 编辑窗体内容键盘弹起(sender As Object, e As KeyEventArgs) Handles 编辑面.KeyUp
        Select Case e.KeyCode
            Case Keys.Tab, Keys.Decimal, Keys.Enter, Keys.Space, Keys.Up, Keys.Down

            Case Else
                候选构建(获得前缀(编辑面.节点内容))
        End Select
    End Sub
    Public Sub 编辑窗体内容键盘按下(sender As Object, e As KeyEventArgs) Handles 编辑面.KeyDown
        Select Case e.KeyCode
            Case Keys.Space
                插入内容(候选表.SelectedItem, " ")
                候选构建(获得前缀(编辑面.节点内容))
                出现()
            Case Keys.Decimal
                插入内容(候选表.SelectedItem, ".")
                候选构建(获得前缀(编辑面.节点内容))
                出现()
            Case Keys.OemPeriod
                If e.Shift = False Then
                    插入内容(候选表.SelectedItem, ".")
                    候选构建(获得前缀(编辑面.节点内容))
                    出现()
                End If
            Case Keys.Tab
                If Visible Then
                    插入内容(候选表.SelectedItem, "")
                Else
                    候选构建(获得前缀(编辑面.节点内容))
                    出现()
                End If
            Case Keys.Enter
                插入内容(候选表.SelectedItem, vbCrLf)
            Case Keys.Up
                If Visible Then
                    If 候选表.SelectedIndex > 0 Then
                        候选表.SelectedIndex -= 1
                    Else
                        候选表.SelectedIndex = 候选表.Items.Count - 1
                    End If
                End If
            Case Keys.Down
                If Visible Then
                    If 候选表.SelectedIndex < 候选表.Items.Count - 1 Then
                        候选表.SelectedIndex += 1
                    Else
                        候选表.SelectedIndex = 0
                    End If
                End If
        End Select
    End Sub
End Class