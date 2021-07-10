Public Class 绘制类
    Public 类型 As String
    Public 起点 As Point
    Public 终点 As Point
    Public 内容 As String
    Public 颜色 As Color
    Public 画笔 As Pen
    Public 刷子 As Brush
    Public 区域 As Rectangle
    Public 路径() As Point
    Public 字体 As Font

    Public Sub 绘制(ByRef g As Graphics)
        Select Case 类型
            Case "DL"
                g.DrawLine(画笔, 起点, 终点)
            Case "DS"
                g.DrawString(内容, 字体, 刷子, 起点)
            Case "FR"
                g.FillRectangle(刷子, 区域)
            Case "DR"
                g.DrawRectangle(画笔, 区域)
            Case "FE"
                g.FillEllipse(刷子, 区域)
            Case "DE"
                g.DrawEllipse(画笔, 区域)
            Case "FP"
                g.FillPolygon(刷子, 路径)
            Case "DP"
                g.DrawPolygon(画笔, 路径)
        End Select
    End Sub
    Public Sub New(p As Pen, x As Integer, y As Integer, x2 As Integer, y2 As Integer)
        类型 = "DL"
        画笔 = p
        起点 = New Point(x, y)
        终点 = New Point(x2, y2)
    End Sub
    Public Sub New(s As String, f As Font, b As Brush, x As Integer, y As Integer)
        类型 = "DS"
        内容 = s
        字体 = f
        刷子 = b
        起点 = New Point(x, y)
    End Sub
    Public Sub New(sz As Brush, r As Rectangle, Optional type As String = "FR")
        类型 = type
        刷子 = sz
        区域 = r
    End Sub
    Public Sub New(hb As Pen, r As Rectangle, Optional type As String = "DR")
        类型 = type
        画笔 = hb
        区域 = r
    End Sub
    Public Sub New(sz As Brush, lj() As Point)
        类型 = "FP"
        刷子 = sz
        路径 = lj
    End Sub
    Public Sub New(hb As Pen, lj() As Point)
        类型 = "DP"
        画笔 = hb
        路径 = lj
    End Sub
End Class
