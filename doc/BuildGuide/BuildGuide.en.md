# Elite Robots C# SDK Build Guide

## 1. Build Requirements

- .NET SDK 8.0+
- `git`
- `cmake`
- C/C++ compiler toolchain
- Network access to the native wrapper repository and, when needed, the upstream C++ SDK repository

By default, this repository bootstraps the native wrapper automatically during `dotnet build` or `dotnet run`.
The build downloads the separate native C wrapper repository, builds `elite_cs_series_sdk_c`, and copies the resulting native binaries into the current .NET output directory.
If `EliteNativeRepoUrl` is not set explicitly, the build tries to derive it from the current `origin` remote.

### 1.1 Windows (Visual Studio)

- Baseline dependency installation

On Windows, it is recommended to install the upstream C++ SDK dependencies through `vcpkg` first.

Download and bootstrap `vcpkg`. Use a dedicated directory because the same path will be needed later during the build:

```powershell
git clone https://github.com/microsoft/vcpkg.git
.\bootstrap-vcpkg.bat
```

Install the baseline dependencies:

```powershell
.\vcpkg install boost
.\vcpkg install libssh
.\vcpkg integrate install
```

After that, keep the `vcpkg` path available because you will use it as `VCPKG_ROOT` during the build.

### 1.2 Ubuntu

Baseline dependency installation:

```bash
sudo apt update

sudo apt install libboost-all-dev

sudo apt install libssh-dev # optional, recommended, recommended version: 0.9.6

# sudo apt install sshpass # required if libssh-dev is not installed
```

---

## 2. Build Steps

### 2.1 Windows Build

On Windows, open the Visual Studio terminal first, then run the following commands.

On Windows, the default build tries to link the C wrapper against the upstream C++ SDK static library so that fewer extra DLLs need to be distributed with the final package. Using a static `vcpkg` triplet is recommended.

Set `VCPKG_ROOT` to your own `vcpkg` installation path, and make sure `boost`, `libssh`, and other required dependencies have already been installed through `vcpkg`:

```bat
cd <clone of this repository>

set VCPKG_ROOT="C:\Users\<user>\vcpkg"
```

If this is the first build, run:

```bat
dotnet build src/elite_cs_sdk.csproj
```

If the project has already been built before and you need to force a native rebuild, run:

```bat
dotnet build src\elite_cs_sdk.csproj /p:EliteForceNativeRebuild=true
```

If you need the kinematics and pose algebra plugin DLLs on Windows, build with plugin compilation enabled and keep the upstream C++ SDK linked dynamically:

```bat
dotnet build src\elite_cs_sdk.csproj /p:EliteForceNativeRebuild=true /p:EliteCompileKinPlugin=true /p:EliteCompilePoseAlgPlugin=true /p:EliteLinkUpstreamStatic=false
```

`EliteLinkUpstreamStatic=false` is required for Windows plugin loading. With the Windows default `EliteLinkUpstreamStatic=true`, the DLLs can still build, but runtime plugin creation can fail because the C wrapper and plugin DLLs do not share the same upstream SDK DLL registry.

Build the example project:

```bat
dotnet build example/example.csproj
```

Create a NuGet package:

```bat
dotnet pack src/elite_cs_sdk.csproj -c Release -o ./nupkg
```

### 2.2 Ubuntu Build

```bash
cd <clone of this repository>

dotnet build src/elite_cs_sdk.csproj
```

The first build may take longer because it may:

- Clone the native wrapper repository into `.native-src/`
- Build native outputs under `.native-build/`
- Cache native runtime files under `.native-out/`
- Copy the runtime files into the current `bin/` output directory

Build the example project:

```bash
dotnet build example/example.csproj
```

Create a NuGet package:

```bash
dotnet pack src/elite_cs_sdk.csproj -c Release -o ./nupkg
```

### 2.3 Optional build properties

You can override the bootstrap behavior with MSBuild properties:

