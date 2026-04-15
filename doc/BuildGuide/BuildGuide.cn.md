# Elite Robots C# SDK 构建指南

## 1. 构建要求

- .NET SDK 8.0+
- `git`
- `cmake`
- C/C++ 编译器工具链
- 可访问原生 C 封装仓库的网络环境；如本机缺少上游 C++ SDK，还需要能访问上游 SDK 仓库

默认情况下，本仓库会在 `dotnet build` 或 `dotnet run` 时自动准备原生依赖。
构建流程会拉取独立的 native C 封装仓库、编译 `elite_cs_series_sdk_c`，并把生成的原生库复制到当前 .NET 输出目录。
如果没有显式设置 `EliteNativeRepoUrl`，构建会尝试根据当前仓库的 `origin` 地址自动推导。

### 1.1 Windows（Visual Studio）

- 基础依赖安装

Windows 下需要使用 `vcpkg` 安装上游 C++ SDK 所需的基础依赖。

首先下载并初始化 `vcpkg`。建议单独创建一个目录保存 `vcpkg`，这个路径后续构建时会用到：

```powershell
git clone https://github.com/microsoft/vcpkg.git
.\bootstrap-vcpkg.bat
```

安装基础依赖：

```powershell
.\vcpkg install boost
.\vcpkg install libssh
.\vcpkg integrate install
```

完成后记住 `vcpkg` 所在目录，后续构建时需要设置 `VCPKG_ROOT`。

### 1.2 Ubuntu

基础依赖安装：

```bash
sudo apt update

sudo apt install libboost-all-dev

sudo apt install libssh-dev # 可选，建议安装，建议版本为0.9.6

# sudo apt install sshpass # 如果没安装 libssh-dev 则需要安装此指令
```

---

## 2. 构建步骤

### 2.1 Windows 编译

Windows 上请先打开 Visual Studio 的终端，再执行以下命令。

Windows 默认会尝试让 C 封装层链接上游 C++ SDK 的静态库，以减少最终交付时额外分发的 DLL 数量。建议使用 `vcpkg` 的静态 triplet。

首先设置 `VCPKG_ROOT`，替换为你自己的 `vcpkg` 安装目录，并确保 `vcpkg` 中已安装 `boost`、`libssh` 等构建所需依赖：

```bat
cd <clone of this repository>

set VCPKG_ROOT="C:\Users\<user>\vcpkg"
```

如果是第一次构建，执行：

```bat
dotnet build src/elite_cs_sdk.csproj
```

如果之前已经构建过，需要强制重新编译 native 依赖时，执行：

```bat
dotnet build src\elite_cs_sdk.csproj /p:EliteForceNativeRebuild=true
```

编译 example：

```bat
dotnet build example/example.csproj
```

生成 NuGet 包：

```bat
dotnet pack src/elite_cs_sdk.csproj -c Release -o ./nupkg
```

### 2.2 Ubuntu 编译

```bash
cd <clone of this repository>

dotnet build src/elite_cs_sdk.csproj
```

首次构建时间可能会更长，因为会自动执行以下步骤：

- 将 native 封装仓库克隆到 `.native-src/`
- 在 `.native-build/` 下编译原生库
- 将可复用的原生产物缓存到 `.native-out/`
- 把运行所需的原生库复制到当前 `bin/` 输出目录

编译 example：

```bash
dotnet build example/example.csproj
```

生成 NuGet 包：

```bash
dotnet pack src/elite_cs_sdk.csproj -c Release -o ./nupkg
```

### 2.3 可选构建参数

可以通过 MSBuild 属性覆盖默认 bootstrap 行为：

```bash
dotnet build src/elite_cs_sdk.csproj /p:EliteAutoBootstrapNative=false
dotnet build src/elite_cs_sdk.csproj /p:EliteNativeRepoUrl=https://github.com/<your-org>/<your-native-repo>.git
dotnet build src/elite_cs_sdk.csproj /p:EliteNativeRepoUrl=https://gitee.com/<your-org>/<your-native-repo>.git
dotnet build src/elite_cs_sdk.csproj /p:EliteNativeRepoRef=main
dotnet build src/elite_cs_sdk.csproj /p:EliteForceNativeRebuild=true
```

参数含义：

- `EliteAutoBootstrapNative`：是否启用自动准备 native 依赖
- `EliteNativeRepoUrl`：native 封装仓库地址
- `EliteNativeRepoRef`：native 封装仓库使用的分支、tag 或 commit
- `EliteForceNativeRebuild`：忽略本地缓存并强制重新编译
- `EliteLinkUpstreamStatic`：是否让 C 封装层链接上游 C++ SDK 的静态库；Windows 默认 `true`，Linux 默认 `false`

如果未传入 `EliteNativeRepoUrl`，构建会在可能的情况下根据当前 git `origin` 自动推导仓库地址，并在 GitHub/Gitee 镜像之间回退。

bootstrap 脚本会自动根据 `VCPKG_ROOT` 推导：

- `CMAKE_TOOLCHAIN_FILE`
- `VCPKG_TARGET_TRIPLET`
- Windows 下 MSVC 所需的 `INCLUDE`
- `BOOST_ROOT` / `Boost_INCLUDE_DIR`

如果使用的是已安装好的第三方库目录，也可以通过 `CMAKE_PREFIX_PATH` 传给 bootstrap：

```powershell
$env:CMAKE_PREFIX_PATH="C:\path\to\your\deps"
dotnet build src/elite_cs_sdk.csproj /p:EliteForceNativeRebuild=true
```

如果你需要手动覆盖默认行为，也可以显式指定：

```bash
dotnet build src/elite_cs_sdk.csproj /p:EliteLinkUpstreamStatic=true
dotnet build src/elite_cs_sdk.csproj /p:EliteLinkUpstreamStatic=false
```

---

## 3. 相关文档

- 运行方式、示例说明、常见问题和外部项目接入请参考：[使用指南](../UserGuide/UserGuide.cn.md)
