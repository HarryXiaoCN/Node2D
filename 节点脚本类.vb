Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Threading
Imports Node2D.节点平面类

Public Module 节点全局
    Public Structure 文本定位类
        Public 前缀 As String, 行 As Long, 列 As Long
        Public Sub New(beforeStr As String, row As Long, col As Long)
            前缀 = beforeStr
            行 = row
            列 = col
        End Sub
    End Structure
    Public 控制台 As NodeConsole
    Public Function 获得前缀(文本域 As TextBox) As 文本定位类
        Dim s() As String = Split(文本域.Text, vbCrLf)
        Dim 已长 As Long, 前缀 As String = "", 句首长 As Long, 句内已长 As Long, 行 As Long, 列 As Long
        For i As Long = 0 To UBound(s)
            句首长 = 已长
            已长 += s(i).Length
            If i > 0 Then
                已长 += 2
            End If
            If 已长 >= 文本域.SelectionStart Then
                行 = i
                Dim s2() As String = Split(s(i), " ")
                If i > 0 Then
                    句内已长 += 2
                End If
                For j As Long = 0 To UBound(s2)
                    句内已长 += s2(j).Length
                    If j > 0 Then
                        句内已长 += 1
                    End If
                    If 句首长 + 句内已长 >= 文本域.SelectionStart Then
                        列 = j
                        前缀 = s2(j)
                        Exit For
                    End If
                Next
                Exit For
            End If
        Next
        'Debug.WriteLine(String.Format("{0}，{1}，{2}", 前缀, 行, 列))
        Return New 文本定位类(前缀, 行, 列)
    End Function
    Public Function BoolToInt(b As Boolean) As Integer
        If b Then
            Return 1
        End If
        Return 0
    End Function
    Public Function 式判断(句 As String) As String
        If Regex.IsMatch(句, ".{1,} .{1,}") Then
            Return "赋值式"
        End If
        Return "执行式"
    End Function
    Public Function 获得平面(路径 As String, ByRef 搜索发起节点 As 节点类) As 节点平面类
        Dim 域() As String = 路径.Split(".")
        Dim 子平面 As 节点平面类 = 搜索发起节点.获得子平面(域(0))
        Dim 子节点 As 节点类 = 搜索发起节点
        For i As Integer = 0 To UBound(域) - 1
            If 子平面 Is Nothing Then
                子节点 = 子节点.获得子节点(域(i))
                If 子节点 Is Nothing Then Return Nothing
                子平面 = 子节点.获得子平面(域(i + 1))
                i += 1
            Else
                子平面 = 子节点.获得子平面(域(i))
            End If
        Next
        Return 子平面
    End Function
    Public Function 获得节点(路径 As String, ByRef 搜索发起节点 As 节点类) As 节点类
        Dim 域() As String = 路径.Split(".")
        Dim 子节点 As 节点类 = 搜索发起节点.获得子节点(域(0))
        Dim 子平面 As 节点平面类 = Nothing
        Dim 上个子节点 As 节点类 = Nothing
        If 子节点 Is Nothing And 子平面 Is Nothing Then
            Return Nothing
        End If
        For i As Integer = 0 To UBound(域) - 1
            If 子节点 Is Nothing Then
                If 上个子节点 Is Nothing Then Return Nothing
                子平面 = 上个子节点.获得子平面(域(i))
                If 子平面 Is Nothing Then Return Nothing
                If 子平面.本域节点.ContainsKey(域(i + 1)) Then
                    上个子节点 = 子节点
                    子节点 = 子平面.本域节点(域(i + 1))
                Else
                    Return Nothing
                End If
                i += 1
            Else
                上个子节点 = 子节点
                子节点 = 子节点.获得子节点(域(i))
            End If
        Next
        Return 子节点
    End Function
    Public Function 连接有效性判断(n1 As 节点类, n2 As 节点类) As Boolean
        If (n1.类型 = n2.类型 And n1.类型 = "值") Or (n1.名字 = n2.名字) Then
            Return False
        End If
        Return True
    End Function
