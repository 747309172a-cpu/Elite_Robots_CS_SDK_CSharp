# EliteLog Module

## Introduction

The Log module provides log-related features in `Elite_Robots_CS_SDK`, including log level definitions, log handler interfaces, and log output functions.

## Import

```csharp
using EliteRobots.CSharp;
```

## Log Levels

### LogLevel

```csharp
public enum LogLevel
```

- ***Function***
  - Defines log levels.
- ***Enum Values***
  - `ELI_DEBUG`, `ELI_INFO`, `ELI_WARN`, `ELI_ERROR`, `ELI_FATAL`, `ELI_NONE`.

## Interfaces

### registerLogHandler

```csharp
public static void registerLogHandler(Action<string, int, LogLevel, string> handler)
```

- ***Function***
  - Register a custom log handler.
- ***Parameters***
  - `handler`: callback parameters are `file`, `line`, `level`, and `message`.
- ***Return Value***
  - None.

### unregisterLogHandler

```csharp
public static void unregisterLogHandler()
```

- ***Function***
  - Unregister the current log handler and enable the default log handler.
- ***Parameters***
  - None.
- ***Return Value***
  - None.

### setLogLevel

```csharp
public static void setLogLevel(LogLevel level)
```

- ***Function***
  - Set the global log level.
- ***Parameters***
  - `level`: log level.
- ***Return Value***
  - None.

### logDebugMessage

```csharp
public static void logDebugMessage(string file, int line, string msg)
```

- ***Function***
  - Output a Debug-level log message.
- ***Parameters***
  - `file`: file name.
  - `line`: line number.
  - `msg`: log message.
- ***Return Value***
  - None.

### logInfoMessage

```csharp
public static void logInfoMessage(string file, int line, string msg)
```

- ***Function***
  - Output an Info-level log message.
- ***Parameters***
  - Same as above.
- ***Return Value***
  - None.

### logWarnMessage

```csharp
public static void logWarnMessage(string file, int line, string msg)
```

- ***Function***
  - Output a Warn-level log message.
- ***Parameters***
  - Same as above.
- ***Return Value***
  - None.

### logErrorMessage

```csharp
public static void logErrorMessage(string file, int line, string msg)
```

- ***Function***
  - Output an Error-level log message.
- ***Parameters***
  - Same as above.
- ***Return Value***
  - None.

### logFatalMessage

```csharp
public static void logFatalMessage(string file, int line, string msg)
```

- ***Function***
  - Output a Fatal-level log message.
- ***Parameters***
  - Same as above.
- ***Return Value***
  - None.

### logNoneMessage

```csharp
public static void logNoneMessage(string file, int line, string msg)
```

- ***Function***
  - Output a None-level log message.
- ***Parameters***
  - Same as above.
- ***Return Value***
  - None.

## Usage Example

```csharp
using EliteRobots.CSharp;

EliteLog.registerLogHandler((file, line, level, msg) =>
{
    Console.WriteLine($"[{file}] : {line}: {level}: {msg}");
});

EliteLog.setLogLevel(LogLevel.ELI_DEBUG);
EliteLog.logDebugMessage("elite_log.cs", 1, "This is a debug message");
```
