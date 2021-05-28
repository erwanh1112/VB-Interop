// The following ifdef block is the standard way of creating macros which make exporting
// from a DLL simpler. All files within this DLL are compiled with the SERVERINTEROP_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see
// SERVERINTEROP_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef SERVERINTEROP_EXPORTS
#define SERVERINTEROP_API __declspec(dllexport)
#else
#define SERVERINTEROP_API __declspec(dllimport)
#endif

extern "C"
{
    SERVERINTEROP_API bool GetFirstLogType(int* blob, char* key, int* keySize, char* value, int* valueSize);
    SERVERINTEROP_API bool GetNextLogType (int* blob, char* key, int* keySize, char* value, int* valueSize);
}