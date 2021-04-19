Public Class 节点平面类
    Public Class 节点类
        Private name As String
        Private type As String
        Private content As String
        Private postion As Point
        Public 父域 As 节点平面类
        Public 连接 As List(Of String)
        Public Event 改变前(node As 节点类)
        Public Event 改变后(node As 节点类)
        Public Sub New(ByRef parent As 节点平面类)
            父域 = parent
        End Sub
        Public Property 位置() As Point
            Get
                Return postion
            End Get
            Set(value As Point)
                postion = value
            End Set
        End Property
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
    Public 引用域 As List(Of 节点平面类)
    Public 本域节点 As Dictionary(Of String, 节点类)
    Public 空间限制 As List(Of Point)

    Public Sub New()

    End Sub
End Class
