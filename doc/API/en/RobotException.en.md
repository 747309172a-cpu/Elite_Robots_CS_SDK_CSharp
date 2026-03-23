# RobotException

## Introduction

When an exception occurs during robot operation, the SDK receives exception packets from the Driver or Primary channel.

The current C# SDK provides two layers of capability at the same time:

- Raw structures: `EliteDriverRobotException`, `PrimaryRobotException`
- Wrapped exceptions: `RobotException`, `RobotError`, `RobotRuntimeException`, and related enums

## Import

```csharp
using EliteRobots.CSharp;
```

## Wrapped Exception Model (New)

### RobotExceptionType

```csharp
public enum RobotExceptionType
```

- ***Function***
  - Defines the main exception type.
- ***Enum Values***
  - `RobotDisconnected`
  - `RobotError`
  - `ScriptRuntime`
  - `Unknown`

### RobotErrorType

```csharp
public enum RobotErrorType
```

- ***Function***
  - Defines the source module of an error.
- ***Enum Values***
  - `Safety`, `Gui`, `Controller`, `Rtsi`, `Joint`, `Tool`, `Tp`, `JointFpga`, `ToolFpga`, `Unknown`

### RobotErrorLevel

```csharp
public enum RobotErrorLevel
```

- ***Function***
  - Defines the severity level of an error.
- ***Enum Values***
  - `Info`, `Warning`, `Error`, `Fatal`, `Unknown`

### RobotErrorDataType

```csharp
public enum RobotErrorDataType
```

- ***Function***
  - Defines the type of additional error data.
- ***Enum Values***
  - `None`, `Unsigned`, `Signed`, `Float`, `Hex`, `String`, `Joint`, `Unknown`

### RobotException (Base Class)

```csharp
public abstract class RobotException
{
    public RobotExceptionType Type { get; }
    public ulong Timestamp { get; }
}
```

- ***Function***
  - Represents common information for robot exceptions in a unified way.

### RobotDisconnectedException

```csharp
public sealed class RobotDisconnectedException : RobotException
```

- ***Function***
  - Represents a robot disconnection exception.

### RobotError

```csharp
public sealed class RobotError : RobotException
```

- ***Function***
  - Represents a robot error exception (hardware, controller, and so on).
- ***Main Properties***
  - `ErrorCode`, `SubErrorCode`
  - `ErrorSource` (`RobotErrorType`)
  - `ErrorLevel` (`RobotErrorLevel`)
  - `ErrorDataType` (`RobotErrorDataType`)
  - `DataU32`, `DataI32`, `DataF32`, `Message`
  - `Data` (object automatically mapped according to `ErrorDataType`)

### RobotRuntimeException

```csharp
public sealed class RobotRuntimeException : RobotException
```

- ***Function***
  - Represents a script runtime exception.
- ***Main Properties***
  - `Line`, `Column`, `Message`

### RobotExceptionMapper

```csharp
public static class RobotExceptionMapper
{
    public static RobotException fromRaw(EliteDriverRobotException ex)
    public static RobotException fromRaw(PrimaryRobotException ex)
}
```

- ***Function***
  - Maps raw exception structures to wrapped exception objects.

## Raw Exception Structures (Kept)

### EliteDriverRobotException

```csharp
public sealed class EliteDriverRobotException
```

- ***Function***
  - Raw exception structure for the Driver channel.
- ***Main Fields***
  - `Type`, `Timestamp`, `ErrorCode`, `SubErrorCode`, `ErrorSource`, `ErrorLevel`, `ErrorDataType`, `DataU32`, `DataI32`, `DataF32`, `Line`, `Column`, `Message`
- ***Helper Interface***

```csharp
public RobotException toRobotException()
```

### PrimaryRobotException

```csharp
public sealed class PrimaryRobotException
```

- ***Function***
  - Raw exception structure for the Primary channel.
- ***Fields***
  - Same as `EliteDriverRobotException`.
- ***Helper Interface***

```csharp
public RobotException toRobotException()
```

## Callback Registration

### Raw Callback

```csharp
// EliteDriver
public void registerRobotExceptionCallback(Action<EliteDriverRobotException> cb)

// PrimaryClientInterface
public void registerRobotExceptionCallback(Action<PrimaryRobotException> callback)
```

### Wrapped Callback (New)

```csharp
// EliteDriver
public void registerWrappedRobotExceptionCallback(Action<RobotException> cb)

// PrimaryClientInterface
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

### Type Definition

```csharp
public sealed class EliteSdkException : Exception
{
    public int StatusCode { get; }
}
```

- ***Function***
  - Wraps native call error codes and error messages.
- ***Trigger Scenario***
  - Thrown when an SDK interface call fails.
