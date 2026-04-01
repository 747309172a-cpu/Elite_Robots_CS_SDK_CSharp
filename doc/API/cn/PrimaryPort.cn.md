# PrimaryClientInterface 类

## 简介

`PrimaryClientInterface` 用于与机器人 Primary 端口通讯，支持发送脚本、读取运动学参数和异常回调。

## 导入

```csharp
using EliteRobots.CSharp;
```

## 构造函数

### PrimaryClientInterface

```csharp
public PrimaryClientInterface()
```

- ***功能***
  - 创建 Primary 客户端实例。
- ***参数***
  - 无。
- ***返回值***
  - 无。

## 通讯接口

### connect

```csharp
public bool connect(string ip, int port = 30001)
```

- ***功能***
  - 连接到机器人的30001端口（默认）。
- ***参数***
  - `ip`：机器人 IP。
  - `port`：端口，默认 `30001`。
- ***返回值***
  - 连接成功返回 `true`，失败返回 `false`。

### disconnect

```csharp
public void disconnect()
```

- ***功能***
  - 断开连接。
- ***参数***
  - 无。
- ***返回值***
  - 无。

### getLocalIP

```csharp
public string getLocalIP()
```

- ***功能***
  - 获取本地绑定 IP。
- ***参数***
  - 无。
- ***返回值***
  - 返回 IP 字符串，若无则可能为空字符串。

## 脚本与数据

### sendScript

```csharp
public bool sendScript(string script)
```

- ***功能***
  - 发送脚本文本到机器人。
- ***参数***
  - `script`：脚本内容。
- ***返回值***
  - 发送成功返回 `true`，失败返回 `false`。

### getPackage

```csharp
public bool getPackage(out KinematicsInfo info, int timeoutMs = 1000)
```

- ***功能***
  - 读取 Primary 运动学数据包。
- ***参数***
  - `info`：输出参数，包含 `DhA`、`DhD`、`DhAlpha` 三组 6 维数组。
  - `timeoutMs`：读取超时（毫秒），默认 `1000`。
- ***返回值***
  - 获取成功返回 `true`，失败返回 `false`。

## 异常回调

### registerRobotExceptionCallback

```csharp
public void registerRobotExceptionCallback(Action<PrimaryRobotException> callback)
```

- ***功能***
  - 注册原始异常回调函数。当从 Primary 端口接收到异常报文时触发。
- ***参数***
  - `callback`：异常回调函数，参数类型为 `PrimaryRobotException`。
- ***返回值***
  - 无。

### registerWrappedRobotExceptionCallback

```csharp
public void registerWrappedRobotExceptionCallback(Action<RobotException> callback)
```

- ***功能***
  - 注册高级异常回调，回调参数会被映射为 `RobotDisconnectedException` / `RobotError` / `RobotRuntimeException`。
- ***参数***
  - `callback`：高级异常回调函数。
- ***返回值***
  - 无。
### registerRobotExceptionCallback

```csharp
public void registerRobotExceptionCallback(
    Action<RobotError> onRobotError,
    Action<RobotRuntimeException> onRuntimeException,
    Action<RobotDisconnectedException>? onDisconnected = null)
```

- ***功能***
  - 注册按类型分发的异常回调。
- ***参数***
  - `onRobotError`：机器人错误回调。
  - `onRuntimeException`：脚本运行时异常回调。
  - `onDisconnected`：机器人断连回调（可选）。
- ***返回值***
  - 无。


### clearRobotExceptionCallback

```csharp
public void clearRobotExceptionCallback()
```

- ***功能***
  - 清除异常回调。
  - 调用后会同步取消 native 层已注册的异常回调。
- ***参数***
  - 无。
- ***返回值***
  - 无。

## 资源释放

### Dispose

```csharp
public void Dispose()
```

- ***功能***
  - 释放客户端资源。
- ***参数***
  - 无。
- ***返回值***
  - 无。

## 数据类型

### KinematicsInfo

```csharp
public sealed class KinematicsInfo
{
    public double[] DhA { get; init; }
    public double[] DhD { get; init; }
    public double[] DhAlpha { get; init; }
}
```

- ***功能***
  - 保存 Primary 输出的运动学参数。

### PrimaryRobotException

```csharp
public sealed class PrimaryRobotException
```

- ***功能***
  - 描述 Primary 异常数据。
- ***主要字段***
  - `Type`、`Timestamp`、`ErrorCode`、`SubErrorCode`、`ErrorSource`、`ErrorLevel`、`ErrorDataType`、`DataU32`、`DataI32`、`DataF32`、`Line`、`Column`、`Message`。
