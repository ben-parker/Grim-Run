#include "pch.h"
#include "LoggerHook.h"
#include <detours.h>
#include <iostream>
#include <iomanip>
//#include <intrin.h>

using std::cout;
using std::endl;
using std::string;

LoggerHook::LoggerFunctionPtr LoggerHook::oLoggerFunc;

LoggerHook::LoggerHook()
{
	
}

void LoggerHook::EnableHook()
{
	oLoggerFunc = reinterpret_cast<LoggerFunctionPtr>(GetProcAddress(
		::GetModuleHandle(L"Engine.dll"),
		"?Log@Engine@GAME@@UEBAXW4LogPriority@2@IPEBDZZ"));
	
	float f = 5.5f;
	double d = 5.5;
	int i = 5;
	char c = { 'e' };
	// ?AttackTarget@Character@GAME@@UAE_NIAAVEntity@2@AAVParametersCombat@2@_NIII@Z
	printf("Size of void: %d\n", sizeof(void*));
	printf("Size of int: %d\n", sizeof(i));
	printf("Size of int*: %d\n", sizeof(int*));
	printf("Size of float: %d\n", sizeof(f));
	printf("Size of float*: %d\n", sizeof(float*));
	printf("Size of double: %d\n", sizeof(d));
	printf("Size of double*: %d\n", sizeof(double*));
	printf("Size of char: %d\n", sizeof(c));
	printf("Size of char*: %d\n", sizeof(char*));

	cout << "\nOriginal Log Function at " << oLoggerFunc << endl;

	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourAttach((PVOID*)&oLoggerFunc, FunctionHook);
	DetourTransactionCommit();
}

void LoggerHook::DisableHook()
{
	DetourTransactionBegin();
	DetourUpdateThread(GetCurrentThread());
	DetourDetach((PVOID*)&oLoggerFunc, FunctionHook);
	DetourTransactionCommit();
}

bool startsWith(const char* prefix, char* str) {
	size_t lenpre = strlen(prefix), lenstr = strlen(str);
	return lenstr < lenpre ? false : strncmp(prefix, str, lenpre) == 0;
}

// double
static void swap8(void* v)
{
	char    in[8], out[8];
	memcpy(in, v, 8);
	out[0] = in[7];
	out[1] = in[6];
	out[2] = in[5];
	out[3] = in[4];
	out[4] = in[3];
	out[5] = in[2];
	out[6] = in[1];
	out[7] = in[0];
	memcpy(v, out, 8);
}

// int and float
static void swap4(void* v)
{
	char    in[4], out[4];
	memcpy(in, v, 4);
	out[0] = in[3];
	out[1] = in[2];
	out[2] = in[1];
	out[3] = in[0];
	memcpy(v, out, 4);
}

struct TotalDamage
{
	float absolute;
	float dot;
};

const char* attackerNameMsg = "    attackerName = %s";
const char* attackerIdMsg = "    attackerID = %d";
const char* defenderNameMsg = "    defenderName = %s";
const char* defenderIdMsg = "    defenderID = %d";
const char* pthOffensiveAbilityMsg = "    PTH Offensive Ability %f";
const char* pthDefensiveAbilityMsg = "    PTH Defensive Ability %f";
const char* pthMsg = "    PTH %f";
const char* pthRandomNumberMsg = "    PTH Random Number %f";
const char* pthRandomValueMsg = "    PTH %f, Rand Value %f";
const char* protectionAbsorbMsg = "    protectionAbsorption = %f";
const char* damageToDefenderMsg = "^y    Damage %f to Defender 0x%x (%s)";
const char* totalDamageMsg = "    Total Damage:  Absolute (%f), Over Time (%f)";
const char* regionHitMsg = "    regionHit = %s";
const char* combatTypeMsg = "    combatType = ";

