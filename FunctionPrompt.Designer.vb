<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FunctionPrompt
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FunctionPrompt))
        Me.法则域 = New System.Windows.Forms.PictureBox()
        CType(Me.法则域, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        '法则域
        '
        Me.法则域.BackColor = System.Drawing.Color.Transparent
        Me.法则域.Dock = System.Windows.Forms.DockStyle.Fill
        Me.法则域.Location = New System.Drawing.Point(0, 0)
        Me.法则域.Margin = New System.Windows.Forms.Padding(4)
        Me.法则域.Name = "法则域"
        Me.法则域.Size = New System.Drawing.Size(696, 85)
        Me.法则域.TabIndex = 0
        Me.法则域.TabStop = False
        '
        'FunctionPrompt
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(696, 85)
        Me.Controls.Add(Me.法则域)
        Me.Font = New System.Drawing.Font("微软雅黑 Light", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "FunctionPrompt"
        Me.Opacity = 0.9R
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "FunctionPrompt"
        Me.TopMost = True
        CType(Me.法则域, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents 法则域 As PictureBox
End Class
