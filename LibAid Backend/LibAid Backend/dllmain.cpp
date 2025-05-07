/*
* FILE		    : dllmain.cpp
* PROJECT	    : LibAid v3.0.12
* PROGRAMMER	: Nicholas Reilly
* FIRST VERSION	: 2025-03-27
* DESCRIPTION	: File that has the main entry point for the DLL application.
* REFERENCING   : Deitel, P., & Deitel, H. (2016). How to Program in C and C++ (8th ed.). Deitel & Associates Inc.
*/

// dllmain.cpp : Defines the entry point for the DLL application.
#include <Windows.h>


// FUNCTION   : DllMain
// DESCRIPTION: Entry point for the DLL application
// PARAMETERS : Handle to the DLL module, Reason for calling, Reserved
// RETURNS    : TRUE if successful, FALSE otherwise
BOOL APIENTRY DllMain(HMODULE hModule,
    DWORD  ul_reason_for_call,
    LPVOID lpReserved
)
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}

