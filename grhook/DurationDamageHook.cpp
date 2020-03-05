#include "pch.h"
#include "DurationDamageHook.h"
#include <detours.h>
#include <iostream>

DurationDamageHook::DurationDamageFunctionPtr DurationDamageHook::oDurationDamageFunc;

void DurationDamageHook::EnableHook()
{
	// float GAME::DurationDamageManager::ExecuteDamage(float &)
	//oDurationDamageFunc = reinterpret_cast<DurationDamageFunctionPtr>(GetProcAddress(
	//	::GetModuleHandle(L"Game.dll"),
	//	"?ExecuteDamage@DurationDamageManager@GAME@@IEAAMAEAM@Z"));
	// void GAME::DurationDamageManager::AddDamage(enum GAME::CombatAttributeType,float,float,unsigned int,unsigned int,float)
	oDurationDamageFunc = reinterpret_cast<DurationDamageFunctionPtr>(GetProcAddress(
		::GetModuleHandle(L"Game.dll"),
		"?AddDamage@DurationDamageManager@GAME@@UEAAXW4CombatAttributeType@2@MMIIM@Z"));


	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourAttach((PVOID*)&oDurationDamageFunc, FunctionHook);
	DetourTransactionCommit();
}

void DurationDamageHook::DisableHook()
{
	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourDetach((PVOID*)&oDurationDamageFunc, FunctionHook);
	DetourTransactionCommit();
}

// Character is the same Entity that was in Attack Target, i.e. the entity that will take the damage
void* __fastcall DurationDamageHook::FunctionHook(void* This, int c, float f, float f2, unsigned int i, unsigned int i2, float f3)
{
	std::cout << "  DurationDamageExecute " << f << std::endl;

	void* v = oDurationDamageFunc(This, c, f, f2, i, i2, f3);
	return v;
}

