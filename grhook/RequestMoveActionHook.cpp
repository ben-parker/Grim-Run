#include "pch.h"
#include "RequestMoveActionHook.h"
#include <iostream>
#include <string>
#include <detours.h>

void* RequestMoveActionHook::originalMethodPtr;

// RequestMoveAction(bool,bool,class GAME::WorldVec3 const &)
//void* RequestMoveActionHook::HookFunction(bool a, bool b, Vec3f const& xyz)
//{
//	void* v = originalMethodPtr;
//	return v;
//}

void RequestMoveActionHook::EnableHook()
{
	originalMethodPtr =
		(void*)GetProcAddress(::GetModuleHandle(L"Game.dll"), "?Finish@PickUpAction@GAME@@UAEXXZ");

	std::cout << "ptr value is " << originalMethodPtr << std::endl;
	//MemberMethodPtr m = &TestHook::HookFunction;

	std::cout << HookFunction << std::endl;
	//std::cout << m << std::endl;

	if (originalMethodPtr > 0)
	{

		DetourTransactionBegin();
		DetourUpdateThread(GetCurrentThread());
		//DetourAttach((PVOID*)&originalMethodPtr, (PVOID)m);
		DetourAttach((PVOID*)&originalMethodPtr, (PVOID)HookFunction);

		if (DetourTransactionCommit() == NO_ERROR)
		{
			std::cout << "Successfully hooked PickUpAction" << std::endl;
		}
		else
		{
			std::cout << "Error hooking PickUpAction" << std::endl;

		}
	}
}

void RequestMoveActionHook::DisableHook()
{
	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourDetach((PVOID*)&originalMethodPtr, (PVOID)HookFunction);
	DetourTransactionCommit();
}
