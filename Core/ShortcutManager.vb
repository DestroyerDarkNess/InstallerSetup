Imports System.Runtime.InteropServices
Imports System.Text
Imports System.IO

#Region " ShortcutManager "

' [ ShortcutManager ]
'
' // By Elektro H@cker

#Region " Usage Examples "

'Private Sub Test()

'    ' Tries to resolve a shortcut which has changed their Target location.
'    ShortcutManager.Resolve_Ui("C:\Truncated Shortcut.lnk", New IntPtr(1))
'    ShortcutManager.Resolve_NoUi("C:\Truncated Shortcut.lnk")

'    ' Creates a new Shortcut file
'    ShortcutManager.Create("C:\Shortcut.lnk",
'                           "C:\TargetFile.ext",
'                           "C:\",
'                           "Description",
'                           "-Arguments",
'                           "C:\Icon.ico", 0,
'                           ShortcutManager.HotkeyModifiers.ALT Or ShortcutManager.HotkeyModifiers.CONTROL,
'                           Keys.F1,
'                           ShortcutManager.ShortcutWindowState.Normal)

'    ' Gets Shortcut file information
'    Dim ShortcutInfo As ShortcutManager.ShortcutInfo =
'        ShortcutManager.GetInfo("C:\Shortcut.lnk")

'    Dim sb As New System.Text.StringBuilder

'    With ShortcutInfo

'        sb.AppendLine(String.Format(" ""{0}"" ", .ShortcutFile))
'        sb.AppendLine(String.Format("------------------------"))
'        sb.AppendLine(String.Format("Description: {0}", .Description))
'        sb.AppendLine(String.Format("Target: {0}", .Target))
'        sb.AppendLine(String.Format("Arguments: {0}", .Arguments))
'        sb.AppendLine(String.Format("Target Is Directory?: {0}", CStr(.IsDirectory)))
'        sb.AppendLine(String.Format("Target Is File?: {0}", CStr(.IsFile)))
'        sb.AppendLine(String.Format("WorkingDir: {0}", .WorkingDir))
'        sb.AppendLine(String.Format("DirectoryName: {0}", .DirectoryName))
'        sb.AppendLine(String.Format("FileName: {0}", .FileName))
'        sb.AppendLine(String.Format("FileExtension: {0}", .FileExtension))
'        sb.AppendLine(String.Format("DriveLetter: {0}", .DriveLetter))
'        sb.AppendLine(String.Format("Icon: {0}", .Icon))
'        sb.AppendLine(String.Format("Icon Index: {0}", CStr(.IconIndex)))
'        sb.AppendLine(String.Format("Hotkey (Hex): {0}", CStr(.Hotkey)))
'        sb.AppendLine(String.Format("Hotkey (Str): {0} + {1}", .Hotkey_Modifier.ToString, .Hotkey_Key.ToString))
'        sb.AppendLine(String.Format("Window State: {0}", .WindowState.ToString))

'    End With

'    MsgBox(sb.ToString)

'End Sub

#End Region

Public Class ShortcutManager

#Region " Variables "

    Private Shared lnk As New ShellLink()
    Private Shared lnk_data As New WIN32_FIND_DATAW()

    Private Shared lnk_arguments As New StringBuilder(260)
    Private Shared lnk_description As New StringBuilder(260)
    Private Shared lnk_target As New StringBuilder(260)
    Private Shared lnk_workingdir As New StringBuilder(260)
    Private Shared lnk_iconpath As New StringBuilder(260)
    Private Shared lnk_iconindex As Integer = -1
    Private Shared lnk_hotkey As Short = -1
    Private Shared lnk_windowstate As ShortcutWindowState = ShortcutWindowState.Normal

#End Region

