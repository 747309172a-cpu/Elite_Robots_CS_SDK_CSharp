# DashboardClientInterface 类

## 简介

`DashboardClientInterface` 用于与机器人 Dashboard 服务通讯，执行上电、程序运行、状态查询、配置加载等操作。

## 导入

```csharp
using EliteRobots.CSharp;
```

## 构造函数

### DashboardClientInterface

```csharp
public DashboardClientInterface()
```

- ***功能***
  - 创建 Dashboard 客户端实例。
- ***参数***
  - 无。
- ***返回值***
  - 无。

## 通讯接口

### connect

```csharp
public bool connect(string ip, int port = 29999)
```

- ***功能***
  - 连接 Dashboard 服务器。
- ***参数***
  - `ip`：机器人 IP。
  - `port`：端口，默认 `29999`。
- ***返回值***
  - 连接成功返回 `true`，失败返回 `false`。

### disconnect

```csharp
public void disconnect()
```

- ***功能***
  - 断开 Dashboard 连接。
- ***参数***
  - 无。
- ***返回值***
  - 无。

### echo

```csharp
public bool echo()
```

- ***功能***
  - 检查与仪表盘shell服务器的连接状态。
- ***参数***
  - 无。
- ***返回值***
  - 连通返回 `true`，否则 `false`。

## 机器人控制

### powerOn

```csharp
public bool powerOn()
```

- ***功能***
  - 机器人上电。
- ***参数***
  - 无。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### powerOff

```csharp
public bool powerOff()
```

- ***功能***
  - 机器人下电。
- ***参数***
  - 无。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### brakeRelease

```csharp
public bool brakeRelease()
```

- ***功能***
  - 释放抱闸。
- ***参数***
  - 无。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### closeSafetyDialog

```csharp
public bool closeSafetyDialog()
```

- ***功能***
  - 关闭安全弹窗。
- ***参数***
  - 无。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### unlockProtectiveStop

```csharp
public bool unlockProtectiveStop()
```

- ***功能***
  - 解除机器人保护性停止。
- ***参数***
  - 无。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### safetySystemRestart

```csharp
public bool safetySystemRestart()
```

- ***功能***
  - 重启安全系统。
- ***参数***
  - 无。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

## 程序控制

### playProgram

```csharp
public bool playProgram()
```

- ***功能***
  - 启动程序。
- ***参数***
  - 无。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### pauseProgram

```csharp
public bool pauseProgram()
```

- ***功能***
  - 暂停程序。
- ***参数***
  - 无。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### stopProgram

```csharp
public bool stopProgram()
```

- ***功能***
  - 停止程序。
- ***参数***
  - 无。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### taskIsRunning

```csharp
public bool taskIsRunning()
```

- ***功能***
  - 查询当前任务是否在运行。
- ***参数***
  - 无。
- ***返回值***
  - 运行中返回 `true`，否则 `false`。

### getTaskStatus

```csharp
public TaskStatus getTaskStatus()
```

- ***功能***
  - 获取任务状态。
- ***参数***
  - 无。
- ***返回值***
  - 返回 `TaskStatus` 枚举值。

## 配置与任务

### loadConfiguration

```csharp
public bool loadConfiguration(string path)
```

- ***功能***
  - 加载机器人配置文件。
- ***参数***
  - `path`：配置文件路径。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### loadTask

```csharp
public bool loadTask(string path)
```

- ***功能***
  - 加载任务文件。
- ***参数***
  - `path`：任务文件路径。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### isConfigurationModify

```csharp
public bool isConfigurationModify()
```

- ***功能***
  - 查询配置是否被修改。
- ***参数***
  - 无。
- ***返回值***
  - 有修改返回 `true`，否则 `false`。

### isTaskSaved

```csharp
public bool isTaskSaved()
```

- ***功能***
  - 查询任务是否已保存。
- ***参数***
  - 无。
- ***返回值***
  - 已保存返回 `true`，否则 `false`。

### getTaskPath

