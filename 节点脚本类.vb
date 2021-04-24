Imports System.Text.RegularExpressions
Imports Node2D.节点平面类

Public Module 节点全局
    Public 控制台 As New NodeConsole
End Module
Public Class 节点脚本类

    Public 主域 As 节点平面类

    Public Sub New(节点 As 节点类)
        主域 = 节点.父域
    End Sub

    Public Function 获得节点(路径 As String, ByRef 搜索域 As 节点平面类, Optional 路径起点 As Long = 0) As 节点类
        Dim 域() As String = 路径.Split(".")
        If 搜索域.本域节点.ContainsKey(域(路径起点)) Then
            If 搜索域.本域节点(域(路径起点)).类型 = "引用" And UBound(域) > 路径起点 + 2 Then
                If 搜索域.本域节点(域(路径起点)).空间.ContainsKey(域(路径起点 + 1)) Then
                    Return 获得节点(路径, 搜索域.本域节点(域(路径起点)).空间(域(路径起点 + 1)), 路径起点 + 2)
                End If
                Return Nothing
            End If
            Return 搜索域.本域节点(域(路径起点))
        End If
        Return Nothing
    End Function
    Public Function 函数解释(节点 As 节点类) As String
        Dim 反馈 As New List(Of String)
        Dim 句集() As String = Split(节点.内容, vbCrLf)
        For i As Integer = 0 To UBound(句集)
            If 句集(i) <> "" And Not 句集(i).StartsWith("'") Then
                Dim 性质 As String = 式判断(句集(i))
                Select Case 性质
                    Case "执行式"
                        Dim 执行节点 As 节点类 = 获得节点(句集(i).Substring(0, 句集(i).Length - 2), 主域)
                        If 执行节点 IsNot Nothing Then
                            反馈.Add(函数解释(执行节点))
                        End If
                    Case "判断式"

                    Case "循环式"

                    Case "赋值式"
                        反馈.Add(赋值式运算(句集(i)))
                    Case Else
                        反馈.Add(String.Format("函数点[{0}]未识别句：{1}", 节点.名字, 句集(i)))
                End Select
            End If
        Next
        Return Join(反馈.ToArray, vbCrLf)
    End Function
    Public Function 赋值式运算(targetString As String) As String
        Dim result As New List(Of String)
        '禁用：空格 + . = 换行
        Dim nodesString() As String = Split(targetString, " ")
        Dim nodes As New Dictionary(Of String, 节点类)
        '0:赋值节点 1:=
        Dim targetNode As 节点类 = 获得节点(nodesString(0), 主域)
        If targetNode Is Nothing Then Return String.Format("被赋值节点[{0}]未找到。", nodesString(0))

        For i As Integer = 2 To UBound(nodesString)
            Select Case nodesString(i)
                Case "+"
                    If Not nodes.ContainsKey(nodesString(i + 1)) Then
                        nodes.Add(nodesString(i + 1), 获得节点(nodesString(i + 1), 主域))
                    End If
                    targetNode.内容 = Val(nodes(nodesString(i - 1)).内容) + Val(nodes(nodesString(i + 1)).内容)
                Case Else
                    If Not nodes.ContainsKey(nodesString(i)) Then
                        nodes.Add(nodesString(i), 获得节点(nodesString(i), 主域))
                    End If
            End Select
        Next

        Return Join(result.ToArray, vbCrLf)
    End Function

    Private Function 式判断(句 As String) As String
        If 句.EndsWith("()") Then
            Return "执行式" '默认将句末()前作为函数节点路径
        End If
        If Regex.IsMatch(句, ".{1,}=.{1,}") Then
            Return "赋值式"
        End If
        If Regex.IsMatch(句, "^若 [^ ]{1,} [^ ]{1,}$") Or Regex.IsMatch(句, "^若 [^ ]{1,}$") Then
            Return "判断式"
        End If
        If Regex.IsMatch(句, "^遍 [^ ]{1,}$") Then
            Return "循环式"
        End If
        Return ""
    End Function
End Class
