#pragma once

#include <string>
#include <mutex>
#include <queue>
#include <condition_variable>
#include "GameEventMessage.h"

class GrimDawnHook
{
protected:
	static std::string GetLastErrorAsString();
	

public:
	GrimDawnHook() = default;
	virtual void EnableHook();
	virtual void DisableHook();
	static std::mutex mQueue;
	static std::condition_variable condition;
};
