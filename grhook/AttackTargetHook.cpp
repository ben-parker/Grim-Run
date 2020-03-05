#include "pch.h"
#include "AttackTargetHook.h"
#include <stdio.h>
#include <stdlib.h>
#include <detours.h>
#include <iostream>

using std::cout;
using std::endl;

AttackTargetHook::AttackTargetFunctionPtr AttackTargetHook::oAttackTargetFunc;

void AttackTargetHook::EnableHook()
{
	oAttackTargetFunc = reinterpret_cast<AttackTargetFunctionPtr>(GetProcAddress(
		::GetModuleHandle(L"Game.dll"),
		"?AttackTarget@Character@GAME@@UEAA_NIAEAVEntity@2@AEAVParametersCombat@2@_NIII@Z"));

	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourAttach((PVOID*)&oAttackTargetFunc, FunctionHook);
	DetourTransactionCommit();
}

void AttackTargetHook::DisableHook()
{
	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourDetach((PVOID*)&oAttackTargetFunc, FunctionHook);
	DetourTransactionCommit();
}
// bool GAME::Character::AttackTarget(unsigned int,class GAME::Entity &,class GAME::ParametersCombat &,bool,unsigned int,unsigned int,unsigned int)
/*
uint(1) seems to be a skill id, but different for different entities
	24411 when i forcewave 
	24397 when default attack
	24402 when cadence procs
	252101 attack from flamer

Entity address is a unique entity I think
	Entity value is a type of enemy, not a specific one

uint2 is weapon type?  or specific weapon ID
	0 when using a spell

uint4 is populated when enemy ability is used on me?
	24206 when Deepmire Shaman attacks with spell
*/

void* __fastcall AttackTargetHook::FunctionHook(
	void* This,
	unsigned int uint, 
	int* Entity, 
	int* ParametersCombat, 
	bool b, 
	unsigned int uint2, 
	unsigned int uint3, 
	unsigned int uint4)
{
	cout << endl << "AttackTarget 0x" << Entity << " value " << *Entity << endl;
	cout << uint << " " << ParametersCombat << " " << b << " " << uint2 << " " << uint3 << " " << uint4 << endl;

	void* v = oAttackTargetFunc(This, uint, Entity, ParametersCombat, b, uint2, uint3, uint4);
	return v;
}