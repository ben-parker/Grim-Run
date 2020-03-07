#pragma once
#include "GameEventType.h"
#include <cstdint>

struct GameEventMessage
{
public:
	GameEventMessage() = default;

	GameEventType msgType;
	float damage;
	char data[100];
	char data2[100];
};

