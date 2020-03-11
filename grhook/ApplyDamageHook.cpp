#include "pch.h"
#include "ApplyDamageHook.h"
#include "GameEventMessage.h"
#include <stdio.h>
#include <stdlib.h>
#include <detours.h>
#include <iostream>
#include <mutex>
#include <queue>

//#include <boost/stacktrace.hpp>

ApplyDamageHook::ApplyDamageFunctionPtr ApplyDamageHook::oApplyDamageFunc;
std::queue<GameEventMessage>* ApplyDamageHook::msgQueue;

ApplyDamageHook::ApplyDamageHook() 
{
	msgQueue = nullptr;
}

ApplyDamageHook::ApplyDamageHook(std::queue<GameEventMessage>* q)
{
	msgQueue = q;
}

void ApplyDamageHook::EnableHook() 
{
	oApplyDamageFunc = reinterpret_cast<ApplyDamageFunctionPtr>(GetProcAddress(
		::GetModuleHandle(L"Game.dll"),
		"?ApplyDamage@CombatManager@GAME@@QEAA_NMAEBUPlayStatsDamageType@2@W4CombatAttributeType@2@AEBV?$vector@I@mem@@@Z"));


	// ?AttackTarget@Character@GAME@@UAE_NIAAVEntity@2@AAVParametersCombat@2@_NIII@Z
	std::cout << "original method pointer is " << oApplyDamageFunc << std::endl;
	
	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourAttach((PVOID*)&oApplyDamageFunc, FunctionHook);
	DetourTransactionCommit();
}

void ApplyDamageHook::DisableHook() 
{
	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourDetach((PVOID*)&oApplyDamageFunc, FunctionHook);
	DetourTransactionCommit();
}

// ApplyDamage(float,struct GAME::PlayStatsDamageType const &,enum GAME::CombatAttributeType,class mem::vector<unsigned int> const &)
// Notes:  Vector is ignored
// PlayStatsDamageType is related to physical/non-physical damage components of attack
// There are at least 2 pairs of values that mean the same thing, not used together
void* __fastcall ApplyDamageHook::FunctionHook(void* This, float f, int* PlayStatsDamageType, int CombatAttributeType, void* Vector) 
{
	float damageDone = f;
	int playStatsDamageTypeValue = static_cast<int>(*PlayStatsDamageType);
	int damageType = CombatAttributeType;
	GameEventMessage m;

	// PlayStatsDamageType seems to be 4 at the end of a damage "message"
	// e.g. Cadence elemental damage component
	// Transmuter damage component
	// end of message
	if (playStatsDamageTypeValue != 4)
	{
		std::cout << "    ApplyDamage amount " << f << 
			" type " << damageType << std::endl;

		m.msgType = GameEventType::apply_damage;
		m.damage = f;
	}
	else
	{
		std::cout << "End attack" << std::endl;
		m.msgType = GameEventType::end_combat;
	}

	if (msgQueue != nullptr)
	{
		{
			std::lock_guard<std::mutex> guard(GrimDawnHook::mQueue);
			msgQueue->push(m);
		}
		GrimDawnHook::condition.notify_one();
	}
	
	void* v = oApplyDamageFunc(This, f, PlayStatsDamageType, CombatAttributeType, Vector);
	return v;
}
