<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class import
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
        Me.域外导入列表 = New System.Windows.Forms.ListBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        '域外导入列表
        '
        Me.域外导入列表.FormattingEnabled = True
        Me.域外导入列表.ItemHeight = 17
        Me.域外导入列表.Location = New System.Drawing.Point(12, 12)
        Me.域外导入列表.Name = "域外导入列表"
        Me.域外导入列表.Size = New System.Drawing.Size(308, 208)
        Me.域外导入列表.TabIndex = 0
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(11, 227)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(145, 38)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "添加"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(173, 227)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(147, 38)
        Me.Button2.TabIndex = 2
        Me.Button2.Text = "删除"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'import
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(331, 273)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.域外导入列表)
        Me.Name = "import"
        Me.Text = "导入其它域"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents 域外导入列表 As ListBox
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
End Class
