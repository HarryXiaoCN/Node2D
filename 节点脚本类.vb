Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Threading
Imports Node2D.节点平面类

Public Module 节点全局
    Public ReadOnly 控制台 As New NodeConsole
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
    Public Function 获得节点(路径 As String, ByRef 搜索发起节点 As 节点类) As 节点类
        Dim 域() As String = 路径.Split(".")
        Dim 起点 As Integer = 1
        Dim 子节点 As 节点类 = 搜索发起节点.获得子节点(域(0))
        If 子节点 Is Nothing Then
            If 搜索发起节点.父域.全局平面.ContainsKey(域(0)) Then
                If 搜索发起节点.父域.全局平面(域(0)).本域节点.ContainsKey(域(1)) Then
                    子节点 = 搜索发起节点.父域.全局平面(域(0)).本域节点(域(1))
                    起点 += 1
                End If
            End If
        End If
        For i As Integer = 起点 To UBound(域)
            If 子节点 Is Nothing Then
                Return Nothing
            Else
                Dim 上个节点 As 节点类 = 子节点
                子节点 = 子节点.获得子节点(域(i))
                If 子节点 Is Nothing And i < UBound(域) Then
                    If 上个节点.类型 = "引用" Then
                        If 上个节点.空间.ContainsKey(域(i)) Then
                            If 上个节点.空间(域(i)).本域节点.ContainsKey(域(i + 1)) Then
                                子节点 = 上个节点.空间(域(i)).本域节点(域(i + 1))
                                i += 1
                            End If
                        End If
                    End If
                    If 子节点 Is Nothing Then
                        If 搜索发起节点.父域.全局平面.ContainsKey(域(i)) Then
                            If 搜索发起节点.父域.全局平面(域(i)).本域节点.ContainsKey(域(i + 1)) Then
                                子节点 = 上个节点.空间(域(i)).本域节点(域(i + 1))
                                i += 1
                            End If
                        End If
                    End If
                End If
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
                        If 执行节点 IsNot Nothing Then
                            函数解释(执行节点)
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
                            Return String.Format("函数节点[{1}]第{2}行：切片语句""{0}""过短。", targetString, 节点.名字, 行)
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
                            Return String.Format("函数节点[{1}]第{2}行：判断语句""{0}""过短。", targetString, 节点.名字, 行)
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
                            Return String.Format("函数节点[{1}]第{2}行：判断语句""{0}""过短。", targetString, 节点.名字, 行)
                    End Select
                Case Else
                    Return String.Format("函数节点[{1}]第{2}行：处理法则【{0}】未找到。", nodesString(0), 节点.名字, 行)
            End Select
        Catch ex As Exception
            Return String.Format("函数节点[{3}]第{4}行：处理法则【{0}】时错误：{1}(语句：{2})", nodesString(0), ex.Message, targetString, 节点.名字, 行)
        End Try

        Return ""
    End Function

End Class
