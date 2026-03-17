# RobotException

## Introduction

When robot exceptions occur, SDK receives exception packets from Driver/Primary channels.

C# SDK provides two levels:

- Raw structures: `EliteDriverRobotException`, `PrimaryRobotException`
- Wrapped models: `RobotException`, `RobotError`, `RobotRuntimeException` and related enums

## Import

```csharp
using EliteRobots.CSharp;
```

## Wrapped Exception Model

```csharp
public enum RobotExceptionType
public enum RobotErrorType
public enum RobotErrorLevel
public enum RobotErrorDataType

public abstract class RobotException
public sealed class RobotDisconnectedException : RobotException
public sealed class RobotError : RobotException
public sealed class RobotRuntimeException : RobotException

public static class RobotExceptionMapper
{
    public static RobotException fromRaw(EliteDriverRobotException ex)
    public static RobotException fromRaw(PrimaryRobotException ex)
}
```

## Raw Structures

```csharp
public sealed class EliteDriverRobotException
public sealed class PrimaryRobotException
```

Both include fields like:
`Type`, `Timestamp`, `ErrorCode`, `SubErrorCode`, `ErrorSource`, `ErrorLevel`, `ErrorDataType`, `DataU32`, `DataI32`, `DataF32`, `Line`, `Column`, `Message`.

Helper conversion:

```csharp
public RobotException toRobotException()
```

## Callback Registration

```csharp
// raw callback
public void registerRobotExceptionCallback(Action<EliteDriverRobotException> cb)
public void registerRobotExceptionCallback(Action<PrimaryRobotException> callback)

// wrapped callback
public void registerWrappedRobotExceptionCallback(Action<RobotException> cb)
public void registerWrappedRobotExceptionCallback(Action<RobotException> callback)
```

## Usage Example

```csharp
using EliteRobots.CSharp;

driver.registerWrappedRobotExceptionCallback(ex =>
{
    switch (ex)
    {
        case RobotDisconnectedException disconnected:
            Console.WriteLine($"Disconnected at {disconnected.Timestamp}");
            break;
        case RobotRuntimeException runtime:
            Console.WriteLine($"Runtime error: line={runtime.Line}, col={runtime.Column}, msg={runtime.Message}");
            break;
        case RobotError err:
            Console.WriteLine($"RobotError code={err.ErrorCode}, source={err.ErrorSource}, level={err.ErrorLevel}, data={err.Data}");
            break;
        default:
            Console.WriteLine($"Unknown exception type: {ex.Type}");
            break;
    }
});
```

## EliteSdkException

```csharp
public sealed class EliteSdkException : Exception
{
    public int StatusCode { get; }
}
```