```bash
dotnet build src/elite_cs_sdk.csproj /p:EliteAutoBootstrapNative=false
dotnet build src/elite_cs_sdk.csproj /p:EliteNativeRepoUrl=https://github.com/<your-org>/<your-native-repo>.git
dotnet build src/elite_cs_sdk.csproj /p:EliteNativeRepoUrl=https://gitee.com/<your-org>/<your-native-repo>.git
dotnet build src/elite_cs_sdk.csproj /p:EliteNativeRepoRef=main
dotnet build src/elite_cs_sdk.csproj /p:EliteUpstreamSdkRepoUrl=https://gitee.com/<your-org>/Elite_Robots_CS_SDK.git
dotnet build src/elite_cs_sdk.csproj /p:EliteForceNativeRebuild=true
dotnet build src/elite_cs_sdk.csproj /p:EliteCompileKinPlugin=true /p:EliteCompilePoseAlgPlugin=true /p:EliteForceNativeRebuild=true
dotnet build src/elite_cs_sdk.csproj /p:EliteCompileKinPlugin=true /p:EliteCompilePoseAlgPlugin=true /p:EliteForceNativeRebuild=true /p:EliteLinkUpstreamStatic=false
```

Property meanings:

- `EliteAutoBootstrapNative`: enable or disable automatic native bootstrap
- `EliteNativeRepoUrl`: native wrapper repository URL
- `EliteNativeRepoRef`: git branch/tag/commit used for the native wrapper
- `EliteUpstreamSdkRepoUrl`: upstream C++ SDK repository URL used by the native wrapper when `ELITE_AUTO_FETCH_SDK=ON`
- `EliteForceNativeRebuild`: ignore cached native output and rebuild
- `EliteLinkUpstreamStatic`: whether the C wrapper links against the upstream C++ SDK static library; default is `true` on Windows and `false` on Linux
- `EliteCompileKinPlugin`: also build the kinematics plugin when the upstream C++ SDK is auto-fetched or built from source
- `EliteCompilePoseAlgPlugin`: also build the pose algebra plugin when the upstream C++ SDK is auto-fetched or built from source

On Windows, use `/p:EliteLinkUpstreamStatic=false` when building plugins that will be loaded at runtime by `KinematicsBase` or `PoseAlgebraBase`.

If `EliteNativeRepoUrl` is omitted, the build derives it from the current git `origin` remote when possible and falls back across GitHub/Gitee mirrors.
`EliteNativeRepoUrl` only controls the C wrapper repository, for example `Elite_Robots_CS_SDK_C`.
If you need to use a private mirror of the upstream C++ SDK repository, pass `EliteUpstreamSdkRepoUrl`; the bootstrap script forwards it to CMake as `ELITE_CS_SDK_REPO`.

`EliteCompileKinPlugin` and `EliteCompilePoseAlgPlugin` are forwarded to the native CMake configure step:

```bash
-DELITE_COMPILE_KIN_PLUGIN=true
-DELITE_COMPILE_POSE_ALG_PLUGIN=true
```

For example, to build from private mirrors on Windows:

```powershell
dotnet build src\elite_cs_sdk.csproj /p:EliteForceNativeRebuild=true /p:EliteNativeRepoUrl=https://gitee.com/<your-org>/Elite_Robots_CS_SDK_C.git /p:EliteUpstreamSdkRepoUrl=https://gitee.com/<your-org>/Elite_Robots_CS_SDK.git
```

Note: if the native build finds an already installed C++ SDK through `find_package(elite-cs-series-sdk)`, it uses that SDK directly and does not rebuild plugins from the installed SDK. In that case, build the plugins in the C++ SDK project beforehand, or clear/adjust the local SDK lookup path so the build uses the auto-fetch/source-build path.

Plugins add extra dependencies. For example, the kinematics plugin requires `orocos_kdl` and `Eigen3`, and the Eigen pose algebra plugin requires `Eigen3`. On Windows, using `vcpkg` with the matching triplet is recommended.

The bootstrap script will derive these automatically from `VCPKG_ROOT`:

- `CMAKE_TOOLCHAIN_FILE`
- `VCPKG_TARGET_TRIPLET`
- the MSVC `INCLUDE` path needed on Windows
- `BOOST_ROOT` / `Boost_INCLUDE_DIR`

If you use preinstalled dependency prefixes instead of `vcpkg`, pass them through `CMAKE_PREFIX_PATH`:

```powershell
$env:CMAKE_PREFIX_PATH="C:\path\to\your\deps"
dotnet build src/elite_cs_sdk.csproj /p:EliteForceNativeRebuild=true
```

If you need to override the default behavior explicitly, you can also run:

```bash
dotnet build src/elite_cs_sdk.csproj /p:EliteLinkUpstreamStatic=true
dotnet build src/elite_cs_sdk.csproj /p:EliteLinkUpstreamStatic=false
```

---

## 3. Related Documents

- For runtime usage, examples, FAQ, and external project integration, see: [User Guide](../UserGuide/UserGuide.en.md)
