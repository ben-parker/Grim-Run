#include "pch.h"
#include "PauseGameHook.h"
#include <detours.h>
#include <iostream>

PauseGameHook::OriginalMethodPtr PauseGameHook::originalMethod;

void PauseGameHook::EnableHook()
{
	originalMethod =
		(OriginalMethodPtr)GetProcAddress(::GetModuleHandle(L"Engine.dll"), "?PauseGameTime@GAME@@YAXXZ");

	//MemberMethodPtr hookFunctionPtr = PauseGameHook::HookFunction;

	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourAttach((PVOID*)&originalMethod, HookFunction);
	DetourTransactionCommit();
}

void PauseGameHook::DisableHook()
{
	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourDetach((PVOID*)&originalMethod, HookFunction);
	DetourTransactionCommit();
}

void* PauseGameHook::HookFunction(void* This)
{
	std::cout << "Game paused 1" << std::endl;

	void* v = originalMethod(This);
	return v;
}