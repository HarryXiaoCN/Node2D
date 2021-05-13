<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.绘制空间 = New System.Windows.Forms.PictureBox()
        Me.菜单栏 = New System.Windows.Forms.MenuStrip()
        Me.文件菜单 = New System.Windows.Forms.ToolStripMenuItem()
        Me.新建平面 = New System.Windows.Forms.ToolStripMenuItem()
        Me.载入平面 = New System.Windows.Forms.ToolStripMenuItem()
        Me.保存平面 = New System.Windows.Forms.ToolStripMenuItem()
        Me.另存为平面 = New System.Windows.Forms.ToolStripMenuItem()
        Me.重载平面 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.文件退出菜单 = New System.Windows.Forms.ToolStripMenuItem()
        Me.编辑菜单 = New System.Windows.Forms.ToolStripMenuItem()
        Me.编辑启用菜单 = New System.Windows.Forms.ToolStripMenuItem()
        Me.编辑右键点击创建值节点 = New System.Windows.Forms.ToolStripMenuItem()
        Me.编辑右键点击创建引用点 = New System.Windows.Forms.ToolStripMenuItem()
        Me.编辑右键点击创建函数点 = New System.Windows.Forms.ToolStripMenuItem()
        Me.编辑右键点击创建接口点 = New System.Windows.Forms.ToolStripMenuItem()
        Me.运行菜单 = New System.Windows.Forms.ToolStripMenuItem()
        Me.运行菜单执行当前节点 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.控制台每次运行时清空 = New System.Windows.Forms.ToolStripMenuItem()
        Me.控制台输出时间戳 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.打开全局引用窗体 = New System.Windows.Forms.ToolStripMenuItem()
        Me.视图菜单 = New System.Windows.Forms.ToolStripMenuItem()
        Me.主窗体字体设置 = New System.Windows.Forms.ToolStripMenuItem()
        Me.主窗体文字居中 = New System.Windows.Forms.ToolStripMenuItem()
        Me.主窗体显示内容 = New System.Windows.Forms.ToolStripMenuItem()
        Me.默认节点颜色 = New System.Windows.Forms.ToolStripMenuItem()
        Me.值节点背景色 = New System.Windows.Forms.ToolStripMenuItem()
        Me.引用点背景色 = New System.Windows.Forms.ToolStripMenuItem()
        Me.函数点背景色 = New System.Windows.Forms.ToolStripMenuItem()
        Me.接口点背景色 = New System.Windows.Forms.ToolStripMenuItem()
        Me.帮助菜单 = New System.Windows.Forms.ToolStripMenuItem()
        Me.帮助主页 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.解除文件关联 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.程序版本 = New System.Windows.Forms.ToolStripMenuItem()
        Me.SFD = New System.Windows.Forms.SaveFileDialog()
        Me.OFD = New System.Windows.Forms.OpenFileDialog()
        Me.FontD = New System.Windows.Forms.FontDialog()
        Me.ColorD = New System.Windows.Forms.ColorDialog()
        CType(Me.绘制空间, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.菜单栏.SuspendLayout()
        Me.SuspendLayout()
        '
        '绘制空间
        '
        Me.绘制空间.BackColor = System.Drawing.Color.White
        Me.绘制空间.Dock = System.Windows.Forms.DockStyle.Fill
        Me.绘制空间.Location = New System.Drawing.Point(0, 25)
        Me.绘制空间.Name = "绘制空间"
        Me.绘制空间.Size = New System.Drawing.Size(784, 536)
        Me.绘制空间.TabIndex = 0
        Me.绘制空间.TabStop = False
        '
        '菜单栏
        '
        Me.菜单栏.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.文件菜单, Me.编辑菜单, Me.运行菜单, Me.视图菜单, Me.帮助菜单})
        Me.菜单栏.Location = New System.Drawing.Point(0, 0)
        Me.菜单栏.Name = "菜单栏"
        Me.菜单栏.Size = New System.Drawing.Size(784, 25)
        Me.菜单栏.TabIndex = 1
        Me.菜单栏.Text = "菜单栏"
        '
        '文件菜单
        '
        Me.文件菜单.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.新建平面, Me.载入平面, Me.保存平面, Me.另存为平面, Me.重载平面, Me.ToolStripSeparator1, Me.文件退出菜单})
        Me.文件菜单.Name = "文件菜单"
        Me.文件菜单.Size = New System.Drawing.Size(58, 21)
        Me.文件菜单.Text = "文件(&F)"
        '
        '新建平面
        '
        Me.新建平面.Name = "新建平面"
        Me.新建平面.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
        Me.新建平面.Size = New System.Drawing.Size(165, 22)
        Me.新建平面.Text = "新建(&N)"
        '
        '载入平面
        '
        Me.载入平面.Name = "载入平面"
        Me.载入平面.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.L), System.Windows.Forms.Keys)
        Me.载入平面.Size = New System.Drawing.Size(165, 22)
        Me.载入平面.Text = "载入(&L)"
        '
        '保存平面
        '
        Me.保存平面.Name = "保存平面"
        Me.保存平面.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.保存平面.Size = New System.Drawing.Size(165, 22)
        Me.保存平面.Text = "保存(&S)"
        '
        '另存为平面
        '
        Me.另存为平面.Name = "另存为平面"
        Me.另存为平面.Size = New System.Drawing.Size(165, 22)
        Me.另存为平面.Text = "另存为(&A)"
        '
        '重载平面
        '
        Me.重载平面.Name = "重载平面"
        Me.重载平面.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.R), System.Windows.Forms.Keys)
        Me.重载平面.Size = New System.Drawing.Size(165, 22)
        Me.重载平面.Text = "重载(&R)"
        Me.重载平面.ToolTipText = "重新加载当前平面"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(162, 6)
        '
        '文件退出菜单
        '
        Me.文件退出菜单.Name = "文件退出菜单"
        Me.文件退出菜单.Size = New System.Drawing.Size(165, 22)
        Me.文件退出菜单.Text = "退出(&Q)"
        '
        '编辑菜单
        '
        Me.编辑菜单.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.编辑启用菜单})
        Me.编辑菜单.Name = "编辑菜单"
        Me.编辑菜单.Size = New System.Drawing.Size(59, 21)
        Me.编辑菜单.Text = "编辑(&E)"
        '
        '编辑启用菜单
        '
        Me.编辑启用菜单.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.编辑右键点击创建值节点, Me.编辑右键点击创建引用点, Me.编辑右键点击创建函数点, Me.编辑右键点击创建接口点})
        Me.编辑启用菜单.Name = "编辑启用菜单"
        Me.编辑启用菜单.Size = New System.Drawing.Size(124, 22)
        Me.编辑启用菜单.Text = "右键点击"
        '
        '编辑右键点击创建值节点
        '
        Me.编辑右键点击创建值节点.Checked = True
        Me.编辑右键点击创建值节点.CheckState = System.Windows.Forms.CheckState.Checked
        Me.编辑右键点击创建值节点.Name = "编辑右键点击创建值节点"
        Me.编辑右键点击创建值节点.Size = New System.Drawing.Size(136, 22)
        Me.编辑右键点击创建值节点.Text = "创建值节点"
        '
        '编辑右键点击创建引用点
        '
        Me.编辑右键点击创建引用点.Name = "编辑右键点击创建引用点"
        Me.编辑右键点击创建引用点.Size = New System.Drawing.Size(136, 22)
        Me.编辑右键点击创建引用点.Text = "创建引用点"
        '
        '编辑右键点击创建函数点
        '
        Me.编辑右键点击创建函数点.Name = "编辑右键点击创建函数点"
        Me.编辑右键点击创建函数点.Size = New System.Drawing.Size(136, 22)
        Me.编辑右键点击创建函数点.Text = "创建函数点"
        '
        '编辑右键点击创建接口点
        '
        Me.编辑右键点击创建接口点.Name = "编辑右键点击创建接口点"
        Me.编辑右键点击创建接口点.Size = New System.Drawing.Size(136, 22)
        Me.编辑右键点击创建接口点.Text = "创建接口点"
        '
        '运行菜单
        '
        Me.运行菜单.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.运行菜单执行当前节点, Me.ToolStripSeparator2, Me.控制台每次运行时清空, Me.控制台输出时间戳, Me.ToolStripSeparator3, Me.打开全局引用窗体})
        Me.运行菜单.Name = "运行菜单"
        Me.运行菜单.Size = New System.Drawing.Size(60, 21)
        Me.运行菜单.Text = "运行(&R)"
        '
        '运行菜单执行当前节点
        '
        Me.运行菜单执行当前节点.Name = "运行菜单执行当前节点"
        Me.运行菜单执行当前节点.ShortcutKeys = System.Windows.Forms.Keys.F5
        Me.运行菜单执行当前节点.Size = New System.Drawing.Size(177, 22)
        Me.运行菜单执行当前节点.Text = "执行选中节点"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(174, 6)
        '
        '控制台每次运行时清空
        '
        Me.控制台每次运行时清空.Name = "控制台每次运行时清空"
        Me.控制台每次运行时清空.Size = New System.Drawing.Size(177, 22)
        Me.控制台每次运行时清空.Text = "控制台自动清空(&C)"
        Me.控制台每次运行时清空.ToolTipText = "每次函数节点执行时清空"
        '
        '控制台输出时间戳
        '
        Me.控制台输出时间戳.Name = "控制台输出时间戳"
        Me.控制台输出时间戳.Size = New System.Drawing.Size(177, 22)
        Me.控制台输出时间戳.Text = "控制台消息时间(&T)"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(174, 6)
        '
        '打开全局引用窗体
        '
        Me.打开全局引用窗体.Name = "打开全局引用窗体"
        Me.打开全局引用窗体.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.I), System.Windows.Forms.Keys)
        Me.打开全局引用窗体.Size = New System.Drawing.Size(177, 22)
        Me.打开全局引用窗体.Text = "全局引用(&I)"
        '
        '视图菜单
        '
        Me.视图菜单.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.主窗体字体设置, Me.主窗体文字居中, Me.主窗体显示内容, Me.默认节点颜色})
        Me.视图菜单.Name = "视图菜单"
        Me.视图菜单.Size = New System.Drawing.Size(60, 21)
        Me.视图菜单.Text = "视图(&V)"
        '
        '主窗体字体设置
        '
        Me.主窗体字体设置.Name = "主窗体字体设置"
        Me.主窗体字体设置.Size = New System.Drawing.Size(144, 22)
        Me.主窗体字体设置.Text = "设置字体(&F)"
        '
        '主窗体文字居中
        '
        Me.主窗体文字居中.Checked = True
        Me.主窗体文字居中.CheckState = System.Windows.Forms.CheckState.Checked
        Me.主窗体文字居中.Name = "主窗体文字居中"
        Me.主窗体文字居中.Size = New System.Drawing.Size(144, 22)
        Me.主窗体文字居中.Text = "文字居中(&M)"
        '
        '主窗体显示内容
        '
        Me.主窗体显示内容.Checked = True
        Me.主窗体显示内容.CheckState = System.Windows.Forms.CheckState.Checked
        Me.主窗体显示内容.Name = "主窗体显示内容"
        Me.主窗体显示内容.Size = New System.Drawing.Size(144, 22)
        Me.主窗体显示内容.Text = "完整内容(&A)"
        '
        '默认节点颜色
        '
        Me.默认节点颜色.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.值节点背景色, Me.引用点背景色, Me.函数点背景色, Me.接口点背景色})
        Me.默认节点颜色.Name = "默认节点颜色"
        Me.默认节点颜色.Size = New System.Drawing.Size(144, 22)
        Me.默认节点颜色.Text = "节点颜色(&C)"
        '
        '值节点背景色
        '
        Me.值节点背景色.BackColor = System.Drawing.Color.Gold
        Me.值节点背景色.Name = "值节点背景色"
        Me.值节点背景色.Size = New System.Drawing.Size(130, 22)
        Me.值节点背景色.Text = "值节点(&V)"
        Me.值节点背景色.ToolTipText = "值节点背景色"
        '
        '引用点背景色
        '
        Me.引用点背景色.BackColor = System.Drawing.Color.DeepSkyBlue
        Me.引用点背景色.Name = "引用点背景色"
        Me.引用点背景色.Size = New System.Drawing.Size(130, 22)
        Me.引用点背景色.Text = "引用点(&I)"
        Me.引用点背景色.ToolTipText = "引用点背景色"
        '
        '函数点背景色
        '
        Me.函数点背景色.BackColor = System.Drawing.Color.LimeGreen
        Me.函数点背景色.Name = "函数点背景色"
        Me.函数点背景色.Size = New System.Drawing.Size(130, 22)
        Me.函数点背景色.Text = "函数点(&F)"
        Me.函数点背景色.ToolTipText = "函数点背景色"
        '
        '接口点背景色
        '
        Me.接口点背景色.BackColor = System.Drawing.Color.PaleVioletRed
        Me.接口点背景色.Name = "接口点背景色"
        Me.接口点背景色.Size = New System.Drawing.Size(130, 22)
        Me.接口点背景色.Text = "接口点(&N)"
        Me.接口点背景色.ToolTipText = "接口点背景色"
        '
        '帮助菜单
        '
        Me.帮助菜单.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.帮助主页, Me.ToolStripSeparator5, Me.解除文件关联, Me.ToolStripSeparator4, Me.程序版本})
        Me.帮助菜单.Name = "帮助菜单"
        Me.帮助菜单.Size = New System.Drawing.Size(61, 21)
        Me.帮助菜单.Text = "帮助(&H)"
        '
        '帮助主页
        '
        Me.帮助主页.Name = "帮助主页"
        Me.帮助主页.Size = New System.Drawing.Size(164, 22)
        Me.帮助主页.Text = "主页(&I)"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(161, 6)
        '
        '解除文件关联
        '
        Me.解除文件关联.Name = "解除文件关联"
        Me.解除文件关联.Size = New System.Drawing.Size(164, 22)
        Me.解除文件关联.Text = "解除文件关联(&R)"
        Me.解除文件关联.ToolTipText = "恢复文件关联请将""程序设置.ini""文件中的""已注册""行删除后重启程序。"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(161, 6)
        '
        '程序版本
        '
        Me.程序版本.Enabled = False
        Me.程序版本.Name = "程序版本"
        Me.程序版本.Size = New System.Drawing.Size(164, 22)
        Me.程序版本.Text = "版本：未知"
        '
        'SFD
        '
        Me.SFD.FileName = "新的平面"
        Me.SFD.Filter = "节点平面|*.n2d"
        Me.SFD.Title = "保存节点平面"
        '
        'OFD
        '
        Me.OFD.Filter = "节点平面|*.n2d"
        Me.OFD.Title = "载入节点平面"
        '
        'Form1
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(784, 561)
        Me.Controls.Add(Me.绘制空间)
        Me.Controls.Add(Me.菜单栏)
        Me.DoubleBuffered = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MainMenuStrip = Me.菜单栏
        Me.MinimumSize = New System.Drawing.Size(400, 400)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "节点平面"
        CType(Me.绘制空间, System.ComponentModel.ISupportInitialize).EndInit()
        Me.菜单栏.ResumeLayout(False)
        Me.菜单栏.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents 绘制空间 As PictureBox
    Friend WithEvents 菜单栏 As MenuStrip
    Friend WithEvents 文件菜单 As ToolStripMenuItem
    Friend WithEvents 文件退出菜单 As ToolStripMenuItem
    Friend WithEvents 编辑菜单 As ToolStripMenuItem
    Friend WithEvents 编辑启用菜单 As ToolStripMenuItem
    Friend WithEvents 编辑右键点击创建值节点 As ToolStripMenuItem
    Friend WithEvents 编辑右键点击创建引用点 As ToolStripMenuItem
    Friend WithEvents 编辑右键点击创建函数点 As ToolStripMenuItem
    Friend WithEvents 运行菜单 As ToolStripMenuItem
    Friend WithEvents 运行菜单执行当前节点 As ToolStripMenuItem
    Friend WithEvents 载入平面 As ToolStripMenuItem
    Friend WithEvents 保存平面 As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents SFD As SaveFileDialog
    Friend WithEvents OFD As OpenFileDialog
    Friend WithEvents 另存为平面 As ToolStripMenuItem
    Friend WithEvents 重载平面 As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents 控制台每次运行时清空 As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents 打开全局引用窗体 As ToolStripMenuItem
    Friend WithEvents 新建平面 As ToolStripMenuItem
    Friend WithEvents 控制台输出时间戳 As ToolStripMenuItem
    Friend WithEvents 帮助菜单 As ToolStripMenuItem
    Friend WithEvents 帮助主页 As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
    Friend WithEvents 程序版本 As ToolStripMenuItem
    Friend WithEvents 编辑右键点击创建接口点 As ToolStripMenuItem
    Friend WithEvents 视图菜单 As ToolStripMenuItem
    Friend WithEvents 主窗体字体设置 As ToolStripMenuItem
    Friend WithEvents FontD As FontDialog
    Friend WithEvents 主窗体文字居中 As ToolStripMenuItem
    Friend WithEvents 主窗体显示内容 As ToolStripMenuItem
    Friend WithEvents 默认节点颜色 As ToolStripMenuItem
    Friend WithEvents 值节点背景色 As ToolStripMenuItem
    Friend WithEvents 引用点背景色 As ToolStripMenuItem
    Friend WithEvents 函数点背景色 As ToolStripMenuItem
    Friend WithEvents 接口点背景色 As ToolStripMenuItem
    Friend WithEvents ColorD As ColorDialog
    Friend WithEvents 解除文件关联 As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator5 As ToolStripSeparator
End Class
