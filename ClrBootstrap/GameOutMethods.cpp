#include "pch.h"
#include "GameOutMethods.h"
#include <exception>
#include <typeinfo>
#include <string>

#ifdef _WIN64
    #define process L"ffxiv_dx11.exe"
#else
    #define process L"ffxiv.exe"
#endif



long long GetBaseAdress()
{
    // return BaseAddres;
    return (uintptr_t)GetModuleHandle(process);
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

