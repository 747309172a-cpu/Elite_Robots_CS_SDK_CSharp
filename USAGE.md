# Elite Robots C# SDK 对外使用说明

## 1. 项目结构

- `src/wrapper_c`：C/C++ 包装层源码（生成 native 动态库）
- `src/wrapper_csharp`：C# 包装层项目（`wrapper_csharp.csproj`）
- `example`：示例项目（`example.csproj`）

---

## 2. 使用前准备

使用源码方式时，需要：

- .NET SDK 8.0+
- CMake
- C++ 编译器（gcc/clang/msvc）

---

## 3. 快速开始（源码方式）

在仓库根目录执行：

```bash
cmake -S . -B build -DELITE_COMPILE_C_WRAPPER=ON
cmake --build build -j4

dotnet build src/wrapper_csharp/wrapper_csharp.csproj
dotnet build example/example.csproj
```

运行示例：

```bash
dotnet run --project example -- <mode> <args...>
```

可用 `mode`：

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

## 4. 常用命令示例

```bash
dotnet run --project example -- primary_client <robot-ip>
dotnet run --project example -- dashboard_client <robot-ip>
dotnet run --project example -- rtsi_client <robot-ip> --port 30004
dotnet run --project example -- connect_robot_test <robot-ip> --server-port 50002
```

涉及外控脚本的模式（`driver/speedl/trajectory/servoj_plan/serial`）需要提供有效脚本文件路径（`--script-file` 或位置参数）。

---

## 5. 给第三方交付（二进制方式）

如果你不希望对方编译源码，可直接交付以下产物：

- `src/wrapper_csharp/bin/<Config>/net8.0/wrapper_csharp.dll`
- `build/wrapper/libelite_cs_series_sdk_c.so`（Linux，Windows 对应 `.dll`）
- `build/libelite-cs-series-sdk.so`（Linux，Windows 对应 `.dll`）

对方项目中：

1. 引用 `wrapper_csharp.dll`
2. 保证 native 动态库在运行时可被加载（放到可执行同目录，或配置系统库路径）

---

## 6. 常见问题

`DllNotFoundException` / 找不到 `elite_cs_series_sdk_c`：

- 先确认 native 动态库已生成
- 再确认动态库在程序运行时可搜索到

`serial` 示例连接失败：

- 确认机器人 SSH 可达（默认 22 端口）
- 确认网络和防火墙未阻断

---

## 7. 安全提示

运动相关示例会驱动机器人真实运动，务必：

- 先确认现场安全
- 保持急停可用
- 先用低速/低加速度验证
