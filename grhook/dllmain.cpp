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
#include "GameEventMessage.h"
#include <vector>
#include <thread>
#include <iostream>
#include <queue>
#include <Windows.h>
#include <combaseapi.h>
#include <string.h>
#include <TlHelp32.h>
#include <mutex>
using std::string;
using std::thread;
using std::queue;
using std::vector;
using std::mutex;
using std::lock_guard;

int ProcessAttach(HINSTANCE hModule);
int ProcessDetach(HINSTANCE hModule);
DWORD WINAPI WorkerThread(HMODULE hModule);
void ListenerThread(queue<GameEventMessage>* q);
void SendGrimRunMessage(GrimRunMessage msg);
HANDLE OpenProcessByName(LPCTSTR Name, DWORD dwAccess);

HANDLE g_hWorkerThread;
HANDLE g_hEvent;
HWND g_hGrimRunWnd;
bool listen = true;

vector<GrimDawnHook*> hooks;

DWORD WINAPI WorkerThread(HMODULE hModule)
{
    queue<GameEventMessage> msgQueue;

    FreeConsole();
    AllocConsole();
    FILE* f;
    freopen_s(&f, "CONOUT$", "w", stdout);

    std::cout << "*** Worker Thread started ***" << std::endl;

    //hooks.push_back(new PauseGameHook());
    //hooks.push_back(new UnpauseGameHook());
    hooks.push_back(new ApplyDamageHook(&msgQueue));
    //hooks.push_back(new AttackTargetHook());
    //hooks.push_back(new ExecuteDamageHook());
    //hooks.push_back(new DurationDamageHook());
    hooks.push_back(new LoggerHook(&msgQueue));

    for (auto const& hook : hooks)
    {
        hook->EnableHook();
    }

    
    std::thread listenerThread(ListenerThread, &msgQueue);
    while (true)
    {
        if (GetKeyState(VK_END) & 0x8000)
            break;
    }

    listen = false;
    GrimDawnHook::condition.notify_one();
    listenerThread.join();
    
    fclose(f);
    FreeConsole();
    FreeLibraryAndExitThread(hModule, 0);
    
    ProcessDetach(hModule);
    
    return 0;
}

void ListenerThread(queue<GameEventMessage>* q)
{
    bool combatEvent = false;
    GrimRunMessage grm;
    string attackerName, attackerId, defenderName, defenderId;
    size_t attackerNameLen, defenderNameLen = 0;
 
    std::unique_lock<mutex> lock(GrimDawnHook::mQueue);

    while (listen)
    {
        while (q->empty() && listen)
        {
            GrimDawnHook::condition.wait(lock);
        }

        if (q->empty())
        {
            break;
        }
       
        auto msg = q->front();
        q->pop();

        if (msg.msgType == GameEventType::apply_damage)
        {
            grm.Damage = msg.damage;
        }
        else if (msg.msgType == GameEventType::attacker_name)
        {
            combatEvent = true;
            attackerName = std::string(msg.data);
            attackerNameLen = attackerName.length();
        }
        else if (msg.msgType == GameEventType::attacker_id)
        {
            attackerId = std::string(msg.data);
        }
        else if (msg.msgType == GameEventType::defender_name)
        {
            defenderName = std::string(msg.data);
            defenderNameLen = defenderName.length();
        }
        else if (msg.msgType == GameEventType::damage_to_defender)
        {
            memcpy(&grm.DefenderId, msg.data, 8);
            memcpy(&grm.DamageType, msg.data2, 20);
        }
        else if (msg.msgType == GameEventType::end_combat)
        {
            combatEvent = false;
        }

        // multiple ApplyDamage messages occur within one combat event
        if (combatEvent)
        {
            attackerName.copy(grm.AttackerName, attackerNameLen);
            attackerId.copy(grm.AttackerId, 4);
            defenderName.copy(grm.DefenderName, defenderNameLen);

            grm.AttackerNameLen = attackerNameLen;
            grm.DefenderNameLen = defenderNameLen;
        }
        
        // send message
        // if outside of a combat event (dot or environmental damage)
        // will just be damage amount and defender Id
        if (grm.Damage > 0)
        {
            SendGrimRunMessage(grm);
        }
    }
}

int ProcessAttach(HINSTANCE hModule) {
    //std::cout << "*** Process attached! ***" << std::endl;
    g_hGrimRunWnd = FindWindow(L"WindowsForms10.Window.8.app.0.141b42a_r8_ad1", NULL);
    
    // why does this crash the target process?
    // thread worker_thread(ListenerThread);
    g_hWorkerThread = CreateThread(nullptr, 0, (LPTHREAD_START_ROUTINE)WorkerThread, hModule, 0, nullptr);

    return true;
}

int ProcessDetach(HINSTANCE hModule) {
    // Signal that we are shutting down
    // This message is not at all guaranteed to get sent.
    
    for (auto const& hook : hooks)
    {
        hook->DisableHook();
    }

    CloseHandle(g_hWorkerThread);
    return TRUE;
}

void SendGrimRunMessage(GrimRunMessage msg)
{
    COPYDATASTRUCT cds;
    
    std::string str = "this is a test from DLL!";
    size_t msgSize = 
        str.size() < 255 
        ? str.size() 
        : 255;
    
    //str.copy(data.Text, msgSize);

    cds.dwData = 1; // can be anything, identifies the data type.  should be structured?
    cds.cbData = sizeof(msg); // size of whole struct?  does it matter?
    cds.lpData = &msg; // ptr to payload to send

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