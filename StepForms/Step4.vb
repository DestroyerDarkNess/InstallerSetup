Imports InstallerSetup.Core.Helper
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Runtime.InteropServices.ComTypes

Public Class Step4

    Private Sub Step4_Load(sender As Object, e As EventArgs) Handles MyBase.Load
       
    End Sub

    Public Sub StartInstall()
        On Error Resume Next
        If Core.Helper.MainForm IsNot Nothing Then
            Core.Helper.MainForm.ContinueButton.Visible = False
            Core.Helper.MainForm.ReturnButton.Visible = False
        End If
        Timer1.Enabled = True
        StatusLabel.Text = "Installing Please wait..." : Core.Helper.WaitTemp(1)
        Dim thread = New System.Threading.Thread(New System.Threading.ThreadStart(AddressOf StartInstallAsync))
        thread.Start()
    End Sub

    Private Sub StartInstallAsync()
        Core.Helper.InstallerInstanse.ZipBytes = My.Resources.Files
        Dim InstallResult As Boolean = Core.Helper.InstallerInstanse.Install
        If InstallResult = False Then
            MsgBox("Error has ocurret!" & vbNewLine & Core.Helper.InstallerInstanse.ExErrorMessage)
            End
        End If

        If IO.File.Exists(Core.Helper.InstallerInstanse.MainFileDir) = True Then

            Me.BeginInvoke(Sub()
                               StatusLabel.Text = "Creating Shortcut... #Testing Method 1"
                               StatusLabel.ForeColor = Color.White
                               Core.Helper.WaitTemp(2)
                           End Sub)

            Dim ShortcutName As String = "CodeSmart" 'IO.Path.GetTempPath & ShortcutName & ".lnk" '
            Dim Shortcutdir As String = IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), ShortcutName & ".lnk")

            ShortcutManager.Create(Shortcutdir,
                      Core.Helper.InstallerInstanse.MainFileDir,
                    Core.Helper.InstallerInstanse.InstallationDir,
                      "Code Smart Powerfulll Script IDE",
                       "",
                   Core.Helper.InstallerInstanse.MainFileDir, 0,
                        ShortcutManager.HotkeyModifiers.ALT Or ShortcutManager.HotkeyModifiers.CONTROL,
                    Keys.S,
                      ShortcutManager.ShortcutWindowState.Normal)



            If IO.File.Exists(Shortcutdir) = False Then

                Me.BeginInvoke(Sub()
                                   StatusLabel.Text = "Creating Shortcut... [Method 1 Failure!...] #Testing Method 2"
                                   StatusLabel.ForeColor = Color.Yellow
                                   Core.Helper.WaitTemp(2)
                               End Sub)

                Dim link As IShellLink = CType(New ShellLink(), IShellLink)
                link.SetDescription("Code Smart - Powerfulll Script IDE")
                link.SetPath(Core.Helper.InstallerInstanse.MainFileDir)
                Dim file As IPersistFile = CType(link, IPersistFile)
                Dim desktopPath As String = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
                file.Save(IO.Path.Combine(desktopPath, ShortcutName & ".lnk"), False)

                If IO.File.Exists(Shortcutdir) = False Then

                    Me.BeginInvoke(Sub()
                                       StatusLabel.Text = "Methods 1 and 2 Failed, Error creating shortcut"
                                       StatusLabel.ForeColor = Color.Red
                                   End Sub)

                    MsgBox("Error creating desktop shortcut." & vbNewLine & "To run the Application go to the following path:" & vbNewLine & Core.Helper.InstallerInstanse.InstallationDir)

                End If

            End If

        End If

        Dim ComRegisteredDir As String = Core.Helper.InstallerInstanse.InstallationDir & "\VBSDebugger.dll"



        Me.BeginInvoke(Sub()
                           StatusLabel.Text = "Registering COM " & """" & "VBSDebugger.dll" & """" & " , Please Wait..."
                           StatusLabel.ForeColor = Color.White
                           Core.Helper.WaitTemp(2)
                       End Sub)

        If IO.File.Exists(ComRegisteredDir) = True Then

            Dim RegisterResult As Boolean = Core.Helper.ComRegistered.RegisterCOM(ComRegisteredDir)

            If RegisterResult = True Then
                Me.BeginInvoke(Sub()
                                   StatusLabel.Text = "COM Correctly Registered"
                                   StatusLabel.ForeColor = Color.Lime
                                   Core.Helper.WaitTemp(2)
                               End Sub)
            Else
                Me.BeginInvoke(Sub()
                                   StatusLabel.Text = "Error Registering COM!"
                                   StatusLabel.ForeColor = Color.Red
                                   Core.Helper.WaitTemp(2)
                               End Sub)
            End If

        Else

            Me.BeginInvoke(Sub()
                               StatusLabel.Text = """" & "VBSDebugger.dll" & """" & " Not found to register"
                               StatusLabel.ForeColor = Color.Red
                               Core.Helper.WaitTemp(2)
                           End Sub)
        End If

        Me.BeginInvoke(Sub()
                           StatusLabel.Text = "Creating Registry Keys"
                           StatusLabel.ForeColor = Color.White
                           Core.Helper.WaitTemp(2)
                       End Sub)

        Dim KeyRegedit As String = Core.Helper.InstallerInstanse.MainFileDir & " " & """" & "%1" & """"
        RegEdit.CreateSubKey(fullKeyPath:="HKCR\*\shell\Open with CodeSmart\command\")
        Core.Helper.WaitTemp(2)
        RegEdit.CreateValue(fullKeyPath:="HKCR\*\shell\Open with CodeSmart\command\",
                            valueName:="",
                            valueData:=KeyRegedit,
                            valueType:=Microsoft.Win32.RegistryValueKind.String)

        Me.BeginInvoke(Sub()
                           StatusLabel.Text = "Finished Process..."
                           If Core.Helper.MainForm IsNot Nothing Then
                               Core.Helper.MainForm.NextStep()
                           End If
                       End Sub)

        Dim UnistallerDir As String = IO.Path.Combine(Core.Helper.InstallerInstanse.InstallationDir, "SetupWizard.exe")

        If IO.File.Exists(UnistallerDir) = False Then
            IO.File.Copy(Application.ExecutablePath, UnistallerDir)
        End If

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        XylosProgressBar1.Value = Core.Helper.InstallerInstanse.InstallProgress
        If XylosProgressBar1.Value = XylosProgressBar1.Maximum Then
            Timer1.Enabled = False
        Else
            Dim TheLastFile As String = LastFileWritte(Core.Helper.InstallerInstanse.InstallationDir)
            If Not TheLastFile = String.Empty Then
                StatusLabel.Text = TheLastFile
            End If
        End If
    End Sub

    Private Function LastFileWritte(ByVal Location As String) As String
        If IO.Directory.Exists(Location) = True Then
            Dim dir = New System.IO.DirectoryInfo(Location)
            Dim file = dir.EnumerateFiles("*.*").OrderByDescending(Function(f) f.LastWriteTime).FirstOrDefault()

            If file IsNot Nothing Then
                Dim FilePath As String = file.FullName
                Return FilePath
            End If
        End If
        Return String.Empty
    End Function

End Class


<ComImport>
<Guid("00021401-0000-0000-C000-000000000046")>
Friend Class ShellLink
End Class

<ComImport>
<InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
<Guid("000214F9-0000-0000-C000-000000000046")>
Friend Interface IShellLink
    Sub GetPath(
<Out, MarshalAs(UnmanagedType.LPWStr)> ByVal pszFile As StringBuilder, ByVal cchMaxPath As Integer, <Out> ByRef pfd As IntPtr, ByVal fFlags As Integer)
    Sub GetIDList(<Out> ByRef ppidl As IntPtr)
    Sub SetIDList(ByVal pidl As IntPtr)
    Sub GetDescription(
<Out, MarshalAs(UnmanagedType.LPWStr)> ByVal pszName As StringBuilder, ByVal cchMaxName As Integer)
    Sub SetDescription(
<MarshalAs(UnmanagedType.LPWStr)> ByVal pszName As String)
    Sub GetWorkingDirectory(
<Out, MarshalAs(UnmanagedType.LPWStr)> ByVal pszDir As StringBuilder, ByVal cchMaxPath As Integer)
    Sub SetWorkingDirectory(
<MarshalAs(UnmanagedType.LPWStr)> ByVal pszDir As String)
    Sub GetArguments(
<Out, MarshalAs(UnmanagedType.LPWStr)> ByVal pszArgs As StringBuilder, ByVal cchMaxPath As Integer)
    Sub SetArguments(
<MarshalAs(UnmanagedType.LPWStr)> ByVal pszArgs As String)
    Sub GetHotkey(<Out> ByRef pwHotkey As Short)
    Sub SetHotkey(ByVal wHotkey As Short)
    Sub GetShowCmd(<Out> ByRef piShowCmd As Integer)
    Sub SetShowCmd(ByVal iShowCmd As Integer)
    Sub GetIconLocation(
<Out, MarshalAs(UnmanagedType.LPWStr)> ByVal pszIconPath As StringBuilder, ByVal cchIconPath As Integer, <Out> ByRef piIcon As Integer)
    Sub SetIconLocation(
<MarshalAs(UnmanagedType.LPWStr)> ByVal pszIconPath As String, ByVal iIcon As Integer)
    Sub SetRelativePath(
<MarshalAs(UnmanagedType.LPWStr)> ByVal pszPathRel As String, ByVal dwReserved As Integer)
    Sub Resolve(ByVal hwnd As IntPtr, ByVal fFlags As Integer)
    Sub SetPath(
<MarshalAs(UnmanagedType.LPWStr)> ByVal pszFile As String)
End Interface