#Region " API, Interfaces, Enumerations "

    <DllImport("shfolder.dll",
    CharSet:=CharSet.Auto)>
    Friend Shared Function SHGetFolderPath(ByVal hwndOwner As IntPtr,
                                           ByVal nFolder As Integer,
                                           ByVal hToken As IntPtr,
                                           ByVal dwFlags As Integer,
                                           ByVal lpszPath As StringBuilder
    ) As Integer
    End Function

    <Flags()>
    Private Enum SLGP_FLAGS

        ''' <summary>
        ''' Retrieves the standard short (8.3 format) file name.
        ''' </summary>
        SLGP_SHORTPATH = &H1

        ''' <summary>
        ''' Retrieves the Universal Naming Convention (UNC) path name of the file.
        ''' </summary>
        SLGP_UNCPRIORITY = &H2

        ''' <summary>
        ''' Retrieves the raw path name. 
        ''' A raw path is something that might not exist and may include environment variables that need to be expanded.
        ''' </summary>
        SLGP_RAWPATH = &H4

    End Enum

    <Flags()>
    Private Enum SLR_FLAGS

        ''' <summary>
        ''' Do not display a dialog box if the link cannot be resolved. When SLR_NO_UI is set,
        ''' the high-order word of fFlags can be set to a time-out value that specifies the
        ''' maximum amount of time to be spent resolving the link. The function returns if the
        ''' link cannot be resolved within the time-out duration. If the high-order word is set
        ''' to zero, the time-out duration will be set to the default value of 3,000 milliseconds
        ''' (3 seconds). To specify a value, set the high word of fFlags to the desired time-out
        ''' duration, in milliseconds.
        ''' </summary>
        SLR_NO_UI = &H1

        ''' <summary>
        ''' If the link object has changed, update its path and list of identifiers.
        ''' If SLR_UPDATE is set, you do not need to call IPersistFile::IsDirty to determine,
        ''' whether or not the link object has changed.
        ''' </summary>
        SLR_UPDATE = &H4

        ''' <summary>
        ''' Do not update the link information
        ''' </summary>
        SLR_NOUPDATE = &H8

        ''' <summary>
        ''' Do not execute the search heuristics
        ''' </summary>
        SLR_NOSEARCH = &H10

        ''' <summary>
        ''' Do not use distributed link tracking
        ''' </summary>
        SLR_NOTRACK = &H20

        ''' <summary>
        ''' Disable distributed link tracking. 
        ''' By default, distributed link tracking tracks removable media,
        ''' across multiple devices based on the volume name. 
        ''' It also uses the Universal Naming Convention (UNC) path to track remote file systems,
        ''' whose drive letter has changed.
        ''' Setting SLR_NOLINKINFO disables both types of tracking.
        ''' </summary>
        SLR_NOLINKINFO = &H40

        ''' <summary>
        ''' Call the Microsoft Windows Installer
        ''' </summary>
        SLR_INVOKE_MSI = &H80

    End Enum

    ''' <summary>
    ''' Stores information about a shortcut file.
    ''' </summary>
    Public Class ShortcutInfo

        ''' <summary>
        ''' Shortcut file full path.
        ''' </summary>
        Public Property ShortcutFile As String

        ''' <summary>
        ''' Shortcut Comment/Description.
        ''' </summary>
        Public Property Description As String

        ''' <summary>
        ''' Shortcut Target Arguments.
        ''' </summary>
        Public Property Arguments As String

        ''' <summary>
        ''' Shortcut Target.
        ''' </summary>
        Public Property Target As String

        ''' <summary>
        ''' Shortcut Working Directory.
        ''' </summary>
        Public Property WorkingDir As String

        ''' <summary>
        ''' Shortcut Icon Location.
        ''' </summary>
        Public Property Icon As String

        ''' <summary>
        ''' Shortcut Icon Index.
        ''' </summary>
        Public Property IconIndex As Integer

        ''' <summary>
        ''' Shortcut Hotkey combination.
        ''' Is represented as Hexadecimal.
        ''' </summary>
        Public Property Hotkey As Short

        ''' <summary>
        ''' Shortcut Hotkey modifiers.
        ''' </summary>
        Public Property Hotkey_Modifier As HotkeyModifiers

        ''' <summary>
        ''' Shortcut Hotkey Combination.
        ''' </summary>
        Public Property Hotkey_Key As Keys

        ''' <summary>
        ''' Shortcut Window State.
        ''' </summary>
        Public Property WindowState As ShortcutWindowState

        ''' <summary>
        ''' Indicates if the target is a file.
        ''' </summary>
        Public Property IsFile As Boolean

        ''' <summary>
        ''' Indicates if the target is a directory.
        ''' </summary>
        Public Property IsDirectory As Boolean

        ''' <summary>
        ''' Shortcut target drive letter.
        ''' </summary>
        Public Property DriveLetter As String

        ''' <summary>
        ''' Shortcut target directory name.
        ''' </summary>
        Public Property DirectoryName As String

        ''' <summary>
        ''' Shortcut target filename.
        ''' (File extension is not included in name)
        ''' </summary>
        Public Property FileName As String

        ''' <summary>
        ''' Shortcut target file extension.
        ''' </summary>
        Public Property FileExtension As String

    End Class

    ''' <summary>
    ''' Hotkey modifiers for a shortcut file.
    ''' </summary>
    <FlagsAttribute()>
    Public Enum HotkeyModifiers As Short

        ''' <summary>
        ''' The SHIFT key.
        ''' </summary>
        SHIFT = 1

        ''' <summary>
        ''' The CTRL key.
        ''' </summary>
        CONTROL = 2

        ''' <summary>
        ''' The ALT key.
        ''' </summary>
        ALT = 4

        ''' <summary>
        ''' None.
        ''' Specifies any hotkey modificator.
        ''' </summary>
        NONE = 0

    End Enum

    ''' <summary>
    ''' The Window States for a shortcut file.
    ''' </summary>
    Public Enum ShortcutWindowState As Integer

        ''' <summary>
        ''' Shortcut Window is at normal state.
        ''' </summary>
        Normal = 1

        ''' <summary>
        ''' Shortcut Window is Maximized.
        ''' </summary>
        Maximized = 3

        ''' <summary>
        ''' Shortcut Window is Minimized.
        ''' </summary>
        Minimized = 7

    End Enum

    <StructLayout(LayoutKind.Sequential,
    CharSet:=CharSet.Auto)>
    Private Structure WIN32_FIND_DATAW
        Public dwFileAttributes As UInteger
        Public ftCreationTime As Long
        Public ftLastAccessTime As Long
        Public ftLastWriteTime As Long
        Public nFileSizeHigh As UInteger
        Public nFileSizeLow As UInteger
        Public dwReserved0 As UInteger
        Public dwReserved1 As UInteger
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=260)>
        Public cFileName As String
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=14)>
        Public cAlternateFileName As String
    End Structure

    ''' <summary>
    ''' The IShellLink interface allows Shell links to be created, modified, and resolved
    ''' </summary>
    <ComImport(),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("000214F9-0000-0000-C000-000000000046")>
    Private Interface IShellLinkW

        ''' <summary>
        ''' Retrieves the path and file name of a Shell link object.
        ''' </summary>
        Sub GetPath(<Out(), MarshalAs(UnmanagedType.LPWStr)>
                    ByVal pszFile As StringBuilder,
                    ByVal cchMaxPath As Integer,
                    ByRef pfd As WIN32_FIND_DATAW,
                    ByVal fFlags As SLGP_FLAGS)

        ''' <summary>
        ''' Retrieves the list of item identifiers for a Shell link object.
        ''' </summary>
        Sub GetIDList(ByRef ppidl As IntPtr)

        ''' <summary>
        ''' Sets the pointer to an item identifier list (PIDL) for a Shell link object.
        ''' </summary>
        Sub SetIDList(ByVal pidl As IntPtr)

        ''' <summary>
        ''' Retrieves the description string for a Shell link object.
        ''' </summary>
        Sub GetDescription(<Out(), MarshalAs(UnmanagedType.LPWStr)>
                           ByVal pszName As StringBuilder,
                           ByVal cchMaxName As Integer)

        ''' <summary>
        ''' Sets the description for a Shell link object. 
        ''' The description can be any application-defined string.
        ''' </summary>
        Sub SetDescription(<MarshalAs(UnmanagedType.LPWStr)>
                           ByVal pszName As String)

        ''' <summary>
        ''' Retrieves the name of the working directory for a Shell link object.
        ''' </summary>
        Sub GetWorkingDirectory(<Out(), MarshalAs(UnmanagedType.LPWStr)>
                                ByVal pszDir As StringBuilder,
                                ByVal cchMaxPath As Integer)

        ''' <summary>
        ''' Sets the name of the working directory for a Shell link object.
        ''' </summary>
        Sub SetWorkingDirectory(<MarshalAs(UnmanagedType.LPWStr)>
                                ByVal pszDir As String)

        ''' <summary>
        ''' Retrieves the command-line arguments associated with a Shell link object.
        ''' </summary>
        Sub GetArguments(<Out(), MarshalAs(UnmanagedType.LPWStr)>
                         ByVal pszArgs As StringBuilder,
                         ByVal cchMaxPath As Integer)

        ''' <summary>
        ''' Sets the command-line arguments for a Shell link object.
        ''' </summary>
        Sub SetArguments(<MarshalAs(UnmanagedType.LPWStr)>
                         ByVal pszArgs As String)

        ''' <summary>
        ''' Retrieves the hot key for a Shell link object.
        ''' </summary>
        Sub GetHotkey(ByRef pwHotkey As Short)

        ''' <summary>
        ''' Sets a hot key for a Shell link object.
        ''' </summary>
        Sub SetHotkey(ByVal wHotkey As Short)

        ''' <summary>
        ''' Retrieves the show command for a Shell link object.
        ''' </summary>
        Sub GetShowCmd(ByRef piShowCmd As Integer)

        ''' <summary>
        ''' Sets the show command for a Shell link object. 
        ''' The show command sets the initial show state of the window.
        ''' </summary>
        Sub SetShowCmd(ByVal iShowCmd As ShortcutWindowState)

        ''' <summary>
        ''' Retrieves the location (path and index) of the icon for a Shell link object.
        ''' </summary>
        Sub GetIconLocation(<Out(), MarshalAs(UnmanagedType.LPWStr)>
                            ByVal pszIconPath As StringBuilder,
                            ByVal cchIconPath As Integer,
                            ByRef piIcon As Integer)

        ''' <summary>
        ''' Sets the location (path and index) of the icon for a Shell link object.
        ''' </summary>
        Sub SetIconLocation(<MarshalAs(UnmanagedType.LPWStr)>
                            ByVal pszIconPath As String,
                            ByVal iIcon As Integer)

        ''' <summary>
        ''' Sets the relative path to the Shell link object.
        ''' </summary>
        Sub SetRelativePath(<MarshalAs(UnmanagedType.LPWStr)>
                            ByVal pszPathRel As String,
                            ByVal dwReserved As Integer)

        ''' <summary>
        ''' Attempts to find the target of a Shell link, 
        ''' even if it has been moved or renamed.
        ''' </summary>
        Sub Resolve(ByVal hwnd As IntPtr,
                    ByVal fFlags As SLR_FLAGS)

        ''' <summary>
        ''' Sets the path and file name of a Shell link object
        ''' </summary>
        Sub SetPath(ByVal pszFile As String)

    End Interface

    <ComImport(), Guid("0000010c-0000-0000-c000-000000000046"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Public Interface IPersist

        <PreserveSig()>
        Sub GetClassID(ByRef pClassID As Guid)

    End Interface

    <ComImport(), Guid("0000010b-0000-0000-C000-000000000046"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Public Interface IPersistFile
        Inherits IPersist

        Shadows Sub GetClassID(ByRef pClassID As Guid)

        <PreserveSig()>
        Function IsDirty() As Integer

        <PreserveSig()>
        Sub Load(<[In](), MarshalAs(UnmanagedType.LPWStr)>
                 pszFileName As String,
                 dwMode As UInteger)

        <PreserveSig()>
        Sub Save(<[In](), MarshalAs(UnmanagedType.LPWStr)>
                 pszFileName As String,
                 <[In](), MarshalAs(UnmanagedType.Bool)>
                 fRemember As Boolean)

        <PreserveSig()>
        Sub SaveCompleted(<[In](), MarshalAs(UnmanagedType.LPWStr)>
                          pszFileName As String)

        <PreserveSig()>
        Sub GetCurFile(<[In](), MarshalAs(UnmanagedType.LPWStr)>
                       ppszFileName As String)

    End Interface

    ' "CLSID_ShellLink" from "ShlGuid.h"
    <ComImport(),
    Guid("00021401-0000-0000-C000-000000000046")>
    Public Class ShellLink
    End Class

#End Region

#Region " Public Methods "

    ''' <summary>
    ''' Resolves the target of a shortcut.
    ''' If shortcut can't be resolved, an error message would be displayed.
    ''' This is usefull when the target path of a shortcut file is changed from a driveletter for example,
    ''' then the shortcut file need to be resolved before trying to retrieve the target path.
    ''' </summary>
    ''' <param name="ShortcutFile">
    ''' The shortcut file to resolve.
    ''' </param>
    ''' <param name="hwnd">
    ''' The new handle pointer that would be generated
    ''' for the window which should display the error message (if any).
    ''' </param>
    Public Shared Sub Resolve_Ui(ShortcutFile As String, hwnd As IntPtr)
        LoadShortcut(ShortcutFile)
        DirectCast(lnk, IShellLinkW).Resolve(hwnd, SLR_FLAGS.SLR_UPDATE)
    End Sub

    ''' <summary>
    ''' Resolves the target of a shortcut.
    ''' If shortcut can't be resolved, any error message would be displayed.
    ''' This is usefull when the target path of a shortcut file is changed from a driveletter for example,
    ''' then the shortcut file need to be resolved before trying to retrieve the target path.
    ''' </summary>
    ''' <param name="ShortcutFile">
    ''' The shortcut file to resolve.
    ''' </param>
    Public Shared Sub Resolve_NoUi(ByVal ShortcutFile As String)
        LoadShortcut(ShortcutFile)
        DirectCast(lnk, IShellLinkW).Resolve(IntPtr.Zero, SLR_FLAGS.SLR_UPDATE Or SLR_FLAGS.SLR_NO_UI)
    End Sub

    ''' <summary>
    ''' Returns the description of a shortcut file.
    ''' </summary>
    ''' <param name="ShortcutFile">
    ''' The shortcut file to retrieve the info.
    ''' </param>
    Public Shared Function Get_Description(ByVal ShortcutFile As String) As String
        LoadShortcut(ShortcutFile)
        lnk_description.Clear()
        DirectCast(lnk, IShellLinkW).GetDescription(lnk_description, lnk_description.Capacity)
        Return lnk_description.ToString()
    End Function

    ''' <summary>
    ''' Returns the Arguments of a shortcut file.
    ''' </summary>
    ''' <param name="ShortcutFile">
    ''' The shortcut file to retrieve the info.
    ''' </param>
    Public Shared Function Get_Arguments(ByVal ShortcutFile As String) As String
        LoadShortcut(ShortcutFile)
        lnk_arguments.Clear()
        DirectCast(lnk, IShellLinkW).GetArguments(lnk_arguments, lnk_arguments.Capacity)
        Return lnk_arguments.ToString()
    End Function

    ''' <summary>
    ''' Returns the path and filename of a shortcut file.
    ''' </summary>
    ''' <param name="ShortcutFile">
    ''' The shortcut file to retrieve the info.
    ''' </param>
    Public Shared Function Get_FullPath(ByVal ShortcutFile As String) As String
        LoadShortcut(ShortcutFile)
        lnk_target.Clear()
        DirectCast(lnk, IShellLinkW).GetPath(lnk_target, lnk_target.Capacity, lnk_data, SLGP_FLAGS.SLGP_UNCPRIORITY)
        Return lnk_target.ToString()
    End Function

    ''' <summary>
    ''' Returns the working directory of a shortcut file.
    ''' </summary>
    ''' <param name="ShortcutFile">
    ''' The shortcut file to retrieve the info.
    ''' </param>
    Public Shared Function Get_WorkingDir(ByVal ShortcutFile As String) As String
        LoadShortcut(ShortcutFile)
        lnk_workingdir.Clear()
        DirectCast(lnk, IShellLinkW).GetWorkingDirectory(lnk_workingdir, lnk_workingdir.Capacity)
        Return lnk_workingdir.ToString()
    End Function

    ''' <summary>
    ''' Returns the Hotkey of a shortcut file.
    ''' </summary>
    ''' <param name="ShortcutFile">
    ''' The shortcut file to retrieve the info.
    ''' </param>
    Public Shared Function Get_Hotkey(ByVal ShortcutFile As String) As Short
        LoadShortcut(ShortcutFile)
        lnk_hotkey = -1
        DirectCast(lnk, IShellLinkW).GetHotkey(lnk_hotkey)
        Return lnk_hotkey
    End Function

    ''' <summary>
    ''' Returns the Window State of a shortcut file.
    ''' </summary>
    ''' <param name="ShortcutFile">
    ''' The shortcut file to retrieve the info.
    ''' </param>
    Public Shared Function Get_WindowStyle(ByVal ShortcutFile As String) As ShortcutWindowState
        LoadShortcut(ShortcutFile)
        DirectCast(lnk, IShellLinkW).GetShowCmd(lnk_windowstate)
        Return lnk_windowstate
    End Function

    ''' <summary>
    ''' Returns the Icon location of a shortcut file.
    ''' </summary>
    ''' <param name="ShortcutFile">
    ''' The shortcut file to retrieve the info.
    ''' </param>
    ''' <param name="IconIndex">
    ''' Optional Integer type variable to store the IconIndex.
    ''' </param>
    Public Shared Function Get_IconLocation(ByVal ShortcutFile As String,
                                            Optional ByRef IconIndex As Integer = 0) As String
        LoadShortcut(ShortcutFile)
        lnk_iconpath.Clear()
        DirectCast(lnk, IShellLinkW).GetIconLocation(lnk_iconpath, lnk_iconpath.Capacity, IconIndex)
        Return lnk_iconpath.ToString()
    End Function

    ''' <summary>
    ''' Retrieves all the information about a shortcut file.
    ''' </summary>
    ''' <param name="ShortcutFile">
    ''' The shortcut file to retrieve the info.
    ''' </param>
    Public Shared Function GetInfo(ByVal ShortcutFile As String) As ShortcutInfo

        ' Load Shortcut
        LoadShortcut(ShortcutFile)

        ' Clean objects
        lnk_description.Clear()
        lnk_arguments.Clear()
        lnk_target.Clear()
        lnk_workingdir.Clear()
        lnk_iconpath.Clear()
        lnk_hotkey = -1
        lnk_iconindex = -1

        ' Retrieve Info
        DirectCast(lnk, IShellLinkW).GetDescription(lnk_description, lnk_description.Capacity)
        DirectCast(lnk, IShellLinkW).GetArguments(lnk_arguments, lnk_arguments.Capacity)
        DirectCast(lnk, IShellLinkW).GetPath(lnk_target, lnk_target.Capacity, lnk_data, SLGP_FLAGS.SLGP_UNCPRIORITY)
        DirectCast(lnk, IShellLinkW).GetWorkingDirectory(lnk_workingdir, lnk_workingdir.Capacity)
        DirectCast(lnk, IShellLinkW).GetIconLocation(lnk_iconpath, lnk_iconpath.Capacity, lnk_iconindex)
        DirectCast(lnk, IShellLinkW).GetHotkey(lnk_hotkey)
        DirectCast(lnk, IShellLinkW).GetShowCmd(lnk_windowstate)

        ' Return Info
        Return New ShortcutInfo With {
            .ShortcutFile = ShortcutFile,
            .Description = lnk_description.ToString,
            .Arguments = lnk_arguments.ToString,
            .Target = lnk_target.ToString,
            .Icon = lnk_iconpath.ToString,
            .IconIndex = lnk_iconindex,
            .WorkingDir = lnk_workingdir.ToString,
            .Hotkey = Hex(lnk_hotkey),
            .Hotkey_Modifier = [Enum].Parse(GetType(HotkeyModifiers), GetHiByte(lnk_hotkey)),
            .Hotkey_Key = [Enum].Parse(GetType(Keys), GetLoByte(lnk_hotkey)),
            .WindowState = lnk_windowstate,
            .IsFile = File.Exists(lnk_target.ToString),
            .IsDirectory = Directory.Exists(lnk_target.ToString),
            .DriveLetter = lnk_target.ToString.Substring(0, 1),
            .DirectoryName = lnk_target.ToString.Substring(0, lnk_target.ToString.LastIndexOf("\")),
            .FileName = lnk_target.ToString.Split("\").LastOrDefault.Split(".").FirstOrDefault,
            .FileExtension = lnk_target.ToString.Split(".").LastOrDefault
        }

    End Function

    ''' <summary>
    ''' Creates a shortcut file.
    ''' </summary>
    ''' <param name="FilePath">
    ''' The filepath to create the shortcut.
    ''' </param>
    ''' <param name="Target">
    ''' The target file or directory.
    ''' </param>
    ''' <param name="WorkingDirectory">
    ''' The working directory os the shortcut.
    ''' </param>
    ''' <param name="Description">
    ''' The shortcut description.
    ''' </param>
    ''' <param name="Arguments">
    ''' The target file arguments.
    ''' This value only should be set when target is an executable file.
    ''' </param>
    ''' <param name="Icon">
    ''' The icon location of the shortcut.
    ''' </param>
    ''' <param name="IconIndex">
    ''' The icon index of the icon file.
    ''' </param>
    ''' <param name="HotKey_Modifier">
    ''' The hotkey modifier(s) which should be used for the hotkey combination.
    ''' <paramref name="HotkeyModifiers"/> can be one or more modifiers.
    ''' </param>
    ''' <param name="HotKey_Key">
    ''' The key used in combination with the <paramref name="HotkeyModifiers"/> for hotkey combination.
    ''' </param>
    ''' <param name="WindowState">
    ''' The Window state for the target.
    ''' </param>
    Public Shared Sub Create(ByVal FilePath As String,
                             ByVal Target As String,
                             Optional ByVal WorkingDirectory As String = Nothing,
                             Optional ByVal Description As String = Nothing,
                             Optional ByVal Arguments As String = Nothing,
                             Optional ByVal Icon As String = Nothing,
                             Optional ByVal IconIndex As Integer = Nothing,
                             Optional ByVal HotKey_Modifier As HotkeyModifiers = Nothing,
                             Optional ByVal HotKey_Key As Keys = Nothing,
                             Optional ByVal WindowState As ShortcutWindowState = ShortcutWindowState.Normal)

        LoadShortcut(FilePath)

        DirectCast(lnk, IShellLinkW).SetPath(Target)

        DirectCast(lnk, IShellLinkW).SetWorkingDirectory(If(WorkingDirectory IsNot Nothing,
                                                            WorkingDirectory,
                                                            Path.GetDirectoryName(Target)))

        DirectCast(lnk, IShellLinkW).SetDescription(Description)
        DirectCast(lnk, IShellLinkW).SetArguments(Arguments)
        DirectCast(lnk, IShellLinkW).SetIconLocation(Icon, IconIndex)

        DirectCast(lnk, IShellLinkW).SetHotkey(If(HotKey_Modifier + HotKey_Key <> 0,
                                                  Convert.ToInt32(CInt(HotKey_Modifier & Hex(HotKey_Key)), 16),
                                                  Nothing))

        DirectCast(lnk, IShellLinkW).SetShowCmd(WindowState)

        DirectCast(lnk, IPersistFile).Save(FilePath, True)
        DirectCast(lnk, IPersistFile).SaveCompleted(FilePath)

    End Sub

#End Region

#Region " Private Methods "

    ''' <summary>
    ''' Loads the shortcut object to retrieve information.
    ''' </summary>
    ''' <param name="ShortcutFile">
    ''' The shortcut file to retrieve the info.
    ''' </param>
    Private Shared Sub LoadShortcut(ByVal ShortcutFile As String)
        DirectCast(lnk, IPersistFile).Load(ShortcutFile, 0)
    End Sub

    ''' <summary>
    ''' Gets the low order byte of a number.
    ''' </summary>
    Private Shared Function GetLoByte(ByVal Intg As Integer) As Integer
        Return Intg And &HFF&
    End Function

    ''' <summary>
    ''' Gets the high order byte of a number.
    ''' </summary>
    Private Shared Function GetHiByte(ByVal Intg As Integer) As Integer
        Return (Intg And &HFF00&) / 256
    End Function

#End Region

End Class

#End Region