```csharp
public string getTaskPath()
```

- ***功能***
  - 获取当前任务路径。
- ***参数***
  - 无。
- ***返回值***
  - 返回任务路径字符串。

## 状态查询

### robotMode

```csharp
public RobotMode robotMode()
```

- ***功能***
  - 获取机器人模式。
- ***参数***
  - 无。
- ***返回值***
  - 返回 `RobotMode` 枚举值。

### safetyMode

```csharp
public SafetyMode safetyMode()
```

- ***功能***
  - 获取安全模式。
- ***参数***
  - 无。
- ***返回值***
  - 返回 `SafetyMode` 枚举值。

### runningStatus

```csharp
public TaskStatus runningStatus()
```

- ***功能***
  - 获取任务运行状态。
- ***参数***
  - 无。
- ***返回值***
  - 返回 `TaskStatus` 枚举值。

### speedScaling

```csharp
public int speedScaling()
```

- ***功能***
  - 获取当前速度缩放百分比。
- ***参数***
  - 无。
- ***返回值***
  - 返回整型缩放值。

### setSpeedScaling

```csharp
public bool setSpeedScaling(int scaling)
```

- ***功能***
  - 设置速度缩放百分比。
- ***参数***
  - `scaling`：目标缩放值。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

## 其他接口

### log

```csharp
public bool log(string message)
```

- ***功能***
  - 向控制器日志写入消息。
- ***参数***
  - `message`：日志内容。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### popup

```csharp
public bool popup(string arg, string message = "")
```

- ***功能***
  - 弹出消息框。
- ***参数***
  - `arg`：弹窗参数。
  - `message`：弹窗文本。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### quit

```csharp
public void quit()
```

- ***功能***
  - 退出仪表盘并断开连接。
- ***参数***
  - 无。
- ***返回值***
  - 无。

### reboot

```csharp
public void reboot()
```

- ***功能***
  - 重启机器人系统。
- ***参数***
  - 无。
- ***返回值***
  - 无。

### shutdown

```csharp
public void shutdown()
```

- ***功能***
  - 关闭系统。
- ***参数***
  - 无。
- ***返回值***
  - 无。

### help

```csharp
public string help(string cmd)
```

- ***功能***
  - 获取命令帮助信息。
- ***参数***
  - `cmd`：需要帮助的命令。
- ***返回值***
  - 返回帮助文本。

### usage

```csharp
public string usage(string cmd)
```

- ***功能***
  - 获取命令用法信息。
- ***参数***
  - `cmd`：命令名。
- ***返回值***
  - 返回用法文本。

### version

```csharp
public string version()
```

- ***功能***
  - 获取版本信息。
- ***参数***
  - 无。
- ***返回值***
  - 返回版本字符串。

### robotType

```csharp
public string robotType()
```

- ***功能***
  - 获取机器人型号。
- ***参数***
  - 无。
- ***返回值***
  - 返回型号字符串。

### robotSerialNumber

```csharp
public string robotSerialNumber()
```

- ***功能***
  - 获取机器人序列号。
- ***参数***
  - 无。
- ***返回值***
  - 返回序列号字符串。

### robotID

```csharp
public string robotID()
```

- ***功能***
  - 获取机器人 ID。
- ***参数***
  - 无。
- ***返回值***
  - 返回 ID 字符串。

### configurationPath

```csharp
public string configurationPath()
```

- ***功能***
  - 获取当前配置路径。
- ***参数***
  - 无。
- ***返回值***
  - 返回路径字符串。

### sendAndReceive

```csharp
public string sendAndReceive(string cmd)
```

- ***功能***
  - 发送仪表盘命令并接收响应。
- ***参数***
  - `cmd`：Dashboard 命令。
- ***返回值***
  - 返回响应文本。

## 资源释放

### Dispose

```csharp
public void Dispose()
```

- ***功能***
  - 释放客户端句柄。
- ***参数***
  - 无。
- ***返回值***
  - 无。
