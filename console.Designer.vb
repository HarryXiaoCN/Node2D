<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NodeConsole
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
        Me.控制输出 = New System.Windows.Forms.TextBox()
        Me.右键菜单 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.复制文本 = New System.Windows.Forms.ToolStripMenuItem()
        Me.全选文本 = New System.Windows.Forms.ToolStripMenuItem()
        Me.清空文本 = New System.Windows.Forms.ToolStripMenuItem()
        Me.右键菜单.SuspendLayout()
        Me.SuspendLayout()
        '
        '控制输出
        '
        Me.控制输出.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.控制输出.ContextMenuStrip = Me.右键菜单
        Me.控制输出.Dock = System.Windows.Forms.DockStyle.Fill
        Me.控制输出.Location = New System.Drawing.Point(0, 0)
        Me.控制输出.Multiline = True
        Me.控制输出.Name = "控制输出"
        Me.控制输出.ReadOnly = True
        Me.控制输出.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.控制输出.Size = New System.Drawing.Size(784, 161)
        Me.控制输出.TabIndex = 0
        '
        '右键菜单
        '
        Me.右键菜单.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.复制文本, Me.全选文本, Me.清空文本})
        Me.右键菜单.Name = "右键菜单"
        Me.右键菜单.Size = New System.Drawing.Size(146, 70)
        '
        '复制文本
        '
        Me.复制文本.Name = "复制文本"
        Me.复制文本.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.复制文本.Size = New System.Drawing.Size(145, 22)
        Me.复制文本.Text = "复制"
        '
        '全选文本
        '
        Me.全选文本.Name = "全选文本"
        Me.全选文本.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.A), System.Windows.Forms.Keys)
        Me.全选文本.Size = New System.Drawing.Size(145, 22)
        Me.全选文本.Text = "全选"
        '
        '清空文本
        '
        Me.清空文本.Name = "清空文本"
        Me.清空文本.ShortcutKeys = System.Windows.Forms.Keys.Delete
        Me.清空文本.Size = New System.Drawing.Size(145, 22)
        Me.清空文本.Text = "清空"
        '
        'NodeConsole
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 161)
        Me.Controls.Add(Me.控制输出)
        Me.Name = "NodeConsole"
        Me.Text = "控制台"
        Me.右键菜单.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents 控制输出 As TextBox
    Friend WithEvents 右键菜单 As ContextMenuStrip
    Friend WithEvents 复制文本 As ToolStripMenuItem
    Friend WithEvents 全选文本 As ToolStripMenuItem
    Friend WithEvents 清空文本 As ToolStripMenuItem
End Class
