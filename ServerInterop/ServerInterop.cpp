// ServerInterop.cpp : Defines the exported functions for the DLL.
//

#include "pch.h"
#include "framework.h"
#include "ServerInterop.h"
#include "../Server/Server.h"
#include <vector>

std::vector<std::pair<std::string, std::string>> _logTypes;

bool CopyLogType(int* blob, char* key, int* keySize, char* value, int* valueSize)
{
    // Check  hidden index -- i.e. blob -- is in range.
    if (*blob >= _logTypes.size())
        return false; // We reached the end of the vector.

    // Check output buffer size.
    int keyLength = static_cast<int>(_logTypes[*blob].first.size());
    int valueLength = static_cast<int>(_logTypes[*blob].second.size());
    bool valid = (*keySize > keyLength) && (*valueSize > valueLength);
    *keySize = keyLength + 1; // Requested buffer size including terminating null.
    *valueSize = valueLength + 1; // Requested buffer size including terminating null.

    if (!valid)
        return false; // At least one of the output buffers is too small.

    strcpy_s(key, *keySize, _logTypes[*blob].first.data());
    strcpy_s(value, *valueSize, _logTypes[*blob].second.data());

    (*blob)++;
    return true;
}

SERVERINTEROP_API bool GetFirstLogType(int* blob, char* key, int* keySize, char* value, int* valueSize)
{
    if ((blob == nullptr) || (key == nullptr) || (keySize == nullptr) ||
        (value == nullptr) || (valueSize == nullptr))
        return false; // Incorrect parameters.

    // Cache map in global variable, to ensure it remains constant during enumeration.
    std::map<std::string, std::string> logTypes;
    if (!GetLogTypes(&logTypes))
        return false;

    _logTypes.clear();
    for (const auto& pair : logTypes)
        _logTypes.push_back(pair);

    // Initialize hidden index -- i.e. blob -- to first value and check that it is in range.
    *blob = 0;

    return CopyLogType(blob, key, keySize, value, valueSize);
}


SERVERINTEROP_API bool GetNextLogType(int* blob, char* key, int* keySize, char* value, int* valueSize)
{
    if ((blob == nullptr) || (key == nullptr) || (keySize == nullptr) ||
        (value == nullptr) || (valueSize == nullptr))
        return false; // Incorrect parameters.

    return CopyLogType(blob, key, keySize, value, valueSize);
}
