Public Class Step3



    Private Sub Step3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RichTextLabel1.AppendText("Click on " & """", Color.White, , New Font("Segoe UI", 9, FontStyle.Bold))
        RichTextLabel1.AppendText("Continue" & """", Color.DodgerBlue, , New Font("Segoe UI", 10, FontStyle.Regular))
        RichTextLabel1.AppendText(" to continue with the installation.", Color.White, , New Font("Segoe UI", 9, FontStyle.Bold))
        InstallationDirtextbox.Text = Core.Helper.InstallerInstanse.InstallationDir
    End Sub




    Private Sub KnightButton1_Click(sender As Object, e As EventArgs) Handles KnightButton1.Click
        Dim NewInstallDir As String = String.Empty
        If FolderBrowserDialog1.ShowDialog(Me) = DialogResult.OK Then

            NewInstallDir = FolderBrowserDialog1.DirectoryPath

            If IO.Directory.Exists(NewInstallDir) = True Then
                Core.Helper.InstallerInstanse.InstallationDir = NewInstallDir
                InstallationDirtextbox.Text = Core.Helper.InstallerInstanse.InstallationDir
            End If

        End If

    End Sub

End Class