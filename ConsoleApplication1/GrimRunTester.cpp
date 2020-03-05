// dllmain.cpp : Defines the entry point for the DLL application.

#include <string>
#include <vector>
#include <thread>
#include <iostream>
#include <Windows.h>
#include <tlhelp32.h>
#include <atlstr.h>

using std::string;

bool testing = false;
void worker_thread();
bool InjectDLL(HANDLE h_process, string dllName);
HANDLE OpenProcessByName(LPCTSTR Name, DWORD dwAccess);
string GetLastErrorAsString();

struct DataRec
{
    char Text[255];
};

int main() {

    HWND h_targetWnd = FindWindow(L"WindowsForms10.Window.8.app.0.141b42a_r9_ad1", nullptr);

     //if(h_targetWnd && h_targetWnd > 0) {
     //    MessageBox(NULL, L"good", L"testing", NULL);
     //}
     //else {
     //    MessageBox(NULL, L"no window", L"testing", NULL);
     //}
    if (h_targetWnd) {
        COPYDATASTRUCT cds;
        DataRec data;

        string str = "this is a test from DLL!";
        size_t size =
            str.size() < 255 ? str.size() : 255;
        str.copy(data.Text, size);

        cds.dwData = 1; // can be anything, identifies the data type.  should be structured?
        cds.cbData = size; // size of whole struct?  does it matter?
        cds.lpData = &data; // ptr to payload to send

        SendMessage(h_targetWnd, WM_COPYDATA, 0, (LPARAM)&cds);
    }

    string dllPath = "D:\\Projects\\Grim Run\\x64\\Debug\\grhook.dll";
    // test to make sure the dll exists
    struct stat buffer;
    if (stat(dllPath.c_str(), &buffer) != 0)
    {
        MessageBox(NULL, L"DLL file not found!", L"Error", NULL);
        return 0;
    }

    CString processName;
    if (testing)
    {
        processName = "MockGrimDawn.exe";
    }
    else
    {
        processName = "Grim Dawn.exe";
    }
    LPCTSTR processNameLPCT = processName;
    HANDLE h_mockGD = OpenProcessByName(processNameLPCT, PROCESS_CREATE_THREAD | PROCESS_QUERY_INFORMATION | PROCESS_VM_OPERATION |
                                                            PROCESS_VM_WRITE | PROCESS_VM_READ);
    DWORD processId = GetProcessId(h_mockGD);
    
    bool injectionSuccess = InjectDLL(h_mockGD, dllPath);
    
    if (!injectionSuccess || h_mockGD == INVALID_HANDLE_VALUE)
    {
        MessageBox(NULL, L"Injection problem!", L"testing", NULL);
    }
    
    return 0;
}

void worker_thread() {
    std::cout << "thread function Executing" << std::endl;

    
     
    // hook methods

    // connect to pipe

    // when hooked methods are called, my hook will create a data object to send down the pipe
    // send it down the pipe
}

bool InjectDLL(HANDLE h_process, string dllName)
{
    LPVOID remoteString, loadLibAddress;
    const char* dll = dllName.c_str();
    SIZE_T dwSize = (dllName.size() + 1);

    if (h_process == INVALID_HANDLE_VALUE)
    {
        MessageBox(NULL, L"failed to inject", L"Loader", NULL);
        return false;
    }

    loadLibAddress = static_cast<LPVOID>(GetProcAddress(GetModuleHandle(L"kernel32.dll"), "LoadLibraryA"));
    remoteString = static_cast<LPVOID>(VirtualAllocEx(h_process, 0, dwSize, MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE));

    if (remoteString == 0)
    {
        std::cout << GetLastErrorAsString() << std::endl;
    }

    int wpmResult = 0;
    wpmResult = WriteProcessMemory(
        h_process, 
        static_cast<LPVOID>(remoteString), 
        dll, 
        strlen(dll),
        NULL);

    if (wpmResult == 0)
    {
        std::cout << GetLastErrorAsString() << std::endl;
        return false;
    }
    

    CreateRemoteThread(h_process, 
                        NULL, 
                        NULL, 
                        static_cast<LPTHREAD_START_ROUTINE>(loadLibAddress), 
                        static_cast<LPVOID>(remoteString), 
                        NULL, 
                        NULL);

    CloseHandle(h_process);
    return true;
}

HANDLE OpenProcessByName(LPCTSTR Name, DWORD dwAccess)
{
    HANDLE hSnap = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
    if (hSnap != INVALID_HANDLE_VALUE)
    {
        PROCESSENTRY32 pe;
        ZeroMemory(&pe, sizeof(PROCESSENTRY32));
        pe.dwSize = sizeof(PROCESSENTRY32);
        Process32First(hSnap, &pe);
        do
        {
            if (!lstrcmpi(pe.szExeFile, Name))
            {
                return OpenProcess(dwAccess, 0, pe.th32ProcessID);
            }
        } while (Process32Next(hSnap, &pe));

    }
    return INVALID_HANDLE_VALUE;
}

string GetLastErrorAsString()
{
    //Get the error message, if any.
    DWORD errorMessageID = ::GetLastError();
    if (errorMessageID == 0)
        return std::string(); //No error message has been recorded

    LPSTR messageBuffer = nullptr;
    size_t size = FormatMessageA(FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
        NULL, errorMessageID, MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), (LPSTR)&messageBuffer, 0, NULL);

    std::string message(messageBuffer, size);

    //Free the buffer.
    LocalFree(messageBuffer);

    return message;
}