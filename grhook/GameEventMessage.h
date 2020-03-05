#pragma once
#include "GameEventType.h"

class GameEventMessage
{
public:
	GameEventMessage();
	GameEventMessage(GameEventType type);

	GameEventType msgType;
};

