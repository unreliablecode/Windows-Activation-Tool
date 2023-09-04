using System;
using System.Diagnostics;
using Microsoft.Win32;
using System.Management;

class Program
{
    static void Main(string[] args)
    {
        // Print the current Windows version, edition, and OS name
        //string windowsVersion = GetWindowsVersion();
        //string windowsEdition = GetWindowsEdition();
        string osName = GetOSFriendlyName();
        //Console.WriteLine($"Current Windows Version: {windowsVersion}");
        //Console.WriteLine($"Current Windows Edition: {windowsEdition}");
        Console.WriteLine($"Operating System: {osName}");

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
        RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
        var buildNumber = registryKey.GetValue("UBR").ToString();

        Version version = Environment.OSVersion.Version;
        string versionString = $"{version.Major}.{version.Minor}";

        if (!string.IsNullOrEmpty(buildNumber))
        {
            versionString += $".{buildNumber}";
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
        catch (Exception)
        {
            // Handle any exceptions that may occur while reading the registry
        }

        return edition;
    }

    public static string GetOSFriendlyName()
    {
        string result = string.Empty;
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem");
        foreach (ManagementObject os in searcher.Get())
        {
            result = os["Caption"].ToString();
            break;
        }
        return result;
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
