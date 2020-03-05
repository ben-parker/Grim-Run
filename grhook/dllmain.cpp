// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
#include "GrimRunMessage.h"
#include "TestHook.h"
#include "PauseGameHook.h"
#include "UnpauseGameHook.h"
#include "GrimDawnHook.h"
#include "ApplyDamageHook.h"
#include "AttackTargetHook.h"
#include "ExecuteDamageHook.h"
#include "DurationDamageHook.h"
#include "LoggerHook.h"
#include <vector>
#include <thread>
#include <iostream>
#include <Windows.h>
#include <combaseapi.h>
#include <string.h>
#include <TlHelp32.h>
using std::thread;

int ProcessAttach(HINSTANCE hModule);
int ProcessDetach(HINSTANCE hModule);
DWORD WINAPI ListenerThread(HMODULE hModule);
void SendGrimRunMessage();
HANDLE OpenProcessByName(LPCTSTR Name, DWORD dwAccess);

bool testing = false;

std::vector<GrimDawnHook*> hooks;
//PauseGameHook pauseGameHook;
//UnpauseGameHook unpauseGameHook;
//ApplyDamageHook applyDamageHook;
//AttackTargetHook attackTargetHook;
//ExecuteDamageHook executeDamageHook;
//DurationDamageHook durationDamageHook;

HANDLE g_hWorkerThread;
HANDLE g_hEvent;
HWND g_hGrimRunWnd;

DWORD WINAPI ListenerThread(HMODULE hModule)
{
    FreeConsole();
    AllocConsole();
    FILE* f;
    freopen_s(&f, "CONOUT$", "w", stdout);
    
    std::cout << "*** Worker Thread started ***" << std::endl;

    // attach all hooks
    if (testing)
    {
        TestHook testHook;
        testHook.EnableHook();
    }
    else
    {
        //hooks.push_back(new PauseGameHook());
        //hooks.push_back(new UnpauseGameHook());
        hooks.push_back(new ApplyDamageHook());
        //hooks.push_back(new AttackTargetHook());
        //hooks.push_back(new ExecuteDamageHook());
        //hooks.push_back(new DurationDamageHook());
        hooks.push_back(new LoggerHook());

        for (auto const& hook : hooks)
        {
            hook->EnableHook();
        }
    }
    
    while (true)
    {
        if (GetKeyState(VK_END) & 0x8000)
            break;
    }

    fclose(f);
    FreeConsole();
    FreeLibraryAndExitThread(hModule, 0);
    
    ProcessDetach(hModule);
    
    return 0;
}

//std::vector<BaseMethodHook*> hooks;
int ProcessAttach(HINSTANCE hModule) {
    //g_hEvent = CreateEvent(NULL, FALSE, FALSE, "IA_Worker");
    //std::cout << "*** Process attached! ***" << std::endl;
    g_hGrimRunWnd = FindWindow(L"WindowsForms10.Window.8.app.0.141b42a_r10_ad1", NULL);

    if (g_hGrimRunWnd > 0)
    {
        SendGrimRunMessage();
    }
    
    // why does this crash the target process?
    //thread worker_thread(ListenerThread);
    g_hWorkerThread = CreateThread(nullptr, 0, (LPTHREAD_START_ROUTINE)ListenerThread, hModule, 0, nullptr);

    return true;
}

int ProcessDetach(HINSTANCE hModule) {
    // Signal that we are shutting down
    // This message is not at all guaranteed to get sent.
    if (!testing)
    {
        /*for (auto const& hook : hooks)
        {
            hook.DisableHook();
        }*/
        for (auto const& hook : hooks)
        {
            hook->DisableHook();
        }
    }

    CloseHandle(g_hWorkerThread);
    return TRUE;
}

void SendGrimRunMessage()
{
    GrimRunMessage data;
    COPYDATASTRUCT cds;
    
    std::string str = "this is a test from DLL!";
    size_t msgSize = str.size() < 255 ? str.size() : 255;
    
    str.copy(data.Text, msgSize);

    cds.dwData = 1; // can be anything, identifies the data type.  should be structured?
    cds.cbData = str.size(); // size of whole struct?  does it matter?
    cds.lpData = &data; // ptr to payload to send

    if (g_hGrimRunWnd > 0)
    {
        SendMessage(g_hGrimRunWnd, WM_COPYDATA, 0, (LPARAM)&cds);
    }
}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD ul_reason_for_call, LPVOID lpReserved)
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
        return ProcessAttach(hModule);
    case DLL_PROCESS_DETACH:
        return ProcessDetach(hModule);
    case DLL_THREAD_ATTACH:
        break;
    case DLL_THREAD_DETACH:
        break;
    }

    return TRUE;
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