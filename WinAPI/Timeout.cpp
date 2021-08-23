#include "pch.h"
#include "Timeout.h"

extern "C" __declspec(dllexport) bool SleepA(int ms) 
{
	Sleep(ms);
	return TRUE;
}