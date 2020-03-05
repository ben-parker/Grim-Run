#pragma once
#include "GrimDawnHook.h"

class UnpauseGameHook : public GrimDawnHook
{
private:
	typedef int* (__thiscall* OriginalMethodPtr)(void*);
	static OriginalMethodPtr originalMethod;
	//typedef void* (PauseGameHook::*MemberMethodPtr)(void* This, void* notUsed);  // Please do this!
	static void* __fastcall HookFunction(void* This);

public:
	UnpauseGameHook() = default;
	void EnableHook();
	void DisableHook();
};

