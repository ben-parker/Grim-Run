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
void SendGrimRunMessage(GameEventMessage msg);
HANDLE OpenProcessByName(LPCTSTR Name, DWORD dwAccess);

HANDLE g_hWorkerThread;
HANDLE g_hEvent;
HANDLE g_hPipe;
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
    
    // Connect to named pipe to communicate back to Grim Run
    g_hPipe = CreateFile(TEXT("\\\\.\\pipe\\GrimRunPipe"),
        GENERIC_WRITE,
        0,
        NULL,
        OPEN_EXISTING,
        0,
        NULL);
    
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
    bool waitForDefenderMsg = false;
    GameEventMessage grm = {};
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
        std::cout << "Popped message " << static_cast<int>(msg.msgType) << std::endl;
        grm.msgType = msg.msgType;
        
        if (msg.msgType == GameEventType::apply_damage)
        {
            grm.damage = msg.damage;
        }
        else if (msg.msgType == GameEventType::damage_to_defender)
        {
            memcpy(&grm.data, msg.data, 8);
            memcpy(&grm.data2, msg.data2, 20);
            grm.data_len = 8;
            grm.data2_len = 20;
        }
        else if (msg.msgType == GameEventType::attacker_name)
        {
            std::cout << "Popped attacker name " << std::endl;
            std::string attackerName = std::string(msg.data);
            memcpy(&grm.data, msg.data, 100);
            grm.data_len = attackerName.length();
        }
        else if (msg.msgType == GameEventType::combat_type)
        {
            std::string combatType = std::string(msg.data);
            memcpy(&grm.data, msg.data, 100);
            grm.data_len = combatType.length();
        }

        SendGrimRunMessage(grm);
        /*else if (msg.msgType == GameEventType::attacker_id)
        {
            attackerId = std::string(msg.data);
        }
        else if (msg.msgType == GameEventType::defender_name)
        {
            defenderName = std::string(msg.data);
            defenderNameLen = defenderName.length();
        }
        else if (msg.msgType == GameEventType::end_combat)
        {
            combatEvent = false;
            waitForDefenderMsg = false;
        }*/

        // multiple ApplyDamage messages occur within one combat event
        
        // send message
        // if outside of a combat event (dot or environmental damage)
        // will just be damage amount and defender Id
    }
}

int ProcessAttach(HINSTANCE hModule) {
    //std::cout << "*** Process attached! ***" << std::endl;
        
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
    CloseHandle(g_hPipe);
    return TRUE;
}

void SendGrimRunMessage(GameEventMessage msg)
{
    DWORD bytesWritten;

    if (g_hPipe != INVALID_HANDLE_VALUE)
    {
        WriteFile(g_hPipe,
            &msg,
            sizeof(GameEventMessage),
            &bytesWritten,
            NULL);
    }
    else
    {
        std::cout << "Invalid handle to pipe" << std::endl;
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