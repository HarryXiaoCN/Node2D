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
        Me.SuspendLayout()
        '
        '节点名
        '
        Me.节点名.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.节点名.Font = New System.Drawing.Font("微软雅黑", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.节点名.Location = New System.Drawing.Point(12, 43)
        Me.节点名.Name = "节点名"
        Me.节点名.Size = New System.Drawing.Size(132, 29)
        Me.节点名.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(74, 21)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "节点名："
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(156, 19)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(58, 21)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "类型："
        '
        '节点类型
        '
        Me.节点类型.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.节点类型.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.节点类型.FormattingEnabled = True
        Me.节点类型.Items.AddRange(New Object() {"值", "引用", "函数"})
        Me.节点类型.Location = New System.Drawing.Point(156, 43)
        Me.节点类型.Name = "节点类型"
        Me.节点类型.Size = New System.Drawing.Size(132, 29)
        Me.节点类型.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 75)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(58, 21)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "内容："
        '
        '节点内容
        '
        Me.节点内容.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.节点内容.Location = New System.Drawing.Point(12, 99)
        Me.节点内容.Multiline = True
        Me.节点内容.Name = "节点内容"
        Me.节点内容.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.节点内容.Size = New System.Drawing.Size(276, 147)
        Me.节点内容.TabIndex = 5
        '
        'Node
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(10.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Gainsboro
        Me.ClientSize = New System.Drawing.Size(301, 260)
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
    Friend WithEvents 节点内容 As TextBox
End Class
