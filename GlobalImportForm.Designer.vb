<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GlobalImportForm
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
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.移除全局平面 = New System.Windows.Forms.Button()
        Me.添加全局平面 = New System.Windows.Forms.Button()
        Me.新全局平面路径 = New System.Windows.Forms.TextBox()
        Me.全局平面列表 = New System.Windows.Forms.ListBox()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.移除全局节点 = New System.Windows.Forms.Button()
        Me.添加全局节点 = New System.Windows.Forms.Button()
        Me.新全局节点路径 = New System.Windows.Forms.TextBox()
        Me.全局节点列表 = New System.Windows.Forms.ListBox()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.HotTrack = True
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(385, 561)
        Me.TabControl1.TabIndex = 1
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.移除全局平面)
        Me.TabPage1.Controls.Add(Me.添加全局平面)
        Me.TabPage1.Controls.Add(Me.新全局平面路径)
        Me.TabPage1.Controls.Add(Me.全局平面列表)
        Me.TabPage1.Location = New System.Drawing.Point(4, 26)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(377, 531)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "平面"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        '移除全局平面
        '
        Me.移除全局平面.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.移除全局平面.Location = New System.Drawing.Point(318, 492)
        Me.移除全局平面.Name = "移除全局平面"
        Me.移除全局平面.Size = New System.Drawing.Size(55, 31)
        Me.移除全局平面.TabIndex = 6
        Me.移除全局平面.Text = "移除"
        Me.移除全局平面.UseVisualStyleBackColor = True
        '
        '添加全局平面
        '
        Me.添加全局平面.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.添加全局平面.Location = New System.Drawing.Point(257, 492)
        Me.添加全局平面.Name = "添加全局平面"
        Me.添加全局平面.Size = New System.Drawing.Size(55, 31)
        Me.添加全局平面.TabIndex = 5
        Me.添加全局平面.Text = "添加"
        Me.添加全局平面.UseVisualStyleBackColor = True
        '
        '新全局平面路径
        '
        Me.新全局平面路径.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.新全局平面路径.Font = New System.Drawing.Font("Microsoft YaHei UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.新全局平面路径.Location = New System.Drawing.Point(3, 493)
        Me.新全局平面路径.Name = "新全局平面路径"
        Me.新全局平面路径.Size = New System.Drawing.Size(248, 28)
        Me.新全局平面路径.TabIndex = 4
        '
        '全局平面列表
        '
        Me.全局平面列表.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.全局平面列表.FormattingEnabled = True
        Me.全局平面列表.IntegralHeight = False
        Me.全局平面列表.ItemHeight = 17
        Me.全局平面列表.Location = New System.Drawing.Point(3, 0)
        Me.全局平面列表.Name = "全局平面列表"
        Me.全局平面列表.Size = New System.Drawing.Size(370, 486)
        Me.全局平面列表.TabIndex = 3
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.移除全局节点)
        Me.TabPage2.Controls.Add(Me.添加全局节点)
        Me.TabPage2.Controls.Add(Me.新全局节点路径)
        Me.TabPage2.Controls.Add(Me.全局节点列表)
        Me.TabPage2.Location = New System.Drawing.Point(4, 26)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(377, 531)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "节点"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        '移除全局节点
        '
        Me.移除全局节点.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.移除全局节点.Location = New System.Drawing.Point(318, 492)
        Me.移除全局节点.Name = "移除全局节点"
        Me.移除全局节点.Size = New System.Drawing.Size(55, 31)
        Me.移除全局节点.TabIndex = 9
        Me.移除全局节点.Text = "移除"
        Me.移除全局节点.UseVisualStyleBackColor = True
        '
        '添加全局节点
        '
        Me.添加全局节点.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.添加全局节点.Location = New System.Drawing.Point(257, 492)
        Me.添加全局节点.Name = "添加全局节点"
        Me.添加全局节点.Size = New System.Drawing.Size(55, 31)
        Me.添加全局节点.TabIndex = 8
        Me.添加全局节点.Text = "添加"
        Me.添加全局节点.UseVisualStyleBackColor = True
        '
        '新全局节点路径
        '
        Me.新全局节点路径.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.新全局节点路径.Font = New System.Drawing.Font("Microsoft YaHei UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.新全局节点路径.Location = New System.Drawing.Point(3, 493)
        Me.新全局节点路径.Name = "新全局节点路径"
        Me.新全局节点路径.Size = New System.Drawing.Size(248, 28)
        Me.新全局节点路径.TabIndex = 7
        '
        '全局节点列表
        '
        Me.全局节点列表.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.全局节点列表.FormattingEnabled = True
        Me.全局节点列表.IntegralHeight = False
        Me.全局节点列表.ItemHeight = 17
        Me.全局节点列表.Location = New System.Drawing.Point(3, 0)
        Me.全局节点列表.Name = "全局节点列表"
        Me.全局节点列表.Size = New System.Drawing.Size(370, 486)
        Me.全局节点列表.TabIndex = 2
        '
        'GlobalImportForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(384, 561)
        Me.Controls.Add(Me.TabControl1)
        Me.Name = "GlobalImportForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "全局引用管理"
        Me.TopMost = True
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents 全局节点列表 As ListBox
    Friend WithEvents 移除全局平面 As Button
    Friend WithEvents 添加全局平面 As Button
    Friend WithEvents 新全局平面路径 As TextBox
    Friend WithEvents 全局平面列表 As ListBox
    Friend WithEvents 移除全局节点 As Button
    Friend WithEvents 添加全局节点 As Button
    Friend WithEvents 新全局节点路径 As TextBox
End Class
