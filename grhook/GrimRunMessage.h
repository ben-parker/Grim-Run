#pragma once
struct GrimRunMessage
{
	float Damage;
	int AttackerNameLen;
	int DefenderNameLen;
	char AttackerId[8];
	char AttackerName[100];
	char DefenderId[8];
	char DefenderName[100];
	char CombatType[50];
	char DamageType[20];
};

