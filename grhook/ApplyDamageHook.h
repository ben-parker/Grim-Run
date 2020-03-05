#pragma once
#include "GrimDawnHook.h"
#include "GameEventMessage.h"
#include <mutex>
#include <queue>

class ApplyDamageHook : public GrimDawnHook {
private:
	typedef int* (__thiscall* ApplyDamageFunctionPtr)
		(void* This, float f, int* PlayStatsDamageType, int CombatAttributeType, void* Vector);
	
	static ApplyDamageFunctionPtr oApplyDamageFunc;

	static void* __fastcall FunctionHook
		(void* This, float f, int* PlayStatsDamageType, int CombatAttributeType, void* Vector);

	static std::queue<GameEventMessage>* msgQueue;

public:
	ApplyDamageHook();
	ApplyDamageHook(std::queue<GameEventMessage>* q);
	void EnableHook();
	void DisableHook();
};
