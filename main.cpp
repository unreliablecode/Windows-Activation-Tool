#include <iostream>
#include <cstdlib>
#include <string>
#include <windows.h>

int main() {
    std::cout << "Enter the Windows product key: ";
    std::string productKey;
    std::getline(std::cin, productKey);

    if (ActivateWindows(productKey)) {
        std::cout << "Windows has been successfully activated!" << std::endl;
    } else {
        std::cout << "Failed to activate Windows. Please check the product key and try again." << std::endl;
    }

    std::cout << "Press any key to exit...";
    std::cin.get();
    return 0;
}

bool ActivateWindows(const std::string& productKey) {
    std::string activationCommand = "slmgr.vbs /ipk " + productKey;

    try {
        SHELLEXECUTEINFO shExecInfo = {0};
        shExecInfo.cbSize = sizeof(SHELLEXECUTEINFO);
        shExecInfo.fMask = SEE_MASK_NOCLOSEPROCESS;
        shExecInfo.hwnd = NULL;
        shExecInfo.lpVerb = L"runas";  // Run as administrator
        shExecInfo.lpFile = L"cmd.exe";
        shExecInfo.lpParameters = (LPWSTR)activationCommand.c_str();
        shExecInfo.nShow = SW_HIDE;     // Hide the Command Prompt window

        if (ShellExecuteEx(&shExecInfo)) {
            WaitForSingleObject(shExecInfo.hProcess, INFINITE);
            CloseHandle(shExecInfo.hProcess);
            return true;
        } else {
            return false;
        }
    } catch (...) {
        return false;
    }
}
