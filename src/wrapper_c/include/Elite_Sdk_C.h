// SPDX-License-Identifier: MIT
// Copyright (c) 2026, Elite Robots.

//
// C ABI wrapper for a subset of Elite Robots CS SDK.
//c语言总入口头文件，包含了c语言接口需要的所有头文件，用户在使用c接口时只需要包含这个头文件即可
#ifndef __ELITE_SDK_C_H__
#define __ELITE_SDK_C_H__

#include <Elite_C_Types.h>
#include <stdint.h>

#include <Dashboard/DashboardClient_C.hpp>
#include <Elite/ControllerLog_C.hpp>
#include <Elite/EliteDriver_C.h>
#include <Elite/Log_C.hpp>
#include <Elite/RemoteUpgrade_C.hpp>
#include <Elite/SerialCommunication_C.hpp>
#include <Elite/VersionInfo_C.hpp>
#include <Primary/PrimaryPortInterface_C.hpp>
#include <Rtsi/Elite_Rtsi_C.h>

#endif
