open System
open System.Diagnostics

let activateWindows (productKey: string) =
    let activationCommand = sprintf "slmgr.vbs /ipk %s" productKey
    try
        let psi = new ProcessStartInfo(
            FileName = "cmd.exe",
            Verb = "runas", // Run as administrator
            Arguments = sprintf "/K %s" activationCommand, // /K keeps the Command Prompt window open
            UseShellExecute = true,
            CreateNoWindow = true)
        Process.Start(psi).WaitForExit()
        true
    with
    | _ -> false

[<EntryPoint>]
let main argv =
    Console.WriteLine("Enter the Windows product key:")
    let productKey = Console.ReadLine()
    
    if activateWindows productKey then
        Console.WriteLine("Windows has been successfully activated!")
    else
        Console.WriteLine("Failed to activate Windows. Please check the product key and try again.")
    
    Console.WriteLine("Press any key to exit...")
    Console.ReadKey() |> ignore
    0