End Module
Public Class 节点脚本类
    Public Sub 解释(ByRef 节点 As 节点类)
        Dim 执行线程 As New Thread(AddressOf 函数解释)
        执行线程.SetApartmentState(ApartmentState.STA)
        执行线程.Start(节点)
    End Sub
    Private Sub 函数解释(节点 As 节点类)
        Dim 句集() As String = Split(节点.内容, vbCrLf)
        For i As Integer = 0 To UBound(句集)
            If 句集(i) <> "" And Not 句集(i).StartsWith("'") Then
                Dim 性质 As String = 式判断(句集(i))
                Select Case 性质
                    Case "执行式"
                        Dim 执行节点 As 节点类
                        If 句集(i).EndsWith("()") Then
                            执行节点 = 获得节点(句集(i).Substring(0, 句集(i).Length - 2), 节点)
                        Else
                            执行节点 = 获得节点(句集(i), 节点)
                        End If
                        If 执行节点 Is Nothing Then
                            控制台.添加消息(String.Format("函数节点[{0}]第{1}行：未找到欲执行节点[{2}]。", 节点.名字, i + 1, 句集(i)))
                        Else
                            If 执行节点.类型 = "函数" Then
                                函数解释(执行节点)
                            Else
                                控制台.添加消息(String.Format("函数节点[{0}]第{1}行：节点[{2}]不是函数节点，无法执行。", 节点.名字, i + 1, 句集(i)))
                            End If
                        End If
                    Case "赋值式"
                        控制台.添加消息(赋值式运算(节点, 句集(i), i + 1))
                    Case Else
                        控制台.添加消息(String.Format("函数节点[{0}]未识别句：{1}", 节点.名字, 句集(i)))
                End Select
            End If
        Next
    End Sub

    Public Function 赋值式运算(ByRef 节点 As 节点类, targetString As String, 行 As Long) As String
        '禁用：空格 + . = 换行
        Dim nodesString() As String = Split(targetString, " ")
        Dim nodes As New List(Of 节点类)
        For i As Integer = 1 To UBound(nodesString)
            nodes.Add(获得节点(nodesString(i), 节点))
            If nodes.Last Is Nothing Then Return String.Format("函数节点[{0}]第{1}行：参与节点[{2}]未找到。", 节点.名字, 行, nodesString(i))
        Next
        Try
            Select Case nodesString(0).ToLower
                Case "+", "加", "加法"
                    If nodesString.Length < 4 Then Return String.Format("函数节点[{1}]第{2}行：加法语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).内容 = Val(nodes(1).内容) + Val(nodes(2).内容)
                Case "-", "减", "减法"
                    If nodesString.Length < 4 Then Return String.Format("函数节点[{1}]第{2}行：减法语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).内容 = Val(nodes(1).内容) - Val(nodes(2).内容)
                Case "*", "乘", "乘法"
                    If nodesString.Length < 4 Then Return String.Format("函数节点[{1}]第{2}行：乘法语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).内容 = Val(nodes(1).内容) * Val(nodes(2).内容)
                Case "/", "除", "除法"
                    If nodesString.Length < 4 Then Return String.Format("函数节点[{1}]第{2}行：除法语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).内容 = Val(nodes(1).内容) / Val(nodes(2).内容)
                Case "//", "\", "整除"
                    If nodesString.Length < 4 Then Return String.Format("函数节点[{1}]第{2}行：整除语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).内容 = Val(nodes(1).内容) \ Val(nodes(2).内容)
                Case "mod", "%", "取余"
                    If nodesString.Length < 4 Then Return String.Format("函数节点[{1}]第{2}行：取余语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).内容 = Val(nodes(1).内容) Mod Val(nodes(2).内容)
                Case "^", "**", "幂运算"
                    If nodesString.Length < 4 Then Return String.Format("函数节点[{1}]第{2}行：幂运算语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).内容 = Val(nodes(1).内容) ^ Val(nodes(2).内容)
                Case "&", "拼接"
                    If nodesString.Length < 4 Then Return String.Format("函数节点[{1}]第{2}行：拼接语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).内容 = nodes(1).内容 & nodes(2).内容
                Case ">", "大于"
                    If nodesString.Length < 4 Then Return String.Format("函数节点[{1}]第{2}行：大于判断语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).内容 = BoolToInt(Val(nodes(1).内容) > Val(nodes(2).内容))
                Case "<", "小于"
                    If nodesString.Length < 4 Then Return String.Format("函数节点[{1}]第{2}行：小于判断语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).内容 = BoolToInt(Val(nodes(1).内容) < Val(nodes(2).内容))
                Case ">=", "大于或等于"
                    If nodesString.Length < 4 Then Return String.Format("函数节点[{1}]第{2}行：大于或等于判断语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).内容 = BoolToInt(Val(nodes(1).内容) >= Val(nodes(2).内容))
                Case "<=", "小于或等于"
                    If nodesString.Length < 4 Then Return String.Format("函数节点[{1}]第{2}行：小于或等于判断语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).内容 = BoolToInt(Val(nodes(1).内容) <= Val(nodes(2).内容))
                Case "==", "等于"
                    If nodesString.Length < 4 Then Return String.Format("函数节点[{1}]第{2}行：等于判断语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).内容 = BoolToInt(nodes(1).内容 = nodes(2).内容)
                Case "!=", "<>", "不等于"
                    If nodesString.Length < 4 Then Return String.Format("函数节点[{1}]第{2}行：不等于判断语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).内容 = BoolToInt(nodes(1).内容 <> nodes(2).内容)
                Case "and", "与"
                    If nodesString.Length < 4 Then Return String.Format("函数节点[{1}]第{2}行：与运算语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).内容 = BoolToInt(CBool(Val(nodes(1).内容)) And CBool(Val(nodes(2).内容)))
                Case "or", "或"
                    If nodesString.Length < 4 Then Return String.Format("函数节点[{1}]第{2}行：或运算语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).内容 = BoolToInt(CBool(Val(nodes(1).内容)) Or CBool(Val(nodes(2).内容)))
                Case "!", "not", "非"
                    If nodesString.Length < 3 Then Return String.Format("函数节点[{1}]第{2}行：非运算语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).内容 = BoolToInt(Not CBool(Val(nodes(1).内容)))
                Case "xor", "异或"
                    If nodesString.Length < 4 Then Return String.Format("函数节点[{1}]第{2}行：异或运算语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).内容 = BoolToInt(CBool(Val(nodes(1).内容)) Xor CBool(Val(nodes(2).内容)))
                Case "in", "indexof", "存在"
                    If nodesString.Length < 4 Then Return String.Format("函数节点[{1}]第{2}行：存在语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).内容 = nodes(1).内容.IndexOf(nodes(2).内容)
                Case ":", "切片"
                    Select Case nodesString.Length
                        Case 4
                            nodes(0).内容 = nodes(1).内容.Substring(Val(nodes(2).内容))
                        Case 5
                            nodes(0).内容 = nodes(1).内容.Substring(Val(nodes(2).内容), Val(nodes(3).内容))
                        Case Else
                            Return String.Format("函数节点[{1}]第{2}行：切片语句""{0}""参数数量不对。", targetString, 节点.名字, 行)
                    End Select
                Case "=", "赋值"
                    If nodesString.Length < 3 Then Return String.Format("函数节点[{1}]第{2}行：赋值语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).内容 = nodes(1).内容
                Case "int", "取整"
                    If nodesString.Length < 3 Then Return String.Format("函数节点[{1}]第{2}行：取整语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).内容 = Int(Val(nodes(1).内容))
                Case "len", "长度"
                    If nodesString.Length < 3 Then Return String.Format("函数节点[{1}]第{2}行：取整语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).内容 = nodes(1).内容.Length
                Case "tolower", "lcase", "小写化"
                    If nodesString.Length < 3 Then Return String.Format("函数节点[{1}]第{2}行：小写化语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).内容 = nodes(1).内容.ToLower
                Case "toupper", "ucase", "大写化"
                    If nodesString.Length < 3 Then Return String.Format("函数节点[{1}]第{2}行：大写化语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).内容 = nodes(1).内容.ToUpper
                Case "x", "横坐标"
                    If nodesString.Length < 3 Then Return String.Format("函数节点[{1}]第{2}行：取横坐标语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).内容 = nodes(1).位置.X
                Case "y", "纵坐标"
                    If nodesString.Length < 3 Then Return String.Format("函数节点[{1}]第{2}行：取纵坐标语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).内容 = nodes(1).位置.Y
                Case "name", "名字"
                    If nodesString.Length < 3 Then Return String.Format("函数节点[{1}]第{2}行：取得名字语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).内容 = nodes(1).名字
                Case "val", "float", "数化"
                    If nodesString.Length < 3 Then Return String.Format("函数节点[{1}]第{2}行：数化语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).内容 = Val(nodes(1).名字)
                Case "readtxt", "读文本"
                    If nodesString.Length < 3 Then Return String.Format("函数节点[{1}]第{2}行：读文本语句""{0}""过短。", targetString, 节点.名字, 行)
                    If File.Exists(nodes(1).内容) Then
                        nodes(0).内容 = File.ReadAllText(nodes(1).内容)
                    Else
                        Return String.Format("函数节点[{1}]第{2}行：读文本失败，路径""{0}""不存在。", nodes(1).内容, 节点.名字, 行)
                    End If
                Case "writetxt", "写文本"
                    If nodesString.Length < 3 Then Return String.Format("函数节点[{1}]第{2}行：存文本语句""{0}""过短。", targetString, 节点.名字, 行)
                    File.WriteAllText(nodes(0).内容, nodes(1).内容)
                Case "readclipboard", "读剪切板"
                    If nodesString.Length < 2 Then Return String.Format("函数节点[{1}]第{2}行：读剪切板语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).内容 = Clipboard.GetText
                Case "writeclipboard", "写剪切板"
                    If nodesString.Length < 2 Then Return String.Format("函数节点[{1}]第{2}行：写剪切板语句""{0}""过短。", targetString, 节点.名字, 行)
                    Clipboard.Clear()
                    Clipboard.SetText(nodes(0).内容)
                Case "sleep", "睡眠"
                    If nodesString.Length < 2 Then Return String.Format("函数节点[{1}]第{2}行：睡眠语句""{0}""过短。", targetString, 节点.名字, 行)
                    Thread.Sleep(Val(nodes(0).内容))
                Case "if", "若", "如果"
                    Select Case nodesString.Length
                        Case 3
                            If Val(nodes(0).内容) Then
                                函数解释(nodes(1))
                            End If
                        Case 4
                            If Val(nodes(0).内容) Then
                                函数解释(nodes(1))
                            Else
                                函数解释(nodes(2))
                            End If
                        Case Else
                            Return String.Format("函数节点[{1}]第{2}行：判断语句""{0}""参数数量不对。", targetString, 节点.名字, 行)
                    End Select
                Case "for", "遍历", "循环", "遍"
                    Select Case nodesString.Length
                        Case 5
                            For i As Long = Val(nodes(1).内容) To Val(nodes(2).内容)
                                nodes(0).内容 = i
                                函数解释(nodes(3))
                            Next
                        Case 6
                            For i As Long = Val(nodes(1).内容) To Val(nodes(2).内容) Step Val(nodes(3).内容)
                                nodes(0).内容 = i
                                函数解释(nodes(4))
                            Next
                        Case Else
                            Return String.Format("函数节点[{1}]第{2}行：循环语句""{0}""参数数量不对。", targetString, 节点.名字, 行)
                    End Select
                Case "call", "调用"
                    If nodesString.Length < 2 Then Return String.Format("函数节点[{1}]第{2}行：调用语句""{0}""过短。", targetString, 节点.名字, 行)
                    Dim 调用节点 As 节点类 = 获得节点(nodes(0).内容, nodes(0))
                    If 调用节点 Is Nothing Then
                        Return String.Format("函数节点[{0}]第{1}行：根据节点[{2}]的内容""{3}""未找到对应欲调用节点。", 节点.名字, 行, nodes(0).名字, nodes(0).内容)
                    Else
                        If 调用节点.类型 = "函数" Then
                            函数解释(调用节点)
                        Else
                            Return String.Format("函数节点[{0}]第{1}行：节点[{2}]不是函数节点，无法执行。", 节点.名字, 行, 调用节点.名字)
                        End If
                    End If
                Case "m-run", "多线程运行", "并行"
                    If nodesString.Length < 2 Then Return String.Format("函数节点[{1}]第{2}行：并行语句""{0}""过短。", targetString, 节点.名字, 行)
                    If nodes(0).类型 = "函数" Then
                        Dim t As New Thread(AddressOf 函数解释)
                        t.Start(nodes(0))
                    Else
                        Return String.Format("函数节点[{0}]第{1}行：节点[{2}]不是函数节点，无法执行。", 节点.名字, 行, nodes(0).名字)
                    End If
                Case "m-call", "多线程调用", "并用"
                    If nodesString.Length < 2 Then Return String.Format("函数节点[{1}]第{2}行：并用语句""{0}""过短。", targetString, 节点.名字, 行)
                    Dim 调用节点 As 节点类 = 获得节点(nodes(0).内容, nodes(0))
                    If 调用节点 Is Nothing Then
                        Return String.Format("函数节点[{0}]第{1}行：根据节点[{2}]的内容""{3}""未找到对应欲调用节点。", 节点.名字, 行, nodes(0).名字, nodes(0).内容)
                    Else
                        If 调用节点.类型 = "函数" Then
                            Dim t As New Thread(AddressOf 函数解释)
                            t.Start(调用节点)
                        Else
                            Return String.Format("函数节点[{0}]第{1}行：节点[{2}]不是函数节点，无法执行。", 节点.名字, 行, 调用节点.名字)
                        End If
                    End If
                Case "split", "分拆"
                    Select Case nodesString.Length
                        Case 4
                            nodes(0).父域 = New 节点平面类(Split(nodes(1).内容, nodes(2).内容))
                        Case 5
                            If nodes(0).空间.ContainsKey(nodes(1).内容) Then
                                nodes(0).空间(nodes(1).内容) = New 节点平面类(Split(nodes(2).内容, nodes(3).内容))
                            Else
                                Return String.Format("函数节点[{0}]第{1}行：分拆结果存放引用节点[{2}]的引用空间[{3}]未找到。", 节点.名字, 行, nodes(0).名字, nodes(1).名字)
                            End If
                        Case Else
                            Return String.Format("函数节点[{1}]第{2}行：分拆语句""{0}""参数数量不对。", targetString, 节点.名字, 行)
                    End Select
                Case "n-count", "节点数量"
                    Select Case nodesString.Length
                        Case 2
                            nodes(0).内容 = 节点.父域.本域节点.Count
                        Case 3
                            nodes(0).内容 = nodes(1).父域.本域节点.Count
                        Case 4
                            If nodes(1).空间.ContainsKey(nodes(2).内容) Then
                                nodes(0).内容 = nodes(1).空间(nodes(2).内容).本域节点.Count
                            Else
                                Return String.Format("函数节点[{0}]第{1}行：欲统计平面缺失，引用节点[{2}]的引用空间[{3}]未找到。", 节点.名字, 行, nodes(1).名字, nodes(2).名字)
                            End If
                        Case Else
                            Return String.Format("函数节点[{1}]第{2}行：平面内节点数量获取语句""{0}""参数数量不对。", targetString, 节点.名字, 行)
                    End Select
                Case "n-for", "节点遍历"
                    Select Case nodesString.Length
                        Case 3
                            If nodes(1).类型 = "函数" Then
                                节点遍历(节点.父域, nodes(0), nodes(1))
                            Else
                                Return String.Format("函数节点[{1}]第{2}行：遍历触发节点[{0}]不是函数点。", nodes(1).名字, 节点.名字, 行)
                            End If
                        Case 5
                            If nodes(0).空间.ContainsKey(nodes(1).内容) Then
                                If nodes(3).类型 = "函数" Then
                                    节点遍历(nodes(0).空间(nodes(1).内容), nodes(2), nodes(3))
                                Else
                                    Return String.Format("函数节点[{1}]第{2}行：遍历触发节点[{0}]不是函数点。", nodes(3).名字, 节点.名字, 行)
                                End If
                            Else
                                Return String.Format("函数节点[{0}]第{1}行：欲遍历平面缺失，引用节点[{2}]的引用空间[{3}]未找到。", 节点.名字, 行, nodes(0).名字, nodes(1).名字)
                            End If
                        Case Else
                            Return String.Format("函数节点[{1}]第{2}行：平面内节点遍历语句""{0}""参数数量不对。", targetString, 节点.名字, 行)
                    End Select
                Case "select", "选取"
                    If nodesString.Length < 2 Then Return String.Format("函数节点[{1}]第{2}行：选取语句""{0}""过短。", targetString, 节点.名字, 行)
                    控制台.添加消息("请用鼠标左击某节点完成选取操作……")
                    节点.高亮 = True
                    Do While 节点.父域.当前按住节点 Is Nothing
                        Thread.Sleep(10)
                    Loop
                    节点.高亮 = False
                    nodes(0).内容 = 节点.父域.当前按住节点.名字
                Case "type", "类型"
                    If nodesString.Length < 3 Then Return String.Format("函数节点[{1}]第{2}行：获取节点类型语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).内容 = nodes(1).类型
                Case "copytype", "复制类型"
                    If nodesString.Length < 3 Then Return String.Format("函数节点[{1}]第{2}行：复制节点类型语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).类型 = nodes(1).类型
                Case "settype", "设置类型"
                    If nodesString.Length < 3 Then Return String.Format("函数节点[{1}]第{2}行：设置节点类型语句""{0}""过短。", targetString, 节点.名字, 行)
                    Select Case nodes(1).内容
                        Case "值", "0"
                            nodes(0).类型 = "值"
                        Case "引用", "1"
                            nodes(0).类型 = "引用"
                        Case "函数", "2"
                            nodes(0).类型 = "函数"
                        Case Else
                            Return String.Format("函数节点[{1}]第{2}行：无节点类型""{0}""。", nodes(1).内容, 节点.名字, 行)
                    End Select
                Case "setpos", "设置位置"
                    If nodesString.Length < 4 Then Return String.Format("函数节点[{1}]第{2}行：设置节点位置语句""{0}""过短。", targetString, 节点.名字, 行)
                    Dim 新位置 As New Point(Val(nodes(1).内容), Val(nodes(2).内容))
                    Dim 设置结果 As Integer = nodes(0).父域.编辑节点位置(nodes(0).名字, 新位置)
                    Select Case 设置结果
                        Case 0
                            Return String.Format("函数节点[{1}]第{2}行：节点""{0}""未找到。", nodes(0).名字, 节点.名字, 行)
                        Case 1
                            Return String.Format("函数节点[{1}]第{2}行：目标位置""{0}""已有节点存在！", 新位置.ToString, 节点.名字, 行)
                    End Select
                Case "setname", "设置名字"
                    If nodesString.Length < 3 Then Return String.Format("函数节点[{1}]第{2}行：设置节点名字语句""{0}""过短。", targetString, 节点.名字, 行)
                    Dim 设置结果 As Integer = nodes(0).父域.编辑节点名(nodes(0).名字, nodes(1).内容)
                    Select Case 设置结果
                        Case 0
                            Return String.Format("函数节点[{1}]第{2}行：节点""{0}""未找到。", nodes(0).名字, 节点.名字, 行)
                        Case 1
                            Return String.Format("函数节点[{1}]第{2}行：新名字""{0}""已有节点使用！", nodes(1).内容, 节点.名字, 行)
                        Case 2
                            Return String.Format("函数节点[{1}]第{2}行：新名字""{0}""不合法！", nodes(1).内容, 节点.名字, 行)
                    End Select
                Case "netsend", "网络发送", "出口"
                    If nodesString.Length < 5 Then Return String.Format("函数节点[{1}]第{2}行：出口语句""{0}""过短。", targetString, 节点.名字, 行)
                    If nodes(0).类型 = "接口" Then
                        If nodes(0).接口类型 = "网络出口" Then
                            Try
                                nodes(0).发送数据(nodes(1).内容, nodes(2).内容, nodes(3).内容)
                            Catch ex As Exception
                                Return String.Format("函数节点[{1}]第{2}行：接口节点[{0}]发送数据出错：{3}。", nodes(0).名字, 节点.名字, 行, ex.Message)
                            End Try
                        Else
                            Return String.Format("函数节点[{1}]第{2}行：接口节点[{0}]不是出口节点。", nodes(0).名字, 节点.名字, 行)
                        End If
                    Else
                        Return String.Format("函数节点[{1}]第{2}行：节点[{0}]不是接口节点。", nodes(0).名字, 节点.名字, 行)
                    End If
                Case "<<=", "<<c", "getcontent", "获得内容"
                    If nodesString.Length < 3 Then Return String.Format("函数节点[{1}]第{2}行：获取指定节点内容语句""{0}""过短。", targetString, 节点.名字, 行)
                    Dim 目标节点 As 节点类 = 获得节点(nodes(1).内容, 节点)
                    If 目标节点 Is Nothing Then
                        Return String.Format("函数节点[{0}]第{1}行：目标节点[{2}]未找到。", 节点.名字, 行, nodes(1).内容)
                    Else
                        nodes(0).内容 = 目标节点.内容
                    End If
                Case "<<t", "gettype", "获得类型"
                    If nodesString.Length < 3 Then Return String.Format("函数节点[{1}]第{2}行：获取指定节点类型语句""{0}""过短。", targetString, 节点.名字, 行)
                    Dim 目标节点 As 节点类 = 获得节点(nodes(1).内容, 节点)
                    If 目标节点 Is Nothing Then
                        Return String.Format("函数节点[{0}]第{1}行：目标节点[{2}]未找到。", 节点.名字, 行, nodes(1).内容)
                    Else
                        nodes(0).内容 = 目标节点.类型
                    End If
                Case "<<n", "getname", "获得名字"
                    If nodesString.Length < 3 Then Return String.Format("函数节点[{1}]第{2}行：获取指定节点名字语句""{0}""过短。", targetString, 节点.名字, 行)
                    Dim 目标节点 As 节点类 = 获得节点(nodes(1).内容, 节点)
                    If 目标节点 Is Nothing Then
                        Return String.Format("函数节点[{0}]第{1}行：目标节点[{2}]未找到。", 节点.名字, 行, nodes(1).内容)
                    Else
                        nodes(0).内容 = 目标节点.名字
                    End If
                Case "<<x", "getx", "获得横坐标"
                    If nodesString.Length < 3 Then Return String.Format("函数节点[{1}]第{2}行：获取指定节点横坐标语句""{0}""过短。", targetString, 节点.名字, 行)
                    Dim 目标节点 As 节点类 = 获得节点(nodes(1).内容, 节点)
                    If 目标节点 Is Nothing Then
                        Return String.Format("函数节点[{0}]第{1}行：目标节点[{2}]未找到。", 节点.名字, 行, nodes(1).内容)
                    Else
                        nodes(0).内容 = 目标节点.位置.X
                    End If
                Case "<<y", "gety", "获得纵坐标"
                    If nodesString.Length < 3 Then Return String.Format("函数节点[{1}]第{2}行：获取指定节点纵坐标语句""{0}""过短。", targetString, 节点.名字, 行)
                    Dim 目标节点 As 节点类 = 获得节点(nodes(1).内容, 节点)
                    If 目标节点 Is Nothing Then
                        Return String.Format("函数节点[{0}]第{1}行：目标节点[{2}]未找到。", 节点.名字, 行, nodes(1).内容)
                    Else
                        nodes(0).内容 = 目标节点.位置.Y
                    End If
                Case "l-for", "连接遍历"
                    If nodesString.Length < 4 Then Return String.Format("函数节点[{1}]第{2}行：连接遍历语句""{0}""过短。", targetString, 节点.名字, 行)
                    If nodes(2).类型 = "函数" Then
                        连接遍历(nodes(0), nodes(1), nodes(2))
                    Else
                        Return String.Format("函数节点[{1}]第{2}行：遍历触发节点[{0}]不是函数点。", nodes(2).名字, 节点.名字, 行)
                    End If
                Case Else
                    Return String.Format("函数节点[{1}]第{2}行：处理法则【{0}】未找到。", nodesString(0), 节点.名字, 行)
            End Select
        Catch ex As Exception
            Return String.Format("函数节点[{3}]第{4}行：处理法则【{0}】时错误：{1}(语句：{2})", nodesString(0), ex.Message, targetString, 节点.名字, 行)
        End Try

        Return ""
    End Function
    Public Sub 节点遍历(ByRef 平面 As 节点平面类, ByRef 反馈存放点 As 节点类, ByRef 函数点 As 节点类)
        Dim r As New List(Of String)
        For i As Long = 0 To 平面.本域节点.Count - 1
            反馈存放点.内容 = 平面.本域节点.Keys(i)
            函数解释(函数点)
        Next
    End Sub
    Public Sub 连接遍历(ByRef 源点 As 节点类, ByRef 反馈存放点 As 节点类, ByRef 函数点 As 节点类)
        Dim r As New List(Of String)
        For i As Long = 0 To 源点.连接.Count - 1
            反馈存放点.内容 = 源点.连接(i).名字
            函数解释(函数点)
        Next
    End Sub
End Class
