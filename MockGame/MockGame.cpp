// MockGame.cpp : Defines the exported functions for the DLL.
//

#include "pch.h"
#include "framework.h"
#include "MockGame.h"
#include <iostream>


// This is an example of an exported variable
MOCKGAME_API int nMockGame=0;

// This is an example of an exported function.
bool CMockGame::ApplyDamage(std::string msg)
{
    return true;
}

// This is the constructor of a class that has been exported.
CMockGame::CMockGame()
{
    return;
}
