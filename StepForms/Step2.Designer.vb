<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Step2
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Step2))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.DeclineRadioButton = New InstallerSetup.Controls.KnightTheme.KnightRadioButton()
        Me.AcceptRadioButton = New InstallerSetup.Controls.KnightTheme.KnightRadioButton()
        Me.RichTextBox1 = New System.Windows.Forms.RichTextBox()
        Me.LogInLabel3 = New InstallerSetup.Controls.LogInLabelTheme.LogInLabel()
        Me.LogInLabel2 = New InstallerSetup.Controls.LogInLabelTheme.LogInLabel()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.DeclineRadioButton)
        Me.Panel1.Controls.Add(Me.AcceptRadioButton)
        Me.Panel1.Controls.Add(Me.RichTextBox1)
        Me.Panel1.Controls.Add(Me.LogInLabel3)
        Me.Panel1.Controls.Add(Me.LogInLabel2)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(600, 327)
        Me.Panel1.TabIndex = 2
        '
        'DeclineRadioButton
        '
        Me.DeclineRadioButton.Checked = False
        Me.DeclineRadioButton.Location = New System.Drawing.Point(30, 296)
        Me.DeclineRadioButton.Name = "DeclineRadioButton"
        Me.DeclineRadioButton.Size = New System.Drawing.Size(294, 16)
        Me.DeclineRadioButton.TabIndex = 8
        Me.DeclineRadioButton.Text = "I do Not agree with the license terms."
        '
        'AcceptRadioButton
        '
        Me.AcceptRadioButton.Checked = False
        Me.AcceptRadioButton.Location = New System.Drawing.Point(30, 275)
        Me.AcceptRadioButton.Name = "AcceptRadioButton"
        Me.AcceptRadioButton.Size = New System.Drawing.Size(294, 16)
        Me.AcceptRadioButton.TabIndex = 7
        Me.AcceptRadioButton.Text = "I Accept the terms of the license agreement."
        '
        'RichTextBox1
        '
        Me.RichTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.RichTextBox1.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RichTextBox1.Location = New System.Drawing.Point(12, 91)
        Me.RichTextBox1.Name = "RichTextBox1"
        Me.RichTextBox1.Size = New System.Drawing.Size(576, 178)
        Me.RichTextBox1.TabIndex = 4
        Me.RichTextBox1.Text = resources.GetString("RichTextBox1.Text")
        '
        'LogInLabel3
        '
        Me.LogInLabel3.BackColor = System.Drawing.Color.Transparent
        Me.LogInLabel3.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LogInLabel3.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LogInLabel3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.LogInLabel3.Location = New System.Drawing.Point(27, 65)
        Me.LogInLabel3.Name = "LogInLabel3"
        Me.LogInLabel3.Size = New System.Drawing.Size(316, 23)
        Me.LogInLabel3.TabIndex = 3
        Me.LogInLabel3.Text = "Please Read the License Agreement"
        '
        'LogInLabel2
        '
        Me.LogInLabel2.BackColor = System.Drawing.Color.Transparent
        Me.LogInLabel2.Font = New System.Drawing.Font("Segoe UI Semibold", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LogInLabel2.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LogInLabel2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LogInLabel2.Location = New System.Drawing.Point(25, 27)
        Me.LogInLabel2.Name = "LogInLabel2"
        Me.LogInLabel2.Size = New System.Drawing.Size(316, 28)
        Me.LogInLabel2.TabIndex = 2
        Me.LogInLabel2.Text = "License Agreement"
        '
        'Step2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(39, Byte), Integer), CType(CType(45, Byte), Integer), CType(CType(59, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(600, 327)
        Me.Controls.Add(Me.Panel1)
        Me.ForeColor = System.Drawing.Color.White
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "Step2"
        Me.Text = "Step2"
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents LogInLabel3 As InstallerSetup.Controls.LogInLabelTheme.LogInLabel
    Friend WithEvents LogInLabel2 As InstallerSetup.Controls.LogInLabelTheme.LogInLabel
    Friend WithEvents RichTextBox1 As System.Windows.Forms.RichTextBox
    Friend WithEvents DeclineRadioButton As InstallerSetup.Controls.KnightTheme.KnightRadioButton
    Friend WithEvents AcceptRadioButton As InstallerSetup.Controls.KnightTheme.KnightRadioButton
End Class
