#pragma once
#include "GrimDawnHook.h"

class RequestMoveActionHook : public GrimDawnHook
{
private:
	static void* originalMethodPtr;
	static void* HookFunction();

	struct Vec3f {
		float x, y, z, u;
	};

public:
	RequestMoveActionHook() = default;
	void EnableHook();
	void DisableHook();
};

