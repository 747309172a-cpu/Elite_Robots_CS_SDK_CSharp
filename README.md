[中文](./README_CN.md)
# Elite Robots CS SDK (C#)

This repository provides C# bindings for the Elite Robots CS SDK.
By default, `dotnet build` and `dotnet run` will automatically fetch and build the native C wrapper `elite_cs_series_sdk_c` when it is missing.
The bootstrap step expects `git`, `cmake`, a C/C++ compiler, and network access to the native dependency repository.
If `EliteNativeRepoUrl` is not provided, the build will try to derive the native repository URL from the current `origin` remote and use the sibling repository `Elite_Robots_CS_SDK_C.git`.

## Requirements
- ***CS Controller*** (robot controller software):
  - for 2.13.x, version must be >= **2.13.4** (CS-Series)
  - for 2.14.x, version must be >= **2.14.2**
  - If your controller software is lower than the required version, upgrade is recommended.
- .NET SDK 8.0+

## Build and Install
For build/install steps, see: [Build Guide](./doc/BuildGuide/BuildGuide.en.md)

## API Reference
[API Reference](./doc/API/en/API.en.md)
