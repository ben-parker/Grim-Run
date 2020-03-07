#pragma once
struct GrimRunMessage
{
	char AttackerId[8];
	char AttackerName[100];
	char DefenderId[8];
	char DefenderName[100];
	char CombatType[50];
	float Damage;
	char DamageType[20];
};

