// Server.cpp : Defines the exported functions for the DLL.
//

#include "pch.h"
#include "framework.h"
#include "Server.h"

SERVER_API bool GetLogTypes(std::map<std::string, std::string>* pMap)
{
    if (pMap == nullptr)
        return false;

    pMap->clear();
    pMap->emplace("DBG",    "Debug");
    pMap->emplace("INFO",   "Information");
    pMap->emplace("WARN",   "Warning");
    pMap->emplace("ERR",    "Error");
    pMap->emplace("FATAL",  "Fatal error");

    return true;
}
