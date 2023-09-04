Imports System
Imports System.Diagnostics

Module Program
    Sub Main()
        Console.WriteLine("Enter the Windows product key:")
        Dim productKey As String = Console.ReadLine()

        If ActivateWindows(productKey) Then
            Console.WriteLine("Windows has been successfully activated!")
        Else
            Console.WriteLine("Failed to activate Windows. Please check the product key and try again.")
        End If

        Console.WriteLine("Press any key to exit...")
        Console.ReadKey()
    End Sub

    Function ActivateWindows(ByVal productKey As String) As Boolean
        Dim activationCommand As String = $"slmgr.vbs /ipk {productKey}"

        Try
            Dim psi As New ProcessStartInfo With {
                .FileName = "cmd.exe",
                .Verb = "runas", ' Run as administrator
                .Arguments = $"/K {activationCommand}", ' /K keeps the Command Prompt window open
                .UseShellExecute = True,
                .CreateNoWindow = True
            }

            Process.Start(psi).WaitForExit()
            Return True
        Catch
            Return False
        End Try
    End Function
End Module
