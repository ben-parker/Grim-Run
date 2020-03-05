#include "pch.h"
#include <iostream>
#include <detours.h>
#pragma comment(lib, "detours.lib")
#include "TestHook.h"



TestHook::TestHook()
{
}

bool TestHook::EnableHook()
{
	originalMethodPtr = 
		(bool*)GetProcAddress(::GetModuleHandle(L"MockGame.dll"), "?ApplyDamage@CMockGame@@QAE_NV?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@@Z");

	std::cout << "ptr value is " << originalMethodPtr << std::endl;
	//MemberMethodPtr m = &TestHook::HookFunction;

	std::cout << originalMethodPtr << std::endl;
	//std::cout << m << std::endl;

	if (originalMethodPtr > 0)
	{
		
		DetourTransactionBegin();
		DetourUpdateThread(GetCurrentThread());
		//DetourAttach((PVOID*)&originalMethodPtr, (PVOID)m);
		DetourAttach((PVOID*)&originalMethodPtr, (PVOID)HookFunction);
		
		if (DetourTransactionCommit() == NO_ERROR)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	else 
	{
		return false;
	}
}

bool TestHook::HookFunction(std::string msg)
{
	std::cout << "*** Message from ApplyDamage is " << msg << std::endl;

	//bool* (*f)();
	//f = origMethodPtr;
	//f();

	//bool* b = origMethodPtr(msg);
	//return b;
	return true;
}