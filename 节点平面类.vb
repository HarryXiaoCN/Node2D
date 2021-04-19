Imports System.Threading
Public Class 节点平面类
    Public Class 节点类
        Private name As String
        Private type As String
        Private content As String
        Public 位置 As Point
        Public 父域 As 节点平面类
        Public 连接 As List(Of String)
        Public Event 改变前(node As 节点类)
        Public Event 改变后(node As 节点类)
        Public Sub New(ByRef parent As 节点平面类, nodeName As String, nodeType As String, nodeContent As String, nodePos As Point)
            父域 = parent
            name = nodeName
            type = nodeType
            content = nodeContent
            位置 = nodePos
        End Sub
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
        Public Property 内容() As String
            Get
                Return content
            End Get
            Set(value As String)
                RaiseEvent 改变前(Me)
                content = value
                RaiseEvent 改变后(Me)
            End Set
        End Property
    End Class
    Public 引用域 As New List(Of 节点平面类)
    Public 本域节点 As New Dictionary(Of String, 节点类)
    Public 空间限制 As New List(Of Point)
    Public 主平面 As Boolean
    Public 结束标识 As Boolean
    Public 绘制间隔 As Long = 100
    Public 缩放倍数 As Integer = 50
    Public 主窗体 As Form
    Public WithEvents 绘制空间 As PictureBox

    Private 绘制线程 As Thread
    Private Delegate Sub 绘制更新委托(ByRef 帧 As Image)
    Public Sub New(ByRef mainForm As Form, ByRef drawSpace As PictureBox, Optional mainNode2D As Boolean = True)
        主窗体 = mainForm
        绘制空间 = drawSpace
        主平面 = mainNode2D
        If 主平面 Then
            绘制线程 = New Thread(AddressOf 绘制)
            绘制线程.Start()
        End If
    End Sub

    Public Sub 鼠标点击事件(sender As Object, e As MouseEventArgs) Handles 绘制空间.MouseDown
        If e.Button = MouseButtons.Right Then
            Dim dP As New Point(e.Location.X \ 缩放倍数, e.Location.Y \ 缩放倍数)
            新建节点(本域节点.Count, dP)
        End If
    End Sub

    Public Sub 新建节点(节点名 As String, 位置 As Point)
        If Not 本域节点.ContainsKey(节点名) And Not 空间限制.Contains(位置) Then
            本域节点.Add(节点名, New 节点类(Me, 节点名, "值", "", 位置))
            空间限制.Add(位置)
        End If
    End Sub

    Private Sub 绘制()
        Dim 缓存帧 As New Bitmap(10, 10)
        Do Until 结束标识
            缓存帧.Dispose()
            缓存帧 = New Bitmap(绘制空间.Width, 绘制空间.Height)
            Dim g As Graphics = Graphics.FromImage(缓存帧)

            For i As Long = 0 To 本域节点.Count - 1
                绘制节点(g, 本域节点.Values(i))
            Next

            g.Dispose()
            绘制更新(缓存帧)
            Thread.Sleep(绘制间隔)
        Loop
    End Sub

    Private Sub 绘制更新(ByRef 帧 As Image)
        Dim d As New 绘制更新委托(AddressOf 绘制更新操作)
        主窗体.Invoke(d, 帧)
    End Sub

    Private Sub 绘制更新操作(ByRef 帧 As Image)
        绘制空间.Image = 帧
    End Sub

    Private Sub 绘制节点(ByRef g As Graphics, node As 节点类)
        Dim r As New Rectangle(node.位置.X * 缩放倍数, node.位置.Y * 缩放倍数, 缩放倍数, 缩放倍数)
        Select Case node.类型
            Case "值"
                g.FillRectangle(Brushes.Yellow, r)
                g.DrawRectangle(Pens.Black, r)
            Case "引用"
                g.FillPolygon(Brushes.Yellow, 获得三角形点集(node.位置))
                g.DrawPolygon(Pens.Black, 获得三角形点集(node.位置))
            Case "函数"
                g.FillEllipse(Brushes.Green, r)
                g.DrawEllipse(Pens.Black, r)
        End Select
        g.DrawString(String.Format("{0}:{1}", node.名字, node.内容), 主窗体.Font, Brushes.Black, node.位置.X * 缩放倍数, node.位置.Y * 缩放倍数)
    End Sub
    Private Function 获得三角形点集(左上基点 As Point) As Point()
        Dim p As New List(Of Point)
        p.Add(New Point(左上基点.X + 缩放倍数 * 0.5, 左上基点.Y))
        p.Add(New Point(左上基点.X, 左上基点.Y + 缩放倍数))
        p.Add(New Point(左上基点.X + 缩放倍数, 左上基点.Y + 缩放倍数))
        Return p.ToArray
    End Function
End Class
