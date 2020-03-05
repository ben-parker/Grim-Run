#include "pch.h"
#include "UnpauseGameHook.h"
#include <detours.h>
#include <iostream>

UnpauseGameHook::OriginalMethodPtr UnpauseGameHook::originalMethod;

void UnpauseGameHook::EnableHook()
{
	originalMethod = 
		(OriginalMethodPtr)GetProcAddress(::GetModuleHandle(L"Engine.dll"), "?UnpauseGameTime@GAME@@YAXXZ");

	//MemberMethodPtr hookFunctionPtr = PauseGameHook::HookFunction;
	
	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourAttach((PVOID*)&originalMethod, HookFunction);
	DetourTransactionCommit();
}

void UnpauseGameHook::DisableHook()
{
	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourDetach((PVOID*)&originalMethod, HookFunction);
	DetourTransactionCommit();	
}

void* UnpauseGameHook::HookFunction(void* This)
{
	std::cout << "Game unpaused 1" << std::endl;

	void* v = originalMethod(This);
	return v;
}