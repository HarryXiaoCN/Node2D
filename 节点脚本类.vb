Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Threading
Imports Node2D.节点平面类
Imports System.Net
Imports System.Text

Public Module 节点全局
    Public 主界面 As Form1
    Public 控制台 As NodeConsole

    Public Structure 文本定位类
        Public 前缀 As String, 行 As Long, 列 As Long
        Public Sub New(beforeStr As String, row As Long, col As Long)
            前缀 = beforeStr
            行 = row
            列 = col
        End Sub
    End Structure
    Public Function 获得字符(s As String) As String
        If s = "" Then Return ""
        Dim r As New List(Of String)
        Dim sT() As String = Split(s, " ")
        For Each c As String In sT
            Dim ci As Integer = Val(c)
            Try
                r.Add(Chr(ci))
            Catch ex As Exception
                Dim b() As Byte = BitConverter.GetBytes(CUShort(c))
                r.Add(Text.Encoding.Unicode.GetString(b))
            End Try
        Next
        Return Join(r.ToArray, "")
    End Function
    Public Function 获得ASCII码(s As String) As String
        If s = "" Then Return ""
        Dim r As New List(Of String)
        For Each c As String In s
            Try
                r.Add(Asc(c))
            Catch ex As Exception
                Dim b() As Byte = Text.Encoding.Unicode.GetBytes(c)
                r.Add(BitConverter.ToUInt16(b))
            End Try
        Next
        Return Join(r.ToArray, " ")
    End Function
    Public Function 获得正则有效捕获(r As Match) As String
        For i As Integer = 1 To r.Groups.Count - 1
            If r.Groups(i).Value <> "" Then
                Return r.Groups(i).Value
            End If
        Next
        Return ""
    End Function

    Public Function 删除连接(ByRef n1 As 节点类, ByRef n2 As 节点类) As Integer
        If n1.父域.Equals(n2.父域) Then
            If n1.连接.Contains(n2) And n2.连接.Contains(n1) Then
                n1.连接.Remove(n2)
                n2.连接.Remove(n1)
                Return 2
            End If
            Return 1
        End If
        Return 0
    End Function
    Public Function 新建连接(ByRef n1 As 节点类, ByRef n2 As 节点类) As Integer
        If n1.父域.Equals(n2.父域) Then
            If 连接有效性判断(n1, n2) Then
                If Not n1.连接.Contains(n2) And Not n2.连接.Contains(n1) Then
                    n1.连接.Add(n2)
                    n2.连接.Add(n1)
                    Return 3
                End If
                Return 2
            End If
            Return 1
        End If
        Return 0
    End Function
    Public Function WebClient头导出(ByRef wc As WebClient) As String
        Dim r As New List(Of String)
        For Each h As String In wc.Headers
            r.Add(h)
        Next
        Return Join(r.ToArray, vbCrLf)
    End Function
    Public Sub WebClient头写入(ByRef wc As WebClient, c As String)
        Dim cT() As String = Split(c, vbCrLf)
        For Each h As String In cT
            If h.IndexOf(":") <> -1 Then
                wc.Headers.Add(h)
            End If
        Next
    End Sub
    Public Sub 程序卸载()
        Dim mKey As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.ClassesRoot
        Try
            If mKey.OpenSubKey(".n2d") IsNot Nothing Then
                mKey.DeleteSubKeyTree(".n2d")
            End If
            If mKey.OpenSubKey("Node2D") IsNot Nothing Then
                mKey.DeleteSubKeyTree("Node2D")
            End If
        Catch ex As Exception
            If MsgBox(String.Format("解除文件关联失败！是否尝试管理员权限重新运行程序注册？" & vbCrLf & "(错误提示：{0})", ex.Message), MsgBoxStyle.YesNo + vbExclamation, "节点平面") = MsgBoxResult.Yes Then
                管理员权限运行自己()
            End If
        End Try
    End Sub
    Public Sub 管理员权限运行自己()
        Try
            Dim procInfo As New ProcessStartInfo With {
                .UseShellExecute = True,
                .FileName = Application.ExecutablePath,
                .Verb = "runas"
            }
            Process.Start(procInfo)
            主界面.Close()
        Catch ex As Exception
            MsgBox("管理员权限运行失败，错误描述：" & ex.Message, 48)
        End Try
    End Sub
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
    Public Function StringToBool(s As String) As Boolean
        If s = "0" Or s = "" Or s.ToLower = "false" Then
            Return False
        End If
        Return True
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
    Public Function 获得平面(路径 As String, ByRef 搜索平面 As 节点平面类) As 节点平面类
        If 搜索平面.全局平面.ContainsKey(路径) Then
            Return 搜索平面.全局平面(路径)
        End If
        Return Nothing
    End Function
    Public Function 获得节点(路径 As String, ByRef 搜索平面 As 节点平面类) As 节点类
        If 搜索平面.本域节点.ContainsKey(路径) Then
            Return 搜索平面.本域节点(路径)
        End If
        Return Nothing
    End Function
    Public Function 获得节点(路径 As String, ByRef 搜索发起节点 As 节点类) As 节点类
        Dim 域() As String = 路径.Split(".")
        Dim 子节点 As 节点类 = 搜索发起节点.获得子节点(域(0))
        Dim 上个节点 As 节点类 = 搜索发起节点
        Dim 子平面 As 节点平面类 = 获得平面(域(0), 上个节点)
        If 域.Length > 1 Then
            Dim 当前域类 As String
            If 子节点 Is Nothing Then
                当前域类 = "平面"
            Else
                当前域类 = "节点"
            End If
            For i As Long = 1 To UBound(域)
                If 当前域类 = "节点" Then
                    上个节点 = 子节点
                    子节点 = 获得节点(域(i), 子节点)
                    If 子节点 Is Nothing Then
                        子平面 = 获得平面(域(i), 上个节点)
                        当前域类 = "平面"
                    Else
                        当前域类 = "节点"
                    End If
                Else
                    If 子平面 Is Nothing Then Return Nothing
                    子节点 = 获得节点(域(i), 子平面)
                    If 子节点 Is Nothing Then
                        子平面 = 获得平面(域(i), 子平面)
                        当前域类 = "平面"
                    Else
                        当前域类 = "节点"
                    End If
                End If
            Next
        End If
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
    Public 全局等待锁 As New List(Of Integer)
    Public 全局浏览器 As New Dictionary(Of String, WebBrowser)
    Public Sub 解释(ByRef 节点 As 节点类)
        Dim 执行线程 As New Thread(AddressOf 函数解释)
        执行线程.SetApartmentState(ApartmentState.STA)
        执行线程.Start(节点)
    End Sub
    Private Sub 函数解释(节点 As 节点类)
        If 节点.类型 <> "函数" Then
            控制台.添加消息(String.Format("函数节点[{0}]：不是函数节点，无法执行解释。", 节点.名字))
            Exit Sub
        End If
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
            If nodesString(i).StartsWith("$") Then
                Dim 指引名 As String = nodesString(i).Substring(1)
                Dim nodeTemp As 节点类 = 获得节点(指引名, 节点)
                If nodeTemp Is Nothing Then
                    Return String.Format("函数节点[{0}]第{1}行：指引节点[{2}]未找到。", 节点.名字, 行, 指引名)
                Else
                    nodes.Add(获得节点(nodeTemp.内容, 节点))
                End If
            Else
                nodes.Add(获得节点(nodesString(i), 节点))
            End If
            If nodes.Last Is Nothing Then Return String.Format("函数节点[{0}]第{1}行：参与节点[{2}]未找到。", 节点.名字, 行, nodesString(i))
        Next
        If 主界面.激活节点变色.Checked Then
            节点.激活 = 180
            For Each n As 节点类 In nodes
                n.激活 = 180
            Next
        End If
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
                    Select Case nodesString.Length
                        Case 4
                            If Val(nodes(0).内容) > Val(nodes(1).内容) Then
                                函数解释(nodes(2))
                            End If
                        Case 5
                            If Val(nodes(0).内容) > Val(nodes(1).内容) Then
                                函数解释(nodes(2))
                            Else
                                函数解释(nodes(3))
                            End If
                        Case Else
                            Return String.Format("函数节点[{1}]第{2}行：大于判断语句""{0}""参数数量不对。", targetString, 节点.名字, 行)
                    End Select
                Case "<", "小于"
                    Select Case nodesString.Length
                        Case 4
                            If Val(nodes(0).内容) < Val(nodes(1).内容) Then
                                函数解释(nodes(2))
                            End If
                        Case 5
                            If Val(nodes(0).内容) < Val(nodes(1).内容) Then
                                函数解释(nodes(2))
                            Else
                                函数解释(nodes(3))
                            End If
                        Case Else
                            Return String.Format("函数节点[{1}]第{2}行：小于判断语句""{0}""参数数量不对。", targetString, 节点.名字, 行)
                    End Select
                Case ">=", "大于或等于"
                    Select Case nodesString.Length
                        Case 4
                            If Val(nodes(0).内容) >= Val(nodes(1).内容) Then
                                函数解释(nodes(2))
                            End If
                        Case 5
                            If Val(nodes(0).内容) >= Val(nodes(1).内容) Then
                                函数解释(nodes(2))
                            Else
                                函数解释(nodes(3))
                            End If
                        Case Else
                            Return String.Format("函数节点[{1}]第{2}行：大于或等于判断语句""{0}""参数数量不对。", targetString, 节点.名字, 行)
                    End Select
                Case "<=", "小于或等于"
                    Select Case nodesString.Length
                        Case 4
                            If Val(nodes(0).内容) <= Val(nodes(1).内容) Then
                                函数解释(nodes(2))
                            End If
                        Case 5
                            If Val(nodes(0).内容) <= Val(nodes(1).内容) Then
                                函数解释(nodes(2))
                            Else
                                函数解释(nodes(3))
                            End If
                        Case Else
                            Return String.Format("函数节点[{1}]第{2}行：小于或等于判断语句""{0}""参数数量不对。", targetString, 节点.名字, 行)
                    End Select
                Case "==", "等于"
                    Select Case nodesString.Length
                        Case 4
                            If Val(nodes(0).内容) = Val(nodes(1).内容) Then
                                函数解释(nodes(2))
                            End If
                        Case 5
                            If Val(nodes(0).内容) = Val(nodes(1).内容) Then
                                函数解释(nodes(2))
                            Else
                                函数解释(nodes(3))
                            End If
                        Case Else
                            Return String.Format("函数节点[{1}]第{2}行：等于判断语句""{0}""参数数量不对。", targetString, 节点.名字, 行)
                    End Select
                Case "!=", "<>", "不等于"
                    Select Case nodesString.Length
                        Case 4
                            If Val(nodes(0).内容) <> Val(nodes(1).内容) Then
                                函数解释(nodes(2))
                            End If
                        Case 5
                            If Val(nodes(0).内容) <> Val(nodes(1).内容) Then
                                函数解释(nodes(2))
                            Else
                                函数解释(nodes(3))
                            End If
                        Case Else
                            Return String.Format("函数节点[{1}]第{2}行：不等于判断语句""{0}""参数数量不对。", targetString, 节点.名字, 行)
                    End Select
                Case "and", "与"
                    Select Case nodesString.Length
                        Case 4
                            If CBool(Val(nodes(0).内容)) And CBool(Val(nodes(1).内容)) Then
                                函数解释(nodes(2))
                            End If
                        Case 5
                            If CBool(Val(nodes(0).内容) And Val(nodes(1).内容)) Then
                                函数解释(nodes(2))
                            Else
                                函数解释(nodes(3))
                            End If
                        Case Else
                            Return String.Format("函数节点[{1}]第{2}行：与运算判断语句""{0}""参数数量不对。", targetString, 节点.名字, 行)
                    End Select
                Case "or", "或"
                    Select Case nodesString.Length
                        Case 4
                            If CBool(Val(nodes(0).内容)) Or CBool(Val(nodes(1).内容)) Then
                                函数解释(nodes(2))
                            End If
                        Case 5
                            If CBool(Val(nodes(0).内容)) Or CBool(Val(nodes(1).内容)) Then
                                函数解释(nodes(2))
                            Else
                                函数解释(nodes(3))
                            End If
                        Case Else
                            Return String.Format("函数节点[{1}]第{2}行：或运算判断语句""{0}""参数数量不对。", targetString, 节点.名字, 行)
                    End Select
                Case "!", "not", "非"
                    Select Case nodesString.Length
                        Case 3
                            If Not CBool(Val(nodes(0).内容)) Then
                                函数解释(nodes(1))
                            End If
                        Case 4
                            If Not CBool(Val(nodes(0).内容)) Then
                                函数解释(nodes(1))
                            Else
                                函数解释(nodes(2))
                            End If
                        Case Else
                            Return String.Format("函数节点[{1}]第{2}行：非运算判断语句""{0}""参数数量不对。", targetString, 节点.名字, 行)
                    End Select
                Case "xor", "异或"
                    Select Case nodesString.Length
                        Case 4
                            If CBool(Val(nodes(0).内容)) Xor CBool(Val(nodes(1).内容)) Then
                                函数解释(nodes(2))
                            End If
                        Case 5
                            If CBool(Val(nodes(0).内容)) Xor CBool(Val(nodes(1).内容)) Then
                                函数解释(nodes(2))
                            Else
                                函数解释(nodes(3))
                            End If
                        Case Else
                            Return String.Format("函数节点[{1}]第{2}行：异或运算判断语句""{0}""参数数量不对。", targetString, 节点.名字, 行)
                    End Select
                Case "in", "indexof", "存在"
                    Select Case nodesString.Length
                        Case 4
                            nodes(0).内容 = nodes(1).内容.IndexOf(nodes(2).内容)
                        Case 5
                            Dim r As Long = nodes(1).内容.IndexOf(nodes(2).内容)
                            nodes(0).内容 = r
                            If r <> -1 Then
                                函数解释(nodes(3))
                            End If
                        Case 6
                            Dim r As Long = nodes(1).内容.IndexOf(nodes(2).内容)
                            nodes(0).内容 = r
                            If r <> -1 Then
                                函数解释(nodes(3))
                            Else
                                函数解释(nodes(4))
                            End If
                        Case Else
                            Return String.Format("函数节点[{1}]第{2}行：存在判断语句""{0}""参数数量不对。", targetString, 节点.名字, 行)
                    End Select
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
                        函数解释(调用节点)
                    End If
                Case "m-run", "多线程运行", "并行"
                    If nodesString.Length < 2 Then Return String.Format("函数节点[{1}]第{2}行：并行语句""{0}""过短。", targetString, 节点.名字, 行)
                    Dim t As New Thread(AddressOf 函数解释)
                    t.Start(nodes(0))
                Case "m-call", "多线程调用", "并用"
                    If nodesString.Length < 2 Then Return String.Format("函数节点[{1}]第{2}行：并用语句""{0}""过短。", targetString, 节点.名字, 行)
                    Dim 调用节点 As 节点类 = 获得节点(nodes(0).内容, nodes(0))
                    If 调用节点 Is Nothing Then
                        Return String.Format("函数节点[{0}]第{1}行：根据节点[{2}]的内容""{3}""未找到对应欲调用节点。", 节点.名字, 行, nodes(0).名字, nodes(0).内容)
                    Else
                        Dim t As New Thread(AddressOf 函数解释)
                        t.Start(调用节点)
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
                            节点遍历(节点.父域, nodes(0), nodes(1))
                        Case 5
                            If nodes(0).空间.ContainsKey(nodes(1).内容) Then
                                节点遍历(nodes(0).空间(nodes(1).内容), nodes(2), nodes(3))
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
                    If nodesString.Length < 5 Then Return String.Format("函数节点[{1}]第{2}行：设置节点位置语句""{0}""过短。", targetString, 节点.名字, 行)
                    Dim 新位置 As New Point(Val(nodes(2).内容), Val(nodes(3).内容))
                    nodes(0).内容 = nodes(1).父域.编辑节点位置(nodes(1).名字, 新位置)
                Case "setname", "设置名字"
                    If nodesString.Length < 4 Then Return String.Format("函数节点[{1}]第{2}行：设置节点名字语句""{0}""过短。", targetString, 节点.名字, 行)
                    Dim 设置结果 As Integer = nodes(1).父域.编辑节点名(nodes(1).名字, nodes(2).内容)
                    nodes(0).内容 = 设置结果
                    Select Case 设置结果
                        Case 0
                            Return String.Format("函数节点[{1}]第{2}行：节点""{0}""未找到。", nodes(1).名字, 节点.名字, 行)
                        Case 1
                            Return String.Format("函数节点[{1}]第{2}行：新名字""{0}""已有节点使用！", nodes(2).内容, 节点.名字, 行)
                        Case 2
                            Return String.Format("函数节点[{1}]第{2}行：新名字""{0}""不合法！", nodes(2).内容, 节点.名字, 行)
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
                    连接遍历(nodes(0), nodes(1), nodes(2))

                Case "re-test", "正则测试"
                    If nodesString.Length < 4 Then Return String.Format("函数节点[{1}]第{2}行：正则匹配测试语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).内容 = BoolToInt(Regex.IsMatch(nodes(1).内容, nodes(2).内容))
                Case "split-for", "拆分循环"
                    If nodesString.Length < 5 Then Return String.Format("函数节点[{1}]第{2}行：拆分循环语句""{0}""过短。", targetString, 节点.名字, 行)
                    Dim cT() As String = Split(nodes(0).内容, nodes(1).内容)
                    For i As Long = 0 To UBound(cT)
                        nodes(2).内容 = cT(i)
                        函数解释(nodes(3))
                    Next
                Case "add2d", "新增平面", "增加平面", "新建平面"
                    If nodesString.Length < 2 Then Return String.Format("函数节点[{1}]第{2}行：新增平面语句""{0}""过短。", targetString, 节点.名字, 行)
                    Dim 新平面 As New 节点平面类()
                    新平面.保存(nodes(0).内容)
                Case "addnode", "新增节点", "增加节点", "新建节点"
                    If nodesString.Length < 6 Then Return String.Format("函数节点[{1}]第{2}行：新增节点语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).内容 = 节点.父域.新建节点(nodes(1).内容, nodes(2).内容, New Point(Val(nodes(3).内容), Val(nodes(4).内容)))
                Case "delnode", "删除节点"
                    If nodesString.Length < 2 Then Return String.Format("函数节点[{1}]第{2}行：删除节点语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).删除()
                Case "addline", "新增连接", "增加连接", "新建连接"
                    If nodesString.Length < 4 Then Return String.Format("函数节点[{1}]第{2}行：新增连接语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).内容 = 新建连接(nodes(1), nodes(2))
                Case "delline", "删除连接"
                    If nodesString.Length < 4 Then Return String.Format("函数节点[{1}]第{2}行：删除连接语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).内容 = 删除连接(nodes(1), nodes(2))
                Case "waitkeydown", "等待按键"
                    If nodesString.Length < 3 Then Return String.Format("函数节点[{1}]第{2}行：等待按键语句""{0}""过短。", targetString, 节点.名字, 行)
                    '第3个参数意义： 0=nothing 1 -alt 2-ctrl 3-ctrl+alt 4-shift 5-alt+shift 6-ctrl+shift 7-ctrl+shift+alt
                    Dim tid As Integer = Thread.CurrentThread.ManagedThreadId
                    控制台.热键注册(控制台.获得窗体句柄(), tid, Val(nodes(0).内容), Val(nodes(1).内容))
                    全局等待锁.Add(Thread.CurrentThread.ManagedThreadId)
                    Do While 全局等待锁.Contains(tid)
                        Application.DoEvents()
                        Thread.Sleep(10)
                    Loop
                    控制台.热键卸载(控制台.获得窗体句柄(), tid)
                Case "sendkeys", "发送按键"
                    If nodesString.Length < 2 Then Return String.Format("函数节点[{1}]第{2}行：发送按键语句""{0}""过短。", targetString, 节点.名字, 行)
                    SendKeys.SendWait(nodes(0).内容)
                Case "like", "像", "相似"
                    If nodesString.Length < 4 Then Return String.Format("函数节点[{1}]第{2}行：判断相似语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).内容 = BoolToInt(nodes(1).内容 Like nodes(2).内容)
                Case "rnd", "随机数"
                    Select Case targetString.Length
                        Case 2
                            Dim r As New Random
                            nodes(0).内容 = r.NextDouble
                        Case 3
                            Dim r As New Random(Val(nodes(1).内容))
                            nodes(0).内容 = r.NextDouble
                        Case Else
                            Return String.Format("函数节点[{1}]第{2}行：获取随机数语句""{0}""参数数量不对。", targetString, 节点.名字, 行)
                    End Select
                Case "addglobal2d", "添加全局平面引用"
                    Select Case targetString.Length
                        Case 2
                            If nodes(1).内容 = "" Then Return String.Format("函数节点[{1}]第{2}行：节点[{0}]内容不能为空。", nodes(1).名字, 节点.名字, 行)
                            If Not nodes(0).父域.全局窗体.全局平面列表.Items.Contains(nodes(1).内容) Then
                                nodes(0).父域.添加全局平面(nodes(1).内容)
                            Else
                                Return String.Format("函数节点[{1}]第{2}行：平面""{0}""已引用。", nodes(1).内容, 节点.名字, 行)
                            End If
                        Case 3
                            If nodes(2).内容 = "" Then Return String.Format("函数节点[{1}]第{2}行：节点[{0}]内容不能为空。", nodes(2).名字, 节点.名字, 行)
                            If nodes(0).空间.ContainsKey(nodes(1).内容) Then
                                If Not nodes(0).空间(nodes(1).内容).全局窗体.全局平面列表.Items.Contains(nodes(2).内容) Then
                                    nodes(0).空间(nodes(1).内容).添加全局平面(nodes(2).内容)
                                Else
                                    Return String.Format("函数节点[{1}]第{2}行：平面""{0}""已引用。", nodes(2).内容, 节点.名字, 行)
                                End If
                            Else
                                Return String.Format("函数节点[{0}]第{1}行：未在节点[{2}]找到平面""{3}""。", 节点.名字, 行, nodes(0).名字, nodes(1).内容)
                            End If
                        Case Else
                            Return String.Format("函数节点[{1}]第{2}行：添加全局平面引用语句""{0}""参数数量不对。", targetString, 节点.名字, 行)
                    End Select
                Case "delgloabl2d", "删除全局平面引用"
                    Select Case targetString.Length
                        Case 2
                            If nodes(1).内容 = "" Then Return String.Format("函数节点[{1}]第{2}行：节点[{0}]内容不能为空。", nodes(1).名字, 节点.名字, 行)
                            If nodes(0).父域.全局窗体.全局平面列表.Items.Contains(nodes(1).内容) Then
                                nodes(0).父域.移除全局平面(nodes(1).内容)
                            Else
                                Return String.Format("函数节点[{1}]第{2}行：未全局引用平面""{0}""。", nodes(1).内容, 节点.名字, 行)
                            End If
                        Case 3
                            If nodes(2).内容 = "" Then Return String.Format("函数节点[{1}]第{2}行：节点[{0}]内容不能为空。", nodes(2).名字, 节点.名字, 行)
                            If nodes(0).空间.ContainsKey(nodes(1).内容) Then
                                If nodes(0).空间(nodes(1).内容).全局窗体.全局平面列表.Items.Contains(nodes(2).内容) Then
                                    nodes(0).空间(nodes(1).内容).移除全局平面(nodes(2).内容)
                                Else
                                    Return String.Format("函数节点[{1}]第{2}行：未全局引用平面""{0}""。", nodes(2).内容, 节点.名字, 行)
                                End If
                            Else
                                Return String.Format("函数节点[{0}]第{1}行：未在节点[{2}]找到平面""{3}""。", 节点.名字, 行, nodes(0).名字, nodes(1).内容)
                            End If
                        Case Else
                            Return String.Format("函数节点[{1}]第{2}行：删除全局平面引用语句""{0}""参数数量不对。", targetString, 节点.名字, 行)
                    End Select
                Case "addglobalnode", "添加全局节点引用"
                    Select Case targetString.Length
                        Case 2
                            If nodes(1).内容 = "" Then Return String.Format("函数节点[{1}]第{2}行：节点[{0}]内容不能为空。", nodes(1).名字, 节点.名字, 行)
                            If Not nodes(0).父域.全局窗体.全局节点列表.Items.Contains(nodes(1).内容) Then
                                If nodes(0).父域.本域节点.ContainsKey(nodes(1).内容) Then
                                    nodes(0).父域.全局窗体.全局节点列表.Items.Add(nodes(1).内容)
                                Else
                                    Return String.Format("函数节点[{1}]第{2}行：节点""{0}""不在目标平面内。", nodes(1).内容, 节点.名字, 行)
                                End If
                            Else
                                Return String.Format("函数节点[{1}]第{2}行：节点""{0}""已引用。", nodes(1).内容, 节点.名字, 行)
                            End If
                        Case 3
                            If nodes(2).内容 = "" Then Return String.Format("函数节点[{1}]第{2}行：节点[{0}]内容不能为空。", nodes(2).名字, 节点.名字, 行)
                            If nodes(0).空间.ContainsKey(nodes(1).内容) Then
                                If Not nodes(0).空间(nodes(1).内容).全局窗体.全局平面列表.Items.Contains(nodes(2).内容) Then
                                    If nodes(0).空间(nodes(1).内容).本域节点.ContainsKey(nodes(2).内容) Then
                                        nodes(0).空间(nodes(1).内容).全局窗体.全局节点列表.Items.Add(nodes(2).内容)
                                    Else
                                        Return String.Format("函数节点[{1}]第{2}行：节点""{0}""不在目标平面内。", nodes(2).内容, 节点.名字, 行)
                                    End If
                                Else
                                    Return String.Format("函数节点[{1}]第{2}行：节点""{0}""已引用。", nodes(2).内容, 节点.名字, 行)
                                End If
                            Else
                                Return String.Format("函数节点[{0}]第{1}行：未在节点[{2}]找到平面""{3}""。", 节点.名字, 行, nodes(0).名字, nodes(1).内容)
                            End If
                        Case Else
                            Return String.Format("函数节点[{1}]第{2}行：添加全局节点引用语句""{0}""参数数量不对。", targetString, 节点.名字, 行)
                    End Select
                Case "delglobalnode", "删除全局节点引用"
                    Select Case targetString.Length
                        Case 2
                            If nodes(1).内容 = "" Then Return String.Format("函数节点[{1}]第{2}行：节点[{0}]内容不能为空。", nodes(1).名字, 节点.名字, 行)
                            If nodes(0).父域.全局窗体.全局节点列表.Items.Contains(nodes(1).内容) Then
                                nodes(0).父域.全局窗体.全局节点列表.Items.Remove(nodes(1).内容)
                            Else
                                Return String.Format("函数节点[{1}]第{2}行：未全局引用节点""{0}""。", nodes(1).内容, 节点.名字, 行)
                            End If
                        Case 3
                            If nodes(2).内容 = "" Then Return String.Format("函数节点[{1}]第{2}行：节点[{0}]内容不能为空。", nodes(2).名字, 节点.名字, 行)
                            If nodes(0).空间.ContainsKey(nodes(1).内容) Then
                                If nodes(0).空间(nodes(1).内容).全局窗体.全局平面列表.Items.Contains(nodes(2).内容) Then
                                    nodes(0).空间(nodes(1).内容).全局窗体.全局节点列表.Items.Remove(nodes(2).内容)
                                Else
                                    Return String.Format("函数节点[{1}]第{2}行：未全局引用节点""{0}""。", nodes(2).内容, 节点.名字, 行)
                                End If
                            Else
                                Return String.Format("函数节点[{0}]第{1}行：未在节点[{2}]找到平面""{3}""。", 节点.名字, 行, nodes(0).名字, nodes(1).内容)
                            End If
                        Case Else
                            Return String.Format("函数节点[{1}]第{2}行：删除全局节点引用语句""{0}""参数数量不对。", targetString, 节点.名字, 行)
                    End Select
                Case "shell", "命令"
                    Try
                        Select Case targetString.Length
                            Case 2
                                Shell(nodes(0).内容)
                            Case 3
                                Shell(nodes(0).内容, nodes(1).内容)
                            Case 4
                                nodes(0).内容 = Shell(nodes(1).内容, nodes(2).内容)
                            Case Else
                                Return String.Format("函数节点[{1}]第{2}行：命令语句""{0}""参数数量不对。", targetString, 节点.名字, 行)
                        End Select
                    Catch ex As Exception
                        Return String.Format("函数节点[{1}]第{2}行：命令""{3}""执行错误：{0}。", ex.Message, 节点.名字, 行, nodes(0).内容)
                    End Try
                Case "l-conut", "连接数量"
                    If targetString.Length < 3 Then Return String.Format("函数节点[{0}]第{1}行：获取连接数量语句""{2}""过短。", 节点.名字, 行, targetString)
                    nodes(0).内容 = nodes(1).连接.Count
                Case "mypath", "平面路径"
                    Select Case targetString.Length
                        Case 2
                            nodes(0).内容 = 节点.父域.路径
                        Case 3
                            nodes(0).内容 = nodes(1).父域.路径
                        Case Else
                            Return String.Format("函数节点[{1}]第{2}行：获取平面路径语句""{0}""参数数量不对。", targetString, 节点.名字, 行)
                    End Select
                Case "now", "当前时间"
                    Select Case targetString.Length
                        Case 2
                            nodes(0).内容 = Now.ToOADate
                        Case 3
                            nodes(0).内容 = Now.ToString(nodes(1).内容)
                        Case Else
                            Return String.Format("函数节点[{1}]第{2}行：获取当前时间语句""{0}""参数数量不对。", targetString, 节点.名字, 行)
                    End Select
                Case "save", "保存平面"
                    Select Case targetString.Length
                        Case 2
                            节点.父域.保存(nodes(0).内容)
                        Case 3
                            nodes(0).父域.保存(nodes(1).内容)
                        Case 4
                            nodes(0).空间(nodes(1).内容).保存(nodes(2).内容)
                        Case Else
                            Return String.Format("函数节点[{1}]第{2}行：保存平面语句""{0}""参数数量不对。", targetString, 节点.名字, 行)
                    End Select
                Case "addfunction", "addfun", "增加法则"
                    If targetString.Length < 3 Then Return String.Format("函数节点[{1}]第{2}行：增加用户法则语句""{0}""过短。", targetString, 节点.名字, 行)
                    Dim 参数(nodes.Count - 2) As 节点类
                    nodes.CopyTo(1, 参数, 0, nodes.Count - 1)
                    If 节点.父域.增加用户法则(nodes(0), 参数) Then
                        Return String.Format("函数节点[{1}]第{2}行：用户法则""{0}""添加成功！参数数量：{3}。", nodes(0).名字, 节点.名字, 行, 参数.Length)
                    Else
                        Return String.Format("函数节点[{1}]第{2}行：用户法则""{0}""修改完毕！参数数量：{3}。", nodes(0).名字, 节点.名字, 行, 参数.Length)
                    End If
                Case "delfunction", "delfun", "删除法则"
                    If targetString.Length < 2 Then Return String.Format("函数节点[{1}]第{2}行：删除用户法则语句""{0}""过短。", targetString, 节点.名字, 行)
                    If 节点.父域.用户法则.ContainsKey(nodes(0).内容) Then
                        节点.父域.用户法则.Remove(nodes(0).内容)
                        Return String.Format("函数节点[{1}]第{2}行：用户法则""{0}""已移除。", nodes(0).名字, 节点.名字, 行)
                    Else
                        Return String.Format("函数节点[{1}]第{2}行：用户法则""{0}""不存在。", nodes(0).名字, 节点.名字, 行)
                    End If
                Case "downloadstring", "下载字符串", "下载文本"
                    If targetString.Length < 4 Then Return String.Format("函数节点[{1}]第{2}行：下载字符串语句""{0}""过短。", targetString, 节点.名字, 行)
                    Dim wc As New WebClient
                    Try
                        WebClient头写入(wc, nodes(2).内容)
                        nodes(0).内容 = wc.DownloadString(nodes(1).内容)
                        nodes(2).内容 = WebClient头导出(wc)
                    Catch ex As Exception
                        Return String.Format("函数节点[{1}]第{2}行：下载字符串出错：{0}", ex.Message, 节点.名字, 行)
                    End Try
                    wc.Dispose()
                Case "replace", "替换"
                    If targetString.Length < 5 Then Return String.Format("函数节点[{1}]第{2}行：替换语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).内容 = Replace(nodes(1).内容, nodes(2).内容, nodes(3).内容)
                Case "visible", "显示"
                    If targetString.Length < 6 Then Return String.Format("函数节点[{1}]第{2}行：显示语句""{0}""过短。", targetString, 节点.名字, 行)
                    主界面.显示(nodes(0).内容)
                    控制台.显示(nodes(1).内容)
                    主界面.托盘显示(nodes(2).内容, nodes(3).内容, nodes(4).内容)
                Case "re-matches-for", "re-ms-for", "正则匹配遍历"
                    If targetString.Length < 6 Then Return String.Format("函数节点[{1}]第{2}行：正则匹配遍历语句""{0}""过短。", targetString, 节点.名字, 行)
                    Dim reResult = Regex.Matches(nodes(0).内容, nodes(1).内容)
                    For Each m As Match In reResult
                        nodes(2).内容 = m.Value
                        nodes(3).内容 = 获得正则有效捕获(m)
                        函数解释(nodes(4))
                    Next
                Case "re-match", "re-m", "正则捕获"
                    If targetString.Length < 4 Then Return String.Format("函数节点[{1}]第{2}行：正则捕获语句""{0}""过短。", targetString, 节点.名字, 行)
                    Dim reResult = Regex.Match(nodes(0).内容, nodes(1).内容)
                    nodes(2).内容 = 获得正则有效捕获(reResult)
                Case "debug-append", "debug-a", "调试追加写入"
                    If targetString.Length < 2 Then Return String.Format("函数节点[{1}]第{2}行：调试追加写入语句""{0}""过短。", targetString, 节点.名字, 行)
                    控制台.添加消息(nodes(0).内容)
                Case "debug-write", "debug-w", "调试覆盖写入"
                    If targetString.Length < 2 Then Return String.Format("函数节点[{1}]第{2}行：调试覆盖写入语句""{0}""过短。", targetString, 节点.名字, 行)
                    控制台.设定消息(nodes(0).内容)
                Case "asc", "ord", "编码"
                    If targetString.Length < 3 Then Return String.Format("函数节点[{1}]第{2}行：获得字符编码语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).内容 = 获得ASCII码(nodes(1).内容)
                Case "chr", "字符"
                    If targetString.Length < 3 Then Return String.Format("函数节点[{1}]第{2}行：获得对应字符语句""{0}""过短。", targetString, 节点.名字, 行)
                    nodes(0).内容 = 获得字符(nodes(1).内容)
                Case "add-webbrowser", "add-wb", "新建浏览器"
                    If targetString.Length < 2 Then Return String.Format("函数节点[{1}]第{2}行：新建浏览器语句""{0}""过短。", targetString, 节点.名字, 行)
                    If 全局浏览器.ContainsKey(nodes(0).内容) Then
                        Return String.Format("函数节点[{1}]第{2}行：浏览器""{0}""已存在。", nodes(0).内容, 节点.名字, 行)
                    Else
                        全局浏览器.Add(nodes(0).内容, New WebBrowser)
                        全局浏览器(nodes(0).内容).ScriptErrorsSuppressed = True
                    End If
                Case "navigate-wb", "navigate-webbrowser", "浏览器导航至"
                    If targetString.Length < 3 Then Return String.Format("函数节点[{1}]第{2}行：浏览器导航至语句""{0}""过短。", targetString, 节点.名字, 行)
                    If Not 全局浏览器.ContainsKey(nodes(0).内容) Then
                        Return String.Format("函数节点[{1}]第{2}行：浏览器""{0}""不存在。", nodes(0).内容, 节点.名字, 行)
                    End If
                    Select Case targetString.Length
                        Case 3
                            全局浏览器(nodes(0).内容).Navigate(nodes(1).内容)
                        Case 4
                            全局浏览器(nodes(0).内容).Navigate(nodes(1).内容, nodes(2).内容)
                        Case 6
                            全局浏览器(nodes(0).内容).Navigate(nodes(1).内容, nodes(2).内容, Encoding.UTF8.GetBytes(nodes(3).内容), nodes(4).内容)
                        Case Else
                            Return String.Format("函数节点[{1}]第{2}行：浏览器导航至语句""{0}""参数数量不对。", nodes(0).内容, 节点.名字, 行)
                    End Select
                Case "get-documenttext", "get-doctext", "获得网页内容"
                    If targetString.Length < 3 Then Return String.Format("函数节点[{1}]第{2}行：获得网页内容语句""{0}""过短。", targetString, 节点.名字, 行)
                    If Not 全局浏览器.ContainsKey(nodes(0).内容) Then
                        Return String.Format("函数节点[{1}]第{2}行：浏览器""{0}""不存在。", nodes(0).内容, 节点.名字, 行)
                    End If
                    nodes(1).内容 = 全局浏览器(nodes(0).内容).DocumentText
                Case "id-get-attribute", "id-get-att", "根据标识获得网页属性"
                    If targetString.Length < 5 Then Return String.Format("函数节点[{1}]第{2}行：获得网页属性语句""{0}""过短。", targetString, 节点.名字, 行)
                    If Not 全局浏览器.ContainsKey(nodes(0).内容) Then
                        Return String.Format("函数节点[{1}]第{2}行：浏览器""{0}""不存在。", nodes(0).内容, 节点.名字, 行)
                    End If
                    nodes(3).内容 = 全局浏览器(nodes(0).内容).Document.GetElementById(nodes(1).内容).GetAttribute(nodes(2).内容)
                Case "id-set-attribute", "id-set-att", "根据标识设置网页属性"
                    If targetString.Length < 5 Then Return String.Format("函数节点[{1}]第{2}行：设置网页属性语句""{0}""过短。", targetString, 节点.名字, 行)
                    If Not 全局浏览器.ContainsKey(nodes(0).内容) Then
                        Return String.Format("函数节点[{1}]第{2}行：浏览器""{0}""不存在。", nodes(0).内容, 节点.名字, 行)
                    End If
                    全局浏览器(nodes(0).内容).Document.GetElementById(nodes(1).内容).SetAttribute(nodes(2).内容， nodes(3).内容)
                Case "webbrowser-invokescript", "wb-script", "浏览器执行脚本"
                    If targetString.Length < 3 Then Return String.Format("函数节点[{1}]第{2}行：浏览器执行脚本语句""{0}""过短。", targetString, 节点.名字, 行)
                    If Not 全局浏览器.ContainsKey(nodes(0).内容) Then
                        Return String.Format("函数节点[{1}]第{2}行：浏览器""{0}""不存在。", nodes(0).内容, 节点.名字, 行)
                    End If
                    Dim 参数 As New List(Of String)
                    For i As Long = 2 To nodes.Count - 1
                        参数.Add(nodes(i).内容)
                    Next
                    If 参数.Count > 0 Then
                        全局浏览器(nodes(0).内容).Document.InvokeScript(nodes(1).内容, 参数.ToArray)
                    Else
                        全局浏览器(nodes(0).内容).Document.InvokeScript(nodes(1).内容)
                    End If
                Case "visible-webbrowser", "vis-wb", "浏览器显示"
                    If targetString.Length < 3 Then Return String.Format("函数节点[{1}]第{2}行：浏览器显示语句""{0}""过短。", targetString, 节点.名字, 行)
                    If Not 全局浏览器.ContainsKey(nodes(0).内容) Then
                        Return String.Format("函数节点[{1}]第{2}行：浏览器""{0}""不存在。", nodes(0).内容, 节点.名字, 行)
                    End If
                    全局浏览器(nodes(0).内容).Visible = StringToBool(nodes(1).内容)
                Case "close-webbrowser", "close-wb", "关闭浏览器"
                    If Not 全局浏览器.ContainsKey(nodes(0).内容) Then
                        Return String.Format("函数节点[{1}]第{2}行：浏览器""{0}""不存在。", nodes(0).内容, 节点.名字, 行)
                    End If
                    全局浏览器(nodes(0).内容).Dispose()
                    全局浏览器.Remove(nodes(0).内容)
                Case Else
                    Return 用户法则解释(节点, nodes, nodesString(0).ToLower, 行)
            End Select
        Catch ex As Exception
            Return String.Format("函数节点[{3}]第{4}行：处理法则【{0}】时错误：{1}(语句：{2})", nodesString(0), ex.Message, targetString, 节点.名字, 行)
        End Try
        Return ""
    End Function
    Public Function 用户法则解释(ByRef 节点 As 节点类, nodes As List(Of 节点类), 法则名 As String, 行 As Long) As String
        If 节点.父域.用户法则.ContainsKey(法则名) Then
            If nodes.Count = 节点.父域.用户法则(法则名).Count - 1 Then
                For i As Integer = 0 To nodes.Count - 1
                    节点.父域.用户法则(法则名)(i + 1).内容 = nodes(i).内容
                Next
                函数解释(节点.父域.用户法则(法则名)(0))
                For i As Integer = 0 To nodes.Count - 1
                    nodes(i).内容 = 节点.父域.用户法则(法则名)(i + 1).内容
                Next
                Return ""
            End If
            Return String.Format("函数节点[{1}]第{2}行：用户法则【{0}】参数不匹配：{3}/{4}。", 法则名, 节点.名字, 行, nodes.Count, 节点.父域.用户法则(法则名).Count - 1)
        End If
        Return String.Format("函数节点[{1}]第{2}行：处理法则【{0}】未找到。", 法则名, 节点.名字, 行)
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
