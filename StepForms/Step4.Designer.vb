<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Step4
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
        Me.components = New System.ComponentModel.Container()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.XylosProgressBar1 = New InstallerSetup.Controls.XylosThemeMod.XylosProgressBar()
        Me.StatusLabel = New InstallerSetup.Controls.LogInLabelTheme.LogInLabel()
        Me.LogInLabel2 = New InstallerSetup.Controls.LogInLabelTheme.LogInLabel()
        Me.LogInLabel3 = New InstallerSetup.Controls.LogInLabelTheme.LogInLabel()
        Me.LogInLabel1 = New InstallerSetup.Controls.LogInLabelTheme.LogInLabel()
        Me.FolderBrowserDialog1 = New InstallerSetup.FileBorserDialog.FolderBrowserDialog()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.XylosProgressBar1)
        Me.Panel1.Controls.Add(Me.StatusLabel)
        Me.Panel1.Controls.Add(Me.LogInLabel2)
        Me.Panel1.Controls.Add(Me.LogInLabel3)
        Me.Panel1.Controls.Add(Me.LogInLabel1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(600, 327)
        Me.Panel1.TabIndex = 3
        '
        'XylosProgressBar1
        '
        Me.XylosProgressBar1.BackgroundColor = System.Drawing.Color.DodgerBlue
        Me.XylosProgressBar1.Location = New System.Drawing.Point(27, 196)
        Me.XylosProgressBar1.Maximum = 100
        Me.XylosProgressBar1.Minimum = 0
        Me.XylosProgressBar1.Name = "XylosProgressBar1"
        Me.XylosProgressBar1.Size = New System.Drawing.Size(544, 40)
        Me.XylosProgressBar1.Stripes = System.Drawing.Color.DodgerBlue
        Me.XylosProgressBar1.TabIndex = 9
        Me.XylosProgressBar1.Text = "XylosProgressBar1"
        Me.XylosProgressBar1.Value = 0
        '
        'StatusLabel
        '
        Me.StatusLabel.BackColor = System.Drawing.Color.Transparent
        Me.StatusLabel.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StatusLabel.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.StatusLabel.ForeColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.StatusLabel.Location = New System.Drawing.Point(24, 171)
        Me.StatusLabel.Name = "StatusLabel"
        Me.StatusLabel.Size = New System.Drawing.Size(547, 13)
        Me.StatusLabel.TabIndex = 8
        Me.StatusLabel.Text = "//."
        '
        'LogInLabel2
        '
        Me.LogInLabel2.AutoSize = True
        Me.LogInLabel2.BackColor = System.Drawing.Color.Transparent
        Me.LogInLabel2.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LogInLabel2.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LogInLabel2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.LogInLabel2.Location = New System.Drawing.Point(24, 129)
        Me.LogInLabel2.Name = "LogInLabel2"
        Me.LogInLabel2.Size = New System.Drawing.Size(99, 17)
        Me.LogInLabel2.TabIndex = 5
        Me.LogInLabel2.Text = "Installing files..."
        '
        'LogInLabel3
        '
        Me.LogInLabel3.BackColor = System.Drawing.Color.Transparent
        Me.LogInLabel3.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LogInLabel3.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LogInLabel3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.LogInLabel3.Location = New System.Drawing.Point(24, 73)
        Me.LogInLabel3.Name = "LogInLabel3"
        Me.LogInLabel3.Size = New System.Drawing.Size(547, 38)
        Me.LogInLabel3.TabIndex = 3
        Me.LogInLabel3.Text = "Wait while the wizard completes all the " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "operations to install Code Smart on you" & _
    "r computer."
        '
        'LogInLabel1
        '
        Me.LogInLabel1.AutoSize = True
        Me.LogInLabel1.BackColor = System.Drawing.Color.Transparent
        Me.LogInLabel1.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LogInLabel1.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LogInLabel1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LogInLabel1.Location = New System.Drawing.Point(21, 28)
        Me.LogInLabel1.Name = "LogInLabel1"
        Me.LogInLabel1.Size = New System.Drawing.Size(270, 32)
        Me.LogInLabel1.TabIndex = 0
        Me.LogInLabel1.Text = "Installation in Progress..."
        '
        'FolderBrowserDialog1
        '
        Me.FolderBrowserDialog1.DirectoryPath = Nothing
        '
        'Timer1
        '
        Me.Timer1.Interval = 1
        '
        'Step4
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(39, Byte), Integer), CType(CType(45, Byte), Integer), CType(CType(59, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(600, 327)
        Me.Controls.Add(Me.Panel1)
        Me.ForeColor = System.Drawing.Color.White
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "Step4"
        Me.Text = "Step4"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents LogInLabel2 As InstallerSetup.Controls.LogInLabelTheme.LogInLabel
    Friend WithEvents LogInLabel3 As InstallerSetup.Controls.LogInLabelTheme.LogInLabel
    Friend WithEvents LogInLabel1 As InstallerSetup.Controls.LogInLabelTheme.LogInLabel
    Friend WithEvents FolderBrowserDialog1 As InstallerSetup.FileBorserDialog.FolderBrowserDialog
    Friend WithEvents XylosProgressBar1 As InstallerSetup.Controls.XylosThemeMod.XylosProgressBar
    Friend WithEvents StatusLabel As InstallerSetup.Controls.LogInLabelTheme.LogInLabel
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
End Class