void PrintBytesAtAddress(const HANDLE& hProcess, void* address, size_t len, uint8_t* result)
{
	SIZE_T bytesRead = 0;
	if (ReadProcessMemory(hProcess, address, result, len, &bytesRead))
	{
		cout << "[";
		cout << std::hex;
		for (size_t i = 0; i < len; i++)
		{
			//cout << static_cast<unsigned int>(result[i]) << " ";
			cout << std::setfill('0') << std::setw(2) << 
				static_cast<unsigned int>(result[i]) << " ";
		}
		cout << std::dec;
		cout << "]" << endl;
	}
	else
	{
		cout << "Only partial read: " << bytesRead << " bytes" << endl;
	}
}

// void GAME::Engine::Log(enum GAME::LogPriority, unsigned int, char const*, ...)
void __cdecl LoggerHook::FunctionHook(
	void* This,
	int priority,
	unsigned int dummyint,
	char* str,
	void* _param0, // maybe each of these can only be a certain type?  so this is string
	void* _param1, // maybe this is double?
	void* _param2,
	void* _param3,
	void* _param4, // defender address?
	void* _param5)
{
	HANDLE hCurProcess = GetCurrentProcess();
	HANDLE hProcess = OpenProcess(
		PROCESS_CREATE_THREAD |
		PROCESS_QUERY_INFORMATION |
		PROCESS_VM_OPERATION |
		PROCESS_VM_WRITE |
		PROCESS_VM_READ,
		true,
		GetProcessId(hCurProcess));

	if (startsWith(attackerNameMsg, str))
	{
		//cout << str;
		// %s param0
		std::string name = std::string((char*)_param0);
		cout << endl << "Begin Attack" << endl
			<< "    Attacker name : " << name << endl;
	}
	else if (startsWith(attackerIdMsg, str))
	{
		//cout << str;
		cout << "    Attacker ID 0x" << _param0 << endl;
	}
	else if (startsWith(defenderNameMsg, str))
	{
		// %s param0
		//cout << str;

		std::string name = std::string((char*)_param0);
		std::cout << "    Defender name: " << name << std::endl;
	}
	//else if (startsWith(defenderIdMsg, str))
	//{
	//	// last 2 bytes of pointer are same across messages
	//	cout << str;
	//	cout << "      ** Defender ID 0x" << _param0 << endl;
	//	cout << _param0 << " "
	//		<< _param1 << " " << _param2 << " "
	//		<< _param3 << " " << _param4 << " " << _param5 << endl;
	//}
	else if (startsWith(damageToDefenderMsg, str))
	{
		std::string dmgType = std::string((char*)_param2);
		cout << "      Damage to defender 0x" << _param1 << 
			" (" << dmgType << ")" << endl;
	}
	//else if (startsWith(totalDamageMsg, str))
	//{
	//	// gotta be 1 and 2
	//	// param4 is defender ID
	//	cout << "Total damage " << _param0 << " "
	//		<< _param1 << " " << _param2 << " "
	//		<< _param3 << " " << _param4 << " " << _param5 << endl;
	//}
	//else if (startsWith(pthOffensiveAbilityMsg, str))
	//{
	//	//printf("      ** %f\n", *static_cast<float*>(_param1
	//	//cout << *(float*)_param1 << endl;
	//}
	//else if (startsWith(pthDefensiveAbilityMsg, str))
	//{
	//	/*string pth = string((char*)_param0);
	//	cout << "      ** " << pth << endl;*/
	//	printf("\n    %016x %016x %016x %016x %016x %016x\n",
	//		_param0, _param1, _param2, _param3, _param4, _param5);
	//}
	//else if (startsWith(pthRandomValueMsg, str))
	//{
	//	cout << _param0 << " " <<
	//		_param1 << endl;
	//}
	//else if (startsWith(pthMsg, str))
	//{
	//	const size_t size = 8;
	//	uint8_t result[size] = { 0 };
	//	PrintBytesAtAddress(hProcess, &_param0, size, result);
	//}
	/*else if (startsWith(regionHitMsg, str))
	{
		cout << str;

		string regionHit = string((char*)_param0);
		cout << "      ** " << regionHit << endl;
	}*/
	else if (startsWith(combatTypeMsg, str))
	{
		cout << str;
	}
	
}
