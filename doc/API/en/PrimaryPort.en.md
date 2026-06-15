# PrimaryClientInterface Class

## Introduction

`PrimaryClientInterface` is used to communicate with the robot Primary port, and supports sending scripts, reading kinematic parameters, and exception callbacks.

## Import

```csharp
using EliteRobots.CSharp;
```

## Constructor

### PrimaryClientInterface

```csharp
public PrimaryClientInterface()
```

- ***Function***
  - Create a Primary client instance.
- ***Parameters***
  - None.
- ***Return Value***
  - None.

## Communication Interface

### connect

```csharp
public bool connect(string ip, int port = 30001)
```

- ***Function***
  - Connect to the robot port `30001` by default.
- ***Parameters***
  - `ip`: robot IP address.
  - `port`: port number, default `30001`.
- ***Return Value***
  - Returns `true` if the connection succeeds, otherwise `false`.

### disconnect

```csharp
public void disconnect()
```

- ***Function***
  - Disconnect.
- ***Parameters***
  - None.
- ***Return Value***
  - None.

### getLocalIP

```csharp
public string getLocalIP()
```

- ***Function***
  - Get the locally bound IP.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns the IP string, or possibly an empty string if unavailable.

## Script and Data

### sendScript

```csharp
public bool sendScript(string script)
```

- ***Function***
  - Send script text to the robot.
- ***Parameters***
  - `script`: script content.
- ***Return Value***
  - Returns `true` if sending succeeds, otherwise `false`.

### powerOn

```csharp
public bool powerOn()
```

- ***Function***
  - Power on the robot through the Primary port.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns `true` if the command succeeds, otherwise `false`.

### powerOff

```csharp
public bool powerOff()
```

- ***Function***
  - Power off the robot through the Primary port.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns `true` if the command succeeds, otherwise `false`.

### brakeRelease

```csharp
public bool brakeRelease()
```

- ***Function***
  - Release the robot brakes through the Primary port.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns `true` if the command succeeds, otherwise `false`.

### pauseProgram

```csharp
public bool pauseProgram()
```

- ***Function***
  - Pause the running task through the Primary port.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns `true` if the command succeeds, otherwise `false`.

### stopProgram

```csharp
public bool stopProgram()
```

- ***Function***
  - Stop the current task through the Primary port.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns `true` if the command succeeds, otherwise `false`.

### unlockProtectiveStop

```csharp
public bool unlockProtectiveStop()
```

- ***Function***
  - Unlock protective stop through the Primary port.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns `true` if the command succeeds, otherwise `false`.

### safetySystemRestart

```csharp
public bool safetySystemRestart()
```

- ***Function***
  - Restart the safety system through the Primary port.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns `true` if the command succeeds, otherwise `false`.

### setSpeedScaling

```csharp
public bool setSpeedScaling(int scaling)
```

- ***Function***
  - Set the target speed scaling through the Primary port.
- ***Parameters***
  - `scaling`: target speed percentage.
- ***Return Value***
  - Returns `true` if the command succeeds, otherwise `false`.

### getPackage

```csharp
public bool getPackage(out KinematicsInfo info, int timeoutMs = 1000)
```

- ***Function***
  - Read a Primary kinematics data package.
- ***Parameters***
  - `info`: output parameter containing three 6-element arrays: `DhA`, `DhD`, and `DhAlpha`.
  - `timeoutMs`: read timeout in milliseconds, default `1000`.
- ***Return Value***
  - Returns `true` if the package is obtained successfully, otherwise `false`.

## Exception Callback

### registerRobotExceptionCallback

```csharp
public void registerRobotExceptionCallback(Action<PrimaryRobotException> callback)
```

- ***Function***
  - Register a raw exception callback function. Triggered when an exception packet is received from the Primary port.
- ***Parameters***
  - `callback`: exception callback whose argument type is `PrimaryRobotException`.
- ***Return Value***
  - None.

### registerWrappedRobotExceptionCallback

```csharp
public void registerWrappedRobotExceptionCallback(Action<RobotException> callback)
```

- ***Function***
  - Register a wrapped exception callback. The callback argument is mapped to `RobotDisconnectedException`, `RobotError`, or `RobotRuntimeException`.
- ***Parameters***
  - `callback`: wrapped exception callback function.
- ***Return Value***
  - None.

### registerRobotExceptionCallback

```csharp
public void registerRobotExceptionCallback(
    Action<RobotError> onRobotError,
    Action<RobotRuntimeException> onRuntimeException,
    Action<RobotDisconnectedException>? onDisconnected = null)
```

- ***Function***
  - Register exception callbacks dispatched by type.
- ***Parameters***
  - `onRobotError`: robot error callback.
  - `onRuntimeException`: script runtime exception callback.
  - `onDisconnected`: robot disconnection callback (optional).
- ***Return Value***
  - None.

### clearRobotExceptionCallback

```csharp
public void clearRobotExceptionCallback()
```

- ***Function***
  - Clear exception callbacks.
  - This also unregisters the callback from the native layer.
- ***Parameters***
  - None.
- ***Return Value***
  - None.

## Resource Disposal

### Dispose

```csharp
public void Dispose()
```

- ***Function***
  - Release client resources.
- ***Parameters***
  - None.
- ***Return Value***
  - None.

## Data Types

### KinematicsInfo

```csharp
public sealed class KinematicsInfo
{
    public double[] DhA { get; init; }
    public double[] DhD { get; init; }
    public double[] DhAlpha { get; init; }
}
```

- ***Function***
  - Stores kinematic parameters output from Primary.

### PrimaryRobotException

```csharp
public sealed class PrimaryRobotException
```

- ***Function***
  - Describes Primary exception data.
- ***Main Fields***
  - `Type`, `Timestamp`, `ErrorCode`, `SubErrorCode`, `ErrorSource`, `ErrorLevel`, `ErrorDataType`, `DataU32`, `DataI32`, `DataF32`, `Line`, `Column`, `Message`.
