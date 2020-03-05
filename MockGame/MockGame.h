// The following ifdef block is the standard way of creating macros which make exporting
// from a DLL simpler. All files within this DLL are compiled with the MOCKGAME_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see
// MOCKGAME_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#include <string>

#ifdef MOCKGAME_EXPORTS
#define MOCKGAME_API __declspec(dllexport)
#else
#define MOCKGAME_API __declspec(dllimport)
#endif

// This class is exported from the dll
class MOCKGAME_API CMockGame {
public:
	CMockGame(void);
	// TODO: add your methods here.
	bool ApplyDamage(std::string msg);
};