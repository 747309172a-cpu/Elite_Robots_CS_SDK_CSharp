# Elite Robots C# SDK 使用指南

## 1. 概览

仓库主要目录包含：

- `src`：C# 封装库（基于 `P/Invoke` 与 `SafeHandle`）
- `example`：C# 示例程序

C# 调用链：

`C# -> C 封装层 (libelite_cs_series_sdk_c) -> C++ SDK`

---

## 2. 如何运行示例

通用格式：

```bash
dotnet run --project example -- <mode> <args...>
```

当前支持模式（见 `Program.cs`）：

- `primary_client`
- `dashboard_client`
- `driver`
- `speedl`
- `trajectory`
- `servoj_plan`
- `rtsi_client`
- `serial`
- `connect_robot_test`

---

## 3. 示例命令

```bash
# Primary
dotnet run --project example -- primary_client 172.16.102.156(机器人ip)

# Dashboard
dotnet run --project example -- dashboard_client 172.16.102.156

# Driver
dotnet run --project example -- driver 172.16.102.156 /path/to/external_control.script --headless

# SpeedL
dotnet run --project example -- speedl 172.16.102.156 --headless true --script-file /path/to/external_control.script

# Trajectory
dotnet run --project example -- trajectory 172.16.102.156 --headless true --script-file /path/to/external_control.script

# Servoj
dotnet run --project example -- servoj_plan 172.16.102.156 --headless true --script-file /path/to/external_control.script

# RTSI Client
dotnet run --project example -- rtsi_client 172.16.102.156 --port 30004

# serial
dotnet run --project example -- serial 172.16.102.156 --ssh-password 123456 --headless true --script-file /path/to/external_control.script

# 机器人反连 PC 套接字测试
dotnet run --project example -- connect_robot_test 172.16.102.156 --server-port 50002
```

---

## 4. 外部 C# 项目使用示例

### 4.1 通过 NuGet 包引用

**此步骤需要先按构建指南生成 NuGet 包。**

执行 `dotnet pack` 时，NuGet 包会把当前 `.native-out/` 下已经准备好的 native 运行库一并打进去。
外部项目通过包引用后，构建阶段只会把包内的 native 库复制到输出目录，不会再次编译 C 库。

进入项目目录：

```bash
cd myproject/
```

从本地添加对应功能包：

```bash
dotnet add package elite_cs_sdk --version 0.1.1 --source /xxxxx/nupkg
```

`--source` 后面的参数为 `Elite_Robots_CS_SDK_CSharp` 项目生成的本地 `nupkg` 目录。
如果需要分发多个平台，请先准备好对应平台的 native 输出，再执行 `dotnet pack`。

代码示例：

```csharp
using EliteRobots.CSharp; // 所有接口统一命名空间

var ip = args.Length > 0 ? args[0] : "172.16.102.156"; // 机器人默认 IP，需修改成自己的机器人 IP

using var dash = new DashboardClientInterface();
if (!dash.connect(ip))
{
    Console.WriteLine("connect failed");
    return;
}
Console.WriteLine($"RobotMode: {dash.robotMode()}");
dash.disconnect();
```

编译项目：

```bash
dotnet build
```

运行示例：

```bash
dotnet run
```

### 4.2 Windows 下使用 Visual Studio 直接引用 DLL

**此方式需要以下两个dll库文件**
- `elite_cs_sdk.dll`：C# 封装层
- `elite_cs_series_sdk_c.dll`：原生 C 封装层


推荐做法：

1. 在 Visual Studio 中新建一个 `.NET 8` 控制台项目。
2. 右键项目，选择“添加引用”，浏览并添加本仓库构建输出目录中的 `elite_cs_sdk.dll`。
3. 将 `elite_cs_series_sdk_c.dll` 复制到你的目标项目输出目录，或配置为生成时自动复制到 `bin/Debug/net8.0/` 或 `bin/Release/net8.0/`。
4. 确保运行目录里最终同时存在你的程序、`elite_cs_sdk.dll` 和所需 native DLL。

一个简单的目录示例：

```text
MyApp/bin/Debug/net8.0/
  MyApp.exe
  MyApp.dll
  elite_cs_sdk.dll
  elite_cs_series_sdk_c.dll
```
代码示例可以参考example下使用案例（需修改配方和脚本读取路径）


如果运行时提示 `DllNotFoundException: elite_cs_series_sdk_c`，通常表示 native DLL 没有被复制到程序运行目录。

---

## 5. example 各示例功能与流程

### 5.1 `primary_client`

- 功能：连接 Primary 端口，读取运动学包，发送脚本，接收机器人异常回调。
- 流程：`connect -> getPackage(KinematicsInfo) -> 注册回调 -> 发送脚本 -> disconnect`

### 5.2 `dashboard_client`

