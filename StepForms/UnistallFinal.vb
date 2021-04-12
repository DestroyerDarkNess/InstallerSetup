Public Class UnistallFinal

    Private Sub UnistallFinal_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Public Sub StartUninstall()
        If Core.Helper.MainForm IsNot Nothing Then
            Core.Helper.MainForm.ContinueButton.Visible = False
            Core.Helper.MainForm.UpdateButton.Visible = False
            Core.Helper.MainForm.ReturnButton.Visible = False
        End If
        StatusLabel.Text = "Uninstalling Please wait..." : Core.Helper.WaitTemp(1)
        Dim thread = New System.Threading.Thread(New System.Threading.ThreadStart(AddressOf StartUnInstallAsync))
        thread.Start()
    End Sub

    Private Sub StartUnInstallAsync()
        On Error Resume Next
        If Core.Helper.InstallerInstanse.Installed = True Then
            Dim ProcessMain() As Process = Process.GetProcessesByName(IO.Path.GetFileNameWithoutExtension(Core.Helper.InstallerInstanse.MainFileDir))

            For Each ProcessInstances As Process In ProcessMain
                ProcessInstances.Kill()
            Next
            XylosProgressBar1.Value = 12
            If IO.File.Exists(Core.Helper.InstallerInstanse.MainFileDir) = True Then

                Me.BeginInvoke(Sub()
                                   StatusLabel.Text = "Deleting Shortcut..."
                                   StatusLabel.ForeColor = Color.White
                                   Core.Helper.WaitTemp(2)
                               End Sub)

                Dim ShortcutName As String = "CodeSmart" 'IO.Path.GetTempPath & ShortcutName & ".lnk" '
                Dim Shortcutdir As String = IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), ShortcutName & ".lnk")

                XylosProgressBar1.Value = 20

                If IO.File.Exists(Shortcutdir) = False Then

                    IO.File.Delete(Shortcutdir)

                Else

                    Dim desktopPath As String = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
                    Dim Shorcut2 As String = IO.Path.Combine(desktopPath, ShortcutName & ".lnk")

                    If IO.File.Exists(Shortcutdir) = False Then

                        IO.File.Delete(Shortcutdir)

                    End If

                End If

            End If

            XylosProgressBar1.Value = 25

            Dim ComRegisteredDir As String = Core.Helper.InstallerInstanse.InstallationDir & "\VBSDebugger.dll"



            Me.BeginInvoke(Sub()
                               StatusLabel.Text = "Unregistering COM " & """" & "VBSDebugger.dll" & """" & " , Please Wait..."
                               StatusLabel.ForeColor = Color.White
                               Core.Helper.WaitTemp(2)
                           End Sub)

            If IO.File.Exists(ComRegisteredDir) = True Then

                Dim RegisterResult As Boolean = Core.Helper.ComRegistered.UnRegisterCOM(ComRegisteredDir)

                If RegisterResult = True Then
                    Me.BeginInvoke(Sub()
                                       StatusLabel.Text = "COM Correctly Unregistered"
                                       StatusLabel.ForeColor = Color.Lime
                                       Core.Helper.WaitTemp(2)
                                   End Sub)
                End If

            End If

            XylosProgressBar1.Value = 36

            Me.BeginInvoke(Sub()
                               StatusLabel.Text = "Deleting Files..."
                           End Sub)


            For Each File In Core.Helper.Get_All_Files(Core.Helper.InstallerInstanse.InstallationDir, False)
                StatusLabel.Text = "Deleting " & """" & File & """"
                IO.File.Delete(File)
                If Not XylosProgressBar1.Value = XylosProgressBar1.Maximum Then
                    XylosProgressBar1.Value += 1
                End If
            Next

            Me.BeginInvoke(Sub()
                               StatusLabel.Text = "Deleting Registry Keys"
                               StatusLabel.ForeColor = Color.White
                               Core.Helper.WaitTemp(2)
                           End Sub)

            RegEdit.DeleteSubKey(fullKeyPath:="HKCR\*\shell\Open with CodeSmart",
                       throwOnMissingSubKey:=False)

            If Not XylosProgressBar1.Value = XylosProgressBar1.Maximum Then
                XylosProgressBar1.Value += (XylosProgressBar1.Maximum - XylosProgressBar1.Value) / 2
            End If

            Me.BeginInvoke(Sub()
                               StatusLabel.Text = "Uninstallation Complete...."
                               If Core.Helper.MainForm IsNot Nothing Then
                                   Core.Helper.MainForm.NextStep()
                               End If
                           End Sub)

            XylosProgressBar1.Value = XylosProgressBar1.Maximum

        Else
            End
        End If

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs)

    End Sub

End Class