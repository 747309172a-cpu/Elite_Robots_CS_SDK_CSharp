# KinematicsBase 类

## 简介

`KinematicsBase` 是运动学插件接口的 C# 封装，接口名称与 C++ SDK 保持一致。该类通过 native C 封装层加载运动学插件，并提供 MDH 参数设置、正解、逆解和默认超时时间配置能力。

## 导入

```csharp
using EliteRobots.CSharp;
```

## 构造函数

### KinematicsBase

```csharp
public KinematicsBase(string plugin_lib_path, string? plugin_class_name = null)
```

- ***功能***
  - 加载运动学插件并创建 `KinematicsBase` 实例。
- ***参数***
  - `plugin_lib_path`：运动学插件动态库路径。
  - `plugin_class_name`：插件类名；可传入 `ELITE::KdlKinematicsPlugin`。
- ***返回值***
  - 无。
- ***异常***
  - `ArgumentNullException`：`plugin_lib_path` 为空。
  - `EliteSdkException`：插件加载或实例创建失败。

## 插件库

常用插件动态库和类名：

| 平台 | 插件动态库 | 插件类名 |
| --- | --- | --- |
| Windows | `elite_kdl_kinematics.dll` | `ELITE::KdlKinematicsPlugin` |
| Linux | `libelite_kdl_kinematics.so` | `ELITE::KdlKinematicsPlugin` |

Windows 下运行目录还必须包含 `elite_cs_sdk.dll`、`elite_cs_series_sdk_c.dll` 和 `elite_cs_series_sdk.dll`。插件 DLL 需要使用 `/p:EliteLinkUpstreamStatic=false` 编译；否则即使 DLL 文件都存在，创建插件实例也可能失败。

## 相关类型

### KinematicsResult

```csharp
public struct KinematicsResult
{
    public KinematicError kinematic_error;
}
```

- ***功能***
  - 表示运动学求解结果。

### KinematicError

```csharp
public enum KinematicError
{
    OK = 1,
    SOLVER_NOT_ACTIVE = 2,
    NO_SOLUTION = 3,
}
```

- ***功能***
  - 表示运动学求解错误类型。

## 方法

### setMDH

```csharp
public void setMDH(double[] alpha, double[] a, double[] d)
```

- ***功能***
  - 设置机器人 MDH 参数。
- ***参数***
  - `alpha`：MDH alpha 参数，长度必须为 6。
  - `a`：MDH a 参数，长度必须为 6。
  - `d`：MDH d 参数，长度必须为 6。
- ***返回值***
  - 无。
- ***异常***
  - `ArgumentNullException`：数组参数为空。
  - `ArgumentException`：数组长度不是 6。
  - `EliteSdkException`：底层接口调用失败。

### getPositionFK

```csharp
public bool getPositionFK(double[] joint_angles, double[] poses)
```

- ***功能***
  - 根据关节角计算 TCP 位姿。
- ***参数***
  - `joint_angles`：输入关节角，长度必须为 6。
  - `poses`：输出 TCP 位姿，长度必须为 6。
- ***返回值***
  - 计算成功返回 `true`，否则返回 `false`。
- ***异常***
  - `ArgumentNullException`：数组参数为空。
  - `ArgumentException`：数组长度不是 6。
  - `EliteSdkException`：底层接口调用失败。

### getPositionIK

```csharp
public bool getPositionIK(double[] pose, double[] near, double[] solution, out KinematicsResult result)
```

- ***功能***
  - 根据 TCP 位姿和参考关节角计算单组逆解。
- ***参数***
  - `pose`：输入 TCP 位姿，长度必须为 6。
  - `near`：参考关节角，长度必须为 6。
  - `solution`：逆解输出关节角，长度必须为 6。
  - `result`：运动学结果，包含 `kinematic_error`。
- ***返回值***
  - 求解成功返回 `true`，否则返回 `false`。
- ***异常***
  - `ArgumentNullException`：数组参数为空。
  - `ArgumentException`：数组长度不是 6。
  - `EliteSdkException`：底层接口调用失败。

