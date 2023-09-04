using System;
using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter the Windows product key:");
        string productKey = Console.ReadLine();

        if (ActivateWindows(productKey))
        {
            Console.WriteLine("Windows has been successfully activated!");
        }
        else
        {
            Console.WriteLine("Failed to activate Windows. Please check the product key and try again.");
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    private static bool ActivateWindows(string productKey)
    {
        string activationCommand = $"slmgr.vbs /ipk {productKey}";

        try
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Verb = "runas", 
                Arguments = $"/K {activationCommand}",
                UseShellExecute = true,
            };

            Process.Start(psi).WaitForExit();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
