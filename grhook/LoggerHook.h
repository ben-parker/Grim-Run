#pragma once
#include "GrimDawnHook.h"
class LoggerHook : public GrimDawnHook
{
private:
	typedef void (__cdecl* LoggerFunctionPtr)
		(void* This, void* priority, unsigned int dummyint, char* str, ...);

	static LoggerFunctionPtr oLoggerFunc;

	//  void GAME::Engine::Log(enum GAME::LogPriority, unsigned int, char const*, ...)
	static void __cdecl FunctionHook(
		void* This,
		int priority,
		unsigned int dummyint,
		char* str,
		void* _param0,
		void* _param1,
		void* _param2,
		void* _param3,
		void* _param4,
		void* _param5
	);

	static HANDLE hProcess;
	static std::queue<GameEventMessage>* msgQueue;

public:
	LoggerHook();
	LoggerHook(std::queue<GameEventMessage>* q);
	void EnableHook();
	void DisableHook();
};

