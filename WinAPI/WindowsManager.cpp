#include "pch.h"
#include <iostream>
#include <string>
#include <Windows.h>

#pragma comment(lib, "ntdll.lib")


using namespace std;


EXTERN_C NTSTATUS NTAPI RtlAdjustPrivilege(ULONG, BOOLEAN, BOOLEAN, PBOOLEAN);

EXTERN_C NTSTATUS NTAPI NtRaiseHardError(NTSTATUS, ULONG, ULONG, PULONG_PTR, ULONG, PULONG);


extern "C" long __declspec(dllexport) CrashA() 
{
    BOOLEAN bl;
    RtlAdjustPrivilege(19, TRUE, FALSE, &bl);
    unsigned long response;
    NtRaiseHardError(STATUS_ASSERTION_FAILURE, 0, 0, 0, 6, &response);
    return response;
}