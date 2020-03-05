#include "pch.h"
#include "PickUpActionHook.h"
#include <string>
#include <iostream>
#include <detours.h>

void* PickUpActionHook::originalMethodPtr;

PickUpActionHook::PickUpActionHook()
{

}



// bool GAME::CombatManager::ApplyDamage(float,struct GAME::PlayStatsDamageType const &,enum GAME::CombatAttributeType,class mem::vector<unsigned int> const &)
void* PickUpActionHook::HookFunction() {
	std::cout << "In PickUpActionHook!" << std::endl;
	
	void* v = originalMethodPtr;
	return v;
}
