# SerialCommunication（RS485）

## 简介

机器人的末端和控制柜有RS485通讯接口，SDK 提供了相应的功能。

SDK 读写机器人串口本质是，机器人将 RS485 接口的数据转发到指定的 TCP 端口，SDK直接去读写该TCP端口。

## 导入

```csharp
using EliteRobots.CSharp;
```

## 串口配置

### SerialConfig

```csharp
public sealed class SerialConfig
```

- ***功能***
  - 配置串口参数。
- ***属性***
  - `baud_rate`：波特率（`SerialConfigBaudRate`）。
  - `parity`：校验位（`SerialConfigParity`）。
  - `stop_bits`：停止位（`SerialConfigStopBits`）。

## 通信对象

### connect

```csharp
public bool connect(int timeout_ms)
```

- ***功能***
  - 连接转发串口。
- ***参数***
  - `timeout_ms`：连接超时时间（毫秒）。
- ***返回值***
  - 连接成功返回 `true`，失败返回 `false`。

### disconnect

```csharp
public void disconnect()
```

- ***功能***
  - 断开串口连接。
- ***参数***
  - 无。
- ***返回值***
  - 无。

### isConnected

```csharp
public bool isConnected()
```

- ***功能***
  - 查询连接状态。
- ***参数***
  - 无。
- ***返回值***
  - 已连接返回 `true`，否则 `false`。

### getSocatPid

```csharp
public int getSocatPid()
```

- ***功能***
  - 获取底层转发进程 PID。
- ***参数***
  - 无。
- ***返回值***
  - 返回 PID。

### write

```csharp
public int write(byte[] data)
```

- ***功能***
  - 向串口写入字节数据。
- ***参数***
  - `data`：待写入字节数组。
- ***返回值***
  - 返回实际写入字节数。

### read

```csharp
public byte[] read(int size, int timeout_ms)
```

- ***功能***
  - 从串口读取字节数据。
- ***参数***
  - `size`：期望读取字节数。
  - `timeout_ms`：读取超时（毫秒）。
- ***返回值***
  - 返回读取到的字节数组，超时或无数据时可能为空数组。

### Dispose

```csharp
public void Dispose()
```

- ***功能***
  - 释放通信对象资源。
- ***参数***
  - 无。
- ***返回值***
  - 无。

## EliteDriver 中的入口

### startToolRs485

```csharp
public EliteSerialCommunication? startToolRs485(SerialConfig config, string ssh_password, int tcp_port = 54321)
```

- ***功能***
  - 启动工具端 RS485 转发。
- ***参数***
  - `config`：串口配置。
  - `ssh_password`：机器人 SSH 密码。
  - `tcp_port`：本地转发端口。
- ***返回值***
  - 成功返回通信对象，失败可能返回 `null`。

