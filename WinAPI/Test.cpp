#include "pch.h"
#include <Windows.h>

extern "C" __declspec(dllexport) void Create()
{
	HWND hwnd = CreateWindowExW(0, L"TestClass", L"Test", 0, 0, 0, 1000, 400, NULL, NULL, NULL, NULL);
	ShowWindow(hwnd, 1);
}