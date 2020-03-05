#pragma once
#include "GrimDawnHook.h"

class ApplyDamageHook : public GrimDawnHook {
private:
	typedef int* (__thiscall* ApplyDamageFunctionPtr)
		(void* This, float f, int* PlayStatsDamageType, int CombatAttributeType, void* Vector);
	
	static ApplyDamageFunctionPtr oApplyDamageFunc;

	static void* __fastcall FunctionHook
		(void* This, float f, int* PlayStatsDamageType, int CombatAttributeType, void* Vector);
	
public:
	ApplyDamageHook() = default;
	void EnableHook();
	void DisableHook();
};