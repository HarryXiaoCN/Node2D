Public Class FunctionPrompt
    Public 父域 As 节点平面类
    Private 标题字体 As Font
    Public Sub New(ByRef parent As 节点平面类)

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        父域 = parent
        字体设置(主界面.法则字体)
    End Sub
    Public Sub 字体设置(setFont As Font)
        Font = setFont
        标题字体 = New Font(Font, FontStyle.Bold)
    End Sub

    Private Function 获得展示大小(fStr As String) As Size
        Dim img As New Bitmap(10, 10)
        Dim g As Graphics = Graphics.FromImage(img)
        Dim s As SizeF = g.MeasureString(fStr.Substring(1), Font)
        g.Dispose()
        img.Dispose()
        Return New Size(Int(s.Width) + 10, Int(s.Height) + 10)
    End Function

    Public Sub 展示(f As String)
        Dim fStr As String = 获得法则解释(f)
        If fStr <> "" Then
            Dim s As Size = 获得展示大小(fStr)
            Dim img As New Bitmap(s.Width, s.Height)
            Width = s.Width
            Height = s.Height
            Dim g As Graphics = Graphics.FromImage(img)
            g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
            g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
            Dim fT() As String = Split(fStr, vbCrLf)
            For i As Integer = 0 To UBound(fT)
                If fT(i).StartsWith("·") Then
                    Dim titleStr As String = fT(i).Substring(1)
                    Dim titleTemp() As String = Split(titleStr, " ")
                    Dim tT As String = titleTemp(0)
                    Dim tT2 As String = titleStr.Substring(tT.Length + 1)
                    Dim titleSizeF As SizeF = g.MeasureString(tT, 标题字体)
                    g.DrawString(tT2, 标题字体, Brushes.Orange, 4 + titleSizeF.Width, i * FontHeight + 4)
                    g.DrawString(tT, 标题字体, Brushes.DeepSkyBlue, 4, i * FontHeight + 4)
                Else
                    g.DrawString(fT(i), Font, Brushes.Black, 4, i * FontHeight + 4)
                End If
            Next
            If 法则域.Image IsNot Nothing Then
                法则域.Image.Dispose()
            End If
            法则域.Image = img
            If 父域.节点编辑窗体.Left + 父域.节点编辑窗体.Width + Width <= Screen.PrimaryScreen.Bounds.Width Or Screen.PrimaryScreen.Bounds.Width - (父域.节点编辑窗体.Left + 父域.节点编辑窗体.Width) >= 父域.节点编辑窗体.Left Then
                Left = 父域.节点编辑窗体.Left + 父域.节点编辑窗体.Width
            Else
                Left = 父域.节点编辑窗体.Left - Width
            End If
            Top = 父域.节点编辑窗体.Top + (父域.节点编辑窗体.Height - Height) / 2
            Visible = True
            父域.节点编辑窗体.节点内容.Focus()
        ElseIf Visible Then
            Visible = False
        End If
    End Sub
    Protected Overrides ReadOnly Property ShowWithoutActivation As Boolean
        Get
            Return True
        End Get
    End Property
End Class