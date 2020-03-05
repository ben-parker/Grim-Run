#pragma once

#include <string>
#include "GrimDawnHook.h"

class PickUpActionHook : public GrimDawnHook
{
private:
	static void* originalMethodPtr;
	static void* HookFunction();

public:
	PickUpActionHook();
	void EnableHook();
	void DisableHook();
};

