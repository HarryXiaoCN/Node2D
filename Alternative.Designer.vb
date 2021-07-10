<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Alternative
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。  
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Alternative))
        Me.类型图标表 = New System.Windows.Forms.ImageList(Me.components)
        Me.候选域 = New System.Windows.Forms.PictureBox()
        CType(Me.候选域, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        '类型图标表
        '
        Me.类型图标表.ColorDepth = System.Windows.Forms.ColorDepth.Depth16Bit
        Me.类型图标表.ImageStream = CType(resources.GetObject("类型图标表.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.类型图标表.TransparentColor = System.Drawing.Color.Transparent
        Me.类型图标表.Images.SetKeyName(0, "法_24.png")
        Me.类型图标表.Images.SetKeyName(1, "函_24.png")
        Me.类型图标表.Images.SetKeyName(2, "接_24.png")
        Me.类型图标表.Images.SetKeyName(3, "面_24.png")
        Me.类型图标表.Images.SetKeyName(4, "引_24.png")
        Me.类型图标表.Images.SetKeyName(5, "值_24.png")
        Me.类型图标表.Images.SetKeyName(6, "未知_24.png")
        '
        '候选域
        '
        Me.候选域.BackColor = System.Drawing.Color.White
        Me.候选域.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.候选域.Dock = System.Windows.Forms.DockStyle.Fill
        Me.候选域.Location = New System.Drawing.Point(0, 0)
        Me.候选域.Margin = New System.Windows.Forms.Padding(4)
        Me.候选域.Name = "候选域"
        Me.候选域.Size = New System.Drawing.Size(306, 230)
        Me.候选域.TabIndex = 0
        Me.候选域.TabStop = False
        '
        'Alternative
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(306, 230)
        Me.Controls.Add(Me.候选域)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("微软雅黑 Light", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "Alternative"
        Me.Opacity = 0.8R
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Alternative"
        Me.TopMost = True
        CType(Me.候选域, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents 类型图标表 As ImageList
    Friend WithEvents 候选域 As PictureBox
End Class
