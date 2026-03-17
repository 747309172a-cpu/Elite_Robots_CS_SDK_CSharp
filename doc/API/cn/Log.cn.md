# EliteLog 模块

## 简介

Log模块提供了Elite_Robots_CS_SDK中的日志功能相关设置，包括日志级别定义、日志处理器接口和日志输出功能。

## 导入

```csharp
using EliteRobots.CSharp;
```

## 日志级别

### LogLevel

```csharp
public enum LogLevel
```

- ***功能***
  - 定义日志等级。
- ***枚举值***
  - `ELI_DEBUG`、`ELI_INFO`、`ELI_WARN`、`ELI_ERROR`、`ELI_FATAL`、`ELI_NONE`。

## 接口

### registerLogHandler

```csharp
public static void registerLogHandler(Action<string, int, LogLevel, string> handler)
```

- ***功能***
  - 注册自定义的日志处理器。
- ***参数***
  - `handler`：回调参数依次为 `file`、`line`、`level`、`message`。
- ***返回值***
  - 无。

### unregisterLogHandler

```csharp
public static void unregisterLogHandler()
```

- ***功能***
  - 注销当前日志处理器，将启用默认日志处理器。
- ***参数***
  - 无。
- ***返回值***
  - 无。

### setLogLevel

```csharp
public static void setLogLevel(LogLevel level)
```

- ***功能***
  - 设置全局日志级别。
- ***参数***
  - `level`：日志级别。
- ***返回值***
  - 无。

### logDebugMessage

```csharp
public static void logDebugMessage(string file, int line, string msg)
```

- ***功能***
  - 输出 Debug 级别日志。
- ***参数***
  - `file`：文件名。
  - `line`：行号。
  - `msg`：日志消息。
- ***返回值***
  - 无。

### logInfoMessage

```csharp
public static void logInfoMessage(string file, int line, string msg)
```

- ***功能***
  - 输出 Info 级别日志。
- ***参数***
  - 同上。
- ***返回值***
  - 无。

### logWarnMessage

```csharp
public static void logWarnMessage(string file, int line, string msg)
```

- ***功能***
  - 输出 Warn 级别日志。
- ***参数***
  - 同上。
- ***返回值***
  - 无。

### logErrorMessage

```csharp
public static void logErrorMessage(string file, int line, string msg)
```

- ***功能***
  - 输出 Error 级别日志。
- ***参数***
  - 同上。
- ***返回值***
  - 无。

### logFatalMessage

```csharp
public static void logFatalMessage(string file, int line, string msg)
```

- ***功能***
  - 输出 Fatal 级别日志。
- ***参数***
  - 同上。
- ***返回值***
  - 无。

### logNoneMessage

```csharp
public static void logNoneMessage(string file, int line, string msg)
```

- ***功能***
  - 输出 None 级别日志。
- ***参数***
  - 同上。
- ***返回值***
  - 无。

## 使用示例

```csharp
using EliteRobots.CSharp;

EliteLog.registerLogHandler((file, line, level, msg) =>
{
    Console.WriteLine($"[{file}] : {line}: {level}: {msg}");
});

EliteLog.setLogLevel(LogLevel.ELI_DEBUG);
EliteLog.logDebugMessage("elite_log.cs", 1, "This is a debug message");
```
