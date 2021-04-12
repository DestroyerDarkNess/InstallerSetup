
Public Class Form1

    Private Step1Form As Step1 = New Step1 With {.TopLevel = False, .Visible = True}
    Private Step2Form As Step2 = New Step2 With {.TopLevel = False, .Visible = True}
    Private Step3Form As Step3 = New Step3 With {.TopLevel = False, .Visible = True}
    Private Step4Form As Step4 = New Step4 With {.TopLevel = False, .Visible = True}
    Private Step5Form As Step5 = New Step5 With {.TopLevel = False, .Visible = True}

    Private UnistallForm As Unistall = New Unistall With {.TopLevel = False, .Visible = True}
    Private UnistallFinalForm As UnistallFinal = New UnistallFinal With {.TopLevel = False, .Visible = True}
    Private UnistallByeForm As UninstallBye = New UninstallBye With {.TopLevel = False, .Visible = True}

    Private Update1Form As UpdateForm = New UpdateForm With {.TopLevel = False, .Visible = True}
    Private CurrentVer As String = "1.0.6.0"

    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Core.Helper.MainForm = Me
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        StartGuiAll()
    End Sub

    Public Sub StartGuiAll()
        ReturnButton.Enabled = False
        If Core.Helper.InstallerInstanse.Installed = True Then
            Dim FileVer As String = FileVersionInfo.GetVersionInfo(Core.Helper.InstallerInstanse.GetMainFile).FileVersion

            If (CurrentVer > FileVer) Then
                PanelContainer.Controls.Add(Update1Form)
                UpdateButton.Visible = True

            Else

                PanelContainer.Controls.Add(UnistallForm)

            End If

            Me.Text = "Code Smart Uninstaller"
            Me.ContinueButton.Text = "Unistall"
            Me.ContinueButton.Invalidate()

        Else
            PanelContainer.Controls.Add(Step1Form)
        End If

    End Sub


    Public Sub NextStep()
        Dim GetCurrentForm As Form = PanelContainer.Controls(0)
        PanelContainer.Controls.Clear()
        If TypeOf GetCurrentForm Is Step1 Then
            PanelContainer.Controls.Add(Step2Form)
            Step2Form.CheckBoxes()
            ReturnButton.Enabled = True
        ElseIf TypeOf GetCurrentForm Is Step2 Then
            PanelContainer.Controls.Add(Step3Form)
            ReturnButton.Enabled = True
        ElseIf TypeOf GetCurrentForm Is Step3 Then
            PanelContainer.Controls.Add(Step4Form)
            Step4Form.StartInstall()
        ElseIf TypeOf GetCurrentForm Is Step4 Then
            Me.CancelButtonEx.Text = "Close"
            Me.CancelButtonEx.Invalidate()
            PanelContainer.Controls.Add(Step5Form)
        ElseIf TypeOf GetCurrentForm Is Step5 Then

        ElseIf TypeOf GetCurrentForm Is UpdateForm Then
            PanelContainer.Controls.Add(UnistallFinalForm)
            UnistallFinalForm.StartUninstall()
        ElseIf TypeOf GetCurrentForm Is Unistall Then
            PanelContainer.Controls.Add(UnistallFinalForm)
            UnistallFinalForm.StartUninstall()
        ElseIf TypeOf GetCurrentForm Is UnistallFinal Then
            PanelContainer.Controls.Add(UnistallByeForm)
        End If
    End Sub

    Public Sub ReturnStep()
        Dim GetCurrentForm As Form = PanelContainer.Controls(0)
        PanelContainer.Controls.Clear()
        If TypeOf GetCurrentForm Is Step1 Then

        ElseIf TypeOf GetCurrentForm Is Step2 Then
            PanelContainer.Controls.Add(Step1Form)
            ReturnButton.Enabled = False
            ContinueButton.Enabled = True
        ElseIf TypeOf GetCurrentForm Is Step3 Then
            PanelContainer.Controls.Add(Step2Form)
            Step2Form.CheckBoxes()
        ElseIf TypeOf GetCurrentForm Is Step4 Then
            PanelContainer.Controls.Add(Step3Form)
        ElseIf TypeOf GetCurrentForm Is Step5 Then
            PanelContainer.Controls.Add(Step4Form)
        ElseIf TypeOf GetCurrentForm Is UnistallFinal Then
            PanelContainer.Controls.Add(UnistallForm)
            ReturnButton.Enabled = False
        End If
    End Sub

    Private Sub ReturnButton_Click(sender As Object, e As EventArgs) Handles ReturnButton.Click
        ReturnStep()
    End Sub

    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelButtonEx.Click
        End
    End Sub

    Private Sub ContinueButton_Click(sender As Object, e As EventArgs) Handles ContinueButton.Click
        NextStep()
    End Sub

    Private Sub UpdateButton_Click(sender As Object, e As EventArgs) Handles UpdateButton.Click
        UpdateButton.Visible = False
        Try
            PanelContainer.Controls.Clear()
            IO.Directory.Delete(Core.Helper.InstallerInstanse.InstallationDir, True)
            PanelContainer.Controls.Add(Step4Form)
            Step4Form.StartInstall()
        Catch ex As Exception
            PanelContainer.Controls.Clear()
            PanelContainer.Controls.Add(UnistallForm)
        End Try
     End Sub

End Class
