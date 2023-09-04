import subprocess

def main():
    print("Enter the Windows product key:")
    product_key = input()
    
    if activate_windows(product_key):
        print("Windows has been successfully activated!")
    else:
        print("Failed to activate Windows. Please check the product key and try again.")
    
    input("Press Enter to exit...")

def activate_windows(product_key):
    activation_command = f"slmgr.vbs /ipk {product_key}"
    
    try:
        subprocess.run(["cmd.exe", "/K", activation_command], shell=True, check=True)
        return True
    except subprocess.CalledProcessError:
        return False

if __name__ == "__main__":
    main()
