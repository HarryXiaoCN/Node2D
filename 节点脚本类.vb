Imports System.Text.RegularExpressions
Imports Node2D.节点平面类
Public Class 节点脚本类
    Public 主域 As 节点平面类
    Public Function 获得节点(路径 As String) As 节点类

    End Function
    Public Function 函数解释(节点 As 节点类) As String
        Dim 反馈 As New List(Of String)
        Dim 句集() As String = Split(节点.内容, "。")
        For i As Integer = 0 To UBound(句集)
            If 句集(i) <> "" And 句集(i).StartsWith("'") Then
                Dim 性质 As String = 式判断(句集(i), 节点)
                Select Case 性质
                    Case "执行式"
                        Dim 执行节点 As 节点类 = 获得节点(句集(i).Substring(0, 句集(i).Length - 2))
                        If 执行节点 IsNot Nothing Then
                            反馈.Add(函数解释(执行节点))
                        End If
                    Case "判断式"

                    Case "循环式"

                    Case "赋值式"

                    Case Else

                End Select
            End If
        Next
        Return Join(反馈.ToArray, vbCrLf)
    End Function

    Private Function 式判断(句 As String, 节点 As 节点类) As String
        If 句.EndsWith("()") Then
            Return "执行式" '默认将句末()前作为函数节点路径
        End If
        If Regex.IsMatch(句, "^若 [^ ]{1,}$") Or Regex.IsMatch(句, "^若 [^ ]{1,} [^ ]{1,}$") Then
            Return "判断式"
        End If
        If Regex.IsMatch(句, "^遍 [^ ]{1,} [^ ]{1,}$") Then
            Return "循环式"
        End If
        For i As Integer = 0 To 节点.连接.Count - 1
            If Regex.IsMatch(句, "^" & 节点.连接(i).名字 & "=[^=]") Then
                '此时只是获得了路径=
                Return "赋值式"
            End If
        Next
        Return ""
    End Function
    Private Function 解释执行式(行 As Integer, 句 As String, 函数点 As 节点类) As String
        Dim 节点名 As String = ""
        Return String.Format("在【{0}】中第{1}行：执行语法错误，未找到节点【{2}】！", 函数点.名字, 行, 节点名)
    End Function

    Private Function 解释赋值式(行 As Integer, 句 As String, 被赋值节点 As 节点类, 函数点 As 节点类) As String
        If 句.StartsWith(被赋值节点.名字 & "=") Then

        End If
        Return String.Format("在【{0}】中第{1}行：赋值语法错误，缺少""=""！", 函数点.名字, 行)
    End Function
End Class
