#include "pch.h"
#include "ApplyDamageHook.h"
#include <stdio.h>
#include <stdlib.h>
#include <detours.h>
#include <iostream>
//#include <boost/stacktrace.hpp>

//HANDLE ApplyDamageHook::m_hEvent;
ApplyDamageHook::ApplyDamageFunctionPtr ApplyDamageHook::oApplyDamageFunc;
int CharacterApplyDamage = 10101012;

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
	//const int buffSize = sizeof(float) + sizeof(int) + sizeof(int);
	//unsigned char* buffer[buffSize] = {};
	//
	float damageDone = f;
	int* playStatsDamageType = PlayStatsDamageType;
	int playStatsDamageTypeValue = static_cast<int>(*playStatsDamageType);
	int damageType = CombatAttributeType;

	////void* damageAddress = (void*)&f;
	//// float, struct, enum, vector<int>

	// PlayStatsDamageType seems to be 4 at the end of a damage "message"
	// e.g. Cadence elemental damage component
	// Transmuter damage component
	// end of message
	if (playStatsDamageTypeValue != 4)
	{
		std::cout << "ApplyDamage amount " << f << 
			" type " << damageType << std::endl;
	}
	else
	{
		std::cout << "End attack" << std::endl;
	}
	
	// before returning, create data object to send to Grim Run
	// can probably just be the byte array since it has to be Marshalled anyway?
	// maybe using Pipes would be different, tbd

	void* v = oApplyDamageFunc(This, f, PlayStatsDamageType, CombatAttributeType, Vector);
	return v;
}
