#include "pch.h"
#include "GameOutMethods.h"
#include <exception>
#include <typeinfo>
#include <string>



typedef void(__fastcall* _AttackTarget)(DWORD skill, DWORD monsteIndex);
typedef void(__fastcall* _PacketSend)(uintptr_t deviceAddr, char packet[]);

int AttackTarget(DWORD skill, DWORD monsterIndex) {
    _AttackTarget attackTargetWithSkill = (_AttackTarget)(GetBaseAdress() + (long long)0x0000041B41);

    attackTargetWithSkill(skill, monsterIndex);
    return 1;
}


DWORD casstFuncAddr;

void ActionCommand(int skill)
{
    /*
    ffxiv.exe + 56AB93 - 6A 00 - push 00
        ffxiv.exe + 56AB95 - 6A 00 - push 00
        ffxiv.exe + 56AB97 - 6A 00 - push 00
        ffxiv.exe + 56AB99 - 6A 00 - push 00
        ffxiv.exe + 56AB9B - E8 7090EBFF - call ffxiv.exe + 423C10
        ffxiv.exe + 56ABA0 - 52 - push edx
        ffxiv.exe + 56ABA1 - 50 - push eax
        ffxiv.exe + 56ABA2 - 56 - push esi
        ffxiv.exe + 56ABA3 - 6A 01 - push 01
        ffxiv.exe + 56ABA5 - B9 C0BD5702 - mov ecx, ffxiv.exe + 16BBDC0
        ffxiv.exe + 56ABAA - E8 91A62900 - call ffxiv.exe + 805240
        */

    DWORD thisAddr = GetBaseAdress() + 0x16BBDC0;
    casstFuncAddr = GetBaseAdress() + 0x805240;
    __asm
    {
        push 0
        push 0
        push 0
        push 0
        push 0
        push 0xE0000000
        push skill
        push 01
        mov ecx, thisAddr
        call casstFuncAddr
    }
}

long long GetBaseAdress()
{
    // return BaseAddres;
    return (uintptr_t)GetModuleHandle(L"ffxiv.exe");
}



long long GetInt64(uintptr_t adress)
{
    return *reinterpret_cast<long long*>(adress);
}


int GetInt32(uintptr_t adress)
{
    return *reinterpret_cast<int*>(adress);
}

float GetFloat(uintptr_t adress)
{
        return *reinterpret_cast<float*>(adress);
}


short GetShort(uintptr_t adress)
{
    return *reinterpret_cast<short*>(adress);
}


BYTE GetByte(uintptr_t adress)
{
    return *reinterpret_cast<BYTE*>(adress);
}

void GetByteArray(uintptr_t adress, char* outTable, int size)
{
    memcpy(outTable, reinterpret_cast<char*>(adress), size);
}



int SendPacketToServer(uintptr_t deviceAddr, char packet[])
{
    _PacketSend attackTargetWithSkill = (_PacketSend)(GetBaseAdress() + 0x268DC);

    attackTargetWithSkill(deviceAddr, packet);

    return 0; 
}