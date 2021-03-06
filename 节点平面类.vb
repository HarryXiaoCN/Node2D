Imports System.Threading
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Public Class 节点平面类
    Public Class 节点类
        Private name As String
        Private type As String
        Private content As String
        Private 接口端口控制节点 As 节点类
        Private 接口数据缓存节点 As 节点类
        Private 接口大小控制节点 As 节点类
        Private 接口进入触发节点 As 节点类
        Private 接口解码方式节点 As 节点类
        Private 接收数据解码方式 As Encoding
        Private 发送数据编码方式 As Encoding
        Private 接口线程 As Thread = Nothing
        Private ReadOnly 待连接 As New List(Of String)

        Public 行内容() As String
        'Public 网络接口 As Socket = Nothing
        Public 网络接口 As UdpClient
        Public 接口类型 As String = ""
        Public 接口开启 As Boolean
        Public 高亮 As Boolean
        Public 候选 As Boolean
        Public 激活 As Integer
        Public 位置 As Point
        Public 父域 As 节点平面类
        Public 引用等级 As Integer '0:一次引用 1:本次引用 2:永久引用
        Public 连接 As New List(Of 节点类)
        Public 空间 As New Dictionary(Of String, 节点平面类)
        Public Event 改变前(node As 节点类)
        Public Event 改变后(node As 节点类)
        Public Sub New(ByRef parent As 节点平面类, nodeName As String, nodeType As String, nodeContent As String, nodePos As Point)
            父域 = parent
            name = nodeName
            type = nodeType
            content = nodeContent
            行内容 = Split(content, vbCrLf)
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
            If content Is Nothing Then content = ""
            行内容 = Split(content, vbCrLf)
            位置 = New Point(Val(s(3)), Val(s(4)))
            If s(5) <> "" Then
                待连接.AddRange(Split(s(5), Chr(2)))
            End If
            父域.本域节点.Add(name, Me)
            父域.空间限制.Add(位置, Me)
            重构空间()
        End Sub
        Public Sub 重构接口()
            If type = "接口" And content <> "" Then
                Dim s() As String = Split(content, vbCrLf)
                Select Case s(0).ToLower
                    Case "listen", "接入", "入口", "监听"
                        接口类型 = "网络入口"
                        If s.Length < 4 Then
                            控制台.添加消息(String.Format("接口节点[{0}]：内容过短。", name))
                            Exit Sub
                        End If
                        接口端口控制节点 = 获得节点(s(1), Me)
                        If 接口端口控制节点 Is Nothing Then
                            控制台.添加消息(String.Format("接口节点[{0}]：端口控制节点[{1}]未找到。", name, s(1)))
                            Exit Sub
                        End If
                        '接口大小控制节点 = 获得节点(s(2), Me)
                        'If 接口大小控制节点 Is Nothing Then
                        '    控制台.添加消息(String.Format("接口节点[{0}]：缓存大小控制节点[{1}]未找到。", name, s(2)))
                        '    Exit Sub
                        'End If
                        接口数据缓存节点 = 获得节点(s(2), Me)
                        If 接口数据缓存节点 Is Nothing Then
                            控制台.添加消息(String.Format("接口节点[{0}]：数据缓存节点[{1}]未找到。", name, s(2)))
                            Exit Sub
                        End If
                        接口进入触发节点 = 获得节点(s(3), Me)
                        If 接口进入触发节点 Is Nothing Then
                            控制台.添加消息(String.Format("接口节点[{0}]：进入触发节点[{1}]未找到。", name, s(3)))
                            Exit Sub
                        End If
                        接口释放()
                        '网络接口 = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                        Try
                            网络接口 = New UdpClient(CInt(接口端口控制节点.内容))
                            If s.Length > 4 Then
                                接口解码方式节点 = 获得节点(s(4), Me)
                                If 接口进入触发节点 Is Nothing Then
                                    控制台.添加消息(String.Format("接口节点[{0}]：接口解码方式节点[{1}]未找到。", name, s(4)))
                                    Exit Sub
                                End If
                                接收数据解码方式 = Encoding.GetEncoding(接口解码方式节点.内容)
                            Else
                                接收数据解码方式 = Encoding.UTF8
                            End If
                            '网络接口.Bind(New IPEndPoint(IPAddress.Parse("0.0.0.0"), Val(接口端口控制节点.内容)))
                            '网络接口.Listen()
                            接口线程 = New Thread(AddressOf 接口过程)
                            父域.线程集.Add(接口线程)
                            接口线程.Start()
                        Catch ex As Exception
                            网络接口 = Nothing
                            接口线程 = Nothing
                            发送数据编码方式 = Encoding.UTF8
                            控制台.添加消息(String.Format("接口节点[{0}]：接口初始化错误：{1}。", name, ex.Message))
                        End Try
                    Case "send", "接出", "出口", "发送"
                        接口类型 = "网络出口"
                        Try
                            If s.Length > 1 Then
                                接口解码方式节点 = 获得节点(s(1), Me)
                                If 接口解码方式节点 Is Nothing Then
                                    控制台.添加消息(String.Format("接口节点[{0}]：接口解码方式节点[{1}]未找到。", name, s(1)))
                                    Exit Sub
                                End If
                                发送数据编码方式 = Encoding.GetEncoding(接口解码方式节点.内容)
                                网络接口 = New UdpClient
                            Else
                                发送数据编码方式 = Encoding.UTF8
                            End If
                        Catch ex As Exception
                            发送数据编码方式 = Encoding.UTF8
                            控制台.添加消息(String.Format("接口节点[{0}]：接口初始化错误：{1}。", name, ex.Message))
                        End Try
                    Case Else
                        控制台.添加消息(String.Format("接口节点[{0}]：未知的接口类型""{1}""。", name, s(0)))
                End Select
            End If
        End Sub
        Public Sub 发送数据(rIP As String, port As String, 内容 As String)
            '接口释放()
            Dim b() As Byte = 发送数据编码方式.GetBytes(内容)
            网络接口.Send(b, b.Length, rIP, port)
        End Sub
        Private Sub 接口过程()
            Dim clients As New List(Of Socket)
            Dim rIP As New IPEndPoint(IPAddress.Parse("0.0.0.0"), CInt(接口端口控制节点.内容))
            接口开启 = True
            Try
                While type = "接口" And 接口开启 And Not 父域.结束标识
                    Dim b() As Byte = 网络接口.Receive(rIP)
                    接口数据缓存节点.内容 = 接收数据解码方式.GetString(b)
                    激活 = 180
                    父域.脚本.解释(接口进入触发节点)
                    Thread.Sleep(10)
                End While
            Catch ex As Exception
                控制台.添加消息(String.Format("接口节点[{0}]：接口线程终止，原因：{1}。", name, ex.Message))
            End Try
            For Each s As Socket In clients
                关闭套接字(s)
            Next
        End Sub
        'Private Sub 接收数据(s As Socket)
        '    Try
        '        While type = "接口" And 接口开启 And Not 父域.结束标识
        '            Dim bSize As Integer = Val(接口大小控制节点.内容)
        '            接口大小控制节点.激活 = 180
        '            Dim b(bSize) As Byte
        '            s.Receive(b)
        '            接口数据缓存节点.内容 = Replace(接收数据解码方式.GetString(b), vbNullChar, "")
        '            激活 = 180
        '            父域.脚本.解释(接口进入触发节点)
        '            Thread.Sleep(10)
        '        End While
        '    Catch ex As Exception

        '    End Try
        '    关闭套接字(s)
        'End Sub
        Public Sub 接口释放()
            On Error Resume Next
            接口开启 = False
            'If 网络接口 IsNot Nothing Then 网络接口.Shutdown(SocketShutdown.Both)
            If 网络接口 IsNot Nothing Then 网络接口.Close()
            If 网络接口 IsNot Nothing Then 网络接口 = Nothing
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
        Public Function 获得子平面(平面名 As String) As 节点平面类
            If 类型 = "引用" Then
                If 空间.ContainsKey(平面名) Then
                    Return 空间(平面名)
                End If
            End If
            If 父域.全局平面.ContainsKey(平面名) Then
                Return 父域.全局平面(平面名)
            End If
            Return Nothing
        End Function
        Public Sub 确认连接()
            For i As Integer = 0 To 待连接.Count - 1
                连接.Add(父域.本域节点(待连接(i)))
            Next
            重构接口()
        End Sub
        Public Overloads Function ToString() As String
            Dim 连接名集 As New List(Of String)
            For i As Integer = 0 To 连接.Count - 1
                连接名集.Add(连接(i).名字)
            Next
            Return name & Chr(1) & type & Chr(1) & Replace(content, vbCrLf, Chr(2)) & Chr(1) & 位置.X & Chr(1) & 位置.Y _
                & Chr(1) & Join(连接名集.ToArray, Chr(2))
        End Function
        Public Sub 触发行为()
            If type = "值" Then
                If name.StartsWith("#") Then
                    Dim 脚本 As New 节点脚本类
                    For Each n As 节点类 In 连接
                        If n.类型 = "函数" Then
                            If n.名字.StartsWith("#") Then
                                脚本.解释(n)
                            End If
                        End If
                    Next
                End If
            End If
        End Sub
        Public Property 名字() As String
            Get
                Return name
            End Get
            Set(value As String)
                RaiseEvent 改变前(Me)
                name = value
                触发行为()
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
                重构连接()
                重构空间()
                重构接口()
                RaiseEvent 改变后(Me)
            End Set
        End Property
        Private Sub 重构连接()
            If type = "值" Then
                For i As Integer = 0 To 连接.Count - 1
                    If 连接(i).类型 = "值" Then
                        连接(i).连接.Remove(Me)
                        连接.RemoveAt(i)
                        i -= 1
                        If i >= 连接.Count - 1 Then Exit Sub
                    End If
                Next
                For i As Integer = 0 To 空间.Count - 1
                    空间(i).资源回收()
                Next
                空间.Clear()
            End If
        End Sub
        Private Sub 重构空间()
            If type = "引用" Then
                空间.Clear()
                If content <> "" Then
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
            End If
        End Sub
        Public Property 内容() As String
            Get
                Return content
            End Get
            Set(value As String)
                RaiseEvent 改变前(Me)
                content = value
                行内容 = Split(content, vbCrLf)
                重构空间()
                重构接口()
                触发行为()
                RaiseEvent 改变后(Me)
            End Set
        End Property
        Public Sub 删除()
            接口释放()
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
    Public 用户法则 As New Dictionary(Of String, List(Of 节点类))
    Public 主平面 As Boolean
    Public 结束标识 As Boolean
    Public 清理完成 As Boolean
    Public 绘制间隔 As Long = 15
    Public 缩放倍数 As Integer = 50
    Public 主窗体 As Form1
    Public 节点编辑窗体 As Node
    Public 全局窗体 As New GlobalImportForm(Me)
    Public 候选窗体 As Alternative
    Public 法则提示窗口 As FunctionPrompt
    Public 鼠标移动选中节点 As 节点类
    Public 当前编辑节点 As 节点类
    Public 当前按住节点 As 节点类
    Public 连接发起节点 As 节点类
    Public 节点创建模式 As String = "值"
    Public 鼠标当前位置 As Point
    Public 鼠标实际位置 As Point
    Public 线程集 As New List(Of Thread)

    Public 视角偏移 As Point
    Public 节点移动填充色_安全 As New SolidBrush(Color.FromArgb(180, Color.LightGreen))
    Public 节点移动填充色_危险 As New SolidBrush(Color.FromArgb(180, Color.OrangeRed))
    Public 节点移动边缘色 As New Pen(Color.FromArgb(180, Color.Cyan), 3)
    Public 连接颜色_有效 As New Pen(Color.FromArgb(180, Color.LightGreen), 10)
    Public 连接颜色_待定 As New Pen(Color.FromArgb(180, Color.Gray), 10)
    Public 连接颜色_无效 As New Pen(Color.FromArgb(180, Color.OrangeRed), 10)
    Public 节点高亮色 As Color = Color.LightPink
    Public 节点候选色 As Color = Color.Crimson
    Public 函数节点填充色 As Color = Color.LimeGreen
    Public 引用节点填充色 As Color = Color.DeepSkyBlue
    Public 值节点填充颜色 As Color = Color.Gold
    Public 接口点填充颜色 As Color = Color.PaleVioletRed
    Public 函数节点连接色 As New Pen(Color.FromArgb(180, 函数节点填充色), 10)
    Public 引用节点连接色 As New Pen(Color.FromArgb(180, 引用节点填充色), 10)
    Public 接口节点连接色 As New Pen(Color.FromArgb(180, 接口点填充颜色), 10)
    Public 激活色 As Color = Color.Red
    Public 连接起点颜色 As Color = Color.Purple
    Public WithEvents 绘制空间 As PictureBox
    Public 版本 As String = "NODE2D.20210430.1"
    Public 路径 As String = Application.StartupPath
    Public 平面名 As String = ""
    Public 脚本 As New 节点脚本类
    Public 标题字体 As Font

    Private 视角拖拽起点 As Point
    Private 视角拖拽 As Boolean
    Private 绘制集(9) As List(Of 绘制类)
    Private ReadOnly 节点删除列表 As New List(Of 节点类)
    Private ReadOnly 绘制线程 As Thread
    Private Delegate Sub 绘制更新委托(ByRef 帧 As Image)
    Public Sub 初始化颜色()
        函数节点填充色 = 主窗体.函数点背景色.BackColor
        引用节点填充色 = 主窗体.引用点背景色.BackColor
        值节点填充颜色 = 主窗体.值节点背景色.BackColor
        接口点填充颜色 = 主窗体.接口点背景色.BackColor
    End Sub
    Public Sub New(ByRef mainForm As Form1, ByRef drawSpace As PictureBox, Optional mainNode2D As Boolean = True)
        主窗体 = mainForm
        标题字体 = New Font(主窗体.Font, FontStyle.Bold)
        初始化颜色()
        绘制空间 = drawSpace
        主平面 = mainNode2D
        '节点编辑窗体.Show()
        '节点编辑窗体.Hide()
        If 主平面 Then
            节点编辑窗体 = New Node
            节点编辑窗体.主域 = Me
            候选窗体 = New Alternative(Me)
            法则提示窗口 = New FunctionPrompt(Me)
            主窗体.Text = "节点平面 - " & 路径
            For i As Integer = 0 To UBound(绘制集)
                绘制集(i) = New List(Of 绘制类)
            Next
            绘制线程 = New Thread(AddressOf 绘制)
            线程集.Add(绘制线程)
            绘制线程.Start()
        End If
    End Sub
    Public Sub New(节点集() As String)
        For i As Integer = 0 To UBound(节点集)
            新建节点(i, 节点集(i), New Point(i, i \ 100))
        Next
    End Sub
    Public Sub New(路径 As String)
        加载(路径)
    End Sub
    Public Sub New()

    End Sub
    Public Function 增加用户法则(执行点 As 节点类, ParamArray 参数() As 节点类) As Boolean
        Dim 法则名 As String = 执行点.名字
        If Not 用户法则.ContainsKey(法则名) Then
            用户法则.Add(法则名, New List(Of 节点类))
            用户法则(法则名).Add(执行点)
            用户法则(法则名).AddRange(参数)
            Return True
        End If
        用户法则(法则名).Clear()
        用户法则(法则名).Add(执行点)
        用户法则(法则名).AddRange(参数)
        Return False
    End Function
    Public Sub 结束()
        结束标识 = True
        Do Until 清理完成
            Application.DoEvents()
            Thread.Sleep(10)
        Loop
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
        Dim f As String = File.ReadAllText(filePath)
        Dim s() As String = Split(f, vbCrLf)
        If s(0) = "NODE2D.20210424.1" Then
            Load_NODE2D_20210424_1(filePath, s)
        ElseIf s(0) = "NODE2D.20210430.1" Then
            路径赋予(filePath)
            Dim p() As String = Split(s(1), " ")
            视角偏移 = New Point(Val(p(0)), Val(p(1)))
            If s(2) <> "" Then
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
            End If
            If s(3) <> "" Then
                Dim nS() As String = Split(s(3), Chr(1))
                全局窗体.全局节点列表.Items.AddRange(nS)
            End If
            For i = 4 To UBound(s)
                Dim node As New 节点类(Me, s(i))
            Next
            For Each n As 节点类 In 本域节点.Values
                n.确认连接()
            Next
        ElseIf f = "" Then
            路径赋予(filePath)
        End If
        If 主界面.启动时执行主节点.Checked Then
            If 本域节点.ContainsKey("主节点") Then
                If 本域节点("主节点").类型 = "函数" Then
                    脚本.解释(本域节点("主节点"))
                End If
            ElseIf 本域节点.ContainsKey("Main") Then
                If 本域节点("Main").类型 = "函数" Then
                    脚本.解释(本域节点("Main"))
                End If
            End If
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
            控制台.添加消息(String.Format("未找到全局平面[{0}]（{1}），添加平面失败。", filePath, readPath))
        End If
    End Sub
    Public Sub 移除全局平面()
        If 全局窗体.全局平面列表.SelectedIndex >= 0 And 全局窗体.全局平面列表.SelectedIndex < 全局窗体.全局平面列表.Items.Count Then
            全局平面.Remove(全局窗体.全局平面列表.Items(全局窗体.全局平面列表.SelectedIndex))
            全局窗体.全局平面列表.Items.RemoveAt(全局窗体.全局平面列表.SelectedIndex)
        End If
    End Sub
    Public Sub 移除全局平面(平面路径 As String)
        If 全局窗体.全局平面列表.Items.Contains(平面路径) Then
            全局平面.Remove(平面路径)
            全局窗体.全局平面列表.Items.Remove(平面路径)
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
                        节点编辑窗体居中对齐()
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
    Public Sub 节点编辑窗体居中对齐()
        节点编辑窗体.Left = 当前编辑节点.位置.X * 缩放倍数 + 主窗体.Left - 节点编辑窗体.Width / 2 + 缩放倍数 / 2 + 视角偏移.X
        节点编辑窗体.Top = 当前编辑节点.位置.Y * 缩放倍数 + 主窗体.Top + 50 + 缩放倍数 + 视角偏移.Y
    End Sub

    Public Function 编辑节点名(name As String, value As String) As Integer
        If 本域节点.ContainsKey(name) Then
            If Not 本域节点.ContainsKey(value) Then
                If Regex.IsMatch(value, "^[^().'\s$]{1,}$") Then
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
    Public Function 新建节点(节点名 As String, 内容 As String, 位置 As Point) As Integer
        If Not 本域节点.ContainsKey(节点名) Then
            If Not 空间限制.ContainsKey(位置) Then
                本域节点.Add(节点名, New 节点类(Me, 节点名, "值", 内容, 位置))
                Return 2
            End If
            Return 1
        End If
        Return 0
    End Function

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
                                绘制线段(函数节点连接色, 本域节点.Values(i)， targetNode)
                                If targetNode.激活 > 0 And 本域节点.Values(i).激活 > 0 Then
                                    绘制线段(New Pen(Color.FromArgb((targetNode.激活 + 本域节点.Values(i).激活) \ 2, 激活色), 函数节点连接色.Width), 本域节点.Values(i)， targetNode)
                                End If
                            Next
                        Case "引用"
                            For Each targetNode As 节点类 In 本域节点.Values(i).连接
                                绘制线段(引用节点连接色, 本域节点.Values(i)， targetNode)
                                If targetNode.激活 > 0 And 本域节点.Values(i).激活 > 0 Then
                                    绘制线段(New Pen(Color.FromArgb((targetNode.激活 + 本域节点.Values(i).激活) \ 2, 激活色), 函数节点连接色.Width), 本域节点.Values(i)， targetNode)
                                End If
                            Next
                        Case "接口"
                            For Each targetNode As 节点类 In 本域节点.Values(i).连接
                                绘制线段(接口节点连接色, 本域节点.Values(i)， targetNode)
                                If targetNode.激活 > 0 And 本域节点.Values(i).激活 > 0 Then
                                    绘制线段(New Pen(Color.FromArgb((targetNode.激活 + 本域节点.Values(i).激活) \ 2, 激活色), 函数节点连接色.Width), 本域节点.Values(i)， targetNode)
                                End If
                            Next
                    End Select
                Next
                For i As Long = 0 To 本域节点.Count - 1
                    绘制节点(g, 本域节点.Values(i))
                Next
                If 鼠标移动选中节点 IsNot Nothing Then
                    绘制节点边缘(鼠标移动选中节点, Color.Red)
                    If 连接发起节点 IsNot Nothing Then
                        绘制节点边缘(连接发起节点, 连接起点颜色)
                        If 连接有效性判断(连接发起节点, 鼠标移动选中节点) Then
                            绘制线段(连接颜色_有效, 连接发起节点.位置, 鼠标实际位置)
                        Else
                            绘制线段(连接颜色_无效, 连接发起节点.位置, 鼠标实际位置)
                        End If
                    End If
                Else
                    If 连接发起节点 IsNot Nothing Then
                        绘制节点边缘(连接发起节点, 连接起点颜色)
                        绘制线段(连接颜色_待定, 连接发起节点.位置, 鼠标实际位置)
                    End If
                End If
                If 当前编辑节点 IsNot Nothing And 节点编辑窗体.Visible Then
                    绘制节点边缘(当前编辑节点, Color.Lime, 3)
                End If
                If 当前按住节点 IsNot Nothing Then
                    If 空间限制.ContainsKey(鼠标当前位置) Then
                        If 鼠标当前位置 <> 当前按住节点.位置 Then
                            绘制节点(g, 当前按住节点, 鼠标实际位置, 节点移动填充色_危险, 节点移动边缘色)
                        End If
                    Else
                        绘制节点(g, 当前按住节点, 鼠标实际位置, 节点移动填充色_安全, 节点移动边缘色)
                        绘制节点边缘(当前按住节点, 鼠标当前位置, Color.Lime, 3)
                    End If
                End If
                For ceng As Integer = 0 To UBound(绘制集)
                    For Each drawObj As 绘制类 In 绘制集(ceng)
                        drawObj.绘制(g)
                    Next
                    绘制集(ceng).Clear()
                Next
                g.Dispose()
                绘制更新(缓存帧.Clone)
            End If
            Thread.Sleep(绘制间隔)
        Loop
        资源回收()
        RemoveHandler 绘制空间.DoubleClick, AddressOf 鼠标双击事件
        RemoveHandler 绘制空间.MouseDown, AddressOf 鼠标点击事件
        RemoveHandler 绘制空间.MouseUp, AddressOf 鼠标点完事件
        RemoveHandler 绘制空间.MouseMove, AddressOf 鼠标移动事件
        清理完成 = True
    End Sub
    Public Sub 资源回收()
        For i As Integer = 0 To 全局平面.Count - 1
            全局平面.Values(i).资源回收()
        Next
        全局平面.Clear()
        For i As Integer = 0 To 本域节点.Count - 1
            Select Case 本域节点.Values(i).类型
                Case "引用"
                    For j As Integer = 0 To 本域节点.Values(i).空间.Count - 1
                        本域节点.Values(i).空间.Values(j).资源回收()
                    Next
                    本域节点.Values(i).空间.Clear()
                Case "接口"
                    本域节点.Values(i).接口释放()
            End Select
        Next
        'For i As Integer = 0 To UBound(绘制集)
        '    绘制集(i).Clear()
        'Next
        本域节点.Clear()
        If 节点编辑窗体 IsNot Nothing Then 节点编辑窗体.Close()
        全局窗体.Close()
    End Sub
    Private Sub 绘制更新(ByRef 帧 As Image)
        Dim d As New 绘制更新委托(AddressOf 绘制更新操作)
        主窗体.Invoke(d, 帧)
    End Sub

    Private Sub 绘制更新操作(ByRef 帧 As Image)
        绘制空间.Image.Dispose()
        绘制空间.Image = 帧
    End Sub

    Private Sub 绘制线段(笔 As Pen, 位置1 As Point, 位置2 As Point)
        绘制集(0).Add(New 绘制类(笔, 位置1.X * 缩放倍数 + 缩放倍数 \ 2, 位置1.Y * 缩放倍数 + 缩放倍数 \ 2, 位置2.X, 位置2.Y))
        'g.DrawLine(笔, 位置1.X * 缩放倍数 + 缩放倍数 \ 2, 位置1.Y * 缩放倍数 + 缩放倍数 \ 2, 位置2.X, 位置2.Y)
    End Sub

    Private Sub 绘制线段(笔 As Pen, n1 As 节点类, n2 As 节点类)
        Dim 半缩放 As Integer = 缩放倍数 \ 2
        绘制集(0).Add(New 绘制类(笔, n1.位置.X * 缩放倍数 + 半缩放, n1.位置.Y * 缩放倍数 + 半缩放, n2.位置.X * 缩放倍数 + 半缩放, n2.位置.Y * 缩放倍数 + 半缩放))
        'g.DrawLine(笔, n1.位置.X * 缩放倍数 + 半缩放, n1.位置.Y * 缩放倍数 + 半缩放, n2.位置.X * 缩放倍数 + 半缩放, n2.位置.Y * 缩放倍数 + 半缩放)
    End Sub

    Private Sub 绘制节点边缘(node As 节点类, 颜色 As Color, Optional 线宽 As Integer = 2)
        Dim r As New Rectangle(node.位置.X * 缩放倍数, node.位置.Y * 缩放倍数, 缩放倍数, 缩放倍数)
        Select Case node.类型
            Case "值"
                绘制集(4).Add(New 绘制类(New Pen(颜色, 线宽), r))
                'g.DrawRectangle(New Pen(颜色, 线宽), r)
            Case "引用"
                绘制集(4).Add(New 绘制类(New Pen(颜色, 线宽), 获得三角形点集(node.位置)))
                'g.DrawPolygon(New Pen(颜色, 线宽), 获得三角形点集(node.位置))
            Case "函数"
                绘制集(4).Add(New 绘制类(New Pen(颜色, 线宽), r, "DE"))
                'g.DrawEllipse(New Pen(颜色, 线宽), r)
            Case "接口"
                绘制集(4).Add(New 绘制类(New Pen(颜色, 线宽), 获得六边形点集(node.位置)))
                'g.DrawPolygon(New Pen(颜色, 线宽), 获得六边形点集(node.位置))
        End Select
    End Sub
    Private Sub 绘制节点边缘(node As 节点类, 位置 As Point, 颜色 As Color, Optional 线宽 As Integer = 2)
        If node Is Nothing Then Exit Sub
        Dim r As New Rectangle(位置.X * 缩放倍数, 位置.Y * 缩放倍数, 缩放倍数, 缩放倍数)
        Select Case node.类型
            Case "值"
                绘制集(4).Add(New 绘制类(New Pen(颜色, 线宽), r))
                'g.DrawRectangle(New Pen(颜色, 线宽), r)
            Case "引用"
                绘制集(4).Add(New 绘制类(New Pen(颜色, 线宽), 获得三角形点集(位置)))
                'g.DrawPolygon(New Pen(颜色, 线宽), 获得三角形点集(位置))
            Case "函数"
                绘制集(4).Add(New 绘制类(New Pen(颜色, 线宽), r, "DE"))
                'g.DrawEllipse(New Pen(颜色, 线宽), r)
            Case "接口"
                绘制集(4).Add(New 绘制类(New Pen(颜色, 线宽), 获得六边形点集(位置)))
                'g.DrawPolygon(New Pen(颜色, 线宽), 获得六边形点集(位置))
        End Select
    End Sub
    Private Sub 绘制节点(ByRef g As Graphics, node As 节点类, 位置 As Point, 填充笔刷 As Brush, 图形边缘 As Pen)
        位置.Offset(-缩放倍数 / 2, -缩放倍数 / 2)
        Dim r As New Rectangle(位置.X, 位置.Y, 缩放倍数, 缩放倍数)
        Select Case node.类型
            Case "值"
                绘制集(1).Add(New 绘制类(填充笔刷, r))
                'g.FillRectangle(填充笔刷, r)
                绘制集(2).Add(New 绘制类(图形边缘, r))
                'g.DrawRectangle(图形边缘, r)
            Case "引用"
                Dim 三角形路径() As Point = 获得三角形点集_无缩放(位置)
                绘制集(1).Add(New 绘制类(填充笔刷, 三角形路径))
                'g.FillPolygon(填充笔刷, 三角形路径)
                绘制集(2).Add(New 绘制类(图形边缘, 三角形路径))
                'g.DrawPolygon(图形边缘, 三角形路径)
            Case "函数"
                绘制集(1).Add(New 绘制类(填充笔刷, r, "FE"))
                'g.FillEllipse(填充笔刷, r)
                绘制集(2).Add(New 绘制类(图形边缘, r, "DE"))
                'g.DrawEllipse(图形边缘, r)
            Case "接口"
                Dim 六边形路径() As Point = 获得六边形点集_无缩放(位置)
                绘制集(1).Add(New 绘制类(填充笔刷, 六边形路径))
                'g.FillPolygon(填充笔刷, 六边形路径)
                绘制集(2).Add(New 绘制类(图形边缘, 六边形路径))
                'g.DrawPolygon(图形边缘, 六边形路径)
        End Select
        Dim printStr As String
        If 主窗体.主窗体显示内容.Checked Then
            printStr = node.内容
        Else
            If node.行内容.Length > 1 Then
                printStr = String.Format("{0}...({1})", node.行内容(0), node.行内容.Length)
            Else
                printStr = node.内容
            End If
        End If
        Dim title As String = node.名字 & ":"
        Dim s2 As SizeF = g.MeasureString(title, 标题字体)
        If 主窗体.主窗体文字居中.Checked Then
            Dim s As SizeF
            If printStr = "" Then
                s = s2
                s.Width = 0
            Else
                s = g.MeasureString(printStr, 主窗体.Font)
            End If
            绘制集(8).Add(New 绘制类(printStr, 主窗体.Font, Brushes.Black, node.位置.X * 缩放倍数 + (缩放倍数 \ 2) - ((s.Width + s2.Width) \ 2) + s2.Width, node.位置.Y * 缩放倍数 + (缩放倍数 \ 2) - (s.Height \ 2)))
            'g.DrawString(printStr, 主窗体.Font, Brushes.Black, node.位置.X * 缩放倍数 + (缩放倍数 \ 2) - ((s.Width + s2.Width) \ 2) + s2.Width, node.位置.Y * 缩放倍数 + (缩放倍数 \ 2) - (s.Height \ 2))
            绘制集(8).Add(New 绘制类(title, 标题字体, Brushes.Black, node.位置.X * 缩放倍数 + (缩放倍数 \ 2) - ((s.Width + s2.Width) \ 2), node.位置.Y * 缩放倍数 + (缩放倍数 \ 2) - (s.Height \ 2)))
            'g.DrawString(title, 标题字体, Brushes.Black, node.位置.X * 缩放倍数 + (缩放倍数 \ 2) - ((s.Width + s2.Width) \ 2), node.位置.Y * 缩放倍数 + (缩放倍数 \ 2) - (s.Height \ 2))
        Else
            绘制集(8).Add(New 绘制类(printStr, 主窗体.Font, Brushes.Black, node.位置.X * 缩放倍数, node.位置.Y * 缩放倍数 + s2.Height))
            'g.DrawString(printStr, 主窗体.Font, Brushes.Black, node.位置.X * 缩放倍数, node.位置.Y * 缩放倍数 + s2.Height)
            绘制集(8).Add(New 绘制类(title, 标题字体, Brushes.Black, node.位置.X * 缩放倍数, node.位置.Y * 缩放倍数))
            'g.DrawString(title, 标题字体, Brushes.Black, node.位置.X * 缩放倍数, node.位置.Y * 缩放倍数)
        End If
    End Sub
    Private Sub 绘制节点(ByRef g As Graphics, node As 节点类)
        Dim r As New Rectangle(node.位置.X * 缩放倍数, node.位置.Y * 缩放倍数, 缩放倍数, 缩放倍数)
        Dim 边缘色 As New Pen(Color.White, 3), 边缘图层 As Integer = 2
        If node.高亮 Then
            边缘色 = New Pen(节点高亮色, 3)
            边缘图层 = 3
        End If
        If node.候选 Then
            边缘色 = New Pen(节点候选色, 3)
            边缘图层 = 3
        End If
        Select Case node.类型
            Case "值"
                绘制集(1).Add(New 绘制类(New SolidBrush(值节点填充颜色), r))
                'g.FillRectangle(New SolidBrush(值节点填充颜色), r)
                绘制集(边缘图层).Add(New 绘制类(边缘色, r))
                'g.DrawRectangle(边缘色, r)
                If node.激活 > 0 Then
                    绘制集(3).Add(New 绘制类(New SolidBrush(Color.FromArgb(node.激活, 激活色)), r))
                    'g.FillRectangle(New SolidBrush(Color.FromArgb(node.激活, 激活色)), r)
                    node.激活 -= 10
                End If
            Case "引用"
                Dim 三角形路径() As Point = 获得三角形点集(node.位置)
                绘制集(1).Add(New 绘制类(New SolidBrush(引用节点填充色), 三角形路径))
                'g.FillPolygon(New SolidBrush(引用节点填充色), 三角形路径)
                绘制集(边缘图层).Add(New 绘制类(边缘色, 三角形路径))
                'g.DrawPolygon(边缘色, 三角形路径)
                If node.激活 > 0 Then
                    绘制集(3).Add(New 绘制类(New SolidBrush(Color.FromArgb(node.激活, 激活色)), 三角形路径))
                    'g.FillPolygon(New SolidBrush(Color.FromArgb(node.激活, 激活色)), 三角形路径)
                    node.激活 -= 10
                End If
            Case "函数"
                绘制集(1).Add(New 绘制类(New SolidBrush(函数节点填充色), r, "FE"))
                'g.FillEllipse(New SolidBrush(函数节点填充色), r)
                绘制集(边缘图层).Add(New 绘制类(边缘色, r, "DE"))
                'g.DrawEllipse(边缘色, r)
                If node.激活 > 0 Then
                    绘制集(3).Add(New 绘制类(New SolidBrush(Color.FromArgb(node.激活, 激活色)), r, "FE"))
                    'g.FillEllipse(New SolidBrush(Color.FromArgb(node.激活, 激活色)), r)
                    node.激活 -= 10
                End If
            Case "接口"
                Dim 六边形路径() As Point = 获得六边形点集(node.位置)
                绘制集(1).Add(New 绘制类(New SolidBrush(接口点填充颜色), 六边形路径))
                'g.FillPolygon(New SolidBrush(接口点填充颜色), 六边形路径)
                绘制集(边缘图层).Add(New 绘制类(边缘色, 六边形路径))
                'g.DrawPolygon(边缘色, 六边形路径)
                If node.激活 > 0 Then
                    绘制集(3).Add(New 绘制类(New SolidBrush(Color.FromArgb(node.激活, 激活色)), 六边形路径))
                    'g.FillPolygon(New SolidBrush(Color.FromArgb(node.激活, 激活色)), 六边形路径)
                    node.激活 -= 10
                End If
        End Select
        Dim printStr As String
        If 主窗体.主窗体显示内容.Checked Then
            printStr = node.内容
        Else
            If node.行内容.Length > 1 Then
                printStr = String.Format("{0}...({1})", node.行内容(0), node.行内容.Length)
            Else
                printStr = node.内容
            End If
        End If
        Dim title As String = node.名字 & ":"
        Dim s2 As SizeF = g.MeasureString(title, 标题字体)
        If 主窗体.主窗体文字居中.Checked Then
            Dim s As SizeF
            If printStr = "" Then
                s = s2
                s.Width = 0
            Else
                s = g.MeasureString(printStr, 主窗体.Font)
            End If
            绘制集(8).Add(New 绘制类(printStr, 主窗体.Font, Brushes.Black, node.位置.X * 缩放倍数 + (缩放倍数 \ 2) - ((s.Width + s2.Width) \ 2) + s2.Width, node.位置.Y * 缩放倍数 + (缩放倍数 \ 2) - (s.Height \ 2)))
            'g.DrawString(printStr, 主窗体.Font, Brushes.Black, node.位置.X * 缩放倍数 + (缩放倍数 \ 2) - ((s.Width + s2.Width) \ 2) + s2.Width, node.位置.Y * 缩放倍数 + (缩放倍数 \ 2) - (s.Height \ 2))
            绘制集(8).Add(New 绘制类(title, 标题字体, Brushes.Black, node.位置.X * 缩放倍数 + (缩放倍数 \ 2) - ((s.Width + s2.Width) \ 2), node.位置.Y * 缩放倍数 + (缩放倍数 \ 2) - (s.Height \ 2)))
            'g.DrawString(title, 标题字体, Brushes.Black, node.位置.X * 缩放倍数 + (缩放倍数 \ 2) - ((s.Width + s2.Width) \ 2), node.位置.Y * 缩放倍数 + (缩放倍数 \ 2) - (s.Height \ 2))
        Else
            绘制集(8).Add(New 绘制类(printStr, 主窗体.Font, Brushes.Black, node.位置.X * 缩放倍数, node.位置.Y * 缩放倍数 + s2.Height))
            'g.DrawString(printStr, 主窗体.Font, Brushes.Black, node.位置.X * 缩放倍数, node.位置.Y * 缩放倍数 + s2.Height)
            绘制集(8).Add(New 绘制类(title, 标题字体, Brushes.Black, node.位置.X * 缩放倍数, node.位置.Y * 缩放倍数))
            'g.DrawString(title, 标题字体, Brushes.Black, node.位置.X * 缩放倍数, node.位置.Y * 缩放倍数)
        End If
    End Sub
    Private Function 获得六边形点集(左上基点 As Point) As Point()
        Dim v As Integer = 缩放倍数 / 2 * Math.Sin(30 * Math.PI / 180)
        'Dim p As New List(Of Point) From {
        '    New Point(左上基点.X * 缩放倍数, 左上基点.Y * 缩放倍数 + 缩放倍数 \ 2),
        '    New Point(左上基点.X * 缩放倍数 + v, 左上基点.Y * 缩放倍数 + 缩放倍数),
        '    New Point(左上基点.X * 缩放倍数 + 缩放倍数 - v, 左上基点.Y * 缩放倍数 + 缩放倍数),
        '    New Point(左上基点.X * 缩放倍数 + 缩放倍数, 左上基点.Y * 缩放倍数 + 缩放倍数 \ 2),
        '    New Point(左上基点.X * 缩放倍数 + 缩放倍数 - v, 左上基点.Y * 缩放倍数),
        '    New Point(左上基点.X * 缩放倍数 + v, 左上基点.Y * 缩放倍数)
        '}
        Dim p As New List(Of Point) From {
            New Point(左上基点.X * 缩放倍数 + 缩放倍数 \ 2, 左上基点.Y * 缩放倍数),
            New Point(左上基点.X * 缩放倍数 + 缩放倍数, 左上基点.Y * 缩放倍数 + v),
            New Point(左上基点.X * 缩放倍数 + 缩放倍数, 左上基点.Y * 缩放倍数 + 缩放倍数 - v),
            New Point(左上基点.X * 缩放倍数 + 缩放倍数 \ 2, 左上基点.Y * 缩放倍数 + 缩放倍数),
            New Point(左上基点.X * 缩放倍数, 左上基点.Y * 缩放倍数 + 缩放倍数 - v),
            New Point(左上基点.X * 缩放倍数, 左上基点.Y * 缩放倍数 + v)
        }
        Return p.ToArray
    End Function
    Private Function 获得六边形点集_无缩放(左上基点 As Point) As Point()
        Dim v As Integer = 缩放倍数 / 2 * Math.Sin(30 * Math.PI / 180)
        'Dim p As New List(Of Point) From {
        '    New Point(左上基点.X, 左上基点.Y + 缩放倍数 \ 2),
        '    New Point(左上基点.X + v, 左上基点.Y + 缩放倍数),
        '    New Point(左上基点.X + 缩放倍数 - v, 左上基点.Y + 缩放倍数),
        '    New Point(左上基点.X + 缩放倍数, 左上基点.Y + 缩放倍数 \ 2),
        '    New Point(左上基点.X + 缩放倍数 - v, 左上基点.Y),
        '    New Point(左上基点.X + v, 左上基点.Y)
        '}
        Dim p As New List(Of Point) From {
            New Point(左上基点.X + 缩放倍数 \ 2, 左上基点.Y),
            New Point(左上基点.X + 缩放倍数, 左上基点.Y + v),
            New Point(左上基点.X + 缩放倍数, 左上基点.Y + 缩放倍数 - v),
            New Point(左上基点.X + 缩放倍数 \ 2, 左上基点.Y + 缩放倍数),
            New Point(左上基点.X, 左上基点.Y + 缩放倍数 - v),
            New Point(左上基点.X, 左上基点.Y + v)
        }
        Return p.ToArray
    End Function
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
