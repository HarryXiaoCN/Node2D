Imports System.Threading
Imports System.Text.RegularExpressions
Imports System.IO
Public Class 节点平面类
    Public Class 节点类
        Private name As String
        Private type As String
        Private content As String
        Public 位置 As Point
        Public 父域 As 节点平面类
        Public 引用等级 As Integer '0:一次引用 1:本次引用 2:永久引用
        Public 连接 As New List(Of 节点类)
        Private ReadOnly 待连接 As New List(Of String)
        Public 空间 As New Dictionary(Of String, 节点平面类)
        Public Event 改变前(node As 节点类)
        Public Event 改变后(node As 节点类)
        Public Sub New(ByRef parent As 节点平面类, nodeName As String, nodeType As String, nodeContent As String, nodePos As Point)
            父域 = parent
            name = nodeName
            type = nodeType
            content = nodeContent
            位置 = nodePos
            父域.空间限制.Add(位置, Me)
            重构空间()
        End Sub
        Public Sub New(ByRef parent As 节点平面类, nodeString As String)
            父域 = parent
            Dim s() As String = Split(nodeString, Chr(1))
            name = s(0)
            type = s(1)
            content = Replace(s(2), Chr(2), vbCrLf)
            位置 = New Point(Val(s(3)), Val(s(4)))
            If s(5) <> "" Then
                待连接.AddRange(Split(s(5), Chr(2)))
            End If
            父域.本域节点.Add(name, Me)
            父域.空间限制.Add(位置, Me)
            重构空间()
        End Sub
        Public Function 获得子节点(节点名 As String) As 节点类
            For i As Integer = 0 To 连接.Count - 1
                If 连接(i).名字 = 节点名 Then
                    Return 连接(i)
                End If
            Next
            For i As Integer = 0 To 父域.全局窗体.全局节点列表.Items.Count - 1
                If 父域.全局窗体.全局节点列表.Items(i) = 节点名 And 父域.本域节点.ContainsKey(节点名) Then
                    Return 父域.本域节点(节点名)
                End If
            Next
            Return Nothing
        End Function
        Public Sub 确认连接()
            For i As Integer = 0 To 待连接.Count - 1
                连接.Add(父域.本域节点(待连接(i)))
            Next
        End Sub
        Public Overloads Function ToString() As String
            Dim 连接名集 As New List(Of String)
            For i As Integer = 0 To 连接.Count - 1
                连接名集.Add(连接(i).名字)
            Next
            Return name & Chr(1) & type & Chr(1) & Replace(content, vbCrLf, Chr(2)) & Chr(1) & 位置.X & Chr(1) & 位置.Y _
                & Chr(1) & Join(连接名集.ToArray, Chr(2))
        End Function
        Public Property 名字() As String
            Get
                Return name
            End Get
            Set(value As String)
                RaiseEvent 改变前(Me)
                name = value
                RaiseEvent 改变后(Me)
            End Set
        End Property
        Public Property 类型() As String
            Get
                Return type
            End Get
            Set(value As String)
                RaiseEvent 改变前(Me)
                type = value
                RaiseEvent 改变后(Me)
            End Set
        End Property
        Private Sub 重构空间()
            If type = "引用" Then
                空间.Clear()
                Dim sT() As String = Split(content, vbCrLf)
                For i As Integer = 0 To UBound(sT)
                    Dim filePath As String = Path.GetFullPath(sT(i), 父域.路径)
                    If File.Exists(filePath) Then
                        Dim fileName As String = Path.GetFileNameWithoutExtension(filePath)
                        If 空间.ContainsKey(fileName) Then
                            控制台.添加消息(String.Format("引用节点[{0}]内第{2}行：引用空间""{1}""已存在。", name, filePath, i + 1))
                        Else
                            Try
                                空间.Add(fileName, New 节点平面类(filePath))
                            Catch ex As Exception
                                控制台.添加消息(String.Format("引用节点[{0}]内第{1}行：初始化引用平面[{2}]错误，原因：{3}", name, i + 1, filePath, ex.Message))
                            End Try
                        End If
                    Else
                        控制台.添加消息(String.Format("引用节点[{0}]内第{2}行：引用空间""{1}""不存在。", name, filePath, i + 1))
                    End If
                Next
            End If
        End Sub
        Public Property 内容() As String
            Get
                Return content
            End Get
            Set(value As String)
                RaiseEvent 改变前(Me)
                content = value
                重构空间()
                RaiseEvent 改变后(Me)
            End Set
        End Property
        Public Sub 删除()
            For Each n As 节点类 In 连接
                n.连接.Remove(Me)
            Next
            父域.本域节点.Remove(名字)
            父域.空间限制.Remove(位置)
        End Sub
    End Class
    Public 本域节点 As New Dictionary(Of String, 节点类)
    Public 空间限制 As New Dictionary(Of Point, 节点类)
    Public 全局平面 As New Dictionary(Of String, 节点平面类)
    Public 主平面 As Boolean
    Public 结束标识 As Boolean
    Public 绘制间隔 As Long = 15
    Public 缩放倍数 As Integer = 50
    Public 主窗体 As Form1
    Public 全局窗体 As New GlobalImportForm
    Public 鼠标移动选中节点 As 节点类
    Public 当前编辑节点 As 节点类
    Public 当前按住节点 As 节点类
    Public 连接发起节点 As 节点类
    Public 节点创建模式 As String = "值"
    Public 节点编辑窗体 As New Node
    Public 鼠标当前位置 As Point
    Public 鼠标实际位置 As Point

    Public 视角偏移 As Point
    Public 节点移动填充色_安全 As New SolidBrush(Color.FromArgb(180, Color.LightGreen))
    Public 节点移动填充色_危险 As New SolidBrush(Color.FromArgb(180, Color.OrangeRed))
    Public 节点移动边缘色 As New Pen(Color.FromArgb(180, Color.Cyan), 3)
    Public 连接颜色_有效 As New Pen(Color.FromArgb(180, Color.LightGreen), 10)
    Public 连接颜色_待定 As New Pen(Color.FromArgb(180, Color.Gray), 10)
    Public 连接颜色_无效 As New Pen(Color.FromArgb(180, Color.OrangeRed), 10)
    Public 函数节点填充色 As Color = Color.LimeGreen
    Public 引用节点填充色 As Color = Color.DeepSkyBlue
    Public 值节点填充颜色 As Color = Color.Gold
    Public 函数节点连接色 As New Pen(Color.FromArgb(180, 函数节点填充色), 10)
    Public 引用节点连接色 As New Pen(Color.FromArgb(180, 引用节点填充色), 10)
    Public 连接起点颜色 As Color = Color.Purple
    Public WithEvents 绘制空间 As PictureBox
    Public 版本 As String = "NODE2D.20210430.1"
    Public 路径 As String = Application.StartupPath
    Public 平面名 As String = ""

    Private 视角拖拽起点 As Point
    Private 视角拖拽 As Boolean
    Private ReadOnly 节点删除列表 As New List(Of 节点类)
    Private ReadOnly 绘制线程 As Thread
    Private Delegate Sub 绘制更新委托(ByRef 帧 As Image)
    Public Sub New(ByRef mainForm As Form1, ByRef drawSpace As PictureBox, Optional mainNode2D As Boolean = True)
        主窗体 = mainForm
        绘制空间 = drawSpace
        主平面 = mainNode2D
        节点编辑窗体.主域 = Me
        '节点编辑窗体.Show()
        '节点编辑窗体.Hide()
        If 主平面 Then
            主窗体.Text = "节点平面 - " & 路径
            绘制线程 = New Thread(AddressOf 绘制)
            绘制线程.Start()
        End If
    End Sub
    Public Sub New(路径 As String)
        加载(路径)
    End Sub

    Public Function 获得平面路径() As String
        Return 路径 & 平面名
    End Function
    Public Sub 路径赋予(filePath As String)
        路径 = Path.GetDirectoryName(filePath)
        If Not 路径.EndsWith("\") Then
            路径 &= "\"
        End If
        平面名 = Path.GetFileName(filePath)
        If 主平面 Then
            主窗体.Text = "节点平面 - " & 路径 & 平面名
        End If
    End Sub

    Public Sub 加载(filePath As String)
        Dim s() As String = Split(File.ReadAllText(filePath), vbCrLf)
        If s(0) = "NODE2D.20210424.1" Then
            Load_NODE2D_20210424_1(filePath, s)
        ElseIf s(0) = "NODE2D.20210430.1" Then
            路径赋予(filePath)
            Dim p() As String = Split(s(1), " ")
            视角偏移 = New Point(Val(p(0)), Val(p(1)))
            Dim gS() As String = Split(s(2), Chr(1))
            全局窗体.全局平面列表.Items.AddRange(gS)
            For i As Integer = 0 To UBound(gS)
                Dim readPath As String = Path.GetFullPath(gS(i), 路径)
                If File.Exists(readPath) Then
                    Dim fileName As String = Path.GetFileNameWithoutExtension(readPath)
                    If Not 全局平面.ContainsKey(fileName) Then
                        Try
                            全局平面.Add(fileName, New 节点平面类(readPath))
                        Catch ex As Exception
                            控制台.添加消息(String.Format("添加全局平面[{0}]时错误，原因：{1}", gS(i), ex.Message))
                        End Try
                    End If
                End If
            Next
            Dim nS() As String = Split(s(3), Chr(1))
            全局窗体.全局节点列表.Items.AddRange(nS)
            For i = 4 To UBound(s)
                Dim node As New 节点类(Me, s(i))
            Next
            For Each n As 节点类 In 本域节点.Values
                n.确认连接()
            Next
        End If
    End Sub
    Private Sub Load_NODE2D_20210424_1(filePath As String, s() As String)
        路径赋予(filePath)
        Dim p() As String = Split(s(1), " ")
        视角偏移 = New Point(Val(p(0)), Val(p(1)))
        For i = 2 To UBound(s)
            Dim node As New 节点类(Me, s(i))
        Next
        For Each n As 节点类 In 本域节点.Values
            n.确认连接()
        Next
    End Sub
    Public Sub 添加全局平面(filePath As String)
        Dim readPath As String = Path.GetFullPath(filePath, 路径)
        If File.Exists(readPath) Then
            Dim fileName As String = Path.GetFileNameWithoutExtension(readPath)
            Try
                全局平面.Add(fileName, New 节点平面类(readPath))
                全局窗体.全局平面列表.Items.Add(filePath)
            Catch ex As Exception
                控制台.添加消息(String.Format("添加全局平面[{0}]时错误，原因：{1}", filePath, ex.Message))
            End Try
        Else
            控制台.添加消息(String.Format("未找到全局平面[{0}]（{1}），确认平面路径后重载当前平面", filePath, readPath))
        End If
    End Sub
    Public Sub 移除全局平面()
        If 全局窗体.全局平面列表.SelectedIndex >= 0 And 全局窗体.全局平面列表.SelectedIndex < 全局窗体.全局平面列表.Items.Count Then
            全局平面.Remove(全局窗体.全局平面列表.Items(全局窗体.全局平面列表.SelectedIndex))
            全局窗体.全局平面列表.Items.RemoveAt(全局窗体.全局平面列表.SelectedIndex)
        End If
    End Sub

    Public Sub 保存(filePath As String)
        Dim result As New List(Of String) From {
            版本
        }
        result.Add(String.Format("{0} {1}", 视角偏移.X, 视角偏移.Y))
        Dim 全局平面路径(全局窗体.全局平面列表.Items.Count - 1) As String
        全局窗体.全局平面列表.Items.CopyTo(全局平面路径, 0)
        result.Add(Join(全局平面路径.ToArray, Chr(1)))
        Dim 全局节点路径(全局窗体.全局节点列表.Items.Count - 1) As String
        全局窗体.全局节点列表.Items.CopyTo(全局节点路径, 0)
        result.Add(Join(全局节点路径.ToArray, Chr(1)))
        For i As Integer = 0 To 本域节点.Count - 1
            result.Add(本域节点.Values(i).ToString)
        Next

        File.WriteAllText(filePath, Join(result.ToArray, vbCrLf))
        路径赋予(filePath)
    End Sub
    Private Sub 鼠标双击事件(sender As Object, e As EventArgs) Handles 绘制空间.DoubleClick
        If 鼠标移动选中节点 IsNot Nothing Then
            节点删除列表.Add(鼠标移动选中节点)
            鼠标移动选中节点 = Nothing
            当前编辑节点 = Nothing
            当前按住节点 = Nothing
            连接发起节点 = Nothing
        End If
    End Sub
    Private Sub 鼠标位置获取(p As Point)
        鼠标实际位置 = p
        鼠标实际位置.Offset(-视角偏移.X, -视角偏移.Y)
        鼠标当前位置 = New Point(鼠标实际位置.X \ 缩放倍数, 鼠标实际位置.Y \ 缩放倍数)
        If 鼠标实际位置.X < 0 Then
            鼠标当前位置.X -= 1
        End If
        If 鼠标实际位置.Y < 0 Then
            鼠标当前位置.Y -= 1
        End If
    End Sub
    Private Sub 鼠标点击事件(sender As Object, e As MouseEventArgs) Handles 绘制空间.MouseDown
        鼠标位置获取(e.Location)
        If e.Button = MouseButtons.Right Then
            新建节点(String.Format("{0}-{1}-{2}", 本域节点.Count, 鼠标当前位置.X, 鼠标当前位置.Y), 鼠标当前位置)
        ElseIf e.Button = MouseButtons.Left Then
            当前按住节点 = 位置到节点(鼠标当前位置)
            If 当前按住节点 IsNot Nothing Then
                If 连接发起节点 IsNot Nothing Then
                    If 连接有效性判断(当前按住节点, 连接发起节点) Then
                        If 当前按住节点.连接.Contains(连接发起节点) Then
                            当前按住节点.连接.Remove(连接发起节点)
                        Else
                            当前按住节点.连接.Add(连接发起节点)
                        End If
                        If 连接发起节点.连接.Contains(当前按住节点) Then
                            连接发起节点.连接.Remove(当前按住节点)
                        Else
                            连接发起节点.连接.Add(当前按住节点)
                        End If
                        '连接发起节点 = Nothing
                    End If
                End If
                If 节点编辑窗体.Visible Then 节点编辑窗体.Visible = False
            Else
                连接发起节点 = Nothing
                视角拖拽 = True
                视角拖拽起点 = e.Location
            End If
        End If
    End Sub
    Private Sub 鼠标点完事件(sender As Object, e As MouseEventArgs) Handles 绘制空间.MouseUp
        鼠标位置获取(e.Location)
        If 当前按住节点 IsNot Nothing Then
            If 当前按住节点.位置 = 鼠标当前位置 Then
                If 连接发起节点 Is Nothing Then 连接发起节点 = 当前按住节点
                If 节点编辑窗体.Visible Then 节点编辑窗体.Visible = False
            Else
                编辑节点位置(当前按住节点.名字, 鼠标当前位置)
            End If
            当前按住节点 = Nothing
        End If
        视角拖拽 = False
    End Sub

    Private Sub 鼠标移动事件(sender As Object, e As MouseEventArgs) Handles 绘制空间.MouseMove
        鼠标位置获取(e.Location)
        If 视角拖拽 Then
            视角偏移.Offset(e.Location - 视角拖拽起点)
            视角拖拽起点 = e.Location
        Else
            If 当前按住节点 Is Nothing Then
                鼠标移动选中节点 = 位置到节点(鼠标当前位置)
                If 鼠标移动选中节点 IsNot Nothing And 连接发起节点 Is Nothing Then
                    If 节点编辑窗体.Visible = False Then
                        当前编辑节点 = 鼠标移动选中节点
                        节点编辑窗体.Left = 当前编辑节点.位置.X * 缩放倍数 + 主窗体.Left - 节点编辑窗体.Width / 2 + 缩放倍数 / 2 + 视角偏移.X
                        节点编辑窗体.Top = 当前编辑节点.位置.Y * 缩放倍数 + 主窗体.Top + 50 + 缩放倍数 + 视角偏移.Y
                        节点编辑窗体.节点名.Text = 当前编辑节点.名字
                        节点编辑窗体.节点类型.Text = 当前编辑节点.类型
                        节点编辑窗体.节点内容.Text = 当前编辑节点.内容
                        节点编辑窗体.Visible = True
                    End If
                ElseIf 节点编辑窗体.Visible Then
                    节点编辑窗体.Visible = False
                End If
            End If
        End If
    End Sub

    Public Function 编辑节点名(name As String, value As String) As Integer
        If 本域节点.ContainsKey(name) Then
            If Not 本域节点.ContainsKey(value) Then
                If Regex.IsMatch(value, "^[^+\-*/\\=<> ^().']{1,}$") Then
                    Dim 缓存节点 As 节点类 = 本域节点(name)
                    缓存节点.名字 = value
                    本域节点.Remove(name)
                    本域节点.Add(value, 缓存节点)
                    Return 3
                End If
                Return 2
            End If
            Return 1
        End If
        Return 0
    End Function
    Public Function 编辑节点类型(name As String, value As String) As Integer
        If 本域节点.ContainsKey(name) Then
            本域节点(name).类型 = value
            Return 1
        End If
        Return 0
    End Function
    Public Function 编辑节点内容(name As String, value As String) As Integer
        If 本域节点.ContainsKey(name) Then
            本域节点(name).内容 = value
            Return 1
        End If
        Return 0
    End Function

    Public Function 编辑节点位置(name As String, value As Point) As Integer
        If 本域节点.ContainsKey(name) Then
            If Not 空间限制.ContainsKey(value) Then
                空间限制.Remove(本域节点(name).位置)
                本域节点(name).位置 = value
                空间限制.Add(value, 本域节点(name))
                Return 2
            End If
            Return 1
        End If
        Return 0
    End Function
    Public Function 位置到节点(p As Point) As 节点类
        If 空间限制.ContainsKey(p) Then
            Return 空间限制(p)
        End If
        Return Nothing
    End Function

    Public Sub 新建节点(节点名 As String, 位置 As Point)
        If Not 本域节点.ContainsKey(节点名) And Not 空间限制.ContainsKey(位置) And 节点创建模式 <> "" Then
            本域节点.Add(节点名, New 节点类(Me, 节点名, 节点创建模式, "", 位置))
            ' 待添加节点事件钩子
        End If
    End Sub

    Private Sub 绘制()
        On Error Resume Next
        Dim 缓存帧 As New Bitmap(10, 10)
        Do Until 结束标识

            If 主窗体.WindowState <> FormWindowState.Minimized And 主窗体.Visible Then
                For i As Integer = 0 To 节点删除列表.Count - 1
                    节点删除列表(i).删除()
                Next
                节点删除列表.Clear()
                缓存帧.Dispose()
                缓存帧 = New Bitmap(绘制空间.Width, 绘制空间.Height)
                Dim g As Graphics = Graphics.FromImage(缓存帧)

                g.TranslateTransform(视角偏移.X, 视角偏移.Y)
                g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
                g.TextRenderingHint = Text.TextRenderingHint.AntiAlias

                For i As Long = 0 To 本域节点.Count - 1
                    Select Case 本域节点.Values(i).类型
                        Case "函数"
                            For Each targetNode As 节点类 In 本域节点.Values(i).连接
                                绘制线段(g, 函数节点连接色, 本域节点.Values(i)， targetNode)
                            Next
                        Case "引用"
                            For Each targetNode As 节点类 In 本域节点.Values(i).连接
                                绘制线段(g, 引用节点连接色, 本域节点.Values(i)， targetNode)
                            Next
                    End Select
                Next
                For i As Long = 0 To 本域节点.Count - 1
                    绘制节点(g, 本域节点.Values(i))
                Next
                If 鼠标移动选中节点 IsNot Nothing Then
                    绘制节点边缘(g, 鼠标移动选中节点, Color.Red)
                    If 连接发起节点 IsNot Nothing Then
                        绘制节点边缘(g, 连接发起节点, 连接起点颜色)
                        If 连接有效性判断(连接发起节点, 鼠标移动选中节点) Then
                            绘制线段(g, 连接颜色_有效, 连接发起节点.位置, 鼠标实际位置)
                        Else
                            绘制线段(g, 连接颜色_无效, 连接发起节点.位置, 鼠标实际位置)
                        End If
                    End If
                Else
                    If 连接发起节点 IsNot Nothing Then
                        绘制节点边缘(g, 连接发起节点, 连接起点颜色)
                        绘制线段(g, 连接颜色_待定, 连接发起节点.位置, 鼠标实际位置)
                    End If
                End If
                If 当前编辑节点 IsNot Nothing And 节点编辑窗体.Visible Then
                    绘制节点边缘(g, 当前编辑节点, Color.Lime, 3)
                End If
                If 当前按住节点 IsNot Nothing Then
                    If 空间限制.ContainsKey(鼠标当前位置) Then
                        If 鼠标当前位置 <> 当前按住节点.位置 Then
                            绘制节点(g, 当前按住节点, 鼠标实际位置, 节点移动填充色_危险, 节点移动边缘色)
                        End If
                    Else
                        绘制节点(g, 当前按住节点, 鼠标实际位置, 节点移动填充色_安全, 节点移动边缘色)
                        绘制节点边缘(g, 当前按住节点, 鼠标当前位置, Color.Lime, 3)
                    End If
                End If
                g.Dispose()

                绘制更新(缓存帧.Clone)
            End If
            Thread.Sleep(绘制间隔)
        Loop
        全局平面.Clear()
        本域节点.Clear()
        节点编辑窗体.Close()
        全局窗体.Close()
        RemoveHandler 绘制空间.DoubleClick, AddressOf 鼠标双击事件
        RemoveHandler 绘制空间.MouseDown, AddressOf 鼠标点击事件
        RemoveHandler 绘制空间.MouseUp, AddressOf 鼠标点完事件
        RemoveHandler 绘制空间.MouseMove, AddressOf 鼠标移动事件
    End Sub


    Private Sub 绘制更新(ByRef 帧 As Image)
        Dim d As New 绘制更新委托(AddressOf 绘制更新操作)
        主窗体.Invoke(d, 帧)
    End Sub

    Private Sub 绘制更新操作(ByRef 帧 As Image)
        绘制空间.Image.Dispose()
        绘制空间.Image = 帧
    End Sub

    Private Sub 绘制线段(ByRef g As Graphics, 笔 As Pen, 位置1 As Point, 位置2 As Point)
        g.DrawLine(笔, 位置1.X * 缩放倍数 + 缩放倍数 \ 2, 位置1.Y * 缩放倍数 + 缩放倍数 \ 2, 位置2.X, 位置2.Y)
    End Sub

    Private Sub 绘制线段(ByRef g As Graphics, 笔 As Pen, n1 As 节点类, n2 As 节点类)
        Dim 半缩放 As Integer = 缩放倍数 \ 2
        g.DrawLine(笔, n1.位置.X * 缩放倍数 + 半缩放, n1.位置.Y * 缩放倍数 + 半缩放, n2.位置.X * 缩放倍数 + 半缩放, n2.位置.Y * 缩放倍数 + 半缩放)
    End Sub

    Private Sub 绘制节点边缘(ByRef g As Graphics, node As 节点类, 颜色 As Color, Optional 线宽 As Integer = 2)
        Dim r As New Rectangle(node.位置.X * 缩放倍数, node.位置.Y * 缩放倍数, 缩放倍数, 缩放倍数)
        Select Case node.类型
            Case "值"
                g.DrawRectangle(New Pen(颜色, 线宽), r)
            Case "引用"
                g.DrawPolygon(New Pen(颜色, 线宽), 获得三角形点集(node.位置))
            Case "函数"
                g.DrawEllipse(New Pen(颜色, 线宽), r)
        End Select
    End Sub
    Private Sub 绘制节点边缘(ByRef g As Graphics, node As 节点类, 位置 As Point, 颜色 As Color, Optional 线宽 As Integer = 2)
        If node Is Nothing Then Exit Sub
        Dim r As New Rectangle(位置.X * 缩放倍数, 位置.Y * 缩放倍数, 缩放倍数, 缩放倍数)
        Select Case node.类型
            Case "值"
                g.DrawRectangle(New Pen(颜色, 线宽), r)
            Case "引用"
                g.DrawPolygon(New Pen(颜色, 线宽), 获得三角形点集(位置))
            Case "函数"
                g.DrawEllipse(New Pen(颜色, 线宽), r)
        End Select
    End Sub
    Private Sub 绘制节点(ByRef g As Graphics, node As 节点类, 位置 As Point, 填充笔刷 As Brush, 图形边缘 As Pen)
        位置.Offset(-缩放倍数 / 2, -缩放倍数 / 2)
        Dim r As New Rectangle(位置.X, 位置.Y, 缩放倍数, 缩放倍数)
        Select Case node.类型
            Case "值"
                g.FillRectangle(填充笔刷, r)
                g.DrawRectangle(图形边缘, r)
            Case "引用"
                Dim 三角形路径() As Point = 获得三角形点集_无缩放(位置)
                g.FillPolygon(填充笔刷, 三角形路径)
                g.DrawPolygon(图形边缘, 三角形路径)
            Case "函数"
                g.FillEllipse(填充笔刷, r)
                g.DrawEllipse(图形边缘, r)
        End Select
        g.DrawString(String.Format("{0}:{1}", node.名字, node.内容), 主窗体.Font, Brushes.Black, 位置.X, 位置.Y)
    End Sub
    Private Sub 绘制节点(ByRef g As Graphics, node As 节点类)
        Dim r As New Rectangle(node.位置.X * 缩放倍数, node.位置.Y * 缩放倍数, 缩放倍数, 缩放倍数)
        Select Case node.类型
            Case "值"
                g.FillRectangle(New SolidBrush(值节点填充颜色), r)
                g.DrawRectangle(Pens.Black, r)
            Case "引用"
                Dim 三角形路径() As Point = 获得三角形点集(node.位置)
                g.FillPolygon(New SolidBrush(引用节点填充色), 三角形路径)
                g.DrawPolygon(Pens.Black, 三角形路径)
            Case "函数"
                g.FillEllipse(New SolidBrush(函数节点填充色), r)
                g.DrawEllipse(Pens.Black, r)
        End Select
        g.DrawString(String.Format("{0}:{1}", node.名字, node.内容), 主窗体.Font, Brushes.Black, node.位置.X * 缩放倍数, node.位置.Y * 缩放倍数)
    End Sub
    Private Function 获得三角形点集(左上基点 As Point) As Point()
        Dim p As New List(Of Point) From {
            New Point(左上基点.X * 缩放倍数 + 缩放倍数 * 0.5, 左上基点.Y * 缩放倍数),
            New Point(左上基点.X * 缩放倍数, 左上基点.Y * 缩放倍数 + 缩放倍数),
            New Point(左上基点.X * 缩放倍数 + 缩放倍数, 左上基点.Y * 缩放倍数 + 缩放倍数)
        }
        Return p.ToArray
    End Function
    Private Function 获得三角形点集_无缩放(左上基点 As Point) As Point()
        Dim p As New List(Of Point) From {
            New Point(左上基点.X + 缩放倍数 * 0.5, 左上基点.Y),
            New Point(左上基点.X, 左上基点.Y + 缩放倍数),
            New Point(左上基点.X + 缩放倍数, 左上基点.Y + 缩放倍数)
        }
        Return p.ToArray
    End Function
End Class
