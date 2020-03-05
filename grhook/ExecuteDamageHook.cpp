#include "pch.h"
#include "ExecuteDamageHook.h"
#include <detours.h>
#include <iostream>

ExecuteDamageHook::ExecuteDamageFunctionPtr ExecuteDamageHook::oExecuteDamageFunc;

void ExecuteDamageHook::EnableHook()
{
	oExecuteDamageFunc = reinterpret_cast<ExecuteDamageFunctionPtr>(GetProcAddress(
		::GetModuleHandle(L"Game.dll"),
		"?Execute@CombatAttributeAbsDamage@GAME@@UAEXAAVCharacter@2@@Z"));

	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourAttach((PVOID*)&oExecuteDamageFunc, FunctionHook);
	DetourTransactionCommit();
}

void ExecuteDamageHook::DisableHook()
{
	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourDetach((PVOID*)&oExecuteDamageFunc, FunctionHook);
	DetourTransactionCommit();
}

// Character is the same Entity that was in Attack Target, i.e. the entity that will take the damage
// seems to only be called when non-physical damage needs to be applied
void* __fastcall ExecuteDamageHook::FunctionHook(void* This, void* _, int* Character)
{
	std::cout << "ExecuteDamage on Character 0x" << Character << std::endl;

	void* v = oExecuteDamageFunc(This, Character);
	return v;
}

