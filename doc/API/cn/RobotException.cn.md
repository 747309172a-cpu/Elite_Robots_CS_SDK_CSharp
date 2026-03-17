# RobotException

## 简介

机器人运行中出现异常时，SDK 会从 Driver/Primary 通道收到异常报文。

当前 C# SDK 同时提供两层能力：

- 原始结构：`EliteDriverRobotException`、`PrimaryRobotException`
- 高级包装：`RobotException`、`RobotError`、`RobotRuntimeException` 及相关枚举

## 导入

```csharp
using EliteRobots.CSharp;
```

## 高级异常模型（新增）

### RobotExceptionType

```csharp
public enum RobotExceptionType
```

- ***功能***
  - 定义异常主类型。
- ***枚举值***
  - `RobotDisconnected`
  - `RobotError`
  - `ScriptRuntime`
  - `Unknown`

### RobotErrorType

```csharp
public enum RobotErrorType
```

- ***功能***
  - 定义错误来源模块。
- ***枚举值***
  - `Safety`、`Gui`、`Controller`、`Rtsi`、`Joint`、`Tool`、`Tp`、`JointFpga`、`ToolFpga`、`Unknown`

### RobotErrorLevel

```csharp
public enum RobotErrorLevel
```

- ***功能***
  - 定义错误等级。
- ***枚举值***
  - `Info`、`Warning`、`Error`、`Fatal`、`Unknown`

### RobotErrorDataType

```csharp
public enum RobotErrorDataType
```

- ***功能***
  - 定义错误附加数据类型。
- ***枚举值***
  - `None`、`Unsigned`、`Signed`、`Float`、`Hex`、`String`、`Joint`、`Unknown`

### RobotException（基类）

```csharp
public abstract class RobotException
{
    public RobotExceptionType Type { get; }
    public ulong Timestamp { get; }
}
```

- ***功能***
  - 统一表示机器人异常公共信息。

### RobotDisconnectedException

```csharp
public sealed class RobotDisconnectedException : RobotException
```

- ***功能***
  - 表示机器人断连异常。

### RobotError

```csharp
public sealed class RobotError : RobotException
```

- ***功能***
  - 表示机器人错误异常（硬件/控制器等）。
- ***主要属性***
  - `ErrorCode`、`SubErrorCode`
  - `ErrorSource`（`RobotErrorType`）
  - `ErrorLevel`（`RobotErrorLevel`）
  - `ErrorDataType`（`RobotErrorDataType`）
  - `DataU32`、`DataI32`、`DataF32`、`Message`
  - `Data`（按 `ErrorDataType` 自动映射后的对象）

### RobotRuntimeException

```csharp
public sealed class RobotRuntimeException : RobotException
```

- ***功能***
  - 表示脚本运行时异常。
- ***主要属性***
  - `Line`、`Column`、`Message`

### RobotExceptionMapper

```csharp
public static class RobotExceptionMapper
{
    public static RobotException fromRaw(EliteDriverRobotException ex)
    public static RobotException fromRaw(PrimaryRobotException ex)
}
```

- ***功能***
  - 将原始异常结构映射为高级异常对象。

## 原始异常结构（保留）

### EliteDriverRobotException

```csharp
public sealed class EliteDriverRobotException
```

- ***功能***
  - Driver 通道原始异常结构。
- ***主要字段***
  - `Type`、`Timestamp`、`ErrorCode`、`SubErrorCode`、`ErrorSource`、`ErrorLevel`、`ErrorDataType`、`DataU32`、`DataI32`、`DataF32`、`Line`、`Column`、`Message`
- ***辅助接口***

```csharp
public RobotException toRobotException()
```

### PrimaryRobotException

```csharp
public sealed class PrimaryRobotException
```

- ***功能***
  - Primary 通道原始异常结构。
- ***字段***
  - 与 `EliteDriverRobotException` 一致。
- ***辅助接口***

```csharp
public RobotException toRobotException()
```

## 回调注册方式

### 原始结构回调

```csharp
// EliteDriver
public void registerRobotExceptionCallback(Action<EliteDriverRobotException> cb)

// PrimaryClientInterface
public void registerRobotExceptionCallback(Action<PrimaryRobotException> callback)
```

### 高级包装回调（新增）

```csharp
// EliteDriver
public void registerWrappedRobotExceptionCallback(Action<RobotException> cb)

// PrimaryClientInterface
public void registerWrappedRobotExceptionCallback(Action<RobotException> callback)
```

## 使用示例

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

### 类型定义

```csharp
public sealed class EliteSdkException : Exception
{
    public int StatusCode { get; }
}
```

- ***功能***
  - 封装 native 调用错误码和错误消息。
- ***触发场景***
  - SDK 接口调用失败时抛出。