- 功能：执行 Dashboard 层面的机器人控制。
- 流程：`connect -> 版本/速度/模式读写 -> popup -> 上下电 -> disconnect`

### 5.3 `driver`

- 功能：覆盖测试 `EliteDriver` 的主要控制接口。
- 流程：创建配置并初始化 Driver，依次测试控制方法（`writeServoj/speedl/speedj`、轨迹、力控、Primary 包、回调、可选 RS485）。

### 5.4 `speedl`

- 功能：`writeSpeedl` 简化示例，包含外控启动流程。
- 流程：`dashboard 上电+刹车释放 -> driver 外控就绪 -> 下行速度 5 秒 -> 上行速度 5 秒 -> stopControl`

### 5.5 `trajectory`

- 功能：轨迹运动示例，包含基于回调的完成判定。
- 流程：
  - 通过 RTSI 读取当前关节/TCP
  - 启动控制
  - 先执行关节 `move`
  - 再用 `writeTrajectoryPoint` 下发笛卡尔轨迹点
  - 循环发送 NOOP 保活并等待回调结果

### 5.6 `servoj_plan`

- 功能：梯形速度规划 `servoj` 示例。
- 流程：计算第 6 关节梯形速度轨迹并循环 `writeServoj` 下发点位。

### 5.7 `rtsi_client`

- 功能：RTSI 通用客户端示例。
- 流程：`connect -> 协议协商 -> 配置 recipe -> start/receive -> 发送输入 recipe -> pause/disconnect`

### 5.8 `serial`

- 功能：通过 Driver 打通工具端 RS485 串口通信。
- 流程：`dashboard 就绪 -> 外控就绪 -> startToolRs485 -> 串口 connect/write/read -> endToolRs485`

### 5.9 `connect_robot_test`

- 功能：验证机器人能否通过脚本反连 PC 套接字。
- 流程：本地启动 TCP 服务，Primary 下发脚本，校验机器人返回字符串。

---

## 6. 配方文件

示例配方文件位置：

- `example/resource/input_recipe.txt`
- `example/resource/output_recipe.txt`

---

## 7. 常见问题

### 7.1 `DllNotFoundException: elite_cs_series_sdk_c`

- 如果你是直接构建本仓库：
  - 先重新执行 `dotnet build`，确认自动 bootstrap 处于启用状态
  - 如果自动 bootstrap 失败，重点检查：
    - `git`、`cmake`、C/C++ 编译器是否已安装
    - 机器是否能访问 native 封装仓库
    - native 封装在需要 `ELITE_AUTO_FETCH_SDK=ON` 时，是否能继续访问上游 SDK 仓库
    - 如果仓库地址是自动推导的，当前仓库的 `origin` 是否确实对应正确的 owner 或 fork
    - 如果当前网络无法访问 GitHub，确认 Gitee 是否可访问
- 如果你是在其他项目里通过 NuGet 包引用：
  - 确认打包前 `.native-out/` 下已经准备好了对应平台的 native 运行库
  - 如果你更新了包但仍在使用旧缓存，请升级包版本后重新添加引用
  - 确认 native 库文件已经被复制到项目输出目录

### 7.2 串口示例报 `SSH connection failed: Connection refused`

- `startToolRs485` 依赖控制器 SSH。
- 检查控制器 SSH 服务是否开启、22 端口是否可达、防火墙/路由是否阻断。

### 7.3 Windows 下出现 `NMAKE fatal error U1052: 未找到文件 "Makefile"`

- 这通常不是根因，而是前面的 `cmake` 配置步骤已经失败，后续 `cmake --build` 又继续执行导致的二次报错。
- 重点检查：
  - 是否安装了 Visual Studio 的 C++ 构建工具，或 `Ninja`
  - `cmake` 是否能找到可用的 C/C++ 编译器
  - native 封装仓库及其上游依赖仓库是否可访问
- 更新到包含最新 bootstrap 脚本的版本后，构建会直接显示真实的 `cmake configure` 失败原因，而不是只显示 `NMAKE` 错误。

### 7.4 `RtUtils` FIFO 调度警告

- 这是实时调度优化提示，通常不是致命错误，可忽略。

### 7.5 Windows 下报 `Could NOT find Boost`

- 这通常表示上游 C++ SDK 的依赖没有通过 `vcpkg` 或其他前缀路径传给 `cmake`。
- 如果你已经通过 `vcpkg` 安装了 `boost`，请显式设置：
  - `VCPKG_ROOT`
  - `CMAKE_TOOLCHAIN_FILE`
  - 需要时再设置 `VCPKG_TARGET_TRIPLET`
- 如果不是 `vcpkg` 安装的依赖，可以改为设置 `CMAKE_PREFIX_PATH`。

---

## 8. 安全提示

轨迹、速度与伺服示例会驱动机器人真实运动。

- 运行前确认工作空间安全。
- 保持急停可用。
- 先使用较低速度与加速度。
