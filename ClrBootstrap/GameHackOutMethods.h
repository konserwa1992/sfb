#pragma once
#include <Windows.h>

extern "C" {
	__declspec(dllexport) int AttackTarget(DWORD skill, DWORD MonsterIndex);
	__declspec(dllexport) long long GetBaseAdress();
}

typedef void(__fastcall* _AttackTarget)(DWORD skill, DWORD monsteIndex);
void Init();


