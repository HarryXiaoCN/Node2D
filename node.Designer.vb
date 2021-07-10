<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Node
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
        Me.节点名 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.节点类型 = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.节点内容 = New System.Windows.Forms.TextBox()
        Me.大小控制_中 = New System.Windows.Forms.Label()
        Me.大小控制_大 = New System.Windows.Forms.Label()
        Me.大小控制_小 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        '节点名
        '
        Me.节点名.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.节点名.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.节点名.Font = New System.Drawing.Font("微软雅黑", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.节点名.Location = New System.Drawing.Point(12, 34)
        Me.节点名.Name = "节点名"
        Me.节点名.Size = New System.Drawing.Size(132, 29)
        Me.节点名.TabIndex = 0
        Me.节点名.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(74, 21)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "节点名："
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(162, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(58, 21)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "类型："
        '
        '节点类型
        '
        Me.节点类型.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.节点类型.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.节点类型.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.节点类型.FormattingEnabled = True
        Me.节点类型.Items.AddRange(New Object() {"值", "引用", "函数", "接口"})
        Me.节点类型.Location = New System.Drawing.Point(162, 33)
        Me.节点类型.Name = "节点类型"
        Me.节点类型.Size = New System.Drawing.Size(132, 29)
        Me.节点类型.TabIndex = 3
        Me.节点类型.TabStop = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 66)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(58, 21)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "内容："
        '
        '节点内容
        '
        Me.节点内容.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.节点内容.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.节点内容.Location = New System.Drawing.Point(12, 90)
        Me.节点内容.Multiline = True
        Me.节点内容.Name = "节点内容"
        Me.节点内容.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.节点内容.Size = New System.Drawing.Size(282, 128)
        Me.节点内容.TabIndex = 5
        Me.节点内容.TabStop = False
        '
        '大小控制_中
        '
        Me.大小控制_中.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.大小控制_中.BackColor = System.Drawing.Color.DarkGray
        Me.大小控制_中.Font = New System.Drawing.Font("楷体", 10.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.大小控制_中.ForeColor = System.Drawing.Color.White
        Me.大小控制_中.Location = New System.Drawing.Point(252, 9)
        Me.大小控制_中.Margin = New System.Windows.Forms.Padding(1, 0, 1, 0)
        Me.大小控制_中.Name = "大小控制_中"
        Me.大小控制_中.Size = New System.Drawing.Size(21, 14)
        Me.大小控制_中.TabIndex = 6
        Me.大小控制_中.Text = "中"
        '
        '大小控制_大
        '
        Me.大小控制_大.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.大小控制_大.BackColor = System.Drawing.Color.DarkGray
        Me.大小控制_大.Font = New System.Drawing.Font("楷体", 10.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.大小控制_大.ForeColor = System.Drawing.Color.White
        Me.大小控制_大.Location = New System.Drawing.Point(275, 9)
        Me.大小控制_大.Margin = New System.Windows.Forms.Padding(1, 0, 1, 0)
        Me.大小控制_大.Name = "大小控制_大"
        Me.大小控制_大.Size = New System.Drawing.Size(21, 14)
        Me.大小控制_大.TabIndex = 7
        Me.大小控制_大.Text = "大"
        '
        '大小控制_小
        '
        Me.大小控制_小.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.大小控制_小.BackColor = System.Drawing.Color.DarkGray
        Me.大小控制_小.Font = New System.Drawing.Font("楷体", 10.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.大小控制_小.ForeColor = System.Drawing.Color.MediumSpringGreen
        Me.大小控制_小.Location = New System.Drawing.Point(229, 9)
        Me.大小控制_小.Margin = New System.Windows.Forms.Padding(1, 0, 1, 0)
        Me.大小控制_小.Name = "大小控制_小"
        Me.大小控制_小.Size = New System.Drawing.Size(21, 14)
        Me.大小控制_小.TabIndex = 8
        Me.大小控制_小.Text = "小"
        '
        'Node
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(10.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Gainsboro
        Me.ClientSize = New System.Drawing.Size(306, 230)
        Me.Controls.Add(Me.大小控制_小)
        Me.Controls.Add(Me.大小控制_大)
        Me.Controls.Add(Me.大小控制_中)
        Me.Controls.Add(Me.节点内容)
        Me.Controls.Add(Me.节点类型)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.节点名)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label3)
        Me.Font = New System.Drawing.Font("微软雅黑", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.Name = "Node"
        Me.Opacity = 0.9R
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "节点编辑"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents 节点名 As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents 节点类型 As ComboBox
    Friend WithEvents Label3 As Label
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents 大小控制_中 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents 大小控制_小 As Label
    Friend WithEvents 大小控制_大 As Label
    Friend WithEvents 节点内容 As TextBox
End Class
