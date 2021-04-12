Imports System.IO

Namespace Core

    Public Class COManager

        Private ReadOnly _regasmPaths As List(Of String) = New List(Of String) From {
   "C:\Windows\Microsoft.NET\Framework64\v2.0.50727\regasm.exe",
   "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\regasm.exe",
   "C:\Windows\Microsoft.NET\Framework\v2.0.50727\regasm.exe",
   "C:\Windows\Microsoft.NET\Framework\v4.0.30319\regasm.exe"
}

        Private RegasmPath As String = String.Empty
        Private ValidRegasm As Boolean = False
        Public ErrorOuput As String = String.Empty

        Public Sub New()
            _regasmPaths.ForEach(Sub(regasmPath)
                                     If File.Exists(regasmPath) Then
                                         regasmPath = regasmPath
                                         ValidRegasm = True
                                     Else
                                         ValidRegasm = False
                                     End If
                                 End Sub)
        End Sub

        Public Function RegisterCOM(ByVal FilePath As String) As Boolean
            If Not ValidRegasm Then
                MessageBox.Show("Invalid regasm.exe file path! Check regasm.exe file exists in the specified location.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End If

            Dim regasmArguments = "/codebase " & """" & FilePath & """"

            Return RunRegAsmCommand(regasmArguments, "register")
        End Function

        Public Function UnRegisterCOM(ByVal FilePath As String) As Boolean
            If Not ValidRegasm Then
                MessageBox.Show("Invalid regasm.exe file path! Check regasm.exe file exists in the specified location.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End If

            Dim regasmArguments = "/u " & """" & FilePath & """"

            Return RunRegAsmCommand(regasmArguments, "unregister")
        End Function

        Private Function RunRegAsmCommand(ByVal regasmArguments As String, ByVal OperationType As String) As Boolean
            Try

                Dim process = New Process()
                Dim processStartInfo = New ProcessStartInfo With {
                    .WindowStyle = ProcessWindowStyle.Normal,
                    .FileName = RegasmPath,
                    .Arguments = regasmArguments,
                    .UseShellExecute = False,
                    .RedirectStandardOutput = True,
                    .RedirectStandardError = True,
                    .CreateNoWindow = True,
                    .Verb = "runas"
                }
                process.StartInfo.RedirectStandardOutput = True
                process.StartInfo.RedirectStandardError = True
                process.StartInfo = processStartInfo
                process.Start()
                process.WaitForExit()

                Dim HostOutput As String = process.StandardOutput.ReadToEnd

                Return Process_OutputData(HostOutput, OperationType)

            Catch ex As Exception
                Return False
            End Try
        End Function

        Private Function Process_OutputData(ByVal Data As String, ByVal _lastOperationType As String) As Boolean

            If Data IsNot Nothing Then

                If _lastOperationType = "register" Then

                    If Data.Contains("registered successfully") Then
                        Return True
                    End If
                ElseIf _lastOperationType = "unregister" Then
                    If Data.Contains("un-registered successfully") Then
                        Return True
                    End If
                End If

            End If
            ErrorOuput = "-COM " & _lastOperationType & " Error! " & vbNewLine & Data
            Return False
        End Function

    End Class

End Namespace

