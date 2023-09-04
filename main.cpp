#include <iostream>
#include <windows.h>
#include <Shlwapi.h>

// Function to get the friendly name of the operating system
std::string GetOSFriendlyName() {
    std::string result;
    HKEY hKey;
    if (RegOpenKeyEx(HKEY_LOCAL_MACHINE, "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", 0, KEY_READ, &hKey) == ERROR_SUCCESS) {
        char buffer[MAX_PATH];
        DWORD bufferSize = sizeof(buffer);
        if (RegQueryValueEx(hKey, "ProductName", nullptr, nullptr, (LPBYTE)buffer, &bufferSize) == ERROR_SUCCESS) {
            result = buffer;
        }
        RegCloseKey(hKey);
    }
    return result;
}

// Function to activate Windows using a product key
bool ActivateWindows(const std::string& productKey) {
    std::string activationCommand = "slmgr.vbs /ipk " + productKey;

    STARTUPINFO si = { sizeof(STARTUPINFO) };
    PROCESS_INFORMATION pi;

    if (CreateProcess(NULL, const_cast<char*>(activationCommand.c_str()), NULL, NULL, FALSE, CREATE_NO_WINDOW, NULL, NULL, &si, &pi)) {
        WaitForSingleObject(pi.hProcess, INFINITE);
        CloseHandle(pi.hProcess);
        CloseHandle(pi.hThread);
        return true;
    }
    return false;
}

int main() {
    std::string osName = GetOSFriendlyName();
    std::cout << "Operating System: " << osName << std::endl;

    std::cout << "Enter the Windows product key: ";
    std::string productKey;
    std::cin >> productKey;

    if (ActivateWindows(productKey)) {
        std::cout << "Windows has been successfully activated!" << std::endl;
    } else {
        std::cout << "Failed to activate Windows. Please check the product key and try again." << std::endl;
    }

    std::cout << "Press any key to exit...";
    std::cin.get();
    return 0;
}
