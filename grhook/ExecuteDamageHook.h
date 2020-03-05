#pragma once
#include "GrimDawnHook.h"
class ExecuteDamageHook : public GrimDawnHook
{
private:
	typedef int* (__thiscall* ExecuteDamageFunctionPtr)
		(void* This, int* Character);

	static ExecuteDamageFunctionPtr oExecuteDamageFunc;

	static void* __fastcall FunctionHook
	(void* This, void* _, int* Character);

public:
	ExecuteDamageHook() = default;
	void EnableHook();
	void DisableHook();
};

