#include "Game.h"

#include <string>
#include <iostream>
#include <windows.h>

bool Game::ApplyDamage(std::string msg)
{
	std::cout << "ApplyDamage was called with message: " + msg << std::endl;
	return true;
}
