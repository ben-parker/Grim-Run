// MockGrimDawn.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include "../MockGame/MockGame.h"

#include <iostream>
#include <random>
#include <chrono>
#include <thread>


int main()
{
    bool run = true;
    CMockGame game;
    std::string messages[] = { "earth", "wind", "fire" };

    std::random_device rd; // obtain a random number from hardware
    std::mt19937 eng(rd()); // seed the generator
    std::uniform_int_distribution<> distr(0, 2); // define the range


    while (run)
    {
        std::cout << "Calling ApplyDamage..." << std::endl;
        int idx = distr(eng);
        bool result = game.ApplyDamage(messages[idx]);

        std::this_thread::sleep_for(std::chrono::milliseconds(4000));
    }
}

// Run program: Ctrl + F5 or Debug > Start Without Debugging menu
// Debug program: F5 or Debug > Start Debugging menu

// Tips for Getting Started: 
//   1. Use the Solution Explorer window to add/manage files
//   2. Use the Team Explorer window to connect to source control
//   3. Use the Output window to see build output and other messages
//   4. Use the Error List window to view errors
//   5. Go to Project > Add New Item to create new code files, or Project > Add Existing Item to add existing code files to the project
//   6. In the future, to open this project again, go to File > Open > Project and select the .sln file
