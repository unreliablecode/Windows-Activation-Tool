using System;
using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        // Print the current Windows version and edition
        string windowsVersion = GetWindowsVersion();
        string windowsEdition = GetWindowsEdition();
        Console.WriteLine($"Current Windows Version: {windowsVersion}");
        Console.WriteLine($"Current Windows Edition: {windowsEdition}");

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

    private static string GetWindowsVersion()
    {
        Version version = Environment.OSVersion.Version;
        string versionString = $"{version.Major}.{version.Minor}";

        if (version.Build != 0)
        {
            versionString += $".{version.Build}";
        }

        return versionString;
    }

    private static string GetWindowsEdition()
    {
        string edition = string.Empty;

        try
        {
            using (var regKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion"))
            {
                if (regKey != null)
                {
                    edition = regKey.GetValue("EditionID") as string;
                }
            }
        }
        catch (Exception Ex)
        {
            Console.WriteLine(Ex.Message);
        }

        return edition;
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
                CreateNoWindow = true
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
