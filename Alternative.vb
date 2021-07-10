Public Class Alternative
    Public 父域 As 节点平面类
    Public WithEvents 编辑面 As Node
    Public WithEvents 节点内容 As TextBox
    Public 候选表 As New Dictionary(Of String, Integer)
    Public 候选指针 As Integer = 0
    Public 视图指针 As Integer = 0
    Public 候选节点 As 节点平面类.节点类
    Private 滚动条 As New Rectangle
    Private 滚动条锁定 As Boolean
    Private 滚动条悬浮 As Boolean
    Private 鼠标起点 As Point
    Public 法则提示模式 As Integer
    Public Sub New(ByRef parent As 节点平面类)

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        父域 = parent
        编辑面 = 父域.节点编辑窗体
        节点内容 = 编辑面.节点内容
    End Sub
    Public Sub 候选构建(前缀类 As 文本定位类)
        Dim 前缀 As String = 前缀类.前缀
        候选表.Clear()
        视图指针 = 0
        候选指针变动(0)
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
                    源平面候选表构建(前缀类, 前缀集, 源平面)
                End If
            End If
        Else
            源节点候选表构建(前缀类, 前缀集, 源节点)
        End If
        '父域.法则提示窗口.展示(前缀类.行首)
    End Sub
    Private Function 获得候选项对应节点(前缀类 As 文本定位类) As 节点平面类.节点类
        If 候选指针 < 0 Then Return Nothing
        Dim 前缀 As String = 前缀类.前缀
        Dim 前缀集() As String = Split(前缀, ".")
        Dim 源节点 As 节点平面类.节点类
        If 前缀集.Length > 1 Then
            源节点 = 获得节点(前缀.Substring(0, 前缀.LastIndexOf(".")) & "." & 候选表.Keys(候选指针), 父域.当前编辑节点)
        Else
            源节点 = 获得节点(候选表.Keys(候选指针), 父域.当前编辑节点)
        End If
        Return 源节点
    End Function
    Private Shared Function 获得节点候选项类型(n As 节点平面类.节点类) As Integer
        Select Case n.类型
            Case "值"
                Return 5
            Case "函数"
                Return 1
            Case "接口"
                Return 2
            Case "引用"
                Return 4
            Case Else
                Return 6
        End Select
    End Function
    Private Sub 源平面候选表构建(前缀类 As 文本定位类, 前缀集() As String, 源平面 As 节点平面类)
        If 前缀类.行首 = 前缀类.前缀 Then
            法则提示模式 = 0
            For i As Integer = 0 To 精简法则集.Count - 1
                If 精简法则集(i).StartsWith(前缀类.行首) Then
                    添加项(精简法则集(i), 0)
                End If
            Next
        Else
            法则提示模式 = 1
            父域.法则提示窗口.展示(前缀类.行首)
        End If
        For i As Integer = 0 To 源平面.本域节点.Count - 1
            If 前缀集.Last = "" Or 源平面.本域节点.Keys(i).StartsWith(前缀集.Last) Then
                添加项(源平面.本域节点.Keys(i), 获得节点候选项类型(源平面.本域节点.Values(i)))
            End If
        Next
        For i As Integer = 0 To 源平面.全局平面.Count - 1
            If 前缀集.Last = "" Or 源平面.全局平面.Keys(i).StartsWith(前缀集.Last) Then
                添加项(源平面.全局平面.Keys(i), 3)
            End If
        Next
        For i As Integer = 0 To 源平面.本域节点.Count - 1
            If 前缀集.Last = "" And Not 候选表.ContainsKey(源平面.本域节点.Keys(i)) Then
                添加项(源平面.本域节点.Keys(i), 获得节点候选项类型(源平面.本域节点.Values(i)))
            End If
        Next
        For i As Integer = 0 To 源平面.全局平面.Count - 1
            If 前缀集.Last = "" And Not 候选表.ContainsKey(源平面.全局平面.Keys(i)) Then
                添加项(源平面.全局平面.Keys(i), 3)
            End If
        Next
        If 候选表.Count > 0 Then
            If Visible = False Then
                出现()
            End If
            候选指针变动(0)
        ElseIf Visible Then
            Hide()
        End If
    End Sub
    Private Sub 源节点候选表构建(前缀类 As 文本定位类, 前缀集() As String, 源节点 As 节点平面类.节点类)
        If 前缀类.行首 = 前缀类.前缀 Then
            法则提示模式 = 0
            For i As Integer = 0 To 精简法则集.Count - 1
                If 精简法则集(i).StartsWith(前缀类.行首) Then
                    添加项(精简法则集(i), 0)
                End If
            Next
        Else
            法则提示模式 = 1
            父域.法则提示窗口.展示(前缀类.行首)
        End If
        For i As Integer = 0 To 源节点.连接.Count - 1
            If 前缀集.Last = "" Or 源节点.连接(i).名字.StartsWith(前缀集.Last) Then
                添加项(源节点.连接(i).名字, 获得节点候选项类型(源节点.连接(i)))
            End If
        Next
        If 源节点.类型 = "引用" Then
            For i As Integer = 0 To 源节点.空间.Count - 1
                If 前缀集.Last = "" Or 源节点.空间.Keys(i).StartsWith(前缀集.Last) Then
                    添加项(源节点.空间.Keys(i), 3)
                End If
            Next
        End If
        For i As Integer = 0 To 源节点.父域.全局窗体.全局节点列表.Items.Count - 1
            If 源节点.父域.本域节点.ContainsKey(源节点.父域.全局窗体.全局节点列表.Items(i)) Then
                If 前缀集.Last = "" Or 源节点.父域.全局窗体.全局节点列表.Items(i).StartsWith(前缀集.Last) Then
                    Dim n As 节点平面类.节点类 = 获得节点(源节点.父域.全局窗体.全局节点列表.Items(i), 源节点)
                    添加项(源节点.父域.全局窗体.全局节点列表.Items(i), 获得节点候选项类型(n))
                End If
            End If
        Next
        For i As Integer = 0 To 源节点.父域.全局平面.Count - 1
            If 前缀集.Last = "" Or 源节点.父域.全局平面.Keys(i).StartsWith(前缀集.Last) Then
                添加项(源节点.父域.全局平面.Keys(i), 3)
            End If
        Next
        For i As Integer = 0 To 源节点.连接.Count - 1
            If 前缀集.Last = "" And Not 候选表.ContainsKey(源节点.连接(i).名字) Then
                添加项(源节点.连接(i).名字, 获得节点候选项类型(源节点.连接(i)))
            End If
        Next
        If 源节点.类型 = "引用" Then
            For i As Integer = 0 To 源节点.空间.Count - 1
                If 前缀集.Last = "" And Not 候选表.ContainsKey(源节点.空间.Keys(i)) Then
                    添加项(源节点.空间.Keys(i), 3)
                End If
            Next
        End If
        For i As Integer = 0 To 源节点.父域.全局窗体.全局节点列表.Items.Count - 1
            If 前缀集.Last = "" And 源节点.父域.本域节点.ContainsKey(源节点.父域.全局窗体.全局节点列表.Items(i)) Then
                If Not 候选表.ContainsKey(源节点.父域.全局窗体.全局节点列表.Items(i)) Then
                    Dim n As 节点平面类.节点类 = 获得节点(源节点.父域.全局窗体.全局节点列表.Items(i), 源节点)
                    添加项(源节点.父域.全局窗体.全局节点列表.Items(i), 获得节点候选项类型(n))
                End If
            End If
        Next
        For i As Integer = 0 To 源节点.父域.全局平面.Count - 1
            If 前缀集.Last = "" And Not 候选表.ContainsKey(源节点.父域.全局平面.Keys(i)) Then
                添加项(源节点.父域.全局平面.Keys(i), 3)
            End If
        Next
        If 候选表.Count > 0 Then
            If Visible = False Then
                出现()
            End If
            候选指针变动(0)
        ElseIf Visible Then
            Hide()
        End If
    End Sub
    Public Sub 添加项(提示 As List(Of String), 图索引 As Integer)
        For i As Integer = 0 To 提示.Count - 1
            If Not 候选表.ContainsKey(提示(i)) Then
                候选表.Add(提示(i)， 图索引)
            End If
        Next
    End Sub
    Public Sub 添加项(提示 As String, 图索引 As Integer)
        If Not 候选表.ContainsKey(提示) Then
            候选表.Add(提示， 图索引)
        End If
    End Sub
    Public Sub 出现()
        If 候选表.Count > 0 Then
            Top = 编辑面.Top + 编辑面.Height
            If Top + Height > Screen.PrimaryScreen.Bounds.Height Then
                Top = 编辑面.Top - Height
            End If
            Left = 编辑面.Left
            Visible = True
        End If
    End Sub
    Private Sub 编辑窗体隐藏(sender As Object, e As EventArgs) Handles 编辑面.VisibleChanged
        If 编辑面.Visible Then
            Visible = False
        End If
    End Sub
    Private Sub 插入内容(内容 As String, 后缀 As String)
        If Visible Then
            'If 编辑面.节点内容.Text.IndexOf(".") <> -1 Or 编辑面.节点内容.Text.IndexOf(" ") <> -1 Then
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
            'End If
            编辑面.节点内容.SelectedText = 内容 & 后缀
            Visible = False
        Else
            编辑面.节点内容.SelectedText = 后缀
        End If
    End Sub
    Public Sub 编辑窗体内容键入(sender As Object, e As KeyPressEventArgs) Handles 节点内容.KeyPress, Me.KeyPress
        Select Case e.KeyChar
            Case vbTab, ".", " ", vbCrLf
                e.Handled = True
        End Select
    End Sub

    Public Sub 候选内容准备(内容 As TextBox)
        候选构建(获得前缀(内容))
    End Sub

    Public Sub 节点内容点击() Handles 节点内容.MouseClick
        候选内容准备(编辑面.节点内容)
        绘制候选表()
        节点内容.Focus()
    End Sub

    Protected Overrides ReadOnly Property ShowWithoutActivation As Boolean
        Get
            Return True
        End Get
    End Property
    Public Sub 编辑窗体内容键盘弹起(sender As Object, e As KeyEventArgs) Handles 节点内容.KeyUp, Me.KeyUp
        Select Case e.KeyCode
            Case Keys.Tab, Keys.Decimal, Keys.Enter, Keys.Space, Keys.Up, Keys.Down

            Case Else
                候选内容准备(编辑面.节点内容)
                绘制候选表()
        End Select
    End Sub
    Public Sub 编辑窗体内容键盘按下(sender As Object, e As KeyEventArgs) Handles 节点内容.KeyDown, Me.KeyDown
        Select Case e.KeyCode
            Case Keys.Space
                插入内容(候选表.Keys(候选指针), " ")
                候选内容准备(编辑面.节点内容)
                出现()
            Case Keys.Decimal
                插入内容(候选表.Keys(候选指针), ".")
                候选内容准备(编辑面.节点内容)
                出现()
            Case Keys.OemPeriod
                If e.Shift = False Then
                    插入内容(候选表.Keys(候选指针), ".")
                    候选内容准备(编辑面.节点内容)
                    出现()
                End If
            Case Keys.Tab
                If Visible Then
                    插入内容(候选表.Keys(候选指针), "")
                Else
                    候选内容准备(编辑面.节点内容)
                    出现()
                End If
            Case Keys.Enter
                插入内容(候选表.Keys(候选指针), vbCrLf)
            Case Keys.Up
                If Visible Then
                    If 候选指针 > 0 Then
                        候选指针变动(候选指针 - 1)
                    Else
                        候选指针变动(候选表.Count - 1)
                    End If
                End If
            Case Keys.Down
                If Visible Then
                    If 候选指针 < 候选表.Count - 1 Then
                        候选指针变动(候选指针 + 1)
                    Else
                        候选指针变动(0)
                    End If
                End If
        End Select
        绘制候选表()
    End Sub
    Private Sub 候选指针变动(v As Integer)
        候选指针 = v
        If 候选指针 < 0 Then
            候选指针 = 0
        End If
        If 候选指针 > 候选表.Count - 1 Then
            候选指针 = 候选表.Count - 1
        End If
        If 候选指针 = 0 Then
            视图指针 = 0
        ElseIf 候选指针 = 候选表.Count - 1 And 候选指针 > 候选域.Height \ FontHeight Then
            视图指针 = 候选表.Count - (候选域.Height \ FontHeight)
        ElseIf 候选指针 * FontHeight + FontHeight > 视图指针 * FontHeight + 候选域.Height Then
            视图指针 += 1
        ElseIf 候选指针 * FontHeight < 视图指针 * FontHeight Then
            视图指针 -= 1
        End If
        If 候选节点 IsNot Nothing Then
            候选节点.候选 = False
        End If
        候选节点 = 获得候选项对应节点(获得前缀(编辑面.节点内容))
        If 候选节点 IsNot Nothing Then
            候选节点.候选 = True
        End If
        If 候选指针 >= 0 Then
            If 法则提示模式 = 0 Then
                If 候选表.Values(候选指针) = 0 Then
                    父域.法则提示窗口.展示(候选表.Keys(候选指针))
                ElseIf 父域.法则提示窗口.Visible Then
                    父域.法则提示窗口.Visible = False
                End If
            End If
        End If
    End Sub

    Private Sub 绘制候选表()
        If 候选表.Count = 0 Then Exit Sub
        Dim img As New Bitmap(候选域.Width, 候选域.Height)
        Dim g As Graphics = Graphics.FromImage(img)
        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
        Dim fS As SizeF = g.MeasureString(候选表.Keys(0), Font)
        Dim sumHeight As Single = 候选表.Count * fS.Height
        Dim gdtHeight As Single = img.Height / sumHeight * img.Height

        g.TranslateTransform(0, -视图指针 * fS.Height)
        For i As Integer = 0 To 候选表.Count - 1
            Dim nowY As Single = i * fS.Height
            If i = 候选指针 Then
                g.FillRectangle(Brushes.SpringGreen, 26, nowY, img.Width, fS.Height)
            End If
            g.DrawImage(类型图标表.Images(候选表.Values(i)), 0, nowY)
            g.DrawString(候选表.Keys(i), Font, Brushes.Black, 26, nowY)
        Next
        g.ResetTransform()
        If gdtHeight < img.Height Then
            g.FillRectangle(Brushes.WhiteSmoke, img.Width - 12, 0, img.Width, img.Height)
            滚动条 = New Rectangle(img.Width - 10, (视图指针 * fS.Height / sumHeight) * img.Height, 6, gdtHeight)
            If 滚动条锁定 Then
                g.FillRectangle(Brushes.Turquoise, 滚动条)
            ElseIf 滚动条悬浮 = False Then
                g.FillRectangle(Brushes.LimeGreen, 滚动条)
            Else
                g.FillRectangle(Brushes.MediumAquamarine, 滚动条)
            End If
        End If

        If 候选域.Image IsNot Nothing Then
            候选域.Image.Dispose()
        End If
        候选域.Image = img
        候选域.Refresh()
        节点内容.Focus()
    End Sub

    Private Function 获得鼠标指中条目(p As Point) As Integer
        If 候选表.Count = 0 Then Return -1
        Return p.Y \ FontHeight + 视图指针
    End Function

    Private Sub 候选域_MouseDown(sender As Object, e As MouseEventArgs) Handles 候选域.MouseDown
        If e.X < 候选域.Width - 12 Then
            Dim aim As Integer = 获得鼠标指中条目(e.Location)
            If 候选指针 = aim Then
                插入内容(候选表.Keys(候选指针), "")
            Else
                候选指针变动(aim)
            End If
        Else
            If e.X >= 滚动条.X And e.X <= 滚动条.X + 滚动条.Width And e.Y >= 滚动条.Y And e.Y <= 滚动条.Y + 滚动条.Height Then
                鼠标起点 = e.Location
                滚动条锁定 = True
            End If
        End If
        绘制候选表()
    End Sub
    Private Sub 候选域_MouseMove(sender As Object, e As MouseEventArgs) Handles 候选域.MouseMove
        If 鼠标起点.X <> 0 And 鼠标起点.Y <> 0 Then
            Dim rowHeight As Single = 候选域.Height / 候选表.Count
            If e.Y > 鼠标起点.Y + rowHeight Then
                视图指针 += 1
                鼠标起点 = e.Location
            ElseIf e.Y < 鼠标起点.Y - rowHeight Then
                视图指针 -= 1
                鼠标起点 = e.Location
            End If
            If 视图指针 < 0 Then 视图指针 = 0
            If 视图指针 > 候选表.Count - (候选域.Height \ FontHeight) Then 视图指针 = 候选表.Count - (候选域.Height \ FontHeight)
        ElseIf e.X >= 滚动条.X And e.X <= 滚动条.X + 滚动条.Width And e.Y >= 滚动条.Y And e.Y <= 滚动条.Y + 滚动条.Height Then
            滚动条悬浮 = True
        Else
            滚动条悬浮 = False
        End If
        绘制候选表()
    End Sub

    Private Sub 编辑面_MouseWheel(sender As Object, e As MouseEventArgs) Handles Me.MouseWheel
        候选指针变动(候选指针 - e.Delta \ 120)
        绘制候选表()
    End Sub

    Private Sub 候选域_MouseUp(sender As Object, e As MouseEventArgs) Handles 候选域.MouseUp
        滚动条锁定 = False
        鼠标起点 = New Point
        绘制候选表()
    End Sub

    Private Sub 候选域_MouseLeave(sender As Object, e As EventArgs) Handles 候选域.MouseLeave
        滚动条悬浮 = False
        绘制候选表()
    End Sub

    Private Sub Alternative_VisibleChanged(sender As Object, e As EventArgs) Handles MyBase.VisibleChanged
        If Visible = False Then
            If 候选节点 IsNot Nothing Then
                候选节点.候选 = False
                候选节点 = Nothing
            End If
            父域.法则提示窗口.Hide()
        End If
    End Sub
End Class