### getPositionIK

```csharp
public bool getPositionIK(double[] pose, double[] near, List<double[]> solutions, out KinematicsResult result)
```

- ***功能***
  - 根据 TCP 位姿和参考关节角计算多组逆解，最多返回默认数量的解。
- ***参数***
  - `pose`：输入 TCP 位姿，长度必须为 6。
  - `near`：参考关节角，长度必须为 6。
  - `solutions`：逆解输出列表；调用成功时会被清空并写入新的解。
  - `result`：运动学结果，包含 `kinematic_error`。
- ***返回值***
  - 求解成功返回 `true`，否则返回 `false`。
- ***异常***
  - `ArgumentNullException`：数组参数或 `solutions` 为空。
  - `ArgumentException`：数组长度不是 6。
  - `EliteSdkException`：底层接口调用失败。

### getPositionIK

```csharp
public bool getPositionIK(double[] pose, double[] near, List<double[]> solutions, int max_solutions, out KinematicsResult result)
```

- ***功能***
  - 根据 TCP 位姿和参考关节角计算多组逆解，并指定最多返回解的数量。
- ***参数***
  - `pose`：输入 TCP 位姿，长度必须为 6。
  - `near`：参考关节角，长度必须为 6。
  - `solutions`：逆解输出列表；调用成功时会被清空并写入新的解。
  - `max_solutions`：最多返回解的数量，必须大于等于 0。
  - `result`：运动学结果，包含 `kinematic_error`。
- ***返回值***
  - 求解成功返回 `true`，否则返回 `false`。
- ***异常***
  - `ArgumentNullException`：数组参数或 `solutions` 为空。
  - `ArgumentException`：数组长度不是 6，或 `max_solutions` 小于 0。
  - `EliteSdkException`：底层接口调用失败。

### setDefaultTimeout

```csharp
public void setDefaultTimeout(double timeout)
```

- ***功能***
  - 设置运动学求解默认超时时间。
- ***参数***
  - `timeout`：超时时间。
- ***返回值***
  - 无。
- ***异常***
  - `EliteSdkException`：底层接口调用失败。

### getDefaultTimeout

```csharp
public double getDefaultTimeout()
```

- ***功能***
  - 获取运动学求解默认超时时间。
- ***参数***
  - 无。
- ***返回值***
  - 默认超时时间。
- ***异常***
  - `EliteSdkException`：底层接口调用失败。

### Dispose

```csharp
public void Dispose()
```

- ***功能***
  - 释放 native 运动学插件实例。
- ***参数***
  - 无。
- ***返回值***
  - 无。

## 示例

Windows 下最小 C# 用法：

```csharp
using System;
using System.IO;
using EliteRobots.CSharp;

var pluginPath = Path.Combine(AppContext.BaseDirectory, "elite_kdl_kinematics.dll");

using var kinematics = new KinematicsBase(pluginPath, "ELITE::KdlKinematicsPlugin");

double[] alpha = { 0, -Math.PI / 2, 0, -Math.PI / 2, Math.PI / 2, -Math.PI / 2 };
double[] a = { 0, 0, -0.425, -0.39225, 0, 0 };
double[] d = { 0.089159, 0, 0, 0.10915, 0.09465, 0.0823 };
kinematics.setMDH(alpha, a, d);

double[] joints = { 0, -1.57, 1.57, 0, 1.57, 0 };
double[] pose = new double[6];

if (kinematics.getPositionFK(joints, pose))
{
    Console.WriteLine(string.Join(", ", pose));
}
```

Linux 下运行示例工程：

```bash
dotnet run --project example -- kinematics 172.16.102.156 /path/to/libelite_kdl_kinematics.so ELITE::KdlKinematicsPlugin
```

Windows 下运行示例工程：

```powershell
dotnet run --project example -- kinematics 172.16.102.156 .\libs\elite_kdl_kinematics.dll ELITE::KdlKinematicsPlugin
```
