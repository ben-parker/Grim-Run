#pragma once
#include "GameEventType.h"
#include <cstdint>

struct GameEventMessage
{
public:
	GameEventMessage() = default;

	GameEventType msgType;
	float damage;
	int data_len;
	int data2_len;
	char bytes[4];
	char data[100];
	char data2[100];
};

