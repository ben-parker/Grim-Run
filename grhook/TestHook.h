#pragma once
#include <string>

class TestHook
{
private:
	typedef bool (TestHook::* MemberMethodPtr)(std::string msg);  // Please do this!
	bool* originalMethodPtr;

public:
	TestHook();
	bool EnableHook();
	static bool HookFunction(std::string msg);
};

