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
        Me.候选表 = New System.Windows.Forms.ListBox()
        Me.SuspendLayout()
        '
        '候选表
        '
        Me.候选表.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.候选表.Dock = System.Windows.Forms.DockStyle.Fill
        Me.候选表.Font = New System.Drawing.Font("Microsoft YaHei UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.候选表.FormattingEnabled = True
        Me.候选表.IntegralHeight = False
        Me.候选表.ItemHeight = 21
        Me.候选表.Location = New System.Drawing.Point(0, 0)
        Me.候选表.Name = "候选表"
        Me.候选表.Size = New System.Drawing.Size(306, 230)
        Me.候选表.TabIndex = 0
        '
        'Alternative
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(306, 230)
        Me.Controls.Add(Me.候选表)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "Alternative"
        Me.Opacity = 0.8R
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Alternative"
        Me.TopMost = True
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents 候选表 As ListBox
End Class
