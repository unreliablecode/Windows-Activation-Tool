#include <iostream>
#include <windows.h>
#include <Shlwapi.h>
#include <string>

// Function to get the friendly name of the operating system
std::wstring GetOSFriendlyName() {
    std::wstring result;
    HKEY hKey;
    if (RegOpenKeyExW(HKEY_LOCAL_MACHINE, L"SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", 0, KEY_READ, &hKey) == ERROR_SUCCESS) {
        WCHAR buffer[MAX_PATH];
        DWORD bufferSize = sizeof(buffer);
        if (RegQueryValueExW(hKey, L"ProductName", nullptr, nullptr, (LPBYTE)buffer, &bufferSize) == ERROR_SUCCESS) {
            result = buffer;
        }
        RegCloseKey(hKey);
    }
    return result;
}

// Function to activate Windows using a product key
bool ActivateWindows(const std::wstring& productKey) {
    std::wstring activationCommand = L"slmgr.vbs /ipk " + productKey;

    STARTUPINFOW si = { sizeof(STARTUPINFOW) };
    PROCESS_INFORMATION pi;

    if (CreateProcessW(NULL, const_cast<LPWSTR>(activationCommand.c_str()), NULL, NULL, FALSE, CREATE_NO_WINDOW, NULL, NULL, &si, &pi)) {
        WaitForSingleObject(pi.hProcess, INFINITE);
        CloseHandle(pi.hProcess);
        CloseHandle(pi.hThread);
        return true;
    }
    return false;
}

int main() {
    while (true) {
        std::wstring osName = GetOSFriendlyName();
        std::wstring message = L"Operating System: " + osName;

        MessageBoxW(NULL, message.c_str(), L"Operating System Info", MB_ICONINFORMATION);

        std::wstring productKey;
        std::wcout << "Enter the Windows product key: ";
        std::wcin >> productKey;

        if (ActivateWindows(productKey)) {
            MessageBoxW(NULL, L"Windows has been successfully activated!", L"Activation Success", MB_ICONINFORMATION);
        }
        else {
            MessageBoxW(NULL, L"Failed to activate Windows. Please check the product key and try again.", L"Activation Error", MB_ICONERROR);
        }

        int choice = MessageBoxW(NULL, L"Do you want to exit?", L"Exit Program", MB_YESNO | MB_ICONQUESTION);
        if (choice == IDYES) {
            break; // Exit the loop
        }
    }
    return 0;
}
