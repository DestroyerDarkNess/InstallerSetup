Namespace Core

    Public Class Helper

        Public Shared InstallerInstanse As New InstallerCore
        Public Shared ComRegistered As New Core.COManager
        Public Shared MainForm As Form1 = Nothing

        Public Shared Sub ControlCorner(ctrl As Control, CurveSize As Integer)
            Dim p As New System.Drawing.Drawing2D.GraphicsPath

            p.StartFigure()
            p.AddArc(New Rectangle(0, 0, CurveSize, CurveSize), 180, 90)
            'p.AddLine(CurveSize, 0, ctrl.Width - CurveSize, 0)

            p.AddArc(New Rectangle(ctrl.Width - CurveSize, 0, CurveSize, CurveSize), -90, 90)
            'p.AddLine(ctrl.Width, CurveSize, ctrl.Width, ctrl.Height - CurveSize)

            p.AddArc(New Rectangle(ctrl.Width - CurveSize, ctrl.Height - CurveSize, CurveSize, CurveSize), 0, 90)
            'p.AddLine(ctrl.Width - 40, ctrl.Height, 40, ctrl.Height)

            p.AddArc(New Rectangle(0, ctrl.Height - CurveSize, CurveSize, CurveSize), 90, 90)
            p.CloseFigure()

            ctrl.Region = New Region(p)
            p.Dispose()
        End Sub

        Public Shared Sub WaitTemp(ByVal seconds As Integer)
            For i As Integer = 0 To seconds * 100
                System.Threading.Thread.Sleep(10)
                Application.DoEvents()
            Next
        End Sub

#Region " Get All Files Function "

        ' [ Get All Files Function ]
        '
        '
        ' Examples:
        '
        ' Dim Files As Array = Get_All_Files("C:\Test", True)
        ' For Each File In Get_All_Files("C:\Test", False) : MsgBox(File) : Next

        Public Shared Function Get_All_Files(ByVal Directory As String, Optional ByVal Recursive As Boolean = False) As Array
            If System.IO.Directory.Exists(Directory) Then
                If Not Recursive Then : Return System.IO.Directory.GetFiles(Directory, "*", IO.SearchOption.TopDirectoryOnly)
                Else : Return IO.Directory.GetFiles(Directory, "*", IO.SearchOption.AllDirectories)
                End If
            Else
                Return Nothing
            End If
        End Function

#End Region

#Region " RenameFile function "

        ' Usage:
        '
        ' RenameFile("C:\Test.txt", "TeSt.TxT")
        ' RenameFile("C:\Test.txt", "Test", "doc")
        ' RenameFile(FileInfoObject.FullName, FileInfoObject.Name.ToLower, FileInfoObject.Extension.ToUpper)
        ' If RenameFile("C:\Test.txt", "TeSt.TxT") Is Nothing Then MsgBox("El archivo no existe!")

        Public Shared Function RenameFile(ByVal File As String, ByVal NewFileName As String, Optional ByVal NewFileExtension As String = Nothing)
            If IO.File.Exists(File) Then
                Try
                    Dim FileToBeRenamed As New System.IO.FileInfo(File)
                    If NewFileExtension Is Nothing Then
                        FileToBeRenamed.MoveTo(FileToBeRenamed.Directory.FullName & "\" & NewFileName) ' Rename file with same extension
                    Else
                        FileToBeRenamed.MoveTo(FileToBeRenamed.Directory.FullName & "\" & NewFileName & NewFileExtension) ' Rename file with new extension
                    End If
                    Return True ' File was renamed OK
                Catch ex As Exception
                    ' MsgBox(ex.Message)
                    Return False ' File can't be renamed maybe because User Permissions
                End Try
            Else
                Return Nothing ' File doesn't exist
            End If
        End Function

#End Region

    End Class


End Namespace
