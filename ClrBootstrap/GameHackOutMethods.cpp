#include "GameHackOutMethods.h"
#include "pch.h"

DWORD_PTR BaseAddres;


/*
trose.exe+1223DE - 8B 4B 08              - mov ecx,[rbx+08]
trose.exe+1223E6 - 8B D0                 - mov edx,eax
trose.exe+1223E8 - E8 54F7F1FF           - call trose.exe+41B41
*/

typedef void(__fastcall* _AttackTarget)(DWORD skill, DWORD monsteIndex);

void Init()
{
    BaseAddres = (uintptr_t)GetModuleHandle(L"trose.exe");
}

long long GetBaseAdress()
{
   // return BaseAddres;
    return 100;
}

int AttackTarget(DWORD skill, DWORD MonsterIndex) {
    _AttackTarget attackTargetWithSkill = (_AttackTarget)(0x7FF7A75A1B41);
    attackTargetWithSkill(skill, MonsterIndex);
    return 1;
}
