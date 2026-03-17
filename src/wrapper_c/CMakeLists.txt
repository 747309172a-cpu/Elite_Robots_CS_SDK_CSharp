cmake_minimum_required(VERSION 3.16)
project(elite_cs_series_sdk_c LANGUAGES C CXX)
include(GNUInstallDirs)
include(FetchContent)

set(ELITE_C_WRAPPER_TARGET elite_cs_series_sdk_c)
option(ELITE_AUTO_FETCH_SDK "Auto download Elite CS SDK when not found" OFF)

set(
    ELITE_C_WRAPPER_SOURCES
    source/Dashboard/DashboardClient_C.cpp
    source/Elite/EliteDriver_C.cpp
    source/Elite/ControllerLog_C.cpp
    source/Elite/Log_C.cpp
    source/Elite/RemoteUpgrade_C.cpp
    source/Elite/SerialCommunication_C.cpp
    source/Elite/VersionInfo_C.cpp
    source/Primary/PrimaryPortInterface_C.cpp
    source/Rtsi/RtsiClientInterface_C.cpp
    source/Rtsi/RtsiRecipeInternal_C.cpp
    source/Rtsi/RtsiIoInterface_C.cpp
)

add_library(${ELITE_C_WRAPPER_TARGET} SHARED ${ELITE_C_WRAPPER_SOURCES})
target_compile_features(${ELITE_C_WRAPPER_TARGET} PRIVATE cxx_std_17)

if(CMAKE_SYSTEM_NAME STREQUAL "Windows")
    target_compile_definitions(${ELITE_C_WRAPPER_TARGET} PRIVATE ELITE_C_EXPORT_LIBRARY)
endif()

find_package(elite-cs-series-sdk CONFIG QUIET)
if(NOT TARGET elite-cs-series-sdk::shared)
    if(ELITE_AUTO_FETCH_SDK)
        message(STATUS "elite-cs-series-sdk not found locally, fetching from remote repository...")

        execute_process(
            COMMAND git config --get remote.origin.url
            WORKING_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}
            OUTPUT_VARIABLE GIT_REMOTE_URL
            OUTPUT_STRIP_TRAILING_WHITESPACE
        )

        if(GIT_REMOTE_URL MATCHES "gitee.com")
            if(NOT DEFINED ELITE_CS_SDK_REPO)
                set(ELITE_CS_SDK_REPO "https://gitee.com/elite-robots/Elite_Robots_CS_SDK.git")
            endif()
        else()
            if(NOT DEFINED ELITE_CS_SDK_REPO)
                set(ELITE_CS_SDK_REPO "https://github.com/Elite-Robots/Elite_Robots_CS_SDK.git")
            endif()
        endif()

        FetchContent_Declare(
            elite_cs_sdk
            GIT_REPOSITORY ${ELITE_CS_SDK_REPO}
            GIT_TAG main
        )
        FetchContent_MakeAvailable(elite_cs_sdk)

        # Upstream exports elite_cs_series_sdk_SHARED; create expected alias if needed.
        if(TARGET elite_cs_series_sdk_SHARED AND NOT TARGET elite-cs-series-sdk::shared)
            add_library(elite-cs-series-sdk::shared ALIAS elite_cs_series_sdk_SHARED)
        endif()
    endif()
endif()

if(NOT TARGET elite-cs-series-sdk::shared)
    message(FATAL_ERROR
        "Cannot find elite-cs-series-sdk::shared. "
        "Set CMAKE_PREFIX_PATH to a valid SDK install, or use -DELITE_AUTO_FETCH_SDK=ON.")
endif()

target_link_libraries(${ELITE_C_WRAPPER_TARGET} PRIVATE elite-cs-series-sdk::shared)

target_include_directories(
    ${ELITE_C_WRAPPER_TARGET}
    PUBLIC
        $<BUILD_INTERFACE:${CMAKE_CURRENT_SOURCE_DIR}/include>
        $<INSTALL_INTERFACE:include>
)

if(CMAKE_SYSTEM_NAME STREQUAL "Linux" OR APPLE)
    set_target_properties(${ELITE_C_WRAPPER_TARGET} PROPERTIES OUTPUT_NAME "elite_cs_series_sdk_c")
endif()

if(ELITE_INSTALL)
    install(
        TARGETS ${ELITE_C_WRAPPER_TARGET}
        RUNTIME DESTINATION ${CMAKE_INSTALL_BINDIR}
        LIBRARY DESTINATION ${CMAKE_INSTALL_LIBDIR}
        ARCHIVE DESTINATION ${CMAKE_INSTALL_LIBDIR}
    )
    install(DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}/include/ DESTINATION ${CMAKE_INSTALL_INCLUDEDIR})
endif()
