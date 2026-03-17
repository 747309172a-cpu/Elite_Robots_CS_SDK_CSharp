# EliteLog Module

## Introduction

The Log module provides log level definition, log callback registration, and active logging output.

## Import

```csharp
using EliteRobots.CSharp;
```

## LogLevel

```csharp
public enum LogLevel
```

- `ELI_DEBUG`, `ELI_INFO`, `ELI_WARN`, `ELI_ERROR`, `ELI_FATAL`, `ELI_NONE`

## Interfaces

```csharp
public static void registerLogHandler(Action<string, int, LogLevel, string> handler)
public static void unregisterLogHandler()
public static void setLogLevel(LogLevel level)

public static void logDebugMessage(string file, int line, string msg)
public static void logInfoMessage(string file, int line, string msg)
public static void logWarnMessage(string file, int line, string msg)
public static void logErrorMessage(string file, int line, string msg)
public static void logFatalMessage(string file, int line, string msg)
public static void logNoneMessage(string file, int line, string msg)
```

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
