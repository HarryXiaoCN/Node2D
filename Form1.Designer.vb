﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
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
        Me.绘制空间 = New System.Windows.Forms.PictureBox()
        Me.菜单栏 = New System.Windows.Forms.MenuStrip()
        Me.文件菜单 = New System.Windows.Forms.ToolStripMenuItem()
        Me.载入平面 = New System.Windows.Forms.ToolStripMenuItem()
        Me.保存平面 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.文件退出菜单 = New System.Windows.Forms.ToolStripMenuItem()
        Me.编辑菜单 = New System.Windows.Forms.ToolStripMenuItem()
        Me.编辑启用菜单 = New System.Windows.Forms.ToolStripMenuItem()
        Me.编辑右键点击创建值节点 = New System.Windows.Forms.ToolStripMenuItem()
        Me.编辑右键点击创建引用点 = New System.Windows.Forms.ToolStripMenuItem()
        Me.编辑右键点击创建函数点 = New System.Windows.Forms.ToolStripMenuItem()
        Me.运行菜单 = New System.Windows.Forms.ToolStripMenuItem()
        Me.运行菜单执行当前节点 = New System.Windows.Forms.ToolStripMenuItem()
        Me.SFD = New System.Windows.Forms.SaveFileDialog()
        Me.OFD = New System.Windows.Forms.OpenFileDialog()
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
        Me.菜单栏.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.文件菜单, Me.编辑菜单, Me.运行菜单})
        Me.菜单栏.Location = New System.Drawing.Point(0, 0)
        Me.菜单栏.Name = "菜单栏"
        Me.菜单栏.Size = New System.Drawing.Size(784, 25)
        Me.菜单栏.TabIndex = 1
        Me.菜单栏.Text = "菜单栏"
        '
        '文件菜单
        '
        Me.文件菜单.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.载入平面, Me.保存平面, Me.ToolStripSeparator1, Me.文件退出菜单})
        Me.文件菜单.Name = "文件菜单"
        Me.文件菜单.Size = New System.Drawing.Size(58, 21)
        Me.文件菜单.Text = "文件(&F)"
        '
        '载入平面
        '
        Me.载入平面.Name = "载入平面"
        Me.载入平面.Size = New System.Drawing.Size(118, 22)
        Me.载入平面.Text = "载入(&L)"
        '
        '保存平面
        '
        Me.保存平面.Name = "保存平面"
        Me.保存平面.Size = New System.Drawing.Size(118, 22)
        Me.保存平面.Text = "保存(&S)"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(115, 6)
        '
        '文件退出菜单
        '
        Me.文件退出菜单.Name = "文件退出菜单"
        Me.文件退出菜单.Size = New System.Drawing.Size(118, 22)
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
        Me.编辑启用菜单.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.编辑右键点击创建值节点, Me.编辑右键点击创建引用点, Me.编辑右键点击创建函数点})
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
        '运行菜单
        '
        Me.运行菜单.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.运行菜单执行当前节点})
        Me.运行菜单.Name = "运行菜单"
        Me.运行菜单.Size = New System.Drawing.Size(60, 21)
        Me.运行菜单.Text = "运行(&R)"
        '
        '运行菜单执行当前节点
        '
        Me.运行菜单执行当前节点.Name = "运行菜单执行当前节点"
        Me.运行菜单执行当前节点.ShortcutKeys = System.Windows.Forms.Keys.F5
        Me.运行菜单执行当前节点.Size = New System.Drawing.Size(169, 22)
        Me.运行菜单执行当前节点.Text = "执行选中节点"
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
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(784, 561)
        Me.Controls.Add(Me.绘制空间)
        Me.Controls.Add(Me.菜单栏)
        Me.DoubleBuffered = True
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
End Class
