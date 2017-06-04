#include <Windows.h>
#include "Functions.h"
#include "Main.h"
#include <math.h>

static DWORD baseAddress = (DWORD)0x00400000;

bool Hook(void * toHook, void * ourFunction, int lenght)
{
	if (lenght < 5)
		return false;

	DWORD curProtectionFlag;
	VirtualProtect(toHook, lenght, PAGE_EXECUTE_READWRITE, &curProtectionFlag);
	memset(toHook, 0x90, lenght);
	DWORD relativeAddress = ((DWORD)ourFunction - (DWORD)toHook) - 5;

	*(BYTE*)toHook = 0xE9;
	*(DWORD*)((DWORD)toHook + 1) = relativeAddress;

	DWORD temp;
	VirtualProtect(toHook, lenght, curProtectionFlag, &temp);
	return true;
}

DWORD jmpVectorStoreRet;
DWORD jmpRetPressFire;
DWORD jmpRetStrLoading;


float vectorX;
float vectorY;
float vectorZ;
int isLoading;	//doesn't matter it's int, it's only for safety, because I use mov

DWORD jmpRetToFlipBuffer;

void __declspec(naked) vectorHack()
{
	//pc_matador.exe+198F97 - 83 EC 08              - sub esp,08 { 8 }
	//----------
	//pc_matador.exe+198F9A - D9 98 E4000000        - fstp dword ptr [eax+000000E4]
	//pc_matador.exe+198FA0 - D9 46 04              - fld dword ptr [esi+04]
	//pc_matador.exe+198FA3 - D9 98 E8000000        - fstp dword ptr [eax+000000E8]
	//pc_matador.exe+198FA9 - D9 46 08              - fld dword ptr [esi+08]
	//pc_matador.exe+198FAC - D9 98 EC000000        - fstp dword ptr [eax+000000EC]
	//----------
	//pc_matador.exe+198FB2 - D9 06                 - fld dword ptr [esi]
	//pc_matador.exe+198FB4 - DD 1C 24              - fstp qword ptr [esp]

	__asm
	{
		fst dword ptr[vectorX]
		fstp dword ptr[eax + 0x000000E4]
		fld dword ptr[esi + 0x04]
		fst dword ptr[vectorZ]
		fstp dword ptr[eax + 0x000000E8]
		fld dword ptr[esi + 0x08]
		fst dword ptr[vectorY]
		fstp dword ptr[eax + 0x000000EC]
		jmp[jmpVectorStoreRet]
	}
}

void __declspec(naked) storeFinishedLoading()
{
	//8 bytes
	//pc_matador.exe + 34393F - 8B C8 - mov ecx, eax
	//pc_matador.exe + 343941 - 8B 82 A4000000 - mov eax, [edx + 000000A4]
	//pc_matador.exe + 343947 - 68 94728800 - push pc_matador.exe + 487294 { ["STR_PRESS_FIRE"] }
	//pc_matador.exe + 34394C - FF D0 - call eax
	//pc_matador.exe + 34394E - C7 44 24 1C 01000000 - mov[esp + 1C], 00000001 { 1 }
	//pc_matador.exe + 343956 - EB 29 - jmp pc_matador.exe + 343981

	__asm
	{
		mov ecx, eax
		mov eax, [edx + 0x000000A4]
		mov [isLoading],0
		jmp[jmpRetPressFire]
	}
}

void __declspec(naked) storeIsLoading()
{
	//8 bytes
	//pc_matador.exe + 343972 - 8B C8 - mov ecx, eax
	//pc_matador.exe + 343974 - 8B 82 A4000000 - mov eax, [edx + 000000A4]
	//pc_matador.exe + 34397A - 68 88728800 - push pc_matador.exe + 487288 { ["STR_LOADING"] }
	//pc_matador.exe + 34397F - FF D0 - call eax

	__asm
	{
		mov ecx, eax
		mov eax, [edx + 0x000000A4]
		mov [isLoading],1
		jmp[jmpRetStrLoading]
	}
}

void __declspec(naked) storeWhateverThatis()
{
	//5 bytes
	//pc_matador.exe + 345250 - 8B 4C 24 18 - mov ecx, [esp + 18]
	//pc_matador.exe + 345254 - 51 - push ecx

	__asm
	{
		mov ecx, [esp + 0x18]
		push ecx
		mov [isLoading], 0
		jmp [jmpRetToFlipBuffer]
	}
}

DWORD WINAPI HookThread(LPVOID param)
{
	{
		int hookLenght = 24;
		DWORD hookAddress = baseAddress + 0x198F9A;
		jmpVectorStoreRet = hookAddress + hookLenght;
		Hook((void*)hookAddress, vectorHack, hookLenght);
	}

	{
		int hookLenght = 8;
		DWORD hookAddress = baseAddress + 0x34393F;
		jmpRetPressFire = hookAddress + hookLenght;
		Hook((void*)hookAddress, storeFinishedLoading, hookLenght);
	}

	{
		int hookLenght = 8;
		DWORD hookAddress = baseAddress + 0x343972;
		jmpRetStrLoading = hookAddress + hookLenght;
		Hook((void*)hookAddress, storeIsLoading, hookLenght);
	}

	//Probably flipBuffer
	{
		int hookLenght = 5;
		DWORD hookAddress = baseAddress + 0x345250;
		jmpRetToFlipBuffer = hookAddress + hookLenght;
		Hook((void*)hookAddress, storeWhateverThatis, hookLenght);
	}


	while (true)
	{
		Sleep(400);
	}
}

BOOL WINAPI DllMain(HINSTANCE hModule, DWORD dwReason, LPVOID lpReserved)
{
	//starts from here
	switch (dwReason)
	{
	case DLL_PROCESS_ATTACH:
		CreateThread(0, 0, HookThread, hModule, 0, 0);
		break;
	}

	return true;
}