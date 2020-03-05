#pragma once

#include <string>

class GrimDawnHook
{
protected:
	static std::string GetLastErrorAsString();

public:
	GrimDawnHook() = default;
	virtual void EnableHook();
	virtual void DisableHook();
};
