#pragma once
#include "GrimDawnHook.h"

class AttackTargetHook : public GrimDawnHook
{
private:
	typedef int* (__thiscall* AttackTargetFunctionPtr)
		(void* This, unsigned int uint, int* Entity, int* ParametersCombat, bool b, unsigned int uint2, unsigned int uint3, unsigned int uint4);

	static AttackTargetFunctionPtr oAttackTargetFunc;
	
	static void* __fastcall FunctionHook(
		void* This, unsigned int uint, int* Entity, int* ParametersCombat, bool b, unsigned int uint2, unsigned int uint3, unsigned int uint4);

public:
	AttackTargetHook() = default;
	void EnableHook();
	void DisableHook();
};

