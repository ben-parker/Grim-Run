#pragma once
#include "GrimDawnHook.h"
class DurationDamageHook : public GrimDawnHook
{
private:
	typedef int* (__thiscall* DurationDamageFunctionPtr)(void* This, int c, float f, float f2, unsigned int i, unsigned int i2, float f3);
	
	static DurationDamageFunctionPtr oDurationDamageFunc;

	static void* __fastcall FunctionHook(void* This, int c, float f, float f2, unsigned int i, unsigned int i2, float f3);

public:
	DurationDamageHook() = default;
	void EnableHook();
	void DisableHook();
};

